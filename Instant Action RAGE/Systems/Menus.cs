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

namespace Instant_Action_RAGE.Systems
{
    internal static class Menus
    {
        private static UIMenuListItem menuMainRandomCrime;
        private static UIMenuItem menuMainTakeoverNearestPed;
        private static UIMenuListItem menuMainTakeoverRandomPed;
        private static UIMenuItem menuMainReloadSettings;
        private static UIMenuItem menuMainOptions;
        private static UIMenuItem menuDebugResetCharacter;
        private static UIMenuItem menuMainSuicide;
        private static UIMenuItem menuMainChangeLicensePlate;
        private static UIMenuItem menuMainRemoveLicensePlate;
        private static UIMenuItem menuMainChangeHelmet;
        private static UIMenuItem menuDebugKillPlayer;
        private static UIMenuListItem menuDebugRandomWeapon;
        private static UIMenuListItem menuDebugScreenEffect;
        private static UIMenuCheckboxItem menuDebugEnabled;
        private static UIMenuItem menuDeathUndie;
        private static UIMenuItem menuDeathRespawnInPlace;
        private static UIMenuListItem menuDeathRandomCrime;
        private static UIMenuItem menuDeathNormalRespawn;
        private static UIMenuItem menuDeathTakeoverNearestPed;
        private static UIMenuListItem menuDeathTakeoverRandomPed;
        private static UIMenuItem menuBustedResistArrest;
        private static UIMenuItem menuBustedBribe;
        private static UIMenuItem menuBustedRespawnInPlace;
        private static UIMenuListItem menuBustedRandomCrime;
        private static UIMenuItem menuBustedNormalRespawn;
        private static UIMenuItem menuBustedTakeoverNearestPed;
        private static UIMenuListItem menuBustedTakeoverRandomPed;
        private static UIMenuItem menuBustedSurrender;
        private static UIMenuItem menuDeathHospitalRespawn;
        private static UIMenuItem menuDebugGiveMoney;
        private static UIMenuItem menuDebugHealthAndArmor;

        private static MenuPool menuPool;
        public static UIMenu mainMenu;
        public static UIMenu deathMenu;
        public static UIMenu debugMenu;
        public static UIMenu bustedMenu;
        public static UIMenu optionsMenu;

        private static int RandomCrimeLevel = 1;
        private static int RandomWeaponLevel = 0;
        private static Vector3 WorldPos = new Vector3(0f, 0f, 0f);
        private static Entity EntityToHighlight;
        private static List<int> BribeList = new List<int> { 250, 500, 1000, 1250, 1750, 2000, 3500 };
        private static List<int> UndieLimit = new List<int> { 0,1,2,3,4,5 };
        private static string CurrentScreenEffect = "Rampage";
        private static float TakeoverRadius = -1f;
        public static int ChangePlateIndex = 0;
        private static int PrevMainMenuCurrentSelection;
        private static bool PrevMainMenuVisible;
        private static bool MainMenuVisible;

