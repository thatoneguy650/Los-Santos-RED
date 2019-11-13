using ExtensionsMethods;
using Instant_Action_RAGE.Systems;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DispatchAudioSystem;
using static VehicleLookup;

public static class InstantAction
{
    //Player State
    public static bool isDead = false;
    public static bool isBusted = false;
    private static bool BeingArrested = false;
    private static bool DiedInVehicle = false;
    private static int MaxWantedLastLife;
    public static int TimesDied;
    private static int PreviousWantedLevel;
    private static Random rnd;
    private static string LastModelHash;
    public static PedVariation myPedVariation;
    private static Vector3 PositionOfDeath;
    private static Vector3 LastWantedCenterPosition = Vector3.Zero;
    public static bool PedOriginallyHadHelmet = false;
    private static PoliceState HandsUpPreviousPoliceState;
    public static List<GTAWeapon> Weapons = new List<GTAWeapon>();
    public static List<WeaponVariation.WeaponComponent> WeaponComponentsLookup = new List<WeaponVariation.WeaponComponent>();
    public static bool PlayerIsJacking = false;
    public static List<TakenOverPed> TakenOverPeds = new List<TakenOverPed>();
    public static Model OriginalModel;
    public static List<DroppedWeapon> DroppedWeapons = new List<DroppedWeapon>();
    public static List<GTALicensePlate> SpareLicensePlates = new List<GTALicensePlate>();
    public static List<Rage.Object> CreatedObjects = new List<Rage.Object>();

    //Police Items
    private static int TimeAimedAtPolice = 0;
    public static bool areHandsUp = false;
    private static bool firedWeapon = false;
    private static bool aimedAtPolice = false;
    public static bool SurrenderBust = false;
    private static uint LastBust;
    private static int ForceSurrenderTime;
    private static Model CopModel = new Model("s_m_y_cop_01");
    private static List<GTALocation> Locations = new List<GTALocation>();
    private static List<GTAStreet> Streets = new List<GTAStreet>();
    public static Ped GhostCop;
    private static uint WantedLevelStartTime;
    private static bool CanReportLastSeen;
    private static uint GameTimeLastGreyedOut;
    private static bool GhostCopFollow;
    public static List<Blip> CreatedBlips = new List<Blip>();
    public static uint GameTimeWantedStarted;
    public static bool PlayerBreakingIntoCar = false;
    public static bool PlayerChangingPlate = false;
    public static uint GameTimeLastWantedEnded;
    public static bool PlayerArtificiallyShooting = false;
    public static Vector3 PlaceWantedStarted;

    //traffic
    public static List<GTAVehicle> TrackedVehicles = new List<GTAVehicle>();
    public static Vehicle OwnedCar = null;
    private static bool ViolationDrivingAgainstTraffic = false;
    private static bool ViolationDrivingOnPavement = false;
    public static bool ReportedStolenVehicle;
    private static bool ViolationHitPed = false;
    private static bool ViolationHitVehicle = false;
    private static uint GameTimeInterval;
    private static bool ViolationsSpeeding = false;
    private static bool IsViolationSpeedLimit;
    private static uint GameTimeLastTakenOver;
    private static bool PlayerIsRunningRedLight = false;

    //Event Checkers
    private static bool PrevIsViolationSpeedLimit;
    private static bool PrevPlayerKilledPolice = false;
    private static int PrevCopsKilledByPlayer = 0;
    private static bool PrevPlayerStarsGreyedOut;
    public static bool PrevPlayerIsJacking = false;
    private static uint GameTimePoliceStateStart;
    public static PoliceState PrevPoliceState = PoliceState.Normal;
    public static PoliceState LastPoliceState = PoliceState.Normal;
    private static bool PrevfiredWeapon = false;
    private static bool PrevPlayerHurtPolice = false;
    private static bool PrevPlayerInVehicle = false;
    private static int PrevWantedLevel = 0;
    private static bool PrevaimedAtPolice;
    private static bool DroppingWeapon = false;
    private static int PrevCountWeapons = 1;
    private static bool PrevAnyCanRecognizePlayer;
    private static List<long> FrameTimes = new List<long>();
    private static WeaponHash LastWeapon = 0;
    private static bool PrevGettingIntoVehicle = false;

    public static bool AnyPoliceCanSeePlayer { get; set; } = false;
    public static bool AnyPoliceCanRecognizePlayer { get; set; } = false;
    public static bool AnyPoliceCanRecognizePlayerAfterWanted { get; set; } = false;
    public static bool AnyPoliceRecentlySeenPlayer { get; set; } = false;
    private static bool IsRunning { get; set; } = true;
    public static PoliceState CurrentPoliceState { get; set; }
    public static bool PlayerInVehicle { get; set; } = false;

    private static bool PlayerIsGettingIntoVehicle;
    private static bool PlayerIsInAnyPoliceVehicle;
    private static bool PrevPlayerIsGettingIntoVehicle;
    private static uint GameTimeLastShowHelp;
    private static bool ViolationNonRoadworthy;
    private static bool Logging = true;
    private static bool PlayerIsSpeeding = false;
    private static float CurrentSpeedLimit;
    private static int PrevCiviliansKilledByPlayer;
    private static bool PrevIsRunningRedLight;

    private static Blip LastWantedCenterBlip;
    private static Blip CurrentWantedCenterBlip;
    public static bool PlayerStarsGreyedOut { get; set; } = false;
    public static bool AnyPoliceSeenPlayerThisWanted { get; set; } = false;
    public static bool PlayerWasJustJacking
    {
        get
        {
            if (GameTimeLastStartedJacking == 0)
                return false;
            else
                return Game.GameTime - GameTimeLastStartedJacking >= 5000;
        }
    } 
    public static uint GameTimeLastStartedJacking = 0;
    private static uint GameTimeAbandonVehicleCheck;
    private static bool PrevAnyPoliceRecentlySeenPlayer;
    private static uint GameTimeLastTriedCarJacking;
    private static bool PlayerIsPersonOfInterest = false;

    public static bool IsHardToSeeInWeather
    {
        get
        {
            WeatherType TheWeather = World.Weather;
            if (TheWeather == WeatherType.Blizzard || TheWeather == WeatherType.Foggy || TheWeather == WeatherType.Rain || TheWeather == WeatherType.Snow || TheWeather == WeatherType.Snowlight || TheWeather == WeatherType.Thunder || TheWeather == WeatherType.Xmas)
                return true;
            else
                return false;
        }
    }

    public static bool IsNightTime { get; private set; }
    public static uint PlayerHasBeenWantedFor//seconds
    {
        get
        {
            if (Game.LocalPlayer.WantedLevel == 0)
                return 0;
            else
                return (Game.GameTime - WantedLevelStartTime);
        }
    }
    public static uint PlayerHasBeenNotWantedFor//seconds
    {
        get
        {
            if (Game.LocalPlayer.WantedLevel != 0)
                return 0;
            if (GameTimeLastWantedEnded == 0)
                return 0;
            else
                return (Game.GameTime - GameTimeLastWantedEnded);
        }
    }

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

