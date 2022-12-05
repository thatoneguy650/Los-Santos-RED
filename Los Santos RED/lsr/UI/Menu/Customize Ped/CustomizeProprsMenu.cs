using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Mod;
using Rage.Native;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.ComponentModel;


public class CustomizePropsMenu
{
    private IPedSwap PedSwap;
    private MenuPool MenuPool;
    private INameProvideable Names;
    private IPedSwappable Player;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private PedCustomizer PedCustomizer;
    private UIMenuItem InputModel;
    private UIMenuListScrollerItem<string> SelectModel;
    private UIMenuItem SearchModel;
    private PedCustomizerMenu PedCustomizerMenu;
    private UIMenu ModelSearchSubMenu;
    private string FilterString;
    private List<FashionProp> PropLookup;
    private UIMenu PickPropMenu;
    private UIMenuItem ClearProps;

    public CustomizePropsMenu(MenuPool menuPool, IPedSwap pedSwap, INameProvideable names, IPedSwappable player, IEntityProvideable world, ISettingsProvideable settings, PedCustomizer pedCustomizer, PedCustomizerMenu pedCustomizerMenu)
    {
        PedSwap = pedSwap;
        MenuPool = menuPool;
        Names = names;
        Player = player;
        World = world;
        Settings = settings;
        PedCustomizer = pedCustomizer;
        PedCustomizerMenu = pedCustomizerMenu;
    }
    public void Setup(UIMenu CustomizeMainMenu)
    {
        PropLookup = new List<FashionProp>() {
            new FashionProp(0,"Hats"),
            new FashionProp(1, "Glasses"),
            new FashionProp(2, "Ear"),};

        PickPropMenu = MenuPool.AddSubMenu(CustomizeMainMenu, "Props");
        CustomizeMainMenu.MenuItems[CustomizeMainMenu.MenuItems.Count() - 1].Description = "Change the props of the current ped";
        CustomizeMainMenu.MenuItems[CustomizeMainMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Clothes;
        PickPropMenu.SetBannerType(EntryPoint.LSRedColor);
    }
    public void OnModelChanged()
    {
        PickPropMenu.Clear();
        AddPropItems();
    }
    private void AddPropItems()
    {
        foreach (FashionProp s in PropLookup)
        {
            s.AddCustomizeMenu(MenuPool, PickPropMenu, PedCustomizer.ModelPed, PedCustomizer);
        }
        //ClearProps = new UIMenuItem("Clear Props", "Remove ALL props from displayed character");
        //ClearProps.RightBadge = UIMenuItem.BadgeStyle.Crown;
        //ClearProps.Activated += (sender, e) =>
        //{
        //    PedCustomizer.WorkingVariation.Props.Clear();
        //    PedCustomizer.OnVariationChanged();
        //};
        //PickPropMenu.AddItem(ClearProps);
    }
}

