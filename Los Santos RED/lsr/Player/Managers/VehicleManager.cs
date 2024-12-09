using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VehicleManager
{
    private IVehicleManageable Player;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;

    private List<StoredVehicle> PersistantVehicles = new List<StoredVehicle>();

    public VehicleManager(IVehicleManageable player, IEntityProvideable world, ISettingsProvideable settings)
    {
        Player = player;
        World = world;
        Settings = settings;
    }

    public void Setup()
    {

    }
    public void Reset()
    {
        UnPersistAll();
    }
    public void Dispose()
    {
        UnPersistAll();
    }
    public void Update()
    {
        if(!Player.IsInVehicle)
        {
            return;
        }
        if (Player.CurrentVehicle == null) 
        {
            return;
        }
        if(!Player.CurrentVehicle.Vehicle.Exists())
        {
            return;
        }
        if (Player.CurrentVehicle.Vehicle.IsPersistent)
        {
            return;
        }
        if(PersistantVehicles.Any(x=> x.VehicleExt != null && x.VehicleExt.Handle == Player.CurrentVehicle.Handle))
        {
            return;
        }
        if(PersistantVehicles.Count() >= Settings.SettingsManager.VehicleSettings.PlayerEnteredPersistantVehicleLimit)
        {
            RemoveOldest();
        }
        AddCurrent();
    }

    private void AddCurrent()
    {
        if (Player.CurrentVehicle == null)
        {
            return;
        }
        if (!Player.CurrentVehicle.Vehicle.Exists())
        {
            return;
        }
        if (Player.CurrentVehicle.Vehicle.IsPersistent)
        {
            return;
        }
        if(Player.VehicleOwnership.OwnedVehicles.Any(x=> x.Handle == Player.CurrentVehicle.Handle))
        {
            return;
        }
        Player.CurrentVehicle.Vehicle.IsPersistent = true;
        PersistantVehicles.Add(new StoredVehicle(Player.CurrentVehicle, Game.GameTime));
        EntryPoint.WriteToConsole($"Adding Vehicle to Persistant List {Player.CurrentVehicle.Handle}");
    }

    private void RemoveOldest()
    {
        StoredVehicle oldest = PersistantVehicles.OrderBy(x=> x.GameTimeAdded).FirstOrDefault();
        if(oldest == null)
        {
            return;
        }

        if (oldest.VehicleExt != null && oldest.VehicleExt.Vehicle.Exists())
        {
            oldest.VehicleExt.Vehicle.IsPersistent = false;
            EntryPoint.WriteToConsole($"REMOVING Vehicle FROM Persistant List {oldest.VehicleExt.Handle}");
        }
        PersistantVehicles.Remove(oldest);
    }

    private void UnPersistAll()
    {
        foreach (StoredVehicle storedVehicle in PersistantVehicles)
        {
            if (storedVehicle.VehicleExt.Vehicle.Exists())
            {
                storedVehicle.VehicleExt.Vehicle.IsPersistent = false;
            }
        }
        EntryPoint.WriteToConsole($"UNPERSISTED ALL VEHICLES");
    }

    public void OnTookOwnership(VehicleExt toOwn)
    {
        if(toOwn == null)
        {
            return;
        }
        PersistantVehicles.RemoveAll(x=> x.VehicleExt.Handle == toOwn.Handle);
    }

    private class StoredVehicle
    {
        public VehicleExt VehicleExt;
        public uint GameTimeAdded;

        public StoredVehicle(VehicleExt vehicleExt, uint gameTimeAdded)
        {
            VehicleExt = vehicleExt;
            GameTimeAdded = gameTimeAdded;
        }
    }
}

