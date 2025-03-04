using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

public class ConfigMenu : ModUIMenu
{
    private MenuPool MenuPool;
    private UIMenu ParentMenu;
    private UI UI;
    private UIMenu ConfigUIMenu;

    public ConfigMenu(MenuPool menuPool, UIMenu parentMenu, UI ui)
    {
        MenuPool = menuPool;
        ParentMenu = parentMenu;
        UI = ui;
    }
    public void Setup()
    {
        ConfigUIMenu = MenuPool.AddSubMenu(ParentMenu, "Config Manager");
        ParentMenu.MenuItems[ParentMenu.MenuItems.Count() - 1].Description = "Change, Save, and Load Configs.";
        ParentMenu.MenuItems[ParentMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Crown;
        ConfigUIMenu.SetBannerType(EntryPoint.LSRedColor);
        CreateConfigManagerMenu();
        CreateCustomConfigCuratorMenu();
    }

    public override void Hide()
    {
        ConfigUIMenu.Visible = false;
    }
    public override void Show()
    {
        Update();
        ConfigUIMenu.Visible = true;
    }
    public override void Toggle()
    {
        Update();
        if (!ConfigUIMenu.Visible)
        {
            ConfigUIMenu.Visible = true;
        }
        else
        {
            ConfigUIMenu.Visible = false;
        }
    }
    public void Update()
    {
        ConfigUIMenu.Clear();
        CreateConfigManagerMenu();
        CreateCustomConfigCuratorMenu();
    }
    private void CreateConfigManagerMenu()
    {
        UIMenuItem ShowGameConfigsMenu = new UIMenuItem("Configs", "Shows a list of configurations.");
        ShowGameConfigsMenu.RightBadge = UIMenuItem.BadgeStyle.Mask;
        ShowGameConfigsMenu.Activated += (s, e) =>
        {
            UI.ConfigPauseMenu.Toggle();
            ConfigUIMenu.Visible = false;
        };
        ConfigUIMenu.AddItem(ShowGameConfigsMenu);
    }
    private void CreateCustomConfigCuratorMenu()
    {
        UIMenuItem ShowConfigCuratorMenu = new UIMenuItem("Config Curator", "Create a custom config bundle.");
        ShowConfigCuratorMenu.RightBadge = UIMenuItem.BadgeStyle.Makeup;
        ShowConfigCuratorMenu.Activated += (s, e) =>
        {
            UI.ConfigPauseMenu.Toggle();
            ConfigUIMenu.Visible = false;
        };
        ConfigUIMenu.AddItem(ShowConfigCuratorMenu);
    }
}