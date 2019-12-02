using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

public static class Zones
{
    public static Zone GetZoneAtLocation(Vector3 ZonePosition)
    {      
        string zoneName = string.Empty;
        unsafe
        {
            IntPtr ptr = Rage.Native.NativeFunction.CallByName<IntPtr>("GET_NAME_OF_ZONE", ZonePosition.X, ZonePosition.Y, ZonePosition.Z);

            zoneName = Marshal.PtrToStringAnsi(ptr);
        }
        return ZoneList.Where(x => x.GameName == zoneName).FirstOrDefault();
    }

    public static readonly List<Zone> ZoneList = new List<Zone>(new[]
    {
        new Zone("AIRP", "Los Santos International Airport",ScannerAudio.areas.LosSantosInternationalAirport.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("ALAMO", "Alamo Sea",ScannerAudio.areas.TheAlamaSea.FileName,County.BlaineCounty,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("ALTA", "Alta",ScannerAudio.areas.Alta.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("ARMYB", "Fort Zancudo",ScannerAudio.areas.FtZancudo.FileName,County.BlaineCounty,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("BANHAMC", "Banham Canyon Dr","",County.LosSantosCounty,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("BANNING", "Banning",ScannerAudio.areas.Banning.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("BEACH", "Vespucci Beach",ScannerAudio.areas.VespucciBeach.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("BHAMCA", "Banham Canyon","",County.LosSantosCounty,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("BRADP", "Braddock Pass",ScannerAudio.areas.BraddockPass.FileName,County.BlaineCounty,Agencies.LSSD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("BRADT", "Braddock Tunnel",ScannerAudio.areas.TheBraddockTunnel.FileName,County.BlaineCounty,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("BURTON", "Burton",ScannerAudio.areas.Burton.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("CALAFB", "Calafia Bridge","",County.BlaineCounty,Agencies.SAPR),
        new Zone("CANNY", "Raton Canyon",ScannerAudio.areas.RatonCanyon.FileName,County.BlaineCounty,Agencies.SAPR),
        new Zone("CCREAK", "Cassidy Creek","",County.BlaineCounty,Agencies.SAPR),
        new Zone("CHAMH", "Chamberlain Hills",ScannerAudio.areas.ChamberlainHills.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("CHIL", "Vinewood Hills",ScannerAudio.areas.VinewoodHills.FileName,County.LosSantosCounty,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("CHU", "Chumash",ScannerAudio.areas.Chumash.FileName,County.LosSantosCounty,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("CMSW", "Chiliad Mountain State Wilderness",ScannerAudio.areas.ChilliadMountainStWilderness.FileName,County.BlaineCounty,Agencies.SAPR),
        new Zone("CYPRE", "Cypress Flats",ScannerAudio.areas.CypressFlats.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("DAVIS", "Davis",ScannerAudio.areas.Davis.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("DELBE", "Del Perro Beach",ScannerAudio.areas.DelPierroBeach.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("DELPE", "Del Perro",ScannerAudio.areas.DelPierro.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("DELSOL", "Puerto Del Sol",ScannerAudio.areas.LaPorta.FileName,County.LosSantosCounty,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),//????????
        new Zone("DESRT", "Grand Senora Desert",ScannerAudio.areas.GrandeSonoranDesert.FileName,County.BlaineCounty,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("DOWNT", "Downtown",ScannerAudio.areas.Downtown.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("DTVINE", "Downtown Vinewood",ScannerAudio.areas.DowntownVinewood.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("EAST_V", "East Vinewood",ScannerAudio.areas.EastVinewood.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("EBURO", "El Burro Heights",ScannerAudio.areas.ElBerroHights.FileName,County.LosSantosCounty,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("ELGORL", "El Gordo Lighthouse",ScannerAudio.areas.MountGordo.FileName,County.BlaineCounty,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("ELYSIAN", "Elysian Island",ScannerAudio.areas.ElysianIsland.FileName,County.CityOfLosSantos,Agencies.LSPA,new List<Agency>() { Agencies.LSPD, Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("GALFISH", "Galilee",ScannerAudio.areas.GalileoPark.FileName,County.LosSantosCounty,Agencies.SAPR),
        new Zone("GOLF", "GWC and Golfing Society",ScannerAudio.areas.TheGWCGolfingSociety.FileName,County.CityOfLosSantos,Agencies.LSPD),
        new Zone("GRAPES", "Grapeseed",ScannerAudio.areas.Grapeseed.FileName,County.BlaineCounty,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("GREATC", "Great Chaparral",ScannerAudio.areas.GreatChapparalle.FileName,County.LosSantosCounty,Agencies.SAPR),
        new Zone("HARMO", "Harmony",ScannerAudio.areas.Harmony.FileName,County.LosSantosCounty,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("HAWICK", "Hawick","",County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("HORS", "Vinewood Racetrack",ScannerAudio.areas.TheRaceCourse.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("HUMLAB", "Humane Labs and Research","",County.BlaineCounty,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("JAIL", "Bolingbroke Penitentiary",ScannerAudio.areas.BoilingBrookPenitentiary.FileName,County.BlaineCounty,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("KOREAT", "Little Seoul",ScannerAudio.areas.LittleSeoul.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("LACT", "Land Act Reservoir","",County.LosSantosCounty,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("LAGO", "Lago Zancudo",ScannerAudio.areas.LagoZancudo.FileName,County.BlaineCounty,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("LDAM", "Land Act Dam","",County.LosSantosCounty,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("LEGSQU", "Legion Square","",County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("LMESA", "La Mesa",ScannerAudio.areas.LaMesa.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("LOSPUER", "La Puerta",ScannerAudio.areas.LaPuertes.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("MIRR", "Mirror Park",ScannerAudio.areas.MirrorPark.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("MORN", "Morningwood",ScannerAudio.areas.MorningWood.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("MOVIE", "Richards Majestic",ScannerAudio.areas.RichardsMajesticStudio.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("MTCHIL", "Mount Chiliad",ScannerAudio.areas.MountChiliad.FileName,County.BlaineCounty,Agencies.SAPR),
        new Zone("MTGORDO", "Mount Gordo",ScannerAudio.areas.MountGordo.FileName,County.BlaineCounty,Agencies.SAPR),
        new Zone("MTJOSE", "Mount Josiah",ScannerAudio.areas.MtJosiah.FileName,County.BlaineCounty,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("MURRI", "Murrieta Heights",ScannerAudio.areas.MuriettaHeights.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("NCHU", "North Chumash",ScannerAudio.areas.NorthChumash.FileName,County.BlaineCounty,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("NOOSE", "N.O.O.S.E","",County.LosSantosCounty,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("OCEANA", "Pacific Ocean",ScannerAudio.areas.TheOcean.FileName,County.BlaineCounty,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("PALCOV", "Paleto Cove",ScannerAudio.areas.PaletoBay.FileName,County.BlaineCounty,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("PALETO", "Paleto Bay",ScannerAudio.areas.PaletoBay.FileName,County.BlaineCounty,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("PALFOR", "Paleto Forest",ScannerAudio.areas.PaletoForest.FileName,County.BlaineCounty,Agencies.SAPR),
        new Zone("PALHIGH", "Palomino Highlands",ScannerAudio.areas.PalominoHighlands.FileName,County.LosSantosCounty,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("PALMPOW", "Palmer - Taylor Power Station",ScannerAudio.areas.PalmerTaylorPowerStation.FileName,County.LosSantosCounty,Agencies.LSSD),
        new Zone("PBLUFF", "Pacific Bluffs",ScannerAudio.areas.PacificBluffs.FileName,County.LosSantosCounty,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("PBOX", "Pillbox Hill",ScannerAudio.areas.PillboxHill.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("PROCOB", "Procopio Beach",ScannerAudio.areas.ProcopioBeach.FileName,County.BlaineCounty,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("RANCHO", "Rancho",ScannerAudio.areas.Rancho.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("RGLEN", "Richman Glen",ScannerAudio.areas.RichmanGlenn.FileName,County.LosSantosCounty,Agencies.LSPD),
        new Zone("RICHM", "Richman",ScannerAudio.areas.Richman.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("ROCKF", "Rockford Hills",ScannerAudio.areas.RockfordHills.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("RTRAK", "Redwood Lights Track",ScannerAudio.areas.TheRedwoodLightsTrack.FileName,County.LosSantosCounty,Agencies.LSSD),
        new Zone("SANAND", "San Andreas",ScannerAudio.areas.SanAndreas.FileName,County.LosSantosCounty,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("SANCHIA", "San Chianski Mountain Range","",County.BlaineCounty,Agencies.SAPR),
        new Zone("SANDY", "Sandy Shores",ScannerAudio.areas.SandyShores.FileName,County.BlaineCounty,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("SKID", "Mission Row",ScannerAudio.areas.MissionRow.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.DOA,Agencies.FIB }),
        new Zone("SLAB", "Slab City",ScannerAudio.areas.SlabCity.FileName,County.BlaineCounty,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("STAD", "Maze Bank Arena",ScannerAudio.areas.MazeBankArena.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("STRAW", "Strawberry",ScannerAudio.areas.Strawberry.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("TATAMO", "Tataviam Mountains",ScannerAudio.areas.TatathiaMountains.FileName,County.LosSantosCounty,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("TERMINA", "Terminal",ScannerAudio.areas.Terminal.FileName,County.CityOfLosSantos,Agencies.LSPA,new List<Agency>() { Agencies.LSPD, Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("TEXTI", "Textile City",ScannerAudio.areas.TextileCity.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("TONGVAH", "Tongva Hills",ScannerAudio.areas.TongaHills.FileName,County.LosSantosCounty,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("TONGVAV", "Tongva Valley",ScannerAudio.areas.TongvaValley.FileName,County.LosSantosCounty,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("VCANA", "Vespucci Canals",ScannerAudio.areas.VespucciCanal.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("VESP", "Vespucci",ScannerAudio.areas.Vespucci.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("VINE", "Vinewood",ScannerAudio.areas.Vinewood.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("WINDF", "Ron Alternates Wind Farm",ScannerAudio.areas.RonAlternatesWindFarm.FileName,County.LosSantosCounty,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("WVINE", "West Vinewood",ScannerAudio.areas.WestVinewood.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("ZANCUDO", "Zancudo River",ScannerAudio.areas.ZancudoRiver.FileName,County.BlaineCounty,Agencies.SAPR),
        new Zone("ZP_ORT", "Port of South Los Santos",ScannerAudio.areas.PortOfSouthLosSantos.FileName,County.CityOfLosSantos,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("ZQ_UAR", "Davis Quartz",ScannerAudio.areas.DavisCourts.FileName,County.BlaineCounty,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
    });

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


