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

public static class WantedLevelScript
{
    private static PoliceState PrevPoliceState;
    private static uint GameTimePoliceStateStart;
    private static uint GameTimeLastSetWanted;
    private static uint GameTimeLastReportedSpotted;
    private static uint GameTimeWantedStarted;
    private static uint GameTimeWantedLevelStarted;
    private static uint GameTimeLastWantedEnded;
    private static uint GameTimeStartedBrandishing;
    public static int TimeAimedAtPolice;
    private static bool CanReportLastSeen;
    private static Blip CurrentWantedCenterBlip;
    private static int PreviousWantedLevel;
    
    public static bool IsRunning { get; set; } = true;
    public static RapSheet CurrentCrimes { get; set; } 
    public static Vector3 LastWantedCenterPosition { get; set; }
    public static PoliceState CurrentPoliceState { get; set; }
    public static PoliceState LastPoliceState { get; private set; }
    public static Vector3 PlaceWantedStarted { get; private set; }
    public static uint HasBeenNotWantedFor
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
    public static uint HasBeenWantedFor
    {
        get
        {
            if (Game.LocalPlayer.WantedLevel == 0)
                return 0;
            else
                return (Game.GameTime - GameTimeWantedStarted);
        }
    }
    public static uint HasBeenAtCurrentWantedLevelFor
    {
        get
        {
            if (Game.LocalPlayer.WantedLevel == 0)
                return 0;
            else
                return (Game.GameTime - GameTimeWantedLevelStarted);
        }
    }
    public static uint HasBeenAtCurrentPoliceStateFor
    {
        get
        {
             return (Game.GameTime - GameTimePoliceStateStart);
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
    public static bool NearLastWanted(float DistanceTo)
    {
        return LastWantedCenterPosition != Vector3.Zero && Game.LocalPlayer.Character.DistanceTo2D(LastWantedCenterPosition) <= DistanceTo;
    }
    public static ResponsePriority CurrentResponse
    {
        get
        {
            if (PlayerState.IsNotWanted)
            {
                if (InvestigationScript.InInvestigationMode)
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
                if (PlayerState.WantedLevel > 4)
                {
                    return ResponsePriority.Full;
                }
                else if (PlayerState.WantedLevel >= 2)
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
        LastWantedCenterPosition = default;
        CanReportLastSeen = false;
        PlaceWantedStarted = default;
        CurrentWantedCenterBlip = default;
        GameTimePoliceStateStart = 0;
        GameTimeLastSetWanted = 0;
        PrevPoliceState = PoliceState.Normal;
        CurrentPoliceState = PoliceState.Normal;
        LastPoliceState = PoliceState.Normal;
        PreviousWantedLevel = 0;
        GameTimeLastReportedSpotted = 0;
        IsRunning = true;
        SetWantedLevel(0, "Initial", true);
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            GetPoliceState();
            CheckCrimes();
            WantedLevelTick();
        }
    }
    private static void GetPoliceState()
    {
        if (PlayerState.WantedLevel == 0)
            CurrentPoliceState = PoliceState.Normal;//Default state

        if (CurrentPoliceState == PoliceState.ArrestedWait || CurrentPoliceState == PoliceState.DeadlyChase)
            return;

        if (PlayerState.WantedLevel >= 1 && PlayerState.WantedLevel <= 3 && Police.AnyCanSeePlayer)
        {
            bool IsDeadly = CurrentCrimes.LethalForceAuthorized;
            if (!IsDeadly && !PlayerState.IsConsideredArmed) // Unarmed and you havent killed anyone
                CurrentPoliceState = PoliceState.UnarmedChase;
            else if (!IsDeadly)
                CurrentPoliceState = PoliceState.CautiousChase;
            else
                CurrentPoliceState = PoliceState.DeadlyChase;
        }
        else if (PlayerState.WantedLevel >= 1 && PlayerState.WantedLevel <= 3)
        {
            CurrentPoliceState = PoliceState.UnarmedChase;
        }
        else if (PlayerState.WantedLevel >= 4)
            CurrentPoliceState = PoliceState.DeadlyChase;

    }
    private static void PoliceStateChanged()
    {
        Debugging.WriteToLog("ValueChecker", string.Format("PoliceState Changed to: {0} Was {1}", CurrentPoliceState, PrevPoliceState));
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
    private static void CheckCrimes()
    {
        if (PlayerState.IsBusted || PlayerState.IsDead)
            return;


        if (PedWounds.RecentlyKilledCop(5000))
        {
            CurrentCrimes.KillingPolice.IsCurrentlyViolating = true;
            if (CurrentCrimes.KillingPolice.CanObserveCrime)
                CurrentCrimes.KillingPolice.CrimeObserved();
        }
        else
        {
            CurrentCrimes.KillingPolice.IsCurrentlyViolating = false;
        }

        if (PedWounds.RecentlyHurtCop(5000))
        {
            CurrentCrimes.HurtingPolice.IsCurrentlyViolating = true;
            if (CurrentCrimes.HurtingPolice.CanObserveCrime)
                CurrentCrimes.HurtingPolice.CrimeObserved();
        }
        else
        {
            CurrentCrimes.HurtingPolice.IsCurrentlyViolating = false;
        }


        if (PedWounds.RecentlyKilledCivilian(5000) || PedWounds.NearCivilianMurderVictim(9f))
        {
            CurrentCrimes.KillingCivilians.IsCurrentlyViolating = true;
            if (CurrentCrimes.KillingCivilians.CanObserveCrime && Police.AnyCanSeePlayer)
                CurrentCrimes.KillingCivilians.CrimeObserved();
        }
        else
        {
            CurrentCrimes.KillingCivilians.IsCurrentlyViolating = false;
        }

        if (PedWounds.RecentlyHurtCivilian(5000))
        {
            CurrentCrimes.HurtingCivilians.IsCurrentlyViolating = true;
            if (CurrentCrimes.HurtingCivilians.CanObserveCrime && Police.AnyCanSeePlayer)
                CurrentCrimes.HurtingCivilians.CrimeObserved();
        }
        else
        {
            CurrentCrimes.HurtingCivilians.IsCurrentlyViolating = false;
        }

        if (PlayerState.RecentlyShot(5000) || Game.LocalPlayer.Character.IsShooting)
        {
            CurrentCrimes.FiringWeapon.IsCurrentlyViolating = true;
            if (Game.LocalPlayer.Character.IsCurrentWeaponSilenced)
            {
                if (PedList.CopPeds.Any(x => x.RecentlySeenPlayer()))
                {
                    CurrentCrimes.FiringWeaponNearPolice.IsCurrentlyViolating = true;
                    if (CurrentCrimes.FiringWeaponNearPolice.CanObserveCrime)
                        CurrentCrimes.FiringWeaponNearPolice.CrimeObserved();
                }
            }
            else
            {
                if (PedList.CopPeds.Any(x => x.RecentlySeenPlayer() || x.DistanceToPlayer <= 45f))
                {
                    CurrentCrimes.FiringWeaponNearPolice.IsCurrentlyViolating = true;
                    if (CurrentCrimes.FiringWeaponNearPolice.CanObserveCrime)
                        CurrentCrimes.FiringWeaponNearPolice.CrimeObserved();
                }
            }
        }
        else
        {
            CurrentCrimes.FiringWeapon.IsCurrentlyViolating = false;
            CurrentCrimes.FiringWeaponNearPolice.IsCurrentlyViolating = false;
        }


        if (Surrendering.IsCommitingSuicide)
        {
            CurrentCrimes.AttemptingSuicide.IsCurrentlyViolating = true;
            if (CurrentCrimes.AttemptingSuicide.CanObserveCrime && Police.AnyCanSeePlayer)
                CurrentCrimes.AttemptingSuicide.CrimeObserved();
        }
        else
        {
            CurrentCrimes.AttemptingSuicide.IsCurrentlyViolating = false;
        }

        bool IsBrandishing = false;
        if (PlayerState.IsConsideredArmed)
        {
            if (GameTimeStartedBrandishing == 0)
                GameTimeStartedBrandishing = Game.GameTime;
        }
        else
        {
            GameTimeStartedBrandishing = 0;
        }

        if (GameTimeStartedBrandishing > 0 && Game.GameTime - GameTimeStartedBrandishing >= 1500)
            IsBrandishing = true;

        if (IsBrandishing && Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && !PlayerState.IsInVehicle)
        {
            CurrentCrimes.BrandishingWeapon.IsCurrentlyViolating = true;
            GTAWeapon MatchedWeapon = GTAWeapons.GetWeaponFromHash((ulong)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash);
            if (MatchedWeapon != null && MatchedWeapon.WeaponLevel >= 2)
                CurrentCrimes.BrandishingWeapon.ResultingWantedLevel = MatchedWeapon.WeaponLevel;
            else
                CurrentCrimes.BrandishingWeapon.ResultingWantedLevel = 1;

            CurrentCrimes.BrandishingWeapon.DispatchToPlay.WeaponToReport = MatchedWeapon;

            if (CurrentCrimes.BrandishingWeapon.CanObserveCrime && Police.AnyCanSeePlayer)
                CurrentCrimes.BrandishingWeapon.CrimeObserved();
        }
        else
        {
            CurrentCrimes.BrandishingWeapon.IsCurrentlyViolating = false;
        }

        if (PlayerState.IsBreakingIntoCar)
        {
            CurrentCrimes.GrandTheftAuto.IsCurrentlyViolating = true;
            if (CurrentCrimes.GrandTheftAuto.CanObserveCrime && Police.AnyCanSeePlayer)
                CurrentCrimes.GrandTheftAuto.CrimeObserved();
        }
        else
        {
            CurrentCrimes.GrandTheftAuto.IsCurrentlyViolating = false;
        }

        if (LicensePlateChanging.PlayerChangingPlate)
        {
            CurrentCrimes.ChangingPlates.IsCurrentlyViolating = true;
            if (CurrentCrimes.ChangingPlates.CanObserveCrime && Police.AnyCanSeePlayer)
                CurrentCrimes.ChangingPlates.CrimeObserved();
        }
        else
        {
            CurrentCrimes.ChangingPlates.IsCurrentlyViolating = false;
        }


        if (MuggingScript.IsMugging)
        {
            CurrentCrimes.Mugging.IsCurrentlyViolating = true;
            if (CurrentCrimes.Mugging.CanObserveCrime && Police.AnyCanSeePlayer)
                CurrentCrimes.Mugging.CrimeObserved();
        }
        else
        {
            CurrentCrimes.Mugging.IsCurrentlyViolating = false;
        }

        //Police Only
        if (CurrentCrimes.TrespessingOnGovtProperty.CanObserveCrime && PlayerState.IsWanted && PlayerLocation.PlayerCurrentZone.IsRestrictedDuringWanted && Police.AnyCanSeePlayer)
        {
            CurrentCrimes.TrespessingOnGovtProperty.CrimeObserved();
        }

        CheckAimingAtPolice();

        if (!CurrentCrimes.AimingWeaponAtPolice.CanObserveCrime && TimeAimedAtPolice >= 10)
        {
            CurrentCrimes.AimingWeaponAtPolice.CrimeObserved();
        }

        if (CurrentCrimes.ResistingArrest.CanObserveCrime && !CurrentCrimes.ResistingArrest.HasBeenWitnessedByPolice && PlayerState.IsWanted && Police.AnyCanSeePlayer && Game.LocalPlayer.Character.Speed >= 2.0f && !PlayerState.HandsAreUp && WantedLevelScript.HasBeenWantedFor >= 15000)
        {
            bool InVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            CurrentCrimes.ResistingArrest.DispatchToPlay.SuspectStatusOnFoot = true;
            if (InVehicle)
            {
                CurrentCrimes.ResistingArrest.DispatchToPlay.SuspectStatusOnFoot = false;
                CurrentCrimes.ResistingArrest.DispatchToPlay.VehicleToReport = PlayerState.GetCurrentVehicle();
            }
            CurrentCrimes.ResistingArrest.CrimeObserved();
        }

        if (CurrentCrimes.GrandTheftAuto.CanObserveCrime && Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.IsInAnyPoliceVehicle && Police.AnyCanSeePlayer && PlayerState.GetCurrentVehicle() != null && !PlayerState.GetCurrentVehicle().WasReportedStolen && PedList.CopPeds.Any(x => x.DistanceToPlayer <= 17f))
        {
            CurrentCrimes.GrandTheftAuto.CrimeObserved();
        }

        if (CurrentCrimes.GotInAirVehicleDuringChase.CanObserveCrime && PlayerState.IsWanted && PlayerState.IsInVehicle && Game.LocalPlayer.Character.IsInAirVehicle)
        {
            CurrentCrimes.GotInAirVehicleDuringChase.CrimeObserved();
        }
    }
    private static void CheckAimingAtPolice()
    {
        if (!CurrentCrimes.AimingWeaponAtPolice.HasBeenWitnessedByPolice && PlayerState.IsConsideredArmed && Game.LocalPlayer.IsFreeAiming && Police.AnyCanSeePlayer && PedList.CopPeds.Any(x => Game.LocalPlayer.IsFreeAimingAtEntity(x.Pedestrian)))
            TimeAimedAtPolice++;
        else
            TimeAimedAtPolice = 0;
    }
    private static void WantedLevelTick()
    {
        if (PreviousWantedLevel != Game.LocalPlayer.WantedLevel)
            WantedLevelChanged();

        if (PrevPoliceState != CurrentPoliceState)
            PoliceStateChanged();

        if (PlayerState.IsWanted)
        {
            Vector3 CurrentWantedCenter = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
            if (CurrentWantedCenter != Vector3.Zero)
            {
                LastWantedCenterPosition = CurrentWantedCenter;
                AddUpdateCurrentWantedBlip(CurrentWantedCenter);
            }
            if (General.MySettings.Police.WantedLevelIncreasesOverTime && HasBeenWantedFor > General.MySettings.Police.WantedLevelIncreaseTime && Police.AnyCanSeePlayer && PlayerState.WantedLevel <= General.MySettings.Police.WantedLevelInceaseOverTimeLimit)
            {
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.RequestBackup, 1)
                {
                    ResultsInLethalForce = Game.LocalPlayer.WantedLevel >= 3,
                    ResultingWantedLevel = PlayerState.WantedLevel + 1
                });
            }
            if (CanReportLastSeen && PlayerState.StarsRecentlyGreyedOut && Police.AnySeenPlayerCurrentWanted && HasBeenWantedFor > 45000 && !PedList.CopPeds.Any(x => x.DistanceToPlayer <= 150f))
            {
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.LostVisualOnSuspect, 10));
                CanReportLastSeen = false;
            }
            if (CurrentPoliceState == PoliceState.DeadlyChase && PlayerState.WantedLevel < 3)
            {
                SetWantedLevel(3, "Deadly chase requires 3+ wanted level", true);
            }


            if (!DispatchAudio.RecentAnnouncedDispatch && Police.AnySeenPlayerCurrentWanted && CanPlaySuspectSpotted && Police.AnyCanSeePlayer && !DispatchAudio.IsPlayingAudio)
            {
                Debugging.WriteToLog("Spotted", "Playing Spotted");
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.SuspectSpotted, 25) { IsAmbient = true, ReportedBy = DispatchAudio.ReportType.Officers });
                GameTimeLastReportedSpotted = Game.GameTime;
            }

