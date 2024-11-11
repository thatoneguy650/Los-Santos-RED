using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


public class TaxiRide
{
    private Vector3 InitialPickupLocation;
    private IEntityProvideable World;
    private ITaxiRideable Player;
    private Blip PickupBlip;
    private uint GameTimeArrivedAtPickup;
    private bool HasDonePickupItems;
    private bool IsWaitingOnPlayerAfterGetOut;

    private SpawnLocation PulloverLocation;
    public TaxiRide(IEntityProvideable world, ITaxiRideable player, TaxiFirm requestedFirm, TaxiVehicleExt respondingVehicle, TaxiDriver respondingDriver, Vector3 pickupLocation)
    {
        World = world;
        Player = player;
        HasPickedUpPlayer = true;
        IsNearbyPickup = false;
        GameTimeArrivedAtPickup = Game.GameTime;
        RequestedFirm = requestedFirm;
        InitialPickupLocation = pickupLocation;
        RespondingVehicle = respondingVehicle;
        RespondingDriver = respondingDriver;
        TaxiDrivingStyle = new PedDrivingStyle("Normal", eCustomDrivingStyles.Taxi_StandardDrivingMode, 10f);
        PossibleTaxiDrivingStyles = new List<PedDrivingStyle>() {
            new PedDrivingStyle("Normal", eCustomDrivingStyles.Taxi_StandardDrivingMode, 15f),
            new PedDrivingStyle("Step On It", eCustomDrivingStyles.TaxiRushed_AvoidCarsObeyLights, 42f) { Fee = RequestedFirm == null ? 0 : RequestedFirm.FastSpeedFee },
            new PedDrivingStyle("Crazy", eCustomDrivingStyles.TaxiCrazy_AvoidCars, 90f) { Fee = RequestedFirm == null ? 0 : RequestedFirm.CrazySpeedFee },
        };
        PickupLocation = new SpawnLocation();
        DestinationLocation = new SpawnLocation();
    }
    public TaxiRide(IEntityProvideable world, ITaxiRideable player, TaxiFirm requestedFirm, Vector3 pickupLocation)
    {
        World = world;
        Player = player;
        RequestedFirm = requestedFirm;
        InitialPickupLocation = pickupLocation;
        TaxiDrivingStyle = new PedDrivingStyle("Normal", eCustomDrivingStyles.Taxi_StandardDrivingMode, 10f);
        PossibleTaxiDrivingStyles = new List<PedDrivingStyle>() {
            new PedDrivingStyle("Drive Normal", eCustomDrivingStyles.Taxi_StandardDrivingMode, 15f),
            new PedDrivingStyle("Step On It", eCustomDrivingStyles.TaxiRushed_AvoidCarsObeyLights, 42f) { Fee = RequestedFirm == null ? 0 : RequestedFirm.FastSpeedFee },
            new PedDrivingStyle("Crazy", eCustomDrivingStyles.TaxiCrazy_AvoidCars, 90f) { Fee = RequestedFirm == null ? 0 : RequestedFirm.CrazySpeedFee },
        };
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
            else if (HasPickedUpPlayer && (IsWaitingOnPlayerAfterGetOut || IsWaitingOnPlayer) && HasPullover)
            {
                return PulloverLocation.StreetPosition;
            }
            //else if (HasPickedUpPlayer && IsWaitingOnPlayer && HasPullover)
            //{
            //    return PulloverLocation.StreetPosition;
            //}
            else if (HasPickedUpPlayer && HasDestination)
            {
                return DestinationLocation.StreetPosition;
            }
            else if(HasArrivedAtDestination)
            {
                return DestinationLocation.StreetPosition;
            }
            else if (RespondingVehicle != null && RespondingVehicle.Vehicle.Exists())
            {
                return RespondingVehicle.Vehicle.Position;
            }
            return Vector3.Zero;
        }
    }
    public float CurrentDriveToHeading
    {
        get
        {
            if (!HasPickedUpPlayer && HasPickup)
            {
                return PickupLocation.Heading;
            }
            else if (HasPickedUpPlayer && HasDestination)
            {
                return DestinationLocation.Heading;
            }
            return 0f;
        }
    }
    public float EstimatedDistance { get; set; }
    public float EstimatedDistanceMiles { get; set; }
    public List<PedDrivingStyle> PossibleTaxiDrivingStyles { get; private set; }
    public string DestinationName { get; set; }
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

    public bool HasPullover => PulloverLocation != null && PulloverLocation.HasStreetPosition && PulloverLocation.StreetPosition != Vector3.Zero;

    public bool HasDestination => DestinationLocation != null && DestinationLocation.StreetPosition != Vector3.Zero;
    public bool HasPickup => PickupLocation != null && PickupLocation.StreetPosition != Vector3.Zero;
    public bool IsNearbyDestination { get; private set; }
    public bool IsNearbyPickup { get; private set; }
    public bool IsWaitingOnPlayer { get; private set; }
    public bool CanSpawnRide { get; set; } = true;

    public bool IsHailed { get; set; } = false;
    public void SetupHailedRide()
    {
        IsActive = false;
        IsWaitingOnPlayer = false;
        IsHailed = true;
        EntryPoint.WriteToConsole("SetupHailedRide START");
        if(!GetVehicleAndDriver() || RespondingVehicle == null || !RespondingVehicle.Vehicle.Exists())
        {
            EntryPoint.WriteToConsole("Taxi Hailed Ride Return 1");
            return;
        }
        PickupLocation = new SpawnLocation(RespondingVehicle.Vehicle.GetOffsetPositionFront(10f));
        DestinationLocation = new SpawnLocation();
        PickupLocation.GetClosestStreet(false);
        PickupLocation.GetRoadBoundaryPosition();
        if (PickupLocation.HasRoadBoundaryPosition)
        {
            PickupLocation.StreetPosition = PickupLocation.RoadBoundaryPosition;
        }
        if (!PickupLocation.HasStreetPosition)
        {
            EntryPoint.WriteToConsole("Taxi Hailed Ride Return 2");
            return;
        }
        if (GetVehicleAndDriver())//if there is an existing one
        {
            IsActive = true;
            AddPickupBlip();
            //if (RequestedFirm != null && RequestedFirm.PhoneContact != null)
            //{
            //    Player.CellPhone.AddContact(RequestedFirm.PhoneContact, true);
            //}
            EntryPoint.WriteToConsole("Taxi Hailed Ride Return 3");
            return;
        }
        EntryPoint.WriteToConsole("SetupHailedRide END NO DISPATCH?");
    }



    public void SetupCalledRide()
    {
        IsActive = false;
        IsWaitingOnPlayer = false;
        IsHailed = false;
        EntryPoint.WriteToConsole("SetupCalledRide");
        PickupLocation = new SpawnLocation(InitialPickupLocation);
        DestinationLocation = new SpawnLocation();
        PickupLocation.GetClosestStreet(false);
        PickupLocation.GetClosestSideOfRoad();
        PickupLocation.GetRoadBoundaryPosition();
        if (PickupLocation.HasRoadBoundaryPosition)
        {
            PickupLocation.StreetPosition = PickupLocation.RoadBoundaryPosition;
        }
        if (!PickupLocation.HasStreetPosition)
        {
            EntryPoint.WriteToConsole("Taxi Called Ride Return 1");
            return;
        }
        if (GetVehicleAndDriver())//if there is an existing one
        {
            IsActive = true;
            AddPickupBlip();
            EntryPoint.WriteToConsole("Taxi Called Ride Return 2");
            //Player.GPSManager.AddGPSRoute("Taxi Pickup", PickupLocation.FinalPosition);
            return;
        }
        if(!CanSpawnRide)
        {
            return;
        }
        SpawnVehicleAndDriver();
        if (GetVehicleAndDriver())//if there is an existing one
        {
            IsActive = true;
            AddPickupBlip();
            EntryPoint.WriteToConsole("Taxi Called Ride Return 3");
            return;
        }
        EntryPoint.WriteToConsole("SetupCalledRide END NO DISPATCH?");
    }
    public void Update()
    {
        if (RespondingVehicle == null || !RespondingVehicle.Vehicle.Exists())
        {
            Cancel();
            IsValid = false;
            return;
        }
        if (RespondingDriver == null || !RespondingDriver.Pedestrian.Exists())
        {
            Cancel();
            IsValid = false;
            return;
        }
        if (RespondingDriver.WillCancelRide || (Player.IsWanted && Player.PoliceResponse.WantedLevelHasBeenRadioedIn && Player.AnyPoliceRecentlySeenPlayer && !Player.IsInSearchMode) || Player.IsDead)
        {
            Cancel();
            IsValid = false;
            return;
        }
        if(HasPickedUpPlayer && (Player.CurrentVehicle == null || Player.CurrentVehicle.Handle != RespondingVehicle.Handle) && Player.Position.DistanceTo2D(RespondingDriver.Pedestrian) >= 50f)
        {
            EntryPoint.WriteToConsole("YOU GOT OUT OF THE RIDE AND WENT AWAY, CANCELLED");
            IsValid = false;
            Cancel();
            return;
        }
        if (!HasPickedUpPlayer)
        {
            if (Player.CurrentVehicle != null && Player.CurrentVehicle.Handle == RespondingVehicle.Handle)
            {
                OnPickedUpPlayer();
            }
            float distanceToPickup = RespondingDriver.Position.DistanceTo2D(PickupLocation.StreetPosition);
            if(!IsNearbyPickup && distanceToPickup <= 20f)
            {
                OnArrivedAtPickup();
            }
            if(IsNearbyPickup && Game.GameTime - GameTimeArrivedAtPickup >= 5000 && !HasDonePickupItems)
            {
                OnWaitedAtPickup();
            }
        }
        if (!HasArrivedAtDestination && HasPickedUpPlayer && !HasDroppedOffPlayer && HasDestination)
        {
            float distanceToDestination = RespondingDriver.Position.DistanceTo2D(DestinationLocation.StreetPosition);
            if (distanceToDestination <= 20f)
            {
                OnArrivedAtDestination();
            }
            else if(!IsNearbyDestination && distanceToDestination <= 125f)
            {
                OnIsNearbyDestination();
            }
        }
        if (IsWaitingOnPlayerAfterGetOut && Player.CurrentVehicle != null && Player.CurrentVehicle.Handle == RespondingVehicle.Handle)
        {
            OnPlayerReturnedToTaxi();
        }
    }
    public void Cancel()
    {
        if(IsActive)
        {

            Game.DisplayHelp("Ride Cancelled");
        }
        IsActive = false;
        EntryPoint.WriteToConsole("Taxi Ride IS ACTIVE SET TO FALSE");
        if (RespondingDriver != null)
        {
            RespondingDriver.TaxiRide = null;
            RespondingDriver.ClearTasks(true);
            RespondingDriver.SetNonPersistent();
        }
        if (PickupBlip.Exists())
        {
            PickupBlip.Delete();
        }
        EntryPoint.WriteToConsole("TAXI RIDE HAS BEEN CANCELLED");

        // RespondingDriver?.ReleaseTasking();     
    }
    public void Dispose()
    {
        Cancel();
    }
    public void UpdateDestination(Vector3 destinationCoordinates, Vector3 startCoordinates, string Name)
    {
        if (destinationCoordinates == Vector3.Zero)
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
        if (HasDestination && !HasArrivedAtDestination)
        {
            EntryPoint.WriteToConsole($"TAXI RIDE UPDATED DESTINATION ALREADY HAS DESTINATION CHARGING");
            int PartialPrice = GetPrice(startCoordinates.DistanceTo2D(Player.Position) * 0.000621371f);
            if (PartialPrice > 0)
            {
                Player.BankAccounts.GiveMoney(-1 * PartialPrice, true);
                DisplayNotification("~r~Destination Updated", $"Partial Ride Price: ~r~${PartialPrice}");
            }
        }
        else
        {
            DisplayNotification("~g~Destination Added", $"Added New Destination: {Name}");
        }
        PickupLocation = new SpawnLocation(startCoordinates);
        PickupLocation.StreetPosition = startCoordinates;
        DestinationLocation.StreetPosition = destinationCoordinates;
        EstimatedDistance = startCoordinates.DistanceTo2D(DestinationLocation.StreetPosition);
        EstimatedDistanceMiles = EstimatedDistance * 0.000621371f;
        DestinationName = Name;
        HasArrivedAtDestination = false;
        IsNearbyDestination = false;
        EntryPoint.WriteToConsole($"TAXI RIDE UPDATED DESTINATION {destinationCoordinates}");
        RespondingDriver?.PlaySpeech("TAXID_CHANGE_DEST", false);
    }
    public int GetPrice(float distance)
    {
        if (RequestedFirm == null)
        {
            return 0;
        }
        return RequestedFirm.CalculateFare(distance);
    }
    public void SetActive()
    {
        IsActive = true;
    }
    public void OnGotOutOfVehicle()
    {
        if (!IsActive)
        {
            return;
        }
        if (HasArrivedAtDestination || IsNearbyDestination)
        {
            FinishRide();
        }
        else if(HasPickedUpPlayer)
        {
            OnPlayerGotOutMidway();
        }
    }

    private void FinishRide()
    {
        if (IsActive)
        {
            Game.DisplayHelp("Taxi Ride Completed");
        }
        IsActive = false;
        EntryPoint.WriteToConsole("Taxi Ride IS ACTIVE SET TO FALSE FINISH");
        if (RespondingDriver != null)
        {
            RespondingDriver.TaxiRide = null;
            RespondingDriver.ClearTasks(true);
            RespondingDriver.SetNonPersistent();
        }
        if (PickupBlip.Exists())
        {
            PickupBlip.Delete();
        }
        EntryPoint.WriteToConsole("TAXI RIDE HAS BEEN Completed");
    }

    private void OnPlayerReturnedToTaxi()
    {
        IsWaitingOnPlayerAfterGetOut = false;
        PulloverLocation = null;
        if (RespondingDriver == null)
        {
            return;
        }
        RespondingDriver.PlaySpeech("GENERIC_HOWS_IT_GOING", false);
        
    }
    private void OnPlayerGotOutMidway()
    {
        IsWaitingOnPlayerAfterGetOut = true;
        if(RespondingDriver == null)
        {
            return;
        }
        RespondingDriver.PlaySpeech("TAXID_GET_OUT_EARLY", false);
        Game.DisplayHelp("Return to the taxi to continue the ride.");
        SpawnLocation spawnLocation = new SpawnLocation(RespondingDriver.Position);
        spawnLocation.GetClosestStreet(false);
        spawnLocation.GetClosestSideOfRoad();
        spawnLocation.GetRoadBoundaryPosition();
        if (spawnLocation.HasRoadBoundaryPosition)
        {
            spawnLocation.StreetPosition = spawnLocation.RoadBoundaryPosition;
        }
        if (!spawnLocation.HasStreetPosition)
        {
            spawnLocation.StreetPosition = RespondingDriver.Position;
        }
        PulloverLocation = spawnLocation;
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
    private void OnArrivedAtPickup()
    {
        if (IsHailed)
        {
            DisplayNotification("~g~Driver Hailed", $"Your driver will attempt to pull over at the nearest safe location. Please wait for the vehicle to come to a complete stop before entering.");
        }
        else
        {
            DisplayNotification("~g~Pick Up Ready", $"Your driver is about to arrive at ~p~{DestinationName}~s~ for the pickup. Please wait for the vehicle to come to a complete stop before entering.");
        }
        IsNearbyPickup = true;
        EntryPoint.WriteToConsole("TAXI RIDE IS NEARBY PICKUP");
        GameTimeArrivedAtPickup = Game.GameTime;
        PickupBlip.Flash(1000, 15000);
    }
    private void OnWaitedAtPickup()
    {
        if (RespondingVehicle != null && RespondingVehicle.Vehicle.Exists())
        {
            NativeFunction.Natives.START_VEHICLE_HORN(RespondingVehicle.Vehicle, 2000, Game.GetHashKey("HELDDOWN"), 0);
        }
        HasDonePickupItems = true;
        EntryPoint.WriteToConsole("OnWaitedAtPickup RAN");
        RespondingDriver?.PlaySpeech("GENERIC_HOWS_IT_GOING", false);
    }
    private void OnPickedUpPlayer()
    {
        EntryPoint.WriteToConsole("TAXI RIDE PLAYER WAS PICKED UP");
        HasPickedUpPlayer = true;
        if (PickupBlip.Exists())
        {
            PickupBlip.Delete();
        }
        RespondingDriver?.PlaySpeech("TAXID_WHERE_TO", false);

        if(RequestedFirm != null && RequestedFirm.PhoneContact != null)
        {
            Player.CellPhone.AddContact(RequestedFirm.PhoneContact, true);
        }
    }
    private void OnIsNearbyDestination()
    {
        DisplayNotification("~g~Arriving", $"You are about to arrive at ~p~{DestinationName}~s~. Please wait for the vehicle to come to a complete stop before exiting. Thank your for your business.");
        IsNearbyDestination = true;
        EntryPoint.WriteToConsole("TAXI RIDE IS NEARBY DESTINATION");
        SpawnLocation spawnLocation = new SpawnLocation(DestinationLocation.StreetPosition);
        spawnLocation.GetClosestStreet(false);
        spawnLocation.GetClosestSideOfRoad();


        spawnLocation.GetRoadBoundaryPosition();
        if (spawnLocation.HasRoadBoundaryPosition)
        {
            spawnLocation.StreetPosition = spawnLocation.RoadBoundaryPosition;
        }



        if (spawnLocation.HasStreetPosition)
        {
            EntryPoint.WriteToConsole($"NEARBY DESTINATION UPDATED WAS {DestinationLocation.StreetPosition} NOW {spawnLocation.StreetPosition}");
            DestinationLocation.StreetPosition = spawnLocation.StreetPosition;
        }
        else
        {
            EntryPoint.WriteToConsole($"NEARBY DESTINATION UPDATED COULDNT FIND NEW POS");
        }
    }
    private void OnArrivedAtDestination()
    {
        EntryPoint.WriteToConsole("TAXI RIDE PLAYER HAS ARRIVED");
        int fullFare = GetPrice(EstimatedDistanceMiles);
        if (fullFare > 0)
        {
            Player.BankAccounts.GiveMoney(-1 * fullFare, true);
        }
        DisplayNotification("~g~Arrived", $"You have arrived at ~p~{DestinationName}~s~.~n~~n~Fare: ~r~${fullFare}~s~");
        HasArrivedAtDestination = true;
        IsNearbyDestination = true;
        RespondingDriver?.PlaySpeech("TAXID_ARRIVE_AT_DEST", false);
    }
    private void AddPickupBlip()
    {
        if (!HasPickedUpPlayer)
        {
            PickupBlip = new Blip(PickupLocation.StreetPosition) { Sprite = (BlipSprite)198, Scale = 0.7f, Color = EntryPoint.LSRedColor };
            if (PickupBlip.Exists())
            {
                NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)PickupBlip.Handle, true);
                NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");
                NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME("Pickup");
                NativeFunction.Natives.END_TEXT_COMMAND_SET_BLIP_NAME(PickupBlip);
                World.AddBlip(PickupBlip);
            }

            EntryPoint.WriteToConsole($"PICKUP BLIP CREATED");
        }
    }
    private void SpawnVehicleAndDriver()
    {
        EntryPoint.WriteToConsole("TAXI RIDE SETUP FORCING TAXI SPAWN");
        Player.Dispatcher.ForceTaxiSpawn(RequestedFirm);// Dispatcher.ForceTaxiSpawn(RequestedFirm);
    }
    private bool GetVehicleAndDriver()
    {
        foreach (TaxiVehicleExt respondingVehicle in World.Vehicles.TaxiVehicles.Where(x => x.TaxiFirm.ID == RequestedFirm.ID && x.Vehicle.Exists() && x.Vehicle.HasDriver).OrderBy(x => x.Vehicle.Position.DistanceTo2D(Player.Position)))
        {
            GameFiber.Yield();
            if (respondingVehicle == null || !respondingVehicle.Vehicle.Exists() || !respondingVehicle.Vehicle.Driver.Exists())
            {
                EntryPoint.WriteToConsole("GetVehicleAndDriver RETURN 1");
                continue;//return false;
            }
            uint driverhandle = respondingVehicle.Vehicle.Driver.Handle;
            RespondingVehicle = respondingVehicle;
            RespondingDriver = World.Pedestrians.TaxiDriverList.FirstOrDefault(x => x.Handle == driverhandle);
            if (RespondingDriver == null || !RespondingDriver.Pedestrian.Exists())
            {
                EntryPoint.WriteToConsole("GetVehicleAndDriver RETURN 2");
                continue; //return false;
            }
            if (RespondingDriver.WillCancelRide)
            {
                EntryPoint.WriteToConsole("GetVehicleAndDriver RETURN 3 WILL CANCEL RIDE!");
                continue; //return false;
            }
            //RespondingDriver.SetTaskingActive(PickupLocation.StreetPosition);
            EntryPoint.WriteToConsole("GetVehicleAndDriver FOUND DRIVER RETURNING TRUE");
            RespondingDriver.SetTaxiRide(this);
            return true;
        }
        return false;
    }

    public void Pullover()
    {
        SpawnLocation spawnLocation = new SpawnLocation(RespondingDriver.Position);
        spawnLocation.GetClosestStreet(false);
        spawnLocation.GetClosestSideOfRoad();
        if (!spawnLocation.HasStreetPosition)
        {
            spawnLocation.StreetPosition = RespondingDriver.Position;
        }
        spawnLocation.GetRoadBoundaryPosition();
        if (spawnLocation.HasRoadBoundaryPosition)
        {
            spawnLocation.StreetPosition = spawnLocation.RoadBoundaryPosition;
        }
        PulloverLocation = spawnLocation;
        IsWaitingOnPlayer = true;
    }
    public void ContinueRide()
    {
        IsWaitingOnPlayer = false;
    }

    public void UpdatePickupLocation()
    {
        SpawnLocation NewPickupLocation = new SpawnLocation(Player.Position);
        NewPickupLocation.GetClosestStreet(true);
        NewPickupLocation.GetClosestSideOfRoad();

        NewPickupLocation.GetRoadBoundaryPosition();
        if (NewPickupLocation.HasRoadBoundaryPosition)
        {
            NewPickupLocation.StreetPosition = NewPickupLocation.RoadBoundaryPosition;
        }

        if (!NewPickupLocation.HasStreetPosition)
        {
            Game.DisplayHelp("Could not find new pickup location");
            return;
        }
        PickupLocation.InitialPosition = NewPickupLocation.InitialPosition;
        PickupLocation.StreetPosition = NewPickupLocation.StreetPosition;
        UpdatePickupBlip();
    }

    public void SetPickupLocationAtPlayer()
    {
        PickupLocation.InitialPosition = Player.Position;
        PickupLocation.StreetPosition = Player.Position;
        UpdatePickupBlip();
    }
    private void UpdatePickupBlip()
    {
        if(!PickupBlip.Exists())
        {
            return;
        }
        PickupBlip.Position = PickupLocation.StreetPosition;
    }

    public void TeleportToPickup()
    {
        if(RespondingVehicle == null || !RespondingVehicle.Vehicle.Exists())
        {
            return;
        }
        Game.FadeScreenOut(1000, true);
        if (RespondingVehicle == null || !RespondingVehicle.Vehicle.Exists())
        {
            Game.FadeScreenIn(1000, true);
            return;
        }
        RespondingVehicle.Vehicle.Position = PickupLocation.StreetPosition;
        RespondingVehicle.Vehicle.Heading = PickupLocation.Heading;
        GameFiber.Sleep(1000);
        Game.FadeScreenIn(1000, true);
    }
}

