using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GeneralBeStationary : ComplexTask
{
    private PedExt PedGeneral;
    private IEntityProvideable World;
    private IPlacesOfInterest PlacesOfInterest;
    private SeatAssigner SeatAssigner;
    private TaskState CurrentTaskState;
    private ISettingsProvideable Settings;

    public GeneralBeStationary(PedExt pedGeneral, IComplexTaskable ped, ITargetable player, IEntityProvideable world, ISettingsProvideable settings) :
        base(player, ped, 1000)//1500
    {
        PedGeneral = pedGeneral;
        Name = "GeneralBeStationary";
        SubTaskName = "";
        World = world;
        Settings = settings;
    }
    public override void ReTask()
    {
        Start();
    }
    public override void Start()
    {
        EntryPoint.WriteToConsole("GeneralBeStationary START RAN");
        CurrentTaskState?.Stop();
        GetNewTaskState();
        CurrentTaskState?.Start();
    }
    public override void Stop()
    {
        CurrentTaskState?.Stop();
    }
    public override void Update()
    {
        if (CurrentTaskState == null || !CurrentTaskState.IsValid)
        {
            Start();
        }
        else
        {
            SubTaskName = CurrentTaskState.DebugName;
            CurrentTaskState.Update();
        }
        EntryPoint.WriteToConsole("GENERAL RACE UPDATE RAN");
    }
    private void GetNewTaskState()
    {
        CurrentTaskState = new BeStationaryTaskState(PedGeneral, Player, World, Settings, true);
    }
}

