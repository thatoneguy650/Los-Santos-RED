using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DispatchAudio;

public class RapSheet
{
    public int MaxWantedLevel = 0;

    public Crime KillingPolice = new Crime("Police Fatality", CrimeLevel.Felony, 3, true, new DispatchQueueItem(AvailableDispatch.OfficerDown, 1, true)) { CanBeReportedMultipleTimes = true };
    public Crime FiringWeaponNearPolice = new Crime("Shots Fired at Police", CrimeLevel.Felony, 3, true, new DispatchQueueItem(AvailableDispatch.ShootingAtPolice, 2, true)) { CanBeReportedMultipleTimes = true };
    public Crime AimingWeaponAtPolice = new Crime("Aiming Weapons At Police", CrimeLevel.Felony, 3, true, new DispatchQueueItem(AvailableDispatch.ThreateningOfficerWithFirearm, 3, true)) { CanBeReportedMultipleTimes = true };
    public Crime HurtingPolice = new Crime("Assaulting Police", CrimeLevel.Felony, 3, true, new DispatchQueueItem(AvailableDispatch.AssaultingOfficer, 4, true)) { CanBeReportedMultipleTimes = true };
    public Crime TrespessingOnGovtProperty = new Crime("Trespassing on Government Property", CrimeLevel.Felony, 3, true, new DispatchQueueItem(AvailableDispatch.TrespassingOnGovernmentProperty, 5, true)) { CanBeReportedMultipleTimes = true };
    public Crime GotInAirVehicleDuringChase = new Crime("Stealing an Air Vehicle", CrimeLevel.Felony, 4, true, new DispatchQueueItem(AvailableDispatch.StealingAirVehicle, 6, true)) { CanBeReportedMultipleTimes = true };
    public Crime FiringWeapon = new Crime("Firing Weapon", CrimeLevel.Misdemeanor, 2, false, new DispatchQueueItem(AvailableDispatch.ShotsFired, 6)) { CanBeCalledInBySound = true,CanBeReportedMultipleTimes = true };
    public Crime KillingCivilians = new Crime("Civilian Fatality", CrimeLevel.Misdemeanor, 2, false, new DispatchQueueItem(AvailableDispatch.CivlianFatality, 7)) { CiviliansCanFightIfObserved = true,CanBeReportedMultipleTimes = true };
    public Crime GrandTheftAuto = new Crime("Grand Theft Auto", CrimeLevel.Misdemeanor, 2, false, new DispatchQueueItem(AvailableDispatch.GrandTheftAuto, 8)) { CiviliansCanFightIfObserved = true,CanBeReportedMultipleTimes = true };
    public Crime Mugging = new Crime("Mugging", CrimeLevel.Misdemeanor, 2, false, new DispatchQueueItem(AvailableDispatch.CivilianMugged, 9)) { CiviliansCanFightIfObserved = true };
    public Crime AttemptingSuicide = new Crime("Attempting Suicide", CrimeLevel.Misdemeanor, 2, false, new DispatchQueueItem(AvailableDispatch.AttemptingSuicide, 10));
    public Crime BrandishingWeapon = new Crime("Brandishing Weapon", CrimeLevel.Misdemeanor, 2, false, new DispatchQueueItem(AvailableDispatch.CarryingWeapon, 11)) { CiviliansCanFightIfObserved = true };
    public Crime HitPedWithCar = new Crime("Pedestrian Hit and Run", CrimeLevel.Misdemeanor, 2, false, new DispatchQueueItem(AvailableDispatch.PedestrianHitAndRun, 12)) { CiviliansCanFightIfObserved = true };
    public Crime HurtingCivilians = new Crime("Assaulting Civilians", CrimeLevel.Misdemeanor, 2, false, new DispatchQueueItem(AvailableDispatch.CivilianInjury, 13)) { CiviliansCanFightIfObserved = true };
    public Crime HitCarWithCar = new Crime("Hit and Run", CrimeLevel.Misdemeanor, 1, false, new DispatchQueueItem(AvailableDispatch.VehicleHitAndRun, 14) { ResultsInStolenCarSpotted = true, IsTrafficViolation = true }) { CiviliansCanFightIfObserved = true, CanBeReportedMultipleTimes = false, WillScareCivilians = false };
    public Crime ChangingPlates = new Crime("Stealing License Plates", CrimeLevel.Misdemeanor, 1, false, new DispatchQueueItem(AvailableDispatch.SuspiciousActivity, 15)) { CiviliansCanFightIfObserved = true, WillScareCivilians = false };


    public Crime ResistingArrest = new Crime("Resisting Arrest", CrimeLevel.Misdemeanor, 2, false, new DispatchQueueItem(AvailableDispatch.SuspectResisitingArrest, 22));   

