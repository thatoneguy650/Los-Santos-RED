using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GeneralIdle : ComplexTask
{
    protected PedExt PedGeneral;
    protected IEntityProvideable World;
    protected IPlacesOfInterest PlacesOfInterest;
    protected SeatAssigner SeatAssigner;
    protected TaskState CurrentTaskState;
    protected ISettingsProvideable Settings;
    protected bool BlockPermanentEvents = false;
    protected bool CheckPassengers = false;
    protected bool CheckVehicleState = false;
    protected bool ForceStandardScenarios = false;
    protected virtual bool AllowEnteringVehicle => !Ped.IsAnimal && (!Ped.IsLocationSpawned || PedGeneral.HasExistedFor >= 10000);
    public GeneralIdle(PedExt pedGeneral, IComplexTaskable ped, ITargetable player, IEntityProvideable world,List<VehicleExt> possibleVehicles, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, bool blockPermanentEvents, bool checkPassengers, bool checkVehicleState, bool forceStandardScenarios) : base(player, ped, 1500)//1500
    {
        PedGeneral = pedGeneral;
        Name = "Idle";
        SubTaskName = "";
        World = world;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        BlockPermanentEvents = blockPermanentEvents;
        CheckPassengers = checkPassengers;
        CheckVehicleState = checkVehicleState;
        ForceStandardScenarios = forceStandardScenarios;
        SeatAssigner = new SeatAssigner(Ped, World, possibleVehicles);
    }
    public override void ReTask()
    {
        Start();
    }
    public override void Start()
    {
        CurrentTaskState?.Stop();
        GetNewTaskState();
        if (Ped.Pedestrian.Exists())
        {
            NativeFunction.Natives.SET_PED_USING_ACTION_MODE(Ped.Pedestrian, false, -1, "DEFAULT_ACTION");
        }
        CurrentTaskState?.Start();
    }
    public override void Stop()
    {
        CurrentTaskState?.Stop();
    }
    public override void Update()
    {
        if(CurrentTaskState == null || !CurrentTaskState.IsValid)
        {
            Start();
        }
        else
        {
            SubTaskName = CurrentTaskState.DebugName;
            CurrentTaskState.Update();
        }
        if (CheckVehicleState)
        {
            SetVehicleState();
        }
    }
    protected virtual void GetNewTaskState()
    {
        if(AllowEnteringVehicle && !Ped.IsInVehicle && !SeatAssigner.IsAssignmentValid(true))
        {
            SeatAssigner.AssignFrontSeat(PedGeneral.HasExistedFor >= 10000);
        }
        if(Ped.IsInVehicle)
        {
            if(Ped.IsDriver)
            {
                if (Ped.Pedestrian.Exists() && Ped.Pedestrian.IsInAnyVehicle(false) && SeatAssigner.HasPedsWaitingToEnter(World.Vehicles.GetVehicleExt(Ped.Pedestrian.CurrentVehicle), Ped.Pedestrian.SeatIndex))
                {
                    CurrentTaskState = new WaitInVehicleTaskState(PedGeneral, Player, World, SeatAssigner, Settings, BlockPermanentEvents);
                }
                else
                {
                    CurrentTaskState = new WanderInVehicleTaskState(PedGeneral, World, SeatAssigner, PlacesOfInterest, Settings, BlockPermanentEvents, false);
                }
            }
            else
            {
                CurrentTaskState = new WanderInVehicleTaskState(PedGeneral, World, SeatAssigner, PlacesOfInterest, Settings, BlockPermanentEvents, false);//Maybe Get Out
            }
        }
        else
        {
            if (SeatAssigner.IsAssignmentValid(true))//Ped.ShouldGetInVehicle)
            {
                CurrentTaskState = new GetInVehicleTaskState(PedGeneral, Player, World,SeatAssigner, Settings, BlockPermanentEvents);
            }
            else
            {
                CurrentTaskState = new WanderOnFootTaskState(PedGeneral, World, SeatAssigner, Settings, BlockPermanentEvents, ForceStandardScenarios);
            }
        }
    }
    protected virtual void SetVehicleState()
    {
        if (Ped.Pedestrian.Exists() && Ped.Pedestrian.CurrentVehicle.Exists() && Ped.IsDriver && Ped.Pedestrian.CurrentVehicle.HasSiren && Ped.Pedestrian.CurrentVehicle.IsSirenOn)
        {
            Ped.Pedestrian.CurrentVehicle.IsSirenOn = false;
            Ped.Pedestrian.CurrentVehicle.IsSirenSilent = false;
        }
    }
}

