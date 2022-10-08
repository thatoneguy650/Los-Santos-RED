using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class GetInVehicleTaskState : TaskState
{
    private PedExt PedGeneral;
    private IEntityProvideable World;
    private SeatAssigner SeatAssigner;

    private VehicleExt TaskedVehicle;
    private int TaskedSeat;
    private ISettingsProvideable Settings;
    public GetInVehicleTaskState(PedExt pedGeneral, IEntityProvideable world, SeatAssigner seatAssigner, ISettingsProvideable settings)
    {
        PedGeneral = pedGeneral;
        World = world;
        SeatAssigner = seatAssigner;
        Settings = settings;
    }

    public bool IsValid => PedGeneral != null && !PedGeneral.IsInVehicle && PedGeneral.Pedestrian.Exists() && SeatAssigner != null && SeatAssigner.IsAssignmentValid();
    public string DebugName => $"GetInVehicleTaskState Vehicle {SeatAssigner?.VehicleAssigned?.Handle} Seat {SeatAssigner?.SeatAssigned}";
    public void Dispose()
    {
        Stop();
    }
    public void Start()
    {
        TaskEntry();
    }
    public void Stop()
    {

    }
    public void Update()
    {

    }
    private void TaskEntry()
    {
        if (SeatAssigner.VehicleAssigned != null && SeatAssigner.VehicleAssigned.Vehicle.Exists())
        {
            TaskedVehicle = SeatAssigner.VehicleAssigned;
            TaskedSeat = SeatAssigner.SeatAssigned;
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, SeatAssigner.VehicleAssigned.Vehicle, -1, SeatAssigner.SeatAssigned, 1f, 9);
                //NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(8000, 16000));
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", PedGeneral.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
    }
}

