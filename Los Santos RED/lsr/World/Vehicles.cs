using ExtensionsMethods;
using LosSantosRED.lsr;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


public class Vehicles
{
    private readonly float DistanceToScan = 450f;
    private readonly List<VehicleExt> PoliceVehicles = new List<VehicleExt>();
    private readonly List<VehicleExt> CivilianVehicles = new List<VehicleExt>();
    public int PoliceHelicoptersCount
    {
        get
        {
            return Mod.World.Vehicles.PoliceVehicles.Count(x => x.Vehicle.IsHelicopter);
        }
    }
    public int PoliceBoatsCount
    {
        get
        {
            return Mod.World.Vehicles.PoliceVehicles.Count(x => x.Vehicle.IsBoat);
        }
    }
    public void Scan()
    {
        CleanLists();
        Vehicle[] RageVehicles = Array.ConvertAll(Rage.World.GetEntities(Game.LocalPlayer.Character.Position, DistanceToScan, GetEntitiesFlags.ConsiderAllVehicles).Where(x => x is Vehicle && x.Exists()).ToArray(), x => (Vehicle)x);//250
        foreach (Vehicle Veh in RageVehicles.Where(x=> x.Exists()))
        {
            AddToList(Veh);
        }
    }
    public void CleanLists()
    {
        CivilianVehicles.RemoveAll(x => !x.Vehicle.Exists());
        PoliceVehicles.RemoveAll(x => !x.Vehicle.Exists());
    }
    public void UpdatePlates()
    {
        int VehiclesUpdated = 0;
        foreach (VehicleExt MyCar in CivilianVehicles.Where(x => x.Vehicle.Exists() && !x.HasUpdatedPlateType))
        {
            MyCar.UpdatePlate();
            VehiclesUpdated++;
            if (VehiclesUpdated > 5)
            {
                break;
            }
        }
    }
    public void ClearPolice()
    {
        foreach (VehicleExt Veh in PoliceVehicles)
        {
            if (Veh.Vehicle.Exists())
            {
                Veh.Vehicle.Delete();
            }
        }
        PoliceVehicles.Clear();
    }
    public VehicleExt GetVehicle(Vehicle ToFind)
    {
        VehicleExt ToReturn = PoliceVehicles.FirstOrDefault(x => x.Vehicle.Handle == ToFind.Handle);
        if(ToReturn == null)
        {
            ToReturn = CivilianVehicles.FirstOrDefault(x => x.Vehicle.Handle == ToFind.Handle);
        }
        return ToReturn;
    }
    public void AddToList(Vehicle Veh)
    {
        if (Veh.Exists())
        {
            VehicleExt Car = new VehicleExt(Veh);
            if (Veh.IsPoliceVehicle)
            {
                if (!PoliceVehicles.Any(x => x.Vehicle.Handle == Veh.Handle))
                {
                    Mod.World.PoliceSpawning.UpdateLivery(Veh);
                    Mod.World.PoliceSpawning.UpgradeCruiser(Veh);

                    PoliceVehicles.Add(Car);
                }
            }
            else
            {
                if (!CivilianVehicles.Any(x => x.Vehicle.Handle == Veh.Handle))
                {
                    CivilianVehicles.Add(Car);
                }
            }
        }
    }
    public void AddToList(VehicleExt Veh)
    {
        if (Veh.Vehicle.Exists())
        {
            if (Veh.Vehicle.IsPoliceVehicle)
            {
                if (!PoliceVehicles.Any(x => x.Vehicle.Handle == Veh.Vehicle.Handle))
                {
                    Mod.World.PoliceSpawning.UpdateLivery(Veh.Vehicle);
                    Mod.World.PoliceSpawning.UpgradeCruiser(Veh.Vehicle);

                    PoliceVehicles.Add(Veh);
                }
            }
            else
            {
                if (!CivilianVehicles.Any(x => x.Vehicle.Handle == Veh.Vehicle.Handle))
                {
                    CivilianVehicles.Add(Veh);
                }
            }
        }
    }
}
