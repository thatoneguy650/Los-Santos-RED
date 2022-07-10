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
        private IZones Zones;
        private IJurisdictions Jurisdictions;
        private ISettingsProvideable Settings;
        private ICrimes Crimes;
        private IWeapons Weapons;     
        private ITimeReportable Time;
        private IInteriors Interiors;
        private IShopMenus ShopMenus;
        private IGangTerritories GangTerritories;
        private IGangs Gangs;
        private IStreets Streets;
        private IPlacesOfInterest PlacesOfInterest;
        private List<Blip> CreatedBlips = new List<Blip>();
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
            Pedestrians = new Pedestrians(agencies, zones, jurisdictions, settings, names, relationshipGroups, weapons, crimes, shopMenus, Gangs, GangTerritories);
            Vehicles = new Vehicles(agencies, zones, jurisdictions, settings, plateTypes);
            Places = new Places(this,zones,jurisdictions,settings,placesOfInterest,weapons,crimes,time,shopMenus,interiors,gangs,gangTerritories,streets, agencies);
        }
        public bool IsMPMapLoaded { get; private set; }
        public bool IsZombieApocalypse { get; set; } = false;
        public Vehicles Vehicles { get; private set; }
        public Pedestrians Pedestrians { get; private set; }
        public Places Places { get; private set; }
        public int TotalWantedLevel { get; set; }
        public string DebugString => "";
        public void Setup()
        {
            Pedestrians.Setup();
            Places.Setup();
            Vehicles.Setup();
            AddBlipsToMap();
        }
        public void Dispose()
        {
            Places.Dispose();
            Pedestrians.Dispose();
            Vehicles.Dispose();
            RemoveBlips();
        }
        public void ClearSpawned()
        {
            Pedestrians.ClearSpawned();
            Vehicles.ClearSpawned();
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
        public void AddBlip(Blip myBlip)
        {
            if (myBlip.Exists())
            {
                CreatedBlips.Add(myBlip);
            }
        }
        public void AddBlipsToMap()
        {
            CreatedBlips = new List<Blip>();
        }
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
    }
}