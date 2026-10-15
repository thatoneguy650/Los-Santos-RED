using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class FightClubsMenu
{
    private MenuPool MenuPool;
    private LocationCamera LocationCamera;
    private UIMenu FightMenu;
    IEntityProvideable World;
    IInteractionable Player;
    private UIMenuItem startFightMenuItem;
    private IFightClubable FightClubable;
    private ITargetable Targetable;
    private FightClub FightClub;
    private IGangs Gangs;
    private ISettingsProvideable Settings;
    private List<string> AllowedGangs;
    List<DispatchablePerson> NonGangFightersGroup;

    public FightClubsMenu(MenuPool menuPool, UIMenu fightSubMenu, IEntityProvideable world, ISettingsProvideable settings, IInteractionable player, IFightClubable fightClubable, 
        ITargetable targetable, FightClub fightClub, IGangs gangs, List<string> allowedGangs, List<DispatchablePerson> nonGangFightersGroup)
    {
        MenuPool = menuPool;
        FightMenu = fightSubMenu;
        World = world;
        Settings = settings;
        Player = player;
        FightClubable = fightClubable;
        Targetable = targetable;
        FightClub = fightClub;
        Gangs = gangs;
        AllowedGangs = allowedGangs;
        NonGangFightersGroup = nonGangFightersGroup;
    }
    public void Setup(LocationCamera locationCamera)
    {
        EntryPoint.WriteToConsole("FightClubsMenu START RAN");
        LocationCamera = locationCamera;
        CreateMenuItems();
    }
    private void CreateMenuItems()
    {
        if (FightClub != null && !FightClub.DisablePlayerFights)
        {
            CreatePlayerFightMenuItems();
        }
        CreatePlayerSpectateFightMenuItems();
    }
    private void CreatePlayerFightMenuItems()
    {
        UIMenu PlayerFightSubMenu = MenuPool.AddSubMenu(FightMenu, "Join a Fight");
        PlayerFightSubMenu.SetBannerType(EntryPoint.LSRedColor);

        UIMenuNumericScrollerItem<int> numberOfFightersScroller = new UIMenuNumericScrollerItem<int>("Number of Opponents", "Pick the number of opponents", 1, 4, 1);
        numberOfFightersScroller.Value = 1;
        PlayerFightSubMenu.AddItem(numberOfFightersScroller);

        UIMenuCheckboxItem meleeWeaponsCheckbox = null;
        UIMenuCheckboxItem sidearmsWeaponsCheckbox = null;
        UIMenuCheckboxItem heavyWeaponsCheckbox = null;
        UIMenuCheckboxItem isGangFightMenuItem = null;
        UIMenuListScrollerItem<Gang> GangToFightScroller = null;
        if (FightClub != null && FightClub.AllowMeleeWeapons)
        {
            meleeWeaponsCheckbox = new UIMenuCheckboxItem("Allow Melee Weapons", false);
            PlayerFightSubMenu.AddItem(meleeWeaponsCheckbox);
        }
        if (FightClub != null && FightClub.AllowSidearms)
        {
            sidearmsWeaponsCheckbox = new UIMenuCheckboxItem("Allow Sidearm Weapons", false);
            PlayerFightSubMenu.AddItem(sidearmsWeaponsCheckbox);
        }
        if (FightClub != null && FightClub.AllowHeavyWeapons)
        {
            heavyWeaponsCheckbox = new UIMenuCheckboxItem("Allow Heavy Weapons", false);
            PlayerFightSubMenu.AddItem(heavyWeaponsCheckbox);
        }
        if (FightClub != null && !FightClub.DisableGangFights)
        {
            isGangFightMenuItem = new UIMenuCheckboxItem("Gang Fight", false);
            PlayerFightSubMenu.AddItem(isGangFightMenuItem);
            List<Gang> toFightGangs = Gangs.GetAllGangs();
            if (AllowedGangs != null && AllowedGangs.Any())
            {
                toFightGangs.RemoveAll(x => AllowedGangs.Contains(x.ID));
            }
            GangToFightScroller = new UIMenuListScrollerItem<Gang>("Selected Gang", "Select a gang to fight", toFightGangs);
            GangToFightScroller.Enabled = false;
            PlayerFightSubMenu.AddItem(GangToFightScroller);

            isGangFightMenuItem.CheckboxEvent += (sender, Checked) =>
            {
                GangToFightScroller.Enabled = isGangFightMenuItem.Checked;
            };
        }
        UIMenuItem startFightMenuItem = new UIMenuItem("Start Fight", "Select to start the current fight");
        startFightMenuItem.Activated += (menu, item) =>
        {
            StartFight(menu, true, numberOfFightersScroller.Value + 1, 
                isGangFightMenuItem != null && GangToFightScroller != null && isGangFightMenuItem.Checked ? GangToFightScroller.SelectedItem : null,
                meleeWeaponsCheckbox != null && meleeWeaponsCheckbox.Checked,
                sidearmsWeaponsCheckbox != null && sidearmsWeaponsCheckbox.Checked,
                heavyWeaponsCheckbox != null && heavyWeaponsCheckbox.Checked);
        };
        PlayerFightSubMenu.AddItem(startFightMenuItem);
    }
    private void CreatePlayerSpectateFightMenuItems()
    {
        UIMenu PlayerSpectateSubMenu = MenuPool.AddSubMenu(FightMenu, "Spectate a Fight");
        PlayerSpectateSubMenu.SetBannerType(EntryPoint.LSRedColor);

        UIMenuNumericScrollerItem<int>  numberOfFightersScrollerSpectate = new UIMenuNumericScrollerItem<int>("Number of Fighters", "Pick the number of fighters to watch", 2, 5, 1);
        numberOfFightersScrollerSpectate.Value = 2;
        PlayerSpectateSubMenu.AddItem(numberOfFightersScrollerSpectate);

        UIMenuCheckboxItem meleeWeaponsCheckbox = null;
        UIMenuCheckboxItem sidearmsWeaponsCheckbox = null;
        UIMenuCheckboxItem heavyWeaponsCheckbox = null;
        UIMenuCheckboxItem isGangFightMenuItem = null;
        UIMenuListScrollerItem<Gang> GangToFightScroller = null;

        if (FightClub != null && FightClub.AllowMeleeWeapons)
        {
            meleeWeaponsCheckbox = new UIMenuCheckboxItem("Allow Melee Weapons", false);
            PlayerSpectateSubMenu.AddItem(meleeWeaponsCheckbox);
        }  
        if (FightClub != null && FightClub.AllowSidearms)
        {
            sidearmsWeaponsCheckbox = new UIMenuCheckboxItem("Allow Sidearm Weapons", false);
            PlayerSpectateSubMenu.AddItem(sidearmsWeaponsCheckbox);
        }   
        if (FightClub != null && FightClub.AllowHeavyWeapons)
        {
            heavyWeaponsCheckbox = new UIMenuCheckboxItem("Allow Heavy Weapons", false);
            PlayerSpectateSubMenu.AddItem(heavyWeaponsCheckbox);
        }
        if (FightClub != null && !FightClub.DisableGangFights)
        {
            isGangFightMenuItem = new UIMenuCheckboxItem("Gang Fight", false);
            PlayerSpectateSubMenu.AddItem(isGangFightMenuItem);
            List<Gang> toFightGangs = Gangs.GetAllGangs();
            if (AllowedGangs != null && AllowedGangs.Any())
            {
                toFightGangs.RemoveAll(x => AllowedGangs.Contains(x.ID));
            }
            GangToFightScroller = new UIMenuListScrollerItem<Gang>("Selected Gang", "Select a gang to watch fighting", toFightGangs);
            GangToFightScroller.Enabled = false;
            PlayerSpectateSubMenu.AddItem(GangToFightScroller);
            isGangFightMenuItem.CheckboxEvent += (sender, Checked) =>
            {
                GangToFightScroller.Enabled = isGangFightMenuItem.Checked;
            };
        }
        UIMenuItem startFightMenuItemSpectator = new UIMenuItem("Watch Fight", "Select to start watching the current fight");
        startFightMenuItemSpectator.Activated += (menu, item) =>
        {
            StartFight(menu, 
                false, 
                numberOfFightersScrollerSpectate.Value,
                isGangFightMenuItem != null && GangToFightScroller != null && isGangFightMenuItem.Checked ? GangToFightScroller.SelectedItem : null, 
                meleeWeaponsCheckbox != null && meleeWeaponsCheckbox.Checked,
                sidearmsWeaponsCheckbox != null && sidearmsWeaponsCheckbox.Checked,
                heavyWeaponsCheckbox != null && heavyWeaponsCheckbox.Checked);
        };
        PlayerSpectateSubMenu.AddItem(startFightMenuItemSpectator);
    }
    public void UpdateMenus()
    {
        while (MenuPool.IsAnyMenuOpen())
        {
            MenuPool.ProcessMenus();
            GameFiber.Yield();
        }
    }
    private void StartFight(UIMenu menu, bool isPlayerFight, int numberOfFightersSelected, Gang gangToFight,bool allowMelee, bool allowSidearms, bool allowHeavyWeapons)
    {
        menu.Visible = false;
        try
        {
            MenuPool.CloseAllMenus();
            LocationCamera?.ReturnToGameplay(true);
            LocationCamera?.StopImmediately(true);
            int TotalFighters = numberOfFightersSelected;// numberOfFightersScroller.Value;
            FightClubFight fightClubFight;
            if (gangToFight != null)
            {
                fightClubFight = new FightClubFight(FightClub.FightClubArena, FightClub, isPlayerFight, TotalFighters, World, Settings, Targetable, FightClubable, gangToFight);
            }
            else
            {
                fightClubFight = new FightClubFight(FightClub.FightClubArena, FightClub, isPlayerFight, TotalFighters, World, Settings, Targetable, FightClubable, NonGangFightersGroup);
            }
            fightClubFight.AllowMeleeWeapons = allowMelee;
            fightClubFight.AllowSidearms = allowSidearms;
            fightClubFight.AllowHeavyWeapons = allowHeavyWeapons;
            fightClubFight.Setup();
            Game.FadeScreenOut(1000, true);
            fightClubFight.BeginFirstFight();
            GameFiber FightClubDebug = GameFiber.StartNew(delegate
            {
                while (!fightClubFight.IsEnded && EntryPoint.ModController.IsRunning)
                {
                    fightClubFight.Update();
                    GameFiber.Yield();
                }
            }, "FightClubDebug");
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
            EntryPoint.ModController.CrashUnload();
        }
    }
}

