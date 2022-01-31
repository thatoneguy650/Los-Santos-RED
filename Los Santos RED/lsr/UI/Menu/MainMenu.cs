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
    private UIMenuItem RemoveVehicleOwnership;
    private UIMenuItem AboutMenu;

    public MainMenu(MenuPool menuPool, IActionable player,ISaveable saveablePlayer, IGameSaves gameSaves, IWeapons weapons, IPedSwap pedswap, IEntityProvideable world, ISettingsProvideable settings, ITaskerable tasker, IInventoryable playerinventory, IModItems modItems, UI ui, IGangs gangs, ITimeControllable time)
    {
        Player = player;
        Settings = settings;
        Tasker = tasker;
        UI = ui;
        Main = new UIMenu("Los Santos RED", "Select an Option");
        Main.SetBannerType(EntryPoint.LSRedColor);
        menuPool.Add(Main);
        Main.OnItemSelect += OnItemSelect;
        Main.OnListChange += OnListChange;
        SettingsMenuNew = new SettingsMenuNew(menuPool, Main, Player, world, Settings);
        SaveMenu = new SaveMenu(menuPool, Main, saveablePlayer, gameSaves, weapons, pedswap, playerinventory, Settings, world, gangs, time);
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
            SettingsMenuNew.Hide();
            SaveMenu.Hide();
            PedSwapMenu.Hide();
        }
        else
        {
            Main.Visible = false;

            ActionMenu.Hide();
            InventoryMenu.Hide();
            SettingsMenuNew.Hide();
            SaveMenu.Hide();
            PedSwapMenu.Hide();
        }
    }
    private void CreateMainMenu()
    {
        AboutMenu = new UIMenuItem("About", "Shows some general information about the mod and its features. More to Come.");
        AboutMenu.RightBadge = UIMenuItem.BadgeStyle.Alert;
        ShowReportingMenu = new UIMenuItem("Player Information", "Show the player information menu. This pause menu has info about owned vehicles, gang relationships, locations, text messages, and contacts.");
        ShowReportingMenu.RightBadge = UIMenuItem.BadgeStyle.Lock;
        TakeVehicleOwnership = new UIMenuItem("Set Vehicle as Owned", "Set closest vehicle as owned by the mode. This will let you enter it freely and police/civilians will not react as if it is stolen when you enter.");
        TakeVehicleOwnership.RightBadge = UIMenuItem.BadgeStyle.Car;
        RemoveVehicleOwnership = new UIMenuItem("Remove Vehicle Onwership", "Set closest vehicle as not owned");
        RemoveVehicleOwnership.RightBadge = UIMenuItem.BadgeStyle.Car;
        UnloadMod = new UIMenuItem("Unload Mod", "Unload mod and change back to vanilla. ~r~Load Game~s~ required at minimum, ~r~Restart~s~ for best results.");
        UnloadMod.RightBadge = UIMenuItem.BadgeStyle.Star;
        Main.AddItem(AboutMenu);
        Main.AddItem(ShowReportingMenu);
        Main.AddItem(TakeVehicleOwnership);
        Main.AddItem(RemoveVehicleOwnership);
        Main.AddItem(UnloadMod);
    }
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == ShowReportingMenu)
        {
            UI.ToggleReportingMenu();
        }
        else if (selectedItem == UnloadMod)
        {
            EntryPoint.ModController.Dispose();
        }
        else if (selectedItem == TakeVehicleOwnership)
        {
            Player.TakeOwnershipOfNearestCar();
        }
        else if (selectedItem == RemoveVehicleOwnership)
        {
            Player.RemoveOwnershipOfNearestCar();
        }
        else if (selectedItem == AboutMenu)
        {
            UI.ToggleAboutMenu();
        }
        Main.Visible = false;
    }
    private void OnListChange(UIMenu sender, UIMenuListItem list, int index)
    {

    }
}