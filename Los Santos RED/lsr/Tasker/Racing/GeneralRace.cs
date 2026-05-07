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

        if (PedGeneral != null && PedGeneral.Pedestrian.Exists())
        {
            Ped racePed = PedGeneral.Pedestrian;

            // Apply dynamic values instead of hardcoded floats
            if (VehicleRacer is AIVehicleRacer aiRacer)
            {
                NativeFunction.Natives.SET_DRIVER_ABILITY(racePed, aiRacer.GetDriverAbility());
                NativeFunction.Natives.SET_DRIVER_AGGRESSIVENESS(racePed, aiRacer.GetDriverAggressiveness());
            }
            else
            {
                // Fallback just in case
                NativeFunction.Natives.SET_DRIVER_ABILITY(racePed, 1.0f);
                NativeFunction.Natives.SET_DRIVER_AGGRESSIVENESS(racePed, 1.0f);
            }
            NativeFunction.Natives.SET_DRIVER_RACING_MODIFIER(racePed, 1.0f);

            // This is the SPEED LIMITER. Set it high so our rubber-banding isn't capped.
            NativeFunction.Natives.SET_DRIVE_TASK_MAX_CRUISE_SPEED(racePed, 500f, true);

            //NativeFunction.Natives.SET_PED_HIGHLY_PERCEPTIVE(racePed, true);  //test
            //NativeFunction.Natives.SET_PED_SEEING_RANGE(racePed, 10000f);
            //NativeFunction.Natives.SET_PED_VISUAL_FIELD_PERIPHERAL_RANGE(racePed, 400f); //test
            //NativeFunction.Natives.SET_PED_INCREASED_AVOIDANCE_RADIUS(racePed); // test
        }

        GetNewTaskState();
        CurrentTaskState?.Start();
    }
    public override void Stop()
    {
        CurrentTaskState?.Stop();
    }

    public override void Update()
    {
        if (VehicleRacer == null || PedGeneral == null) return;

        // Detect if the racer has moved on to a new checkpoint in the base class
        if (AssignedCheckpoint != VehicleRacer.TargetCheckpoint)
        {
            GetNewTaskState();
        }

        if (CurrentTaskState == null || !CurrentTaskState.IsValid)
        {
            Start();
        }
        else
        {
            SubTaskName = CurrentTaskState.DebugName;
            CurrentTaskState.Update();
        }
    }

    private void GetNewTaskState()
    {
        if (VehicleRacer?.TargetCheckpoint != null)
        {
            AssignedCheckpoint = VehicleRacer.TargetCheckpoint;

           
            CurrentTaskState = new GoToPositionVehicleRaceTaskState(
                PedGeneral, Player, World, SeatAssigner, Settings, true, AssignedCheckpoint.Position, this);

            CurrentTaskState.Start();
        }
    }
}