    //Header Items
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
        setupStreets();
        setupLicensePlates();
        MainLoop();
    }
    public static void MainLoop()
    {
        var stopwatch = new Stopwatch();
        GameFiber.StartNew(delegate
        {
            Settings.Initialize();
            Menus.Intitialize();
            RespawnSystem.Initialize();
            PoliceScanningSystem.Initialize();
            DispatchAudioSystem.Initialize();
            CustomOptions.Initialize();
            PoliceSpeechSystem.Initialize();
            VehicleLookup.Initialize();
            VehicleEngineSystem.Initialize();
            while (IsRunning)
            {
                stopwatch.Start();
                UpdatePlayer();
                StateTick();
                ControlTick();
                UITick();
                PoliceTick();
                if (Game.GameTime > GameTimeInterval + 500)
                {
                    TrafficViolationsTick();
                    GameTimeInterval = Game.GameTime;
                }
                stopwatch.Stop();
                if (stopwatch.ElapsedMilliseconds >= 20)
                {
                    WriteToLog("InstantActionTick", string.Format("Tick took {0} ms", stopwatch.ElapsedMilliseconds));
                }
                stopwatch.Reset();
                GameFiber.Yield();
            }

        });

        GameFiber.StartNew(delegate
        {
            while (IsRunning)
            {
                DebugLoop();
                GameFiber.Yield();
            }
        });
    }
    public static void Dispose()
    {
        IsRunning = false;
        VehicleEngineSystem.IsRunning = false;
        foreach (Blip myBlip in CreatedBlips)
        {
            if (myBlip.Exists())
                myBlip.Delete();
        }
    }

    //Player Items
    private static void UpdatePlayer()
    {
        PlayerInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
        PlayerIsGettingIntoVehicle = Game.LocalPlayer.Character.IsGettingIntoVehicle;

        if(PrevPlayerIsGettingIntoVehicle != PlayerIsGettingIntoVehicle)
        {
            PlayerIsGettingIntoVehicleChanged();
        }

        if (PlayerInVehicle)
        {
            Vehicle CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
            if (!TrackedVehicles.Any(x => x.VehicleEnt.Handle == CurrVehicle.Handle)) //Not Tracking Current Vehicle
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
                GTALicensePlate MyPlate = new GTALicensePlate(CurrVehicle.LicensePlate, (uint)CurrVehicle.Handle, NativeFunction.CallByName<int>("GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", CurrVehicle), false);
                TrackedVehicles.Add(new GTAVehicle(CurrVehicle, Game.GameTime, AmStealingCarFromPrerson, CurrVehicle.IsAlarmSounding, PreviousOwner, !stolen, stolen, MyPlate));
            }
        }

        int RecentlySeenTime = 10000;
        int WeaponCount = Game.LocalPlayer.Character.Inventory.Weapons.Count;
        if (PrevCountWeapons != WeaponCount)
        {
            WeaponInventoryChanged(WeaponCount);
            
        }

        AnyPoliceCanSeePlayer = PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer);
        AnyPoliceRecentlySeenPlayer = PoliceScanningSystem.CopPeds.Any(x => x.SeenPlayerSince(RecentlySeenTime));

        uint TimeToRecongize = 2000;

        IsNightTime = false;
        int HourOfDay = NativeFunction.CallByName<int>("GET_CLOCK_HOURS");

        if (HourOfDay >= 20 || HourOfDay <= 5)
            IsNightTime = true;
        if (IsNightTime)
            TimeToRecongize = 3500;
        else if (PlayerInVehicle)
            TimeToRecongize = 750;
        else
            TimeToRecongize = 2000;
        AnyPoliceCanRecognizePlayer = PoliceScanningSystem.CopPeds.Any(x => x.HasSeenPlayerFor >= TimeToRecongize || (x.canSeePlayer && x.DistanceToPlayer <= 20f) || (x.DistanceToPlayer <= 7f && x.DistanceToPlayer > 0f));

        
        if (PlayerInVehicle)
            TimeToRecongize = 6000;
        else
            TimeToRecongize = 4000;

        if (IsNightTime)
            TimeToRecongize = TimeToRecongize + 6000;

        AnyPoliceCanRecognizePlayerAfterWanted = PoliceScanningSystem.CopPeds.Any(x => x.HasSeenPlayerFor >= TimeToRecongize || (x.canSeePlayer && x.DistanceToPlayer <= 12f) || (x.DistanceToPlayer <= 7f && x.DistanceToPlayer > 0f));


        if (PrevAnyCanRecognizePlayer != AnyPoliceCanRecognizePlayer)
        {
            PrevAnyCanRecognizePlayer = AnyPoliceCanRecognizePlayer;
        }

        PlayerStarsGreyedOut = NativeFunction.CallByName<bool>("ARE_PLAYER_STARS_GREYED_OUT", Game.LocalPlayer);
        if (!AnyPoliceSeenPlayerThisWanted && AnyPoliceRecentlySeenPlayer)
            AnyPoliceSeenPlayerThisWanted = true;

        if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash != LastWeapon)
        {
            LastWeapon = Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash;
        }



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
                AddDispatchToQueue(new DispatchQueueItem(ReportDispatch.ReportSuspectWasted, 5, false));
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
            Game.TimeScale = .4f;
            areHandsUp = false;
            Menus.bustedMenu.Visible = true;
            SetArrestedAnimation(Game.LocalPlayer.Character, false);
            AddDispatchToQueue(new DispatchQueueItem(ReportDispatch.ReportSuspectArrested, 5, false));
        }

        if (Game.LocalPlayer.WantedLevel > PreviousWantedLevel)
            PreviousWantedLevel = Game.LocalPlayer.WantedLevel;

        if (Game.LocalPlayer.WantedLevel > MaxWantedLastLife) // The max wanted level i saw in the last life, not just right before being busted
            MaxWantedLastLife = Game.LocalPlayer.WantedLevel;
        //else if (Game.LocalPlayer.WantedLevel == 0 && MaxWantedLastLife > 0 && !isBusted && !isDead)
        //    MaxWantedLastLife = 0;

        NativeFunction.Natives.xB9EFD5C25018725A("PoliceScannerDisabled", true);
        NativeFunction.Natives.xB9EFD5C25018725A("WantedMusicDisabled", true);
        NativeFunction.Natives.xB9EFD5C25018725A("DISPLAY_HUD", true);


        if (Game.GameTime - GameTimeLastTakenOver <= 1000 && Game.LocalPlayer.WantedLevel > 0)
        {
            WriteToLog("ValueChecker", String.Format("Reset Wanted Level After Ped Takeover: {0}", GameTimeLastTakenOver));
            Game.LocalPlayer.WantedLevel = 0;
        }

    }
    private static void ControlTick()
    {
        if (Game.IsKeyDownRightNow(Settings.SurrenderKey) && !Game.LocalPlayer.IsFreeAiming && (!Game.LocalPlayer.Character.IsInAnyVehicle(false) || Game.LocalPlayer.Character.CurrentVehicle.Speed < 2.5f))
        {
            if (!areHandsUp && !isBusted)
            {
                SetPedUnarmed(Game.LocalPlayer.Character, false);
                HandsUpPreviousPoliceState = CurrentPoliceState;
                areHandsUp = true;
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

        if (Game.IsKeyDownRightNow(Settings.DropWeaponKey) && !DroppingWeapon && !PlayerInVehicle && Game.LocalPlayer.Character.isConsideredArmed())
        {
            DropWeapon();
        }
    }
    private static void UITick()
    {
        if (Settings.TrafficViolationsUI && Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.WantedLevel == 0 && CurrentSpeedLimit != 0)
        {
            float VehicleSpeedMPH = Game.LocalPlayer.Character.CurrentVehicle.Speed * 2.23694f;
            if (PlayerIsSpeeding)
            {
                Text(string.Format("{0} MPH Over Limit", Math.Round(VehicleSpeedMPH - CurrentSpeedLimit, MidpointRounding.AwayFromZero)), Settings.TrafficViolationsUIPositionX, Settings.TrafficViolationsUIPositionY, Settings.TrafficViolationsUIScale, true, Color.DarkRed);
            }
            else if(!Settings.TrafficViolationsUIOnlyWhenSpeeding)
            {
                Text(string.Format("Speed Limit {0} MPH", CurrentSpeedLimit), Settings.TrafficViolationsUIPositionX, Settings.TrafficViolationsUIPositionY, Settings.TrafficViolationsUIScale, true, Color.White);
            }
        }
    }

    //Police
    private static void PoliceTick()
    {
        PoliceScanningSystem.UpdatePolice();
        GetPoliceState();
        PoliceVehicleTick();
        CheckPoliceEvents();
        TrackedVehiclesTick();


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
    private static void GetPoliceState()
    {
        if (CurrentPoliceState == PoliceState.ArrestedWait || CurrentPoliceState == PoliceState.DeadlyChase)
            return;

  //      bool AnyCanSeePlayer = PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer);


        if (Game.LocalPlayer.WantedLevel == 0)
        {
            CurrentPoliceState = PoliceState.Normal;
        }
        else if (Game.LocalPlayer.WantedLevel >= 1 && Game.LocalPlayer.WantedLevel <= 3 && AnyPoliceCanSeePlayer)//AnyCanSeePlayer)
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
        if (AnyPoliceCanRecognizePlayer/*AnyCanSeePlayer*/ && Game.LocalPlayer.Character.isConsideredArmed() && PlayerWantedLevel < 3 && !Game.LocalPlayer.Character.IsInAnyVehicle(false))
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
                DispatchQueueItem CarryingWeapon = new DispatchQueueItem(ReportDispatch.ReportCarryingWeapon, 3, false);
                CarryingWeapon.WeaponToReport = MatchedWeapon;
                AddDispatchToQueue(CarryingWeapon);
            }
        }

        if (!aimedAtPolice && Game.LocalPlayer.Character.isConsideredArmed() && Game.LocalPlayer.IsFreeAiming && /*AnyCanSeePlayer*/AnyPoliceCanSeePlayer && PoliceScanningSystem.CopPeds.Any(x => Game.LocalPlayer.IsFreeAimingAtEntity(x.CopPed)))
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

        if (!firedWeapon && (Game.LocalPlayer.Character.IsShooting || PlayerArtificiallyShooting) && (PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer || (x.DistanceToPlayer <= 100f && !Game.LocalPlayer.Character.IsCurrentWeaponSilenced)))) //if (!firedWeapon && Game.LocalPlayer.Character.IsShooting && (PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer || x.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f))))
        {
            Game.LocalPlayer.WantedLevel = 2;
            firedWeapon = true;
            WriteToLog("Fired weapon", "");
        }

        if(Game.LocalPlayer.WantedLevel < 2 && PlayerChangingPlate && AnyPoliceCanSeePlayer)
        {
            Game.LocalPlayer.WantedLevel = 2;
            AddDispatchToQueue(new DispatchQueueItem(ReportDispatch.ReportSuspiciousActivity, 3, false));
            WriteToLog("Cops caught you swapping plates", "");
        }

        if (Game.LocalPlayer.WantedLevel < 2 && PlayerBreakingIntoCar && AnyPoliceCanSeePlayer)
        {
            Game.LocalPlayer.WantedLevel = 2;
            AddDispatchToQueue(new DispatchQueueItem(ReportDispatch.ReportGrandTheftAuto, 3, false));
            WriteToLog("Cops caught you breaking into a car", "");
        }



        //if (PlayerIsPersonOfInterest && PlayerWantedLevel == 0 && AnyPoliceCanRecognizePlayerAfterWanted && PlayerHasBeenNotWantedFor >= 5000 && PlayerHasBeenNotWantedFor <= 120000)
        //{
        //    Game.LocalPlayer.WantedLevel = 3;
        //    AddDispatchToQueue(new DispatchQueueItem(ReportDispatch.ReportSuspectSpotted, 3, false));
        //    WriteToLog("Cops Reacquired after losing them", "");
        //}
        //if (PlayerIsPersonOfInterest && PlayerWantedLevel == 0 && AnyPoliceCanSeePlayer && PlayerHasBeenNotWantedFor >= 5000 && PlayerHasBeenNotWantedFor <= 120000 && LastWantedCenterPosition != Vector3.Zero && Game.LocalPlayer.Character.DistanceTo2D(LastWantedCenterPosition) <= 250f)
        //{
        //    Game.LocalPlayer.WantedLevel = 3;
        //    AddDispatchToQueue(new DispatchQueueItem(ReportDispatch.ReportSuspectSpotted, 3, false));
        //    WriteToLog("Cops Reacquired after losing them in the same area", "");
        //}


        if (PlayerHasBeenNotWantedFor >= 120000)
        {
            PlayerIsPersonOfInterest = false;
            if (LastWantedCenterBlip.Exists())
                LastWantedCenterBlip.Delete();
        }
    }
    private static void PoliceTickNormal()
    {
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.isTasked && !x.TaskIsQueued))
        {
            Cop.TaskIsQueued = true;
            PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
        }
        //foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.SetDeadly || x.SetTazer || x.SetUnarmed))
        //{
        //    ResetCopWeapons(Cop);
        //}
        if(Game.GameTime - GameTimePoliceStateStart >= 8000)
        {
            foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.SetDeadly || x.SetTazer || x.SetUnarmed))
            {
                ResetCopWeapons(Cop);
            }
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

            int TotalFootChaseTasked = PoliceScanningSystem.CopPeds.Where(x => (x.isTasked || x.TaskIsQueued) && x.TaskType == PoliceTask.Task.Chase).Count();
            int TotalVehicleChaseTasked = PoliceScanningSystem.CopPeds.Where(x => (x.isTasked || x.TaskIsQueued) && x.TaskType == PoliceTask.Task.VehicleChase).Count();

            if (!isBusted && Cop.RecentlySeenPlayer() && !Cop.TaskIsQueued && TotalFootChaseTasked <= 4 && !Cop.CopPed.IsInAnyVehicle(false) && Cop.DistanceToPlayer <= 55f && (!Game.LocalPlayer.Character.IsInAnyVehicle(false) || Game.LocalPlayer.Character.CurrentVehicle.Speed <= 5f))
            {
                Cop.TaskIsQueued = true;
                PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Chase));
            }
            else if (!isBusted && Cop.RecentlySeenPlayer() && !Cop.TaskIsQueued && TotalFootChaseTasked > 0 && TotalVehicleChaseTasked <= 5 && Cop.isInVehicle && !Cop.isInHelicopter && Cop.DistanceToPlayer <= 55f && !Game.LocalPlayer.Character.IsInAnyVehicle(false))
            {
                Cop.TaskIsQueued = true;
                PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.VehicleChase));
            }
        }
        //if (PoliceScanningSystem.CopPeds.Any(x => x.DistanceToPlayer <= 4f) && (Game.LocalPlayer.Character.IsRagdoll || Game.LocalPlayer.Character.Speed <= 4.0f) && !Game.LocalPlayer.Character.IsInAnyVehicle(false) && !isBusted)
        //    SurrenderBust = true;

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
                else if (!Cop.TaskIsQueued && (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask || Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.Preparing || Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.Interrupted) && (Cop.RecentlySeenPlayer() || Cop.DistanceToPlayer <= 65f))
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

        if (PoliceScanningSystem.CopPeds.Any(x => x.DistanceToPlayer <= 4f) && (Game.LocalPlayer.Character.IsRagdoll || Game.LocalPlayer.Character.Speed <= 4.0f) && !Game.LocalPlayer.Character.IsInAnyVehicle(false) && !isBusted)
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
            if (!Cop.TaskIsQueued && PoliceScanningSystem.CopPeds.Where(x => x.isTasked || x.TaskIsQueued).Count() <= 4 && Cop.DistanceToPlayer <= 45f)
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
                WriteToLog("PoliceTickCautiousChase", "Queued An Untask");
                PoliceScanningSystem.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
            }

        }
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => !x.isTasked && (x.isInHelicopter || x.isOnBike)))
        {
            SetUnarmed(Cop);
        }

        if (PoliceScanningSystem.CopPeds.Any(x => x.DistanceToPlayer <= 8f) && Game.LocalPlayer.Character.Speed <= 4.0f && !Game.LocalPlayer.Character.IsInAnyVehicle(false) && !isBusted && !PlayerWasJustJacking)
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
        if (Settings.IssuePoliceHeavyWeapons)
        {
            foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.isInVehicle && x.IssuedHeavyWeapon == null))
            {
                PoliceScanningSystem.IssueCopHeavyWeapon(Cop);
                break;
            }
        }

        if (PoliceScanningSystem.CopsKilledByPlayer >= Settings.PoliceKilledSurrenderLimit && Game.LocalPlayer.WantedLevel < 4)
        {
            Game.LocalPlayer.WantedLevel = 4;
            AddDispatchToQueue(new DispatchQueueItem(ReportDispatch.ReportWeaponsFree, 2, false));
        }

        if (SurrenderBust && !isBustTimeOut())
            SurrenderBustEvent();
    }
    private static void PoliceTickSearchMode()
    {
        if(AnyPoliceSeenPlayerThisWanted)
            NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, PoliceScanningSystem.PlacePlayerLastSeen.X, PoliceScanningSystem.PlacePlayerLastSeen.Y, PoliceScanningSystem.PlacePlayerLastSeen.Z);
        else
            NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, PlaceWantedStarted.X, PlaceWantedStarted.Y, PlaceWantedStarted.Z);

        if (CanReportLastSeen && Game.GameTime - GameTimeLastGreyedOut > 10000 && AnyPoliceSeenPlayerThisWanted && PlayerHasBeenWantedFor > 45000)
        {
            WriteToLog("ReportSuspectLastSeen", "ReportSuspectLastSeen");
            AddDispatchToQueue(new DispatchQueueItem(ReportDispatch.ReportSuspectLastSeen, 10, false));
            CanReportLastSeen = false;
        }
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

        if (PrevCiviliansKilledByPlayer != PoliceScanningSystem.CiviliansKilledByPlayer)
            CiviliansKilledChanged();

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
            PlayerStarsGreyedOutChanged();

        if (PrevPoliceState != CurrentPoliceState)
            PoliceStateChanged();
    }
    private static void TrackedVehiclesTick()
    {
        TrackedVehicles.RemoveAll(x => !x.VehicleEnt.Exists());
        if (Game.LocalPlayer.WantedLevel == 0)
        {
            foreach (GTAVehicle StolenCar in TrackedVehicles.Where(x => x.ShouldReportStolen))
            {
                StolenCar.QuedeReportedStolen = true;
                AddDispatchToQueue(new DispatchQueueItem(ReportDispatch.ReportStolenVehicle, 10, false, true, StolenCar));
            }
        }
        int PlayerWantedLevel = Game.LocalPlayer.WantedLevel;
        if (PlayerInVehicle)
        {
            Vehicle CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
            GTAVehicle CurrTrackedVehicle = TrackedVehicles.Where(x => x.VehicleEnt.Handle == CurrVehicle.Handle).FirstOrDefault();
            

            if (AnyPoliceCanRecognizePlayer)
            {
                if (PlayerWantedLevel > 0 && !PlayerStarsGreyedOut)
                {
                    UpdateVehicleDescription(CurrTrackedVehicle);
                }

                if (CurrTrackedVehicle.WasReportedStolen && CurrTrackedVehicle.IsStolen && CurrTrackedVehicle.MatchesOriginalDescription && PlayerWantedLevel < 2)
                {
                    Game.LocalPlayer.WantedLevel = 2;
                    AddDispatchToQueue(new DispatchQueueItem(ReportDispatch.ReportSpottedStolenCar, 10, false, true, CurrTrackedVehicle));
                    WriteToLog("TrackedVehiclesTick", "First");
                }
                else if (CurrTrackedVehicle.CarPlate.IsWanted && !CurrTrackedVehicle.IsStolen && CurrTrackedVehicle.ColorMatchesDescription && PlayerWantedLevel < 2)
                {
                    Game.LocalPlayer.WantedLevel = 2;
                    AddDispatchToQueue(new DispatchQueueItem(ReportDispatch.ReportSuspiciousVehicle, 10, false, true, CurrTrackedVehicle));
                    WriteToLog("TrackedVehiclesTick", "Third");
                }
            }

            if (CurrTrackedVehicle.CarPlate.IsWanted && PlayerWantedLevel == 0 && PlayerHasBeenNotWantedFor >= 60000 && !CurrTrackedVehicle.IsStolen)
            {
                CurrTrackedVehicle.CarPlate.IsWanted = false;
                WriteToLog("TrackedVehiclesTick", "No Longer looking for your WantedVehicle"); //Cops gave up 
                AddDispatchToQueue(new DispatchQueueItem(ReportDispatch.ReportSuspectLost, 10, false, true));
                Menus.UpdateLists();
            }
        }
    }
    public static void UpdateVehicleDescription(GTAVehicle MyVehicle)
    {
        MyVehicle.DescriptionColor = MyVehicle.VehicleEnt.PrimaryColor;
        MyVehicle.CarPlate.IsWanted = true;
        if (MyVehicle.IsStolen && !MyVehicle.WasReportedStolen)
            MyVehicle.WasReportedStolen = true;
    }
    private static void WantedLevelTick()
    {       
        if (Game.LocalPlayer.WantedLevel > 0)
        {
            Vector3 CurrentWantedCenter = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
            if (CurrentWantedCenter != Vector3.Zero)
            {
                LastWantedCenterPosition = CurrentWantedCenter;
                AddUpdateCurrentWantedBlip(CurrentWantedCenter);
            }

            if (Settings.WantedLevelIncreasesOverTime && Game.GameTime - WantedLevelStartTime > 180000 && WantedLevelStartTime > 0 && AnyPoliceRecentlySeenPlayer && Game.LocalPlayer.WantedLevel > 0 && Game.LocalPlayer.WantedLevel <= 4)
            {
                Game.LocalPlayer.WantedLevel++;
                WriteToLog("WantedLevelStartTime", "Wanted Level Increased Over Time");
            }

            if (Settings.SpawnNewsChopper && Game.GameTime - WantedLevelStartTime > 180000 && WantedLevelStartTime > 0 && AnyPoliceRecentlySeenPlayer && Game.LocalPlayer.WantedLevel > 4 && PoliceScanningSystem.NewsTeam.Count() == 0)
            {
                PoliceScanningSystem.SpawnNewsChopper();
                WriteToLog("WantedLevelTick", "Been at this wanted for a while, wanted news chopper spawned (if they dont already exist)");
            }

            if (PlayerStarsGreyedOut && PoliceScanningSystem.CopPeds.All(x => !x.RecentlySeenPlayer()))
            {
                PoliceTickSearchMode();
            }

            if (AnyPoliceRecentlySeenPlayer && !PlayerStarsGreyedOut)
            {
                PoliceScanningSystem.PlacePlayerLastSeen = Game.LocalPlayer.Character.Position;
                NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z);
            }

        }
    }
    private static bool isBustTimeOut()
    {
        if (Game.GameTime - LastBust >= 10000)
            return false;
        else
            return true;
    }
    public static void SetUnarmed(GTACop Cop)
    {
        if (!Cop.CopPed.Exists() || (Cop.SetUnarmed && !Cop.NeedsWeaponCheck))
            return;
        if (Settings.OverridePoliceAccuracy)
            Cop.CopPed.Accuracy = Settings.PoliceGeneralAccuracy;
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
        if (Settings.OverridePoliceAccuracy)
            Cop.CopPed.Accuracy = Settings.PoliceGeneralAccuracy;
        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.CopPed, 30);
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
        if (Settings.OverridePoliceAccuracy)
            Cop.CopPed.Accuracy = Settings.PoliceGeneralAccuracy;
        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.CopPed, 30);
        if (!Cop.CopPed.Inventory.Weapons.Contains(Cop.IssuedPistol.Name))
            Cop.CopPed.Inventory.GiveNewWeapon(Cop.IssuedPistol.Name, -1, true);

        if ((Cop.CopPed.Inventory.EquippedWeapon == null || Cop.CopPed.Inventory.EquippedWeapon.Hash == WeaponHash.StunGun) && Game.LocalPlayer.WantedLevel >= 0)
            Cop.CopPed.Inventory.GiveNewWeapon(Cop.IssuedPistol.Name, -1, true);

        ApplyWeaponVariation(Cop.CopPed,(uint) Cop.IssuedPistol.Hash, Cop.PistolVariation);
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

        if (Settings.OverridePoliceAccuracy)
            Cop.CopPed.Accuracy = Settings.PoliceTazerAccuracy;
        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.CopPed, 100);
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
            Vector3 DesiredPosition = Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0f, 4f, 1f));
            Vector3 PlacedPosition = Vector3.Zero;
            bool FoundPlace = false;
            unsafe
            {
                FoundPlace = NativeFunction.CallByName<bool>("GET_SAFE_COORD_FOR_PED", DesiredPosition.X, DesiredPosition.Y, DesiredPosition.Z, false, &PlacedPosition, 16);
            }

            if(FoundPlace)
                GhostCop.Position = PlacedPosition;
            else
                GhostCop.Position = DesiredPosition;

            Vector3 Resultant = Vector3.Subtract(Game.LocalPlayer.Character.Position, GhostCop.Position);
            GhostCop.Heading = NativeFunction.CallByName<float>("GET_HEADING_FROM_VECTOR_2D", Resultant.X, Resultant.Y);
        }
        if (PoliceScanningSystem.CopPeds.Any(x => x.RecentlySeenPlayer()))// Needed for the AI to keep the player in the wanted position
        {
            GhostCopFollow = true;
        }
        else
        {
            if (GhostCop != null)
                GhostCop.Position = new Vector3(0f, 0f, 0f);
            GhostCopFollow = false;
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
        NativeFunction.CallByName<bool>("STOP_PED_SPEAKING", GhostCop, true);
        NativeFunction.CallByName<uint>("SET_PED_CONFIG_FLAG", GhostCop, 69, true);
        NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", GhostCop, (uint)2725352035, true); //Unequip weapon so you don't get shot
        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", GhostCop, false);
        NativeFunction.CallByName<uint>("SET_PED_MOVE_RATE_OVERRIDE", GhostCop, 0f);
        GhostCopFollow = true;
    }
    public static void ResetPoliceStats()
    {
        PoliceScanningSystem.CopsKilledByPlayer = 0;
        PoliceScanningSystem.CiviliansKilledByPlayer = 0;
        PoliceScanningSystem.PlayerHurtPolice = false;
        PoliceScanningSystem.PlayerKilledPolice = false;
        PoliceScanningSystem.PlayerKilledCivilians = false;
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds)
        {
            Cop.HurtByPlayer = false;
        }
        aimedAtPolice = false;
        firedWeapon = false;
        DispatchAudioSystem.ResetReportedItems();
    }

    //Traffic Violations
    private static void TrafficViolationsTick()
    {
        if (CurrentPoliceState != PoliceState.Normal)
            return;

        if (!Settings.TrafficViolations)
            return;

        PlayerInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false); 
        bool JustTakenOver = Game.GameTime - GameTimeLastTakenOver <= 5000;

        if(!PlayerInVehicle)
        {
            IsViolationSpeedLimit = false;
        }

        if (PlayerInVehicle && !JustTakenOver)
        {
            float VehicleSpeedMPH = Game.LocalPlayer.Character.CurrentVehicle.Speed * 2.23694f;
            Vehicle CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
            GTAVehicle MyCar = GetPlayersCurrentTrackedVehicle();
            bool TreatAsCop = false;

            if (Settings.TrafficViolationsExemptCode3 && CurrVehicle != null && CurrVehicle.IsPoliceVehicle && MyCar != null && !MyCar.WasReportedStolen)
            {
                if(CurrVehicle.IsSirenOn)
                {
                    TreatAsCop = true;//Cops dont have to do traffic laws stuff if ur running code3?
                }
            }

            if (Settings.TrafficViolationsDrivingAgainstTraffic && AnyPoliceCanSeePlayer && !ViolationDrivingAgainstTraffic && !TreatAsCop && Game.LocalPlayer.IsDrivingAgainstTraffic)
            {
                ViolationDrivingAgainstTraffic = true;
                Game.LocalPlayer.WantedLevel = 1;
                DispatchQueueItem RecklessDriver = new DispatchQueueItem(ReportDispatch.ReportRecklessDriver, 10, false,true, MyCar);
                RecklessDriver.IsTrafficViolation = true;
                AddDispatchToQueue(RecklessDriver);
            }
            if (Settings.TrafficViolationsDrivingOnPavement && AnyPoliceCanSeePlayer && !ViolationDrivingOnPavement && !TreatAsCop && Game.LocalPlayer.IsDrivingOnPavement)
            {
                ViolationDrivingOnPavement = true;
                Game.LocalPlayer.WantedLevel = 1;
                DispatchQueueItem RecklessDriver = new DispatchQueueItem(ReportDispatch.ReportRecklessDriver, 10, false,true, MyCar);
                RecklessDriver.IsTrafficViolation = true;
                AddDispatchToQueue(RecklessDriver);
            }
            int TimeSincePlayerLastHitAnyPed = Game.LocalPlayer.TimeSincePlayerLastHitAnyPed;
            if (Settings.TrafficViolationsHitPed && AnyPoliceCanSeePlayer && !ViolationHitPed && TimeSincePlayerLastHitAnyPed > -1 && TimeSincePlayerLastHitAnyPed <= 1000)
            {
                ViolationHitPed = true;
                Game.LocalPlayer.WantedLevel = 2;
                DispatchQueueItem PedHitAndRun = new DispatchQueueItem(ReportDispatch.ReportPedHitAndRun, 8, false, true, MyCar);
                PedHitAndRun.IsTrafficViolation = true;
                AddDispatchToQueue(PedHitAndRun);
            }
            int TimeSincePlayerLastHitAnyVehicle = Game.LocalPlayer.TimeSincePlayerLastHitAnyVehicle;
            if (Settings.TrafficViolationsHitVehicle && AnyPoliceCanSeePlayer && !ViolationHitVehicle && TimeSincePlayerLastHitAnyVehicle > -1 && TimeSincePlayerLastHitAnyVehicle <= 1000)
            {
                ViolationHitVehicle = true;
                Game.LocalPlayer.WantedLevel = 1;
                DispatchQueueItem VehicleHitAndRun = new DispatchQueueItem(ReportDispatch.ReportVehicleHitAndRun, 9, false, true, MyCar);
                VehicleHitAndRun.IsTrafficViolation = true;
                AddDispatchToQueue(VehicleHitAndRun);
            }
            if (Settings.TrafficViolationsNotRoadworthy && AnyPoliceCanSeePlayer && !ViolationNonRoadworthy && !TreatAsCop && (!CurrVehicle.IsRoadWorthy() || CurrVehicle.IsDamaged()))
            {
                ViolationNonRoadworthy = true;
                Game.LocalPlayer.WantedLevel = 1;
                DispatchQueueItem NonRoadWorthy = new DispatchQueueItem(ReportDispatch.ReportSuspiciousVehicle, 10, false, true, MyCar);
                NonRoadWorthy.IsTrafficViolation = true;
                AddDispatchToQueue(NonRoadWorthy);
            }

            float SpeedLimit  = GetSpeedLimit();
            bool ViolationSpeedLimit = VehicleSpeedMPH > SpeedLimit + Settings.TrafficViolationsSpeedingOverLimitThreshold;
            PlayerIsSpeeding = ViolationSpeedLimit;
            CurrentSpeedLimit = SpeedLimit;

            if (ViolationSpeedLimit)
                IsViolationSpeedLimit = true;
            else
                IsViolationSpeedLimit = false;

            if (Settings.TrafficViolationsSpeeding && AnyPoliceCanSeePlayer && !ViolationsSpeeding && !TreatAsCop && ViolationSpeedLimit)
            {
                ViolationsSpeeding = true;
                Game.LocalPlayer.WantedLevel = 1;
                DispatchQueueItem FelonySpeeding = new DispatchQueueItem(ReportDispatch.ReportFelonySpeeding, 10, false,true, MyCar);
                FelonySpeeding.IsTrafficViolation = true;
                AddDispatchToQueue(FelonySpeeding);
            }      
        }
    }
    private static void ResetTrafficViolations()
    {
        ViolationDrivingOnPavement = false;
        ViolationDrivingAgainstTraffic = false;
        ViolationHitPed = false;
        ViolationsSpeeding = false;
        ViolationHitVehicle = false;
        ViolationNonRoadworthy = false;
    }
    private static string GetCurrentStreet()
    {
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
        return StreetName;
    }
    private static float GetSpeedLimit()
    {
        string StreetName = GetCurrentStreet();
        GTAStreet MyStreet = Streets.Where(x => x.Name == StreetName).FirstOrDefault();
        if (MyStreet == null)
            return 50f;
        else
            return MyStreet.SpeedLimit;
    }

    //Police Events
    private static void WantedLevelChanged()
    {
        if (Game.LocalPlayer.WantedLevel == 0)//Just Removed
        {
            if (AnyPoliceSeenPlayerThisWanted && PrevWantedLevel != 0)//maxwantedlastlife
            {
                AddDispatchToQueue(new DispatchQueueItem(ReportDispatch.ReportSuspectLost, 5, false));
                AddUpdateLastWantedBlip(LastWantedCenterPosition);
            }
            CurrentPoliceState = PoliceState.Normal;
            AnyPoliceSeenPlayerThisWanted = false;
            ResetTrafficViolations();
            WantedLevelStartTime = 0;
            GameTimeWantedStarted = 0;
            DispatchAudioSystem.ResetReportedItems();
            GameTimeLastWantedEnded = Game.GameTime;

            if (CurrentWantedCenterBlip.Exists())
                CurrentWantedCenterBlip.Delete();
           
            PoliceScanningSystem.UntaskAll(false);
        }
        else
        {
            GameTimeLastWantedEnded = 0;
        }
        if(PrevWantedLevel == 0 && Game.LocalPlayer.WantedLevel > 0)
        {
            GameTimeWantedStarted = Game.GameTime;
            PlaceWantedStarted = Game.LocalPlayer.Character.Position;

            if (LastWantedCenterBlip.Exists())
                LastWantedCenterBlip.Delete();

            PoliceScanningSystem.UntaskAllRandomSpawns(false);
            PlayerIsPersonOfInterest = true;
        }
        WantedLevelStartTime = Game.GameTime;
        WriteToLog("ValueChecker", String.Format("WantedLevel Changed to: {0}", Game.LocalPlayer.WantedLevel));
        PrevWantedLevel = Game.LocalPlayer.WantedLevel;
    }
    private static void CopsKilledChanged()
    {
        WriteToLog("ValueChecker", String.Format("CopsKilledByPlayer Changed to: {0}", PoliceScanningSystem.CopsKilledByPlayer));
        PrevCopsKilledByPlayer = PoliceScanningSystem.CopsKilledByPlayer;
    }
    private static void CiviliansKilledChanged()
    {
        WriteToLog("ValueChecker", String.Format("CiviliansKilledChanged Changed to: {0}", PoliceScanningSystem.CiviliansKilledByPlayer));
        PrevCiviliansKilledByPlayer = PoliceScanningSystem.CiviliansKilledByPlayer;
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
    private static void aimedAtPoliceChanged()
    {
        if (aimedAtPolice)
        {
            AddDispatchToQueue(new DispatchQueueItem(ReportDispatch.ReportThreateningWithFirearm, 2, true));
        }
        PrevaimedAtPolice = aimedAtPolice;
    }
    private static void PlayerStarsGreyedOutChanged()
    {
        WriteToLog("ValueChecker", String.Format("PlayerStarsGreyedOut Changed to: {0}", PlayerStarsGreyedOut));
        if (PlayerStarsGreyedOut)
        {
            CanReportLastSeen = true;
            GameTimeLastGreyedOut = Game.GameTime;
        }
        else
        {
            CanReportLastSeen = false;
        }
        PrevPlayerStarsGreyedOut = PlayerStarsGreyedOut;
    }
    private static void PoliceStateChanged()
    {
        WriteToLog("ValueChecker", String.Format("PoliceState Changed to: {0}", CurrentPoliceState));
        WriteToLog("ValueChecker", String.Format("PreviousPoliceState Changed to: {0}", PrevPoliceState));
        LastPoliceState = PrevPoliceState;
        if (CurrentPoliceState == PoliceState.Normal && !isDead)
        {
            ResetPoliceStats();
            PoliceScanningSystem.DeleteNewsTeam();
        }

        if (CurrentPoliceState == PoliceState.DeadlyChase)
        {
            if (PrevPoliceState != PoliceState.ArrestedWait)
            {
                AddDispatchToQueue(new DispatchQueueItem(ReportDispatch.ReportLethalForceAuthorized, 1, true));
            }
        }
        GameTimePoliceStateStart = Game.GameTime;
        PrevPoliceState = CurrentPoliceState;
    }

    //Player Events
    private static void PlayerIsGettingIntoVehicleChanged()
    {
        if (PlayerIsGettingIntoVehicle)
        {
            Vehicle TargetVeh = Game.LocalPlayer.Character.VehicleTryingToEnter;
            int SeatTryingToEnter = Game.LocalPlayer.Character.SeatIndexTryingToEnter;


            LockCarDoor(TargetVeh);

            int LockStatus = (int)TargetVeh.LockStatus;


            if(LockStatus == 7)
            {
                UnlockCarDoor(TargetVeh, SeatTryingToEnter);
            }

            if (TargetVeh != null && SeatTryingToEnter == -1)
            {
                Ped Driver = TargetVeh.Driver;
                if (Driver != null && Driver.IsAlive)
                {
                    //GivePlayerLastWeaponIfUnarmed();
                    CarJackPedWithWeapon(TargetVeh,Driver, SeatTryingToEnter);
                    WriteToLog("EnterVehicle", "CarJacking");
                }
                else
                {
                    WriteToLog("EnterVehicle", "Regular Enter No Driver");
                }
            }
            else
            {
                WriteToLog("EnterVehicle", "Regular Enter");
            }

        }
        PrevPlayerIsGettingIntoVehicle = PlayerIsGettingIntoVehicle;
    }
    private static void PlayerInVehicleChanged(bool playerInVehicle)
    {
        if (playerInVehicle)
        {
            GTAVehicle MyVehicle = GetPlayersCurrentTrackedVehicle();
            if (MyVehicle == null || MyVehicle.IsStolen)
                return;

            if(OwnedCar == null)
                MyVehicle.IsStolen = true;
            else if (MyVehicle.VehicleEnt.Handle != OwnedCar.Handle && !MyVehicle.IsStolen)
                MyVehicle.IsStolen = true;
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
            GameTimeLastStartedJacking = Game.GameTime;
        }
        PrevPlayerIsJacking = PlayerIsJacking;
    }
    private static void WeaponInventoryChanged(int weaponCount)
    {
        if (weaponCount > PrevCountWeapons) //Added Weapon
        {
            WeaponDescriptorCollection PlayerWeapons = Game.LocalPlayer.Character.Inventory.Weapons;
            foreach (DroppedWeapon MyOldGuns in DroppedWeapons)
            {
                if (PlayerWeapons.Contains(MyOldGuns.Weapon.Hash) && Game.LocalPlayer.Character.Position.DistanceTo2D(MyOldGuns.CoordinatedDropped) <= 2f)
                {
                    WriteToLog("WeaponInventoryChanged", string.Format("Just picked up an old weapon {0},OldAmmo: {1}", MyOldGuns.Weapon.Hash, MyOldGuns.Ammo));
                    ApplyWeaponVariation(Game.LocalPlayer.Character, (uint)MyOldGuns.Weapon.Hash, MyOldGuns.Variation);


                    //NativeFunction.CallByName<bool>("SET_PED_AMMO", Game.LocalPlayer.Character, (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash, Game.LocalPlayer.Character.Inventory.EquippedWeapon.Ammo + MyOldGuns.Ammo);
                    NativeFunction.CallByName<bool>("ADD_AMMO_TO_PED", Game.LocalPlayer.Character, (uint)MyOldGuns.Weapon.Hash, MyOldGuns.Ammo+1);
                }
            }
            DroppedWeapons.RemoveAll(x => PlayerWeapons.Contains(x.Weapon.Hash) && Game.LocalPlayer.Character.Position.DistanceTo2D(x.CoordinatedDropped) <= 2f);
        }
        else //Lost Weapon
        {

        }
        WriteToLog("WeaponInventoryChanged", string.Format("Previous Weapon Count {0}, Current {1}, Total Dropped Weapons {2}", PrevCountWeapons, weaponCount, DroppedWeapons.Count()));
        PrevCountWeapons = weaponCount;
    }

    //Surrendering
    private static void RaiseHands()
    {
        if (Game.LocalPlayer.WantedLevel > 0 && PoliceScanningSystem.CopsKilledByPlayer < 5)
            CurrentPoliceState = PoliceState.ArrestedWait;



        bool inVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
        var sDict = (inVehicle) ? "veh@busted_std" : "ped";
        RequestAnimationDictionay(sDict);
        if (inVehicle)
        {
            //NativeFunction.CallByName<bool>("ROLL_DOWN_WINDOW", Game.LocalPlayer.Character.CurrentVehicle, 0);
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, sDict, "stay_in_car_crim", 2.0f, -2.0f, -1, 50, 0, true, false, true);
            //GameFiber.Sleep(250);
            //NativeFunction.CallByName<bool>("SET_ENTITY_ANIM_CURRENT_TIME", Game.LocalPlayer.Character, sDict, "stay_in_car_crim", 0.5f);
        }
        else
        {
            //if (!DroppingWeapon && Game.LocalPlayer.Character.isConsideredArmed())
            //    DropWeapon();

            //while (DroppingWeapon)
            //    GameFiber.Sleep(100);

            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, sDict, "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
        }

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
                //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "veh@busted_std", "get_out_car_crim", 2.0f, -2.0f, 2500, 50, 0, false, false, false);
                //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "veh@busted_std", "get_out_car_crim", 8.0f, -8.0f, -1, 50, 0, false, false, false);

                //GameFiber.Sleep(6000);
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
    private static void UnSetArrestedAnimation(Ped PedToArrest)
    {
        GameFiber.StartNew(delegate
        {
            RequestAnimationDictionay("random@arrests");
            RequestAnimationDictionay("busted");
            RequestAnimationDictionay("ped");

            if (NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_a", 1) || NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_2_hands_up", 1))
            {
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "random@arrests", "kneeling_arrest_escape", 8.0f, -8.0f, -1, 120, 0, 0, 1, 0);//"random@arrests", "kneeling_arrest_escape", 8.0f, -8.0f, -1, 4096, 0, 0, 1, 0);
            }
            else if (NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "ped", "handsup_enter", 1))
            {
                PedToArrest.Tasks.Clear();
            }
        });
    }

    //Drop Weapon
    public static void DropWeapon()
    {
        DroppingWeapon = true;
        GameFiber.StartNew(delegate
        {
            DropWeaponAnimation();
            if (Game.LocalPlayer.Character.IsRunning)
                GameFiber.Sleep(500);
            else
                GameFiber.Sleep(250);

            int CurrentWeaponAmmo = Game.LocalPlayer.Character.Inventory.EquippedWeapon.Ammo;
            int AmmoToDrop = 0;
            if (CurrentWeaponAmmo > 60)
                AmmoToDrop = 60;
            else if (CurrentWeaponAmmo == 0)
                AmmoToDrop = 0;
            else
                AmmoToDrop = CurrentWeaponAmmo;

            NativeFunction.CallByName<bool>("SET_PED_AMMO", Game.LocalPlayer.Character, (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash, CurrentWeaponAmmo - AmmoToDrop);

            WeaponVariation DroppedGunVariation = GetWeaponVariation(Game.LocalPlayer.Character, (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash);
            DroppedWeapons.Add(new DroppedWeapon(Game.LocalPlayer.Character.Inventory.EquippedWeapon, Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0f, 0.5f, 0f)), DroppedGunVariation, AmmoToDrop));

            NativeFunction.CallByName<bool>("SET_PED_DROPS_INVENTORY_WEAPON", Game.LocalPlayer.Character, (int)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash, 0.0f, 0.5f, 0.0f, -1);
            if (!(Game.LocalPlayer.Character.Inventory.EquippedWeapon == null))
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)2725352035, true);
            WriteToLog("DroppingWeapon", string.Format("Dropped your gun, Ammo {0}", AmmoToDrop));

            GameFiber.Sleep(1000);
            DroppingWeapon = false;
        });
    }
    private static void DropWeaponAnimation()
    {
        GameFiber.StartNew(delegate
        {
            RequestAnimationDictionay("pickup_object");
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "pickup_object", "pickup_low", 8.0f, -8.0f, -1, 56, 0, false, false, false);

        });
    }
    public static GTAWeapon GetCurrentWeapon()
    {
        WeaponDescriptor MyWeapon = Game.LocalPlayer.Character.Inventory.EquippedWeapon;
        if (MyWeapon == null)
            return null;

        GTAWeapon CurrentGun = Weapons.Where(x => (WeaponHash)x.Hash == MyWeapon.Hash).FirstOrDefault();
        if (CurrentGun != null)
            return CurrentGun;
        else
            return null;
    }
    public static void SetPlayerToLastWeapon()
    {
        if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && LastWeapon != 0)
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)LastWeapon, true);
        }
    }

    //Suicide
    public static void CommitSuicide(Ped PedToSuicide)
    {
        GameFiber.StartNew(delegate
        {
            if (!PedToSuicide.IsInAnyVehicle(false))
            {
                RequestAnimationDictionay("mp_suicide");

                GTAWeapon CurrentGun = null;
                if (PedToSuicide.Inventory.EquippedWeapon != null)
                    CurrentGun = Weapons.Where(x => (WeaponHash)x.Hash == PedToSuicide.Inventory.EquippedWeapon.Hash && x.CanPistolSuicide).FirstOrDefault();

                if (CurrentGun != null)
                {
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToSuicide, "mp_suicide", "pistol", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                    GameFiber.Wait(750);
                    Vector3 HeadCoordinated = PedToSuicide.GetBonePosition(PedBoneId.Head);
                    NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", PedToSuicide, HeadCoordinated.X, HeadCoordinated.Y, HeadCoordinated.Z, true);
                }
                else
                {
                    NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", PedToSuicide, (uint)2725352035, true);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToSuicide, "mp_suicide", "pill", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                    GameFiber.Wait(6000);
                }
            }
            PedToSuicide.Kill();
        });
    }

    //Car Stealing
    private static void UnlockCarDoor(Vehicle ToEnter, int SeatTryingToEnter)
    {
        if (!Game.IsControlPressed(2, GameControl.Enter))//holding enter go thru normal
            return;

        try
        {
            GameFiber.StartNew(delegate
            {

                SetPedUnarmed(Game.LocalPlayer.Character, false);

                bool Continue = true;
                ToEnter.MustBeHotwired = true;

                Vector3 GameEntryPosition = GetHandlePosition(ToEnter);
                if (GameEntryPosition == Vector3.Zero)
                    return;

                string AnimationName = "std_force_entry_ds";
                int DoorIndex = 0;
                int WaitTime = 1750;


                if (ToEnter.HasBone("door_dside_f") && ToEnter.HasBone("door_pside_f"))
                {
                    if(Game.LocalPlayer.Character.DistanceTo2D(ToEnter.GetBonePosition("door_dside_f")) > Game.LocalPlayer.Character.DistanceTo2D(ToEnter.GetBonePosition("door_pside_f")))
                    {
                        AnimationName = "std_force_entry_ps";
                        DoorIndex = 1;
                        WaitTime = 2200;
                    }
                    else
                    {
                        AnimationName = "std_force_entry_ds";
                        DoorIndex = 0;
                        WaitTime = 1750;
                    }
                }

                WriteToLog("UnlockCarDoor", string.Format("DoorIndex: {0},AnimationName: {1}", DoorIndex, AnimationName));
                Rage.Object Screwdriver = AttachScrewdriverToPed(Game.LocalPlayer.Character);
                RequestAnimationDictionay("veh@break_in@0h@p_m_one@");
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "veh@break_in@0h@p_m_one@", AnimationName, 2.0f, -2.0f, -1, 0, 0, false, false, false);

                PlayerBreakingIntoCar = true;

                uint GameTimeStarted = Game.GameTime;
                while (Game.GameTime - GameTimeStarted <= WaitTime)
                {
                    GameFiber.Yield();
                    if (Extensions.IsMoveControlPressed())
                    {
                        Continue = false;
                        break;
                    }
                }

                if (!Continue)
                {
                    Game.LocalPlayer.Character.Tasks.Clear();
                    Screwdriver.Delete();
                    PlayerBreakingIntoCar = false;
                    return;
                }

                ToEnter.LockStatus = VehicleLockStatus.Unlocked;
                ToEnter.Doors[DoorIndex].Open(true, false);

                //GameFiber.Sleep(500);

                WriteToLog("UnlockCarDoor", string.Format("Open Door: {0}", DoorIndex));
                GameTimeStarted = Game.GameTime;
                Game.LocalPlayer.Character.Tasks.EnterVehicle(ToEnter, SeatTryingToEnter);
                while (!Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.GameTime - GameTimeStarted <= 10000)
                {
                    GameFiber.Yield();
                    if (Extensions.IsMoveControlPressed())
                    {
                        Continue = false;
                        break;
                    }
                }
                if (ToEnter.Doors[DoorIndex].IsValid())
                    NativeFunction.CallByName<bool>("SET_VEHICLE_DOOR_CONTROL", ToEnter, DoorIndex, 4, 0f);
                GameFiber.Sleep(5000);
                Screwdriver.Delete();
                PlayerBreakingIntoCar = false;
                WriteToLog("UnlockCarDoor", string.Format("Made it to the end: {0}", SeatTryingToEnter));
            });
        }
        catch (Exception e)
        {
            foreach (Rage.Object obj in CreatedObjects.Where(x => x.Exists()))
                obj.Delete();
            CreatedObjects.Clear();
            PlayerBreakingIntoCar = false;
            WriteToLog("UnlockCarDoor", e.Message);
        }


    }
    private static void LockCarDoor(Vehicle ToLock)
    {
        WriteToLog("LockCarDoor", string.Format("Go To Start, Lock Status {0}", ToLock.LockStatus));
        if (ToLock.LockStatus != (VehicleLockStatus)1) //unlocked
            return;
        WriteToLog("LockCarDoor", "1");
        if (ToLock.HasDriver)//If they have a driver 
            return;
        WriteToLog("LockCarDoor", "2");
        foreach (VehicleDoor myDoor in ToLock.GetDoors())
        {
            if (!myDoor.IsValid() || myDoor.IsOpen)
                return;//invalid doors make the car not locked
        }
        WriteToLog("LockCarDoor", "3");
        if (!NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", ToLock))
            return;//broken windows == not locked
        WriteToLog("LockCarDoor", "4");
        if (TrackedVehicles.Any(x => x.VehicleEnt.Handle == ToLock.Handle))
            return; //previously entered vehicle arent locked


        WriteToLog("LockCarDoor", "Locked");
        ToLock.LockStatus = (VehicleLockStatus)7;
    }

    //Car Jacking
    public static void CarJackPedWithWeapon(Vehicle TargetVehicle, Ped Driver, int SeatTryingToEnter) 
    {
        if (!Game.IsControlPressed(2, GameControl.Enter))//holding enter go thru normal
            return;
        if (Game.GameTime - GameTimeLastTriedCarJacking <= 5000)
            return;
        try
        {
            if (SeatTryingToEnter != -1)
                return;

            GTAWeapon myGun = GetCurrentWeapon();
            if (myGun == null)
                return;

            GameFiber.StartNew(delegate
            {
                SetPlayerToLastWeapon();
                NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", Driver, TargetVehicle, 27, -1);
                Driver.BlockPermanentEvents = true;

                Vector3 GameEntryPosition = GetHandlePosition(TargetVehicle);
                float DesiredHeading = TargetVehicle.Heading - 90f;


                string dict = "";
                string PerpAnim = "";
                string VictimAnim = "";
                int BoneIndexSpine = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Driver, 57597);//11816
                Vector3 DriverSeatCoordinates = NativeFunction.CallByName<Vector3>("GET_PED_BONE_COORDS", Driver, BoneIndexSpine, 0f, 0f, 0f);

                GameTimeLastTriedCarJacking = Game.GameTime;

                if (!GetCarjackingAnimations(TargetVehicle, DriverSeatCoordinates, myGun, ref dict, ref PerpAnim, ref VictimAnim))//couldnt find animations
                {
                    Game.LocalPlayer.Character.Tasks.ClearImmediately();
                    GameFiber.Sleep(200);
                    Game.LocalPlayer.Character.Tasks.EnterVehicle(TargetVehicle, SeatTryingToEnter);
                    return;
                }

                if (!MovePedToCarPosition(TargetVehicle, Game.LocalPlayer.Character, DesiredHeading, GameEntryPosition, true, false))
                {
                    Game.LocalPlayer.Character.Tasks.Clear();
                    return;
                }


                RequestAnimationDictionay(dict);

                float DriverHeading = Driver.Heading;
                int Scene1 = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", GameEntryPosition.X, GameEntryPosition.Y, Game.LocalPlayer.Character.Position.Z, 0.0f, 0.0f, DesiredHeading, 2);//270f //old
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", Scene1, false);
                NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Game.LocalPlayer.Character, Scene1, dict, PerpAnim, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);//std_perp_ds_a
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", Scene1, 0.0f);

                int Scene2 = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", DriverSeatCoordinates.X, DriverSeatCoordinates.Y, DriverSeatCoordinates.Z, 0.0f, 0.0f, DriverHeading, 2);//270f
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", Scene2, false);
                NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", Driver, Scene2, dict, VictimAnim, 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);
                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", Scene2, 0.0f);

                PlayerBreakingIntoCar = true;
                bool locOpenDoor = false;
                bool Cancel = false;

                while (NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", Scene1) < 0.75f)
                {
                    float ScenePhase = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", Scene1);
                    float Scene2Phase = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", Scene2);
                    GameFiber.Yield();
                    if (ScenePhase <= 0.4f && Game.IsControlJustPressed(2, GameControl.MoveUp) || Game.IsControlJustPressed(2, GameControl.MoveRight) || Game.IsControlJustPressed(2, GameControl.MoveDown) || Game.IsControlJustPressed(2, GameControl.MoveLeft))
                    {
                        Cancel = true;
                        break;
                    }

                    if(!locOpenDoor && ScenePhase > .1f && TargetVehicle.Doors[0].IsValid() && !TargetVehicle.Doors[0].IsFullyOpen)
                    {
                        locOpenDoor = true;
                        TargetVehicle.Doors[0].Open(false, false);
                    }
                    if (Game.LocalPlayer.Character.isConsideredArmed() && Game.IsControlPressed(2, GameControl.Attack))
                    {
                        Vector3 TargetCoordinate = Driver.GetBonePosition(PedBoneId.Head);
                        NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", Game.LocalPlayer.Character, TargetCoordinate.X, TargetCoordinate.Y, TargetCoordinate.Z, true);
                        PlayerArtificiallyShooting = true;

                        if (ScenePhase <= 0.35f)
                        {
                            Driver.WarpIntoVehicle(TargetVehicle, -1);
                            Game.LocalPlayer.Character.Tasks.Clear();
                            NativeFunction.CallByName<bool>("SET_PLAYER_FORCED_AIM", Game.LocalPlayer.Character, true);
                            break;
                        }
                    }
                    if (Game.LocalPlayer.Character.isConsideredArmed() && Game.IsControlJustPressed(2, GameControl.Aim))
                    {
                        if (NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", Scene1) <= 0.4f)
                        {
                            Driver.WarpIntoVehicle(TargetVehicle, -1);
                            Game.LocalPlayer.Character.Tasks.Clear();
                            NativeFunction.CallByName<bool>("SET_PLAYER_FORCED_AIM", Game.LocalPlayer.Character, true);
                            break;
                        }
                    }
                }

                PlayerArtificiallyShooting = false;

                float FinalScenePhase = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", Scene1);

                if(FinalScenePhase <= 0.4f)
                {
                    if (Cancel || Driver.IsDead)
                    {
                        Driver.BlockPermanentEvents = false;
                        Driver.WarpIntoVehicle(TargetVehicle, -1);
                        Game.LocalPlayer.Character.Tasks.Clear();
                    }
                }
                else
                {
                    if (Cancel)
                    {
                        Driver.BlockPermanentEvents = false;
                        Driver.WarpIntoVehicle(TargetVehicle, -1);
                        Game.LocalPlayer.Character.Tasks.Clear();
                    }
                    else
                        Game.LocalPlayer.Character.WarpIntoVehicle(TargetVehicle, -1);
                }

                if (Cancel)
                    return;

                if(TargetVehicle.Doors[0].IsValid())
                    NativeFunction.CallByName<bool>("SET_VEHICLE_DOOR_CONTROL", TargetVehicle, 0, 4, 0f);

                WriteToLog("CarJackPedWithWeapon", string.Format("Scene1 Phase: {0}", FinalScenePhase));

                if (Driver.IsInAnyVehicle(false))
                {
                    WriteToLog("CarjackAnimation", "Driver In Vehicle");
                    //return;
                }
                else
                {
                    WriteToLog("CarjackAnimation", "Driver Out of Vehicle");
                    // GameFiber.Sleep(1000);




                    //float? GroundZ = World.GetGroundZ(Driver.Position, true, false);
                    //if (GroundZ != null && Driver.Position.Z-1.0f <= GroundZ)
                    //    Driver.Position = new Vector3(Driver.Position.X, Driver.Position.Y, (float)GroundZ + 2f);
                    Driver.Tasks.ClearImmediately();

                    Driver.IsRagdoll = false;
                    Driver.BlockPermanentEvents = false;

                    if (rnd.Next(1, 11) >= 11)
                    {
                        GiveGunAndAttackPlayer(Driver);
                    }
                    else
                    {
                        Driver.Tasks.Flee(Game.LocalPlayer.Character, 100f, 30000);
                    }
                    
                }
                GameFiber.Sleep(5000);

                PlayerBreakingIntoCar = false;
            });
        }
        catch (Exception e)
        {
            foreach (Rage.Object obj in CreatedObjects.Where(x => x.Exists()))
                obj.Delete();
            CreatedObjects.Clear();
            PlayerBreakingIntoCar = false;
            WriteToLog("UnlockCarDoor", e.Message);
        }
    }
    public static bool GetCarjackingAnimations(Vehicle TargetVehicle,Vector3 DriverSeatCoordinates, GTAWeapon MyGun, ref string Dictionary,ref string PerpAnimation,ref string VictimAnimation)
    {
        if (MyGun == null || (!MyGun.IsTwoHanded && !MyGun.IsOneHanded))
            return false;

        int intVehicleClass = NativeFunction.CallByName<int>("GET_VEHICLE_CLASS", TargetVehicle);
        VehicleLookup.VehicleClass VehicleClass = (VehicleLookup.VehicleClass)intVehicleClass;


        if (!TargetVehicle.Doors[0].IsValid())
            return false;

        float? GroundZ = World.GetGroundZ(DriverSeatCoordinates, true, false);
        if (GroundZ == null)
            GroundZ = 0f;
        float DriverDistanceToGround = DriverSeatCoordinates.Z - (float)GroundZ;
        WriteToLog("GetCarjackingAnimations", string.Format("VehicleClass {0},DriverSeatCoordinates: {1},GroundZ: {2}, PedHeight: {3}", VehicleClass, DriverSeatCoordinates, GroundZ, DriverDistanceToGround));
        if (VehicleClass == VehicleLookup.VehicleClass.Vans)
        {
            if (MyGun.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PerpAnimation = "van_perp_ds_a";
                VictimAnimation = "van_victim_ds_a";
            }
            else if (MyGun.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PerpAnimation = "van_perp_ds_a";
                VictimAnimation = "van_victim_ds_a";
            }
        }
        else if (VehicleClass == VehicleLookup.VehicleClass.Helicopters)
        {
            if (MyGun.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PerpAnimation = "heli_perp_ds_a";
                VictimAnimation = "heli_victim_ds_a";
            }
            else if (MyGun.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PerpAnimation = "heli_perp_ds_a";
                VictimAnimation = "heli_victim_ds_a";
            }
        }
        else if (VehicleClass == VehicleLookup.VehicleClass.Commercial)
        {
            if (MyGun.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PerpAnimation = "truck_perp_ds_a";
                VictimAnimation = "truck_victim_ds_a";
            }
            else if (MyGun.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PerpAnimation = "truck_perp_ds_a";
                VictimAnimation = "truck_victim_ds_a";
            }
        }
        else if (DriverDistanceToGround > 1.75f)
        {
            if (MyGun.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PerpAnimation = "truck_perp_ds_a";
                VictimAnimation = "truck_victim_ds_a";
            }
            else if (MyGun.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PerpAnimation = "truck_perp_ds_a";
                VictimAnimation = "truck_victim_ds_a";
            }
        }
        else if (DriverDistanceToGround <0.5f)
        {
            if (MyGun.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PerpAnimation = "low_perp_ds_a";
                VictimAnimation = "low_victim_ds_a";
            }
            else if (MyGun.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PerpAnimation = "low_perp_ds_a";
                VictimAnimation = "low_victim_ds_a";
            }
        }
        else if(VehicleClass == VehicleLookup.VehicleClass.Motorcycles)
        {
            return false;
        }
        else
        {
            if (MyGun.IsTwoHanded)
            {
                Dictionary = "veh@jacking@2h";
                PerpAnimation = "std_perp_ds_a";
                VictimAnimation = "std_victim_ds_a";
            }
            else if (MyGun.IsOneHanded)
            {
                Dictionary = "veh@jacking@1h";
                PerpAnimation = "std_perp_ds";
                VictimAnimation = "std_victim_ds";
            }
        }
        return true;
    }

    //License Plate Changing
    public static void RemoveNearestLicensePlate()
    {
        Vehicle[] NearbyVehicles = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 10f, GetEntitiesFlags.ConsiderAllVehicles).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));
        Vehicle ClosestVehicle = NearbyVehicles.Where(x => x.LicensePlate != "        ").OrderBy(x => GetLicensePlateChangePosition(x).DistanceTo2D(Game.LocalPlayer.Character.Position)).FirstOrDefault();

        if (ClosestVehicle != null)
        {
            GTAVehicle VehicleToChange = TrackedVehicles.Where(x => x.VehicleEnt.Handle == ClosestVehicle.Handle).FirstOrDefault();
            if (VehicleToChange == null)
            {
                VehicleToChange = new GTAVehicle(ClosestVehicle, false, false, new GTALicensePlate(ClosestVehicle.LicensePlate, (uint)ClosestVehicle.Handle, NativeFunction.CallByName<int>("GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", ClosestVehicle), false));
                TrackedVehicles.Add(VehicleToChange);
            }

            if (ClosestVehicle.HasDriver)
            {
                PedReactToThreatening(ClosestVehicle.Driver);
            }
            ChangeLicensePlateAnimation(VehicleToChange, false);
        }
    }
    public static void ChangeNearestLicensePlate()
    {
        if (!SpareLicensePlates.Any())
            return;


        Vehicle[] NearbyVehicles = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 10f, GetEntitiesFlags.ConsiderAllVehicles).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));
        Vehicle ClosestVehicle = NearbyVehicles.OrderBy(x => GetLicensePlateChangePosition(x).DistanceTo2D(Game.LocalPlayer.Character.Position)).FirstOrDefault();

        if (ClosestVehicle != null)
        {
            GTAVehicle VehicleToChange = TrackedVehicles.Where(x => x.VehicleEnt.Handle == ClosestVehicle.Handle).FirstOrDefault();
            if (VehicleToChange == null)
            {
                VehicleToChange = new GTAVehicle(ClosestVehicle, false, false, new GTALicensePlate(ClosestVehicle.LicensePlate, (uint)ClosestVehicle.Handle, NativeFunction.CallByName<int>("GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", ClosestVehicle), false));
                TrackedVehicles.Add(VehicleToChange);
            }

            ChangeLicensePlateAnimation(VehicleToChange, true);
        }
    }
    public static void ChangeLicensePlateAnimation(GTAVehicle VehicleToChange, bool ChangePlates)
    {
        if (!ChangePlates && VehicleToChange.VehicleEnt.LicensePlate == "        ")// Plate already removed
            return;

        GameFiber.StartNew(delegate
        {
            try
            {
                Vector3 CarPosition = VehicleToChange.VehicleEnt.Position;
                Vector3 ChangeSpot = GetLicensePlateChangePosition(VehicleToChange.VehicleEnt);
                if (ChangeSpot == Vector3.Zero)
                    return;

                SetPedUnarmed(Game.LocalPlayer.Character, false);

                bool Moved = MovePedToCarPosition(VehicleToChange.VehicleEnt, Game.LocalPlayer.Character, VehicleToChange.VehicleEnt.Heading, ChangeSpot, true,true);

                if (!Moved)
                    return;

                Game.LocalPlayer.Character.Position = ChangeSpot;
                Game.LocalPlayer.Character.Heading = VehicleToChange.VehicleEnt.Heading;

                PlayerChangingPlate = true;

                Rage.Object Screwdriver = AttachScrewdriverToPed(Game.LocalPlayer.Character);

                Rage.Object LicensePlate = null;
                if(ChangePlates)
                    LicensePlate = AttachLicensePlateToPed(Game.LocalPlayer.Character);

                RequestAnimationDictionay("mp_car_bomb");
                uint GameTimeStartedAnimation = Game.GameTime;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "mp_car_bomb", "car_bomb_mechanic", 2.0f, -2.0f, 5000, 0, 0, false, false, false);
                bool Continue = true;
                while (Game.GameTime - GameTimeStartedAnimation < 2000 && CarPosition.DistanceTo2D(VehicleToChange.VehicleEnt.Position) <= 1f && Game.LocalPlayer.Character.DistanceTo2D(ChangeSpot) <= 2f && !Game.LocalPlayer.Character.IsDead)
                {
                    if (Extensions.IsMoveControlPressed())
                    {
                        Continue = false;
                        break;
                    }
                    GameFiber.Yield();
                }

                if (Continue && CarPosition.DistanceTo2D(VehicleToChange.VehicleEnt.Position) <= 1f && Game.LocalPlayer.Character.DistanceTo2D(ChangeSpot) <= 2f && !Game.LocalPlayer.Character.IsDead)
                {
                    if (ChangePlates && VehicleToChange.VehicleEnt.Exists())
                    {
                        GTALicensePlate PlateToAdd = SpareLicensePlates[Menus.ChangePlateIndex];
                        GTALicensePlate PlateToRemove = VehicleToChange.CarPlate;
                        SpareLicensePlates.RemoveAt(Menus.ChangePlateIndex);
                        if (PlateToRemove != null)
                        {
                            SpareLicensePlates.Add(PlateToRemove);
                        }

                        VehicleToChange.CarPlate = PlateToAdd;
                        VehicleToChange.VehicleEnt.LicensePlate = PlateToAdd.PlateNumber;
                        NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", VehicleToChange.VehicleEnt, PlateToAdd.PlateType);
                        Menus.UpdateLists();
                    }
                    else if (!ChangePlates && VehicleToChange.VehicleEnt.Exists())
                    {
                        SpareLicensePlates.Add(VehicleToChange.CarPlate);
                        Menus.UpdateLists();
                        VehicleToChange.CarPlate = null;
                        VehicleToChange.VehicleEnt.LicensePlate = "        ";
                        VehicleToChange.CarPlate = null;
                    }
                }
                else
                {
                    Game.LocalPlayer.Character.Tasks.Clear();
                }

                if(LicensePlate != null)
                    LicensePlate.Delete();
                GameFiber.Sleep(750);
                Screwdriver.Delete();
                CreatedObjects.Clear();
                PlayerChangingPlate = false;
            }
            catch (Exception e)
            {
                foreach (Rage.Object obj in CreatedObjects.Where(x => x.Exists()))
                    obj.Delete();
                CreatedObjects.Clear();
                PlayerChangingPlate = false;
                WriteToLog("ChangeLicensePlate", e.Message);
            }
        });
    }

    //General Car Items
     public static bool MovePedToCarPosition(Vehicle TargetVehicle, Ped PedToMove, float DesiredHeading, Vector3 PositionToMoveTo, bool StopDriver, bool SlowHeading)
    {
        bool Continue = true;
        bool isPlayer = false;
        if (PedToMove == Game.LocalPlayer.Character)
            isPlayer = true;
        WriteToLog("MovePedToCarPosition", "Slide Started");
        Ped Driver = TargetVehicle.Driver;
        Vector3 CarPosition = TargetVehicle.Position;
        NativeFunction.CallByName<uint>("TASK_PED_SLIDE_TO_COORD", PedToMove, PositionToMoveTo.X, PositionToMoveTo.Y, PositionToMoveTo.Z, DesiredHeading, -1);
        uint GameTimeStarted = Game.GameTime;
        while (PedToMove.DistanceTo2D(PositionToMoveTo) >= 0.05f && Game.GameTime - GameTimeStarted <= 10000 && Game.LocalPlayer.Character.Speed > 0.05f)
        {
            GameFiber.Yield();
            if (isPlayer && Extensions.IsMoveControlPressed())
            {
                Continue = false;
                break;
            }
            if (StopDriver && TargetVehicle.Driver != null)
                NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", Driver, TargetVehicle, 27, -1);
        }
        if (!Continue)
        {
            PedToMove.Tasks.Clear();
            return false;
        }

        if (SlowHeading)
        {
            WriteToLog("MovePedToCarPosition", string.Format("Heading Started Desired: {0}", DesiredHeading));
            NativeFunction.CallByName<int>("TASK_ACHIEVE_HEADING", PedToMove, DesiredHeading, -1);
            uint GameTimeStartedHeading = Game.GameTime;
            while (!PedToMove.Heading.IsWithin(DesiredHeading - 5f, DesiredHeading + 5f) && Game.GameTime - GameTimeStartedHeading < 750)// && TargetVehicle.Position.DistanceTo2D(CarPosition) <= .5f)
            {
                //NativeFunction.CallByName<bool>("FORCE_PED_MOTION_STATE", PedToMove, -530524, true, true, false);//(Ped ped, Hash motionStateHash, BOOL p2, BOOL p3, BOOL p4)     
                GameFiber.Yield();
                if (isPlayer && Extensions.IsMoveControlPressed())
                {
                    Continue = false;
                    break;
                }
                if (StopDriver && TargetVehicle.Driver != null)
                    NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", Driver, TargetVehicle, 27, -1);
            }
            WriteToLog("MovePedToCarPosition", string.Format("Heading Done Actual: {0}", PedToMove.Heading));
            if (!Continue)
            {
                PedToMove.Tasks.Clear();
                return false;
            }
            if (TargetVehicle.Position.DistanceTo2D(CarPosition) > .5f)
            {
                PedToMove.Tasks.Clear();
                return false;
            }
        }
        else
        {
            PedToMove.Heading = DesiredHeading;
        }
        return true;
    }

    public static Vector3 GetHandlePosition(Vehicle TargetVehicle)
    {
        Vector3 GameEntryPosition = Vector3.Zero;
        if (TargetVehicle.HasBone("handle_dside_f") && 1 == 0)
        {
            GameEntryPosition = TargetVehicle.GetBonePosition("handle_dside_f");
            WriteToLog("CarJackPedWithWeapon", string.Format("Handle Pos: {0}", GameEntryPosition));
        }
        else
        {
            GameEntryPosition = NativeFunction.CallByHash<Vector3>(0xC0572928C0ABFDA3, TargetVehicle, 0);
            WriteToLog("CarJackPedWithWeapon", string.Format("Game Entry Pos: {0}", GameEntryPosition));
        }
        return GameEntryPosition;
    }
    public static Vector3 GetLicensePlateChangePosition(Vehicle VehicleToChange)
    {
        Vector3 Position;
        Vector3 Right;
        Vector3 Forward;
        Vector3 Up;

        if (VehicleToChange.HasBone("numberplate"))
        {
            Position = VehicleToChange.GetBonePosition("numberplate");
            VehicleToChange.GetBoneAxes("numberplate", out Right, out Forward, out Up);
            return Vector3.Add(Forward * -1.0f, Position);
        }

        else if (VehicleToChange.HasBone("boot"))
        {
            Position = VehicleToChange.GetBonePosition("boot");
            VehicleToChange.GetBoneAxes("boot", out Right, out Forward, out Up);
            return Vector3.Add(Forward * -1.75f, Position);
        }
        else if (VehicleToChange.IsBike)
        {
            return VehicleToChange.GetOffsetPositionFront(-1.5f);
        }
        else if (VehicleToChange.HasBone("bumper_r"))
        {
            Position = VehicleToChange.GetBonePosition("bumper_r");
            VehicleToChange.GetBoneAxes("bumper_r", out Right, out Forward, out Up);
            Position = Vector3.Add(Forward * -1.0f, Position);
            return Vector3.Add(Right * 0.25f, Position);
        }
        else
            return Vector3.Zero;
    }

    //Respawn
    public static void BribePolice(int Amount)
    {
        if (Game.LocalPlayer.Character.GetCash(Settings.MainCharacterToAlias) < Amount)
            return;

        if (Amount < PreviousWantedLevel * Settings.PoliceBribeWantedLevelScale)
        {
            Game.DisplayNotification("Thats it? Thanks for the cash, but you're going downtown.");
            Game.LocalPlayer.Character.GiveCash(-1 * Amount, Settings.MainCharacterToAlias);
            return;
        }
        else
        {
            BeingArrested = false;
            isBusted = false;
            Game.DisplayNotification("Thanks for the cash, now beat it.");
            Game.LocalPlayer.Character.GiveCash(-1 * Amount, Settings.MainCharacterToAlias);
        }
        PlayerIsPersonOfInterest = false;
        CurrentPoliceState = PoliceState.Normal;
        UnSetArrestedAnimation(Game.LocalPlayer.Character);
        NativeFunction.CallByName<bool>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
        //if (Game.LocalPlayer.Character.LastVehicle.Exists())
        //    NativeFunction.CallByName<bool>("SET_VEHICLE_HAS_BEEN_OWNED_BY_PLAYER", Game.LocalPlayer.Character.LastVehicle, true);
        ResetPlayer(true, false);

        PoliceScanningSystem.UntaskAll(true);

    }
    public static void RespawnAtHospital()
    {
        Game.FadeScreenOut(1500);
        GameFiber.Wait(1500);
        isDead = false;
        isBusted = false;
        CurrentPoliceState = PoliceState.Normal;
        PlayerIsPersonOfInterest = false;
        ResetPlayer(true, true);

        Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        RespawnInPlace(false);
        Game.LocalPlayer.WantedLevel = 0;
        GTALocation ClosestPolice = Locations.Where(x => x.Type == GTALocation.LocationType.Hospital).OrderBy(s => Game.LocalPlayer.Character.Position.DistanceTo2D(s.Location)).FirstOrDefault();

        Game.LocalPlayer.Character.Position = ClosestPolice.Location;
        Game.LocalPlayer.Character.Heading = ClosestPolice.Heading;

        GameFiber.Wait(1500);
        Game.FadeScreenIn(1500);
        Game.DisplayNotification(string.Format("You have been charged ~r~${0} ~s~in Hospital fees.", Settings.HospitalFee));
        Game.LocalPlayer.Character.GiveCash(-1 * Settings.HospitalFee, Settings.MainCharacterToAlias);     
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
        PoliceScanningSystem.UntaskAll(true);
    }
    public static void Surrender()
    {
        Game.FadeScreenOut(1500);
        GameFiber.Wait(1500);

        int bailMoney = MaxWantedLastLife * Settings.PoliceBailWantedLevelScale;
        BeingArrested = false;
        isBusted = false;
        Game.LocalPlayer.WantedLevel = 0;
        PlayerIsPersonOfInterest = false;
        RaiseHands();
        ResetPlayer(true, true);
        NativeFunction.CallByName<bool>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
        GTALocation ClosestPolice = Locations.Where(x => x.Type == GTALocation.LocationType.Police).OrderBy(s => Game.LocalPlayer.Character.Position.DistanceTo2D(s.Location)).FirstOrDefault();

        Game.LocalPlayer.Character.Position = ClosestPolice.Location;
        Game.LocalPlayer.Character.Heading = ClosestPolice.Heading;

        Game.LocalPlayer.Character.Tasks.ClearImmediately();
        Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        Game.LocalPlayer.Character.Inventory.GiveNewWeapon((WeaponHash)2725352035, -1, true);
        ResetPlayer(true, true);
        CurrentPoliceState = PoliceState.Normal;
        GameFiber.Wait(1500);
        Game.FadeScreenIn(1500);   
        Game.DisplayNotification(string.Format("You have been charged ~r~${0} ~s~in bail money, try to stay out of trouble.", bailMoney));
        Game.LocalPlayer.Character.GiveCash(-1 * bailMoney, Settings.MainCharacterToAlias);
        PoliceScanningSystem.UntaskAll(true);
    }
    public static void ResetPlayer(bool ClearWanted, bool ResetHealth)
    {
        isDead = false;
        isBusted = false;
        BeingArrested = false;

        NativeFunction.CallByName<bool>("NETWORK_REQUEST_CONTROL_OF_ENTITY", Game.LocalPlayer.Character);
        NativeFunction.Natives.xC0AA53F866B3134D();
        Game.TimeScale = 1f;
        if (ClearWanted)
        {
            Game.LocalPlayer.WantedLevel = 0;
            ResetPoliceStats();
            ResetTrafficViolations();

            LastWantedCenterPosition = Vector3.Zero;
            if (LastWantedCenterBlip.Exists())
                LastWantedCenterBlip.Delete();
        }

        //ResetCamera();
        NativeFunction.Natives.xB4EDDC19532BFB85(); //_STOP_ALL_SCREEN_EFFECTS;
        if (ResetHealth)
            Game.LocalPlayer.Character.Health = 100;
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
                DispatchAudioSystem.AbortAllAudio();
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



                LastWantedCenterPosition = Vector3.Zero;
                if (LastWantedCenterBlip.Exists())
                    LastWantedCenterBlip.Delete();


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

    //Attachment
    public static Rage.Object AttachScrewdriverToPed(Ped Pedestrian)
    {
        Rage.Object Screwdriver = new Rage.Object("prop_tool_screwdvr01", Pedestrian.GetOffsetPositionUp(50f));
        CreatedObjects.Add(Screwdriver);
        int BoneIndexRightHand = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 57005);
        Screwdriver.AttachTo(Pedestrian, BoneIndexRightHand, new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f));
        return Screwdriver;
    }
    public static Rage.Object AttachLicensePlateToPed(Ped Pedestrian)
    {
        Rage.Object LicensePlate = new Rage.Object("p_num_plate_01", Pedestrian.GetOffsetPositionUp(55f));
        CreatedObjects.Add(LicensePlate);
        LicensePlate.IsVisible = true;
        int BoneIndexLeftHand = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 18905);
        LicensePlate.AttachTo(Game.LocalPlayer.Character, BoneIndexLeftHand, new Vector3(0.19f, 0.08f, 0.0f), new Rotator(-57.2f, 90f, -173f));
        
        return LicensePlate;
    }

    //Ped Takeover
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

            bool AlreadyTakenOver = false;

            if (TakenOverPeds.Count > 0)
            {
                uint TargetModelHash = TargetPed.Model.Hash;
                if (TargetModelHash == 225514697 || TargetModelHash == 2602752943 || TargetModelHash == 2608926626)
                {
                    AlreadyTakenOver = true;
                    ChangeModel(OriginalModel.Name);
                    if (!Game.LocalPlayer.Character.isMainCharacter())
                        ReplacePedComponentVariation(Game.LocalPlayer.Character);
                }
            }

            OriginalModel = TargetPed.Model;
            TakenOverPed PedTakenOver = new TakenOverPed(TargetPed, TargetPed.Handle, GetPedVariation(TargetPed), TargetPed.Model, Game.GameTime);
            AddPedToTakenOverPeds(PedTakenOver);


           if(!AlreadyTakenOver)
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

            if (DeleteOld)
                CurrentPed.Delete();
            else if (ArrestOld)
                SetArrestedAnimation(CurrentPed, true);
            else
                AITakeoverPlayer(CurrentPed);

            NativeFunction.Natives.x2206BF9A37B7F724("MinigameTransitionOut", 5000, false);

            if (!AlreadyTakenOver)
            {
                SetPlayerOffset();
                ChangeModel(Settings.MainCharacterToAliasModelName);
                ChangeModel(LastModelHash);
            }


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

            if(Settings.PedTakeoverSetRandomMoney)
                Game.LocalPlayer.Character.SetCash(rnd.Next(Settings.PedTakeoverRandomMoneyMin, Settings.PedTakeoverRandomMoneyMax), Settings.MainCharacterToAlias);

            Game.LocalPlayer.Character.Inventory.Weapons.Clear();
            Game.LocalPlayer.Character.Inventory.GiveNewWeapon(2725352035, 0, true);
            TimesDied = 0;
            MaxWantedLastLife = 0;
            Game.TimeScale = 1f;
            Game.LocalPlayer.WantedLevel = 0;
            NativeFunction.Natives.xB4EDDC19532BFB85();
            Game.HandleRespawn();
            NativeFunction.CallByName<bool>("NETWORK_REQUEST_CONTROL_OF_ENTITY", Game.LocalPlayer.Character);
            NativeFunction.Natives.xC0AA53F866B3134D();
            LastWantedCenterPosition = Vector3.Zero;
            isDead = false;
            isBusted = false;
            CurrentPoliceState = PoliceState.Normal;
            BeingArrested = false;
            ResetTrafficViolations();
            ResetPoliceStats();
            PlayerIsPersonOfInterest = false;
            GameTimeLastTakenOver = Game.GameTime;

            if(Game.LocalPlayer.Character.IsWearingHelmet)
            {
                PedOriginallyHadHelmet = true;
            }
        }
        catch (Exception e3)
        {
            WriteToLog("TakeoverPed", "TakeoverPed Error; " + e3.Message);
        }
    }
    private static void AddPedToTakenOverPeds(TakenOverPed MyPed)
    {
        if (!TakenOverPeds.Any(x => x.OriginalHandle == MyPed.Pedestrian.Handle))
        {
            TakenOverPeds.Add(MyPed);
            WriteToLog("AddPedToTakenOverPeds", string.Format("Added Ped to List {0} ", MyPed.Pedestrian.Handle));
        }
        else
        {
            WriteToLog("AddPedToTakenOverPeds", string.Format("Ped already in list {0} ", MyPed.Pedestrian.Handle));
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
            WriteToLog("CopyPedComponentVariation", "CopyPedComponentVariation Error; " + e.Message);
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
            WriteToLog("CopyPedComponentVariation", "CopyPedComponentVariation Error; " + e.Message);
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
        // Request the character model
        Model characterModel = new Model(ModelRequested);
        characterModel.LoadAndWait();
        characterModel.LoadCollisionAndWait();
        Game.LocalPlayer.Model = characterModel;
        Game.LocalPlayer.Character.IsCollisionEnabled = true;
    }

    //Ped Tasking
    public static void GiveGunAndAttackPlayer(Ped Attacker)
    {
        GTAWeapon GunToGive = Weapons.Where(x => x.Category == GTAWeapon.WeaponCategory.Pistol).PickRandom();
        Attacker.Inventory.GiveNewWeapon(GunToGive.Name, GunToGive.AmmoAmount, true);
        Attacker.Tasks.FightAgainst(Game.LocalPlayer.Character);
        Attacker.BlockPermanentEvents = true;
        Attacker.KeepTasks = true;
    }
    public static void PedReactToThreatening(Ped Attacker)
    {
        int RandomNum = rnd.Next(1, 20);
        if (RandomNum == 1) //Murder
        {
            GTAWeapon GunToGive = Weapons.Where(x => x.Category == GTAWeapon.WeaponCategory.Pistol).PickRandom();
            Attacker.Inventory.GiveNewWeapon(GunToGive.Name, GunToGive.AmmoAmount, true);
            Attacker.Tasks.FightAgainst(Game.LocalPlayer.Character);
            Attacker.BlockPermanentEvents = true;
            Attacker.KeepTasks = true;
        }
        //else if (RandomNum == 2) //Run Away
        //{
        //    Attacker.Tasks.Cower(30000);
        //    Attacker.BlockPermanentEvents = true;
        //    Attacker.KeepTasks = true;
        //}
        else //Flee
        {
            Attacker.Tasks.Flee(Game.LocalPlayer.Character, 100f, 30000);
            Attacker.BlockPermanentEvents = true;
            Attacker.KeepTasks = true;
        }
    }

    //Weapon Variations
    public static WeaponVariation GetWeaponVariation(Ped WeaponOwner, uint WeaponHash)
    {
        int Tint = NativeFunction.CallByName<int>("GET_PED_WEAPON_TINT_INDEX", WeaponOwner, WeaponHash);
        GTAWeapon MyGun = Weapons.Where(x => x.Hash == WeaponHash).First();
        if (MyGun == null)
            return new WeaponVariation(Tint);

        WeaponComponentsLookup.Where(x => x.BaseWeapon == MyGun.Name);

        if (!WeaponComponentsLookup.Any())
            return new WeaponVariation(Tint);

        List<WeaponVariation.WeaponComponent> Components = new List<WeaponVariation.WeaponComponent>();

        foreach (WeaponVariation.WeaponComponent PossibleComponent in WeaponComponentsLookup.Where(x => x.BaseWeapon == MyGun.Name))
        {
            if (NativeFunction.CallByName<bool>("HAS_PED_GOT_WEAPON_COMPONENT", WeaponOwner, WeaponHash, PossibleComponent.Hash))
            {
                Components.Add(new WeaponVariation.WeaponComponent(PossibleComponent.Name, PossibleComponent.HashKey, PossibleComponent.Hash, true));
            }

        }
        return new WeaponVariation(Tint, Components);

    }
    public static void ApplyWeaponVariation(Ped WeaponOwner, uint WeaponHash, WeaponVariation _WeaponVariation)
    {
        if (_WeaponVariation == null)
            return;
        NativeFunction.CallByName<bool>("SET_PED_WEAPON_TINT_INDEX", WeaponOwner, WeaponHash, _WeaponVariation.Tint);
        GTAWeapon LookupGun = Weapons.Where(x => x.Hash == WeaponHash).FirstOrDefault();
        if (LookupGun == null)
            return;
        List<WeaponVariation.WeaponComponent> PossibleComponents = WeaponComponentsLookup.Where(x => x.BaseWeapon == LookupGun.Name).ToList();
        foreach (WeaponVariation.WeaponComponent ToRemove in PossibleComponents)
        {
            NativeFunction.CallByName<bool>("REMOVE_WEAPON_COMPONENT_FROM_PED", WeaponOwner, WeaponHash, ToRemove.Hash);
        }


        foreach (WeaponVariation.WeaponComponent ToAdd in _WeaponVariation.Components)
        {
            NativeFunction.CallByName<bool>("GIVE_WEAPON_COMPONENT_TO_PED", WeaponOwner, WeaponHash, ToAdd.Hash);
        }
    }

    //Lists
    private static void setupLocations()
    {
        //Hospital
        GTALocation PillBoxHillHospital = new GTALocation(new Vector3(364.7124f, -583.1641f, 28.69318f), 280.637f, GTALocation.LocationType.Hospital, "Pill Box Hill Hospital");
        GTALocation CentralLosStantosHospital = new GTALocation(new Vector3(338.208f, -1396.154f, 32.50927f), 77.07102f, GTALocation.LocationType.Hospital, "Central Los Santos Hospital");
        GTALocation SandyShoresHospital = new GTALocation(new Vector3(1842.057f, 3668.679f, 33.67996f), 228.3818f, GTALocation.LocationType.Hospital, "Sandy Shores Hospital");
        GTALocation PaletoBayHospital = new GTALocation(new Vector3(-244.3214f, 6328.575f, 32.42618f), 219.7734f, GTALocation.LocationType.Hospital, "Paleto Bay Hospital");

        Locations.Add(PillBoxHillHospital);
        Locations.Add(CentralLosStantosHospital);
        Locations.Add(SandyShoresHospital);
        Locations.Add(PaletoBayHospital);

        //Police
        GTALocation DavisPolice = new GTALocation(new Vector3(358.9726f, -1582.881f, 29.29195f), 323.5287f, GTALocation.LocationType.Police, "Davis Police Station");
        GTALocation SandyShoresPolice = new GTALocation(new Vector3(1858.19f, 3679.873f, 33.75724f), 218.3256f, GTALocation.LocationType.Police, "Sandy Shores Police Station");
        GTALocation PaletoBayPolice = new GTALocation(new Vector3(-437.973f, 6021.403f, 31.49011f), 316.3756f, GTALocation.LocationType.Police, "Paleto Bay Police Station");
        GTALocation MissionRowPolice = new GTALocation(new Vector3(440.0835f, -982.3911f, 30.68966f), 47.88088f, GTALocation.LocationType.Police, "Mission Row Police Station");
        GTALocation LasMesaPolice = new GTALocation(new Vector3(815.8774f, -1290.531f, 26.28391f), 74.91704f, GTALocation.LocationType.Police, "La Mesa Police Station");
        GTALocation VinewoodPolice = new GTALocation(new Vector3(642.1356f, -3.134667f, 82.78872f), 215.299f, GTALocation.LocationType.Police, "Vinewood Police Station");
        GTALocation RockfordHillsPolice = new GTALocation(new Vector3(-557.0687f, -134.7315f, 38.20231f), 214.5968f, GTALocation.LocationType.Police, "Vinewood Police Station");
        GTALocation VespucciPolice = new GTALocation(new Vector3(-1093.817f, -807.1993f, 19.28864f), 22.23846f, GTALocation.LocationType.Police, "Vinewood Police Station");

        Locations.Add(DavisPolice);
        Locations.Add(SandyShoresPolice);
        Locations.Add(PaletoBayPolice);
        Locations.Add(MissionRowPolice);
        Locations.Add(LasMesaPolice);
        Locations.Add(VinewoodPolice);
        Locations.Add(RockfordHillsPolice);
        Locations.Add(VespucciPolice);

        //Stores
        GTALocation LTDGasLIttleSeoul = new GTALocation(new Vector3(-709.68f, -923.198f, 19.0193f), 22.23846f, GTALocation.LocationType.ConvenienceStore, "LTD Gas - Little Seoul");
        GTALocation RobsLiquors = new GTALocation(new Vector3(-1226.09f, -896.166f, 12.4057f), 22.23846f, GTALocation.LocationType.ConvenienceStore, "Rob's Liquors");
        GTALocation Store1 = new GTALocation(new Vector3(1725f, 6410f, 35f), 22.23846f, GTALocation.LocationType.ConvenienceStore, "24/7 Store");
        GTALocation Store2 = new GTALocation(new Vector3(2560f, 385f, 107f), 22.23846f, GTALocation.LocationType.ConvenienceStore, "24/7 Store");
        GTALocation Store3 = new GTALocation(new Vector3(547f, 2678f, 41f), 22.23846f, GTALocation.LocationType.ConvenienceStore, "24/7 Store");


        Locations.Add(LTDGasLIttleSeoul);
        Locations.Add(RobsLiquors);
        Locations.Add(Store1);
        Locations.Add(Store2);
        Locations.Add(Store3);

    }
    private static void setupWeapons()
    {
        //Melee
        Weapons.Add(new GTAWeapon("weapon_dagger", 0, GTAWeapon.WeaponCategory.Melee, 0, 2460120199,false,false,false));
        Weapons.Add(new GTAWeapon("weapon_bat", 0, GTAWeapon.WeaponCategory.Melee, 0, 2508868239, false, false, false));
        Weapons.Add(new GTAWeapon("weapon_bottle", 0, GTAWeapon.WeaponCategory.Melee, 0, 4192643659, false, false, false));
        Weapons.Add(new GTAWeapon("weapon_crowbar", 0, GTAWeapon.WeaponCategory.Melee, 0, 2227010557, false, false, false));
        Weapons.Add(new GTAWeapon("weapon_flashlight", 0, GTAWeapon.WeaponCategory.Melee, 0, 2343591895, false, false, false));
        Weapons.Add(new GTAWeapon("weapon_golfclub", 0, GTAWeapon.WeaponCategory.Melee, 0, 1141786504, false, false, false));
        Weapons.Add(new GTAWeapon("weapon_hammer", 0, GTAWeapon.WeaponCategory.Melee, 0, 1317494643, false, false, false));
        Weapons.Add(new GTAWeapon("weapon_hatchet", 0, GTAWeapon.WeaponCategory.Melee, 0, 4191993645, false, false, false));
        Weapons.Add(new GTAWeapon("weapon_knuckle", 0, GTAWeapon.WeaponCategory.Melee, 0, 3638508604, false, false, false));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Base Model", "COMPONENT_KNUCKLE_VARMOD_BASE", 0xF3462F33, false, "weapon_knuckle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("The Pimp", "COMPONENT_KNUCKLE_VARMOD_PIMP", 0xC613F685, false, "weapon_knuckle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("The Ballas", "COMPONENT_KNUCKLE_VARMOD_BALLAS", 0xEED9FD63, false, "weapon_knuckle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("The Hustler", "COMPONENT_KNUCKLE_VARMOD_DOLLAR", 0x50910C31, false, "weapon_knuckle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("The Rock", "COMPONENT_KNUCKLE_VARMOD_DIAMOND", 0x9761D9DC, false, "weapon_knuckle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("The Hater", "COMPONENT_KNUCKLE_VARMOD_HATE", 0x7DECFE30, false, "weapon_knuckle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("The Lover", "COMPONENT_KNUCKLE_VARMOD_LOVE", 0x3F4E8AA6, false, "weapon_knuckle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("The Player", "COMPONENT_KNUCKLE_VARMOD_PLAYER", 0x8B808BB, false, "weapon_knuckle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("The King", "COMPONENT_KNUCKLE_VARMOD_KING", 0xE28BABEF, false, "weapon_knuckle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("The Vagos", "COMPONENT_KNUCKLE_VARMOD_VAGOS", 0x7AF3F785, false, "weapon_knuckle"));
        Weapons.Add(new GTAWeapon("weapon_knife", 0, GTAWeapon.WeaponCategory.Melee, 0, 2578778090, false, false, false));
        Weapons.Add(new GTAWeapon("weapon_machete", 0, GTAWeapon.WeaponCategory.Melee, 0, 3713923289, false, false, false));
        Weapons.Add(new GTAWeapon("weapon_switchblade", 0, GTAWeapon.WeaponCategory.Melee, 0, 3756226112, false, false, false));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Handle", "COMPONENT_SWITCHBLADE_VARMOD_BASE", 0x9137A500, false, "weapon_switchblade"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("VIP Variant", "COMPONENT_SWITCHBLADE_VARMOD_VAR1", 0x5B3E7DB6, false, "weapon_switchblade"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Bodyguard Variant", "COMPONENT_SWITCHBLADE_VARMOD_VAR2", 0xE7939662, false, "weapon_switchblade"));
        Weapons.Add(new GTAWeapon("weapon_nightstick", 0, GTAWeapon.WeaponCategory.Melee, 0, 1737195953, false, false, false));
        Weapons.Add(new GTAWeapon("weapon_wrench", 0, GTAWeapon.WeaponCategory.Melee, 0, 0x19044EE0, false, false, false));
        Weapons.Add(new GTAWeapon("weapon_battleaxe", 0, GTAWeapon.WeaponCategory.Melee, 0, 3441901897, false, false, false));
        Weapons.Add(new GTAWeapon("weapon_poolcue", 0, GTAWeapon.WeaponCategory.Melee, 0, 0x94117305, false, false, false));
        Weapons.Add(new GTAWeapon("weapon_stone_hatchet", 0, GTAWeapon.WeaponCategory.Melee, 0, 0x3813FC08, false, false, false));
        //Pistol
        Weapons.Add(new GTAWeapon("weapon_pistol", 60, GTAWeapon.WeaponCategory.Pistol, 1, 453432689, true,true,false));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_PISTOL_CLIP_01", 0xFED0FD71, false, "weapon_pistol"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_PISTOL_CLIP_02", 0xED265A1C, false, "weapon_pistol"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, false, "weapon_pistol"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_PI_SUPP_02", 0x65EA7EBB, false, "weapon_pistol"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Yusuf Amir Luxury Finish", "COMPONENT_PISTOL_VARMOD_LUXE", 0xD7391086, false, "weapon_pistol"));
        Weapons.Add(new GTAWeapon("weapon_pistol_mk2", 60, GTAWeapon.WeaponCategory.Pistol, 1, 0xBFE256D4, true,true,false));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_PISTOL_MK2_CLIP_01", 0x94F42D62, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_PISTOL_MK2_CLIP_02", 0x5ED6C128, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Tracer Rounds", "COMPONENT_PISTOL_MK2_CLIP_TRACER", 0x25CAAEAF, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Incendiary Rounds", "COMPONENT_PISTOL_MK2_CLIP_INCENDIARY", 0x2BBD7A3A, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Hollow Point Rounds", "COMPONENT_PISTOL_MK2_CLIP_HOLLOWPOINT", 0x85FEA109, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Full Metal Jacket Rounds", "COMPONENT_PISTOL_MK2_CLIP_FMJ", 0x4F37DF2A, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Mounted Scope", "COMPONENT_AT_PI_RAIL", 0x8ED4BB70, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH_02", 0x43FD595B, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_PI_SUPP_02", 0x65EA7EBB, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Compensator", "COMPONENT_AT_PI_COMP", 0x21E34793, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Digital Camo", "COMPONENT_PISTOL_MK2_CAMO", 0x5C6C749C, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Brushstroke Camo", "COMPONENT_PISTOL_MK2_CAMO_02", 0x15F7A390, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Woodland Camo", "COMPONENT_PISTOL_MK2_CAMO_03", 0x968E24DB, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Skull", "COMPONENT_PISTOL_MK2_CAMO_04", 0x17BFA99, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Sessanta Nove", "COMPONENT_PISTOL_MK2_CAMO_05", 0xF2685C72, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Perseus", "COMPONENT_PISTOL_MK2_CAMO_06", 0xDD2231E6, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Leopard", "COMPONENT_PISTOL_MK2_CAMO_07", 0xBB43EE76, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Zebra", "COMPONENT_PISTOL_MK2_CAMO_08", 0x4D901310, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Geometric", "COMPONENT_PISTOL_MK2_CAMO_09", 0x5F31B653, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Boom!", "COMPONENT_PISTOL_MK2_CAMO_10", 0x697E19A0, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Patriotic", "COMPONENT_PISTOL_MK2_CAMO_IND_01", 0x930CB951, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Digital Camo", "COMPONENT_PISTOL_MK2_CAMO_SLIDE", 0xB4FC92B0, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Digital Camo", "COMPONENT_PISTOL_MK2_CAMO_02_SLIDE", 0x1A1F1260, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Digital Camo", "COMPONENT_PISTOL_MK2_CAMO_03_SLIDE", 0xE4E00B70, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Digital Camo", "COMPONENT_PISTOL_MK2_CAMO_04_SLIDE", 0x2C298B2B, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Digital Camo", "COMPONENT_PISTOL_MK2_CAMO_05_SLIDE", 0xDFB79725, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Digital Camo", "COMPONENT_PISTOL_MK2_CAMO_06_SLIDE", 0x6BD7228C, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Digital Camo", "COMPONENT_PISTOL_MK2_CAMO_07_SLIDE", 0x9DDBCF8C, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Digital Camo", "COMPONENT_PISTOL_MK2_CAMO_08_SLIDE", 0xB319A52C, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Digital Camo", "COMPONENT_PISTOL_MK2_CAMO_09_SLIDE", 0xC6836E12, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Digital Camo", "COMPONENT_PISTOL_MK2_CAMO_10_SLIDE", 0x43B1B173, false, "weapon_pistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Patriotic", "COMPONENT_PISTOL_MK2_CAMO_IND_01_SLIDE", 0x4ABDA3FA, false, "weapon_pistol_mk2"));
        Weapons.Add(new GTAWeapon("weapon_combatpistol", 60, GTAWeapon.WeaponCategory.Pistol, 1, 1593441988, true, true,false));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_COMBATPISTOL_CLIP_01", 0x721B079, false, "weapon_combatpistol"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_COMBATPISTOL_CLIP_02", 0xD67B4F2D, false, "weapon_combatpistol"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, false, "weapon_combatpistol"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_PI_SUPP", 0xC304849A, false, "weapon_combatpistol"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Yusuf Amir Luxury Finish", "COMPONENT_COMBATPISTOL_VARMOD_LOWRIDER", 0xC6654D72, false, "weapon_combatpistol"));
        Weapons.Add(new GTAWeapon("weapon_appistol", 60, GTAWeapon.WeaponCategory.Pistol, 1, 584646201, false, true, false));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_APPISTOL_CLIP_01", 0x31C4B22A, false, "weapon_appistol"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_APPISTOL_CLIP_02", 0x249A17D5, false, "weapon_appistol"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, false, "weapon_appistol"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_PI_SUPP", 0xC304849A, false, "weapon_appistol"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Gilded Gun Metal Finish", "COMPONENT_APPISTOL_VARMOD_LUXE", 0x9B76C72C, false, "weapon_appistol"));
        Weapons.Add(new GTAWeapon("weapon_stungun", 0, GTAWeapon.WeaponCategory.Pistol, 1, 911657153, false, true, false));
        Weapons.Add(new GTAWeapon("weapon_pistol50", 60, GTAWeapon.WeaponCategory.Pistol, 1, 2578377531, false, true, false));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_PISTOL50_CLIP_01", 0x2297BE19, false, "weapon_pistol50"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_PISTOL50_CLIP_02", 0xD9D3AC92, false, "weapon_pistol50"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, false, "weapon_pistol50"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP_02", 0xA73D4664, false, "weapon_pistol50"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Platinum Pearl Deluxe Finish", "COMPONENT_PISTOL50_VARMOD_LUXE", 0x77B8AB2F, false, "weapon_pistol50"));
        Weapons.Add(new GTAWeapon("weapon_snspistol", 60, GTAWeapon.WeaponCategory.Pistol, 1, 3218215474, false, true, false));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_SNSPISTOL_CLIP_01", 0xF8802ED9, false, "weapon_snspistol"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_SNSPISTOL_CLIP_02", 0x7B0033B3, false, "weapon_snspistol"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Etched Wood Grip Finish", "COMPONENT_SNSPISTOL_VARMOD_LOWRIDER", 0x8033ECAF, false, "weapon_snspistol"));
        Weapons.Add(new GTAWeapon("weapon_snspistol_mk2", 60, GTAWeapon.WeaponCategory.Pistol, 1, 0x88374054, false, true, false));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_SNSPISTOL_MK2_CLIP_01", 0x1466CE6, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_SNSPISTOL_MK2_CLIP_02", 0xCE8C0772, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Tracer Rounds", "COMPONENT_SNSPISTOL_MK2_CLIP_TRACER", 0x902DA26E, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Incendiary Rounds", "COMPONENT_SNSPISTOL_MK2_CLIP_INCENDIARY", 0xE6AD5F79, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Hollow Point Rounds", "COMPONENT_SNSPISTOL_MK2_CLIP_HOLLOWPOINT", 0x8D107402, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Full Metal Jacket Rounds", "COMPONENT_SNSPISTOL_MK2_CLIP_FMJ", 0xC111EB26, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH_03", 0x4A4965F3, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Mounted Scope", "COMPONENT_AT_PI_RAIL_02", 0x47DE9258, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_PI_SUPP_02", 0x65EA7EBB, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Compensator", "COMPONENT_AT_PI_COMP_02", 0xAA8283BF, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Digital Camo", "COMPONENT_SNSPISTOL_MK2_CAMO", 0xF7BEEDD, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Brushstroke Camo", "COMPONENT_SNSPISTOL_MK2_CAMO_02", 0x8A612EF6, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Woodland Camo", "COMPONENT_SNSPISTOL_MK2_CAMO_03", 0x76FA8829, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Skull", "COMPONENT_SNSPISTOL_MK2_CAMO_04", 0xA93C6CAC, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Sessanta Nove", "COMPONENT_SNSPISTOL_MK2_CAMO_05", 0x9C905354, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Perseus", "COMPONENT_SNSPISTOL_MK2_CAMO_06", 0x4DFA3621, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Leopard", "COMPONENT_SNSPISTOL_MK2_CAMO_07", 0x42E91FFF, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Zebra", "COMPONENT_SNSPISTOL_MK2_CAMO_08", 0x54A8437D, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Geometric", "COMPONENT_SNSPISTOL_MK2_CAMO_09", 0x68C2746, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Boom!", "COMPONENT_SNSPISTOL_MK2_CAMO_10", 0x2366E467, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Boom!", "COMPONENT_SNSPISTOL_MK2_CAMO_IND_01", 0x441882E6, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Digital Camo", "COMPONENT_SNSPISTOL_MK2_CAMO_SLIDE", 0xE7EE68EA, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Brushstroke Camo", "COMPONENT_SNSPISTOL_MK2_CAMO_02_SLIDE", 0x29366D21, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Woodland Camo", "COMPONENT_SNSPISTOL_MK2_CAMO_03_SLIDE", 0x3ADE514B, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Skull", "COMPONENT_SNSPISTOL_MK2_CAMO_04_SLIDE", 0xE64513E9, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Sessanta Nove", "COMPONENT_SNSPISTOL_MK2_CAMO_05_SLIDE", 0xCD7AEB9A, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Perseus", "COMPONENT_SNSPISTOL_MK2_CAMO_06_SLIDE", 0xFA7B27A6, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Leopard", "COMPONENT_SNSPISTOL_MK2_CAMO_07_SLIDE", 0xE285CA9A, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Zebra", "COMPONENT_SNSPISTOL_MK2_CAMO_08_SLIDE", 0x2B904B19, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Geometric", "COMPONENT_SNSPISTOL_MK2_CAMO_09_SLIDE", 0x22C24F9C, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Boom!", "COMPONENT_SNSPISTOL_MK2_CAMO_10_SLIDE", 0x8D0D5ECD, false, "weapon_snspistol_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Patriotic", "COMPONENT_SNSPISTOL_MK2_CAMO_IND_01_SLIDE", 0x1F07150A, false, "weapon_snspistol_mk2"));
        Weapons.Add(new GTAWeapon("weapon_heavypistol", 60, GTAWeapon.WeaponCategory.Pistol, 1, 3523564046, true,true,false));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_HEAVYPISTOL_CLIP_01", 0xD4A969A, false, "weapon_heavypistol"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_HEAVYPISTOL_CLIP_02", 0x64F9C62B, false, "weapon_heavypistol"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, false, "weapon_heavypistol"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_PI_SUPP", 0xC304849A, false, "weapon_heavypistol"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Etched Wood Grip Finish", "COMPONENT_HEAVYPISTOL_VARMOD_LUXE", 0x7A6A7B7B, false, "weapon_heavypistol"));
        Weapons.Add(new GTAWeapon("weapon_vintagepistol", 60, GTAWeapon.WeaponCategory.Pistol, 1, 137902532, false, true, false));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_VINTAGEPISTOL_CLIP_01", 0x45A3B6BB, false, "weapon_vintagepistol"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_VINTAGEPISTOL_CLIP_02", 0x33BA12E8, false, "weapon_vintagepistol"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_PI_SUPP", 0xC304849A, false, "weapon_vintagepistol"));
        Weapons.Add(new GTAWeapon("weapon_flaregun", 60, GTAWeapon.WeaponCategory.Pistol, 1, 1198879012, false, true, false));
        Weapons.Add(new GTAWeapon("weapon_marksmanpistol", 60, GTAWeapon.WeaponCategory.Pistol, 1, 3696079510, false, true, false));
        Weapons.Add(new GTAWeapon("weapon_revolver", 60, GTAWeapon.WeaponCategory.Pistol, 1, 3249783761, false, true, false));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("VIP Variant", "COMPONENT_REVOLVER_VARMOD_BOSS", 0x16EE3040, false, "weapon_revolver"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Bodyguard Variant", "COMPONENT_REVOLVER_VARMOD_GOON", 0x9493B80D, false, "weapon_revolver"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_REVOLVER_CLIP_01", 0xE9867CE3, false, "weapon_revolver"));
        Weapons.Add(new GTAWeapon("weapon_revolver_mk2", 60, GTAWeapon.WeaponCategory.Pistol, 1, 0xCB96392F, false, true, false));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Rounds", "COMPONENT_REVOLVER_MK2_CLIP_01", 0xBA23D8BE, false, "weapon_revolver_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Tracer Rounds", "COMPONENT_REVOLVER_MK2_CLIP_TRACER", 0xC6D8E476, false, "weapon_revolver_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Incendiary Rounds", "COMPONENT_REVOLVER_MK2_CLIP_INCENDIARY", 0xEFBF25, false, "weapon_revolver_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Hollow Point Rounds", "COMPONENT_REVOLVER_MK2_CLIP_HOLLOWPOINT", 0x10F42E8F, false, "weapon_revolver_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Full Metal Jacket Rounds", "COMPONENT_REVOLVER_MK2_CLIP_FMJ", 0xDC8BA3F, false, "weapon_revolver_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS", 0x420FD713, false, "weapon_revolver_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Small Scope", "COMPONENT_AT_SCOPE_MACRO_MK2", 0x49B2945, false, "weapon_revolver_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, false, "weapon_revolver_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Compensator", "COMPONENT_AT_PI_COMP_03", 0x27077CCB, false, "weapon_revolver_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Digital Camo", "COMPONENT_REVOLVER_MK2_CAMO", 0xC03FED9F, false, "weapon_revolver_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Brushstroke Camo", "COMPONENT_REVOLVER_MK2_CAMO_02", 0xB5DE24, false, "weapon_revolver_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Woodland Camo", "COMPONENT_REVOLVER_MK2_CAMO_03", 0xA7FF1B8, false, "weapon_revolver_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Skull", "COMPONENT_REVOLVER_MK2_CAMO_04", 0xF2E24289, false, "weapon_revolver_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Sessanta Nove", "COMPONENT_REVOLVER_MK2_CAMO_05", 0x11317F27, false, "weapon_revolver_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Perseus", "COMPONENT_REVOLVER_MK2_CAMO_06", 0x17C30C42, false, "weapon_revolver_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Leopard", "COMPONENT_REVOLVER_MK2_CAMO_07", 0x257927AE, false, "weapon_revolver_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Zebra", "COMPONENT_REVOLVER_MK2_CAMO_08", 0x37304B1C, false, "weapon_revolver_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Geometric", "COMPONENT_REVOLVER_MK2_CAMO_09", 0x48DAEE71, false, "weapon_revolver_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Boom!", "COMPONENT_REVOLVER_MK2_CAMO_10", 0x20ED9B5B, false, "weapon_revolver_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Patriotic", "COMPONENT_REVOLVER_MK2_CAMO_IND_01", 0xD951E867, false, "weapon_revolver_mk2"));
        Weapons.Add(new GTAWeapon("weapon_doubleaction", 60, GTAWeapon.WeaponCategory.Pistol, 1, 0x97EA20B8, false, true, false));
        Weapons.Add(new GTAWeapon("weapon_raypistol", 60, GTAWeapon.WeaponCategory.Pistol, 1, 0xAF3696A1, false, true, false));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Festive tint", "COMPONENT_RAYPISTOL_VARMOD_XMAS18", 0xD7DBF707, false, "weapon_raypistol"));
        //Shotgun
        Weapons.Add(new GTAWeapon("weapon_pumpshotgun", 32, GTAWeapon.WeaponCategory.Shotgun, 2, 487013001, true,false,true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_pumpshotgun"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_SR_SUPP", 0xE608B35E, false, "weapon_pumpshotgun"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Yusuf Amir Luxury Finish", "COMPONENT_PUMPSHOTGUN_VARMOD_LOWRIDER", 0xA2D79DDB, false, "weapon_pumpshotgun"));
        Weapons.Add(new GTAWeapon("weapon_pumpshotgun_mk2", 32, GTAWeapon.WeaponCategory.Shotgun, 2, 0x555AF99A, true,false,true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Shells", "COMPONENT_PUMPSHOTGUN_MK2_CLIP_01", 0xCD940141, false, "weapon_pumpshotgun_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Dragon's Breath Shells", "COMPONENT_PUMPSHOTGUN_MK2_CLIP_INCENDIARY", 0x9F8A1BF5, false, "weapon_pumpshotgun_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Steel Buckshot Shells", "COMPONENT_PUMPSHOTGUN_MK2_CLIP_ARMORPIERCING", 0x4E65B425, false, "weapon_pumpshotgun_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flechette Shells", "COMPONENT_PUMPSHOTGUN_MK2_CLIP_HOLLOWPOINT", 0xE9582927, false, "weapon_pumpshotgun_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Explosive Slugs", "COMPONENT_PUMPSHOTGUN_MK2_CLIP_EXPLOSIVE", 0x3BE4465D, false, "weapon_pumpshotgun_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS", 0x420FD713, false, "weapon_pumpshotgun_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Small Scope", "COMPONENT_AT_SCOPE_MACRO_MK2", 0x49B2945, false, "weapon_pumpshotgun_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Medium Scope", "COMPONENT_AT_SCOPE_SMALL_MK2", 0x3F3C8181, false, "weapon_pumpshotgun_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_pumpshotgun_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_SR_SUPP_03", 0xAC42DF71, false, "weapon_pumpshotgun_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Squared Muzzle Brake", "COMPONENT_AT_MUZZLE_08", 0x5F7DCE4D, false, "weapon_pumpshotgun_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Digital Camo", "COMPONENT_PUMPSHOTGUN_MK2_CAMO", 0xE3BD9E44, false, "weapon_pumpshotgun_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Brushstroke Camo", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_02", 0x17148F9B, false, "weapon_pumpshotgun_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Woodland Camo", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_03", 0x24D22B16, false, "weapon_pumpshotgun_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Skull", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_04", 0xF2BEC6F0, false, "weapon_pumpshotgun_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Sessanta Nove", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_05", 0x85627D, false, "weapon_pumpshotgun_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Perseus", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_06", 0xDC2919C5, false, "weapon_pumpshotgun_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Leopard", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_07", 0xE184247B, false, "weapon_pumpshotgun_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Zebra", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_08", 0xD8EF9356, false, "weapon_pumpshotgun_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Geometric", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_09", 0xEF29BFCA, false, "weapon_pumpshotgun_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Boom!", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_10", 0x67AEB165, false, "weapon_pumpshotgun_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Patriotic", "COMPONENT_PUMPSHOTGUN_MK2_CAMO_IND_01", 0x46411A1D, false, "weapon_pumpshotgun_mk2"));
        Weapons.Add(new GTAWeapon("weapon_sawnoffshotgun", 32, GTAWeapon.WeaponCategory.Shotgun, 2, 2017895192, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Gilded Gun Metal Finish", "COMPONENT_SAWNOFFSHOTGUN_VARMOD_LUXE", 0x85A64DF9, false, "weapon_sawnoffshotgun"));
        Weapons.Add(new GTAWeapon("weapon_assaultshotgun", 32, GTAWeapon.WeaponCategory.Shotgun, 2, 3800352039, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_ASSAULTSHOTGUN_CLIP_01", 0x94E81BC7, false, "weapon_assaultshotgun"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_ASSAULTSHOTGUN_CLIP_02", 0x86BD7F72, false, "weapon_assaultshotgun"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_assaultshotgun"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP", 0x837445AA, false, "weapon_assaultshotgun"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_assaultshotgun"));
        Weapons.Add(new GTAWeapon("weapon_bullpupshotgun", 32, GTAWeapon.WeaponCategory.Shotgun, 2, 2640438543, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_bullpupshotgun"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP_02", 0xA73D4664, false, "weapon_bullpupshotgun"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_bullpupshotgun"));
        Weapons.Add(new GTAWeapon("weapon_musket", 32, GTAWeapon.WeaponCategory.Shotgun, 2, 2828843422, false, false, true));
        Weapons.Add(new GTAWeapon("weapon_heavyshotgun", 32, GTAWeapon.WeaponCategory.Shotgun, 2, 984333226, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_HEAVYSHOTGUN_CLIP_01", 0x324F2D5F, false, "weapon_heavyshotgun"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_HEAVYSHOTGUN_CLIP_02", 0x971CF6FD, false, "weapon_heavyshotgun"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Drum Magazine", "COMPONENT_HEAVYSHOTGUN_CLIP_03", 0x88C7DA53, false, "weapon_heavyshotgun"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_heavyshotgun"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP_02", 0xA73D4664, false, "weapon_heavyshotgun"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_heavyshotgun"));
        Weapons.Add(new GTAWeapon("weapon_dbshotgun", 32, GTAWeapon.WeaponCategory.Shotgun, 2, 4019527611, false, false, true));
        Weapons.Add(new GTAWeapon("weapon_autoshotgun", 32, GTAWeapon.WeaponCategory.Shotgun, 2, 317205821, false, false, true));
        //SMG
        Weapons.Add(new GTAWeapon("weapon_microsmg", 32, GTAWeapon.WeaponCategory.SMG, 2, 324215364, false, true, false));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_MICROSMG_CLIP_01", 0xCB48AEF0, false, "weapon_microsmg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_MICROSMG_CLIP_02", 0x10E6BA2B, false, "weapon_microsmg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, false, "weapon_microsmg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_MACRO", 0x9D2FBF29, false, "weapon_microsmg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP_02", 0xA73D4664, false, "weapon_microsmg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Yusuf Amir Luxury Finish", "COMPONENT_MICROSMG_VARMOD_LUXE", 0x487AAE09, false, "weapon_microsmg"));
        Weapons.Add(new GTAWeapon("weapon_smg", 32, GTAWeapon.WeaponCategory.SMG, 2, 736523883, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_SMG_CLIP_01", 0x26574997, false, "weapon_smg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_SMG_CLIP_02", 0x350966FB, false, "weapon_smg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Drum Magazine", "COMPONENT_SMG_CLIP_03", 0x79C77076, false, "weapon_smg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_smg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_MACRO_02", 0x3CC6BA57, false, "weapon_smg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_PI_SUPP", 0xC304849A, false, "weapon_smg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Yusuf Amir Luxury Finish", "COMPONENT_SMG_VARMOD_LUXE", 0x27872C90, false, "weapon_smg"));
        Weapons.Add(new GTAWeapon("weapon_smg_mk2", 32, GTAWeapon.WeaponCategory.SMG, 2, 0x78A97CD0, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_SMG_MK2_CLIP_01", 0x4C24806E, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_SMG_MK2_CLIP_02", 0xB9835B2E, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Tracer Rounds", "COMPONENT_SMG_MK2_CLIP_TRACER", 0x7FEA36EC, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Incendiary Rounds", "COMPONENT_SMG_MK2_CLIP_INCENDIARY", 0xD99222E5, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Hollow Point Rounds", "COMPONENT_SMG_MK2_CLIP_HOLLOWPOINT", 0x3A1BD6FA, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Full Metal Jacket Rounds", "COMPONENT_SMG_MK2_CLIP_FMJ", 0xB5A715F, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS_SMG", 0x9FDB5652, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Small Scope", "COMPONENT_AT_SCOPE_MACRO_02_SMG_MK2", 0xE502AB6B, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Medium Scope", "COMPONENT_AT_SCOPE_SMALL_SMG_MK2", 0x3DECC7DA, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_PI_SUPP", 0xC304849A, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flat Muzzle Brake", "COMPONENT_AT_MUZZLE_01", 0xB99402D4, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Tactical Muzzle Brake", "COMPONENT_AT_MUZZLE_02", 0xC867A07B, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Fat-End Muzzle Brake", "COMPONENT_AT_MUZZLE_03", 0xDE11CBCF, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Precision Muzzle Brake", "COMPONENT_AT_MUZZLE_04", 0xEC9068CC, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Heavy Duty Muzzle Brake", "COMPONENT_AT_MUZZLE_05", 0x2E7957A, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Slanted Muzzle Brake", "COMPONENT_AT_MUZZLE_06", 0x347EF8AC, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Split-End Muzzle Brake", "COMPONENT_AT_MUZZLE_07", 0x4DB62ABE, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Barrel", "COMPONENT_AT_SB_BARREL_01", 0xD9103EE1, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Heavy Barrel", "COMPONENT_AT_SB_BARREL_02", 0xA564D78B, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Digital Camo", "COMPONENT_SMG_MK2_CAMO", 0xC4979067, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Brushstroke Camo", "COMPONENT_SMG_MK2_CAMO_02", 0x3815A945, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Woodland Camo", "COMPONENT_SMG_MK2_CAMO_03", 0x4B4B4FB0, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Skull", "COMPONENT_SMG_MK2_CAMO_04", 0xEC729200, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Sessanta Nove", "COMPONENT_SMG_MK2_CAMO_05", 0x48F64B22, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Perseus", "COMPONENT_SMG_MK2_CAMO_06", 0x35992468, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Leopard", "COMPONENT_SMG_MK2_CAMO_07", 0x24B782A5, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Zebra", "COMPONENT_SMG_MK2_CAMO_08", 0xA2E67F01, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Geometric", "COMPONENT_SMG_MK2_CAMO_09", 0x2218FD68, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Boom!", "COMPONENT_SMG_MK2_CAMO_10", 0x45C5C3C5, false, "weapon_smg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Patriotic", "COMPONENT_SMG_MK2_CAMO_IND_01", 0x399D558F, false, "weapon_smg_mk2"));
        Weapons.Add(new GTAWeapon("weapon_assaultsmg", 32, GTAWeapon.WeaponCategory.SMG, 2, 4024951519, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_ASSAULTSMG_CLIP_01", 0x8D1307B0, false, "weapon_assaultsmg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_ASSAULTSMG_CLIP_02", 0xBB46E417, false, "weapon_assaultsmg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_assaultsmg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_MACRO", 0x9D2FBF29, false, "weapon_assaultsmg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP_02", 0xA73D4664, false, "weapon_assaultsmg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Yusuf Amir Luxury Finish", "COMPONENT_ASSAULTSMG_VARMOD_LOWRIDER", 0x278C78AF, false, "weapon_assaultsmg"));
        Weapons.Add(new GTAWeapon("weapon_combatpdw", 32, GTAWeapon.WeaponCategory.SMG, 2, 171789620, true,false,true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_COMBATPDW_CLIP_01", 0x4317F19E, false, "weapon_combatpdw"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_COMBATPDW_CLIP_02", 0x334A5203, false, "weapon_combatpdw"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Drum Magazine", "COMPONENT_COMBATPDW_CLIP_03", 0x6EB8C8DB, false, "weapon_combatpdw"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_combatpdw"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_combatpdw"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_SMALL", 0xAA2C45B4, false, "weapon_combatpdw"));
        Weapons.Add(new GTAWeapon("weapon_machinepistol", 32, GTAWeapon.WeaponCategory.SMG, 2, 3675956304, false, true, false));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_MACHINEPISTOL_CLIP_01", 0x476E85FF, false, "weapon_machinepistol"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_MACHINEPISTOL_CLIP_02", 0xB92C6979, false, "weapon_machinepistol"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Drum Magazine", "COMPONENT_MACHINEPISTOL_CLIP_03", 0xA9E9CAF4, false, "weapon_machinepistol"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_PI_SUPP", 0xC304849A, false, "weapon_machinepistol"));
        Weapons.Add(new GTAWeapon("weapon_minismg", 32, GTAWeapon.WeaponCategory.SMG, 2, 3173288789, false, true, false));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_MINISMG_CLIP_01", 0x84C8B2D3, false, "weapon_minismg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_MINISMG_CLIP_02", 0x937ED0B7, false, "weapon_minismg"));
        Weapons.Add(new GTAWeapon("weapon_raycarbine", 32, GTAWeapon.WeaponCategory.SMG, 2, 0x476BF155, false, true, false));
        //AR
        Weapons.Add(new GTAWeapon("weapon_assaultrifle", 120, GTAWeapon.WeaponCategory.AR, 3, 3220176749, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_ASSAULTRIFLE_CLIP_01", 0xBE5EEA16, false, "weapon_assaultrifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_ASSAULTRIFLE_CLIP_02", 0xB1214F9B, false, "weapon_assaultrifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Drum Magazine", "COMPONENT_ASSAULTRIFLE_CLIP_03", 0xDBF0A53D, false, "weapon_assaultrifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_assaultrifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_MACRO", 0x9D2FBF29, false, "weapon_assaultrifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP_02", 0xA73D4664, false, "weapon_assaultrifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_assaultrifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Yusuf Amir Luxury Finish", "COMPONENT_ASSAULTRIFLE_VARMOD_LUXE", 0x4EAD7533, false, "weapon_assaultrifle"));
        Weapons.Add(new GTAWeapon("weapon_assaultrifle_mk2", 120, GTAWeapon.WeaponCategory.AR, 3, 0x394F415C, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_ASSAULTRIFLE_MK2_CLIP_01", 0x8610343F, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_ASSAULTRIFLE_MK2_CLIP_02", 0xD12ACA6F, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Tracer Rounds", "COMPONENT_ASSAULTRIFLE_MK2_CLIP_TRACER", 0xEF2C78C1, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Incendiary Rounds", "COMPONENT_ASSAULTRIFLE_MK2_CLIP_INCENDIARY", 0xFB70D853, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Armor Piercing Rounds", "COMPONENT_ASSAULTRIFLE_MK2_CLIP_ARMORPIERCING", 0xA7DD1E58, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Full Metal Jacket Rounds", "COMPONENT_ASSAULTRIFLE_MK2_CLIP_FMJ", 0x63E0A098, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP_02", 0x9D65907A, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS", 0x420FD713, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Small Scope", "COMPONENT_AT_SCOPE_MACRO_MK2", 0x49B2945, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Large Scope", "COMPONENT_AT_SCOPE_MEDIUM_MK2", 0xC66B6542, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP_02", 0xA73D4664, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flat Muzzle Brake", "COMPONENT_AT_MUZZLE_01", 0xB99402D4, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Tactical Muzzle Brake", "COMPONENT_AT_MUZZLE_02", 0xC867A07B, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Fat-End Muzzle Brake", "COMPONENT_AT_MUZZLE_03", 0xDE11CBCF, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Precision Muzzle Brake", "COMPONENT_AT_MUZZLE_04", 0xEC9068CC, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Heavy Duty Muzzle Brake", "COMPONENT_AT_MUZZLE_05", 0x2E7957A, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Slanted Muzzle Brake", "COMPONENT_AT_MUZZLE_06", 0x347EF8AC, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Split-End Muzzle Brake", "COMPONENT_AT_MUZZLE_07", 0x4DB62ABE, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Barrel", "COMPONENT_AT_AR_BARREL_01", 0x43A49D26, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Heavy Barrel", "COMPONENT_AT_AR_BARREL_02", 0x5646C26A, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Digital Camo", "COMPONENT_ASSAULTRIFLE_MK2_CAMO", 0x911B24AF, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Brushstroke Camo", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_02", 0x37E5444B, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Woodland Camo", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_03", 0x538B7B97, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Skull", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_04", 0x25789F72, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Sessanta Nove", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_05", 0xC5495F2D, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Perseus", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_06", 0xCF8B73B1, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Leopard", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_07", 0xA9BB2811, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Zebra", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_08", 0xFC674D54, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Geometric", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_09", 0x7C7FCD9B, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Boom!", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_10", 0xA5C38392, false, "weapon_assaultrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Patriotic", "COMPONENT_ASSAULTRIFLE_MK2_CAMO_IND_01", 0xB9B15DB0, false, "weapon_assaultrifle_mk2"));
        Weapons.Add(new GTAWeapon("weapon_carbinerifle", 120, GTAWeapon.WeaponCategory.AR, 3, 2210333304, true,false,true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_CARBINERIFLE_CLIP_01", 0x9FBE33EC, false, "weapon_carbinerifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_CARBINERIFLE_CLIP_02", 0x91109691, false, "weapon_carbinerifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Box Magazine", "COMPONENT_CARBINERIFLE_CLIP_03", 0xBA62E935, false, "weapon_carbinerifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_carbinerifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_MEDIUM", 0xA0D89C42, false, "weapon_carbinerifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP", 0x837445AA, false, "weapon_carbinerifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_carbinerifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Yusuf Amir Luxury Finish", "COMPONENT_CARBINERIFLE_VARMOD_LUXE", 0xD89B9658, false, "weapon_carbinerifle"));
        Weapons.Add(new GTAWeapon("weapon_carbinerifle_mk2", 120, GTAWeapon.WeaponCategory.AR, 3, 0xFAD1F1C9, true,false,true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_CARBINERIFLE_MK2_CLIP_01", 0x4C7A391E, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_CARBINERIFLE_MK2_CLIP_02", 0x5DD5DBD5, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Tracer Rounds", "COMPONENT_CARBINERIFLE_MK2_CLIP_TRACER", 0x1757F566, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Incendiary Rounds", "COMPONENT_CARBINERIFLE_MK2_CLIP_INCENDIARY", 0x3D25C2A7, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Armor Piercing Rounds", "COMPONENT_CARBINERIFLE_MK2_CLIP_ARMORPIERCING", 0x255D5D57, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Full Metal Jacket Rounds", "COMPONENT_CARBINERIFLE_MK2_CLIP_FMJ", 0x44032F11, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP_02", 0x9D65907A, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS", 0x420FD713, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Small Scope", "COMPONENT_AT_SCOPE_MACRO_MK2", 0x49B2945, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Large Scope", "COMPONENT_AT_SCOPE_MEDIUM_MK2", 0xC66B6542, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP", 0x837445AA, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flat Muzzle Brake", "COMPONENT_AT_MUZZLE_01", 0xB99402D4, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Tactical Muzzle Brake", "COMPONENT_AT_MUZZLE_02", 0xC867A07B, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Fat-End Muzzle Brake", "COMPONENT_AT_MUZZLE_03", 0xDE11CBCF, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Precision Muzzle Brake", "COMPONENT_AT_MUZZLE_04", 0xEC9068CC, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Heavy Duty Muzzle Brake", "COMPONENT_AT_MUZZLE_05", 0x2E7957A, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Slanted Muzzle Brake", "COMPONENT_AT_MUZZLE_06", 0x347EF8AC, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Split-End Muzzle Brake", "COMPONENT_AT_MUZZLE_07", 0x4DB62ABE, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Barrel", "COMPONENT_AT_CR_BARREL_01", 0x833637FF, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Heavy Barrel", "COMPONENT_AT_CR_BARREL_02", 0x8B3C480B, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Digital Camo", "COMPONENT_CARBINERIFLE_MK2_CAMO", 0x4BDD6F16, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Brushstroke Camo", "COMPONENT_CARBINERIFLE_MK2_CAMO_02", 0x406A7908, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Woodland Camo", "COMPONENT_CARBINERIFLE_MK2_CAMO_03", 0x2F3856A4, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Skull", "COMPONENT_CARBINERIFLE_MK2_CAMO_04", 0xE50C424D, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Sessanta Nove", "COMPONENT_CARBINERIFLE_MK2_CAMO_05", 0xD37D1F2F, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Perseus", "COMPONENT_CARBINERIFLE_MK2_CAMO_06", 0x86268483, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Leopard", "COMPONENT_CARBINERIFLE_MK2_CAMO_07", 0xF420E076, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Zebra", "COMPONENT_CARBINERIFLE_MK2_CAMO_08", 0xAAE14DF8, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Geometric", "COMPONENT_CARBINERIFLE_MK2_CAMO_09", 0x9893A95D, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Boom!", "COMPONENT_CARBINERIFLE_MK2_CAMO_10", 0x6B13CD3E, false, "weapon_carbinerifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Patriotic", "COMPONENT_CARBINERIFLE_MK2_CAMO_IND_01", 0xDA55CD3F, false, "weapon_carbinerifle_mk2"));
        Weapons.Add(new GTAWeapon("weapon_advancedrifle", 120, GTAWeapon.WeaponCategory.AR, 3, 2937143193, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_ADVANCEDRIFLE_CLIP_01", 0xFA8FA10F, false, "weapon_advancedrifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_ADVANCEDRIFLE_CLIP_02", 0x8EC1C979, false, "weapon_advancedrifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_advancedrifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_SMALL", 0xAA2C45B4, false, "weapon_advancedrifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP", 0x837445AA, false, "weapon_advancedrifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Gilded Gun Metal Finish", "COMPONENT_ADVANCEDRIFLE_VARMOD_LUXE", 0x377CD377, false, "weapon_advancedrifle"));
        Weapons.Add(new GTAWeapon("weapon_specialcarbine", 120, GTAWeapon.WeaponCategory.AR, 3, 3231910285, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_SPECIALCARBINE_CLIP_01", 0xC6C7E581, false, "weapon_specialcarbine"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_SPECIALCARBINE_CLIP_02", 0x7C8BD10E, false, "weapon_specialcarbine"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Drum Magazine", "COMPONENT_SPECIALCARBINE_CLIP_03", 0x6B59AEAA, false, "weapon_specialcarbine"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_specialcarbine"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_MEDIUM", 0xA0D89C42, false, "weapon_specialcarbine"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP_02", 0xA73D4664, false, "weapon_specialcarbine"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_specialcarbine"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Etched Gun Metal Finish", "COMPONENT_SPECIALCARBINE_VARMOD_LOWRIDER", 0x730154F2, false, "weapon_specialcarbine"));
        Weapons.Add(new GTAWeapon("weapon_specialcarbine_mk2", 120, GTAWeapon.WeaponCategory.AR, 3, 0x969C3D67, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_SPECIALCARBINE_MK2_CLIP_01", 0x16C69281, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_SPECIALCARBINE_MK2_CLIP_02", 0xDE1FA12C, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Tracer Rounds", "COMPONENT_SPECIALCARBINE_MK2_CLIP_TRACER", 0x8765C68A, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Incendiary Rounds", "COMPONENT_SPECIALCARBINE_MK2_CLIP_INCENDIARY", 0xDE011286, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Armor Piercing Rounds", "COMPONENT_SPECIALCARBINE_MK2_CLIP_ARMORPIERCING", 0x51351635, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Full Metal Jacket Rounds", "COMPONENT_SPECIALCARBINE_MK2_CLIP_FMJ", 0x503DEA90, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS", 0x420FD713, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Small Scope", "COMPONENT_AT_SCOPE_MACRO_MK2", 0x49B2945, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Large Scope", "COMPONENT_AT_SCOPE_MEDIUM_MK2", 0xC66B6542, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP_02", 0xA73D4664, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flat Muzzle Brake", "COMPONENT_AT_MUZZLE_01", 0xB99402D4, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Tactical Muzzle Brake", "COMPONENT_AT_MUZZLE_02", 0xC867A07B, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Fat-End Muzzle Brake", "COMPONENT_AT_MUZZLE_03", 0xDE11CBCF, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Precision Muzzle Brake", "COMPONENT_AT_MUZZLE_04", 0xEC9068CC, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Heavy Duty Muzzle Brake", "COMPONENT_AT_MUZZLE_05", 0x2E7957A, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Slanted Muzzle Brake", "COMPONENT_AT_MUZZLE_06", 0x347EF8AC, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Split-End Muzzle Brake", "COMPONENT_AT_MUZZLE_07", 0x4DB62ABE, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP_02", 0x9D65907A, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Barrel", "COMPONENT_AT_SC_BARREL_01", 0xE73653A9, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Heavy Barrel", "COMPONENT_AT_SC_BARREL_02", 0xF97F783B, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Digital Camo", "COMPONENT_SPECIALCARBINE_MK2_CAMO", 0xD40BB53B, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Brushstroke Camo", "COMPONENT_SPECIALCARBINE_MK2_CAMO_02", 0x431B238B, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Woodland Camo", "COMPONENT_SPECIALCARBINE_MK2_CAMO_03", 0x34CF86F4, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Skull", "COMPONENT_SPECIALCARBINE_MK2_CAMO_04", 0xB4C306DD, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Sessanta Nove", "COMPONENT_SPECIALCARBINE_MK2_CAMO_05", 0xEE677A25, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Perseus", "COMPONENT_SPECIALCARBINE_MK2_CAMO_06", 0xDF90DC78, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Leopard", "COMPONENT_SPECIALCARBINE_MK2_CAMO_07", 0xA4C31EE, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Zebra", "COMPONENT_SPECIALCARBINE_MK2_CAMO_08", 0x89CFB0F7, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Geometric", "COMPONENT_SPECIALCARBINE_MK2_CAMO_09", 0x7B82145C, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Boom!", "COMPONENT_SPECIALCARBINE_MK2_CAMO_10", 0x899CAF75, false, "weapon_specialcarbine_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Patriotic", "COMPONENT_SPECIALCARBINE_MK2_CAMO_IND_01", 0x5218C819, false, "weapon_specialcarbine_mk2"));
        Weapons.Add(new GTAWeapon("weapon_bullpuprifle", 120, GTAWeapon.WeaponCategory.AR, 3, 2132975508, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_BULLPUPRIFLE_CLIP_01", 0xC5A12F80, false, "weapon_bullpuprifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_BULLPUPRIFLE_CLIP_02", 0xB3688B0F, false, "weapon_bullpuprifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_bullpuprifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_SMALL", 0xAA2C45B4, false, "weapon_bullpuprifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP", 0x837445AA, false, "weapon_bullpuprifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_bullpuprifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Gilded Gun Metal Finish", "COMPONENT_BULLPUPRIFLE_VARMOD_LOW", 0xA857BC78, false, "weapon_bullpuprifle"));
        Weapons.Add(new GTAWeapon("weapon_bullpuprifle_mk2", 120, GTAWeapon.WeaponCategory.AR, 3, 0x84D6FAFD, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_BULLPUPRIFLE_MK2_CLIP_01", 0x18929DA, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_BULLPUPRIFLE_MK2_CLIP_02", 0xEFB00628, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Tracer Rounds", "COMPONENT_BULLPUPRIFLE_MK2_CLIP_TRACER", 0x822060A9, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Incendiary Rounds", "COMPONENT_BULLPUPRIFLE_MK2_CLIP_INCENDIARY", 0xA99CF95A, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Armor Piercing Rounds", "COMPONENT_BULLPUPRIFLE_MK2_CLIP_ARMORPIERCING", 0xFAA7F5ED, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Full Metal Jacket Rounds", "COMPONENT_BULLPUPRIFLE_MK2_CLIP_FMJ", 0x43621710, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS", 0x420FD713, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Small Scope", "COMPONENT_AT_SCOPE_MACRO_02_MK2", 0xC7ADD105, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Medium Scope", "COMPONENT_AT_SCOPE_SMALL_MK2", 0x3F3C8181, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Barrel", "COMPONENT_AT_BP_BARREL_01", 0x659AC11B, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Heavy Barrel", "COMPONENT_AT_BP_BARREL_02", 0x3BF26DC7, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP", 0x837445AA, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flat Muzzle Brake", "COMPONENT_AT_MUZZLE_01", 0xB99402D4, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Tactical Muzzle Brake", "COMPONENT_AT_MUZZLE_02", 0xC867A07B, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Fat-End Muzzle Brake", "COMPONENT_AT_MUZZLE_03", 0xDE11CBCF, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Precision Muzzle Brake", "COMPONENT_AT_MUZZLE_04", 0xEC9068CC, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Heavy Duty Muzzle Brake", "COMPONENT_AT_MUZZLE_05", 0x2E7957A, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Slanted Muzzle Brake", "COMPONENT_AT_MUZZLE_06", 0x347EF8AC, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Split-End Muzzle Brake", "COMPONENT_AT_MUZZLE_07", 0x4DB62ABE, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP_02", 0x9D65907A, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Digital Camo", "COMPONENT_BULLPUPRIFLE_MK2_CAMO", 0xAE4055B7, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Brushstroke Camo", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_02", 0xB905ED6B, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Woodland Camo", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_03", 0xA6C448E8, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Skull", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_04", 0x9486246C, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Sessanta Nove", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_05", 0x8A390FD2, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Perseus", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_06", 0x2337FC5, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Leopard", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_07", 0xEFFFDB5E, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Zebra", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_08", 0xDDBDB6DA, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Geometric", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_09", 0xCB631225, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Boom!", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_10", 0xA87D541E, false, "weapon_bullpuprifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Patriotic", "COMPONENT_BULLPUPRIFLE_MK2_CAMO_IND_01", 0xC5E9AE52, false, "weapon_bullpuprifle_mk2"));
        Weapons.Add(new GTAWeapon("weapon_compactrifle", 120, GTAWeapon.WeaponCategory.AR, 3, 1649403952, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_COMPACTRIFLE_CLIP_01", 0x513F0A63, false, "weapon_compactrifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_COMPACTRIFLE_CLIP_02", 0x59FF9BF8, false, "weapon_compactrifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Drum Magazine", "COMPONENT_COMPACTRIFLE_CLIP_03", 0xC607740E, false, "weapon_compactrifle"));
        //LMG
        Weapons.Add(new GTAWeapon("weapon_mg", 200, GTAWeapon.WeaponCategory.LMG, 4, 2634544996, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_MG_CLIP_01", 0xF434EF84, false, "weapon_mg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_MG_CLIP_02", 0x82158B47, false, "weapon_mg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_SMALL_02", 0x3C00AFED, false, "weapon_mg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Yusuf Amir Luxury Finish", "COMPONENT_MG_VARMOD_LOWRIDER", 0xD6DABABE, false, "weapon_mg"));
        Weapons.Add(new GTAWeapon("weapon_combatmg", 200, GTAWeapon.WeaponCategory.LMG, 4, 2144741730, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_COMBATMG_CLIP_01", 0xE1FFB34A, false, "weapon_combatmg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_COMBATMG_CLIP_02", 0xD6C59CD6, false, "weapon_combatmg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_MEDIUM", 0xA0D89C42, false, "weapon_combatmg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_combatmg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Etched Gun Metal Finish", "COMPONENT_COMBATMG_VARMOD_LOWRIDER", 0x92FECCDD, false, "weapon_combatmg"));
        Weapons.Add(new GTAWeapon("weapon_combatmg_mk2", 200, GTAWeapon.WeaponCategory.LMG, 4, 0xDBBD7280, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_COMBATMG_MK2_CLIP_01", 0x492B257C, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_COMBATMG_MK2_CLIP_02", 0x17DF42E9, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Tracer Rounds", "COMPONENT_COMBATMG_MK2_CLIP_TRACER", 0xF6649745, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Incendiary Rounds", "COMPONENT_COMBATMG_MK2_CLIP_INCENDIARY", 0xC326BDBA, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Armor Piercing Rounds", "COMPONENT_COMBATMG_MK2_CLIP_ARMORPIERCING", 0x29882423, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Full Metal Jacket Rounds", "COMPONENT_COMBATMG_MK2_CLIP_FMJ", 0x57EF1CC8, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP_02", 0x9D65907A, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS", 0x420FD713, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Medium Scope", "COMPONENT_AT_SCOPE_SMALL_MK2", 0x3F3C8181, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Large Scope", "COMPONENT_AT_SCOPE_MEDIUM_MK2", 0xC66B6542, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flat Muzzle Brake", "COMPONENT_AT_MUZZLE_01", 0xB99402D4, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Tactical Muzzle Brake", "COMPONENT_AT_MUZZLE_02", 0xC867A07B, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Fat-End Muzzle Brake", "COMPONENT_AT_MUZZLE_03", 0xDE11CBCF, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Precision Muzzle Brake", "COMPONENT_AT_MUZZLE_04", 0xEC9068CC, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Heavy Duty Muzzle Brake", "COMPONENT_AT_MUZZLE_05", 0x2E7957A, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Slanted Muzzle Brake", "COMPONENT_AT_MUZZLE_06", 0x347EF8AC, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Split-End Muzzle Brake", "COMPONENT_AT_MUZZLE_07", 0x4DB62ABE, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Barrel", "COMPONENT_AT_MG_BARREL_01", 0xC34EF234, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Heavy Barrel", "COMPONENT_AT_MG_BARREL_02", 0xB5E2575B, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Digital Camo", "COMPONENT_COMBATMG_MK2_CAMO", 0x4A768CB5, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Brushstroke Camo", "COMPONENT_COMBATMG_MK2_CAMO_02", 0xCCE06BBD, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Woodland Camo", "COMPONENT_COMBATMG_MK2_CAMO_03", 0xBE94CF26, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Skull", "COMPONENT_COMBATMG_MK2_CAMO_04", 0x7609BE11, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Sessanta Nove", "COMPONENT_COMBATMG_MK2_CAMO_05", 0x48AF6351, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Perseus", "COMPONENT_COMBATMG_MK2_CAMO_06", 0x9186750A, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Leopard", "COMPONENT_COMBATMG_MK2_CAMO_07", 0x84555AA8, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Zebra", "COMPONENT_COMBATMG_MK2_CAMO_08", 0x1B4C088B, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Geometric", "COMPONENT_COMBATMG_MK2_CAMO_09", 0xE046DFC, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Boom!", "COMPONENT_COMBATMG_MK2_CAMO_10", 0x28B536E, false, "weapon_combatmg_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Patriotic", "COMPONENT_COMBATMG_MK2_CAMO_IND_01", 0xD703C94D, false, "weapon_combatmg_mk2"));
        Weapons.Add(new GTAWeapon("weapon_gusenberg", 200, GTAWeapon.WeaponCategory.LMG, 4, 1627465347, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_GUSENBERG_CLIP_01", 0x1CE5A6A5, false, "weapon_gusenberg"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_GUSENBERG_CLIP_02", 0xEAC8C270, false, "weapon_gusenberg"));
        //Sniper
        Weapons.Add(new GTAWeapon("weapon_sniperrifle", 40, GTAWeapon.WeaponCategory.Sniper, 4, 100416529, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_SNIPERRIFLE_CLIP_01", 0x9BC64089, false, "weapon_sniperrifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP_02", 0xA73D4664, false, "weapon_sniperrifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_LARGE", 0xD2443DDC, false, "weapon_sniperrifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Advanced Scope", "COMPONENT_AT_SCOPE_MAX", 0xBC54DA77, false, "weapon_sniperrifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Etched Wood Grip Finish", "COMPONENT_SNIPERRIFLE_VARMOD_LUXE", 0x4032B5E7, false, "weapon_sniperrifle"));
        Weapons.Add(new GTAWeapon("weapon_heavysniper", 40, GTAWeapon.WeaponCategory.Sniper, 4, 205991906, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_HEAVYSNIPER_CLIP_01", 0x476F52F4, false, "weapon_heavysniper"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_LARGE", 0xD2443DDC, false, "weapon_heavysniper"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Advanced Scope", "COMPONENT_AT_SCOPE_MAX", 0xBC54DA77, false, "weapon_heavysniper"));
        Weapons.Add(new GTAWeapon("weapon_heavysniper_mk2", 40, GTAWeapon.WeaponCategory.Sniper, 4, 0xA914799, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_HEAVYSNIPER_MK2_CLIP_01", 0xFA1E1A28, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_HEAVYSNIPER_MK2_CLIP_02", 0x2CD8FF9D, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Incendiary Rounds", "COMPONENT_HEAVYSNIPER_MK2_CLIP_INCENDIARY", 0xEC0F617, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Armor Piercing Rounds", "COMPONENT_HEAVYSNIPER_MK2_CLIP_ARMORPIERCING", 0xF835D6D4, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Full Metal Jacket Rounds", "COMPONENT_HEAVYSNIPER_MK2_CLIP_FMJ", 0x3BE948F6, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Explosive Rounds", "COMPONENT_HEAVYSNIPER_MK2_CLIP_EXPLOSIVE", 0x89EBDAA7, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Zoom Scope", "COMPONENT_AT_SCOPE_LARGE_MK2", 0x82C10383, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Advanced Scope", "COMPONENT_AT_SCOPE_MAX", 0xBC54DA77, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Night Vision Scope", "COMPONENT_AT_SCOPE_NV", 0xB68010B0, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Thermal Scope", "COMPONENT_AT_SCOPE_THERMAL", 0x2E43DA41, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_SR_SUPP_03", 0xAC42DF71, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Squared Muzzle Brake", "COMPONENT_AT_MUZZLE_08", 0x5F7DCE4D, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Bell-End Muzzle Brake", "COMPONENT_AT_MUZZLE_09", 0x6927E1A1, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Barrel", "COMPONENT_AT_SR_BARREL_01", 0x909630B7, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Heavy Barrel", "COMPONENT_AT_SR_BARREL_02", 0x108AB09E, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Digital Camo", "COMPONENT_HEAVYSNIPER_MK2_CAMO", 0xF8337D02, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Brushstroke Camo", "COMPONENT_HEAVYSNIPER_MK2_CAMO_02", 0xC5BEDD65, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Woodland Camo", "COMPONENT_HEAVYSNIPER_MK2_CAMO_03", 0xE9712475, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Skull", "COMPONENT_HEAVYSNIPER_MK2_CAMO_04", 0x13AA78E7, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Sessanta Nove", "COMPONENT_HEAVYSNIPER_MK2_CAMO_05", 0x26591E50, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Perseus", "COMPONENT_HEAVYSNIPER_MK2_CAMO_06", 0x302731EC, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Leopard", "COMPONENT_HEAVYSNIPER_MK2_CAMO_07", 0xAC722A78, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Zebra", "COMPONENT_HEAVYSNIPER_MK2_CAMO_08", 0xBEA4CEDD, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Geometric", "COMPONENT_HEAVYSNIPER_MK2_CAMO_09", 0xCD776C82, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Boom!", "COMPONENT_HEAVYSNIPER_MK2_CAMO_10", 0xABC5ACC7, false, "weapon_heavysniper_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Patriotic", "COMPONENT_HEAVYSNIPER_MK2_CAMO_IND_01", 0x6C32D2EB, false, "weapon_heavysniper_mk2"));
        Weapons.Add(new GTAWeapon("weapon_marksmanrifle", 40, GTAWeapon.WeaponCategory.Sniper, 4, 3342088282, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_MARKSMANRIFLE_CLIP_01", 0xD83B4141, false, "weapon_marksmanrifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_MARKSMANRIFLE_CLIP_02", 0xCCFD2AC5, false, "weapon_marksmanrifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_LARGE_FIXED_ZOOM", 0x1C221B1A, false, "weapon_marksmanrifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_marksmanrifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP", 0x837445AA, false, "weapon_marksmanrifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_marksmanrifle"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Yusuf Amir Luxury Finish", "COMPONENT_MARKSMANRIFLE_VARMOD_LUXE", 0x161E9241, false, "weapon_marksmanrifle"));
        Weapons.Add(new GTAWeapon("weapon_marksmanrifle_mk2", 40, GTAWeapon.WeaponCategory.Sniper, 4, 0x6A6C02E0, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_MARKSMANRIFLE_MK2_CLIP_01", 0x94E12DCE, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_MARKSMANRIFLE_MK2_CLIP_02", 0xE6CFD1AA, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Tracer Rounds", "COMPONENT_MARKSMANRIFLE_MK2_CLIP_TRACER", 0xD77A22D2, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Incendiary Rounds", "COMPONENT_MARKSMANRIFLE_MK2_CLIP_INCENDIARY", 0x6DD7A86E, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Armor Piercing Rounds", "COMPONENT_MARKSMANRIFLE_MK2_CLIP_ARMORPIERCING", 0xF46FD079, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Full Metal Jacket Rounds", "COMPONENT_MARKSMANRIFLE_MK2_CLIP_FMJ", 0xE14A9ED3, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS", 0x420FD713, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Large Scope", "COMPONENT_AT_SCOPE_MEDIUM_MK2", 0xC66B6542, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Zoom Scope", "COMPONENT_AT_SCOPE_LARGE_FIXED_ZOOM_MK2", 0x5B1C713C, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP", 0x837445AA, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flat Muzzle Brake", "COMPONENT_AT_MUZZLE_01", 0xB99402D4, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Tactical Muzzle Brake", "COMPONENT_AT_MUZZLE_02", 0xC867A07B, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Fat-End Muzzle Brake", "COMPONENT_AT_MUZZLE_03", 0xDE11CBCF, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Precision Muzzle Brake", "COMPONENT_AT_MUZZLE_04", 0xEC9068CC, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Heavy Duty Muzzle Brake", "COMPONENT_AT_MUZZLE_05", 0x2E7957A, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Slanted Muzzle Brake", "COMPONENT_AT_MUZZLE_06", 0x347EF8AC, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Split-End Muzzle Brake", "COMPONENT_AT_MUZZLE_07", 0x4DB62ABE, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Barrel", "COMPONENT_AT_MRFL_BARREL_01", 0x381B5D89, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Heavy Barrel", "COMPONENT_AT_MRFL_BARREL_02", 0x68373DDC, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP_02", 0x9D65907A, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Digital Camo", "COMPONENT_MARKSMANRIFLE_MK2_CAMO", 0x9094FBA0, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Brushstroke Camo", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_02", 0x7320F4B2, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Woodland Camo", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_03", 0x60CF500F, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Skull", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_04", 0xFE668B3F, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Sessanta Nove", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_05", 0xF3757559, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Perseus", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_06", 0x193B40E8, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Leopard", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_07", 0x107D2F6C, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Zebra", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_08", 0xC4E91841, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Geometric", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_09", 0x9BB1C5D3, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Boom!", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_10", 0x3B61040B, false, "weapon_marksmanrifle_mk2"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Boom!", "COMPONENT_MARKSMANRIFLE_MK2_CAMO_IND_01", 0xB7A316DA, false, "weapon_marksmanrifle_mk2"));
        //Heavy
        Weapons.Add(new GTAWeapon("weapon_rpg", 3, GTAWeapon.WeaponCategory.Heavy, 4, 2982836145, false, false, true));
        Weapons.Add(new GTAWeapon("weapon_grenadelauncher", 32, GTAWeapon.WeaponCategory.Heavy, 4, 2726580491, false, false, true));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Default Clip", "COMPONENT_GRENADELAUNCHER_CLIP_01", 0x11AE5C97, false, "weapon_grenadelauncher"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_grenadelauncher"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_grenadelauncher"));
        WeaponComponentsLookup.Add(new WeaponVariation.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_SMALL", 0xAA2C45B4, false, "weapon_grenadelauncher"));
        Weapons.Add(new GTAWeapon("weapon_grenadelauncher_smoke", 32, GTAWeapon.WeaponCategory.Heavy, 4, 1305664598, false, false, true));
        Weapons.Add(new GTAWeapon("weapon_minigun", 500, GTAWeapon.WeaponCategory.Heavy, 4, 1119849093, false, false, true));
        Weapons.Add(new GTAWeapon("weapon_firework", 20, GTAWeapon.WeaponCategory.Heavy, 4, 0x7F7497E5, false, false, true));
        Weapons.Add(new GTAWeapon("weapon_railgun", 50, GTAWeapon.WeaponCategory.Heavy, 4, 0x6D544C99, false, false, true));
        Weapons.Add(new GTAWeapon("weapon_hominglauncher", 3, GTAWeapon.WeaponCategory.Heavy, 4, 0x63AB0442, false, false, true));
        Weapons.Add(new GTAWeapon("weapon_compactlauncher", 10, GTAWeapon.WeaponCategory.Heavy, 4, 125959754, false, false, true));
        Weapons.Add(new GTAWeapon("weapon_rayminigun", 50, GTAWeapon.WeaponCategory.Heavy, 4, 0xB62D1F67, false, false, true));

        foreach(GTAWeapon Weapon in Weapons.Where(x => x.Category == GTAWeapon.WeaponCategory.Pistol))
        {
            if (Weapon.Name == "weapon_marksmanpistol" || Weapon.Name == "weapon_stungun" || Weapon.Name == "weapon_flaregun" || Weapon.Name == "weapon_raypistol")
                Weapon.CanPistolSuicide = false;
            else
                Weapon.CanPistolSuicide = true;

            if(Weapon.Name == "weapon_pistol")
            {
                WeaponVariation Police1 = new WeaponVariation(0);
                Weapon.PoliceVariations.Add(Police1);

                WeaponVariation Police2 = new WeaponVariation(0);
                Police2.Components.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, true, "weapon_pistol"));
                Weapon.PoliceVariations.Add(Police2);

                WeaponVariation Police3 = new WeaponVariation(0);
                Police3.Components.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_PISTOL_CLIP_02", 0xED265A1C, true, "weapon_pistol"));
                Weapon.PoliceVariations.Add(Police3);

                WeaponVariation Police4 = new WeaponVariation(0);
                Police4.Components.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, true, "weapon_pistol"));
                Police4.Components.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_PISTOL_CLIP_02", 0xED265A1C, true, "weapon_pistol"));
                Weapon.PoliceVariations.Add(Police4);
            }
            if (Weapon.Name == "weapon_pistol_mk2")
            {
                WeaponVariation Police1 = new WeaponVariation(0);
                Weapon.PoliceVariations.Add(Police1);

                WeaponVariation Police2 = new WeaponVariation(0);
                Police2.Components.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH_02", 0x43FD595B, true, "weapon_pistol_mk2"));
                Weapon.PoliceVariations.Add(Police2);

                WeaponVariation Police3 = new WeaponVariation(0);
                Police3.Components.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_PISTOL_MK2_CLIP_02", 0x5ED6C128, true, "weapon_pistol_mk2"));
                Weapon.PoliceVariations.Add(Police3);

                WeaponVariation Police4 = new WeaponVariation(0);
                Police4.Components.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH_02", 0x43FD595B, true, "weapon_pistol_mk2"));
                Police4.Components.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_PISTOL_MK2_CLIP_02", 0x5ED6C128, true, "weapon_pistol_mk2"));
                Weapon.PoliceVariations.Add(Police4);
            }
            if (Weapon.Name == "weapon_combatpistol")
            {
                WeaponVariation Police1 = new WeaponVariation(0);
                Weapon.PoliceVariations.Add(Police1);

                WeaponVariation Police2 = new WeaponVariation(0);
                Police2.Components.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, true, "weapon_combatpistol"));
                Weapon.PoliceVariations.Add(Police2);

                WeaponVariation Police3 = new WeaponVariation(0);
                Police3.Components.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_COMBATPISTOL_CLIP_02", 0xD67B4F2D, true, "weapon_combatpistol"));
                Weapon.PoliceVariations.Add(Police3);

                WeaponVariation Police4 = new WeaponVariation(0);
                Police4.Components.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, true, "weapon_combatpistol"));
                Police4.Components.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_COMBATPISTOL_CLIP_02", 0xD67B4F2D, true, "weapon_combatpistol"));
                Weapon.PoliceVariations.Add(Police4);
            }
            if (Weapon.Name == "weapon_heavypistol")
            {
                WeaponVariation Police1 = new WeaponVariation(0);
                Weapon.PoliceVariations.Add(Police1);

                WeaponVariation Police2 = new WeaponVariation(0);
                Police2.Components.Add(new WeaponVariation.WeaponComponent("Etched Wood Grip Finish", "COMPONENT_HEAVYPISTOL_VARMOD_LUXE", 0x7A6A7B7B, true, "weapon_heavypistol"));
                Weapon.PoliceVariations.Add(Police2);

                WeaponVariation Police3 = new WeaponVariation(0);
                Police3.Components.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, true, "weapon_heavypistol"));
                Police3.Components.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_HEAVYPISTOL_CLIP_02", 0x64F9C62B, true, "weapon_heavypistol"));
                Weapon.PoliceVariations.Add(Police3);

                WeaponVariation Police4 = new WeaponVariation(0);
                Police4.Components.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, true, "weapon_heavypistol"));
                Police4.Components.Add(new WeaponVariation.WeaponComponent("Etched Wood Grip Finish", "COMPONENT_HEAVYPISTOL_VARMOD_LUXE", 0x7A6A7B7B, true, "weapon_heavypistol"));
                Weapon.PoliceVariations.Add(Police4);
            }



        }
        foreach (GTAWeapon Weapon in Weapons.Where(x => x.Category == GTAWeapon.WeaponCategory.AR))
        {
            if (Weapon.Name == "weapon_carbinerifle")
            {
                WeaponVariation Police1 = new WeaponVariation(0);
                Weapon.PoliceVariations.Add(Police1);

                WeaponVariation Police2 = new WeaponVariation(0);
                Police2.Components.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, true, "weapon_carbinerifle"));
                Police2.Components.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, true, "weapon_carbinerifle"));
                Weapon.PoliceVariations.Add(Police2);

                WeaponVariation Police3 = new WeaponVariation(0);
                Police3.Components.Add(new WeaponVariation.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_MEDIUM", 0xA0D89C42, true, "weapon_carbinerifle"));
                Police3.Components.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, true, "weapon_carbinerifle"));
                Police3.Components.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_CARBINERIFLE_CLIP_02", 0x91109691, true, "weapon_carbinerifle"));
                Weapon.PoliceVariations.Add(Police3);

                WeaponVariation Police4 = new WeaponVariation(0);
                Police4.Components.Add(new WeaponVariation.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_MEDIUM", 0xA0D89C42, true, "weapon_carbinerifle"));
                Police4.Components.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, true, "weapon_carbinerifle"));
                Police4.Components.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, true, "weapon_carbinerifle"));
                Weapon.PoliceVariations.Add(Police4);
            }
            if (Weapon.Name == "weapon_carbinerifle_mk2")
            {
                WeaponVariation Police1 = new WeaponVariation(0);
                Weapon.PoliceVariations.Add(Police1);

                WeaponVariation Police2 = new WeaponVariation(0);
                Police2.Components.Add(new WeaponVariation.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS", 0x420FD713, true, "weapon_carbinerifle_mk2"));
                Police2.Components.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP_02", 0x9D65907A, true, "weapon_carbinerifle_mk2"));
                Police2.Components.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, true, "weapon_carbinerifle_mk2"));
                Weapon.PoliceVariations.Add(Police2);

                WeaponVariation Police3 = new WeaponVariation(0);
                Police3.Components.Add(new WeaponVariation.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS", 0x420FD713, true, "weapon_carbinerifle_mk2"));
                Police3.Components.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP_02", 0x9D65907A, true, "weapon_carbinerifle_mk2"));
                Police3.Components.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_CARBINERIFLE_MK2_CLIP_02", 0x5DD5DBD5, true, "weapon_carbinerifle_mk2"));
                Weapon.PoliceVariations.Add(Police3);

                WeaponVariation Police4 = new WeaponVariation(0);
                Police4.Components.Add(new WeaponVariation.WeaponComponent("Large Scope", "COMPONENT_AT_SCOPE_MEDIUM_MK2", 0xC66B6542, true, "weapon_carbinerifle_mk2"));
                Police4.Components.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP_02", 0x9D65907A, true, "weapon_carbinerifle_mk2"));
                Police4.Components.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, true, "weapon_carbinerifle_mk2"));
                Weapon.PoliceVariations.Add(Police4);
            }
        }
        foreach (GTAWeapon Weapon in Weapons.Where(x => x.Category == GTAWeapon.WeaponCategory.SMG))
        {
            if (Weapon.Name == "weapon_combatpdw")
            {
                WeaponVariation Police1 = new WeaponVariation(0);
                Weapon.PoliceVariations.Add(Police1);

                WeaponVariation Police2 = new WeaponVariation(0);
                Police2.Components.Add(new WeaponVariation.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_SMALL", 0xAA2C45B4, false, "weapon_combatpdw"));
                Police2.Components.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_combatpdw"));
                Police2.Components.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_combatpdw"));
                Weapon.PoliceVariations.Add(Police2);

                WeaponVariation Police3 = new WeaponVariation(0);
                Police3.Components.Add(new WeaponVariation.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_SMALL", 0xAA2C45B4, false, "weapon_combatpdw"));
                Police3.Components.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_combatpdw"));
                Police3.Components.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_COMBATPDW_CLIP_02", 0x334A5203, false, "weapon_combatpdw"));
                Weapon.PoliceVariations.Add(Police3);

                WeaponVariation Police4 = new WeaponVariation(0);
                Police4.Components.Add(new WeaponVariation.WeaponComponent("Scope", "COMPONENT_AT_SCOPE_SMALL", 0xAA2C45B4, false, "weapon_combatpdw"));
                Police4.Components.Add(new WeaponVariation.WeaponComponent("Grip", "COMPONENT_AT_AR_AFGRIP", 0xC164F53, false, "weapon_combatpdw"));
                Police4.Components.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_combatpdw"));
                Weapon.PoliceVariations.Add(Police4);
            }
            if (Weapon.Name == "weapon_microsmg")
            {
                WeaponVariation Player1 = new WeaponVariation(0);
                Weapon.PlayerVariations.Add(Player1);

                WeaponVariation Player2 = new WeaponVariation(0);
                Player2.Components.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_MICROSMG_CLIP_02", 0x10E6BA2B, false, "weapon_microsmg"));
                Player2.Components.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_PI_FLSH", 0x359B7AAE, false, "weapon_microsmg"));
                Weapon.PlayerVariations.Add(Player2);

                WeaponVariation Player3 = new WeaponVariation(0);
                Player3.Components.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_MICROSMG_CLIP_02", 0x10E6BA2B, false, "weapon_microsmg"));
                Weapon.PlayerVariations.Add(Player3);

                WeaponVariation Player4 = new WeaponVariation(0);
                Player4.Components.Add(new WeaponVariation.WeaponComponent("Extended Clip", "COMPONENT_MICROSMG_CLIP_02", 0x10E6BA2B, false, "weapon_microsmg"));
                Player4.Components.Add(new WeaponVariation.WeaponComponent("Suppressor", "COMPONENT_AT_AR_SUPP_02", 0xA73D4664, false, "weapon_microsmg"));
                Weapon.PlayerVariations.Add(Player4);
            }

        }

        foreach (GTAWeapon Weapon in Weapons.Where(x => x.Category == GTAWeapon.WeaponCategory.Shotgun))
        {
            if (Weapon.Name == "weapon_pumpshotgun")
            {
                WeaponVariation Police1 = new WeaponVariation(0);
                Weapon.PoliceVariations.Add(Police1);

                WeaponVariation Police2 = new WeaponVariation(0);
                Police2.Components.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_pumpshotgun"));
                Weapon.PoliceVariations.Add(Police2);
            }
            if (Weapon.Name == "weapon_pumpshotgun_mk2")
            {
                WeaponVariation Police1 = new WeaponVariation(0);
                Weapon.PoliceVariations.Add(Police1);

                WeaponVariation Police2 = new WeaponVariation(0);
                Police2.Components.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_pumpshotgun_mk2"));
                Weapon.PoliceVariations.Add(Police2);

                WeaponVariation Police3 = new WeaponVariation(0);
                Police3.Components.Add(new WeaponVariation.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS", 0x420FD713, false, "weapon_pumpshotgun_mk2"));
                Weapon.PoliceVariations.Add(Police3);

                WeaponVariation Police4 = new WeaponVariation(0);
                Police4.Components.Add(new WeaponVariation.WeaponComponent("Holographic Sight", "COMPONENT_AT_SIGHTS", 0x420FD713, false, "weapon_pumpshotgun_mk2"));
                Police4.Components.Add(new WeaponVariation.WeaponComponent("Flashlight", "COMPONENT_AT_AR_FLSH", 0x7BC4CDDC, false, "weapon_pumpshotgun_mk2"));
                Weapon.PoliceVariations.Add(Police4);
            }
        }
    }
    private static void setupStreets()
    {
        Streets.Add(new GTAStreet("Joshua Rd", 50f));
        Streets.Add(new GTAStreet("East Joshua Road", 50f));
        Streets.Add(new GTAStreet("Marina Dr", 35f));
        Streets.Add(new GTAStreet("Alhambra Dr", 35f));
        Streets.Add(new GTAStreet("Niland Ave", 35f));
        Streets.Add(new GTAStreet("Zancudo Ave", 35f));
        Streets.Add(new GTAStreet("Armadillo Ave", 35f));
        Streets.Add(new GTAStreet("Algonquin Blvd", 35f));
        Streets.Add(new GTAStreet("Mountain View Dr", 35f));
        Streets.Add(new GTAStreet("Cholla Springs Ave", 35f));
        Streets.Add(new GTAStreet("Panorama Dr", 40f));
        Streets.Add(new GTAStreet("Lesbos Ln", 35f));
        Streets.Add(new GTAStreet("Calafia Rd", 30f));
        Streets.Add(new GTAStreet("North Calafia Way", 30f));
        Streets.Add(new GTAStreet("Cassidy Trail", 25f));
        Streets.Add(new GTAStreet("Seaview Rd", 35f));
        Streets.Add(new GTAStreet("Grapeseed Main St", 35f));
        Streets.Add(new GTAStreet("Grapeseed Ave", 35f));
        Streets.Add(new GTAStreet("Joad Ln", 35f));
        Streets.Add(new GTAStreet("Union Rd", 40f));
        Streets.Add(new GTAStreet("O'Neil Way", 25f));
        Streets.Add(new GTAStreet("Senora Fwy", 65f));
        Streets.Add(new GTAStreet("Catfish View", 35f));
        Streets.Add(new GTAStreet("Great Ocean Hwy", 60f));
        Streets.Add(new GTAStreet("Paleto Blvd", 35f));
        Streets.Add(new GTAStreet("Duluoz Ave", 35f));
        Streets.Add(new GTAStreet("Procopio Dr", 35f));
        Streets.Add(new GTAStreet("Cascabel Ave", 30f));
        Streets.Add(new GTAStreet("Procopio Promenade", 25f));
        Streets.Add(new GTAStreet("Pyrite Ave", 30f));
        Streets.Add(new GTAStreet("Fort Zancudo Approach Rd", 25f));
        Streets.Add(new GTAStreet("Barbareno Rd", 30f));
        Streets.Add(new GTAStreet("Ineseno Road", 30f));
        Streets.Add(new GTAStreet("West Eclipse Blvd", 35f));
        Streets.Add(new GTAStreet("Playa Vista", 30f));
        Streets.Add(new GTAStreet("Bay City Ave", 30f));
        Streets.Add(new GTAStreet("Del Perro Fwy", 65f));
        Streets.Add(new GTAStreet("Equality Way", 30f));
        Streets.Add(new GTAStreet("Red Desert Ave", 30f));
        Streets.Add(new GTAStreet("Magellan Ave", 25f));
        Streets.Add(new GTAStreet("Sandcastle Way", 30f));
        Streets.Add(new GTAStreet("Vespucci Blvd", 40f));
        Streets.Add(new GTAStreet("Prosperity St", 30f));
        Streets.Add(new GTAStreet("San Andreas Ave", 40f));
        Streets.Add(new GTAStreet("North Rockford Dr", 35f));
        Streets.Add(new GTAStreet("South Rockford Dr", 35f));
        Streets.Add(new GTAStreet("Marathon Ave", 30f));
        Streets.Add(new GTAStreet("Boulevard Del Perro", 35f));
        Streets.Add(new GTAStreet("Cougar Ave", 30f));
        Streets.Add(new GTAStreet("Liberty St", 30f));
        Streets.Add(new GTAStreet("Bay City Incline", 40f));
        Streets.Add(new GTAStreet("Conquistador St", 25f));
        Streets.Add(new GTAStreet("Cortes St", 25f));
        Streets.Add(new GTAStreet("Vitus St", 25f));
        Streets.Add(new GTAStreet("Aguja St", 25f));
        Streets.Add(new GTAStreet("Goma St", 25f));
        Streets.Add(new GTAStreet("Melanoma St", 25f));
        Streets.Add(new GTAStreet("Palomino Ave", 35f));
        Streets.Add(new GTAStreet("Invention Ct", 25f));
        Streets.Add(new GTAStreet("Imagination Ct", 25f));
        Streets.Add(new GTAStreet("Rub St", 25f));
        Streets.Add(new GTAStreet("Tug St", 25f));
        Streets.Add(new GTAStreet("Ginger St", 30f));
        Streets.Add(new GTAStreet("Lindsay Circus", 30f));
        Streets.Add(new GTAStreet("Calais Ave", 35f));
        Streets.Add(new GTAStreet("Adam's Apple Blvd", 40f));
        Streets.Add(new GTAStreet("Alta St", 40f));
        Streets.Add(new GTAStreet("Integrity Way", 30f));
        Streets.Add(new GTAStreet("Swiss St", 30f));
        Streets.Add(new GTAStreet("Strawberry Ave", 40f));
        Streets.Add(new GTAStreet("Capital Blvd", 30f));
        Streets.Add(new GTAStreet("Crusade Rd", 30f));
        Streets.Add(new GTAStreet("Innocence Blvd", 40f));
        Streets.Add(new GTAStreet("Davis Ave", 40f));
        Streets.Add(new GTAStreet("Little Bighorn Ave", 35f));
        Streets.Add(new GTAStreet("Roy Lowenstein Blvd", 35f));
        Streets.Add(new GTAStreet("Jamestown St", 30f));
        Streets.Add(new GTAStreet("Carson Ave", 35f));
        Streets.Add(new GTAStreet("Grove St", 30f));
        Streets.Add(new GTAStreet("Brouge Ave", 30f));
        Streets.Add(new GTAStreet("Covenant Ave", 30f));
        Streets.Add(new GTAStreet("Dutch London St", 40f));
        Streets.Add(new GTAStreet("Signal St", 30f));
        Streets.Add(new GTAStreet("Elysian Fields Fwy", 50f));
        Streets.Add(new GTAStreet("Plaice Pl", 30f));
        Streets.Add(new GTAStreet("Chum St", 40f));
        Streets.Add(new GTAStreet("Chupacabra St", 30f));
        Streets.Add(new GTAStreet("Miriam Turner Overpass", 30f));
        Streets.Add(new GTAStreet("Autopia Pkwy", 35f));
        Streets.Add(new GTAStreet("Exceptionalists Way", 35f));
        Streets.Add(new GTAStreet("La Puerta Fwy", 60f));
        Streets.Add(new GTAStreet("New Empire Way", 30f));
        Streets.Add(new GTAStreet("Runway1", 90f));
        Streets.Add(new GTAStreet("Greenwich Pkwy", 35f));
        Streets.Add(new GTAStreet("Kortz Dr", 30f));
        Streets.Add(new GTAStreet("Banham Canyon Dr", 40f));
        Streets.Add(new GTAStreet("Buen Vino Rd", 40f));
        Streets.Add(new GTAStreet("Route 68", 55f));
        Streets.Add(new GTAStreet("Zancudo Grande Valley", 40f));
        Streets.Add(new GTAStreet("Zancudo Barranca", 40f));
        Streets.Add(new GTAStreet("Galileo Rd", 40f));
        Streets.Add(new GTAStreet("Mt Vinewood Dr", 40f));
        Streets.Add(new GTAStreet("Marlowe Dr", 40f));
        Streets.Add(new GTAStreet("Milton Rd", 35f));
        Streets.Add(new GTAStreet("Kimble Hill Dr", 35f));
        Streets.Add(new GTAStreet("Normandy Dr", 35f));
        Streets.Add(new GTAStreet("Hillcrest Ave", 35f));
        Streets.Add(new GTAStreet("Hillcrest Ridge Access Rd", 35f));
        Streets.Add(new GTAStreet("North Sheldon Ave", 35f));
        Streets.Add(new GTAStreet("Lake Vinewood Dr", 35f));
        Streets.Add(new GTAStreet("Lake Vinewood Est", 35f));
        Streets.Add(new GTAStreet("Baytree Canyon Rd", 40f));
        Streets.Add(new GTAStreet("North Conker Ave", 35f));
        Streets.Add(new GTAStreet("Wild Oats Dr", 35f));
        Streets.Add(new GTAStreet("Whispymound Dr", 35f));
        Streets.Add(new GTAStreet("Didion Dr", 35f));
        Streets.Add(new GTAStreet("Cox Way", 35f));
        Streets.Add(new GTAStreet("Picture Perfect Drive", 35f));
        Streets.Add(new GTAStreet("South Mo Milton Dr", 35f));
        Streets.Add(new GTAStreet("Cockingend Dr", 35f));
        Streets.Add(new GTAStreet("Mad Wayne Thunder Dr", 35f));
        Streets.Add(new GTAStreet("Hangman Ave", 35f));
        Streets.Add(new GTAStreet("Dunstable Ln", 35f));
        Streets.Add(new GTAStreet("Dunstable Dr", 35f));
        Streets.Add(new GTAStreet("Greenwich Way", 35f));
        Streets.Add(new GTAStreet("Greenwich Pl", 35f));
        Streets.Add(new GTAStreet("Hardy Way", 35f));
        Streets.Add(new GTAStreet("Richman St", 35f));
        Streets.Add(new GTAStreet("Ace Jones Dr", 35f));
        Streets.Add(new GTAStreet("Los Santos Freeway", 65f));
        Streets.Add(new GTAStreet("Senora Rd", 40f));
        Streets.Add(new GTAStreet("Nowhere Rd", 25f));
        Streets.Add(new GTAStreet("Smoke Tree Rd", 35f));
        Streets.Add(new GTAStreet("Cholla Rd", 35f));
        Streets.Add(new GTAStreet("Cat-Claw Ave", 35f));
        Streets.Add(new GTAStreet("Senora Way", 40f));
        Streets.Add(new GTAStreet("Palomino Fwy", 60f));
        Streets.Add(new GTAStreet("Shank St", 25f));
        Streets.Add(new GTAStreet("Macdonald St", 35f));
        Streets.Add(new GTAStreet("Route 68 Approach", 55f));
        Streets.Add(new GTAStreet("Vinewood Park Dr", 35f));
        Streets.Add(new GTAStreet("Vinewood Blvd", 40f));
        Streets.Add(new GTAStreet("Mirror Park Blvd", 35f));
        Streets.Add(new GTAStreet("Glory Way", 35f));
        Streets.Add(new GTAStreet("Bridge St", 35f));
        Streets.Add(new GTAStreet("West Mirror Drive", 35f));
        Streets.Add(new GTAStreet("Nikola Ave", 35f));
        Streets.Add(new GTAStreet("East Mirror Dr", 35f));
        Streets.Add(new GTAStreet("Nikola Pl", 25f));
        Streets.Add(new GTAStreet("Mirror Pl", 35f));
        Streets.Add(new GTAStreet("El Rancho Blvd", 40f));
        Streets.Add(new GTAStreet("Olympic Fwy", 60f));
        Streets.Add(new GTAStreet("Fudge Ln", 25f));
        Streets.Add(new GTAStreet("Amarillo Vista", 25f));
        Streets.Add(new GTAStreet("Labor Pl", 35f));
        Streets.Add(new GTAStreet("El Burro Blvd", 35f));
        Streets.Add(new GTAStreet("Sustancia Rd", 45f));
        Streets.Add(new GTAStreet("South Shambles St", 30f));
        Streets.Add(new GTAStreet("Hanger Way", 30f));
        Streets.Add(new GTAStreet("Orchardville Ave", 30f));
        Streets.Add(new GTAStreet("Popular St", 40f));
        Streets.Add(new GTAStreet("Buccaneer Way", 45f));
        Streets.Add(new GTAStreet("Abattoir Ave", 35f));
        Streets.Add(new GTAStreet("Voodoo Place", 30f));
        Streets.Add(new GTAStreet("Mutiny Rd", 35f));
        Streets.Add(new GTAStreet("South Arsenal St", 35f));
        Streets.Add(new GTAStreet("Forum Dr", 35f));
        Streets.Add(new GTAStreet("Morningwood Blvd", 35f));
        Streets.Add(new GTAStreet("Dorset Dr", 40f));
        Streets.Add(new GTAStreet("Caesars Place", 25f));
        Streets.Add(new GTAStreet("Spanish Ave", 30f));
        Streets.Add(new GTAStreet("Portola Dr", 30f));
        Streets.Add(new GTAStreet("Edwood Way", 25f));
        Streets.Add(new GTAStreet("San Vitus Blvd", 40f));
        Streets.Add(new GTAStreet("Eclipse Blvd", 35f));
        Streets.Add(new GTAStreet("Gentry Lane", 30f));
        Streets.Add(new GTAStreet("Las Lagunas Blvd", 40f));
        Streets.Add(new GTAStreet("Power St", 40f));
        Streets.Add(new GTAStreet("Mt Haan Rd", 40f));
        Streets.Add(new GTAStreet("Elgin Ave", 40f));
        Streets.Add(new GTAStreet("Hawick Ave", 35f));
        Streets.Add(new GTAStreet("Meteor St", 30f));
        Streets.Add(new GTAStreet("Alta Pl", 30f));
        Streets.Add(new GTAStreet("Occupation Ave", 35f));
        Streets.Add(new GTAStreet("Carcer Way", 40f));
        Streets.Add(new GTAStreet("Eastbourne Way", 30f));
        Streets.Add(new GTAStreet("Rockford Dr", 35f));
        Streets.Add(new GTAStreet("Abe Milton Pkwy", 35f));
        Streets.Add(new GTAStreet("Laguna Pl", 30f));
        Streets.Add(new GTAStreet("Sinners Passage", 30f));
        Streets.Add(new GTAStreet("Atlee St", 30f));
        Streets.Add(new GTAStreet("Sinner St", 30f));
        Streets.Add(new GTAStreet("Supply St", 30f));
        Streets.Add(new GTAStreet("Amarillo Way", 35f));
        Streets.Add(new GTAStreet("Tower Way", 35f));
        Streets.Add(new GTAStreet("Decker St", 35f));
        Streets.Add(new GTAStreet("Tackle St", 25f));
        Streets.Add(new GTAStreet("Low Power St", 35f));
        Streets.Add(new GTAStreet("Clinton Ave", 35f));
        Streets.Add(new GTAStreet("Fenwell Pl", 35f));
        Streets.Add(new GTAStreet("Utopia Gardens", 25f));
        Streets.Add(new GTAStreet("Cavalry Blvd", 35f));
        Streets.Add(new GTAStreet("South Boulevard Del Perro", 35f));
        Streets.Add(new GTAStreet("Americano Way", 25f));
        Streets.Add(new GTAStreet("Sam Austin Dr", 25f));
        Streets.Add(new GTAStreet("East Galileo Ave", 35f));
        Streets.Add(new GTAStreet("Galileo Park", 35f));
        Streets.Add(new GTAStreet("West Galileo Ave", 35f));
        Streets.Add(new GTAStreet("Tongva Dr", 40f));
        Streets.Add(new GTAStreet("Zancudo Rd", 35f));
        Streets.Add(new GTAStreet("Movie Star Way", 35f));
        Streets.Add(new GTAStreet("Heritage Way", 35f));
        Streets.Add(new GTAStreet("Perth St", 25f));
        Streets.Add(new GTAStreet("Chianski Passage", 30f));
        Streets.Add(new GTAStreet("Lolita Ave", 35f));
        Streets.Add(new GTAStreet("Meringue Ln", 35f));
        Streets.Add(new GTAStreet("Strangeways Dr", 30f));
    }
    private static void setupLicensePlates()
    {
        List<string> StartingPlateOptions = new List<string> { "BRNEBRO", "IMWITHER", "JOE30303", "JEBSGUAC", "MAGA2020", "YNGGANG", "POCAHNTS", "NOTPHOON", "LYINTED" };
        SpareLicensePlates.Add(new GTALicensePlate(StartingPlateOptions.PickRandom(), 1, 1, false));
    }

    //Other
    public static GTAWeapon GetRandomWeapon(int RequestedLevel)
    {
        GTAWeapon MyWeapon = Weapons.OrderBy(s => rnd.Next()).Where(s => s.WeaponLevel == RequestedLevel).First();
        return MyWeapon;
    }
    public static VehicleInfo GetVehicleInfo(GTAVehicle myVehicle)
    {
        VehicleInfo ToReturn = VehicleLookup.Vehicles.Where(x => x.Hash == myVehicle.VehicleEnt.Model.Hash).FirstOrDefault();
        return ToReturn;
    }
    public static void RequestAnimationDictionay(String sDict)
    {
        NativeFunction.CallByName<bool>("REQUEST_ANIM_DICT", sDict);
        while (!NativeFunction.CallByName<bool>("HAS_ANIM_DICT_LOADED", sDict))
            GameFiber.Yield();
    }
    public static GTAVehicle GetPlayersCurrentTrackedVehicle()
    {
        if (!Game.LocalPlayer.Character.IsInAnyVehicle(false))
            return null;
        else
        {
            Vehicle CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
            return TrackedVehicles.Where(x => x.VehicleEnt.Handle == CurrVehicle.Handle).FirstOrDefault();
        }

    }
    public static void SetPedUnarmed(Ped Pedestrian,bool SetCantChange)
    {
        if (!(Pedestrian.Inventory.EquippedWeapon == null))
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Pedestrian, (uint)2725352035, true); //Unequip weapon so you don't get shot
            if(SetCantChange)
                NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Pedestrian, false);
        }
    }

    //Test Code
    public static Vehicle GetClosestVehicleToPlayer()
    {
        Vehicle[] NearbyVehicles = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 10f, GetEntitiesFlags.ConsiderAllVehicles).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));
        return NearbyVehicles.OrderBy(x => x.DistanceTo2D(Game.LocalPlayer.Character.Position)).FirstOrDefault();
    }
    internal static void AddRemovePlayerHelmet()
    {
        if (Game.LocalPlayer.Character.IsWearingHelmet)
            Game.LocalPlayer.Character.RemoveHelmet(false);
        else
        {
            if (PedOriginallyHadHelmet)
            {
                PropComponent MyPropComponent = InstantAction.myPedVariation.MyPedProps.Where(x => x.PropID == 0).FirstOrDefault();
                if (MyPropComponent == null)
                    return;

                Game.LocalPlayer.Character.GiveHelmet(true, (Rage.HelmetTypes)MyPropComponent.DrawableID, MyPropComponent.TextureID);
                WriteToLog("AddRemovePlayerHelmet", "Original");
            }
            else
            {
                Game.LocalPlayer.Character.GiveHelmet(true, HelmetTypes.RegularMotorcycleHelmet, 0);
                WriteToLog("AddRemovePlayerHelmet", "Not Original");
            }
        }
    }
    private static void UnlockCarDoorOld(Vehicle ToEnter, int SeatTryingToEnter)
    {
        if (!Game.IsControlPressed(2, GameControl.Enter))//holding enter go thru normal
            return;
        try
        {
            GameFiber.StartNew(delegate
            {
                if (SeatTryingToEnter != -1)
                    return;


                SetPedUnarmed(Game.LocalPlayer.Character, false);

                bool Continue = true;
                ToEnter.MustBeHotwired = true;



                //Vector3 GameEntryPosition = NativeFunction.CallByHash<Vector3>(0xC0572928C0ABFDA3, ToEnter, 0); //old
                Vector3 GameEntryPosition = ToEnter.GetBonePosition("handle_dside_f");


                Vector3 CarPosition = ToEnter.Position;
                //Game.LocalPlayer.Character.Tasks.GoStraightToPosition(GameEntryPosition, 1f, ToEnter.Heading, 1f, 10000);
                //uint GameTimeStartedWalking = Game.GameTime;
                //while (Game.LocalPlayer.Character.DistanceTo2D(GameEntryPosition) >= 0.05f && Game.GameTime - GameTimeStartedWalking < 10000 && CarPosition == ToEnter.Position && Game.LocalPlayer.Character.Speed > 0.5f)
                //{
                //    Rage.Debug.DrawArrowDebug(new Vector3(GameEntryPosition.X, GameEntryPosition.Y, GameEntryPosition.Z), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Yellow);
                //    GameFiber.Sleep(100);
                //}
                float DesiredHeading = ToEnter.Heading - 90f;
                NativeFunction.CallByName<uint>("TASK_PED_SLIDE_TO_COORD", Game.LocalPlayer.Character, GameEntryPosition.X, GameEntryPosition.Y, GameEntryPosition.Z, DesiredHeading, 3000);
                // GameFiber.Sleep(3000);



                uint GameTimeStarted = Game.GameTime;

                while (Game.GameTime - GameTimeStarted <= 1000)
                {
                    GameFiber.Yield();
                    if (Game.IsControlJustPressed(2, GameControl.MoveUp) || Game.IsControlJustPressed(2, GameControl.MoveRight) || Game.IsControlJustPressed(2, GameControl.MoveDown) || Game.IsControlJustPressed(2, GameControl.MoveLeft))
                    {
                        Continue = false;
                        break;
                    }
                }

                Rage.Object Screwdriver = new Rage.Object("prop_tool_screwdvr01", Game.LocalPlayer.Character.GetOffsetPositionUp(2f));
                CreatedObjects.Add(Screwdriver);
                int BoneIndexRightHand = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 57005);
                Screwdriver.AttachTo(Game.LocalPlayer.Character, BoneIndexRightHand, new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f));
                Game.LocalPlayer.Character.Tasks.Clear();

                NativeFunction.CallByName<int>("TASK_ACHIEVE_HEADING", Game.LocalPlayer.Character, DesiredHeading, 1500);
                uint GameTimeStartedHeading = Game.GameTime;
                while (Game.LocalPlayer.Character.Heading != DesiredHeading && Game.GameTime - GameTimeStartedHeading < 700)
                {
                    GameFiber.Yield();
                    if (Game.IsControlJustPressed(2, GameControl.MoveUp) || Game.IsControlJustPressed(2, GameControl.MoveRight) || Game.IsControlJustPressed(2, GameControl.MoveDown) || Game.IsControlJustPressed(2, GameControl.MoveLeft))
                    {
                        Continue = false;
                        break;
                    }
                }

                RequestAnimationDictionay("veh@break_in@0h@p_m_one@");
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "veh@break_in@0h@p_m_one@", "std_force_entry_ds", 2.0f, -2.0f, -1, 0, 0, false, false, false);

                PlayerBreakingIntoCar = true;

                GameTimeStarted = Game.GameTime;
                while (Game.GameTime - GameTimeStarted <= 1750)
                {
                    GameFiber.Yield();
                    if (Game.IsControlJustPressed(2, GameControl.MoveUp) || Game.IsControlJustPressed(2, GameControl.MoveRight) || Game.IsControlJustPressed(2, GameControl.MoveDown) || Game.IsControlJustPressed(2, GameControl.MoveLeft))
                    {
                        Continue = false;
                        break;
                    }
                }
                //GameFiber.Sleep(1750);

                if (!Continue)
                {
                    Game.LocalPlayer.Character.Tasks.Clear();
                    Screwdriver.Delete();
                    PlayerBreakingIntoCar = false;
                    return;
                }

                ToEnter.LockStatus = VehicleLockStatus.Unlocked;
                ToEnter.Doors[SeatTryingToEnter + 1].Open(true, false);

                GameTimeStarted = Game.GameTime;
                while (Game.GameTime - GameTimeStarted <= 1000)
                {
                    GameFiber.Yield();
                    if (Game.IsControlJustPressed(2, GameControl.MoveUp) || Game.IsControlJustPressed(2, GameControl.MoveRight) || Game.IsControlJustPressed(2, GameControl.MoveDown) || Game.IsControlJustPressed(2, GameControl.MoveLeft))
                    {
                        Continue = false;
                        break;
                    }
                }

                if (!Continue)
                {
                    Game.LocalPlayer.Character.Tasks.Clear();
                    Screwdriver.Delete();
                    PlayerBreakingIntoCar = false;
                    return;
                }

                //GameFiber.Sleep(1000);
                Game.LocalPlayer.Character.Tasks.EnterVehicle(ToEnter, SeatTryingToEnter);
                GameFiber.Sleep(5000);
                Screwdriver.Delete();
                PlayerBreakingIntoCar = false;
            });
        }
        catch (Exception e)
        {
            foreach (Rage.Object obj in CreatedObjects.Where(x => x.Exists()))
                obj.Delete();
            CreatedObjects.Clear();
            PlayerBreakingIntoCar = false;
            WriteToLog("UnlockCarDoor", e.Message);
        }


    }
    private static bool IsRunningRedNew()
    {

        //Vector3 PlayerPos = Game.LocalPlayer.Character.Position;
        //int NodeID = NativeFunction.CallByName<int>("GET_NTH_CLOSEST_VEHICLE_NODE_ID", PlayerPos.X, PlayerPos.Y, PlayerPos.Z, 1, 0, 300f, 300f);


        return false;

        //List<Ped> CloseDrivers = PoliceScanningSystem.Civilians.Where(x => x.Exists() && x.IsAlive && x.DistanceTo2D(Game.LocalPlayer.Character) <= 20f && x.IsInAnyVehicle(false)).ToList();
        //Vehicle PlayersVehicle = Game.LocalPlayer.Character.CurrentVehicle;

        //foreach (Ped TrafficPed in CloseDrivers)
        //{
        //    Vehicle TrafficeVehicle = TrafficPed.CurrentVehicle;
        //    bool StoppedAtRed = NativeFunction.CallByHash<bool>(0x2959F696AE390A99, TrafficeVehicle);
        //    if (StoppedAtRed)
        //    {
        //        Rage.Debug.DrawArrowDebug(new Vector3(TrafficeVehicle.Position.X, TrafficeVehicle.Position.Y, TrafficeVehicle.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Red);
        //        if ((PlayersVehicle.FacingSameDirection(TrafficeVehicle) || PlayersVehicle.FacingOppositeDirection(TrafficeVehicle)) && PlayersVehicle.InFrontOf(TrafficeVehicle) && PlayersVehicle.Speed >= 3f)
        //        {
        //            return true;
        //        }
        //    }
        //}
        //return false;














        //Vehicle[] MyVehciles = World.GetAllVehicles();

        //Vehicle TrafficeVehicle = (Vehicle)World.GetClosestEntity(Game.LocalPlayer.Character.Position, 20f, GetEntitiesFlags.ConsiderGroundVehicles | GetEntitiesFlags.ExcludePlayerVehicle);
        // if (TrafficeVehicle == null)
        //    return false;
        //Vehicle[] TrafficeVehicles = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 20f, GetEntitiesFlags.ConsiderGroundVehicles | GetEntitiesFlags.ExcludePlayerVehicle).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));
        // foreach (Vehicle TrafficeVehicle in TrafficeVehicles)
        // {

        // Vehicle PlayersVehicle = Game.LocalPlayer.Character.CurrentVehicle;
        //bool FacingSameDirection = PlayersVehicle.FacingSameDirection(TrafficeVehicle);
        //bool FacingOppositeDirection = PlayersVehicle.FacingOppositeDirection(TrafficeVehicle);

        //if (FacingSameDirection)
        //    Rage.Debug.DrawArrowDebug(new Vector3(TrafficeVehicle.Position.X, TrafficeVehicle.Position.Y, TrafficeVehicle.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Green);
        //if (FacingOppositeDirection)
        //    Rage.Debug.DrawArrowDebug(new Vector3(TrafficeVehicle.Position.X, TrafficeVehicle.Position.Y, TrafficeVehicle.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Red);

        //    // bool StoppedAtRed = NativeFunction.CallByHash<bool>(0x2959F696AE390A99, TrafficeVehicle);
        //    if (StoppedAtRed)
        //{
        //    if (PlayersVehicle.FacingSameDirection(TrafficeVehicle) || PlayersVehicle.FacingOppositeDirection(TrafficeVehicle) && PlayersVehicle.InFrontOf(TrafficeVehicle) && PlayersVehicle.Speed >= 3f)
        //    {
        //        return true;
        //    }
        //}
        //// }
        //return false;
        //this.vehs = World.GetNearbyVehicles(Game.Player.Character.Position, 9f);
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
        //    if ((double)Vector3.Distance(this.autolista[index].Position, Game.Player.Character.Position) > 35.0)
        //        this.autolista.Remove(this.autolista[index]);
        //}


        //for (int index = 0; index < this.autolista.Count; ++index)
        //{
        //    if (Function.Call<bool>(Hash._0x2959F696AE390A99, (InputArgument)this.autolista[index]))
        //    {
        //        float num = Vector3.Angle(this.autolista[index].ForwardVector, Game.Player.Character.ForwardVector);
        //        if ((double)num <= 35.0 && ((double)num <= 35.0 && (double)Vector3.Distance(this.autolista[index].Position, Game.Player.Character.Position) > 30.0 && (double)Vector3.Angle(this.autolista[index].Position - Game.Player.Character.Position, Game.Player.Character.ForwardVector) > 150.0))
        //            return true;
        //    }
        //}
        //return false;

    }
    private static bool RunningRedLight()
    {

        Vehicle[] Vehicles = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 10f, GetEntitiesFlags.ConsiderAllVehicles | GetEntitiesFlags.ExcludePlayerVehicle).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));
        foreach (Vehicle vehicle in Vehicles)
        {
            if (NativeFunction.CallByName<bool>("IS_VEHICLE_STOPPED_AT_TRAFFIC_LIGHTS", vehicle))
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
    private static void RunningRedTick()
    {
        Vector3 PlayerPos = Game.LocalPlayer.Character.Position;
        Vector3 MyOutput;
        int Density;
        int Flag;
        int NodeID = 0;

        unsafe
        {
            NativeFunction.CallByName<bool>("GET_VEHICLE_NODE_PROPERTIES", PlayerPos.X, PlayerPos.Y, PlayerPos.Z, &Density, &Flag);
        }



        string s = Convert.ToString(Flag, 2); //Convert to binary in a string

        int[] bits = s.PadLeft(10, '0') // Add 0's from left
                     .Select(c => int.Parse(c.ToString())) // convert each char to int
                     .ToArray(); // Convert IEnumerable from select to Array



        InstantAction.Text(string.Format("Node Flag: {0}, {1}", Flag, string.Join("", bits)), 0.5f, 0.5f, 0.5f, true, Color.Yellow);


        NodeID = NativeFunction.CallByName<int>("GET_NTH_CLOSEST_VEHICLE_NODE_ID", PlayerPos.X, PlayerPos.Y, PlayerPos.Z, 1, 0, 300f, 300f);
        unsafe
        {
            NativeFunction.CallByName<bool>("GET_VEHICLE_NODE_POSITION", NodeID, &MyOutput);
        }

        if (Flag > 128)// intersection like stuff? seems to be a bit that add up? 2 lanes + 128 for intersection = 130?
        {
            Rage.Debug.DrawArrowDebug(new Vector3(MyOutput.X, MyOutput.Y, MyOutput.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Red);
        }
        else
        {
            Rage.Debug.DrawArrowDebug(new Vector3(MyOutput.X, MyOutput.Y, MyOutput.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Green);
        }



        //Vector3 PlayerPos = Game.LocalPlayer.Character.Position;

        //int NodeID = 0;

        //Vector3 MyOutput;

        //for (int i = 1; i < 101; i++)
        //{
        //    NodeID = NativeFunction.CallByName<int>("GET_NTH_CLOSEST_VEHICLE_NODE_ID", PlayerPos.X, PlayerPos.Y, PlayerPos.Z, i, 0, 300f, 300f);
        //    int Density;
        //    int Flag;

        //    unsafe
        //    {
        //        NativeFunction.CallByName<bool>("GET_VEHICLE_NODE_POSITION", NodeID, &MyOutput);
        //        NativeFunction.CallByName<bool>("GET_VEHICLE_NODE_PROPERTIES", MyOutput.X, MyOutput.Y, MyOutput.Z, &Density,&Flag);
        //    }
        //    if(Flag == 4 || Flag == 128 || Flag == 512)
        //    {
        //        Rage.Debug.DrawArrowDebug(new Vector3(MyOutput.X, MyOutput.Y, MyOutput.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Orange);
        //    }
        //}





        //Vector3 MyOutput;

        //unsafe
        //{
        //    NativeFunction.CallByName<bool>("GET_VEHICLE_NODE_POSITION", NodeID, &MyOutput);
        //}



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
    private static void CreatePassengerCop(GTACop Cop)
    {
        if (Cop.CopPed.CurrentVehicle.IsSeatFree(0))
        {
            Ped CreatedCop = new Ped("s_m_y_cop_01", new Vector3(0f, 0f, 0f), Cop.CopPed.Heading);
            CreatedCop.WarpIntoVehicle(Cop.CopPed.CurrentVehicle, 0);
        }
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

    //Debug 
    private static void AddUpdateLastWantedBlip(Vector3 Position)
    {
        if (Position == Vector3.Zero)
        {
            if (LastWantedCenterBlip.Exists())
                LastWantedCenterBlip.Delete();
            return;
        }
        if (!LastWantedCenterBlip.Exists())
        {
            LastWantedCenterBlip = new Blip(LastWantedCenterPosition, 250f);
            LastWantedCenterBlip.Name = "Last Wanted Center Position";
            LastWantedCenterBlip.Color = Color.Yellow;
            LastWantedCenterBlip.Alpha = 0.5f;

            NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)LastWantedCenterBlip.Handle, true);
            CreatedBlips.Add(LastWantedCenterBlip);
        }
        if (LastWantedCenterBlip.Exists())
            LastWantedCenterBlip.Position = Position;
    }
    private static void AddUpdateCurrentWantedBlip(Vector3 Position)
    {
        if (Position == Vector3.Zero)
        {
            if (CurrentWantedCenterBlip.Exists())
                CurrentWantedCenterBlip.Delete();
            return;
        }
        if (!CurrentWantedCenterBlip.Exists())
        {
            CurrentWantedCenterBlip = new Blip(Position, 100f);
            CurrentWantedCenterBlip.Name = "Current Wanted Center Position";
            CurrentWantedCenterBlip.Color = Color.Red;
            CurrentWantedCenterBlip.Alpha = 0.5f;

            NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)CurrentWantedCenterBlip.Handle, true);
            CreatedBlips.Add(CurrentWantedCenterBlip);
        }
        if (CurrentWantedCenterBlip.Exists())
            CurrentWantedCenterBlip.Position = Position;
    }
    public static void WriteToLog(String ProcedureString, String TextToLog)
    {
        // if (!Logging)
        //     return;
        //if (ProcedureString != "GetCarjackingAnimations")
        //    return;
        //StringBuilder sb = new StringBuilder();
        //sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + ProcedureString + ": " + TextToLog + System.Environment.NewLine);
        //File.AppendAllText("Plugins\\InstantAction\\" + "log.txt", sb.ToString());
        // sb.Clear();

        Game.Console.Print(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + ProcedureString + ": " + TextToLog);
    }
    internal static void Text(string text, float x, float y, float scale, bool center, Color TextColor)
    {
        //Game.Console.Print("Invoke font");
        NativeFunction.Natives.SetTextFont(4);
        //Game.Console.Print("Set scale");
        NativeFunction.Natives.SetTextScale(scale, scale);
        //Game.Console.Print("Calling color ref");
        NativeFunction.CallByName<uint>("SET_TEXT_COLOUR", new NativeArgument[]
        {
               (int)TextColor.R, (int)TextColor.G, (int)TextColor.B, 255
        });
        //Game.Console.Print("Set wrap");
        NativeFunction.Natives.SetTextWrap((float)0.0, (float)1.0);
        //Game.Console.Print("Set centre");
        NativeFunction.Natives.SetTextCentre(center);
        //Game.Console.Print("Set dropshadow");
        NativeFunction.Natives.SetTextDropshadow(2, 2, 0, 0, 0);
        //Game.Console.Print("Set edge");
        NativeFunction.Natives.SetTextEdge(1, 0, 0, 0, 205);
        //Game.Console.Print("Set leading");
        NativeFunction.Natives.SetTextLeading(1);
        //Game.Console.Print("Set entry type");
        NativeFunction.CallByHash<uint>(0x25fbb336df1804cb, "STRING"); // Remplacant fonction SET_TEXT_ENTRY, le nouveau nom est BEGIN_TEXT_COMMAND_DISPLAY_TEXT
                                                                       //Game.Console.Print("Create text component");
                                                                       //NativeFunction.CallByName<uint>("ADD_TEXT_COMPONENT_SUBSTRING_TEXT_LABEL", text); //Pour RPH 0.52
        NativeFunction.CallByHash<uint>(0x25FBB336DF1804CB, text); //BEGIN TEXT COMMAND DISPLAY TEXT
                                                                   //Game.Console.Print("Add substring");
        NativeFunction.CallByHash<uint>(0x6C188BE134E074AA, text); //Hash pour le nom ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME.
                                                                   //Game.Console.Print("Draw text !");
        NativeFunction.CallByHash<uint>(0xCD015E5BB0D96A57, y, x);
        //NativeFunction.CallByName<uint>("_DRAW_TEXT", y, x);
        //Game.Console.Print("Text displayed.");
        return;
    }
    private static void DebugCopReset()
    {
        CurrentPoliceState = PoliceState.Normal;
        Game.LocalPlayer.WantedLevel = 0;
        PoliceScanningSystem.UntaskAll(true);


        foreach (GTACop Cop in PoliceScanningSystem.K9Peds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInHelicopter))
        {
            Cop.CopPed.Delete();
        }
        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInAnyVehicle(false) && !x.CopPed.IsInHelicopter))
        {
            Cop.CopPed.Delete();
        }

        foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && x.CopPed.IsInAnyVehicle(false) && !x.CopPed.IsInHelicopter))
        {
            Cop.CopPed.CurrentVehicle.Delete();
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


        PoliceScanningSystem.RemoveAllCreatedEntities();
    }
    private static void DebugTest1()
    {
        try
        {

        }
        catch (Exception e)
        {
            WriteToLog("Car stuff", e.Message);
        }
    }
    private static void DebugNumpad4()
    {

        PoliceScanningSystem.RemoveAllCreatedEntities();





        GameFiber.StartNew(delegate
        {
            VehicleInfo myLookup = Vehicles.Where(x => x.VehicleClass != VehicleLookup.VehicleClass.Utility).PickRandom();
            Vehicle MyCar = new Vehicle(myLookup.Name, Game.LocalPlayer.Character.GetOffsetPositionFront(4f));
            Ped Driver = new Ped("a_m_y_hipster_01", Game.LocalPlayer.Character.Position.Around2D(5f), 0f);
            PoliceScanningSystem.CreatedEntities.Add(MyCar);
            PoliceScanningSystem.CreatedEntities.Add(Driver);
            Driver.WarpIntoVehicle(MyCar, -1);
            uint GameTimeStarted = Game.GameTime;
            //while (!Game.LocalPlayer.Character.IsGettingIntoVehicle)
            //  GameFiber.Yield();

            WriteToLog("Bones", string.Format("Driver Position: {0}", Driver.Position));
            WriteToLog("Bones", string.Format("MyCar Position: {0}", MyCar.Position));

            // CarJackPedWithWeapon(MyCar, Driver, -1);
            while(Game.GameTime - GameTimeStarted <= 20000)
            {
                //Text(myLookup.VehicleClass.ToString(), 0.5f, 0.5f, 0.75f, true, Color.Black);
                GameFiber.Yield();
            }

            if (Driver.Exists())
                Driver.Delete();

            if (MyCar.Exists())
                MyCar.Delete();

        });

        //GameFiber.StartNew(delegate
        //{
        //    VehicleInfo myLookup = Vehicles.Where(x => x.VehicleClass == VehicleLookup.VehicleClass.Coupe || x.VehicleClass == VehicleLookup.VehicleClass.Sedan || x.VehicleClass == VehicleLookup.VehicleClass.Sports || x.VehicleClass == VehicleLookup.VehicleClass.SUV || x.VehicleClass == VehicleLookup.VehicleClass.Compact).PickRandom();
        //    Vehicle MyCar = new Vehicle(myLookup.Name, Game.LocalPlayer.Character.GetOffsetPositionFront(4f));
        //    Ped Driver = new Ped("a_m_y_hipster_01", Game.LocalPlayer.Character.Position.Around2D(5f), 0f);
        //    PoliceScanningSystem.CreatedEntities.Add(MyCar);
        //    PoliceScanningSystem.CreatedEntities.Add(Driver);
        //    Driver.WarpIntoVehicle(MyCar, -1);
        //    uint GameTimeStarted = Game.GameTime;
        //    //while (!Game.LocalPlayer.Character.IsGettingIntoVehicle)
        //    //  GameFiber.Yield();

        //    WriteToLog("Bones", string.Format("Driver Position: {0}", Driver.Position));
        //    WriteToLog("Bones", string.Format("MyCar Position: {0}", MyCar.Position));

        //    // CarJackPedWithWeapon(MyCar, Driver, -1);
        //    GameFiber.Sleep(20000);
        //    if (Driver.Exists())
        //        Driver.Delete();

        //    if (MyCar.Exists())
        //        MyCar.Delete();

        //});






        //Vehicle[] NearbyVehicles = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 10f, GetEntitiesFlags.ConsiderAllVehicles).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));
        //Vehicle ClosestVehicle = NearbyVehicles.OrderBy(x => x.DistanceTo2D(Game.LocalPlayer.Character.Position)).FirstOrDefault();
        //if (ClosestVehicle != null)
        //{
        //    ClosestVehicle.LockStatus = (Rage.VehicleLockStatus)7;
        //}






        //DispatchAudioSystem.AbortAllAudio();







        //Vehicle MyCar = new Vehicle("gauntlet", Game.LocalPlayer.Character.GetOffsetPositionFront(4f));
        //Ped Driver = new Ped("u_m_y_hippie_01", Game.LocalPlayer.Character.Position.Around2D(5f), 0f);
        //Driver.BlockPermanentEvents = true;

        //Driver.WarpIntoVehicle(MyCar, -1);

        ////uint GameTimeStarted = Game.GameTime;
        ////while (Game.GameTime - GameTimeStarted <= 10000)
        ////{
        ////    Vector3 Resultant = Vector3.Subtract(Game.LocalPlayer.Character.Position, Driver.Position);
        ////    Driver.Heading = NativeFunction.CallByName<float>("GET_HEADING_FROM_VECTOR_2D", Resultant.X, Resultant.Y);
        ////    GameFiber.Yield();
        ////}
        //GameFiber.Sleep(3000);

        //int BoneIndexSpine = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Driver, 11816);
        //Vector3 DriverSeatCoordinates = NativeFunction.CallByName<Vector3>("GET_PED_BONE_COORDS", Driver, BoneIndexSpine, 0f, 0f, 0f);



        //uint GameTimeStarted = Game.GameTime;
        ////while (Game.GameTime - GameTimeStarted <= 10000)
        ////{
        ////    Vector3 Resultant = Vector3.Subtract(Game.LocalPlayer.Character.Position, Driver.Position);
        ////    Driver.Heading = NativeFunction.CallByName<float>("GET_HEADING_FROM_VECTOR_2D", Resultant.X, Resultant.Y);
        ////    GameFiber.Yield();
        ////}



        //Driver.Position = DriverSeatCoordinates;

        //GameFiber.Sleep(3000);

        //Driver.WarpIntoVehicle(MyCar, -1);

        //GameFiber.Sleep(3000);

        ////GameFiber.Sleep(3000);

        //if (MyCar.Exists())
        //    MyCar.Delete();

        //if (Driver.Exists())
        //    Driver.Delete();



        //foreach (DroppedWeapon MyOldGuns in DroppedWeapons)
        //{

        //    WriteToLog("WeaponInventoryChanged", string.Format("Dropped Gun {0},OldAmmo: {1}", MyOldGuns.Weapon.Hash, MyOldGuns.Ammo));

        //}




        //List<string> Bones = new List<string> { "SKEL_ROOT", "skel_root", "SKEL_Pelvis", "SKEL_PELVIS", "skel_pelvis", "SKEL_Spine_Root", "SKEL_SPINE_ROOT", "skel_spine_root", "SKEL_Spine0","SKEL_SPINE0","skel_spine0" };


        //foreach(string Stuff in Bones)
        //{
        //    if(Game.LocalPlayer.Character.HasBone(Stuff))
        //    {
        //        WriteToLog("Bones", string.Format("I have bone: {0}", Stuff));
        //    }
        //}


        //int BoneIndexSpine = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 0);
        //Vector3 MyPosition = NativeFunction.CallByName<Vector3>("GET_PED_BONE_COORDS", Game.LocalPlayer.Character, BoneIndexSpine, 0f, 0f, 0f);
        // WriteToLog("Bones", string.Format("Spine Bone?: {0}", MyPosition));

        //Vehicle[] NearbyVehicles = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 10f, GetEntitiesFlags.ConsiderAllVehicles).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));
        //Vehicle ClosestVehicle = NearbyVehicles.OrderBy(x => x.DistanceTo2D(Game.LocalPlayer.Character.Position)).FirstOrDefault();
        //if (ClosestVehicle != null)
        //{
        //    ClosestVehicle.LockStatus = (Rage.VehicleLockStatus)7;




        //    //Vector3 GameEntryPosition = NativeFunction.CallByHash<Vector3>(0xC0572928C0ABFDA3, ClosestVehicle, 0);
        //    //Vector3 CarPosition = ClosestVehicle.Position;
        //    //float DesiredHeading = ClosestVehicle.Heading - 90f;
        //    ////NativeFunction.CallByName<uint>("TASK_PED_SLIDE_TO_COORD", Game.LocalPlayer.Character, GameEntryPosition.X, GameEntryPosition.Y, GameEntryPosition.Z, DesiredHeading, 3000);

        //    //uint GameTimeStarted = Game.GameTime;

        //    //while (Game.GameTime - GameTimeStarted <= 10000)
        //    //{
        //    //    Rage.Debug.DrawArrowDebug(new Vector3(GameEntryPosition.X, GameEntryPosition.Y, GameEntryPosition.Z), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Yellow);
        //    //    GameFiber.Yield();
        //    //}


        //   // GameFiber.Sleep(3000);

        //}






    }
    private static void DebugNonInvincible()
    {
        Game.LocalPlayer.Character.IsInvincible = false;
        Game.LocalPlayer.Character.Health = 100;
        WriteToLog("KeyDown", "You are NOT invicible");
    }
    private static void DebugInvincible()
    {
        Game.LocalPlayer.Character.IsInvincible = true;
        Game.LocalPlayer.Character.Health = 100;
        WriteToLog("KeyDown", "You are invicible");
    }
    private static void DebugNumpad5()
    {



        Vector3 CurrentWantedLevelPosition = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
        float DistanceToPlayer = Game.LocalPlayer.Character.DistanceTo2D(CurrentWantedLevelPosition);
        float DistanceToPlacePlayerLastSeen = Game.LocalPlayer.Character.DistanceTo2D(PoliceScanningSystem.PlacePlayerLastSeen);
        WriteToLog("WantedLevel", string.Format("CenterPosition: {0},DistanceToPlayer: {1},PlacePlayerLastSeen: {2},DistanceToPlacePlayerLastSeen: {3}", CurrentWantedLevelPosition,DistanceToPlayer,PoliceScanningSystem.PlacePlayerLastSeen, DistanceToPlacePlayerLastSeen));

        WriteToLog("WantedLevel2", string.Format("LastWantedCenterPosition: {0}", LastWantedCenterPosition));

       // DispatchAudioSystem.ReportSuspiciousActivity();



        if (!Game.LocalPlayer.Character.IsInAnyVehicle(false))
            return;

        GTAVehicle VehicleDescription = TrackedVehicles.Where(x => x.VehicleEnt.Handle == Game.LocalPlayer.Character.CurrentVehicle.Handle).FirstOrDefault();
        Vehicle myCar = VehicleDescription.VehicleEnt;

        //if (myCar.Health <= 500 || myCar.EngineHealth <= 300)
        WriteToLog("RoadWorthyness", string.Format("CurrentCar: Health: {0},Engine Health {1}", myCar.Health, myCar.EngineHealth));

        //if (!NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", myCar))
        WriteToLog("RoadWorthyness", string.Format("CurrentCar: ARE_ALL_VEHICLE_WINDOWS_INTACT: {0}", NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", myCar)));

        VehicleDoor[] CarDoors = myCar.GetDoors();

        foreach (VehicleDoor myDoor in CarDoors)
        {
            //if (myDoor.IsDamaged)
            WriteToLog("RoadWorthyness", string.Format("CurrentCar: door {0} Is Damaged: {1}", myDoor.Index, myDoor.IsDamaged));
        }


        bool LightsOn;
        bool HighbeamsOn;

        unsafe
        {
            NativeFunction.CallByName<bool>("GET_VEHICLE_LIGHTS_STATE", myCar, &LightsOn, &HighbeamsOn);
        }

        WriteToLog("RoadWorthyness", string.Format("CurrentCar: IsStolen: {0},IsRoadWorthy: {1}, .CarPlate.IsWanted: {2},ColorMatchesDescription: {3},MatchesOriginalDescription: {4}", VehicleDescription.IsStolen, myCar.IsRoadWorthy(), VehicleDescription.CarPlate.IsWanted, VehicleDescription.ColorMatchesDescription, VehicleDescription.MatchesOriginalDescription));
        WriteToLog("RoadWorthyness", string.Format("CurrentCar: Night: {0},LightsOn: {1}, HighbeamsOn: {2},RightHeadlightDamaged: {3},LeftHeadlightDmaaged: {4}",InstantAction.IsNightTime, LightsOn, HighbeamsOn, NativeFunction.CallByName<bool>("GET_IS_RIGHT_VEHICLE_HEADLIGHT_DAMAGED", myCar), NativeFunction.CallByName<bool>("GET_IS_LEFT_VEHICLE_HEADLIGHT_DAMAGED", myCar)));

        //ReportGrandTheftAuto();
        // ReportSuspiciousVehicle(VehicleDescription);


        // WriteToLog("RoadWorthyness", string.Format("CurrentCar: CurrentLicensePlateWanted: {0},IsStolen:{1},IsWanted:{2}", VehicleDescription.CurrentLicensePlateWanted,VehicleDescription.IsStolen,VehicleDescription.IsWanted));


        WriteToLog("RoadWorthyness", string.Format("CurrentCar: IS_VEHICLE_TYRE_BURST 0: {0}", NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 0, false)));
        WriteToLog("RoadWorthyness", string.Format("CurrentCar: IS_VEHICLE_TYRE_BURST 1: {0}", NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 1, false)));
        WriteToLog("RoadWorthyness", string.Format("CurrentCar: IS_VEHICLE_TYRE_BURST 2: {0}", NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 2, false)));
        WriteToLog("RoadWorthyness", string.Format("CurrentCar: IS_VEHICLE_TYRE_BURST 3: {0}", NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 3, false)));
        WriteToLog("RoadWorthyness", string.Format("CurrentCar: IS_VEHICLE_TYRE_BURST 4: {0}", NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 4, false)));
        WriteToLog("RoadWorthyness", string.Format("CurrentCar: IS_VEHICLE_TYRE_BURST 5: {0}", NativeFunction.CallByName<bool>("IS_VEHICLE_TYRE_BURST", myCar, 5, false)));


        //ReportStolenVehicle(GetPlayersCurrentTrackedVehicle());

        WriteToLog("Civilians", string.Format("Total Civilians: {0}", PoliceScanningSystem.Civilians.Count()));






                //if (!Game.LocalPlayer.Character.IsInAnyVehicle(false))
                //    return;

                //GTAVehicle VehicleDescription = EnteredVehicles.Where(x => x.VehicleEnt.Handle == Game.LocalPlayer.Character.CurrentVehicle.Handle).FirstOrDefault();

                //Color BaseColor = GetBaseColor(VehicleDescription.OriginalColor);
                //ColorLookup LookupColor = ColorLookups.Where(x => x.BaseColor == BaseColor).PickRandom();
                //VehicleInfo VehicleInformation = InstantAction.GetVehicleInfo(VehicleDescription);
                //string ManufacturerScannerFile;
                //if (VehicleInformation != null)
                //{
                //    ManufacturerScannerFile = GetManufacturerScannerFile(VehicleInformation.Manufacturer);
                //    WriteToLog("", string.Format("Name: {0},Manufac {1}, ModelScanner {2},Color {3}", VehicleInformation.Name, VehicleInformation.Manufacturer, VehicleInformation.ModelScannerFile, LookupColor.BaseColor));
                //}
                //else
                //{
                //    WriteToLog("", string.Format("Hash: {0},Name {1}", VehicleDescription.VehicleEnt.Model.Hash, VehicleDescription.VehicleEnt.Model.Name));
                //}




                //VehicleInfo ToReturn = VehicleLookup.Vehicles.Where(x => x.Name.ToUpper() == "CAVALCADE2").FirstOrDefault();
                //WriteToLog("", string.Format("CAVALCADE2: Hash: {0},Name {1}", ToReturn.Hash, ToReturn.Name));

                //ReportStolenVehicle(VehicleDescription);

                //Rage.Object camera = new Rage.Object("prop_ing_camera_01", Game.LocalPlayer.Character.GetOffsetPosition(Vector3.RelativeFront * 2));



                //Rage.Object FoodBag = new Rage.Object("prop_tool_screwdvr01", Game.LocalPlayer.Character.GetOffsetPositionFront(2f));

                //int BoneIndex = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 57005);



                ////FoodBag.AttachTo(Game.LocalPlayer.Character, BoneIndex, new Vector3(0.1f, -0.1f, -0.1f), new Rotator(120.0f, 0.0f, 0.0f));
                //FoodBag.AttachTo(Game.LocalPlayer.Character, BoneIndex, new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f));
                ////camera.AttachTo(Game.LocalPlayer.Character, 28252, Vector3.Zero, Rotator.Zero);
                //GameFiber.Sleep(5000);

                //FoodBag.Delete();
                //camera.Delete();
    }
    private static void DebugNumpad6()
    {
        try
        {
            GTAWeapon CurrentWeapon = Weapons.Where(x => x.Hash == (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash).First();

            if (CurrentWeapon != null)
            {
                WeaponVariation.WeaponComponent myComponent = WeaponComponentsLookup.Where(x => x.BaseWeapon == CurrentWeapon.Name && x.Name == "Suppressor").FirstOrDefault();
                if (myComponent == null)
                {
                    WriteToLog("DebugNumpad6", "No Component Found");
                    return;
                }

                WeaponVariation Cool = new WeaponVariation(0);
                Cool.Components.Add(myComponent);

                ApplyWeaponVariation(Game.LocalPlayer.Character, (uint)CurrentWeapon.Hash, Cool);
            }

            WeaponVariation DroppedGunVariation = GetWeaponVariation(Game.LocalPlayer.Character, (uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash);
            foreach (WeaponVariation.WeaponComponent Comp in DroppedGunVariation.Components)
            {
                WriteToLog("GetWeaponVariation", string.Format("Name: {0},HashKey: {1},Hash: {2}", Comp.Name, Comp.HashKey, Comp.Hash));
            }
            WriteToLog("GetWeaponVariation", string.Format("Tint: {0}", DroppedGunVariation.Tint));
        }
        catch (Exception e)
        {
            WriteToLog("DebugApplyPoliceVariation", e.Message);
        }
    }
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[rnd.Next(s.Length)]).ToArray());
    }
    private static void DebugLoop()
    { 
        if (Game.IsKeyDown(Keys.NumPad0))
        {
            DebugNonInvincible();
        }
        if (Game.IsKeyDown(Keys.NumPad1))
        {
            DebugInvincible();
        }
        if (Game.IsKeyDown(Keys.NumPad2))
        {
            if (Game.LocalPlayer.WantedLevel > 0)
                Game.LocalPlayer.WantedLevel = 0;
            else
                Game.LocalPlayer.WantedLevel = 2;

            //ChangeLicensePlate();
        }
        if (Game.IsKeyDown(Keys.NumPad3))
        {
            DebugCopReset();
        }
        if (Game.IsKeyDown(Keys.NumPad4))
        {
            DebugNumpad4();
        }
        if (Game.IsKeyDown(Keys.NumPad5))
        {
            DebugNumpad5();
        }
        if (Game.IsKeyDown(Keys.NumPad6))
        {
            DebugNumpad6();
        }
        if (Game.IsKeyDown(Keys.NumPad7))
        {
            Settings.Debug = true;
            bool prestate = Logging;
            Logging = true;
            foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.CopPed.Exists() && x.CopPed.IsAlive))
            {
                WriteToLog("Debug",string.Format("Cop: {0},Model.Name:{1},isTasked: {2},canSeePlayer: {3},DistanceToPlayer: {4},HurtByPlayer: {5},IssuedHeavyWeapon {6},TaskIsQueued: {7},TaskType: {8},WasRandomSpawn: {9},TaskFiber: {10},CurrentTaskStatus: {11}", 
                        Cop.CopPed.Handle,Cop.CopPed.Model.Name,Cop.isTasked,Cop.canSeePlayer,Cop.DistanceToPlayer,Cop.HurtByPlayer,Cop.IssuedHeavyWeapon,Cop.TaskIsQueued,Cop.TaskType,Cop.WasRandomSpawn,Cop.TaskFiber,Cop.CopPed.Tasks.CurrentTaskStatus));
            }
            Logging = prestate;
        }
        if (Game.IsKeyDown(Keys.NumPad8))
        {
            Logging = !Logging;
            Game.DisplayNotification(string.Format("Logging {0}", Logging));
        }

        if (Game.IsKeyDown(Keys.NumPad9))
        {
            Game.DisplayNotification("Instant Action Deactivated");
            PoliceScanningSystem.Dispose();
            Dispose();
        }


        if (Settings.Debug)
        {
            foreach (GTACop Cop in PoliceScanningSystem.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead))
            {
                if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.InProgress)
                    Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Green);
                else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.Interrupted)
                    Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Purple);
                else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.None)
                    Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.White);
                else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask)
                    Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Orange);
                else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.Preparing)
                    Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Red);
                else if (Cop.CopPed.Tasks.CurrentTaskStatus == Rage.TaskStatus.Unknown)
                    Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Black);
                else
                    Rage.Debug.DrawArrowDebug(new Vector3(Cop.CopPed.Position.X, Cop.CopPed.Position.Y, Cop.CopPed.Position.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Yellow);
            }
            if (Game.LocalPlayer.WantedLevel > 0)
            {
                Vector3 CurrentWantedLevelPosition = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
                Rage.Debug.DrawArrowDebug(new Vector3(CurrentWantedLevelPosition.X, CurrentWantedLevelPosition.Y, CurrentWantedLevelPosition.Z + 2f), Vector3.Zero, Rage.Rotator.Zero, 1f, Color.Blue);
            }
        }

    }

}