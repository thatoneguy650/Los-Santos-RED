using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MessagesTab : ITabbableMenu
{
    private IGangRelateable Player;
    private TabView TabView;

    public MessagesTab(IGangRelateable player, TabView tabView)
    {
        Player = player;
        TabView = tabView;
    }

    public void AddItems()
    {
        List<TabItem> items = new List<TabItem>();
        bool addedItems = false;

        List<Tuple<string, DateTime>> MessageTimes = new List<Tuple<string, DateTime>>();

        MessageTimes.AddRange(Player.CellPhone.PhoneResponseList.OrderByDescending(x => x.TimeReceived).Take(15).Select(x => new Tuple<string, DateTime>(x.ContactName, x.TimeReceived)));
        MessageTimes.AddRange(Player.CellPhone.TextList.OrderByDescending(x => x.TimeReceived).Take(15).Select(x => new Tuple<string, DateTime>(x.ContactName, x.TimeReceived)));

        foreach (Tuple<string, DateTime> dateTime in MessageTimes.OrderByDescending(x => x.Item2).Take(15))
        {
            PhoneResponse pr = Player.CellPhone.PhoneResponseList.Where(x => x.TimeReceived == dateTime.Item2 && x.ContactName == dateTime.Item1).FirstOrDefault();
            if (pr != null)
            {
                string TimeReceived = pr.TimeReceived.ToString("HH:mm");
                string DescriptionText = "";
                DescriptionText += $"~n~Received At: {TimeReceived}";
                DescriptionText += $"~n~{pr.Message}";
                string ListEntryItem = $"{pr.ContactName} {TimeReceived}";
                string DescriptionHeaderText = $"{pr.ContactName}";
                TabItem tItem = new TabTextItem(ListEntryItem, DescriptionHeaderText, DescriptionText);
                items.Add(tItem);
                addedItems = true;
            }
            PhoneText text = Player.CellPhone.TextList.Where(x => x.TimeReceived == dateTime.Item2 && x.ContactName == dateTime.Item1).FirstOrDefault();
            if (text != null)
            {
                string TimeReceived = text.HourSent.ToString("00") + ":" + text.MinuteSent.ToString("00");// string.Format("{0:D2}h:{1:D2}m",text.HourSent,text.MinuteSent);
                string DescriptionText = "";
                DescriptionText += $"~n~Received At: {TimeReceived}";  //+ gr.ToStringBare();
                DescriptionText += $"~n~{text.Message}";
                string ListEntryItem = $"{text.ContactName}{(!text.IsRead ? " *" : "")} {TimeReceived}";
                string DescriptionHeaderText = $"{text.ContactName}";
                TabItem tItem = new TabTextItem(ListEntryItem, DescriptionHeaderText, DescriptionText);
                items.Add(tItem);
                addedItems = true;
            }
        }
        if (addedItems)
        {
            TabView.AddTab(new TabSubmenuItem("Recent", items));
        }
        else
        {
            TabView.AddTab(new TabItem("Recent"));
        }
    }
}

