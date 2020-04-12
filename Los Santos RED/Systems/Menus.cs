using ExtensionsMethods;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


internal static class Menus
{
    private static UIMenuListItem menuMainTakeoverRandomPed;
    private static UIMenuItem menuDebugResetCharacter;
    private static UIMenuItem menuMainSuicide;
    private static UIMenuItem menuMainChangeLicensePlate;
    private static UIMenuItem menuMainRemoveLicensePlate;
    private static UIMenuItem menuMainShowPlayerStatus;
    private static UIMenuItem menuMainChangeHelmet;
    private static UIMenuItem menuDebugKillPlayer;
    private static UIMenuListItem menuDebugRandomWeapon;
    private static UIMenuItem menuDebugRandomVariation;
    private static UIMenuListItem menuDebugScreenEffect;

    // private static UIMenuCheckboxItem menuDebugEnabled;
    private static UIMenuItem menuDeathUndie;
    //private static UIMenuItem menuDeathRespawnInPlace;
   // private static UIMenuItem menuDeathNormalRespawn;
    private static UIMenuListItem menuDeathTakeoverRandomPed;
    private static UIMenuItem menuBustedResistArrest;
    private static UIMenuItem menuBustedBribe;
    private static UIMenuItem menuBustedTalk;
    //private static UIMenuItem menuBustedRespawnInPlace;
    private static UIMenuListItem menuBustedTakeoverRandomPed;
    private static UIMenuListItem menuBustedSurrender;
    private static UIMenuListItem menuDeathHospitalRespawn;
    private static UIMenuItem menuDebugGiveMoney;
    private static UIMenuItem menuDebugHealthAndArmor;
    private static UIMenuItem menuActionSmoking;
   // private static UIMenuCheckboxItem menuRadioOff;
    private static UIMenuListItem menuAutoSetRadioStation;
    private static UIMenuItem ReloadSettings;

    private static MenuPool menuPool;
    private static UIMenu mainMenu;
    private static UIMenu deathMenu;
    private static UIMenu debugMenu;
    private static UIMenu bustedMenu;
    private static UIMenu optionsMenu;
    private static UIMenu actionsMenu;

   // private static UIMenu talkMenu;

    private static int RandomWeaponCategory;
    //private static Vector3 WorldPos;

    private static List<string> SmokingOptionsList;
    private static Location CurrentSelectedSurrenderLocation;
    private static Location CurrentSelectedHospitalLocation;
    //private static string CurrentSelectedRadioStation;

    private static readonly List<string> strRadioStations = new List<string> { "NONE", "RADIO_01_CLASS_ROCK", "RADIO_02_POP", "RADIO_03_HIPHOP_NEW", "RADIO_04_PUNK", "RADIO_05_TALK_01", "RADIO_06_COUNTRY", "RADIO_07_DANCE_01", "RADIO_08_MEXICAN", "RADIO_09_HIPHOP_OLD", "RADIO_12_REGGAE", "RADIO_13_JAZZ", "RADIO_14_DANCE_02", "RADIO_15_MOTOWN", "RADIO_20_THELAB", "RADIO_16_SILVERLAKE", "RADIO_17_FUNK", "RADIO_18_90S_ROCK", "RADIO_19_USER", "RADIO_11_TALK_02", "HIDDEN_RADIO_AMBIENT_TV_BRIGHT", "OFF" };
    private static readonly new List<string> ScreenEffects = new List<string>  { "SwitchHUDIn",
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
        "Dont_tazeme_bro" };

    public static float TakeoverRadius;
    public static int ChangePlateIndex;
    private static string CurrentScreenEffect = "SwitchHUDIn";

