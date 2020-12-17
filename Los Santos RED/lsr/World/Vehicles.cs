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
    public List<VehicleExt> PoliceVehicles { get; private set; } = new List<VehicleExt>();
    public List<VehicleExt> CivilianVehicles { get; private set; } = new List<VehicleExt>();
    public void Dispose()
    {
        ClearPolice();
    }

    public void PruneVehicles()
    {
        CivilianVehicles.RemoveAll(x => !x.Vehicle.Exists());
        PoliceVehicles.RemoveAll(x => !x.Vehicle.Exists());
    }
    public void ScanForVehicles()
    {
        Vehicle[] Vehicles = Array.ConvertAll(Rage.World.GetEntities(Game.LocalPlayer.Character.Position, DistanceToScan, GetEntitiesFlags.ConsiderAllVehicles).Where(x => x is Vehicle && x.Exists()).ToArray(), (x => (Vehicle)x));//250
        foreach (Vehicle Veh in Vehicles)
        {
            AddToLists(Veh);
        }
    }
    public void UpdateVehiclePlates()
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
    private void AddToLists(Vehicle Veh)
    {
        if (Veh.IsPoliceVehicle)
        {
            if (!PoliceVehicles.Any(x => x.Vehicle.Handle == Veh.Handle))
            {
                Mod.World.PoliceSpawning.UpdateLivery(Veh);
                Mod.World.PoliceSpawning.UpgradeCruiser(Veh);
                VehicleExt PoliceCar = new VehicleExt(Veh);
                PoliceVehicles.Add(PoliceCar);
            }
        }
        else
        {
            if (!CivilianVehicles.Any(x => x.Vehicle.Handle == Veh.Handle))
            {
                VehicleExt CivilianCar = new VehicleExt(Veh);
                //if (!CivilianCar.Vehicle.HasDriver)
                //{
                //    CivilianCar.Vehicle.SetLock((VehicleLockStatus)7);
                //    if (!CivilianCar.Vehicle.IsEngineOn)
                //    {
                //        CivilianCar.Vehicle.MustBeHotwired = true;
                //    }
                //}
                CivilianVehicles.Add(CivilianCar);
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
}
