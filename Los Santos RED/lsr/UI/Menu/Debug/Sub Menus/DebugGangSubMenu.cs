using LosSantosRED.lsr.Interface;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DebugGangSubMenu : DebugSubMenu
{
    private IGangs Gangs;
    private Dispatcher Dispatcher;
    public DebugGangSubMenu(UIMenu debug, MenuPool menuPool, IActionable player, IGangs gangs, Dispatcher dispatcher) : base(debug, menuPool, player)
    {
        Gangs = gangs;
        Dispatcher = dispatcher;
    }
    public override void AddItems()
    {
        UIMenu GangItems = MenuPool.AddSubMenu(Debug, "Gang Items");
        GangItems.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Debug Gang Items.";

        UIMenuListScrollerItem<Gang> SetAsGangMember = new UIMenuListScrollerItem<Gang>("Become Gang Member", "Become a gang member of the selected gang", Gangs.GetAllGangs());
        SetAsGangMember.Activated += (menu, item) =>
        {
            Player.RelationshipManager.GangRelationships.ResetGang(true);
            Player.RelationshipManager.GangRelationships.SetGang(SetAsGangMember.SelectedItem, true);
            menu.Visible = false;
        };
        UIMenuItem LeaveGang = new UIMenuItem("Leave Gang", "Leave your current gang");
        LeaveGang.Activated += (menu, item) =>
        {
            Player.RelationshipManager.GangRelationships.ResetGang(true);
            menu.Visible = false;
        };
        UIMenuListScrollerItem<Gang> SetGangRepDefault = new UIMenuListScrollerItem<Gang>("Set Gang Default", "Sets the selected gang to the default reputation", Gangs.GetAllGangs());
        SetGangRepDefault.Activated += (menu, item) =>
        {
            Player.RelationshipManager.GangRelationships.SetReputation(SetGangRepDefault.SelectedItem, 200, false);
            menu.Visible = false;
        };
        UIMenuListScrollerItem<Gang> SetGangRepFriendly = new UIMenuListScrollerItem<Gang>("Set Gang Friendly", "Sets the selected gang to a friendly reputation", Gangs.GetAllGangs());
        SetGangRepFriendly.Activated += (menu, item) =>
        {
            Player.RelationshipManager.GangRelationships.SetReputation(SetGangRepFriendly.SelectedItem, 5000, false);
            menu.Visible = false;
        };
        UIMenuListScrollerItem<Gang> SetGangRepHostile = new UIMenuListScrollerItem<Gang>("Set Gang Hostile", "Sets the selected gang to a hostile reputation", Gangs.GetAllGangs());
        SetGangRepHostile.Activated += (menu, item) =>
        {
            Player.RelationshipManager.GangRelationships.SetReputation(SetGangRepHostile.SelectedItem, -5000, false);
            menu.Visible = false;
        };
        UIMenuItem DefaultGangRep = new UIMenuItem("Set Gang Rep Default", "Sets the player reputation to each gang to the default value");
        DefaultGangRep.Activated += (menu, item) =>
        {
            Player.RelationshipManager.GangRelationships.Reset();
            menu.Visible = false;
        };
        UIMenuItem RandomGangRep = new UIMenuItem("Set Gang Rep Random", "Sets the player reputation to each gang to a randomized number");
        RandomGangRep.Activated += (menu, item) =>
        {
            Player.RelationshipManager.GangRelationships.SetAllRandomReputations();
            menu.Visible = false;
        };
        UIMenuItem RandomSingleGangRep = new UIMenuItem("Set Single Gang Rep Random", "Sets the player reputation to random gang to a randomized number");
        RandomSingleGangRep.Activated += (menu, item) =>
        {
            Player.RelationshipManager.GangRelationships.SetSingleRandomReputation();
            menu.Visible = false;
        };
        UIMenuItem HostileGangRep = new UIMenuItem("Set Gang Rep Hostile", "Sets the player reputation to each gang to hostile");
        HostileGangRep.Activated += (menu, item) =>
        {
            Player.RelationshipManager.GangRelationships.SetHostileReputations();
            menu.Visible = false;
        };
        UIMenuItem FriendlyGangRep = new UIMenuItem("Set Gang Rep Friendly", "Sets the player reputation to each gang to friendly");
        FriendlyGangRep.Activated += (menu, item) =>
        {
            Player.RelationshipManager.GangRelationships.SetFriendlyReputations();
            menu.Visible = false;
        };

        UIMenuItem DispatchHitSquad = new UIMenuItem("Dispatch Hit Squad", "Attempt to spawn a hit squad from enemy/hostile gangs");
        DispatchHitSquad.Activated += (menu, item) =>
        {
            Dispatcher.DebugSpawnHitSquad();
            menu.Visible = false;
        };

        GangItems.AddItem(SetAsGangMember);
        GangItems.AddItem(LeaveGang);
        GangItems.AddItem(SetGangRepDefault);
        GangItems.AddItem(SetGangRepFriendly);
        GangItems.AddItem(SetGangRepHostile);
        GangItems.AddItem(DefaultGangRep);
        GangItems.AddItem(RandomGangRep);
        GangItems.AddItem(RandomSingleGangRep);
        GangItems.AddItem(HostileGangRep);
        GangItems.AddItem(FriendlyGangRep);
        GangItems.AddItem(DispatchHitSquad);

    }
}

