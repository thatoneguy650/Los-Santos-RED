using System.Runtime.Serialization;
using System.Security.Policy;

public class DebugSettings : ISettingsDefaultable
{
    public int SpawnCarsTestLimit { get; set; }
    public bool ShowPoliceTaskArrows { get; set; }
    public bool ShowCivilianTaskArrows { get; set; }
    public bool ShowCivilianPerceptionArrows { get; set; }
    public bool ShowTrafficArrows { get; set; }
    //New Drag
    public bool UseNewDrag { get; set; }
    public bool DoBothAttachments { get; set; }
    public bool DoPhysicalAttachment { get; set; }
    public bool DraggingDoAttach { get; set; }
    public bool DraggingPlayPedAnimation { get; set; }
    public bool DraggingResurrectPed { get; set; }
    public string PedAttachBoneName { get; set; }
    public string PedAttachBoneName2 { get; set; }
    public string PlayerAttachBoneName { get; set; }
    public string PlayerAttachBoneName2 { get; set; }
    public float RagdollAttach1X { get; set; }
    public float RagdollAttach1Y { get; set; }
    public float RagdollAttach1Z { get; set; }
    public float RagdollAttach2X { get; set; }
    public float RagdollAttach2Y { get; set; }
    public float RagdollAttach2Z { get; set; }
    public float RagdollAttach3X { get; set; }
    public float RagdollAttach3Y { get; set; }
    public float RagdollAttach3Z { get; set; }
    public float RagdollAttach1X2 { get; set; }
    public float RagdollAttach1Y2 { get; set; }
    public float RagdollAttach1Z2 { get; set; }
    public float RagdollAttach2X2 { get; set; }
    public float RagdollAttach2Y2 { get; set; }
    public float RagdollAttach2Z2 { get; set; }
    public float RagdollAttach3X2 { get; set; }
    public float RagdollAttach3Y2 { get; set; }
    public float RagdollAttach3Z2 { get; set; }
    public bool RagdollFixedRotation { get; set; }
    public bool RagdollDoInitialWarp { get; set; }
    public bool RagdollCollision { get; set; }
    public bool RagdollTeleport { get; set; }
    public int RagdollRotationOrder { get; set; }


    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        SetDefault();
    }

    public DebugSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        SpawnCarsTestLimit = 90;
        ShowPoliceTaskArrows = false;
        ShowCivilianTaskArrows = false;
        ShowCivilianPerceptionArrows = false;
        ShowTrafficArrows = false;


        UseNewDrag = false;
        DoBothAttachments = false;
        DoPhysicalAttachment = true;


        DraggingDoAttach = false;
        DraggingPlayPedAnimation = false;
        DraggingResurrectPed = false;



        PedAttachBoneName = "BONETAG_L_CLAVICLE";
        PedAttachBoneName2 = "BONETAG_R_CLAVICLE";
        PlayerAttachBoneName = "BONETAG_L_HAND";

        PlayerAttachBoneName2 = "BONETAG_R_HAND";


        RagdollAttach1X = 0.0f;// 0.1f;
        RagdollAttach1Y = 0.4f;//0.3f;
        RagdollAttach1Z = -0.4f;//-0.1f;
        RagdollAttach2X = 0.0f;
        RagdollAttach2Y = 0.0f;
        RagdollAttach2Z = 0.0f;
        RagdollAttach3X = 0.0f;//180f;
        RagdollAttach3Y = 0.0f;//90f;
        RagdollAttach3Z = 0.0f;//0f;



        RagdollAttach1X2 = 0.0f;// 0.1f;
        RagdollAttach1Y2 = -0.4f;//0.3f;
        RagdollAttach1Z2 = -0.4f;//-0.1f;
        RagdollAttach2X2 = 0.0f;
        RagdollAttach2Y2 = 0.0f;
        RagdollAttach2Z2 = 0.0f;
        RagdollAttach3X2 = 0.0f;//180f;
        RagdollAttach3Y2 = 0.0f;//90f;
        RagdollAttach3Z2 = 0.0f;//0f;


        //seems the hands move too much during the anim of them walking, not sure of the attachments

        RagdollFixedRotation = true;
        RagdollDoInitialWarp = true;
        RagdollCollision = true;
        RagdollTeleport = true;
        RagdollRotationOrder = 1;




    }
}