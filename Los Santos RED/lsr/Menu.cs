using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ExtensionsMethods;
using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;

public class Menu
{
    private UIMenuListItem menuMainTakeoverRandomPed;
    private UIMenuItem menuDebugResetCharacter;
    private UIMenuItem menuMainSuicide;
    private UIMenuItem menuMainChangeLicensePlate;
    private UIMenuItem menuMainRemoveLicensePlate;
    private UIMenuItem menuMainShowPlayerStatus;
    private UIMenuItem menuMainChangeHelmet;
    private UIMenuItem menuDebugKillPlayer;
    private UIMenuListItem menuDebugRandomWeapon;
    private UIMenuItem menuDebugRandomVariation;
    private UIMenuListItem menuDebugScreenEffect;

    private UIMenuItem menuDeathUndie;
    private UIMenuListItem menuDeathTakeoverRandomPed;
    private UIMenuItem menuBustedResistArrest;
    private UIMenuItem menuBustedBribe;
    private UIMenuItem menuBustedTalk;
    private UIMenuListItem menuBustedTakeoverRandomPed;
    private UIMenuListItem menuBustedSurrender;
    private UIMenuListItem menuDeathHospitalRespawn;
    private UIMenuItem menuDebugGiveMoney;
    private UIMenuItem menuDebugHealthAndArmor;
    private UIMenuItem menuActionSmoking;
    private UIMenuListItem menuAutoSetRadioStation;
    private UIMenuItem menuDebugResetMod;
    private UIMenuItem ReloadSettings;

    private MenuPool menuPool;
    private UIMenu mainMenu;
    private UIMenu deathMenu;
    private UIMenu debugMenu;
    private UIMenu bustedMenu;
    private UIMenu optionsMenu;
    private UIMenu actionsMenu;
    private UIMenu scenariosMenu;


    private UIMenu settingsMenuGeneral;
    private UIMenu settingsMenuPolice;
    private UIMenu settingsMenuUISettings;
    private UIMenu settingsMenuKeySettings;
    private UIMenu settingsMenuTrafficViolations;
    private int RandomWeaponCategory;

    private GameLocation CurrentSelectedSurrenderLocation;
    private GameLocation CurrentSelectedHospitalLocation;
    private UIMenuItem scenariosMainPrisonEscape;


    private readonly List<string> strRadioStations = new List<string>
    {
        "NONE", "RADIO_01_CLASS_ROCK", "RADIO_02_POP", "RADIO_03_HIPHOP_NEW", "RADIO_04_PUNK", "RADIO_05_TALK_01",
        "RADIO_06_COUNTRY", "RADIO_07_DANCE_01", "RADIO_08_MEXICAN", "RADIO_09_HIPHOP_OLD", "RADIO_12_REGGAE",
        "RADIO_13_JAZZ", "RADIO_14_DANCE_02", "RADIO_15_MOTOWN", "RADIO_20_THELAB", "RADIO_16_SILVERLAKE",
        "RADIO_17_FUNK", "RADIO_18_90S_ROCK", "RADIO_19_USER", "RADIO_11_TALK_02", "HIDDEN_RADIO_AMBIENT_TV_BRIGHT",
        "OFF"
    };

