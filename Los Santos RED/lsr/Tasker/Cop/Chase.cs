using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System.Linq;


public class Chase : ComplexTask
{
    private float ChaseDistance = 5f;
    private Cop Cop;
    private Vehicle CopsVehicle;
    private SubTask CurrentSubTask;
    private Task CurrentTask = Task.Nothing;
    private uint GameTimeChaseStarted;
    private uint GameTimeGotStuck;
    private uint GameTimeVehicleStoppedMoving;
    private bool hasOwnFiber = false;
    private bool IsChasingRecklessly;
    private bool IsChasingSlowly = false;
    private bool IsFirstRun;
    private bool IsStuck;
    private Vector3 LastPosition;
    private bool NeedsUpdates;
    private bool prevIsChasingSlowly = false;
    private IEntityProvideable World;
    public Chase(IComplexTaskable myPed, ITargetable player, IEntityProvideable world, Cop cop) : base(player, myPed, 500)//was 500
    {
        Name = "Chase";
        SubTaskName = "";
        World = world;
        Cop = cop;
    }
    private enum eVehicleMissionType
    {
        Cruise = 1,
        Ram = 2,
        Block = 3,
        GoTo = 4,
        Stop = 5,
        Attack = 6,
        Follow = 7,
        Flee = 8,
        Circle = 9,
        Escort = 12,
        FollowRecording = 15,
        PoliceBehaviour = 16,
        Land = 19,
        Land2 = 20,
        Crash = 21,
        PullOver = 22,
        HeliProtect = 23
    };
    private enum SubTask
    {
        Shoot,
        Aim,
        Goto,
        None,
        CarJackPlayer,
        GoToVehicleDoor,
        Look,
    }
    private enum Task
    {
        VehicleChase,
        VehicleChasePed,
        ExitVehicle,
        EnterVehicle,
        CarJack,
        FootChase,
        Nothing,
        StopCar,
    }
    public bool ShouldStopCar => Ped.DistanceToPlayer < 30f && Ped.Pedestrian.CurrentVehicle.Exists() && Ped.Pedestrian.CurrentVehicle.Speed > 0.5f && !Player.IsMovingFast && !ChaseRecentlyStarted && !Ped.IsInHelicopter && !Ped.IsInBoat;
    private bool ChaseRecentlyStarted => false;
    private bool ShouldAim => Player.WantedLevel > 1;
    private bool ShouldCarJackPlayer => Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() && !Player.IsMovingFast;
    private bool ShouldChasePedInVehicle => Ped.IsDriver && (Ped.DistanceToPlayer >= 55f || Ped.IsInBoat || Ped.IsInHelicopter || World.Pedestrians.PoliceList.Count(x => x.DistanceToPlayer <= 25f && !x.IsInVehicle) > 3);
    private bool ShouldChaseRecklessly => Player.WantedLevel >= 2;//Player.PoliceResponse.IsDeadlyChase;
    private bool ShouldChaseVehicleInVehicle => Ped.IsDriver && Ped.Pedestrian.CurrentVehicle.Exists() && !ShouldExitPoliceVehicle && Player.CurrentVehicle != null;
    // && !Cop.Pedestrian.IsGettingIntoVehicle;
    private bool ShouldExitPoliceVehicle => !Ped.RecentlyGotInVehicle && Ped.DistanceToPlayer < 30f && Ped.Pedestrian.CurrentVehicle.Exists() && Ped.Pedestrian.CurrentVehicle.Speed < 0.5f && !Player.IsMovingFast && !ChaseRecentlyStarted && !Ped.IsInHelicopter && !Ped.IsInBoat;
    //35f;//25f
    private bool ShouldGetBackInCar => !Ped.RecentlyGotOutOfVehicle && Ped.Pedestrian.Exists() && CopsVehicle.Exists() && Ped.Pedestrian.DistanceTo2D(CopsVehicle) <= 30f && CopsVehicle.IsDriveable && CopsVehicle.FreeSeatsCount > 0;
    private bool ShouldShoot => !Player.IsBusted && !Player.IsAttemptingToSurrender && Player.WantedLevel > 1;
    //25f
    // GameTimeChaseStarted != 0 &&  Game.GameTime - GameTimeChaseStarted <= 3000;
    private bool VehicleIsStopped => GameTimeVehicleStoppedMoving != 0 && Game.GameTime - GameTimeVehicleStoppedMoving >= 500;//20000
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            EntryPoint.WriteToConsole($"TASKER: Chase Start: {Ped.Pedestrian.Handle} ChaseDistance: {ChaseDistance}", 5);
            GameTimeChaseStarted = Game.GameTime;
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            NativeFunction.Natives.SET_PED_PATH_CAN_USE_CLIMBOVERS(Ped.Pedestrian, true);
            NativeFunction.Natives.SET_PED_PATH_CAN_USE_LADDERS(Ped.Pedestrian, true);
            NativeFunction.Natives.SET_PED_PATH_CAN_DROP_FROM_HEIGHT(Ped.Pedestrian, true);
            Update();
        }
    }
    public override void Stop()
    {

    }
    public override void Update()
    {
        if (Ped.Pedestrian.Exists() && ShouldUpdate)
        {
            if (Ped.Pedestrian.IsInAnyPoliceVehicle && !CopsVehicle.Exists())
            {
                CopsVehicle = Ped.Pedestrian.CurrentVehicle;
            }
            Task UpdatedTask = GetCurrentTaskDynamic();
            if (CurrentTask != UpdatedTask)
            {
                IsFirstRun = true;
                hasOwnFiber = false;
                CurrentTask = UpdatedTask;
                NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 10f);

                //EntryPoint.WriteToConsole($"TASKER: Chase SubTask Changed: {Ped.Pedestrian.Handle} to {CurrentTask} {CurrentDynamic}");
                ExecuteCurrentSubTask();
            }
            else if (NeedsUpdates)
            {
                ExecuteCurrentSubTask();
            }
            else if (IsChasingSlowly != prevIsChasingSlowly)
            {
                CurrentSubTask = SubTask.None;
                if (!hasOwnFiber)
                {
                    ExecuteCurrentSubTask();
                }
                prevIsChasingSlowly = IsChasingSlowly;
            }
            if (Ped.IsInVehicle)//CurrentTask == Task.VehicleChase || CurrentTask == Task.VehicleChasePed || Cu)
            {
                //NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.NoContact, true);
                SetSiren();
                if (Ped.IsInVehicle && Ped.Pedestrian.CurrentVehicle.Exists())
                {
                    if (Ped.Pedestrian.CurrentVehicle.Speed == 0f)
                    {
                        if (GameTimeVehicleStoppedMoving == 0)
                        {
                            GameTimeVehicleStoppedMoving = Game.GameTime;
                        }

                    }
                    else
                    {
                        GameTimeVehicleStoppedMoving = 0;
                    }
                }
            }
            GameTimeLastRan = Game.GameTime;
        }
        //EntryPoint.WriteToConsole($"TASKER: Chase UpdateEnd: {Ped.Pedestrian.Handle} InVeh: {Ped.IsInVehicle} InVeh2: {Ped.Pedestrian.IsInAnyVehicle(false)}");
    }
    private void EnterVehicle()
    {
        Ped.Pedestrian.BlockPermanentEvents = true;
        Ped.Pedestrian.KeepTasks = true;
        NeedsUpdates = false;
        if (Ped.Pedestrian.Exists() && CopsVehicle.Exists())
        {
            NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", Ped.Pedestrian, CopsVehicle, -1, Ped.LastSeatIndex, 2.0f, 9);
        }
        //EntryPoint.WriteToConsole(string.Format("Started Enter Old Car: {0}", Ped.Pedestrian.Handle));

    }
    private void ExecuteCurrentSubTask()
    {
        if (CurrentTask == Task.CarJack)
        {
            RunInterval = 200;
            SubTaskName = "CarJack";
            GoToPlayersCar();
        }
        else if (CurrentTask == Task.EnterVehicle)
        {
            Cop.ResetWeaponsState();
            RunInterval = 200;
            SubTaskName = "EnterVehicle";
            EnterVehicle();
        }
        else if (CurrentTask == Task.ExitVehicle)
        {
            Cop.ResetWeaponsState();
            RunInterval = 200;
            SubTaskName = "ExitVehicle";
            ExitVehicle();
        }
        else if (CurrentTask == Task.FootChase)
        {
            Cop.ResetWeaponsState();
            RunInterval = 200;
            SubTaskName = "FootChase";
            FootChase();
        }
        else if (CurrentTask == Task.VehicleChase)
        {
            Cop.ResetWeaponsState();
            RunInterval = 500;
            SubTaskName = "VehicleChase";
            VehicleChase();
        }
        else if (CurrentTask == Task.VehicleChasePed)
        {
            Cop.ResetWeaponsState();
            RunInterval = 500;
            SubTaskName = "VehicleChasePed";
            VehicleChasePed();
        }
        else if (CurrentTask == Task.Nothing)
        {
            Cop.ResetWeaponsState();
            RunInterval = 500;
            SubTaskName = "Nothing";
            //VehicleChasePed();
        }
        else if (CurrentTask == Task.StopCar)
        {
            Cop.ResetWeaponsState();
            RunInterval = 500;
            SubTaskName = "StopCar";
            StopCar();
        }
        GameTimeLastRan = Game.GameTime;
    }
    private void ExitVehicle()
    {
        NeedsUpdates = false;
        Ped.Pedestrian.BlockPermanentEvents = true;
        Ped.Pedestrian.KeepTasks = true;
        if (Ped.Pedestrian.Exists() && Ped.Pedestrian.CurrentVehicle.Exists())
        {
            //NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, 27, 2000);
            //NativeFunction.CallByName<bool>("TASK_LEAVE_VEHICLE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, 256);
            if (Player.WantedLevel == 1)
            {
                IsChasingSlowly = true;
                unsafe
                {
                    int lol = 0;
                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                    NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", 0, Ped.Pedestrian.CurrentVehicle, 27, 1000);
                    NativeFunction.CallByName<bool>("TASK_LEAVE_VEHICLE", 0, Ped.Pedestrian.CurrentVehicle, 64);
                    NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.Character, -1, 3f, 1.4f, 1073741824, 1); //Original and works ok
                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                }
            }
            else
            {
                IsChasingSlowly = false;
                unsafe
                {
                    int lol = 0;
                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                    NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", 0, Ped.Pedestrian.CurrentVehicle, 27, 1000);
                    NativeFunction.CallByName<bool>("TASK_LEAVE_VEHICLE", 0, Ped.Pedestrian.CurrentVehicle, 256);
                    NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.Character, -1, 7f, 500f, 1073741824, 1); //Original and works ok
                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                }
            }
            //EntryPoint.WriteToConsole(string.Format("Started Exit Car: {0}", Ped.Pedestrian.Handle));
        }
    }
    private void FootChase()
    {
        NeedsUpdates = false;
        hasOwnFiber = true;
        Ped.IsRunningOwnFiber = true;
        float MoveRate = (float)(RandomItems.MyRand.NextDouble() * (1.175 - 1.1) + 1.1);
        float RunSpeed = 500f;
        bool prevIsChasingSlowly = IsChasingSlowly;
        CurrentSubTask = SubTask.None;
        GameFiber.StartNew(delegate
        {
            while (hasOwnFiber && Ped.Pedestrian.Exists() && Ped.CurrentTask != null & Ped.CurrentTask?.Name == "Chase" && CurrentTask == Task.FootChase)
            {
                if (Player.WantedLevel == 1)
                {
                    IsChasingSlowly = true;
                    RunSpeed = 3.0f;// 1.4f;
                }
                else
                {
                    IsChasingSlowly = false;
                    RunSpeed = 500f;
                    NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE<uint>(Ped.Pedestrian, MoveRate);
                }
                Ped.Pedestrian.BlockPermanentEvents = true;
                Ped.Pedestrian.KeepTasks = false;
                float LocalDistance = Ped.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character);

                if (CurrentSubTask != SubTask.Shoot && ShouldShoot && LocalDistance < 10f && ShouldAim)//7f
                {
                    CurrentSubTask = SubTask.Shoot;
                    if (LocalDistance > 5f)
                    {
                        unsafe
                        {
                            int lol = 0;
                            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                            NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY", 0, Player.Character, Player.Character, 200f, true, 4.0f, 200f, false, false, (uint)FiringPattern.DelayFireByOneSecond);
                            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                        }
                    }
                    else
                    {
                        unsafe
                        {
                            int lol = 0;
                            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                            NativeFunction.CallByName<bool>("TASK_SHOOT_AT_ENTITY", 0, Player.Character, -1, (uint)FiringPattern.DelayFireByOneSecond);
                            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                        }
                    }

                }
                else if (CurrentSubTask != SubTask.Aim && !ShouldShoot && LocalDistance < 10f && ShouldAim)//7f
                {
                    CurrentSubTask = SubTask.Aim;
                    if (LocalDistance > 5f)
                    {
                        unsafe
                        {
                            int lol = 0;
                            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                            NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", 0, Player.Character, 4f, 20f);
                            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                        }
                    }
                    else
                    {
                        unsafe
                        {
                            int lol = 0;
                            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                            NativeFunction.CallByName<bool>("TASK_AIM_GUN_AT_ENTITY", 0, Player.Character, -1, false);
                            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                        }
                    }
                }
                else if (CurrentSubTask != SubTask.Look && !ShouldShoot && LocalDistance < 10f && !ShouldAim)//7f
                {
                    CurrentSubTask = SubTask.Look;
                    //if (LocalDistance > 5f)
                    //{
                    unsafe
                    {
                        int lol = 0;
                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.Character, -1, 3f, RunSpeed, 2f, 0);
                        NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Player.Character, 2000);
                        NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Player.Character, -1, 0, 2);
                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    }
                    //}
                    //else
                    //{
                    //    unsafe
                    //    {
                    //        int lol = 0;
                    //        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                    //        NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Player.Character, 2000);
                    //        NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, Player.Character, -1, 0, 2);
                    //        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                    //        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                    //        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                    //        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    //    }
                    //}
                }
                else if (CurrentSubTask != SubTask.Goto && LocalDistance >= 10f)//15f
                {
                    CurrentSubTask = SubTask.Goto;
                    unsafe
                    {
                        int lol = 0;
                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        //NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.Character, -1, 5f, RunSpeed, 1073741824, 1); //Original and works ok//7f
                        NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.Character, -1, 5f, RunSpeed, 2f, 0); //Original and works ok//7f
                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    }

                }
                GameFiber.Yield();
            }
            Ped.IsRunningOwnFiber = false;
        }, "Run Cop Chase Logic");
    }
    private Task GetCurrentTaskDynamic()
    {
        if (CurrentDynamic == AIDynamic.Cop_InVehicle_Player_InVehicle)
        {
            if (ShouldExitPoliceVehicle)
            {
                return Task.ExitVehicle;
            }
            else if (ShouldChaseVehicleInVehicle)
            {
                return Task.VehicleChase;
            }
            else
            {
                return Task.Nothing;
            }
        }
        else if (CurrentDynamic == AIDynamic.Cop_InVehicle_Player_OnFoot)
        {
            if (Ped.IsDriver)
            {
                if (ShouldChasePedInVehicle)
                {
                    return Task.VehicleChasePed;
                }
                else if (ShouldStopCar)//is new
                {
                    return Task.StopCar;
                }
                else if (ShouldExitPoliceVehicle)
                {
                    return Task.ExitVehicle;
                }
                else
                {
                    return Task.Nothing;
                }
            }
            else
            {
                if (ShouldExitPoliceVehicle)
                {
                    return Task.ExitVehicle;
                }
                else
                {
                    return Task.Nothing;
                }
            }
        }
        else if (CurrentDynamic == AIDynamic.Cop_OnFoot_Player_InVehicle)
        {
            if (ShouldCarJackPlayer)
            {
                return Task.CarJack;
            }
            else if (ShouldGetBackInCar)
            {
                return Task.EnterVehicle;
            }
            else
            {
                return Task.Nothing;
            }
        }
        else if (CurrentDynamic == AIDynamic.Cop_OnFoot_Player_OnFoot)
        {
            if (Ped.DistanceToPlayer >= 50f && ShouldGetBackInCar)//this is new, was only footchase in here before, cant wait to see the bugs....
            {
                return Task.EnterVehicle;
            }
            else
            {
                return Task.FootChase;
            }
            //return Task.FootChase;
        }
        else
        {
            return Task.Nothing;
        }
    }
    private void GoToPlayersCar()
    {
        Ped.Pedestrian.BlockPermanentEvents = true;
        Ped.Pedestrian.KeepTasks = true;
        //NeedsUpdates = true;
        if (Ped.Pedestrian.Exists() && Player.IsInVehicle && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
        {
            if (IsFirstRun)
            {
                if (Player.WantedLevel == 1)
                {
                    Cop.ResetWeaponsState();
                    IsChasingSlowly = true;
                    unsafe
                    {
                        int lol = 0;
                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.CurrentVehicle.Vehicle, -1, 3f, 1.4f, 1073741824, 1);
                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    }
                }
                else
                {
                    Cop.SetCompletlyUnarmed();
                    IsChasingSlowly = false;
                    CurrentSubTask = SubTask.GoToVehicleDoor;
                    unsafe
                    {
                        int lol = 0;
                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.CurrentVehicle.Vehicle, -1, 7f, 500f, 1073741824, 1); //Original and works ok
                                                                                                                                             // NativeFunction.CallByName<bool>("TASK_OPEN_VEHICLE_DOOR", 0, Player.CurrentVehicle.Vehicle, -1, -1, 7f); //doesnt really work
                                                                                                                                             //NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, Player.CurrentVehicle.Vehicle, -1, Player.Character.SeatIndex, 5.0f, 9);//caused them to get confused about getting back in thier car
                                                                                                                                             //NativeFunction.CallByName<bool>("TASK_ARREST_PED", 0, Player.Character);
                        NativeFunction.CallByName<bool>("TASK_COMBAT_PED", 0, Player.Character, 0, 16);//do they need to be unarmed or armed with melee?
                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    }
                }
            }
            //else
            //{
            //    if (!IsChasingSlowly)
            //    {
            //        if (CurrentSubTask != SubTask.CarJackPlayer && Ped.DistanceToPlayer <= 10f)
            //        {
            //            unsafe
            //            {
            //                int lol = 0;
            //                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            //                NativeFunction.CallByName<bool>("TASK_COMBAT_PED", 0, Player.Character, 0, 16);//do they need to be unarmed or armed with melee?
            //                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
            //                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            //                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
            //                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            //            }
            //            CurrentSubTask = SubTask.CarJackPlayer;
            //            EntryPoint.WriteToConsole($"", 5);
            //        }
            //        else if (CurrentSubTask != SubTask.GoToVehicleDoor && Ped.DistanceToPlayer > 10f)
            //        {
            //            unsafe
            //            {
            //                int lol = 0;
            //                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            //                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.CurrentVehicle.Vehicle, -1, 7f, 500f, 1073741824, 1);
            //                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
            //                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            //                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
            //                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            //            }
            //            CurrentSubTask = SubTask.GoToVehicleDoor;
            //        }
            //    }
            //}
        }
    }
    private void SetSiren()
    {
        if (Ped.Pedestrian.Exists() && Ped.Pedestrian.CurrentVehicle.Exists() && Ped.Pedestrian.CurrentVehicle.HasSiren && !Ped.Pedestrian.CurrentVehicle.IsSirenOn)
        {
            Ped.Pedestrian.CurrentVehicle.IsSirenOn = true;
            Ped.Pedestrian.CurrentVehicle.IsSirenSilent = false;
        }
    }
    private void StopCar()
    {
        if (Ped.Pedestrian.CurrentVehicle.Exists())
        {
            NeedsUpdates = false;
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 27, 2000);
            //EntryPoint.WriteToConsole($"CHASE Stop Car: {Ped.Pedestrian.Handle}", 5);
        }
        else
        {
            NeedsUpdates = true;
            return;
        }
    }
    private void VehicleChase()
    {
        if (Ped.Pedestrian.Exists())
        {
            NeedsUpdates = true;
            if (IsFirstRun)
            {
                NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, 1000f);
                //NativeFunction.Natives.SET_DRIVER_RACING_MODIFIER(Ped.Pedestrian, 1.0f);
                NativeFunction.Natives.SET_DRIVER_AGGRESSIVENESS(Ped.Pedestrian, 100.0f);

                //NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.FastEmergency);

                //NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_DisableBlockFromPursueDuringVehicleChase, true);
                //NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_DisableCruiseInFrontDuringBlockDuringVehicleChase, false);
                //NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_DisableSpinOutDuringVehicleChase, false);

               
                Ped.Pedestrian.BlockPermanentEvents = true;
                Ped.Pedestrian.KeepTasks = true;
                if (Ped.IsInHelicopter)
                {
                    NativeFunction.Natives.TASK_HELI_CHASE(Ped.Pedestrian, Player.Character, -50f, 50f, 60f);
                }
                else if (Ped.IsInBoat)
                {
                    NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, Player.Character);
                }
                else
                {
                    if (ShouldChaseRecklessly && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
                    {
                        IsChasingRecklessly = true;
                        NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, Player.Character);
                        NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.PIT, true);
                        NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.CrazyEmergency);
                        NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 70f);
                    }
                    else
                    {
                        IsChasingRecklessly = false;
                        NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, Player.Character);
                        NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE(Ped.Pedestrian, 15f);
                        NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.NoContact, true);
                        NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.FastEmergency);
                        NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 70f);
                    }
                    NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 70f);
                }
                IsFirstRun = false;
            }
            else
            {
                NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, 1000f);
                //NativeFunction.Natives.SET_DRIVER_RACING_MODIFIER(Ped.Pedestrian, 1.0f);
                NativeFunction.Natives.SET_DRIVER_AGGRESSIVENESS(Ped.Pedestrian, 100.0f);

                //NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.FastEmergency);

                //NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_DisableBlockFromPursueDuringVehicleChase, true);
                //NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_DisableCruiseInFrontDuringBlockDuringVehicleChase, false);
                //NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_DisableSpinOutDuringVehicleChase, false);

                if (!Ped.IsInHelicopter && !Ped.IsInBoat)
                {
                    if (IsChasingRecklessly)
                    {
                        NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.PIT, true);
                        NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.CrazyEmergency);
                    }
                    else
                    {
                        NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.NoContact, true);
                        NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.FastEmergency);
                    }
                }
                Vector3 CurrentPosition = Ped.Pedestrian.Position;
                IsStuck = LastPosition.DistanceTo2D(CurrentPosition) <= 1.0f;
                if (IsStuck)
                {
                    if (GameTimeGotStuck == 0)
                    {
                        GameTimeGotStuck = Game.GameTime;
                    }
                }
                else
                {
                    GameTimeGotStuck = 0;
                }
                if (IsStuck && Game.GameTime - GameTimeGotStuck >= 3000)
                {
                    EntryPoint.WriteToConsole($"VehicleChase Vehicle Target I AM STUCK!!: {Ped.Pedestrian.Handle}", 5);
                }
                LastPosition = CurrentPosition;
            }
        }
    }
    private void VehicleChasePed()
    {
        if (Ped.Pedestrian.Exists())
        {
            if (Ped.Pedestrian.CurrentVehicle.Exists())
            {
                NeedsUpdates = false;
            }
            else
            {
                NeedsUpdates = true;
                return;
            }
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            float Speed = 30f;
            if (Ped.DistanceToPlayer <= 10f)
            {
                Speed = 10f;
            }
            if (IsFirstRun)
            {
                if (Ped.IsInHelicopter)
                {
                    NativeFunction.Natives.TASK_HELI_CHASE(Ped.Pedestrian, Player.Character, 25f, 0f, 25f);
                }
                else if (Ped.IsInBoat)
                {
                    NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, Player.Character);
                }
                else
                {
                    if (Ped.Pedestrian.CurrentVehicle.Exists())
                    {
                        unsafe
                        {
                            int lol = 0;
                            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                            //NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION_PED_TARGET", 0, Ped.Pedestrian.CurrentVehicle, Player.Character, 7, Speed, 541327934, 8f, 0f, true);//541327934//4 | 8 | 16 | 32 | 512 | 262144
                            NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION_PED_TARGET", 0, Ped.Pedestrian.CurrentVehicle, Player.Character, 7, Speed, (int)eCustomDrivingStyles.FastEmergency, 4f, 2f, true);
                            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                        }
                    }
                }
            }
            else
            {
                Speed = 30f;
                if (Ped.DistanceToPlayer <= 10f)
                {
                    Speed = 10f;
                }
                if (Ped.IsInHelicopter)
                {
                    NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, 100f);
                    if (Ped.DistanceToPlayer <= 100f && Player.Character.Speed < 20f)//32f)//70 mph
                    {
                        NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 10f);
                    }
                    else
                    {
                        NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 100f);
                    }
                }
                else
                {
                    NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, Speed);
                }
            }
            //NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION_PED_TARGET", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, Player.Character, 7, 30f, 4 | 8 | 16 | 32 | 512 | 262144, 0f, 0f, true);
            EntryPoint.WriteToConsole($"VehicleChase Ped Target: {Ped.Pedestrian.Handle}", 5);
        }
    }




}


