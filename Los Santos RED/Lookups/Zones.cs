using ExtensionsMethods;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

public static class Zones
{
    public static Zone AIRP;
    public static Zone ALAMO;
    public static Zone ALTA;
    public static Zone ARMYB;
    public static Zone BANHAMC;
    public static Zone BANNING;
    public static Zone BAYTRE;
    public static Zone BEACH;
    public static Zone BHAMCA;
    public static Zone BRADP;
    public static Zone BRADT;
    public static Zone BURTON;
    public static Zone CALAFB;
    public static Zone CANNY;
    public static Zone CCREAK;
    public static Zone CHAMH;
    public static Zone CHIL;
    public static Zone CHU;
    public static Zone CMSW;
    public static Zone CYPRE;
    public static Zone DAVIS;
    public static Zone DELBE;
    public static Zone DELPE;
    public static Zone DELSOL;
    public static Zone DESRT;
    public static Zone DOWNT;
    public static Zone DTVINE;
    public static Zone EAST_V;
    public static Zone EBURO;
    public static Zone ELGORL;
    public static Zone ELYSIAN;
    public static Zone GALFISH;
    public static Zone GOLF;
    public static Zone GRAPES;
    public static Zone GREATC;
    public static Zone HARMO;
    public static Zone HAWICK;
    public static Zone HORS;
    public static Zone HUMLAB;
    public static Zone JAIL;
    public static Zone KOREAT;
    public static Zone LACT;
    public static Zone LAGO;
    public static Zone LDAM;
    public static Zone LEGSQU;
    public static Zone LMESA;
    public static Zone LOSPUER;
    public static Zone MIRR;
    public static Zone MORN;
    public static Zone MOVIE;
    public static Zone MTCHIL;
    public static Zone MTGORDO;
    public static Zone MTJOSE;
    public static Zone MURRI;
    public static Zone NCHU;
    public static Zone NOOSE;
    public static Zone OCEANA;
    public static Zone PALCOV;
    public static Zone PALETO;
    public static Zone PALFOR;
    public static Zone PALHIGH;
    public static Zone PALMPOW;
    public static Zone PBLUFF;
    public static Zone PBOX;
    public static Zone PROCOB;
    public static Zone RANCHO;
    public static Zone RGLEN;
    public static Zone RICHM;
    public static Zone ROCKF;
    public static Zone RTRAK;
    public static Zone SANAND;
    public static Zone SANCHIA;
    public static Zone SANDY;
    public static Zone SKID;
    public static Zone SLAB;
    public static Zone STAD;
    public static Zone STRAW;
    public static Zone TATAMO;
    public static Zone TERMINA;
    public static Zone TEXTI;
    public static Zone TONGVAH;
    public static Zone TONGVAV;
    public static Zone VCANA;
    public static Zone VESP;
    public static Zone VINE;
    public static Zone WINDF;
    public static Zone WVINE;
    public static Zone ZANCUDO;
    public static Zone ZP_ORT;
    public static Zone ZQ_UAR;
    public static Zone UNK_LSCITY;
    public static Zone UNK_LSCOUNTY;
    public static Zone UNK_BLANIECOUNTY;
    public static List<Zone> ZoneList;
    public static void Initialize()
    {

        //One Off
        AIRP = new Zone("AIRP", "Los Santos International Airport", ScannerAudio.areas.LosSantosInternationalAirport.FileName, County.CityOfLosSantos) { ZoneAgencies = new List<ZoneAgency>() { new ZoneAgency(Agencies.LSIAPD, 0, true, true), new ZoneAgency(Agencies.LSPD, 1, false, false) } };
        OCEANA = new Zone("OCEANA", "Pacific Ocean", ScannerAudio.areas.TheOcean.FileName, County.PacificOcean) { ZoneAgencies = new List<ZoneAgency>() { new ZoneAgency(Agencies.SACG, 0, true, false), new ZoneAgency(Agencies.LSPD, 1, false, false) } };

        //Blaine
        List<ZoneAgency> StandardBlaineAgencies = new List<ZoneAgency>() { new ZoneAgency(Agencies.BCSO, 0, true, true), new ZoneAgency(Agencies.DOA, 1, false, true), new ZoneAgency(Agencies.LSSD, 1, false, false) };
        List<ZoneAgency> BlaineParkRangerAgencies = new List<ZoneAgency>() { new ZoneAgency(Agencies.SAPR, 0, true, true), new ZoneAgency(Agencies.BCSO, 1, false, true), new ZoneAgency(Agencies.LSSD, 1, false, false) };
        List<ZoneAgency> BlainePrisonAgencies = new List<ZoneAgency>() { new ZoneAgency(Agencies.SASPA, 0, true, true), new ZoneAgency(Agencies.BCSO, 1, false, true), new ZoneAgency(Agencies.LSSD, 1, false, false) };

        ALAMO = new Zone("ALAMO", "Alamo Sea", ScannerAudio.areas.TheAlamaSea.FileName, County.BlaineCounty) { ZoneAgencies = StandardBlaineAgencies };
        ARMYB = new Zone("ARMYB", "Fort Zancudo", ScannerAudio.areas.FtZancudo.FileName, County.BlaineCounty) { ZoneAgencies = new List<ZoneAgency>() { new ZoneAgency(Agencies.ARMY, 0, true, false), new ZoneAgency(Agencies.BCSO, 1, false, false), new ZoneAgency(Agencies.LSSD, 1, false, false) } };//, Agencies.BCSO, new List<Agency>() { Agencies.DOA });
        BRADP = new Zone("BRADP", "Braddock Pass", ScannerAudio.areas.BraddockPass.FileName, County.BlaineCounty) { ZoneAgencies = StandardBlaineAgencies };
        BRADT = new Zone("BRADT", "Braddock Tunnel", ScannerAudio.areas.TheBraddockTunnel.FileName, County.BlaineCounty) { ZoneAgencies = StandardBlaineAgencies };
        CALAFB = new Zone("CALAFB", "Calafia Bridge", "", County.BlaineCounty) { ZoneAgencies = BlaineParkRangerAgencies };
        CANNY = new Zone("CANNY", "Raton Canyon", ScannerAudio.areas.RatonCanyon.FileName, County.BlaineCounty) { ZoneAgencies = StandardBlaineAgencies };
        CCREAK = new Zone("CCREAK", "Cassidy Creek", "", County.BlaineCounty) { ZoneAgencies = BlaineParkRangerAgencies };
        CMSW = new Zone("CMSW", "Chiliad Mountain State Wilderness", ScannerAudio.areas.ChilliadMountainStWilderness.FileName, County.BlaineCounty) { ZoneAgencies = BlaineParkRangerAgencies };
        DESRT = new Zone("DESRT", "Grand Senora Desert", ScannerAudio.areas.GrandeSonoranDesert.FileName, County.BlaineCounty) { ZoneAgencies = StandardBlaineAgencies };
        ELGORL = new Zone("ELGORL", "El Gordo Lighthouse", ScannerAudio.areas.MountGordo.FileName, County.BlaineCounty) { ZoneAgencies = StandardBlaineAgencies };
        GRAPES = new Zone("GRAPES", "Grapeseed", ScannerAudio.areas.Grapeseed.FileName, County.BlaineCounty) { ZoneAgencies = StandardBlaineAgencies };
        HUMLAB = new Zone("HUMLAB", "Humane Labs and Research", "", County.BlaineCounty) { ZoneAgencies = StandardBlaineAgencies };
        JAIL = new Zone("JAIL", "Bolingbroke Penitentiary", ScannerAudio.areas.BoilingBrookPenitentiary.FileName, County.BlaineCounty) { ZoneAgencies = BlainePrisonAgencies };
        LAGO = new Zone("LAGO", "Lago Zancudo", ScannerAudio.areas.LagoZancudo.FileName, County.BlaineCounty) { ZoneAgencies = StandardBlaineAgencies };
        MTCHIL = new Zone("MTCHIL", "Mount Chiliad", ScannerAudio.areas.MountChiliad.FileName, County.BlaineCounty) { ZoneAgencies = BlaineParkRangerAgencies };
        MTGORDO = new Zone("MTGORDO", "Mount Gordo", ScannerAudio.areas.MountGordo.FileName, County.BlaineCounty) { ZoneAgencies = BlaineParkRangerAgencies };
        MTJOSE = new Zone("MTJOSE", "Mount Josiah", ScannerAudio.areas.MtJosiah.FileName, County.BlaineCounty) { ZoneAgencies = StandardBlaineAgencies };
        NCHU = new Zone("NCHU", "North Chumash", ScannerAudio.areas.NorthChumash.FileName, County.BlaineCounty) { ZoneAgencies = StandardBlaineAgencies };
        PALCOV = new Zone("PALCOV", "Paleto Cove", ScannerAudio.areas.PaletoBay.FileName, County.BlaineCounty) { ZoneAgencies = StandardBlaineAgencies };
        PALETO = new Zone("PALETO", "Paleto Bay", ScannerAudio.areas.PaletoBay.FileName, County.BlaineCounty, new List<string>() { ScannerAudio.attention_all_area_units.PaletoaBayUnits.FileName, ScannerAudio.attention_all_area_units.PaletoBayUnits.FileName, ScannerAudio.attention_all_area_units.PaletoBayUnits3.FileName }, "Paleto Bay Units") { ZoneAgencies = StandardBlaineAgencies };
        PALFOR = new Zone("PALFOR", "Paleto Forest", ScannerAudio.areas.PaletoForest.FileName, County.BlaineCounty) { ZoneAgencies = BlaineParkRangerAgencies };
        PROCOB = new Zone("PROCOB", "Procopio Beach", ScannerAudio.areas.ProcopioBeach.FileName, County.BlaineCounty) { ZoneAgencies = StandardBlaineAgencies };
        SANCHIA = new Zone("SANCHIA", "San Chianski Mountain Range", "", County.BlaineCounty) { ZoneAgencies = BlaineParkRangerAgencies };
        SANDY = new Zone("SANDY", "Sandy Shores", ScannerAudio.areas.SandyShores.FileName, County.BlaineCounty, new List<string>() { ScannerAudio.attention_all_area_units.SandyShoreUnits.FileName, ScannerAudio.attention_all_area_units.SandyShoresUnits2.FileName, ScannerAudio.attention_all_area_units.SanyShoreUnits3.FileName }, "Sandy Shores Units") { ZoneAgencies = StandardBlaineAgencies };
        SLAB = new Zone("SLAB", "Slab City", ScannerAudio.areas.SlabCity.FileName, County.BlaineCounty) { ZoneAgencies = StandardBlaineAgencies };
        ZANCUDO = new Zone("ZANCUDO", "Zancudo River", ScannerAudio.areas.ZancudoRiver.FileName, County.BlaineCounty, new List<string>() { ScannerAudio.attention_all_area_units.ZancudoRiverUnits.FileName }, "Zancudo River Units") { ZoneAgencies = BlaineParkRangerAgencies };
        ZQ_UAR = new Zone("ZQ_UAR", "Davis Quartz", ScannerAudio.areas.DavisCourts.FileName, County.BlaineCounty) { ZoneAgencies = StandardBlaineAgencies };


        List<ZoneAgency> StandardCityAgencies = new List<ZoneAgency>() { new ZoneAgency(Agencies.LSPD, 0, true, true), new ZoneAgency(Agencies.FIB, 1, false, true), new ZoneAgency(Agencies.DOA, 1, false, true), new ZoneAgency(Agencies.LSSDCH, 2, false, false) };
        //Vespucci
        List<string> VespucciAreaUnits = new List<string>() { ScannerAudio.attention_all_area_units.VespucciAreaUnits.FileName, ScannerAudio.attention_all_area_units.VespucciAreaUnits2.FileName };
        List<ZoneAgency> VespucciAgencies = new List<ZoneAgency>() { new ZoneAgency(Agencies.VPPD, 0, true, true), new ZoneAgency(Agencies.LSPD, 1, false, true), new ZoneAgency(Agencies.FIB, 1, false, true) };
        BEACH = new Zone("BEACH", "Vespucci Beach", ScannerAudio.areas.VespucciBeach.FileName, County.CityOfLosSantos, VespucciAreaUnits, "Vespucci Area Units") { ZoneAgencies = VespucciAgencies };
        DELBE = new Zone("DELBE", "Del Perro Beach", ScannerAudio.areas.DelPierroBeach.FileName, County.CityOfLosSantos, VespucciAreaUnits, "Vespucci Area Units") { ZoneAgencies = VespucciAgencies };
        DELPE = new Zone("DELPE", "Del Perro", ScannerAudio.areas.DelPierro.FileName, County.CityOfLosSantos, VespucciAreaUnits, "Vespucci Area Units") { ZoneAgencies = VespucciAgencies };
        VCANA = new Zone("VCANA", "Vespucci Canals", ScannerAudio.areas.VespucciCanal.FileName, County.CityOfLosSantos, VespucciAreaUnits, "Vespucci Area Units") { ZoneAgencies = VespucciAgencies };
        VESP = new Zone("VESP", "Vespucci", ScannerAudio.areas.Vespucci.FileName, County.CityOfLosSantos, VespucciAreaUnits, "Vespucci Area Units") { ZoneAgencies = VespucciAgencies };
        LOSPUER = new Zone("LOSPUER", "La Puerta", ScannerAudio.areas.LaPuertes.FileName, County.CityOfLosSantos, VespucciAreaUnits, "Vespucci Area Units") { ZoneAgencies = VespucciAgencies };//Was LSPD
        PBLUFF = new Zone("PBLUFF", "Pacific Bluffs", ScannerAudio.areas.PacificBluffs.FileName, County.CityOfLosSantos, VespucciAreaUnits, "Vespucci Area Units") { ZoneAgencies = VespucciAgencies };//Was County


        //Central
        List<string> CentralAreaUnits = new List<string>() { ScannerAudio.attention_all_area_units.CentralUnits.FileName, ScannerAudio.attention_all_area_units.CentralUnits1.FileName, ScannerAudio.attention_all_area_units.CentralAreaUnits.FileName };
        BANNING = new Zone("BANNING", "Banning", ScannerAudio.areas.Banning.FileName, County.CityOfLosSantos, CentralAreaUnits, "Central Area Units") { ZoneAgencies = StandardCityAgencies };
        CHAMH = new Zone("CHAMH", "Chamberlain Hills", ScannerAudio.areas.ChamberlainHills.FileName, County.CityOfLosSantos, CentralAreaUnits, "Central Area Units") { ZoneAgencies = StandardCityAgencies };
        DAVIS = new Zone("DAVIS", "Davis", ScannerAudio.areas.Davis.FileName, County.CityOfLosSantos, CentralAreaUnits, "Central Area Units") { ZoneAgencies = StandardCityAgencies };
        DOWNT = new Zone("DOWNT", "Downtown", ScannerAudio.areas.Downtown.FileName, County.CityOfLosSantos, CentralAreaUnits, "Central Area Units") { ZoneAgencies = StandardCityAgencies };
        PBOX = new Zone("PBOX", "Pillbox Hill", ScannerAudio.areas.PillboxHill.FileName, County.CityOfLosSantos, CentralAreaUnits, "Central Area Units") { ZoneAgencies = StandardCityAgencies };
        RANCHO = new Zone("RANCHO", "Rancho", ScannerAudio.areas.Rancho.FileName, County.CityOfLosSantos, CentralAreaUnits, "Central Area Units") { ZoneAgencies = StandardCityAgencies };
        SKID = new Zone("SKID", "Mission Row", ScannerAudio.areas.MissionRow.FileName, County.CityOfLosSantos, CentralAreaUnits, "Central Area Units") { ZoneAgencies = StandardCityAgencies };
        STAD = new Zone("STAD", "Maze Bank Arena", ScannerAudio.areas.MazeBankArena.FileName, County.CityOfLosSantos, CentralAreaUnits, "Central Area Units") { ZoneAgencies = StandardCityAgencies };
        STRAW = new Zone("STRAW", "Strawberry", ScannerAudio.areas.Strawberry.FileName, County.CityOfLosSantos, CentralAreaUnits, "Central Area Units") { ZoneAgencies = StandardCityAgencies };
        TEXTI = new Zone("TEXTI", "Textile City", ScannerAudio.areas.TextileCity.FileName, County.CityOfLosSantos, CentralAreaUnits, "Central Area Units") { ZoneAgencies = StandardCityAgencies };
        LEGSQU = new Zone("LEGSQU", "Legion Square", "", County.CityOfLosSantos, CentralAreaUnits, "Central Area Units") { ZoneAgencies = StandardCityAgencies };

        //EastLosSantos
        List<string> EastLosSantosAreaUnits = new List<string>() { ScannerAudio.attention_all_area_units.EastLosSantosUnits1.FileName, ScannerAudio.attention_all_area_units.EastLosSantosUnits1.FileName, ScannerAudio.attention_all_area_units.EastLosSantos2.FileName };
        List<ZoneAgency> EastLosSantosAgencies = new List<ZoneAgency>() { new ZoneAgency(Agencies.LSPDELS, 0, true, true), new ZoneAgency(Agencies.LSPD, 1, false, true), new ZoneAgency(Agencies.FIB, 1, false, true) };
        CYPRE = new Zone("CYPRE", "Cypress Flats", ScannerAudio.areas.CypressFlats.FileName, County.CityOfLosSantos, EastLosSantosAreaUnits, "East Los Santos Units") { ZoneAgencies = EastLosSantosAgencies };
        LMESA = new Zone("LMESA", "La Mesa", ScannerAudio.areas.LaMesa.FileName, County.CityOfLosSantos, EastLosSantosAreaUnits, "East Los Santos Units") { ZoneAgencies = EastLosSantosAgencies };
        MIRR = new Zone("MIRR", "Mirror Park", ScannerAudio.areas.MirrorPark.FileName, County.CityOfLosSantos, EastLosSantosAreaUnits, "East Los Santos Units") { ZoneAgencies = EastLosSantosAgencies };
        MURRI = new Zone("MURRI", "Murrieta Heights", ScannerAudio.areas.MuriettaHeights.FileName, County.CityOfLosSantos, EastLosSantosAreaUnits, "East Los Santos Units") { ZoneAgencies = EastLosSantosAgencies };
        EBURO = new Zone("EBURO", "El Burro Heights", ScannerAudio.areas.ElBerroHights.FileName, County.CityOfLosSantos, EastLosSantosAreaUnits, "East Los Santos Units") { ZoneAgencies = EastLosSantosAgencies };//was county

        //Vinewood
        List<string> VinewoodAreaUnits = new List<string>() { ScannerAudio.attention_all_area_units.VinewoodAreaUnits.FileName, ScannerAudio.attention_all_area_units.VinewoodUnits2.FileName };
        List<ZoneAgency> VinewoodAgencies = new List<ZoneAgency>() { new ZoneAgency(Agencies.LSPDVW, 0, true, true), new ZoneAgency(Agencies.FIB, 1, false, true), new ZoneAgency(Agencies.DOA, 1, false, true), new ZoneAgency(Agencies.LSSDVW, 2, false, false) };
        ALTA = new Zone("ALTA", "Alta", ScannerAudio.areas.Alta.FileName, County.CityOfLosSantos, VinewoodAreaUnits, "Vinewood Units") { ZoneAgencies = VinewoodAgencies };
        DTVINE = new Zone("DTVINE", "Downtown Vinewood", ScannerAudio.areas.DowntownVinewood.FileName, County.CityOfLosSantos, VinewoodAreaUnits, "Vinewood Units") { ZoneAgencies = VinewoodAgencies };
        EAST_V = new Zone("EAST_V", "East Vinewood", ScannerAudio.areas.EastVinewood.FileName, County.CityOfLosSantos, VinewoodAreaUnits, "Vinewood Units") { ZoneAgencies = VinewoodAgencies };
        HAWICK = new Zone("HAWICK", "Hawick", "", County.CityOfLosSantos, VinewoodAreaUnits, "Vinewood Units") { ZoneAgencies = VinewoodAgencies };
        HORS = new Zone("HORS", "Vinewood Racetrack", ScannerAudio.areas.TheRaceCourse.FileName, County.CityOfLosSantos, VinewoodAreaUnits, "Vinewood Units") { ZoneAgencies = VinewoodAgencies };
        VINE = new Zone("VINE", "Vinewood", ScannerAudio.areas.Vinewood.FileName, County.CityOfLosSantos, VinewoodAreaUnits, "Vinewood Units") { ZoneAgencies = VinewoodAgencies };
        WVINE = new Zone("WVINE", "West Vinewood", ScannerAudio.areas.WestVinewood.FileName, County.CityOfLosSantos, VinewoodAreaUnits, "Vinewood Units") { ZoneAgencies = VinewoodAgencies };

        //PortOfLosSantos
        List<ZoneAgency> PortAgencies = new List<ZoneAgency>() { new ZoneAgency(Agencies.LSPA, 0, true, true), new ZoneAgency(Agencies.LSPD, 1, false, false) };
        List<string> PortAreaUnits = new List<string>() { ScannerAudio.attention_all_area_units.PortOfLosSantosUnits.FileName, ScannerAudio.attention_all_area_units.PortOfLosSantosUnits1.FileName, ScannerAudio.attention_all_area_units.PortOfLosSantosUnits2.FileName };
        ELYSIAN = new Zone("ELYSIAN", "Elysian Island", ScannerAudio.areas.ElysianIsland.FileName, County.CityOfLosSantos, PortAreaUnits, "Port of Los Santos Units") { ZoneAgencies = PortAgencies };
        ZP_ORT = new Zone("ZP_ORT", "Port of South Los Santos", ScannerAudio.areas.PortOfSouthLosSantos.FileName, County.CityOfLosSantos, PortAreaUnits, "Port of Los Santos Units") { ZoneAgencies = PortAgencies };
        TERMINA = new Zone("TERMINA", "Terminal", ScannerAudio.areas.Terminal.FileName, County.CityOfLosSantos, PortAreaUnits, "Port of Los Santos Units") { ZoneAgencies = PortAgencies };
        ZP_ORT = new Zone("ZP_ORT", "Port of South Los Santos", ScannerAudio.areas.PortOfSouthLosSantos.FileName, County.CityOfLosSantos, PortAreaUnits, "Port of Los Santos Units") { ZoneAgencies = PortAgencies };

        //Rockford Hills
        List<ZoneAgency> RockfordHillsAgencies = new List<ZoneAgency>() { new ZoneAgency(Agencies.RHPD, 0, true, true), new ZoneAgency(Agencies.FIB, 1, false, true), new ZoneAgency(Agencies.LSPD, 2, false, false) };
        BURTON = new Zone("BURTON", "Burton", ScannerAudio.areas.Burton.FileName, County.CityOfLosSantos) { ZoneAgencies = RockfordHillsAgencies };
        GOLF = new Zone("GOLF", "GWC and Golfing Society", ScannerAudio.areas.TheGWCGolfingSociety.FileName, County.CityOfLosSantos) { ZoneAgencies = RockfordHillsAgencies };
        KOREAT = new Zone("KOREAT", "Little Seoul", ScannerAudio.areas.LittleSeoul.FileName, County.CityOfLosSantos) { ZoneAgencies = RockfordHillsAgencies };
        MORN = new Zone("MORN", "Morningwood", ScannerAudio.areas.MorningWood.FileName, County.CityOfLosSantos) { ZoneAgencies = RockfordHillsAgencies };
        MOVIE = new Zone("MOVIE", "Richards Majestic", ScannerAudio.areas.RichardsMajesticStudio.FileName, County.CityOfLosSantos) { ZoneAgencies = RockfordHillsAgencies };
        RICHM = new Zone("RICHM", "Richman", ScannerAudio.areas.Richman.FileName, County.CityOfLosSantos) { ZoneAgencies = RockfordHillsAgencies };
        ROCKF = new Zone("ROCKF", "Rockford Hills", ScannerAudio.areas.RockfordHills.FileName, County.CityOfLosSantos) { ZoneAgencies = RockfordHillsAgencies };


        //County
        //Vespucci
        //EastLosSantos
        //Vinewood Hills
        List<string> VinewoodHillsAreaUnits = new List<string>() { ScannerAudio.attention_all_area_units.VinewoodAreaUnits.FileName, ScannerAudio.attention_all_area_units.VinewoodUnits2.FileName };
        List<ZoneAgency> VinewoodHillsAgencies = new List<ZoneAgency>() { new ZoneAgency(Agencies.LSSDVW, 0, true, true), new ZoneAgency(Agencies.DOA, 1, false, true) };  
        CHIL = new Zone("CHIL", "Vinewood Hills", ScannerAudio.areas.VinewoodHills.FileName, County.LosSantosCounty, VinewoodHillsAreaUnits, "Vinewood Hills Units") { ZoneAgencies = VinewoodHillsAgencies };
        GREATC = new Zone("GREATC", "Great Chaparral", ScannerAudio.areas.GreatChapparalle.FileName, County.LosSantosCounty, VinewoodHillsAreaUnits, "Vinewood Hills Units") { ZoneAgencies = VinewoodHillsAgencies };
        RGLEN = new Zone("RGLEN", "Richman Glen", ScannerAudio.areas.RichmanGlenn.FileName, County.LosSantosCounty, VinewoodHillsAreaUnits, "Vinewood Hills Units") { ZoneAgencies = VinewoodHillsAgencies };
        TONGVAV = new Zone("TONGVAV", "Tongva Valley", ScannerAudio.areas.TongvaValley.FileName, County.LosSantosCounty, VinewoodHillsAreaUnits, "Vinewood Hills Units") { ZoneAgencies = VinewoodHillsAgencies };
        //Gallilee
        GALFISH = new Zone("GALFISH", "Galilee", ScannerAudio.areas.GalileoPark.FileName, County.LosSantosCounty);// Agencies.SAPR, null, new List<string>() { ScannerAudio.attention_all_area_units.GalilleUnits.FileName }, "Galille Units");
        //Chumash
        List<string> ChumashsAreaUnits = new List<string>() { ScannerAudio.attention_all_area_units.ChumashUnits.FileName, ScannerAudio.attention_all_area_units.ChumashUnits1.FileName, ScannerAudio.attention_all_area_units.ChumashUnits2.FileName };
        List<ZoneAgency> ChumashAgencies = new List<ZoneAgency>() { new ZoneAgency(Agencies.LSSDCH, 0, true, true), new ZoneAgency(Agencies.DOA, 1, false, true), new ZoneAgency(Agencies.LSPDCH, 2, false, true) };
        BANHAMC = new Zone("BANHAMC", "Banham Canyon Dr", "", County.LosSantosCounty, ChumashsAreaUnits, "Chumash Units") { ZoneAgencies = ChumashAgencies };
        BHAMCA = new Zone("BHAMCA", "Banham Canyon", "", County.LosSantosCounty, ChumashsAreaUnits, "Chumash Units") { ZoneAgencies = ChumashAgencies };
        CHU = new Zone("CHU", "Chumash", ScannerAudio.areas.Chumash.FileName, County.LosSantosCounty, ChumashsAreaUnits, "Chumash Units") { ZoneAgencies = ChumashAgencies };
        TONGVAH = new Zone("TONGVAH", "Tongva Hills", ScannerAudio.areas.TongaHills.FileName, County.LosSantosCounty, ChumashsAreaUnits, "Chumash Units") { ZoneAgencies = ChumashAgencies };
        //Other
        List<ZoneAgency> StandardSheriffAgencies = new List<ZoneAgency>() { new ZoneAgency(Agencies.LSSD, 0, true, true), new ZoneAgency(Agencies.DOA, 1, false, true) };
        BAYTRE = new Zone("BAYTRE", "Baytree Canyon", ScannerAudio.areas.BayTreeCanyon.FileName, County.LosSantosCounty) { ZoneAgencies = StandardSheriffAgencies };
        DELSOL = new Zone("DELSOL", "Puerto Del Sol", ScannerAudio.areas.LaPorta.FileName, County.LosSantosCounty) { ZoneAgencies = StandardSheriffAgencies };
        HARMO = new Zone("HARMO", "Harmony", ScannerAudio.areas.Harmony.FileName, County.LosSantosCounty) { ZoneAgencies = StandardSheriffAgencies };
        LACT = new Zone("LACT", "Land Act Reservoir", "", County.LosSantosCounty) { ZoneAgencies = StandardSheriffAgencies };
        LDAM = new Zone("LDAM", "Land Act Dam", "", County.LosSantosCounty) { ZoneAgencies = StandardSheriffAgencies };
        NOOSE = new Zone("NOOSE", "N.O.O.S.E", "", County.LosSantosCounty) { ZoneAgencies = StandardSheriffAgencies };
        PALHIGH = new Zone("PALHIGH", "Palomino Highlands", ScannerAudio.areas.PalominoHighlands.FileName, County.LosSantosCounty) { ZoneAgencies = StandardSheriffAgencies };
        PALMPOW = new Zone("PALMPOW", "Palmer - Taylor Power Station", ScannerAudio.areas.PalmerTaylorPowerStation.FileName, County.LosSantosCounty) { ZoneAgencies = StandardSheriffAgencies };
        RTRAK = new Zone("RTRAK", "Redwood Lights Track", ScannerAudio.areas.TheRedwoodLightsTrack.FileName, County.LosSantosCounty) { ZoneAgencies = StandardSheriffAgencies };
        SANAND = new Zone("SANAND", "San Andreas", ScannerAudio.areas.SanAndreas.FileName, County.LosSantosCounty) { ZoneAgencies = StandardSheriffAgencies };
        TATAMO = new Zone("TATAMO", "Tataviam Mountains", ScannerAudio.areas.TatathiaMountains.FileName, County.LosSantosCounty) { ZoneAgencies = StandardSheriffAgencies };
        WINDF = new Zone("WINDF", "Ron Alternates Wind Farm", ScannerAudio.areas.RonAlternatesWindFarm.FileName, County.LosSantosCounty) { ZoneAgencies = StandardSheriffAgencies };
        //Unknown
        UNK_LSCOUNTY = new Zone("UNK_LSCOUNTY", "Los Santos County", "", County.LosSantosCounty) { ZoneAgencies = StandardCityAgencies };
        UNK_BLANIECOUNTY = new Zone("UNK_BLANIECOUNTY", "Blaine County", "", County.BlaineCounty) { ZoneAgencies = StandardBlaineAgencies };
        UNK_LSCITY = new Zone("UNK_LSCITY", "Los Santos", "", County.CityOfLosSantos) { ZoneAgencies = StandardSheriffAgencies };

        ZoneList = new List<Zone>
        {
            AIRP,
            ALAMO,
            ALTA,
            ARMYB,
            BANHAMC,
            BANNING,
            BAYTRE,
            BEACH,
            BHAMCA,
            BRADP,
            BRADT,
            BURTON,
            CALAFB,
            CANNY,
            CCREAK,
            CHAMH,
            CHIL,
            CHU,
            CMSW,
            CYPRE,
            DAVIS,
            DELBE,
            DELPE,
            DELSOL,
            DESRT,
            DOWNT,
            DTVINE,
            EAST_V,
            EBURO,
            ELGORL,
            ELYSIAN,
            GALFISH,
            GOLF,
            GRAPES,
            GREATC,
            HARMO,
            HAWICK,
            HORS,
            HUMLAB,
            JAIL,
            KOREAT,
            LACT,
            LAGO,
            LDAM,
            LEGSQU,
            LMESA,
            LOSPUER,
            MIRR,
            MORN,
            MOVIE,
            MTCHIL,
            MTGORDO,
            MTJOSE,
            MURRI,
            NCHU,
            NOOSE,
            OCEANA,
            PALCOV,
            PALETO,
            PALFOR,
            PALHIGH,
            PALMPOW,
            PBLUFF,
            PBOX,
            PROCOB,
            RANCHO,
            RGLEN,
            RICHM,
            ROCKF,
            RTRAK,
            SANAND,
            SANCHIA,
            SANDY,
            SKID,
            SLAB,
            STAD,
            STRAW,
            TATAMO,
            TERMINA,
            TEXTI,
            TONGVAH,
            TONGVAV,
            VCANA,
            VESP,
            VINE,
            WINDF,
            WVINE,
            ZANCUDO,
            ZP_ORT,
            ZQ_UAR
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
            if (Game.LocalPlayer.Character.IsInLosSantosCity())
                return UNK_LSCITY;
            else
                return UNK_LSCOUNTY;
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
    public static string GetFormattedZoneName(Zone MyZone)
    {
        string CountyName = "San Andreas";
        if (MyZone.ZoneCounty == County.BlaineCounty)
            CountyName = "Blaine County";
        else if (MyZone.ZoneCounty == County.CityOfLosSantos)
            CountyName = "City of Los Santos";
        else if (MyZone.ZoneCounty == County.LosSantosCounty)
            CountyName = "Los Santos County";

        return MyZone.TextName + ", " + CountyName;
    }
    public static Agency GetCountyAgencyByZone(Zone MyZone)
    {
        if (MyZone.ZoneCounty == County.BlaineCounty)
        {
            return Agencies.BCSO;
        }
        else if (MyZone.ZoneCounty == County.LosSantosCounty)
        {
            return Agencies.LSSD;
        }
        else if (MyZone.ZoneCounty == County.CityOfLosSantos)
        {
            return Agencies.LSPD;
        }
        else if (MyZone.ZoneCounty == County.PacificOcean)
        {
            return Agencies.LSPD;
        }
        else
        {
            return Agencies.LSPD;
        }
    }

}
public class Zone
{
    public Zone(string _GameName, string _TextName, string _ScannerValue,County _ZoneCounty)//, Agency _MainZoneAgency)
    {
        GameName = _GameName;
        TextName = _TextName;
        ScannerValue = _ScannerValue;
        //MainZoneAgency = _MainZoneAgency;
        ZoneCounty = _ZoneCounty;
    }
    public Zone(string _GameName, string _TextName, string _ScannerValue, County _ZoneCounty, List<ZoneAgency> _ZoneAgencies)
    {
        GameName = _GameName;
        TextName = _TextName;
        ScannerValue = _ScannerValue;
        ZoneAgencies = _ZoneAgencies;
        ZoneCounty = _ZoneCounty;
    }
    public Zone(string _GameName, string _TextName, string _ScannerValue, County _ZoneCounty, List<string> _DispatchUnitAudio, string _DispatchUnitName)
    {
        GameName = _GameName;
        TextName = _TextName;
        ScannerValue = _ScannerValue;
        ZoneCounty = _ZoneCounty;
        DispatchUnitAudio = _DispatchUnitAudio;
        DispatchUnitName = _DispatchUnitName;
    }
    public string DispatchUnitName { get; set; }
    public string GameName { get; set; }
    public string TextName { get; set; }
    public County ZoneCounty { get; set; }
    public List<string> DispatchUnitAudio { get; set; } = new List<string>();
    public string ScannerValue { get; set; }
    public List<ZoneAgency> ZoneAgencies { get; set; }

    public Agency MainZoneAgency
    {
        get
        {
            if (HasAgencies)
                return ZoneAgencies.OrderBy(x => x.Priority).FirstOrDefault().AssiciatedAgency;
            else
                return Agencies.LSPD;
        }
    }
    public ZoneAgency MainZone
    {
        get
        {
            if (HasAgencies)
                return ZoneAgencies.OrderBy(x => x.Priority).FirstOrDefault();
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
    public bool HasSecondaryAgencies
    {
        get
        {
            if (ZoneAgencies != null && ZoneAgencies.Any(x => !x.IsMain))
                return true;
            else
                return false;
        }
    }
    //public List<Agency> SecondaryZoneAgencies { get; set; } = new List<Agency>();
}
public class ZoneAgency
{
    public Agency AssiciatedAgency;
    public int Priority;
    public bool IsMain = false;
    public bool CanRandomSpawn = false;

    public ZoneAgency(Agency assiciatedAgency, int priority, bool isMain, bool canRandomSpawn)
    {
        AssiciatedAgency = assiciatedAgency;
        Priority = priority;
        IsMain = isMain;
        CanRandomSpawn = canRandomSpawn;
    }
}
public enum County
{
    CityOfLosSantos = 0,
    LosSantosCounty = 1,
    BlaineCounty = 2,
    PacificOcean = 3,
}


/*
 using ExtensionsMethods;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

public static class Zones
{
    public static Zone AIRP;
    public static Zone ALAMO;
    public static Zone ALTA;
    public static Zone ARMYB;
    public static Zone BANHAMC;
    public static Zone BANNING;
    public static Zone BAYTRE;
    public static Zone BEACH;
    public static Zone BHAMCA;
    public static Zone BRADP;
    public static Zone BRADT;
    public static Zone BURTON;
    public static Zone CALAFB;
    public static Zone CANNY;
    public static Zone CCREAK;
    public static Zone CHAMH;
    public static Zone CHIL;
    public static Zone CHU;
    public static Zone CMSW;
    public static Zone CYPRE;
    public static Zone DAVIS;
    public static Zone DELBE;
    public static Zone DELPE;
    public static Zone DELSOL;
    public static Zone DESRT;
    public static Zone DOWNT;
    public static Zone DTVINE;
    public static Zone EAST_V;
    public static Zone EBURO;
    public static Zone ELGORL;
    public static Zone ELYSIAN;
    public static Zone GALFISH;
    public static Zone GOLF;
    public static Zone GRAPES;
    public static Zone GREATC;
    public static Zone HARMO;
    public static Zone HAWICK;
    public static Zone HORS;
    public static Zone HUMLAB;
    public static Zone JAIL;
    public static Zone KOREAT;
    public static Zone LACT;
    public static Zone LAGO;
    public static Zone LDAM;
    public static Zone LEGSQU;
    public static Zone LMESA;
    public static Zone LOSPUER;
    public static Zone MIRR;
    public static Zone MORN;
    public static Zone MOVIE;
    public static Zone MTCHIL;
    public static Zone MTGORDO;
    public static Zone MTJOSE;
    public static Zone MURRI;
    public static Zone NCHU;
    public static Zone NOOSE;
    public static Zone OCEANA;
    public static Zone PALCOV;
    public static Zone PALETO;
    public static Zone PALFOR;
    public static Zone PALHIGH;
    public static Zone PALMPOW;
    public static Zone PBLUFF;
    public static Zone PBOX;
    public static Zone PROCOB;
    public static Zone RANCHO;
    public static Zone RGLEN;
    public static Zone RICHM;
    public static Zone ROCKF;
    public static Zone RTRAK;
    public static Zone SANAND;
    public static Zone SANCHIA;
    public static Zone SANDY;
    public static Zone SKID;
    public static Zone SLAB;
    public static Zone STAD;
    public static Zone STRAW;
    public static Zone TATAMO;
    public static Zone TERMINA;
    public static Zone TEXTI;
    public static Zone TONGVAH;
    public static Zone TONGVAV;
    public static Zone VCANA;
    public static Zone VESP;
    public static Zone VINE;
    public static Zone WINDF;
    public static Zone WVINE;
    public static Zone ZANCUDO;
    public static Zone ZP_ORT;
    public static Zone ZQ_UAR;
    public static Zone UNK_LSCITY;
    public static Zone UNK_LSCOUNTY;
    public static Zone UNK_BLANIECOUNTY;
    public static List<Zone> ZoneList;
    public static void Initialize()
    {
        AIRP = new Zone("AIRP", "Los Santos International Airport", ScannerAudio.areas.LosSantosInternationalAirport.FileName, County.CityOfLosSantos);// Agencies.LSIAPD, new List<Agency>() { Agencies.LSPD, Agencies.IAA, Agencies.DOA, Agencies.FIB });
        ALAMO = new Zone("ALAMO", "Alamo Sea", ScannerAudio.areas.TheAlamaSea.FileName, County.BlaineCounty);// Agencies.BCSO, new List<Agency>() { Agencies.DOA });
        ALTA = new Zone("ALTA", "Alta", ScannerAudio.areas.Alta.FileName, County.CityOfLosSantos);// Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        ARMYB = new Zone("ARMYB", "Fort Zancudo", ScannerAudio.areas.FtZancudo.FileName, County.BlaineCounty);// Agencies.BCSO, new List<Agency>() { Agencies.DOA });
        BANHAMC = new Zone("BANHAMC", "Banham Canyon Dr", "", County.LosSantosCounty);// Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.ChumashUnits.FileName, ScannerAudio.attention_all_area_units.ChumashUnits1.FileName, ScannerAudio.attention_all_area_units.ChumashUnits2.FileName },"Chumash Units");
        BANNING = new Zone("BANNING", "Banning", ScannerAudio.areas.Banning.FileName, County.CityOfLosSantos);// Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.CentralUnits.FileName, ScannerAudio.attention_all_area_units.CentralUnits1.FileName, ScannerAudio.attention_all_area_units.CentralAreaUnits.FileName },"Central Area Units");
        BAYTRE = new Zone("BAYTRE", "Baytree Canyon", ScannerAudio.areas.BayTreeCanyon.FileName, County.LosSantosCounty);// Agencies.LSSD);
        BEACH = new Zone("BEACH", "Vespucci Beach", ScannerAudio.areas.VespucciBeach.FileName, County.CityOfLosSantos);// Agencies.VPPD, new List<Agency>() { Agencies.LSPD, Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.VespucciAreaUnits.FileName, ScannerAudio.attention_all_area_units.VespucciAreaUnits2.FileName }, "Vespucci Area Units");
        BHAMCA = new Zone("BHAMCA", "Banham Canyon", "", County.LosSantosCounty);// Agencies.LSSDCH, new List<Agency>() { Agencies.DOA }, new List<string>() { ScannerAudio.attention_all_area_units.ChumashUnits.FileName, ScannerAudio.attention_all_area_units.ChumashUnits1.FileName, ScannerAudio.attention_all_area_units.ChumashUnits2.FileName }, "Chumash Units");
        BRADP = new Zone("BRADP", "Braddock Pass", ScannerAudio.areas.BraddockPass.FileName, County.BlaineCounty);// Agencies.LSSD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        BRADT = new Zone("BRADT", "Braddock Tunnel", ScannerAudio.areas.TheBraddockTunnel.FileName, County.BlaineCounty);// Agencies.BCSO, new List<Agency>() { Agencies.DOA });
        BURTON = new Zone("BURTON", "Burton", ScannerAudio.areas.Burton.FileName, County.CityOfLosSantos);// Agencies.RHPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        CALAFB = new Zone("CALAFB", "Calafia Bridge", "", County.BlaineCounty);// Agencies.SAPR);
        CANNY = new Zone("CANNY", "Raton Canyon", ScannerAudio.areas.RatonCanyon.FileName, County.BlaineCounty);// Agencies.SAPR);
        CCREAK = new Zone("CCREAK", "Cassidy Creek", "", County.BlaineCounty);// Agencies.SAPR);
        CHAMH = new Zone("CHAMH", "Chamberlain Hills", ScannerAudio.areas.ChamberlainHills.FileName, County.CityOfLosSantos);// Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.CentralUnits.FileName, ScannerAudio.attention_all_area_units.CentralUnits1.FileName, ScannerAudio.attention_all_area_units.CentralAreaUnits.FileName }, "Central Area Units");
        CHIL = new Zone("CHIL", "Vinewood Hills", ScannerAudio.areas.VinewoodHills.FileName, County.LosSantosCounty);// Agencies.LSSDVW, new List<Agency>() { Agencies.DOA }, new List<string>() { ScannerAudio.attention_all_area_units.VinewoodHillsUnits.FileName, ScannerAudio.attention_all_area_units.VinewoodHillsUnits1.FileName, ScannerAudio.attention_all_area_units.VinewoodHillsUnits3.FileName }, "Vinewood Hills Units");
        CHU = new Zone("CHU", "Chumash", ScannerAudio.areas.Chumash.FileName, County.LosSantosCounty);// Agencies.LSSDCH, new List<Agency>() { Agencies.DOA }, new List<string>() { ScannerAudio.attention_all_area_units.ChumashUnits.FileName, ScannerAudio.attention_all_area_units.ChumashUnits1.FileName, ScannerAudio.attention_all_area_units.ChumashUnits2.FileName }, "Chumash Units");
        CMSW = new Zone("CMSW", "Chiliad Mountain State Wilderness", ScannerAudio.areas.ChilliadMountainStWilderness.FileName, County.BlaineCounty);// Agencies.SAPR);
        CYPRE = new Zone("CYPRE", "Cypress Flats", ScannerAudio.areas.CypressFlats.FileName, County.CityOfLosSantos);// Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.EastLosSantosUnits1.FileName, ScannerAudio.attention_all_area_units.EastLosSantosUnits1.FileName, ScannerAudio.attention_all_area_units.EastLosSantos2.FileName }, "East Los Santos Units");
        DAVIS = new Zone("DAVIS", "Davis", ScannerAudio.areas.Davis.FileName, County.CityOfLosSantos);// Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.CentralUnits.FileName, ScannerAudio.attention_all_area_units.CentralUnits1.FileName, ScannerAudio.attention_all_area_units.CentralAreaUnits.FileName }, "Central Area Units");
        DELBE = new Zone("DELBE", "Del Perro Beach", ScannerAudio.areas.DelPierroBeach.FileName, County.CityOfLosSantos);// Agencies.VPPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.VespucciAreaUnits.FileName, ScannerAudio.attention_all_area_units.VespucciAreaUnits2.FileName }, "Vespucci Area Units");
        DELPE = new Zone("DELPE", "Del Perro", ScannerAudio.areas.DelPierro.FileName, County.CityOfLosSantos);// Agencies.VPPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.VespucciAreaUnits.FileName, ScannerAudio.attention_all_area_units.VespucciAreaUnits2.FileName }, "Vespucci Area Units");
        DELSOL = new Zone("DELSOL", "Puerto Del Sol", ScannerAudio.areas.LaPorta.FileName, County.LosSantosCounty);// Agencies.VPPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });//???????;
        DESRT = new Zone("DESRT", "Grand Senora Desert", ScannerAudio.areas.GrandeSonoranDesert.FileName, County.BlaineCounty);// Agencies.BCSO, new List<Agency>() { Agencies.DOA });
        DOWNT = new Zone("DOWNT", "Downtown", ScannerAudio.areas.Downtown.FileName, County.CityOfLosSantos);// Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.CentralUnits.FileName, ScannerAudio.attention_all_area_units.CentralUnits1.FileName, ScannerAudio.attention_all_area_units.CentralAreaUnits.FileName }, "Central Area Units");
        DTVINE = new Zone("DTVINE", "Downtown Vinewood", ScannerAudio.areas.DowntownVinewood.FileName, County.CityOfLosSantos);// Agencies.LSPDVW, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.VinewoodAreaUnits.FileName, ScannerAudio.attention_all_area_units.VinewoodUnits2.FileName }, "Vinewood Units");
        EAST_V = new Zone("EAST_V", "East Vinewood", ScannerAudio.areas.EastVinewood.FileName, County.CityOfLosSantos);// Agencies.LSPDVW, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.VinewoodAreaUnits.FileName, ScannerAudio.attention_all_area_units.VinewoodUnits2.FileName }, "Vinewood Units");
        EBURO = new Zone("EBURO", "El Burro Heights", ScannerAudio.areas.ElBerroHights.FileName, County.LosSantosCounty);// Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.EastLosSantosUnits1.FileName, ScannerAudio.attention_all_area_units.EastLosSantosUnits1.FileName, ScannerAudio.attention_all_area_units.EastLosSantos2.FileName }, "East Los Santos Units");
        ELGORL = new Zone("ELGORL", "El Gordo Lighthouse", ScannerAudio.areas.MountGordo.FileName, County.BlaineCounty);// Agencies.BCSO, new List<Agency>() { Agencies.DOA });
        ELYSIAN = new Zone("ELYSIAN", "Elysian Island", ScannerAudio.areas.ElysianIsland.FileName, County.CityOfLosSantos);// Agencies.LSPA, new List<Agency>() { Agencies.LSPD, Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.PortOfLosSantosUnits.FileName, ScannerAudio.attention_all_area_units.PortOfLosSantosUnits1.FileName, ScannerAudio.attention_all_area_units.PortOfLosSantosUnits2.FileName }, "Port of Los Santos Units");
        GALFISH = new Zone("GALFISH", "Galilee", ScannerAudio.areas.GalileoPark.FileName, County.LosSantosCounty);// Agencies.SAPR, null, new List<string>() { ScannerAudio.attention_all_area_units.GalilleUnits.FileName }, "Galille Units");
        GOLF = new Zone("GOLF", "GWC and Golfing Society", ScannerAudio.areas.TheGWCGolfingSociety.FileName, County.CityOfLosSantos);// Agencies.RHPD);
        GRAPES = new Zone("GRAPES", "Grapeseed", ScannerAudio.areas.Grapeseed.FileName, County.BlaineCounty);// Agencies.BCSO, new List<Agency>() { Agencies.DOA });
        GREATC = new Zone("GREATC", "Great Chaparral", ScannerAudio.areas.GreatChapparalle.FileName, County.LosSantosCounty);// Agencies.LSSDVW, new List<Agency>() { Agencies.DOA }, new List<string>() { ScannerAudio.attention_all_area_units.VinewoodHillsUnits.FileName, ScannerAudio.attention_all_area_units.VinewoodHillsUnits1.FileName, ScannerAudio.attention_all_area_units.VinewoodHillsUnits3.FileName }, "Vinewood Hills Units");
        HARMO = new Zone("HARMO", "Harmony", ScannerAudio.areas.Harmony.FileName, County.LosSantosCounty);// Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        HAWICK = new Zone("HAWICK", "Hawick", "", County.CityOfLosSantos);// Agencies.LSPDVW, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.VinewoodAreaUnits.FileName, ScannerAudio.attention_all_area_units.VinewoodUnits2.FileName }, "Vinewood Units");
        HORS = new Zone("HORS", "Vinewood Racetrack", ScannerAudio.areas.TheRaceCourse.FileName, County.CityOfLosSantos);// Agencies.LSPDVW, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.VinewoodAreaUnits.FileName, ScannerAudio.attention_all_area_units.VinewoodUnits2.FileName }, "Vinewood Units");
        HUMLAB = new Zone("HUMLAB", "Humane Labs and Research", "", County.BlaineCounty);// Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        JAIL = new Zone("JAIL", "Bolingbroke Penitentiary", ScannerAudio.areas.BoilingBrookPenitentiary.FileName, County.BlaineCounty);// Agencies.SASPA, new List<Agency>() { Agencies.DOA });
        KOREAT = new Zone("KOREAT", "Little Seoul", ScannerAudio.areas.LittleSeoul.FileName, County.CityOfLosSantos);// Agencies.RHPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.CentralUnits.FileName, ScannerAudio.attention_all_area_units.CentralUnits1.FileName, ScannerAudio.attention_all_area_units.CentralAreaUnits.FileName }, "Central Area Units");
        LACT = new Zone("LACT", "Land Act Reservoir", "", County.LosSantosCounty);// Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        LAGO = new Zone("LAGO", "Lago Zancudo", ScannerAudio.areas.LagoZancudo.FileName, County.BlaineCounty);// Agencies.BCSO, new List<Agency>() { Agencies.DOA });
        LDAM = new Zone("LDAM", "Land Act Dam", "", County.LosSantosCounty);// Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        LEGSQU = new Zone("LEGSQU", "Legion Square", "", County.CityOfLosSantos);// Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        LMESA = new Zone("LMESA", "La Mesa", ScannerAudio.areas.LaMesa.FileName, County.CityOfLosSantos);// Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.EastLosSantosUnits1.FileName, ScannerAudio.attention_all_area_units.EastLosSantosUnits1.FileName, ScannerAudio.attention_all_area_units.EastLosSantos2.FileName }, "East Los Santos Units");
        LOSPUER = new Zone("LOSPUER", "La Puerta", ScannerAudio.areas.LaPuertes.FileName, County.CityOfLosSantos);// Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        MIRR = new Zone("MIRR", "Mirror Park", ScannerAudio.areas.MirrorPark.FileName, County.CityOfLosSantos);// Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.EastLosSantosUnits1.FileName, ScannerAudio.attention_all_area_units.EastLosSantosUnits1.FileName, ScannerAudio.attention_all_area_units.EastLosSantos2.FileName }, "East Los Santos Units");
        MORN = new Zone("MORN", "Morningwood", ScannerAudio.areas.MorningWood.FileName, County.CityOfLosSantos);// Agencies.RHPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        MOVIE = new Zone("MOVIE", "Richards Majestic", ScannerAudio.areas.RichardsMajesticStudio.FileName, County.CityOfLosSantos);// Agencies.RHPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        MTCHIL = new Zone("MTCHIL", "Mount Chiliad", ScannerAudio.areas.MountChiliad.FileName, County.BlaineCounty);// Agencies.SAPR);
        MTGORDO = new Zone("MTGORDO", "Mount Gordo", ScannerAudio.areas.MountGordo.FileName, County.BlaineCounty);// Agencies.SAPR);
        MTJOSE = new Zone("MTJOSE", "Mount Josiah", ScannerAudio.areas.MtJosiah.FileName, County.BlaineCounty);// Agencies.BCSO, new List<Agency>() { Agencies.DOA });
        MURRI = new Zone("MURRI", "Murrieta Heights", ScannerAudio.areas.MuriettaHeights.FileName, County.CityOfLosSantos);// Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.EastLosSantosUnits1.FileName, ScannerAudio.attention_all_area_units.EastLosSantosUnits1.FileName, ScannerAudio.attention_all_area_units.EastLosSantos2.FileName }, "East Los Santos Units");
        NCHU = new Zone("NCHU", "North Chumash", ScannerAudio.areas.NorthChumash.FileName, County.BlaineCounty);// Agencies.BCSO, new List<Agency>() { Agencies.DOA });
        NOOSE = new Zone("NOOSE", "N.O.O.S.E", "", County.LosSantosCounty);// Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        OCEANA = new Zone("OCEANA", "Pacific Ocean", ScannerAudio.areas.TheOcean.FileName, County.PacificOcean, Agencies.SACG);
        PALCOV = new Zone("PALCOV", "Paleto Cove", ScannerAudio.areas.PaletoBay.FileName, County.BlaineCounty);// Agencies.BCSO, new List<Agency>() { Agencies.DOA });
        PALETO = new Zone("PALETO", "Paleto Bay", ScannerAudio.areas.PaletoBay.FileName, County.BlaineCounty);// Agencies.BCSO, new List<Agency>() { Agencies.DOA }, new List<string>() { ScannerAudio.attention_all_area_units.PaletoaBayUnits.FileName, ScannerAudio.attention_all_area_units.PaletoBayUnits.FileName, ScannerAudio.attention_all_area_units.PaletoBayUnits3.FileName }, "Paleto Bay Units");
        PALFOR = new Zone("PALFOR", "Paleto Forest", ScannerAudio.areas.PaletoForest.FileName, County.BlaineCounty);// Agencies.SAPR);
        PALHIGH = new Zone("PALHIGH", "Palomino Highlands", ScannerAudio.areas.PalominoHighlands.FileName, County.LosSantosCounty);// Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        PALMPOW = new Zone("PALMPOW", "Palmer - Taylor Power Station", ScannerAudio.areas.PalmerTaylorPowerStation.FileName, County.LosSantosCounty);// Agencies.LSSD);
        PBLUFF = new Zone("PBLUFF", "Pacific Bluffs", ScannerAudio.areas.PacificBluffs.FileName, County.LosSantosCounty);// Agencies.VPPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.VespucciAreaUnits.FileName, ScannerAudio.attention_all_area_units.VespucciAreaUnits2.FileName }, "Vespucci Area Units");
        PBOX = new Zone("PBOX", "Pillbox Hill", ScannerAudio.areas.PillboxHill.FileName, County.CityOfLosSantos);// Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.CentralUnits.FileName, ScannerAudio.attention_all_area_units.CentralUnits1.FileName, ScannerAudio.attention_all_area_units.CentralAreaUnits.FileName }, "Central Area Units");
        PROCOB = new Zone("PROCOB", "Procopio Beach", ScannerAudio.areas.ProcopioBeach.FileName, County.BlaineCounty);// Agencies.BCSO, new List<Agency>() { Agencies.DOA });
        RANCHO = new Zone("RANCHO", "Rancho", ScannerAudio.areas.Rancho.FileName, County.CityOfLosSantos);// Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.CentralUnits.FileName, ScannerAudio.attention_all_area_units.CentralUnits1.FileName, ScannerAudio.attention_all_area_units.CentralAreaUnits.FileName }, "Central Area Units");
        RGLEN = new Zone("RGLEN", "Richman Glen", ScannerAudio.areas.RichmanGlenn.FileName, County.LosSantosCounty);// Agencies.LSSDVW, new List<Agency>() { Agencies.DOA }, new List<string>() { ScannerAudio.attention_all_area_units.VinewoodHillsUnits.FileName, ScannerAudio.attention_all_area_units.VinewoodHillsUnits1.FileName, ScannerAudio.attention_all_area_units.VinewoodHillsUnits3.FileName }, "Vinewood Hills Units");
        RICHM = new Zone("RICHM", "Richman", ScannerAudio.areas.Richman.FileName, County.CityOfLosSantos);// Agencies.RHPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        ROCKF = new Zone("ROCKF", "Rockford Hills", ScannerAudio.areas.RockfordHills.FileName, County.CityOfLosSantos);// Agencies.RHPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        RTRAK = new Zone("RTRAK", "Redwood Lights Track", ScannerAudio.areas.TheRedwoodLightsTrack.FileName, County.LosSantosCounty);// Agencies.LSSD);
        SANAND = new Zone("SANAND", "San Andreas", ScannerAudio.areas.SanAndreas.FileName, County.LosSantosCounty);// Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        SANCHIA = new Zone("SANCHIA", "San Chianski Mountain Range", "", County.BlaineCounty);// Agencies.SAPR);
        SANDY = new Zone("SANDY", "Sandy Shores", ScannerAudio.areas.SandyShores.FileName, County.BlaineCounty);// Agencies.BCSO, new List<Agency>() { Agencies.DOA }, new List<string>() { ScannerAudio.attention_all_area_units.SandyShoreUnits.FileName, ScannerAudio.attention_all_area_units.SandyShoresUnits2.FileName, ScannerAudio.attention_all_area_units.SanyShoreUnits3.FileName }, "Sandy Shores Units");
        SKID = new Zone("SKID", "Mission Row", ScannerAudio.areas.MissionRow.FileName, County.CityOfLosSantos);// Agencies.LSPD, new List<Agency>() { Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.CentralUnits.FileName, ScannerAudio.attention_all_area_units.CentralUnits1.FileName, ScannerAudio.attention_all_area_units.CentralAreaUnits.FileName }, "Central Area Units");
        SLAB = new Zone("SLAB", "Slab City", ScannerAudio.areas.SlabCity.FileName, County.BlaineCounty);// Agencies.BCSO, new List<Agency>() { Agencies.DOA });
        STAD = new Zone("STAD", "Maze Bank Arena", ScannerAudio.areas.MazeBankArena.FileName, County.CityOfLosSantos);// Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.CentralUnits.FileName, ScannerAudio.attention_all_area_units.CentralUnits1.FileName, ScannerAudio.attention_all_area_units.CentralAreaUnits.FileName }, "Central Area Units");
        STRAW = new Zone("STRAW", "Strawberry", ScannerAudio.areas.Strawberry.FileName, County.CityOfLosSantos);// Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.CentralUnits.FileName, ScannerAudio.attention_all_area_units.CentralUnits1.FileName, ScannerAudio.attention_all_area_units.CentralAreaUnits.FileName }, "Central Area Units");
        TATAMO = new Zone("TATAMO", "Tataviam Mountains", ScannerAudio.areas.TatathiaMountains.FileName, County.LosSantosCounty);// Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        TERMINA = new Zone("TERMINA", "Terminal", ScannerAudio.areas.Terminal.FileName, County.CityOfLosSantos);// Agencies.LSPA, new List<Agency>() { Agencies.LSPD, Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.PortOfLosSantosUnits.FileName, ScannerAudio.attention_all_area_units.PortOfLosSantosUnits1.FileName, ScannerAudio.attention_all_area_units.PortOfLosSantosUnits2.FileName }, "Port of Los Santos Units");
        TEXTI = new Zone("TEXTI", "Textile City", ScannerAudio.areas.TextileCity.FileName, County.CityOfLosSantos);// Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.CentralUnits.FileName, ScannerAudio.attention_all_area_units.CentralUnits1.FileName, ScannerAudio.attention_all_area_units.CentralAreaUnits.FileName }, "Central Area Units");
        TONGVAH = new Zone("TONGVAH", "Tongva Hills", ScannerAudio.areas.TongaHills.FileName, County.LosSantosCounty);// Agencies.LSSDCH, new List<Agency>() { Agencies.DOA }, new List<string>() { ScannerAudio.attention_all_area_units.ChumashUnits.FileName, ScannerAudio.attention_all_area_units.ChumashUnits1.FileName, ScannerAudio.attention_all_area_units.ChumashUnits2.FileName }, "Chumash Units");
        TONGVAV = new Zone("TONGVAV", "Tongva Valley", ScannerAudio.areas.TongvaValley.FileName, County.LosSantosCounty);// Agencies.LSSDVW, new List<Agency>() { Agencies.DOA }, new List<string>() { ScannerAudio.attention_all_area_units.VinewoodHillsUnits.FileName, ScannerAudio.attention_all_area_units.VinewoodHillsUnits1.FileName, ScannerAudio.attention_all_area_units.VinewoodHillsUnits3.FileName }, "Vinewood Hills Units");
        VCANA = new Zone("VCANA", "Vespucci Canals", ScannerAudio.areas.VespucciCanal.FileName, County.CityOfLosSantos);// Agencies.VPPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.VespucciAreaUnits.FileName, ScannerAudio.attention_all_area_units.VespucciAreaUnits2.FileName }, "Vespucci Area Units");
        VESP = new Zone("VESP", "Vespucci", ScannerAudio.areas.Vespucci.FileName, County.CityOfLosSantos);// Agencies.VPPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.VespucciAreaUnits.FileName, ScannerAudio.attention_all_area_units.VespucciAreaUnits2.FileName }, "Vespucci Area Units");
        VINE = new Zone("VINE", "Vinewood", ScannerAudio.areas.Vinewood.FileName, County.CityOfLosSantos);// Agencies.LSPDVW, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.VinewoodAreaUnits.FileName, ScannerAudio.attention_all_area_units.VinewoodUnits2.FileName }, "Vinewood Units");
        WINDF = new Zone("WINDF", "Ron Alternates Wind Farm", ScannerAudio.areas.RonAlternatesWindFarm.FileName, County.LosSantosCounty);// Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        WVINE = new Zone("WVINE", "West Vinewood", ScannerAudio.areas.WestVinewood.FileName, County.CityOfLosSantos);// Agencies.LSPDVW, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB }, new List<string>() { ScannerAudio.attention_all_area_units.VinewoodAreaUnits.FileName, ScannerAudio.attention_all_area_units.VinewoodUnits2.FileName }, "Vinewood Units");
        ZANCUDO = new Zone("ZANCUDO", "Zancudo River", ScannerAudio.areas.ZancudoRiver.FileName, County.BlaineCounty);// Agencies.SAPR,null, new List<string>() { ScannerAudio.attention_all_area_units.ZancudoRiverUnits.FileName }, "Zancudo River Units");
        ZP_ORT = new Zone("ZP_ORT", "Port of South Los Santos", ScannerAudio.areas.PortOfSouthLosSantos.FileName, County.CityOfLosSantos);// Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        ZQ_UAR = new Zone("ZQ_UAR", "Davis Quartz", ScannerAudio.areas.DavisCourts.FileName, County.BlaineCounty);// Agencies.BCSO, new List<Agency>() { Agencies.DOA });

        UNK_LSCOUNTY = new Zone("UNK_LSCOUNTY", "Los Santos County", "", County.LosSantosCounty);// Agencies.LSSD);
        UNK_BLANIECOUNTY = new Zone("UNK_BLANIECOUNTY", "Blaine County", "", County.BlaineCounty);// Agencies.LSSD);
        UNK_LSCITY = new Zone("UNK_LSCITY", "Los Santos",  "", County.CityOfLosSantos);// Agencies.LSPD);
        ZoneList = new List<Zone>
        {
            AIRP,
            ALAMO,
            ALTA,
            ARMYB,
            BANHAMC,
            BANNING,
            BAYTRE,
            BEACH,
            BHAMCA,
            BRADP,
            BRADT,
            BURTON,
            CALAFB,
            CANNY,
            CCREAK,
            CHAMH,
            CHIL,
            CHU,
            CMSW,
            CYPRE,
            DAVIS,
            DELBE,
            DELPE,
            DELSOL,
            DESRT,
            DOWNT,
            DTVINE,
            EAST_V,
            EBURO,
            ELGORL,
            ELYSIAN,
            GALFISH,
            GOLF,
            GRAPES,
            GREATC,
            HARMO,
            HAWICK,
            HORS,
            HUMLAB,
            JAIL,
            KOREAT,
            LACT,
            LAGO,
            LDAM,
            LEGSQU,
            LMESA,
            LOSPUER,
            MIRR,
            MORN,
            MOVIE,
            MTCHIL,
            MTGORDO,
            MTJOSE,
            MURRI,
            NCHU,
            NOOSE,
            OCEANA,
            PALCOV,
            PALETO,
            PALFOR,
            PALHIGH,
            PALMPOW,
            PBLUFF,
            PBOX,
            PROCOB,
            RANCHO,
            RGLEN,
            RICHM,
            ROCKF,
            RTRAK,
            SANAND,
            SANCHIA,
            SANDY,
            SKID,
            SLAB,
            STAD,
            STRAW,
            TATAMO,
            TERMINA,
            TEXTI,
            TONGVAH,
            TONGVAV,
            VCANA,
            VESP,
            VINE,
            WINDF,
            WVINE,
            ZANCUDO,
            ZP_ORT,
            ZQ_UAR
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
            if (Game.LocalPlayer.Character.IsInLosSantosCity())
                return UNK_LSCITY;
            else
                return UNK_LSCOUNTY;
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
    public static string GetFormattedZoneName(Zone MyZone)
    {
        string CountyName = "San Andreas";
        if (MyZone.ZoneCounty == County.BlaineCounty)
            CountyName = "Blaine County";
        else if (MyZone.ZoneCounty == County.CityOfLosSantos)
            CountyName = "City of Los Santos";
        else if (MyZone.ZoneCounty == County.LosSantosCounty)
            CountyName = "Los Santos County";

        return MyZone.TextName + ", " + CountyName;
    }
    public static Agency GetCountyAgencyByZone(Zone MyZone)
    {
        if (MyZone.ZoneCounty == County.BlaineCounty)
        {
            return Agencies.BCSO;
        }
        else if (MyZone.ZoneCounty == County.LosSantosCounty)
        {
            return Agencies.LSSD;
        }
        else if (MyZone.ZoneCounty == County.CityOfLosSantos)
        {
            return Agencies.LSPD;
        }
        else if (MyZone.ZoneCounty == County.PacificOcean)
        {
            return Agencies.LSPD;
        }
        else
        {
            return Agencies.LSPD;
        }
    }
}
public class Zone
{
    public Zone(string _GameName, string _TextName, string _ScannerValue,County _ZoneCounty, Agency _MainZoneAgency)
    {
        GameName = _GameName;
        TextName = _TextName;
        ScannerValue = _ScannerValue;
        MainZoneAgency = _MainZoneAgency;
        ZoneCounty = _ZoneCounty;
    }
    public Zone(string _GameName, string _TextName, string _ScannerValue, County _ZoneCounty, Agency _MainZoneAgency, List<Agency> _SecondaryZoneAgencies)
    {
        GameName = _GameName;
        TextName = _TextName;
        ScannerValue = _ScannerValue;
        MainZoneAgency = _MainZoneAgency;
        SecondaryZoneAgencies = _SecondaryZoneAgencies;
        ZoneCounty = _ZoneCounty;
    }
    public Zone(string _GameName, string _TextName, string _ScannerValue, County _ZoneCounty, Agency _MainZoneAgency, List<Agency> _SecondaryZoneAgencies, List<string> _DispatchUnitAudio, string _DispatchUnitName)
    {
        GameName = _GameName;
        TextName = _TextName;
        ScannerValue = _ScannerValue;
        MainZoneAgency = _MainZoneAgency;
        SecondaryZoneAgencies = _SecondaryZoneAgencies;
        ZoneCounty = _ZoneCounty;
        DispatchUnitAudio = _DispatchUnitAudio;
        DispatchUnitName = _DispatchUnitName;
    }
    public string DispatchUnitName { get; set; }
    public string GameName { get; set; }
    public string TextName { get; set; }
    public County ZoneCounty { get; set; }
    public List<string> DispatchUnitAudio { get; set; } = new List<string>();
    public string ScannerValue { get; set; }
    public Agency MainZoneAgency { get; set; } = Agencies.LSPD;
    public List<Agency> SecondaryZoneAgencies { get; set; } = new List<Agency>();
}
public enum County
{
    CityOfLosSantos = 0,
    LosSantosCounty = 1,
    BlaineCounty = 2,
    PacificOcean = 3,
}


*/

