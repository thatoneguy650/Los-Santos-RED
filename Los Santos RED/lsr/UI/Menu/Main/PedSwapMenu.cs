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

        TakeoverRandomPed = new UIMenuListScrollerItem<DistanceSelect>("Takeover Random Pedestrian", "Takes over a random pedestrian around the player.", Distances);
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

        BecomeRandomPed = new UIMenuItem("Become Random Pedestrian", "Becomes a random ped model.");
        BecomeRandomPed.Activated += (menu, item) =>
        {
            PedSwap.BecomeRandomPed();
            PedSwapUIMenu.Visible = false;
        };
        PedSwapUIMenu.AddItem(BecomeRandomPed);


        SetAsGangMember = new UIMenuListScrollerItem<Gang>("Become Gang Member", "Become a random gang member of the selected gang", Gangs.GetAllGangs());
        SetAsGangMember.Activated += (menu, item) =>
        {
            PedSwap.BecomeGangMember(SetAsGangMember.SelectedItem);
            PedSwapUIMenu.Visible = false;
        };
        PedSwapUIMenu.AddItem(SetAsGangMember);


        SetAsCop = new UIMenuListScrollerItem<Agency>("Become Cop", "Become a random cop from the selected agency. ~r~WIP~s~ Functionality will be expanded later", Agencies.GetAgencies().Where(x=> x.ResponseType == ResponseType.LawEnforcement));
        SetAsCop.Activated += (menu, item) =>
        {
            PedSwap.BecomeCop(SetAsCop.SelectedItem);
            PedSwapUIMenu.Visible = false;
        };
        //PedSwapUIMenu.AddItem(SetAsCop);


        BecomeCustomPed = new UIMenuItem("Become Custom Pedestrian (Legacy)", "Becomes a customized ped from user input. (Old Version)");
        BecomeCustomPed.Activated += (menu, item) =>
        {
            PedSwap.BecomeCustomPed();
            PedSwapUIMenu.Visible = false;
        };
        PedSwapUIMenu.AddItem(BecomeCustomPed);




        //SetAsCop = new UIMenuItem("Set as Cop", "Treat the current player model as a cop without any changes.");
        //SetAsCop.Activated += (menu, item) =>
        //{
        //    PedSwap.TreatAsCop();
        //    PedSwapUIMenu.Visible = false;
        //};


        //SetAsCivilian = new UIMenuItem("Set as Civilian", "Treat the current player model as a civilian without any changes.");
        //SetAsCivilian.Activated += (menu, item) =>
        //{
        //    PedSwap.TreatAsCivilian();
        //    PedSwapUIMenu.Visible = false;
        //};


    }
}