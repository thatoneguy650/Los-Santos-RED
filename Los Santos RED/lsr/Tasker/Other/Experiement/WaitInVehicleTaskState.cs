using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class WaitInVehicleTaskState : TaskState
{
    private PedExt PedGeneral;
    private IEntityProvideable World;
    private SeatAssigner SeatAssigner;
    private ISettingsProvideable Settings;

    public WaitInVehicleTaskState(PedExt pedGeneral, IEntityProvideable world, SeatAssigner seatAssigner, ISettingsProvideable settings)
    {
        PedGeneral = pedGeneral;
        World = world;
        SeatAssigner = seatAssigner;
        Settings = settings;
    }

    public bool IsValid => PedGeneral != null && PedGeneral.Pedestrian.Exists() && PedGeneral.IsInVehicle && PedGeneral.Pedestrian.CurrentVehicle.Exists() && SeatAssigner.HasPedsWaitingToEnter(World.Vehicles.GetVehicleExt(PedGeneral.Pedestrian.CurrentVehicle), PedGeneral.Pedestrian.SeatIndex);

    public string DebugName { get; } = "WaitInVehicleTaskState";

    public void Dispose()
    {

    }
    public void Start()
    {

    }
    public void Stop()
    {
    }

    public void Update()
    {

    }

}

