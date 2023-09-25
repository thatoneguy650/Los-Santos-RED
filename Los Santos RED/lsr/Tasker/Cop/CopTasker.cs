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


public class CopTasker
{
    private IEntityProvideable World;
    private ITargetable Player;
    private IWeapons Weapons;
    private ISettingsProvideable Settings;
    private Mod.Tasker Tasker;
    private IPlacesOfInterest PlacesOfInterest;
    public CopTasker(Mod.Tasker tasker, IEntityProvideable pedProvider, ITargetable player, IWeapons weapons, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest)
    {
        Tasker = tasker;
        World = pedProvider;
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
        if (!Settings.SettingsManager.PoliceTaskSettings.ManageTasking)
        {
            return;
        }
        World.Pedestrians.SetPossiblePoliceTargets();
        World.Pedestrians.ExpireSeatAssignments();
        GameFiber.Yield();//TR 29
        foreach (Cop cop in World.Pedestrians.AllPoliceList)
        {
            try
            {
                cop.PedBrain.Update(Player,PlacesOfInterest);
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Setting Civilian Task");
            }
        }     
    }
}


