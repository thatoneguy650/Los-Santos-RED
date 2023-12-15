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

public class SavePauseMenu
{
    private IGangs Gangs;
    private IGangTerritories GangTerritories;
    private IInteriors Interiors;
    private IPlacesOfInterest PlacesOfInterest;
    private ISaveable Player;
    private IStreets Streets;
    private TabView tabView;
    private ITimeControllable Time;
    private IEntityProvideable World;
    private IZones Zones;
    private IShopMenus ShopMenus;
    private IModItems ModItems;
    private IWeapons Weapons;
    private ILocationTypes Counties;
    private IGameSaves GameSaves;
    private IPedSwap PedSwap;
    private IInventoryable Inventoryable;
    private SaveGameTab NewSaveGameTab;
    private ISaveable Saveable;
    private ISettingsProvideable Settings;
    private IAgencies Agencies;
    private IContacts Contacts;
    private IInteractionable Interactionable;
    public SavePauseMenu(ISaveable player, ITimeControllable time, IPlacesOfInterest placesOfInterest, IGangs gangs, IGangTerritories gangTerritories, IZones zones, IStreets streets, 
        IInteriors interiors, IEntityProvideable world, IShopMenus shopMenus, IModItems modItems, IWeapons weapons, ISettingsProvideable settings, IGameSaves gameSaves,
        IPedSwap pedSwap, IInventoryable inventoryable, ISaveable saveable, IAgencies agencies, IContacts contacts, IInteractionable interactionable)
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
        GameSaves = gameSaves;
        PedSwap = pedSwap;
        Inventoryable = inventoryable;
        Saveable = saveable;
        Agencies = agencies;
        Contacts = contacts;
        Interactionable = interactionable;
    }
    public void Setup()
    {
        tabView = new TabView("Los Santos ~r~RED~s~ Game Save Manager");
        tabView.Tabs.Clear();
        tabView.ScrollTabs = true;
        tabView.OnMenuClose += (s, e) =>
        {
            Game.IsPaused = false;
        };
        Game.RawFrameRender += (s, e) => tabView.DrawTextures(e.Graphics);
        NewSaveGameTab = new SaveGameTab(Player, PlacesOfInterest, ShopMenus, ModItems, Weapons, GangTerritories, Zones, tabView, Time, Settings, GameSaves, Gangs, PedSwap, Inventoryable, World, Saveable, Agencies, Contacts, Interactionable);
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
        tabView.MoneySubtitle = Player.BankAccounts.TotalMoney.ToString("C0");
        tabView.Name = Player.PlayerName;
        tabView.Money = Time.CurrentTime;
        tabView.Tabs.Clear();

        NewSaveGameTab.AddLoadItems();
        NewSaveGameTab.AddSaveItems();
        NewSaveGameTab.AddDeleteItems();

        tabView.RefreshIndex();
    }
}