using iFruitAddon2;
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

    public PlayerInfoMenu(IGangRelateable player, ITimeReportable time, IPlacesOfInterest placesOfInterest, IGangs gangs, IGangTerritories gangTerritories, IZones zones, IStreets streets, IInteriors interiors, IEntityProvideable world)
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
        tabView = new TabView("Los Santos ~r~RED~s~ Information");
        tabView.Tabs.Clear();
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
        foreach (iFruitContact contact in Player.CellPhone.ContactList.OrderBy(x => x.Name))
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
        }
        tabView.AddTab(ContactsSubMenu = new TabSubmenuItem("Contacts", items));
    }
    private void AddCrimes()
    {
        List<UIMenuItem> menuItems2 = new List<UIMenuItem>();
        if (Player.IsWanted)
        {
            foreach (CrimeEvent crime in Player.PoliceResponse.CrimesObserved.OrderByDescending(x => x.AssociatedCrime?.ResultingWantedLevel))
            {
                string crimeText = crime.AssociatedCrime.Name;
                crimeText += $" Instances: ({crime.Instances})";
                menuItems2.Add(new UIMenuItem(crimeText, "") { RightLabel = $"Wanted Level: {crime.AssociatedCrime.ResultingWantedLevel}" });
            }
            TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("Crimes", menuItems2);
            tabView.AddTab(interactiveListItem2);
        }
        else if (Player.WantedCrimes != null)
        {
            foreach (Crime crime in Player.WantedCrimes.OrderByDescending(x => x.ResultingWantedLevel))
            {
                menuItems2.Add(new UIMenuItem(crime.Name, "") { RightLabel = $"Wanted Level: {crime.ResultingWantedLevel}" });
            }
            TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("Crimes", menuItems2);
            tabView.AddTab(interactiveListItem2);
        }
        else
        {
            TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("Crimes", menuItems2);
            tabView.AddTab(interactiveListItem2);
        }
        //menuItems2[0].Activated += (m, s) => Game.DisplaySubtitle("Activated first item!");
    }
    private void AddGangItems()
    {
        List<TabItem> items = new List<TabItem>();

        foreach (GangReputation gr in Player.GangRelationships.GangReputations.OrderByDescending(x => x.GangRelationship == GangRespect.Hostile).ThenByDescending(x => x.GangRelationship == GangRespect.Friendly).ThenByDescending(x => Math.Abs(x.ReputationLevel)).ThenBy(x => x.Gang.ShortName))
        {
            string DescriptionText = "";

            DescriptionText = "Relationship: " + gr.ToStringBare();
            GangDen myDen = PlacesOfInterest.PossibleLocations.GangDens.FirstOrDefault(x => x.AssociatedGang?.ID == gr.Gang.ID);
            if (myDen != null && myDen.IsEnabled)
            {
                DescriptionText += $"~n~{gr.Gang.DenName}: {myDen.StreetAddress}"; //+ gr.ToStringBare();
            }

            string TerritoryText = "None";
            List<ZoneJurisdiction> gangTerritory = GangTerritories.GetGangTerritory(gr.Gang.ID);
            if (gangTerritory.Any())
            {
                TerritoryText = "";
                foreach (ZoneJurisdiction zj in gangTerritory)
                {
                    Zone myZone = Zones.GetZone(zj.ZoneInternalGameName);
                    if (myZone != null)
                    {
                        TerritoryText += "~p~" + myZone.DisplayName + "~s~, ";
                    }
                }
            }
            DescriptionText += $"~n~Territory: {TerritoryText.TrimEnd(' ', ',')}";
            //EntryPoint.WriteToConsole($"{gr.Gang.ContactName}");
            //EntryPoint.WriteToConsole($"{gr.Gang.ShortName} Player.CurrentCellPhone {Player.CellPhone != null}");
            //EntryPoint.WriteToConsole($"Player.CurrentCellPhone {Player.CellPhone != null}");
            if (Player.CellPhone.IsContactEnabled(gr.Gang.ContactName))
            {
                string ContactText = gr.Gang.ContactName;
                DescriptionText += $"~n~Contacts: {ContactText}";
            }
            PlayerTask gangTask = Player.PlayerTasks.GetTask(gr.Gang.ContactName);
            if (gangTask != null)
            {
                DescriptionText += $"~n~~g~Has Task~s~";
                if(gangTask.CanExpire)
                {
                    DescriptionText += $" Complete Before: ~r~{gangTask.ExpireTime:d} {gangTask.ExpireTime:t}~s~";
                }
            }

            if (gr.MembersKilled > 0)
            {
                DescriptionText += $"~n~~r~Members Killed~s~: {gr.MembersKilled}~s~ ({gr.MembersKilledInTerritory})";
            }
            if (gr.MembersHurt > 0)
            {
                DescriptionText += $"~n~~o~Members Hurt~s~: {gr.MembersHurt}~s~ ({gr.MembersHurtInTerritory})";
            }
            if (gr.MembersCarJacked > 0)
            {
                DescriptionText += $"~n~~o~Members CarJacked~s~: {gr.MembersCarJacked}~s~ ({gr.MembersCarJackedInTerritory})";
            }

            if (gr.PlayerDebt > 0)
            {
                string debtstring = gr.PlayerDebt.ToString("C0");
                DescriptionText += $"~n~~r~Debt~s~: ~r~{debtstring}~s~";
            }

            TabItem tabItem = new TabTextItem($"{gr.Gang.ShortName} {gr.ToBlip()}~s~", $"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~", DescriptionText);//TabItem tabItem = new TabTextItem($"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~ {gr.ToBlip()}~s~", $"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~", DescriptionText);
            //tabItem.Activated += (s, e) => Game.DisplaySubtitle("Activated Submenu Item #" + GangsSubMenu.Index, 5000);
            items.Add(tabItem);
        }
        tabView.AddTab(GangsSubMenu = new TabSubmenuItem("Gangs", items));
    }
    private void AddLocations()
    {
        List<UIMenuItem> menuItems = new List<UIMenuItem>();
        menuItems.Add(new UIMenuItem("Remove GPS Route", "Remove any enabled GPS Blip"));


        List<BasicLocation> Residences = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.Residences.Any(x=> x.IsOwnedOrRented))
        {
            foreach (Residence res in PlacesOfInterest.PossibleLocations.Residences.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                if(res.IsOwnedOrRented)
                {
                    Residences.Add(res);
                }
                
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Residences", $"List of all owned or rented residences", Residences) { Formatter = sy => $"{sy.Name} - " + $"{sy.StreetAddress}".Trim() });
        }
        List<BasicLocation> ForSaleResidences = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.Residences.Any(x=> !x.IsOwnedOrRented))
        {
            foreach (Residence residence in PlacesOfInterest.PossibleLocations.Residences.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                if (!residence.IsOwnedOrRented)
                {
                    ForSaleResidences.Add(residence);
                }
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("For Sale/Rental", $"List of all residences that are for sale or rental", ForSaleResidences) { Formatter = sy => $"{sy.Name} - " + $"{sy.StreetAddress}".Trim() });
        }
        List<BasicLocation> Hotels = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.Hotels.Any())
        {
            foreach (Hotel hotel in PlacesOfInterest.PossibleLocations.Hotels.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                Hotels.Add(hotel);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Hotels", $"List of all Hotels", Hotels) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.StreetAddress}".Trim() });
        }
        List<BasicLocation> Banks = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.Banks.Any())
        {
            foreach (Bank Bank in PlacesOfInterest.PossibleLocations.Banks.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                Banks.Add(Bank);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Banks", $"List of all Banks", Banks) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.StreetAddress}".Trim() });
        }
        List<BasicLocation> BeautyShops = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.BeautyShops.Any())
        {
            foreach (BeautyShop shop in PlacesOfInterest.PossibleLocations.BeautyShops.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                BeautyShops.Add(shop);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Beauty Shops", $"List of all BeautyShops", BeautyShops) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.StreetAddress}".Trim() });
        }
        List<BasicLocation> Dispensaries = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.Dispensaries.Any())
        {
            foreach (Dispensary shop in PlacesOfInterest.PossibleLocations.Dispensaries.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                Dispensaries.Add(shop);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Dispensaries", $"List of all Dispensaries", Dispensaries) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.StreetAddress}".Trim() });
        }
        List<BasicLocation> HardwareStores = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.HardwareStores.Any())
        {
            foreach (HardwareStore shop in PlacesOfInterest.PossibleLocations.HardwareStores.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                HardwareStores.Add(shop);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Hardward Stores", $"List of all Hardward Stores", HardwareStores) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.StreetAddress}".Trim() });
        }
        List<BasicLocation> HeadShops = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.HeadShops.Any())
        {
            foreach (HeadShop shop in PlacesOfInterest.PossibleLocations.HeadShops.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                HeadShops.Add(shop);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Head Shops", $"List of all Head Shops", HeadShops) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.StreetAddress}".Trim() });
        }
        List<BasicLocation> PawnShops = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.PawnShops.Any())
        {
            foreach (PawnShop shop in PlacesOfInterest.PossibleLocations.PawnShops.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                PawnShops.Add(shop);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Pawn Shops", $"List of all Pawn Shops", PawnShops) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.StreetAddress}".Trim() });
        }
        List<BasicLocation> Pharmacies = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.Pharmacies.Any())
        {
            foreach (Pharmacy shop in PlacesOfInterest.PossibleLocations.Pharmacies.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                Pharmacies.Add(shop);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Pharmacies", $"List of all Pharmacies", Pharmacies) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.StreetAddress}".Trim() });
        }
        List<BasicLocation> Stadiums = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.Stadiums.Any())
        {
            foreach (Stadium shop in PlacesOfInterest.PossibleLocations.Stadiums.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                Stadiums.Add(shop);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Stadiums", $"List of all Stadiums", Stadiums) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.StreetAddress}".Trim() });
        }
        List<BasicLocation> ConvenienceStores = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.ConvenienceStores.Any())
        {
            foreach (ConvenienceStore shop in PlacesOfInterest.PossibleLocations.ConvenienceStores.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                ConvenienceStores.Add(shop);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Convenience Stores", $"List of all Convenience Stores", ConvenienceStores) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.StreetAddress}".Trim() });
        }
        List<BasicLocation> ScrapYards = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.ScrapYards.Any())
        {
            foreach (ScrapYard sy in PlacesOfInterest.PossibleLocations.ScrapYards.OrderBy(x=> x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                ScrapYards.Add(sy);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Scrap Yards", $"List of all Scrap Yards", ScrapYards) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.StreetAddress}".Trim() });
        }
        List<BasicLocation> CityHalls = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.ScrapYards.Any())
        {
            foreach (CityHall sy in PlacesOfInterest.PossibleLocations.CityHalls.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                CityHalls.Add(sy);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("City Halls", $"List of all City Halls", CityHalls) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.StreetAddress}".Trim() });
        }
        List<BasicLocation> PoliceStations = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.PoliceStations.Any())
        {
            foreach (PoliceStation sy in PlacesOfInterest.PossibleLocations.PoliceStations.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                PoliceStations.Add(sy);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Police Stations", $"List of all Police Stations", PoliceStations) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.StreetAddress}".Trim() });
        }
        List<BasicLocation> FireStations = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.PoliceStations.Any())
        {
            foreach (FireStation sy in PlacesOfInterest.PossibleLocations.FireStations.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                FireStations.Add(sy);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Fire Stations", $"List of all Fire Stations", FireStations) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.StreetAddress}".Trim() });
        }
        List<BasicLocation> Hospitals = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.Hospitals.Any())
        {
            foreach (Hospital sy in PlacesOfInterest.PossibleLocations.Hospitals.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                Hospitals.Add(sy);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Hospitals", $"List of all Hospitals", Hospitals) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.StreetAddress}".Trim() });
        }
        List<BasicLocation> Restaurants = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.Restaurants.Any())
        {
            foreach (Restaurant sy in PlacesOfInterest.PossibleLocations.Restaurants.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                Restaurants.Add(sy);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Restaurants", $"List of all Restaurants", Restaurants) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.StreetAddress}".Trim() });
        }
        List<BasicLocation> LiquorStores = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.Restaurants.Any())
        {
            foreach (LiquorStore sy in PlacesOfInterest.PossibleLocations.LiquorStores.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                LiquorStores.Add(sy);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("LiquorStores", $"List of all LiquorStores", LiquorStores) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.StreetAddress}".Trim() });
        }
        List<BasicLocation> GasStations = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.GasStations.Any())
        {
            foreach (GasStation sy in PlacesOfInterest.PossibleLocations.GasStations.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                GasStations.Add(sy);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("GasStations", $"List of all GasStations", GasStations) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.StreetAddress}".Trim() });
        }
        List<BasicLocation> Bars = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.Bars.Any())
        {
            foreach (Bar sy in PlacesOfInterest.PossibleLocations.Bars.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                Bars.Add(sy);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("Bars", $"List of all Bars", Bars) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.StreetAddress}".Trim() });
        }
        List<BasicLocation> FoodStands = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.FoodStands.Any())
        {
            foreach (FoodStand sy in PlacesOfInterest.PossibleLocations.FoodStands.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                FoodStands.Add(sy);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("FoodStands", $"List of all FoodStands", FoodStands) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.StreetAddress}".Trim() });
        }
        List<BasicLocation> CarDealerships = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.CarDealerships.Any())
        {
            foreach (CarDealership sy in PlacesOfInterest.PossibleLocations.CarDealerships.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                CarDealerships.Add(sy);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("CarDealerships", $"List of all CarDealerships", CarDealerships) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.StreetAddress}".Trim() });
        }
        List<BasicLocation> DriveThrus = new List<BasicLocation>();
        if (PlacesOfInterest.PossibleLocations.DriveThrus.Any())
        {
            foreach (DriveThru sy in PlacesOfInterest.PossibleLocations.DriveThrus.OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)))
            {
                DriveThrus.Add(sy);
            }
            menuItems.Add(new UIMenuListScrollerItem<BasicLocation>("DriveThrus", $"List of all DriveThrus", DriveThrus) { Formatter = sy => $"{sy.Name} - {(sy.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{sy.StreetAddress}".Trim() });
        }
        TabInteractiveListItem interactiveListItem = new TabInteractiveListItem("Locations", menuItems);
        interactiveListItem.BackingMenu.OnItemSelect += BackingMenu_OnItemSelect;
        tabView.AddTab(interactiveListItem);
    }
    private void AddPhoneRepliesMessages()
    {
        List<TabItem> items = new List<TabItem>();
        foreach (PhoneResponse text in Player.CellPhone.PhoneResponseList.OrderByDescending(x => x.TimeReceived).Take(15))
        {
            string TimeReceived = text.TimeReceived.ToString("HH:mm");// text.HourSent.ToString("00") + ":" + text.MinuteSent.ToString("00");// string.Format("{0:D2}h:{1:D2}m",text.HourSent,text.MinuteSent);
            string DescriptionText = "";
            DescriptionText += $"~n~Received At: {TimeReceived}";  //+ gr.ToStringBare();
            DescriptionText += $"~n~{text.Message}";
            DescriptionText += $"~n~~n~Select to ~r~Delete Response~s~";
            string ListEntryItem = $"{text.ContactName} {TimeReceived}";
            string DescriptionHeaderText = $"{text.ContactName}";
            TabItem tItem = new TabTextItem(ListEntryItem, DescriptionHeaderText, DescriptionText);
            tItem.Activated += (s, e) =>
            {
                if (text != null)
                {
                    Player.CellPhone.DeletePhoneRespone(text);
                    PhoneRepliesSubMenu.Items.Remove(tItem);
                    PhoneRepliesSubMenu.RefreshIndex();
                    EntryPoint.WriteToConsole($"Phone Reply deleted {text.ContactName} {text.Message}");
                }
            };
            items.Add(tItem);
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
        tabView.AddTab(PhoneRepliesSubMenu);
    }
    private void AddTextMessages()
    {
        List<TabItem> items = new List<TabItem>();
        foreach (iFruitText text in Player.CellPhone.TextList.OrderByDescending(x => x.TimeReceived).Take(15))
        {
            string TimeReceived = text.HourSent.ToString("00") + ":" + text.MinuteSent.ToString("00");// string.Format("{0:D2}h:{1:D2}m",text.HourSent,text.MinuteSent);

            string DescriptionText = "";
            DescriptionText += $"~n~Received At: {TimeReceived}";  //+ gr.ToStringBare();
            DescriptionText += $"~n~{text.Message}";
            DescriptionText += $"~n~~n~Select to ~r~Delete Message~s~";
            string ListEntryItem = $"{text.Name}{(!text.IsRead ? " *" : "")} {TimeReceived}";
            string DescriptionHeaderText = $"{text.Name}";
            TabItem tItem = new TabTextItem(ListEntryItem, DescriptionHeaderText, DescriptionText);
            tItem.Activated += (s, e) =>
            {
                if (text != null)
                {
                    Player.CellPhone.DeleteText(text);
                    TextMessagesSubMenu.Items.Remove(tItem);
                    TextMessagesSubMenu.RefreshIndex();
                    EntryPoint.WriteToConsole($"Text Message deleted {text.Name} {text.Message}");
                }
            };
            items.Add(tItem);
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
        tabView.AddTab(TextMessagesSubMenu);
    }
    private void AddVehicles()
    {
        List<TabItem> items = new List<TabItem>();
        foreach (VehicleExt car in Player.OwnedVehicles)
        {
            Color carColor = car.VehicleColor();
            string Make = car.MakeName();
            string Model = car.ModelName();
            string PlateText = car.CarPlate?.PlateNumber;
            string VehicleName = "";

            string hexColor = ColorTranslator.ToHtml(Color.FromArgb(carColor.ToArgb()));
            string ColorizedColorName = carColor.Name;
            if (carColor.ToString() != "")
            {
                ColorizedColorName = $"<FONT color='{hexColor}'>" + carColor.Name + "~s~";
                VehicleName += ColorizedColorName;
            }
            string rightText = "";
            if (car.CarPlate != null && car.CarPlate.IsWanted)
            {
                rightText = " ~r~(Wanted)~s~";
            }

            string DescriptionText = $"~n~Color: {ColorizedColorName}";
            DescriptionText += $"~n~Make: {Make}";
            DescriptionText += $"~n~Model: {Model}";
            DescriptionText += $"~n~Plate: {PlateText} {rightText}";

            string ListEntryText = $"{ColorizedColorName} {Make} {Model} ({PlateText})";
            string DescriptionHeaderText = $"{Model}";
            if (car.Vehicle.Exists())
            {
                LocationData myData = new LocationData(car.Vehicle, Streets, Zones, Interiors);
                myData.Update(car.Vehicle);

                string StreetText = "";
                if (myData.CurrentStreet != null)
                {
                    StreetText += $"~y~{myData.CurrentStreet.Name}~s~";
                    if (myData.CurrentCrossStreet != null)
                    {
                        StreetText += $" at ~y~{myData.CurrentCrossStreet.Name}~s~";
                    }
                }
                string ZoneText = "";
                if (myData.CurrentZone != null)
                {
                    ZoneText = $" {(myData.CurrentZone.IsSpecificLocation ? "near" : "in")} ~p~{myData.CurrentZone.FullDisplayName}~s~";
                }
                string LocationText = $"{StreetText} {ZoneText}".Trim();
                LocationText = LocationText.Trim();

                DescriptionText += $"~n~Location: {LocationText}";
            }

            DescriptionText += $"~n~Select to ~r~Clear Ownership~s~";

            TabItem tItem = new TabTextItem(ListEntryText, DescriptionHeaderText, DescriptionText);
            tItem.Activated += (s, e) =>
            {
                VehiclesSubMenu.Items.Remove(tItem);
                VehiclesSubMenu.RefreshIndex();
                Player.RemoveOwnershipOfVehicle(car);
            };


            //tItem.Activated += (s, e) => Game.DisplaySubtitle("Activated Submenu Item #" + submenuTab.Index, 5000);
            items.Add(tItem);
        }
        tabView.AddTab(VehiclesSubMenu = new TabSubmenuItem("Vehicles", items));
    }
    private void AddOther()
    {





        List<TabItem> items = new List<TabItem>();
        TabItem ClearTexts = new TabTextItem("Clear Text Messages", "Clear Text Messages", "Select to clear all ~o~Text Messages~s~");//TabItem tabItem = new TabTextItem($"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~ {gr.ToBlip()}~s~", $"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~", DescriptionText);
        ClearTexts.Activated += (s, e) =>
        {
            tabView.Visible = false;
            Game.IsPaused = false;
            Player.CellPhone.ClearTextMessages();
        };
        items.Add(ClearTexts);

        //TabItem ClearContacts = new TabTextItem("Clear Contacts", "Clear Contacts", "Select to clear all ~o~Contacts~s~");//TabItem tabItem = new TabTextItem($"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~ {gr.ToBlip()}~s~", $"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~", DescriptionText);
        //ClearContacts.Activated += (s, e) =>
        //{
        //    tabView.Visible = false;
        //    Game.IsPaused = false;
        //    Player.CellPhone.ClearContacts();
        //};
        //items.Add(ClearContacts);

        TabItem ClearPhoneResponses = new TabTextItem("Clear Phone Responses", "Clear Phone Responses", "Select to clear all ~o~Phone Responses~s~");//TabItem tabItem = new TabTextItem($"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~ {gr.ToBlip()}~s~", $"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~", DescriptionText);
        ClearPhoneResponses.Activated += (s, e) =>
        {
            tabView.Visible = false;
            Game.IsPaused = false;
            Player.CellPhone.ClearPhoneResponses();
        };
        items.Add(ClearPhoneResponses);

        tabView.AddTab(ContactsSubMenu = new TabSubmenuItem("Other", items));
    }
    private void AddLicenses()
    {
        List<TabItem> items = new List<TabItem>();
        string dldesc = "";
        string ccwdesc = "";
        if (Player.Licenses.HasDriversLicense)
        {
            dldesc = "State: ~y~San Andreas~s~~n~";
            dldesc += $"~n~Status: " + (Player.Licenses.DriversLicense.IsValid(Time) ? "~g~Valid~s~" : "~r~Expired~s~");
            dldesc += Player.Licenses.DriversLicense.ExpirationDescription(Time);
        }
        else
        {
            dldesc = "~r~No Drivers License Issued~s~";
        }
        if (Player.Licenses.HasCCWLicense)
        {
            ccwdesc = "State: ~y~San Andreas~s~~n~";
            ccwdesc += $"~n~Status: " + (Player.Licenses.CCWLicense.IsValid(Time) ? "~g~Valid~s~" : "~r~Expired~s~");
            ccwdesc += Player.Licenses.CCWLicense.ExpirationDescription(Time);
        }
        else
        {
            ccwdesc = "~r~No CCW Issued~s~";
        }
        dldesc += "~n~~n~Description: A legal authorization for a specific individual to operate one or more types of motorized vehicles such as motorcycles, cars, trucks, or buses—on a public road. Vehicle Operators caught without one will be fined.";
        ccwdesc += "~n~~n~Description: Allows Carrying a weapon (such as a handgun) in public in a concealed manner, either on one's person or in close proximity. Legal weapons are returned to owners after medical/bail services.";

        TabItem dl = new TabTextItem("Drivers License", "Drivers License", dldesc);//TabItem tabItem = new TabTextItem($"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~ {gr.ToBlip()}~s~", $"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~", DescriptionText);
        items.Add(dl);

        TabItem ccw = new TabTextItem("CCW License", "CCW License", ccwdesc);//TabItem tabItem = new TabTextItem($"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~ {gr.ToBlip()}~s~", $"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~", DescriptionText);
        items.Add(ccw);

        LicensesSubMenu = new TabSubmenuItem("Info", items);
        tabView.AddTab(LicensesSubMenu);
    }
    private void BackingMenu_OnItemSelect(RAGENativeUI.UIMenu sender, UIMenuItem selectedItem, int index)
    {
        //if (selectedItem.GetType() == typeof(UIMenuListScrollerItem<GameLocation>))
        //{
        //    UIMenuListScrollerItem<GameLocation> myItem = (UIMenuListScrollerItem<GameLocation>)selectedItem;
        //    if (myItem.SelectedItem != null)
        //    {
        //        Player.AddGPSRoute(myItem.SelectedItem.Name, myItem.SelectedItem.EntrancePosition);
        //    }
        //}
        if (selectedItem.GetType() == typeof(UIMenuListScrollerItem<BasicLocation>))
        {
            UIMenuListScrollerItem<BasicLocation> myItem = (UIMenuListScrollerItem<BasicLocation>)selectedItem;
            if (myItem.SelectedItem != null)
            {
                Player.AddGPSRoute(myItem.SelectedItem.Name, myItem.SelectedItem.EntrancePosition);
            }
        }
        else if (selectedItem.Text == "Remove GPR Route")
        {
            Player.RemoveGPSRoute();
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
            //case LocationType.ScrapYard:
            //    return "Scrap Yard";
            case LocationType.StripClub:
                return "Strip Club";
            case LocationType.VendingMachine:
                return "Vending Machine";
            //case LocationType.GunShop:
            //    return "Gun Shop";
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
        Game.IsPaused = false;
    }
    private void UpdateMenu()
    {
        tabView.MoneySubtitle = Player.Money.ToString("C0");
        tabView.Name = Player.PlayerName;
        tabView.Money = Time.CurrentTime;

        tabView.Tabs.Clear();
        AddVehicles();
        AddLicenses();
        AddCrimes();
        AddGangItems();
        AddContacts();
        AddPhoneRepliesMessages();
        AddTextMessages();
        AddLocations();
       //AddOther();
        tabView.RefreshIndex();
    }
}