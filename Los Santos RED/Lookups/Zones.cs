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
            new Zone("OCEANA", "Pacific Ocean", DispatchScannerFiles.areas.TheOcean.FileName, Zone.County.PacificOcean),

            //North Blaine
            new Zone("PROCOB", "Procopio Beach", DispatchScannerFiles.areas.ProcopioBeach.FileName, Zone.County.BlaineCounty),
            new Zone("MTCHIL", "Mount Chiliad", DispatchScannerFiles.areas.MountChiliad.FileName, Zone.County.BlaineCounty),
            new Zone("MTGORDO", "Mount Gordo", DispatchScannerFiles.areas.MountGordo.FileName, Zone.County.BlaineCounty),
            new Zone("PALETO", "Paleto Bay", DispatchScannerFiles.areas.PaletoBay.FileName, Zone.County.BlaineCounty),
            new Zone("PALCOV", "Paleto Cove", DispatchScannerFiles.areas.PaletoBay.FileName, Zone.County.BlaineCounty),
            new Zone("PALFOR", "Paleto Forest", DispatchScannerFiles.areas.PaletoForest.FileName, Zone.County.BlaineCounty),
            new Zone("CMSW", "Chiliad Mountain State Wilderness", DispatchScannerFiles.areas.ChilliadMountainStWilderness.FileName, Zone.County.BlaineCounty),
            new Zone("CALAFB", "Calafia Bridge", "", Zone.County.BlaineCounty),
            new Zone("GALFISH", "Galilee", "", Zone.County.BlaineCounty),
            new Zone("ELGORL", "El Gordo Lighthouse", DispatchScannerFiles.areas.MountGordo.FileName, Zone.County.BlaineCounty),
            new Zone("GRAPES", "Grapeseed", DispatchScannerFiles.areas.Grapeseed.FileName, Zone.County.BlaineCounty),
            new Zone("BRADP", "Braddock Pass", DispatchScannerFiles.areas.BraddockPass.FileName, Zone.County.BlaineCounty),
            new Zone("BRADT", "Braddock Tunnel", DispatchScannerFiles.areas.TheBraddockTunnel.FileName, Zone.County.BlaineCounty),
            new Zone("CCREAK", "Cassidy Creek", "", Zone.County.BlaineCounty),

            //Blaine
            new Zone("ALAMO", "Alamo Sea", DispatchScannerFiles.areas.TheAlamaSea.FileName, Zone.County.BlaineCounty),
            new Zone("ARMYB", "Fort Zancudo", DispatchScannerFiles.areas.FtZancudo.FileName, Zone.County.BlaineCounty),
            new Zone("CANNY", "Raton Canyon", DispatchScannerFiles.areas.RatonCanyon.FileName, Zone.County.BlaineCounty), 
            new Zone("DESRT", "Grand Senora Desert", DispatchScannerFiles.areas.GrandeSonoranDesert.FileName, Zone.County.BlaineCounty),
            new Zone("HUMLAB", "Humane Labs and Research", "", Zone.County.BlaineCounty),
            new Zone("JAIL", "Bolingbroke Penitentiary", DispatchScannerFiles.areas.BoilingBrookPenitentiary.FileName, Zone.County.BlaineCounty) { IsRestrictedDuringWanted = true },
            new Zone("LAGO", "Lago Zancudo", DispatchScannerFiles.areas.LagoZancudo.FileName, Zone.County.BlaineCounty),
            new Zone("MTJOSE", "Mount Josiah", DispatchScannerFiles.areas.MtJosiah.FileName, Zone.County.BlaineCounty),
            new Zone("NCHU", "North Chumash", DispatchScannerFiles.areas.NorthChumash.FileName, Zone.County.BlaineCounty),  
            new Zone("SANCHIA", "San Chianski Mountain Range", "", Zone.County.BlaineCounty),
            new Zone("SANDY", "Sandy Shores", DispatchScannerFiles.areas.SandyShores.FileName, Zone.County.BlaineCounty),
            new Zone("SLAB", "Slab City", DispatchScannerFiles.areas.SlabCity.FileName, Zone.County.BlaineCounty),
            new Zone("ZANCUDO", "Zancudo River", DispatchScannerFiles.areas.ZancudoRiver.FileName, Zone.County.BlaineCounty),
            new Zone("ZQ_UAR", "Davis Quartz", DispatchScannerFiles.areas.DavisCourts.FileName, Zone.County.BlaineCounty),

            //Vespucci
            new Zone("BEACH", "Vespucci Beach", DispatchScannerFiles.areas.VespucciBeach.FileName, Zone.County.CityOfLosSantos),
            new Zone("DELBE", "Del Perro Beach", DispatchScannerFiles.areas.DelPierroBeach.FileName, Zone.County.CityOfLosSantos),
            new Zone("DELPE", "Del Perro", DispatchScannerFiles.areas.DelPierro.FileName, Zone.County.CityOfLosSantos),
            new Zone("VCANA", "Vespucci Canals", DispatchScannerFiles.areas.VespucciCanal.FileName, Zone.County.CityOfLosSantos),
            new Zone("VESP", "Vespucci Metro", DispatchScannerFiles.areas.Vespucci.FileName, Zone.County.CityOfLosSantos),
            new Zone("LOSPUER", "La Puerta", DispatchScannerFiles.areas.LaPuertes.FileName, Zone.County.CityOfLosSantos),
            new Zone("PBLUFF", "Pacific Bluffs", DispatchScannerFiles.areas.PacificBluffs.FileName, Zone.County.CityOfLosSantos),
            new Zone("DELSOL", "Puerto Del Sol", DispatchScannerFiles.areas.PuertoDelSoul.FileName, Zone.County.CityOfLosSantos),

            //Central
            new Zone("BANNING", "Banning", DispatchScannerFiles.areas.Banning.FileName, Zone.County.CityOfLosSantos),
            new Zone("CHAMH", "Chamberlain Hills", DispatchScannerFiles.areas.ChamberlainHills.FileName, Zone.County.CityOfLosSantos),
            new Zone("DAVIS", "Davis", DispatchScannerFiles.areas.Davis.FileName, Zone.County.CityOfLosSantos),
            new Zone("DOWNT", "Downtown", DispatchScannerFiles.areas.Downtown.FileName, Zone.County.CityOfLosSantos),
            new Zone("PBOX", "Pillbox Hill", DispatchScannerFiles.areas.PillboxHill.FileName, Zone.County.CityOfLosSantos),
            new Zone("RANCHO", "Rancho", DispatchScannerFiles.areas.Rancho.FileName, Zone.County.CityOfLosSantos),
            new Zone("SKID", "Mission Row", DispatchScannerFiles.areas.MissionRow.FileName, Zone.County.CityOfLosSantos),
            new Zone("STAD", "Maze Bank Arena", DispatchScannerFiles.areas.MazeBankArena.FileName, Zone.County.CityOfLosSantos),
            new Zone("STRAW", "Strawberry", DispatchScannerFiles.areas.Strawberry.FileName, Zone.County.CityOfLosSantos),
            new Zone("TEXTI", "Textile City", DispatchScannerFiles.areas.TextileCity.FileName, Zone.County.CityOfLosSantos),
            new Zone("LEGSQU", "Legion Square", "", Zone.County.CityOfLosSantos),

            //East LS
            new Zone("CYPRE", "Cypress Flats", DispatchScannerFiles.areas.CypressFlats.FileName, Zone.County.CityOfLosSantos),
            new Zone("LMESA", "La Mesa", DispatchScannerFiles.areas.LaMesa.FileName, Zone.County.CityOfLosSantos),
            new Zone("MIRR", "Mirror Park", DispatchScannerFiles.areas.MirrorPark.FileName, Zone.County.CityOfLosSantos),
            new Zone("MURRI", "Murrieta Heights", DispatchScannerFiles.areas.MuriettaHeights.FileName, Zone.County.CityOfLosSantos),
            new Zone("EBURO", "El Burro Heights", DispatchScannerFiles.areas.ElBerroHights.FileName, Zone.County.CityOfLosSantos),

            //Vinewood
            new Zone("ALTA", "Alta", DispatchScannerFiles.areas.Alta.FileName, Zone.County.CityOfLosSantos),
            new Zone("DTVINE", "Downtown Vinewood", DispatchScannerFiles.areas.DowntownVinewood.FileName, Zone.County.CityOfLosSantos),
            new Zone("EAST_V", "East Vinewood", DispatchScannerFiles.areas.EastVinewood.FileName, Zone.County.CityOfLosSantos),
            new Zone("HAWICK", "Hawick", "", Zone.County.CityOfLosSantos),
            new Zone("HORS", "Vinewood Racetrack", DispatchScannerFiles.areas.TheRaceCourse.FileName, Zone.County.CityOfLosSantos),
            new Zone("VINE", "Vinewood", DispatchScannerFiles.areas.Vinewood.FileName, Zone.County.CityOfLosSantos),
            new Zone("WVINE", "West Vinewood", DispatchScannerFiles.areas.WestVinewood.FileName, Zone.County.CityOfLosSantos),

            //PortOfLosSantos
            new Zone("ELYSIAN", "Elysian Island", DispatchScannerFiles.areas.ElysianIsland.FileName, Zone.County.CityOfLosSantos),
            new Zone("ZP_ORT", "Port of South Los Santos", DispatchScannerFiles.areas.PortOfSouthLosSantos.FileName, Zone.County.CityOfLosSantos),
            new Zone("TERMINA", "Terminal", DispatchScannerFiles.areas.Terminal.FileName, Zone.County.CityOfLosSantos),
            new Zone("ZP_ORT", "Port of South Los Santos", DispatchScannerFiles.areas.PortOfSouthLosSantos.FileName, Zone.County.CityOfLosSantos),
            new Zone("AIRP", "Los Santos International Airport", DispatchScannerFiles.areas.LosSantosInternationalAirport.FileName, Zone.County.CityOfLosSantos) { IsRestrictedDuringWanted = true },

            //Rockford Hills
            new Zone("BURTON", "Burton", DispatchScannerFiles.areas.Burton.FileName, Zone.County.CityOfLosSantos),
            new Zone("GOLF", "GWC and Golfing Society", DispatchScannerFiles.areas.TheGWCGolfingSociety.FileName, Zone.County.CityOfLosSantos),
            new Zone("KOREAT", "Little Seoul", DispatchScannerFiles.areas.LittleSeoul.FileName, Zone.County.CityOfLosSantos),
            new Zone("MORN", "Morningwood", DispatchScannerFiles.areas.MorningWood.FileName, Zone.County.CityOfLosSantos),
            new Zone("MOVIE", "Richards Majestic", DispatchScannerFiles.areas.RichardsMajesticStudio.FileName, Zone.County.CityOfLosSantos),
            new Zone("RICHM", "Richman", DispatchScannerFiles.areas.Richman.FileName, Zone.County.CityOfLosSantos),
            new Zone("ROCKF", "Rockford Hills", DispatchScannerFiles.areas.RockfordHills.FileName, Zone.County.CityOfLosSantos),     

            //Vinewood Hills
            new Zone("CHIL", "Vinewood Hills", DispatchScannerFiles.areas.VinewoodHills.FileName, Zone.County.LosSantosCounty),
            new Zone("GREATC", "Great Chaparral", DispatchScannerFiles.areas.GreatChapparalle.FileName, Zone.County.LosSantosCounty),
            new Zone("BAYTRE", "Baytree Canyon", DispatchScannerFiles.areas.BayTreeCanyon.FileName, Zone.County.LosSantosCounty),
            new Zone("RGLEN", "Richman Glen", DispatchScannerFiles.areas.RichmanGlenn.FileName, Zone.County.LosSantosCounty),
            new Zone("TONGVAV", "Tongva Valley", DispatchScannerFiles.areas.TongvaValley.FileName, Zone.County.LosSantosCounty),
            new Zone("HARMO", "Harmony", DispatchScannerFiles.areas.Harmony.FileName, Zone.County.LosSantosCounty),
            new Zone("RTRAK", "Redwood Lights Track", DispatchScannerFiles.areas.TheRedwoodLightsTrack.FileName, Zone.County.LosSantosCounty),
           
            //Chumash
            new Zone("BANHAMC", "Banham Canyon Dr", "", Zone.County.LosSantosCounty),
            new Zone("BHAMCA", "Banham Canyon", "", Zone.County.LosSantosCounty),
            new Zone("CHU", "Chumash", DispatchScannerFiles.areas.Chumash.FileName, Zone.County.LosSantosCounty),
            new Zone("TONGVAH", "Tongva Hills", DispatchScannerFiles.areas.TongaHills.FileName, Zone.County.LosSantosCounty),
           
            //Tataviam 
            new Zone("LACT", "Land Act Reservoir", "", Zone.County.LosSantosCounty),
            new Zone("LDAM", "Land Act Dam", "", Zone.County.LosSantosCounty),
            new Zone("NOOSE", "N.O.O.S.E", "", Zone.County.LosSantosCounty) { IsRestrictedDuringWanted = true },
            new Zone("PALHIGH", "Palomino Highlands", DispatchScannerFiles.areas.PalominoHighlands.FileName, Zone.County.LosSantosCounty),
            new Zone("PALMPOW", "Palmer - Taylor Power Station", DispatchScannerFiles.areas.PalmerTaylorPowerStation.FileName, Zone.County.LosSantosCounty),
            new Zone("SANAND", "San Andreas", DispatchScannerFiles.areas.SanAndreas.FileName, Zone.County.LosSantosCounty),
            new Zone("TATAMO", "Tataviam Mountains", DispatchScannerFiles.areas.TatathiaMountains.FileName, Zone.County.LosSantosCounty),
            new Zone("WINDF", "Ron Alternates Wind Farm", DispatchScannerFiles.areas.RonAlternatesWindFarm.FileName, Zone.County.LosSantosCounty),
    };
        
    }
    public static Zone GetZoneAtLocation(Vector3 ZonePosition)
    {
        string zoneName = GetZoneStringAtLocation(ZonePosition);
        Zone ListResult = ZoneList.Where(x => x.InternalGameName.ToUpper() == zoneName.ToUpper()).FirstOrDefault();
        if(ListResult == null)
        {
            if (ZonePosition.IsInLosSantosCity())
                return new Zone("UNK_LSCITY", "Los Santos", "", Zone.County.CityOfLosSantos);
            else
                return new Zone("UNK_LSCOUNTY", "Los Santos County", "", Zone.County.LosSantosCounty);
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
    public Zone(string _GameName, string _TextName, string _ScannerValue,County _ZoneCounty)
    {
        InternalGameName = _GameName;
        DisplayName = _TextName;
        ScannerValue = _ScannerValue;
        ZoneCounty = _ZoneCounty;
    }
    public string DispatchUnitName { get; set; }
    public string InternalGameName { get; set; }
    public string DisplayName { get; set; }
    public County ZoneCounty { get; set; }
    public string ScannerValue { get; set; }
    public bool IsRestrictedDuringWanted { get; set; } = false;

}