/*
 * private void VehicleChase()
    {
        if (Ped.Pedestrian.Exists())
        {
            NeedsUpdates = true;
            if (IsFirstRun)
            {
                NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, 1000f);
                //NativeFunction.Natives.SET_DRIVER_RACING_MODIFIER(Ped.Pedestrian, 1.0f);
                NativeFunction.Natives.SET_DRIVER_AGGRESSIVENESS(Ped.Pedestrian, 100.0f);


                //NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.FastEmergency);

                //NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_DisableBlockFromPursueDuringVehicleChase, true);
                //NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_DisableCruiseInFrontDuringBlockDuringVehicleChase, false);
                //NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_DisableSpinOutDuringVehicleChase, false);

                

                Ped.Pedestrian.BlockPermanentEvents = true;
                Ped.Pedestrian.KeepTasks = true;
                if (Ped.IsInHelicopter)
                {
                    NativeFunction.Natives.TASK_HELI_CHASE(Ped.Pedestrian, Player.Character, -50f, 50f, 60f);
                }
                else if (Ped.IsInBoat)
                {
                    NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, Player.Character);
                }
                else
                {
                    if (ShouldChaseRecklessly && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
                    {
                        IsChasingRecklessly = true;
                        //NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.FullContact, true);
                        //NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, Player.Character);
                        //NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.FullContact, true);
                        //unsafe
                        //{
                        //    int lol = 0;
                        //    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        //    NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION", 0, Ped.Pedestrian.CurrentVehicle, Player.CurrentVehicle.Vehicle, (int)eVehicleMissionType.Ram, 50f, (int)eCustomDrivingStyles.CrazyEmergency, 0f, 2f, true);//8f
                        //    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                        //    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        //    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                        //    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                        //}
                        NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, Player.Character);
                        NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.PIT, true);
                        NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.CrazyEmergency);


                        NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 70f);

                    }
                    else
                    {
                        IsChasingRecklessly = false;
                        NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, Player.Character);
                        NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE(Ped.Pedestrian, 15f);
                        NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.NoContact, true);
                        NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.FastEmergency);


                        NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 70f);
                        


                        //if (Player.WantedLevel < 3)
                        //{
                        //    NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE(Ped.Pedestrian, 8f);
                        //    NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.NoContact, true);
                        //}
                        //else
                        //{
                        //    NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.PIT, true);
                        //}
                        //NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, Player.Character);
                        //if (Player.WantedLevel < 3)//need to do it before or after?
                        //{
                        //    NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE(Ped.Pedestrian, 8f);
                        //    NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.NoContact, true);
                        //}
                        //else
                        //{
                        //    NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.PIT, true);
                        //}
                    }
                    //if (Ped.DistanceToPlayer <= 150f)
                    //{
                    //    if (IsChasingRecklessly)
                    //    {
                    //        NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.CrazyEmergencyClose);
                    //    }
                    //    else
                    //    {
                    //        NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.FastEmergencyClose);
                    //    }
                    //}
                    //else
                    //{
                    //    if (IsChasingRecklessly)
                    //    {
                    //        NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.CrazyEmergency);
                    //    }
                    //    else
                    //    {
                    //        NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.FastEmergency);
                    //    }
                    //}
                    NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 70f);
                }
                IsFirstRun = false;
            }
            else
            {
                NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, 1000f);
                //NativeFunction.Natives.SET_DRIVER_RACING_MODIFIER(Ped.Pedestrian, 1.0f);
                NativeFunction.Natives.SET_DRIVER_AGGRESSIVENESS(Ped.Pedestrian, 100.0f);

                //NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.FastEmergency);




                //NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_DisableBlockFromPursueDuringVehicleChase, true);
                //NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_DisableCruiseInFrontDuringBlockDuringVehicleChase, false);
                //NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_DisableSpinOutDuringVehicleChase, false);

                if (!Ped.IsInHelicopter && !Ped.IsInBoat)
                {
                    if (IsChasingRecklessly != ShouldChaseRecklessly)
                    {
                        if (ShouldChaseRecklessly && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() && Ped.Pedestrian.CurrentVehicle.Exists())
                        {
                            IsChasingRecklessly = true;
                            //NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.FullContact, true);
                            //NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, Player.Character);
                            //NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.FullContact, true);
                            //unsafe
                            //{
                            //    int lol = 0;
                            //    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                            //    NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION", 0, Ped.Pedestrian.CurrentVehicle, Player.CurrentVehicle.Vehicle, (int)eVehicleMissionType.Ram, 50f, (int)eCustomDrivingStyles.CrazyEmergency, 0f, 2f, true);//8f
                            //    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                            //    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                            //    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                            //    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                            //}
                            NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, Player.Character);
                            NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.PIT, true);
                            NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.CrazyEmergency);
                        }
                        else
                        {
                            IsChasingRecklessly = false;
                            //NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE(Ped.Pedestrian, 8f);
                            //NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.NoContact, true);
                            //if(Player.WantedLevel < 3)
                            //{
                            //    NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE(Ped.Pedestrian, 8f);
                            //    NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.NoContact, true);
                            //}
                            //else
                            //{
                            //    NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.PIT, true);
                            //}
                            NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, Player.Character);
                            NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE(Ped.Pedestrian, 15f);
                            //NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.NoContact, true);
                            NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.FastEmergency);


                            //if (Player.WantedLevel < 3)
                            //{
                            //    NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE(Ped.Pedestrian, 8f);
                            //    NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.NoContact, true);
                            //}
                            //else
                            //{
                            //    NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.PIT, true);
                            //}
                        }
                    }
                    if (IsChasingRecklessly)
                    {
                        NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.PIT, true);
                        NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.CrazyEmergency);
                    }
                    else
                    {
                        NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.NoContact, true);
                        NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.FastEmergency);
                    }


                    //if (Ped.DistanceToPlayer <= 150f)
                    //{
                    //    if (IsChasingRecklessly)
                    //    {
                    //        NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.CrazyEmergencyClose);
                    //    }
                    //    else
                    //    {
                    //        NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.FastEmergencyClose);
                    //    }
                    //}
                    //else
                    //{
                    //    if (IsChasingRecklessly)
                    //    {
                    //        NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.CrazyEmergency);
                    //    }
                    //    else
                    //    {
                    //        NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.FastEmergency);
                    //    }
                    //}
                }
                Vector3 CurrentPosition = Ped.Pedestrian.Position;
                IsStuck = LastPosition.DistanceTo2D(CurrentPosition) <= 1.0f;
                if (IsStuck)
                {
                    if (GameTimeGotStuck == 0)
                    {
                        GameTimeGotStuck = Game.GameTime;
                    }
                }
                else
                {
                    GameTimeGotStuck = 0;
                }
                if (IsStuck && Game.GameTime - GameTimeGotStuck >= 3000)
                {
                    EntryPoint.WriteToConsole($"VehicleChase Vehicle Target I AM STUCK!!: {Ped.Pedestrian.Handle}", 5);
                }
                LastPosition = CurrentPosition;
            }
        }
    }*/