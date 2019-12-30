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
    private static TickTask LineOfSightTick;
    private static TickTask PoliceScanningTick;
    private static TickTask CleanupCopTick;
    private static TickTask RandomCopSpawningTick;
    private static TickTask VehicleEngineTick;
    private static TickTask ProcessTaskQueueTick;
    private static TickTask PoliceStateTick;
    private static TickTask TrafficViolationsTick;
    private static TickTask PlayerLocationTick;
    private static TickTask PersonOfInterestTick;
    private static TickTask PoliceSpeechTick;
    private static TickTask SearchModeStopperTick;
    private static TickTask DispatchAudioTick;
    private static TickTask WeaponDroppingTick;
    private static TickTask WeatherCheckingTick;
    private static TickTask InstantActionTick;
    private static TickTask PoliceTick;
    private static TickTask CivilianTick;
    private static List<TickTask> MyTickTasks;
 
    public static bool IsRunning { get; set; }


    public static void Initialize()
    {    
        IsRunning = true;

        LineOfSightTick = new TickTask(500, "LineOfSightTick");
        PoliceScanningTick = new TickTask(5000, "PoliceScanningTick");
        CleanupCopTick = new TickTask(5000, "CleanupCopTick");
        RandomCopSpawningTick = new TickTask(5000, "RandomCopSpawningTick");//was 500
        VehicleEngineTick = new TickTask(0, "VehicleEngineTick");
        ProcessTaskQueueTick = new TickTask(50, "ProcessTaskQueueTick");
        PoliceStateTick = new TickTask(50, "PoliceStateTick");
        TrafficViolationsTick = new TickTask(500, "TrafficViolationsTick");
        PlayerLocationTick = new TickTask(2000, "PlayerLocationTick");
        PersonOfInterestTick = new TickTask(500, "PersonOfInterestTick");
        PoliceSpeechTick = new TickTask(500, "PoliceSpeechTick");
        SearchModeStopperTick = new TickTask(500, "SearchModeStopperTick");//was 50
        DispatchAudioTick = new TickTask(500, "DispatchAudioTick");
        WeaponDroppingTick = new TickTask(100, "WeaponDroppingTick");
        WeatherCheckingTick = new TickTask(5000, "WeatherCheckingTick");
        InstantActionTick = new TickTask(25, "InstantActionTick");
        PoliceTick = new TickTask(25, "PoliceTick");
        CivilianTick = new TickTask(150, "Civilian");
        GameStopWatch = new Stopwatch();

        MyTickTasks = new List<TickTask>()
        {
            LineOfSightTick,PoliceScanningTick,CleanupCopTick,RandomCopSpawningTick,VehicleEngineTick,ProcessTaskQueueTick
            ,PoliceStateTick,TrafficViolationsTick,PlayerLocationTick,PersonOfInterestTick,PoliceSpeechTick,SearchModeStopperTick,DispatchAudioTick
            ,WeaponDroppingTick,WeatherCheckingTick,InstantActionTick,PoliceTick,CivilianTick
        };

        MainLoop();
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

                    //Required
                    if (InstantAction.IsRunning && InstantActionTick.ShouldRun)
                    {
                        InstantAction.UpdatePlayer();
                        InstantAction.StateTick();
                        InstantAction.ControlTick();
                        InstantAction.AudioTick();

                        InstantActionTick.RanTask();
                    }
                    else if (Police.IsRunning && PoliceTick.ShouldRun)
                    {
                        Police.Tick();
                        PoliceTick.RanTask();
                    }

                    if (VehicleEngine.IsRunning && VehicleEngineTick.ShouldRun)//needs to be separate?
                    {
                        VehicleEngine.VehicleEngineTick();
                        VehicleEngineTick.RanTask();
                    }

                    //Police Stuff
                    if (Police.IsRunning && PoliceScanningTick.ShouldRun)
                    {
                        PoliceScanning.ScanForPolice();
                        PoliceScanningTick.RanTask();
                    }
                    else if (Police.IsRunning && LineOfSightTick.ShouldRun)
                    {
                        Police.CheckLOS((Game.LocalPlayer.Character.IsInAnyVehicle(false)) ? (Entity)Game.LocalPlayer.Character.CurrentVehicle : (Entity)Game.LocalPlayer.Character);
                        Police.SetPrimaryPursuer();
                        LineOfSightTick.RanTask();
                    }
                    else if (Tasking.IsRunning && ProcessTaskQueueTick.ShouldRun)//used to be IF
                    {
                        Tasking.ProcessQueue();
                        ProcessTaskQueueTick.RanTask();
                    }
                    else if (Tasking.IsRunning && PoliceStateTick.ShouldRun)
                    {
                        Tasking.PoliceStateTick();
                        PoliceStateTick.RanTask();
                    }
                    else if (SearchModeStopping.IsRunning && SearchModeStopperTick.ShouldRun)//used to be IF
                    {
                        SearchModeStopping.StopPoliceSearchMode();
                        SearchModeStopperTick.RanTask();
                    }


                    //Weapon dropping kinda important
                    if (WeaponDropping.IsRunning && WeaponDroppingTick.ShouldRun)
                    {
                        WeaponDropping.WeaponDroppingTick();
                        WeaponDroppingTick.RanTask();
                    }
                    //Civilians 
                    if (Civilians.IsRunning && CivilianTick.ShouldRun)
                    {
                        Civilians.CivilianTick();
                        CivilianTick.RanTask();
                    }



                    //Low priority, can happen whenever
                    if (GameStopWatch.ElapsedMilliseconds <= 10)
                    {
                        //Needed eventually, but less important
                        if (TrafficViolations.IsRunning && TrafficViolationsTick.ShouldRun)
                        {
                            TrafficViolations.CheckViolations();
                            TrafficViolationsTick.RanTask();
                        }
                        else if (PlayerLocation.IsRunning && PlayerLocationTick.ShouldRun)
                        {
                            PlayerLocation.UpdateLocation();
                            PlayerLocationTick.RanTask();
                        }
                        else if (PersonOfInterest.IsRunning && PersonOfInterestTick.ShouldRun)
                        {
                            PersonOfInterest.PersonOfInterestTick();
                            PersonOfInterestTick.RanTask();
                        }



                        if (DispatchAudio.IsRunning && DispatchAudioTick.ShouldRun)
                        {
                            DispatchAudio.PlayDispatchQueue();
                            DispatchAudioTick.RanTask();
                        }
                        else if (WeatherReporting.IsRunning && WeatherCheckingTick.ShouldRun)
                        {
                            WeatherReporting.CheckWeather();
                            WeatherCheckingTick.RanTask();
                        }
                        else if (PoliceSpeech.IsRunning && PoliceSpeechTick.ShouldRun)//used to be IF
                        {
                            PoliceSpeech.CheckSpeech();
                            PoliceSpeechTick.RanTask();
                        }
                        else if (Police.IsRunning && Settings.SpawnRandomPolice && RandomCopSpawningTick.ShouldRun)// used to be IF
                        {
                            if (PoliceScanning.CopPeds.Where(x => x.WasRandomSpawn).Count() < Settings.SpawnRandomPoliceLimit)
                            {
                                PoliceSpawning.SpawnRandomCop();
                            }
                            RandomCopSpawningTick.RanTask();
                        }
                        else if (Police.IsRunning && CleanupCopTick.ShouldRun)
                        {
                            PoliceSpawning.RemoveFarAwayRandomlySpawnedCops();
                            CleanupCopTick.RanTask();
                        }
                        
                    }

                    GameStopWatch.Stop();

                    if (GameStopWatch.ElapsedMilliseconds >= 10)
                    {
                        LocalWriteToLog("InstantActionTick", string.Format("Tick took {0} ms: {1}", GameStopWatch.ElapsedMilliseconds, GetStatus()));
                    }


                    if (MyTickTasks.Any(x => x.MissedInterval))
                    {
                        LocalWriteToLog("InstantActionTick", string.Format("Missed Intervals Tick took {0} ms: {1}", GameStopWatch.ElapsedMilliseconds, string.Join(",", MyTickTasks.Where(x => x.MissedInterval).Select(x => x.DebugName + "; RanThisTick:" + x.RanThisTick + ";GameTimeLastRan:" + x.GameTimeLastRan + ";MissedInterval" + x.MissedInterval + Environment.NewLine))));
                    }

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
        InstantAction.Dispose();
    }
    private static void ResetRanItems()
    {
        MyTickTasks.ForEach(x => x.RanThisTick = false);
    }
    public static string GetStatus()
    {
        return "Name:RanThisTick:GameTimeLastRan:MissedInterval" + Environment.NewLine + string.Join(",",MyTickTasks.Select(x => x.DebugName + ":" + x.RanThisTick + ":" + x.GameTimeLastRan + ":" + x.MissedInterval + Environment.NewLine));
    }
    private static void LocalWriteToLog(string ProcedureString, string TextToLog)
    {
        if (Settings.GeneralLogging)
            Debugging.WriteToLog(ProcedureString, TextToLog);
    }
}
public class TickTask
{
    public bool RanThisTick = false;
    public uint GameTimeLastRan = 0;
    public uint Interval = 500;
    public uint IntervalMissLength;
    public RunSeverity OftenToRun;
    public string DebugName;
    public enum RunSeverity
    {
        Optional = 0,
        Low = 1,
        Medium = 2,
        High = 3,
        Required = 5,
    }
    public TickTask(uint _GameTimeLastRan,uint _Interval, string _DebugName)
    {
        GameTimeLastRan = _GameTimeLastRan;
        Interval = _Interval;
        IntervalMissLength = Interval * 2;
        DebugName = _DebugName;
    }
    public TickTask(uint _Interval, string _DebugName)
    {
        GameTimeLastRan = 0;
        Interval = _Interval;
        IntervalMissLength = Interval * 2;
        DebugName = _DebugName;
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
    public void RanTask()
    {
        GameTimeLastRan = Game.GameTime;
        RanThisTick = true;
    }

}

