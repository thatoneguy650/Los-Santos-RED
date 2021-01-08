using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Locate : ComplexTask
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
    public Locate(IComplexTaskable cop, ITargetable player) : base(player, cop, 1000)
    {
        Name = "Locate";
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
                Game.Console.Print($"      Locate SubTask Changed: {Cop.Pedestrian.Handle} to {CurrentTask} {CurrentDynamic}");
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
        else if (CurrentTask == Task.GoTo)
        {
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
                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, 30f, (int)VehicleDrivingFlags.Emergency, 10f);
            }
            else
            {
                Cop.Pedestrian.Tasks.Wander();
            }
            Game.Console.Print(string.Format("Locate Began SearchingPosition: {0}", Cop.Pedestrian.Handle));
        }
    }
    private void GoTo()
    {
        NeedsUpdates = true;
        if (CurrentTaskedPosition.DistanceTo2D(Player.PlacePoliceLastSeenPlayer) >= 5f)
        {
            HasReachedReportedPosition = false;
            CurrentTaskedPosition = Player.PlacePoliceLastSeenPlayer;
            if (Cop.Pedestrian.IsInAnyVehicle(false))
            {
                if (Cop.IsDriver)
                {
                    NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 30f, (int)VehicleDrivingFlags.Emergency, 10f);
                }
            }
            else
            {
                Cop.Pedestrian.Tasks.GoStraightToPosition(CurrentTaskedPosition, 15f, 0f, 2f, 0);
            }
            Game.Console.Print(string.Format("Locate Position Updated: {0}", Cop.Pedestrian.Handle));
        }
        if (Cop.Pedestrian.DistanceTo2D(CurrentTaskedPosition) <= 20f)
        {
            HasReachedReportedPosition = true;
        }
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

