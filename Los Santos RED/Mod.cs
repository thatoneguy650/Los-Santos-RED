using Rage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LosSantosRED.lsr
{
    public static class Mod
    {
        private static List<ModTask> MyTickTasks;
        public static Debug Debug { get; private set; } = new Debug();
        public static Player Player { get; private set; } = new Player();
        public static World World { get; private set; } = new World();
        public static Input Input { get; private set; } = new Input();
        public static UI UI { get; private set; } = new UI();
        public static Audio Audio { get; private set; } = new Audio();
        public static Menu Menu { get; private set; } = new Menu();
        public static DataMart DataMart { get; private set; } = new DataMart();
        public static bool IsRunning { get; private set; }
        public static void Start()
        {
            IsRunning = true;

            while (Game.IsLoading)
                GameFiber.Yield();

            DataMart.ReadConfig();
            Player.AddSpareLicensePlates();
            World.CreateLocationBlips();
            Debug.Start();

            SetupModTasks();
            RunTasks();

            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~g~Loaded", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~v0.1 Loaded by Greskrendtregk");
        }
        public static void Dispose()
        {
            IsRunning = false;
            GameFiber.Sleep(500);
            Player.Dispose();
            World.Dispose();
        }
        private static void RunTasks()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                        foreach (int RunGroup in MyTickTasks.GroupBy(x => x.RunGroup).Select(x => x.First()).ToList().Select(x => x.RunGroup))
                        {
                            ModTask ToRun = MyTickTasks.Where(x => x.RunGroup == RunGroup && x.ShouldRun).OrderBy(x => x.MissedInterval ? 0 : 1).OrderBy(x => x.GameTimeLastRan).OrderBy(x => x.RunOrder).FirstOrDefault();//should also check if something has barely ran or 
                            if (ToRun != null)
                            {
                                ToRun.Run();
                            }
                            foreach (ModTask RunningBehind in MyTickTasks.Where(x => x.RunGroup == RunGroup && x.RunningBehind))
                            {
                                RunningBehind.Run();
                            }
                        }
                        MyTickTasks.ForEach(x => x.RanThisTick = false);
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

            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                        Menu.Tick();
                        UI.Tick();
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
        private static void SetupModTasks()
        {
            MyTickTasks = new List<ModTask>()
            {
                new ModTask(0, "ClockManager", World.Clock.Tick, 0,0),

                new ModTask(0, "InputManager", Input.Tick, 1,0),

                new ModTask(25, "Player", Player.Update, 2,0),
                new ModTask(25, "PolicePerception", World.PoliceForce.Tick, 2,1),




                new ModTask(50, "CrimeManager", Player.Violations.Update, 4,0),
                new ModTask(50, "WantedLevelManager", Player.CurrentPoliceResponse.Update, 4,1),


                new ModTask(150, "InvestigationManager", Player.Investigations.Tick, 5,0),
                new ModTask(150, "CivilianManager", World.Civilians.Tick, 5,1),

                new ModTask(200, "PedDamageManager", World.PedDamage.Tick, 6,0),
                new ModTask(250, "MuggingManager", Player.MuggingTick, 6,1),

                new ModTask(250, "PrunePedList", World.Pedestrians.PrunePeds, 7,0),
                new ModTask(1000, "ScanForNewPeds", World.Pedestrians.ScanForPeds, 7,1),

                new ModTask(250, "PruneVehicleList", World.Vehicles.CleanLists, 7,2),
                new ModTask(1000, "VehicleManager", World.Vehicles.Scan, 7,3),
                new ModTask(500, "VehicleManager", World.Vehicles.UpdatePlates, 7,4),

                new ModTask(250, "WeaponDroppingManager", Player.WeaponDropping.Tick, 8,0),

                new ModTask(500, "TrafficViolationsManager", Player.Violations.TrafficUpdate, 9,0),
                new ModTask(500, "PlayerLocationManager", Player.CurrentLocation.Update, 9,1),
                new ModTask(500, "ArrestWarrant", Player.ArrestWarrant.Update, 9,2),

                new ModTask(500, "PoliceSpeechManager", World.PoliceForce.SpeechTick, 10,2),
                new ModTask(500, "PoliceSpawningManager", World.PoliceSpawning.Tick, 10,3),


                new ModTask(500, "DispatchManager.SpawnChecking", World.Dispatch.SpawnChecking, 11,0),
                new ModTask(500, "DispatchManager.DeleteChecking", World.Dispatch.DeleteChecking, 11,1),

                new ModTask(500, "WorldCache",World.CacheWorldData,13,0),

                new ModTask(500, "TaskManager.UpdateTaskablePeds", World.Tasking.UpdateTaskablePeds, 14,0),
                new ModTask(500, "TaskManager.RunActivities", World.Tasking.RunActivities, 14,1),


                new ModTask(150, "SearchModeManager", Player.SearchMode.UpdateWanted, 15,0),
                new ModTask(150, "SearchModeStoppingManager", Player.SearchMode.StopVanillaSearchMode, 15,1),

                new ModTask(500, "ScannerManager", World.Scanner.Tick, 16,0),


                new ModTask(100, "Audio",Audio.Tick,17,0),


            };
        }
        private class ModTask
        {
            public bool RanThisTick = false;
            public uint GameTimeLastRan = 0;
            public uint Interval = 500;
            public uint IntervalMissLength;
            public string DebugName;
            public Action TickToRun;
            public int RunGroup;
            public int RunOrder;
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
            public bool ShouldRun
            {
                get
                {
                    if (GameTimeLastRan == 0)
                        return true;
                    else if (Game.GameTime - GameTimeLastRan > Interval)
                        return true;
                    else
                        return false;
                }
            }
            public bool MissedInterval
            {
                get
                {
                    if (Interval == 0)
                        return false;
                    //if (GameTimeLastRan == 0)
                    //    return true;
                    else if (Game.GameTime - GameTimeLastRan >= IntervalMissLength)
                        return true;
                    else
                        return false;
                }
            }
            public bool RunningBehind
            {
                get
                {
                    if (Interval == 0)
                        return false;
                    //if (GameTimeLastRan == 0)
                    //    return true;
                    else if (Game.GameTime - GameTimeLastRan >= (IntervalMissLength * 2))
                        return true;
                    else
                        return false;
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
