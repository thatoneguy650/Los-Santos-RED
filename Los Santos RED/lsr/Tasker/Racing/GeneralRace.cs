using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GeneralRace : ComplexTask, ILocationReachable
{
    private PedExt PedGeneral;
    private IEntityProvideable World;
    private IPlacesOfInterest PlacesOfInterest;
    private SeatAssigner SeatAssigner;
    private TaskState CurrentTaskState;
    private ISettingsProvideable Settings;
    private VehicleRace VehicleRace;
    private VehicleRacer VehicleRacer;
    private VehicleRaceCheckpoint AssignedCheckpoint;

    public GeneralRace(PedExt pedGeneral, IComplexTaskable ped, ITargetable player, IEntityProvideable world, List<VehicleExt> possibleVehicles, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, 
         VehicleRace vehicleRace, VehicleRacer vehicleRacer) :
        base(player, ped, 1000)//1500
    {
        PedGeneral = pedGeneral;
        Name = "GeneralRace";
        SubTaskName = "";
        World = world;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        VehicleRace = vehicleRace;
        VehicleRacer = vehicleRacer;
    }

    public bool HasReachedLocatePosition { get; set; }

    public void OnFinalSearchLocationReached()
    {

    }

    public void OnLocationReached()
    {
        EntryPoint.WriteToConsole("AI RACER HAS REACHED LOACTION");
        HasReachedLocatePosition = true;
    }

    public override void ReTask()
    {
        Start();
    }
    public override void Start()
    {
        HasReachedLocatePosition = false;
        CurrentTaskState?.Stop();
        //World.Pedestrians.RemoveSeatAssignment(PedGeneral);
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
        if (PedGeneral != null && PedGeneral.Pedestrian.Exists())
        {
            //NativeFunction.Natives.SET_DRIVER_ABILITY(PedGeneral.Pedestrian, 1.0f);
            NativeFunction.Natives.SET_DRIVER_AGGRESSIVENESS(PedGeneral.Pedestrian, 1.0f);
            NativeFunction.Natives.SET_DRIVER_RACING_MODIFIER(Ped.Pedestrian, 1.0f);
        }
        EntryPoint.WriteToConsole("GENERAL RACE UPDATE RAN");
    }
    private void GetNewTaskState()
    {
        EntryPoint.WriteToConsole($"AI RACER TASKED {VehicleRacer.TargetCheckpoint.Position}");
        CurrentTaskState = new GoToPositionRacingTaskState(PedGeneral, Player, World, SeatAssigner, Settings, true, VehicleRacer.TargetCheckpoint.Position, this);   
    }
}

