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

        private List<Blip> CreatedBlips = new List<Blip>();
        private Pedestrians Pedestrians;
        private IPlacesOfInterest PlacesOfInterest;
        private Vehicles Vehicles;
        private IZones Zones;
        private IJurisdictions Jurisdictions;
        public World(IAgencies agencies, IZones zones, IJurisdictions jurisdictions, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest, IPlateTypes plateTypes, INameProvideable names, IPedGroups relationshipGroups)
        {
            PlacesOfInterest = placesOfInterest;
            Zones = zones;
            Jurisdictions = jurisdictions;
            Pedestrians = new Pedestrians(agencies, zones, jurisdictions, settings, names, relationshipGroups);
            Vehicles = new Vehicles(agencies, zones, jurisdictions, settings, plateTypes);
        }
        public bool AnyArmyUnitsSpawned => Pedestrians.AnyArmyUnitsSpawned;
        public bool AnyCopsNearPlayer => Pedestrians.AnyCopsNearPlayer;
        public bool AnyHelicopterUnitsSpawned => Pedestrians.AnyHelicopterUnitsSpawned;
        public bool AnyNooseUnitsSpawned => Pedestrians.AnyNooseUnitsSpawned;
        public List<PedExt> CivilianList => Pedestrians.Civilians.Where(x => x.Pedestrian.Exists()).ToList();
        public int PoliceBoatsCount => Vehicles.PoliceBoatsCount;
        public int PoliceHelicoptersCount => Vehicles.PoliceHelicoptersCount;
        public List<Cop> PoliceList => Pedestrians.Police.Where(x => x.Pedestrian.Exists()).ToList();
        public int TotalSpawnedPolice => Pedestrians.TotalSpawnedPolice;
        public int TotalSpawnedFirefighters => Pedestrians.TotalSpawnedFirefighters;
        public int TotalSpawnedEMTs => Pedestrians.TotalSpawnedEMTs;
        public void Setup()
        {
            foreach (Zone zone in Zones.ZoneList)
            {
                zone.AssignedLEAgencyInitials = Jurisdictions.GetMainAgency(zone.InternalGameName,ResponseType.LawEnforcement)?.ColorInitials;
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
        public void AddEntity(PedExt pedExt)// => Pedestrians.Police.Add(cop);
        {
            if(pedExt.GetType() == typeof(Cop))
            {
                Pedestrians.Police.Add((Cop)pedExt);
            }
            else if (pedExt.GetType() == typeof(EMT))
            {
                Pedestrians.EMTs.Add((EMT)pedExt);
            }
            else if (pedExt.GetType() == typeof(Firefighter))
            {
                Pedestrians.Firefighters.Add((Firefighter)pedExt);
            }
        }
        public void AddEntity(VehicleExt vehicle) => Vehicles.AddToList(vehicle);
        public bool AnyCopsNearPosition(Vector3 position, float radius) => Pedestrians.AnyCopsNearPosition(position, radius);
        public void ClearSpawned()
        {
            Pedestrians.ClearSpawned();
            Vehicles.ClearSpawned();
        }
        public int CountNearbyPolice(Ped pedestrian) => Pedestrians.CountNearbyPolice(pedestrian);
        public void Dispose()
        {
            RemoveBlips();
            ClearSpawned();

        }
        public PedExt GetPedExt(uint handle) => Pedestrians.GetPedExt(handle);
        public VehicleExt GetVehicleExt(Vehicle vehicle) => Vehicles.GetVehicleExt(vehicle);
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