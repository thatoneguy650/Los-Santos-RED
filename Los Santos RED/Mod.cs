using Rage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LosSantosRED.lsr
{
    public static class Mod
    {
        private static List<ModTask> MyTickTasks;
        private static readonly Stopwatch TickStopWatch = new Stopwatch();
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
            {
                GameFiber.Yield();
            }

            DataMart.ReadConfig();
            Player.AddSpareLicensePlates();
            World.CreateLocationBlips();

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
                        TickStopWatch.Start();

                        foreach (int RunGroup in MyTickTasks.GroupBy(x => x.RunGroup).Select(x => x.First()).ToList().Select(x => x.RunGroup))
                        {
                            if (RunGroup >= 4 && TickStopWatch.ElapsedMilliseconds >= 16)//Abort processing, we are running over time? might not work with any yields?, still do the most important ones
                            {
                                Debug.WriteToLog("GameLogic", string.Format("Tick took > 16 ms ({0} ms), aborting", TickStopWatch.ElapsedMilliseconds));
                                break;
                            }

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


            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                        Debug.DebugLoop();
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
        private static void SetupModTasks()
        {
            MyTickTasks = new List<ModTask>()
            {
                new ModTask(0, "World.Clock.Tick", World.Time.Tick, 0,0),
                new ModTask(0, "Input.Tick", Input.Tick, 1,0),

                new ModTask(25, "Player.Update", Player.Update, 2,0),
                new ModTask(100, "World.PoliceForce.Tick", World.Police.Tick, 2,1),//25

                new ModTask(200, "Player.Violations.Update", Player.Violations.Update, 3,0),//50
                new ModTask(200, "Player.CurrentPoliceResponse.Update", Player.CurrentPoliceResponse.Update, 3,1),//50

                new ModTask(150, "Player.Investigations.Tick", Player.Investigations.Tick, 4,0),
                new ModTask(500, "World.Civilians.Tick", World.Civilians.Tick, 4,1),//150

                new ModTask(200, "World.PedDamage.Tick", World.Wounds.Tick, 5,0),
                new ModTask(250, "Player.MuggingTick", Player.MuggingTick, 5,1),

                new ModTask(250, "World.Pedestrians.Prune", World.Pedestrians.Prune, 6,0),
                new ModTask(1000, "World.Pedestrians.Scan", World.Pedestrians.Scan, 6,1),
                new ModTask(250, "World.Vehicles.CleanLists", World.Vehicles.CleanLists, 6,2),
                new ModTask(1000, "World.Vehicles.Scan", World.Vehicles.Scan, 6,3),
                
                new ModTask(250, "Player.WeaponDropping.Tick", Player.WeaponDropping.Tick, 7,0),

                new ModTask(500, "Player.Violations.TrafficUpdate", Player.Violations.TrafficUpdate, 8,0),
                new ModTask(500, "Player.CurrentLocation.Update", Player.CurrentLocation.Update, 8,1),
                new ModTask(500, "Player.ArrestWarrant.Update", Player.ArrestWarrant.Update, 8,2),
                new ModTask(500, "World.PoliceForce.SpeechTick", World.Police.SpeechTick, 9,0),
                new ModTask(500, "World.Vehicles.Tick", World.Vehicles.Tick, 9,1),

                new ModTask(500, "World.Update",World.Update,10,0),//not as scary as it seems

                new ModTask(150, "Player.SearchMode.UpdateWanted", Player.SearchMode.UpdateWanted, 11,0),
                new ModTask(150, "Player.SearchMode.StopVanillaSearchMode", Player.SearchMode.StopVanilla, 11,1),
                new ModTask(500, "World.Scanner.Tick", World.Scanner.Tick, 12,0),
                new ModTask(100, "Audio.Tick",Audio.Tick,13,0),

                new ModTask(500, "World.Tasking.UpdatePeds", World.Tasking.UpdatePeds, 14,0),
                new ModTask(500, "World.Tasking.Tick", World.Tasking.TaskCops, 14,1),
                new ModTask(750, "World.Tasking.Tick", World.Tasking.TaskCivilians, 14,2),

                new ModTask(500, "World.Dispatch.SpawnChecking", World.Dispatcher.Dispatch, 15,0),
                new ModTask(500, "World.Dispatch.DeleteChecking", World.Dispatcher.Recall, 15,1),
                new ModTask(1000, "World.Vehicles.UpdatePlates", World.Vehicles.UpdatePlates, 16,2),
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
