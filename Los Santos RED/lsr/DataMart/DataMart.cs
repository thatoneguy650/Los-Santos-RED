using LosSantosRED.lsr.Data;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr
{
    public class DataMart : IDataMart, IAgencyWeaponProvider, IJurisdictionProvider, IZoneProvider, IAgencyProvider, ISettingsProvider, IWeaponProvider, IZoneAgencyProvider, IAgencyWeaponSettingsProvider, IWeaponPlacesSettingsProvider
        , IPlacesProvider, IStreetProvider, IStreetZoneProvider, IJurisdictionStreetZoneProvider, IStreetZoneWeaponSettingsProvider
    {

        public DataMart()
        {
            Places = new Places();
            Zones = new Zones();
            PlateTypes = new PlateTypes();
            Settings = new Settings();
            Streets = new Streets();
            Weapons = new Weapons();
            Names = new Names();
            VehicleScannerAudio = new VehicleScannerAudio();
            ZoneScannerAudio = new ZoneScannerAudio();
            StreetScannerAudio = new StreetScannerAudio();
            Agencies = new Agencies(this);
            CountyJurisdictions = new CountyJurisdictions(this);
            ZoneJurisdiction = new ZoneJurisdictions(this);
        }
        public Places Places { get; private set; }
        public Agencies Agencies { get; private set; }
        public CountyJurisdictions CountyJurisdictions { get; private set; }
        public ZoneJurisdictions ZoneJurisdiction { get; private set; }
        public Zones Zones { get; private set; }
        public PlateTypes PlateTypes { get; private set; } 
        public Settings Settings { get; private set; } 
        public Streets Streets { get; private set; }
        public Weapons Weapons { get; private set; } 
        public Names Names { get; private set; } 
        public VehicleScannerAudio VehicleScannerAudio { get; private set; } 
        public ZoneScannerAudio ZoneScannerAudio { get; private set; } 
        public StreetScannerAudio StreetScannerAudio { get; private set; } 
        public Bones Bones { get; private set; } 
        public void ReadConfig()
        {
            Places.ReadConfig();
            PlateTypes.ReadConfig();
            Agencies.ReadConfig();
            Zones.ReadConfig();
            Streets.ReadConfig();
            Settings.ReadConfig();
            ZoneJurisdiction.ReadConfig();
            CountyJurisdictions.ReadConfig();
            Weapons.ReadConfig();
            VehicleScannerAudio.ReadConfig();
            ZoneScannerAudio.ReadConfig();
            StreetScannerAudio.ReadConfig();
            Names.ReadConfig();
        }
    }
}
