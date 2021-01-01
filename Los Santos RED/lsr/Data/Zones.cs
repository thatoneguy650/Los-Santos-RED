using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

public class Zones : IZones
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Zones.xml";
    private List<Zone> ZoneList = new List<Zone>();
    public void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            ZoneList = Serialization.DeserializeParams<Zone>(ConfigFileName);
        }
        else
        {
            DefaultConfig();
            Serialization.SerializeParams(ZoneList, ConfigFileName);
        }
    }
    public Zone GetZone(Vector3 ZonePosition)
    {
        Zone ListResult = null;
        ListResult = ZoneList.Where(x => x.Boundaries != null && IsPointInPolygon(new Vector2(ZonePosition.X, ZonePosition.Y), x.Boundaries)).FirstOrDefault();
        if (ListResult == null)
        {
            string zoneName = GetInternalZoneString(ZonePosition);
            ListResult = ZoneList.Where(x => x.InternalGameName.ToUpper() == zoneName.ToUpper()).FirstOrDefault();
        }
        if (ListResult == null)
        {
            return new Zone("UNK", "Unknown", County.Unknown, "Unknown");
        }
        else
        {
            return ListResult;
        }
    }
    public Zone GetZone(string InternalGameName)
    {
        return ZoneList.Where(x => x.InternalGameName.ToLower() == InternalGameName.ToLower()).FirstOrDefault();
    }
    private string GetInternalZoneString(Vector3 ZonePosition)
    {
        string zoneName;
        unsafe
        {
            IntPtr ptr = Rage.Native.NativeFunction.CallByName<IntPtr>("GET_NAME_OF_ZONE", ZonePosition.X, ZonePosition.Y, ZonePosition.Z);

            zoneName = Marshal.PtrToStringAnsi(ptr);
        }
        return zoneName;
    }
    private void DefaultConfig()
    {

        ZoneList = new List<Zone>
        {
            //One Off
            new Zone("OCEANA", "Pacific Ocean", County.PacificOcean, "Unknown"),

            //North Blaine
            new Zone("PROCOB", "Procopio Beach", County.BlaineCounty, "San Andreas"),
            new Zone("MTCHIL", "Mount Chiliad", County.BlaineCounty, "San Andreas"),
            new Zone("MTGORDO", "Mount Gordo", County.BlaineCounty, "San Andreas"),
            new Zone("PALETO", "Paleto Bay", County.BlaineCounty, "San Andreas"),
            new Zone("PALCOV", "Paleto Cove", County.BlaineCounty, "San Andreas"),
            new Zone("PALFOR", "Paleto Forest", County.BlaineCounty, "San Andreas"),
            new Zone("CMSW", "Chiliad Mountain State Wilderness", County.BlaineCounty, "San Andreas"),
            new Zone("CALAFB", "Calafia Bridge", County.BlaineCounty, "San Andreas"),
            new Zone("GALFISH", "Galilee", County.BlaineCounty, "San Andreas"),
            new Zone("ELGORL", "El Gordo Lighthouse", County.BlaineCounty, "San Andreas"),
            new Zone("GRAPES", "Grapeseed", County.BlaineCounty, "San Andreas"),
            new Zone("BRADP", "Braddock Pass", County.BlaineCounty, "San Andreas"),
            new Zone("BRADT", "Braddock Tunnel", County.BlaineCounty, "San Andreas"),
            new Zone("CCREAK", "Cassidy Creek", County.BlaineCounty, "San Andreas"),

            //Blaine
            new Zone("ALAMO", "Alamo Sea", County.BlaineCounty, "San Andreas"),
            new Zone("ARMYB", "Fort Zancudo", County.BlaineCounty, "San Andreas"),
            new Zone("CANNY", "Raton Canyon", County.BlaineCounty, "San Andreas"), 
            new Zone("DESRT", "Grand Senora Desert", County.BlaineCounty, "San Andreas"),
            new Zone("HUMLAB", "Humane Labs and Research", County.BlaineCounty, "San Andreas"),
            new Zone("JAIL", "Bolingbroke Penitentiary", County.BlaineCounty, "San Andreas") { IsRestrictedDuringWanted = true },
            new Zone("LAGO", "Lago Zancudo", County.BlaineCounty, "San Andreas"),
            new Zone("MTJOSE", "Mount Josiah", County.BlaineCounty, "San Andreas"),
            new Zone("NCHU", "North Chumash", County.BlaineCounty, "San Andreas"),  
            new Zone("SANCHIA", "San Chianski Mountain Range", County.BlaineCounty, "San Andreas"),
            new Zone("SANDY", "Sandy Shores", County.BlaineCounty, "San Andreas"),
            new Zone("SLAB", "Slab City", County.BlaineCounty, "San Andreas"),
            new Zone("ZANCUDO", "Zancudo River", County.BlaineCounty, "San Andreas"),
            new Zone("ZQ_UAR", "Davis Quartz", County.BlaineCounty, "San Andreas"),

            //Vespucci
            new Zone("BEACH", "Vespucci Beach", County.CityOfLosSantos, "San Andreas"),
            new Zone("DELBE", "Del Perro Beach", County.CityOfLosSantos, "San Andreas"),
            new Zone("DELPE", "Del Perro", County.CityOfLosSantos, "San Andreas"),
            new Zone("VCANA", "Vespucci Canals", County.CityOfLosSantos, "San Andreas"),
            new Zone("VESP", "Vespucci Metro", County.CityOfLosSantos, "San Andreas"),
            new Zone("LOSPUER", "La Puerta", County.CityOfLosSantos, "San Andreas"),
            new Zone("PBLUFF", "Pacific Bluffs", County.CityOfLosSantos, "San Andreas"),
            new Zone("DELSOL", "Puerto Del Sol", County.CityOfLosSantos, "San Andreas"),

            //Central
            new Zone("BANNING", "Banning", County.CityOfLosSantos, "San Andreas"),
            new Zone("CHAMH", "Chamberlain Hills", County.CityOfLosSantos, "San Andreas"),
            new Zone("DAVIS", "Davis", County.CityOfLosSantos, "San Andreas"),
            new Zone("DOWNT", "Downtown", County.CityOfLosSantos, "San Andreas"),
            new Zone("PBOX", "Pillbox Hill", County.CityOfLosSantos, "San Andreas"),
            new Zone("RANCHO", "Rancho", County.CityOfLosSantos, "San Andreas"),
            new Zone("SKID", "Mission Row", County.CityOfLosSantos, "San Andreas"),
            new Zone("STAD", "Maze Bank Arena", County.CityOfLosSantos, "San Andreas"),
            new Zone("STRAW", "Strawberry", County.CityOfLosSantos, "San Andreas"),
            new Zone("TEXTI", "Textile City", County.CityOfLosSantos, "San Andreas"),
            new Zone("LEGSQU", "Legion Square", County.CityOfLosSantos, "San Andreas"),

            //East LS
            new Zone("CYPRE", "Cypress Flats", County.CityOfLosSantos, "San Andreas"),
            new Zone("LMESA", "La Mesa", County.CityOfLosSantos, "San Andreas"),
            new Zone("MIRR", "Mirror Park", County.CityOfLosSantos, "San Andreas"),
            new Zone("MURRI", "Murrieta Heights", County.CityOfLosSantos, "San Andreas"),
            new Zone("EBURO", "El Burro Heights", County.CityOfLosSantos, "San Andreas"),

            //Vinewood
            new Zone("ALTA", "Alta", County.CityOfLosSantos, "San Andreas"),
            new Zone("DTVINE", "Downtown Vinewood", County.CityOfLosSantos, "San Andreas"),
            new Zone("EAST_V", "East Vinewood", County.CityOfLosSantos, "San Andreas"),
            new Zone("HAWICK", "Hawick", County.CityOfLosSantos, "San Andreas"),
            new Zone("HORS", "Vinewood Racetrack", County.CityOfLosSantos, "San Andreas"),
            new Zone("VINE", "Vinewood", County.CityOfLosSantos, "San Andreas"),
            new Zone("WVINE", "West Vinewood", County.CityOfLosSantos, "San Andreas"),

            //PortOfLosSantos
            new Zone("ELYSIAN", "Elysian Island", County.CityOfLosSantos, "San Andreas"),
            new Zone("ZP_ORT", "Port of South Los Santos", County.CityOfLosSantos, "San Andreas"),
            new Zone("TERMINA", "Terminal", County.CityOfLosSantos, "San Andreas"),
            new Zone("ZP_ORT", "Port of South Los Santos", County.CityOfLosSantos, "San Andreas"),
            new Zone("AIRP", "Los Santos International Airport", County.CityOfLosSantos, "San Andreas") { IsRestrictedDuringWanted = true },

            //Rockford Hills
            new Zone("BURTON", "Burton", County.CityOfLosSantos, "San Andreas"),
            new Zone("GOLF", "GWC and Golfing Society", County.CityOfLosSantos, "San Andreas"),
            new Zone("KOREAT", "Little Seoul", County.CityOfLosSantos, "San Andreas"),
            new Zone("MORN", "Morningwood", County.CityOfLosSantos, "San Andreas"),
            new Zone("MOVIE", "Richards Majestic", County.CityOfLosSantos, "San Andreas"),
            new Zone("RICHM", "Richman", County.CityOfLosSantos, "San Andreas"),
            new Zone("ROCKF", "Rockford Hills", County.CityOfLosSantos, "San Andreas"),     

            //Vinewood Hills
            new Zone("CHIL", "Vinewood Hills", County.LosSantosCounty, "San Andreas"),
            new Zone("GREATC", "Great Chaparral", County.LosSantosCounty, "San Andreas"),
            new Zone("BAYTRE", "Baytree Canyon", County.LosSantosCounty, "San Andreas"),
            new Zone("RGLEN", "Richman Glen", County.LosSantosCounty, "San Andreas"),
            new Zone("TONGVAV", "Tongva Valley", County.LosSantosCounty, "San Andreas"),
            new Zone("HARMO", "Harmony", County.LosSantosCounty, "San Andreas"),
            new Zone("RTRAK", "Redwood Lights Track", County.LosSantosCounty, "San Andreas"),
           
            //Chumash
            new Zone("BANHAMC", "Banham Canyon Dr", County.LosSantosCounty, "San Andreas"),
            new Zone("BHAMCA", "Banham Canyon", County.LosSantosCounty, "San Andreas"),
            new Zone("CHU", "Chumash", County.LosSantosCounty, "San Andreas"),
            new Zone("TONGVAH", "Tongva Hills", County.LosSantosCounty, "San Andreas"),
           
            //Tataviam 
            new Zone("LACT", "Land Act Reservoir", County.LosSantosCounty, "San Andreas"),
            new Zone("LDAM", "Land Act Dam", County.LosSantosCounty, "San Andreas"),
            new Zone("NOOSE", "N.O.O.S.E", County.LosSantosCounty, "San Andreas") { IsRestrictedDuringWanted = true },
            new Zone("PALHIGH", "Palomino Highlands", County.LosSantosCounty, "San Andreas"),
            new Zone("PALMPOW", "Palmer - Taylor Power Station", County.LosSantosCounty, "San Andreas"),
            new Zone("SANAND", "San Andreas", County.LosSantosCounty, "San Andreas"),
            new Zone("TATAMO", "Tataviam Mountains", County.LosSantosCounty, "San Andreas"),
            new Zone("WINDF", "Ron Alternates Wind Farm", County.LosSantosCounty, "San Andreas"),

            //Other
            new Zone("LUDEN", "Ludendorff", County.NorthYankton, new Vector2[] { new Vector2 { X = 2545.142f, Y = -5124.292f },
                                        new Vector2 { X = 2648.361f, Y = -4091.664f },
                                        new Vector2 { X = 5647.14f, Y = -4131.478f },
                                        new Vector2 { X = 5922.999f, Y = -5640.681f } }, "North Yankton"),

            new Zone("CHI1", "Acadia", County.Crook, new Vector2[] { new Vector2 { X = 4830.579f, Y = 1982.126f },
                                        new Vector2 { X = 7898.494f, Y = 3093.242f },
                                        new Vector2 { X = 5845.111f, Y = 8616.287f },
                                        new Vector2 { X = 1748.942f, Y = 8188.261f } }, "Lincoln"),


            new Zone("VICE", "Vice City", County.Vice, new Vector2[] { new Vector2 { X = 4669.141f, Y = -1614.298f },
                                        new Vector2 { X = 4920.789f, Y = 2035.281f },
                                        new Vector2 { X = 7549.999f, Y = 2008.153f },
                                        new Vector2 { X = 7770.286f, Y = -1657.735f } }, "Florida")


            };
    }
    private bool IsPointInPolygon(Vector2 point, Vector2[] polygon)
    {
        int polygonLength = polygon.Length, i = 0;
        bool inside = false;
        // x, y for tested point.
        float pointX = point.X, pointY = point.Y;
        // start / end point for the current polygon segment.
        float startX, startY, endX, endY;
        Vector2 endPoint = polygon[polygonLength - 1];
        endX = endPoint.X;
        endY = endPoint.Y;
        while (i < polygonLength)
        {
            startX = endX; startY = endY;
            endPoint = polygon[i++];
            endX = endPoint.X; endY = endPoint.Y;
            //
            inside ^= (endY > pointY ^ startY > pointY) /* ? pointY inside [startY;endY] segment ? */
                      && /* if so, test if it is under the segment */
                      ((pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY));
        }
        return inside;
    }
}
