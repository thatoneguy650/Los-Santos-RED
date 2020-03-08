using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class CameraSystem
{
    private static uint GameTimeStartedAltCamera;
    private static Camera Cam = new Camera(false);
    private static Camera InterpolationTempCam = new Camera(false);
    private static bool IsTransitioning = false;
    private static bool AltCameraActive;
    private static bool IsInterpolating;

    public static bool AltCameraTimeOut
    {
        get
        {
            if (GameTimeStartedAltCamera == 0)
                return false;
            else if (Game.GameTime - GameTimeStartedAltCamera >= 20000)
                return true;
            else return false;
        }
    }
    public static void Tick()
    {
        if(AltCameraActive && AltCameraTimeOut)
        {

        }
    }
    public static void TransitionToAltCam(Entity EntityToLookAt,Vector3 CameraPosition)
    {
        if (IsTransitioning)
            return;

        GameFiber.StartNew(delegate
        {
            GameTimeStartedAltCamera = Game.GameTime;
            AltCameraActive = true;
            IsTransitioning = true;
            Cam.Position = CameraPosition;
            Cam.PointAtEntity(EntityToLookAt, Vector3.Zero, false);

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
            IsTransitioning = false;         
        });     
    }

    public static void RestoreGameplayerCamera(Entity EntityToLookAt, Vector3 CameraPosition)
    {
        if (IsTransitioning)
            return;

        GameFiber.StartNew(delegate
        {
            IsTransitioning = true;
            Cam.Position = CameraPosition;
            Cam.PointAtEntity(EntityToLookAt, Vector3.Zero, false);
            Cam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
            InterpolationTempCam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
            InterpolationTempCam.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
            Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
            InterpolationTempCam.Rotation = new Rotator(r.X, r.Y, r.Z);
            Cam.Active = true;
            Cam.Interpolate(InterpolationTempCam, 1500, true, true, true);
            InterpolationTempCam.Active = false;
            Cam.Active = false;
            IsTransitioning = false;
            GameTimeStartedAltCamera = 0;
            AltCameraActive = false;
        });
    }
    private static void Interpolate(this Camera from, Camera to, int time, bool easeLocation, bool easeRotation, bool waitForCompletion)
    {
        uint GameTimeStartedInterpolating = Game.GameTime;
        NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(to, from, time, easeLocation, easeRotation);
        if (waitForCompletion && Game.GameTime - GameTimeStartedInterpolating <= time)
        {
            IsInterpolating = true;
            GameFiber.Sleep(25);
        }
        IsInterpolating = false;
    }

}

