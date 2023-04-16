//using ExtensionsMethods;
//using LosSantosRED.lsr.Helper;
//using LosSantosRED.lsr.Interface;
//using LSR.Vehicles;
//using Rage;
//using Rage.Native;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//public class GangIdle_Old : ComplexTask
//{
//    private bool NeedsUpdates;
//    private Task CurrentTask = Task.Nothing;
//    private uint GameTimeClearedIdle;
//    private int SeatTryingToEnter;
//    private VehicleExt VehicleTryingToEnter;
//    private IEntityProvideable World;
//    private Vehicle VehicleTaskedToEnter;
//    private int SeatTaskedToEnter;
//    private IPlacesOfInterest PlacesOfInterest;
//    private Vector3 taskedPosition;
//    private uint GameTimeBetweenScenarios;
//    private uint GameTimeLastStartedScenario;
//    private uint GameTimeLastChangedWanderStuff;
//    private uint GameTimeBetweenWanderDecision = 60000;
//    private uint GameTimeLastExitedVehicle;


//    private bool ForceGuard = false;
//    private bool hasBeenVehiclePatrolTasked;
//    private uint GameTimeLastStartedFootPatrol;
//    private uint GameTimeBetweenFootPatrols;

//    private bool RecentlyExitedVehicle => GameTimeLastExitedVehicle != 0 && Game.GameTime - GameTimeLastExitedVehicle >= 1000;

//    private enum Task
//    {
//        GetInCar,
//        Wander,
//        Nothing,
//        OtherTarget,
//        VehiclePatrol,
//        GuardArea,
//        FootPatrol,
//    }
//    private Task CurrentTaskDynamic
//    {
//        get
//        {

//            if (ForceGuard)
//            {
//                if (Ped.Pedestrian.IsInAnyVehicle(false))
//                {
//                    return Task.VehiclePatrol;
//                }
//                else
//                {
//                    return Task.GuardArea;
//                }
//            }
//            else if (Ped.Pedestrian.IsInAnyVehicle(false))
//            {
//                return Task.VehiclePatrol;
//            }
//            else
//            {
//                return Task.FootPatrol;
//            }
//        }
//    }
//    public GangIdle_Old(IComplexTaskable cop, ITargetable player, IEntityProvideable world, IPlacesOfInterest placesOfInterest) : base(player, cop, 1500)//1500
//    {
//        Name = "GangIdle";
//        SubTaskName = "";
//        World = world;
//        PlacesOfInterest = placesOfInterest;
//    }
//    public override void Start()
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            if (Ped.IsLocationSpawned)
//            {
//                ForceGuard = true;
//            }
//            else
//            {
//                ForceGuard = false;
//            }


//            ClearTasks(true);
//            Update();
//        }
//    }
//    public override void Update()
//    {
//        if (Ped.Pedestrian.Exists() && ShouldUpdate)
//        {
//            if (CurrentTask != CurrentTaskDynamic)
//            {
//                CurrentTask = CurrentTaskDynamic;
//                ExecuteCurrentSubTask(true);
//            }
//            else if (NeedsUpdates)
//            {
//                ExecuteCurrentSubTask(false);
//            }
//        }
//    }
//    public override void Stop()
//    {

//    }
//    public override void ReTask()
//    {

//    }
//    private void ExecuteCurrentSubTask(bool IsFirstRun)
//    {
//        if (CurrentTask == Task.Wander)
//        {
//            RunInterval = 1500;
//            SubTaskName = "Wander";
//            Wander(IsFirstRun);
//        }
//        else if (CurrentTask == Task.VehiclePatrol)
//        {
//            RunInterval = 1500;
//            SubTaskName = "VehiclePatrol";
//            VehiclePatrol(IsFirstRun);
//        }
//        else if (CurrentTask == Task.GuardArea)
//        {
//            RunInterval = 1500;
//            SubTaskName = "GuardArea";
//            GuardArea(IsFirstRun);
//        }

