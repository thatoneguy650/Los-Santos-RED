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
    private uint GameTimeStartedBrandishing;
    public int TimeAimedAtPolice;
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
            return Police.CurrentCrimes.CrimeList.Any(x => x.IsCurrentlyViolating && x.CanBeReportedByCivilians);// && x.Severity != CrimeLevel.Traffic);
        }
    }
    public List<Crime> CurrentlyViolatingCanBeReportedByCivilians
    {
        get
        {
            return Police.CurrentCrimes.CrimeList.Where(x => x.IsCurrentlyViolating && x.CanBeReportedByCivilians).ToList();// && x.Severity != CrimeLevel.Traffic);
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
                General.GivePlayerRandomWeapon(new List<GTAWeapon.WeaponCategory>() { GTAWeapon.WeaponCategory.Pistol, GTAWeapon.WeaponCategory.Melee }.PickRandom());
            }
            else
            {
                General.GivePlayerRandomWeapon(new List<GTAWeapon.WeaponCategory>() { GTAWeapon.WeaponCategory.Pistol, GTAWeapon.WeaponCategory.SMG, GTAWeapon.WeaponCategory.Shotgun, GTAWeapon.WeaponCategory.AR }.PickRandom());
            }
        }
        else if (CrimeList.Any(x => x.HasBeenWitnessedByPolice && x.Severity == CrimeLevel.Misdemeanor))
        {
            if (General.MyRand.Next(1, 11) <= 7)
            {
                General.GivePlayerRandomWeapon(new List<GTAWeapon.WeaponCategory>() { GTAWeapon.WeaponCategory.Pistol, GTAWeapon.WeaponCategory.Melee }.PickRandom());
            }
        }
        else
        {
            if(General.MyRand.Next(1,11)<= 2)
            {
                General.GivePlayerRandomWeapon(new List<GTAWeapon.WeaponCategory>() { GTAWeapon.WeaponCategory.Pistol, GTAWeapon.WeaponCategory.Melee }.PickRandom());
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
    public void CheckCrimes()
    {
        if (General.IsBusted || General.IsDead)
            return;


        if (PedWounds.RecentlyKilledCop(5000))
        {
            KillingPolice.IsCurrentlyViolating = true;
            if (KillingPolice.CanObserveCrime)
                KillingPolice.CrimeObserved();
        }
        else
        {
            KillingPolice.IsCurrentlyViolating = false;
        }

        if (PedWounds.RecentlyHurtCop(5000))
        {
            HurtingPolice.IsCurrentlyViolating = true;
            if (HurtingPolice.CanObserveCrime)
                HurtingPolice.CrimeObserved();
        }
        else
        {
            HurtingPolice.IsCurrentlyViolating = false;
        }


        if (PedWounds.RecentlyKilledCivilian(5000) || PedWounds.NearCivilianMurderVictim(9f))
        {
            KillingCivilians.IsCurrentlyViolating = true;
            if (KillingCivilians.CanObserveCrime && Police.AnyPoliceCanSeePlayer)
                KillingCivilians.CrimeObserved();
        }
        else
        {
            KillingCivilians.IsCurrentlyViolating = false;
        }

        if (PedWounds.RecentlyHurtCivilian(5000))
        {
            HurtingCivilians.IsCurrentlyViolating = true;
            if (HurtingCivilians.CanObserveCrime && Police.AnyPoliceCanSeePlayer)
                HurtingCivilians.CrimeObserved();
        }
        else
        {
            HurtingCivilians.IsCurrentlyViolating = false;
        }

        if(General.PlayerRecentlyShot(5000) || Police.PlayerArtificiallyShooting || Game.LocalPlayer.Character.IsShooting)
        {
            FiringWeapon.IsCurrentlyViolating = true;
            if (Game.LocalPlayer.Character.IsCurrentWeaponSilenced)
            {
                if (PedList.CopPeds.Any(x => x.RecentlySeenPlayer()))
                {
                    FiringWeaponNearPolice.IsCurrentlyViolating = true;
                    if (FiringWeaponNearPolice.CanObserveCrime)
                        FiringWeaponNearPolice.CrimeObserved();
                }
            }
            else
            {
                if (PedList.CopPeds.Any(x => x.RecentlySeenPlayer() || x.DistanceToPlayer <= 45f))
                {
                    FiringWeaponNearPolice.IsCurrentlyViolating = true;
                    if (FiringWeaponNearPolice.CanObserveCrime)
                        FiringWeaponNearPolice.CrimeObserved();
                }
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
        if (General.PlayerIsConsideredArmed)
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

        if (IsBrandishing && Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && !General.PlayerInVehicle)
        {
            BrandishingWeapon.IsCurrentlyViolating = true;
            GTAWeapon MatchedWeapon = GTAWeapons.GetWeaponFromHash((ulong)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash);
            if (MatchedWeapon != null && MatchedWeapon.WeaponLevel >= 2)
                BrandishingWeapon.ResultingWantedLevel = MatchedWeapon.WeaponLevel;
            else
                BrandishingWeapon.ResultingWantedLevel = 1;

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


        if (global::Mugging.IsMugging)
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
        if (TrespessingOnGovtProperty.CanObserveCrime && General.PlayerIsWanted && PlayerLocation.PlayerCurrentZone.IsRestrictedDuringWanted && Police.AnyPoliceCanSeePlayer)
        {
            TrespessingOnGovtProperty.CrimeObserved();
        }
        CheckAimingAtPolice();
        if (!AimingWeaponAtPolice.CanObserveCrime && TimeAimedAtPolice >= 10)
        {
            AimingWeaponAtPolice.CrimeObserved();
        }
        if(ResistingArrest.CanObserveCrime && !ResistingArrest.HasBeenWitnessedByPolice && General.PlayerIsWanted && Police.AnyPoliceCanSeePlayer && Game.LocalPlayer.Character.Speed >= 2.0f && !General.HandsAreUp && Police.PlayerHasBeenWantedFor >= 15000)
        {
            bool InVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            ResistingArrest.DispatchToPlay.SuspectStatusOnFoot = true;
            if (InVehicle)
            {
                ResistingArrest.DispatchToPlay.SuspectStatusOnFoot = false;
                ResistingArrest.DispatchToPlay.VehicleToReport = General.GetPlayersCurrentTrackedVehicle();
            }
            ResistingArrest.CrimeObserved();
        }
        if (GrandTheftAuto.CanObserveCrime && Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.IsInAnyPoliceVehicle && Police.AnyPoliceCanSeePlayer && General.GetPlayersCurrentTrackedVehicle() != null && !General.GetPlayersCurrentTrackedVehicle().WasReportedStolen && PedList.CopPeds.Any(x => x.DistanceToPlayer <= 17f))
        {
            GrandTheftAuto.CrimeObserved();
        }
        if (GotInAirVehicleDuringChase.CanObserveCrime && General.PlayerIsWanted && General.PlayerInVehicle && Game.LocalPlayer.Character.IsInAirVehicle)
        {
            GotInAirVehicleDuringChase.CrimeObserved();
        }
    }
    private void CheckAimingAtPolice()
    {
        if (!AimingWeaponAtPolice.HasBeenWitnessedByPolice && General.PlayerIsConsideredArmed && Game.LocalPlayer.IsFreeAiming && Police.AnyPoliceCanSeePlayer && PedList.CopPeds.Any(x => Game.LocalPlayer.IsFreeAimingAtEntity(x.Pedestrian)))
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
            if (General.PlayerIsWanted)
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
        if ((!HasBeenReportedByDispatch || Severity == CrimeLevel.Felony) && !RecentlyReportedCrime(25000) && General.PlayerWantedLevel <= ResultingWantedLevel)//if (!HasBeenWitnessedByPolice && !HasBeenReportedByDispatch && InstantAction.PlayerWantedLevel <= ResultingWantedLevel)
        {
            GameTimeLastReported = Game.GameTime;
            DispatchToPlay.ResultsInLethalForce = ResultsInLethalForce;
            DispatchToPlay.ReportedBy = ReportType.Officers;
            AddDispatchToQueue(DispatchToPlay);
        }
        else if (!Police.CurrentCrimes.RecentlyReportedAnyCrime && CanBeReportedMultipleTimes && !RecentlyReportedCrime(25000))//if (!HasBeenWitnessedByPolice && !HasBeenReportedByDispatch && InstantAction.PlayerWantedLevel <= ResultingWantedLevel)
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
        Police.SetWantedLevel(ResultingWantedLevel, Name,true);
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