    private readonly List<string> ScreenEffects = new List<string>
    {
        "SwitchHUDIn",
        "SwitchHUDOut",
        "FocusIn",
        "FocusOut",
        "MinigameEndNeutral",
        "MinigameEndTrevor",
        "MinigameEndFranklin",
        "MinigameEndMichael",
        "MinigameTransitionOut",
        "MinigameTransitionIn",
        "SwitchShortNeutralIn",
        "SwitchShortFranklinIn",
        "SwitchShortTrevorIn",
        "SwitchShortMichaelIn",
        "SwitchOpenMichaelIn",
        "SwitchOpenFranklinIn",
        "SwitchOpenTrevorIn",
        "SwitchHUDMichaelOut",
        "SwitchHUDFranklinOut",
        "SwitchHUDTrevorOut",
        "SwitchShortFranklinMid",
        "SwitchShortMichaelMid",
        "SwitchShortTrevorMid",
        "DeathFailOut",
        "CamPushInNeutral",
        "CamPushInFranklin",
        "CamPushInMichael",
        "CamPushInTrevor",
        "SwitchSceneFranklin",
        "SwitchSceneTrevor",
        "SwitchSceneMichael",
        "SwitchSceneNeutral",
        "MP_Celeb_Win",
        "MP_Celeb_Win_Out",
        "MP_Celeb_Lose",
        "MP_Celeb_Lose_Out",
        "DeathFailNeutralIn",
        "DeathFailMPDark",
        "DeathFailMPIn",
        "MP_Celeb_Preload_Fade",
        "PeyoteEndOut",
        "PeyoteEndIn",
        "PeyoteIn",
        "PeyoteOut",
        "MP_race_crash",
        "SuccessFranklin",
        "SuccessTrevor",
        "SuccessMichael",
        "DrugsMichaelAliensFightIn",
        "DrugsMichaelAliensFight",
        "DrugsMichaelAliensFightOut",
        "DrugsTrevorClownsFightIn",
        "DrugsTrevorClownsFight",
        "DrugsTrevorClownsFightOut",
        "HeistCelebPass",
        "HeistCelebPassBW",
        "HeistCelebEnd",
        "HeistCelebToast",
        "MenuMGHeistIn",
        "MenuMGTournamentIn",
        "MenuMGSelectionIn",
        "ChopVision",
        "DMT_flight_intro",
        "DMT_flight",
        "DrugsDrivingIn",
        "DrugsDrivingOut",
        "SwitchOpenNeutralFIB5",
        "HeistLocate",
        "MP_job_load",
        "RaceTurbo",
        "MP_intro_logo",
        "HeistTripSkipFade",
        "MenuMGHeistOut",
        "MP_corona_switch",
        "MenuMGSelectionTint",
        "SuccessNeutral",
        "ExplosionJosh3",
        "SniperOverlay",
        "RampageOut",
        "Rampage",
        "Dont_tazeme_bro"
    };

    public Menu()
    {
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
    }

