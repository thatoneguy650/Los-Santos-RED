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

    public ResidencesTab(ILocationInteractable player, TabView tabView)
    {
        Player = player;
        TabView = tabView;
    }
    public void AddItems()
    {
        List<TabItem> items = new List<TabItem>();
        bool canFocusTabItem = false;
        List<Residence> ResidenceList = new List<Residence>();
        if(Player.Properties.PropertyList.OfType<Residence>().ToList()!=null)
        {
            ResidenceList.AddRange(Player.Properties.PropertyList.OfType<Residence>().ToList());
        }
        if (ResidenceList.Count == 0)
        {
            TabTextItem ttx = new TabTextItem("No Residences", "", "You do not own any residences.");
            ttx.CanBeFocused = false;
            items.Add(ttx);
        }
        else
        {
            foreach (Residence residence in ResidenceList)
            {
                TabTextItem ttx = new TabTextItem(residence.Name, "", GetResidenceInformation(residence));
                ttx.Activated += (s, e) =>
                {
                    Player.GPSManager.AddGPSRoute(residence.Name, residence.EntrancePosition, true);
                };
                items.Add(ttx);
            }
            canFocusTabItem = true;
        }
        TabItem = new TabSubmenuItem("Residences", items);
        TabItem.CanBeFocused = canFocusTabItem;
        TabView.AddTab(TabItem);
    }
    private string GetResidenceInformation(Residence residence)
    {
        StringBuilder residenceInformation = new StringBuilder();
        if(residence.IsOwned)
        {
            residenceInformation.AppendLine("~w~Status: ~g~Owned");
        }
        else
        {
            residenceInformation.AppendLine("~w~Status: ~o~Rented");
        }
        return residenceInformation.ToString();
    }
}
