using ExtensionsMethods;
using LosSantosRED.lsr;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


public class VehicleManager
{
    private readonly float DistanceToScan = 450f;
    public List<Vehicle> PoliceVehicles { get; private set; } = new List<Vehicle>();
    public List<VehicleExt> CivilianVehicles { get; private set; } = new List<VehicleExt>();
    public void Dispose()
    {
        ClearPolice();
    }
    public void Tick()
    {
        ScanForVehicles();
        UpdateVehiclePlates();  
    }
    private void ScanForVehicles()
    {
        Vehicle[] Vehicles = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, DistanceToScan, GetEntitiesFlags.ConsiderAllVehicles).Where(x => x is Vehicle && x.Exists()).ToArray(), (x => (Vehicle)x));//250
        foreach (Vehicle Veh in Vehicles)
        {
            AddToLists(Veh);
        }
    }
    private void UpdateVehiclePlates()
    {
        int VehiclesUpdated = 0;
        foreach (VehicleExt MyCar in CivilianVehicles.Where(x => x.VehicleEnt.Exists() && !x.HasUpdatedPlateType))
        {
            MyCar.UpdatePlate();
            VehiclesUpdated++;
            if (VehiclesUpdated > 5)
            {
                break;
            }
        }
    }
    private void AddToLists(Vehicle Veh)
    {
        if (Veh.IsPoliceVehicle)
        {
            if (!PoliceVehicles.Any(x => x.Handle == Veh.Handle))
            {
                Mod.PoliceSpawningManager.UpdateLivery(Veh);
                Mod.PoliceSpawningManager.UpgradeCruiser(Veh);
                PoliceVehicles.Add(Veh);
            }
        }
        else
        {
            if (!CivilianVehicles.Any(x => x.VehicleEnt.Handle == Veh.Handle))
            {
                CivilianVehicles.Add(new VehicleExt(Veh));
            }
        }
    }
    public void ClearPolice()
    {
        foreach (Vehicle Veh in PoliceVehicles)
        {
            if (Veh.Exists())
            {
                Veh.Delete();
            }
        }
        PoliceVehicles.Clear();
    }
}
