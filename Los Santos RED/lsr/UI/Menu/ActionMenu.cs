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
    public ActionMenu(MenuPool menuPool, UIMenu parentMenu, IActionable player, ISettingsProvideable settings, IDances dances)
    {
        Player = player;
        Settings = settings;
        MenuPool = menuPool;
        Dances = dances;
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
        ChangePlate = new UIMenuListScrollerItem<LSR.Vehicles.LicensePlate>("Change Plate", "Change your license plate if you have spares.", Player.SpareLicensePlates);
        RemovePlate = new UIMenuItem("Remove Plate", "Remove the license plate.");
        SitDown = new UIMenuListScrollerItem<string>("Sit Down", "Sit down either at the nearest seat or where you are.", new List<string>() { "At Closest Seat", "Here Backwards", "Here Forwards" });
        LayDown = new UIMenuListScrollerItem<string>("Lay Down", "Lay down either at the nearest seat or where you are.", new List<string>() { "At Closest Bed", "Here" });
        GestureMenu = new UIMenuListScrollerItem<GestureData>("Gesture", "Perform the selected gesture", GestureLookups);

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

#if DEBUG
        Actions.AddItem(LayDown);
#endif
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
            if (!Settings.SettingsManager.ActivitySettings.CloseMenuOnGesture)
            {
                return;
            }
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
            Player.ToggleActionMode();
            //bool isUsingActionMode = NativeFunction.Natives.IS_PED_USING_ACTION_MODE<bool>(Player.Character);
            //NativeFunction.Natives.SET_PED_USING_ACTION_MODE(Player.Character, !isUsingActionMode, -1, "DEFAULT_ACTION");
        }
        else if (selectedItem == ToggleStealthMode)
        {
            Player.ToggleStealthMode();
            //bool isUsingStealthMode = NativeFunction.Natives.GET_PED_STEALTH_MOVEMENT<bool>(Player.Character);
            //NativeFunction.Natives.SET_PED_STEALTH_MOVEMENT(Player.Character, !isUsingStealthMode, "DEFAULT_ACTION");
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
    private void Setup()
    {
        Player.LastGesture = new GestureData("Thumbs Up Quick", "anim@mp_player_intselfiethumbs_up", "enter");
        GestureLookups = new List<GestureData>()
        {
            new GestureData("The Finger Quick","anim@mp_player_intselfiethe_bird","enter") {IsInsulting = true },//left hand middle finger close
            new GestureData("The Finger Alt Quick","mp_player_int_upperfinger","mp_player_int_finger_02_enter") {IsInsulting = true },//Both HAnds middle finger
            new GestureData("Double Finger Quick","mp_player_int_upperfinger","mp_player_int_finger_01_enter") {IsInsulting = true },//Both HAnds middle finger
            new GestureData("Thumbs Up Quick","anim@mp_player_intselfiethumbs_up","enter"),
            new GestureData("Wank Quick","anim@mp_player_intselfiewank","enter"),
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
        
//        DanceLookups = new List<DanceData>()
//        {
//new DanceData("dance_m_default","missfbi3_sniping","dance_m_default"),
//new DanceData("priv_dance_p1","mini@strip_club@private_dance@part1","priv_dance_p1"),
//new DanceData("mi_dance_facedj_15_v1_male^4","anim@amb@nightclub@dancers@dixon_entourage@","mi_dance_facedj_15_v1_male^4"),
//new DanceData("hi_dance_facedj_17_v2_female^2","anim@amb@nightclub@dancers@podium_dancers@","hi_dance_facedj_17_v2_female^2"),
//new DanceData("hi_dance_facedj_17_v2_male^5","anim@amb@nightclub@dancers@podium_dancers@","hi_dance_facedj_17_v2_male^5"),
//new DanceData("mi_dance_facedj_17_v1_female^1","anim@amb@nightclub@dancers@solomun_entourage@","mi_dance_facedj_17_v1_female^1"),
//new DanceData("mi_dance_crowd_13_v2_female^4","anim@amb@nightclub@dancers@tale_of_us_entourage@","mi_dance_crowd_13_v2_female^4"),
//new DanceData("ped_a_dance_idle","anim@amb@nightclub@mini@dance@dance_paired@dance_a@","ped_a_dance_idle"),
//new DanceData("ped_b_dance_idle","anim@amb@nightclub@mini@dance@dance_paired@dance_a@","ped_b_dance_idle"),
//new DanceData("ped_a_dance_idle","anim@amb@nightclub@mini@dance@dance_paired@dance_b@","ped_a_dance_idle"),
//new DanceData("ped_b_dance_idle","anim@amb@nightclub@mini@dance@dance_paired@dance_b@","ped_b_dance_idle"),
//new DanceData("ped_a_dance_idle","anim@amb@nightclub@mini@dance@dance_paired@dance_d@","ped_a_dance_idle"),
//new DanceData("ped_b_dance_idle","anim@amb@nightclub@mini@dance@dance_paired@dance_d@","ped_b_dance_idle"),
//new DanceData("ped_a_dance_idle","anim@amb@nightclub@mini@dance@dance_paired@dance_e@","ped_a_dance_idle"),
//new DanceData("ped_b_dance_idle","anim@amb@nightclub@mini@dance@dance_paired@dance_e@","ped_b_dance_idle"),
//new DanceData("ped_a_dance_idle","anim@amb@nightclub@mini@dance@dance_paired@dance_f@","ped_a_dance_idle"),
//new DanceData("ped_b_dance_idle","anim@amb@nightclub@mini@dance@dance_paired@dance_f@","ped_b_dance_idle"),
//new DanceData("hi_idle_a_f02","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_f02"),
//new DanceData("hi_idle_a_f01","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_f01"),
//new DanceData("hi_idle_a_m01","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_m01"),
//new DanceData("hi_idle_a_m02","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_m02"),
//new DanceData("hi_idle_a_m03","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_m03"),
//new DanceData("hi_idle_a_m04","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_m04"),
//new DanceData("hi_idle_a_m05","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_a_m05"),
//new DanceData("hi_idle_b_f01","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_f01"),
//new DanceData("hi_idle_b_f02","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_f02"),
//new DanceData("hi_idle_b_m01","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_m01"),
//new DanceData("hi_idle_b_m02","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_m02"),
//new DanceData("hi_idle_b_m03","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_m03"),
//new DanceData("hi_idle_b_m04","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_m04"),
//new DanceData("hi_idle_b_m05","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_b_m05"),
//new DanceData("hi_idle_c_f01","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_f01"),
//new DanceData("hi_idle_c_f02","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_f02"),
//new DanceData("hi_idle_c_m01","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_m01"),
//new DanceData("hi_idle_c_m02","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_m02"),
//new DanceData("hi_idle_c_m03","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_m03"),
//new DanceData("hi_idle_c_m04","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_m04"),
//new DanceData("hi_idle_c_m05","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_c_m05"),
//new DanceData("hi_idle_d_f01","anim@amb@nightclub_island@dancers@beachdance@","hi_idle_d_f01"),
//new DanceData("hi_idle_a_f01","anim@amb@nightclub_island@dancers@club@","hi_idle_a_f01"),
//new DanceData("hi_idle_a_f02","anim@amb@nightclub_island@dancers@club@","hi_idle_a_f02"),
//new DanceData("hi_idle_a_f03","anim@amb@nightclub_island@dancers@club@","hi_idle_a_f03"),
//new DanceData("hi_idle_a_m01","anim@amb@nightclub_island@dancers@club@","hi_idle_a_m01"),
//new DanceData("hi_idle_a_m02","anim@amb@nightclub_island@dancers@club@","hi_idle_a_m02"),
//new DanceData("hi_idle_a_m03","anim@amb@nightclub_island@dancers@club@","hi_idle_a_m03"),
//new DanceData("hi_idle_b_f01","anim@amb@nightclub_island@dancers@club@","hi_idle_b_f01"),
//new DanceData("hi_idle_b_f02","anim@amb@nightclub_island@dancers@club@","hi_idle_b_f02"),

//        };

//        Player.LastDance = DanceLookups.PickRandom();
    }
}