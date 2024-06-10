using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class GoToOnFootTaskState : TaskState
{
    private PedExt PedGeneral;
    private IEntityProvideable World;
    private SeatAssigner SeatAssigner;

    private VehicleExt TaskedVehicle;
    private int TaskedSeat;
    private ISettingsProvideable Settings;
    private ITargetable Player;
    private bool BlockPermanentEvents = false;
    private Vector3 PlaceToWalkTo;
    private ILocationReachable LocationReachable;
    private IWeaponIssuable WeaponIssuable;
    private bool SetArmed = false;
    public GoToOnFootTaskState(PedExt pedGeneral, ITargetable player, IEntityProvideable world, SeatAssigner seatAssigner, ISettingsProvideable settings, bool blockPermanentEvents, Vector3 placeToWalkTo, ILocationReachable locationReachable, IWeaponIssuable weaponIssuable, bool setArmed)
    {
        PedGeneral = pedGeneral;
        Player = player;
        World = world;
        SeatAssigner = seatAssigner;
        Settings = settings;
        BlockPermanentEvents = blockPermanentEvents;
        PlaceToWalkTo = placeToWalkTo;
        LocationReachable = locationReachable;
        WeaponIssuable = weaponIssuable;
        SetArmed = setArmed;
    }
    public bool IsValid => PedGeneral != null && !PedGeneral.IsInVehicle && !LocationReachable.HasReachedLocatePosition;
    public string DebugName => $"GoToOnFootTaskState";
    public void Dispose()
    {
        Stop();
    }
    public void Start()
    {
        PedGeneral.ClearTasks(true);
        if (SetArmed)
        {
            WeaponIssuable?.WeaponInventory.SetDefaultArmed();
        }
        TaskEntry();
    }
    public void Stop()
    {
        PedGeneral.ClearTasks(true);
    }
    public void Update()
    {
        UpdateDistances();
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
        if (PedGeneral == null || !PedGeneral.Pedestrian.Exists() || PlaceToWalkTo == null || PlaceToWalkTo == Vector3.Zero)
        {
            return;
        }
        NativeFunction.Natives.SET_PED_USING_ACTION_MODE(PedGeneral.Pedestrian, true, -1, "DEFAULT_ACTION");
        NativeFunction.Natives.TASK_FOLLOW_NAV_MESH_TO_COORD(PedGeneral.Pedestrian, PlaceToWalkTo.X, PlaceToWalkTo.Y, PlaceToWalkTo.Z, 2.0f, -1, 0f, 0, 0f);//15f, -1, 0.25f, 0, 40000.0f);

        //NativeFunction.Natives.TASK_FOLLOW_NAV_MESH_TO_COORD(PedGeneral.Pedestrian, PlaceToWalkTo.X, PlaceToWalkTo.Y, PlaceToWalkTo.Z, 15f, -1, 0.25f, 0, 40000.0f);
    }
    private void UpdateDistances()
    {
        if(!PedGeneral.Pedestrian.Exists())
        {
            return;
        }
        float DistanceToCoordinates = PedGeneral.Pedestrian.DistanceTo2D(PlaceToWalkTo);
        if (DistanceToCoordinates <= 7f)
        {
            LocationReachable.OnLocationReached();
            //EntryPoint.WriteToConsoleTestLong($"LOCATE TASK: Cop {Ped.Handle} HAS REACHED POSITION");
        }
    }
}

