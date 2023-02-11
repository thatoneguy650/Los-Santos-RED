using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class UIGeneralSettings : ISettingsDefaultable
{
    public bool IsEnabled { get; set; }
    public bool AlwaysShowRadar { get; set; }
    public bool NeverShowRadar { get; set; }
    public bool ShowRadarInVehicleOnly { get; set; }
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
    public bool UseLegacySaveMenu { get; set; }
    public bool ShowLegacyActionMenu { get; set; }
    public bool ShowLegacyInventoryMenu { get; set; }
    public bool ShowFullscreenWarnings { get; set; }
    public bool ShowFakeWantedLevelStars { get; set; }


    public bool UseCustomWantedLevelStars { get; set; }
    public float CustomWantedLevelStarsScale { get; set; }  
    public float CustomWantedLevelStarsSpacingPixelReduction { get; set; }
    public int CustomWantedLevelStarsRedColorLimit { get; set; }

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
        ShowRadarOnFootWhenCellPhoneActiveOnly = false;
        HideLSRUIUnlessActionWheelActive = false;
        HideRadarUnlessActionWheelActive = false;

        UseLegacySaveMenu = false;
        ShowLegacyActionMenu = false;
        ShowLegacyInventoryMenu = false;

        ShowFullscreenWarnings = true;
        ShowFakeWantedLevelStars = true;

        CustomWantedLevelStarsScale = 0.6f;
        UseCustomWantedLevelStars = false;
        CustomWantedLevelStarsSpacingPixelReduction = 10f;
#if DEBUG
        ShowDebug = true;
        ShowFakeWantedLevelStars = false;
        UseCustomWantedLevelStars = true;
#endif
        CustomWantedLevelStarsRedColorLimit = 6;


    }
}