using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class GoToInVehicleTaskState : TaskState
{
    private PedExt PedGeneral;
    private IEntityProvideable World;
    private SeatAssigner SeatAssigner;

    private VehicleExt TaskedVehicle;
    private int TaskedSeat;
    private ISettingsProvideable Settings;
    private ITargetable Player;
    private bool BlockPermanentEvents = false;
    private Vector3 PlaceToDriveTo;
    private bool isSetCode3Close;
    private ILocationReachable LocationReachable;
    private uint GametimeLastRetasked;

    public GoToInVehicleTaskState(PedExt pedGeneral, ITargetable player, IEntityProvideable world, SeatAssigner seatAssigner, ISettingsProvideable settings, bool blockPermanentEvents, Vector3 placetoDriveTo, ILocationReachable locationReachable)
    {
        PedGeneral = pedGeneral;
        Player = player;
        World = world;
        SeatAssigner = seatAssigner;
        Settings = settings;
        BlockPermanentEvents = blockPermanentEvents;
        PlaceToDriveTo = placetoDriveTo;
        LocationReachable = locationReachable;
    }
    public bool IsValid => PedGeneral != null && PedGeneral.IsInVehicle && !LocationReachable.HasReachedLocatePosition;
    public string DebugName => $"GoToInVehicleTaskState";
    public void Dispose()
    {
        Stop();
    }
    public void Start()
    {
        PedGeneral.ClearTasks(true);
        TaskEntry();
    }
    public void Stop()
    {
        PedGeneral.ClearTasks(true);
    }
    public void Update()
    {
        if (!PedGeneral.Pedestrian.Exists())
        {
            return;
        }
        CheckTasks();
        CheckGoToDistances();
        SetGoToDrivingStyle();
        if (PedGeneral.IsDriver && (PedGeneral.IsInHelicopter || PedGeneral.IsInPlane))
        {
            PedGeneral.ControlLandingGear();
        }
    }

    private void CheckTasks()
    {
        Rage.TaskStatus taskStatus = PedGeneral.Pedestrian.Tasks.CurrentTaskStatus;
        if (PedGeneral.IsDriver && (taskStatus == Rage.TaskStatus.NoTask || taskStatus == Rage.TaskStatus.Preparing) && Game.GameTime - GametimeLastRetasked >= 2000)
        {
            //PedGeneral.ClearTasks(true);
            TaskEntry();
            GametimeLastRetasked = Game.GameTime;
            EntryPoint.WriteToConsole($"LOCATE TASK: Cop {PedGeneral?.Handle} RETASKED");
        }
    }

    private void TaskEntry()
    {
        if (!PedGeneral.Pedestrian.Exists())
        {
            return;
        }
        if (BlockPermanentEvents)
        {
            PedGeneral.Pedestrian.BlockPermanentEvents = true;
            PedGeneral.Pedestrian.KeepTasks = true;
        }
        bool pedExists = PedGeneral != null && PedGeneral.Pedestrian.Exists();
        bool pedVehicleExists = PedGeneral != null && PedGeneral.Pedestrian.Exists() && PedGeneral.Pedestrian.CurrentVehicle.Exists();
        bool locationEror = PlaceToDriveTo == null || PlaceToDriveTo == Vector3.Zero;
       // Vector3 JitterPlace = PlaceToDriveTo.Around2D(5f);

        if (PedGeneral == null || !PedGeneral.Pedestrian.Exists() || !PedGeneral.Pedestrian.CurrentVehicle.Exists() || PlaceToDriveTo == null || PlaceToDriveTo == Vector3.Zero || !PedGeneral.IsDriver)
        {
            //EntryPoint.WriteToConsole($"LOCATE TASK: Cop {PedGeneral?.Handle} TASK ENTRY ABORTED Driver:{PedGeneral?.IsDriver} pedExists{pedExists} pedVehicleExists {pedVehicleExists} locationEror {locationEror}");
            return;
        }
        if (PedGeneral.IsInHelicopter)
        {
           //EntryPoint.WriteToConsole($"LOCATE TASK: Cop {PedGeneral?.Handle} HELI TASK ASSIGNED");
            NativeFunction.Natives.TASK_HELI_MISSION(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, 0, 0, PlaceToDriveTo.X, PlaceToDriveTo.Y, PlaceToDriveTo.Z, 6, 50f, 10f, -1f, 60, 60, -1.0f, 0);//6 = attack
            //NativeFunction.Natives.TASK_HELI_MISSION(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, 0, 0, PlaceToDriveTo.X, PlaceToDriveTo.Y, PlaceToDriveTo.Z, 4, 50f, 150f, -1f, -1, 30, -1.0f, 0);//NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, 0, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 4, 50f, 10f, 0f, -1, -1, -1, 0);
        }
        else if (PedGeneral.IsInPlane)
        {
            //EntryPoint.WriteToConsole($"LOCATE TASK: Cop {PedGeneral?.Handle} HELI TASK ASSIGNED");
            NativeFunction.Natives.TASK_PLANE_MISSION(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, 0, 0, PlaceToDriveTo.X, PlaceToDriveTo.Y, PlaceToDriveTo.Z, 4, 70f, 40, -1.0f, 40, 20, true);//THIS KINDA WORKS//NativeFunction.Natives.TASK_PLANE_MISSION(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, 0, 0, PlaceToDriveTo.X, PlaceToDriveTo.Y, PlaceToDriveTo.Z, 6, 70f, 40, -1.0f, 40, 20, true);
            //NativeFunction.Natives.TASK_HELI_MISSION(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, 0, 0, PlaceToDriveTo.X, PlaceToDriveTo.Y, PlaceToDriveTo.Z, 4, 50f, 150f, -1f, -1, 30, -1.0f, 0);//NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, 0, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 4, 50f, 10f, 0f, -1, -1, -1, 0);
        }
        else if (PedGeneral.IsInBoat)
        {
            //EntryPoint.WriteToConsole($"LOCATE TASK: Cop {PedGeneral?.Handle} BOAT TASK ASSIGNED");
            NativeFunction.Natives.TASK_BOAT_MISSION(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, 0, 0, PlaceToDriveTo.X, PlaceToDriveTo.Y, PlaceToDriveTo.Z, 4, 50f, (int)eCustomDrivingStyles.Code3, -1.0f, 7);
        }
        else
        {
            //EntryPoint.WriteToConsole($"LOCATE TASK: Cop {PedGeneral?.Handle} DRIVE TASK ASSIGNED");

            //NativeFunction.Natives.TASK_VEHICLE_DRIVE_TO_COORD(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, JitterPlace.X, JitterPlace.Y, JitterPlace.Z, 70f, 0, "", (int)eCustomDrivingStyles.Code3, 15.0f, -1);


            NativeFunction.Natives.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, PlaceToDriveTo.X, PlaceToDriveTo.Y, PlaceToDriveTo.Z, 40.0f, (int)eCustomDrivingStyles.Code3, 10f); //30f speed
        }
        GametimeLastRetasked = Game.GameTime;
    }
    private void SetGoToDrivingStyle()
    {
        if (PedGeneral.IsDriver && !PedGeneral.IsInHelicopter && !PedGeneral.IsInBoat && PedGeneral.DistanceToPlayer <= Settings.SettingsManager.PoliceTaskSettings.DriveBySightDuringLocateDistance && Settings.SettingsManager.PoliceTaskSettings.AllowDriveBySightDuringLocate)// && Player.CurrentLocation.IsOffroad && Player.CurrentLocation.HasBeenOffRoad)
        {
            if (!isSetCode3Close)
            {
                NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(PedGeneral.Pedestrian, (int)eCustomDrivingStyles.Code3Close);
                isSetCode3Close = true;
            }
        }
        else
        {
            if (isSetCode3Close)
            {
                NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(PedGeneral.Pedestrian, (int)eCustomDrivingStyles.Code3);
                isSetCode3Close = false;
            }
        }
    }
    private void CheckGoToDistances()
    {
        float DistanceToCoordinates = PedGeneral.Pedestrian.DistanceTo(PlaceToDriveTo); //PedGeneral.Pedestrian.DistanceTo2D(PlaceToDriveTo);
        if (PedGeneral.Pedestrian.IsInAirVehicle)
        {
            //if (DistanceToCoordinates <= 150f)
            //{
            //    NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(PedGeneral.Pedestrian, 10f);//tr cruise speed test
            //}
            //else
            //{
            //    NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(PedGeneral.Pedestrian, 50f);
            //}
        }
        else
        {
            if (DistanceToCoordinates >= 100)
            {
                NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(PedGeneral.Pedestrian, 40f);//tr cruise speed test
            }
            else if (DistanceToCoordinates >= 45f)
            {
                NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(PedGeneral.Pedestrian, 15f);
            }
            else
            {
                NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(PedGeneral.Pedestrian, 10f);
            }
        }
        if (DistanceToCoordinates <= 20f)
        {
            LocationReachable.OnLocationReached();
            //LocationReachable.HasReachedLocatePosition = true;
           // EntryPoint.WriteToConsole($"LOCATE TASK: Cop {PedGeneral?.Handle} HAS REACHED POSITION");
        }
    }
}

