using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DispatchScannerFiles;


public static class StaticStrings
{
    public static readonly string LosSantosCountyID = "LosSantosCounty";
    public static readonly string CityOfLosSantosCountyID = "CityOfLosSantos";
    public static readonly string BlaineCountyID = "BlaineCounty";
    public static readonly string MajesticCountyID = "MajesticCounty";
    public static readonly string SanAndreasStateID = "San Andreas";

    public static readonly string NorthYanktonCountyID = "NorthYankton";
    public static readonly string NorthYanktonStateID = "North Yankton";

    public static readonly string PacificOceanCountyID = "PacificOcean";

    public static readonly string AlderneyCountyID = "Alderney";
    public static readonly string AlderneyStateID = "Alderney";
    
    public static readonly string LibertyCityCountyID = "LibertyCity";
    public static readonly string LibertyStateID = "Liberty";

    public static string ColombiaStateID => "Colombia";
    public static string FloridaStateID => "Florida";

    public static string OfficerFriendlyContactName => "Officer Friendly";//these have gotta go, but where?
    public static string UndergroundGunsContactName => "Underground Guns";//these have gotta go, but where?
    public static string EmergencyServicesContactName => "911 - Emergency Services";//these have gotta go, but where?  


    public static string KillingPoliceCrimeID => "KillingPolice";
    public static string TerroristActivityCrimeID => "TerroristActivity";
    public static string FiringWeaponNearPoliceCrimeID => "FiringWeaponNearPolice";
    public static string AimingWeaponAtPoliceCrimeID => "AimingWeaponAtPolice";
    public static string HurtingPoliceCrimeID => "HurtingPolice";
    public static string BrandishingHeavyWeaponCrimeID => "BrandishingHeavyWeapon";
    public static string TrespessingOnGovtPropertyCrimeID => "TrespessingOnGovtProperty";
    public static string GotInAirVehicleDuringChaseCrimeID => "GotInAirVehicleDuringChase";
    public static string FiringWeaponCrimeID => "FiringWeapon";
    public static string KidnappingCrimeID => "Kidnapping";
    public static string KillingCiviliansCrimeID => "KillingCivilians";
    public static string ArmedRobberyCrimeID => "ArmedRobbery";
    public static string MuggingCrimeID => "Mugging";
    public static string AttemptingSuicideCrimeID => "AttemptingSuicide";
    public static string HitPedWithCarCrimeID => "HitPedWithCar";
    public static string HurtingCiviliansCrimeID => "HurtingCivilians";
    public static string GrandTheftAutoCrimeID => "GrandTheftAuto";
    public static string BrandishingWeaponCrimeID => "BrandishingWeapon";
    public static string ResistingArrestCrimeID => "ResistingArrest";
    public static string BrandishingCloseCombatWeaponCrimeID => "BrandishingCloseCombatWeapon";
    public static string DrunkDrivingCrimeID => "DrunkDriving";
    public static string AssaultingWithDeadlyWeaponCrimeID => "AssaultingWithDeadlyWeapon";
    public static string AssaultingCiviliansCrimeID => "AssaultingCivilians";
    public static string DealingDrugsCrimeID => "DealingDrugs";
    public static string DealingGunsCrimeID => "DealingGuns";
    public static string HitCarWithCarCrimeID => "HitCarWithCar";
    public static string PublicIntoxicationCrimeID => "PublicIntoxication";
    public static string ChangingPlatesCrimeID => "ChangingPlates";
    public static string DrivingAgainstTrafficCrimeID => "DrivingAgainstTraffic";
    public static string DrivingOnPavementCrimeID => "DrivingOnPavement";
    public static string NonRoadworthyVehicleCrimeID => "NonRoadworthyVehicle";
    public static string RunningARedLightCrimeID => "RunningARedLight";
    public static string FelonySpeedingCrimeID => "FelonySpeeding";
    public static string DrivingStolenVehicleCrimeID => "DrivingStolenVehicle";
    public static string SuspiciousActivityCrimeID => "SuspiciousActivity";
    public static string InsultingOfficerCrimeID => "InsultingOfficer";
    public static string HarassmentCrimeID => "Harassment";
    public static string SpeedingCrimeID => "Speeding";
    public static string PublicNuisanceCrimeID => "PublicNuisance";
    public static string PublicVagrancyCrimeID => "PublicVagrancy";
    public static string OfficersNeededCrimeID => "OfficersNeeded";

    public static string AirHerlerCarrierID => "AirHerler";
    public static string CaipiraAirwaysCarrierID => "CaipiraAirways";
    public static string SanFierroAirCarrierID => "SanFierroAir";
    public static string LosSantosAirCarrierID => "LosSantosAir";
    public static string FlyUSCarrierID => "FlyUS";
    public static string AdiosAirlinesCarrierID => "Adios Airlines";



}

