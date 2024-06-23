using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

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
    private Crime TrespassingOnMilitaryBase;
    private Crime GotInAirVehicleDuringChase;
    private Crime FiringWeapon;
    private Crime FiringSilencedWeapon;
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
    private Crime SuspiciousVehicle;
    private Crime SevereTrespessing;
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
    private Crime IndecentExposure;
    private Crime PublicNuisance;
    private Crime PublicVagrancy;
    private Crime OfficersNeeded;
    private Crime Trespassing;
    private Crime VehicleInvasion;
    private Crime MaliciousVehicleDamage;
    private Crime DrugPossession;
    private Crime StandingOnVehicle;
    private Crime BuringABody;
    private Crime Theft;
    private Crime Shoplifting;
    private Crime BankRobbery;
    public Crimes()
    {
    }
    public List<Crime> CrimeList { get; private set; } = new List<Crime>();

    [XmlIgnore]
    public bool IsBackendChanged { get; set; } = false;
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

        TrespassingOnMilitaryBase = new Crime(StaticStrings.TrespassingOnMilitaryBaseCrimeID, "Trespassing on Military Installation", 6, true, 0, false, false, false);

        KillingPolice = new Crime(StaticStrings.KillingPoliceCrimeID, "Police Fatality", 3, true, 1, true, true, true);// { CanViolateWithoutPerception = true };
        TerroristActivity = new Crime(StaticStrings.TerroristActivityCrimeID, "Terrorist Activity", 3, true, 2, false, false, true) { CanReportBySound = true };
        FiringWeaponNearPolice = new Crime(StaticStrings.FiringWeaponNearPoliceCrimeID, "Shots Fired Near Police", 3, true, 3, false, false, true) { CanReportBySound = true };
        AimingWeaponAtPolice = new Crime(StaticStrings.AimingWeaponAtPoliceCrimeID, "Aiming Weapons At Police", 3, true, 4, false, false, true);
        HurtingPolice = new Crime(StaticStrings.HurtingPoliceCrimeID, "Assaulting Police", 3, false, 5, true, false, true);// { CanViolateWithoutPerception = true };
        BrandishingHeavyWeapon = new Crime(StaticStrings.BrandishingHeavyWeaponCrimeID, "Brandishing Heavy Weapon", 3, false, 6, true, true, true);
        BankRobbery = new Crime(StaticStrings.BankRobberyCrimeID, "Bank Robbery", 3, false, 6, true, true, true);



        TrespessingOnGovtProperty = new Crime(StaticStrings.TrespessingOnGovtPropertyCrimeID, "Trespassing on Government Property", 3, false, 7, false, false, true);
        





        GotInAirVehicleDuringChase = new Crime(StaticStrings.GotInAirVehicleDuringChaseCrimeID, "Stealing an Air Vehicle", 3, false, 8, false, false, true);
        


        FiringWeapon = new Crime(StaticStrings.FiringWeaponCrimeID, "Firing Weapon", 2, false, 9, true, true, true) { CanReportBySound = true };
        FiringSilencedWeapon = new Crime(StaticStrings.FiringSilencedWeaponCrimeID, "Firing Weapon(!)", 2, false, 9, true, true, true) { MaxHearingDistance = 10f, CanReportBySound = true };
        Kidnapping = new Crime(StaticStrings.KidnappingCrimeID, "Kidnapping", 2, false, 10, true, false, true);
        KillingCivilians = new Crime(StaticStrings.KillingCiviliansCrimeID, "Civilian Fatality", 2, false, 11, true, true, true);
        ArmedRobbery = new Crime(StaticStrings.ArmedRobberyCrimeID, "Armed Robbery", 2, false, 12, true, true, true);
        Mugging = new Crime(StaticStrings.MuggingCrimeID, "Mugging", 2, false, 12, true, true, true);
        AttemptingSuicide = new Crime(StaticStrings.AttemptingSuicideCrimeID, "Attempting Suicide", 2, false, 13, true, false, true);
        HitPedWithCar = new Crime(StaticStrings.HitPedWithCarCrimeID, "Pedestrian Hit and Run", 2, false, 14, true, true, true);
        HurtingCivilians = new Crime(StaticStrings.HurtingCiviliansCrimeID, "Assaulting Civilians", 2, false, 15, true, true, true);     
        GrandTheftAuto = new Crime(StaticStrings.GrandTheftAutoCrimeID, "Grand Theft Auto", 2, false, 16, true, true, true);
        BrandishingWeapon = new Crime(StaticStrings.BrandishingWeaponCrimeID, "Brandishing Weapon", 2, false, 17, true, true, true) { ShowsWarning = true, WarningMessage = "You are ~r~brandishing a firearm~s~" };
        ResistingArrest = new Crime(StaticStrings.ResistingArrestCrimeID, "Resisting Arrest", 2, false, 18, false, false, false);
        BrandishingCloseCombatWeapon = new Crime(StaticStrings.BrandishingCloseCombatWeaponCrimeID, "Brandishing Close Combat Weapon", 1, false, 19, true, true, true) { ShowsWarning = true, WarningMessage = "You are ~r~brandishing a dangerous melee weapon~s~" };
        DrunkDriving = new Crime(StaticStrings.DrunkDrivingCrimeID, "Drunk Driving", 2, false, 20, true, false, false) { ShowsWarning = true, WarningMessage = "You are ~r~driving under the influence~s~~n~Sober up or stop driving" };     
        AssaultingWithDeadlyWeapon = new Crime(StaticStrings.AssaultingWithDeadlyWeaponCrimeID, "Assaulting With A Deadly Weapon", 2, false, 21, true, true, true);
        AssaultingCivilians = new Crime(StaticStrings.AssaultingCiviliansCrimeID, "Assaulting", 2, false, 22, true, true, true);

        Theft = new Crime(StaticStrings.TheftCrimeID, "Theft", 2, false, 22, true, true, true);
        Shoplifting = new Crime(StaticStrings.ShopliftingCrimeID, "Shoplifting", 2, false, 22, true, true, true);

        DealingDrugs = new Crime(StaticStrings.DealingDrugsCrimeID, "Dealing Drugs", 2, false, 23, true, false, false) { MaxReportingDistance = 20f, ShowsWarning = true, WarningMessage = "You are ~r~dealing illegal drugs~s~~n~Avoid dealing in public" };
        DealingGuns = new Crime(StaticStrings.DealingGunsCrimeID, "Illegal Weapons Dealing", 2, false, 24, true, false, false) { MaxReportingDistance = 20f, ShowsWarning = true, WarningMessage = "You are ~r~dealing illegal guns~s~~n~Avoid dealing in public" };
        DrugPossession = new Crime(StaticStrings.DrugPossessionCrimeID, "Drug Possession", 2, false, 25, true, false, false) { MaxReportingDistance = 20f, ShowsWarning = true, WarningMessage = "You are ~r~in possession of illegal drugs~s~~n~Avoid using in public" };
        SuspiciousVehicle = new Crime(StaticStrings.SuspiciousVehicleCrimeID, "Suspicious Vehicle", 2, false, 26, true, false, false) { MaxObservingDistance = 30f, MaxReportingDistance = 15f, ShowsWarning = true, WarningMessage = "This vehicle is ~r~suspicious~s~~n~Remove offending items" };

        SevereTrespessing = new Crime(StaticStrings.CivilianTrespessingCrimeID, "Trespassing", 2, false, 27, true, true, false) { ShowsWarning = true, WarningMessage = "You are ~r~trespassing~s~~n~Leave the area or avoid being spotted" };

        Trespassing = new Crime(StaticStrings.TrespessingCrimeID, "Trespassing", 2, false, 27, true, false, false) { ShowsWarning = true, WarningMessage = "You are ~r~trespassing~s~~n~Leave the area or avoid being spotted" };
        VehicleInvasion = new Crime(StaticStrings.VehicleInvasionCrimeID, "Vehicle Invasion", 2, false, 28, true, true, true) { MaxReportingDistance = 15f  };


        BuringABody = new Crime(StaticStrings.BuryingABody, "Burying Body", 2,false,29,true,true,true) { MaxReportingDistance = 45f };



        MaliciousVehicleDamage = new Crime(StaticStrings.MaliciousVehicleDamageCrimeID, "Malicious Vehicle Damage", 1, false, 29, true, false, false) { CanReleaseOnTalkItOut = true, CanReleaseOnCite = true,  ShowsWarning = true, WarningMessage = "You are ~r~causing malicious vehicle damage~s~" };
        HitCarWithCar = new Crime(StaticStrings.HitCarWithCarCrimeID, "Hit and Run", 1, false, 30, false, false, false) { CanReleaseOnTalkItOut = true, CanReleaseOnCite = true, IsTrafficViolation = true, ShowsWarning = true, WarningMessage = "You ~r~crashed your vehicle~s~" };
        PublicIntoxication = new Crime(StaticStrings.PublicIntoxicationCrimeID, "Public Intoxication", 1, false, 31, true, false, false) { CanReleaseOnCite = true, CanReleaseOnTalkItOut = true, GracePeriod = 180000, ShowsWarning = true, WarningMessage = "You are ~r~intoxicated in public~s~~n~Sober up or avoid attention" };
        ChangingPlates = new Crime(StaticStrings.ChangingPlatesCrimeID, "Stealing License Plates", 1, false, 32, true, false, false) { CanReleaseOnCite = true, CanReleaseOnTalkItOut = true, MaxReportingDistance = 20f, ShowsWarning = true, WarningMessage = "You are ~r~stealing license plates~s~~n~Avoid removing plates in public" };
        DrivingAgainstTraffic = new Crime(StaticStrings.DrivingAgainstTrafficCrimeID, "Driving Against Traffic", 1, false, 33, false, false, false) { CanReleaseOnTalkItOut = true, IsTrafficViolation = true, CanReleaseOnCite = true, ShowsWarning = true, WarningMessage = "You are ~r~driving the wrong way~s~" };
        DrivingOnPavement = new Crime(StaticStrings.DrivingOnPavementCrimeID, "Driving On Pavement", 1, false, 34, false, false, false) { CanReleaseOnTalkItOut = true, IsTrafficViolation = true, CanReleaseOnCite = true, ShowsWarning = true, WarningMessage = "You are ~r~driving recklessly~s~" }; 
        NonRoadworthyVehicle = new Crime(StaticStrings.NonRoadworthyVehicleCrimeID, "NonRoadworthy Vehicle", 1, false, 35, false, false, false) { CanReleaseOnTalkItOut = true, GracePeriod = 300000, IsTrafficViolation = true, CanReleaseOnCite = true, ShowsWarning = true, WarningMessage = "This vehicle is not ~r~roadworthy~s~~n~Avoid suspicion by repairing or changing vehicles" };
        RunningARedLight = new Crime(StaticStrings.RunningARedLightCrimeID, "Running a Red Light", 1, false, 36, false, false, false) { CanReleaseOnTalkItOut = true, IsTrafficViolation = true, CanReleaseOnCite = true, ShowsWarning = true, WarningMessage = "You ~r~ran a red light~s~~n~Use indicators/blinkers for turns" };
        FelonySpeeding = new Crime(StaticStrings.FelonySpeedingCrimeID, "Felony Speeding", 1, false, 37, false, false, false) { CanReleaseOnTalkItOut = true, IsTrafficViolation = true, CanReleaseOnCite = true, ShowsWarning = true, WarningMessage = "You are ~r~felony speeding~s~" };
        DrivingStolenVehicle = new Crime(StaticStrings.DrivingStolenVehicleCrimeID, "Driving a Stolen Vehicle", 2, false, 38, false, false, false) { ShowsWarning = true, WarningMessage = "You are driving a ~r~stolen vehicle~s~~n~Reported vehicles attract police attention" };
        SuspiciousActivity = new Crime(StaticStrings.SuspiciousActivityCrimeID, "Suspicious Activity", 1, false, 39, false, false, false) { GracePeriod = 180000, CanReleaseOnTalkItOut = true, CanReleaseOnCleanSearch = true, ShowsWarning = true, WarningMessage = "You are ~r~acting suspicious~s~~n~Leave the area to avoid issues" };  
        InsultingOfficer = new Crime(StaticStrings.InsultingOfficerCrimeID, "Insulting a Police Officer", 2, false, 40, false, false, true);
        Harassment = new Crime(StaticStrings.HarassmentCrimeID, "Harassment", 1, false, 41, true, false, false) { CanReleaseOnTalkItOut = true, CanReleaseOnCleanSearch = true, MaxReportingDistance = 15f, CanReleaseOnCite = true, WarningMessage = "You are ~r~harassing citizens~s~" };
        Speeding = new Crime(StaticStrings.SpeedingCrimeID, "Speeding", 1, false, 44, false, false, false) { CanReleaseOnTalkItOut = true, Enabled = false, IsTrafficViolation = true, CanReleaseOnCite = true, ShowsWarning = true, WarningMessage = "You are ~r~speeding~s~" };
        IndecentExposure = new Crime(StaticStrings.IndecentExposureCrimeID, "Indecent Exposure", 1, false, 46, true, false, false) { MaxReportingDistance = 15f, CanReleaseOnCite = true, ShowsWarning = true, WarningMessage = "You are guilt of ~r~indecent exposure~s~" };
        StandingOnVehicle = new Crime(StaticStrings.StandingOnVehicleCrimeID, "Standing On Vehicle", 1, false, 48, true, false, false) { MaxReportingDistance = 20f, CanReleaseOnCite = true, ShowsWarning = true, WarningMessage = "You are ~r~standing on vehicles~s~", };
        PublicNuisance = new Crime(StaticStrings.PublicNuisanceCrimeID, "Public Nuisance", 1, false, 50, true, false, false) { CanReleaseOnTalkItOut = true, CanReleaseOnCleanSearch = true, MaxReportingDistance = 15f, CanReleaseOnCite = true, ShowsWarning = true, WarningMessage = "You are ~r~being a nuisance~s~" };   
        PublicVagrancy = new Crime(StaticStrings.PublicVagrancyCrimeID, "Public Vagrancy", 1, false, 51, true, false, false) { CanReleaseOnTalkItOut = true, CanReleaseOnCleanSearch = true, MaxReportingDistance = 15f, CanReleaseOnCite = true, ShowsWarning = true, WarningMessage = "You are guilty of ~r~vagrancy~s~~n~Use hotels or residences to sleep" };
        OfficersNeeded = new Crime(StaticStrings.OfficersNeededCrimeID, "Officers Needed", 1, false, 60, false, false, false) { CanReleaseOnTalkItOut = true, CanReleaseOnCleanSearch = true };
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
             TrespassingOnMilitaryBase,
             SevereTrespessing,
             Trespassing,
             VehicleInvasion,
             SuspiciousVehicle,
             GotInAirVehicleDuringChase,
             FiringWeapon,
             FiringSilencedWeapon,
             Kidnapping,
             KillingCivilians,
             ArmedRobbery,
             BankRobbery,
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
             IndecentExposure,
             MaliciousVehicleDamage,
             DrugPossession,
             StandingOnVehicle,
             BuringABody,
             Theft,
             Shoplifting,
        };
        Serialization.SerializeParams(CrimeList, ConfigFileName);
    }

    public void SetEasy()
    {
        IsBackendChanged = true;
        SetDefault();
    }

    public void SetDefault()
    {
        IsBackendChanged = true;
        CrimeList.Clear();
        CrimeList.AddRange(
            new List<Crime> {
             KillingPolice,
             TerroristActivity,
             FiringWeaponNearPolice,
             AimingWeaponAtPolice,
             HurtingPolice,
             SuspiciousVehicle,
             BrandishingHeavyWeapon,
             SevereTrespessing,
             Trespassing,
             VehicleInvasion,
             TrespessingOnGovtProperty,
             TrespassingOnMilitaryBase,
             GotInAirVehicleDuringChase,
             FiringWeapon,
             FiringSilencedWeapon,
             Kidnapping,
             KillingCivilians,
             ArmedRobbery,
             BankRobbery,
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
             IndecentExposure,
             MaliciousVehicleDamage,
             DrugPossession,
             StandingOnVehicle,
             BuringABody,
             Theft,
             Shoplifting,
            });

        HitCarWithCar.CanBeReactedToByCivilians = false;
        DrivingAgainstTraffic.CanBeReactedToByCivilians = false;
        DrivingOnPavement.CanBeReactedToByCivilians = false;
        NonRoadworthyVehicle.CanBeReactedToByCivilians = false;
        RunningARedLight.CanBeReactedToByCivilians = false;
        FelonySpeeding.CanBeReactedToByCivilians = false;

    }
    public void SetHard()
    {
        IsBackendChanged = true;
        SetDefault();
        HitCarWithCar.CanBeReactedToByCivilians = true;
        DrivingAgainstTraffic.CanBeReactedToByCivilians = true;
        DrivingOnPavement.CanBeReactedToByCivilians = true;
        NonRoadworthyVehicle.CanBeReactedToByCivilians = true;
        RunningARedLight.CanBeReactedToByCivilians = true;
        FelonySpeeding.CanBeReactedToByCivilians = true;
    }
    public void SetPreferred()
    {
        IsBackendChanged = true;
        SetDefault();
        //SetHard();
        HitCarWithCar.CanBeReactedToByCivilians = true;
    }
}