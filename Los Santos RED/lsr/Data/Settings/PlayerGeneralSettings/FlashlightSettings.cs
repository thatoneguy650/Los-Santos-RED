public class FlashlightSettings : ISettingsDefaultable
{




    public bool LightFollowsCamera { get; set; }
    public bool LightFollowsPropDuringSearch { get; set; }
    public bool AllowPropRotation { get; set; }
    public bool UseFakeEmissive { get; set; }
    public float PitchMax { get; set; }
    public float PitchMin { get; set; }
    public float HeadingMax { get; set; }
    public float HeadingMin { get; set; }
    //public float ExtraDistanceX { get; set; }
    //public float ExtraDistanceY { get; set; }







    public float EmissiveDistance { get; set; }
    public float EmissiveBrightness { get; set; }
    public float EmissiveHardness { get; set; }
    public float EmissiveRadius { get; set; }
    public float EmissiveFallOff { get; set; }




    //public float FakeEmissiveExtraDistanceX { get; set; }
    //public float FakeEmissiveExtraDistanceY { get; set; }
    //public float FakeEmissiveExtraDistanceZ { get; set; }

    public float FakeEmissiveDistance { get; set; }
    public float FakeEmissiveBrightness { get; set; }
    public float FakeEmissiveHardness { get; set; }
    public float FakeEmissiveRadius { get; set; }
    public float FakeEmissiveFallOff { get; set; }
    public bool ShowDebugMarkerAtEmissiveTip { get; set; }
 //   public string PropName { get; set; }


    public FlashlightSettings()
    {
        SetDefault();

    }
    public void SetDefault()
    {

        LightFollowsCamera = true;
        PitchMax = 25f;
        PitchMin = -25f;
        HeadingMax = 30f;
        HeadingMin = -30f;




        //ExtraDistanceX = 0.0f;
        //ExtraDistanceY = -0.05f;

        //FakeEmissiveExtraDistanceX = -0.07f;
        //FakeEmissiveExtraDistanceY = -0.2f;
        //FakeEmissiveExtraDistanceZ = 0.0f;



        UseFakeEmissive = true;

        AllowPropRotation = true;



        EmissiveDistance = 100.0f;
        EmissiveBrightness = 1.0f;
        EmissiveHardness = 0.0f;
        EmissiveRadius = 13.0f;
        EmissiveFallOff = 1.0f;



        FakeEmissiveDistance = 0.3f;
        FakeEmissiveBrightness = 1.0f;
        FakeEmissiveHardness = 0.0f;
        FakeEmissiveRadius = 100.0f;
        FakeEmissiveFallOff = 1.0f;

        ShowDebugMarkerAtEmissiveTip = false;

        LightFollowsPropDuringSearch = true;


//        PropName = "prop_cs_police_torch";

//#if DEBUG
//        PropName = "prop_tool_torch";
//#endif

    }
}