//        else if (CurrentTask == Task.GetInCar)
//        {
//            RunInterval = 500;
//            SubTaskName = "GetInCar";
//            GetInCar(IsFirstRun);
//        }
//        else if (CurrentTask == Task.Nothing)
//        {
//            RunInterval = 1500;
//            SubTaskName = "Nothing";
//            Nothing(IsFirstRun);
//        }
//        else if (CurrentTask == Task.FootPatrol)
//        {
//            RunInterval = 1500;
//            SubTaskName = "FootPatrol";
//            FootPatrol(IsFirstRun);
//        }
//        GameTimeLastRan = Game.GameTime;
//    }
//    private void Wander(bool IsFirstRun)
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            if (IsFirstRun)
//            {
//                NeedsUpdates = true;
//                ClearTasks(true);
//                WanderTask();
//            }
//            else if (!RecentlyExitedVehicle && Ped.Pedestrian.IsInAnyVehicle(false) && Ped.Pedestrian.CurrentVehicle.Exists() && Player.IsInVehicle && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() && Player.CurrentVehicle.Vehicle.Handle == Ped.Pedestrian.CurrentVehicle.Handle && !Ped.IsDriver && Ped.Pedestrian.CurrentVehicle.Exists() && Ped.Pedestrian.CurrentVehicle.Speed <= 0.2f)
//            {
//                ExitVehicleTask();
//            }
//            else if (Ped.DistanceToPlayer <= 150f && Ped.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask)//might be a crash cause?, is there a regular native for this?
//            {
//                WanderTask();
//                //EntryPoint.WriteToConsole($"COP EVENT: Wander Idle Reset: {Ped.Pedestrian.Handle}", 3);
//            }
//            else if (GameTimeLastChangedWanderStuff != 0 && Game.GameTime - GameTimeLastChangedWanderStuff >= GameTimeBetweenWanderDecision && !Ped.IsInVehicle)
//            {
//                WanderDecisionTask();
//            }
//        }
//    }


//    private void VehiclePatrol(bool IsFirstRun)
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            if (IsFirstRun)
//            {
//                NeedsUpdates = true;
//                ClearTasks(true);
//                VehiclePatrolTask();
//            }
//            if (!hasBeenVehiclePatrolTasked)
//            {
//                VehiclePatrolTask();
//            }
//        }
//    }
//    private void VehiclePatrolTask()
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            //Ped.Pedestrian.BlockPermanentEvents = true;
//            //Ped.Pedestrian.KeepTasks = true;
//            if ((Ped.IsDriver || Ped.Pedestrian.SeatIndex == -1) && Ped.Pedestrian.CurrentVehicle.Exists())
//            {
//                hasBeenVehiclePatrolTasked = true;
//                if (Ped.IsInHelicopter)
//                {
//                    NativeFunction.CallByName<bool>("TASK_HELI_MISSION", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, 0, 0f, 0f, 300f, 9, 50f, 150f, -1f, -1, 30, -1.0f, 0);
//                }
//                else
//                {
//                    unsafe
//                    {
//                        int lol = 0;
//                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
//                        //NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(1000, 3000));
//                        NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, Ped.Pedestrian.CurrentVehicle, 10f, (int)eCustomDrivingStyles.RegularDriving, 10f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, Ped.Pedestrian.CurrentVehicle, 10f, (int)(VehicleDrivingFlags.FollowTraffic | VehicleDrivingFlags.YieldToCrossingPedestrians | VehicleDrivingFlags.RespectIntersections | (VehicleDrivingFlags)8), 10f);
//                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
//                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
//                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
//                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
//                    }
//                }

//            }
//        }
//    }


//    private void GuardArea(bool IsFirstRun)
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            if (IsFirstRun)
//            {
//                NeedsUpdates = true;
//                ClearTasks(true);
//                GuardAreaTask();
//            }
//            else
//            {
//                if (GameTimeLastStartedScenario > 0 && Game.GameTime - GameTimeLastStartedScenario >= GameTimeBetweenScenarios)
//                {
//                    if (RandomItems.RandomPercent(10f))//10 percent let tham transition to foot patrol people
//                    {
//                        ForceGuard = false;
//                    }
//                    else
//                    {
//                        GuardAreaTask();
//                    }
//                }
//            }
//        }
//    }
//    private void GuardAreaTask()
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            //Ped.Pedestrian.BlockPermanentEvents = true;
//            //Ped.Pedestrian.KeepTasks = true;
//            //List<string> PossibleScenarios = new List<string>() { "WORLD_HUMAN_AA_COFFEE", "WORLD_HUMAN_AA_SMOKE", "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_MOBILE_UPRIGHT", "WORLD_HUMAN_SMOKING" };
//            //string ScenarioChosen = PossibleScenarios.PickRandom();

//            string ScenarioChosen;
//            if (Ped.HasMenu)
//            {
//                ScenarioChosen = new List<string>() { "WORLD_HUMAN_DRUG_DEALER", "WORLD_HUMAN_DRUG_DEALER_HARD" }.PickRandom();
//            }
//            else
//            {
//                ScenarioChosen = new List<string>() { "WORLD_HUMAN_SMOKING", "WORLD_HUMAN_AA_SMOKE", "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_HANG_OUT_STREET", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_DRINKING" }.PickRandom();
//            }



