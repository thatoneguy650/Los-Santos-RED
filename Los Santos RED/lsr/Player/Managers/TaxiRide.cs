using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TaxiRide
{
    private Vector3 InitialPickupLocation;
    private IEntityProvideable World;
    private ITaxiRideable Player;
    public TaxiRide(IEntityProvideable world, ITaxiRideable player, TaxiFirm requestedFirm, TaxiVehicleExt respondingVehicle, TaxiDriver respondingDriver)
    {
        World = world;
        Player = player;
        RequestedFirm = requestedFirm;
        RespondingVehicle = respondingVehicle;
        RespondingDriver = respondingDriver;
        TaxiDrivingStyle = new PedDrivingStyle("Normal", eCustomDrivingStyles.RegularDriving, 10f);
        PickupLocation = new SpawnLocation();
        DestinationLocation = new SpawnLocation();
    }
    public TaxiRide(IEntityProvideable world, ITaxiRideable player, TaxiFirm requestedFirm, Vector3 pickupLocation)
    {
        World = world;
        Player = player;
        RequestedFirm = requestedFirm;
        InitialPickupLocation = pickupLocation;
        TaxiDrivingStyle = new PedDrivingStyle("Normal", eCustomDrivingStyles.RegularDriving, 10f);
        PickupLocation = new SpawnLocation();
        DestinationLocation = new SpawnLocation();
    }
    public Vector3 CurrentDriveToPosition
    {
        get
        {
            if(!HasPickedUpPlayer && HasPickup)
            {
                return PickupLocation.StreetPosition;
            }
            else if (HasPickedUpPlayer && HasDestination)
            {
                return DestinationLocation.StreetPosition;
            }
            return Vector3.Zero;
        }
    }
    public TaxiFirm RequestedFirm { get; set; }
    public SpawnLocation PickupLocation { get; set; }
    public TaxiVehicleExt RespondingVehicle { get; private set; }
    public TaxiDriver RespondingDriver { get; private set; }
    public SpawnLocation DestinationLocation { get; private set; }
    public PedDrivingStyle TaxiDrivingStyle { get; set; }
    public bool IsValid { get; private set; } = true;
    public bool IsActive { get; private set; } = false;
    public bool HasPickedUpPlayer { get; set; } = false;
    public bool HasDroppedOffPlayer { get; set; } = false;
    public bool HasArrivedAtDestination { get; set; } = false;
    public bool HasDestination => DestinationLocation != null && DestinationLocation.StreetPosition != Vector3.Zero;
    public bool HasPickup => PickupLocation != null && PickupLocation.StreetPosition != Vector3.Zero;
    public void Setup()
    {
        IsActive = false;
        PickupLocation = new SpawnLocation(InitialPickupLocation);
        DestinationLocation = new SpawnLocation();
        PickupLocation.GetClosestStreet(true);
        if(!PickupLocation.HasStreetPosition)
        {
            return;
        }
        if (GetVehicleAndDriver())//if there is an existing one
        {
            IsActive = true;
            return;
        }
        SpawnVehicleAndDriver();
        if (GetVehicleAndDriver())//if there is an existing one
        {
            IsActive = true;
            return;
        }
    }
    private void SpawnVehicleAndDriver()
    {
        Player.Dispatcher.ForceTaxiSpawn(RequestedFirm);// Dispatcher.ForceTaxiSpawn(RequestedFirm);
    }
    private bool GetVehicleAndDriver()
    {
        RespondingVehicle = World.Vehicles.TaxiVehicles.Where(x => x.TaxiFirm.ID == RequestedFirm.ID && x.Vehicle.Exists() && x.Vehicle.HasDriver).OrderBy(x=> x.Vehicle.Position.DistanceTo2D(Player.Position)).FirstOrDefault();
        if(RespondingVehicle == null || !RespondingVehicle.Vehicle.Exists() || !RespondingVehicle.Vehicle.Driver.Exists())
        {
            return false;
        }
        uint driverhandle = RespondingVehicle.Vehicle.Driver.Handle;
        RespondingDriver = World.Pedestrians.TaxiDriverList.FirstOrDefault(x => x.Handle == driverhandle);
        if(RespondingDriver == null || !RespondingDriver.Pedestrian.Exists())
        {
            return false;
        }
        //RespondingDriver.SetTaskingActive(PickupLocation.StreetPosition);
        RespondingDriver.SetTaxiRide(this);
        return true;
    }
    public void Cancel()
    {
        IsActive = false;
       // RespondingDriver?.ReleaseTasking();     
    }
    public void Update()
    {
        if(RespondingVehicle == null || !RespondingVehicle.Vehicle.Exists())
        {
            IsValid = false;
            return;
        }
        if (RespondingDriver == null || !RespondingDriver.Pedestrian.Exists())
        {
            IsValid = false;
            return;
        }
        if (!HasPickedUpPlayer && Player.CurrentVehicle != null && Player.CurrentVehicle.Handle == RespondingVehicle.Handle)
        {
            OnPickedUpPlayer();
        }
        if(HasPickedUpPlayer && !HasDroppedOffPlayer && HasDestination && RespondingDriver.Position.DistanceTo2D(DestinationLocation.StreetPosition) <= 15f)
        {
            OnArrivedAtDestination();
        }
    }
    private void OnArrivedAtDestination()
    {
        EntryPoint.WriteToConsole("TAXI RIDE PLAYER HAS ARRIVED");
        HasArrivedAtDestination = true;
    }
    private void OnPickedUpPlayer()
    {
        EntryPoint.WriteToConsole("TAXI RIDE PLAYER WAS PICKED UP");
        HasPickedUpPlayer = true;
    }
    public void UpdateDestination(Vector3 coordinates)
    {
        if(coordinates == Vector3.Zero)
        {
            return;
        }
        if(RespondingDriver == null)
        {
            return;
        }
        //HasArrivedAtDestination = false;
        DestinationLocation.StreetPosition = coordinates;
        //RespondingDriver.SetTaskingActive(coordinates);
        EntryPoint.WriteToConsole($"TAXI RIDE UPDATED DESTINATION {coordinates}");
    }

    public void SetActive()
    {
        IsActive = true;
    }
}

