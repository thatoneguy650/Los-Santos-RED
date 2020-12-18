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
            return Mod.World.Vehicles.PoliceVehicles.Count(x => x.Vehicle.Exists() && x.Vehicle.IsHelicopter);
        }
    }
    public int PoliceBoatsCount
    {
        get
        {
            return Mod.World.Vehicles.PoliceVehicles.Count(x => x.Vehicle.Exists() && x.Vehicle.IsBoat);
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
    public void Tick()
    {
        RemoveAbandonedPoliceVehicles();
        FixDamagedPoliceVehicles();
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


    private void RemoveAbandonedPoliceVehicles()
    {
        foreach (VehicleExt PoliceCar in PoliceVehicles.Where(x => x.Vehicle.Exists() && x.WasModSpawned))//cleanup abandoned police cars, either cop dies or he gets marked non persisitent
        {
            if (PoliceCar.Vehicle.IsEmpty)
            {
                if (PoliceCar.Vehicle.DistanceTo2D(Game.LocalPlayer.Character) >= 250f)
                {
                    PoliceCar.Vehicle.Delete();
                }
            }
        }
    }
    private void FixDamagedPoliceVehicles()
    {
        foreach (Cop Cop in Mod.World.Pedestrians.Police.Where(x => x.Pedestrian.Exists() && x.DistanceToPlayer >= 100f && x.Pedestrian.IsInAnyVehicle(false)))//was 175f
        {
            if (Cop.Pedestrian.CurrentVehicle.Health < Cop.Pedestrian.CurrentVehicle.MaxHealth || Cop.Pedestrian.CurrentVehicle.EngineHealth < 1000f)
            {
                Cop.Pedestrian.CurrentVehicle.Repair();
            }
            else if (Cop.Pedestrian.CurrentVehicle.Health <= 600 || Cop.Pedestrian.CurrentVehicle.EngineHealth <= 600 || Cop.Pedestrian.CurrentVehicle.IsUpsideDown)
            {
                Mod.World.Spawner.Delete(Cop);
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
            
            if (Veh.IsPoliceVehicle)
            {
                if (!PoliceVehicles.Any(x => x.Vehicle.Handle == Veh.Handle))
                {
                    VehicleExt Car = new VehicleExt(Veh);
                    Car.UpdateCopCarLivery();
                    Car.UpgradeCopCarPerformance();
                    PoliceVehicles.Add(Car);
                }
            }
            else
            {
                if (!CivilianVehicles.Any(x => x.Vehicle.Handle == Veh.Handle))
                {
                    VehicleExt Car = new VehicleExt(Veh);
                    CivilianVehicles.Add(Car);
                }
            }
        }
    }
    public void AddToList(VehicleExt Car)
    {
        if (Car.Vehicle.Exists())
        {
            if (Car.Vehicle.IsPoliceVehicle)
            {
                if (!PoliceVehicles.Any(x => x.Vehicle.Handle == Car.Vehicle.Handle))
                {
                    Car.UpdateCopCarLivery();
                    Car.UpgradeCopCarPerformance();
                    PoliceVehicles.Add(Car);
                }
            }
            else
            {
                if (!CivilianVehicles.Any(x => x.Vehicle.Handle == Car.Vehicle.Handle))
                {
                    CivilianVehicles.Add(Car);
                }
            }
        }
    }
}
