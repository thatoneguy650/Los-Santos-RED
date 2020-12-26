using LosSantosRED.lsr.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IDataMart
    {
        Settings Settings { get; }
        Names Names { get; }
        Weapons Weapons { get; }
        CountyJurisdictions CountyJurisdictions { get; }
        ZoneJurisdictions ZoneJurisdiction { get; }
        Zones Zones { get; }
        Streets Streets { get; }
        Agencies Agencies { get; }
        PlateTypes PlateTypes { get; }
        Places Places { get; }
        VehicleScannerAudio VehicleScannerAudio { get; }
        StreetScannerAudio StreetScannerAudio { get; }
        ZoneScannerAudio ZoneScannerAudio { get; }
        Bones Bones { get; }
    }
}
