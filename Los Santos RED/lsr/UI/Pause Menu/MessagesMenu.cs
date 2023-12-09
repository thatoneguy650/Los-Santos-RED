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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

public class MessagesMenu
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
    private LocationsTab LocationsTab;
    private ContactsTab ContactsTab;
    private MessagesTab MessagesTab;
    private PhoneRepliesTab PhoneRepliesTab;
    private TextMessagesTab TextMessagesTab;
    private ContactsAddTab ContactAddTab;
    private ISettingsProvideable Settings;
    private IContacts Contacts;
    private List<ITabbableMenu> Tabs = new List<ITabbableMenu>();
    public MessagesMenu(IGangRelateable player, ITimeReportable time, IPlacesOfInterest placesOfInterest, IGangs gangs, IGangTerritories gangTerritories, IZones zones, IStreets streets, IInteriors interiors, IEntityProvideable world, ISettingsProvideable settings, IContacts contacts)
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
        Settings = settings;
        Contacts = contacts;
    }
    public void Setup()
    {
        tabView = new TabView("Los Santos ~r~RED~s~ Phone & Messages");
        tabView.Tabs.Clear();
        tabView.ScrollTabs = true;
        tabView.OnMenuClose += (s, e) =>
        {
            Player.ActivityManager.StopDynamicActivity();
            Game.IsPaused = false;
        };
        Game.RawFrameRender += (s, e) => tabView.DrawTextures(e.Graphics);


        LocationsTab = new LocationsTab(Player, PlacesOfInterest, Time, Settings, tabView, World);
        ContactsTab = new ContactsTab(Player, Gangs, tabView);
        MessagesTab = new MessagesTab(Player, tabView);
        PhoneRepliesTab = new PhoneRepliesTab(Player, tabView);
        TextMessagesTab = new TextMessagesTab(Player, tabView);
        ContactAddTab = new ContactsAddTab(Player, PlacesOfInterest, Time, Settings, tabView, Contacts, this);

        Tabs.Add(LocationsTab);
        Tabs.Add(ContactsTab);
        Tabs.Add(MessagesTab);
        Tabs.Add(PhoneRepliesTab);
        Tabs.Add(TextMessagesTab);
        Tabs.Add(ContactAddTab);
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
    public void RefreshMenu()
    {
        UpdateMenu();
    }
    private void UpdateMenu()
    {
        tabView.MoneySubtitle = Player.BankAccounts.TotalMoney.ToString("C0");
        tabView.Name = Player.PlayerName;
        tabView.Money = Time.CurrentTime;
        tabView.Tabs.Clear();


        foreach(ITabbableMenu tabbableMenu in Tabs)
        {
            tabbableMenu.AddItems();
        }
        //MessagesTab.AddItems();
        //ContactsTab.AddItems();
        //PhoneRepliesTab.AddItems();
        //TextMessagesTab.AddItems();
        //LocationsTab.AddItems();

        tabView.RefreshIndex();
    }
}