using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.Elements;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CrimesTab
{
    private IGangRelateable Player;
    private TabView TabView;

    public CrimesTab(IGangRelateable player, TabView tabView)
    {
        Player = player;
        TabView = tabView;
    }

    public void AddItems()
    {
        List<UIMenuItem> menuItems2 = new List<UIMenuItem>();
        bool addedItems = false;
        if (Player.IsWanted)
        {
            foreach (CrimeEvent crime in Player.PoliceResponse.CrimesObserved.OrderByDescending(x => x.AssociatedCrime?.ResultingWantedLevel))
            {
                string crimeText = crime.AssociatedCrime.Name;
                crimeText += $" Instances: ({crime.Instances})";
                menuItems2.Add(new UIMenuItem(crimeText, "") { RightLabel = $"Wanted Level: {crime.AssociatedCrime.ResultingWantedLevel}" });
                addedItems = true;
            }
            TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("Crimes", menuItems2);
           TabView.AddTab(interactiveListItem2);
        }
        else if (Player.CriminalHistory.WantedCrimes != null)
        {
            foreach (Crime crime in Player.CriminalHistory.WantedCrimes.OrderByDescending(x => x.ResultingWantedLevel))
            {
                menuItems2.Add(new UIMenuItem(crime.Name, "") { RightLabel = $"Wanted Level: {crime.ResultingWantedLevel}" });
                addedItems = true;
            }
            TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("Crimes", menuItems2);
            TabView.AddTab(interactiveListItem2);
        }
        else
        {
            TabView.AddTab(new TabItem("Crimes"));
        }
    }
}

