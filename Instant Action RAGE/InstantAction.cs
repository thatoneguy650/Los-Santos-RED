using ExtensionsMethods;
using Instant_Action_RAGE.Systems;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

public static class InstantAction
{
    //Player State
    public static bool isDead = false;
    public static bool isBusted = false;
    private static bool BeingArrested = false;
    private static bool DiedInVehicle = false;
    private static int MaxWantedLastLife;
    private static int TimesDied;
    private static int PreviousWantedLevel;
    private static Random rnd;
    private static string LastModelHash;
    private static PedVariation myPedVariation;
    private static Vector3 PositionOfDeath;
    private static PoliceState HandsUpPreviousPoliceState;
    public static List<GTAWeapon> Weapons = new List<GTAWeapon>();
    public static bool PlayerIsJacking = false;

    //Police Items
    private static int TimeAimedAtPolice = 0;
    public static bool areHandsUp = false;
    private static bool firedWeapon = false;
    private static bool aimedAtPolice = false;
    public static bool SurrenderBust = false;
    private static uint LastBust;
    private static int ForceSurrenderTime;
    private static bool CaughtWithWeapon = false;
    private static Model CopModel = new Model("s_m_y_cop_01");
    private static List<EmergencyLocation> EmergencyLocations = new List<EmergencyLocation>();
    public static Ped GhostCop;
    private static uint WantedLevelStartTime;

    private static bool CanReportLastSeen;
    private static uint GameTimeLastGreyedOut;
    private static bool GhostCopFollow;

    //traffic
    public static List<GTAVehicle> EnteredVehicles = new List<GTAVehicle>();
    public static Vehicle OwnedCar = null;
    private static bool ViolationDrivingAgainstTraffic = false;
    private static bool ViolationDrivingOnPavement = false;
    private static bool ViolationRanRedLight = false;
    private static bool IsRunningRedLight = false;
    public static bool ReportedStolenVehicle;
    private static bool JackedCurrentVehicle;
    private static bool ViolationHitPed = false;
    private static bool ViolationHitVehicle = false;
    private static uint GameTimeInterval;
    private static bool ViolationsSpeeding = false;
    private static bool IsViolationSpeedLimit;
    private static uint GameTimeLastTakenOver;

    //Event Checkers
    private static bool PrevIsViolationSpeedLimit;
    private static int PrevTimeSincePlayerLastHitAnyVehicle;
    private static int PrevTimeSincePlayerLastHitAnyPed;
    private static bool PrevPlayerKilledPolice = false;
    private static int PrevCopsKilledByPlayer = 0;
    private static bool PrevPlayerStarsGreyedOut;
    private static bool PrevIsRunningRedLight = false;
    public static bool PrevPlayerIsJacking = false;
    private static PoliceState PrevPoliceState = PoliceState.Normal;
    private static bool PrevfiredWeapon = false;
    private static bool PrevPlayerHurtPolice = false;
    private static bool PrevPlayerInVehicle = false;
    private static int PrevWantedLevel = 0;
    private static bool PrevaimedAtPolice;
    private static bool DroppingWeapon = false;

    public static bool AnyPoliceCanSeePlayer { get; set; } = false;
    public static bool AnyPoliceRecentlySeenPlayer { get; set; } = false;
    private static bool IsRunning { get; set; } = true;
    public static PoliceState CurrentPoliceState { get; set; }
    public static bool PlayerInVehicle { get; set; } = false;
    public static bool PlayerStarsGreyedOut { get; set; } = false;
    public static bool AnyPoliceSeenPlayerThisWanted { get; set; } = false;

    enum DRIVING_FLAGS
    {
        FLAG1_STOP_VEHS = 1,
        FLAG2_STOP_PEDS = 2,
        FLAG3_AVOID_VEHS = 4,
        FLAG5_AVOID_PEDS = 16,
        FLAG6_AVOID_OBJS = 32,
        FLAG8_STOP_LIGHTS = 128,
        FLAG11_REVERSE = 1024,
        FLAG19_SHORTEST_PATH = 262144,
        FLAG23_IGN_ROADS = 4194304,
    };
    public enum PoliceState
    {
        Normal = 0,
        UnarmedChase = 1,
        CautiousChase = 2,
        DeadlyChase = 3,
        ArrestedWait = 4,
    }
    static InstantAction()
    {
        rnd = new Random();
    }
    public static void Initialize()
    {
        CopModel.LoadAndWait();
        CopModel.LoadCollisionAndWait();

        Game.LocalPlayer.Character.CanBePulledOutOfVehicles = true;

        while (Game.IsLoading)
            GameFiber.Yield();
        setupLocations();
        setupWeapons();
        MainLoop();
    }

