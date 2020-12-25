using LosSantosRED.lsr.Helper;
using Rage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LosSantosRED.lsr
{
    public class ModController
    {
        private readonly Stopwatch TickStopWatch = new Stopwatch();
        private string LastRanTask;
        private List<ModTask> MyTickTasks;
        private Audio Audio;
        private PedSwap PedSwap;
        private Mod.Player Player;
        private Mod.World World;
        private Scanner Scanner;
        private Input Input;
        private Menu Menu;
        private Police Police;
        private Civilians Civilians;
        private Respawning Respawning;
        private UI UI;
        private Dispatcher Dispatcher;
        private SearchMode SearchMode;
        private Spawner Spawner;
        private Tasking Tasking;
        public ModController()
        {
            Audio = new Audio();
            World = new Mod.World();  
            Player = new Mod.Player(World);
            Input = new Input(Player);
            Police = new Police(World, Player);
            Civilians = new Civilians(World, Player);
            Spawner = new Spawner(World);
            Dispatcher = new Dispatcher(World, Player, Police, Spawner);
            Respawning = new Respawning(World, Player);
            PedSwap = new PedSwap(World, Player);
            SearchMode = new SearchMode(World, Player, Police);
            Tasking = new Tasking(World, Player, Police);
            UI = new UI(World, Player, SearchMode);
            Scanner = new Scanner(World, Player, Police, Audio, Respawning, SearchMode);
            Menu = new Menu(World, Player, PedSwap, Respawning);
        }
        public bool IsRunning { get; private set; }
        public void NewPlayer(string ModelName, bool Male)
        {
            Player.Reset(true, true, true);
            Scanner.Reset();
            Player.GiveName(ModelName, Male);
            if (DataMart.Instance.Settings.SettingsManager.General.PedTakeoverSetRandomMoney)
            {
                Player.SetMoney(RandomItems.MyRand.Next(DataMart.Instance.Settings.SettingsManager.General.PedTakeoverRandomMoneyMin, DataMart.Instance.Settings.SettingsManager.General.PedTakeoverRandomMoneyMax));
            }
        }
        public void Start()
        {
            IsRunning = true;
            while (Game.IsLoading)
            {
                GameFiber.Yield();
            }
            DataMart.Instance.ReadConfig();
            Player.GiveName();
            Player.AddSpareLicensePlate();

            World.AddBlipsToMap();
            PedSwap.StoreInitialVariation();
            SetupModTasks();
            StartGameLogic();
            StartMenuLogic();
            StartDebugLogic();
            Game.DisplayNotification("~s~Los Santos ~r~RED ~s~v0.1 ~n~By ~g~Greskrendtregk ~n~~s~Has Loaded Successfully");
        }
        public void Stop()
        {
            IsRunning = false;
            GameFiber.Sleep(500);
            Player.Dispose();
            World.Dispose();
            PedSwap.Dispose();
            if (DataMart.Instance.Settings.SettingsManager.General.PedTakeoverSetRandomMoney && PedSwap.OriginalMoney > 0)
            {
                Player.SetMoney(PedSwap.OriginalMoney);
            }
        }
        private void SetupModTasks()
        {
            MyTickTasks = new List<ModTask>()
            {
               new ModTask(0, "World.UpdateTime", World.UpdateTime, 0,0),
                new ModTask(0, "Input.Tick", Input.Update, 1,0),
                new ModTask(25, "Player.Update", Player.Update, 2,0),
                new ModTask(100, "World.Police.Tick", Police.Update, 2,1),//25
                new ModTask(200, "Player.Violations.Update", Player.ViolationsUpdate, 3,0),//50
                new ModTask(200, "Player.CurrentPoliceResponse.Update", Player.CurrentPoliceResponse.Update, 3,1),//50
                new ModTask(150, "Player.Investigations.Tick", Player.Investigations.Update, 4,0),
                new ModTask(500, "World.Civilians.Tick", Civilians.Update, 4,1),//150
                new ModTask(250, "Player.MuggingTick", Player.MuggingUpdate, 5,1),
                new ModTask(250, "World.Pedestrians.Prune", World.PrunePedestrians, 6,0),
                new ModTask(1000, "World.Pedestrians.Scan", World.ScaneForPedestrians, 6,1),
                new ModTask(250, "World.Vehicles.CleanLists", World.PruneVehicles, 6,2),
                new ModTask(1000, "World.Vehicles.Scan", World.ScanForVehicles, 6,3),
                new ModTask(500, "Player.Violations.TrafficUpdate", Player.TrafficViolationsUpdate, 8,0),
                new ModTask(500, "Player.CurrentLocation.Update", Player.LocationUpdate, 8,1),
                new ModTask(500, "Player.ArrestWarrant.Update",Player.ArrestWarrantUpdate, 8,2),
                new ModTask(500, "World.Vehicles.Tick", World.VehiclesTick, 9,1),
                new ModTask(150, "Player.SearchMode.UpdateWanted", SearchMode.UpdateWanted, 11,0),
                new ModTask(150, "Player.SearchMode.StopVanillaSearchMode", SearchMode.StopVanilla, 11,1),
                new ModTask(500, "World.Scanner.Tick", Scanner.Tick, 12,0),
                new ModTask(100, "Audio.Tick",Audio.Update,13,0),
                new ModTask(1000, "World.Vehicles.UpdatePlates", World.UpdateVehiclePlates, 13,1),
                new ModTask(500, "World.Tasking.UpdatePeds", Tasking.AddTaskablePeds, 14,0),
                new ModTask(500, "World.Tasking.Tick", Tasking.TaskCops, 14,1),
                new ModTask(750, "World.Tasking.Tick", Tasking.TaskCivilians, 14,2),
                new ModTask(500, "World.Dispatch.DeleteChecking", Dispatcher.Recall, 15,0),
                new ModTask(500, "World.Dispatch.SpawnChecking", Dispatcher.Dispatch, 15,1),
            };
        }
        private void StartDebugLogic()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                        Debug.Instance.Update();
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    Stop();
                    Debug.Instance.WriteToLog("Error", e.Message + " : " + e.StackTrace);
                }
            }, "Run Debug Logic");
        }
        private void StartGameLogic()
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
                                Debug.Instance.WriteToLog("GameLogic", string.Format("Tick took > 16 ms ({0} ms), aborting, Last Ran {1}", TickStopWatch.ElapsedMilliseconds, LastRanTask));
                                break;
                            }

                            ModTask ToRun = MyTickTasks.Where(x => x.RunGroup == RunGroup && x.ShouldRun).OrderBy(x => x.MissedInterval ? 0 : 1).OrderBy(x => x.GameTimeLastRan).OrderBy(x => x.RunOrder).FirstOrDefault();//should also check if something has barely ran or
                            if (ToRun != null)
                            {
                                ToRun.Run();
                                LastRanTask = ToRun.DebugName;
                            }
                            foreach (ModTask RunningBehind in MyTickTasks.Where(x => x.RunGroup == RunGroup && x.RunningBehind))
                            {
                                RunningBehind.Run();
                                LastRanTask = ToRun.DebugName;
                            }
                        }
                        MyTickTasks.ForEach(x => x.RanThisTick = false);

                        TickStopWatch.Reset();
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    Stop();
                    Debug.Instance.WriteToLog("Error", e.Message + " : " + e.StackTrace);
                }
            }, "Run Game Logic");
            GameFiber.Yield();
        }
        private void StartMenuLogic()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                        Menu.Update();
                        UI.Update();
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    Stop();
                    Debug.Instance.WriteToLog("Error", e.Message + " : " + e.StackTrace);
                }
            }, "Run Menu/UI Logic");
        }
    }
}