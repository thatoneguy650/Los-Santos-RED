using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


public class UIGeneralSettings : ISettingsDefaultable
{
    public bool IsEnabled { get; set; }
    public bool AlwaysShowRadar { get; set; }
    public bool NeverShowRadar { get; set; }
    public bool ShowRadarInVehicleOnly { get; set; }
    public bool ShowRadarWhenCellPhoneActiveOnly { get; set; }
    public bool ShowRadarOnFootWhenCellPhoneActiveOnly { get; set; }
    public bool HideLSRUIUnlessActionWheelActive { get; set; }
    public bool HideRadarUnlessActionWheelActive { get; set; }
    public bool AlwaysShowCash { get; set; }
    public bool DisableVanillaCashDisplay { get; set; }
    public bool AlwaysShowHUD { get; set; }
    public bool ShowVanillaVehicleUI { get; set; }
    public bool ShowVanillaAreaUI { get; set; }
    public bool GreyOutWhiteFontAtNight { get; set; }
    public bool DisplayWastedMessage { get; set; }
    public string WastedMessageText { get; set; }
    public bool PlayWastedSounds { get; set; }
    public bool SetDeathEffect { get; set; }
    public bool DisplayBustedMessage { get; set; }
    public string BustedMessageText { get; set; }
    public string DetainedMessageText { get; set; }
    public bool SetBustedEffect { get; set; }
    public bool SetRadarZoomDistance { get; set; }
    public float RadarZoomDistance_Wanted { get; set; }
    public float RadarZoomDistance_Investigation { get; set; }
    public float RadarZoomDistance_Default { get; set; }
    public bool AllowScreenEffectReset { get; set; }
    public bool DisplayButtonPrompts { get; set; }
    public bool ShowDebug { get; set; }


    public bool ShowFullscreenWarnings { get; set; }
   // public bool ShowFakeWantedLevelStars { get; set; }


    public bool UseCustomWantedLevelStars { get; set; }
    public float CustomWantedLevelStarsScale { get; set; }  
    public float CustomWantedLevelStarsSpacingPixelReduction { get; set; }
    public int CustomWantedLevelStarsRedColorLimit { get; set; }

    public bool CustomWantedLevelStarsFlashWhenSearching { get; set; }
    public uint CustomWantedLevelStarsTimeBetweenFlash { get; set; }

    public bool UseCustomInvestigationMarks { get; set; }
    public float CustomInvestigationMarksScale { get; set; }
    public float CustomInvestigationMarksSpacingPixelReduction { get; set; }
    public float CustomGroupIconsSpacingPixelReduction { get; set; }
    public bool ShowVehicleInteractionPrompt { get; set; }
    public bool ShowVehicleInteractionPromptInVehicle { get; set; }



    public bool CreatePoliceResponseBlip { get; set; }
    public string DefaultTextColor { get; set; }
    public string DefaultTextColorNight { get; set; }

    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        SetDefault();
    }

    public UIGeneralSettings()
    {
        SetDefault();

    }
    public void SetDefault()
    {
        IsEnabled = true;
        AlwaysShowCash = false;
        DisableVanillaCashDisplay = true;
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
        DetainedMessageText = "DETAINED";
        AllowScreenEffectReset = true;

        DisplayButtonPrompts = true;
        SetRadarZoomDistance = false;
        RadarZoomDistance_Wanted = 175f;
        RadarZoomDistance_Investigation = 125f;
        RadarZoomDistance_Default = 75f;
        GreyOutWhiteFontAtNight = true;
        AlwaysShowRadar = true;
        NeverShowRadar = false;
        ShowRadarInVehicleOnly = false;
        ShowRadarWhenCellPhoneActiveOnly = false;
        ShowRadarOnFootWhenCellPhoneActiveOnly = false;
        HideLSRUIUnlessActionWheelActive = false;
        HideRadarUnlessActionWheelActive = false;



        ShowFullscreenWarnings = true;

        CustomWantedLevelStarsScale = 0.6f;
        CustomWantedLevelStarsSpacingPixelReduction = 10f;
        CustomInvestigationMarksSpacingPixelReduction = 0f;
        CustomInvestigationMarksScale = 0.6f;
#if DEBUG
        ShowDebug = true;

#endif

       // ShowFakeWantedLevelStars = false;
        UseCustomWantedLevelStars = true;
        UseCustomInvestigationMarks = true;


        CustomWantedLevelStarsRedColorLimit = 6;
        ShowVehicleInteractionPrompt = true;
        ShowVehicleInteractionPromptInVehicle = true;
        CustomWantedLevelStarsFlashWhenSearching = true;//HAS DESERIALIZED VALUES
        CustomWantedLevelStarsTimeBetweenFlash = 1000;//HAS DESERIALIZED VALUES
        CreatePoliceResponseBlip = true;

        DefaultTextColorNight = "~c~";
        DefaultTextColor = "~s~";
    }
}
