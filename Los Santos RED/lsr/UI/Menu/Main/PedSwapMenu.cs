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
    private UIMenuItem BecomeCustomPed2;
    private UIMenuListScrollerItem<Agency> SetAsEMT;
    private UIMenuListScrollerItem<Agency> SetAsFireFighter;

    public PedSwapMenu(MenuPool menuPool, UIMenu parentMenu, IPedSwap pedSwap, IGangs gangs, IAgencies agencies)
    {
        MenuPool = menuPool;
        ParentMenu = parentMenu;
        PedSwap = pedSwap;
        Gangs = gangs;
        Agencies = agencies;
    }
    public void Setup()
    {
        Distances = new List<DistanceSelect> { new DistanceSelect("Closest", -1f), new DistanceSelect("20 M", 20f), new DistanceSelect("40 M", 40f), new DistanceSelect("100 M", 100f), new DistanceSelect("500 M", 500f), new DistanceSelect("Any", 1000f) };
        PedSwapUIMenu = MenuPool.AddSubMenu(ParentMenu, "Ped Swap");
        ParentMenu.MenuItems[ParentMenu.MenuItems.Count() - 1].Description = "Change your character by taking over an existing ped or creating a ped from scratch.";
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


        BecomeCustomPed2 = new UIMenuItem("Ped Creator", "Becomes a customized ped from user input.");
        BecomeCustomPed2.Activated += (menu, item) =>
        {
            PedSwap.BecomeCreatorPed();
            PedSwapUIMenu.Visible = false;
        };
        PedSwapUIMenu.AddItem(BecomeCustomPed2);

        TakeoverRandomPed = new UIMenuListScrollerItem<DistanceSelect>("Takeover Random Pedestrian", "Takes over a random ~r~civilian~s~ pedestrian around the player.", Distances);
        TakeoverRandomPed.Activated += (s, e) =>
        {
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
            PedSwap.BecomeRandomPed();
            PedSwapUIMenu.Visible = false;
        };
        PedSwapUIMenu.AddItem(BecomeRandomPed);


        SetAsGangMember = new UIMenuListScrollerItem<Gang>("Become Gang Member", "Become a random ~r~gang member~s~ of the selected gang", Gangs.GetAllGangs());
        SetAsGangMember.Activated += (menu, item) =>
        {
            PedSwap.BecomeGangMember(SetAsGangMember.SelectedItem);
            PedSwapUIMenu.Visible = false;
        };
        PedSwapUIMenu.AddItem(SetAsGangMember);

        SetAsCop = new UIMenuListScrollerItem<Agency>("Become Cop", "Become a random ~r~cop~s~ from the selected agency. ~r~WIP~s~ Functionality will be expanded later", Agencies.GetAgencies().Where(x=> x.ResponseType == ResponseType.LawEnforcement));
        SetAsCop.Activated += (menu, item) =>
        {
            PedSwap.BecomeCop(SetAsCop.SelectedItem);
            PedSwapUIMenu.Visible = false;
        };
#if DEBUG
        PedSwapUIMenu.AddItem(SetAsCop);
#endif

        SetAsEMT = new UIMenuListScrollerItem<Agency>("Become EMT", "Become a random ~r~EMT~s~ from the selected agency. ~r~WIP~s~ Functionality will be expanded later", Agencies.GetAgencies().Where(x => x.ResponseType == ResponseType.EMS));
        SetAsEMT.Activated += (menu, item) =>
        {
            PedSwap.BecomeEMT(SetAsEMT.SelectedItem);
            PedSwapUIMenu.Visible = false;
        };
        PedSwapUIMenu.AddItem(SetAsEMT);

        SetAsFireFighter = new UIMenuListScrollerItem<Agency>("Become FireFighter", "Become a random ~r~Firefighter~s~ from the selected agency. ~r~WIP~s~ Functionality will be expanded later", Agencies.GetAgencies().Where(x => x.ResponseType == ResponseType.Fire));
        SetAsFireFighter.Activated += (menu, item) =>
        {
            PedSwap.BecomeFireFighter(SetAsFireFighter.SelectedItem);
            PedSwapUIMenu.Visible = false;
        };
        PedSwapUIMenu.AddItem(SetAsFireFighter);
    }
}