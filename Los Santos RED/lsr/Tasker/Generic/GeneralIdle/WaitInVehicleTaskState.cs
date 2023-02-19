﻿using LosSantosRED.lsr.Interface;
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
    private ITargetable Player;
    private bool BlockPermanentEvents = false;

    public WaitInVehicleTaskState(PedExt pedGeneral, ITargetable player, IEntityProvideable world, SeatAssigner seatAssigner, ISettingsProvideable settings, bool blockPermanentEvents)
    {
        PedGeneral = pedGeneral;
        Player = player;
        World = world;
        SeatAssigner = seatAssigner;
        Settings = settings;
        BlockPermanentEvents = blockPermanentEvents;
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
