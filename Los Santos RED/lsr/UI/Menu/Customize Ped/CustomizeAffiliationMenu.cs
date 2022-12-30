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


public class CustomizeAffiliationMenu
{
    private IPedSwap PedSwap;
    private MenuPool MenuPool;
    private INameProvideable Names;
    private IPedSwappable Player;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private IGangs Gangs;
    private IAgencies Agencies;
    private PedCustomizer PedCustomizer;
    private UIMenuListScrollerItem<Gang> GangsMenu;
    private UIMenuListScrollerItem<Agency> AgenciesMenu;

    private PedCustomizerMenu PedCustomizerMenu;
    private UIMenuItem UnaffiliatedMenu;


    private UIMenu AffiliationSubMenu;
    private UIMenuItem AffiliationSubMenuItem;
    private UIMenuItem CurrentValueMenu;

    public CustomizeAffiliationMenu(MenuPool menuPool, IPedSwap pedSwap, INameProvideable names, IPedSwappable player, IEntityProvideable world, ISettingsProvideable settings, PedCustomizer pedCustomizer, PedCustomizerMenu pedCustomizerMenu, IGangs gangs, IAgencies agencies)
    {
        PedSwap = pedSwap;
        MenuPool = menuPool;
        Names = names;
        Player = player;
        World = world;
        Settings = settings;
        PedCustomizer = pedCustomizer;
        PedCustomizerMenu = pedCustomizerMenu;
        Gangs = gangs;
        Agencies = agencies;
    }
    public void Setup(UIMenu CustomizeMainMenu)
    {
        AffiliationSubMenu = MenuPool.AddSubMenu(CustomizeMainMenu, "Affiliation");
        AffiliationSubMenuItem = CustomizeMainMenu.MenuItems[CustomizeMainMenu.MenuItems.Count() - 1];
        AffiliationSubMenuItem.Description = "Change affiliation for the current ped";
        AffiliationSubMenuItem.RightBadge = UIMenuItem.BadgeStyle.Lock;
        AffiliationSubMenu.SetBannerType(EntryPoint.LSRedColor);
        AffiliationSubMenu.InstructionalButtonsEnabled = false;

        AffiliationSubMenu.OnMenuOpen += (sender) =>
        {
            PedCustomizer.CameraCycler.SetDefault();
        };
        AffiliationSubMenu.OnMenuClose += (sender) =>
        {
            PedCustomizer.CameraCycler.SetDefault();
        };


        CurrentValueMenu = new UIMenuItem("Status", "Your current affiliation");
        CurrentValueMenu.Enabled = false;
        AffiliationSubMenu.AddItem(CurrentValueMenu);


        UnaffiliatedMenu = new UIMenuItem("Set Unaffiliated", "Be unaffiliated");
        UnaffiliatedMenu.Activated += (sender, selectedItem) =>
        {
            PedCustomizer.AssignedGang = null;
            PedCustomizer.AssignedAgency = null;
            SetCurrent();
        };
        AffiliationSubMenu.AddItem(UnaffiliatedMenu);

        GangsMenu = new UIMenuListScrollerItem<Gang>("Set Gang", "Join a criminal gang", Gangs.AllGangs);
        GangsMenu.Activated += (sender, selectedItem) =>
         {
             PedCustomizer.AssignedGang = GangsMenu.SelectedItem;
             PedCustomizer.AssignedAgency = null;
             SetCurrent();
         };   
        AffiliationSubMenu.AddItem(GangsMenu);


        AgenciesMenu = new UIMenuListScrollerItem<Agency>("Set Agency", "Join a law enforcement agency", Agencies.GetAgencies().Where(x => x.ResponseType == ResponseType.LawEnforcement));
        AgenciesMenu.Activated += (sender, selectedItem) =>
        {
            PedCustomizer.AssignedGang = null;
            PedCustomizer.AssignedAgency = AgenciesMenu.SelectedItem;
            SetCurrent();
        };
        AffiliationSubMenu.AddItem(AgenciesMenu);
        SetCurrent();
    }
    private void SetCurrent()
    {
        if (PedCustomizer.AssignedAgency != null)
        {
            CurrentValueMenu.RightLabel = PedCustomizer.AssignedAgency.ID;
            AgenciesMenu.SelectedItem = AgenciesMenu.Items.Where(x => PedCustomizer.AssignedAgency.ID == x.ID).FirstOrDefault();
        }
        else
        {
            AgenciesMenu.SelectedItem = AgenciesMenu.Items[0];
        }
        if (PedCustomizer.AssignedGang != null)
        {
            CurrentValueMenu.RightLabel = PedCustomizer.AssignedGang.ShortName;
            GangsMenu.SelectedItem = GangsMenu.Items.Where(x => PedCustomizer.AssignedGang.ID == x.ID).FirstOrDefault();
        }
        else
        {
            GangsMenu.SelectedItem = GangsMenu.Items[0];
        }
        if(PedCustomizer.AssignedGang == null && PedCustomizer.AssignedAgency == null)
        {
            CurrentValueMenu.RightLabel = "";
        }
        

    }

    public void OnModelChanged()
    {
        SetCurrent();
    }
}

