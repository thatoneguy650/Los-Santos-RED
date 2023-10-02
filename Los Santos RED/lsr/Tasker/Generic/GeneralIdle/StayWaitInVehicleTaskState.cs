using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class StayWaitInVehicleTaskState : TaskState
{
    private PedExt PedGeneral;
    private IEntityProvideable World;
    private SeatAssigner SeatAssigner;
    private ISettingsProvideable Settings;
    private ITargetable Player;
    private bool BlockPermanentEvents = false;

    public StayWaitInVehicleTaskState(PedExt pedGeneral, ITargetable player, IEntityProvideable world, SeatAssigner seatAssigner, ISettingsProvideable settings, bool blockPermanentEvents)
    {
        PedGeneral = pedGeneral;
        Player = player;
        World = world;
        SeatAssigner = seatAssigner;
        Settings = settings;
        BlockPermanentEvents = blockPermanentEvents;
    }

    public bool IsValid => PedGeneral != null && PedGeneral.Pedestrian.Exists() && PedGeneral.IsInVehicle && PedGeneral.Pedestrian.CurrentVehicle.Exists();

    public string DebugName { get; } = "StayWaitInVehicleTaskState";

    public void Dispose()
    {

    }

    public void Update()
    {

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
    }

}

