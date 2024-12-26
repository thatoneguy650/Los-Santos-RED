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
            DefaultConfig_SunshineDream();
        }
    }



    public Zone GetZone(Vector3 ZonePosition)
    {
        Zone ListResult = null;
        string zoneName = "UNK";


        ListResult = ZoneList.Where(x => x.Boundaries != null && NativeHelper.IsPointInPolygon(new Vector2(ZonePosition.X, ZonePosition.Y), x.Boundaries)).FirstOrDefault();


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
    public List<Zone> GetZoneByItem(ModItem modItem, IShopMenus shopMenus, bool isPurchase)
    {
        List<Zone> MatchingZones = new List<Zone>();
        foreach(Zone zone in ZoneList)
        {
            if(isPurchase)
            {
                if (zone.DealerMenus != null && zone.DealerMenus.HasItem(modItem, shopMenus))
                {
                    MatchingZones.Add(zone);    
                }
            }
            else
            {
                if (zone.CustomerMenus != null && zone.CustomerMenus.HasItem(modItem, shopMenus))
                {
                    MatchingZones.Add(zone);
                }
            }
        }
        return MatchingZones;
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

            //Ventura County is now blaine county
            new Zone("PROCOB", "Procopio Beach", StaticStrings.BlaineCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Wilderness),
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
            new Zone("SLAB", "Stab City", StaticStrings.BlaineCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Rural) { DealerMenuContainerID = StaticStrings.ToiletCleanerAreaDrugDealerMenuGroupID, CustomerMenuContainerID = StaticStrings.ToiletCleanerAreaDrugCustomerMenuGroupID },
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
            new Zone("BANNING", "Banning", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Downtown) { DealerMenuContainerID = StaticStrings.CrackAreaDrugDealerMenuGroupID, CustomerMenuContainerID = StaticStrings.CrackAreaDrugCustomerMenuGroupID },
            new Zone("CHAMH", "Chamberlain Hills", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Downtown) { DealerMenuContainerID = StaticStrings.MarijuanaAreaDrugDealerMenuGroupID, CustomerMenuContainerID = StaticStrings.MarijuanaAreaDrugCustomerMenuGroupID },
            new Zone("DAVIS", "Davis", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Downtown) { DealerMenuContainerID = StaticStrings.CrackAreaDrugDealerMenuGroupID, CustomerMenuContainerID = StaticStrings.CrackAreaDrugCustomerMenuGroupID },
            new Zone("DOWNT", "Downtown", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("PBOX", "Pillbox Hill", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Downtown){ DealerMenuContainerID = StaticStrings.HeroinAreaDrugDealerMenuGroupID, CustomerMenuContainerID = StaticStrings.HeroinAreaDrugCustomerMenuGroupID } ,
            new Zone("RANCHO", "Rancho", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Downtown) { DealerMenuContainerID = StaticStrings.CrackAreaDrugDealerMenuGroupID, CustomerMenuContainerID = StaticStrings.CrackAreaDrugCustomerMenuGroupID },
            new Zone("SKID", "Mission Row", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Downtown){ DealerMenuContainerID = StaticStrings.HeroinAreaDrugDealerMenuGroupID, CustomerMenuContainerID = StaticStrings.HeroinAreaDrugCustomerMenuGroupID } ,
            new Zone("STAD", "Maze Bank Arena", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Industrial) { DealerMenuContainerID = StaticStrings.CrackAreaDrugDealerMenuGroupID, CustomerMenuContainerID = StaticStrings.CrackAreaDrugCustomerMenuGroupID },
            new Zone("STRAW", "Strawberry", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Downtown){ DealerMenuContainerID = StaticStrings.MarijuanaAreaDrugDealerMenuGroupID, CustomerMenuContainerID = StaticStrings.MarijuanaAreaDrugCustomerMenuGroupID },
            new Zone("TEXTI", "Textile City", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Downtown){ DealerMenuContainerID = StaticStrings.HeroinAreaDrugDealerMenuGroupID, CustomerMenuContainerID = StaticStrings.HeroinAreaDrugCustomerMenuGroupID } ,
            new Zone("LEGSQU", "Legion Square", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Poor, eLocationType.Downtown){ DealerMenuContainerID = StaticStrings.HeroinAreaDrugDealerMenuGroupID, CustomerMenuContainerID = StaticStrings.HeroinAreaDrugCustomerMenuGroupID } ,

            //East LS
            new Zone("CYPRE", "Cypress Flats", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Industrial){ DealerMenuContainerID = StaticStrings.MarijuanaAreaDrugDealerMenuGroupID, CustomerMenuContainerID = StaticStrings.MarijuanaAreaDrugCustomerMenuGroupID },
            new Zone("LMESA", "La Mesa", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Industrial) { DealerMenuContainerID = StaticStrings.MethamphetamineAreaDrugDealerMenuGroupID, CustomerMenuContainerID = StaticStrings.MethamphetamineAreaDrugCustomerMenuGroupID },
            new Zone("MIRR", "Mirror Park", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Suburb) { DealerMenuContainerID = StaticStrings.MethamphetamineAreaDrugDealerMenuGroupID, CustomerMenuContainerID = StaticStrings.MethamphetamineAreaDrugCustomerMenuGroupID },
            new Zone("MURRI", "Murrieta Heights", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Industrial){ DealerMenuContainerID = StaticStrings.MarijuanaAreaDrugDealerMenuGroupID, CustomerMenuContainerID = StaticStrings.MarijuanaAreaDrugCustomerMenuGroupID },
            new Zone("EBURO", "El Burro Heights", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Industrial){ DealerMenuContainerID = StaticStrings.MarijuanaAreaDrugDealerMenuGroupID, CustomerMenuContainerID = StaticStrings.MarijuanaAreaDrugCustomerMenuGroupID },

            //Vinewood
            new Zone("ALTA", "Alta", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("DTVINE", "Downtown Vinewood", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Rich, eLocationType.Downtown){ DealerMenuContainerID = StaticStrings.CokeAreaDrugDealerMenuGroupID, CustomerMenuContainerID = StaticStrings.CokeAreaDrugCustomerMenuGroupID },
            new Zone("EAST_V", "East Vinewood", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Suburb) { DealerMenuContainerID = StaticStrings.MethamphetamineAreaDrugDealerMenuGroupID, CustomerMenuContainerID = StaticStrings.MethamphetamineAreaDrugCustomerMenuGroupID },
            new Zone("HAWICK", "Hawick", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Rich, eLocationType.Downtown){ DealerMenuContainerID = StaticStrings.CokeAreaDrugDealerMenuGroupID, CustomerMenuContainerID = StaticStrings.CokeAreaDrugCustomerMenuGroupID },
            new Zone("HORS", "Vinewood Racetrack", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("VINE", "Vinewood", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Downtown){ DealerMenuContainerID = StaticStrings.CokeAreaDrugDealerMenuGroupID, CustomerMenuContainerID = StaticStrings.CokeAreaDrugCustomerMenuGroupID },
            new Zone("WVINE", "West Vinewood", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Rich, eLocationType.Suburb),

            //PortOfLosSantos
            new Zone("ELYSIAN", "Elysian Island", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Industrial) { DealerMenuContainerID = StaticStrings.SPANKAreaDrugDealerMenuGroupID, CustomerMenuContainerID = StaticStrings.SPANKAreaDrugCustomerMenuGroupID },
            new Zone("ZP_ORT", "Port of South Los Santos", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Poor, eLocationType.Industrial){ DealerMenuContainerID = StaticStrings.SPANKAreaDrugDealerMenuGroupID, CustomerMenuContainerID = StaticStrings.SPANKAreaDrugCustomerMenuGroupID },
            new Zone("TERMINA", "Terminal", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Poor, eLocationType.Industrial){ DealerMenuContainerID = StaticStrings.SPANKAreaDrugDealerMenuGroupID, CustomerMenuContainerID = StaticStrings.SPANKAreaDrugCustomerMenuGroupID },
            new Zone("AIRP", "Los Santos International Airport", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Middle, eLocationType.Industrial) { IsRestrictedDuringWanted = true },

            //Rockford Hills
            new Zone("BURTON", "Burton", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("GOLF", "GWC and Golfing Society", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, true, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("KOREAT", "Little Seoul", StaticStrings.CityOfLosSantosCountyID, StaticStrings.SanAndreasStateID, false, eLocationEconomy.Middle, eLocationType.Downtown) { DealerMenuContainerID = StaticStrings.HeroinAreaDrugDealerMenuGroupID, CustomerMenuContainerID = StaticStrings.HeroinAreaDrugCustomerMenuGroupID } ,
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


            new Zone("IsHeist", "Cayo Perico", StaticStrings.ColombiaStateID,StaticStrings.ColombiaStateID,false,eLocationEconomy.Middle,eLocationType.Rural),


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
            //Algonquin UpTown
            new Zone("EAHOL", "East Holland", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Rich, eLocationType.Downtown) { BoroughName = "Algonquin" },
            new Zone("LANCA", "Lancaster", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Rich, eLocationType.Downtown) { BoroughName = "Algonquin" },
            new Zone("MIDPE", "Middle Park East", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Rich, eLocationType.Downtown) { BoroughName = "Algonquin" },
            new Zone("MIDPA", "Middle Park", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, true, eLocationEconomy.Rich, eLocationType.Downtown) { BoroughName = "Algonquin" },
            new Zone("MIDPW", "Middle Park West", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Rich, eLocationType.Downtown) { BoroughName = "Algonquin" },
            new Zone("NOHOL", "North Holland", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Rich, eLocationType.Downtown) { BoroughName = "Algonquin" },
            new Zone("NORWO", "Northwood", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Rich, eLocationType.Downtown) { BoroughName = "Algonquin" },
            new Zone("VASIH", "Varsity Heights", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Rich, eLocationType.Downtown) { BoroughName = "Algonquin" },

            //Algonquin MidTown
            new Zone("COISL", "Colony Island", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, true, eLocationEconomy.Middle, eLocationType.Rural) { BoroughName = "Algonquin" },
            new Zone("EASON", "Easton", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown) { BoroughName = "Algonquin" },
            new Zone("HATGA", "Hatton Gardens", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown) { BoroughName = "Algonquin" },
            new Zone("LANCE", "Lancet", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown) { BoroughName = "Algonquin" },
            new Zone("TMEQU", "The Meat Quarter", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown) { BoroughName = "Algonquin" },
            new Zone("THPRES", "Presidents City", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown) { BoroughName = "Algonquin" },
            new Zone("PUGAT", "Purgatory", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown) { BoroughName = "Algonquin" },
            new Zone("STARJ", "Star Junction", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, true, eLocationEconomy.Middle, eLocationType.Downtown) { BoroughName = "Algonquin" },
            new Zone("THTRI", "The Triangle", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Downtown) { BoroughName = "Algonquin" },

            //Algonquin Downtown
            new Zone("CASGC", "Castle Garden City", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Rich, eLocationType.Downtown) { BoroughName = "Algonquin" },
            new Zone("CASGR", "Castle Gardens", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Rich, eLocationType.Downtown) { BoroughName = "Algonquin" },
            new Zone("CHITO", "Chinatown", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Rich, eLocationType.Downtown) { BoroughName = "Algonquin" },
            new Zone("CITH", "City Hall", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Rich, eLocationType.Downtown) { BoroughName = "Algonquin" },
            new Zone("THXCH", "The Exchange", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Rich, eLocationType.Downtown) { BoroughName = "Algonquin" },
            new Zone("FISSO", "Fishmarket South", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Rich, eLocationType.Downtown) { BoroughName = "Algonquin" },
            new Zone("FISSN", "Fishmarket North", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Rich, eLocationType.Downtown) { BoroughName = "Algonquin" },
            new Zone("HAPIN", "Happiness Island", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, true, eLocationEconomy.Rich, eLocationType.Rural) { BoroughName = "Algonquin" },
            new Zone("LITAL", "Little Italy", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Rich, eLocationType.Downtown) { BoroughName = "Algonquin" },
            new Zone("LOWEA", "Lower Easton", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Rich, eLocationType.Downtown) { BoroughName = "Algonquin" },
            new Zone("SUFFO", "Suffolk", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Rich, eLocationType.Downtown) { BoroughName = "Algonquin" },
            new Zone("WESMI", "Westminster", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Rich, eLocationType.Downtown) { BoroughName = "Algonquin" },

            //Broker      
            new Zone("BEGGA", "Beachgate", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Poor, eLocationType.Suburb) { BoroughName = "Broker" },
            new Zone("BEECW", "Beechwood City", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Poor, eLocationType.Suburb) { BoroughName = "Broker" },
            new Zone("BOAB", "BOABO", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, true, eLocationEconomy.Poor, eLocationType.Suburb) { BoroughName = "Broker" },
            new Zone("DOWTW", "Downtown", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Poor, eLocationType.Suburb) { BoroughName = "Broker" },
            new Zone("ESHOO", "East Hook", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Poor, eLocationType.Suburb) { BoroughName = "Broker" },
            new Zone("FIISL", "Firefly Island", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Rural) { BoroughName = "Broker" },
            new Zone("FIREP", "Firefly Projects", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Poor, eLocationType.Suburb) { BoroughName = "Broker" },
            new Zone("HOBEH", "Hove Beach", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Poor, eLocationType.Suburb) { BoroughName = "Broker" },
            new Zone("OUTL", "Outlook", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Poor, eLocationType.Suburb) { BoroughName = "Broker" },
            new Zone("ROTTH", "Rotterdam Hill", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Poor, eLocationType.Suburb) { BoroughName = "Broker" },
            new Zone("SCHOL", "Schlotter", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Poor, eLocationType.Suburb) { BoroughName = "Broker" },
            new Zone("SUTHS", "South Slopes", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Poor, eLocationType.Suburb) { BoroughName = "Broker" },

            //Dukes
            new Zone("CERHE", "Cerveza Heights", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Suburb) { BoroughName = "Dukes" },
            new Zone("CHISL", "Charge Island", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Suburb) { BoroughName = "Dukes" },
            new Zone("EISLC", "East Island City", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Suburb) { BoroughName = "Dukes" },
            new Zone("FRANI", "Francis International Airport", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, true, eLocationEconomy.Middle, eLocationType.Industrial) { BoroughName = "Dukes" },
            new Zone("MEADP", "Meadows Park", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Suburb) { BoroughName = "Dukes" },
            new Zone("MEADH", "Meadow Hills", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Suburb) { BoroughName = "Dukes" },
            new Zone("STEIN", "Steinway", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Suburb) { BoroughName = "Dukes" },
            new Zone("WILLI", "Willis", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Suburb) { BoroughName = "Dukes" },

            //Bohan
            new Zone("BOULE", "Boulevard", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Poor, eLocationType.Suburb) { BoroughName = "Bohan" },
            new Zone("CHAPO", "Chase Point", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Poor, eLocationType.Suburb) { BoroughName = "Bohan" },
            new Zone("FORSI", "Fortside", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Poor, eLocationType.Suburb) { BoroughName = "Bohan" },
            new Zone("INSTI", "Industrial", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Poor, eLocationType.Industrial) { BoroughName = "Bohan" },
            new Zone("LTBAY", "Little Bay", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Poor, eLocationType.Suburb) { BoroughName = "Bohan" },
            new Zone("NRTGA", "Northern Gardens", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Poor, eLocationType.Suburb) { BoroughName = "Bohan" },
            new Zone("STHBO", "South Bohan", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Poor, eLocationType.Suburb) { BoroughName = "Bohan" },

            //Alderney
            
            new Zone("ALSCF", "Alderney State Correctional Facility", StaticStrings.AlderneyCountyID, StaticStrings.AlderneyStateID, true, eLocationEconomy.Poor, eLocationType.Industrial),
            new Zone("ACTRR", "Acter", StaticStrings.AlderneyCountyID, StaticStrings.AlderneyStateID, false, eLocationEconomy.Poor, eLocationType.Suburb),
            new Zone("ALDCI", "Alderney City", StaticStrings.AlderneyCountyID, StaticStrings.AlderneyStateID, false, eLocationEconomy.Poor, eLocationType.Downtown),
            new Zone("ACTIP", "Acter Industrial Park", StaticStrings.AlderneyCountyID, StaticStrings.AlderneyStateID, false, eLocationEconomy.Poor, eLocationType.Industrial),
            new Zone("BERCH", "Berchem", StaticStrings.AlderneyCountyID, StaticStrings.AlderneyStateID, false, eLocationEconomy.Rich, eLocationType.Suburb),
            new Zone("LEFWO", "Leftwood", StaticStrings.AlderneyCountyID, StaticStrings.AlderneyStateID, false, eLocationEconomy.Middle, eLocationType.Suburb),
            new Zone("NORMY", "Normandy", StaticStrings.AlderneyCountyID, StaticStrings.AlderneyStateID, false, eLocationEconomy.Poor, eLocationType.Industrial),
            new Zone("PORTU", "Port Tudor", StaticStrings.AlderneyCountyID, StaticStrings.AlderneyStateID, false, eLocationEconomy.Poor, eLocationType.Industrial),
            new Zone("TUDOR", "Tudor", StaticStrings.AlderneyCountyID, StaticStrings.AlderneyStateID, false, eLocationEconomy.Middle, eLocationType.Suburb),
            new Zone("WESDY", "Westdyke", StaticStrings.AlderneyCountyID, StaticStrings.AlderneyStateID, false, eLocationEconomy.Rich, eLocationType.Suburb),

            //Bridges and Tunnels      
            new Zone("BRALG", "Algonquin Bridge", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, true, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BRDBB", "Dukes Bay Bridge", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, true, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BREBB", "East Borough Bridge", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, true, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BRBRO", "Broker Bridge", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, true, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("BOTU", "Booth Tunnel", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, true, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("HIBRG", "Hickey Bridge", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, true, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("NOWOB", "Northwood Heights Bridge", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, true, eLocationEconomy.Middle, eLocationType.Downtown),
            new Zone("LEAPE", "Leaper's Bridge", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, true, eLocationEconomy.Middle, eLocationType.Downtown),
            //LEAPE
            //Other
            new Zone("OCEANA", "Atlantic Ocean", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("SANAND", "Outskirts", StaticStrings.LibertyCityCountyID, StaticStrings.LibertyStateID, false, eLocationEconomy.Middle, eLocationType.Rural),
            };
        Serialization.SerializeParams(LibertyCityZones, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LibertyConfigFolder}\\Zones_{StaticStrings.LibertyConfigSuffix}.xml");

        List<Zone> LPPZones = new List<Zone>();
        LPPZones.AddRange(LibertyCityZones);
        LPPZones.AddRange(ZoneList);
        Serialization.SerializeParams(LPPZones, $"Plugins\\LosSantosRED\\AlternateConfigs\\{StaticStrings.LPPConfigFolder}\\Zones_{StaticStrings.LPPConfigSuffix}.xml");
    }

    private void DefaultConfig_SunshineDream()
    {
        List<Zone> MiamiDriver3Zones = new List<Zone>
        {
            //original
            new Zone("MFLATO", "Atlantic Ocean", StaticStrings.ViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFLBSB", "Biscayne Bay", StaticStrings.ViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("MFLMIA", "Vice", StaticStrings.CityOfViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("MFLMIB", "Vice Beach", StaticStrings.CityOfViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFLSPB", "South Point Beach", StaticStrings.CityOfViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFLADD", "Art Deco District", StaticStrings.CityOfViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFLBKL", "Brickell", StaticStrings.CityOfViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFLCCG", "Coconut Grove", StaticStrings.CityOfViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFLCRW", "Douglas", StaticStrings.CityOfViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFLCRG", "Coral Gables", StaticStrings.CityOfViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFLMB", "Mid Beach", StaticStrings.CityOfViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("MFLCC", "City Center", StaticStrings.CityOfViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFLFMG", "Flamingo", StaticStrings.CityOfViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFLBSH", "Leaf Links", StaticStrings.CityOfViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),//"Bayshore"
            //new Zone("MFLSB", "South Beach", StaticStrings.CityOfViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFLBI", "Belle Isle", StaticStrings.ViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFLSI", "Star Island", StaticStrings.ViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFLWI", "Watson Island", StaticStrings.ViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFLDI", "Port of Vice", StaticStrings.ViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFSMRI", "San Marco Island", StaticStrings.ViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFLDLI", "Di-Lido Island", StaticStrings.ViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFLSMI", "San Marino Island", StaticStrings.ViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            new Zone("MFLFSI", "Fisher Island", StaticStrings.ViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFLFMP", "Flamingo Park", StaticStrings.ViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFLSPP", "South Point Park", StaticStrings.ViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFDMIA", "Downtown Vice", StaticStrings.CityOfViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFOMNI", "Omni", StaticStrings.ViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFLPKW", "Park West", StaticStrings.ViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFOTWN", "Overtown", StaticStrings.CityOfViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFLUMP", "Riverside", StaticStrings.CityOfViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFLSPG", "Spring Garden", StaticStrings.CityOfViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFLHVA", "Little Havana", StaticStrings.CityOfViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            //new Zone("MFLFMI", "Flagler Memorial Island", StaticStrings.ViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Rich, eLocationType.Downtown),
            ////Other
            //new Zone("OCEANA", "Atlantic Ocean", StaticStrings.ViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Middle, eLocationType.Rural),
            new Zone("SANAND", "Outskirts", StaticStrings.ViceCountyID, StaticStrings.LeonidaStateID, false, eLocationEconomy.Middle, eLocationType.Rural),



            //from configs
            new Zone() {
            InternalGameName = "OCEANA",
            DisplayName = "Atlantic Ocean",
            CountyID = "PacificOcean",
            StateID = StaticStrings.LeonidaStateID,
            Economy = eLocationEconomy.Poor,
            Type = eLocationType.Wilderness,
            },
            new Zone() {
            InternalGameName = "MFLBSH",
            DisplayName = "Leaf Links",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            Type = eLocationType.Suburb,
            },
            new Zone() {
            InternalGameName = "MFLMB",
            DisplayName = "Washington Beach",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            IsSpecificLocation = true,
            Type = eLocationType.Downtown,
            DealerMenuContainerID = "CokeAreaDrugDealerMenuGroupID",
            CustomerMenuContainerID = "CokeAreaDrugCustomerMenuGroupID",
            },
            new Zone() {
            InternalGameName = "MFLFMG",
            DisplayName = "Washington",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            IsSpecificLocation = true,
            Type = eLocationType.Downtown,
            },
            new Zone() {
            InternalGameName = "MFLFMP",
            DisplayName = "Washington Park",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            IsSpecificLocation = true,
            Type = eLocationType.Wilderness,
            DealerMenuContainerID = "MarijuanaAreaDrugDealerMenuGroupID",
            CustomerMenuContainerID = "MarijuanaAreaDrugCustomerMenuGroupID",
            },
            new Zone() {
            InternalGameName = "MFLADD",
            DisplayName = "Ocean View",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            IsSpecificLocation = true,
            Type = eLocationType.Downtown,
            },
            new Zone() {
            InternalGameName = "MFLSB",
            DisplayName = "Ocean Beach",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            IsSpecificLocation = true,
            Type = eLocationType.Suburb,
            DealerMenuContainerID = "CokeAreaDrugDealerMenuGroupID",
            CustomerMenuContainerID = "CokeAreaDrugCustomerMenuGroupID",
            },
            new Zone() {
            InternalGameName = "MFLSPP",
            DisplayName = "Ocean Heights Park",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            IsSpecificLocation = true,
            Type = eLocationType.Wilderness,
            },
            new Zone() {
            InternalGameName = "MFLSPB",
            DisplayName = "Ocean Heights Beach",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            IsSpecificLocation = true,
            Type = eLocationType.Suburb,
            },
            new Zone() {
            InternalGameName = "MFLDLI",
            DisplayName = "Starfish Island",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            Economy = eLocationEconomy.Rich,
            Type = eLocationType.Suburb,
            },
            new Zone() {
            InternalGameName = "MFSMRI",
            DisplayName = "Clymenus Island",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            Economy = eLocationEconomy.Rich,
            Type = eLocationType.Suburb,
            },
            new Zone() {
            InternalGameName = "MFLSI",
            DisplayName = "Meteor Island",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            Economy = eLocationEconomy.Rich,
            Type = eLocationType.Suburb,
            },
            new Zone() {
            InternalGameName = "MFLBI",
            DisplayName = "Beast Isle",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            Economy = eLocationEconomy.Rich,
            Type = eLocationType.Wilderness,
            },
            new Zone() {
            InternalGameName = "MFLSMI",
            DisplayName = "Prawn Island",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            Economy = eLocationEconomy.Rich,
            Type = eLocationType.Suburb,
            },
            new Zone() {
            InternalGameName = "MFLWI",
            DisplayName = "Sherlock Island",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            Economy = eLocationEconomy.Poor,
            DealerMenuContainerID = "MethamphetamineAreaDrugDealerMenuGroupID",
            CustomerMenuContainerID = "MethamphetamineAreaDrugCustomerMenuGroupID",
            },
            new Zone() {
            InternalGameName = "MFLDI",
            DisplayName = "VicePort",
            CountyID = StaticStrings.ViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            Economy = eLocationEconomy.Poor,
            Type = eLocationType.Industrial,
            DealerMenuContainerID = "SPANKAreaDrugDealerMenuGroupID",
            CustomerMenuContainerID = "SPANKAreaDrugCustomerMenuGroupID",
            },
            new Zone() {
            InternalGameName = "MFLBSB",
            DisplayName = "Ocean Bay",
            CountyID = StaticStrings.ViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            },
            new Zone() {
            InternalGameName = "MFLFMI",
            DisplayName = "Escobar Memorial Island ",
            CountyID = StaticStrings.ViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            Economy = eLocationEconomy.Poor,
            Type = eLocationType.Wilderness,
            },
            new Zone() {
            InternalGameName = "MFOMNI",
            DisplayName = "Hyman District",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            IsSpecificLocation = true,
            Type = eLocationType.Downtown,
            },
            new Zone() {
            InternalGameName = "MFLPKW",
            DisplayName = "West Haven",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            IsSpecificLocation = true,
            Type = eLocationType.Downtown,
            },
            new Zone() {
            InternalGameName = "MFDMIA",
            DisplayName = "Downtown Vice City",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            IsSpecificLocation = true,
            Type = eLocationType.Downtown,
            },
            new Zone() {
            InternalGameName = "MFLBKL",
            DisplayName = "Felicity",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            IsSpecificLocation = true,
            Economy = eLocationEconomy.Rich,
            Type = eLocationType.Downtown,
            },
            new Zone() {
            InternalGameName = "MFOTWN",
            DisplayName = "Little Dominica",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            IsSpecificLocation = true,
            Economy = eLocationEconomy.Poor,
            Type = eLocationType.Suburb,
            DealerMenuContainerID = "CrackAreaDrugDealerMenuGroupID",
            CustomerMenuContainerID = "CrackAreaDrugCustomerMenuGroupID",
            },
            new Zone() {
            InternalGameName = "MFLUMP",
            DisplayName = "Rockridge",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            IsSpecificLocation = true,
            Type = eLocationType.Suburb,
            DealerMenuContainerID = "ToiletCleanerAreaDrugDealerMenuGroupID",
            CustomerMenuContainerID = "ToiletCleanerAreaDrugCustomerMenuGroupID",
            },
            new Zone() {
            InternalGameName = "MFLSPG",
            DisplayName = "Olympic Heights",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            IsSpecificLocation = true,
            Type = eLocationType.Suburb,
            DealerMenuContainerID = "HeroinAreaDrugDealerMenuGroupID",
            CustomerMenuContainerID = "HeroinAreaDrugCustomerMenuGroupID",
            },
            new Zone() {
            InternalGameName = "MFLHVA",
            DisplayName = "Little Havana",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            IsSpecificLocation = true,
            Type = eLocationType.Suburb,
            DealerMenuContainerID = "CokeAreaDrugDealerMenuGroupID",
            CustomerMenuContainerID = "CokeAreaDrugCustomerMenuGroupID",
            },
            new Zone() {
            InternalGameName = "MFLCRW",
            DisplayName = "Coral City",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            IsSpecificLocation = true,
            Type = eLocationType.Suburb,
            },
            new Zone() {
            InternalGameName = "MFLCRG",
            DisplayName = "Coral City",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            IsSpecificLocation = true,
            Economy = eLocationEconomy.Rich,
            Type = eLocationType.Suburb,
            },
            new Zone() {
            InternalGameName = "MFLCCG",
            DisplayName = "Banana Grove",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            IsSpecificLocation = true,
            Type = eLocationType.Suburb,
            DealerMenuContainerID = "MarijuanaAreaDrugDealerMenuGroupID",
            CustomerMenuContainerID = "MarijuanaAreaDrugCustomerMenuGroupID",
            },
            new Zone() {
            InternalGameName = "MFGVCR",
            DisplayName = "Federal District",
            CountyID = StaticStrings.CityOfViceCountyID,
            StateID = StaticStrings.LeonidaStateID,
            IsSpecificLocation = true,
            Type = eLocationType.Downtown,
            },





            };
        Serialization.SerializeParams(MiamiDriver3Zones, "Plugins\\LosSantosRED\\AlternateConfigs\\SunshineDream\\Zones_SunshineDream.xml");
    }

    //private bool IsPointInPolygon(Vector2 point, Vector2[] polygon)
    //{
    //    int polygonLength = polygon.Length, i = 0;
    //    bool inside = false;
    //    // x, y for tested point.
    //    float pointX = point.X, pointY = point.Y;
    //    // start / end point for the current polygon segment.
    //    float startX, startY, endX, endY;
    //    Vector2 endPoint = polygon[polygonLength - 1];
    //    endX = endPoint.X;
    //    endY = endPoint.Y;
    //    while (i < polygonLength)
    //    {
    //        startX = endX; startY = endY;
    //        endPoint = polygon[i++];
    //        endX = endPoint.X; endY = endPoint.Y;
    //        //
    //        inside ^= (endY > pointY ^ startY > pointY) /* ? pointY inside [startY;endY] segment ? */
    //                  && /* if so, test if it is under the segment */
    //                  ((pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY));
    //    }
    //    return inside;
    //}

    public void Setup(ILocationTypes locationTypes)
    {
        foreach(Zone zone in ZoneList)
        {
            if(!string.IsNullOrEmpty(zone.CountyID))
            {
                zone.GameCounty = locationTypes.GetCounty(zone.CountyID);
            }
            if (!string.IsNullOrEmpty(zone.StateID))
            {
                zone.GameState = locationTypes.GetState(zone.StateID);
            }
        }
        
    }
}
