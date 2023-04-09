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
    public float EmissiveDistance { get; set; }
    public float EmissiveBrightness { get; set; }
    public float EmissiveHardness { get; set; }
    public float EmissiveRadius { get; set; }
    public float EmissiveFallOff { get; set; }
    public float FakeEmissiveDistance { get; set; }
    public float FakeEmissiveBrightness { get; set; }
    public float FakeEmissiveHardness { get; set; }
    public float FakeEmissiveRadius { get; set; }
    public float FakeEmissiveFallOff { get; set; }
    public bool ShowDebugMarkerAtEmissiveTip { get; set; }
    public float DebugExtraDistanceX { get; set; }
    public float DebugExtraDistanceY { get; set; }
    public float DebugExtraDistanceZ { get; set; }
    public float DebugFakeEmissiveExtraDistanceX { get; set; }
    public float DebugFakeEmissiveExtraDistanceY { get; set; }
    public float DebugFakeEmissiveExtraDistanceZ { get; set; }

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
        UseFakeEmissive = true;
        AllowPropRotation = true;
        EmissiveDistance = 100.0f;
        EmissiveBrightness = 1.0f;
        EmissiveHardness = 0.0f;
        EmissiveRadius = 13.0f;
        EmissiveFallOff = 1.0f;
        FakeEmissiveDistance = 0.4f;
        FakeEmissiveBrightness = 5.0f;
        FakeEmissiveHardness = 0.0f;
        FakeEmissiveRadius = 100.0f;
        FakeEmissiveFallOff = 1.0f;
        ShowDebugMarkerAtEmissiveTip = false;
        LightFollowsPropDuringSearch = true;
    }
}