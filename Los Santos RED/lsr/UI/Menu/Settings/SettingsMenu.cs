using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

public class SettingsMenu : Menu//needs lots of cleanup still
{
    private MenuPool MenuPool;
    private UIMenuItem ReloadSettingsFromFile;
    private UIMenuItem SaveSettingsToFile;
    private ISettingsProvideable SettingsProvider;
    private UIMenu SettingsUIMenu;
    public SettingsMenu(MenuPool menuPool, UIMenu parentMenu, ISettingsProvideable settingsProvideable)
    {
        MenuPool = menuPool;
        SettingsProvider = settingsProvideable;
        SettingsUIMenu = menuPool.AddSubMenu(parentMenu, "Settings");
        parentMenu.MenuItems[parentMenu.MenuItems.Count() - 1].Description = "Change, Save, and Load Settings for the mod.";
        parentMenu.MenuItems[parentMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Art;
        SettingsUIMenu.SetBannerType(EntryPoint.LSRedColor);
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
        ReloadSettingsFromFile = new UIMenuItem("Reload Settings", "Reloads the Settings XML");
        SaveSettingsToFile = new UIMenuItem("Save Settings", "Saves the Settings to XML");
        SettingsUIMenu.AddItem(ReloadSettingsFromFile);
        SettingsUIMenu.AddItem(SaveSettingsToFile);
        SettingsUIMenu.OnItemSelect += OnItemSelect;
        SettingsUIMenu.OnListChange += OnListChange;

        UIMenu playerSubMenu = MenuPool.AddSubMenu(SettingsUIMenu, "Player Settings SubMenu");
        playerSubMenu.SetBannerType(EntryPoint.LSRedColor);

        UIMenu worldSubMenu = MenuPool.AddSubMenu(SettingsUIMenu, "World Settings SubMenu");
        worldSubMenu.SetBannerType(EntryPoint.LSRedColor);

        UIMenu otherSubMenu = MenuPool.AddSubMenu(SettingsUIMenu, "Other Settings SubMenu");
        otherSubMenu.SetBannerType(EntryPoint.LSRedColor);

        PropertyInfo[] properties = SettingsProvider.SettingsManager.GetType().GetProperties();
        foreach (PropertyInfo property in properties)
        {
            string strippedPropertyName = property.Name;//.Replace("Settings","");
            UIMenu subMenu;
            System.ComponentModel.CategoryAttribute propertyCategory = (System.ComponentModel.CategoryAttribute)property.GetCustomAttribute(typeof(CategoryAttribute), true);
            if (propertyCategory != null)
            {
                if (propertyCategory.Category == "Player")
                {
                    subMenu = MenuPool.AddSubMenu(playerSubMenu, strippedPropertyName);
                }
                else if (propertyCategory.Category == "World")
                {
                    subMenu = MenuPool.AddSubMenu(worldSubMenu, strippedPropertyName);
                }
                else
                {
                    subMenu = MenuPool.AddSubMenu(otherSubMenu, strippedPropertyName);
                }
            }
            else
            {
                subMenu = MenuPool.AddSubMenu(otherSubMenu, strippedPropertyName);
            }

            subMenu.SetBannerType(EntryPoint.LSRedColor);
            subMenu.OnItemSelect += OnNewSettingsSelect;
            subMenu.OnCheckboxChange += OnNewCheckboxChange;

            object SubSettings = property.GetValue(SettingsProvider.SettingsManager);
            PropertyInfo[] subSettings = property.PropertyType.GetProperties();
            foreach (PropertyInfo fi in subSettings)
            {
                System.ComponentModel.DescriptionAttribute coolio = (System.ComponentModel.DescriptionAttribute)fi.GetCustomAttribute(typeof(DescriptionAttribute), true);
                string Description = fi.Name;
                if (coolio != null)
                {
                    Description = coolio.Description;
                }
                Description = Description.Substring(0, Math.Min(800, Description.Length));
                if (fi.PropertyType == typeof(bool))
                {
                    UIMenuCheckboxItem MySetting = new UIMenuCheckboxItem(fi.Name, (bool)fi.GetValue(SubSettings), Description);
                    subMenu.AddItem(MySetting);
                }
                else if (fi.PropertyType == typeof(int) || fi.PropertyType == typeof(string) || fi.PropertyType == typeof(float) || fi.PropertyType == typeof(uint) || fi.PropertyType == typeof(Keys))
                {
                    UIMenuItem MySetting = new UIMenuItem($"{fi.Name}: {fi.GetValue(SubSettings)}", Description);
                    subMenu.AddItem(MySetting);
                }
            }
        }
    }
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == ReloadSettingsFromFile)
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
    }
    private void OnNewCheckboxChange(UIMenu sender, UIMenuCheckboxItem checkbox, bool Checked)
    {
        // EntryPoint.WriteToConsole($"OnNewSettingsSelect sender {sender.TitleText}  selectedItem {checkbox.Text} Checked {Checked}", 5);
        PropertyInfo[] properties = SettingsProvider.SettingsManager.GetType().GetProperties();
        foreach (PropertyInfo property in properties)
        {
            if (property.Name == sender.SubtitleText)
            {
                object SubSettings = property.GetValue(SettingsProvider.SettingsManager);
                PropertyInfo[] subSettings = property.PropertyType.GetProperties();

                string propertyName = checkbox.Text;
                int colonIndex = propertyName.IndexOf(":");
                propertyName = (colonIndex > 0 ? propertyName.Substring(0, colonIndex) : propertyName);

                //EntryPoint.WriteToConsole($"OnNewSettingsSelect property.Name {property} propertyName {propertyName}", 5);
                foreach (PropertyInfo fi in subSettings)
                {
                    if (propertyName == fi.Name)
                    {
                        fi.SetValue(SubSettings, Checked);
                        SettingsUIMenu.Visible = false;
                        return;
                    }
                }
            }
        }
    }
    private void OnNewSettingsSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        //EntryPoint.WriteToConsole($"OnNewSettingsSelect sender {sender.TitleText} sender.SubtitleText {sender.SubtitleText}  selectedItem {selectedItem.Text}", 5);
        PropertyInfo[] properties = SettingsProvider.SettingsManager.GetType().GetProperties();
        foreach (PropertyInfo property in properties)
        {
            if (property.Name == sender.SubtitleText)
            {
                object SubSettings = property.GetValue(SettingsProvider.SettingsManager);
                PropertyInfo[] subSettings = property.PropertyType.GetProperties();

                string propertyName = selectedItem.Text;
                int colonIndex = propertyName.IndexOf(":");
                propertyName = (colonIndex > 0 ? propertyName.Substring(0, colonIndex) : propertyName);

                //EntryPoint.WriteToConsole($"OnNewSettingsSelect property.Name {property} propertyName {propertyName}", 5);
                foreach (PropertyInfo fi in subSettings)
                {
                    if (propertyName == fi.Name)
                    {
                        UpdateSettings(sender, selectedItem, index, SubSettings.GetType().GetProperties(), SubSettings);
                        SettingsUIMenu.Visible = false;
                        return;
                    }
                }
            }
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
            if (Keys.TryParse(Value, out Keys myInt))
            {
                MySetting.SetValue(ToSet, myInt);
                selectedItem.Text = $"{MySettingName}: {Value}";
            }
        }
        MenuPool.ProcessMenus();
    }
}