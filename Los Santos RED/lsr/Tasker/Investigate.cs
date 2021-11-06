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
        if (Ped.Pedestrian.Exists())
        {
            EntryPoint.WriteToConsole($"TASKER: Investigate Start: {Ped.Pedestrian.Handle}",5);
            Ped.Pedestrian.BlockPermanentEvents = false;
            Update();
        }
    }
    public override void Update()
    {
        if (Ped.Pedestrian.Exists() && ShouldUpdate)
        {
            if (CurrentTask != CurrentTaskDynamic)
            {
                CurrentTask = CurrentTaskDynamic;
                EntryPoint.WriteToConsole($"TASKER:      Investigate SubTask Changed: {Ped.Pedestrian.Handle} to {CurrentTask}",5);
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
        Ped.Pedestrian.BlockPermanentEvents = true;
        Ped.Pedestrian.KeepTasks = true;
        NeedsUpdates = false;
        if (Ped.Pedestrian.Exists())
        {
            if (Ped.Pedestrian.IsInAnyVehicle(false) && Ped.Pedestrian.CurrentVehicle.Exists())
            {
                //4 | 16 | 32 | 262144
                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 12f, (int)VehicleDrivingFlags.Emergency, 10f);
            }
            else
            {
                //Ped.Pedestrian.Tasks.Wander();
                //NativeFunction.Natives.TASK_WANDER_STANDARD(Ped.Pedestrian, 0, 0);
                Vector3 Pos = Ped.Pedestrian.Position;
                NativeFunction.Natives.TASK_WANDER_IN_AREA(Ped.Pedestrian, Pos.X, Pos.Y, Pos.Z, 45f, 0f, 0f);
            }
            EntryPoint.WriteToConsole(string.Format("TASKER: Investigation Began SearchingPosition: {0}", Ped.Pedestrian.Handle),5);
        }
    }
    private void GoTo()
    {
        Ped.Pedestrian.BlockPermanentEvents = true;
        Ped.Pedestrian.KeepTasks = true;
        NeedsUpdates = true;
        if (CurrentTaskedPosition.DistanceTo2D(Player.Investigation.Position) >= 5f)
        {
            HasReachedReportedPosition = false;
            CurrentTaskedPosition = Player.Investigation.Position;
            UpdateGoTo();
            EntryPoint.WriteToConsole(string.Format("TASKER: Investigation Position Updated: {0}", Ped.Pedestrian.Handle),5);
        }
        float DistanceTo = Ped.Pedestrian.DistanceTo2D(CurrentTaskedPosition);
        if (DistanceTo <= 25f)
        {
            HasReachedReportedPosition = true;
            EntryPoint.WriteToConsole(string.Format("TASKER: Investigation Position Reached: {0}", Ped.Pedestrian.Handle),5);
        }
        else if (DistanceTo < 50f)
        {
            UpdateGoTo();
        }
        //EntryPoint.WriteToConsole(string.Format("Investigation Updated No Change: {0}", Ped.Pedestrian.Handle));
    }
    private void UpdateGoTo()
    {
        if (Ped.Pedestrian.IsInAnyVehicle(false))
        {
            if (Ped.IsDriver)
            {
                if (Ped.Pedestrian.DistanceTo2D(CurrentTaskedPosition) >= 50f)
                {
                    NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 20f, (int)VehicleDrivingFlags.Emergency, 20f);
                }
                else
                {
                    NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 12f, (int)VehicleDrivingFlags.Emergency, 20f); //NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 15f, (int)VehicleDrivingFlags.Normal, 20f);
                }
                //4 | 16 | 32 | 262144
                //NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, PositionOfInterest.X, PositionOfInterest.Y, PositionOfInterest.Z, 70f, 4 | 16 | 32 | 262144, 35f);
            }
        }
        else
        {
            NativeFunction.CallByName<bool>("TASK_GO_STRAIGHT_TO_COORD", Ped.Pedestrian, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 500f, -1, 0f, 2f);
        }
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

