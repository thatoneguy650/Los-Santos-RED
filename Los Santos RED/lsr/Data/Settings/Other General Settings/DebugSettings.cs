using NAudio.Wave;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DebugSettings : ISettingsDefaultable
{
    //public float Draw_LoadBodyZInitialOffset { get; set; }


    public float Draw_LoadBodyXOffset { get; set; }
    public float Draw_LoadBodyYOffset { get; set; }
    public float Draw_LoadBodyZOffset { get; set; }


    public float Draw_LoadBodyXRotation { get; set; }
    public float Draw_LoadBodyYRotation { get; set; }
    public float Draw_LoadBodyZRotation { get; set; }

    public int Draw_BoneIndex { get; set; }

    public bool Drag_UseBasicAttachIfPed { get; set; }
    public int Drag_Euler { get; set; }
    public bool Drag_OffsetIsRelative { get; set; }


    public bool Drag_FadeOut { get; set; }
    public bool Drag_AutoCloseTrunk { get; set; }



    public bool Drag_SetPedsInvisible { get; set; }
    public bool Drag_SetNoCollision { get; set; }


    public string Drag_ItemAttachBone { get; set; }
    public string Drag_PhysicalAttachBone1 { get; set; }
    public string Drag_PhysicalAttachBone2 { get; set; }

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
    public bool Drag_DoInitialWarp { get; set; }
    public bool Drag_Collision { get; set; }
    public bool Drag_Teleport { get; set; }
    public int Drag_RotationOrder { get; set; }

    public int Drag_RagdollType { get; set; }

    public bool Drag_SetNM { get; set; }
    public bool Drag_RunAttach { get; set; }
    public bool Draw_DoInitialRagdoll { get; set; }

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




        DragAttach1X = 0.0f;// 0.1f;
        DragAttach1Y = 0.0f;//0.3f;
        DragAttach1Z = 0.0f;//-0.1f;


        DragAttach2X = 0.0f;
        DragAttach2Y = 0.0f;
        DragAttach2Z = 0.0f;

        DragAttach3X = 0.0f;//180f;
        DragAttach3Y = 0.0f;//90f;
        DragAttach3Z = 0.0f;//0f;

        
        PlateTheftFloat = 1.0f;
        DrinkTimeBetween = 0;
        DrinkStartsBase = false;
        TrunkXOffset = 0.0f;
        TrunkYOffset = -1.0f;// -2.2f;
        TrunkZOffset = 0.5f;
        Drag_SetPedsInvisible = false;
        Drag_SetNoCollision = true;
        Drag_ItemAttachBone = "BONETAG_R_PH_HAND"; //"BONETAG_R_PH_HAND";// "BONETAG_PELVIS";
        Drag_PhysicalAttachBone1 = "BONETAG_NECK"; //"BONETAG_R_CLAVICLE";// "BONETAG_SPINE3";
        Drag_PhysicalAttachBone2 = "BONETAG_PELVIS"; //"BONETAG_R_CLAVICLE";// "BONETAG_SPINE3";
        Drag_RagdollType = 0;
        Drag_SetNM = false;
        Drag_RunAttach = true;
        Draw_DoInitialRagdoll = true;

        DragFixedRotation = false;
        Drag_DoInitialWarp = true;
        Drag_Collision = true;
        Drag_Teleport = true;
        Drag_RotationOrder = 2;


        Drag_UseBasicAttachIfPed = false;
        Drag_Euler = 2;
        Drag_OffsetIsRelative = false;
        Draw_BoneIndex = 0;

        Draw_LoadBodyZOffset = -0.1f;
        Draw_LoadBodyZRotation = 180f;
        Drag_FadeOut = true;
        Drag_AutoCloseTrunk = false;
    }

}