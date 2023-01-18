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


        ListResult = ZoneList.Where(x => x.Boundaries != null && IsPointInPolygon(new Vector2(ZonePosition.X, ZonePosition.Y), x.Boundaries)).FirstOrDefault();


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
            new Zone("OCEANA", "Pacific Ocean", StaticStrings.PacificOceanCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Wilderness),

            ////Ventura County?
            //new Zone("PROCOB", "Procopio Beach", County.VenturaCounty, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("MTCHIL", "Mount Chiliad", County.VenturaCounty, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),    
            //new Zone("PALETO", "Paleto Bay", County.VenturaCounty, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Rural),
            //new Zone("PALCOV", "Paleto Cove", County.VenturaCounty, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("PALFOR", "Paleto Forest", County.VenturaCounty, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("CMSW", "Chiliad Mountain State Wilderness", County.VenturaCounty, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("CCREAK", "Cassidy Creek", County.VenturaCounty, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("CALAFB", "Calafia Bridge", County.VenturaCounty, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("GALFISH", "Galilee", County.VenturaCounty, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("BRADP", "Braddock Pass", County.VenturaCounty, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("BRADT", "Braddock Tunnel", County.VenturaCounty, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("ELGORL", "El Gordo Lighthouse", County.VenturaCounty, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),
            //new Zone("MTGORDO", "Mount Gordo", County.VenturaCounty, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),

            //Ventura County is now blaine county
            new Zone("PROCOB", "Procopio Beach", StaticStrings.BlaineCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness)
            //{
            //    DealerMenus = new ShopMenuGroupContainer("ProcopioDealerContainer","Procopio Dealer Container",
            //        new List<PercentageSelectGroupMenuContainer>() { 
            //            new PercentageSelectGroupMenuContainer(StaticStrings.SPANKDealerMenuGroupID,100), 
            //        }),
            //    CustomerMenus = new ShopMenuGroupContainer("ProcopioCustomerContainer","Procopio Customer Container",
            //        new List<PercentageSelectGroupMenuContainer>() {
            //            new PercentageSelectGroupMenuContainer(StaticStrings.SPANKCustomerMenuGroupID,100),
            //        }),
            //}
            ,
            new Zone("MTCHIL", "Mount Chiliad", StaticStrings.BlaineCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("PALETO", "Paleto Bay", StaticStrings.BlaineCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("PALCOV", "Paleto Cove", StaticStrings.BlaineCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("PALFOR", "Paleto Forest", StaticStrings.BlaineCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("CMSW", "Chiliad Mountain State Wilderness", StaticStrings.BlaineCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("CCREAK", "Cassidy Creek", StaticStrings.BlaineCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("CALAFB", "Calafia Bridge", StaticStrings.BlaineCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("GALFISH", "Galilee", StaticStrings.BlaineCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("BRADP", "Braddock Pass", StaticStrings.BlaineCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("BRADT", "Braddock Tunnel", StaticStrings.BlaineCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("ELGORL", "El Gordo Lighthouse", StaticStrings.BlaineCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("MTGORDO", "Mount Gordo", StaticStrings.BlaineCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),
            
            //Majestic County
            new Zone("GRAPES", "Grapeseed", StaticStrings.MajesticCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Rural),//has own PD

            new Zone("SANCHIA", "San Chianski Mountain Range", StaticStrings.MajesticCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("ALAMO", "Alamo Sea", StaticStrings.MajesticCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Poor, eLocationType.Wilderness),
            new Zone("DESRT", "Grand Senora Desert", StaticStrings.MajesticCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Poor, eLocationType.Rural),
            new Zone("SANDY", "Sandy Shores", StaticStrings.MajesticCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Rural),
            new Zone("HUMLAB", "Humane Labs and Research", StaticStrings.MajesticCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Rich, eLocationType.Industrial),
            new Zone("JAIL", "Bolingbroke Penitentiary", StaticStrings.MajesticCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Industrial) { IsRestrictedDuringWanted = true },
            new Zone("ZQ_UAR", "Davis Quartz", StaticStrings.MajesticCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Poor, eLocationType.Wilderness),
            new Zone("HARMO", "Harmony", StaticStrings.MajesticCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Rural),
            new Zone("RTRAK", "Redwood Lights Track", StaticStrings.MajesticCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Poor, eLocationType.Industrial),

            //Blaine
            new Zone("ARMYB", "Fort Zancudo", StaticStrings.BlaineCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Rich, eLocationType.Industrial),
            new Zone("CANNY", "Raton Canyon", StaticStrings.BlaineCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("LAGO", "Lago Zancudo", StaticStrings.BlaineCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("MTJOSE", "Mount Josiah", StaticStrings.BlaineCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("NCHU", "North Chumash", StaticStrings.BlaineCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("SLAB", "Slab City", StaticStrings.BlaineCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Rural),
            new Zone("ZANCUDO", "Zancudo River", StaticStrings.BlaineCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Poor, eLocationType.Wilderness),

            //Vespucci
            new Zone("BEACH", "Vespucci Beach", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("DELBE", "Del Perro Beach", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("DELPE", "Del Perro", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("VCANA", "Vespucci Canals", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("VESP", "Vespucci Metro", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("LOSPUER", "La Puerta", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("PBLUFF", "Pacific Bluffs", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Rich, eLocationType.Suburb),
            new Zone("DELSOL", "Puerto Del Sol", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),

            //Central
            new Zone("BANNING", "Banning", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("CHAMH", "Chamberlain Hills", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("DAVIS", "Davis", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("DOWNT", "Downtown", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("PBOX", "Pillbox Hill", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("RANCHO", "Rancho", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("SKID", "Mission Row", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("STAD", "Maze Bank Arena", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Industrial),
            new Zone("STRAW", "Strawberry", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("TEXTI", "Textile City", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("LEGSQU", "Legion Square", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Poor, eLocationType.Downtown),

            //East LS
            new Zone("CYPRE", "Cypress Flats", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Industrial),
            new Zone("LMESA", "La Mesa", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Industrial),
            new Zone("MIRR", "Mirror Park", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Suburb),
            new Zone("MURRI", "Murrieta Heights", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Industrial),
            new Zone("EBURO", "El Burro Heights", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Industrial),

            //Vinewood
            new Zone("ALTA", "Alta", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("DTVINE", "Downtown Vinewood", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("EAST_V", "East Vinewood", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Suburb),
            new Zone("HAWICK", "Hawick", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("HORS", "Vinewood Racetrack", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("VINE", "Vinewood", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("WVINE", "West Vinewood", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Rich, eLocationType.Suburb),

            //PortOfLosSantos
            new Zone("ELYSIAN", "Elysian Island", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Industrial),
            new Zone("ZP_ORT", "Port of South Los Santos", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Poor, eLocationType.Industrial),
            new Zone("TERMINA", "Terminal", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Industrial),
            new Zone("AIRP", "Los Santos International Airport", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Industrial) { IsRestrictedDuringWanted = true },

            //Rockford Hills
            new Zone("BURTON", "Burton", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("GOLF", "GWC and Golfing Society", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("KOREAT", "Little Seoul", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("MORN", "Morningwood", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("MOVIE", "Richards Majestic", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("RICHM", "Richman", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Rich, eLocationType.Suburb),
            new Zone("ROCKF", "Rockford Hills", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),     

            //Vinewood Hills
            new Zone("CHIL", "Vinewood Hills", StaticStrings.LosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Rich, eLocationType.Suburb),
            new Zone("GREATC", "Great Chaparral", StaticStrings.LosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("BAYTRE", "Baytree Canyon", StaticStrings.LosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("RGLEN", "Richman Glen", StaticStrings.LosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("TONGVAV", "Tongva Valley", StaticStrings.LosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Rural),


           
            //Chumash
            new Zone("BANHAMC", "Banham Canyon Dr", StaticStrings.LosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("BHAMCA", "Banham Canyon", StaticStrings.LosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("CHU", "Chumash", StaticStrings.LosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("TONGVAH", "Tongva Hills", StaticStrings.LosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Rural),
           
            //Tataviam 
            new Zone("LACT", "Land Act Reservoir", StaticStrings.LosSantosCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Poor, eLocationType.Rural),
            new Zone("LDAM", "Land Act Dam", StaticStrings.LosSantosCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Poor, eLocationType.Rural),
            new Zone("NOOSE", "N.O.O.S.E", StaticStrings.LosSantosCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Rich, eLocationType.Industrial) { IsRestrictedDuringWanted = true },
            new Zone("PALHIGH", "Palomino Highlands", StaticStrings.LosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Rural),
            new Zone("PALMPOW", "Palmer - Taylor Power Station", StaticStrings.LosSantosCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Poor, eLocationType.Rural),
            new Zone("SANAND", StaticStrings.SanAndreasStateID, StaticStrings.LosSantosCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),
            new Zone("TATAMO", "Tataviam Mountains", StaticStrings.LosSantosCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Poor, eLocationType.Wilderness),
            new Zone("WINDF", "Ron Alternates Wind Farm", StaticStrings.LosSantosCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Poor, eLocationType.Wilderness),



            //UNKNWON 
            new Zone("GALLI", "Galilee", StaticStrings.BlaineCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Rural),

            //Other
            new Zone("PROL", "Ludendorff", StaticStrings.NorthYanktonCountyID,StaticStrings.NorthYanktonStateID,false,eLocationEconomy.Middle,eLocationType.Rural),
            //new Zone("LUDEN", "Ludendorff", "NorthYankton", new Vector2[] { new Vector2 { X = 2545.142f, Y = -5124.292f },
            //                            new Vector2 { X = 2648.361f, Y = -4091.664f },
            //                            new Vector2 { X = 5647.14f, Y = -4131.478f },
            //                            new Vector2 { X = 5922.999f, Y = -5640.681f } },"North Yankton",false,eLocationEconomy.Middle,eLocationType.Rural),

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
            new Zone("ACTRR", "Acter", StaticStrings.AlderneyCountyID, StaticStrings.AlderneyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("ALDCI", "Alderney State Correctional Facility", StaticStrings.AlderneyCountyID, StaticStrings.AlderneyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("ACTIP", "Acter Industrial Park", StaticStrings.AlderneyCountyID, StaticStrings.AlderneyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BERCH", "Berchem", StaticStrings.AlderneyCountyID, StaticStrings.AlderneyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BOAB", "BOAB", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BOULE", "Boulevard", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BRALG", "BRALG", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BRDBB", "BRDBB", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BREBB", "BREBB", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BRBRO", "BRBRO", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BEECW", "BEECW", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BOTU", "BOTU", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("CHITO", "CHITO", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("CITH", "CITH", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("COISL", "COISL", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("CHISL", "CHISL", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("CASGR", "CASGR", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("CHAPO", "CHAPO", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("CASGC", "CASGC", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("CERHE", "CERHE", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("DOWTW", "Downtown", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("EAHOL", "EAHOL", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("EISLC", "EISLC", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("FISSO", "FISSO", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("FRANI", "Francis International", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("FISSN", "FISSN", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("FIREP", "FIREP", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("FORSI", "FORSI", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("HATGA", "HATGA", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("HOBEH", "HOBEH", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("INSTI", "INSTI", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("LANCE", "LANCE", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("LEFWO", "Leftwood", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("LTBAY", "Little Bay", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("LANCA", "LANCA", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("LOWEA", "Lower Easton", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("LITAL", "LITAL", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("MIDPE", "MIDPE", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("MIDPA", "MIDPA", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("MIDPW", "MIDPW", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("MEADP", "Meadows Park", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("MEADH", "Meadow Hills", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("NOHOL", "NOHOL", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("NORWO", "NORWO", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("NRTGA", "NRTGA", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("NOWOB", "NOWOB", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("OCEANA", "OCEANA", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("OUTL", "OUTL", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("PUGAT", "PUGAT", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("PORTU", "Port Tudor", StaticStrings.AlderneyCountyID, StaticStrings.AlderneyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("SANAND", "SANAND", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("SUTHS", "SUTHS", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("SCHOL", "Schlotter", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("STARJ", "Star Junction", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("STEIN", "Steinway", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("STHBO", "South Bohan", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("SUFFO", "SUFFO", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("TUDOR", "Tudor", StaticStrings.AlderneyCountyID, StaticStrings.AlderneyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("THPRES", "THPRES", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("THXCH", "THXCH", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("THTRI", "THTRI", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("TMEQU", "TMEQU", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("VASIH", "VASIH", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("WESMI", "WESMI", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("WESDI", "WESDI", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("WESDY", "WESDY", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),


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