    public float SelectedTakeoverRadius { get; set; }
    public int SelectedPlateIndex { get; set; }
    public string CurrentScreenEffect { get; set; }
    public void Tick()
    {
        //try
        //{
        if (Game.IsKeyDown(Mod.DataMart.Settings.SettingsManager.KeyBinding.MenuKey)) // Our menu on/off switch.
        {
            if (Mod.Player.IsDead)
            {
                if (!deathMenu.Visible)
                    ShowDeathMenu();
                else
                    deathMenu.Visible = false;
            }
            else if (Mod.Player.IsBusted)
            {
                if (!bustedMenu.Visible)
                    ShowBustedMenu();
                else
                    bustedMenu.Visible = false;
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
                    ShowMainMenu();
                else
                    mainMenu.Visible = false;
            }
        }
        else if (Game.IsKeyDown(Mod.DataMart.Settings.SettingsManager.KeyBinding.DebugMenuKey)) // Our menu on/off switch.
        {
            if (!debugMenu.Visible)
                ShowDebugMenu();
            else
                debugMenu.Visible = false;
        }

        menuPool.ProcessMenus(); // Process all our menus: draw the menu and process the key strokes and the mouse.      
        //}
        //catch (Exception e)
        //{
        //    ScriptController.Dispose();
        //    Mod.Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
        //}
    }
    public void ShowMainMenu()
    {
        CreateMainMenu();
        mainMenu.Visible = true;
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
    public void ShowBustedMenu()
    {
        if (Mod.Player.IsDead)
            return;

        CreateBustedMenu();

        mainMenu.Visible = false;
        deathMenu.Visible = false;
        debugMenu.Visible = false;
        optionsMenu.Visible = false;
        actionsMenu.Visible = false;
        scenariosMenu.Visible = false;

        bustedMenu.Visible = true;
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
    private void CreateMainMenu()
    {
        mainMenu.Clear();
        menuMainTakeoverRandomPed = new UIMenuListItem("Takeover Random Pedestrian",
            "Takes over a random pedestrian around the player.", new List<dynamic> {"Closest", "100 M", "500 M"});
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
    private void CreateDeathMenu()
    {
        deathMenu.Clear();
        menuDeathUndie = new UIMenuItem("Un-Die", "Respawn at this exact spot as yourself.");
        menuDeathHospitalRespawn = new UIMenuListItem("Give Up",
            "Respawn at the nearest hospital. Lose a hospital fee and your guns.",
            Mod.DataMart.Places.GetLocations(LocationType.Hospital));
        menuDeathTakeoverRandomPed = new UIMenuListItem("Takeover Random Pedestrian",
            "Takes over a random pedestrian around the player.",
            new List<dynamic> {"Closest", "20 M", "40 M", "60 M", "100 M", "500 M"});

        UpdateClosestHospitalIndex();

        deathMenu.AddItem(menuDeathUndie);
        deathMenu.AddItem(menuDeathHospitalRespawn);
        deathMenu.AddItem(menuDeathTakeoverRandomPed);

        if (Mod.DataMart.Settings.SettingsManager.General.UndieLimit == 0 || Mod.Player.TimesDied < Mod.DataMart.Settings.SettingsManager.General.UndieLimit)
            menuDeathUndie.Enabled = true;
        else
            menuDeathUndie.Enabled = false;
    }
    private void CreateBustedMenu()
    {
        bustedMenu.Clear();
        menuBustedResistArrest = new UIMenuItem("Resist Arrest", "Better hope you're strapped.");
        menuBustedBribe = new UIMenuItem("Bribe Police", "Bribe the police to let you go. Don't be cheap.");
        menuBustedSurrender = new UIMenuListItem("Surrender",
            "Surrender and get out on bail. Lose bail money and your guns.",
            Mod.DataMart.Places.GetLocations(LocationType.Police));
        menuBustedTakeoverRandomPed = new UIMenuListItem("Takeover Random Pedestrian",
            "Takes over a random pedestrian around the player.",
            new List<dynamic> {"Closest", "20 M", "40 M", "60 M", "100 M", "500 M"});
        menuBustedTalk = new UIMenuItem("Talk", "Try to talk your way out of an arrest.");

        UpdateClosestPoliceStationIndex();

        bustedMenu.AddItem(menuBustedResistArrest);
        bustedMenu.AddItem(menuBustedBribe);
        bustedMenu.AddItem(menuBustedSurrender);
        bustedMenu.AddItem(menuBustedTalk);
        bustedMenu.AddItem(menuBustedTakeoverRandomPed);

        if (Mod.Player.WantedLevel <= 1)
            menuBustedTalk.Enabled = true;
        else
            menuBustedTalk.Enabled = false;
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

        CreateSettingSubMenu(typeof(GeneralSettings).GetFields(), Mod.DataMart.Settings.SettingsManager.General, settingsMenuGeneral);
        CreateSettingSubMenu(typeof(PoliceSettings).GetFields(), Mod.DataMart.Settings.SettingsManager.Police, settingsMenuPolice);
        CreateSettingSubMenu(typeof(UISettings).GetFields(), Mod.DataMart.Settings.SettingsManager.UI, settingsMenuUISettings);
        CreateSettingSubMenu(typeof(KeySettings).GetFields(), Mod.DataMart.Settings.SettingsManager.KeyBinding, settingsMenuKeySettings);
        CreateSettingSubMenu(typeof(TrafficSettings).GetFields(), Mod.DataMart.Settings.SettingsManager.TrafficViolations,
            settingsMenuTrafficViolations);

        optionsMenu.OnItemSelect += OptionsMenuSelect;
        optionsMenu.OnListChange += OnListChange;
        optionsMenu.OnCheckboxChange += OnCheckboxChange;


        settingsMenuGeneral.OnItemSelect += SettingsMenuSelect;
        settingsMenuPolice.OnItemSelect += SettingsMenuSelect;
        settingsMenuUISettings.OnItemSelect += SettingsMenuSelect;
        settingsMenuKeySettings.OnItemSelect += SettingsMenuSelect;
        settingsMenuTrafficViolations.OnItemSelect += SettingsMenuSelect;
    }
    private void CreateSettingSubMenu(FieldInfo[] Fields, object SettingsSubType, UIMenu MenuToSet)
    {
        foreach (FieldInfo fi in Fields)
        {
            if (fi.FieldType == typeof(bool))
            {
                UIMenuCheckboxItem MySetting = new UIMenuCheckboxItem(fi.Name, (bool) fi.GetValue(SettingsSubType));
                MenuToSet.AddItem(MySetting);
            }

            if (fi.FieldType == typeof(int) || fi.FieldType == typeof(string) || fi.FieldType == typeof(float))
            {
                UIMenuItem MySetting = new UIMenuItem(string.Format("{0}: {1}", fi.Name, fi.GetValue(SettingsSubType)));
                MenuToSet.AddItem(MySetting);
            }
        }
    }
    private void CreateActionsMenu()
    {
        menuMainSuicide = new UIMenuItem("Suicide", "Commit Suicide");
        menuActionSmoking = new UIMenuItem("Smoking", "Start smoking.");
        menuMainChangeLicensePlate = new UIMenuListItem("Change Plate", "Change your license plate if you have spares.",Mod.Player.SpareLicensePlates); //new UIMenuItem("Change Plate", "Change your license plate if you have spares");
        menuMainRemoveLicensePlate = new UIMenuItem("Remove Plate", "Remove the license plate.");
        menuMainChangeHelmet = new UIMenuItem("Toggle Helmet", "Add/Removes your helmet");

        actionsMenu.AddItem(menuMainSuicide);
        actionsMenu.AddItem(menuActionSmoking);

        if (!Mod.Player.IsInVehicle)
        {
            actionsMenu.AddItem(menuMainChangeLicensePlate);
            actionsMenu.AddItem(menuMainRemoveLicensePlate);
        }

        actionsMenu.OnItemSelect += ActionsMenuSelect;
        actionsMenu.OnListChange += OnListChange;
        actionsMenu.OnCheckboxChange += OnCheckboxChange;
    }
    private void CreateScenariosMenu()
    {
        scenariosMainPrisonEscape = new UIMenuItem("Prison Escape", "Escape the prison");

        scenariosMenu.AddItem(scenariosMainPrisonEscape);


        scenariosMenu.OnItemSelect += ScenarioMenuSelect;
        scenariosMenu.OnListChange += OnListChange;
        scenariosMenu.OnCheckboxChange += OnCheckboxChange;
    }
    private void CreateDebugMenu()
    {
        debugMenu.Clear();
        menuDebugResetCharacter = new UIMenuItem("Reset Character", "Change your character back to the default model.");
        menuDebugKillPlayer = new UIMenuItem("Kill Player", "Immediatly die and ragdoll");
        menuDebugRandomWeapon = new UIMenuListItem("Get Random Weapon", "Gives the Player a random weapon and ammo.",
            Enum.GetNames(typeof(WeaponCategory)).ToList());
        menuDebugRandomVariation = new UIMenuItem("Apply Random Variation", "Add some cool stuff to your gun");
        menuDebugScreenEffect = new UIMenuListItem("Play Screen Effect", "Choose Screen Effect To Play", ScreenEffects);
        menuDebugGiveMoney = new UIMenuItem("Get Money", "Give you some cash");
        menuDebugHealthAndArmor = new UIMenuItem("Health and Armor", "Get loaded for bear");
        menuAutoSetRadioStation = new UIMenuListItem("Auto-Set Station",
            "Will auto set the station any time the radio is on", strRadioStations);
        menuDebugResetMod = new UIMenuItem("Reset Mod", "Reloads Mod and reads from the XML");

        debugMenu.AddItem(menuDebugResetCharacter);
        debugMenu.AddItem(menuDebugKillPlayer);
        debugMenu.AddItem(menuDebugRandomWeapon);
        debugMenu.AddItem(menuDebugRandomVariation);
        debugMenu.AddItem(menuDebugGiveMoney);
        debugMenu.AddItem(menuDebugHealthAndArmor);
        debugMenu.AddItem(menuAutoSetRadioStation);
        debugMenu.AddItem(menuDebugScreenEffect);
        debugMenu.AddItem(menuDebugResetMod);
    }
    private void UpdateClosestHospitalIndex()
    {
        menuDeathHospitalRespawn.Index = Mod.DataMart.Places.GetLocations(LocationType.Hospital).IndexOf(Mod.DataMart.Places.GetClosestLocation(Game.LocalPlayer.Character.Position,LocationType.Hospital));
    }
    private void UpdateClosestPoliceStationIndex()
    {
        menuBustedSurrender.Index = Mod.DataMart.Places.GetLocations(LocationType.Police).IndexOf(Mod.DataMart.Places.GetClosestLocation(Game.LocalPlayer.Character.Position,LocationType.Police));
    }
    private void MainMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == menuMainTakeoverRandomPed)
        {
            if (Mod.Player.WantedLevel > 0)
            {
                Game.DisplayNotification("Lose your wanted level first");
                return;
            }

            if (SelectedTakeoverRadius == -1f)
                Mod.Player.PedSwap.TakeoverPed(500f, true, false, true);
            else
                Mod.Player.PedSwap.TakeoverPed(SelectedTakeoverRadius, false, false, true);
        }
        else if (selectedItem == menuMainShowPlayerStatus)
        {
            Mod.Player.DisplayPlayerNotification();
        }

        mainMenu.Visible = false;
    }
    private void BustedMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == menuBustedResistArrest)
            Mod.Player.Respawning.ResistArrest();
        else if (selectedItem == menuBustedBribe)
            if (int.TryParse(GetKeyboardInput(""), out int BribeAmount))
                Mod.Player.Respawning.BribePolice(BribeAmount);
        if (selectedItem == menuBustedSurrender)
        {
            Mod.Player.Respawning.SurrenderToPolice(CurrentSelectedSurrenderLocation);
        }
        else if (selectedItem == menuBustedTalk)
        {
            Mod.Player.Respawning.Talk();
        }
        else if (selectedItem == menuBustedTakeoverRandomPed)
        {
            if (SelectedTakeoverRadius == -1f)
                Mod.Player.PedSwap.TakeoverPed(500f, true, true, true);
            else
                Mod.Player.PedSwap.TakeoverPed(SelectedTakeoverRadius, false, true, true);
        }

