using System.Runtime.Serialization;
using System.Security.Policy;

public class DebugSettings : ISettingsDefaultable
{
    public bool ShowPoliceTaskArrows { get; set; }
    public bool ShowCivilianTaskArrows { get; set; }
    public bool ShowCivilianPerceptionArrows { get; set; }
    public bool ShowTrafficArrows { get; set; }
    public float CanineRunSpeed { get;  set; }
    public bool SetupCopFully { get; set; }
    //public float BongAnimStart { get; set; }
    //public float BongAnimEnd { get; set; }
    //public float BongAnimBlend { get; set; }
    //public float EquipmentAnimEnd { get; set; }
    //public float EquipmentAnimStart { get; set; }



    public int StreetDisplayStyleIndex { get; set; }
    public int StreetDisplayColorIndex { get; set; }
    public int StreetDisplayFontIndex { get; set; }



    public float StreetDisplayOffsetX { get; set; }
    public float StreetDisplayOffsetY { get; set; }
    public float StreetDisplayOffsetZ { get; set; }


    public float StreetDisplayRotationX { get; set; }
    public float StreetDisplayRotationY { get; set; }
    public float StreetDisplayRotationZ { get; set; }

    public float StreetDisplayScaleX { get; set; }
    public float StreetDisplayScaleY { get; set; }
    public float StreetDisplayScaleZ { get; set; }
    public bool StreetDisplayUseCalc { get; set; }
    public float StreetDisplayNodeOffsetFront { get; set; }
    public uint StreetDisplayTimeBetweenUpdate { get; set; }
    public int StreetDisplayNodesToGet { get; set; }
    public float StreetDisplayMinNodeDistance { get; set; }
    public float StreetDisplayMaxNodeDistance { get; set; }




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
    public float PlayerItemAttachX { get; set; }
    public float PlayerItemAttachY { get; set; }
    public float PlayerItemAttachZ { get; set; }



    public float PlayerItemAttach2X { get; set; }
    public float PlayerItemAttach2Y { get; set; }
    public float PlayerItemAttach2Z { get; set; }




    public bool DoBothAttachments { get; set; }
    public bool DoPhysicalAttachment { get; set; }
    public int HeliMission { get; set; }
    public bool UseNewDrag { get; set; }



    public float SynchedSceneOffsetX { get; set; }
    public float SynchedSceneOffsetY { get; set; }
    public float SynchedSceneOffsetZ { get; set; }
    public float ObscuredX { get; set; }
    public float ObscuredY { get; set; }
    public float ObscuredZ { get; set; }
    public float SlowMoScaleTime { get; set; }



    public float CashAnimationMin1 { get; set; }
    public float CashAnimationMax1 { get; set; }
    public float CashAnimationMin2 { get; set; }
    public float CashAnimationMax2 { get; set; }



    public float TunnelZValueMax { get; set; }
    public bool AlwaysAllowTunnelSpawns { get; set; }
    //public float BlendInIdleDrink { get;  set; }
    //public float BlendOutIdleDrink { get;  set; }
    //public float BlendInBaseDrink { get;  set; }
    //public float BlendOutBaseDrink { get;  set; }
    //public float DrinkAnimBaseEndingPercentage { get; set; }




    //public float BlendInIdleEat { get; set; }
    //public float BlendOutIdleEat { get; set; }
    //public float BlendInBaseEat { get; set; }
    //public float BlendOutBaseEat { get; set; }
    //public float EatAnimBaseEndingPercentage { get; set; }


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
        ShowPoliceTaskArrows = false;
        ShowCivilianTaskArrows = false;
        ShowCivilianPerceptionArrows = false;
        ShowTrafficArrows = false;
        CanineRunSpeed = 10.0f;
        SetupCopFully = true;
        //BongAnimStart = 0.15f;
        //BongAnimEnd = 0.5f;
        //BongAnimStart = 0.3f;
        //BongAnimEnd = 0.55f;
        //BongAnimBlend = 2.0f;
        //EquipmentAnimStart = 0.25f;
        //EquipmentAnimEnd = 0.75f;
        StreetDisplayStyleIndex = 0;
        StreetDisplayColorIndex = 2;
        StreetDisplayFontIndex = 5;

