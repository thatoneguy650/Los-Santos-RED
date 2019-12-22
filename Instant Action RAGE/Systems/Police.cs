using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class Police
{
    private static readonly Random rnd;
    private static uint WantedLevelStartTime;
    //private static Vector3 LastWantedCenterPosition;
    private static int TimeAimedAtPolice;
    private static bool firedWeapon;
    private static bool aimedAtPolice;
    private static uint GameTimeInterval;
    private static uint LOSInterval;
    private static uint GameTimeCheckedLOS;
    private static bool CanReportLastSeen;
    private static uint GameTimeLastGreyedOut;
    private static Vector3 PlaceWantedStarted;
    private static Blip LastWantedCenterBlip;
    private static Blip CurrentWantedCenterBlip;
    private static uint GameTimeWantedStarted;
    private static uint GameTimeLastWantedEnded;
    public static uint GameTimePoliceStateStart;
    private static uint GameTimeLastSetWanted;
    private static uint GameTimeLastStartedJacking;
    private static bool PrevaimedAtPolice;
    private static bool PrevPlayerIsJacking;
    private static int PrevCiviliansKilledByPlayer;
    private static bool PrevfiredWeapon;
    private static bool PrevPlayerHurtPolice;
    private static bool PrevPlayerKilledPolice;
    private static int PrevCopsKilledByPlayer = 0;
    private static bool PrevPlayerStarsGreyedOut;
    private static bool PrevAnyCanRecognizePlayer;
    private static PoliceState PrevPoliceState;
    public static List<Blip> CreatedBlips = new List<Blip>();
    public static List<Blip> TempBlips = new List<Blip>();
    private static uint GameTimeLastReportedSpotted;

    public static bool AnyPoliceCanSeePlayer { get; set; }
    public static bool AnyPoliceCanRecognizePlayer { get; set; }
    public static bool AnyPoliceCanRecognizePlayerAfterWanted { get; set; }
    public static bool AnyPoliceRecentlySeenPlayer { get; set; }
    public static PoliceState CurrentPoliceState { get; set; }
    public static bool PlayerStarsGreyedOut { get; set; }
    public static bool AnyPoliceSeenPlayerThisWanted { get; set; }
    public static GTACop PrimaryPursuer { get; set; }
    public static int CopsKilledByPlayer { get; set; }
    public static int CiviliansKilledByPlayer { get; set; }
    public static bool PlayerHurtPolice { get; set; }
    public static bool PlayerKilledPolice { get; set; }
    public static bool PlayerKilledCivilians { get; set; }
    public static Vector3 PlacePlayerLastSeen { get; set; }
    public static bool PlayerArtificiallyShooting { get; set; }
    public static PoliceState LastPoliceState { get; set; }
    public static bool PlayerIsPersonOfInterest { get; set; }
    public static bool PlayerIsJacking { get; set; } = false;
    public static bool PlayerLastSeenInVehicle { get; set; }
    public static float PlayerLastSeenHeading { get; set; }
    public static Vector3 PlayerLastSeenForwardVector { get; set; }
    public static bool IsNightTime { get; set; }
    public static int PreviousWantedLevel { get; set; }
    public static int ScanningInterval { get; set; }
    public static bool PoliceInSearchMode
    {
        get
        {
            if (PlayerStarsGreyedOut && PoliceScanning.CopPeds.All(x => !x.RecentlySeenPlayer()))
                return true;
            else
                return false;
        }
    }
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
    public static bool RecentlySetWanted
    {
        get
        {
            if (GameTimeLastSetWanted == 0)
                return false;
            else if (Game.GameTime - GameTimeLastSetWanted <= 5000)
                return true;
            else
                return false;
        }
    }
    public static bool CanPlaySuspectSpotted
    {
        get
        {
            if (PedSwapping.JustTakenOver(10000))
                return false;
            else if (GameTimeLastReportedSpotted == 0)
                return true;
            else if (Game.GameTime - GameTimeLastReportedSpotted >= 25000)
                return true;
            else
                return false;
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
    public static bool IsRunning { get; set; } = true;
    public static Vector3 LastWantedCenterPosition { get; set; }
    static Police()
    {
        rnd = new Random();
    }
    public enum PoliceState
    {
        Normal = 0,
        UnarmedChase = 1,
        CautiousChase = 2,
        DeadlyChase = 3,
        ArrestedWait = 4,
    }
    public static void Initialize()
    {
        WantedLevelStartTime = 0;
        LastWantedCenterPosition = default;
        TimeAimedAtPolice = 0;
        firedWeapon = false;
        aimedAtPolice = false;
        GameTimeInterval = default;
        LOSInterval = 500;//500//750
        GameTimeCheckedLOS = 0;
        CanReportLastSeen = false;
        GameTimeLastGreyedOut = 0;
        PlaceWantedStarted = default;
        LastWantedCenterBlip = default;
        CurrentWantedCenterBlip = default;
        GameTimeWantedStarted = 0;
        GameTimeLastWantedEnded = 0;
        GameTimePoliceStateStart = 0;
        GameTimeLastSetWanted = 0;
        GameTimeLastStartedJacking = 0;
        PrevaimedAtPolice = false;
        PrevPlayerIsJacking = false;
        PrevCiviliansKilledByPlayer = default;
        PrevfiredWeapon = false;
        PrevPlayerHurtPolice = false;
        PrevPlayerKilledPolice = false;
        PrevCopsKilledByPlayer = 0;
        PrevPlayerStarsGreyedOut = false;
        PrevAnyCanRecognizePlayer = false;
        PrevPoliceState = PoliceState.Normal;
        CreatedBlips = new List<Blip>();
        TempBlips = new List<Blip>();
        AnyPoliceCanSeePlayer = false;
        AnyPoliceCanRecognizePlayer  = false;
        AnyPoliceCanRecognizePlayerAfterWanted = false;
        AnyPoliceRecentlySeenPlayer = false;
        CurrentPoliceState = PoliceState.Normal;
        PlayerStarsGreyedOut = false;
        AnyPoliceSeenPlayerThisWanted = false;
        PrimaryPursuer = null;
        CopsKilledByPlayer = 0;
        CiviliansKilledByPlayer  = 0;
        PlayerHurtPolice = false;
        PlayerKilledPolice = false;
        PlayerKilledCivilians  = false;
        PlacePlayerLastSeen = default;
        PlayerArtificiallyShooting = false;
        LastPoliceState = PoliceState.Normal;
        PlayerIsPersonOfInterest = false;
        PlayerIsJacking = false;
        PlayerLastSeenInVehicle = false;
        PlayerLastSeenHeading = 0f;
        PlayerLastSeenForwardVector = default;
        IsNightTime = false;
        PreviousWantedLevel = 0;
        ScanningInterval = 5000;
        GameTimeLastReportedSpotted = 0;
        MainLoop();
    }
    private static void MainLoop()
    {
        var stopwatch = new Stopwatch();
        GameFiber.StartNew(delegate
        {
            try
            {
                while (IsRunning)
                {
                    stopwatch.Start();

                    Tick();//Every Tick

                    if (Game.GameTime > GameTimeInterval + ScanningInterval)
                    {
                        PoliceScanning.ScanForPolice();
                        GameTimeInterval = Game.GameTime;
                    }
                    if (Game.GameTime > GameTimeCheckedLOS + LOSInterval) // was 2000
                    {
                        UpdateTrackedObjects();
                        CheckLOS((Game.LocalPlayer.Character.IsInAnyVehicle(false)) ? (Entity)Game.LocalPlayer.Character.CurrentVehicle : (Entity)Game.LocalPlayer.Character);
                        SetPrimaryPursuer();
                        GameTimeCheckedLOS = Game.GameTime;
                    }
                    //if (stopwatch.ElapsedMilliseconds < 16)//Optional stuff
                    //{
                    //    if (Settings.SpawnPoliceK9 && 1 == 0 && Game.GameTime > K9Interval + 5555) // was 2000
                    //    {
                    //        if (Game.LocalPlayer.WantedLevel > 0 && !InstantAction.PlayerInVehicle && PoliceScanning.K9Peds.Count < 3)
                    //            PoliceSpawning.CreateK9();
                    //        PoliceSpawning.MoveK9s();
                    //        K9Interval = Game.GameTime;
                    //    }
                    //    if (Settings.SpawnRandomPolice && Game.GameTime > RandomCopInterval + 2000)
                    //    {
                    //        if (Game.LocalPlayer.WantedLevel == 0 && PoliceScanning.CopPeds.Where(x => x.WasRandomSpawn).Count() < Settings.SpawnRandomPoliceLimit)
                    //            PoliceSpawning.SpawnRandomCop();
                    //        PoliceSpawning.RemoveFarAwayRandomlySpawnedCops();
                    //        RandomCopInterval = Game.GameTime;
                    //    }
                    //}
                    stopwatch.Stop();
                    if (stopwatch.ElapsedMilliseconds >= 16)
                        LocalWriteToLog("PoliceTick", string.Format("Tick took {0} ms", stopwatch.ElapsedMilliseconds));
                    stopwatch.Reset();
                    GameFiber.Yield();
                }
            }
            catch (Exception e)
            {
                InstantAction.Dispose();
                Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
            }

        });
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    private static void UpdateTrackedObjects()
    {
       
        //if (TrackedCar != null && !TrackedCar.VehicleEnt.Exists())
        //{
        //    TrackedCar = null;
        //    LocalWriteToLog("UpdateTrackedObjects", "Removing abandoned car it doesnt exist");
        //}

        //if (PoliceInSearchMode && !InstantAction.PlayerInVehicle && Game.LocalPlayer.Character.LastVehicle.Exists())
        //{
        //    GTAVehicle AbandonedCar = InstantAction.TrackedVehicles.Where(x => x.VehicleEnt.Handle == Game.LocalPlayer.Character.LastVehicle.Handle).FirstOrDefault();
        //    if(AbandonedCar != null)
        //    {
        //        if(TrackedCar != null && TrackedCar.VehicleEnt.Exists() && TrackedCar.VehicleEnt.Handle != AbandonedCar.VehicleEnt.Handle)
        //        {
        //            TrackedCar = AbandonedCar;
        //            LocalWriteToLog("UpdateTrackedObjects", "Tracking your abandoned car");
        //        }
        //    }
        //}
        //else
        //{
        //    if (TrackedCar != null)
        //    {
        //        TrackedCar = null;
        //        LocalWriteToLog("UpdateTrackedObjects", "Removing tracked car as we arent in need of it");
        //    }
        //}

        //if(TrackedCar != null && TrackedCar.VehicleEnt.Exists())
        //{
        //    if(PlacePlayerLastSeen != TrackedCar.VehicleEnt.Position && PoliceCanSeeEntity(TrackedCar.VehicleEnt))
        //    {
        //        PlacePlayerLastSeen = TrackedCar.VehicleEnt.Position;
        //        LocalWriteToLog("UpdateTrackedObjects", "Updated place player last seen as they saw your abandoned vehicle");
        //    }
        //}

    }
    private static void Tick()
    {
        UpdatePolice();
        GetPoliceState();
        CheckPoliceEvents();
        TrackedVehiclesTick();
        WantedLevelTick();
        PersonOfInterestTick();
    }
    private static void UpdatePolice()
    {
        PlayerStarsGreyedOut = NativeFunction.CallByName<bool>("ARE_PLAYER_STARS_GREYED_OUT", Game.LocalPlayer);
        PlayerIsJacking = Game.LocalPlayer.Character.IsJacking;
        UpdatedCopsStats();
        CheckRecognition();
    }
    public static void UpdatedCopsStats()
    {
        PoliceScanning.CopPeds.RemoveAll(x => !x.CopPed.Exists());
        PoliceScanning.K9Peds.RemoveAll(x => !x.CopPed.Exists() || x.CopPed.IsDead);
        foreach (GTACop Cop in PoliceScanning.CopPeds)
        {
            if (Cop.CopPed.IsDead)
            {
                CheckKilled(Cop);
                continue;
            }
            int NewHealth = Cop.CopPed.Health;
            if (NewHealth != Cop.Health)
            {
                if (PlayerHurtPed(Cop))
                {
                    PlayerHurtPolice = true;
                    Cop.HurtByPlayer = true;
                    LocalWriteToLog("UpdatePolice", String.Format("Cop {0}, Was hurt by player", Cop.CopPed.Handle));
                }
                LocalWriteToLog("UpdatePolice", String.Format("Cop {0}, Health Changed from {1} to {2}", Cop.CopPed.Handle, Cop.Health, NewHealth));
                Cop.Health = NewHealth;
            }
            Cop.isInVehicle = Cop.CopPed.IsInAnyVehicle(false);
            if (Cop.isInVehicle)
            {
                Cop.isInHelicopter = Cop.CopPed.IsInHelicopter;
                if (!Cop.isInHelicopter)
                    Cop.isOnBike = Cop.CopPed.IsOnBike;
            }
            else
            {
                Cop.isInHelicopter = false;
                Cop.isOnBike = false;
            }

            Cop.DistanceToPlayer = Cop.CopPed.RangeTo(Game.LocalPlayer.Character.Position);
            Cop.DistanceToLastSeen = Cop.CopPed.RangeTo(PlacePlayerLastSeen);
        }
        foreach(GTACop Cop in PoliceScanning.CopPeds.Where(x => x.CopPed.IsDead))
        {
            MarkNonPersistent(Cop);
        }
        PoliceScanning.CopPeds.RemoveAll(x => x.CopPed.IsDead);
    }
    public static void CheckKilled(GTACop Cop)
    {
        if (Cop.WasRandomSpawn)
        {

        }
        if (PlayerHurtPed(Cop))
        {
            Cop.HurtByPlayer = true;
            PlayerHurtPolice = true;
        }
        if (PlayerKilledPed(Cop))
        {
            CopsKilledByPlayer++;
            PlayerKilledPolice = true;
        }
    }
    private static bool PlayerHurtPed(GTACop Cop)
    {
        if (NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY", Cop.CopPed, Game.LocalPlayer.Character, true))
        {
            return true;

        }
        else if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY", Cop.CopPed, Game.LocalPlayer.Character.CurrentVehicle, true))
        {
            return true;
        }
        return false;
    }
    private static bool PlayerKilledPed(GTACop Cop)
    {
        try
        {
            if (Cop.CopPed.IsDead)
            {
                Entity killer = NativeFunction.Natives.GetPedSourceOfDeath<Entity>(Cop.CopPed);
                if (killer.Handle == Game.LocalPlayer.Character.Handle || (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Handle == killer.Handle))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        catch
        {
            if (Cop.HurtByPlayer)
                return true;
            else
                return false;
        }
    }
    private static bool PlayerKilledPed(Ped Pedestrian)
    {
        try
        {
            if (Pedestrian.IsDead)
            {
                Entity killer = NativeFunction.Natives.GetPedSourceOfDeath<Entity>(Pedestrian);
                if (killer.Handle == Game.LocalPlayer.Character.Handle || (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Handle == killer.Handle))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }
        catch
        {
            return false;
        }
    }
    private static void CheckLOS(Entity EntityToCheck)
    {
        int TotalEntityNativeLOSChecks = 0;
        bool SawPlayerThisCheck = false;
        float RangeToCheck = 55f;

        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInHelicopter))
        {
            if (SawPlayerThisCheck && TotalEntityNativeLOSChecks >= 3 && Cop.GameTimeLastLOSCheck <= 1500)//we have already done 3 checks, saw us and they were looked at last check
            {
                break;
            }
            Cop.GameTimeLastLOSCheck = Game.GameTime;
            if (Cop.DistanceToPlayer <= RangeToCheck && Cop.CopPed.PlayerIsInFront() && !Cop.CopPed.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Cop.CopPed, EntityToCheck))//if (Cop.CopPed.PlayerIsInFront() && Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, RangeToCheck) && !Cop.CopPed.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Cop.CopPed, EntityToCheck)) //was 55f
            {
                Cop.UpdateContinuouslySeen();
                Cop.canSeePlayer = true;
                Cop.GameTimeLastSeenPlayer = Game.GameTime;
                Cop.PositionLastSeenPlayer = Game.LocalPlayer.Character.Position;
                PlayerLastSeenInVehicle = InstantAction.PlayerInVehicle;
                PlayerLastSeenHeading = Game.LocalPlayer.Character.Heading;
                PlayerLastSeenForwardVector = Game.LocalPlayer.Character.ForwardVector;
                SawPlayerThisCheck = true;
            }
            else
            {
                Cop.GameTimeContinuoslySeenPlayerSince = 0;
                Cop.canSeePlayer = false;
            }
            TotalEntityNativeLOSChecks++;
        }
        if (SawPlayerThisCheck)
            return;
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && x.CopPed.IsInHelicopter))
        {
            Cop.GameTimeLastLOSCheck = Game.GameTime;
            if (Cop.DistanceToPlayer <= 250f && !Cop.CopPed.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY", Cop.CopPed, EntityToCheck, 17)) //was 55f
            {
                Cop.UpdateContinuouslySeen();
                Cop.canSeePlayer = true;
                Cop.GameTimeLastSeenPlayer = Game.GameTime;
                Cop.PositionLastSeenPlayer = Game.LocalPlayer.Character.Position;
                PlayerLastSeenInVehicle = InstantAction.PlayerInVehicle;
                PlayerLastSeenHeading = Game.LocalPlayer.Character.Heading;
                PlayerLastSeenForwardVector = Game.LocalPlayer.Character.ForwardVector;
                break;//Only care if one of the people saw it as we wont be tasking them 
            }
            else
            {
                Cop.GameTimeContinuoslySeenPlayerSince = 0;
                Cop.canSeePlayer = false;
            }
        }
        //PlacePlayerLastSeen = UpdatePlacePlayerLastSeen();
    }
    private static bool PoliceCanSeeEntity(Entity EntityToCheck)
    {
        if (!EntityToCheck.Exists())
            return false;

        float RangeToCheck = 55f;

        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInHelicopter))
        {
            if (EntityToCheck.IsInFront(Cop.CopPed) && Cop.CopPed.IsInRangeOf(EntityToCheck.Position, RangeToCheck) && !Cop.CopPed.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Cop.CopPed, EntityToCheck)) //was 55f
            {
                return true;
            }
        }
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && x.CopPed.IsInHelicopter))
        {
            if (Cop.CopPed.IsInRangeOf(EntityToCheck.Position, 250f) && !Cop.CopPed.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY", Cop.CopPed, EntityToCheck, 17)) //was 55f
            {
                return true;
            }
        }
        return false;
    }
    private static void SetPrimaryPursuer()
    {
        if (PoliceScanning.CopPeds.Count == 0)
            return;
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInHelicopter))
        {
            Cop.isPursuitPrimary = false;
        }
        GTACop PursuitPrimary = PoliceScanning.CopPeds.Where(x => x.CopPed.Exists() && !x.CopPed.IsDead && !x.CopPed.IsInAnyVehicle(false)).OrderBy(x => x.CopPed.Position.DistanceTo2D(Game.LocalPlayer.Character.Position)).FirstOrDefault();
        if (PursuitPrimary == null)
        {
            PrimaryPursuer = null;
            return;
        }
        else
        {
            PrimaryPursuer = PursuitPrimary;
            PursuitPrimary.isPursuitPrimary = true;
        }
    }
    public static void IssueCopPistol(GTACop Cop)
    {
        GTAWeapon Pistol;
        Pistol = GTAWeapons.WeaponsList.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.Pistol).PickRandom();
        Cop.IssuedPistol = Pistol;
        Cop.CopPed.Inventory.GiveNewWeapon(Pistol.Name, Pistol.AmmoAmount, false);
        if (Settings.AllowPoliceWeaponVariations)
        {
            WeaponVariation MyVariation = Pistol.PoliceVariations.PickRandom();
            Cop.PistolVariation = MyVariation;
            InstantAction.ApplyWeaponVariation(Cop.CopPed, (uint)Pistol.Hash, MyVariation);
        }
    }
    public static void IssueCopHeavyWeapon(GTACop Cop)
    {
        GTAWeapon IssuedHeavy;
        int Num = rnd.Next(1, 5);
        if (Num == 1)
            IssuedHeavy = GTAWeapons.WeaponsList.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.AR).PickRandom();
        else if (Num == 2)
            IssuedHeavy = GTAWeapons.WeaponsList.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.Shotgun).PickRandom();
        else if (Num == 3)
            IssuedHeavy = GTAWeapons.WeaponsList.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.SMG).PickRandom();
        else if (Num == 4)
            IssuedHeavy = GTAWeapons.WeaponsList.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.AR).PickRandom();
        else
            IssuedHeavy = GTAWeapons.WeaponsList.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.AR).PickRandom();

        Cop.IssuedHeavyWeapon = IssuedHeavy;
        Cop.CopPed.Inventory.GiveNewWeapon(IssuedHeavy.Name, IssuedHeavy.AmmoAmount, true);
        if (Settings.OverridePoliceAccuracy)
            Cop.CopPed.Accuracy = Settings.PoliceHeavyAccuracy;
        if (Settings.AllowPoliceWeaponVariations)
        {
            WeaponVariation MyVariation = IssuedHeavy.PoliceVariations.PickRandom();
            Cop.HeavyVariation = MyVariation;
            InstantAction.ApplyWeaponVariation(Cop.CopPed, (uint)IssuedHeavy.Hash, MyVariation);
        }
    }
    private static void GetPoliceState()
    {
        if (CurrentPoliceState == PoliceState.ArrestedWait || CurrentPoliceState == PoliceState.DeadlyChase)
            return;

        if (InstantAction.PlayerWantedLevel == 0)
            CurrentPoliceState = PoliceState.Normal;//Default state
        else if (InstantAction.PlayerWantedLevel >= 1 && InstantAction.PlayerWantedLevel <= 3 && AnyPoliceCanSeePlayer)//AnyCanSeePlayer)
        {
            if ((!firedWeapon && !PlayerHurtPolice && !aimedAtPolice) && !InstantAction.PlayerIsConsideredArmed) // Unarmed and you havent killed anyone
                CurrentPoliceState = PoliceState.UnarmedChase;
            else if ((!firedWeapon && !PlayerHurtPolice && !aimedAtPolice))
                CurrentPoliceState = PoliceState.CautiousChase;
            else
                CurrentPoliceState = PoliceState.DeadlyChase;

        }
        else if (InstantAction.PlayerWantedLevel >= 4 || PlayerHurtPolice || PlayerKilledPolice)
            CurrentPoliceState = PoliceState.DeadlyChase;


        if (!firedWeapon && (Game.LocalPlayer.Character.IsShooting || PlayerArtificiallyShooting) && (PoliceScanning.CopPeds.Any(x => x.canSeePlayer || (x.DistanceToPlayer <= 100f && !Game.LocalPlayer.Character.IsCurrentWeaponSilenced)))) //if (!firedWeapon && Game.LocalPlayer.Character.IsShooting && (PoliceScanning.CopPeds.Any(x => x.canSeePlayer || x.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f))))
        {
            SetWantedLevel(2, "Fired a Weapon");
            firedWeapon = true;
        }

        if (InstantAction.PlayerWantedLevel < 2)
        {
            if (InstantAction.PlayerWantedLevel < 2 && LicensePlateChanging.PlayerChangingPlate && AnyPoliceCanSeePlayer)
            {
                SetWantedLevel(2,"Police saw you changing plates");
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspiciousActivity, 3));
            }

            if (InstantAction.PlayerWantedLevel < 2 && CarStealing.PlayerBreakingIntoCar && AnyPoliceCanSeePlayer)
            {
                SetWantedLevel(2,"Police saw you breaking into a car");
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportGrandTheftAuto, 3));
            }

        }

        if (InstantAction.PlayerWantedLevel < 3)
        {
            if (InstantAction.PlayerWantedLevel > 0 && PlayerLocation.PlayerCurrentZone == Zones.JAIL)
            {
                SetWantedLevel(3, "You went too close to the prison with a wanted level");
                CurrentPoliceState = PoliceState.DeadlyChase;
            }
            if (AnyPoliceCanRecognizePlayer && InstantAction.PlayerIsConsideredArmed && Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && !InstantAction.PlayerInVehicle)
            {
                ulong myHash = (ulong)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash;
                GTAWeapon MatchedWeapon = GTAWeapons.GetWeaponFromHash(myHash);//InstantAction.Weapons.Where(x => x.Hash == myHash).FirstOrDefault();
                bool ChangedWanted = false;
                if (MatchedWeapon != null)
                {
                    if (MatchedWeapon.WeaponLevel >= 3 && InstantAction.PlayerWantedLevel < 3)
                    {
                        ChangedWanted = true;
                        SetWantedLevel(3,"Cops saw you with a level 3 or higher gun");
                    }
                    else if (InstantAction.PlayerWantedLevel < 2)
                    {
                        ChangedWanted = true;
                        SetWantedLevel(2, "Cops saw you with a gun");
                    }
                }
                else if (InstantAction.PlayerWantedLevel < 2)
                {
                    ChangedWanted = true;
                    SetWantedLevel(2, "Cops saw you with a gun");
                }

                if (ChangedWanted)
                {
                    DispatchAudio.DispatchQueueItem CarryingWeapon = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportCarryingWeapon, 3)
                    {
                        WeaponToReport = MatchedWeapon
                    };
                    DispatchAudio.AddDispatchToQueue(CarryingWeapon);
                }
            }
            if (!aimedAtPolice && InstantAction.PlayerIsConsideredArmed && Game.LocalPlayer.IsFreeAiming && AnyPoliceCanSeePlayer && PoliceScanning.CopPeds.Any(x => Game.LocalPlayer.IsFreeAimingAtEntity(x.CopPed)))
            {
                TimeAimedAtPolice++;
            }
            else
            {
                TimeAimedAtPolice = 0;
            }
            if (TimeAimedAtPolice >= 100)
            {
                SetWantedLevel(3, "You aimed at the police too long");
                aimedAtPolice = true;
            }
        }
    }
    private static void CheckPoliceEvents()
    {
        if (PrevPlayerIsJacking != PlayerIsJacking)
            PlayerJackingChanged(PlayerIsJacking);

        if (PrevPlayerKilledPolice != PlayerKilledPolice)
            PlayerKilledPoliceChanged();

        if (PrevCopsKilledByPlayer != CopsKilledByPlayer)
            CopsKilledChanged();

        if (PrevCiviliansKilledByPlayer != CiviliansKilledByPlayer)
            CiviliansKilledChanged();

        if (PrevfiredWeapon != firedWeapon)
            FiredWeaponChanged();

        if (PrevaimedAtPolice != aimedAtPolice)
            AimedAtPoliceChanged();

        if (PrevPlayerHurtPolice != PlayerHurtPolice)
            PlayerHurtPoliceChanged();

        if (PrevPoliceState != CurrentPoliceState)
            PoliceStateChanged();

        if (PreviousWantedLevel != Game.LocalPlayer.WantedLevel)
            WantedLevelChanged();

        if (PrevPlayerStarsGreyedOut != PlayerStarsGreyedOut)
            PlayerStarsGreyedOutChanged();

        if (PrevPoliceState != CurrentPoliceState)
            PoliceStateChanged();
    }
    private static void TrackedVehiclesTick()
    {
        InstantAction.TrackedVehicles.RemoveAll(x => !x.VehicleEnt.Exists());
        if (InstantAction.PlayerWantedLevel == 0)
        {
            foreach (GTAVehicle StolenCar in InstantAction.TrackedVehicles.Where(x => x.ShouldReportStolen))
            {
                StolenCar.QuedeReportedStolen = true;
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportStolenVehicle, 10)
                {
                    ResultsInStolenCarSpotted = true,
                    VehicleToReport = StolenCar
                });
            }
        }
        if (InstantAction.PlayerInVehicle && Game.LocalPlayer.Character.IsInAnyVehicle(false))//first check is cheaper, but second is required to verify
        {
            Vehicle CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
            GTAVehicle CurrTrackedVehicle = InstantAction.TrackedVehicles.Where(x => x.VehicleEnt.Handle == CurrVehicle.Handle).FirstOrDefault();
            if (CurrTrackedVehicle == null)
                return;
            if (AnyPoliceCanRecognizePlayer)
            {
                if (InstantAction.PlayerWantedLevel > 0 && !PlayerStarsGreyedOut)
                    UpdateVehicleDescription(CurrTrackedVehicle);

                if (InstantAction.PlayerWantedLevel < 2 && CurrTrackedVehicle.WasReportedStolen && CurrTrackedVehicle.IsStolen && CurrTrackedVehicle.MatchesOriginalDescription)
                {
                    SetWantedLevel(2, "Car was reported stolen and it matches the original description (formerly First)");
                    DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSpottedStolenCar, 10)
                    {
                        ResultsInStolenCarSpotted = true,
                        VehicleToReport = CurrTrackedVehicle,
                        Speed = Game.LocalPlayer.Character.CurrentVehicle.Speed * 2.23694f
                    });
                }
                else if (InstantAction.PlayerWantedLevel == 0 && CurrTrackedVehicle.CarPlate.IsWanted && !CurrTrackedVehicle.IsStolen && CurrTrackedVehicle.ColorMatchesDescription)
                {
                    SetWantedLevel(2, "Car plate is wanted and color matches original (formerly Second)");
                    DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectSpotted, 10)
                    {
                        ResultsInStolenCarSpotted = true,
                        VehicleToReport = CurrTrackedVehicle
                    });
                }

                if (CurrTrackedVehicle.CarPlate != null && CurrTrackedVehicle.CarPlate.IsWanted && InstantAction.PlayerWantedLevel == 0 && PlayerHasBeenNotWantedFor >= 60000 && !CurrTrackedVehicle.IsStolen) //No longer care about the car
                {
                    CurrTrackedVehicle.CarPlate.IsWanted = false;
                    DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectLost, 10)
                    {
                        ResultsInStolenCarSpotted = true,
                        VehicleToReport = CurrTrackedVehicle
                    });
                }
            }
        }
    }
    private static void UpdateVehicleDescription(GTAVehicle MyVehicle)
    {
        if(MyVehicle.VehicleEnt.Exists())
            MyVehicle.DescriptionColor = MyVehicle.VehicleEnt.PrimaryColor;
        MyVehicle.CarPlate.IsWanted = true;
        if (MyVehicle.IsStolen && !MyVehicle.WasReportedStolen)
            MyVehicle.WasReportedStolen = true;
    }
    private static void WantedLevelTick()
    {
        if (InstantAction.PlayerWantedLevel > 0)
        {
            if (Settings.WantedLevelIncreasesOverTime && Game.GameTime - WantedLevelStartTime > Settings.WantedLevelIncreaseTime && WantedLevelStartTime > 0 && AnyPoliceRecentlySeenPlayer && InstantAction.PlayerWantedLevel > 0 && InstantAction.PlayerWantedLevel <= Settings.WantedLevelInceaseOverTimeLimit)
            {
                SetWantedLevel(InstantAction.PlayerWantedLevel + 1, "Wanted Level increased over time");
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportIncreasedWanted, 3)
                {
                    ResultsInLethalForce = Game.LocalPlayer.WantedLevel == 4
                });
            }

            //if (Settings.SpawnNewsChopper && Game.GameTime - WantedLevelStartTime > 180000 && WantedLevelStartTime > 0 && AnyPoliceRecentlySeenPlayer && InstantAction.PlayerWantedLevel > 4 && !PoliceScanning.Reporters.Any())
            //{
            //    PoliceSpawning.SpawnNewsChopper();
            //    LocalWriteToLog("WantedLevelTick", "Been at this wanted for a while, wanted news chopper spawned (if they dont already exist)");
            //}
            if (CanReportLastSeen && Game.GameTime - GameTimeLastGreyedOut > 15000 && AnyPoliceSeenPlayerThisWanted && PlayerHasBeenWantedFor > 45000 && !PoliceScanning.CopPeds.Any(x => x.DistanceToPlayer <= 150f))
            {
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectLastSeen, 10));
                CanReportLastSeen = false;
            }
        }
    }
    private static void PersonOfInterestTick()
    {
        Vector3 CurrentWantedCenter = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
        if (Game.LocalPlayer.WantedLevel > 0 && CurrentWantedCenter != Vector3.Zero)
        {
            LastWantedCenterPosition = CurrentWantedCenter;
            AddUpdateCurrentWantedBlip(CurrentWantedCenter);
        }

        if (InstantAction.PlayerWantedLevel == 0 && PlayerHasBeenNotWantedFor >= 120000)
        {
            PlayerIsPersonOfInterest = false;
            AddUpdateLastWantedBlip(Vector3.Zero);
            AddUpdateCurrentWantedBlip(Vector3.Zero);
        }
        if (InstantAction.PlayerWantedLevel == 0)
        {
            AddUpdateCurrentWantedBlip(Vector3.Zero);
        }

        if (PlayerIsPersonOfInterest && InstantAction.PlayerWantedLevel == 0 && AnyPoliceCanRecognizePlayerAfterWanted && PlayerHasBeenNotWantedFor >= 5000 && PlayerHasBeenNotWantedFor <= 120000)
        {
            SetWantedLevel(3, "Cops Reacquired after losing them");
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectSpotted, 3));
        }
        if (PlayerIsPersonOfInterest && InstantAction.PlayerWantedLevel == 0 && AnyPoliceCanSeePlayer && PlayerHasBeenNotWantedFor >= 5000 && PlayerHasBeenNotWantedFor <= 120000 && LastWantedCenterPosition != Vector3.Zero && Game.LocalPlayer.Character.DistanceTo2D(LastWantedCenterPosition) <= Settings.LastWantedCenterSize)
        {
            SetWantedLevel(3, "Cops Reacquired after losing them in the same area");
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectSpotted, 3));
        }
    }
    public static void ResetPoliceStats()
    {
        CopsKilledByPlayer = 0;
        CiviliansKilledByPlayer = 0;
        PlayerHurtPolice = false;
        PlayerKilledPolice = false;
        PlayerKilledCivilians = false;
        foreach (GTACop Cop in PoliceScanning.CopPeds)
        {
            Cop.HurtByPlayer = false;
        }
        aimedAtPolice = false;
        firedWeapon = false;
        DispatchAudio.ResetReportedItems();
    }
    public static void SetWantedLevel(int WantedLevel,string Reason)
    {
        GameTimeLastSetWanted = Game.GameTime;
        if (Game.LocalPlayer.WantedLevel != WantedLevel)
        {
            Game.LocalPlayer.WantedLevel = WantedLevel;
            LocalWriteToLog("SetWantedLevel", string.Format("Manually set to: {0}: {1}", WantedLevel, Reason));
        }
    }
    public static void CheckRecognition()//needs some optimization
    {
        AnyPoliceCanSeePlayer = PoliceScanning.CopPeds.Any(x => x.canSeePlayer);
        AnyPoliceRecentlySeenPlayer = PoliceScanning.CopPeds.Any(x => x.SeenPlayerSince(Settings.PoliceRecentlySeenTime));

        uint TimeToRecongize = 2000;
        CheckNight();

        if (IsNightTime)
            TimeToRecongize = 3500;
        else if (InstantAction.PlayerInVehicle)
            TimeToRecongize = 750;
        else
            TimeToRecongize = 2000;
        AnyPoliceCanRecognizePlayer = PoliceScanning.CopPeds.Any(x => x.HasSeenPlayerFor >= TimeToRecongize || (x.canSeePlayer && x.DistanceToPlayer <= 20f) || (x.DistanceToPlayer <= 7f && x.DistanceToPlayer > 0f));

        if (InstantAction.PlayerWantedLevel == 0)
        {
            if (InstantAction.PlayerInVehicle)
                TimeToRecongize = 6000;
            else
                TimeToRecongize = 4000;

            if (IsNightTime)
                TimeToRecongize += 6000;

            AnyPoliceCanRecognizePlayerAfterWanted = PoliceScanning.CopPeds.Any(x => x.HasSeenPlayerFor >= TimeToRecongize || (x.canSeePlayer && x.DistanceToPlayer <= 12f) || (x.DistanceToPlayer <= 7f && x.DistanceToPlayer > 0f));
        }

        if (PrevAnyCanRecognizePlayer != AnyPoliceCanRecognizePlayer)
        {
            PrevAnyCanRecognizePlayer = AnyPoliceCanRecognizePlayer;
        }
      //  Vector3 DesiredWantedLocation;
        if (!AnyPoliceSeenPlayerThisWanted)
        {
            PlacePlayerLastSeen = PlaceWantedStarted;
            //DesiredWantedLocation = PlaceWantedStarted;
        }
        else if (AnyPoliceRecentlySeenPlayer || !PlayerStarsGreyedOut)//was &&
        {
            PlacePlayerLastSeen = Game.LocalPlayer.Character.Position;
            //DesiredWantedLocation = Game.LocalPlayer.Character.Position;
        }
        else if(PlayerStarsGreyedOut && Game.LocalPlayer.Character.DistanceTo2D(PlacePlayerLastSeen) <= 15f && !Game.LocalPlayer.Character.Position.Z.IsWithin(PlacePlayerLastSeen.Z-2f,PlacePlayerLastSeen.Z+2f))//within the search zone but above or below it, need to help out my cops
        {
            SearchModeStopping.StopSearchModeSingle();
            //SearchModeStopping.StopSearchMode = true;
        }
        NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, PlacePlayerLastSeen.X, PlacePlayerLastSeen.Y, PlacePlayerLastSeen.Z);

        if (!AnyPoliceSeenPlayerThisWanted && AnyPoliceRecentlySeenPlayer)
            AnyPoliceSeenPlayerThisWanted = true;
    }
    private static void CheckNight()
    {
        IsNightTime = false;
        int HourOfDay = NativeFunction.CallByName<int>("GET_CLOCK_HOURS");
        int MinuteOfDay = NativeFunction.CallByName<int>("GET_CLOCK_MINUTES"); 
        if (HourOfDay >= 20 || (HourOfDay >= 19 && MinuteOfDay >= 30) || HourOfDay <= 5)
            IsNightTime = true;
    }
    public static void ResetPersonOfInterest()
    {
        PlayerIsPersonOfInterest = false;
        LastWantedCenterPosition = Vector3.Zero;
        AddUpdateCurrentWantedBlip(Vector3.Zero);
        AddUpdateLastWantedBlip(Vector3.Zero);
    }
    private static void WantedLevelChanged()
    {
        if (Game.LocalPlayer.WantedLevel == 0)//Just Removed
        {
            if (AnyPoliceSeenPlayerThisWanted && PreviousWantedLevel != 0)//maxwantedlastlife
            {
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectLost, 5));
            }
            if(PlayerIsPersonOfInterest)
            {
                AddUpdateLastWantedBlip(LastWantedCenterPosition);
            }
            CurrentPoliceState = PoliceState.Normal;
            AnyPoliceSeenPlayerThisWanted = false;
            TrafficViolations.ResetTrafficViolations();
            WantedLevelStartTime = 0;
            GameTimeWantedStarted = 0;
            DispatchAudio.ResetReportedItems();
            GameTimeLastWantedEnded = Game.GameTime;


            Tasking.UntaskAll(false);
            Tasking.RetaskAllRandomSpawns();


            foreach(Blip myBlip in TempBlips)
            {
                if (myBlip.Exists())
                    myBlip.Delete();
            }
            AddUpdateCurrentWantedBlip(Vector3.Zero);
        }
        else
        {
            GameTimeLastWantedEnded = 0;
        }
        if (PreviousWantedLevel == 0 && Game.LocalPlayer.WantedLevel > 0)
        {
            if (Game.LocalPlayer.WantedLevel <= 2)
            {
                if (!RecentlySetWanted)
                {
                    //if (!AnyPoliceRecentlySeenPlayer && !World.GetEntities(Game.LocalPlayer.Character.Position, 25f, GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed).Any(x => x.IsAlive))
                    //{
                    //    SetWantedLevel(0, "Reset a fake wanted level as there is nobody within 25 meters and no police have recently seen you");
                    //    AddUpdateLastWantedBlip(Vector3.Zero);
                    //    return;
                    //}
                    if (PedSwapping.JustTakenOver(10000))
                    {
                        SetWantedLevel(0, "Reset a fake wanted level as you just took over this ped");
                        return;
                    }
                    else
                    {
                        AddDispatchToUnknownWanted();
                    }
                }
            }

            GameTimeWantedStarted = Game.GameTime;
            PlaceWantedStarted = Game.LocalPlayer.Character.Position;

            AddUpdateLastWantedBlip(Vector3.Zero);

            Tasking.UntaskAllRandomSpawns(false);
            PlayerIsPersonOfInterest = true;

            Game.LocalPlayer.Character.PlayAmbientSpeech("GENERIC_CURSE");
            if (InstantAction.PlayerInVehicle)
                VehicleEngine.WantedLevelTune = true;
        }
        WantedLevelStartTime = Game.GameTime;
        LocalWriteToLog("ValueChecker", String.Format("WantedLevel Changed to: {0}", Game.LocalPlayer.WantedLevel));
        PreviousWantedLevel = Game.LocalPlayer.WantedLevel;
    }
    public static void AddDispatchToUnknownWanted()//temp public
    {
        LocalWriteToLog("AddDispatchToUnknownWanted", "Got wanted without being manually set");
        if (InstantAction.PlayerRecentlyShot)
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportLowLevelShotsFired, 20) { IsAmbient = true });
        else if (CarStealing.PlayerBreakingIntoCar)
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportLowLevelGrandTheftAuto, 20) { IsAmbient = true });
        else
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportLowLevelCriminalActivity, 20) { IsAmbient = true });
    }
    private static void CopsKilledChanged()
    {
        LocalWriteToLog("ValueChecker", string.Format("CopsKilledByPlayer Changed to: {0}", CopsKilledByPlayer));
        PrevCopsKilledByPlayer = CopsKilledByPlayer;
    }
    private static void CiviliansKilledChanged()
    {
        LocalWriteToLog("ValueChecker", String.Format("CiviliansKilledChanged Changed to: {0}", CiviliansKilledByPlayer));
        PrevCiviliansKilledByPlayer = CiviliansKilledByPlayer;
    }
    private static void PlayerHurtPoliceChanged()
    {
        LocalWriteToLog("ValueChecker", String.Format("PlayerHurtPolice Changed to: {0}", PlayerHurtPolice));
        if (PlayerHurtPolice)
        {
            //DispatchAudioSystem.ReportAssualtOnOfficer();
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportAssualtOnOfficer, 3));
        }

        PrevPlayerHurtPolice = PlayerHurtPolice;
    }
    private static void PlayerKilledPoliceChanged()
    {
        LocalWriteToLog("ValueChecker", String.Format("PlayerKilledPolice Changed to: {0}", PlayerKilledPolice));
        if (PlayerKilledPolice)
        {
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportOfficerDown, 1) { ResultsInLethalForce = true});
        }
        PrevPlayerKilledPolice = PlayerKilledPolice;
    }
    private static void FiredWeaponChanged()
    {
        LocalWriteToLog("ValueChecker", String.Format("firedWeapon Changed to: {0}", firedWeapon));
        if (firedWeapon)
        {
            //DispatchAudioSystem.ReportShotsFired();
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportShotsFired, 2) { ResultsInLethalForce = true });
        }
        PrevfiredWeapon = firedWeapon;
    }
    private static void AimedAtPoliceChanged()
    {
        if (aimedAtPolice)
        {
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportThreateningWithFirearm, 2) { ResultsInLethalForce = true });
        }
        PrevaimedAtPolice = aimedAtPolice;
    }
    private static void PlayerStarsGreyedOutChanged()
    {
        LocalWriteToLog("ValueChecker", String.Format("PlayerStarsGreyedOut Changed to: {0}", PlayerStarsGreyedOut));
        if (PlayerStarsGreyedOut)
        {
            CanReportLastSeen = true;
            GameTimeLastGreyedOut = Game.GameTime;
        }
        else
        {
            foreach (GTACop Cop in PoliceScanning.CopPeds)
            {
                Cop.AtWantedCenterDuringSearchMode = false;
                if (Cop.isTasked && (Cop.TaskType == PoliceTask.Task.GoToWantedCenter || Cop.TaskType == PoliceTask.Task.SimpleInvestigate || Cop.TaskType == PoliceTask.Task.RandomSpawnIdle))
                {
                    Tasking.AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
                }
            }
            CanReportLastSeen = false;
            if (AnyPoliceSeenPlayerThisWanted && CanPlaySuspectSpotted && InstantAction.PlayerInVehicle)
            {
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportLocalSuspectSpotted, 20) { IsAmbient = true });
                GameTimeLastReportedSpotted = Game.GameTime;
            }
        }
        PrevPlayerStarsGreyedOut = PlayerStarsGreyedOut;
    }
    private static void PoliceStateChanged()
    {
        LocalWriteToLog("ValueChecker", String.Format("PoliceState Changed to: {0}", CurrentPoliceState));
        LocalWriteToLog("ValueChecker", String.Format("PreviousPoliceState Changed to: {0}", PrevPoliceState));
        LastPoliceState = PrevPoliceState;
        if (CurrentPoliceState == PoliceState.Normal && !InstantAction.IsDead)
        {
            ResetPoliceStats();
        }
        if (CurrentPoliceState == PoliceState.DeadlyChase)
        {
            if (PrevPoliceState != PoliceState.ArrestedWait)
            {
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportLethalForceAuthorized, 1) { ResultsInLethalForce = true });
            }
        }
        GameTimePoliceStateStart = Game.GameTime;
        PrevPoliceState = CurrentPoliceState;
    }
    private static void PlayerJackingChanged(bool isJacking)
    {
        PlayerIsJacking = isJacking;
        LocalWriteToLog("ValueChecker", String.Format("PlayerIsJacking Changed to: {0}", PlayerIsJacking));
        if (PlayerIsJacking)
        {
            GameTimeLastStartedJacking = Game.GameTime;
        }
        PrevPlayerIsJacking = PlayerIsJacking;
    }
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
            LastWantedCenterBlip = new Blip(LastWantedCenterPosition, Settings.LastWantedCenterSize)
            {
                Name = "Last Wanted Center Position",
                Color = Color.Yellow,
                Alpha = 0.5f
            };

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
            CurrentWantedCenterBlip = new Blip(Position, 100f)
            {
                Name = "Current Wanted Center Position",
                Color = Color.Red,
                Alpha = 0.5f
            };

            NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)CurrentWantedCenterBlip.Handle, true);
            CreatedBlips.Add(CurrentWantedCenterBlip);
        }
        if (CurrentWantedCenterBlip.Exists())
            CurrentWantedCenterBlip.Position = Position;
    }
    public static void RemoveWantedBlips()
    {
        LastWantedCenterPosition = Vector3.Zero;
        if (LastWantedCenterBlip.Exists())
            LastWantedCenterBlip.Delete();
        if (CurrentWantedCenterBlip.Exists())
            CurrentWantedCenterBlip.Delete();
    }
    private static void LocalWriteToLog(string ProcedureString, string TextToLog)
    {
        if (Settings.PoliceLogging)
            Debugging.WriteToLog(ProcedureString, TextToLog);
    }
    public static void StopWantedTemporarily(int TimeToStop)
    {
        GameFiber StopWantedLevel = GameFiber.StartNew(delegate
        {
            uint GameTimeStarted = Game.GameTime;
            while (!RecentlySetWanted && Game.GameTime - GameTimeStarted < TimeToStop)
            {
                Game.LocalPlayer.WantedLevel = 0;
                GameFiber.Yield();
            }

        }, "StopWantedLevel");
        Debugging.GameFibers.Add(StopWantedLevel);
    }
    public static void RemoveBlip(Ped MyPed)
    {
        Blip MyBlip = MyPed.GetAttachedBlip();
        if (MyBlip.Exists())
            MyBlip.Delete();
    }
    public static void DeleteCop(GTACop Cop)
    {
        if (!Cop.CopPed.Exists())
            return;
        if (Cop.CopPed.IsInAnyVehicle(false))
        {
            if (Cop.CopPed.CurrentVehicle.HasPassengers)
            {
                foreach (Ped Passenger in Cop.CopPed.CurrentVehicle.Passengers)
                {
                    RemoveBlip(Passenger);
                    Passenger.Delete();
                }
            }
            if(Cop.CopPed.CurrentVehicle.Exists())
                Cop.CopPed.CurrentVehicle.Delete();
        }
        RemoveBlip(Cop.CopPed);
        Cop.CopPed.Delete();
        Cop.WasMarkedNonPersistent = false;
        LocalWriteToLog("SpawnCop", string.Format("Cop Deleted: Handled {0}", Cop.CopPed.Handle));
    }
    public static void MarkNonPersistent(GTACop Cop)
    {
        if (!Cop.CopPed.Exists())
            return;
        if (Cop.CopPed.IsInAnyVehicle(false))
        {
            if (Cop.CopPed.CurrentVehicle.HasPassengers)
            {
                foreach (Ped Passenger in Cop.CopPed.CurrentVehicle.Passengers)
                {
                    GTACop PassengerCop = PoliceScanning.CopPeds.Where(x => x.CopPed.Handle == Passenger.Handle).FirstOrDefault();
                    if(PassengerCop != null)
                    {
                        PassengerCop.CopPed.IsPersistent = false;
                        PassengerCop.WasMarkedNonPersistent = false;
                    }
                }
            }
            Cop.CopPed.CurrentVehicle.IsPersistent = false;
        }
        Cop.CopPed.IsPersistent = false;
        Cop.WasMarkedNonPersistent = false;
        LocalWriteToLog("SpawnCop", string.Format("CopMarkedNonPersistant: Handled {0}", Cop.CopPed.Handle));
    }

}

