using System.ComponentModel;

public class ActionWheelSettings : ISettingsDefaultable
{
    public bool RequireButtonHold { get; set; }
    public bool SetPauseOnActivate { get; set; }
    public bool SetPauseOnActivateControllerOnly { get; set; }
    public bool SetSlowMoOnActivate { get; set; }
    public bool SetSlowMoOnActivateControllerOnly { get; set; }
    public float SlowMoScale { get; set; }


    //[Description("Seems to obliterate performance? Disabled for now.")]
    public bool SetTransitionEffectOnActivate { get; set; }
    public bool PlayTransitionSoundsOnActivate { get; set; }







    public float ItemWidth { get; set; }
    public float ItemHeight { get; set; }
    public float ItemSpacingX { get; set; }
    public float ItemSpacingY { get; set; }
    public float ItemScale { get; set; }
    public string ItemColor { get; set; }
    public string SelectedItemColor { get; set; }
    public float SelectedItemMinimumDistance { get; set; }
    public float ItemCenterX { get; set; }
    public float ItemCenterY { get; set; }
    public float ItemDistanceFromCenter { get; set; }
    public float TextScale { get; set; }
    public GTAFont TextFont { get; set; }
    public bool ShowCursor { get; set; }
    public string TextColor { get; set; }
    public string TransitionInEffect { get; set; }
    public string TransitionOutEffect { get; set; }
    public float MessageStartingPositionX { get; set; }
    public float MessageStartingPositionY { get; set; }
    public GTAFont MessageFont { get; set; }
    public string MessageTextColor { get; set; }
    public float MessageScale { get; set; }
    public float MessageBodySpacingY { get; set; }
    public float MessageHeaderSpacingY { get; set; }
    public int MessagesToShow { get; set; }
    public float ItemDistanceFromCenterExtraItemScalar { get; set; }
    public float ItemScaleExtraItemScalar { get; set; }
    public float TextBoxScale { get; set; }
    public float PrevPageCenterX { get; set; }
    public float PrevPageCenterY { get; set; }

    public float NextPageCenterX { get; set; }
    public float NextPageCenterY { get; set; }
    public int ItemsPerPage { get; set; }

    public bool ShowSpeedLimitIcon { get; set; }
    public float SpeedLimitIconScale { get; set; }
    public float SpeedLimitIconX { get; set; }
    public float SpeedLimitIconY { get; set; }





    public float MainMenuCenterX { get; set; }
    public float MainMenuCenterY { get; set; }

    public float DebugMenuCenterX { get; set; }
    public float DebugMenuCenterY { get; set; }

    public float BurnerPhoneOpenCenterX { get; set; }
    public float BurnerPhoneOpenCenterY { get; set; }


    public float ButtonPromptXStart { get; set; }
    public float ButtonPromptYStart { get; set; }

    public float AffiliationCenterX { get; set; }
    public float AffiliationCenterY { get; set; }



    public bool ShowIcons { get; set; }
    public float DebugIconX { get; set; }
    public float DebugIconY { get; set; }
    public float DebugIconScale { get; set; }
    public bool ShowOnlyIcon { get; set; }




    public float ControllerCursorScale { get; set; }
    public bool UseNewClosest { get; set; }


    public ActionWheelSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        ItemWidth = 0.06f;
        ItemHeight = 0.06f;
        ItemSpacingX = 0.04f;
        ItemSpacingY = 0.04f;
        ItemScale = 1.2f;
        TextScale = 0.35f;
        TextFont = GTAFont.FontChaletLondon;// GTAFont.FontMonospace;
        TextColor = "White";
        ItemColor = "Black";
        SelectedItemColor = "Red";
        ShowCursor = true;
        ItemCenterX = 0.5f;
        ItemCenterY = 0.5f;
        ItemDistanceFromCenter = 0.3f;
        TransitionInEffect = "SwitchHUDIn";
        TransitionOutEffect = "SwitchHUDOut";
        MessageStartingPositionX = 0.1f;// 0.05f;
        MessageStartingPositionY = 0.1f;// 0.05f;
        MessageFont = GTAFont.FontChaletLondon;//GTAFont.FontChaletComprimeCologne;
        MessageTextColor = "White";
        MessageScale = 0.25f;
        MessageBodySpacingY = 0.15f;//0.125f;
        MessageHeaderSpacingY = 0.04f;//0.02f;
        MessagesToShow = 5;
        ItemDistanceFromCenterExtraItemScalar = -0.005f;//-0.01f;
        ItemScaleExtraItemScalar = 0.005f;// 0.01f;

        TextBoxScale = 0.084f;//0.025f;

        NextPageCenterX = 0.875f;
        NextPageCenterY = 0.8f;

        PrevPageCenterX = 0.8f;
        PrevPageCenterY = 0.8f;

        ItemsPerPage = 10;

        ShowSpeedLimitIcon = true;
        SpeedLimitIconScale = 0.35f;




        SetSlowMoOnActivate = true;
        SetTransitionEffectOnActivate = true;
        ShowIcons = false;

        DebugIconX = 0.0f;
        DebugIconY = 0.0f;
        DebugIconScale = 1.0f;
        ShowOnlyIcon = false;

        ControllerCursorScale = 0.01f;

        RequireButtonHold = false;


        SpeedLimitIconY = 0.94f;
        SpeedLimitIconX = 0.375f;

        MainMenuCenterX = 0.92f;
        MainMenuCenterY = 0.45f;

        DebugMenuCenterX = 0.92f;
        DebugMenuCenterY = 0.5f;

        BurnerPhoneOpenCenterX = 0.92f;
        BurnerPhoneOpenCenterY = 0.55f;



        AffiliationCenterX = 0.92f;
        AffiliationCenterY = 0.65f;// 0.575f;

        ButtonPromptXStart = 0.92f;
        ButtonPromptYStart = 0.75f;//0.625f;




        SetPauseOnActivate = false;
        SetPauseOnActivateControllerOnly = false;
        SetSlowMoOnActivate = true;
        SetSlowMoOnActivateControllerOnly = false;
        PlayTransitionSoundsOnActivate = true;
        SlowMoScale = 0.2f;

        SelectedItemMinimumDistance = 0.15f;

        UseNewClosest = true;

    }
}