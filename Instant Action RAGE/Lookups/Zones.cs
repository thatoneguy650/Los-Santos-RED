using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

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

    public static readonly List<Zone> ZoneList = new List<Zone>(new[]
    {
        new Zone("AIRP", "Los Santos International Airport",ScannerAudio.areas.LosSantosInternationalAirport.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("ALAMO", "Alamo Sea",ScannerAudio.areas.TheAlamaSea.FileName,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("ALTA", "Alta",ScannerAudio.areas.Alta.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("ARMYB", "Fort Zancudo",ScannerAudio.areas.FtZancudo.FileName,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("BANHAMC", "Banham Canyon Dr","",Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("BANNING", "Banning",ScannerAudio.areas.Banning.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("BEACH", "Vespucci Beach",ScannerAudio.areas.VespucciBeach.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("BHAMCA", "Banham Canyon","",Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("BRADP", "Braddock Pass",ScannerAudio.areas.BraddockPass.FileName,Agencies.LSSD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("BRADT", "Braddock Tunnel",ScannerAudio.areas.TheBraddockTunnel.FileName,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("BURTON", "Burton",ScannerAudio.areas.Burton.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("CALAFB", "Calafia Bridge","",Agencies.SAPR),
        new Zone("CANNY", "Raton Canyon",ScannerAudio.areas.RatonCanyon.FileName,Agencies.SAPR),
        new Zone("CCREAK", "Cassidy Creek","",Agencies.SAPR),
        new Zone("CHAMH", "Chamberlain Hills",ScannerAudio.areas.ChamberlainHills.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("CHIL", "Vinewood Hills",ScannerAudio.areas.VinewoodHills.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("CHU", "Chumash",ScannerAudio.areas.Chumash.FileName,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("CMSW", "Chiliad Mountain State Wilderness",ScannerAudio.areas.ChilliadMountainStWilderness.FileName,Agencies.SAPR),
        new Zone("CYPRE", "Cypress Flats",ScannerAudio.areas.CypressFlats.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("DAVIS", "Davis",ScannerAudio.areas.Davis.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("DELBE", "Del Perro Beach",ScannerAudio.areas.DelPierroBeach.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("DELPE", "Del Perro",ScannerAudio.areas.DelPierro.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("DELSOL", "Puerto Del Sol",ScannerAudio.areas.LaPorta.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("DESRT", "Grand Senora Desert",ScannerAudio.areas.GrandeSonoranDesert.FileName,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("DOWNT", "Downtown",ScannerAudio.areas.Downtown.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("DTVINE", "Downtown Vinewood",ScannerAudio.areas.DowntownVinewood.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("EAST_V", "East Vinewood",ScannerAudio.areas.EastVinewood.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("EBURO", "El Burro Heights",ScannerAudio.areas.ElBerroHights.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("ELGORL", "El Gordo Lighthouse",ScannerAudio.areas.MountGordo.FileName,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("ELYSIAN", "Elysian Island",ScannerAudio.areas.ElysianIsland.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("GALFISH", "Galilee",ScannerAudio.areas.GalileoPark.FileName,Agencies.SAPR),
        new Zone("GOLF", "GWC and Golfing Society",ScannerAudio.areas.TheGWCGolfingSociety.FileName,Agencies.LSPD),
        new Zone("GRAPES", "Grapeseed",ScannerAudio.areas.Grapeseed.FileName,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("GREATC", "Great Chaparral",ScannerAudio.areas.GreatChapparalle.FileName,Agencies.SAPR),
        new Zone("HARMO", "Harmony",ScannerAudio.areas.Harmony.FileName,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("HAWICK", "Hawick","",Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("HORS", "Vinewood Racetrack",ScannerAudio.areas.TheRaceCourse.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("HUMLAB", "Humane Labs and Research","",Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("JAIL", "Bolingbroke Penitentiary",ScannerAudio.areas.BoilingBrookPenitentiary.FileName,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("KOREAT", "Little Seoul",ScannerAudio.areas.LittleSeoul.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("LACT", "Land Act Reservoir","",Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("LAGO", "Lago Zancudo",ScannerAudio.areas.LagoZancudo.FileName,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("LDAM", "Land Act Dam","",Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("LEGSQU", "Legion Square","",Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("LMESA", "La Mesa",ScannerAudio.areas.LaMesa.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("LOSPUER", "La Puerta",ScannerAudio.areas.LaPuertes.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("MIRR", "Mirror Park",ScannerAudio.areas.MirrorPark.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("MORN", "Morningwood",ScannerAudio.areas.MorningWood.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("MOVIE", "Richards Majestic",ScannerAudio.areas.RichardsMajesticStudio.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("MTCHIL", "Mount Chiliad",ScannerAudio.areas.MountChiliad.FileName,Agencies.SAPR),
        new Zone("MTGORDO", "Mount Gordo",ScannerAudio.areas.MountGordo.FileName,Agencies.SAPR),
        new Zone("MTJOSE", "Mount Josiah",ScannerAudio.areas.MtJosiah.FileName,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("MURRI", "Murrieta Heights",ScannerAudio.areas.MuriettaHeights.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("NCHU", "North Chumash",ScannerAudio.areas.NorthChumash.FileName,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("NOOSE", "N.O.O.S.E","",Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("OCEANA", "Pacific Ocean",ScannerAudio.areas.TheOcean.FileName,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("PALCOV", "Paleto Cove",ScannerAudio.areas.PaletoBay.FileName,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("PALETO", "Paleto Bay",ScannerAudio.areas.PaletoBay.FileName,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("PALFOR", "Paleto Forest",ScannerAudio.areas.PaletoForest.FileName,Agencies.SAPR),
        new Zone("PALHIGH", "Palomino Highlands",ScannerAudio.areas.PalominoHighlands.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("PALMPOW", "Palmer - Taylor Power Station",ScannerAudio.areas.PalmerTaylorPowerStation.FileName,Agencies.LSSD),
        new Zone("PBLUFF", "Pacific Bluffs",ScannerAudio.areas.PacificBluffs.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("PBOX", "Pillbox Hill",ScannerAudio.areas.PillboxHill.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("PROCOB", "Procopio Beach",ScannerAudio.areas.ProcopioBeach.FileName,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("RANCHO", "Rancho",ScannerAudio.areas.Rancho.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("RGLEN", "Richman Glen",ScannerAudio.areas.RichmanGlenn.FileName,Agencies.LSPD),
        new Zone("RICHM", "Richman",ScannerAudio.areas.Richman.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("ROCKF", "Rockford Hills",ScannerAudio.areas.RockfordHills.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("RTRAK", "Redwood Lights Track",ScannerAudio.areas.TheRedwoodLightsTrack.FileName,Agencies.LSSD),
        new Zone("SANAND", "San Andreas",ScannerAudio.areas.SanAndreas.FileName,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("SANCHIA", "San Chianski Mountain Range","",Agencies.SAPR),
        new Zone("SANDY", "Sandy Shores",ScannerAudio.areas.SandyShores.FileName,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("SKID", "Mission Row",ScannerAudio.areas.MissionRow.FileName,Agencies.LSPD,new List<Agency>() { Agencies.DOA,Agencies.FIB }),
        new Zone("SLAB", "Slab City",ScannerAudio.areas.SlabCity.FileName,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("STAD", "Maze Bank Arena",ScannerAudio.areas.MazeBankArena.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("STRAW", "Strawberry",ScannerAudio.areas.Strawberry.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("TATAMO", "Tataviam Mountains",ScannerAudio.areas.TatathiaMountains.FileName,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("TERMINA", "Terminal",ScannerAudio.areas.Terminal.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("TEXTI", "Textile City",ScannerAudio.areas.TextileCity.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("TONGVAH", "Tongva Hills",ScannerAudio.areas.TongaHills.FileName,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("TONGVAV", "Tongva Valley",ScannerAudio.areas.TongvaValley.FileName,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("VCANA", "Vespucci Canals",ScannerAudio.areas.VespucciCanal.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("VESP", "Vespucci",ScannerAudio.areas.Vespucci.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("VINE", "Vinewood",ScannerAudio.areas.Vinewood.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("WINDF", "Ron Alternates Wind Farm",ScannerAudio.areas.RonAlternatesWindFarm.FileName,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
        new Zone("WVINE", "West Vinewood",ScannerAudio.areas.WestVinewood.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("ZANCUDO", "Zancudo River",ScannerAudio.areas.ZancudoRiver.FileName,Agencies.SAPR),
        new Zone("ZP_ORT", "Port of South Los Santos",ScannerAudio.areas.PortOfSouthLosSantos.FileName,Agencies.LSPD,new List<Agency>() { Agencies.IAA,Agencies.DOA,Agencies.FIB }),
        new Zone("ZQ_UAR", "Davis Quartz",ScannerAudio.areas.DavisCourts.FileName,Agencies.LSSD,new List<Agency>() { Agencies.DOA }),
    });

    public  class Zone
    {
        public Zone(string _GameName, string _TextName, string _ScannerValue, Agency _MainZoneAgency)
        {
            GameName = _GameName;
            TextName = _TextName;
            ScannerValue = _ScannerValue;
            MainZoneAgency = _MainZoneAgency;
        }
        public Zone(string _GameName, string _TextName, string _ScannerValue, Agency _MainZoneAgency,List<Agency> _SecondaryZoneAgencies)
        {
            GameName = _GameName;
            TextName = _TextName;
            ScannerValue = _ScannerValue;
            MainZoneAgency = _MainZoneAgency;
            SecondaryZoneAgencies = _SecondaryZoneAgencies;
        }
        public string GameName { get; set; }
        public string TextName { get; set; }
        public string ScannerValue { get; set; }
        public Agency MainZoneAgency { get; set; }
        public List<Agency> SecondaryZoneAgencies { get; set; } = new List<Agency>();
    }
}


