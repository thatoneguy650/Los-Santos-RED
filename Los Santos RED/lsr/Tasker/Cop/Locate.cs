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
        SubTaskName = "";
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            //EntryPoint.WriteToConsole($"TASKER: Locate Start: {Ped.Pedestrian.Handle}");
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
                //EntryPoint.WriteToConsole($"      Locate SubTask Changed: {Ped.Pedestrian.Handle} to {CurrentTask} {CurrentDynamic}");
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
        if (Ped.Pedestrian.Exists())
        {
            if (Ped.Pedestrian.IsInAnyVehicle(false) && Ped.Pedestrian.CurrentVehicle.Exists())
            {
                if (Ped.IsDriver)
                {
                    NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 30f, (int)VehicleDrivingFlags.Emergency, 10f);
                }
            }
            else
            {
                //Ped.Pedestrian.Tasks.Wander();
                NativeFunction.Natives.TASK_WANDER_STANDARD(Ped.Pedestrian, 0, 0);
            }
            //EntryPoint.WriteToConsole(string.Format("Locate Began SearchingPosition: {0}", Ped.Pedestrian.Handle),5);
        }
    }
    private void GoTo()
    {
        if (Ped.Pedestrian.Exists())
        {
            NeedsUpdates = true;
            if (CurrentTaskedPosition.DistanceTo2D(Player.PlacePoliceLastSeenPlayer) >= 5f && !HasReachedReportedPosition)
            {
                HasReachedReportedPosition = false;
                CurrentTaskedPosition = Player.PlacePoliceLastSeenPlayer;
                if (Ped.Pedestrian.IsInAnyVehicle(false))
                {
                    if (Ped.IsDriver)
                    {
                        if (Ped.IsInHelicopter)
                        {
                            NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, 0, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 4, 50f, 150f, -1f, -1, 30, -1.0f, 0);//NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, 0, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 4, 50f, 10f, 0f, -1, -1, -1, 0);
                        }
                        else if (Ped.IsInBoat)
                        {
                            NativeFunction.Natives.TASK_BOAT_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, 0, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 4, 50f, (int)VehicleDrivingFlags.Emergency, -1.0f, 7);
                        }
                        else
                        {
                            NativeFunction.Natives.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 30f, (int)VehicleDrivingFlags.Emergency, 10f); 
                        }
                    }
                }
                else
                {
                    //Ped.Pedestrian.Tasks.GoStraightToPosition(CurrentTaskedPosition, 15f, 0f, 2f, 0);
                    NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Ped.Pedestrian, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 15f, -1, 0f, 0f);
                }
                //EntryPoint.WriteToConsole(string.Format("Locate Position Updated: {0}", Ped.Pedestrian.Handle),5);
            }
            float DistanceToCoordinates = Ped.Pedestrian.DistanceTo2D(CurrentTaskedPosition);
            if (Ped.Pedestrian.IsInAirVehicle)
            {
                if (DistanceToCoordinates <= 150f)
                {
                   NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 10f);
                }
                else
                {
                    NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 50f);
                    
                }
            }
            if (DistanceToCoordinates <= 25f)
            {
                HasReachedReportedPosition = true;
            }
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
    public override void Stop()
    {

    }
}

