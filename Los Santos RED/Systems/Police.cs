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
    public static RapSheet CurrentCrimes;

    private static uint WantedLevelStartTime;
    private static bool CanReportLastSeen;
    private static uint GameTimeLastGreyedOut;
    private static Vector3 PlaceWantedStarted;
    private static Blip LastWantedCenterBlip;
    private static Blip CurrentWantedCenterBlip;
    private static Blip InvestigationBlip;

    //private static uint GameTimeWantedStarted;
    private static uint GameTimeLastWantedEnded;
    public static uint GameTimePoliceStateStart;
    private static uint GameTimeLastSetWanted;
    private static uint GameTimeLastStartedJacking;
    private static bool PrevPlayerIsJacking;

    private static bool PrevPlayerStarsGreyedOut;
    private static PoliceState PrevPoliceState;
    private static float LastWantedCenterBlipSize;
    public static List<Blip> CreatedBlips = new List<Blip>();
    public static List<Blip> TempBlips = new List<Blip>();
    private static uint GameTimeLastReportedSpotted;
    private static uint LastSeenVehicleHandle;
    private static bool PrevPoliceInVestigationMode;
    private static uint GameTimeStartedInvestigation;
    private static Vector3 PrevInvestigationPosition;
    public static bool AnyPoliceCanSeePlayer { get; set; }
    public static bool AnyPoliceCanRecognizePlayer { get; set; }
    public static bool AnyPoliceRecentlySeenPlayer { get; set; }
    public static PoliceState CurrentPoliceState { get; set; }
    public static bool PlayerStarsGreyedOut { get; set; }
    public static bool AnyPoliceSeenPlayerThisWanted { get; set; }
    public static Vector3 PlacePlayerLastSeen { get; set; }
    public static bool PlayerArtificiallyShooting { get; set; }
    public static PoliceState LastPoliceState { get; set; }
    public static bool PlayerIsJacking { get; set; } = false;
    public static bool PlayerLastSeenInVehicle { get; set; }
    public static float PlayerLastSeenHeading { get; set; }
    public static Vector3 PlayerLastSeenForwardVector { get; set; }
    public static bool IsNightTime { get; set; }
    public static int PreviousWantedLevel { get; set; }
    public static bool PoliceInSearchMode
    {
        get
        {
            if (PlayerStarsGreyedOut && PedScanning.CopPeds.All(x => !x.RecentlySeenPlayer()))
                return true;
            else
                return false;
        }
    }
    public static float InvestigationDistance { get; set; }
    public static Vector3 InvestigationPosition { get; set; }
    public static float NearInvestigationDistance { get; set; }
    public static bool PoliceInInvestigationMode { get; set; }
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
    public static bool InvestigationModeExpired
    {
        get
        {
            if (GameTimeStartedInvestigation == 0)
                return false;
            else if (Game.GameTime - GameTimeStartedInvestigation >= 180000)
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
    public static uint GameTimeCiviliansLastCalledInCrime { get; set; }

    public enum PoliceState
    {
        Normal = 0,
        UnarmedChase = 1,
        CautiousChase = 2,
        DeadlyChase = 3,
        ArrestedWait = 4,
    }
    enum DispatchType
    {
        PoliceAutomobile = 1,
        PoliceHelicopter = 2,
        FireDepartment = 3,
        SwatAutomobile = 4,
        AmbulanceDepartment = 5,
        PoliceRiders = 6,
        PoliceVehicleRequest = 7,
        PoliceRoadBlock = 8,
        PoliceAutomobileWaitPulledOver = 9,
        PoliceAutomobileWaitCruising = 10,
        Gangs = 11,
        SwatHelicopter = 12,
        PoliceBoat = 13,
        ArmyVehicle = 14,
        BikerBackup = 15
    };
    public static void Initialize()
    {
        CurrentCrimes = new RapSheet();
        WantedLevelStartTime = 0;
        LastWantedCenterPosition = default;
        CanReportLastSeen = false;
        GameTimeLastGreyedOut = 0;
        PlaceWantedStarted = default;
        LastWantedCenterBlip = default;
        CurrentWantedCenterBlip = default;
        GameTimeLastWantedEnded = 0;
        GameTimePoliceStateStart = 0;
        GameTimeLastSetWanted = 0;
        GameTimeLastStartedJacking = 0;
        PrevPlayerIsJacking = false;
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

        PlacePlayerLastSeen = default;
        PlayerArtificiallyShooting = false;
        LastPoliceState = PoliceState.Normal;

        PlayerIsJacking = false;
        PlayerLastSeenInVehicle = false;
        PlayerLastSeenHeading = 0f;
        PlayerLastSeenForwardVector = default;
        IsNightTime = false;
        PreviousWantedLevel = 0;
        GameTimeLastReportedSpotted = 0;
        LastSeenVehicleHandle = 0;
        LastWantedCenterBlipSize = LosSantosRED.MySettings.Police.LastWantedCenterSize;

        InvestigationPosition = Vector3.Zero;
        InvestigationDistance = 800f;//350f;
        PrevInvestigationPosition = Vector3.Zero;
        NearInvestigationDistance = 100f;
        IsRunning = true;
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void PoliceGeneralTick()
    {
        UpdatePolice();
        GetPoliceState();
        TrackedVehiclesTick();
        WantedLevelTick();
        InvestigationTick();
        DispatchTick();
    }
    private static void DispatchTick()
    {
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceAutomobile, false);
        //NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceHelicopter, false);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceVehicleRequest, false);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.SwatAutomobile, false);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.SwatHelicopter, false);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceRiders, false);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceRoadBlock, false);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceAutomobileWaitCruising, false);
    }
    private static void InvestigationTick()
    {
        if (InvestigationModeExpired) //remove after 3 minutes
            PoliceInInvestigationMode = false;

        if (PrevInvestigationPosition != InvestigationPosition)
            InvestigationPositionChanged();

        if(PrevPoliceInVestigationMode != PoliceInInvestigationMode)
            PoliceInInvestigationModeChanged();
    }
    private static void InvestigationPositionChanged()
    {
        UpdateInvestigationUI();
        Debugging.WriteToLog("ValueChecker", string.Format("InvestigationPosition Changed to: {0}", InvestigationPosition));
        PrevInvestigationPosition = InvestigationPosition;
    }
    private static void PoliceInInvestigationModeChanged()
    {
        if (PoliceInInvestigationMode) //added
        {
            UpdateInvestigationUI();

            GameTimeStartedInvestigation = Game.GameTime;
        }
        else //removed
        {
            bool PlayDispatch = Game.LocalPlayer.Character.DistanceTo2D(InvestigationPosition) <= 250f;
            AddUpdateInvestigationBlip(Vector3.Zero, NearInvestigationDistance);
            if (LosSantosRED.PlayerIsNotWanted)
            {
                if(PersonOfInterest.PlayerIsPersonOfInterest)
                    PersonOfInterest.ResetPersonOfInterest(false);
                if(PlayDispatch)
                    DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.NoFurtherUnitsNeeded, 30) { IsAmbient = true });
            }
            GameTimeStartedInvestigation = 0;
        }
        Debugging.WriteToLog("ValueChecker", string.Format("PoliceInInvestigationMode Changed to: {0}", PoliceInInvestigationMode));
        PrevPoliceInVestigationMode = PoliceInInvestigationMode;
    }
    private static void UpdateInvestigationUI()
    {
        UpdateInvestigationPosition();
        AddUpdateInvestigationBlip(InvestigationPosition, NearInvestigationDistance);
    }
    private static void UpdateInvestigationPosition()
    {
        //GO to the nearest road node
        Vector3 SpawnLocation = Vector3.Zero;
        LosSantosRED.GetStreetPositionandHeading(InvestigationPosition, out SpawnLocation, out float Heading);
        if (SpawnLocation != Vector3.Zero)
            InvestigationPosition = SpawnLocation;
    }
    private static void UpdatePolice()
    {
        PlayerStarsGreyedOut = NativeFunction.CallByName<bool>("ARE_PLAYER_STARS_GREYED_OUT", Game.LocalPlayer);

        if (PrevPlayerStarsGreyedOut != PlayerStarsGreyedOut)
            PlayerStarsGreyedOutChanged();

        PlayerIsJacking = Game.LocalPlayer.Character.IsJacking;

        if (PrevPlayerIsJacking != PlayerIsJacking)
            PlayerJackingChanged(PlayerIsJacking);

        UpdatedCopsStats();
        CheckRecognition();

        CurrentCrimes.CheckCrimes();
    }
    public static void UpdatedCopsStats()
    {
        PedScanning.CopPeds.RemoveAll(x => !x.Pedestrian.Exists());
        PedScanning.K9Peds.RemoveAll(x => !x.Pedestrian.Exists() || x.Pedestrian.IsDead);
        foreach (GTACop Cop in PedScanning.CopPeds)
        {
            if (Cop.Pedestrian.IsDead)
            {
                CheckCopKilled(Cop);
                continue;
            }
            int NewHealth = Cop.Pedestrian.Health;
            if (NewHealth != Cop.Health)
            {
                if (!Cop.HurtByPlayer && LosSantosRED.PlayerHurtPed(Cop))
                {
                    CurrentCrimes.HurtingPolice.CrimeObserved();
                    Cop.HurtByPlayer = true;
                }
                Cop.Health = NewHealth;
            }
            Cop.IsInVehicle = Cop.Pedestrian.IsInAnyVehicle(false);
            if (Cop.IsInVehicle)
            {
                Cop.IsInHelicopter = Cop.Pedestrian.IsInHelicopter;
                if (!Cop.IsInHelicopter)
                    Cop.IsOnBike = Cop.Pedestrian.IsOnBike;
            }
            else
            {
                Cop.IsInHelicopter = false;
                Cop.IsOnBike = false;
            }
            Cop.UpdateDistance();
        }
        foreach(GTACop Cop in PedScanning.CopPeds.Where(x => x.Pedestrian.IsDead))
        {
            MarkNonPersistent(Cop);
        }
        PedScanning.CopPeds.RemoveAll(x => x.Pedestrian.IsDead);
        PedScanning.PoliceVehicles.RemoveAll(x => !x.Exists());         

    }
    public static void CheckCopKilled(GTACop MyPed)
    {
        if (!MyPed.HurtByPlayer && LosSantosRED.PlayerHurtPed(MyPed))
        {
            MyPed.HurtByPlayer = true;
            CurrentCrimes.HurtingPolice.CrimeObserved();   
        }
        if (!MyPed.KilledByPlayer && LosSantosRED.PlayerKilledPed(MyPed))
        {
            MyPed.KilledByPlayer = true;
            CurrentCrimes.KillingPolice.CrimeObserved();
            Debugging.WriteToLog("CheckKilled", string.Format("PlayerKilled: {0}", MyPed.Pedestrian.Handle));
        }
    }
    public static void CheckPoliceSight()
    {
        CheckLOS(Game.LocalPlayer.Character.IsInAnyVehicle(false) ? (Entity)Game.LocalPlayer.Character.CurrentVehicle : (Entity)Game.LocalPlayer.Character);
        SetPrimaryPursuer();
    }
    public static void CheckLOS(Entity EntityToCheck)
    {
        int TotalEntityNativeLOSChecks = 0;
        bool SawPlayerThisCheck = false;
        float RangeToCheck = 55f;

        foreach (GTACop Cop in PedScanning.CopPeds.Where(x => x.Pedestrian.Exists() && !x.Pedestrian.IsDead && !x.Pedestrian.IsInHelicopter))
        {
            //if (SawPlayerThisCheck && TotalEntityNativeLOSChecks >= 3 && Cop.GameTimeLastLOSCheck <= 1500)//we have already done 3 checks, saw us and they were looked at last check
            //{
            //    break;
            //}
            Cop.GameTimeLastLOSCheck = Game.GameTime;
            if (Cop.DistanceToPlayer <= RangeToCheck && Cop.Pedestrian.PlayerIsInFront() && !Cop.Pedestrian.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Cop.Pedestrian, EntityToCheck))//if (Cop.CopPed.PlayerIsInFront() && Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, RangeToCheck) && !Cop.CopPed.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Cop.CopPed, EntityToCheck)) //was 55f
            {
                Cop.UpdateContinuouslySeen();
                Cop.CanSeePlayer = true;
                Cop.GameTimeLastSeenPlayer = Game.GameTime;
                Cop.PositionLastSeenPlayer = Game.LocalPlayer.Character.Position;
                PlayerLastSeenInVehicle = LosSantosRED.PlayerInVehicle;
                PlayerLastSeenHeading = Game.LocalPlayer.Character.Heading;
                PlayerLastSeenForwardVector = Game.LocalPlayer.Character.ForwardVector;
                SawPlayerThisCheck = true;
            }
            else
            {
                Cop.GameTimeContinuoslySeenPlayerSince = 0;
                Cop.CanSeePlayer = false;
            }
            TotalEntityNativeLOSChecks++;
        }
        if (SawPlayerThisCheck)
            return;
        foreach (GTACop Cop in PedScanning.CopPeds.Where(x => x.Pedestrian.Exists() && !x.Pedestrian.IsDead && x.Pedestrian.IsInHelicopter))
        {
            Cop.GameTimeLastLOSCheck = Game.GameTime;
            if (Cop.DistanceToPlayer <= 250f && !Cop.Pedestrian.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY", Cop.Pedestrian, EntityToCheck, 17)) //was 55f
            {
                Cop.UpdateContinuouslySeen();
                Cop.CanSeePlayer = true;
                Cop.GameTimeLastSeenPlayer = Game.GameTime;
                Cop.PositionLastSeenPlayer = Game.LocalPlayer.Character.Position;
                PlayerLastSeenInVehicle = LosSantosRED.PlayerInVehicle;
                PlayerLastSeenHeading = Game.LocalPlayer.Character.Heading;
                PlayerLastSeenForwardVector = Game.LocalPlayer.Character.ForwardVector;
                break;//Only care if one of the people saw it as we wont be tasking them 
            }
            else
            {
                Cop.GameTimeContinuoslySeenPlayerSince = 0;
                Cop.CanSeePlayer = false;
            }
        }
        if (SawPlayerThisCheck)
            return;

        foreach (GTAPed Civi in PedScanning.Civilians.Where(x => x.Pedestrian.Exists() && !x.Pedestrian.IsDead && !x.Pedestrian.IsInHelicopter))
        {
            Civi.GameTimeLastLOSCheck = Game.GameTime;
            if (Civi.DistanceToPlayer <= 40f && Civi.Pedestrian.PlayerIsInFront() && !Civi.Pedestrian.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Civi.Pedestrian, EntityToCheck))//if (Cop.CopPed.PlayerIsInFront() && Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, RangeToCheck) && !Cop.CopPed.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Cop.CopPed, EntityToCheck)) //was 55f
            {
                Civi.UpdateContinuouslySeen();
                Civi.CanSeePlayer = true;
                Civi.GameTimeLastSeenPlayer = Game.GameTime;
                Civi.PositionLastSeenPlayer = Game.LocalPlayer.Character.Position;
                SawPlayerThisCheck = true;

                if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
                {
                    if (Civi.CanSeePlayer || Civi.DistanceToPlayer <= 7f)
                        Civi.CanRecognizePlayer = true;
                    else
                        Civi.CanRecognizePlayer = false;
                }
                else
                {
                    if (Civi.HasSeenPlayerFor >= 1250)
                        Civi.CanRecognizePlayer = true;
                    else if (Civi.DistanceToPlayer <= 2f && Civi.DistanceToPlayer > 0f)
                        Civi.CanRecognizePlayer = true;
                    else
                        Civi.CanRecognizePlayer = false;
                }
            }
            else
            {
                Civi.GameTimeContinuoslySeenPlayerSince = 0;
                Civi.CanSeePlayer = false;
                Civi.CanRecognizePlayer = false;
            }
            TotalEntityNativeLOSChecks++;
        }
    }
    //private static bool PoliceCanSeeEntity(Entity EntityToCheck)
    //{
    //    if (!EntityToCheck.Exists())
    //        return false;

    //    float RangeToCheck = 55f;

    //    foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.Pedestrian.Exists() && !x.Pedestrian.IsDead && !x.Pedestrian.IsInHelicopter))
    //    {
    //        if (EntityToCheck.IsInFront(Cop.Pedestrian) && Cop.Pedestrian.IsInRangeOf(EntityToCheck.Position, RangeToCheck) && !Cop.Pedestrian.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Cop.Pedestrian, EntityToCheck)) //was 55f
    //        {
    //            return true;
    //        }
    //    }
    //    foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.Pedestrian.Exists() && !x.Pedestrian.IsDead && x.Pedestrian.IsInHelicopter))
    //    {
    //        if (Cop.Pedestrian.IsInRangeOf(EntityToCheck.Position, 250f) && !Cop.Pedestrian.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY", Cop.Pedestrian, EntityToCheck, 17)) //was 55f
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}
    public static void SetPrimaryPursuer()
    {
        if (PedScanning.CopPeds.Count == 0)
            return;
        foreach (GTACop Cop in PedScanning.CopPeds.Where(x => x.Pedestrian.Exists() && !x.Pedestrian.IsDead && !x.Pedestrian.IsInHelicopter))
        {
            Cop.IsPursuitPrimary = false;
        }
    }
    public static void IssueCopPistol(GTACop Cop)
    {
        GTAWeapon Pistol;
        Pistol = GTAWeapons.WeaponsList.Where(x => x.isPoliceIssue && x.Category == GTAWeapon.WeaponCategory.Pistol).PickRandom();
        Cop.IssuedPistol = Pistol;
        Cop.Pedestrian.Inventory.GiveNewWeapon(Pistol.Name, Pistol.AmmoAmount, false);
        if (LosSantosRED.MySettings.Police.AllowPoliceWeaponVariations)
        {
            GTAWeapon.WeaponVariation MyVariation = Pistol.PoliceVariations.PickRandom();
            Cop.PistolVariation = MyVariation;
            LosSantosRED.ApplyWeaponVariation(Cop.Pedestrian, (uint)Pistol.Hash, MyVariation);
        }
    }
    public static void IssueCopHeavyWeapon(GTACop Cop)
    {
        GTAWeapon IssuedHeavy;
        int Num = LosSantosRED.MyRand.Next(1, 5);
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
        Cop.Pedestrian.Inventory.GiveNewWeapon(IssuedHeavy.Name, IssuedHeavy.AmmoAmount, true);
        if (LosSantosRED.MySettings.Police.OverridePoliceAccuracy)
            Cop.Pedestrian.Accuracy = LosSantosRED.MySettings.Police.PoliceHeavyAccuracy;
        if (LosSantosRED.MySettings.Police.AllowPoliceWeaponVariations)
        {
            GTAWeapon.WeaponVariation MyVariation = IssuedHeavy.PoliceVariations.PickRandom();
            Cop.HeavyVariation = MyVariation;
            LosSantosRED.ApplyWeaponVariation(Cop.Pedestrian, (uint)IssuedHeavy.Hash, MyVariation);
        }
    }
    private static void GetPoliceState()
    {
        if (CurrentPoliceState == PoliceState.ArrestedWait || CurrentPoliceState == PoliceState.DeadlyChase)
            return;

        if (LosSantosRED.PlayerWantedLevel == 0)
            CurrentPoliceState = PoliceState.Normal;//Default state
        else if (LosSantosRED.PlayerWantedLevel >= 1 && LosSantosRED.PlayerWantedLevel <= 3 && AnyPoliceCanSeePlayer)//AnyCanSeePlayer)
        {
            bool IsDeadly = CurrentCrimes.LethalForceAuthorized;//(CurrentCrimes.FiringWeaponNearPolice.HasBeenWitnessedByPolice || CurrentCrimes.HurtingPolice.HasBeenWitnessedByPolice || CurrentCrimes.AimingWeaponAtPolice.HasBeenWitnessedByPolice || CurrentCrimes.TrespessingOnGovtProperty.HasBeenWitnessedByPolice || CurrentCrimes.KillingPolice.HasBeenWitnessedByPolice);
            if (!IsDeadly && !LosSantosRED.PlayerIsConsideredArmed) // Unarmed and you havent killed anyone
                CurrentPoliceState = PoliceState.UnarmedChase;
            else if (!IsDeadly)
                CurrentPoliceState = PoliceState.CautiousChase;
            else
                CurrentPoliceState = PoliceState.DeadlyChase;
        }
        else if (LosSantosRED.PlayerWantedLevel >= 1 && LosSantosRED.PlayerWantedLevel <= 3)
        {
            CurrentPoliceState = PoliceState.UnarmedChase;
        }
        else if (LosSantosRED.PlayerWantedLevel >= 4)
            CurrentPoliceState = PoliceState.DeadlyChase;

    }
    private static void TrackedVehiclesTick()
    {
        LosSantosRED.TrackedVehicles.RemoveAll(x => !x.VehicleEnt.Exists());
        if (LosSantosRED.PlayerIsNotWanted)
        {
            foreach (GTAVehicle StolenCar in LosSantosRED.TrackedVehicles.Where(x => x.NeedsToBeReportedStolen))
            {
                StolenCar.WasReportedStolen = true;
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.ReportStolenVehicle, 10)
                {
                    ResultsInStolenCarSpotted = true,
                    VehicleToReport = StolenCar
                });
            }
        }
        if (LosSantosRED.PlayerInVehicle && Game.LocalPlayer.Character.IsInAnyVehicle(false))//first check is cheaper, but second is required to verify
        {
            if (LosSantosRED.PlayersCurrentTrackedVehicle == null)
                return;

            if(AnyPoliceCanSeePlayer && LosSantosRED.PlayerIsWanted && !PlayerStarsGreyedOut)
            {
                if (LastSeenVehicleHandle != 0 && LastSeenVehicleHandle != LosSantosRED.PlayersCurrentTrackedVehicle.VehicleEnt.Handle && !LosSantosRED.PlayersCurrentTrackedVehicle.HasBeenDescribedByDispatch)
                {
                    GameTimeLastReportedSpotted = Game.GameTime;
                    DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.SuspectChangedVehicle, 21) {IsAmbient = true,VehicleToReport = LosSantosRED.PlayersCurrentTrackedVehicle});
                }


                LastSeenVehicleHandle = LosSantosRED.PlayersCurrentTrackedVehicle.VehicleEnt.Handle;            
            }
            if (AnyPoliceCanRecognizePlayer)
            {
                if (LosSantosRED.PlayerWantedLevel > 0 && !PlayerStarsGreyedOut)
                    UpdateVehicleDescription(LosSantosRED.PlayersCurrentTrackedVehicle);

            }
        }
    }
    private static void UpdateVehicleDescription(GTAVehicle MyVehicle)
    {
        if(MyVehicle.VehicleEnt.Exists())
            MyVehicle.DescriptionColor = MyVehicle.VehicleEnt.PrimaryColor;
        if(MyVehicle.CarPlate != null)
            MyVehicle.CarPlate.IsWanted = true;
        if (MyVehicle.IsStolen && !MyVehicle.WasReportedStolen)
            MyVehicle.WasReportedStolen = true;
    }
    private static void WantedLevelTick()
    {

        


        if (PreviousWantedLevel != Game.LocalPlayer.WantedLevel)
            WantedLevelChanged();

        if (PrevPoliceState != CurrentPoliceState)
            PoliceStateChanged();

        if (LosSantosRED.PlayerIsWanted && Game.LocalPlayer.WantedLevel > 0)
        {
            Vector3 CurrentWantedCenter = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
            if (CurrentWantedCenter != Vector3.Zero)
            {
                LastWantedCenterPosition = CurrentWantedCenter;
                AddUpdateCurrentWantedBlip(CurrentWantedCenter);
            }
           
            if (LosSantosRED.MySettings.Police.WantedLevelIncreasesOverTime && PlayerHasBeenWantedFor > LosSantosRED.MySettings.Police.WantedLevelIncreaseTime && AnyPoliceCanSeePlayer && LosSantosRED.PlayerWantedLevel <= LosSantosRED.MySettings.Police.WantedLevelInceaseOverTimeLimit)
            {
                //SetWantedLevel(InstantAction.PlayerWantedLevel + 1, "Wanted Level increased over time");
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.RequestBackup, 3)
                {
                    ResultsInLethalForce = Game.LocalPlayer.WantedLevel >= 3
                    ,ResultingWantedLevel = LosSantosRED.PlayerWantedLevel + 1
                });
            }

            //if (LosSantosRED.MyLosSantosRED.MySettings.SpawnNewsChopper && Game.GameTime - WantedLevelStartTime > 180000 && WantedLevelStartTime > 0 && AnyPoliceRecentlySeenPlayer && InstantAction.PlayerWantedLevel > 4 && !PoliceScanning.Reporters.Any())
            //{
            //    PoliceSpawning.SpawnNewsChopper();
            //    LocalWriteToLog("WantedLevelTick", "Been at this wanted for a while, wanted news chopper spawned (if they dont already exist)");
            //}
            if (CanReportLastSeen && Game.GameTime - GameTimeLastGreyedOut > 15000 && AnyPoliceSeenPlayerThisWanted && PlayerHasBeenWantedFor > 45000 && !PedScanning.CopPeds.Any(x => x.DistanceToPlayer <= 150f))
            {
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.LostVisualOnSuspect, 10));
                CanReportLastSeen = false;
            }
            if (CurrentPoliceState == PoliceState.DeadlyChase && LosSantosRED.PlayerWantedLevel < 3)
            {
                SetWantedLevel(3, "Deadly chase requires 3+ wanted level",true);
            }


            if (!DispatchAudio.RecentAnnouncedDispatch && AnyPoliceSeenPlayerThisWanted && CanPlaySuspectSpotted && AnyPoliceCanSeePlayer && LosSantosRED.PlayerInVehicle && !DispatchAudio.IsPlayingAudio && LosSantosRED.PlayerInAutomobile && LosSantosRED.PlayersCurrentTrackedVehicle != null)
            {

                Crime ToReportInstead = CurrentCrimes.GetListOfCrimes().Where(x => x.RecentlyCommittedCrime(15000) && !x.RecentlyReportedCrime(60000)).OrderBy(x => x.DispatchToPlay.Priority).FirstOrDefault();
                if (ToReportInstead != null)
                {
                    Debugging.WriteToLog("Reporting Crime Instead of Spotted", ToReportInstead.Name);
                    ToReportInstead.DispatchToPlay.IsAmbient = true;
                    ToReportInstead.DispatchToPlay.ReportedBy = DispatchAudio.ReportType.Officers;
                    DispatchAudio.AddDispatchToQueue(ToReportInstead.DispatchToPlay);
                }
                else
                {
                    Debugging.WriteToLog("Spotted", "Playing Spotted");
                    DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.SuspectSpotted, 25) { IsAmbient = true });
                }
                
                GameTimeLastReportedSpotted = Game.GameTime;
            }

        }
    }
    public static void PoliceReportedAllClear()
    {
        PoliceInInvestigationMode = false;
    }
    public static void ResetPoliceStats()
    {
        foreach (GTACop Cop in PedScanning.CopPeds)
        {
            Cop.HurtByPlayer = false;
        }
        CurrentCrimes = new RapSheet();
        Debugging.WriteToLog("ResetPoliceStats", "Ran (Made New Rap Sheet)");


        CurrentPoliceState = PoliceState.Normal;
        AnyPoliceSeenPlayerThisWanted = false;
        WantedLevelStartTime = 0;
        //GameTimeWantedStarted = 0;
        GameTimeLastWantedEnded = Game.GameTime;
        PoliceInInvestigationMode = false;

        //TrafficViolations.ResetTrafficViolations();
        DispatchAudio.ResetReportedItems();
    }
    public static void SetWantedLevel(int WantedLevel,string Reason,bool UpdateRecent)
    {
        if(UpdateRecent)
            GameTimeLastSetWanted = Game.GameTime;
        if (Game.LocalPlayer.WantedLevel < WantedLevel || WantedLevel == 0)
        {
            Debugging.WriteToLog("SetWantedLevel", string.Format("Current Wanted: {0}, Desired Wanted: {1}", Game.LocalPlayer.WantedLevel, WantedLevel));
            Game.LocalPlayer.WantedLevel = WantedLevel;
            Debugging.WriteToLog("SetWantedLevel", string.Format("Manually set to: {0}: {1}", WantedLevel, Reason));
        }
        //else
        //{
        //    LocalWriteToLog("SetWantedLevel", string.Format("Wanted NOT Set Current Wanted: {0}, Desired Wanted: {1}", Game.LocalPlayer.WantedLevel, WantedLevel));
        //}
    }
    public static bool NearLastWanted()
    {
        return LastWantedCenterPosition != Vector3.Zero && Game.LocalPlayer.Character.DistanceTo2D(LastWantedCenterPosition) <= LastWantedCenterBlipSize;
    }
    public static bool NearInvestigationPosition()
    {
        return InvestigationPosition != Vector3.Zero && Game.LocalPlayer.Character.DistanceTo2D(InvestigationPosition) <= NearInvestigationDistance;
    }
    public static void CheckRecognition()
    {
        AnyPoliceCanSeePlayer = PedScanning.CopPeds.Any(x => x.CanSeePlayer) && !PlayerStarsGreyedOut;
        

        if (AnyPoliceCanSeePlayer)
            AnyPoliceRecentlySeenPlayer = true;
        else
            AnyPoliceRecentlySeenPlayer = PedScanning.CopPeds.Any(x => x.SeenPlayerSince(LosSantosRED.MySettings.Police.PoliceRecentlySeenTime));

        AnyPoliceCanRecognizePlayer = PedScanning.CopPeds.Any(x => x.HasSeenPlayerFor >= TimeToRecognize() || (x.CanSeePlayer && x.DistanceToPlayer <= 20f) || (x.DistanceToPlayer <= 7f && x.DistanceToPlayer > 0f));
        

        if (!AnyPoliceSeenPlayerThisWanted)
            PlacePlayerLastSeen = PlaceWantedStarted;
        else if (!PlayerStarsGreyedOut)//(AnyPoliceRecentlySeenPlayer || !PlayerStarsGreyedOut)
            PlacePlayerLastSeen = Game.LocalPlayer.Character.Position;
        else if(PlayerStarsGreyedOut && Game.LocalPlayer.Character.DistanceTo2D(PlacePlayerLastSeen) <= 15f && !Game.LocalPlayer.Character.Position.Z.IsWithin(PlacePlayerLastSeen.Z-2f,PlacePlayerLastSeen.Z+2f))//within the search zone but above or below it, need to help out my cops
            SearchModeStopping.StopSearchModeSingle();

        NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, PlacePlayerLastSeen.X, PlacePlayerLastSeen.Y, PlacePlayerLastSeen.Z);

        if (!AnyPoliceSeenPlayerThisWanted && AnyPoliceRecentlySeenPlayer)
            AnyPoliceSeenPlayerThisWanted = true;


        
    }
    public static float TimeToRecognize()
    {
        CheckNight();

        if (IsNightTime)
            return 3500;
        else if (LosSantosRED.PlayerInVehicle)
            return 750;
        else
            return 2000;
    }
    private static void CheckNight()
    {
        IsNightTime = false;
        int HourOfDay = NativeFunction.CallByName<int>("GET_CLOCK_HOURS");
        int MinuteOfDay = NativeFunction.CallByName<int>("GET_CLOCK_MINUTES"); 
        if (HourOfDay >= 20 || (HourOfDay >= 19 && MinuteOfDay >= 30) || HourOfDay <= 5)
            IsNightTime = true;
    }
    private static void WantedLevelChanged()
    {
        if (Game.LocalPlayer.WantedLevel == 0)
            WantedLevelRemoved();
        else
            GameTimeLastWantedEnded = 0;

        if (PreviousWantedLevel == 0 && Game.LocalPlayer.WantedLevel > 0)
            WantedLevelAdded();

        CurrentCrimes.MaxWantedLevel = LosSantosRED.PlayerWantedLevel;
        WantedLevelStartTime = Game.GameTime;
        Debugging.WriteToLog("ValueChecker", String.Format("WantedLevel Changed to: {0}, Recently Set: {1}", Game.LocalPlayer.WantedLevel,RecentlySetWanted));
        PreviousWantedLevel = Game.LocalPlayer.WantedLevel;
    }
    private static void WantedLevelRemoved()
    {
        if (!LosSantosRED.IsDead)//they might choose the respawn as the same character, so do not replace it yet?
        {
            CurrentCrimes.GameTimeWantedEnded = Game.GameTime;
            CurrentCrimes.MaxWantedLevel = LosSantosRED.MaxWantedLastLife;
            if (CurrentCrimes.PlayerSeenDuringWanted && PreviousWantedLevel != 0)// && !RecentlySetWanted)//i didnt make it go to zero, the chase was lost organically
            {
                PersonOfInterest.StoreCriminalHistory(CurrentCrimes);
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.SuspectLost, 5));
            }

            ResetPoliceStats();

            Tasking.UntaskAll(false);
            Tasking.RetaskAllRandomSpawns();
        }

        foreach (Blip myBlip in TempBlips)
        {
            if (myBlip.Exists())
                myBlip.Delete();
        }

        AddUpdateCurrentWantedBlip(Vector3.Zero);
    }
    private static void WantedLevelAdded()
    {
        if (!RecentlySetWanted)//randomly set by the game
        {
            if (LosSantosRED.PlayerWantedLevel <= 2)//let some level 3 and 4 wanted override and be set
            {
                SetWantedLevel(0, "Resetting Unknown Wanted", false);
                //GetReportedCrimeFromUnknown();
                return;
            }
        }

        PoliceInInvestigationMode = false;
        CurrentCrimes.GameTimeWantedStarted = Game.GameTime;
        CurrentCrimes.MaxWantedLevel = LosSantosRED.PlayerWantedLevel;
        PlaceWantedStarted = Game.LocalPlayer.Character.Position;
        Tasking.UntaskAllRandomSpawns(false);
    }
    //public static void GetReportedCrimeFromUnknown()//temp public
    //{       
    //    Debugging.WriteToLog("AddDispatchToUnknownWanted", "Got wanted without being manually set");
    //    GTAWeapon MyGun = LosSantosRED.GetCurrentWeapon();
    //    DispatchAudio.ReportDispatch ToReport;

    //    InvestigationPosition = Game.LocalPlayer.Character.Position;
    //    UpdateInvestigationPosition();

    //    if (LosSantosRED.PlayerRecentlyShot(20000) && Civilians.RecentlyKilledCivilian(10000))
    //        ToReport = DispatchAudio.ReportDispatch.ReportCivilianShot;
    //    else if (!LosSantosRED.PlayerRecentlyShot(20000) && Civilians.RecentlyKilledCivilian(10000))
    //        ToReport = DispatchAudio.ReportDispatch.ReportCivilianFatality;
    //    else if (LosSantosRED.PlayerRecentlyShot(20000) && LosSantosRED.PlayerIsConsideredArmed && MyGun != null && MyGun.WeaponLevel >= 3)
    //        ToReport = DispatchAudio.ReportDispatch.ReportLowLevelTerroristActivity;
    //    else if (LosSantosRED.PlayerRecentlyShot(20000))
    //        ToReport = DispatchAudio.ReportDispatch.ReportShotsFired;
    //    else if (CarStealing.PlayerBreakingIntoCar)
    //        ToReport = DispatchAudio.ReportDispatch.ReportGrandTheftAuto;
    //    else if (Civilians.RecentlyHurtCivilian(10000))
    //        ToReport = DispatchAudio.ReportDispatch.ReportCivilianInjury;
    //    else
    //        ToReport = DispatchAudio.ReportDispatch.ReportLowLevelCriminalActivity;

    //    if (LosSantosRED.PlayerIsNotWanted)
    //    {
    //        PoliceInInvestigationMode = true;
    //        DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(ToReport, 20) { IsAmbient = true, ReportedBy = DispatchAudio.ReportType.Civilians });
    //    }
    //}

    private static void PlayerStarsGreyedOutChanged()
    {
        Debugging.WriteToLog("ValueChecker", String.Format("PlayerStarsGreyedOut Changed to: {0}", PlayerStarsGreyedOut));
        if (PlayerStarsGreyedOut)
        {
            CanReportLastSeen = true;
            GameTimeLastGreyedOut = Game.GameTime;
        }
        else
        {
            foreach (GTACop Cop in PedScanning.CopPeds)
            {
                Cop.AtWantedCenterDuringSearchMode = false;
                //if (Cop.IsTasked && (Cop.TaskType == Tasking.AssignableTasks.GoToWantedCenter || Cop.TaskType == Tasking.AssignableTasks.SimpleInvestigate || Cop.TaskType == Tasking.AssignableTasks.RandomSpawnIdle))
                //{
                //    Tasking.AddItemToQueue(new CopTask(Cop, Tasking.AssignableTasks.Untask));
                //}
            }
            CanReportLastSeen = false;
            if (LosSantosRED.PlayerIsWanted && AnyPoliceSeenPlayerThisWanted && CanPlaySuspectSpotted && LosSantosRED.PlayerInVehicle && !DispatchAudio.IsPlayingAudio && LosSantosRED.PlayerInAutomobile && LosSantosRED.PlayersCurrentTrackedVehicle != null)
            {
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.SuspectSpotted, 25) { IsAmbient = true });
                GameTimeLastReportedSpotted = Game.GameTime;  
            }
        }
        PrevPlayerStarsGreyedOut = PlayerStarsGreyedOut;
    }
    private static void PoliceStateChanged()
    {
        Debugging.WriteToLog("ValueChecker", String.Format("PoliceState Changed to: {0}", CurrentPoliceState));
        Debugging.WriteToLog("ValueChecker", String.Format("PreviousPoliceState Changed to: {0}", PrevPoliceState));
        LastPoliceState = PrevPoliceState;
        if (CurrentPoliceState == PoliceState.Normal && !LosSantosRED.IsDead)
        {

        }
        if (CurrentPoliceState == PoliceState.DeadlyChase)
        {
            if (PrevPoliceState != PoliceState.ArrestedWait && !DispatchAudio.ReportedLethalForceAuthorized && !LosSantosRED.IsDead && !LosSantosRED.IsBusted)
            {
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.LethalForceAuthorized, 6) { ResultsInLethalForce = true });
            }
        }
        GameTimePoliceStateStart = Game.GameTime;
        PrevPoliceState = CurrentPoliceState;
    }
    private static void PlayerJackingChanged(bool isJacking)
    {
        PlayerIsJacking = isJacking;
        Debugging.WriteToLog("ValueChecker", String.Format("PlayerIsJacking Changed to: {0}", PlayerIsJacking));
        if (PlayerIsJacking)
        {
            GameTimeLastStartedJacking = Game.GameTime;
        }
        PrevPlayerIsJacking = PlayerIsJacking;
    }
    public static void AddUpdateLastWantedBlip(Vector3 Position)
    {
        if (Position == Vector3.Zero)
        {
            if (LastWantedCenterBlip.Exists())
                LastWantedCenterBlip.Delete();
            return;
        }
        if (!LastWantedCenterBlip.Exists())
        {
            int MaxWanted = PersonOfInterest.LastWantedLevel();
            if (MaxWanted != 0)
                LastWantedCenterBlipSize = MaxWanted * LosSantosRED.MySettings.Police.LastWantedCenterSize;
            else
                LastWantedCenterBlipSize = LosSantosRED.MySettings.Police.LastWantedCenterSize;

            LastWantedCenterBlip = new Blip(LastWantedCenterPosition, LastWantedCenterBlipSize)
            {
                Name = "Last Wanted Center Position",
                Color = Color.Yellow,
                Alpha = 0.25f
            };

            NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)LastWantedCenterBlip.Handle, true);
            CreatedBlips.Add(LastWantedCenterBlip);
        }
        if (LastWantedCenterBlip.Exists())
            LastWantedCenterBlip.Position = Position;
    }
    public static void AddUpdateCurrentWantedBlip(Vector3 Position)
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
    public static void AddUpdateInvestigationBlip(Vector3 Position,float Size)
    {
        if (Position == Vector3.Zero)
        {
            if (InvestigationBlip.Exists())
                InvestigationBlip.Delete();
            return;
        }
        if (!PoliceInInvestigationMode)
        {
            if (InvestigationBlip.Exists())
                InvestigationBlip.Delete();
            return;
        }
        if (!InvestigationBlip.Exists())
        {
            InvestigationBlip = new Blip(Position, Size)
            {
                Name = "Investigation Center",
                Color = Color.Orange,
                Alpha = 0.25f
            };

            NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)InvestigationBlip.Handle, true);
            CreatedBlips.Add(InvestigationBlip);
        }
        if (InvestigationBlip.Exists())
            InvestigationBlip.Position = Position;
    }
    public static void RemoveWantedBlips()
    {
        LastWantedCenterPosition = Vector3.Zero;
        if (LastWantedCenterBlip.Exists())
            LastWantedCenterBlip.Delete();
        if (CurrentWantedCenterBlip.Exists())
            CurrentWantedCenterBlip.Delete();
    }
    public static void RemoveBlip(Ped MyPed)
    {
        Blip MyBlip = MyPed.GetAttachedBlip();
        if (MyBlip.Exists())
            MyBlip.Delete();
    }
    public static void DeleteCop(GTACop Cop)
    {
        if (!Cop.Pedestrian.Exists())
            return;
        if (Cop.Pedestrian.IsInAnyVehicle(false))
        {
            if (Cop.Pedestrian.CurrentVehicle.HasPassengers)
            {
                foreach (Ped Passenger in Cop.Pedestrian.CurrentVehicle.Passengers)
                {
                    RemoveBlip(Passenger);
                    Passenger.Delete();
                }
            }
            if(Cop.Pedestrian.Exists() && Cop.Pedestrian.CurrentVehicle.Exists() && Cop.Pedestrian.CurrentVehicle != null)
                Cop.Pedestrian.CurrentVehicle.Delete();
        }
        RemoveBlip(Cop.Pedestrian);
        Cop.Pedestrian.Delete();
        Cop.WasMarkedNonPersistent = false;
        //LocalWriteToLog("SpawnCop", string.Format("Cop Deleted: Handled {0}", Cop.CopPed.Handle));
    }
    public static void MarkNonPersistent(GTACop Cop)
    {
        if (!Cop.Pedestrian.Exists())
            return;
        if (Cop.Pedestrian.IsInAnyVehicle(false))
        {
            if (Cop.Pedestrian.CurrentVehicle.HasPassengers)
            {
                foreach (Ped Passenger in Cop.Pedestrian.CurrentVehicle.Passengers)
                {
                    GTACop PassengerCop = PedScanning.CopPeds.Where(x => x.Pedestrian.Handle == Passenger.Handle).FirstOrDefault();
                    if(PassengerCop != null)
                    {
                        PassengerCop.Pedestrian.IsPersistent = false;
                        PassengerCop.WasMarkedNonPersistent = false;
                    }
                }
            }
            Cop.Pedestrian.CurrentVehicle.IsPersistent = false;
        }
        Cop.Pedestrian.IsPersistent = false;
        Cop.WasMarkedNonPersistent = false;
        //LocalWriteToLog("SpawnCop", string.Format("CopMarkedNonPersistant: Handled {0}", Cop.CopPed.Handle));
    }

}

