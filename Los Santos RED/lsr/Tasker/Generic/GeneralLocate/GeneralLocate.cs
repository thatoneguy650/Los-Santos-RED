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
        Start();
    }
    public override void Start()
    {
        GetLocations();
        CurrentTaskState?.Stop();
        GetNewTaskState();
        CurrentTaskState?.Start();
        //EntryPoint.WriteToConsole($"{PedGeneral.Handle} STARTING Task{CurrentTaskState?.DebugName}");
    }
    public override void Stop()
    {
        CurrentTaskState?.Stop();
    }
    public override void Update()
    {
        GetLocations();
        CheckLocationChanged();
        if (LocationsChanged)
        {
            Start();
        }
        else
        {
            StandardUpdate();
        }
        UpdateVehicleState();
    }
    public virtual void OnLocationReached()
    {
        HasReachedLocatePosition = true;
    }
    private void StandardUpdate()
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
    }
    private void GetNewTaskState()
    {
        if(!HasReachedLocatePosition)
        {
            if (Ped.IsInVehicle)
            {
                CurrentTaskState = new GoToInVehicleTaskState(PedGeneral, Player, World, SeatAssigner, Settings, BlockPermanentEvents, PlaceToDriveTo, this);
            }
            else
            {
                CurrentTaskState = new GoToOnFootTaskState(PedGeneral, Player, World, SeatAssigner, Settings, BlockPermanentEvents, PlaceToWalkTo, this);
            }
        }
        else if (ShouldInvestigateOnFoot)
        {
            CurrentTaskState = new SearchLocationOnFootTaskState(PedGeneral, Player, World, SeatAssigner, Settings, BlockPermanentEvents, PlaceToWalkTo, this, WeaponIssuable);
        }
        else
        {
            CurrentTaskState = new WanderInVehicleTaskState(PedGeneral, World, SeatAssigner, PlacesOfInterest, Settings, BlockPermanentEvents, true);
        }
    }
    protected virtual void UpdateVehicleState()
    {
        
    }
    private void CheckLocationChanged()
    {
        if(prevPlaceToDriveTo.DistanceTo2D(PlaceToDriveTo) <= 5f && prevPlaceToWalkTo.DistanceTo2D(PlaceToWalkTo) <= 5f)
        {
            LocationsChanged = false;
            return;
        }
        HasReachedLocatePosition = false;    
        prevPlaceToDriveTo = PlaceToDriveTo;
        prevPlaceToWalkTo = PlaceToWalkTo;
        LocationsChanged = true;
        EntryPoint.WriteToConsole($"{PedGeneral.Handle} General Locate, Search Place Changed");
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
            PlaceToDriveTo = HasSixthSense ? Player.Character.Position : Ped.PlayerPerception.PositionLastSeenTarget;// Player.StreetPlacePoliceShouldSearchForPlayer;
            PlaceToWalkTo = HasSixthSense ? Player.Character.Position : Ped.PlayerPerception.PositionLastSeenTarget; //Player.PlacePoliceShouldSearchForPlayer;
        }

    }
}

