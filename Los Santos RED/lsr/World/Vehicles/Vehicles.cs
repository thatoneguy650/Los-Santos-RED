using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using static DispatchScannerFiles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


public class Vehicles
{
    private IZones Zones;
    private IAgencies Agencies;
    private IPlateTypes PlateTypes;
    private IJurisdictions Jurisdictions;
    private ISettingsProvideable Settings;
    private IModItems ModItems;
    private Entity[] RageVehicles;
    private IEntityProvideable World;
    private uint GameTimeLastCreatedVehicles;
    private bool TaxiModelIsDefault = false;
    private bool HasCheckedTaxiModel;
    private TaxiFirm DefaultTaxiFirm;
    private IOrganizations Organizations;

    public Vehicles(IAgencies agencies,IZones zones, IJurisdictions jurisdictions, ISettingsProvideable settings, IPlateTypes plateTypes, IModItems modItems, IEntityProvideable world, IOrganizations organizations)
    {
        Zones = zones;
        Agencies = agencies;
        PlateTypes = plateTypes;
        Jurisdictions = jurisdictions;
        Settings = settings;
        ModItems = modItems;
        World = world;
        Organizations = organizations;
        PlateController = new PlateController(this, Zones, PlateTypes, Settings);
    }
    public PlateController PlateController { get; private set; }
    public List<PoliceVehicleExt> PoliceVehicles { get; private set; } = new List<PoliceVehicleExt>();
    public List<EMSVehicleExt> EMSVehicles { get; private set; } = new List<EMSVehicleExt>();
    public List<FireVehicleExt> FireVehicles { get; private set; } = new List<FireVehicleExt>();
    public List<GangVehicleExt> GangVehicles { get; private set; } = new List<GangVehicleExt>();
    public List<VehicleExt> CivilianVehicles { get; private set; } = new List<VehicleExt>();
    public List<SecurityVehicleExt> SecurityVehicles { get; private set; } = new List<SecurityVehicleExt>();
    public List<TaxiVehicleExt> TaxiVehicles { get; private set; } = new List<TaxiVehicleExt>();
    public List<VehicleExt> AllVehicleList 
    {
        get
        {
            List<VehicleExt> myList = new List<VehicleExt>();
            myList.AddRange(PoliceVehicles);
            myList.AddRange(CivilianVehicles);
            myList.AddRange(FireVehicles);
            myList.AddRange(EMSVehicles);
            myList.AddRange(GangVehicles);
            myList.AddRange(SecurityVehicles);
            myList.AddRange(TaxiVehicles);
            return myList;
        }
    }
    public List<VehicleExt> SimplePoliceVehicles
    {
        get
        {
            List<VehicleExt> myList = new List<VehicleExt>();
            myList.AddRange(PoliceVehicles);
            return myList;
        }
    }
    public List<VehicleExt> SimpleGangVehicles
    {
        get
        {
            List<VehicleExt> myList = new List<VehicleExt>();
            myList.AddRange(GangVehicles);
            return myList;
        }
    }
    public List<VehicleExt> ServiceVehicles
    {
        get
        {
            List<VehicleExt> myList = new List<VehicleExt>();
            myList.AddRange(PoliceVehicles);
            myList.AddRange(FireVehicles);
            myList.AddRange(EMSVehicles);
            myList.AddRange(SecurityVehicles);
            return myList;
        }
    }
    public List<VehicleExt> NonServiceVehicles
    {
        get
        {
            List<VehicleExt> myList = new List<VehicleExt>();
            myList.AddRange(CivilianVehicles);
            myList.AddRange(GangVehicles);
            myList.AddRange(TaxiVehicles);
            return myList;
        }
    }
    public int SpawnedPoliceVehiclesCount => PoliceVehicles.Where(x=> x.WasModSpawned).Count();
    public int SpawnedAmbientPoliceVehiclesCount => PoliceVehicles.Where(x => x.WasModSpawned && !x.WasSpawnedEmpty).Count();
    public int PoliceHelicoptersCount => PoliceVehicles.Count(x => x.Vehicle.Exists() && x.Vehicle.IsHelicopter);
    public int PoliceBoatsCount => PoliceVehicles.Count(x => x.Vehicle.Exists() && x.Vehicle.IsBoat);
    public int GangHelicoptersCount => GangVehicles.Count(x => x.Vehicle.Exists() && x.Vehicle.IsHelicopter);
    public int GangBoatsCount => GangVehicles.Count(x => x.Vehicle.Exists() && x.Vehicle.IsBoat);
    public void Setup()
    {
        DefaultTaxiFirm = Organizations.GetDefaultTaxiFirm();
    }
    public void Dispose()
    {
        ClearSpawned(true);
    }
    public void Prune()
    {
        CivilianVehicles.RemoveAll(x => !x.Vehicle.Exists());
        GameFiber.Yield();//TR 29
        PoliceVehicles.RemoveAll(x => !x.Vehicle.Exists());
        GameFiber.Yield();//TR 29
        EMSVehicles.RemoveAll(x => !x.Vehicle.Exists());
        GameFiber.Yield();//TR 29
        FireVehicles.RemoveAll(x => !x.Vehicle.Exists());
        GameFiber.Yield();//TR 29
        GangVehicles.RemoveAll(x => !x.Vehicle.Exists());
        GameFiber.Yield();//TR 29
        SecurityVehicles.RemoveAll(x => !x.Vehicle.Exists());
        GameFiber.Yield();//TR 29
        TaxiVehicles.RemoveAll(x => !x.Vehicle.Exists());
    }
    public void CreateNew()
    {
        RageVehicles = Rage.World.GetEntities(GetEntitiesFlags.ConsiderAllVehicles);
        GameFiber.Yield();
        int updated = 0;
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
            updated++;
            if(updated > 10)
            {
                GameFiber.Yield();
                updated = 0;
            }
            //GameFiber.Yield();//TR 29
        }
        if (Settings.SettingsManager.PerformanceSettings.PrintUpdateTimes)
        {
            EntryPoint.WriteToConsole($"Vehicles.CreateNew Ran Time Since {Game.GameTime - GameTimeLastCreatedVehicles}", 5);
        }
        GameTimeLastCreatedVehicles = Game.GameTime;
    }
    public bool AddEntity(Vehicle vehicle)
    {
        if(!vehicle.Exists())
        {
            return false;
        }
        if (vehicle.IsPoliceVehicle)
        {
            if (!PoliceVehicles.Any(x => x.Handle == vehicle.Handle))
            {
                CreatePoliceFromAmbient(vehicle);
                return true;
            }
        }
        else
        {
            if (!CivilianVehicles.Any(x => x.Handle == vehicle.Handle))
            {
                CreateCivilianVehicleFromAmbient(vehicle);
                return true;
            }
        }    
        return false;
    }


    private void CreatePoliceFromAmbient(Vehicle vehicle)
    {
        PoliceVehicleExt Car = new PoliceVehicleExt(vehicle, Settings);
        Car.Setup();
        Car.IsPolice = true;
        Car.CanRandomlyHaveIllegalItems = false;
        PoliceVehicles.Add(Car);
    }

    private void CreateCivilianVehicleFromAmbient(Vehicle vehicle)
    {
        if (vehicle.Model.Hash == 3338918751)//.Name.ToLower() == "taxi")
        {
            CreateTaxiVehicleFromAmbient(vehicle);
        }
        else
        {
            CreateRegularCivilianVehicle(vehicle);
        }
    }
    private void CreateTaxiVehicleFromAmbient(Vehicle vehicle)
    {
        if(!HasCheckedTaxiModel)
        {
            CheckVanillaTaxi(vehicle);
        }
        TaxiFirm taxiFirm = DefaultTaxiFirm;
        if (!TaxiModelIsDefault)
        {
            taxiFirm = GetSpecificTaxiFirm(vehicle);
            if (taxiFirm == null)
            {
                taxiFirm = DefaultTaxiFirm;
            }
        }
        TaxiVehicleExt Taxi = new TaxiVehicleExt(vehicle, Settings);
        Taxi.TaxiFirm = taxiFirm;
        Taxi.Setup();
        Taxi.AddVehicleToList(World);

    }
    private TaxiFirm GetSpecificTaxiFirm(Vehicle vehicle)
    {
        if (!vehicle.Exists())
        {
            return null;
        }
        int liveryID = NativeFunction.Natives.GET_VEHICLE_LIVERY<int>(vehicle);
        if (liveryID != -1)
        {
            return Organizations.GetTaxiFirmFromVehicle("taxi", liveryID);
        }
        return null;
    }
    private void CheckVanillaTaxi(Vehicle vehicle)
    {
        if(!vehicle.Exists())
        {
            return;
        }
        int TotalLiveries = NativeFunction.Natives.GET_VEHICLE_LIVERY_COUNT<int>(vehicle);
        TaxiModelIsDefault = TotalLiveries == -1;
        HasCheckedTaxiModel = true;
    }

    private void CreateRegularCivilianVehicle(Vehicle vehicle)
    {
        VehicleExt Car = new VehicleExt(vehicle, Settings);
        Car.Setup();
        Car.AddVehicleToList(World);
    }
    public void AddPolice(PoliceVehicleExt vehicleExt)
    {
        if (vehicleExt == null || !vehicleExt.Vehicle.Exists())
        {
            return;
        }
        if (!PoliceVehicles.Any(x => x.Handle == vehicleExt.Vehicle.Handle))
        {
            PoliceVehicles.Add(vehicleExt);
            CivilianVehicles.RemoveAll(x => x.Handle == vehicleExt.Handle);
        }
        
    }
    public void AddEMS(EMSVehicleExt vehicleExt)
    {
        if (vehicleExt == null || !vehicleExt.Vehicle.Exists())
        {
            return;
        }
        if (!EMSVehicles.Any(x => x.Handle == vehicleExt.Vehicle.Handle))
        {
            EMSVehicles.Add(vehicleExt);
            CivilianVehicles.RemoveAll(x => x.Handle == vehicleExt.Handle);
        }
    }
    public void AddFire(FireVehicleExt vehicleExt)
    {
        if (vehicleExt == null || !vehicleExt.Vehicle.Exists())
        {
            return;
        }
        if (!FireVehicles.Any(x => x.Handle == vehicleExt.Vehicle.Handle))
        {
            FireVehicles.Add(vehicleExt);
            CivilianVehicles.RemoveAll(x => x.Handle == vehicleExt.Handle);
        }
    }
    public void AddGang(GangVehicleExt vehicleExt)
    {
        if (vehicleExt == null || !vehicleExt.Vehicle.Exists())
        {
            return;
        }
        if (!GangVehicles.Any(x => x.Handle == vehicleExt.Vehicle.Handle))
        {
            GangVehicles.Add(vehicleExt);
            CivilianVehicles.RemoveAll(x => x.Handle == vehicleExt.Handle);
        }
    }
    public void AddCivilian(VehicleExt vehicleExt)
    {
        if (vehicleExt == null || !vehicleExt.Vehicle.Exists())
        {
            return;
        }
        if (!CivilianVehicles.Any(x => x.Handle == vehicleExt.Vehicle.Handle))
        {
            CivilianVehicles.Add(vehicleExt);
        }
    }
    public void AddSecurity(SecurityVehicleExt vehicleExt)
    {
        if (vehicleExt == null || !vehicleExt.Vehicle.Exists())
        {
            return;
        }
        if (!SecurityVehicles.Any(x => x.Handle == vehicleExt.Vehicle.Handle))
        {
            SecurityVehicles.Add(vehicleExt);
            CivilianVehicles.RemoveAll(x => x.Handle == vehicleExt.Handle);
        }
    }
    public PoliceVehicleExt GetPolice(Vehicle vehicle)
    {
        PoliceVehicleExt ToReturn = null;
        if (vehicle.Exists())
        {
            ToReturn = PoliceVehicles.FirstOrDefault(x => x.Vehicle.Exists() && x.Vehicle.Handle == vehicle.Handle);
        }
        return ToReturn;
    }
    public EMSVehicleExt GetEMS(Vehicle vehicle)
    {
        EMSVehicleExt ToReturn = null;
        if (vehicle.Exists())
        {
            ToReturn = EMSVehicles.FirstOrDefault(x => x.Vehicle.Exists() && x.Vehicle.Handle == vehicle.Handle);
        }
        return ToReturn;
    }
    public FireVehicleExt GetFire(Vehicle vehicle)
    {
        FireVehicleExt ToReturn = null;
        if (vehicle.Exists())
        {
            ToReturn = FireVehicles.FirstOrDefault(x => x.Vehicle.Exists() && x.Vehicle.Handle == vehicle.Handle);
        }
        return ToReturn;
    }
    public GangVehicleExt GetGang(Vehicle vehicle)
    {
        GangVehicleExt ToReturn = null;
        if (vehicle.Exists())
        {
            ToReturn = GangVehicles.FirstOrDefault(x => x.Vehicle.Exists() && x.Vehicle.Handle == vehicle.Handle);
        }
        return ToReturn;
    }
    public SecurityVehicleExt GetSecurity(Vehicle vehicle)
    {
        SecurityVehicleExt ToReturn = null;
        if (vehicle.Exists())
        {
            ToReturn = SecurityVehicles.FirstOrDefault(x => x.Vehicle.Exists() && x.Vehicle.Handle == vehicle.Handle);
        }
        return ToReturn;
    }
    public void ClearPolice()
    {
        foreach (VehicleExt vehicleExt in PoliceVehicles)
        {
            vehicleExt.FullyDelete();
            if (vehicleExt.Vehicle.Exists())
            {
                EntryPoint.PersistentVehiclesDeleted++;
            }
        }
        PoliceVehicles.Clear();
    }
    public void ClearSpawned(bool includeCivilian)
    {
        ClearPolice();
        foreach (VehicleExt vehicleExt in EMSVehicles)
        {
            vehicleExt.FullyDelete();
            if (vehicleExt.Vehicle.Exists())
            {
                EntryPoint.PersistentVehiclesDeleted++;
            }
        }
        EMSVehicles.Clear();
        foreach (VehicleExt vehicleExt in FireVehicles)
        {
            vehicleExt.FullyDelete();
            if (vehicleExt.Vehicle.Exists())
            {
                EntryPoint.PersistentVehiclesDeleted++;
            }
        }
        FireVehicles.Clear();
        foreach (VehicleExt vehicleExt in SecurityVehicles)
        {
            vehicleExt.FullyDelete();
            if (vehicleExt.Vehicle.Exists())
            {
                EntryPoint.PersistentVehiclesDeleted++;
            }
        }
        SecurityVehicles.Clear();
        if (includeCivilian)
        {
            foreach (GangVehicleExt vehicleExt in GangVehicles.Where(x => x.WasModSpawned))
            {
                vehicleExt.FullyDelete();
                if (vehicleExt.Vehicle.Exists())
                {
                    EntryPoint.PersistentVehiclesDeleted++;
                }
            }
            GangVehicles.Clear();
            foreach (VehicleExt vehicleExt in CivilianVehicles.Where(x => x.WasModSpawned))
            {
                vehicleExt.FullyDelete();
                if (vehicleExt.Vehicle.Exists())
                {
                    EntryPoint.PersistentVehiclesDeleted++;
                }
            }
            CivilianVehicles.Clear();
            foreach (VehicleExt vehicleExt in TaxiVehicles.Where(x => x.WasModSpawned))
            {
                vehicleExt.FullyDelete();
                if (vehicleExt.Vehicle.Exists())
                {
                    EntryPoint.PersistentVehiclesDeleted++;
                }
            }
            TaxiVehicles.Clear();
        }
    }
    public VehicleExt GetClosestVehicleExt(Vector3 position, bool includeService, float maxDistance)
    {
        if(position == Vector3.Zero)
        {
            return null;
        }
        VehicleExt civilianCar = NonServiceVehicles.Where(x => x.Vehicle.Exists()).OrderBy(x => x.Vehicle.DistanceTo2D(position)).FirstOrDefault();
        float civilianDistance = 999f;
        float serviceDistance = 999f;
        if (civilianCar != null && civilianCar.Vehicle.Exists())
        {
            civilianDistance = civilianCar.Vehicle.DistanceTo2D(position);
        }
        if (includeService)
        {
            VehicleExt serviceVehicle = ServiceVehicles.Where(x => x.Vehicle.Exists()).OrderBy(x => x.Vehicle.DistanceTo2D(position)).FirstOrDefault();
            if(serviceVehicle != null && serviceVehicle.Vehicle.Exists())
            {
                serviceDistance = serviceVehicle.Vehicle.DistanceTo2D(position);
            }
            if (serviceDistance < civilianDistance)
            {
                if(serviceDistance <= maxDistance)
                {
                    return serviceVehicle;
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
        VehicleExt ToReturn = null;
        if (vehicle.Exists())
        {
            ToReturn = ServiceVehicles.FirstOrDefault(x => x.Vehicle.Exists() && x.Vehicle.Handle == vehicle.Handle);
            if (ToReturn == null)
            {
                ToReturn = NonServiceVehicles.FirstOrDefault(x => x.Vehicle.Exists() && x.Vehicle.Handle == vehicle.Handle);
            }
        }
        return ToReturn;
    }
    public VehicleExt GetVehicleExt(uint handle)
    {
        VehicleExt ToReturn = ServiceVehicles.FirstOrDefault(x => x.Vehicle.Exists() && x.Vehicle.Handle == handle);
        if (ToReturn == null)
        {
            ToReturn = NonServiceVehicles.FirstOrDefault(x => x.Vehicle.Exists() && x.Vehicle.Handle == handle);
        }
        return ToReturn;
    }
    public void UpdatePoliceSonarBlips(bool setBlipped)
    {
        foreach(VehicleExt copCar in PoliceVehicles)
        {
            if (!copCar.Vehicle.Exists())
            {
                continue;
            }
            if (setBlipped)
            {
                copCar.SonarBlip.Update(World);
            }
            else
            {
                copCar.SonarBlip.Dispose();
            }
        }
    }

    public void AddTaxi(TaxiVehicleExt vehicleExt)
    {
        if (vehicleExt == null || !vehicleExt.Vehicle.Exists())
        {
            return;
        }
        if (!TaxiVehicles.Any(x => x.Handle == vehicleExt.Vehicle.Handle))
        {
            TaxiVehicles.Add(vehicleExt);
            CivilianVehicles.RemoveAll(x => x.Handle == vehicleExt.Handle);
        }
    }
}