    public static bool IsRunning { get; set; }
    public static void Intitialize()
    {
        IsRunning = true;
        RandomWeaponCategory = 0;
        //WorldPos = new Vector3(0f, 0f, 0f);

        SmokingOptionsList = default;
        CurrentSelectedSurrenderLocation = null;
        CurrentSelectedHospitalLocation = null;
        //CurrentSelectedRadioStation = null;

        TakeoverRadius = -1f;
        ChangePlateIndex = 0;
        menuPool = new MenuPool();
        mainMenu = new UIMenu("Los Santos RED", "Select an Option");
        menuPool.Add(mainMenu);
        deathMenu = new UIMenu("Wasted", "Choose Respawn");
        menuPool.Add(deathMenu);
        debugMenu = new UIMenu("Debug", "Debug Settings");
        menuPool.Add(debugMenu);
        bustedMenu = new UIMenu("Busted", "Choose Respawn");
        menuPool.Add(bustedMenu);

        CreateMainMenu();

        menuDebugResetCharacter = new UIMenuItem("Reset Character", "Change your character back to the default model.");
        menuDebugKillPlayer = new UIMenuItem("Kill Player", "Immediatly die and ragdoll");
        menuDebugRandomWeapon = new UIMenuListItem("Get Random Weapon", "Gives the Player a random weapon and ammo.", new List<dynamic> { "Melee", "Pistol", "Shotgun", "SMG", "AR", "LMG", "Sniper", "Heavy" });
        menuDebugRandomVariation = new UIMenuItem("Apply Random Variation", "Add some cool stuff to your gun");



        menuDebugScreenEffect = new UIMenuListItem("Play Screen Effect", "Choose Screen Effect To Play", ScreenEffects);


        //menuDebugEnabled = new UIMenuCheckboxItem("Debug Enabled", LosSantosRED.MySettings.Debug, "Debug for testing");
        menuDebugGiveMoney = new UIMenuItem("Get Money", "Give you some cash");
        menuDebugHealthAndArmor = new UIMenuItem("Health and Armor", "Get loaded for bear");

        //menuRadioOff = new UIMenuCheckboxItem("Radio Enabled", LosSantosRED.MySettings.RadioAlwaysOff, "Will Auto Turn off Radio");
        menuAutoSetRadioStation = new UIMenuListItem("Auto-Set Station", "Will auto set the station any time the radio is on", strRadioStations);

        debugMenu.AddItem(menuDebugResetCharacter);
        debugMenu.AddItem(menuDebugKillPlayer);
        debugMenu.AddItem(menuDebugRandomWeapon);
        debugMenu.AddItem(menuDebugRandomVariation);
        //debugMenu.AddItem(menuDebugEnabled);
        debugMenu.AddItem(menuDebugGiveMoney);
        debugMenu.AddItem(menuDebugHealthAndArmor);
        //debugMenu.AddItem(menuRadioOff);
        debugMenu.AddItem(menuAutoSetRadioStation);
        debugMenu.AddItem(menuDebugScreenEffect);


        menuDeathUndie = new UIMenuItem("Un-Die", "Respawn at this exact spot as yourself.");
        //menuDeathRespawnInPlace = new UIMenuItem("Respawn In Place", "Respawn at this exact spot.");
        menuDeathHospitalRespawn = new UIMenuListItem("Give Up", "Respawn at the nearest hospital. Lose a hospital fee and your guns.", Locations.GetAllLocationsOfType(Location.LocationType.Hospital));
        //menuDeathNormalRespawn = new UIMenuItem("Standard Respawn", "Respawn at the hospital (standard game logc).");
        menuDeathTakeoverRandomPed = new UIMenuListItem("Takeover Random Pedestrian", "Takes over a random pedestrian around the player.", new List<dynamic> { "Closest", "20 M", "40 M", "60 M", "100 M", "500 M" });

        deathMenu.AddItem(menuDeathUndie);
        deathMenu.AddItem(menuDeathHospitalRespawn);
        deathMenu.AddItem(menuDeathTakeoverRandomPed);

        CreateBustedMenu();

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


        //talkMenu.OnItemSelect += TalkMenuSelect;

        ProcessLoop();

    }



