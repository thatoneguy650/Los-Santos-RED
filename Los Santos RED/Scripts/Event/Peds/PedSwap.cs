using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class PedSwap
{
    private static Vector3 CurrentPedPosition;
    private static Vector3 TargetPedPosition;
    private static bool TargetPedUsingScenario;
    private static Ped CurrentPed;
    private static bool TargetPedInVehicle;
    private static Vehicle TargetPedVehicle;
    private static bool TargetPedAlreadyTakenOver;
   // private static Vector3 DopplegangerPosition;

   // public static Ped Doppleganger;//temp public

    private static List<TakenOverPed> TakenOverPeds;
    private static Model OriginalModel;
    private static string LastModelHash;
    private static PedVariation CurrentPedVariation;
    private static bool PedOriginallyHadHelmet;
    private static uint GameTimeLastTakenOver;
    private static bool CurrentPlayerIsMale = false;

    public static string CurrentPlayerModel;//temp public

    //public static PedHeadshot CurrentHeadshot;
    public static bool RecentlyTakenOver
    {
        get
        {
            if (Game.GameTime - GameTimeLastTakenOver <= 1000)//Right when you takeover a ped they might become wanted for some weird reason, this stops that
                return true;
            else
                return false;
        }
    }
    public static Vehicle OwnedCar { get; set; }
    public static string SuspectName { get; set; }
    public static void Initialize()
    {
        OriginalModel = default;
        LastModelHash = "";
        TakenOverPeds = new List<TakenOverPed>();
        PedOriginallyHadHelmet = false;
        GameTimeLastTakenOver = Game.GameTime;
        Names.Initialize();
        CurrentPlayerModel = Game.LocalPlayer.Character.Model.Name;
        CurrentPedVariation = General.GetPedVariation(Game.LocalPlayer.Character);
        CurrentPlayerIsMale = Game.LocalPlayer.Character.IsMale;
        NamePed();
    }
    public static void NamePed()
    {

        Debugging.WriteToLog("Named Ped", "Pre Name");

        if (CurrentPlayerModel.ToLower() == "player_zero")
            SuspectName = "Michael De Santa";
        else if (CurrentPlayerModel.ToLower() == "player_one")
            SuspectName = "Franklin Clinton";
        else if (CurrentPlayerModel.ToLower() == "player_two")
            SuspectName = "Trevor Philips";
        else
            SuspectName = Names.GetRandomName(CurrentPlayerIsMale);

        Debugging.WriteToLog("Named Ped", string.Format("CurrentPlayerModel: {0}, CurrentName: {1}", CurrentPlayerModel.ToLower(), SuspectName));
    }
    public static void Dispose()
    {
        // ResetModel();

        //if (Doppleganger.Exists())
        //    Doppleganger.Delete();
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
            PedToReturn = closestPed.Where(s => s.CanTakeoverPed()).OrderBy(s => General.MyRand.Next()).FirstOrDefault();
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
    private static void AllyPedsToPlayer(Ped[] PedList)
    {
        foreach (Ped PedToAlly in PedList)
        {
            NativeFunction.CallByName<bool>("SET_PED_AS_GROUP_MEMBER", PedToAlly, Game.LocalPlayer.Character.Group);
            PedToAlly.StaysInVehiclesWhenJacked = true;
        }
    }
    public static void TakeoverPed(Ped TargetPed, bool DeleteOld, bool ArrestOld, bool ClearNearPolice,bool AdvanceTime)
    {
        try
        {
            if (TargetPed == null)
                return;

            if(ClearNearPolice)
                PedList.ClearPoliceCompletely();

            StoreTargetPedData(TargetPed);

            NativeFunction.CallByName<uint>("CHANGE_PLAYER_PED", Game.LocalPlayer, TargetPed, false, false);
            CurrentPed.IsPersistent = false;

            if (DeleteOld)
                CurrentPed.Delete();
            else if (ArrestOld)
                Surrender.SetArrestedAnimation(CurrentPed, true);
            else
                AITakeoverPlayer(CurrentPed);

            PostTakeover(LastModelHash);

            //if (AdvanceTime)
            //    World.DateTime.AddHours(18);

        }
        catch (Exception e3)
        {
            Debugging.WriteToLog("TakeoverPed", "TakeoverPed Error; " + e3.Message + " " + e3.StackTrace);
        }
    }
    public static void BecomeScenarioPed()
    {
        string ModelName = "s_m_y_prisoner_01";
        Vector3 Position = new Vector3(1858.988f, 2627.205f, 45.67201f);
        float Heading = 350.5708f;

        TargetPedInVehicle = false;

        PostTakeover(ModelName);
        Game.LocalPlayer.Character.RandomizeVariation();

        Game.LocalPlayer.Character.Position = Position;
        Game.LocalPlayer.Character.Heading = Heading;
        WantedLevelScript.SetWantedLevel(3, "Scenario", true);
        //WantedLevelScript.CurrentCrimes.TrespessingOnGovtProperty.HasBeenWitnessedByPolice = true;

        if(General.RandomPercent(70))
            Weapons.GetRandomRegularWeaponByCategory(GTAWeapon.WeaponCategory.Pistol);
        else
            Weapons.GetRandomRegularWeaponByCategory(GTAWeapon.WeaponCategory.Melee);

    }
    private static void StoreTargetPedData(Ped TargetPed)
    {
        CurrentPedVariation = General.GetPedVariation(TargetPed);
        CurrentPlayerModel = TargetPed.Model.Name;
        CurrentPlayerIsMale = TargetPed.IsMale;
        Clock.StoreTime();

        Debugging.WriteToLog("StoreTargetPedData", string.Format("CurrentPlayerModel: {0}",CurrentPlayerModel));

        CurrentPedPosition = Game.LocalPlayer.Character.Position;//Vector3 CurrentPosition = Game.LocalPlayer.Character.Position;
        TargetPedPosition = TargetPed.Position;// Vector3 TargetPedPosition = TargetPed.Position;

        if (Game.LocalPlayer.Character.IsDead)
        {
            Respawn.RespawnInPlace(false);
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
                    General.ReplacePedComponentVariation(Game.LocalPlayer.Character, CurrentPedVariation);
            }
        }

        OriginalModel = TargetPed.Model;

        AddPedToTakenOverPeds(new TakenOverPed(TargetPed, TargetPed.Handle, General.GetPedVariation(TargetPed), TargetPed.Model, Game.GameTime));

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
            AllyPedsToPlayer(TargetPed.CurrentVehicle.Passengers);
        }
        else
        {
            AllyPedsToPlayer(Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 5f, GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed).Where(x => x is Ped).ToArray(), (x => (Ped)x)));
        }
    }
    private static void PostTakeover(string ModelToChange)
    {
        NativeFunction.Natives.x2206BF9A37B7F724("MinigameTransitionOut", 5000, false);

        if (!TargetPedAlreadyTakenOver)
        {
            SetPlayerOffset();
            ChangeModel(General.MySettings.MainCharacterToAliasModelName);
            ChangeModel(ModelToChange);
        }

        if (!Game.LocalPlayer.Character.IsMainCharacter())
            General.ReplacePedComponentVariation(Game.LocalPlayer.Character,CurrentPedVariation);

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

        if (General.MySettings.General.PedTakeoverSetRandomMoney)
            Game.LocalPlayer.Character.SetCash(General.MyRand.Next(General.MySettings.General.PedTakeoverRandomMoneyMin, General.MySettings.General.PedTakeoverRandomMoneyMax));

        Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        Game.LocalPlayer.Character.Inventory.GiveNewWeapon(2725352035, 0, true);


        PlayerState.ResetState(true);



        //PlayerState.IsDead = false;
        //PlayerState.IsBusted = false;
        //PlayerState.BeingArrested = false;
        //PlayerState.TimesDied = 0;
        //PlayerState.MaxWantedLastLife = 0;
        //PlayerState.LastWeaponHash = 0;

        Game.TimeScale = 1f;
        WantedLevelScript.SetWantedLevel(0, "Reset After Takeover as a precaution",false);

        NativeFunction.Natives.xB4EDDC19532BFB85();
        Game.HandleRespawn();
        NativeFunction.CallByName<bool>("NETWORK_REQUEST_CONTROL_OF_ENTITY", Game.LocalPlayer.Character);
        NativeFunction.Natives.xC0AA53F866B3134D();

        WantedLevelScript.Reset();
        PersonOfInterest.Reset();
        GameTimeLastTakenOver = Game.GameTime;
        Menus.SelectedTakeoverRadius = -1f;//reset this on the menu
        if(CurrentPed.Exists())
            CurrentPed.IsPersistent = false;
        ActivateScenariosAfterTakeover();

        GivePedHistory();
        General.SetPedUnarmed(Game.LocalPlayer.Character, false);
        NamePed();
        Clock.ResetTime();

        PlayerHealth.Health = Game.LocalPlayer.Character.Health;
        PlayerHealth.Armor = Game.LocalPlayer.Character.Armor;

        GameFiber.Wait(50);
        PlayerState.DisplayPlayerNotification();

    }
    private static void GivePedHistory()
    {
        //WantedLevelScript.CurrentCrimes.GiveCriminalHistory();
    }
    private static void ActivateScenariosAfterTakeover()
    {
        if (TargetPedUsingScenario)
        {
            Debugging.WriteToLog("TakeoverPed", string.Format("Using Scenario: {0}", TargetPedUsingScenario));

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
            Debugging.WriteToLog("AddPedToTakenOverPeds", string.Format("Added Ped to List {0} ", MyPed.Pedestrian.Handle));
        }
        else
        {
            Debugging.WriteToLog("AddPedToTakenOverPeds", string.Format("Ped already in list {0} ", MyPed.Pedestrian.Handle));
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

        if (General.MySettings.General.MainCharacterToAlias == "Michael")
            GTA.Write<uint>(Player + SECOND_OFFSET, 225514697, new int[] { THIRD_OFFSET });
        else if (General.MySettings.General.MainCharacterToAlias == "Franklin")
            GTA.Write<uint>(Player + SECOND_OFFSET, 2602752943, new int[] { THIRD_OFFSET });
        else if (General.MySettings.General.MainCharacterToAlias == "Trevor")
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

        Model characterModel = new Model(General.MySettings.MainCharacterToAliasModelName);//should not need to load player models?
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
                PedPropComponent MyPropComponent = CurrentPedVariation.MyPedProps.Where(x => x.PropID == 0).FirstOrDefault();
                if (MyPropComponent == null)
                    return;

                Game.LocalPlayer.Character.GiveHelmet(true, (Rage.HelmetTypes)MyPropComponent.DrawableID, MyPropComponent.TextureID);
                Debugging.WriteToLog("AddRemovePlayerHelmet", "Original");
            }
            else
            {
                Game.LocalPlayer.Character.GiveHelmet(true, HelmetTypes.RegularMotorcycleHelmet, 0);
                Debugging.WriteToLog("AddRemovePlayerHelmet", "Not Original");
            }
        }
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