    public static void MainLoop()
    {

        GameFiber.StartNew(delegate
        {
            Settings.Initialize();
            Menus.Intitialize();
            RespawnSystem.Initialize();
            PoliceScanningSystem.Initialize();
            DispatchAudioSystem.Initialize();
            CustomOptions.Initialize();
            PoliceSpeechSystem.Initialize();
            while (IsRunning)
            {
                UpdatePlayer();
                StateTick();
                ControlTick();
                PoliceTick();
                DebugLoop();
                TestLoop();

                if (Game.GameTime > GameTimeInterval + 500)
                {
                    TrafficViolationsTick();
                    GameTimeInterval = Game.GameTime;
                }
                GameFiber.Yield();
            }

        });

    }
    private static void TestLoop()
    {
        //Other Fun STUFF!

        //Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, Squad1Leader, GetLastVehicle(Squad1Leader), pos.X, pos.Y, pos.Z, 70f, FLAG3_AVOID_VEHS | FLAG5_AVOID_PEDS | FLAG6_AVOID_OBJS | FLAG19_SHORTEST_PATH, 10f);

        //NativeFunction.Natives.xB9EFD5C25018725A("WantedMusicDisabled", true);
        //NativeFunction.CallByName<bool>("SET_AUDIO_FLAG", "WantedMusicDisabled", true);

        NativeFunction.Natives.xB9EFD5C25018725A("WantedMusicDisabled", true);
        NativeFunction.Natives.xB9EFD5C25018725A("DISPLAY_HUD", true);
        //DISPLAY_HUD
        //NativeFunction.CallByName<bool>("SET_PED_MOVE_RATE_OVERRIDE", GhostCop, 0f);

        //if(Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle != null)
        //{
        //    Game.LocalPlayer.Character.CurrentVehicle.RadioStation = RadioStation.SelfRadio;
        //}


        //if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
        //{
        //    if (NativeFunction.Natives.GetPlayerRadioStationIndex<int>() == 17 && Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn)
        //    {
        //        // var status = CommunicationService.GetStatus();

        //        // Disable radio in car.
        //        NativeFunction.Natives.SetFrontendRadioActive(false);
        //        NativeFunction.Natives.SetVehicleRadioLoud(Game.LocalPlayer.Character.CurrentVehicle, false);
        //        NativeFunction.Natives.SetVehicleRadioEnabled(Game.LocalPlayer.Character.CurrentVehicle, false);

        //        //PushSpotifyInfo(status.Track.ArtistResource.Name, status.Track.TrackResource.Name, status.Playing);

        //        //if (DebugDraw)
        //        // DashboardScaleform.Render2D();
        //    }
        //    else
        //    {
        //        NativeFunction.CallByName<bool>("SET_VEH_RADIO_STATION", Game.LocalPlayer.Character.CurrentVehicle, "RADIO_19_USER");
        //    }

        //    // Check if vehicle radio is disabled or if it is set to be loud.
        //    if (!NativeFunction.Natives.x5F43D83FD6738741<bool>() || !NativeFunction.Natives.x032A116663A4D5AC<bool>(Game.LocalPlayer.Character.CurrentVehicle))
        //    {
        //        NativeFunction.Natives.SetFrontendRadioActive(true);
        //        NativeFunction.Natives.SetVehicleRadioLoud(Game.LocalPlayer.Character.CurrentVehicle, true);
        //        NativeFunction.Natives.SetVehicleRadioEnabled(Game.LocalPlayer.Character.CurrentVehicle, true);
        //    }
        //}
    }
    private static void DebugLoop()
    {



        if (Game.IsKeyDown(Keys.NumPad0))
        {
            Game.LocalPlayer.Character.IsInvincible = false;

        }
        if (Game.IsKeyDown(Keys.NumPad1))
        {

            Game.LocalPlayer.Character.IsInvincible = true;
            Game.LocalPlayer.Character.Health = 100;
            WriteToLog("KeyDown", "You are invicible");
        }
        if (Game.IsKeyDown(Keys.NumPad3))
        {

            CurrentPoliceState = PoliceState.Normal;
            Game.LocalPlayer.WantedLevel = 0;
            PoliceScanningSystem.UntaskAll();


            foreach (GTACop Cop in PoliceScanningSystem.K9Peds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInHelicopter))
            {
                Cop.CopPed.Delete();
            }
            foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInAnyVehicle(false) && !x.CopPed.IsInHelicopter))
            {
                Cop.CopPed.Delete();
            }

            Ped[] closestPed = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 400f, GetEntitiesFlags.ExcludePlayerPed | GetEntitiesFlags.ConsiderAnimalPeds).Where(x => x is Ped).ToArray(), (x => (Ped)x));
            foreach (Ped dog in closestPed)
            {
                dog.Delete();
            }

            Game.TimeScale = 1f;
            isBusted = false;
            BeingArrested = false;
            NativeFunction.Natives.xB4EDDC19532BFB85();

        }
      
        if (Game.IsKeyDown(Keys.NumPad5))
        {
            try
            {
                isBusted = true;
                SetArrestedAnimation(Game.LocalPlayer.Character, false);
            }
            catch (Exception e)
            {
                WriteToLog("Car stuff", e.Message);
            }
        }
        if (Game.IsKeyDown(Keys.NumPad6))
        {
            try
            {
                //WriteToLog("KeyDown", string.Format("ModelName: {0}", Game.LocalPlayer.Character.CurrentVehicle.Model.Name));
                //WriteToLog("KeyDown", string.Format("SPeed: {0}", Game.LocalPlayer.Character.CurrentVehicle.Speed));
                // ResetTrafficViolations();
                //Game.LocalPlayer.Character.GiveCash(5000, "Michael");
                //ulong myHash = (ulong)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash;
                //WriteToLog("CurrentWeapon", string.Format("CurrentWeapon is: {0}", myHash));

                //GTAWeapon MatchedWeapon = Weapons.Where(x => x.Hash == myHash).FirstOrDefault();


                //if (MatchedWeapon != null)
                //{
                //    WriteToLog("CurrentWeapon", string.Format("Matched {0}, Category {1}, Level {2}", MatchedWeapon.Name, MatchedWeapon.Category, MatchedWeapon.WeaponLevel));
                //}

                ResetPlayer(true, true);

                ////InstantAction.Weapons.Where(x => x.Name == Game.LocalPlayer.Character.Inventory.EquippedWeapon.Asset.nam)




                //WriteToLog("GameTime", string.Format("GameTime is: {0}", Game.GameTime));
                //foreach (GTAVehicle EnteredVehicle in EnteredVehicles)
                //{
                //    InstantAction.WriteToLog("GTAVehicleS", string.Format("Vehicle Created: Handle {0},GameTimeEntered,{1},GameTimeToReportStolen {2},IsStolen {3},WillBeReportedStolen {4},WasReportedStolen {5}", EnteredVehicle.VehicleEnt.Handle, EnteredVehicle.GameTimeEntered, EnteredVehicle.GameTimeToReportStolen, EnteredVehicle.IsStolen, EnteredVehicle.WillBeReportedStolen, EnteredVehicle.WasReportedStolen));
                //}
                // WriteToLog("KeyDown", string.Format("SPeed: {0}", Game.LocalPlayer.Character.CurrentVehicle.Speed));

                //Game.LocalPlayer.Character.GiveCash(5000, "Michael");
                //Game.HandleRespawn();
                //   DispatchAudioSystem.ReportVehicleHitAndRun(Game.LocalPlayer.Character.CurrentVehicle);
                //UnSetArrestedAnimation(Game.LocalPlayer.Character);


                //MoveGhostCopToPlayer();
                //Game.LocalPlayer.Character.GiveCash(5000, "Michael");
                //MoveGhostCopToPlayer();
            }
            catch (Exception e)
            {
                WriteToLog("Car stuff", e.Message);
            }
        }
    }

    private static void UpdatePlayer()
    {
        PlayerInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);

        if (PlayerInVehicle)
        {
            Vehicle CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;

            if (!EnteredVehicles.Any(x => x.VehicleEnt.Handle == CurrVehicle.Handle))
            {
                bool stolen = true;
                if (OwnedCar != null && OwnedCar.Handle == CurrVehicle.Handle)
                    stolen = false;

                CurrVehicle.IsStolen = stolen;
                bool AmStealingCarFromPrerson = PlayerIsJacking;
                Ped PreviousOwner;

                if (CurrVehicle.HasDriver && CurrVehicle.Driver.Handle != Game.LocalPlayer.Character.Handle)
                    PreviousOwner = CurrVehicle.Driver;
                else
                    PreviousOwner = CurrVehicle.GetPreviousPedOnSeat(-1);

                if(PreviousOwner != null && PreviousOwner.DistanceTo2D(Game.LocalPlayer.Character) <= 20f && PreviousOwner.Handle != Game.LocalPlayer.Character.Handle)
                {
                    AmStealingCarFromPrerson = true;
                }

                EnteredVehicles.Add(new GTAVehicle(CurrVehicle, Game.GameTime, AmStealingCarFromPrerson, CurrVehicle.IsAlarmSounding, PreviousOwner, !stolen, stolen));
            }
        }

        int RecentlySeenTime = 10000;
        if (PlayerInVehicle)
            RecentlySeenTime = 30000;

        AnyPoliceCanSeePlayer = PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer);
        AnyPoliceRecentlySeenPlayer = PoliceScanningSystem.CopPeds.Any(x => x.SeenPlayerSince(RecentlySeenTime));
        PlayerStarsGreyedOut = NativeFunction.CallByName<bool>("ARE_PLAYER_STARS_GREYED_OUT", Game.LocalPlayer);
        if (!AnyPoliceSeenPlayerThisWanted && AnyPoliceRecentlySeenPlayer)
            AnyPoliceSeenPlayerThisWanted = true;
    }
    //Police
    private static void GetPoliceState()
    {
        if (CurrentPoliceState == PoliceState.ArrestedWait || CurrentPoliceState == PoliceState.DeadlyChase)
            return;

        bool AnyCanSeePlayer = PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer);

        if (Game.LocalPlayer.WantedLevel == 0)
        {
            CurrentPoliceState = PoliceState.Normal;
        }
        else if (Game.LocalPlayer.WantedLevel >= 1 && Game.LocalPlayer.WantedLevel <= 3 && AnyCanSeePlayer)
        {
            if ((!firedWeapon && !PoliceScanningSystem.PlayerHurtPolice && !aimedAtPolice) && !Game.LocalPlayer.Character.isConsideredArmed()) // Unarmed and you havent killed anyone
                CurrentPoliceState = PoliceState.UnarmedChase;
            else if ((!firedWeapon && !PoliceScanningSystem.PlayerHurtPolice && !aimedAtPolice))
                CurrentPoliceState = PoliceState.CautiousChase;
            else
                CurrentPoliceState = PoliceState.DeadlyChase;

        }
        else if (Game.LocalPlayer.WantedLevel >= 4 || PoliceScanningSystem.PlayerHurtPolice || PoliceScanningSystem.PlayerKilledPolice)
        {
            CurrentPoliceState = PoliceState.DeadlyChase;
        }

        int PlayerWantedLevel = Game.LocalPlayer.WantedLevel;
        if (AnyCanSeePlayer && Game.LocalPlayer.Character.isConsideredArmed() && PlayerWantedLevel < 3 && !Game.LocalPlayer.Character.IsInAnyVehicle(false))
        {
            ulong myHash = (ulong)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash;
            GTAWeapon MatchedWeapon = Weapons.Where(x => x.Hash == myHash).FirstOrDefault();
            bool ChangedWanted = false;
            if(MatchedWeapon != null)
            {
                if (MatchedWeapon.WeaponLevel >= 3 && PlayerWantedLevel < 3)
                {
                    ChangedWanted = true;
                    Game.LocalPlayer.WantedLevel = 3;
                }
                else if (PlayerWantedLevel < 2)
                {
                    ChangedWanted = true;
                    Game.LocalPlayer.WantedLevel = 2;
                }
            }
            else if(PlayerWantedLevel < 2)
            {
                ChangedWanted = true;
                Game.LocalPlayer.WantedLevel = 2;
            }

            if (ChangedWanted)
            {
                WriteToLog("Caught with a gun", string.Format("Type {0},{1}", MatchedWeapon.Name, MatchedWeapon.Category));
                DispatchAudioSystem.DispatchQueueItem CarryingWeapon = new DispatchAudioSystem.DispatchQueueItem(DispatchAudioSystem.ReportDispatch.ReportCarryingWeapon, 3, false);
                CarryingWeapon.WeaponToReport = MatchedWeapon;
                DispatchAudioSystem.AddDispatchToQueue(CarryingWeapon);
                //DispatchAudioSystem.ReportCarryingWeapon(MatchedWeapon);
            }
        }

        if (!aimedAtPolice && Game.LocalPlayer.Character.isConsideredArmed() && Game.LocalPlayer.IsFreeAiming && AnyCanSeePlayer && PoliceScanningSystem.CopPeds.Any(x => Game.LocalPlayer.IsFreeAimingAtEntity(x.CopPed)))
        {
            if (TimeAimedAtPolice == 0)
                WriteToLog("Started Aiming at Police", "");
            TimeAimedAtPolice++;
        }
        else
        {
            if (TimeAimedAtPolice != 0)
                WriteToLog("Stopped Aiming at Police", "");
            TimeAimedAtPolice = 0;

        }

        if (TimeAimedAtPolice >= 100 && Game.LocalPlayer.WantedLevel < 3)
        {
            WriteToLog("AimedAt Police", "");
            Game.LocalPlayer.WantedLevel = 3;
            aimedAtPolice = true;
        }

        if (!firedWeapon && Game.LocalPlayer.Character.IsShooting && (PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer || (x.DistanceToPlayer <= 100f && !Game.LocalPlayer.Character.IsCurrentWeaponSilenced)))) //if (!firedWeapon && Game.LocalPlayer.Character.IsShooting && (PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer || x.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f))))
        {
            Game.LocalPlayer.WantedLevel = 2;
            firedWeapon = true;
            WriteToLog("Fired weapon", "");
        }

    }
    private static void PoliceTick()
    {
        PoliceScanningSystem.UpdatePolice();
        GetPoliceState();
        PoliceVehicleTick();
        CheckPoliceEvents();
        StolenVehiclesTick();
        

        if (CurrentPoliceState == PoliceState.Normal)
            PoliceTickNormal();
        else if (CurrentPoliceState == PoliceState.UnarmedChase)
            PoliceTickUnarmedChase();
        else if (CurrentPoliceState == PoliceState.CautiousChase)
            PoliceTickCautiousChase();
        else if (CurrentPoliceState == PoliceState.DeadlyChase)
            PoliceTickDeadlyChase();
        else if (CurrentPoliceState == PoliceState.ArrestedWait)
            PoliceTickArrestedWait();
        else
            PoliceTickNormal();

        WantedLevelTick();
    }
    private static void TrafficViolationsTick()
    {
        if (CurrentPoliceState != PoliceState.Normal)
            return;
        bool AnyCanSeePlayer = PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer && x.isInVehicle && !x.isInHelicopter);
        PlayerInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
        bool JustTakenOver = Game.GameTime - GameTimeLastTakenOver <= 5000;

        if (PlayerInVehicle && !JustTakenOver)
        {
            float VehicleSpeedMPH = Game.LocalPlayer.Character.CurrentVehicle.Speed * 2.23694f;
            Vehicle CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
            if (AnyCanSeePlayer && !ViolationDrivingAgainstTraffic && Game.LocalPlayer.IsDrivingAgainstTraffic)
            {
                ViolationDrivingAgainstTraffic = true;
                Game.LocalPlayer.WantedLevel = 1;
                //DispatchAudioSystem.ReportRecklessDriver(CurrVehicle);
                DispatchAudioSystem.DispatchQueueItem RecklessDriver = new DispatchAudioSystem.DispatchQueueItem(DispatchAudioSystem.ReportDispatch.ReportRecklessDriver, 10, false);
                RecklessDriver.VehicleToReport = new GTAVehicle(CurrVehicle, 0, false, false, null, true, false);
                DispatchAudioSystem.AddDispatchToQueue(RecklessDriver);
                WriteToLog("TrafficViolationsTick", string.Format("ViolationDrivingAgainstTraffic: {0}", ViolationDrivingAgainstTraffic));
            }
            if (AnyCanSeePlayer && !ViolationDrivingOnPavement && Game.LocalPlayer.IsDrivingOnPavement)
            {
                ViolationDrivingOnPavement = true;
                Game.LocalPlayer.WantedLevel = 1;
                //DispatchAudioSystem.ReportRecklessDriver(CurrVehicle);
                DispatchAudioSystem.DispatchQueueItem RecklessDriver = new DispatchAudioSystem.DispatchQueueItem(DispatchAudioSystem.ReportDispatch.ReportRecklessDriver, 10, false);
                RecklessDriver.VehicleToReport = new GTAVehicle(CurrVehicle, 0, false, false, null, true, false);
                DispatchAudioSystem.AddDispatchToQueue(RecklessDriver);
                WriteToLog("TrafficViolationsTick", string.Format("ViolationDrivingOnPavement: {0}", ViolationDrivingOnPavement));
            }

            float SpeedLimit  = GetSpeedLimit();
            bool ViolationSpeedLimit = VehicleSpeedMPH > SpeedLimit + 20;
            if(ViolationSpeedLimit)
            {
                IsViolationSpeedLimit = true;
            }
            else
            {
                IsViolationSpeedLimit = false;
            }
            if (AnyCanSeePlayer && AnyCanSeePlayer && !ViolationsSpeeding && ViolationSpeedLimit)
            {
                ViolationsSpeeding = true;
                Game.LocalPlayer.WantedLevel = 1;
                //DispatchAudioSystem.ReportFelonySpeeding(CurrVehicle);
                DispatchAudioSystem.DispatchQueueItem FelonySpeeding = new DispatchAudioSystem.DispatchQueueItem(DispatchAudioSystem.ReportDispatch.ReportFelonySpeeding, 10, false);
                FelonySpeeding.VehicleToReport = new GTAVehicle(CurrVehicle, 0, false, false, null, true, false);
                DispatchAudioSystem.AddDispatchToQueue(FelonySpeeding);
                WriteToLog("TrafficViolationsTick", string.Format("ViolationsSpeeding: {0}", ViolationsSpeeding));
            }
            int TimeSincePlayerLastHitAnyPed = Game.LocalPlayer.TimeSincePlayerLastHitAnyPed;
            if (AnyCanSeePlayer && !ViolationHitPed && TimeSincePlayerLastHitAnyPed > -1 && TimeSincePlayerLastHitAnyPed <= 1000)
            {
                ViolationHitPed = true;
                Game.LocalPlayer.WantedLevel = 2;
                //DispatchAudioSystem.ReportPedHitAndRun(CurrVehicle);
                DispatchAudioSystem.DispatchQueueItem PedHitAndRun = new DispatchAudioSystem.DispatchQueueItem(DispatchAudioSystem.ReportDispatch.ReportPedHitAndRun, 10, false);
                PedHitAndRun.VehicleToReport = new GTAVehicle(CurrVehicle, 0, false, false, null, true, false);
                DispatchAudioSystem.AddDispatchToQueue(PedHitAndRun);
                WriteToLog("TrafficViolationsTick", string.Format("ViolationHitPed: {0}", ViolationHitPed));
            }
            int TimeSincePlayerLastHitAnyVehicle = Game.LocalPlayer.TimeSincePlayerLastHitAnyVehicle;
            if (AnyCanSeePlayer && !ViolationHitVehicle && TimeSincePlayerLastHitAnyVehicle > -1 && TimeSincePlayerLastHitAnyVehicle <= 1000)
            {
                ViolationHitVehicle = true;
                Game.LocalPlayer.WantedLevel = 1;
                //DispatchAudioSystem.ReportVehicleHitAndRun(CurrVehicle);
                DispatchAudioSystem.DispatchQueueItem VehicleHitAndRun = new DispatchAudioSystem.DispatchQueueItem(DispatchAudioSystem.ReportDispatch.ReportVehicleHitAndRun, 10, false);
                VehicleHitAndRun.VehicleToReport = new GTAVehicle(CurrVehicle, 0, false, false, null, true, false);
                DispatchAudioSystem.AddDispatchToQueue(VehicleHitAndRun);
                WriteToLog("TrafficViolationsTick", string.Format("ViolationHitVehicle: {0}", ViolationHitVehicle));
            }

            bool IsRunningRedLight = RunningRedLight();
            //if (AnyCanSeePlayer && IsRunningRedLight)
            //{
            //    ViolationRanRedLight = true;
            //    Game.LocalPlayer.WantedLevel = 1;
            //    DispatchAudioSystem.ReportRecklessDriver(CurrVehicle);
            //    WriteToLog("TrafficViolationsTick", string.Format("Running Red light: {0}", ViolationRanRedLight));
            //}

            if(PrevIsRunningRedLight != IsRunningRedLight)
            {
                WriteToLog("TrafficViolationsTick", string.Format("You are Running a Red: {0}", IsRunningRedLight));
                PrevIsRunningRedLight = IsRunningRedLight;
            }

            if(PrevIsViolationSpeedLimit != IsViolationSpeedLimit)
            {
                WriteToLog("TrafficViolationsTick", string.Format("You are Speeding: {0}", IsViolationSpeedLimit));
                PrevIsViolationSpeedLimit = IsViolationSpeedLimit;
            }

        }
    }

    private static bool RunningRedLight()
    {

        Vehicle[] Vehicles = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 10f, GetEntitiesFlags.ConsiderAllVehicles | GetEntitiesFlags.ExcludePlayerVehicle).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));
        foreach(Vehicle vehicle in Vehicles)
        {
            if(NativeFunction.CallByName<bool>("IS_VEHICLE_STOPPED_AT_TRAFFIC_LIGHTS", vehicle))
            {
                if (GetPedStreetName(vehicle.Driver) == GetPedStreetName(Game.LocalPlayer.Character) && vehicle.PlayerVehicleIsBehind() && Game.LocalPlayer.Character.CurrentVehicle.Speed >= 10f) // We are on the same street and they are stopped
                {
                    //WriteToLog("TrafficViolationsTick", string.Format("Vehicle stopped on same street: {0}", true));
                    return true;
                }
                //if(vehicle.PlayerVehicleIsBehind())
                //{
                //    return true;
                //}
            }
        }
        return false;



        //this.vehs = World.GetNearbyVehicles(((Entity)Game.get_Player().get_Character()).get_Position(), 9f);
        //if (this.vehs.Length == 0)
        //    return false;
        //for (int index = 0; index < this.vehs.Length; ++index)
        //{
        //    if (this.autolista.Count == 0)
        //        this.autolista.Add(this.vehs[index]);
        //    else if (!this.autolista.Contains(this.vehs[index]))
        //        this.autolista.Add(this.vehs[index]);
        //}
        //if (this.autolista.Count == 0)
        //    return false;
        //for (int index = 0; index < this.autolista.Count; ++index)
        //{
        //    if ((double)Vector3.Distance(((Entity)this.autolista[index]).get_Position(), ((Entity)Game.get_Player().get_Character()).get_Position()) > 35.0)
        //        this.autolista.Remove(this.autolista[index]);
        //}
        //for (int index = 0; index < this.autolista.Count; ++index)
        //{
        //    if (Function.Call<bool>((Hash)2979683755510794905L, new InputArgument[1]
        //    {
        //  InputArgument.op_Implicit(this.autolista[index])
        //    }) != 0)
        //    {
        //        float num = Vector3.Angle(((Entity)this.autolista[index]).get_ForwardVector(), ((Entity)Game.get_Player().get_Character()).get_ForwardVector());
        //        if ((double)num <= 35.0 && ((double)num <= 35.0 && (double)Vector3.Distance(((Entity)this.autolista[index]).get_Position(), ((Entity)Game.get_Player().get_Character()).get_Position()) > 30.0 && (double)Vector3.Angle(Vector3.op_Subtraction(((Entity)this.autolista[index]).get_Position(), ((Entity)Game.get_Player().get_Character()).get_Position()), ((Entity)Game.get_Player().get_Character()).get_ForwardVector()) > 150.0))
        //            return true;
        //    }
        //}
        //return false;
    }
    private static void ResetTrafficViolations()
    {
        ViolationDrivingOnPavement = false;
        ViolationDrivingAgainstTraffic = false;
        ViolationHitPed = false;
        ViolationsSpeeding = false;
        ViolationHitVehicle = false;
    }
    private static void PullOver(GTACop Cop, Ped PullOverTarget)
    {

    }
    private static void PoliceTickNormal()
    {
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.isTasked && !x.TaskIsQueued))
        {
            Cop.TaskIsQueued = true;
            PoliceScanningSystem.CopsToTask.Add(new PoliceTask(Cop, PoliceTask.Task.Untask));
        }

    }
    private static void PoliceTickUnarmedChase()
    {
        //Cops on Foot
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => !x.isTasked))
        {
            if (Cop.CopPed.IsOnBike || Cop.CopPed.IsInHelicopter)
                SetUnarmed(Cop);
            else
                SetCopTazer(Cop);

            if (!isBusted && Cop.RecentlySeenPlayer() && !Cop.TaskIsQueued && PoliceScanningSystem.CopPeds.Where(x => x.isTasked || x.TaskIsQueued).Count() <= 4 && !Cop.isInVehicle && Cop.DistanceToPlayer <= 55f && (!Game.LocalPlayer.Character.IsInAnyVehicle(false) || Game.LocalPlayer.Character.CurrentVehicle.Speed <= 5f))
            {
                Cop.TaskIsQueued = true;
                PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Chase));
            }
        }

        if (SurrenderBust && !isBustTimeOut())
            SurrenderBustEvent();

        StopSearchMode();
    }
    private static void PoliceTickArrestedWait()
    {
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => !x.isTasked)) // Exist/Dead Check
        {
            bool InVehicle = Cop.CopPed.IsInAnyVehicle(false);
            if (InVehicle)
            {
                SetUnarmed(Cop);
            }
            else
            {
                if (!Cop.TaskIsQueued && PoliceScanningSystem.CopPeds.Where(x => x.isTasked || x.TaskIsQueued).Count() <= 3 && Cop.DistanceToPlayer <= 45f)
                {
                    Cop.TaskIsQueued = true;
                    PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Arrest));
                }
                else if (!Cop.TaskIsQueued && (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask || Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.Preparing) && (Cop.RecentlySeenPlayer() || Cop.DistanceToPlayer <= 65f))
                {
                    Cop.TaskIsQueued = true;
                    Cop.GameTimeLastTask = Game.GameTime;
                    PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.SimpleArrest));
                }
                else if (!Cop.TaskIsQueued && Game.GameTime - Cop.GameTimeLastTask > 3500 && Cop.RecentlySeenPlayer() && Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.InProgress && Cop.DistanceToPlayer > 45f)
                {
                    Cop.TaskIsQueued = true;
                    Cop.GameTimeLastTask = Game.GameTime;
                    PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.SimpleArrest)); //retask the arrest
                }
            }
        }
        Game.LocalPlayer.WantedLevel = MaxWantedLastLife;

        if (PoliceScanningSystem.CopPeds.Any(x => x.DistanceToPlayer <= 4f) && Game.LocalPlayer.Character.Speed <= 4.0f && !Game.LocalPlayer.Character.IsInAnyVehicle(false) && !isBusted)
            SurrenderBust = true;

        if (SurrenderBust && !isBustTimeOut())
            SurrenderBustEvent();

        StopSearchMode();
    }
    private static void PoliceTickCautiousChase()
    {
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => !x.isTasked && !x.isInVehicle && !x.isInHelicopter))
        {
            SetCopDeadly(Cop);
            if (!Cop.TaskIsQueued && PoliceScanningSystem.CopPeds.Where(x => x.isTasked || x.TaskIsQueued).Count() <= 2 && Cop.DistanceToPlayer <= 45f)
            {
                Cop.TaskIsQueued = true;
                PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Arrest));
            }
            else if (!Cop.TaskIsQueued && Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask && (Cop.RecentlySeenPlayer() || Cop.DistanceToPlayer <= 65f))
            {
                Cop.TaskIsQueued = true;
                Cop.GameTimeLastTask = Game.GameTime;
                PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.SimpleArrest));
            }
            else if (!Cop.TaskIsQueued && Game.GameTime - Cop.GameTimeLastTask > 3500 && Cop.RecentlySeenPlayer() && Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.InProgress && Cop.DistanceToPlayer > 35f)
            {
                Cop.TaskIsQueued = true;
                Cop.GameTimeLastTask = Game.GameTime;
                PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.SimpleArrest));
            }

        }
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.isTasked && x.SimpleTaskName != ""))
        {
            if (!Cop.TaskIsQueued && Game.GameTime - Cop.GameTimeLastTask > 20000 && Cop.RecentlySeenPlayer() && Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.InProgress && Cop.DistanceToPlayer > 25f)
            {
                Cop.TaskIsQueued = true;
                Cop.GameTimeLastTask = Game.GameTime;
                WriteToLog("PoliceTickCautiousChase", "Retasked Simple Arrest");
                PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.SimpleArrest));
            }
            else if (!Cop.TaskIsQueued && Game.GameTime - Cop.GameTimeLastTask > 20000 && !Cop.RecentlySeenPlayer() && Cop.DistanceToPlayer > 35f)
            {
                Cop.TaskIsQueued = true;
                WriteToLog("PoliceTickDeadlyChase", "Queued An Untask");
                PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
            }

        }
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => !x.isTasked && (x.isInHelicopter || x.isOnBike)))
        {
            SetUnarmed(Cop);
        }

        if (PoliceScanningSystem.CopPeds.Any(x => x.DistanceToPlayer <= 8f) && Game.LocalPlayer.Character.Speed <= 4.0f && !Game.LocalPlayer.Character.IsInAnyVehicle(false) && !isBusted)
            ForceSurrenderTime++;
        else
            ForceSurrenderTime = 0;

        if (ForceSurrenderTime >= 500)
            SurrenderBust = true;

        if (SurrenderBust && !isBustTimeOut())
            SurrenderBustEvent();

        StopSearchMode();
    }
    private static void PoliceTickDeadlyChase()
    {
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => !x.isInVehicle))
        {
            SetCopDeadly(Cop);
            if (!areHandsUp && !BeingArrested && !Cop.TaskIsQueued && Cop.isTasked)
            {
                Cop.TaskIsQueued = true;
                WriteToLog("PoliceTickDeadlyChase", "Queued An Untask");
                PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
            }
        }
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => !x.isTasked && x.isInHelicopter))
        {
            if (!areHandsUp && Game.LocalPlayer.WantedLevel >= 4)
                SetCopDeadly(Cop);
            else
                SetUnarmed(Cop);
        }

        if (PoliceScanningSystem.CopsKilledByPlayer >= 5 && Game.LocalPlayer.WantedLevel < 4)
        {
            Game.LocalPlayer.WantedLevel = 4;
            DispatchAudioSystem.AddDispatchToQueue(new DispatchAudioSystem.DispatchQueueItem(DispatchAudioSystem.ReportDispatch.ReportWeaponsFree, 2, false));
            WriteToLog("Value Checker", "Killed too many police, wanted level going up");
        }

        if (SurrenderBust && !isBustTimeOut())
            SurrenderBustEvent();
    }
    private static void PoliceVehicleTick()
    {
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.isInVehicle && !x.isInHelicopter))
        {
            //Cop.CopPed.VisionRange = 100f;

            //NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Cop.CopPed, 100f);
            //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Cop.CopPed, 8f);
            //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 32, true);


            //NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Cop.CopPed, 100f);

            //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 4, true);
            //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 8, true);
            // NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 16, true);
            ////NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 32, true);
            //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 512, true);
            //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 262144, true);
            // NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Cop.CopPed, 8f);

            if (CurrentPoliceState == PoliceState.DeadlyChase)
            {
                NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Cop.CopPed, 100f);

                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 4, true);
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 8, true);
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 16, true);
                //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 32, true);
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 512, true);
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 262144, true);
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Cop.CopPed, 8f);



                //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 262144, true);
                ////CreatePassengerCop(Cop);
                //NativeFunction.CallByName<bool>("SET_DRIVER_AGGRESSIVENESS", Cop.CopPed, 100f);
                ////NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Cop.CopPed, 0f);
            }
            else
            {
                NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Cop.CopPed, 100f);
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Cop.CopPed, 8f);
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 32, true);


                //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 32, true);
                //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Cop.CopPed, 8f);
            }
        }
    }
    private static void StolenVehiclesTick()
    {
        EnteredVehicles.RemoveAll(x => !x.VehicleEnt.Exists());
        if (Game.LocalPlayer.WantedLevel == 0)
        {
            foreach (GTAVehicle StolenCar in EnteredVehicles)
            {
                if (StolenCar.ShouldReportStolen)
                {
                    //StolenCar.WasReportedStolen = true;
                    StolenCar.QuedeReportedStolen = true;
                    WriteToLog("StolenVehicles", "ReportStolenVehicle");
                    // DispatchAudioSystem.ReportStolenVehicle(StolenCar);
                    DispatchAudioSystem.DispatchQueueItem StolenVehicle = new DispatchAudioSystem.DispatchQueueItem(DispatchAudioSystem.ReportDispatch.ReportStolenVehicle, 10, false);
                    StolenVehicle.VehicleToReport = StolenCar;
                    DispatchAudioSystem.AddDispatchToQueue(StolenVehicle);
                }
            }
        }

        if (PlayerInVehicle && AnyPoliceCanSeePlayer && !PlayerStarsGreyedOut)
        {
            if (EnteredVehicles.Any(x => x.IsStolen && x.WasReportedStolen && x.VehicleEnt.Handle == Game.LocalPlayer.Character.CurrentVehicle.Handle) && Game.LocalPlayer.WantedLevel < 2)
            {

                Game.LocalPlayer.WantedLevel = 2;
                //DispatchAudioSystem.ReportSpottedStolenCar(Game.LocalPlayer.Character.CurrentVehicle);
                DispatchAudioSystem.DispatchQueueItem StolenVehicle = new DispatchAudioSystem.DispatchQueueItem(DispatchAudioSystem.ReportDispatch.ReportSpottedStolenCar, 10, false);
                StolenVehicle.VehicleToReport = new GTAVehicle(Game.LocalPlayer.Character.CurrentVehicle,0,false,false,null,true,false);
                WriteToLog("StolenVehicles", "Caught In Stolen Vehicle");
            }
            if (Game.LocalPlayer.WantedLevel > 0)
            {
                foreach (GTAVehicle stolenVehicle in EnteredVehicles.Where(x => !x.WasReportedStolen))
                {
                    stolenVehicle.WasReportedStolen = true;
                    DispatchAudioSystem.DispatchQueueItem StolenVehicle = new DispatchAudioSystem.DispatchQueueItem(DispatchAudioSystem.ReportDispatch.ReportSpottedStolenCar, 10, false);
                    StolenVehicle.VehicleToReport = new GTAVehicle(Game.LocalPlayer.Character.CurrentVehicle, 0, false, false, null, true, false);
                    WriteToLog("StolenVehicles", "Cops Saw your stolen vehicle already on chase");
                }
            }
        }
    }
    private static void WantedLevelTick()
    {
        if (Game.GameTime - WantedLevelStartTime > 180000 && WantedLevelStartTime > 0 && AnyPoliceRecentlySeenPlayer && Game.LocalPlayer.WantedLevel > 0 && Game.LocalPlayer.WantedLevel <= 3)
        {
            Game.LocalPlayer.WantedLevel++;
            WriteToLog("WantedLevelStartTime", "Wanted Level Increased Over Time");
        }

        if (PlayerStarsGreyedOut && PoliceScanningSystem.CopPeds.All(x => !x.RecentlySeenPlayer()))
        {
            NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, PoliceScanningSystem.PlacePlayerLastSeen.X, PoliceScanningSystem.PlacePlayerLastSeen.Y, PoliceScanningSystem.PlacePlayerLastSeen.Z);
            if (CanReportLastSeen && Game.GameTime - GameTimeLastGreyedOut > 4000 && AnyPoliceSeenPlayerThisWanted)
            {
                WriteToLog("ReportSuspectLastSeen", "ReportSuspectLastSeen");
                //DispatchAudioSystem.ReportSuspectLastSeen(rnd.NextDouble() > 0.5);
                DispatchAudioSystem.AddDispatchToQueue(new DispatchAudioSystem.DispatchQueueItem(DispatchAudioSystem.ReportDispatch.ReportSuspectLastSeen, 10, false));
                CanReportLastSeen = false;
            }
        }

        if (AnyPoliceRecentlySeenPlayer)
        {
            NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z);
        }
    }
    private static void CheckPoliceEvents()
    {
        bool isJacking = Game.LocalPlayer.Character.IsJacking;
        if (PrevPlayerIsJacking != isJacking)
            PlayerJackingChanged(isJacking);

        if (PrevPlayerInVehicle != PlayerInVehicle)
            PlayerInVehicleChanged(PlayerInVehicle);

        if (PrevPlayerKilledPolice != PoliceScanningSystem.PlayerKilledPolice)
            PlayerKilledPoliceChanged();

        if (PrevCopsKilledByPlayer != PoliceScanningSystem.CopsKilledByPlayer)
            CopsKilledChanged();

        if (PrevfiredWeapon != firedWeapon)
            FiredWeaponChanged();

        if (PrevaimedAtPolice != aimedAtPolice)
            aimedAtPoliceChanged();

        if (PrevPlayerHurtPolice != PoliceScanningSystem.PlayerHurtPolice)
            PlayerHurtPoliceChanged();

        if (PrevPoliceState != CurrentPoliceState)
            PoliceStateChanged();

        if (PrevWantedLevel != Game.LocalPlayer.WantedLevel)
            WantedLevelChanged();

        if (PrevPlayerStarsGreyedOut != PlayerStarsGreyedOut)
            PlayerStarsGreyedOutChanged(AnyPoliceRecentlySeenPlayer);

        if (PrevPoliceState != CurrentPoliceState)
            PoliceStateChanged();
    }

    private static void SurrenderBustEvent()
    {
        //return;
        //if (!NativeFunction.Natives.x36AD3E690DA5ACEB<bool>("PeyoteIn"))//_GET_SCREEN_EFFECT_IS_ACTIVE
        //    NativeFunction.Natives.x068E835A1D0DC0E3("PeyoteIn", 0, 0);//_STOP_SCREEN_EFFECT
        BeingArrested = true;
        CurrentPoliceState = PoliceState.ArrestedWait;
        NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)2725352035, true);
        areHandsUp = false;
        SurrenderBust = false;
        LastBust = Game.GameTime;
        WriteToLog("SurrenderBust", "SurrenderBust Executed");
    }
    private static bool isBustTimeOut()
    {
        if (Game.GameTime - LastBust >= 10000)
            return false;
        else
            return true;
    }
    private static void WantedLevelChanged()
    {
        if (Game.LocalPlayer.WantedLevel == 0)//Just Removed
        {
            //NativeFunction.CallByName<bool>("SET_FAKE_WANTED_LEVEL", 0);
            if (AnyPoliceSeenPlayerThisWanted && MaxWantedLastLife > 0)
            {
                DispatchAudioSystem.AddDispatchToQueue(new DispatchAudioSystem.DispatchQueueItem(DispatchAudioSystem.ReportDispatch.ReportSuspectLost, 10, false));
                //DispatchAudioSystem.ReportSuspectLost();
            }
            CurrentPoliceState = PoliceState.Normal;
            AnyPoliceSeenPlayerThisWanted = false;
            ResetTrafficViolations();
            WantedLevelStartTime = 0;
            DispatchAudioSystem.ResetReportedItems();
        }
        NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z);
        WantedLevelStartTime = Game.GameTime;
        WriteToLog("ValueChecker", String.Format("WantedLevel Changed to: {0}", Game.LocalPlayer.WantedLevel));
        PrevWantedLevel = Game.LocalPlayer.WantedLevel;
    }
    private static void CopsKilledChanged()
    {
        WriteToLog("ValueChecker", String.Format("CopsKilledByPlayer Changed to: {0}", PoliceScanningSystem.CopsKilledByPlayer));
        PrevCopsKilledByPlayer = PoliceScanningSystem.CopsKilledByPlayer;
    }
    private static void PlayerHurtPoliceChanged()
    {
        WriteToLog("ValueChecker", String.Format("PlayerHurtPolice Changed to: {0}", PoliceScanningSystem.PlayerHurtPolice));
        if (PoliceScanningSystem.PlayerHurtPolice)
        {
            //DispatchAudioSystem.ReportAssualtOnOfficer();
            DispatchAudioSystem.AddDispatchToQueue(new DispatchAudioSystem.DispatchQueueItem(DispatchAudioSystem.ReportDispatch.ReportAssualtOnOfficer, 3, true));
        }
            
        PrevPlayerHurtPolice = PoliceScanningSystem.PlayerHurtPolice;
    }
    private static void PlayerKilledPoliceChanged()
    {
        WriteToLog("ValueChecker", String.Format("PlayerKilledPolice Changed to: {0}", PoliceScanningSystem.PlayerKilledPolice));
        if (PoliceScanningSystem.PlayerKilledPolice)
        {
            //DispatchAudioSystem.ReportOfficerDown();
            DispatchAudioSystem.AddDispatchToQueue(new DispatchAudioSystem.DispatchQueueItem(DispatchAudioSystem.ReportDispatch.ReportOfficerDown, 1, true));
        }
        PrevPlayerKilledPolice = PoliceScanningSystem.PlayerKilledPolice;
    }
    private static void FiredWeaponChanged()
    {
        WriteToLog("ValueChecker", String.Format("firedWeapon Changed to: {0}", firedWeapon));
        if (firedWeapon)
        {
            //DispatchAudioSystem.ReportShotsFired();
            DispatchAudioSystem.AddDispatchToQueue(new DispatchAudioSystem.DispatchQueueItem(DispatchAudioSystem.ReportDispatch.ReportShotsFired, 2, true));
        }
        PrevfiredWeapon = firedWeapon;
    }
    private static void PlayerInVehicleChanged(bool playerInVehicle)
    {
        if (playerInVehicle)
        {

        }
        else
        {

        }
        PlayerInVehicle = playerInVehicle;
        PrevPlayerInVehicle = playerInVehicle;

        WriteToLog("ValueChecker", String.Format("playerInVehicle Changed to: {0}", playerInVehicle));
    }
    private static void PlayerJackingChanged(bool isJacking)
    {
        PlayerIsJacking = isJacking;
        WriteToLog("ValueChecker", String.Format("PlayerIsJacking Changed to: {0}", PlayerIsJacking));
        if (PlayerIsJacking)
        {
            //JackedCurrentVehicle = true;
        }
        PrevPlayerIsJacking = PlayerIsJacking;
    }
    private static void PlayerStarsGreyedOutChanged(bool AnyRecentlySeen)
    {
        WriteToLog("ValueChecker", String.Format("PlayerStarsGreyedOut Changed to: {0}", PlayerStarsGreyedOut));
        if (PlayerStarsGreyedOut)
        {
            //DispatchAudioSystem.ReportSuspectLastSeen(true);
            CanReportLastSeen = true;
            GameTimeLastGreyedOut = Game.GameTime;
        }
        else
        {
            CanReportLastSeen = false;
        }
        //WriteToLog("ValueChecker", String.Format("GameTimeLastGreyedOut Changed to: {0}", GameTimeLastGreyedOut.ToString()));
        //WriteToLog("ValueChecker", String.Format("CanReportLastSeen Changed to: {0}", CanReportLastSeen));
        PrevPlayerStarsGreyedOut = PlayerStarsGreyedOut;
    }
    private static void aimedAtPoliceChanged()
    {
        WriteToLog("ValueChecker", String.Format("aimedAtPolice Changed to: {0}", aimedAtPolice));
        if (aimedAtPolice)
        {
            //DispatchAudioSystem.ReportThreateningWithFirearm();
            DispatchAudioSystem.AddDispatchToQueue(new DispatchAudioSystem.DispatchQueueItem(DispatchAudioSystem.ReportDispatch.ReportThreateningWithFirearm, 2, true));
        }
        PrevaimedAtPolice = aimedAtPolice;
    }
    private static void PoliceStateChanged()
    {
        WriteToLog("ValueChecker", String.Format("PoliceState Changed to: {0}", CurrentPoliceState));
        WriteToLog("ValueChecker", String.Format("PreviousPoliceState Changed to: {0}", PrevPoliceState));

        if (CurrentPoliceState == PoliceState.Normal)
        {
            ResetPoliceStats();
        }

        if (CurrentPoliceState == PoliceState.ArrestedWait)
        {

        }

        if (CurrentPoliceState == PoliceState.DeadlyChase)
        {
            if (PrevPoliceState != PoliceState.ArrestedWait)
            {
                DispatchAudioSystem.AddDispatchToQueue(new DispatchAudioSystem.DispatchQueueItem(DispatchAudioSystem.ReportDispatch.ReportLethalForceAuthorized, 1, true));
                //DispatchAudioSystem.ReportLethalForceAuthorized();
            }


                
        }

        PrevPoliceState = CurrentPoliceState;
    }
    internal static void KillPlayer()
    {
        Game.LocalPlayer.Character.Kill();
        //isDead = true;
    }
    private static void StopSearchMode()
    {
        if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
            return;
        if (!GhostCop.Exists())
        {
            CreateGhostCop();
        }


        if (GhostCopFollow && GhostCop != null)
        {
            GhostCop.Position = Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0f, 4f, 1f));
            GhostCop.Heading = Game.LocalPlayer.Character.Heading;
        }


        //if (Game.GameTime - GameTimeLastReTasked <= 2000)
        //    return;

        if (PoliceScanningSystem.CopPeds.Any(x => x.RecentlySeenPlayer()))//if (NativeFunction.CallByName<bool>("ARE_PLAYER_STARS_GREYED_OUT", Game.LocalPlayer) && PoliceScanningSystem.CopPeds.Any(x => x.RecentlySeenPlayer())) // Needed for the AI to keep the player in the wanted position
        {
            GhostCopFollow = true;
        }
        else
        {
            if (GhostCop != null)
                GhostCop.Position = new Vector3(0f, 0f, 0f);
            GhostCopFollow = false;
        }


        if (NativeFunction.CallByName<bool>("ARE_PLAYER_STARS_GREYED_OUT", Game.LocalPlayer) && !PoliceScanningSystem.CopPeds.Any(x => x.RecentlySeenPlayer()) && PoliceScanningSystem.CopPeds.Any(x => x.isTasked && !x.TaskIsQueued))
        {
            PoliceScanningSystem.UntaskAll();
        }
    }
    private static void CreateGhostCop()
    {
        GhostCop = new Ped(CopModel, Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0f, 4f, 0f)), Game.LocalPlayer.Character.Heading);
        GhostCop.BlockPermanentEvents = false;
        GhostCop.IsPersistent = true;
        GhostCop.IsCollisionEnabled = false;
        GhostCop.IsVisible = false;
        Blip myBlip = GhostCop.GetAttachedBlip();
        if (myBlip != null)
            myBlip.Delete();
        GhostCop.VisionRange = 100f;
        GhostCop.HearingRange = 100f;
        GhostCop.CanRagdoll = false;



        const ulong SetPedMute = 0x7A73D05A607734C7;
        NativeFunction.CallByHash<uint>(SetPedMute, GhostCop);


        NativeFunction.CallByName<bool>("STOP_PED_SPEAKING", GhostCop,true);



        NativeFunction.CallByName<uint>("SET_PED_CONFIG_FLAG", GhostCop, 69, true);

        NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", GhostCop, (uint)2725352035, true); //Unequip weapon so you don't get shot
        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", GhostCop, false);
        NativeFunction.CallByName<uint>("SET_PED_MOVE_RATE_OVERRIDE", GhostCop, 0f);
        GhostCopFollow = true;
        WriteToLog("CreateGhostCop", "Ghost Cop Created");
    }

    public static void SetUnarmed(GTACop Cop)
    {
        if (!Cop.CopPed.Exists() || (Cop.SetUnarmed && !Cop.NeedsWeaponCheck))
            return;
        Cop.CopPed.Accuracy = 10;
        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.CopPed, 0);
        if (!(Cop.CopPed.Inventory.EquippedWeapon == null))
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Cop.CopPed, (uint)2725352035, true); //Unequip weapon so you don't get shot
            NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.CopPed, false);
        }
        Cop.SetTazer = false;
        Cop.SetUnarmed = true;
        Cop.SetDeadly = false;
        Cop.GameTimeLastWeaponCheck = Game.GameTime;
    }
    public static void ResetCopWeapons(GTACop Cop)
    {
        if (!Cop.CopPed.Exists() || (!Cop.SetDeadly && !Cop.SetTazer && !Cop.SetUnarmed && !Cop.NeedsWeaponCheck))
            return;
        Cop.CopPed.Accuracy = 10;
        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.CopPed, 30);
        //Cop.CopPed.BlockPermanentEvents = false;
        if (!Cop.CopPed.Inventory.Weapons.Contains(Cop.IssuedPistol.Name))
            Cop.CopPed.Inventory.GiveNewWeapon(Cop.IssuedPistol.Name, -1, false);
        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.CopPed, true);
        Cop.SetTazer = false;
        Cop.SetUnarmed = false;
        Cop.SetDeadly = false;
        Cop.GameTimeLastWeaponCheck = Game.GameTime;
    }
    public static void SetCopDeadly(GTACop Cop)
    {
        if (!Cop.CopPed.Exists() || (Cop.SetDeadly && !Cop.NeedsWeaponCheck))
            return;
        Cop.CopPed.Accuracy = 10;
        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.CopPed, 30);
        //Cop.CopPed.BlockPermanentEvents = false;

        //TargetCop.Inventory.GiveNewWeapon(WeaponHash.Pistol, 100, true);


        if (!Cop.CopPed.Inventory.Weapons.Contains(Cop.IssuedPistol.Name))
            Cop.CopPed.Inventory.GiveNewWeapon(Cop.IssuedPistol.Name, -1, true);

        if ((Cop.CopPed.Inventory.EquippedWeapon == null || Cop.CopPed.Inventory.EquippedWeapon.Hash == WeaponHash.StunGun) && Game.LocalPlayer.WantedLevel >= 0)
            Cop.CopPed.Inventory.GiveNewWeapon(Cop.IssuedPistol.Name, -1, true);

        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.CopPed, true);
        Cop.SetTazer = false;
        Cop.SetUnarmed = false;
        Cop.SetDeadly = true;
        Cop.GameTimeLastWeaponCheck = Game.GameTime;
    }
    public static void SetCopTazer(GTACop Cop)
    {
        if (!Cop.CopPed.Exists() || (Cop.SetTazer && !Cop.NeedsWeaponCheck))
            return;

        Cop.CopPed.Accuracy = 30;
        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.CopPed, 100);
        //Cop.CopPed.Inventory.GiveNewWeapon(WeaponHash.StunGun, 100, true);
        if (!Cop.CopPed.Inventory.Weapons.Contains(WeaponHash.StunGun))
        {
            Cop.CopPed.Inventory.GiveNewWeapon(WeaponHash.StunGun, 100, true);
        }
        else if (Cop.CopPed.Inventory.EquippedWeapon != WeaponHash.StunGun)
        {
            Cop.CopPed.Inventory.EquippedWeapon = WeaponHash.StunGun;
        }
        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.CopPed, false);
        Cop.SetTazer = true;
        Cop.SetUnarmed = false;
        Cop.SetDeadly = false;
        Cop.GameTimeLastWeaponCheck = Game.GameTime;
    }

    private static void StateTick()
    {

        //Dead
        if (Game.LocalPlayer.Character.IsDead && !isDead)
        {
            PositionOfDeath = Game.LocalPlayer.Character.Position;
            DiedInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            isDead = true;
            NativeFunction.Natives.x2206BF9A37B7F724("DeathFailOut", 0, 0);//_START_SCREEN_EFFECT
            Game.LocalPlayer.Character.Kill();
            Game.LocalPlayer.Character.Health = 0;
            Game.LocalPlayer.Character.IsInvincible = true;
            Game.LocalPlayer.WantedLevel = 0;
            Game.TimeScale = .4f;
            Menus.deathMenu.Visible = true;

            if (PrevWantedLevel > 0 || PoliceScanningSystem.CopPeds.Any(x => x.isTasked))
            {
                //DispatchAudioSystem.ReportSuspectWasted();
                DispatchAudioSystem.AddDispatchToQueue(new DispatchAudioSystem.DispatchQueueItem(DispatchAudioSystem.ReportDispatch.ReportSuspectWasted, 5, false));
            }
        }

        // Busted
        if (NativeFunction.CallByName<bool>("IS_PLAYER_BEING_ARRESTED", 0))
        {
            BeingArrested = true;
        }
        if (NativeFunction.CallByName<bool>("IS_PLAYER_BEING_ARRESTED", 1))
        {
            BeingArrested = true;
            Game.LocalPlayer.Character.Tasks.Clear();
        }

        if (BeingArrested && !isBusted)
        {
            PositionOfDeath = Game.LocalPlayer.Character.Position;
            DiedInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            isBusted = true;
            BeingArrested = true;
            Game.LocalPlayer.Character.Tasks.Clear();
            NativeFunction.Natives.x2206BF9A37B7F724("DeathFailOut", 0, 0);//_START_SCREEN_EFFECT
            //MenuEnableDisable();
            Game.TimeScale = .4f;
            areHandsUp = false;
            Menus.bustedMenu.Visible = true;
            SetArrestedAnimation(Game.LocalPlayer.Character, false);
            //DispatchAudioSystem.ReportSuspectArrested();
            DispatchAudioSystem.AddDispatchToQueue(new DispatchAudioSystem.DispatchQueueItem(DispatchAudioSystem.ReportDispatch.ReportSuspectArrested, 5, false));
        }

        //NativeFunction.CallByName<uint>("DISPLAY_HUD", true);

        if (Game.LocalPlayer.WantedLevel > PreviousWantedLevel)
            PreviousWantedLevel = Game.LocalPlayer.WantedLevel;

        if (Game.LocalPlayer.WantedLevel > MaxWantedLastLife) // The max wanted level i saw in the last life, not just right before being busted
            MaxWantedLastLife = Game.LocalPlayer.WantedLevel;
        else if (Game.LocalPlayer.WantedLevel == 0 && MaxWantedLastLife > 0 && !isBusted && !isDead)
            MaxWantedLastLife = 0;


        if (Game.GameTime - GameTimeLastTakenOver <= 1000 && Game.LocalPlayer.WantedLevel > 0)
        {
            WriteToLog("ValueChecker", String.Format("Reset Wanted Level After Ped Takeover: {0}", GameTimeLastTakenOver));
            Game.LocalPlayer.WantedLevel = 0;
        }

    }
    private static void ControlTick()
    {
        if (Game.IsKeyDownRightNow(Keys.E) && !Game.LocalPlayer.IsFreeAiming && (!Game.LocalPlayer.Character.IsInAnyVehicle(false) || Game.LocalPlayer.Character.CurrentVehicle.Speed < 2.5f))
        {

            if (!areHandsUp && !isBusted)
            {
                if (!(Game.LocalPlayer.Character.Inventory.EquippedWeapon == null))
                    NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)2725352035, true); //Unequip weapon so you don't get shot
                HandsUpPreviousPoliceState = CurrentPoliceState;
                areHandsUp = true;
                //vehicleEngineSystem.TurnOffEngine();
                RaiseHands();
                if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Speed <= 10f)
                    Game.LocalPlayer.Character.CurrentVehicle.IsDriveable = false;
            }

        }
        else
        {
            if (areHandsUp && !isBusted)
            {
                areHandsUp = false; // You put your hands down
                CurrentPoliceState = HandsUpPreviousPoliceState;
                Game.LocalPlayer.Character.Tasks.Clear();
                if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
                    Game.LocalPlayer.Character.CurrentVehicle.IsDriveable = true;
            }
        }

        if(Game.IsKeyDownRightNow(Keys.G) && !DroppingWeapon && !PlayerInVehicle && Game.LocalPlayer.Character.isConsideredArmed())
        {
            DroppingWeapon = true;
            //Game.LocalPlayer.Character.Inventory.EquippedWeapon.DropToGround();
            //GameFiber.Sleep(500);
            DropWeaponAnimation();
            if (Game.LocalPlayer.Character.IsRunning)
                GameFiber.Sleep(500);
            else
                GameFiber.Sleep(250);

            NativeFunction.CallByName<bool>("SET_PED_DROPS_INVENTORY_WEAPON", Game.LocalPlayer.Character, (int)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash, 0.0f, 0.5f, 0.0f, -1);
            if (!(Game.LocalPlayer.Character.Inventory.EquippedWeapon == null))
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)2725352035, true);
            WriteToLog("DroppingWeapon", "Dropped your gun");
            DroppingWeapon = false;
        }
    }
    private static void RaiseHands()
    {
        if (Game.LocalPlayer.WantedLevel > 0 && PoliceScanningSystem.CopsKilledByPlayer < 5)
            CurrentPoliceState = PoliceState.ArrestedWait;

        bool inVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
        var sDict = (inVehicle) ? "veh@busted_std" : "ped";
        RequestAnimationDictionay(sDict);
        if (inVehicle)
        {
            NativeFunction.CallByName<bool>("ROLL_DOWN_WINDOW", Game.LocalPlayer.Character.CurrentVehicle, 0);
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, sDict, "stay_in_car_crim", 2.0f, -2.0f, -1, 50, 0, true, false, true);
            //GameFiber.Sleep(250);
            //NativeFunction.CallByName<bool>("SET_ENTITY_ANIM_CURRENT_TIME", Game.LocalPlayer.Character, sDict, "stay_in_car_crim", 0.5f);
        }
        else
        {
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, sDict, "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
        }

    }

    public static void RequestAnimationDictionay(String sDict)
    {
        NativeFunction.CallByName<bool>("REQUEST_ANIM_DICT", sDict);
        while (!NativeFunction.CallByName<bool>("HAS_ANIM_DICT_LOADED", sDict))
            GameFiber.Yield();
    }
    public static void RespawnInPlace(bool AsOldCharacter)
    {
        try
        {
            isDead = false;
            isBusted = false;
            BeingArrested = false;
            Game.LocalPlayer.Character.Health = 100;
            if (DiedInVehicle)
            {
                NativeFunction.Natives.xB69317BF5E782347(Game.LocalPlayer.Character);
                //NativeFunction.CallByName<uint>("NETWORK_REQUEST_CONTROL_OF_ENTITY", Game.LocalPlayer.Character);      
                NativeFunction.Natives.xEA23C49EAA83ACFB(Game.LocalPlayer.Character.Position.X + 10f, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z, 0, false, false);
                //NativeFunction.CallByName<uint>("NETWORK_RESURRECT_LOCAL_PLAYER", Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z, 0, false, false);
                //NativeFunction.Natives.NetworkResurrectLocalPlayer(Game.LocalPlayer.Character.Position.X + 10F, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z, Camera.RenderingCamera.Direction, false, false);
                NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
                if (Game.LocalPlayer.Character.LastVehicle.Exists())
                {
                    Game.LocalPlayer.Character.WarpIntoVehicle(Game.LocalPlayer.Character.LastVehicle, -1);
                }
                NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
            }
            else
            {
                NativeFunction.Natives.xB69317BF5E782347(Game.LocalPlayer.Character);
                //NativeFunction.CallByName<uint>("NETWORK_REQUEST_CONTROL_OF_ENTITY", Game.LocalPlayer.Character);
                NativeFunction.Natives.xEA23C49EAA83ACFB(Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z, 0, false, false);
                //NativeFunction.CallByName<uint>("NETWORK_RESURRECT_LOCAL_PLAYER", Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z, 0, false, false);
                NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
            }
            if (AsOldCharacter)
            {
                //MaxWantedLastLife = 0;
                Game.LocalPlayer.WantedLevel = MaxWantedLastLife;
                ++TimesDied;
            }
            else
            {
                Game.LocalPlayer.Character.Inventory.Weapons.Clear();
                Game.LocalPlayer.Character.Inventory.GiveNewWeapon(2725352035, 0, true);
                // Game.LocalPlayer.Character.Inventory.Weapons.Clear();
                //Game.LocalPlayer.Character.Inventory.GiveNewWeapon(WeaponDescriptor., 0, true, true);
                //if (mySettings.ReplacePlayerWithPed && mySettings.ReplacePlayerWithPedRandomMoney)
                //    Game.Player.Character.SetCash(0, mySettings.ReplacePlayerWithPedCharacter);
                PreviousWantedLevel = 0;
                Game.LocalPlayer.WantedLevel = 0;
                TimesDied = 0;
                MaxWantedLastLife = 0;
                ResetPoliceStats();
                DispatchAudioSystem.ResetReportedItems();
            }
            Game.TimeScale = 1f;
            DiedInVehicle = false;
            NativeFunction.Natives.xB4EDDC19532BFB85(); //_STOP_ALL_SCREEN_EFFECTS
            ResetPlayer(false, false);
            Game.HandleRespawn();

        }
        catch (Exception e)
        {
            Game.LogTrivial(e.Message);
            // UI.Notify(e.Message);
        }
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

            if (TargetPed.Model.Hash == 225514697)
                WriteToLog("TakeoverPed", "TargetPed.Model.Hash: " + TargetPed.Model.Hash.ToString());
            else
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
                //AllyPedsToPlayer(World.GetNearbyPeds(Game.Player.Character.Position, 3f));
            }

            NativeFunction.CallByName<uint>("CHANGE_PLAYER_PED", Game.LocalPlayer, TargetPed, false, false);

            if (DeleteOld)
                CurrentPed.Delete();
            //else if (ArrestOld)
            //    SetArrestedAnimation(CurrentPed, true);
            //else
            //    AITakeoverPlayer(CurrentPed);



            NativeFunction.Natives.x2206BF9A37B7F724("MinigameTransitionOut", 5000, false);


            SetPlayerOffset();
            ChangeModel("player_zero");
            ChangeModel(LastModelHash);


            if (!Game.LocalPlayer.Character.isMainCharacter())
                ReplacePedComponentVariation(Game.LocalPlayer.Character);

            if (wasInVehicle)
            {
                Game.LocalPlayer.Character.WarpIntoVehicle(carWasIn, -1);
                NativeFunction.CallByName<bool>("SET_VEHICLE_HAS_BEEN_OWNED_BY_PLAYER", Game.LocalPlayer.Character.CurrentVehicle, true);
                OwnedCar = carWasIn;
            }
            else
            {
                Game.LocalPlayer.Character.IsCollisionEnabled = true;
            }

            Game.LocalPlayer.Character.SetCash(rnd.Next(500, 4000), "Michael");


            Game.LocalPlayer.Character.Inventory.Weapons.Clear();
            Game.LocalPlayer.Character.Inventory.GiveNewWeapon(2725352035, 0, true);
            TimesDied = 0;
            MaxWantedLastLife = 0;
            Game.TimeScale = 1f;
            Game.LocalPlayer.WantedLevel = 0;
            NativeFunction.Natives.xB4EDDC19532BFB85();
            Game.HandleRespawn();
            isDead = false;
            isBusted = false;
            CurrentPoliceState = PoliceState.Normal;
            BeingArrested = false;
            ResetTrafficViolations();
            ResetPoliceStats();
            GameTimeLastTakenOver = Game.GameTime;
        }
        catch (Exception e3)
        {
            WriteToLog("TakeoverPed", "TakeoverPed Error; " + e3.Message);
        }
    }
    private static void CopyPedComponentVariation(Ped myPed)
    {
        try
        {
            myPedVariation = new PedVariation();
            myPedVariation.myPedComponents = new List<PedComponent>();
            myPedVariation.myPedProps = new List<PropComponent>();
            for (int ComponentNumber = 0; ComponentNumber < 12; ComponentNumber++)
            {
                myPedVariation.myPedComponents.Add(new PedComponent(ComponentNumber, NativeFunction.CallByName<int>("GET_PED_DRAWABLE_VARIATION", myPed, ComponentNumber), NativeFunction.CallByName<int>("GET_PED_TEXTURE_VARIATION", myPed, ComponentNumber), NativeFunction.CallByName<int>("GET_PED_PALETTE_VARIATION", myPed, ComponentNumber)));
            }
            for (int PropNumber = 0; PropNumber < 8; PropNumber++)
            {
                myPedVariation.myPedProps.Add(new PropComponent(PropNumber, NativeFunction.CallByName<int>("GET_PED_PROP_INDEX", myPed, PropNumber), NativeFunction.CallByName<int>("GET_PED_PROP_TEXTURE_INDEX", myPed, PropNumber)));
            }
        }
        catch (Exception e)
        {
            WriteToLog("CopyPedComponentVariation", "CopyPedComponentVariation Error; " + e.Message);
        }
    }
    private static void ReplacePedComponentVariation(Ped myPed)
    {
        try
        {
            foreach (PedComponent Component in myPedVariation.myPedComponents)
            {
                NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", myPed, Component.ComponentID, Component.DrawableID, Component.TextureID, Component.PaletteID);
            }
            foreach (PropComponent Prop in myPedVariation.myPedProps)
            {
                NativeFunction.CallByName<uint>("SET_PED_PROP_INDEX", myPed, Prop.PropID, Prop.DrawableID, Prop.TextureID, false);
            }
        }
        catch (Exception e)
        {
            WriteToLog("ReplacePedComponentVariation", "ReplacePedComponentVariation Error; " + e.Message);
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

        //if (mySettings.ReplacePlayerWithPedCharacter == "Michael")
        GTA.Write<uint>(Player + SECOND_OFFSET, 225514697, new int[] { THIRD_OFFSET });
        //else if (mySettings.ReplacePlayerWithPedCharacter == "Franklin")
        //    GTA.Write<uint>(Player + SECOND_OFFSET, 2602752943, new int[] { THIRD_OFFSET });
        //else if (mySettings.ReplacePlayerWithPedCharacter == "Trevor")
        //    GTA.Write<uint>(Player + SECOND_OFFSET, 2608926626, new int[] { THIRD_OFFSET });

    }
    private static void ChangeModel(String ModelRequested)
    {
        // Request the character model
        Model characterModel = new Model(ModelRequested);
        characterModel.LoadAndWait();
        characterModel.LoadCollisionAndWait();
        //while (!characterModel.IsCollisionLoaded)
        //{
        //    GameFiber.Yield();
        //}
        //Game.LocalPlayer.Model.ch
        Game.LocalPlayer.Model = characterModel;
        Game.LocalPlayer.Character.IsCollisionEnabled = true;
        //characterModel.Request(500);
        //// Check the model is valid
        //if (characterModel.IsInCdImage && characterModel.IsValid)
        //{
        //    // If the model isn't loaded, wait until it is   
        //    while (!characterModel.IsLoaded)
        //    {
        //        Script.Wait(100);
        //    }
        //    // Set the player's model    
        //    Function.Call(Hash.SET_PLAYER_MODEL, Game.Player, characterModel.Hash);
        //}
        //// Delete the model from memory after we've assigned it
        //characterModel.MarkAsNoLongerNeeded();
    }
    private static void SetArrestedAnimation(Ped PedToArrest, bool MarkAsNoLongerNeeded)
    {
        GameFiber.StartNew(delegate
        {
            RequestAnimationDictionay("veh@busted_std");
            RequestAnimationDictionay("busted");
            RequestAnimationDictionay("ped");

            if (!PedToArrest.Exists())
                return;

            while (PedToArrest.IsRagdoll || PedToArrest.IsStunned)
                GameFiber.Yield();

            if (!PedToArrest.Exists())
                return;

            if (PedToArrest.IsInAnyVehicle(false))
            {
                Vehicle oldVehicle = PedToArrest.CurrentVehicle;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "veh@busted_std", "get_out_car_crim", 2.0f, -2.0f, 2500, 50, 0, false, false, false);
                GameFiber.Sleep(2500);
                if (PedToArrest.Exists() && oldVehicle.Exists())
                {
                    WriteToLog("SetArrestedAnimation", "Tasked to leave the vehicle");
                    NativeFunction.CallByName<uint>("TASK_LEAVE_VEHICLE", PedToArrest, oldVehicle, 256);
                    GameFiber.Sleep(2500);
                }
            }
            if (PedToArrest == Game.LocalPlayer.Character && !isBusted)
                return;


            if (MaxWantedLastLife < 3)
            {
                NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
            }
            else
            {
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_2_hands_up", 2.0f, -8.0f, 5000, 2, 0, false, false, false);
                GameFiber.Sleep(5000);
                if (!PedToArrest.Exists() || (PedToArrest == Game.LocalPlayer.Character && !isBusted))
                    return;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_a", 8.0f, -8.0f, -1, 1, 0, false, false, false);
            }
            PedToArrest.KeepTasks = true;

            if (MarkAsNoLongerNeeded)
                PedToArrest.IsPersistent = false;
        });

    }
    private static void DropWeaponAnimation()
    {
        GameFiber.StartNew(delegate
        {
            RequestAnimationDictionay("pickup_object");
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "pickup_object", "pickup_low", 8.0f, -2.0f, -1, 56, 0, false, false, false);

        });
        //Function.Call(Hash.TASK_PLAY_ANIM, Game.Player.Character, sDict, "putdown_low", 8.0, -8.0, -1, 50, 1.0, false, false, false);
    }
    private static void UnSetArrestedAnimation(Ped PedToArrest)
    {
        GameFiber.StartNew(delegate
        {
            RequestAnimationDictionay("random@arrests");
            RequestAnimationDictionay("busted");
            RequestAnimationDictionay("ped");

            if (NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_a", 1) || NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_2_hands_up", 1))
            {
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "random@arrests", "kneeling_arrest_escape", 8.0f, -8.0f, -1, 4096, 0, 0, 1, 0);
            }
            else if(NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "ped", "handsup_enter", 1))
            {
                PedToArrest.Tasks.Clear();
            }
        });
    }
    public static void CommitSuicide(Ped PedToSuicide)
    {
        GameFiber.StartNew(delegate
        {
            if (!PedToSuicide.IsInAnyVehicle(false))
            {
                RequestAnimationDictionay("mp_suicide");
                if (PedToSuicide.Inventory.EquippedWeapon != null && PedToSuicide.Inventory.EquippedWeapon.Hash == WeaponHash.Pistol)
                {
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToSuicide, "mp_suicide", "pistol", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                    GameFiber.Wait(750);
                }
                else
                {
                    NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", PedToSuicide, (uint)2725352035, true);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToSuicide, "mp_suicide", "pill", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                    GameFiber.Wait(2500);
                }
            }
            PedToSuicide.Kill();
        });
    }
    public static bool RadioIn()
    {
        GTACop Cop = PoliceScanningSystem.PrimaryPursuer;
        if (Cop == null)
            return false;
        else
        {
            GameFiber TaskFiber =
            GameFiber.StartNew(delegate
            {
                RequestAnimationDictionay("random@arrests");
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Cop.CopPed, "random@arrests", "generic_radio_enter", 2.0f, -2.0f, 2500, 16, 0, false, false, false);

            });
            GameFiber AudioFiber =
            GameFiber.StartNew(delegate
            {
                Cop.CopPed.PlayAmbientSpeech("GENERIC_FRIGHTENED_MED");
            });

            Cop.isTasked = false; //Doing the animation clears out his other tasks?
            while (AudioFiber.IsAlive && TaskFiber.IsAlive)
                GameFiber.Yield();

            if (Cop.CopPed.Exists() && !Cop.CopPed.IsDead)
                return true; //successfully radioed it in
            else
                return false; //died beforehand
        }

    }
    public static void RespawnAtHospital()
    {
        Game.FadeScreenOut(1500);
        GameFiber.Wait(1500);
        isDead = false;
        isBusted = false;
        CurrentPoliceState = PoliceState.Normal;

        Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        RespawnInPlace(true);
        Game.LocalPlayer.WantedLevel = 0;
        EmergencyLocation ClosestPolice = EmergencyLocations.Where(x => x.Type == EmergencyLocation.EmergencyLocationType.Hospital).OrderBy(s => Game.LocalPlayer.Character.Position.DistanceTo2D(s.Location)).FirstOrDefault();

        Game.LocalPlayer.Character.Position = ClosestPolice.Location;
        Game.LocalPlayer.Character.Heading = ClosestPolice.Heading;


        GameFiber.Wait(1500);
        Game.FadeScreenIn(1500);

        if (Settings.ReplacePlayerWithPedRandomMoney)
        {
            int CurrentCash = Game.LocalPlayer.Character.GetCash(Settings.ReplacePlayerWithPedCharacter);
            if (CurrentCash < 5000)
                Game.LocalPlayer.Character.SetCash(0, Settings.ReplacePlayerWithPedCharacter); // Stop it from removing and adding cash, you cant go negative.
            else
                Game.LocalPlayer.Character.GiveCash(-5000, Settings.ReplacePlayerWithPedCharacter);
        }
    }
    public static void ResistArrest()
    {
        isBusted = false;
        BeingArrested = false;
        areHandsUp = false;
        CurrentPoliceState = PoliceState.DeadlyChase;
        UnSetArrestedAnimation(Game.LocalPlayer.Character);
        NativeFunction.CallByName<uint>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
        ResetPlayer(false, false);
        PoliceScanningSystem.UntaskAll();
    }
    public static void Surrender()
    {
        Game.FadeScreenOut(2500);
        GameFiber.Wait(2500);
        BeingArrested = false;
        isBusted = false;
        Game.LocalPlayer.WantedLevel = 0;
        RaiseHands();
        NativeFunction.CallByName<bool>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
        EmergencyLocation ClosestPolice = EmergencyLocations.Where(x => x.Type == EmergencyLocation.EmergencyLocationType.Police).OrderBy(s => Game.LocalPlayer.Character.Position.DistanceTo2D(s.Location)).FirstOrDefault();
        Game.LocalPlayer.Character.Position = ClosestPolice.Location;
        Game.LocalPlayer.Character.Heading = ClosestPolice.Heading;
        Game.LocalPlayer.Character.Tasks.ClearImmediately();
        Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        Game.LocalPlayer.Character.Inventory.GiveNewWeapon((WeaponHash)2725352035, -1, true);
        ResetPlayer(true, true);

        CurrentPoliceState = PoliceState.Normal;
        Game.FadeScreenIn(2500);
        Game.DisplayNotification("You are out on bail, try to stay out of trouble");
        if (Settings.ReplacePlayerWithPedRandomMoney)
            Game.LocalPlayer.Character.GiveCash(MaxWantedLastLife * -750, Settings.ReplacePlayerWithPedCharacter);

        PoliceScanningSystem.UntaskAll();
    }
    public static void BribePolice(int Amount)
    {
        if (Amount < PreviousWantedLevel * 500)
        {
            Game.DisplayNotification("Thats it? Thanks for the cash, but you're going downtown.");
            Game.LocalPlayer.Character.GiveCash(-1 * Amount, Settings.ReplacePlayerWithPedCharacter);
            return;
        }
        else
        {
            BeingArrested = false;
            isBusted = false;
            Game.DisplayNotification("Thanks for the cash, now beat it.");
            Game.LocalPlayer.Character.GiveCash(-1 * Amount, Settings.ReplacePlayerWithPedCharacter);
        }
        CurrentPoliceState = PoliceState.Normal;
        UnSetArrestedAnimation(Game.LocalPlayer.Character);
        NativeFunction.CallByName<bool>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
        if (Game.LocalPlayer.Character.LastVehicle.Exists())
            NativeFunction.CallByName<bool>("SET_VEHICLE_HAS_BEEN_OWNED_BY_PLAYER", Game.LocalPlayer.Character.LastVehicle, true);
        ResetPlayer(true, false);

        PoliceScanningSystem.UntaskAll();

    }
    public static void WriteToLog(String ProcedureString, String TextToLog)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + ProcedureString + ": " + TextToLog + System.Environment.NewLine);
        File.AppendAllText("Plugins\\InstantAction\\" + "log.txt", sb.ToString());
        sb.Clear();
    }
    public static void ResetPlayer(bool ClearWanted, bool ResetHealth)
    {
        isDead = false;
        isBusted = false;
        BeingArrested = false;
        firedWeapon = false;
        aimedAtPolice = false;
        CaughtWithWeapon = false;
        PoliceScanningSystem.PlayerHurtPolice = false;
        PoliceScanningSystem.PlayerKilledPolice = false;
        ResetTrafficViolations();
        ResetPoliceStats();
        NativeFunction.CallByName<bool>("NETWORK_REQUEST_CONTROL_OF_ENTITY", Game.LocalPlayer.Character);
        NativeFunction.Natives.xC0AA53F866B3134D();
        Game.TimeScale = 1f;
        if (ClearWanted)
            Game.LocalPlayer.WantedLevel = 0;

        //ResetCamera();
        NativeFunction.Natives.xB4EDDC19532BFB85(); //_STOP_ALL_SCREEN_EFFECTS;
        if (ResetHealth)
            Game.LocalPlayer.Character.Health = 100;
    }
    private static void setupLocations()
    {

        EmergencyLocation PillBoxHillHospital = new EmergencyLocation(new Vector3(364.7124f, -583.1641f, 28.69318f), 280.637f, EmergencyLocation.EmergencyLocationType.Hospital, "Pill Box Hill Hospital");
        EmergencyLocation CentralLosStantosHospital = new EmergencyLocation(new Vector3(338.208f, -1396.154f, 32.50927f), 77.07102f, EmergencyLocation.EmergencyLocationType.Hospital, "Central Los Santos Hospital");
        EmergencyLocation SandyShoresHospital = new EmergencyLocation(new Vector3(1842.057f, 3668.679f, 33.67996f), 228.3818f, EmergencyLocation.EmergencyLocationType.Hospital, "Sandy Shores Hospital");
        EmergencyLocation PaletoBayHospital = new EmergencyLocation(new Vector3(-244.3214f, 6328.575f, 32.42618f), 219.7734f, EmergencyLocation.EmergencyLocationType.Hospital, "Paleto Bay Hospital");

        EmergencyLocations.Add(PillBoxHillHospital);
        EmergencyLocations.Add(CentralLosStantosHospital);
        EmergencyLocations.Add(SandyShoresHospital);
        EmergencyLocations.Add(PaletoBayHospital);


        EmergencyLocation DavisPolice = new EmergencyLocation(new Vector3(358.9726f, -1582.881f, 29.29195f), 323.5287f, EmergencyLocation.EmergencyLocationType.Police, "Davis Police Station");
        EmergencyLocation SandyShoresPolice = new EmergencyLocation(new Vector3(1858.19f, 3679.873f, 33.75724f), 218.3256f, EmergencyLocation.EmergencyLocationType.Police, "Sandy Shores Police Station");
        EmergencyLocation PaletoBayPolice = new EmergencyLocation(new Vector3(-437.973f, 6021.403f, 31.49011f), 316.3756f, EmergencyLocation.EmergencyLocationType.Police, "Paleto Bay Police Station");
        EmergencyLocation MissionRowPolice = new EmergencyLocation(new Vector3(440.0835f, -982.3911f, 30.68966f), 47.88088f, EmergencyLocation.EmergencyLocationType.Police, "Mission Row Police Station");
        EmergencyLocation LasMesaPolice = new EmergencyLocation(new Vector3(815.8774f, -1290.531f, 26.28391f), 74.91704f, EmergencyLocation.EmergencyLocationType.Police, "La Mesa Police Station");
        EmergencyLocation VinewoodPolice = new EmergencyLocation(new Vector3(642.1356f, -3.134667f, 82.78872f), 215.299f, EmergencyLocation.EmergencyLocationType.Police, "Vinewood Police Station");
        EmergencyLocation RockfordHillsPolice = new EmergencyLocation(new Vector3(-557.0687f, -134.7315f, 38.20231f), 214.5968f, EmergencyLocation.EmergencyLocationType.Police, "Vinewood Police Station");
        EmergencyLocation VespucciPolice = new EmergencyLocation(new Vector3(-1093.817f, -807.1993f, 19.28864f), 22.23846f, EmergencyLocation.EmergencyLocationType.Police, "Vinewood Police Station");

        EmergencyLocations.Add(DavisPolice);
        EmergencyLocations.Add(SandyShoresPolice);
        EmergencyLocations.Add(PaletoBayPolice);
        EmergencyLocations.Add(MissionRowPolice);
        EmergencyLocations.Add(LasMesaPolice);
        EmergencyLocations.Add(VinewoodPolice);
        EmergencyLocations.Add(RockfordHillsPolice);
        EmergencyLocations.Add(VespucciPolice);

    }
    private static void setupWeapons()
    {
        //Melee
        Weapons.Add(new GTAWeapon("weapon_dagger", 0, "MELEE", 0, 2460120199));
        Weapons.Add(new GTAWeapon("weapon_bat", 0, "MELEE", 0, 2508868239));
        Weapons.Add(new GTAWeapon("weapon_bottle", 0, "MELEE", 0, 4192643659));
        Weapons.Add(new GTAWeapon("weapon_crowbar", 0, "MELEE", 0, 2227010557));
        Weapons.Add(new GTAWeapon("weapon_flashlight", 0, "MELEE", 0, 2343591895));
        Weapons.Add(new GTAWeapon("weapon_golfclub", 0, "MELEE", 0, 1141786504));
        Weapons.Add(new GTAWeapon("weapon_hammer", 0, "MELEE", 0, 1317494643));
        Weapons.Add(new GTAWeapon("weapon_hatchet", 0, "MELEE", 0, 4191993645));
        Weapons.Add(new GTAWeapon("weapon_knuckle", 0, "MELEE", 0, 3638508604));
        Weapons.Add(new GTAWeapon("weapon_knife", 0, "MELEE", 0, 2578778090));
        Weapons.Add(new GTAWeapon("weapon_machete", 0, "MELEE", 0, 3713923289));
        Weapons.Add(new GTAWeapon("weapon_switchblade", 0, "MELEE", 0, 3756226112));
        Weapons.Add(new GTAWeapon("weapon_nightstick", 0, "MELEE", 0, 1737195953));
        Weapons.Add(new GTAWeapon("weapon_wrench", 0, "MELEE", 0, 0x19044EE0));
        Weapons.Add(new GTAWeapon("weapon_battleaxe", 0, "MELEE", 0, 3441901897));
        Weapons.Add(new GTAWeapon("weapon_poolcue", 0, "MELEE", 0, 0x94117305));
        Weapons.Add(new GTAWeapon("weapon_stone_hatchet", 0, "MELEE", 0, 0x3813FC08));
        //Pistol
        Weapons.Add(new GTAWeapon("weapon_pistol", 60, "PISTOL", 1, 453432689,true));
        Weapons.Add(new GTAWeapon("weapon_pistol_mk2", 60, "PISTOL", 1, 0xBFE256D4,true));
        Weapons.Add(new GTAWeapon("weapon_combatpistol", 60, "PISTOL", 1, 1593441988,true));
        Weapons.Add(new GTAWeapon("weapon_appistol", 60, "PISTOL", 1, 584646201));
        Weapons.Add(new GTAWeapon("weapon_stungun", 0, "PISTOL", 1, 911657153));
        Weapons.Add(new GTAWeapon("weapon_pistol50", 60, "PISTOL", 1, 2578377531));
        Weapons.Add(new GTAWeapon("weapon_snspistol", 60, "PISTOL", 1, 3218215474));
        Weapons.Add(new GTAWeapon("weapon_snspistol_mk2", 60, "PISTOL", 1, 0x88374054));
        Weapons.Add(new GTAWeapon("weapon_heavypistol", 60, "PISTOL", 1, 3523564046,true));
        Weapons.Add(new GTAWeapon("weapon_vintagepistol", 60, "PISTOL", 1, 137902532));
        Weapons.Add(new GTAWeapon("weapon_flaregun", 60, "PISTOL", 1, 1198879012));
        Weapons.Add(new GTAWeapon("weapon_marksmanpistol", 60, "PISTOL", 1, 3696079510));
        Weapons.Add(new GTAWeapon("weapon_revolver", 60, "PISTOL", 1, 3249783761));
        Weapons.Add(new GTAWeapon("weapon_revolver_mk2", 60, "PISTOL", 1, 0xCB96392F));
        Weapons.Add(new GTAWeapon("weapon_doubleaction", 60, "PISTOL", 1, 0x97EA20B8));
        Weapons.Add(new GTAWeapon("weapon_raypistol", 60, "PISTOL", 1, 0xAF3696A1));
        //Shotgun
        Weapons.Add(new GTAWeapon("weapon_pumpshotgun", 32, "SHOTGUN", 2, 487013001,true));
        Weapons.Add(new GTAWeapon("weapon_pumpshotgun_mk2", 32, "SHOTGUN", 2, 0x555AF99A,true));
        Weapons.Add(new GTAWeapon("weapon_sawnoffshotgun", 32, "SHOTGUN", 2, 2017895192));
        Weapons.Add(new GTAWeapon("weapon_assaultshotgun", 32, "SHOTGUN", 2, 3800352039,true));
        Weapons.Add(new GTAWeapon("weapon_bullpupshotgun", 32, "SHOTGUN", 2, 2640438543));
        Weapons.Add(new GTAWeapon("weapon_musket", 32, "SHOTGUN", 2, 2828843422));
        Weapons.Add(new GTAWeapon("weapon_heavyshotgun", 32, "SHOTGUN", 2, 984333226));
        Weapons.Add(new GTAWeapon("weapon_dbshotgun", 32, "SHOTGUN", 2, 4019527611));
        Weapons.Add(new GTAWeapon("weapon_autoshotgun", 32, "SHOTGUN", 2, 317205821));
        //SMG
        Weapons.Add(new GTAWeapon("weapon_microsmg", 32, "SMG", 2, 324215364));
        Weapons.Add(new GTAWeapon("weapon_smg", 32, "SMG", 2, 736523883));
        Weapons.Add(new GTAWeapon("weapon_smg_mk2", 32, "SMG", 2, 0x78A97CD0));
        Weapons.Add(new GTAWeapon("weapon_assaultsmg", 32, "SMG", 2, 4024951519));
        Weapons.Add(new GTAWeapon("weapon_combatpdw", 32, "SMG", 2, 171789620,true));
        Weapons.Add(new GTAWeapon("weapon_machinepistol", 32, "SMG", 2, 3675956304));
        Weapons.Add(new GTAWeapon("weapon_minismg", 32, "SMG", 2, 3173288789));
        Weapons.Add(new GTAWeapon("weapon_raycarbine", 32, "SMG", 2, 0x476BF155));
        //AR
        Weapons.Add(new GTAWeapon("weapon_assaultrifle", 120, "AR", 3, 3220176749));
        Weapons.Add(new GTAWeapon("weapon_assaultrifle_mk2", 120, "AR", 3, 0x394F415C));
        Weapons.Add(new GTAWeapon("weapon_carbinerifle", 120, "AR", 3, 2210333304,true));
        Weapons.Add(new GTAWeapon("weapon_carbinerifle_mk2", 120, "AR", 3, 0xFAD1F1C9,true));
        Weapons.Add(new GTAWeapon("weapon_advancedrifle", 120, "AR", 3, 2937143193));
        Weapons.Add(new GTAWeapon("weapon_specialcarbine", 120, "AR", 3, 3231910285));
        Weapons.Add(new GTAWeapon("weapon_specialcarbine_mk2", 120, "AR", 3, 0x969C3D67));
        Weapons.Add(new GTAWeapon("weapon_bullpuprifle", 120, "AR", 3, 2132975508));
        Weapons.Add(new GTAWeapon("weapon_bullpuprifle_mk2", 120, "AR", 3, 0x84D6FAFD));
        Weapons.Add(new GTAWeapon("weapon_compactrifle", 120, "AR", 3, 1649403952));
        //LMG
        Weapons.Add(new GTAWeapon("weapon_mg", 200, "LMG", 4, 2634544996));
        Weapons.Add(new GTAWeapon("weapon_combatmg", 200, "LMG", 4, 2144741730));
        Weapons.Add(new GTAWeapon("weapon_combatmg_mk2", 200, "LMG", 4, 0xDBBD7280));
        Weapons.Add(new GTAWeapon("weapon_gusenberg", 200, "LMG", 4, 1627465347));
        //Sniper
        Weapons.Add(new GTAWeapon("weapon_sniperrifle", 40, "SNIPER", 4, 100416529));
        Weapons.Add(new GTAWeapon("weapon_heavysniper", 40, "SNIPER", 4, 205991906));
        Weapons.Add(new GTAWeapon("weapon_heavysniper_mk2", 40, "SNIPER", 4, 0xA914799));
        Weapons.Add(new GTAWeapon("weapon_marksmanrifle", 40, "SNIPER", 4, 3342088282));
        Weapons.Add(new GTAWeapon("weapon_marksmanrifle_mk2", 40, "SNIPER", 4, 0x6A6C02E0));
        //Heavy
        Weapons.Add(new GTAWeapon("weapon_rpg", 3, "HEAVY", 4, 2982836145));
        Weapons.Add(new GTAWeapon("weapon_grenadelauncher", 32, "HEAVY", 4, 2726580491));
        Weapons.Add(new GTAWeapon("weapon_grenadelauncher_smoke", 32, "HEAVY", 4, 1305664598));
        Weapons.Add(new GTAWeapon("weapon_minigun", 500, "HEAVY", 4, 1119849093));
        Weapons.Add(new GTAWeapon("weapon_firework", 20, "HEAVY", 4, 0x7F7497E5));
        Weapons.Add(new GTAWeapon("weapon_railgun", 50, "HEAVY", 4, 0x6D544C99));
        Weapons.Add(new GTAWeapon("weapon_hominglauncher", 3, "HEAVY", 4, 0x63AB0442));
        Weapons.Add(new GTAWeapon("weapon_compactlauncher", 10, "HEAVY", 4, 125959754));
        Weapons.Add(new GTAWeapon("weapon_rayminigun", 50, "HEAVY", 4, 0xB62D1F67));
    }
    public static GTAWeapon GetRandomWeapon(int RequestedLevel)
    {
        return Weapons.OrderBy(s => rnd.Next()).Where(s => s.WeaponLevel == RequestedLevel).First();
    }
    public static void ResetPoliceStats()
    {
        PoliceScanningSystem.CopsKilledByPlayer = 0;
        PoliceScanningSystem.PlayerHurtPolice = false;
        PoliceScanningSystem.PlayerKilledPolice = false;
        CaughtWithWeapon = false;
        aimedAtPolice = false;
        firedWeapon = false;
        DispatchAudioSystem.ResetReportedItems();
    }
    private static void CreatePassengerCop(GTACop Cop)
    {
        if (Cop.CopPed.CurrentVehicle.IsSeatFree(0))
        {
            Ped CreatedCop = new Ped("s_m_y_cop_01", Cop.CopPed.GetOffsetPosition(new Vector3(0f, -4f, 0f)), Cop.CopPed.Heading);
            CreatedCop.WarpIntoVehicle(Cop.CopPed.CurrentVehicle, 0);
        }
    }

    private static string GetCurrentStreet()
    {
        //string[] streets = new string[1];

        Vector3 PlayerPos = Game.LocalPlayer.Character.Position;

        int StreetHash = 0;
        int CrossingHash = 0;
        unsafe
        {
            NativeFunction.CallByName<uint>("GET_STREET_NAME_AT_COORD", PlayerPos.X, PlayerPos.Y, PlayerPos.Z, &StreetHash, &CrossingHash);
        }

        string StreetName = string.Empty;
        string CrossStreetName = string.Empty;
        Vector3 Position = Game.LocalPlayer.Character.Position;
        if (StreetHash != 0)
        {
            unsafe
            {
                IntPtr ptr = Rage.Native.NativeFunction.CallByName<IntPtr>("GET_STREET_NAME_FROM_HASH_KEY", StreetHash);

                StreetName = Marshal.PtrToStringAnsi(ptr);
            }
        }

        //if (CrossingHash != 0)
        //{
        //    unsafe
        //    {
        //        IntPtr ptr = Rage.Native.NativeFunction.CallByName<IntPtr>("GET_STREET_NAME_FROM_HASH_KEY", CrossingHash);

        //        CrossStreetName = Marshal.PtrToStringAnsi(ptr);
        //    }
        //}
        //WriteToLog("GetSpeedLimit", string.Format("Street Name: {0}, Cross Street Name: {`}", StreetName, CrossStreetName));
        //streets[0] = StreetName;
        //streets[1] = CrossStreetName;
        //return streets;
        return StreetName;
    }
    private static string GetPedStreetName(Ped MyPed)
    {
        Vector3 MyPedPos = MyPed.Position;
        int StreetHash = 0;
        int CrossingHash = 0;
        unsafe
        {
            NativeFunction.CallByName<uint>("GET_STREET_NAME_AT_COORD", MyPedPos.X, MyPedPos.Y, MyPedPos.Z, &StreetHash, &CrossingHash);
        }
        string StreetName = string.Empty;
        string CrossStreetName = string.Empty;
        Vector3 Position = MyPed.Position;
        if (StreetHash != 0)
        {
            unsafe
            {
                IntPtr ptr = Rage.Native.NativeFunction.CallByName<IntPtr>("GET_STREET_NAME_FROM_HASH_KEY", StreetHash);

                StreetName = Marshal.PtrToStringAnsi(ptr);
            }
        }
        return StreetName;
    }
    private static float GetSpeedLimit()
    {
        string StreetName = GetCurrentStreet();

        int SpeedLimit;

        if (StreetName == "Joshua Rd")
            SpeedLimit = 50;
        else if (StreetName == "East Joshua Road")
            SpeedLimit = 50;
        else if (StreetName == "Marina Dr")
            SpeedLimit = 35;
        else if (StreetName == "Alhambra Dr")
            SpeedLimit = 35;
        else if (StreetName == "Niland Ave")
            SpeedLimit = 35;
        else if (StreetName == "Zancudo Ave")
            SpeedLimit = 35;
        else if (StreetName == "Armadillo Ave")
            SpeedLimit = 35;
        else if (StreetName == "Algonquin Blvd")
            SpeedLimit = 35;
        else if (StreetName == "Mountain View Dr")
            SpeedLimit = 35;
        else if (StreetName == "Cholla Springs Ave")
            SpeedLimit = 35;
        else if (StreetName == "Panorama Dr")
            SpeedLimit = 40;
        else if (StreetName == "Lesbos Ln")
            SpeedLimit = 35;
        else if (StreetName == "Calafia Rd")
            SpeedLimit = 30;
        else if (StreetName == "North Calafia Way")
            SpeedLimit = 30;
        else if (StreetName == "Cassidy Trail")
            SpeedLimit = 25;
        else if (StreetName == "Seaview Rd")
            SpeedLimit = 35;
        else if (StreetName == "Grapeseed Main St")
            SpeedLimit = 35;
        else if (StreetName == "Grapeseed Ave")
            SpeedLimit = 35;
        else if (StreetName == "Joad Ln")
            SpeedLimit = 35;
        else if (StreetName == "Union Rd")
            SpeedLimit = 40;
        else if (StreetName == "O'Neil Way")
            SpeedLimit = 25;
        else if (StreetName == "Senora Fwy")
            SpeedLimit = 65;
        else if (StreetName == "Catfish View")
            SpeedLimit = 35;
        else if (StreetName == "Great Ocean Hwy")
            SpeedLimit = 60;
        else if (StreetName == "Paleto Blvd")
            SpeedLimit = 35;
        else if (StreetName == "Duluoz Ave")
            SpeedLimit = 35;
        else if (StreetName == "Procopio Dr")
            SpeedLimit = 35;
        else if (StreetName == "Cascabel Ave")
            SpeedLimit = 30;
        else if (StreetName == "Procopio Promenade")
            SpeedLimit = 25;
        else if (StreetName == "Pyrite Ave")
            SpeedLimit = 30;
        else if (StreetName == "Fort Zancudo Approach Rd")
            SpeedLimit = 25;
        else if (StreetName == "Barbareno Rd")
            SpeedLimit = 30;
        else if (StreetName == "Ineseno Road")
            SpeedLimit = 30;
        else if (StreetName == "West Eclipse Blvd")
            SpeedLimit = 35;
        else if (StreetName == "Playa Vista")
            SpeedLimit = 30;
        else if (StreetName == "Bay City Ave")
            SpeedLimit = 30;
        else if (StreetName == "Del Perro Fwy")
            SpeedLimit = 65;
        else if (StreetName == "Equality Way")
            SpeedLimit = 30;
        else if (StreetName == "Red Desert Ave")
            SpeedLimit = 30;
        else if (StreetName == "Magellan Ave")
            SpeedLimit = 25;
        else if (StreetName == "Sandcastle Way")
            SpeedLimit = 30;
        else if (StreetName == "Vespucci Blvd")
            SpeedLimit = 40;
        else if (StreetName == "Prosperity St")
            SpeedLimit = 30;
        else if (StreetName == "San Andreas Ave")
            SpeedLimit = 40;
        else if (StreetName == "North Rockford Dr")
            SpeedLimit = 35;
        else if (StreetName == "South Rockford Dr")
            SpeedLimit = 35;
        else if (StreetName == "Marathon Ave")
            SpeedLimit = 30;
        else if (StreetName == "Boulevard Del Perro")
            SpeedLimit = 35;
        else if (StreetName == "Cougar Ave")
            SpeedLimit = 30;
        else if (StreetName == "Liberty St")
            SpeedLimit = 30;
        else if (StreetName == "Bay City Incline")
            SpeedLimit = 40;
        else if (StreetName == "Conquistador St")
            SpeedLimit = 25;
        else if (StreetName == "Cortes St")
            SpeedLimit = 25;
        else if (StreetName == "Vitus St")
            SpeedLimit = 25;
        else if (StreetName == "Aguja St")
            SpeedLimit = 25;
        else if (StreetName == "Goma St")
            SpeedLimit = 25;
        else if (StreetName == "Melanoma St")
            SpeedLimit = 25;
        else if (StreetName == "Palomino Ave")
            SpeedLimit = 35;
        else if (StreetName == "Invention Ct")
            SpeedLimit = 25;
        else if (StreetName == "Imagination Ct")
            SpeedLimit = 25;
        else if (StreetName == "Rub St")
            SpeedLimit = 25;
        else if (StreetName == "Tug St")
            SpeedLimit = 25;
        else if (StreetName == "Ginger St")
            SpeedLimit = 30;
        else if (StreetName == "Lindsay Circus")
            SpeedLimit = 30;
        else if (StreetName == "Calais Ave")
            SpeedLimit = 35;
        else if (StreetName == "Adam's Apple Blvd")
            SpeedLimit = 40;
        else if (StreetName == "Alta St")
            SpeedLimit = 40;
        else if (StreetName == "Integrity Way")
            SpeedLimit = 30;
        else if (StreetName == "Swiss St")
            SpeedLimit = 30;
        else if (StreetName == "Strawberry Ave")
            SpeedLimit = 40;
        else if (StreetName == "Capital Blvd")
            SpeedLimit = 30;
        else if (StreetName == "Crusade Rd")
            SpeedLimit = 30;
        else if (StreetName == "Innocence Blvd")
            SpeedLimit = 40;
        else if (StreetName == "Davis Ave")
            SpeedLimit = 40;
        else if (StreetName == "Little Bighorn Ave")
            SpeedLimit = 35;
        else if (StreetName == "Roy Lowenstein Blvd")
            SpeedLimit = 35;
        else if (StreetName == "Jamestown St")
            SpeedLimit = 30;
        else if (StreetName == "Carson Ave")
            SpeedLimit = 35;
        else if (StreetName == "Grove St")
            SpeedLimit = 30;
        else if (StreetName == "Brouge Ave")
            SpeedLimit = 30;
        else if (StreetName == "Covenant Ave")
            SpeedLimit = 30;
        else if (StreetName == "Dutch London St")
            SpeedLimit = 40;
        else if (StreetName == "Signal St")
            SpeedLimit = 30;
        else if (StreetName == "Elysian Fields Fwy")
            SpeedLimit = 50;
        else if (StreetName == "Plaice Pl")
            SpeedLimit = 30;
        else if (StreetName == "Chum St")
            SpeedLimit = 40;
        else if (StreetName == "Chupacabra St")
            SpeedLimit = 30;
        else if (StreetName == "Miriam Turner Overpass")
            SpeedLimit = 30;
        else if (StreetName == "Autopia Pkwy")
            SpeedLimit = 35;
        else if (StreetName == "Exceptionalists Way")
            SpeedLimit = 35;
        else if (StreetName == "La Puerta Fwy")
            SpeedLimit = 60;
        else if (StreetName == "New Empire Way")
            SpeedLimit = 30;
        else if (StreetName == "Runway1")
            SpeedLimit = 90;
        else if (StreetName == "Greenwich Pkwy")
            SpeedLimit = 35;
        else if (StreetName == "Kortz Dr")
            SpeedLimit = 30;
        else if (StreetName == "Banham Canyon Dr")
            SpeedLimit = 40;
        else if (StreetName == "Buen Vino Rd")
            SpeedLimit = 40;
        else if (StreetName == "Route 68")
            SpeedLimit = 55;
        else if (StreetName == "Zancudo Grande Valley")
            SpeedLimit = 40;
        else if (StreetName == "Zancudo Barranca")
            SpeedLimit = 40;
        else if (StreetName == "Galileo Rd")
            SpeedLimit = 40;
        else if (StreetName == "Mt Vinewood Dr")
            SpeedLimit = 40;
        else if (StreetName == "Marlowe Dr")
            SpeedLimit = 40;
        else if (StreetName == "Milton Rd")
            SpeedLimit = 35;
        else if (StreetName == "Kimble Hill Dr")
            SpeedLimit = 35;
        else if (StreetName == "Normandy Dr")
            SpeedLimit = 35;
        else if (StreetName == "Hillcrest Ave")
            SpeedLimit = 35;
        else if (StreetName == "Hillcrest Ridge Access Rd")
            SpeedLimit = 35;
        else if (StreetName == "North Sheldon Ave")
            SpeedLimit = 35;
        else if (StreetName == "Lake Vinewood Dr")
            SpeedLimit = 35;
        else if (StreetName == "Lake Vinewood Est")
            SpeedLimit = 35;
        else if (StreetName == "Baytree Canyon Rd")
            SpeedLimit = 40;
        else if (StreetName == "North Conker Ave")
            SpeedLimit = 35;
        else if (StreetName == "Wild Oats Dr")
            SpeedLimit = 35;
        else if (StreetName == "Whispymound Dr")
            SpeedLimit = 35;
        else if (StreetName == "Didion Dr")
            SpeedLimit = 35;
        else if (StreetName == "Cox Way")
            SpeedLimit = 35;
        else if (StreetName == "Picture Perfect Drive")
            SpeedLimit = 35;
        else if (StreetName == "South Mo Milton Dr")
            SpeedLimit = 35;
        else if (StreetName == "Cockingend Dr")
            SpeedLimit = 35;
        else if (StreetName == "Mad Wayne Thunder Dr")
            SpeedLimit = 35;
        else if (StreetName == "Hangman Ave")
            SpeedLimit = 35;
        else if (StreetName == "Dunstable Ln")
            SpeedLimit = 35;
        else if (StreetName == "Dunstable Dr")
            SpeedLimit = 35;
        else if (StreetName == "Greenwich Way")
            SpeedLimit = 35;
        else if (StreetName == "Greenwich Pl")
            SpeedLimit = 35;
        else if (StreetName == "Hardy Way")
            SpeedLimit = 35;
        else if (StreetName == "Richman St")
            SpeedLimit = 35;
        else if (StreetName == "Ace Jones Dr")
            SpeedLimit = 35;
        else if (StreetName == "Los Santos Freeway")
            SpeedLimit = 65;
        else if (StreetName == "Senora Rd")
            SpeedLimit = 40;
        else if (StreetName == "Nowhere Rd")
            SpeedLimit = 25;
        else if (StreetName == "Smoke Tree Rd")
            SpeedLimit = 35;
        else if (StreetName == "Cholla Rd")
            SpeedLimit = 35;
        else if (StreetName == "Cat-Claw Ave")
            SpeedLimit = 35;
        else if (StreetName == "Senora Way")
            SpeedLimit = 40;
        else if (StreetName == "Palomino Fwy")
            SpeedLimit = 60;
        else if (StreetName == "Shank St")
            SpeedLimit = 25;
        else if (StreetName == "Macdonald St")
            SpeedLimit = 35;
        else if (StreetName == "Route 68 Approach")
            SpeedLimit = 55;
        else if (StreetName == "Vinewood Park Dr")
            SpeedLimit = 35;
        else if (StreetName == "Vinewood Blvd")
            SpeedLimit = 40;
        else if (StreetName == "Mirror Park Blvd")
            SpeedLimit = 35;
        else if (StreetName == "Glory Way")
            SpeedLimit = 35;
        else if (StreetName == "Bridge St")
            SpeedLimit = 35;
        else if (StreetName == "West Mirror Drive")
            SpeedLimit = 35;
        else if (StreetName == "Nikola Ave")
            SpeedLimit = 35;
        else if (StreetName == "East Mirror Dr")
            SpeedLimit = 35;
        else if (StreetName == "Nikola Pl")
            SpeedLimit = 25;
        else if (StreetName == "Mirror Pl")
            SpeedLimit = 35;
        else if (StreetName == "El Rancho Blvd")
            SpeedLimit = 40;
        else if (StreetName == "Olympic Fwy")
            SpeedLimit = 60;
        else if (StreetName == "Fudge Ln")
            SpeedLimit = 25;
        else if (StreetName == "Amarillo Vista")
            SpeedLimit = 25;
        else if (StreetName == "Labor Pl")
            SpeedLimit = 35;
        else if (StreetName == "El Burro Blvd")
            SpeedLimit = 35;
        else if (StreetName == "Sustancia Rd")
            SpeedLimit = 45;
        else if (StreetName == "South Shambles St")
            SpeedLimit = 30;
        else if (StreetName == "Hanger Way")
            SpeedLimit = 30;
        else if (StreetName == "Orchardville Ave")
            SpeedLimit = 30;
        else if (StreetName == "Popular St")
            SpeedLimit = 40;
        else if (StreetName == "Buccaneer Way")
            SpeedLimit = 45;
        else if (StreetName == "Abattoir Ave")
            SpeedLimit = 35;
        else if (StreetName == "Voodoo Place")
            SpeedLimit = 30;
        else if (StreetName == "Mutiny Rd")
            SpeedLimit = 35;
        else if (StreetName == "South Arsenal St")
            SpeedLimit = 35;
        else if (StreetName == "Forum Dr")
            SpeedLimit = 35;
        else if (StreetName == "Morningwood Blvd")
            SpeedLimit = 35;
        else if (StreetName == "Dorset Dr")
            SpeedLimit = 40;
        else if (StreetName == "Caesars Place")
            SpeedLimit = 25;
        else if (StreetName == "Spanish Ave")
            SpeedLimit = 30;
        else if (StreetName == "Portola Dr")
            SpeedLimit = 30;
        else if (StreetName == "Edwood Way")
            SpeedLimit = 25;
        else if (StreetName == "San Vitus Blvd")
            SpeedLimit = 40;
        else if (StreetName == "Eclipse Blvd")
            SpeedLimit = 35;
        else if (StreetName == "Gentry Lane")
            SpeedLimit = 30;
        else if (StreetName == "Las Lagunas Blvd")
            SpeedLimit = 40;
        else if (StreetName == "Power St")
            SpeedLimit = 40;
        else if (StreetName == "Mt Haan Rd")
            SpeedLimit = 40;
        else if (StreetName == "Elgin Ave")
            SpeedLimit = 40;
        else if (StreetName == "Hawick Ave")
            SpeedLimit = 35;
        else if (StreetName == "Meteor St")
            SpeedLimit = 30;
        else if (StreetName == "Alta Pl")
            SpeedLimit = 30;
        else if (StreetName == "Occupation Ave")
            SpeedLimit = 35;
        else if (StreetName == "Carcer Way")
            SpeedLimit = 40;
        else if (StreetName == "Eastbourne Way")
            SpeedLimit = 30;
        else if (StreetName == "Rockford Dr")
            SpeedLimit = 35;
        else if (StreetName == "Abe Milton Pkwy")
            SpeedLimit = 35;
        else if (StreetName == "Laguna Pl")
            SpeedLimit = 30;
        else if (StreetName == "Sinners Passage")
            SpeedLimit = 30;
        else if (StreetName == "Atlee St")
            SpeedLimit = 30;
        else if (StreetName == "Sinner St")
            SpeedLimit = 30;
        else if (StreetName == "Supply St")
            SpeedLimit = 30;
        else if (StreetName == "Amarillo Way")
            SpeedLimit = 35;
        else if (StreetName == "Tower Way")
            SpeedLimit = 35;
        else if (StreetName == "Decker St")
            SpeedLimit = 35;
        else if (StreetName == "Tackle St")
            SpeedLimit = 25;
        else if (StreetName == "Low Power St")
            SpeedLimit = 35;
        else if (StreetName == "Clinton Ave")
            SpeedLimit = 35;
        else if (StreetName == "Fenwell Pl")
            SpeedLimit = 35;
        else if (StreetName == "Utopia Gardens")
            SpeedLimit = 25;
        else if (StreetName == "Cavalry Blvd")
            SpeedLimit = 35;
        else if (StreetName == "South Boulevard Del Perro")
            SpeedLimit = 35;
        else if (StreetName == "Americano Way")
            SpeedLimit = 25;
        else if (StreetName == "Sam Austin Dr")
            SpeedLimit = 25;
        else if (StreetName == "East Galileo Ave")
            SpeedLimit = 35;
        else if (StreetName == "Galileo Park")
            SpeedLimit = 35;
        else if (StreetName == "West Galileo Ave")
            SpeedLimit = 35;
        else if (StreetName == "Tongva Dr")
            SpeedLimit = 40;
        else if (StreetName == "Zancudo Rd")
            SpeedLimit = 35;
        else if (StreetName == "Movie Star Way")
            SpeedLimit = 35;
        else if (StreetName == "Heritage Way")
            SpeedLimit = 35;
        else if (StreetName == "Perth St")
            SpeedLimit = 25;
        else if (StreetName == "Chianski Passage")
            SpeedLimit = 30;
        else if (StreetName == "Lolita Ave")
            SpeedLimit = 35;
        else if (StreetName == "Meringue Ln")
            SpeedLimit = 35;
        else if (StreetName == "Strangeways Dr")
            SpeedLimit = 30;
        else
            SpeedLimit = 50;


        return SpeedLimit;

    }
}