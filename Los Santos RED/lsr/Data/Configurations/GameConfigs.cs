using LosSantosRED.lsr.Data;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Logging;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;

public class GameConfigs : IGameConfigs
{
    private string ConfigFileName = "Plugins\\LosSantosRED\\CustomConfigs.xml";

    public GameConfigs()
    {
    }
    public List<GameConfig> SuffixConfigList { get; private set; } = new List<GameConfig>();
    public List<GameConfig> CustomConfigList { get; private set; } = new List<GameConfig>();
    public void Setup() // Setup Suffixes
    {
        SetupSuffixes();
    }
    private void SetupSuffixes() // Setup Suffixes
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");

        List<FileInfo> allFiles = LSRDirectory.GetFiles("*.xml").ToList();

        Dictionary<string, List<FileInfo>> groupedConfigs = new Dictionary<string, List<FileInfo>>();

        foreach (FileInfo file in allFiles)
        {
            if (file.Name.StartsWith("SavedVariation", StringComparison.OrdinalIgnoreCase) || file.Name.StartsWith("CustomConfigs", StringComparison.OrdinalIgnoreCase)) continue;

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.Name);
            int lastUnderscoreIndex = fileNameWithoutExtension.LastIndexOf('_');

            if (lastUnderscoreIndex != -1)
            {
                string configSuffix = fileNameWithoutExtension.Substring(lastUnderscoreIndex + 1);

                if (!groupedConfigs.ContainsKey(configSuffix))
                {
                    groupedConfigs[configSuffix] = new List<FileInfo>();
                }
                groupedConfigs[configSuffix].Add(file);
            }
            else
            {
                if (!groupedConfigs.ContainsKey("Default"))
                {
                    groupedConfigs["Default"] = new List<FileInfo>();
                }
                groupedConfigs["Default"].Add(file);
            }
        }

        List<string> groupKeys = groupedConfigs.Keys.ToList();

        for (int i = 0; i < groupKeys.Count; i++)
        {
            string groupKey = groupKeys[i];
            //List<FileInfo> groupFiles = groupedConfigs[groupKey];

            EntryPoint.WriteToConsole($"Config Group: {groupKey}", 0);

            GameConfig config = new GameConfig(groupKey);

            SuffixConfigList.Add(config);
        }
    }

    public void Load(GameConfig config)
    {
        try
        {
            EntryPoint.LoadedConfig = config;
            EntryPoint.IsLoadingAltConfig = true;
            EntryPoint.ModController.Dispose();
        }
        catch (Exception e)
        {
            Game.FadeScreenIn(0);
            EntryPoint.WriteToConsole($"Error Loading {config.ConfigName} config: " + e.Message + " " + e.StackTrace, 0);
            Game.DisplayNotification($"Error Loading {config.ConfigName} config");
        }
    }
    public void SerializeAllSettings()
    {
        Serialization.SerializeParams(CustomConfigList, ConfigFileName);
    }
    public bool AreFilesAvailable(GameConfig config)
    {
        FileInfo[] configFiles =
        {
            GetConfigFile(config.AgenciesConfig, "Agencies"),
            GetConfigFile(config.CrimesConfig, "Crimes"),
            GetConfigFile(config.ZoneJurisdictionsConfig, "ZoneJurisdictions"),
            GetConfigFile(config.CountyJurisdictionsConfig, "CountyJurisdictions"),
            //GetConfigFile(config.WantedLevelsConfig, "WantedLevels"),
            GetConfigFile(config.GangsConfig, "Gangs"),
            GetConfigFile(config.GangTerritoriesConfig, "GangTerritories"),
            GetConfigFile(config.CellphonesConfig, "Cellphones"),
            GetConfigFile(config.CraftableItemsConfig, "CraftableItems"),
            GetConfigFile(config.IntoxicantsConfig, "Itoxicants"),
            GetConfigFile(config.ModItemsConfig, "ModItems"),
            GetConfigFile(config.PhysicalitemsConfig, "PhysicalItems"),
            GetConfigFile(config.ShopMenusConfig, "ShopMenus"),
            GetConfigFile(config.InteriorsConfig, "Interiors"),
            GetConfigFile(config.LocationTypesConfig, "LocationTypes"),
            GetConfigFile(config.LocationsConfig, "Locations"),
            GetConfigFile(config.StreetsConfig, "Streets"),
            GetConfigFile(config.ZonesConfig, "Zones"),
            GetConfigFile(config.ContactsConfig, "Contacts"),
            GetConfigFile(config.OrganizationsConfig, "Organizations"),
            GetConfigFile(config.SpawnBlocksConfig, "SpawnBlocks"),
            GetConfigFile(config.DancesConfig, "Dances"),
            GetConfigFile(config.DispatchablePeopleConfig, "DispatchablePeople"),
            GetConfigFile(config.GesturesConfig, "Gestures"),
            GetConfigFile(config.HeadsConfig, "Heads"),
            GetConfigFile(config.NamesConfig, "Names"),
            GetConfigFile(config.PedGroupsConfig, "PedGroups"),
            GetConfigFile(config.SavedOutfitsConfig, "SavedOutfits"),
            GetConfigFile(config.SpeechesConfig, "Speeches"),
            GetConfigFile(config.SaveGamesConfig, "SaveGames"),
            GetConfigFile(config.SettingsConfig, "Settings"),
            GetConfigFile(config.DispatchableVehiclesConfig, "DispatchableVehicles"),
            GetConfigFile(config.PlateTypesConfig, "PlateTypes"),
            GetConfigFile(config.IssueableWeaponsConfig, "IssuableWeapons"),
            GetConfigFile(config.VehicleRacesConfig, "VehicleRaces"),
            GetConfigFile(config.WeaponsConfig, "Weapons")
        };

        foreach (FileInfo cfg in configFiles) if (cfg == null) return false;
        return true;
    }
    private FileInfo GetConfigFile(string configValue, string configKey)
    {
        DirectoryInfo taskDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        if (string.IsNullOrEmpty(configValue)) return taskDirectory.GetFiles($"{configKey}.xml").OrderByDescending(x => x.Name).FirstOrDefault();

        return taskDirectory.GetFiles(configValue.Equals("Default") ? $"{configKey}.xml" : $"{configKey}_{configValue}.xml").OrderByDescending(x => x.Name) .FirstOrDefault();
    }
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles($"CustomConfigs.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded CustomConfigs config  {ConfigFileName}", 0);
            CustomConfigList = Serialization.DeserializeParams<GameConfig>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No CustomConfigs config found, creating default", 0);
            DefaultConfig();
        }
    }

    private void DefaultConfig()
    {
        CustomConfigList = new List<GameConfig>();
        AddFEJ_FMLP();
        AddFEJ_FMLP_FMT();
        Serialization.SerializeParams(CustomConfigList, ConfigFileName);
    }
    private void AddFEJ_FMLP()
    {
        GameConfig FEJ_FMLP = new GameConfig("FEJ + FMLP", "FullExpandedJurisdiction","Default", "FullExpandedJurisdiction","Default", "Default",
                                            "Default","Default", "FullExpandedJurisdiction", "FullExpandedJurisdiction","Default",
                                            "Default","Default", "Default", "Default", "Default", "FullExpandedJurisdiction", "Default",
                                            "Default", "Default", "Default", "FullExpandedJurisdiction", "Default", "Default", "FullModernLicensePlates",
                                            "Default", "Default", "Default", "Default", "Default", "Default", "Default",  "Default", "FullExpandedJurisdiction", "Default", "Default");
        CustomConfigList.Add(FEJ_FMLP);
    }
    private void AddFEJ_FMLP_FMT()
    {
        GameConfig FEJ_FMLP_FMT = new GameConfig("FEJ + FMLP + FMT", "FullExpandedJurisdiction", "Default", "FullExpandedJurisdiction", "Default", "Default",
                                            "Default", "Default", "FullExpandedJurisdiction", "FullExpandedJurisdiction", "Default",
                                            "Default", "Default", "Default", "Default", "Default", "FullExpandedJurisdiction", "Default",
                                            "Default", "FullModernTraffic", "Default", "FullExpandedJurisdiction", "Default", "Default", "FullModernLicensePlates",
                                            "Default", "Default", "Default", "FullModernTraffic", "Default", "Default", "Default", "Default", "FullExpandedJurisdiction", "Default", "Default");
        CustomConfigList.Add(FEJ_FMLP_FMT);
    }
}