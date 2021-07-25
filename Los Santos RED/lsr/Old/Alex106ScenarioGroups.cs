using LosSantosRED.lsr.Helper;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Alex106ScenarioGroups
{
    //ALL this is from Alex 106 https://www.gta5-mods.com/scripts/scenario-groups
    //i dont want to install SHV.NET :(
    public bool LoadMPMap { get; set; } = false;
    public void Setup()
    {
        SetScenarios(true);
        LoadInteriors();
    }
    public void Dispose()
    {
        SetScenarios(false);

        LoadInteriors();
    }
    private void SetScenarios(bool enabled)
    {
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("ammunation", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("countryside_banks", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("fort_zancudo_guards", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("guards_at_prison", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("paleto_bank", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("paleto_cops", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("police_at_court", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("police_pound1", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("police_pound2", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("police_pound3", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("police_pound4", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("police_pound5", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("prison_towers", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("prison_transport", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("sandy_cops", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("scrap_security", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("vagos_hangout", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("mp_police2", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("lost_hangout", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("facility_cannon", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("facility_main_1", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("facility_main_2", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("facility_main_3", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("club_cypress_flats_warehouse", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("club_del_perro_building", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("club_elysian_island_warehouse", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("club_la_mesa_warehouse", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("club_lsia_warehouse", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("club_mission_row_building", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("club_strawberry_warehouse", enabled);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("club_west_vinewood_building", enabled);
    }
    private void LoadInteriors()
    {
        
        NativeHelper.RemoveIPL("hei_bi_hw1_13_door");
        NativeHelper.RequestIPL("bkr_bi_hw1_13_int");
        int bkr_biker_dlc_int_03 = NativeFunction.Natives.GET_INTERIOR_AT_COORDS<int>(984.1553f, -95.36626f, 75.9326f);
        NativeFunction.Natives.SET_INTERIOR_ACTIVE(bkr_biker_dlc_int_03, true);
        NativeFunction.Natives.DISABLE_INTERIOR(bkr_biker_dlc_int_03, false);
        NativeHelper.RequestIPL("ba_mpbattleipl");
        NativeHelper.RequestIPL("ba_mpbattleipl_long_0");
        NativeHelper.RequestIPL("ba_mpbattleipl_strm_0");
        if (NativeFunction.Natives.IS_SCENARIO_GROUP_ENABLED<bool>("club_cypress_flats_warehouse"))
        {
            NativeHelper.RequestIPL("ba_barriers_case4");
        }
        if (NativeFunction.Natives.IS_SCENARIO_GROUP_ENABLED<bool>("club_del_perro_building"))
        {
            NativeHelper.RequestIPL("ba_barriers_case5");
        }
        if (NativeFunction.Natives.IS_SCENARIO_GROUP_ENABLED<bool>("club_vinewood_dt"))
        {
            NativeHelper.RequestIPL("ba_barriers_case8");
        }
        if (NativeFunction.Natives.IS_SCENARIO_GROUP_ENABLED<bool>("club_elysian_island_warehouse"))
        {
            NativeHelper.RequestIPL("ba_barriers_case7");
        }
        if (NativeFunction.Natives.IS_SCENARIO_GROUP_ENABLED<bool>("club_la_mesa_warehouse"))
        {
            NativeHelper.RequestIPL("ba_barriers_case0");
        }
        if (NativeFunction.Natives.IS_SCENARIO_GROUP_ENABLED<bool>("club_lsia_warehouse"))
        {
            NativeHelper.RequestIPL("ba_barriers_case6");
        }
        if (NativeFunction.Natives.IS_SCENARIO_GROUP_ENABLED<bool>("club_mission_row_building"))
        {
            NativeHelper.RequestIPL("ba_barriers_case1");
        }
        if (NativeFunction.Natives.IS_SCENARIO_GROUP_ENABLED<bool>("club_strawberry_warehouse"))
        {
            NativeHelper.RequestIPL("ba_barriers_case2");
        }
        if (NativeFunction.Natives.IS_SCENARIO_GROUP_ENABLED<bool>("club_vespucci_canals_building"))
        {
            NativeHelper.RequestIPL("ba_barriers_case9");
        }
        if (NativeFunction.Natives.IS_SCENARIO_GROUP_ENABLED<bool>("club_west_vinewood_building"))
        {
            NativeHelper.RequestIPL("ba_barriers_case3");
        }
        NativeHelper.RequestIPL("hei_dlc_casino_aircon");
        NativeHelper.RequestIPL("hei_dlc_casino_aircon_lod");
        NativeHelper.RequestIPL("hei_dlc_casino_door");
        NativeHelper.RequestIPL("hei_dlc_casino_door_lod");
        NativeHelper.RequestIPL("hei_dlc_vw_roofdoors_locked");
        NativeHelper.RequestIPL("hei_dlc_windows_casino");
        NativeHelper.RequestIPL("hei_dlc_windows_casino_lod");
        NativeHelper.RequestIPL("vw_ch3_additions");
        NativeHelper.RequestIPL("vw_ch3_additions_long_0");
        NativeHelper.RequestIPL("vw_ch3_additions_strm_0");
        NativeHelper.RequestIPL("vw_dlc_casino_door");
        NativeHelper.RequestIPL("vw_dlc_casino_door_lod");

        //Pillbox hill hospital?
        NativeFunction.CallByName<bool>("REMOVE_IPL", "RC12B_Destroyed");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "RC12B_HospitalInterior");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "RC12B_Default");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "RC12B_Fixed");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "RC12B_Default");//state 1 normal

        //Lifeinvader
        NativeFunction.CallByName<bool>("REQUEST_IPL", "facelobby");  // lifeinvader
        NativeFunction.CallByName<bool>("REMOVE_IPL", "facelobbyfake");
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -340230128, -1042.518f, -240.6915f, 38.11796f, true, 0.0f, 0.0f, -1.0f);//_DOOR_CONTROL

        //    FIB Lobby      
        NativeFunction.CallByName<bool>("REQUEST_IPL", "FIBlobby");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "FIBlobbyfake");
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -1517873911, 106.3793f, -742.6982f, 46.51962f, false, 0.0f, 0.0f, 0.0f);
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -90456267, 105.7607f, -746.646f, 46.18266f, false, 0.0f, 0.0f, 0.0f);

        //Paleto Sheriff Office
        NativeFunction.CallByName<bool>("DISABLE_INTERIOR", NativeFunction.CallByName<int>("GET_INTERIOR_AT_COORDS", -444.89068603515625f, 6013.5869140625f, 30.7164f), false);
        NativeFunction.CallByName<bool>("CAP_INTERIOR", NativeFunction.CallByName<int>("GET_INTERIOR_AT_COORDS", -444.89068603515625f, 6013.5869140625f, 30.7164f), false);
        NativeFunction.CallByName<bool>("REQUEST_IPL", "v_sheriff2");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "cs1_16_sheriff_cap");
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -1501157055, -444.4985f, 6017.06f, 31.86633f, false, 0.0f, 0.0f, 0.0f);
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -1501157055, -442.66f, 6015.222f, 31.86633f, false, 0.0f, 0.0f, 0.0f);

        //Sheriffs Office Sandy Shores
        NativeFunction.CallByName<bool>("DISABLE_INTERIOR", NativeFunction.CallByName<int>("GET_INTERIOR_AT_COORDS", 1854.2537841796875f, 3686.738525390625f, 33.2671012878418f), false);
        NativeFunction.CallByName<bool>("CAP_INTERIOR", NativeFunction.CallByName<bool>("GET_INTERIOR_AT_COORDS", 1854.2537841796875f, 3686.738525390625f, 33.2671012878418f), false);
        NativeFunction.CallByName<bool>("REQUEST_IPL", "v_sheriff");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "sheriff_cap");
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -1765048490, 1855.685f, 3683.93f, 34.59282f, false, 0.0f, 0.0f, 0.0f);

        //    Tequila la       
        NativeFunction.CallByName<bool>("DISABLE_INTERIOR", NativeFunction.CallByName<bool>("GET_INTERIOR_AT_COORDS", -556.5089111328125f, 286.318115234375f, 81.1763f), false);
        NativeFunction.CallByName<bool>("CAP_INTERIOR", NativeFunction.CallByName<bool>("GET_INTERIOR_AT_COORDS", -556.5089111328125f, 286.318115234375f, 81.1763f), false);
        NativeFunction.CallByName<bool>("REQUEST_IPL", "v_rockclub");
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, 993120320, -565.1712f, 276.6259f, 83.28626f, false, 0.0f, 0.0f, 0.0f);// front door
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, 993120320, -561.2866f, 293.5044f, 87.77851f, false, 0.0f, 0.0f, 0.0f);// back door



    }
}

