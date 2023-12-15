using LosSantosRED.lsr.Interface;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Linq;

public class MainMenu : ModUIMenu
{
    private MenuPool MenuPool;
    private UIMenu Main;
    private UI UI;

   // private ActionMenu ActionMenu;
   // private InventoryMenu InventoryMenu;
    private PedSwapMenu PedSwapMenu;
   // private SaveMenu SaveMenu;
    private SettingsMenu SettingsMenu;

    private IGangs Gangs;
    private IActionable ActionablePlayer;
    private ISaveable SaveablePlayer;
    private IGameSaves GameSaves;
    private IWeapons Weapons;
    private IPedSwap PedSwap;
    private IEntityProvideable World;
    private IInventoryable PlayerInventory;
    private IModItems ModItems;
    private ITimeControllable Time;
    private IPlacesOfInterest PlacesOfInterest;
    private IDances Dances;
    private IGestures Gestures;
    private ITaskerable Tasker;
    private ISettingsProvideable Settings;
    private ILocationInteractable Player;
    private IActivityPerformable ActivityPerformable;
    private ICrimes Crimes;

    public MainMenu(MenuPool menuPool, IActionable actionablePlayer, ILocationInteractable player, ISaveable saveablePlayer, IGameSaves gameSaves, IWeapons weapons, IPedSwap pedswap, IEntityProvideable world, ISettingsProvideable settings, ITaskerable tasker, 
        IInventoryable playerinventory, IModItems modItems, UI ui, IGangs gangs, ITimeControllable time, IPlacesOfInterest placesOfInterest, IDances dances, IGestures gestures, IActivityPerformable activityPerformable, IAgencies agencies, ICrimes crimes, IIntoxicants intoxicants, IShopMenus shopMenus)
    {
        MenuPool = menuPool;
        ActionablePlayer = actionablePlayer;
        SaveablePlayer = saveablePlayer;
        GameSaves = gameSaves;
        Weapons = weapons;
        PedSwap = pedswap;
        World = world;
        PlayerInventory = playerinventory;
        ModItems = modItems;
        Time = time;
        PlacesOfInterest = placesOfInterest;
        Dances = dances;
        Gestures = gestures;
        Player = player;
        Settings = settings;
        Tasker = tasker;
        Gangs = gangs;
        UI = ui;
        Crimes = crimes;
        ActivityPerformable = activityPerformable;
        Main = new UIMenu("Los Santos RED", "Select an Option");
        SettingsMenu = new SettingsMenu(MenuPool, Main, Settings, Crimes, intoxicants, shopMenus);
        PedSwapMenu = new PedSwapMenu(MenuPool, Main, PedSwap, Gangs, agencies, ActionablePlayer);
    }

    public void Setup()
    {
        Main.SetBannerType(EntryPoint.LSRedColor);
        MenuPool.Add(Main);
        SettingsMenu.Setup();    
        PedSwapMenu.Setup();    
        CreateMainMenu();
    }

    public override void Hide()
    {
        Main.Visible = false;
    }
    public override void Show()
    {
        if (!Main.Visible)
        {
            Main.Visible = true;
        }
    }
    public override void Toggle()
    {
        if (!Main.Visible)
        {
            Main.Visible = true;
        }
        else
        {
            Main.Visible = false;
        }
    }
    private void CreateMainMenu()
    {
        //The Submenus have already added their items
        UIMenuItem ShowSaveMenu = new UIMenuItem("Game Saves", "Shows a list of the players saves.");
        ShowSaveMenu.RightBadge = UIMenuItem.BadgeStyle.Makeup;
        ShowSaveMenu.Activated += (s, e) =>
        {
            UI.SavePauseMenu.Toggle();
            Main.Visible = false;
        };
        Main.AddItem(ShowSaveMenu);
        UIMenuItem AboutMenu = new UIMenuItem("About", "Shows some general information about the mod and its features. More to Come.");
        AboutMenu.RightBadge = UIMenuItem.BadgeStyle.Alert;
        AboutMenu.Activated += (s, e) =>
        {
            UI.AboutMenu.Toggle();
            Main.Visible = false;
        };
        //Main.AddItem(AboutMenu);//gone but not forgotten
        UIMenuItem ShowReportingMenu = new UIMenuItem("Player Information", "Show the player information menu. This pause menu has info about Owned Vehicles, Licenses, ~r~Gang Relationships~s~, and ~y~Locations~s~.");
        ShowReportingMenu.RightBadge = UIMenuItem.BadgeStyle.Lock;
        ShowReportingMenu.Activated += (s, e) =>
        {
            UI.PlayerInfoMenu.Toggle();
            Main.Visible = false;
        };
        Main.AddItem(ShowReportingMenu);

        UIMenuItem ShowSimplePhoneMenu = new UIMenuItem("Replies and Contacts", "Shows the phone replies, text messages, and contacts. Will allow you to call ~p~Contacts~s~ and lookup ~y~Locations~s~.");
        ShowSimplePhoneMenu.RightBadge = UIMenuItem.BadgeStyle.Alert;
        ShowSimplePhoneMenu.Activated += (s, e) =>
        {
            UI.MessagesMenu.Toggle();
            Main.Visible = false;
        };
        Main.AddItem(ShowSimplePhoneMenu);

        UIMenu VehicleItems = MenuPool.AddSubMenu(Main, "Vehicle Ownership");
        VehicleItems.SetBannerType(EntryPoint.LSRedColor);
        Main.MenuItems[Main.MenuItems.Count() - 1].Description = "Add or Remove ownership of nearby vehicles.";
        Main.MenuItems[Main.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Car;

        UIMenuItem TakeVehicleOwnership = new UIMenuItem("Set Vehicle as Owned", "Set closest vehicle as owned. This will let you enter it freely and police/civilians will not react as if it is stolen when you enter.");
        TakeVehicleOwnership.RightBadge = UIMenuItem.BadgeStyle.Car;
        TakeVehicleOwnership.Activated += (s, e) =>
        {
            Player.VehicleOwnership.TakeOwnershipOfNearestCar();
            Main.Visible = false;
        };
        VehicleItems.AddItem(TakeVehicleOwnership);
        UIMenuItem RemoveVehicleOwnership = new UIMenuItem("Remove Vehicle Ownership", "Set closest vehicle as not owned");
        RemoveVehicleOwnership.RightBadge = UIMenuItem.BadgeStyle.Car;
        RemoveVehicleOwnership.Activated += (s, e) =>
        {
            Player.VehicleOwnership.RemoveOwnershipOfNearestCar();
            Main.Visible = false;
        };
        VehicleItems.AddItem(RemoveVehicleOwnership);

        UIMenuItem UnloadMod = new UIMenuItem("Deactivate Mod", "Deactivate mod and change back to vanilla. Use Shift+F10 to restart. Does not unload from RPH. ~r~Load Game~s~ required at minimum, ~r~Restart~s~ for best results.");
        UnloadMod.RightBadge = UIMenuItem.BadgeStyle.Star;
        UnloadMod.Activated += (s, e) =>
        {
            EntryPoint.ModController.Dispose();
            Main.Visible = false;
        };
        Main.AddItem(UnloadMod);
    }

}