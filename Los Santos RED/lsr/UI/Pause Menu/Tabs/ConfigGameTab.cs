using LosSantosRED.lsr.Data;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.Elements;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DispatchScannerFiles;


public class ConfigGameTab
{
    private ISaveable Player; // Player only here for Button Prompts
    private TabView TabView;
    private List<TabItem> items;

    private IGameConfigs GameConfigs;
    private ISettingsProvideable Settings;

    public ConfigGameTab(ISaveable player, ISettingsProvideable settings, IGameConfigs gameConfigs, TabView tabView)
    {
        Player = player;
        Settings = settings;
        GameConfigs = gameConfigs;
        TabView = tabView;
    }
    public void AddSuffixItems()
    {
        List<UIMenuItem> configListItems = new List<UIMenuItem>();
        UIMenuItem configCount = new UIMenuItem($"Number of Configs: {GameConfigs.SuffixConfigList.Count()}", "") { Enabled = false };
        configListItems.Add(configCount);
        if (GameConfigs.SuffixConfigList != null && GameConfigs.SuffixConfigList.Any())
        {
            int index = 1;
            foreach (GameConfig config in GameConfigs.SuffixConfigList)
            {
                UIMenuItem loadItem;
                if (config != null)
                {
                    loadItem = new UIMenuItem(config.ConfigName, "") { };
                    loadItem.Activated += (s, e) =>
                    {
                        SimpleWarning popUpWarning = new SimpleWarning("Load", "Are you sure you want to load this config?", "", Player.ButtonPrompts, Settings);
                        popUpWarning.Show();
                        if (popUpWarning.IsAccepted)
                        {
                            TabView.Visible = false;
                            Game.IsPaused = false;

                            GameConfigs.Load(config);
                        }
                    };
                }
                else
                {
                    loadItem = new UIMenuItem($"{index.ToString("D2")} - Empty Config", "") { Enabled = false };
                }
                configListItems.Add(loadItem);
                index++;
            }
        }
        TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("SUFFIXES", configListItems);
        TabView.AddTab(interactiveListItem2);
    }
    public void AddCustomConfigItems()
    {
        List<UIMenuItem> configListItems = new List<UIMenuItem>();
        UIMenuItem configCount = new UIMenuItem($"Number of Configs: {GameConfigs.CustomConfigList.Count()}", "") { Enabled = false };
        configListItems.Add(configCount);
        if (GameConfigs.CustomConfigList != null && GameConfigs.CustomConfigList.Any())
        {
            int index = 1;
            foreach (GameConfig config in GameConfigs.CustomConfigList)
            {
                UIMenuItem loadItem;
                if (config != null)
                {
                    loadItem = new UIMenuItem(config.ConfigName, "") { Enabled = GameConfigs.AreFilesAvailable(config) };
                    loadItem.Activated += (s, e) =>
                    {
                        SimpleWarning popUpWarning = new SimpleWarning("Load", "Are you sure you want to load this config?", "", Player.ButtonPrompts, Settings);
                        popUpWarning.Show();
                        if (popUpWarning.IsAccepted)
                        {
                            TabView.Visible = false;
                            Game.IsPaused = false;

                            GameConfigs.Load(config);
                        }
                    };
                }
                else
                {
                    loadItem = new UIMenuItem($"{index.ToString("D2")} - Empty Config", "") { Enabled = false };
                }
                configListItems.Add(loadItem);
                index++;
            }
        }
        TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("CUSTOM", configListItems);
        TabView.AddTab(interactiveListItem2);
    }
}