using LosSantosRED.lsr.Helper;
using Rage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LosSantosRED.lsr
{
    public static class Mod
    {
        private static readonly Stopwatch TickStopWatch = new Stopwatch();
        private static string LastRanTask;
        private static List<ModTask> MyTickTasks;
        private static bool HasSwappedPeds;
        public static Audio Audio { get; private set; } = new Audio();
        public static DataMart DataMart { get; private set; } = new DataMart();
        public static Debug Debug { get; private set; } = new Debug();
        public static Input Input { get; private set; } = new Input();
        public static bool IsRunning { get; private set; }
        public static Menu Menu { get; private set; } = new Menu();
        public static PedSwap PedSwap { get; private set; } = new PedSwap();
        public static Player Player { get; private set; } = new Player();
        public static UI UI { get; private set; } = new UI();
        public static World World { get; private set; } = new World();
        public static void Dispose()
        {
            IsRunning = false;
            GameFiber.Sleep(500);
            Player.Dispose();
            World.Dispose();
            PedSwap.Dispose();
            if (DataMart.Settings.SettingsManager.General.PedTakeoverSetRandomMoney && PedSwap.OriginalMoney > 0)
            {
                Player.SetMoney(PedSwap.OriginalMoney);
            }
        }
        public static void NewPlayer(string ModelName, bool Male)
        {

            HasSwappedPeds = true;
            Player.Reset(true,true,true);// = new Player();//will break the static reference for some reason, need more info
            Player.GiveName(ModelName, Male);
            if (DataMart.Settings.SettingsManager.General.PedTakeoverSetRandomMoney)
            {
                Player.SetMoney(RandomItems.MyRand.Next(DataMart.Settings.SettingsManager.General.PedTakeoverRandomMoneyMin, DataMart.Settings.SettingsManager.General.PedTakeoverRandomMoneyMax));
            }
        }
        public static void Start()
        {
            IsRunning = true;
            while (Game.IsLoading)
            {
                GameFiber.Yield();
            }
            DataMart.ReadConfig();
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
        private static void SetupModTasks()
        {
            MyTickTasks = new List<ModTask>()
            {
               new ModTask(0, "World.UpdateTime", World.UpdateTime, 0,0),
                new ModTask(0, "Input.Tick", Input.Update, 1,0),

                new ModTask(25, "Player.Update", Player.Update, 2,0),
                new ModTask(100, "World.Police.Tick", World.UpdatePolice, 2,1),//25

                new ModTask(200, "Player.Violations.Update", Player.ViolationsUpdate, 3,0),//50
                new ModTask(200, "Player.CurrentPoliceResponse.Update", Player.CurrentPoliceResponse.Update, 3,1),//50

                new ModTask(150, "Player.Investigations.Tick", Player.Investigations.Update, 4,0),
                new ModTask(500, "World.Civilians.Tick", World.UpdateCivilians, 4,1),//150

                //new ModTask(200, "World.PedDamage.Tick", World.Wounds.Tick, 5,0),//moved to the ped updates for now, might need to readd them here
                new ModTask(250, "Player.MuggingTick", Player.MuggingUpdate, 5,1),

                new ModTask(250, "World.Pedestrians.Prune", World.PrunePedestrians, 6,0),
                new ModTask(1000, "World.Pedestrians.Scan", World.ScaneForPedestrians, 6,1),
                new ModTask(250, "World.Vehicles.CleanLists", World.PruneVehicles, 6,2),
                new ModTask(1000, "World.Vehicles.Scan", World.ScanForVehicles, 6,3),

                //new ModTask(250, "Player.WeaponDropping.Tick", Player.WeaponDropping.Tick, 7,0),//moved into the player

                new ModTask(500, "Player.Violations.TrafficUpdate", Player.TrafficViolationsUpdate, 8,0),
                new ModTask(500, "Player.CurrentLocation.Update", Player.LocationUpdate, 8,1),
                new ModTask(500, "Player.ArrestWarrant.Update", Player.ArrestWarrantUpdate, 8,2),
               // new ModTask(500, "World.PoliceForce.SpeechTick", World.UpdatePoliceSpeech, 9,0),//moved to regular police updates
                new ModTask(500, "World.Vehicles.Tick", World.VehiclesTick, 9,1),

                new ModTask(150, "Player.SearchMode.UpdateWanted", Player.SearchModeUpdate, 11,0),
                new ModTask(150, "Player.SearchMode.StopVanillaSearchMode", Player.StopVanillaSearchMode, 11,1),
                new ModTask(500, "World.Scanner.Tick", World.UpdateScanner, 12,0),

                new ModTask(100, "Audio.Tick",Audio.Update,13,0),
                new ModTask(1000, "World.Vehicles.UpdatePlates", World.UpdateVehiclePlates, 13,1),

                new ModTask(500, "World.Tasking.UpdatePeds", World.AddTaskablePeds, 14,0),
                new ModTask(500, "World.Tasking.Tick", World.TaskCops, 14,1),
                new ModTask(750, "World.Tasking.Tick", World.TaskCivilians, 14,2),//temp off for testing other stuff, dont need them calling the cops

                new ModTask(500, "World.Dispatch.DeleteChecking", World.Recall, 15,0),
                new ModTask(500, "World.Dispatch.SpawnChecking", World.Dispatch, 15,1),
            };
        }
        private static void StartDebugLogic()
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
                    Dispose();
                    Debug.WriteToLog("Error", e.Message + " : " + e.StackTrace);
                }
            }, "Run Debug Logic");
        }
        private static void StartGameLogic()
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
                                Debug.WriteToLog("GameLogic", string.Format("Tick took > 16 ms ({0} ms), aborting, Last Ran {1}", TickStopWatch.ElapsedMilliseconds, LastRanTask));
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
                    Dispose();
                    Debug.WriteToLog("Error", e.Message + " : " + e.StackTrace);
                }
            }, "Run Game Logic");
            GameFiber.Yield();
        }
        private static void StartMenuLogic()
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
                    Dispose();
                    Debug.WriteToLog("Error", e.Message + " : " + e.StackTrace);
                }
            }, "Run Menu/UI Logic");
        }
    }
}