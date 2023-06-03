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
    //Zones and Locations
    public static readonly string LosSantosCountyID = "LosSantosCounty";
    public static readonly string CityOfLosSantosCountyID = "CityOfLosSantos";
    public static readonly string BlaineCountyID = "BlaineCounty";
    public static readonly string MajesticCountyID = "MajesticCounty";
    public static readonly string VenturaCountyID = "VenturaCounty";
    public static readonly string ViceCountyID = "Vice";
    public static readonly string CrookCountyID = "Vice";
    public static readonly string SanAndreasStateID = "SanAndreas";
    public static readonly string NorthYanktonCountyID = "NorthYankton";
    public static readonly string NorthYanktonStateID = "NorthYankton";
    public static readonly string PacificOceanCountyID = "PacificOcean";
    public static readonly string AlderneyCountyID = "Alderney";
    public static readonly string AlderneyStateID = "Alderney";  
    public static readonly string LibertyCityCountyID = "LibertyCity";
    public static readonly string LibertyStateID = "Liberty";
    public static readonly string ColombiaStateID = "Colombia";
    public static readonly string FloridaStateID = "Florida";

    //Contacts
    public static readonly string OfficerFriendlyContactName = "Officer Friendly";//these have gotta go, but where?
    public static readonly string UndergroundGunsContactName = "Underground Guns";//these have gotta go, but where?
    public static readonly string EmergencyServicesContactName = "911 - Emergency Services";//these have gotta go, but where?  

    //Crimes
    public static readonly string KillingPoliceCrimeID = "KillingPolice";
    public static readonly string TerroristActivityCrimeID = "TerroristActivity";
    public static readonly string FiringWeaponNearPoliceCrimeID = "FiringWeaponNearPolice";
    public static readonly string AimingWeaponAtPoliceCrimeID = "AimingWeaponAtPolice";
    public static readonly string HurtingPoliceCrimeID = "HurtingPolice";
    public static readonly string BrandishingHeavyWeaponCrimeID = "BrandishingHeavyWeapon";
    public static readonly string TrespessingOnGovtPropertyCrimeID = "TrespessingOnGovtProperty";
    public static readonly string TrespessingCrimeID = "Trespessing";
    public static readonly string GotInAirVehicleDuringChaseCrimeID = "GotInAirVehicleDuringChase";
    public static readonly string FiringWeaponCrimeID = "FiringWeapon";
    public static readonly string FiringSilencedWeaponCrimeID = "FiringSilencedWeapon";
    public static readonly string KidnappingCrimeID = "Kidnapping";
    public static readonly string KillingCiviliansCrimeID = "KillingCivilians";
    public static readonly string ArmedRobberyCrimeID = "ArmedRobbery";
    public static readonly string MuggingCrimeID = "Mugging";
    public static readonly string AttemptingSuicideCrimeID = "AttemptingSuicide";
    public static readonly string HitPedWithCarCrimeID = "HitPedWithCar";
    public static readonly string HurtingCiviliansCrimeID = "HurtingCivilians";
    public static readonly string GrandTheftAutoCrimeID = "GrandTheftAuto";
    public static readonly string BrandishingWeaponCrimeID = "BrandishingWeapon";
    public static readonly string ResistingArrestCrimeID = "ResistingArrest";
    public static readonly string BrandishingCloseCombatWeaponCrimeID = "BrandishingCloseCombatWeapon";
    public static readonly string DrunkDrivingCrimeID = "DrunkDriving";
    public static readonly string AssaultingWithDeadlyWeaponCrimeID = "AssaultingWithDeadlyWeapon";
    public static readonly string AssaultingCiviliansCrimeID = "AssaultingCivilians";
    public static readonly string DealingDrugsCrimeID = "DealingDrugs";
    public static readonly string DealingGunsCrimeID = "DealingGuns";
    public static readonly string HitCarWithCarCrimeID = "HitCarWithCar";
    public static readonly string PublicIntoxicationCrimeID = "PublicIntoxication";
    public static readonly string ChangingPlatesCrimeID = "ChangingPlates";
    public static readonly string DrivingAgainstTrafficCrimeID = "DrivingAgainstTraffic";
    public static readonly string DrivingOnPavementCrimeID = "DrivingOnPavement";
    public static readonly string NonRoadworthyVehicleCrimeID = "NonRoadworthyVehicle";
    public static readonly string RunningARedLightCrimeID = "RunningARedLight";
    public static readonly string FelonySpeedingCrimeID = "FelonySpeeding";
    public static readonly string DrivingStolenVehicleCrimeID = "DrivingStolenVehicle";
    public static readonly string SuspiciousActivityCrimeID = "SuspiciousActivity";
    public static readonly string InsultingOfficerCrimeID = "InsultingOfficer";
    public static readonly string HarassmentCrimeID = "Harassment";
    public static readonly string SpeedingCrimeID = "Speeding";
    public static readonly string PublicNuisanceCrimeID = "PublicNuisance";
    public static readonly string PublicVagrancyCrimeID = "PublicVagrancy";
    public static readonly string OfficersNeededCrimeID = "OfficersNeeded";

    //Air Carriers
    public static readonly string AirHerlerCarrierID = "AirHerler";
    public static readonly string CaipiraAirwaysCarrierID = "CaipiraAirways";
    public static readonly string SanFierroAirCarrierID = "SanFierroAir";
    public static readonly string LosSantosAirCarrierID = "LosSantosAir";
    public static readonly string FlyUSCarrierID = "FlyUS";
    public static readonly string AdiosAirlinesCarrierID = "Adios Airlines";

    //Menus
    public static readonly string DrugDealerMenuID = "DrugDealerMenu";
    public static readonly string DrugCustomerMenuID = "DrugCustomerMenu";

    public static readonly string MarijuanaDealerMenuGroupID = "MarijuanaDealerMenu";
    public static readonly string ToiletCleanerDealerMenuGroupID = "ToiletCleanerDealerMenu";
    public static readonly string SPANKDealerMenuGroupID = "SPANKDealerMenu";
    public static readonly string MethamphetamineDealerMenuGroupID = "MethamphetamineDealerMenu";
    public static readonly string HeroinDealerMenuGroupID = "HeroinDealerMenu";
    public static readonly string CrackDealerMenuGroupID = "CrackDealerMenu";
    public static readonly string CokeDealerMenuGroupID = "CokeDealerMenu";

    public static readonly string MarijuanaCustomerMenuGroupID = "MarijuanaCustomerMenu";
    public static readonly string ToiletCleanerCustomerMenuGroupID = "ToiletCleanerCustomerMenu";
    public static readonly string SPANKCustomerMenuGroupID = "SPANKCustomerMenu";
    public static readonly string MethamphetamineCustomerMenuGroupID = "MethamphetamineCustomerMenu";
    public static readonly string HeroinCustomerMenuGroupID = "HeroinCustomerMenu";
    public static readonly string CrackCustomerMenuGroupID = "CrackCustomerMenu";
    public static readonly string CokeCustomerMenuGroupID = "CokeCustomerMenu";


    //Area Dealer Menus
    public static readonly string PoorAreaDrugDealerMenuGroupID = "PoorAreaDrugDealerMenuGroupID";
    public static readonly string PoorAreaDrugCustomerMenuGroupID = "PoorAreaDrugCustomerMenuGroupID";

    public static readonly string MiddleAreaDrugDealerMenuGroupID = "MiddleAreaDrugDealerMenuGroupID";
    public static readonly string MiddleAreaDrugCustomerMenuGroupID = "MiddleAreaDrugCustomerMenuGroupID";

    public static readonly string RichAreaDrugDealerMenuGroupID = "RichAreaDrugDealerMenuGroupID";
    public static readonly string RichAreaDrugCustomerMenuGroupID = "RichAreaDrugCustomerMenuGroupID";

    public static readonly string HeroinAreaDrugDealerMenuGroupID = "HeroinAreaDrugDealerMenuGroupID";
    public static readonly string HeroinAreaDrugCustomerMenuGroupID = "HeroinAreaDrugCustomerMenuGroupID";

    public static readonly string MarijuanaAreaDrugDealerMenuGroupID = "MarijuanaAreaDrugDealerMenuGroupID";
    public static readonly string MarijuanaAreaDrugCustomerMenuGroupID = "MarijuanaAreaDrugCustomerMenuGroupID";

    public static readonly string ToiletCleanerAreaDrugDealerMenuGroupID = "ToiletCleanerAreaDrugDealerMenuGroupID";
    public static readonly string ToiletCleanerAreaDrugCustomerMenuGroupID = "ToiletCleanerAreaDrugCustomerMenuGroupID";

    public static readonly string SPANKAreaDrugDealerMenuGroupID = "SPANKAreaDrugDealerMenuGroupID";
    public static readonly string SPANKAreaDrugCustomerMenuGroupID = "SPANKAreaDrugCustomerMenuGroupID";

    public static readonly string MethamphetamineAreaDrugDealerMenuGroupID = "MethamphetamineAreaDrugDealerMenuGroupID";
    public static readonly string MethamphetamineAreaDrugCustomerMenuGroupID = "MethamphetamineAreaDrugCustomerMenuGroupID";

    public static readonly string CrackAreaDrugDealerMenuGroupID = "CrackAreaDrugDealerMenuGroupID";
    public static readonly string CrackAreaDrugCustomerMenuGroupID = "CrackAreaDrugCustomerMenuGroupID";

    public static readonly string CokeAreaDrugDealerMenuGroupID = "CokeAreaDrugDealerMenuGroupID";
    public static readonly string CokeAreaDrugCustomerMenuGroupID = "CokeAreaDrugCustomerMenuGroupID";
}

