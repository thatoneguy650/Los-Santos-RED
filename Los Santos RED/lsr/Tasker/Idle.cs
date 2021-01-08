using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Idle : ComplexTask
{
    private bool NeedsUpdates;
    private Task CurrentTask = Task.Nothing;
    private enum Task
    {
        GetInCar,
        Wander,
        Nothing,
    }
    private Task CurrentTaskDynamic
    {
        get
        {
            if (Cop.DistanceToPlayer <= 75f && Cop.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask && !Cop.Pedestrian.IsInAnyVehicle(false) && Cop.Pedestrian.LastVehicle.Exists() && Cop.Pedestrian.LastVehicle.IsDriveable && Cop.Pedestrian.LastVehicle.FreeSeatsCount > 0)
            {
                return Task.GetInCar;
            }
            else
            {
                return Task.Wander;
            }
        }
    }
    public Idle(IComplexTaskable cop, ITargetable player) : base(player, cop, 1000)
    {
        Name = "Idle";
    }
    public override void Start()
    {
        Cop.Pedestrian.BlockPermanentEvents = false;
        Update();
    }
    public override void Update()
    {
        if (Cop.Pedestrian.Exists() && ShouldUpdate)
        {
            if (CurrentTask != CurrentTaskDynamic)
            {
                CurrentTask = CurrentTaskDynamic;
                Game.Console.Print($"      Idle SubTask Changed: {Cop.Pedestrian.Handle} to {CurrentTask} {CurrentDynamic}");
                ExecuteCurrentSubTask();
            }
            else if (NeedsUpdates)
            {
                ExecuteCurrentSubTask();
            }
            SetSiren();
        }
    }
    private void ExecuteCurrentSubTask()
    {
        if (CurrentTask == Task.Wander)
        {
            Wander();
        }
        else if (CurrentTask == Task.GetInCar)
        {
            GetInCar();
        }
        GameTimeLastRan = Game.GameTime;
    }
    private void Wander()
    {
        NeedsUpdates = false;
        if (Cop.Pedestrian.Exists())
        {
            if (Cop.Pedestrian.IsInAnyVehicle(false) && Cop.IsDriver && Cop.Pedestrian.CurrentVehicle.Exists())
            {
                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, 15f, (int)VehicleDrivingFlags.Normal, 10f);
            }
            else
            {
                Cop.Pedestrian.Tasks.Wander();
            }
            Game.Console.Print(string.Format("Idle Began Wander: {0}", Cop.Pedestrian.Handle));
        }
    }
    private void GetInCar()
    {
        NeedsUpdates = false;
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, Cop.Pedestrian.LastVehicle, -1, Cop.LastSeatIndex, 1f, 9);
            NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(4000, 8000));
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.Pedestrian, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
        Game.Console.Print(string.Format("Idle Began GetInCar: {0}", Cop.Pedestrian.Handle));
    }
    private void SetSiren()
    {
        if (Cop.Pedestrian.CurrentVehicle.Exists() && Cop.Pedestrian.CurrentVehicle.HasSiren && !Cop.Pedestrian.CurrentVehicle.IsSirenOn)
        {
            Cop.Pedestrian.CurrentVehicle.IsSirenOn = false;
            Cop.Pedestrian.CurrentVehicle.IsSirenSilent = false;
        }
    }
    public override void Stop()
    {

    }
}

