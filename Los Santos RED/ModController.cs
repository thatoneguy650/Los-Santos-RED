using LosSantosRED.lsr.Helper;
using Rage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LosSantosRED.lsr
{
    public class ModController
    {
        private List<ModTaskGroup> TaskGroups;
        private Civilians Civilians;
        private Debug Debug;
        private Dispatcher Dispatcher;
        private Input Input;   
        private PedSwap PedSwap;   
        private Mod.Player Player;
        private Police Police;  
        private Tasker Tasker;
        private Mod.Time Time;
        private UI UI;
        private VanillaManager VanillaManager;
        private WavAudioPlayer WavAudioPlayer;
       // private NAudioPlayer NAudioPlayer;
        private Weather Weather;
        private Mod.World World;
        
        private ModDataFileManager ModDataFileManager;
        public ModController()
        {
            ModDataFileManager = new ModDataFileManager();
        }
        public bool IsRunning { get; private set; }
        public void Setup()
        {
            IsRunning = true;
            while (Game.IsLoading)
            {
                GameFiber.Yield();
            }
            ModDataFileManager = new ModDataFileManager();
            ModDataFileManager.Setup();

            WavAudioPlayer = new WavAudioPlayer();
            //NAudioPlayer = new NAudioPlayer();
            GameFiber.Yield();
            Time = new Mod.Time(ModDataFileManager.Settings);
            Time.Setup();
            GameFiber.Yield();
            World = new Mod.World(ModDataFileManager.Agencies, ModDataFileManager.Zones, ModDataFileManager.Jurisdictions, ModDataFileManager.Settings, ModDataFileManager.PlacesOfInterest, ModDataFileManager.PlateTypes, ModDataFileManager.Names, ModDataFileManager.RelationshipGroups, ModDataFileManager.Weapons, ModDataFileManager.Crimes, Time, ModDataFileManager.ShopMenus, ModDataFileManager.Interiors, WavAudioPlayer, ModDataFileManager.Gangs, ModDataFileManager.GangTerritories, ModDataFileManager.Streets);
            World.Setup();
            GameFiber.Yield();
            Player = new Mod.Player(Game.LocalPlayer.Character.Model.Name, Game.LocalPlayer.Character.IsMale, ModDataFileManager.Names.GetRandomName(Game.LocalPlayer.Character.Model.Name, Game.LocalPlayer.Character.IsMale), World, Time, ModDataFileManager.Streets, 
                ModDataFileManager.Zones, ModDataFileManager.Settings, ModDataFileManager.Weapons, ModDataFileManager.RadioStations, ModDataFileManager.Scenarios, ModDataFileManager.Crimes, WavAudioPlayer, ModDataFileManager.PlacesOfInterest, ModDataFileManager.Interiors, 
                ModDataFileManager.ModItems, ModDataFileManager.Intoxicants, ModDataFileManager.Gangs, ModDataFileManager.Jurisdictions, ModDataFileManager.GangTerritories, ModDataFileManager.GameSaves, ModDataFileManager.Names, ModDataFileManager.ShopMenus, 
                ModDataFileManager.RelationshipGroups, ModDataFileManager.DanceList, ModDataFileManager.SpeechList);
            Player.Setup();
            GameFiber.Yield();
            Police = new Police(World, Player, Player, ModDataFileManager.Settings, Player);
            GameFiber.Yield();
            Civilians = new Civilians(World, Player, Player, ModDataFileManager.Settings);
            GameFiber.Yield();
            PedSwap = new PedSwap(Time, Player, ModDataFileManager.Settings, World, ModDataFileManager.Weapons, ModDataFileManager.Crimes, ModDataFileManager.Names, ModDataFileManager.ModItems, World, ModDataFileManager.RelationshipGroups, ModDataFileManager.ShopMenus);
            GameFiber.Yield();
            Tasker = new Tasker(World, Player, ModDataFileManager.Weapons, ModDataFileManager.Settings, ModDataFileManager.PlacesOfInterest);
            Tasker.Setup();
            GameFiber.Yield();
            Dispatcher = new Dispatcher(World, Player, ModDataFileManager.Agencies, ModDataFileManager.Settings, ModDataFileManager.Streets, ModDataFileManager.Zones, ModDataFileManager.Jurisdictions, ModDataFileManager.Weapons, ModDataFileManager.Names, ModDataFileManager.Crimes, ModDataFileManager.RelationshipGroups, ModDataFileManager.Gangs, ModDataFileManager.GangTerritories, ModDataFileManager.ShopMenus, ModDataFileManager.PlacesOfInterest);
            Dispatcher.Setup();
            GameFiber.Yield();
            UI = new UI(Player, ModDataFileManager.Settings, ModDataFileManager.Jurisdictions, PedSwap, ModDataFileManager.PlacesOfInterest, Player, Player, Player, ModDataFileManager.Weapons, ModDataFileManager.RadioStations, ModDataFileManager.GameSaves, World, Player, Player, Tasker, Player, ModDataFileManager.ModItems, Time, Player, ModDataFileManager.Gangs, ModDataFileManager.GangTerritories, ModDataFileManager.Zones, ModDataFileManager.Streets, ModDataFileManager.Interiors, Dispatcher, ModDataFileManager.Agencies, Player, ModDataFileManager.DanceList, ModDataFileManager.GestureList, ModDataFileManager.ShopMenus, Player);
            UI.Setup();
            GameFiber.Yield();
            Input = new Input(Player, ModDataFileManager.Settings, UI, PedSwap);
            GameFiber.Yield();
            VanillaManager = new VanillaManager(ModDataFileManager.Settings, ModDataFileManager.PlacesOfInterest);
            VanillaManager.Setup();
            GameFiber.Yield();
            Weather = new Weather(WavAudioPlayer, ModDataFileManager.Settings, Time, Player);
            Weather.Setup();
            GameFiber.Yield();
            Debug = new Debug(ModDataFileManager.PlateTypes, World, Player, ModDataFileManager.Streets, Dispatcher, ModDataFileManager.Zones, ModDataFileManager.Crimes, this, ModDataFileManager.Settings, Tasker, Time, ModDataFileManager.Agencies, ModDataFileManager.Weapons, ModDataFileManager.ModItems, Weather, ModDataFileManager.PlacesOfInterest, ModDataFileManager.Interiors, ModDataFileManager.Gangs, Input, ModDataFileManager.ShopMenus);
            Debug.Setup();
            GameFiber.Yield();
            World.Setup();
            GameFiber.Yield();
            PedSwap.Setup();
            GameFiber.Yield();
            StartModLogic();
            GameFiber.Yield();
            StartCoreLogic();
            GameFiber.Yield();
            StartUILogic();
            GameFiber.Yield();
            StartInputLogic();
            GameFiber.Yield();
#if DEBUG
            StartDebugLogic();
            GameFiber.Yield();
#endif
            DisplayLoadSuccessfulMessage();
        }
        public void Dispose()
        {
            IsRunning = false;
            GameFiber.Sleep(500);
            Player.Dispose();
            World.Dispose();
            PedSwap.Dispose();
            Dispatcher.Dispose();
            VanillaManager.Dispose();
            UI.Dispose();
            Time.Dispose();
            Weather.Dispose();
            Debug.Dispose();
            Game.DisplayNotification("~s~Los Santos ~r~RED ~s~Deactivated");
            EntryPoint.WriteToConsole($"Has Been Deactivated",0);
        }
        private void StartModLogic()
        {
            TaskGroups = new List<ModTaskGroup>();
            TaskGroups.Add(new ModTaskGroup("Group1", new List<ModTask>()
            {
                 new ModTask(100, "Player.Update", Player.Update, 0),//1
                 new ModTask(250, "UI.UpdateData", UI.CacheData, 1),//500
            }));
            TaskGroups.Add(new ModTaskGroup("Group2", new List<ModTask>()
            {
                new ModTask(250, "Player.Violations.Update", Player.Violations.Update, 0),
                new ModTask(250, "Player.CurrentPoliceResponse.Update", Player.PoliceResponse.Update, 1),
                new ModTask(250, "Player.Investigation.Update", Player.Investigation.Update, 2),
                new ModTask(250, "Player.SearchModeUpdate", Player.SearchMode.Update, 3),

                new ModTask(250, "Player.TrafficViolationsUpdate", Player.Violations.UpdateTraffic, 4),
                new ModTask(250, "Player.LocationUpdate", Player.LocationUpdate, 5),
                new ModTask(250, "Player.ArrestWarrantUpdate",Player.CriminalHistory.Update, 6),//these were all 500
            }));
            TaskGroups.Add(new ModTaskGroup("Group3", new List<ModTask>()
            {
                new ModTask(1000, "World.PrunePedestrians", World.Pedestrians.Prune, 0),
                new ModTask(500, "World.CreateNewPedestrians", World.Pedestrians.CreateNew, 1), //very bad performance//500, need to up this somehow, we are stuck around 1250 to 1500, maybe just up the times?
                new ModTask(1000, "World.PruneVehicles", World.Vehicles.Prune, 2),//500
                new ModTask(1000, "World.CreateNewVehicles", World.Vehicles.CreateNew, 3), //very bad performance
                new ModTask(1000, "World.CleanUpVehicles", World.Vehicles.CleanUp, 4),
                new ModTask(1000, "World.UpdateVehiclePlates", World.Vehicles.PlateController.UpdatePlates, 5),
                new ModTask(1500, "Player.ScannerUpdate", Player.Scanner.Update, 6),//500
                //new ModTask(500, "VanillaManager.Tick", VanillaManager.Tick, 7),//2000
            }));
            TaskGroups.Add(new ModTaskGroup("Group4", new List<ModTask>()
            {
                new ModTask(1500, "Dispatcher.Recall", Dispatcher.Recall, 0),
                new ModTask(1500, "Dispatcher.Dispatch", Dispatcher.Dispatch, 1),
                //new ModTask(500, "Tasker.UpdatePolice", Tasker.UpdatePolice, 2),
                //new ModTask(500, "Tasker.UpdateCivilians", Tasker.UpdateCivilians, 3),
            }));
            TaskGroups.Add(new ModTaskGroup("Group5", new List<ModTask>()
            {
                new ModTask(250, "Police.Update", Police.Update,0),
            }));
            TaskGroups.Add(new ModTaskGroup("Group6", new List<ModTask>()
            {
                new ModTask(250, "Civilians.Update", Civilians.Update, 0),//500//250
            }));
            TaskGroups.Add(new ModTaskGroup("Group7", new List<ModTask>()
            {
                new ModTask(2000, "World.ActiveNearLocations", World.Places.ActivateLocations, 0),//1000
                new ModTask(4000, "Weather.Update", Weather.Update, 1),//1000

                new ModTask(500, "World.UpdateNear", World.Places.UpdateLocations, 2),//1000
                new ModTask(2000, "Player.GangRelationshipsUpdate", Player.GangRelationships.Update, 3),//might become a priority...
                new ModTask(5000, "Player.Properties.Update", Player.Properties.Update, 4),//might become a priority...

                new ModTask(1000, "World.Update", World.Update, 5),

            }));
            TaskGroups.Add(new ModTaskGroup("Group8", new List<ModTask>()
            {
                new ModTask(500, "Tasker.UpdatePolice", Tasker.UpdatePolice, 0),
                new ModTask(500, "Tasker.UpdateCivilians", Tasker.UpdateCivilians, 1),
            }));
        }
        private void StartCoreLogic()
        {
            foreach(ModTaskGroup modTaskGroup in TaskGroups)
            {
                //Start a new gamefiber to loop our tasks
                GameFiber.StartNew(delegate
                {
                    try
                    {
                        while (IsRunning)
                        {
                            modTaskGroup.Update();
                            GameFiber.Yield();
                        }
                    }
                    catch (Exception e)
                    {
                        EntryPoint.WriteToConsole("Error: " + e.Message + " : " + e.StackTrace, 0);
                        DisplayCrashMessage();
                        Dispose();
                    }
                }, $"Run Logic {modTaskGroup.Name}");
            } 
            GameFiber.Yield();
        }
        private void StartInputLogic()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                        Input.Update();
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error: " + e.Message + " : " + e.StackTrace, 0);
                    DisplayCrashMessage();
                    Dispose();
                }
            }, "Run Input Logic");


            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                        VanillaManager.Tick();
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error: " + e.Message + " : " + e.StackTrace, 0);
                    DisplayCrashMessage();
                    Dispose();
                }
            }, "Run Vanilla Manager Logic");
        }
        private void StartUILogic()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {

                        UI.Tick1();
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error: " + e.Message + " : " + e.StackTrace, 0);
                    DisplayCrashMessage();
                    Dispose();
                }
            }, "Run UI Logic");
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                        UI.Tick2();
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error: " + e.Message + " : " + e.StackTrace, 0);
                    DisplayCrashMessage();
                    Dispose();
                }
            }, "Run UI Logic 2");
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                        UI.Tick3();
                        GameFiber.Yield();
                        Time.Tick();//this was below before, but shouldnt be any different
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error: " + e.Message + " : " + e.StackTrace, 0);
                    DisplayCrashMessage();
                    Dispose();
                }
            }, "Run UI Logic 3");
        }
        private void StartDebugLogic()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                        Debug.Update();
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~has crashed and needs to be restarted");
                    EntryPoint.WriteToConsole("Error: " + e.Message + " : " + e.StackTrace, 0);
                    Dispose();
                }
            }, "Run Debug Logic");
        }
        private void DisplayCrashMessage()
        {
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~has crashed and needs to be restarted");
        }
        private void DisplayLoadSuccessfulMessage()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            if (EntryPoint.NotificationID != 0)
            {
                Game.RemoveNotification(EntryPoint.NotificationID);
            }
            Game.DisplayNotification(
                $"~s~Los Santos ~r~RED ~s~v{fvi.FileVersion} " +
                $"~n~By ~g~Greskrendtregk ~s~Has Loaded Successfully." +
                $"~n~" +
                $"~n~Press {ModDataFileManager.Settings.SettingsManager.KeySettings.MenuKey} to open the ~r~Main Menu~s~" +
                $"~n~Select ~r~Main Menu -> About~s~ for mod information and controls." +
                $"~n~" +
                $"~n~Press {NativeHelper.FormatControls(ModDataFileManager.Settings.SettingsManager.KeySettings.ActionPopUpDisplayKeyModifier, ModDataFileManager.Settings.SettingsManager.KeySettings.ActionPopUpDisplayKey)} " +
                    $"or {NativeHelper.FormatControls(ModDataFileManager.Settings.SettingsManager.KeySettings.AltActionPopUpDisplayKeyModifier, ModDataFileManager.Settings.SettingsManager.KeySettings.AltActionPopUpDisplayKey)} " +
                    $"to open the ~r~Action Wheel~s~");

            EntryPoint.WriteToConsole($"Has Loaded Successfully",0);


        }
    }
}