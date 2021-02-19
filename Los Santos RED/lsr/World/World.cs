using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Util.Locations;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace Mod
{
    public class World : IEntityLoggable, IEntityProvideable
    {

        private List<Blip> CreatedBlips;
        private Pedestrians Pedestrians;
        private IPlacesOfInterest PlacesOfInterest;
        private Vehicles Vehicles;
        private IZones Zones;
        private IZoneJurisdictions ZoneJurisdictions;
        public World(IAgencies agencies, IZones zones, IZoneJurisdictions zoneJurisdictions, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest, IPlateTypes plateTypes, INameProvideable names, IPedGroups relationshipGroups)
        {
            PlacesOfInterest = placesOfInterest;
            Zones = zones;
            ZoneJurisdictions = zoneJurisdictions;
            Pedestrians = new Pedestrians(agencies, zones, zoneJurisdictions, settings, names, relationshipGroups);
            Vehicles = new Vehicles(agencies, zones, zoneJurisdictions, settings, plateTypes);
        }
        public bool AnyArmyUnitsSpawned => Pedestrians.AnyArmyUnitsSpawned;
        public bool AnyCopsNearPlayer => Pedestrians.AnyCopsNearPlayer;
        public bool AnyHelicopterUnitsSpawned => Pedestrians.AnyHelicopterUnitsSpawned;
        public bool AnyNooseUnitsSpawned => Pedestrians.AnyNooseUnitsSpawned;
        public List<PedExt> CivilianList => Pedestrians.Civilians.Where(x => x.Pedestrian.Exists()).ToList();
        public int PoliceBoatsCount => Vehicles.PoliceBoatsCount;
        public int PoliceHelicoptersCount => Vehicles.PoliceHelicoptersCount;
        public List<Cop> PoliceList => Pedestrians.Police.Where(x => x.Pedestrian.Exists()).ToList();
        public int TotalSpawnedCops => Pedestrians.TotalSpawnedCops;
        public void Setup()
        {
            foreach (Zone zone in Zones.ZoneList)
            {
                zone.AssignedAgencyInitials = ZoneJurisdictions.GetMainAgency(zone.InternalGameName)?.ColoredInitials;
            }
        }
        public void AddBlipsToMap()
        {
            CreatedBlips = new List<Blip>();
            foreach (GameLocation MyLocation in PlacesOfInterest.GetAllPlaces())
            {
                MapBlip myBlip = new MapBlip(MyLocation.LocationPosition, MyLocation.Name, MyLocation.Type);
                myBlip.AddToMap();
            }
        }
        public void AddEntity(Blip myBlip) => CreatedBlips.Add(myBlip);
        public void AddEntity(Cop cop) => Pedestrians.Police.Add(cop);
        public void AddEntity(VehicleExt vehicle) => Vehicles.AddToList(vehicle);
        public bool AnyCopsNearPosition(Vector3 position, float radius) => Pedestrians.AnyCopsNearPosition(position, radius);
        public void ClearPolice()
        {
            Pedestrians.ClearPolice();
            Vehicles.ClearPolice();
        }
        public int CountNearbyCops(Ped pedestrian) => Pedestrians.CountNearbyCops(pedestrian);
        public void Dispose()
        {
            RemoveBlips();
            ClearPolice();

        }
        public PedExt GetCivilian(uint handle) => Pedestrians.GetCivilian(handle);
        public VehicleExt GetVehicle(Vehicle vehicleTryingToEnter) => Vehicles.GetVehicle(vehicleTryingToEnter);
        public void PrunePedestrians() => Pedestrians.Prune();
        public void PruneVehicles() => Vehicles.Prune();
        public void RemoveBlips()
        {
            foreach (Blip MyBlip in CreatedBlips)
            {
                if (MyBlip.Exists())
                    MyBlip.Delete();
            }
        }
        public void ScanForPedestrians() => Pedestrians.Scan();
        public void CreateNewPedestrians() => Pedestrians.CreateNew();
        public void ScanForVehicles() => Vehicles.Scan();

        public void CreateNewVehicles() => Vehicles.CreateNew();

        public void UpdateVehiclePlates() => Vehicles.UpdatePlates();

        public void CleanUpVehicles() => Vehicles.CleanUp();
    }
}