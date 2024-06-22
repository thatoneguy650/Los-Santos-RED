using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GeneralInvestigate : ComplexTask, ILocationReachable
{
    protected uint GameTimeStarted;
    protected IComplexTaskable complexTaskablePed;
    protected PedExt PedGeneral;
    protected IEntityProvideable World;
    protected IPlacesOfInterest PlacesOfInterest;
    protected SeatAssigner SeatAssigner;
    protected TaskState CurrentTaskState;
    protected ISettingsProvideable Settings;
    protected IWeaponIssuable WeaponIssuable;
    protected bool BlockPermanentEvents = false;
    protected bool HasSixthSense = false;
    protected bool LocationsChanged = false;
    protected Vector3 PlaceToDriveTo;
    protected Vector3 PlaceToWalkTo;
    protected Vector3 prevPlaceToDriveTo;
    protected Vector3 prevPlaceToWalkTo;
    protected bool ShouldSearchArea;
    public bool HasReachedLocatePosition { get; protected set; } = false;
    protected virtual bool ShouldInvestigateOnFoot => !Ped.IsInHelicopter && Player.IsOnFoot;
    protected virtual bool ForceSetArmed => false;
    public List<Vector3> SearchPoints { get; protected set; }

    public GeneralInvestigate(PedExt pedGeneral, IComplexTaskable ped, ITargetable player, IEntityProvideable world, List<VehicleExt> possibleVehicles, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, bool blockPermanentEvents,
        IWeaponIssuable weaponIssuable, bool shouldSearchArea) : base(player, ped, 1500)//1500
    {
        PedGeneral = pedGeneral;
        complexTaskablePed = ped;
        Name = "Investigate";
        SubTaskName = "";
        World = world;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        BlockPermanentEvents = blockPermanentEvents;
        WeaponIssuable = weaponIssuable;
        ShouldSearchArea = shouldSearchArea;
        SeatAssigner = new SeatAssigner(Ped, World, possibleVehicles);
    }
    public override void ReTask()
    {
        //Start();
    }
    public override void Start()
    {
        GameTimeStarted = Game.GameTime;
        GetLocations();
        prevPlaceToDriveTo = PlaceToDriveTo;
        prevPlaceToWalkTo = PlaceToWalkTo;
        CurrentTaskState?.Stop();
        GetNewTaskState();
        CurrentTaskState?.Start();
       // EntryPoint.WriteToConsole($"{PedGeneral.Handle} STARTED Task{CurrentTaskState?.DebugName}");
    }
    public override void Stop()
    {
        CurrentTaskState?.Stop();
    }
    public override void Update()
    {
        GetLocations();
        CheckLocationChanged();
        StandardUpdate();
        UpdateVehicleState();
    }
    public virtual void OnFinalSearchLocationReached()
    {

    }
    public virtual void OnLocationReached()
    {
        Ped.GameTimeReachedInvestigationPosition = Game.GameTime;
        HasReachedLocatePosition = true;
    }
    private void StandardUpdate()
    {
        if (CurrentTaskState == null || !CurrentTaskState.IsValid || LocationsChanged)
        {
            if (Game.GameTime - GameTimeStarted >= 1000)
            {
                Start();
                //EntryPoint.WriteToConsole($"{PedGeneral.Handle} UPDATE START Task{CurrentTaskState?.DebugName} INVALID {CurrentTaskState?.IsValid}");
            }
        }
        else
        {
            SubTaskName = CurrentTaskState.DebugName;
            CurrentTaskState.Update();
        }
    }
    private void GetNewTaskState()
    {
        if (!HasReachedLocatePosition)
        {
            if (Ped.IsInVehicle)
            {
                CurrentTaskState = new GoToInVehicleTaskState(PedGeneral, Player, World, SeatAssigner, Settings, BlockPermanentEvents, PlaceToDriveTo, this);
                SubTaskName = "GoToInVehicleTaskState";
            }
            else
            {
                if (PedGeneral.Pedestrian.Exists())
                {
                    NativeFunction.Natives.SET_PED_SHOULD_PLAY_IMMEDIATE_SCENARIO_EXIT(PedGeneral.Pedestrian);
                }
                CurrentTaskState = new GoToOnFootTaskState(PedGeneral, Player, World, SeatAssigner, Settings, BlockPermanentEvents, PlaceToWalkTo, this, WeaponIssuable, ForceSetArmed);
                SubTaskName = "GoToOnFootTaskState";
            }
        }
        else if (ShouldInvestigateOnFoot)
        {
            //EntryPoint.WriteToConsole($"SearchLocationOnFootTaskState {PedGeneral.Handle} ShouldInvestigateOnFoot:{ShouldInvestigateOnFoot} IsInHelicopter:{Ped.IsInHelicopter} Player.IsOnFoot:{Player.IsOnFoot}");
            if (PedGeneral.Pedestrian.Exists())
            {
                NativeFunction.Natives.SET_PED_SHOULD_PLAY_IMMEDIATE_SCENARIO_EXIT(PedGeneral.Pedestrian);
            }
            CurrentTaskState = new SearchLocationOnFootTaskState(PedGeneral, Player, World, SeatAssigner, Settings, BlockPermanentEvents, PlaceToWalkTo, this, WeaponIssuable, ForceSetArmed, ShouldSearchArea, SearchPoints);
            SubTaskName = "SearchLocationOnFootTaskState";
        }
        else
        {
            //EntryPoint.WriteToConsole($"WanderInVehicleTaskState {PedGeneral.Handle} ShouldInvestigateOnFoot:{ShouldInvestigateOnFoot} IsInHelicopter:{Ped.IsInHelicopter} Player.IsOnFoot:{Player.IsOnFoot}");
            CurrentTaskState = new WanderInVehicleTaskState(PedGeneral, World, SeatAssigner, PlacesOfInterest, Settings, BlockPermanentEvents, true);
            SubTaskName = "WanderInVehicleTaskState";
        }
    }
    protected virtual void UpdateVehicleState()
    {

    }
    private void CheckLocationChanged()
    {
        float driveChangeDistance = prevPlaceToDriveTo.DistanceTo2D(PlaceToDriveTo);
        float walkChangeDistance = prevPlaceToWalkTo.DistanceTo2D(PlaceToWalkTo);
        if ((PedGeneral.IsInVehicle && driveChangeDistance <= 15f) || (walkChangeDistance <= 10f)) //if (prevPlaceToDriveTo != PlaceToDriveTo  || prevPlaceToWalkTo != PlaceToWalkTo)// if (prevPlaceToDriveTo.DistanceTo2D(PlaceToDriveTo) <= 5f && prevPlaceToWalkTo.DistanceTo2D(PlaceToWalkTo) <= 5f)       
        {
            LocationsChanged = false;
            return;
        }
        HasReachedLocatePosition = false;
        Ped.GameTimeReachedInvestigationPosition = 0;
        prevPlaceToDriveTo = PlaceToDriveTo;
        prevPlaceToWalkTo = PlaceToWalkTo;
        LocationsChanged = true;
        //EntryPoint.WriteToConsole($"{PedGeneral.Handle} General Locate, Search Place Changed driveChangeDistance{driveChangeDistance} walkChangeDistance{walkChangeDistance}");
    }

    protected virtual void GetLocations()
    {
        if(Ped.PedAlerts.IsAlerted)
        {
            PlaceToDriveTo = Ped.PedAlerts.AlertedPoint;
            PlaceToWalkTo = Ped.PedAlerts.AlertedPoint;
        }
    }
}

