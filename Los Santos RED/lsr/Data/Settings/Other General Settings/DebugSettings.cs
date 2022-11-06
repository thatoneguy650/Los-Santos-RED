using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DebugSettings : ISettingsDefaultable
{
    public bool ShowPoliceTaskArrows { get; set; }
    public bool ShowCivilianTaskArrows { get; set; }
    public bool ShowCivilianPerceptionArrows { get; set; }
    public bool ShowTrafficArrows { get; set; }


    public float ShovelAnimationStopTime { get; set; }
    public bool ShovelFadeOut { get; set; }
    public bool ShovelUseAltCamera { get; set; }
    public float ShovelHoleOffsetX { get; set; }
    public float ShovelHoleOffsetY { get; set; }
    public float ShovelStartOffsetX { get; set; }
    public float ShovelStartOffsetY { get; set; }
    public float ShovelCameraOffsetX { get; set; }
    public float ShovelCameraOffsetY { get; set; }
    public float ShovelCameraOffsetZ { get; set; }
    public bool ShovelDebugDrawMarkers { get; set; }





    public float BinocDebugExtraDistanceX { get; set; }
    public float BinocDebugExtraDistanceY { get; set; }
    public float BinocDebugExtraDistanceZ { get; set; }
    public bool BinocDebugDrawMarkers { get; set; }
    public float BinocFOV { get; set; }
    public float BinocMotionBlur { get; set; }
    public float BinocNearDOF { get; set; }
    public float BinocFarDOF { get; set; }
    public float BinocDOFStrength { get; set; }
    public bool BinocDrawScaleform { get; set; }


    public DebugSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        ShowPoliceTaskArrows = false;
        ShowCivilianTaskArrows = false;
        ShowCivilianPerceptionArrows = false;
        ShowTrafficArrows = false;


        ShovelAnimationStopTime = 0.7f;
        ShovelFadeOut = true;
        ShovelUseAltCamera = true;


        ShovelHoleOffsetX = -1.0f;
        ShovelHoleOffsetY = 0.0f;


        ShovelStartOffsetX = 0.0f;
        ShovelStartOffsetY = 1.0f;

        ShovelCameraOffsetX = 2.0f;
        ShovelCameraOffsetY = 3.5f;
        ShovelCameraOffsetZ = 0.5f;




        ShovelDebugDrawMarkers = false;
        BinocDebugDrawMarkers = false;

        BinocDrawScaleform = true;

    }

}