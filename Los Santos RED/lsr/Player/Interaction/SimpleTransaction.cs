using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

public class SimpleTransaction : Interaction
{
    private uint GameTimeStartedConversing;
    private bool IsActivelyConversing;
    private bool IsTasked;
    private GameLocation Store;
    private IInteractionable Player;
    private bool CancelledConversation;
    private ISettingsProvideable Settings;
    private Rage.Object SellingProp;
    private MenuPool menuPool;
    private UIMenu ModItemMenu;
    private Camera StoreCam;

    private Quaternion _lookRotation;
    private Vector3 _direction;
    private Camera InterpolationCamera;
    private bool IsDisposed = false;
    private bool IsUsingCustomCam = false;
    private IModItems ModItems;
    private ITimeReportable Time;
    private int ItemsBought;
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
    public SimpleTransaction(IInteractionable player, GameLocation store, ISettingsProvideable settings, IModItems modItems, ITimeReportable time)
    {
        Player = player;
        Store = store;
        Settings = settings;
        ModItems = modItems;
        Time = time;
        menuPool = new MenuPool();
    }
    public override string DebugString => "";
    private bool CanContinueConversation => Player.Character.DistanceTo2D(Store.EntrancePosition) <= 6f && Player.CanConverse;
    public override void Dispose()
    {
        if (!IsDisposed)
        {
            IsDisposed = true;
            HideMenu();
            Player.ButtonPrompts.RemoveAll(x => x.Group == "Transaction");
            Player.IsConversing = false;

            if (IsUsingCustomCam)
            {
                if (ItemsBought > 0)
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
                GameFiber.Sleep(1500);
                InterpolationCamera.Active = false;
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
            else
            {
                NativeFunction.Natives.STOP_GAMEPLAY_HINT(true);
            }
            EntryPoint.WriteToConsole($"Simple Transaction DISPOSE IsUsingCustomCam {IsUsingCustomCam}", 3);
        }
    }
    public override void Start()
    {
        if (Store != null)
        {
            ModItemMenu = new UIMenu(Store.Name, Store.Description);
            ModItemMenu.OnIndexChange += OnIndexChange;
            ModItemMenu.OnItemSelect += OnItemSelect;
            menuPool.Add(ModItemMenu);
            Player.IsConversing = true;
            if(Player.IsInVehicle || Store.Type == LocationType.DriveThru)
            {
                IsUsingCustomCam = false;
            }
            else
            {
                IsUsingCustomCam = true;
            }
            if (IsUsingCustomCam)
            {
                HighlightStoreWithCamera();
                Game.LocalPlayer.HasControl = false;
                Game.LocalPlayer.Character.IsVisible = false;
                NativeFunction.Natives.SET_PLAYER_CONTROL(Game.LocalPlayer, (int)eSetPlayerControlFlag.SPC_LEAVE_CAMERA_CONTROL_ON, false);
            }
            else
            {
                NativeFunction.Natives.SET_GAMEPLAY_COORD_HINT(Store.EntrancePosition.X, Store.EntrancePosition.Y, Store.EntrancePosition.Z, -1, 2000, 2000);
            }
            EntryPoint.WriteToConsole($"Simple Transaction START {IsUsingCustomCam}", 3);
            GameFiber.StartNew(delegate
            {
                if(IsUsingCustomCam)
                {
                    GameFiber.Sleep(1500);
                }
                Greet();
                ShowMenu();
                Tick();
                Dispose();
            }, "Transaction");
        }
    }
    private void HighlightStoreWithCamera()
    {
        if (!StoreCam.Exists())
        {
            StoreCam = new Camera(false);
        }
        if(Store.HasCustomCamera)
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

    }
    private void ReturnToGameplay()
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
    }
    //private void HighlightStoreWithCamera()
    //{
    //    if (!StoreCam.Exists())
    //    {
    //        StoreCam = new Camera(false);
    //    }
    //    float distanceAway = 10f;
    //    float distanceAbove = 7f;
    //    if(Store.Type == LocationType.Hotel)
    //    {
    //        distanceAway = 30f;
    //        distanceAbove = 20f;
    //    }
    //    Vector3 InitialCameraPosition = NativeHelper.GetOffsetPosition(Store.EntrancePosition, Store.EntranceHeading+90f, distanceAway);
    //    InitialCameraPosition = new Vector3(InitialCameraPosition.X, InitialCameraPosition.Y, InitialCameraPosition.Z + distanceAbove);
    //    StoreCam.Position = InitialCameraPosition;
    //    StoreCam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
    //    Vector3 ToLookAt = new Vector3(Store.EntrancePosition.X, Store.EntrancePosition.Y, Store.EntrancePosition.Z + 2f);
    //    _direction = (ToLookAt - InitialCameraPosition).ToNormalized();
    //    StoreCam.Direction = _direction;
    //    if (!InterpolationCamera.Exists())
    //    {
    //        InterpolationCamera = new Camera(false);
    //    }
    //    InterpolationCamera.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
    //    InterpolationCamera.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
    //    Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
    //    InterpolationCamera.Rotation = new Rotator(r.X, r.Y, r.Z);
    //    InterpolationCamera.Active = true;
    //    NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(StoreCam, InterpolationCamera, 1500, true, true);

