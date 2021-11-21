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
    public SimpleTransaction(IInteractionable player, GameLocation store, ISettingsProvideable settings)
    {
        Player = player;
        Store = store;
        Settings = settings;
        menuPool = new MenuPool();
    }
    public override string DebugString => "";
    private bool CanContinueConversation => Player.Character.DistanceTo2D(Store.VendorPosition) <= 6f && Player.CanConverse;
    public override void Dispose()
    {
        HideMenu();
        Player.ButtonPrompts.RemoveAll(x => x.Group == "Transaction");
        Player.IsConversing = false;
        NativeFunction.Natives.STOP_GAMEPLAY_HINT(true);
    }
    public override void Start()
    {
        if (Store != null)
        {
            Menu = new UIMenu(Store.Name, Store.Description);
            Menu.OnItemSelect += OnItemSelect;
            menuPool.Add(Menu);

            Player.IsConversing = true;


            
            NativeFunction.Natives.SET_GAMEPLAY_COORD_HINT(Store.VendorPosition.X, Store.VendorPosition.Y, Store.VendorPosition.Z, -1, 2000, 2000);
            //NativeFunction.Natives.SET_GAMEPLAY_PED_HINT(0, Store.VendorPosition.X, Store.VendorPosition.Y, Store.VendorPosition.Z + 2f, true, -1, 2000, 2000);
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
        foreach (ConsumableSubstance cii in Store.SellableItems)
        {
            if (cii != null)
            {
                Menu.AddItem(new UIMenuItem(cii.Name, $"{cii.Name} ${cii.Price}"));
            }
        }

    }
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        ConsumableSubstance ToAdd = Store.SellableItems.Where(x => x != null && x.Name == selectedItem.Text).FirstOrDefault();
        if (ToAdd != null && Player.Money >= ToAdd.Price)
        {
            Buy(ToAdd);
            Player.AddToInventory(ToAdd, ToAdd.AmountPerPackage);
            EntryPoint.WriteToConsole($"ADDED {ToAdd.Name} {ToAdd.Type}  Amount: {ToAdd.AmountPerPackage}", 5);
            Player.GiveMoney(-1 * ToAdd.Price);
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
    private void Buy(ConsumableSubstance item)
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