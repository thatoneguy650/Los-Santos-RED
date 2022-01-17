using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class UISettings : ISettingsDefaultable
{
    public bool UIEnabled { get; set; }
    public bool ShowCrimesDisplay { get; set; }
    public float CrimesViolatingPositionX { get; set; }
    public float CrimesViolatingPositionY { get; set; }
    public float CrimesViolatingScale { get; set; }
    public int CrimesViolatingJustificationID { get; set; }
    public GTAFont CrimesViolatingFont { get; set; }
    public bool ShowSpeedLimitDisplay { get; set; }
    public float SpeedLimitPositionX { get; set; }
    public float SpeedLimitPositionY { get; set; }
    public float SpeedLimitScale { get; set; }
    public bool ShowVehicleStatusDisplay { get; set; }
    public bool FadeVehicleStatusDisplay { get; set; }
    public bool FadeVehicleStatusDisplayDuringWantedAndInvestigation { get; set; }
    public uint VehicleStatusTimeToShow { get; set; }
    public uint VehicleStatusTimeToFade { get; set; }
    public float VehicleStatusPositionX { get; set; }
    public float VehicleStatusPositionY { get; set; }
    public float VehicleStatusScale { get; set; }
    public int VehicleStatusJustificationID { get; set; }
    public GTAFont VehicleStatusFont { get; set; }
    public bool VehicleStatusIncludeTextSpeedLimit { get; set; }
    public bool ShowPlayerDisplay { get; set; }
    public bool FadePlayerDisplay { get; set; }
    public bool FadePlayerDisplayDuringWantedAndInvestigation { get; set; }
    public uint PlayerDisplayTimeToShow { get; set; }
    public uint PlayerDisplayTimeToFade { get; set; }
    public float PlayerStatusPositionX { get; set; }
    public float PlayerStatusPositionY { get; set; }
    public float PlayerStatusScale { get; set; }
    public int PlayerStatusJustificationID { get; set; }
    public GTAFont PlayerStatusFont { get; set; }
    public bool PlayerStatusIncludeTime { get; set; }
    public bool PlayerStatusSimpleTime { get; set; }
    public bool ShowStreetDisplay { get; set; }
    public bool FadeStreetDisplay { get; set; }
    public bool FadeStreetDisplayDuringWantedAndInvestigation { get; set; }
    public uint StreetDisplayTimeToShow { get; set; }
    public uint StreetDisplayTimeToFade { get; set; }
    public float StreetPositionX { get; set; }
    public float StreetPositionY { get; set; }
    public float StreetScale { get; set; }
    public int StreetJustificationID { get; set; }
    public GTAFont StreetFont { get; set; }
    public bool ShowZoneDisplay { get; set; }
    public bool FadeZoneDisplay { get; set; }
    public bool FadeZoneDisplayDuringWantedAndInvestigation { get; set; }
    public uint ZoneDisplayTimeToShow { get; set; }
    public uint ZoneDisplayTimeToFade { get; set; }
    public float ZonePositionX { get; set; }
    public float ZonePositionY { get; set; }
    public float ZoneScale { get; set; }
    public int ZoneJustificationID { get; set; }
    public GTAFont ZoneFont { get; set; }
    public bool ZoneDisplayShowPrimaryAgency { get; set; }
    public bool ZoneDisplayShowSecondaryAgency { get; set; }
    public bool ZoneDisplayShowPrimaryGang { get; set; }
    public bool AlwaysShowCash { get; set; }
    public bool AlwaysShowRadar { get; set; }
    public bool AlwaysShowHUD { get; set; }
    public bool ShowDebug { get; set; }
    public bool ShowVanillaVehicleUI { get; set; }
    public bool ShowVanillaAreaUI { get; set; }
    public bool SetBustedEffect { get; set; }
    public bool PlayWastedSounds { get; set; }
    public bool DisplayBustedMessage { get; set; }
    public bool SetDeathEffect { get; set; }
    public bool DisplayWastedMessage { get; set; }
    public string WastedMessageText { get; set; }
    public string BustedMessageText { get; set; }
    public bool AllowScreenEffectReset { get; set; }
    public string SpeedDisplayUnits { get; set; }
    public bool DisplayButtonPrompts { get; set; }
    public bool SetRadarZoomDistance { get; set; }
    public float RadarZoomDistance_Wanted { get; set; }
    public float RadarZoomDistance_Investigation { get; set; }
    public float RadarZoomDistance_Default { get; set; }
    public bool GreyOutWhiteFontAtNight { get; set; }
    public bool ShowStaminaBar { get; set; }
    public float StaminaBarPositionX { get; set; }
    public float StaminaBarPositionY { get; set; }
    public float StaminaBarWidth { get; set; }
    public float StaminaBarHeight { get; set; }


    public bool ShowSelectorDisplay { get; set; }
    public float SelectorPositionX { get; set; }
    public float SelectorPositionY { get; set; }
    public float SelectorScale { get; set; }



    public UISettings()
    {
        SetDefault();
        #if DEBUG
            SetRadarZoomDistance = false;
            ShowDebug = false;
        #endif
    }
    public void SetDefault()
    {
        UIEnabled = true;
        ShowCrimesDisplay = false;
        CrimesViolatingPositionX = 0.65f;
        CrimesViolatingPositionY = 0.98f;
        CrimesViolatingScale = 0.4f;
        CrimesViolatingJustificationID = 2;
        CrimesViolatingFont = GTAFont.FontChaletComprimeCologne;
        ShowSpeedLimitDisplay = true;
        SpeedLimitPositionX = 0.78f;
        SpeedLimitPositionY = 0.98f;
        SpeedLimitScale = 0.2f;
        ShowVehicleStatusDisplay = true;
        FadeVehicleStatusDisplay = true;
        FadeVehicleStatusDisplayDuringWantedAndInvestigation = false;
        VehicleStatusTimeToShow = 5000;
        VehicleStatusTimeToFade = 1500;
        VehicleStatusPositionX = 0.81f;
        VehicleStatusPositionY = 0.98f;
        VehicleStatusScale = 0.52f;
        VehicleStatusJustificationID = 2;
        VehicleStatusFont = GTAFont.FontHouseScript;
        VehicleStatusIncludeTextSpeedLimit = false;
        ShowPlayerDisplay = true;
        FadePlayerDisplay = false;
        FadePlayerDisplayDuringWantedAndInvestigation = false;
        PlayerDisplayTimeToShow = 7500;
        PlayerDisplayTimeToFade = 1500;
        PlayerStatusPositionX = 0.84f;
        PlayerStatusPositionY = 0.98f;
        PlayerStatusScale = 0.52f;
        PlayerStatusJustificationID = 2;
        PlayerStatusFont = GTAFont.FontHouseScript;
        PlayerStatusIncludeTime = true;
        PlayerStatusSimpleTime = true;
        ShowStreetDisplay = true;
        FadeStreetDisplay = true;
        FadeStreetDisplayDuringWantedAndInvestigation = false;
        StreetDisplayTimeToShow = 7500;
        StreetDisplayTimeToFade = 1500;
        StreetPositionX = 0.87f;
        StreetPositionY = 0.98f;
        StreetScale = 0.52f;
        StreetJustificationID = 2;
        StreetFont = GTAFont.FontHouseScript;
        ShowZoneDisplay = true;
        FadeZoneDisplay = true;
        FadeZoneDisplayDuringWantedAndInvestigation = false;
        ZoneDisplayTimeToShow = 7500;
        ZoneDisplayTimeToFade = 1500;
        ZonePositionX = 0.90f;
        ZonePositionY = 0.98f;
        ZoneScale = 0.55f;
        ZoneJustificationID = 2;
        ZoneFont = GTAFont.FontHouseScript;
        ZoneDisplayShowPrimaryAgency = true;
        ZoneDisplayShowSecondaryAgency = false;
        ZoneDisplayShowPrimaryGang = true;
        AlwaysShowCash = true;
        AlwaysShowRadar = true;
        AlwaysShowHUD = true;
        ShowDebug = false;
        ShowVanillaVehicleUI = false;
        ShowVanillaAreaUI = false;
        SetBustedEffect = true;
        PlayWastedSounds = true;
        DisplayBustedMessage = true;
        SetDeathEffect = true;
        DisplayWastedMessage = true;
        WastedMessageText = "WASTED";
        BustedMessageText = "BUSTED";
        AllowScreenEffectReset = true;
        SpeedDisplayUnits = "MPH";
        DisplayButtonPrompts = true;
        SetRadarZoomDistance = false;
        RadarZoomDistance_Wanted = 175f;
        RadarZoomDistance_Investigation = 125f;
        RadarZoomDistance_Default = 75f;
        GreyOutWhiteFontAtNight = true;
        ShowStaminaBar = true;
        StaminaBarPositionX = 0.05f;
        StaminaBarPositionY = 0.9925f;
        StaminaBarWidth = 0.07f;
        StaminaBarHeight = 0.0075f;

        ShowSelectorDisplay = true;
        SelectorPositionX = 0.7f;
        SelectorPositionY = 0.98f;
        SelectorScale = 0.5f;
    }
}