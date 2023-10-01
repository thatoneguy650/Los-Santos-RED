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

    public TaxiRide(IEntityProvideable world, ITaxiRideable player, TaxiFirm requestedFirm, Vector3 pickupLocation)
    {
        World = world;
        Player = player;
        RequestedFirm = requestedFirm;
        InitialPickupLocation = pickupLocation;
    }

    public TaxiFirm RequestedFirm { get; set; }
    public SpawnLocation PickupLocation { get; set; }
    public TaxiVehicleExt RespondingVehicle { get; private set; }
    public TaxiDriver RespondingDriver { get; private set; }
    public bool CanStart { get; private set; }
    public void Setup()
    {
        CanStart = false;
        PickupLocation = new SpawnLocation(InitialPickupLocation);
        PickupLocation.GetClosestStreet(true);
        if(!PickupLocation.HasStreetPosition)
        {
            return;
        }
        if (GetVehicleAndDriver())//if there is an existing one
        {
            CanStart = true;
            return;
        }
        SpawnVehicleAndDriver();
        if (GetVehicleAndDriver())//if there is an existing one
        {
            CanStart = true;
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
        RespondingDriver.SetTaskingActive(PickupLocation.StreetPosition);
        return true;
    }
}

