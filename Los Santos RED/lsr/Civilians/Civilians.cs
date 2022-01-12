using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Civilians
{
    private IEntityProvideable World;
    private IPoliceRespondable PoliceRespondable;
    private IPerceptable Perceptable;
    private ISettingsProvideable Settings;
    private uint GameTimeLastUpdatedPeds;

    public Civilians(IEntityProvideable world, IPoliceRespondable policeRespondable, IPerceptable perceptable, ISettingsProvideable settings)
    {
        World = world;
        PoliceRespondable = policeRespondable;
        Perceptable = perceptable;
        Settings = settings;
    }
    public int PersistentCount
    {
        get
        {
            return World.CivilianList.Count(x => x.Pedestrian.IsPersistent);
        }
    }
    public void ResetWitnessedCrimes()
    {
        World.CivilianList.ForEach(x => x.PlayerCrimesWitnessed.Clear());
    }
    public void Update()
    {
        foreach (PedExt ped in World.CivilianList.OrderBy(x => x.GameTimeLastUpdated))
        {
            try
            {
                ped.Update(Perceptable, PoliceRespondable, Vector3.Zero, World);
                GameFiber.Yield();
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Updating Civilian Data");
            }
        }
        foreach (PedExt ped in World.MerchantList.OrderBy(x => x.GameTimeLastUpdated))
        {
            try
            {
                ped.Update(Perceptable, PoliceRespondable, Vector3.Zero, World);
                GameFiber.Yield();
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Updating Merchant Data");
            }
        }
        foreach (PedExt ped in World.ZombieList.OrderBy(x => x.GameTimeLastUpdated))
        {
            try
            {
                ped.Update(Perceptable, PoliceRespondable, Vector3.Zero, World);
                GameFiber.Yield();
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Updating Zombie Data");
            }
        }
        foreach (GangMember ped in World.GangMemberList.OrderBy(x => x.GameTimeLastUpdated))
        {
            try
            {
                ped.Update(Perceptable, PoliceRespondable, Vector3.Zero, World);
                GameFiber.Yield();
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Updating GangMember Data");
            }
        }

        bool anyGangMemberCanSeePlayer = false;
        bool anyGangMemberCanHearPlayer = false;
        bool anyGangMemberRecentlySeenPlayer = false;
        foreach (GangMember gangBanger in World.GangMemberList)
        {
            if (gangBanger.Pedestrian.Exists() && gangBanger.Pedestrian.IsAlive)
            {
                if (gangBanger.CanSeePlayer)
                {
                    anyGangMemberCanSeePlayer = true;
                    anyGangMemberCanHearPlayer = true;
                    anyGangMemberRecentlySeenPlayer = true;
                }
                else if (gangBanger.WithinWeaponsAudioRange)
                {
                    anyGangMemberCanHearPlayer = true;
                }
                if (gangBanger.SeenPlayerWithin(Settings.SettingsManager.PoliceSettings.RecentlySeenTime))
                {
                    anyGangMemberRecentlySeenPlayer = true;
                }
            }
            if (anyGangMemberCanSeePlayer)
            {
                break;
            }
            GameFiber.Yield();
        }

        Perceptable.AnyGangMemberCanSeePlayer = anyGangMemberCanSeePlayer;
        Perceptable.AnyGangMemberCanHearPlayer = anyGangMemberCanHearPlayer;
        Perceptable.AnyGangMemberRecentlySeenPlayer = anyGangMemberRecentlySeenPlayer;






        PedExt worstPed = World.CivilianList.OrderByDescending(x => x.WantedLevel).FirstOrDefault();
        if (worstPed != null && worstPed.WantedLevel > PoliceRespondable.WantedLevel)
        {
            World.TotalWantedLevel = worstPed.WantedLevel;
        }
        else
        {
            World.TotalWantedLevel = PoliceRespondable.WantedLevel;
        }
        if (Settings.SettingsManager.DebugSettings.PrintUpdateTimes)
        {
            EntryPoint.WriteToConsole($"Civilians.Update Ran Time Since {Game.GameTime - GameTimeLastUpdatedPeds}", 5);
        }
        GameTimeLastUpdatedPeds = Game.GameTime;
    }
}
