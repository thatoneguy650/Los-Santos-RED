using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;


public class Vehicles
{
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
    private uint GameTimeLastCreatedVehicles;
    public Vehicles(IAgencies agencies,IZones zones, IJurisdictions jurisdictions, ISettingsProvideable settings, IPlateTypes plateTypes)
    {
        Zones = zones;
        Agencies = agencies;
        PlateTypes = plateTypes;
        Jurisdictions = jurisdictions;
        Settings = settings;
        PlateController = new PlateController(this, Zones, PlateTypes, Settings);
    }
    public PlateController PlateController { get; private set; }
    public List<VehicleExt> PoliceVehicleList => PoliceVehicles;
    public List<VehicleExt> CivilianVehicleList => CivilianVehicles;
    public List<VehicleExt> FireVehicleList => FireVehicles;
    public List<VehicleExt> EMSVehicleList => EMSVehicles;
    public int SpawnedPoliceVehiclesCount => PoliceVehicles.Where(x=> x.WasModSpawned).Count();
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
    public void Setup()
    {

    }
    public void Dispose()
    {
        ClearSpawned();
    }
    public void Prune()
    {
        CivilianVehicles.RemoveAll(x => !x.Vehicle.Exists());
        PoliceVehicles.RemoveAll(x => !x.Vehicle.Exists());
        EMSVehicles.RemoveAll(x => !x.Vehicle.Exists());
        FireVehicles.RemoveAll(x => !x.Vehicle.Exists());
    }
    public void CreateNew()
    {
        RageVehicles = Rage.World.GetEntities(GetEntitiesFlags.ConsiderAllVehicles);
        GameFiber.Yield();
        foreach (Vehicle vehicle in RageVehicles.Where(x => x.Exists()))//take 20 is new
        {
            if (Settings.SettingsManager.VehicleSettings.UseBetterLightStateOnAI)//move into a controller proc?
            {
                NativeFunction.Natives.SET_VEHICLE_USE_PLAYER_LIGHT_SETTINGS(vehicle, true);
            }

            if (AddEntity(vehicle))
            {   
                GameFiber.Yield();
            }
        }
        if (Settings.SettingsManager.DebugSettings.PrintUpdateTimes)
        {
            EntryPoint.WriteToConsole($"Vehicles.CreateNew Ran Time Since {Game.GameTime - GameTimeLastCreatedVehicles}", 5);
        }
        GameTimeLastCreatedVehicles = Game.GameTime;
    }
    public void CleanUp()
    {
        if (Settings.SettingsManager.WorldSettings.CleanupVehicles)
        {
            RemoveAbandonedPoliceVehicles();
            FixDamagedPoliceVehicles();
        }
    }
    private void RemoveAbandonedPoliceVehicles()
    {
        try
        {
            foreach (VehicleExt PoliceCar in PoliceVehicles.Where(x => x.Vehicle.Exists() && !x.WasSpawnedEmpty && x.HasExistedFor >= 15000).ToList())
            {
                if (PoliceCar.Vehicle.Exists())
                {
                    if (!PoliceCar.Vehicle.Occupants.Any(x => x.Exists() && x.IsAlive))
                    {
                        if (PoliceCar.Vehicle.DistanceTo2D(Game.LocalPlayer.Character) >= 250f)
                        {
                            if (PoliceCar.Vehicle.IsPersistent)
                            {
                                EntryPoint.PersistentVehiclesDeleted++;
                            }
                            PoliceCar.Vehicle.Delete();
                            GameFiber.Yield();
                        }
                        GameFiber.Yield();
                    }  
                }
            }
            foreach (VehicleExt PoliceCar in CivilianVehicles.Where(x => x.WasModSpawned && !x.WasSpawnedEmpty && x.Vehicle.Exists() && x.Vehicle.IsPersistent && x.HasExistedFor >= 15000).ToList())
            {
                if (PoliceCar.Vehicle.Exists())
                {
                    if (!PoliceCar.Vehicle.Occupants.Any(x => x.Exists() && x.IsAlive))
                    {
                        if (PoliceCar.Vehicle.DistanceTo2D(Game.LocalPlayer.Character) >= 250f)
                        {
                            if (PoliceCar.Vehicle.IsPersistent)
                            {
                                EntryPoint.PersistentVehiclesDeleted++;
                            }
                            EntryPoint.WriteToConsole($"RemoveAbandonedGangVehicles {PoliceCar.Vehicle.Handle}", 5);
                            PoliceCar.Vehicle.Delete();
                            GameFiber.Yield();
                        }
                        GameFiber.Yield();
                    }
                }
            }
        }
        catch(InvalidOperationException ex)
        {
            EntryPoint.WriteToConsole($"Remove Abandoned Vehicles, Collection Modified Error: {ex.Message} {ex.StackTrace}", 0);
        }
    }
    private void FixDamagedPoliceVehicles()
    {
        foreach (VehicleExt PoliceCar in PoliceVehicles.Where(x => x.Vehicle.Exists() && x.WasModSpawned && x.HasExistedFor >= 15000).ToList())
        {
            if (PoliceCar.Vehicle.Exists())
            {
                if (PoliceCar.Vehicle.HasOccupants && (PoliceCar.Vehicle.Health < PoliceCar.Vehicle.MaxHealth - 500 || PoliceCar.Vehicle.EngineHealth < 200f) && PoliceCar.Vehicle.DistanceTo2D(Game.LocalPlayer.Character) >= 25f && !PoliceCar.Vehicle.IsOnScreen)
                {
                    PoliceCar.Vehicle.Repair();
                    GameFiber.Yield();
                }
            }
        }
    }
    public bool AddEntity(Vehicle vehicle)
    {
        if (vehicle.Exists())
        {

            if (vehicle.IsPoliceVehicle)
            {
                if (!PoliceVehicles.Any(x => x.Handle == vehicle.Handle))
                {
                    VehicleExt Car = new VehicleExt(vehicle, Settings);
                    Car.IsPolice = true;
                    PoliceVehicles.Add(Car);
                    return true;
                }
            }
            else
            {
                if (!CivilianVehicles.Any(x => x.Handle == vehicle.Handle))
                {
                    VehicleExt Car = new VehicleExt(vehicle, Settings);
                    CivilianVehicles.Add(Car);
                    return true;
                }
            }
        }
        return false;
    }
    public void AddEntity(VehicleExt vehicleExt, ResponseType responseType)
    {
        if (vehicleExt != null && vehicleExt.Vehicle.Exists())
        {
            if (responseType == ResponseType.LawEnforcement || vehicleExt.Vehicle.IsPoliceVehicle)
            {
                if (!PoliceVehicles.Any(x => x.Handle == vehicleExt.Vehicle.Handle))
                {
                    vehicleExt.IsPolice = true;
                    PoliceVehicles.Add(vehicleExt);

                }
            }
            else if (responseType == ResponseType.EMS)
            {
                if (!EMSVehicles.Any(x => x.Handle == vehicleExt.Vehicle.Handle))
                {
                    vehicleExt.IsEMT = true;
                    EMSVehicles.Add(vehicleExt);
                }
            }
            else if (responseType == ResponseType.Fire)
            {
                if (!FireVehicles.Any(x => x.Handle == vehicleExt.Vehicle.Handle))
                {
                    vehicleExt.IsFire = true;
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
    }
    public void ClearSpawned()
    {
        foreach (VehicleExt vehicleExt in PoliceVehicles)
        {
            if (vehicleExt.Vehicle.Exists())
            {
                vehicleExt.Vehicle.Delete();
                EntryPoint.PersistentVehiclesDeleted++;
            }
        }
        PoliceVehicles.Clear();
        foreach (VehicleExt vehicleExt in EMSVehicles)
        {
            if (vehicleExt.Vehicle.Exists())
            {
                vehicleExt.Vehicle.Delete();
                EntryPoint.PersistentVehiclesDeleted++;
            }
        }
        EMSVehicles.Clear();
        foreach (VehicleExt vehicleExt in FireVehicles)
        {
            if (vehicleExt.Vehicle.Exists())
            {
                vehicleExt.Vehicle.Delete();
                EntryPoint.PersistentVehiclesDeleted++;
            }
        }
        FireVehicles.Clear();
        foreach (VehicleExt vehicleExt in CivilianVehicles.Where(x=> x.WasModSpawned))
        {
            if (vehicleExt.Vehicle.Exists())
            {
                vehicleExt.Vehicle.Delete();
                EntryPoint.PersistentVehiclesDeleted++;
            }
        }
    }
    public VehicleExt GetClosestVehicleExt(Vector3 position, bool includePolice, float maxDistance)
    {
        if(position == Vector3.Zero)
        {
            return null;
        }
        VehicleExt civilianCar = CivilianVehicles.Where(x => x.Vehicle.Exists()).OrderBy(x => x.Vehicle.DistanceTo2D(position)).FirstOrDefault();
        float civilianDistance = 999f;
        float policeDistance = 999f;
        if (civilianCar != null && civilianCar.Vehicle.Exists())
        {
            civilianDistance = civilianCar.Vehicle.DistanceTo2D(position);
        }
        if (includePolice)
        {
            VehicleExt policeCar = PoliceVehicles.Where(x => x.Vehicle.Exists()).OrderBy(x => x.Vehicle.DistanceTo2D(position)).FirstOrDefault();
            if(policeCar != null && policeCar.Vehicle.Exists())
            {
                policeDistance = policeCar.Vehicle.DistanceTo2D(position);
            }
            if (policeDistance < civilianDistance)
            {
                if(policeDistance <= maxDistance)
                {
                    return policeCar;
                }
                else
                {
                    return null;
                }
            }
        }
        if (civilianDistance <= maxDistance)
        {
            return civilianCar;
        }
        else
        {
            return null;
        }
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
            if (vehicle.IsPersistent)
            {
                EntryPoint.PersistentVehiclesDeleted++;
            }
            vehicle.Delete();
        }
        return ToReturn;
    }
}
