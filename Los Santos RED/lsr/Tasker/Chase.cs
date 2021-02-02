using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Helper;


public class Chase : ComplexTask
{
    private bool NeedsUpdates;
    private SubTask CurrentSubTask;
    private Task CurrentTask = Task.Nothing;
    private Vehicle CopsVehicle;
    private bool IsGoingFast;
    private bool IsStuck;
    private Vector3 LastPosition;
    private bool IsFirstRun;
    private uint GameTimeGotStuck;
    private uint GameTimeVehicleStoppedMoving;
    private uint GameTimeChaseStarted;

    private enum Task
    {
        VehicleChase,
        VehicleChasePed,
        ExitVehicle,
        EnterVehicle,
        CarJack,
        FootChase,
        Nothing,
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
    }
    private bool ShouldChaseVehicleInVehicle => Cop.IsDriver && Cop.Pedestrian.CurrentVehicle.Exists() && !ShouldExitPoliceVehicle && Player.CurrentVehicle != null;
    private bool ShouldChasePedInVehicle => Cop.DistanceToPlayer >= 25f;
    private bool ShouldGetBackInCar => CopsVehicle.Exists() && Cop.Pedestrian.DistanceTo2D(CopsVehicle) <= 30f && CopsVehicle.IsDriveable && CopsVehicle.FreeSeatsCount > 0;
    private bool ShouldCarJackPlayer => Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() && !Player.IsMovingFast;// && !Cop.Pedestrian.IsGettingIntoVehicle;
    private bool ShouldExitPoliceVehicle => Cop.DistanceToPlayer < 25f && Cop.Pedestrian.CurrentVehicle.Exists() && VehicleIsStopped && !Player.IsMovingFast && !ChaseRecentlyStarted;
    private bool ChaseRecentlyStarted => GameTimeChaseStarted != 0 &&  Game.GameTime - GameTimeChaseStarted <= 3000;
    private bool VehicleIsStopped => GameTimeVehicleStoppedMoving != 0 && Game.GameTime - GameTimeVehicleStoppedMoving >= 2000;
    private bool ShouldShoot => !Player.IsBusted && !Player.IsAttemptingToSurrender;
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
            if (Cop.IsDriver)
            {
                if (ShouldChasePedInVehicle)
                {
                    return Task.VehicleChasePed;
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
            return Task.FootChase;
        }
        else
        {
            return Task.Nothing;
        }
    }

