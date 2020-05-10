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
   // private static TickTask LineOfSightTick;
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
    private static TickTask PedWoundSystemTick;
    private static TickTask VehicleFuelTick;
    private static List<TickTask> MyTickTasks;
 
    public static bool IsRunning { get; set; }


    public static void Initialize()
    {    
        IsRunning = true;


        LosSantosREDTick = new TickTask(25, "InstantActionTick", LosSantosRED.LosSantosREDTick, TickTask.Type.RequiredGeneral);//25
        PoliceTick = new TickTask(25, "PoliceTick", Police.PoliceGeneralTick, TickTask.Type.RequiredGeneral);//25
        VehicleEngineTick = new TickTask(0, "VehicleEngineTick", VehicleEngine.VehicleEngineTick, TickTask.Type.RequiredGeneral);//0
        VehicleFuelTick = new TickTask(200, "VehicleFuelTick", VehicleFuelSystem.FuelTick, TickTask.Type.RequiredGeneral);
        PlayerHealthTick = new TickTask(200, "PlayerHealthTick", PlayerHealth.Tick, TickTask.Type.RequiredGeneral);//50
        PedWoundSystemTick = new TickTask(200, "PedHealthTick", PedWoundSystem.Tick, TickTask.Type.RequiredGeneral);
        MuggingTick = new TickTask(250, "MuggingTick", MuggingSystem.Tick, TickTask.Type.RequiredGeneral);//50
        ClockTick = new TickTask(0, "ClockTick", ClockSystem.ClockTick, TickTask.Type.RequiredGeneral);

        PoliceScanningTick = new TickTask(1000, "PoliceScanningTick", PedList.ScanForPeds, TickTask.Type.Police);
        
        PoliceStateTick = new TickTask(150, "PoliceStateTick", Tasking.PoliceStateTick, TickTask.Type.Police);//50  //does the tasking checking
        ProcessTaskQueueTick = new TickTask(250, "ProcessTaskQueueTick", Tasking.ProcessQueue, TickTask.Type.Police);//250 //goes thru the actual assigning out

        SearchModeStopperTick = new TickTask(500, "SearchModeStopperTick", SearchModeStopping.StopPoliceSearchMode, TickTask.Type.Police);
        PoliceVehicleScanningTick = new TickTask(1000, "PoliceVehicleScanningTick", PedList.ScanforPoliceVehicles, TickTask.Type.Police);

        WeaponDroppingTick = new TickTask(250, "WeaponDroppingTick", WeaponDropping.WeaponDroppingTick, TickTask.Type.RequiredGeneral);//100
        CivilianTick = new TickTask(150, "Civilian", Civilians.CivilianTick, TickTask.Type.RequiredGeneral);//150

        TrafficViolationsTick = new TickTask(500, "TrafficViolationsTick", TrafficViolations.CheckViolations, TickTask.Type.Normal);
        PlayerLocationTick = new TickTask(2000, "PlayerLocationTick", PlayerLocation.UpdateLocation, TickTask.Type.Normal);
        PersonOfInterestTick = new TickTask(500, "PersonOfInterestTick", PersonOfInterest.PersonOfInterestTick, TickTask.Type.Normal);

        DispatchAudioTick = new TickTask(500, "DispatchAudioTick", DispatchAudio.PlayDispatchQueue, TickTask.Type.Optional);
        PoliceSpeechTick = new TickTask(500, "PoliceSpeechTick", PoliceSpeech.CheckSpeech, TickTask.Type.Optional);
        RandomCopSpawningTick = new TickTask(500, "RandomCopSpawningTick", PoliceSpawning.PoliceSpawningTick, TickTask.Type.Optional);
        CleanupCopTick = new TickTask(500, "CleanupCopTick", PoliceSpawning.RemoveCops, TickTask.Type.Optional);

        GameStopWatch = new Stopwatch();

        MyTickTasks = new List<TickTask>()
        {

            LosSantosREDTick,PoliceTick,VehicleEngineTick,VehicleFuelTick,PlayerHealthTick,PedWoundSystemTick,PoliceScanningTick,/*LineOfSightTick,*/ProcessTaskQueueTick,PoliceStateTick,SearchModeStopperTick,PoliceVehicleScanningTick,WeaponDroppingTick
            ,CivilianTick,TrafficViolationsTick,PlayerLocationTick,PersonOfInterestTick,DispatchAudioTick/*,WeatherCheckingTick*/,PoliceSpeechTick,RandomCopSpawningTick,CleanupCopTick,MuggingTick,ClockTick
        };

        MainLoop();
        LosSantosRED.Initialize();
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

                    if (VehicleEngine.IsRunning && VehicleEngineTick.ShouldRun)
                        VehicleEngineTick.RunTask();

                    if (PlayerHealth.IsRunning && PlayerHealthTick.ShouldRun)
                        PlayerHealthTick.RunTask();
                    else if (PedWoundSystem.IsRunning && PedWoundSystemTick.ShouldRun)
                        PedWoundSystemTick.RunTask();
                    else if (MuggingSystem.IsRunning && MuggingTick.ShouldRun)
                        MuggingTick.RunTask();
                    else if (ClockSystem.IsRunning && ClockTick.ShouldRun)
                        ClockTick.RunTask();
                    else if (VehicleFuelSystem.IsRunning && VehicleFuelTick.ShouldRun)
                        VehicleFuelTick.RunTask();

                    //Police Stuff
                    if (Police.IsRunning && PoliceScanningTick.ShouldRun)
                        PoliceScanningTick.RunTask();
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

                        if (GameStopWatch.ElapsedMilliseconds <= 1 || !MyTickTasks.Any(x => x.RanThisTick))
                        {
                            //Least Important
                            if (DispatchAudio.IsRunning && DispatchAudioTick.ShouldRun)
                                DispatchAudioTick.RunTask();
                            else if (PoliceSpeech.IsRunning && PoliceSpeechTick.ShouldRun)//used to be IF
                                PoliceSpeechTick.RunTask();
                            else if (PoliceSpawning.IsRunning && LosSantosRED.MySettings.Police.SpawnRandomPolice && RandomCopSpawningTick.ShouldRun)// used to be IF
                                RandomCopSpawningTick.RunTask();
                            else if (PoliceSpawning.IsRunning && CleanupCopTick.ShouldRun)
                                CleanupCopTick.RunTask();
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
        LosSantosRED.Dispose();
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

