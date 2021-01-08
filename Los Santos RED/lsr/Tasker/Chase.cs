using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Chase : ComplexTask
{
    private bool NeedsUpdates;
    private SubTask CurrentSubTask;
    private Task CurrentTask = Task.Nothing;
    private Vehicle CopsVehicle;

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
    private enum SubTask
    {
        Shoot,
        Aim,
        Goto,
    }
    private bool ShouldChaseVehicleInVehicle => Cop.IsDriver && Cop.Pedestrian.CurrentVehicle.Exists() && !ShouldExitPoliceVehicle && Player.CurrentVehicle != null;
    private bool ShouldChasePedInVehicle => Cop.DistanceToPlayer >= 25f;
    private bool ShouldGetBackInCar => CopsVehicle.Exists() && Cop.Pedestrian.DistanceTo2D(CopsVehicle) <= 30f && CopsVehicle.IsDriveable && CopsVehicle.FreeSeatsCount > 0;
    private bool ShouldCarJackPlayer => Player.CurrentVehicle.Vehicle.Exists() && !Player.IsMovingFast && !Cop.Pedestrian.IsGettingIntoVehicle;
    private bool ShouldExitPoliceVehicle => Cop.DistanceToPlayer < 25f && Cop.Pedestrian.CurrentVehicle.Exists() && Cop.Pedestrian.CurrentVehicle.Speed <= 2.0f && !Player.IsMovingFast;
    private bool ShouldShoot => !Player.IsBusted && !Player.IsAttemptingToSurrender;
    private Task CurrentTaskDynamic
    {
        get
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
                else if(ShouldGetBackInCar)
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
    }

    public Chase(IComplexTaskable cop, ITargetable player) : base(player, cop, 500)
    {
        Name = "Chase";
    }
    public override void Start()
    {
        NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_CLIMBOVERS", Cop.Pedestrian, true);
        NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_LADDERS", Cop.Pedestrian, true);
        NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_DROP_FROM_HEIGHT", Cop.Pedestrian, true);
        Update();
    }
    public override void Update()
    {

        if (Cop.Pedestrian.Exists() && ShouldUpdate)
        {
            if(Cop.Pedestrian.IsInAnyPoliceVehicle)
            {
                CopsVehicle = Cop.Pedestrian.CurrentVehicle;
            }
            if (CurrentTask != CurrentTaskDynamic)
            {
                CurrentTask = CurrentTaskDynamic;
                Game.Console.Print($"      Chase SubTask Changed: {Cop.Pedestrian.Handle} to {CurrentTask} {CurrentDynamic}");
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
            }
            GameTimeLastRan = Game.GameTime;
        }

    }
    private void ExecuteCurrentSubTask()
    {
        if (CurrentTask == Task.CarJack)
        {
            GoToPlayersCar();
        }
        else if (CurrentTask == Task.EnterVehicle)
        {
            EnterVehicle();
        }
        else if (CurrentTask == Task.ExitVehicle)
        {
            ExitVehicle();
        }
        else if (CurrentTask == Task.FootChase)
        {
            FootChase();
        }
        else if (CurrentTask == Task.VehicleChase)
        {
            VehicleChase();
        }
        else if (CurrentTask == Task.VehicleChasePed)
        {
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
            NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.Character, -1, 7f, 500f, 1073741824, 1); //Original and works ok
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.Pedestrian, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
    }
    private void EnterVehicle()
    {
        NeedsUpdates = false;
        if(CopsVehicle.Exists())
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
        NeedsUpdates = false;
        Cop.Pedestrian.BlockPermanentEvents = true;
        Cop.Pedestrian.KeepTasks = true;
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION", 0, Cop.Pedestrian.CurrentVehicle, Player.CurrentVehicle.Vehicle, 7, 45f, 4 | 8 | 16 | 32 | 512 | 262144, 0f, 0f, true);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.Pedestrian, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
        //NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, Player.CurrentVehicle.Vehicle, 7, 45f, 4 | 8 | 16 | 32 | 512 | 262144, 0f, 0f, true);
        Game.Console.Print($"VehicleChase Vehicle Target: {Cop.Pedestrian.Handle}");
    }
    private void VehicleChasePed()
    {
        NeedsUpdates = false;
        Cop.Pedestrian.BlockPermanentEvents = true;
        Cop.Pedestrian.KeepTasks = true;
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION_PED_TARGET", 0, Cop.Pedestrian.CurrentVehicle, Player.Character, 7, 30f, 4 | 8 | 16 | 32 | 512 | 262144, 0f, 0f, true);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.Pedestrian, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
        //NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION_PED_TARGET", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, Player.Character, 7, 30f, 4 | 8 | 16 | 32 | 512 | 262144, 0f, 0f, true);
        Game.Console.Print($"VehicleChase Ped Target: {Cop.Pedestrian.Handle}");
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

