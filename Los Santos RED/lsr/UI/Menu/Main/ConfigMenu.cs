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

    private UIMenuItem ReloadSettingsFromFile;
    private UIMenuItem SaveSettingsToFile;
    private UIMenuItem DefaultSettings;

    public UIMenuItem LCConfigSettingsMenu { get; private set; }

    private UIMenuItem EasySettings;
    private UIMenuItem HardSettings;
    private UIMenu ConfigUIMenu;
    private UIMenuItem MySettings;

    private ISettingsProvideable SettingsProvider;
    private ICrimes Crimes;
    private IIntoxicants Intoxicants;
    private IShopMenus ShopMenus;

    public ConfigMenu(MenuPool menuPool, UIMenu parentMenu, ISettingsProvideable settingsProvideable, ICrimes crimes, IIntoxicants intoxicants, IShopMenus shopMenus, UI ui)
    {
        MenuPool = menuPool;
        ParentMenu = parentMenu;
        SettingsProvider = settingsProvideable;
        Crimes = crimes;
        Intoxicants = intoxicants;
        ShopMenus = shopMenus;
        UI = ui;
    }
    public void Setup()
    {
        ConfigUIMenu = MenuPool.AddSubMenu(ParentMenu, "Config Manager");
        ParentMenu.MenuItems[ParentMenu.MenuItems.Count() - 1].Description = "Change, Save, and Load Configs.";
        ParentMenu.MenuItems[ParentMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Crown;
        ConfigUIMenu.SetBannerType(EntryPoint.LSRedColor);
        CreateConfigManagerMenu();
        ConfigUIMenu.OnMenuOpen += ConfigUIMenu_OnMenuOpen;
    }

    private void ConfigUIMenu_OnMenuOpen(UIMenu sender)
    {
        if (SettingsProvider.IsBackendChanged || Crimes.IsBackendChanged)
        {
            CreateConfigManagerMenu();
            SettingsProvider.IsBackendChanged = false;
            Crimes.IsBackendChanged = false;
        }
        // Update();
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
        CreateConfigManagerMenu();
    }
    private void CreateConfigManagerMenu()
    {
        ConfigUIMenu.Clear();
        /*
        UIMenuItem ShowWorldSaveMenu = new UIMenuItem("World Saves", "Shows a list of the players worlds.");
        ShowWorldSaveMenu.RightBadge = UIMenuItem.BadgeStyle.Makeup;
        ShowWorldSaveMenu.Activated += (s, e) =>
        {
            UI.SavePauseMenu.Toggle();
            ConfigUIMenu.Visible = false;
        };
        ConfigUIMenu.AddItem(ShowWorldSaveMenu);*/

        UIMenuItem ShowGameConfigsMenu = new UIMenuItem("Configs", "Shows a list of configurations.");
        ShowGameConfigsMenu.RightBadge = UIMenuItem.BadgeStyle.Makeup;
        ShowGameConfigsMenu.Activated += (s, e) =>
        {
            UI.ConfigPauseMenu.Toggle();
            ConfigUIMenu.Visible = false;
        };
        ConfigUIMenu.AddItem(ShowGameConfigsMenu);
        /*

        UIMenu worldSubMenu = MenuPool.AddSubMenu(ConfigUIMenu, "Current Config"); // if no world loaded in, grey out option, and have bottom text read, modify current world
        worldSubMenu.SetBannerType(EntryPoint.LSRedColor);

        UIMenu worldConfigMenu = MenuPool.AddSubMenu(worldSubMenu, "Configuration");
        worldConfigMenu.SetBannerType(EntryPoint.LSRedColor);

        CreateToggleConfigurationMenu(worldConfigMenu);
        CreateToggleSettingsMenu(worldConfigMenu);
        UIMenuItem ShowEventLogMenu = new UIMenuItem("Event Log", "Shows the events and interactions for the ~r~Gangs~s~.");
        ShowEventLogMenu.RightBadge = UIMenuItem.BadgeStyle.Alert;
        ShowEventLogMenu.Activated += (s, e) =>
        {
            UI.MessagesMenu.Toggle();
            ConfigUIMenu.Visible = false;
        };
        worldSubMenu.AddItem(ShowEventLogMenu);

        worldSubMenu.OnItemSelect += OnItemSelect;
        worldSubMenu.OnListChange += OnListChange;


        ConfigUIMenu.OnItemSelect += OnItemSelect;
        ConfigUIMenu.OnListChange += OnListChange;*/
    }

    private void CreateToggleConfigurationMenu(UIMenu uiMenu)
    {
        UIMenu changesettingsMenu = MenuPool.AddSubMenu(uiMenu, "World Config");
        changesettingsMenu.SetBannerType(EntryPoint.LSRedColor);

        //UIMenu genericCategoryMenu = null;

        PropertyInfo[] properties = SettingsProvider.SettingsManager.GetType().GetProperties();
        List<Tuple<CategoryAttribute, UIMenu>> CategoryMenus = new List<Tuple<CategoryAttribute, UIMenu>>();

        List<CategoryAttribute> Categories = properties.Select(x => (System.ComponentModel.CategoryAttribute)x.GetCustomAttribute(typeof(CategoryAttribute), true)).Distinct().ToList();
        foreach (CategoryAttribute ca in Categories)//.OrderBy(x=> x.Category))
        {
            UIMenu categorysubMenu = MenuPool.AddSubMenu(changesettingsMenu, $"{ca.Category} SubMenu");
            categorysubMenu.SetBannerType(EntryPoint.LSRedColor);
            CategoryMenus.Add(new Tuple<CategoryAttribute, UIMenu>(ca, categorysubMenu));
        }

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

            UIMenu menuToAdd = null;
            if (propertyCategory != null)
            {
                UIMenu specificMenu = CategoryMenus.FirstOrDefault(x => x.Item1.Category == propertyCategory.Category)?.Item2;
                if (specificMenu != null)
                {
                    menuToAdd = specificMenu;
                }
            }
            if (menuToAdd == null)
            {
                continue;
            }
            subMenu = MenuPool.AddSubMenu(menuToAdd, strippedPropertyName);

            subMenu.SetBannerType(EntryPoint.LSRedColor);
            subMenu.OnItemSelect += OnNewSettingsSelect;
            subMenu.OnCheckboxChange += OnNewCheckboxChange;
            subMenu.Width = 0.5f;
            object SubSettings = property.GetValue(SettingsProvider.SettingsManager);
            PropertyInfo[] subSettings = property.PropertyType.GetProperties();
            object defSubSettings = property.GetValue(SettingsProvider.DefaultSettingsManager);

            foreach (PropertyInfo fi in subSettings)
            {
                System.ComponentModel.DescriptionAttribute coolio = (System.ComponentModel.DescriptionAttribute)fi.GetCustomAttribute(typeof(DescriptionAttribute), true);
                string Description = fi.Name;
                if (coolio != null)
                {
                    Description = coolio.Description;
                }
                Description = Description.Substring(0, Math.Min(750, Description.Length));
                if (fi.PropertyType == typeof(bool))
                {
                    Description += $"~n~Default: {(bool)fi.GetValue(defSubSettings)}";
                    UIMenuCheckboxItem MySetting = new UIMenuCheckboxItem(fi.Name, (bool)fi.GetValue(SubSettings), Description);
                    subMenu.AddItem(MySetting);
                }
                else if (fi.PropertyType == typeof(int) || fi.PropertyType == typeof(string) || fi.PropertyType == typeof(float) || fi.PropertyType == typeof(uint) || fi.PropertyType == typeof(Keys))
                {
                    Description += $"~n~Default: {fi.GetValue(defSubSettings)}";
                    UIMenuItem MySetting = new UIMenuItem($"{fi.Name}: {fi.GetValue(SubSettings)}", Description);
                    subMenu.AddItem(MySetting);
                }
            }
        }
        //genericCategoryMenu = MenuPool.AddSubMenu(changesettingsMenu, "Other SubMenu");
        // genericCategoryMenu.SetBannerType(EntryPoint.LSRedColor);
    }

    private void CreateToggleSettingsMenu(UIMenu uiMenu)
    {
        UIMenu changesettingsMenu = MenuPool.AddSubMenu(uiMenu, "Custom Settings");
        changesettingsMenu.SetBannerType(EntryPoint.LSRedColor);

        //UIMenu genericCategoryMenu = null;

        PropertyInfo[] properties = SettingsProvider.SettingsManager.GetType().GetProperties();
        List<Tuple<CategoryAttribute, UIMenu>> CategoryMenus = new List<Tuple<CategoryAttribute, UIMenu>>();

        List<CategoryAttribute> Categories = properties.Select(x => (System.ComponentModel.CategoryAttribute)x.GetCustomAttribute(typeof(CategoryAttribute), true)).Distinct().ToList();
        foreach (CategoryAttribute ca in Categories)//.OrderBy(x=> x.Category))
        {
            UIMenu categorysubMenu = MenuPool.AddSubMenu(changesettingsMenu, $"{ca.Category} SubMenu");
            categorysubMenu.SetBannerType(EntryPoint.LSRedColor);
            CategoryMenus.Add(new Tuple<CategoryAttribute, UIMenu>(ca, categorysubMenu));
        }

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

            UIMenu menuToAdd = null;
            if (propertyCategory != null)
            {
                UIMenu specificMenu = CategoryMenus.FirstOrDefault(x => x.Item1.Category == propertyCategory.Category)?.Item2;
                if (specificMenu != null)
                {
                    menuToAdd = specificMenu;
                }
            }
            if (menuToAdd == null)
            {
                continue;
            }
            subMenu = MenuPool.AddSubMenu(menuToAdd, strippedPropertyName);

            subMenu.SetBannerType(EntryPoint.LSRedColor);
            subMenu.OnItemSelect += OnNewSettingsSelect;
            subMenu.OnCheckboxChange += OnNewCheckboxChange;
            subMenu.Width = 0.5f;
            object SubSettings = property.GetValue(SettingsProvider.SettingsManager);
            PropertyInfo[] subSettings = property.PropertyType.GetProperties();
            object defSubSettings = property.GetValue(SettingsProvider.DefaultSettingsManager);

            foreach (PropertyInfo fi in subSettings)
            {
                System.ComponentModel.DescriptionAttribute coolio = (System.ComponentModel.DescriptionAttribute)fi.GetCustomAttribute(typeof(DescriptionAttribute), true);
                string Description = fi.Name;
                if (coolio != null)
                {
                    Description = coolio.Description;
                }
                Description = Description.Substring(0, Math.Min(750, Description.Length));
                if (fi.PropertyType == typeof(bool))
                {
                    Description += $"~n~Default: {(bool)fi.GetValue(defSubSettings)}";
                    UIMenuCheckboxItem MySetting = new UIMenuCheckboxItem(fi.Name, (bool)fi.GetValue(SubSettings), Description);
                    subMenu.AddItem(MySetting);
                }
                else if (fi.PropertyType == typeof(int) || fi.PropertyType == typeof(string) || fi.PropertyType == typeof(float) || fi.PropertyType == typeof(uint) || fi.PropertyType == typeof(Keys))
                {
                    Description += $"~n~Default: {fi.GetValue(defSubSettings)}";
                    UIMenuItem MySetting = new UIMenuItem($"{fi.Name}: {fi.GetValue(SubSettings)}", Description);
                    subMenu.AddItem(MySetting);
                }
            }
        }
        //genericCategoryMenu = MenuPool.AddSubMenu(changesettingsMenu, "Other SubMenu");
        // genericCategoryMenu.SetBannerType(EntryPoint.LSRedColor);
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
        else if (selectedItem == LCConfigSettingsMenu)
        {
            SettingsProvider.SetLC();
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
        ConfigUIMenu.Visible = false;
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

                //EntryPoint.WriteToConsole($"OnNewCheckboxChange property.Name {property} propertyName {propertyName}", 5);
                foreach (PropertyInfo fi in subSettings)
                {
                    if (propertyName == fi.Name)
                    {
                        fi.SetValue(SubSettings, Checked);
                        ConfigUIMenu.Visible = false;
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

                //EntryPoint.WriteToConsole($"OnNewSettingsSelect property.Name {property} propertyName {propertyName}", 5);
                foreach (PropertyInfo fi in subSettings)
                {
                    if (propertyName == fi.Name)
                    {
                        UpdateSettings(sender, selectedItem, index, SubSettings.GetType().GetProperties(), SubSettings);
                        ConfigUIMenu.Visible = false;
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
    private void CreateItemSubMenu<T>(string name, List<T> list)
    {
        UIMenu genericSubMenu = MenuPool.AddSubMenu(ConfigUIMenu, name);
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