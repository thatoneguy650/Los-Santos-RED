using Rage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ScriptController
{
    private static Stopwatch GameStopWatch;
    private static List<TickTask> MyTickTasks;
 
    public static bool IsRunning { get; set; }

    public static void Initialize()
    {    
        IsRunning = true;
        GameStopWatch = new Stopwatch();

        MyTickTasks = new List<TickTask>()
        {
            new TickTask(25, "LosSantosRED", General.Tick, 0,0),
            new TickTask(25, "Police", Police.Tick, 0,1),

            new TickTask(0, "VehicleEngine", VehicleEngine.Tick, 1,0),

            new TickTask(200, "PlayerHealth.Tick", PlayerHealth.Tick, 2,0),
            new TickTask(200, "PedWoundSystem", PedWounds.Tick, 2,1),
            new TickTask(250, "MuggingSystem", Mugging.Tick, 2,2),
            new TickTask(0, "ClockSystem", Clock.Tick, 2,3),
            new TickTask(0, "VehicleFuelSystem", VehicleFuelSystem.Tick, 2,4),

            new TickTask(1000, "ScanForPeds", PedList.ScanForPeds, 3,0),
            new TickTask(250, "ProcessQueue", Tasking.ProcessQueue, 3,1),
            new TickTask(150, "PoliceStateTick", Tasking.PoliceStateTick, 3,2),
            new TickTask(50, "SearchModeStopping", SearchModeStopping.Tick, 3,3),
            new TickTask(1000, "ScanforPoliceVehicles", PedList.ScanforPoliceVehicles, 3,4),

            new TickTask(250, "WeaponDropping", WeaponDropping.TIck, 4,0),

            new TickTask(150, "Civilians", Civilians.Tick, 5,0),

            new TickTask(500, "TrafficViolations", TrafficViolations.Tick, 6,0),
            new TickTask(2000, "PlayerLocation", PlayerLocation.Tick, 6,1),
            new TickTask(500, "PersonOfInterest", PersonOfInterest.Tick, 6,2),

            new TickTask(500, "DispatchAudio", DispatchAudio.Tick, 7,0),
            new TickTask(500, "PoliceSpeech", PoliceSpeech.Tick, 7,2),
            new TickTask(500, "PoliceSpawning", PoliceSpawning.Tick, 7,3),
            new TickTask(500, "PoliceSpawning.RemoveCops", PoliceSpawning.RemoveCops, 7,4)
        };

        MainLoop();
        General.Initialize();
    }
    public static void MainLoop()
    {     
        GameFiber.StartNew(delegate
        {
            try
            {
                while (IsRunning)
                {
                    GameStopWatch.Start();
                    foreach(int RunGroup in MyTickTasks.Select(x => x.RunGroup))
                    {
                        TickTask ToRun = MyTickTasks.Where(x => x.RunGroup == RunGroup && x.ShouldRun).OrderBy(x => x.RunOrder).FirstOrDefault();
                        if (ToRun != null)
                        {
                            ToRun.RunTask();
                        }
                        
                    }

                    GameStopWatch.Stop();

                    if (GameStopWatch.ElapsedMilliseconds >= 15)
                        Debugging.WriteToLog("InstantActionTick", string.Format("Tick took {0} ms: {1}", GameStopWatch.ElapsedMilliseconds, GetStatus()));

                    ResetRanItems();

                    GameStopWatch.Reset();
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
    }
    private static void ResetRanItems()
    {
        MyTickTasks.ForEach(x => x.RanThisTick = false);
    }
    public static string GetStatus()
    {
        return "Name:RanThisTick:GameTimeLastRan:MissedInterval:"  + string.Join("|",MyTickTasks.Where(x => x.RanThisTick || x.MissedInterval).Select(x => x.DebugName + ":" + x.RanThisTick + ":" + x.GameTimeLastRan + ":" + x.MissedInterval));
    }
}
public class TickTask
{
    public bool RanThisTick = false;
    public uint GameTimeLastRan = 0;
    public uint Interval = 500;
    public uint IntervalMissLength;
    public string DebugName;
    public Action TickToRun;
    public int RunGroup;
    public int RunOrder;
    public TickTask(uint _Interval, string _DebugName, Action _TickToRun, int _RunGroup,int _RunOrder)
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
