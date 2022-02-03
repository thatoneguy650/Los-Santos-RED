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
        private List<string> VendingMachines = new List<string>();
        private List<uint> VendingMachinesHash;
        private IShopMenus ShopMenus;
        private IGangTerritories GangTerritories;
        private IGangs Gangs;
        private IStreets Streets;
        public World(IAgencies agencies, IZones zones, IJurisdictions jurisdictions, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest, IPlateTypes plateTypes, INameProvideable names, IPedGroups relationshipGroups, IWeapons weapons, ICrimes crimes, ITimeReportable time, IShopMenus shopMenus, IInteriors interiors, IAudioPlayable audio, IGangs gangs, IGangTerritories gangTerritories, IStreets streets)
        {
            PlacesOfInterest = placesOfInterest;
            Zones = zones;
            Jurisdictions = jurisdictions;
            Settings = settings;
            Weapons = weapons;
            Crimes = crimes;
            Time = time;
            Interiors = interiors;
            ShopMenus = shopMenus;
            Gangs = gangs;
            GangTerritories = gangTerritories;
            Streets = streets;
            Pedestrians = new Pedestrians(agencies, zones, jurisdictions, settings, names, relationshipGroups, weapons, crimes, shopMenus, Gangs);
            Vehicles = new Vehicles(agencies, zones, jurisdictions, settings, plateTypes);
        }
        public List<GameLocation> ActiveLocations { get; private set; } = new List<GameLocation>();

        public List<InteractableLocation> ActiveInteractableLocations { get; private set; } = new List<InteractableLocation>();

        public bool AnyWantedCiviliansNearPlayer => CivilianList.Any(x => x.WantedLevel > 0 && x.DistanceToPlayer <= 150f);
        public bool AnyArmyUnitsSpawned => Pedestrians.AnyArmyUnitsSpawned;
        public bool AnyHelicopterUnitsSpawned => Pedestrians.AnyHelicopterUnitsSpawned;
        public bool AnyNooseUnitsSpawned => Pedestrians.AnyNooseUnitsSpawned;

        //public List<PedExt> TaskableCiviliansList
        //{
        //    get
        //    {
        //        List<PedExt> pedExts = new List<PedExt>();
        //        pedExts.AddRange(CivilianList);
        //        pedExts.AddRange(MerchantList);
        //        pedExts.AddRange(GangMemberList);
        //        return pedExts;
        //    }
        //}

        public List<PedExt> CivilianList => Pedestrians.Civilians.Where(x => x.Pedestrian.Exists()).ToList();
        public bool IsMPMapLoaded { get; private set; }
        public bool IsZombieApocalypse { get; set; } = false;
        public List<VehicleExt> CivilianVehicleList => Vehicles.CivilianVehicleList;
        public List<VehicleExt> PoliceVehicleList => Vehicles.PoliceVehicleList;
        public int PoliceBoatsCount => Vehicles.PoliceBoatsCount;
        public int PoliceHelicoptersCount => Vehicles.PoliceHelicoptersCount;
        public int PoliceVehicleCount => Vehicles.PoliceVehiclesCount;
        public int SpawnedPoliceVehicleCount => Vehicles.SpawnedPoliceVehiclesCount;
        public int CivilianVehicleCount => Vehicles.CivilianVehiclesCount;
        public List<GangMember> GangMemberList => Pedestrians.GangMembers.Where(x => x.Pedestrian.Exists()).ToList();
        public List<Zombie> ZombieList => Pedestrians.Zombies.Where(x => x.Pedestrian.Exists()).ToList();
        public List<Cop> PoliceList => Pedestrians.Police.Where(x => x.Pedestrian.Exists()).ToList();
        public List<EMT> EMTList => Pedestrians.EMTs.Where(x => x.Pedestrian.Exists()).ToList();
        public List<Firefighter> FirefighterList => Pedestrians.Firefighters.Where(x => x.Pedestrian.Exists()).ToList();
        public List<Merchant> MerchantList => Pedestrians.Merchants.Where(x => x.Pedestrian.Exists()).ToList();
        public int TotalSpawnedPolice => Pedestrians.TotalSpawnedPolice;
        public int TotalSpawnedFirefighters => Pedestrians.TotalSpawnedFirefighters;
        public int TotalSpawnedZombies => Pedestrians.TotalSpawnedZombies;
        public int TotalSpawnedEMTs => Pedestrians.TotalSpawnedEMTs;
        public int TotalSpawnedGangMembers => Pedestrians.TotalSpawnedGangMembers;
        public int TotalWantedLevel { get; set; }
        public string DebugString => Pedestrians.DebugString + " - " + Vehicles.DebugString;
        public void Setup()
        {
            foreach (Zone zone in Zones.ZoneList)
            {
                zone.AssignedLEAgencyInitials = Jurisdictions.GetMainAgency(zone.InternalGameName,ResponseType.LawEnforcement)?.ColorInitials;
                Gang mainGang = GangTerritories.GetMainGang(zone.InternalGameName);
                if(mainGang != null)
                {
                    zone.AssignedGangInitials = mainGang.ColorInitials;
                }
                else
                {
                    zone.AssignedGangInitials = "";
                }
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

            VendingMachines = new List<string>()
            { "prop_vend_soda_01","prop_vend_soda_02","prop_vend_coffe_01","prop_vend_condom_01","prop_vend_fags_01","prop_vend_snak_01","prop_vend_water_01"};

            VendingMachinesHash = new List<uint>()
            {0x3b21c5e7,0x426a547c,0x418f055a};

            Pedestrians.Setup();


            foreach(BasicLocation basicLocation in PlacesOfInterest.GetAllInteractableLocations())
            {
                Zone placeZone = Zones.GetZone(basicLocation.EntrancePosition);
                string betweener = "";
                string zoneString = "";
                if (placeZone != null)
                {
                    if (placeZone.IsSpecificLocation)
                    {
                        betweener = $"near";
                    }
                    else
                    {
                        betweener = $"in";
                    }
                    zoneString = $"~p~{placeZone.DisplayName}~s~";
                }
                string streetName = Streets.GetStreetNames(basicLocation.EntrancePosition);
                if (streetName == "")
                {
                    betweener = "";
                }
                string LocationName = $"{streetName} {betweener} {zoneString}".Trim();
                basicLocation.StreetAddress = LocationName;
            }

        }
        public void AddBlipsToMap()
        {
            CreatedBlips = new List<Blip>();
            if (Settings.SettingsManager.WorldSettings.AddPOIBlipsToMap)
            {
                foreach (GameLocation MyLocation in PlacesOfInterest.GetAllPlaces())
                {
                    if (MyLocation.ShouldAlwaysHaveBlip && MyLocation.IsBlipEnabled)
                    {
                        MapBlip myBlip = new MapBlip(MyLocation.EntrancePosition, MyLocation.Name, MyLocation.BlipSprite);
                        AddEntity(myBlip.AddToMap());
                        GameFiber.Yield();
                    }
                }
            }
        }
        public void AddEntity(Blip myBlip)
        {
            if (myBlip.Exists())
            {
                CreatedBlips.Add(myBlip);
            }
        }
        public void AddEntity(PedExt pedExt)
        {
            if (pedExt != null)
            {
                if (pedExt.GetType() == typeof(Cop))
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
                else if (pedExt.GetType() == typeof(Zombie))
                {
                    Pedestrians.Zombies.Add((Zombie)pedExt);
                }
                else if (pedExt.GetType() == typeof(GangMember))
                {
                    Pedestrians.GangMembers.Add((GangMember)pedExt);
                }
                else
                {
                    Pedestrians.Civilians.Add(pedExt);
                }
            }
        }
        public void AddEntity(VehicleExt vehicle, ResponseType responseType)
        {
            if (vehicle != null)
            {
                Vehicles.AddToList(vehicle, responseType);
            }
        }
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
                loc.Dispose();
            }
            foreach (InteractableLocation loc in ActiveInteractableLocations)
            {
                loc.Dispose();
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
        public void CreateNewPedestrians() => Pedestrians.CreateNew();
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
                Game.FadeScreenOut(1500, true);
                NativeFunction.Natives.SET_INSTANCE_PRIORITY_MODE(1);
                NativeFunction.Natives.x0888C3502DBBEEF5();// ON_ENTER_MP();
                Game.FadeScreenIn(1500, true);
                IsMPMapLoaded = true;
            }
        }
        public void LoadSPMap()
        {
            if (IsMPMapLoaded)
            {
                Game.FadeScreenOut(1500, true);
                NativeFunction.Natives.SET_INSTANCE_PRIORITY_MODE(0);
                NativeFunction.Natives.xD7C10C4A637992C9();// ON_ENTER_SP();
                Game.FadeScreenIn(1500, true);
                IsMPMapLoaded = false;
            }
        }
        public void ActiveNearLocations()
        {
            int LocationsCalculated = 0;
            foreach(GameLocation gl in PlacesOfInterest.GetAllPlaces())
            {
                if (gl.IsEnabled && gl.IsOpen(Time.CurrentHour) && gl.IsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, 3))// && NativeHelper.IsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, gl.CellX, gl.CellY, 4))// gl.DistanceToPlayer <= 200f)//gl.EntrancePosition.DistanceTo2D(Game.LocalPlayer.Character) <= 200f)
                {
                    if (!ActiveLocations.Contains(gl))
                    {
                        ActiveLocations.Add(gl);
                        gl.Setup(Interiors,Settings,Crimes,Weapons);
                        AddEntity(gl.Merchant);
                        AddEntity(gl.Blip);
                        GameFiber.Yield();
                    }
                }
                else
                {
                    if (ActiveLocations.Contains(gl))
                    {
                        ActiveLocations.Remove(gl);
                        gl.Dispose();
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




            LocationsCalculated = 0;
            foreach (InteractableLocation gl in PlacesOfInterest.GetAllInteractableLocations())
            {
                if (gl.IsEnabled && gl.IsOpen(Time.CurrentHour) && gl.CheckIsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, 3))// && NativeHelper.IsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, gl.CellX, gl.CellY, 4))// gl.DistanceToPlayer <= 200f)//gl.EntrancePosition.DistanceTo2D(Game.LocalPlayer.Character) <= 200f)
                {
                    if (!ActiveInteractableLocations.Contains(gl))
                    {
                        ActiveInteractableLocations.Add(gl);
                        gl.Setup(Interiors, Settings, Crimes, Weapons);
                        AddEntity(gl.Blip);
                        GameFiber.Yield();
                    }
                }
                else
                {
                    if (ActiveInteractableLocations.Contains(gl))
                    {
                        ActiveInteractableLocations.Remove(gl);
                        gl.Dispose();
                        GameFiber.Yield();
                    }
                }
                LocationsCalculated++;
                if (LocationsCalculated >= 20)//50//20//5
                {
                    LocationsCalculated = 0;
                    GameFiber.Yield();
                }
            }





            GameFiber.Yield();
            UpdateVendingMachines();
        }
        public void UpdateNearLocations()
        {
            foreach (GameLocation gl in ActiveLocations)
            {
                gl.Update();
                GameFiber.Yield();
            }
            foreach (InteractableLocation gl in ActiveInteractableLocations)
            {
                gl.Update();
                GameFiber.Yield();
            }
        }
        private void UpdateVendingMachines()
        {
            List<Rage.Object> Objects = Rage.World.GetAllObjects().ToList();
            foreach (Rage.Object obj in Objects)
            {
                if (obj.Exists())
                {
                    string modelName = obj.Model.Name.ToLower();
                    Vector3 position = obj.Position;
                    float heading = obj.Heading;
                    uint hash = obj.Model.Hash;
                    if (VendingMachines.Contains(modelName) || VendingMachinesHash.Contains(hash))
                    {
                        float distanceTo = obj.DistanceTo(Game.LocalPlayer.Character.Position);
                        if(distanceTo <= 50f)
                        {
                            if(!ActiveLocations.Any(x=> x.Type == LocationType.VendingMachine && x.EntrancePosition.DistanceTo2D(obj.Position) <= 0.2f))
                            {
                               ShopMenu toBuy = ShopMenus.GetVendingMenu(modelName);
                                GameLocation newVend = new GameLocation(position, heading, LocationType.VendingMachine, toBuy.Name, toBuy.Name, obj) { OpenTime = 0, CloseTime = 24, Menu = toBuy.Items, BannerImage = toBuy.BannerOverride };
                                newVend.Setup(Interiors, Settings, Crimes, Weapons);
                                AddEntity(newVend.Blip);
                                ActiveLocations.Add(newVend);
                                EntryPoint.WriteToConsole($"Nearby Vending {toBuy.Name} ADDED Props FOUND {modelName}", 5);
                            }
                        }
                        GameFiber.Yield();
                    }
                }
            }
            GameFiber.Yield();
            for (int i = ActiveLocations.Count - 1; i >= 0; i--)
            {
                GameLocation gl = ActiveLocations[i];
                if (gl.Type == LocationType.VendingMachine && gl.DistanceToPlayer >= 100f)// && NativeHelper.IsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, gl.CellX, gl.CellY, 4))// gl.DistanceToPlayer <= 200f)//gl.EntrancePosition.DistanceTo2D(Game.LocalPlayer.Character) <= 200f)
                {
                    if (ActiveLocations.Contains(gl))
                    {
                        EntryPoint.WriteToConsole($"Nearby Vending {gl.Name} REMOVED", 5);
                        ActiveLocations.Remove(gl);
                        gl.Dispose();
                        GameFiber.Yield();

                    }
                }
            }
        }
        public void ActivateLocation(GameLocation gl)
        {
            if (gl.IsEnabled && gl.IsOpen(Time.CurrentHour) && gl.IsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, 4))
            {
                if (!ActiveLocations.Contains(gl))
                {
                    ActiveLocations.Add(gl);
                    gl.Setup(Interiors, Settings, Crimes, Weapons);
                    AddEntity(gl.Merchant);
                    AddEntity(gl.Blip);
                    GameFiber.Yield();
                }
            }
        }

        public void SetLocationsActive(string iD, bool v)
        {
            foreach (GameLocation gl in PlacesOfInterest.GetAllPlaces().Where(x=> x.GangID == iD))
            {
                gl.IsEnabled = v;
            }
        }
    }
}