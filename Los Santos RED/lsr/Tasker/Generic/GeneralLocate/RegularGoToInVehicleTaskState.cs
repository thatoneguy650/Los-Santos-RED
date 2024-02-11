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
    private float PlaceToDriveToHeading;
    private bool isSetCode3Close;
    private ILocationReachable LocationReachable;
    private uint GametimeLastRetasked;
    private eCustomDrivingStyles DrivingStyle;
    private float DrivingSpeed;

    public RegularGoToInVehicleTaskState(PedExt pedGeneral, ITargetable player, IEntityProvideable world, SeatAssigner seatAssigner, ISettingsProvideable settings, bool blockPermanentEvents, Vector3 placetoDriveTo, float placeToDriveToHeading , ILocationReachable locationReachable,
        eCustomDrivingStyles drivingStyle, float speed)
    {
        PedGeneral = pedGeneral;
        Player = player;
        World = world;
        SeatAssigner = seatAssigner;
        Settings = settings;
        BlockPermanentEvents = blockPermanentEvents;
        PlaceToDriveTo = placetoDriveTo;
        PlaceToDriveToHeading = placeToDriveToHeading;
        LocationReachable = locationReachable;
        DrivingStyle = drivingStyle;
        DrivingSpeed = speed;
    }
    public bool IsValid => PedGeneral != null && PedGeneral.IsInVehicle;// && !LocationReachable.HasReachedLocatePosition;
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
        //Rage.TaskStatus taskStatus = PedGeneral.Pedestrian.Tasks.CurrentTaskStatus;
        //if (PedGeneral.IsDriver && (taskStatus == Rage.TaskStatus.NoTask) && Game.GameTime - GametimeLastRetasked >= 4000)
        //{
        //    TaskEntry();
        //    GametimeLastRetasked = Game.GameTime;
        //    EntryPoint.WriteToConsole($"RAGULAR GO TO  TASK: TAXI {PedGeneral?.Handle} RETASKED");
        //}
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
        PedGeneral.Pedestrian.KeepTasks = true;
        if (PedGeneral == null || !PedGeneral.Pedestrian.Exists() || !PedGeneral.Pedestrian.CurrentVehicle.Exists() || PlaceToDriveTo == null || PlaceToDriveTo == Vector3.Zero || !PedGeneral.IsDriver)
        {
            return;
        }
        if (PedGeneral.IsInHelicopter)
        {
            NativeFunction.Natives.TASK_HELI_MISSION(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, 0, 0, PlaceToDriveTo.X, PlaceToDriveTo.Y, PlaceToDriveTo.Z, 6, DrivingSpeed, 10f, -1f, 60, 60, -1.0f, 0);//6 = attack
        }
        else if (PedGeneral.IsInPlane)
        {
            NativeFunction.Natives.TASK_PLANE_MISSION(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, 0, 0, PlaceToDriveTo.X, PlaceToDriveTo.Y, PlaceToDriveTo.Z, 6, 70f, 40, -1.0f, 40, 20, true);
        }
        else if (PedGeneral.IsInBoat)
        {
            NativeFunction.Natives.TASK_BOAT_MISSION(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, 0, 0, PlaceToDriveTo.X, PlaceToDriveTo.Y, PlaceToDriveTo.Z, 4, DrivingSpeed, (int)DrivingStyle, -1.0f, 7);
        }
        else
        {

            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", 0, PedGeneral.Pedestrian.CurrentVehicle, PlaceToDriveTo.X, PlaceToDriveTo.Y, PlaceToDriveTo.Z, DrivingSpeed, (int)DrivingStyle, 20f);


                NativeFunction.CallByName<bool>("TASK_VEHICLE_PARK", 0, PedGeneral.Pedestrian.CurrentVehicle, PlaceToDriveTo.X, PlaceToDriveTo.Y, PlaceToDriveTo.Z, PlaceToDriveToHeading, 3, 20f, false);


                NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", 0, PedGeneral.Pedestrian.CurrentVehicle, 6, 9999999);
               // NativeFunction.CallByName<bool>("TASK_PAUSE", 0, 99999);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", PedGeneral.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }




            //NativeFunction.Natives.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, PlaceToDriveTo.X, PlaceToDriveTo.Y, PlaceToDriveTo.Z, DrivingSpeed, (int)DrivingStyle, 10f); //30f speed
        }
        GametimeLastRetasked = Game.GameTime;
    }
    //private enum PARK_TYPE
    //{
    //    PARK_TYPE_PARALLEL,
    //    PARK_TYPE_PERPENDICULAR_NOSE_IN,
    //    PARK_TYPE_PERPENDICULAR_BACK_IN,
    //    PARK_TYPE_PULL_OVER,
    //    PARK_TYPE_LEAVE_PARALLEL_SPACE,
    //    PARK_TYPE_BACK_OUT_PERPENDICULAR_SPACE,
    //    PARK_TYPE_PASSENGER_EXIT,
    //    PARK_TYPE_PULL_OVER_IMMEDIATE
    //}
    private void CheckGoToDistances()
    {
        float DistanceToCoordinates = PedGeneral.Pedestrian.DistanceTo2D(PlaceToDriveTo);
        if (DistanceToCoordinates <= 10f)
        {
            LocationReachable.OnLocationReached();
        }
    }
}

