using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TextMessagesTab : ITabbableMenu
{
    private IGangRelateable Player;
    private TabView TabView;

    public TextMessagesTab(IGangRelateable player, TabView tabView)
    {
        Player = player;
        TabView = tabView;
    }

    public void AddItems()
    {
        List<TabItem> items = new List<TabItem>();
        bool addedItems = false;
        foreach (PhoneText text in Player.CellPhone.TextList.OrderByDescending(x => x.TimeReceived).Take(15))
        {
            string TimeReceived = text.HourSent.ToString("00") + ":" + text.MinuteSent.ToString("00");// string.Format("{0:D2}h:{1:D2}m",text.HourSent,text.MinuteSent);

            string DescriptionText = "";
            DescriptionText += $"~n~Received At: {TimeReceived}";  //+ gr.ToStringBare();
            DescriptionText += $"~n~{text.Message}";
            //DescriptionText += $"~n~~n~Select to ~r~Delete Message~s~";
            string ListEntryItem = $"{text.ContactName}{(!text.IsRead ? " *" : "")} {TimeReceived}";
            string DescriptionHeaderText = $"{text.ContactName}";
            TabItem tItem = new TabTextItem(ListEntryItem, DescriptionHeaderText, DescriptionText);
            //tItem.Activated += (s, e) =>
            //{
            //    if (text != null)
            //    {
            //        Player.CellPhone.DeleteText(text);
            //        TextMessagesSubMenu.Items.Remove(tItem);
            //        TextMessagesSubMenu.RefreshIndex();
            //        EntryPoint.WriteToConsole($"Text Message deleted {text.Name} {text.Message}");
            //    }
            //};
            items.Add(tItem);
            addedItems = true;
        }

        TabItem ClearTexts = new TabTextItem("Clear Text Messages", "Clear Text Messages", "Select to clear all ~o~Text Messages~s~");//TabItem tabItem = new TabTextItem($"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~ {gr.ToBlip()}~s~", $"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~", DescriptionText);
        ClearTexts.Activated += (s, e) =>
        {
            TabView.Visible = false;
            Game.IsPaused = false;
            Player.CellPhone.ClearTextMessages();
        };
        items.Add(ClearTexts);
        if (addedItems)
        {
            TabView.AddTab(new TabSubmenuItem("Texts", items));
        }
        else
        {
            TabView.AddTab(new TabItem("Texts"));
        }
    }
}

