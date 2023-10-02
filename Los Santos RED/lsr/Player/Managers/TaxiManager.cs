using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TaxiManager
{
    private bool HasRequestedService = false;
    private ITaxiRideable Player;
    private IEntityProvideable World;
    private IPlacesOfInterest PlacesOfInterest;
    private ISettingsProvideable Settings;
    public List<TaxiRide> ActiveRides { get; private set; } = new List<TaxiRide>();
    public TaxiManager(ITaxiRideable player, IEntityProvideable world, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings)
    {
        Player = player;
        World = world;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
    }
    public void Reset()
    {
        foreach(TaxiRide taxiRide in ActiveRides)
        {
            taxiRide.Cancel();
        }
        ActiveRides.Clear();
        EntryPoint.WriteToConsole("TaxiManager Reset");
    }
    public void Update()
    {
        foreach (TaxiRide taxiRide in ActiveRides)
        {
            taxiRide.CheckValid();
        }
        ActiveRides.RemoveAll(x => !x.IsValid);
    }
    public bool RequestService(TaxiFirm taxiFirm)
    {
        if(taxiFirm == null)
        {
            return false;
        }
        if(ActiveRides.Any(x=> x.RequestedFirm.ID == taxiFirm.ID))
        {
            return false;
        }
        TaxiRide taxiRide = new TaxiRide(World, Player, taxiFirm, Player.Position);
        taxiRide.Setup();
        if(!taxiRide.CanStart)
        {
            return false;
        }
        ActiveRides.Add(taxiRide);
        EntryPoint.WriteToConsole("TaxiManager RequestService Active Ride Added");
        return true;
    }

    //public void CancelRide(TaxiVehicleExt taxiVehicleExt)
    //{
    //    if(taxiVehicleExt == null)
    //    {
    //        return;
    //    }
    //    TaxiRide taxiRide = ActiveRides.Where(x=> x.RespondingVehicle != null && x.RespondingVehicle.Handle == taxiVehicleExt.Handle).FirstOrDefault();
    //    if(taxiRide == null)
    //    {
    //        EntryPoint.WriteToConsole("TaxiManager Cancel Ride no active ride found");
    //        return;
    //    }
    //    taxiRide.Cancel();
    //    ActiveRides.Remove(taxiRide);
    //}
    public void CancelRide(TaxiRide taxiRide)
    {
        if (taxiRide == null)
        {
            EntryPoint.WriteToConsole("TaxiManager Cancel Ride no active ride found");
            return;
        }
        taxiRide.Cancel();
        ActiveRides.Remove(taxiRide);
    }
    public TaxiRide GetOrCreateRide(TaxiVehicleExt taxiVehicleExt)
    {
        if(taxiVehicleExt == null)
        {
            return null;
        }
        TaxiRide tr = ActiveRides.FirstOrDefault(x => x.RespondingVehicle != null && taxiVehicleExt != null && x.RespondingVehicle.Handle == taxiVehicleExt.Handle);
        if (tr == null)
        {
            if(!taxiVehicleExt.Vehicle.Exists() || !taxiVehicleExt.Vehicle.Driver.Exists())
            {
                return null;
            }
            uint driverhandle = taxiVehicleExt.Vehicle.Driver.Handle;
            TaxiDriver RespondingDriver = World.Pedestrians.TaxiDriverList.FirstOrDefault(x => x.Handle == driverhandle);
            if(RespondingDriver == null)
            {
                return null;
            }
            tr = new TaxiRide(World, Player, taxiVehicleExt.TaxiFirm, taxiVehicleExt, RespondingDriver);
        }
        return tr;
    }
}

