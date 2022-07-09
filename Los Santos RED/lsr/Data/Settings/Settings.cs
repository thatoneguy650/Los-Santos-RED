using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System.IO;
using System.Linq;

public class Settings : ISettingsProvideable
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Settings.xml";
    public SettingsManager SettingsManager { get; private set; }
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
    public void SetRelease()
    {
        SettingsManager.GangSettings.ShowSpawnedBlip = false;
        SettingsManager.PoliceSettings.ShowSpawnedBlips = false;
        SettingsManager.UIGeneralSettings.ShowDebug = false;
        SettingsManager.VehicleSettings.AutoTuneRadioOnEntry = false;
        SettingsManager.EMSSettings.ShowSpawnedBlips = false;
        SettingsManager.FireSettings.ShowSpawnedBlips = false;
    }
    public void SetEasy()
    {
        SettingsManager.SetDefault();

        SettingsManager.GangSettings.ShowSpawnedBlip = true;
        SettingsManager.PoliceSettings.ShowSpawnedBlips = true;
        SettingsManager.EMSSettings.ShowSpawnedBlips = true;
    }
    public void SetDefault()
    {
        SettingsManager.SetDefault();
    }
    public void SetPreferred()
    {
        SettingsManager.SetDefault();

        SettingsManager.GangSettings.ShowSpawnedBlip = false;
        SettingsManager.PoliceSettings.ShowSpawnedBlips = false;
        SettingsManager.UIGeneralSettings.ShowDebug = false;
        SettingsManager.VehicleSettings.AutoTuneRadioOnEntry = false;
        SettingsManager.EMSSettings.ShowSpawnedBlips = false;
        SettingsManager.FireSettings.ShowSpawnedBlips = false;

        SettingsManager.CriminalHistorySettings.RealTimeExpireWantedMultiplier = 120000;

        SettingsManager.CriminalHistorySettings.CalendarTimeExpireWantedMultiplier = 24;

        SettingsManager.PlayerOtherSettings.Recognize_BaseTime = 1500;
        SettingsManager.PlayerOtherSettings.Recognize_NightPenalty = 1500;

        SettingsManager.RecoilSettings.ApplyRecoilInVehicle = true;


        SettingsManager.SwaySettings.ApplySwayInVehicle = true;


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
        SettingsManager.WeatherReportingSettings.ReportWeather = false;
        SettingsManager.WeatherReportingSettings.ShowWeatherNotifications = false;

    }
    public void SetHard()
    {
        SettingsManager.SetDefault();


        SettingsManager.GangSettings.ShowSpawnedBlip = false;
        SettingsManager.PoliceSettings.ShowSpawnedBlips = false;
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
        SettingsManager.PlayerOtherSettings.SearchMode_SearchTimeMultiplier = 60000;




        SettingsManager.PlayerOtherSettings.Recognize_BaseTime = 1500;
        SettingsManager.PlayerOtherSettings.Recognize_NightPenalty = 1500;

        SettingsManager.RecoilSettings.ApplyRecoilInVehicle = true;



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

        SettingsManager.SwaySettings.ApplySwayInVehicle = true;


        SettingsManager.LSRHUDSettings.ZoneDisplayShowPrimaryAgency = false;

        SettingsManager.UIGeneralSettings.AlwaysShowRadar = false;
        SettingsManager.UIGeneralSettings.NeverShowRadar = false;
        SettingsManager.UIGeneralSettings.ShowRadarInVehicleOnly = false;
        SettingsManager.UIGeneralSettings.ShowRadarOnFootWhenCellPhoneActiveOnly = false;


        SettingsManager.UIGeneralSettings.HideLSRUIUnlessActionWheelActive = true;
        SettingsManager.UIGeneralSettings.HideRadarUnlessActionWheelActive = true;

        SettingsManager.VehicleSettings.RequireScrewdriverForLockPickEntry = true;
        SettingsManager.VehicleSettings.RequireScrewdriverForHotwire = true;


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
        SettingsManager.PoliceSettings.WantedLevelIncreasesOverTime = true;

        SettingsManager.PoliceSettings.SightDistance_Helicopter_AdditionalAtWanted = 200f;
        SettingsManager.PoliceSettings.MaxDistanceToSpawn_WantedSeen = 500f;
        SettingsManager.PoliceSettings.MaxDistanceToSpawn_WantedUnseen = 300f;


    }
}