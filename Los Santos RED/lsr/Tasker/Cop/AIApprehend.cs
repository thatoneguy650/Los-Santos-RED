using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Helper;


public class AIApprehend : ComplexTask
{
    private bool NeedsUpdates;
    private SubTask CurrentSubTask;
    private Task CurrentTask = Task.Nothing;
    private Vehicle CopsVehicle;
    private bool IsStuck;
    private Vector3 LastPosition;
    private bool IsFirstRun;
    private uint GameTimeGotStuck;
    private uint GameTimeVehicleStoppedMoving;
    private uint GameTimeChaseStarted;
    private float ChaseDistance = 5f;
    private int VehicleMissionFlag;
    private bool IsChasingRecklessly;
    private uint CurrentlyChasingHandle;
    private float DistanceToTarget = 999f;
    private bool IsArresting = false;
    private float MoveRate = 2.0f;
    private bool hasOwnFiber;
    private bool IsChasingSlowly;
    private bool prevIsChasingSlowly;

    private enum Task
    {
        VehicleChase,
        VehicleChasePed,
        ExitVehicle,
        EnterVehicle,
        CarJack,
        FootChase,
        Nothing,
        StopCar
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
        Look,
    }
    private bool ShouldChaseRecklessly => OtherTarget.IsDeadlyChase;
    private bool ShouldChaseVehicleInVehicle => Ped.IsDriver && Ped.Pedestrian.CurrentVehicle.Exists() && !ShouldExitPoliceVehicle && OtherTarget.IsInVehicle;
    private bool ShouldChasePedInVehicle => DistanceToTarget >= 30f;//55f
    private bool ShouldGetBackInCar => !Ped.RecentlyGotOutOfVehicle && CopsVehicle.Exists() && Ped.Pedestrian.Exists() && Ped.Pedestrian.DistanceTo2D(CopsVehicle) <= 30f && CopsVehicle.IsDriveable && CopsVehicle.FreeSeatsCount > 0;
    private bool ShouldCarJackTarget => OtherTarget.Pedestrian.CurrentVehicle.Exists() && !OtherTarget.IsMovingFast;
    public bool ShouldStopCar => DistanceToTarget < 30f && Ped.Pedestrian.CurrentVehicle.Exists() && Ped.Pedestrian.CurrentVehicle.Speed > 0.5f && !OtherTarget.IsMovingFast && !ChaseRecentlyStarted && !Ped.IsInHelicopter && !Ped.IsInBoat /* NEW */&& Ped.Pedestrian.CurrentVehicle.Speed < 20f;
    private bool ShouldExitPoliceVehicle => !Ped.RecentlyGotInVehicle && DistanceToTarget < 30f && Ped.Pedestrian.CurrentVehicle.Exists() && Ped.Pedestrian.CurrentVehicle.Speed < 0.5f && !OtherTarget.IsMovingFast && !ChaseRecentlyStarted && !Ped.IsInHelicopter && !Ped.IsInBoat;
    private bool ChaseRecentlyStarted => false;
    private bool ShouldShoot => !OtherTarget.IsBusted && OtherTarget.WantedLevel > 1;
    private bool ShouldAim => OtherTarget.WantedLevel > 1;

