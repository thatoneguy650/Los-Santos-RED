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
        private string LastRanTask;
        private List<ModTask> MyTickTasks;
        private Mod.World World;
        private Audio Audio;
        private PedSwap PedSwap;
        private Mod.Player Player;
        private Debug Debug;
        private Scanner Scanner;
        private Input Input;
        private Menu Menu;
        private Police Police;
        private Civilians Civilians;
        private Respawning Respawning;
        private UI UI;
        private Dispatcher Dispatcher;
        private SearchMode SearchMode;
        private Spawner Spawner;
        private Tasking Tasking;
        private PlacesOfInterest PlacesOfInterest;
        private Agencies Agencies;
        private CountyJurisdictions CountyJurisdictions;
        private ZoneJurisdictions ZoneJurisdictions;
        private Zones Zones;
        private PlateTypes PlateTypes;
        private Settings Settings;
        private Streets Streets;
        private Weapons Weapons;
        private Names Names;
        private VehicleScannerAudio VehicleScannerAudio;
        private ZoneScannerAudio ZoneScannerAudio;
        private StreetScannerAudio StreetScannerAudio;

        public ModController()
        {
            //DataMart = new DataMart();
            //Audio = new Audio(DataMart);
            //World = new Mod.World(DataMart);  
            //Player = new Mod.Player(World,DataMart);
            //Input = new Input(Player,DataMart);
            //Police = new Police(World, Player, DataMart);
            //Civilians = new Civilians(World, Player);
            //Spawner = new Spawner(World, DataMart);
            //Dispatcher = new Dispatcher(World, Player, Police, Spawner, DataMart);
            //Respawning = new Respawning(World, Player, DataMart);
            //PedSwap = new PedSwap(World, Player, DataMart);
            //SearchMode = new SearchMode(World, Player, Police);
            //Tasking = new Tasking(World, Player, Police);
            //UI = new UI(World, Player, SearchMode, DataMart);
            //Scanner = new Scanner(World, Player, Police, Audio, Respawning, SearchMode, DataMart);
            //Menu = new Menu(World, Player, PedSwap, Respawning, DataMart);
        }
        public bool IsRunning { get; private set; }
        public void NewPlayer(string ModelName, bool Male)
        {
            Player.Reset(true, true, true);
            Scanner.Reset();
            Player.GiveName(ModelName, Male, Names.GetRandomName(Male));
            if (Settings.SettingsManager.General.PedTakeoverSetRandomMoney)
            {
                Player.SetMoney(RandomItems.MyRand.Next(Settings.SettingsManager.General.PedTakeoverRandomMoneyMin, Settings.SettingsManager.General.PedTakeoverRandomMoneyMax));
            }
        }
        public void Start()
        {
            IsRunning = true;
            while (Game.IsLoading)
            {
                GameFiber.Yield();
            }
            ReadDataFiles();
            Audio = new Audio();
            World = new Mod.World(Agencies, ZoneJurisdictions, Settings, Weapons, PlacesOfInterest, Zones, PlateTypes);
            Player = new Mod.Player(World, Streets, Zones, Settings, Weapons);
            Input = new Input(Player, Settings);
            Police = new Police(World, Player);
            Civilians = new Civilians(World, Player);
            Spawner = new Spawner(World);
            Respawning = new Respawning(World, Player, Weapons, PlacesOfInterest, Settings);
            PedSwap = new PedSwap(World, Player, Settings);
            SearchMode = new SearchMode(World, Player, Police);
            Tasking = new Tasking(World, Player, Police);
            UI = new UI(World, Player, SearchMode, Settings, ZoneJurisdictions);
            Dispatcher = new Dispatcher(World, Player, Police, Spawner, Agencies, Weapons, Settings, Streets, Zones, CountyJurisdictions, ZoneJurisdictions);
            Scanner = new Scanner(World, Player, Police, Audio, Respawning, SearchMode, Settings);
            Menu = new Menu(World, Player, PedSwap, Respawning, Settings, Weapons, PlacesOfInterest);
            Debug = new Debug();

            Player.GiveName();
            Player.AddSpareLicensePlate();

            World.AddBlipsToMap();
            PedSwap.StoreInitialVariation();
            SetupModTasks();
            StartGameLogic();
            StartMenuLogic();
            StartDebugLogic();
            Game.DisplayNotification("~s~Los Santos ~r~RED ~s~v0.1 ~n~By ~g~Greskrendtregk ~n~~s~Has Loaded Successfully");
        }

        private void ReadDataFiles()
        {
            Settings = new Settings();
            Settings.ReadConfig();
            Zones = new Zones();
            Zones.ReadConfig();
            PlateTypes = new PlateTypes();
            PlateTypes.ReadConfig();
            Streets = new Streets();
            Streets.ReadConfig();
            Weapons = new Weapons();
            Weapons.ReadConfig();
            Names = new Names();
            Names.ReadConfig();
            PlacesOfInterest = new PlacesOfInterest();
            PlacesOfInterest.ReadConfig();
            VehicleScannerAudio = new VehicleScannerAudio();
            VehicleScannerAudio.ReadConfig();
            ZoneScannerAudio = new ZoneScannerAudio();
            ZoneScannerAudio.ReadConfig();
            StreetScannerAudio = new StreetScannerAudio();
            StreetScannerAudio.ReadConfig();
            Agencies = new Agencies();
            Agencies.ReadConfig();
            CountyJurisdictions = new CountyJurisdictions(Agencies);
            CountyJurisdictions.ReadConfig();
            ZoneJurisdictions = new ZoneJurisdictions(Agencies);
            ZoneJurisdictions.ReadConfig();
        }
        public void Stop()
        {
            IsRunning = false;
            GameFiber.Sleep(500);
            Player.Dispose();
            World.Dispose();
            PedSwap.Dispose();
            if (Settings.SettingsManager.General.PedTakeoverSetRandomMoney && PedSwap.OriginalMoney > 0)
            {
                Player.SetMoney(PedSwap.OriginalMoney);
            }
        }
        private void SetupModTasks()
        {
            MyTickTasks = new List<ModTask>()
            {
               new ModTask(0, "World.UpdateTime", World.UpdateTime, 0,0),
                new ModTask(0, "Input.Tick", Input.Update, 1,0),
                new ModTask(25, "Player.Update", Player.Update, 2,0),
                new ModTask(100, "World.Police.Tick", Police.Update, 2,1),//25
                new ModTask(200, "Player.Violations.Update", Player.ViolationsUpdate, 3,0),//50
                new ModTask(200, "Player.CurrentPoliceResponse.Update", Player.CurrentPoliceResponse.Update, 3,1),//50
                new ModTask(150, "Player.Investigations.Tick", Player.Investigations.Update, 4,0),
                new ModTask(500, "World.Civilians.Tick", Civilians.Update, 4,1),//150
                new ModTask(250, "Player.MuggingTick", Player.MuggingUpdate, 5,1),
                new ModTask(250, "World.Pedestrians.Prune", World.PrunePedestrians, 6,0),
                new ModTask(1000, "World.Pedestrians.Scan", World.ScaneForPedestrians, 6,1),
                new ModTask(250, "World.Vehicles.CleanLists", World.PruneVehicles, 6,2),
                new ModTask(1000, "World.Vehicles.Scan", World.ScanForVehicles, 6,3),
                new ModTask(500, "Player.Violations.TrafficUpdate", Player.TrafficViolationsUpdate, 8,0),
                new ModTask(500, "Player.CurrentLocation.Update", Player.LocationUpdate, 8,1),
                new ModTask(500, "Player.ArrestWarrant.Update",Player.ArrestWarrantUpdate, 8,2),
                new ModTask(500, "World.Vehicles.Tick", World.VehiclesTick, 9,1),
                new ModTask(150, "Player.SearchMode.UpdateWanted", SearchMode.UpdateWanted, 11,0),
                new ModTask(150, "Player.SearchMode.StopVanillaSearchMode", SearchMode.StopVanilla, 11,1),
                new ModTask(500, "World.Scanner.Tick", Scanner.Tick, 12,0),
                new ModTask(1000, "World.Vehicles.UpdatePlates", World.UpdateVehiclePlates, 13,0),
                new ModTask(500, "World.Tasking.UpdatePeds", Tasking.AddTaskablePeds, 14,0),
                new ModTask(500, "World.Tasking.Tick", Tasking.TaskCops, 14,1),
                new ModTask(750, "World.Tasking.Tick", Tasking.TaskCivilians, 14,2),
                new ModTask(500, "World.Dispatch.DeleteChecking", Dispatcher.Recall, 15,0),
                new ModTask(500, "World.Dispatch.SpawnChecking", Dispatcher.Dispatch, 15,1),
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
                    Stop();
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
                            if (RunGroup >= 4 && TickStopWatch.ElapsedMilliseconds >= 16)//Abort processing, we are running over time? might not work with any yields?, still do the most important ones
                            {
                                Game.Console.Print(string.Format("GameLogic Tick took > 16 ms ({0} ms), aborting, Last Ran {1}", TickStopWatch.ElapsedMilliseconds, LastRanTask));
                                break;
                            }

                            ModTask ToRun = MyTickTasks.Where(x => x.RunGroup == RunGroup && x.ShouldRun).OrderBy(x => x.MissedInterval ? 0 : 1).OrderBy(x => x.GameTimeLastRan).OrderBy(x => x.RunOrder).FirstOrDefault();//should also check if something has barely ran or
                            if (ToRun != null)
                            {
                                ToRun.Run();
                                LastRanTask = ToRun.DebugName;
                            }
                            foreach (ModTask RunningBehind in MyTickTasks.Where(x => x.RunGroup == RunGroup && x.RunningBehind))
                            {
                                RunningBehind.Run();
                                LastRanTask = ToRun.DebugName;
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
                    Stop();
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
                        Menu.Update();
                        UI.Update();
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    Game.Console.Print("Error" + e.Message + " : " + e.StackTrace);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~has crashed and needs to be restarted");
                    Stop();
                }
            }, "Run Menu/UI Logic");
        }
    }
}