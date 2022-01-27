using iFruitAddon2;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using RAGENativeUI.Elements;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ReportingMenu
{
    private TabView tabView;

    private TabItemSimpleList simpleListTab;
    private TabMissionSelectItem missionSelectTab;
    private TabTextItem textTab;
    private TabSubmenuItem submenuTab;
    private IGangRelateable Player;
    private ITimeReportable Time;
    private IPlacesOfInterest PlacesOfInterest;
    private IGangs Gangs;
    private IGangTerritories GangTerritories;
    private IZones Zones;
    public ReportingMenu(IGangRelateable player, ITimeReportable time, IPlacesOfInterest placesOfInterest, IGangs gangs, IGangTerritories gangTerritories, IZones zones)
    {
        Player = player;
        Time = time;
        PlacesOfInterest = placesOfInterest;
        Gangs = gangs;
        GangTerritories = gangTerritories;
        Zones = zones;
    }
    public void Setup()
    {
        tabView = new TabView("Los Santos ~r~RED~s~");
        tabView.Tabs.Clear();
        tabView.OnMenuClose += TabView_OnMenuClose;
    }

    private void TabView_OnMenuClose(object sender, EventArgs e)
    {

        Game.IsPaused = false;
    }

    public void Update()
    {
        tabView.Update();
        if(tabView.Visible)
        {
            tabView.Money = Time.CurrentTime;
        }
    }
    public void Toggle()
    {
        if (!TabView.IsAnyPauseMenuVisible)
        {
            if(!tabView.Visible)
            {
                UpdateMenu();
                Game.IsPaused = true;
            }
            tabView.Visible = !tabView.Visible;
        }
    }
    private void UpdateMenu()
    {
        tabView.MoneySubtitle = Player.Money.ToString("C0");
        tabView.Name = Player.PlayerName;
        tabView.Money = Time.CurrentTime;

        tabView.Tabs.Clear();

        AddVehicles();
        AddCrimes();
        //AddLocations();
        //AddGangDens();
        //AddGangReputation();
        AddGangItems();

        AddTextMessages();

        tabView.RefreshIndex();
    }
    private void AddVehicles()
    {
        List<UIMenuItem> menuItems2 = new List<UIMenuItem>();
        foreach(VehicleExt car in Player.OwnedVehicles)
        {
            Color carColor = car.VehicleColor();
            string Make = car.MakeName();
            string Model = car.ModelName();
            string VehicleName = "";

            string hexColor = ColorTranslator.ToHtml(Color.FromArgb(carColor.ToArgb()));
            if (carColor.ToString() != "")
            {
                VehicleName += $"<FONT color='{hexColor}'>" + carColor.Name + "~s~";
            }
            EntryPoint.WriteToConsole($"hexColor IS NOW: {hexColor}", 5);
            if (Make != "")
            {
                VehicleName += " " + Make;
            }
            if (Model != "")
            {
                VehicleName += " " + Model;
            }
            string rightText = "";
            if (car.CarPlate != null)
            {
                if (car.CarPlate.IsWanted)
                {
                    VehicleName += $" Plate: {car.CarPlate}";
                    rightText = " ~r~(Wanted)~s~";
                }
                else
                {
                    VehicleName += $" Plate: {car.CarPlate}~s~";
                }
            }
            menuItems2.Add(new UIMenuItem(VehicleName, "") { RightLabel = rightText });
        }


        //if (Player.OwnedVehicle != null)
        //{
        //    Color carColor = Player.OwnedVehicle.VehicleColor();
        //    string Make = Player.OwnedVehicle.MakeName();
        //    string Model = Player.OwnedVehicle.ModelName();
        //    string VehicleName = "";

        //    string hexColor = ColorTranslator.ToHtml(Color.FromArgb(carColor.ToArgb()));
        //    if (carColor.ToString() != "")
        //    {
        //        VehicleName += $"<FONT color='{hexColor}'>" + carColor.Name + "~s~";
        //    }
        //    EntryPoint.WriteToConsole($"hexColor IS NOW: {hexColor}", 5);
        //    if (Make != "")
        //    {
        //        VehicleName += " " + Make;
        //    }
        //    if (Model != "")
        //    {
        //        VehicleName += " " + Model;
        //    }
        //    string rightText = "";
        //    if (Player.OwnedVehicle.CarPlate != null)
        //    {
        //        if(Player.OwnedVehicle.CarPlate.IsWanted)
        //        {
        //            VehicleName += $" Plate: {Player.OwnedVehicle.CarPlate}";
        //            rightText = " ~r~(Wanted)~s~";
        //        }
        //        else
        //        {
        //            VehicleName += $" Plate: {Player.OwnedVehicle.CarPlate}~s~";
        //        }  
        //    }
        //    menuItems2.Add(new UIMenuItem(VehicleName, "") { RightLabel = rightText });
        //}
        TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("Vehicles", menuItems2);
        tabView.AddTab(interactiveListItem2);
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
            TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("Current Crimes", menuItems2);
            tabView.AddTab(interactiveListItem2);
        }
        else if (Player.WantedCrimes != null)
        {
            foreach (Crime crime in Player.WantedCrimes.OrderByDescending(x => x.ResultingWantedLevel))
            {
                menuItems2.Add(new UIMenuItem(crime.Name, "") { RightLabel = $"Wanted Level: {crime.ResultingWantedLevel}" });
            }
            TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("Criminal History", menuItems2);
            tabView.AddTab(interactiveListItem2);
        }
        else
        {
            TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("Criminal History", menuItems2);
            tabView.AddTab(interactiveListItem2);
        }
        //menuItems2[0].Activated += (m, s) => Game.DisplaySubtitle("Activated first item!");
        
        
    }

    private void AddGangDens()
    {
        List<UIMenuItem> menuItems2 = new List<UIMenuItem>();
        foreach (GameLocation gl in PlacesOfInterest.GetLocations(LocationType.GangDen))
        {
            Gang mygang = Gangs.GetGang(gl.GangID);
            if (mygang != null)
            {
                menuItems2.Add(new UIMenuItem($"{mygang.ColorPrefix}{mygang.FullName}~s~", "") { RightLabel = gl.IsEnabled ? "Available" : "Unavailable", Enabled = gl.IsEnabled });
            }
            else
            {
                menuItems2.Add(new UIMenuItem(gl.Name, "") { Enabled = gl.IsEnabled });
            }
        }
        //menuItems2[0].Activated += (m, s) => Game.DisplaySubtitle("Activated first item!");
        TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("Gang Dens", menuItems2);
        tabView.AddTab(interactiveListItem2);
    }
    private void AddTextMessages()
    {
        List<TabItem> items = new List<TabItem>();


        foreach (iFruitText text in Player.TextList.OrderByDescending(x => x.Index))
        {

            string TimeReceived = string.Format("{0:D2}h:{1:D2}m",
                text.HourSent,
                text.MinuteSent);

            string DescriptionText = "";
            DescriptionText = "From: " + text.Name;
            DescriptionText += $"~n~At: {TimeReceived}";  //+ gr.ToStringBare();
            DescriptionText += $"~n~Message: {text.Message}";
            string TitleText = $"{text.Name}{(text.IsRead ? " Unread" : "")}";
            TabItem tItem = new TabTextItem(TitleText, TitleText, text.Message);

            tItem.Activated += (s, e) => Game.DisplaySubtitle("Activated Submenu Item #" + submenuTab.Index, 5000);
            items.Add(tItem);
        }
        tabView.AddTab(submenuTab = new TabSubmenuItem("Text Messages", items));
    }


    private void AddGangReputation()
    {
        List<UIMenuItem> menuItems = new List<UIMenuItem>();
        foreach (GangReputation gr in Player.GangReputations.OrderByDescending(x => x.GangRelationship == GangRespect.Hostile).ThenByDescending(x => x.GangRelationship == GangRespect.Friendly).ThenByDescending(x => Math.Abs(x.ReputationLevel)).ThenBy(x => x.Gang.ShortName))
        {
            menuItems.Add(new UIMenuItem($"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~", gr.ToStringBare()) { RightLabel = gr.ToStringBare() });
        }
        //menuItems[0].Activated += (m, s) => Game.DisplaySubtitle("Activated first item!");
        TabInteractiveListItem interactiveListItem = new TabInteractiveListItem("Gang Reputation", menuItems);
        tabView.AddTab(interactiveListItem);
    }


    private void AddGangItems()
    {
        List<TabItem> items = new List<TabItem>();


        foreach (GangReputation gr in Player.GangReputations.OrderByDescending(x => x.GangRelationship == GangRespect.Hostile).ThenByDescending(x => x.GangRelationship == GangRespect.Friendly).ThenByDescending(x => Math.Abs(x.ReputationLevel)).ThenBy(x => x.Gang.ShortName))
        {
            List<ZoneJurisdiction> gangTerritory = GangTerritories.GetGangTerritory(gr.Gang.ID);

            GameLocation gangDen = PlacesOfInterest.GetLocations(LocationType.GangDen).Where(x => x.GangID == gr.Gang.ID).FirstOrDefault();
            string DenText = "~y~Unknown~s~";
            if (gangDen != null)
            {
                 DenText = gangDen.IsEnabled ? "~g~Available~s~" : "~o~Unavailable~s~";
            }

            string TerritoryText = "None";
            if(gangTerritory.Any())
            {
                TerritoryText = "";
                foreach (ZoneJurisdiction zj in gangTerritory)
                {
                    Zone myZone = Zones.GetZone(zj.ZoneInternalGameName);
                    if(myZone != null)
                    {
                        TerritoryText += "~w~" + myZone.DisplayName + "~s~, ";
                    }
                }
            }
            string DescriptionText = "";


            string ContactText = Player.IsContactEnabled(gr.Gang.ContactName) ? "~g~Available~s~" : "~r~Unavailable";


            DescriptionText = "Relationship: " + gr.ToStringBare();
            DescriptionText += $"~n~Den: {DenText}"; //+ gr.ToStringBare();
            DescriptionText += $"~n~Territory: {TerritoryText.TrimEnd(' ', ',')}";
            DescriptionText += $"~n~Contacts: {ContactText}";

            TabItem tItem = new TabTextItem($"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~ {gr.ToBlip()}~s~", $"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~", DescriptionText);

            tItem.Activated += (s, e) => Game.DisplaySubtitle("Activated Submenu Item #" + submenuTab.Index, 5000);
            items.Add(tItem);

            //menuItems.Add(new UIMenuItem($"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~", gr.ToStringBare()) { RightLabel = gr.ToStringBare() });
        }



        //for (int i = 0; i < 10; i++)
        //{
        //    TabItem tItem = new TabTextItem("Item #" + i, "Title #" + i, "Some random text for #" + i);
        //    tItem.Activated += (s, e) => Game.DisplaySubtitle("Activated Submenu Item #" + submenuTab.Index, 5000);
        //    items.Add(tItem);
        //}
        tabView.AddTab(submenuTab = new TabSubmenuItem("Gangs", items));
    }
    private void AddLocations()
    {
        List<TabItem> items = new List<TabItem>();
        foreach (LocationType lt in (LocationType[])Enum.GetValues(typeof(LocationType)))
        {
            List<UIMenuItem> subplaces = new List<UIMenuItem>(); ;
            foreach (GameLocation gl in PlacesOfInterest.GetLocations(lt))
            {
                subplaces.Add(new UIMenuItem(gl.Name, ""));
            }
            TabItem tItem = new TabInteractiveListItem(lt.ToString(), subplaces);
            tItem.Activated += (s, e) => Game.DisplaySubtitle("Activated Submenu Item #" + submenuTab.Index, 5000);
            items.Add(tItem);
        }
        tabView.AddTab(submenuTab = new TabSubmenuItem("Locations", items) {   });
    }


}

