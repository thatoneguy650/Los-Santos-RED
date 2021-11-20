using LosSantosRED.lsr.Interface;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Collections.Generic;

public class PedSwapMenu : Menu
{
    private UIMenu PedSwapUIMenu;
    private UIMenuListItem TakeoverRandomPed;
    private UIMenuItem BecomeRandomPed;
    private UIMenuItem BecomeRandomCop;
    private IPedSwap PedSwap;
    private List<DistanceSelect> Distances;
    public PedSwapMenu(MenuPool menuPool, UIMenu parentMenu, IPedSwap pedSwap)
    {
        PedSwap = pedSwap;
        PedSwapUIMenu = menuPool.AddSubMenu(parentMenu, "Ped Swap");
        PedSwapUIMenu.OnItemSelect += OnActionItemSelect;
        PedSwapUIMenu.OnListChange += OnListChange;
        CreatePedSwap();
    }
    public float SelectedTakeoverRadius { get; set; }
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
        BecomeRandomCop = new UIMenuItem("Become Random Cop", "Becomes a random cop around the player (Alpha)");
        PedSwapUIMenu.AddItem(TakeoverRandomPed);
        PedSwapUIMenu.AddItem(BecomeRandomPed);
        PedSwapUIMenu.AddItem(BecomeRandomCop);
    }
    private void OnActionItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
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