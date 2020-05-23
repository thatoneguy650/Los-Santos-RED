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
    private static uint WantedLevelStartTime;
    private static bool CanReportLastSeen;
    private static uint GameTimeLastGreyedOut;
    public static Vector3 PlaceWantedStarted;//temp public
    private static Blip CurrentWantedCenterBlip;
    private static Blip InvestigationBlip;

    private static uint GameTimeLastWantedEnded;
    private static uint GameTimePoliceStateStart;
    private static uint GameTimeLastSetWanted;
    private static uint GameTimeLastStartedJacking;
    private static bool PrevPlayerIsJacking;

    private static bool PrevPlayerStarsGreyedOut;
    private static PoliceState PrevPoliceState;
    
    private static uint GameTimeLastReportedSpotted;
    private static uint LastSeenVehicleHandle;
    private static bool PrevPoliceInVestigationMode;
    private static uint GameTimeStartedInvestigation;
    private static Vector3 PrevInvestigationPosition;

    public static uint GameTimeWantedStarted { get; set; }
    public static List<Blip> CreatedBlips { get; set; } = new List<Blip>();
    public static RapSheet CurrentCrimes { get; set; }
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
    public static float InvestigationDistance { get; set; }
    public static Vector3 InvestigationPosition { get; set; }
    public static float NearInvestigationDistance { get; set; }
    public static bool PoliceInInvestigationMode { get; set; }
    public static bool IsRunning { get; set; } = true;
    public static Vector3 LastWantedCenterPosition { get; set; }
    public static bool PoliceStateRecentlyStart
    {
        get
        {
            if (GameTimePoliceStateStart == 0)
                return true;
            else if (Game.GameTime - GameTimePoliceStateStart >= 8000)
                return true;
            else
                return false;
        }
    }
    public static bool PoliceInSearchMode
    {
        get
        {
            if (PlayerStarsGreyedOut && PedList.CopPeds.All(x => !x.RecentlySeenPlayer()))
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
            else if (Game.GameTime - GameTimeLastReportedSpotted >= 20000)//15000
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
    public static float ActiveDistance
    {
        get
        {
            return PlayerState.PlayerWantedLevel * 500f;
        }
    }
    public static bool NearInvestigationPosition
    {
        get
        {
            return InvestigationPosition != Vector3.Zero && Game.LocalPlayer.Character.DistanceTo2D(InvestigationPosition) <= NearInvestigationDistance;
        }    
    }
    private static float TimeToRecognize
    {
        get
        {
            CheckNight();

            if (IsNightTime)
                return 3500;
            else if (PlayerState.PlayerInVehicle)
                return 750;
            else
                return 2000;
        }
    }
    public static bool NearLastWanted(float DistanceTo)
    {
        return LastWantedCenterPosition != Vector3.Zero && Game.LocalPlayer.Character.DistanceTo2D(LastWantedCenterPosition) <= DistanceTo;
    }
    public static ResponsePriority CurrentResponse
    {
        get
        {
            if(PlayerState.PlayerIsNotWanted)
            {
                if (PoliceInInvestigationMode)
                {
                    if (CurrentCrimes.CrimeList.Any(x => x.RecentlyCalledInByCivilians(180000) && x.DispatchToPlay.Priority <= 8))
                    {
                        return ResponsePriority.Medium;
                    }
                    else
                    {
                        return ResponsePriority.Low;
                    }
                }
                else
                {
                    return ResponsePriority.None;
                }
            }
            else
            {
                if(PlayerState.PlayerWantedLevel > 4)
                {
                    return ResponsePriority.Full;
                }
                else if (PlayerState.PlayerWantedLevel >= 2)
                {
                    return ResponsePriority.High;
                }
                else
                {
                    return ResponsePriority.Medium;
                }
            }
        }
    }
    public enum PoliceState
    {
        Normal = 0,
        UnarmedChase = 1,
        CautiousChase = 2,
        DeadlyChase = 3,
        ArrestedWait = 4,
    }
    public enum DispatchType
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
    public enum ResponsePriority
    {
        None = 0,
        Low = 1,
        Medium = 2,
        High = 3,
        Full = 4,
    }
    public static void Initialize()
    {
        CurrentCrimes = new RapSheet();
        WantedLevelStartTime = 0;
        LastWantedCenterPosition = default;
        CanReportLastSeen = false;
        GameTimeLastGreyedOut = 0;
        PlaceWantedStarted = default;
        CurrentWantedCenterBlip = default;
        GameTimeLastWantedEnded = 0;
        GameTimePoliceStateStart = 0;
        GameTimeLastSetWanted = 0;
        GameTimeLastStartedJacking = 0;
        PrevPlayerIsJacking = false;
        PrevPlayerStarsGreyedOut = false;
        PrevPoliceState = PoliceState.Normal;
        CreatedBlips = new List<Blip>();
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

        InvestigationPosition = Vector3.Zero;
        InvestigationDistance = 800f;//350f;
        PrevInvestigationPosition = Vector3.Zero;
        NearInvestigationDistance = 250f;
        IsRunning = true;
        SetWantedLevel(0, "Initial", true);
    }
    public static void Dispose()
    {
        IsRunning = false;
        SetDispatchService(true);       
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            UpdatePolice();
            GetPoliceState();
            TrackedVehiclesTick();
            WantedLevelTick();
            InvestigationTick();
            DispatchTick();
        }
    }
    private static void UpdatePolice()
    {
        CheckEvents();
        UpdateCops();
        CheckRecognition();
        CurrentCrimes.CheckCrimes();

        //UI.DebugLine = string.Format("See: {0} Stars: {1} RecSeen: {2} Recog: {3}", AnyPoliceCanSeePlayer, PlayerStarsGreyedOut, AnyPoliceRecentlySeenPlayer,AnyPoliceCanRecognizePlayer);
    }
    private static void CheckEvents()
    {
        PlayerStarsGreyedOut = NativeFunction.CallByName<bool>("ARE_PLAYER_STARS_GREYED_OUT", Game.LocalPlayer);

        if (PrevPlayerStarsGreyedOut != PlayerStarsGreyedOut)
            PlayerStarsGreyedOutChanged();

        PlayerIsJacking = Game.LocalPlayer.Character.IsJacking;

        if (PrevPlayerIsJacking != PlayerIsJacking)
            PlayerJackingChanged(PlayerIsJacking);
    }
    private static void UpdateCops()
    {
        PedList.CopPeds.RemoveAll(x => !x.Pedestrian.Exists());
        PedList.K9Peds.RemoveAll(x => !x.Pedestrian.Exists() || x.Pedestrian.IsDead);
        foreach (GTACop Cop in PedList.CopPeds)
        {
            Cop.Update();
        }
        foreach (GTACop Cop in PedList.CopPeds.Where(x => x.Pedestrian.IsDead))
        {
            PoliceSpawning.MarkNonPersistent(Cop);
        }
        PedList.CopPeds.RemoveAll(x => x.Pedestrian.IsDead);
        PedList.PoliceVehicles.RemoveAll(x => !x.Exists());
    }
    private static void CheckRecognition()
    {
        AnyPoliceCanSeePlayer = PedList.CopPeds.Any(x => x.CanSeePlayer);// && !PlayerStarsGreyedOut;

        if (AnyPoliceCanSeePlayer)
            AnyPoliceRecentlySeenPlayer = true;
        else
            AnyPoliceRecentlySeenPlayer = PedList.CopPeds.Any(x => x.SeenPlayerSince(General.MySettings.Police.PoliceRecentlySeenTime));

        AnyPoliceCanRecognizePlayer = PedList.CopPeds.Any(x => x.HasSeenPlayerFor >= TimeToRecognize || (x.CanSeePlayer && x.DistanceToPlayer <= 20f) || (x.DistanceToPlayer <= 7f && x.DistanceToPlayer > 0f));

        if (!AnyPoliceSeenPlayerThisWanted && AnyPoliceRecentlySeenPlayer)
            AnyPoliceSeenPlayerThisWanted = true;

        if (!AnyPoliceSeenPlayerThisWanted)
        {
           // Debugging.WriteToLog("CheckRecognition", "PlacePlayerLastSeen1");
            PlacePlayerLastSeen = PlaceWantedStarted;
        }
        else if (!PlayerStarsGreyedOut)//(AnyPoliceRecentlySeenPlayer || !PlayerStarsGreyedOut)
        {
           // Debugging.WriteToLog("CheckRecognition", "PlacePlayerLastSeen1");
            PlacePlayerLastSeen = Game.LocalPlayer.Character.Position;
        }
        //else if (PlayerStarsGreyedOut && Game.LocalPlayer.Character.DistanceTo2D(PlacePlayerLastSeen) <= 15f && !Game.LocalPlayer.Character.Position.Z.IsWithin(PlacePlayerLastSeen.Z - 2f, PlacePlayerLastSeen.Z + 2f))//within the search zone but above or below it, need to help out my cops
        //    SearchModeStopping.StopSearchModeSingle();

        NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, PlacePlayerLastSeen.X, PlacePlayerLastSeen.Y, PlacePlayerLastSeen.Z);

    }
    private static void GetPoliceState()
    {
        if (PlayerState.PlayerWantedLevel == 0)
            CurrentPoliceState = PoliceState.Normal;//Default state

        if (CurrentPoliceState == PoliceState.ArrestedWait || CurrentPoliceState == PoliceState.DeadlyChase)
            return;

        if (PlayerState.PlayerWantedLevel >= 1 && PlayerState.PlayerWantedLevel <= 3 && AnyPoliceCanSeePlayer)//AnyCanSeePlayer)
        {
            bool IsDeadly = CurrentCrimes.LethalForceAuthorized;
            if (!IsDeadly && !PlayerState.PlayerIsConsideredArmed) // Unarmed and you havent killed anyone
                CurrentPoliceState = PoliceState.UnarmedChase;
            else if (!IsDeadly)
                CurrentPoliceState = PoliceState.CautiousChase;
            else
                CurrentPoliceState = PoliceState.DeadlyChase;
        }
        else if (PlayerState.PlayerWantedLevel >= 1 && PlayerState.PlayerWantedLevel <= 3)
        {
            CurrentPoliceState = PoliceState.UnarmedChase;
        }
        else if (PlayerState.PlayerWantedLevel >= 4)
            CurrentPoliceState = PoliceState.DeadlyChase;

    }
    private static void TrackedVehiclesTick()
    {
        PlayerState.TrackedVehicles.RemoveAll(x => !x.VehicleEnt.Exists());
        if (PlayerState.PlayerIsNotWanted)
        {
            foreach (GTAVehicle StolenCar in PlayerState.TrackedVehicles.Where(x => x.NeedsToBeReportedStolen))
            {
                StolenCar.WasReportedStolen = true;
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.ReportStolenVehicle, 10)
                {
                    ResultsInStolenCarSpotted = true,
                    VehicleToReport = StolenCar
                });
            }
        }
        if (PlayerState.PlayerInVehicle && Game.LocalPlayer.Character.IsInAnyVehicle(false))//first check is cheaper, but second is required to verify
        {
            if (PlayerState.PlayersCurrentTrackedVehicle == null)
                return;

            if (AnyPoliceCanSeePlayer && PlayerState.PlayerIsWanted && !PlayerStarsGreyedOut)
            {
                if (LastSeenVehicleHandle != 0 && LastSeenVehicleHandle != PlayerState.PlayersCurrentTrackedVehicle.VehicleEnt.Handle && !PlayerState.PlayersCurrentTrackedVehicle.HasBeenDescribedByDispatch)
                {
                    GameTimeLastReportedSpotted = Game.GameTime;
                    DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.SuspectChangedVehicle, 21) { IsAmbient = true, VehicleToReport = PlayerState.PlayersCurrentTrackedVehicle });
                }


                LastSeenVehicleHandle = PlayerState.PlayersCurrentTrackedVehicle.VehicleEnt.Handle;
            }
            if (AnyPoliceCanRecognizePlayer)
            {
                if (PlayerState.PlayerWantedLevel > 0 && !PlayerStarsGreyedOut)
                    UpdateVehicleDescription(PlayerState.PlayersCurrentTrackedVehicle);
            }
        }
    }
    private static void WantedLevelTick()
    {
        if (PreviousWantedLevel != Game.LocalPlayer.WantedLevel)
            WantedLevelChanged();

        if (PrevPoliceState != CurrentPoliceState)
            PoliceStateChanged();

        if (PlayerState.PlayerIsWanted)
        {
            Vector3 CurrentWantedCenter = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
            if (CurrentWantedCenter != Vector3.Zero)
            {
                LastWantedCenterPosition = CurrentWantedCenter;
                AddUpdateCurrentWantedBlip(CurrentWantedCenter);
            }
            if (General.MySettings.Police.WantedLevelIncreasesOverTime && PlayerHasBeenWantedFor > General.MySettings.Police.WantedLevelIncreaseTime && AnyPoliceCanSeePlayer && PlayerState.PlayerWantedLevel <= General.MySettings.Police.WantedLevelInceaseOverTimeLimit)
            {
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.RequestBackup, 1)
                {
                    ResultsInLethalForce = Game.LocalPlayer.WantedLevel >= 3,
                    ResultingWantedLevel = PlayerState.PlayerWantedLevel + 1
                });
            }
            if (CanReportLastSeen && Game.GameTime - GameTimeLastGreyedOut > 15000 && AnyPoliceSeenPlayerThisWanted && PlayerHasBeenWantedFor > 45000 && !PedList.CopPeds.Any(x => x.DistanceToPlayer <= 150f))
            {
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.LostVisualOnSuspect, 10));
                CanReportLastSeen = false;
            }
            if (CurrentPoliceState == PoliceState.DeadlyChase && PlayerState.PlayerWantedLevel < 3)
            {
                SetWantedLevel(3, "Deadly chase requires 3+ wanted level", true);
            }


            if (!DispatchAudio.RecentAnnouncedDispatch && AnyPoliceSeenPlayerThisWanted && CanPlaySuspectSpotted && AnyPoliceCanSeePlayer&& !DispatchAudio.IsPlayingAudio)
            {
                Debugging.WriteToLog("Spotted", "Playing Spotted");
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.SuspectSpotted, 25) { IsAmbient = true,ReportedBy = DispatchAudio.ReportType.Officers });
                GameTimeLastReportedSpotted = Game.GameTime;
            }

            if (CurrentCrimes.KillingPolice.InstancesObserved >= General.MySettings.Police.PoliceKilledSurrenderLimit && PlayerState.PlayerWantedLevel < 4 && !PlayerState.IsDead && !PlayerState.IsBusted)
            {
                SetWantedLevel(4, "You killed too many cops 4 Stars", true);
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.WeaponsFree, 1));
            }

            if (CurrentCrimes.KillingPolice.InstancesObserved >= 10 && PlayerState.PlayerWantedLevel < 5 && !PlayerState.IsDead && !PlayerState.IsBusted)
            {
                SetWantedLevel(5, "You killed too many cops 5 Stars", true);
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.RequestBackup, 1));
            }
        }
        else
        {
            RemoveWantedBlips();
        }
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
    private static void DispatchTick()
    {
        SetDispatchService(false);
    }
    private static void SetDispatchService(bool ValueToSet)
    {
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceAutomobile, ValueToSet);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceHelicopter, ValueToSet);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceVehicleRequest, ValueToSet);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.SwatAutomobile, ValueToSet);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.SwatHelicopter, ValueToSet);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceRiders, ValueToSet);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceRoadBlock, ValueToSet);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceAutomobileWaitCruising, ValueToSet);
        NativeFunction.CallByName<bool>("ENABLE_DISPATCH_SERVICE", (int)DispatchType.PoliceAutomobileWaitPulledOver, ValueToSet);

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
            bool PlayDispatch = Game.LocalPlayer.Character.DistanceTo2D(InvestigationPosition) <= 550f;
            if (InvestigationBlip.Exists())
                InvestigationBlip.Delete();
            if (PlayerState.PlayerIsNotWanted)
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
        Vector3 SpawnLocation = Vector3.Zero;
        General.GetStreetPositionandHeading(InvestigationPosition, out SpawnLocation, out float Heading,false);
        if (SpawnLocation != Vector3.Zero)
            InvestigationPosition = SpawnLocation;
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

        CurrentCrimes.MaxWantedLevel = PlayerState.PlayerWantedLevel;
        WantedLevelStartTime = Game.GameTime;
        Debugging.WriteToLog("ValueChecker", String.Format("WantedLevel Changed to: {0}, Recently Set: {1}", Game.LocalPlayer.WantedLevel,RecentlySetWanted));
        PreviousWantedLevel = Game.LocalPlayer.WantedLevel;
    }
    private static void WantedLevelRemoved()
    {
        if (!PlayerState.IsDead)//they might choose the respawn as the same character, so do not replace it yet?
        {
            CurrentCrimes.GameTimeWantedEnded = Game.GameTime;
            CurrentCrimes.MaxWantedLevel = PlayerState.MaxWantedLastLife;
            if (CurrentCrimes.PlayerSeenDuringWanted && PreviousWantedLevel != 0)// && !RecentlySetWanted)//i didnt make it go to zero, the chase was lost organically
            {
                PersonOfInterest.StoreCriminalHistory(CurrentCrimes);
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.SuspectLost, 5));
            }

            ResetPoliceStats();

            Tasking.UntaskAll(false);
            Tasking.RetaskAllRandomSpawns();
        }

        RemoveWantedBlips();
    }
    private static void WantedLevelAdded()
    {
        if (!RecentlySetWanted)//randomly set by the game
        {
            if (PlayerState.PlayerWantedLevel <= 2)//let some level 3 and 4 wanted override and be set
            {
                SetWantedLevel(0, "Resetting Unknown Wanted", false);
                //GetReportedCrimeFromUnknown();
                return;
            }
        }

        PoliceInInvestigationMode = false;
        CurrentCrimes.GameTimeWantedStarted = Game.GameTime;
        CurrentCrimes.MaxWantedLevel = PlayerState.PlayerWantedLevel;
        PlaceWantedStarted = Game.LocalPlayer.Character.Position;
        GameTimeWantedStarted = Game.GameTime;
        Tasking.UntaskAllRandomSpawns(false);
    }
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
            foreach (GTACop Cop in PedList.CopPeds)
            {
                Cop.AtWantedCenterDuringSearchMode = false;
            }
            CanReportLastSeen = false;
            if (PlayerState.PlayerIsWanted && AnyPoliceSeenPlayerThisWanted && CanPlaySuspectSpotted && !DispatchAudio.IsPlayingAudio)
            {
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.SuspectSpotted, 25) { IsAmbient = true,ReportedBy = DispatchAudio.ReportType.Officers });
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
        if (CurrentPoliceState == PoliceState.Normal && !PlayerState.IsDead)
        {

        }
        if (CurrentPoliceState == PoliceState.DeadlyChase)
        {
            if (PrevPoliceState != PoliceState.ArrestedWait && !DispatchAudio.ReportedLethalForceAuthorized && !PlayerState.IsDead && !PlayerState.IsBusted)
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
    public static void SetWantedLevel(int WantedLevel, string Reason, bool UpdateRecent)
    {
        if (UpdateRecent)
            GameTimeLastSetWanted = Game.GameTime;
        if (Game.LocalPlayer.WantedLevel < WantedLevel || WantedLevel == 0)
        {
            Debugging.WriteToLog("SetWantedLevel", string.Format("Current Wanted: {0}, Desired Wanted: {1}", Game.LocalPlayer.WantedLevel, WantedLevel));
            Game.LocalPlayer.WantedLevel = WantedLevel;
            Debugging.WriteToLog("SetWantedLevel", string.Format("Manually set to: {0}: {1}", WantedLevel, Reason));

            NativeFunction.CallByName<bool>("SET_MAX_WANTED_LEVEL", WantedLevel);
        }
        //else
        //{
        //    LocalWriteToLog("SetWantedLevel", string.Format("Wanted NOT Set Current Wanted: {0}, Desired Wanted: {1}", Game.LocalPlayer.WantedLevel, WantedLevel));
        //}
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
    private static void AddUpdateInvestigationBlip(Vector3 Position,float Size)
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
    private static void RemoveWantedBlips()
    {
        LastWantedCenterPosition = Vector3.Zero;
        if (CurrentWantedCenterBlip.Exists())
            CurrentWantedCenterBlip.Delete();
    }
    public static void PlayerSeen()
    {
        PlayerLastSeenInVehicle = PlayerState.PlayerInVehicle;
        PlayerLastSeenHeading = Game.LocalPlayer.Character.Heading;
        PlayerLastSeenForwardVector = Game.LocalPlayer.Character.ForwardVector;
    }
    public static void PoliceReportedAllClear()
    {
        PoliceInInvestigationMode = false;
    }
    public static void ResetPoliceStats()
    {
        foreach (GTACop Cop in PedList.CopPeds)
        {
            Cop.HurtByPlayer = false;
        }
        CurrentCrimes = new RapSheet();
        Debugging.WriteToLog("ResetPoliceStats", "Ran (Made New Rap Sheet)");

        CurrentPoliceState = PoliceState.Normal;
        AnyPoliceSeenPlayerThisWanted = false;
        WantedLevelStartTime = 0;
        GameTimeLastWantedEnded = Game.GameTime;
        PoliceInInvestigationMode = false;
        DispatchAudio.ResetReportedItems();
    }
}

