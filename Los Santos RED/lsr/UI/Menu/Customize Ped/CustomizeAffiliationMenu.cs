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
    private UIMenuListScrollerItem<Agency> LEMenu;

    private PedCustomizerMenu PedCustomizerMenu;
    private UIMenuItem UnaffiliatedMenu;


    private UIMenu AffiliationSubMenu;
    private UIMenuItem AffiliationSubMenuItem;
    private UIMenuItem CurrentValueMenu;
    private UIMenuListScrollerItem<Agency> EMSMenu;
    private UIMenuListScrollerItem<Agency> FireMenu;
    private UIMenuListScrollerItem<Agency> SecurityMenu;

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
        //AffiliationSubMenuItem.RightBadge = UIMenuItem.BadgeStyle.Lock;
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


        LEMenu = new UIMenuListScrollerItem<Agency>("Set LE", "Join a law enforcement agency. ~r~WIP Most Features TBD~s~", Agencies.GetAgencies().Where(x => x.ResponseType == ResponseType.LawEnforcement));
        LEMenu.Activated += (sender, selectedItem) =>
        {
            PedCustomizer.AssignedGang = null;
            PedCustomizer.AssignedAgency = LEMenu.SelectedItem;
            SetCurrent();
        };
        AffiliationSubMenu.AddItem(LEMenu);

        SecurityMenu = new UIMenuListScrollerItem<Agency>("Set Security", "Join a security agency. ~r~WIP Most Features TBD~s~", Agencies.GetAgencies().Where(x => x.ResponseType == ResponseType.Security));
        SecurityMenu.Activated += (sender, selectedItem) =>
        {
            PedCustomizer.AssignedGang = null;
            PedCustomizer.AssignedAgency = SecurityMenu.SelectedItem;
            SetCurrent();
        };
        AffiliationSubMenu.AddItem(SecurityMenu);


        EMSMenu = new UIMenuListScrollerItem<Agency>("Set EMS", "Join an EMS agency. ~r~WIP Most Features TBD~s~", Agencies.GetAgencies().Where(x => x.ResponseType == ResponseType.EMS));
        EMSMenu.Activated += (sender, selectedItem) =>
        {
            PedCustomizer.AssignedAgency = EMSMenu.SelectedItem;
            SetCurrent();
        };
        AffiliationSubMenu.AddItem(EMSMenu);

        FireMenu = new UIMenuListScrollerItem<Agency>("Set Fire", "Join a fire fighting agency. ~r~WIP Most Features TBD~s~", Agencies.GetAgencies().Where(x => x.ResponseType == ResponseType.Fire));
        FireMenu.Activated += (sender, selectedItem) =>
        {
            PedCustomizer.AssignedAgency = FireMenu.SelectedItem;
            SetCurrent();
        };
        AffiliationSubMenu.AddItem(FireMenu);

        SetCurrent();
    }
    private void SetCurrent()
    {
        if (PedCustomizer.AssignedAgency != null)
        {
            CurrentValueMenu.RightLabel = PedCustomizer.AssignedAgency.ID;
            if(PedCustomizer.AssignedAgency.ResponseType == ResponseType.Fire)
            {
                FireMenu.SelectedItem = FireMenu.Items.Where(x => PedCustomizer.AssignedAgency.ID == x.ID).FirstOrDefault();
                LEMenu.SelectedItem = LEMenu.Items[0];
                EMSMenu.SelectedItem = EMSMenu.Items[0];
                SecurityMenu.SelectedItem = SecurityMenu.Items[0];
            }
            else if (PedCustomizer.AssignedAgency.ResponseType == ResponseType.EMS)
            {
                EMSMenu.SelectedItem = EMSMenu.Items.Where(x => PedCustomizer.AssignedAgency.ID == x.ID).FirstOrDefault();
                FireMenu.SelectedItem = FireMenu.Items[0];
                LEMenu.SelectedItem = LEMenu.Items[0];
                SecurityMenu.SelectedItem = SecurityMenu.Items[0];
            }
            else if (PedCustomizer.AssignedAgency.ResponseType == ResponseType.LawEnforcement)
            {
                LEMenu.SelectedItem = LEMenu.Items.Where(x => PedCustomizer.AssignedAgency.ID == x.ID).FirstOrDefault();
                FireMenu.SelectedItem = FireMenu.Items[0];
                EMSMenu.SelectedItem = EMSMenu.Items[0];
                SecurityMenu.SelectedItem = SecurityMenu.Items[0];
            }
            else if (PedCustomizer.AssignedAgency.ResponseType == ResponseType.Security)
            {
                SecurityMenu.SelectedItem = SecurityMenu.Items.Where(x => PedCustomizer.AssignedAgency.ID == x.ID).FirstOrDefault();
                FireMenu.SelectedItem = FireMenu.Items[0];
                EMSMenu.SelectedItem = EMSMenu.Items[0];
                LEMenu.SelectedItem = LEMenu.Items[0];
            }
        }
        else
        {
            FireMenu.SelectedItem = FireMenu.Items[0];
            LEMenu.SelectedItem = LEMenu.Items[0];
            EMSMenu.SelectedItem = EMSMenu.Items[0];
            SecurityMenu.SelectedItem = SecurityMenu.Items[0];
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

