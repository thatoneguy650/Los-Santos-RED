using Rage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LosSantosRED.lsr
{
    public class ModController
    {
        private List<ModTaskGroup> TaskGroups;
        private Agencies Agencies;
        private Civilians Civilians;
        private Crimes Crimes;
        private Debug Debug;
        private Dispatcher Dispatcher;
        private GameSaves GameSaves;
        private Gangs Gangs;
        private GangTerritories GangTerritories;
        private Input Input;
        private Interiors Interiors;
        private Intoxicants Intoxicants;
        private Jurisdictions Jurisdictions;
        private ModItems ModItems;
        private Names Names;
        private PedSwap PedSwap;
        private PlacesOfInterest PlacesOfInterest;
        private PlateTypes PlateTypes;
        private Mod.Player Player;
        private Police Police;
        private RadioStations RadioStations;
        private PedGroups RelationshipGroups;
        private Scenarios Scenarios;
        private Settings Settings;
        private ShopMenus ShopMenus;
        private Streets Streets;
        private Tasker Tasker;
        private Mod.Time Time;
        private UI UI;
        private VanillaManager VanillaManager;
        private WavAudioPlayer WavAudioPlayer;
       // private NAudioPlayer NAudioPlayer;
        private Weapons Weapons;
        private Weather Weather;
        private Mod.World World;
        private Zones Zones;
        private Heads Heads;
        private DispatchableVehicles DispatchableVehicles;
        private DispatchablePeople DispatchablePeople;
        private IssueableWeapons IssueableWeapons;
        private Dances DanceList;

        public ModController()
        {
        }
        public bool IsRunning { get; private set; }
        public void Setup()
        {
            IsRunning = true;
            while (Game.IsLoading)
            {
                GameFiber.Yield();
            }
            LoadDataFiles();
            WavAudioPlayer = new WavAudioPlayer();
            //NAudioPlayer = new NAudioPlayer();
            GameFiber.Yield();
            Time = new Mod.Time(Settings);
            Time.Setup();
            GameFiber.Yield();
            World = new Mod.World(Agencies, Zones, Jurisdictions, Settings, PlacesOfInterest, PlateTypes, Names, RelationshipGroups, Weapons, Crimes, Time, ShopMenus, Interiors, WavAudioPlayer, Gangs, GangTerritories, Streets);
            World.Setup();
            GameFiber.Yield();
            Player = new Mod.Player(Game.LocalPlayer.Character.Model.Name, Game.LocalPlayer.Character.IsMale, Names.GetRandomName(Game.LocalPlayer.Character.Model.Name, Game.LocalPlayer.Character.IsMale), World, Time, Streets, Zones, Settings, Weapons, RadioStations, Scenarios, Crimes, WavAudioPlayer, PlacesOfInterest, Interiors, ModItems, Intoxicants, Gangs, Jurisdictions, GangTerritories, GameSaves, Names, ShopMenus, RelationshipGroups, DanceList);
            Player.Setup();
            GameFiber.Yield();
            Police = new Police(World, Player, Player, Settings);
            GameFiber.Yield();
            Civilians = new Civilians(World, Player, Player, Settings);
            GameFiber.Yield();
            PedSwap = new PedSwap(Time, Player, Settings, World, Weapons, Crimes, Names, ModItems);
            GameFiber.Yield();
            Tasker = new Tasker(World, Player, Weapons, Settings, PlacesOfInterest);
            Tasker.Setup();
            GameFiber.Yield();
            Dispatcher = new Dispatcher(World, Player, Agencies, Settings, Streets, Zones, Jurisdictions, Weapons, Names, Crimes, RelationshipGroups, Gangs, GangTerritories, ShopMenus, PlacesOfInterest);
            Dispatcher.Setup();
            GameFiber.Yield();
            UI = new UI(Player, Settings, Jurisdictions, PedSwap, PlacesOfInterest, Player, Player, Player, Weapons, RadioStations, GameSaves, World, Player, Player, Tasker, Player, ModItems, Time, Player, Gangs, GangTerritories, Zones, Streets, Interiors, Dispatcher, Agencies, Player, DanceList);
            UI.Setup();
            GameFiber.Yield();
            Input = new Input(Player, Settings, UI, PedSwap);
            GameFiber.Yield();
            VanillaManager = new VanillaManager(Settings);
            GameFiber.Yield();
            Weather = new Weather(WavAudioPlayer, Settings, Time, Player);
            Weather.Setup();
            GameFiber.Yield();
            Debug = new Debug(PlateTypes, World, Player, Streets, Dispatcher, Zones, Crimes, this, Settings, Tasker, Time, Agencies, Weapons, ModItems, Weather, PlacesOfInterest, Interiors, Gangs, Input, ShopMenus);
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
        }
        private void LoadDataFiles()
        {
            Settings = new Settings();
            Settings.ReadConfig();
            GameFiber.Yield();
            ModItems = new ModItems();
            ModItems.ReadConfig();
            GameFiber.Yield();
            ShopMenus = new ShopMenus();
            ShopMenus.ReadConfig();
            GameFiber.Yield();
            Zones = new Zones();
            Zones.ReadConfig();
            GameFiber.Yield();
            PlateTypes = new PlateTypes();
            PlateTypes.ReadConfig();
            GameFiber.Yield();
            Streets = new Streets();
            Streets.ReadConfig();
            GameFiber.Yield();
            Weapons = new Weapons();
            Weapons.ReadConfig();
            GameFiber.Yield();
            Names = new Names();
            Names.ReadConfig();
            GameFiber.Yield();
            Heads = new Heads();
            Heads.ReadConfig();
            GameFiber.Yield();
            DispatchableVehicles = new DispatchableVehicles();
            DispatchableVehicles.ReadConfig();
            GameFiber.Yield();
            DispatchablePeople = new DispatchablePeople();
            DispatchablePeople.ReadConfig();
            GameFiber.Yield();
            IssueableWeapons = new IssueableWeapons();
            IssueableWeapons.ReadConfig();
            GameFiber.Yield();
            Agencies = new Agencies();
            Agencies.ReadConfig();
            Agencies.Setup(Heads, DispatchableVehicles, DispatchablePeople, IssueableWeapons);
            GameFiber.Yield();
            Gangs = new Gangs();
            Gangs.ReadConfig();
            Gangs.Setup(Heads, DispatchableVehicles, DispatchablePeople, IssueableWeapons);
            GameFiber.Yield();
            PlacesOfInterest = new PlacesOfInterest(ShopMenus, Gangs);
            PlacesOfInterest.ReadConfig();
            GameFiber.Yield();
            Jurisdictions = new Jurisdictions(Agencies);
            Jurisdictions.ReadConfig();
            GameFiber.Yield();
            GangTerritories = new GangTerritories(Gangs);
            GangTerritories.ReadConfig();
            GameFiber.Yield();
            RadioStations = new RadioStations();
            RadioStations.ReadConfig();
            GameFiber.Yield();
            RelationshipGroups = new PedGroups();
            RelationshipGroups.ReadConfig();
            GameFiber.Yield();
            Scenarios = new Scenarios();
            GameFiber.Yield();
            Crimes = new Crimes();
            Crimes.ReadConfig();
            GameFiber.Yield();
            GameSaves = new GameSaves();
            GameSaves.ReadConfig();
            GameFiber.Yield();
            Interiors = new Interiors();
            Interiors.ReadConfig();
            Intoxicants = new Intoxicants();
            Intoxicants.ReadConfig();
            GameFiber.Yield();
            DanceList = new Dances();
        }
        private void StartModLogic()
        {
            TaskGroups = new List<ModTaskGroup>();
            TaskGroups.Add(new ModTaskGroup("Group1", new List<ModTask>()
            {
                 new ModTask(100, "Player.Update", Player.Update, 0),//1
                 new ModTask(250, "UI.Update", UI.Update, 1),//500
            }));
            TaskGroups.Add(new ModTaskGroup("Group2", new List<ModTask>()
            {
                new ModTask(250, "Player.Violations.Update", Player.ViolationsUpdate, 0),
                new ModTask(250, "Player.CurrentPoliceResponse.Update", Player.PoliceResponse.Update, 1),
                new ModTask(250, "Player.Investigation.Update", Player.Investigation.Update, 2),
                new ModTask(250, "Player.SearchModeUpdate", Player.SearchModeUpdate, 3),

                new ModTask(250, "Player.TrafficViolationsUpdate", Player.TrafficViolationsUpdate, 4),
                new ModTask(250, "Player.LocationUpdate", Player.LocationUpdate, 5),
                new ModTask(250, "Player.ArrestWarrantUpdate",Player.ArrestWarrantUpdate, 6),//these were all 500
            }));
            TaskGroups.Add(new ModTaskGroup("Group3", new List<ModTask>()
            {
                new ModTask(1000, "World.PrunePedestrians", World.Pedestrians.Prune, 0),
                new ModTask(500, "World.CreateNewPedestrians", World.Pedestrians.CreateNew, 1), //very bad performance//500, need to up this somehow, we are stuck around 1250 to 1500, maybe just up the times?
                new ModTask(1000, "World.PruneVehicles", World.Vehicles.Prune, 2),//500
                new ModTask(1000, "World.CreateNewVehicles", World.Vehicles.CreateNew, 3), //very bad performance
                new ModTask(1000, "World.CleanUpVehicles", World.Vehicles.CleanUp, 4),
                new ModTask(1000, "World.UpdateVehiclePlates", World.Vehicles.UpdatePlates, 5),
                new ModTask(1500, "Player.ScannerUpdate", Player.ScannerUpdate, 6),//500
                //new ModTask(500, "VanillaManager.Tick", VanillaManager.Tick, 7),//2000
            }));
            TaskGroups.Add(new ModTaskGroup("Group4", new List<ModTask>()
            {
                new ModTask(1500, "Dispatcher.Recall", Dispatcher.Recall, 0),
                new ModTask(1500, "Dispatcher.Dispatch", Dispatcher.Dispatch, 1),
                new ModTask(500, "Tasker.UpdatePolice", Tasker.UpdatePolice, 2),
                new ModTask(500, "Tasker.UpdateCivilians", Tasker.UpdateCivilians, 3),
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
                new ModTask(2000, "World.ActiveNearLocations", World.Places.ActiveNearLocations, 0),//1000
                new ModTask(4000, "Weather.Update", Weather.Update, 1),//1000
                new ModTask(500, "World.UpdateNear", World.Places.UpdateNearLocations, 2),//1000
                new ModTask(2000, "Player.GangRelationshipsUpdate", Player.GangRelationships.Update, 3),//might become a priority...
                new ModTask(5000, "Player.Properties.Update", Player.Properties.Update, 4),//might become a priority...
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
                        EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
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
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
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
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
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

                        UI.PrimaryTick();
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
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
                        UI.SecondaryTick();
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
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
                        UI.TertiaryTick();
                        GameFiber.Yield();
                        Time.Tick();//this was below before, but shouldnt be any different
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
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
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
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
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            if (EntryPoint.NotificationID != 0)
            {
                Game.RemoveNotification(EntryPoint.NotificationID);
            }
            Game.DisplayNotification($"~s~Los Santos ~r~RED ~s~v{fvi.FileVersion} ~n~By ~g~Greskrendtregk ~n~~s~Has Loaded Successfully.~n~~n~Press {Settings.SettingsManager.KeySettings.MenuKey} to open the ~r~Main Menu~s~~n~~n~Select ~r~About~s~ for mod information.");
        }
        private class ModTaskGroup
        {
            private int CurrentTaskOrderID;
            public ModTaskGroup(string name, List<ModTask> tasksToRun)
            {
                Name = name;
                TasksToRun = tasksToRun;
            }
            public string Name { get; set; }
            public bool IsRunning { get; set; } = true;
            public List<ModTask> TasksToRun { get; set; }
            public void Update()
            {
                if (IsRunning)
                {
                    if (CurrentTaskOrderID > TasksToRun.Count)
                    {
                        CurrentTaskOrderID = 0;
                    }
                    ModTask taskToRun = TasksToRun.Where(x => x.ShouldRun && x.RunOrder == CurrentTaskOrderID).OrderBy(x => x.GameTimeLastRan).FirstOrDefault();
                    if (taskToRun != null)
                    {
                        taskToRun.Run();
                        CurrentTaskOrderID++;
                    }
                    else
                    {
                        ModTask altTaskToRun = TasksToRun.Where(x => x.ShouldRun).OrderBy(x => x.GameTimeLastRan).FirstOrDefault();
                        if (altTaskToRun != null)
                        {
                            altTaskToRun.Run();
                        }
                    }
                }
            }

        }
        private class ModTask
        {
            public string DebugName;
            public uint GameTimeLastRan = 0;
            public uint Interval = 500;
            public uint IntervalMissLength;
            public bool RanThisTick = false;
            public int RunGroup;
            public int RunOrder;
            public Action TickToRun;
            public ModTask(uint _Interval, string _DebugName, Action _TickToRun, int _RunGroup, int _RunOrder)
            {
                GameTimeLastRan = 0;
                Interval = _Interval;
                IntervalMissLength = Interval * 2;
                DebugName = _DebugName;
                TickToRun = _TickToRun;
                RunGroup = _RunGroup;
                RunOrder = _RunOrder;
            }
            public ModTask(uint _Interval, string _DebugName, Action _TickToRun, int _RunOrder)
            {
                GameTimeLastRan = 0;
                Interval = _Interval;
                IntervalMissLength = Interval * 2;
                DebugName = _DebugName;
                TickToRun = _TickToRun;
                RunOrder = _RunOrder;
            }
            public bool MissedInterval => Interval != 0 && Game.GameTime - GameTimeLastRan >= IntervalMissLength;
            public bool RunningBehind => Interval != 0 && Game.GameTime - GameTimeLastRan >= (IntervalMissLength * 2);
            public bool ShouldRun => GameTimeLastRan == 0 || Game.GameTime - GameTimeLastRan > Interval;
            public void Run()
            {
                TickToRun();
                GameTimeLastRan = Game.GameTime;
                RanThisTick = true;
            }
        }
    }
}