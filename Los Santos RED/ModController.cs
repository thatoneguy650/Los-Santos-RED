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
                
                
                new ModTask(500, "Tasker.UpdatePoliceTasks", Tasker.UpdatePoliceTasks, 7,12), //very bad performance, trying to limit counts
                new ModTask(500, "Tasker.RunPoliceTasks", Tasker.RunPoliceTasks, 7,13),
                new ModTask(500, "Tasker.UpdateCivilianTasks", Tasker.UpdateCivilianTasks, 7,14), //very bad performance, trying to limit counts
                new ModTask(500, "Tasker.RunCiviliansTasks", Tasker.RunCiviliansTasks, 7,15),





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
        private void StartGameLogic()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                        TickStopWatch.Start();
                        PrevLastRanTask = LastRanTask;

                        Time.Tick();
                        Input.Update();
                        VanillaManager.Tick();
                        Player.Update();

                        if (TickStopWatch.ElapsedMilliseconds >= 2)//16//Abort processing, we are running over time? might not work with any yields?, still do the most important ones
                        {
                            EntryPoint.WriteToConsole($"GameLogic Tick took > 2 ms", 3);
                        }
                        else
                        {
                            ModTask ToRun = MyTickTasks.Where(x => x.ShouldRun).OrderBy(x => x.MissedInterval ? 0 : 1).OrderBy(x => x.GameTimeLastRan).OrderBy(x => x.RunOrder).FirstOrDefault();//should also check if something has barely ran or
                            if (ToRun != null)
                            {
                                ToRun.Run();
                                LastRanTask = ToRun.DebugName;
                            }
                            if (Game.FrameRate <= 50)
                            {
                                EntryPoint.WriteToConsole($"GameLogic Slow FrameTime {Game.FrameTime} FPS {Game.FrameRate}; Ran: {LastRanTask}, Ran Last Tick: {PrevLastRanTask}", 3);
                            }
                        }
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