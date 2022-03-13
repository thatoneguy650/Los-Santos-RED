using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PersonTransaction : Interaction
{
    private IActivityPerformable Player;
    private PedExt Ped;
    private ShopMenu ShopMenu;
    private Transaction Transaction;
    private MenuPool MenuPool;
    private UIMenu InteractionMenu;
   // private Texture BannerImage;

    private IModItems ModItems;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private IWeapons Weapons;
    private ITimeControllable Time;
    private bool IsDoingTransactionAnimation = false;


    private bool IsActivelyConversing = false;
    private uint GameTimeStartedConversing;
    private bool IsVendorTasked = false;
    private bool IsDisposed = false;
    private Rage.Object SellingProp;

    public InteractableLocation AssociatedStore { get; set; }


    private bool CanContinueConversation => Player.CanConverse;// ((AssociatedStore != null && AssociatedStore.HasVendor && Player.Character.DistanceTo2D(AssociatedStore.VendorPosition) <= 6f) && (Ped.Pedestrian.Exists() && Ped.Pedestrian.DistanceTo2D(Player.Character) <= 6f)) && Player.CanConverse && Ped.CanConverse;
    public override string DebugString => "";

    public PersonTransaction(IActivityPerformable player, PedExt ped, ShopMenu shopMenu, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        Player = player;
        Ped = ped;
        ShopMenu = shopMenu;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;
    }
    public override void Start()
    {

        if (ShopMenu == null)
        {
            EntryPoint.WriteToConsole("Shop Menu is null for some fucking reason !");

        }
        else
        {
            Player.IsConversing = false;
            Player.IsTransacting = false;
        }

        GameFiber.StartNew(delegate
        {
            CreateInteractionMenu();

            if (Ped != null && Ped.Pedestrian.Exists())
            {

                Player.IsConversing = true;
                Player.IsTransacting = true;

                AnimationDictionary.RequestAnimationDictionay("mp_safehousevagos@");
                AnimationDictionary.RequestAnimationDictionay("mp_common");

                NativeFunction.Natives.SET_GAMEPLAY_PED_HINT(Ped.Pedestrian, 0f, 0f, 0f, true, -1, 2000, 2000);



                Transaction = new Transaction(MenuPool, InteractionMenu, ShopMenu, AssociatedStore);
                Transaction.PreviewItems = false;
                Transaction.PersonTransaction = this;
                Transaction.CreateTransactionMenu(Player, ModItems, World, Settings, Weapons, Time);


                Greet();


                InteractionMenu.Visible = true;
                InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;

                


                //Transaction.ProcessTransactionMenu();
                while (MenuPool.IsAnyMenuOpen())
                {
                    MenuPool.ProcessMenus();
                    Transaction.PurchaseMenu?.Update();
                    Transaction.SellMenu?.Update();
                    GameFiber.Yield();
                }

            }
            Dispose();

        }, "PersonTransaction");
    }
    public override void Dispose()
    {
        EntryPoint.WriteToConsole($"PERSON TRANSACTION Dispose IsDisposed {IsDisposed}");
        if (!IsDisposed)
        {
            IsDisposed = true;
            Player.IsConversing = false;
            Player.IsTransacting = false;
            Transaction?.DisposeTransactionMenu();
            DisposeInteractionMenu();

            if (Ped != null && Ped.Pedestrian.Exists())
            {
                if (AssociatedStore != null && AssociatedStore.VendorHeading != 0f)
                {
                    Ped.Pedestrian.BlockPermanentEvents = false;
                    Ped.Pedestrian.KeepTasks = false;
                    NativeFunction.Natives.TASK_ACHIEVE_HEADING(Ped.Pedestrian, AssociatedStore.VendorHeading, -1);
                    EntryPoint.WriteToConsole($"PersonTransaction: DISPOSE Set Heading", 3);
                }
                else
                {
                    Ped.Pedestrian.BlockPermanentEvents = false;
                    Ped.Pedestrian.KeepTasks = false;
                    NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
                    EntryPoint.WriteToConsole($"PersonTransaction: DISPOSE UnTasking", 3);
                }
            }
            NativeFunction.Natives.STOP_GAMEPLAY_HINT(false);
        }
    }

    public void OnItemPurchased(ModItem modItem, MenuItem menuItem, int totalItems)
    {
        StartBuyAnimation(modItem, menuItem.IsIllicilt);
        if (Ped.GetType() == typeof(GangMember))
        {
            GangMember gm = (GangMember)Ped;
            Player.GangRelationships.ChangeReputation(gm.Gang, menuItem.PurchasePrice * totalItems, true);
        }
    }
    public void OnItemSold(ModItem modItem, MenuItem menuItem, int totalItems)
    {
        StartSellAnimation(modItem, menuItem.IsIllicilt);
        if (Ped.GetType() == typeof(GangMember))
        {
            GangMember gm = (GangMember)Ped;
            Player.GangRelationships.ChangeReputation(gm.Gang, menuItem.SalesPrice * totalItems, true);
        }
    }

    private void InteractionMenu_OnItemSelect(RAGENativeUI.UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem.Text == "Buy")
        {
            Transaction?.SellMenu?.Dispose();
            Transaction?.PurchaseMenu?.Show();
        }
        else if (selectedItem.Text == "Sell")
        {
            Transaction?.PurchaseMenu?.Dispose();
            Transaction?.SellMenu?.Show();
        }
    }
    public void CreateInteractionMenu()
    {
        MenuPool = new MenuPool();
        
        if (AssociatedStore != null && AssociatedStore.HasBannerImage)
        {
            InteractionMenu = new UIMenu(AssociatedStore.Name, AssociatedStore.Description);
            AssociatedStore.BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{AssociatedStore.BannerImagePath}");
            InteractionMenu.SetBannerType(AssociatedStore.BannerImage);
            Game.RawFrameRender += (s, e) => MenuPool.DrawBanners(e.Graphics);
        }
        else if (AssociatedStore == null || AssociatedStore.RemoveBanner)
        {
            InteractionMenu = new UIMenu("", "");
            InteractionMenu.RemoveBanner();
        }
        MenuPool.Add(InteractionMenu);
    }
    public void DisposeInteractionMenu()
    {
        Game.RawFrameRender -= (s, e) => MenuPool.DrawBanners(e.Graphics);
        if (InteractionMenu != null)
        {
            InteractionMenu.Visible = false;
        }

    }
    private bool SayAvailableAmbient(Ped ToSpeak, List<string> Possibilities, bool WaitForComplete)
    {
        bool Spoke = false;
        if (CanContinueConversation)
        {
            foreach (string AmbientSpeech in Possibilities)
            {
                if (ToSpeak.Handle == Player.Character.Handle)
                {
                    if (Player.CharacterModelIsFreeMode)
                    {
                        ToSpeak.PlayAmbientSpeech(Player.FreeModeVoice, AmbientSpeech, 0, SpeechModifier.Force);
                    }
                    else
                    {
                        ToSpeak.PlayAmbientSpeech(null, AmbientSpeech, 0, SpeechModifier.Force);
                    }
                }
                else
                {
                    if (Ped.VoiceName != "")
                    {
                        ToSpeak.PlayAmbientSpeech(Ped.VoiceName, AmbientSpeech, 0, SpeechModifier.Force);
                    }
                    else
                    {
                        ToSpeak.PlayAmbientSpeech(null, AmbientSpeech, 0, SpeechModifier.Force);
                    }
                }
                GameFiber.Sleep(300);
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
            uint GameTimeStartedWatiing = Game.GameTime;
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
        if (IsDisposed)
        {
            return;
        }
        if (Ped != null && Ped.Pedestrian.Exists())
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
                if (Ped.IsInVehicle)
                {
                    Ped.Pedestrian.BlockPermanentEvents = true;
                    Ped.Pedestrian.KeepTasks = true;
                    IsVendorTasked = true;
                    NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", Ped.Pedestrian, Player.Character, -1, 0, 2);
                }
                else
                {
                    IsVendorTasked = true;
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
    private void StartBuyAnimation(ModItem item, bool isIllicit)
    {
        //Hide();
        IsActivelyConversing = true;
        //if (hideShowMenu)
        //{
        //    Hide();
        //}
        string modelName = "";
        bool HasProp = false;
        bool isWeapon = false;
        if (item.PackageItem != null && item.PackageItem.ModelName != "")
        {
            modelName = item.PackageItem.ModelName;
            HasProp = true;
            if (item.PackageItem.Type == ePhysicalItemType.Weapon)
            {
                isWeapon = true;
            }
        }
        else if (item.ModelItem != null && item.ModelItem.ModelName != "")
        {
            modelName = item.ModelItem.ModelName;
            HasProp = true;
            if (item.ModelItem.Type == ePhysicalItemType.Weapon)
            {
                isWeapon = true;
            }
        }
        IsActivelyConversing = true;
        if (isIllicit)
        {
            if (isWeapon)
            {
                Player.IsDealingIllegalGuns = true;
                Ped.IsDealingIllegalGuns = true;
            }
            else
            {
                Player.IsDealingDrugs = true;
                Ped.IsDealingDrugs = true;
            }
        }
        Player.ButtonPrompts.Clear();
        SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_BUY", "GENERIC_YES", "BLOCKED_GENEIRC" }, true);
        if (Ped.Pedestrian.Exists())
        {
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "mp_common", "givetake1_a", 1.0f, -1.0f, 5000, 50, 0, false, false, false);
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "mp_common", "givetake1_b", 1.0f, -1.0f, 5000, 50, 0, false, false, false);
        }
        GameFiber.Sleep(500);
        if (!isWeapon && Ped.Pedestrian.Exists() && HasProp && modelName != "")
        {
            SellingProp = new Rage.Object(modelName, Player.Character.GetOffsetPositionUp(50f));
            GameFiber.Yield();
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
        if (isIllicit)
        {
            if (isWeapon)
            {
                Player.IsDealingIllegalGuns = false;
                Ped.IsDealingIllegalGuns = false;
            }
            else
            {
                Player.IsDealingDrugs = false;
                Ped.IsDealingDrugs = false;
            }
        }

        //Show();   
        //if (hideShowMenu)
        //{
        //    Show();
        //}
    }
    private void StartSellAnimation(ModItem item, bool isIllicit)
    {
        //Hide();
        string modelName = "";
        bool HasProp = false;
        bool isWeapon = false;
        if (item.PackageItem != null && item.PackageItem.ModelName != "")
        {
            modelName = item.PackageItem.ModelName;
            HasProp = true;
            if (item.PackageItem.Type == ePhysicalItemType.Weapon)
            {
                isWeapon = true;
            }
        }
        else if (item.ModelItem != null && item.ModelItem.ModelName != "")
        {
            modelName = item.ModelItem.ModelName;
            HasProp = true;
            if (item.ModelItem.Type == ePhysicalItemType.Weapon)
            {
                isWeapon = true;
            }
        }
        IsActivelyConversing = true;
        if (isIllicit)
        {
            if (isWeapon)
            {
                Player.IsDealingIllegalGuns = true;
                Ped.IsDealingIllegalGuns = true;
            }
            else
            {
                Player.IsDealingDrugs = true;
                Ped.IsDealingDrugs = true;
            }
        }
        Player.ButtonPrompts.Clear();
        SayAvailableAmbient(Ped.Pedestrian, new List<string>() { "GENERIC_BUY", "GENERIC_YES", "BLOCKED_GENEIRC" }, true);
        if (Ped.Pedestrian.Exists())
        {
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "mp_common", "givetake1_b", 1.0f, -1.0f, 5000, 50, 0, false, false, false);
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "mp_common", "givetake1_a", 1.0f, -1.0f, 5000, 50, 0, false, false, false);
        }
        GameFiber.Sleep(500);

        if (!isWeapon && Ped.Pedestrian.Exists() && HasProp && modelName != "")
        {
            SellingProp = new Rage.Object(modelName, Player.Character.GetOffsetPositionUp(50f));
            GameFiber.Yield();
            if (SellingProp.Exists())
            {
                SellingProp.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, item.ModelItem.AttachBoneIndex), item.ModelItem.AttachOffset, item.ModelItem.AttachRotation);
            }
        }
        GameFiber.Sleep(500);
        if (Ped.Pedestrian.Exists())
        {
            if (SellingProp.Exists())
            {
                SellingProp.AttachTo(Ped.Pedestrian, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Ped.Pedestrian, item.ModelItem.AttachBoneIndex), item.ModelItem.AttachOffset, item.ModelItem.AttachRotation);
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
            //SayAvailableAmbient(Ped.Pedestrian, new List<string>() { "GENERIC_BYE", "GENERIC_THANKS", "PED_RANT" }, true);
        }
        IsActivelyConversing = false;
        if (isIllicit)
        {
            if (isWeapon)
            {
                Player.IsDealingIllegalGuns = false;
                Ped.IsDealingIllegalGuns = false;
            }
            else
            {
                Player.IsDealingDrugs = false;
                Ped.IsDealingDrugs = false;
            }
        }
        //Show();     
    }
}