        public static bool IsRunning { get; set; } = true;
        public static void MainLoop()
        {
            GameFiber.StartNew(delegate
            {
                while (IsRunning)
                {
                    GameFiber.Yield();
                }
            });
        }
        public static void Intitialize()
        {
            //Game.FrameRender += ProcessLoop;
            //MenusProcessFiber = new GameFiber(ProcessLoop);
            menuPool = new MenuPool();
            mainMenu = new UIMenu("Instant Action", "Select an Option");
            menuPool.Add(mainMenu);
            deathMenu = new UIMenu("Instant Action", "Choose Respawn (Wasted)");
            menuPool.Add(deathMenu);
            debugMenu = new UIMenu("Instant Action", "Debug");
            menuPool.Add(debugMenu);
            bustedMenu = new UIMenu("Instant Action", "Choose Respawn (Busted)");
            menuPool.Add(bustedMenu);

            CreateMainMenu();



            menuDebugResetCharacter = new UIMenuItem("Reset Character", "Change your character back to the default model.");
            menuDebugKillPlayer = new UIMenuItem("Kill Player", "Immediatly die and ragdoll");
            menuDebugRandomWeapon = new UIMenuListItem("Get Random Weapon", "Gives the Player a random weapon and ammo.", new List<dynamic> { "Level 0", "Level 1", "Level 2", "Level 3", "Level 4" } );
            menuDebugScreenEffect = new UIMenuListItem("Play Screen Effect", "", new List<dynamic> { "SwitchHUDIn",
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
        "Dont_tazeme_bro" });
            menuDebugEnabled = new UIMenuCheckboxItem("Debug Enabled", Settings.Debug, "Debug for testing");
            menuDebugGiveMoney = new UIMenuItem("Get Money", "Give you some cash");
            menuDebugHealthAndArmor = new UIMenuItem("Health and Armor", "Get loaded for bear");

            debugMenu.AddItem(menuDebugResetCharacter);
            debugMenu.AddItem(menuDebugKillPlayer);
            debugMenu.AddItem(menuDebugRandomWeapon);
            debugMenu.AddItem(menuDebugScreenEffect);
            debugMenu.AddItem(menuDebugEnabled);
            debugMenu.AddItem(menuDebugGiveMoney);
            debugMenu.AddItem(menuDebugHealthAndArmor);

            menuDeathUndie = new UIMenuItem("Un-Die", "Respawn at this exact spot as yourself.");
            menuDeathRespawnInPlace = new UIMenuItem("Respawn In Place", "Respawn at this exact spot.");
            menuDeathHospitalRespawn = new UIMenuItem("Give Up", "Respawn at the nearest hospital. Lose a hospital fee and your guns.");
            menuDeathNormalRespawn = new UIMenuItem("Standard Respawn", "Respawn at the hospital (standard game logc).");
            menuDeathTakeoverRandomPed = new UIMenuListItem("Takeover Random Pedestrian", "Takes over a random pedestrian around the player.", new List<dynamic> {"Closest", "20 M", "40 M", "60 M", "100 M", "500 M" });

            deathMenu.AddItem(menuDeathUndie);
            deathMenu.AddItem(menuDeathHospitalRespawn);
            deathMenu.AddItem(menuDeathTakeoverRandomPed);

            menuBustedResistArrest = new UIMenuItem("Resist Arrest", "Better hope you're strapped.");
            //menuBustedBribe = new UIMenuListItem("Bribe Police", "Bribe the police to let you go. Don't be cheap.",new List<dynamic> { 250, 500, 1000, 1250, 1750, 2000, 3500 } );
            menuBustedBribe = new UIMenuItem("Bribe Police", "Bribe the police to let you go. Don't be cheap.");
            menuBustedSurrender = new UIMenuItem("Surrender", "Surrender and get out on bail. Lose bail money and your guns.");
            menuBustedRespawnInPlace = new UIMenuItem("Respawn In Place", "Respawn at this exact spot.");
            menuBustedTakeoverRandomPed = new UIMenuListItem("Takeover Random Pedestrian", "Takes over a random pedestrian around the player.", new List<dynamic> { "Closest", "20 M", "40 M", "60 M", "100 M", "500 M" });

            bustedMenu.AddItem(menuBustedResistArrest);
            bustedMenu.AddItem(menuBustedBribe);
            bustedMenu.AddItem(menuBustedSurrender);
            bustedMenu.AddItem(menuBustedTakeoverRandomPed);

            mainMenu.OnItemSelect += OnItemSelect;
            mainMenu.OnListChange += OnListChange;
            mainMenu.OnCheckboxChange += OnCheckboxChange;
            deathMenu.OnItemSelect += OnItemSelect;
            deathMenu.OnListChange += OnListChange;
            bustedMenu.OnItemSelect += OnItemSelect;
            bustedMenu.OnListChange += OnListChange;
            debugMenu.OnItemSelect += OnItemSelect;
            debugMenu.OnListChange += OnListChange;
            debugMenu.OnCheckboxChange += OnCheckboxChange;

            ProcessLoop();

        }

