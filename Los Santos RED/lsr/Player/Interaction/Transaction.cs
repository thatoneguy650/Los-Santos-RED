using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

public class Transaction : Interaction
{
    private bool IsUsingHintCamera = false;
    private bool IsUsingCustomCamera = false;

    private uint GameTimeStartedConversing;
    private bool IsActivelyConversing;
    private GameLocation Store;
    private IInteractionable Player;
    private ISettingsProvideable Settings;
    private MenuPool menuPool;
    private UIMenu ModItemMenu;
    private Camera StoreCam;
    private Camera EntranceCam;

    private Vector3 _direction;
    private Camera InterpolationCamera;
    private bool IsDisposed = false;
    private bool IsUsingCustomCam = false;
    private IModItems ModItems;
    private ITimeReportable Time;
    private IEntityProvideable World;
    private PurchaseMenu PurchaseMenu;
    private SellMenu SellMenu;
    private PedExt Ped;
    private bool IsTasked;
    private Vector3 EgressCamPosition;
    private float EgressCamFOV = 55f;

    private bool IsAnyMenuVisible => menuPool.IsAnyMenuOpen();//(ModItemMenu != null && ModItemMenu.Visible && ModItemMenu.MenuItems.Count() > 1) || (PurchaseMenu != null && PurchaseMenu.Visible) || (SellMenu != null && SellMenu.Visible);
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
    public Transaction(IInteractionable player, PedExt ped, GameLocation store, ISettingsProvideable settings, IModItems modItems, ITimeReportable time, IEntityProvideable world)
    {
        Ped = ped;
        Player = player;
        Store = store;
        Settings = settings;
        ModItems = modItems;
        Time = time;
        World = world;
        menuPool = new MenuPool();
    }
    public override string DebugString => "";
    private bool CanContinueConversation => (Store.EntrancePosition == Vector3.Zero || Player.Character.DistanceTo2D(Store.EntrancePosition) <= 6f) && Player.CanConverse;

    public void ClearPreviews()
    {
        PurchaseMenu?.ClearPreviews();
        SellMenu?.ClearPreviews();
    }

