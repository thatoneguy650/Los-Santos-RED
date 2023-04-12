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
    private PedCustomizerMenu PedCustomizerMenu;
    private List<FashionProp> PropLookup;
    private UIMenu PickPropMenu;
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
            new FashionProp(2, "Ear"),
            new FashionProp(6, "Watches"),
            new FashionProp(7, "Bracelets"),
        };

        PickPropMenu = MenuPool.AddSubMenu(CustomizeMainMenu, "Props");
        CustomizeMainMenu.MenuItems[CustomizeMainMenu.MenuItems.Count() - 1].Description = "Change the props of the current ped";
        //CustomizeMainMenu.MenuItems[CustomizeMainMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Clothes;
        PickPropMenu.SetBannerType(EntryPoint.LSRedColor);
        PickPropMenu.InstructionalButtonsEnabled = false;
        PickPropMenu.OnMenuOpen += (sender) =>
        {
            PedCustomizer.CameraCycler.SetDefault();
        };
        PickPropMenu.OnMenuClose += (sender) =>
        {
            PedCustomizer.CameraCycler.SetDefault();
        };

    }
    public void OnModelChanged()
    {
        PickPropMenu.Clear();
        AddPropItems();
        //EntryPoint.WriteToConsoleTestLong("CustomizePropsMenu.OnModelChanged Executed");
    }
    private void AddPropItems()
    {
        foreach (FashionProp s in PropLookup)
        {
            s.AddCustomizeMenu(MenuPool, PickPropMenu, PedCustomizer.ModelPed, PedCustomizer);
        }
    }
}

