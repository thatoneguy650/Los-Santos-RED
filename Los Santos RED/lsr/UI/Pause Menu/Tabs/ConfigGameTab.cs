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
    public void AddConfigItems()
    {
        List<UIMenuItem> configListItems = new List<UIMenuItem>();
        UIMenuItem configCount = new UIMenuItem($"Number of Configs: {GameConfigs.GameConfigList.Count()}", "") { Enabled = false };
        configListItems.Add(configCount);
        if (GameConfigs.GameConfigList != null && GameConfigs.GameConfigList.Any())
        {
            int maxNumber = GameConfigs.GameConfigList.Max(x => x.ConfigNumber);
            for (int i = 1; i <= maxNumber; i++)
            {
                UIMenuItem loadItem;
                GameConfig config = GameConfigs.GameConfigList.FirstOrDefault(x => x.ConfigNumber == i);
                if (config != null)
                {
                    loadItem = new UIMenuItem(config.configName, "") { };
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
                    loadItem = new UIMenuItem($"{i.ToString("D2")} - Empty Config", "") { Enabled = false };
                }
                configListItems.Add(loadItem);
            }
        }
        TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("LOAD", configListItems);
        TabView.AddTab(interactiveListItem2);
    }
}