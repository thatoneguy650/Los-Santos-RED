using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Collections.Generic;

public class MainMenu : Menu
{
    private InventoryMenu InventoryMenu;
    private ActionMenu ActionMenu;
    private PedSwapMenu PedSwapMenu;
    private SaveMenu SaveMenu;
    //private SettingsMenu SettingsMenu;

    private SettingsMenuNew SettingsMenuNew;

    private UIMenu Main;
    private IActionable Player;
    private UIMenuItem CallPolice;

    private UIMenuItem GenerateCrime;

    private UIMenuItem ShowStatus;
    private UIMenuItem UnloadMod;
    private UIMenuItem ShowReportingMenu;
    private UIMenuItem TakeVehicleOwnership;
    private ISettingsProvideable Settings;
    private ITaskerable Tasker;
    private UI UI;
    public MainMenu(MenuPool menuPool, IActionable player,ISaveable saveablePlayer, IGameSaves gameSaves, IWeapons weapons, IPedSwap pedswap, IEntityProvideable world, ISettingsProvideable settings, ITaskerable tasker, IInventoryable playerinventory, IModItems modItems, UI ui)
    {
        Player = player;
        Settings = settings;
        Tasker = tasker;
        UI = ui;
        Main = new UIMenu("Los Santos RED", "Select an Option");
        Main.SetBannerType(System.Drawing.Color.FromArgb(181, 48, 48));
        menuPool.Add(Main);
        Main.OnItemSelect += OnItemSelect;
        Main.OnListChange += OnListChange;
        //SettingsMenu = new SettingsMenu(menuPool, Main, Player, world, Settings);

        SettingsMenuNew = new SettingsMenuNew(menuPool, Main, Player, world, Settings);



        SaveMenu = new SaveMenu(menuPool, Main, saveablePlayer, gameSaves, weapons, pedswap, playerinventory, Settings, world);
        PedSwapMenu = new PedSwapMenu(menuPool, Main, pedswap);
        ActionMenu = new ActionMenu(menuPool, Main, Player, Settings);
        InventoryMenu = new InventoryMenu(menuPool, Main, Player, modItems);
        CreateMainMenu();
    }
    public override void Hide()
    {
        Main.Visible = false;
        ActionMenu.Hide();
        SaveMenu.Hide();
        InventoryMenu.Hide();
    }
    public override void Show()
    {
        if (!Main.Visible)
        {
            ActionMenu.Update();
            InventoryMenu.Update();
            Main.Visible = true;
            ActionMenu.Hide();
            InventoryMenu.Hide();
            //SettingsMenu.Hide();
            SettingsMenuNew.Hide();
            SaveMenu.Hide();
            PedSwapMenu.Hide();
        }
    }
    public override void Toggle()
    {
        if (!Main.Visible)
        {
            ActionMenu.Update();
            InventoryMenu.Update();

            Main.Visible = true;

            ActionMenu.Hide();
            InventoryMenu.Hide();
            //SettingsMenu.Hide();
            SettingsMenuNew.Hide();
            SaveMenu.Hide();
            PedSwapMenu.Hide();
        }
        else
        {
            Main.Visible = false;

            ActionMenu.Hide();
            InventoryMenu.Hide();
            //SettingsMenu.Hide();
            SettingsMenuNew.Hide();
            SaveMenu.Hide();
            PedSwapMenu.Hide();
        }
    }
    private void CreateMainMenu()
    {
        //ShowStatus = new UIMenuItem("Show Status", "Show the player status with a notification");
        //ShowStatus.RightBadge = UIMenuItem.BadgeStyle.Makeup;
        ShowReportingMenu = new UIMenuItem("Status Menu", "Show the player status menu");
        ShowReportingMenu.RightBadge = UIMenuItem.BadgeStyle.Lock;
        TakeVehicleOwnership = new UIMenuItem("Set as Owned", "Set closest vehicle as owned");
        TakeVehicleOwnership.RightBadge = UIMenuItem.BadgeStyle.Car;
        UnloadMod = new UIMenuItem("Unload Mod", "Unload mod and change back to vanilla (Load Game Required)");
        UnloadMod.RightBadge = UIMenuItem.BadgeStyle.Star; 
       // Main.AddItem(ShowStatus);
        Main.AddItem(ShowReportingMenu);
        Main.AddItem(TakeVehicleOwnership);
        Main.AddItem(UnloadMod);
    }
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == ShowStatus)
        {
            Player.DisplayPlayerNotification();
        }
        else if (selectedItem == ShowReportingMenu)
        {
            UI.ToggleReportingMenu();
            //Player.DisplayPlayerGangNotification();
        }
        else if (selectedItem == UnloadMod)
        {
            EntryPoint.ModController.Dispose();
        }
        else if (selectedItem == TakeVehicleOwnership)
        {
            Player.TakeOwnershipOfNearestCar();
        }
        Main.Visible = false;
    }
    private void OnListChange(UIMenu sender, UIMenuListItem list, int index)
    {

    }
}