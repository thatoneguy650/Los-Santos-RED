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
        new Zone("AIRP", "Los Santos International Airport",ScannerAudio.areas.LosSantosInternationalAirport.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("ALAMO", "Alamo Sea",ScannerAudio.areas.TheAlamaSea.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.DOA }),
        new Zone("ALTA", "Alta",ScannerAudio.areas.Alta.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("ARMYB", "Fort Zancudo",ScannerAudio.areas.FtZancudo.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.DOA }),
        new Zone("BANHAMC", "Banham Canyon Dr","",PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("BANNING", "Banning",ScannerAudio.areas.Banning.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("BEACH", "Vespucci Beach",ScannerAudio.areas.VespucciBeach.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("BHAMCA", "Banham Canyon","",PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("BRADP", "Braddock Pass",ScannerAudio.areas.BraddockPass.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("BRADT", "Braddock Tunnel",ScannerAudio.areas.TheBraddockTunnel.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.DOA }),
        new Zone("BURTON", "Burton",ScannerAudio.areas.Burton.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("CALAFB", "Calafia Bridge","",PoliceScanningSystem.SAPR),
        new Zone("CANNY", "Raton Canyon",ScannerAudio.areas.RatonCanyon.FileName,PoliceScanningSystem.SAPR),
        new Zone("CCREAK", "Cassidy Creek","",PoliceScanningSystem.SAPR),
        new Zone("CHAMH", "Chamberlain Hills",ScannerAudio.areas.ChamberlainHills.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("CHIL", "Vinewood Hills",ScannerAudio.areas.VinewoodHills.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("CHU", "Chumash",ScannerAudio.areas.Chumash.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.DOA }),
        new Zone("CMSW", "Chiliad Mountain State Wilderness",ScannerAudio.areas.ChilliadMountainStWilderness.FileName,PoliceScanningSystem.SAPR),
        new Zone("CYPRE", "Cypress Flats",ScannerAudio.areas.CypressFlats.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("DAVIS", "Davis",ScannerAudio.areas.Davis.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("DELBE", "Del Perro Beach",ScannerAudio.areas.DelPierroBeach.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("DELPE", "Del Perro",ScannerAudio.areas.DelPierro.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("DELSOL", "Puerto Del Sol",ScannerAudio.areas.LaPorta.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("DESRT", "Grand Senora Desert",ScannerAudio.areas.GrandeSonoranDesert.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.DOA }),
        new Zone("DOWNT", "Downtown",ScannerAudio.areas.Downtown.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("DTVINE", "Downtown Vinewood",ScannerAudio.areas.DowntownVinewood.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("EAST_V", "East Vinewood",ScannerAudio.areas.EastVinewood.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("EBURO", "El Burro Heights",ScannerAudio.areas.ElBerroHights.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("ELGORL", "El Gordo Lighthouse",ScannerAudio.areas.MountGordo.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.DOA }),
        new Zone("ELYSIAN", "Elysian Island",ScannerAudio.areas.ElysianIsland.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("GALFISH", "Galilee",ScannerAudio.areas.GalileoPark.FileName,PoliceScanningSystem.SAPR),
        new Zone("GOLF", "GWC and Golfing Society",ScannerAudio.areas.TheGWCGolfingSociety.FileName,PoliceScanningSystem.LSPD),
        new Zone("GRAPES", "Grapeseed",ScannerAudio.areas.Grapeseed.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.DOA }),
        new Zone("GREATC", "Great Chaparral",ScannerAudio.areas.GreatChapparalle.FileName,PoliceScanningSystem.SAPR),
        new Zone("HARMO", "Harmony",ScannerAudio.areas.Harmony.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.DOA }),
        new Zone("HAWICK", "Hawick","",PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("HORS", "Vinewood Racetrack",ScannerAudio.areas.TheRaceCourse.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("HUMLAB", "Humane Labs and Research","",PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("JAIL", "Bolingbroke Penitentiary",ScannerAudio.areas.BoilingBrookPenitentiary.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.DOA }),
        new Zone("KOREAT", "Little Seoul",ScannerAudio.areas.LittleSeoul.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("LACT", "Land Act Reservoir","",PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("LAGO", "Lago Zancudo",ScannerAudio.areas.LagoZancudo.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.DOA }),
        new Zone("LDAM", "Land Act Dam","",PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("LEGSQU", "Legion Square","",PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("LMESA", "La Mesa",ScannerAudio.areas.LaMesa.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("LOSPUER", "La Puerta",ScannerAudio.areas.LaPuertes.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("MIRR", "Mirror Park",ScannerAudio.areas.MirrorPark.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("MORN", "Morningwood",ScannerAudio.areas.MorningWood.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("MOVIE", "Richards Majestic",ScannerAudio.areas.RichardsMajesticStudio.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("MTCHIL", "Mount Chiliad",ScannerAudio.areas.MountChiliad.FileName,PoliceScanningSystem.SAPR),
        new Zone("MTGORDO", "Mount Gordo",ScannerAudio.areas.MountGordo.FileName,PoliceScanningSystem.SAPR),
        new Zone("MTJOSE", "Mount Josiah",ScannerAudio.areas.MtJosiah.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.DOA }),
        new Zone("MURRI", "Murrieta Heights",ScannerAudio.areas.MuriettaHeights.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("NCHU", "North Chumash",ScannerAudio.areas.NorthChumash.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.DOA }),
        new Zone("NOOSE", "N.O.O.S.E","",PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("OCEANA", "Pacific Ocean",ScannerAudio.areas.TheOcean.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.DOA }),
        new Zone("PALCOV", "Paleto Cove",ScannerAudio.areas.PaletoBay.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.DOA }),
        new Zone("PALETO", "Paleto Bay",ScannerAudio.areas.PaletoBay.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.DOA }),
        new Zone("PALFOR", "Paleto Forest",ScannerAudio.areas.PaletoForest.FileName,PoliceScanningSystem.SAPR),
        new Zone("PALHIGH", "Palomino Highlands",ScannerAudio.areas.PalominoHighlands.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("PALMPOW", "Palmer - Taylor Power Station",ScannerAudio.areas.PalmerTaylorPowerStation.FileName,PoliceScanningSystem.LSSD),
        new Zone("PBLUFF", "Pacific Bluffs",ScannerAudio.areas.PacificBluffs.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("PBOX", "Pillbox Hill",ScannerAudio.areas.PillboxHill.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("PROCOB", "Procopio Beach",ScannerAudio.areas.ProcopioBeach.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.DOA }),
        new Zone("RANCHO", "Rancho",ScannerAudio.areas.Rancho.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("RGLEN", "Richman Glen",ScannerAudio.areas.RichmanGlenn.FileName,PoliceScanningSystem.LSPD),
        new Zone("RICHM", "Richman",ScannerAudio.areas.Richman.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("ROCKF", "Rockford Hills",ScannerAudio.areas.RockfordHills.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("RTRAK", "Redwood Lights Track",ScannerAudio.areas.TheRedwoodLightsTrack.FileName,PoliceScanningSystem.LSSD),
        new Zone("SANAND", "San Andreas",ScannerAudio.areas.SanAndreas.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.DOA }),
        new Zone("SANCHIA", "San Chianski Mountain Range","",PoliceScanningSystem.SAPR),
        new Zone("SANDY", "Sandy Shores",ScannerAudio.areas.SandyShores.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.DOA }),
        new Zone("SKID", "Mission Row",ScannerAudio.areas.MissionRow.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("SLAB", "Slab City",ScannerAudio.areas.SlabCity.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.DOA }),
        new Zone("STAD", "Maze Bank Arena",ScannerAudio.areas.MazeBankArena.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("STRAW", "Strawberry",ScannerAudio.areas.Strawberry.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("TATAMO", "Tataviam Mountains",ScannerAudio.areas.TatathiaMountains.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.DOA }),
        new Zone("TERMINA", "Terminal",ScannerAudio.areas.Terminal.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("TEXTI", "Textile City",ScannerAudio.areas.TextileCity.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("TONGVAH", "Tongva Hills",ScannerAudio.areas.TongaHills.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.DOA }),
        new Zone("TONGVAV", "Tongva Valley",ScannerAudio.areas.TongvaValley.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.DOA }),
        new Zone("VCANA", "Vespucci Canals",ScannerAudio.areas.VespucciCanal.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("VESP", "Vespucci",ScannerAudio.areas.Vespucci.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("VINE", "Vinewood",ScannerAudio.areas.Vinewood.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("WINDF", "Ron Alternates Wind Farm",ScannerAudio.areas.RonAlternatesWindFarm.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.DOA }),
        new Zone("WVINE", "West Vinewood",ScannerAudio.areas.WestVinewood.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("ZANCUDO", "Zancudo River",ScannerAudio.areas.ZancudoRiver.FileName,PoliceScanningSystem.SAPR),
        new Zone("ZP_ORT", "Port of South Los Santos",ScannerAudio.areas.PortOfSouthLosSantos.FileName,PoliceScanningSystem.LSPD,new List<Agency>() { PoliceScanningSystem.IAA,PoliceScanningSystem.DOA,PoliceScanningSystem.FIB }),
        new Zone("ZQ_UAR", "Davis Quartz",ScannerAudio.areas.DavisCourts.FileName,PoliceScanningSystem.LSSD,new List<Agency>() { PoliceScanningSystem.DOA }),
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
        public List<Agency> SecondaryZoneAgencies { get; set; }
    }
}


