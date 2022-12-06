using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Mod;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CustomizeExistingVariationsMenu
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
    private IDispatchablePeople DispatchablePeople;
    private IHeads Heads;
    public CustomizeExistingVariationsMenu(MenuPool menuPool, IPedSwap pedSwap, INameProvideable names, IPedSwappable player, IEntityProvideable world, ISettingsProvideable settings, PedCustomizer pedCustomizer, PedCustomizerMenu pedCustomizerMenu, IDispatchablePeople dispatchablePeople, IHeads heads)
    {
        PedSwap = pedSwap;
        MenuPool = menuPool;
        Names = names;
        Player = player;
        World = world;
        Settings = settings;
        PedCustomizer = pedCustomizer;
        PedCustomizerMenu = pedCustomizerMenu;
        DispatchablePeople = dispatchablePeople;
        Heads = heads;
    }
    public void Setup(UIMenu CustomizeMainMenu)
    {
        UIMenu ModelSubMenu = MenuPool.AddSubMenu(CustomizeMainMenu, "Saved Variations");
        CustomizeMainMenu.MenuItems[CustomizeMainMenu.MenuItems.Count() - 1].Description = "Choose from a list of saved variations";
        CustomizeMainMenu.MenuItems[CustomizeMainMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Clothes;
        ModelSubMenu.SetBannerType(EntryPoint.LSRedColor);

        foreach (DispatchablePersonGroup dpg in DispatchablePeople.AllPeople)
        {
            UIMenu dpgSubMenu = MenuPool.AddSubMenu(ModelSubMenu, dpg.DispatchablePersonGroupID);
            dpgSubMenu.SetBannerType(EntryPoint.LSRedColor);
            foreach (DispatchablePerson dp in dpg.DispatchablePeople)
            {
                UIMenuItem uIMenuItem = new UIMenuItem(dp.DebugName + " - " + dp.ModelName, dp.DebugName);
                uIMenuItem.Activated += (sender, e) =>
                {
                    PedCustomizer.WorkingModelName = dp.ModelName;
                    if(dp.RequiredVariation == null)
                    {
                        PedCustomizer.WorkingVariation = new PedVariation();
                    }
                    else
                    {
                        PedCustomizer.WorkingVariation = dp.RequiredVariation.Copy();
                    }
                    PedCustomizer.OnModelChanged(false);
                };
                dpgSubMenu.AddItem(uIMenuItem);
            }
        }
    }

}

