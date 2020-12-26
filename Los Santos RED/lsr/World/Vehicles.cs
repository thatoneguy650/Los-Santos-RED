using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
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
    private IDataMart DataMart;

    public Vehicles(IDataMart dataMart)
    {
        DataMart = dataMart;
    }

    public int PoliceHelicoptersCount
    {
        get
        {
            return PoliceVehicles.Count(x => x.Vehicle.Exists() && x.Vehicle.IsHelicopter);
        }
    }
    public int PoliceBoatsCount
    {
        get
        {
            return PoliceVehicles.Count(x => x.Vehicle.Exists() && x.Vehicle.IsBoat);
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
            UpdatePlate(MyCar);
            //MyCar.UpdatePlate();
            VehiclesUpdated++;
            if (VehiclesUpdated > 5)
            {
                break;
            }
        }
    }


    private void RemoveAbandonedPoliceVehicles()
    {
        foreach (VehicleExt PoliceCar in PoliceVehicles.Where(x => x.Vehicle.Exists() && x.WasModSpawned))
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
        foreach (VehicleExt PoliceCar in PoliceVehicles.Where(x => x.Vehicle.Exists() && x.WasModSpawned))
        {
            if ((PoliceCar.Vehicle.Health < PoliceCar.Vehicle.MaxHealth || PoliceCar.Vehicle.EngineHealth < 1000f) && PoliceCar.Vehicle.DistanceTo2D(Game.LocalPlayer.Character) >= 75f)
            {
                PoliceCar.Vehicle.Repair();
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
                    SetVehicleAgency(Car);
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
                    SetVehicleAgency(Car);
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

    public void SetVehicleAgency(VehicleExt Car)
    {
        Agency AssignedAgency = DataMart.Agencies.GetAgency(Car.Vehicle, 0);//might need to real wanted level here
        Car.UpdateCopCarLivery(AssignedAgency);
    }
    public void UpdatePlate(VehicleExt Car)//this might need to come out of here.... along with the two bools
    {
        Car.HasUpdatedPlateType = true;
        PlateType CurrentType = DataMart.PlateTypes.GetPlateType(NativeFunction.CallByName<int>("GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", Car.Vehicle));
        string CurrentPlateNumber = Car.Vehicle.LicensePlate;
        Zone CurrentZone = DataMart.Zones.GetZone(Car.Vehicle.Position);


        /*
         * 
         *TEMP HERE UNTIL I DECIDE
         * 
         * 
         * */
        if (CurrentZone != null && CurrentZone.State != "San Andreas")//change the plates based on state
        {
            PlateType NewType = DataMart.PlateTypes.GetPlateType(CurrentZone.State);
            if (NewType != null)
            {
                string NewPlateNumber = NewType.GenerateNewLicensePlateNumber();
                if (NewPlateNumber != "")
                {
                    Car.Vehicle.LicensePlate = NewPlateNumber;
                    Car.OriginalLicensePlate.PlateNumber = NewPlateNumber;
                    Car.CarPlate.PlateNumber = NewPlateNumber;
                }
                NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", Car.Vehicle, NewType.Index);
                Car.OriginalLicensePlate.PlateType = NewType.Index;
                Car.CarPlate.PlateType = NewType.Index;
                // Game.Console.Print("UpdatePlate", string.Format("Updated {0} {1}", Vehicle.Model.Name, NewType.Index));
            }
        }
        else
        {
            if (RandomItems.RandomPercent(10) && CurrentType != null && CurrentType.CanOverwrite && Car.CanUpdatePlate)
            {
                PlateType NewType = DataMart.PlateTypes.GetRandomPlateType();
                if (NewType != null)
                {
                    string NewPlateNumber = NewType.GenerateNewLicensePlateNumber();
                    if (NewPlateNumber != "")
                    {
                        Car.Vehicle.LicensePlate = NewPlateNumber;
                        Car.OriginalLicensePlate.PlateNumber = NewPlateNumber;
                        Car.CarPlate.PlateNumber = NewPlateNumber;
                    }
                    NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", Car.Vehicle, NewType.Index);
                    Car.OriginalLicensePlate.PlateType = NewType.Index;
                    Car.CarPlate.PlateType = NewType.Index;
                    // Game.Console.Print("UpdatePlate", string.Format("Updated {0} {1}", Vehicle.Model.Name, NewType.Index));
                }
            }
        }

    }


}
