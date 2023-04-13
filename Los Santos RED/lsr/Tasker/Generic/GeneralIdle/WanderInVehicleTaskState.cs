using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class WanderInVehicleTaskState : TaskState
{
    private PedExt PedGeneral;
    private IEntityProvideable World;
    private SeatAssigner SeatAssigner;
    private IPlacesOfInterest PlacesOfInterest;
    private bool IsReturningToStation;
    private Vector3 taskedPosition;
    private ISettingsProvideable Settings;
    private bool BlockPermanentEvents = false;
    private bool canGuard;
    private bool canPatrol;
    private bool HasSpawnRequirements;
    private bool IsGuarding;
    private bool IsPatrolling;
    private bool IsReckless;

    public WanderInVehicleTaskState(PedExt pedGeneral, IEntityProvideable world, SeatAssigner seatAssigner, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, bool blockPermanentEvents, bool isReckless)
    {
        PedGeneral = pedGeneral;
        World = world;
        SeatAssigner = seatAssigner;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        BlockPermanentEvents = blockPermanentEvents;
        IsReckless = isReckless;
    }

    public bool IsValid => PedGeneral != null && PedGeneral.Pedestrian.Exists() && PedGeneral.IsInVehicle;
    public string DebugName => $"WanderInVehicleTaskState";
    public void Dispose()
    {
        Stop();
    }
    public void Start()
    {
        PedGeneral.ClearTasks(true);

        IsGuarding = false;
        IsPatrolling = false;
        canGuard = false;
        canPatrol = false;

        if (PedGeneral.LocationTaskRequirements.TaskRequirements.Equals(TaskRequirements.None) || IsReckless)
        {
            canGuard = false;
            canPatrol = true;
        }
        else
        {
            HasSpawnRequirements = true;
            if (PedGeneral.LocationTaskRequirements.TaskRequirements.HasFlag(TaskRequirements.Guard) && PedGeneral.IsNearSpawnPosition)
            {
                canGuard = true;
            }
            else
            {
                canPatrol = true;
            }
            if (PedGeneral.LocationTaskRequirements.TaskRequirements.HasFlag(TaskRequirements.Patrol))
            {
                canPatrol = true;
            }
        }
        if (canGuard)
        {
            IsGuarding = true;
        }
        else if (canPatrol)
        {
            IsPatrolling = true;
        }

        if (IsGuarding)
        {
            VehicleGuard();
        }
        else if (IsPatrolling)
        {
            VehiclePatrol();
        }

        //EntryPoint.WriteToConsoleTestLong($"{PedGeneral?.Handle} START WANDER IsNearSpawnPosition:{PedGeneral?.IsNearSpawnPosition} canGuard{canGuard} canPatrol{canPatrol}");
        //VehiclePatrol();
    }
    public void Stop()
    {
        PedGeneral.ClearTasks(true);
    }
    public void Update()
    {

    }
    private void VehicleGuard()
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
        if (!PedGeneral.IsDriver || !PedGeneral.Pedestrian.IsInAnyVehicle(false) || !PedGeneral.Pedestrian.CurrentVehicle.Exists())
        {
            return;
        }
        if (PedGeneral.IsInHelicopter)
        {
            NativeFunction.CallByName<bool>("TASK_HELI_MISSION", PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, 0, 0, 0f, 0f, 300f, 9, 50f, 150f, -1f, -1, 30, -1.0f, 0);
        }
        else
        {
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_PAUSE", 0, -1);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", PedGeneral.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }

        }
        //EntryPoint.WriteToConsoleTestLong($"{PedGeneral?.Handle} VEHICLE GUARD IsNearSpawnPosition:{PedGeneral.IsNearSpawnPosition} canGuard{canGuard} canPatrol{canPatrol}");
    }
    private void VehiclePatrol()
    {
        if(!PedGeneral.Pedestrian.Exists())
        {
            return;
        }
        if (BlockPermanentEvents)
        {
            PedGeneral.Pedestrian.BlockPermanentEvents = true;
            PedGeneral.Pedestrian.KeepTasks = true;
        }
        if(!PedGeneral.IsDriver || !PedGeneral.Pedestrian.IsInAnyVehicle(false) || !PedGeneral.Pedestrian.CurrentVehicle.Exists())
        {
            return;
        }
        if (PedGeneral.IsInHelicopter)
        {
            NativeFunction.CallByName<bool>("TASK_HELI_MISSION", PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, 0, 0, 0f, 0f, 300f, 9, 50f, 150f, -1f, -1, 30, -1.0f, 0);
        }
        else
        {
            float Speed = IsReckless ? 30f : 10f;
            int DrivingStyle = IsReckless ? (int)eCustomDrivingStyles.Code3 : (int)eCustomDrivingStyles.RegularDriving;
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                //NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(1000, 2000));
                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, PedGeneral.Pedestrian.CurrentVehicle, Speed, DrivingStyle, Speed);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, Ped.Pedestrian.CurrentVehicle, 10f, (int)(VehicleDrivingFlags.FollowTraffic | VehicleDrivingFlags.YieldToCrossingPedestrians | VehicleDrivingFlags.RespectIntersections | (VehicleDrivingFlags)8), 10f);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", PedGeneral.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
        //EntryPoint.WriteToConsoleTestLong($"{PedGeneral?.Handle} VEHICLE PATROL IsNearSpawnPosition:{PedGeneral.IsNearSpawnPosition} canGuard{canGuard} canPatrol{canPatrol}");
    }
}