        StreetDisplayOffsetZ = 2f;

        StreetDisplayScaleX = 5.0f;
        StreetDisplayScaleY = 1.0f;
        StreetDisplayScaleZ = 1.0f;
        StreetDisplayUseCalc = true;
        StreetDisplayNodeOffsetFront = 40f;
        StreetDisplayTimeBetweenUpdate = 500;
        StreetDisplayNodesToGet = 50;
        StreetDisplayMinNodeDistance = 10f;
        StreetDisplayMaxNodeDistance = 40f;

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


        DoBothAttachments = false;
        DoPhysicalAttachment = true;
        //more and more kidna working
        //PedAttachBoneName = "bonetag_spine3";
        //PlayerAttachBoneName = "bonetag_root";

        //RagdollAttach1X = 0.0f;// 0.1f;
        //RagdollAttach1Y = 0.0f;//0.3f;
        //RagdollAttach1Z = 0.0f;//-0.1f;
        //RagdollAttach2X = 0.0f;
        //RagdollAttach2Y = 0.0f;
        //RagdollAttach2Z = 0.0f;
        //RagdollAttach3X = 0.0f;//180f;
        //RagdollAttach3Y = 0.0f;//90f;
        //RagdollAttach3Z = 0.0f;//0f;
        //RagdollFixedRotation = true;
        //RagdollDoInitialWarp = true;
        //RagdollCollision = true;
        //RagdollTeleport = true;
        //RagdollRotationOrder = 1;
        //PlayerItemAttachX = 0.0f;
        //PlayerItemAttachY = 0.5f;
        //PlayerItemAttachZ = -0.2f;


        //more kinda working
        //PedAttachBoneName = "SKEL_Head";
        //PlayerAttachBoneName = "SKEL_Head";

        //RagdollAttach1X = 0.0f;// 0.1f;
        //RagdollAttach1Y = 0.6f;//0.3f;
        //RagdollAttach1Z = -0.5f;//-0.1f;
        //RagdollAttach2X = 0.0f;
        //RagdollAttach2Y = 0.0f;
        //RagdollAttach2Z = 0.0f;
        //RagdollAttach3X = 0.0f;//180f;
        //RagdollAttach3Y = 0.0f;//90f;
        //RagdollAttach3Z = 0.0f;//0f;
        //RagdollFixedRotation = true   ;
        //RagdollDoInitialWarp = true;
        //RagdollCollision = true   ;
        //RagdollTeleport = true;
        //RagdollRotationOrder = 1;


        //kinda working
        //PedAttachBoneName = "SKEL_L_Forearm";
        //PlayerAttachBoneName = "PH_L_Hand";

        //RagdollAttach1X = 0.0f;// 0.1f;
        //RagdollAttach1Y = 0.4f;//0.3f;
        //RagdollAttach1Z = -0.4f;//-0.1f;
        //RagdollAttach2X = 0.0f;
        //RagdollAttach2Y = 0.0f;
        //RagdollAttach2Z = 0.0f;
        //RagdollAttach3X = 0.0f;//180f;
        //RagdollAttach3Y = 0.0f;//90f;
        //RagdollAttach3Z = 0.0f;//0f;
        //RagdollFixedRotation = false;
        //RagdollDoInitialWarp = true;
        //RagdollCollision = false;
        //RagdollTeleport = false;
        //RagdollRotationOrder = 1;

        HeliMission = 6;


        UseNewDrag = false;

        ObscuredX = 0.5f;
        ObscuredY = 1.0f;
        ObscuredZ = 1.0f;

        SlowMoScaleTime = 0.3f;

        CashAnimationMin1 = 0.01f;
        CashAnimationMax1 = 0.4f;
        CashAnimationMin2 = 0.6f;
        CashAnimationMax2 = 0.8f;

        TunnelZValueMax = -10.0f;
        AlwaysAllowTunnelSpawns = false;


    }
}