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
   // private static List<TickTask> MyTickTasks;
    //private static InputManager inputManager;
    private static UIManager uiManager;
    public static bool IsRunning { get; set; }
    //public static void Initialize()
    //{
    //    IsRunning = true;
    //    Setup();

    //    while (Game.IsLoading)
    //        GameFiber.Yield();

    //    InitializeSubProcesses();
    //    RunTasks();
    //}
    //public static void RunTasks()
    //{
    //    GameFiber.StartNew(delegate
    //    {
    //        try
    //        {
    //            TickID = 0;
    //            while (IsRunning)
    //            {
    //                GameStopWatch.Start();
    //                foreach (int RunGroup in MyTickTasks.GroupBy(x => x.RunGroup).Select(x => x.First()).ToList().Select(x => x.RunGroup))
    //                {
    //                    TickTask ToRun = MyTickTasks.Where(x => x.RunGroup == RunGroup && x.ShouldRun).OrderBy(x => x.MissedInterval ? 0 : 1).OrderBy(x => x.GameTimeLastRan).OrderBy(x => x.RunOrder).FirstOrDefault();//should also check if something has barely ran or 
    //                    if (ToRun != null)
    //                    {
    //                        uint GameTimeStarted = Game.GameTime;
    //                        TickStopWatch.Stop();
    //                        ToRun.RunTask();
    //                        TickStopWatch.Stop();
    //                        //TickTable.Rows.Add(TickID, GameTimeStarted, Game.GameTime, ToRun.DebugName, TickStopWatch.ElapsedMilliseconds);
    //                        TickStopWatch.Reset();
    //                    }
    //                    foreach (TickTask RunningBehind in MyTickTasks.Where(x => x.RunGroup == RunGroup && x.RunningBehind))
    //                    {
    //                        uint GameTimeStarted = Game.GameTime;
    //                        TickStopWatch.Stop();
    //                        RunningBehind.RunTask();
    //                        TickStopWatch.Stop();
    //                       // TickTable.Rows.Add(TickID, GameTimeStarted, Game.GameTime, ToRun.DebugName, TickStopWatch.ElapsedMilliseconds);
    //                        TickStopWatch.Reset();
    //                    }
    //                }

    //                GameStopWatch.Stop();

    //                if (GameStopWatch.ElapsedMilliseconds >= 15)
    //                    Debugging.WriteToLog("InstantActionTick", string.Format("Tick took {0} ms: {1}", GameStopWatch.ElapsedMilliseconds, GetStatus()));

    //                ResetRanItems();

    //                GameStopWatch.Reset();
    //                TickID++;

    //                if(TickTable.Rows.Count >= 100000)//dont let it get too big
    //                {
    //                    TickTable.Rows.Clear();
    //                }
    //                GameFiber.Yield();
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            Dispose();
    //            Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
    //        }
    //    }, "Run Tasks");


    //    GameFiber.Sleep(1000);

    //    //UI & Menu is run on another main gamefiber (Needs to always be ready)
    //    GameFiber.StartNew(delegate
    //    {
    //        try
    //        {
    //            while (IsRunning)
    //            {
    //                MenuManager.Tick();
    //                uiManager.Tick();
    //                GameFiber.Yield();
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            Dispose();
    //            Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
    //        }
    //    }, "Run Menu/UI");
    //}
    //public static void Dispose()
    //{
    //    IsRunning = false;
    //    DisposeSubProcesses();
    //}
    //private static void Setup()
    //{
    //    GameStopWatch = new Stopwatch();
    //    TickStopWatch = new Stopwatch();

    //    TickTable.Columns.Add("TickID");
    //    TickTable.Columns.Add("GameTimeStarted");
    //    TickTable.Columns.Add("GameTimeFinished");
    //    TickTable.Columns.Add("DebugName");
    //    TickTable.Columns.Add("ElapsedMilliseconds");
    //    MyTickTasks = new List<TickTask>()
    //    {
    //       // new TickTask(0, "ClockManager", ClockManager.Tick, 0,0),

    //        new TickTask(0, "InputManager", inputManager.Tick, 1,0),

    //        new TickTask(25, "PlayerStateManager", Mod.Player.Tick, 2,0),
    //        new TickTask(25, "PolicePedManager", PoliceManager.Tick, 2,1),

    //        new TickTask(0, "VehicleEngineManager", VehicleEngineManager.Tick, 3,0),

    //        new TickTask(50, "CrimeManager", CrimeManager.Tick, 4,0),
    //        new TickTask(50, "WantedLevelManager", WantedLevelManager.Tick, 4,1),

    //        new TickTask(150, "SearchModeManager", SearchModeManager.Tick, 5,0),
    //        new TickTask(150, "InvestigationManager", InvestigationManager.Tick, 5,1),
    //        new TickTask(150, "SearchModeStoppingManager", SearchModeStoppingManager.Tick, 5,2),
    //        //new TickTask(150, "CivilianManager", CivilianPerception.Tick, 5,3),

    //        new TickTask(200, "PedDamageManager", PedDamageManager.Tick, 6,0),
    //        new TickTask(250, "MuggingManager", MuggingManager.Tick, 6,1),
            
    //        new TickTask(1000, "PedManager", Mod.PedManager.Tick, 7,0),
    //        new TickTask(1000, "VehicleManager", VehicleManager.Tick, 7,1),
    //        new TickTask(500, "TaskManager.UpdateTaskablePeds", TaskManager.UpdateTaskablePeds, 7,3),
    //        new TickTask(500, "TaskManager.RunActivities", TaskManager.RunActivities, 7,4),

    //        new TickTask(250, "WeaponDroppingManager", WeaponDroppingManager.Tick, 8,0),
       
    //        new TickTask(500, "TrafficViolationsManager", TrafficViolationsManager.Tick, 9,0),
    //        new TickTask(500, "PlayerLocationManager", PlayerLocationManager.Tick, 9,1),
    //        new TickTask(500, "PersonOfInterestManager", PersonOfInterestManager.Tick, 9,2),

    //        new TickTask(250, "PoliceEquipmentManager", PoliceEquipmentManager.Tick, 10,0),
    //        new TickTask(500, "ScannerManager", ScannerManager.Tick, 10,1),
    //        new TickTask(500, "PoliceSpeechManager", PoliceSpeechManager.Tick, 10,2),
    //        new TickTask(500, "PoliceSpawningManager", PoliceSpawningManager.Tick, 10,3),
            
    //        new TickTask(500, "DispatchManager.SpawnChecking", DispatchManager.SpawnChecking, 11,0),
    //        new TickTask(500, "DispatchManager.DeleteChecking", DispatchManager.DeleteChecking, 11,1),

    //        new TickTask(100, "VehicleFuelManager", VehicleFuelManager.Tick, 12,0),
    //        new TickTask(100, "VehicleIndicatorManager", VehicleIndicatorManager.Tick, 12,1),
    //        new TickTask(500, "RadioManager", RadioManager.Tick, 12,2),
    //    };
    //}
    //private static void InitializeSubProcesses()
    //{
    //    //External Data
    //    SettingsManager.Initialize();
    //    AgencyManager.Initialize();
    //    ZoneManager.Initialize();
    //    ZoneJurisdictionManager.Initialize();
    //    CountyJurisdictionManager.Initialize();
    //    LocationManager.Initialize();
    //    PlateTypeManager.Initialize();
    //    NameManager.Initialize();
    //    WeaponManager.Initialize();
    //    StreetManager.Initialize();



    //    inputManager = new InputManager();
    //    uiManager = new UIManager();    
    //    MenuManager.Intitialize();
        

    //    Mod.Player.Initialize();
    //    WeaponDroppingManager.Initialize();
    //    PlayerLocationManager.Initialize();

    //    //ClockManager.Initialize();
    //    BlipManager.Initialize();

    //    VehicleEngineManager.Initialize();
    //    VehicleIndicatorManager.Initialize();
    //    RadioManager.Initialize();
    //    VehicleFuelManager.Initialize();

        
    //    InvestigationManager.Initialize();
    //    WantedLevelManager.Initialize();
    //    SearchModeManager.Initialize();
    //    CrimeManager.Initialize();
    //    PoliceSpawningManager.Initialize();
    //    ScannerManager.Initialize();
    //    PoliceSpeechManager.Initialize();
    //    PoliceEquipmentManager.Initialize();
    //    TrafficViolationsManager.Initialize();
    //    SearchModeStoppingManager.Initialize();
    //    PersonOfInterestManager.Initialize();   
    //    DispatchManager.Initialize();

        

    //    TaskManager.Initialize();
    //    PoliceManager.Initialize();
    //    MuggingManager.Initialize();
    //    PedDamageManager.Initialize();
    //    Mod.PedManager.Initialize();
    //    VehicleManager.Initialize();

    //    LicensePlateTheftManager.Initialize();//Event
    //    PedSwapManager.Initialize();//Event

    //    Debugging.Initialize();//debugging
    //}
    //private static void DisposeSubProcesses()
    //{
    //    Mod.Player.Dispose();
    //    inputManager.Dispose();
    //    MenuManager.Dispose();
    //    Mod.PedManager.Dispose();
    //    VehicleManager.Dispose();
    //    ScannerManager.Dispose();
    //    PoliceSpeechManager.Dispose();
    //    VehicleEngineManager.Dispose();
    //    VehicleIndicatorManager.Dispose();
    //    RadioManager.Dispose();
    //    VehicleFuelManager.Dispose();
    //    //SmokingManager.Dispose();
    //    TaskManager.Dispose();
    //    PoliceEquipmentManager.Dispose();
    //    WeaponDroppingManager.Dispose();
    //    uiManager.Dispose();
    //    Debugging.Dispose();
    //    PlayerLocationManager.Dispose();
    //    BlipManager.Dispose();
    //    PoliceManager.Dispose();
    //    InvestigationManager.Dispose();
    //    WantedLevelManager.Dispose();
    //    SearchModeManager.Dispose();
    //    CrimeManager.Dispose();
    //    PoliceSpawningManager.Dispose();
    //    TrafficViolationsManager.Dispose();
    //    SearchModeStoppingManager.Dispose();
    //    PersonOfInterestManager.Dispose();
    //    //CivilianPerception.Dispose();
    //    //ClockManager.Dispose();
    //    MuggingManager.Dispose();
    //    PedDamageManager.Dispose();
    //    DispatchManager.Dispose();
    //}
    //private static void ResetRanItems()
    //{
    //    MyTickTasks.ForEach(x => x.RanThisTick = false);
    //}
    //public static string GetStatus()
    //{
    //    return string.Join("|", MyTickTasks.Where(x => x.RanThisTick).Select(x => x.DebugName));
    //}
    //public static void OutputTable()
    //{
    //    StringBuilder sb = new StringBuilder();

    //    IEnumerable<string> columnNames = TickTable.Columns.Cast<DataColumn>().
    //                                      Select(column => column.ColumnName);
    //    sb.AppendLine(string.Join(",", columnNames));

    //    foreach (DataRow row in TickTable.Rows)
    //    {
    //        IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
    //        sb.AppendLine(string.Join(",", fields));
    //    }

    //    TickTable.Rows.Clear();

    //    File.WriteAllText("TickTable.csv", sb.ToString());
    //}
    //private class TickTask
    //{
    //    public bool RanThisTick = false;
    //    public uint GameTimeLastRan = 0;
    //    public uint Interval = 500;
    //    public uint IntervalMissLength;
    //    public string DebugName;
    //    public Action TickToRun;
    //    public int RunGroup;
    //    public int RunOrder;
    //    public TickTask(uint _Interval, string _DebugName, Action _TickToRun, int _RunGroup, int _RunOrder)
    //    {
    //        GameTimeLastRan = 0;
    //        Interval = _Interval;
    //        IntervalMissLength = Interval * 2;
    //        DebugName = _DebugName;
    //        TickToRun = _TickToRun;
    //        RunGroup = _RunGroup;
    //        RunOrder = _RunOrder;
    //    }
    //    public bool ShouldRun
    //    {
    //        get
    //        {
    //            if (GameTimeLastRan == 0)
    //                return true;
    //            else if (Game.GameTime - GameTimeLastRan > Interval)
    //                return true;
    //            else
    //                return false;
    //        }
    //    }
    //    public bool MissedInterval
    //    {
    //        get
    //        {
    //            if (Interval == 0)
    //                return false;
    //            //if (GameTimeLastRan == 0)
    //            //    return true;
    //            else if (Game.GameTime - GameTimeLastRan >= IntervalMissLength)
    //                return true;
    //            else
    //                return false;
    //        }
    //    }
    //    public bool RunningBehind
    //    {
    //        get
    //        {
    //            if (Interval == 0)
    //                return false;
    //            //if (GameTimeLastRan == 0)
    //            //    return true;
    //            else if (Game.GameTime - GameTimeLastRan >= (IntervalMissLength * 2))
    //                return true;
    //            else
    //                return false;
    //        }
    //    }
    //    public void RunTask()
    //    {
    //        TickToRun();
    //        GameTimeLastRan = Game.GameTime;
    //        RanThisTick = true;
    //    }

    //}
}