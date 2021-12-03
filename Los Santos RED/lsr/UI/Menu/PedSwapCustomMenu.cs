using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;

public class PedSwapCustomMenu : Menu
{
    private UIMenu PedSwapCustomUIMenu;
    private UIMenuListItem TakeoverRandomPed;
    private UIMenuItem BecomeRandomPed;
    private UIMenuItem BecomeRandomCop;
    private IPedSwap PedSwap;
    private List<DistanceSelect> Distances;
    public PedSwapCustomMenu(MenuPool menuPool, UIMenu parentMenu, IPedSwap pedSwap)
    {
        PedSwap = pedSwap;
        PedSwapCustomUIMenu = menuPool.AddSubMenu(parentMenu, "Become Custom Ped");
        PedSwapCustomUIMenu.SetBannerType(System.Drawing.Color.FromArgb(181, 48, 48));
        PedSwapCustomUIMenu.OnItemSelect += OnActionItemSelect;
        PedSwapCustomUIMenu.OnListChange += OnListChange;
        CreatePedSwap();
    }
    public float SelectedTakeoverRadius { get; set; } = -1f;
    public override void Hide()
    {
        PedSwapCustomUIMenu.Visible = false;
    }

    public override void Show()
    {
        Update();
        PedSwapCustomUIMenu.Visible = true;
    }
    public override void Toggle()
    {

        if (!PedSwapCustomUIMenu.Visible)
        {
            PedSwapCustomUIMenu.Visible = true;
        }
        else
        {
            PedSwapCustomUIMenu.Visible = false;
        }
    }
    public void Update()
    {
        CreatePedSwap();
    }
    private void CreatePedSwap()
    {
        PedSwapCustomUIMenu.Clear();
        Distances = new List<DistanceSelect> { new DistanceSelect("Closest", -1f), new DistanceSelect("20 M", 20f), new DistanceSelect("40 M", 40f), new DistanceSelect("100 M", 100f), new DistanceSelect("500 M", 500f), new DistanceSelect("Any", 1000f) };
        TakeoverRandomPed = new UIMenuListItem("Takeover Random Pedestrian", "Takes over a random pedestrian around the player.", Distances);
        BecomeRandomPed = new UIMenuItem("Become Random Pedestrian", "Becomes a random ped model.");





        BecomeRandomCop = new UIMenuItem("Become Random Cop", "Becomes a random cop around the player (Alpha)");
        PedSwapCustomUIMenu.AddItem(TakeoverRandomPed);
        PedSwapCustomUIMenu.AddItem(BecomeRandomPed);
        PedSwapCustomUIMenu.AddItem(BecomeRandomCop);
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
        PedSwapCustomUIMenu.Visible = false;
    }
    private void OnListChange(UIMenu sender, UIMenuListItem list, int index)
    {
        if (list == TakeoverRandomPed)
        {
            SelectedTakeoverRadius = Distances[index].Distance;
        }
    }








    public PedVariation GetPedVariation(Ped myPed)
    {
        try
        {
            PedVariation myPedVariation = new PedVariation
            {
                MyPedComponents = new List<PedComponent>(),
                MyPedProps = new List<PedPropComponent>()
            };
            for (int ComponentNumber = 0; ComponentNumber < 12; ComponentNumber++)
            {
                myPedVariation.MyPedComponents.Add(new PedComponent(ComponentNumber, NativeFunction.Natives.GET_PED_DRAWABLE_VARIATION<int>(myPed, ComponentNumber), NativeFunction.Natives.GET_PED_TEXTURE_VARIATION<int>(myPed, ComponentNumber), NativeFunction.Natives.GET_PED_PALETTE_VARIATION<int>(myPed, ComponentNumber)));
            }
            for (int PropNumber = 0; PropNumber < 8; PropNumber++)
            {
                myPedVariation.MyPedProps.Add(new PedPropComponent(PropNumber, NativeFunction.Natives.GET_PED_PROP_INDEX<int>(myPed, PropNumber), NativeFunction.Natives.GET_PED_PROP_TEXTURE_INDEX<int>(myPed, PropNumber)));
            }
            return myPedVariation;
        }
        catch (Exception e)
        {
            EntryPoint.WriteToConsole("CopyPedComponentVariation! CopyPedComponentVariation Error; " + e.Message, 0);
            return null;
        }
    }



}