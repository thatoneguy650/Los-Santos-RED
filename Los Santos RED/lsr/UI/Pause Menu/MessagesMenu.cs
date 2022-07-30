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

public class MessagesMenu
{
    private TabSubmenuItem ContactsSubMenu;
    private IGangs Gangs;
    private TabSubmenuItem GangsSubMenu;
    private IGangTerritories GangTerritories;
    private IInteriors Interiors;
    private IPlacesOfInterest PlacesOfInterest;
    private IGangRelateable Player;
    private IStreets Streets;
    private TabView tabView;
    private TabSubmenuItem TextMessagesSubMenu;
    private ITimeReportable Time;
    private TabSubmenuItem VehiclesSubMenu;
    private IEntityProvideable World;
    private IZones Zones;
    private TabSubmenuItem PhoneRepliesSubMenu;
    private TabSubmenuItem LicensesSubMenu;
    private List<BasicLocation> SearchResultLocations = new List<BasicLocation>();
    private UIMenuItem SearchLocationByName;
    private UIMenuListScrollerItem<BasicLocation> LocationResults;

    public MessagesMenu(IGangRelateable player, ITimeReportable time, IPlacesOfInterest placesOfInterest, IGangs gangs, IGangTerritories gangTerritories, IZones zones, IStreets streets, IInteriors interiors, IEntityProvideable world)
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
    }
    public void Setup()
    {
        tabView = new TabView("Los Santos ~r~RED~s~ Phone & Replies");
        tabView.Tabs.Clear();
        tabView.ScrollTabs = true;
        tabView.OnMenuClose += TabView_OnMenuClose;
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
    private void AddContacts()
    {
        List<TabItem> items = new List<TabItem>();
        bool addedItems = false;
        foreach (PhoneContact contact in Player.CellPhone.ContactList.OrderBy(x => x.Name))
        {
            string DescriptionText = "Select to ~o~Call~s~ the contact";
            string Title = contact.Name;
            string SubTitle = contact.Name;
            Gang myGang = Gangs.GetGangByContact(contact.Name);
            if (myGang != null)
            {
                GangReputation gr = Player.GangRelationships.GangReputations.FirstOrDefault(x => x.Gang.ID == myGang.ID);
                if (gr != null)
                {
                    Title = $"{contact.Name} {gr.ToBlip()}~s~";
                    SubTitle = $"{gr.Gang.ColorPrefix}{contact.Name}~s~";
                }
            }

            PlayerTask contactTask = Player.PlayerTasks.GetTask(contact.Name);
            if (contactTask != null)
            {
                DescriptionText += $"~n~~g~Has Task~s~";
                if (contactTask.CanExpire)
                {
                    DescriptionText += $" Complete Before: ~r~{contactTask.ExpireTime:d} {contactTask.ExpireTime:t}~s~";
                }
            }


            TabItem tabItem = new TabTextItem(Title, SubTitle, DescriptionText);//TabItem tabItem = new TabTextItem($"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~ {gr.ToBlip()}~s~", $"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~", DescriptionText);
            tabItem.Activated += (s, e) =>
            {
                //Game.DisplaySubtitle("Activated Submenu Item #" + ContactsSubMenu.Index + " "+ contact.Name, 5000);
                tabView.Visible = false;
                Game.IsPaused = false;
                Player.CellPhone.ContactAnswered(contact);
            };
            items.Add(tabItem);
            addedItems = true;
        }
        if (addedItems)
        {
            tabView.AddTab(ContactsSubMenu = new TabSubmenuItem("Contacts", items));
        }
        else
        {
            tabView.AddTab(new TabItem("Contacts"));
        }
    }
    private void AddLocations()
    {
        List<UIMenuItem> menuItems = new List<UIMenuItem>();
        menuItems.Add(new UIMenuItem("Remove GPS Route", "Remove any enabled GPS Blip"));
        SearchLocationByName = new UIMenuItem("Search For Location", "Search for a location");
        menuItems.Add(SearchLocationByName);
        LocationResults = new UIMenuListScrollerItem<BasicLocation>("Search Results", "Results of the search", SearchResultLocations) { Formatter = sy => $"{sy.Name} - " + $"{sy.FullStreetAddress}".Trim() };
        menuItems.Add(LocationResults);
        List<BasicLocation> Residences = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.Residences.Any(x => x.IsOwnedOrRented))
        {
            foreach (Residence res in PlacesOfInterest.PossibleLocations.Residences.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                if (res.IsOwnedOrRented)
                {
                    Residences.Add(res);
                }

            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Residences", $"List of all owned or rented residences", Residences) { Formatter = sy => $"{sy.Name} - " + $"{sy.FullStreetAddress}".Trim() });
        }
        List<BasicLocation> ForSaleResidences = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.Residences.Any(x => !x.IsOwnedOrRented))
        {
            foreach (Residence residence in PlacesOfInterest.PossibleLocations.Residences.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                if (!residence.IsOwnedOrRented)
                {
                    ForSaleResidences.Add(residence);
                }
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("For Sale/Rental", $"List of all residences that are for sale or rental", ForSaleResidences) { Formatter = sy => $"{sy.Name} - " + $"{sy.FullStreetAddress}".Trim() });
        }
        List<BasicLocation> Hotels = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.Hotels.Any())
        {
            foreach (Hotel hotel in PlacesOfInterest.PossibleLocations.Hotels.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                Hotels.Add(hotel);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Hotels", $"List of all Hotels", Hotels) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.FullStreetAddress}".Trim() });
        }
        List<BasicLocation> Banks = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.Banks.Any())
        {
            foreach (Bank Bank in PlacesOfInterest.PossibleLocations.Banks.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                Banks.Add(Bank);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Banks", $"List of all Banks", Banks) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.FullStreetAddress}".Trim() });
        }
        List<BasicLocation> BeautyShops = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.BeautyShops.Any())
        {
            foreach (BeautyShop shop in PlacesOfInterest.PossibleLocations.BeautyShops.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                BeautyShops.Add(shop);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Beauty Shops", $"List of all BeautyShops", BeautyShops) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.FullStreetAddress}".Trim() });
        }
        List<BasicLocation> Dispensaries = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.Dispensaries.Any())
        {
            foreach (Dispensary shop in PlacesOfInterest.PossibleLocations.Dispensaries.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                Dispensaries.Add(shop);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Dispensaries", $"List of all Dispensaries", Dispensaries) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.FullStreetAddress}".Trim() });
        }
        List<BasicLocation> HardwareStores = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.HardwareStores.Any())
        {
            foreach (HardwareStore shop in PlacesOfInterest.PossibleLocations.HardwareStores.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                HardwareStores.Add(shop);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Hardward Stores", $"List of all Hardward Stores", HardwareStores) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.FullStreetAddress}".Trim() });
        }
        List<BasicLocation> HeadShops = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.HeadShops.Any())
        {
            foreach (HeadShop shop in PlacesOfInterest.PossibleLocations.HeadShops.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                HeadShops.Add(shop);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Head Shops", $"List of all Head Shops", HeadShops) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.FullStreetAddress}".Trim() });
        }
        List<BasicLocation> PawnShops = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.PawnShops.Any())
        {
            foreach (PawnShop shop in PlacesOfInterest.PossibleLocations.PawnShops.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                PawnShops.Add(shop);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Pawn Shops", $"List of all Pawn Shops", PawnShops) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.FullStreetAddress}".Trim() });
        }
        List<BasicLocation> Pharmacies = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.Pharmacies.Any())
        {
            foreach (Pharmacy shop in PlacesOfInterest.PossibleLocations.Pharmacies.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                Pharmacies.Add(shop);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Pharmacies", $"List of all Pharmacies", Pharmacies) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.FullStreetAddress}".Trim() });
        }
        List<BasicLocation> Stadiums = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.Stadiums.Any())
        {
            foreach (Stadium shop in PlacesOfInterest.PossibleLocations.Stadiums.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                Stadiums.Add(shop);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Stadiums", $"List of all Stadiums", Stadiums) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.FullStreetAddress}".Trim() });
        }
        List<BasicLocation> ConvenienceStores = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.ConvenienceStores.Any())
        {
            foreach (ConvenienceStore shop in PlacesOfInterest.PossibleLocations.ConvenienceStores.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                ConvenienceStores.Add(shop);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Convenience Stores", $"List of all Convenience Stores", ConvenienceStores) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.FullStreetAddress}".Trim() });
        }
        List<BasicLocation> ScrapYards = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.ScrapYards.Any())
        {
            foreach (ScrapYard sy in PlacesOfInterest.PossibleLocations.ScrapYards.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                ScrapYards.Add(sy);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Scrap Yards", $"List of all Scrap Yards", ScrapYards) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.FullStreetAddress}".Trim() });
        }
        List<BasicLocation> CityHalls = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.ScrapYards.Any())
        {
            foreach (CityHall sy in PlacesOfInterest.PossibleLocations.CityHalls.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                CityHalls.Add(sy);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("City Halls", $"List of all City Halls", CityHalls) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.FullStreetAddress}".Trim() });
        }
        List<BasicLocation> PoliceStations = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.PoliceStations.Any())
        {
            foreach (PoliceStation sy in PlacesOfInterest.PossibleLocations.PoliceStations.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                PoliceStations.Add(sy);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Police Stations", $"List of all Police Stations", PoliceStations) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.FullStreetAddress}".Trim() });
        }
        List<BasicLocation> FireStations = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.PoliceStations.Any())
        {
            foreach (FireStation sy in PlacesOfInterest.PossibleLocations.FireStations.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                FireStations.Add(sy);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Fire Stations", $"List of all Fire Stations", FireStations) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.FullStreetAddress}".Trim() });
        }
        List<BasicLocation> Hospitals = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.Hospitals.Any())
        {
            foreach (Hospital sy in PlacesOfInterest.PossibleLocations.Hospitals.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                Hospitals.Add(sy);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Hospitals", $"List of all Hospitals", Hospitals) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.FullStreetAddress}".Trim() });
        }
        List<BasicLocation> Restaurants = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.Restaurants.Any())
        {
            foreach (Restaurant sy in PlacesOfInterest.PossibleLocations.Restaurants.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                Restaurants.Add(sy);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Restaurants", $"List of all Restaurants", Restaurants) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.FullStreetAddress}".Trim() });
        }
        List<BasicLocation> LiquorStores = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.Restaurants.Any())
        {
            foreach (LiquorStore sy in PlacesOfInterest.PossibleLocations.LiquorStores.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                LiquorStores.Add(sy);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("LiquorStores", $"List of all LiquorStores", LiquorStores) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.FullStreetAddress}".Trim() });
        }
        List<BasicLocation> GasStations = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.GasStations.Any())
        {
            foreach (GasStation sy in PlacesOfInterest.PossibleLocations.GasStations.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                GasStations.Add(sy);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("GasStations", $"List of all GasStations", GasStations) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.FullStreetAddress}".Trim() });
        }
        List<BasicLocation> Bars = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.Bars.Any())
        {
            foreach (Bar sy in PlacesOfInterest.PossibleLocations.Bars.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                Bars.Add(sy);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Bars", $"List of all Bars", Bars) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.FullStreetAddress}".Trim() });
        }
        List<BasicLocation> FoodStands = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.FoodStands.Any())
        {
            foreach (FoodStand sy in PlacesOfInterest.PossibleLocations.FoodStands.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                FoodStands.Add(sy);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("FoodStands", $"List of all FoodStands", FoodStands) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.FullStreetAddress}".Trim() });
        }
        List<BasicLocation> CarDealerships = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.CarDealerships.Any())
        {
            foreach (CarDealership sy in PlacesOfInterest.PossibleLocations.CarDealerships.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                CarDealerships.Add(sy);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("CarDealerships", $"List of all CarDealerships", CarDealerships) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.FullStreetAddress}".Trim() });
        }
        List<BasicLocation> DriveThrus = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.DriveThrus.Any())
        {
            foreach (DriveThru sy in PlacesOfInterest.PossibleLocations.DriveThrus.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                DriveThrus.Add(sy);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("DriveThrus", $"List of all DriveThrus", DriveThrus) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.FullStreetAddress}".Trim() });
        }
        TabInteractiveListItem interactiveListItem = new TabInteractiveListItem("Locations", menuItems);
        interactiveListItem.BackingMenu.OnItemSelect += BackingMenu_OnItemSelect;
        tabView.AddTab(interactiveListItem);
    }
    private void AddMessages()
    {
        List<TabItem> items = new List<TabItem>();
        bool addedItems = false;

        List<Tuple<string,DateTime>> MessageTimes = new List<Tuple<string, DateTime>>();

        MessageTimes.AddRange(Player.CellPhone.PhoneResponseList.OrderByDescending(x => x.TimeReceived).Take(15).Select(x => new Tuple<string, DateTime>(x.ContactName, x.TimeReceived)));
        MessageTimes.AddRange(Player.CellPhone.TextList.OrderByDescending(x => x.TimeReceived).Take(15).Select(x => new Tuple<string, DateTime>(x.ContactName, x.TimeReceived)));

        foreach (Tuple<string, DateTime> dateTime in MessageTimes.OrderByDescending(x=> x.Item2).Take(15))
        {
            PhoneResponse pr = Player.CellPhone.PhoneResponseList.Where(x => x.TimeReceived == dateTime.Item2 && x.ContactName == dateTime.Item1).FirstOrDefault();
            if(pr != null)
            {
                string TimeReceived = pr.TimeReceived.ToString("HH:mm");
                string DescriptionText = "";
                DescriptionText += $"~n~Received At: {TimeReceived}";
                DescriptionText += $"~n~{pr.Message}";
                string ListEntryItem = $"{pr.ContactName} {TimeReceived}";
                string DescriptionHeaderText = $"{pr.ContactName}";
                TabItem tItem = new TabTextItem(ListEntryItem, DescriptionHeaderText, DescriptionText);
                items.Add(tItem);
                addedItems = true;
            }
            PhoneText text = Player.CellPhone.TextList.Where(x => x.TimeReceived == dateTime.Item2 && x.ContactName == dateTime.Item1).FirstOrDefault();
            if (text != null)
            {
                string TimeReceived = text.HourSent.ToString("00") + ":" + text.MinuteSent.ToString("00");// string.Format("{0:D2}h:{1:D2}m",text.HourSent,text.MinuteSent);
                string DescriptionText = "";
                DescriptionText += $"~n~Received At: {TimeReceived}";  //+ gr.ToStringBare();
                DescriptionText += $"~n~{text.Message}";
                string ListEntryItem = $"{text.ContactName}{(!text.IsRead ? " *" : "")} {TimeReceived}";
                string DescriptionHeaderText = $"{text.ContactName}";
                TabItem tItem = new TabTextItem(ListEntryItem, DescriptionHeaderText, DescriptionText);
                items.Add(tItem);
                addedItems = true;
            }
        }
        PhoneRepliesSubMenu = new TabSubmenuItem("Recent", items);
        if (addedItems)
        {
            tabView.AddTab(PhoneRepliesSubMenu);
        }
        else
        {
            tabView.AddTab(new TabItem("Recent"));
        }
    }
    private void AddPhoneRepliesMessages()
    {
        List<TabItem> items = new List<TabItem>();
        bool addedItems = false;
        foreach (PhoneResponse text in Player.CellPhone.PhoneResponseList.OrderByDescending(x => x.TimeReceived).Take(15))
        {
            string TimeReceived = text.TimeReceived.ToString("HH:mm");// text.HourSent.ToString("00") + ":" + text.MinuteSent.ToString("00");// string.Format("{0:D2}h:{1:D2}m",text.HourSent,text.MinuteSent);
            string DescriptionText = "";
            DescriptionText += $"~n~Received At: {TimeReceived}";  //+ gr.ToStringBare();
            DescriptionText += $"~n~{text.Message}";
            //DescriptionText += $"~n~~n~Select to ~r~Delete Response~s~";
            string ListEntryItem = $"{text.ContactName} {TimeReceived}";
            string DescriptionHeaderText = $"{text.ContactName}";
            TabItem tItem = new TabTextItem(ListEntryItem, DescriptionHeaderText, DescriptionText);
            //tItem.Activated += (s, e) =>
            //{
            //    if (text != null)
            //    {
            //        Player.CellPhone.DeletePhoneRespone(text);
            //        PhoneRepliesSubMenu.Items.Remove(tItem);
            //        PhoneRepliesSubMenu.RefreshIndex();
            //        EntryPoint.WriteToConsole($"Phone Reply deleted {text.ContactName} {text.Message}");
            //    }
            //};
            items.Add(tItem);
            addedItems = true;
        }
        TabItem ClearPhoneResponses = new TabTextItem("Clear Phone Responses", "Clear Phone Responses", "Select to clear all ~o~Phone Responses~s~");//TabItem tabItem = new TabTextItem($"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~ {gr.ToBlip()}~s~", $"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~", DescriptionText);
        ClearPhoneResponses.Activated += (s, e) =>
        {
            tabView.Visible = false;
            Game.IsPaused = false;
            Player.CellPhone.ClearPhoneResponses();
        };
        items.Add(ClearPhoneResponses);

        PhoneRepliesSubMenu = new TabSubmenuItem("Replies", items);

        if (addedItems)
        {
            tabView.AddTab(PhoneRepliesSubMenu);
        }
        else
        {
            tabView.AddTab(new TabItem("Replies"));
        }
    }
    private void AddTextMessages()
    {
        List<TabItem> items = new List<TabItem>();
        bool addedItems = false;
        foreach (PhoneText text in Player.CellPhone.TextList.OrderByDescending(x => x.TimeReceived).Take(15))
        {
            string TimeReceived = text.HourSent.ToString("00") + ":" + text.MinuteSent.ToString("00");// string.Format("{0:D2}h:{1:D2}m",text.HourSent,text.MinuteSent);

            string DescriptionText = "";
            DescriptionText += $"~n~Received At: {TimeReceived}";  //+ gr.ToStringBare();
            DescriptionText += $"~n~{text.Message}";
            //DescriptionText += $"~n~~n~Select to ~r~Delete Message~s~";
            string ListEntryItem = $"{text.ContactName}{(!text.IsRead ? " *" : "")} {TimeReceived}";
            string DescriptionHeaderText = $"{text.ContactName}";
            TabItem tItem = new TabTextItem(ListEntryItem, DescriptionHeaderText, DescriptionText);
            //tItem.Activated += (s, e) =>
            //{
            //    if (text != null)
            //    {
            //        Player.CellPhone.DeleteText(text);
            //        TextMessagesSubMenu.Items.Remove(tItem);
            //        TextMessagesSubMenu.RefreshIndex();
            //        EntryPoint.WriteToConsole($"Text Message deleted {text.Name} {text.Message}");
            //    }
            //};
            items.Add(tItem);
            addedItems = true;
        }

        TabItem ClearTexts = new TabTextItem("Clear Text Messages", "Clear Text Messages", "Select to clear all ~o~Text Messages~s~");//TabItem tabItem = new TabTextItem($"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~ {gr.ToBlip()}~s~", $"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~", DescriptionText);
        ClearTexts.Activated += (s, e) =>
        {
            tabView.Visible = false;
            Game.IsPaused = false;
            Player.CellPhone.ClearTextMessages();
        };
        items.Add(ClearTexts);



        TextMessagesSubMenu = new TabSubmenuItem("Texts", items);

        if (addedItems)
        {
            tabView.AddTab(TextMessagesSubMenu);
        }
        else
        {
            tabView.AddTab(new TabItem("Texts"));
        }
    }
    private void BackingMenu_OnItemSelect(RAGENativeUI.UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == SearchLocationByName)
        {
            string text1 = NativeHelper.GetKeyboardInput("");
            SearchResultLocations = PlacesOfInterest.GetAllLocations().Where(x => x.Name.ToLower().Contains(text1.ToLower())).ToList();
            LocationResults.Items = SearchResultLocations;
            LocationResults.Reformat();
        }
        else if (selectedItem.GetType() == typeof(UIMenuListScrollerItem<BasicLocation>))
        {
            UIMenuListScrollerItem<BasicLocation> myItem = (UIMenuListScrollerItem<BasicLocation>)selectedItem;
            if (selectedItem != null && myItem != null && myItem.Items.Any() && myItem.SelectedItem != null)
            {
                Player.Destinations.AddGPSRoute(myItem.SelectedItem.Name, myItem.SelectedItem.EntrancePosition);
            }
        }
        else if (selectedItem.Text == "Remove GPR Route")
        {
            Player.Destinations.RemoveGPSRoute();
        }
    }
    private string GetSafeLocationName(LocationType lt)
    {
        switch (lt)
        {
            case LocationType.BeautyShop:
                return "Beauty Shop";
            case LocationType.CarDealer:
                return "Car Dealership";
            case LocationType.ConvenienceStore:
                return "Convenience Store";
            case LocationType.DriveThru:
                return "Drive-Thru";
            case LocationType.GasStation:
                return "Gas Station";
            case LocationType.HardwareStore:
                return "Hardware Store";
            case LocationType.LiquorStore:
                return "Liquor Store";
            case LocationType.PawnShop:
                return "Pawn Shop";
            case LocationType.StripClub:
                return "Strip Club";
            case LocationType.VendingMachine:
                return "Vending Machine";
            default:
                return lt.ToString();
        }
    }
    private bool IsValidLocationType(LocationType lt)
    {
        if (lt == LocationType.BeautyShop || lt == LocationType.Garage || lt == LocationType.VendingMachine || lt == LocationType.Stadium || lt == LocationType.Grave || lt == LocationType.BusStop || lt == LocationType.DrugDealer || lt == LocationType.Other)
        {
            return false;
        }
        return true;
    }
    private void TabView_OnMenuClose(object sender, EventArgs e)
    {
        Player.StopDynamicActivity();
        Game.IsPaused = false;
    }
    private void UpdateMenu()
    {
        tabView.MoneySubtitle = Player.BankAccounts.Money.ToString("C0");
        tabView.Name = Player.PlayerName;
        tabView.Money = Time.CurrentTime;

        tabView.Tabs.Clear();
        AddMessages();
        AddContacts();
        AddPhoneRepliesMessages();
        AddTextMessages();
        AddLocations();
        tabView.RefreshIndex();
    }
}