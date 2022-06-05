using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class GasPump : InteractableLocation
{
    private LocationCamera StoreCamera;
    private IActivityPerformable Player;
    private IModItems ModItems;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private IWeapons Weapons;
    private ITimeControllable Time;
    private UIMenuItem completeTask;
    private Transaction Transaction;
    private Vector3 PropEntryPosition;
    private float PropEntryHeading;
    private bool IsCancelled;
    private string PlayingDict;
    private string PlayingAnim;
    private bool hasAttachedProp;
    private UIMenu GetGasSubMenu;

    private Rage.Object SellingProp;
    private VehicleExt VehicleToFill;
    private UIMenuItem FillMenuItem;
    private UIMenuNumericScrollerItem<int> AddSomeMenuItem;
    private int pricePerUnit;
    private float PercentFuelNeeded;
    private int VehicleToFillFuelTankCapacity;
    private int UnitsOfFuelNeeded;
    private float PercentFilledPerUnit;
    private float AmountToFill;
    private string VehicleToFillMakeName;
    private string VehicleToFillModelName;
    private string VehicleToFillClassName;
    private string VehicleToFillName;
    private GasStation AssociatedStation;
    private bool KeepInteractionGoing;
    private bool IsFueling;

    private enum eSetPlayerControlFlag
    {
        SPC_AMBIENT_SCRIPT = (1 << 1),
        SPC_CLEAR_TASKS = (1 << 2),
        SPC_REMOVE_FIRES = (1 << 3),
        SPC_REMOVE_EXPLOSIONS = (1 << 4),
        SPC_REMOVE_PROJECTILES = (1 << 5),
        SPC_DEACTIVATE_GADGETS = (1 << 6),
        SPC_REENABLE_CONTROL_ON_DEATH = (1 << 7),
        SPC_LEAVE_CAMERA_CONTROL_ON = (1 << 8),
        SPC_ALLOW_PLAYER_DAMAGE = (1 << 9),
        SPC_DONT_STOP_OTHER_CARS_AROUND_PLAYER = (1 << 10),
        SPC_PREVENT_EVERYBODY_BACKOFF = (1 << 11),
        SPC_ALLOW_PAD_SHAKE = (1 << 12)
    };

    public GasPump() : base()
    {

    }
    [XmlIgnore]
    public Rage.Object PumpProp { get; set; }
    public override bool ShowsOnDirectory { get; set; } = false;
    public override string TypeName { get; set; } = "Gas Pump";
    public override int MapIcon { get; set; } = (int)BlipSprite.PointOfInterest;
    public override Color MapIconColor { get; set; } = Color.White;
    public override float MapIconScale { get; set; } = 0.5f;
    public override string ButtonPromptText { get; set; }
    public GasPump(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID, Rage.Object machineProp, GasStation gasStation) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = menuID;
        PumpProp = machineProp;
        AssociatedStation = gasStation;
        ButtonPromptText = $"Get Gas at {Name}";
    }
    public override void OnInteract(ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        Player = player;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;

        if (CanInteract)
        {
            Player.IsInteractingWithLocation = true;
            CanInteract = false;
            Player.IsTransacting = true;
            GameFiber.StartNew(delegate
            {

              // NativeFunction.Natives.SET_GAMEPLAY_COORD_HINT(EntrancePosition.X, EntrancePosition.Y, EntrancePosition.Z, -1, 2000, 2000);
                GetPropEntry();
                if (!MoveToMachine())
                {
                    EntryPoint.WriteToConsole("Transaction: TOP LEVE DISPOSE AFTER NO MOVE FUCKER", 5);
                    FullDispose();
                }

                CreateInteractionMenu();
                InteractionMenu.Visible = true;
                InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;

                GenerateGasMenu();

                while (IsAnyMenuVisible || KeepInteractionGoing || IsFueling)
                {
                    MenuPool.ProcessMenus();
                    GameFiber.Yield();
                }

                EntryPoint.WriteToConsole("Gas Pump DIspose                                                     1111111111");

                DisposeInteractionMenu();

                FullDispose();


                Player.IsInteractingWithLocation = false;
                Player.IsTransacting = false;
                CanInteract = true;
            }, "Gas Station Interact");
        }
    }

    private void GenerateGasMenu()
    {

        VehicleToFill = World.Vehicles.GetClosestVehicleExt(EntrancePosition, true, 6f);
        if (VehicleToFill != null && VehicleToFill.Vehicle.Exists() && !VehicleToFill.Vehicle.IsEngineOn && VehicleToFill.Vehicle.FuelLevel < 100f)
        {

            GetVehicleData();



            string MenuString = $"~n~Price Per Gallon: ~r~${pricePerUnit}~s~~n~Fuel Capacity: ~y~{VehicleToFillFuelTankCapacity}~s~ Gallons~n~Fuel Needed: ~p~{UnitsOfFuelNeeded}~s~ Gallons";

            GetGasSubMenu = MenuPool.AddSubMenu(InteractionMenu, $"Gas Up {VehicleToFillName}");

            InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = $"Gas Up Your Vehicle";
            InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Car;

            if (HasBannerImage)
            {
                BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
                GetGasSubMenu.SetBannerType(BannerImage);
            }
            GetGasSubMenu.OnItemSelect += InteractionMenu_OnItemSelect;
            GetGasSubMenu.OnMenuOpen += GetGasSubMenu_OnMenuOpen;
            GetGasSubMenu.OnMenuClose += GetGasSubMenu_OnMenuClose;
            FillMenuItem = new UIMenuItem("Fill", "Fill the entire tank" + MenuString) { RightLabel = UnitsOfFuelNeeded + " Gallons - " +  AmountToFill.ToString("C0") };
            if (UnitsOfFuelNeeded > 1)
            {
                AddSomeMenuItem = new UIMenuNumericScrollerItem<int>("Partial", "Add gasoline by the gallon" + MenuString, 1, UnitsOfFuelNeeded, 1) { Formatter = v => v + " Gallons - " + (v * pricePerUnit).ToString("C0") };
                GetGasSubMenu.AddItem(AddSomeMenuItem);
            }
            GetGasSubMenu.AddItem(FillMenuItem);            
        }
        else
        {
            InteractionMenu.Visible = false;
        }


     
    }

    private void GetVehicleData()
    {
        VehicleToFillMakeName = NativeHelper.VehicleMakeName(VehicleToFill.Vehicle.Model.Hash);
        VehicleToFillModelName = NativeHelper.VehicleModelName(VehicleToFill.Vehicle.Model.Hash);
        VehicleToFillClassName = NativeHelper.VehicleClassName(VehicleToFill.Vehicle.Model.Hash);
        VehicleToFillName = (VehicleToFillMakeName + " " + VehicleToFillModelName).Trim();
        string CarDescription = "";
        if (VehicleToFillMakeName != "")
        {
            CarDescription += $"~n~Manufacturer: ~b~{VehicleToFillMakeName}~s~";
        }
        if (VehicleToFillModelName != "")
        {
            CarDescription += $"~n~Model: ~g~{VehicleToFillModelName}~s~";
        }
        if (VehicleToFillClassName != "")
        {
            CarDescription += $"~n~Class: ~p~{VehicleToFillClassName}~s~";
        }
        if(AssociatedStation != null)
        {
            pricePerUnit = AssociatedStation.PricePerGallon;
        }
        else
        {
            pricePerUnit = 3;
        }
        
        PercentFuelNeeded = (100f - VehicleToFill.Vehicle.FuelLevel)/100f;

        VehicleToFillFuelTankCapacity = VehicleToFill.FuelTankCapacity(VehicleToFillClassName);
        UnitsOfFuelNeeded = (int)Math.Ceiling(PercentFuelNeeded * VehicleToFillFuelTankCapacity);

        if (VehicleToFillFuelTankCapacity == 0)
        {
            PercentFilledPerUnit = 0;
        }
        else
        {
            PercentFilledPerUnit = 100f / VehicleToFillFuelTankCapacity;
        }

        AmountToFill = UnitsOfFuelNeeded * pricePerUnit;

    }
    private void GetGasSubMenu_OnMenuOpen(UIMenu sender)
    {
        if(VehicleToFill != null && VehicleToFill.Vehicle.Exists())
        {
            EntryPoint.WriteToConsole("Gas Sub Menu Opened");
            //NativeFunction.Natives.STOP_GAMEPLAY_HINT(false);
            //GameFiber.Sleep(2000);
            // NativeFunction.Natives.SET_GAMEPLAY_COORD_HINT(VehicleToFill.Vehicle.Position.X, VehicleToFill.Vehicle.Position.Y, VehicleToFill.Vehicle.Position.Z, -1, 2000, 2000);
            if (VehicleToFill != null && VehicleToFill.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_GAMEPLAY_VEHICLE_HINT(VehicleToFill.Vehicle, 0f, 0f, 0f, true, -1, 2000, 2000);
            }
        }
    }
    private void GetGasSubMenu_OnMenuClose(UIMenu sender)
    {
        EntryPoint.WriteToConsole("Gas Sub Menu Closed");
        //NativeFunction.Natives.STOP_GAMEPLAY_HINT(true);
        //NativeFunction.Natives.SET_GAMEPLAY_COORD_HINT(EntrancePosition.X, EntrancePosition.Y, EntrancePosition.Z, -1, 2000, 2000);
    }
    public override void OnItemPurchased(ModItem modItem, MenuItem menuItem, int totalItems)
    {
        StartMachineBuyAnimation(modItem, false);
        base.OnItemPurchased(modItem, menuItem, totalItems);
    }
    private void InteractionMenu_OnItemSelect(RAGENativeUI.UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if(selectedItem == FillMenuItem && VehicleToFill != null && VehicleToFill.Vehicle.Exists())
        {
            KeepInteractionGoing = true;
            InteractionMenu.Visible = false;
            sender.Visible = false;
            FuelVehicle(UnitsOfFuelNeeded);
            //KeepInteractionGoing = false;
        }
        else if (selectedItem == AddSomeMenuItem && VehicleToFill != null && VehicleToFill.Vehicle.Exists())
        {
            KeepInteractionGoing = true;
            InteractionMenu.Visible = false;
            sender.Visible = false;
            FuelVehicle(AddSomeMenuItem.Value);
            //KeepInteractionGoing = false;
        }
    }

    private void DisableControl()
    {

        Game.LocalPlayer.HasControl = false;
        NativeFunction.Natives.SET_PLAYER_CONTROL(Game.LocalPlayer, (int)eSetPlayerControlFlag.SPC_LEAVE_CAMERA_CONTROL_ON, false);
        Game.DisableControlAction(0, GameControl.LookLeftRight, true);
        Game.DisableControlAction(0, GameControl.LookUpDown, true);

        Game.DisableControlAction(0, GameControl.LookUpDown, true);


    }
    private void EnableControl()
    {
        Game.DisableControlAction(0, GameControl.LookLeftRight, false);
        Game.DisableControlAction(0, GameControl.LookUpDown, false);
        Game.LocalPlayer.HasControl = true;
        NativeFunction.Natives.SET_PLAYER_CONTROL(Game.LocalPlayer, (int)eSetPlayerControlFlag.SPC_LEAVE_CAMERA_CONTROL_ON, true);
    }



    private void FuelVehicle(int UnitsToAdd)
    {
        if(UnitsToAdd * pricePerUnit > Player.Money)
        {
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~r~Purchase Failed", "We are sorry, we are unable to complete this transation. Please make sure you have the funds.");
            return;
        }
        Player.ButtonPrompts.AddPrompt("Fueling", "Cancel Fueling", "CancelFueling", Settings.SettingsManager.KeySettings.InteractCancel, 99);
        KeepInteractionGoing = true;
        int UnitsAdded = 0;
        GameFiber FastForwardWatcher = GameFiber.StartNew(delegate
        {
            IsFueling = true;
           // DisableControl();
            uint GameTimeBetweenUnits = 2500;  
            uint GameTimeAddedUnit = Game.GameTime;
            int dotsAdded = 0;
            while (UnitsAdded < UnitsToAdd && VehicleToFill.Vehicle.Exists() && !VehicleToFill.Vehicle.IsEngineOn)
            {
                string tabs = new string('.', dotsAdded);

                Game.DisplayHelp($"Fueling Progress {UnitsAdded}/{UnitsToAdd}");
                Game.LocalPlayer.HasControl = false;
                if (Game.GameTime - GameTimeAddedUnit >= GameTimeBetweenUnits)
                {
                    UnitsAdded++;
                    GameTimeAddedUnit = Game.GameTime;


                    if (VehicleToFill.Vehicle.FuelLevel + PercentFilledPerUnit > 100f)
                    {
                        VehicleToFill.Vehicle.FuelLevel = 100f;
                    }
                    else
                    {
                        VehicleToFill.Vehicle.FuelLevel += PercentFilledPerUnit;
                    }
                    Player.GiveMoney(-1 * pricePerUnit);

                    int TextSound = NativeFunction.Natives.GET_SOUND_ID<int>();
                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(TextSound, "PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET", 0);

                    EntryPoint.WriteToConsole($"Gas pump added unit of gas Percent Added {PercentFilledPerUnit} Money Subtracted {-1 * pricePerUnit}");
                }
                if (Player.ButtonPrompts.IsPressed("CancelFueling"))
                {
                    break;
                }
                GameFiber.Yield();
            }


            if (UnitsAdded > 0)
            {
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~g~Purchased", $"Thank you for purchasing {UnitsAdded} gallons (${UnitsAdded * pricePerUnit}) of fuel at {Name}");
            }
            Game.LocalPlayer.HasControl = true;
            //EnableControl();
            KeepInteractionGoing = false;
            Player.ButtonPrompts.RemovePrompts("Fueling");
            IsFueling = false;

        }, "FastForwardWatcher");

    }
    private void FullDispose()
    {
        Dispose();
        NativeFunction.Natives.STOP_GAMEPLAY_HINT(false);
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
    }
    private void GetPropEntry()
    {
        if (PumpProp != null && PumpProp.Exists())
        {
            float DistanceToFront = Player.Position.DistanceTo2D(PumpProp.GetOffsetPositionFront(-1f));
            float DistanceToRear = Player.Position.DistanceTo2D(PumpProp.GetOffsetPositionFront(1f));
            if (DistanceToFront <= DistanceToRear)
            {
                EntryPoint.WriteToConsole("Gas Pump You are Closer to the FRONT, using that side");
                PropEntryPosition = PumpProp.GetOffsetPositionFront(-1f);
                PropEntryPosition = new Vector3(PropEntryPosition.X, PropEntryPosition.Y, Game.LocalPlayer.Character.Position.Z);
                float ObjectHeading = PumpProp.Heading - 180f;
                if (ObjectHeading >= 180f)
                {
                    PropEntryHeading = ObjectHeading - 180f;
                }
                else
                {
                    PropEntryHeading = ObjectHeading + 180f;
                }
            }
            else
            {
                EntryPoint.WriteToConsole("Gas Pump You are Closer to the REAR, using that side");
                PropEntryPosition = PumpProp.GetOffsetPositionFront(1f);
                PropEntryPosition = new Vector3(PropEntryPosition.X, PropEntryPosition.Y, Game.LocalPlayer.Character.Position.Z);
                float ObjectHeading = PumpProp.Heading;
                if (ObjectHeading >= 180f)
                {
                    PropEntryHeading = ObjectHeading - 180f;
                }
                else
                {
                    PropEntryHeading = ObjectHeading + 180f;
                }
            }
        }
    }
    private bool MoveToMachine()
    {
        if (PropEntryPosition == Vector3.Zero)
        {
            return false;
        }


        NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Game.LocalPlayer.Character, PropEntryPosition.X, PropEntryPosition.Y, PropEntryPosition.Z, 1.0f, -1, PropEntryHeading, 0.2f);
        uint GameTimeStartedSitting = Game.GameTime;
        float heading = Game.LocalPlayer.Character.Heading;
        bool IsFacingDirection = false;
        bool IsCloseEnough = false;
        while (Game.GameTime - GameTimeStartedSitting <= 5000 && !IsCloseEnough && !IsCancelled)
        {
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
            }
            IsCloseEnough = Game.LocalPlayer.Character.DistanceTo2D(PropEntryPosition) < 0.2f;
            GameFiber.Yield();
        }
        GameFiber.Sleep(250);
        GameTimeStartedSitting = Game.GameTime;
        while (Game.GameTime - GameTimeStartedSitting <= 5000 && !IsFacingDirection && !IsCancelled)
        {
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
            }
            heading = Game.LocalPlayer.Character.Heading;
            if (Math.Abs(ExtensionsMethods.Extensions.GetHeadingDifference(heading, PropEntryHeading)) <= 0.5f)//0.5f)
            {
                IsFacingDirection = true;
                EntryPoint.WriteToConsole($"Moving to Machine FACING TRUE {Game.LocalPlayer.Character.DistanceTo(PropEntryPosition)} {ExtensionsMethods.Extensions.GetHeadingDifference(heading, PropEntryHeading)} {heading} {PropEntryHeading}", 5);
            }
            GameFiber.Yield();
        }
        GameFiber.Sleep(250);
        if (IsCloseEnough && IsFacingDirection && !IsCancelled)
        {
            EntryPoint.WriteToConsole($"Moving to Machine IN POSITION {Game.LocalPlayer.Character.DistanceTo(PropEntryPosition)} {ExtensionsMethods.Extensions.GetHeadingDifference(heading, PropEntryHeading)} {heading} {PropEntryHeading}", 5);
            return true;
        }
        else
        {
            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            return false;
        }
    }
    private void StartMachineBuyAnimation(ModItem item, bool isIllicit)
    {

        if (MoveToMachine())
        {
            //if (UseMachine(item))
            //{

            //}
        }
        else
        {
            FullDispose();
        }
    }
    private bool UseMachine(ModItem item)
    {
        string modelName = "";
        bool HasProp = false;
        if (item.PackageItem != null && item.PackageItem.ModelName != "")
        {
            modelName = item.PackageItem.ModelName;
            HasProp = true;
        }
        else if (item.ModelItem != null && item.ModelItem.ModelName != "")
        {
            modelName = item.ModelItem.ModelName;
            HasProp = true;
        }



        PlayingDict = "mini@sprunk";
        PlayingAnim = "plyr_buy_drink_pt1";
        AnimationDictionary.RequestAnimationDictionay(PlayingDict);

        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 2.0f, -4.0f, -1, 0, 0, false, false, false);//-1
        EntryPoint.WriteToConsole($"Vending Activity Playing {PlayingDict} {PlayingAnim}", 5);
        bool IsCompleted = false;
        while (Player.CanPerformActivities && !IsCancelled)
        {
            Player.SetUnarmed();
            float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim);
            if (AnimationTime >= 0.5f)
            {
                if (HasProp && modelName != "" && !hasAttachedProp)
                {
                    SellingProp = new Rage.Object(modelName, Player.Character.GetOffsetPositionUp(50f));
                    GameFiber.Yield();
                    if (SellingProp.Exists())
                    {
                        SellingProp.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, item.ModelItem.AttachBoneIndex), item.ModelItem.AttachOffset, item.ModelItem.AttachRotation);
                    }
                    hasAttachedProp = true;
                }
            }
            if (AnimationTime >= 0.7f)
            {
                IsCompleted = true;
                break;
            }
            GameFiber.Yield();
        }
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        return IsCompleted;
    }
}

