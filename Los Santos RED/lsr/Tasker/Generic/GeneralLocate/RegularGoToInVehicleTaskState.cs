using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RegularGoToInVehicleTaskState : TaskState
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
    private eCustomDrivingStyles DrivingStyle;
    private float DrivingSpeed;

    public RegularGoToInVehicleTaskState(PedExt pedGeneral, ITargetable player, IEntityProvideable world, SeatAssigner seatAssigner, ISettingsProvideable settings, bool blockPermanentEvents, Vector3 placetoDriveTo, ILocationReachable locationReachable,
        eCustomDrivingStyles drivingStyle, float speed)
    {
        PedGeneral = pedGeneral;
        Player = player;
        World = world;
        SeatAssigner = seatAssigner;
        Settings = settings;
        BlockPermanentEvents = blockPermanentEvents;
        PlaceToDriveTo = placetoDriveTo;
        LocationReachable = locationReachable;
        DrivingStyle = drivingStyle;
        DrivingSpeed = speed;
    }
    public bool IsValid => PedGeneral != null && PedGeneral.IsInVehicle && !LocationReachable.HasReachedLocatePosition;
    public string DebugName => $"RegularGoToInVehicleTaskState";
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
    }

    private void CheckTasks()
    {
        Rage.TaskStatus taskStatus = PedGeneral.Pedestrian.Tasks.CurrentTaskStatus;
        if (PedGeneral.IsDriver && (taskStatus == Rage.TaskStatus.NoTask || taskStatus == Rage.TaskStatus.Preparing) && Game.GameTime - GametimeLastRetasked >= 2000)
        {
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
        if (PedGeneral == null || !PedGeneral.Pedestrian.Exists() || !PedGeneral.Pedestrian.CurrentVehicle.Exists() || PlaceToDriveTo == null || PlaceToDriveTo == Vector3.Zero || !PedGeneral.IsDriver)
        {
            return;
        }
        if (PedGeneral.IsInHelicopter)
        {
            NativeFunction.Natives.TASK_HELI_MISSION(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, 0, 0, PlaceToDriveTo.X, PlaceToDriveTo.Y, PlaceToDriveTo.Z, 6, DrivingSpeed, 10f, -1f, 60, 60, -1.0f, 0);//6 = attack
        }
        else if (PedGeneral.IsInBoat)
        {
            NativeFunction.Natives.TASK_BOAT_MISSION(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, 0, 0, PlaceToDriveTo.X, PlaceToDriveTo.Y, PlaceToDriveTo.Z, 4, DrivingSpeed, (int)DrivingStyle, -1.0f, 7);
        }
        else
        {
            NativeFunction.Natives.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, PlaceToDriveTo.X, PlaceToDriveTo.Y, PlaceToDriveTo.Z, DrivingSpeed, (int)DrivingStyle, 10f); //30f speed
        }
        GametimeLastRetasked = Game.GameTime;
    }
    private void CheckGoToDistances()
    {
        float DistanceToCoordinates = PedGeneral.Pedestrian.DistanceTo2D(PlaceToDriveTo);
        if (DistanceToCoordinates <= 20f)
        {
            LocationReachable.OnLocationReached();
        }
    }
}

