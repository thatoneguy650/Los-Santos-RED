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
    private IGangs Gangs;
    private uint GameTimeLastUpdatedPeds;
    private int TotalRan;
    private int TotalChecked;
    private int RespondingPolice;
    private int CurrentRespondingPoliceCount;
    private int prevCitizenWantedLevel;

    public Civilians(IEntityProvideable world, IPoliceRespondable policeRespondable, IPerceptable perceptable, ISettingsProvideable settings, IGangs gangs)
    {
        World = world;
        PoliceRespondable = policeRespondable;
        Perceptable = perceptable;
        Settings = settings;
        Gangs = gangs;
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
        GameFiber.Yield();
        UpdateMerchants();
        GameFiber.Yield();
        UpdateZombies();
        GameFiber.Yield();
        UpdateGangMembers();
        GameFiber.Yield();
        UpdateEMTs();
        GameFiber.Yield();

        UpdateTotalWanted();
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

                //GameFiber.Yield();

                if (!ped.WasEverSetPersistent && ped.Pedestrian.Exists() && ped.Pedestrian.IsPersistent)
                {
                    ped.CanBeAmbientTasked = false;
                    ped.WillCallPolice = false;
                    ped.WillCallPoliceIntense = false;
                    ped.WillFight = false;
                    ped.WillFightPolice = false;
                    ped.WasEverSetPersistent = true;
                }
                if (yield && localRan == Settings.SettingsManager.DebugSettings.CivilianUpdateBatch)//5
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
                //GameFiber.Yield();

                if (!ped.WasEverSetPersistent && ped.Pedestrian.Exists() && ped.Pedestrian.IsPersistent)
                {
                    ped.CanBeAmbientTasked = false;
                    ped.WillCallPolice = false;
                    ped.WillCallPoliceIntense = false;
                    ped.WillFight = false;
                    ped.WasEverSetPersistent = true;
                }
                if (yield && localRan == Settings.SettingsManager.DebugSettings.EMTsUpdateBatch)
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
                //GameFiber.Yield();
                if (yield && localRan == Settings.SettingsManager.DebugSettings.MerchantsUpdateBatch)
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

        string playerGangID = Perceptable.RelationshipManager.GangRelationships.CurrentGang?.ID;

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
                //GameFiber.Yield();

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
                    ped.WillFightPolice = true;
                    ped.WasEverSetPersistent = true;
                }

                if(ped.Gang?.ID == playerGangID)
                {
                    ped.PlayerKnownsName = true;
                    ped.IsTrustingOfPlayer = true;
                }

                if (yield && localRan == Settings.SettingsManager.DebugSettings.GangUpdateBatch)//1
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
        }
        Perceptable.AnyGangMemberCanSeePlayer = anyGangMemberCanSeePlayer;
        Perceptable.AnyGangMemberCanHearPlayer = anyGangMemberCanHearPlayer;
        Perceptable.AnyGangMemberRecentlySeenPlayer = anyGangMemberRecentlySeenPlayer;
        GameFiber.Yield();
        List<Gang> WantedGangs = World.Pedestrians.GangMemberList.Where(x => x.IsWanted && !x.IsBusted).GroupBy(x => x.Gang).Select(x => x.Key).ToList();
        foreach (Gang gang in Gangs.AllGangs.Where(x=> x.HasWantedMembers))
        {
            RelationshipGroup gangRG = new RelationshipGroup(gang.ID);
            if (!WantedGangs.Contains(gang))
            {     
                RelationshipGroup.Cop.SetRelationshipWith(gangRG, Relationship.Neutral);
                gangRG.SetRelationshipWith(RelationshipGroup.Cop, Relationship.Neutral);
                gang.HasWantedMembers = false;
                EntryPoint.WriteToConsole($"GANG {gang.ID} has no wanted members, settings relationship with cops to neutral");
            }
        }
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
    private void UpdateTotalWanted()
    {
        PedExt worstPed = World.Pedestrians.Citizens.Where(x => !x.IsBusted && !x.IsArrested).OrderByDescending(x => x.WantedLevel).FirstOrDefault();
        Vector3 PoliceInterestPoint;
        if (worstPed != null && worstPed.Pedestrian.Exists() && worstPed.WantedLevel > PoliceRespondable.WantedLevel)
        {
            World.TotalWantedLevel = worstPed.WantedLevel;
            PoliceInterestPoint = worstPed.PedViolations.PlacePoliceLastSeen;
        }
        else
        {
            World.TotalWantedLevel = PoliceRespondable.WantedLevel;
            PoliceInterestPoint = Vector3.Zero;
        }
        World.PoliceBackupPoint = PoliceInterestPoint;
        //if(PoliceInterestPoint == Vector3.Zero)
        //{
        //    World.PoliceBackupPoint = PoliceInterestPoint;
        //}
        //else if(World.PoliceBackupPoint == Vector3.Zero || PoliceInterestPoint.DistanceTo2D(World.PoliceBackupPoint) >= 25f)
        //{
        //    World.PoliceBackupPoint = PoliceInterestPoint;
        //    EntryPoint.WriteToConsole("Police Interest Point Changed by 25");
        //}


        if (worstPed != null)
        {
            World.CitizenWantedLevel = worstPed.WantedLevel;
        }
        else
        {
            World.CitizenWantedLevel = 0;
        }


        if(prevCitizenWantedLevel != World.CitizenWantedLevel)
        {
            if(World.CitizenWantedLevel > 1 && !PoliceRespondable.Investigation.IsActive && PoliceRespondable.IsNotWanted)
            {
                PoliceRespondable.Scanner.OnRequestedBackUpSimple();
            }
            else
            {

            }
            EntryPoint.WriteToConsole($"Citizen Wanted Level Changed from {prevCitizenWantedLevel} to {World.CitizenWantedLevel}");
            prevCitizenWantedLevel = World.CitizenWantedLevel;
        }


        if (Settings.SettingsManager.PoliceSettings.AllowRespondingWithoutCallIn)
        {
            if (World.CitizenWantedLevel > 0 && World.PoliceBackupPoint != Vector3.Zero)
            {
                AssignCops();
                GameFiber.Yield();
            }
            else
            {
                foreach (Cop cop in World.Pedestrians.Police.Where(x => x.IsRespondingToCitizenWanted))
                {
                    cop.IsRespondingToCitizenWanted = false;
                }
            }
        }
    }

    private void AssignCops()
    {
        if (World.CitizenWantedLevel > 0)
        {
            RespondingPolice = PoliceToRespond(World.CitizenWantedLevel);    
            int tasked = 0;
            foreach (Cop cop in World.Pedestrians.Police.Where(x => x.Pedestrian.Exists() && !x.IsRespondingToWanted && !x.IsRespondingToInvestigation && (World.CitizenWantedLevel >= 3 || x.AssignedAgency?.Classification == Classification.Police || x.AssignedAgency?.Classification == Classification.Sheriff)).OrderBy(x => x.Pedestrian.DistanceTo2D(World.PoliceBackupPoint)))//first pass, only want my police and whatever units?
            {
                if(!cop.IsInVehicle && cop.Pedestrian.DistanceTo2D(World.PoliceBackupPoint) >= 150f)
                {
                    cop.IsRespondingToCitizenWanted = false;
                }
                else if (!cop.IsDead && !cop.IsUnconscious && tasked < RespondingPolice)
                {
                    cop.IsRespondingToCitizenWanted = true;
                    tasked++;
                }
                else
                {
                    cop.IsRespondingToCitizenWanted = false;
                }
            }
            if(tasked < RespondingPolice)
            {
                foreach (Cop cop in World.Pedestrians.Police.Where(x => x.Pedestrian.Exists() && !x.IsRespondingToCitizenWanted && !x.IsRespondingToWanted && !x.IsRespondingToInvestigation && x.AssignedAgency?.Classification != Classification.Police && x.AssignedAgency?.Classification != Classification.Sheriff).OrderBy(x => x.Pedestrian.DistanceTo2D(World.PoliceBackupPoint)))
                {
                    if (!cop.IsInVehicle && cop.Pedestrian.DistanceTo2D(World.PoliceBackupPoint) >= 150f)
                    {
                        cop.IsRespondingToCitizenWanted = false;
                    }
                    else if (!cop.IsDead && !cop.IsUnconscious && tasked < RespondingPolice)
                    {
                        cop.IsRespondingToCitizenWanted = true;
                        tasked++;
                    }
                }
            }
            CurrentRespondingPoliceCount = tasked;
        }
        else
        {
            foreach (Cop cop in World.Pedestrians.Police.Where(x => x.IsRespondingToCitizenWanted))
            {
                cop.IsRespondingToCitizenWanted = false;
            }
            CurrentRespondingPoliceCount = 0;
        }
    }

    private int PoliceToRespond(int wantedLevel)
    {
        {
            if (wantedLevel >= 6)
            {
                return Settings.SettingsManager.PoliceSettings.InvestigationRespondingOfficers_Wanted6;
            }
            if (wantedLevel >= 5)
            {
                return Settings.SettingsManager.PoliceSettings.InvestigationRespondingOfficers_Wanted5;
            }
            else if (wantedLevel >= 4)
            {
                return Settings.SettingsManager.PoliceSettings.InvestigationRespondingOfficers_Wanted4;
            }
            else if (wantedLevel >= 3)
            {
                return Settings.SettingsManager.PoliceSettings.InvestigationRespondingOfficers_Wanted3;
            }
            else if (wantedLevel >= 2)
            {
                return Settings.SettingsManager.PoliceSettings.InvestigationRespondingOfficers_Wanted2;
            }
            else
            {
                return Settings.SettingsManager.PoliceSettings.InvestigationRespondingOfficers_Wanted1;
            }
        }
    }

}
