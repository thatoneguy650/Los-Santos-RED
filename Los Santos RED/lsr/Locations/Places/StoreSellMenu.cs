using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

public class StoreSellMenu : Menu
{
    private UIMenu sellMenu;
    private IModItems ModItems;
    private GameLocation Store;
    private MenuPool MenuPool;
    private IInteractionable Player;
    private int ItemsSold;
    private Vehicle SellingVehicle;
    private Rage.Object SellingProp;
    private Ped SellingPed;
    private Camera StoreCam;
    private bool ShouldPreviewItem;
    private PedExt Ped;
    private bool IsActivelyConversing;
    private Transaction Transaction;
    private VehicleExt ToSellVehicle;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    public bool Visible => sellMenu.Visible;
    public bool SoldItem => ItemsSold > 0;
    private bool CanContinueConversation => Ped != null && Ped.Pedestrian.Exists() && Player.Character.DistanceTo2D(Ped.Pedestrian) <= 6f && Ped.CanConverse && Player.CanConverse;
    public StoreSellMenu(MenuPool menuPool, UIMenu parentMenu, PedExt ped, GameLocation store, IModItems modItems, IInteractionable player, Camera storeCamera, bool shouldPreviewItem, Transaction transaction, IEntityProvideable world, ISettingsProvideable settings)
    {
        Ped = ped;
        ModItems = modItems;
        Store = store;
        Player = player;
        StoreCam = storeCamera;
        ShouldPreviewItem = shouldPreviewItem;
        Transaction = transaction;
        World = world;
        Settings = settings;
        sellMenu = menuPool.AddSubMenu(parentMenu, "Sell");
        if (Transaction.HasBannerImage)
        {
            sellMenu.SetBannerType(Transaction.BannerImage);
        }
        else if (Transaction.RemoveBanner)
        {
            sellMenu.RemoveBanner();
        }
        sellMenu.OnIndexChange += OnIndexChange;
        sellMenu.OnItemSelect += OnItemSelect;
        sellMenu.OnListChange += OnListChange;
    }
    public void Setup()
    {
        if (Settings.SettingsManager.PlayerOtherSettings.GenerateStoreItemPreviews)
        {
            PreloadModels();
        }
        Transaction.ClearPreviews();
        if (Ped != null)
        {
            AnimationDictionary.RequestAnimationDictionay("mp_safehousevagos@");
            AnimationDictionary.RequestAnimationDictionay("mp_common");
        }
    }
    public void Dispose()
    {
        Hide();
        Game.RawFrameRender -= (s, e) => MenuPool.DrawBanners(e.Graphics);
    }
    public void Update()
    {
        if (sellMenu.Visible)
        {
            if (SellingProp.Exists())
            {
                SellingProp.SetRotationYaw(SellingProp.Rotation.Yaw + 1f);
            }
            if (SellingVehicle.Exists())
            {
                SellingVehicle.SetRotationYaw(SellingVehicle.Rotation.Yaw + 1f);
            }
        }
        else
        {
            ClearPreviews();
        }
    }
    public override void Hide()
    {
        ClearPreviews();
        sellMenu.Visible = false;
        Player.ButtonPrompts.Clear();
    }
    public override void Show()
    {
        CreateSellMenu();
        sellMenu.Visible = true;
    }
    public override void Toggle()
    {
        if (!sellMenu.Visible)
        {
            CreateSellMenu();
            sellMenu.Visible = true;
        }
        else
        {
            Hide();
            sellMenu.Visible = false;
        }
    }
    private void CreateSellMenu()
    {
        sellMenu.Clear();
        ToSellVehicle = null;
        foreach (MenuItem cii in Store.Menu)
        {
            if (cii != null && cii.Sellable)
            {
                ModItem myItem = ModItems.Get(cii.ModItemName);
                if (myItem != null)
                {
                    string formattedSalesPrice = cii.SalesPrice.ToString("C0");
                    if (myItem.ModelItem?.Type == ePhysicalItemType.Vehicle)
                    {
                        sellMenu.AddItem(new UIMenuItem(cii.ModItemName, $"{cii.ModItemName} {formattedSalesPrice}") { RightLabel = formattedSalesPrice });
                    }
                    else
                    {
                        string description = myItem.Description;
                        if (description == "")
                        {
                            description = $"{cii.ModItemName} {formattedSalesPrice}";
                        }

                        bool enabled = Player.Inventory.HasItem(cii.ModItemName);
                        description += "~n~~s~";
                        if (!enabled && myItem.Type == eConsumableType.Service && Store.Type == LocationType.ScrapYard)
                        {
                            ToSellVehicle = World.GetClosestVehicleExt(Store.EntrancePosition, true, 15f);
                            if (ToSellVehicle != null)
                            {
                                enabled = true;
                                description += $"~n~Selected Vehicle: ~p~{ToSellVehicle.MakeName()} ~p~{ToSellVehicle.ModelName()}~s~";
                            }
                        }

                        description += $"~n~Type: ~p~{myItem.FormattedItemType}~s~";
                        UIMenuItem myMenuItem = new UIMenuItem(cii.ModItemName, description) { Enabled = enabled, RightLabel = formattedSalesPrice };
                        sellMenu.AddItem(myMenuItem);
                    }
                }
            }
        }
        OnIndexChange(sellMenu, sellMenu.CurrentSelection);
    }
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        ModItem ToAdd = ModItems.Items.Where(x => x.Name == selectedItem.Text).FirstOrDefault();
        MenuItem menuItem = Store.Menu.Where(x => x.ModItemName == selectedItem.Text).FirstOrDefault();
        bool ExitAfterPurchase = false;
        if (ToAdd != null && menuItem != null)
        {
            if (Ped != null && Ped.Pedestrian.Exists())
            {
                StartSellAnimation(ToAdd, menuItem.IsIllicilt);
            }
            else
            {
                Hide();
            }
            if (ToAdd.Type == eConsumableType.Service && Store.Type == LocationType.ScrapYard)
            {
                ExitAfterPurchase = true;
                Player.GiveMoney(menuItem.SalesPrice);
                ItemsSold++;

                ScrapVehicle();

            }
            else
            {
                if (ToAdd.CanConsume)
                {
                    if (Player.Inventory.Remove(ToAdd, 1))
                    {
                        Player.GiveMoney(menuItem.SalesPrice);
                        ItemsSold++;
                        EntryPoint.WriteToConsole($"REMOVED {ToAdd.Name} {ToAdd.GetType()}  Amount: {1}", 5);
                    }
                }
            }
        }
        GameFiber.Sleep(500);
        while (Player.IsPerformingActivity)
        {
            GameFiber.Sleep(500);
        }
        if (ExitAfterPurchase)
        {
            Dispose();
        }
        else
        {
            Show();
        }
    }
    private void OnIndexChange(UIMenu sender, int newIndex)
    {
        if (newIndex != -1)
        {
            CreatePreview(sender.MenuItems[newIndex]);
        }
    }
    private void OnListChange(UIMenu sender, UIMenuListItem listItem, int newIndex)
    {

    }
    private void CreatePreview(UIMenuItem myItem)
    {
        ClearPreviews();
        if (myItem != null && Settings.SettingsManager.PlayerOtherSettings.GenerateStoreItemPreviews)
        {
            EntryPoint.WriteToConsole($"SIMPLE TRANSACTION OnIndexChange Text: {myItem.Text}", 5);
            ModItem itemToShow = ModItems.Items.Where(x => x.Name == myItem.Text).FirstOrDefault();
            if (itemToShow != null && ShouldPreviewItem)
            {
                if (itemToShow.PackageItem?.Type == ePhysicalItemType.Prop || itemToShow.ModelItem?.Type == ePhysicalItemType.Prop)
                {
                    PreviewProp(itemToShow);
                }
                else if (itemToShow.ModelItem?.Type == ePhysicalItemType.Vehicle)
                {
                    PreviewVehicle(itemToShow);
                }
                else if (itemToShow.ModelItem?.Type == ePhysicalItemType.Ped)
                {
                    PreviewPed(itemToShow);
                }
            }
        }
    }
    private void PreviewPed(ModItem itemToShow)
    {
        if (itemToShow != null && itemToShow.ModelItem != null)
        {
            SellingVehicle = new Vehicle(itemToShow.ModelItem.ModelName, Store.ItemPreviewPosition, Store.ItemPreviewHeading);
        }
        GameFiber.Yield();
    }
    private void PreviewVehicle(ModItem itemToShow)
    {
        if (itemToShow != null && itemToShow.ModelItem != null)
        {
            SellingVehicle = new Vehicle(itemToShow.ModelItem.ModelName, Store.ItemPreviewPosition, Store.ItemPreviewHeading);
        }
        GameFiber.Yield();
        if (SellingVehicle.Exists())
        {

            //SellingVehicle.PrimaryColor = CurrentSelectedColor;
            //CurrentDisplayColor = CurrentSelectedColor;
            NativeFunction.Natives.SET_VEHICLE_ON_GROUND_PROPERLY<bool>(SellingVehicle, 5.0f);
        }
    }
    private void PreviewProp(ModItem itemToShow)
    {
        string ModelToSpawn = "";
        bool useClose = true;
        if (itemToShow.PackageItem != null)
        {
            ModelToSpawn = itemToShow.PackageItem.ModelName;
            useClose = !itemToShow.PackageItem.IsLarge;
        }
        if (ModelToSpawn == "")
        {
            ModelToSpawn = itemToShow.ModelItem.ModelName;
            useClose = !itemToShow.ModelItem.IsLarge;
        }
        if (ModelToSpawn != "" && NativeFunction.Natives.IS_MODEL_VALID<bool>(Game.GetHashKey(ModelToSpawn)))
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
                SellingProp.SetRotationYaw(SellingProp.Rotation.Yaw + 45f);
                if (SellingProp != null && SellingProp.Exists())
                {
                    NativeFunction.Natives.SET_ENTITY_HAS_GRAVITY(SellingProp, false);
                }
            }
            EntryPoint.WriteToConsole("SIMPLE TRANSACTION: PREVIEW ITEM RAN", 5);
        }
        else
        {
            if (SellingProp.Exists())
            {
                SellingProp.Delete();
            }
        }
    }




    public void ClearPreviews()
    {
        if (SellingProp.Exists())
        {
            SellingProp.Delete();
        }
        if (SellingVehicle.Exists())
        {
            SellingVehicle.Delete();
        }
        if (SellingPed.Exists())
        {
            SellingPed.Delete();
        }
    }
    private void PreloadModels()
    {
        foreach (MenuItem menuItem in Store.Menu)//preload all item models so it doesnt bog the menu down
        {
            try
            {
                if (menuItem.Sellable)
                {
                    ModItem myItem = ModItems.Items.Where(x => x.Name == menuItem.ModItemName).FirstOrDefault();
                    if (myItem != null)
                    {
                        if (myItem.ModelItem != null && myItem.ModelItem.Type == ePhysicalItemType.Vehicle)
                        {

                        }
                        else if (myItem.ModelItem != null && myItem.ModelItem.Type == ePhysicalItemType.Weapon)
                        {

                        }
                        else if (myItem.PackageItem != null && myItem.PackageItem.Type == ePhysicalItemType.Prop && myItem.PackageItem.ModelName != "")
                        {
                            new Model(myItem.PackageItem.ModelName).LoadAndWait();
                        }
                        else if (myItem.ModelItem != null && myItem.ModelItem.Type == ePhysicalItemType.Prop && myItem.ModelItem.ModelName != "")
                        {
                            new Model(myItem.ModelItem.ModelName).LoadAndWait();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Game.DisplayNotification($"Error Preloading Model {ex.Message} {ex.StackTrace}");
            }
        }
    }
    private void StartSellAnimation(ModItem item, bool isIllicit)
    {
        Hide();
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
    }
    private bool SayAvailableAmbient(Ped ToSpeak, List<string> Possibilities, bool WaitForComplete)
    {
        bool Spoke = false;
        if (CanContinueConversation)
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


    private void ScrapVehicle()
    {
        if (ToSellVehicle != null && ToSellVehicle.Vehicle.Exists())
        {
            ToSellVehicle.Vehicle.Delete();
        }
    }

}