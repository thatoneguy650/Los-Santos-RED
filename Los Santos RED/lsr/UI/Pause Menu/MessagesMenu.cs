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
    private IGangTerritories GangTerritories;
    private IInteriors Interiors;
    private IPlacesOfInterest PlacesOfInterest;
    private IGangRelateable Player;
    private IStreets Streets;
    private TabView tabView;
    private TabSubmenuItem TextMessagesSubMenu;
    private ITimeReportable Time;
    private IEntityProvideable World;
    private IZones Zones;
    private TabSubmenuItem PhoneRepliesSubMenu;
    private TabTextItem ttx;
    private string FilterString;
    private TabSubmenuItem dynamicLocationsTabSubmenuITem;

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
        tabView.OnMenuClose += (s, e) =>
        {
            Player.StopDynamicActivity();
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
    private void AddMessages()
    {
        List<TabItem> items = new List<TabItem>();
        bool addedItems = false;

        List<Tuple<string, DateTime>> MessageTimes = new List<Tuple<string, DateTime>>();

        MessageTimes.AddRange(Player.CellPhone.PhoneResponseList.OrderByDescending(x => x.TimeReceived).Take(15).Select(x => new Tuple<string, DateTime>(x.ContactName, x.TimeReceived)));
        MessageTimes.AddRange(Player.CellPhone.TextList.OrderByDescending(x => x.TimeReceived).Take(15).Select(x => new Tuple<string, DateTime>(x.ContactName, x.TimeReceived)));

        foreach (Tuple<string, DateTime> dateTime in MessageTimes.OrderByDescending(x => x.Item2).Take(15))
        {
            PhoneResponse pr = Player.CellPhone.PhoneResponseList.Where(x => x.TimeReceived == dateTime.Item2 && x.ContactName == dateTime.Item1).FirstOrDefault();
            if (pr != null)
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
                if (dynamicLocationsTabSubmenuITem.Items[i].Title != "Search For Location" && dynamicLocationsTabSubmenuITem.Items[i].Title != "Remove GPS")
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
            foreach (BasicLocation bl in DirectoryLocations.Where(x => x.TypeName == typeName).OrderBy(x => Player.Character.DistanceTo2D(x.EntrancePosition)))
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

}