    public AIDynamic CurrentAIDynamic
    {
        get
        {
            if (OtherTarget.IsInVehicle)
            {
                if (Ped.IsInVehicle)
                {
                    return AIDynamic.Cop_InVehicle_Player_InVehicle;
                }
                else
                {
                    return AIDynamic.Cop_OnFoot_Player_InVehicle;
                }
            }
            else
            {
                if (Ped.IsInVehicle)
                {
                    return AIDynamic.Cop_InVehicle_Player_OnFoot;
                }
                else
                {
                    return AIDynamic.Cop_OnFoot_Player_OnFoot;
                }
            }
        }
    }
    private Task GetCurrentTaskDynamic()
    {
        if (CurrentAIDynamic == AIDynamic.Cop_InVehicle_Player_InVehicle)
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
        else if (CurrentAIDynamic == AIDynamic.Cop_InVehicle_Player_OnFoot)
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
        else if (CurrentAIDynamic == AIDynamic.Cop_OnFoot_Player_InVehicle)
        {
            if (ShouldCarJackTarget)
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
        else if (CurrentAIDynamic == AIDynamic.Cop_OnFoot_Player_OnFoot)
        {
            if (DistanceToTarget >= 50f && ShouldGetBackInCar)//this is new, was only footchase in here before, cant wait to see the bugs....
            {
                return Task.EnterVehicle;
            }
            else
            {
                return Task.FootChase;
            }
        }
        else
        {
            return Task.Nothing;
        }
    }
    public AIApprehend(IComplexTaskable cop, ITargetable player) : base(player, cop, 500)//was 500
    {
        Name = "AIApprehend";
        SubTaskName = "";
    }
    public AIApprehend(IComplexTaskable cop, ITargetable player, float chaseDistance, int vehicleMissionFlag) : base(player, cop, 500)//was 500
    {
        Name = "AIApprehend";
        SubTaskName = "";
        ChaseDistance = chaseDistance;
        VehicleMissionFlag = vehicleMissionFlag;
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            //EntryPoint.WriteToConsole($"TASKER: AIApprehend Start: {Ped.Pedestrian.Handle} ChaseDistance: {ChaseDistance} VehicleMissionFlag: {VehicleMissionFlag}", 5);
            GameTimeChaseStarted = Game.GameTime;
            NativeFunction.Natives.SET_PED_PATH_CAN_USE_CLIMBOVERS(Ped.Pedestrian, true);
            NativeFunction.Natives.SET_PED_PATH_CAN_USE_LADDERS(Ped.Pedestrian, true);
            NativeFunction.Natives.SET_PED_PATH_CAN_DROP_FROM_HEIGHT(Ped.Pedestrian, true);
            Update();
        }
    }
    public override void Update()
    {
        if (Ped.Pedestrian.Exists() && ShouldUpdate)
        {
            bool TargetChanged = false;
            if(CurrentlyChasingHandle != 0)
            {
                if(OtherTarget != null && OtherTarget.Pedestrian.Exists() && OtherTarget.Handle != CurrentlyChasingHandle)
                {
                    EntryPoint.WriteToConsole($"TASKER: Target Changed From: {CurrentlyChasingHandle} to {OtherTarget.Handle}", 5);
                    CurrentlyChasingHandle = OtherTarget.Handle;
                    TargetChanged = true;   
                }
            }
            if (OtherTarget != null && OtherTarget.Pedestrian.Exists())
            {
                DistanceToTarget = OtherTarget.Pedestrian.DistanceTo2D(Ped.Pedestrian);
                if (Ped.Pedestrian.IsInAnyPoliceVehicle && !CopsVehicle.Exists())
                {
                    CopsVehicle = Ped.Pedestrian.CurrentVehicle;
                }
                Task UpdatedTask = GetCurrentTaskDynamic();
                if (CurrentTask != UpdatedTask || TargetChanged)
                {

                    hasOwnFiber = false;

                    IsFirstRun = true;
                    CurrentTask = UpdatedTask;
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
                    NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.NoContact, true);
                    NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.FastEmergency);
                    SetSiren();
                    if (Ped.Pedestrian.CurrentVehicle.Exists())
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
        }
        //EntryPoint.WriteToConsole($"TASKER: Chase UpdateEnd: {Ped.Pedestrian.Handle} InVeh: {Ped.IsInVehicle} InVeh2: {Ped.Pedestrian.IsInAnyVehicle(false)}");
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
            RunInterval = 200;
            SubTaskName = "EnterVehicle";
            EnterVehicle();
        }
        else if (CurrentTask == Task.ExitVehicle)
        {
            RunInterval = 200;
            SubTaskName = "ExitVehicle";
            ExitVehicle();
        }
        else if (CurrentTask == Task.FootChase)
        {
            RunInterval = 200;
            SubTaskName = "FootChase";
            FootChase();
        }
        else if (CurrentTask == Task.VehicleChase)
        {
            RunInterval = 500;
            SubTaskName = "VehicleChase";
            VehicleChase();
        }
        else if (CurrentTask == Task.VehicleChasePed)
        {
            RunInterval = 500;
            SubTaskName = "VehicleChasePed";
            VehicleChasePed();
        }
        else if (CurrentTask == Task.Nothing)
        {
            RunInterval = 500;
            SubTaskName = "Nothing";
            //VehicleChasePed();
        }
        else if (CurrentTask == Task.StopCar)
        {
            RunInterval = 500;
            SubTaskName = "StopCar";
            StopCar();
        }
        GameTimeLastRan = Game.GameTime;
    }
    private void GoToPlayersCar()
    {
        NeedsUpdates = false;
        if (Ped.Pedestrian.Exists() && OtherTarget.Pedestrian.Exists() && OtherTarget.IsInVehicle && OtherTarget.Pedestrian.CurrentVehicle.Exists())
        {
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, OtherTarget.Pedestrian.CurrentVehicle, -1, 7f, 500f, 1073741824, 1); //Original and works ok
                                                                                                                                        // NativeFunction.CallByName<bool>("TASK_OPEN_VEHICLE_DOOR", 0, Player.CurrentVehicle.Vehicle, -1, -1, 7f); //doesnt really work
                                                                                                                                        //NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, Player.CurrentVehicle.Vehicle, -1, Target.Pedestrian.SeatIndex, 5.0f, 9);//caused them to get confused about getting back in thier car,also has an error with the seat index!
                NativeFunction.CallByName<bool>("TASK_ARREST_PED", 0, OtherTarget.Pedestrian);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
    }
    private void EnterVehicle()
    {
        NeedsUpdates = false;
        if (Ped.Pedestrian.Exists() && CopsVehicle.Exists())
        {
            NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", Ped.Pedestrian, CopsVehicle, -1, Ped.LastSeatIndex, 2.0f, 9);
        }
        //EntryPoint.WriteToConsole(string.Format("Started Enter Old Car: {0}", Ped.Pedestrian.Handle));
    }
    private void ExitVehicle()
    {
        NeedsUpdates = false;
        if (Ped.Pedestrian.Exists() && Ped.Pedestrian.CurrentVehicle.Exists() && OtherTarget.Pedestrian.Exists())
        {
                unsafe
                {
                    int lol = 0;
                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                    NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", 0, Ped.Pedestrian.CurrentVehicle, 27, 1000);
                    NativeFunction.CallByName<bool>("TASK_LEAVE_VEHICLE", 0, Ped.Pedestrian.CurrentVehicle, 256);
                    NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, OtherTarget.Pedestrian, -1, 7f, 500f, 1073741824, 1); //Original and works ok
                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                }
            //EntryPoint.WriteToConsole(string.Format("Started Exit Car: {0}", Ped.Pedestrian.Handle));
        }
    }
    private void FootChase()
    {
        if (OtherTarget != null && OtherTarget.Pedestrian.Exists())
        {
            if (IsFirstRun)
            {
                IsFirstRun = false;
                NeedsUpdates = true;
                FootChaseLoop();
                EntryPoint.WriteToConsole($"COP EVENT: AI Apprehend Start: {Ped.Pedestrian.Handle}", 3);
            }
            //FootChaseLoop();
            //if (OtherTarget.Pedestrian.IsStunned && !OtherTarget.IsBusted && !OtherTarget.IsZombie)
            //{
            //    OtherTarget.IsBusted = true;
            //    if (Ped.Pedestrian.Exists())
            //    {
            //        OtherTarget.ArrestingPedHandle = Ped.Pedestrian.Handle;
            //    }
            //    EntryPoint.WriteToConsole($"Should bust {OtherTarget.Pedestrian.Handle}", 3);
            //}
        }
    }

    private void FootChaseLoop()
    {
        NeedsUpdates = false;
        hasOwnFiber = true;
        Ped.IsRunningOwnFiber = true;
        float MoveRate = 1.25f;//(float)(RandomItems.MyRand.NextDouble() * (1.175 - 1.1) + 1.1);
        float RunSpeed = 500f;
        bool prevIsChasingSlowly = IsChasingSlowly;
        CurrentSubTask = SubTask.None;
        GameFiber.StartNew(delegate
        {
        while (hasOwnFiber && Ped.Pedestrian.Exists() && OtherTarget != null && OtherTarget.Pedestrian.Exists() && Ped.CurrentTask != null & Ped.CurrentTask?.Name == "AIApprehend" && CurrentTask == Task.FootChase)
        {
            if (Ped.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.None || Ped.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask)//might be a error?
            {
                SubTaskName = "";
            }
            if (OtherTarget.WantedLevel == 1)
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



            if (IsArresting && OtherTarget != null && OtherTarget.IsDeadlyChase)
            {
                IsArresting = false;
            }
            else if (!IsArresting && OtherTarget != null && !OtherTarget.IsDeadlyChase)
            {
                IsArresting = true;
            }


            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            float LocalDistance = Ped.Pedestrian.DistanceTo2D(OtherTarget.Pedestrian);

            if (IsArresting)
            {

                if (CurrentSubTask != SubTask.Shoot && ShouldShoot && LocalDistance < 10f && ShouldAim)//7f
                {
                    CurrentSubTask = SubTask.Shoot;
                    if (LocalDistance > 5f)
                    {
                        unsafe
                        {
                            int lol = 0;
                            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                            NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY", 0, OtherTarget.Pedestrian, OtherTarget.Pedestrian, 200f, true, 4.0f, 200f, false, false, (uint)FiringPattern.DelayFireByOneSecond);
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
                            NativeFunction.CallByName<bool>("TASK_SHOOT_AT_ENTITY", 0, OtherTarget.Pedestrian, -1, (uint)FiringPattern.DelayFireByOneSecond);
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
                            NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", 0, OtherTarget.Pedestrian, 4f, 20f);
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
                            NativeFunction.CallByName<bool>("TASK_AIM_GUN_AT_ENTITY", 0, OtherTarget.Pedestrian, -1, false);
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
                    unsafe
                    {
                        int lol = 0;
                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, OtherTarget.Pedestrian, -1, 3f, RunSpeed, 2f, 0);
                        NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, OtherTarget.Pedestrian, 2000);
                        NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, OtherTarget.Pedestrian, -1, 0, 2);
                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    }
                }
                else if (CurrentSubTask != SubTask.Goto && LocalDistance >= 10f)//15f
                {
                    CurrentSubTask = SubTask.Goto;
                    unsafe
                    {
                        int lol = 0;
                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, OtherTarget.Pedestrian, -1, 5f, RunSpeed, 2f, 0); //Original and works ok//7f
                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    }
                }




                if ((OtherTarget.Pedestrian.IsStunned || OtherTarget.Pedestrian.IsRagdoll) && !OtherTarget.IsBusted && LocalDistance <= 5f && !OtherTarget.IsZombie)
                {
                    OtherTarget.IsBusted = true;
                    if (Ped.Pedestrian.Exists())
                    {
                        OtherTarget.ArrestingPedHandle = Ped.Pedestrian.Handle;
                    }
                    if (OtherTarget.CurrentTask?.Name != "GetArrestedSimple")
                    {
                        OtherTarget.CurrentTask = new GetArrestedSimple(OtherTarget, Player);
                        GameFiber.Yield();//TR Added back 7
                        OtherTarget.CurrentTask.Start();
                    }
                    EntryPoint.WriteToConsole($"Should bust {OtherTarget.Pedestrian.Handle}", 3);
                }






            }
            else
            {
                Ped combatTarget = Ped.Pedestrian.CombatTarget;
                bool ISSTUPIDSHIT = combatTarget.Exists() && combatTarget.Handle != OtherTarget.Pedestrian.Handle;

                if(ISSTUPIDSHIT)
                {
                    EntryPoint.WriteToConsole($"RETARD COP ALERT, IM GONNA JUST SHOOT RANDOMLY CUZ IM A FUCKUP! {Ped.Pedestrian.Handle}");
                }
                if (SubTaskName != "Fighting" || ISSTUPIDSHIT)
                {

                    NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
                    NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Ped.Pedestrian, true);
                    //NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_CanChaseTargetOnFoot, false);
                    NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_Aggressive, true);
                    NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_CanUseCover, true);
                    NativeFunction.Natives.TASK_COMBAT_PED(Ped.Pedestrian, OtherTarget.Pedestrian, 0, 16);
                    SubTaskName = "Fighting";
                }
            }
            GameFiber.Yield();
            }
            Ped.IsRunningOwnFiber = false;
        }, "Run Cop Chase Logic");
    }
    private void FootChaseLoopOld()
    {
        if (Ped.Pedestrian.Exists())
        {
            if (Ped.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.None || Ped.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask)//might be a error?
            {
                SubTaskName = "";
            }
            if (IsArresting && OtherTarget != null && OtherTarget.IsDeadlyChase)
            {
                IsArresting = false;
            }
            else if (!IsArresting && OtherTarget != null && !OtherTarget.IsDeadlyChase)
            {
                IsArresting = true;
            }
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;

            NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE<uint>(Ped.Pedestrian, 1.175f);

            //NativeFunction.Natives.SET_PED_SHOOT_RATE(Ped.Pedestrian, 100);//30
            NativeFunction.Natives.SET_PED_ALERTNESS(Ped.Pedestrian, 3);//very altert
            //NativeFunction.Natives.SET_PED_COMBAT_ABILITY(Ped.Pedestrian, 2);//professional
            //NativeFunction.Natives.SET_PED_COMBAT_RANGE(Ped.Pedestrian, 2);//far
            //NativeFunction.Natives.SET_PED_COMBAT_MOVEMENT(Ped.Pedestrian, 2);//offensinve
            if (OtherTarget != null && OtherTarget.Pedestrian.Exists())
            {
                NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE<uint>(Ped.Pedestrian, MoveRate);
                IsReadyForWeaponUpdates = true;
                if (IsArresting)
                {
                    if (OtherTarget.IsInVehicle)
                    {
                        if (SubTaskName != "ArrestingInVehicle")
                        {
                            unsafe
                            {
                                int lol = 0;
                                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                NativeFunction.CallByName<bool>("TASK_ARREST_PED", 0, OtherTarget.Pedestrian);
                                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                            }
                            SubTaskName = "ArrestingInVehicle";
                        }
                    }
                    else
                    {
                        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_CanChaseTargetOnFoot, true);
                        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_Aggressive, true);
                        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_CanUseCover, false);
                        if (DistanceToTarget >= 10f)
                        {
                            if (SubTaskName != "ArrestingFar")
                            {
                                unsafe
                                {
                                    int lol = 0;
                                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                    NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, OtherTarget.Pedestrian, -1, 5f, 200.0f, 2f, 0); //Original and works ok//7f
                                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                                }
                                SubTaskName = "ArrestingFar";
                            }
                        }
                        else
                        {
                            if(OtherTarget.IsBusted)
                            {
                                if (SubTaskName != "ArrestingCloseBusted")
                                {
                                    unsafe
                                    {
                                        int lol = 0;
                                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                        NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", 0, OtherTarget.Pedestrian, 4f, 20f);
                                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                                    }
                                    SubTaskName = "ArrestingCloseBusted";
                                }
                            }
                            else
                            {
                                if (SubTaskName != "ArrestingClose")
                                {
                                    unsafe
                                    {
                                        int lol = 0;
                                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                        NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY", 0, OtherTarget.Pedestrian, OtherTarget.Pedestrian, 200f, true, 4.0f, 200f, false, false, (uint)FiringPattern.FullAutomatic);
                                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                                    }
                                    SubTaskName = "ArrestingClose";
                                }
                            }

                        }
                    }
                }
                else
                {
                    if (SubTaskName != "Fighting")
                    {
                        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Ped.Pedestrian, true);
                        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_CanChaseTargetOnFoot, false);
                        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_Aggressive, true);
                        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_CanUseCover, true);
                        NativeFunction.Natives.TASK_COMBAT_PED(Ped.Pedestrian, OtherTarget.Pedestrian, 0, 16);
                        SubTaskName = "Fighting";
                    }
                }
            }
        }
    }
    private void VehicleChase()
    {
        NeedsUpdates = true;
        if (IsFirstRun)
        {
            NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, 100f);
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            if (Ped.IsInHelicopter)
            {
                NativeFunction.Natives.TASK_HELI_CHASE(Ped.Pedestrian, OtherTarget.Pedestrian, 25f, 25f, 60f);
            }
            else if (Ped.IsInBoat)
            {
                NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, OtherTarget.Pedestrian);
            }
            else
            {
                if (OtherTarget.Pedestrian.Exists() && OtherTarget.Pedestrian.CurrentVehicle.Exists())
                {
                    if (ShouldChaseRecklessly)
                    {
                        IsChasingRecklessly = true;
                        unsafe
                        {
                            int lol = 0;
                            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                            NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION", 0, Ped.Pedestrian.CurrentVehicle, OtherTarget.Pedestrian.CurrentVehicle, (int)eVehicleMissionType.Ram, 50f, (int)eCustomDrivingStyles.FastEmergency, 0f, 2f, true);//8f
                            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                        }
                    }
                    else
                    {
                        IsChasingRecklessly = false;
                        NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, OtherTarget.Pedestrian);
                        NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.CrazyEmergency);
                    }
                }
            }
            EntryPoint.WriteToConsole($"VehicleChase Vehicle Target: {Ped.Pedestrian.Handle} IsChasingRecklessly: {IsChasingRecklessly}", 5);
            IsFirstRun = false;
        }
        else
        {
            NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, 100f);
            if (Ped.IsInHelicopter)
            {
                NativeFunction.Natives.TASK_HELI_CHASE(Ped.Pedestrian, OtherTarget.Pedestrian, 25f, 25f, 60f);
            }
            else if (Ped.IsInBoat)
            {
                NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, OtherTarget.Pedestrian);
            }
            else
            {
                if (OtherTarget.Pedestrian.Exists() && OtherTarget.Pedestrian.CurrentVehicle.Exists())
                {
                    if (IsChasingRecklessly != ShouldChaseRecklessly)
                    {
                        if (ShouldChaseRecklessly)
                        {
                            IsChasingRecklessly = true;
                            unsafe
                            {
                                int lol = 0;
                                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION", 0, Ped.Pedestrian.CurrentVehicle, OtherTarget.Pedestrian.CurrentVehicle, (int)eVehicleMissionType.Ram, 50f, (int)eCustomDrivingStyles.FastEmergency, 0f, 2f, true);//8f
                                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                            }
                        }
                        else
                        {
                            IsChasingRecklessly = false;
                            NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, OtherTarget.Pedestrian);
                            NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.CrazyEmergency);
                        }
                    }
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
    private void VehicleChasePed()
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
        if (DistanceToTarget <= 10f)
        {
            Speed = 10f;
        }
        if (Ped.IsInHelicopter)
        {
            NativeFunction.Natives.TASK_HELI_CHASE(Ped.Pedestrian, OtherTarget.Pedestrian, -50f, 50f, 60f);
        }
        else if (Ped.IsInBoat)
        {
            NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, OtherTarget.Pedestrian);
        }
        else
        {
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION_PED_TARGET", 0, Ped.Pedestrian.CurrentVehicle, OtherTarget.Pedestrian, 7, Speed, (int)eCustomDrivingStyles.FastEmergency, 4f, 2f, true);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
        //NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION_PED_TARGET", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, Target.Pedestrian, 7, 30f, 4 | 8 | 16 | 32 | 512 | 262144, 0f, 0f, true);
        EntryPoint.WriteToConsole($"VehicleChase Ped Target: {Ped.Pedestrian.Handle}", 5);
    }
    private void StopCar()
    {
        if (Ped.Pedestrian.CurrentVehicle.Exists())
        {
            NeedsUpdates = false;
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 27, 2000);
            EntryPoint.WriteToConsole($"AIApprehend Stop Car: {Ped.Pedestrian.Handle}", 5);
        }
        else
        {
            NeedsUpdates = true;
            return;
        }
        EntryPoint.WriteToConsole($"AI Apprehend STOPCAR: {Ped.Pedestrian.Handle}", 5);
    }
    private void SetSiren()
    {
        if (Ped.Pedestrian.CurrentVehicle.Exists() && Ped.Pedestrian.CurrentVehicle.HasSiren && !Ped.Pedestrian.CurrentVehicle.IsSirenOn)
        {
            Ped.Pedestrian.CurrentVehicle.IsSirenOn = true;
            Ped.Pedestrian.CurrentVehicle.IsSirenSilent = false;
        }
    }
    public override void Stop()
    {

    }

}

