using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr
{
    public static class Mod
    {
        public static Police Police { get; private set; } = new Police();
        public static InputManager InputManager { get; private set; } = new InputManager();
        public static CrimeSomething CrimeSomething { get; private set; } = new CrimeSomething();
        public static Player Player { get; private set; } = new Player();
        public static bool IsRunning { get; set; }
        public static void Initialize()
        {
            IsRunning = true;

            while (Game.IsLoading)
                GameFiber.Yield();


            Player.TerminateVanillaRespawn();


            RunTasks();
        }
        public static void RunTasks()
        {
            GameFiber.StartNew(delegate
            {
                try
                {
                    while (IsRunning)
                    {
                        InputManager.Tick();
                        Player.Tick();
                        CrimeSomething.Tick();
                        Police.Tick();
                        
                    }
                }
                catch (Exception e)
                {
                    Dispose();
                    Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
                }
            }, "GameManager");
        }
        public static void Dispose()
        {
            IsRunning = false;


            Player.ActivateVanillaRespawn();






        //    MyTickTasks = new List<TickTask>()
        //{
        //    new TickTask(0, "ClockManager", ClockManager.Tick, 0,0),

        //    new TickTask(0, "InputManager", inputManager.Tick, 1,0),

        //    new TickTask(25, "PlayerStateManager", PlayerStateManager.Tick, 2,0),
        //    new TickTask(25, "PolicePedManager", PoliceManager.Tick, 2,1),

        //    new TickTask(0, "VehicleEngineManager", VehicleEngineManager.Tick, 3,0),

        //    new TickTask(50, "CrimeManager", CrimeManager.Tick, 4,0),
        //    new TickTask(50, "WantedLevelManager", WantedLevelManager.Tick, 4,1),

        //    new TickTask(150, "SearchModeManager", SearchModeManager.Tick, 5,0),
        //    new TickTask(150, "InvestigationManager", InvestigationManager.Tick, 5,1),
        //    new TickTask(150, "SearchModeStoppingManager", SearchModeStoppingManager.Tick, 5,2),
        //    new TickTask(150, "CivilianManager", CivilianManager.Tick, 5,3),

        //    new TickTask(200, "PedDamageManager", PedDamageManager.Tick, 6,0),
        //    new TickTask(250, "MuggingManager", MuggingManager.Tick, 6,1),

        //    new TickTask(1000, "PedManager", PedManager.Tick, 7,0),
        //    new TickTask(1000, "VehicleManager", VehicleManager.Tick, 7,1),
        //    new TickTask(500, "TaskManager.UpdateTaskablePeds", TaskManager.UpdateTaskablePeds, 7,3),
        //    new TickTask(500, "TaskManager.RunActivities", TaskManager.RunActivities, 7,4),

        //    new TickTask(250, "WeaponDroppingManager", WeaponDroppingManager.Tick, 8,0),

        //    new TickTask(500, "TrafficViolationsManager", TrafficViolationsManager.Tick, 9,0),
        //    new TickTask(500, "PlayerLocationManager", PlayerLocationManager.Tick, 9,1),
        //    new TickTask(500, "PersonOfInterestManager", PersonOfInterestManager.Tick, 9,2),

        //    new TickTask(250, "PoliceEquipmentManager", PoliceEquipmentManager.Tick, 10,0),
        //    new TickTask(500, "ScannerManager", ScannerManager.Tick, 10,1),
        //    new TickTask(500, "PoliceSpeechManager", PoliceSpeechManager.Tick, 10,2),
        //    new TickTask(500, "PoliceSpawningManager", PoliceSpawningManager.Tick, 10,3),

        //    new TickTask(500, "DispatchManager.SpawnChecking", DispatchManager.SpawnChecking, 11,0),
        //    new TickTask(500, "DispatchManager.DeleteChecking", DispatchManager.DeleteChecking, 11,1),

        //    new TickTask(100, "VehicleFuelManager", VehicleFuelManager.Tick, 12,0),
        //    new TickTask(100, "VehicleIndicatorManager", VehicleIndicatorManager.Tick, 12,1),
        //    new TickTask(500, "RadioManager", RadioManager.Tick, 12,2),
        //};

        }

    }
}
