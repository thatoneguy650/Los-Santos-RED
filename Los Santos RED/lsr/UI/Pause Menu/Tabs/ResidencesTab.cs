using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ResidencesTab : ITabbableMenu
{
    private ILocationInteractable Player;
    private TabView TabView;
    private TabSubmenuItem TabItem;
    public List<TabItem> items;
    public ResidencesTab(ILocationInteractable player, TabView tabView)
    {
        Player = player;
        TabView = tabView;
    }
    public void AddItems()
    {
        if(items == null)
        {
            items = new List<TabItem>();
            TabTextItem ttx = new TabTextItem("Residences Owned", "", "");
            ttx.CanBeFocused = false;
            items.Add(ttx);
        }
        TabItem = new TabSubmenuItem("Residences", items);
        TabView.AddTab(TabItem);
    }
}
