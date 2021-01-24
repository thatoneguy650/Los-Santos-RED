using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

//basically need to redo this entire class, it is a dumpster fire, indeed
public class Menu
{
    private readonly List<string> strRadioStations = new List<string>
    {
        "NONE", "RADIO_01_CLASS_ROCK", "RADIO_02_POP", "RADIO_03_HIPHOP_NEW", "RADIO_04_PUNK", "RADIO_05_TALK_01",
        "RADIO_06_COUNTRY", "RADIO_07_DANCE_01", "RADIO_08_MEXICAN", "RADIO_09_HIPHOP_OLD", "RADIO_12_REGGAE",
        "RADIO_13_JAZZ", "RADIO_14_DANCE_02", "RADIO_15_MOTOWN", "RADIO_20_THELAB", "RADIO_16_SILVERLAKE",
        "RADIO_17_FUNK", "RADIO_18_90S_ROCK", "RADIO_19_USER", "RADIO_11_TALK_02", "HIDDEN_RADIO_AMBIENT_TV_BRIGHT",
        "OFF"
    };
    private UIMenu actionsMenu;
    private UIMenu bustedMenu;
    private GameLocation CurrentSelectedHospitalLocation;
    private GameLocation CurrentSelectedSurrenderLocation;
    private UIMenu deathMenu;
    private UIMenu debugMenu;
    private UIMenu mainMenu;
    private UIMenuItem menuActionDrink;
    private UIMenuItem menuActionSmoke;
    private UIMenuItem menuActionSmokePot;
    private UIMenuItem menuActionStopConsumingActivity;
    private UIMenuItem menuActionSmoking;
    private UIMenuListItem menuAutoSetRadioStation;
    private UIMenuItem menuBustedBribe;
    private UIMenuItem menuBustedResistArrest;
    private UIMenuListItem menuBustedSurrender;
    private UIMenuListItem menuBustedTakeoverRandomPed;
    private UIMenuItem menuBustedTalk;
    private UIMenuListItem menuDeathHospitalRespawn;
    private UIMenuListItem menuDeathTakeoverRandomPed;
    private UIMenuItem menuDeathUndie;
    private UIMenuItem menuDebugGiveMoney;
    private UIMenuItem menuDebugHealthAndArmor;
    private UIMenuItem menuDebugKillPlayer;
    private UIMenuItem menuDebugRandomVariation;
    private UIMenuListItem menuDebugRandomWeapon;
    private UIMenuItem menuDebugResetCharacter;
    private UIMenuItem menuDebugResetMod;
    private UIMenuItem menuMainChangeLicensePlate;
    private UIMenuItem menuMainRemoveLicensePlate;
    private UIMenuItem menuMainShowPlayerStatus;
    private UIMenuItem menuMainSuicide;
    private UIMenuListItem menuMainTakeoverRandomPed;
    private MenuPool menuPool;
    private UIMenu optionsMenu;
    private IPedswappable PedSwap;
    private IPlacesOfInterest PlacesOfInterest;
    private IActionable Player;
    private int RandomWeaponCategory;
    private UIMenuItem ReloadSettings;
    private IRespawning Respawning;
    private UIMenuItem scenariosMainPrisonEscape;
    private UIMenu scenariosMenu;
    private ISettingsProvideable Settings;
    private UIMenu settingsMenuGeneral;
    private UIMenu settingsMenuKeySettings;
    private UIMenu settingsMenuPolice;
    private UIMenu settingsMenuTrafficViolations;
    private UIMenu settingsMenuUISettings;
    private IWeapons Weapons;
    public Menu(IActionable player, IPedswappable pedSwap, IRespawning respawning, ISettingsProvideable settings, IWeapons weapons, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        PedSwap = pedSwap;
        Respawning = respawning;
        Settings = settings;
        Weapons = weapons;
        PlacesOfInterest = placesOfInterest;
        menuPool = new MenuPool();
        mainMenu = new UIMenu("Los Santos RED", "Select an Option");
        menuPool.Add(mainMenu);
        deathMenu = new UIMenu("Wasted", "Choose Respawn");
        menuPool.Add(deathMenu);
        debugMenu = new UIMenu("Debug", "Debug Settings");
        menuPool.Add(debugMenu);
        bustedMenu = new UIMenu("Busted", "Choose Respawn");
        menuPool.Add(bustedMenu);
        actionsMenu = menuPool.AddSubMenu(mainMenu, "Actions");
        optionsMenu = menuPool.AddSubMenu(mainMenu, "Options");
        scenariosMenu = menuPool.AddSubMenu(mainMenu, "Scenarios");
        mainMenu.OnItemSelect += MainMenuSelect;
        mainMenu.OnListChange += OnListChange;
        mainMenu.OnCheckboxChange += OnCheckboxChange;
        deathMenu.OnItemSelect += DeathMenuSelect;
        deathMenu.OnListChange += OnListChange;
        bustedMenu.OnItemSelect += BustedMenuSelect;
        bustedMenu.OnListChange += OnListChange;
        debugMenu.OnItemSelect += DebugMenuSelect;
        debugMenu.OnListChange += OnListChange;
        debugMenu.OnCheckboxChange += OnCheckboxChange;
        //Player.Busted += OnBusted;
        //Player.Killed += OnDeath;
    }
    private void OnDeath(object sender, EventArgs e)
    {
        GameFiber Transition = GameFiber.StartNew(delegate
        {
            GameFiber.Sleep(2000);
            ShowDeathMenu();
        }, "TransitionIn");
    }
    private void OnBusted(object sender, EventArgs e)
    {
        GameFiber Transition = GameFiber.StartNew(delegate
        {
            GameFiber.Sleep(2000);
            ShowBustedMenu();
        }, "TransitionIn");
    }
    public string CurrentScreenEffect { get; set; }
    public int SelectedPlateIndex { get; set; }
    public float SelectedTakeoverRadius { get; set; }
    public void ShowBustedMenu()
    {
        if (Player.IsDead)
        {
            return;
        }
        CreateBustedMenu();
        mainMenu.Visible = false;
        deathMenu.Visible = false;
        debugMenu.Visible = false;
        optionsMenu.Visible = false;
        actionsMenu.Visible = false;
        scenariosMenu.Visible = false;
        bustedMenu.Visible = true;
    }
    public void ShowDeathMenu()
    {
        CreateDeathMenu();
        mainMenu.Visible = false;
        debugMenu.Visible = false;
        bustedMenu.Visible = false;
        optionsMenu.Visible = false;
        actionsMenu.Visible = false;
        scenariosMenu.Visible = false;
        deathMenu.Visible = true;
    }
    public void ShowDebugMenu()
    {
        CreateDebugMenu();
        mainMenu.Visible = false;
        deathMenu.Visible = false;
        optionsMenu.Visible = false;
        actionsMenu.Visible = false;
        scenariosMenu.Visible = false;
        debugMenu.Visible = true;
    }
    public void ShowMainMenu()
    {
        CreateMainMenu();
        mainMenu.Visible = true;
    }
    public void Update()
    {
        if (Game.IsKeyDown(Settings.SettingsManager.KeyBinding.MenuKey)) // Our menu on/off switch.
        {
            if (Player.IsDead)
            {
                if (!deathMenu.Visible)
                {
                    ShowDeathMenu();
                }
                else
                {
                    deathMenu.Visible = false;
                }
            }
            else if (Player.IsBusted)
            {
                if (!bustedMenu.Visible)
                {
                    ShowBustedMenu();
                }
                else
                {
                    bustedMenu.Visible = false;
                }
            }
            else if (optionsMenu.Visible)
            {
                optionsMenu.Visible = !optionsMenu.Visible;
            }
            else if (actionsMenu.Visible)
            {
                actionsMenu.Visible = !actionsMenu.Visible;
            }
            else if (scenariosMenu.Visible)
            {
                scenariosMenu.Visible = !scenariosMenu.Visible;
            }
            else
            {
                if (!mainMenu.Visible)
                {
                    ShowMainMenu();
                }
                else
                {
                    mainMenu.Visible = false;
                }
            }
        }
        else if (Game.IsKeyDown(Settings.SettingsManager.KeyBinding.DebugMenuKey)) // Our menu on/off switch.
        {
            if (!debugMenu.Visible)
            {
                ShowDebugMenu();
            }
            else
            {
                debugMenu.Visible = false;
            }
        }
        menuPool.ProcessMenus();
    }
    private void ActionsMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == menuMainSuicide)
        {
            Player.CommitSuicide();
        }
        else if (selectedItem == menuMainChangeLicensePlate)
        {
            Player.ChangePlate();
        }
        else if (selectedItem == menuMainRemoveLicensePlate)
        {
            Player.RemovePlate();
        }
        else if (selectedItem == menuActionDrink)
        {
            Player.DrinkBeer();
        }
        else if (selectedItem == menuActionSmoke)
        {
            Player.StartSmoking();
        }
        else if (selectedItem == menuActionSmokePot)
        {
            Player.StartSmokingPot();
        }
        else if (selectedItem == menuActionStopConsumingActivity)
        {
            Player.StopConsumingActivity();
        }
    }
    private void BustedMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == menuBustedResistArrest)
        {
            Respawning.ResistArrest();
        }
        else if (selectedItem == menuBustedBribe)
        {
            if (int.TryParse(GetKeyboardInput(""), out int BribeAmount))
            {
                Respawning.BribePolice(BribeAmount);
            }
        }
        if (selectedItem == menuBustedSurrender)
        {
            Respawning.SurrenderToPolice(CurrentSelectedSurrenderLocation);
        }
        else if (selectedItem == menuBustedTakeoverRandomPed)
        {
            if (SelectedTakeoverRadius == -1f)
            {
                PedSwap.TakeoverPed(500f, true, true, true);
            }
            else
            {
                PedSwap.TakeoverPed(SelectedTakeoverRadius, false, true, true);
            }
        }
        bustedMenu.Visible = false;
    }
    private void CreateActionsMenu()
    {
        menuMainSuicide = new UIMenuItem("Suicide", "Commit Suicide");
        menuMainChangeLicensePlate = new UIMenuItem("Change Plate", "Change your license plate if you have spares.");
        menuMainRemoveLicensePlate = new UIMenuItem("Remove Plate", "Remove the license plate.");
        menuActionDrink = new UIMenuItem("Drink", "Start Drinking");
        menuActionSmoke = new UIMenuItem("Smoke", "Start Smoking");
        menuActionSmokePot = new UIMenuItem("Smoke Pot", "Start Smoking Pot");


        menuActionStopConsumingActivity = new UIMenuItem("Stop", "Stop Consuming Activity");

        actionsMenu.AddItem(menuMainSuicide);
        if (!Player.IsInVehicle)
        {
            actionsMenu.AddItem(menuMainChangeLicensePlate);
            actionsMenu.AddItem(menuMainRemoveLicensePlate);
            actionsMenu.AddItem(menuActionDrink);
            actionsMenu.AddItem(menuActionSmoke);
            actionsMenu.AddItem(menuActionSmokePot);
            if (Player.IsConsuming)
            {
                actionsMenu.AddItem(menuActionStopConsumingActivity);
            }
        }
        actionsMenu.OnItemSelect += ActionsMenuSelect;
        actionsMenu.OnListChange += OnListChange;
        actionsMenu.OnCheckboxChange += OnCheckboxChange;
    }
    private void CreateBustedMenu()
    {
        bustedMenu.Clear();
        menuBustedResistArrest = new UIMenuItem("Resist Arrest", "Better hope you're strapped.");
        menuBustedBribe = new UIMenuItem("Bribe Police", "Bribe the police to let you go. Don't be cheap.");
        menuBustedSurrender = new UIMenuListItem("Surrender","Surrender and get out on bail. Lose bail money and your guns.",PlacesOfInterest.GetLocations(LocationType.Police));
        menuBustedTakeoverRandomPed = new UIMenuListItem("Takeover Random Pedestrian","Takes over a random pedestrian around the player.", new List<dynamic> { "Closest", "20 M", "40 M", "60 M", "100 M", "500 M" });
        UpdateClosestPoliceStationIndex();
        bustedMenu.AddItem(menuBustedResistArrest);
        bustedMenu.AddItem(menuBustedBribe);
        bustedMenu.AddItem(menuBustedSurrender);
        bustedMenu.AddItem(menuBustedTakeoverRandomPed);
    }
    private void CreateDeathMenu()
    {
        deathMenu.Clear();
        menuDeathUndie = new UIMenuItem("Un-Die", "Respawn at this exact spot as yourself.");
        menuDeathHospitalRespawn = new UIMenuListItem("Give Up","Respawn at the nearest hospital. Lose a hospital fee and your guns.",PlacesOfInterest.GetLocations(LocationType.Hospital));
        menuDeathTakeoverRandomPed = new UIMenuListItem("Takeover Random Pedestrian","Takes over a random pedestrian around the player.", new List<dynamic> { "Closest", "20 M", "40 M", "60 M", "100 M", "500 M" });
        UpdateClosestHospitalIndex();
        deathMenu.AddItem(menuDeathUndie);
        deathMenu.AddItem(menuDeathHospitalRespawn);
        deathMenu.AddItem(menuDeathTakeoverRandomPed);
    }
    private void CreateDebugMenu()
    {
        debugMenu.Clear();
        menuDebugResetCharacter = new UIMenuItem("Reset Character", "Change your character back to the default model.");
        menuDebugKillPlayer = new UIMenuItem("Kill Player", "Immediatly die and ragdoll");
        menuDebugRandomWeapon = new UIMenuListItem("Get Random Weapon", "Gives the Player a random weapon and ammo.",Enum.GetNames(typeof(WeaponCategory)).ToList());
        menuDebugRandomVariation = new UIMenuItem("Apply Random Variation", "Add some cool stuff to your gun");
        menuDebugGiveMoney = new UIMenuItem("Get Money", "Give you some cash");
        menuDebugHealthAndArmor = new UIMenuItem("Health and Armor", "Get loaded for bear");
        menuAutoSetRadioStation = new UIMenuListItem("Auto-Set Station","Will auto set the station any time the radio is on", strRadioStations);
        menuDebugResetMod = new UIMenuItem("Reset Mod", "Reloads Mod and reads from the XML");
        debugMenu.AddItem(menuDebugResetCharacter);
        debugMenu.AddItem(menuDebugKillPlayer);
        debugMenu.AddItem(menuDebugRandomWeapon);
        debugMenu.AddItem(menuDebugRandomVariation);
        debugMenu.AddItem(menuDebugGiveMoney);
        debugMenu.AddItem(menuDebugHealthAndArmor);
        debugMenu.AddItem(menuAutoSetRadioStation);
        debugMenu.AddItem(menuDebugResetMod);
    }
    private void CreateMainMenu()
    {
        mainMenu.Clear();
        menuMainTakeoverRandomPed = new UIMenuListItem("Takeover Random Pedestrian","Takes over a random pedestrian around the player.", new List<dynamic> { "Closest", "100 M", "500 M" });
        menuMainShowPlayerStatus = new UIMenuItem("Show Status", "Show the player status with a notification");
        mainMenu.AddItem(menuMainTakeoverRandomPed);
        mainMenu.AddItem(menuMainShowPlayerStatus);
        actionsMenu = menuPool.AddSubMenu(mainMenu, "Actions");
        optionsMenu = menuPool.AddSubMenu(mainMenu, "Options");
        scenariosMenu = menuPool.AddSubMenu(mainMenu, "Scenarios");
        CreateActionsMenu();
        CreateOptionsMenu();
        CreateScenariosMenu();
    }
    private void CreateOptionsMenu()
    {
        ReloadSettings = new UIMenuItem("Reload Settings", "Reload All settings from XML");
        optionsMenu.AddItem(ReloadSettings);
        settingsMenuGeneral = menuPool.AddSubMenu(optionsMenu, "General Settings");
        settingsMenuPolice = menuPool.AddSubMenu(optionsMenu, "Police Settings");
        settingsMenuUISettings = menuPool.AddSubMenu(optionsMenu, "UI Settings");
        settingsMenuKeySettings = menuPool.AddSubMenu(optionsMenu, "Key Settings");
        settingsMenuTrafficViolations = menuPool.AddSubMenu(optionsMenu, "Traffic Violations Settings");
        CreateSettingSubMenu(typeof(GeneralSettings).GetFields(), Settings.SettingsManager.General, settingsMenuGeneral);
        CreateSettingSubMenu(typeof(PoliceSettings).GetFields(), Settings.SettingsManager.Police, settingsMenuPolice);
        CreateSettingSubMenu(typeof(UISettings).GetFields(), Settings.SettingsManager.UI, settingsMenuUISettings);
        CreateSettingSubMenu(typeof(KeySettings).GetFields(), Settings.SettingsManager.KeyBinding, settingsMenuKeySettings);
        CreateSettingSubMenu(typeof(TrafficSettings).GetFields(), Settings.SettingsManager.TrafficViolations,settingsMenuTrafficViolations);
        optionsMenu.OnItemSelect += OptionsMenuSelect;
        optionsMenu.OnListChange += OnListChange;
        optionsMenu.OnCheckboxChange += OnCheckboxChange;
        settingsMenuGeneral.OnItemSelect += SettingsMenuSelect;
        settingsMenuPolice.OnItemSelect += SettingsMenuSelect;
        settingsMenuUISettings.OnItemSelect += SettingsMenuSelect;
        settingsMenuKeySettings.OnItemSelect += SettingsMenuSelect;
        settingsMenuTrafficViolations.OnItemSelect += SettingsMenuSelect;
    }
    private void CreateScenariosMenu()
    {
        scenariosMainPrisonEscape = new UIMenuItem("Prison Escape", "Escape the prison");
        scenariosMenu.AddItem(scenariosMainPrisonEscape);
        scenariosMenu.OnItemSelect += ScenarioMenuSelect;
        scenariosMenu.OnListChange += OnListChange;
        scenariosMenu.OnCheckboxChange += OnCheckboxChange;
    }
    private void CreateSettingSubMenu(FieldInfo[] Fields, object SettingsSubType, UIMenu MenuToSet)
    {
        foreach (FieldInfo fi in Fields)
        {
            if (fi.FieldType == typeof(bool))
            {
                UIMenuCheckboxItem MySetting = new UIMenuCheckboxItem(fi.Name, (bool)fi.GetValue(SettingsSubType));
                MenuToSet.AddItem(MySetting);
            }
            if (fi.FieldType == typeof(int) || fi.FieldType == typeof(string) || fi.FieldType == typeof(float))
            {
                UIMenuItem MySetting = new UIMenuItem(string.Format("{0}: {1}", fi.Name, fi.GetValue(SettingsSubType)));
                MenuToSet.AddItem(MySetting);
            }
        }
    }
    private void DeathMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == menuDeathUndie)
        {
            Respawning.RespawnAtCurrentLocation(true, false);
        }
        if (selectedItem == menuDeathHospitalRespawn)
        {
            if (RandomItems.RandomPercent(0))//turned off for testing
            {
                Respawning.RespawnAtHospital(CurrentSelectedHospitalLocation);
            }
            else
            {
                Respawning.RespawnAtGrave();
            }
        }
        else if (selectedItem == menuDeathTakeoverRandomPed)
        {
            if (SelectedTakeoverRadius == -1f)
            {
                PedSwap.TakeoverPed(500f, true, true, true);
            }
            else
            {
                PedSwap.TakeoverPed(SelectedTakeoverRadius, false, true, true);
            }
        }

        deathMenu.Visible = false;
    }
    private void DebugMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == menuDebugKillPlayer)
        {
            Game.LocalPlayer.Character.Kill();
        }
        else if (selectedItem == menuDebugRandomWeapon)
        {
            WeaponInformation myGun = Weapons.GetRandomRegularWeapon((WeaponCategory)RandomWeaponCategory);
            if (myGun != null)
            {
                Game.LocalPlayer.Character.Inventory.GiveNewWeapon(myGun.ModelName, myGun.AmmoAmount, true);
            }
        }
        else if (selectedItem == menuDebugGiveMoney)
        {
            Player.GiveMoney(50000);
        }
        else if (selectedItem == menuDebugHealthAndArmor)
        {
            Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
            Game.LocalPlayer.Character.Armor = 100;
        }
        debugMenu.Visible = false;
    }
    private string GetKeyboardInput(string DefaultText)
    {
        NativeFunction.Natives.DISPLAY_ONSCREEN_KEYBOARD<bool>(true, "FMMC_KEY_TIP8", "", DefaultText, "", "", "", 255 + 1);
        while (NativeFunction.Natives.UPDATE_ONSCREEN_KEYBOARD<int>() == 0)
        {
            GameFiber.Sleep(500);
        }
        string Value;
        IntPtr ptr = NativeFunction.Natives.GET_ONSCREEN_KEYBOARD_RESULT<IntPtr>();
        Value = Marshal.PtrToStringAnsi(ptr);
        return Value;
    }
    private void MainMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == menuMainTakeoverRandomPed)
        {
            if (SelectedTakeoverRadius == -1f)
            {
                PedSwap.TakeoverPed(500f, true, false, true);
            }
            else
            {
                PedSwap.TakeoverPed(SelectedTakeoverRadius, false, false, true);
            }
        }
        else if (selectedItem == menuMainShowPlayerStatus)
        {
            Player.DisplayPlayerNotification();
        }
        mainMenu.Visible = false;
    }
    private void OnCheckboxChange(UIMenu sender, UIMenuCheckboxItem checkbox, bool Checked)
    {
        if (sender == optionsMenu)
        {
            FieldInfo[] MyFields = Type.GetType("Settings", false).GetFields();
            FieldInfo MySetting = MyFields.Where(x => x.Name == checkbox.Text).FirstOrDefault();
            MySetting.SetValue(null, Checked);
        }
    }
    private void OnListChange(UIMenu sender, UIMenuListItem list, int index)
    {
        if (sender == mainMenu)
        {
            if (list == menuMainTakeoverRandomPed)
            {
                if (index == 0)
                {
                    SelectedTakeoverRadius = -1f;
                }
                else if (index == 1)
                {
                    SelectedTakeoverRadius = 100f;
                }
                else if (index == 2)
                {
                    SelectedTakeoverRadius = 500f;
                }
            }
            else if (list == menuMainChangeLicensePlate)
            {
                SelectedPlateIndex = index;
            }
        }
        else if (sender == deathMenu)
        {
            if (list == menuDeathHospitalRespawn)
            {
                CurrentSelectedHospitalLocation = PlacesOfInterest.GetLocations(LocationType.Hospital)[index];
            }
        }
        else if (sender == bustedMenu)
        {
            if (list == menuBustedSurrender)
            {
                CurrentSelectedSurrenderLocation = PlacesOfInterest.GetLocations(LocationType.Police)[index];
            }
        }
        else if (sender == debugMenu)
        {
            if (list == menuDebugRandomWeapon)
            {
                RandomWeaponCategory = list.Index;
            }
        }
    }
    private void OptionsMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {


    }
    private void ScenarioMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {

    }
    private void SettingsMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        string MySettingName = selectedItem.Text.Split(':')[0];
        FieldInfo[] MyFields = typeof(GeneralSettings).GetFields();
        object ToSet = Settings.SettingsManager.General;

        if (sender == settingsMenuGeneral)
        {
            MyFields = typeof(GeneralSettings).GetFields();
        }
        else if (sender == settingsMenuPolice)
        {
            ToSet = Settings.SettingsManager.Police;
            MyFields = typeof(PoliceSettings).GetFields();
        }
        else if (sender == settingsMenuKeySettings)
        {
            ToSet = Settings.SettingsManager.KeyBinding;
            MyFields = typeof(KeySettings).GetFields();
        }
        else if (sender == settingsMenuUISettings)
        {
            ToSet = Settings.SettingsManager.UI;
            MyFields = typeof(UISettings).GetFields();
        }
        else if (sender == settingsMenuTrafficViolations)
        {
            ToSet = Settings.SettingsManager.TrafficViolations;
            MyFields = typeof(TrafficSettings).GetFields();
        }

        FieldInfo MySetting = MyFields.FirstOrDefault(x => x.Name == MySettingName);
        if (MySetting == null)
            return;
        string Value = GetKeyboardInput(MySetting.GetValue(null).ToString());
        if (MySetting.FieldType == typeof(float))
        {
            if (float.TryParse(Value, out float myFloat))
            {
                MySetting.SetValue(ToSet, myFloat);
                selectedItem.Text = $"{MySettingName}: {Value}";
            }
        }
        else if (MySetting.FieldType == typeof(int))
        {
            if (int.TryParse(Value, out int myInt))
            {
                MySetting.SetValue(ToSet, myInt);
                selectedItem.Text = $"{MySettingName}: {Value}";
            }
        }
        else if (MySetting.FieldType == typeof(string))
        {
            MySetting.SetValue(ToSet, Value);
            selectedItem.Text = $"{MySettingName}: {Value}";
        }
        else if (MySetting.FieldType == typeof(Keys))
        {
            MySetting.SetValue(ToSet, Value);
            selectedItem.Text = $"{MySettingName}: {Value}";
        }

        Settings.SerializeAllSettings();
    }
    private void UpdateClosestHospitalIndex()
    {
        menuDeathHospitalRespawn.Index = PlacesOfInterest.GetLocations(LocationType.Hospital).IndexOf(PlacesOfInterest.GetClosestLocation(Game.LocalPlayer.Character.Position, LocationType.Hospital));
    }
    private void UpdateClosestPoliceStationIndex()
    {
        menuBustedSurrender.Index = PlacesOfInterest.GetLocations(LocationType.Police).IndexOf(PlacesOfInterest.GetClosestLocation(Game.LocalPlayer.Character.Position, LocationType.Police));
    }
}