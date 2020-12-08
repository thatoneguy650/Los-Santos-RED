using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr
{
    public static class Mod
    {
        private static List<TickTask> MyTickTasks;
        public static VehicleEngineManager VehicleEngineManager { get; private set; } = new VehicleEngineManager();
        public static PedManager PedManager { get; private set; } = new PedManager();
        public static ClockManager ClockManager { get; private set; } = new ClockManager();
        public static PolicePerception PolicePerception { get; private set; } = new PolicePerception();
        public static CivilianPerception CivilianPerception { get; private set; } = new CivilianPerception();
        public static InputManager InputManager { get; private set; } = new InputManager();
        public static UIManager UIManager { get; private set; } = new UIManager();
        public static CrimeManager CrimeManager { get; private set; } = new CrimeManager();
        public static Player Player { get; private set; } = new Player();
        public static Map Map { get; private set; } = new Map();
        public static bool IsRunning { get; set; }
        public static void Initialize()
        {
            IsRunning = true;

            while (Game.IsLoading)
                GameFiber.Yield();
 
            Setup();
            RunTasks();
        }
        public static void Dispose()
        {
            IsRunning = false;//NEED TO MOVE THIS TO AUTO RUN, maybe add dispose as a field it inherits
            Player.Dispose();
            ClockManager.Dispose();
            PedManager.Dispose();
            Map.RemoveBlips();
        }
        public static void RunTasks()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                        foreach (int RunGroup in MyTickTasks.GroupBy(x => x.RunGroup).Select(x => x.First()).ToList().Select(x => x.RunGroup))
                        {
                            TickTask ToRun = MyTickTasks.Where(x => x.RunGroup == RunGroup && x.ShouldRun).OrderBy(x => x.MissedInterval ? 0 : 1).OrderBy(x => x.GameTimeLastRan).OrderBy(x => x.RunOrder).FirstOrDefault();//should also check if something has barely ran or 
                            if (ToRun != null)
                            {
                                ToRun.RunTask();
                            }
                            foreach (TickTask RunningBehind in MyTickTasks.Where(x => x.RunGroup == RunGroup && x.RunningBehind))
                            {
                                RunningBehind.RunTask();
                            }
                        }
                        ResetRanItems();
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    Dispose();
                    Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
                }
            }, "Run Game Logic");
            GameFiber.Yield();
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                        MenuManager.Tick();
                        UIManager.Tick();
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    Dispose();
                    Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
                }
            }, "Run Menu/UI Logic");
        }
        private static void Setup()
        {
            MyTickTasks = new List<TickTask>()
        {
            new TickTask(0, "ClockManager", ClockManager.Tick, 0,0),

            new TickTask(0, "InputManager", InputManager.Tick, 1,0),

            new TickTask(25, "Player", Player.Tick, 2,0),
            new TickTask(25, "PolicePerception", PolicePerception.Tick, 2,1),

            new TickTask(0, "VehicleEngineManager", VehicleEngineManager.Tick, 3,0),

            new TickTask(50, "CrimeManager", CrimeManager.Tick, 4,0),
            new TickTask(50, "WantedLevelManager", WantedLevelManager.Tick, 4,1),

            new TickTask(150, "SearchModeManager", SearchModeManager.Tick, 5,0),
            new TickTask(150, "InvestigationManager", InvestigationManager.Tick, 5,1),
            new TickTask(150, "SearchModeStoppingManager", SearchModeStoppingManager.Tick, 5,2),
            new TickTask(150, "CivilianManager", CivilianPerception.Tick, 5,3),

            new TickTask(200, "PedDamageManager", PedDamageManager.Tick, 6,0),
            new TickTask(250, "MuggingManager", MuggingManager.Tick, 6,1),

            new TickTask(1000, "PedManager", Mod.PedManager.Tick, 7,0),
            new TickTask(1000, "VehicleManager", VehicleManager.Tick, 7,1),
            new TickTask(500, "TaskManager.UpdateTaskablePeds", TaskManager.UpdateTaskablePeds, 7,3),
            new TickTask(500, "TaskManager.RunActivities", TaskManager.RunActivities, 7,4),

            new TickTask(250, "WeaponDroppingManager", WeaponDroppingManager.Tick, 8,0),

            new TickTask(500, "TrafficViolationsManager", TrafficViolationsManager.Tick, 9,0),
            new TickTask(500, "PlayerLocationManager", PlayerLocationManager.Tick, 9,1),
            new TickTask(500, "PersonOfInterestManager", PersonOfInterestManager.Tick, 9,2),

            new TickTask(250, "PoliceEquipmentManager", PoliceEquipmentManager.Tick, 10,0),
            new TickTask(500, "ScannerManager", ScannerManager.Tick, 10,1),
            new TickTask(500, "PoliceSpeechManager", PoliceSpeechManager.Tick, 10,2),
            new TickTask(500, "PoliceSpawningManager", PoliceSpawningManager.Tick, 10,3),

            new TickTask(500, "DispatchManager.SpawnChecking", DispatchManager.SpawnChecking, 11,0),
            new TickTask(500, "DispatchManager.DeleteChecking", DispatchManager.DeleteChecking, 11,1),

            new TickTask(100, "VehicleFuelManager", VehicleFuelManager.Tick, 12,0),
            new TickTask(100, "VehicleIndicatorManager", VehicleIndicatorManager.Tick, 12,1),
            new TickTask(500, "RadioManager", RadioManager.Tick, 12,2),
        };
            Map.CreateLocationBlips();
        }
        private static void ResetRanItems()
        {
            MyTickTasks.ForEach(x => x.RanThisTick = false);
        }
        private class TickTask
        {
            public bool RanThisTick = false;
            public uint GameTimeLastRan = 0;
            public uint Interval = 500;
            public uint IntervalMissLength;
            public string DebugName;
            public Action TickToRun;
            public int RunGroup;
            public int RunOrder;
            public TickTask(uint _Interval, string _DebugName, Action _TickToRun, int _RunGroup, int _RunOrder)
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
            public void RunTask()
            {
                TickToRun();
                GameTimeLastRan = Game.GameTime;
                RanThisTick = true;
            }

        }
    }
}
