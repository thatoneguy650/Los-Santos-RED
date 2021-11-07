using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;


public class Vehicles
{
    private readonly float DistanceToScan = 200f;//450f
    private readonly List<VehicleExt> PoliceVehicles = new List<VehicleExt>();
    private readonly List<VehicleExt> EMSVehicles = new List<VehicleExt>();
    private readonly List<VehicleExt> FireVehicles = new List<VehicleExt>();
    private readonly List<VehicleExt> CivilianVehicles = new List<VehicleExt>();
    private IZones Zones;
    private IAgencies Agencies;
    private IPlateTypes PlateTypes;
    private IJurisdictions Jurisdictions;
    private ISettingsProvideable Settings;
    private Entity[] RageVehicles;

    public Vehicles(IAgencies agencies,IZones zones, IJurisdictions jurisdictions, ISettingsProvideable settings, IPlateTypes plateTypes)
    {
        Zones = zones;
        Agencies = agencies;
        PlateTypes = plateTypes;
        Jurisdictions = jurisdictions;
        Settings = settings;
    }
    public List<VehicleExt> PoliceVehicleList => PoliceVehicles;
    public int PoliceVehiclesCount => PoliceVehicles.Count();
    public int CivilianVehiclesCount => CivilianVehicles.Count();
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
    public void Prune()
    {
        CivilianVehicles.RemoveAll(x => !x.Vehicle.Exists());
        PoliceVehicles.RemoveAll(x => !x.Vehicle.Exists());
        EMSVehicles.RemoveAll(x => !x.Vehicle.Exists());
        FireVehicles.RemoveAll(x => !x.Vehicle.Exists());
    }
    public void Scan()
    {
        RageVehicles = Rage.World.GetEntities(Game.LocalPlayer.Character.Position, DistanceToScan, GetEntitiesFlags.ConsiderAllVehicles);
    }
    public void CreateNew()
    {
        int VehiclesCreated = 0;
        foreach (Vehicle vehicle in RageVehicles.Where(x => x.Exists()))//take 20 is new
        {
            if(AddToList(vehicle))
            {
                VehiclesCreated++;
            }
            if (VehiclesCreated > 4)//10//2, at two it keeps missing vehicles im trying to enter, even 4 is too little?
            {
                VehiclesCreated = 0;
                GameFiber.Yield();
            }
        }
    }
    public void CleanUp()
    {
        RemoveAbandonedPoliceVehicles();
        FixDamagedPoliceVehicles();
    }
    public void UpdatePlates()
    {
        int VehiclesUpdated = 0;
        foreach (VehicleExt MyCar in CivilianVehicles.Where(x => x.Vehicle.Exists() && !x.HasUpdatedPlateType))
        {
            if (MyCar.Vehicle.Exists())
            {
                UpdatePlate(MyCar);
            }
            VehiclesUpdated++;
            if (VehiclesUpdated > 4)
            {
                VehiclesUpdated = 0;
                GameFiber.Yield();
            }
        }
    }
    private void RemoveAbandonedPoliceVehicles()
    {
        foreach (VehicleExt PoliceCar in PoliceVehicles.Where(x => x.Vehicle.Exists() && x.WasModSpawned && !x.WasSpawnedEmpty))
        {
            if (PoliceCar.Vehicle.Exists() && PoliceCar.Vehicle.IsEmpty)
            {
                if (PoliceCar.Vehicle.DistanceTo2D(Game.LocalPlayer.Character) >= 250f)
                {
                    PoliceCar.Vehicle.Delete();
                    GameFiber.Yield();
                }
            }
        }
    }
    private void FixDamagedPoliceVehicles()
    {
        foreach (VehicleExt PoliceCar in PoliceVehicles.Where(x => x.Vehicle.Exists() && x.WasModSpawned))
        {
            if (PoliceCar.Vehicle.Exists())
            {
                if ((PoliceCar.Vehicle.Health < PoliceCar.Vehicle.MaxHealth || PoliceCar.Vehicle.EngineHealth < 1000f) && PoliceCar.Vehicle.DistanceTo2D(Game.LocalPlayer.Character) >= 25f && !PoliceCar.Vehicle.IsOnScreen)
                {
                    PoliceCar.Vehicle.Repair();
                }
            }
        }
    }
    public void ClearSpawned()
    {
        foreach (VehicleExt vehicleExt in PoliceVehicles)
        {
            if (vehicleExt.Vehicle.Exists())
            {
                vehicleExt.Vehicle.Delete();
            }
        }
        foreach (VehicleExt vehicleExt in EMSVehicles)
        {
            if (vehicleExt.Vehicle.Exists())
            {
                vehicleExt.Vehicle.Delete();
            }
        }
        foreach (VehicleExt vehicleExt in FireVehicles)
        {
            if (vehicleExt.Vehicle.Exists())
            {
                vehicleExt.Vehicle.Delete();
            }
        }
        PoliceVehicles.Clear();
    }
    public VehicleExt GetVehicleExt(Vehicle vehicle)
    {
        VehicleExt ToReturn = PoliceVehicles.FirstOrDefault(x => x.Vehicle.Handle == vehicle.Handle);
        if(ToReturn == null)
        {
            ToReturn = CivilianVehicles.FirstOrDefault(x => x.Vehicle.Handle == vehicle.Handle);
        }
        return ToReturn;
    }
    public VehicleExt GetVehicleExt(uint handle)
    {
        VehicleExt ToReturn = PoliceVehicles.FirstOrDefault(x => x.Vehicle.Handle == handle);
        if (ToReturn == null)
        {
            ToReturn = CivilianVehicles.FirstOrDefault(x => x.Vehicle.Handle == handle);
        }
        return ToReturn;
    }
    public bool AddToList(Vehicle vehicle)
    {
        if (vehicle.Exists())
        {
            VehicleExt Car = new VehicleExt(vehicle, Settings);
            if (vehicle.IsPoliceVehicle)
            {
                if (!PoliceVehicles.Any(x => x.Handle == vehicle.Handle))
                {
                    PoliceVehicles.Add(Car);
                    return true;
                }
            }
            //else if (responseType == ResponseType.EMS)
            //{
            //    if (!EMSVehicles.Any(x => x.Handle == vehicle.Handle))
            //    {
            //        EMSVehicles.Add(Car);
            //        return true;
            //    }
            //}
            //else if (responseType == ResponseType.Fire)
            //{
            //    if (!FireVehicles.Any(x => x.Handle == vehicle.Handle))
            //    {
            //        FireVehicles.Add(Car);
            //        return true;
            //    }
            //}
            else
            {
                if (!CivilianVehicles.Any(x => x.Handle == vehicle.Handle))
                {
                    CivilianVehicles.Add(Car);
                    return true;
                }
            }
        }
        return false;
     }
    public void AddToList(VehicleExt vehicleExt, ResponseType responseType)
    {
        if (responseType == ResponseType.LawEnforcement || vehicleExt.Vehicle.IsPoliceVehicle)
        {
            if (!PoliceVehicles.Any(x => x.Handle == vehicleExt.Vehicle.Handle))
            {
                PoliceVehicles.Add(vehicleExt);
            }
        }
        else if (responseType == ResponseType.EMS)
        {
            if (!EMSVehicles.Any(x => x.Handle == vehicleExt.Vehicle.Handle))
            {
                EMSVehicles.Add(vehicleExt);
            }
        }
        else if (responseType == ResponseType.Fire)
        {
            if (!FireVehicles.Any(x => x.Handle == vehicleExt.Vehicle.Handle))
            {
                FireVehicles.Add(vehicleExt);
            }
        }
        else
        {
            if (!CivilianVehicles.Any(x => x.Handle == vehicleExt.Vehicle.Handle))
            {
                CivilianVehicles.Add(vehicleExt);
            }
        }
    }
    public void UpdatePlate(VehicleExt vehicleExt)//this might need to come out of here.... along with the two bools
    {
        if (vehicleExt.Vehicle.Exists())
        {
            vehicleExt.HasUpdatedPlateType = true;
            PlateType CurrentType = PlateTypes.GetPlateType(NativeFunction.CallByName<int>("GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", vehicleExt.Vehicle));
            string CurrentPlateNumber = vehicleExt.Vehicle.LicensePlate;
            Zone CurrentZone = Zones.GetZone(vehicleExt.Vehicle.Position);


            /*
             * 
             *TEMP HERE UNTIL I DECIDE
             * 
             * 
             * */
            if (CurrentZone != null && CurrentZone.State != "San Andreas")//change the plates based on state
            {
                PlateType NewType = PlateTypes.GetPlateType(CurrentZone.State);

                if (NewType != null)
                {
                    //EntryPoint.WriteToConsole($"Zone State: {CurrentZone.State} Plate State {NewType.State} Index {NewType.Index} Index+1 {NewType.Index + 1}");
                    string NewPlateNumber = NewType.GenerateNewLicensePlateNumber();
                    if (NewPlateNumber != "")
                    {
                        vehicleExt.Vehicle.LicensePlate = NewPlateNumber;
                        vehicleExt.OriginalLicensePlate.PlateNumber = NewPlateNumber;
                        vehicleExt.CarPlate.PlateNumber = NewPlateNumber;
                    }
                    if (NativeFunction.CallByName<int>("GET_NUMBER_OF_VEHICLE_NUMBER_PLATES") <= NewType.Index)
                    {
                        NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", vehicleExt.Vehicle, NewType.Index);
                        vehicleExt.OriginalLicensePlate.PlateType = NewType.Index;
                        vehicleExt.CarPlate.PlateType = NewType.Index;
                    }
                    // //EntryPoint.WriteToConsole("UpdatePlate", string.Format("Updated {0} {1}", Vehicle.Model.Name, NewType.Index));
                }
            }
            else
            {
                if (RandomItems.RandomPercent(15) && CurrentType != null && CurrentType.CanOverwrite && vehicleExt.CanUpdatePlate)
                {
                    PlateType NewType = PlateTypes.GetRandomPlateType();
                    if (NewType != null)
                    {
                        string NewPlateNumber = NewType.GenerateNewLicensePlateNumber();
                        if (NewPlateNumber != "")
                        {
                            vehicleExt.Vehicle.LicensePlate = NewPlateNumber;
                            vehicleExt.OriginalLicensePlate.PlateNumber = NewPlateNumber;
                            vehicleExt.CarPlate.PlateNumber = NewPlateNumber;
                        }
                        if (NativeFunction.CallByName<int>("GET_NUMBER_OF_VEHICLE_NUMBER_PLATES") <= NewType.Index)
                        {
                            NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", vehicleExt.Vehicle, NewType.Index + 1);
                            vehicleExt.OriginalLicensePlate.PlateType = NewType.Index;
                            vehicleExt.CarPlate.PlateType = NewType.Index;
                        }
                        // //EntryPoint.WriteToConsole("UpdatePlate", string.Format("Updated {0} {1}", Vehicle.Model.Name, NewType.Index));
                    }
                }
            }
        }

    }
    public Agency GetAgency(Vehicle vehicle, int wantedLevel, ResponseType responseType)
    {
        Agency ToReturn;
        List<Agency> ModelMatchAgencies = Agencies.GetAgencies(vehicle);
        if (ModelMatchAgencies.Count > 1)
        {
            Zone ZoneFound = Zones.GetZone(vehicle.Position);
            if (ZoneFound != null && ZoneFound.InternalGameName != "UNK")
            {
                List<Agency> ToGoThru = Jurisdictions.GetAgencies(ZoneFound.InternalGameName, wantedLevel, responseType);
                if (ToGoThru != null)
                {
                    foreach (Agency ZoneAgency in ToGoThru)
                    {
                        if (ModelMatchAgencies.Any(x => x.ID == ZoneAgency.ID))
                        {
                            return ZoneAgency;
                        }
                    }
                }
            }
        }
        ToReturn = ModelMatchAgencies.FirstOrDefault();
        if (ToReturn == null)
        {
            vehicle.Delete();
        }
        return ToReturn;
    }
}
