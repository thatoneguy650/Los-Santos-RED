using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

public class Settings : ISettingsProvideable
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Settings.xml";
    public SettingsManager SettingsManager { get; private set; }



    //[XmlIgnore]
    //public SettingsManager EasySettingsManager { get; private set; }

    [XmlIgnore]
    public SettingsManager DefaultSettingsManager { get; private set; }

    [XmlIgnore]
    public bool IsBackendChanged { get; set; } = false;
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("Settings*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Settings config: {ConfigFile.FullName}", 0);
            SettingsManager = Serialization.DeserializeParam<SettingsManager>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Settings config  {ConfigFileName}", 0);
            SettingsManager = Serialization.DeserializeParam<SettingsManager>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Settings config found, creating default", 0);
            DefaultConfig();
        }
        DefaultSettingsManager = new SettingsManager();
        DefaultSettingsManager.Setup();
        DefaultSettingsManager.SetDefault();
        SettingsManager.Setup();
    }
    public void DefaultConfig()
    {
        SettingsManager = new SettingsManager();
        SettingsManager.Setup();
        Serialization.SerializeParam(SettingsManager, ConfigFileName);
    }
    public void SerializeAllSettings()
    {
        Serialization.SerializeParam(SettingsManager == null ? new SettingsManager() : SettingsManager, ConfigFileName);
    }
    public void SetDefault()
    {
        IsBackendChanged = true;
        SettingsManager.SetDefault();
    }
    public void SetEasy()
    {
        IsBackendChanged = true;
        SettingsManager.SetDefault();


        //Easy Preset

        SettingsManager.PlayerOtherSettings.CorruptCopClearCanAlwaysClearWanted = true;

        SettingsManager.ViolationSettings.ShowCrimeWarnings = true;

        SettingsManager.VehicleSettings.InjureOnWindowBreak = false;

        SettingsManager.VehicleSettings.RequireScrewdriverForLockPickEntry = false;
        SettingsManager.VehicleSettings.RequireScrewdriverForHotwire = false;

        SettingsManager.GangSettings.ShowSpawnedBlip = true;
        SettingsManager.PoliceSpawnSettings.ShowSpawnedBlips = true;
        SettingsManager.EMSSettings.ShowSpawnedBlips = true;
        SettingsManager.FireSettings.ShowSpawnedBlips = true;
        SettingsManager.TaxiSettings.ShowSpawnedBlip = true;
        SettingsManager.SecuritySettings.ShowSpawnedBlips = true;



        SettingsManager.CriminalHistorySettings.RealTimeExpireWantedMultiplier = 500000;
        SettingsManager.CriminalHistorySettings.CalendarTimeExpireWantedMultiplier = 8;


        SettingsManager.PoliceSettings.TalkFailFineAmount = 100;


        SettingsManager.RecoilSettings.ApplyRecoil = false;
        SettingsManager.SwaySettings.ApplySway = false;

        SettingsManager.DamageSettings.AllowInjuryEffects = false;
        SettingsManager.DamageSettings.ModifyPlayerDamage = false;

        SettingsManager.RespawnSettings.RemoveWeaponsOnSurrender = false;

        SettingsManager.RespawnSettings.HospitalStayDailyFee = 1000;
        SettingsManager.RespawnSettings.HospitalStayMinDays = 1;
        SettingsManager.RespawnSettings.HospitalStayMaxDays = 2;

        SettingsManager.RespawnSettings.RemoveWeaponsOnDeath = false;

        SettingsManager.RespawnSettings.DeductMoneyOnFailedBribe = false;
        SettingsManager.RespawnSettings.PoliceBribeWantedLevelScale = 500;
        SettingsManager.RespawnSettings.PoliceBribePoliceKilledMultiplier = 5000;
        SettingsManager.RespawnSettings.PoliceBribePoliceInjuredMultiplier = 200;
        SettingsManager.RespawnSettings.PoliceBailWantedLevelScale = 500;
        SettingsManager.RespawnSettings.PoliceBailPoliceKilledMultiplier = 300;
        SettingsManager.RespawnSettings.PoliceBailPoliceInjuredMultiplier = 150;
        SettingsManager.RespawnSettings.PoliceBailCiviliansKilledMultiplier = 500;


        SettingsManager.RespawnSettings.PoliceBailDurationPoliceKilledMultiplier = 2;
        SettingsManager.RespawnSettings.PoliceBailDurationCiviliansKilledMultiplier = 1;
        SettingsManager.RespawnSettings.ClearIllicitInventoryOnDeath = false;
        SettingsManager.RespawnSettings.ClearIllicitInventoryOnSurrender = false;
        //SettingsManager.RespawnSettings.ShowRequiredBribeAmount = true;
        SettingsManager.RespawnSettings.ImpoundVehicles = false;

        SettingsManager.VehicleSettings.ForceFirstPersonOnVehicleDuck = false;
        SettingsManager.VehicleSettings.LockVehiclePercentage = 30f;
        SettingsManager.VehicleSettings.NonRoadworthyVehicleCheckNoHeadlights = false;
        SettingsManager.VehicleSettings.NonRoadworthyVehicleCheckDamagedHeadlights = false;
        SettingsManager.VehicleSettings.NonRoadworthyVehicleCheckDamagedDoors = false;
        SettingsManager.VehicleSettings.NonRoadworthyVehicleCheckDamagedWindows = false;

        SettingsManager.ScannerSettings.ShowPoliceVehicleBlipsWithScanner = true;
    }
    public void SetPreferred()
    {
        IsBackendChanged = true;
        SettingsManager.SetDefault();

        //Preferred

        SettingsManager.ViolationSettings.ShowCrimeWarnings = true;

        SettingsManager.VehicleSettings.RequireScrewdriverForLockPickEntry = true;
        SettingsManager.VehicleSettings.RequireScrewdriverForHotwire = true;

        SettingsManager.VehicleSettings.DisableAircraftWithoutLicense = true;

        SettingsManager.WorldSettings.AirportsRequireLicenseForPrivateFlights = true;

        SettingsManager.GangSettings.ShowSpawnedBlip = false;
        SettingsManager.PoliceSpawnSettings.ShowSpawnedBlips = false;
        SettingsManager.UIGeneralSettings.ShowDebug = false;
        SettingsManager.VehicleSettings.AutoTuneRadioOnEntry = false;
        SettingsManager.EMSSettings.ShowSpawnedBlips = false;
        SettingsManager.FireSettings.ShowSpawnedBlips = false;

        SettingsManager.CriminalHistorySettings.RealTimeExpireWantedMultiplier = 120000;

        SettingsManager.CriminalHistorySettings.CalendarTimeExpireWantedMultiplier = 24;

        SettingsManager.PlayerOtherSettings.Recognize_BaseTime = 1500;
        SettingsManager.PlayerOtherSettings.Recognize_NightPenalty = 1500;

        SettingsManager.LSRHUDSettings.ZoneDisplayShowPrimaryAgency = false;

        SettingsManager.UIGeneralSettings.AlwaysShowRadar = false;
        SettingsManager.UIGeneralSettings.NeverShowRadar = false;
        SettingsManager.UIGeneralSettings.ShowRadarInVehicleOnly = true;
        SettingsManager.UIGeneralSettings.ShowRadarOnFootWhenCellPhoneActiveOnly = true;
        SettingsManager.UIGeneralSettings.HideLSRUIUnlessActionWheelActive = true;
        SettingsManager.UIGeneralSettings.HideRadarUnlessActionWheelActive = false;

        SettingsManager.ScannerSettings.DisableScannerWithoutRadioItem = true;
        SettingsManager.ScannerSettings.EnableNotifications = false;
        SettingsManager.ScannerSettings.ShowPoliceVehicleBlipsWithScanner = false;


        SettingsManager.WeatherSettings.ReportWeather = false;
        SettingsManager.WeatherSettings.ShowWeatherNotifications = false;

        SettingsManager.TaskSettings.ShowEntityBlips = false;
        SettingsManager.TaskSettings.DisplayHelpPrompts = false;
        
    }
    public void SetHard()
    {
        IsBackendChanged = true;
        SettingsManager.SetDefault();
        //Hard Preset

        SettingsManager.ViolationSettings.ShowCrimeWarnings = false;


        SettingsManager.VehicleSettings.RequireScrewdriverForLockPickEntry = true;
        SettingsManager.VehicleSettings.RequireScrewdriverForHotwire = true;

        SettingsManager.VehicleSettings.DisableAircraftWithoutLicense = true;

        SettingsManager.VehicleSettings.LockVehiclePercentage = 95f;


        SettingsManager.PlayerOtherSettings.PercentageOfGangVehiclesToGetRandomWeapons = 35f;
        SettingsManager.PlayerOtherSettings.PercentageOfPoliceVehiclesToGetRandomWeapons = 35f;
        SettingsManager.PlayerOtherSettings.PercentageToGetRandomCash = 15f;

        SettingsManager.PlayerOtherSettings.RandomCashMin = 1;
        SettingsManager.PlayerOtherSettings.RandomCashMax = 250;

        SettingsManager.PlayerOtherSettings.PercentageToGetRandomWeapons = 15f;
        SettingsManager.PlayerOtherSettings.PercentageToGetRandomItems = 55f;


        SettingsManager.WorldSettings.AirportsRequireLicenseForPrivateFlights = true;

        SettingsManager.GangSettings.ShowSpawnedBlip = false;
        SettingsManager.PoliceSpawnSettings.ShowSpawnedBlips = false;
        SettingsManager.UIGeneralSettings.ShowDebug = false;
        SettingsManager.VehicleSettings.AutoTuneRadioOnEntry = false;
        SettingsManager.EMSSettings.ShowSpawnedBlips = false;
        SettingsManager.FireSettings.ShowSpawnedBlips = false;

        SettingsManager.CriminalHistorySettings.RealTimeExpireWantedMultiplier = 120000;

        SettingsManager.CriminalHistorySettings.CalendarTimeExpireWantedMultiplier = 24;

        SettingsManager.InvestigationSettings.SuspiciousDistance = 800f;
        SettingsManager.PedSwapSettings.PercentageToGetCriminalHistory = 15;
        SettingsManager.PedSwapSettings.PercentageToGetRandomItems = 20;
        SettingsManager.PedSwapSettings.MaxRandomItemsToGet = 1;
        SettingsManager.PedSwapSettings.MaxRandomItemsAmount = 2;
        

        SettingsManager.PlayerOtherSettings.MeleeDamageModifier = 0.5f;
        SettingsManager.PlayerOtherSettings.CorruptCopInvestigationClearCost = 10000;
        SettingsManager.PlayerOtherSettings.CorruptCopWantedClearCostScalar = 10000;
        SettingsManager.PoliceSettings.SearchTimeMultiplier = 60000;

        SettingsManager.PlayerOtherSettings.Recognize_BaseTime = 1500;
        SettingsManager.PlayerOtherSettings.Recognize_NightPenalty = 1500;
        SettingsManager.PlayerOtherSettings.Recognize_VehiclePenalty = 1500;

        SettingsManager.RecoilSettings.VerticalRecoilAdjuster = 1.5f;
        SettingsManager.RecoilSettings.HorizontalRecoilAdjuster = 1.5f;
        SettingsManager.RecoilSettings.VerticalOnFootRecoilAdjuster = 1.5f;
        SettingsManager.RecoilSettings.HorizontalOnFootRecoilAdjuster = 1.5f;
        SettingsManager.RecoilSettings.VerticalInVehicleRecoilAdjuster = 1.5f;
        SettingsManager.RecoilSettings.HorizontalInVehicleRecoilAdjuster = 1.5f;




        SettingsManager.SwaySettings.VeritcalSwayAdjuster = 1.5f;
        SettingsManager.SwaySettings.HorizontalSwayAdjuster = 1.5f;
        SettingsManager.SwaySettings.VeritcalOnFootSwayAdjuster = 1.5f;
        SettingsManager.SwaySettings.HorizontalOnFootSwayAdjuster = 1.5f;
        SettingsManager.SwaySettings.VeritcalInVehicleSwayAdjuster = 1.5f;
        SettingsManager.SwaySettings.HorizontalInVehicleSwayAdjuster = 1.5f;

        SettingsManager.LSRHUDSettings.ZoneDisplayShowPrimaryAgency = false;

        SettingsManager.UIGeneralSettings.AlwaysShowRadar = false;
        SettingsManager.UIGeneralSettings.NeverShowRadar = false;
        SettingsManager.UIGeneralSettings.ShowRadarInVehicleOnly = false;
        SettingsManager.UIGeneralSettings.ShowRadarOnFootWhenCellPhoneActiveOnly = false;


        SettingsManager.UIGeneralSettings.HideLSRUIUnlessActionWheelActive = true;
        SettingsManager.UIGeneralSettings.HideRadarUnlessActionWheelActive = true;

        SettingsManager.ViolationSettings.RecentlyHurtCivilianTime = 10000;
        SettingsManager.ViolationSettings.RecentlyHurtPoliceTime = 10000;
        SettingsManager.ViolationSettings.RecentlyKilledCivilianTime = 10000;
        SettingsManager.ViolationSettings.RecentlyKilledPoliceTime = 10000;
        SettingsManager.ViolationSettings.MurderDistance = 12f;
        SettingsManager.ViolationSettings.RecentlyDrivingAgainstTrafficTime = 2500;
        SettingsManager.ViolationSettings.RecentlyDrivingOnPavementTime = 2500;
        SettingsManager.ViolationSettings.RecentlyHitPedTime = 2500;
        SettingsManager.ViolationSettings.RecentlyHitVehicleTime = 2500;
        SettingsManager.ViolationSettings.ResistingArrestFastTriggerTime = 2000;
        SettingsManager.ViolationSettings.ResistingArrestMediumTriggerTime = 5000;
        SettingsManager.ViolationSettings.ResistingArrestSlowTriggerTime = 15000;

        SettingsManager.DamageSettings.Armor_NormalDamageModifierPlayer = 1.5f;
        SettingsManager.DamageSettings.Armor_GrazeDamageModifierPlayer = 0.5f;
        SettingsManager.DamageSettings.Armor_CriticalDamageModifierPlayer = 3.0f;
        SettingsManager.DamageSettings.Health_FatalDamageModifierPlayer = 100.0f;
        SettingsManager.DamageSettings.Health_NormalDamageModifierPlayer = 2.0f;
        SettingsManager.DamageSettings.Health_GrazeDamageModifierPlayer = 0.75f;
        SettingsManager.DamageSettings.Health_CriticalDamageModifierPlayer = 3.5f;


        SettingsManager.DamageSettings.NormalDamagePercentPlayer = 70f;
        SettingsManager.DamageSettings.GrazeDamagePercentPlayer = 2f;
        SettingsManager.DamageSettings.CriticalDamagePercentPlayer = 20f;
        SettingsManager.DamageSettings.FatalDamagePercentPlayer = 8f;

        SettingsManager.DamageSettings.AllowInjuryEffects = true;
        SettingsManager.DamageSettings.InjuryEffectHealthLostStart = 30;

        SettingsManager.PoliceSettings.GeneralFineAmount = 1000;
        SettingsManager.PoliceSettings.DrivingWithoutLicenseFineAmount = 2000;

        SettingsManager.PoliceSettings.RecentlySeenTime = 10000;
        SettingsManager.PoliceSettings.KillLimit_Wanted4 = 3;
        SettingsManager.PoliceSettings.KillLimit_Wanted5 = 5;
        SettingsManager.PoliceSettings.KillLimit_Wanted6 = 8;
        SettingsManager.PoliceSettings.KillLimit_Wanted7 = 15;
        SettingsManager.PoliceSettings.KillLimit_Wanted8 = 25;
        SettingsManager.PoliceSettings.KillLimit_Wanted9 = 35;
        SettingsManager.PoliceSettings.KillLimit_Wanted10 = 45;


        SettingsManager.PoliceSpawnSettings.MaxDistanceToSpawn_WantedSeen = 500f;
        SettingsManager.PoliceSpawnSettings.MaxDistanceToSpawn_WantedUnseen = 300f;

        SettingsManager.TaskSettings.ShowEntityBlips = false;
        SettingsManager.TaskSettings.DisplayHelpPrompts = false;


        SettingsManager.ScannerSettings.DisableScannerWithoutRadioItem = true;
        SettingsManager.ScannerSettings.EnableNotifications = false;
        SettingsManager.ScannerSettings.ShowPoliceVehicleBlipsWithScanner = false;

        SettingsManager.GangSettings.AllowNonEnemyTargets = false;

    }
    public void SetLC()
    {
        IsBackendChanged = true;
        //SettingsManager.PedSwapSettings.AliasPedAsMainCharacter = false;
        SettingsManager.VanillaSettings.TerminateVanillaBlips = true;
        SettingsManager.VanillaSettings.TerminateVanillaShops = true;



        SettingsManager.PoliceSpawnSettings.MaxDistanceToSpawn_WantedSeen = 350f;// 550f;// 650f;//550f
        SettingsManager.PoliceSpawnSettings.MaxDistanceToSpawn_WantedUnseen = 250f;// 450f;//350f
        SettingsManager.PoliceSpawnSettings.MaxDistanceToSpawn_NotWanted = 550f;// 900f;
        SettingsManager.PoliceSpawnSettings.MinDistanceToSpawn_WantedUnseen = 150f;// 350f;//250f
        SettingsManager.PoliceSpawnSettings.MinDistanceToSpawn_WantedSeen = 200f;// 400f;// 500f;//400f
        SettingsManager.PoliceSpawnSettings.MinDistanceToSpawn_NotWanted = 100f;// 200f;//350f;//150f

        SettingsManager.PoliceSpawnSettings.MinDistanceToSpawn_WantedUnseenScalar = 40f;
        SettingsManager.PoliceSpawnSettings.MinDistanceToSpawn_WantedSeenScalar = 40f;








        SettingsManager.RoadblockSettings.AllowRoadblockOnNonCurrentStreet = true;



        SettingsManager.RoadblockSettings.RoadblockSpawnDistance = 175f;
        SettingsManager.RoadblockSettings.TimeBetweenRoadblock_Unseen = 999999;
        SettingsManager.RoadblockSettings.TimeBetweenRoadblock_Seen_Min = 60000;
        SettingsManager.RoadblockSettings.TimeBetweenRoadblock_Seen_AdditionalTimeScaler = 20000;

        SettingsManager.RoadblockSettings.RoadblockSpawnPercentage_Wanted1 = 0;
        SettingsManager.RoadblockSettings.RoadblockSpawnPercentage_Wanted2 = 55;
        SettingsManager.RoadblockSettings.RoadblockSpawnPercentage_Wanted3 = 95;



        SettingsManager.PoliceTaskSettings.DriveBySightDuringChaseDistance = 50f;
        SettingsManager.PoliceTaskSettings.DriveBySightDuringInvestigateDistance = 50f;
        SettingsManager.PoliceTaskSettings.DriveBySightDuringLocateDistance = 50f;


        SettingsManager.WorldSettings.LowerPedSpawnsAtHigherWantedLevels = false;

        //Vendor
        SettingsManager.CivilianSettings.TotalSpawnedServiceMembersLimit = 10;

        //TAXI 

        SettingsManager.TaxiSettings.TimeBetweenSpawn = 5000;// 20000;//10000;

        SettingsManager.TaxiSettings.TimeBetweenSpawn_DowntownAdditional = 15000;
        SettingsManager.TaxiSettings.TimeBetweenSpawn_WildernessAdditional = 80000;
        SettingsManager.TaxiSettings.TimeBetweenSpawn_RuralAdditional = 50000;
        SettingsManager.TaxiSettings.TimeBetweenSpawn_SuburbAdditional = 20000;
        SettingsManager.TaxiSettings.TimeBetweenSpawn_IndustrialAdditional = 20000;

        SettingsManager.TaxiSettings.MinDistanceToSpawnOnFoot = 100f;//75f// 50f;
        SettingsManager.TaxiSettings.MaxDistanceToSpawnOnFoot = 200f;//200f// 150f;

        SettingsManager.TaxiSettings.MinDistanceToSpawnInVehicle = 100f;//300f// 50f;
        SettingsManager.TaxiSettings.MaxDistanceToSpawnInVehicle = 250f;//500f// 150f;

        SettingsManager.TaxiSettings.TotalSpawnedMembersLimit = 4;// 3;// 2;//5
        SettingsManager.TaxiSettings.TotalSpawnedAmbientMembersLimit = 3;// 2;// 1;// 8;

        SettingsManager.TaxiSettings.TotalSpawnedAmbientMembersLimit_Downtown = 4;
        SettingsManager.TaxiSettings.TotalSpawnedAmbientMembersLimit_Wilderness = 1;
        SettingsManager.TaxiSettings.TotalSpawnedAmbientMembersLimit_Rural = 1;
        SettingsManager.TaxiSettings.TotalSpawnedAmbientMembersLimit_Suburb = 2;
        SettingsManager.TaxiSettings.TotalSpawnedAmbientMembersLimit_Industrial = 2;

        SettingsManager.TaxiSettings.AmbientSpawnPercentage = 10;
        SettingsManager.TaxiSettings.AmbientSpawnPercentage_Wilderness = 1;
        SettingsManager.TaxiSettings.AmbientSpawnPercentage_Rural = 1;
        SettingsManager.TaxiSettings.AmbientSpawnPercentage_Suburb = 10;
        SettingsManager.TaxiSettings.AmbientSpawnPercentage_Industrial = 12;
        SettingsManager.TaxiSettings.AmbientSpawnPercentage_Downtown = 35;
    }
}