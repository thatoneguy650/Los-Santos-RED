using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Mod;
using Rage;
using Rage.Native;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static DispatchScannerFiles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

[Serializable()]
public class GameConfig
{
    public GameConfig()
    {
    }
    public GameConfig(string configName,string suffix)
    {
        ConfigName = configName;
        AgenciesConfig = suffix;
        CellphonesConfig = suffix;
        ContactsConfig = suffix;
        CountyJurisdictionsConfig = suffix;
        CraftableItemsConfig = suffix;
        CrimesConfig = suffix;
        DancesConfig = suffix;
        DispatchablePeopleConfig = suffix;
        DispatchableVehiclesConfig = suffix;
        GangsConfig = suffix;
        GangTerritoriesConfig = suffix;
        GesturesConfig = suffix;
        HeadsConfig = suffix;
        InteriorsConfig = suffix;
        IntoxicantsConfig = suffix;
        IssueableWeaponsConfig = suffix;
        LocationsConfig = suffix;
        LocationTypesConfig = suffix;
        ModItemsConfig = suffix;
        NamesConfig = suffix;
        OrganizationsConfig = suffix;
        PedGroupsConfig = suffix;
        PhysicalitemsConfig = suffix;
        PlateTypesConfig = suffix;
        SavedOutfitsConfig = suffix;
        SaveGamesConfig = suffix;
        SettingsConfig = suffix;
        ShopMenusConfig = suffix;
        SpawnBlocksConfig = suffix;
        SpeechesConfig = suffix;
        StreetsConfig = suffix;
        WantedLevelsConfig = suffix;
        WeaponsConfig = suffix;
        ZoneJurisdictionsConfig = suffix;
        ZonesConfig = suffix;
    }
    public GameConfig(string suffix)
    {
        ConfigName = suffix;
        AgenciesConfig = suffix;
        CellphonesConfig = suffix;
        ContactsConfig = suffix;
        CountyJurisdictionsConfig = suffix;
        CraftableItemsConfig = suffix;
        CrimesConfig = suffix;
        DancesConfig = suffix;
        DispatchablePeopleConfig = suffix;
        DispatchableVehiclesConfig = suffix;
        GangsConfig = suffix;
        GangTerritoriesConfig = suffix;
        GesturesConfig = suffix;
        HeadsConfig = suffix;
        InteriorsConfig = suffix;
        IntoxicantsConfig = suffix;
        IssueableWeaponsConfig = suffix;
        LocationsConfig = suffix;
        LocationTypesConfig = suffix;
        ModItemsConfig = suffix;
        NamesConfig = suffix;
        OrganizationsConfig = suffix;
        PedGroupsConfig = suffix;
        PhysicalitemsConfig = suffix;
        PlateTypesConfig = suffix;
        SavedOutfitsConfig = suffix;
        SaveGamesConfig = suffix;
        SettingsConfig = suffix;
        ShopMenusConfig = suffix;
        SpawnBlocksConfig = suffix;
        SpeechesConfig = suffix;
        StreetsConfig = suffix;
        WantedLevelsConfig = suffix;
        WeaponsConfig = suffix;
        ZoneJurisdictionsConfig = suffix;
        ZonesConfig = suffix;
    }
    public GameConfig(
        string configName, string agencies, string cellphones, string contacts, string countyJurisdictions,
        string craftableItems, string crimes, string dances, string dispatchablePeople, string dispatchableVehicles,
        string gangs, string gangTerritories, string gestures, string heads, string interiors,
        string intoxicants, string issueableWeapons, string locations, string locationTypes,
        string modItems, string names, string organizations, string pedGroups, string physicalitems,
        string plateTypes, string savedOutfits, string saveGames, string settings, string shopMenus,
        string spawnBlocks, string speeches, string streets, string wantedLevels, string weapons,
        string zoneJurisdictions, string zones)
    {
        ConfigName = configName;
        AgenciesConfig = agencies;
        CellphonesConfig = cellphones;
        ContactsConfig = contacts;
        CountyJurisdictionsConfig = countyJurisdictions;
        CraftableItemsConfig = craftableItems;
        CrimesConfig = crimes;
        DancesConfig = dances;
        DispatchablePeopleConfig = dispatchablePeople;
        DispatchableVehiclesConfig = dispatchableVehicles;
        GangsConfig = gangs;
        GangTerritoriesConfig = gangTerritories;
        GesturesConfig = gestures;
        HeadsConfig = heads;
        InteriorsConfig = interiors;
        IntoxicantsConfig = intoxicants;
        IssueableWeaponsConfig = issueableWeapons;
        LocationsConfig = locations;
        LocationTypesConfig = locationTypes;
        ModItemsConfig = modItems;
        NamesConfig = names;
        OrganizationsConfig = organizations;
        PedGroupsConfig = pedGroups;
        PhysicalitemsConfig = physicalitems;
        PlateTypesConfig = plateTypes;
        SavedOutfitsConfig = savedOutfits;
        SaveGamesConfig = saveGames;
        SettingsConfig = settings;
        ShopMenusConfig = shopMenus;
        SpawnBlocksConfig = spawnBlocks;
        SpeechesConfig = speeches;
        StreetsConfig = streets;
        WantedLevelsConfig = wantedLevels;
        WeaponsConfig = weapons;
        ZoneJurisdictionsConfig = zoneJurisdictions;
        ZonesConfig = zones;
    }

