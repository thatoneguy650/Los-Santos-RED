using Rage;
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



    public float DragAttach1X { get; set; }
    public float DragAttach1Y { get; set; }
    public float DragAttach1Z { get; set; }


    public float DragAttach2X { get; set; }
    public float DragAttach2Y { get; set; }
    public float DragAttach2Z { get; set; }

    public float DragAttach3X { get; set; }
    public float DragAttach3Y { get; set; }
    public float DragAttach3Z { get; set; }

    //public float DragAttach4X { get; set; }
    //public float DragAttach4Y { get; set; }
    //public float DragAttach4Z { get; set; }

    public bool DragFixedRotation { get; set; }
    public float PlateTheftFloat { get; set; }
    public uint DrinkTimeBetween { get; set; }
    public bool DrinkStartsBase { get; set; }
    public float TrunkXOffset { get; set; }
    public float TrunkYOffset { get; set; }
    public float TrunkZOffset { get; set; }

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




        DragAttach1X = 0.1f;
        DragAttach1Y = 0.3f;
        DragAttach1Z = -0.1f;


        DragAttach2X = 0.0f;
        DragAttach2Y = 0.0f;
        DragAttach2Z = 0.0f;

        DragAttach3X = 180f;
        DragAttach3Y = 90f;
        DragAttach3Z = 0f;

        DragFixedRotation = true;
        PlateTheftFloat = 1.0f;
        DrinkTimeBetween = 0;
        DrinkStartsBase = false;
        TrunkXOffset = 0.0f;
        TrunkYOffset = -1.0f;// -2.2f;
        TrunkZOffset = 0.5f;
}

}