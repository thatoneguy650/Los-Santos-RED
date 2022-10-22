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

public class SettingsMenu : Menu//needs lots of cleanup still
{
    private MenuPool MenuPool;
    private UIMenu ParentMenu;

    private UIMenuItem ReloadSettingsFromFile;
    private UIMenuItem SaveSettingsToFile;
    private UIMenuItem DefaultSettings;
    private UIMenuItem EasySettings;
    private UIMenuItem HardSettings;
    private UIMenu SettingsUIMenu;
    private UIMenuItem MySettings;

    private ISettingsProvideable SettingsProvider;
    private ICrimes Crimes;
    private IIntoxicants Intoxicants;
    public SettingsMenu(MenuPool menuPool, UIMenu parentMenu, ISettingsProvideable settingsProvideable, ICrimes crimes, IIntoxicants intoxicants)
    {
        MenuPool = menuPool;
        ParentMenu = parentMenu;
        SettingsProvider = settingsProvideable;
        Crimes = crimes;
        Intoxicants = intoxicants;
    }
    public void Setup()
    {
        SettingsUIMenu = MenuPool.AddSubMenu(ParentMenu, "Settings");
        ParentMenu.MenuItems[ParentMenu.MenuItems.Count() - 1].Description = "Change, Save, and Load Settings for the mod.";
        ParentMenu.MenuItems[ParentMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Art;
        SettingsUIMenu.SetBannerType(EntryPoint.LSRedColor);
        CreateSettingsMenu();
        SettingsUIMenu.OnMenuOpen += SettingsUIMenu_OnMenuOpen;
    }

    private void SettingsUIMenu_OnMenuOpen(UIMenu sender)
    {
       // Update();
    }

    public override void Hide()
    {
        SettingsUIMenu.Visible = false;
    }
    public override void Show()
    {
        Update();
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
        //ReloadSettingsFromFile = new UIMenuItem("Reload Settings From File", "Reloads the Settings XML");
        SaveSettingsToFile = new UIMenuItem("Save Settings To File", "Saves the Settings to XML");
        DefaultSettings = new UIMenuItem("Set Default Settings", "Set all values back to default settings");
        MySettings = new UIMenuItem("Set Greskrendtregk Settings", "Set my personal settings");
        EasySettings = new UIMenuItem("Set Easy Settings", "Use the easy preset for settings");
        HardSettings = new UIMenuItem("Set Hard Settings", "Use the hard preset for settings");

        //SettingsUIMenu.AddItem(ReloadSettingsFromFile);
        SettingsUIMenu.AddItem(SaveSettingsToFile);
        SettingsUIMenu.AddItem(DefaultSettings);
        SettingsUIMenu.AddItem(MySettings);
        //SettingsUIMenu.AddItem(EasySettings);
        SettingsUIMenu.AddItem(HardSettings);


        SettingsUIMenu.OnItemSelect += OnItemSelect;
        SettingsUIMenu.OnListChange += OnListChange;

        UIMenu playerSubMenu = MenuPool.AddSubMenu(SettingsUIMenu, "Change Player Settings SubMenu");
        playerSubMenu.SetBannerType(EntryPoint.LSRedColor);

        UIMenu worldSubMenu = MenuPool.AddSubMenu(SettingsUIMenu, "Change World Settings SubMenu");
        worldSubMenu.SetBannerType(EntryPoint.LSRedColor);

        UIMenu uiSubMenu = MenuPool.AddSubMenu(SettingsUIMenu, "Change UI Settings SubMenu");
        uiSubMenu.SetBannerType(EntryPoint.LSRedColor);

        UIMenu otherSubMenu = MenuPool.AddSubMenu(SettingsUIMenu, "Change Other Settings SubMenu");
        otherSubMenu.SetBannerType(EntryPoint.LSRedColor);

        PropertyInfo[] properties = SettingsProvider.SettingsManager.GetType().GetProperties();
        foreach (PropertyInfo property in properties)
        {
            string strippedPropertyName = property.Name;//.Replace("Settings","");
            UIMenu subMenu;
            System.ComponentModel.CategoryAttribute propertyCategory = (System.ComponentModel.CategoryAttribute)property.GetCustomAttribute(typeof(CategoryAttribute), true);

            System.ComponentModel.DescriptionAttribute propertyNameAlt = (System.ComponentModel.DescriptionAttribute)property.GetCustomAttribute(typeof(DescriptionAttribute), true);
            if (propertyNameAlt != null)
            {
                strippedPropertyName = propertyNameAlt.Description;
            }


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
                else if (propertyCategory.Category == "UI")
                {
                    subMenu = MenuPool.AddSubMenu(uiSubMenu, strippedPropertyName);
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
            subMenu.Width = 0.5f;

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


        //CreateCrimesSubMenu();
        CreateItemSubMenu("Change Crimes SubMenu", Crimes.CrimeList);
        //CreateItemSubMenu("Change Intoxicants SubMenu", Intoxicants.Items);

    }
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == SaveSettingsToFile)
        {
            SettingsProvider.SerializeAllSettings();
            Crimes.SerializeAllSettings();
        }
        else if (selectedItem == EasySettings)
        {
            SettingsProvider.SetEasy();
            Crimes.SetEasy();
        }
        else if (selectedItem == DefaultSettings)
        {
            SettingsProvider.SetDefault();
            Crimes.SetDefault();
        }
        else if (selectedItem == HardSettings)
        {
            SettingsProvider.SetHard();
            Crimes.SetHard();
        }
        else if (selectedItem == MySettings)
        {
            SettingsProvider.SetPreferred();
            Crimes.SetPreferred();
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
            DescriptionAttribute coolio = (DescriptionAttribute)property.GetCustomAttribute(typeof(DescriptionAttribute), true);



            if (property.Name == sender.SubtitleText || coolio?.Description == sender.SubtitleText)
            {
                object SubSettings = property.GetValue(SettingsProvider.SettingsManager);
                PropertyInfo[] subSettings = property.PropertyType.GetProperties();

                string propertyName = checkbox.Text;
                int colonIndex = propertyName.IndexOf(":");
                propertyName = (colonIndex > 0 ? propertyName.Substring(0, colonIndex) : propertyName);

                EntryPoint.WriteToConsole($"OnNewCheckboxChange property.Name {property} propertyName {propertyName}", 5);
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
            DescriptionAttribute coolio = (DescriptionAttribute)property.GetCustomAttribute(typeof(DescriptionAttribute), true);


            if (property.Name == sender.SubtitleText || coolio?.Description == sender.SubtitleText)
            {
                object SubSettings = property.GetValue(SettingsProvider.SettingsManager);
                PropertyInfo[] subSettings = property.PropertyType.GetProperties();

                string propertyName = selectedItem.Text;
                int colonIndex = propertyName.IndexOf(":");
                propertyName = (colonIndex > 0 ? propertyName.Substring(0, colonIndex) : propertyName);

                EntryPoint.WriteToConsole($"OnNewSettingsSelect property.Name {property} propertyName {propertyName}", 5);
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

    private void CreateCrimesSubMenu()
    {
        UIMenu crimesSubMenu = MenuPool.AddSubMenu(SettingsUIMenu, "Change Crimes SubMenu");
        crimesSubMenu.SetBannerType(EntryPoint.LSRedColor);
        crimesSubMenu.Width = 0.5f;
        foreach (Crime crime in Crimes.CrimeList)
        {
            UIMenu crimeMenu = MenuPool.AddSubMenu(crimesSubMenu, crime.Name);
            crimeMenu.SetBannerType(EntryPoint.LSRedColor);
            crimeMenu.Width = 0.5f;
            PropertyInfo[] properties = crime.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string strippedPropertyName = property.Name;
                CategoryAttribute propertyCategory = (CategoryAttribute)property.GetCustomAttribute(typeof(CategoryAttribute), true);
                DescriptionAttribute propertyNameAlt = (DescriptionAttribute)property.GetCustomAttribute(typeof(DescriptionAttribute), true);
                if (propertyNameAlt != null)
                {
                    strippedPropertyName = propertyNameAlt.Description;
                }
                string Description = property.Name;
                Description = Description.Substring(0, Math.Min(800, Description.Length));
                if (property.PropertyType == typeof(bool))
                {
                    UIMenuCheckboxItem MySetting = new UIMenuCheckboxItem(property.Name, (bool)property.GetValue(crime), Description);
                    MySetting.CheckboxEvent += (sender, Checked) =>
                    {
                        property.SetValue(crime, Checked);
                        //crimeMenu.Visible = false;
                    };
                    crimeMenu.AddItem(MySetting);
                }
                else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(string) || property.PropertyType == typeof(float) || property.PropertyType == typeof(uint) || property.PropertyType == typeof(Keys))
                {
                    UIMenuItem MySetting = new UIMenuItem($"{property.Name}: {property.GetValue(crime)}", Description);
                    MySetting.Activated += (sender, selectedItem) =>
                    {
                        UpdateSettings(sender, selectedItem, 0, crime.GetType().GetProperties(), crime);
                        //sender.Visible = false;
                    };
                    crimeMenu.AddItem(MySetting);
                }
            }
        }
    }
    private void CreateItemSubMenu<T>(string name, List<T> list)
    {
        UIMenu genericSubMenu = MenuPool.AddSubMenu(SettingsUIMenu, name);
        genericSubMenu.SetBannerType(EntryPoint.LSRedColor);
        genericSubMenu.Width = 0.5f;
        foreach (T genericItem in list)
        {
            UIMenu genericMenu = MenuPool.AddSubMenu(genericSubMenu, genericItem.ToString());
            genericMenu.SetBannerType(EntryPoint.LSRedColor);
            genericMenu.Width = 0.5f;
            PropertyInfo[] properties = genericItem.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string Description = property.Name;
                Description = Description.Substring(0, Math.Min(800, Description.Length));
                if (property.PropertyType == typeof(bool))
                {
                    UIMenuCheckboxItem MySetting = new UIMenuCheckboxItem(property.Name, (bool)property.GetValue(genericItem), Description);
                    MySetting.CheckboxEvent += (sender, Checked) =>
                    {
                        property.SetValue(genericItem, Checked);
                    };
                    genericMenu.AddItem(MySetting);
                }
                else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(string) || property.PropertyType == typeof(float) || property.PropertyType == typeof(uint) || property.PropertyType == typeof(Keys))
                {
                    UIMenuItem MySetting = new UIMenuItem($"{property.Name}: {property.GetValue(genericItem)}", Description);
                    MySetting.Activated += (sender, selectedItem) =>
                    {
                        UpdateSettings(sender, selectedItem, 0, genericItem.GetType().GetProperties(), genericItem);
                    };
                    genericMenu.AddItem(MySetting);
                }
            }
        }
    }

}