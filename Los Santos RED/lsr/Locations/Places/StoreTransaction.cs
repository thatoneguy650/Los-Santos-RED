using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class StoreTransaction
{
    private MenuPool MenuPool;
    private UIMenu ModItemMenu;
    private Camera StoreCam;
    private StorePurchaseMenu PurchaseMenu;
    private StoreSellMenu SellMenu;
    private bool IsDisposed;

    private IModItems ModItems;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private IWeapons Weapons;
    private ITimeControllable Time;
    private IActivityPerformable Player;
    private bool IsAnyMenuVisible => MenuPool.IsAnyMenuOpen();
    public StoreTransaction(IActivityPerformable player, TransactableLocation store, Camera storeCam, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        Player = player;
        Store = store;
        StoreCam = storeCam;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;
    }

    public TransactableLocation Store { get; set; }
    public Texture BannerImage { get; private set; }
    public string BannerImagePath { get; set; }
    public bool HasBannerImage => BannerImagePath != "";
    public bool RemoveBanner { get; set; } = false;
    public void Setup()
    {
        MenuPool = new MenuPool();
        ModItemMenu = new UIMenu(Store.Name, Store.Description);
        if (HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
            ModItemMenu.SetBannerType(BannerImage);
            Game.RawFrameRender += (s, e) => MenuPool.DrawBanners(e.Graphics);
        }     
        ModItemMenu.OnItemSelect += OnItemSelect;
        MenuPool.Add(ModItemMenu);
    }
    public void Dispose()
    {
        if (!IsDisposed)
        {
            Game.RawFrameRender -= (s, e) => MenuPool.DrawBanners(e.Graphics);
            IsDisposed = true;
            if (ModItemMenu != null)
            {
                ModItemMenu.Visible = false;
            }
            PurchaseMenu?.Dispose();
            SellMenu?.Dispose();
            Player.ButtonPrompts.RemoveAll(x => x.Group == "Transaction");
        }
    }
    public void Start()
    {
        Setup();
        ShowMenu();
        Dispose();
        Tick();
        Dispose();
    }
    public void ClearPreviews()
    {
        PurchaseMenu?.ClearPreviews();
        SellMenu?.ClearPreviews();
    }
    private void Tick()
    {
        while (!IsDisposed)
        {
            MenuPool.ProcessMenus();
            if (!IsAnyMenuVisible)
            {
                EntryPoint.WriteToConsole("Transaction Dispose 1", 5);
                Dispose();
            }
            if (ModItemMenu.MenuItems.Count() == 1 && ModItemMenu.Visible)
            {
                EntryPoint.WriteToConsole("Transaction Dispose 2", 5);
                Dispose();
            }
            PurchaseMenu?.Update();
            SellMenu?.Update();
            GameFiber.Yield();
        }
        EntryPoint.WriteToConsole("Transaction Dispose 3", 5);
        Dispose();
        GameFiber.Sleep(1000);
    }
    private void ShowMenu()
    {
        if (IsDisposed)
        {
            return;
        }
        ModItemMenu.Clear();
        bool hasPurchaseMenu = false;
        bool hasSellMenu = false;
        if (Store.Menu.Items.Any(x => x.Purchaseable))
        {
            PurchaseMenu = new StorePurchaseMenu(MenuPool, ModItemMenu, Store,StoreCam,  ModItems, Player, World, Settings, this, Weapons, Time);
            PurchaseMenu.Setup();
            hasPurchaseMenu = true;
        }
        if (Store.Menu.Items.Any(x => x.Sellable))
        {
           // SellMenu = new StoreSellMenu(MenuPool, ModItemMenu, Ped, Store, ModItems, Player, StoreCam, IsUsingCustomCam, this, World, Settings);//was IsUsingCustomCam before
           // SellMenu.Setup();
           // hasSellMenu = true;
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
}

