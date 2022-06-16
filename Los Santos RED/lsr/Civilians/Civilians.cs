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
    private int TotalRan;
    private int TotalChecked;

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
            return World.Pedestrians.CivilianList.Count(x => x.Pedestrian.IsPersistent);
        }
    }
    public void ResetWitnessedCrimes()
    {
        World.Pedestrians.CivilianList.ForEach(x => x.PlayerCrimesWitnessed.Clear());
    }
    public void Update()
    {
        TotalRan = 0;
        TotalChecked = 0;

        UpdateCivilians();
        UpdateMerchants();
        UpdateZombies();
        UpdateGangMembers();
        UpdateEMTs();

        PedExt worstPed = World.Pedestrians.Citizens.Where(x=>!x.IsBusted && !x.IsArrested).OrderByDescending(x => x.WantedLevel).FirstOrDefault();
        if (worstPed != null && worstPed.WantedLevel > PoliceRespondable.WantedLevel)
        {
            World.TotalWantedLevel = worstPed.WantedLevel;
        }
        else
        {
            World.TotalWantedLevel = PoliceRespondable.WantedLevel;
        }

        if(World.TotalWantedLevel > 0 && PoliceRespondable.IsNotWanted && !PoliceRespondable.Investigation.IsActive)
        {
            if (worstPed.Pedestrian.Exists())
            {
                PoliceRespondable.Investigation.Start(worstPed.Pedestrian.Position, false, true, false, false);
            }
        }

        if (Settings.SettingsManager.DebugSettings.PrintUpdateTimes)
        {
            EntryPoint.WriteToConsole($"Civilians.Update Ran Time Since {Game.GameTime - GameTimeLastUpdatedPeds} TotalRan: {TotalRan} TotalChecked: {TotalChecked}", 5);
        }
        GameTimeLastUpdatedPeds = Game.GameTime;
    }
    private void UpdateCivilians()
    {
        int localRan = 0;
        foreach (PedExt ped in World.Pedestrians.CivilianList.OrderBy(x => x.GameTimeLastUpdated))
        {
            try
            {
                bool yield = false;
                if (ped.NeedsFullUpdate)
                {
                    yield = true;
                    TotalRan++;
                    localRan++;
                }
                ped.Update(Perceptable, PoliceRespondable, Vector3.Zero, World);
                if (!ped.WasEverSetPersistent && ped.Pedestrian.Exists() && ped.Pedestrian.IsPersistent)
                {
                    ped.CanBeAmbientTasked = false;
                    ped.WillCallPolice = false;
                    ped.WillCallPoliceIntense = false;
                    ped.WillFight = false;
                    ped.WasEverSetPersistent = true;
                }
                if (yield && localRan == 7)//5
                {
                    GameFiber.Yield();
                    localRan = 0;
                }
                TotalChecked++;
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Updating Civilian Data");
            }
        }
    }
    private void UpdateEMTs()
    {
        int localRan = 0;
        foreach (EMT ped in World.Pedestrians.EMTList.OrderBy(x => x.GameTimeLastUpdated))
        {
            try
            {
                bool yield = false;
                if (ped.NeedsFullUpdate)
                {
                    yield = true;
                    TotalRan++;
                    localRan++;
                }
                ped.Update(Perceptable, PoliceRespondable, Vector3.Zero, World);
                if (!ped.WasEverSetPersistent && ped.Pedestrian.Exists() && ped.Pedestrian.IsPersistent)
                {
                    ped.CanBeAmbientTasked = false;
                    ped.WillCallPolice = false;
                    ped.WillCallPoliceIntense = false;
                    ped.WillFight = false;
                    ped.WasEverSetPersistent = true;
                }
                if (yield && localRan == 5)
                {
                    GameFiber.Yield();
                    localRan = 0;
                }
                TotalChecked++;
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Updating Civilian Data");
            }
        }
    }
    private void UpdateMerchants()
    {
        int localRan = 0;
        foreach (PedExt ped in World.Pedestrians.MerchantList.OrderBy(x => x.GameTimeLastUpdated))
        {
            try
            {
                bool yield = false;
                if (ped.NeedsFullUpdate)
                {
                    yield = true;
                    TotalRan++;
                    localRan++;
                }
                ped.Update(Perceptable, PoliceRespondable, Vector3.Zero, World);
                if (yield && localRan == 5)
                {
                    GameFiber.Yield();
                    localRan = 0;
                }
                TotalChecked++;
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Updating Merchant Data");
            }
        }
    }
    private void UpdateGangMembers()
    {
        int localRan = 0;
        foreach (GangMember ped in World.Pedestrians.GangMemberList.OrderBy(x => x.GameTimeLastUpdated))
        {
            try
            {
                bool yield = false;
                if (ped.NeedsFullUpdate)
                {
                    yield = true;
                    TotalRan++;
                    localRan++;
                }
                ped.Update(Perceptable, PoliceRespondable, Vector3.Zero, World);
                if (yield)
                {
                    ped.WeaponInventory.UpdateSettings();
                }
                


                if (!ped.WasModSpawned && !ped.WasEverSetPersistent && ped.Pedestrian.Exists() && ped.Pedestrian.IsPersistent)
                {
                    ped.CanBeAmbientTasked = false;
                    ped.WillCallPolice = false;
                    ped.WillCallPoliceIntense = false;
                    ped.WillFight = true;
                    ped.WasEverSetPersistent = true;
                }
                if (yield && localRan == 5)//1
                {
                    GameFiber.Yield();
                    localRan = 0;
                }
                TotalChecked++;
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
        foreach (GangMember gangBanger in World.Pedestrians.GangMemberList)
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
            //GameFiber.Yield();
        }

        Perceptable.AnyGangMemberCanSeePlayer = anyGangMemberCanSeePlayer;
        Perceptable.AnyGangMemberCanHearPlayer = anyGangMemberCanHearPlayer;
        Perceptable.AnyGangMemberRecentlySeenPlayer = anyGangMemberRecentlySeenPlayer;
    }
    private void UpdateZombies()
    {
        int localRan = 0;
        foreach (PedExt ped in World.Pedestrians.ZombieList.OrderBy(x => x.GameTimeLastUpdated))
        {
            try
            {
                bool yield = false;
                if (ped.NeedsFullUpdate)
                {
                    yield = true;
                    TotalRan++;
                    localRan++;
                }
                ped.Update(Perceptable, PoliceRespondable, Vector3.Zero, World);
                if (yield && localRan == 5)
                {
                    GameFiber.Yield();
                    localRan = 0;
                }
                TotalChecked++;
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Updating Zombie Data");
            }
        }
    }
}
