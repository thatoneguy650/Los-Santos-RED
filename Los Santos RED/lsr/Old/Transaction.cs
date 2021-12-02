using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

public class Transaction : Interaction
{
    private uint GameTimeStartedConversing;
    private bool IsActivelyConversing;
    private bool IsTasked;
    private Merchant Ped;
    private IInteractionable Player;
    private bool CancelledConversation;
    private ISettingsProvideable Settings;
    private Rage.Object SellingProp;
    private MenuPool menuPool;
    private UIMenu Menu;
    private GameLocation Store;
    private IModItems ModItems;
    private bool IsDisposed = false;
    public Transaction(IInteractionable player, Merchant ped, GameLocation store, ISettingsProvideable settings, IModItems modItems)
    {
        Player = player;
        Ped = ped;
        Store = store;
        Settings = settings;
        ModItems = modItems;
        menuPool = new MenuPool();
    }
    public override string DebugString => $"TimesInsultedByPlayer {Ped.TimesInsultedByPlayer} FedUp {Ped.IsFedUpWithPlayer}";
    private bool CanContinueConversation => Player.Character.DistanceTo2D(Ped.Pedestrian) <= 6f && Ped.CanConverse && Player.CanConverse;
    public override void Dispose()
    {
        if (!IsDisposed)
        {
            Game.RawFrameRender -= (s, e) => menuPool.DrawBanners(e.Graphics);
            IsDisposed = true;
            HideMenu();
            Player.ButtonPrompts.RemoveAll(x => x.Group == "Transaction");
            Player.IsConversing = false;
            if (Ped != null && Ped.Pedestrian.Exists() && IsTasked && Ped.Pedestrian.IsAlive)
            {
                NativeFunction.Natives.TASK_ACHIEVE_HEADING(Ped.Pedestrian, Ped.Store.VendorHeading, -1);
            }
            NativeFunction.Natives.STOP_GAMEPLAY_HINT(true);
        }
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            Menu = new UIMenu(Ped.Store.Name, Ped.Store.Description);

            if (Store.BannerImage != "")
            {
                Menu.SetBannerType(Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{Store.BannerImage}"));
                Game.RawFrameRender += (s, e) => menuPool.DrawBanners(e.Graphics);
            }


            Menu.OnItemSelect += OnItemSelect;
            menuPool.Add(Menu);

            Player.IsConversing = true;
            NativeFunction.Natives.SET_GAMEPLAY_PED_HINT(Ped.Pedestrian, 0f, 0f, 0f, true, -1, 2000, 2000);
            AnimationDictionary.RequestAnimationDictionay("mp_safehousevagos@");
            AnimationDictionary.RequestAnimationDictionay("mp_common");
            EntryPoint.WriteToConsole($"Transaction START", 3);
            GameFiber.StartNew(delegate
            {
                Greet();
                ShowMenu();
                Tick();
                Dispose();
            }, "Transaction");
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
    private void CreateTransactionMenu()
    {
        Menu.Clear();
        foreach (MenuItem cii in Store.Menu)
        {
            if (cii != null)
            {
                Menu.AddItem(new UIMenuItem(cii.ModItemName, $"{cii.ModItemName} ${cii.PurchasePrice}"));
            }
        }
    }
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        ModItem ToAdd = ModItems.Items.Where(x => x != null && x.Name == selectedItem.Text).FirstOrDefault();
        MenuItem menuItem = Store.Menu.Where(x => x != null && x.ModItemName == selectedItem.Text).FirstOrDefault();
        if (ToAdd != null && menuItem != null && Player.Money >= menuItem.PurchasePrice)
        {
            StartBuyAnimation(ToAdd);
            if (ToAdd.Type == eConsumableType.Service)
            {
                Player.StartServiceActivity(ToAdd, Store);
            }
            else if (ToAdd.CanConsume)
            {
                Player.AddToInventory(ToAdd, ToAdd.AmountPerPackage);
                EntryPoint.WriteToConsole($"ADDED {ToAdd.Name} {ToAdd.Type}  Amount: {ToAdd.AmountPerPackage}", 5);
            }
            Player.GiveMoney(-1 * menuItem.PurchasePrice);
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
        if(!IsActivelyConversing && !Menu.Visible)
        {
            Dispose();
        }
    }
    private bool CanSay(Ped ToSpeak, string Speech)
    {
        bool CanSay = NativeFunction.CallByHash<bool>(0x49B99BF3FDA89A7A, ToSpeak, Speech, 0);
        return CanSay;
    }
    private void StartBuyAnimation(ModItem item)
    {
        HideMenu();
        IsActivelyConversing = true;
        Player.ButtonPrompts.Clear();
        SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_BUY","GENERIC_YES","BLOCKED_GENEIRC" }, true);
        if (Ped.Pedestrian.Exists())
        {
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "mp_common", "givetake1_a", 1.0f, -1.0f, 5000, 50, 0, false, false, false);
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "mp_common", "givetake1_b", 1.0f, -1.0f, 5000, 50, 0, false, false, false);
        }
        GameFiber.Sleep(500);
        string modelName = item.PackageItem?.ModelName;
        if(modelName == "")
        {
            modelName = item.ModelItem?.ModelName;
        }
        if (Ped.Pedestrian.Exists() && modelName != "")
        {
            SellingProp = new Rage.Object(modelName, Player.Character.GetOffsetPositionUp(50f));
            if (SellingProp.Exists())
            {
                SellingProp.AttachTo(Ped.Pedestrian, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Ped.Pedestrian, item.ModelItem.AttachBoneIndex), item.ModelItem.AttachOffset, item.ModelItem.AttachRotation);
            }
        }
        GameFiber.Sleep(500);
        if (Ped.Pedestrian.Exists())
        {
            if (SellingProp.Exists())
            {
                SellingProp.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, item.ModelItem.AttachBoneIndex), item.ModelItem.AttachOffset, item.ModelItem.AttachRotation);
            }
        }
        GameFiber.Sleep(1000);
        if (Ped.Pedestrian.Exists())
        {
            if (SellingProp.Exists())
            {
                SellingProp.Delete();
            }
            SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_THANKS", "GENERIC_BYE" }, true);
            SayAvailableAmbient(Ped.Pedestrian, new List<string>() { "GENERIC_BYE", "GENERIC_THANKS", "PED_RANT" }, true);       
        }
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
               // Game.DisplayNotification($"\"{Possibilities.FirstOrDefault()}\"");
            }
        }
        return Spoke;
    }
    private void Greet()
    {
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
            if (NativeFunction.CallByName<bool>("IS_PED_USING_ANY_SCENARIO", Ped.Pedestrian))
            {
                IsTasked = false;
            }
            else
            {
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
            }
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

}