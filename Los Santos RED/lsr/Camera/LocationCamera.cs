using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class LocationCamera
{
    //private Camera StoreCam;
    private Camera EntranceCam;
    private Vector3 _direction;
    private Camera CameraTo;
    private bool IsDisposed = false;
    private Vector3 EgressCamPosition;
    private Vector3 EntityCamPosition;
    private float EgressCamFOV = 55f;
    private float EntityCamFOV = 55f;
    private bool IsCancelled;
    private GameLocation Store;
    private bool isHighlightingLocation = false;
    private float PlayerHeading = 0f;
    private Vector3 PlayerPosition;
    private ISettingsProvideable Settings;
    private Entity currentHighlightedEntity;

    private Vector3 CurrentFocusPosition;


    private Vector3 HomePosition;
    private Vector3 HomeDirection;
    private Rotator HomeRotator;

    public Vector3 ItemPreviewPosition { get; set; } = Vector3.Zero;
    public float ItemPreviewHeading { get; set; } = 0f;


    private ILocationInteractable Player;
    public Camera StoreCam { get; private set; }
    public Camera CurrentCamera { get; private set; }
    public bool SayGreeting { get; set; } = true;
    public bool ForceRegularCamera { get; set; } = false;


    public bool StaysInVehicle { get; set; } = false;
    public bool NoEntryCam { get; set; } = false;
    public Interior Interior { get; set; }
    public bool IsInterior { get; set; } = false;
    public bool HasHomeCam => HomePosition != Vector3.Zero;

    public LocationCamera(GameLocation store, ILocationInteractable player, ISettingsProvideable settings, bool noEntryCam)
    {
        Store = store;
        Player = player;
        Settings = settings;
        NoEntryCam = noEntryCam;
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
        if(StaysInVehicle || NoEntryCam)
        {
            return;
        }
        Game.LocalPlayer.Character.IsVisible = false;
        PlayerPosition = Player.Position;
        PlayerHeading = Player.Character.Heading;
        NativeFunction.Natives.SET_EVERYONE_IGNORE_PLAYER(Game.LocalPlayer, true);
    }
    public void Dispose()
    {
        NativeFunction.Natives.SET_EVERYONE_IGNORE_PLAYER(Game.LocalPlayer, false);
        if (isHighlightingLocation)
        {
            Game.FadeScreenOut(1500, true);
            NativeFunction.Natives.CLEAR_FOCUS();     
            ReturnToGameplay();
            Game.FadeScreenIn(1500, true);
            DoExitCam();
        }
        else
        {
            ReturnToGameplay();
            DoExitCam();
        }
        EnableControl();
        if (StoreCam.Exists())
        {
            StoreCam.Delete();
        }
        if (CameraTo.Exists())
        {
            CameraTo.Delete();
        }
        if (EntranceCam.Exists())
        {
            EntranceCam.Delete();
        }
        if(StaysInVehicle || NoEntryCam)
        {
            NativeFunction.Natives.CLEAR_FOCUS();
            return;
        }
        Game.LocalPlayer.Character.Tasks.Clear();
    }

    public void DoEntranceOnly()
    {
        DisableControl();
        DoEntryCam();
        if (StaysInVehicle || NoEntryCam)
        {
            return;
        }
        Game.LocalPlayer.Character.IsVisible = false;
        PlayerPosition = Player.Position;
        PlayerHeading = Player.Character.Heading;
        NativeFunction.Natives.SET_EVERYONE_IGNORE_PLAYER(Game.LocalPlayer, true);
    }
    public void DoExitOnly()
    {

    }

    public void StopImmediately(bool clearTasks)
    {
        NativeFunction.Natives.SET_EVERYONE_IGNORE_PLAYER(Game.LocalPlayer, false);
        EnableControl();
        Player.Character.IsVisible = true;
        NativeFunction.Natives.CLEAR_FOCUS();
        if (StoreCam.Exists())
        {
            StoreCam.Delete();
        }
        if (CameraTo.Exists())
        {
            CameraTo.Delete();
        }
        if (EntranceCam.Exists())
        {
            EntranceCam.Delete();
        }
        if (clearTasks)
        {
            Game.LocalPlayer.Character.Tasks.Clear();
        }
        CurrentFocusPosition = Vector3.Zero;
    }

    //public void StopHighlightingPosition()
    //{
    //    NativeFunction.Natives.SET_EVERYONE_IGNORE_PLAYER(Game.LocalPlayer, false);
    //    ReturnToGameplay();
    //    EnableControl();
    //    StopImmediately();
    //}


    public void HighlightEntity(Entity toHighlight)
    {
        if (toHighlight.Exists())//will freeze on the second camera movement
        {
            //if(currentHighlightedEntity.Exists() && toHighlight.Handle == currentHighlightedEntity.Handle)
            //{

            //}


            if (!StoreCam.Exists())
            {
                StoreCam = new Camera(false);
            }

            float width = toHighlight.Model.Dimensions.X;
            float length = toHighlight.Model.Dimensions.Y;
            float height = toHighlight.Model.Dimensions.Z;
            Vector3 InitialCameraPosition;
            if (width >= 5f || length >= 5f || height >= 5f)
            {
                InitialCameraPosition = toHighlight.GetOffsetPosition(new Vector3(8f, 8f, 2f));
            }
            else
            {
                InitialCameraPosition = toHighlight.GetOffsetPosition(new Vector3(5f, 5f, 2f));
            }
            
            StoreCam.Position = InitialCameraPosition;
            Vector3 ToLookAt = new Vector3(toHighlight.Position.X, toHighlight.Position.Y, toHighlight.Position.Z + 0.5f);
            _direction = (ToLookAt - InitialCameraPosition).ToNormalized();
            StoreCam.Direction = _direction;
            StoreCam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
            if (!CameraTo.Exists())
            {
                CameraTo = new Camera(false);
            }
            if(Camera.RenderingCamera != null)
            {
                CameraTo.Position = Camera.RenderingCamera.Position;
                CameraTo.FOV = Camera.RenderingCamera.FOV;
                CameraTo.Rotation = Camera.RenderingCamera.Rotation;
            }
            else
            {
                CameraTo.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
                CameraTo.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
                Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
                CameraTo.Rotation = new Rotator(r.X, r.Y, r.Z);
                CameraTo.Active = true;
            }
            NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(StoreCam, CameraTo, 1500, true, true);
            GameFiber.Sleep(1500);
        }
    }
    public void MoveToPosition(Vector3 desiredPosition, Vector3 desiredDirection, Rotator desiredRotation, bool wait, bool setHomePosition, bool instant)
    {
        if (!StoreCam.Exists())
        {
            StoreCam = new Camera(false);
        }
        //EntryPoint.WriteToConsole($"Camera.RenderingCamera.Position{Camera.RenderingCamera.Position}");
        //EntryPoint.WriteToConsole($"StoreCam.Position{StoreCam.Position}");
        //StoreCam.Position = Camera.RenderingCamera.Position;
        //StoreCam.FOV = Camera.RenderingCamera.FOV;
        //StoreCam.Rotation = Camera.RenderingCamera.Rotation;
        //StoreCam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();

        if (Camera.RenderingCamera != null)
        {
            StoreCam.Position = Camera.RenderingCamera.Position;
            StoreCam.FOV = Camera.RenderingCamera.FOV;
            StoreCam.Rotation = Camera.RenderingCamera.Rotation;
        }
        else
        {
            StoreCam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
            StoreCam.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
            Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
            StoreCam.Rotation = new Rotator(r.X, r.Y, r.Z);
            StoreCam.Active = true;
        }







        if (!CameraTo.Exists())
        {
            CameraTo = new Camera(false);
        }
        CameraTo.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
        CameraTo.Position = desiredPosition;
        CameraTo.Rotation = desiredRotation;
        CameraTo.Direction = desiredDirection;
        CameraTo.Active = true;


        if(setHomePosition)
        {
            HomePosition = desiredPosition;
            HomeRotator = desiredRotation;
            HomeDirection = desiredDirection;
        }
        int waitTime = 1500;
        if(instant)
        {
            waitTime = 0;
        }
        NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(CameraTo, StoreCam, waitTime, true, true);
        if (wait)
        {
            GameFiber.Sleep(1500);
        }
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
    private void DoEntryCam()
    {
        if(StaysInVehicle || NoEntryCam)
        {
            return;
        }
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

        if (SayGreeting)
        {
            AnimationDictionary.RequestAnimationDictionay("gestures@f@standing@casual");
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "gestures@f@standing@casual", "gesture_bye_soft", 4.0f, -4.0f, -1, 50, 0, false, false, false);//-1
            Player.PlaySpeech(new List<string>() { "GENERIC_HOWS_IT_GOING", "GENERIC_HI" }.PickRandom(), false);
        }
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
        if (StaysInVehicle || NoEntryCam)
        {
            return;
        }
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
        NativeFunction.Natives.CLEAR_FOCUS();

        //NativeFunction.Natives.SET_FOCUS_POS_AND_VEL(Player.Character.Position.X, Player.Character.Position.Y, Player.Character.Position.Z, 0f, 0f, 0f);

        Game.LocalPlayer.Character.Tasks.GoStraightToPosition(EntranceEndWalkPosition, 1.0f, Store.EntranceHeading, 1.0f, 3000);

        if (SayGreeting)
        {
            AnimationDictionary.RequestAnimationDictionay("gestures@f@standing@casual");
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "gestures@f@standing@casual", "gesture_bye_soft", 4.0f, -4.0f, -1, 50, 0, false, false, false);//-1
            Player.PlaySpeech(new List<string>() { "GENERIC_THANKS", "GENERIC_BYE" }.PickRandom(),false);
        }

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
        //if(IsInterior && Interior != null)
        //{
        //    if(Interior.StandardInteractCameraPosition != Vector3.Zero)
        //    {
        //        StoreCam.Position = Interior.StandardInteractCameraPosition;
        //        StoreCam.Rotation = Interior.StandardInteractCameraRotation;
        //        StoreCam.Direction = Interior.StandardInteractCameraDirection;
        //    }
        //    else
        //    {
        //        float distanceAway = 3f;
        //        float distanceAbove = 4f;
        //        Vector3 InitialCameraPosition = NativeHelper.GetOffsetPosition(Interior.StandardInteractLocation, Interior.StandardInteractHeading + 90f, distanceAway);
        //        InitialCameraPosition = new Vector3(InitialCameraPosition.X, InitialCameraPosition.Y, InitialCameraPosition.Z + distanceAbove);
        //        StoreCam.Position = InitialCameraPosition;
        //        Vector3 ToLookAt = new Vector3(Interior.StandardInteractLocation.X, Interior.StandardInteractLocation.Y, Interior.StandardInteractLocation.Z + 2f);
        //        _direction = (ToLookAt - InitialCameraPosition).ToNormalized();
        //        StoreCam.Direction = _direction;
        //    }
        //}
        //else 
        
        if (Store.HasCustomCamera && !ForceRegularCamera)
        {
            StoreCam.Position = Store.CameraPosition;
            StoreCam.Rotation = Store.CameraRotation;
            StoreCam.Direction = Store.CameraDirection;
        }
        else
        {
            float distanceAway = 10f;
            float distanceAbove = 7f;
            Vector3 InitialCameraPosition = NativeHelper.GetOffsetPosition(Store.EntrancePosition, Store.EntranceHeading + 90f, distanceAway);
            InitialCameraPosition = new Vector3(InitialCameraPosition.X, InitialCameraPosition.Y, InitialCameraPosition.Z + distanceAbove);
            StoreCam.Position = InitialCameraPosition;
            Vector3 ToLookAt = new Vector3(Store.EntrancePosition.X, Store.EntrancePosition.Y, Store.EntrancePosition.Z + 2f);
            _direction = (ToLookAt - InitialCameraPosition).ToNormalized();
            StoreCam.Direction = _direction;
        }

        StoreCam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
        if (!CameraTo.Exists())
        {
            CameraTo = new Camera(false);
        }
        CameraTo.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
        CameraTo.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
        Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
        CameraTo.Rotation = new Rotator(r.X, r.Y, r.Z);
        CameraTo.Active = true;
        NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(StoreCam, CameraTo, 1500, true, true);
        NativeFunction.Natives.SET_FOCUS_POS_AND_VEL(StoreCam.Position.X, StoreCam.Position.Y, StoreCam.Position.Z, 0f, 0f, 0f);
        GameFiber.Sleep(1500);
    }
    public void ReHighlightStoreWithCamera()
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
            Vector3 InitialCameraPosition = NativeHelper.GetOffsetPosition(Store.EntrancePosition, Store.EntranceHeading + 90f, distanceAway);
            InitialCameraPosition = new Vector3(InitialCameraPosition.X, InitialCameraPosition.Y, InitialCameraPosition.Z + distanceAbove);
            StoreCam.Position = InitialCameraPosition;
            Vector3 ToLookAt = new Vector3(Store.EntrancePosition.X, Store.EntrancePosition.Y, Store.EntrancePosition.Z + 2f);
            _direction = (ToLookAt - InitialCameraPosition).ToNormalized();
            StoreCam.Direction = _direction;
        }
        StoreCam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
        if (!CameraTo.Exists())
        {
            CameraTo = new Camera(false);
        }

        if (Camera.RenderingCamera != null)
        {
            CameraTo.Position = Camera.RenderingCamera.Position;
            CameraTo.FOV = Camera.RenderingCamera.FOV;
            CameraTo.Rotation = Camera.RenderingCamera.Rotation;
        }
        else
        {
            CameraTo.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
            CameraTo.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
            Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
            CameraTo.Rotation = new Rotator(r.X, r.Y, r.Z);
            CameraTo.Active = true;
        }

        CameraTo.Active = true;
        NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(StoreCam, CameraTo, 1500, true, true);
        GameFiber.Sleep(1500);
    }
    private void HighlightLocationWithCamera()
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
            Game.FadeScreenOut(1500, true);
            NativeFunction.Natives.SET_FOCUS_POS_AND_VEL(Store.CameraPosition.X, Store.CameraPosition.Y, Store.CameraPosition.Z, 0f, 0f, 0f);
            Vector3 ToLookAt = new Vector3(ItemPreviewPosition.X, ItemPreviewPosition.Y, ItemPreviewPosition.Z);
            _direction = (ToLookAt - Store.CameraPosition).ToNormalized();
            StoreCam.Direction = _direction;
            StoreCam.Active = true;
            GameFiber.Sleep(500);
            Game.FadeScreenIn(1500, true);
        }
    }
    private void ReturnToGameplay()
    {
        if(StaysInVehicle || NoEntryCam)
        {
            return;
        }

        if (!CameraTo.Exists())
        {
            CameraTo = new Camera(false);
        }

        Vector3 ToLookAtPos = NativeHelper.GetOffsetPosition(Store.EntrancePosition, Store.EntranceHeading + 90f, 2f);
        EgressCamPosition = NativeHelper.GetOffsetPosition(ToLookAtPos, Store.EntranceHeading, 1f);
        EgressCamPosition += new Vector3(0f, 0f, 0.4f);
        ToLookAtPos += new Vector3(0f, 0f, 0.4f);
        _direction = (ToLookAtPos - EgressCamPosition).ToNormalized();

        CameraTo.FOV = EgressCamFOV; //NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
        CameraTo.Position = EgressCamPosition;// NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
        CameraTo.Rotation = Store.CameraRotation;
        CameraTo.Direction = _direction;


        if(!StoreCam.Exists())
        {
            return;
        }

        CameraTo.Active = true;
        NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(CameraTo, StoreCam, 1500, true, true);
        GameFiber.Sleep(1500);
        CameraTo.Active = false;
    }

    public void ReturnToGameplay(bool wait)
    {
        if(Camera.RenderingCamera == null)
        {
            return;
        }
        if (!StoreCam.Exists())
        {
            StoreCam = new Camera(false);
        }
        StoreCam.Position = Camera.RenderingCamera.Position;
        StoreCam.FOV = Camera.RenderingCamera.FOV;
        StoreCam.Rotation = Camera.RenderingCamera.Rotation;
        if (!CameraTo.Exists())
        {
            CameraTo = new Camera(false);
        }
        CameraTo.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
        CameraTo.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
        Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
        CameraTo.Rotation = new Rotator(r.X, r.Y, r.Z);
        CameraTo.Active = true;
        NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(CameraTo, StoreCam, 1500, true, true);//Destination first, then source
        if (wait)
        {
            GameFiber.Sleep(1500);
        }
    }

    public void HighlightHome()
    {
        Vector3 CurrentPosition = Vector3.Zero;
        if (StoreCam.Exists())
        {
            CurrentPosition = StoreCam.Position;
        }
        if (CurrentPosition.DistanceTo(Store.EntrancePosition) <= 100f)
        {
            TransitionFromLocation();
        }
        else
        {
            SetCamHome();
        }
    }
    public void HighlightPosition(Vector3 focusPosition, float focusHeading)
    {
        Vector3 CurrentPosition = Vector3.Zero;
        if(StoreCam.Exists())
        {
            CurrentPosition = StoreCam.Position;
        }
        if(CurrentPosition.DistanceTo(focusPosition) <= 100f)//300f)
        {
            TransitionToLocation(focusPosition, focusHeading);
        }
        else
        {
            SetCamAt(focusPosition, focusHeading);  
        }
    }
    public void HighlightVehicle()
    {
        Vector3 CurrentPosition = Vector3.Zero;
        if (StoreCam.Exists())
        {
            CurrentPosition = StoreCam.Position;
        }
        if(Store.HasCustomVehicleCamera)
        {
            EntryPoint.WriteToConsole("STORE CAM HAS CUSTOM VEHICLE CAMERA");
            if (CurrentPosition.DistanceTo(Store.VehiclePreviewCameraPosition) <= 300f)
            {
                TransitionToVehicleCamera();
            }
            else
            {
                SetCamAtVehiclePosition();
            }
        }
        else
        {
            EntryPoint.WriteToConsole("STORE CAM NO CUSTOM VEHICLE CAMERA!!!");
            HighlightPosition(Store.VehiclePreviewLocation.Position, Store.VehiclePreviewLocation.Heading);
        }
    }


    private void TransitionToVehicleCamera()
    {
        if (CurrentFocusPosition == Store.VehiclePreviewCameraPosition)
        {
            return;
        }
        if (!StoreCam.Exists())
        {
            StoreCam = new Camera(false);
        }
        isHighlightingLocation = true;


        StoreCam.Position = Store.VehiclePreviewCameraPosition;
        StoreCam.Rotation = Store.VehiclePreviewCameraRotation;
        StoreCam.Direction = Store.VehiclePreviewCameraDirection;


        if (!CameraTo.Exists())
        {
            CameraTo = new Camera(false);
        }
        if (Camera.RenderingCamera != null)
        {
            CameraTo.Position = Camera.RenderingCamera.Position;
            CameraTo.FOV = Camera.RenderingCamera.FOV;
            CameraTo.Rotation = Camera.RenderingCamera.Rotation;
        }
        else
        {
            CameraTo.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
            CameraTo.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
            Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
            CameraTo.Rotation = new Rotator(r.X, r.Y, r.Z);
            CameraTo.Active = true;
        }
        CameraTo.Active = true;
        NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(StoreCam, CameraTo, 1500, true, true);
        GameFiber.Sleep(1500);
        CurrentFocusPosition = Store.VehiclePreviewCameraPosition;
    }

    private void SetCamAtVehiclePosition()
    {
        if (CurrentFocusPosition == Store.VehiclePreviewCameraPosition)
        {
            return;
        }
        if (!StoreCam.Exists())
        {
            StoreCam = new Camera(false);
        }
        Game.FadeScreenOut(500, true);
        StoreCam.Position = Store.VehiclePreviewCameraPosition;
        StoreCam.Rotation = Store.VehiclePreviewCameraRotation;
        StoreCam.Direction = Store.VehiclePreviewCameraDirection;
        NativeFunction.Natives.SET_FOCUS_POS_AND_VEL(Store.VehiclePreviewCameraPosition.X, Store.VehiclePreviewCameraPosition.Y, Store.VehiclePreviewCameraPosition.Z, 0f, 0f, 0f);
        StoreCam.Active = true;
        isHighlightingLocation = true;
        GameFiber.Sleep(500);
        Game.FadeScreenIn(500, true);
        CurrentFocusPosition = Store.VehiclePreviewCameraPosition;
    }




    private void SetCamHome()
    {
        if (!StoreCam.Exists())
        {
            StoreCam = new Camera(false);
        }
        Game.FadeScreenOut(500, true);     
        SetCamAutoEntrance();
        NativeFunction.Natives.SET_FOCUS_POS_AND_VEL(Store.EntrancePosition.X, Store.EntrancePosition.Y, Store.EntrancePosition.Z, 0f, 0f, 0f);
        StoreCam.Active = true;
        isHighlightingLocation = false;
        GameFiber.Sleep(500);
        Game.FadeScreenIn(500, true);
        CurrentFocusPosition = Vector3.Zero;
    }
    private void SetCamAt(Vector3 focusPosition, float focusHeading)
    {
        if (CurrentFocusPosition == focusPosition)
        {
            return;
        }
        if (!StoreCam.Exists())
        {
            StoreCam = new Camera(false);
        }
        Game.FadeScreenOut(500, true);    
        SetCamAutoPosition(focusPosition, focusHeading);
        NativeFunction.Natives.SET_FOCUS_POS_AND_VEL(focusPosition.X, focusPosition.Y, focusPosition.Z, 0f, 0f, 0f);
        StoreCam.Active = true;
        isHighlightingLocation = true;
        GameFiber.Sleep(500);
        Game.FadeScreenIn(500, true);
        CurrentFocusPosition = focusPosition;
    }
    private void TransitionFromLocation()
    {
        if (!StoreCam.Exists())
        {
            StoreCam = new Camera(false);
        }
        SetCamAutoEntrance();
        if (!CameraTo.Exists())
        {
            CameraTo = new Camera(false);
        }
        if (Camera.RenderingCamera != null)
        {
            CameraTo.Position = Camera.RenderingCamera.Position;
            CameraTo.FOV = Camera.RenderingCamera.FOV;
            CameraTo.Rotation = Camera.RenderingCamera.Rotation;
        }
        else
        {
            CameraTo.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
            CameraTo.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
            Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
            CameraTo.Rotation = new Rotator(r.X, r.Y, r.Z);
            CameraTo.Active = true;
        }
        CameraTo.Active = true;
        NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(StoreCam, CameraTo, 1500, true, true);
        GameFiber.Sleep(1500);
        CurrentFocusPosition = Vector3.Zero;
        isHighlightingLocation = false;
    }
    private void TransitionToLocation(Vector3 focusPosition, float focusHeading)
    {
        if (CurrentFocusPosition == focusPosition)
        {
            return;
        }
        if (!StoreCam.Exists())
        {
            StoreCam = new Camera(false);
        }
        isHighlightingLocation = true;
        SetCamAutoPosition(focusPosition, focusHeading);
        if (!CameraTo.Exists())
        {
            CameraTo = new Camera(false);
        }
        if (Camera.RenderingCamera != null)
        {
            CameraTo.Position = Camera.RenderingCamera.Position;
            CameraTo.FOV = Camera.RenderingCamera.FOV;
            CameraTo.Rotation = Camera.RenderingCamera.Rotation;
        }
        else
        {
            CameraTo.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
            CameraTo.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
            Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
            CameraTo.Rotation = new Rotator(r.X, r.Y, r.Z);
            CameraTo.Active = true;
        }
        CameraTo.Active = true;
        NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(StoreCam, CameraTo, 1500, true, true);
        GameFiber.Sleep(1500);
        CurrentFocusPosition = focusPosition;
    }
    private void SetCamAutoEntrance()
    {
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
            Vector3 InitialCameraPosition = NativeHelper.GetOffsetPosition(Store.EntrancePosition, Store.EntranceHeading + 90f, distanceAway);
            InitialCameraPosition = new Vector3(InitialCameraPosition.X, InitialCameraPosition.Y, InitialCameraPosition.Z + distanceAbove);
            StoreCam.Position = InitialCameraPosition;
            Vector3 ToLookAt = new Vector3(Store.EntrancePosition.X, Store.EntrancePosition.Y, Store.EntrancePosition.Z + 2f);
            _direction = (ToLookAt - InitialCameraPosition).ToNormalized();
            StoreCam.Direction = _direction;
        }
        StoreCam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
    }
    private void SetCamAutoPosition(Vector3 focusPosition, float focusHeading)
    {
        float distanceX = Settings.SettingsManager.PlayerOtherSettings.VehicleAutoCameraXDistance;// 5f;
        float distanceAway = Settings.SettingsManager.PlayerOtherSettings.VehicleAutoCameraYDistance;// 5f;
        float distanceAbove = Settings.SettingsManager.PlayerOtherSettings.VehicleAutoCameraZDistance;// 3f;
        Vector3 InitialCameraPosition = NativeHelper.GetOffsetPosition(focusPosition, focusHeading + 90f, distanceAway);
        InitialCameraPosition = NativeHelper.GetOffsetPosition(InitialCameraPosition, focusHeading, distanceX);
        InitialCameraPosition = new Vector3(InitialCameraPosition.X, InitialCameraPosition.Y, InitialCameraPosition.Z + distanceAbove);
        StoreCam.Position = InitialCameraPosition;
        Vector3 ToLookAt1 = new Vector3(focusPosition.X, focusPosition.Y, focusPosition.Z);
        _direction = (ToLookAt1 - InitialCameraPosition).ToNormalized();
        StoreCam.Direction = _direction;
    }

    public void RotateCameraByMouse()
    {
        CameraRotator cameraRotator = new CameraRotator(StoreCam, Store.VehiclePreviewCameraPosition);
        cameraRotator.RotateCameraByMouse();
    }

    public void AutoInterior(Vector3 focusPosition, float focusHeading, bool wait, bool setHomePosition)
    {
        if (!StoreCam.Exists())
        {
            StoreCam = new Camera(false);
        }

        if (Camera.RenderingCamera != null)
        {
            StoreCam.Position = Camera.RenderingCamera.Position;
            StoreCam.FOV = Camera.RenderingCamera.FOV;
            StoreCam.Rotation = Camera.RenderingCamera.Rotation;
        }
        else
        {
            StoreCam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
            StoreCam.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
            Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
            StoreCam.Rotation = new Rotator(r.X, r.Y, r.Z);
            StoreCam.Active = true;
        }




        float distanceX = Settings.SettingsManager.PlayerOtherSettings.InteriorAutoCameraXDistance;// 5f;
        float distanceAway = Settings.SettingsManager.PlayerOtherSettings.InteriorAutoCameraYDistance;// 5f;
        float distanceAbove = Settings.SettingsManager.PlayerOtherSettings.InteriorAutoCameraZDistance;// 3f;

        float HeadingOffset = Settings.SettingsManager.PlayerOtherSettings.InteriorAutoCameraHeadingOffset;// 3f;

        Vector3 InitialCameraPosition = NativeHelper.GetOffsetPosition(focusPosition, focusHeading + HeadingOffset, distanceAway);
        InitialCameraPosition = NativeHelper.GetOffsetPosition(InitialCameraPosition, focusHeading, distanceX);
        InitialCameraPosition = new Vector3(InitialCameraPosition.X, InitialCameraPosition.Y, InitialCameraPosition.Z + distanceAbove);
        Vector3 ToLookAt1 = new Vector3(focusPosition.X, focusPosition.Y, focusPosition.Z);
        _direction = (ToLookAt1 - InitialCameraPosition).ToNormalized();


        if (!CameraTo.Exists())
        {
            CameraTo = new Camera(false);
        }
        CameraTo.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
        CameraTo.Position = InitialCameraPosition;
        CameraTo.Rotation = StoreCam.Rotation;// desiredRotation;
        CameraTo.Direction = _direction;
        CameraTo.Active = true;



        if (setHomePosition)
        {
            HomePosition = InitialCameraPosition;
            HomeRotator = StoreCam.Rotation;
            HomeDirection = _direction;
        }


        NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(CameraTo, StoreCam, 1500, true, true);
        if (wait)
        {
            GameFiber.Sleep(1500);
        }
    }

    public void RehighlightOriginalPosition()
    {
        Vector3 CurrentPosition = Vector3.Zero;
        if (StoreCam.Exists())
        {
            CurrentPosition = StoreCam.Position;
        }
        if (CurrentPosition.DistanceTo(HomePosition) <= 100f)
        {
            TransitionFromLocation2();
        }
        else
        {
            SetCamHome2();
        }
    }

    private void TransitionFromLocation2()
    {
        EntryPoint.WriteToConsole("TRANSITION FROM LOCATION 2 RAN");
        if (!StoreCam.Exists())
        {
            StoreCam = new Camera(false);
        }

        //NativeFunction.Natives.CLEAR_FOCUS();



        StoreCam.Position = HomePosition;
        StoreCam.Rotation = HomeRotator;
        StoreCam.Direction = HomeDirection;
        StoreCam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();


        if (!CameraTo.Exists())
        {
            CameraTo = new Camera(false);
        }
        if (Camera.RenderingCamera != null)
        {
            CameraTo.Position = Camera.RenderingCamera.Position;
            CameraTo.FOV = Camera.RenderingCamera.FOV;
            CameraTo.Rotation = Camera.RenderingCamera.Rotation;
        }
        else
        {
            CameraTo.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
            CameraTo.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
            Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
            CameraTo.Rotation = new Rotator(r.X, r.Y, r.Z);
            CameraTo.Active = true;
        }
        CameraTo.Active = true;
        NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(StoreCam, CameraTo, 1500, true, true);
        GameFiber.Sleep(1500);
        //NativeFunction.Natives.CLEAR_FOCUS();
        CurrentFocusPosition = Vector3.Zero;
        isHighlightingLocation = false;
    }


    private void SetCamHome2()
    {
        EntryPoint.WriteToConsole("SET CAM HOME 2 RAN");
        if (!StoreCam.Exists())
        {
            StoreCam = new Camera(false);
        }
        Game.FadeScreenOut(500, true);


        StoreCam.Position = HomePosition;
        StoreCam.Rotation = HomeRotator;
        StoreCam.Direction = HomeDirection;
        StoreCam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();



        NativeFunction.Natives.SET_FOCUS_POS_AND_VEL(StoreCam.Position.X, StoreCam.Position.Y, StoreCam.Position.Z, 0f, 0f, 0f);
        StoreCam.Active = true;
        isHighlightingLocation = false;
        GameFiber.Sleep(500);
        Game.FadeScreenIn(500, true);
        CurrentFocusPosition = Vector3.Zero;


        
    }


}

