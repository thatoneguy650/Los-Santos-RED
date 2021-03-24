using LosSantosRED.lsr.Helper;
using Rage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LosSantosRED.lsr
{
    public class ModController_Old
    {
        private readonly Stopwatch TickStopWatch = new Stopwatch();
        private Agencies Agencies;
        private WavAudio WavAudio;
        private Civilians Civilians;
        private Crimes Crimes;
        private Debug Debug;
        private Dispatcher Dispatcher;
        private Input Input;
        private string PrevLastRanTask;
        private string LastRanTask;
        private string LastRanTaskLocation;
        private int LastRunGroupRan;
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
        private Mod.Time Time;
        private UI UI;
        private VehicleScannerAudio VehicleScannerAudio;
        private Weapons Weapons;
        private Mod.World World;
        private Jurisdictions Jurisdictions;
        private Zones Zones;
        private ZoneScannerAudio ZoneScannerAudio;
        private RadioStations RadioStations;
        private PedGroups RelationshipGroups;
        private VanillaManager VanillaManager;
        private Scenarios Scenarios;
        public ModController_Old()
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
            if (Settings.SettingsManager.General.PedTakeoverSetRandomMoney && PedSwap.CurrentPedMoney > 0)
            {
                Player.SetMoney(PedSwap.CurrentPedMoney);
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
            WavAudio = new WavAudio();
            Time = new Mod.Time();
            World = new Mod.World(Agencies, Zones, Jurisdictions, Settings, PlacesOfInterest, PlateTypes, Names, RelationshipGroups);
            World.Setup();
            
            Player = new Mod.Player(Game.LocalPlayer.Character.Model.Name, Game.LocalPlayer.Character.IsMale, GetName(Game.LocalPlayer.Character.Model.Name, Names.GetRandomName(Game.LocalPlayer.Character.IsMale)), 0, World, Time, Streets, Zones, Settings, Weapons, RadioStations, Scenarios, Crimes);
            Player.Setup();

            Input = new Input(Player, Settings);
            Police = new Police(World, Player);
            Civilians = new Civilians(World, Player);
            Respawning = new Respawning(Time, World, Player, Weapons, PlacesOfInterest, Settings);
            PedSwap = new PedSwap(Time, Player, Settings, World);
            Tasker = new Tasker(World, Player, Weapons);
            UI = new UI(Player, Settings, Jurisdictions, PedSwap, PlacesOfInterest, Respawning, Player, Weapons, RadioStations);
            Dispatcher = new Dispatcher(World, Player, Agencies, Settings, Streets, Zones, Jurisdictions);
            Scanner = new Scanner(World, Player, WavAudio, Respawning, Settings);
            VanillaManager = new VanillaManager();
            Debug = new Debug(PlateTypes, World, Player, Scanner, Streets, Dispatcher);
            World.AddBlipsToMap();//off for now as i do lots of restarts
            PedSwap.Setup();
            GameFiber.Yield();
            //SetupModTasks();
            SetupModTasksAlt();
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
            Jurisdictions = new Jurisdictions(Agencies);
            Jurisdictions.ReadConfig();
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
        }
        private void SetupModTasks()
        {
            MyTickTasks = new List<ModTask>()
            {

                //Required Run
                new ModTask(0, "Time.Tick", Time.Tick, 0,0),
                new ModTask(0, "Input.Tick", Input.Update, 1,0),
                new ModTask(0, "VanillaManager.Tick", VanillaManager.Tick, 2,0),


                new ModTask(25, "Player.Update", Player.Update, 3,0),
                new ModTask(100, "World.Police.Tick", Police.Update, 4,0),//is a big problem, need to fix performance, maybe split up?
                new ModTask(200, "Player.Violations.Update", Player.ViolationsUpdate, 5,0),//
                new ModTask(200, "Player.CurrentPoliceResponse.Update", Player.PoliceResponse.Update, 5,1),//

                //Can TimeOut
                new ModTask(150, "Player.Investigations.Tick", Player.Investigation.Update, 6,0),

                new ModTask(250, "World.Civilians.Tick", Civilians.Update, 7,0),//150
                new ModTask(250, "World.Pedestrians.Prune", World.PrunePedestrians, 7,1),
                new ModTask(1000, "World.Pedestrians.Scan", World.ScanForPedestrians, 7,2),
                new ModTask(1000, "World.Pedestrians.CreateNewPedestrians", World.CreateNewPedestrians, 7,3),
                new ModTask(250, "World.Vehicles.CleanLists", World.PruneVehicles, 7,4),
                new ModTask(1000, "World.Vehicles.Scan", World.ScanForVehicles, 7,5),

                new ModTask(500, "Player.Violations.TrafficUpdate", Player.TrafficViolationsUpdate, 8,0),
                new ModTask(500, "Player.CurrentLocation.Update", Player.LocationUpdate, 8,1),
                new ModTask(500, "Player.ArrestWarrant.Update",Player.ArrestWarrantUpdate, 8,2),
                new ModTask(500, "World.Vehicles.Tick", World.CleanUpVehicles, 9,1),
                new ModTask(150, "Player.SearchMode.UpdateWanted", Player.SearchModeUpdate, 11,0),
                new ModTask(150, "Player.SearchMode.StopVanillaSearchMode", Player.StopVanillaSearchMode, 11,1),
                new ModTask(500, "World.Scanner.Tick", Scanner.Tick, 12,0),
                new ModTask(1000, "World.Vehicles.UpdatePlates", World.UpdateVehiclePlates, 13,0),

                //VEHICLE STUFF HAS BEEN UPDATED SINCE< NEED TO ADD IT!!!! IF USE THIS AGAIN!!!
                new ModTask(500, "World.Dispatch.DeleteChecking", Dispatcher.Recall, 15,0),
                new ModTask(500, "World.Dispatch.SpawnChecking", Dispatcher.Dispatch, 15,1),


                //New Tasking
                new ModTask(500, "NewTasking.Update", Tasker.RunPoliceTasks, 16,0),
                new ModTask(500, "NewTasking.Update", Tasker.RunCiviliansTasks, 16,1),
                new ModTask(500, "NewTasking.Update", Tasker.UpdatePoliceTasks, 16,2),
                new ModTask(500, "NewTasking.Update", Tasker.UpdateCivilianTasks, 16,3),
            };
        }
        private void SetupModTasksAlt()
        {
            MyTickTasks = new List<ModTask>()
            {

                //Required Run
             //   new ModTask(100, "Time.Tick", Time.Tick, 0,0),//0
               //new ModTask(100, "Input.Tick", Input.Update, 1,0),//0
             //   new ModTask(100, "VanillaManager.Tick", VanillaManager.Tick, 2,0),//0

               // new ModTask(100, "Player.Update", Player.Update, 3,0),//25

                new ModTask(300, "World.Police.Tick", Police.Update, 3,1),//100

                new ModTask(200, "Player.Violations.Update", Player.ViolationsUpdate, 5,0),
                new ModTask(200, "Player.CurrentPoliceResponse.Update", Player.PoliceResponse.Update, 5,1),

                new ModTask(500, "Player.Investigation.Update", Player.Investigation.Update, 6,0),//150
                new ModTask(250, "Player.SearchModeUpdate", Player.SearchModeUpdate, 6,1),//150
                new ModTask(150, "Player.StopVanillaSearchMode", Player.StopVanillaSearchMode, 6,2),
                new ModTask(500, "Player.TrafficViolationsUpdate", Player.TrafficViolationsUpdate, 6,3),
                new ModTask(500, "Player.LocationUpdate", Player.LocationUpdate, 6,4),
                new ModTask(500, "Player.ArrestWarrantUpdate",Player.ArrestWarrantUpdate, 6,5),

                new ModTask(250, "Civilians.Update", Civilians.Update, 7,0),
                new ModTask(250, "World.PrunePedestrians", World.PrunePedestrians, 7,1),
                new ModTask(1000, "World.ScanForPedestrians", World.ScanForPedestrians, 7,2),
                new ModTask(1000, "World.CreateNewPedestrians", World.CreateNewPedestrians, 7,3),

                new ModTask(250, "World.PruneVehicles", World.PruneVehicles, 7,4),
                new ModTask(1000, "World.ScanForVehicles", World.ScanForVehicles, 7,5),
                new ModTask(1000, "World.CreateNewVehicles", World.CreateNewVehicles, 7,6), //very bad performance
                new ModTask(500, "World.CleanUpVehicles", World.CleanUpVehicles, 7,7),
                new ModTask(1000, "World.UpdateVehiclePlates", World.UpdateVehiclePlates, 7,8),

                new ModTask(500, "Scanner.Tick", Scanner.Tick, 7,9),
                new ModTask(500, "Dispatcher.Recall", Dispatcher.Recall, 7,10),
                new ModTask(500, "Dispatcher.Dispatch", Dispatcher.Dispatch, 7,11),

                //New Tasking
                new ModTask(500, "Tasker.RunTasks", Tasker.RunPoliceTasks, 7,12),
                new ModTask(500, "Tasker.RunTasks", Tasker.RunCiviliansTasks, 7,13),
                new ModTask(500, "Tasker.UpdatePoliceTasks", Tasker.UpdatePoliceTasks, 7,14), //very bad performance, trying to limit counts
                new ModTask(500, "Tasker.UpdateCivilianTasks", Tasker.UpdateCivilianTasks, 7,15), //very bad performance, trying to limit counts
            //NEED TO CHECK THE GETAGENCY THING FOR IT BEING SLOW?
            //COULT BE CALLED BY LOTS OF THE SLOW STUFF
            //PRUNE CHECKS AGENCY? slow? create vehicle? slow
            //dispatcher? slow?

            //change zone to be hawick, street to be whatever, agency to lspd and see if we get the weird slowness
            //store the whole agency instead of looking it up all the time *in jurisdiction, maybe agency, etc.)
            
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
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace,0);
                    Dispose();
                }
            }, "Run Debug Logic");
        }
        private void StartGameLogicOLD()
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
                                EntryPoint.WriteToConsole($"GameLogic Tick took > 5 ms ({TickStopWatch.ElapsedMilliseconds} ms), aborting, Last Ran {LastRanTask} in {LastRanTaskLocation}",5);
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

                                EntryPoint.WriteToConsole($"RUNNING BEHIND FOR ({RunningBehind.DebugName} Time Since Run {Game.GameTime - RunningBehind.GameTimeLastRan} Miss Length {RunningBehind.IntervalMissLength}",5);
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
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace,0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~has crashed and needs to be restarted");
                    Dispose();
                }
            }, "Run Game Logic");
            GameFiber.Yield();
        }
        private void StartGameLogic()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                        PrevLastRanTask = LastRanTask;

                        Time.Tick();
                        Input.Update();
                        VanillaManager.Tick();
                        Player.Update();
                        ModTask ToRun = MyTickTasks.Where(x => x.ShouldRun).OrderBy(x => x.MissedInterval ? 0 : 1).OrderBy(x => x.GameTimeLastRan).OrderBy(x => x.RunOrder).FirstOrDefault();//should also check if something has barely ran or
                        if (ToRun != null)
                        {
                            ToRun.Run();
                            LastRanTask = ToRun.DebugName;
                        }
                        if (Game.FrameRate <= 50)
                        {
                            EntryPoint.WriteToConsole($"GameLogic Slow FrameTime {Game.FrameTime} FPS {Game.FrameRate}", 3);
                            EntryPoint.WriteToConsole($"Ran: {LastRanTask}", 3);
                            EntryPoint.WriteToConsole($"Ran Last Tick: {PrevLastRanTask}", 3);      
                        }
                        GameFiber.Yield();
                    }

                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~has crashed and needs to be restarted");
                    Dispose();
                }
            }, "Run Game Logic");
            GameFiber.Yield();
        }
        private void StartGameLogicOld2()
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
                            if (RunGroup >= 2 && TickStopWatch.ElapsedMilliseconds >= 3)//16//Abort processing, we are running over time? might not work with any yields?, still do the most important ones
                            {
                                EntryPoint.WriteToConsole($"GameLogic Tick took > 3 ms ({TickStopWatch.ElapsedMilliseconds} ms), aborting, Last Ran {LastRanTask} in {LastRanTaskLocation} FrameTime {Game.FrameTime} FPS {Game.FrameRate}", 3);
                                EntryPoint.WriteToConsole($"Ran: {string.Join(";", MyTickTasks.Where(x => x.RanThisTick == true && x.Interval > 0).Select(x => x.DebugName).ToList())}", 3);
                                break;
                            }

                            ModTask ToRun = MyTickTasks.Where(x => x.RunGroup == RunGroup && x.ShouldRun).OrderBy(x => x.MissedInterval ? 0 : 1).OrderBy(x => x.GameTimeLastRan).OrderBy(x => x.RunOrder).FirstOrDefault();//should also check if something has barely ran or
                            if (ToRun != null)
                            {
                                ToRun.Run();
                                LastRanTask = ToRun.DebugName;
                                LastRanTaskLocation = "Regular";
                            }

                            ModTask RunningBehind = MyTickTasks.Where(x => x.RunGroup == RunGroup && x.RunningBehind).OrderBy(x => x.GameTimeLastRan).FirstOrDefault();//should also check if something has barely ran or
                            //foreach (ModTask RunningBehind in MyTickTasks.Where(x => x.RunGroup == RunGroup && x.RunningBehind))
                            if (RunningBehind != null)
                            {
                                EntryPoint.WriteToConsole($"RUNNING BEHIND FOR ({RunningBehind.DebugName} Time Since Run {Game.GameTime - RunningBehind.GameTimeLastRan} Miss Length{RunningBehind.IntervalMissLength}", 3);
                                //RunningBehind.Run();
                                //LastRanTask = ToRun.DebugName;
                                //LastRanTaskLocation = "RunningBehind";
                            }
                        }
                        //if (Game.FrameRate <= 60)
                        //{
                        //    EntryPoint.WriteToConsole($"GameLogic Slow FrameTime {Game.FrameTime} FPS {Game.FrameRate}", 3);
                        //    EntryPoint.WriteToConsole($"Ran: {string.Join(";", MyTickTasks.Where(x => x.RanThisTick == true).Select(x => x.DebugName).ToList())}", 3);
                        //}
                        MyTickTasks.ForEach(x => x.RanThisTick = false);
                        TickStopWatch.Reset();
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~has crashed and needs to be restarted");
                    Dispose();
                }
            }, "Run Game Logic");
            GameFiber.Yield();
        }
        private void StartGameLogic2()
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
                            if (TickStopWatch.ElapsedMilliseconds >= 3)//16//Abort processing, we are running over time? might not work with any yields?, still do the most important ones
                            {
                                EntryPoint.WriteToConsole($"GameLogic Tick took > 3 ms ({TickStopWatch.ElapsedMilliseconds} ms), aborting, Last Ran {LastRanTask} in {LastRanTaskLocation} FrameTime {Game.FrameTime}",5);
                                break;
                            }

                            if(RunGroup <= 2)
                            {
                                foreach(ModTask modTask in MyTickTasks.Where(x => x.RunGroup == RunGroup && x.ShouldRun))
                                {
                                    modTask.Run();
                                    LastRanTask = modTask.DebugName;
                                    LastRanTaskLocation = "Regular";
                                }
                            }

                            LastRunGroupRan = RunGroup;








                            ModTask ToRun = MyTickTasks.Where(x => x.RunGroup == RunGroup && x.ShouldRun).OrderBy(x => x.MissedInterval ? 0 : 1).OrderBy(x => x.GameTimeLastRan).OrderBy(x => x.RunOrder).FirstOrDefault();//should also check if something has barely ran or
                            if (ToRun != null)
                            {
                                ToRun.Run();
                                LastRanTask = ToRun.DebugName;
                                LastRanTaskLocation = "Regular";
                            }

                            ModTask RunningBehind = MyTickTasks.Where(x => x.RunGroup == RunGroup && x.RunningBehind).OrderBy(x => x.GameTimeLastRan).FirstOrDefault();//should also check if something has barely ran or
                            //foreach (ModTask RunningBehind in MyTickTasks.Where(x => x.RunGroup == RunGroup && x.RunningBehind))
                            if (RunningBehind != null)
                            {
                                EntryPoint.WriteToConsole($"RUNNING BEHIND FOR ({RunningBehind.DebugName} Time Since Run {Game.GameTime - RunningBehind.GameTimeLastRan} Miss Length{RunningBehind.IntervalMissLength}",5);
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
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace,0);
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
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace,0);
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