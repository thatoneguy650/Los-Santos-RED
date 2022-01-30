using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

public class ActionMenu : Menu
{
    private UIMenu Actions;
    private UIMenuListScrollerItem<LSR.Vehicles.LicensePlate> ChangePlate;
    private IActionable Player;
    private UIMenuItem RemovePlate;
    private UIMenuListScrollerItem<string> SitDown;
    private UIMenuListScrollerItem<string> LayDown;
    private UIMenuItem PauseConsuming;
    private UIMenuItem StopConsuming;
    private UIMenuItem Suicide;
    private ISettingsProvideable Settings;
    private UIMenuListScrollerItem<string> CurrentActivityMenu;
    private UIMenuItem EnterAsPassenger;
    private UIMenuItem ShuffleSeat;
    private UIMenuItem IntimidateDriver;
    private UIMenuItem HotwireVehicle;
    private UIMenuItem ContinueConsuming;
    private MenuPool MenuPool;
    private UIMenuListScrollerItem<GestureData> GestureMenu;
    private List<GestureData> GestureLookups;
    private UIMenuItem CallPolice;
    private UIMenuNumericScrollerItem<int> ToggleBodyArmor;

    public ActionMenu(MenuPool menuPool, UIMenu parentMenu, IActionable player, ISettingsProvideable settings)
    {
        Player = player;
        Settings = settings;
        MenuPool = menuPool;
        Actions = MenuPool.AddSubMenu(parentMenu, "Actions");
        Actions.SetBannerType(EntryPoint.LSRedColor);
        Actions.OnItemSelect += OnActionItemSelect;
        Actions.OnScrollerChange += OnScrollerChange;

        CreateActionsMenu();
    }



    public int SelectedPlateIndex { get; set; }
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

        if(Player.HasCurrentActivity)
        {
            CurrentActivityMenu.Enabled = true;
        }
        else
        {
            CurrentActivityMenu.Enabled = false;
        }

        if(Player.CharacterModelIsFreeMode)
        {
            ToggleBodyArmor.Enabled = true;
        }
        else
        {
            ToggleBodyArmor.Enabled = false;
        }

        //CreateActionsMenu();
        //if (Player.CanPerformActivities)
        //{
        //    //Suicide.Enabled = true;
        //    //ChangePlate.Enabled = true;
        //    //RemovePlate.Enabled = true;