    public override void Start()
    {
        try
        {
            if (Store != null || Ped.HasMenu)
            {
                Player.IsConversing = true;
                Player.IsTransacting = true;
                GameFiber.StartNew(delegate
                {
                    Setup();
                    SetupCamera();
                    Greet();
                    ShowMenu();
                    Tick();
                    Dispose();
                }, "Transaction");
            }
            else
            {
                Dispose();
            }
        }
        catch (Exception ex)
        {
            Game.DisplayNotification(ex.Message);
            EntryPoint.WriteToConsole("SIMPLE TRANSACTION ERROR:" + ex.Message + ex.StackTrace, 0);
        }
    }
    public override void Dispose()
    {
        if (!IsDisposed)
        {
            Game.RawFrameRender -= (s, e) => menuPool.DrawBanners(e.Graphics);
            IsDisposed = true;
            if (ModItemMenu != null)
            {
                ModItemMenu.Visible = false;
            }
            PurchaseMenu?.Dispose();
            SellMenu?.Dispose();
            Player.ButtonPrompts.RemoveAll(x => x.Group == "Transaction");
            Player.IsConversing = false;
            Player.IsTransacting = false;
            if (IsUsingCustomCam)
            {


                //DoExitCam();



                //if (PurchaseMenu?.BoughtItem == true || SellMenu?.SoldItem == true)
                //{
                //    SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_THANKS", "GENERIC_BYE" }, true);
                //}
                //if (!Player.IsInVehicle)
                //{
                //    if(Store != null)
                //    {
                //        Game.LocalPlayer.Character.Position = Store.EntrancePosition;
                //        Game.LocalPlayer.Character.Heading = Store.EntranceHeading;
                //        if (!Store.IsWalkup)
                //        {
                //            Game.LocalPlayer.Character.IsVisible = true;
                //        }
                //    }
                //    Game.LocalPlayer.Character.Tasks.GoStraightToPosition(Game.LocalPlayer.Character.GetOffsetPositionFront(3f), 1.0f, Store.EntranceHeading, 1.0f, 1500);
                //}










                ReturnToGameplay();
                DoExitCam();
                if (!Player.IsInVehicle)
                {
                    EnableControl();
                }
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
            else if (IsUsingHintCamera)
            {
                NativeFunction.Natives.STOP_GAMEPLAY_HINT(true);
            }

            if(Ped != null && Ped.GetType() == typeof(Merchant))
            {
                if (Ped != null && Ped.Pedestrian.Exists() && IsTasked && Ped.Pedestrian.IsAlive && Store != null && Store.VendorHeading != 0f)
                {
                    NativeFunction.Natives.TASK_ACHIEVE_HEADING(Ped.Pedestrian, Store.VendorHeading, -1);
                    EntryPoint.WriteToConsole($"Transaction: DISPOSE Set Heading", 3);
                }
            }
            else
            {
                if (Ped != null && Ped.Pedestrian.Exists() && IsTasked)
                {
                    NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
                    EntryPoint.WriteToConsole($"Transaction: DISPOSE UnTasking", 3);
                }
            }
            EntryPoint.WriteToConsole($"Simple Transaction DISPOSE IsUsingCustomCam {IsUsingCustomCam}", 3);
        }
    }
    private void Setup()
    {
        if(Store == null)
        {
            Store = new GameLocation() { Name = "" };
            Store.Menu = Ped.TransactionMenu;
            ModItemMenu = new UIMenu("", "Transaction");
            ModItemMenu.RemoveBanner();
        }
        else
        {
            ModItemMenu = new UIMenu(Store.Name, Store.Description);
            if (Store.BannerImage != "")
            {
                ModItemMenu.SetBannerType(Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{Store.BannerImage}"));
                Game.RawFrameRender += (s, e) => menuPool.DrawBanners(e.Graphics);
            }
        }
        ModItemMenu.OnItemSelect += OnItemSelect;
        menuPool.Add(ModItemMenu);
        EntryPoint.WriteToConsole("Transaction: Setup Ran", 5);
    }
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem.Text == "Buy")
        {
            SellMenu?.Dispose();
            PurchaseMenu?.Show();
        }
        if (selectedItem.Text == "Sell")
        {
            PurchaseMenu?.Dispose();
            SellMenu?.Show();
        }
    }
    private void ShowMenu()
    {
        ModItemMenu.Clear();
        bool hasPurchaseMenu = false;
        bool hasSellMenu = false;
        if (Store.Menu.Any(x => x.Purchaseable))
        {
            PurchaseMenu = new PurchaseMenu(menuPool, ModItemMenu, Ped, Store, ModItems, Player, StoreCam, IsUsingCustomCam, World, Settings, this);
            PurchaseMenu.Setup();
            hasPurchaseMenu = true;
        }
        if (Store.Menu.Any(x => x.Sellable))
        {
            SellMenu = new SellMenu(menuPool, ModItemMenu, Ped, Store, ModItems, Player, StoreCam, IsUsingCustomCam, this);
            SellMenu.Setup();
            hasSellMenu = true;
        }
        if (hasSellMenu && hasPurchaseMenu)
        {
            ModItemMenu.Visible = true;
        }
        else if (hasSellMenu)
        {
            SellMenu.Show();
        }
        else
        {
            PurchaseMenu.Show();
        }
    }
    private void SetupCamera()
    {
        if(Ped != null && Ped.Pedestrian.Exists())
        {
            IsUsingHintCamera = true;
            IsUsingCustomCam = false;
            NativeFunction.Natives.SET_GAMEPLAY_PED_HINT(Ped.Pedestrian, 0f, 0f, 0f, true, -1, 2000, 2000);
        }
        else if (Player.IsInVehicle || Store.Type == LocationType.DriveThru)
        {
            IsUsingHintCamera = true;
            IsUsingCustomCam = false;
            NativeFunction.Natives.SET_GAMEPLAY_COORD_HINT(Store.EntrancePosition.X, Store.EntrancePosition.Y, Store.EntrancePosition.Z, -1, 2000, 2000);
        }
        else
        {
            IsUsingCustomCam = true;
            IsUsingHintCamera = false;
            DisableControl();



            DoEntryCam();




            if (Store.HasCustomItemPostion)
            {
                HighlightLocationWithCamera();
            }
            else
            {
                HighlightStoreWithCamera();
            }
            if(Store != null && !Store.IsWalkup)
            {
                Game.LocalPlayer.Character.IsVisible = false;
            }
        }
        EntryPoint.WriteToConsole("Transaction: Setup Camera Ran", 5);
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
    private void Tick()
    {
        while (CanContinueConversation)
        {
            menuPool.ProcessMenus();
            if (!IsActivelyConversing && !IsAnyMenuVisible)
            {
                Dispose();
            }
            if(ModItemMenu.MenuItems.Count() == 1 && ModItemMenu.Visible)
            {
                Dispose();
            }
            PurchaseMenu?.Update();
            SellMenu?.Update();
            GameFiber.Yield();
        }
        Dispose();
        GameFiber.Sleep(1000);
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
            Vector3 ToLookAt = new Vector3(Store.ItemPreviewPosition.X, Store.ItemPreviewPosition.Y, Store.ItemPreviewPosition.Z);
            _direction = (ToLookAt - Store.CameraPosition).ToNormalized();
            StoreCam.Direction = _direction;
            StoreCam.Active = true;
            GameFiber.Sleep(500);
            Game.FadeScreenIn(1500, true);
        }
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
            if (Store.Type == LocationType.Hotel)
            {
                distanceAway = 30f;
                distanceAbove = 20f;
            }
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
        if (Store.HasCustomItemPostion)
        {
            Game.FadeScreenOut(1500, true);
            StoreCam.Active = false;
            NativeFunction.Natives.CLEAR_FOCUS();
            //GameFiber.Sleep(500);
            Game.FadeScreenIn(1500, false);//was true   
        }
        else
        {
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
    }
    private bool CanSay(Ped ToSpeak, string Speech)
    {
        bool CanSay = NativeFunction.CallByHash<bool>(0x49B99BF3FDA89A7A, ToSpeak, Speech, 0);
        return CanSay;
    }
    private bool SayAvailableAmbient(Ped ToSpeak, List<string> Possibilities, bool WaitForComplete)
    {
        bool Spoke = false;
        if (CanContinueConversation)
        {
            foreach (string AmbientSpeech in Possibilities)
            {
                ToSpeak.PlayAmbientSpeech(null, AmbientSpeech, 0, SpeechModifier.Force);
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
            while (ToSpeak.Exists() && ToSpeak.IsAnySpeechPlaying && WaitForComplete && CanContinueConversation)
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
    private void Greet()
    {
        if(Ped != null && Ped.Pedestrian.Exists())
        {
            IsActivelyConversing = true;
            GameTimeStartedConversing = Game.GameTime;
            IsActivelyConversing = true;
            if (Ped.TimesInsultedByPlayer <= 0)
            {
                SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_HOWS_IT_GOING", "GENERIC_HI" }, false);
            }
            else
            {
                SayAvailableAmbient(Player.Character, new List<string>() { "PROVOKE_GENERIC", "GENERIC_WHATEVER" }, false);
            }
            while (CanContinueConversation && Game.GameTime - GameTimeStartedConversing <= 1000)
            {
                GameFiber.Yield();
            }
            if (!CanContinueConversation)
            {
                return;
            }

            if (!Ped.IsFedUpWithPlayer)
            {
                //if (NativeFunction.CallByName<bool>("IS_PED_USING_ANY_SCENARIO", Ped.Pedestrian))
                //{
                //    IsTasked = false;
                //}
                //else
                //{
                    IsTasked = true;
                    unsafe
                    {
                        int lol = 0;
                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Player.Character, 2000);
                        NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Player.Character, -1, 0, 2);
                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    }
              //  }



                if (Player.IsInVehicle)
                {
                    NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", Player.Character, Ped.Pedestrian, -1, 0, 2);
                }
                else
                {
                    unsafe
                    {
                        int lol = 0;
                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Ped.Pedestrian, 2000);
                        NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Ped.Pedestrian, -1, 0, 2);
                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    }
                }
                uint GameTimeStartedFacing = Game.GameTime;
                while (CanContinueConversation && Game.GameTime - GameTimeStartedFacing <= 500)
                {
                    GameFiber.Yield();
                }
                if (!CanContinueConversation)
                {
                    return;
                }
                if (Ped.TimesInsultedByPlayer <= 0)
                {
                    SayAvailableAmbient(Ped.Pedestrian, new List<string>() { "GENERIC_HOWS_IT_GOING", "GENERIC_HI" }, true);
                }
                else
                {
                    SayAvailableAmbient(Ped.Pedestrian, new List<string>() { "GENERIC_WHATEVER" }, true);
                }
                Ped.HasSpokenWithPlayer = true;
            }
            IsActivelyConversing = false;
        }
        else if (IsUsingHintCamera)// || Store.ItemPreviewPosition.DistanceTo2D(Store.EntrancePosition) <= 30f)
        {
            GameTimeStartedConversing = Game.GameTime;
            IsActivelyConversing = true;
            SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_HOWS_IT_GOING", "GENERIC_HI" }, false);
            while (CanContinueConversation && Game.GameTime - GameTimeStartedConversing <= 1000)
            {
                GameFiber.Yield();
            }
            if (!CanContinueConversation)
            {
                return;
            }
            IsActivelyConversing = false;
        }
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
}