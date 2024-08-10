using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ZonesTab
{
    private IZones Zones;

    private TabView TabView;
    private List<TabItem> items;
    private bool addedItems;

    public ZonesTab(IZones zones, TabView tabView)
    {
        Zones = zones;
        TabView = tabView;
    }
    public void AddItems()
    {
        items = new List<TabItem>();
        addedItems = false;
        foreach (Zone zone in Zones.ZoneList.OrderBy(x => x.DisplayName))
        {
            AddItem(zone);
        }
        if (addedItems)
        {
            TabView.AddTab(new TabSubmenuItem("Zones", items));
        }
    }
    private void AddItem(Zone zone)
    {
        items.Add(new TabItem(zone.DisplayName));
        addedItems = true;
    }
}