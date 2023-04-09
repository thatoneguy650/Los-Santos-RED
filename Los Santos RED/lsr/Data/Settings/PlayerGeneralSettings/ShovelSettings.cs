using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ShovelSettings : ISettingsDefaultable
{
    [Description("")]
    public float AnimationStopTime { get; set; }
    public bool FadeOut { get; set; }
    public bool UseAltCamera { get; set; }
    public float HoleOffsetX { get; set; }
    public float HoleOffsetY { get; set; }
    public float StartOffsetX { get; set; }
    public float StartOffsetY { get; set; }
    public float CameraOffsetX { get; set; }
    public float CameraOffsetY { get; set; }
    public float CameraOffsetZ { get; set; }
    public bool DrawMarkers { get; set; }
    public ShovelSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        AnimationStopTime = 0.7f;
        FadeOut = true;
        UseAltCamera = true;
        HoleOffsetX = -1.0f;
        HoleOffsetY = 0.0f;
        StartOffsetX = 0.0f;
        StartOffsetY = 1.0f;
        CameraOffsetX = 2.0f;
        CameraOffsetY = 3.5f;
        CameraOffsetZ = 0.5f;
        DrawMarkers = false;
    }
}