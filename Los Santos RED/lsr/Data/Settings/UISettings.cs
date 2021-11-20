using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UISettings
{
    public bool UIEnabled { get; set; } = true;
    public bool ShowCrimesDisplay { get; set; } = false;
    public float CrimesViolatingPositionX { get; set; } = 0.65f;
    public float CrimesViolatingPositionY { get; set; } = 0.98f;
    public float CrimesViolatingScale { get; set; } = 0.4f;
    public int CrimesViolatingJustificationID { get; set; } = 2;
    public GTAFont CrimesViolatingFont { get; set; } = GTAFont.FontChaletComprimeCologne;
    public bool ShowSpeedLimitDisplay { get; set; } = true;
    public float SpeedLimitPositionX { get; set; } = 0.78f;//0.7f;
    public float SpeedLimitPositionY { get; set; } = 0.98f;
    public float SpeedLimitScale { get; set; } = 0.5f;
    public bool ShowVehicleStatusDisplay { get; set; } = true;
    public bool FadeVehicleStatusDisplay { get; set; } = true;
    public bool FadeVehicleStatusDisplayDuringWantedAndInvestigation { get; set; } = false;
    public uint VehicleStatusTimeToShow { get; set; } = 5000;
    public uint VehicleStatusTimeToFade { get; set; } = 1500; 
    public float VehicleStatusPositionX { get; set; } = 0.81f;//0.7f;
    public float VehicleStatusPositionY { get; set; } = 0.98f;
    public float VehicleStatusScale { get; set; } = 0.52f;//0.5f;
    public int VehicleStatusJustificationID { get; set; } = 2;
    public GTAFont VehicleStatusFont { get; set; } = GTAFont.FontHouseScript;
    public bool ShowPlayerDisplay { get; set; } = true;
    public float PlayerStatusPositionX { get; set; } = 0.84f;//= 0.6f;
    public float PlayerStatusPositionY { get; set; } = 0.98f;
    public float PlayerStatusScale { get; set; } = 0.52f;//0.5f;
    public int PlayerStatusJustificationID { get; set; } = 2;
    public GTAFont PlayerStatusFont { get; set; } = GTAFont.FontHouseScript;
    public bool ShowStreetDisplay { get; set; } = true;
    public bool FadeStreetDisplay { get; set; } = true;
    public bool FadeStreetDisplayDuringWantedAndInvestigation { get; set; } = false;
    public uint StreetDisplayTimeToShow { get; set; } = 7500;
    public uint StreetDisplayTimeToFade { get; set; } = 1500;
    public float StreetPositionX { get; set; } = 0.87f;
    public float StreetPositionY { get; set; } = 0.98f;
    public float StreetScale { get; set; } = 0.52f;//0.5f;
    public int StreetJustificationID { get; set; } = 2;
    public GTAFont StreetFont { get; set; } = GTAFont.FontHouseScript;
    public bool ShowZoneDisplay { get; set; } = true;
    public bool FadeZoneDisplay { get; set; } = true;
    public bool FadeZoneDisplayDuringWantedAndInvestigation { get; set; } = false;
    public uint ZoneDisplayTimeToShow { get; set; } = 7500;
    public uint ZoneDisplayTimeToFade { get; set; } = 1500;
    public float ZonePositionX { get; set; } = 0.90f;
    public float ZonePositionY { get; set; } = 0.98f;
    public float ZoneScale { get; set; } = 0.55f;//0.5f;
    public int ZoneJustificationID { get; set; } = 2;
    public GTAFont ZoneFont { get; set; } = GTAFont.FontHouseScript;
    public bool ZoneDisplayShowPrimaryAgency { get; set; } = true;
    public bool ZoneDisplayShowSecondaryAgency { get; set; } = true;
    public bool AlwaysShowCash { get; set; } = true;
    public bool AlwaysShowRadar { get; set; } = true;
    public bool AlwaysShowHUD { get; set; } = true;

    #if DEBUG
        public bool ShowDebug { get; set; } = true;
    #else
        public bool ShowDebug { get; set; } = false;
    #endif
    public bool ShowVanillaVehicleUI { get; set; } = false;
    public bool ShowVanillaAreaUI { get; set; } = false;





    public bool SetBustedEffect { get; set; } = true;
    public bool PlayWastedSounds { get; set; } = true;
    public bool DisplayBustedMessage { get; set; } = true;
    public bool SetDeathEffect { get; set; } = true;
    public bool DisplayWastedMessage { get; set; } = true;
    public string WastedMessageText { get; set; } = "WASTED";
    public string BustedMessageText { get; set; } = "BUSTED";
    public bool AllowScreenEffectReset { get; set; } = true;
    public string SpeedDisplayUnits { get; set; } = "MPH";
    public bool DisplayButtonPrompts { get; set; } = true;



    

    public UISettings()
    {

    }
}