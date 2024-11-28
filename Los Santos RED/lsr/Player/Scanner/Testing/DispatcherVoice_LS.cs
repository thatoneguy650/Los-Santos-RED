using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DispatchScannerFiles;


public class DispatcherVoice_LS : IDispatcherVoiceable
{
    public string Name { get; private set; }
    //set a county here? zone? state? can be toggled by each?
    public ScannerDispatchInformation_LS ScannerDispatchInformation_LS { get; private set; }
    public VehicleScannerAudio_LS VehicleScannerAudio_LS { get; private set; }
    public ZoneScannerAudio_LS ZoneScannerAudio { get; private set; }
    public StreetScannerAudio_LS StreetScannerAudio { get; private set; }
    public CallsignScannerAudio_LS CallsignScannerAudio { get; private set; }


    public IVehicleScannerAudio VehicleScannerAudio => VehicleScannerAudio_LS;


    public IScannerDispatchableInformation SDI => ScannerDispatchInformation_LS;
    public List<AudioSet> LethalForce { get; private set; }
    public List<AudioSet> LicensePlateSet { get; private set; }
    public List<AudioSet> AttentionAllUnits { get; private set; }
    public List<AudioSet> OfficersReport { get; private set; }
    public List<AudioSet> CiviliansReport { get; private set; }
    public List<AudioSet> RespondCode2Set { get; private set; }
    public List<AudioSet> RespondCode3Set { get; private set; }
    public List<AudioSet> UnitEnRouteSet { get; private set; }
    public List<CrimeDispatch> DispatchLookup { get; private set; }
    public List<Dispatch> DispatchList { get; private set; } = new List<Dispatch>();



    public List<string> RadioStart;
    public List<string> RadioEnd;

    public DispatcherVoice_LS()
    {
        VehicleScannerAudio_LS = new VehicleScannerAudio_LS();
        StreetScannerAudio = new StreetScannerAudio_LS();
        ZoneScannerAudio = new ZoneScannerAudio_LS();
        CallsignScannerAudio = new CallsignScannerAudio_LS();
    }




