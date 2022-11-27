using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class LocationsTab
{
    private IPlacesOfInterest PlacesOfInterest;
    private IGangRelateable Player;
    private ITimeReportable Time;
    private ISettingsProvideable Settings;

    private TabSubmenuItem dynamicLocationsTabSubmenuITem;
    private string FilterString = "";
    private TabTextItem ttx;
    private TabView TabView;
    private TabTextItem removeGPSTTI;

    public LocationsTab(IGangRelateable player, IPlacesOfInterest placesOfInterest, ITimeReportable time, ISettingsProvideable settings, TabView tabView)
    {
        Player = player;
        PlacesOfInterest = placesOfInterest;
        Time = time;
        Settings = settings;
        TabView = tabView;
    }

    public void AddItems()
    {
        FilterString = "";
        List<TabItem> items = new List<TabItem>();
        removeGPSTTI = new TabTextItem("Remove GPS", "Remove GPS", "Remove the GPS Blip");
        removeGPSTTI.Activated += (s, e) =>
        {
            Player.GPSManager.RemoveGPSRoute();
        };
        items.Add(removeGPSTTI);

        ttx = new TabTextItem("Search For Location", "Search For Location", $"Search for a location, select to enter search criteria");
        ttx.Activated += (s, e) =>
        {
            FilterString = NativeHelper.GetKeyboardInput("");
            ttx.Text = $"Search for a location, select to enter search criteria~n~Current Search String: ~h~{FilterString}~h~";
            for (int i = dynamicLocationsTabSubmenuITem.Items.Count; i-- > 0;)
            {
                if (dynamicLocationsTabSubmenuITem.Items[i].Title != ttx.Title && dynamicLocationsTabSubmenuITem.Items[i].Title != removeGPSTTI.Title)
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
        TabView.AddTab(dynamicLocationsTabSubmenuITem);
    }
    private List<TabItem> GetDirectoryLocations()
    {
        List<TabItem> items = new List<TabItem>();
        List<BasicLocation> DirectoryLocations = PlacesOfInterest.AllLocations().Where(x => (x.ShowsOnDirectory || Settings.SettingsManager.WorldSettings.ShowAllLocationsOnDirectory) && x.IsEnabled && (FilterString == "" || x.Name.ToLower().Contains(FilterString.ToLower()))).ToList();
        foreach (string typeName in DirectoryLocations.OrderBy(x => x.TypeName).Select(x => x.TypeName).Distinct())
        {
            List<MissionInformation> missionInfoList = new List<MissionInformation>();
            foreach (BasicLocation bl in DirectoryLocations.Where(x => x.TypeName == typeName).OrderBy(x=> x.SortOrder).ThenBy(x => Player.Character.DistanceTo2D(x.EntrancePosition)))
            {
                MissionInformation locationInfo = new MissionInformation(bl.Name, "", bl.DirectoryInfo(Time.CurrentHour, Player.Character.DistanceTo2D(bl.EntrancePosition)));
                if (bl.HasBannerImage)
                {
                    locationInfo.Logo = new MissionLogo(Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{bl.BannerImagePath}"));
                }
                missionInfoList.Add(locationInfo);
            }
            TabMissionSelectItem basicLocationTypeList = new TabMissionSelectItem(typeName, missionInfoList);
            basicLocationTypeList.OnItemSelect += (selectedItem) =>
            {
                if (selectedItem != null)
                {
                    string streetAddress = selectedItem.ValueList.FirstOrDefault(x => x.Item1 == "Address:")?.Item2;
                    BasicLocation toGPS = DirectoryLocations.FirstOrDefault(x => x.Name == selectedItem.Name && x.StreetAddress == streetAddress);
                    if (toGPS != null)
                    {
                        Player.GPSManager.AddGPSRoute(toGPS.Name, toGPS.EntrancePosition);
                    }
                }
            };
            items.Add(basicLocationTypeList);
        }
        return items;
    }
}

