using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CayoPericoAirport : Airport
{
    private List<string> cayoIPLs = new List<string>()
        {
            "h4_islandairstrip",
            "h4_islandairstrip_props",
            "h4_islandx_mansion",
            "h4_islandx_mansion_props",
            "h4_islandx_props",
            "h4_islandxdock",
            "h4_islandxdock_props",
            "h4_islandxdock_props_2",
            "h4_islandxtower",
            "h4_islandx_maindock",
            "h4_islandx_maindock_props",
            "h4_islandx_maindock_props_2",
            "h4_IslandX_Mansion_Vault",
            "h4_islandairstrip_propsb",
            "h4_beach",
            "h4_beach_props",
            "h4_beach_bar_props",
            "h4_islandx_barrack_props",
            "h4_islandx_checkpoint",
            "h4_islandx_checkpoint_props",
            "h4_islandx_Mansion_Office",
            "h4_islandx_Mansion_LockUp_01",
            "h4_islandx_Mansion_LockUp_02",
            "h4_islandx_Mansion_LockUp_03",
            "h4_islandairstrip_hangar_props",
            "h4_IslandX_Mansion_B",
            "h4_islandairstrip_doorsclosed",
            "h4_Underwater_Gate_Closed",
            "h4_mansion_gate_closed",
            "h4_aa_guns",
            "h4_IslandX_Mansion_GuardFence",
            "h4_IslandX_Mansion_Entrance_Fence",
            "h4_IslandX_Mansion_B_Side_Fence",
            "h4_IslandX_Mansion_Lights",
            "h4_islandxcanal_props",
            "h4_beach_props_party",
            "h4_islandX_Terrain_props_06_a",
            "h4_islandX_Terrain_props_06_b",
            "h4_islandX_Terrain_props_06_c",
            "h4_islandX_Terrain_props_05_a",
            "h4_islandX_Terrain_props_05_b",
            "h4_islandX_Terrain_props_05_c",
            "h4_islandX_Terrain_props_05_d",
            "h4_islandX_Terrain_props_05_e",
            "h4_islandX_Terrain_props_05_f",
            "H4_islandx_terrain_01",
            "H4_islandx_terrain_02",
            "H4_islandx_terrain_03",
            "H4_islandx_terrain_04",
            "H4_islandx_terrain_05",
            "H4_islandx_terrain_06",
            "h4_ne_ipl_00",
            "h4_ne_ipl_01",
            "h4_ne_ipl_02",
            "h4_ne_ipl_03",
            "h4_ne_ipl_04",
            "h4_ne_ipl_05",
            "h4_ne_ipl_06",
            "h4_ne_ipl_07",
            "h4_ne_ipl_08",
            "h4_ne_ipl_09",
            "h4_nw_ipl_00",
            "h4_nw_ipl_01",
            "h4_nw_ipl_02",
            "h4_nw_ipl_03",
            "h4_nw_ipl_04",
            "h4_nw_ipl_05",
            "h4_nw_ipl_06",
            "h4_nw_ipl_07",
            "h4_nw_ipl_08",
            "h4_nw_ipl_09",
            "h4_se_ipl_00",
            "h4_se_ipl_01",
            "h4_se_ipl_02",
            "h4_se_ipl_03",
            "h4_se_ipl_04",
            "h4_se_ipl_05",
            "h4_se_ipl_06",
            "h4_se_ipl_07",
            "h4_se_ipl_08",
            "h4_se_ipl_09",
            "h4_sw_ipl_00",
            "h4_sw_ipl_01",
            "h4_sw_ipl_02",
            "h4_sw_ipl_03",
            "h4_sw_ipl_04",
            "h4_sw_ipl_05",
            "h4_sw_ipl_06",
            "h4_sw_ipl_07",
            "h4_sw_ipl_08",
            "h4_sw_ipl_09",
            "h4_islandx_mansion",
            "h4_islandxtower_veg",
            "h4_islandx_sea_mines",
            "h4_islandx",
            "h4_islandx_barrack_hatch",
            "h4_islandxdock_water_hatch",
            "h4_beach_party",

        };
    public CayoPericoAirport(string airportID, Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(airportID, _EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public CayoPericoAirport()
    {

    }

    public override void OnDepart(ILocationInteractable Player)
    {
        DisableCayo();
        ////////NativeFunction.Natives.SET_ISLAND_ENABLED("HeistIsland", false);
        ////////NativeFunction.Natives.SET_USE_ISLAND_MAP(false);
        ////////NativeFunction.Natives.SET_ALLOW_STREAM_HEIST_ISLAND_NODES(false);
        ////////NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Heist_Island_Peds", false);
        ////////NativeFunction.Natives.SET_AMBIENT_ZONE_STATE_PERSISTENT("AZL_DLC_Hei4_Island_Zones", false, false);
        ////////NativeFunction.Natives.SET_AMBIENT_ZONE_STATE_PERSISTENT("AZL_DLC_Hei4_Island_Disabled_Zones", true, false);
        ////////NativeFunction.Natives.SET_ALLOW_STREAM_HEIST_ISLAND_NODES(false);
        ///
        Player.CurrentLocation.ClearFakeZone();

        base.OnDepart(Player);
    }
    public override void OnArrive(ILocationInteractable Player, bool setPos)
    {
        EnableCayo();
        ////////NativeFunction.Natives.SET_ISLAND_ENABLED("HeistIsland", true);
        ////////NativeFunction.Natives.SET_USE_ISLAND_MAP(true);
        ////////NativeFunction.Natives.SET_ALLOW_STREAM_HEIST_ISLAND_NODES(true);
        ////////NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Heist_Island_Peds", true);
        ////////NativeFunction.Natives.SET_AMBIENT_ZONE_STATE_PERSISTENT("AZL_DLC_Hei4_Island_Zones", true, true);
        ////////NativeFunction.Natives.SET_AMBIENT_ZONE_STATE_PERSISTENT("AZL_DLC_Hei4_Island_Disabled_Zones", false, true);
        ////////NativeFunction.Natives.SET_ALLOW_STREAM_HEIST_ISLAND_NODES(true);
        //////////int zone = NativeFunction.Natives.GET_ZONE_FROM_NAME_ID<int>("IsHeist");
        //////////NativeFunction.Natives.SET_ZONE_ENABLED(zone, 1);
        ///
        Player.CurrentLocation.SetFakeZone("IsHeist");

        base.OnArrive(Player,setPos);
    }

    private void DisableCayo()
    {
        foreach (string removeIPL in cayoIPLs)
        {
            NativeFunction.Natives.REMOVE_IPL(removeIPL);
        }
        NativeFunction.Natives.SET_ISLAND_ENABLED("HeistIsland", false);
        NativeFunction.Natives.SET_USE_ISLAND_MAP(false);
        NativeFunction.Natives.SET_ALLOW_STREAM_HEIST_ISLAND_NODES(false);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Heist_Island_Peds", false);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Heist_Island_Peds_2", false);
        NativeFunction.Natives.SET_AMBIENT_ZONE_STATE_PERSISTENT("AZL_DLC_Hei4_Island_Zones", false, false);
        NativeFunction.Natives.SET_AMBIENT_ZONE_STATE_PERSISTENT("AZL_DLC_Hei4_Island_Disabled_Zones", true, false);
        NativeFunction.Natives.SET_ALLOW_STREAM_HEIST_ISLAND_NODES(false);
        int zone = NativeFunction.Natives.GET_ZONE_FROM_NAME_ID<int>("IsHeist");
        NativeFunction.Natives.SET_ZONE_ENABLED(zone, false);
    }
    private void EnableCayo()
    {
        foreach (string requestIPL in cayoIPLs)
        {
            NativeFunction.Natives.REQUEST_IPL(requestIPL);
        }
        NativeFunction.Natives.SET_ISLAND_ENABLED("HeistIsland", true);
        NativeFunction.Natives.SET_USE_ISLAND_MAP(true);
        NativeFunction.Natives.SET_ALLOW_STREAM_HEIST_ISLAND_NODES(true);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Heist_Island_Peds", true);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Heist_Island_Peds_2", true);
        NativeFunction.Natives.SET_AMBIENT_ZONE_STATE_PERSISTENT("AZL_DLC_Hei4_Island_Zones", true, true);
        NativeFunction.Natives.SET_AMBIENT_ZONE_STATE_PERSISTENT("AZL_DLC_Hei4_Island_Disabled_Zones", false, true);
        NativeFunction.Natives.SET_ALLOW_STREAM_HEIST_ISLAND_NODES(true);
        int zone = NativeFunction.Natives.GET_ZONE_FROM_NAME_ID<int>("IsHeist");
        NativeFunction.Natives.SET_ZONE_ENABLED(zone, 1);
    }
}

