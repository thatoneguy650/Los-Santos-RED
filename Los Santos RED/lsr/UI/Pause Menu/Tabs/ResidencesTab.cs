using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
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
        if (Player.Properties.Residences.Count == 0)
        {
            TabTextItem ttx = new TabTextItem("No Residences", "", "You do not own any residences.");
            ttx.CanBeFocused = false;
            items.Add(ttx);
        }
        else
        {
            foreach (Residence residence in Player.Properties.Residences)
            {
                TabTextItem ttx = new TabTextItem(residence.Name, "",GetResidenceInformation(residence));
                ttx.Activated += (s, e) =>
                {
                    Player.GPSManager.AddGPSRoute(residence.Name, residence.EntrancePosition, true);
                };
                items.Add(ttx);
            }
            canFocusTabItem=true;
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
        if(residence.IsRentedOut)
        {
            residenceInformation.AppendLine($"~w~Rented Out: Earning ~g~${residence.RentalFee} ~w~every ~g~{residence.RentalDays}~w~ day(s).");
            if(residence.CashStorage !=null && residence.CashStorage.StoredCash > 0)
            {
                residenceInformation.AppendLine($"~w~Accumulated: ~g~${residence.CashStorage.StoredCash}");
            }
        }
        return residenceInformation.ToString();
    }
}
