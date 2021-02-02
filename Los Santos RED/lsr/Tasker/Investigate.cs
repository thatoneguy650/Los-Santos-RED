using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Investigate : ComplexTask
{
    private bool NeedsUpdates;
    private Vector3 CurrentTaskedPosition;
    private Task CurrentTask = Task.Nothing;
    private bool HasReachedReportedPosition;
    private enum Task
    {
        Wander,
        GoTo,
        Nothing,
    }
    private Task CurrentTaskDynamic
    {
        get
        {
            if (HasReachedReportedPosition)
            {
                return Task.Wander;
            }
            else
            {
                return Task.GoTo;
            }
        }
    }
    public Investigate(IComplexTaskable cop, ITargetable player) : base(player, cop, 1000)
    {
        Name = "Investigate";
        SubTaskName = "";
    }
    public override void Start()
    {
        Game.Console.Print($"TASKER: Investigate Start: {Cop.Pedestrian.Handle}");
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
                Game.Console.Print($"      Investigate SubTask Changed: {Cop.Pedestrian.Handle} to {CurrentTask}");
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
            SubTaskName = "Wander";
            Wander();
        }
        else if (CurrentTask == Task.GoTo)
        {
            SubTaskName = "GoTo";
            GoTo();
        }
        GameTimeLastRan = Game.GameTime;
    }
    private void Wander()
    {
        NeedsUpdates = false;
        if (Cop.Pedestrian.Exists())
        {
            if (Cop.Pedestrian.IsInAnyVehicle(false) && Cop.Pedestrian.CurrentVehicle.Exists())
            {
                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, Player.PoliceResponse.ResponseDrivingSpeed, 4 | 16 | 32 | 262144, 10f);
            }
            else
            {
                Cop.Pedestrian.Tasks.Wander();
            }
            Game.Console.Print(string.Format("Investigation Began SearchingPosition: {0}", Cop.Pedestrian.Handle));
        }
    }
    private void GoTo()
    {
        NeedsUpdates = true;
        if (CurrentTaskedPosition.DistanceTo2D(Player.Investigation.Position) >= 5f)
        {
            HasReachedReportedPosition = false;
            CurrentTaskedPosition = Player.Investigation.Position;
            if (Cop.Pedestrian.IsInAnyVehicle(false))
            {
                if (Cop.IsDriver)
                {
                    NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, Player.PoliceResponse.ResponseDrivingSpeed, 4 | 16 | 32 | 262144, 10f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, PositionOfInterest.X, PositionOfInterest.Y, PositionOfInterest.Z, 70f, 4 | 16 | 32 | 262144, 35f);
                }
            }
            else
            {
                NativeFunction.CallByName<bool>("TASK_GO_STRAIGHT_TO_COORD", Cop.Pedestrian, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 500f, -1, 0f, 2f);
            }
            Game.Console.Print(string.Format("Investigation Position Updated: {0}", Cop.Pedestrian.Handle));
        }
        if(Cop.Pedestrian.DistanceTo2D(CurrentTaskedPosition) <= 25f)
        {
            HasReachedReportedPosition = true;
            Game.Console.Print(string.Format("Investigation Position Reached: {0}", Cop.Pedestrian.Handle));
        }
        Game.Console.Print(string.Format("Investigation Updated No Change: {0}", Cop.Pedestrian.Handle));
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

