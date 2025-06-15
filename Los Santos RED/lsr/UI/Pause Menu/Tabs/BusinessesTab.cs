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
    public List<TabItem> items;
    public BusinessesTab(ILocationInteractable player, TabView tabView)
    {
        Player = player;
        TabView = tabView;
    }
    public void AddItems()
    {
        if(items == null)
        {
            items = new List<TabItem>();
            TabTextItem ttx = new TabTextItem("Businesses Owned", "", "");
            ttx.CanBeFocused = false;
            items.Add(ttx);
        }
        TabItem = new TabSubmenuItem("Businesses", items);
        TabView.AddTab(TabItem);
    }
}
