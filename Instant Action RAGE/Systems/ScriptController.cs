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

    private static uint LOSInterval;
    private static uint GameTimeCheckedLOS;

    private static uint PoliceScanningInterval;
    private static uint GameTimeLastScannedForPolice;

    private static uint RandomCopInterval;
    private static uint GameTimeLastSpawnedCop;

    private static uint CleanupCopInterval;
    private static uint GameTimeLastCleanedUpCops;

    private static uint ProcessTaskQueueInterval;
    private static uint GameTimeLastProcessedTaskQueue;

    private static uint PoliceStateInterval;
    private static uint GameTimeLastPoliceState;

    private static uint TrafficViolationsInterval;
    private static uint GameTimeLastCheckedTrafficViolation;

    private static uint PlayerLocationInterval;
    private static uint GameTimeLastUpdatedPlayerLocation;

    private static uint PersonOfInterestInterval;
    private static uint GameTimeLastCheckedPersonOfInterest;

    private static uint PoliceSpeechInterval;
    private static uint GameTimeLastPoliceSpeech;

    private static uint SearchModeStoppingInterval;
    private static uint GameTimeLastStoppedSearchMode;

    private static uint DispatchAudioInterval;
    private static uint GameTimeLastCheckedDispatchAudio;

    private static uint WeaponDroppingInterval;
    private static uint GameTimeCheckedWeaponDropping;

    private static uint WeatherCheckingInterval;
    private static uint GameTimeLastCheckedWeather;

    private static uint InstantActionInterval;
    private static uint GameTimeLastRanInstantAction;

    private static uint PoliceInterval;
    private static uint GameTimeLastRanPolice;

    private static bool RanLOS;
    private static bool ScannedForPolice;
    private static bool SpawnedRandomCop;
    private static bool CleanedUpCops;
    private static bool ProcessedTaskQueue;
    private static bool UpdatedPoliceState;
    private static bool RanSearchModeStopping;
    private static bool RanTrafficViolation;
    private static bool UpdatedPlayerLocation;
    private static bool CheckedPersonOfInterest;
    private static bool PoliceSpeechRan;
    private static bool CheckedDispatchAudio;
    private static bool RanWeaponDropping;
    private static bool CheckedWeather;
    private static bool RanVehicleEngine;

    private static bool RanInstantAction;
    private static bool RanPolice;
    public static bool IsRunning { get; set; }
    public enum RunSeverity
    {
        Optional = 0,
        Low = 1,
        Medium = 2,
        High = 3,
        Necessary = 5,
    }

    public static void Initialize()
    {    
        IsRunning = true;

        LOSInterval = 500;
        GameTimeCheckedLOS = 0;

        PoliceScanningInterval = 5000;
        GameTimeLastScannedForPolice = 0;

        CleanupCopInterval = 5000;
        GameTimeLastCleanedUpCops = 0;

        RandomCopInterval = 500;
        GameTimeLastSpawnedCop = 0;

        ProcessTaskQueueInterval = 50;
        GameTimeLastProcessedTaskQueue = 0;

        PoliceStateInterval = 50;
        GameTimeLastPoliceState = 0;

        TrafficViolationsInterval = 500;
        GameTimeLastCheckedTrafficViolation = 0;

        PlayerLocationInterval = 2000;
        GameTimeLastUpdatedPlayerLocation = 0;

        PersonOfInterestInterval = 500;
        GameTimeLastCheckedPersonOfInterest = 0;

        TrafficViolationsInterval = 500;
        GameTimeLastCheckedTrafficViolation = 0;

        PoliceSpeechInterval = 500;
        GameTimeLastPoliceSpeech = 0;

        SearchModeStoppingInterval = 50;
        GameTimeLastStoppedSearchMode = 0;

        DispatchAudioInterval = 500;
        GameTimeLastCheckedDispatchAudio = 0;

        WeaponDroppingInterval = 100;
        GameTimeCheckedWeaponDropping = 0;

        WeatherCheckingInterval = 5000;
        GameTimeLastCheckedWeather = 0;

        InstantActionInterval = 25;
        GameTimeLastRanInstantAction = 0;

        PoliceInterval = 25;
        GameTimeLastRanPolice = 0;


        RanInstantAction = false;
        RanPolice = false;

        RanLOS = false;
        ScannedForPolice = false;
        SpawnedRandomCop = false;
        CleanedUpCops = false;
        ProcessedTaskQueue = false;
        UpdatedPoliceState = false;
        RanSearchModeStopping = false;
        RanTrafficViolation = false;
        UpdatedPlayerLocation = false;
        CheckedPersonOfInterest = false;
        PoliceSpeechRan = false;
        CheckedDispatchAudio = false;
        RanWeaponDropping = false;
        CheckedWeather = false;
        RanVehicleEngine = false;


        GameStopWatch = new Stopwatch();

        MainLoop();
    }
    public static bool CanRunLowPriority
    {
        get
        {
            if (!RanLOS && !ScannedForPolice && !ProcessedTaskQueue && !UpdatedPoliceState && !RanTrafficViolation && !UpdatedPlayerLocation && !CheckedPersonOfInterest && !RanSearchModeStopping)
                return true;
            else
                return false;
        }
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
                    if (InstantAction.IsRunning && Game.GameTime - GameTimeLastRanInstantAction > InstantActionInterval)
                    {
                        RanInstantAction = true;
                        InstantAction.UpdatePlayer();
                        InstantAction.StateTick();
                        InstantAction.ControlTick();
                        InstantAction.AudioTick();
                        GameTimeLastRanInstantAction = Game.GameTime;
                    }
                    else if (Police.IsRunning && Game.GameTime - GameTimeLastRanPolice > PoliceInterval)
                    {
                        RanPolice = true;
                        Police.Tick();
                        GameTimeLastRanPolice = Game.GameTime;
                    }


                    if (Tasking.IsRunning && InstantAction.PlayerIsWanted)//Dont need to do this each tick if we arent wanted?
                    {
                        Tasking.PoliceVehicleTick();
                    }
                    if (VehicleEngine.IsRunning)
                    {
                        VehicleEngine.VehicleEngineTick();
                    }
                    //were okay about 5 fps




                    if (Police.IsRunning && Game.GameTime > GameTimeLastScannedForPolice + PoliceScanningInterval)
                    {
                        ScannedForPolice = true;
                        PoliceScanning.ScanForPolice();
                        GameTimeLastScannedForPolice = Game.GameTime;
                    }
                    else if (Police.IsRunning && Game.GameTime > GameTimeCheckedLOS + LOSInterval)
                    {
                        RanLOS = true;
                        Police.CheckLOS((Game.LocalPlayer.Character.IsInAnyVehicle(false)) ? (Entity)Game.LocalPlayer.Character.CurrentVehicle : (Entity)Game.LocalPlayer.Character);
                        Police.SetPrimaryPursuer();
                        GameTimeCheckedLOS = Game.GameTime;
                    }

                    //Run opposite times
                    if (Tasking.IsRunning && Game.GameTime > GameTimeLastProcessedTaskQueue + ProcessTaskQueueInterval)
                    {
                        ProcessedTaskQueue = true;
                        Tasking.ProcessQueue();//50 ms
                        GameTimeLastProcessedTaskQueue = Game.GameTime;
                    }
                    else if (Tasking.IsRunning && Game.GameTime > GameTimeLastPoliceState + PoliceStateInterval)
                    {
                        UpdatedPoliceState = true;
                        Tasking.PoliceStateTick();//50 ms
                        GameTimeLastPoliceState = Game.GameTime;
                    }

                    if (SearchModeStopping.IsRunning && Game.GameTime > GameTimeLastStoppedSearchMode + SearchModeStoppingInterval)
                    {
                        RanSearchModeStopping = true;
                        SearchModeStopping.StopPoliceSearchMode();
                        GameTimeLastStoppedSearchMode = Game.GameTime;
                    }

                    if (WeaponDropping.IsRunning && Game.GameTime > GameTimeCheckedWeaponDropping + WeaponDroppingInterval)
                    {
                        RanWeaponDropping = true;
                        WeaponDropping.WeaponDroppingTick();
                        GameTimeCheckedWeaponDropping = Game.GameTime;
                    }

                    //Needed eventually
                    if (TrafficViolations.IsRunning && Game.GameTime > GameTimeLastCheckedTrafficViolation + TrafficViolationsInterval)
                    {
                        RanTrafficViolation = true;
                        TrafficViolations.CheckViolations();
                        GameTimeLastCheckedTrafficViolation = Game.GameTime;
                    }
                    else if (PlayerLocation.IsRunning && Game.GameTime > GameTimeLastUpdatedPlayerLocation + PlayerLocationInterval)
                    {
                        UpdatedPlayerLocation = true;
                        PlayerLocation.UpdateLocation();
                        GameTimeLastUpdatedPlayerLocation = Game.GameTime;
                    }
                    else if (PersonOfInterest.IsRunning && Game.GameTime > GameTimeLastCheckedPersonOfInterest + PersonOfInterestInterval)
                    {
                        CheckedPersonOfInterest = true;
                        PersonOfInterest.PersonOfInterestTick();
                        GameTimeLastCheckedPersonOfInterest = Game.GameTime;
                    }



                    //Low priority
                    if (CanRunLowPriority && GameStopWatch.ElapsedMilliseconds <= 10)
                    {
                        //Audio Reporting
                        if (DispatchAudio.IsRunning && Game.GameTime > GameTimeLastCheckedDispatchAudio + DispatchAudioInterval)
                        {
                            CheckedDispatchAudio = true;
                            DispatchAudio.PlayDispatchQueue();
                            GameTimeLastCheckedDispatchAudio = Game.GameTime;
                        }
                        else if (WeatherReporting.IsRunning && Game.GameTime - GameTimeLastCheckedWeather > 5000)
                        {
                            CheckedWeather = true;
                            WeatherReporting.CheckWeather();
                            GameTimeLastCheckedWeather = Game.GameTime;
                        }

                        if (PoliceSpeech.IsRunning && Game.GameTime > GameTimeLastPoliceSpeech + PoliceSpeechInterval)
                        {
                            PoliceSpeechRan = true;
                            PoliceSpeech.CheckSpeech();
                            GameTimeLastPoliceSpeech = Game.GameTime;
                        }

                        if (InstantAction.PlayerIsNotWanted || Police.PlayerHasBeenWantedFor >= 15000)
                        {
                            if (Police.IsRunning && Settings.SpawnRandomPolice && Game.GameTime > GameTimeLastSpawnedCop + RandomCopInterval)
                            {
                                SpawnedRandomCop = true;
                                if (PoliceScanning.CopPeds.Where(x => x.WasRandomSpawn).Count() < Settings.SpawnRandomPoliceLimit)
                                {
                                    PoliceSpawning.SpawnRandomCop();
                                }
                                GameTimeLastSpawnedCop = Game.GameTime;
                            }
                            else if (Police.IsRunning && Game.GameTime > GameTimeLastCleanedUpCops + CleanupCopInterval)
                            {
                                CleanedUpCops = true;
                                PoliceSpawning.RemoveFarAwayRandomlySpawnedCops();
                                GameTimeLastCleanedUpCops = Game.GameTime;
                            }
                        }
                    }

                    GameStopWatch.Stop();

                    if (GameStopWatch.ElapsedMilliseconds >= 10)
                        LocalWriteToLog("InstantActionTick", string.Format("Tick took {0} ms: RanLOS {1}, ScannedForPolice {2},SpawnedRandomCop {3},CleanedUpCops {4},ProcessedTaskQueue {5},UpdatedPoliceState {6},RanTrafficViolation {7},UpdatedPlayerLocation {8},CheckedPersonOfInterest {9},PoliceSpeechRan {10},CheckedDispatchAudio {11},RanWeaponDropping {12},CheckedWeather {13},RanVehicleEngine {14},RanSearchModeStopping {15}", 
                                    GameStopWatch.ElapsedMilliseconds, RanLOS, ScannedForPolice,SpawnedRandomCop,CleanedUpCops,ProcessedTaskQueue,UpdatedPoliceState,RanTrafficViolation,UpdatedPlayerLocation,CheckedPersonOfInterest,PoliceSpeechRan, CheckedDispatchAudio, RanWeaponDropping, CheckedWeather, RanVehicleEngine, RanSearchModeStopping));


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
        RanLOS = false;
        ScannedForPolice = false;
        SpawnedRandomCop = false;
        CleanedUpCops = false;
        ProcessedTaskQueue = false;
        UpdatedPoliceState = false;
        RanSearchModeStopping = false;
        RanTrafficViolation = false;
        UpdatedPlayerLocation = false;
        CheckedPersonOfInterest = false;
        PoliceSpeechRan = false;

        CheckedDispatchAudio = false;
        RanWeaponDropping = false;
        CheckedWeather = false;
        RanVehicleEngine = false;

        RanInstantAction = false;
        RanPolice = false;
    }
    private static void LocalWriteToLog(string ProcedureString, string TextToLog)
    {
        if (Settings.GeneralLogging)
            Debugging.WriteToLog(ProcedureString, TextToLog);
    }
    public static bool CanExecute(RunSeverity SeveritySent)
    {
        if (SeveritySent == RunSeverity.Necessary)
            return true;
        else if (GameStopWatch.ElapsedMilliseconds <= 10)
            return true;
        else
            return false;
    }
}