//            NativeFunction.CallByName<bool>("TASK_START_SCENARIO_IN_PLACE", Ped.Pedestrian, ScenarioChosen, 0, true);
//            GameTimeBetweenScenarios = RandomItems.GetRandomNumber(30000, 90000);
//            GameTimeLastStartedScenario = Game.GameTime;
//        }
//    }


//    private void FootPatrol(bool IsFirstRun)
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            if (IsFirstRun)
//            {
//                NeedsUpdates = true;
//                ClearTasks(true);
//                FootPatrolTask();
//            }
//            else
//            {
//                if (GameTimeLastStartedFootPatrol > 0 && Game.GameTime - GameTimeLastStartedFootPatrol >= GameTimeBetweenFootPatrols)
//                {
//                    if (Ped.IsLocationSpawned && RandomItems.RandomPercent(10f))//10 percent let tham transition to foot patrol people
//                    {
//                        ForceGuard = true;
//                    }
//                }
//            }
//        }
//    }
//    private void FootPatrolTask()
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            //Ped.Pedestrian.BlockPermanentEvents = true;
//            //Ped.Pedestrian.KeepTasks = true;
//            NativeFunction.Natives.TASK_WANDER_STANDARD(Ped.Pedestrian, 0, 0);
//            //NativeFunction.Natives.TASK_WANDER_IN_AREA(Ped.Pedestrian, Ped.Pedestrian.Position.X, Ped.Pedestrian.Position.Y, Ped.Pedestrian.Position.Z, 100f, 0f, 0f);
//            GameTimeBetweenFootPatrols = RandomItems.GetRandomNumber(30000, 90000);
//            GameTimeLastStartedFootPatrol = Game.GameTime;
//        }
//    }


//    private void WanderTask()
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            if (Ped.Pedestrian.IsInAnyVehicle(false))
//            {
//                if (Ped.IsDriver && Ped.Pedestrian.CurrentVehicle.Exists())
//                {
//                    unsafe
//                    {
//                        int lol = 0;
//                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
//                        NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, Ped.Pedestrian.CurrentVehicle, 10f, (int)eCustomDrivingStyles.RegularDriving, 10f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, Ped.Pedestrian.CurrentVehicle, 10f, (int)(VehicleDrivingFlags.FollowTraffic | VehicleDrivingFlags.YieldToCrossingPedestrians | VehicleDrivingFlags.RespectIntersections | (VehicleDrivingFlags)8), 10f);
//                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
//                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
//                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
//                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
//                    }
//                }
//            }
//            else
//            {
//                WanderDecisionTask();
//            }
            
