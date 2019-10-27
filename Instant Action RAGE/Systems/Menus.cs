using ExtensionsMethods;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
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
       // private static UIMenuItem menuMainOptions;

        private static UIMenuCheckboxItem menuOptionsAutoRespawn;
        private static UIMenuCheckboxItem menuOptionsRandomEvents;
        private static UIMenuCheckboxItem menuOptionsAllowBust;
        private static UIMenuCheckboxItem menuOptionsPoliceEnhancements;
        private static UIMenuListItem menuOptionsUndieLimit;
        private static UIMenuCheckboxItem menuOptionsReplacePlayerWithPed;
        private static UIMenuListItem menuOptionsReplacePlayerWithPedCharacter;
        private static UIMenuCheckboxItem menuOptionsReplacePlayerWithPedRandomMoney;
        private static UIMenuCheckboxItem menuOptionsDebug;

        private static UIMenuItem menuDebugResetCharacter;
        private static UIMenuItem menuMainSuicide;
        private static UIMenuItem menuMainChangeLicensePlate;
        private static UIMenuItem menuMainRemoveLicensePlate;
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
        private static UIMenuListItem menuBustedBribe;
        private static UIMenuItem menuBustedRespawnInPlace;
        private static UIMenuListItem menuBustedRandomCrime;
        private static UIMenuItem menuBustedNormalRespawn;
        private static UIMenuItem menuBustedTakeoverNearestPed;
        private static UIMenuListItem menuBustedTakeoverRandomPed;
        private static UIMenuItem menuBustedSurrender;
        private static UIMenuItem menuDeathHospitalRespawn;

        private static MenuPool menuPool;
        public static UIMenu mainMenu;
        public static UIMenu deathMenu;
        public static UIMenu debugMenu;
        public static UIMenu bustedMenu;
        public static UIMenu optionsMenu;

        private static int RandomCrimeLevel = 1;
        private static int RandomWeaponLevel = 0;
        private static int BribeAmount = 2000;
        private static List<int> BribeList = new List<int> { 250, 500, 1000, 1250, 1750, 2000, 3500 };
        private static List<int> UndieLimit = new List<int> { 0,1,2,3,4,5 };
        private static string CurrentScreenEffect = "Rampage";
        private static float TakeoverRadius = -1f;
        public static int ChangePlateIndex = 0;
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

            //optionsMenu = new UIMenu("Instant Action", "Change Options");
            //menuPool.Add(optionsMenu);

            CreateMainMenu();
            // mainMenu.AddItem(menuMainOptions);


            //menuOptionsAutoRespawn = new UIMenuCheckboxItem("AutoRespawn Enabled", Settings.AutoRespawn, "Sets if the game will automatically handle the respawn logic or show the options menu.");
            //menuOptionsRandomEvents = new UIMenuCheckboxItem("Random Events", Settings.RandomEvents, "Can Generate Randome crimes in the world");
            //menuOptionsAllowBust = new UIMenuCheckboxItem("Allow Bust Opportunities", Settings.BetterChasesAllowBustOportunity, "Adds features to BetterChases Bust Opportunities feature");
            //menuOptionsPoliceEnhancements = new UIMenuCheckboxItem("Police Enhancements", Settings.PoliceEnhancements, "Adds some features to the police AI");


            //menuOptionsUndieLimit = new UIMenuListItem("Undie Limit", "Limit how many times you can Undie as the same character, set to 0 for unlimited", new List<dynamic> { "0", "1", "2", "3", "4", "5" });
            //menuOptionsReplacePlayerWithPed = new UIMenuCheckboxItem("Replace Player with Ped", Settings.ReplacePlayerWithPed, "If true, it will trick the game into thinking your player ped is one of the main characters.You have money and can go to shops");
            //menuOptionsReplacePlayerWithPedCharacter = new UIMenuListItem("Replace Ped with:", "Select to Change Hash Now. Options: Michael, Trevor, Franklin", new List<dynamic> { "Michael", "Franklin", "Trevor" });
            //menuOptionsReplacePlayerWithPedRandomMoney = new UIMenuCheckboxItem("Random Money On Takeover", Settings.ReplacePlayerWithPedRandomMoney, "If true, will reset the players money on each takeover");
            //menuOptionsDebug = new UIMenuCheckboxItem("Debug Enabled", Settings.Debug, "Debug for testing");

            //optionsMenu.AddItem(menuOptionsAutoRespawn);
            //optionsMenu.AddItem(menuOptionsRandomEvents);
            //optionsMenu.AddItem(menuOptionsAllowBust);
            //optionsMenu.AddItem(menuOptionsPoliceEnhancements);
            //optionsMenu.AddItem(menuOptionsUndieLimit);
            //optionsMenu.AddItem(menuOptionsReplacePlayerWithPed);
            //optionsMenu.AddItem(menuOptionsReplacePlayerWithPedCharacter);
            //optionsMenu.AddItem(menuOptionsReplacePlayerWithPedRandomMoney);
            //optionsMenu.AddItem(menuOptionsDebug);


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

            debugMenu.AddItem(menuDebugResetCharacter);
            debugMenu.AddItem(menuDebugKillPlayer);
            debugMenu.AddItem(menuDebugRandomWeapon);
            debugMenu.AddItem(menuDebugScreenEffect);
            debugMenu.AddItem(menuDebugEnabled);

            //
            menuDeathUndie = new UIMenuItem("Un-Die", "Respawn at this exact spot as yourself.");
            menuDeathRespawnInPlace = new UIMenuItem("Respawn In Place", "Respawn at this exact spot.");
            menuDeathHospitalRespawn = new UIMenuItem("Give Up", "Respawn at the nearest hospital. Lose $5K and your guns.");
           // menuDeathRandomCrime = new UIMenuListItem("Start Random Crime", "Start Random Crime", new List<dynamic> { "Level 1", "Level 2", "Level 3" });
            menuDeathNormalRespawn = new UIMenuItem("Standard Respawn", "Respawn at the hospital (standard game logc).");
            //menuDeathTakeoverNearestPed = new UIMenuItem("Takeover Nearest Pedestrian", "Takes over the nearest pedestrian to the player.");
            menuDeathTakeoverRandomPed = new UIMenuListItem("Takeover Random Pedestrian", "Takes over a random pedestrian around the player.", new List<dynamic> {"Closest", "20 M", "40 M", "60 M", "100 M", "500 M" });

            deathMenu.AddItem(menuDeathUndie);
            //DeathMenu.AddItem(menuDeathRespawnInPlace);
            deathMenu.AddItem(menuDeathHospitalRespawn);
            //deathMenu.AddItem(menuDeathRandomCrime);
            //DeathMenu.AddItem(menuDeathNormalRespawn);
           //deathMenu.AddItem(menuDeathTakeoverNearestPed);
            deathMenu.AddItem(menuDeathTakeoverRandomPed);

            //
            menuBustedResistArrest = new UIMenuItem("Resist Arrest", "Better hope you're strapped.");
            menuBustedBribe = new UIMenuListItem("Bribe Police", "Bribe the police to let you go. Don't be cheap.",new List<dynamic> { 250, 500, 1000, 1250, 1750, 2000, 3500 } );
            menuBustedSurrender = new UIMenuItem("Surrender", "Surrender and get out on bail. Lose bail money and your guns.");
            menuBustedRespawnInPlace = new UIMenuItem("Respawn In Place", "Respawn at this exact spot.");
            menuBustedRandomCrime = new UIMenuListItem("Start Random Crime", "Start Random Crime", new List<dynamic> { "Level 1", "Level 2", "Level 3" });
            menuBustedNormalRespawn = new UIMenuItem("Standard Respawn", "Respawn at the police station (standard game logc).");
            menuBustedTakeoverNearestPed = new UIMenuItem("Takeover Nearest Pedestrian", "Takes over the nearest pedestrian to the player.");
            menuBustedTakeoverRandomPed = new UIMenuListItem("Takeover Random Pedestrian", "Takes over a random pedestrian around the player.", new List<dynamic> { "Closest", "20 M", "40 M", "60 M", "100 M", "500 M" });

            bustedMenu.AddItem(menuBustedResistArrest);
            bustedMenu.AddItem(menuBustedBribe);
            bustedMenu.AddItem(menuBustedSurrender);
            //BustedMenu.AddItem(menuBustedRespawnInPlace);
           // bustedMenu.AddItem(menuBustedRandomCrime);
            //BustedMenu.AddItem(menuBustedNormalRespawn);
           // bustedMenu.AddItem(menuBustedTakeoverNearestPed);
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
            //optionsMenu.OnItemSelect += OnItemSelect;
            //optionsMenu.OnListChange += OnListChange;
            //optionsMenu.OnCheckboxChange += OnCheckboxChange;
            ProcessLoop();

        }
        public static void CreateMainMenu()
        {
            //
            menuMainSuicide = new UIMenuItem("Suicide", "Commit Suicide");
            // menuMainRandomCrime = new UIMenuListItem("Start Random Crime","Random Crime", new List<dynamic> { "Level 1", "Level 2", "Level 3" });
            //menuMainTakeoverNearestPed = new UIMenuItem("Takeover Nearest Pedestrian", "Takes over the nearest pedestrian to the player.");
            menuMainTakeoverRandomPed = new UIMenuListItem("Takeover Random Pedestrian", "Takes over a random pedestrian around the player.", new List<dynamic> { "Closest", "20 M", "40 M", "60 M", "100 M", "500 M" });
            //menuMainOptions = new UIMenuItem("Options", "Change options");
            menuMainChangeLicensePlate = new UIMenuListItem("Change Plate", "Change your license plate if you have spares.", InstantAction.SpareLicensePlates);//new UIMenuItem("Change Plate", "Change your license plate if you have spares");
            menuMainRemoveLicensePlate = new UIMenuItem("Remove Plate", "Removes the plate of the nearest vehicle");
            // mainMenu.AddItem(menuMainRandomCrime);
            //mainMenu.AddItem(menuMainTakeoverNearestPed);
            mainMenu.AddItem(menuMainTakeoverRandomPed);
            mainMenu.AddItem(menuMainSuicide);
            mainMenu.AddItem(menuMainChangeLicensePlate);
            mainMenu.AddItem(menuMainRemoveLicensePlate);
            // mainMenu.AddItem(menuMainOptions);
        }
        public static void UpdateLists()
        {
            mainMenu.Clear();
            CreateMainMenu();
        }
        public static void OnCheckboxChange(UIMenu sender, UIMenuCheckboxItem checkbox, bool Checked)
        {
            if (sender == debugMenu)
            {
                if (checkbox == menuDebugEnabled)
                {
                    Settings.Debug = Checked;
                    Settings.WriteSettings("Debug", Settings.Debug.ToString());
                }
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

            else if (sender == bustedMenu)
            {
                if (list == menuBustedBribe)
                {
                    BribeAmount = BribeList[list.Index];// + 1;
                    InstantAction.WriteToLog("Bribe Changed", String.Format("Bribe: {0}", BribeAmount));
                }
            }
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
                    int CurrentCash = Game.LocalPlayer.Character.GetCash(Settings.MainCharacterToAlias);
                    if (CurrentCash < BribeAmount)
                        return;
                    InstantAction.BribePolice(BribeAmount);
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
                    InstantAction.KillPlayer();
                }
                if (selectedItem == menuDebugRandomWeapon)
                {
                    GTAWeapon myGun = InstantAction.GetRandomWeapon(RandomWeaponLevel);
                    Game.LocalPlayer.Character.Inventory.GiveNewWeapon(myGun.Name, myGun.AmmoAmount, true);
                }
                debugMenu.Visible = false;
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
                        else
                        {
                            bustedMenu.Visible = false;
                            deathMenu.Visible = false;
                            mainMenu.Visible = !mainMenu.Visible;
                        }
                    }
                    else if (Game.IsKeyDown(Keys.F11)) // Our menu on/off switch.
                    {
                        debugMenu.Visible = !debugMenu.Visible;
                    }
                    //else if (Game.IsKeyDown(Keys.F12)) // Our menu on/off switch.
                    //{
                    //    bustedMenu.Visible = !bustedMenu.Visible;
                    //}

                    menuPool.ProcessMenus();       // Process all our menus: draw the menu and process the key strokes and the mouse. 
                    GameFiber.Yield();
                }
            });
        }

    }

}

