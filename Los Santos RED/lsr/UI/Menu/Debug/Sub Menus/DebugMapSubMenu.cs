using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.Activity;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Xml.Serialization;

public class DebugMapSubMenu : DebugSubMenu
{
    private bool IsBigMapActive;
    private IEntityProvideable World;
    private IPlacesOfInterest PlacesOfInterest;
    private ISettingsProvideable Settings;
    private ITimeControllable Time;
    private IPoliceRespondable PoliceRespondable;
    private ModDataFileManager ModDataFileManager;
    private IGangs Gangs;
    private UIMenu MapMenuItem;
    private UIMenuItem DisableLS;

    public DebugMapSubMenu(UIMenu debug, MenuPool menuPool, IActionable player, IEntityProvideable world, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, ITimeControllable time, IPoliceRespondable policeRespondable, ModDataFileManager modDataFileManager, IGangs gangs) : base(debug, menuPool, player)
    {
        World = world;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        Time = time;
        PoliceRespondable = policeRespondable;
        ModDataFileManager = modDataFileManager;
        Gangs = gangs;
    }
    public override void AddItems()
    {
        MapMenuItem = MenuPool.AddSubMenu(Debug, "Map Menu");
        MapMenuItem.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Various map items";
        CreateGeneralItems();
        CreateLSItems();
        CreateSunShineDreamItems();
        CreateLCItems();
        CreateCayoPericoItems();
    }
    private void CreateGeneralItems()
    {
        UIMenu GeneralSubMenu = MenuPool.AddSubMenu(MapMenuItem, "General Menu");
        GeneralSubMenu.SetBannerType(EntryPoint.LSRedColor);


        UIMenuItem SetBigMap = new UIMenuItem("Toggle Big MiniMap", "Toggles the big GTAO style mini map");
        SetBigMap.Activated += (menu, item) =>
        {
            IsBigMapActive = !IsBigMapActive;
            NativeFunction.Natives.SET_BIGMAP_ACTIVE(IsBigMapActive, false);
            Game.DisplaySubtitle($"IsBigMapActive:{IsBigMapActive}");
            menu.Visible = false;
        };
        GeneralSubMenu.AddItem(SetBigMap);//CreateDisableLSItem()
        UIMenuItem RevealMap = new UIMenuItem("Reveal Map", "Reveal the enitre map and remove fog of war");
        RevealMap.Activated += (menu, item) =>
        {
            NativeFunction.Natives.SET_MINIMAP_HIDE_FOW(true);
            menu.Visible = false;
        };
        GeneralSubMenu.AddItem(RevealMap);


        GeneralSubMenu.AddItem(CreateDisableLSItem());

        UIMenuItem LoadSPMap = new UIMenuItem("Load SP Map", "Loads the SP map if you have the MP map enabled");
        LoadSPMap.Activated += (menu, item) =>
        {
            World.LoadSPMap();
            menu.Visible = false;
        };
        UIMenuItem LoadMPMap = new UIMenuItem("Load MP Map", "Load the MP map if you have the SP map enabled");
        LoadMPMap.Activated += (menu, item) =>
        {
            World.LoadMPMap();
            menu.Visible = false;
        };
        GeneralSubMenu.AddItem(LoadSPMap);
        GeneralSubMenu.AddItem(LoadMPMap);


        UIMenuItem CheckMapLoaded = new UIMenuItem("Loaded Map", "Display the current loaded map (MP/SP)");
        CheckMapLoaded.Activated += (menu, item) =>
        {
            string iplName = "bkr_bi_hw1_13_int";
            NativeFunction.Natives.REQUEST_IPL(iplName);
            GameFiber.Sleep(500);
            Game.DisplaySubtitle(NativeFunction.Natives.IS_IPL_ACTIVE<bool>(iplName) ? "MP Map" : "SP Map");
            menu.Visible = false;
        };
        GeneralSubMenu.AddItem(CheckMapLoaded);
    }
    private void CreateLSItems()
    {
        UIMenu LSSubMenu = MenuPool.AddSubMenu(MapMenuItem, "Los Santos Menu");
        LSSubMenu.SetBannerType(EntryPoint.LSRedColor);
        LSSubMenu.AddItem(CreateDisableLSItem());
    }
    private void CreateSunShineDreamItems()
    {
        UIMenu SunShineDreamSubMenu = MenuPool.AddSubMenu(MapMenuItem, "Sunshine Dream Menu");
        SunShineDreamSubMenu.SetBannerType(EntryPoint.LSRedColor);
        SunShineDreamSubMenu.AddItem(CreateDisableLSItem());
    }
    private void CreateLCItems()
    {
        UIMenu LibertyCitySubMenu = MenuPool.AddSubMenu(MapMenuItem, "Liberty City Menu");
        LibertyCitySubMenu.SetBannerType(EntryPoint.LSRedColor);
        UIMenuItem SetLCSettingsMenu = new UIMenuItem("Set LC Active", "Disable LS ymaps, scenarios, set the settings, and set a mission flag for LC.");
        SetLCSettingsMenu.Activated += (menu, item) =>
        {
            menu.Visible = false;
            DisableLosSantosYMAPS();
            SetLCSettings();
        };
        LibertyCitySubMenu.AddItem(SetLCSettingsMenu);
    }
    private void CreateCayoPericoItems()
    {
        UIMenu CayoPericoSubMenu = MenuPool.AddSubMenu(MapMenuItem, "Cayo Perico Menu");
        CayoPericoSubMenu.SetBannerType(EntryPoint.LSRedColor);
        List<string> cayoIPLs = new List<string>()
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
        UIMenuItem TurnOffCayo = new UIMenuItem("Turn Off Cayo", "Turn Off Cayo Perico");
        TurnOffCayo.Activated += (menu, item) =>
        {
            menu.Visible = false;
            foreach (string removeIPL in cayoIPLs)
            {
                NativeFunction.Natives.REMOVE_IPL(removeIPL);
            }
            NativeFunction.Natives.SET_ISLAND_ENABLED("HeistIsland", false);
            NativeFunction.Natives.SET_USE_ISLAND_MAP(false);
            NativeFunction.Natives.SET_ALLOW_STREAM_HEIST_ISLAND_NODES(false);
            NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Heist_Island_Peds", false);
            NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Heist_Island_Peds_2", false);
            //NativeFunction.Natives.SET_AMBIENT_ZONE_STATE_PERSISTENT("AZL_DLC_Hei4_Island_Zones", false, false);
            //NativeFunction.Natives.SET_AMBIENT_ZONE_STATE_PERSISTENT("AZL_DLC_Hei4_Island_Disabled_Zones", true, false);
            NativeFunction.Natives.SET_ALLOW_STREAM_HEIST_ISLAND_NODES(false);
            int zone = NativeFunction.Natives.GET_ZONE_FROM_NAME_ID<int>("IsHeist");
            NativeFunction.Natives.SET_ZONE_ENABLED(zone, false);
            //NativeFunction.Natives.LOAD_GLOBAL_WATER_FILE(0);
        };
        UIMenuItem TurnOnCayo = new UIMenuItem("Turn On Cayo", "Turn On Cayo Perico");
        TurnOnCayo.Activated += (menu, item) =>
        {
            menu.Visible = false;
            foreach (string requestIPL in cayoIPLs)
            {
                NativeFunction.Natives.REQUEST_IPL(requestIPL);
            }
            NativeFunction.Natives.SET_ISLAND_ENABLED("HeistIsland", true);
            NativeFunction.Natives.SET_USE_ISLAND_MAP(true);
            NativeFunction.Natives.SET_ALLOW_STREAM_HEIST_ISLAND_NODES(true);
            NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Heist_Island_Peds", true);
            NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Heist_Island_Peds_2", true);
            //NativeFunction.Natives.SET_AMBIENT_ZONE_STATE_PERSISTENT("AZL_DLC_Hei4_Island_Zones", true, true);
            //NativeFunction.Natives.SET_AMBIENT_ZONE_STATE_PERSISTENT("AZL_DLC_Hei4_Island_Disabled_Zones", false, true);
            NativeFunction.Natives.SET_ALLOW_STREAM_HEIST_ISLAND_NODES(true);
            int zone = NativeFunction.Natives.GET_ZONE_FROM_NAME_ID<int>("IsHeist");
            NativeFunction.Natives.SET_ZONE_ENABLED(zone, 1);
            //NativeFunction.Natives.LOAD_GLOBAL_WATER_FILE(1);
            Player.Character.Position = new Vector3(4524.132f, -4498.074f, 4.23596f);
        };
        UIMenuItem TurnOffCayo1 = new UIMenuItem("Turn Off Cayo ALT", "Turn Off Cayo Perico");
        TurnOffCayo1.Activated += (menu, item) =>
        {
            menu.Visible = false;
            NativeFunction.Natives.x9A9D1BA639675CF1("HeistIsland", false);
            NativeFunction.Natives.xF74B1FFA4A15FBEA(false);
            //NativeFunction.Natives.xDD3D5F9CA0C715D0(false);
            NativeFunction.Natives.x5E1460624D194A38(false);

        };
        UIMenuItem TurnOnCayo1 = new UIMenuItem("Turn On Cayo ALT", "Turn On Cayo Perico");
        TurnOnCayo1.Activated += (menu, item) =>
        {
            menu.Visible = false;
            NativeFunction.Natives.x9A9D1BA639675CF1("HeistIsland", true);
            NativeFunction.Natives.xF74B1FFA4A15FBEA(true);
            //NativeFunction.Natives.xDD3D5F9CA0C715D0(true);
            NativeFunction.Natives.x5E1460624D194A38(true);
            Player.Character.Position = new Vector3(4524.132f, -4498.074f, 4.23596f);

        };
        CayoPericoSubMenu.AddItem(TurnOffCayo);
        CayoPericoSubMenu.AddItem(TurnOnCayo);
        CayoPericoSubMenu.AddItem(TurnOffCayo1);
        CayoPericoSubMenu.AddItem(TurnOnCayo1);
    }
    private void DisableLosSantosYMAPS()
    {
        Game.DisplaySubtitle("Disabling LS");
        LSMapDisabler lSMapDisabler = new LSMapDisabler();
        lSMapDisabler.DisableLS();
        GameFiber.Sleep(500);
        Game.DisplaySubtitle("LS Disabled");
    }
    private void SetLCSettings()
    {
        Settings.SetLC();
        Game.DisplaySubtitle("Set LC Settings");
        NativeFunction.Natives.SET_MISSION_FLAG(true);
    }
    private UIMenuItem CreateDisableLSItem()
    {
        UIMenuItem DisableLS = new UIMenuItem("Disable LS", "Disable ALL LS IPLs. Useful for travelling to some far away places. CANNOT UNDO WITHOUT GAME RESTART!");
        DisableLS.Activated += (menu, item) =>
        {
            menu.Visible = false;
            DisableLosSantosYMAPS();
        };
        return DisableLS;
    }
}

