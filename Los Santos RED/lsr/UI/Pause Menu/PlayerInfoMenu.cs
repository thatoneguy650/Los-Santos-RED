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
    private TabSubmenuItem GangsSubMenu;
    private IGangTerritories GangTerritories;
    private IInteriors Interiors;
    private IPlacesOfInterest PlacesOfInterest;
    private IGangRelateable Player;
    private IStreets Streets;
    private TabView tabView;
    private ITimeReportable Time;
    private TabSubmenuItem VehiclesSubMenu;
    private IEntityProvideable World;
    private IZones Zones;
    private TabSubmenuItem LicensesSubMenu;
    private List<BasicLocation> SearchResultLocations = new List<BasicLocation>();
    private UIMenuItem SearchLocationByName;
    private UIMenuListScrollerItem<BasicLocation> LocationResults;
    private IShopMenus ShopMenus;
    private IModItems ModItems;
    private IWeapons Weapons;
    private TabSubmenuItem dynamicLocationsTabSubmenuITem;
    private string FilterString = "";
    private TabTextItem ttx;

    public PlayerInfoMenu(IGangRelateable player, ITimeReportable time, IPlacesOfInterest placesOfInterest, IGangs gangs, IGangTerritories gangTerritories, IZones zones, IStreets streets, IInteriors interiors, IEntityProvideable world, IShopMenus shopMenus, IModItems modItems, IWeapons weapons)
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
    private void AddVehicles()
    {
        List<TabItem> items = new List<TabItem>();
        bool addedItems = false;
        foreach (VehicleExt car in Player.VehicleOwnership.OwnedVehicles)
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
                tabView.Visible = false;
                Game.IsPaused = false;
                //VehiclesSubMenu.Items.Remove(tItem);
                //VehiclesSubMenu.RefreshIndex();
                // Game.DisplaySubtitle("Ownership Cleared");
                Player.VehicleOwnership.RemoveOwnershipOfVehicle(car);
            };


            //tItem.Activated += (s, e) => Game.DisplaySubtitle("Activated Submenu Item #" + submenuTab.Index, 5000);
            items.Add(tItem);
            addedItems = true;
        }
        if (addedItems)
        {
            tabView.AddTab(VehiclesSubMenu = new TabSubmenuItem("Vehicles", items));
        }
        else
        {
            tabView.AddTab(new TabItem("Vehicles"));
        }
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
    private void AddCrimes()
    {
        List<UIMenuItem> menuItems2 = new List<UIMenuItem>();
        bool addedItems = false;
        if (Player.IsWanted)
        {
            foreach (CrimeEvent crime in Player.PoliceResponse.CrimesObserved.OrderByDescending(x => x.AssociatedCrime?.ResultingWantedLevel))
            {
                string crimeText = crime.AssociatedCrime.Name;
                crimeText += $" Instances: ({crime.Instances})";
                menuItems2.Add(new UIMenuItem(crimeText, "") { RightLabel = $"Wanted Level: {crime.AssociatedCrime.ResultingWantedLevel}" });
                addedItems = true;
            }
            TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("Crimes", menuItems2);
            tabView.AddTab(interactiveListItem2);
        }
        else if (Player.WantedCrimes != null)
        {
            foreach (Crime crime in Player.WantedCrimes.OrderByDescending(x => x.ResultingWantedLevel))
            {
                menuItems2.Add(new UIMenuItem(crime.Name, "") { RightLabel = $"Wanted Level: {crime.ResultingWantedLevel}" });
                addedItems = true;
            }
            TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("Crimes", menuItems2);
            tabView.AddTab(interactiveListItem2);
        }
        else
        {
            tabView.AddTab(new TabItem("Crimes"));
        }
        //else
        //{
        //    TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("Crimes", menuItems2);
        //    tabView.AddTab(interactiveListItem2);
        //}
        //menuItems2[0].Activated += (m, s) => Game.DisplaySubtitle("Activated first item!");
    }
    private void AddGangItems()
    {
        List<TabItem> items = new List<TabItem>();
        bool addedItems = false;
        foreach (GangReputation gr in Player.GangRelationships.GangReputations.OrderByDescending(x => x.GangRelationship == GangRespect.Hostile).ThenByDescending(x => x.GangRelationship == GangRespect.Friendly).ThenByDescending(x => Math.Abs(x.ReputationLevel)).ThenBy(x => x.Gang.ShortName))
        {
            string DescriptionText = "";
            ShopMenu dealerMenu;
            ShopMenu denMenu;
            DescriptionText = "Relationship: " + gr.ToStringBare();
            GangDen myDen = PlacesOfInterest.PossibleLocations.GangDens.FirstOrDefault(x => x.AssociatedGang?.ID == gr.Gang.ID);
            if (myDen != null && myDen.IsEnabled)
            {
                DescriptionText += $"~n~{gr.Gang.DenName}: {myDen.FullStreetAddress}"; //+ gr.ToStringBare();
            }
            dealerMenu = ShopMenus.GetRandomMenu(gr.Gang.DealerMenuGroup);
            denMenu = myDen?.Menu;
            List<string> Drugs = new List<string>();
            List<string> Guns = new List<string>();
            if (dealerMenu != null)
            {
                foreach (MenuItem mi in dealerMenu.Items)
                {
                    ModItem modItem = ModItems.Get(mi.ModItemName);
                    if (modItem != null)
                    {
                        if (modItem.ItemType == ItemType.Drugs)
                        {
                            if (!Drugs.Contains(modItem.Name))
                            {
                                Drugs.Add(modItem.Name);
                            }
                        }
                        else if (modItem.ItemType == ItemType.Weapons)
                        {
                            WeaponInformation wi = Weapons.GetWeapon(modItem.ModelItem?.ModelName);
                            if (wi != null)
                            {
                                if (!Guns.Contains(wi.Category.ToString()))
                                {
                                    Guns.Add(wi.Category.ToString());
                                }
                            }
                        }
                    }
                }
            }
            if(denMenu != null)
            {
                foreach (MenuItem mi in denMenu.Items)
                {
                    ModItem modItem = ModItems.Get(mi.ModItemName);
                    if (modItem != null)
                    {
                        if (modItem.ItemType == ItemType.Drugs)
                        {
                            if (!Drugs.Contains(modItem.Name))
                            {
                                Drugs.Add(modItem.Name);
                            }
                        }
                        else if (modItem.ItemType == ItemType.Weapons)
                        {
                            WeaponInformation wi = Weapons.GetWeapon(modItem.ModelItem?.ModelName);
                            if (wi != null)
                            {
                                if (!Guns.Contains(wi.Category.ToString()))
                                {
                                    Guns.Add(wi.Category.ToString());
                                }
                            }
                        }
                    }
                }
            }
            if(Drugs.Any())
            {
                DescriptionText += $"~n~Drugs: {string.Join(", ",Drugs.OrderBy(x=> x))}"; //+ gr.ToStringBare();
            }
            if (Guns.Any())
            {
                DescriptionText += $"~n~Guns: {string.Join(", ", Guns.OrderBy(x=> x))}"; //+ gr.ToStringBare();
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
            addedItems = true;
        }
        if (addedItems)
        {
            tabView.AddTab(GangsSubMenu = new TabSubmenuItem("Gangs", items));
        }
    }
    private void AddLocations()
    {
        FilterString = "";
        List<TabItem> items = new List<TabItem>();
        TabTextItem removeGPSTTI = new TabTextItem("Remove GPS", "Remove GPS", "Remove the GPS Blip");
        removeGPSTTI.Activated += (s, e) =>
        {
            Player.Destinations.RemoveGPSRoute();
        };
        items.Add(removeGPSTTI);

        ttx = new TabTextItem("Search For Location", "Search For Location", $"Search for a location, select to enter search criteria");
        ttx.Activated += (s, e) =>
        {
            FilterString = NativeHelper.GetKeyboardInput("");
            ttx.Text = $"Search for a location, select to enter search criteria~n~Current Search String: ~h~{FilterString}~h~";
            for (int i = dynamicLocationsTabSubmenuITem.Items.Count; i-- > 0;)
            {
                if(dynamicLocationsTabSubmenuITem.Items[i].Title != "Search For Location" && dynamicLocationsTabSubmenuITem.Items[i].Title != "Remove GPS")
                {
                    dynamicLocationsTabSubmenuITem.Items.RemoveAt(i);
                }
            }
            dynamicLocationsTabSubmenuITem.Items.AddRange(GetDirectoryLocations());
            dynamicLocationsTabSubmenuITem.RefreshIndex();
        };
        items.Add(ttx);
        items.AddRange(GetDirectoryLocations());
        dynamicLocationsTabSubmenuITem = new TabSubmenuItem("Locations", items);
        tabView.AddTab(dynamicLocationsTabSubmenuITem);       
    }
    private List<TabItem> GetDirectoryLocations()
    {
        List<TabItem> items = new List<TabItem>();
        List<BasicLocation> DirectoryLocations = PlacesOfInterest.GetAllLocations().Where(x => x.ShowsOnDirectory && (FilterString == "" || x.Name.ToLower().Contains(FilterString.ToLower()))).ToList();
        foreach (string typeName in DirectoryLocations.OrderBy(x => x.TypeName).Select(x => x.TypeName).Distinct())
        {
            List<MissionInformation> missionInfoList = new List<MissionInformation>();
            foreach (BasicLocation bl in DirectoryLocations.Where(x => x.TypeName == typeName).OrderBy(x=> Player.Character.DistanceTo2D(x.EntrancePosition)))
            {
                MissionInformation locationInfo = new MissionInformation(bl.FullName, bl.Description, bl.DirectoryInfo(Time.CurrentHour, Player.Character.DistanceTo2D(bl.EntrancePosition)));
                missionInfoList.Add(locationInfo);
            }
            TabMissionSelectItem basicLocationTypeList = new TabMissionSelectItem(typeName, missionInfoList);
            basicLocationTypeList.OnItemSelect += (selectedItem) =>
            {
                if (selectedItem != null)
                {
                    BasicLocation toGPS = DirectoryLocations.FirstOrDefault(x => x.FullName == selectedItem.Name);
                    if (toGPS != null)
                    {
                        Player.Destinations.AddGPSRoute(toGPS.Name, toGPS.EntrancePosition);
                    }
                }
            };
            items.Add(basicLocationTypeList);
        }
        return items;
    }
    private void UpdateMenu()
    {
        tabView.MoneySubtitle = Player.BankAccounts.Money.ToString("C0");
        tabView.Name = Player.PlayerName;
        tabView.Money = Time.CurrentTime;
        tabView.Tabs.Clear();
        AddVehicles();
        AddLicenses();
        AddCrimes();
        AddGangItems();
        AddLocations();
        tabView.RefreshIndex();
    }
}