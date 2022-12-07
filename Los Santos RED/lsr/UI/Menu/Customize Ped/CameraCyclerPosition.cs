using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public class CameraCyclerPosition
{

    private Vector3 CurrentBonePosition;
    private Vector3 DesiredCameraPosition;

    public CameraCyclerPosition(string name, int boneID, float offset, int order)
    {

        Name = name;
        BoneID = boneID;
        Offset = offset;
        Order = order;  
    }

    public string Name { get; set; }
    public int BoneID { get; set; }
    public float Offset { get; set; } = 1.0f;
    public int Order { get; set; }
    public void MoveToPosition(Camera charCam, PedExt modelPed)
    {
        EntryPoint.WriteToConsole($"CameraCyclerPosition MoveToPosition {Name}");
        CurrentBonePosition = NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", modelPed.Pedestrian, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", modelPed.Pedestrian, BoneID));
        DesiredCameraPosition = NativeHelper.GetOffsetPosition(CurrentBonePosition, modelPed.Pedestrian.Heading, Offset);
        charCam = new Camera(false);
        charCam.Position = DesiredCameraPosition;
        Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
        charCam.Rotation = new Rotator(r.X, r.Y, r.Z);
        charCam.Direction = (CurrentBonePosition - charCam.Position).ToNormalized();
        charCam.Active = true;
    }
}
