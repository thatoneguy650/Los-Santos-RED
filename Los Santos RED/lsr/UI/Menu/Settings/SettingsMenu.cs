using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

public class SettingsMenu : Menu//needs lots of cleanup still
{
    private UIMenu SettingsUIMenu;
    private UIMenuItem ReloadSettingsFromFile;
    private UIMenuItem SaveSettingsToFile;
    private UIMenuItem MapToggle;
    private IActionable Player;
    private IEntityProvideable World;
    private ISettingsProvideable SettingsProvider;
    private MenuPool MenuPool;

    private UIMenu PlayerSettingsIUMenu;
    private UIMenu GeneralSettingsIUMenu;
    private UIMenu KeyBindingSettingsIUMenu;
    private UIMenu PoliceSettingsIUMenu;
    private UIMenu RespawnSettingsIUMenu;
    private UIMenu UISettingsIUMenu;
    private UIMenu DebugSettingsIUMenu;

    public SettingsMenu(MenuPool menuPool, UIMenu parentMenu, IActionable player, IEntityProvideable world, ISettingsProvideable settingsProvideable)
    {
        MenuPool = menuPool;
        Player = player;
        World = world;
        SettingsProvider = settingsProvideable;
        SettingsUIMenu = menuPool.AddSubMenu(parentMenu, "Settings");
        CreateSettingsMenu();
    }
    public override void Hide()
    {
        SettingsUIMenu.Visible = false;
    }

    public override void Show()
    {
        SettingsUIMenu.Visible = true;
    }
    public override void Toggle()
    {
        Update();
        if (!SettingsUIMenu.Visible)
        {
            SettingsUIMenu.Visible = true;
        }
        else
        {
            SettingsUIMenu.Visible = false;
        }
    }
    public void Update()
    {
        CreateSettingsMenu();
    }
    private void CreateSettingsMenu()
    {
        SettingsUIMenu.Clear();
        if(World.IsMPMapLoaded)
        {
            MapToggle = new UIMenuItem("Load SP Map", "Load the SP Map (Default)");
        }
        else
        {
            MapToggle = new UIMenuItem("Load MP Map", "Load the MP Map (For More Interiors)");
        }
        ReloadSettingsFromFile = new UIMenuItem("Reload Settings", "Reloads the Settings XML");
        SaveSettingsToFile = new UIMenuItem("Save Settings", "Saves the Settings to XML");


        SettingsUIMenu.AddItem(MapToggle);
        SettingsUIMenu.AddItem(ReloadSettingsFromFile);
        SettingsUIMenu.AddItem(SaveSettingsToFile);


        PlayerSettingsIUMenu = MenuPool.AddSubMenu(SettingsUIMenu, "Player Settings");
        GeneralSettingsIUMenu = MenuPool.AddSubMenu(SettingsUIMenu, "General Settings");
        KeyBindingSettingsIUMenu = MenuPool.AddSubMenu(SettingsUIMenu, "Key Binding");
        PoliceSettingsIUMenu = MenuPool.AddSubMenu(SettingsUIMenu, "Police Settings");
        RespawnSettingsIUMenu = MenuPool.AddSubMenu(SettingsUIMenu, "Respawn Settings");
        UISettingsIUMenu = MenuPool.AddSubMenu(SettingsUIMenu, "UI Settings");

        DebugSettingsIUMenu = MenuPool.AddSubMenu(SettingsUIMenu, "Debug Settings");

        EntryPoint.WriteToConsole($"CreateSettingSubMenu START", 3);
        CreateSettingSubMenu(typeof(PlayerSettings).GetProperties(), SettingsProvider.SettingsManager.PlayerSettings, PlayerSettingsIUMenu);
        CreateSettingSubMenu(typeof(GeneralSettings).GetProperties(), SettingsProvider.SettingsManager.GeneralSettings, GeneralSettingsIUMenu);
        CreateSettingSubMenu(typeof(KeySettings).GetProperties(), SettingsProvider.SettingsManager.KeySettings, KeyBindingSettingsIUMenu);
        CreateSettingSubMenu(typeof(PoliceSettings).GetProperties(), SettingsProvider.SettingsManager.PoliceSettings, PoliceSettingsIUMenu);
        CreateSettingSubMenu(typeof(RespawnSettings).GetProperties(), SettingsProvider.SettingsManager.RespawnSettings, RespawnSettingsIUMenu);
        CreateSettingSubMenu(typeof(UISettings).GetProperties(), SettingsProvider.SettingsManager.UISettings, UISettingsIUMenu);

        CreateSettingSubMenu(typeof(DebugSettings).GetProperties(), SettingsProvider.SettingsManager.DebugSettings, DebugSettingsIUMenu);
        EntryPoint.WriteToConsole($"CreateSettingSubMenu END", 3);

        SettingsUIMenu.OnItemSelect += OnActionItemSelect;
        SettingsUIMenu.OnListChange += OnListChange;

        PlayerSettingsIUMenu.OnItemSelect += OnPlayerSettingsSelect;
        GeneralSettingsIUMenu.OnItemSelect += OnGeneralSettingsSelect;
        KeyBindingSettingsIUMenu.OnItemSelect += OnKeyBindingsSelect;
        PoliceSettingsIUMenu.OnItemSelect += OnPoliceSettingsSelect;
        RespawnSettingsIUMenu.OnItemSelect += OnRespawnSettingsSelect;
        UISettingsIUMenu.OnItemSelect += OnUISettingsSelect;
        DebugSettingsIUMenu.OnItemSelect += OnDebugSettingsSelect;

        PlayerSettingsIUMenu.OnCheckboxChange += OnCheckboxChange;
        GeneralSettingsIUMenu.OnCheckboxChange += OnCheckboxChange;
        KeyBindingSettingsIUMenu.OnCheckboxChange += OnCheckboxChange;
        PoliceSettingsIUMenu.OnCheckboxChange += OnCheckboxChange;
        RespawnSettingsIUMenu.OnCheckboxChange += OnCheckboxChange;
        UISettingsIUMenu.OnCheckboxChange += OnCheckboxChange;
        DebugSettingsIUMenu.OnCheckboxChange += OnCheckboxChange;
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
        else if (selectedItem == SaveSettingsToFile)
        {
            EntryPoint.ModController.SaveSettingsToFile();
        }
        SettingsUIMenu.Visible = false;
    }
    private void OnListChange(UIMenu sender, UIMenuListItem list, int index)
    {
        //typeof(GeneralSettings).GetFields()
    }

