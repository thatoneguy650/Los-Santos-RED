using LosSantosRED.lsr.Helper;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
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
        public List<ModTaskGroup> TaskGroups { get; private set; } = new List<ModTaskGroup>();
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
        private NAudioPlayer NAudioPlayer;
        private NAudioPlayer NAudioPlayer2;
        private Weather Weather;
        private Mod.World World;
        
        private ModDataFileManager ModDataFileManager;
        private WeatherManager WeatherManager;

        public ModController()
        {
            ModDataFileManager = new ModDataFileManager();
        }
        public bool IsRunning { get; private set; }
        //test

        public bool RunUI { get; set; } = true;
        public bool RunInput { get; set; } = true;
        public bool RunOther { get; set; } = true;
        public bool RunVanilla { get; set; } = true;
        public bool RunMenuOnly { get; set; } = true;
        public bool IsDisplayingAlertScreen { get; set; } = false;
        public void Setup()
        {
            IsRunning = true;
            while (Game.IsLoading)
            {
                GameFiber.Yield();
            }
            ModDataFileManager = new ModDataFileManager();
            ModDataFileManager.Setup();


            //WavAudioPlayer = new WavAudioPlayer();
            NAudioPlayer = new NAudioPlayer(ModDataFileManager.Settings);
            NAudioPlayer2 = new NAudioPlayer(ModDataFileManager.Settings);
            //MediaPlayerAudio = new MediaPlayerAudio(ModDataFileManager.Settings);
            GameFiber.Yield();
            Time = new Mod.Time(ModDataFileManager.Settings);
            Time.Setup();
            GameFiber.Yield();
            World = new Mod.World(ModDataFileManager.Agencies, ModDataFileManager.Zones, ModDataFileManager.Jurisdictions, ModDataFileManager.Settings, ModDataFileManager.PlacesOfInterest, ModDataFileManager.PlateTypes, ModDataFileManager.Names, ModDataFileManager.RelationshipGroups, ModDataFileManager.Weapons, 
                ModDataFileManager.Crimes, Time, ModDataFileManager.ShopMenus, ModDataFileManager.Interiors, NAudioPlayer, ModDataFileManager.Gangs, ModDataFileManager.GangTerritories, ModDataFileManager.Streets);
            World.Setup();
            GameFiber.Yield();
            Player = new Mod.Player(Game.LocalPlayer.Character.Model.Name, Game.LocalPlayer.Character.IsMale, ModDataFileManager.Names.GetRandomName(Game.LocalPlayer.Character.Model.Name, Game.LocalPlayer.Character.IsMale), World, Time, ModDataFileManager.Streets, 
                ModDataFileManager.Zones, ModDataFileManager.Settings, ModDataFileManager.Weapons, ModDataFileManager.RadioStations, ModDataFileManager.Scenarios, ModDataFileManager.Crimes, NAudioPlayer, NAudioPlayer2, ModDataFileManager.PlacesOfInterest, ModDataFileManager.Interiors, 
                ModDataFileManager.ModItems, ModDataFileManager.Intoxicants, ModDataFileManager.Gangs, ModDataFileManager.Jurisdictions, ModDataFileManager.GangTerritories, ModDataFileManager.GameSaves, ModDataFileManager.Names, ModDataFileManager.ShopMenus, 
                ModDataFileManager.RelationshipGroups, ModDataFileManager.DanceList, ModDataFileManager.SpeechList, ModDataFileManager.Seats, ModDataFileManager.Agencies);
            Player.Setup();
            GameFiber.Yield();
            Police = new Police(World, Player, Player, ModDataFileManager.Settings, Player);
            GameFiber.Yield();
            Civilians = new Civilians(World, Player, Player, ModDataFileManager.Settings, ModDataFileManager.Gangs);
            GameFiber.Yield();
            PedSwap = new PedSwap(Time, Player, ModDataFileManager.Settings, World, ModDataFileManager.Weapons, ModDataFileManager.Crimes, ModDataFileManager.Names, ModDataFileManager.ModItems, World, ModDataFileManager.RelationshipGroups, 
                ModDataFileManager.ShopMenus, ModDataFileManager.DispatchablePeople, ModDataFileManager.Heads);
            GameFiber.Yield();
            Tasker = new Tasker(World, Player, ModDataFileManager.Weapons, ModDataFileManager.Settings, ModDataFileManager.PlacesOfInterest);
            Tasker.Setup();
            GameFiber.Yield();
            Dispatcher = new Dispatcher(World, Player, ModDataFileManager.Agencies, ModDataFileManager.Settings, ModDataFileManager.Streets, ModDataFileManager.Zones, ModDataFileManager.Jurisdictions, ModDataFileManager.Weapons, ModDataFileManager.Names, ModDataFileManager.Crimes, 
                ModDataFileManager.RelationshipGroups, ModDataFileManager.Gangs, ModDataFileManager.GangTerritories, ModDataFileManager.ShopMenus, ModDataFileManager.PlacesOfInterest);
            Dispatcher.Setup();
            GameFiber.Yield();
            UI = new UI(Player, ModDataFileManager.Settings, ModDataFileManager.Jurisdictions, PedSwap, ModDataFileManager.PlacesOfInterest, Player, Player, Player, ModDataFileManager.Weapons, ModDataFileManager.RadioStations, ModDataFileManager.GameSaves, World, Player, Player, Tasker, Player, 
                ModDataFileManager.ModItems, Time, Player, ModDataFileManager.Gangs, ModDataFileManager.GangTerritories, ModDataFileManager.Zones, ModDataFileManager.Streets, ModDataFileManager.Interiors, Dispatcher, ModDataFileManager.Agencies, Player, ModDataFileManager.DanceList, ModDataFileManager.GestureList, 
                ModDataFileManager.ShopMenus, Player, ModDataFileManager.Crimes, ModDataFileManager.Counties, ModDataFileManager.Intoxicants);
            UI.Setup();
            GameFiber.Yield();
            Input = new Input(Player, ModDataFileManager.Settings, UI, PedSwap);
            GameFiber.Yield();
            VanillaManager = new VanillaManager(ModDataFileManager.Settings, ModDataFileManager.PlacesOfInterest);
            VanillaManager.Setup();
            GameFiber.Yield();
            Weather = new Weather(NAudioPlayer, ModDataFileManager.Settings, Time, Player);
            Weather.Setup();

            GameFiber.Yield();
            WeatherManager = new WeatherManager(ModDataFileManager.Settings, Time, ModDataFileManager.WeatherForecasts);
            WeatherManager.Setup();


            GameFiber.Yield();
            Debug = new Debug(ModDataFileManager.PlateTypes, World, Player, ModDataFileManager.Streets, Dispatcher, ModDataFileManager.Zones, ModDataFileManager.Crimes, this, ModDataFileManager.Settings, Tasker, Time, ModDataFileManager.Agencies, ModDataFileManager.Weapons, ModDataFileManager.ModItems, Weather, 
                ModDataFileManager.PlacesOfInterest, ModDataFileManager.Interiors, ModDataFileManager.Gangs, Input, ModDataFileManager.ShopMenus, ModDataFileManager);
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

            UI.SetupDebugMenu();

            DisplayLoadSuccessfulMessage();
        }
        public void Dispose()
        {
            IsRunning = false;
            //GameFiber.Sleep(500);
            Player.Dispose();
            World.Dispose();
            PedSwap.Dispose();
            Dispatcher.Dispose();
            VanillaManager.Dispose();
            UI.Dispose();
            Time.Dispose();
            Weather.Dispose();
            Debug.Dispose();
            WeatherManager.Dispose();
            Game.DisplayNotification("~s~Los Santos ~r~RED ~s~Deactivated");
            EntryPoint.WriteToConsole($"Has Been Deactivated",0);
        }
        public void CrashUnload()
        {
            DisplayCrashMessage();
            Dispose();
        }
        private void StartModLogic()
        {
            TaskGroups = new List<ModTaskGroup>
            {
                new ModTaskGroup("RG1:Player", new List<ModTask>()
            {
                 new ModTask(100, "Player.Update", Player.Update, 0),//1
                 new ModTask(250, "UI.UpdateData", UI.CacheData, 1),//500
            }),
                new ModTaskGroup("RG2:Player Gen", new List<ModTask>()
            {
                new ModTask(250, "Player.Violations.Update", Player.Violations.Update, 0),
                new ModTask(250, "Player.CurrentPoliceResponse.Update", Player.PoliceResponse.Update, 1),
                new ModTask(250, "Player.Investigation.Update", Player.Investigation.Update, 2),
                new ModTask(250, "Player.SearchModeUpdate", Player.SearchMode.Update, 3),

                new ModTask(250, "Player.TrafficViolationsUpdate", Player.Violations.TrafficViolations.Update, 4),
                new ModTask(250, "Player.LocationUpdate", Player.LocationUpdate, 5),
                new ModTask(250, "Player.ArrestWarrantUpdate",Player.CriminalHistory.Update, 6),//these were all 500
            }),
                new ModTaskGroup("RG3:World Gen", new List<ModTask>()
            {
                new ModTask(1000, "World.PrunePedestrians", World.Pedestrians.Prune, 0),
                new ModTask(500, "World.CreateNewPedestrians", World.Pedestrians.CreateNew, 1), //very bad performance//500, need to up this somehow, we are stuck around 1250 to 1500, maybe just up the times?
                new ModTask(1000, "World.PruneVehicles", World.Vehicles.Prune, 2),//500
                new ModTask(500, "World.CreateNewVehicles", World.Vehicles.CreateNew, 3),//1000 //very bad performance
                new ModTask(1000, "World.CleanUpVehicles", World.Vehicles.CleanUp, 4),
                new ModTask(1000, "World.UpdateVehiclePlates", World.Vehicles.PlateController.UpdatePlates, 5),
                new ModTask(1500, "Player.ScannerUpdate", Player.Scanner.Update, 6),
            }),
                new ModTaskGroup("RG4:Dispatch", new List<ModTask>()
            {
                new ModTask(1500, "Dispatcher.Recall", Dispatcher.Recall, 0),
                new ModTask(1500, "Dispatcher.Dispatch", Dispatcher.Dispatch, 1),
            }),
                new ModTaskGroup("RG5:Police Update", new List<ModTask>()
            {
                new ModTask(250, "Police.Update", Police.Update,0),//250
            }),
                new ModTaskGroup("RG6:Civilian Update", new List<ModTask>()
            {
                new ModTask(250, "Civilians.Update", Civilians.UpdateCivilians, 0),//500//250
            }),

                new ModTaskGroup("RG:7 GangMember Update", new List<ModTask>()
            {
                new ModTask(250, "Civilians.UpdateGangMembers", Civilians.UpdateGangMembers, 0),//500//250
            }),
                new ModTaskGroup("RG8:Merchant Update", new List<ModTask>()
            {
                new ModTask(250, "Civilians.UpdateMerchants", Civilians.UpdateMerchants, 0),//500//250
            }),
                new ModTaskGroup("RG9:EMT Update", new List<ModTask>()
            {
                new ModTask(250, "Civilians.UpdateEMTs", Civilians.UpdateEMTs, 0),//500//250
                new ModTask(200, "Civilians.UpdateTotalWanted", Civilians.UpdateTotalWanted, 1),//500//250
            }),

                new ModTaskGroup("RG10:World LowPri", new List<ModTask>()
            {
                new ModTask(2000, "World.ActiveNearLocations", World.Places.ActivateLocations, 0),//1000
                new ModTask(4000, "Weather.Update", Weather.Update, 1),//1000
                new ModTask(2000,"WeatherManager.Update",WeatherManager.Update,2),
                new ModTask(500, "World.UpdateNear", World.Places.UpdateLocations, 3),//1000
                new ModTask(2000, "Player.GangRelationshipsUpdate", Player.RelationshipManager.GangRelationships.Update, 4),//might become a priority...
                new ModTask(5000, "Player.Properties.Update", Player.Properties.Update, 5),//might become a priority...
                new ModTask(1000, "World.Update", World.Update, 6),

            }),
                new ModTaskGroup("RG11:TaskerUpdate", new List<ModTask>()
            {
                new ModTask(500, "Tasker.UpdatePolice", Tasker.UpdatePolice, 0),
                new ModTask(500, "Tasker.UpdateCivilians", Tasker.UpdateCivilians, 1),
            })
            };
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
                        if(RunInput)
                        {
                            Input.Tick();
                        }
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
                        if (RunVanilla)
                        {
                            VanillaManager.Tick();
                        }
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
                        if (RunUI)
                        {
                            UI.Tick1();
                        }
                        else if(RunMenuOnly)
                        {
                            UI.MenuOnly();
                        }
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
                        if (RunUI)
                        {
                            UI.Tick2();
                        }
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
                        if (RunUI)
                        {
                            UI.Tick3();
                            GameFiber.Yield();
                            Time.Tick();//this was below before, but shouldnt be any different
                        }
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
                        if (RunOther)
                        {
                            Debug.Update();
                        }
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
                $"~n~By ~g~Greskrendtregk ~s~Has Loaded Successfully.");

            string controlString =
                $"Main Menu: ~{ModDataFileManager.Settings.SettingsManager.KeySettings.MenuKey.GetInstructionalId()}~" +
                $"~n~Action Wheel: {FormatKeys(ModDataFileManager.Settings.SettingsManager.KeySettings.ActionPopUpDisplayKeyModifier, ModDataFileManager.Settings.SettingsManager.KeySettings.ActionPopUpDisplayKey)}" +
                $"~n~Action Wheel (Alt): {FormatKeys(ModDataFileManager.Settings.SettingsManager.KeySettings.AltActionPopUpDisplayKeyModifier, ModDataFileManager.Settings.SettingsManager.KeySettings.AltActionPopUpDisplayKey)}";
            if(NativeHelper.IsUsingController || 1==1)
            {
                controlString += $"~n~Action Wheel (Controller): ~{InstructionalButton.GetButtonId((GameControl)ModDataFileManager.Settings.SettingsManager.KeySettings.GameControlActionPopUpDisplayKey)}~";
            }
            Game.DisplayHelp(controlString);
            EntryPoint.WriteToConsole($"Has Loaded Successfully",0);
        }
        private string FormatKeys(Keys modifier, Keys key)
        {
            if(modifier != Keys.None)
            {
                return $"~{modifier.GetInstructionalId()}~ + ~{key.GetInstructionalId()}~";
            }
            else
            {
                return $"~{key.GetInstructionalId()}~";
            }
        }
    }
}