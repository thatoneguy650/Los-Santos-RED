using ExtensionsMethods;
using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

public static class Zones
{
    private static string ConfigFileName = "Plugins\\LosSantosRED\\Zones.xml";

    public static List<Zone> ZoneList = new List<Zone>();
    public static void Initialize()
    {
        ReadConfig();
    }
    public static void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            ZoneList = General.DeserializeParams<Zone>(ConfigFileName);
        }
        else
        {
            DefaultConfig();
            General.SerializeParams(ZoneList, ConfigFileName);
        }
    }
    private static void DefaultConfig()
    {

        ZoneList = new List<Zone>
        {
            //One Off
            new Zone("OCEANA", "Pacific Ocean", Zone.County.PacificOcean),

            //North Blaine
            new Zone("PROCOB", "Procopio Beach", Zone.County.BlaineCounty),
            new Zone("MTCHIL", "Mount Chiliad", Zone.County.BlaineCounty),
            new Zone("MTGORDO", "Mount Gordo", Zone.County.BlaineCounty),
            new Zone("PALETO", "Paleto Bay", Zone.County.BlaineCounty),
            new Zone("PALCOV", "Paleto Cove", Zone.County.BlaineCounty),
            new Zone("PALFOR", "Paleto Forest", Zone.County.BlaineCounty),
            new Zone("CMSW", "Chiliad Mountain State Wilderness", Zone.County.BlaineCounty),
            new Zone("CALAFB", "Calafia Bridge", Zone.County.BlaineCounty),
            new Zone("GALFISH", "Galilee", Zone.County.BlaineCounty),
            new Zone("ELGORL", "El Gordo Lighthouse", Zone.County.BlaineCounty),
            new Zone("GRAPES", "Grapeseed", Zone.County.BlaineCounty),
            new Zone("BRADP", "Braddock Pass", Zone.County.BlaineCounty),
            new Zone("BRADT", "Braddock Tunnel", Zone.County.BlaineCounty),
            new Zone("CCREAK", "Cassidy Creek", Zone.County.BlaineCounty),

            //Blaine
            new Zone("ALAMO", "Alamo Sea", Zone.County.BlaineCounty),
            new Zone("ARMYB", "Fort Zancudo", Zone.County.BlaineCounty),
            new Zone("CANNY", "Raton Canyon", Zone.County.BlaineCounty), 
            new Zone("DESRT", "Grand Senora Desert", Zone.County.BlaineCounty),
            new Zone("HUMLAB", "Humane Labs and Research", Zone.County.BlaineCounty),
            new Zone("JAIL", "Bolingbroke Penitentiary", Zone.County.BlaineCounty) { IsRestrictedDuringWanted = true },
            new Zone("LAGO", "Lago Zancudo", Zone.County.BlaineCounty),
            new Zone("MTJOSE", "Mount Josiah", Zone.County.BlaineCounty),
            new Zone("NCHU", "North Chumash", Zone.County.BlaineCounty),  
            new Zone("SANCHIA", "San Chianski Mountain Range", Zone.County.BlaineCounty),
            new Zone("SANDY", "Sandy Shores", Zone.County.BlaineCounty),
            new Zone("SLAB", "Slab City", Zone.County.BlaineCounty),
            new Zone("ZANCUDO", "Zancudo River", Zone.County.BlaineCounty),
            new Zone("ZQ_UAR", "Davis Quartz", Zone.County.BlaineCounty),

            //Vespucci
            new Zone("BEACH", "Vespucci Beach", Zone.County.CityOfLosSantos),
            new Zone("DELBE", "Del Perro Beach", Zone.County.CityOfLosSantos),
            new Zone("DELPE", "Del Perro", Zone.County.CityOfLosSantos),
            new Zone("VCANA", "Vespucci Canals", Zone.County.CityOfLosSantos),
            new Zone("VESP", "Vespucci Metro", Zone.County.CityOfLosSantos),
            new Zone("LOSPUER", "La Puerta", Zone.County.CityOfLosSantos),
            new Zone("PBLUFF", "Pacific Bluffs", Zone.County.CityOfLosSantos),
            new Zone("DELSOL", "Puerto Del Sol", Zone.County.CityOfLosSantos),

            //Central
            new Zone("BANNING", "Banning", Zone.County.CityOfLosSantos),
            new Zone("CHAMH", "Chamberlain Hills", Zone.County.CityOfLosSantos),
            new Zone("DAVIS", "Davis", Zone.County.CityOfLosSantos),
            new Zone("DOWNT", "Downtown", Zone.County.CityOfLosSantos),
            new Zone("PBOX", "Pillbox Hill", Zone.County.CityOfLosSantos),
            new Zone("RANCHO", "Rancho", Zone.County.CityOfLosSantos),
            new Zone("SKID", "Mission Row", Zone.County.CityOfLosSantos),
            new Zone("STAD", "Maze Bank Arena", Zone.County.CityOfLosSantos),
            new Zone("STRAW", "Strawberry", Zone.County.CityOfLosSantos),
            new Zone("TEXTI", "Textile City", Zone.County.CityOfLosSantos),
            new Zone("LEGSQU", "Legion Square", Zone.County.CityOfLosSantos),

            //East LS
            new Zone("CYPRE", "Cypress Flats", Zone.County.CityOfLosSantos),
            new Zone("LMESA", "La Mesa", Zone.County.CityOfLosSantos),
            new Zone("MIRR", "Mirror Park", Zone.County.CityOfLosSantos),
            new Zone("MURRI", "Murrieta Heights", Zone.County.CityOfLosSantos),
            new Zone("EBURO", "El Burro Heights", Zone.County.CityOfLosSantos),

            //Vinewood
            new Zone("ALTA", "Alta", Zone.County.CityOfLosSantos),
            new Zone("DTVINE", "Downtown Vinewood", Zone.County.CityOfLosSantos),
            new Zone("EAST_V", "East Vinewood", Zone.County.CityOfLosSantos),
            new Zone("HAWICK", "Hawick", Zone.County.CityOfLosSantos),
            new Zone("HORS", "Vinewood Racetrack", Zone.County.CityOfLosSantos),
            new Zone("VINE", "Vinewood", Zone.County.CityOfLosSantos),
            new Zone("WVINE", "West Vinewood", Zone.County.CityOfLosSantos),

            //PortOfLosSantos
            new Zone("ELYSIAN", "Elysian Island", Zone.County.CityOfLosSantos),
            new Zone("ZP_ORT", "Port of South Los Santos", Zone.County.CityOfLosSantos),
            new Zone("TERMINA", "Terminal", Zone.County.CityOfLosSantos),
            new Zone("ZP_ORT", "Port of South Los Santos", Zone.County.CityOfLosSantos),
            new Zone("AIRP", "Los Santos International Airport", Zone.County.CityOfLosSantos) { IsRestrictedDuringWanted = true },

            //Rockford Hills
            new Zone("BURTON", "Burton", Zone.County.CityOfLosSantos),
            new Zone("GOLF", "GWC and Golfing Society", Zone.County.CityOfLosSantos),
            new Zone("KOREAT", "Little Seoul", Zone.County.CityOfLosSantos),
            new Zone("MORN", "Morningwood", Zone.County.CityOfLosSantos),
            new Zone("MOVIE", "Richards Majestic", Zone.County.CityOfLosSantos),
            new Zone("RICHM", "Richman", Zone.County.CityOfLosSantos),
            new Zone("ROCKF", "Rockford Hills", Zone.County.CityOfLosSantos),     

            //Vinewood Hills
            new Zone("CHIL", "Vinewood Hills", Zone.County.LosSantosCounty),
            new Zone("GREATC", "Great Chaparral", Zone.County.LosSantosCounty),
            new Zone("BAYTRE", "Baytree Canyon", Zone.County.LosSantosCounty),
            new Zone("RGLEN", "Richman Glen", Zone.County.LosSantosCounty),
            new Zone("TONGVAV", "Tongva Valley", Zone.County.LosSantosCounty),
            new Zone("HARMO", "Harmony", Zone.County.LosSantosCounty),
            new Zone("RTRAK", "Redwood Lights Track", Zone.County.LosSantosCounty),
           
            //Chumash
            new Zone("BANHAMC", "Banham Canyon Dr", Zone.County.LosSantosCounty),
            new Zone("BHAMCA", "Banham Canyon", Zone.County.LosSantosCounty),
            new Zone("CHU", "Chumash", Zone.County.LosSantosCounty),
            new Zone("TONGVAH", "Tongva Hills", Zone.County.LosSantosCounty),
           
            //Tataviam 
            new Zone("LACT", "Land Act Reservoir", Zone.County.LosSantosCounty),
            new Zone("LDAM", "Land Act Dam", Zone.County.LosSantosCounty),
            new Zone("NOOSE", "N.O.O.S.E", Zone.County.LosSantosCounty) { IsRestrictedDuringWanted = true },
            new Zone("PALHIGH", "Palomino Highlands", Zone.County.LosSantosCounty),
            new Zone("PALMPOW", "Palmer - Taylor Power Station", Zone.County.LosSantosCounty),
            new Zone("SANAND", "San Andreas", Zone.County.LosSantosCounty),
            new Zone("TATAMO", "Tataviam Mountains", Zone.County.LosSantosCounty),
            new Zone("WINDF", "Ron Alternates Wind Farm", Zone.County.LosSantosCounty),
    };
        
    }
    public static Zone GetZoneAtLocation(Vector3 ZonePosition)
    {
        string zoneName = GetZoneStringAtLocation(ZonePosition);
        Zone ListResult = ZoneList.Where(x => x.InternalGameName.ToUpper() == zoneName.ToUpper()).FirstOrDefault();
        if(ListResult == null)
        {
            if (ZonePosition.IsInLosSantosCity())
                return new Zone("UNK_LSCITY", "Los Santos", Zone.County.CityOfLosSantos);
            else
                return new Zone("UNK_LSCOUNTY", "Los Santos County", Zone.County.LosSantosCounty);
        }
        else
        {
            return ListResult;
        }
    }
    public static string GetZoneStringAtLocation(Vector3 ZonePosition)
    {
        string zoneName;
        unsafe
        {
            IntPtr ptr = Rage.Native.NativeFunction.CallByName<IntPtr>("GET_NAME_OF_ZONE", ZonePosition.X, ZonePosition.Y, ZonePosition.Z);

            zoneName = Marshal.PtrToStringAnsi(ptr);
        }
        return zoneName;
    }
    public static string GetFormattedZoneName(Zone MyZone,bool WithCounty)
    {
        if (WithCounty)
        {
            string CountyName = "San Andreas";
            if (MyZone.ZoneCounty == Zone.County.BlaineCounty)
                CountyName = "Blaine County";
            else if (MyZone.ZoneCounty == Zone.County.CityOfLosSantos)
                CountyName = "City of Los Santos";
            else if (MyZone.ZoneCounty == Zone.County.LosSantosCounty)
                CountyName = "Los Santos County";

            return MyZone.DisplayName + ", " + CountyName;
        }
        else
        {
            return MyZone.DisplayName;
        }

    }
    public static Zone GetZoneByName(string InternalGameName)
    {
        return ZoneList.Where(x => x.InternalGameName.ToLower() == InternalGameName.ToLower()).FirstOrDefault();
    }
}
[Serializable()]
public class Zone
{
    public enum County
    {
        CityOfLosSantos = 0,
        LosSantosCounty = 1,
        BlaineCounty = 2,
        PacificOcean = 3,
    }
    public Zone()
    {

    }
    public Zone(string _GameName, string _TextName, County _ZoneCounty)
    {
        InternalGameName = _GameName;
        DisplayName = _TextName;
        ZoneCounty = _ZoneCounty;
    }
    public string DispatchUnitName { get; set; }
    public string InternalGameName { get; set; }
    public string DisplayName { get; set; }
    public County ZoneCounty { get; set; }
    public bool IsRestrictedDuringWanted { get; set; } = false;

}
