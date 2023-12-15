using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

public class PedSwapMenu : ModUIMenu
{
    private MenuPool MenuPool;
    private UIMenu ParentMenu;

    private UIMenu PedSwapUIMenu;
    private UIMenuListScrollerItem<DistanceSelect> TakeoverRandomPed;
    private UIMenuItem BecomeRandomPed;
    private UIMenuItem BecomeCustomPed;
    private UIMenuListScrollerItem<Agency> SetAsCop;
    private UIMenuItem SetAsCivilian;
    private UIMenuListScrollerItem<Gang> SetAsGangMember;
    private List<DistanceSelect> Distances;

    private IPedSwap PedSwap;
    private IGangs Gangs;
    private IAgencies Agencies;
    private IActionable Player;
    private UIMenuItem BecomeCustomPed2;
    private UIMenuListScrollerItem<Agency> SetAsEMT;
    private UIMenuListScrollerItem<Agency> SetAsFireFighter;
    private UIMenuListScrollerItem<Agency> SetAsSecurity;

    public PedSwapMenu(MenuPool menuPool, UIMenu parentMenu, IPedSwap pedSwap, IGangs gangs, IAgencies agencies, IActionable player)
    {
        MenuPool = menuPool;
        ParentMenu = parentMenu;
        PedSwap = pedSwap;
        Gangs = gangs;
        Agencies = agencies;
        Player = player;
    }
    public void Setup()
    {
        Distances = new List<DistanceSelect> { new DistanceSelect("Closest", -1f), new DistanceSelect("20 M", 20f), new DistanceSelect("40 M", 40f), new DistanceSelect("100 M", 100f), new DistanceSelect("500 M", 500f), new DistanceSelect("Any", 1000f) };
        PedSwapUIMenu = MenuPool.AddSubMenu(ParentMenu, "Ped Swap");
        ParentMenu.MenuItems[ParentMenu.MenuItems.Count() - 1].Description = "Change your character by taking over an existing ped, editing the current ped or creating a ped from scratch.";
        ParentMenu.MenuItems[ParentMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Clothes;
        PedSwapUIMenu.SetBannerType(System.Drawing.Color.FromArgb(181, 48, 48));
        CreatePedSwap();
    }
    public float SelectedTakeoverRadius { get; set; } = -1f;
    public override void Hide()
    {
        PedSwapUIMenu.Visible = false;
    }

    public override void Show()
    {
        Update();
        PedSwapUIMenu.Visible = true;
    }
    public override void Toggle()
    {

        if (!PedSwapUIMenu.Visible)
        {
            PedSwapUIMenu.Visible = true;
        }
        else
        {
            PedSwapUIMenu.Visible = false;
        }
    }
    public void Update()
    {
        CreatePedSwap();
    }
    private void CreatePedSwap()
    {
        PedSwapUIMenu.Clear();


        BecomeCustomPed2 = new UIMenuItem("Ped Update/Create", "Update your existing ped or create a new customized ped from user input.");
        BecomeCustomPed2.Activated += (menu, item) =>
        {
            //if(Player.InteriorManager.IsInsideTeleportInterior)
            //{
            //    Game.DisplayHelp("Cannot swap ped inside teleport interior");
            //    return;
            //}
            PedSwap.BecomeCreatorPed();
            PedSwapUIMenu.Visible = false;
        };
        PedSwapUIMenu.AddItem(BecomeCustomPed2);

        TakeoverRandomPed = new UIMenuListScrollerItem<DistanceSelect>("Takeover Random Pedestrian", "Takes over a random ~r~civilian~s~ pedestrian around the player.", Distances);
        TakeoverRandomPed.Activated += (s, e) =>
        {
            if (Player.InteriorManager.IsInsideTeleportInterior)
            {
                Game.DisplayHelp("Cannot swap ped inside teleport interior");
                return;
            }
            if (TakeoverRandomPed.SelectedItem.Distance == -1f)
            {
                PedSwap.BecomeExistingPed(500f, true, false, true, false);
            }
            else
            {
                PedSwap.BecomeExistingPed(TakeoverRandomPed.SelectedItem.Distance, false, false, true, false);
            }
            PedSwapUIMenu.Visible = false;
        };
        PedSwapUIMenu.AddItem(TakeoverRandomPed);

        BecomeRandomPed = new UIMenuItem("Become Random Pedestrian", "Becomes a random ~r~civilian~s~ ped model.");
        BecomeRandomPed.Activated += (menu, item) =>
        {
            if (Player.InteriorManager.IsInsideTeleportInterior)
            {
                Game.DisplayHelp("Cannot swap ped inside teleport interior");
                return;
            }
            PedSwap.BecomeRandomPed();
            PedSwapUIMenu.Visible = false;
        };
        PedSwapUIMenu.AddItem(BecomeRandomPed);


        SetAsGangMember = new UIMenuListScrollerItem<Gang>("Become Gang Member", "Become a random ~r~gang member~s~ of the selected gang. When you join, you join for life.", Gangs.GetAllGangs());
        SetAsGangMember.Activated += (menu, item) =>
        {
            if (Player.InteriorManager.IsInsideTeleportInterior)
            {
                Game.DisplayHelp("Cannot swap ped inside teleport interior");
                return;
            }
            PedSwap.BecomeGangMember(SetAsGangMember.SelectedItem);
            PedSwapUIMenu.Visible = false;
        };
        PedSwapUIMenu.AddItem(SetAsGangMember);

        SetAsCop = new UIMenuListScrollerItem<Agency>("Become Cop", "Become a random ~b~Police Officer~s~ from the selected agency. Become part of the REAL biggest gang around. Use your police powers to enrich yourself and terrorize your enemies. Be on the lookout for nosey civilians and so-called 'good' cops when you make your moves. ~r~WIP Most Features TBD~s~", Agencies.GetAgencies().Where(x=> x.ResponseType == ResponseType.LawEnforcement));
        SetAsCop.Activated += (menu, item) =>
        {
            if (Player.InteriorManager.IsInsideTeleportInterior)
            {
                Game.DisplayHelp("Cannot swap ped inside teleport interior");
                return;
            }
            PedSwap.BecomeCop(SetAsCop.SelectedItem);
            PedSwapUIMenu.Visible = false;
        };
        PedSwapUIMenu.AddItem(SetAsCop);

        SetAsSecurity = new UIMenuListScrollerItem<Agency>("Become Security", "Become a random ~y~Security Guard~s~ from the selected agency. Want to walk around with a gun without questions? Not accepted by the real police? Join a security agency and become the king of your small castle.~r~WIP Most Features TBD~s~", Agencies.GetAgencies().Where(x => x.ResponseType == ResponseType.Security));
        SetAsSecurity.Activated += (menu, item) =>
        {
            if (Player.InteriorManager.IsInsideTeleportInterior)
            {
                Game.DisplayHelp("Cannot swap ped inside teleport interior");
                return;
            }
            PedSwap.BecomeSecurity(SetAsSecurity.SelectedItem);
            PedSwapUIMenu.Visible = false;
        };
        PedSwapUIMenu.AddItem(SetAsSecurity);

        SetAsEMT = new UIMenuListScrollerItem<Agency>("Become EMT", "Become a random ~w~EMT~s~ from the selected agency. Treat the unwashed masses, or don't. Got sticky fingers? They won't miss it if they are dead. ~r~WIP Most Features TBD~s~", Agencies.GetAgencies().Where(x => x.ResponseType == ResponseType.EMS));
        SetAsEMT.Activated += (menu, item) =>
        {
            if (Player.InteriorManager.IsInsideTeleportInterior)
            {
                Game.DisplayHelp("Cannot swap ped inside teleport interior");
                return;
            }
            PedSwap.BecomeEMT(SetAsEMT.SelectedItem);
            PedSwapUIMenu.Visible = false;
        };
        PedSwapUIMenu.AddItem(SetAsEMT);

        SetAsFireFighter = new UIMenuListScrollerItem<Agency>("Become FireFighter", "Become a random ~r~Firefighter~s~ from the selected agency. You've seen Backdraft right? ~r~WIP Most Features TBD~s~", Agencies.GetAgencies().Where(x => x.ResponseType == ResponseType.Fire));
        SetAsFireFighter.Activated += (menu, item) =>
        {
            if (Player.InteriorManager.IsInsideTeleportInterior)
            {
                Game.DisplayHelp("Cannot swap ped inside teleport interior");
                return;
            }
            PedSwap.BecomeFireFighter(SetAsFireFighter.SelectedItem);
            PedSwapUIMenu.Visible = false;
        };
        PedSwapUIMenu.AddItem(SetAsFireFighter);
    }
}