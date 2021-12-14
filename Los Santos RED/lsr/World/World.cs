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
        private List<GameLocation> ActiveLocations = new List<GameLocation>();
        private ITimeReportable Time;
        private List<IPLLocation> IPLLocations = new List<IPLLocation>();
        public World(IAgencies agencies, IZones zones, IJurisdictions jurisdictions, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest, IPlateTypes plateTypes, INameProvideable names, IPedGroups relationshipGroups, IWeapons weapons, ICrimes crimes, ITimeReportable time, IShopMenus shopMenus)
        {
            PlacesOfInterest = placesOfInterest;
            Zones = zones;
            Jurisdictions = jurisdictions;
            Settings = settings;
            Weapons = weapons;
            Crimes = crimes;
            Time = time;
            Pedestrians = new Pedestrians(agencies, zones, jurisdictions, settings, names, relationshipGroups, weapons, crimes, shopMenus);
            Vehicles = new Vehicles(agencies, zones, jurisdictions, settings, plateTypes);
        }
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


            SetupIPLs();

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
            UnloadIPLs();
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
        public void ScanForPedestrians() => Pedestrians.Scan();
        public void CreateNewPedestrians() => Pedestrians.CreateNew();
        public void ScanForVehicles() => Vehicles.Scan();
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
            foreach(GameLocation gl in PlacesOfInterest.GetAllPlaces())
            {
                //gl.Update();
                if (gl.IsOpen(Time.CurrentHour) && NativeHelper.IsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, gl.CellX, gl.CellY, 4))// gl.DistanceToPlayer <= 200f)//gl.EntrancePosition.DistanceTo2D(Game.LocalPlayer.Character) <= 200f)
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
            }

            foreach(IPLLocation iPLLocation in IPLLocations)
            {
                if(NativeHelper.IsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, iPLLocation.CellX, iPLLocation.CellY, 4))
                {
                    if(!iPLLocation.IsActive)
                    {
                        iPLLocation.Load();
                        EntryPoint.WriteToConsole($"World: Loaded {iPLLocation.ID}", 5);
                    }
                }
                else
                {
                    if (iPLLocation.IsActive)
                    {
                        iPLLocation.Unload();
                        EntryPoint.WriteToConsole($"World: UnLoaded {iPLLocation.ID}", 5);
                    }
                }
            }

        }
        public void SetupLocation(GameLocation gameLocation)
        {
            if (gameLocation.HasVendor)
            {
                SpawnVendor(gameLocation);
            }
            if (!gameLocation.ShouldAlwaysHaveBlip)
            {
                SetupBlip(gameLocation);
            }
            gameLocation.Update();
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
        private void SpawnVendor(GameLocation gameLocation)
        {
            Ped ped;
            string ModelName = gameLocation.VendorModels.PickRandom();
            foreach(PedExt possibleCollision in CivilianList)
            {
                if(possibleCollision.Pedestrian.Exists() && possibleCollision.Pedestrian.DistanceTo2D(gameLocation.VendorPosition) <= 5f)
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
        private void SetupIPLs()
        {

            IPLLocations = new List<IPLLocation>() {
                new IPLLocation("premiumdeluxemotorsport",7170,new List<string>() { "shr_int" },new List<string>() { "fakeint" },new List<string>() { "shutter_open","csr_beforeMission" },new Vector3(-38.83289f, -1108.61f, 26.46652f)),
            };


            ////FIB Lobby
            //NativeFunction.Natives.REQUEST_IPL("FIBlobby");
            //NativeFunction.Natives.REMOVE_IPL("FIBlobbyfake");
            //NativeFunction.Natives.x9B12F9A24FABEDB0(-1517873911, 106.3793f, -742.6982f, 46.51962f, false, 0.0f, 0.0f, 0.0f);
            //NativeFunction.Natives.x9B12F9A24FABEDB0(-90456267, 105.7607f, -746.646f, 46.18266f, false, 0.0f, 0.0f, 0.0f);

            ////Paleto Sheriff
            //NativeFunction.Natives.DISABLE_INTERIOR(NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(-444.89068603515625f, 6013.5869140625f, 30.7164f), false);
            //NativeFunction.Natives.CAP_INTERIOR(NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(-444.89068603515625f, 6013.5869140625f, 30.7164f), false);
            //NativeFunction.Natives.REQUEST_IPL("v_sheriff2");
            //NativeFunction.Natives.REMOVE_IPL("cs1_16_sheriff_cap");
            //NativeFunction.Natives.x9B12F9A24FABEDB0(-1501157055, -444.4985f, 6017.06f, 31.86633f, false, 0.0f, 0.0f, 0.0f);
            //NativeFunction.Natives.x9B12F9A24FABEDB0(-1501157055, -442.66f, 6015.222f, 31.86633f, false, 0.0f, 0.0f, 0.0f);


            ////Simeon
            //NativeFunction.Natives.REQUEST_IPL("shr_int");
            //NativeFunction.Natives.REMOVE_IPL("fakeint");
            //NativeFunction.Natives.REMOVE_IPL("shutter_closed");



            //NativeFunction.Natives.ACTIVATE_INTERIOR_ENTITY_SET(7170, "csr_beforeMission");
            //NativeFunction.Natives.ACTIVATE_INTERIOR_ENTITY_SET(7170, "shutter_open");
            //NativeFunction.Natives.REFRESH_INTERIOR(7170);



        }
        private void UnloadIPLs()
        {
            //NativeFunction.Natives.REMOVE_IPL("shr_int");
            //NativeFunction.Natives.REQUEST_IPL("fakeint");
            //NativeFunction.Natives.REQUEST_IPL("shutter_closed");
            //NativeFunction.Natives.DEACTIVATE_INTERIOR_ENTITY_SET(7170, "csr_afterMissionB");
            //NativeFunction.Natives.DEACTIVATE_INTERIOR_ENTITY_SET(7170, "shutter_closed");
            //NativeFunction.Natives.REFRESH_INTERIOR(7170);
        }
    }
}