using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class StoreCamera
{
    //private Camera StoreCam;
    private Camera EntranceCam;
    private Vector3 _direction;
    private Camera InterpolationCamera;
    private bool IsDisposed = false;
    private Vector3 EgressCamPosition;
    private float EgressCamFOV = 55f;
    private bool IsCancelled;
    private InteractableLocation Store;


    private IActivityPerformable Player;
    public Camera StoreCam { get; private set; }
    public StoreCamera(InteractableLocation store, IActivityPerformable player)
    {
        Store = store;
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
        DoEntryCam();
        HighlightStoreWithCamera();
        Game.LocalPlayer.Character.IsVisible = false;
        EntryPoint.WriteToConsole("Transaction: Setup Camera Ran", 5);
    }
    public void Dispose()
    {
        ReturnToGameplay();
        DoExitCam();
        EnableControl();
        if (StoreCam.Exists())
        {
            StoreCam.Delete();
        }
        if (InterpolationCamera.Exists())
        {
            InterpolationCamera.Delete();
        }
        if (EntranceCam.Exists())
        {
            EntranceCam.Delete();
        }
        Game.LocalPlayer.Character.Tasks.Clear();
    }

    private void DisableControl()
    {
        Game.LocalPlayer.HasControl = false;
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
    private void DoEntryCam()
    {
        Vector3 ToLookAtPos = NativeHelper.GetOffsetPosition(Store.EntrancePosition, Store.EntranceHeading + 90f, 2f);
        EgressCamPosition = NativeHelper.GetOffsetPosition(ToLookAtPos, Store.EntranceHeading, 1f);
        EgressCamPosition += new Vector3(0f, 0f, 0.4f);
        ToLookAtPos += new Vector3(0f, 0f, 0.4f);
        Vector3 EntranceStartWalkPosition = NativeHelper.GetOffsetPosition(Store.EntrancePosition, Store.EntranceHeading + 90f, 3f);

        if (!EntranceCam.Exists())
        {
            EntranceCam = new Camera(false);
        }

        EntranceCam.Position = EgressCamPosition;
        EntranceCam.Rotation = Store.CameraRotation;
        EntranceCam.Direction = Store.CameraDirection;
        EntranceCam.FOV = EgressCamFOV;
        Player.Character.Position = EntranceStartWalkPosition;
        Player.Character.Heading = Store.EntranceHeading - 180f;

        _direction = (ToLookAtPos - EgressCamPosition).ToNormalized();
        EntranceCam.Direction = _direction;
        EntranceCam.Active = true;

        Game.LocalPlayer.Character.Tasks.GoStraightToPosition(Store.EntrancePosition, 1.0f, Store.EntranceHeading - 180f, 1.0f, 3000);

        AnimationDictionary.RequestAnimationDictionay("gestures@f@standing@casual");
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "gestures@f@standing@casual", "gesture_bye_soft", 4.0f, -4.0f, -1, 50, 0, false, false, false);//-1

        SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_HOWS_IT_GOING", "GENERIC_HI" }, false);

        uint GameTimeStartedWalkingEntrance = Game.GameTime;
        while (Game.GameTime - GameTimeStartedWalkingEntrance <= 3000 && Player.Character.DistanceTo2D(Store.EntrancePosition) > 0.1f)
        {

            GameFiber.Yield();
        }

        Player.Character.IsVisible = false;
        //GameFiber.Sleep(3000);
        if (EntranceCam.Exists())
        {
            EntranceCam.Delete();
        }
        //get camera pos, 2 m out from door one meter left or right
        //get player start entrance pos, 3 m out from door
        //set player to walk from start entrance pos to entrance pos while camera goes 
        //stop camera, transition to the regular store cam
    }
    private void DoExitCam()
    {
        Vector3 ToLookAtPos = NativeHelper.GetOffsetPosition(Store.EntrancePosition, Store.EntranceHeading + 90f, 2f);
        EgressCamPosition = NativeHelper.GetOffsetPosition(ToLookAtPos, Store.EntranceHeading, 1f);
        EgressCamPosition += new Vector3(0f, 0f, 0.4f);
        ToLookAtPos += new Vector3(0f, 0f, 0.4f);
        Vector3 EntranceEndWalkPosition = NativeHelper.GetOffsetPosition(Store.EntrancePosition, Store.EntranceHeading + 90f, 3f);

        if (!EntranceCam.Exists())
        {
            EntranceCam = new Camera(false);
        }

        EntranceCam.Position = EgressCamPosition;
        EntranceCam.Rotation = Store.CameraRotation;
        EntranceCam.Direction = Store.CameraDirection;
        EntranceCam.FOV = EgressCamFOV;
        Player.Character.Position = Store.EntrancePosition;
        Player.Character.Heading = Store.EntranceHeading;



        _direction = (ToLookAtPos - EgressCamPosition).ToNormalized();
        EntranceCam.Direction = _direction;
        EntranceCam.Active = true;



        Player.Character.IsVisible = true;

        Game.LocalPlayer.Character.Tasks.GoStraightToPosition(EntranceEndWalkPosition, 1.0f, Store.EntranceHeading, 1.0f, 3000);


        AnimationDictionary.RequestAnimationDictionay("gestures@f@standing@casual");
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "gestures@f@standing@casual", "gesture_bye_soft", 4.0f, -4.0f, -1, 50, 0, false, false, false);//-1


        SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_THANKS", "GENERIC_BYE" }, false);


        uint GameTimeStartedWalkingEntrance = Game.GameTime;
        while (Game.GameTime - GameTimeStartedWalkingEntrance <= 3000 && Player.Character.DistanceTo2D(EntranceEndWalkPosition) > 0.1f)
        {

            GameFiber.Yield();
        }

        //GameFiber.Sleep(3000);

        if (EntranceCam.Exists())
        {
            EntranceCam.Delete();
        }

        //get camera pos, 2 m out from door one meter left or right
        //get player start entrance pos, 3 m out from door
        //set player to walk from start entrance pos to entrance pos while camera goes 
        //stop camera, transition to the regular store cam
    }
    private void HighlightStoreWithCamera()
    {
        if (!StoreCam.Exists())
        {
            StoreCam = new Camera(false);
        }
        if (Store.HasCustomCamera)
        {
            StoreCam.Position = Store.CameraPosition;
            StoreCam.Rotation = Store.CameraRotation;
            StoreCam.Direction = Store.CameraDirection;
        }
        else
        {
            float distanceAway = 10f;
            float distanceAbove = 7f;
            //if (Store.Type == LocationType.Hotel)
            //{
            //    distanceAway = 30f;
            //    distanceAbove = 20f;
            //}
            Vector3 InitialCameraPosition = NativeHelper.GetOffsetPosition(Store.EntrancePosition, Store.EntranceHeading + 90f, distanceAway);
            InitialCameraPosition = new Vector3(InitialCameraPosition.X, InitialCameraPosition.Y, InitialCameraPosition.Z + distanceAbove);
            StoreCam.Position = InitialCameraPosition;
            Vector3 ToLookAt = new Vector3(Store.EntrancePosition.X, Store.EntrancePosition.Y, Store.EntrancePosition.Z + 2f);
            _direction = (ToLookAt - InitialCameraPosition).ToNormalized();
            StoreCam.Direction = _direction;
        }
        StoreCam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
        if (!InterpolationCamera.Exists())
        {
            InterpolationCamera = new Camera(false);
        }
        InterpolationCamera.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
        InterpolationCamera.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
        Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
        InterpolationCamera.Rotation = new Rotator(r.X, r.Y, r.Z);
        InterpolationCamera.Active = true;
        NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(StoreCam, InterpolationCamera, 1500, true, true);
        GameFiber.Sleep(1500);
    }
    private void ReturnToGameplay()
    {
        //if (Store.HasCustomItemPostion)
        //{
        //    Game.FadeScreenOut(1500, true);
        //    StoreCam.Active = false;
        //    NativeFunction.Natives.CLEAR_FOCUS();
        //    //GameFiber.Sleep(500);
        //    Game.FadeScreenIn(1500, false);//was true   
        //}
        //else
        //{
            if (!InterpolationCamera.Exists())
            {
                InterpolationCamera = new Camera(false);
            }



            Vector3 ToLookAtPos = NativeHelper.GetOffsetPosition(Store.EntrancePosition, Store.EntranceHeading + 90f, 2f);
            EgressCamPosition = NativeHelper.GetOffsetPosition(ToLookAtPos, Store.EntranceHeading, 1f);
            EgressCamPosition += new Vector3(0f, 0f, 0.4f);
            ToLookAtPos += new Vector3(0f, 0f, 0.4f);
            _direction = (ToLookAtPos - EgressCamPosition).ToNormalized();



            InterpolationCamera.FOV = EgressCamFOV; //NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
            InterpolationCamera.Position = EgressCamPosition;// NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
            InterpolationCamera.Rotation = Store.CameraRotation;
            InterpolationCamera.Direction = _direction;



            InterpolationCamera.Active = true;
            NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(InterpolationCamera, StoreCam, 1500, true, true);
            GameFiber.Sleep(1500);
            InterpolationCamera.Active = false;
        }
    private bool CanSay(Ped ToSpeak, string Speech)
    {
        bool CanSay = NativeFunction.CallByHash<bool>(0x49B99BF3FDA89A7A, ToSpeak, Speech, 0);
        return CanSay;
    }
    private bool SayAvailableAmbient(Ped ToSpeak, List<string> Possibilities, bool WaitForComplete)
    {
        bool Spoke = false;
        if (Player.CanConverse)
        {
            foreach (string AmbientSpeech in Possibilities)
            {
                if (ToSpeak.Handle == Player.Character.Handle && Player.CharacterModelIsFreeMode)
                {
                    ToSpeak.PlayAmbientSpeech(Player.FreeModeVoice, AmbientSpeech, 0, SpeechModifier.Force);
                }
                else
                {
                    ToSpeak.PlayAmbientSpeech(null, AmbientSpeech, 0, SpeechModifier.Force);
                }
                //ToSpeak.PlayAmbientSpeech(null, AmbientSpeech, 0, SpeechModifier.Force);
                GameFiber.Sleep(100);
                if (ToSpeak.Exists() && ToSpeak.IsAnySpeechPlaying)
                {
                    Spoke = true;
                }
                //EntryPoint.WriteToConsole($"SAYAMBIENTSPEECH: {ToSpeak.Handle} Attempting {AmbientSpeech}, Result: {Spoke}");
                if (Spoke)
                {
                    break;
                }
            }
            GameFiber.Sleep(100);
            while (ToSpeak.Exists() && ToSpeak.IsAnySpeechPlaying && WaitForComplete && Player.CanConverse)
            {
                Spoke = true;
                GameFiber.Yield();
            }
            if (!Spoke)
            {
                //Game.DisplayNotification($"\"{Possibilities.FirstOrDefault()}\"");
            }
        }
        return Spoke;
    }

}

