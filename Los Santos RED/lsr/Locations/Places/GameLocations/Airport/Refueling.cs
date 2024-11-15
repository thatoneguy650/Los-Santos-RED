using LosSantosRED.lsr.Helper;
using LSR.Vehicles;
using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;
using System.Xml.Linq;

public class Refueling
{
    private GameLocation Shop;
    private ILocationInteractable Player;
    private ISettingsProvideable Settings;
    private int PricePerUnit;
    private Rage.Object PumpHandleProp;
    private GasPump ActiveGasPump;
    private bool SpawnedRope;

    private bool IsCancelled = false;
    private string Name;
    public Refueling(ILocationInteractable player, string name, int pricePerUnit, VehicleExt vehicleExt, ISettingsProvideable settings, GameLocation shop)
    {
        Player = player;
        PricePerUnit = pricePerUnit;
        Name = name;
        VehicleExt = vehicleExt;
        Settings = settings;
        Shop = shop;
    }
    public int UnitsAdded = 0;
    private string PlayingDict;
    private string PlayingAnim;
    private int RopeID = 0;

    public int VehicleToFillFuelTankCapacity { get; private set; }
    public float PercentFuelNeeded { get; private set; }
    public int UnitsOfFuelNeeded { get; private set; }
    public float PercentFilledPerUnit { get; private set; }
    public float AmountToFill { get; private set; }
    public VehicleExt VehicleExt { get; private set; }
    public bool CanRefuel => VehicleExt != null && VehicleExt.Vehicle.Exists() && !VehicleExt.Vehicle.IsEngineOn && VehicleExt.Vehicle.FuelLevel < Settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMax && VehicleExt.RequiresFuel;
    public void Setup()
    {
        if(VehicleExt != null && VehicleExt.Vehicle.Exists())
        {
            VehicleToFillFuelTankCapacity = VehicleExt.FuelTankCapacity;
            GetFuelStatus();
        }
        else
        {
            IsCancelled = true;
        }
    }
    public void GetFuelStatus()
    {
        PercentFuelNeeded = (Settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMax - VehicleExt.Vehicle.FuelLevel) / Settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMax;
        UnitsOfFuelNeeded = (int)Math.Ceiling(PercentFuelNeeded * VehicleToFillFuelTankCapacity);
        if (VehicleToFillFuelTankCapacity == 0)
        {
            PercentFilledPerUnit = 0;
        }
        else
        {
            PercentFilledPerUnit = Settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMax / VehicleToFillFuelTankCapacity;
        }
        AmountToFill = UnitsOfFuelNeeded * PricePerUnit;
    }
    public bool RefuelSlow(int UnitsToAdd, GasPump gasPump)
    {
        ActiveGasPump = gasPump;
        if (UnitsToAdd * PricePerUnit > Player.BankAccounts.GetMoney(true))
        {
            PurchaseFailed();
            return false;
        }
        else
        {
            Player.ButtonPrompts.AddPrompt("Fueling", "Cancel Fueling", "CancelFueling", Settings.SettingsManager.KeySettings.InteractCancel, 99);
            EntryPoint.WriteToConsole("Refuel Slow Started");
            int UnitsAdded = 0;
            GameFiber FastForwardWatcher = GameFiber.StartNew(delegate
            {
                try
                {
                    if(Settings.SettingsManager.VehicleSettings.FuelUsesAnimationsAndProps)
                    {
                        if (!DoRefuelingAnimation())
                        {
                            Cleanup();
                            return;
                        }
                    }
                    else
                    {
                        if (VehicleExt != null && VehicleExt.Vehicle.Exists())
                        {
                            unsafe
                            {
                                int lol = 0;
                                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, VehicleExt.Vehicle, 2000);
                                NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, VehicleExt.Vehicle, -1, 0, 2);
                                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
                                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                            }
                        }
                    }



                    uint GameTimeBetweenUnits = 1500;
                    uint GameTimeAddedUnit = Game.GameTime;
                    int dotsAdded = 0;

                    while (UnitsAdded < UnitsToAdd && VehicleExt.Vehicle.Exists() && !VehicleExt.Vehicle.IsEngineOn)
                    {
                        string tabs = new string('.', dotsAdded);
                        Game.DisplayHelp($"Fueling Progress {UnitsAdded}/{UnitsToAdd}");
                        NativeHelper.DisablePlayerMovementControl();
                        if (Game.GameTime - GameTimeAddedUnit >= GameTimeBetweenUnits)
                        {
                            UnitsAdded++;
                            GameTimeAddedUnit = Game.GameTime;
                            if (VehicleExt.Vehicle.FuelLevel + PercentFilledPerUnit > Settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMax)
                            {
                                VehicleExt.Vehicle.FuelLevel = Settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMax;
                            }
                            else
                            {
                                VehicleExt.Vehicle.FuelLevel += PercentFilledPerUnit;
                            }
                            Player.BankAccounts.GiveMoney(-1 * PricePerUnit, true);
                            //NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET", 0);
                            Shop.PlaySuccessSound();
                            //EntryPoint.WriteToConsoleTestLong($"Gas pump added unit of gas Percent Added {PercentFilledPerUnit} Money Subtracted {-1 * PricePerUnit}");
                        }
                        if (Player.ButtonPrompts.IsPressed("CancelFueling"))
                        {
                            break;
                        }
                        GameFiber.Yield();
                    }
                    EntryPoint.WriteToConsole("Refuel Slow LOOP ENDED");
                    if (UnitsAdded > 0)
                    {
                        PurchaseSucceeded(UnitsToAdd);
                    }
                    Cleanup();
                    //NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                    //NativeFunction.Natives.ENABLE_ALL_CONTROL_ACTIONS(0);
                    //Player.ButtonPrompts.RemovePrompts("Fueling");
                    //if(PumpHandleProp.Exists())
                    //{
                    //    PumpHandleProp.Delete();
                    //}

                    //if (gasPump != null)
                    //{
                    //    gasPump.IsFueling = false;
                    //}
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "FastForwardWatcher");
            return true;
        }
    }
    private void Cleanup()
    {
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        NativeFunction.Natives.ENABLE_ALL_CONTROL_ACTIONS(0);
        Player.ButtonPrompts.RemovePrompts("Fueling");

        if (RopeID > 0 && SpawnedRope)
        {
            //NativeFunction.Natives.DELETE_ROPE(out RopeID);

            unsafe
            {
                int lol = RopeID;
                NativeFunction.CallByName<bool>("DELETE_ROPE", &lol);
            }



        }





        if (PumpHandleProp.Exists())
        {
            PumpHandleProp.Delete();
        }
        if (ActiveGasPump != null)
        {
            ActiveGasPump.IsFueling = false;
        }
        //if(NativeFunction.Natives.DOES_ROPE_EXIST<bool>(RopeID))
        //{

        //}
    }
    private bool DoRefuelingAnimation()
    {
        if(!UseMachine())
        {
            return false;
        }
        if(!GrabPumpHandle())
        {
            return false;
        }
        if(!TurnPedToFaceVehicle())
        {
            return false;
        }
        if(!InsertPumpHandle())
        {
            return false;
        }
        return true;
    }
    private bool UseMachine()
    {
        PlayingDict = "anim_casino_a@amb@casino@games@insidetrack@ped_male@regular@02a@play@v02";// "anim @mp_atm@enter";
        PlayingAnim = "place_bet";// "enter";
        bool IsCompleted = false;
        AnimationWatcher aw = new AnimationWatcher();
        AnimationDictionary.RequestAnimationDictionay(PlayingDict);
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 4.0f, -4.0f, -1, (int)AnimationFlags.UpperBodyOnly | (int)AnimationFlags.StayInEndFrame, 0, false, false, false);//-1
        while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled)
        {
            Player.WeaponEquipment.SetUnarmed();
            float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim);
            if (AnimationTime >= 0.5f)
            {
                IsCompleted = true;
                break;
            }
            if (Player.ButtonPrompts.IsPressed("CancelFueling"))
            {
                break;
            }
            GameFiber.Yield();
        }
        return IsCompleted;
    }
    private bool GrabPumpHandle()
    {
        PlayingDict = "mp_common";
        PlayingAnim = "givetake1_b";
        bool IsCompleted = false;
        bool SpawnedPumpProp = false;
        AnimationWatcher aw = new AnimationWatcher();
        AnimationDictionary.RequestAnimationDictionay(PlayingDict);
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 4.0f, -4.0f, -1, (int)AnimationFlags.UpperBodyOnly | (int)AnimationFlags.StayInEndFrame, 0, false, false, false);//-1
        while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled)
        {
            Player.WeaponEquipment.SetUnarmed();
            float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim);
            if (AnimationTime >= 1.0f)
            {
                IsCompleted = true;
                break;
            }
            if(!SpawnedPumpProp && AnimationTime >= 0.3f)
            {
                SpawnPumpProp();
                SpawnedPumpProp = true;
            }
            if (Player.ButtonPrompts.IsPressed("CancelFueling"))
            {
                break;
            }
            GameFiber.Yield();
        }
        return IsCompleted;
    }
    private void AttachPumpHandleToVehicle()
    {
        if (!PumpHandleProp.Exists())
        {
            return;
        }
        if (!VehicleExt.Vehicle.Exists())
        {
            return;
        }
        string vehicleBoneName = "hub_lr";
        Vector3 VehicleOffset = new Vector3(-0.27f, -0.37f, 0.44f);// new Vector3(0.08f, 0.05f, 0f);
        Rotator VehicleRotation = new Rotator(-40f, 0f, -90f);// new Rotator(-30f, -80f, -120f);
        bool IsLeftSide = true;
        bool IsNormalCar = true;

        if (VehicleExt.Vehicle.HasBone("hub_lr") && VehicleExt.Vehicle.HasBone("hub_rr"))
        {
            if (Game.LocalPlayer.Character.DistanceTo2D(VehicleExt.Vehicle.GetBonePosition("hub_lr")) > Game.LocalPlayer.Character.DistanceTo2D(VehicleExt.Vehicle.GetBonePosition("hub_rr")))
            {
                IsLeftSide = false;
            }
        }
        if(VehicleExt.Vehicle.Model.Dimensions.Z >= 1.7f)
        {
            IsNormalCar = false;
        }
        EntryPoint.WriteToConsole($"IsLeftSide {IsLeftSide} IsNormalCar {IsNormalCar} X:{VehicleExt.Vehicle.Model.Dimensions.X} Y:{VehicleExt.Vehicle.Model.Dimensions.Y} Z:{VehicleExt.Vehicle.Model.Dimensions.Z}");

        if (VehicleExt.IsMotorcycle)
        {
            VehicleOffset = new Vector3(0.01f, 0.06999998f, 0.7699996f);// new Vector3(0f, 0.78f, 0.81f);
            VehicleRotation = new Rotator(-90f, 0f, 0f);// new Rotator(-80f, 0f, 0f);
            vehicleBoneName = "chassis";
            EntryPoint.WriteToConsole("MOTORCYCLE RAN");
        }
        else
        {
            if(IsLeftSide)
            {
                vehicleBoneName = "hub_lr";
                if (IsNormalCar)
                {
                    VehicleOffset = new Vector3(-0.27f, -0.37f, 0.44f);// new Vector3(0.08f, 0.05f, 0f);
                    VehicleRotation = new Rotator(-40f, 0f, -90f);// new Rotator(-30f, -80f, -120f);                  
                }
                else
                {
                    VehicleOffset = new Vector3(-0.3f, 0.51f, 0.69f);
                    VehicleRotation = new Rotator(-40f, 0f, -90f);
                }
            }
            else
            {
                vehicleBoneName = "hub_rr";
                if (IsNormalCar)
                {
                    VehicleOffset = new Vector3(0.31f, -0.2f, 0.45f);
                    VehicleRotation = new Rotator(-30f, 0f, 90f);
                }
                else
                {
                    VehicleOffset = new Vector3(0.32f, 0.54f, 0.63f);
                    VehicleRotation = new Rotator(-40f, 0f, 90f);
                }
            }
        }

        PumpHandleProp.AttachTo(VehicleExt.Vehicle, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", VehicleExt.Vehicle, vehicleBoneName), VehicleOffset, VehicleRotation);
    }
    private void SpawnPumpProp()
    {
        if (PumpHandleProp.Exists())
        {
            PumpHandleProp.Delete();
        }
        string HandBoneName = "BONETAG_R_PH_HAND";
        Vector3 HandOffset = new Vector3(0.08f, 0.05f, 0f);// new Vector3(0.04f, 0.03f, 0.03f);
        Rotator HandRotator = new Rotator(-30f, -80f, -120f);// new Rotator(20f, -90f, -140f);
        try
        {
            PumpHandleProp = new Rage.Object("prop_cs_fuel_nozle", Player.Character.GetOffsetPositionUp(50f));// new Rage.Object(modelName, Player.Character.GetOffsetPositionUp(50f));
        }
        catch (Exception ex)
        {

        }
        GameFiber.Yield();
        if (PumpHandleProp.Exists())
        {
            PumpHandleProp.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, HandBoneName), HandOffset, HandRotator);
        }
        else
        {
            return;
        }
        SpawnAndAttachRope();
    }
    private void SpawnAndAttachRope()
    {
        if (ActiveGasPump == null || !ActiveGasPump.PumpProp.Exists())
        {
            return;
        }
        NativeFunction.Natives.ROPE_LOAD_TEXTURES();
        uint GameTimeStarted = Game.GameTime;
        while (Game.GameTime - GameTimeStarted <= 500)
        {
            if (NativeFunction.Natives.ROPE_ARE_TEXTURES_LOADED<bool>())
            {
                break;
            }
            GameFiber.Yield();
        }
        RopeID = NativeFunction.Natives.ADD_ROPE<int>(
            ActiveGasPump.PumpProp.Position.X, ActiveGasPump.PumpProp.Position.Y, ActiveGasPump.PumpProp.Position.Z,//pos
            0f, 0f, 0f,//rot
            2.5f,//3f,//length
            1,//rope type
            15f,//1000f,//max length
            0.0f,//min length
            1.0f,//len change rate
            false,//ppuonbly
            false,//collision
            false,//lockfromfront
            1.0f,//time multiplier
            true//breakable
            );
        SpawnedRope = true;
        if (RopeID > 0)
        {
            NativeFunction.Natives.ACTIVATE_PHYSICS(RopeID);
        }
        GameFiber.Sleep(100);
        if(!PumpHandleProp.Exists() || ActiveGasPump == null || !ActiveGasPump.PumpProp.Exists())
        {
            return;
        }
        Vector3 attachPos = PumpHandleProp.GetOffsetPosition(new Vector3(0f, -0.032f, -0.194f)); //PumpHandleProp.GetOffsetPosition(new Vector3(0f, -0.033f, -0.195f));
        NativeFunction.Natives.ATTACH_ENTITIES_TO_ROPE(RopeID, 
            ActiveGasPump.PumpProp, 
            PumpHandleProp,
            ActiveGasPump.PumpProp.Position.X, ActiveGasPump.PumpProp.Position.Y + 0.5f, ActiveGasPump.PumpProp.Position.Z + 1.3f, //ActiveGasPump.PumpProp.Position.X, ActiveGasPump.PumpProp.Position.Y, ActiveGasPump.PumpProp.Position.Z + 1.45f,
            attachPos.X, attachPos.Y, attachPos.Z,
            6.0f,//5.0f,
            false,
            false,
            0,
            0
            );

    }
    private bool TurnPedToFaceVehicle()
    {
        if (!VehicleExt.Vehicle.Exists())
        {
            return false;

        }
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, VehicleExt.Vehicle, 2000);
            NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, VehicleExt.Vehicle, -1, 0, 2);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
        GameFiber.Sleep(2000);
        return true;
    }
    private bool InsertPumpHandle()
    {
        PlayingDict = "mp_common";
        PlayingAnim = "givetake1_b";
        bool IsCompleted = false;
        bool AttachedToVehicle = false;
        AnimationWatcher aw = new AnimationWatcher();
        AnimationDictionary.RequestAnimationDictionay(PlayingDict);
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 4.0f, -4.0f, -1, (int)AnimationFlags.UpperBodyOnly | (int)AnimationFlags.StayInEndFrame, 0, false, false, false);//-1
        while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled)
        {
            Player.WeaponEquipment.SetUnarmed();
            float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim);
            if (AnimationTime >= 1.0f)
            {
                IsCompleted = true;
                break;
            }
            if (!AttachedToVehicle && AnimationTime >= 0.3f)
            {
                AttachPumpHandleToVehicle();
                AttachedToVehicle = true;
            }
            if (Player.ButtonPrompts.IsPressed("CancelFueling"))
            {
                break;
            }
            GameFiber.Yield();
        }
        return IsCompleted;
    }
    public bool RefuelQuick(int UnitsToAdd)
    {
        if (UnitsToAdd * PricePerUnit > Player.BankAccounts.GetMoney(true))
        {
            PurchaseFailed();
            return false;
        }
        else
        {
            if (VehicleExt != null && VehicleExt.Vehicle.Exists())
            {
                VehicleExt.Vehicle.FuelLevel += PercentFilledPerUnit * UnitsToAdd;
                if (VehicleExt.Vehicle.FuelLevel >= Settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMax-0.01f)
                {
                    VehicleExt.Vehicle.FuelLevel = Settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMax;
                }
                Player.BankAccounts.GiveMoney(-1 * PricePerUnit * UnitsToAdd, true);
                if (UnitsToAdd > 0)
                {
                    PurchaseSucceeded(UnitsToAdd);
                }
            }
        }
        return true;
    }
    private void PurchaseFailed()
    {
        Shop.PlayErrorSound();
        Shop.DisplayMessage("~r~Purchase Failed", "We are sorry, we are unable to complete this transation. Please make sure you have the funds.");
    }
    private void PurchaseSucceeded(int UnitsToAdd)
    {
        Shop.PlaySuccessSound();
        Shop.DisplayMessage("~g~Purchased", $"Gallons: {UnitsToAdd}~n~Total price: ~r~${UnitsToAdd * PricePerUnit}~s~");
    }
    public void DisplayFuelingFailedReason()
    {
        string reason = GetFuelingFailedReason();
        if(string.IsNullOrEmpty(reason))
        {
            return;
        }
        Shop.PlayErrorSound();
        Shop.DisplayMessage("~r~Fueling Failed", reason);    
    }
    public string GetFuelingFailedReason()
    {
        if (VehicleExt == null || (VehicleExt != null && !VehicleExt.Vehicle.Exists()))
        {
            return $"No vehicle found to fuel";
        }
        else if (VehicleExt != null && VehicleExt.Vehicle.Exists() && !VehicleExt.RequiresFuel)
        {
            return $"Incompatible Fueling";
        }
        else if (VehicleExt != null && VehicleExt.Vehicle.Exists() && VehicleExt.Vehicle.IsEngineOn)
        {
            return $"Vehicle engine is still on";
        }
        else if (VehicleExt != null && VehicleExt.Vehicle.Exists() && !VehicleExt.Vehicle.IsEngineOn && VehicleExt.Vehicle.FuelLevel >= Settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMax)
        {
            return $"Vehicle fuel tank is already full";
        }
        return "";
    }
}