    //}
    //private void ReturnToGameplay()
    //{
    //    if (!InterpolationCamera.Exists())
    //    {
    //        InterpolationCamera = new Camera(false);
    //    }
    //    InterpolationCamera.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
    //    InterpolationCamera.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
    //    Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
    //    InterpolationCamera.Rotation = new Rotator(r.X, r.Y, r.Z);
    //    InterpolationCamera.Active = true;
    //    NativeFunction.Natives.SET_CAM_ACTIVE_WITH_INTERP(InterpolationCamera, StoreCam, 1500, true, true);
    //}
    private void Tick()
    {
        while (CanContinueConversation)
        {
            //CheckInput();
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
    private void CreateTransactionMenu()
    {
        ModItemMenu.Clear();
        foreach (MenuItem cii in Store.Menu)
        {
            if (cii != null)
            {
                ModItemMenu.AddItem(new UIMenuItem(cii.ModItemName, $"{cii.ModItemName} ${cii.Price}"));
            }
        }
        OnIndexChange(ModItemMenu, ModItemMenu.CurrentSelection);
    }
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        ModItem ToAdd = ModItems.Items.Where(x => x.Name == selectedItem.Text).FirstOrDefault();
        MenuItem menuItem = Store.Menu.Where(x => x.ModItemName == selectedItem.Text).FirstOrDefault();
        if (ToAdd != null && menuItem != null && Player.Money >= menuItem.Price)
        {
            HideMenu();
            ItemsBought++;
            Buy();
            if (ToAdd.Type != eConsumableType.Service)
            {
                Player.AddToInventory(ToAdd, ToAdd.AmountPerPackage);
                EntryPoint.WriteToConsole($"ADDED {ToAdd.Name} {ToAdd.Type}  Amount: {ToAdd.AmountPerPackage}", 5);
            }
            else if (ToAdd.Type == eConsumableType.Service)
            {
                Player.StartServiceActivity(ToAdd, Store);
            }
            Player.GiveMoney(-1 * menuItem.Price);
        }
        GameFiber.Sleep(500);
        while (Player.IsPerformingActivity)
        {
            GameFiber.Sleep(500);
        }
        ShowMenu();
    }
    private void OnIndexChange(UIMenu sender, int newIndex)
    {
        EntryPoint.WriteToConsole($"SIMPLE TRANSACTION OnIndexChange IncomingIndex {newIndex}", 5);
        if (SellingProp.Exists())
        {
            SellingProp.Delete();
        }
        UIMenuItem myItem = sender.MenuItems[newIndex];
        ModItem itemToShow = null;
        if (myItem != null)
        {
            EntryPoint.WriteToConsole($"SIMPLE TRANSACTION OnIndexChange Text: {myItem.Text}", 5);
            itemToShow = ModItems.Items.Where(x => x.Name == myItem.Text).FirstOrDefault();
        }
        if (itemToShow != null && itemToShow.PhysicalItem != null)
        {
            PreviewItem(itemToShow);
        }
    }
    private void PreviewItem(ModItem itemToShow)
    {
        try
        {
            string ModelToSpawn = itemToShow.PhysicalItem.PackageModelName;
            bool useClose = !itemToShow.PhysicalItem.PackageIsLarge;
            if (ModelToSpawn == "")
            {
                ModelToSpawn = itemToShow.PhysicalItem.ModelName;
                useClose = !itemToShow.PhysicalItem.ItemIsLarge;
            }
            if (ModelToSpawn != "")
            {
                if (useClose)
                {
                    SellingProp = new Rage.Object(ModelToSpawn, StoreCam.Position + StoreCam.Direction);
                }
                else
                {
                    SellingProp = new Rage.Object(ModelToSpawn, StoreCam.Position + (StoreCam.Direction.ToNormalized() * 3f));
                }
                if (SellingProp.Exists())
                {
                    SellingProp.SetRotationYaw(SellingProp.Rotation.Yaw - 45f);
                    if (SellingProp != null && SellingProp.Exists())
                    {
                        NativeFunction.Natives.SET_ENTITY_HAS_GRAVITY(SellingProp, false);


                        //SellingProp.IsGravityDisabled = true;
                    }
                }
            }
        }
        catch(Exception ex)
        {
            Game.DisplayNotification("Error Displaying Item");
            EntryPoint.WriteToConsole(ex.Message + ";" + ex.StackTrace, 0);
        }
    }
    private void HideMenu()
    {
        if (SellingProp.Exists())
        {
            SellingProp.Delete();
        }
        ModItemMenu.Visible = false;
    }
    private void ShowMenu()
    {
        if (!ModItemMenu.Visible)
        {
            CreateTransactionMenu();
            ModItemMenu.Visible = true;
        }
    }
    private void Loop()
    {
        menuPool.ProcessMenus();
        if (!IsActivelyConversing && !ModItemMenu.Visible)
        {
            Dispose();
        }
        if(SellingProp.Exists())
        {
            SellingProp.SetRotationYaw(SellingProp.Rotation.Yaw + 1f);
        }
    }
    private bool CanSay(Ped ToSpeak, string Speech)
    {
        bool CanSay = NativeFunction.CallByHash<bool>(0x49B99BF3FDA89A7A, ToSpeak, Speech, 0);
        return CanSay;
    }
    private void Buy()
    {     
        IsActivelyConversing = true;
        Player.ButtonPrompts.Clear();
        //SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_BUY", "GENERIC_YES", "BLOCKED_GENEIRC" }, true);
        //SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_THANKS", "GENERIC_BYE" }, true);    
        IsActivelyConversing = false;
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
                Game.DisplayNotification($"\"{Possibilities.FirstOrDefault()}\"");
            }
        }
        return Spoke;
    }
    private void Greet()
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