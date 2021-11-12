using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Collections.Generic;

public class MainMenu : Menu
{

    private ActionMenu ActionMenu;
    private PedSwapMenu PedSwapMenu;
    private SaveMenu SaveMenu;
    private SettingsMenu SettingsMenu;
    private UIMenu Main;
    private IActionable Player;
    private UIMenuItem CallPolice;

    private UIMenuItem GenerateCrime;

    private UIMenuItem ShowStatus;
    private UIMenuItem UnloadMod;
    private UIMenuItem TakeVehicleOwnership;
    private ISettingsProvideable Settings;
    private ITaskerable Tasker;
    public MainMenu(MenuPool menuPool, IActionable player,ISaveable saveablePlayer, IGameSaves gameSaves, IWeapons weapons, IPedSwap pedswap, IEntityProvideable world, ISettingsProvideable settings, ITaskerable tasker)
    {
        Player = player;
        Settings = settings;
        Tasker = tasker;
        Main = new UIMenu("Los Santos RED", "Select an Option");
        menuPool.Add(Main);
        SettingsMenu = new SettingsMenu(menuPool, Main, Player, world, Settings);
        SaveMenu = new SaveMenu(menuPool, Main, saveablePlayer, gameSaves, weapons, pedswap);
        PedSwapMenu = new PedSwapMenu(menuPool, Main, pedswap);
        ActionMenu = new ActionMenu(menuPool, Main, Player);
        CreateMainMenu();
    }
    public override void Hide()
    {
        Main.Visible = false;
        ActionMenu.Hide();
        SaveMenu.Hide();
    }
    public override void Show()
    {
        if (!Main.Visible)
        {
            ActionMenu.Update();
            Main.Visible = true;
            ActionMenu.Hide();
            SettingsMenu.Hide();
            SaveMenu.Hide();
            PedSwapMenu.Hide();
        }
    }
    public override void Toggle()
    {
        if (!Main.Visible)
        {
            ActionMenu.Update();
            Main.Visible = true;
            ActionMenu.Hide();
            SettingsMenu.Hide();
            SaveMenu.Hide();
            PedSwapMenu.Hide();
        }
        else
        {
            Main.Visible = false;
            ActionMenu.Hide();
            SettingsMenu.Hide();
            SaveMenu.Hide();
            PedSwapMenu.Hide();
        }
    }
    private void CreateMainMenu()
    {
        Main.OnItemSelect += OnItemSelect;
        Main.OnListChange += OnListChange;

        ShowStatus = new UIMenuItem("Show Status", "Show the player status with a notification");
        ShowStatus.RightBadge = UIMenuItem.BadgeStyle.Makeup;
        CallPolice = new UIMenuItem("Call Police", "Need some help?");
        CallPolice.RightBadge = UIMenuItem.BadgeStyle.Alert;
        GenerateCrime = new UIMenuItem("Generate Crime", "Attempt to generate a random crime around the player");
        GenerateCrime.RightBadge = UIMenuItem.BadgeStyle.Gun;
        TakeVehicleOwnership = new UIMenuItem("Set as Owned", "Set closest vehicle as owned");
        TakeVehicleOwnership.RightBadge = UIMenuItem.BadgeStyle.Car;
        UnloadMod = new UIMenuItem("Unload Mod", "Unload mod and change back to vanilla (Load Game Required)");
        UnloadMod.RightBadge = UIMenuItem.BadgeStyle.Star;

        Main.AddItem(CallPolice);
        Main.AddItem(GenerateCrime);
        Main.AddItem(TakeVehicleOwnership);
        Main.AddItem(ShowStatus);
        Main.AddItem(UnloadMod);
    }
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == ShowStatus)
        {
            Player.DisplayPlayerNotification();
        }
        else if (selectedItem == UnloadMod)
        {
            EntryPoint.ModController.Dispose();
        }
        else if (selectedItem == TakeVehicleOwnership)
        {
            Player.TakeOwnershipOfNearestCar();
        }
        else if (selectedItem == CallPolice)
        {
            Player.CallPolice();
        }
        else if (selectedItem == GenerateCrime)
        {
            Tasker.CreateCrime();
        }
        Main.Visible = false;
    }
    private void OnListChange(UIMenu sender, UIMenuListItem list, int index)
    {

    }
}