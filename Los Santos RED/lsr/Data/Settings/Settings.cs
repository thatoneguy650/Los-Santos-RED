using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System.IO;

public class Settings : ISettingsProvideable
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Settings.xml";
    public SettingsManager SettingsManager { get; private set; }
    public void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            SettingsManager = Serialization.DeserializeParam<SettingsManager>(ConfigFileName);
        }
        else
        {
            SettingsManager = new SettingsManager();
            Serialization.SerializeParam(SettingsManager, ConfigFileName);
        }
    }
    public void SerializeAllSettings()
    {
        Serialization.SerializeParam(SettingsManager == null ? new SettingsManager() : SettingsManager, ConfigFileName);
    }


    public void SetReleaseSettings()
    {
        SettingsManager.GangSettings.ShowSpawnedBlip = false;
        SettingsManager.PoliceSettings.ShowSpawnedBlips = false;
        SettingsManager.UISettings.ShowDebug = false;
        SettingsManager.VehicleSettings.AutoTuneRadioOnEntry = false;
        SettingsManager.EMSSettings.ShowSpawnedBlips = false;
        SettingsManager.FireSettings.ShowSpawnedBlips = false;
    }
    public void SetHardcoreSettings()
    {
        SettingsManager.GangSettings.ShowSpawnedBlip = false;
        SettingsManager.PoliceSettings.ShowSpawnedBlips = false;
        SettingsManager.UISettings.ShowDebug = false;
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


        SettingsManager.UISettings.ZoneDisplayShowPrimaryAgency = false;

        SettingsManager.UISettings.AlwaysShowRadar = false;
        SettingsManager.UISettings.NeverShowRadar = false;
        SettingsManager.UISettings.ShowRadarInVehicleOnly = true;
        SettingsManager.UISettings.ShowRadarOnFootWhenCellPhoneActiveOnly = true;


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



        SettingsManager.DamageSettings.Armor_NormalDamageModifier = 1.5f;
        SettingsManager.DamageSettings.Armor_GrazeDamageModifier = 0.5f;
        SettingsManager.DamageSettings.Armor_CriticalDamageModifier = 3.0f;
        SettingsManager.DamageSettings.Health_FatalDamageModifier = 100.0f;
        SettingsManager.DamageSettings.Health_NormalDamageModifier = 2.0f;
        SettingsManager.DamageSettings.Health_GrazeDamageModifier = 0.75f;
        SettingsManager.DamageSettings.Health_CriticalDamageModifier = 3.5f;


        SettingsManager.DamageSettings.NormalDamagePercent = 70f;
        SettingsManager.DamageSettings.GrazeDamagePercent = 2f;
        SettingsManager.DamageSettings.CriticalDamagePercent = 20f;
        SettingsManager.DamageSettings.FatalDamagePercent = 8f;




        SettingsManager.DamageSettings.AllowInjuryEffects = false;
        SettingsManager.DamageSettings.InjuryEffectHealthLostStart = 30;




        SettingsManager.PoliceSettings.GeneralFineAmount = 1000;
        SettingsManager.PoliceSettings.DrivingWithoutLicenseFineAmount = 2000;


        SettingsManager.PoliceSettings.RecentlySeenTime = 10000;
        SettingsManager.PoliceSettings.KillLimit_Wanted4 = 3;
        SettingsManager.PoliceSettings.KillLimit_Wanted5 = 5;
        SettingsManager.PoliceSettings.KillLimit_Wanted6 = 8;
        SettingsManager.PoliceSettings.WantedLevelIncreasesOverTime = true;
        SettingsManager.PoliceSettings.WantedLevelIncreaseTime = 180000;
        SettingsManager.PoliceSettings.SightDistance_Helicopter_AdditionalAtWanted = 200f;
        SettingsManager.PoliceSettings.MaxDistanceToSpawn_WantedSeen = 500f;
        SettingsManager.PoliceSettings.MaxDistanceToSpawn_WantedUnseen = 300f;


    }



}