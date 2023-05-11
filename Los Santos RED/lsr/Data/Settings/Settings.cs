using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

public class Settings : ISettingsProvideable
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Settings.xml";
    public SettingsManager SettingsManager { get; private set; }
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

    public void SetEasy()
    {
        IsBackendChanged = true;
        SettingsManager.SetDefault();

        SettingsManager.GangSettings.ShowSpawnedBlip = true;
        SettingsManager.PoliceSpawnSettings.ShowSpawnedBlips = true;
        SettingsManager.EMSSettings.ShowSpawnedBlips = true;

        SettingsManager.GangSettings.ShowAmbientBlips = true;
        SettingsManager.PoliceSettings.TalkFailFineAmount = 100;

        SettingsManager.RespawnSettings.ShowRequiredBribeAmount = true;





    }
    public void SetDefault()
    {
        IsBackendChanged = true;
        SettingsManager.SetDefault();
    }
    public void SetPreferred()
    {
        IsBackendChanged = true;
        SettingsManager.SetDefault();


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

       // SettingsManager.RecoilSettings.ApplyRecoilInVehicle = true;


       // SettingsManager.SwaySettings.ApplySwayInVehicle = true;


        SettingsManager.LSRHUDSettings.ZoneDisplayShowPrimaryAgency = false;

        SettingsManager.UIGeneralSettings.AlwaysShowRadar = false;
        SettingsManager.UIGeneralSettings.NeverShowRadar = false;
        SettingsManager.UIGeneralSettings.ShowRadarInVehicleOnly = false;
        SettingsManager.UIGeneralSettings.ShowRadarOnFootWhenCellPhoneActiveOnly = false;


        SettingsManager.UIGeneralSettings.HideLSRUIUnlessActionWheelActive = true;
        SettingsManager.UIGeneralSettings.HideRadarUnlessActionWheelActive = true;


        SettingsManager.ScannerSettings.EnableNotifications = false;


        SettingsManager.ActionWheelSettings.MessagesToShow = 5;


        SettingsManager.VehicleSettings.AutoTuneRadioOnEntry = false;
        SettingsManager.VehicleSettings.KeepRadioAutoTuned = false;
        SettingsManager.WeatherSettings.ReportWeather = false;
        SettingsManager.WeatherSettings.ShowWeatherNotifications = false;

        


    }
    public void SetLC()
    {
        IsBackendChanged = true;
        SettingsManager.PedSwapSettings.AliasPedAsMainCharacter = false;
        SettingsManager.VanillaSettings.TerminateVanillaBlips = true;
        SettingsManager.VanillaSettings.TerminateVanillaShops = true;
        SettingsManager.VanillaSettings.BlockVanillaPoliceCarGenerators = false;
        SettingsManager.VanillaSettings.BlockGangScenarios = false;
        SettingsManager.VanillaSettings.BlockVanillaPoliceAndSecurityScenarios = false;
    }
    public void SetHard()
    {
        IsBackendChanged = true;
        SettingsManager.SetDefault();

        SettingsManager.ScannerSettings.DisableScannerWithoutRadioItem = true;

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

       // SettingsManager.RecoilSettings.ApplyRecoilInVehicle = true;



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

       // SettingsManager.SwaySettings.ApplySwayInVehicle = true;


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

        SettingsManager.PoliceSettings.WantedLevelIncreasesOverTime = true;

        SettingsManager.PoliceSettings.SightDistance_Helicopter_AdditionalAtWanted = 200f;
        SettingsManager.PoliceSpawnSettings.MaxDistanceToSpawn_WantedSeen = 500f;
        SettingsManager.PoliceSpawnSettings.MaxDistanceToSpawn_WantedUnseen = 300f;





    }
}