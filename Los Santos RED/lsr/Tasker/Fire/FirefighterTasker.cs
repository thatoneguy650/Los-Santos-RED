using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


public class FirefighterTasker
{
    private IEntityProvideable World;
    private ITargetable Player;
    private IWeapons Weapons;
    private ISettingsProvideable Settings;
    private Mod.Tasker Tasker;
    private IPlacesOfInterest PlacesOfInterest;
    public FirefighterTasker(Mod.Tasker tasker, IEntityProvideable world, ITargetable player, IWeapons weapons, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest)
    {
        Tasker = tasker;
        World = world;
        Player = player;
        Weapons = weapons;
        Settings = settings;
        PlacesOfInterest = placesOfInterest;
    }

    public void Setup()
    {

    }
    public void Update()
    {
        if (!Settings.SettingsManager.FireSettings.ManageTasking)
        {
            return;
        }
        World.Pedestrians.ExpireSeatAssignments();
        foreach (Firefighter firefighter in World.Pedestrians.FirefighterList.Where(x => x.Pedestrian.Exists() && x.HasExistedFor >= 1000))
        {
            try
            {
                firefighter.PedBrain.Update(Player, PlacesOfInterest);
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Setting Firefighter Task");
            }
        }
        GameFiber.Yield();   
    }
}