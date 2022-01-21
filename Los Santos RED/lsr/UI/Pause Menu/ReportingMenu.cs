using LosSantosRED.lsr.Interface;
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
    public ReportingMenu(IGangRelateable player, ITimeReportable time, IPlacesOfInterest placesOfInterest, IGangs gangs)
    {
        Player = player;
        Time = time;
        PlacesOfInterest = placesOfInterest;
        Gangs = gangs;
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
        AddGangDens();
        AddGangReputation();


        tabView.RefreshIndex();
    }
    private void AddVehicles()
    {
        List<UIMenuItem> menuItems2 = new List<UIMenuItem>();
        if (Player.OwnedVehicle != null)
        {
            Color carColor = Player.OwnedVehicle.VehicleColor();
            string Make = Player.OwnedVehicle.MakeName();
            string Model = Player.OwnedVehicle.ModelName();
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
            if (Player.OwnedVehicle.CarPlate != null)
            {
                if(Player.OwnedVehicle.CarPlate.IsWanted)
                {
                    VehicleName += $" Plate: {Player.OwnedVehicle.CarPlate}";
                    rightText = " ~r~(Wanted)~s~";
                }
                else
                {
                    VehicleName += $" Plate: {Player.OwnedVehicle.CarPlate}~s~";
                }  
            }
            menuItems2.Add(new UIMenuItem(VehicleName, "") { RightLabel = rightText });
        }
        TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("Vehicles", menuItems2);
        tabView.AddTab(interactiveListItem2);
    }
    private void AddCrimes()
    {
        List<UIMenuItem> menuItems2 = new List<UIMenuItem>();
        if (Player.WantedCrimes != null)
        {
            foreach (Crime crime in Player.WantedCrimes.OrderByDescending(x => x.ResultingWantedLevel))
            {
                menuItems2.Add(new UIMenuItem(crime.Name, "") { RightLabel = $"Wanted Level: {crime.ResultingWantedLevel}" });
            }
        }
        //menuItems2[0].Activated += (m, s) => Game.DisplaySubtitle("Activated first item!");
        TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("Criminal History", menuItems2);
        tabView.AddTab(interactiveListItem2);
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

}

