using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

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

    public PedCustomizerMenu(MenuPool menuPool, IPedSwap pedSwap, INameProvideable names, IPedSwappable player, IEntityProvideable world, ISettingsProvideable settings, PedCustomizer pedCustomizer)
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
    }
    public void Setup()
    {
        CustomizeMainMenu = new UIMenu("Customize Ped 2", "Select an Option");
        CustomizeMainMenu.SetBannerType(EntryPoint.LSRedColor);
        MenuPool.Add(CustomizeMainMenu);
        CustomizeDemographicsMenu.Setup(CustomizeMainMenu);
        CustomizeModelMenu.Setup(CustomizeMainMenu);
        CustomizeHeadMenu.Setup(CustomizeMainMenu);
    }
    public void Start()
    {
        OnVariationChanged();
        CustomizeMainMenu.Visible = true;
    }
    public void OnModelChanged()
    {
        //Change the components and stuff, reset everything
        CustomizeHeadMenu.OnVariationChanged();
    }
    public void OnVariationChanged()
    {
        CustomizeHeadMenu.OnVariationChanged();
    }
}