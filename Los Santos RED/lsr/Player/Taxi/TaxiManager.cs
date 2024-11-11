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
    private uint GameTimeLastHailedCab;

    public List<TaxiRide> ActiveRides { get; private set; } = new List<TaxiRide>();
    public TaxiManager(ITaxiRideable player, IEntityProvideable world, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings)
    {
        Player = player;
        World = world;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
    }
    public void Dispose()
    {
        foreach (TaxiRide taxiRide in ActiveRides)
        {
            taxiRide.Dispose();
        }
        ActiveRides.Clear();
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
            taxiRide.Update();
        }


        if(Player.ActivityManager.IsHailingTaxi && Game.GameTime - GameTimeLastHailedCab >= 6000)
        {
            TaxiFirm closestFirm = GetClosestTaxiFirm();
            if (closestFirm != null && RequestService(closestFirm, true))
            {
                Game.DisplayHelp($"Hailed {closestFirm.ShortName}");
            }
            GameTimeLastHailedCab = Game.GameTime;
        }
        ActiveRides.RemoveAll(x => !x.IsValid || !x.IsActive);
    }
    public void OnGotInVehicle()
    {
        if (Player.CurrentVehicle != null && Player.CurrentVehicle.IsTaxi)
        {
            GetOrCreateRide(Player.CurrentVehicle);
            EntryPoint.WriteToConsole("TaxiManager CREATE RIDE ON GETIN");
        }
    }
    public void OnGotOutOfVehicle()
    {
        foreach(TaxiRide taxiRide in ActiveRides)
        {
            taxiRide.OnGotOutOfVehicle();
        }
    }
    public bool RequestService(TaxiFirm taxiFirm, bool isHailed)
    {
        if(taxiFirm == null)
        {
            EntryPoint.WriteToConsole($"RequestService FAIL, NO TAXI FIRM");
            return false;
        }
        if(ActiveRides.Any(x=> x.RequestedFirm.ID == taxiFirm.ID))
        {
            EntryPoint.WriteToConsole($"RequestService FAIL, ALREADY ACTIVE RIDE");
            return false;
        }

        TaxiRide taxiRide = new TaxiRide(World, Player, taxiFirm, Player.Position);
        taxiRide.CanSpawnRide = !isHailed;
        if (isHailed)
        {
            taxiRide.SetupHailedRide();
        }
        else
        {
            taxiRide.SetupCalledRide();
        }
        EntryPoint.WriteToConsole($"RequestService RAN SETUP");
        if (!taxiRide.IsActive)
        {
            EntryPoint.WriteToConsole($"RequestService FAIL, NOT ACTIVE");
            return false;
        }
        ActiveRides.Add(taxiRide);
        EntryPoint.WriteToConsole("TaxiManager RequestService Active Ride Added");
        return true;
    }
    private TaxiFirm GetClosestTaxiFirm()
    {
        TaxiVehicleExt RespondingVehicle = World.Vehicles.TaxiVehicles.Where(x => x.Vehicle.Exists() && x.Vehicle.HasDriver && !x.Vehicle.HasPassengers && x.Vehicle.IsOnScreen && x.Vehicle.Position.DistanceTo2D(Player.Position) <= 50f).OrderBy(x => x.Vehicle.Position.DistanceTo2D(Player.Position)).FirstOrDefault();
        if (RespondingVehicle == null || !RespondingVehicle.Vehicle.Exists() || !RespondingVehicle.Vehicle.Driver.Exists())
        {
            return null;
        }
        return RespondingVehicle.TaxiFirm;
    }


    public void CancelRide(TaxiRide taxiRide, bool showNotification)
    {
        if (taxiRide == null)
        {
            EntryPoint.WriteToConsole("TaxiManager Cancel Ride no active ride found");
            return;
        }
        if (showNotification)
        {
            taxiRide.DisplayNotification("~r~Ride Cancelled", "You have cancelled this taxi ride. Please wait for the vehicle to stop before exiting.");
        }
        Player.ActivityManager.LeaveVehicle(true);
        taxiRide.Cancel();
        ActiveRides.Remove(taxiRide);
    }
    public TaxiRide GetOrCreateRide(VehicleExt regularVehicle)
    {
        if(regularVehicle == null)
        {
            return null;
        }
        return GetOrCreateRide(World.Vehicles.TaxiVehicles.FirstOrDefault(x=> x.Handle == regularVehicle.Handle));
    }
    public TaxiRide GetOrCreateRide(TaxiVehicleExt taxiVehicleExt)
    {
        if(taxiVehicleExt == null)
        {
            EntryPoint.WriteToConsole("GetOrCreateRide FAIL 1");
            return null;
        }
        TaxiRide tr = ActiveRides.FirstOrDefault(x => x.RespondingVehicle != null && taxiVehicleExt != null && x.RespondingVehicle.Handle == taxiVehicleExt.Handle);
        if (tr == null)
        {
            if(!taxiVehicleExt.Vehicle.Exists() || !taxiVehicleExt.Vehicle.Driver.Exists())
            {
                EntryPoint.WriteToConsole("GetOrCreateRide FAIL 2");
                return null;
            }
            uint driverhandle = taxiVehicleExt.Vehicle.Driver.Handle;
            TaxiDriver RespondingDriver = World.Pedestrians.TaxiDriverList.FirstOrDefault(x => x.Handle == driverhandle);
            if(RespondingDriver == null)
            {
                EntryPoint.WriteToConsole("GetOrCreateRide FAIL 3");
                return null;
            }
            tr = new TaxiRide(World, Player, taxiVehicleExt.TaxiFirm, taxiVehicleExt, RespondingDriver, Player.Position);
            RespondingDriver.SetTaxiRide(tr);
            tr.SetActive();
            if (taxiVehicleExt != null && taxiVehicleExt.TaxiFirm != null && taxiVehicleExt.TaxiFirm.PhoneContact != null)
            {
                Player.CellPhone.AddContact(taxiVehicleExt.TaxiFirm.PhoneContact, true);
            }
            ActiveRides.Add(tr);
            EntryPoint.WriteToConsole($"CREATE NEW TAXI RIDE WITH DRIVER:{RespondingDriver.Handle} VEH:{taxiVehicleExt.Handle}");
        }
        return tr;
    }


    public void DismissClosestCab()
    {
        ActiveRides.Where(x => x.RespondingDriver != null && (x.IsHailed || x.RespondingDriver.DistanceToPlayer <= 50f)).OrderBy(x => x.RespondingDriver.DistanceToPlayer).FirstOrDefault()?.Cancel();
    }
}

