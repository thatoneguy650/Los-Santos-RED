using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Tasking
{
    private static Random rnd;
    private static List<PoliceTask> CopsToTask = new List<PoliceTask>();
    public static bool IsRunning { get; set; } = true;
    static Tasking()
    {
        rnd = new Random();
    }
    public static void Initialize()
    {
        MainLoop();
    }
    private static void MainLoop()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                while (IsRunning)
                {
                    int _ToTask = CopsToTask.Count;

                    if (_ToTask > 0)
                    {
                        Debugging.WriteToLog("TaskQueue", string.Format("Cops To Task: {0}", _ToTask));
                        PoliceTask _policeTask = CopsToTask[0];
                        _policeTask.CopToAssign.isTasked = true;

                        if (_policeTask.TaskToAssign == PoliceTask.Task.Untask && CopsToTask.Any(x => x.CopToAssign == _policeTask.CopToAssign && x.TaskToAssign != PoliceTask.Task.Untask && x.GameTimeAssigned >= _policeTask.GameTimeAssigned))
                        {
                            _policeTask.CopToAssign.TaskIsQueued = false;
                            CopsToTask.RemoveAt(0);
                        }
                        else
                        {
                            if (_policeTask.TaskToAssign == PoliceTask.Task.Arrest)
                                TaskChasing(_policeTask.CopToAssign);
                            else if (_policeTask.TaskToAssign == PoliceTask.Task.Chase)
                                TaskChasing(_policeTask.CopToAssign);
                            else if (_policeTask.TaskToAssign == PoliceTask.Task.Untask)
                                Untask(_policeTask.CopToAssign);
                            else if (_policeTask.TaskToAssign == PoliceTask.Task.SimpleArrest)
                                TaskSimpleArrest(_policeTask.CopToAssign);
                            else if (_policeTask.TaskToAssign == PoliceTask.Task.SimpleChase)
                                TaskSimpleChase(_policeTask.CopToAssign);
                            else if (_policeTask.TaskToAssign == PoliceTask.Task.VehicleChase)
                                TaskVehicleChase(_policeTask.CopToAssign);
                            else if (_policeTask.TaskToAssign == PoliceTask.Task.SimpleInvestigate)
                                TaskSimpleInvestigate(_policeTask.CopToAssign);
                            else if (_policeTask.TaskToAssign == PoliceTask.Task.GoToWantedCenter)
                                TaskGoToWantedCenter(_policeTask.CopToAssign);
                            else if (_policeTask.TaskToAssign == PoliceTask.Task.RandomSpawnIdle)
                                RandomSpawnIdle(_policeTask.CopToAssign);

                            _policeTask.CopToAssign.TaskIsQueued = false;
                            CopsToTask.RemoveAt(0);
                        }
                    }
                    GameFiber.Sleep(100);
                }
            }
            catch (Exception e)
            {
                InstantAction.Dispose();
                Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
            }
        });
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void AddItemToQueue(PoliceTask MyTask)
    {
        if (!CopsToTask.Any(x => x.CopToAssign == MyTask.CopToAssign && x.TaskToAssign == MyTask.TaskToAssign))
        {
            MyTask.GameTimeAssigned = Game.GameTime;
            CopsToTask.Add(MyTask);
            MyTask.CopToAssign.TaskIsQueued = true;
        }
    }
    private static void TaskChasing(GTACop Cop)
    {
        if (Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f) && Cop.TaskFiber != null && Cop.TaskFiber.Name == "Chase" && !Cop.RecentlySeenPlayer())
        {
            return;
        }
        if (!Cop.CopPed.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f) && Cop.TaskFiber != null)
        {
            Cop.CopPed.Tasks.Clear();
            Cop.CopPed.BlockPermanentEvents = false;
            Cop.TaskFiber.Abort();
            Cop.TaskFiber = null;
            Debugging.WriteToLog("Task Chasing", string.Format("Initial Return: {0}", Cop.CopPed.Handle));
            return;
        }
        Cop.TaskType = PoliceTask.Task.Chase;
        Cop.TaskFiber =
        GameFiber.StartNew(delegate
        {
            if (!Cop.CopPed.Exists())
                return;
            Debugging.WriteToLog("Task Chasing", string.Format("Started Chase: {0}", Cop.CopPed.Handle));
            uint TaskTime = 0;// = Game.GameTime;
            string LocalTaskName = "GoTo";
            double cool = rnd.NextDouble() * (1.175 - 1.1) + 1.1;
            float MoveRate = (float)cool;
            Cop.SimpleTaskName = "Chase";
            NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_CLIMBOVERS", Cop.CopPed, true);
            NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_LADDERS", Cop.CopPed, true);
            NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_DROP_FROM_HEIGHT", Cop.CopPed, true);
            Cop.CopPed.BlockPermanentEvents = true;

            //Main Loop
            while (Cop.CopPed.Exists() && !Cop.CopPed.IsDead)
            {
                Cop.CopPed.BlockPermanentEvents = true;

                NativeFunction.CallByName<uint>("SET_PED_MOVE_RATE_OVERRIDE", Cop.CopPed, MoveRate);
                if (TaskTime == 0 || Game.GameTime - TaskTime >= 250)//250
                {
                    ArmCopAppropriately(Cop);
                    if (Cop.DistanceToPlayer > 100f || !Cop.SeenPlayerSince(15000))
                        break;

                    Cop.InChasingLoop = true;
                    if (Cop.CopPed.IsGettingIntoVehicle)
                    {
                        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Handle == Cop.CopPed.VehicleTryingToEnter.Handle)
                        {
                            Cop.CopPed.Tasks.Clear();
                            NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", Cop.CopPed, Game.LocalPlayer.Character, 4f, 20f);
                            Cop.CopPed.KeepTasks = true;
                            TaskTime = Game.GameTime;
                            Cop.SubTaskName = "Arrest";
                            LocalTaskName = "Arrest";
                            Debugging.WriteToLog("TaskChasing", string.Format("Cop SubTasked with Car Arrest From Carjacking!!!!, {0}", Cop.CopPed.Handle));
                        }
                    }

                    if (InstantAction.PlayerInVehicle && Game.LocalPlayer.Character.CurrentVehicle.Speed <= 2.5f)
                    {
                        if (Cop.isPursuitPrimary && Cop.DistanceToPlayer <= 25f && LocalTaskName != "CarJack")
                        {
                            Cop.CopPed.CanRagdoll = false;
                            //NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", Cop.CopPed, Game.LocalPlayer.Character.CurrentVehicle, -1, -1, 2f, 9);

                            NativeFunction.CallByName<bool>("TASK_OPEN_VEHICLE_DOOR", Cop.CopPed, Game.LocalPlayer.Character.CurrentVehicle, -1, -1, 10f);
                            Cop.CopPed.KeepTasks = true;
                            TaskTime = Game.GameTime;
                            Cop.SubTaskName = "CarJack";
                            LocalTaskName = "CarJack";
                            Debugging.WriteToLog("TaskChasing", "Primary Cop SubTasked with CarJack 2");
                        }
                        else if (!Cop.isPursuitPrimary && Cop.DistanceToPlayer <= 25f && LocalTaskName != "Arrest")
                        {
                            NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", Cop.CopPed, Game.LocalPlayer.Character, 4f, 20f);
                            Cop.CopPed.KeepTasks = true;
                            TaskTime = Game.GameTime;
                            Cop.SubTaskName = "Arrest";
                            LocalTaskName = "Arrest";
                            Debugging.WriteToLog("TaskChasing", string.Format("Cop SubTasked with Car Arrest, {0}", Cop.CopPed.Handle));
                        }
                    }
                    else
                    {
                        if (LocalTaskName != "Arrest" && (Police.CurrentPoliceState == Police.PoliceState.ArrestedWait || (Police.CurrentPoliceState == Police.PoliceState.CautiousChase && Cop.DistanceToPlayer <= 15f)))
                        {
                            unsafe
                            {
                                int lol = 0;
                                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Game.LocalPlayer.Character, -1, 20f, 500f, 1073741824, 1); //Original and works ok
                                NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", 0, Game.LocalPlayer.Character, 4f, 20f);
                                NativeFunction.CallByName<bool>("TASK_AIM_GUN_AT_ENTITY", 0, Game.LocalPlayer.Character, 10000, false);
                                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.CopPed, lol);
                                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                            }
                            TaskTime = Game.GameTime;
                            Cop.CopPed.KeepTasks = true;
                            Cop.SubTaskName = "Arrest";
                            LocalTaskName = "Arrest";
                        }
                        else if (LocalTaskName != "GotoShooting" && Police.CurrentPoliceState == Police.PoliceState.UnarmedChase && Cop.DistanceToPlayer <= 7f)
                        {
                            Cop.CopPed.CanRagdoll = true;
                            NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY", Cop.CopPed, Game.LocalPlayer.Character, Game.LocalPlayer.Character, 200f, true, 4.0f, 200f, false, false, (uint)FiringPattern.DelayFireByOneSecond);
                            Cop.CopPed.KeepTasks = true;
                            TaskTime = Game.GameTime;
                            Cop.SubTaskName = "GotoShooting";
                            LocalTaskName = "GotoShooting";
                        }
                        else if (LocalTaskName != "Goto" && (Police.CurrentPoliceState == Police.PoliceState.UnarmedChase || Police.CurrentPoliceState == Police.PoliceState.CautiousChase) && Cop.DistanceToPlayer >= 15) //was 15f
                        {
                            Cop.CopPed.CanRagdoll = true;
                            NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", Cop.CopPed, Game.LocalPlayer.Character, -1, 5.0f, 500f, 1073741824, 1); //Original and works ok
                            Cop.CopPed.KeepTasks = true;
                            TaskTime = Game.GameTime;
                            LocalTaskName = "Goto";
                            Cop.SubTaskName = "Goto";
                        }

                    }

                    if ((InstantAction.HandsAreUp || Game.LocalPlayer.Character.IsStunned || Game.LocalPlayer.Character.IsRagdoll) && !InstantAction.isBusted && Cop.DistanceToPlayer <= 4f && !Police.PlayerWasJustJacking)
                        Police.SurrenderBust = true;

                    if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Speed <= 4f && !InstantAction.isBusted && Cop.DistanceToPlayer <= 4f && !Police.PlayerWasJustJacking)
                        Police.SurrenderBust = true;

                    if (InstantAction.PlayerInVehicle && (Cop.DistanceToPlayer >= 45f || Game.LocalPlayer.Character.CurrentVehicle.Speed >= 10f))
                    {
                        GameFiber.Sleep(rnd.Next(500, 2000));//GameFiber.Sleep(rnd.Next(900, 1500));//reaction time?
                        break;
                    }
                    Cop.CopPed.KeepTasks = true;
                    TaskTime = Game.GameTime;
                }

                GameFiber.Yield();
                if (Police.CurrentPoliceState == Police.PoliceState.Normal || Police.CurrentPoliceState == Police.PoliceState.DeadlyChase || InstantAction.isDead)
                {
                    GameFiber.Sleep(rnd.Next(500, 2000));//GameFiber.Sleep(rnd.Next(900, 1500));//reaction time?
                    break;
                }
            }
            if (Cop.CopPed.Exists() && !Cop.CopPed.IsDead)
            {
                Cop.CopPed.BlockPermanentEvents = false;
                Cop.CopPed.Tasks.Clear();
                if (Cop.CopPed.LastVehicle.Exists() && !Cop.CopPed.LastVehicle.IsPoliceVehicle)
                    Cop.CopPed.ClearLastVehicle();
            }
            Debugging.WriteToLog("Task Chasing", string.Format("Loop End: {0}", Cop.CopPed.Handle));
            Cop.TaskFiber = null;
            Cop.isTasked = false;
            Cop.TaskType = PoliceTask.Task.NoTask;
            Cop.SimpleTaskName = "";
            if (Cop.CopPed.Exists() && !Cop.CopPed.IsDead)
                Cop.CopPed.CanRagdoll = true;


            Cop.InChasingLoop = false;
        }, "Chase");
        Debugging.GameFibers.Add(Cop.TaskFiber);
    }
    private static void TaskSimpleChase(GTACop Cop)
    {
        Cop.TaskType = PoliceTask.Task.SimpleChase;
        Cop.CopPed.BlockPermanentEvents = true;
        Cop.SimpleTaskName = "SimpleChase";
        Cop.CopPed.Tasks.GoToWhileAiming(Game.LocalPlayer.Character, 10f, 40f);
        Cop.CopPed.KeepTasks = true;
        Debugging.WriteToLog("TaskSimpleChase", "How many times i this getting called?");
    }
    private static void TaskSimpleArrest(GTACop Cop)
    {
        Cop.TaskType = PoliceTask.Task.SimpleArrest;
        Cop.CopPed.BlockPermanentEvents = true;
        Cop.SimpleTaskName = "SimpleArrest";
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Game.LocalPlayer.Character, -1, 20f, 500f, 1073741824, 1); //Original and works ok
            NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", 0, Game.LocalPlayer.Character, 4f, 20f);
            NativeFunction.CallByName<bool>("TASK_AIM_GUN_AT_ENTITY", 0, Game.LocalPlayer.Character, -1, false);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.CopPed, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
        Cop.CopPed.KeepTasks = true;
        Debugging.WriteToLog("TaskSimpleArrest", string.Format("Started SimpleArrest: {0}", Cop.CopPed.Handle));
    }
    private static void TaskVehicleChase(GTACop Cop)
    {
        if (!PoliceScanning.CopPeds.Any(x => x.TaskType == PoliceTask.Task.Chase))
        {
            Debugging.WriteToLog("Task Vehicle Chasing", string.Format("Didn't Start Vehicle Chase: {0}", Cop.CopPed.Handle));
            return; //Only task this is we already have officers on foot
        }
        Cop.TaskType = PoliceTask.Task.VehicleChase;
        Cop.TaskFiber =
        GameFiber.StartNew(delegate
        {
            Debugging.WriteToLog("Task Vehicle Chasing", string.Format("Started Vehicle Chase: {0}", Cop.CopPed.Handle));
            uint TaskTime = Game.GameTime;
            Cop.CopPed.BlockPermanentEvents = true;
            Cop.SimpleTaskName = "VehicleChase";

            // Cop.CopPed.Tasks.ChaseWithGroundVehicle(Game.LocalPlayer.Character);
            NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Cop.CopPed, 100f);
            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.CopPed, 3, false);
            // NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Cop.CopPed, 8f);
            // NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.CopPed, 32, true);
            Cop.CopPed.KeepTasks = true;

            //Main Loop
            while (Cop.CopPed.Exists() && !Cop.CopPed.IsDead)
            {

                if (Game.GameTime - TaskTime >= 250)
                {
                    //if (!CopPeds.Any(x => x.TaskFiber != null && x.TaskFiber.Name == "Chase"))
                    //{
                    //    GameFiber.Sleep(rnd.Next(500, 2000));//GameFiber.Sleep(rnd.Next(900, 1500));//reaction time?
                    //    break;
                    //}

                    if (!Cop.CopPed.IsInAnyVehicle(false))
                    {
                        Debugging.WriteToLog("Task Vehicle Chase", string.Format("I got out of my car like a dummy: {0}", Cop.CopPed.Handle));
                        break;
                    }

                    if (InstantAction.PlayerInVehicle)
                    {
                        Debugging.WriteToLog("Task Vehicle Chase", string.Format("Player got in a vehicle, letting ai takeover: {0}", Cop.CopPed.Handle));
                        break;
                    }


                    if (!Cop.RecentlySeenPlayer())
                    {
                        Debugging.WriteToLog("Task Vehicle Chase", string.Format("Lost the player, let AI takeover: {0}", Cop.CopPed.Handle));
                        break;
                    }


                    Vector3 PlayerPos = Game.LocalPlayer.Character.Position;
                    Vector3 DrivingCoords = World.GetNextPositionOnStreet(PlayerPos);
                    NativeFunction.CallByName<bool>("SET_DRIVE_TASK_DRIVING_STYLE", Cop.CopPed, 6);

                    //NativeFunction.CallByName<bool>("TASK_VEHICLE_GOTO_NAVMESH", Cop.CopPed, Cop.CopPed.CurrentVehicle, PlayerPos.X, PlayerPos.Y, PlayerPos.Z,10.0f, 156, 3f);
                    // NativeFunction.CallByName<bool>("TASK_VEHICLE_GOTO_NAVMESH", Cop.CopPed, Cop.CopPed.CurrentVehicle, PlayerPos.X, PlayerPos.Y, PlayerPos.Z, 15.0f, 110, 5f); //Best one so far, but they get out
                    //NativeFunction.CallByName<bool>("TASK_VEHICLE_GOTO_NAVMESH", Cop.CopPed, Cop.CopPed.CurrentVehicle, PlayerPos.X, PlayerPos.Y, PlayerPos.Z,10.0f, 171, 3f);
                    //escort?
                    //NativeFunction.CallByName<bool>("TASK_VEHICLE_GOTO_NAVMESH", Cop.CopPed, Cop.CopPed.CurrentVehicle, PlayerPos.X, PlayerPos.Y + 10f, PlayerPos.Z, 25f, 110, 10f);
                    NativeFunction.CallByName<bool>("TASK_VEHICLE_GOTO_NAVMESH", Cop.CopPed, Cop.CopPed.CurrentVehicle, DrivingCoords.X, DrivingCoords.Y, DrivingCoords.Z, 25f, 110, 10f);


                    Cop.CopPed.KeepTasks = true;
                    TaskTime = Game.GameTime;
                }
                GameFiber.Yield();
                if (Police.CurrentPoliceState == Police.PoliceState.Normal || Police.CurrentPoliceState == Police.PoliceState.DeadlyChase || Police.CurrentPoliceState == Police.PoliceState.ArrestedWait || InstantAction.isBusted || InstantAction.isDead)
                {
                    GameFiber.Sleep(rnd.Next(500, 2000));//GameFiber.Sleep(rnd.Next(900, 1500));//reaction time?
                    break;
                }
            }
            if (Cop.CopPed.Exists() && !Cop.CopPed.IsDead)
            {
                NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.CopPed, 3, true);
                Cop.CopPed.BlockPermanentEvents = false;
                Cop.CopPed.Tasks.Clear();
            }
            Debugging.WriteToLog("Task Vehicle Chase", string.Format("Loop End: {0}", Cop.CopPed.Handle));
            Cop.TaskFiber = null;
            Cop.isTasked = false;
            Cop.SimpleTaskName = "";
            Cop.TaskType = PoliceTask.Task.NoTask;
        }, "VehicleChase");
        Debugging.GameFibers.Add(Cop.TaskFiber);
    }
    private static void TaskSimpleInvestigate(GTACop Cop)
    {
        if (!Cop.CopPed.Exists())
            return;
        Cop.TaskType = PoliceTask.Task.SimpleInvestigate;
        Cop.CopPed.BlockPermanentEvents = false;
        Cop.SimpleTaskName = "SimpleInvestigate";
        if (Cop.isInVehicle)
            Cop.CopPed.Tasks.CruiseWithVehicle(30f, VehicleDrivingFlags.Emergency);
        else
            Cop.CopPed.Tasks.Wander();

        // Cop.CopPed.KeepTasks = true;
        Debugging.WriteToLog("TaskSimpleInvestigate", string.Format("Started SimpleInvestigate: {0}", Cop.CopPed.Handle));
    }
    private static void TaskGoToWantedCenter(GTACop Cop)
    {
        if (!Cop.CopPed.Exists())
            return;
        Cop.TaskType = PoliceTask.Task.GoToWantedCenter;
        Cop.CopPed.BlockPermanentEvents = false;
        Cop.SimpleTaskName = "GoToWantedCenter";
        Vector3 WantedCenter = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
        if (Cop.isInVehicle)
            NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.CopPed, Cop.CopPed.CurrentVehicle, WantedCenter.X, WantedCenter.Y, WantedCenter.Z, 70f, 4 | 16 | 32 | 262144, 35f);
        else
            NativeFunction.CallByName<bool>("TASK_GO_STRAIGHT_TO_COORD", Cop.CopPed, WantedCenter.X, WantedCenter.Y, WantedCenter.Z, 500f, -1, 0f, 2f);

        //Cop.CopPed.KeepTasks = true;
        Debugging.WriteToLog("TaskGoToWantedCenter", string.Format("Started GoToWantedCenter: {0}", Cop.CopPed.Handle));
    }
    public static void RetaskAllRandomSpawns()
    {
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.WasRandomSpawn))
        {
            if (!Cop.TaskIsQueued)
            {
                Cop.TaskIsQueued = true;
                AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.RandomSpawnIdle));
            }
        }
        Debugging.WriteToLog("RetaskAllRandomSpawns", "Done");
    }
    private static void RandomSpawnIdle(GTACop Cop)
    {
        if (Cop.CopPed.Exists())
        {
            if (!Cop.CopPed.IsInAnyVehicle(false))
            {
                Vehicle LastVehicle = Cop.CopPed.LastVehicle;
                if (LastVehicle.Exists() && LastVehicle.IsDriveable && Cop.WasRandomSpawnDriver)
                {
                    unsafe
                    {
                        int lol = 0;
                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, LastVehicle, -1, -1, 2f, 9);
                        NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, LastVehicle, 18f, 183);
                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.CopPed, lol);
                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    }
                    Debugging.WriteToLog("RetaskAllRandomSpawns", "Told him to get in and drive");
                }
                else
                {
                    Cop.CopPed.Tasks.Wander();
                    Debugging.WriteToLog("RetaskAllRandomSpawns", "Told him to wander");
                }
            }
            else
            {
                Cop.CopPed.Tasks.CruiseWithVehicle(Cop.CopPed.CurrentVehicle, 15f, VehicleDrivingFlags.Normal);
                Cop.CopPed.CurrentVehicle.IsSirenOn = false;
                //NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", Cop.CopPed, Cop.CopPed.CurrentVehicle, 18f, 183);
                //Cop.CopPed.Tasks.Wander();
                Debugging.WriteToLog("RetaskAllRandomSpawns", "Told him to drive");
            }
        }

    }
    public static void UntaskAll(bool OnlyTasked)
    {
        foreach (GTACop Cop in PoliceScanning.CopPeds)
        {

            if (OnlyTasked && Cop.isTasked && !Cop.TaskIsQueued)
            {
                Cop.TaskIsQueued = true;
                AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
            }
            else
            {
                Cop.TaskIsQueued = true;
                AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
            }
        }
        foreach (GTACop Cop in PoliceScanning.K9Peds)
        {
            if (Cop.isTasked && !Cop.TaskIsQueued)
            {
                Cop.TaskIsQueued = true;
                AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
            }
        }
        Debugging.WriteToLog("UntaskAll", "");
    }
    public static void UntaskAllRandomSpawns(bool OnlyTasked)
    {
        foreach (GTACop Cop in PoliceScanning.CopPeds.Where(x => x.WasRandomSpawn))
        {
            if (OnlyTasked && Cop.isTasked && !Cop.TaskIsQueued)
            {
                Cop.TaskIsQueued = true;
                AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
            }
            else
            {
                Cop.TaskIsQueued = true;
                AddItemToQueue(new PoliceTask(Cop, PoliceTask.Task.Untask));
            }
        }

        Debugging.WriteToLog("UntaskAll Random", "");
    }
    private static void Untask(GTACop Cop)
    {
        if (Cop.CopPed.Exists())
        {
            if (Cop.TaskFiber != null)
            {
                Cop.TaskFiber.Abort();
                Cop.TaskFiber = null;
            }
            int seatIndex = 0;
            Vehicle CurrentVehicle = null;
            bool WasInVehicle = false;
            if (Cop.WasRandomSpawn && Cop.CopPed.IsInAnyVehicle(false))
            {
                WasInVehicle = true;
                CurrentVehicle = Cop.CopPed.CurrentVehicle;
                seatIndex = Cop.CopPed.SeatIndex;
            }
            Cop.CopPed.Tasks.Clear();

            Cop.CopPed.BlockPermanentEvents = false;

            if (!Cop.WasRandomSpawn)
                Cop.CopPed.IsPersistent = false;

            if (Cop.WasRandomSpawn && WasInVehicle && !Cop.CopPed.IsInAnyVehicle(false) && CurrentVehicle != null)
            {
                Cop.CopPed.WarpIntoVehicle(CurrentVehicle, seatIndex);

            }

            if (WasInVehicle)
                Debugging.WriteToLog("Untask", string.Format("Untasked: {0} in vehicle", Cop.CopPed.Handle));
            else
                Debugging.WriteToLog("Untask", string.Format("Untasked: {0}", Cop.CopPed.Handle));
        }

        Cop.TaskType = PoliceTask.Task.NoTask;
        Cop.SimpleTaskName = "";
        Cop.isTasked = false;
    }
    private static void ArmCopAppropriately(GTACop Cop)
    {
        if (Police.CurrentPoliceState == Police.PoliceState.UnarmedChase)
        {
            Police.SetCopTazer(Cop);
        }
        else if (Police.CurrentPoliceState == Police.PoliceState.CautiousChase)
        {
            Police.SetCopDeadly(Cop);
        }
        else if (Police.CurrentPoliceState == Police.PoliceState.ArrestedWait && Police.LastPoliceState == Police.PoliceState.UnarmedChase)
        {
            Police.SetCopTazer(Cop);
        }
        else if (Police.CurrentPoliceState == Police.PoliceState.ArrestedWait && Police.LastPoliceState != Police.PoliceState.UnarmedChase)
        {
            Police.SetCopDeadly(Cop);
        }
    }
}

