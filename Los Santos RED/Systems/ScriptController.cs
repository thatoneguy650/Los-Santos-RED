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
    private static TickTask PoliceVehicleScanningTick;
    private static TickTask DispatchAudioTick;
    private static TickTask WeaponDroppingTick;
    //private static TickTask WeatherCheckingTick;
    private static TickTask LosSantosREDTick;
    private static TickTask PoliceTick;
    private static TickTask CivilianTick;
    private static TickTask MuggingTick;
    private static TickTask ClockTick;
    private static TickTask PlayerHealthTick;
    private static List<TickTask> MyTickTasks;
 
    public static bool IsRunning { get; set; }


    public static void Initialize()
    {    
        IsRunning = true;


        LosSantosREDTick = new TickTask(25, "InstantActionTick", LosSantosRED.InstantActionTick, TickTask.Type.RequiredGeneral);
        PoliceTick = new TickTask(25, "PoliceTick", Police.PoliceGeneralTick, TickTask.Type.RequiredGeneral);
        VehicleEngineTick = new TickTask(0, "VehicleEngineTick", VehicleEngine.VehicleEngineTick, TickTask.Type.RequiredGeneral);


        PlayerHealthTick = new TickTask(50, "PlayerHealthTick", PlayerHealth.Tick, TickTask.Type.RequiredGeneral);

        MuggingTick = new TickTask(50, "MuggingTick", MuggingSystem.Tick, TickTask.Type.RequiredGeneral);
        ClockTick = new TickTask(0, "ClockTick", ClockSystem.ClockTick, TickTask.Type.RequiredGeneral);


        PoliceScanningTick = new TickTask(1000, "PoliceScanningTick", PedScanning.ScanForPeds, TickTask.Type.Police);//was 5000
        LineOfSightTick = new TickTask(500, "LineOfSightTick",Police.CheckPoliceSight, TickTask.Type.Police);
        ProcessTaskQueueTick = new TickTask(50, "ProcessTaskQueueTick", Tasking.ProcessQueue, TickTask.Type.Police);
        PoliceStateTick = new TickTask(50, "PoliceStateTick", Tasking.PoliceStateTick, TickTask.Type.Police);
        SearchModeStopperTick = new TickTask(500, "SearchModeStopperTick", SearchModeStopping.StopPoliceSearchMode, TickTask.Type.Police);//was 50
        PoliceVehicleScanningTick = new TickTask(1000, "PoliceVehicleScanningTick", PedScanning.ScanforPoliceVehicles, TickTask.Type.Police);//was 5000//was 1500

        WeaponDroppingTick = new TickTask(100, "WeaponDroppingTick", WeaponDropping.WeaponDroppingTick, TickTask.Type.RequiredGeneral);
        CivilianTick = new TickTask(150, "Civilian", Civilians.CivilianTick, TickTask.Type.RequiredGeneral);

        TrafficViolationsTick = new TickTask(500, "TrafficViolationsTick", TrafficViolations.CheckViolations, TickTask.Type.Normal);
        PlayerLocationTick = new TickTask(2000, "PlayerLocationTick", PlayerLocation.UpdateLocation, TickTask.Type.Normal);
        PersonOfInterestTick = new TickTask(500, "PersonOfInterestTick", PersonOfInterest.PersonOfInterestTick, TickTask.Type.Normal);

        DispatchAudioTick = new TickTask(500, "DispatchAudioTick", DispatchAudio.PlayDispatchQueue, TickTask.Type.Optional);
        //WeatherCheckingTick = new TickTask(5000, "WeatherCheckingTick", WeatherReporting.CheckWeather, TickTask.Type.Optional);
        PoliceSpeechTick = new TickTask(500, "PoliceSpeechTick", PoliceSpeech.CheckSpeech, TickTask.Type.Optional);
        RandomCopSpawningTick = new TickTask(500, "RandomCopSpawningTick", PoliceSpawning.RandomCopTick, TickTask.Type.Optional);//was 500//was 3000
        CleanupCopTick = new TickTask(5000, "CleanupCopTick", PoliceSpawning.RemoveFarAwayRandomlySpawnedCops, TickTask.Type.Optional);

        GameStopWatch = new Stopwatch();

        MyTickTasks = new List<TickTask>()
        {

            LosSantosREDTick,PoliceTick,VehicleEngineTick,PlayerHealthTick,PoliceScanningTick,LineOfSightTick,ProcessTaskQueueTick,PoliceStateTick,SearchModeStopperTick,PoliceVehicleScanningTick,WeaponDroppingTick
            ,CivilianTick,TrafficViolationsTick,PlayerLocationTick,PersonOfInterestTick,DispatchAudioTick/*,WeatherCheckingTick*/,PoliceSpeechTick,RandomCopSpawningTick,CleanupCopTick,MuggingTick,ClockTick
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
                    if (LosSantosRED.IsRunning && LosSantosREDTick.ShouldRun)
                        LosSantosREDTick.RunTask();
                    else if (Police.IsRunning && PoliceTick.ShouldRun)
                        PoliceTick.RunTask();

                    if (PlayerHealth.IsRunning && PlayerHealthTick.ShouldRun)//needs to be separate?
                        PlayerHealthTick.RunTask();

                    if (VehicleEngine.IsRunning && VehicleEngineTick.ShouldRun)//needs to be separate?
                        VehicleEngineTick.RunTask();

                    if (MuggingSystem.IsRunning && MuggingTick.ShouldRun)//needs to be separate?
                        MuggingTick.RunTask();

                    if (ClockSystem.IsRunning && ClockTick.ShouldRun)//needs to be separate?
                        ClockTick.RunTask();

                    //Police Stuff
                    if (Police.IsRunning && PoliceScanningTick.ShouldRun)
                        PoliceScanningTick.RunTask();
                    else if (Police.IsRunning && LineOfSightTick.ShouldRun)
                        LineOfSightTick.RunTask();
                    else if (Tasking.IsRunning && ProcessTaskQueueTick.ShouldRun)//used to be IF
                        ProcessTaskQueueTick.RunTask();
                    else if (Tasking.IsRunning && PoliceStateTick.ShouldRun)
                        PoliceStateTick.RunTask();
                    else if (SearchModeStopping.IsRunning && SearchModeStopperTick.ShouldRun)//used to be IF
                        SearchModeStopperTick.RunTask();
                    else if (Police.IsRunning && PoliceVehicleScanningTick.ShouldRun)
                        PoliceVehicleScanningTick.RunTask();

                    //Weapon dropping kinda important
                    if (WeaponDropping.IsRunning && WeaponDroppingTick.ShouldRun)
                        WeaponDroppingTick.RunTask();

                    //Civilians 
                    if (Civilians.IsRunning && CivilianTick.ShouldRun)
                        CivilianTick.RunTask();

                    //Low priority, can happen whenever
                    if (GameStopWatch.ElapsedMilliseconds <= 10 && !MyTickTasks.Any(x => x.RunGroup == TickTask.Type.Police && x.RanThisTick))
                    {
                        //Needed eventually, but less important
                        if (TrafficViolations.IsRunning && TrafficViolationsTick.ShouldRun)
                            TrafficViolationsTick.RunTask();
                        else if (PlayerLocation.IsRunning && PlayerLocationTick.ShouldRun)
                            PlayerLocationTick.RunTask();
                        else if (PersonOfInterest.IsRunning && PersonOfInterestTick.ShouldRun)
                            PersonOfInterestTick.RunTask();

                        if (GameStopWatch.ElapsedMilliseconds <= 2 || !MyTickTasks.Any(x => x.RanThisTick))
                        {
                            //Least Important
                            if (DispatchAudio.IsRunning && DispatchAudioTick.ShouldRun)
                                DispatchAudioTick.RunTask();
                            //else if (WeatherReporting.IsRunning && WeatherCheckingTick.ShouldRun)
                            //    WeatherCheckingTick.RunTask();
                            else if (PoliceSpeech.IsRunning && PoliceSpeechTick.ShouldRun)//used to be IF
                                PoliceSpeechTick.RunTask();
                            else if (Police.IsRunning && LosSantosRED.MySettings.Police.SpawnRandomPolice && RandomCopSpawningTick.ShouldRun)// used to be IF
                                RandomCopSpawningTick.RunTask();
                            else if (Police.IsRunning && CleanupCopTick.ShouldRun)
                                CleanupCopTick.RunTask();
                        }
                    }

                    GameStopWatch.Stop();

                    if (GameStopWatch.ElapsedMilliseconds >= 20)
                        Debugging.WriteToLog("InstantActionTick", string.Format("Tick took {0} ms: {1}", GameStopWatch.ElapsedMilliseconds, ""));//GetStatus()

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
        LosSantosRED.Dispose();
    }
    private static void ResetRanItems()
    {
        MyTickTasks.ForEach(x => x.RanThisTick = false);
    }
    public static string GetStatus()
    {
        return "Name:RanThisTick:GameTimeLastRan:MissedInterval" + Environment.NewLine + string.Join(",",MyTickTasks.Where(x => x.RanThisTick || x.MissedInterval).Select(x => x.DebugName + ":" + x.RanThisTick + ":" + x.GameTimeLastRan + ":" + x.MissedInterval + Environment.NewLine));
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
    public Type RunGroup;
    public enum Type
    {
        RequiredGeneral = 0,
        Police = 1,
        Normal = 2,
        Optional = 3,
    }
    public TickTask(uint _Interval, string _DebugName, Action _TickToRun, Type _RunGroup)
    {
        GameTimeLastRan = 0;
        Interval = _Interval;
        IntervalMissLength = Interval * 2;
        DebugName = _DebugName;
        TickToRun = _TickToRun;
        RunGroup = _RunGroup;
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

