using ExtensionsMethods;
using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;


public static class PedSwapManager
{
    private static Vector3 CurrentPedPosition;
    private static Vector3 TargetPedPosition;
    private static bool TargetPedUsingScenario;
    private static Ped CurrentPed;
    private static bool TargetPedInVehicle;
    private static Vehicle TargetPedVehicle;
    private static bool TargetPedAlreadyTakenOver;
    private static List<TakenOverPed> TakenOverPeds;
    private static Model OriginalModel;
    private static string LastModelHash;
    private static PedVariation CurrentPedVariation;
    private static uint GameTimeLastTakenOver;
    private static bool CurrentPlayerIsMale = false;

    private static string CurrentPlayerModel;
    public static Vehicle OwnedCar { get; set; }
    public static string SuspectName { get; set; }
    public static bool RecentlyTakenOver
    {
        get
        {
            if (Game.GameTime - GameTimeLastTakenOver <= 5000)//Right when you takeover a ped they might become wanted for some weird reason, this stops that
                return true;
            else
                return false;
        }
    }
    public static void Initialize()
    {
        OriginalModel = default;
        LastModelHash = "";
        TakenOverPeds = new List<TakenOverPed>();
        GameTimeLastTakenOver = Game.GameTime;
        CurrentPlayerModel = Game.LocalPlayer.Character.Model.Name;
        CurrentPedVariation = GetPedVariation(Game.LocalPlayer.Character);
        CurrentPlayerIsMale = Game.LocalPlayer.Character.IsMale;
        GiveName();
    }
    public static void TakeoverPed(float Radius, bool Nearest, bool DeleteOld, bool ClearNearPolice)
    {
        try
        {
            Ped TargetPed = FindPedToSwapWith(Radius, Nearest);

            if (TargetPed == null)
                return;

            if (ClearNearPolice)
            {
                Mod.PedManager.ClearPolice();
                VehicleManager.ClearPolice();
            }

            StoreTargetPedData(TargetPed);

            NativeFunction.CallByName<uint>("CHANGE_PLAYER_PED", Game.LocalPlayer, TargetPed, false, false);
            CurrentPed.IsPersistent = false;

            if (DeleteOld)
                CurrentPed.Delete();
            else
                TaskFormerPed(CurrentPed);

            PostTakeover(LastModelHash);
        }
        catch (Exception e3)
        {
            Debugging.WriteToLog("TakeoverPed", "TakeoverPed Error; " + e3.Message + " " + e3.StackTrace);
        }
    }
    public static void BecomeMPCharacter(bool IsMale)
    {
        SetPlayerOffset();
        ChangeModel(SettingsManager.MySettings.General.MainCharacterToAliasModelName);
        //if(IsMale)
        //{
        //    ChangeModel(ModelToChange);
        //}
        //else
        //{
        //    ChangeModel(ModelToChange);
        //}
        
    }
    private static Ped FindPedToSwapWith(float Radius, bool Nearest)
    {
        Ped PedToReturn = null;
        Ped[] closestPed = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, Radius, GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed | GetEntitiesFlags.ConsiderAllPeds).Where(x => x is Ped).ToArray(), (x => (Ped)x));
        if (Nearest)
            PedToReturn = closestPed.Where(s => s.CanTakeoverPed()).OrderBy(s => Vector3.Distance(Game.LocalPlayer.Character.Position, s.Position)).FirstOrDefault();
        else
            PedToReturn = closestPed.Where(s => s.CanTakeoverPed()).OrderBy(s => RandomItems.MyRand.Next()).FirstOrDefault();
        if (PedToReturn == null)
        {
            Debugging.WriteToLog("Ped Takeover", "No Peds Found");
            return null;
        }
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
    private static PedVariation GetPedVariation(Ped myPed)
    {
        try
        {
            PedVariation myPedVariation = new PedVariation
            {
                MyPedComponents = new List<PedComponent>(),
                MyPedProps = new List<PedPropComponent>()
            };
            for (int ComponentNumber = 0; ComponentNumber < 12; ComponentNumber++)
            {
                myPedVariation.MyPedComponents.Add(new PedComponent(ComponentNumber, NativeFunction.CallByName<int>("GET_PED_DRAWABLE_VARIATION", myPed, ComponentNumber), NativeFunction.CallByName<int>("GET_PED_TEXTURE_VARIATION", myPed, ComponentNumber), NativeFunction.CallByName<int>("GET_PED_PALETTE_VARIATION", myPed, ComponentNumber)));
            }
            for (int PropNumber = 0; PropNumber < 8; PropNumber++)
            {
                myPedVariation.MyPedProps.Add(new PedPropComponent(PropNumber, NativeFunction.CallByName<int>("GET_PED_PROP_INDEX", myPed, PropNumber), NativeFunction.CallByName<int>("GET_PED_PROP_TEXTURE_INDEX", myPed, PropNumber)));
            }
            return myPedVariation;
        }
        catch (Exception e)
        {
            Debugging.WriteToLog("CopyPedComponentVariation", "CopyPedComponentVariation Error; " + e.Message);
            return null;
        }
    }
    private static void GiveName()
    {
        if (CurrentPlayerModel.ToLower() == "player_zero")
            SuspectName = "Michael De Santa";
        else if (CurrentPlayerModel.ToLower() == "player_one")
            SuspectName = "Franklin Clinton";
        else if (CurrentPlayerModel.ToLower() == "player_two")
            SuspectName = "Trevor Philips";
        else
            SuspectName = NameManager.GetRandomName(CurrentPlayerIsMale);
    }
    private static void StoreTargetPedData(Ped TargetPed)
    {
        CurrentPedVariation = GetPedVariation(TargetPed);
        CurrentPlayerModel = TargetPed.Model.Name;
        CurrentPlayerIsMale = TargetPed.IsMale;
        Mod.ClockManager.PauseTime();

        CurrentPedPosition = Game.LocalPlayer.Character.Position;
        TargetPedPosition = TargetPed.Position;

        if (Game.LocalPlayer.Character.IsDead)
        {
            RespawnManager.RespawnInPlace(false);
        }
        Vector3 PlayerOriginalPedPosition = Game.LocalPlayer.Character.Position;
        TargetPedAlreadyTakenOver = false;
        if (TakenOverPeds.Count > 0)
        {
            uint TargetModelHash = TargetPed.Model.Hash;
            if (TargetModelHash == 225514697 || TargetModelHash == 2602752943 || TargetModelHash == 2608926626)
            {
                TargetPedAlreadyTakenOver = true;
                ChangeModel(OriginalModel.Name);
                if (!Game.LocalPlayer.Character.IsMainCharacter())
                    CurrentPedVariation.ReplacePedComponentVariation(Game.LocalPlayer.Character);
            }
        }

        OriginalModel = TargetPed.Model;

        AddPedToTakenOverPeds(new TakenOverPed(TargetPed, TargetPed.Handle, GetPedVariation(TargetPed), TargetPed.Model, Game.GameTime));

        if (!TargetPedAlreadyTakenOver)
            LastModelHash = TargetPed.Model.Name;

        TargetPedInVehicle = TargetPed.IsInAnyVehicle(false);//bool wasInVehicle = TargetPed.IsInAnyVehicle(false);
        if (TargetPedInVehicle)
            TargetPedVehicle = TargetPed.CurrentVehicle;

        TargetPedUsingScenario = NativeFunction.CallByName<bool>("IS_PED_USING_ANY_SCENARIO", TargetPed);//bool Scenario = false;

        if (Game.LocalPlayer.Character.LastVehicle.Exists())
            Game.LocalPlayer.Character.LastVehicle.Delete();

        CurrentPed = Game.LocalPlayer.Character;

        if (TargetPed.IsInAnyVehicle(false))
        {
            Game.LocalPlayer.Character.WarpIntoVehicle(TargetPedVehicle, -1);
            AllyClosePedsToPlayer(TargetPed.CurrentVehicle.Passengers);
        }
        else
        {
            AllyClosePedsToPlayer(Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 5f, GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed).Where(x => x is Ped).ToArray(), (x => (Ped)x)));
        }
    }
    private static void PostTakeover(string ModelToChange)
    {
        NativeFunction.Natives.x2206BF9A37B7F724("MinigameTransitionOut", 5000, false);

        if (!TargetPedAlreadyTakenOver)
        {
            SetPlayerOffset();
            ChangeModel(SettingsManager.MySettings.General.MainCharacterToAliasModelName);
            ChangeModel(ModelToChange);
        }

        if (!Game.LocalPlayer.Character.IsMainCharacter())
            CurrentPedVariation.ReplacePedComponentVariation(Game.LocalPlayer.Character);

        if (TargetPedInVehicle)
        {
            Game.LocalPlayer.Character.WarpIntoVehicle(TargetPedVehicle, -1);
            NativeFunction.CallByName<bool>("SET_VEHICLE_HAS_BEEN_OWNED_BY_PLAYER", Game.LocalPlayer.Character.CurrentVehicle, true);
            if (OwnedCar != null && OwnedCar.Exists())
                OwnedCar.IsPersistent = false;
            OwnedCar = TargetPedVehicle;
            OwnedCar.IsPersistent = true;
        }
        else
        {
            Game.LocalPlayer.Character.IsCollisionEnabled = true;
        }

        if (SettingsManager.MySettings.General.PedTakeoverSetRandomMoney)
            Game.LocalPlayer.Character.SetCash(RandomItems.MyRand.Next(SettingsManager.MySettings.General.PedTakeoverRandomMoneyMin, SettingsManager.MySettings.General.PedTakeoverRandomMoneyMax));

        Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        Game.LocalPlayer.Character.Inventory.GiveNewWeapon(2725352035, 0, true);


        Mod.Player.ResetState(true);

        Game.TimeScale = 1f;
        WantedLevelManager.SetWantedLevel(0, "Reset After Takeover as a precaution",false);

        NativeFunction.Natives.xB4EDDC19532BFB85();
        Game.HandleRespawn();
        NativeFunction.CallByName<bool>("NETWORK_REQUEST_CONTROL_OF_ENTITY", Game.LocalPlayer.Character);
        NativeFunction.Natives.xC0AA53F866B3134D();

        WantedLevelManager.Reset();
        PersonOfInterestManager.Reset();
        GameTimeLastTakenOver = Game.GameTime;
        MenuManager.SelectedTakeoverRadius = -1f;//reset this on the menu
        if(CurrentPed.Exists())
            CurrentPed.IsPersistent = false;
        ActivatePreviousScenarios();

        Game.LocalPlayer.Character.SetUnarmed();
        GiveName();
        Mod.ClockManager.UnpauseTime();

        WeaponDroppingManager.Reset();

        //PlayerHealth.Health = Game.LocalPlayer.Character.Health;
        //PlayerHealth.Armor = Game.LocalPlayer.Character.Armor;

        GameFiber.Wait(50);
        Mod.Player.DisplayPlayerNotification();

    }
    private static void AllyClosePedsToPlayer(Ped[] PedList)
    {
        foreach (Ped PedToAlly in PedList)
        {
            NativeFunction.CallByName<bool>("SET_PED_AS_GROUP_MEMBER", PedToAlly, Game.LocalPlayer.Character.Group);
            PedToAlly.StaysInVehiclesWhenJacked = true;
        }
    }
    private static void ActivatePreviousScenarios()
    {
        if (TargetPedUsingScenario)
        {
            NativeFunction.CallByName<bool>("TASK_USE_NEAREST_SCENARIO_TO_COORD_WARP", Game.LocalPlayer.Character, TargetPedPosition.X, TargetPedPosition.Y, TargetPedPosition.Z, 5f, 0);
            GameFiber ScenarioWatcher = GameFiber.StartNew(delegate
            {
                while (!Extensions.IsMoveControlPressed())
                    GameFiber.Yield();
                Game.LocalPlayer.Character.Tasks.Clear();
        }, "ScenarioWatcher");
            Debugging.GameFibers.Add(ScenarioWatcher);
        }
    }
    private static void AddPedToTakenOverPeds(TakenOverPed MyPed)
    {
        if (!TakenOverPeds.Any(x => x.OriginalHandle == MyPed.Pedestrian.Handle))
        {
            TakenOverPeds.Add(MyPed);
        }
    }
    private static void SetPlayerOffset()
    {
        //i have no idea how this works
        const int WORLD_OFFSET = 8;
        const int SECOND_OFFSET = 0x20;
        const int THIRD_OFFSET = 0x18;

        Memory GTA = new Memory("GTA5");
        UInt64 WorldFlirtPointer = GTA.PointerScan("48 8B 05 ? ? ? ? 45 ? ? ? ? 48 8B 48 08 48 85 C9 74 07");
        UInt64 World = GTA.ReadRelativeAddress(WorldFlirtPointer);
        UInt64 Player = GTA.Read<UInt64>(World, new int[] { WORLD_OFFSET });
        UInt64 Second = GTA.Read<UInt64>(Player + SECOND_OFFSET);
        UInt64 Third = GTA.Read<UInt64>(Second + THIRD_OFFSET);

        if (SettingsManager.MySettings.General.MainCharacterToAlias == "Michael")
            GTA.Write<uint>(Player + SECOND_OFFSET, 225514697, new int[] { THIRD_OFFSET });
        else if (SettingsManager.MySettings.General.MainCharacterToAlias == "Franklin")
            GTA.Write<uint>(Player + SECOND_OFFSET, 2602752943, new int[] { THIRD_OFFSET });
        else if (SettingsManager.MySettings.General.MainCharacterToAlias == "Trevor")
            GTA.Write<uint>(Player + SECOND_OFFSET, 2608926626, new int[] { THIRD_OFFSET });

    }
    private static void TaskFormerPed(Ped FormerPlayer)
    {
        if (FormerPlayer.IsDead)
        {
            return;
        }
        if (FormerPlayer.IsInAnyVehicle(false))
        {
            FormerPlayer.Tasks.CruiseWithVehicle(FormerPlayer.CurrentVehicle, 30f, VehicleDrivingFlags.Normal); //normal driving style
        }
        if(NativeFunction.CallByName<bool>("IS_PED_USING_ANY_SCENARIO", FormerPlayer))
        {
            return;
        }
        else
        {
            FormerPlayer.Tasks.ClearImmediately();
            FormerPlayer.Tasks.Wander();
        }
    }
    private static void ChangeModel(string ModelRequested)
    {
        Model characterModel = new Model(ModelRequested);
        characterModel.LoadAndWait();
        characterModel.LoadCollisionAndWait();
        Game.LocalPlayer.Model = characterModel;
        Game.LocalPlayer.Character.IsCollisionEnabled = true;
    }
    private class TakenOverPed
    {
        public Ped Pedestrian { get; set; }
        public PoolHandle OriginalHandle { get; set; }
        public PedVariation Variation { get; set; }
        public Model OriginalModel { get; set; }
        public uint GameTimeTakenover { get; set; }
        public TakenOverPed(Ped _Pedestrian, PoolHandle _OriginalHandle)
        {
            Pedestrian = _Pedestrian;
            OriginalHandle = _OriginalHandle;
        }
        public TakenOverPed(Ped _Pedestrian, PoolHandle _OriginalHandle, PedVariation _Variation, Model _OriginalModel, uint _GameTimeTakenover)
        {
            Pedestrian = _Pedestrian;
            OriginalHandle = _OriginalHandle;
            Variation = _Variation;
            OriginalModel = _OriginalModel;
            GameTimeTakenover = _GameTimeTakenover;
        }
    }
}

