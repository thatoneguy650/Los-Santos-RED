using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class CameraSystem
{
    //private static readonly Vector3 camOffsetFromPlayer = /*new Vector3(-0.31125f, 0.0f, 0.5f)*/Game.LocalPlayer.Character.GetPositionOffset(Game.LocalPlayer.Character.GetBonePosition(PedBoneId.Head)) + new Vector3(0.485f, 0.0f, -0.01225f);
    public static Camera Cam { get; set; } = new Camera(false);
    public static Camera InterpolationTempCam { get; set; } = new Camera(false);
    public static bool UsingOtherCamera = false;
    public static bool IsTransitioning = false;

    public static void HighLightCarjacking(Vehicle VehicleToLookAt,bool IsDriverSide)
    {
        if (IsTransitioning)
            return;

        GameFiber.StartNew(delegate
        {
            IsTransitioning = true;
            Cam.Position = GetCameraPosition(VehicleToLookAt,IsDriverSide);
            Cam.PointAtEntity(VehicleToLookAt, Vector3.Zero, false);

            Cam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
            InterpolationTempCam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
            InterpolationTempCam.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
            Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
            InterpolationTempCam.Rotation = new Rotator(r.X, r.Y, r.Z);
            InterpolationTempCam.Active = true;
            Game.LocalPlayer.Character.IsPositionFrozen = true;
            InterpolationTempCam.Interpolate(Cam, 1500, true, true, true);
            Game.LocalPlayer.Character.IsPositionFrozen = false;
            Cam.Active = true;
            UsingOtherCamera = true;
            IsTransitioning = false;
        });
       
    }

    public static void UnHighLightCarjacking(Vehicle VehicleToLookAt, bool IsDriverSide)
    {
        if (IsTransitioning)
            return;

        GameFiber.StartNew(delegate
        {
            IsTransitioning = true;
            Cam.Position = GetCameraPosition(VehicleToLookAt, IsDriverSide);
            Cam.PointAtEntity(VehicleToLookAt, Vector3.Zero, false);
            Cam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
            InterpolationTempCam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
            InterpolationTempCam.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
            Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
            InterpolationTempCam.Rotation = new Rotator(r.X, r.Y, r.Z);
            Cam.Active = true;
            Cam.Interpolate(InterpolationTempCam, 1500, true, true, true);
            InterpolationTempCam.Active = false;
            Cam.Active = false;
            UsingOtherCamera = false;
            IsTransitioning = false;
        });
    }
    public static void Interpolate(this Camera from, Camera to, int time, bool easeLocation, bool easeRotation, bool waitForCompletion)
    {
        NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(to, from, time, easeLocation, easeRotation);
        if (waitForCompletion)
            GameFiber.Sleep(time);
    }
    public static Vector3 GetCameraPosition(Vehicle VehicleToLookAt, bool IsDriverSide)
    {
        Vector3 CameraPosition;
        if (IsDriverSide)
            CameraPosition = VehicleToLookAt.GetOffsetPositionRight(-6f);
        else
            CameraPosition = VehicleToLookAt.GetOffsetPositionRight(6f);
        CameraPosition += new Vector3(0f, 0f, 1.8f);
        return CameraPosition;
    }
}

