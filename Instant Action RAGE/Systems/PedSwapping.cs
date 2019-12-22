using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal static class PedSwapping
{
    private static Random rnd;
    private static List<TakenOverPed> TakenOverPeds = new List<TakenOverPed>();
    private static Model OriginalModel;
    private static string LastModelHash;
    private static PedVariation myPedVariation;
    private static bool PedOriginallyHadHelmet = false;
    private static uint GameTimeLastTakenOver = Game.GameTime;
    static PedSwapping()
    {
        rnd = new Random();
    }
    public static void Initialize()
    {

    }
    public static void Dispose()
    {
       // ResetModel();
    }
    public static bool JustTakenOver(int Duration)
    {
        if (Game.GameTime - GameTimeLastTakenOver <= Duration)//Right when you takeover a ped they might become wanted for some weird reason, this stops that
            return true;
        else
            return false;
    }
    public static Ped GetPedestrian(float Radius, bool Nearest)
    {
        Ped PedToReturn = null;
        Ped[] closestPed = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, Radius, GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed | GetEntitiesFlags.ConsiderAllPeds).Where(x => x is Ped).ToArray(), (x => (Ped)x));
        if (Nearest)
            PedToReturn = closestPed.Where(s => s.CanTakeoverPed()).OrderBy(s => Vector3.Distance(Game.LocalPlayer.Character.Position, s.Position)).FirstOrDefault();
        else
            PedToReturn = closestPed.Where(s => s.CanTakeoverPed()).OrderBy(s => rnd.Next()).FirstOrDefault();
        if (PedToReturn == null)
            return null;
        else if (PedToReturn.IsInAnyVehicle(false))
        {
            if (PedToReturn.CurrentVehicle.Driver.Exists())
            {
                PedToReturn.CurrentVehicle.Driver.MakePersistent();
                return PedToReturn.CurrentVehicle.Driver;
            }
            else
            {
                PedToReturn.MakePersistent();
                return PedToReturn;
            }
        }
        else
        {
            PedToReturn.MakePersistent();
            return PedToReturn;
        }
    }
    private static void AllyPedsToPlayer(Ped[] PedList)
    {
        foreach (Ped PedToAlly in PedList)
        {
            NativeFunction.CallByName<bool>("SET_PED_AS_GROUP_MEMBER", PedToAlly, Game.LocalPlayer.Character.Group);
            PedToAlly.StaysInVehiclesWhenJacked = true;
        }
    }
    public static void TakeoverPed(Ped TargetPed, bool DeleteOld, bool ArrestOld)
    {
        try
        {
            if (TargetPed == null)
                return;


            if (Game.LocalPlayer.Character.IsDead)
            {
                Respawning.RespawnInPlace(false);
            }
            Vector3 PlayerOriginalPedPosition = Game.LocalPlayer.Character.Position;

            bool AlreadyTakenOver = false;

            if (TakenOverPeds.Count > 0)
            {
                uint TargetModelHash = TargetPed.Model.Hash;
                if (TargetModelHash == 225514697 || TargetModelHash == 2602752943 || TargetModelHash == 2608926626)
                {
                    AlreadyTakenOver = true;
                    ChangeModel(OriginalModel.Name);
                    if (!Game.LocalPlayer.Character.IsMainCharacter())
                        ReplacePedComponentVariation(Game.LocalPlayer.Character);
                }
            }

            OriginalModel = TargetPed.Model;
            TakenOverPed PedTakenOver = new TakenOverPed(TargetPed, TargetPed.Handle, GetPedVariation(TargetPed), TargetPed.Model, Game.GameTime);
            AddPedToTakenOverPeds(PedTakenOver);


            if (!AlreadyTakenOver)
                LastModelHash = TargetPed.Model.Name;

            bool wasInVehicle = TargetPed.IsInAnyVehicle(false);
            Vehicle carWasIn = null;
            if (wasInVehicle)
                carWasIn = TargetPed.CurrentVehicle;

            CopyPedComponentVariation(TargetPed);

            Ped CurrentPed = Game.LocalPlayer.Character;
            if (TargetPed.IsInAnyVehicle(false))
            {
                Game.LocalPlayer.Character.WarpIntoVehicle(carWasIn, -1);
                AllyPedsToPlayer(TargetPed.CurrentVehicle.Passengers);
            }
            else
            {
                AllyPedsToPlayer(Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 3f, GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed).Where(x => x is Ped).ToArray(), (x => (Ped)x)));
            }

            NativeFunction.CallByName<uint>("CHANGE_PLAYER_PED", Game.LocalPlayer, TargetPed, false, false);

            CurrentPed.IsPersistent = false;


            if (DeleteOld)
                CurrentPed.Delete();
            else if (ArrestOld)
                Surrendering.SetArrestedAnimation(CurrentPed, true);
            else
                AITakeoverPlayer(CurrentPed);

            NativeFunction.Natives.x2206BF9A37B7F724("MinigameTransitionOut", 5000, false);

            if (!AlreadyTakenOver)
            {
                SetPlayerOffset();
                ChangeModel(Settings.MainCharacterToAliasModelName);
                ChangeModel(LastModelHash);
            }


            if (!Game.LocalPlayer.Character.IsMainCharacter())
                ReplacePedComponentVariation(Game.LocalPlayer.Character);

            if (wasInVehicle)
            {
                Game.LocalPlayer.Character.WarpIntoVehicle(carWasIn, -1);
                NativeFunction.CallByName<bool>("SET_VEHICLE_HAS_BEEN_OWNED_BY_PLAYER", Game.LocalPlayer.Character.CurrentVehicle, true);
                InstantAction.OwnedCar = carWasIn;
            }
            else
            {
                Game.LocalPlayer.Character.IsCollisionEnabled = true;
            }

            if (Settings.PedTakeoverSetRandomMoney)
                Game.LocalPlayer.Character.SetCash(rnd.Next(Settings.PedTakeoverRandomMoneyMin, Settings.PedTakeoverRandomMoneyMax));

            Game.LocalPlayer.Character.Inventory.Weapons.Clear();
            Game.LocalPlayer.Character.Inventory.GiveNewWeapon(2725352035, 0, true);
            InstantAction.TimesDied = 0;
            InstantAction.MaxWantedLastLife = 0;
            Game.TimeScale = 1f;
            Police.SetWantedLevel(0,"Reset After Takeover as a precaution");
            NativeFunction.Natives.xB4EDDC19532BFB85();
            Game.HandleRespawn();
            NativeFunction.CallByName<bool>("NETWORK_REQUEST_CONTROL_OF_ENTITY", Game.LocalPlayer.Character);
            NativeFunction.Natives.xC0AA53F866B3134D();
            Police.RemoveWantedBlips();
            InstantAction.IsDead = false;
            InstantAction.IsBusted = false;
            Police.CurrentPoliceState = Police.PoliceState.Normal;
            InstantAction.BeingArrested = false;
            TrafficViolations.ResetTrafficViolations();
            Police.ResetPoliceStats();
            Police.ResetPersonOfInterest();
            GameTimeLastTakenOver = Game.GameTime;
            Menus.TakeoverRadius = -1f;//reset this on the menu
            InstantAction.LastWeapon = 0;
            CurrentPed.IsPersistent = false;
            if (Game.LocalPlayer.Character.IsWearingHelmet)
            {
                PedOriginallyHadHelmet = true;
            }
        }
        catch (Exception e3)
        {
            LocalWriteToLog("TakeoverPed", "TakeoverPed Error; " + e3.Message);
        }
    }
    private static void AddPedToTakenOverPeds(TakenOverPed MyPed)
    {
        if (!TakenOverPeds.Any(x => x.OriginalHandle == MyPed.Pedestrian.Handle))
        {
            TakenOverPeds.Add(MyPed);
            LocalWriteToLog("AddPedToTakenOverPeds", string.Format("Added Ped to List {0} ", MyPed.Pedestrian.Handle));
        }
        else
        {
            LocalWriteToLog("AddPedToTakenOverPeds", string.Format("Ped already in list {0} ", MyPed.Pedestrian.Handle));
        }
    }
    private static void CopyPedComponentVariation(Ped myPed)
    {
        try
        {
            myPedVariation = new PedVariation();
            myPedVariation.MyPedComponents = new List<PedComponent>();
            myPedVariation.MyPedProps = new List<PropComponent>();
            for (int ComponentNumber = 0; ComponentNumber < 12; ComponentNumber++)
            {
                myPedVariation.MyPedComponents.Add(new PedComponent(ComponentNumber, NativeFunction.CallByName<int>("GET_PED_DRAWABLE_VARIATION", myPed, ComponentNumber), NativeFunction.CallByName<int>("GET_PED_TEXTURE_VARIATION", myPed, ComponentNumber), NativeFunction.CallByName<int>("GET_PED_PALETTE_VARIATION", myPed, ComponentNumber)));
            }
            for (int PropNumber = 0; PropNumber < 8; PropNumber++)
            {
                myPedVariation.MyPedProps.Add(new PropComponent(PropNumber, NativeFunction.CallByName<int>("GET_PED_PROP_INDEX", myPed, PropNumber), NativeFunction.CallByName<int>("GET_PED_PROP_TEXTURE_INDEX", myPed, PropNumber)));
            }
        }
        catch (Exception e)
        {
            LocalWriteToLog("CopyPedComponentVariation", "CopyPedComponentVariation Error; " + e.Message);
        }
    }
    private static PedVariation GetPedVariation(Ped myPed)
    {
        try
        {
            myPedVariation = new PedVariation();
            myPedVariation.MyPedComponents = new List<PedComponent>();
            myPedVariation.MyPedProps = new List<PropComponent>();
            for (int ComponentNumber = 0; ComponentNumber < 12; ComponentNumber++)
            {
                myPedVariation.MyPedComponents.Add(new PedComponent(ComponentNumber, NativeFunction.CallByName<int>("GET_PED_DRAWABLE_VARIATION", myPed, ComponentNumber), NativeFunction.CallByName<int>("GET_PED_TEXTURE_VARIATION", myPed, ComponentNumber), NativeFunction.CallByName<int>("GET_PED_PALETTE_VARIATION", myPed, ComponentNumber)));
            }
            for (int PropNumber = 0; PropNumber < 8; PropNumber++)
            {
                myPedVariation.MyPedProps.Add(new PropComponent(PropNumber, NativeFunction.CallByName<int>("GET_PED_PROP_INDEX", myPed, PropNumber), NativeFunction.CallByName<int>("GET_PED_PROP_TEXTURE_INDEX", myPed, PropNumber)));
            }
            return myPedVariation;
        }
        catch (Exception e)
        {
            LocalWriteToLog("CopyPedComponentVariation", "CopyPedComponentVariation Error; " + e.Message);
            return null;
        }
    }
    private static void ReplacePedComponentVariation(Ped myPed)
    {
        try
        {
            foreach (PedComponent Component in myPedVariation.MyPedComponents)
            {
                NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", myPed, Component.ComponentID, Component.DrawableID, Component.TextureID, Component.PaletteID);
            }
            foreach (PropComponent Prop in myPedVariation.MyPedProps)
            {
                NativeFunction.CallByName<uint>("SET_PED_PROP_INDEX", myPed, Prop.PropID, Prop.DrawableID, Prop.TextureID, false);
            }
        }
        catch (Exception e)
        {
            LocalWriteToLog("ReplacePedComponentVariation", "ReplacePedComponentVariation Error; " + e.Message);
        }
    }
    private static void SetPlayerOffset()
    {
        const int WORLD_OFFSET = 8;
        const int SECOND_OFFSET = 0x20;
        const int THIRD_OFFSET = 0x18;

        Memory GTA = new Memory("GTA5");
        UInt64 WorldFlirtPointer = GTA.PointerScan("48 8B 05 ? ? ? ? 45 ? ? ? ? 48 8B 48 08 48 85 C9 74 07");
        UInt64 World = GTA.ReadRelativeAddress(WorldFlirtPointer);
        UInt64 Player = GTA.Read<UInt64>(World, new int[] { WORLD_OFFSET });
        UInt64 Second = GTA.Read<UInt64>(Player + SECOND_OFFSET);
        UInt64 Third = GTA.Read<UInt64>(Second + THIRD_OFFSET);

        if (Settings.MainCharacterToAlias == "Michael")
            GTA.Write<uint>(Player + SECOND_OFFSET, 225514697, new int[] { THIRD_OFFSET });
        else if (Settings.MainCharacterToAlias == "Franklin")
            GTA.Write<uint>(Player + SECOND_OFFSET, 2602752943, new int[] { THIRD_OFFSET });
        else if (Settings.MainCharacterToAlias == "Trevor")
            GTA.Write<uint>(Player + SECOND_OFFSET, 2608926626, new int[] { THIRD_OFFSET });

    }
    private static void AITakeoverPlayer(Ped FormerPlayer)
    {
        if (FormerPlayer.IsDead)
        {
            return;
        }
        if (FormerPlayer.IsInAnyVehicle(false))
        {
            FormerPlayer.Tasks.CruiseWithVehicle(FormerPlayer.CurrentVehicle, 30f, VehicleDrivingFlags.Normal); //normal driving style
        }
        else
        {
            FormerPlayer.Tasks.ClearImmediately();
            FormerPlayer.Tasks.Wander();
        }
    }
    private static void ChangeModel(String ModelRequested)
    {
        Model characterModel = new Model(ModelRequested);
        characterModel.LoadAndWait();
        characterModel.LoadCollisionAndWait();
        Game.LocalPlayer.Model = characterModel;
        Game.LocalPlayer.Character.IsCollisionEnabled = true;
    }
    public static void ResetModel()
    {
        bool WasInVehicle = false;
        Vehicle vehicleWasIn = null;
        if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
        {
            WasInVehicle = true;
            vehicleWasIn = Game.LocalPlayer.Character.CurrentVehicle;
        }

        Model characterModel = new Model(Settings.MainCharacterToAliasModelName);//should not need to load player models?
        Game.LocalPlayer.Model = characterModel;
        Game.LocalPlayer.Character.IsCollisionEnabled = true;
        if(WasInVehicle)
        {
            Game.LocalPlayer.Character.WarpIntoVehicle(vehicleWasIn, -1);
        }
    }
    internal static void AddRemovePlayerHelmet()
    {
        if (Game.LocalPlayer.Character.IsWearingHelmet)
            Game.LocalPlayer.Character.RemoveHelmet(false);
        else
        {
            if (PedOriginallyHadHelmet)
            {
                PropComponent MyPropComponent = myPedVariation.MyPedProps.Where(x => x.PropID == 0).FirstOrDefault();
                if (MyPropComponent == null)
                    return;

                Game.LocalPlayer.Character.GiveHelmet(true, (Rage.HelmetTypes)MyPropComponent.DrawableID, MyPropComponent.TextureID);
                LocalWriteToLog("AddRemovePlayerHelmet", "Original");
            }
            else
            {
                Game.LocalPlayer.Character.GiveHelmet(true, HelmetTypes.RegularMotorcycleHelmet, 0);
                LocalWriteToLog("AddRemovePlayerHelmet", "Not Original");
            }
        }
    }
    private static void LocalWriteToLog(string ProcedureString, string TextToLog)
    {
        if (Settings.PedSwappingLogging)
            Debugging.WriteToLog(ProcedureString, TextToLog);
    }
}

