using System.ComponentModel;

public class DragSettings : ISettingsDefaultable
{
    [Description("")]
    public bool AllowLoadingBodies { get; set; }
    public float LoadBodyXOffset { get; set; }
    public float LoadBodyYOffset { get; set; }
    public float LoadBodyZOffset { get; set; }
    public float LoadBodyXOffsetBed { get; set; }
    public float LoadBodyYOffsetBed { get; set; }
    public float LoadBodyZOffsetBed { get; set; }
    public float LoadBodyXRotation { get; set; }
    public float LoadBodyYRotation { get; set; }
    public float LoadBodyZRotation { get; set; }
    public int BoneIndex { get; set; }
    public bool UseBasicAttachIfPed { get; set; }
    public int Euler { get; set; }
    public bool OffsetIsRelative { get; set; }
    public bool FadeOut { get; set; }
    public bool RagdollSetPedsInvisible { get; set; }
    public bool RagdollSetNoCollision { get; set; }
    public string RagdollItemAttachBone { get; set; }
    public string RagdollPhysicalAttachBone1 { get; set; }
    public float RagdollAttach1X { get; set; }
    public float RagdollAttach1Y { get; set; }
    public float RagdollAttach1Z { get; set; }
    public float RagdollAttach2X { get; set; }
    public float RagdollAttach2Y { get; set; }
    public float RagdollAttach2Z { get; set; }
    public float RagdollAttach3X { get; set; }
    public float RagdollAttach3Y { get; set; }
    public float RagdollAttach3Z { get; set; }
    public bool RagdollFixedRotation { get; set; }
    public bool RagdollDoInitialWarp { get; set; }
    public bool RagdollCollision { get; set; }
    public bool RagdollTeleport { get; set; }
    public int RagdollRotationOrder { get; set; }
    public bool RagdollRunAttach { get; set; }
    public bool AllowRagdolling { get; set; }

    public DragSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        AllowLoadingBodies = true;
        RagdollAttach1X = 0.0f;// 0.1f;
        RagdollAttach1Y = 0.0f;//0.3f;
        RagdollAttach1Z = 0.0f;//-0.1f;
        RagdollAttach2X = 0.0f;
        RagdollAttach2Y = 0.0f;
        RagdollAttach2Z = 0.0f;
        RagdollAttach3X = 0.0f;//180f;
        RagdollAttach3Y = 0.0f;//90f;
        RagdollAttach3Z = 0.0f;//0f;
        RagdollSetPedsInvisible = false;
        RagdollSetNoCollision = true;
        RagdollItemAttachBone = "BONETAG_R_PH_HAND"; //"BONETAG_R_PH_HAND";// "BONETAG_PELVIS";
        RagdollPhysicalAttachBone1 = "BONETAG_NECK"; //"BONETAG_R_CLAVICLE";// "BONETAG_SPINE3";
        RagdollRunAttach = true;
        RagdollFixedRotation = false;
        RagdollDoInitialWarp = true;
        RagdollCollision = true;
        RagdollTeleport = true;
        RagdollRotationOrder = 2;
        UseBasicAttachIfPed = false;
        Euler = 2;
        OffsetIsRelative = false;
        BoneIndex = 0;
        LoadBodyZOffsetBed = 1.5f;// -0.2f;
        LoadBodyZOffset = -0.1f;
        LoadBodyZRotation = 180f;
        LoadBodyYOffsetBed = -0.5f;


        AllowRagdolling = false;
        FadeOut = true;

#if DEBUG
        AllowRagdolling = true;
#endif

    }
}