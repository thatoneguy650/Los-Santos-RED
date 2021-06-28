using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Collections.Generic;

public class SettingsMenu : Menu
{
    private UIMenu Settings;
    private UIMenuItem ListTemp;
    private IActionable Player;
    private UIMenuItem MenuTemp;
    public SettingsMenu(MenuPool menuPool, UIMenu parentMenu, IActionable player)
    {
        Player = player;
        Settings = menuPool.AddSubMenu(parentMenu, "Settings");
        CreateSettingsMenu();
    }
    public int SelectedPlateIndex { get; set; }

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

    }
    private void CreateSettingsMenu()
    {
        MenuTemp = new UIMenuItem("Temp", "Settings Temp");
        Settings.AddItem(MenuTemp);
        Settings.OnItemSelect += OnActionItemSelect;
        Settings.OnListChange += OnListChange;
    }
    private void OnActionItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == MenuTemp)
        {
            Player.CommitSuicide();
        }
        Settings.Visible = false;
    }
    private void OnListChange(UIMenu sender, UIMenuListItem list, int index)
    {

    }
}