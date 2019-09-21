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
        new Zone("AIRP", "Los Santos International Airport",Scanner.Areas.AREA_LOS_SANTOS_INTERNATIONAL_01.Value),
        new Zone("ALAMO", "Alamo Sea",Scanner.Areas.AREA_THE_ALAMO_SEA_01.Value),
        new Zone("ALTA", "Alta",Scanner.Areas.AREA_ALTA_01.Value),
        new Zone("ARMYB", "Fort Zancudo",Scanner.Areas.AREA_FORT_ZANCUDO_01.Value),
        new Zone("BANHAMC", "Banham Canyon Dr",Scanner.Areas.AREA_BANHAM_CANYON_01.Value),
        new Zone("BANNING", "Banning",Scanner.Areas.AREA_BANNING_01.Value),
        new Zone("BEACH", "Vespucci Beach",Scanner.Areas.AREA_VESPUCCI_01.Value),
        new Zone("BHAMCA", "Banham Canyon",Scanner.Areas.AREA_BANHAM_CANYON_01.Value),
        new Zone("BRADP", "Braddock Pass",Scanner.Areas.AREA_BRADDOCK_PASS_01.Value),
        new Zone("BRADT", "Braddock Tunnel",Scanner.Areas.AREA_THE_BRADDOCK_TUNNEL_01.Value),
        new Zone("BURTON", "Burton",Scanner.Areas.AREA_BURTON_01.Value),
        new Zone("CALAFB", "Calafia Bridge",Scanner.Areas.AREA_THE_CALAFIA_BRIDGE_01.Value),
        new Zone("CANNY", "Raton Canyon",Scanner.Areas.AREA_RATON_CANYON_01.Value),
        new Zone("CCREAK", "Cassidy Creek",Scanner.Areas.AREA_CASSIDY_CREEK_01.Value),
        new Zone("CHAMH", "Chamberlain Hills",Scanner.Areas.AREA_CHAMBERLAIN_HILLS_01.Value),
        new Zone("CHIL", "Vinewood Hills",Scanner.Areas.AREA_VINEWOOD_HILLS_01.Value),
        new Zone("CHU", "Chumash",Scanner.Areas.AREA_CHUMASH_01.Value),
        new Zone("CMSW", "Chiliad Mountain State Wilderness",Scanner.Areas.AREA_THE_CHILLIAD_MOUNTAIN_STATE_WILDERNESS_01.Value),
        new Zone("CYPRE", "Cypress Flats",Scanner.Areas.AREA_CYPRESS_FLATS_01.Value),
        new Zone("DAVIS", "Davis",Scanner.Areas.AREA_DAVIS_01.Value),
        new Zone("DELBE", "Del Perro Beach",Scanner.Areas.AREA_DEL_PERRO_BEACH_01.Value),
        new Zone("DELPE", "Del Perro",Scanner.Areas.AREA_DEL_PERRO_01.Value),
        new Zone("DELSOL", "La Puerta",Scanner.Areas.AREA_LA_PUERTA_01.Value),
        new Zone("DESRT", "Grand Senora Desert",Scanner.Areas.AREA_GRANDE_SENORA_DESERT_01.Value),
        new Zone("DOWNT", "Downtown",Scanner.Areas.AREA_DOWNTOWN_01.Value),
        new Zone("DTVINE", "Downtown Vinewood",Scanner.Areas.AREA_DOWNTOWN_VINEWOOD_01.Value),
        new Zone("EAST_V", "East Vinewood",Scanner.Areas.AREA_EAST_VINEWOOD_01.Value),
        new Zone("EBURO", "El Burro Heights",Scanner.Areas.AREA_EL_BURRO_HEIGHTS_01.Value),
        new Zone("ELGORL", "El Gordo Lighthouse",Scanner.Areas.AREA_MOUNT_GORDO_01.Value),
        new Zone("ELYSIAN", "Elysian Island",Scanner.Areas.AREA_ELYSIAN_ISLAND_01.Value),
        new Zone("GALFISH", "Galilee",Scanner.Areas.AREA_GALILEE_01.Value),
        new Zone("GOLF", "GWC and Golfing Society",Scanner.Areas.AREA_GWC_GOLF_CLUB_01.Value),
        new Zone("GRAPES", "Grapeseed",Scanner.Areas.AREA_GRAPESEED_01.Value),
        new Zone("GREATC", "Great Chaparral",Scanner.Areas.AREA_GREAT_CHAPARRAL_01.Value),
        new Zone("HARMO", "Harmony",Scanner.Areas.AREA_HARMONY_01.Value),
        new Zone("HAWICK", "Hawick",Scanner.Areas.AREA_HAWICK_01.Value),
        new Zone("HORS", "Vinewood Racetrack",Scanner.Areas.AREA_VINEWOOD_RACETRACK_01.Value),
        new Zone("HUMLAB", "Humane Labs and Research",Scanner.Areas.AREA_HUMANE_LABS.Value),
        new Zone("JAIL", "Bolingbroke Penitentiary",Scanner.Areas.AREA_BOLLINGBROKE_PENITENTIARY_01.Value),
        new Zone("KOREAT", "Little Seoul",Scanner.Areas.AREA_LITTLE_SEOUL_01.Value),
        new Zone("LACT", "Land Act Reservoir",Scanner.Areas.AREA_LAND_ACT_RESERVOIR_01.Value),
        new Zone("LAGO", "Lago Zancudo",Scanner.Areas.AREA_LAGO_ZANCUDO_01.Value),
        new Zone("LDAM", "Land Act Dam",Scanner.Areas.AREA_LAND_ACT_DAM_01.Value),
        //new Zone("LEGSQU", "Legion Square",""),
        new Zone("LMESA", "La Mesa",Scanner.Areas.AREA_LA_MESA_01.Value),
        new Zone("LOSPUER", "La Puerta",Scanner.Areas.AREA_LA_PUERTA_01.Value),
        new Zone("MIRR", "Mirror Park",Scanner.Areas.AREA_MIRROR_PARK_01.Value),
        new Zone("MORN", "Morningwood",Scanner.Areas.AREA_MORNINGWOOD_01.Value),
        //new Zone("MOVIE", "Richards Majestic",Scanner.Areas.AREA_RICHMAN_01.Value),
        new Zone("MTCHIL", "Mount Chiliad",Scanner.Areas.AREA_MOUNT_CHILLIAD_01.Value),
        new Zone("MTGORDO", "Mount Gordo",Scanner.Areas.AREA_MOUNT_GORDO_01.Value),
        new Zone("MTJOSE", "Mount Josiah",Scanner.Areas.AREA_MOUNT_JOSIAH_01.Value),
        new Zone("MURRI", "Murrieta Heights",Scanner.Areas.AREA_MURRIETA_HEIGHTS_01.Value),
        new Zone("NCHU", "North Chumash",Scanner.Areas.AREA_NORTH_CHUMASH_01.Value),
        new Zone("NOOSE", "N.O.O.S.E",Scanner.Areas.AREA_NOOSE_HQ_01.Value),
        new Zone("OCEANA", "Pacific Ocean",Scanner.Areas.AREA_PACIFIC_OCEAN_01.Value),
        new Zone("PALCOV", "Paleto Cove",Scanner.Areas.AREA_PALETO_COVE_01.Value),
        new Zone("PALETO", "Paleto Bay",Scanner.Areas.AREA_PALETO_BAY_01.Value),
        new Zone("PALFOR", "Paleto Forest",Scanner.Areas.AREA_PALETO_FOREST_01.Value),
        new Zone("PALHIGH", "Palomino Highlands",Scanner.Areas.AREA_PALOMINO_HIGHLANDS_01.Value),
        new Zone("PALMPOW", "Palmer - Taylor Power Station",Scanner.Areas.AREA_PALMER_TAYLOR_POWER_STATION_01.Value),
        new Zone("PBLUFF", "Pacific Bluffs",Scanner.Areas.AREA_PACIFIC_BLUFFS_01.Value),
        new Zone("PBOX", "Pillbox Hill",Scanner.Areas.AREA_PILLBOX_HILL_01.Value),
        new Zone("PROCOB", "Procopio Beach",Scanner.Areas.AREA_PROCOPIO_BEACH_01.Value),
        new Zone("RANCHO", "Rancho",Scanner.Areas.AREA_RANCHO_01.Value),
        new Zone("RGLEN", "Richman Glen",Scanner.Areas.AREA_RICHMAN_GLEN_01.Value),
        new Zone("RICHM", "Richman",Scanner.Areas.AREA_RICHMAN_01.Value),
        new Zone("ROCKF", "Rockford Hills",Scanner.Areas.AREA_ROCKFORD_HILLS_01.Value),
        new Zone("RTRAK", "Redwood Lights Track",Scanner.Areas.AREA_THE_REDWOOD_LIGHTS_TRACK_01.Value),
        new Zone("SANAND", "San Andreas",Scanner.Areas.AREA_SAN_ANDREAS_01.Value),
        new Zone("SANCHIA", "San Chianski Mountain Range",Scanner.Areas.AREA_SAN_CHIANSKI_MOUNTAINS_01.Value),
        new Zone("SANDY", "Sandy Shores",Scanner.Areas.AREA_SANDY_SHORES_01.Value),
        new Zone("SKID", "Mission Row",Scanner.Areas.AREA_MISSION_ROW_01.Value),
        new Zone("SLAB", "Stab City",Scanner.Areas.AREA_STAB_CITY_01.Value),
        new Zone("STAD", "Maze Bank Arena",Scanner.Areas.AREA_MAZE_BANK_ARENA_01.Value),
        new Zone("STRAW", "Strawberry",Scanner.Areas.AREA_STRAWBERRY_01.Value),
        new Zone("TATAMO", "Tataviam Mountains",Scanner.Areas.AREA_TATAVIAM_MOUNTAINS_01.Value),
        new Zone("TERMINA", "Terminal",Scanner.Areas.AREA_TERMINAL_01.Value),
        new Zone("TEXTI", "Textile City",Scanner.Areas.AREA_TEXTILE_CITY_01.Value),
        new Zone("TONGVAH", "Tongva Hills",Scanner.Areas.AREA_TONGVA_HILLS_01.Value),
        new Zone("TONGVAV", "Tongva Valley",Scanner.Areas.AREA_TONGVA_VALLEY_01.Value),
        new Zone("VCANA", "Vespucci Canals",Scanner.Areas.AREA_VESPUCCI_CANALS_01.Value),
        new Zone("VESP", "Vespucci",Scanner.Areas.AREA_VESPUCCI_01.Value),
        new Zone("VINE", "Vinewood",Scanner.Areas.AREA_VINEWOOD_01.Value),
        new Zone("WINDF", "Ron Alternates Wind Farm",Scanner.Areas.AREA_RON_ALTERNATES_WINDFARM_01.Value),
        new Zone("WVINE", "West Vinewood",Scanner.Areas.AREA_WEST_VINEWOOD_01.Value),
        new Zone("ZANCUDO", "Zancudo River",Scanner.Areas.AREA_ZANCUDO_RIVER_01.Value),
        new Zone("ZP_ORT", "Port of South Los Santos",Scanner.Areas.AREA_PORT_OF_SOUTH_LOS_SANTOS_01.Value),
        new Zone("ZQ_UAR", "Davis Quartz",Scanner.Areas.AREA_DAVIS_QUARTZ_01.Value),
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