    public static void ProcessLoop()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                while (IsRunning)
                {
                    if (Game.IsKeyDown(LosSantosRED.MySettings.KeyBinding.MenuKey)) // Our menu on/off switch.
                    {
                        if (LosSantosRED.IsDead)
                        {
                            if (!deathMenu.Visible)
                                ShowDeathMenu();
                            else
                                deathMenu.Visible = false;
                        }
                        else if (LosSantosRED.IsBusted)
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
                        else
                        {
                            if (!mainMenu.Visible)
                                ShowMainMenu();
                            else
                                mainMenu.Visible = false;
                        }
                    }
                    else if (Game.IsKeyDown(Keys.F11)) // Our menu on/off switch.
                    {
                        debugMenu.Visible = !debugMenu.Visible;
                    }
                    menuPool.ProcessMenus();       // Process all our menus: draw the menu and process the key strokes and the mouse.      
                    GameFiber.Yield();
                }
            }
            catch (Exception e)
            {
                LosSantosRED.Dispose();
                Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
            }
        });
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void ShowDeathMenu()
    {
        if (LosSantosRED.MySettings.General.UndieLimit == 0)
        {
            menuDeathUndie.Enabled = true;
        }
        else if (LosSantosRED.TimesDied < LosSantosRED.MySettings.General.UndieLimit)
        {
            menuDeathUndie.Enabled = true;
        }
        else
        {
            menuDeathUndie.Enabled = false;
        }
        UpdateClosestHospitalIndex();
        mainMenu.Visible = false;
        debugMenu.Visible = false;
        bustedMenu.Visible = false;
        optionsMenu.Visible = false;
        actionsMenu.Visible = false;

        deathMenu.Visible = true;
    }
    public static void ShowBustedMenu()
    {
        if (LosSantosRED.IsDead)
            return;
        CreateBustedMenu();


        UpdateClosestPoliceStationIndex();
        mainMenu.Visible = false;
        deathMenu.Visible = false;
        debugMenu.Visible = false;
        optionsMenu.Visible = false;
        actionsMenu.Visible = false;

        bustedMenu.Visible = true;
    }

    

    private static void CreateMainMenu()
    {
        mainMenu.Clear();
        menuMainTakeoverRandomPed = new UIMenuListItem("Takeover Random Pedestrian", "Takes over a random pedestrian around the player.", new List<dynamic> { "Closest", "100 M", "500 M" });
        menuMainShowPlayerStatus = new UIMenuItem("Show Status", "Show the player status with a notification");
        mainMenu.AddItem(menuMainTakeoverRandomPed);
        mainMenu.AddItem(menuMainShowPlayerStatus);

        CreateActionsMenu();
        CreateOptionsMenu();     
    }


    private static void CreateBustedMenu()
    {
        bustedMenu.Clear();
        menuBustedResistArrest = new UIMenuItem("Resist Arrest", "Better hope you're strapped.");
        menuBustedBribe = new UIMenuItem("Bribe Police", "Bribe the police to let you go. Don't be cheap.");
        menuBustedSurrender = new UIMenuListItem("Surrender", "Surrender and get out on bail. Lose bail money and your guns.", Locations.GetAllLocationsOfType(Location.LocationType.Police));
        menuBustedTakeoverRandomPed = new UIMenuListItem("Takeover Random Pedestrian", "Takes over a random pedestrian around the player.", new List<dynamic> { "Closest", "20 M", "40 M", "60 M", "100 M", "500 M" });


        bustedMenu.AddItem(menuBustedResistArrest);
        bustedMenu.AddItem(menuBustedBribe);
        bustedMenu.AddItem(menuBustedSurrender);
        if (LosSantosRED.PlayerWantedLevel <= 1 || !Police.CurrentCrimes.CommittedAnyCrimes)
        {
            menuBustedTalk = new UIMenuItem("Talk", "Try to talk your way out of an arrest.");
            bustedMenu.AddItem(menuBustedTalk);
        }        
        bustedMenu.AddItem(menuBustedTakeoverRandomPed);

    }
    private static void CreateOptionsMenu()
    {
        optionsMenu = menuPool.AddSubMenu(mainMenu, "Options");
       ReloadSettings = new UIMenuItem("Reload Settings", "Reload All settings from XML");
        optionsMenu.AddItem(ReloadSettings);
        //foreach (FieldInfo fi in Type.GetType("Settings", false).GetFields())
        //{
        //    if (fi.FieldType == typeof(bool))
        //    {
        //        UIMenuCheckboxItem MySetting = new UIMenuCheckboxItem(fi.Name, (bool)fi.GetValue(null));
        //        optionsMenu.AddItem(MySetting);
        //    }
        //    if (fi.FieldType == typeof(int) || fi.FieldType == typeof(string) || fi.FieldType == typeof(float))
        //    {
        //        UIMenuItem MySetting = new UIMenuItem(string.Format("{0}: {1}", fi.Name, fi.GetValue(null)));
        //        optionsMenu.AddItem(MySetting);
        //    }
        //}
        optionsMenu.OnItemSelect += OptionsMenuSelect;
        optionsMenu.OnListChange += OnListChange;
        optionsMenu.OnCheckboxChange += OnCheckboxChange;
        optionsMenu.RefreshIndex();
    }
    private static void CreateActionsMenu()
    {
        actionsMenu = menuPool.AddSubMenu(mainMenu, "Actions");
        menuMainSuicide = new UIMenuItem("Suicide", "Commit Suicide");

        if (Smoking.CurrentAttachedPosition == Smoking.CigarettePosition.Mouth)
            SmokingOptionsList = new List<string> { "Start", "Stop" };
        else if (Smoking.CurrentAttachedPosition == Smoking.CigarettePosition.Hand)
            SmokingOptionsList = new List<string> { "Stop" };
        else if (Smoking.CurrentAttachedPosition == Smoking.CigarettePosition.None)
            SmokingOptionsList = new List<string> { "Start" };

        menuActionSmoking = new UIMenuListItem("Smoking", "Start smoking.", SmokingOptionsList);


        menuMainChangeLicensePlate = new UIMenuListItem("Change Plate", "Change your license plate if you have spares.", LicensePlateChanging.SpareLicensePlates);//new UIMenuItem("Change Plate", "Change your license plate if you have spares");
        menuMainRemoveLicensePlate = new UIMenuItem("Remove Plate", "Remove the license plate.");
        actionsMenu.AddItem(menuMainSuicide);
        actionsMenu.AddItem(menuActionSmoking);

        if (!LosSantosRED.PlayerInVehicle)
        {
            actionsMenu.AddItem(menuMainChangeLicensePlate);
            actionsMenu.AddItem(menuMainRemoveLicensePlate);
        }

        menuMainChangeHelmet = new UIMenuItem("Toggle Helmet", "Add/Removes your helmet");

        actionsMenu.OnItemSelect += ActionsMenuSelect;
        actionsMenu.OnListChange += OnListChange;
        actionsMenu.OnCheckboxChange += OnCheckboxChange;
        actionsMenu.RefreshIndex();
    }
    public static void ShowMainMenu()
    {         
        CreateMainMenu();
        mainMenu.Visible = true;
    }
    private static void UpdateClosestHospitalIndex()
    {
        menuDeathHospitalRespawn.Index = Locations.GetAllLocationsOfType(Location.LocationType.Hospital).IndexOf(Locations.GetClosestLocationByType(Game.LocalPlayer.Character.Position, Location.LocationType.Hospital));
    }
    private static void UpdateClosestPoliceStationIndex()
    {
        menuBustedSurrender.Index = Locations.GetAllLocationsOfType(Location.LocationType.Police).IndexOf(Locations.GetClosestLocationByType(Game.LocalPlayer.Character.Position, Location.LocationType.Police));
    }
    public static void OnCheckboxChange(UIMenu sender, UIMenuCheckboxItem checkbox, bool Checked)
    {
        //if (sender == optionsMenu)
        //{
        //    FieldInfo[] MyFields = Type.GetType("Settings", false).GetFields();
        //    FieldInfo MySetting = MyFields.Where(x => x.Name == checkbox.Text).FirstOrDefault();
        //    MySetting.SetValue(null, Checked);
        //    LosSantosRED.MySettings.WriteSettings();
        //}
        //if(sender == debugMenu)
        //{
        //    if(checkbox == menuRadioOff)
        //    {
        //        LosSantosRED.MySettings.RadioAlwaysOff = Checked;
        //    }
        //}
    }
    public static void OnListChange(UIMenu sender, UIMenuListItem list, int index)
    {
        if (sender == mainMenu)
        {
            if (list == menuMainTakeoverRandomPed)
            {
                if (index == 0)
                    TakeoverRadius = -1f;
                else if(index == 1)
                    TakeoverRadius = 100f;
                else if (index == 2)
                    TakeoverRadius = 500f;
            }
            if(list == menuMainChangeLicensePlate)
            {
                ChangePlateIndex = index;
            }
        }
        else if (sender == deathMenu)
        {
            if (list == menuDeathHospitalRespawn)
            {
                CurrentSelectedHospitalLocation = Locations.GetAllLocationsOfType(Location.LocationType.Hospital)[index];
                Debugging.WriteToLog("menuDeathHospitalRespawn Changed", String.Format("Location: {0}", CurrentSelectedHospitalLocation));
            }
        }
        else if (sender == bustedMenu)
        {
            if (list == menuBustedSurrender)
            {
                CurrentSelectedSurrenderLocation = Locations.GetAllLocationsOfType(Location.LocationType.Police)[index];
                Debugging.WriteToLog("menuBustedSurrender Changed", String.Format("Location: {0}", CurrentSelectedSurrenderLocation));
            }
        }
        else if (sender == debugMenu)
        {
            if (list == menuDebugRandomWeapon)
                RandomWeaponCategory = list.Index;
            else if (list == menuAutoSetRadioStation)
                VehicleEngine.AutoTuneStation = strRadioStations[index];
            if (list == menuDebugScreenEffect)
                CurrentScreenEffect = ScreenEffects[index];
        }
    }
    private static void MainMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == menuMainTakeoverRandomPed)
        {
            if(LosSantosRED.PlayerWantedLevel > 0)
            {
                Game.DisplayNotification("Lose your wanted level first");
                return;
            }
            if (TakeoverRadius == -1f)
                PedSwapping.TakeoverPed(PedSwapping.GetPedestrian(500f, true), false, false,true,false);
            else
                PedSwapping.TakeoverPed(PedSwapping.GetPedestrian(TakeoverRadius, false), false, false,true,false);
        }
        //else if (selectedItem == menuMainSuicide)
        //{
        //    Surrendering.CommitSuicide(Game.LocalPlayer.Character);
        //}
        //else if (selectedItem == menuMainChangeLicensePlate)
        //{
        //    LicensePlateChanging.ChangeNearestLicensePlate();
        //}
        //else if (selectedItem == menuMainRemoveLicensePlate)
        //{
        //    LicensePlateChanging.RemoveNearestLicensePlate();
        //}
        //else if (selectedItem == menuMainChangeHelmet)
        //{
        //    PedSwapping.AddRemovePlayerHelmet();
        //}
        else if(selectedItem == menuMainShowPlayerStatus)
        {
            LosSantosRED.DisplayPlayerNotification();
        }
        mainMenu.Visible = false;
    }
    private static void BustedMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == menuBustedResistArrest)
        {
            Respawning.ResistArrest();
        }
        else if (selectedItem == menuBustedBribe)
        {
            if (int.TryParse(GetKeyboardInput(), out int BribeAmount))
            {
                Respawning.BribePolice(BribeAmount);
            }
        }
        if (selectedItem == menuBustedSurrender)
        {
            Respawning.Surrender(CurrentSelectedSurrenderLocation);
        }
        else if (selectedItem == menuBustedTalk)
        {
            Respawning.Talk();
        }
        else if (selectedItem == menuBustedTakeoverRandomPed)
        {
            if (TakeoverRadius == -1f)
                PedSwapping.TakeoverPed(PedSwapping.GetPedestrian(500f, true), true, false,true,true);
            else
                PedSwapping.TakeoverPed(PedSwapping.GetPedestrian(TakeoverRadius, false), true, false,true,true);
        }
        bustedMenu.Visible = false;
    }
    private static void DeathMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == menuDeathUndie)
        {
            Respawning.UnDie();
        }
        if (selectedItem == menuDeathHospitalRespawn)
        {
            Respawning.RespawnAtHospital(CurrentSelectedHospitalLocation);
        }
        else if (selectedItem == menuDeathTakeoverRandomPed)
        {
            if (TakeoverRadius == -1f)
                PedSwapping.TakeoverPed(PedSwapping.GetPedestrian(500f, true), true, false,true,true);
            else
                PedSwapping.TakeoverPed(PedSwapping.GetPedestrian(TakeoverRadius, false), true, false,true,true);
        }
        deathMenu.Visible = false;
    }
    private static void OptionsMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if(selectedItem == ReloadSettings)
        {
            LosSantosRED.ReadAllConfigs();
        }
        //string mySettingName = selectedItem.Text.Split(':')[0];
        //FieldInfo[] MyFields = Type.GetType("Settings", false).GetFields();
        //FieldInfo MySetting = MyFields.Where(x => x.Name == mySettingName).FirstOrDefault();

        //string Value = GetKeyboardInput();
        //if (MySetting.FieldType == typeof(float))
        //{
        //    if (float.TryParse(Value, out float myFloat))
        //    {
        //        MySetting.SetValue(null, myFloat);
        //        selectedItem.Text = string.Format("{0}: {1}", mySettingName, Value);
        //        LosSantosRED.MySettings.WriteSettings();
        //    }
        //}
        //else if (MySetting.FieldType == typeof(int))
        //{
        //    if (int.TryParse(Value, out int myInt))
        //    {
        //        MySetting.SetValue(null, myInt);
        //        selectedItem.Text = string.Format("{0}: {1}", mySettingName, Value);
        //        LosSantosRED.MySettings.WriteSettings();
        //    }
        //}
        //else if (MySetting.FieldType == typeof(string))
        //{
        //    MySetting.SetValue(null, Value);
        //    selectedItem.Text = string.Format("{0}: {1}", mySettingName, Value);
        //    LosSantosRED.MySettings.WriteSettings();
        //}
    }
    private static void ActionsMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == menuMainSuicide)
        {
            Surrendering.CommitSuicide(Game.LocalPlayer.Character);
        }
        else if (selectedItem == menuActionSmoking)
        {
            Smoking.StartScenario();
        }
        else if (selectedItem == menuMainChangeLicensePlate)
        {
            LicensePlateChanging.ChangeNearestLicensePlate();
        }
        else if (selectedItem == menuMainRemoveLicensePlate)
        {
            LicensePlateChanging.RemoveNearestLicensePlate();
        }
    }
    private static void DebugMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == menuDebugKillPlayer)
        {
            Game.LocalPlayer.Character.Kill();
        }
        if (selectedItem == menuDebugRandomWeapon)
        {
            GTAWeapon myGun = GTAWeapons.GetRandomWeapon((GTAWeapon.WeaponCategory)RandomWeaponCategory);
            Game.LocalPlayer.Character.Inventory.GiveNewWeapon(myGun.Name, myGun.AmmoAmount, true);
            if (myGun.PlayerVariations.Any())
                LosSantosRED.ApplyWeaponVariation(Game.LocalPlayer.Character, (uint)myGun.Hash, myGun.PlayerVariations.PickRandom());
        }
        if (selectedItem == menuDebugRandomVariation)
        {
            GTAWeapon myGun = LosSantosRED.GetCurrentWeapon(Game.LocalPlayer.Character);
            if (myGun == null)
                return;
            if (myGun.PlayerVariations.Any())
                LosSantosRED.ApplyWeaponVariation(Game.LocalPlayer.Character, (uint)myGun.Hash, myGun.PlayerVariations.PickRandom());
        }
        if (selectedItem == menuDebugGiveMoney)
        {
            Game.LocalPlayer.Character.GiveCash(5000);
        }
        if (selectedItem == menuDebugHealthAndArmor)
        {
            Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
            Game.LocalPlayer.Character.Armor = 100;
        }
        if(selectedItem == menuDebugScreenEffect)
        {
            NativeFunction.Natives.xB4EDDC19532BFB85();//ANIMPOSTFX_STOP_ALL
            Debugging.WriteToLog("Screen Effect: ", CurrentScreenEffect);


            if (CurrentScreenEffect != "")
            {
                NativeFunction.Natives.x2206BF9A37B7F724(CurrentScreenEffect, 0, true);//ANIMPOSTFX_PLAY
            }
        }
        debugMenu.Visible = false;
    }
   
    public static string GetKeyboardInput()
    {
        NativeFunction.CallByName<bool>("DISPLAY_ONSCREEN_KEYBOARD", true, "FMMC_KEY_TIP8", "", "", "", "", "", 255 + 1);

        while (NativeFunction.CallByName<int>("UPDATE_ONSCREEN_KEYBOARD") == 0)
        {
            GameFiber.Sleep(500);
        }
        string Value;
        unsafe
        {
            IntPtr ptr = Rage.Native.NativeFunction.CallByName<IntPtr>("GET_ONSCREEN_KEYBOARD_RESULT");
            Value = Marshal.PtrToStringAnsi(ptr);
        }
        return Value;
    }

}



