using LosSantosRED.lsr.Data;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class GameConfigs : IGameConfigs
{
    private ModDataFileManager ModDataFileManager;
    public GameConfigs(ModDataFileManager modDataFileManager)
    {
        ModDataFileManager = modDataFileManager;
    }
    public List<GameConfig> GameConfigList { get; private set; } = new List<GameConfig>();
    public void Setup()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");

        List<FileInfo> allFiles = LSRDirectory.GetFiles("*.xml").ToList();

        Dictionary<string, List<FileInfo>> groupedConfigs = new Dictionary<string, List<FileInfo>>();

        foreach (FileInfo file in allFiles)
        {
            if (file.Name.StartsWith("SavedVariation", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

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

            GameConfig config = new GameConfig(ModDataFileManager);
            config.ConfigNumber = i + 1;
            config.configName = groupKey;

            GameConfigList.Add(config);
        }
    }

    public void Load(GameConfig config)
    {
        config.Load();
    }
}