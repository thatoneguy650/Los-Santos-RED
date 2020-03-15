using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RapSheet
{
    private uint GameTimeStartedBrandishing;
    public int TimeAimedAtPolice;
    public int MaxWantedLevel = 0;

    public Crime KillingPolice = new Crime() { Severity = CrimeLevel.Felony, ResultsInLethalForce = true, ResultingWantedLevel = 3, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportOfficerDown, 1) { ResultsInLethalForce = true }, DebugName = "Police Fatality"  };
    public Crime FiringWeaponNearPolice = new Crime() { Severity = CrimeLevel.Felony, ResultsInLethalForce = true, ResultingWantedLevel = 3, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportShotsFiredAtPolice, 2) { ResultsInLethalForce = true }, DebugName = "Shots Fired at Police" };
    public Crime AimingWeaponAtPolice = new Crime() { Severity = CrimeLevel.Felony, ResultsInLethalForce = true, ResultingWantedLevel = 3, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportThreateningOfficerWithFirearm, 3) { ResultsInLethalForce = true }, DebugName = "Aiming Weapons At Police" };
    public Crime HurtingPolice = new Crime() { Severity = CrimeLevel.Felony, ResultsInLethalForce = true, ResultingWantedLevel = 3, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportAssualtOnOfficer, 4) { ResultsInLethalForce = true }, DebugName = "Assaulting Police" };
    public Crime TrespessingOnGovtProperty = new Crime() { Severity = CrimeLevel.Felony, ResultsInLethalForce = true, ResultingWantedLevel = 3, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportTrespassingOnGovernmentProperty, 5) { ResultsInLethalForce = true }, DebugName = "Trespassing on Government Property" };
    public Crime GotInAirVehicleDuringChase = new Crime() { Severity = CrimeLevel.Felony, ResultsInLethalForce = true, ResultingWantedLevel = 4, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportStolenAirVehicle, 6) { ResultsInLethalForce = true }, DebugName = "Stealing an Air Vehicle" };

    public Crime ResistingArrest = new Crime() { Severity = CrimeLevel.Misdemeanor, ResultsInLethalForce = false, ResultingWantedLevel = 2, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportResistingArrest, 10), DebugName = "Resisting Arrest" };
    public Crime KillingCivilians = new Crime() { Severity = CrimeLevel.Misdemeanor, ResultsInLethalForce = false, ResultingWantedLevel = 2, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportCivilianFatality, 7), DebugName = "Civilian Fatality" };
    public Crime BrandishingWeapon = new Crime() { Severity = CrimeLevel.Misdemeanor, ResultsInLethalForce = false, ResultingWantedLevel = 2, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportCarryingWeapon, 8), DebugName = "Brandishing Weapon" };
    public Crime ChangingPlates = new Crime() { Severity = CrimeLevel.Misdemeanor, ResultsInLethalForce = false, ResultingWantedLevel = 2, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspiciousActivity, 9), DebugName = "Stealing License Plates" };
    public Crime GrandTheftAuto = new Crime() { Severity = CrimeLevel.Misdemeanor, ResultsInLethalForce = false, ResultingWantedLevel = 2, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportGrandTheftAuto, 7) , DebugName = "Grand Theft Auto" };
    public Crime HurtingCivilians = new Crime() { Severity = CrimeLevel.Misdemeanor, ResultsInLethalForce = false, ResultingWantedLevel = 2, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportCivilianInjury, 10), DebugName = "Assaulting Civilians" };
    public Crime AttemptingSuicide = new Crime() { Severity = CrimeLevel.Misdemeanor, ResultsInLethalForce = false, ResultingWantedLevel = 2, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportAttemptingSuicide, 7), DebugName = "Attempting Suicide" };
    public Crime Mugging = new Crime() { Severity = CrimeLevel.Misdemeanor, ResultsInLethalForce = false, ResultingWantedLevel = 2, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportMugging, 7), DebugName = "Mugging" };
    public Crime FiringWeapon = new Crime() { Severity = CrimeLevel.Misdemeanor, ResultsInLethalForce = false, ResultingWantedLevel = 2, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportShotsFired, 6), DebugName = "Firing Weapon", CanBeCalledInBySound = true };


    public Crime HitPedWithCar = new Crime() { Severity = CrimeLevel.Misdemeanor, ResultsInLethalForce = false, ResultingWantedLevel = 2, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportPedHitAndRun, 8) { ResultsInStolenCarSpotted = true, IsTrafficViolation = true }, DebugName = "Pedestrian Hit and Run" };
    public Crime HitCarWithCar = new Crime() { Severity = CrimeLevel.Misdemeanor, ResultsInLethalForce = false, ResultingWantedLevel = 1, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportVehicleHitAndRun, 9) { ResultsInStolenCarSpotted = true, IsTrafficViolation = true }, DebugName = "Hit and Run" };


    public Crime DrivingAgainstTraffic = new Crime() { Severity = CrimeLevel.Traffic, ResultsInLethalForce = false, ResultingWantedLevel = 1, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportRecklessDriver, 10) { ResultsInStolenCarSpotted = true,IsTrafficViolation = true }, DebugName = "Driving Against Traffic", CanBeReportedByCivilians = false };
    public Crime DrivingOnPavement = new Crime() { Severity = CrimeLevel.Traffic, ResultsInLethalForce = false, ResultingWantedLevel = 1, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportRecklessDriver, 10) { ResultsInStolenCarSpotted = true, IsTrafficViolation = true }, DebugName = "Driving On Pavement", CanBeReportedByCivilians = false };
    public Crime NonRoadworthyVehicle = new Crime() { Severity = CrimeLevel.Traffic, ResultsInLethalForce = false, ResultingWantedLevel = 1, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspiciousVehicle, 10) { ResultsInStolenCarSpotted = true, IsTrafficViolation = true }, DebugName = "Non-Roadworthy Vehicle", CanBeReportedByCivilians = false };
    public Crime FelonySpeeding = new Crime() { Severity = CrimeLevel.Traffic, ResultsInLethalForce = false, ResultingWantedLevel = 1, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportFelonySpeeding, 10) { ResultsInStolenCarSpotted = true, IsTrafficViolation = true }, DebugName = "Speeding", CanBeReportedByCivilians = false };
    public Crime RunningARedLight = new Crime() { Severity = CrimeLevel.Traffic, ResultsInLethalForce = false, ResultingWantedLevel = 1, DispatchToPlay = new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportRunningRed, 10) { ResultsInStolenCarSpotted = true, IsTrafficViolation = true }, DebugName = "Running a Red Light", CanBeReportedByCivilians = false };

    public List<GTALicensePlate> WantedPlates = new List<GTALicensePlate>();
    public uint GameTimeWantedStarted;
    public uint GameTimeWantedEnded;
    public bool PlayerSeenDuringWanted = false;

    public uint GameTimeLastKilledCivilian;
    public uint GameTimeLastKilledCop;
    public bool IsViolatingAnyCivilianReportedCrimes
    {
        get
        {
            return Police.CurrentCrimes.GetListOfCrimes().Any(x => x.IsCurrentlyViolating && x.CanBeReportedByCivilians);// && x.Severity != CrimeLevel.Traffic);
        }
    }
    public List<Crime> CurrentlyViolatingCanBeReportedByCivilians
    {
        get
        {
            return Police.CurrentCrimes.GetListOfCrimes().Where(x => x.IsCurrentlyViolating && x.CanBeReportedByCivilians).ToList();// && x.Severity != CrimeLevel.Traffic);
        }
    }
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
        List<Crime> CrimeList = new List<Crime>() { FiringWeapon,Mugging, AttemptingSuicide, ResistingArrest, KillingPolice, FiringWeaponNearPolice, AimingWeaponAtPolice, HurtingPolice, TrespessingOnGovtProperty, GotInAirVehicleDuringChase, KillingCivilians, BrandishingWeapon, ChangingPlates, GrandTheftAuto, HurtingCivilians, DrivingAgainstTraffic, DrivingOnPavement, HitPedWithCar, HitCarWithCar, NonRoadworthyVehicle, FelonySpeeding, RunningARedLight };
        return CrimeList;
    }
    public RapSheet()
    {

    }
    public void GiveCriminalHistory()
    {
        if (LosSantosRED.MyRand.Next(1, 10) <= 1)
        {
            foreach (Crime myCrime in GetListOfCrimes().Where(x => !x.HasBeenWitnessedByPolice).PickRandom(LosSantosRED.MyRand.Next(1,3)))
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

        if(GetListOfCrimes().Any(x => x.HasBeenWitnessedByPolice && x.Severity == CrimeLevel.Felony))
        {
            if (LosSantosRED.MyRand.Next(1, 11) <= 2)
            {
                LosSantosRED.GivePlayerRandomWeapon(new List<GTAWeapon.WeaponCategory>() { GTAWeapon.WeaponCategory.Pistol, GTAWeapon.WeaponCategory.Melee }.PickRandom());
            }
            else
            {
                LosSantosRED.GivePlayerRandomWeapon(new List<GTAWeapon.WeaponCategory>() { GTAWeapon.WeaponCategory.Pistol, GTAWeapon.WeaponCategory.SMG, GTAWeapon.WeaponCategory.Shotgun, GTAWeapon.WeaponCategory.AR }.PickRandom());
            }
        }
        else if (GetListOfCrimes().Any(x => x.HasBeenWitnessedByPolice && x.Severity == CrimeLevel.Misdemeanor))
        {
            if (LosSantosRED.MyRand.Next(1, 11) <= 7)
            {
                LosSantosRED.GivePlayerRandomWeapon(new List<GTAWeapon.WeaponCategory>() { GTAWeapon.WeaponCategory.Pistol, GTAWeapon.WeaponCategory.Melee }.PickRandom());
            }
        }
        else
        {
            if(LosSantosRED.MyRand.Next(1,11)<= 2)
            {
                LosSantosRED.GivePlayerRandomWeapon(new List<GTAWeapon.WeaponCategory>() { GTAWeapon.WeaponCategory.Pistol, GTAWeapon.WeaponCategory.Melee }.PickRandom());
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
 
        if (Civilians.RecentlyKilledCivilian(5000) || Civilians.NearMurderVictim(15f))
        {
            KillingCivilians.IsCurrentlyViolating = true;
            if (KillingCivilians.CanObserveCrime && Police.AnyPoliceCanSeePlayer)
                KillingCivilians.CrimeObserved();
        }
        else
        {
            KillingCivilians.IsCurrentlyViolating = false;
        }

        if (Civilians.RecentlyHurtCivilian(5000))
        {
            HurtingCivilians.IsCurrentlyViolating = true;
            if (HurtingCivilians.CanObserveCrime && Police.AnyPoliceCanSeePlayer)
                HurtingCivilians.CrimeObserved();
        }
        else
        {
            HurtingCivilians.IsCurrentlyViolating = false;
        }

        if((LosSantosRED.PlayerRecentlyShot(3000) || Police.PlayerArtificiallyShooting) && !Game.LocalPlayer.Character.IsCurrentWeaponSilenced)
        {
            FiringWeapon.IsCurrentlyViolating = true;
            if (PoliceScanning.CopPeds.Any(x => x.RecentlySeenPlayer() || x.DistanceToPlayer <= 45f))
            {
                FiringWeaponNearPolice.IsCurrentlyViolating = true;
                if (FiringWeaponNearPolice.CanObserveCrime)
                    FiringWeaponNearPolice.CrimeObserved();
            }
        }
        else
        {
            FiringWeapon.IsCurrentlyViolating = false;
            FiringWeaponNearPolice.IsCurrentlyViolating = false;
        }


        if (Surrendering.IsCommitingSuicide)
        {
            AttemptingSuicide.IsCurrentlyViolating = true;
            if (AttemptingSuicide.CanObserveCrime && Police.AnyPoliceCanSeePlayer)
                AttemptingSuicide.CrimeObserved();
        }
        else
        {
            AttemptingSuicide.IsCurrentlyViolating = false;
        }

        bool IsBrandishing = false;
        if (LosSantosRED.PlayerIsConsideredArmed)
        {
            if (GameTimeStartedBrandishing == 0)
                GameTimeStartedBrandishing = Game.GameTime;
        }
        else
        {
            GameTimeStartedBrandishing = 0;
        }

        if(GameTimeStartedBrandishing > 0 && Game.GameTime - GameTimeStartedBrandishing >= 1500)
            IsBrandishing = true;

        if (IsBrandishing && Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && !LosSantosRED.PlayerInVehicle)
        {
            BrandishingWeapon.IsCurrentlyViolating = true;
            GTAWeapon MatchedWeapon = GTAWeapons.GetWeaponFromHash((ulong)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash);
            if (MatchedWeapon != null && MatchedWeapon.WeaponLevel >= 2)
                BrandishingWeapon.ResultingWantedLevel = MatchedWeapon.WeaponLevel;

            BrandishingWeapon.DispatchToPlay.WeaponToReport = MatchedWeapon;

            if (BrandishingWeapon.CanObserveCrime && Police.AnyPoliceCanSeePlayer)
                BrandishingWeapon.CrimeObserved();
        }
        else
        {
            BrandishingWeapon.IsCurrentlyViolating = false;
        }

        if (CarStealing.PlayerBreakingIntoCar)
        {
            GrandTheftAuto.IsCurrentlyViolating = true;
            if (GrandTheftAuto.CanObserveCrime && Police.AnyPoliceCanSeePlayer)
                GrandTheftAuto.CrimeObserved();
        }
        else
        {
            GrandTheftAuto.IsCurrentlyViolating = false;
        }
        if (LicensePlateChanging.PlayerChangingPlate)
        {
            ChangingPlates.IsCurrentlyViolating = true;
            if (ChangingPlates.CanObserveCrime && Police.AnyPoliceCanSeePlayer)
                ChangingPlates.CrimeObserved();
        }
        else
        {
            ChangingPlates.IsCurrentlyViolating = false;
        }


        if (MuggingSystem.IsMugging)
        {
            Mugging.IsCurrentlyViolating = true;
            if (Mugging.CanObserveCrime && Police.AnyPoliceCanSeePlayer)
                Mugging.CrimeObserved();
        }
        else
        {
            Mugging.IsCurrentlyViolating = false;
        }

        //Police Only
        if (TrespessingOnGovtProperty.CanObserveCrime && LosSantosRED.PlayerIsWanted && (PlayerLocation.PlayerCurrentZone == Zones.JAIL || PlayerLocation.PlayerCurrentZone == Zones.ARMYB || PlayerLocation.PlayerCurrentZone == Zones.NOOSE || PlayerLocation.PlayerCurrentZone == Zones.AIRP) && Police.AnyPoliceCanSeePlayer)
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
            bool InVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            ResistingArrest.DispatchToPlay.SuspectStatusOnFoot = true;
            if (InVehicle)
            {
                ResistingArrest.DispatchToPlay.SuspectStatusOnFoot = false;
                ResistingArrest.DispatchToPlay.VehicleToReport = LosSantosRED.GetPlayersCurrentTrackedVehicle();
            }
            ResistingArrest.CrimeObserved();
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
    public int TotalInstances = 0;
    public DispatchAudio.DispatchQueueItem DispatchToPlay;
    private uint GameTimeLastReported;
    private uint InstanceDuration = 20000;
    public bool IsMurder = false;
    public uint GameTimeLastCalledInByCivilians;
    public bool IsCurrentlyViolating = false;
    public bool CanBeReportedByCivilians = true;
    public bool CanBeCalledInBySound = false;
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
            if (LosSantosRED.PlayerIsWanted)
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
    public void CrimeObserved()
    {
        if ((!HasBeenReportedByDispatch || Severity == CrimeLevel.Felony) && !RecentlyReportedCrime(25000) && LosSantosRED.PlayerWantedLevel <= ResultingWantedLevel)//if (!HasBeenWitnessedByPolice && !HasBeenReportedByDispatch && InstantAction.PlayerWantedLevel <= ResultingWantedLevel)
        {
            GameTimeLastReported = Game.GameTime;
            DispatchToPlay.ResultsInLethalForce = ResultsInLethalForce;
            DispatchToPlay.ReportedBy = DispatchAudio.ReportType.Officers;
            DispatchAudio.AddDispatchToQueue(DispatchToPlay);
        }
        HasBeenWitnessedByPolice = true;
        InstancesObserved++;
        GameTimeLastWitnessed = Game.GameTime;
        Police.SetWantedLevel(ResultingWantedLevel, DebugName,true);
        HasBeenReportedByDispatch = true;
        Debugging.WriteToLog("Crime Logged", DebugName);        
    }
    //public void CrimeCalledInByCivilians(bool HaveDescription,bool ResultsInWanted)
    //{
    //    if (LosSantosRED.PlayerIsWanted || Police.PoliceInInvestigationMode)
    //        return;

    //    if (!RecentlyReportedCrime(25000))
    //    {
    //        Police.InvestigationPosition = Game.LocalPlayer.Character.Position;
    //        GameTimeLastReported = Game.GameTime;
    //        DispatchToPlay.ResultsInLethalForce = ResultsInLethalForce;
    //        DispatchToPlay.ReportedBy = DispatchAudio.ReportType.Civilians;

    //        if (ResultsInWanted)
    //            DispatchToPlay.ResultingWantedLevel = ResultingWantedLevel;

    //        GameFiber CrimeReportedFiber = GameFiber.StartNew(delegate
    //        {
    //            GTAPed ClosestPed = PoliceScanning.Civilians.Where(x => x.canSeePlayer || x.DistanceToPlayer <= 25f).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
    //            if (ClosestPed != null && ClosestPed.Pedestrian.Exists())
    //            {
    //                NativeFunction.CallByName<bool>("TASK_USE_MOBILE_PHONE_TIMED", ClosestPed.Pedestrian, 10000);
    //                ClosestPed.Pedestrian.PlayAmbientSpeech("JACKED_GENERIC");
    //            }

    //            Debugging.WriteToLog("Crime Pre Reported", DebugName);
    //            uint GameTimeStarted = Game.GameTime;
    //            int TimeToWait = LosSantosRED.MyRand.Next(3000, 7000);
    //            while(Game.GameTime - GameTimeStarted <= TimeToWait)
    //            {
    //                if (PedSwapping.JustTakenOver(2000))
    //                    return;
    //                GameFiber.Sleep(200);
    //            }

    //            if (LosSantosRED.PlayerIsWanted)
    //                return;

    //            DispatchAudio.AddDispatchToQueue(DispatchToPlay);
    //            if (HaveDescription)
    //                PersonOfInterest.PlayerBecamePersonOfInterest();

    //            if (!ResultsInWanted)
    //                Police.PoliceInInvestigationMode = true;

    //        }, "CrimeCalledInByCivilians");
    //        Debugging.GameFibers.Add(CrimeReportedFiber);
            
    //    }
    //    else
    //    {
    //        if (HaveDescription)
    //            PersonOfInterest.PlayerBecamePersonOfInterest();

    //        if (!ResultsInWanted)
    //            Police.PoliceInInvestigationMode = true;
    //    }


    //    GameTimeLastCalledInByCivilians = Game.GameTime;
    //    HasBeenReportedByDispatch = true;
    //    Debugging.WriteToLog("CrimeCalledInByCivilians", DebugName);
    //}
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
