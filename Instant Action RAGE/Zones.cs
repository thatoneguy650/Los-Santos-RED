using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


public static class Zones
{
    public static Zone GetZoneName(Vector3 pos)
    {      
        string zoneName = string.Empty;
        Vector3 Position = Game.LocalPlayer.Character.Position;
        unsafe
        {
            IntPtr ptr = Rage.Native.NativeFunction.CallByName<IntPtr>("GET_NAME_OF_ZONE",Position.X, Position.Y, Position.Z);

            zoneName = Marshal.PtrToStringAnsi(ptr);
        }
        return ZoneList.Where(x => x.GameName == zoneName).FirstOrDefault();
    }

    public enum EZones
    {
        AIRP,
        ALAMO,
        ALTA,
        ARMYB,
        BANHAMC,
        BANNING,
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
        ZQ_UAR,
    };

    public static readonly List<Zone> ZoneList = new List<Zone>(new[]
    {
        new Zone("AIRP", "Los Santos International Airport",ScannerAudio.areas.LosSantosInternationalAirport.FileName),
        new Zone("ALAMO", "Alamo Sea",ScannerAudio.areas.TheAlamaSea.FileName),
        new Zone("ALTA", "Alta",ScannerAudio.areas.Alta.FileName),
        new Zone("ARMYB", "Fort Zancudo",ScannerAudio.areas.FtZancudo.FileName),
       // new Zone("BANHAMC", "Banham Canyon Dr",ScannerAudio.areas.BayTreeCanyon.FileName),
        new Zone("BANNING", "Banning",ScannerAudio.areas.Banning.FileName),
        new Zone("BEACH", "Vespucci Beach",ScannerAudio.areas.VespucciBeach.FileName),
        //new Zone("BHAMCA", "Banham Canyon",ScannerAudio.areas..FileName),
        new Zone("BRADP", "Braddock Pass",ScannerAudio.areas.BraddockPass.FileName),
        new Zone("BRADT", "Braddock Tunnel",ScannerAudio.areas.TheBraddockTunnel.FileName),
        new Zone("BURTON", "Burton",ScannerAudio.areas.Burton.FileName),
        //new Zone("CALAFB", "Calafia Bridge",ScannerAudio.areas..FileName),
        new Zone("CANNY", "Raton Canyon",ScannerAudio.areas.RatonCanyon.FileName),
        //new Zone("CCREAK", "Cassidy Creek",ScannerAudio.areas..FileName),
        new Zone("CHAMH", "Chamberlain Hills",ScannerAudio.areas.ChamberlainHills.FileName),
        new Zone("CHIL", "Vinewood Hills",ScannerAudio.areas.VinewoodHills.FileName),
        new Zone("CHU", "Chumash",ScannerAudio.areas.Chumash.FileName),
        new Zone("CMSW", "Chiliad Mountain State Wilderness",ScannerAudio.areas.ChilliadMountainStWilderness.FileName),
        new Zone("CYPRE", "Cypress Flats",ScannerAudio.areas.CypressFlats.FileName),
        new Zone("DAVIS", "Davis",ScannerAudio.areas.Davis.FileName),
        new Zone("DELBE", "Del Perro Beach",ScannerAudio.areas.DelPierroBeach.FileName),
        new Zone("DELPE", "Del Perro",ScannerAudio.areas.DelPierro.FileName),
        new Zone("DELSOL", "La Puerta",ScannerAudio.areas.LaPorta.FileName),
        new Zone("DESRT", "Grand Senora Desert",ScannerAudio.areas.GrandeSonoranDesert.FileName),
        new Zone("DOWNT", "Downtown",ScannerAudio.areas.Downtown.FileName),
        new Zone("DTVINE", "Downtown Vinewood",ScannerAudio.areas.DowntownVinewood.FileName),
        new Zone("EAST_V", "East Vinewood",ScannerAudio.areas.EastVinewood.FileName),
        new Zone("EBURO", "El Burro Heights",ScannerAudio.areas.ElBerroHights.FileName),
        new Zone("ELGORL", "El Gordo Lighthouse",ScannerAudio.areas.MountGordo.FileName),
        new Zone("ELYSIAN", "Elysian Island",ScannerAudio.areas.ElysianIsland.FileName),
        new Zone("GALFISH", "Galilee",ScannerAudio.areas.GalileoPark.FileName),
        new Zone("GOLF", "GWC and Golfing Society",ScannerAudio.areas.TheGWCGolfingSociety.FileName),
        new Zone("GRAPES", "Grapeseed",ScannerAudio.areas.Grapeseed.FileName),
        new Zone("GREATC", "Great Chaparral",ScannerAudio.areas.GreatChapparalle.FileName),
        new Zone("HARMO", "Harmony",ScannerAudio.areas.Harmony.FileName),
        //new Zone("HAWICK", "Hawick",ScannerAudio.areas..FileName),
        new Zone("HORS", "Vinewood Racetrack",ScannerAudio.areas.TheRaceCourse.FileName),
        //new Zone("HUMLAB", "Humane Labs and Research",ScannerAudio.areas.hum.FileName),
        new Zone("JAIL", "Bolingbroke Penitentiary",ScannerAudio.areas.BoilingBrookPenitentiary.FileName),
        new Zone("KOREAT", "Little Seoul",ScannerAudio.areas.LittleSeoul.FileName),
        //new Zone("LACT", "Land Act Reservoir",ScannerAudio.areas..FileName),
        new Zone("LAGO", "Lago Zancudo",ScannerAudio.areas.LagoZancudo.FileName),
        //new Zone("LDAM", "Land Act Dam",ScannerAudio.areas.la.FileName),
       // new Zone("LEGSQU", "Legion Square",ScannerAudio.areas.squ),
        new Zone("LMESA", "La Mesa",ScannerAudio.areas.LaMesa.FileName),
        new Zone("LOSPUER", "La Puerta",ScannerAudio.areas.LaPuertes.FileName),
        new Zone("MIRR", "Mirror Park",ScannerAudio.areas.MirrorPark.FileName),
        new Zone("MORN", "Morningwood",ScannerAudio.areas.MorningWood.FileName),
        new Zone("MOVIE", "Richards Majestic",ScannerAudio.areas.RichardsMajesticStudio.FileName),
        new Zone("MTCHIL", "Mount Chiliad",ScannerAudio.areas.MountChiliad.FileName),
        new Zone("MTGORDO", "Mount Gordo",ScannerAudio.areas.MountGordo.FileName),
        new Zone("MTJOSE", "Mount Josiah",ScannerAudio.areas.MtJosiah.FileName),
        new Zone("MURRI", "Murrieta Heights",ScannerAudio.areas.MuriettaHeights.FileName),
        new Zone("NCHU", "North Chumash",ScannerAudio.areas.NorthChumash.FileName),
        //new Zone("NOOSE", "N.O.O.S.E",ScannerAudio.areas.noo.FileName),
        new Zone("OCEANA", "Pacific Ocean",ScannerAudio.areas.TheOcean.FileName),
        new Zone("PALCOV", "Paleto Cove",ScannerAudio.areas.PaletoBay.FileName),
        new Zone("PALETO", "Paleto Bay",ScannerAudio.areas.PaletoBay.FileName),
        new Zone("PALFOR", "Paleto Forest",ScannerAudio.areas.PaletoForest.FileName),
        new Zone("PALHIGH", "Palomino Highlands",ScannerAudio.areas.PalominoHighlands.FileName),
        new Zone("PALMPOW", "Palmer - Taylor Power Station",ScannerAudio.areas.PalmerTaylorPowerStation.FileName),
        new Zone("PBLUFF", "Pacific Bluffs",ScannerAudio.areas.PacificBluffs.FileName),
        new Zone("PBOX", "Pillbox Hill",ScannerAudio.areas.PillboxHill.FileName),
        new Zone("PROCOB", "Procopio Beach",ScannerAudio.areas.ProcopioBeach.FileName),
        new Zone("RANCHO", "Rancho",ScannerAudio.areas.Rancho.FileName),
        new Zone("RGLEN", "Richman Glen",ScannerAudio.areas.RichmanGlenn.FileName),
        new Zone("RICHM", "Richman",ScannerAudio.areas.Richman.FileName),
        new Zone("ROCKF", "Rockford Hills",ScannerAudio.areas.RockfordHills.FileName),
        new Zone("RTRAK", "Redwood Lights Track",ScannerAudio.areas.TheRedwoodLightsTrack.FileName),
        new Zone("SANAND", "San Andreas",ScannerAudio.areas.SanAndreas.FileName),
        //new Zone("SANCHIA", "San Chianski Mountain Range",ScannerAudio.areas.TatathiaMountains.FileName),
        new Zone("SANDY", "Sandy Shores",ScannerAudio.areas.SandyShores.FileName),
        new Zone("SKID", "Mission Row",ScannerAudio.areas.MissionRow.FileName),
        new Zone("SLAB", "Slab City",ScannerAudio.areas.SlabCity.FileName),
        new Zone("STAD", "Maze Bank Arena",ScannerAudio.areas.MazeBankArena.FileName),
        new Zone("STRAW", "Strawberry",ScannerAudio.areas.Strawberry.FileName),
        new Zone("TATAMO", "Tataviam Mountains",ScannerAudio.areas.TatathiaMountains.FileName),
        new Zone("TERMINA", "Terminal",ScannerAudio.areas.Terminal.FileName),
        new Zone("TEXTI", "Textile City",ScannerAudio.areas.TextileCity.FileName),
        new Zone("TONGVAH", "Tongva Hills",ScannerAudio.areas.TongaHills.FileName),
        new Zone("TONGVAV", "Tongva Valley",ScannerAudio.areas.TongvaValley.FileName),
        new Zone("VCANA", "Vespucci Canals",ScannerAudio.areas.VespucciCanal.FileName),
        new Zone("VESP", "Vespucci",ScannerAudio.areas.Vespucci.FileName),
        new Zone("VINE", "Vinewood",ScannerAudio.areas.Vinewood.FileName),
        new Zone("WINDF", "Ron Alternates Wind Farm",ScannerAudio.areas.RonAlternatesWindFarm.FileName),
        new Zone("WVINE", "West Vinewood",ScannerAudio.areas.WestVinewood.FileName),
        new Zone("ZANCUDO", "Zancudo River",ScannerAudio.areas.ZancudoRiver.FileName),
        new Zone("ZP_ORT", "Port of South Los Santos",ScannerAudio.areas.PortOfSouthLosSantos.FileName),
        new Zone("ZQ_UAR", "Davis Quartz",ScannerAudio.areas.DavisCourts.FileName),
    });

    public  class Zone
    {   public Zone(string _GameName,string _TextName, string _ScannerValue)
        {
            GameName = _GameName;
            TextName = _TextName;
            ScannerValue = _ScannerValue;
        }
        public string GameName { get; set; }
        public string TextName { get; set; }
        public string ScannerValue { get; set; }
    }
}


