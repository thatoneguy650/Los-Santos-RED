using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TaxiService : ComplexTask, ILocationReachable
{
    private TaxiDriver TaxiDriver;
    private PedExt PedGeneral;
    private IEntityProvideable World;
    private IPlacesOfInterest PlacesOfInterest;
    private SeatAssigner SeatAssigner;
    private TaskState CurrentTaskState;
    private ISettingsProvideable Settings;
    private bool BlockPermanentEvents = false;
    private uint GameTimeReachedLocation;
    protected bool LocationsChanged = false;
    protected bool DrivingStyleChanged = false;

    protected Vector3 PlaceToDriveTo;
    private float PlaceToDriveToHeading;
    protected Vector3 prevPlaceToDriveTo;
    private uint GameTimeStarted;
    protected string prevDrivingStyle;
    private bool AllowEnteringVehicle => !Ped.IsAnimal && (!Ped.IsLocationSpawned || PedGeneral.HasExistedFor >= 10000);
    public bool HasReachedLocatePosition { get; protected set; } = false;
    public TaxiService(PedExt pedGeneral, IComplexTaskable ped, ITargetable player, IEntityProvideable world, List<VehicleExt> possibleVehicles, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, 
        bool blockPermanentEvents, TaxiDriver taxiDriver) : base(player, ped, 1500)//1500
    {
        PedGeneral = pedGeneral;
        Name = "TaxiService";
        SubTaskName = "";
        World = world;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        BlockPermanentEvents = blockPermanentEvents;
        TaxiDriver = taxiDriver;
        SeatAssigner = new SeatAssigner(Ped, World, possibleVehicles);
    }
    public override void ReTask()
    {
        Start();
    }
    public override void Start()
    {
        PedGeneral.ClearTasks(true);
        GameTimeStarted = Game.GameTime;
        GetLocations();
        prevPlaceToDriveTo = PlaceToDriveTo;
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
        GetLocations();
        CheckLocationChanged();
        CheckDrivingStyleChanged();
        StandardUpdate();
        CheckLocationItems();
        CheckLight();
    }
    private void CheckLight()
    {
        if (Ped.IsDriver && Ped.Pedestrian.Exists() && Ped.Pedestrian.CurrentVehicle.Exists() && NativeFunction.Natives.IS_TAXI_LIGHT_ON<bool>(Ped.Pedestrian.CurrentVehicle))
        {
            NativeFunction.Natives.SET_TAXI_LIGHTS(Ped.Pedestrian.CurrentVehicle, false);
           // Ped.Pedestrian.CurrentVehicle.IsTaxiLightOn = false;
        }
    }
    private void StandardUpdate()
    {
        if (CurrentTaskState == null || !CurrentTaskState.IsValid || LocationsChanged || DrivingStyleChanged)
        {
            if (Game.GameTime - GameTimeStarted >= 1000)
            {
                Start();
                EntryPoint.WriteToConsole($"{PedGeneral.Handle} UPDATE START Task{CurrentTaskState?.DebugName} INVALID {CurrentTaskState?.IsValid}");
            }
        }
        else
        {
            SubTaskName = CurrentTaskState.DebugName;
            CurrentTaskState.Update();
        }
    }
    private void CheckLocationItems()
    {
        //if(TaxiDriver.TaxiRide == null)
        //{
        //    return;
        //}
        //if (TaxiDriver.TaxiRide.HasPickedUpPlayer)// || TaxiDriver.TaxiRide.HasArrivedAtDestination)
        //{
        //    if ((GameTimeReachedLocation > 0 && Game.GameTime - GameTimeReachedLocation >= 45000) || TaxiDriver.DistanceToPlayer >= 35f)
        //    {
        //        EntryPoint.WriteToConsole("TAXI DRIVER WAITED 45 SECONDS OR PLAYER IS OVER 35meters away, RELEASING");
        //        if (TaxiDriver == null || TaxiDriver.TaxiRide == null)
        //        {
        //            return;
        //        }
        //        TaxiDriver.TaxiRide?.Cancel();
        //    }
        //}
    }
    private void GetNewTaskState()
    {
        if(TaxiDriver == null || TaxiDriver.TaxiRide == null || !Ped.Pedestrian.Exists())
        {
            EntryPoint.WriteToConsole("TAXI BRAIN GetNewTaskState SOMETHING IS WRONG");
            return;
        }
        if (Ped.IsInVehicle)
        {
            NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, 1.0f);
            CurrentTaskState = new RegularGoToInVehicleTaskState(PedGeneral, Player, World, SeatAssigner, Settings, true, PlaceToDriveTo, PlaceToDriveToHeading, this, TaxiDriver.TaxiRide.TaxiDrivingStyle.DrivingStyle, TaxiDriver.TaxiRide.TaxiDrivingStyle.Speed);
        }
        else
        {
            if (SeatAssigner.IsAssignmentValid(true))
            {
                CurrentTaskState = new GetInVehicleTaskState(PedGeneral, Player, World, SeatAssigner, Settings, false);
            }
            else
            {
                CurrentTaskState = new WanderOnFootTaskState(PedGeneral, World, SeatAssigner, Settings, false, true);
            }
        }
    }

    public virtual void OnLocationReached()
    {
        if (!HasReachedLocatePosition)
        { 
            GameTimeReachedLocation = Game.GameTime;
            HasReachedLocatePosition = true;
            EntryPoint.WriteToConsole("TAXI DRIVER ARRIVED");
        }
    }

    private void CheckLocationChanged()
    {
        float driveChangeDistance = prevPlaceToDriveTo.DistanceTo2D(PlaceToDriveTo);
        if (PedGeneral.IsInVehicle && driveChangeDistance <= 3f) //if (prevPlaceToDriveTo != PlaceToDriveTo  || prevPlaceToWalkTo != PlaceToWalkTo)// if (prevPlaceToDriveTo.DistanceTo2D(PlaceToDriveTo) <= 5f && prevPlaceToWalkTo.DistanceTo2D(PlaceToWalkTo) <= 5f)       
        {
            LocationsChanged = false;
            return;
        }
        GameTimeReachedLocation = 0;
        HasReachedLocatePosition = false;
        prevPlaceToDriveTo = PlaceToDriveTo;
        LocationsChanged = true;
        EntryPoint.WriteToConsole($"{PedGeneral.Handle} TaxiService, Drive Place Changed driveChangeDistance{driveChangeDistance} ");
    }
    private void CheckDrivingStyleChanged()
    {
        if(TaxiDriver == null || TaxiDriver.TaxiRide == null)
        {
            DrivingStyleChanged = false;
            return;
        }
        if(TaxiDriver.TaxiRide.TaxiDrivingStyle.Name == prevDrivingStyle)
        {
            DrivingStyleChanged = false;
            return;
        }
        else 
        {
            DrivingStyleChanged = true;
            prevDrivingStyle = TaxiDriver.TaxiRide.TaxiDrivingStyle.Name;
        }
    }
    protected virtual void GetLocations()
    {
        if (TaxiDriver == null || TaxiDriver.TaxiRide == null)
        {
            return;
        }
        PlaceToDriveTo = TaxiDriver.TaxiRide.CurrentDriveToPosition;
        PlaceToDriveToHeading = TaxiDriver.TaxiRide.CurrentDriveToHeading;
    }

    public void OnFinalSearchLocationReached()
    {

    }
}

