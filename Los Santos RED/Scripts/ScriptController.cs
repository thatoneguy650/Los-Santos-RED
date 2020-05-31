using Rage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ScriptController
{
    private static DataTable TickTable = new DataTable();
    private static int TickID = 0;
    private static Stopwatch GameStopWatch;
    private static Stopwatch TickStopWatch;
    private static List<TickTask> MyTickTasks; 
    public static bool IsRunning { get; set; }
    public static void Initialize()
    {    
        IsRunning = true;
        GameStopWatch = new Stopwatch();
        TickStopWatch = new Stopwatch();

        TickTable.Columns.Add("TickID");
        TickTable.Columns.Add("GameTimeStarted");
        TickTable.Columns.Add("GameTimeFinished");
        TickTable.Columns.Add("DebugName");
        TickTable.Columns.Add("ElapsedMilliseconds");     

        MyTickTasks = new List<TickTask>()
        {
            new TickTask(25, "PlayerState", PlayerState.Tick, 0,0),
            new TickTask(25, "Police", Police.Tick, 0,2),

            new TickTask(25, "ControlScript", ControlScript.Tick, 1,0),

            new TickTask(0, "VehicleEngine", VehicleEngine.Tick, 2,0),
  
            new TickTask(200, "PlayerHealth", PlayerHealth.Tick, 3,0),
            new TickTask(200, "PedWoundSystem", PedWounds.Tick, 3,1),
            new TickTask(250, "MuggingSystem", MuggingScript.Tick, 3,2),

            new TickTask(0, "ClockSystem", Clock.Tick, 3,3),
            
            new TickTask(1000, "ScanForPeds", PedList.ScanForPeds, 4,0),
            new TickTask(250, "ProcessQueue", Tasking.ProcessQueue, 4,1),
            new TickTask(150, "TaskPeds", Tasking.TaskPeds, 4,2),
              
            new TickTask(250, "WeaponDropping", WeaponDropping.Tick, 5,0),

            new TickTask(150, "Civilians", Civilians.Tick, 6,0),

            new TickTask(500, "TrafficViolations", TrafficViolations.Tick, 7,0),
            new TickTask(500, "PlayerLocation", PlayerLocation.Tick, 7,1),
            new TickTask(500, "PersonOfInterest", PersonOfInterest.Tick, 7,2),

            new TickTask(500, "DispatchAudio", DispatchAudio.Tick, 8,0),
            new TickTask(500, "PoliceSpeech", PoliceSpeech.Tick, 8,2),
            new TickTask(500, "PoliceSpawning", PoliceSpawning.CheckSpawn, 8,3),
            new TickTask(500, "PoliceSpawning.RemoveCops", PoliceSpawning.CheckRemove, 8,4),
            new TickTask(1000, "ScanforPoliceVehicles", PedList.ScanforPoliceVehicles, 8,5),

            new TickTask(0, "VehicleFuelSystem", VehicleFuelSystem.Tick, 9,0),

            new TickTask(25, "WantedLevel", WantedLevelScript.Tick, 10,0),

            new TickTask(25, "Investigation", Investigation.Tick, 11,0),

            new TickTask(50, "SearchModeStopping", SearchModeStopping.Tick, 12,0),

        };

        
        General.Initialize();
        PlayerState.Initialize();
        ControlScript.Initialize();
        Debugging.Initialize();
        Agencies.Initialize();
        Zones.Initialize();
        Jurisdiction.Initialize();
        Locations.Initialize();
        Police.Initialize();
        Investigation.Initialize();
        WantedLevelScript.Initialize();
        PoliceSpawning.Initialize();
        LicensePlateTheft.Initialize();
        Menus.Intitialize();
        PedList.Initialize();
        DispatchAudio.Initialize();
        PoliceSpeech.Initialize();
        Vehicles.Initialize();
        VehicleEngine.Initialize();
        VehicleFuelSystem.Initialize();
        Tasking.Initialize();
        Weapons.Initialize();
        WeaponDropping.Initialize();
        Streets.Initialize();
        PlayerLocation.Initialize();
        TrafficViolations.Initialize();
        SearchModeStopping.Initialize();
        UI.Initialize();
        PedSwap.Initialize();
        PersonOfInterest.Initialize();
        Civilians.Initialize();
        Clock.Initialize();
        MuggingScript.Initialize();
        PlayerHealth.Initialize();
        PedWounds.Initialize();

        RunTasks();
    }
    public static void RunTasks()
    {     
        GameFiber.StartNew(delegate
        {
            try
            {
                TickID = 0;
                while (IsRunning)
                {
                    GameStopWatch.Start();
                    foreach(int RunGroup in MyTickTasks.GroupBy(x => x.RunGroup).Select(x => x.First()).ToList().Select(x => x.RunGroup))
                    {
                        TickTask ToRun = MyTickTasks.Where(x => x.RunGroup == RunGroup && x.ShouldRun).OrderBy(x => x.MissedInterval ? 0 : 1).OrderBy(x => x.RunOrder).FirstOrDefault();//should also check if something has barely ran or 
                        if (ToRun != null)
                        {
                            uint GameTimeStarted = Game.GameTime;
                            TickStopWatch.Stop();
                            ToRun.RunTask();
                            TickStopWatch.Stop();
                            TickTable.Rows.Add(TickID, GameTimeStarted, Game.GameTime, ToRun.DebugName, TickStopWatch.ElapsedMilliseconds);
                            TickStopWatch.Reset();
                        }
                        
                    }

                    GameStopWatch.Stop();

                    if (GameStopWatch.ElapsedMilliseconds >= 15)
                        Debugging.WriteToLog("InstantActionTick", string.Format("Tick took {0} ms: {1}", GameStopWatch.ElapsedMilliseconds, GetStatus()));

                    ResetRanItems();

                    GameStopWatch.Reset();
                    TickID++;
                    GameFiber.Yield();
                }
            }
            catch (Exception e)
            {
                Dispose();
                Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
            }
        });


        GameFiber.Sleep(1000);

        GameFiber.StartNew(delegate
        {
            try
            {
                while (IsRunning)
                {
                    Menus.Tick();
                    UI.Tick();
                    GameFiber.Yield();
                }
            }
            catch (Exception e)
            {
                Dispose();
                Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
            }
        });
    }
    public static void Dispose()
    {
        IsRunning = false;
        General.Dispose();
        PlayerState.Dispose();
        ControlScript.Dispose();
        LicensePlateTheft.Dispose();
        Menus.Dispose();
        PedList.Dispose();
        DispatchAudio.Dispose();
        PoliceSpeech.Dispose();
        Vehicles.Dispose();
        VehicleEngine.Dispose();
        VehicleFuelSystem.Dispose();
        Smoking.Dispose();
        Tasking.Dispose();
        Agencies.Dispose();
        Locations.Dispose();
        Weapons.Dispose();
        WeaponDropping.Dispose();
        Streets.Dispose();
        UI.Dispose();
        Debugging.Dispose();
        PlayerLocation.Dispose();
        Police.Dispose();
        Investigation.Dispose();
        WantedLevelScript.Dispose();
        PoliceSpawning.Dispose();
        TrafficViolations.Dispose();
        SearchModeStopping.Dispose();
        PedSwap.Dispose();
        PersonOfInterest.Dispose();
        Civilians.Dispose();
        PlayerHealth.Dispose();
        Clock.Dispose();
        MuggingScript.Dispose();
        PedWounds.Dispose();
    }
    private static void ResetRanItems()
    {
        MyTickTasks.ForEach(x => x.RanThisTick = false);
    }
    public static string GetStatus()
    {
        return "Name:RanThisTick:GameTimeLastRan:MissedInterval:"  + string.Join("|",MyTickTasks.Where(x => x.RanThisTick || x.MissedInterval).Select(x => x.DebugName + ":" + x.RanThisTick + ":" + x.GameTimeLastRan + ":" + x.MissedInterval));
    }
    public static void OutputTable()
    {
        StringBuilder sb = new StringBuilder();

        IEnumerable<string> columnNames = TickTable.Columns.Cast<DataColumn>().
                                          Select(column => column.ColumnName);
        sb.AppendLine(string.Join(",", columnNames));

        foreach (DataRow row in TickTable.Rows)
        {
            IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
            sb.AppendLine(string.Join(",", fields));
        }

        File.WriteAllText("TickTable.csv", sb.ToString());
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
                if (GameTimeLastRan == 0)
                    return true;
                else if (Game.GameTime - GameTimeLastRan >= IntervalMissLength)
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

