using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class CameraScript
{
    private static Camera Cam = new Camera(false);
    private static Camera InterpolationTempCam = new Camera(false);
    private static bool IsTransitioning = false;

    public static void TransitionToAltCam(Entity EntityToLookAt, Vector3 CameraPosition,int TimeToWait)
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
            InterpolationTempCam.Active = true;
            InterpolationTempCam.Interpolate(Cam, TimeToWait, true, true, true);//1500 to wait is okay
            Cam.Active = true;
            IsTransitioning = false;
        });
    }
    private static void Interpolate(this Camera from, Camera to, int time, bool easeLocation, bool easeRotation, bool waitForCompletion)
    {
        uint GameTimeStartedInterpolating = Game.GameTime;
        NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(to, from, time, easeLocation, easeRotation);
        if (waitForCompletion && Game.GameTime - GameTimeStartedInterpolating <= time)
        {
            GameFiber.Sleep(25);
        }
    }
    public static void RestoreGameplayerCamera()
    {
        if(Cam.Active)
            NativeFunction.CallByName<bool>("RENDER_SCRIPT_CAMS", false, true, 2000, 0, 0);
    }
    public static void DebugAbort()
    {
        Cam.Active = false;
    }


}