        private static void CreateOptionsMenu()
        {
            optionsMenu = menuPool.AddSubMenu(mainMenu, "Options");
            UIMenuItem ReloadSettings = new UIMenuItem("Reload Settings", "Reload settings from XML");
            optionsMenu.AddItem(ReloadSettings);
            foreach (FieldInfo fi in Type.GetType("Settings", false).GetFields())
            {
                if (fi.FieldType == typeof(bool))
                {
                    UIMenuCheckboxItem MySetting = new UIMenuCheckboxItem(fi.Name, (bool)fi.GetValue(null));
                    optionsMenu.AddItem(MySetting);
                }
                if (fi.FieldType == typeof(int) || fi.FieldType == typeof(string) || fi.FieldType == typeof(float))
                {
                    UIMenuItem MySetting = new UIMenuItem(string.Format("{0}: {1}",fi.Name, fi.GetValue(null)));
                    optionsMenu.AddItem(MySetting);
                }
            }
            optionsMenu.OnItemSelect += OnItemSelect;
            optionsMenu.OnListChange += OnListChange;
            optionsMenu.OnCheckboxChange += OnCheckboxChange;
            optionsMenu.RefreshIndex();
        }

        public static void CreateMainMenu()
        {
            mainMenu.Clear();
            menuMainSuicide = new UIMenuItem("Suicide", "Commit Suicide");
            menuMainTakeoverRandomPed = new UIMenuListItem("Takeover Random Pedestrian", "Takes over a random pedestrian around the player.", new List<dynamic> { "Closest", "20 M", "40 M", "60 M", "100 M", "500 M" });
            menuMainChangeLicensePlate = new UIMenuListItem("Change Plate", "Change your license plate if you have spares.", InstantAction.SpareLicensePlates);//new UIMenuItem("Change Plate", "Change your license plate if you have spares");
            menuMainRemoveLicensePlate = new UIMenuItem("Remove Plate", "Removes the plate of the nearest vehicle");
            menuMainChangeHelmet = new UIMenuItem("Toggle Helmet", "Add/Removes your helmet");

            mainMenu.AddItem(menuMainTakeoverRandomPed);
            mainMenu.AddItem(menuMainSuicide);
            if (!InstantAction.PlayerInVehicle)
            {
                mainMenu.AddItem(menuMainChangeLicensePlate);
                mainMenu.AddItem(menuMainRemoveLicensePlate);
                //mainMenu.AddItem(menuMainChangeHelmet); //doesnt work fully so far, and only on certain peds
            }
            CreateOptionsMenu();
        }
        public static void UpdateLists()
        {         
            CreateMainMenu();
        }
        public static void OnCheckboxChange(UIMenu sender, UIMenuCheckboxItem checkbox, bool Checked)
        {
            if (sender == optionsMenu)
            {
                FieldInfo[] MyFields = Type.GetType("Settings", false).GetFields();
                FieldInfo MySetting = MyFields.Where(x => x.Name == checkbox.Text).FirstOrDefault();
                MySetting.SetValue(null, Checked);
                Settings.WriteSettings();
            }
        }
        public static void OnListChange(UIMenu sender, UIMenuListItem list, int index)
        {
            if (sender == mainMenu)
            {
                if (list == menuMainRandomCrime)
                    RandomCrimeLevel = list.Index + 1;
                if (list == menuMainTakeoverRandomPed)
                {
                    if (index == 0)
                        TakeoverRadius = -1f;
                    else if(index == 1)
                        TakeoverRadius = 20f;
                    else if (index == 2)
                        TakeoverRadius = 40f;
                    else if (index == 3)
                        TakeoverRadius = 60f;
                    else if (index == 4)
                        TakeoverRadius = 100f;
                    else if (index == 5)
                        TakeoverRadius = 500f;
                }
                if(list == menuMainChangeLicensePlate)
                {
                    ChangePlateIndex = index;
                }
            }

            //else if (sender == bustedMenu)
            //{
            //    if (list == menuBustedBribe)
            //    {
            //        BribeAmount = BribeList[list.Index];// + 1;
            //        InstantAction.WriteToLog("Bribe Changed", String.Format("Bribe: {0}", BribeAmount));
            //    }
            //}
            else if (sender == debugMenu)
            {
                if (list == menuDebugScreenEffect)
                    CurrentScreenEffect = list.Collection[index].ToString();
                if (list == menuDebugRandomWeapon)
                    RandomWeaponLevel = list.Index;
            }
        }
        public static void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
        {
            if (sender == mainMenu)
            {
                if (selectedItem == menuMainTakeoverRandomPed)
                {
                    if(TakeoverRadius == -1f)
                        InstantAction.TakeoverPed(InstantAction.GetPedestrian(500f,true), false, false);
                    else
                        InstantAction.TakeoverPed(InstantAction.GetPedestrian(TakeoverRadius, false), false, false);
                }
                else if (selectedItem == menuMainSuicide)
                {
                    InstantAction.CommitSuicide(Game.LocalPlayer.Character);
                }
                else if (selectedItem == menuMainChangeLicensePlate)
                {
                    InstantAction.ChangeNearestLicensePlate();
                }
                else if (selectedItem == menuMainRemoveLicensePlate)
                {
                    InstantAction.RemoveNearestLicensePlate();
                }
                else if (selectedItem == menuMainChangeHelmet)
                {
                    InstantAction.AddRemovePlayerHelmet();                
                }
                else if (selectedItem == menuMainReloadSettings)
                {
                    Settings.ReadSettings();
                }
                mainMenu.Visible = false;
            }
            else if (sender == bustedMenu)
            {
                if (selectedItem == menuBustedResistArrest)
                {
                    InstantAction.ResistArrest();
                }
                else if (selectedItem == menuBustedBribe)
                {
                    int BribeAmount;
                    if (int.TryParse(GetKeyboardInput(), out BribeAmount))
                    {
                        InstantAction.BribePolice(BribeAmount);
                    }
                }
                else if (selectedItem == menuBustedSurrender)
                {
                    InstantAction.Surrender();
                }
                else if (selectedItem == menuBustedTakeoverRandomPed)
                {
                    if (TakeoverRadius == -1f)
                        InstantAction.TakeoverPed(InstantAction.GetPedestrian(500f, true), false, false);
                    else
                        InstantAction.TakeoverPed(InstantAction.GetPedestrian(TakeoverRadius, false), false, false);
                }
                bustedMenu.Visible = false;
            }
            else if (sender == deathMenu)
            {
                if (selectedItem == menuDeathUndie)
                {
                    InstantAction.RespawnInPlace(true);
                }
                else if (selectedItem == menuDeathHospitalRespawn)
                {
                    InstantAction.RespawnAtHospital();
                }
                else if (selectedItem == menuDeathTakeoverRandomPed)
                {
                    if (TakeoverRadius == -1f)
                        InstantAction.TakeoverPed(InstantAction.GetPedestrian(500f, true), false, false);
                    else
                        InstantAction.TakeoverPed(InstantAction.GetPedestrian(TakeoverRadius, false), false, false);
                }
                deathMenu.Visible = false;
            }
            else if (sender == debugMenu)
            {
                if (selectedItem == menuDebugKillPlayer)
                {
                    Game.LocalPlayer.Character.Kill();
                }
                if (selectedItem == menuDebugRandomWeapon)
                {
                    GTAWeapon myGun = InstantAction.GetRandomWeapon(RandomWeaponLevel);
                    Game.LocalPlayer.Character.Inventory.GiveNewWeapon(myGun.Name, myGun.AmmoAmount, true);
                    if (myGun.PlayerVariations.Any())
                        InstantAction.ApplyWeaponVariation(Game.LocalPlayer.Character, (uint)myGun.Hash, myGun.PlayerVariations.PickRandom());
                }
                if (selectedItem == menuDebugGiveMoney)
                {
                    Game.LocalPlayer.Character.GiveCash(5000, Settings.MainCharacterToAlias);
                }
                if (selectedItem == menuDebugScreenEffect)
                {
                    NativeFunction.Natives.xB4EDDC19532BFB85();
                }
                if (selectedItem == menuDebugHealthAndArmor)
                {
                    Game.LocalPlayer.Character.Health = 100;
                    Game.LocalPlayer.Character.Armor = 100;
                }

                
                debugMenu.Visible = false;
            }  
            else if(sender == optionsMenu)
            {
                string mySettingName = selectedItem.Text.Split(':')[0];
                FieldInfo[] MyFields = Type.GetType("Settings", false).GetFields();
                FieldInfo MySetting = MyFields.Where(x => x.Name == mySettingName).FirstOrDefault();

                string Value = GetKeyboardInput();
                if(MySetting.FieldType == typeof(float))
                {
                    float myFloat;
                    if(float.TryParse(Value,out myFloat))
                    {
                        MySetting.SetValue(null, myFloat);
                        selectedItem.Text = string.Format("{0}: {1}", mySettingName, Value);
                        Settings.WriteSettings();
                    }
                }
                else if (MySetting.FieldType == typeof(int))
                {
                    int myInt;
                    if (int.TryParse(Value, out myInt))
                    {
                        MySetting.SetValue(null, myInt);
                        selectedItem.Text = string.Format("{0}: {1}", mySettingName, Value);
                        Settings.WriteSettings();
                    }
                }
                else if (MySetting.FieldType == typeof(string))
                {
                    MySetting.SetValue(null, Value);
                    selectedItem.Text = string.Format("{0}: {1}", mySettingName, Value);
                    Settings.WriteSettings();
                }

            }
        }
        public static void ProcessLoop()
        {
            GameFiber.StartNew(delegate
            {
                while (true)
                {
                    if (Game.IsKeyDown(Keys.F10)) // Our menu on/off switch.
                    {
                        if (InstantAction.isDead)
                        {
                            bustedMenu.Visible = false;
                            mainMenu.Visible = false;
                            deathMenu.Visible = !deathMenu.Visible;
                        }
                        else if (InstantAction.isBusted)
                        {
                            deathMenu.Visible = false;
                            mainMenu.Visible = false;
                            bustedMenu.Visible = !bustedMenu.Visible;
                        }
                        else if(optionsMenu.Visible)
                        {

                        }
                        else
                        {
                            UpdateLists();
                            bustedMenu.Visible = false;
                            deathMenu.Visible = false;
                            mainMenu.Visible = !mainMenu.Visible;
                        }
                    }
                    else if (Game.IsKeyDown(Keys.F11)) // Our menu on/off switch.
                    {
                        debugMenu.Visible = !debugMenu.Visible;
                    }
                    menuPool.ProcessMenus();       // Process all our menus: draw the menu and process the key strokes and the mouse. 

                    if (Settings.UndieLimit == 0)
                    {
                        menuDeathUndie.Enabled = true;
                    }
                    else if (InstantAction.TimesDied < Settings.UndieLimit)
                    {
                        menuDeathUndie.Enabled = true;
                    }
                    else
                    {
                        menuDeathUndie.Enabled = false;
                    }
                    GameFiber.Yield();
                }
            });
        }
        public static string GetKeyboardInput()
        {
            NativeFunction.CallByName<bool>("DISPLAY_ONSCREEN_KEYBOARD", true, "FMMC_KEY_TIP8", "", "", "", "", "", 255 + 1);

            while (NativeFunction.CallByName<int>("UPDATE_ONSCREEN_KEYBOARD") == 0)
            {
                GameFiber.Sleep(500);
            }
            string Value = "";
            unsafe
            {
                IntPtr ptr = Rage.Native.NativeFunction.CallByName<IntPtr>("GET_ONSCREEN_KEYBOARD_RESULT");
                Value = Marshal.PtrToStringAnsi(ptr);
            }
            return Value;
        }
        

    }

}

