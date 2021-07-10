using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Collections.Generic;

public class SettingsMenu : Menu
{
    private UIMenu Settings;
    private UIMenuItem ReloadSettingsFromFile;
    private UIMenuItem MapToggle;
    private IActionable Player;
    private IEntityProvideable World;
    public SettingsMenu(MenuPool menuPool, UIMenu parentMenu, IActionable player, IEntityProvideable world)
    {
        Player = player;
        World = world;
        Settings = menuPool.AddSubMenu(parentMenu, "Settings");
        CreateSettingsMenu();
    }

    public override void Hide()
    {
        Settings.Visible = false;
    }

    public override void Show()
    {
        Settings.Visible = true;
    }
    public override void Toggle()
    {
        Update();
        if (!Settings.Visible)
        {
            Settings.Visible = true;
        }
        else
        {
            Settings.Visible = false;
        }
    }
    public void Update()
    {
        CreateSettingsMenu();
    }
    private void CreateSettingsMenu()
    {
        Settings.Clear();
        if(World.IsMPMapLoaded)
        {
            MapToggle = new UIMenuItem("Load SP Map", "Load the SP Map (Default)");
        }
        else
        {
            MapToggle = new UIMenuItem("Load MP Map", "Load the MP Map (For More Interiors)");
        }
        ReloadSettingsFromFile = new UIMenuItem("Reload Settings", "Reloads the Settings XML");
        Settings.AddItem(MapToggle);

        Settings.AddItem(ReloadSettingsFromFile);


        Settings.OnItemSelect += OnActionItemSelect;
        Settings.OnListChange += OnListChange;
    }
    private void OnActionItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == MapToggle)
        {
            if(World.IsMPMapLoaded)
            {
                World.LoadSPMap();
            }
            else
            {
                World.LoadMPMap();
            }
        }
        else if (selectedItem == ReloadSettingsFromFile)
        {
            EntryPoint.ModController.ReloadSettingsFromFile();
        }
        Settings.Visible = false;
    }
    private void OnListChange(UIMenu sender, UIMenuListItem list, int index)
    {

    }
}