    public void Setup()
    {
        VehicleScannerAudio_LS.ReadConfig();
        StreetScannerAudio.ReadConfig();
        ZoneScannerAudio.ReadConfig();
        CallsignScannerAudio.ReadConfig();

        SetupDispatches();

        DispatchLookup = new List<CrimeDispatch>
        {
            new CrimeDispatch("AttemptingSuicide",ScannerDispatchInformation_LS.AttemptingSuicide),
            new CrimeDispatch("BrandishingWeapon",ScannerDispatchInformation_LS.CarryingWeapon),
            new CrimeDispatch("ChangingPlates",ScannerDispatchInformation_LS.TamperingWithVehicle),
            new CrimeDispatch("DrivingAgainstTraffic",ScannerDispatchInformation_LS.RecklessDriving),
            new CrimeDispatch("DrivingOnPavement",ScannerDispatchInformation_LS.RecklessDriving),
            new CrimeDispatch("FelonySpeeding",ScannerDispatchInformation_LS.FelonySpeeding),
            new CrimeDispatch(StaticStrings.FiringWeaponCrimeID,ScannerDispatchInformation_LS.ShotsFired),
            new CrimeDispatch(StaticStrings.FiringSilencedWeaponCrimeID,ScannerDispatchInformation_LS.ShotsFired),
            new CrimeDispatch("FiringWeaponNearPolice",ScannerDispatchInformation_LS.ShotsFiredAtAnOfficer),
            new CrimeDispatch("GotInAirVehicleDuringChase",ScannerDispatchInformation_LS.StealingAirVehicle),
            new CrimeDispatch("GrandTheftAuto",ScannerDispatchInformation_LS.GrandTheftAuto),
            new CrimeDispatch("HitCarWithCar",ScannerDispatchInformation_LS.VehicleHitAndRun),
            new CrimeDispatch("HitPedWithCar",ScannerDispatchInformation_LS.PedHitAndRun),
            new CrimeDispatch("RunningARedLight",ScannerDispatchInformation_LS.RunningARedLight),
            new CrimeDispatch("HurtingCivilians",ScannerDispatchInformation_LS.CivilianInjury),
            new CrimeDispatch("HurtingPolice",ScannerDispatchInformation_LS.AssaultingOfficer),
            new CrimeDispatch("KillingCivilians",ScannerDispatchInformation_LS.CivilianDown),
            new CrimeDispatch("KillingPolice",ScannerDispatchInformation_LS.OfficerDown),
            new CrimeDispatch("Mugging",ScannerDispatchInformation_LS.Mugging),
            new CrimeDispatch("NonRoadworthyVehicle",ScannerDispatchInformation_LS.SuspiciousVehicle),
            new CrimeDispatch("ResistingArrest",ScannerDispatchInformation_LS.ResistingArrest),
            new CrimeDispatch(StaticStrings.TrespessingOnGovtPropertyCrimeID,ScannerDispatchInformation_LS.TrespassingOnGovernmentProperty),


            new CrimeDispatch(StaticStrings.TrespassingOnMilitaryBaseCrimeID,ScannerDispatchInformation_LS.TrespassingOnMilitaryBase),

            new CrimeDispatch(StaticStrings.TrespessingCrimeID,ScannerDispatchInformation_LS.Trespassing),
            new CrimeDispatch(StaticStrings.CivilianTrespessingCrimeID,ScannerDispatchInformation_LS.Trespassing),
            new CrimeDispatch(StaticStrings.VehicleInvasionCrimeID,ScannerDispatchInformation_LS.SuspiciousActivity),

            new CrimeDispatch(StaticStrings.SuspiciousVehicleCrimeID,ScannerDispatchInformation_LS.SuspiciousActivity),

            new CrimeDispatch("DrivingStolenVehicle",ScannerDispatchInformation_LS.DrivingAtStolenVehicle),
            new CrimeDispatch("TerroristActivity",ScannerDispatchInformation_LS.TerroristActivity),
            new CrimeDispatch("BrandishingCloseCombatWeapon",ScannerDispatchInformation_LS.CarryingWeapon),
            new CrimeDispatch("SuspiciousActivity",ScannerDispatchInformation_LS.SuspiciousActivity),
            new CrimeDispatch("DrunkDriving",ScannerDispatchInformation_LS.DrunkDriving),
            new CrimeDispatch("Kidnapping",ScannerDispatchInformation_LS.Kidnapping),
            new CrimeDispatch("PublicIntoxication",ScannerDispatchInformation_LS.PublicIntoxication),
            new CrimeDispatch("InsultingOfficer",ScannerDispatchInformation_LS.OfficerNeedsAssistance),//these are bad
            new CrimeDispatch("OfficersNeeded",ScannerDispatchInformation_LS.OfficersNeeded),
            new CrimeDispatch("Harassment",ScannerDispatchInformation_LS.Harassment),
            new CrimeDispatch("AssaultingCivilians",ScannerDispatchInformation_LS.AssaultingCivilians),
            new CrimeDispatch("AssaultingWithDeadlyWeapon",ScannerDispatchInformation_LS.AssaultingCiviliansWithDeadlyWeapon),
            new CrimeDispatch("DealingDrugs",ScannerDispatchInformation_LS.DealingDrugs),
            new CrimeDispatch("DealingGuns",ScannerDispatchInformation_LS.DealingGuns),
            new CrimeDispatch("AimingWeaponAtPolice",ScannerDispatchInformation_LS.AimingWeaponAtPolice),
            new CrimeDispatch(StaticStrings.ArmedRobberyCrimeID,ScannerDispatchInformation_LS.ArmedRobbery),
            new CrimeDispatch(StaticStrings.BankRobberyCrimeID,ScannerDispatchInformation_LS.BankRobbery),
            new CrimeDispatch("PublicNuisance",ScannerDispatchInformation_LS.PublicNuisance),
            new CrimeDispatch("Speeding",ScannerDispatchInformation_LS.Speeding),
            new CrimeDispatch("PublicVagrancy",ScannerDispatchInformation_LS.PublicVagrancy),
            new CrimeDispatch(StaticStrings.IndecentExposureCrimeID,ScannerDispatchInformation_LS.IndecentExposure),
            new CrimeDispatch(StaticStrings.MaliciousVehicleDamageCrimeID,ScannerDispatchInformation_LS.MaliciousVehicleDamage),
            new CrimeDispatch(StaticStrings.DrugPossessionCrimeID,ScannerDispatchInformation_LS.DrugPossession),
            new CrimeDispatch(StaticStrings.StandingOnVehicleCrimeID,ScannerDispatchInformation_LS.StandingOnVehicle),
            new CrimeDispatch(StaticStrings.BuryingABody,ScannerDispatchInformation_LS.UnlawfulBodyDisposal),

            new CrimeDispatch(StaticStrings.TheftCrimeID,ScannerDispatchInformation_LS.TheftDispatch),
            new CrimeDispatch(StaticStrings.ShopliftingCrimeID,ScannerDispatchInformation_LS.Shoplifting),
        };
        DispatchList = new List<Dispatch>
        {
            ScannerDispatchInformation_LS.OfficerDown
            ,ScannerDispatchInformation_LS.ShotsFiredAtAnOfficer
            ,ScannerDispatchInformation_LS.AssaultingOfficer
            ,ScannerDispatchInformation_LS.ThreateningOfficerWithFirearm
            ,ScannerDispatchInformation_LS.TrespassingOnGovernmentProperty
            ,ScannerDispatchInformation_LS.TrespassingOnMilitaryBase
            ,ScannerDispatchInformation_LS.Trespassing
            ,ScannerDispatchInformation_LS.StealingAirVehicle
            ,ScannerDispatchInformation_LS.ShotsFired
            ,ScannerDispatchInformation_LS.CarryingWeapon
            ,ScannerDispatchInformation_LS.CivilianDown
            ,ScannerDispatchInformation_LS.CivilianShot
            ,ScannerDispatchInformation_LS.CivilianInjury
            ,ScannerDispatchInformation_LS.GrandTheftAuto
            ,ScannerDispatchInformation_LS.SuspiciousActivity
            ,ScannerDispatchInformation_LS.CriminalActivity
            ,ScannerDispatchInformation_LS.Mugging
            ,ScannerDispatchInformation_LS.TerroristActivity
            ,ScannerDispatchInformation_LS.SuspiciousVehicle
            ,ScannerDispatchInformation_LS.DrivingAtStolenVehicle
            ,ScannerDispatchInformation_LS.ResistingArrest
            ,ScannerDispatchInformation_LS.AttemptingSuicide
            ,ScannerDispatchInformation_LS.FelonySpeeding
            ,ScannerDispatchInformation_LS.Speeding
            ,ScannerDispatchInformation_LS.PedHitAndRun
            ,ScannerDispatchInformation_LS.VehicleHitAndRun
            ,ScannerDispatchInformation_LS.RecklessDriving
            ,ScannerDispatchInformation_LS.AnnounceStolenVehicle
            ,ScannerDispatchInformation_LS.RequestAirSupport
            ,ScannerDispatchInformation_LS.RequestMilitaryUnits
            ,ScannerDispatchInformation_LS.RequestNOOSEUnits
            ,ScannerDispatchInformation_LS.SuspectSpotted
            ,ScannerDispatchInformation_LS.SuspectSpottedSimple
            ,ScannerDispatchInformation_LS.SuspectEvaded
            ,ScannerDispatchInformation_LS.SuspectEvadedSimple
            ,ScannerDispatchInformation_LS.RemainInArea
            ,ScannerDispatchInformation_LS.ResumePatrol
            ,ScannerDispatchInformation_LS.AttemptToReacquireSuspect
            ,ScannerDispatchInformation_LS.NoFurtherUnitsNeeded
            ,ScannerDispatchInformation_LS.SuspectArrested
            ,ScannerDispatchInformation_LS.SuspectWasted
            ,ScannerDispatchInformation_LS.ChangedVehicles
            ,ScannerDispatchInformation_LS.RequestBackup
            ,ScannerDispatchInformation_LS.RequestBackupSimple
            ,ScannerDispatchInformation_LS.WeaponsFree
            ,ScannerDispatchInformation_LS.LethalForceAuthorized
            ,ScannerDispatchInformation_LS.RunningARedLight
            ,ScannerDispatchInformation_LS.DrunkDriving
            ,ScannerDispatchInformation_LS.Kidnapping
            ,ScannerDispatchInformation_LS.PublicIntoxication
            ,ScannerDispatchInformation_LS.Harassment
            ,ScannerDispatchInformation_LS.OfficerNeedsAssistance
            ,ScannerDispatchInformation_LS.OfficersNeeded
            ,ScannerDispatchInformation_LS.AssaultingCivilians
            ,ScannerDispatchInformation_LS.AssaultingCiviliansWithDeadlyWeapon
            ,ScannerDispatchInformation_LS.DealingDrugs
            ,ScannerDispatchInformation_LS.DealingGuns
            ,ScannerDispatchInformation_LS.WantedSuspectSpotted
            ,ScannerDispatchInformation_LS.RequestNooseUnitsAlt
            ,ScannerDispatchInformation_LS.RequestNooseUnitsAlt2
            ,ScannerDispatchInformation_LS.RequestFIBUnits
            ,ScannerDispatchInformation_LS.RequestSwatAirSupport
            ,ScannerDispatchInformation_LS.AimingWeaponAtPolice
            ,ScannerDispatchInformation_LS.OnFoot
            ,ScannerDispatchInformation_LS.ExcessiveSpeed
            ,ScannerDispatchInformation_LS.GotOnFreeway
            ,ScannerDispatchInformation_LS.GotOffFreeway
            ,ScannerDispatchInformation_LS.WentInTunnel
            ,ScannerDispatchInformation_LS.TamperingWithVehicle
            ,ScannerDispatchInformation_LS.VehicleCrashed
            ,ScannerDispatchInformation_LS.VehicleStartedFire
            ,ScannerDispatchInformation_LS.ArmedRobbery
            ,ScannerDispatchInformation_LS.BankRobbery
            ,ScannerDispatchInformation_LS.MedicalServicesRequired
            ,ScannerDispatchInformation_LS.FirefightingServicesRequired
            ,ScannerDispatchInformation_LS.PublicNuisance
            ,ScannerDispatchInformation_LS.CivilianReportUpdate
            ,ScannerDispatchInformation_LS.ShotsFiredStatus
            ,ScannerDispatchInformation_LS.PublicVagrancy
            ,ScannerDispatchInformation_LS.IndecentExposure
            ,ScannerDispatchInformation_LS.MaliciousVehicleDamage
            ,ScannerDispatchInformation_LS.DrugPossession
            ,ScannerDispatchInformation_LS.StandingOnVehicle
            ,ScannerDispatchInformation_LS.UnlawfulBodyDisposal
            ,ScannerDispatchInformation_LS.StoppingTrains
            ,ScannerDispatchInformation_LS.TheftDispatch
            ,ScannerDispatchInformation_LS.Shoplifting
        };


    }

