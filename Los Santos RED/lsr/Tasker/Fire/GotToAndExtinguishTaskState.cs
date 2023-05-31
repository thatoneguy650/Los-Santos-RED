using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class GoToAndExtinguishTaskState : TaskState
{
    private PedExt PedGeneral;
    private IEntityProvideable World;
    private SeatAssigner SeatAssigner;

    private VehicleExt TaskedVehicle;
    private int TaskedSeat;
    private ISettingsProvideable Settings;
    private ITargetable Player;
    private bool BlockPermanentEvents = false;
    private IWeaponIssuable WeaponIssuable;
    private bool ResetWeapons = false;
    private bool HasFires = true;
    private Vector3 FirePosition;
    private Vector3 PrevFirePosition;
    private Vector3 FireStandLocation;
    public GoToAndExtinguishTaskState(PedExt pedGeneral, ITargetable player, IEntityProvideable world, SeatAssigner seatAssigner, ISettingsProvideable settings, bool blockPermanentEvents,
        IWeaponIssuable weaponIssuable)
    {
        PedGeneral = pedGeneral;
        Player = player;
        World = world;
        SeatAssigner = seatAssigner;
        Settings = settings;
        BlockPermanentEvents = blockPermanentEvents;
        WeaponIssuable = weaponIssuable;
    }
    public bool IsValid => PedGeneral != null && PedGeneral.Pedestrian.Exists() && HasFires;
    public string DebugName => $"GoToAndExtinguishTaskState";
    public void Dispose()
    {
        Stop();
    }
    public void Start()
    {
        PedGeneral.ClearTasks(true);
        GetClosestFire();
        TaskEntry();
    }
    public void Stop()
    {
        PedGeneral.ClearTasks(true);
        WeaponIssuable?.WeaponInventory.SetCompletelyUnarmed();
        EntryPoint.WriteToConsole("GoToAndExtinguishTaskState STOP RAN");
    }
    public void Update()
    {
        if (!PedGeneral.IsInVehicle && !ResetWeapons)
        {
            WeaponIssuable?.WeaponInventory.Reset();
            WeaponIssuable?.WeaponInventory.SetDefaultArmed();  
            ResetWeapons = true;
        }
        GetClosestFire();
    }

    private void GetClosestFire()
    {
        HasFires = NativeFunction.Natives.GET_CLOSEST_FIRE_POS<bool>(out Vector3 outPosition, PedGeneral.Position);
        if(!HasFires)
        {
            FirePosition = Vector3.Zero;
            return;
        }
        float FireChangeDistance = FirePosition.DistanceTo(outPosition);
        FirePosition = outPosition;
        if (FireChangeDistance >= 3f)
        {
            FireStandLocation = FirePosition.Around2D(1f);
            TaskEntry();
            EntryPoint.WriteToConsole("$FIRE POSITION CHANGED RETASKING");
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
        if (PedGeneral == null || !PedGeneral.Pedestrian.Exists())
        {
            return;
        }
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            if (PedGeneral.Pedestrian.IsInAnyVehicle(false) && PedGeneral.Pedestrian.CurrentVehicle.Exists())
            {
                NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", 0, PedGeneral.Pedestrian.CurrentVehicle, 27, 1000);
                NativeFunction.CallByName<bool>("TASK_LEAVE_VEHICLE", 0, PedGeneral.Pedestrian.CurrentVehicle, 256);
            }
            NativeFunction.CallByName<bool>("TASK_FOLLOW_NAV_MESH_TO_COORD", 0, FireStandLocation.X, FireStandLocation.Y, FireStandLocation.Z, 2.0f, -1, 5f, 0, 0f);//15f, -1, 0.25f, 0, 40000.0f);
            NativeFunction.CallByName<bool>("TASK_SHOOT_AT_COORD", 0, FirePosition.X, FirePosition.Y, FirePosition.Z, 4000, (uint)FiringPattern.FullAutomatic);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", PedGeneral.Pedestrian, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }

    }
}

