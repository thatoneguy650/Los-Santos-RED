using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MenuManager
{
    private MenuPool menuPool;
    private DeathMenu DeathMenu;
    private BustedMenu BustedMenu;
    private MainMenu MainMenu;
    private DebugMenu DebugMenu;
    public MenuManager(IPedswappable pedSwap, IPlacesOfInterest placesOfInterest, IRespawning respawning, IActionable player, IWeapons weapons, RadioStations radioStations)
    {
        menuPool = new MenuPool();
        DeathMenu = new DeathMenu(menuPool, pedSwap, respawning, placesOfInterest);
        BustedMenu = new BustedMenu(menuPool, pedSwap, respawning, placesOfInterest);
        MainMenu = new MainMenu(menuPool, pedSwap, player);
        DebugMenu = new DebugMenu(menuPool, player, weapons, radioStations);
    }
    public void Update(bool IsDead, bool IsBusted)
    {
        if (Game.IsKeyDown(System.Windows.Forms.Keys.F10))
        {
            if (IsDead)
            {
                MainMenu.Hide();
                BustedMenu.Hide();
                DebugMenu.Hide();
                DeathMenu.Toggle();
            }
            else if (IsBusted)
            {
                MainMenu.Hide();
                DeathMenu.Hide();
                DebugMenu.Hide();
                BustedMenu.Toggle();
            }
            else
            {
                DeathMenu.Hide();
                BustedMenu.Hide();
                DebugMenu.Hide();
                MainMenu.Toggle();
            }
        }
        else if (Game.IsKeyDown(System.Windows.Forms.Keys.F11))
        {
            DeathMenu.Hide();
            BustedMenu.Hide();
            MainMenu.Hide();
            DebugMenu.Toggle();
        }
        menuPool.ProcessMenus();
    }

}