    private void SetupDispatches()
    {
        RadioStart = new List<string>() { AudioBeeps.Radio_Start_1.FileName };
        RadioEnd = new List<string>() { AudioBeeps.Radio_End_1.FileName };
        AttentionAllUnits = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { attention_all_units_gen.Attentionallunits.FileName},"attention all units"),
                new AudioSet(new List<string>() { attention_all_units_gen.Attentionallunits1.FileName },"attention all units"),
                new AudioSet(new List<string>() { attention_all_units_gen.Attentionallunits3.FileName },"attention all units"),
            };
        OfficersReport = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { we_have.OfficersReport_1.FileName},"officers report"),
                new AudioSet(new List<string>() { we_have.OfficersReport_2.FileName },"officers report"),
                new AudioSet(new List<string>() { we_have.UnitsReport_1.FileName },"units report"),
            };
        CiviliansReport = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { we_have.CitizensReport_1.FileName },"citizens report"),
                new AudioSet(new List<string>() { we_have.CitizensReport_2.FileName },"citizens report"),
                new AudioSet(new List<string>() { we_have.CitizensReport_3.FileName },"citizens report"),
                new AudioSet(new List<string>() { we_have.CitizensReport_4.FileName },"citizens report"),
            };
        LethalForce = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { lethal_force.Useofdeadlyforceauthorized.FileName},"use of deadly force authorized"),
                new AudioSet(new List<string>() { lethal_force.Useofdeadlyforceisauthorized.FileName },"use of deadly force is authorized"),
                new AudioSet(new List<string>() { lethal_force.Useofdeadlyforceisauthorized1.FileName },"use of deadly force is authorized"),
                new AudioSet(new List<string>() { lethal_force.Useoflethalforceisauthorized.FileName },"use of lethal force is authorized"),
                new AudioSet(new List<string>() { lethal_force.Useofdeadlyforcepermitted1.FileName },"use of deadly force permitted"),
            };
        LicensePlateSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { suspect_license_plate.SuspectLicensePlate.FileName},"suspect license plate"),
                new AudioSet(new List<string>() { suspect_license_plate.SuspectsLicensePlate01.FileName },"suspects license plate"),
                new AudioSet(new List<string>() { suspect_license_plate.SuspectsLicensePlate02.FileName },"suspects license plate"),
                new AudioSet(new List<string>() { suspect_license_plate.TargetLicensePlate.FileName },"target license plate"),
                new AudioSet(new List<string>() { suspect_license_plate.TargetsLicensePlate.FileName },"targets license plate"),
                new AudioSet(new List<string>() { suspect_license_plate.TargetVehicleLicensePlate.FileName },"target vehicle license plate"),
            };
        RespondCode3Set = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { dispatch_respond_code.UnitsrespondCode3.FileName },"units respond code-3"),
                new AudioSet(new List<string>() { dispatch_respond_code.RespondCode3.FileName },"units respond code-3"),
                new AudioSet(new List<string>() { dispatch_respond_code.UnitrespondCode3.FileName },"units respond code-3"),
            };
        RespondCode2Set = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { dispatch_respond_code.RespondCode2.FileName },"units respond code-2"),
                new AudioSet(new List<string>() { dispatch_respond_code.UnitrespondCode2.FileName },"units respond code-2"),
                new AudioSet(new List<string>() { dispatch_respond_code.UnitsrespondCode2.FileName },"units respond code-2"),
            };

        UnitEnRouteSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { s_m_y_cop_black_full_02.RogerEnRoute1.FileName},"Copy Dispatch."),
                new AudioSet(new List<string>() { s_m_y_cop_black_mini_01.RogerEnRoute1.FileName},"Copy that we are on our way."),
               // new AudioSet(new List<string>() { s_m_y_cop_black_mini_02.RogerEnRoute1.FileName},"we are en route"),//victor 13
                new AudioSet(new List<string>() { s_m_y_cop_black_mini_03.RogerEnRoute1.FileName},"Acknowledged, on our way."),
                //new AudioSet(new List<string>() { s_m_y_cop_black_mini_04.RogerEnRoute1.FileName},"we are en route"),//ocean-1
                new AudioSet(new List<string>() { s_m_y_cop_white_full_01.RogerEnRoute1.FileName},"Copy that we are on our way."),
                new AudioSet(new List<string>() { s_m_y_cop_white_full_02.RogerEnRoute1.FileName},"Copy that we are on our way."),
                new AudioSet(new List<string>() { s_m_y_cop_white_mini_02.RogerEnRoute1.FileName},"Copy that we are on our way."),
                //new AudioSet(new List<string>() { s_m_y_cop_white_mini_03.RogerEnRoute1.FileName},"we are en route"),//Specific unit
                //new AudioSet(new List<string>() { s_m_y_cop_white_mini_04.RogerEnRoute1.FileName},"we are en route"),//Specific unit
                new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_01.RogerEnRoute1.FileName},"Copy that we are on our way."),
                //new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_02.RogerEnRoute1.FileName},"we are en route"),//Specific unit
               // new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_01.RogerEnRoute1.FileName},"we are en route"),//Specific unit
               // new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_02.RogerEnRoute1.FileName},"we are en route"),//Specific unit
            };
        ScannerDispatchInformation_LS = new ScannerDispatchInformation_LS();
        ScannerDispatchInformation_LS.Setup();
    }

    public CrimeDispatch GetCrimeDispatch(Crime crimeAssociated)
    {
        return null;
    }
}

