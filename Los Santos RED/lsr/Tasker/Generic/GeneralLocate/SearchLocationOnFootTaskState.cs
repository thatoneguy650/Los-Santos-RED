using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class SearchLocationOnFootTaskState : TaskState
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
    private Cop Cop;
    private bool ResetWeapons = false;
    public SearchLocationOnFootTaskState(PedExt pedGeneral, ITargetable player, IEntityProvideable world, SeatAssigner seatAssigner, ISettingsProvideable settings, bool blockPermanentEvents, Vector3 placeToWalkTo, ILocationReachable locationReachable, Cop cop)
    {
        PedGeneral = pedGeneral;
        Player = player;
        World = world;
        SeatAssigner = seatAssigner;
        Settings = settings;
        BlockPermanentEvents = blockPermanentEvents;
        PlaceToWalkTo = placeToWalkTo;
        LocationReachable = locationReachable;
        Cop = cop;
    }
    public bool IsValid => PedGeneral != null && PedGeneral.Pedestrian.Exists() && PedGeneral.Pedestrian.DistanceTo2D(PlaceToWalkTo) <= 200f;
    public string DebugName => $"SearchLocationOnFootTaskState";
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
        if(!PedGeneral.IsInVehicle && !ResetWeapons)
        {
            Cop?.WeaponInventory.Reset();
            ResetWeapons = true;
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
        if (PedGeneral == null || !PedGeneral.Pedestrian.Exists() || PlaceToWalkTo == null || PlaceToWalkTo == Vector3.Zero)
        {
            return;
        }
        Vector3 RandomPlaceOnFoot = PlaceToWalkTo.Around2D(15f);
        //Vector3 RandomPlaceOnFoot2 = RandomPlaceOnFoot.Around2D(15f);
       // Vector3 RandomPlaceOnFoot3 = RandomPlaceOnFoot2.Around2D(15f);
        if (PedGeneral.Pedestrian.IsInAnyVehicle(false) && PedGeneral.Pedestrian.CurrentVehicle.Exists())
        {
            EntryPoint.WriteToConsole("LOCATE SET TO LEAVE VEHICLE AND WANDER");
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", 0, PedGeneral.Pedestrian.CurrentVehicle, 27, 1000);
                NativeFunction.CallByName<bool>("TASK_LEAVE_VEHICLE", 0, PedGeneral.Pedestrian.CurrentVehicle, 256);
                NativeFunction.CallByName<bool>("TASK_FOLLOW_NAV_MESH_TO_COORD", 0, RandomPlaceOnFoot.X, RandomPlaceOnFoot.Y, RandomPlaceOnFoot.Z, 100.0f, -1, 0f, 0, 0f);//15f, -1, 0.25f, 0, 40000.0f);
               // NativeFunction.CallByName<bool>("TASK_FOLLOW_NAV_MESH_TO_COORD", 0, RandomPlaceOnFoot2.X, RandomPlaceOnFoot2.Y, RandomPlaceOnFoot2.Z, 100.0f, -1, 0f, 0, 0f);//15f, -1, 0.25f, 0, 40000.0f);
               // NativeFunction.CallByName<bool>("TASK_FOLLOW_NAV_MESH_TO_COORD", 0, RandomPlaceOnFoot3.X, RandomPlaceOnFoot3.Y, RandomPlaceOnFoot3.Z, 100.0f, -1, 0f, 0, 0f);//15f, -1, 0.25f, 0, 40000.0f);
                                                                                                                                                                             //TASK_WANDER_STANDARD
                NativeFunction.CallByName<bool>("TASK_WANDER_STANDARD", 0, 0,0);
                //NativeFunction.CallByName<bool>("TASK_WANDER_IN_AREA", 0, RandomPlaceOnFoot.X, RandomPlaceOnFoot.Y, RandomPlaceOnFoot.Z, 250f, 0.0f, 0.0f);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", PedGeneral.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
        else
        {
            EntryPoint.WriteToConsole("LOCATE SET TO JUST WANDER");
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_FOLLOW_NAV_MESH_TO_COORD", 0, RandomPlaceOnFoot.X, RandomPlaceOnFoot.Y, RandomPlaceOnFoot.Z, 100.0f, -1, 0f, 0, 0f);//15f, -1, 0.25f, 0, 40000.0f);
               // NativeFunction.CallByName<bool>("TASK_FOLLOW_NAV_MESH_TO_COORD", 0, RandomPlaceOnFoot2.X, RandomPlaceOnFoot2.Y, RandomPlaceOnFoot2.Z, 100.0f, -1, 0f, 0, 0f);//15f, -1, 0.25f, 0, 40000.0f);
               // NativeFunction.CallByName<bool>("TASK_FOLLOW_NAV_MESH_TO_COORD", 0, RandomPlaceOnFoot3.X, RandomPlaceOnFoot3.Y, RandomPlaceOnFoot3.Z, 100.0f, -1, 0f, 0, 0f);//15f, -1, 0.25f, 0, 40000.0f);
                NativeFunction.CallByName<bool>("TASK_WANDER_STANDARD", 0, 0, 0);
                //NativeFunction.CallByName<bool>("TASK_WANDER_IN_AREA", 0, RandomPlaceOnFoot.X, RandomPlaceOnFoot.Y, RandomPlaceOnFoot.Z, 250f, 0.0f, 0.0f);//DONT REALLY WNADER MOST TIMES....
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", PedGeneral.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
    }
}