        //    //if (Settings.SettingsManager.PlayerSettings.AllowConsumeWithoutInventory)
        //    //{
        //    //    Drink.Enabled = true;
        //    //    Smoke.Enabled = true;
        //    //    SmokePot.Enabled = true;
        //    //}
        //}
        //else
        //{
        //    //Suicide.Enabled = false;
        //    //ChangePlate.Enabled = false;
        //    //RemovePlate.Enabled = false;
        //    //if (Settings.SettingsManager.PlayerSettings.AllowConsumeWithoutInventory)
        //    //{
        //    //    Drink.Enabled = false;
        //    //    Smoke.Enabled = false;
        //    //    SmokePot.Enabled = false;
        //    //}
        //}
        //if (Player.IsPerformingActivity)
        //{
        //    StopConsuming.Enabled = true;
        //}
        //else
        //{
        //    StopConsuming.Enabled = false;
        //}
        ChangePlate.Items = Player.SpareLicensePlates;
    }
    private void CreateActionsMenu()
    {
        Setup();
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
        ChangePlate = new UIMenuListScrollerItem<LSR.Vehicles.LicensePlate>("Change Plate", "Change your license plate if you have spares.",Player.SpareLicensePlates);
        RemovePlate = new UIMenuItem("Remove Plate", "Remove the license plate.");
        SitDown = new UIMenuListScrollerItem<string>("Sit Down", "Sit down either at the nearest seat or where you are.", new List<string>() { "At Closest Seat", "Here Backwards", "Here Forwards" });
        LayDown = new UIMenuListScrollerItem<string>("Lay Down", "Lay down either at the nearest seat or where you are.", new List<string>() { "At Closest Bed", "Here" });
        GestureMenu = new UIMenuListScrollerItem<GestureData>("Gesture", "Perform the selected gesture", GestureLookups);
        CurrentActivityMenu = new UIMenuListScrollerItem<string>("Current Activity", "Continue, Pause, or Stop the Current Activity", new List<string>() { "Continue","Pause","Stop" });

        EnterAsPassenger = new UIMenuItem("Enter as Passenger", "Enter nearest vehicle as a passenger");
        ShuffleSeat = new UIMenuItem("Shuffle Seat", "Shuffles your current seat");

        IntimidateDriver = new UIMenuItem("Intimidate Driver", "Force driver to flee in the vehicle");
        HotwireVehicle = new UIMenuItem("Hotwire Vehicle", "Hotwire current vehicle");

        ToggleBodyArmor = new UIMenuNumericScrollerItem<int>("Toggle Body Armor", "Select to take toggle, scroll to change", 0, 18, 1);
        ToggleBodyArmor.Value = 0;

        Actions.AddItem(CurrentActivityMenu);
        Actions.AddItem(GestureMenu);
        Actions.AddItem(SitDown);
#if DEBUG
        Actions.AddItem(LayDown);
#endif
        //Actions.AddItem(CallPolice);
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
                if(SitDown.SelectedItem == "Here Backwards")
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
            if(CurrentActivityMenu.SelectedItem == "Continue")
            {
                Player.ContinueDynamicActivity();
            }
            else if (CurrentActivityMenu.SelectedItem == "Pause")
            {
                Player.PauseDynamicActivity();
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
        Actions.Visible = false;
        ChangePlate.Items = Player.SpareLicensePlates;
    }
    private void OnScrollerChange(UIMenu sender, UIMenuScrollerItem item, int oldIndex, int newIndex)
    {
        if(item == ToggleBodyArmor)
        {
            Player.SetBodyArmor(ToggleBodyArmor.Value);
        }
    }
    private void Setup()
    {
        GestureLookups = new List<GestureData>()
        {
            //new GestureLookup("The Finger","mp_player_intfinger","mp_player_intfinger"),
           // new GestureData("The Bird","anim@mp_player_intselfiethe_bird","idle_a"),//left hand middle finger close
            new GestureData("The Finger Quick","anim@mp_player_intselfiethe_bird","enter") {IsInsulting = true },//left hand middle finger close
            new GestureData("The Finger Alt Quick","mp_player_int_upperfinger","mp_player_int_finger_02_enter") {IsInsulting = true },//Both HAnds middle finger 
            new GestureData("Double Finger Quick","mp_player_int_upperfinger","mp_player_int_finger_01_enter") {IsInsulting = true },//Both HAnds middle finger 
            new GestureData("Thumbs Up Quick","anim@mp_player_intselfiethumbs_up","enter"),
            new GestureData("Wank Quick","anim@mp_player_intselfiewank","enter"),
            //new GestureData("Chin Brush Quick","anim@mp_player_intupperchin_brush","enter"),
# if DEBUG
            new GestureData("Wank Full","anim@mp_player_intselfiewank","idle_a","enter","exit"),
            new GestureData("Blow Kiss Full","anim@mp_player_intselfieblow_kiss","idle_a","enter","exit"),//looks weird
            new GestureData("Chicken Full","anim@mp_player_intupperchicken_taunt","idle_a","enter","exit"),
            new GestureData("Chin Brush Full","anim@mp_player_intupperchin_brush","idle_a","enter","exit"),
            new GestureData("Gang Signs Full","mp_player_int_uppergang_sign_a","mp_player_int_gang_sign_a","mp_player_int_gang_sign_a_enter","mp_player_int_gang_sign_a_exit"),//Both HAnds middle finger       
#endif
            new GestureData("Bring It On","gesture_bring_it_on") {IsInsulting = true },
            new GestureData("Bye (Hard)","gesture_bye_hard"),
            new GestureData("Bye (Soft)","gesture_bye_soft"),
            new GestureData("Come Here (Hard)","gesture_come_here_hard"),
            new GestureData("Come Here (Soft)","gesture_come_here_soft"),
            new GestureData("Damn","gesture_damn"),
            new GestureData("Displeased","gesture_displeased"),
            new GestureData("Easy Now","gesture_easy_now"),
            new GestureData("Hand Down","gesture_hand_down"),
            new GestureData("Hand Left","gesture_hand_left"),
            new GestureData("Hand Right","gesture_hand_right"),
            new GestureData("Head No","gesture_head_no"),
            new GestureData("Hello","gesture_hello"),
            new GestureData("I Will","gesture_i_will"),
            new GestureData("Me","gesture_me"),
            new GestureData("Me (Hard)","gesture_me_hard"),
            new GestureData("No Way","gesture_no_way"),
            new GestureData("Nod No (Hard)","gesture_nod_no_hard"),
            new GestureData("Nod No (Soft)","gesture_nod_no_soft"),
            new GestureData("Nod Yes (Hard)","gesture_nod_yes_hard"),
            new GestureData("Nod Yes (Soft)","gesture_nod_yes_soft"),
            new GestureData("Pleased","gesture_pleased"),
            new GestureData("Point","gesture_point"),
            new GestureData("Shrug (Hard)","gesture_shrug_hard"),
            new GestureData("Shrug (Soft)","gesture_shrug_soft"),
            new GestureData("What (Hard)","gesture_what_hard"),
            new GestureData("What (Soft)","gesture_what_soft"),
            new GestureData("Why","gesture_why"),
            new GestureData("Why Left","gesture_why_left"),
            new GestureData("You (Hard)","gesture_you_hard"),
            new GestureData("You (Soft)","gesture_you_soft"),
            new GestureData("Its Mine","getsure_its_mine"),
        };



        // new GestureData("Upper Finger","mp_player_int_upperfinger","mp_player_int_finger_01","mp_player_int_finger_01_enter","mp_player_int_finger_01_exit"),//Both HAnds middle finger 
        //  new GestureData("Upper Finger Enter Exit","mp_player_int_upperfinger","","mp_player_int_finger_01_enter","mp_player_int_finger_01_exit"),//Both HAnds middle finger 
        // new GestureData("Upper Finger Base","mp_player_int_upperfinger","mp_player_int_finger_01"),//Both HAnds middle finger 
        //  new GestureData("Upper Finger 2","mp_player_int_upperfinger","mp_player_int_finger_02","mp_player_int_finger_02_enter","mp_player_int_finger_02_exit"),//Both HAnds middle finger 
        //  new GestureData("Upper Finger 2 Enter Exit","mp_player_int_upperfinger","","mp_player_int_finger_02_enter","mp_player_int_finger_02_exit"),//Both HAnds middle finger 
        //
        //  new GestureData("Upper Finger 2 Base","mp_player_int_upperfinger","mp_player_int_finger_02"),//Both HAnds middle finger 
        //     new GestureData("Gang Signs","mp_player_int_uppergang_sign_a","mp_player_int_gang_sign_a","mp_player_int_gang_sign_a_enter","mp_player_int_gang_sign_a_exit"),//Both HAnds middle finger 
        //     new GestureData("Gang Signs Bare","mp_player_int_uppergang_sign_a","mp_player_int_gang_sign_a"),//Both HAnds middle finger 
        //new GestureLookup("Rock","mp_player_int_rock","mp_player_int_rock"),
        //new GestureLookup("Salute","mp_player_int_salute","mp_player_int_salute"),

    }


}