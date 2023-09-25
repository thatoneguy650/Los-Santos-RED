using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CameraControl
{
    private ICameraControllable Player;
    private Camera camFrom;
    private Camera camTo;
    public CameraControl(ICameraControllable player)
    {
        Player = player;
    }
    private enum eSetPlayerControlFlag
    {
        SPC_AMBIENT_SCRIPT = (1 << 1),
        SPC_CLEAR_TASKS = (1 << 2),
        SPC_REMOVE_FIRES = (1 << 3),
        SPC_REMOVE_EXPLOSIONS = (1 << 4),
        SPC_REMOVE_PROJECTILES = (1 << 5),
        SPC_DEACTIVATE_GADGETS = (1 << 6),
        SPC_REENABLE_CONTROL_ON_DEATH = (1 << 7),
        SPC_LEAVE_CAMERA_CONTROL_ON = (1 << 8),
        SPC_ALLOW_PLAYER_DAMAGE = (1 << 9),
        SPC_DONT_STOP_OTHER_CARS_AROUND_PLAYER = (1 << 10),
        SPC_PREVENT_EVERYBODY_BACKOFF = (1 << 11),
        SPC_ALLOW_PAD_SHAKE = (1 << 12)
    };
    public void Setup()
    {
        DisableControl();
    }
    public void Dispose()
    {
        TransitionToGameplayCam(false);
    }
    public void TransitionHighlightEntity(Entity toHighlight, bool wait, float XOffset, float YOffset,float ZOffset)
    {
        if (toHighlight.Exists())//will freeze on the second camera movement
        {
            if (!camFrom.Exists())
            {
                camFrom = new Camera(false);
            }
            camFrom.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
            camFrom.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
            Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(0);
            camFrom.Rotation = new Rotator(r.X, r.Y, r.Z);

            if (!camTo.Exists())
            {
                camTo = new Camera(false);
            }

            Vector3 InitialCameraPosition = toHighlight.GetOffsetPosition(new Vector3(XOffset, YOffset, ZOffset));
            Vector3 ToLookAt = new Vector3(toHighlight.Position.X, toHighlight.Position.Y, toHighlight.Position.Z + 0.5f);
            Vector3 _direction = (ToLookAt - InitialCameraPosition).ToNormalized();
            camTo.Position = InitialCameraPosition;
            camTo.Direction = _direction;
            camTo.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();

            camFrom.Active = true;

            NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(camTo, camFrom, 2500, true, true);
            //NativeFunction.Natives.SET_FOCUS_POS_AND_VEL(camTo.Position.X, camTo.Position.Y, camTo.Position.Z, 0f, 0f, 0f);
            if (wait)
            {
                GameFiber.Sleep(2500);
            }
        }
    }
    public void TransitionHighlightEntity(Entity toHighlight, bool wait)
    {
        float XOffset = RandomItems.RandomPercent(50) ? -2f : 2f;//  RandomItems.GetRandomNumber(-3f, 3f);
        float YOffset = 2f;// RandomItems.GetRandomNumber(1f, 3f);
        float ZOffset = 1f;//;RandomItems.GetRandomNumber(1f, 2f);
        TransitionHighlightEntity(toHighlight, wait, XOffset, YOffset, ZOffset);
    }

    public void HighlightEntity(Entity toHighlight, float xOffset, float yOffset, float zOffset)
    {
        if (toHighlight.Exists())//will freeze on the second camera movement
        {
            if (!camTo.Exists())
            {
                camTo = new Camera(false);
            }
            float XOffset = xOffset;// RandomItems.RandomPercent(50) ? -2f : 2f;//  RandomItems.GetRandomNumber(-3f, 3f);
            float YOffset = yOffset;// 2f;// RandomItems.GetRandomNumber(1f, 3f);
            float ZOffset = zOffset;// 1f;//;RandomItems.GetRandomNumber(1f, 2f);
            Vector3 InitialCameraPosition = toHighlight.GetOffsetPosition(new Vector3(XOffset, YOffset, ZOffset));
            Vector3 ToLookAt = new Vector3(toHighlight.Position.X, toHighlight.Position.Y, toHighlight.Position.Z + 0.5f);
            Vector3 _direction = (ToLookAt - InitialCameraPosition).ToNormalized();
            camTo.Position = InitialCameraPosition;
            camTo.Direction = _direction;
            camTo.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
            camTo.Active = true;
        }
    }
    public void HighlightEntity(Entity toHighlight) => HighlightEntity(toHighlight, RandomItems.RandomPercent(50) ? -2f : 2f, 2f, 1f);
    public void ReturnToGameplayCam()
    {  
        if (camTo.Exists())
        {
            camTo.Active = false;
            camTo.Delete();
        }
        if (camFrom.Exists())
        {
            camFrom.Active = false;
            camFrom.Delete();
        }
        EnableControl();
    }
    public void SetCameraAt(Vector3 desiredPosition, Vector3 desiredDirection, Rotator desiredRotation, bool wait)
    {
        if (!camFrom.Exists())
        {
            camFrom = new Camera(false);
        }
        camFrom.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
        camFrom.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
        Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(0);
        camFrom.Rotation = new Rotator(r.X, r.Y, r.Z);

        if (!camTo.Exists())
        {
            camTo = new Camera(false);
        }
        camTo.Position = desiredPosition;
        camTo.Direction = desiredDirection;
        camTo.Rotation = desiredRotation;
        camTo.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();

        camFrom.Active = true;

        NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(camTo, camFrom, 2500, true, true);
        //NativeFunction.Natives.SET_FOCUS_POS_AND_VEL(camTo.Position.X, camTo.Position.Y, camTo.Position.Z, 0f, 0f, 0f);
        if (wait)
        {
            GameFiber.Sleep(2500);
        }    
    }
    public void TransitionToGameplayCam(bool wait)
    {
        if (!camFrom.Exists())
        {
            camFrom = new Camera(false);
        }
        if (Camera.RenderingCamera != null)
        {
            camFrom.Position = Camera.RenderingCamera.Position;
            camFrom.FOV = Camera.RenderingCamera.FOV;
            camFrom.Rotation = Camera.RenderingCamera.Rotation;
        }
        else
        {
            camFrom.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
            camFrom.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
            Vector3 r2 = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(0);
            camFrom.Rotation = new Rotator(r2.X, r2.Y, r2.Z);
        }

        if (!camTo.Exists())
        {
            camTo = new Camera(false);
        }
        camTo.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
        camTo.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
        Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(0);
        camTo.Rotation = new Rotator(r.X, r.Y, r.Z);
        camTo.Heading = NativeFunction.Natives.GetGameplayCamRelativeHeading<float>();

        camFrom.Active = true;

        NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(camTo, camFrom, 2500, true, true);
        if (wait)
        {
            FinishReturn();
        }
        else
        {
            GameFiber CameraWatcher = GameFiber.StartNew(delegate
            {
                try
                { 
                    FinishReturn();
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "CameraWatcher");
        }
        
    }
    private void FinishReturn()
    {
        GameFiber.Sleep(2500);
        camTo.Active = false;
        if (camTo.Exists())
        {
            camTo.Delete();
        }
        if (camFrom.Exists())
        {
            camFrom.Delete();
        }
        EnableControl();
        //NativeFunction.Natives.CLEAR_FOCUS();
    }
    private void DisableControl()
    {
        NativeHelper.DisablePlayerControl();
        //Game.LocalPlayer.HasControl = false;
        NativeFunction.Natives.SET_PLAYER_CONTROL(Game.LocalPlayer, (int)eSetPlayerControlFlag.SPC_LEAVE_CAMERA_CONTROL_ON, false);
        Game.DisableControlAction(0, GameControl.LookLeftRight, false);
        Game.DisableControlAction(0, GameControl.LookUpDown, false);
    }
    private void EnableControl()
    {
        Game.DisableControlAction(0, GameControl.LookLeftRight, false);
        Game.DisableControlAction(0, GameControl.LookUpDown, false);
        Game.LocalPlayer.HasControl = true;
        NativeFunction.Natives.SET_PLAYER_CONTROL(Game.LocalPlayer, (int)eSetPlayerControlFlag.SPC_LEAVE_CAMERA_CONTROL_ON, true);
    } 

}