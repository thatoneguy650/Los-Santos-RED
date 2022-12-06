using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

public class PedCustomizerMenu
{
    private MenuPool MenuPool;
    private INameProvideable Names;
    private IPedSwap PedSwap;
    private IPedSwappable Player;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private UIMenu CustomizeMainMenu;
    private PedCustomizer PedCustomizer;
    private CustomizeDemographicsMenu CustomizeDemographicsMenu;
    private CustomizeModelMenu CustomizeModelMenu;
    private CustomizeHeadMenu CustomizeHeadMenu;
    private CustomizeComponentsMenu CustomizeComponentsMenu;
    private CustomizePropsMenu CustomizePropsMenu;
    private CustomizeExistingVariationsMenu CustomizeExistingVariationsMenu;
    public PedCustomizerMenu(MenuPool menuPool, IPedSwap pedSwap, INameProvideable names, IPedSwappable player, IEntityProvideable world, ISettingsProvideable settings, PedCustomizer pedCustomizer, IDispatchablePeople dispatchablePeople, IHeads heads)
    {
        PedSwap = pedSwap;
        MenuPool = menuPool;
        Names = names;
        Player = player;
        World = world;
        Settings = settings;
        PedCustomizer = pedCustomizer;
        CustomizeDemographicsMenu = new CustomizeDemographicsMenu(MenuPool, PedSwap, Names, Player, World, Settings, PedCustomizer, this);
        CustomizeModelMenu = new CustomizeModelMenu(MenuPool, PedSwap, Names, Player, World, Settings, PedCustomizer, this);
        CustomizeHeadMenu = new CustomizeHeadMenu(MenuPool, PedSwap, Names, Player, World, Settings, PedCustomizer, this);
        CustomizeComponentsMenu = new CustomizeComponentsMenu(MenuPool, PedSwap, Names, Player, World, Settings, PedCustomizer, this);
        CustomizePropsMenu = new CustomizePropsMenu(MenuPool, PedSwap, Names, Player, World, Settings, PedCustomizer, this);
        CustomizeExistingVariationsMenu = new CustomizeExistingVariationsMenu(MenuPool, PedSwap, Names, Player, World, Settings, PedCustomizer, this, dispatchablePeople, heads);

    }
    public void Setup()
    {
        CustomizeMainMenu = new UIMenu("Customize Ped 2", "Select an Option");
        CustomizeMainMenu.SetBannerType(EntryPoint.LSRedColor);
        MenuPool.Add(CustomizeMainMenu);
        CustomizeDemographicsMenu.Setup(CustomizeMainMenu);
        CustomizeModelMenu.Setup(CustomizeMainMenu);
        CustomizeHeadMenu.Setup(CustomizeMainMenu);
        CustomizeComponentsMenu.Setup(CustomizeMainMenu);
        CustomizePropsMenu.Setup(CustomizeMainMenu);
        CustomizeExistingVariationsMenu.Setup(CustomizeMainMenu);

        UIMenuItem PrintVariation = new UIMenuItem("Print Variation", "Print the variation out to the log");
        PrintVariation.RightBadge = UIMenuItem.BadgeStyle.Clothes;
        PrintVariation.Activated += (sender, e) =>
        {
            PedCustomizer.PrintVariation();
        };
        CustomizeMainMenu.AddItem(PrintVariation);


        UIMenuItem BecomeModel = new UIMenuItem("Become Character", "Return to gameplay as displayed character");
        BecomeModel.RightBadge = UIMenuItem.BadgeStyle.Clothes;
        BecomeModel.Activated += (sender, e) =>
        {
            PedCustomizer.BecomePed();
        };
        CustomizeMainMenu.AddItem(BecomeModel);

        UIMenuItem Exit = new UIMenuItem("Exit", "Return to gameplay as old character");
        Exit.RightBadge = UIMenuItem.BadgeStyle.Alert;
        Exit.Activated += (sender, e) =>
        {
            PedCustomizer.Exit();
        };
        CustomizeMainMenu.AddItem(Exit);
    }
    public void Start()
    {
        OnModelChanged();
        CustomizeMainMenu.Visible = true;
    }
    public void OnModelChanged()
    {
        //Change the components and stuff, reset everything
        CustomizeDemographicsMenu.OnModelChanged();
        CustomizeHeadMenu.OnModelChanged();
        CustomizeComponentsMenu.OnModelChanged();
        CustomizePropsMenu.OnModelChanged();
        EntryPoint.WriteToConsole("PedCustomizerMenu.OnModelChanged Executed");
    }
    //public void OnVariationChanged()
    //{
    //    //CustomizeHeadMenu.OnVariationChanged();
    //}
}