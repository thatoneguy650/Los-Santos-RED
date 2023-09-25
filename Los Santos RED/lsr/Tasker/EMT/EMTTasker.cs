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


public class EMTTasker
{
    private IEntityProvideable World;
    private ITargetable Player;
    private IWeapons Weapons;
    private ISettingsProvideable Settings;
    private Mod.Tasker Tasker;
    private IPlacesOfInterest PlacesOfInterest;
    private List<PedExt> PossibleTargets;
    public EMTTasker(Mod.Tasker tasker, IEntityProvideable world, ITargetable player, IWeapons weapons, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest)
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
        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@enter");
        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@base");
        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@exit");
        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@idle_a");
    }
    public void Update()
    {
        if (!Settings.SettingsManager.EMSSettings.ManageTasking)
        {
            return;
        }
        World.Pedestrians.ExpireSeatAssignments();
        foreach (EMT emt in World.Pedestrians.EMTList.Where(x => x.Pedestrian.Exists() && x.HasExistedFor >= 1000))
        {
            try
            {
                emt.PedBrain.Update(Player, PlacesOfInterest);
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Setting EMT Task");
            }
        }
        GameFiber.Yield();
    }
}