    public Crime DrivingAgainstTraffic = new Crime("Driving Against Traffic", CrimeLevel.Traffic,1,false, new DispatchQueueItem(AvailableDispatch.RecklessDriving, 17) { ResultsInStolenCarSpotted = true, IsTrafficViolation = true }) { CanBeReportedByCivilians = false, CiviliansCanFightIfObserved = false, CanBeReportedMultipleTimes = false, WillScareCivilians = false };
    public Crime DrivingOnPavement = new Crime("Driving On Pavement", CrimeLevel.Traffic,1,false, new DispatchQueueItem(AvailableDispatch.RecklessDriving, 18) { ResultsInStolenCarSpotted = true, IsTrafficViolation = true }) { CanBeReportedByCivilians = false, CiviliansCanFightIfObserved = false, CanBeReportedMultipleTimes = false, WillScareCivilians = false };
    public Crime NonRoadworthyVehicle = new Crime("Non-Roadworthy Vehicle", CrimeLevel.Traffic,1,false, new DispatchQueueItem(AvailableDispatch.SuspiciousVehicle, 19) { ResultsInStolenCarSpotted = true, IsTrafficViolation = true }) { CanBeReportedByCivilians = false, CiviliansCanFightIfObserved = false, CanBeReportedMultipleTimes = false, WillScareCivilians = false };
    public Crime FelonySpeeding = new Crime("Speeding", CrimeLevel.Traffic,1,false, new DispatchQueueItem(AvailableDispatch.FelonySpeeding, 20) { ResultsInStolenCarSpotted = true, IsTrafficViolation = true }) { CanBeReportedByCivilians = false, CiviliansCanFightIfObserved = false, CanBeReportedMultipleTimes = false, WillScareCivilians = false };
    public Crime RunningARedLight = new Crime("Running a Red Light", CrimeLevel.Traffic, 1, false, new DispatchQueueItem(AvailableDispatch.RunningARedLight, 21) { ResultsInStolenCarSpotted = true, IsTrafficViolation = true }) { CanBeReportedByCivilians = false, CiviliansCanFightIfObserved = false, CanBeReportedMultipleTimes = false, WillScareCivilians = false };

    public List<GTALicensePlate> WantedPlates = new List<GTALicensePlate>();
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
            foreach (Crime myCrime in CrimeList.Where(x => !x.HasBeenWitnessedByPolice).PickRandom(General.MyRand.Next(1,3)))
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

        if(CrimeList.Any(x => x.HasBeenWitnessedByPolice && x.Severity == CrimeLevel.Felony))
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
            if(General.MyRand.Next(1,11)<= 2)
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
            CrimeString += string.Format("~n~{0}{1} ({2})~s~", GetColorSeverity(MyCrime.Severity),MyCrime.Name,MyCrime.InstancesObserved);
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
        foreach(Crime MyCrime in CrimeList.Where(x => x.HasBeenWitnessedByPolice))
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
    public DispatchQueueItem DispatchToPlay;
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
    public Crime(string _Name, CrimeLevel _Severity, int _ResultingWantedLevel, bool _ResultsInLethalForce, DispatchQueueItem _DispatchToPlay)
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
            DispatchToPlay.ReportedBy = ReportType.Officers;
            AddDispatchToQueue(DispatchToPlay);
        }
        else if (!WantedLevelScript.CurrentCrimes.RecentlyReportedAnyCrime && CanBeReportedMultipleTimes && !RecentlyReportedCrime(25000))//if (!HasBeenWitnessedByPolice && !HasBeenReportedByDispatch && InstantAction.PlayerWantedLevel <= ResultingWantedLevel)
        {
            Debugging.WriteToLog("Crime Logged No Dispatch Activity", Name);
            GameTimeLastReported = Game.GameTime;
            DispatchToPlay.ResultsInLethalForce = ResultsInLethalForce;
            DispatchToPlay.ReportedBy = ReportType.Officers;
            AddDispatchToQueue(DispatchToPlay);
        }
        int CopsToRadio = 0;
        foreach(GTACop Cop in PedList.CopPeds.Where(x => x.Pedestrian.Exists() && x.RecentlySeenPlayer()).OrderBy(x=> x.DistanceToPlayer).Take(2))
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
        WantedLevelScript.SetWantedLevel(ResultingWantedLevel, Name,true);
        HasBeenReportedByDispatch = true;
        Debugging.WriteToLog("Crime Logged", Name);        
    }
    private void PickCopToRadioIn()
    {
        General.RequestAnimationDictionay("random@arrests");

        NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "random@arrests", "generic_radio_enter", 2.0f, -2.0f, -1, 48, 0, false, false, false);
    }
    //public void CrimeCalledInByCivilians(bool HaveDescription,bool ResultsInWanted)
    //{
    //    if (LosSantosRED.PlayerIsWanted || InvestigationScript.InInvestigationMode)
    //        return;

    //    if (!RecentlyReportedCrime(25000))
    //    {
    //        InvestigationScript.InvestigationPosition = Game.LocalPlayer.Character.Position;
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
    //                InvestigationScript.InInvestigationMode = true;

    //        }, "CrimeCalledInByCivilians");
    //        Debugging.GameFibers.Add(CrimeReportedFiber);
            
    //    }
    //    else
    //    {
    //        if (HaveDescription)
    //            PersonOfInterest.PlayerBecamePersonOfInterest();

    //        if (!ResultsInWanted)
    //            InvestigationScript.InInvestigationMode = true;
    //    }


    //    GameTimeLastCalledInByCivilians = Game.GameTime;
    //    HasBeenReportedByDispatch = true;
    //    Debugging.WriteToLog("CrimeCalledInByCivilians", DebugName);
    //}
    public string CrimeStats()
    {
        return string.Format("CrimeName {0}, HasBeenWitnessedByPolice: {1},GameTimeLastWitnessed {2},InstancesObserved {3},HasBeenReportedByDispatch {4}", Name, HasBeenWitnessedByPolice, GameTimeLastWitnessed, InstancesObserved, HasBeenReportedByDispatch);
    }
}
public enum CrimeLevel
{
    Unknown,
    Traffic,
    Misdemeanor,
    Felony
    
}
