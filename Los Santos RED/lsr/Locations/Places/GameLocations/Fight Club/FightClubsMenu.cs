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
    private UIMenu FightMenu;
    IEntityProvideable World;
    IInteractionable Player;
    private UIMenuListScrollerItem<Gang> GangToFightScroller;
    private UIMenuItem startFightMenuItem;
    private IFightClubable FightClubable;
    private ITargetable Targetable;
    private FightClub FightClub;
    private UIMenuCheckboxItem isPlayerFightMenuItem;
    private UIMenuCheckboxItem isGangFightMenuItem;
    private IGangs Gangs;
    private UIMenuNumericScrollerItem<int> numberOfFightersScroller;

    public FightClubsMenu(MenuPool menuPool, UIMenu fightSubMenu, IEntityProvideable world, IInteractionable player, IFightClubable fightClubable, ITargetable targetable, FightClub fightClub, IGangs gangs)
    {
        MenuPool = menuPool;
        FightMenu = fightSubMenu;
        World = world;
        Player = player;
        FightClubable = fightClubable;
        Targetable = targetable;
        FightClub = fightClub;
        Gangs = gangs;
    }

    public void Setup()
    {
        EntryPoint.WriteToConsole("FightClubsMenu START RAN");


        //AddTrackSubMenu();
        //AddPlayerVehicleMenu();
        //AddBettingSubMenu();
        // AddOpponentsSubMenu();
        //AddSettingsSubMenu();




        //Determine if player is fighting
        //determine the arena to use if more than one
        //determine the amount of fighters
        //determine gang or civilian fight

        numberOfFightersScroller = new UIMenuNumericScrollerItem<int>("Number of Fighters", "Pick the number of fighters, includes player if selected to fight", 2, 5, 1);
        FightMenu.AddItem(numberOfFightersScroller);

        isPlayerFightMenuItem = new UIMenuCheckboxItem("Player Fight",false);

        FightMenu.AddItem(isPlayerFightMenuItem);


        isGangFightMenuItem = new UIMenuCheckboxItem("Gang Fight", false);

        FightMenu.AddItem(isGangFightMenuItem);

        GangToFightScroller = new UIMenuListScrollerItem<Gang>("Selected Gang","Select a gang to fight",Gangs.GetAllGangs());
        FightMenu.AddItem(GangToFightScroller);

        startFightMenuItem = new UIMenuItem("Start Fight", "Select to start the current fight");
        startFightMenuItem.Activated += (menu, item) =>
        {
            if (!AttemptStartFight(menu))
            {
                Game.DisplayHelp("Error starting fight");
            }
        };
        FightMenu.AddItem(startFightMenuItem);

        //UpdateStartRaceDescription();
    }

    private bool AttemptStartFight(UIMenu menu)
    {

        menu.Visible = false;
        GameFiber.StartNew(delegate
        {
            try
            {
                Game.FadeScreenOut(1000, true);
                MenuPool.CloseAllMenus();
                while (Player.ActivityManager.IsInteractingWithLocation)
                {
                    GameFiber.Yield();
                }
                int TotalFighters = numberOfFightersScroller.Value;
                FightClubFight fightClubFight;
                if(isGangFightMenuItem.Checked)
                {
                    fightClubFight = new FightClubFight(FightClub.FightClubArena, FightClub, isPlayerFightMenuItem.Checked, TotalFighters, World, Targetable, FightClubable, GangToFightScroller.SelectedItem);
                }
                else
                {
                    fightClubFight = new FightClubFight(FightClub.FightClubArena, FightClub, isPlayerFightMenuItem.Checked, TotalFighters, World, Targetable, FightClubable);
                }
                fightClubFight.Setup();
                Game.FadeScreenIn(1000, true);
                fightClubFight.Begin();
                GameFiber FightClubDebug = GameFiber.StartNew(delegate
                {
                    while (!fightClubFight.IsEnded || EntryPoint.ModController.IsRunning)
                    {
                        fightClubFight.Update();
                        GameFiber.Yield();
                    }
                    //fightClubFight.Dispose();
                }, "FightClubDebug");
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "RaceMeetupInteract");

        return true;
    }
}

