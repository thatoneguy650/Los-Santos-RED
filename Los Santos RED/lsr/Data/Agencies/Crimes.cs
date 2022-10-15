using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Crimes : ICrimes
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Crimes.xml";
    private List<Crime> DefaultCrimeList;
    private Crime KillingPolice;
    private Crime TerroristActivity;
    private Crime FiringWeaponNearPolice;
    private Crime AimingWeaponAtPolice;
    private Crime HurtingPolice;
    private Crime BrandishingHeavyWeapon;
    private Crime TrespessingOnGovtProperty;
    private Crime GotInAirVehicleDuringChase;
    private Crime FiringWeapon;
    private Crime Kidnapping;
    private Crime KillingCivilians;
    private Crime ArmedRobbery;
    private Crime Mugging;
    private Crime AttemptingSuicide;
    private Crime HitPedWithCar;
    private Crime HurtingCivilians;
    private Crime GrandTheftAuto;
    private Crime BrandishingWeapon;
    private Crime ResistingArrest;
    private Crime BrandishingCloseCombatWeapon;
    private Crime DrunkDriving;
    private Crime AssaultingWithDeadlyWeapon;
    private Crime AssaultingCivilians;
    private Crime DealingDrugs;
    private Crime DealingGuns;
    private Crime HitCarWithCar;
    private Crime PublicIntoxication;
    private Crime ChangingPlates;
    private Crime DrivingAgainstTraffic;
    private Crime DrivingOnPavement;
    private Crime NonRoadworthyVehicle;
    private Crime RunningARedLight;
    private Crime FelonySpeeding;
    private Crime DrivingStolenVehicle;
    private Crime SuspiciousActivity;
    private Crime InsultingOfficer;
    private Crime Harassment;
    private Crime Speeding;
    private Crime PublicNuisance;
    private Crime PublicVagrancy;
    private Crime OfficersNeeded;

    public Crimes()
    {
    }
    public List<Crime> CrimeList { get; private set; } = new List<Crime>();


    public void ReadConfig()
    {
        SetupCrimes();
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("Crimes*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Crimes config: {ConfigFile.FullName}",0);
            CrimeList = Serialization.DeserializeParams<Crime>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Crimes config  {ConfigFileName}",0);
            CrimeList = Serialization.DeserializeParams<Crime>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Crimes config found, creating default", 0);       
            DefaultConfig();
        }
    }
    public Crime GetCrime(string crimeID)
    {
        return CrimeList.FirstOrDefault(x => x.ID == crimeID);
    }
    public void SerializeAllSettings()
    {
        Serialization.SerializeParams(CrimeList == null ? new List<Crime>() : CrimeList, ConfigFileName);
    }

    private void SetupCrimes()
    {
        KillingPolice = new Crime("KillingPolice", "Police Fatality", 3, true, 1, false, true, true) { CanViolateWithoutPerception = true };
        TerroristActivity = new Crime("TerroristActivity", "Terrorist Activity", 4, true, 2, false, false, true) { CanReportBySound = true };
        FiringWeaponNearPolice = new Crime("FiringWeaponNearPolice", "Shots Fired at Police", 3, true, 3, false, false, true) { CanReportBySound = true };
        AimingWeaponAtPolice = new Crime("AimingWeaponAtPolice", "Aiming Weapons At Police", 3, true, 4, false, false, true);
        HurtingPolice = new Crime("HurtingPolice", "Assaulting Police", 3, false, 5, false, false, true) { CanViolateWithoutPerception = true };
        BrandishingHeavyWeapon = new Crime("BrandishingHeavyWeapon", "Brandishing Heavy Weapon", 3, false, 6, false, true, true);
        TrespessingOnGovtProperty = new Crime("TrespessingOnGovtProperty", "Trespassing on Government Property", 3, false, 7, false, false, true);
        GotInAirVehicleDuringChase = new Crime("GotInAirVehicleDuringChase", "Stealing an Air Vehicle", 3, false, 8, false, false, true);
        FiringWeapon = new Crime("FiringWeapon", "Firing Weapon", 2, false, 9, true, true, true) { CanReportBySound = true };
        Kidnapping = new Crime("Kidnapping", "Kidnapping", 2, false, 10, true, false, true);
        KillingCivilians = new Crime("KillingCivilians", "Civilian Fatality", 2, false, 11, true, true, true);
        ArmedRobbery = new Crime("ArmedRobbery", "Armed Robbery", 2, false, 12, true, true, true);
        Mugging = new Crime("Mugging", "Mugging", 2, false, 12, true, true, true);
        AttemptingSuicide = new Crime("AttemptingSuicide", "Attempting Suicide", 2, false, 13, false, false, true);
        HitPedWithCar = new Crime("HitPedWithCar", "Pedestrian Hit and Run", 2, false, 14, true, true, true);
        HurtingCivilians = new Crime("HurtingCivilians", "Assaulting Civilians", 2, false, 15, true, true, true);     
        GrandTheftAuto = new Crime("GrandTheftAuto", "Grand Theft Auto", 2, false, 16, true, true, true);
        BrandishingWeapon = new Crime("BrandishingWeapon", "Brandishing Weapon", 2, false, 17, true, true, true);
        ResistingArrest = new Crime("ResistingArrest", "Resisting Arrest", 2, false, 18, false, false, true);
        BrandishingCloseCombatWeapon = new Crime("BrandishingCloseCombatWeapon", "Brandishing Close Combat Weapon", 1, false, 19, true, true, true);
        DrunkDriving = new Crime("DrunkDriving", "Drunk Driving", 2, false, 20, true, false, false);     
        AssaultingWithDeadlyWeapon = new Crime("AssaultingWithDeadlyWeapon", "Assaulting With A Deadly Weapon", 2, false, 21, true, true, true);
        AssaultingCivilians = new Crime("AssaultingCivilians", "Assaulting", 2, false, 22, true, true, true);

        DealingDrugs = new Crime("DealingDrugs", "Dealing Drugs", 2, false, 23, true, false, false) { MaxReportingDistance = 20f };
        DealingGuns = new Crime("DealingGuns", "Illegal Weapons Dealing", 2, false, 24, true, false, false) { MaxReportingDistance = 20f };

        HitCarWithCar = new Crime("HitCarWithCar", "Hit and Run", 1, false, 30, false, false, false) { IsTrafficViolation = true };

        PublicIntoxication = new Crime("PublicIntoxication", "Public Intoxication", 1, false, 31, true, false, false);
        ChangingPlates = new Crime("ChangingPlates", "Stealing License Plates", 1, false, 32, true, false, false) { MaxReportingDistance = 20f };

        DrivingAgainstTraffic = new Crime("DrivingAgainstTraffic", "Driving Against Traffic", 1, false, 33, false, false, false) { IsTrafficViolation = true, RequiresCitation = true };
        DrivingOnPavement = new Crime("DrivingOnPavement", "Driving On Pavement", 1, false, 34, false, false, false) { IsTrafficViolation = true, RequiresCitation = true };
        NonRoadworthyVehicle = new Crime("NonRoadworthyVehicle", "NonRoadworthy Vehicle", 1, false, 35, false, false, false) { IsTrafficViolation = true, RequiresCitation = true };
        RunningARedLight = new Crime("RunningARedLight", "Running a Red Light", 1, false, 36, false, false, false) { IsTrafficViolation = true, RequiresCitation = true };
        FelonySpeeding = new Crime("FelonySpeeding", "Felony Speeding", 1, false, 37, false, false, false) { IsTrafficViolation = true, RequiresCitation = true };
 
        DrivingStolenVehicle = new Crime("DrivingStolenVehicle", "Driving a Stolen Vehicle", 2, false, 38, false, false, false);
        SuspiciousActivity = new Crime("SuspiciousActivity", "Suspicious Activity", 1, false, 39, false, false, false) { RequiresSearch = true };
        InsultingOfficer = new Crime("InsultingOfficer", "Insulting a Police Officer", 2, false, 40, false, false, true);
        Harassment = new Crime("Harassment", "Harassment", 1, false, 41, true, false, false) { MaxReportingDistance = 15f, RequiresCitation = true };
        Speeding = new Crime("Speeding", "Speeding", 1, false, 44, false, false, false) { IsTrafficViolation = true, RequiresCitation = true };
        PublicNuisance = new Crime("PublicNuisance", "Public Nuisance", 1, false, 50, true, false, false) { MaxReportingDistance = 15f, RequiresCitation = true };
        PublicVagrancy = new Crime("PublicVagrancy", "Public Vagrancy", 1, false, 51, true, false, false) { MaxReportingDistance = 15f, RequiresCitation = true };
        OfficersNeeded = new Crime("OfficersNeeded", "Officers Needed", 1, false, 60, false, false, false);
    }

    private void DefaultConfig()
    {
        CrimeList = new List<Crime>()
        {
             KillingPolice,
             TerroristActivity,
             FiringWeaponNearPolice,
             AimingWeaponAtPolice,
             HurtingPolice,
             BrandishingHeavyWeapon,
             TrespessingOnGovtProperty,
             GotInAirVehicleDuringChase,
             FiringWeapon,
             Kidnapping,
             KillingCivilians,
             ArmedRobbery,
             Mugging,
             AttemptingSuicide,
             HitPedWithCar,
             HurtingCivilians,
             GrandTheftAuto,
             BrandishingWeapon,
             ResistingArrest,
             BrandishingCloseCombatWeapon,
             DrunkDriving,
             AssaultingWithDeadlyWeapon,
             AssaultingCivilians,
             DealingDrugs,
             DealingGuns,
             HitCarWithCar,
             PublicIntoxication,
             ChangingPlates,
             DrivingAgainstTraffic,
             DrivingOnPavement,
             NonRoadworthyVehicle,
             RunningARedLight,
             FelonySpeeding,
             DrivingStolenVehicle,
             SuspiciousActivity,
             InsultingOfficer,
             Harassment,
             Speeding,
             PublicNuisance,
             PublicVagrancy,
             OfficersNeeded,
        };
        Serialization.SerializeParams(CrimeList, ConfigFileName);
    }

    public void SetEasy()
    {
        SetDefault();
    }

    public void SetDefault()
    {
        CrimeList.Clear();
        CrimeList.AddRange(
            new List<Crime> {
             KillingPolice,
             TerroristActivity,
             FiringWeaponNearPolice,
             AimingWeaponAtPolice,
             HurtingPolice,
             BrandishingHeavyWeapon,
             TrespessingOnGovtProperty,
             GotInAirVehicleDuringChase,
             FiringWeapon,
             Kidnapping,
             KillingCivilians,
             ArmedRobbery,
             Mugging,
             AttemptingSuicide,
             HitPedWithCar,
             HurtingCivilians,
             GrandTheftAuto,
             BrandishingWeapon,
             ResistingArrest,
             BrandishingCloseCombatWeapon,
             DrunkDriving,
             AssaultingWithDeadlyWeapon,
             AssaultingCivilians,
             DealingDrugs,
             DealingGuns,
             HitCarWithCar,
             PublicIntoxication,
             ChangingPlates,
             DrivingAgainstTraffic,
             DrivingOnPavement,
             NonRoadworthyVehicle,
             RunningARedLight,
             FelonySpeeding,
             DrivingStolenVehicle,
             SuspiciousActivity,
             InsultingOfficer,
             Harassment,
             Speeding,
             PublicNuisance,
             PublicVagrancy,
             OfficersNeeded,
            });

        HitCarWithCar.CanBeReportedByCivilians = false;
        DrivingAgainstTraffic.CanBeReportedByCivilians = false;
        DrivingOnPavement.CanBeReportedByCivilians = false;
        NonRoadworthyVehicle.CanBeReportedByCivilians = false;
        RunningARedLight.CanBeReportedByCivilians = false;
        FelonySpeeding.CanBeReportedByCivilians = false;
    }

    public void SetHard()
    {
        SetDefault();
        HitCarWithCar.CanBeReportedByCivilians = true;
        DrivingAgainstTraffic.CanBeReportedByCivilians = true;
        DrivingOnPavement.CanBeReportedByCivilians = true;
        NonRoadworthyVehicle.CanBeReportedByCivilians = true;
        RunningARedLight.CanBeReportedByCivilians = true;
        FelonySpeeding.CanBeReportedByCivilians = true;
    }

    public void SetPreferred()
    {
        SetHard();
    }
}