    public string ConfigName { get; set; }
    public string AgenciesConfig { get; set; }
    public string CellphonesConfig { get; set; }
    public string ContactsConfig { get; set; }
    public string CountyJurisdictionsConfig { get; set; }
    public string CraftableItemsConfig { get; set; }
    public string CrimesConfig { get; set; }
    public string DancesConfig { get; set; }
    public string DispatchablePeopleConfig { get; set; }
    public string DispatchableVehiclesConfig { get; set; }
    public string GangsConfig { get; set; }
    public string GangTerritoriesConfig { get; set; }
    public string GesturesConfig { get; set; }
    public string HeadsConfig { get; set; }
    public string InteriorsConfig { get; set; }
    public string IntoxicantsConfig { get; set; }
    public string IssueableWeaponsConfig { get; set; }
    public string LocationsConfig { get; set; }
    public string LocationTypesConfig { get; set; }
    public string ModItemsConfig { get; set; }
    public string NamesConfig { get; set; }
    public string OrganizationsConfig { get; set; }
    public string PedGroupsConfig { get; set; }
    public string PhysicalitemsConfig { get; set; }
    public string PlateTypesConfig { get; set; }
    public string SavedOutfitsConfig { get; set; }
    public string SaveGamesConfig { get; set; }
    public string SettingsConfig { get; set; }
    public string ShopMenusConfig { get; set; }
    public string SpawnBlocksConfig { get; set; }
    public string SpeechesConfig { get; set; }
    public string StreetsConfig { get; set; }
    public string WantedLevelsConfig { get; set; }
    public string WeaponsConfig { get; set; }
    public string ZoneJurisdictionsConfig { get; set; }
    public string ZonesConfig { get; set; }
    public List<string> toList()
    {
        return new List<string>
        {
            AgenciesConfig,
            CellphonesConfig,
            ContactsConfig,
            CountyJurisdictionsConfig,
            CraftableItemsConfig,
            CrimesConfig,
            DancesConfig,
            DispatchablePeopleConfig,
            DispatchableVehiclesConfig,
            GangsConfig,
            GangTerritoriesConfig,
            GesturesConfig,
            HeadsConfig,
            InteriorsConfig,
            IntoxicantsConfig,
            IssueableWeaponsConfig,
            LocationsConfig,
            LocationTypesConfig,
            ModItemsConfig,
            NamesConfig,
            OrganizationsConfig,
            PedGroupsConfig,
            PhysicalitemsConfig,
            PlateTypesConfig,
            SavedOutfitsConfig,
            SaveGamesConfig,
            SettingsConfig,
            ShopMenusConfig,
            SpawnBlocksConfig,
            SpeechesConfig,
            StreetsConfig,
            WantedLevelsConfig,
            WeaponsConfig,
            ZoneJurisdictionsConfig,
            ZonesConfig
        };
    }
    public override string ToString()
    {
        return ConfigName;
    }
}