    private void CreateSettingSubMenu(PropertyInfo[] Properties, object SettingsSubType, UIMenu MenuToSet)
    {
        foreach (PropertyInfo fi in Properties)
        {
            if (fi.PropertyType == typeof(bool))
            {
                UIMenuCheckboxItem MySetting = new UIMenuCheckboxItem(fi.Name, (bool)fi.GetValue(SettingsSubType));
                MenuToSet.AddItem(MySetting);
            }
            else if (fi.PropertyType == typeof(int) || fi.PropertyType == typeof(string) || fi.PropertyType == typeof(float) || fi.PropertyType == typeof(uint) || fi.PropertyType == typeof(Keys))
            {
                UIMenuItem MySetting = new UIMenuItem($"{fi.Name}: {fi.GetValue(SettingsSubType)}");
                MenuToSet.AddItem(MySetting);
            }
        }
    }
    private void OnPlayerSettingsSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        UpdateSettings(sender, selectedItem, index, typeof(PlayerSettings).GetProperties(), SettingsProvider.SettingsManager.PlayerSettings);
        SettingsUIMenu.Visible = false;
    }
    private void OnGeneralSettingsSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        UpdateSettings(sender, selectedItem, index, typeof(GeneralSettings).GetProperties(), SettingsProvider.SettingsManager.GeneralSettings);
        SettingsUIMenu.Visible = false;
    }
    private void OnKeyBindingsSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        UpdateSettings(sender, selectedItem, index, typeof(KeySettings).GetProperties(), SettingsProvider.SettingsManager.KeySettings);
        SettingsUIMenu.Visible = false;
    }
    private void OnPoliceSettingsSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        UpdateSettings(sender, selectedItem, index, typeof(PoliceSettings).GetProperties(), SettingsProvider.SettingsManager.PoliceSettings);
        SettingsUIMenu.Visible = false;
    }
    private void OnRespawnSettingsSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        UpdateSettings(sender, selectedItem, index, typeof(RespawnSettings).GetProperties(), SettingsProvider.SettingsManager.RespawnSettings);
        SettingsUIMenu.Visible = false;
    }
    private void OnUISettingsSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        UpdateSettings(sender, selectedItem, index, typeof(UISettings).GetProperties(), SettingsProvider.SettingsManager.UISettings);
        SettingsUIMenu.Visible = false;
    }
    private void OnDebugSettingsSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        UpdateSettings(sender, selectedItem, index, typeof(DebugSettings).GetProperties(), SettingsProvider.SettingsManager.DebugSettings);
        SettingsUIMenu.Visible = false;
    }
    private void OnCheckboxChange(UIMenu sender, UIMenuCheckboxItem checkbox, bool Checked)
    {
        if (sender == UISettingsIUMenu)
        {
            PropertyInfo[] MyFields = typeof(UISettings).GetProperties();
            PropertyInfo MySetting = MyFields.Where(x => x.Name == checkbox.Text).FirstOrDefault();
            MySetting.SetValue(SettingsProvider.SettingsManager.UISettings, Checked);
        }
        else if (sender == PlayerSettingsIUMenu)
        {
            PropertyInfo[] MyFields = typeof(PlayerSettings).GetProperties();
            PropertyInfo MySetting = MyFields.Where(x => x.Name == checkbox.Text).FirstOrDefault();
            MySetting.SetValue(SettingsProvider.SettingsManager.PlayerSettings, Checked);
        }
        else if (sender == GeneralSettingsIUMenu)
        {
            PropertyInfo[] MyFields = typeof(GeneralSettings).GetProperties();
            PropertyInfo MySetting = MyFields.Where(x => x.Name == checkbox.Text).FirstOrDefault();
            MySetting.SetValue(SettingsProvider.SettingsManager.GeneralSettings, Checked);
        }
        else if (sender == KeyBindingSettingsIUMenu)
        {
            PropertyInfo[] MyFields = typeof(KeySettings).GetProperties();
            PropertyInfo MySetting = MyFields.Where(x => x.Name == checkbox.Text).FirstOrDefault();
            MySetting.SetValue(SettingsProvider.SettingsManager.KeySettings, Checked);
        }
        else if (sender == PoliceSettingsIUMenu)
        {
            PropertyInfo[] MyFields = typeof(PoliceSettings).GetProperties();
            PropertyInfo MySetting = MyFields.Where(x => x.Name == checkbox.Text).FirstOrDefault();
            MySetting.SetValue(SettingsProvider.SettingsManager.PoliceSettings, Checked);
        }
        else if (sender == RespawnSettingsIUMenu)
        {
            PropertyInfo[] MyFields = typeof(RespawnSettings).GetProperties();
            PropertyInfo MySetting = MyFields.Where(x => x.Name == checkbox.Text).FirstOrDefault();
            MySetting.SetValue(SettingsProvider.SettingsManager.RespawnSettings, Checked);
        }
        else if (sender == DebugSettingsIUMenu)
        {
            PropertyInfo[] MyFields = typeof(DebugSettings).GetProperties();
            PropertyInfo MySetting = MyFields.Where(x => x.Name == checkbox.Text).FirstOrDefault();
            MySetting.SetValue(SettingsProvider.SettingsManager.DebugSettings, Checked);
        }
    }
    private void UpdateSettings(UIMenu sender, UIMenuItem selectedItem, int index, PropertyInfo[] MyProperties, object ToSet)
    {
        string MySettingName = selectedItem.Text.Split(':')[0];
        PropertyInfo MySetting = MyProperties.FirstOrDefault(x => x.Name == MySettingName);
        if (MySetting == null || MySetting.PropertyType == typeof(bool))
            return;
        string Value = NativeHelper.GetKeyboardInput("");

        if (MySetting.PropertyType == typeof(float))
        {
            if (float.TryParse(Value, out float myFloat))
            {
                MySetting.SetValue(ToSet, myFloat);
                selectedItem.Text = $"{MySettingName}: {Value}";
            }
        }
        else if (MySetting.PropertyType == typeof(int))
        {
            if (int.TryParse(Value, out int myInt))
            {
                MySetting.SetValue(ToSet, myInt);
                selectedItem.Text = $"{MySettingName}: {Value}";
            }
        }
        else if (MySetting.PropertyType == typeof(uint))
        {
            if (uint.TryParse(Value, out uint myInt))
            {
                MySetting.SetValue(ToSet, myInt);
                selectedItem.Text = $"{MySettingName}: {Value}";
            }
        }
        else if (MySetting.PropertyType == typeof(string))
        {
            MySetting.SetValue(ToSet, Value);
            selectedItem.Text = $"{MySettingName}: {Value}";
        }
        else if (MySetting.PropertyType == typeof(Keys))
        {
            MySetting.SetValue(ToSet, Value);
            selectedItem.Text = $"{MySettingName}: {Value}";
        }
        MenuPool.ProcessMenus();
    }

}