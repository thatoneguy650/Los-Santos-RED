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


public class CustomizeOverlaysMenu
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
    private List<OverlayZoneComponent> OverlayZoneComponentLookup;
    public CustomizeOverlaysMenu(MenuPool menuPool, IPedSwap pedSwap, INameProvideable names, IPedSwappable player, IEntityProvideable world, ISettingsProvideable settings, PedCustomizer pedCustomizer, PedCustomizerMenu pedCustomizerMenu)
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
        OverlayZoneComponentLookup = new List<OverlayZoneComponent>() {
            new OverlayZoneComponent(0,"ZONE_HEAD","Head"),
            new OverlayZoneComponent(1, "ZONE_TORSO","Torso"),
            new OverlayZoneComponent(2, "ZONE_RIGHT_ARM","Right Arm"),
            new OverlayZoneComponent(3, "ZONE_LEFT_ARM","Left Arm"),
            new OverlayZoneComponent(4, "ZONE_LEFT_LEG","Left Leg"),
            new OverlayZoneComponent(5, "ZONE_RIGHT_LEG","Right Leg"),
         };
        PickComponentMenu = MenuPool.AddSubMenu(CustomizeMainMenu, "Body Overlays");
        CustomizeMainMenu.MenuItems[CustomizeMainMenu.MenuItems.Count() - 1].Description = "Change the body overlays of the current ped. These include tattoos and clothing logos.";
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
        //EntryPoint.WriteToConsoleTestLong("CustomizeOverlaysMenu.OnModelChanged Executed");
    }
    private void AddComponentItems()
    {
        foreach (OverlayZoneComponent s in OverlayZoneComponentLookup)
        {
            s.AddCustomizeMenu(MenuPool, PickComponentMenu, PedCustomizer.ModelPed, PedCustomizer);
        }
    }
}

