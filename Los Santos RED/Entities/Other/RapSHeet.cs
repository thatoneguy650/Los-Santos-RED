using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RapSheet
{
    public int TimeAimedAtPolice;
    public int MaxWantedLevel = 0;

    

    public Crime KillingPolice = new Crime() { Severity = CrimeLevel.Felony, ResultsInLethalForce = true, ResultingWantedLevel = 3,  DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportOfficerDown, 1) { ResultsInLethalForce = true }, DebugName = "Police Fatality" };
    public Crime FiringWeaponNearPolice = new Crime() { Severity = CrimeLevel.Felony, ResultsInLethalForce = true, ResultingWantedLevel = 3, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportShotsFired, 2) { ResultsInLethalForce = true }, DebugName = "Shots Fired at Police" };
    public Crime AimingWeaponAtPolice = new Crime() { Severity = CrimeLevel.Felony, ResultsInLethalForce = true, ResultingWantedLevel = 3, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportThreateningWithFirearm, 3) { ResultsInLethalForce = true }, DebugName = "Aiming Weapons At Police" };
    public Crime HurtingPolice = new Crime() { Severity = CrimeLevel.Felony, ResultsInLethalForce = true, ResultingWantedLevel = 3, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportAssualtOnOfficer, 4) { ResultsInLethalForce = true }, DebugName = "Assaulting Police" };
    public Crime TrespessingOnGovtProperty = new Crime() { Severity = CrimeLevel.Felony, ResultsInLethalForce = true, ResultingWantedLevel = 3, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportTrespassingOnGovernmentProperty, 5) { ResultsInLethalForce = true }, DebugName = "Trespassing on Government Property" };
    public Crime GotInAirVehicleDuringChase = new Crime() { Severity = CrimeLevel.Felony, ResultsInLethalForce = true, ResultingWantedLevel = 4, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportStolenAirVehicle, 6) { ResultsInLethalForce = true }, DebugName = "Stealing an Air Vehicle" };

    public Crime ResistingArrest = new Crime() { Severity = CrimeLevel.Misdemeanor, ResultsInLethalForce = false, ResultingWantedLevel = 2, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportResistingArrest, 10), DebugName = "Resisting Arrest" };
    public Crime KillingCivilians = new Crime() { Severity = CrimeLevel.Misdemeanor, ResultsInLethalForce = false, ResultingWantedLevel = 2, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportCivilianKilled, 7), DebugName = "Civilian Fatality" };
    public Crime BrandishingWeapon = new Crime() { Severity = CrimeLevel.Misdemeanor, ResultsInLethalForce = false, ResultingWantedLevel = 2, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportCarryingWeapon, 8), DebugName = "Brandishing Weapon" };
    public Crime ChangingPlates = new Crime() { Severity = CrimeLevel.Misdemeanor, ResultsInLethalForce = false, ResultingWantedLevel = 2, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspiciousActivity, 9), DebugName = "Stealing License Plates" };
    public Crime GrandTheftAuto = new Crime() { Severity = CrimeLevel.Misdemeanor, ResultsInLethalForce = false, ResultingWantedLevel = 2, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportGrandTheftAuto, 10) , DebugName = "Grand Theft Auto" };
    public Crime HurtingCivilians = new Crime() { Severity = CrimeLevel.Misdemeanor, ResultsInLethalForce = false, ResultingWantedLevel = 2, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportLowLevelCiviliansInjured, 10), DebugName = "Assaulting Civilians" };
    public Crime AttemptingSuicide = new Crime() { Severity = CrimeLevel.Misdemeanor, ResultsInLethalForce = false, ResultingWantedLevel = 2, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportAttemptingSuicide, 7), DebugName = "Attempting Suicide" };



    public Crime DrivingAgainstTraffic = new Crime() { Severity = CrimeLevel.Traffic, ResultsInLethalForce = false, ResultingWantedLevel = 1, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportRecklessDriver, 10) { ResultsInStolenCarSpotted = true,IsTrafficViolation = true }, DebugName = "Driving Against Traffic" };
    public Crime DrivingOnPavement = new Crime() { Severity = CrimeLevel.Traffic, ResultsInLethalForce = false, ResultingWantedLevel = 1, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportRecklessDriver, 10) { ResultsInStolenCarSpotted = true, IsTrafficViolation = true }, DebugName = "Driving On Pavement" };
    public Crime HitPedWithCar = new Crime() { Severity = CrimeLevel.Traffic, ResultsInLethalForce = false, ResultingWantedLevel = 2, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportPedHitAndRun, 8) { ResultsInStolenCarSpotted = true, IsTrafficViolation = true }, DebugName = "Pedestrian Hit and Run" };
    public Crime HitCarWithCar = new Crime() { Severity = CrimeLevel.Traffic, ResultsInLethalForce = false, ResultingWantedLevel = 1, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportVehicleHitAndRun, 9) { ResultsInStolenCarSpotted = true, IsTrafficViolation = true }, DebugName = "Hit and Run" };
    public Crime NonRoadworthyVehicle = new Crime() { Severity = CrimeLevel.Traffic, ResultsInLethalForce = false, ResultingWantedLevel = 1, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspiciousVehicle, 10) { ResultsInStolenCarSpotted = true, IsTrafficViolation = true }, DebugName = "Non-Roadworthy Vehicle" };
    public Crime FelonySpeeding = new Crime() { Severity = CrimeLevel.Traffic, ResultsInLethalForce = false, ResultingWantedLevel = 1, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportFelonySpeeding, 10) { ResultsInStolenCarSpotted = true, IsTrafficViolation = true }, DebugName = "Speeding" };
    public Crime RunningARedLight = new Crime() { Severity = CrimeLevel.Traffic, ResultsInLethalForce = false, ResultingWantedLevel = 1, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportRunningRed, 10) { ResultsInStolenCarSpotted = true, IsTrafficViolation = true }, DebugName = "Running a Red Light" };

    public List<GTALicensePlate> WantedPlates = new List<GTALicensePlate>();
    public uint GameTimeWantedStarted;
    public uint GameTimeWantedEnded;
    public bool PlayerSeenDuringWanted = false;

    public uint GameTimeLastKilledCivilian;
    public uint GameTimeLastKilledCop;
    public bool LethalForceAuthorized
    {
        get
        {
            return GetListOfCrimes().Any(x => x.ResultsInLethalForce && x.HasBeenWitnessedByPolice);
        }
    }
    public bool CommittedAnyCrimes
    {
        get
        {
            return GetListOfCrimes().Any(x => x.HasBeenWitnessedByPolice);
        }
    }
    public List<Crime> GetListOfCrimes()
    {
        List<Crime> CrimeList = new List<Crime>() { AttemptingSuicide,ResistingArrest, KillingPolice, FiringWeaponNearPolice, AimingWeaponAtPolice, HurtingPolice, TrespessingOnGovtProperty, GotInAirVehicleDuringChase, KillingCivilians, BrandishingWeapon, ChangingPlates, GrandTheftAuto, HurtingCivilians, DrivingAgainstTraffic, DrivingOnPavement, HitPedWithCar, HitCarWithCar, NonRoadworthyVehicle, FelonySpeeding, RunningARedLight };
        return CrimeList;
    }
    public RapSheet()
    {

    }
    public void GiveCriminalHistory()
    {
        if (LosSantosRED.MyRand.Next(1, 10) <= 8)
        {
            foreach (Crime myCrime in GetListOfCrimes().Where(x => !x.HasBeenWitnessedByPolice).Take(LosSantosRED.MyRand.Next(1, 4)))
            {
                myCrime.HasBeenWitnessedByPolice = true;
                myCrime.InstancesObserved++;
                myCrime.GameTimeLastWitnessed = Game.GameTime;
                myCrime.HasBeenReportedByDispatch = true;
            }
        }
    }
    public string PrintCrimes()
    {
        string CrimeString = "";
        foreach (Crime MyCrime in GetListOfCrimes().Where(x => x.HasBeenWitnessedByPolice).OrderByDescending(x => x.Severity).Take(3))
        {
            CrimeString += string.Format("~n~{0}{1} ({2})~s~", GetColorSeverity(MyCrime.Severity),MyCrime.DebugName,MyCrime.InstancesObserved);
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
        foreach(Crime MyCrime in GetListOfCrimes().Where(x => x.HasBeenWitnessedByPolice))
        {
            CrimeString += Environment.NewLine + MyCrime.CrimeStats();
        }
        return CrimeString;
    }
    public void CheckCrimes()
    {
        if (LosSantosRED.IsBusted || LosSantosRED.IsDead)
            return;

        if (KillingCivilians.CanObserveCrime && (Civilians.RecentlyKilledCivilian(5000) || Civilians.NearMurderVictim(15f)) && Police.AnyPoliceCanSeePlayer)///HasBeenWitnessedByPolice
        {
            KillingCivilians.CrimeObserved();
        }

        if (HurtingCivilians.CanObserveCrime && Civilians.RecentlyHurtCivilian(5000) && Police.AnyPoliceCanSeePlayer)//HasBeenWitnessedByPolice
        {
            HurtingCivilians.CrimeObserved();
        }

        if (FiringWeaponNearPolice.CanObserveCrime && (LosSantosRED.PlayerRecentlyShot(3000) || Police.PlayerArtificiallyShooting) && (PoliceScanning.CopPeds.Any(x => x.RecentlySeenPlayer() || (x.DistanceToPlayer <= 45f && !Game.LocalPlayer.Character.IsCurrentWeaponSilenced)))) //if (!firedWeapon && Game.LocalPlayer.Character.IsShooting && (PoliceScanning.CopPeds.Any(x => x.canSeePlayer || x.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f))))
        {
            FiringWeaponNearPolice.CrimeObserved();
        }

        if (TrespessingOnGovtProperty.CanObserveCrime && LosSantosRED.PlayerIsWanted && PlayerLocation.PlayerCurrentZone == Zones.JAIL && Police.AnyPoliceCanSeePlayer)
        {
            TrespessingOnGovtProperty.CrimeObserved();
        }

        CheckAimingAtPolice();
        if (!AimingWeaponAtPolice.CanObserveCrime && TimeAimedAtPolice >= 100)
        {
            AimingWeaponAtPolice.CrimeObserved();
        }

        if(ResistingArrest.CanObserveCrime && !ResistingArrest.HasBeenWitnessedByPolice && LosSantosRED.PlayerIsWanted && Police.AnyPoliceCanSeePlayer && Game.LocalPlayer.Character.Speed >= 2.0f && !LosSantosRED.HandsAreUp)
        {
            ResistingArrest.CrimeObserved();
        }

        if(AttemptingSuicide.CanObserveCrime && Police.AnyPoliceCanSeePlayer && Surrendering.IsCommitingSuicide)
        {
            AttemptingSuicide.CrimeObserved();
        }

        if (BrandishingWeapon.CanObserveCrime && Police.AnyPoliceCanSeePlayer && LosSantosRED.PlayerIsConsideredArmed && Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && !LosSantosRED.PlayerInVehicle)
        {
            GTAWeapon MatchedWeapon = GTAWeapons.GetWeaponFromHash((ulong)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash);
            if (MatchedWeapon != null && MatchedWeapon.WeaponLevel >= 2)
                BrandishingWeapon.ResultingWantedLevel = MatchedWeapon.WeaponLevel;

            BrandishingWeapon.DispatchToPlay.WeaponToReport = MatchedWeapon;
            BrandishingWeapon.CrimeObserved();
        }

        if (ChangingPlates.CanObserveCrime && LicensePlateChanging.PlayerChangingPlate && Police.AnyPoliceCanSeePlayer)
        {
            ChangingPlates.CrimeObserved();
        }

        if (GrandTheftAuto.CanObserveCrime && CarStealing.PlayerBreakingIntoCar && Police.AnyPoliceCanSeePlayer)
        {
            GrandTheftAuto.CrimeObserved();
        }

        if (GrandTheftAuto.CanObserveCrime && Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.IsInAnyPoliceVehicle && Police.AnyPoliceCanSeePlayer && PoliceScanning.CopPeds.Any(x => x.DistanceToPlayer <= 17f))
        {
            GrandTheftAuto.CrimeObserved();
        }

        if (GotInAirVehicleDuringChase.CanObserveCrime && LosSantosRED.PlayerIsWanted && LosSantosRED.PlayerInVehicle && Game.LocalPlayer.Character.IsInAirVehicle)
        {
            GotInAirVehicleDuringChase.CrimeObserved();
        }
    }
    private void CheckAimingAtPolice()
    {
        if (!AimingWeaponAtPolice.HasBeenWitnessedByPolice && LosSantosRED.PlayerIsConsideredArmed && Game.LocalPlayer.IsFreeAiming && Police.AnyPoliceCanSeePlayer && PoliceScanning.CopPeds.Any(x => Game.LocalPlayer.IsFreeAimingAtEntity(x.Pedestrian)))
            TimeAimedAtPolice++;
        else
            TimeAimedAtPolice = 0;
    }
}
public class Crime
{
    public int ResultingWantedLevel = 0;
    public bool HasBeenWitnessedByPolice = false;
    public uint GameTimeLastWitnessed;
    public bool ResultsInLethalForce = false;
    public bool HasBeenReportedByDispatch = false;
    public string DebugName;
    public CrimeLevel Severity = CrimeLevel.Unknown;
    public int InstancesObserved = 0;
    public DispatchAudio.DispatchQueueItem DispatchToPlay;
    private uint GameTimeLastReported;
    private uint InstanceDuration = 20000;
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
    public Crime()
    {

    }
    public void CrimeObserved()
    {
        if ((!HasBeenReportedByDispatch || Severity == CrimeLevel.Felony) && !RecentlyReportedCrime(25000) && LosSantosRED.PlayerWantedLevel <= ResultingWantedLevel)//if (!HasBeenWitnessedByPolice && !HasBeenReportedByDispatch && InstantAction.PlayerWantedLevel <= ResultingWantedLevel)
        {
            GameTimeLastReported = Game.GameTime;
            DispatchToPlay.ResultsInLethalForce = ResultsInLethalForce;
            DispatchAudio.AddDispatchToQueue(DispatchToPlay);
        }
        HasBeenWitnessedByPolice = true;
        InstancesObserved++;
        GameTimeLastWitnessed = Game.GameTime;
        Police.SetWantedLevel(ResultingWantedLevel, DebugName,true);
        HasBeenReportedByDispatch = true;
        Debugging.WriteToLog("Crime Logged", DebugName);        
    }
    public string CrimeStats()
    {
        return string.Format("CrimeName {0}, HasBeenWitnessedByPolice: {1},GameTimeLastWitnessed {2},InstancesObserved {3},HasBeenReportedByDispatch {4}", DebugName, HasBeenWitnessedByPolice, GameTimeLastWitnessed, InstancesObserved, HasBeenReportedByDispatch);
    }
}
public enum CrimeLevel
{
    Unknown,
    Traffic,
    Misdemeanor,
    Felony
    
}
