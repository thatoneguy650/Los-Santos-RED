using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class LSRHUDSettings : ISettingsDefaultable
{
    public bool CrimesDisplayEnabled { get; set; }
    public float CrimesDisplayPositionX { get; set; }
    public float CrimesDisplayPositionY { get; set; }
    public float CrimesDisplayScale { get; set; }
    public int CrimesDisplayJustificationID { get; set; }
    public GTAFont CrimesDisplayFont { get; set; }

    public bool ShowSpeedLimitDisplay { get; set; }
    public float SpeedLimitPositionX { get; set; }
    public float SpeedLimitPositionY { get; set; }
    public float SpeedLimitScale { get; set; }
    public string SpeedDisplayUnits { get; set; }




    public bool VehicleDisplayEnabled { get; set; }
    public bool FadeVehicleDisplay { get; set; }
    public bool FadeVehicleDisplayDuringWantedAndInvestigation { get; set; }
    public uint VehicleDisplayTimeToShow { get; set; }
    public uint VehicleDisplayTimeToFade { get; set; }
    public float VehicleDisplayPositionX { get; set; }
    public float VehicleDisplayPositionY { get; set; }
    public float VehicleDisplayScale { get; set; }
    public int VehicleDisplayJustificationID { get; set; }
    public GTAFont VehicleDisplayFont { get; set; }
    public bool VehicleDisplayIncludeTextSpeedLimit { get; set; }
    public bool VehicleDisplayIncludeCurrentSpeed { get; set; }
    public bool VehicleDisplayIncludeCompass { get; set; }




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






    public bool ShowWeaponDisplay { get; set; }
    public bool FadeWeaponDisplay { get; set; }
    public float WeaponDisplayPositionX { get; set; }
    public float WeaponDisplayPositionY { get; set; }
    public float WeaponDisplayScale { get; set; }
    public GTAFont WeaponDisplayFont { get; set; }
    public int WeaponDisplayJustificationID { get; set; }
    public bool FadeWeaponDisplayDuringWantedAndInvestigation { get; set; }
    public uint WeaponDisplayTimeToShow { get; set; }
    public uint WeaponDisplayTimeToFade { get; set; }
    public bool WeaponDisplaySimpleSelector { get; set; }
 
    public LSRHUDSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        CrimesDisplayEnabled = false;
        CrimesDisplayPositionX = 0.65f;
        CrimesDisplayPositionY = 0.98f;
        CrimesDisplayScale = 0.4f;
        CrimesDisplayJustificationID = 2;
        CrimesDisplayFont = GTAFont.FontChaletComprimeCologne;
        ShowSpeedLimitDisplay = true;
        SpeedLimitPositionX = 0.75f;//0.78f
        SpeedLimitPositionY = 0.98f;
        SpeedLimitScale = 0.2f;
        SpeedDisplayUnits = "MPH";
        VehicleDisplayEnabled = true;
        FadeVehicleDisplay = true;
        FadeVehicleDisplayDuringWantedAndInvestigation = false;
        VehicleDisplayTimeToShow = 5000;
        VehicleDisplayTimeToFade = 1500;
        VehicleDisplayPositionX = 0.81f;
        VehicleDisplayPositionY = 0.98f;
        VehicleDisplayScale = 0.52f;
        VehicleDisplayJustificationID = 2;
        VehicleDisplayFont = GTAFont.FontHouseScript;
        VehicleDisplayIncludeTextSpeedLimit = true;
        VehicleDisplayIncludeCurrentSpeed = true;
        VehicleDisplayIncludeCompass = true;
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
        ShowWeaponDisplay = true;
        FadeWeaponDisplay = true;
        FadeWeaponDisplayDuringWantedAndInvestigation = false;
        WeaponDisplayTimeToShow = 2500;
        WeaponDisplayTimeToFade = 2500;
        WeaponDisplayPositionX = 0.78f;
        WeaponDisplayPositionY = 0.98f;
        WeaponDisplayScale = 0.52f;
        WeaponDisplayJustificationID = 2;
        WeaponDisplayFont = GTAFont.FontHouseScript;
        WeaponDisplaySimpleSelector = false;
    }
}