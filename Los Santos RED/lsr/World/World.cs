using ExtensionsMethods;
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
        private ISettingsProvideable Settings;
        private ICrimes Crimes;
        private IWeapons Weapons;     
        private ITimeReportable Time;
        private IInteriors Interiors;
        public World(IAgencies agencies, IZones zones, IJurisdictions jurisdictions, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest, IPlateTypes plateTypes, INameProvideable names, IPedGroups relationshipGroups, IWeapons weapons, ICrimes crimes, ITimeReportable time, IShopMenus shopMenus, IInteriors interiors, IAudioPlayable audio)
        {
            PlacesOfInterest = placesOfInterest;
            Zones = zones;
            Jurisdictions = jurisdictions;
            Settings = settings;
            Weapons = weapons;
            Crimes = crimes;
            Time = time;
            Interiors = interiors;
            Pedestrians = new Pedestrians(agencies, zones, jurisdictions, settings, names, relationshipGroups, weapons, crimes, shopMenus);
            Vehicles = new Vehicles(agencies, zones, jurisdictions, settings, plateTypes);
        }
        public List<GameLocation> ActiveLocations { get; private set; } = new List<GameLocation>();
        public bool AnyWantedCiviliansNearPlayer => CivilianList.Any(x => x.WantedLevel > 0 && x.DistanceToPlayer <= 150f);
        public bool AnyArmyUnitsSpawned => Pedestrians.AnyArmyUnitsSpawned;
        public bool AnyHelicopterUnitsSpawned => Pedestrians.AnyHelicopterUnitsSpawned;
        public bool AnyNooseUnitsSpawned => Pedestrians.AnyNooseUnitsSpawned;
        public List<PedExt> CivilianList => Pedestrians.Civilians.Where(x => x.Pedestrian.Exists()).ToList();
        public bool IsMPMapLoaded { get; private set; }
        public List<VehicleExt> CivilianVehicleList => Vehicles.CivilianVehicleList;
        public List<VehicleExt> PoliceVehicleList => Vehicles.PoliceVehicleList;
        public int PoliceBoatsCount => Vehicles.PoliceBoatsCount;
        public int PoliceHelicoptersCount => Vehicles.PoliceHelicoptersCount;
        public int PoliceVehicleCount => Vehicles.PoliceVehiclesCount;
        public int SpawnedPoliceVehicleCount => Vehicles.SpawnedPoliceVehiclesCount;
        public int CivilianVehicleCount => Vehicles.CivilianVehiclesCount;
        public List<Cop> PoliceList => Pedestrians.Police.Where(x => x.Pedestrian.Exists()).ToList();
        public List<EMT> EMTList => Pedestrians.EMTs.Where(x => x.Pedestrian.Exists()).ToList();
        public List<Firefighter> FirefighterList => Pedestrians.Firefighters.Where(x => x.Pedestrian.Exists()).ToList();
        public List<Merchant> MerchantList => Pedestrians.Merchants.Where(x => x.Pedestrian.Exists()).ToList();
        public int TotalSpawnedPolice => Pedestrians.TotalSpawnedPolice;
        public int TotalSpawnedFirefighters => Pedestrians.TotalSpawnedFirefighters;
        public int TotalSpawnedEMTs => Pedestrians.TotalSpawnedEMTs;
        public string DebugString => Pedestrians.DebugString + " - " + Vehicles.DebugString;
        public void Setup()
        {
            foreach (Zone zone in Zones.ZoneList)
            {
                zone.AssignedLEAgencyInitials = Jurisdictions.GetMainAgency(zone.InternalGameName,ResponseType.LawEnforcement)?.ColorInitials;

                Agency secondaryAgency = Jurisdictions.GetNthAgency(zone.InternalGameName, ResponseType.LawEnforcement, 2);
                if(secondaryAgency != null)
                {
                    zone.AssignedSecondLEAgencyInitials = secondaryAgency.ColorInitials;
                }
                else
                {
                    zone.AssignedSecondLEAgencyInitials = "";
                }
                GameFiber.Yield();
            }
        }
        public void AddBlipsToMap()
        {
            CreatedBlips = new List<Blip>();
            if (Settings.SettingsManager.WorldSettings.AddPOIBlipsToMap)
            {
                foreach (GameLocation MyLocation in PlacesOfInterest.GetAllPlaces())
                {
                    if (MyLocation.ShouldAlwaysHaveBlip)
                    {
                        MapBlip myBlip = new MapBlip(MyLocation.EntrancePosition, MyLocation.Name, MyLocation.Type);
                        AddEntity(myBlip.AddToMap());
                        GameFiber.Yield();
                    }
                }
            }
        }
        public void AddEntity(Blip myBlip) => CreatedBlips.Add(myBlip);
        public void AddEntity(PedExt pedExt)
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
            else if (pedExt.GetType() == typeof(Merchant))
            {
                Pedestrians.Merchants.Add((Merchant)pedExt);
            }
            else 
            {
                Pedestrians.Civilians.Add(pedExt);
            }
        }
        public void AddEntity(VehicleExt vehicle, ResponseType responseType) => Vehicles.AddToList(vehicle, responseType);
        public bool AnyCopsNearCop(Cop cop, int CellsAway) => Pedestrians.AnyCopsNearCop(cop, CellsAway);
        public bool AnyCopsNearPosition(Vector3 position, float radius) => Pedestrians.AnyCopsNearPosition(position, radius);
        public void ClearSpawned()
        {
            Pedestrians.ClearSpawned();
            Vehicles.ClearSpawned();
        }
        public void ClearSpawnedVehicles() => Vehicles.ClearSpawned();
        public void ClearSpawnedPedestrians() => Pedestrians.ClearSpawned();
        public void Dispose()
        {
            RemoveBlips();
            ClearSpawned();
            foreach(GameLocation loc in ActiveLocations)
            {
                if(loc.HasInterior)
                {
                    Interior myInt = Interiors.GetInterior(loc.InteriorID);
                    if (myInt != null)
                    {
                        myInt.Unload();
                    }
                }
            }
        }
        public PedExt GetPedExt(uint handle) => Pedestrians.GetPedExt(handle);
        public VehicleExt GetVehicleExt(Vehicle vehicle) => Vehicles.GetVehicleExt(vehicle);
        public VehicleExt GetVehicleExt(uint handle) => Vehicles.GetVehicleExt(handle);
        public VehicleExt GetClosestVehicleExt(Vector3 position, bool includePolice, float maxDistance) => Vehicles.GetClosestVehicleExt(position, includePolice, maxDistance);
        public void PrunePedestrians() => Pedestrians.Prune();
        public void PruneVehicles() => Vehicles.Prune();
        public void RemoveBlips()
        {
            foreach (Blip MyBlip in CreatedBlips)
            {
                if (MyBlip.Exists())
                {
                    MyBlip.Delete();
                }
            }
        }
        //public void ScanForPedestrians() => Pedestrians.Scan();
        public void CreateNewPedestrians() => Pedestrians.CreateNew();
        //public void ScanForVehicles() => Vehicles.Scan();
        public void CreateNewVehicles() => Vehicles.CreateNew();
        public void UpdateVehiclePlates()
        {
            if (Settings.SettingsManager.WorldSettings.UpdateVehiclePlates)
            {
                Vehicles.UpdatePlates();
            }
        }
        public void CleanUpVehicles()
        {
            if (Settings.SettingsManager.WorldSettings.CleanupVehicles)
            {
                Vehicles.CleanUp();
            }
        }
        public void LoadMPMap()
        {
            if (!IsMPMapLoaded)
            {
                NativeFunction.Natives.SET_INSTANCE_PRIORITY_MODE(1);
                NativeFunction.Natives.x0888C3502DBBEEF5();// ON_ENTER_MP();
                IsMPMapLoaded = true;
            }
        }
        public void LoadSPMap()
        {
            if (IsMPMapLoaded)
            {
                NativeFunction.Natives.SET_INSTANCE_PRIORITY_MODE(0);
                NativeFunction.Natives.xD7C10C4A637992C9();// ON_ENTER_SP();
                IsMPMapLoaded = false;
            }
        }
        public void ActiveNearLocations()
        {
            int LocationsCalculated = 0;
            foreach(GameLocation gl in PlacesOfInterest.GetAllPlaces())
            {
                if (gl.IsOpen(Time.CurrentHour) && gl.IsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, 4))// && NativeHelper.IsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, gl.CellX, gl.CellY, 4))// gl.DistanceToPlayer <= 200f)//gl.EntrancePosition.DistanceTo2D(Game.LocalPlayer.Character) <= 200f)
                {
                    if (!ActiveLocations.Contains(gl))
                    {
                        ActiveLocations.Add(gl);
                        SetupLocation(gl);
                        GameFiber.Yield();
                    }
                    else
                    {
                        gl.Update();
                    }
                }
                else
                {
                    if (ActiveLocations.Contains(gl))
                    {
                        ActiveLocations.Remove(gl);
                        RemoveLocation(gl);
                        GameFiber.Yield();
                    }
                }
                LocationsCalculated++;
                if(LocationsCalculated >= 20)//50//20//5
                {
                    LocationsCalculated = 0;
                    GameFiber.Yield();
                }
            }
        }
        public void ActivateLocation(GameLocation gl)
        {
            if (gl.IsOpen(Time.CurrentHour) && gl.IsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, 4))
            {
                if (!ActiveLocations.Contains(gl))
                {
                    ActiveLocations.Add(gl);
                    SetupLocation(gl);
                    GameFiber.Yield();
                }
            }
        }
        public void SetupLocation(GameLocation gameLocation)
        {
            if(gameLocation.HasInterior)
            {
                Interior myInt = Interiors.GetInterior(gameLocation.InteriorID);
                if (myInt != null)
                {
                    myInt.Load();
                }
            }
            if (gameLocation.HasVendor)
            {
                SpawnVendor(gameLocation);
                GameFiber.Yield();
            }
            if (!gameLocation.ShouldAlwaysHaveBlip)
            {
                SetupBlip(gameLocation);
                GameFiber.Yield();
            }
            gameLocation.SetNearby();
            gameLocation.Update();
            //GameFiber.Yield();
        }
        private void SpawnVendor(GameLocation gameLocation)
        {
            Ped ped;
            string ModelName = gameLocation.VendorModels.PickRandom();
            foreach (PedExt possibleCollision in CivilianList)
            {
                if (possibleCollision.Pedestrian.Exists() && possibleCollision.Pedestrian.DistanceTo2D(gameLocation.VendorPosition) <= 5f)
                {
                    possibleCollision.Pedestrian.Delete();
                }
            }

            if (RandomItems.RandomPercent(30))
            {
                //ped = new Ped(ModelName, new Vector3(gameLocation.VendorPosition.X, gameLocation.VendorPosition.Y, gameLocation.VendorPosition.Z), gameLocation.VendorHeading);
                Model modelToCreate = new Model(Game.GetHashKey(ModelName));
                modelToCreate.LoadAndWait();
                ped = NativeFunction.Natives.CREATE_PED<Ped>(26, Game.GetHashKey(ModelName), gameLocation.VendorPosition.X, gameLocation.VendorPosition.Y, gameLocation.VendorPosition.Z + 1f, gameLocation.VendorHeading, false, false);
                //EntryPoint.SpawnedEntities.Add(ped);
                //EntryPoint.WriteToConsole($"VENDOR: CREATED {ped.Handle}", 5);
            }
            else
            {
                //ped = new Ped(new Vector3(gameLocation.VendorPosition.X, gameLocation.VendorPosition.Y, gameLocation.VendorPosition.Z), gameLocation.VendorHeading);
                Model modelToCreate = new Model(Game.GetHashKey(ModelName));
                modelToCreate.LoadAndWait();
                ped = NativeFunction.Natives.CREATE_PED<Ped>(26, Game.GetHashKey(ModelName), gameLocation.VendorPosition.X, gameLocation.VendorPosition.Y, gameLocation.VendorPosition.Z + 1f, gameLocation.VendorHeading, false, false);

                // EntryPoint.WriteToConsole($"VENDOR: CREATED {ped.Handle}", 5);
            }

            GameFiber.Yield();
            if (ped.Exists())
            {
                ped.IsPersistent = false;
                ped.RandomizeVariation();
                ped.Tasks.StandStill(-1);
                ped.KeepTasks = true;
                EntryPoint.SpawnedEntities.Add(ped);
                GameFiber.Yield();
                Merchant Person = new Merchant(ped, Settings, false, false, false, "Vendor", new PedGroup("Vendor", gameLocation.Name, "Vendor", false), Crimes, Weapons);
                Person.Store = gameLocation;
                AddEntity(Person);
            }
        }
        private void SetupBlip(GameLocation gameLocation)
        {
            MapBlip myBlip = new MapBlip(gameLocation.EntrancePosition, gameLocation.Name, gameLocation.Type);
            Blip createdBlip = myBlip.AddToMap();
            gameLocation.SetCreatedBlip(createdBlip);
            AddEntity(createdBlip);
        }
        private void RemoveLocation(GameLocation gameLocation)
        {
            if (gameLocation.HasInterior)
            {
                Interior myInt = Interiors.GetInterior(gameLocation.InteriorID);
                if (myInt != null)
                {
                    myInt.Unload();
                }
            }
            if (!gameLocation.ShouldAlwaysHaveBlip)
            {
                RemoveBlip(gameLocation);
            }
        }
        private void RemoveBlip(GameLocation gameLocation)
        {
            if (gameLocation.CreatedBlip.Exists())
            {
                gameLocation.CreatedBlip.Delete();
            }
        }
    }
}