using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
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
    private UIMenu KeyBindingSettingsIUMenu;
    private UIMenu PoliceSettingsIUMenu;
    private UIMenu RespawnSettingsIUMenu;
    private UIMenu UISettingsIUMenu;
    private UIMenu DebugSettingsIUMenu;
    private UIMenu EMSSettingsIUMenu;
    private UIMenu FireSettingsIUMenu;
    private UIMenu DamageSettingsIUMenu;
    private UIMenu PedSwapSettingsIUMenu;
    private UIMenu CivilianSettingsIUMenu;
    private UIMenu WorldSettingsIUMenu;
    private UIMenu TimeSettingsIUMenu;
    private UIMenu VanillaSettingsIUMenu;
    private UIMenu ActivitySettingsIUMenu;

    public SettingsMenu(MenuPool menuPool, UIMenu parentMenu, IActionable player, IEntityProvideable world, ISettingsProvideable settingsProvideable)
    {
        MenuPool = menuPool;
        Player = player;
        World = world;
        SettingsProvider = settingsProvideable;
        SettingsUIMenu = menuPool.AddSubMenu(parentMenu, "Settings");
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
        //if(World.IsMPMapLoaded)
        //{
        //    MapToggle = new UIMenuItem("Load SP Map", "Load the SP Map (Default)");
        //}
        //else
        //{
        //    MapToggle = new UIMenuItem("Load MP Map", "Load the MP Map (For More Interiors)");
        //}
        ReloadSettingsFromFile = new UIMenuItem("Reload Settings", "Reloads the Settings XML");
        SaveSettingsToFile = new UIMenuItem("Save Settings", "Saves the Settings to XML");

        //SettingsUIMenu.AddItem(MapToggle);
        SettingsUIMenu.AddItem(ReloadSettingsFromFile);
        SettingsUIMenu.AddItem(SaveSettingsToFile);

        PlayerSettingsIUMenu = MenuPool.AddSubMenu(SettingsUIMenu, "Player Settings");
        PlayerSettingsIUMenu.SetBannerType(EntryPoint.LSRedColor);
        RespawnSettingsIUMenu = MenuPool.AddSubMenu(SettingsUIMenu, "Respawn Settings");
        RespawnSettingsIUMenu.SetBannerType(EntryPoint.LSRedColor);
        UISettingsIUMenu = MenuPool.AddSubMenu(SettingsUIMenu, "UI Settings");
        UISettingsIUMenu.SetBannerType(EntryPoint.LSRedColor);
        KeyBindingSettingsIUMenu = MenuPool.AddSubMenu(SettingsUIMenu, "Key Binding");
        KeyBindingSettingsIUMenu.SetBannerType(EntryPoint.LSRedColor);
        PoliceSettingsIUMenu = MenuPool.AddSubMenu(SettingsUIMenu, "Police Settings");
        PoliceSettingsIUMenu.SetBannerType(EntryPoint.LSRedColor);
        EMSSettingsIUMenu = MenuPool.AddSubMenu(SettingsUIMenu, "EMS Settings");
        EMSSettingsIUMenu.SetBannerType(EntryPoint.LSRedColor);
        FireSettingsIUMenu = MenuPool.AddSubMenu(SettingsUIMenu, "Fire Settings");
        FireSettingsIUMenu.SetBannerType(EntryPoint.LSRedColor);
        DamageSettingsIUMenu = MenuPool.AddSubMenu(SettingsUIMenu, "Damage Settings");
        DamageSettingsIUMenu.SetBannerType(EntryPoint.LSRedColor);
        PedSwapSettingsIUMenu = MenuPool.AddSubMenu(SettingsUIMenu, "PedSwap Settings");
        PedSwapSettingsIUMenu.SetBannerType(EntryPoint.LSRedColor);
        CivilianSettingsIUMenu = MenuPool.AddSubMenu(SettingsUIMenu, "Civilian Settings");
        CivilianSettingsIUMenu.SetBannerType(EntryPoint.LSRedColor);
        ActivitySettingsIUMenu = MenuPool.AddSubMenu(SettingsUIMenu, "Activity Settings");
        ActivitySettingsIUMenu.SetBannerType(EntryPoint.LSRedColor);
        WorldSettingsIUMenu = MenuPool.AddSubMenu(SettingsUIMenu, "World Settings");
        WorldSettingsIUMenu.SetBannerType(EntryPoint.LSRedColor);
        TimeSettingsIUMenu = MenuPool.AddSubMenu(SettingsUIMenu, "Time Settings");
        TimeSettingsIUMenu.SetBannerType(EntryPoint.LSRedColor);
        VanillaSettingsIUMenu = MenuPool.AddSubMenu(SettingsUIMenu, "Vanilla Settings");
        VanillaSettingsIUMenu.SetBannerType(EntryPoint.LSRedColor);
        DebugSettingsIUMenu = MenuPool.AddSubMenu(SettingsUIMenu, "Debug Settings");
        DebugSettingsIUMenu.SetBannerType(EntryPoint.LSRedColor);

        CreateSettingSubMenu(typeof(PlayerSettings).GetProperties(), SettingsProvider.SettingsManager.PlayerSettings, PlayerSettingsIUMenu);
        CreateSettingSubMenu(typeof(RespawnSettings).GetProperties(), SettingsProvider.SettingsManager.RespawnSettings, RespawnSettingsIUMenu);
        CreateSettingSubMenu(typeof(UISettings).GetProperties(), SettingsProvider.SettingsManager.UISettings, UISettingsIUMenu);
        CreateSettingSubMenu(typeof(KeySettings).GetProperties(), SettingsProvider.SettingsManager.KeySettings, KeyBindingSettingsIUMenu);
        CreateSettingSubMenu(typeof(PoliceSettings).GetProperties(), SettingsProvider.SettingsManager.PoliceSettings, PoliceSettingsIUMenu);
        CreateSettingSubMenu(typeof(EMSSettings).GetProperties(), SettingsProvider.SettingsManager.EMSSettings, EMSSettingsIUMenu);
        CreateSettingSubMenu(typeof(FireSettings).GetProperties(), SettingsProvider.SettingsManager.FireSettings, FireSettingsIUMenu);
        CreateSettingSubMenu(typeof(DamageSettings).GetProperties(), SettingsProvider.SettingsManager.DamageSettings, DamageSettingsIUMenu);
        CreateSettingSubMenu(typeof(PedSwapSettings).GetProperties(), SettingsProvider.SettingsManager.PedSwapSettings, PedSwapSettingsIUMenu);
        CreateSettingSubMenu(typeof(CivilianSettings).GetProperties(), SettingsProvider.SettingsManager.CivilianSettings, CivilianSettingsIUMenu);
        CreateSettingSubMenu(typeof(ActivitySettings).GetProperties(), SettingsProvider.SettingsManager.ActivitySettings, ActivitySettingsIUMenu);
        CreateSettingSubMenu(typeof(WorldSettings).GetProperties(), SettingsProvider.SettingsManager.WorldSettings, WorldSettingsIUMenu);
        CreateSettingSubMenu(typeof(TimeSettings).GetProperties(), SettingsProvider.SettingsManager.TimeSettings, TimeSettingsIUMenu);
        CreateSettingSubMenu(typeof(VanillaSettings).GetProperties(), SettingsProvider.SettingsManager.VanillaSettings, VanillaSettingsIUMenu);
        CreateSettingSubMenu(typeof(DebugSettings).GetProperties(), SettingsProvider.SettingsManager.DebugSettings, DebugSettingsIUMenu);

        SettingsUIMenu.OnItemSelect += OnItemSelect;
        SettingsUIMenu.OnListChange += OnListChange;

        PlayerSettingsIUMenu.OnItemSelect += OnPlayerSettingsSelect;
        RespawnSettingsIUMenu.OnItemSelect += OnRespawnSettingsSelect;
        UISettingsIUMenu.OnItemSelect += OnUISettingsSelect;
        KeyBindingSettingsIUMenu.OnItemSelect += OnKeyBindingsSelect;
        PoliceSettingsIUMenu.OnItemSelect += OnPoliceSettingsSelect;
        EMSSettingsIUMenu.OnItemSelect += OnEMSSettingsSelect;
        FireSettingsIUMenu.OnItemSelect += OnFireSettingsSelect;
        DamageSettingsIUMenu.OnItemSelect += OnDamageSettingsSelect;
        PedSwapSettingsIUMenu.OnItemSelect += OnPedSwapSettingsSelect;
        CivilianSettingsIUMenu.OnItemSelect += OnCivilianSettingsSelect;
        ActivitySettingsIUMenu.OnItemSelect += OnActivitySettingsSelect;
        WorldSettingsIUMenu.OnItemSelect += OnWorldSettingsSelect;
        TimeSettingsIUMenu.OnItemSelect += OnTimeSettingsSelect;
        VanillaSettingsIUMenu.OnItemSelect += OnVanillaSettingsSelect;
        DebugSettingsIUMenu.OnItemSelect += OnDebugSettingsSelect;


        PlayerSettingsIUMenu.OnCheckboxChange += OnCheckboxChange;
        RespawnSettingsIUMenu.OnCheckboxChange += OnCheckboxChange;
        UISettingsIUMenu.OnCheckboxChange += OnCheckboxChange;
        KeyBindingSettingsIUMenu.OnCheckboxChange += OnCheckboxChange;
        PoliceSettingsIUMenu.OnCheckboxChange += OnCheckboxChange;
        EMSSettingsIUMenu.OnCheckboxChange += OnCheckboxChange;
        FireSettingsIUMenu.OnCheckboxChange += OnCheckboxChange;
        DamageSettingsIUMenu.OnCheckboxChange += OnCheckboxChange;
        PedSwapSettingsIUMenu.OnCheckboxChange += OnCheckboxChange;
        CivilianSettingsIUMenu.OnCheckboxChange += OnCheckboxChange;
        ActivitySettingsIUMenu.OnCheckboxChange += OnCheckboxChange;
        WorldSettingsIUMenu.OnCheckboxChange += OnCheckboxChange;
        TimeSettingsIUMenu.OnCheckboxChange += OnCheckboxChange;
        VanillaSettingsIUMenu.OnCheckboxChange += OnCheckboxChange;
        DebugSettingsIUMenu.OnCheckboxChange += OnCheckboxChange;

    }

    private void OnPlayerSettingsSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        UpdateSettings(sender, selectedItem, index, typeof(PlayerSettings).GetProperties(), SettingsProvider.SettingsManager.PlayerSettings);
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
    private void OnEMSSettingsSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        UpdateSettings(sender, selectedItem, index, typeof(EMSSettings).GetProperties(), SettingsProvider.SettingsManager.EMSSettings);
        SettingsUIMenu.Visible = false;
    }
    private void OnFireSettingsSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        UpdateSettings(sender, selectedItem, index, typeof(FireSettings).GetProperties(), SettingsProvider.SettingsManager.FireSettings);
        SettingsUIMenu.Visible = false;
    }
    private void OnDamageSettingsSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        UpdateSettings(sender, selectedItem, index, typeof(DamageSettings).GetProperties(), SettingsProvider.SettingsManager.DamageSettings);
        SettingsUIMenu.Visible = false;
    }
    private void OnPedSwapSettingsSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        UpdateSettings(sender, selectedItem, index, typeof(PedSwapSettings).GetProperties(), SettingsProvider.SettingsManager.PedSwapSettings);
        SettingsUIMenu.Visible = false;
    }
    private void OnCivilianSettingsSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        UpdateSettings(sender, selectedItem, index, typeof(CivilianSettings).GetProperties(), SettingsProvider.SettingsManager.CivilianSettings);
        SettingsUIMenu.Visible = false;
    }
    private void OnActivitySettingsSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        UpdateSettings(sender, selectedItem, index, typeof(ActivitySettings).GetProperties(), SettingsProvider.SettingsManager.ActivitySettings);
        SettingsUIMenu.Visible = false;
    }
    private void OnWorldSettingsSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        UpdateSettings(sender, selectedItem, index, typeof(WorldSettings).GetProperties(), SettingsProvider.SettingsManager.WorldSettings);
        SettingsUIMenu.Visible = false;
    }
    private void OnTimeSettingsSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        UpdateSettings(sender, selectedItem, index, typeof(TimeSettings).GetProperties(), SettingsProvider.SettingsManager.TimeSettings);
        SettingsUIMenu.Visible = false;
    }
    private void OnVanillaSettingsSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        UpdateSettings(sender, selectedItem, index, typeof(VanillaSettings).GetProperties(), SettingsProvider.SettingsManager.VanillaSettings);
        SettingsUIMenu.Visible = false;
    }
    private void OnDebugSettingsSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        UpdateSettings(sender, selectedItem, index, typeof(DebugSettings).GetProperties(), SettingsProvider.SettingsManager.DebugSettings);
        SettingsUIMenu.Visible = false;
    }
    private void OnCheckboxChange(UIMenu sender, UIMenuCheckboxItem checkbox, bool Checked)
    {
        if (sender == PlayerSettingsIUMenu)
        {
            PropertyInfo[] MyFields = typeof(PlayerSettings).GetProperties();
            PropertyInfo MySetting = MyFields.Where(x => x.Name == checkbox.Text).FirstOrDefault();
            MySetting.SetValue(SettingsProvider.SettingsManager.PlayerSettings, Checked);
        }
        else if (sender == RespawnSettingsIUMenu)
        {
            PropertyInfo[] MyFields = typeof(RespawnSettings).GetProperties();
            PropertyInfo MySetting = MyFields.Where(x => x.Name == checkbox.Text).FirstOrDefault();
            MySetting.SetValue(SettingsProvider.SettingsManager.RespawnSettings, Checked);
        }
        else if (sender == UISettingsIUMenu)
        {
            PropertyInfo[] MyFields = typeof(UISettings).GetProperties();
            PropertyInfo MySetting = MyFields.Where(x => x.Name == checkbox.Text).FirstOrDefault();
            MySetting.SetValue(SettingsProvider.SettingsManager.UISettings, Checked);
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
        else if (sender == EMSSettingsIUMenu)
        {
            PropertyInfo[] MyFields = typeof(EMSSettings).GetProperties();
            PropertyInfo MySetting = MyFields.Where(x => x.Name == checkbox.Text).FirstOrDefault();
            MySetting.SetValue(SettingsProvider.SettingsManager.EMSSettings, Checked);
        }
        else if (sender == FireSettingsIUMenu)
        {
            PropertyInfo[] MyFields = typeof(FireSettings).GetProperties();
            PropertyInfo MySetting = MyFields.Where(x => x.Name == checkbox.Text).FirstOrDefault();
            MySetting.SetValue(SettingsProvider.SettingsManager.FireSettings, Checked);
        }
        else if (sender == DamageSettingsIUMenu)
        {
            PropertyInfo[] MyFields = typeof(DamageSettings).GetProperties();
            PropertyInfo MySetting = MyFields.Where(x => x.Name == checkbox.Text).FirstOrDefault();
            MySetting.SetValue(SettingsProvider.SettingsManager.DamageSettings, Checked);
        }
        else if (sender == PedSwapSettingsIUMenu)
        {
            PropertyInfo[] MyFields = typeof(PedSwapSettings).GetProperties();
            PropertyInfo MySetting = MyFields.Where(x => x.Name == checkbox.Text).FirstOrDefault();
            MySetting.SetValue(SettingsProvider.SettingsManager.PedSwapSettings, Checked);
        }
        else if (sender == CivilianSettingsIUMenu)
        {
            PropertyInfo[] MyFields = typeof(CivilianSettings).GetProperties();
            PropertyInfo MySetting = MyFields.Where(x => x.Name == checkbox.Text).FirstOrDefault();
            MySetting.SetValue(SettingsProvider.SettingsManager.CivilianSettings, Checked);
        }
        else if (sender == ActivitySettingsIUMenu)
        {
            PropertyInfo[] MyFields = typeof(ActivitySettings).GetProperties();
            PropertyInfo MySetting = MyFields.Where(x => x.Name == checkbox.Text).FirstOrDefault();
            MySetting.SetValue(SettingsProvider.SettingsManager.ActivitySettings, Checked);
        }
        else if (sender == WorldSettingsIUMenu)
        {
            PropertyInfo[] MyFields = typeof(WorldSettings).GetProperties();
            PropertyInfo MySetting = MyFields.Where(x => x.Name == checkbox.Text).FirstOrDefault();
            MySetting.SetValue(SettingsProvider.SettingsManager.WorldSettings, Checked);
        }
        else if (sender == TimeSettingsIUMenu)
        {
            PropertyInfo[] MyFields = typeof(TimeSettings).GetProperties();
            PropertyInfo MySetting = MyFields.Where(x => x.Name == checkbox.Text).FirstOrDefault();
            MySetting.SetValue(SettingsProvider.SettingsManager.TimeSettings, Checked);
        }
        else if (sender == VanillaSettingsIUMenu)
        {
            PropertyInfo[] MyFields = typeof(VanillaSettings).GetProperties();
            PropertyInfo MySetting = MyFields.Where(x => x.Name == checkbox.Text).FirstOrDefault();
            MySetting.SetValue(SettingsProvider.SettingsManager.VanillaSettings, Checked);
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
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == MapToggle)
        {
            if (World.IsMPMapLoaded)
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

}