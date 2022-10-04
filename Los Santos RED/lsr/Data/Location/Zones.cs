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
    public List<Zone> ZoneList { get; private set; } = new List<Zone>();
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("Zones*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Zones config: {ConfigFile.FullName}",0);
            ZoneList = Serialization.DeserializeParams<Zone>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Zones config  {ConfigFileName}",0);
            ZoneList = Serialization.DeserializeParams<Zone>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Zones config found, creating default", 0);
            DefaultConfig();
            DefaultConfig_LibertyCity();
        }
    }
    public string GetZoneName(Vector3 ZonePosition)
    {
        Zone ListResult = null;
        string zoneName = "UNK";
        //ListResult = ZoneList.Where(x => x.Boundaries != null && IsPointInPolygon(new Vector2(ZonePosition.X, ZonePosition.Y), x.Boundaries)).FirstOrDefault();
        if (ListResult == null)
        {
            zoneName = GetInternalZoneString(ZonePosition);
            ListResult = ZoneList.Where(x => x.InternalGameName.ToUpper() == zoneName.ToUpper()).FirstOrDefault();
        }
        if (ListResult == null)
        {
            return "";
        }
        else
        {
            return ListResult.FullDisplayName;
        }
    }
    public Zone GetZone(Vector3 ZonePosition)
    {
        Zone ListResult = null;
        string zoneName = "UNK";
        //ListResult = ZoneList.Where(x => x.Boundaries != null && IsPointInPolygon(new Vector2(ZonePosition.X, ZonePosition.Y), x.Boundaries)).FirstOrDefault();
        if (ListResult == null)
        {
            zoneName = GetInternalZoneString(ZonePosition);
            ListResult = ZoneList.Where(x => x.InternalGameName.ToUpper() == zoneName.ToUpper()).FirstOrDefault();
        }
        if (ListResult == null)
        {
            return new Zone(zoneName, "Unknown", County.Unknown, "Unknown", false, eLocationEconomy.Middle, eLocationType.Rural);
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
        IntPtr ptr = Rage.Native.NativeFunction.Natives.GET_NAME_OF_ZONE<IntPtr>(ZonePosition.X, ZonePosition.Y, ZonePosition.Z);
        return Marshal.PtrToStringAnsi(ptr);
    }
    private void DefaultConfig()
    {

        ZoneList = new List<Zone>
        {
            //One Off
            new Zone("OCEANA", "Pacific Ocean", County.PacificOcean, "Unknown", false, eLocationEconomy.Poor, eLocationType.Wilderness),

            ////Ventura County?
            //new Zone("PROCOB", "Procopio Beach", County.VenturaCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("MTCHIL", "Mount Chiliad", County.VenturaCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),    
            //new Zone("PALETO", "Paleto Bay", County.VenturaCounty, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),
            //new Zone("PALCOV", "Paleto Cove", County.VenturaCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("PALFOR", "Paleto Forest", County.VenturaCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("CMSW", "Chiliad Mountain State Wilderness", County.VenturaCounty, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("CCREAK", "Cassidy Creek", County.VenturaCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("CALAFB", "Calafia Bridge", County.VenturaCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("GALFISH", "Galilee", County.VenturaCounty, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("BRADP", "Braddock Pass", County.VenturaCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("BRADT", "Braddock Tunnel", County.VenturaCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("ELGORL", "El Gordo Lighthouse", County.VenturaCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("MTGORDO", "Mount Gordo", County.VenturaCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //Ventura County is now blaine county
            new Zone("PROCOB", "Procopio Beach", County.BlaineCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("MTCHIL", "Mount Chiliad", County.BlaineCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("PALETO", "Paleto Bay", County.BlaineCounty, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("PALCOV", "Paleto Cove", County.BlaineCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("PALFOR", "Paleto Forest", County.BlaineCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("CMSW", "Chiliad Mountain State Wilderness", County.BlaineCounty, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("CCREAK", "Cassidy Creek", County.BlaineCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("CALAFB", "Calafia Bridge", County.BlaineCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("GALFISH", "Galilee", County.BlaineCounty, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("BRADP", "Braddock Pass", County.BlaineCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("BRADT", "Braddock Tunnel", County.BlaineCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("ELGORL", "El Gordo Lighthouse", County.BlaineCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("MTGORDO", "Mount Gordo", County.BlaineCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            
            //Majestic County
            new Zone("GRAPES", "Grapeseed", County.MajesticCounty, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),//has own PD

            new Zone("SANCHIA", "San Chianski Mountain Range", County.MajesticCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("ALAMO", "Alamo Sea", County.MajesticCounty, "San Andreas", true, eLocationEconomy.Poor, eLocationType.Wilderness),
            new Zone("DESRT", "Grand Senora Desert", County.MajesticCounty, "San Andreas", true, eLocationEconomy.Poor, eLocationType.Rural),
            new Zone("SANDY", "Sandy Shores", County.MajesticCounty, "San Andreas", false, eLocationEconomy.Poor, eLocationType.Rural),
            new Zone("HUMLAB", "Humane Labs and Research", County.MajesticCounty, "San Andreas", true, eLocationEconomy.Rich, eLocationType.Industrial),
            new Zone("JAIL", "Bolingbroke Penitentiary", County.MajesticCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Industrial) { IsRestrictedDuringWanted = true },
            new Zone("ZQ_UAR", "Davis Quartz", County.MajesticCounty, "San Andreas", true, eLocationEconomy.Poor, eLocationType.Wilderness),
            new Zone("HARMO", "Harmony", County.MajesticCounty, "San Andreas", false, eLocationEconomy.Poor, eLocationType.Rural),
            new Zone("RTRAK", "Redwood Lights Track", County.MajesticCounty, "San Andreas", true, eLocationEconomy.Poor, eLocationType.Industrial),

            //Blaine
            new Zone("ARMYB", "Fort Zancudo", County.BlaineCounty, "San Andreas", true, eLocationEconomy.Rich, eLocationType.Industrial),
            new Zone("CANNY", "Raton Canyon", County.BlaineCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("LAGO", "Lago Zancudo", County.BlaineCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("MTJOSE", "Mount Josiah", County.BlaineCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("NCHU", "North Chumash", County.BlaineCounty, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("SLAB", "Slab City", County.BlaineCounty, "San Andreas", false, eLocationEconomy.Poor, eLocationType.Rural),
            new Zone("ZANCUDO", "Zancudo River", County.BlaineCounty, "San Andreas", true, eLocationEconomy.Poor, eLocationType.Wilderness),

            //Vespucci
            new Zone("BEACH", "Vespucci Beach", County.CityOfLosSantos, "San Andreas", true, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("DELBE", "Del Perro Beach", County.CityOfLosSantos, "San Andreas", true, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("DELPE", "Del Perro", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("VCANA", "Vespucci Canals", County.CityOfLosSantos, "San Andreas", true, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("VESP", "Vespucci Metro", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("LOSPUER", "La Puerta", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("PBLUFF", "Pacific Bluffs", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Rich, eLocationType.Suburb),
            new Zone("DELSOL", "Puerto Del Sol", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Rich, eLocationType.Downtown),

            //Central
            new Zone("BANNING", "Banning", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("CHAMH", "Chamberlain Hills", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("DAVIS", "Davis", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("DOWNT", "Downtown", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("PBOX", "Pillbox Hill", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("RANCHO", "Rancho", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("SKID", "Mission Row", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("STAD", "Maze Bank Arena", County.CityOfLosSantos, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Industrial),
            new Zone("STRAW", "Strawberry", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("TEXTI", "Textile City", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("LEGSQU", "Legion Square", County.CityOfLosSantos, "San Andreas", true, eLocationEconomy.Poor, eLocationType.Downtown),

            //East LS
            new Zone("CYPRE", "Cypress Flats", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Poor, eLocationType.Industrial),
            new Zone("LMESA", "La Mesa", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Poor, eLocationType.Industrial),
            new Zone("MIRR", "Mirror Park", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Poor, eLocationType.Suburb),
            new Zone("MURRI", "Murrieta Heights", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Poor, eLocationType.Industrial),
            new Zone("EBURO", "El Burro Heights", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Poor, eLocationType.Industrial),

            //Vinewood
            new Zone("ALTA", "Alta", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("DTVINE", "Downtown Vinewood", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("EAST_V", "East Vinewood", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Suburb),
            new Zone("HAWICK", "Hawick", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("HORS", "Vinewood Racetrack", County.CityOfLosSantos, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("VINE", "Vinewood", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("WVINE", "West Vinewood", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Rich, eLocationType.Suburb),

            //PortOfLosSantos
            new Zone("ELYSIAN", "Elysian Island", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Poor, eLocationType.Industrial),
            new Zone("ZP_ORT", "Port of South Los Santos", County.CityOfLosSantos, "San Andreas", true, eLocationEconomy.Poor, eLocationType.Industrial),
            new Zone("TERMINA", "Terminal", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Poor, eLocationType.Industrial),
            new Zone("AIRP", "Los Santos International Airport", County.CityOfLosSantos, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Industrial) { IsRestrictedDuringWanted = true },

            //Rockford Hills
            new Zone("BURTON", "Burton", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("GOLF", "GWC and Golfing Society", County.CityOfLosSantos, "San Andreas", true, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("KOREAT", "Little Seoul", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("MORN", "Morningwood", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("MOVIE", "Richards Majestic", County.CityOfLosSantos, "San Andreas", true, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("RICHM", "Richman", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Rich, eLocationType.Suburb),
            new Zone("ROCKF", "Rockford Hills", County.CityOfLosSantos, "San Andreas", false, eLocationEconomy.Rich, eLocationType.Downtown),     

            //Vinewood Hills
            new Zone("CHIL", "Vinewood Hills", County.LosSantosCounty, "San Andreas", false, eLocationEconomy.Rich, eLocationType.Suburb),
            new Zone("GREATC", "Great Chaparral", County.LosSantosCounty, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("BAYTRE", "Baytree Canyon", County.LosSantosCounty, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("RGLEN", "Richman Glen", County.LosSantosCounty, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("TONGVAV", "Tongva Valley", County.LosSantosCounty, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),


           
            //Chumash
            new Zone("BANHAMC", "Banham Canyon Dr", County.LosSantosCounty, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("BHAMCA", "Banham Canyon", County.LosSantosCounty, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("CHU", "Chumash", County.LosSantosCounty, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("TONGVAH", "Tongva Hills", County.LosSantosCounty, "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),
           
            //Tataviam 
            new Zone("LACT", "Land Act Reservoir", County.LosSantosCounty, "San Andreas", true, eLocationEconomy.Poor, eLocationType.Rural),
            new Zone("LDAM", "Land Act Dam", County.LosSantosCounty, "San Andreas", true, eLocationEconomy.Poor, eLocationType.Rural),
            new Zone("NOOSE", "N.O.O.S.E", County.LosSantosCounty, "San Andreas", true, eLocationEconomy.Rich, eLocationType.Industrial) { IsRestrictedDuringWanted = true },
            new Zone("PALHIGH", "Palomino Highlands", County.LosSantosCounty, "San Andreas", false, eLocationEconomy.Poor, eLocationType.Rural),
            new Zone("PALMPOW", "Palmer - Taylor Power Station", County.LosSantosCounty, "San Andreas", true, eLocationEconomy.Poor, eLocationType.Rural),
            new Zone("SANAND", "San Andreas", County.LosSantosCounty, "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("TATAMO", "Tataviam Mountains", County.LosSantosCounty, "San Andreas", true, eLocationEconomy.Poor, eLocationType.Wilderness),
            new Zone("WINDF", "Ron Alternates Wind Farm", County.LosSantosCounty, "San Andreas", true, eLocationEconomy.Poor, eLocationType.Wilderness),



            //UNKNWON 
            new Zone("GALLI", "Galilee", County.BlaineCounty, "San Andreas", false, eLocationEconomy.Poor, eLocationType.Rural),

            //Other
            //new Zone("LUDEN", "Ludendorff", County.NorthYankton, new Vector2[] { new Vector2 { X = 2545.142f, Y = -5124.292f },
            //                            new Vector2 { X = 2648.361f, Y = -4091.664f },
            //                            new Vector2 { X = 5647.14f, Y = -4131.478f },
            //                            new Vector2 { X = 5922.999f, Y = -5640.681f } }, "North Yankton"),

            //new Zone("CHI1", "Acadia", County.Crook, new Vector2[] { new Vector2 { X = 4830.579f, Y = 1982.126f },
            //                            new Vector2 { X = 7898.494f, Y = 3093.242f },
            //                            new Vector2 { X = 5845.111f, Y = 8616.287f },
            //                            new Vector2 { X = 1748.942f, Y = 8188.261f } }, "Lincoln"),


            //new Zone("VICE", "Vice City", County.Vice, new Vector2[] { new Vector2 { X = 4669.141f, Y = -1614.298f },
            //                            new Vector2 { X = 4920.789f, Y = 2035.281f },
            //                            new Vector2 { X = 7549.999f, Y = 2008.153f },
            //                            new Vector2 { X = 7770.286f, Y = -1657.735f } }, "Florida")


            };
        Serialization.SerializeParams(ZoneList, ConfigFileName);
    }
    private void DefaultConfig_LibertyCity()
    {
        List<Zone> LibertyCityZones = new List<Zone>
        {
            new Zone("ACTRR", "Acter", County.Alderney, "Alderney", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("ALDCI", "Alderney State Correctional Facility", County.Alderney, "Alderney", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("ACTIP", "Acter Industrial Park", County.Alderney, "Alderney", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BERCH", "Berchem", County.Alderney, "Alderney", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BOAB", "BOAB", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BOULE", "Boulevard", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BRALG", "BRALG", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BRDBB", "BRDBB", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BREBB", "BREBB", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BRBRO", "BRBRO", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BEECW", "BEECW", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BOTU", "BOTU", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("CHITO", "CHITO", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("CITH", "CITH", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("COISL", "COISL", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("CHISL", "CHISL", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("CASGR", "CASGR", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("CHAPO", "CHAPO", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("CASGC", "CASGC", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("CERHE", "CERHE", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("DOWTW", "Downtown", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("EAHOL", "EAHOL", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("EISLC", "EISLC", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("FISSO", "FISSO", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("FRANI", "Francis International", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("FISSN", "FISSN", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("FIREP", "FIREP", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("FORSI", "FORSI", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("HATGA", "HATGA", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("HOBEH", "HOBEH", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("INSTI", "INSTI", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("LANCE", "LANCE", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("LEFWO", "Leftwood", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("LTBAY", "Little Bay", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("LANCA", "LANCA", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("LOWEA", "Lower Easton", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("LITAL", "LITAL", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("MIDPE", "MIDPE", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("MIDPA", "MIDPA", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("MIDPW", "MIDPW", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("MEADP", "Meadows Park", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("MEADH", "Meadow Hills", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("NOHOL", "NOHOL", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("NORWO", "NORWO", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("NRTGA", "NRTGA", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("NOWOB", "NOWOB", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("OCEANA", "OCEANA", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("OUTL", "OUTL", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("PUGAT", "PUGAT", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("PORTU", "Port Tudor", County.Alderney, "Alderney", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("SANAND", "SANAND", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("SUTHS", "SUTHS", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("SCHOL", "Schlotter", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("STARJ", "Star Junction", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("STEIN", "Steinway", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("STHBO", "South Bohan", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("SUFFO", "SUFFO", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("TUDOR", "Tudor", County.Alderney, "Alderney", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("THPRES", "THPRES", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("THXCH", "THXCH", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("THTRI", "THTRI", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("TMEQU", "TMEQU", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("VASIH", "VASIH", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("WESMI", "WESMI", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("WESDI", "WESDI", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("WESDY", "WESDY", County.LibertyCity, "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),


            };
        Serialization.SerializeParams(LibertyCityZones, "Plugins\\LosSantosRED\\AlternateConfigs\\LibertyCity\\Zones_LibertyCity.xml");
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
