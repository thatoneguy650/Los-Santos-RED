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
    private MenuPool MenuPool;
    private IActivityPerformable Player;
    private int ItemsSold;
    private Vehicle SellingVehicle;
    private Rage.Object SellingProp;
    private Ped SellingPed;
    private Camera StoreCam;
    private bool ShouldPreviewItem;
    //private TransactionOld Transaction;
    private VehicleExt ToSellVehicle;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private TransactableLocation Store;
    public bool Visible => sellMenu.Visible;
    public bool SoldItem => ItemsSold > 0;
    public StoreSellMenu(MenuPool menuPool, UIMenu parentMenu, TransactableLocation store, IModItems modItems, IActivityPerformable player, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        ModItems = modItems;
        Player = player;
        World = world;
        Settings = settings;
        MenuPool = menuPool;
        Store = store;
        StoreCam = Camera.RenderingCamera;
        sellMenu = menuPool.AddSubMenu(parentMenu, "Sell");
        if (Store.HasBannerImage)
        {
            sellMenu.SetBannerType(Store.BannerImage);
        }
        else if (Store.RemoveBanner)
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
        Store.ClearPreviews();
        //Transaction.ClearPreviews();
        //if (Ped != null)
        //{
        //    AnimationDictionary.RequestAnimationDictionay("mp_safehousevagos@");
        //    AnimationDictionary.RequestAnimationDictionay("mp_common");
        //}
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
        foreach (MenuItem cii in Store.Menu.Items)
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
        MenuItem menuItem = Store.Menu.Items.Where(x => x.ModItemName == selectedItem.Text).FirstOrDefault();
        bool ExitAfterPurchase = false;
        if (ToAdd != null && menuItem != null)
        {
            Hide();
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
        foreach (MenuItem menuItem in Store.Menu.Items)//preload all item models so it doesnt bog the menu down
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
}