using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ContactsTab : ITabbableMenu
{
    private IGangRelateable Player;
    private IGangs Gangs;
    private TabView TabView;

    public ContactsTab(IGangRelateable player, IGangs gangs, TabView tabView)
    {
        Player = player;
        Gangs = gangs;
        TabView = tabView;
    }

    public void AddItems()
    {
        List<TabItem> items = new List<TabItem>();
        bool addedItems = false;
        foreach (PhoneContact contact in Player.CellPhone.ContactList.OrderBy(x => x.Name))
        {
            string DescriptionText = "Select to ~o~Call~s~ the contact";
            string Title = contact.Name;
            string SubTitle = contact.Name;
            Gang myGang = Gangs.GetGangByContact(contact.Name);
            if (myGang != null)
            {
                GangReputation gr = Player.RelationshipManager.GangRelationships.GangReputations.FirstOrDefault(x => x.Gang.ID == myGang.ID);
                if (gr != null)
                {
                    Title = $"{contact.Name} {gr.ToBlip()}~s~";
                    SubTitle = $"{gr.Gang.ColorPrefix}{contact.Name}~s~";
                }
            }

            PlayerTask contactTask = Player.PlayerTasks.GetTask(contact.Name);
            if (contactTask != null)
            {
                DescriptionText += $"~n~~g~Has Task~s~";
                if (contactTask.CanExpire)
                {
                    DescriptionText += $" Complete Before: ~r~{contactTask.ExpireTime:d} {contactTask.ExpireTime:t}~s~";
                }
            }


            TabItem tabItem = new TabTextItem(Title, SubTitle, DescriptionText);//TabItem tabItem = new TabTextItem($"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~ {gr.ToBlip()}~s~", $"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~", DescriptionText);
            tabItem.Activated += (s, e) =>
            {
                //Game.DisplaySubtitle("Activated Submenu Item #" + ContactsSubMenu.Index + " "+ contact.Name, 5000);
                TabView.Visible = false;
                Game.IsPaused = false;
                Player.CellPhone.ContactAnswered(contact);
            };
            items.Add(tabItem);
            addedItems = true;
        }
        if (addedItems)
        {
            TabView.AddTab(new TabSubmenuItem("Contacts", items));
        }
        else
        {
            TabView.AddTab(new TabItem("Contacts"));
        }
    }
}

