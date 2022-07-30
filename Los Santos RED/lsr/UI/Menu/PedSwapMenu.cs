using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

public class PedSwapMenu : Menu
{
    private UIMenu PedSwapUIMenu;
    private UIMenuListItem TakeoverRandomPed;
    private UIMenuItem BecomeRandomPed;

    private UIMenuItem BecomeCustomPed;

    //private UIMenu PedSwapCustomizeUIMenu;
    private UIMenuItem BecomeRandomCop;
    private UIMenuItem AddOffset;
    private UIMenuItem RemoveOffset;
    private UIMenuItem SetAsCop;
    private UIMenuItem SetAsCivilian;
    private UIMenuListScrollerItem<Gang> SetAsGangMember;
    private IPedSwap PedSwap;
    private List<DistanceSelect> Distances;
    private IGangs Gangs;
   // private PedSwapCustomMenu PedSwapCustomMenu;
    public PedSwapMenu(MenuPool menuPool, UIMenu parentMenu, IPedSwap pedSwap, IGangs gangs)
    {
        PedSwap = pedSwap;
        Gangs = gangs;
        PedSwapUIMenu = menuPool.AddSubMenu(parentMenu, "Ped Swap");
        parentMenu.MenuItems[parentMenu.MenuItems.Count() - 1].Description = "Change your character by taking over an existing ped or creating a ped from scratch.";
        parentMenu.MenuItems[parentMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Clothes;
        PedSwapUIMenu.SetBannerType(System.Drawing.Color.FromArgb(181, 48, 48));
        PedSwapUIMenu.OnItemSelect += OnItemSelect;
        PedSwapUIMenu.OnListChange += OnListChange;




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
        Distances = new List<DistanceSelect> { new DistanceSelect("Closest", -1f), new DistanceSelect("20 M", 20f), new DistanceSelect("40 M", 40f), new DistanceSelect("100 M", 100f), new DistanceSelect("500 M", 500f), new DistanceSelect("Any", 1000f) };
        TakeoverRandomPed = new UIMenuListItem("Takeover Random Pedestrian", "Takes over a random pedestrian around the player.", Distances);
        BecomeRandomPed = new UIMenuItem("Become Random Pedestrian", "Becomes a random ped model.");
        BecomeCustomPed = new UIMenuItem("Become Custom Pedestrian", "Becomes a customized ped from user input.");
        BecomeRandomCop = new UIMenuItem("Become Random Cop", "Becomes a random cop around the player (Alpha)");

        AddOffset = new UIMenuItem("Re-Alias Current Ped as Main Character", "Re-Alias the current model name as a main character. The game will see your current model as either player_zero, player_one, or player_two depending on your settings.");
        RemoveOffset = new UIMenuItem("Remove Current Ped Alias", "Remove any aliasing for the current model name. The game will see your current model as it originally is.");


        SetAsCop = new UIMenuItem("Set as Cop", "Treat the current player model as a cop without any changes.");
        SetAsCivilian = new UIMenuItem("Set as Civilian", "Treat the current player model as a civilian without any changes.");

        SetAsGangMember = new UIMenuListScrollerItem<Gang>("Become Gang Member", "Become a gang member of the selected gang", Gangs.GetAllGangs());
        SetAsGangMember.Activated += (menu, item) =>
        {
            PedSwap.BecomeGangMember(SetAsGangMember.SelectedItem);
            PedSwapUIMenu.Visible = false;
        };





        PedSwapUIMenu.AddItem(TakeoverRandomPed);
        PedSwapUIMenu.AddItem(BecomeRandomPed);
        PedSwapUIMenu.AddItem(BecomeCustomPed);
        PedSwapUIMenu.AddItem(SetAsGangMember);
        //PedSwapUIMenu.AddItem(BecomeRandomCop);
        // PedSwapUIMenu.AddItem(AddOffset);
        // PedSwapUIMenu.AddItem(RemoveOffset);

        //PedSwapUIMenu.AddItem(SetAsCop);
        //PedSwapUIMenu.AddItem(SetAsCivilian);
    }
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == TakeoverRandomPed)
        {
            if (SelectedTakeoverRadius == -1f)
            {
                PedSwap.BecomeExistingPed(500f, true, false, true, false);
            }
            else
            {
                PedSwap.BecomeExistingPed(SelectedTakeoverRadius, false, false, true, false);
            }
        }
        else if (selectedItem == BecomeRandomPed)
        {
            PedSwap.BecomeRandomPed();
        }
        else if (selectedItem == BecomeRandomCop)
        {
            PedSwap.BecomeRandomCop();
        }
        else if (selectedItem == BecomeCustomPed)
        {
            PedSwap.BecomeCustomPed();
        }
        else if (selectedItem == AddOffset)
        {
            PedSwap.AddOffset();
        }
        else if (selectedItem == RemoveOffset)
        {
            PedSwap.RemoveOffset();
        }
        else if (selectedItem == SetAsCop)
        {
            PedSwap.TreatAsCop();
        }
        else if (selectedItem == SetAsCivilian)
        {
            PedSwap.TreatAsCivilian();
        }
        PedSwapUIMenu.Visible = false;
    }
    private void OnListChange(UIMenu sender, UIMenuListItem list, int index)
    {
        if (list == TakeoverRandomPed)
        {
            SelectedTakeoverRadius = Distances[index].Distance;
        }
    }
}