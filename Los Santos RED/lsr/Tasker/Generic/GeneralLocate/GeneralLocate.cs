using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GeneralLocate : ComplexTask, ILocationReachable
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
    public bool HasReachedLocatePosition { get; protected set; } = false;
    public List<Vector3> SearchPoints { get; protected set; }

    protected virtual bool ShouldInvestigateOnFoot => !Ped.IsInHelicopter && Player.IsOnFoot;

    public GeneralLocate(PedExt pedGeneral, IComplexTaskable ped, ITargetable player, IEntityProvideable world, List<VehicleExt> possibleVehicles, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, bool blockPermanentEvents,
        IWeaponIssuable weaponIssuable, bool hasSixthSense) : base(player, ped, 1500)//1500
    {
        PedGeneral = pedGeneral;
        complexTaskablePed = ped;
        Name = "Locate";
        SubTaskName = "";
        World = world;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        BlockPermanentEvents = blockPermanentEvents;
        WeaponIssuable = weaponIssuable;
        HasSixthSense = hasSixthSense;
        SeatAssigner = new SeatAssigner(Ped, World, possibleVehicles);
    }
    
    public override void ReTask()
    {

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
        //EntryPoint.WriteToConsole($"{PedGeneral.Handle} STARTED Task{CurrentTaskState?.DebugName}");
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
        if(!HasReachedLocatePosition)
        {
            if (Ped.IsInVehicle)
            {
                CurrentTaskState = new GoToInVehicleTaskState(PedGeneral, Player, World, SeatAssigner, Settings, BlockPermanentEvents, PlaceToDriveTo, this);
                SubTaskName = "GoToInVehicleTaskState";
            }
            else
            {
                CurrentTaskState = new GoToOnFootTaskState(PedGeneral, Player, World, SeatAssigner, Settings, BlockPermanentEvents, PlaceToWalkTo, this, WeaponIssuable, false);
                SubTaskName = "GoToOnFootTaskState";
            }
        }
        else if (ShouldInvestigateOnFoot)
        {
            CurrentTaskState = new SearchLocationOnFootTaskState(PedGeneral, Player, World, SeatAssigner, Settings, BlockPermanentEvents, PlaceToWalkTo, this, WeaponIssuable, false, true, SearchPoints);
            SubTaskName = "SearchLocationOnFootTaskState";
        }
        else
        {
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
        prevPlaceToDriveTo = PlaceToDriveTo;
        prevPlaceToWalkTo = PlaceToWalkTo;
        LocationsChanged = true;
        //EntryPoint.WriteToConsole($"{PedGeneral.Handle} General Locate, Search Place Changed driveChangeDistance{driveChangeDistance} walkChangeDistance{walkChangeDistance}");
    }

    protected virtual void GetLocations()
    {
        if(OtherTarget != null && OtherTarget.Pedestrian.Exists())
        {
            PlaceToDriveTo = OtherTarget.Pedestrian.Position;// Player.StreetPlacePoliceShouldSearchForPlayer;
            PlaceToWalkTo = OtherTarget.Pedestrian.Position; //Player.PlacePoliceShouldSearchForPlayer;
        }
        else
        {
            PlaceToDriveTo = HasSixthSense ? Player.Character.Position : Ped.PlayerPerception.PositionLastSeenTarget;
            PlaceToWalkTo = HasSixthSense ? Player.Character.Position : Ped.PlayerPerception.PositionLastSeenTarget;
        }

    }
}

