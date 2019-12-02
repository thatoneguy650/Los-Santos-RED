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
    public static List<Zone> ZoneList = new List<Zone>();
    public static void Initialize()
    {
        AIRP = new Zone("AIRP", "Los Santos International Airport", ScannerAudio.areas.LosSantosInternationalAirport.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        ALAMO = new Zone("ALAMO", "Alamo Sea", ScannerAudio.areas.TheAlamaSea.FileName, County.BlaineCounty, Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        ALTA = new Zone("ALTA", "Alta", ScannerAudio.areas.Alta.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        ARMYB = new Zone("ARMYB", "Fort Zancudo", ScannerAudio.areas.FtZancudo.FileName, County.BlaineCounty, Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        BANHAMC = new Zone("BANHAMC", "Banham Canyon Dr", "", County.LosSantosCounty, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        BANNING = new Zone("BANNING", "Banning", ScannerAudio.areas.Banning.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        BAYTRE = new Zone("BAYTRE", "Baytree Canyon", ScannerAudio.areas.BayTreeCanyon.FileName, County.LosSantosCounty, Agencies.LSSD);
        BEACH = new Zone("BEACH", "Vespucci Beach", ScannerAudio.areas.VespucciBeach.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        BHAMCA = new Zone("BHAMCA", "Banham Canyon", "", County.LosSantosCounty, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        BRADP = new Zone("BRADP", "Braddock Pass", ScannerAudio.areas.BraddockPass.FileName, County.BlaineCounty, Agencies.LSSD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        BRADT = new Zone("BRADT", "Braddock Tunnel", ScannerAudio.areas.TheBraddockTunnel.FileName, County.BlaineCounty, Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        BURTON = new Zone("BURTON", "Burton", ScannerAudio.areas.Burton.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        CALAFB = new Zone("CALAFB", "Calafia Bridge", "", County.BlaineCounty, Agencies.SAPR);
        CANNY = new Zone("CANNY", "Raton Canyon", ScannerAudio.areas.RatonCanyon.FileName, County.BlaineCounty, Agencies.SAPR);
        CCREAK = new Zone("CCREAK", "Cassidy Creek", "", County.BlaineCounty, Agencies.SAPR);
        CHAMH = new Zone("CHAMH", "Chamberlain Hills", ScannerAudio.areas.ChamberlainHills.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        CHIL = new Zone("CHIL", "Vinewood Hills", ScannerAudio.areas.VinewoodHills.FileName, County.LosSantosCounty, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        CHU = new Zone("CHU", "Chumash", ScannerAudio.areas.Chumash.FileName, County.LosSantosCounty, Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        CMSW = new Zone("CMSW", "Chiliad Mountain State Wilderness", ScannerAudio.areas.ChilliadMountainStWilderness.FileName, County.BlaineCounty, Agencies.SAPR);
        CYPRE = new Zone("CYPRE", "Cypress Flats", ScannerAudio.areas.CypressFlats.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        DAVIS = new Zone("DAVIS", "Davis", ScannerAudio.areas.Davis.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        DELBE = new Zone("DELBE", "Del Perro Beach", ScannerAudio.areas.DelPierroBeach.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        DELPE = new Zone("DELPE", "Del Perro", ScannerAudio.areas.DelPierro.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        DELSOL = new Zone("DELSOL", "Puerto Del Sol", ScannerAudio.areas.LaPorta.FileName, County.LosSantosCounty, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });//???????;
        DESRT = new Zone("DESRT", "Grand Senora Desert", ScannerAudio.areas.GrandeSonoranDesert.FileName, County.BlaineCounty, Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        DOWNT = new Zone("DOWNT", "Downtown", ScannerAudio.areas.Downtown.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        DTVINE = new Zone("DTVINE", "Downtown Vinewood", ScannerAudio.areas.DowntownVinewood.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        EAST_V = new Zone("EAST_V", "East Vinewood", ScannerAudio.areas.EastVinewood.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        EBURO = new Zone("EBURO", "El Burro Heights", ScannerAudio.areas.ElBerroHights.FileName, County.LosSantosCounty, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        ELGORL = new Zone("ELGORL", "El Gordo Lighthouse", ScannerAudio.areas.MountGordo.FileName, County.BlaineCounty, Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        ELYSIAN = new Zone("ELYSIAN", "Elysian Island", ScannerAudio.areas.ElysianIsland.FileName, County.CityOfLosSantos, Agencies.LSPA, new List<Agency>() { Agencies.LSPD, Agencies.IAA, Agencies.DOA, Agencies.FIB });
        GALFISH = new Zone("GALFISH", "Galilee", ScannerAudio.areas.GalileoPark.FileName, County.LosSantosCounty, Agencies.SAPR);
        GOLF = new Zone("GOLF", "GWC and Golfing Society", ScannerAudio.areas.TheGWCGolfingSociety.FileName, County.CityOfLosSantos, Agencies.LSPD);
        GRAPES = new Zone("GRAPES", "Grapeseed", ScannerAudio.areas.Grapeseed.FileName, County.BlaineCounty, Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        GREATC = new Zone("GREATC", "Great Chaparral", ScannerAudio.areas.GreatChapparalle.FileName, County.LosSantosCounty, Agencies.SAPR);
        HARMO = new Zone("HARMO", "Harmony", ScannerAudio.areas.Harmony.FileName, County.LosSantosCounty, Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        HAWICK = new Zone("HAWICK", "Hawick", "", County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        HORS = new Zone("HORS", "Vinewood Racetrack", ScannerAudio.areas.TheRaceCourse.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        HUMLAB = new Zone("HUMLAB", "Humane Labs and Research", "", County.BlaineCounty, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        JAIL = new Zone("JAIL", "Bolingbroke Penitentiary", ScannerAudio.areas.BoilingBrookPenitentiary.FileName, County.BlaineCounty, Agencies.SASPA, new List<Agency>() { Agencies.DOA });
        KOREAT = new Zone("KOREAT", "Little Seoul", ScannerAudio.areas.LittleSeoul.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        LACT = new Zone("LACT", "Land Act Reservoir", "", County.LosSantosCounty, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        LAGO = new Zone("LAGO", "Lago Zancudo", ScannerAudio.areas.LagoZancudo.FileName, County.BlaineCounty, Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        LDAM = new Zone("LDAM", "Land Act Dam", "", County.LosSantosCounty, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        LEGSQU = new Zone("LEGSQU", "Legion Square", "", County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        LMESA = new Zone("LMESA", "La Mesa", ScannerAudio.areas.LaMesa.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        LOSPUER = new Zone("LOSPUER", "La Puerta", ScannerAudio.areas.LaPuertes.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        MIRR = new Zone("MIRR", "Mirror Park", ScannerAudio.areas.MirrorPark.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        MORN = new Zone("MORN", "Morningwood", ScannerAudio.areas.MorningWood.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        MOVIE = new Zone("MOVIE", "Richards Majestic", ScannerAudio.areas.RichardsMajesticStudio.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        MTCHIL = new Zone("MTCHIL", "Mount Chiliad", ScannerAudio.areas.MountChiliad.FileName, County.BlaineCounty, Agencies.SAPR);
        MTGORDO = new Zone("MTGORDO", "Mount Gordo", ScannerAudio.areas.MountGordo.FileName, County.BlaineCounty, Agencies.SAPR);
        MTJOSE = new Zone("MTJOSE", "Mount Josiah", ScannerAudio.areas.MtJosiah.FileName, County.BlaineCounty, Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        MURRI = new Zone("MURRI", "Murrieta Heights", ScannerAudio.areas.MuriettaHeights.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        NCHU = new Zone("NCHU", "North Chumash", ScannerAudio.areas.NorthChumash.FileName, County.BlaineCounty, Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        NOOSE = new Zone("NOOSE", "N.O.O.S.E", "", County.LosSantosCounty, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        OCEANA = new Zone("OCEANA", "Pacific Ocean", ScannerAudio.areas.TheOcean.FileName, County.BlaineCounty, Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        PALCOV = new Zone("PALCOV", "Paleto Cove", ScannerAudio.areas.PaletoBay.FileName, County.BlaineCounty, Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        PALETO = new Zone("PALETO", "Paleto Bay", ScannerAudio.areas.PaletoBay.FileName, County.BlaineCounty, Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        PALFOR = new Zone("PALFOR", "Paleto Forest", ScannerAudio.areas.PaletoForest.FileName, County.BlaineCounty, Agencies.SAPR);
        PALHIGH = new Zone("PALHIGH", "Palomino Highlands", ScannerAudio.areas.PalominoHighlands.FileName, County.LosSantosCounty, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        PALMPOW = new Zone("PALMPOW", "Palmer - Taylor Power Station", ScannerAudio.areas.PalmerTaylorPowerStation.FileName, County.LosSantosCounty, Agencies.LSSD);
        PBLUFF = new Zone("PBLUFF", "Pacific Bluffs", ScannerAudio.areas.PacificBluffs.FileName, County.LosSantosCounty, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        PBOX = new Zone("PBOX", "Pillbox Hill", ScannerAudio.areas.PillboxHill.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        PROCOB = new Zone("PROCOB", "Procopio Beach", ScannerAudio.areas.ProcopioBeach.FileName, County.BlaineCounty, Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        RANCHO = new Zone("RANCHO", "Rancho", ScannerAudio.areas.Rancho.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        RGLEN = new Zone("RGLEN", "Richman Glen", ScannerAudio.areas.RichmanGlenn.FileName, County.LosSantosCounty, Agencies.LSPD);
        RICHM = new Zone("RICHM", "Richman", ScannerAudio.areas.Richman.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        ROCKF = new Zone("ROCKF", "Rockford Hills", ScannerAudio.areas.RockfordHills.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        RTRAK = new Zone("RTRAK", "Redwood Lights Track", ScannerAudio.areas.TheRedwoodLightsTrack.FileName, County.LosSantosCounty, Agencies.LSSD);
        SANAND = new Zone("SANAND", "San Andreas", ScannerAudio.areas.SanAndreas.FileName, County.LosSantosCounty, Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        SANCHIA = new Zone("SANCHIA", "San Chianski Mountain Range", "", County.BlaineCounty, Agencies.SAPR);
        SANDY = new Zone("SANDY", "Sandy Shores", ScannerAudio.areas.SandyShores.FileName, County.BlaineCounty, Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        SKID = new Zone("SKID", "Mission Row", ScannerAudio.areas.MissionRow.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.DOA, Agencies.FIB });
        SLAB = new Zone("SLAB", "Slab City", ScannerAudio.areas.SlabCity.FileName, County.BlaineCounty, Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        STAD = new Zone("STAD", "Maze Bank Arena", ScannerAudio.areas.MazeBankArena.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        STRAW = new Zone("STRAW", "Strawberry", ScannerAudio.areas.Strawberry.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        TATAMO = new Zone("TATAMO", "Tataviam Mountains", ScannerAudio.areas.TatathiaMountains.FileName, County.LosSantosCounty, Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        TERMINA = new Zone("TERMINA", "Terminal", ScannerAudio.areas.Terminal.FileName, County.CityOfLosSantos, Agencies.LSPA, new List<Agency>() { Agencies.LSPD, Agencies.IAA, Agencies.DOA, Agencies.FIB });
        TEXTI = new Zone("TEXTI", "Textile City", ScannerAudio.areas.TextileCity.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        TONGVAH = new Zone("TONGVAH", "Tongva Hills", ScannerAudio.areas.TongaHills.FileName, County.LosSantosCounty, Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        TONGVAV = new Zone("TONGVAV", "Tongva Valley", ScannerAudio.areas.TongvaValley.FileName, County.LosSantosCounty, Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        VCANA = new Zone("VCANA", "Vespucci Canals", ScannerAudio.areas.VespucciCanal.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        VESP = new Zone("VESP", "Vespucci", ScannerAudio.areas.Vespucci.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        VINE = new Zone("VINE", "Vinewood", ScannerAudio.areas.Vinewood.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        WINDF = new Zone("WINDF", "Ron Alternates Wind Farm", ScannerAudio.areas.RonAlternatesWindFarm.FileName, County.LosSantosCounty, Agencies.LSSD, new List<Agency>() { Agencies.DOA });
        WVINE = new Zone("WVINE", "West Vinewood", ScannerAudio.areas.WestVinewood.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        ZANCUDO = new Zone("ZANCUDO", "Zancudo River", ScannerAudio.areas.ZancudoRiver.FileName, County.BlaineCounty, Agencies.SAPR);
        ZP_ORT = new Zone("ZP_ORT", "Port of South Los Santos", ScannerAudio.areas.PortOfSouthLosSantos.FileName, County.CityOfLosSantos, Agencies.LSPD, new List<Agency>() { Agencies.IAA, Agencies.DOA, Agencies.FIB });
        ZQ_UAR = new Zone("ZQ_UAR", "Davis Quartz", ScannerAudio.areas.DavisCourts.FileName, County.BlaineCounty, Agencies.LSSD, new List<Agency>() { Agencies.DOA });

        UNK_LSCOUNTY = new Zone("UNK_LSCOUNTY", "Los Santos County", "", County.LosSantosCounty, Agencies.LSSD);
        UNK_BLANIECOUNTY = new Zone("UNK_BLANIECOUNTY", "Blaine County", "", County.BlaineCounty, Agencies.LSSD);
        UNK_LSCITY = new Zone("UNK_LSCITY", "Los Santos",  "", County.CityOfLosSantos, Agencies.LSPD);

        ZoneList.Add(AIRP);
        ZoneList.Add(ALAMO);
        ZoneList.Add(ALTA);
        ZoneList.Add(ARMYB);
        ZoneList.Add(BANHAMC);
        ZoneList.Add(BANNING);
        ZoneList.Add(BAYTRE);
        ZoneList.Add(BEACH);
        ZoneList.Add(BHAMCA);
        ZoneList.Add(BRADP);
        ZoneList.Add(BRADT);
        ZoneList.Add(BURTON);
        ZoneList.Add(CALAFB);
        ZoneList.Add(CANNY);
        ZoneList.Add(CCREAK);
        ZoneList.Add(CHAMH);
        ZoneList.Add(CHIL);
        ZoneList.Add(CHU);
        ZoneList.Add(CMSW);
        ZoneList.Add(CYPRE);
        ZoneList.Add(DAVIS);
        ZoneList.Add(DELBE);
        ZoneList.Add(DELPE);
        ZoneList.Add(DELSOL);
        ZoneList.Add(DESRT);
        ZoneList.Add(DOWNT);
        ZoneList.Add(DTVINE);
        ZoneList.Add(EAST_V);
        ZoneList.Add(EBURO);
        ZoneList.Add(ELGORL);
        ZoneList.Add(ELYSIAN);
        ZoneList.Add(GALFISH);
        ZoneList.Add(GOLF);
        ZoneList.Add(GRAPES);
        ZoneList.Add(GREATC);
        ZoneList.Add(HARMO);
        ZoneList.Add(HAWICK);
        ZoneList.Add(HORS);
        ZoneList.Add(HUMLAB);
        ZoneList.Add(JAIL);
        ZoneList.Add(KOREAT);
        ZoneList.Add(LACT);
        ZoneList.Add(LAGO);
        ZoneList.Add(LDAM);
        ZoneList.Add(LEGSQU);
        ZoneList.Add(LMESA);
        ZoneList.Add(LOSPUER);
        ZoneList.Add(MIRR);
        ZoneList.Add(MORN);
        ZoneList.Add(MOVIE);
        ZoneList.Add(MTCHIL);
        ZoneList.Add(MTGORDO);
        ZoneList.Add(MTJOSE);
        ZoneList.Add(MURRI);
        ZoneList.Add(NCHU);
        ZoneList.Add(NOOSE);
        ZoneList.Add(OCEANA);
        ZoneList.Add(PALCOV);
        ZoneList.Add(PALETO);
        ZoneList.Add(PALFOR);
        ZoneList.Add(PALHIGH);
        ZoneList.Add(PALMPOW);
        ZoneList.Add(PBLUFF);
        ZoneList.Add(PBOX);
        ZoneList.Add(PROCOB);
        ZoneList.Add(RANCHO);
        ZoneList.Add(RGLEN);
        ZoneList.Add(RICHM);
        ZoneList.Add(ROCKF);
        ZoneList.Add(RTRAK);
        ZoneList.Add(SANAND);
        ZoneList.Add(SANCHIA);
        ZoneList.Add(SANDY);
        ZoneList.Add(SKID);
        ZoneList.Add(SLAB);
        ZoneList.Add(STAD);
        ZoneList.Add(STRAW);
        ZoneList.Add(TATAMO);
        ZoneList.Add(TERMINA);
        ZoneList.Add(TEXTI);
        ZoneList.Add(TONGVAH);
        ZoneList.Add(TONGVAV);
        ZoneList.Add(VCANA);
        ZoneList.Add(VESP);
        ZoneList.Add(VINE);
        ZoneList.Add(WINDF);
        ZoneList.Add(WVINE);
        ZoneList.Add(ZANCUDO);
        ZoneList.Add(ZP_ORT);
        ZoneList.Add(ZQ_UAR);
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
            if (Game.LocalPlayer.Character.isInLosSantosCity())
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
        string zoneName = string.Empty;
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
    public string GameName { get; set; }
    public string TextName { get; set; }
    public County ZoneCounty { get; set; }
    public string ScannerValue { get; set; }
    public Agency MainZoneAgency { get; set; } = Agencies.LSPD;
    public List<Agency> SecondaryZoneAgencies { get; set; } = new List<Agency>();
}
public enum County
{
    CityOfLosSantos = 0,
    LosSantosCounty = 1,
    BlaineCounty = 2,
}


