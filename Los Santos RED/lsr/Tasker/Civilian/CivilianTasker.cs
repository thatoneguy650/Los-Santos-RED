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


public class CivilianTasker
{
    private IEntityProvideable PedProvider;
    private ITargetable Player;
    private IWeapons Weapons;
    private ISettingsProvideable Settings;
    private Mod.Tasker Tasker;
    private IPlacesOfInterest PlacesOfInterest;
    public CivilianTasker(Mod.Tasker tasker, IEntityProvideable pedProvider, ITargetable player, IWeapons weapons, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest)
    {
        Tasker = tasker;
        PedProvider = pedProvider;
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
        if (Settings.SettingsManager.CivilianSettings.ManageCivilianTasking)
        {
            PedProvider.Pedestrians.ExpireSeatAssignments();
            foreach (PedExt civilian in PedProvider.Pedestrians.CivilianList.Where(x => x.Pedestrian.Exists()))
            {
                try
                {
                    civilian.PedBrain.Update(Player, PlacesOfInterest);
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Setting Civilian Task");
                }
            }
            foreach (Merchant merchant in PedProvider.Pedestrians.MerchantList.Where(x => x.Pedestrian.Exists()))
            {
                try
                {
                    merchant.PedBrain.Update(Player, PlacesOfInterest);          
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Setting Civilian Task");
                }
            }
            foreach (SecurityGuard securityGuard in PedProvider.Pedestrians.SecurityGuardList.Where(x => x.Pedestrian.Exists()))
            {
                try
                {
                    securityGuard.PedBrain.Update(Player, PlacesOfInterest);
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Setting Civilian Task");
                }
            }
        }
    }
}