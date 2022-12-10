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



    public CameraCyclerPosition(string name, Vector3 cameraPosition, Vector3 cameraFocusPosition, int order)
    {
        Name = name;
        CameraPosition = cameraPosition;
        CameraFocusPosition = cameraFocusPosition;
        Order = order;
    }

    public CameraCyclerPosition(string name, Vector3 cameraPosition, Vector3 cameraDirection, Rotator cameraRotation, int order)
    {
        Name = name;
        CameraPosition = cameraPosition;
        CameraDirection = cameraDirection;
        CameraRotation = cameraRotation;
        Order = order;
    }

    public string Name { get; set; }

    public int Order { get; set; }
    public Vector3 CameraPosition { get; set; }
    public Vector3 CameraDirection { get; set; } = Vector3.Zero;
    public Rotator CameraRotation { get; set; }
    public Vector3 CameraFocusPosition { get; set; }

    public void Move(Camera charCam, PedExt modelPed)
    {
        EntryPoint.WriteToConsole($"CameraCyclerPosition MoveToPosition {Name}");
        if (!charCam.Exists())
        {
            charCam = new Camera(false);
        }
        charCam.Position = CameraPosition;
        if (CameraRotation == Rotator.Zero)
        {
            Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
            charCam.Rotation = new Rotator(r.X, r.Y, r.Z);
        }
        else
        {
            charCam.Rotation = CameraRotation;
        }
        if (CameraDirection == Vector3.Zero)
        { 
            charCam.Direction = (CameraFocusPosition - charCam.Position).ToNormalized();
        }
        else
        {
            charCam.Direction = CameraDirection;
        }
        charCam.Active = true;
    }



}
