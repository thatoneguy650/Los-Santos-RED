using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CustomizeHeadMenu
{
    private IPedSwap PedSwap;
    private MenuPool MenuPool;
    private INameProvideable Names;
    private IPedSwappable Player;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private PedCustomizer PedCustomizer;
    private PedCustomizerMenu PedCustomizerMenu;

    private UIMenu HeadSubMenu;
    private UIMenuItem HeadSubMenuItem;
    private UIMenu CustomizeMainMenu;
    private CustomizeAncestryMenu CustomizeAncestryMenu;
    private CustomizeHairMenu CustomizeHairMenu;
    private CustomizeFaceMenu CustomizeFaceMenu;
    private CustomizeFaceMorphMenu CustomizeFaceMorphMenu;

    public CustomizeHeadMenu(MenuPool menuPool, IPedSwap pedSwap, INameProvideable names, IPedSwappable player, IEntityProvideable world, ISettingsProvideable settings, PedCustomizer pedCustomizer, PedCustomizerMenu pedCustomizerMenu)
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
    public void Setup(UIMenu customizeMainMenu)
    {
        CustomizeMainMenu = customizeMainMenu;
        HeadSubMenu = MenuPool.AddSubMenu(CustomizeMainMenu, "Head");
        HeadSubMenuItem = CustomizeMainMenu.MenuItems[CustomizeMainMenu.MenuItems.Count() - 1];

        HeadSubMenuItem.Description = "Change the head features of the current ped. Only available for the freemode peds. Other peds change head items through the components menu.";
        //HeadSubMenuItem.RightBadge = UIMenuItem.BadgeStyle.Makeup;
        HeadSubMenu.SetBannerType(EntryPoint.LSRedColor);
        HeadSubMenu.InstructionalButtonsEnabled = false;
        HeadSubMenu.OnMenuOpen += (sender) =>
        {
            PedCustomizer.CameraCycler.Set("Face");
        };
        HeadSubMenu.OnMenuClose += (sender) =>
        {
            PedCustomizer.CameraCycler.SetDefault();
        };



        SetHeadEnabledStatus();

        CustomizeAncestryMenu = new CustomizeAncestryMenu(MenuPool, PedSwap, Names, Player, World, Settings, PedCustomizer, PedCustomizerMenu);
        CustomizeAncestryMenu.Create(HeadSubMenu);
        CustomizeAncestryMenu.Setup();


        CustomizeHairMenu = new CustomizeHairMenu(MenuPool, PedSwap, Names, Player, World, Settings, PedCustomizer, PedCustomizerMenu);
        CustomizeHairMenu.Create(HeadSubMenu);
        CustomizeHairMenu.Setup();

        CustomizeFaceMenu = new CustomizeFaceMenu(MenuPool, PedSwap, Names, Player, World, Settings, PedCustomizer, PedCustomizerMenu);
        CustomizeFaceMenu.Create(HeadSubMenu);
        CustomizeFaceMenu.Setup();


        CustomizeFaceMorphMenu = new CustomizeFaceMorphMenu(MenuPool, PedSwap, Names, Player, World, Settings, PedCustomizer, PedCustomizerMenu);
        CustomizeFaceMorphMenu.Create(HeadSubMenu);
        CustomizeFaceMorphMenu.Setup();
    }



    private void SetHeadEnabledStatus()
    {
        if (PedCustomizer.ModelPed.Exists() && PedCustomizer.PedModelIsFreeMode)
        {
            HeadSubMenuItem.Enabled = true;
        }
        else
        {
            HeadSubMenuItem.Enabled = false;
        }
    }
    public void OnModelChanged()
    {
        SetHeadEnabledStatus();
        CustomizeHairMenu.Setup();
        CustomizeAncestryMenu.Setup();
        CustomizeFaceMenu.Setup();
        CustomizeFaceMorphMenu.Setup();
        //EntryPoint.WriteToConsoleTestLong("CustomizeHeadMenu.OnModelChanged Executed");
    }

}

