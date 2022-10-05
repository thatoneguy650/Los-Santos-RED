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
            return new Zone(zoneName, "Unknown", "Unknown", "Unknown", false, eLocationEconomy.Middle, eLocationType.Rural);
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
            new Zone("OCEANA", "Pacific Ocean", "PacificOcean", "Unknown", false, eLocationEconomy.Poor, eLocationType.Wilderness),

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
            new Zone("PROCOB", "Procopio Beach", "BlaineCounty", "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("MTCHIL", "Mount Chiliad", "BlaineCounty", "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("PALETO", "Paleto Bay", "BlaineCounty", "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("PALCOV", "Paleto Cove", "BlaineCounty", "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("PALFOR", "Paleto Forest", "BlaineCounty", "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("CMSW", "Chiliad Mountain State Wilderness", "BlaineCounty", "San Andreas", false, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("CCREAK", "Cassidy Creek", "BlaineCounty", "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("CALAFB", "Calafia Bridge", "BlaineCounty", "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("GALFISH", "Galilee", "BlaineCounty", "San Andreas", false, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("BRADP", "Braddock Pass", "BlaineCounty", "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("BRADT", "Braddock Tunnel", "BlaineCounty", "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("ELGORL", "El Gordo Lighthouse", "BlaineCounty", "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("MTGORDO", "Mount Gordo", "BlaineCounty", "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            
            //Majestic County
            new Zone("GRAPES", "Grapeseed", "MajesticCounty", "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),//has own PD

            new Zone("SANCHIA", "San Chianski Mountain Range", "MajesticCounty", "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("ALAMO", "Alamo Sea", "MajesticCounty", "San Andreas", true, eLocationEconomy.Poor, eLocationType.Wilderness),
            new Zone("DESRT", "Grand Senora Desert", "MajesticCounty", "San Andreas", true, eLocationEconomy.Poor, eLocationType.Rural),
            new Zone("SANDY", "Sandy Shores", "MajesticCounty", "San Andreas", false, eLocationEconomy.Poor, eLocationType.Rural),
            new Zone("HUMLAB", "Humane Labs and Research", "MajesticCounty", "San Andreas", true, eLocationEconomy.Rich, eLocationType.Industrial),
            new Zone("JAIL", "Bolingbroke Penitentiary", "MajesticCounty", "San Andreas", true, eLocationEconomy.Middle, eLocationType.Industrial) { IsRestrictedDuringWanted = true },
            new Zone("ZQ_UAR", "Davis Quartz", "MajesticCounty", "San Andreas", true, eLocationEconomy.Poor, eLocationType.Wilderness),
            new Zone("HARMO", "Harmony", "MajesticCounty", "San Andreas", false, eLocationEconomy.Poor, eLocationType.Rural),
            new Zone("RTRAK", "Redwood Lights Track", "MajesticCounty", "San Andreas", true, eLocationEconomy.Poor, eLocationType.Industrial),

            //Blaine
            new Zone("ARMYB", "Fort Zancudo", "BlaineCounty", "San Andreas", true, eLocationEconomy.Rich, eLocationType.Industrial),
            new Zone("CANNY", "Raton Canyon", "BlaineCounty", "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("LAGO", "Lago Zancudo", "BlaineCounty", "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("MTJOSE", "Mount Josiah", "BlaineCounty", "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("NCHU", "North Chumash", "BlaineCounty", "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("SLAB", "Slab City", "BlaineCounty", "San Andreas", false, eLocationEconomy.Poor, eLocationType.Rural),
            new Zone("ZANCUDO", "Zancudo River", "BlaineCounty", "San Andreas", true, eLocationEconomy.Poor, eLocationType.Wilderness),

            //Vespucci
            new Zone("BEACH", "Vespucci Beach", "CityOfLosSantos", "San Andreas", true, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("DELBE", "Del Perro Beach", "CityOfLosSantos", "San Andreas", true, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("DELPE", "Del Perro", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("VCANA", "Vespucci Canals", "CityOfLosSantos", "San Andreas", true, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("VESP", "Vespucci Metro", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("LOSPUER", "La Puerta", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("PBLUFF", "Pacific Bluffs", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Rich, eLocationType.Suburb),
            new Zone("DELSOL", "Puerto Del Sol", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Rich, eLocationType.Downtown),

            //Central
            new Zone("BANNING", "Banning", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("CHAMH", "Chamberlain Hills", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("DAVIS", "Davis", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("DOWNT", "Downtown", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("PBOX", "Pillbox Hill", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("RANCHO", "Rancho", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("SKID", "Mission Row", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("STAD", "Maze Bank Arena", "CityOfLosSantos", "San Andreas", true, eLocationEconomy.Middle, eLocationType.Industrial),
            new Zone("STRAW", "Strawberry", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("TEXTI", "Textile City", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("LEGSQU", "Legion Square", "CityOfLosSantos", "San Andreas", true, eLocationEconomy.Poor, eLocationType.Downtown),

            //East LS
            new Zone("CYPRE", "Cypress Flats", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Poor, eLocationType.Industrial),
            new Zone("LMESA", "La Mesa", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Poor, eLocationType.Industrial),
            new Zone("MIRR", "Mirror Park", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Poor, eLocationType.Suburb),
            new Zone("MURRI", "Murrieta Heights", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Poor, eLocationType.Industrial),
            new Zone("EBURO", "El Burro Heights", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Poor, eLocationType.Industrial),

            //Vinewood
            new Zone("ALTA", "Alta", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("DTVINE", "Downtown Vinewood", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("EAST_V", "East Vinewood", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Middle, eLocationType.Suburb),
            new Zone("HAWICK", "Hawick", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("HORS", "Vinewood Racetrack", "CityOfLosSantos", "San Andreas", true, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("VINE", "Vinewood", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("WVINE", "West Vinewood", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Rich, eLocationType.Suburb),

            //PortOfLosSantos
            new Zone("ELYSIAN", "Elysian Island", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Poor, eLocationType.Industrial),
            new Zone("ZP_ORT", "Port of South Los Santos", "CityOfLosSantos", "San Andreas", true, eLocationEconomy.Poor, eLocationType.Industrial),
            new Zone("TERMINA", "Terminal", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Poor, eLocationType.Industrial),
            new Zone("AIRP", "Los Santos International Airport", "CityOfLosSantos", "San Andreas", true, eLocationEconomy.Middle, eLocationType.Industrial) { IsRestrictedDuringWanted = true },

            //Rockford Hills
            new Zone("BURTON", "Burton", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("GOLF", "GWC and Golfing Society", "CityOfLosSantos", "San Andreas", true, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("KOREAT", "Little Seoul", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("MORN", "Morningwood", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("MOVIE", "Richards Majestic", "CityOfLosSantos", "San Andreas", true, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("RICHM", "Richman", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Rich, eLocationType.Suburb),
            new Zone("ROCKF", "Rockford Hills", "CityOfLosSantos", "San Andreas", false, eLocationEconomy.Rich, eLocationType.Downtown),     

            //Vinewood Hills
            new Zone("CHIL", "Vinewood Hills", "LosSantosCounty", "San Andreas", false, eLocationEconomy.Rich, eLocationType.Suburb),
            new Zone("GREATC", "Great Chaparral", "LosSantosCounty", "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("BAYTRE", "Baytree Canyon", "LosSantosCounty", "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("RGLEN", "Richman Glen", "LosSantosCounty", "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("TONGVAV", "Tongva Valley", "LosSantosCounty", "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),


           
            //Chumash
            new Zone("BANHAMC", "Banham Canyon Dr", "LosSantosCounty", "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("BHAMCA", "Banham Canyon", "LosSantosCounty", "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("CHU", "Chumash", "LosSantosCounty", "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("TONGVAH", "Tongva Hills", "LosSantosCounty", "San Andreas", false, eLocationEconomy.Middle, eLocationType.Rural),
           
            //Tataviam 
            new Zone("LACT", "Land Act Reservoir", "LosSantosCounty", "San Andreas", true, eLocationEconomy.Poor, eLocationType.Rural),
            new Zone("LDAM", "Land Act Dam", "LosSantosCounty", "San Andreas", true, eLocationEconomy.Poor, eLocationType.Rural),
            new Zone("NOOSE", "N.O.O.S.E", "LosSantosCounty", "San Andreas", true, eLocationEconomy.Rich, eLocationType.Industrial) { IsRestrictedDuringWanted = true },
            new Zone("PALHIGH", "Palomino Highlands", "LosSantosCounty", "San Andreas", false, eLocationEconomy.Poor, eLocationType.Rural),
            new Zone("PALMPOW", "Palmer - Taylor Power Station", "LosSantosCounty", "San Andreas", true, eLocationEconomy.Poor, eLocationType.Rural),
            new Zone("SANAND", "San Andreas", "LosSantosCounty", "San Andreas", true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("TATAMO", "Tataviam Mountains", "LosSantosCounty", "San Andreas", true, eLocationEconomy.Poor, eLocationType.Wilderness),
            new Zone("WINDF", "Ron Alternates Wind Farm", "LosSantosCounty", "San Andreas", true, eLocationEconomy.Poor, eLocationType.Wilderness),



            //UNKNWON 
            new Zone("GALLI", "Galilee", "BlaineCounty", "San Andreas", false, eLocationEconomy.Poor, eLocationType.Rural),

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
            new Zone("ACTRR", "Acter", "Alderney", "Alderney", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("ALDCI", "Alderney State Correctional Facility", "Alderney", "Alderney", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("ACTIP", "Acter Industrial Park", "Alderney", "Alderney", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BERCH", "Berchem", "Alderney", "Alderney", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BOAB", "BOAB", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BOULE", "Boulevard", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BRALG", "BRALG", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BRDBB", "BRDBB", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BREBB", "BREBB", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BRBRO", "BRBRO", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BEECW", "BEECW", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BOTU", "BOTU", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("CHITO", "CHITO", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("CITH", "CITH", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("COISL", "COISL", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("CHISL", "CHISL", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("CASGR", "CASGR", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("CHAPO", "CHAPO", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("CASGC", "CASGC", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("CERHE", "CERHE", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("DOWTW", "Downtown", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("EAHOL", "EAHOL", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("EISLC", "EISLC", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("FISSO", "FISSO", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("FRANI", "Francis International", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("FISSN", "FISSN", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("FIREP", "FIREP", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("FORSI", "FORSI", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("HATGA", "HATGA", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("HOBEH", "HOBEH", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("INSTI", "INSTI", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("LANCE", "LANCE", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("LEFWO", "Leftwood", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("LTBAY", "Little Bay", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("LANCA", "LANCA", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("LOWEA", "Lower Easton", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("LITAL", "LITAL", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("MIDPE", "MIDPE", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("MIDPA", "MIDPA", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("MIDPW", "MIDPW", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("MEADP", "Meadows Park", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("MEADH", "Meadow Hills", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("NOHOL", "NOHOL", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("NORWO", "NORWO", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("NRTGA", "NRTGA", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("NOWOB", "NOWOB", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("OCEANA", "OCEANA", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("OUTL", "OUTL", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("PUGAT", "PUGAT", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("PORTU", "Port Tudor", "Alderney", "Alderney", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("SANAND", "SANAND", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("SUTHS", "SUTHS", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("SCHOL", "Schlotter", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("STARJ", "Star Junction", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("STEIN", "Steinway", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("STHBO", "South Bohan", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("SUFFO", "SUFFO", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("TUDOR", "Tudor", "Alderney", "Alderney", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("THPRES", "THPRES", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("THXCH", "THXCH", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("THTRI", "THTRI", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("TMEQU", "TMEQU", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("VASIH", "VASIH", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("WESMI", "WESMI", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("WESDI", "WESDI", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("WESDY", "WESDY", "LibertyCity", "Liberty", false, eLocationEconomy.Middle, eLocationType.Downtown),


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
