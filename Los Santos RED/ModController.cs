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
        private WavAudio WavAudio;
       // private Audio OldAudio;
        private Civilians Civilians;
        private Crimes Crimes;
        private Debug Debug;
        private Dispatcher Dispatcher;
        private Input Input;
        private string PrevLastRanTask;
        private string LastRanTask;
        private List<ModTask> CoreTasks;
        private List<ModTask> SecondaryTasks;
        private Names Names;
        private PedSwap PedSwap;
        private PlacesOfInterest PlacesOfInterest;
        private PlateTypes PlateTypes;
        private Mod.Player Player;
        private Police Police;
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
            if (Settings.SettingsManager.General.PedTakeoverSetRandomMoney && PedSwap.CurrentPedMoney > 0)
            {
                Player.SetMoney(PedSwap.CurrentPedMoney);
            }
        }
        public void NewPlayer(string modelName, bool isMale)
        {
            Player.Reset(true, true, true, true);
            Player.SetDemographics(modelName, isMale, GetName(modelName, Names.GetRandomName(isMale)), RandomItems.MyRand.Next(Settings.SettingsManager.General.PedTakeoverRandomMoneyMin, Settings.SettingsManager.General.PedTakeoverRandomMoneyMax));
            //Scanner.Reset();
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
            //OldAudio = new Audio();
            Time = new Mod.Time();
            World = new Mod.World(Agencies, Zones, Jurisdictions, Settings, PlacesOfInterest, PlateTypes, Names, RelationshipGroups);
            World.Setup();
            
            Player = new Mod.Player(Game.LocalPlayer.Character.Model.Name, Game.LocalPlayer.Character.IsMale, GetName(Game.LocalPlayer.Character.Model.Name, Names.GetRandomName(Game.LocalPlayer.Character.IsMale)), 0, World, Time, Streets, Zones, Settings, Weapons, RadioStations, Scenarios, Crimes, WavAudio, PlacesOfInterest);
            Player.Setup();

            Input = new Input(Player, Settings);
            Police = new Police(World, Player);
            Civilians = new Civilians(World, Player);
            PedSwap = new PedSwap(Time, Player, Settings, World);
            Tasker = new Tasker(World, Player, Weapons);
            UI = new UI(Player, Settings, Jurisdictions, PedSwap, PlacesOfInterest, Player, Player, Weapons, RadioStations);
            Dispatcher = new Dispatcher(World, Player, Agencies, Settings, Streets, Zones, Jurisdictions);
            VanillaManager = new VanillaManager();
            Debug = new Debug(PlateTypes, World, Player, Streets, Dispatcher,Zones);




            World.AddBlipsToMap();//off for now as i do lots of restarts
            PedSwap.Setup();
            GameFiber.Yield();
            SetupModTasks();
            GameFiber.Yield();
            StartGameLogic();
            GameFiber.Yield();
            StartMenuLogic();
            GameFiber.Yield();
            StartInputLogic();
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
            CoreTasks = new List<ModTask>()
            {
                //Required Run
                //new ModTask(0, "Time.Tick", Time.Tick, 0),
               // new ModTask(0, "Input.Tick", Input.Update, 1),//100
                //new ModTask(100, "VanillaManager.Tick", VanillaManager.Tick, 2),
                new ModTask(100, "Player.Update", Player.Update, 3),
                //new ModTask(300, "Police.Update", Police.Update, 4),
            };
            SecondaryTasks = new List<ModTask>()
            {
                new ModTask(500, "Player.Violations.Update", Player.ViolationsUpdate, 0),
                new ModTask(500, "Player.CurrentPoliceResponse.Update", Player.PoliceResponse.Update, 1),
                new ModTask(500, "Player.Investigation.Update", Player.Investigation.Update, 2),//150
                new ModTask(500, "Player.SearchModeUpdate", Player.SearchModeUpdate, 3),//150
                new ModTask(500, "Player.StopVanillaSearchMode", Player.StopVanillaSearchMode, 4),//150
                new ModTask(500, "Player.TrafficViolationsUpdate", Player.TrafficViolationsUpdate, 5),
                new ModTask(500, "Player.LocationUpdate", Player.LocationUpdate, 6),
                new ModTask(500, "Player.ArrestWarrantUpdate",Player.ArrestWarrantUpdate, 7),
                new ModTask(500, "Civilians.Update", Civilians.Update, 8),//250
                new ModTask(1000, "World.PrunePedestrians", World.PrunePedestrians, 9),
                new ModTask(500, "World.ScanForPedestrians", World.ScanForPedestrians, 10),
                new ModTask(500, "World.CreateNewPedestrians", World.CreateNewPedestrians, 11),
                new ModTask(500, "World.PruneVehicles", World.PruneVehicles, 12),
                new ModTask(1000, "World.ScanForVehicles", World.ScanForVehicles, 13),
                new ModTask(1000, "World.CreateNewVehicles", World.CreateNewVehicles, 14), //very bad performance
                new ModTask(1000, "World.CleanUpVehicles", World.CleanUpVehicles, 15),
                new ModTask(1000, "World.UpdateVehiclePlates", World.UpdateVehiclePlates, 16),
                new ModTask(500, "Player.ScannerUpdate", Player.ScannerUpdate, 17),
                new ModTask(500, "Dispatcher.Recall", Dispatcher.Recall, 18),
                new ModTask(500, "Dispatcher.Dispatch", Dispatcher.Dispatch, 19),
                new ModTask(500, "Tasker.UpdatePoliceTasks", Tasker.UpdatePoliceTasks, 20), //very bad performance, trying to limit counts
                new ModTask(500, "Tasker.RunPoliceTasks", Tasker.RunPoliceTasks, 21),
                new ModTask(500, "Tasker.UpdateCivilianTasks", Tasker.UpdateCivilianTasks, 22), //very bad performance, trying to limit counts
                new ModTask(500, "Tasker.RunCiviliansTasks", Tasker.RunCiviliansTasks, 23),

                new ModTask(1000, "VanillaManager.Tick", VanillaManager.Tick, 24),
                new ModTask(1000, "Time.Tick", Time.Tick, 25),


                new ModTask(300, "Police.Update", Police.Update, 26),

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
        private void StartGameLogic()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                  //  bool RunCore = true;
                    int CurrentSecondaryTask = 0;
                    //int TimesRanSecondary = 0;
                    while (IsRunning)
                    {
                        if (CurrentSecondaryTask > SecondaryTasks.Count)
                        {
                            CurrentSecondaryTask = 0;
                        }
                        PrevLastRanTask = LastRanTask;
                        ModTask coreTask = CoreTasks.Where(x => x.ShouldRun).OrderBy(x => x.GameTimeLastRan).FirstOrDefault();
                        if (coreTask != null)
                        {
                            LastRanTask = coreTask.DebugName + $": TimeBetweenRuns: {Game.GameTime - coreTask.GameTimeLastRan}";
                            coreTask.Run();
                        }
                        else
                        {
                            ModTask firstSecondaryTask = SecondaryTasks.Where(x => x.ShouldRun && x.RunOrder == CurrentSecondaryTask).FirstOrDefault();
                            if(firstSecondaryTask != null)
                            {
                                LastRanTask = firstSecondaryTask.DebugName + $": TimeBetweenRuns: {Game.GameTime - firstSecondaryTask.GameTimeLastRan}";
                                firstSecondaryTask.Run();
                                CurrentSecondaryTask++;
                            }
                            else
                            {
                                ModTask alternateSecondaryTask = SecondaryTasks.Where(x => x.ShouldRun).OrderBy(x => x.GameTimeLastRan).FirstOrDefault();
                                if(alternateSecondaryTask != null)
                                {
                                    LastRanTask = alternateSecondaryTask.DebugName + $": TimeBetweenRuns: {Game.GameTime - alternateSecondaryTask.GameTimeLastRan}";
                                    alternateSecondaryTask.Run();
                                }
                                else
                                {
                                    LastRanTask = "NONE";//nothing to run at all this tick, everything is on time
                                }

                            }
                            //TimesRanSecondary++;
                            //if (TimesRanSecondary >= 2)
                            //{
                            //    RunCore = true;
                            //    TimesRanSecondary = 0;
                            //}
                        }
                        //if (CurrentSecondaryTask > SecondaryTasks.Count)
                        //{
                        //    CurrentSecondaryTask = 0;
                        //}
                        //PrevLastRanTask = LastRanTask;
                        //if (RunCore)
                        //{
                        //  //  Input.Update();
                        //    List<string> TaskList = new List<string>();
                        //    foreach (ModTask coreTask in CoreTasks.Where(x => x.ShouldRun).Take(1))
                        //    {
                        //        coreTask.Run();
                        //        TaskList.Add(coreTask.DebugName + $": TimeBetweenRuns: {Game.GameTime - coreTask.GameTimeLastRan}");
                        //    }
                        //    LastRanTask = string.Join(",", TaskList);
                        //    RunCore = false;
                        //}
                        //else
                        //{ 
                        //    ModTask ToRun = SecondaryTasks.Where(x => x.ShouldRun && x.RunOrder == CurrentSecondaryTask).FirstOrDefault();//should also check if something has barely ran or
                        //    if (ToRun != null)
                        //    {
                        //        LastRanTask = ToRun.DebugName + $": TimeBetweenRuns: {Game.GameTime - ToRun.GameTimeLastRan}";
                        //        ToRun.Run();

                        //    }
                        //    CurrentSecondaryTask++;
                        //    TimesRanSecondary++;
                        //    if (TimesRanSecondary >= 2)
                        //    {
                        //        RunCore = true;
                        //        TimesRanSecondary = 0;
                        //    }
                        //}

                        if (!Game.IsPaused && Game.FrameRate < 50)
                        {
                            EntryPoint.WriteToConsole($"GameLogic Low FPS {Game.FrameRate}; Ran: {LastRanTask}, Ran Last Tick: {PrevLastRanTask}", 3); //EntryPoint.WriteToConsole($"GameLogic Slow FrameTime {Game.FrameTime} FPS {Game.FrameRate}; Ran: {LastRanTask}, Ran Last Tick: {PrevLastRanTask}", 3);
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
        private void StartMenuLogic()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
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
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~has crashed and needs to be restarted");
                    Dispose();
                }
            }, "Run Input Logic");
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