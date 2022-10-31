using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Collections.Generic;
using System.Linq;

public class ActionMenu : Menu
{
    private MenuPool MenuPool;
    private UIMenu ParentMenu;

    private UIMenu Actions;
    private UIMenuItem CallPolice;
    //private UIMenuListScrollerItem<LSR.Vehicles.LicensePlate> ChangePlate;
    private UIMenuListScrollerItem<string> CurrentActivityMenu;
    private UIMenuItem EnterAsPassenger;
    private UIMenuListScrollerItem<GestureData> GestureMenu;
    private UIMenuListScrollerItem<DanceData> DanceMenu;
    private UIMenuItem HotwireVehicle;
    private UIMenuItem ToggleActionMode;
    private UIMenuItem ToggleStealthMode;
    private UIMenuItem IntimidateDriver;
    private UIMenuListScrollerItem<string> LayDown;
    //private UIMenuItem RemovePlate;
    private UIMenuItem ShuffleSeat;
    private UIMenuListScrollerItem<string> SitDown;
    private UIMenuItem Suicide;
    private UIMenuNumericScrollerItem<int> ToggleBodyArmor;

    private List<GestureData> GestureLookups;
    private List<DanceData> DanceLookups;

    private IActionable Player;
    private ISettingsProvideable Settings;
    private IDances Dances;
    private IGestures Gestures;
    public ActionMenu(MenuPool menuPool, UIMenu parentMenu, IActionable player, ISettingsProvideable settings, IDances dances, IGestures gestures)
    {
        MenuPool = menuPool;
        ParentMenu = parentMenu;
        Player = player;
        Settings = settings;
        Dances = dances;
        Gestures = gestures;
    }
    public void Setup()
    {
        Actions = MenuPool.AddSubMenu(ParentMenu, "Actions");
        ParentMenu.MenuItems[ParentMenu.MenuItems.Count() - 1].Description = "Start, Pause, or Stop actions with your character.";
        ParentMenu.MenuItems[ParentMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Tick;
        Actions.SetBannerType(EntryPoint.LSRedColor);
        CreateActionsMenu();
    }
    public override void Hide()
    {
        Actions.Visible = false;
    }
    public override void Show()
    {
        Update();
        Actions.Visible = true;
    }
    public override void Toggle()
    {
        if (!Actions.Visible)
        {
            Actions.Visible = true;
        }
        else
        {
            Actions.Visible = false;
        }
    }
    public void Update()
    {
        if (Player.ActivityManager.HasCurrentActivity)
        {
            CurrentActivityMenu.Enabled = true;
        }
        else
        {
            CurrentActivityMenu.Enabled = false;
        }

        if (Player.CharacterModelIsFreeMode)
        {
            ToggleBodyArmor.Enabled = true;
        }
        else
        {
            ToggleBodyArmor.Enabled = false;
        }
        //ChangePlate.Items = Player.SpareLicensePlates;
    }
    private void CreateActionsMenu()
    {
        Actions.Clear();

        Suicide = new UIMenuItem("Suicide", "Commit Suicide");
        Suicide.Activated += (sender, selectedItem) =>
        {
            Player.ActivityManager.CommitSuicide();
            Actions.Visible = false;
        };
        //ChangePlate = new UIMenuListScrollerItem<LSR.Vehicles.LicensePlate>("Change Plate", "Change your license plate if you have spares.", Player.SpareLicensePlates);
        //ChangePlate.Activated += (sender, selectedItem) =>
        //{
        //    Player.ActivityManager.ChangePlate(ChangePlate.SelectedItem);
        //    Actions.Visible = false;
        //};       
        //RemovePlate = new UIMenuItem("Remove Plate", "Remove the license plate.");
        //RemovePlate.Activated += (sender, selectedItem) =>
        //{
        //    Player.ActivityManager.RemovePlate();
        //    Actions.Visible = false;
        //};
        SitDown = new UIMenuListScrollerItem<string>("Sit Down", "Sit down either at the nearest seat or where you are.", new List<string>() { "At Closest Seat", "Here Backwards", "Here Forwards" });
        SitDown.Activated += (sender, selectedItem) =>
        {
            if (SitDown.SelectedItem == "At Closest Seat")
            {
                Player.ActivityManager.StartSittingDown(true, true);
            }
            else
            {
                if (SitDown.SelectedItem == "Here Backwards")
                {
                    Player.ActivityManager.StartSittingDown(false, false);
                }
                else
                {
                    Player.ActivityManager.StartSittingDown(false, true);
                }
            }
            Actions.Visible = false;
        };
        LayDown = new UIMenuListScrollerItem<string>("Lay Down", "Lay down either at the nearest seat or where you are.", new List<string>() { "At Closest Bed", "Here" });
        LayDown.Activated += (sender, selectedItem) =>
        {
            if (LayDown.SelectedItem == "At Closest Bed")
            {
                Player.ActivityManager.StartSleeping();
            }
            else
            {
                Player.ActivityManager.StartSleeping();
            }
            Actions.Visible = false;
        };
        GestureMenu = new UIMenuListScrollerItem<GestureData>("Gesture", "Perform the selected gesture", Gestures.GestureLookups);
        GestureMenu.Activated += (sender, selectedItem) =>
        {
            Player.ActivityManager.Gesture(GestureMenu.SelectedItem);
            Actions.Visible = false;
        };
        DanceMenu = new UIMenuListScrollerItem<DanceData>("Dance", "Perform the selected dance", Dances.DanceLookups);
        DanceMenu.Activated += (sender, selectedItem) =>
        {
            Player.ActivityManager.Dance(DanceMenu.SelectedItem);
            Actions.Visible = false;
        };
        CurrentActivityMenu = new UIMenuListScrollerItem<string>("Current Activity", "Continue, Pause, or Stop the Current Activity", new List<string>() { "Continue", "Pause", "Stop" });
        CurrentActivityMenu.Activated += (sender, selectedItem) =>
        {
            if (CurrentActivityMenu.SelectedItem == "Continue")
            {
                Player.ActivityManager.ContinueCurrentActivity();
            }
            else if (CurrentActivityMenu.SelectedItem == "Pause")
            {
                Player.ActivityManager.PauseCurrentActivity();
            }
            else if (CurrentActivityMenu.SelectedItem == "Stop")
            {
                Player.ActivityManager.StopDynamicActivity();
            }
            Actions.Visible = false;
        };
        EnterAsPassenger = new UIMenuItem("Enter as Passenger", "Enter nearest vehicle as a passenger");
        EnterAsPassenger.Activated += (sender, selectedItem) =>
        {
            Player.ActivityManager.EnterVehicleAsPassenger(false);
            Actions.Visible = false;
        };
        ShuffleSeat = new UIMenuItem("Shuffle Seat", "Shuffles your current seat");
        ShuffleSeat.Activated += (sender, selectedItem) =>
        {
            Player.ActivityManager.ShuffleToNextSeat();
            Actions.Visible = false;
        };
        IntimidateDriver = new UIMenuItem("Intimidate Driver", "Force driver to flee in the vehicle");
        IntimidateDriver.Activated += (sender, selectedItem) =>
        {
            Player.ActivityManager.ForceErraticDriver();
            Actions.Visible = false;
        };
        HotwireVehicle = new UIMenuItem("Hotwire Vehicle", "Hotwire current vehicle");
        HotwireVehicle.Activated += (sender, selectedItem) =>
        {
            Player.ActivityManager.StartHotwiring();
            Actions.Visible = false;
        };
        ToggleActionMode = new UIMenuItem("Toggle Action Mode", "Toggle action mode on or off");
        ToggleActionMode.Activated += (sender, selectedItem) =>
        {
            Player.Stance.ToggleActionMode();
            Actions.Visible = false;
        };
        ToggleStealthMode = new UIMenuItem("Toggle Stealth Mode", "Toggle stealth mode on or off");
        ToggleStealthMode.Activated += (sender, selectedItem) =>
        {
            Player.Stance.ToggleStealthMode();
            Actions.Visible = false;
        };
        ToggleBodyArmor = new UIMenuNumericScrollerItem<int>("Toggle Body Armor", "Select to take toggle, scroll to change", 0, 18, 1);
        ToggleBodyArmor.Value = 0;
        ToggleBodyArmor.Activated += (sender, selectedItem) =>
        {
            Player.ToggleBodyArmor(ToggleBodyArmor.Value);
            Actions.Visible = false;
        };
        ToggleBodyArmor.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            Player.SetBodyArmor(ToggleBodyArmor.Value);
        };

        Actions.AddItem(CurrentActivityMenu);
        Actions.AddItem(GestureMenu);
        Actions.AddItem(DanceMenu);
        Actions.AddItem(SitDown);
        Actions.AddItem(ToggleActionMode);
        Actions.AddItem(ToggleStealthMode);
        Actions.AddItem(LayDown);
        //Actions.AddItem(ChangePlate);
        //Actions.AddItem(RemovePlate);
        Actions.AddItem(Suicide);
        Actions.AddItem(ToggleBodyArmor);
#if DEBUG
        Actions.AddItem(EnterAsPassenger);
        Actions.AddItem(ShuffleSeat);
        Actions.AddItem(IntimidateDriver);
        Actions.AddItem(HotwireVehicle);
#endif
    }
}