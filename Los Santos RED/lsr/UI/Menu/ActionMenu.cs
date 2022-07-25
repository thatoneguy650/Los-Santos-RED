using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Collections.Generic;
using System.Linq;

public class ActionMenu : Menu
{
    private UIMenu Actions;
    private UIMenuItem CallPolice;
    private UIMenuListScrollerItem<LSR.Vehicles.LicensePlate> ChangePlate;
    private UIMenuListScrollerItem<string> CurrentActivityMenu;
    private UIMenuItem EnterAsPassenger;
    private List<GestureData> GestureLookups;
    private List<DanceData> DanceLookups;
    private UIMenuListScrollerItem<GestureData> GestureMenu;
    private UIMenuListScrollerItem<DanceData> DanceMenu;
    private UIMenuItem HotwireVehicle;
    private UIMenuItem ToggleActionMode;
    private UIMenuItem ToggleStealthMode;
    private UIMenuItem IntimidateDriver;
    private UIMenuListScrollerItem<string> LayDown;
    private MenuPool MenuPool;
    private IActionable Player;
    private UIMenuItem RemovePlate;
    private ISettingsProvideable Settings;
    private UIMenuItem ShuffleSeat;
    private UIMenuListScrollerItem<string> SitDown;
    private UIMenuItem Suicide;
    private UIMenuNumericScrollerItem<int> ToggleBodyArmor;
    private IDances Dances;
    private IGestures Gestures;
    public ActionMenu(MenuPool menuPool, UIMenu parentMenu, IActionable player, ISettingsProvideable settings, IDances dances, IGestures gestures)
    {
        Player = player;
        Settings = settings;
        MenuPool = menuPool;
        Dances = dances;
        Gestures = gestures;
        Actions = MenuPool.AddSubMenu(parentMenu, "Actions");
        parentMenu.MenuItems[parentMenu.MenuItems.Count() - 1].Description = "Start, Pause, or Stop actions with your character.";
        parentMenu.MenuItems[parentMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Tick;
        Actions.SetBannerType(EntryPoint.LSRedColor);
        Actions.OnItemSelect += OnActionItemSelect;
        Actions.OnScrollerChange += OnScrollerChange;

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
        if (Player.HasCurrentActivity)
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
        ChangePlate.Items = Player.SpareLicensePlates;
    }
    private void CreateActionsMenu()
    {
        Actions.Clear();

        if (Player.IsCop)
        {
            CallPolice = new UIMenuItem("Radio for Backup", "Need some help?");
            CallPolice.RightBadge = UIMenuItem.BadgeStyle.Alert;
            Actions.AddItem(CallPolice);
        }
        else
        {
            CallPolice = new UIMenuItem("Call Police", "Need some help?");
            CallPolice.RightBadge = UIMenuItem.BadgeStyle.Alert;
        }
        Suicide = new UIMenuItem("Suicide", "Commit Suicide");
        ChangePlate = new UIMenuListScrollerItem<LSR.Vehicles.LicensePlate>("Change Plate", "Change your license plate if you have spares.", Player.SpareLicensePlates);
        RemovePlate = new UIMenuItem("Remove Plate", "Remove the license plate.");
        SitDown = new UIMenuListScrollerItem<string>("Sit Down", "Sit down either at the nearest seat or where you are.", new List<string>() { "At Closest Seat", "Here Backwards", "Here Forwards" });
        LayDown = new UIMenuListScrollerItem<string>("Lay Down", "Lay down either at the nearest seat or where you are.", new List<string>() { "At Closest Bed", "Here" });
        GestureMenu = new UIMenuListScrollerItem<GestureData>("Gesture", "Perform the selected gesture", Gestures.GestureLookups);
        DanceMenu = new UIMenuListScrollerItem<DanceData>("Dance", "Perform the selected dance", Dances.DanceLookups);
        CurrentActivityMenu = new UIMenuListScrollerItem<string>("Current Activity", "Continue, Pause, or Stop the Current Activity", new List<string>() { "Continue", "Pause", "Stop" });
        EnterAsPassenger = new UIMenuItem("Enter as Passenger", "Enter nearest vehicle as a passenger");
        ShuffleSeat = new UIMenuItem("Shuffle Seat", "Shuffles your current seat");
        IntimidateDriver = new UIMenuItem("Intimidate Driver", "Force driver to flee in the vehicle");
        HotwireVehicle = new UIMenuItem("Hotwire Vehicle", "Hotwire current vehicle");
        ToggleActionMode = new UIMenuItem("Toggle Action Mode", "Toggle action mode on or off");
        ToggleStealthMode = new UIMenuItem("Toggle Stealth Mode", "Toggle stealth mode on or off");
        ToggleBodyArmor = new UIMenuNumericScrollerItem<int>("Toggle Body Armor", "Select to take toggle, scroll to change", 0, 18, 1);
        ToggleBodyArmor.Value = 0;

        Actions.AddItem(CurrentActivityMenu);
        Actions.AddItem(GestureMenu);
        Actions.AddItem(DanceMenu);
        Actions.AddItem(SitDown);
        Actions.AddItem(ToggleActionMode);
        Actions.AddItem(ToggleStealthMode);

        Actions.AddItem(LayDown);

        Actions.AddItem(ChangePlate);
        Actions.AddItem(RemovePlate);
        Actions.AddItem(Suicide);
        Actions.AddItem(ToggleBodyArmor);
#if DEBUG
        Actions.AddItem(EnterAsPassenger);
        Actions.AddItem(ShuffleSeat);
        Actions.AddItem(IntimidateDriver);
        Actions.AddItem(HotwireVehicle);
#endif
    }
    private void OnActionItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == Suicide)
        {
            Player.CommitSuicide();
        }
        else if (selectedItem == ChangePlate)
        {
            Player.ChangePlate(ChangePlate.SelectedItem);
        }
        else if (selectedItem == RemovePlate)
        {
            Player.RemovePlate();
        }
        else if (selectedItem == SitDown)
        {
            if (SitDown.SelectedItem == "At Closest Seat")
            {
                Player.StartSittingDown(true, true);
            }
            else
            {
                if (SitDown.SelectedItem == "Here Backwards")
                {
                    Player.StartSittingDown(false, false);
                }
                else
                {
                    Player.StartSittingDown(false, true);
                }
            }
        }
        else if (selectedItem == LayDown)
        {
            if (LayDown.SelectedItem == "At Closest Bed")
            {
                Player.StartLayingDown(true);
            }
            else
            {
                Player.StartLayingDown(false);
            }
        }
        else if (selectedItem == EnterAsPassenger)
        {
            Player.EnterVehicleAsPassenger();
        }
        else if (selectedItem == ShuffleSeat)
        {
            Player.ShuffleToNextSeat();
        }
        else if (selectedItem == IntimidateDriver)
        {
            Player.ForceErraticDriver();
        }
        else if (selectedItem == CurrentActivityMenu)
        {
            if (CurrentActivityMenu.SelectedItem == "Continue")
            {
                Player.ContinueCurrentActivity();
            }
            else if (CurrentActivityMenu.SelectedItem == "Pause")
            {
                Player.PauseCurrentActivity();
            }
            else if (CurrentActivityMenu.SelectedItem == "Stop")
            {
                Player.StopDynamicActivity();
            }
        }
        else if (selectedItem == GestureMenu)
        {
            Player.Gesture(GestureMenu.SelectedItem);
        }
        else if (selectedItem == DanceMenu)
        {
            Player.Dance(DanceMenu.SelectedItem);
        }
        else if (selectedItem == CallPolice)
        {
            Player.CallPolice();
        }
        else if (selectedItem == ToggleBodyArmor)
        {
            Player.ToggleBodyArmor(ToggleBodyArmor.Value);
        }
        else if (selectedItem == HotwireVehicle)
        {
            Player.StartHotwiring();
        }

        else if (selectedItem == ToggleActionMode)
        {
            Player.Stance.ToggleActionMode();
        }
        else if (selectedItem == ToggleStealthMode)
        {
            Player.Stance.ToggleStealthMode();
        }
        Actions.Visible = false;
        ChangePlate.Items = Player.SpareLicensePlates;
    }
    private void OnScrollerChange(UIMenu sender, UIMenuScrollerItem item, int oldIndex, int newIndex)
    {
        if (item == ToggleBodyArmor)
        {
            Player.SetBodyArmor(ToggleBodyArmor.Value);
        }
    }
}