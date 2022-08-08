using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Locations;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI.Elements;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public class PlayerInfoMenu
{
    private IGangs Gangs;
    private IGangTerritories GangTerritories;
    private IInteriors Interiors;
    private IPlacesOfInterest PlacesOfInterest;
    private IGangRelateable Player;
    private IStreets Streets;
    private TabView tabView;
    private ITimeReportable Time;
    private IEntityProvideable World;
    private IZones Zones;
    private IShopMenus ShopMenus;
    private IModItems ModItems;
    private IWeapons Weapons;

    private LocationsTab LocationsTab;
    private VehiclesTab VehiclesTab;
    private LicensesTab LicensesTab;
    private CrimesTab CrimesTab;
    private GangTab GangTab;

    private ISettingsProvideable Settings;

    public PlayerInfoMenu(IGangRelateable player, ITimeReportable time, IPlacesOfInterest placesOfInterest, IGangs gangs, IGangTerritories gangTerritories, IZones zones, IStreets streets, IInteriors interiors, IEntityProvideable world, IShopMenus shopMenus, IModItems modItems, IWeapons weapons, ISettingsProvideable settings)
    {
        Player = player;
        Time = time;
        PlacesOfInterest = placesOfInterest;
        Gangs = gangs;
        GangTerritories = gangTerritories;
        Zones = zones;
        Streets = streets;
        Interiors = interiors;
        World = world;
        ShopMenus = shopMenus;
        ModItems = modItems;
        Weapons = weapons;
        Settings = settings;
    }
    public void Setup()
    {
        tabView = new TabView("Los Santos ~r~RED~s~ Information");
        tabView.Tabs.Clear();
        tabView.ScrollTabs = true;
        tabView.OnMenuClose += (s, e) =>
        {
            Game.IsPaused = false;
        };
        LocationsTab = new LocationsTab(Player, PlacesOfInterest, Time, Settings, tabView);
        VehiclesTab = new VehiclesTab(Player, Streets, Zones, Interiors, tabView);
        LicensesTab = new LicensesTab(Player, Time, tabView);
        CrimesTab = new CrimesTab(Player, tabView);
        GangTab = new GangTab(Player,PlacesOfInterest,ShopMenus,ModItems,Weapons,GangTerritories,Zones, tabView);
    }
    public void Toggle()
    {
        if (!TabView.IsAnyPauseMenuVisible)
        {
            if (!tabView.Visible)
            {
                UpdateMenu();
                Game.IsPaused = true;
            }
            tabView.Visible = !tabView.Visible;
        }
    }
    public void Update()
    {
        tabView.Update();
        if (tabView.Visible)
        {
            tabView.Money = Time.CurrentDateTime.ToString("ddd, dd MMM yyyy hh:mm tt");
        }
    }
    private void UpdateMenu()
    {
        tabView.MoneySubtitle = Player.BankAccounts.Money.ToString("C0");
        tabView.Name = Player.PlayerName;
        tabView.Money = Time.CurrentTime;
        tabView.Tabs.Clear();

        VehiclesTab.AddItems();
        LicensesTab.AddItems();
        CrimesTab.AddItems();
        GangTab.AddItems();
        LocationsTab.AddItems();

        tabView.RefreshIndex();
    }
}