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
        new Zone("AIRP", "Los Santos International Airport",ScannerAudio.areas.LosSantosInternationalAirport.FileName,PoliceDispatchType.LSPD),
        new Zone("ALAMO", "Alamo Sea",ScannerAudio.areas.TheAlamaSea.FileName,PoliceDispatchType.Sheriff),
        new Zone("ALTA", "Alta",ScannerAudio.areas.Alta.FileName,PoliceDispatchType.LSPD),
        new Zone("ARMYB", "Fort Zancudo",ScannerAudio.areas.FtZancudo.FileName,PoliceDispatchType.Sheriff),
        new Zone("BANHAMC", "Banham Canyon Dr","",PoliceDispatchType.LSPD),
        new Zone("BANNING", "Banning",ScannerAudio.areas.Banning.FileName,PoliceDispatchType.LSPD),
        new Zone("BEACH", "Vespucci Beach",ScannerAudio.areas.VespucciBeach.FileName,PoliceDispatchType.LSPD),
        new Zone("BHAMCA", "Banham Canyon","",PoliceDispatchType.LSPD),
        new Zone("BRADP", "Braddock Pass",ScannerAudio.areas.BraddockPass.FileName,PoliceDispatchType.Sheriff),
        new Zone("BRADT", "Braddock Tunnel",ScannerAudio.areas.TheBraddockTunnel.FileName,PoliceDispatchType.Sheriff),
        new Zone("BURTON", "Burton",ScannerAudio.areas.Burton.FileName,PoliceDispatchType.LSPD),
        new Zone("CALAFB", "Calafia Bridge","",PoliceDispatchType.LSPD),
        new Zone("CANNY", "Raton Canyon",ScannerAudio.areas.RatonCanyon.FileName,PoliceDispatchType.Sheriff),
        new Zone("CCREAK", "Cassidy Creek","",PoliceDispatchType.LSPD),
        new Zone("CHAMH", "Chamberlain Hills",ScannerAudio.areas.ChamberlainHills.FileName,PoliceDispatchType.LSPD),
        new Zone("CHIL", "Vinewood Hills",ScannerAudio.areas.VinewoodHills.FileName,PoliceDispatchType.LSPD),
        new Zone("CHU", "Chumash",ScannerAudio.areas.Chumash.FileName,PoliceDispatchType.Sheriff),
        new Zone("CMSW", "Chiliad Mountain State Wilderness",ScannerAudio.areas.ChilliadMountainStWilderness.FileName,PoliceDispatchType.Sheriff),
        new Zone("CYPRE", "Cypress Flats",ScannerAudio.areas.CypressFlats.FileName,PoliceDispatchType.LSPD),
        new Zone("DAVIS", "Davis",ScannerAudio.areas.Davis.FileName,PoliceDispatchType.LSPD),
        new Zone("DELBE", "Del Perro Beach",ScannerAudio.areas.DelPierroBeach.FileName,PoliceDispatchType.LSPD),
        new Zone("DELPE", "Del Perro",ScannerAudio.areas.DelPierro.FileName,PoliceDispatchType.LSPD),
        new Zone("DELSOL", "La Puerta",ScannerAudio.areas.LaPorta.FileName,PoliceDispatchType.Sheriff),
        new Zone("DESRT", "Grand Senora Desert",ScannerAudio.areas.GrandeSonoranDesert.FileName),
        new Zone("DOWNT", "Downtown",ScannerAudio.areas.Downtown.FileName,PoliceDispatchType.LSPD),
        new Zone("DTVINE", "Downtown Vinewood",ScannerAudio.areas.DowntownVinewood.FileName,PoliceDispatchType.LSPD),
        new Zone("EAST_V", "East Vinewood",ScannerAudio.areas.EastVinewood.FileName,PoliceDispatchType.LSPD),
        new Zone("EBURO", "El Burro Heights",ScannerAudio.areas.ElBerroHights.FileName,PoliceDispatchType.LSPD),
        new Zone("ELGORL", "El Gordo Lighthouse",ScannerAudio.areas.MountGordo.FileName,PoliceDispatchType.Sheriff),
        new Zone("ELYSIAN", "Elysian Island",ScannerAudio.areas.ElysianIsland.FileName,PoliceDispatchType.LSPD),
        new Zone("GALFISH", "Galilee",ScannerAudio.areas.GalileoPark.FileName,PoliceDispatchType.Sheriff),
        new Zone("GOLF", "GWC and Golfing Society",ScannerAudio.areas.TheGWCGolfingSociety.FileName,PoliceDispatchType.LSPD),
        new Zone("GRAPES", "Grapeseed",ScannerAudio.areas.Grapeseed.FileName,PoliceDispatchType.Sheriff),
        new Zone("GREATC", "Great Chaparral",ScannerAudio.areas.GreatChapparalle.FileName,PoliceDispatchType.Sheriff),
        new Zone("HARMO", "Harmony",ScannerAudio.areas.Harmony.FileName,PoliceDispatchType.Sheriff),
        new Zone("HAWICK", "Hawick","",PoliceDispatchType.LSPD),
        new Zone("HORS", "Vinewood Racetrack",ScannerAudio.areas.TheRaceCourse.FileName,PoliceDispatchType.LSPD),
        new Zone("HUMLAB", "Humane Labs and Research","",PoliceDispatchType.LSPD),
        new Zone("JAIL", "Bolingbroke Penitentiary",ScannerAudio.areas.BoilingBrookPenitentiary.FileName,PoliceDispatchType.Sheriff),
        new Zone("KOREAT", "Little Seoul",ScannerAudio.areas.LittleSeoul.FileName,PoliceDispatchType.LSPD),
        new Zone("LACT", "Land Act Reservoir","",PoliceDispatchType.LSPD),
        new Zone("LAGO", "Lago Zancudo",ScannerAudio.areas.LagoZancudo.FileName,PoliceDispatchType.Sheriff),
        new Zone("LDAM", "Land Act Dam","",PoliceDispatchType.LSPD),
        new Zone("LEGSQU", "Legion Square","",PoliceDispatchType.LSPD),
        new Zone("LMESA", "La Mesa",ScannerAudio.areas.LaMesa.FileName,PoliceDispatchType.LSPD),
        new Zone("LOSPUER", "La Puerta",ScannerAudio.areas.LaPuertes.FileName,PoliceDispatchType.LSPD),
        new Zone("MIRR", "Mirror Park",ScannerAudio.areas.MirrorPark.FileName,PoliceDispatchType.LSPD),
        new Zone("MORN", "Morningwood",ScannerAudio.areas.MorningWood.FileName,PoliceDispatchType.LSPD),
        new Zone("MOVIE", "Richards Majestic",ScannerAudio.areas.RichardsMajesticStudio.FileName,PoliceDispatchType.LSPD),
        new Zone("MTCHIL", "Mount Chiliad",ScannerAudio.areas.MountChiliad.FileName,PoliceDispatchType.Sheriff),
        new Zone("MTGORDO", "Mount Gordo",ScannerAudio.areas.MountGordo.FileName,PoliceDispatchType.Sheriff),
        new Zone("MTJOSE", "Mount Josiah",ScannerAudio.areas.MtJosiah.FileName,PoliceDispatchType.Sheriff),
        new Zone("MURRI", "Murrieta Heights",ScannerAudio.areas.MuriettaHeights.FileName,PoliceDispatchType.LSPD),
        new Zone("NCHU", "North Chumash",ScannerAudio.areas.NorthChumash.FileName,PoliceDispatchType.Sheriff),
        new Zone("NOOSE", "N.O.O.S.E","",PoliceDispatchType.LSPD),
        new Zone("OCEANA", "Pacific Ocean",ScannerAudio.areas.TheOcean.FileName,PoliceDispatchType.Sheriff),
        new Zone("PALCOV", "Paleto Cove",ScannerAudio.areas.PaletoBay.FileName,PoliceDispatchType.Sheriff),
        new Zone("PALETO", "Paleto Bay",ScannerAudio.areas.PaletoBay.FileName,PoliceDispatchType.Sheriff),
        new Zone("PALFOR", "Paleto Forest",ScannerAudio.areas.PaletoForest.FileName,PoliceDispatchType.Sheriff),
        new Zone("PALHIGH", "Palomino Highlands",ScannerAudio.areas.PalominoHighlands.FileName,PoliceDispatchType.LSPD),
        new Zone("PALMPOW", "Palmer - Taylor Power Station",ScannerAudio.areas.PalmerTaylorPowerStation.FileName,PoliceDispatchType.Sheriff),
        new Zone("PBLUFF", "Pacific Bluffs",ScannerAudio.areas.PacificBluffs.FileName,PoliceDispatchType.Sheriff),
        new Zone("PBOX", "Pillbox Hill",ScannerAudio.areas.PillboxHill.FileName,PoliceDispatchType.LSPD),
        new Zone("PROCOB", "Procopio Beach",ScannerAudio.areas.ProcopioBeach.FileName,PoliceDispatchType.Sheriff),
        new Zone("RANCHO", "Rancho",ScannerAudio.areas.Rancho.FileName,PoliceDispatchType.LSPD),
        new Zone("RGLEN", "Richman Glen",ScannerAudio.areas.RichmanGlenn.FileName,PoliceDispatchType.LSPD),
        new Zone("RICHM", "Richman",ScannerAudio.areas.Richman.FileName,PoliceDispatchType.LSPD),
        new Zone("ROCKF", "Rockford Hills",ScannerAudio.areas.RockfordHills.FileName,PoliceDispatchType.LSPD),
        new Zone("RTRAK", "Redwood Lights Track",ScannerAudio.areas.TheRedwoodLightsTrack.FileName,PoliceDispatchType.Sheriff),
        new Zone("SANAND", "San Andreas",ScannerAudio.areas.SanAndreas.FileName,PoliceDispatchType.Sheriff),
        new Zone("SANCHIA", "San Chianski Mountain Range","",PoliceDispatchType.Sheriff),
        new Zone("SANDY", "Sandy Shores",ScannerAudio.areas.SandyShores.FileName,PoliceDispatchType.Sheriff),
        new Zone("SKID", "Mission Row",ScannerAudio.areas.MissionRow.FileName,PoliceDispatchType.LSPD),
        new Zone("SLAB", "Slab City",ScannerAudio.areas.SlabCity.FileName,PoliceDispatchType.Sheriff),
        new Zone("STAD", "Maze Bank Arena",ScannerAudio.areas.MazeBankArena.FileName,PoliceDispatchType.LSPD),
        new Zone("STRAW", "Strawberry",ScannerAudio.areas.Strawberry.FileName,PoliceDispatchType.LSPD),
        new Zone("TATAMO", "Tataviam Mountains",ScannerAudio.areas.TatathiaMountains.FileName),
        new Zone("TERMINA", "Terminal",ScannerAudio.areas.Terminal.FileName,PoliceDispatchType.LSPD),
        new Zone("TEXTI", "Textile City",ScannerAudio.areas.TextileCity.FileName,PoliceDispatchType.LSPD),
        new Zone("TONGVAH", "Tongva Hills",ScannerAudio.areas.TongaHills.FileName,PoliceDispatchType.Sheriff),
        new Zone("TONGVAV", "Tongva Valley",ScannerAudio.areas.TongvaValley.FileName,PoliceDispatchType.Sheriff),
        new Zone("VCANA", "Vespucci Canals",ScannerAudio.areas.VespucciCanal.FileName,PoliceDispatchType.Sheriff),
        new Zone("VESP", "Vespucci",ScannerAudio.areas.Vespucci.FileName,PoliceDispatchType.LSPD),
        new Zone("VINE", "Vinewood",ScannerAudio.areas.Vinewood.FileName,PoliceDispatchType.LSPD),
        new Zone("WINDF", "Ron Alternates Wind Farm",ScannerAudio.areas.RonAlternatesWindFarm.FileName,PoliceDispatchType.Sheriff),
        new Zone("WVINE", "West Vinewood",ScannerAudio.areas.WestVinewood.FileName,PoliceDispatchType.LSPD),
        new Zone("ZANCUDO", "Zancudo River",ScannerAudio.areas.ZancudoRiver.FileName,PoliceDispatchType.Sheriff),
        new Zone("ZP_ORT", "Port of South Los Santos",ScannerAudio.areas.PortOfSouthLosSantos.FileName,PoliceDispatchType.LSPD),
        new Zone("ZQ_UAR", "Davis Quartz",ScannerAudio.areas.DavisCourts.FileName,PoliceDispatchType.Sheriff),
    });

    public  class Zone
    {
        public Zone(string _GameName,string _TextName, string _ScannerValue)
        {
            GameName = _GameName;
            TextName = _TextName;
            ScannerValue = _ScannerValue;
        }
        public Zone(string _GameName, string _TextName, string _ScannerValue, PoliceDispatchType _CopsTypeToDispatch)
        {
            GameName = _GameName;
            TextName = _TextName;
            ScannerValue = _ScannerValue;
            CopsTypeToDispatch = _CopsTypeToDispatch;
        }
        public string GameName { get; set; }
        public string TextName { get; set; }
        public string ScannerValue { get; set; }
        public PoliceDispatchType CopsTypeToDispatch { get; set; }
    }
    public enum PoliceDispatchType
    {
        LSPD = 0,
        Sheriff = 1,
    }
}


