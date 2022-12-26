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

public class SellMenu : Menu
{
    private UIMenu ParentMenu;
    private UIMenu sellMenuRNUI;
    private IModItems ModItems;
    private MenuPool MenuPool;
    private ILocationInteractable Player;
    private Camera StoreCam;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private ITimeControllable Time;
    private IWeapons Weapons;

    private Transaction Transaction;
    private ShopMenu ShopMenu;

    public SellMenu(MenuPool menuPool, UIMenu parentMenu, ShopMenu shopMenu, Transaction transaction, IModItems modItems, ILocationInteractable player, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        ParentMenu = parentMenu;
        ModItems = modItems;
        Player = player;
        World = world;
        Settings = settings;
        MenuPool = menuPool;
        Transaction = transaction;
        ShopMenu = shopMenu;
        Time = time;
        Weapons = weapons;
        StoreCam = Camera.RenderingCamera;
    }
    public void Setup()
    {
        StoreCam = Camera.RenderingCamera;
        EntryPoint.WriteToConsole($"SellMenu: HasBannerImage {Transaction.HasBannerImage} RemoveBanner {Transaction.RemoveBanner}");
        if (ParentMenu != null)
        {
            sellMenuRNUI = MenuPool.AddSubMenu(ParentMenu, "Sell");
            if (Transaction.HasBannerImage)
            {
                sellMenuRNUI.SetBannerType(Transaction.BannerImage);
            }
            else if (Transaction.RemoveBanner)
            {
                sellMenuRNUI.RemoveBanner();
            }

            sellMenuRNUI.OnIndexChange += (sender, newIndex) =>
            {
                GeneratePreview(sellMenuRNUI, newIndex);
            };
            sellMenuRNUI.OnMenuOpen += (sender) =>
            {
                GeneratePreview(sellMenuRNUI, sellMenuRNUI.CurrentSelection);
            };
            sellMenuRNUI.OnMenuClose += (sender) =>
            {
                Transaction.ClearPreviews();
            };
        }
        if (sellMenuRNUI == null)
        {
            return;
        }
        sellMenuRNUI.Clear();
        CreateCategories();
        foreach (MenuItem cii in ShopMenu.Items.Where(x => x.Sellable))
        {
            EntryPoint.WriteToConsole($"SELL MENU ADD ITEM {cii.ModItemName} Sellable:{cii.Sellable} SalesPrice {cii.SalesPrice} NumberOfItemsToSellToPlayer:{cii.NumberOfItemsToSellToPlayer} NumberOfItemsToPurchaseFromPlayer:{cii.NumberOfItemsToPurchaseFromPlayer}");
            cii.ModItem.CreateSellMenuItem(Transaction, cii, sellMenuRNUI, Settings, Player, Transaction.IsStealing, World);
        }
    }
    public void Dispose()
    {
        Hide();
        Game.RawFrameRender -= (s, e) => MenuPool.DrawBanners(e.Graphics);
    }
    public void Update()
    {

    }
    public override void Hide()
    {
        Transaction.ClearPreviews();
        if (sellMenuRNUI != null)
        {
            sellMenuRNUI.Visible = false;
            Player.ButtonPrompts.Clear();
        }
    }
    public override void Show()
    {
        if (sellMenuRNUI != null)
        {
            sellMenuRNUI.Visible = true;
        }
    }
    public override void Toggle()
    {
        if (!sellMenuRNUI.Visible)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    private void CreateCategories()
    {
        if (ShopMenu.Items.Where(x => x.Sellable).Count() < 7)
        {
            return;
        }
        List<string> Categories = new List<string>();
        foreach (MenuItem cii in ShopMenu.Items.Where(x => x.Sellable))
        {
            if (!Categories.Contains(cii.ModItem.MenuCategory))
            {
                Categories.Add(cii.ModItem.MenuCategory);
            }
        }
        foreach (string category in Categories)
        {
            UIMenu categoryMenu = MenuPool.AddSubMenu(sellMenuRNUI, category);
            if (Transaction.HasBannerImage)
            {
                categoryMenu.SetBannerType(Transaction.BannerImage);
            }
            else if (Transaction.RemoveBanner)
            {
                categoryMenu.RemoveBanner();
            }
            categoryMenu.OnIndexChange += (sender, newIndex) =>
            {
                GeneratePreview(categoryMenu, newIndex);
            };
            categoryMenu.OnMenuOpen += (sender) =>
            {
                GeneratePreview(categoryMenu, categoryMenu.CurrentSelection);
            };
            categoryMenu.OnMenuClose += (sender) =>
            {
                Transaction.ClearPreviews();
            };
        }
    }
    private void GeneratePreview(UIMenu menuSelected, int v)
    {
        MenuItem selectedMenuItem = ShopMenu.Items.Where(x => x.ModItemName == menuSelected.MenuItems[v].Text).FirstOrDefault();
        if (selectedMenuItem != null)
        {
            CreatePreview(selectedMenuItem);
            EntryPoint.WriteToConsole($"{menuSelected.TitleText} Menu OnIndexChange newIndex {v} SELECTED {selectedMenuItem.ModItemName}");
        }
        else
        {
            Transaction.ClearPreviews();
            EntryPoint.WriteToConsole($"{menuSelected.TitleText} Menu OnIndexChange newIndex {v} NO ITEM SELECTED");
        }
    }
    private void CreatePreview(MenuItem selectedMenu)
    {
        Transaction.ClearPreviews();
        if (selectedMenu == null || selectedMenu.ModItem == null || !Transaction.PreviewItems || !Settings.SettingsManager.PlayerOtherSettings.GenerateStoreItemPreviews)
        {
            return;
        }
        selectedMenu.ModItem.CreatePreview(Transaction, StoreCam);
    }
    public void OnItemPurchased(MenuItem menuItem)
    {
        menuItem.ModItem.UpdateSellMenuItem(Transaction, menuItem, Settings, Player, Transaction.IsStealing);
    }
}