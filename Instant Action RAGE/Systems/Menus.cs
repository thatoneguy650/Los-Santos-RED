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
        private static UIMenuItem menuMainOptions;

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
        private static UIMenuItem menuDebugKillPlayer;
        private static UIMenuListItem menuDebugRandomWeapon;
        private static UIMenuListItem menuDebugScreenEffect;
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
        private static string CurrentScreenEffect = "Rampage";
        private static float TakeoverRadius = 500f;
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

            optionsMenu = new UIMenu("Instant Action", "Change Options");
            menuPool.Add(optionsMenu);

            //
            menuMainRandomCrime = new UIMenuListItem("Start Random Crime", new List<dynamic> { "Level 1", "Level 2", "Level 3" }, 0);
            menuMainTakeoverNearestPed = new UIMenuItem("Takeover Nearest Pedestrian", "Takes over the nearest pedestrian to the player.");
            menuMainTakeoverRandomPed = new UIMenuListItem("Takeover Random Pedestrian", new List<dynamic> { "20 M", "40 M", "60 M", "100 M", "500 M" }, 0, "Takes over a random pedestrian around the player.");
            menuMainOptions = new UIMenuItem("Options", "Change options");

            mainMenu.AddItem(menuMainRandomCrime);
            mainMenu.AddItem(menuMainTakeoverNearestPed);
            mainMenu.AddItem(menuMainTakeoverRandomPed);
            mainMenu.AddItem(menuMainOptions);

            //
            //menuOptionsAutoRespawn = new UIMenuCheckboxItem("AutoRespawn Enabled", mySettings.AutoRespawn, "Sets if the game will automatically handle the respawn logic or show the options menu.");
            //menuOptionsRandomEvents = new UIMenuCheckboxItem("Random Events", mySettings.RandomEvents, "Can Generate Randome crimes in the world");
            //menuOptionsAllowBust = new UIMenuCheckboxItem("Allow Bust Opportunities", mySettings.BetterChasesAllowBustOportunity, "Adds features to BetterChases Bust Opportunities feature");
            //menuOptionsPoliceEnhancements = new UIMenuCheckboxItem("Police Enhancements", mySettings.PoliceEnhancements, "Adds some features to the police AI");
            //menuOptionsUndieLimit = new UIMenuListItem("Undie Limit", new List<dynamic> { "0", "1", "2", "3", "4", "5" }, mySettings.UndieLimit, "Limit how many times you can Undie as the same character, set to 0 for unlimited");
            //menuOptionsReplacePlayerWithPed = new UIMenuCheckboxItem("Replace Player with Ped", mySettings.ReplacePlayerWithPed, "If true, it will trick the game into thinking your player ped is one of the main characters.You have money and can go to shops");
            //menuOptionsReplacePlayerWithPedCharacter = new UIMenuListItem("Replace Ped with:", new List<dynamic> { "Michael", "Franklin", "Trevor" }, mySettings.IntReplacePlayerWithPedCharacter, "Select to Change Hash Now. Options: Michael, Trevor, Franklin");
            //menuOptionsReplacePlayerWithPedRandomMoney = new UIMenuCheckboxItem("Random Money On Takeover", mySettings.ReplacePlayerWithPedRandomMoney, "If true, will reset the players money on each takeover");
            //menuOptionsDebug = new UIMenuCheckboxItem("Debug Enabled", mySettings.Debug, "Debug for testing");

            //optionsMenu.AddItem(menuOptionsAutoRespawn);
            //optionsMenu.AddItem(menuOptionsRandomEvents);
            //optionsMenu.AddItem(menuOptionsAllowBust);
            //optionsMenu.AddItem(menuOptionsPoliceEnhancements);
            //optionsMenu.AddItem(menuOptionsUndieLimit);
            //optionsMenu.AddItem(menuOptionsReplacePlayerWithPed);
            //optionsMenu.AddItem(menuOptionsReplacePlayerWithPedCharacter);
            //optionsMenu.AddItem(menuOptionsReplacePlayerWithPedRandomMoney);
            //optionsMenu.AddItem(menuOptionsDebug);

            //
            menuDebugResetCharacter = new UIMenuItem("Reset Character", "Change your character back to the default model.");
            menuDebugKillPlayer = new UIMenuItem("Kill Player", "Immediatly die and ragdoll");
            menuDebugRandomWeapon = new UIMenuListItem("Get Random Weapon", new List<dynamic> { "Level 0", "Level 1", "Level 2", "Level 3", "Level 4" }, 0, "Gives the Player a random weapon and ammo.");
            menuDebugScreenEffect = new UIMenuListItem("Play Screen Effect", new List<dynamic> { "SwitchHUDIn",
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
        "Dont_tazeme_bro" }, 0, "");

            debugMenu.AddItem(menuDebugResetCharacter);
            debugMenu.AddItem(menuDebugKillPlayer);
            debugMenu.AddItem(menuDebugRandomWeapon);
            debugMenu.AddItem(menuDebugScreenEffect);

            //
            menuDeathUndie = new UIMenuItem("Un-Die", "Respawn at this exact spot as yourself.");
            menuDeathRespawnInPlace = new UIMenuItem("Respawn In Place", "Respawn at this exact spot.");
            menuDeathHospitalRespawn = new UIMenuItem("Give Up", "Respawn at the nearest hospital. Lose $5K and your guns.");
            menuDeathRandomCrime = new UIMenuListItem("Start Random Crime", new List<dynamic> { "Level 1", "Level 2", "Level 3" }, 0);
            menuDeathNormalRespawn = new UIMenuItem("Standard Respawn", "Respawn at the hospital (standard game logc).");
            menuDeathTakeoverNearestPed = new UIMenuItem("Takeover Nearest Pedestrian", "Takes over the nearest pedestrian to the player.");
            menuDeathTakeoverRandomPed = new UIMenuListItem("Takeover Random Pedestrian", new List<dynamic> { "20 M", "40 M", "60 M", "100 M", "500 M" }, 0, "Takes over a random pedestrian around the player.");

            deathMenu.AddItem(menuDeathUndie);
            //DeathMenu.AddItem(menuDeathRespawnInPlace);
            deathMenu.AddItem(menuDeathHospitalRespawn);
            deathMenu.AddItem(menuDeathRandomCrime);
            //DeathMenu.AddItem(menuDeathNormalRespawn);
            deathMenu.AddItem(menuDeathTakeoverNearestPed);
            deathMenu.AddItem(menuDeathTakeoverRandomPed);

            //
            menuBustedResistArrest = new UIMenuItem("Resist Arrest", "Better hope you're strapped.");
            menuBustedBribe = new UIMenuListItem("Bribe Police", new List<dynamic> { 250, 500, 1000, 1250, 1750, 2000, 3500 }, 500, "Bribe the police to let you go. Don't be cheap.");
            menuBustedSurrender = new UIMenuItem("Surrender", "Surrender and get out on bail. Lose bail money and your guns.");
            menuBustedRespawnInPlace = new UIMenuItem("Respawn In Place", "Respawn at this exact spot.");
            menuBustedRandomCrime = new UIMenuListItem("Start Random Crime", new List<dynamic> { "Level 1", "Level 2", "Level 3" }, 0);
            menuBustedNormalRespawn = new UIMenuItem("Standard Respawn", "Respawn at the police station (standard game logc).");
            menuBustedTakeoverNearestPed = new UIMenuItem("Takeover Nearest Pedestrian", "Takes over the nearest pedestrian to the player.");
            menuBustedTakeoverRandomPed = new UIMenuListItem("Takeover Random Pedestrian", new List<dynamic> { "20 M", "40 M", "60 M", "100 M", "500 M" }, 0, "Takes over a random pedestrian around the player.");

            bustedMenu.AddItem(menuBustedResistArrest);
            bustedMenu.AddItem(menuBustedBribe);
            bustedMenu.AddItem(menuBustedSurrender);
            //BustedMenu.AddItem(menuBustedRespawnInPlace);
            bustedMenu.AddItem(menuBustedRandomCrime);
            //BustedMenu.AddItem(menuBustedNormalRespawn);
            bustedMenu.AddItem(menuBustedTakeoverNearestPed);
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
            optionsMenu.OnItemSelect += OnItemSelect;
            optionsMenu.OnListChange += OnListChange;
            optionsMenu.OnCheckboxChange += OnCheckboxChange;
            ProcessLoop();

        }
        public static void OnItemChange(UIMenu sender, int index)
        {

        }
        public static void OnCheckboxChange(UIMenu sender, UIMenuCheckboxItem checkbox, bool Checked)
        {

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
                        TakeoverRadius = 20f;
                    else if (index == 1)
                        TakeoverRadius = 40f;
                    else if (index == 2)
                        TakeoverRadius = 60f;
                    else if (index == 3)
                        TakeoverRadius = 100f;
                    else if (index == 4)
                        TakeoverRadius = 500f;
                }
            }
            else if (sender == deathMenu)
            {
                if (list == menuDeathRandomCrime)
                    RandomCrimeLevel = list.Index + 1;
            }
            else if (sender == bustedMenu)
            {
                if (list == menuBustedBribe)
                {
                    BribeAmount = BribeList[list.Index];// + 1;
                    if (BribeAmount > Game.LocalPlayer.Character.GetCash("Michael"))
                    {
                        menuBustedBribe.Enabled = false;
                    }
                    else
                    {
                        menuBustedBribe.Enabled = true;
                    }
                }
            }
            //else if (sender == optionsMenu)
            //{
            //    if (list == menuOptionsReplacePlayerWithPedCharacter)
            //    {
            //        String Char = "Michael";
            //        if (index == 0)
            //            Char = "Michael";
            //        else if (index == 1)
            //            Char = "Franklin";
            //        else if (index == 2)
            //            Char = "Trevor";
            //        mySettings.ReplacePlayerWithPedCharacter = Char;
            //        mySettings.WriteSettings("ReplacePlayerWithPedCharacter", mySettings.ReplacePlayerWithPedCharacter);
            //    }
            //    else if (item == menuOptionsUndieLimit)
            //    {
            //        mySettings.UndieLimit = item.Index;
            //        mySettings.WriteSettings("UndieLimit", mySettings.UndieLimit.ToString());
            //    }
            //}
            else if (sender == debugMenu)
            {
                if (list == menuDebugScreenEffect)
                    CurrentScreenEffect = list.IndexToItem(index).ToString();
                if (list == menuDebugRandomWeapon)
                    RandomWeaponLevel = list.Index + 1;
            }
        }
        public static void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
        {
            if (sender == debugMenu)
            {
                if(selectedItem == menuDebugKillPlayer)
                {
                    InstantAction.KillPlayer();
                }
                if (selectedItem == menuDebugRandomWeapon)
                {
                    Game.LocalPlayer.Character.Inventory.GiveNewWeapon(WeaponHash.Pistol,20,true);
                }
                debugMenu.Visible = false;
            }
            else if (sender == deathMenu)
            {
                if (selectedItem == menuDeathUndie)
                {
                    InstantAction.RespawnInPlace(false);
                }
                else if (selectedItem == menuDeathTakeoverNearestPed)
                {
                    InstantAction.TakeoverPed(InstantAction.GetPedestrian(500f, true), true, false);
                }
                else if (selectedItem == menuDeathTakeoverRandomPed)
                {
                    InstantAction.TakeoverPed(InstantAction.GetPedestrian(TakeoverRadius, false), true, false);
                }
                deathMenu.Visible = false;
            }
            else if (sender == mainMenu)
            {
                if (selectedItem == menuMainTakeoverNearestPed)
                {
                    InstantAction.TakeoverPed(InstantAction.GetPedestrian(500f, true), true, false);
                }
                else if (selectedItem == menuMainTakeoverRandomPed)
                {
                    InstantAction.TakeoverPed(InstantAction.GetPedestrian(TakeoverRadius, false), true, false);
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
                    InstantAction.BribePolice(3500);
                }
                else if (selectedItem == menuBustedSurrender)
                {
                    InstantAction.BribePolice(3500);
                }
                else if (selectedItem == menuBustedTakeoverNearestPed)
                {
                    InstantAction.TakeoverPed(InstantAction.GetPedestrian(500f, true), true, false);
                }
                else if (selectedItem == menuBustedTakeoverRandomPed)
                {
                    InstantAction.TakeoverPed(InstantAction.GetPedestrian(TakeoverRadius, false), true, false);
                }
                bustedMenu.Visible = false;
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
                        mainMenu.Visible = !mainMenu.Visible;
                    }
                    else if (Game.IsKeyDown(Keys.F11)) // Our menu on/off switch.
                    {
                        debugMenu.Visible = !debugMenu.Visible;
                    }
                    else if (Game.IsKeyDown(Keys.F12)) // Our menu on/off switch.
                    {
                        deathMenu.Visible = !deathMenu.Visible;
                    }

                    menuPool.ProcessMenus();       // Process all our menus: draw the menu and process the key strokes and the mouse. 
                    GameFiber.Yield();
                }
            });
        }

    }

}

