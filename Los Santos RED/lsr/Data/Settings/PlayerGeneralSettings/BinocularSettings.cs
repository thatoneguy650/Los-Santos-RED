using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BinocularSettings : ISettingsDefaultable
{
    [Description("")]
    public float ExtraDistanceX { get; set; }
    public float ExtraDistanceY { get; set; }
    public float ExtraDistanceZ { get; set; }
    public bool DrawMarkers { get; set; }
    public float FOV { get; set; }
    public float MotionBlur { get; set; }
    public float NearDOF { get; set; }
    public float FarDOF { get; set; }
    public float DOFStrength { get; set; }
    public bool DrawScaleform { get; set; }
    public BinocularSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {    
        DrawMarkers = false;
        DrawScaleform = true;
    }
}