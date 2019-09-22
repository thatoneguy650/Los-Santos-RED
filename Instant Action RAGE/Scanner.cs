using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Scanner
{
    private readonly static Random random = new Random();
    private Scanner(string value) { Value = value; }
    public string Value { get; set; }
    public class Areas
    {
        public static Scanner AREA_ALTA_01 { get { return new Scanner("AREAS\\AREA_ALTA_01"); } }
        public static Scanner AREA_BACKLOT_CITY_01 { get { return new Scanner("AREAS\\AREA_BACKLOT_CITY_01"); } }
        public static Scanner AREA_BANHAM_CANYON_01 { get { return new Scanner("AREAS\\AREA_BANHAM_CANYON_01"); } }
        public static Scanner AREA_BANNING_01 { get { return new Scanner("AREAS\\AREA_BANNING_01"); } }
        public static Scanner AREA_BAYTREE_CANYON_01 { get { return new Scanner("AREAS\\AREA_BAYTREE_CANYON_01"); } }
        public static Scanner AREA_BOLLINGBROKE_PENITENTIARY_01 { get { return new Scanner("AREAS\\AREA_BOLLINGBROKE_PENITENTIARY_01"); } }
        public static Scanner AREA_BRADDOCK_PASS_01 { get { return new Scanner("AREAS\\AREA_BRADDOCK_PASS_01"); } }
        public static Scanner AREA_BURTON_01 { get { return new Scanner("AREAS\\AREA_BURTON_01"); } }
        public static Scanner AREA_CAPE_CATFISH_01 { get { return new Scanner("AREAS\\AREA_CAPE_CATFISH_01"); } }
        public static Scanner AREA_CASSIDY_CREEK_01 { get { return new Scanner("AREAS\\AREA_CASSIDY_CREEK_01"); } }
        public static Scanner AREA_CHAMBERLAIN_HILLS_01 { get { return new Scanner("AREAS\\AREA_CHAMBERLAIN_HILLS_01"); } }
        public static Scanner AREA_CHILLIAD_MOUNTAIN_STATE_WILDERNESS_01 { get { return new Scanner("AREAS\\AREA_CHILLIAD_MOUNTAIN_STATE_WILDERNESS_01"); } }
        public static Scanner AREA_CHUMASH_01 { get { return new Scanner("AREAS\\AREA_CHUMASH_01"); } }
        public static Scanner AREA_CHUMASH_02 { get { return new Scanner("AREAS\\AREA_CHUMASH_02"); } }
        public static Scanner AREA_CYPRESS_FLATS_01 { get { return new Scanner("AREAS\\AREA_CYPRESS_FLATS_01"); } }
        public static Scanner AREA_DAVIS_01 { get { return new Scanner("AREAS\\AREA_DAVIS_01"); } }
        public static Scanner AREA_DAVIS_QUARTZ_01 { get { return new Scanner("AREAS\\AREA_DAVIS_QUARTZ_01"); } }
        public static Scanner AREA_DEL_PERRO_01 { get { return new Scanner("AREAS\\AREA_DEL_PERRO_01"); } }
        public static Scanner AREA_DEL_PERRO_02 { get { return new Scanner("AREAS\\AREA_DEL_PERRO_02"); } }
        public static Scanner AREA_DEL_PERRO_BEACH_01 { get { return new Scanner("AREAS\\AREA_DEL_PERRO_BEACH_01"); } }
        public static Scanner AREA_DOWNTOWN_01 { get { return new Scanner("AREAS\\AREA_DOWNTOWN_01"); } }
        public static Scanner AREA_DOWNTOWN_02 { get { return new Scanner("AREAS\\AREA_DOWNTOWN_02"); } }
        public static Scanner AREA_DOWNTOWN_VINEWOOD_01 { get { return new Scanner("AREAS\\AREA_DOWNTOWN_VINEWOOD_01"); } }
        public static Scanner AREA_EAST_LOS_SANTOS_01 { get { return new Scanner("AREAS\\AREA_EAST_LOS_SANTOS_01"); } }
        public static Scanner AREA_EAST_LOS_SANTOS_02 { get { return new Scanner("AREAS\\AREA_EAST_LOS_SANTOS_02"); } }
        public static Scanner AREA_EAST_VINEWOOD_01 { get { return new Scanner("AREAS\\AREA_EAST_VINEWOOD_01"); } }
        public static Scanner AREA_ELYSIAN_ISLAND_01 { get { return new Scanner("AREAS\\AREA_ELYSIAN_ISLAND_01"); } }
        public static Scanner AREA_EL_BURRO_HEIGHTS_01 { get { return new Scanner("AREAS\\AREA_EL_BURRO_HEIGHTS_01"); } }
        public static Scanner AREA_FORT_ZANCUDO_01 { get { return new Scanner("AREAS\\AREA_FORT_ZANCUDO_01"); } }
        public static Scanner AREA_GALILEE_01 { get { return new Scanner("AREAS\\AREA_GALILEE_01"); } }
        public static Scanner AREA_GALILEO_OBSERVATORY_01 { get { return new Scanner("AREAS\\AREA_GALILEO_OBSERVATORY_01"); } }
        public static Scanner AREA_GALILEO_PARK_01 { get { return new Scanner("AREAS\\AREA_GALILEO_PARK_01"); } }
        public static Scanner AREA_GRANDE_SENORA_DESERT_01 { get { return new Scanner("AREAS\\AREA_GRANDE_SENORA_DESERT_01"); } }
        public static Scanner AREA_GRAPESEED_01 { get { return new Scanner("AREAS\\AREA_GRAPESEED_01"); } }
        public static Scanner AREA_GREAT_CHAPARRAL_01 { get { return new Scanner("AREAS\\AREA_GREAT_CHAPARRAL_01"); } }
        public static Scanner AREA_GWC_GOLF_CLUB_01 { get { return new Scanner("AREAS\\AREA_GWC_GOLF_CLUB_01"); } }
        public static Scanner AREA_HARMONY_01 { get { return new Scanner("AREAS\\AREA_HARMONY_01"); } }
        public static Scanner AREA_HAWICK_01 { get { return new Scanner("AREAS\\AREA_HAWICK_01"); } }
        public static Scanner AREA_HUMANE_LABS { get { return new Scanner("AREAS\\AREA_HUMANE_LABS"); } }
        public static Scanner AREA_LAGO_ZANCUDO_01 { get { return new Scanner("AREAS\\AREA_LAGO_ZANCUDO_01"); } }
        public static Scanner AREA_LAND_ACT_DAM_01 { get { return new Scanner("AREAS\\AREA_LAND_ACT_DAM_01"); } }
        public static Scanner AREA_LAND_ACT_RESERVOIR_01 { get { return new Scanner("AREAS\\AREA_LAND_ACT_RESERVOIR_01"); } }
        public static Scanner AREA_LA_MESA_01 { get { return new Scanner("AREAS\\AREA_LA_MESA_01"); } }
        public static Scanner AREA_LA_PUERTA_01 { get { return new Scanner("AREAS\\AREA_LA_PUERTA_01"); } }
        public static Scanner AREA_LA_PUERTA_FREEWAY_01 { get { return new Scanner("AREAS\\AREA_LA_PUERTA_FREEWAY_01"); } }
        public static Scanner AREA_LITTLE_SEOUL_01 { get { return new Scanner("AREAS\\AREA_LITTLE_SEOUL_01"); } }
        public static Scanner AREA_LOS_SANTOS_FREEWAY_01 { get { return new Scanner("AREAS\\AREA_LOS_SANTOS_FREEWAY_01"); } }
        public static Scanner AREA_LOS_SANTOS_INTERNATIONAL_01 { get { return new Scanner("AREAS\\AREA_LOS_SANTOS_INTERNATIONAL_01"); } }
        public static Scanner AREA_LOS_SANTOS_INTERNATIONAL_02 { get { return new Scanner("AREAS\\AREA_LOS_SANTOS_INTERNATIONAL_02"); } }
        public static Scanner AREA_MAZE_BANK_ARENA_01 { get { return new Scanner("AREAS\\AREA_MAZE_BANK_ARENA_01"); } }
        public static Scanner AREA_MAZE_BANK_ARENA_02 { get { return new Scanner("AREAS\\AREA_MAZE_BANK_ARENA_02"); } }
        public static Scanner AREA_MIRROR_PARK_01 { get { return new Scanner("AREAS\\AREA_MIRROR_PARK_01"); } }
        public static Scanner AREA_MISSION_ROW_01 { get { return new Scanner("AREAS\\AREA_MISSION_ROW_01"); } }
        public static Scanner AREA_MORNINGWOOD_01 { get { return new Scanner("AREAS\\AREA_MORNINGWOOD_01"); } }
        public static Scanner AREA_MOUNT_CHILLIAD_01 { get { return new Scanner("AREAS\\AREA_MOUNT_CHILLIAD_01"); } }
        public static Scanner AREA_MOUNT_GORDO_01 { get { return new Scanner("AREAS\\AREA_MOUNT_GORDO_01"); } }
        public static Scanner AREA_MOUNT_JOSIAH_01 { get { return new Scanner("AREAS\\AREA_MOUNT_JOSIAH_01"); } }
        public static Scanner AREA_MURRIETA_HEIGHTS_01 { get { return new Scanner("AREAS\\AREA_MURRIETA_HEIGHTS_01"); } }
        public static Scanner AREA_NOOSE_HQ_01 { get { return new Scanner("AREAS\\AREA_NOOSE_HQ_01"); } }
        public static Scanner AREA_NORTH_CHUMASH_01 { get { return new Scanner("AREAS\\AREA_NORTH_CHUMASH_01"); } }
        public static Scanner AREA_NORTH_YANKTON_01 { get { return new Scanner("AREAS\\AREA_NORTH_YANKTON_01"); } }
        public static Scanner AREA_PACIFIC_BLUFFS_01 { get { return new Scanner("AREAS\\AREA_PACIFIC_BLUFFS_01"); } }
        public static Scanner AREA_PACIFIC_OCEAN_01 { get { return new Scanner("AREAS\\AREA_PACIFIC_OCEAN_01"); } }
        public static Scanner AREA_PALETO_BAY_01 { get { return new Scanner("AREAS\\AREA_PALETO_BAY_01"); } }
        public static Scanner AREA_PALETO_COVE_01 { get { return new Scanner("AREAS\\AREA_PALETO_COVE_01"); } }
        public static Scanner AREA_PALETO_FOREST_01 { get { return new Scanner("AREAS\\AREA_PALETO_FOREST_01"); } }
        public static Scanner AREA_PALMER_TAYLOR_POWER_STATION_01 { get { return new Scanner("AREAS\\AREA_PALMER_TAYLOR_POWER_STATION_01"); } }
        public static Scanner AREA_PALOMINO_HIGHLANDS_01 { get { return new Scanner("AREAS\\AREA_PALOMINO_HIGHLANDS_01"); } }
        public static Scanner AREA_PILLBOX_HILL_01 { get { return new Scanner("AREAS\\AREA_PILLBOX_HILL_01"); } }
        public static Scanner AREA_PORCOPIO_BEACH_01 { get { return new Scanner("AREAS\\AREA_PORCOPIO_BEACH_01"); } }
        public static Scanner AREA_PORCOPIO_TRUCK_STOP_01 { get { return new Scanner("AREAS\\AREA_PORCOPIO_TRUCK_STOP_01"); } }
        public static Scanner AREA_PORT_OF_SOUTH_LOS_SANTOS_01 { get { return new Scanner("AREAS\\AREA_PORT_OF_SOUTH_LOS_SANTOS_01"); } }
        public static Scanner AREA_PROCOPIO_BEACH_01 { get { return new Scanner("AREAS\\AREA_PROCOPIO_BEACH_01"); } }
        public static Scanner AREA_PUERTO_DEL_SOL_01 { get { return new Scanner("AREAS\\AREA_PUERTO_DEL_SOL_01"); } }
        public static Scanner AREA_RANCHO_01 { get { return new Scanner("AREAS\\AREA_RANCHO_01"); } }
        public static Scanner AREA_RATON_CANYON_01 { get { return new Scanner("AREAS\\AREA_RATON_CANYON_01"); } }
        public static Scanner AREA_RICHMAN_01 { get { return new Scanner("AREAS\\AREA_RICHMAN_01"); } }
        public static Scanner AREA_RICHMAN_GLEN_01 { get { return new Scanner("AREAS\\AREA_RICHMAN_GLEN_01"); } }
        public static Scanner AREA_ROCKFORD_HILLS_01 { get { return new Scanner("AREAS\\AREA_ROCKFORD_HILLS_01"); } }
        public static Scanner AREA_ROCKFORD_HILLS_02 { get { return new Scanner("AREAS\\AREA_ROCKFORD_HILLS_02"); } }
        public static Scanner AREA_RON_ALTERNATES_WINDFARM_01 { get { return new Scanner("AREAS\\AREA_RON_ALTERNATES_WINDFARM_01"); } }
        public static Scanner AREA_SANDY_SHORES_01 { get { return new Scanner("AREAS\\AREA_SANDY_SHORES_01"); } }
        public static Scanner AREA_SANDY_SHORES_02 { get { return new Scanner("AREAS\\AREA_SANDY_SHORES_02"); } }
        public static Scanner AREA_SAN_ANDREAS_01 { get { return new Scanner("AREAS\\AREA_SAN_ANDREAS_01"); } }
        public static Scanner AREA_SAN_CHIANSKI_MOUNTAINS_01 { get { return new Scanner("AREAS\\AREA_SAN_CHIANSKI_MOUNTAINS_01"); } }
        public static Scanner AREA_SENORA_WINDFARM_01 { get { return new Scanner("AREAS\\AREA_SENORA_WINDFARM_01"); } }
        public static Scanner AREA_SOUTH_LOS_SANTOS_01 { get { return new Scanner("AREAS\\AREA_SOUTH_LOS_SANTOS_01"); } }
        public static Scanner AREA_SOUTH_LOS_SANTOS_02 { get { return new Scanner("AREAS\\AREA_SOUTH_LOS_SANTOS_02"); } }
        public static Scanner AREA_STAB_CITY_01 { get { return new Scanner("AREAS\\AREA_STAB_CITY_01"); } }
        public static Scanner AREA_STRAWBERRY_01 { get { return new Scanner("AREAS\\AREA_STRAWBERRY_01"); } }
        public static Scanner AREA_TATAVIAM_MOUNTAINS_01 { get { return new Scanner("AREAS\\AREA_TATAVIAM_MOUNTAINS_01"); } }
        public static Scanner AREA_TERMINAL_01 { get { return new Scanner("AREAS\\AREA_TERMINAL_01"); } }
        public static Scanner AREA_TEXTILE_CITY_01 { get { return new Scanner("AREAS\\AREA_TEXTILE_CITY_01"); } }
        public static Scanner AREA_THE_ALAMO_SEA_01 { get { return new Scanner("AREAS\\AREA_THE_ALAMO_SEA_01"); } }
        public static Scanner AREA_THE_BRADDOCK_TUNNEL_01 { get { return new Scanner("AREAS\\AREA_THE_BRADDOCK_TUNNEL_01"); } }
        public static Scanner AREA_THE_CALAFIA_BRIDGE_01 { get { return new Scanner("AREAS\\AREA_THE_CALAFIA_BRIDGE_01"); } }
        public static Scanner AREA_THE_CHILLIAD_MOUNTAIN_STATE_WILDERNESS_01 { get { return new Scanner("AREAS\\AREA_THE_CHILLIAD_MOUNTAIN_STATE_WILDERNESS_01"); } }
        public static Scanner AREA_THE_REDWOOD_LIGHTS_TRACK_01 { get { return new Scanner("AREAS\\AREA_THE_REDWOOD_LIGHTS_TRACK_01"); } }
        public static Scanner AREA_THE_STADIUM_01 { get { return new Scanner("AREAS\\AREA_THE_STADIUM_01"); } }
        public static Scanner AREA_TONGVA_HILLS_01 { get { return new Scanner("AREAS\\AREA_TONGVA_HILLS_01"); } }
        public static Scanner AREA_TONGVA_VALLEY_01 { get { return new Scanner("AREAS\\AREA_TONGVA_VALLEY_01"); } }
        public static Scanner AREA_UTOPIA_GARDENS_01 { get { return new Scanner("AREAS\\AREA_UTOPIA_GARDENS_01"); } }
        public static Scanner AREA_VESPUCCI_01 { get { return new Scanner("AREAS\\AREA_VESPUCCI_01"); } }
        public static Scanner AREA_VESPUCCI_BEACH_01 { get { return new Scanner("AREAS\\AREA_VESPUCCI_BEACH_01"); } }
        public static Scanner AREA_VESPUCCI_CANALS_01 { get { return new Scanner("AREAS\\AREA_VESPUCCI_CANALS_01"); } }
        public static Scanner AREA_VINEWOOD_01 { get { return new Scanner("AREAS\\AREA_VINEWOOD_01"); } }
        public static Scanner AREA_VINEWOOD_HILLS_01 { get { return new Scanner("AREAS\\AREA_VINEWOOD_HILLS_01"); } }
        public static Scanner AREA_VINEWOOD_RACETRACK_01 { get { return new Scanner("AREAS\\AREA_VINEWOOD_RACETRACK_01"); } }
        public static Scanner AREA_WEST_VINEWOOD_01 { get { return new Scanner("AREAS\\AREA_WEST_VINEWOOD_01"); } }
        public static Scanner AREA_ZANCUDO_RIVER_01 { get { return new Scanner("AREAS\\AREA_ZANCUDO_RIVER_01"); } }
    }
    public class AssistanceRequired
    {
        public static String AssistanceRequiredRandom()
        {
            int rnd = random.Next(1, 4);
            if (rnd == 1)
                return Scanner.AssistanceRequired.ASSISTANCE_REQUIRED_01.Value;
            else if (rnd == 2)
                return Scanner.AssistanceRequired.ASSISTANCE_REQUIRED_02.Value;
            else if (rnd == 3)
                return Scanner.AssistanceRequired.ASSISTANCE_REQUIRED_03.Value;
            else
                return Scanner.AssistanceRequired.ASSISTANCE_REQUIRED_04.Value;
        }
        public static Scanner ASSISTANCE_REQUIRED_01 { get { return new Scanner("ASSISTANCE_REQUIRED\\ASSISTANCE_REQUIRED_01"); } }
        public static Scanner ASSISTANCE_REQUIRED_02 { get { return new Scanner("ASSISTANCE_REQUIRED\\ASSISTANCE_REQUIRED_02"); } }
        public static Scanner ASSISTANCE_REQUIRED_03 { get { return new Scanner("ASSISTANCE_REQUIRED\\ASSISTANCE_REQUIRED_03"); } }
        public static Scanner ASSISTANCE_REQUIRED_04 { get { return new Scanner("ASSISTANCE_REQUIRED\\ASSISTANCE_REQUIRED_04"); } }
    }
    public class AttemptToFind
    {
        public static Scanner ALL_UNITS_ATTEMPT_TO_REAQUIRE_01 { get { return new Scanner("ATTEMPT_TO_FIND\\ALL_UNITS_ATTEMPT_TO_REAQUIRE_01"); } }
        public static Scanner REMAIN_IN_THE_AREA_01 { get { return new Scanner("ATTEMPT_TO_FIND\\REMAIN_IN_THE_AREA_01"); } }
        public static Scanner REMAIN_IN_THE_AREA_2 { get { return new Scanner("ATTEMPT_TO_FIND\\REMAIN_IN_THE_AREA_2"); } }
    }
    public class AttentionAllUnits
    {
        public static String AttentionAllUnitsRandom()
        {
            int rnd = random.Next(1, 5);
            if (rnd == 1)
                return Scanner.AttentionAllUnits.ATTENTION_ALL_UNITS_01.Value;
            else if (rnd == 2)
                return Scanner.AttentionAllUnits.ATTENTION_ALL_UNITS_02.Value;
            else if (rnd == 3)
                return Scanner.AttentionAllUnits.ATTENTION_ALL_UNITS_03.Value;
            else if (rnd == 4)
                return Scanner.AttentionAllUnits.ATTENTION_ALL_UNITS_04.Value;
            else
                return Scanner.AttentionAllUnits.ATTENTION_ALL_UNITS_05.Value;
        }
        public static Scanner ATTENTION_ALL_SWAT_UNITS_01 { get { return new Scanner("ATTENTION_ALL_UNITS\\ATTENTION_ALL_SWAT_UNITS_01"); } }
        public static Scanner ATTENTION_ALL_SWAT_UNITS_02 { get { return new Scanner("ATTENTION_ALL_UNITS\\ATTENTION_ALL_SWAT_UNITS_02"); } }
        public static Scanner ATTENTION_ALL_UNITS_01 { get { return new Scanner("ATTENTION_ALL_UNITS\\ATTENTION_ALL_UNITS_01"); } }
        public static Scanner ATTENTION_ALL_UNITS_02 { get { return new Scanner("ATTENTION_ALL_UNITS\\ATTENTION_ALL_UNITS_02"); } }
        public static Scanner ATTENTION_ALL_UNITS_03 { get { return new Scanner("ATTENTION_ALL_UNITS\\ATTENTION_ALL_UNITS_03"); } }
        public static Scanner ATTENTION_ALL_UNITS_04 { get { return new Scanner("ATTENTION_ALL_UNITS\\ATTENTION_ALL_UNITS_04"); } }
        public static Scanner ATTENTION_ALL_UNITS_05 { get { return new Scanner("ATTENTION_ALL_UNITS\\ATTENTION_ALL_UNITS_05"); } }
        public static Scanner DISPATCH_SWAT_UNITS_FROM_01 { get { return new Scanner("ATTENTION_ALL_UNITS\\DISPATCH_SWAT_UNITS_FROM_01"); } }
    }
    public class CarModel
    {
        public static Scanner CM_0x0022E61C { get { return new Scanner("CAR_MODEL\\0x0022E61C"); } }
        public static Scanner CM_0x00F1C772 { get { return new Scanner("CAR_MODEL\\0x00F1C772"); } }
        public static Scanner CM_0x01A34F3C { get { return new Scanner("CAR_MODEL\\0x01A34F3C"); } }
        public static Scanner CM_0x02DEE01C { get { return new Scanner("CAR_MODEL\\0x02DEE01C"); } }
        public static Scanner CM_0x04A8B8D8 { get { return new Scanner("CAR_MODEL\\0x04A8B8D8"); } }
        public static Scanner CM_0x0530481E { get { return new Scanner("CAR_MODEL\\0x0530481E"); } }
        public static Scanner CM_0x065EFFE4 { get { return new Scanner("CAR_MODEL\\0x065EFFE4"); } }
        public static Scanner CM_0x071B9975 { get { return new Scanner("CAR_MODEL\\0x071B9975"); } }
        public static Scanner CM_0x07F22777 { get { return new Scanner("CAR_MODEL\\0x07F22777"); } }
        public static Scanner CM_0x07F4BF21 { get { return new Scanner("CAR_MODEL\\0x07F4BF21"); } }
        public static Scanner CM_0x0A040051 { get { return new Scanner("CAR_MODEL\\0x0A040051"); } }
        public static Scanner CM_0x0CBF172E { get { return new Scanner("CAR_MODEL\\0x0CBF172E"); } }
        public static Scanner CM_0x0D2D849F { get { return new Scanner("CAR_MODEL\\0x0D2D849F"); } }
        public static Scanner CM_0x0D5AB8CA { get { return new Scanner("CAR_MODEL\\0x0D5AB8CA"); } }
        public static Scanner CM_0x103EFC8A { get { return new Scanner("CAR_MODEL\\0x103EFC8A"); } }
        public static Scanner CM_0x10669BD6 { get { return new Scanner("CAR_MODEL\\0x10669BD6"); } }
        public static Scanner CM_0x13187F4D { get { return new Scanner("CAR_MODEL\\0x13187F4D"); } }
        public static Scanner CM_0x134427FD { get { return new Scanner("CAR_MODEL\\0x134427FD"); } }
        public static Scanner CM_0x15459108 { get { return new Scanner("CAR_MODEL\\0x15459108"); } }
        public static Scanner CM_0x16DA499A { get { return new Scanner("CAR_MODEL\\0x16DA499A"); } }
        public static Scanner CM_0x18E71631 { get { return new Scanner("CAR_MODEL\\0x18E71631"); } }
        public static Scanner CM_0x19605066 { get { return new Scanner("CAR_MODEL\\0x19605066"); } }
        public static Scanner CM_0x1A9DDC82 { get { return new Scanner("CAR_MODEL\\0x1A9DDC82"); } }
        public static Scanner CM_0x1B11C93F { get { return new Scanner("CAR_MODEL\\0x1B11C93F"); } }
        public static Scanner CM_0x1BA79845 { get { return new Scanner("CAR_MODEL\\0x1BA79845"); } }
        public static Scanner CM_0x1CB7C601 { get { return new Scanner("CAR_MODEL\\0x1CB7C601"); } }
        public static Scanner CM_0x1D457D44 { get { return new Scanner("CAR_MODEL\\0x1D457D44"); } }
        public static Scanner CM_0x1E3E998F { get { return new Scanner("CAR_MODEL\\0x1E3E998F"); } }
        public static Scanner CM_0x1FCD4216 { get { return new Scanner("CAR_MODEL\\0x1FCD4216"); } }
        public static Scanner CM_ADDER_01 { get { return new Scanner("CAR_MODEL\\ADDER_01"); } }
        public static Scanner CM_AIRSHIP_01 { get { return new Scanner("CAR_MODEL\\AIRSHIP_01"); } }
        public static Scanner CM_AIRTUG_01 { get { return new Scanner("CAR_MODEL\\AIRTUG_01"); } }
        public static Scanner CM_AMBULANCE_01 { get { return new Scanner("CAR_MODEL\\AMBULANCE_01"); } }
        public static Scanner CM_ANNIHILATOR_01 { get { return new Scanner("CAR_MODEL\\ANNIHILATOR_01"); } }
        public static Scanner CM_ASEA_01 { get { return new Scanner("CAR_MODEL\\ASEA_01"); } }
        public static Scanner CM_BAGGER_01 { get { return new Scanner("CAR_MODEL\\BAGGER_01"); } }
        public static Scanner CM_BALLER_01 { get { return new Scanner("CAR_MODEL\\BALLER_01"); } }
        public static Scanner CM_BANSHEE_01 { get { return new Scanner("CAR_MODEL\\BANSHEE_01"); } }
        public static Scanner CM_BARRACKS_01 { get { return new Scanner("CAR_MODEL\\BARRACKS_01"); } }
        public static Scanner CM_BATI_01 { get { return new Scanner("CAR_MODEL\\BATI_01"); } }
        public static Scanner CM_BENSON_01 { get { return new Scanner("CAR_MODEL\\BENSON_01"); } }
        public static Scanner CM_BF_INJECTION_01 { get { return new Scanner("CAR_MODEL\\BF_INJECTION_01"); } }
        public static Scanner CM_BIFF_01 { get { return new Scanner("CAR_MODEL\\BIFF_01"); } }
        public static Scanner CM_BISON_01 { get { return new Scanner("CAR_MODEL\\BISON_01"); } }
        public static Scanner CM_BJXL_01 { get { return new Scanner("CAR_MODEL\\BJXL_01"); } }
        public static Scanner CM_BLAZER_01 { get { return new Scanner("CAR_MODEL\\BLAZER_01"); } }
        public static Scanner CM_BLIMP_01 { get { return new Scanner("CAR_MODEL\\BLIMP_01"); } }
        public static Scanner CM_BLISTA_01 { get { return new Scanner("CAR_MODEL\\BLISTA_01"); } }
        public static Scanner CM_BMX_01 { get { return new Scanner("CAR_MODEL\\BMX_01"); } }
        public static Scanner CM_BOBCAT_01 { get { return new Scanner("CAR_MODEL\\BOBCAT_01"); } }
        public static Scanner CM_BOBCAT_XL_01 { get { return new Scanner("CAR_MODEL\\BOBCAT_XL_01"); } }
        public static Scanner CM_BOXVILLE_01 { get { return new Scanner("CAR_MODEL\\BOXVILLE_01"); } }
        public static Scanner CM_BUFFALO_01 { get { return new Scanner("CAR_MODEL\\BUFFALO_01"); } }
        public static Scanner CM_BULLDOZER_01 { get { return new Scanner("CAR_MODEL\\BULLDOZER_01"); } }
        public static Scanner CM_BULLET_01 { get { return new Scanner("CAR_MODEL\\BULLET_01"); } }
        public static Scanner CM_BULLET_GT_01 { get { return new Scanner("CAR_MODEL\\BULLET_GT_01"); } }
        public static Scanner CM_BURRITO_01 { get { return new Scanner("CAR_MODEL\\BURRITO_01"); } }
        public static Scanner CM_BUS_01 { get { return new Scanner("CAR_MODEL\\BUS_01"); } }
        public static Scanner CM_BUZZARD_01 { get { return new Scanner("CAR_MODEL\\BUZZARD_01"); } }
        public static Scanner CM_CADDY_01 { get { return new Scanner("CAR_MODEL\\CADDY_01"); } }
        public static Scanner CM_CAMPER_01 { get { return new Scanner("CAR_MODEL\\CAMPER_01"); } }
        public static Scanner CM_CARBON_RS_01 { get { return new Scanner("CAR_MODEL\\CARBON_RS_01"); } }
        public static Scanner CM_CARGOBOB_01 { get { return new Scanner("CAR_MODEL\\CARGOBOB_01"); } }
        public static Scanner CM_CARGO_PLANE_01 { get { return new Scanner("CAR_MODEL\\CARGO_PLANE_01"); } }
        public static Scanner CM_CAVALCADE_01 { get { return new Scanner("CAR_MODEL\\CAVALCADE_01"); } }
        public static Scanner CM_CEMENT_MIXER_01 { get { return new Scanner("CAR_MODEL\\CEMENT_MIXER_01"); } }
        public static Scanner CM_CHEETAH_01 { get { return new Scanner("CAR_MODEL\\CHEETAH_01"); } }
        public static Scanner CM_COACH_01 { get { return new Scanner("CAR_MODEL\\COACH_01"); } }
        public static Scanner CM_COGNOSCENTI_01 { get { return new Scanner("CAR_MODEL\\COGNOSCENTI_01"); } }
        public static Scanner CM_COG_55_01 { get { return new Scanner("CAR_MODEL\\COG_55_01"); } }
        public static Scanner CM_COMET_01 { get { return new Scanner("CAR_MODEL\\COMET_01"); } }
        public static Scanner CM_COQUETTE_01 { get { return new Scanner("CAR_MODEL\\COQUETTE_01"); } }
        public static Scanner CM_CRUISER_01 { get { return new Scanner("CAR_MODEL\\CRUISER_01"); } }
        public static Scanner CM_CRUSADER_01 { get { return new Scanner("CAR_MODEL\\CRUSADER_01"); } }
        public static Scanner CM_CUBAN_800_01 { get { return new Scanner("CAR_MODEL\\CUBAN_800_01"); } }
        public static Scanner CM_CUTTER_01 { get { return new Scanner("CAR_MODEL\\CUTTER_01"); } }
        public static Scanner CM_DAEMON_01 { get { return new Scanner("CAR_MODEL\\DAEMON_01"); } }
        public static Scanner CM_DIGGER_01 { get { return new Scanner("CAR_MODEL\\DIGGER_01"); } }
        public static Scanner CM_DILETTANTE_01 { get { return new Scanner("CAR_MODEL\\DILETTANTE_01"); } }
        public static Scanner CM_DINGHY_01 { get { return new Scanner("CAR_MODEL\\DINGHY_01"); } }
        public static Scanner CM_DOCK_TUG_01 { get { return new Scanner("CAR_MODEL\\DOCK_TUG_01"); } }
        public static Scanner CM_DOMINATOR_01 { get { return new Scanner("CAR_MODEL\\DOMINATOR_01"); } }
        public static Scanner CM_DOUBLE_01 { get { return new Scanner("CAR_MODEL\\DOUBLE_01"); } }
        public static Scanner CM_DOUBLE_T_01 { get { return new Scanner("CAR_MODEL\\DOUBLE_T_01"); } }
        public static Scanner CM_DUBSTA_01 { get { return new Scanner("CAR_MODEL\\DUBSTA_01"); } }
        public static Scanner CM_DUKES_01 { get { return new Scanner("CAR_MODEL\\DUKES_01"); } }
        public static Scanner CM_DUMPER_01 { get { return new Scanner("CAR_MODEL\\DUMPER_01"); } }
        public static Scanner CM_DUMP_01 { get { return new Scanner("CAR_MODEL\\DUMP_01"); } }
        public static Scanner CM_DUNELOADER_01 { get { return new Scanner("CAR_MODEL\\DUNELOADER_01"); } }
        public static Scanner CM_DUNE_01 { get { return new Scanner("CAR_MODEL\\DUNE_01"); } }
        public static Scanner CM_DUNE_BUGGY_01 { get { return new Scanner("CAR_MODEL\\DUNE_BUGGY_01"); } }
        public static Scanner CM_DUSTER_01 { get { return new Scanner("CAR_MODEL\\DUSTER_01"); } }
        public static Scanner CM_ELEGY_01 { get { return new Scanner("CAR_MODEL\\ELEGY_01"); } }
        public static Scanner CM_EMPEROR_01 { get { return new Scanner("CAR_MODEL\\EMPEROR_01"); } }
        public static Scanner CM_EXEMPLAR_01 { get { return new Scanner("CAR_MODEL\\EXEMPLAR_01"); } }
        public static Scanner CM_F620_01 { get { return new Scanner("CAR_MODEL\\F620_01"); } }
        public static Scanner CM_FACTION_01 { get { return new Scanner("CAR_MODEL\\FACTION_01"); } }
        public static Scanner CM_FAGGIO_01 { get { return new Scanner("CAR_MODEL\\FAGGIO_01"); } }
        public static Scanner CM_FELON_01 { get { return new Scanner("CAR_MODEL\\FELON_01"); } }
        public static Scanner CM_FELTZER_01 { get { return new Scanner("CAR_MODEL\\FELTZER_01"); } }
        public static Scanner CM_FEROCI_01 { get { return new Scanner("CAR_MODEL\\FEROCI_01"); } }
        public static Scanner CM_FIELDMASTER_01 { get { return new Scanner("CAR_MODEL\\FIELDMASTER_01"); } }
        public static Scanner CM_FIRETRUCK_01 { get { return new Scanner("CAR_MODEL\\FIRETRUCK_01"); } }
        public static Scanner CM_FLATBED_01 { get { return new Scanner("CAR_MODEL\\FLATBED_01"); } }
        public static Scanner CM_FORKLIFT_01 { get { return new Scanner("CAR_MODEL\\FORKLIFT_01"); } }
        public static Scanner CM_FQ2_01 { get { return new Scanner("CAR_MODEL\\FQ2_01"); } }
        public static Scanner CM_FREIGHT_01 { get { return new Scanner("CAR_MODEL\\FREIGHT_01"); } }
        public static Scanner CM_FROGGER_01 { get { return new Scanner("CAR_MODEL\\FROGGER_01"); } }
        public static Scanner CM_FUGITIVE_01 { get { return new Scanner("CAR_MODEL\\FUGITIVE_01"); } }
        public static Scanner CM_GAUNTLET_01 { get { return new Scanner("CAR_MODEL\\GAUNTLET_01"); } }
        public static Scanner CM_GRANGER_01 { get { return new Scanner("CAR_MODEL\\GRANGER_01"); } }
        public static Scanner CM_HANDLER_01 { get { return new Scanner("CAR_MODEL\\HANDLER_01"); } }
        public static Scanner CM_HAULER_01 { get { return new Scanner("CAR_MODEL\\HAULER_01"); } }
        public static Scanner CM_HEARSE_01 { get { return new Scanner("CAR_MODEL\\HEARSE_01"); } }
        public static Scanner CM_HELLFURY_01 { get { return new Scanner("CAR_MODEL\\HELLFURY_01"); } }
        public static Scanner CM_HEXER_01 { get { return new Scanner("CAR_MODEL\\HEXER_01"); } }
        public static Scanner CM_HOT_KNIFE_01 { get { return new Scanner("CAR_MODEL\\HOT_KNIFE_01"); } }
        public static Scanner CM_HUNTER_01 { get { return new Scanner("CAR_MODEL\\HUNTER_01"); } }
        public static Scanner CM_INFERNUS_01 { get { return new Scanner("CAR_MODEL\\INFERNUS_01"); } }
        public static Scanner CM_INGOT_01 { get { return new Scanner("CAR_MODEL\\INGOT_01"); } }
        public static Scanner CM_INTRUDER_01 { get { return new Scanner("CAR_MODEL\\INTRUDER_01"); } }
        public static Scanner CM_JACKAL_01 { get { return new Scanner("CAR_MODEL\\JACKAL_01"); } }
        public static Scanner CM_JB700_01 { get { return new Scanner("CAR_MODEL\\JB700_01"); } }
        public static Scanner CM_JETMAX_01 { get { return new Scanner("CAR_MODEL\\JETMAX_01"); } }
        public static Scanner CM_JET_01 { get { return new Scanner("CAR_MODEL\\JET_01"); } }
        public static Scanner CM_JOURNEY_01 { get { return new Scanner("CAR_MODEL\\JOURNEY_01"); } }
        public static Scanner CM_KHAMELION_01 { get { return new Scanner("CAR_MODEL\\KHAMELION_01"); } }
        public static Scanner CM_LANDSTALKER_01 { get { return new Scanner("CAR_MODEL\\LANDSTALKER_01"); } }
        public static Scanner CM_LAZER_01 { get { return new Scanner("CAR_MODEL\\LAZER_01"); } }
        public static Scanner CM_LIFEGUARD_GRANGER_01 { get { return new Scanner("CAR_MODEL\\LIFEGUARD_GRANGER_01"); } }
        public static Scanner CM_MANANA_01 { get { return new Scanner("CAR_MODEL\\MANANA_01"); } }
        public static Scanner CM_MAVERICK_01 { get { return new Scanner("CAR_MODEL\\MAVERICK_01"); } }
        public static Scanner CM_MESA_01 { get { return new Scanner("CAR_MODEL\\MESA_01"); } }
        public static Scanner CM_METROTRAIN_01 { get { return new Scanner("CAR_MODEL\\METROTRAIN_01"); } }
        public static Scanner CM_MINIVAN_01 { get { return new Scanner("CAR_MODEL\\MINIVAN_01"); } }
        public static Scanner CM_MIXER_01 { get { return new Scanner("CAR_MODEL\\MIXER_01"); } }
        public static Scanner CM_MOWER_01 { get { return new Scanner("CAR_MODEL\\MOWER_01"); } }
        public static Scanner CM_MULE_01 { get { return new Scanner("CAR_MODEL\\MULE_01"); } }
        public static Scanner CM_NEMESIS_01 { get { return new Scanner("CAR_MODEL\\NEMESIS_01"); } }
        public static Scanner CM_NINE_F_01 { get { return new Scanner("CAR_MODEL\\NINE_F_01"); } }
        public static Scanner CM_ORACLE_01 { get { return new Scanner("CAR_MODEL\\ORACLE_01"); } }
        public static Scanner CM_PACKER_01 { get { return new Scanner("CAR_MODEL\\PACKER_01"); } }
        public static Scanner CM_PATRIOT_01 { get { return new Scanner("CAR_MODEL\\PATRIOT_01"); } }
        public static Scanner CM_PCJ_600_01 { get { return new Scanner("CAR_MODEL\\PCJ_600_01"); } }
        public static Scanner CM_PENUMBRA_01 { get { return new Scanner("CAR_MODEL\\PENUMBRA_01"); } }
        public static Scanner CM_PEYOTE_01 { get { return new Scanner("CAR_MODEL\\PEYOTE_01"); } }
        public static Scanner CM_PHANTOM_01 { get { return new Scanner("CAR_MODEL\\PHANTOM_01"); } }
        public static Scanner CM_PHOENIX_01 { get { return new Scanner("CAR_MODEL\\PHOENIX_01"); } }
        public static Scanner CM_PICADOR_01 { get { return new Scanner("CAR_MODEL\\PICADOR_01"); } }
        public static Scanner CM_POLICE_CAR_01 { get { return new Scanner("CAR_MODEL\\POLICE_CAR_01"); } }
        public static Scanner CM_POLICE_FUGITIVE_01 { get { return new Scanner("CAR_MODEL\\POLICE_FUGITIVE_01"); } }
        public static Scanner CM_POLICE_MAVERICK_01 { get { return new Scanner("CAR_MODEL\\POLICE_MAVERICK_01"); } }
        public static Scanner CM_POLICE_TRANSPORT_01 { get { return new Scanner("CAR_MODEL\\POLICE_TRANSPORT_01"); } }
        public static Scanner CM_PONY_01 { get { return new Scanner("CAR_MODEL\\PONY_01"); } }
        public static Scanner CM_POUNDER_01 { get { return new Scanner("CAR_MODEL\\POUNDER_01"); } }
        public static Scanner CM_PRAIRIE_01 { get { return new Scanner("CAR_MODEL\\PRAIRIE_01"); } }
        public static Scanner CM_PREDATOR_01 { get { return new Scanner("CAR_MODEL\\PREDATOR_01"); } }
        public static Scanner CM_PREMIER_01 { get { return new Scanner("CAR_MODEL\\PREMIER_01"); } }
        public static Scanner CM_PRIMO_01 { get { return new Scanner("CAR_MODEL\\PRIMO_01"); } }
        public static Scanner CM_RADIUS_01 { get { return new Scanner("CAR_MODEL\\RADIUS_01"); } }
        public static Scanner CM_RADI_01 { get { return new Scanner("CAR_MODEL\\RADI_01"); } }
        public static Scanner CM_RANCHER_XL_01 { get { return new Scanner("CAR_MODEL\\RANCHER_XL_01"); } }
        public static Scanner CM_RAPID_GT_01 { get { return new Scanner("CAR_MODEL\\RAPID_GT_01"); } }
        public static Scanner CM_RATLOADER_01 { get { return new Scanner("CAR_MODEL\\RATLOADER_01"); } }
        public static Scanner CM_RC_BANDITO_01 { get { return new Scanner("CAR_MODEL\\RC_BANDITO_01"); } }
        public static Scanner CM_REBEL_01 { get { return new Scanner("CAR_MODEL\\REBEL_01"); } }
        public static Scanner CM_REGINA_01 { get { return new Scanner("CAR_MODEL\\REGINA_01"); } }
        public static Scanner CM_RHINO_01 { get { return new Scanner("CAR_MODEL\\RHINO_01"); } }
        public static Scanner CM_RIDE_ON_MOWER_01 { get { return new Scanner("CAR_MODEL\\RIDE_ON_MOWER_01"); } }
        public static Scanner CM_RIOT_01 { get { return new Scanner("CAR_MODEL\\RIOT_01"); } }
        public static Scanner CM_RIPLEY_01 { get { return new Scanner("CAR_MODEL\\RIPLEY_01"); } }
        public static Scanner CM_RUBBLE_01 { get { return new Scanner("CAR_MODEL\\RUBBLE_01"); } }
        public static Scanner CM_RUFFIAN_01 { get { return new Scanner("CAR_MODEL\\RUFFIAN_01"); } }
        public static Scanner CM_RUINER_01 { get { return new Scanner("CAR_MODEL\\RUINER_01"); } }
        public static Scanner CM_RUMPO_01 { get { return new Scanner("CAR_MODEL\\RUMPO_01"); } }
        public static Scanner CM_SADLER_01 { get { return new Scanner("CAR_MODEL\\SADLER_01"); } }
        public static Scanner CM_SANCHEZ_01 { get { return new Scanner("CAR_MODEL\\SANCHEZ_01"); } }
        public static Scanner CM_SANDKING_01 { get { return new Scanner("CAR_MODEL\\SANDKING_01"); } }
        public static Scanner CM_SCHAFTER_01 { get { return new Scanner("CAR_MODEL\\SCHAFTER_01"); } }
        public static Scanner CM_SCORCHER_01 { get { return new Scanner("CAR_MODEL\\SCORCHER_01"); } }
        public static Scanner CM_SCRAP_01 { get { return new Scanner("CAR_MODEL\\SCRAP_01"); } }
        public static Scanner CM_SEAPLANE_01 { get { return new Scanner("CAR_MODEL\\SEAPLANE_01"); } }
        public static Scanner CM_SEASHARK_01 { get { return new Scanner("CAR_MODEL\\SEASHARK_01"); } }
        public static Scanner CM_SEMINOLE_01 { get { return new Scanner("CAR_MODEL\\SEMINOLE_01"); } }
        public static Scanner CM_SENTINEL_01 { get { return new Scanner("CAR_MODEL\\SENTINEL_01"); } }
        public static Scanner CM_SKYLIFT_01 { get { return new Scanner("CAR_MODEL\\SKYLIFT_01"); } }
        public static Scanner CM_SLAMVAN_01 { get { return new Scanner("CAR_MODEL\\SLAMVAN_01"); } }
        public static Scanner CM_SPEEDER_01 { get { return new Scanner("CAR_MODEL\\SPEEDER_01"); } }
        public static Scanner CM_SPEEDO_01 { get { return new Scanner("CAR_MODEL\\SPEEDO_01"); } }
        public static Scanner CM_SQUALO_01 { get { return new Scanner("CAR_MODEL\\SQUALO_01"); } }
        public static Scanner CM_STANIER_01 { get { return new Scanner("CAR_MODEL\\STANIER_01"); } }
        public static Scanner CM_STINGER_01 { get { return new Scanner("CAR_MODEL\\STINGER_01"); } }
        public static Scanner CM_STINGER_GT_01 { get { return new Scanner("CAR_MODEL\\STINGER_GT_01"); } }
        public static Scanner CM_STOCKADE_01 { get { return new Scanner("CAR_MODEL\\STOCKADE_01"); } }
        public static Scanner CM_STRATUM_01 { get { return new Scanner("CAR_MODEL\\STRATUM_01"); } }
        public static Scanner CM_STRETCH_01 { get { return new Scanner("CAR_MODEL\\STRETCH_01"); } }
        public static Scanner CM_STUNT_01 { get { return new Scanner("CAR_MODEL\\STUNT_01"); } }
        public static Scanner CM_SUBMARINE_01 { get { return new Scanner("CAR_MODEL\\SUBMARINE_01"); } }
        public static Scanner CM_SUBMERSIBLE_01 { get { return new Scanner("CAR_MODEL\\SUBMERSIBLE_01"); } }
        public static Scanner CM_SULTAN_01 { get { return new Scanner("CAR_MODEL\\SULTAN_01"); } }
        public static Scanner CM_SUNTRAP_01 { get { return new Scanner("CAR_MODEL\\SUNTRAP_01"); } }
        public static Scanner CM_SUPER_DIAMOND_01 { get { return new Scanner("CAR_MODEL\\SUPER_DIAMOND_01"); } }
        public static Scanner CM_SURFER_01 { get { return new Scanner("CAR_MODEL\\SURFER_01"); } }
        public static Scanner CM_SURGE_01 { get { return new Scanner("CAR_MODEL\\SURGE_01"); } }
        public static Scanner CM_SXR_01 { get { return new Scanner("CAR_MODEL\\SXR_01"); } }
        public static Scanner CM_TACO_01 { get { return new Scanner("CAR_MODEL\\TACO_01"); } }
        public static Scanner CM_TACO_VAN_01 { get { return new Scanner("CAR_MODEL\\TACO_VAN_01"); } }
        public static Scanner CM_TAILGATER_01 { get { return new Scanner("CAR_MODEL\\TAILGATER_01"); } }
        public static Scanner CM_TAMPA_01 { get { return new Scanner("CAR_MODEL\\TAMPA_01"); } }
        public static Scanner CM_TAXI_01 { get { return new Scanner("CAR_MODEL\\TAXI_01"); } }
        public static Scanner CM_TIPPER_01 { get { return new Scanner("CAR_MODEL\\TIPPER_01"); } }
        public static Scanner CM_TIPTRUCK_01 { get { return new Scanner("CAR_MODEL\\TIPTRUCK_01"); } }
        public static Scanner CM_TITAN_01 { get { return new Scanner("CAR_MODEL\\TITAN_01"); } }
        public static Scanner CM_TORNADO_01 { get { return new Scanner("CAR_MODEL\\TORNADO_01"); } }
        public static Scanner CM_TOUR_BUS_01 { get { return new Scanner("CAR_MODEL\\TOUR_BUS_01"); } }
        public static Scanner CM_TOWTRUCK_01 { get { return new Scanner("CAR_MODEL\\TOWTRUCK_01"); } }
        public static Scanner CM_TR3_01 { get { return new Scanner("CAR_MODEL\\TR3_01"); } }
        public static Scanner CM_TRACTOR_01 { get { return new Scanner("CAR_MODEL\\TRACTOR_01"); } }
        public static Scanner CM_TRASH_01 { get { return new Scanner("CAR_MODEL\\TRASH_01"); } }
        public static Scanner CM_TRIALS_01 { get { return new Scanner("CAR_MODEL\\TRIALS_01"); } }
        public static Scanner CM_TRIBIKE_01 { get { return new Scanner("CAR_MODEL\\TRIBIKE_01"); } }
        public static Scanner CM_TROPIC_01 { get { return new Scanner("CAR_MODEL\\TROPIC_01"); } }
        public static Scanner CM_TUG_BOAT_01 { get { return new Scanner("CAR_MODEL\\TUG_BOAT_01"); } }
        public static Scanner CM_UTILITY_TRUCK_01 { get { return new Scanner("CAR_MODEL\\UTILITY_TRUCK_01"); } }
        public static Scanner CM_VADER_01 { get { return new Scanner("CAR_MODEL\\VADER_01"); } }
        public static Scanner CM_VIGERO_01 { get { return new Scanner("CAR_MODEL\\VIGERO_01"); } }
        public static Scanner CM_VOODOO_01 { get { return new Scanner("CAR_MODEL\\VOODOO_01"); } }
        public static Scanner CM_WASHINGTON_01 { get { return new Scanner("CAR_MODEL\\WASHINGTON_01"); } }
        public static Scanner CM_WAYFARER_01 { get { return new Scanner("CAR_MODEL\\WAYFARER_01"); } }
        public static Scanner CM_WILLARD_01 { get { return new Scanner("CAR_MODEL\\WILLARD_01"); } }
        public static Scanner CM_XL_01 { get { return new Scanner("CAR_MODEL\\XL_01"); } }
        public static Scanner CM_ZION_01 { get { return new Scanner("CAR_MODEL\\ZION_01"); } }
        public static Scanner CM_ZTYPE_01 { get { return new Scanner("CAR_MODEL\\ZTYPE_01"); } }

    }
    public class Color
    {
        public static Scanner COLOR_AQUA_01 { get { return new Scanner("COLOUR\\COLOR_AQUA_01"); } }
        public static Scanner COLOR_BEIGE_01 { get { return new Scanner("COLOUR\\COLOR_BEIGE_01"); } }
        public static Scanner COLOR_BLACK_01 { get { return new Scanner("COLOUR\\COLOR_BLACK_01"); } }
        public static Scanner COLOR_BLUE_01 { get { return new Scanner("COLOUR\\COLOR_BLUE_01"); } }
        public static Scanner COLOR_BRONZE_01 { get { return new Scanner("COLOUR\\COLOR_BRONZE_01"); } }
        public static Scanner COLOR_BROWN_01 { get { return new Scanner("COLOUR\\COLOR_BROWN_01"); } }
        public static Scanner COLOR_DARK_BLUE_01 { get { return new Scanner("COLOUR\\COLOR_DARK_BLUE_01"); } }
        public static Scanner COLOR_DARK_BROWN_01 { get { return new Scanner("COLOUR\\COLOR_DARK_BROWN_01"); } }
        public static Scanner COLOR_DARK_GREEN_01 { get { return new Scanner("COLOUR\\COLOR_DARK_GREEN_01"); } }
        public static Scanner COLOR_DARK_GREY_01 { get { return new Scanner("COLOUR\\COLOR_DARK_GREY_01"); } }
        public static Scanner COLOR_DARK_MAROON_01 { get { return new Scanner("COLOUR\\COLOR_DARK_MAROON_01"); } }
        public static Scanner COLOR_DARK_ORANGE_01 { get { return new Scanner("COLOUR\\COLOR_DARK_ORANGE_01"); } }
        public static Scanner COLOR_DARK_PURPLE_01 { get { return new Scanner("COLOUR\\COLOR_DARK_PURPLE_01"); } }
        public static Scanner COLOR_DARK_RED_01 { get { return new Scanner("COLOUR\\COLOR_DARK_RED_01"); } }
        public static Scanner COLOR_DARK_SILVER_01 { get { return new Scanner("COLOUR\\COLOR_DARK_SILVER_01"); } }
        public static Scanner COLOR_DARK_YELLOW_01 { get { return new Scanner("COLOUR\\COLOR_DARK_YELLOW_01"); } }
        public static Scanner COLOR_GOLD_01 { get { return new Scanner("COLOUR\\COLOR_GOLD_01"); } }
        public static Scanner COLOR_GRAPHITE_01 { get { return new Scanner("COLOUR\\COLOR_GRAPHITE_01"); } }
        public static Scanner COLOR_GREEN_01 { get { return new Scanner("COLOUR\\COLOR_GREEN_01"); } }
        public static Scanner COLOR_GREY_01 { get { return new Scanner("COLOUR\\COLOR_GREY_01"); } }
        public static Scanner COLOR_GREY_02 { get { return new Scanner("COLOUR\\COLOR_GREY_02"); } }
        public static Scanner COLOR_LIGHT_BLUE_01 { get { return new Scanner("COLOUR\\COLOR_LIGHT_BLUE_01"); } }
        public static Scanner COLOR_LIGHT_BROWN_01 { get { return new Scanner("COLOUR\\COLOR_LIGHT_BROWN_01"); } }
        public static Scanner COLOR_LIGHT_GREEN_01 { get { return new Scanner("COLOUR\\COLOR_LIGHT_GREEN_01"); } }
        public static Scanner COLOR_LIGHT_GREY_01 { get { return new Scanner("COLOUR\\COLOR_LIGHT_GREY_01"); } }
        public static Scanner COLOR_LIGHT_MAROON_01 { get { return new Scanner("COLOUR\\COLOR_LIGHT_MAROON_01"); } }
        public static Scanner COLOR_LIGHT_ORANGE_01 { get { return new Scanner("COLOUR\\COLOR_LIGHT_ORANGE_01"); } }
        public static Scanner COLOR_LIGHT_PURPLE_01 { get { return new Scanner("COLOUR\\COLOR_LIGHT_PURPLE_01"); } }
        public static Scanner COLOR_LIGHT_RED_01 { get { return new Scanner("COLOUR\\COLOR_LIGHT_RED_01"); } }
        public static Scanner COLOR_LIGHT_SILVER_01 { get { return new Scanner("COLOUR\\COLOR_LIGHT_SILVER_01"); } }
        public static Scanner COLOR_LIGHT_YELLOW_01 { get { return new Scanner("COLOUR\\COLOR_LIGHT_YELLOW_01"); } }
        public static Scanner COLOR_MAROON_01 { get { return new Scanner("COLOUR\\COLOR_MAROON_01"); } }
        public static Scanner COLOR_ORANGE_01 { get { return new Scanner("COLOUR\\COLOR_ORANGE_01"); } }
        public static Scanner COLOR_PINK_01 { get { return new Scanner("COLOUR\\COLOR_PINK_01"); } }
        public static Scanner COLOR_PURPLE_01 { get { return new Scanner("COLOUR\\COLOR_PURPLE_01"); } }
        public static Scanner COLOR_RED_01 { get { return new Scanner("COLOUR\\COLOR_RED_01"); } }
        public static Scanner COLOR_SILVER_01 { get { return new Scanner("COLOUR\\COLOR_SILVER_01"); } }
        public static Scanner COLOR_WHITE_01 { get { return new Scanner("COLOUR\\COLOR_WHITE_01"); } }
        public static Scanner COLOR_YELLOW_01 { get { return new Scanner("COLOUR\\COLOR_YELLOW_01"); } }
    }
    public class Conjunctives
    {
        public static String NearAtCloseTo()
        {
            int rnd = random.Next(1, 5);
            if (rnd == 1)
                return Scanner.Conjunctives.NEAR_01.Value;
            else if (rnd == 2)
                return Scanner.Conjunctives.AT_01.Value;
            else if (rnd == 3)
                return Scanner.Conjunctives.AT_02.Value;
            else if (rnd == 4)
                return Scanner.Conjunctives.CLOSE_TO_02.Value;
            else
                return Scanner.Conjunctives.CLOSE_TO_01.Value;
        }
        public static Scanner AN_01 { get { return new Scanner("CONJUNCTIVES\\AN_01"); } }
        public static Scanner AN_02 { get { return new Scanner("CONJUNCTIVES\\AN_02"); } }
        public static Scanner AT_01 { get { return new Scanner("CONJUNCTIVES\\AT_01"); } }
        public static Scanner AT_02 { get { return new Scanner("CONJUNCTIVES\\AT_02"); } }
        public static Scanner A_01 { get { return new Scanner("CONJUNCTIVES\\A_01"); } }
        public static Scanner A_02 { get { return new Scanner("CONJUNCTIVES\\A_02"); } }
        public static Scanner CLOSE_TO_01 { get { return new Scanner("CONJUNCTIVES\\CLOSE_TO_01"); } }
        public static Scanner CLOSE_TO_02 { get { return new Scanner("CONJUNCTIVES\\CLOSE_TO_02"); } }
        public static Scanner DRIVING_A_01 { get { return new Scanner("CONJUNCTIVES\\DRIVING_A_01"); } }
        public static Scanner DRIVING_A_02 { get { return new Scanner("CONJUNCTIVES\\DRIVING_A_02"); } }
        public static Scanner INSIDE_01 { get { return new Scanner("CONJUNCTIVES\\INSIDE_01"); } }
        public static Scanner INSIDE_02 { get { return new Scanner("CONJUNCTIVES\\INSIDE_02"); } }
        public static Scanner INSIDE_03 { get { return new Scanner("CONJUNCTIVES\\INSIDE_03"); } }
        public static Scanner IN_01 { get { return new Scanner("CONJUNCTIVES\\IN_01"); } }
        public static Scanner IN_02 { get { return new Scanner("CONJUNCTIVES\\IN_02"); } }
        public static Scanner IN_03 { get { return new Scanner("CONJUNCTIVES\\IN_03"); } }
        public static Scanner IN_04 { get { return new Scanner("CONJUNCTIVES\\IN_04"); } }
        public static Scanner IN_05 { get { return new Scanner("CONJUNCTIVES\\IN_05"); } }
        public static Scanner IN_A_01 { get { return new Scanner("CONJUNCTIVES\\IN_A_01"); } }
        public static Scanner IN_A_02 { get { return new Scanner("CONJUNCTIVES\\IN_A_02"); } }
        public static Scanner NEAR_01 { get { return new Scanner("CONJUNCTIVES\\NEAR_01"); } }
        public static Scanner ON_01 { get { return new Scanner("CONJUNCTIVES\\ON_01"); } }
        public static Scanner ON_02 { get { return new Scanner("CONJUNCTIVES\\ON_02"); } }
        public static Scanner ON_03 { get { return new Scanner("CONJUNCTIVES\\ON_03"); } }
        public static Scanner ON_A_01 { get { return new Scanner("CONJUNCTIVES\\ON_A_01"); } }
        public static Scanner ON_A_02 { get { return new Scanner("CONJUNCTIVES\\ON_A_02"); } }
        public static Scanner OVER_01 { get { return new Scanner("CONJUNCTIVES\\OVER_01"); } }
        public static Scanner OVER_02 { get { return new Scanner("CONJUNCTIVES\\OVER_02"); } }
        public static Scanner OVER_03 { get { return new Scanner("CONJUNCTIVES\\OVER_03"); } }
    }
    public class Crimes
    {
        public static Scanner CRIME_10_99_DAVID_01 { get { return new Scanner("CRIMES\\CRIME_10_99_DAVID_01"); } }
        public static Scanner CRIME_11_351_01 { get { return new Scanner("CRIMES\\CRIME_11_351_01"); } }
        public static Scanner CRIME_11_351_02 { get { return new Scanner("CRIMES\\CRIME_11_351_02"); } }
        public static Scanner CRIME_2_42_01 { get { return new Scanner("CRIMES\\CRIME_2_42_01"); } }
        public static Scanner CRIME_AMBULANCE_REQUESTED_01 { get { return new Scanner("CRIMES\\CRIME_AMBULANCE_REQUESTED_01"); } }
        public static Scanner CRIME_AMBULANCE_REQUESTED_02 { get { return new Scanner("CRIMES\\CRIME_AMBULANCE_REQUESTED_02"); } }
        public static Scanner CRIME_AMBULANCE_REQUESTED_03 { get { return new Scanner("CRIMES\\CRIME_AMBULANCE_REQUESTED_03"); } }
        public static Scanner CRIME_ASSAULT_01 { get { return new Scanner("CRIMES\\CRIME_ASSAULT_01"); } }
        public static Scanner CRIME_ASSAULT_02 { get { return new Scanner("CRIMES\\CRIME_ASSAULT_02"); } }
        public static String CrimeAssaultPeaceOfficerRandom()
        {
            int rnd = random.Next(1, 4);
            if (rnd == 1)
                return Scanner.Crimes.CRIME_ASSAULT_PEACE_OFFICER_01.Value;
            else if (rnd == 2)
                return Scanner.Crimes.CRIME_ASSAULT_PEACE_OFFICER_02.Value;
            else if (rnd == 3)
                return Scanner.Crimes.CRIME_ASSAULT_PEACE_OFFICER_03.Value;
            else
                return Scanner.Crimes.CRIME_ASSAULT_PEACE_OFFICER_04.Value;
        }
        public static Scanner CRIME_ASSAULT_PEACE_OFFICER_01 { get { return new Scanner("CRIMES\\CRIME_ASSAULT_PEACE_OFFICER_01"); } }
        public static Scanner CRIME_ASSAULT_PEACE_OFFICER_02 { get { return new Scanner("CRIMES\\CRIME_ASSAULT_PEACE_OFFICER_02"); } }
        public static Scanner CRIME_ASSAULT_PEACE_OFFICER_03 { get { return new Scanner("CRIMES\\CRIME_ASSAULT_PEACE_OFFICER_03"); } }
        public static Scanner CRIME_ASSAULT_PEACE_OFFICER_04 { get { return new Scanner("CRIMES\\CRIME_ASSAULT_PEACE_OFFICER_04"); } }
        public static Scanner CRIME_ASSAULT_WITH_A_DEADLY_WEAPON_01 { get { return new Scanner("CRIMES\\CRIME_ASSAULT_WITH_A_DEADLY_WEAPON_01"); } }
        public static Scanner CRIME_ASSAULT_WITH_A_DEADLY_WEAPON_02 { get { return new Scanner("CRIMES\\CRIME_ASSAULT_WITH_A_DEADLY_WEAPON_02"); } }
        public static Scanner CRIME_ASSAULT_WITH_A_DEADLY_WEAPON_03 { get { return new Scanner("CRIMES\\CRIME_ASSAULT_WITH_A_DEADLY_WEAPON_03"); } }
        public static Scanner CRIME_BRANDISHING_WEAPON_01 { get { return new Scanner("CRIMES\\CRIME_BRANDISHING_WEAPON_01"); } }
        public static Scanner CRIME_BRANDISHING_WEAPON_02 { get { return new Scanner("CRIMES\\CRIME_BRANDISHING_WEAPON_02"); } }
        public static Scanner CRIME_BRANDISHING_WEAPON_03 { get { return new Scanner("CRIMES\\CRIME_BRANDISHING_WEAPON_03"); } }
        public static Scanner CRIME_DISTURBING_THE_PEACE_01 { get { return new Scanner("CRIMES\\CRIME_DISTURBING_THE_PEACE_01"); } }
        public static Scanner CRIME_DISTURBING_THE_PEACE_02 { get { return new Scanner("CRIMES\\CRIME_DISTURBING_THE_PEACE_02"); } }
        public static Scanner CRIME_GRAND_THEFT_AUTO_01 { get { return new Scanner("CRIMES\\CRIME_GRAND_THEFT_AUTO_01"); } }
        public static Scanner CRIME_GRAND_THEFT_AUTO_02 { get { return new Scanner("CRIMES\\CRIME_GRAND_THEFT_AUTO_02"); } }
        public static Scanner CRIME_GRAND_THEFT_AUTO_03 { get { return new Scanner("CRIMES\\CRIME_GRAND_THEFT_AUTO_03"); } }
        public static Scanner CRIME_GRAND_THEFT_AUTO_04 { get { return new Scanner("CRIMES\\CRIME_GRAND_THEFT_AUTO_04"); } }
        public static Scanner CRIME_GUNFIRE_01 { get { return new Scanner("CRIMES\\CRIME_GUNFIRE_01"); } }
        public static Scanner CRIME_GUNFIRE_02 { get { return new Scanner("CRIMES\\CRIME_GUNFIRE_02"); } }
        public static Scanner CRIME_GUNFIRE_03 { get { return new Scanner("CRIMES\\CRIME_GUNFIRE_03"); } }
        public static Scanner CRIME_HIT_AND_RUN_01 { get { return new Scanner("CRIMES\\CRIME_HIT_AND_RUN_01"); } }
        public static Scanner CRIME_HIT_AND_RUN_02 { get { return new Scanner("CRIMES\\CRIME_HIT_AND_RUN_02"); } }
        public static Scanner CRIME_HIT_AND_RUN_03 { get { return new Scanner("CRIMES\\CRIME_HIT_AND_RUN_03"); } }
        public static Scanner CRIME_OFFICER_IN_NEED_OF_ASSISTANCE_01 { get { return new Scanner("CRIMES\\CRIME_OFFICER_IN_NEED_OF_ASSISTANCE_01"); } }
        public static Scanner CRIME_OFFICER_IN_NEED_OF_ASSISTANCE_02 { get { return new Scanner("CRIMES\\CRIME_OFFICER_IN_NEED_OF_ASSISTANCE_02"); } }
        public static Scanner CRIME_OFFICER_IN_NEED_OF_ASSISTANCE_03 { get { return new Scanner("CRIMES\\CRIME_OFFICER_IN_NEED_OF_ASSISTANCE_03"); } }
        public static Scanner CRIME_OFFICER_IN_NEED_OF_ASSISTANCE_04 { get { return new Scanner("CRIMES\\CRIME_OFFICER_IN_NEED_OF_ASSISTANCE_04"); } }
        public static Scanner CRIME_OFFICER_REQUESTS_AIR_SUPPORT_01 { get { return new Scanner("CRIMES\\CRIME_OFFICER_REQUESTS_AIR_SUPPORT_01"); } }
        public static Scanner CRIME_OFFICER_REQUESTS_AIR_SUPPORT_02 { get { return new Scanner("CRIMES\\CRIME_OFFICER_REQUESTS_AIR_SUPPORT_02"); } }
        public static Scanner CRIME_OFFICER_REQUESTS_TRANSPORT_01 { get { return new Scanner("CRIMES\\CRIME_OFFICER_REQUESTS_TRANSPORT_01"); } }
        public static Scanner CRIME_RESIST_ARREST_01 { get { return new Scanner("CRIMES\\CRIME_RESIST_ARREST_01"); } }
        public static Scanner CRIME_RESIST_ARREST_02 { get { return new Scanner("CRIMES\\CRIME_RESIST_ARREST_02"); } }
        public static Scanner CRIME_RESIST_ARREST_03 { get { return new Scanner("CRIMES\\CRIME_RESIST_ARREST_03"); } }
        public static Scanner CRIME_RESIST_ARREST_04 { get { return new Scanner("CRIMES\\CRIME_RESIST_ARREST_04"); } }
        public static Scanner CRIME_ROBBERY_01 { get { return new Scanner("CRIMES\\CRIME_ROBBERY_01"); } }
        public static Scanner CRIME_ROBBERY_02 { get { return new Scanner("CRIMES\\CRIME_ROBBERY_02"); } }
        public static Scanner CRIME_ROBBERY_03 { get { return new Scanner("CRIMES\\CRIME_ROBBERY_03"); } }
        public static Scanner CRIME_ROBBERY_04 { get { return new Scanner("CRIMES\\CRIME_ROBBERY_04"); } }
        public static Scanner CRIME_SHOTS_FIRED_01 { get { return new Scanner("CRIMES\\CRIME_SHOTS_FIRED_01"); } }
        public static Scanner CRIME_SHOTS_FIRED_AT_AN_OFFICER_01 { get { return new Scanner("CRIMES\\CRIME_SHOTS_FIRED_AT_AN_OFFICER_01"); } }
        public static Scanner CRIME_SHOTS_FIRED_AT_AN_OFFICER_02 { get { return new Scanner("CRIMES\\CRIME_SHOTS_FIRED_AT_AN_OFFICER_02"); } }
        public static Scanner CRIME_SHOTS_FIRED_AT_AN_OFFICER_03 { get { return new Scanner("CRIMES\\CRIME_SHOTS_FIRED_AT_AN_OFFICER_03"); } }
        public static Scanner CRIME_SUSPECT_ON_THE_RUN_01 { get { return new Scanner("CRIMES\\CRIME_SUSPECT_ON_THE_RUN_01"); } }
        public static Scanner CRIME_SUSPECT_ON_THE_RUN_02 { get { return new Scanner("CRIMES\\CRIME_SUSPECT_ON_THE_RUN_02"); } }
        public static Scanner CRIME_SUSPECT_ON_THE_RUN_03 { get { return new Scanner("CRIMES\\CRIME_SUSPECT_ON_THE_RUN_03"); } }
        public static Scanner CRIME_THREATEN_OFFICER_WITH_FIREARM_01 { get { return new Scanner("CRIMES\\CRIME_THREATEN_OFFICER_WITH_FIREARM_01"); } }
    }
    public class CrookArrested
    {
        public static String CrookArrestedRandom()
        {
            int rnd = random.Next(1, 4);
            if (rnd == 1)
                return Scanner.CrookArrested.SUSPECT_APPREHENDED_01.Value;
            else if (rnd == 2)
                return Scanner.CrookArrested.SUSPECT_APPREHENDED_02.Value;
            else if (rnd == 3)
                return Scanner.CrookArrested.SUSPECT_ARRESTED.Value;
            else
                return Scanner.CrookArrested.SUSPECT_IN_CUSTODY.Value;
        }
        public static Scanner SUSPECT_APPREHENDED_01 { get { return new Scanner("CROOK_ARRESTED\\SUSPECT_APPREHENDED_01"); } }
        public static Scanner SUSPECT_APPREHENDED_02 { get { return new Scanner("CROOK_ARRESTED\\SUSPECT_APPREHENDED_02"); } }
        public static Scanner SUSPECT_ARRESTED { get { return new Scanner("CROOK_ARRESTED\\SUSPECT_ARRESTED"); } }
        public static Scanner SUSPECT_IN_CUSTODY { get { return new Scanner("CROOK_ARRESTED\\SUSPECT_IN_CUSTODY"); } }
    }
    public class Direction
    {
        public static Scanner DIRECTION_BOUND_EAST_01 { get { return new Scanner("DIRECTION\\DIRECTION_BOUND_EAST_01"); } }
        public static Scanner DIRECTION_BOUND_NORTH_01 { get { return new Scanner("DIRECTION\\DIRECTION_BOUND_NORTH_01"); } }
        public static Scanner DIRECTION_BOUND_SOUTH_01 { get { return new Scanner("DIRECTION\\DIRECTION_BOUND_SOUTH_01"); } }
        public static Scanner DIRECTION_BOUND_WEST_01 { get { return new Scanner("DIRECTION\\DIRECTION_BOUND_WEST_01"); } }
        public static Scanner DIRECTION_HEADING_EAST_01 { get { return new Scanner("DIRECTION\\DIRECTION_HEADING_EAST_01"); } }
        public static Scanner DIRECTION_HEADING_NORTH_01 { get { return new Scanner("DIRECTION\\DIRECTION_HEADING_NORTH_01"); } }
        public static Scanner DIRECTION_HEADING_SOUTH_01 { get { return new Scanner("DIRECTION\\DIRECTION_HEADING_SOUTH_01"); } }
        public static Scanner DIRECTION_HEADING_WEST_01 { get { return new Scanner("DIRECTION\\DIRECTION_HEADING_WEST_01"); } }
    }
    public class LastSeen
    {
        public static Scanner SUSPECT_LAST_SEEN_01 { get { return new Scanner("LAST_SEEN\\SUSPECT_LAST_SEEN_01"); } }
    }
    public class OnFoot
    {
        public static Scanner ON_FOOT_01 { get { return new Scanner("ON_FOOT\\ON_FOOT_01"); } }
        public static Scanner ON_FOOT_02 { get { return new Scanner("ON_FOOT\\ON_FOOT_02"); } }
    }
    public class ReportResponce
    {
        public static Scanner REPORT_RESPONSE_COPY_01 { get { return new Scanner("REPORT_RESPONSE\\REPORT_RESPONSE_COPY_01"); } }
        public static Scanner REPORT_RESPONSE_COPY_02 { get { return new Scanner("REPORT_RESPONSE\\REPORT_RESPONSE_COPY_02"); } }
        public static Scanner REPORT_RESPONSE_COPY_03 { get { return new Scanner("REPORT_RESPONSE\\REPORT_RESPONSE_COPY_03"); } }
        public static Scanner REPORT_RESPONSE_COPY_04 { get { return new Scanner("REPORT_RESPONSE\\REPORT_RESPONSE_COPY_04"); } }
    }
    public class Resident
    {
        public static Scanner DISPATCH_INTRO_01 { get { return new Scanner("RESIDENT\\DISPATCH_INTRO_01"); } }
        public static Scanner DISPATCH_INTRO_02 { get { return new Scanner("RESIDENT\\DISPATCH_INTRO_02"); } }
        public static Scanner INSERT_01 { get { return new Scanner("RESIDENT\\INSERT_01"); } }
        public static Scanner INSERT_02 { get { return new Scanner("RESIDENT\\INSERT_02"); } }
        public static Scanner INSERT_03 { get { return new Scanner("RESIDENT\\INSERT_03"); } }
        public static Scanner INSERT_04 { get { return new Scanner("RESIDENT\\INSERT_04"); } }
        public static Scanner INSERT_05 { get { return new Scanner("RESIDENT\\INSERT_05"); } }
        public static Scanner INSERT_06 { get { return new Scanner("RESIDENT\\INSERT_06"); } }
        public static Scanner INSERT_07 { get { return new Scanner("RESIDENT\\INSERT_07"); } }
        public static Scanner INTRO_01 { get { return new Scanner("RESIDENT\\INTRO_01"); } }
        public static Scanner INTRO_02 { get { return new Scanner("RESIDENT\\INTRO_02"); } }
        public static Scanner IN_01 { get { return new Scanner("RESIDENT\\IN_01"); } }
        public static Scanner IN_02 { get { return new Scanner("RESIDENT\\IN_02"); } }
        public static Scanner NOISE_LOOP_01 { get { return new Scanner("RESIDENT\\NOISE_LOOP_01"); } }
        public static Scanner OFFICER_INTRO_01 { get { return new Scanner("RESIDENT\\OFFICER_INTRO_01"); } }
        public static Scanner OFFICER_INTRO_02 { get { return new Scanner("RESIDENT\\OFFICER_INTRO_02"); } }
        public static Scanner OUTRO_01 { get { return new Scanner("RESIDENT\\OUTRO_01"); } }
        public static Scanner OUTRO_02 { get { return new Scanner("RESIDENT\\OUTRO_02"); } }
        public static Scanner OUTRO_03 { get { return new Scanner("RESIDENT\\OUTRO_03"); } }
    }
    public class SpecificLocation
    {
        public static Scanner SPECIFIC_LOCATION_BOB_MULET_SALON_01 { get { return new Scanner("SPECIFIC_LOCATION\\SPECIFIC_LOCATION_BOB_MULET_SALON_01"); } }
        public static Scanner SPECIFIC_LOCATION_BRADDOCK_PASS_GAS_STATION_01 { get { return new Scanner("SPECIFIC_LOCATION\\SPECIFIC_LOCATION_BRADDOCK_PASS_GAS_STATION_01"); } }
        public static Scanner SPECIFIC_LOCATION_EARLS_MINI_MART_01 { get { return new Scanner("SPECIFIC_LOCATION\\SPECIFIC_LOCATION_EARLS_MINI_MART_01"); } }
        public static Scanner SPECIFIC_LOCATION_HARMONY_24_7_MARKET { get { return new Scanner("SPECIFIC_LOCATION\\SPECIFIC_LOCATION_HARMONY_24_7_MARKET"); } }
        public static Scanner SPECIFIC_LOCATION_LITTLE_SEOUL_RON_STATION_01 { get { return new Scanner("SPECIFIC_LOCATION\\SPECIFIC_LOCATION_LITTLE_SEOUL_RON_STATION_01"); } }
        public static Scanner SPECIFIC_LOCATION_LITTLE_SEOUL_RON_STATION_02 { get { return new Scanner("SPECIFIC_LOCATION\\SPECIFIC_LOCATION_LITTLE_SEOUL_RON_STATION_02"); } }
        public static Scanner SPECIFIC_LOCATION_MIRROR_PARK_GAS_STATION_01 { get { return new Scanner("SPECIFIC_LOCATION\\SPECIFIC_LOCATION_MIRROR_PARK_GAS_STATION_01"); } }
        public static Scanner SPECIFIC_LOCATION_STRAWBERRY_24_7_MARKET_01 { get { return new Scanner("SPECIFIC_LOCATION\\SPECIFIC_LOCATION_STRAWBERRY_24_7_MARKET_01"); } }
        public static Scanner SPECIFIC_LOCATION_VESPUCCI_SUBURBAN_01 { get { return new Scanner("SPECIFIC_LOCATION\\SPECIFIC_LOCATION_VESPUCCI_SUBURBAN_01"); } }
        public static Scanner SPECIFIC_LOCATION_VINEWOOD_24_7_MARKET_01 { get { return new Scanner("SPECIFIC_LOCATION\\SPECIFIC_LOCATION_VINEWOOD_24_7_MARKET_01"); } }
        public static Scanner SPECIFIC_LOCATION_VINEWOOD_BRANCH_OF_SUB_URBAN_01 { get { return new Scanner("SPECIFIC_LOCATION\\SPECIFIC_LOCATION_VINEWOOD_BRANCH_OF_SUB_URBAN_01"); } }
    }
    public class Streets
    {
        public static Scanner STREET_ABE_MILTON_PKWY_01 { get { return new Scanner("STREETS\\STREET_ABE_MILTON_PKWY_01"); } }
        public static Scanner STREET_ARMADILLO_AVE_01 { get { return new Scanner("STREETS\\STREET_ARMADILLO_AVE_01"); } }
        public static Scanner STREET_CENTURY_BLVD_01 { get { return new Scanner("STREETS\\STREET_CENTURY_BLVD_01"); } }
        public static Scanner STREET_CENTURY_FWY_01 { get { return new Scanner("STREETS\\STREET_CENTURY_FWY_01"); } }
        public static Scanner STREET_CHOLLA_SPRINGS_AVE_01 { get { return new Scanner("STREETS\\STREET_CHOLLA_SPRINGS_AVE_01"); } }
        public static Scanner STREET_DEL_PERRO_FWY_01 { get { return new Scanner("STREETS\\STREET_DEL_PERRO_FWY_01"); } }
        public static Scanner STREET_ELYSIAN_FIELDS_FWY_01 { get { return new Scanner("STREETS\\STREET_ELYSIAN_FIELDS_FWY_01"); } }
        public static Scanner STREET_GREAT_OCEAN_HWY_01 { get { return new Scanner("STREETS\\STREET_GREAT_OCEAN_HWY_01"); } }
        public static Scanner STREET_GREAT_OCEAN_HWY_02 { get { return new Scanner("STREETS\\STREET_GREAT_OCEAN_HWY_02"); } }
        public static Scanner STREET_LA_PUERTA_FWY_01 { get { return new Scanner("STREETS\\STREET_LA_PUERTA_FWY_01"); } }
        public static Scanner STREET_MORNINGWOOD_BLVD_01 { get { return new Scanner("STREETS\\STREET_MORNINGWOOD_BLVD_01"); } }
        public static Scanner STREET_OLYMPIC_FWY_01 { get { return new Scanner("STREETS\\STREET_OLYMPIC_FWY_01"); } }
        public static Scanner STREET_PALOMINO_FWY_01 { get { return new Scanner("STREETS\\STREET_PALOMINO_FWY_01"); } }
        public static Scanner STREET_POPULAR_ST_01 { get { return new Scanner("STREETS\\STREET_POPULAR_ST_01"); } }
        public static Scanner STREET_PORTOLA_DR_01 { get { return new Scanner("STREETS\\STREET_PORTOLA_DR_01"); } }
        public static Scanner STREET_ROUTE_68_01 { get { return new Scanner("STREETS\\STREET_ROUTE_68_01"); } }
        public static Scanner STREET_SENORA_FWY_01 { get { return new Scanner("STREETS\\STREET_SENORA_FWY_01"); } }
        public static Scanner STREET_SENORA_FWY_02 { get { return new Scanner("STREETS\\STREET_SENORA_FWY_02"); } }
        public static Scanner STREET_UNION_STREET_01 { get { return new Scanner("STREETS\\STREET_UNION_STREET_01"); } }
        public static Scanner STREET_VINE_ST_01 { get { return new Scanner("STREETS\\STREET_VINE_ST_01"); } }
        public static Scanner STREET_YORK_ST_01 { get { return new Scanner("STREETS\\STREET_YORK_ST_01"); } }
    }
    public class Suspect
    {
        public static Scanner SUSPECT_HEADING_01 { get { return new Scanner("SUSPECT\\SUSPECT_HEADING_01"); } }
        public static Scanner SUSPECT_HEADING_02 { get { return new Scanner("SUSPECT\\SUSPECT_HEADING_02"); } }
        public static Scanner SUSPECT_HEADING_03 { get { return new Scanner("SUSPECT\\SUSPECT_HEADING_03"); } }
        public static Scanner SUSPECT_IS_01 { get { return new Scanner("SUSPECT\\SUSPECT_IS_01"); } }
        public static Scanner SUSPECT_IS_02 { get { return new Scanner("SUSPECT\\SUSPECT_IS_02"); } }
        public static Scanner SUSPECT_LAST_SEEN_01 { get { return new Scanner("SUSPECT\\SUSPECT_LAST_SEEN_01"); } }
        public static Scanner SUSPECT_LAST_SEEN_02 { get { return new Scanner("SUSPECT\\SUSPECT_LAST_SEEN_02"); } }

    }
    public class Suspects
    {
        public static Scanner SUSPECTS_ARE_01 { get { return new Scanner("SUSPECTS\\SUSPECTS_ARE_01"); } }
        public static Scanner SUSPECTS_ARE_02 { get { return new Scanner("SUSPECTS\\SUSPECTS_ARE_02"); } }
        public static Scanner SUSPECTS_ARE_03 { get { return new Scanner("SUSPECTS\\SUSPECTS_ARE_03"); } }
        public static Scanner SUSPECTS_ARE_04 { get { return new Scanner("SUSPECTS\\SUSPECTS_ARE_04"); } }
        public static Scanner SUSPECTS_HEADING_01 { get { return new Scanner("SUSPECTS\\SUSPECTS_HEADING_01"); } }
        public static Scanner SUSPECTS_HEADING_02 { get { return new Scanner("SUSPECTS\\SUSPECTS_HEADING_02"); } }
        public static Scanner SUSPECTS_HEADING_03 { get { return new Scanner("SUSPECTS\\SUSPECTS_HEADING_03"); } }
        public static Scanner SUSPECTS_HEADING_04 { get { return new Scanner("SUSPECTS\\SUSPECTS_HEADING_04"); } }
        public static Scanner SUSPECTS_HEADING_05 { get { return new Scanner("SUSPECTS\\SUSPECTS_HEADING_05"); } }
        public static Scanner SUSPECTS_LAST_SEEN_01 { get { return new Scanner("SUSPECTS\\SUSPECTS_LAST_SEEN_01"); } }
        public static Scanner SUSPECTS_LAST_SEEN_02 { get { return new Scanner("SUSPECTS\\SUSPECTS_LAST_SEEN_02"); } }
        public static Scanner SUSPECTS_LAST_SEEN_03 { get { return new Scanner("SUSPECTS\\SUSPECTS_LAST_SEEN_03"); } }
        public static Scanner SUSPECTS_LAST_SEEN_04 { get { return new Scanner("SUSPECTS\\SUSPECTS_LAST_SEEN_04"); } }
    }
    public class s_f_y_cop_01_black_full_01
    {
        public static Scanner REPORT_SUSPECT_CRASHED_VEHICLE_01 { get { return new Scanner("s_f_y_cop_01_black_full_01\\REPORT_SUSPECT_CRASHED_VEHICLE_01"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_01 { get { return new Scanner("s_f_y_cop_01_black_full_01\\REPORT_SUSPECT_ENTERED_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_02 { get { return new Scanner("s_f_y_cop_01_black_full_01\\REPORT_SUSPECT_ENTERED_FREEWAY_02"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_METRO_01 { get { return new Scanner("s_f_y_cop_01_black_full_01\\REPORT_SUSPECT_ENTERED_METRO_01"); } }
        public static Scanner REPORT_SUSPECT_LEFT_FREEWAY_01 { get { return new Scanner("s_f_y_cop_01_black_full_01\\REPORT_SUSPECT_LEFT_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_LEFT_FREEWAY_02 { get { return new Scanner("s_f_y_cop_01_black_full_01\\REPORT_SUSPECT_LEFT_FREEWAY_02"); } }
        public static Scanner REQUEST_BACKUP_01 { get { return new Scanner("s_f_y_cop_01_black_full_01\\REQUEST_BACKUP_01"); } }
        public static Scanner REQUEST_BACKUP_02 { get { return new Scanner("s_f_y_cop_01_black_full_01\\REQUEST_BACKUP_02"); } }
        public static Scanner REQUEST_BACKUP_03 { get { return new Scanner("s_f_y_cop_01_black_full_01\\REQUEST_BACKUP_03"); } }
        public static Scanner SUSPECT_CRASHED_01 { get { return new Scanner("s_f_y_cop_01_black_full_01\\SUSPECT_CRASHED_01"); } }
        public static Scanner SUSPECT_IS_IN_CAR_01 { get { return new Scanner("s_f_y_cop_01_black_full_01\\SUSPECT_IS_IN_CAR_01"); } }
        public static Scanner SUSPECT_IS_ON_BIKE_01 { get { return new Scanner("s_f_y_cop_01_black_full_01\\SUSPECT_IS_ON_BIKE_01"); } }
        public static Scanner SUSPECT_IS_ON_FOOT_01 { get { return new Scanner("s_f_y_cop_01_black_full_01\\SUSPECT_IS_ON_FOOT_01"); } }
        public static Scanner SUSPECT_IS_ON_FOOT_02 { get { return new Scanner("s_f_y_cop_01_black_full_01\\SUSPECT_IS_ON_FOOT_02"); } }
    }
    public class s_f_y_cop_01_black_full_02
    {
        public static Scanner REPORT_SUSPECT_CRASHED_VEHICLE_01 { get { return new Scanner("s_f_y_cop_01_black_full_02\\REPORT_SUSPECT_CRASHED_VEHICLE_01"); } }
        public static Scanner REPORT_SUSPECT_CRASHED_VEHICLE_02 { get { return new Scanner("s_f_y_cop_01_black_full_02\\REPORT_SUSPECT_CRASHED_VEHICLE_02"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_01 { get { return new Scanner("s_f_y_cop_01_black_full_02\\REPORT_SUSPECT_ENTERED_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_02 { get { return new Scanner("s_f_y_cop_01_black_full_02\\REPORT_SUSPECT_ENTERED_FREEWAY_02"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_METRO_01 { get { return new Scanner("s_f_y_cop_01_black_full_02\\REPORT_SUSPECT_ENTERED_METRO_01"); } }
        public static Scanner REPORT_SUSPECT_IS_IN_CAR_01 { get { return new Scanner("s_f_y_cop_01_black_full_02\\REPORT_SUSPECT_IS_IN_CAR_01"); } }
        public static Scanner REPORT_SUSPECT_IS_IN_CAR_02 { get { return new Scanner("s_f_y_cop_01_black_full_02\\REPORT_SUSPECT_IS_IN_CAR_02"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_BIKE_01 { get { return new Scanner("s_f_y_cop_01_black_full_02\\REPORT_SUSPECT_IS_ON_BIKE_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_01 { get { return new Scanner("s_f_y_cop_01_black_full_02\\REPORT_SUSPECT_IS_ON_FOOT_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_02 { get { return new Scanner("s_f_y_cop_01_black_full_02\\REPORT_SUSPECT_IS_ON_FOOT_02"); } }
        public static Scanner REPORT_SUSPECT_LEFT_FREEWAY_01 { get { return new Scanner("s_f_y_cop_01_black_full_02\\REPORT_SUSPECT_LEFT_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_LEFT_FREEWAY_02 { get { return new Scanner("s_f_y_cop_01_black_full_02\\REPORT_SUSPECT_LEFT_FREEWAY_02"); } }
        public static Scanner REQUEST_BACKUP_01 { get { return new Scanner("s_f_y_cop_01_black_full_02\\REQUEST_BACKUP_01"); } }
        public static Scanner REQUEST_BACKUP_02 { get { return new Scanner("s_f_y_cop_01_black_full_02\\REQUEST_BACKUP_02"); } }
    }
    public class s_f_y_cop_01_white_full_01
    {
        public static Scanner REPORT_SUSPECT_CRASHED_VEHICLE_01 { get { return new Scanner("s_f_y_cop_01_white_full_01\\REPORT_SUSPECT_CRASHED_VEHICLE_01"); } }
        public static Scanner REPORT_SUSPECT_CRASHED_VEHICLE_02 { get { return new Scanner("s_f_y_cop_01_white_full_01\\REPORT_SUSPECT_CRASHED_VEHICLE_02"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_01 { get { return new Scanner("s_f_y_cop_01_white_full_01\\REPORT_SUSPECT_ENTERED_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_02 { get { return new Scanner("s_f_y_cop_01_white_full_01\\REPORT_SUSPECT_ENTERED_FREEWAY_02"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_METRO_01 { get { return new Scanner("s_f_y_cop_01_white_full_01\\REPORT_SUSPECT_ENTERED_METRO_01"); } }
        public static Scanner REPORT_SUSPECT_IS_IN_CAR_01 { get { return new Scanner("s_f_y_cop_01_white_full_01\\REPORT_SUSPECT_IS_IN_CAR_01"); } }
        public static Scanner REPORT_SUSPECT_IS_IN_CAR_02 { get { return new Scanner("s_f_y_cop_01_white_full_01\\REPORT_SUSPECT_IS_IN_CAR_02"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_BIKE_01 { get { return new Scanner("s_f_y_cop_01_white_full_01\\REPORT_SUSPECT_IS_ON_BIKE_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_01 { get { return new Scanner("s_f_y_cop_01_white_full_01\\REPORT_SUSPECT_IS_ON_FOOT_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_02 { get { return new Scanner("s_f_y_cop_01_white_full_01\\REPORT_SUSPECT_IS_ON_FOOT_02"); } }
        public static Scanner REPORT_SUSPECT_LEFT_FREEWAY_01 { get { return new Scanner("s_f_y_cop_01_white_full_01\\REPORT_SUSPECT_LEFT_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_LEFT_FREEWAY_02 { get { return new Scanner("s_f_y_cop_01_white_full_01\\REPORT_SUSPECT_LEFT_FREEWAY_02"); } }
        public static Scanner REQUEST_BACKUP_01 { get { return new Scanner("s_f_y_cop_01_white_full_01\\REQUEST_BACKUP_01"); } }
        public static Scanner REQUEST_BACKUP_02 { get { return new Scanner("s_f_y_cop_01_white_full_01\\REQUEST_BACKUP_02"); } }
    }
    public class s_f_y_cop_01_white_full_02
    {
        public static Scanner REPORT_SUSPECT_CRASHED_VEHICLE_01 { get { return new Scanner("s_f_y_cop_01_white_full_02\\REPORT_SUSPECT_CRASHED_VEHICLE_01"); } }
        public static Scanner REPORT_SUSPECT_CRASHED_VEHICLE_02 { get { return new Scanner("s_f_y_cop_01_white_full_02\\REPORT_SUSPECT_CRASHED_VEHICLE_02"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_01 { get { return new Scanner("s_f_y_cop_01_white_full_02\\REPORT_SUSPECT_ENTERED_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_02 { get { return new Scanner("s_f_y_cop_01_white_full_02\\REPORT_SUSPECT_ENTERED_FREEWAY_02"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_METRO_01 { get { return new Scanner("s_f_y_cop_01_white_full_02\\REPORT_SUSPECT_ENTERED_METRO_01"); } }
        public static Scanner REPORT_SUSPECT_IS_IN_CAR_01 { get { return new Scanner("s_f_y_cop_01_white_full_02\\REPORT_SUSPECT_IS_IN_CAR_01"); } }
        public static Scanner REPORT_SUSPECT_IS_IN_CAR_02 { get { return new Scanner("s_f_y_cop_01_white_full_02\\REPORT_SUSPECT_IS_IN_CAR_02"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_BIKE_01 { get { return new Scanner("s_f_y_cop_01_white_full_02\\REPORT_SUSPECT_IS_ON_BIKE_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_01 { get { return new Scanner("s_f_y_cop_01_white_full_02\\REPORT_SUSPECT_IS_ON_FOOT_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_02 { get { return new Scanner("s_f_y_cop_01_white_full_02\\REPORT_SUSPECT_IS_ON_FOOT_02"); } }
        public static Scanner REPORT_SUSPECT_LEFT_FREEWAY_01 { get { return new Scanner("s_f_y_cop_01_white_full_02\\REPORT_SUSPECT_LEFT_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_LEFT_FREEWAY_02 { get { return new Scanner("s_f_y_cop_01_white_full_02\\REPORT_SUSPECT_LEFT_FREEWAY_02"); } }
        public static Scanner REQUEST_BACKUP_01 { get { return new Scanner("s_f_y_cop_01_white_full_02\\REQUEST_BACKUP_01"); } }
        public static Scanner REQUEST_BACKUP_02 { get { return new Scanner("s_f_y_cop_01_white_full_02\\REQUEST_BACKUP_02"); } }
    }
    public class s_m_y_cop_01_black_full_01
    {
        public static Scanner REPORT_SUSPECT_CRASHED_VEHICLE_01 { get { return new Scanner("s_m_y_cop_01_black_full_01\\REPORT_SUSPECT_CRASHED_VEHICLE_01"); } }
        public static Scanner REPORT_SUSPECT_CRASHED_VEHICLE_02 { get { return new Scanner("s_m_y_cop_01_black_full_01\\REPORT_SUSPECT_CRASHED_VEHICLE_02"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_01 { get { return new Scanner("s_m_y_cop_01_black_full_01\\REPORT_SUSPECT_ENTERED_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_02 { get { return new Scanner("s_m_y_cop_01_black_full_01\\REPORT_SUSPECT_ENTERED_FREEWAY_02"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_METRO_01 { get { return new Scanner("s_m_y_cop_01_black_full_01\\REPORT_SUSPECT_ENTERED_METRO_01"); } }
        public static Scanner REPORT_SUSPECT_IS_IN_CAR_01 { get { return new Scanner("s_m_y_cop_01_black_full_01\\REPORT_SUSPECT_IS_IN_CAR_01"); } }
        public static Scanner REPORT_SUSPECT_IS_IN_CAR_02 { get { return new Scanner("s_m_y_cop_01_black_full_01\\REPORT_SUSPECT_IS_IN_CAR_02"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_BIKE_01 { get { return new Scanner("s_m_y_cop_01_black_full_01\\REPORT_SUSPECT_IS_ON_BIKE_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_01 { get { return new Scanner("s_m_y_cop_01_black_full_01\\REPORT_SUSPECT_IS_ON_FOOT_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_02 { get { return new Scanner("s_m_y_cop_01_black_full_01\\REPORT_SUSPECT_IS_ON_FOOT_02"); } }
        public static Scanner REPORT_SUSPECT_LEFT_FREEWAY_01 { get { return new Scanner("s_m_y_cop_01_black_full_01\\REPORT_SUSPECT_LEFT_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_LEFT_FREEWAY_02 { get { return new Scanner("s_m_y_cop_01_black_full_01\\REPORT_SUSPECT_LEFT_FREEWAY_02"); } }
        public static Scanner REQUEST_BACKUP_01 { get { return new Scanner("s_m_y_cop_01_black_full_01\\REQUEST_BACKUP_01"); } }
        public static Scanner REQUEST_BACKUP_02 { get { return new Scanner("s_m_y_cop_01_black_full_01\\REQUEST_BACKUP_02"); } }
    }
    public class s_m_y_cop_01_black_full_02
    {
        public static Scanner HELI_APPROACHING_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_full_02\\HELI_APPROACHING_DISPATCH_01"); } }
        public static Scanner HELI_APPROACHING_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_full_02\\HELI_APPROACHING_DISPATCH_02"); } }
        public static Scanner HELI_MAYDAY_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_full_02\\HELI_MAYDAY_DISPATCH_01"); } }
        public static Scanner HELI_MAYDAY_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_full_02\\HELI_MAYDAY_DISPATCH_02"); } }
        public static Scanner HELI_NO_VISUAL_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_full_02\\HELI_NO_VISUAL_DISPATCH_01"); } }
        public static Scanner HELI_NO_VISUAL_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_full_02\\HELI_NO_VISUAL_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_EAST_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_full_02\\HELI_VISUAL_HEADING_EAST_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_EAST_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_full_02\\HELI_VISUAL_HEADING_EAST_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_NORTH_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_full_02\\HELI_VISUAL_HEADING_NORTH_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_NORTH_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_full_02\\HELI_VISUAL_HEADING_NORTH_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_SOUTH_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_full_02\\HELI_VISUAL_HEADING_SOUTH_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_SOUTH_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_full_02\\HELI_VISUAL_HEADING_SOUTH_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_WEST_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_full_02\\HELI_VISUAL_HEADING_WEST_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_WEST_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_full_02\\HELI_VISUAL_HEADING_WEST_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_ON_FOOT_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_full_02\\HELI_VISUAL_ON_FOOT_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_ON_FOOT_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_full_02\\HELI_VISUAL_ON_FOOT_DISPATCH_02"); } }
        public static Scanner REPORT_SUSPECT_CRASHED_VEHICLE_01 { get { return new Scanner("s_m_y_cop_01_black_full_02\\REPORT_SUSPECT_CRASHED_VEHICLE_01"); } }
        public static Scanner REPORT_SUSPECT_CRASHED_VEHICLE_02 { get { return new Scanner("s_m_y_cop_01_black_full_02\\REPORT_SUSPECT_CRASHED_VEHICLE_02"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_01 { get { return new Scanner("s_m_y_cop_01_black_full_02\\REPORT_SUSPECT_ENTERED_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_02 { get { return new Scanner("s_m_y_cop_01_black_full_02\\REPORT_SUSPECT_ENTERED_FREEWAY_02"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_METRO_01 { get { return new Scanner("s_m_y_cop_01_black_full_02\\REPORT_SUSPECT_ENTERED_METRO_01"); } }
        public static Scanner REPORT_SUSPECT_IS_IN_CAR_01 { get { return new Scanner("s_m_y_cop_01_black_full_02\\REPORT_SUSPECT_IS_IN_CAR_01"); } }
        public static Scanner REPORT_SUSPECT_IS_IN_CAR_02 { get { return new Scanner("s_m_y_cop_01_black_full_02\\REPORT_SUSPECT_IS_IN_CAR_02"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_BIKE_01 { get { return new Scanner("s_m_y_cop_01_black_full_02\\REPORT_SUSPECT_IS_ON_BIKE_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_01 { get { return new Scanner("s_m_y_cop_01_black_full_02\\REPORT_SUSPECT_IS_ON_FOOT_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_02 { get { return new Scanner("s_m_y_cop_01_black_full_02\\REPORT_SUSPECT_IS_ON_FOOT_02"); } }
        public static Scanner REPORT_SUSPECT_LEFT_FREEWAY_01 { get { return new Scanner("s_m_y_cop_01_black_full_02\\REPORT_SUSPECT_LEFT_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_LEFT_FREEWAY_02 { get { return new Scanner("s_m_y_cop_01_black_full_02\\REPORT_SUSPECT_LEFT_FREEWAY_02"); } }
        public static Scanner REQUEST_BACKUP_01 { get { return new Scanner("s_m_y_cop_01_black_full_02\\REQUEST_BACKUP_01"); } }
        public static Scanner REQUEST_BACKUP_02 { get { return new Scanner("s_m_y_cop_01_black_full_02\\REQUEST_BACKUP_02"); } }
        public static Scanner REQUEST_GUIDANCE_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_full_02\\REQUEST_GUIDANCE_DISPATCH_01"); } }
        public static Scanner REQUEST_GUIDANCE_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_full_02\\REQUEST_GUIDANCE_DISPATCH_02"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_full_02\\UNIT_RESPONDING_DISPATCH_01"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_full_02\\UNIT_RESPONDING_DISPATCH_02"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_03 { get { return new Scanner("s_m_y_cop_01_black_full_02\\UNIT_RESPONDING_DISPATCH_03"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_04 { get { return new Scanner("s_m_y_cop_01_black_full_02\\UNIT_RESPONDING_DISPATCH_04"); } }
    }
    public class s_m_y_cop_01_black_mini_01
    {
        public static Scanner HELI_APPROACHING_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\HELI_APPROACHING_DISPATCH_01"); } }
        public static Scanner HELI_APPROACHING_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\HELI_APPROACHING_DISPATCH_02"); } }
        public static Scanner HELI_MAYDAY_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\HELI_MAYDAY_DISPATCH_01"); } }
        public static Scanner HELI_MAYDAY_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\HELI_MAYDAY_DISPATCH_02"); } }
        public static Scanner HELI_NO_VISUAL_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\HELI_NO_VISUAL_DISPATCH_01"); } }
        public static Scanner HELI_NO_VISUAL_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\HELI_NO_VISUAL_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_EAST_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\HELI_VISUAL_HEADING_EAST_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_EAST_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\HELI_VISUAL_HEADING_EAST_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_NORTH_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\HELI_VISUAL_HEADING_NORTH_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_NORTH_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\HELI_VISUAL_HEADING_NORTH_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_SOUTH_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\HELI_VISUAL_HEADING_SOUTH_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_SOUTH_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\HELI_VISUAL_HEADING_SOUTH_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_WEST_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\HELI_VISUAL_HEADING_WEST_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_WEST_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\HELI_VISUAL_HEADING_WEST_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_ON_FOOT_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\HELI_VISUAL_ON_FOOT_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_ON_FOOT_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\HELI_VISUAL_ON_FOOT_DISPATCH_02"); } }
        public static Scanner REPORT_SUSPECT_CRASHED_VEHICLE_01 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\REPORT_SUSPECT_CRASHED_VEHICLE_01"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_01 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\REPORT_SUSPECT_ENTERED_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_02 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\REPORT_SUSPECT_ENTERED_FREEWAY_02"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_METRO_01 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\REPORT_SUSPECT_ENTERED_METRO_01"); } }
        public static Scanner REPORT_SUSPECT_IS_IN_CAR_01 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\REPORT_SUSPECT_IS_IN_CAR_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_BIKE_01 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\REPORT_SUSPECT_IS_ON_BIKE_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_01 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\REPORT_SUSPECT_IS_ON_FOOT_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_02 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\REPORT_SUSPECT_IS_ON_FOOT_02"); } }
        public static Scanner REPORT_SUSPECT_LEFT_FREEWAY_01 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\REPORT_SUSPECT_LEFT_FREEWAY_01"); } }
        public static Scanner REQUEST_BACKUP_01 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\REQUEST_BACKUP_01"); } }
        public static Scanner REQUEST_GUIDANCE_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\REQUEST_GUIDANCE_DISPATCH_01"); } }
        public static Scanner REQUEST_GUIDANCE_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\REQUEST_GUIDANCE_DISPATCH_02"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\UNIT_RESPONDING_DISPATCH_01"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\UNIT_RESPONDING_DISPATCH_02"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_03 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\UNIT_RESPONDING_DISPATCH_03"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_04 { get { return new Scanner("s_m_y_cop_01_black_mini_01\\UNIT_RESPONDING_DISPATCH_04"); } }
    }
    public class s_m_y_cop_01_black_mini_02
    {
        public static Scanner HELI_APPROACHING_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\HELI_APPROACHING_DISPATCH_01"); } }
        public static Scanner HELI_APPROACHING_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\HELI_APPROACHING_DISPATCH_02"); } }
        public static Scanner HELI_MAYDAY_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\HELI_MAYDAY_DISPATCH_01"); } }
        public static Scanner HELI_MAYDAY_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\HELI_MAYDAY_DISPATCH_02"); } }
        public static Scanner HELI_NO_VISUAL_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\HELI_NO_VISUAL_DISPATCH_01"); } }
        public static Scanner HELI_NO_VISUAL_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\HELI_NO_VISUAL_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_EAST_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\HELI_VISUAL_HEADING_EAST_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_EAST_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\HELI_VISUAL_HEADING_EAST_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_NORTH_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\HELI_VISUAL_HEADING_NORTH_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_NORTH_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\HELI_VISUAL_HEADING_NORTH_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_SOUTH_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\HELI_VISUAL_HEADING_SOUTH_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_SOUTH_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\HELI_VISUAL_HEADING_SOUTH_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_WEST_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\HELI_VISUAL_HEADING_WEST_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_WEST_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\HELI_VISUAL_HEADING_WEST_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_ON_FOOT_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\HELI_VISUAL_ON_FOOT_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_ON_FOOT_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\HELI_VISUAL_ON_FOOT_DISPATCH_02"); } }
        public static Scanner REPORT_SUSPECT_CRASHED_VEHICLE_01 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\REPORT_SUSPECT_CRASHED_VEHICLE_01"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_01 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\REPORT_SUSPECT_ENTERED_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_02 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\REPORT_SUSPECT_ENTERED_FREEWAY_02"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_METRO_01 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\REPORT_SUSPECT_ENTERED_METRO_01"); } }
        public static Scanner REPORT_SUSPECT_IS_IN_CAR_01 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\REPORT_SUSPECT_IS_IN_CAR_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_BIKE_01 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\REPORT_SUSPECT_IS_ON_BIKE_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_01 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\REPORT_SUSPECT_IS_ON_FOOT_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_02 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\REPORT_SUSPECT_IS_ON_FOOT_02"); } }
        public static Scanner REPORT_SUSPECT_LEFT_FREEWAY_01 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\REPORT_SUSPECT_LEFT_FREEWAY_01"); } }
        public static Scanner REQUEST_BACKUP_01 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\REQUEST_BACKUP_01"); } }
        public static Scanner REQUEST_GUIDANCE_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\REQUEST_GUIDANCE_DISPATCH_01"); } }
        public static Scanner REQUEST_GUIDANCE_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\REQUEST_GUIDANCE_DISPATCH_02"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\UNIT_RESPONDING_DISPATCH_01"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\UNIT_RESPONDING_DISPATCH_02"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_03 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\UNIT_RESPONDING_DISPATCH_03"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_04 { get { return new Scanner("s_m_y_cop_01_black_mini_02\\UNIT_RESPONDING_DISPATCH_04"); } }
    }
    public class s_m_y_cop_01_black_mini_03
    {
        public static Scanner HELI_APPROACHING_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\HELI_APPROACHING_DISPATCH_01"); } }
        public static Scanner HELI_APPROACHING_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\HELI_APPROACHING_DISPATCH_02"); } }
        public static Scanner HELI_MAYDAY_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\HELI_MAYDAY_DISPATCH_01"); } }
        public static Scanner HELI_MAYDAY_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\HELI_MAYDAY_DISPATCH_02"); } }
        public static Scanner HELI_NO_VISUAL_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\HELI_NO_VISUAL_DISPATCH_01"); } }
        public static Scanner HELI_NO_VISUAL_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\HELI_NO_VISUAL_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_EAST_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\HELI_VISUAL_HEADING_EAST_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_EAST_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\HELI_VISUAL_HEADING_EAST_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_NORTH_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\HELI_VISUAL_HEADING_NORTH_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_NORTH_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\HELI_VISUAL_HEADING_NORTH_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_SOUTH_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\HELI_VISUAL_HEADING_SOUTH_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_SOUTH_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\HELI_VISUAL_HEADING_SOUTH_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_WEST_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\HELI_VISUAL_HEADING_WEST_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_WEST_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\HELI_VISUAL_HEADING_WEST_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_ON_FOOT_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\HELI_VISUAL_ON_FOOT_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_ON_FOOT_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\HELI_VISUAL_ON_FOOT_DISPATCH_02"); } }
        public static Scanner REPORT_SUSPECT_CRASHED_VEHICLE_01 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\REPORT_SUSPECT_CRASHED_VEHICLE_01"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_01 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\REPORT_SUSPECT_ENTERED_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_02 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\REPORT_SUSPECT_ENTERED_FREEWAY_02"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_METRO_01 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\REPORT_SUSPECT_ENTERED_METRO_01"); } }
        public static Scanner REPORT_SUSPECT_IS_IN_CAR_01 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\REPORT_SUSPECT_IS_IN_CAR_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_BIKE_01 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\REPORT_SUSPECT_IS_ON_BIKE_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_01 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\REPORT_SUSPECT_IS_ON_FOOT_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_02 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\REPORT_SUSPECT_IS_ON_FOOT_02"); } }
        public static Scanner REPORT_SUSPECT_LEFT_FREEWAY_01 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\REPORT_SUSPECT_LEFT_FREEWAY_01"); } }
        public static Scanner REQUEST_BACKUP_01 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\REQUEST_BACKUP_01"); } }
        public static Scanner REQUEST_GUIDANCE_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\REQUEST_GUIDANCE_DISPATCH_01"); } }
        public static Scanner REQUEST_GUIDANCE_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\REQUEST_GUIDANCE_DISPATCH_02"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\UNIT_RESPONDING_DISPATCH_01"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\UNIT_RESPONDING_DISPATCH_02"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_03 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\UNIT_RESPONDING_DISPATCH_03"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_04 { get { return new Scanner("s_m_y_cop_01_black_mini_03\\UNIT_RESPONDING_DISPATCH_04"); } }
    }
    public class s_m_y_cop_01_black_mini_04
    {
        public static Scanner HELI_APPROACHING_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\HELI_APPROACHING_DISPATCH_01"); } }
        public static Scanner HELI_APPROACHING_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\HELI_APPROACHING_DISPATCH_02"); } }
        public static Scanner HELI_MAYDAY_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\HELI_MAYDAY_DISPATCH_01"); } }
        public static Scanner HELI_MAYDAY_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\HELI_MAYDAY_DISPATCH_02"); } }
        public static Scanner HELI_NO_VISUAL_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\HELI_NO_VISUAL_DISPATCH_01"); } }
        public static Scanner HELI_NO_VISUAL_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\HELI_NO_VISUAL_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_EAST_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\HELI_VISUAL_HEADING_EAST_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_EAST_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\HELI_VISUAL_HEADING_EAST_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_NORTH_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\HELI_VISUAL_HEADING_NORTH_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_NORTH_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\HELI_VISUAL_HEADING_NORTH_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_SOUTH_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\HELI_VISUAL_HEADING_SOUTH_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_SOUTH_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\HELI_VISUAL_HEADING_SOUTH_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_WEST_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\HELI_VISUAL_HEADING_WEST_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_WEST_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\HELI_VISUAL_HEADING_WEST_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_ON_FOOT_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\HELI_VISUAL_ON_FOOT_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_ON_FOOT_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\HELI_VISUAL_ON_FOOT_DISPATCH_02"); } }
        public static Scanner REPORT_SUSPECT_CRASHED_VEHICLE_01 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\REPORT_SUSPECT_CRASHED_VEHICLE_01"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_01 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\REPORT_SUSPECT_ENTERED_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_02 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\REPORT_SUSPECT_ENTERED_FREEWAY_02"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_METRO_01 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\REPORT_SUSPECT_ENTERED_METRO_01"); } }
        public static Scanner REPORT_SUSPECT_IS_IN_CAR_01 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\REPORT_SUSPECT_IS_IN_CAR_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_BIKE_01 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\REPORT_SUSPECT_IS_ON_BIKE_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_01 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\REPORT_SUSPECT_IS_ON_FOOT_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_02 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\REPORT_SUSPECT_IS_ON_FOOT_02"); } }
        public static Scanner REPORT_SUSPECT_LEFT_FREEWAY_01 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\REPORT_SUSPECT_LEFT_FREEWAY_01"); } }
        public static Scanner REQUEST_BACKUP_01 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\REQUEST_BACKUP_01"); } }
        public static Scanner REQUEST_GUIDANCE_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\REQUEST_GUIDANCE_DISPATCH_01"); } }
        public static Scanner REQUEST_GUIDANCE_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\REQUEST_GUIDANCE_DISPATCH_02"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\UNIT_RESPONDING_DISPATCH_01"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\UNIT_RESPONDING_DISPATCH_02"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_03 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\UNIT_RESPONDING_DISPATCH_03"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_04 { get { return new Scanner("s_m_y_cop_01_black_mini_04\\UNIT_RESPONDING_DISPATCH_04"); } }
    }
    public class s_m_y_cop_01_white_full_01
    {
        public static Scanner HELI_APPROACHING_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_full_01\\HELI_APPROACHING_DISPATCH_01"); } }
        public static Scanner HELI_APPROACHING_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_full_01\\HELI_APPROACHING_DISPATCH_02"); } }
        public static Scanner HELI_MAYDAY_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_full_01\\HELI_MAYDAY_DISPATCH_01"); } }
        public static Scanner HELI_MAYDAY_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_full_01\\HELI_MAYDAY_DISPATCH_02"); } }
        public static Scanner HELI_NO_VISUAL_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_full_01\\HELI_NO_VISUAL_DISPATCH_01"); } }
        public static Scanner HELI_NO_VISUAL_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_full_01\\HELI_NO_VISUAL_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_EAST_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_full_01\\HELI_VISUAL_HEADING_EAST_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_EAST_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_full_01\\HELI_VISUAL_HEADING_EAST_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_NORTH_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_full_01\\HELI_VISUAL_HEADING_NORTH_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_NORTH_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_full_01\\HELI_VISUAL_HEADING_NORTH_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_SOUTH_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_full_01\\HELI_VISUAL_HEADING_SOUTH_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_SOUTH_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_full_01\\HELI_VISUAL_HEADING_SOUTH_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_WEST_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_full_01\\HELI_VISUAL_HEADING_WEST_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_WEST_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_full_01\\HELI_VISUAL_HEADING_WEST_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_ON_FOOT_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_full_01\\HELI_VISUAL_ON_FOOT_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_ON_FOOT_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_full_01\\HELI_VISUAL_ON_FOOT_DISPATCH_02"); } }
        public static Scanner REPORT_SUSPECT_CRASHED_VEHICLE_01 { get { return new Scanner("s_m_y_cop_01_white_full_01\\REPORT_SUSPECT_CRASHED_VEHICLE_01"); } }
        public static Scanner REPORT_SUSPECT_CRASHED_VEHICLE_02 { get { return new Scanner("s_m_y_cop_01_white_full_01\\REPORT_SUSPECT_CRASHED_VEHICLE_02"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_01 { get { return new Scanner("s_m_y_cop_01_white_full_01\\REPORT_SUSPECT_ENTERED_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_02 { get { return new Scanner("s_m_y_cop_01_white_full_01\\REPORT_SUSPECT_ENTERED_FREEWAY_02"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_METRO_01 { get { return new Scanner("s_m_y_cop_01_white_full_01\\REPORT_SUSPECT_ENTERED_METRO_01"); } }
        public static Scanner REPORT_SUSPECT_IS_IN_CAR_01 { get { return new Scanner("s_m_y_cop_01_white_full_01\\REPORT_SUSPECT_IS_IN_CAR_01"); } }
        public static Scanner REPORT_SUSPECT_IS_IN_CAR_02 { get { return new Scanner("s_m_y_cop_01_white_full_01\\REPORT_SUSPECT_IS_IN_CAR_02"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_BIKE_01 { get { return new Scanner("s_m_y_cop_01_white_full_01\\REPORT_SUSPECT_IS_ON_BIKE_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_01 { get { return new Scanner("s_m_y_cop_01_white_full_01\\REPORT_SUSPECT_IS_ON_FOOT_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_02 { get { return new Scanner("s_m_y_cop_01_white_full_01\\REPORT_SUSPECT_IS_ON_FOOT_02"); } }
        public static Scanner REPORT_SUSPECT_LEFT_FREEWAY_01 { get { return new Scanner("s_m_y_cop_01_white_full_01\\REPORT_SUSPECT_LEFT_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_LEFT_FREEWAY_02 { get { return new Scanner("s_m_y_cop_01_white_full_01\\REPORT_SUSPECT_LEFT_FREEWAY_02"); } }
        public static Scanner REPORT_SUSPECT_SPOTTED_01 { get { return new Scanner("s_m_y_cop_01_white_full_01\\REPORT_SUSPECT_SPOTTED_01"); } }
        public static Scanner REPORT_SUSPECT_SPOTTED_02 { get { return new Scanner("s_m_y_cop_01_white_full_01\\REPORT_SUSPECT_SPOTTED_02"); } }
        public static Scanner REQUEST_BACKUP_01 { get { return new Scanner("s_m_y_cop_01_white_full_01\\REQUEST_BACKUP_01"); } }
        public static Scanner REQUEST_BACKUP_02 { get { return new Scanner("s_m_y_cop_01_white_full_01\\REQUEST_BACKUP_02"); } }
        public static Scanner REQUEST_GUIDANCE_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_full_01\\REQUEST_GUIDANCE_DISPATCH_01"); } }
        public static Scanner REQUEST_GUIDANCE_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_full_01\\REQUEST_GUIDANCE_DISPATCH_02"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_full_01\\UNIT_RESPONDING_DISPATCH_01"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_full_01\\UNIT_RESPONDING_DISPATCH_02"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_03 { get { return new Scanner("s_m_y_cop_01_white_full_01\\UNIT_RESPONDING_DISPATCH_03"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_04 { get { return new Scanner("s_m_y_cop_01_white_full_01\\UNIT_RESPONDING_DISPATCH_04"); } }
    }
    public class s_m_y_cop_01_white_full_02
    {
        public static Scanner HELI_APPROACHING_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_full_02\\HELI_APPROACHING_DISPATCH_01"); } }
        public static Scanner HELI_APPROACHING_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_full_02\\HELI_APPROACHING_DISPATCH_02"); } }
        public static Scanner HELI_MAYDAY_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_full_02\\HELI_MAYDAY_DISPATCH_01"); } }
        public static Scanner HELI_MAYDAY_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_full_02\\HELI_MAYDAY_DISPATCH_02"); } }
        public static Scanner HELI_NO_VISUAL_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_full_02\\HELI_NO_VISUAL_DISPATCH_01"); } }
        public static Scanner HELI_NO_VISUAL_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_full_02\\HELI_NO_VISUAL_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_EAST_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_full_02\\HELI_VISUAL_HEADING_EAST_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_EAST_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_full_02\\HELI_VISUAL_HEADING_EAST_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_NORTH_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_full_02\\HELI_VISUAL_HEADING_NORTH_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_NORTH_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_full_02\\HELI_VISUAL_HEADING_NORTH_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_SOUTH_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_full_02\\HELI_VISUAL_HEADING_SOUTH_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_SOUTH_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_full_02\\HELI_VISUAL_HEADING_SOUTH_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_WEST_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_full_02\\HELI_VISUAL_HEADING_WEST_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_WEST_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_full_02\\HELI_VISUAL_HEADING_WEST_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_ON_FOOT_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_full_02\\HELI_VISUAL_ON_FOOT_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_ON_FOOT_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_full_02\\HELI_VISUAL_ON_FOOT_DISPATCH_02"); } }
        public static Scanner REPORT_SUSPECT_CRASHED_VEHICLE_01 { get { return new Scanner("s_m_y_cop_01_white_full_02\\REPORT_SUSPECT_CRASHED_VEHICLE_01"); } }
        public static Scanner REPORT_SUSPECT_CRASHED_VEHICLE_02 { get { return new Scanner("s_m_y_cop_01_white_full_02\\REPORT_SUSPECT_CRASHED_VEHICLE_02"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_01 { get { return new Scanner("s_m_y_cop_01_white_full_02\\REPORT_SUSPECT_ENTERED_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_02 { get { return new Scanner("s_m_y_cop_01_white_full_02\\REPORT_SUSPECT_ENTERED_FREEWAY_02"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_METRO_01 { get { return new Scanner("s_m_y_cop_01_white_full_02\\REPORT_SUSPECT_ENTERED_METRO_01"); } }
        public static Scanner REPORT_SUSPECT_IS_IN_CAR_01 { get { return new Scanner("s_m_y_cop_01_white_full_02\\REPORT_SUSPECT_IS_IN_CAR_01"); } }
        public static Scanner REPORT_SUSPECT_IS_IN_CAR_02 { get { return new Scanner("s_m_y_cop_01_white_full_02\\REPORT_SUSPECT_IS_IN_CAR_02"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_BIKE_01 { get { return new Scanner("s_m_y_cop_01_white_full_02\\REPORT_SUSPECT_IS_ON_BIKE_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_01 { get { return new Scanner("s_m_y_cop_01_white_full_02\\REPORT_SUSPECT_IS_ON_FOOT_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_02 { get { return new Scanner("s_m_y_cop_01_white_full_02\\REPORT_SUSPECT_IS_ON_FOOT_02"); } }
        public static Scanner REPORT_SUSPECT_LEFT_FREEWAY_01 { get { return new Scanner("s_m_y_cop_01_white_full_02\\REPORT_SUSPECT_LEFT_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_LEFT_FREEWAY_02 { get { return new Scanner("s_m_y_cop_01_white_full_02\\REPORT_SUSPECT_LEFT_FREEWAY_02"); } }
        public static Scanner REPORT_SUSPECT_SPOTTED_01 { get { return new Scanner("s_m_y_cop_01_white_full_02\\REPORT_SUSPECT_SPOTTED_01"); } }
        public static Scanner REPORT_SUSPECT_SPOTTED_02 { get { return new Scanner("s_m_y_cop_01_white_full_02\\REPORT_SUSPECT_SPOTTED_02"); } }
        public static Scanner REQUEST_BACKUP_01 { get { return new Scanner("s_m_y_cop_01_white_full_02\\REQUEST_BACKUP_01"); } }
        public static Scanner REQUEST_BACKUP_02 { get { return new Scanner("s_m_y_cop_01_white_full_02\\REQUEST_BACKUP_02"); } }
        public static Scanner REQUEST_GUIDANCE_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_full_02\\REQUEST_GUIDANCE_DISPATCH_01"); } }
        public static Scanner REQUEST_GUIDANCE_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_full_02\\REQUEST_GUIDANCE_DISPATCH_02"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_full_02\\UNIT_RESPONDING_DISPATCH_01"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_full_02\\UNIT_RESPONDING_DISPATCH_02"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_03 { get { return new Scanner("s_m_y_cop_01_white_full_02\\UNIT_RESPONDING_DISPATCH_03"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_04 { get { return new Scanner("s_m_y_cop_01_white_full_02\\UNIT_RESPONDING_DISPATCH_04"); } }
    }
    public class s_m_y_cop_01_white_mini_0
    {
        public static Scanner REPORT_SUSPECT_CRASHED_VEHICLE_01 { get { return new Scanner("s_m_y_cop_01_white_mini_01\\REPORT_SUSPECT_CRASHED_VEHICLE_01"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_02 { get { return new Scanner("s_m_y_cop_01_white_mini_01\\REPORT_SUSPECT_ENTERED_FREEWAY_02"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_METRO_01 { get { return new Scanner("s_m_y_cop_01_white_mini_01\\REPORT_SUSPECT_ENTERED_METRO_01"); } }
        public static Scanner REPORT_SUSPECT_IS_IN_CAR_01 { get { return new Scanner("s_m_y_cop_01_white_mini_01\\REPORT_SUSPECT_IS_IN_CAR_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_BIKE_01 { get { return new Scanner("s_m_y_cop_01_white_mini_01\\REPORT_SUSPECT_IS_ON_BIKE_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_02 { get { return new Scanner("s_m_y_cop_01_white_mini_01\\REPORT_SUSPECT_IS_ON_FOOT_02"); } }
        public static Scanner REPORT_SUSPECT_LEFT_FREEWAY_01 { get { return new Scanner("s_m_y_cop_01_white_mini_01\\REPORT_SUSPECT_LEFT_FREEWAY_01"); } }
        public static Scanner REQUEST_BACKUP_01 { get { return new Scanner("s_m_y_cop_01_white_mini_01\\REQUEST_BACKUP_01"); } }
    }
    public class s_m_y_cop_01_white_mini_02
    {
        public static Scanner HELI_APPROACHING_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\HELI_APPROACHING_DISPATCH_01"); } }
        public static Scanner HELI_APPROACHING_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\HELI_APPROACHING_DISPATCH_02"); } }
        public static Scanner HELI_MAYDAY_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\HELI_MAYDAY_DISPATCH_01"); } }
        public static Scanner HELI_MAYDAY_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\HELI_MAYDAY_DISPATCH_02"); } }
        public static Scanner HELI_NO_VISUAL_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\HELI_NO_VISUAL_DISPATCH_01"); } }
        public static Scanner HELI_NO_VISUAL_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\HELI_NO_VISUAL_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_EAST_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\HELI_VISUAL_HEADING_EAST_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_EAST_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\HELI_VISUAL_HEADING_EAST_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_NORTH_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\HELI_VISUAL_HEADING_NORTH_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_NORTH_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\HELI_VISUAL_HEADING_NORTH_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_SOUTH_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\HELI_VISUAL_HEADING_SOUTH_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_SOUTH_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\HELI_VISUAL_HEADING_SOUTH_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_WEST_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\HELI_VISUAL_HEADING_WEST_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_WEST_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\HELI_VISUAL_HEADING_WEST_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_ON_FOOT_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\HELI_VISUAL_ON_FOOT_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_ON_FOOT_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\HELI_VISUAL_ON_FOOT_DISPATCH_02"); } }
        public static Scanner REPORT_SUSPECT_CRASHED_VEHICLE_01 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\REPORT_SUSPECT_CRASHED_VEHICLE_01"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_01 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\REPORT_SUSPECT_ENTERED_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_02 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\REPORT_SUSPECT_ENTERED_FREEWAY_02"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_METRO_01 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\REPORT_SUSPECT_ENTERED_METRO_01"); } }
        public static Scanner REPORT_SUSPECT_IS_IN_CAR_01 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\REPORT_SUSPECT_IS_IN_CAR_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_BIKE_01 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\REPORT_SUSPECT_IS_ON_BIKE_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_01 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\REPORT_SUSPECT_IS_ON_FOOT_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_02 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\REPORT_SUSPECT_IS_ON_FOOT_02"); } }
        public static Scanner REPORT_SUSPECT_LEFT_FREEWAY_01 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\REPORT_SUSPECT_LEFT_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_SPOTTED_01 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\REPORT_SUSPECT_SPOTTED_01"); } }
        public static Scanner REPORT_SUSPECT_SPOTTED_02 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\REPORT_SUSPECT_SPOTTED_02"); } }
        public static Scanner REQUEST_BACKUP_01 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\REQUEST_BACKUP_01"); } }
        public static Scanner REQUEST_GUIDANCE_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\REQUEST_GUIDANCE_DISPATCH_01"); } }
        public static Scanner REQUEST_GUIDANCE_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\REQUEST_GUIDANCE_DISPATCH_02"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\UNIT_RESPONDING_DISPATCH_01"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\UNIT_RESPONDING_DISPATCH_02"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_03 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\UNIT_RESPONDING_DISPATCH_03"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_04 { get { return new Scanner("s_m_y_cop_01_white_mini_02\\UNIT_RESPONDING_DISPATCH_04"); } }
    }
    public class s_m_y_cop_01_white_mini_03
    {
        public static Scanner HELI_APPROACHING_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\HELI_APPROACHING_DISPATCH_01"); } }
        public static Scanner HELI_APPROACHING_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\HELI_APPROACHING_DISPATCH_02"); } }
        public static Scanner HELI_MAYDAY_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\HELI_MAYDAY_DISPATCH_01"); } }
        public static Scanner HELI_MAYDAY_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\HELI_MAYDAY_DISPATCH_02"); } }
        public static Scanner HELI_NO_VISUAL_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\HELI_NO_VISUAL_DISPATCH_01"); } }
        public static Scanner HELI_NO_VISUAL_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\HELI_NO_VISUAL_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_EAST_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\HELI_VISUAL_HEADING_EAST_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_EAST_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\HELI_VISUAL_HEADING_EAST_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_NORTH_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\HELI_VISUAL_HEADING_NORTH_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_NORTH_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\HELI_VISUAL_HEADING_NORTH_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_SOUTH_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\HELI_VISUAL_HEADING_SOUTH_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_SOUTH_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\HELI_VISUAL_HEADING_SOUTH_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_WEST_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\HELI_VISUAL_HEADING_WEST_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_WEST_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\HELI_VISUAL_HEADING_WEST_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_ON_FOOT_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\HELI_VISUAL_ON_FOOT_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_ON_FOOT_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\HELI_VISUAL_ON_FOOT_DISPATCH_02"); } }
        public static Scanner REPORT_SUSPECT_CRASHED_VEHICLE_01 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\REPORT_SUSPECT_CRASHED_VEHICLE_01"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_01 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\REPORT_SUSPECT_ENTERED_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_02 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\REPORT_SUSPECT_ENTERED_FREEWAY_02"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_METRO_01 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\REPORT_SUSPECT_ENTERED_METRO_01"); } }
        public static Scanner REPORT_SUSPECT_IS_IN_CAR_01 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\REPORT_SUSPECT_IS_IN_CAR_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_BIKE_01 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\REPORT_SUSPECT_IS_ON_BIKE_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_01 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\REPORT_SUSPECT_IS_ON_FOOT_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_02 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\REPORT_SUSPECT_IS_ON_FOOT_02"); } }
        public static Scanner REPORT_SUSPECT_LEFT_FREEWAY_01 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\REPORT_SUSPECT_LEFT_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_SPOTTED_01 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\REPORT_SUSPECT_SPOTTED_01"); } }
        public static Scanner REPORT_SUSPECT_SPOTTED_02 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\REPORT_SUSPECT_SPOTTED_02"); } }
        public static Scanner REQUEST_BACKUP_01 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\REQUEST_BACKUP_01"); } }
        public static Scanner REQUEST_GUIDANCE_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\REQUEST_GUIDANCE_DISPATCH_01"); } }
        public static Scanner REQUEST_GUIDANCE_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\REQUEST_GUIDANCE_DISPATCH_02"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\UNIT_RESPONDING_DISPATCH_01"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\UNIT_RESPONDING_DISPATCH_02"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_03 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\UNIT_RESPONDING_DISPATCH_03"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_04 { get { return new Scanner("s_m_y_cop_01_white_mini_03\\UNIT_RESPONDING_DISPATCH_04"); } }
    }
    public class s_m_y_cop_01_white_mini_04
    {
        public static Scanner HELI_APPROACHING_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\HELI_APPROACHING_DISPATCH_01"); } }
        public static Scanner HELI_APPROACHING_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\HELI_APPROACHING_DISPATCH_02"); } }
        public static Scanner HELI_MAYDAY_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\HELI_MAYDAY_DISPATCH_01"); } }
        public static Scanner HELI_MAYDAY_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\HELI_MAYDAY_DISPATCH_02"); } }
        public static Scanner HELI_NO_VISUAL_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\HELI_NO_VISUAL_DISPATCH_01"); } }
        public static Scanner HELI_NO_VISUAL_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\HELI_NO_VISUAL_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_EAST_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\HELI_VISUAL_HEADING_EAST_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_EAST_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\HELI_VISUAL_HEADING_EAST_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_NORTH_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\HELI_VISUAL_HEADING_NORTH_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_NORTH_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\HELI_VISUAL_HEADING_NORTH_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_SOUTH_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\HELI_VISUAL_HEADING_SOUTH_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_SOUTH_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\HELI_VISUAL_HEADING_SOUTH_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_HEADING_WEST_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\HELI_VISUAL_HEADING_WEST_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_HEADING_WEST_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\HELI_VISUAL_HEADING_WEST_DISPATCH_02"); } }
        public static Scanner HELI_VISUAL_ON_FOOT_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\HELI_VISUAL_ON_FOOT_DISPATCH_01"); } }
        public static Scanner HELI_VISUAL_ON_FOOT_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\HELI_VISUAL_ON_FOOT_DISPATCH_02"); } }
        public static Scanner REPORT_SUSPECT_CRASHED_VEHICLE_01 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\REPORT_SUSPECT_CRASHED_VEHICLE_01"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_01 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\REPORT_SUSPECT_ENTERED_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_FREEWAY_02 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\REPORT_SUSPECT_ENTERED_FREEWAY_02"); } }
        public static Scanner REPORT_SUSPECT_ENTERED_METRO_01 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\REPORT_SUSPECT_ENTERED_METRO_01"); } }
        public static Scanner REPORT_SUSPECT_IS_IN_CAR_01 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\REPORT_SUSPECT_IS_IN_CAR_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_BIKE_01 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\REPORT_SUSPECT_IS_ON_BIKE_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_01 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\REPORT_SUSPECT_IS_ON_FOOT_01"); } }
        public static Scanner REPORT_SUSPECT_IS_ON_FOOT_02 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\REPORT_SUSPECT_IS_ON_FOOT_02"); } }
        public static Scanner REPORT_SUSPECT_LEFT_FREEWAY_01 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\REPORT_SUSPECT_LEFT_FREEWAY_01"); } }
        public static Scanner REPORT_SUSPECT_SPOTTED_01 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\REPORT_SUSPECT_SPOTTED_01"); } }
        public static Scanner REPORT_SUSPECT_SPOTTED_02 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\REPORT_SUSPECT_SPOTTED_02"); } }
        public static Scanner REQUEST_BACKUP_01 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\REQUEST_BACKUP_01"); } }
        public static Scanner REQUEST_GUIDANCE_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\REQUEST_GUIDANCE_DISPATCH_01"); } }
        public static Scanner REQUEST_GUIDANCE_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\REQUEST_GUIDANCE_DISPATCH_02"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_01 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\UNIT_RESPONDING_DISPATCH_01"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_02 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\UNIT_RESPONDING_DISPATCH_02"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_03 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\UNIT_RESPONDING_DISPATCH_03"); } }
        public static Scanner UNIT_RESPONDING_DISPATCH_04 { get { return new Scanner("s_m_y_cop_01_white_mini_04\\UNIT_RESPONDING_DISPATCH_04"); } }
    }
    public class UnitsRespond
    {
        public static Scanner UNITS_RESPOND_CODE_02_01 { get { return new Scanner("UNITS_RESPOND\\UNITS_RESPOND_CODE_02_01"); } }
        public static Scanner UNITS_RESPOND_CODE_02_02 { get { return new Scanner("UNITS_RESPOND\\UNITS_RESPOND_CODE_02_02"); } }
        public static Scanner UNITS_RESPOND_CODE_03_01 { get { return new Scanner("UNITS_RESPOND\\UNITS_RESPOND_CODE_03_01"); } }
        public static Scanner UNITS_RESPOND_CODE_03_02 { get { return new Scanner("UNITS_RESPOND\\UNITS_RESPOND_CODE_03_02"); } }
        public static Scanner UNITS_RESPOND_CODE_99_01 { get { return new Scanner("UNITS_RESPOND\\UNITS_RESPOND_CODE_99_01"); } }
        public static Scanner UNITS_RESPOND_CODE_99_02 { get { return new Scanner("UNITS_RESPOND\\UNITS_RESPOND_CODE_99_02"); } }
        public static Scanner UNITS_RESPOND_CODE_99_03 { get { return new Scanner("UNITS_RESPOND\\UNITS_RESPOND_CODE_99_03"); } }
    }
    public class VehicleCategory
    {
        public static Scanner VEHICLE_CATEGORY_AMBULANCE_01 { get { return new Scanner("VEHICLE_CATEGORY\\VEHICLE_CATEGORY_AMBULANCE_01"); } }
        public static Scanner VEHICLE_CATEGORY_BICYCLE_01 { get { return new Scanner("VEHICLE_CATEGORY\\VEHICLE_CATEGORY_BICYCLE_01"); } }
        public static Scanner VEHICLE_CATEGORY_BOAT_01 { get { return new Scanner("VEHICLE_CATEGORY\\VEHICLE_CATEGORY_BOAT_01"); } }
        public static Scanner VEHICLE_CATEGORY_COUPE_01 { get { return new Scanner("VEHICLE_CATEGORY\\VEHICLE_CATEGORY_COUPE_01"); } }
        public static Scanner VEHICLE_CATEGORY_COUPE_02 { get { return new Scanner("VEHICLE_CATEGORY\\VEHICLE_CATEGORY_COUPE_02"); } }
        public static Scanner VEHICLE_CATEGORY_FIRETRUCK_01 { get { return new Scanner("VEHICLE_CATEGORY\\VEHICLE_CATEGORY_FIRETRUCK_01"); } }
        public static Scanner VEHICLE_CATEGORY_HATCHBACK_01 { get { return new Scanner("VEHICLE_CATEGORY\\VEHICLE_CATEGORY_HATCHBACK_01"); } }
        public static Scanner VEHICLE_CATEGORY_HELICOPTER_01 { get { return new Scanner("VEHICLE_CATEGORY\\VEHICLE_CATEGORY_HELICOPTER_01"); } }
        public static Scanner VEHICLE_CATEGORY_INDUSTRIAL_VEHICLE_01 { get { return new Scanner("VEHICLE_CATEGORY\\VEHICLE_CATEGORY_INDUSTRIAL_VEHICLE_01"); } }
        public static Scanner VEHICLE_CATEGORY_MILITARY_VEHICLE_01 { get { return new Scanner("VEHICLE_CATEGORY\\VEHICLE_CATEGORY_MILITARY_VEHICLE_01"); } }
        public static Scanner VEHICLE_CATEGORY_MOTORCYCLE_01 { get { return new Scanner("VEHICLE_CATEGORY\\VEHICLE_CATEGORY_MOTORCYCLE_01"); } }
        public static Scanner VEHICLE_CATEGORY_MUSCLE_CAR_01 { get { return new Scanner("VEHICLE_CATEGORY\\VEHICLE_CATEGORY_MUSCLE_CAR_01"); } }
        public static Scanner VEHICLE_CATEGORY_OFF_ROAD_VEHICLE_01 { get { return new Scanner("VEHICLE_CATEGORY\\VEHICLE_CATEGORY_OFF_ROAD_VEHICLE_01"); } }
        public static Scanner VEHICLE_CATEGORY_PERFORMANCE_CAR_01 { get { return new Scanner("VEHICLE_CATEGORY\\VEHICLE_CATEGORY_PERFORMANCE_CAR_01"); } }
        public static Scanner VEHICLE_CATEGORY_POLICE_CAR_01 { get { return new Scanner("VEHICLE_CATEGORY\\VEHICLE_CATEGORY_POLICE_CAR_01"); } }
        public static Scanner VEHICLE_CATEGORY_SEDAN_01 { get { return new Scanner("VEHICLE_CATEGORY\\VEHICLE_CATEGORY_SEDAN_01"); } }
        public static Scanner VEHICLE_CATEGORY_SERVICE_VEHICLE_01 { get { return new Scanner("VEHICLE_CATEGORY\\VEHICLE_CATEGORY_SERVICE_VEHICLE_01"); } }
        public static Scanner VEHICLE_CATEGORY_SPORTS_CAR_01 { get { return new Scanner("VEHICLE_CATEGORY\\VEHICLE_CATEGORY_SPORTS_CAR_01"); } }
        public static Scanner VEHICLE_CATEGORY_SUV_01 { get { return new Scanner("VEHICLE_CATEGORY\\VEHICLE_CATEGORY_SUV_01"); } }
        public static Scanner VEHICLE_CATEGORY_SUV_02 { get { return new Scanner("VEHICLE_CATEGORY\\VEHICLE_CATEGORY_SUV_02"); } }
        public static Scanner VEHICLE_CATEGORY_TRAIN_01 { get { return new Scanner("VEHICLE_CATEGORY\\VEHICLE_CATEGORY_TRAIN_01"); } }
        public static Scanner VEHICLE_CATEGORY_TRUCK_01 { get { return new Scanner("VEHICLE_CATEGORY\\VEHICLE_CATEGORY_TRUCK_01"); } }
        public static Scanner VEHICLE_CATEGORY_UTILITY_VEHICLE_01 { get { return new Scanner("VEHICLE_CATEGORY\\VEHICLE_CATEGORY_UTILITY_VEHICLE_01"); } }
        public static Scanner VEHICLE_CATEGORY_VAN_01 { get { return new Scanner("VEHICLE_CATEGORY\\VEHICLE_CATEGORY_VAN_01"); } }
    }
    public class WeHave
    {
        public static String CitizensReportRandom()
        {
            int rnd = random.Next(1, 4);
            if (rnd == 1)
                return Scanner.WeHave.CITIZENS_REPORT_01.Value;
            else if (rnd == 2)
                return Scanner.WeHave.CITIZENS_REPORT_02.Value;
            else if (rnd == 3)
                return Scanner.WeHave.CITIZENS_REPORT_03.Value;
            else
                return Scanner.WeHave.CITIZENS_REPORT_04.Value;
        }
        public static String OfficersReportRandom()
        {
            int rnd = random.Next(1, 4);
            if (rnd == 1)
                return Scanner.WeHave.OFFICERS_REPORT_01.Value;
            else if (rnd == 2)
                return Scanner.WeHave.OFFICERS_REPORT_02.Value;
            else
                return Scanner.WeHave.OFFICERS_REPORT_03.Value;
        }
        public static Scanner CITIZENS_REPORT_01 { get { return new Scanner("WE_HAVE\\CITIZENS_REPORT_01"); } }
        public static Scanner CITIZENS_REPORT_02 { get { return new Scanner("WE_HAVE\\CITIZENS_REPORT_02"); } }
        public static Scanner CITIZENS_REPORT_03 { get { return new Scanner("WE_HAVE\\CITIZENS_REPORT_03"); } }
        public static Scanner CITIZENS_REPORT_04 { get { return new Scanner("WE_HAVE\\CITIZENS_REPORT_04"); } }
        public static Scanner OFFICERS_REPORT_01 { get { return new Scanner("WE_HAVE\\OFFICERS_REPORT_01"); } }
        public static Scanner OFFICERS_REPORT_02 { get { return new Scanner("WE_HAVE\\OFFICERS_REPORT_02"); } }
        public static Scanner OFFICERS_REPORT_03 { get { return new Scanner("WE_HAVE\\OFFICERS_REPORT_03"); } }
        public static Scanner WE_HAVE_01 { get { return new Scanner("WE_HAVE\\WE_HAVE_01"); } }
        public static Scanner WE_HAVE_02 { get { return new Scanner("WE_HAVE\\WE_HAVE_02"); } }
    }
}