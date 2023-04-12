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


public class CustomizeComponentsMenu
{
    private IPedSwap PedSwap;
    private MenuPool MenuPool;
    private INameProvideable Names;
    private IPedSwappable Player;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private PedCustomizer PedCustomizer;
    private PedCustomizerMenu PedCustomizerMenu;
    private UIMenu PickComponentMenu;
    private List<FashionComponent> ComponentLookup;
    public CustomizeComponentsMenu(MenuPool menuPool, IPedSwap pedSwap, INameProvideable names, IPedSwappable player, IEntityProvideable world, ISettingsProvideable settings, PedCustomizer pedCustomizer, PedCustomizerMenu pedCustomizerMenu)
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
        ComponentLookup = new List<FashionComponent>() {
            new FashionComponent(0,"Face"),
            new FashionComponent(1, "Mask/Beard"),
            new FashionComponent(2, "Hair"),
            new FashionComponent(3, "Torso"),
            new FashionComponent(4, "Lower"),
            new FashionComponent(5, "Bags"),
            new FashionComponent(6, "Foot"),
            new FashionComponent(7, "Accessories"),
            new FashionComponent(8, "Undershirt"),
            new FashionComponent(9, "Body Armor"),
            new FashionComponent(10, "Decals"),
            new FashionComponent(11, "Tops"), };

        PickComponentMenu = MenuPool.AddSubMenu(CustomizeMainMenu, "Components");
        CustomizeMainMenu.MenuItems[CustomizeMainMenu.MenuItems.Count() - 1].Description = "Change the components of the current ped";
       // CustomizeMainMenu.MenuItems[CustomizeMainMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Clothes;
        PickComponentMenu.SetBannerType(EntryPoint.LSRedColor);
        PickComponentMenu.InstructionalButtonsEnabled = false;

        PickComponentMenu.OnMenuOpen += (sender) =>
        {
            PedCustomizer.CameraCycler.SetDefault();
        };
        PickComponentMenu.OnMenuClose += (sender) =>
        {
            PedCustomizer.CameraCycler.SetDefault();
        };


    }
    public void OnModelChanged()
    {
        PickComponentMenu.Clear();
        AddComponentItems();
        //EntryPoint.WriteToConsoleTestLong("CustomizeComponentsMenu.OnModelChanged Executed");
    }
    private void AddComponentItems()
    {
        foreach (FashionComponent s in ComponentLookup)
        {
            s.AddCustomizeMenu(MenuPool, PickComponentMenu, PedCustomizer.ModelPed, PedCustomizer);
        }
    }
}

