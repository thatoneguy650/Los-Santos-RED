using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Windows.Media;
using static DispatchScannerFiles;


public class BusinessesTab : ITabbableMenu
{
    private ILocationInteractable Player;
    private TabSubmenuItem TabItem;
    private TabView TabView;
    private List<TabItem> items;
    public BusinessesTab(ILocationInteractable player, TabView tabView)
    {
        Player = player;
        TabView = tabView;
    }
    public void AddItems()
    {
        items = new List<TabItem>();
        bool canFocusTabItem = false;
        List<GameLocation> OwnedLocations = new List<GameLocation>();
        if (Player.Properties.PropertyList.OfType<Residence>().ToList() != null)
        {
            OwnedLocations.AddRange(Player.Properties.PropertyList.Where(x=> !(x is Residence)).ToList());
        }
        if (OwnedLocations.Count == 0)
        {
            TabTextItem ttx = new TabTextItem("No businesses", "", "You do not own any businesses.");
            ttx.CanBeFocused = false;
            items.Add(ttx);
        }
        else
        {
            foreach (GameLocation ownedLocation in OwnedLocations)
            {
                TabMissionSelectItem BusinessItem = ownedLocation.GetUIInformation();
                BusinessItem.OnItemSelect += (selectedItem) =>
                {
                    if (selectedItem != null && selectedItem.Name == "GPS")
                    {
                        Player.GPSManager.AddGPSRoute(ownedLocation.Name, ownedLocation.EntrancePosition, true);
                    }
                };
                items.Add(BusinessItem);
            }
            canFocusTabItem = true;
        }
        TabItem = new TabSubmenuItem("Businesses", items);
        TabItem.CanBeFocused = canFocusTabItem;
        TabView.AddTab(TabItem);
    }
}
