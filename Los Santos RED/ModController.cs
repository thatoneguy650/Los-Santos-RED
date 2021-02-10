using LosSantosRED.lsr.Helper;
using Rage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LosSantosRED.lsr
{
    public class ModController
    {
        private readonly Stopwatch TickStopWatch = new Stopwatch();
        private Agencies Agencies;
        //private Audio Audio;
        private AudioNew Audio;
        private Civilians Civilians;
        private CountyJurisdictions CountyJurisdictions;
        private Debug Debug;
        private Dispatcher Dispatcher;
        private Input Input;
        private string LastRanTask;
        private string LastRanTaskLocation;
        //private Menu_Old Menu_Old;
       // private MenuManager Menu;
        private List<ModTask> MyTickTasks;
        private Names Names;
        private PedSwap PedSwap;
        private PlacesOfInterest PlacesOfInterest;
        private PlateTypes PlateTypes;
        private Mod.Player Player;
        private Police Police;
        private Respawning Respawning;
        private Scanner Scanner;
        private Settings Settings;
        private Streets Streets;
        private StreetScannerAudio StreetScannerAudio;
        private Tasker Tasker;
        //private Tasking_Old Tasking_Old;
        private Mod.Time Time;
        private UI UI;
        private VehicleScannerAudio VehicleScannerAudio;
        private Weapons Weapons;
        private Mod.World World;
        private ZoneJurisdictions ZoneJurisdictions;
        private Zones Zones;
        private ZoneScannerAudio ZoneScannerAudio;
        private RadioStations RadioStations;
        private PedGroups RelationshipGroups;
        private VanillaManager VanillaManager;
        public ModController()
        {
        }
        public bool IsRunning { get; private set; }
        public void Dispose()
        {
            IsRunning = false;
            GameFiber.Sleep(500);
            Player.Dispose();
            World.Dispose();
            PedSwap.Dispose();
            Dispatcher.Dispose();
            VanillaManager.Dispose();
            if (Settings.SettingsManager.General.PedTakeoverSetRandomMoney && PedSwap.OriginalMoney > 0)
            {
                Player.SetMoney(PedSwap.OriginalMoney);
            }
        }
        public void NewPlayer(string modelName, bool isMale)
        {
            Player.Reset(true, true, true, true);
            Player.SetDemographics(modelName, isMale, GetName(modelName, Names.GetRandomName(isMale)), RandomItems.MyRand.Next(Settings.SettingsManager.General.PedTakeoverRandomMoneyMin, Settings.SettingsManager.General.PedTakeoverRandomMoneyMax));
            Scanner.Reset();
        }
        public void Start()
        {
            IsRunning = true;
            while (Game.IsLoading)
            {
                GameFiber.Yield();
            }
            ReadDataFiles();
            //GameFiber.Sleep(500);
            Audio = new AudioNew();
            Time = new Mod.Time();
            World = new Mod.World(Agencies, Zones, ZoneJurisdictions, Settings, PlacesOfInterest, PlateTypes, Names, RelationshipGroups);
            World.Setup();
            
            Player = new Mod.Player(Game.LocalPlayer.Character.Model.Name, Game.LocalPlayer.Character.IsMale, GetName(Game.LocalPlayer.Character.Model.Name, Names.GetRandomName(Game.LocalPlayer.Character.IsMale)), 0, World, Time, Streets, Zones, Settings, Weapons, RadioStations);
            Player.Setup();

            Input = new Input(Player, Settings);
            Police = new Police(World, Player);
            Civilians = new Civilians(World, Player);
            Respawning = new Respawning(Time, World, Player, Weapons, PlacesOfInterest, Settings);
            PedSwap = new PedSwap(Time, Player, Settings, World);
            Tasker = new Tasker(World, Player, Weapons);
            UI = new UI(Player, Settings, ZoneJurisdictions, PedSwap, PlacesOfInterest, Respawning, Player, Weapons, RadioStations);
            Dispatcher = new Dispatcher(World, Player, Agencies, Settings, Streets, Zones, CountyJurisdictions, ZoneJurisdictions);
            Scanner = new Scanner(World, Player, Audio, Respawning, Settings);
            VanillaManager = new VanillaManager();
            Debug = new Debug(PlateTypes, World, Player, Scanner);
            World.AddBlipsToMap();
            PedSwap.StoreInitialVariation();
            GameFiber.Yield();

            SetupModTasks();
            GameFiber.Yield();
            StartGameLogic();
            GameFiber.Yield();
            StartMenuLogic();
            GameFiber.Yield();
            StartDebugLogic();
            GameFiber.Yield();
            Game.DisplayNotification("~s~Los Santos ~r~RED ~s~v0.1 ~n~By ~g~Greskrendtregk ~n~~s~Has Loaded Successfully");
        }
        private void ReadDataFiles()
        {
            Settings = new Settings();
            Settings.ReadConfig();
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
            PlacesOfInterest = new PlacesOfInterest();
            PlacesOfInterest.ReadConfig();
            GameFiber.Yield();
            VehicleScannerAudio = new VehicleScannerAudio();
            VehicleScannerAudio.ReadConfig();
            GameFiber.Yield();
            ZoneScannerAudio = new ZoneScannerAudio();
            ZoneScannerAudio.ReadConfig();
            GameFiber.Yield();
            StreetScannerAudio = new StreetScannerAudio();
            StreetScannerAudio.ReadConfig();
            GameFiber.Yield();
            Agencies = new Agencies();
            Agencies.ReadConfig();
            GameFiber.Yield();
            CountyJurisdictions = new CountyJurisdictions(Agencies);
            CountyJurisdictions.ReadConfig();
            GameFiber.Yield();
            ZoneJurisdictions = new ZoneJurisdictions(Agencies);
            ZoneJurisdictions.ReadConfig();
            GameFiber.Yield();
            RadioStations = new RadioStations();
            RadioStations.ReadConfig();
            GameFiber.Yield();
            RelationshipGroups = new PedGroups();
            RelationshipGroups.ReadConfig();
            GameFiber.Yield();
        }
        private void SetupModTasks()
        {
            MyTickTasks = new List<ModTask>()
            {

                //Required Run
                new ModTask(0, "Time.Tick", Time.Tick, 0,0),
                new ModTask(0, "Input.Tick", Input.Update, 1,0),
                new ModTask(0, "VanillaManager.Tick", VanillaManager.Tick, 2,0),//new 
                new ModTask(25, "Player.Update", Player.Update, 3,0),
                new ModTask(100, "World.Police.Tick", Police.Update, 4,0),//25
                new ModTask(200, "Player.Violations.Update", Player.ViolationsUpdate, 5,0),//50
                new ModTask(200, "Player.CurrentPoliceResponse.Update", Player.PoliceResponse.Update, 5,1),//50



                //Can TimeOut
                new ModTask(150, "Player.Investigations.Tick", Player.Investigation.Update, 6,0),




                new ModTask(250, "World.Civilians.Tick", Civilians.Update, 7,0),//150
                new ModTask(250, "World.Pedestrians.Prune", World.PrunePedestrians, 7,1),
                new ModTask(1000, "World.Pedestrians.Scan", World.ScaneForPedestrians, 7,2),
                new ModTask(250, "World.Vehicles.CleanLists", World.PruneVehicles, 7,3),
                new ModTask(1000, "World.Vehicles.Scan", World.ScanForVehicles, 7,4),



                new ModTask(500, "Player.Violations.TrafficUpdate", Player.TrafficViolationsUpdate, 8,0),
                new ModTask(500, "Player.CurrentLocation.Update", Player.LocationUpdate, 8,1),
                new ModTask(500, "Player.ArrestWarrant.Update",Player.ArrestWarrantUpdate, 8,2),
                new ModTask(500, "World.Vehicles.Tick", World.VehiclesTick, 9,1),
                new ModTask(150, "Player.SearchMode.UpdateWanted", Player.SearchModeUpdate, 11,0),
                new ModTask(150, "Player.SearchMode.StopVanillaSearchMode", Player.StopVanillaSearchMode, 11,1),
                new ModTask(500, "World.Scanner.Tick", Scanner.Tick, 12,0),
                new ModTask(1000, "World.Vehicles.UpdatePlates", World.UpdateVehiclePlates, 13,0),

                new ModTask(500, "World.Dispatch.DeleteChecking", Dispatcher.Recall, 15,0),
                new ModTask(500, "World.Dispatch.SpawnChecking", Dispatcher.Dispatch, 15,1),

               //// Old Tasking
               // new ModTask(500, "Tasking_Old.AddTaskablePeds", Tasking_Old.AddTaskablePeds, 14,0),//cops turned off in this
               // new ModTask(500, "World.Tasking.Tick", Tasking_Old.TaskCops, 14,1),
               // new ModTask(750, "Tasking_Old.TaskCivilians", Tasking_Old.TaskCivilians, 14,2),

                //New Tasking


                new ModTask(500, "NewTasking.Update", Tasker.RunTasks, 16,0),
                new ModTask(500, "NewTasking.Update", Tasker.UpdatePolice, 16,1),
                new ModTask(500, "NewTasking.Update", Tasker.UpdateCivilians, 16,2),
            };
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
                    Game.Console.Print("Error" + e.Message + " : " + e.StackTrace);
                    Dispose();
                }
            }, "Run Debug Logic");
        }
        private void StartGameLogic()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                        TickStopWatch.Start();

                        foreach (int RunGroup in MyTickTasks.GroupBy(x => x.RunGroup).Select(x => x.First()).ToList().Select(x => x.RunGroup))
                        {
                            if (RunGroup >= 3 && TickStopWatch.ElapsedMilliseconds >= 5)//16//Abort processing, we are running over time? might not work with any yields?, still do the most important ones
                            {
                                Game.Console.Print($"GameLogic Tick took > 5 ms ({TickStopWatch.ElapsedMilliseconds} ms), aborting, Last Ran {LastRanTask} in {LastRanTaskLocation}");
                                break;
                            }

                            ModTask ToRun = MyTickTasks.Where(x => x.RunGroup == RunGroup && x.ShouldRun).OrderBy(x => x.MissedInterval ? 0 : 1).OrderBy(x => x.GameTimeLastRan).OrderBy(x => x.RunOrder).FirstOrDefault();//should also check if something has barely ran or
                            if (ToRun != null)
                            {
                                ToRun.Run();
                                LastRanTask = ToRun.DebugName;
                                LastRanTaskLocation = "Regular";
                            }
                            foreach (ModTask RunningBehind in MyTickTasks.Where(x => x.RunGroup == RunGroup && x.RunningBehind))
                            {
                                RunningBehind.Run();
                                LastRanTask = ToRun.DebugName;
                                LastRanTaskLocation = "RunningBehind";
                            }
                        }
                        MyTickTasks.ForEach(x => x.RanThisTick = false);
                        TickStopWatch.Reset();
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    Game.Console.Print("Error" + e.Message + " : " + e.StackTrace);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~has crashed and needs to be restarted");
                    Dispose();
                }
            }, "Run Game Logic");
            GameFiber.Yield();
        }
        private void StartMenuLogic()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                       // Menu.Update(Player.IsDead,Player.IsBusted);
                        //Menu_Old.Update();
                        UI.Update();
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    Game.Console.Print("Error" + e.Message + " : " + e.StackTrace);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~has crashed and needs to be restarted");
                    Dispose();
                }
            }, "Run Menu/UI Logic");
        }
        private string GetName(string modelBeforeSpoof, string defaultName)
        {
            if (modelBeforeSpoof.ToLower() == "player_zero")
            {
                return "Michael De Santa";
            }
            else if (modelBeforeSpoof.ToLower() == "player_one")
            {
                return "Franklin Clinton";
            }
            else if (modelBeforeSpoof.ToLower() == "player_two")
            {
                return "Trevor Philips";
            }
            else
            {
                return defaultName;
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
            public bool MissedInterval
            {
                get
                {
                    if (Interval == 0)
                    {
                        return false;
                    }
                    else if (Game.GameTime - GameTimeLastRan >= IntervalMissLength)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            public bool RunningBehind
            {
                get
                {
                    if (Interval == 0)
                    {
                        return false;
                    }
                    else if (Game.GameTime - GameTimeLastRan >= (IntervalMissLength * 2))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            public bool ShouldRun
            {
                get
                {
                    if (GameTimeLastRan == 0)
                    {
                        return true;
                    }
                    else if (Game.GameTime - GameTimeLastRan > Interval)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            public void Run()
            {
                TickToRun();
                GameTimeLastRan = Game.GameTime;
                RanThisTick = true;
            }
        }
    }
}