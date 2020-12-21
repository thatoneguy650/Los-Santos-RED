using LosSantosRED.lsr.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr
{
    public class DataMart
    {
        public DataMart()
        {

        }
        public Places Places { get; private set; } = new Places();
        public Agencies Agencies { get; private set; } = new Agencies();
        public CountyJurisdictions CountyJurisdictions { get; private set; } = new CountyJurisdictions();
        public ZoneJurisdictions ZoneJurisdiction { get; private set; } = new ZoneJurisdictions();
        public Zones Zones { get; private set; } = new Zones();
        public PlateTypes PlateTypes { get; private set; } = new PlateTypes();
        public Settings Settings { get; private set; } = new Settings();
        public Streets Streets { get; private set; } = new Streets();
        public Weapons Weapons { get; private set; } = new Weapons();
        public Names Names { get; private set; } = new Names();
        public VehicleScannerAudio VehicleScannerAudio { get; private set; } = new VehicleScannerAudio();
        public ZoneScannerAudio ZoneScannerAudio { get; private set; } = new ZoneScannerAudio();
        public StreetScannerAudio StreetScannerAudio { get; private set; } = new StreetScannerAudio();
        public Bones Bones { get; private set; } = new Bones();
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
