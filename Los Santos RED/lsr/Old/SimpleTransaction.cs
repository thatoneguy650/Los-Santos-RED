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

public class SimpleTransaction : Interaction
{
    private bool IsUsingHintCamera = false;
    private bool IsUsingCustomCamera = false;

    private uint GameTimeStartedConversing;
    private bool IsActivelyConversing;
    private GameLocation Store;
    private IInteractionable Player;
    private bool CancelledConversation;
    private ISettingsProvideable Settings;
    private MenuPool menuPool;
    private UIMenu ModItemMenu;
    private Camera StoreCam;

    private Vector3 _direction;
    private Camera InterpolationCamera;
    private bool IsDisposed = false;
    private bool IsUsingCustomCam = false;
    private IModItems ModItems;
    private ITimeReportable Time;
    private IEntityProvideable World;
    private List<Color> Colors = new List<Color>() { Color.White, Color.Red, Color.Green, Color.Gray, Color.DarkGray, Color.Black, Color.Yellow,Color.LightBlue,Color.Navy,Color.Purple,Color.DarkViolet };

    private List<string> ColorString = new List<string>() { "White", "Red", "Green", "Gray", "DarkGray", "Black", "Yellow", "LightBlue", "Navy", "Purple", "DarkViolet" };

    private Color CurrentSelectedColor = Color.Black;
    private Color CurrentDisplayColor = Color.Black;
    private PurchaseMenu PurchaseMenu;
    private SellMenu SellMenu;
    private bool IsAnyMenuVisible => (ModItemMenu != null && ModItemMenu.Visible && ModItemMenu.MenuItems.Count() > 1) || (PurchaseMenu != null && PurchaseMenu.Visible) || (SellMenu != null && SellMenu.Visible);
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
    private enum PreviewType
    {
        None,
        Prop,
        Vehicle,
        Ped,
    }
    public SimpleTransaction(IInteractionable player, GameLocation store, ISettingsProvideable settings, IModItems modItems, ITimeReportable time, IEntityProvideable world)
    {
        Player = player;
        Store = store;
        Settings = settings;
        ModItems = modItems;
        Time = time;
        World = world;
        menuPool = new MenuPool();
    }
    public override string DebugString => "";
    private bool CanContinueConversation => Player.Character.DistanceTo2D(Store.EntrancePosition) <= 6f && Player.CanConverse;
    public override void Dispose()
    {
        if (!IsDisposed)
        {
            Game.RawFrameRender -= (s, e) => menuPool.DrawBanners(e.Graphics);
            IsDisposed = true;
            ModItemMenu.Visible = false;
            PurchaseMenu?.Dispose();
            SellMenu?.Dispose();
            Player.ButtonPrompts.RemoveAll(x => x.Group == "Transaction");
            Player.IsConversing = false;
            if (IsUsingCustomCam)
            {
                if (PurchaseMenu?.BoughtItem == true || SellMenu?.SoldItem == true)
                {
                    SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_THANKS", "GENERIC_BYE" }, true);
                }
                if (!Player.IsInVehicle)
                {
                    Game.LocalPlayer.Character.Position = Store.EntrancePosition;
                    Game.LocalPlayer.Character.Heading = Store.EntranceHeading;
                    Game.LocalPlayer.Character.IsVisible = true;
                    Game.LocalPlayer.Character.Tasks.GoStraightToPosition(Game.LocalPlayer.Character.GetOffsetPositionFront(3f), 1.0f, Store.EntranceHeading, 1.0f, 1500);
                }
                ReturnToGameplay();
                if (!Player.IsInVehicle)
                {
                    Game.LocalPlayer.HasControl = true;
                    NativeFunction.Natives.SET_PLAYER_CONTROL(Game.LocalPlayer, (int)eSetPlayerControlFlag.SPC_LEAVE_CAMERA_CONTROL_ON, true);
                }
                if (StoreCam.Exists())
                {
                    StoreCam.Delete();
                }
                if (InterpolationCamera.Exists())
                {
                    InterpolationCamera.Delete();
                }
                Game.LocalPlayer.Character.Tasks.Clear();
            }
            else if(IsUsingHintCamera)
            {
                NativeFunction.Natives.STOP_GAMEPLAY_HINT(true);
            }
            EntryPoint.WriteToConsole($"Simple Transaction DISPOSE IsUsingCustomCam {IsUsingCustomCam}", 3);
        }
    }
    public override void Start()
    {
        try
        {
            if (Store != null)
            {
                Player.IsConversing = true;      
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
        }
        catch(Exception ex)
        {
            Game.DisplayNotification(ex.Message);
            EntryPoint.WriteToConsole("SIMPLE TRANSACTION ERROR:" + ex.Message + ex.StackTrace, 0);
        }
    }
    private void Setup()
    {
        ModItemMenu = new UIMenu(Store.Name, Store.Description);
        if (Store.BannerImage != "")
        {
            ModItemMenu.SetBannerType(Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{Store.BannerImage}"));
            Game.RawFrameRender += (s, e) => menuPool.DrawBanners(e.Graphics);
        }
        ModItemMenu.OnItemSelect += OnItemSelect;
        menuPool.Add(ModItemMenu);
    }
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if(selectedItem.Text == "Buy")
        {
            PurchaseMenu?.Show();
        }
        if (selectedItem.Text == "Sell")
        {
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
            PurchaseMenu = new PurchaseMenu(menuPool, ModItemMenu, null, Store, ModItems, Player, StoreCam, IsUsingCustomCam);
            PurchaseMenu.Setup();
            hasPurchaseMenu = true;
        }
        if (Store.Menu.Any(x => x.Sellable))
        {
            SellMenu = new SellMenu(menuPool, ModItemMenu, null, Store, ModItems, Player, StoreCam, IsUsingCustomCam);
            SellMenu.Setup();
            hasSellMenu = true;
        }
        if (hasSellMenu && hasPurchaseMenu)
        {
            ModItemMenu.Visible = true;
        }
        else if(hasSellMenu)
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
        if (Player.IsInVehicle)
        {
            IsUsingHintCamera = true;
            IsUsingCustomCam = false;
            NativeFunction.Natives.SET_GAMEPLAY_COORD_HINT(Store.EntrancePosition.X, Store.EntrancePosition.Y, Store.EntrancePosition.Z, -1, 2000, 2000);
        }
        else
        {
            IsUsingCustomCam = true;
            IsUsingHintCamera = false;
            Game.LocalPlayer.HasControl = false;
            if (Store.HasCustomItemPostion)
            {
                HighlightLocationWithCamera();
            }
            else
            {
                HighlightStoreWithCamera();
            }
            
            Game.LocalPlayer.Character.IsVisible = false;
            NativeFunction.Natives.SET_PLAYER_CONTROL(Game.LocalPlayer, (int)eSetPlayerControlFlag.SPC_LEAVE_CAMERA_CONTROL_ON, false);
        }
    }
    private void Tick()
    {
        while (CanContinueConversation)
        {
            Loop();
            if (CancelledConversation)
            {
                Dispose();
                break;
            }
            GameFiber.Yield();
        }
        Dispose();
        GameFiber.Sleep(1000);
    }
    private void Loop()
    {
        menuPool.ProcessMenus();
        if (!IsActivelyConversing && !IsAnyMenuVisible)
        {
            Dispose();
        }
        PurchaseMenu?.Update();
        SellMenu?.Update();
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
            Game.FadeScreenOut(1500,true);
            NativeFunction.Natives.SET_FOCUS_POS_AND_VEL(Store.CameraPosition.X, Store.CameraPosition.Y, Store.CameraPosition.Z, 0f, 0f, 0f);
            Vector3 ToLookAt = new Vector3(Store.ItemPreviewPosition.X, Store.ItemPreviewPosition.Y, Store.ItemPreviewPosition.Z);
            _direction = (ToLookAt - Store.CameraPosition).ToNormalized();
            StoreCam.Direction = _direction;
            StoreCam.Active = true;
            GameFiber.Sleep(500);
            Game.FadeScreenIn(1500,true);
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
            Game.FadeScreenOut(1500,true);
            StoreCam.Active = false;
            NativeFunction.Natives.CLEAR_FOCUS();
            GameFiber.Sleep(500);
            Game.FadeScreenIn(1500,true);
        }
        else
        {
            if (!InterpolationCamera.Exists())
            {
                InterpolationCamera = new Camera(false);
            }
            InterpolationCamera.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
            InterpolationCamera.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
            Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
            InterpolationCamera.Rotation = new Rotator(r.X, r.Y, r.Z);
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
        if (IsUsingHintCamera || Store.ItemPreviewPosition.DistanceTo2D(Store.EntrancePosition) <= 30f)
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

}