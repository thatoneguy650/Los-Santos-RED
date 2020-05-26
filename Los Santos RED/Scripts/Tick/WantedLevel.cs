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
            if (PedSwap.JustTakenOver(10000))
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
                if (Investigation.InInvestigationMode)
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
    public enum CrimeLevel
    {
        Unknown,
        Traffic,
        Misdemeanor,
        Felony

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
                if (PedList.CopPeds.Any(x => x.CanSeePlayer))
                {
                    CurrentCrimes.FiringWeaponNearPolice.IsCurrentlyViolating = true;
                    if (CurrentCrimes.FiringWeaponNearPolice.CanObserveCrime)
                        CurrentCrimes.FiringWeaponNearPolice.CrimeObserved();
                }
            }
            else
            {
                if (PedList.CopPeds.Any(x => x.CanSeePlayer))
                {
                    CurrentCrimes.FiringWeaponNearPolice.IsCurrentlyViolating = true;
                    if (CurrentCrimes.FiringWeaponNearPolice.CanObserveCrime)
                        CurrentCrimes.FiringWeaponNearPolice.CrimeObserved();
                }
                else if (PedList.CopPeds.Any(x => x.CanHearPlayer))
                {
                    CurrentCrimes.FiringWeapon.IsCurrentlyViolating = true;
                    if (CurrentCrimes.FiringWeapon.CanObserveCrime)
                        CurrentCrimes.FiringWeapon.CrimeObserved();
                }
            }
        }
        else
        {
            CurrentCrimes.FiringWeapon.IsCurrentlyViolating = false;
            CurrentCrimes.FiringWeaponNearPolice.IsCurrentlyViolating = false;
        }


        if (Surrender.IsCommitingSuicide)
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
            GTAWeapon MatchedWeapon = Weapons.GetWeaponFromHash((ulong)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash);
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

        if (LicensePlateTheft.PlayerChangingPlate)
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
                CurrentCrimes.ResistingArrest.DispatchToPlay.VehicleToReport = PlayerState.CurrentVehicle;
            }
            CurrentCrimes.ResistingArrest.CrimeObserved();
        }

        if (CurrentCrimes.GrandTheftAuto.CanObserveCrime && Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.IsInAnyPoliceVehicle && Police.AnyCanSeePlayer && PlayerState.CurrentVehicle != null && !PlayerState.CurrentVehicle.WasReportedStolen && PedList.CopPeds.Any(x => x.DistanceToPlayer <= 17f))
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
        if (!CurrentCrimes.AimingWeaponAtPolice.HasBeenWitnessedByPolice && PlayerState.IsConsideredArmed && Game.LocalPlayer.IsFreeAiming && Police.AnyCanSeePlayer && PedList.CopPeds.Any(x => x.Pedestrian.Exists() && Game.LocalPlayer.IsFreeAimingAtEntity(x.Pedestrian)))
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
            if (General.MySettings.Police.WantedLevelIncreasesOverTime && HasBeenAtCurrentWantedLevelFor > General.MySettings.Police.WantedLevelIncreaseTime && Police.AnyCanSeePlayer && PlayerState.WantedLevel <= General.MySettings.Police.WantedLevelInceaseOverTimeLimit)
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
                DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.MilitaryDeployed, 1));
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
        Investigation.InInvestigationMode = false;
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
            NativeFunction.CallByName<bool>("SET_MAX_WANTED_LEVEL", WantedLevel);
            Debugging.WriteToLog("SetWantedLevel", string.Format("Current Wanted: {0}, Desired Wanted: {1}, {2}", Game.LocalPlayer.WantedLevel, WantedLevel, Reason));
            Game.LocalPlayer.WantedLevel = WantedLevel;
            
        }
    }
    public static void ResetPoliceStats()
    {
        foreach (Cop Cop in PedList.CopPeds)
        {
            Cop.HurtByPlayer = false;
        }
        CurrentCrimes = new RapSheet();
        Debugging.WriteToLog("ResetPoliceStats", "Ran (Made New Rap Sheet)");

        CurrentPoliceState = PoliceState.Normal;
        Police.AnySeenPlayerCurrentWanted = false;
        GameTimeWantedLevelStarted = 0;

        Investigation.InInvestigationMode = false;
        DispatchAudio.ResetReportedItems();
    }
    public class RapSheet
    {
        public int MaxWantedLevel = 0;

        public Crime KillingPolice = new Crime("Police Fatality", CrimeLevel.Felony, 3, true, new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.OfficerDown, 1, true)) { CanBeReportedMultipleTimes = true };
        public Crime FiringWeaponNearPolice = new Crime("Shots Fired at Police", CrimeLevel.Felony, 3, true, new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.ShootingAtPolice, 2, true)) { CanBeReportedMultipleTimes = true };
        public Crime AimingWeaponAtPolice = new Crime("Aiming Weapons At Police", CrimeLevel.Felony, 3, true, new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.ThreateningOfficerWithFirearm, 3, true)) { CanBeReportedMultipleTimes = true };
        public Crime HurtingPolice = new Crime("Assaulting Police", CrimeLevel.Felony, 3, true, new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.AssaultingOfficer, 4, true)) { CanBeReportedMultipleTimes = true };
        public Crime TrespessingOnGovtProperty = new Crime("Trespassing on Government Property", CrimeLevel.Felony, 3, true, new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.TrespassingOnGovernmentProperty, 5, true)) { CanBeReportedMultipleTimes = true };
        public Crime GotInAirVehicleDuringChase = new Crime("Stealing an Air Vehicle", CrimeLevel.Felony, 4, true, new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.StealingAirVehicle, 6, true)) { CanBeReportedMultipleTimes = true };
        public Crime FiringWeapon = new Crime("Firing Weapon", CrimeLevel.Misdemeanor, 2, false, new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.ShotsFired, 6)) { CanBeCalledInBySound = true, CanBeReportedMultipleTimes = true };
        public Crime KillingCivilians = new Crime("Civilian Fatality", CrimeLevel.Misdemeanor, 2, false, new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.CivlianFatality, 7)) { CiviliansCanFightIfObserved = true, CanBeReportedMultipleTimes = true };
        public Crime GrandTheftAuto = new Crime("Grand Theft Auto", CrimeLevel.Misdemeanor, 2, false, new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.GrandTheftAuto, 8)) { CiviliansCanFightIfObserved = true, CanBeReportedMultipleTimes = true };
        public Crime Mugging = new Crime("Mugging", CrimeLevel.Misdemeanor, 2, false, new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.CivilianMugged, 9)) { CiviliansCanFightIfObserved = true };
        public Crime AttemptingSuicide = new Crime("Attempting Suicide", CrimeLevel.Misdemeanor, 2, false, new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.AttemptingSuicide, 10));
        public Crime BrandishingWeapon = new Crime("Brandishing Weapon", CrimeLevel.Misdemeanor, 2, false, new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.CarryingWeapon, 11)) { CiviliansCanFightIfObserved = true };
        public Crime HitPedWithCar = new Crime("Pedestrian Hit and Run", CrimeLevel.Misdemeanor, 2, false, new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.PedestrianHitAndRun, 12)) { CiviliansCanFightIfObserved = true };
        public Crime HurtingCivilians = new Crime("Assaulting Civilians", CrimeLevel.Misdemeanor, 2, false, new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.CivilianInjury, 13)) { CiviliansCanFightIfObserved = true };
        public Crime HitCarWithCar = new Crime("Hit and Run", CrimeLevel.Misdemeanor, 1, false, new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.VehicleHitAndRun, 14) { ResultsInStolenCarSpotted = true, IsTrafficViolation = true }) { CiviliansCanFightIfObserved = true, CanBeReportedMultipleTimes = false, WillScareCivilians = false };
        public Crime ChangingPlates = new Crime("Stealing License Plates", CrimeLevel.Misdemeanor, 1, false, new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.SuspiciousActivity, 15)) { CiviliansCanFightIfObserved = true, WillScareCivilians = false };


        public Crime ResistingArrest = new Crime("Resisting Arrest", CrimeLevel.Misdemeanor, 2, false, new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.SuspectResisitingArrest, 22));

        public Crime DrivingAgainstTraffic = new Crime("Driving Against Traffic", CrimeLevel.Traffic, 1, false, new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.RecklessDriving, 17) { ResultsInStolenCarSpotted = true, IsTrafficViolation = true }) { CanBeReportedByCivilians = false, CiviliansCanFightIfObserved = false, CanBeReportedMultipleTimes = false, WillScareCivilians = false };
        public Crime DrivingOnPavement = new Crime("Driving On Pavement", CrimeLevel.Traffic, 1, false, new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.RecklessDriving, 18) { ResultsInStolenCarSpotted = true, IsTrafficViolation = true }) { CanBeReportedByCivilians = false, CiviliansCanFightIfObserved = false, CanBeReportedMultipleTimes = false, WillScareCivilians = false };
        public Crime NonRoadworthyVehicle = new Crime("Non-Roadworthy Vehicle", CrimeLevel.Traffic, 1, false, new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.SuspiciousVehicle, 19) { ResultsInStolenCarSpotted = true, IsTrafficViolation = true }) { CanBeReportedByCivilians = false, CiviliansCanFightIfObserved = false, CanBeReportedMultipleTimes = false, WillScareCivilians = false };
        public Crime FelonySpeeding = new Crime("Speeding", CrimeLevel.Traffic, 1, false, new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.FelonySpeeding, 20) { ResultsInStolenCarSpotted = true, IsTrafficViolation = true }) { CanBeReportedByCivilians = false, CiviliansCanFightIfObserved = false, CanBeReportedMultipleTimes = false, WillScareCivilians = false };
        public Crime RunningARedLight = new Crime("Running a Red Light", CrimeLevel.Traffic, 1, false, new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.RunningARedLight, 21) { ResultsInStolenCarSpotted = true, IsTrafficViolation = true }) { CanBeReportedByCivilians = false, CiviliansCanFightIfObserved = false, CanBeReportedMultipleTimes = false, WillScareCivilians = false };

        public List<LicensePlate> WantedPlates = new List<LicensePlate>();
        public uint GameTimeWantedStarted;
        public uint GameTimeWantedEnded;
        public bool PlayerSeenDuringWanted = false;

        public uint GameTimeLastKilledCivilian;
        public uint GameTimeLastKilledCop;
        public bool RecentlyReportedAnyCrime
        {
            get
            {
                if (CrimeList.Any(x => x.RecentlyReportedCrime(60000)))
                    return true;
                else
                    return false;
            }
        }
        public bool IsViolatingAnyCivilianReportedCrimes
        {
            get
            {
                return WantedLevelScript.CurrentCrimes.CrimeList.Any(x => x.IsCurrentlyViolating && x.CanBeReportedByCivilians);// && x.Severity != CrimeLevel.Traffic);
            }
        }
        public List<Crime> CurrentlyViolatingCanBeReportedByCivilians
        {
            get
            {
                return WantedLevelScript.CurrentCrimes.CrimeList.Where(x => x.IsCurrentlyViolating && x.CanBeReportedByCivilians).ToList();// && x.Severity != CrimeLevel.Traffic);
            }
        }
        public bool LethalForceAuthorized
        {
            get
            {
                return CrimeList.Any(x => x.ResultsInLethalForce && x.HasBeenWitnessedByPolice);
            }
        }
        public bool CommittedAnyCrimes
        {
            get
            {
                return CrimeList.Any(x => x.HasBeenWitnessedByPolice);
            }
        }
        public List<Crime> CrimeList
        {
            get
            {
                List<Crime> CrimeList = new List<Crime>() { FiringWeapon, Mugging, AttemptingSuicide, ResistingArrest, KillingPolice, FiringWeaponNearPolice, AimingWeaponAtPolice, HurtingPolice, TrespessingOnGovtProperty, GotInAirVehicleDuringChase, KillingCivilians, BrandishingWeapon, ChangingPlates, GrandTheftAuto, HurtingCivilians, DrivingAgainstTraffic, DrivingOnPavement, HitPedWithCar, HitCarWithCar, NonRoadworthyVehicle, FelonySpeeding, RunningARedLight };
                return CrimeList;
            }

        }
        public RapSheet()
        {

        }
        public void GiveCriminalHistory()
        {
            if (General.MyRand.Next(1, 10) <= 1)
            {
                foreach (Crime myCrime in CrimeList.Where(x => !x.HasBeenWitnessedByPolice).PickRandom(General.MyRand.Next(1, 3)))
                {
                    myCrime.HasBeenWitnessedByPolice = true;
                    myCrime.InstancesObserved++;
                    myCrime.GameTimeLastWitnessed = Game.GameTime;
                    myCrime.HasBeenReportedByDispatch = true;
                }

                GiveWeaponsForHistory();
            }
        }
        private void GiveWeaponsForHistory()
        {

            if (CrimeList.Any(x => x.HasBeenWitnessedByPolice && x.Severity == CrimeLevel.Felony))
            {
                if (General.MyRand.Next(1, 11) <= 2)
                {
                    PlayerState.GivePlayerRandomWeapon(new List<GTAWeapon.WeaponCategory>() { GTAWeapon.WeaponCategory.Pistol, GTAWeapon.WeaponCategory.Melee }.PickRandom());
                }
                else
                {
                    PlayerState.GivePlayerRandomWeapon(new List<GTAWeapon.WeaponCategory>() { GTAWeapon.WeaponCategory.Pistol, GTAWeapon.WeaponCategory.SMG, GTAWeapon.WeaponCategory.Shotgun, GTAWeapon.WeaponCategory.AR }.PickRandom());
                }
            }
            else if (CrimeList.Any(x => x.HasBeenWitnessedByPolice && x.Severity == CrimeLevel.Misdemeanor))
            {
                if (General.MyRand.Next(1, 11) <= 7)
                {
                    PlayerState.GivePlayerRandomWeapon(new List<GTAWeapon.WeaponCategory>() { GTAWeapon.WeaponCategory.Pistol, GTAWeapon.WeaponCategory.Melee }.PickRandom());
                }
            }
            else
            {
                if (General.MyRand.Next(1, 11) <= 2)
                {
                    PlayerState.GivePlayerRandomWeapon(new List<GTAWeapon.WeaponCategory>() { GTAWeapon.WeaponCategory.Pistol, GTAWeapon.WeaponCategory.Melee }.PickRandom());
                }

            }


        }
        public string PrintCrimes()
        {
            string CrimeString = "";
            foreach (Crime MyCrime in CrimeList.Where(x => x.HasBeenWitnessedByPolice).OrderByDescending(x => x.Severity).Take(3))
            {
                CrimeString += string.Format("~n~{0}{1} ({2})~s~", GetColorSeverity(MyCrime.Severity), MyCrime.Name, MyCrime.InstancesObserved);
            }
            return CrimeString;
        }
        private static string GetColorSeverity(CrimeLevel Severity)
        {
            if (Severity == CrimeLevel.Felony)
                return "~r~";
            else if (Severity == CrimeLevel.Misdemeanor)
                return "~o~";
            else if (Severity == CrimeLevel.Traffic)
                return "~y~";
            else
                return "~s~";

        }
        public string DebugPrintCrimes()
        {
            string CrimeString = "";
            foreach (Crime MyCrime in CrimeList.Where(x => x.HasBeenWitnessedByPolice))
            {
                CrimeString += Environment.NewLine + MyCrime.CrimeStats();
            }
            return CrimeString;
        }
    }
    public class Crime
    {
        public int ResultingWantedLevel = 0;
        public bool HasBeenWitnessedByPolice = false;
        public uint GameTimeLastWitnessed;
        public bool ResultsInLethalForce = false;
        public bool HasBeenReportedByDispatch = false;
        public string Name;
        public CrimeLevel Severity = CrimeLevel.Unknown;
        public int InstancesObserved = 0;
        public int TotalInstances = 0;
        public DispatchAudio.DispatchQueueItem DispatchToPlay;
        private uint GameTimeLastReported;
        public uint InstanceDuration = 20000;
        public bool IsMurder = false;
        public uint GameTimeLastCalledInByCivilians;
        public bool IsCurrentlyViolating = false;
        public bool CanBeReportedByCivilians = true;
        public bool CanBeCalledInBySound = false;
        public bool CiviliansCanFightIfObserved = false;
        public bool CanBeReportedMultipleTimes = false;
        public bool WillScareCivilians = true;
        public bool CanObserveCrime
        {
            get
            {
                if (!HasBeenWitnessedByPolice)
                    return true;
                else if (Game.GameTime - GameTimeLastWitnessed >= InstanceDuration)
                    return true;
                else
                    return false;
            }
        }
        public bool CiviliansCanReportCrimeToDispatch
        {
            get
            {
                if (PlayerState.IsWanted)
                    return false;
                else if (GameTimeLastCalledInByCivilians == 0)
                    return true;
                else if (Game.GameTime - GameTimeLastCalledInByCivilians >= 180000)
                    return true;
                else
                    return false;
            }
        }
        public bool RecentlyCommittedCrime(uint TimeSince)
        {
            if (!HasBeenWitnessedByPolice)
                return false;
            else if (Game.GameTime - GameTimeLastWitnessed <= TimeSince)
                return true;
            else
                return false;
        }
        public bool RecentlyReportedCrime(uint TimeSince)
        {
            if (!HasBeenReportedByDispatch)
                return false;
            else if (Game.GameTime - GameTimeLastReported <= TimeSince)
                return true;
            else
                return false;
        }
        public bool RecentlyCalledInByCivilians(uint TimeSince)
        {
            if (GameTimeLastCalledInByCivilians == 0)
                return false;
            else if (Game.GameTime - GameTimeLastCalledInByCivilians <= TimeSince)
                return true;
            else
                return false;
        }
        public Crime()
        {

        }
        public Crime(string _Name, CrimeLevel _Severity, int _ResultingWantedLevel, bool _ResultsInLethalForce, DispatchAudio.DispatchQueueItem _DispatchToPlay)
        {
            Severity = _Severity;
            ResultsInLethalForce = _ResultsInLethalForce;
            ResultingWantedLevel = _ResultingWantedLevel;
            DispatchToPlay = _DispatchToPlay;
            Name = _Name;
        }
        public void CrimeObserved()
        {
            if ((!HasBeenReportedByDispatch || Severity == CrimeLevel.Felony) && !RecentlyReportedCrime(25000) && PlayerState.WantedLevel <= ResultingWantedLevel)//if (!HasBeenWitnessedByPolice && !HasBeenReportedByDispatch && InstantAction.PlayerWantedLevel <= ResultingWantedLevel)
            {
                GameTimeLastReported = Game.GameTime;
                DispatchToPlay.ResultsInLethalForce = ResultsInLethalForce;
                DispatchToPlay.ReportedBy = DispatchAudio.ReportType.Officers;
                DispatchAudio.AddDispatchToQueue(DispatchToPlay);
            }
            else if (!WantedLevelScript.CurrentCrimes.RecentlyReportedAnyCrime && CanBeReportedMultipleTimes && !RecentlyReportedCrime(25000))//if (!HasBeenWitnessedByPolice && !HasBeenReportedByDispatch && InstantAction.PlayerWantedLevel <= ResultingWantedLevel)
            {
                Debugging.WriteToLog("Crime Logged No Dispatch Activity", Name);
                GameTimeLastReported = Game.GameTime;
                DispatchToPlay.ResultsInLethalForce = ResultsInLethalForce;
                DispatchToPlay.ReportedBy = DispatchAudio.ReportType.Officers;
                DispatchAudio.AddDispatchToQueue(DispatchToPlay);
            }
            int CopsToRadio = 0;
            foreach (Cop Cop in PedList.CopPeds.Where(x => x.Pedestrian.Exists() && x.RecentlySeenPlayer()).OrderBy(x => x.DistanceToPlayer).Take(2))
            {
                if (CopsToRadio == 0)
                {
                    Cop.HasItemsToRadioIn = true;
                    CopsToRadio++;
                }
                else if (General.RandomPercent(40))
                {
                    Cop.HasItemsToRadioIn = true;
                    CopsToRadio++;
                }
            }
            HasBeenWitnessedByPolice = true;
            InstancesObserved++;
            GameTimeLastWitnessed = Game.GameTime;
            WantedLevelScript.SetWantedLevel(ResultingWantedLevel, Name, true);
            HasBeenReportedByDispatch = true;
            Debugging.WriteToLog("Crime Logged", Name);
        }
        private void PickCopToRadioIn()
        {
            General.RequestAnimationDictionay("random@arrests");

            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "random@arrests", "generic_radio_enter", 2.0f, -2.0f, -1, 48, 0, false, false, false);
        }
        public string CrimeStats()
        {
            return string.Format("CrimeName {0}, HasBeenWitnessedByPolice: {1},GameTimeLastWitnessed {2},InstancesObserved {3},HasBeenReportedByDispatch {4}", Name, HasBeenWitnessedByPolice, GameTimeLastWitnessed, InstancesObserved, HasBeenReportedByDispatch);
        }
    }



}

