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

namespace Mod
{
    public class World : IWorld , IWorldLogger
    {
        private List<Blip> CreatedBlips;
        private Pedestrians Pedestrians;
        private Time Time;
        private Vehicles Vehicles;
        private IAgencies Agencies;
        private IZoneJurisdictions ZoneJurisdictions;
        private ISettings Settings;
        private IWeapons Weapons;
        private IPlacesOfInterest PlacesOfInterest;
        private IZones Zones;
        private IPlateTypes PlateTypes;

        public World(IAgencies agencies, IZoneJurisdictions zoneJurisdictions, ISettings settings, IWeapons weapons, IPlacesOfInterest placesOfInterest, IZones zones, IPlateTypes plateTypes)
        {
            Agencies = agencies;
            ZoneJurisdictions = zoneJurisdictions;
            Settings = settings;
            Weapons = weapons;
            PlacesOfInterest = placesOfInterest;
            Zones = zones;
            PlateTypes = plateTypes;
            Time = new Time();
            Pedestrians = new Pedestrians(this, Agencies, ZoneJurisdictions, Weapons, Settings);
            Vehicles = new Vehicles(Zones, Agencies, PlateTypes, ZoneJurisdictions);
        }

        public IPlayer CurrentPlayer { get; private set; }
        public bool AnyArmyUnitsSpawned => Pedestrians.AnyArmyUnitsSpawned;
        public bool AnyCopsNearPlayer => Pedestrians.AnyCopsNearPlayer;
        public bool AnyHelicopterUnitsSpawned => Pedestrians.AnyHelicopterUnitsSpawned;
        public bool AnyNooseUnitsSpawned => Pedestrians.AnyNooseUnitsSpawned;
        public List<PedExt> CivilianList => Pedestrians.Civilians.Where(x => x.Pedestrian.Exists()).ToList();
        public string CurrentTime => Time.CurrentTime;
        public bool IsNight => Time.IsNight;
        public int PoliceBoatsCount => Vehicles.PoliceBoatsCount;
        public int PoliceHelicoptersCount => Vehicles.PoliceHelicoptersCount;
        public List<Cop> PoliceList => Pedestrians.Police.Where(x => x.Pedestrian.Exists()).ToList();
        public bool ShouldBustPlayer => Pedestrians.ShouldBustPlayer;
        public int TotalSpawnedCops => Pedestrians.TotalSpawnedCops;
        public void AddBlip(Blip myBlip)
        {
            CreatedBlips.Add(myBlip);
        }
        public void AddCop(Cop myNewCop)
        {
            Pedestrians.Police.Add(myNewCop);
        }
        public void AddToList(VehicleExt toReturn)
        {
            Vehicles.AddToList(toReturn);
        }
        public bool AnyCopsNearPosition(Vector3 position, float radius)
        {
            return Pedestrians.AnyCopsNearPosition(position, radius);
        }
        public void ClearPolice()
        {
            Pedestrians.ClearPolice();
            Vehicles.ClearPolice();
        }
        public int CountNearbyCops(Ped pedestrian)
        {
            return Pedestrians.CountNearbyCops(pedestrian);
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
        public void Dispose()
        {
            Time.Dispose();
            RemoveBlips();
            ClearPolice();
        }
        public PedExt GetCivilian(uint handle)
        {
            return Pedestrians.GetCivilian(handle);
        }
        public VehicleExt GetVehicle(Vehicle vehicleTryingToEnter)
        {
            return Vehicles.GetVehicle(vehicleTryingToEnter);
        }
        public void PauseTime()
        {
            Time.PauseTime();
        }
        public void PrunePedestrians()
        {
            Pedestrians.Prune();
        }
        public void PruneVehicles()
        {
            Vehicles.CleanLists();
        }
        public void RemoveBlips()
        {
            foreach (Blip MyBlip in CreatedBlips)
            {
                if (MyBlip.Exists())
                    MyBlip.Delete();
            }
        }
        public void ScaneForPedestrians()
        {
            Pedestrians.Scan();
        }
        public void ScanForVehicles()
        {
            Vehicles.Scan();
        }
        public void UnPauseTime()
        {
            Time.UnpauseTime();
        }
        public void UpdateTime()
        {
            Time.Tick();
        }
        public void UpdateVehiclePlates()
        {
            Vehicles.UpdatePlates();
        }
        public void VehiclesTick()
        {
            Vehicles.Tick();
        }
       
    }
}