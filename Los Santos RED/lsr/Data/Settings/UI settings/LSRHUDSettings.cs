using System.Runtime.Serialization;

public class LSRHUDSettings : ISettingsDefaultable
{

    public bool CrimesDisplayEnabled { get; set; }
    public float CrimesDisplayPositionX { get; set; }
    public float CrimesDisplayPositionY { get; set; }
    public float CrimesDisplayScale { get; set; }
    public int CrimesDisplayJustificationID { get; set; }
    public GTAFont CrimesDisplayFont { get; set; }


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

    public bool PlayerStatusShowWhenSleeping { get; set; }


    public bool PlayerStatusIncludePoliceCount { get; set; }

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

    public bool ZoneDisplayShowState { get; set; }
    public bool ZoneDisplayShowCounty { get; set; }
    public bool ZoneDisplayShowBorough { get; set; }


    public bool ZoneDisplayShowCountyShort { get; set; }

    public float TopDisplayPositionX { get; set; }
    public float TopDisplayPositionY { get; set; }
    public float TopDisplayScale { get; set; }
    public float TopDisplaySpacing { get; set; }
    public bool TopDisplayShowWeapon { get; set; }
    public bool TopDisplayWeaponSimpleSelector { get; set; }




    public float ExtraTopDisplayPositionX { get; set; }
    public float ExtraTopDisplayPositionY { get; set; }
    public float ExtraTopDisplayScale { get; set; }
    public float ExtraTopDisplaySpacing { get; set; }
    public float ExtraTopDisplayPositionXMediumOffset { get; set; }
    public int ExtraTopDisplayFont { get; set; }


    public float ExtraTopDisplayIconPositionX { get; set; }
    public float ExtraTopDisplayIconPositionY { get; set; }
    public float ExtraTopDisplayIconSpacing { get; set; }
    public float ExtraTopDisplayIconScale { get; set; }
    public float ExtraTopDisplayIconSpacingPixelReduction { get; set; }



    //public float LowerDisplayPositionX { get; set; }
    //public float LowerDisplayPositionY { get; set; }
    //public float LowerDisplayScale { get; set; }
    public float LowerDisplaySpacing { get; set; }

    public float LowerDisplayTimerBarSpacing { get; set; }
    public float LowerDisplayButtonPromptSpacing { get; set; }
    public float LowerDisplayNoItemSpacing { get; set; }
    public bool ShowStaminaDisplay { get; set; }
    public bool ShowIntoxicationDisplay { get; set; }
    public bool ShowSearchModeDisplay { get; set; }


    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {

    }

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
        PlayerStatusIncludeTime = false;
        PlayerStatusSimpleTime = true;
        PlayerStatusShowWhenSleeping = true;
        PlayerStatusIncludePoliceCount = false;
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
        TopDisplayShowWeapon = true;
        TopDisplayWeaponSimpleSelector = false;




        //Top Display
        TopDisplayPositionX = 0.0175f; 
        TopDisplayPositionY = 0.985f;
        TopDisplayScale = 0.65f;
        TopDisplaySpacing = 0.035f;

        

        //Extra
        ExtraTopDisplayPositionX = 0.0175f;
        ExtraTopDisplayPositionY = 0.985f;
        ExtraTopDisplayScale = 0.8f;
        ExtraTopDisplaySpacing = 0.12f;// 0.07f;
        ExtraTopDisplayPositionXMediumOffset = 0.2f;

        ExtraTopDisplayFont = 4;

        ExtraTopDisplayIconPositionX = 0.98f;// 0.985f;
        ExtraTopDisplayIconPositionY = 0.4f;// 0.52f;// 0.0175f;

        ExtraTopDisplayIconScale = 0.5f;
        ExtraTopDisplayIconSpacingPixelReduction = 10f;
        ExtraTopDisplayIconSpacing = 0.12f;


        //Lower Display
        //LowerDisplayPositionX = 0.93f;
        //LowerDisplayPositionY = 0.98f;
        //LowerDisplayScale = 0.52f;
        LowerDisplaySpacing = 0.035f;
        LowerDisplayTimerBarSpacing = 0.035f;
        LowerDisplayButtonPromptSpacing = 0.04f;
        LowerDisplayNoItemSpacing = 0.04f;


        ShowStaminaDisplay = true;
        ShowIntoxicationDisplay = true;
        ShowSearchModeDisplay = true;

        CrimesDisplayPositionX = 0.83f;
        VehicleDisplayPositionX = 0.86f;
        PlayerStatusPositionX = 0.89f;
        StreetPositionX = 0.92f;
        ZonePositionX = 0.95f;

        ZoneDisplayShowState = false;
        ZoneDisplayShowBorough = true;
        ZoneDisplayShowCounty = true;
    }
}