            if (CurrentCrimes.KillingPolice.InstancesObserved >= General.MySettings.Police.PoliceKilledSurrenderLimit && PlayerState.WantedLevel < 4 && !PlayerState.IsDead && !PlayerState.IsBusted)
            {
                SetWantedLevel(4, "You killed too many cops 4 Stars", true);
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.WeaponsFree, 1));
            }

            if (CurrentCrimes.KillingPolice.InstancesObserved >= 10 && PlayerState.WantedLevel < 5 && !PlayerState.IsDead && !PlayerState.IsBusted)
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
    private static void WantedLevelChanged()
    {
        if (Game.LocalPlayer.WantedLevel == 0)
        {
            WantedLevelRemoved();
        }
        else if (PreviousWantedLevel == 0 && Game.LocalPlayer.WantedLevel > 0)
        {
            WantedLevelAdded();
        }

        CurrentCrimes.MaxWantedLevel = PlayerState.WantedLevel;
        GameTimeWantedLevelStarted = Game.GameTime;
        Debugging.WriteToLog("ValueChecker", string.Format("WantedLevel Changed to: {0}, Recently Set: {1}", Game.LocalPlayer.WantedLevel, RecentlySetWanted));
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
        }

        GameTimeLastWantedEnded = Game.GameTime;
        RemoveWantedBlips();
    }
    private static void WantedLevelAdded()
    {
        if (!RecentlySetWanted)//randomly set by the game
        {
            if (PlayerState.WantedLevel <= 2)//let some level 3 and 4 wanted override and be set
            {
                SetWantedLevel(0, "Resetting Unknown Wanted", false);
                return;
            }
        }
        InvestigationScript.InInvestigationMode = false;
        CurrentCrimes.GameTimeWantedStarted = Game.GameTime;
        CurrentCrimes.MaxWantedLevel = PlayerState.WantedLevel;
        PlaceWantedStarted = Game.LocalPlayer.Character.Position;
        GameTimeWantedStarted = Game.GameTime;
    }
    private static void RemoveWantedBlips()
    {
        LastWantedCenterPosition = Vector3.Zero;
        if (CurrentWantedCenterBlip.Exists())
            CurrentWantedCenterBlip.Delete();
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
            General.CreatedBlips.Add(CurrentWantedCenterBlip);
        }
        if (CurrentWantedCenterBlip.Exists())
            CurrentWantedCenterBlip.Position = Position;
    }
    public static void SetWantedLevel(int WantedLevel, string Reason, bool UpdateRecent)
    {
        if (UpdateRecent)
            GameTimeLastSetWanted = Game.GameTime;

        if (Game.LocalPlayer.WantedLevel < WantedLevel || WantedLevel == 0)
        {
            Debugging.WriteToLog("SetWantedLevel", string.Format("Current Wanted: {0}, Desired Wanted: {1}, {2}", Game.LocalPlayer.WantedLevel, WantedLevel, Reason));
            Game.LocalPlayer.WantedLevel = WantedLevel;
            NativeFunction.CallByName<bool>("SET_MAX_WANTED_LEVEL", WantedLevel);
        }
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
        Police.AnySeenPlayerCurrentWanted = false;
        GameTimeWantedLevelStarted = 0;

        InvestigationScript.InInvestigationMode = false;
        DispatchAudio.ResetReportedItems();
    }

}

