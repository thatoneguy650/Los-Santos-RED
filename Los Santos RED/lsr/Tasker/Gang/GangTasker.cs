using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GangTasker
{
    private IEntityProvideable World;
    private ITargetable Player;
    private IWeapons Weapons;
    private ISettingsProvideable Settings;
    private Mod.Tasker Tasker;
    private IPlacesOfInterest PlacesOfInterest;
    public GangTasker(Mod.Tasker tasker, IEntityProvideable pedProvider, ITargetable player, IWeapons weapons, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest)
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
        if (!Settings.SettingsManager.GangSettings.ManageTasking)
        {
            return;
        }
        World.Pedestrians.ExpireSeatAssignments();
        foreach (GangMember gangMember in World.Pedestrians.GangMemberList.Where(x => x.Pedestrian.Exists() && x.HasExistedFor >= 1000))
        {
            try
            {
                gangMember.PedBrain.Update(Player, PlacesOfInterest);
                //OldLoopWas Here
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Setting Civilian Task");
            }
        }   
    }
}


