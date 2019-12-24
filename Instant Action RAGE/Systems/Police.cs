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
    private static int TimeAimedAtPolice;

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
    private static PoliceState PrevPoliceState;
    public static List<Blip> CreatedBlips = new List<Blip>();
    public static List<Blip> TempBlips = new List<Blip>();
    private static uint GameTimeLastReportedSpotted;
    private static WantedLevelStats PreviousWantedStats;
    private static uint GameTimeLastReacquired;
    private static bool ApplyPreviousWantedOnSight;
    private static bool PrevPlayerWentNearPrisonDuringChase;
    private static bool PrevPlayerCaughtWithGun;
    private static bool PrevPlayerCaughtChangingPlates;
    private static bool PrevPlayerCaughtBreakingIntoCar;

    //public static bool PlayerRecentlyReacquired
    //{
    //    get
    //    {
    //        if (GameTimeLastReacquired == 0)
    //            return false;
    //        else if (GameTimeLastReacquired - Game.GameTime <= 15000)
    //            return true;
    //        else
    //            return false;
    //    }
    //}
    public static bool PlayerFiredWeaponNearPolice { get; set; }
    public static bool PlayerAimedAtPolice { get; set; }
    public static bool AnyPoliceCanSeePlayer { get; set; }
    public static bool AnyPoliceCanRecognizePlayer { get; set; }
    public static bool AnyPoliceRecentlySeenPlayer { get; set; }
    public static PoliceState CurrentPoliceState { get; set; }
    public static bool PlayerStarsGreyedOut { get; set; }
    public static bool AnyPoliceSeenPlayerThisWanted { get; set; }
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
    public static bool PlayerCaughtWithGun { get; set; }
    public static bool PlayerCaughtChangingPlates { get; set; }
    public static bool PlayerCaughtBreakingIntoCar { get; set; }
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
    public static bool PlayerWentNearPrisonDuringChase { get; set; }

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
        PlayerFiredWeaponNearPolice = false;
        PlayerAimedAtPolice = false;
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
        PrevPoliceState = PoliceState.Normal;
        CreatedBlips = new List<Blip>();
        TempBlips = new List<Blip>();
        AnyPoliceCanSeePlayer = false;
        AnyPoliceCanRecognizePlayer  = false;
        AnyPoliceRecentlySeenPlayer = false;
        CurrentPoliceState = PoliceState.Normal;
        PlayerStarsGreyedOut = false;
        AnyPoliceSeenPlayerThisWanted = false;
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
        GameTimeLastReacquired = 0;
        ApplyPreviousWantedOnSight = false;
        PlayerWentNearPrisonDuringChase = false;
        PrevPlayerWentNearPrisonDuringChase = false;
        PrevPlayerCaughtWithGun = false;
        PlayerCaughtWithGun = false;
        PrevPlayerCaughtChangingPlates = false;
        PlayerCaughtChangingPlates = false;
        PrevPlayerCaughtBreakingIntoCar = false;
        PlayerCaughtBreakingIntoCar = false;
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
        CheckCrimes();
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
                }
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
            bool IsDeadly = (PlayerFiredWeaponNearPolice || PlayerHurtPolice || PlayerAimedAtPolice || PlayerWentNearPrisonDuringChase || PlayerKilledPolice);
            if (!IsDeadly && !InstantAction.PlayerIsConsideredArmed) // Unarmed and you havent killed anyone
                CurrentPoliceState = PoliceState.UnarmedChase;
            else if (!IsDeadly)
                CurrentPoliceState = PoliceState.CautiousChase;
            else
                CurrentPoliceState = PoliceState.DeadlyChase;
        }
        else if (InstantAction.PlayerWantedLevel >= 4)
            CurrentPoliceState = PoliceState.DeadlyChase;
    }
    private static void CheckCrimes()
    {
        if (CurrentPoliceState == PoliceState.ArrestedWait || CurrentPoliceState == PoliceState.DeadlyChase)
            return;

        if (!PlayerFiredWeaponNearPolice && (Game.LocalPlayer.Character.IsShooting || PlayerArtificiallyShooting) && (PoliceScanning.CopPeds.Any(x => x.canSeePlayer || (x.DistanceToPlayer <= 20f && !Game.LocalPlayer.Character.IsCurrentWeaponSilenced)))) //if (!firedWeapon && Game.LocalPlayer.Character.IsShooting && (PoliceScanning.CopPeds.Any(x => x.canSeePlayer || x.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f))))
            PlayerFiredWeaponNearPolice = true;

        if (!PlayerWentNearPrisonDuringChase && InstantAction.PlayerWantedLevel > 0 && PlayerLocation.PlayerCurrentZone == Zones.JAIL && AnyPoliceCanSeePlayer)
            PlayerWentNearPrisonDuringChase = true;

        if (!PlayerAimedAtPolice && InstantAction.PlayerIsConsideredArmed && Game.LocalPlayer.IsFreeAiming && AnyPoliceCanSeePlayer && PoliceScanning.CopPeds.Any(x => Game.LocalPlayer.IsFreeAimingAtEntity(x.CopPed)))
            TimeAimedAtPolice++;
        else
            TimeAimedAtPolice = 0;

        if (!PlayerAimedAtPolice && TimeAimedAtPolice >= 100)
            PlayerAimedAtPolice = true;

        if (!PlayerCaughtWithGun && AnyPoliceCanSeePlayer && InstantAction.PlayerIsConsideredArmed && Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && !InstantAction.PlayerInVehicle)
            PlayerCaughtWithGun = true;

        if (!PlayerCaughtChangingPlates && LicensePlateChanging.PlayerChangingPlate && AnyPoliceCanSeePlayer)
            PlayerCaughtChangingPlates = true;

        if (!PlayerCaughtBreakingIntoCar && CarStealing.PlayerBreakingIntoCar && AnyPoliceCanSeePlayer)
            PlayerCaughtBreakingIntoCar = true;

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

        if (PrevfiredWeapon != PlayerFiredWeaponNearPolice)
            FiredWeaponChanged();

        if (PrevaimedAtPolice != PlayerAimedAtPolice)
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

        if (PrevPlayerWentNearPrisonDuringChase != PlayerWentNearPrisonDuringChase)
            PlayerWentNearPrisonDuringChaseChanged();

        if (PrevPlayerCaughtWithGun != PlayerCaughtWithGun)
            PlayerCaughtWithGunChanged();

        if (PrevPlayerCaughtChangingPlates != PlayerCaughtChangingPlates)
            PlayerCaughtChangingPlatesChanged();

        if (PrevPlayerCaughtBreakingIntoCar != PlayerCaughtBreakingIntoCar)
            PlayerCaughtBreakingIntoCarChanged();
    }
    private static void PlayerCaughtBreakingIntoCarChanged()
    {
        if(PlayerCaughtBreakingIntoCar)
        {
            SetWantedLevel(2, "Police saw you breaking into a car");
            if (InstantAction.PlayerWantedLevel <= 2)
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportGrandTheftAuto, 3));
        }
        PrevPlayerCaughtBreakingIntoCar = PlayerCaughtBreakingIntoCar;
    }
    private static void CopsKilledChanged()
    {
        PrevCopsKilledByPlayer = CopsKilledByPlayer;
    }
    private static void CiviliansKilledChanged()
    {
        PrevCiviliansKilledByPlayer = CiviliansKilledByPlayer;
    }
    private static void PlayerHurtPoliceChanged()
    {
        LocalWriteToLog("ValueChecker", String.Format("PlayerHurtPolice Changed to: {0}", PlayerHurtPolice));
        if (PlayerHurtPolice)
        {
            SetWantedLevel(3, "You hurt a police officer");
            if (InstantAction.PlayerWantedLevel <= 3)
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportAssualtOnOfficer, 3));
        }
        PrevPlayerHurtPolice = PlayerHurtPolice;
    }
    private static void PlayerKilledPoliceChanged()
    {
        LocalWriteToLog("ValueChecker", String.Format("PlayerKilledPolice Changed to: {0}", PlayerKilledPolice));
        if (PlayerKilledPolice)
        {
            SetWantedLevel(3, "You killed a police officer");
            if (InstantAction.PlayerWantedLevel <= 3)
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportOfficerDown, 1) { ResultsInLethalForce = true });
        }
        PrevPlayerKilledPolice = PlayerKilledPolice;
    }
    private static void FiredWeaponChanged()
    {
        LocalWriteToLog("ValueChecker", String.Format("firedWeapon Changed to: {0}", PlayerFiredWeaponNearPolice));
        if (PlayerFiredWeaponNearPolice)
        {
            SetWantedLevel(3, "You fired a weapon at the police");
            if (InstantAction.PlayerWantedLevel <= 3)
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportShotsFired, 2) { ResultsInLethalForce = true });
        }
        PrevfiredWeapon = PlayerFiredWeaponNearPolice;
    }
    private static void AimedAtPoliceChanged()
    {
        if (PlayerAimedAtPolice)
        {
            SetWantedLevel(3, "You aimed at the police too long");
            if (InstantAction.PlayerWantedLevel <= 3)
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportThreateningWithFirearm, 2) { ResultsInLethalForce = true });
        }
        PrevaimedAtPolice = PlayerAimedAtPolice;
    }
    private static void PlayerCaughtChangingPlatesChanged()
    {
        if(PlayerCaughtChangingPlates)
        {
            SetWantedLevel(2, "Police saw you changing plates");
            if(InstantAction.PlayerWantedLevel <= 2)
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspiciousActivity, 3));
        }
        PrevPlayerCaughtChangingPlates = PlayerCaughtChangingPlates;
    }

    private static void PlayerCaughtWithGunChanged()
    {
        if (PlayerCaughtWithGun)
        {
            if (Game.LocalPlayer.Character.Inventory.EquippedWeapon == null)
                return;
            GTAWeapon MatchedWeapon = GTAWeapons.GetWeaponFromHash((ulong)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash);
            int DesiredWantedLevel = 2;
            if (MatchedWeapon != null && MatchedWeapon.WeaponLevel >= 2)
                DesiredWantedLevel = MatchedWeapon.WeaponLevel;
            SetWantedLevel(DesiredWantedLevel, "Cops saw you with a gun");
            if (InstantAction.PlayerWantedLevel <= DesiredWantedLevel)
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportCarryingWeapon, 3)
                {
                    WeaponToReport = MatchedWeapon
                });
        }
        PrevPlayerCaughtWithGun = PlayerCaughtWithGun;
    }
    private static void PlayerWentNearPrisonDuringChaseChanged()
    {
        if(PlayerWentNearPrisonDuringChase)
        {
            SetWantedLevel(3, "You went too close to the prison with a wanted level");
            if (InstantAction.PlayerWantedLevel <= 3)
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportTrespassingOnGovernmentProperty, 3)
                {
                    ResultsInLethalForce = true
                });
        }
        PrevPlayerWentNearPrisonDuringChase = PlayerWentNearPrisonDuringChase;
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
            uint WantedLevelInceaseTime = Settings.WantedLevelIncreaseTime;
            if (InstantAction.PlayerWantedLevel >= 3)
            {
                WantedLevelInceaseTime = 2 * WantedLevelInceaseTime;
            }
            if (Settings.WantedLevelIncreasesOverTime && Game.GameTime - WantedLevelStartTime > WantedLevelInceaseTime && WantedLevelStartTime > 0 && AnyPoliceRecentlySeenPlayer && InstantAction.PlayerWantedLevel > 0 && InstantAction.PlayerWantedLevel <= Settings.WantedLevelInceaseOverTimeLimit)
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
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectLostVisual, 10));
                CanReportLastSeen = false;
            }
            if (CurrentPoliceState == PoliceState.DeadlyChase && InstantAction.PlayerWantedLevel < 3)
            {
                SetWantedLevel(3, "Deadly chase requires 3+ wanted level");
            }

        }
    }
    private static void PersonOfInterestTick()
    {
        if (!PlayerIsPersonOfInterest && InstantAction.PlayerWantedLevel > 0 && AnyPoliceCanSeePlayer)
        {
            PlayerIsPersonOfInterest = true;
        }

        if (ApplyPreviousWantedOnSight && PlayerIsPersonOfInterest && AnyPoliceCanSeePlayer)
        {
            ApplyPreviousWantedOnSight = false;
            if (PreviousWantedStats != null)
            {
                PreviousWantedStats.ReplaceValues();
                DispatchAudio.ClearDispatchQueue();
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectSpotted, 1));
                LocalWriteToLog("PersonOfInterestTick", string.Format("Cops saw you and you had history, applying old stuff: ApplyPreviousWantedOnSight: {0}", ApplyPreviousWantedOnSight));
            }
        }

        Vector3 CurrentWantedCenter = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
        if (Game.LocalPlayer.WantedLevel > 0 && CurrentWantedCenter != Vector3.Zero)
        {
            LastWantedCenterPosition = CurrentWantedCenter;
            AddUpdateCurrentWantedBlip(CurrentWantedCenter);
        }

        if (PlayerIsPersonOfInterest && InstantAction.PlayerWantedLevel == 0 && PlayerHasBeenNotWantedFor >= 120000)
        {
            PlayerIsPersonOfInterest = false;
            AddUpdateLastWantedBlip(Vector3.Zero);
            AddUpdateCurrentWantedBlip(Vector3.Zero);
            PreviousWantedStats.ClearValues();
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportPersonOfInterestExpire, 3));
        }
        if (InstantAction.PlayerWantedLevel == 0)
        {
            AddUpdateCurrentWantedBlip(Vector3.Zero);
        }
        if (PlayerIsPersonOfInterest && InstantAction.PlayerWantedLevel == 0 && PlayerHasBeenNotWantedFor >= 5000 && PlayerHasBeenNotWantedFor <= 120000)
        {
            //if (AnyPoliceCanRecognizePlayerAfterWanted)
            //{
            //    SetWantedLevel(3, "Cops Reacquired after losing them");
            //    DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectSpotted, 3));
            //}
            if (AnyPoliceCanSeePlayer && LastWantedCenterPosition != Vector3.Zero && Game.LocalPlayer.Character.DistanceTo2D(LastWantedCenterPosition) <= Settings.LastWantedCenterSize)
            {
                int WantedLevel = 2;
                if (PreviousWantedStats != null && PreviousWantedStats.MaxWantedLevel > 0)
                    WantedLevel = PreviousWantedStats.MaxWantedLevel;
                SetWantedLevel(WantedLevel, "Cops Reacquired after losing them in the same area");
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectSpotted, 3));
            }
        }
    }
    public static void ResetPoliceStats()
    {
        PreviousWantedStats = new WantedLevelStats();
        CopsKilledByPlayer = 0;
        CiviliansKilledByPlayer = 0;
        PlayerHurtPolice = false;
        PlayerKilledPolice = false;
        PlayerKilledCivilians = false;
        foreach (GTACop Cop in PoliceScanning.CopPeds)
        {
            Cop.HurtByPlayer = false;
        }
        PlayerAimedAtPolice = false;
        PlayerFiredWeaponNearPolice = false;
        PlayerWentNearPrisonDuringChase = false;
        PlayerCaughtBreakingIntoCar = false;
        PlayerCaughtChangingPlates = false;
        PlayerWentNearPrisonDuringChase = false;
        PlayerCaughtWithGun = false;

        DispatchAudio.ResetReportedItems();
    }
    public static void SetWantedLevel(int WantedLevel,string Reason)
    {
        GameTimeLastSetWanted = Game.GameTime;
        if (Game.LocalPlayer.WantedLevel < WantedLevel || WantedLevel == 0)
        {
            LocalWriteToLog("SetWantedLevel", string.Format("Current Wanted: {0}, Desired Wanted: {1}", Game.LocalPlayer.WantedLevel, WantedLevel));
            Game.LocalPlayer.WantedLevel = WantedLevel;
            LocalWriteToLog("SetWantedLevel", string.Format("Manually set to: {0}: {1}", WantedLevel, Reason));
        }
        InstantAction.PlayerWantedLevel = Game.LocalPlayer.WantedLevel;
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

        if (!AnyPoliceSeenPlayerThisWanted)
        {
            PlacePlayerLastSeen = PlaceWantedStarted;
        }
        else if (AnyPoliceRecentlySeenPlayer || !PlayerStarsGreyedOut)//was &&
        {
            PlacePlayerLastSeen = Game.LocalPlayer.Character.Position;
        }
        else if(PlayerStarsGreyedOut && Game.LocalPlayer.Character.DistanceTo2D(PlacePlayerLastSeen) <= 15f && !Game.LocalPlayer.Character.Position.Z.IsWithin(PlacePlayerLastSeen.Z-2f,PlacePlayerLastSeen.Z+2f))//within the search zone but above or below it, need to help out my cops
        {
            SearchModeStopping.StopSearchModeSingle();
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
        PreviousWantedStats = null;
        LastWantedCenterPosition = Vector3.Zero;
        AddUpdateCurrentWantedBlip(Vector3.Zero);
        AddUpdateLastWantedBlip(Vector3.Zero);
    }
    private static void WantedLevelChanged()
    {
        if (Game.LocalPlayer.WantedLevel == 0)
            WantedLevelRemoved();
        else
            GameTimeLastWantedEnded = 0;

        if (PreviousWantedLevel == 0 && Game.LocalPlayer.WantedLevel > 0)
            WantedLevelAdded();

        WantedLevelStartTime = Game.GameTime;
        LocalWriteToLog("ValueChecker", String.Format("WantedLevel Changed to: {0}", Game.LocalPlayer.WantedLevel));
        PreviousWantedLevel = Game.LocalPlayer.WantedLevel;
    }
    private static void WantedLevelRemoved()
    {
        if (AnyPoliceSeenPlayerThisWanted && PreviousWantedLevel != 0 && !RecentlySetWanted)//i didnt make it go to zero, the chase was lost organically
        {
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectLost, 5));
        }
        if (PlayerIsPersonOfInterest)
        {
            AddUpdateLastWantedBlip(LastWantedCenterPosition);
        }
        ResetPoliceStats();
        CurrentPoliceState = PoliceState.Normal;
        AnyPoliceSeenPlayerThisWanted = false;
        TrafficViolations.ResetTrafficViolations();
        WantedLevelStartTime = 0;
        GameTimeWantedStarted = 0;
        DispatchAudio.ResetReportedItems();
        GameTimeLastWantedEnded = Game.GameTime;
        

        Tasking.UntaskAll(false);
        Tasking.RetaskAllRandomSpawns();


        foreach (Blip myBlip in TempBlips)
        {
            if (myBlip.Exists())
                myBlip.Delete();
        }
        AddUpdateCurrentWantedBlip(Vector3.Zero);
    }
    private static void WantedLevelAdded()
    {
        if (!RecentlySetWanted)
        {
            AddDispatchToUnknownWanted();
        }
        
        if (PlayerIsPersonOfInterest && PreviousWantedStats != null)
            ApplyPreviousWantedOnSight = true;
        else
            ApplyPreviousWantedOnSight = false;

        GameTimeWantedStarted = Game.GameTime;
        PlaceWantedStarted = Game.LocalPlayer.Character.Position;
        AddUpdateLastWantedBlip(Vector3.Zero);
        Tasking.UntaskAllRandomSpawns(false);
    }
    public static void AddDispatchToUnknownWanted()//temp public
    {
        LocalWriteToLog("AddDispatchToUnknownWanted", "Got wanted without being manually set");
        GTAWeapon MyGun = InstantAction.GetCurrentWeapon();
        if (InstantAction.PlayerIsConsideredArmed && MyGun != null && MyGun.WeaponLevel >= 4)
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportLowLevelTerroristActivity, 20) { IsAmbient = true });
        else if (InstantAction.PlayerRecentlyShot)
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportLowLevelShotsFired, 20) { IsAmbient = true });
        else if (CarStealing.PlayerBreakingIntoCar)
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportLowLevelGrandTheftAuto, 20) { IsAmbient = true });
        else
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportLowLevelCriminalActivity, 20) { IsAmbient = true });
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
            if (AnyPoliceSeenPlayerThisWanted && CanPlaySuspectSpotted && InstantAction.PlayerInVehicle && !DispatchAudio.IsPlayingAudio)
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
        //LocalWriteToLog("SpawnCop", string.Format("Cop Deleted: Handled {0}", Cop.CopPed.Handle));
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
        //LocalWriteToLog("SpawnCop", string.Format("CopMarkedNonPersistant: Handled {0}", Cop.CopPed.Handle));
    }

}