//        }
//    }
//    private void ExitVehicleTask()
//    {
//        if (!RecentlyExitedVehicle && Ped.Pedestrian.IsInAnyVehicle(false) && !Ped.IsDriver && Ped.Pedestrian.CurrentVehicle.Exists() && Ped.Pedestrian.CurrentVehicle.Speed <= 0.2f)
//        {
//            NativeFunction.CallByName<bool>("TASK_LEAVE_VEHICLE", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 64);
//            GameTimeLastExitedVehicle = Game.GameTime;
//        }
//    }
//    private void WanderDecisionTask()
//    {
//        Vector3 pedPos = Ped.Pedestrian.Position;
//        float ForceScenarioPercent = 30f;
//        bool ScenarioInArea = NativeFunction.Natives.DOES_SCENARIO_EXIST_IN_AREA<bool>(pedPos.X, pedPos.Y, pedPos.Z, 10f, true);
//        if (ScenarioInArea && (Ped.HasMenu || RandomItems.RandomPercent(ForceScenarioPercent)))
//        {
//            string Scenario;
//            if (Ped.HasMenu)
//            {
//                Scenario = new List<string>() { "WORLD_HUMAN_DRUG_DEALER","WORLD_HUMAN_DRUG_DEALER_HARD" }.PickRandom();
//            }
//            else
//            {
//                Scenario = new List<string>() { "WORLD_HUMAN_SMOKING", "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_HANG_OUT_STREET", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_DRINKING" }.PickRandom();
//            }
//            NativeFunction.Natives.TASK_START_SCENARIO_IN_PLACE(Ped.Pedestrian, Scenario, 0, true);
//            //EntryPoint.WriteToConsole($"PED {Ped.Pedestrian.Handle} Started Scenario FORCED! {Scenario}", 5);
//        }
//        else if (ScenarioInArea)
//        {
//            NativeFunction.Natives.TASK_USE_NEAREST_SCENARIO_TO_COORD(Ped.Pedestrian, pedPos.X, pedPos.Y, pedPos.Z, 15f, 15000);
//            //EntryPoint.WriteToConsole($"PED {Ped.Pedestrian.Handle} Started Scenarion NEARBY", 5);
//        }
//        else
//        {
//            NativeFunction.Natives.TASK_WANDER_STANDARD(Ped.Pedestrian, 0, 0);
//            //EntryPoint.WriteToConsole($"PED {Ped.Pedestrian.Handle} Started Regular wander on foot", 5);
//        }
//        GameTimeBetweenWanderDecision = RandomItems.GetRandomNumber(30000, 80000);
//        GameTimeLastChangedWanderStuff = Game.GameTime;
//    }
//    private void GetInCar(bool IsFirstRun)
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            if (IsFirstRun)
//            {
//                NeedsUpdates = true;
//            }
//            if (VehicleTaskedToEnter == null || !VehicleTaskedToEnter.Exists())
//            {
//                GetInCarTask();
//            }
//            else if (VehicleTryingToEnter != null && VehicleTaskedToEnter.Exists() && !VehicleTaskedToEnter.IsSeatFree(SeatTaskedToEnter) && VehicleTaskedToEnter.GetPedOnSeat(SeatTaskedToEnter).Exists() && VehicleTaskedToEnter.GetPedOnSeat(SeatTaskedToEnter).Handle != Ped.Pedestrian.Handle)// && (VehicleTryingToEnter.Vehicle.Handle != VehicleTaskedToEnter.Handle || SeatTaskedToEnter != SeatTryingToEnter) && Ped.Pedestrian.Exists() && !Ped.Pedestrian.IsInAnyVehicle(true))
//            {
//                GetInCarTask();
//            }
//            else if (VehicleTryingToEnter != null && VehicleTaskedToEnter.Exists() && VehicleTaskedToEnter.Speed > 1.0f)// && (VehicleTryingToEnter.Vehicle.Handle != VehicleTaskedToEnter.Handle || SeatTaskedToEnter != SeatTryingToEnter) && Ped.Pedestrian.Exists() && !Ped.Pedestrian.IsInAnyVehicle(true))
//            {
//                GetInCarTask();
//            }
//        }
//    }
//    private void GetInCarTask()
//    {
//        if (Ped.Pedestrian.Exists() && VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists())
//        {
//            //Ped.Pedestrian.BlockPermanentEvents = true;
//            //Ped.Pedestrian.KeepTasks = true;
//            VehicleTaskedToEnter = VehicleTryingToEnter.Vehicle;
//            SeatTaskedToEnter = SeatTryingToEnter;
//            unsafe
//            {
//                int lol = 0;
//                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
//                NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, VehicleTryingToEnter.Vehicle, -1, SeatTryingToEnter, 1f, 9);
//                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
//                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
//                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
//                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
//            }
//        }
//    }
//    private void ClearTasks(bool resetAlertness)//temp public
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            int seatIndex = 0;
//            Vehicle CurrentVehicle = null;
//            bool WasInVehicle = false;
//            if (Ped.Pedestrian.IsInAnyVehicle(false))
//            {
//                WasInVehicle = true;
//                CurrentVehicle = Ped.Pedestrian.CurrentVehicle;
//                seatIndex = Ped.Pedestrian.SeatIndex;
//            }
//            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
//            Ped.Pedestrian.BlockPermanentEvents = false;
//            Ped.Pedestrian.KeepTasks = false;
//            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
//            if (resetAlertness)
//            {
//                NativeFunction.Natives.SET_PED_ALERTNESS(Ped.Pedestrian, 0);
//            }
//            if (WasInVehicle && !Ped.Pedestrian.IsInAnyVehicle(false) && CurrentVehicle != null)
//            {
//                Ped.Pedestrian.WarpIntoVehicle(CurrentVehicle, seatIndex);
//            }
//            //EntryPoint.WriteToConsole(string.Format("     ClearedTasks: {0}", Ped.Pedestrian.Handle));
//        }
//    }
//    private void Nothing(bool IsFirstRun)
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            //EntryPoint.WriteToConsole($"COP EVENT: Nothing Idle Start: {Ped.Pedestrian.Handle}", 3);
//            if (IsFirstRun)
//            {
//                ClearTasks(false);
//                GameTimeClearedIdle = Game.GameTime;
//            }
//            else if (Game.GameTime - GameTimeClearedIdle >= 10000)
//            {
//                NativeFunction.Natives.SET_PED_ALERTNESS(Ped.Pedestrian, 0);
//            }
//        }
//    }
//}