    public Chase(IComplexTaskable cop, ITargetable player) : base(player, cop, 150)//was 500
    {
        Name = "Chase";
        SubTaskName = "";
    }
    public override void Start()
    {
        Game.Console.Print($"TASKER: Chase Start: {Cop.Pedestrian.Handle} InVeh: {Cop.IsInVehicle} InVeh2: {Cop.Pedestrian.IsInAnyVehicle(false)}");
        GameTimeChaseStarted = Game.GameTime;
        NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_CLIMBOVERS", Cop.Pedestrian, true);
        NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_LADDERS", Cop.Pedestrian, true);
        NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_DROP_FROM_HEIGHT", Cop.Pedestrian, true);
        Update();
    }
    public override void Update()
    {

        if (Cop.Pedestrian.Exists() && ShouldUpdate)
        {
            if (Cop.Pedestrian.IsInAnyPoliceVehicle)
            {
                CopsVehicle = Cop.Pedestrian.CurrentVehicle;
            }
            Task UpdatedTask = GetCurrentTaskDynamic();
            if (CurrentTask != UpdatedTask)
            {
                IsFirstRun = true;
                CurrentTask = UpdatedTask;
                Game.Console.Print($"TASKER: Chase SubTask Changed: {Cop.Pedestrian.Handle} to {CurrentTask} {CurrentDynamic}");
                ExecuteCurrentSubTask();
            }
            else if (NeedsUpdates)
            {
                ExecuteCurrentSubTask();
            }
            if (CurrentTask == Task.VehicleChase || CurrentTask == Task.VehicleChasePed)
            {
                NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.Pedestrian, 32, true);
                SetSiren();
                if (Cop.IsInVehicle)
                {
                    if (Cop.Pedestrian.CurrentVehicle.Speed == 0f)
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
        Game.Console.Print($"TASKER: Chase UpdateEnd: {Cop.Pedestrian.Handle} InVeh: {Cop.IsInVehicle} InVeh2: {Cop.Pedestrian.IsInAnyVehicle(false)}");
    }
    private void ExecuteCurrentSubTask()
    {
        if (CurrentTask == Task.CarJack)
        {
            SubTaskName = "CarJack";
            GoToPlayersCar();
        }
        else if (CurrentTask == Task.EnterVehicle)
        {
            SubTaskName = "EnterVehicle";
            EnterVehicle();
        }
        else if (CurrentTask == Task.ExitVehicle)
        {
            SubTaskName = "ExitVehicle";
            ExitVehicle();
        }
        else if (CurrentTask == Task.FootChase)
        {
            SubTaskName = "FootChase";
            FootChase();
        }
        else if (CurrentTask == Task.VehicleChase)
        {
            SubTaskName = "VehicleChase";
            VehicleChase();
        }
        else if (CurrentTask == Task.VehicleChasePed)
        {
            SubTaskName = "VehicleChasePed";
            VehicleChasePed();
        }
        else if (CurrentTask == Task.Nothing)
        {
            SubTaskName = "Nothing";
            VehicleChasePed();
        }
        GameTimeLastRan = Game.GameTime;
    }

    private void GoToPlayersCar()
    {
        NeedsUpdates = false;
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.CurrentVehicle.Vehicle, -1, 7f, 500f, 1073741824, 1); //Original and works ok
            NativeFunction.CallByName<bool>("TASK_OPEN_VEHICLE_DOOR", 0, Player.CurrentVehicle.Vehicle, -1, -1, 7f); //doesnt really work
          //  NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, Player.CurrentVehicle.Vehicle, -1, Player.Character.SeatIndex, 5.0f, 9);//caused them to get confused about getting back in thier car
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.Pedestrian, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
    }
    private void EnterVehicle()
    {
        NeedsUpdates = false;
        if (CopsVehicle.Exists())
        {
            NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", Cop.Pedestrian, CopsVehicle, -1, Cop.LastSeatIndex, 2.0f, 9);
        }
        Game.Console.Print(string.Format("Started Enter Old Car: {0}", Cop.Pedestrian.Handle));

    }
    private void ExitVehicle()
    {
        NeedsUpdates = false;
        if (Cop.Pedestrian.CurrentVehicle.Exists())
        {
            //NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, 27, 2000);
            //NativeFunction.CallByName<bool>("TASK_LEAVE_VEHICLE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, 256);
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", 0, Cop.Pedestrian.CurrentVehicle, 27, 1000);
                NativeFunction.CallByName<bool>("TASK_LEAVE_VEHICLE", 0, Cop.Pedestrian.CurrentVehicle, 256);
                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.Character, -1, 7f, 500f, 1073741824, 1); //Original and works ok
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
            Game.Console.Print(string.Format("Started Exit Car: {0}", Cop.Pedestrian.Handle));
        }
    }
    private void FootChase()
    {
        NeedsUpdates = true;
        if (Player.WantedLevel >= 2)
        {
            NativeFunction.CallByName<uint>("SET_PED_MOVE_RATE_OVERRIDE", Cop.Pedestrian, (float)(RandomItems.MyRand.NextDouble() * (1.175 - 1.1) + 1.1));
        }
        Cop.Pedestrian.BlockPermanentEvents = true;
        Cop.Pedestrian.KeepTasks = true;
        if (CurrentSubTask != SubTask.Shoot && ShouldShoot && Cop.DistanceToPlayer <= 7f)
        {
            CurrentSubTask = SubTask.Shoot;
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY", 0, Player.Character, Player.Character, 200f, true, 4.0f, 200f, false, false, (uint)FiringPattern.DelayFireByOneSecond);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
            Game.Console.Print($"FootChase Shoot: {Cop.Pedestrian.Handle}");
        }
        else if (CurrentSubTask != SubTask.Aim && !ShouldShoot && Cop.DistanceToPlayer <= 7f)
        {
            CurrentSubTask = SubTask.Aim;
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", 0, Player.Character, 4f, 20f);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
            Game.Console.Print($"FootChase Aim: {Cop.Pedestrian.Handle}");
        }
        else if (CurrentSubTask != SubTask.Goto && Cop.DistanceToPlayer >= 15f)
        {
            CurrentSubTask = SubTask.Goto;
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.Character, -1, 7f, 500f, 1073741824, 1); //Original and works ok
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
            Game.Console.Print($"FootChase GoTo: {Cop.Pedestrian.Handle}");
        }
    }
    private void VehicleChase()
    {
        NeedsUpdates = true;
        if(IsFirstRun)
        {
            NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Cop.Pedestrian, 100f);
            NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Cop.Pedestrian, RandomItems.GetRandomNumber(3f,10f));
            Cop.Pedestrian.BlockPermanentEvents = true;
            Cop.Pedestrian.KeepTasks = true;
            int DesiredStyle;
            DesiredStyle = (int)eDrivingStyles.AvoidVehicles | (int)eDrivingStyles.AvoidEmptyVehicles | (int)eDrivingStyles.AvoidPeds | (int)eDrivingStyles.AvoidObject | (int)eDrivingStyles.AllowWrongWay | (int)eDrivingStyles.ShortestPath;
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION", 0, Cop.Pedestrian.CurrentVehicle, Player.CurrentVehicle.Vehicle, (int)eVehicleMissionType.Follow, 50f, DesiredStyle, 8f, 0f, true);//7
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
            Game.Console.Print($"VehicleChase Vehicle Target: {Cop.Pedestrian.Handle}");
            IsFirstRun = false;
        }
        else
        {
            //if (Cop.DistanceToPlayer <= 20f)
            //{
            //    Cop.Pedestrian.CurrentVehicle.IsCollisionProof = false;
            //}
            //else
            //{
                Entity Closest = Rage.World.GetClosestEntity(Cop.Pedestrian.CurrentVehicle.GetOffsetPositionFront(10f), 10f, GetEntitiesFlags.ConsiderGroundVehicles | GetEntitiesFlags.ExcludePoliceCars);
                if (Closest != null && Closest.Handle != Cop.Pedestrian.CurrentVehicle.Handle)
                {
                    Cop.Pedestrian.CurrentVehicle.CollisionIgnoredEntity = Closest;
                }
                else
                {
                    Cop.Pedestrian.CurrentVehicle.CollisionIgnoredEntity = null;
                }
                Cop.Pedestrian.CurrentVehicle.IsCollisionProof = true;
          //  }
            Vector3 CurrentPosition = Cop.Pedestrian.Position;
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
                Game.Console.Print($"VehicleChase Vehicle Target I AM STUCK!!: {Cop.Pedestrian.Handle}");
            }
            LastPosition = CurrentPosition;
        }
    }
    private void VehicleChasePed()
    {
        if(Cop.Pedestrian.CurrentVehicle.Exists())
        {
            NeedsUpdates = false;
        }
        else
        {
            NeedsUpdates = true;
            return;
        }

        Cop.Pedestrian.BlockPermanentEvents = true;
        Cop.Pedestrian.KeepTasks = true;
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION_PED_TARGET", 0, Cop.Pedestrian.CurrentVehicle, Player.Character, 7, 7f, 4 | 8 | 16 | 32 | 512 | 262144, 8f, 0f, true);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.Pedestrian, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
        //NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION_PED_TARGET", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, Player.Character, 7, 30f, 4 | 8 | 16 | 32 | 512 | 262144, 0f, 0f, true);
        Game.Console.Print($"VehicleChase Ped Target: {Cop.Pedestrian.Handle}");
    }
    private void Nothing()
    {
        NeedsUpdates = true;
        Cop.Pedestrian.BlockPermanentEvents = true;
        Game.Console.Print($"Nothing Ped Target: {Cop.Pedestrian.Handle}");
    }
    private void SetSiren()
    {
        if (Cop.Pedestrian.CurrentVehicle.Exists() && Cop.Pedestrian.CurrentVehicle.HasSiren && !Cop.Pedestrian.CurrentVehicle.IsSirenOn)
        {
            Cop.Pedestrian.CurrentVehicle.IsSirenOn = true;
            Cop.Pedestrian.CurrentVehicle.IsSirenSilent = false;
        }
    }
    public override void Stop()
    {

    }
}

