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

        //Blaine
        List<ZoneAgency> StandardBlaineAgencies = new List<ZoneAgency>() {
            new ZoneAgency("LSSD-BC", 0, 85, 75),
            new ZoneAgency("DOA", 1, 15, 5),
            new ZoneAgency("LSSD", 2, 0, 5),
            new ZoneAgency("NOOSE", 3, 0, 10),
            new ZoneAgency("LSSD-ASD", 4, 0, 5),
            new ZoneAgency("PRISEC", 5, 0, 0)};

        List< ZoneAgency > BlaineParkRangerAgencies = new List<ZoneAgency>() {
            new ZoneAgency("SAPR", 0, 51, 20),
            new ZoneAgency("LSSD-BC", 1, 49, 65),
            new ZoneAgency("NOOSE", 2, 0, 10),
            new ZoneAgency("LSSD-ASD", 3, 0, 5),
            new ZoneAgency("PRISEC", 4, 0, 0)};

        List<ZoneAgency> BlainePrisonAgencies = new List<ZoneAgency>() {
            new ZoneAgency("SASPA",0, 100, 70),
            new ZoneAgency("LSSD-BC", 1, 0, 10),
            new ZoneAgency("NOOSE", 2, 0, 15),
            new ZoneAgency("LSSD-ASD", 3, 0, 5),
            new ZoneAgency("PRISEC", 4, 0, 0)};

        List<ZoneAgency> StandardCityAgencies = new List<ZoneAgency>() {
            new ZoneAgency("LSPD", 0, 94, 80),
            new ZoneAgency("FIB", 1, 4, 4),
            new ZoneAgency("DOA", 2, 1, 1),
            new ZoneAgency("NOOSE", 3, 0, 10),
            new ZoneAgency("LSPD-ASD", 4, 0, 5),
            new ZoneAgency("PRISEC", 5, 1, 0)};

        List<ZoneAgency> DavisAgencies = new List<ZoneAgency>() {
            new ZoneAgency("LSSD-DV", 0, 50, 40),
            new ZoneAgency("LSPD", 1, 50, 40),
            new ZoneAgency("NOOSE", 2, 0, 15),
            new ZoneAgency("LSPD-ASD", 4, 0, 5) };

        List<ZoneAgency> SecurityAgencies = new List<ZoneAgency>() {
            new ZoneAgency("PRISEC", 0, 100, 50),
            new ZoneAgency("NOOSE", 1, 0, 50) };

        List<string> VespucciAreaUnits = new List<string>() { ScannerAudio.attention_all_area_units.VespucciAreaUnits.FileName, ScannerAudio.attention_all_area_units.VespucciAreaUnits2.FileName };
        List<ZoneAgency> VespucciAgencies = new List<ZoneAgency>() {
            new ZoneAgency("LSPD-DP", 0, 84, 70),
            new ZoneAgency("LSPD", 1, 10, 10),
            new ZoneAgency("FIB", 2, 5, 5),
            new ZoneAgency("NOOSE", 3, 0, 10),
            new ZoneAgency("LSPD-ASD", 4, 0, 5),
            new ZoneAgency("PRISEC", 5, 0, 0)};

        List<string> CentralAreaUnits = new List<string>() { ScannerAudio.attention_all_area_units.CentralUnits.FileName, ScannerAudio.attention_all_area_units.CentralUnits1.FileName, ScannerAudio.attention_all_area_units.CentralAreaUnits.FileName };
        List<string> EastLosSantosAreaUnits = new List<string>() { ScannerAudio.attention_all_area_units.EastLosSantosUnits1.FileName, ScannerAudio.attention_all_area_units.EastLosSantosUnits1.FileName, ScannerAudio.attention_all_area_units.EastLosSantos2.FileName };
        List<ZoneAgency> EastLosSantosAgencies = new List<ZoneAgency>() {
            new ZoneAgency("LSPD-ELS", 0, 85, 70),
            new ZoneAgency("LSPD", 1, 10, 10),
            new ZoneAgency("FIB", 2, 5, 5),
            new ZoneAgency("NOOSE", 3, 0, 10),
            new ZoneAgency("LSPD-ASD", 4, 0, 5),
            new ZoneAgency("PRISEC", 5, 0, 0)};

        List<string> VinewoodAreaUnits = new List<string>() { ScannerAudio.attention_all_area_units.VinewoodAreaUnits.FileName, ScannerAudio.attention_all_area_units.VinewoodUnits2.FileName };
        List<ZoneAgency> VinewoodAgencies = new List<ZoneAgency>() {
            new ZoneAgency("LSPD-VW", 0, 75, 70),
            new ZoneAgency("LSSD-VW", 1, 10, 10),
            new ZoneAgency("FIB", 2, 10, 5),
            new ZoneAgency("NOOSE", 3, 0, 10),
            new ZoneAgency("LSPD-ASD", 4, 0, 5),
            new ZoneAgency("PRISEC", 5, 0, 0)};

        List<ZoneAgency> PortAgencies = new List<ZoneAgency>() {
            new ZoneAgency("LSPA", 0, 95, 80),
            new ZoneAgency("LSPD", 1, 5, 5),
            new ZoneAgency("NOOSE", 2, 0, 10),
            new ZoneAgency("LSPD-ASD", 3, 0, 5),
            new ZoneAgency("PRISEC", 4, 0, 0)};

        List<string> PortAreaUnits = new List<string>() { ScannerAudio.attention_all_area_units.PortOfLosSantosUnits.FileName, ScannerAudio.attention_all_area_units.PortOfLosSantosUnits1.FileName, ScannerAudio.attention_all_area_units.PortOfLosSantosUnits2.FileName };

        List<ZoneAgency> RockfordHillsAgencies = new List<ZoneAgency>() {
            new ZoneAgency("LSPD-RH", 0, 85, 70),
            new ZoneAgency("FIB", 1, 10, 10),
            new ZoneAgency("LSPD", 2, 5, 5),
            new ZoneAgency("NOOSE", 3, 0, 10),
            new ZoneAgency("LSPD-ASD", 4, 0, 5),
            new ZoneAgency("PRISEC", 5, 0, 0)};

        List<string> VinewoodHillsAreaUnits = new List<string>() { ScannerAudio.attention_all_area_units.VinewoodAreaUnits.FileName, ScannerAudio.attention_all_area_units.VinewoodUnits2.FileName };

        List<ZoneAgency> VinewoodHillsAgencies = new List<ZoneAgency>() {
            new ZoneAgency("LSSD-VW", 0, 85, 70),
            new ZoneAgency("DOA", 1, 15, 15),
            new ZoneAgency("NOOSE", 2, 0, 10),
            new ZoneAgency("LSPD-ASD", 3, 0, 5),
            new ZoneAgency("PRISEC", 4, 0, 0)};

        List<string> ChumashsAreaUnits = new List<string>() { ScannerAudio.attention_all_area_units.ChumashUnits.FileName, ScannerAudio.attention_all_area_units.ChumashUnits1.FileName, ScannerAudio.attention_all_area_units.ChumashUnits2.FileName };
        List<ZoneAgency> ChumashAgencies = new List<ZoneAgency>() {
            new ZoneAgency("LSSD-CH", 0, 85, 70),
            new ZoneAgency("LSPD-CH", 1, 10, 5),
            new ZoneAgency("DOA", 2, 5, 5),
            new ZoneAgency("NOOSE", 3, 0, 15),
            new ZoneAgency("LSPD-ASD", 4, 0, 5),
            new ZoneAgency("PRISEC", 5, 0, 0)};

        List<ZoneAgency> StandardSheriffAgencies = new List<ZoneAgency>() {
            new ZoneAgency("LSSD", 0, 85, 70),
            new ZoneAgency("DOA", 1, 15, 5),
            new ZoneAgency("NOOSE", 2, 0, 15),
            new ZoneAgency("LSSD-ASD", 3, 0, 10),
            new ZoneAgency("PRISEC", 4, 0, 0)};

        List<ZoneAgency> OceanAgencies = new List<ZoneAgency>() {
            new ZoneAgency("SACG", 0, 95, 80),
            new ZoneAgency("LSPD", 1, 5, 5),
            new ZoneAgency("NOOSE", 2, 0, 15),
            new ZoneAgency("PRISEC", 3, 0, 0)
        };

        List<ZoneAgency> AirportAgencies = new List<ZoneAgency>() {
            new ZoneAgency("LSIAPD", 0, 95, 80),
            new ZoneAgency("LSPD", 1, 5, 5),
            new ZoneAgency("NOOSE", 2, 0, 10),
            new ZoneAgency("LSPD-ASD", 3, 0, 5),
            new ZoneAgency("PRISEC", 4, 1, 0),
            new ZoneAgency("SACG", 5, 1, 0)};

        List<ZoneAgency> ArmyAgencies = new List<ZoneAgency>() {
            new ZoneAgency("ARMY", 0, 100, 100) };

        ZoneList = new List<Zone>
        {
            //One Off
            new Zone("OCEANA", "Pacific Ocean", ScannerAudio.areas.TheOcean.FileName, County.PacificOcean,"Pacific Coast") { ZoneAgencies = OceanAgencies },   

            //North Blaine
            new Zone("PROCOB", "Procopio Beach", ScannerAudio.areas.ProcopioBeach.FileName, County.BlaineCounty,"North Blaine County") { ZoneAgencies = StandardBlaineAgencies },
            new Zone("MTCHIL", "Mount Chiliad", ScannerAudio.areas.MountChiliad.FileName, County.BlaineCounty,"North Blaine County") { ZoneAgencies = BlaineParkRangerAgencies },
            new Zone("MTGORDO", "Mount Gordo", ScannerAudio.areas.MountGordo.FileName, County.BlaineCounty,"North Blaine County") { ZoneAgencies = BlaineParkRangerAgencies },
            new Zone("PALETO", "Paleto Bay", ScannerAudio.areas.PaletoBay.FileName, County.BlaineCounty,"North Blaine County", new List<string>() { ScannerAudio.attention_all_area_units.PaletoaBayUnits.FileName, ScannerAudio.attention_all_area_units.PaletoBayUnits.FileName, ScannerAudio.attention_all_area_units.PaletoBayUnits3.FileName }, "Paleto Bay Units") { ZoneAgencies = StandardBlaineAgencies },
            new Zone("PALCOV", "Paleto Cove", ScannerAudio.areas.PaletoBay.FileName, County.BlaineCounty,"North Blaine County") { ZoneAgencies = StandardBlaineAgencies },
            new Zone("PALFOR", "Paleto Forest", ScannerAudio.areas.PaletoForest.FileName, County.BlaineCounty,"North Blaine County") { ZoneAgencies = StandardBlaineAgencies },
            new Zone("CMSW", "Chiliad Mountain State Wilderness", ScannerAudio.areas.ChilliadMountainStWilderness.FileName, County.BlaineCounty,"North Blaine County") { ZoneAgencies = BlaineParkRangerAgencies },
            new Zone("CALAFB", "Calafia Bridge", "", County.BlaineCounty,"North Blaine County") { ZoneAgencies = StandardBlaineAgencies },
            new Zone("GALFISH", "Galilee", "", County.BlaineCounty,"North Blaine County") { ZoneAgencies = StandardBlaineAgencies },
            new Zone("ELGORL", "El Gordo Lighthouse", ScannerAudio.areas.MountGordo.FileName, County.BlaineCounty,"North Blaine County") { ZoneAgencies = StandardBlaineAgencies },

            //Blaine
            new Zone("ALAMO", "Alamo Sea", ScannerAudio.areas.TheAlamaSea.FileName, County.BlaineCounty,"Blaine County") { ZoneAgencies = StandardBlaineAgencies },
            new Zone("ARMYB", "Fort Zancudo", ScannerAudio.areas.FtZancudo.FileName, County.BlaineCounty,"Blaine County") { IsRestrictedDuringWanted = true, ZoneAgencies = ArmyAgencies },
            new Zone("BRADP", "Braddock Pass", ScannerAudio.areas.BraddockPass.FileName, County.BlaineCounty,"Blaine County") { ZoneAgencies = StandardBlaineAgencies },
            new Zone("BRADT", "Braddock Tunnel", ScannerAudio.areas.TheBraddockTunnel.FileName, County.BlaineCounty,"Blaine County") { ZoneAgencies = StandardBlaineAgencies },
            new Zone("CANNY", "Raton Canyon", ScannerAudio.areas.RatonCanyon.FileName, County.BlaineCounty,"Blaine County") { ZoneAgencies = StandardBlaineAgencies },
            new Zone("CCREAK", "Cassidy Creek", "", County.BlaineCounty,"Blaine County") { ZoneAgencies = StandardBlaineAgencies },
            new Zone("DESRT", "Grand Senora Desert", ScannerAudio.areas.GrandeSonoranDesert.FileName, County.BlaineCounty,"Blaine County") { ZoneAgencies = StandardBlaineAgencies },
            new Zone("GRAPES", "Grapeseed", ScannerAudio.areas.Grapeseed.FileName, County.BlaineCounty,"Blaine County") { ZoneAgencies = StandardBlaineAgencies },
            new Zone("HUMLAB", "Humane Labs and Research", "", County.BlaineCounty,"Blaine County") { ZoneAgencies = StandardBlaineAgencies },
            new Zone("JAIL", "Bolingbroke Penitentiary", ScannerAudio.areas.BoilingBrookPenitentiary.FileName, County.BlaineCounty,"Blaine County") { ZoneAgencies = BlainePrisonAgencies,IsRestrictedDuringWanted = true },
            new Zone("LAGO", "Lago Zancudo", ScannerAudio.areas.LagoZancudo.FileName, County.BlaineCounty,"Blaine County") { ZoneAgencies = StandardBlaineAgencies },
            new Zone("MTJOSE", "Mount Josiah", ScannerAudio.areas.MtJosiah.FileName, County.BlaineCounty,"Blaine County") { ZoneAgencies = StandardBlaineAgencies },
            new Zone("NCHU", "North Chumash", ScannerAudio.areas.NorthChumash.FileName, County.BlaineCounty,"Blaine County") { ZoneAgencies = StandardBlaineAgencies },   
            new Zone("SANCHIA", "San Chianski Mountain Range", "", County.BlaineCounty,"Blaine County") { ZoneAgencies = BlaineParkRangerAgencies },
            new Zone("SANDY", "Sandy Shores", ScannerAudio.areas.SandyShores.FileName, County.BlaineCounty,"Blaine County", new List<string>() { ScannerAudio.attention_all_area_units.SandyShoreUnits.FileName, ScannerAudio.attention_all_area_units.SandyShoresUnits2.FileName, ScannerAudio.attention_all_area_units.SanyShoreUnits3.FileName }, "Sandy Shores Units") { ZoneAgencies = StandardBlaineAgencies },
            new Zone("SLAB", "Slab City", ScannerAudio.areas.SlabCity.FileName, County.BlaineCounty,"Blaine County") { ZoneAgencies = StandardBlaineAgencies },
            new Zone("ZANCUDO", "Zancudo River", ScannerAudio.areas.ZancudoRiver.FileName, County.BlaineCounty,"Blaine County", new List<string>() { ScannerAudio.attention_all_area_units.ZancudoRiverUnits.FileName }, "Zancudo River Units") { ZoneAgencies = BlaineParkRangerAgencies },
            new Zone("ZQ_UAR", "Davis Quartz", ScannerAudio.areas.DavisCourts.FileName, County.BlaineCounty,"Blaine County") { ZoneAgencies = StandardBlaineAgencies },

            //Vespucci
            new Zone("BEACH", "Vespucci Beach", ScannerAudio.areas.VespucciBeach.FileName, County.CityOfLosSantos,"Vespucci", VespucciAreaUnits, "Vespucci Area Units") { ZoneAgencies = VespucciAgencies },
            new Zone("DELBE", "Del Perro Beach", ScannerAudio.areas.DelPierroBeach.FileName, County.CityOfLosSantos,"Vespucci", VespucciAreaUnits, "Vespucci Area Units") { ZoneAgencies = VespucciAgencies },
            new Zone("DELPE", "Del Perro", ScannerAudio.areas.DelPierro.FileName, County.CityOfLosSantos,"Vespucci", VespucciAreaUnits, "Vespucci Area Units") { ZoneAgencies = VespucciAgencies },
            new Zone("VCANA", "Vespucci Canals", ScannerAudio.areas.VespucciCanal.FileName, County.CityOfLosSantos,"Vespucci", VespucciAreaUnits, "Vespucci Area Units") { ZoneAgencies = VespucciAgencies },
            new Zone("VESP", "Vespucci Metro", ScannerAudio.areas.Vespucci.FileName, County.CityOfLosSantos,"Vespucci", VespucciAreaUnits, "Vespucci Area Units") { ZoneAgencies = VespucciAgencies },
            new Zone("LOSPUER", "La Puerta", ScannerAudio.areas.LaPuertes.FileName, County.CityOfLosSantos,"Vespucci", VespucciAreaUnits, "Vespucci Area Units") { ZoneAgencies = VespucciAgencies },
            new Zone("PBLUFF", "Pacific Bluffs", ScannerAudio.areas.PacificBluffs.FileName, County.CityOfLosSantos,"Vespucci", VespucciAreaUnits, "Vespucci Area Units") { ZoneAgencies = VespucciAgencies },
            new Zone("DELSOL", "Puerto Del Sol", ScannerAudio.areas.PuertoDelSoul.FileName, County.CityOfLosSantos,"Vespucci", VespucciAreaUnits, "Vespucci Area Units") { ZoneAgencies = VespucciAgencies },

            //Central
            new Zone("BANNING", "Banning", ScannerAudio.areas.Banning.FileName, County.CityOfLosSantos,"Central", CentralAreaUnits, "Central Area Units") { ZoneAgencies = DavisAgencies },
            new Zone("CHAMH", "Chamberlain Hills", ScannerAudio.areas.ChamberlainHills.FileName, County.CityOfLosSantos,"Central", CentralAreaUnits, "Central Area Units") { ZoneAgencies = DavisAgencies },
            new Zone("DAVIS", "Davis", ScannerAudio.areas.Davis.FileName, County.CityOfLosSantos,"Central", CentralAreaUnits, "Central Area Units") { ZoneAgencies = DavisAgencies },
            new Zone("DOWNT", "Downtown", ScannerAudio.areas.Downtown.FileName, County.CityOfLosSantos,"Central", CentralAreaUnits, "Central Area Units") { ZoneAgencies = StandardCityAgencies },
            new Zone("PBOX", "Pillbox Hill", ScannerAudio.areas.PillboxHill.FileName, County.CityOfLosSantos,"Central", CentralAreaUnits, "Central Area Units") { ZoneAgencies = StandardCityAgencies },
            new Zone("RANCHO", "Rancho", ScannerAudio.areas.Rancho.FileName, County.CityOfLosSantos,"Central", CentralAreaUnits, "Central Area Units") { ZoneAgencies = DavisAgencies },
            new Zone("SKID", "Mission Row", ScannerAudio.areas.MissionRow.FileName, County.CityOfLosSantos,"Central", CentralAreaUnits, "Central Area Units") { ZoneAgencies = StandardCityAgencies },
            new Zone("STAD", "Maze Bank Arena", ScannerAudio.areas.MazeBankArena.FileName, County.CityOfLosSantos,"Central", CentralAreaUnits, "Central Area Units") { ZoneAgencies = SecurityAgencies },
            new Zone("STRAW", "Strawberry", ScannerAudio.areas.Strawberry.FileName, County.CityOfLosSantos,"Central", CentralAreaUnits, "Central Area Units") { ZoneAgencies = DavisAgencies },
            new Zone("TEXTI", "Textile City", ScannerAudio.areas.TextileCity.FileName, County.CityOfLosSantos,"Central", CentralAreaUnits, "Central Area Units") { ZoneAgencies = StandardCityAgencies },
            new Zone("LEGSQU", "Legion Square", "", County.CityOfLosSantos,"Central", CentralAreaUnits, "Central Area Units") { ZoneAgencies = StandardCityAgencies },

            //East LS
            new Zone("CYPRE", "Cypress Flats", ScannerAudio.areas.CypressFlats.FileName, County.CityOfLosSantos,"East Los Santos", EastLosSantosAreaUnits, "East Los Santos Units") { ZoneAgencies = EastLosSantosAgencies },
            new Zone("LMESA", "La Mesa", ScannerAudio.areas.LaMesa.FileName, County.CityOfLosSantos,"East Los Santos", EastLosSantosAreaUnits, "East Los Santos Units") { ZoneAgencies = EastLosSantosAgencies },
            new Zone("MIRR", "Mirror Park", ScannerAudio.areas.MirrorPark.FileName, County.CityOfLosSantos,"East Los Santos", EastLosSantosAreaUnits, "East Los Santos Units") { ZoneAgencies = EastLosSantosAgencies },
            new Zone("MURRI", "Murrieta Heights", ScannerAudio.areas.MuriettaHeights.FileName, County.CityOfLosSantos,"East Los Santos", EastLosSantosAreaUnits, "East Los Santos Units") { ZoneAgencies = EastLosSantosAgencies },
            new Zone("EBURO", "El Burro Heights", ScannerAudio.areas.ElBerroHights.FileName, County.CityOfLosSantos,"East Los Santos", EastLosSantosAreaUnits, "East Los Santos Units") { ZoneAgencies = EastLosSantosAgencies },//was county

            //Vinewood
            new Zone("ALTA", "Alta", ScannerAudio.areas.Alta.FileName, County.CityOfLosSantos,"Vinewood", VinewoodAreaUnits, "Vinewood Units") { ZoneAgencies = VinewoodAgencies },
            new Zone("DTVINE", "Downtown Vinewood", ScannerAudio.areas.DowntownVinewood.FileName, County.CityOfLosSantos,"Vinewood", VinewoodAreaUnits, "Vinewood Units") { ZoneAgencies = VinewoodAgencies },
            new Zone("EAST_V", "East Vinewood", ScannerAudio.areas.EastVinewood.FileName, County.CityOfLosSantos,"Vinewood", VinewoodAreaUnits, "Vinewood Units") { ZoneAgencies = VinewoodAgencies },
            new Zone("HAWICK", "Hawick", "", County.CityOfLosSantos,"Vinewood", VinewoodAreaUnits, "Vinewood Units") { ZoneAgencies = VinewoodAgencies },
            new Zone("HORS", "Vinewood Racetrack", ScannerAudio.areas.TheRaceCourse.FileName, County.CityOfLosSantos,"Vinewood", VinewoodAreaUnits, "Vinewood Units") { ZoneAgencies = VinewoodAgencies },
            new Zone("VINE", "Vinewood", ScannerAudio.areas.Vinewood.FileName, County.CityOfLosSantos,"Vinewood", VinewoodAreaUnits, "Vinewood Units") { ZoneAgencies = VinewoodAgencies },
            new Zone("WVINE", "West Vinewood", ScannerAudio.areas.WestVinewood.FileName, County.CityOfLosSantos,"Vinewood", VinewoodAreaUnits, "Vinewood Units") { ZoneAgencies = VinewoodAgencies },

            //PortOfLosSantos
            new Zone("ELYSIAN", "Elysian Island", ScannerAudio.areas.ElysianIsland.FileName, County.CityOfLosSantos,"Port Area", PortAreaUnits, "Port of Los Santos Units") { ZoneAgencies = PortAgencies },
            new Zone("ZP_ORT", "Port of South Los Santos", ScannerAudio.areas.PortOfSouthLosSantos.FileName, County.CityOfLosSantos,"Port Area", PortAreaUnits, "Port of Los Santos Units") { ZoneAgencies = PortAgencies },
            new Zone("TERMINA", "Terminal", ScannerAudio.areas.Terminal.FileName, County.CityOfLosSantos,"Port Area", PortAreaUnits, "Port of Los Santos Units") { ZoneAgencies = PortAgencies },
            new Zone("ZP_ORT", "Port of South Los Santos", ScannerAudio.areas.PortOfSouthLosSantos.FileName, County.CityOfLosSantos,"Port Area", PortAreaUnits, "Port of Los Santos Units") { ZoneAgencies = PortAgencies },
            new Zone("AIRP", "Los Santos International Airport", ScannerAudio.areas.LosSantosInternationalAirport.FileName, County.CityOfLosSantos,"Port Area") { IsRestrictedDuringWanted = true, ZoneAgencies = AirportAgencies },

            //Rockford Hills
            new Zone("BURTON", "Burton", ScannerAudio.areas.Burton.FileName, County.CityOfLosSantos,"Rockford Hills") { ZoneAgencies = RockfordHillsAgencies },
            new Zone("GOLF", "GWC and Golfing Society", ScannerAudio.areas.TheGWCGolfingSociety.FileName, County.CityOfLosSantos,"Rockford Hills") { ZoneAgencies = SecurityAgencies },
            new Zone("KOREAT", "Little Seoul", ScannerAudio.areas.LittleSeoul.FileName, County.CityOfLosSantos,"Rockford Hills") { ZoneAgencies = RockfordHillsAgencies },
            new Zone("MORN", "Morningwood", ScannerAudio.areas.MorningWood.FileName, County.CityOfLosSantos,"Rockford Hills") { ZoneAgencies = RockfordHillsAgencies },
            new Zone("MOVIE", "Richards Majestic", ScannerAudio.areas.RichardsMajesticStudio.FileName, County.CityOfLosSantos,"Rockford Hills") { ZoneAgencies = SecurityAgencies },
            new Zone("RICHM", "Richman", ScannerAudio.areas.Richman.FileName, County.CityOfLosSantos,"Rockford Hills") { ZoneAgencies = RockfordHillsAgencies },
            new Zone("ROCKF", "Rockford Hills", ScannerAudio.areas.RockfordHills.FileName, County.CityOfLosSantos,"Rockford Hills") { ZoneAgencies = RockfordHillsAgencies },         

            //Vinewood Hills
            new Zone("CHIL", "Vinewood Hills", ScannerAudio.areas.VinewoodHills.FileName, County.LosSantosCounty,"Vinewood Hills", VinewoodHillsAreaUnits, "Vinewood Hills Units") { ZoneAgencies = VinewoodHillsAgencies },
            new Zone("GREATC", "Great Chaparral", ScannerAudio.areas.GreatChapparalle.FileName, County.LosSantosCounty,"Vinewood Hills", VinewoodHillsAreaUnits, "Vinewood Hills Units") { ZoneAgencies = VinewoodHillsAgencies },
            new Zone("BAYTRE", "Baytree Canyon", ScannerAudio.areas.BayTreeCanyon.FileName, County.LosSantosCounty,"Vinewood Hills") { ZoneAgencies = VinewoodHillsAgencies },
            new Zone("RGLEN", "Richman Glen", ScannerAudio.areas.RichmanGlenn.FileName, County.LosSantosCounty,"Vinewood Hills", VinewoodHillsAreaUnits, "Vinewood Hills Units") { ZoneAgencies = VinewoodHillsAgencies },
            new Zone("TONGVAV", "Tongva Valley", ScannerAudio.areas.TongvaValley.FileName, County.LosSantosCounty,"Vinewood Hills", VinewoodHillsAreaUnits, "Vinewood Hills Units") { ZoneAgencies = VinewoodHillsAgencies },

            new Zone("HARMO", "Harmony", ScannerAudio.areas.Harmony.FileName, County.LosSantosCounty,"Vinewood Hills") { ZoneAgencies = StandardSheriffAgencies },
            new Zone("RTRAK", "Redwood Lights Track", ScannerAudio.areas.TheRedwoodLightsTrack.FileName, County.LosSantosCounty,"Vinewood Hills") { ZoneAgencies = StandardSheriffAgencies },
           
            //Chumash
            new Zone("BANHAMC", "Banham Canyon Dr", "", County.LosSantosCounty,"Chumash", ChumashsAreaUnits, "Chumash Units") { ZoneAgencies = ChumashAgencies },
            new Zone("BHAMCA", "Banham Canyon", "", County.LosSantosCounty,"Chumash", ChumashsAreaUnits, "Chumash Units") { ZoneAgencies = ChumashAgencies },
            new Zone("CHU", "Chumash", ScannerAudio.areas.Chumash.FileName, County.LosSantosCounty,"Chumash", ChumashsAreaUnits, "Chumash Units") { ZoneAgencies = ChumashAgencies },
            new Zone("TONGVAH", "Tongva Hills", ScannerAudio.areas.TongaHills.FileName, County.LosSantosCounty,"Chumash", ChumashsAreaUnits, "Chumash Units") { ZoneAgencies = ChumashAgencies },
           
            //Tataviam 
            new Zone("LACT", "Land Act Reservoir", "", County.LosSantosCounty,"Tataviam Area") { ZoneAgencies = StandardSheriffAgencies },
            new Zone("LDAM", "Land Act Dam", "", County.LosSantosCounty,"Tataviam Area") { ZoneAgencies = StandardSheriffAgencies },
            new Zone("NOOSE", "N.O.O.S.E", "", County.LosSantosCounty,"Tataviam Area") { IsRestrictedDuringWanted = true, ZoneAgencies = StandardSheriffAgencies },
            new Zone("PALHIGH", "Palomino Highlands", ScannerAudio.areas.PalominoHighlands.FileName, County.LosSantosCounty,"Tataviam Area") { ZoneAgencies = StandardSheriffAgencies },
            new Zone("PALMPOW", "Palmer - Taylor Power Station", ScannerAudio.areas.PalmerTaylorPowerStation.FileName, County.LosSantosCounty,"Tataviam Area") { ZoneAgencies = StandardSheriffAgencies },    
            new Zone("SANAND", "San Andreas", ScannerAudio.areas.SanAndreas.FileName, County.LosSantosCounty,"San Andreas") { ZoneAgencies = StandardSheriffAgencies },
            new Zone("TATAMO", "Tataviam Mountains", ScannerAudio.areas.TatathiaMountains.FileName, County.LosSantosCounty,"Tataviam Area") { ZoneAgencies = StandardSheriffAgencies },
            new Zone("WINDF", "Ron Alternates Wind Farm", ScannerAudio.areas.RonAlternatesWindFarm.FileName, County.LosSantosCounty,"Tataviam Area") { ZoneAgencies = StandardSheriffAgencies },
    };
        
    }
    public static Zone GetZoneAtLocation(Vector3 ZonePosition)
    {        
        string zoneName = string.Empty;
        unsafe
        {
            IntPtr ptr = Rage.Native.NativeFunction.CallByName<IntPtr>("GET_NAME_OF_ZONE", ZonePosition.X, ZonePosition.Y, ZonePosition.Z);

            zoneName = Marshal.PtrToStringAnsi(ptr);
        }
        Zone ListResult = ZoneList.Where(x => x.GameName.ToUpper() == zoneName.ToUpper()).FirstOrDefault();
        if(ListResult == null)
        {
            if (ZonePosition.IsInLosSantosCity())
                return new Zone("UNK_LSCITY", "Los Santos", "", County.CityOfLosSantos,"San Andreas");
            else
                return new Zone("UNK_LSCOUNTY", "Los Santos County", "", County.LosSantosCounty, "San Andreas");
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
            //if (MyZone.ZoneCounty == County.BlaineCounty)
            //    CountyName = "Blaine County";
            //else if (MyZone.ZoneCounty == County.CityOfLosSantos)
            //    CountyName = "City of Los Santos";
            //else if (MyZone.ZoneCounty == County.LosSantosCounty)
            //    CountyName = "Los Santos County";

            CountyName = MyZone.AreaName;

            return MyZone.TextName + ", " + CountyName;
        }
        else
        {
            return MyZone.TextName;
        }

    }
    //public ZoneAgency GetRandomVehicle(bool IsMotorcycle)
    //{
    //    if (Vehicles == null || !Vehicles.Any())
    //        return null;

    //    List<VehicleInformation> ToPickFrom = Vehicles.Where(x => x.IsMotorcycle == IsMotorcycle && LosSantosRED.PlayerWantedLevel >= x.MinWantedLevelSpawn && LosSantosRED.PlayerWantedLevel <= x.MaxWantedLevelSpawn).ToList();
    //    int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance);
    //    int RandomPick = LosSantosRED.MyRand.Next(0, Total);
    //    foreach (VehicleInformation Vehicle in ToPickFrom)
    //    {
    //        int SpawnChance = Vehicle.CurrentSpawnChance;
    //        if (RandomPick < SpawnChance)
    //        {
    //            return Vehicle;
    //        }
    //        RandomPick -= SpawnChance;
    //    }
    //    return null;
    //}
}
[Serializable()]
public class Zone
{
    public Zone()
    {

    }
    public Zone(string _GameName, string _TextName, string _ScannerValue,County _ZoneCounty, string _AreaName)
    {
        GameName = _GameName;
        TextName = _TextName;
        ScannerValue = _ScannerValue;
        ZoneCounty = _ZoneCounty;
        AreaName = _AreaName;
    }
    public Zone(string _GameName, string _TextName, string _ScannerValue, County _ZoneCounty, string _AreaName, List<string> _DispatchUnitAudio, string _DispatchUnitName)
    {
        GameName = _GameName;
        TextName = _TextName;
        ScannerValue = _ScannerValue;
        ZoneCounty = _ZoneCounty;
        AreaName = _AreaName;
        DispatchUnitAudio = _DispatchUnitAudio;
        DispatchUnitName = _DispatchUnitName;
    }
    public string DispatchUnitName { get; set; }
    public string GameName { get; set; }
    public string TextName { get; set; }
    public string AreaName { get; set; }
    public County ZoneCounty { get; set; }
    public List<string> DispatchUnitAudio { get; set; } = new List<string>();
    public string ScannerValue { get; set; }
    public List<ZoneAgency> ZoneAgencies { get; set; }
    public bool IsRestrictedDuringWanted { get; set; } = false;
    public Agency GetRandomAgency()
    {
        if (ZoneAgencies == null || !ZoneAgencies.Any())
            return null;

        List<ZoneAgency> ToPickFrom = ZoneAgencies.Where(x => x.CanCurrentlySpawn).ToList();
        int Total = ToPickFrom.Sum(x => x.CurrentSpawnChance);
        int RandomPick = General.MyRand.Next(0, Total);
        foreach (ZoneAgency ZA in ToPickFrom)
        {
            int SpawnChance = ZA.CurrentSpawnChance;
            if (RandomPick < SpawnChance)
            {
                return ZA.AssociatedAgency;
            }
            RandomPick -= SpawnChance;
        }
        return null;
    }

