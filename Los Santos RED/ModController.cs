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
        public bool IsRunning { get; private set; }
        public void NewPlayer(string ModelName, bool Male)
        {
            Mod.Player.Instance.Reset(true, true, true);
            Mod.Player.Instance.GiveName(ModelName, Male);
            if (DataMart.Instance.Settings.SettingsManager.General.PedTakeoverSetRandomMoney)
            {
                Mod.Player.Instance.SetMoney(RandomItems.MyRand.Next(DataMart.Instance.Settings.SettingsManager.General.PedTakeoverRandomMoneyMin, DataMart.Instance.Settings.SettingsManager.General.PedTakeoverRandomMoneyMax));
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
            Mod.Player.Instance.GiveName();
            Mod.Player.Instance.AddSpareLicensePlate();

            Mod.World.Instance.AddBlipsToMap();
            PedSwap.Instance.StoreInitialVariation();

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
            Mod.Player.Instance.Dispose();
            Mod.World.Instance.Dispose();
            PedSwap.Instance.Dispose();
            if (DataMart.Instance.Settings.SettingsManager.General.PedTakeoverSetRandomMoney && PedSwap.Instance.OriginalMoney > 0)
            {
                Mod.Player.Instance.SetMoney(PedSwap.Instance.OriginalMoney);
            }
        }
        private void SetupModTasks()
        {
            MyTickTasks = new List<ModTask>()
            {
               new ModTask(0, "World.UpdateTime", Mod.World.Instance.UpdateTime, 0,0),
                new ModTask(0, "Input.Tick", Input.Instance.Update, 1,0),
                new ModTask(25, "Player.Update", Mod.Player.Instance.Update, 2,0),
                new ModTask(100, "World.Police.Tick", Mod.World.Instance.UpdatePolice, 2,1),//25
                new ModTask(200, "Player.Violations.Update", Mod.Player.Instance.ViolationsUpdate, 3,0),//50
                new ModTask(200, "Player.CurrentPoliceResponse.Update", Mod.Player.Instance.CurrentPoliceResponse.Update, 3,1),//50
                new ModTask(150, "Player.Investigations.Tick", Mod.Player.Instance.Investigations.Update, 4,0),
                new ModTask(500, "World.Civilians.Tick", Mod.World.Instance.UpdateCivilians, 4,1),//150
                new ModTask(250, "Player.MuggingTick", Mod.Player.Instance.MuggingUpdate, 5,1),
                new ModTask(250, "World.Pedestrians.Prune", Mod.World.Instance.PrunePedestrians, 6,0),
                new ModTask(1000, "World.Pedestrians.Scan", Mod.World.Instance.ScaneForPedestrians, 6,1),
                new ModTask(250, "World.Vehicles.CleanLists", Mod.World.Instance.PruneVehicles, 6,2),
                new ModTask(1000, "World.Vehicles.Scan", Mod.World.Instance.ScanForVehicles, 6,3),
                new ModTask(500, "Player.Violations.TrafficUpdate", Mod.Player.Instance.TrafficViolationsUpdate, 8,0),
                new ModTask(500, "Player.CurrentLocation.Update", Mod.Player.Instance.LocationUpdate, 8,1),
                new ModTask(500, "Player.ArrestWarrant.Update", Mod.Player.Instance.ArrestWarrantUpdate, 8,2),
                new ModTask(500, "World.Vehicles.Tick", Mod.World.Instance.VehiclesTick, 9,1),
                new ModTask(150, "Player.SearchMode.UpdateWanted", Mod.Player.Instance.SearchModeUpdate, 11,0),
                new ModTask(150, "Player.SearchMode.StopVanillaSearchMode", Mod.Player.Instance.StopVanillaSearchMode, 11,1),
                new ModTask(500, "World.Scanner.Tick", Mod.World.Instance.UpdateScanner, 12,0),
                new ModTask(100, "Audio.Tick",Audio.Instance.Update,13,0),
                new ModTask(1000, "World.Vehicles.UpdatePlates", Mod.World.Instance.UpdateVehiclePlates, 13,1),
                new ModTask(500, "World.Tasking.UpdatePeds", Mod.World.Instance.AddTaskablePeds, 14,0),
                new ModTask(500, "World.Tasking.Tick", Mod.World.Instance.TaskCops, 14,1),
                new ModTask(750, "World.Tasking.Tick", Mod.World.Instance.TaskCivilians, 14,2),//temp off for testing other stuff, dont need them calling the cops
                new ModTask(500, "World.Dispatch.DeleteChecking", Mod.World.Instance.Recall, 15,0),
                new ModTask(500, "World.Dispatch.SpawnChecking", Mod.World.Instance.Dispatch, 15,1),
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
                        Menu.Instance.Update();
                        UI.Instance.Update();
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