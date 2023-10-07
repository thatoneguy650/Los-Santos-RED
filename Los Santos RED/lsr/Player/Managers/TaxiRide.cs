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
    public float EsimatedDistance { get; set; }
    public string DesitnationName { get; set; }
    public bool HasSetFast { get; set; } = false;
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
            EntryPoint.WriteToConsole("TAXI RIDE SETUP FAIL NO STREET POSITION");
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
        EntryPoint.WriteToConsole("TAXI RIDE SETUP FAIL NO DISPATCH");
    }
    private void SpawnVehicleAndDriver()
    {
        EntryPoint.WriteToConsole("TAXI RIDE SETUP FORCING TAXI SPAWN");
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
        if(RespondingDriver != null)
        {
            RespondingDriver.TaxiRide = null;
        }
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
        if(!HasArrivedAtDestination && HasPickedUpPlayer && !HasDroppedOffPlayer && HasDestination && RespondingDriver.Position.DistanceTo2D(DestinationLocation.StreetPosition) <= 15f)
        {
            OnArrivedAtDestination();
        }
    }
    private void OnArrivedAtDestination()
    {
        EntryPoint.WriteToConsole("TAXI RIDE PLAYER HAS ARRIVED");
        ChargePlayer();
        HasArrivedAtDestination = true;
    }
    private void ChargePlayer()
    {
        int fullFare = GetPrice(EsimatedDistance);
        if (fullFare > 0)
        {
            Player.BankAccounts.GiveMoney(-1 * fullFare, true);
            DisplayNotification("~g~Arrived", $"Total Price: ~r~${fullFare}~s~");
        }
    }

    private void OnPickedUpPlayer()
    {
        EntryPoint.WriteToConsole("TAXI RIDE PLAYER WAS PICKED UP");
        HasPickedUpPlayer = true;
    }
    public void UpdateDestination(Vector3 destinationCoordinates, Vector3 startCoordinates, string Name)
    {
        if(destinationCoordinates == Vector3.Zero)
        {
            return;
        }
        if (startCoordinates == Vector3.Zero)
        {
            return;
        }
        if (RespondingDriver == null)
        {
            return;
        }
        if(HasDestination && !HasArrivedAtDestination)
        {
            EntryPoint.WriteToConsole($"TAXI RIDE UPDATED DESTINATION ALREADY HAS DESTINATION CHARGING");
            int PartialPrice = GetPrice(startCoordinates.DistanceTo2D(Player.Position));
            if(PartialPrice > 0)
            {
                Player.BankAccounts.GiveMoney(-1 * PartialPrice, true);
                DisplayNotification("~r~Destination Updated",$"Partial Ride Price: ~r~${PartialPrice}");
            }
        }
        else
        {
            DisplayNotification("~g~Destination Added", $"Added New Destination: {Name}");
        }
        PickupLocation = new SpawnLocation(startCoordinates);
        PickupLocation.StreetPosition = startCoordinates;
        DestinationLocation.StreetPosition = destinationCoordinates;
        EsimatedDistance = startCoordinates.DistanceTo2D(DestinationLocation.StreetPosition);
        DesitnationName = Name;
        EntryPoint.WriteToConsole($"TAXI RIDE UPDATED DESTINATION {destinationCoordinates}");
    }
    public int GetPrice(float distance)
    {
        int totalFare = 20;
        float smallerDistance = distance / 100f;
        int AdditionalFare = (int)Math.Floor(smallerDistance) * 3;
        return totalFare + AdditionalFare;
    }
    public void SetActive()
    {
        IsActive = true;
    }
    public void OnGotOutOfVehicle()
    {
        if(!IsActive)
        {
            return;
        }
        if(HasArrivedAtDestination)
        {
            Cancel();
            return;
        }
        else 
        {
            Game.DisplayHelp("Return to the taxi to continue the ride.");
        }
    }

    public void DisplayNotification(string subtitle, string text)
    {
        if (string.IsNullOrEmpty(subtitle) || string.IsNullOrEmpty(text))
        {
            return;
        }
        string texture = "CHAR_BLANK_ENTRY";
        string title = "Taxi Company";
        if (RequestedFirm != null)
        {
            title = RequestedFirm.ShortName;
            if (RequestedFirm.PhoneContact != null)
            {
                texture = RequestedFirm.PhoneContact.IconName;
            }
        }
        Game.DisplayNotification(texture, texture, title, subtitle, text);
    }
}