    public Agency MainZoneAgency
    {
        get
        {
            if (HasAgencies)
                return ZoneAgencies.OrderBy(x => x.Priority).FirstOrDefault().AssociatedAgency;
            else
                return null;
        }
    }
    public bool HasAgencies
    {
        get
        {
            if (ZoneAgencies != null && ZoneAgencies.Any())
                return true;
            else
                return false;
        }
    }
}
public class ZoneAgency
{
    public string AssociatedAgencyName;
    public int Priority;
    public int AmbientSpawnChance = 0;
    public int WantedSpawnChance = 0;
    public ZoneAgency()
    {

    }
    public ZoneAgency(string associatedAgencyName, int priority, int ambientSpawnChance, int wantedSpawnChance)
    {
        AssociatedAgencyName = associatedAgencyName;
        Priority = priority;
        AmbientSpawnChance = ambientSpawnChance;
        WantedSpawnChance = wantedSpawnChance;
    }
    public bool CanCurrentlySpawn
    {
        get
        {
            if (PlayerState.PlayerIsWanted)
            {
                if (AssociatedAgency.CanSpawn)
                {
                    return WantedSpawnChance > 0;
                }
                else
                    return false;
            }
            else
                return AmbientSpawnChance > 0;
        }
    }
    public int CurrentSpawnChance
    {
        get
        {
            if (PlayerState.PlayerIsWanted)
                return WantedSpawnChance;
            else
                return AmbientSpawnChance;
        }
    }
    public Agency AssociatedAgency
    {
        get
        {
           return Agencies.AgenciesList.Where(x => x.Initials == AssociatedAgencyName).FirstOrDefault();
        }
    }
}
public enum County
{
    CityOfLosSantos = 0,
    LosSantosCounty = 1,
    BlaineCounty = 2,
    PacificOcean = 3,
}
