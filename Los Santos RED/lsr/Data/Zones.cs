using ExtensionsMethods;
using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

public static class Zones
{
    private static readonly string ConfigFileName = "Plugins\\LosSantosRED\\Zones.xml";
    private static List<Zone> ZoneList = new List<Zone>();
    public static void Initialize()
    {
        if (File.Exists(ConfigFileName))
        {
            ZoneList = SettingsManager.DeserializeParams<Zone>(ConfigFileName);
        }
        else
        {
            DefaultConfig();
            SettingsManager.SerializeParams(ZoneList, ConfigFileName);
        }
    }
    public static Zone GetZone(Vector3 ZonePosition)
    {
        string zoneName = GetInternalZoneString(ZonePosition);
        Zone ListResult = ZoneList.Where(x => x.InternalGameName.ToUpper() == zoneName.ToUpper()).FirstOrDefault();
        if (ListResult == null)
        {
            if (ZonePosition.IsInLosSantosCity())
                return new Zone("UNK_LSCITY", "Los Santos", County.CityOfLosSantos);
            else
                return new Zone("UNK_LSCOUNTY", "Los Santos County", County.LosSantosCounty);
        }
        else
        {
            return ListResult;
        }
    }
    public static Zone GetZone(string InternalGameName)
    {
        return ZoneList.Where(x => x.InternalGameName.ToLower() == InternalGameName.ToLower()).FirstOrDefault();
    }
    public static string GetName(Zone MyZone, bool WithCounty)
    {
        if (WithCounty)
        {
            string CountyName = "San Andreas";
            if (MyZone.ZoneCounty == County.BlaineCounty)
                CountyName = "Blaine County";
            else if (MyZone.ZoneCounty == County.CityOfLosSantos)
                CountyName = "City of Los Santos";
            else if (MyZone.ZoneCounty == County.LosSantosCounty)
                CountyName = "Los Santos County";

            return MyZone.DisplayName + ", " + CountyName;
        }
        else
        {
            return MyZone.DisplayName;
        }

    }
    private static string GetInternalZoneString(Vector3 ZonePosition)
    {
        string zoneName;
        unsafe
        {
            IntPtr ptr = Rage.Native.NativeFunction.CallByName<IntPtr>("GET_NAME_OF_ZONE", ZonePosition.X, ZonePosition.Y, ZonePosition.Z);

            zoneName = Marshal.PtrToStringAnsi(ptr);
        }
        return zoneName;
    }
    private static void DefaultConfig()
    {

        ZoneList = new List<Zone>
        {
            //One Off
            new Zone("OCEANA", "Pacific Ocean", County.PacificOcean),

            //North Blaine
            new Zone("PROCOB", "Procopio Beach", County.BlaineCounty),
            new Zone("MTCHIL", "Mount Chiliad", County.BlaineCounty),
            new Zone("MTGORDO", "Mount Gordo", County.BlaineCounty),
            new Zone("PALETO", "Paleto Bay", County.BlaineCounty),
            new Zone("PALCOV", "Paleto Cove", County.BlaineCounty),
            new Zone("PALFOR", "Paleto Forest", County.BlaineCounty),
            new Zone("CMSW", "Chiliad Mountain State Wilderness", County.BlaineCounty),
            new Zone("CALAFB", "Calafia Bridge", County.BlaineCounty),
            new Zone("GALFISH", "Galilee", County.BlaineCounty),
            new Zone("ELGORL", "El Gordo Lighthouse", County.BlaineCounty),
            new Zone("GRAPES", "Grapeseed", County.BlaineCounty),
            new Zone("BRADP", "Braddock Pass", County.BlaineCounty),
            new Zone("BRADT", "Braddock Tunnel", County.BlaineCounty),
            new Zone("CCREAK", "Cassidy Creek", County.BlaineCounty),

            //Blaine
            new Zone("ALAMO", "Alamo Sea", County.BlaineCounty),
            new Zone("ARMYB", "Fort Zancudo", County.BlaineCounty),
            new Zone("CANNY", "Raton Canyon", County.BlaineCounty), 
            new Zone("DESRT", "Grand Senora Desert", County.BlaineCounty),
            new Zone("HUMLAB", "Humane Labs and Research", County.BlaineCounty),
            new Zone("JAIL", "Bolingbroke Penitentiary", County.BlaineCounty) { IsRestrictedDuringWanted = true },
            new Zone("LAGO", "Lago Zancudo", County.BlaineCounty),
            new Zone("MTJOSE", "Mount Josiah", County.BlaineCounty),
            new Zone("NCHU", "North Chumash", County.BlaineCounty),  
            new Zone("SANCHIA", "San Chianski Mountain Range", County.BlaineCounty),
            new Zone("SANDY", "Sandy Shores", County.BlaineCounty),
            new Zone("SLAB", "Slab City", County.BlaineCounty),
            new Zone("ZANCUDO", "Zancudo River", County.BlaineCounty),
            new Zone("ZQ_UAR", "Davis Quartz", County.BlaineCounty),

            //Vespucci
            new Zone("BEACH", "Vespucci Beach", County.CityOfLosSantos),
            new Zone("DELBE", "Del Perro Beach", County.CityOfLosSantos),
            new Zone("DELPE", "Del Perro", County.CityOfLosSantos),
            new Zone("VCANA", "Vespucci Canals", County.CityOfLosSantos),
            new Zone("VESP", "Vespucci Metro", County.CityOfLosSantos),
            new Zone("LOSPUER", "La Puerta", County.CityOfLosSantos),
            new Zone("PBLUFF", "Pacific Bluffs", County.CityOfLosSantos),
            new Zone("DELSOL", "Puerto Del Sol", County.CityOfLosSantos),

            //Central
            new Zone("BANNING", "Banning", County.CityOfLosSantos),
            new Zone("CHAMH", "Chamberlain Hills", County.CityOfLosSantos),
            new Zone("DAVIS", "Davis", County.CityOfLosSantos),
            new Zone("DOWNT", "Downtown", County.CityOfLosSantos),
            new Zone("PBOX", "Pillbox Hill", County.CityOfLosSantos),
            new Zone("RANCHO", "Rancho", County.CityOfLosSantos),
            new Zone("SKID", "Mission Row", County.CityOfLosSantos),
            new Zone("STAD", "Maze Bank Arena", County.CityOfLosSantos),
            new Zone("STRAW", "Strawberry", County.CityOfLosSantos),
            new Zone("TEXTI", "Textile City", County.CityOfLosSantos),
            new Zone("LEGSQU", "Legion Square", County.CityOfLosSantos),

            //East LS
            new Zone("CYPRE", "Cypress Flats", County.CityOfLosSantos),
            new Zone("LMESA", "La Mesa", County.CityOfLosSantos),
            new Zone("MIRR", "Mirror Park", County.CityOfLosSantos),
            new Zone("MURRI", "Murrieta Heights", County.CityOfLosSantos),
            new Zone("EBURO", "El Burro Heights", County.CityOfLosSantos),

            //Vinewood
            new Zone("ALTA", "Alta", County.CityOfLosSantos),
            new Zone("DTVINE", "Downtown Vinewood", County.CityOfLosSantos),
            new Zone("EAST_V", "East Vinewood", County.CityOfLosSantos),
            new Zone("HAWICK", "Hawick", County.CityOfLosSantos),
            new Zone("HORS", "Vinewood Racetrack", County.CityOfLosSantos),
            new Zone("VINE", "Vinewood", County.CityOfLosSantos),
            new Zone("WVINE", "West Vinewood", County.CityOfLosSantos),

            //PortOfLosSantos
            new Zone("ELYSIAN", "Elysian Island", County.CityOfLosSantos),
            new Zone("ZP_ORT", "Port of South Los Santos", County.CityOfLosSantos),
            new Zone("TERMINA", "Terminal", County.CityOfLosSantos),
            new Zone("ZP_ORT", "Port of South Los Santos", County.CityOfLosSantos),
            new Zone("AIRP", "Los Santos International Airport", County.CityOfLosSantos) { IsRestrictedDuringWanted = true },

            //Rockford Hills
            new Zone("BURTON", "Burton", County.CityOfLosSantos),
            new Zone("GOLF", "GWC and Golfing Society", County.CityOfLosSantos),
            new Zone("KOREAT", "Little Seoul", County.CityOfLosSantos),
            new Zone("MORN", "Morningwood", County.CityOfLosSantos),
            new Zone("MOVIE", "Richards Majestic", County.CityOfLosSantos),
            new Zone("RICHM", "Richman", County.CityOfLosSantos),
            new Zone("ROCKF", "Rockford Hills", County.CityOfLosSantos),     

            //Vinewood Hills
            new Zone("CHIL", "Vinewood Hills", County.LosSantosCounty),
            new Zone("GREATC", "Great Chaparral", County.LosSantosCounty),
            new Zone("BAYTRE", "Baytree Canyon", County.LosSantosCounty),
            new Zone("RGLEN", "Richman Glen", County.LosSantosCounty),
            new Zone("TONGVAV", "Tongva Valley", County.LosSantosCounty),
            new Zone("HARMO", "Harmony", County.LosSantosCounty),
            new Zone("RTRAK", "Redwood Lights Track", County.LosSantosCounty),
           
            //Chumash
            new Zone("BANHAMC", "Banham Canyon Dr", County.LosSantosCounty),
            new Zone("BHAMCA", "Banham Canyon", County.LosSantosCounty),
            new Zone("CHU", "Chumash", County.LosSantosCounty),
            new Zone("TONGVAH", "Tongva Hills", County.LosSantosCounty),
           
            //Tataviam 
            new Zone("LACT", "Land Act Reservoir", County.LosSantosCounty),
            new Zone("LDAM", "Land Act Dam", County.LosSantosCounty),
            new Zone("NOOSE", "N.O.O.S.E", County.LosSantosCounty) { IsRestrictedDuringWanted = true },
            new Zone("PALHIGH", "Palomino Highlands", County.LosSantosCounty),
            new Zone("PALMPOW", "Palmer - Taylor Power Station", County.LosSantosCounty),
            new Zone("SANAND", "San Andreas", County.LosSantosCounty),
            new Zone("TATAMO", "Tataviam Mountains", County.LosSantosCounty),
            new Zone("WINDF", "Ron Alternates Wind Farm", County.LosSantosCounty),
    };
        
    }
}