        bustedMenu.Visible = false;
    }
    private void DeathMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == menuDeathUndie) Mod.Player.Respawning.UnDie();
        if (selectedItem == menuDeathHospitalRespawn)
        {
            Mod.Player.Respawning.RespawnAtHospital(CurrentSelectedHospitalLocation);
        }
        else if (selectedItem == menuDeathTakeoverRandomPed)
        {
            if (SelectedTakeoverRadius == -1f)
                Mod.Player.PedSwap.TakeoverPed(500f, true, true, true);
            else
                Mod.Player.PedSwap.TakeoverPed(SelectedTakeoverRadius, false, true, true);
        }

        deathMenu.Visible = false;
    }
    private void OptionsMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
       //if (selectedItem == ReloadSettings) SettingsManager.ReadAllConfigs();
    }
    private void SettingsMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        string MySettingName = selectedItem.Text.Split(':')[0];
        FieldInfo[] MyFields = typeof(GeneralSettings).GetFields();
        object ToSet = Mod.DataMart.Settings.SettingsManager.General;

        if (sender == settingsMenuGeneral)
        {
            MyFields = typeof(GeneralSettings).GetFields();
        }
        else if (sender == settingsMenuPolice)
        {
            ToSet = Mod.DataMart.Settings.SettingsManager.Police;
            MyFields = typeof(PoliceSettings).GetFields();
        }
        else if (sender == settingsMenuKeySettings)
        {
            ToSet = Mod.DataMart.Settings.SettingsManager.KeyBinding;
            MyFields = typeof(KeySettings).GetFields();
        }
        else if (sender == settingsMenuUISettings)
        {
            ToSet = Mod.DataMart.Settings.SettingsManager.UI;
            MyFields = typeof(UISettings).GetFields();
        }
        else if (sender == settingsMenuTrafficViolations)
        {
            ToSet = Mod.DataMart.Settings.SettingsManager.TrafficViolations;
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

        Mod.DataMart.Settings.SerializeAllSettings();
    }
    private void ActionsMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == menuMainSuicide)
        {
            Mod.Player.Surrendering.CommitSuicide(Game.LocalPlayer.Character);
        }
        else if (selectedItem == menuMainChangeLicensePlate)
        {
            PlateTheft plateTheft = new PlateTheft();
            plateTheft.ChangePlate(Mod.Player.SpareLicensePlates[Mod.Menu.SelectedPlateIndex]);
        }
        else if (selectedItem == menuMainRemoveLicensePlate)
        {
            PlateTheft plateTheft = new PlateTheft();
            plateTheft.RemovePlate();
        }
    }
    private void ScenarioMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == scenariosMainPrisonEscape)
        {
            //PedSwap.BecomeScenarioPed();
        }
    }
    private void DebugMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == menuDebugKillPlayer) Game.LocalPlayer.Character.Kill();
        if (selectedItem == menuDebugRandomWeapon)
        {
            WeaponInformation myGun = Mod.DataMart.Weapons.GetRandomRegularWeapon((WeaponCategory) RandomWeaponCategory);
            if (myGun != null)
                Game.LocalPlayer.Character.Inventory.GiveNewWeapon(myGun.ModelName, myGun.AmmoAmount, true);
        }

        if (selectedItem == menuDebugGiveMoney) Game.LocalPlayer.Character.GiveCash(50000);
        if (selectedItem == menuDebugResetMod)
        {
            //ScriptController.Dispose();
            //ScriptController.Initialize();
        }

        if (selectedItem == menuDebugHealthAndArmor)
        {
            Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
            Game.LocalPlayer.Character.Armor = 100;
        }

        if (selectedItem == menuDebugScreenEffect)
        {
            NativeFunction.Natives.xB4EDDC19532BFB85(); //ANIMPOSTFX_STOP_ALL
            Mod.Debug.WriteToLog("Screen Effect: ", CurrentScreenEffect);


            if (CurrentScreenEffect != "")
                NativeFunction.Natives.x2206BF9A37B7F724(CurrentScreenEffect, 0, true); //ANIMPOSTFX_PLAY
        }

        debugMenu.Visible = false;
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
                    SelectedTakeoverRadius = -1f;
                else if (index == 1)
                    SelectedTakeoverRadius = 100f;
                else if (index == 2)
                    SelectedTakeoverRadius = 500f;
            }

            if (list == menuMainChangeLicensePlate) SelectedPlateIndex = index;
        }
        else if (sender == deathMenu)
        {
            if (list == menuDeathHospitalRespawn)
            {
                CurrentSelectedHospitalLocation = Mod.DataMart.Places.GetLocations(LocationType.Hospital)[index];
                Mod.Debug.WriteToLog("menuDeathHospitalRespawn Changed",string.Format("Location: {0}", CurrentSelectedHospitalLocation));
            }
        }
        else if (sender == bustedMenu)
        {
            if (list == menuBustedSurrender)
            {
                CurrentSelectedSurrenderLocation = Mod.DataMart.Places.GetLocations(LocationType.Police)[index];
                Mod.Debug.WriteToLog("menuBustedSurrender Changed",string.Format("Location: {0}", CurrentSelectedSurrenderLocation));
            }
        }
        else if (sender == debugMenu)
        {
            if (list == menuDebugRandomWeapon)
                RandomWeaponCategory = list.Index;
            //else if (list == menuAutoSetRadioStation)
            //    Mod.Player.VehicleRadio.AutoTuneStation = strRadioStations[index];
            if (list == menuDebugScreenEffect)
                CurrentScreenEffect = ScreenEffects[index];
        }
    }
    private string GetKeyboardInput(string DefaultText)
    {
        NativeFunction.CallByName<bool>("DISPLAY_ONSCREEN_KEYBOARD", true, "FMMC_KEY_TIP8", "", DefaultText, "", "", "",
            255 + 1);

        while (NativeFunction.CallByName<int>("UPDATE_ONSCREEN_KEYBOARD") == 0) GameFiber.Sleep(500);
        string Value;
        IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_ONSCREEN_KEYBOARD_RESULT");
        Value = Marshal.PtrToStringAnsi(ptr);
        return Value;
    }
}