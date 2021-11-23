using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
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
    private UIMenu Menu;
    private Camera StoreCam;

    private Quaternion _lookRotation;
    private Vector3 _direction;
    private Camera InterpolationCamera;
    private bool IsDisposed = false;
    private bool IsUsingCustomCam = false;
    private IModItems ModItems;

    public SimpleTransaction(IInteractionable player, GameLocation store, ISettingsProvideable settings, IModItems modItems)
    {
        Player = player;
        Store = store;
        Settings = settings;
        ModItems = modItems;
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
                if (!Player.IsInVehicle)
                {
                    Game.LocalPlayer.Character.Position = Store.EntrancePosition;
                    Game.LocalPlayer.Character.Heading = Store.EntranceHeading;
                    Game.LocalPlayer.Character.IsVisible = true;
                    Game.LocalPlayer.HasControl = true;
                    Game.LocalPlayer.Character.Tasks.GoStraightToPosition(Game.LocalPlayer.Character.GetOffsetPositionFront(3f), 1.0f, Store.EntranceHeading, 1.0f, 1500);
                }
                ReturnToGameplay();
                GameFiber.Sleep(1500);
                InterpolationCamera.Active = false;
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
            Menu = new UIMenu(Store.Name, Store.Description);
            Menu.OnItemSelect += OnItemSelect;
            menuPool.Add(Menu);
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
            }
            else
            {
                NativeFunction.Natives.SET_GAMEPLAY_COORD_HINT(Store.EntrancePosition.X, Store.EntrancePosition.Y, Store.EntrancePosition.Z, -1, 2000, 2000);
            }
            EntryPoint.WriteToConsole($"Simple Transaction START {IsUsingCustomCam}", 3);
            GameFiber.StartNew(delegate
            {
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
        Vector3 InitialCameraPosition = NativeHelper.GetOffsetPosition(Store.EntrancePosition, Store.EntranceHeading+90f, 10f);
        InitialCameraPosition = new Vector3(InitialCameraPosition.X, InitialCameraPosition.Y, InitialCameraPosition.Z + 7f);
        StoreCam.Position = InitialCameraPosition;
        StoreCam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
        Vector3 ToLookAt = new Vector3(Store.EntrancePosition.X, Store.EntrancePosition.Y, Store.EntrancePosition.Z + 2f);
        _direction = (ToLookAt - InitialCameraPosition).ToNormalized();
        StoreCam.Direction = _direction;
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
        Menu.Clear();
        foreach (MenuItem cii in Store.Menu)
        {
            if (cii != null)
            {
                Menu.AddItem(new UIMenuItem(cii.ModItemName, $"{cii.ModItemName} ${cii.Price}"));
            }
        }
    }
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        ModItem ToAdd = ModItems.Items.Where(x => x.Name == selectedItem.Text).FirstOrDefault();
        MenuItem menuItem = Store.Menu.Where(x => x.ModItemName == selectedItem.Text).FirstOrDefault();
        if (ToAdd != null && menuItem != null && Player.Money >= menuItem.Price)
        {
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
    }
    private void HideMenu()
    {
        Menu.Visible = false;
    }
    private void ShowMenu()
    {
        if (!Menu.Visible)
        {
            CreateTransactionMenu();
            Menu.Visible = true;
        }
    }
    private void Loop()
    {
        menuPool.ProcessMenus();
        if (!IsActivelyConversing && !Menu.Visible)
        {
            Dispose();
        }
    }
    private bool CanSay(Ped ToSpeak, string Speech)
    {
        bool CanSay = NativeFunction.CallByHash<bool>(0x49B99BF3FDA89A7A, ToSpeak, Speech, 0);
        return CanSay;
    }
    private void Buy()
    {
        HideMenu();
        IsActivelyConversing = true;
        Player.ButtonPrompts.Clear();
        SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_BUY", "GENERIC_YES", "BLOCKED_GENEIRC" }, true);
        SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_THANKS", "GENERIC_BYE" }, true);    
        IsActivelyConversing = false;
        ShowMenu();
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