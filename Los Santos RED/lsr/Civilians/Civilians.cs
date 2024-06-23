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


    private int TotalMerchantsRan;
    private int TotalMerchantsChecked;
    private int TotalGangMembersRan;
    private int TotalGangMembersChecked;
    private int TotalEMTsRan;
    private int TotalEMTsChecked;

    private int RespondingPolice;
    private int CurrentRespondingPoliceCount;
    private int prevCitizenWantedLevel;
    private uint GameTimeLastUpdatedEMTPeds;
    private uint GameTimeLastUpdatedMerchantPeds;
    private uint GameTimeLastUpdatedGangMemberPeds;
    private uint GameTimeLastUpdatedSecurityPeds;
    private int TotalSecurityGuardsChecked;
    private int TotalSecurityGuardsRan;

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
    public void UpdateCivilians()
    {
        TotalRan = 0;
        TotalChecked = 0;
        int localRan = 0;
        foreach (PedExt ped in World.Pedestrians.CivilianList.OrderBy(x => x.GameTimeLastUpdated))//.Take(30))
        {
            try
            {
                bool yield = false;
                if (ped.NeedsFullUpdate || Settings.SettingsManager.PerformanceSettings.YieldAfterEveryPedExtUpdate)
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
                    ped.WillFightPolice = false;
                    ped.WillAlwaysFightPolice = false;
                    ped.WasEverSetPersistent = true;
                    ped.WillCower = false;
                }
                if (yield && localRan == Settings.SettingsManager.PerformanceSettings.CivilianUpdateBatch)//5
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
        if (Settings.SettingsManager.PerformanceSettings.PrintUpdateTimes || Settings.SettingsManager.PerformanceSettings.PrintCivilianUpdateTimes || Settings.SettingsManager.PerformanceSettings.PrintCivilianOnlyUpdateTimes)
        {
            EntryPoint.WriteToConsole($"Civilians.Update Ran Time Since {Game.GameTime - GameTimeLastUpdatedPeds} TotalRan: {TotalRan} TotalChecked: {TotalChecked}", 5);
        }
        GameTimeLastUpdatedPeds = Game.GameTime;
    }
    public void UpdateEMTs()
    {
        int localRan = 0;
        TotalEMTsRan = 0;
        TotalEMTsChecked = 0;
        foreach (EMT ped in World.Pedestrians.EMTList.OrderBy(x => x.GameTimeLastUpdated))
        {
            try
            {
                bool yield = false;
                if (ped.NeedsFullUpdate || Settings.SettingsManager.PerformanceSettings.YieldAfterEveryPedExtUpdate)
                {
                    yield = true;
                    TotalEMTsRan++;
                    localRan++;
                }
                ped.Update(Perceptable, PoliceRespondable, Vector3.Zero, World);
                if (!ped.WasModSpawned && !ped.WasEverSetPersistent && ped.Pedestrian.Exists() && ped.Pedestrian.IsPersistent)
                {
                    ped.CanBeAmbientTasked = false;
                    ped.WillCallPolice = false;
                    ped.WillCallPoliceIntense = false;
                    ped.WillFight = false;
                    ped.WasEverSetPersistent = true;
                }
                if (yield && localRan == Settings.SettingsManager.PerformanceSettings.EMTsUpdateBatch)
                {
                    GameFiber.Yield();
                    localRan = 0;
                }
                TotalEMTsChecked++;
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Updating Civilian Data");
            }
        }

        if (Settings.SettingsManager.PerformanceSettings.PrintUpdateTimes || Settings.SettingsManager.PerformanceSettings.PrintCivilianUpdateTimes)
        {
            EntryPoint.WriteToConsole($"Civilians.UpdateEMTs Ran Time Since {Game.GameTime - GameTimeLastUpdatedEMTPeds} TotalRan: {TotalEMTsRan} TotalChecked: {TotalEMTsChecked}", 5);
        }
        GameTimeLastUpdatedEMTPeds = Game.GameTime;
    }
    public void UpdateFirefighters()
    {
        int localRan = 0;
        TotalEMTsRan = 0;
        TotalEMTsChecked = 0;
        foreach (Firefighter ped in World.Pedestrians.FirefighterList.OrderBy(x => x.GameTimeLastUpdated))
        {
            try
            {
                bool yield = false;
                if (ped.NeedsFullUpdate || Settings.SettingsManager.PerformanceSettings.YieldAfterEveryPedExtUpdate)
                {
                    yield = true;
                    TotalEMTsRan++;
                    localRan++;
                }
                ped.Update(Perceptable, PoliceRespondable, Vector3.Zero, World);
                if (!ped.WasModSpawned && !ped.WasEverSetPersistent && ped.Pedestrian.Exists() && ped.Pedestrian.IsPersistent)
                {
                    ped.CanBeAmbientTasked = false;
                    ped.WillCallPolice = false;
                    ped.WillCallPoliceIntense = false;
                    ped.WillFight = false;
                    ped.WasEverSetPersistent = true;
                }
                if (yield && localRan == Settings.SettingsManager.PerformanceSettings.EMTsUpdateBatch)
                {
                    GameFiber.Yield();
                    localRan = 0;
                }
                TotalEMTsChecked++;
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Updating Civilian Data");
            }
        }

        if (Settings.SettingsManager.PerformanceSettings.PrintUpdateTimes || Settings.SettingsManager.PerformanceSettings.PrintCivilianUpdateTimes)
        {
            EntryPoint.WriteToConsole($"Civilians.UpdateEMTs Ran Time Since {Game.GameTime - GameTimeLastUpdatedEMTPeds} TotalRan: {TotalEMTsRan} TotalChecked: {TotalEMTsChecked}", 5);
        }
        GameTimeLastUpdatedEMTPeds = Game.GameTime;
    }
    public void UpdateMerchants()
    {
        int localRan = 0;
        TotalMerchantsRan = 0;
        TotalMerchantsChecked = 0;
        foreach (PedExt ped in World.Pedestrians.ServiceWorkers.OrderBy(x => x.GameTimeLastUpdated))
        {
            try
            {
                bool yield = false;
                if (ped.NeedsFullUpdate || Settings.SettingsManager.PerformanceSettings.YieldAfterEveryPedExtUpdate)
                {
                    yield = true;
                    TotalMerchantsRan++;
                    localRan++;
                }
                ped.Update(Perceptable, PoliceRespondable, Vector3.Zero, World);
                if (yield && localRan == Settings.SettingsManager.PerformanceSettings.MerchantsUpdateBatch)
                {
                    GameFiber.Yield();
                    localRan = 0;
                }
                TotalMerchantsChecked++;
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Updating Merchant Data");
            }
        }

        if (Settings.SettingsManager.PerformanceSettings.PrintUpdateTimes || Settings.SettingsManager.PerformanceSettings.PrintCivilianUpdateTimes)
        {
            EntryPoint.WriteToConsole($"Civilians.UpdateMerchants Ran Time Since {Game.GameTime - GameTimeLastUpdatedMerchantPeds} TotalRan: {TotalMerchantsRan} TotalChecked: {TotalMerchantsChecked}", 5);
        }
        GameTimeLastUpdatedMerchantPeds = Game.GameTime;
    }
    public void UpdateGangMembers()
    {
        int localRan = 0;
        TotalGangMembersRan = 0;
        TotalGangMembersChecked = 0;

        string playerGangID = Perceptable.RelationshipManager.GangRelationships.CurrentGang?.ID;

        foreach (GangMember ped in World.Pedestrians.GangMemberList.OrderBy(x => x.GameTimeLastUpdated))
        {
            try
            {
                bool yield = false;
                if (ped.NeedsFullUpdate || Settings.SettingsManager.PerformanceSettings.YieldAfterEveryPedExtUpdate)
                {
                    yield = true;
                    TotalGangMembersRan++;
                    localRan++;
                }
                ped.Update(Perceptable, PoliceRespondable, Vector3.Zero, World);
                if (Settings.SettingsManager.GangSettings.AllowAmbientSpeech)
                {
                    ped.UpdateSpeech(PoliceRespondable);
                }
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
                    ped.WillAlwaysFightPolice = true;
                    ped.WasEverSetPersistent = true;
                }
                if(ped.Gang?.ID == playerGangID)
                {
                    ped.PlayerKnownsName = true;
                    ped.IsTrustingOfPlayer = true;
                }
                if (yield && localRan == Settings.SettingsManager.PerformanceSettings.GangUpdateBatch)//1
                {
                    GameFiber.Yield();
                    localRan = 0;
                }
                TotalGangMembersChecked++;
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Updating GangMember Data");
            }
        }
        GameFiber.Yield();

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

                RelationshipGroup.SecurityGuard.SetRelationshipWith(gangRG, Relationship.Neutral);
                gangRG.SetRelationshipWith(RelationshipGroup.SecurityGuard, Relationship.Neutral);


                gang.HasWantedMembers = false;
                //EntryPoint.WriteToConsoleTestLong($"GANG {gang.ID} has no wanted members, settings relationship with cops to neutral");
            }
        }
        if (Settings.SettingsManager.PerformanceSettings.PrintUpdateTimes || Settings.SettingsManager.PerformanceSettings.PrintCivilianUpdateTimes)
        {
            EntryPoint.WriteToConsole($"Civilians.UpdateGangMembers Ran Time Since {Game.GameTime - GameTimeLastUpdatedGangMemberPeds} TotalRan: {TotalGangMembersRan} TotalChecked: {TotalGangMembersChecked}", 5);
        }
        GameTimeLastUpdatedGangMemberPeds = Game.GameTime;

    }
    public void UpdateSecurityGuards()
    {
        int localRan = 0;
        TotalSecurityGuardsRan = 0;
        TotalSecurityGuardsChecked = 0;
        foreach (SecurityGuard ped in World.Pedestrians.SecurityGuardList.OrderBy(x => x.GameTimeLastUpdated))
        {
            try
            {
                bool yield = false;
                if (ped.NeedsFullUpdate || Settings.SettingsManager.PerformanceSettings.YieldAfterEveryPedExtUpdate)
                {
                    yield = true;
                    TotalSecurityGuardsRan++;
                    localRan++;
                }
                ped.Update(Perceptable, PoliceRespondable, Vector3.Zero, World);
                if (Settings.SettingsManager.PoliceSpeechSettings.AllowAmbientSpeech)
                {
                    ped.UpdateSpeech(PoliceRespondable);
                }
                if (!ped.WasModSpawned && !ped.WasEverSetPersistent && ped.Pedestrian.Exists() && ped.Pedestrian.IsPersistent)
                {
                    ped.CanBeAmbientTasked = false;
                    ped.WillCallPolice = false;
                    ped.WillCallPoliceIntense = false;
                    ped.WillFight = false;
                    ped.WasEverSetPersistent = true;
                }
                if (yield && localRan == Settings.SettingsManager.PerformanceSettings.SecurityGuardsUpdateBatch)
                {
                    GameFiber.Yield();
                    localRan = 0;
                }
                TotalSecurityGuardsChecked++;
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Updating Civilian Data");
            }
        }
        if (Settings.SettingsManager.SecuritySettings.AllowDetainment && PoliceRespondable.IsNotWanted)
        {
            if (PoliceRespondable.IsDetainable && (PoliceRespondable.IsIncapacitated || (PoliceRespondable.IsDangerouslyArmed && PoliceRespondable.IsStill)) && World.Pedestrians.SecurityGuardList.Any(x => x.CanSeePlayer && x.ShouldDetainPlayer))
            {
                GameFiber.Yield();
                PoliceRespondable.Arrest();
                //EntryPoint.WriteToConsoleTestLong("Security Detain 1");
            }
            if (PoliceRespondable.IsDetainable && PoliceRespondable.IsAttemptingToSurrender && World.Pedestrians.SecurityGuardList.Any(x => x.CanSeePlayer && x.DistanceToPlayer <= 10f && x.HeightToPlayer <= 5f))
            {
                GameFiber.Yield();
                PoliceRespondable.Arrest();
                //EntryPoint.WriteToConsoleTestLong("Security Detain 2");
            }
        }
        if (Settings.SettingsManager.PerformanceSettings.PrintUpdateTimes || Settings.SettingsManager.PerformanceSettings.PrintCivilianUpdateTimes)
        {
            EntryPoint.WriteToConsole($"Civilians.UpdateSecurityGuards Ran Time Since {Game.GameTime - GameTimeLastUpdatedSecurityPeds} TotalRan: {TotalSecurityGuardsRan} TotalChecked: {TotalSecurityGuardsChecked}", 5);
        }
        GameTimeLastUpdatedSecurityPeds = Game.GameTime;
    }
    public void UpdateTotalWanted()
    {
        PedExt worstPed = World.Pedestrians.Citizens.Where(x => !x.IsBusted && !x.IsArrested && !x.IsUnconscious && !x.IsDead).OrderByDescending(x => x.WantedLevel).FirstOrDefault();
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



        if(PoliceRespondable.IsCop && PoliceRespondable.ClosestPoliceDistanceToPlayer >= 150f && !PoliceRespondable.IsSetAutoCallBackup)
        {
            World.PoliceBackupPoint = Vector3.Zero;
        }
        else
        {
            World.PoliceBackupPoint = PoliceInterestPoint;
        }


        







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
            if(World.CitizenWantedLevel > 1 && !PoliceRespondable.Investigation.IsActive && PoliceRespondable.IsNotWanted && !PoliceRespondable.IsCop)
            {
                PoliceRespondable.Scanner.OnRequestedBackUpSimple();
            }
            //EntryPoint.WriteToConsoleTestLong($"Citizen Wanted Level Changed from {prevCitizenWantedLevel} to {World.CitizenWantedLevel}");
            prevCitizenWantedLevel = World.CitizenWantedLevel;
        }
        if (Settings.SettingsManager.PoliceTaskSettings.AllowRespondingWithoutCallIn)
        {
            if (World.CitizenWantedLevel > 0 && World.PoliceBackupPoint != Vector3.Zero)
            {
                AssignCops();
                GameFiber.Yield();
            }
            else
            {
                foreach (Cop cop in World.Pedestrians.AllPoliceList.Where(x => x.IsRespondingToCitizenWanted))
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
            foreach (Cop cop in World.Pedestrians.AllPoliceList.Where(x => x.Pedestrian.Exists() && !x.IsRespondingToWanted && !x.IsRespondingToInvestigation && (World.CitizenWantedLevel >= 3 || x.AssignedAgency?.Classification == Classification.Police || x.AssignedAgency?.Classification == Classification.Sheriff)).OrderBy(x => x.Pedestrian.DistanceTo2D(World.PoliceBackupPoint)))//first pass, only want my police and whatever units?
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
                foreach (Cop cop in World.Pedestrians.AllPoliceList.Where(x => x.Pedestrian.Exists() && !x.IsRespondingToCitizenWanted && !x.IsRespondingToWanted && !x.IsRespondingToInvestigation && x.AssignedAgency?.Classification != Classification.Police && x.AssignedAgency?.Classification != Classification.Sheriff).OrderBy(x => x.Pedestrian.DistanceTo2D(World.PoliceBackupPoint)))
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
            foreach (Cop cop in World.Pedestrians.AllPoliceList.Where(x => x.IsRespondingToCitizenWanted))
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
                return Settings.SettingsManager.PoliceTaskSettings.InvestigationRespondingOfficers_Wanted6;
            }
            if (wantedLevel >= 5)
            {
                return Settings.SettingsManager.PoliceTaskSettings.InvestigationRespondingOfficers_Wanted5;
            }
            else if (wantedLevel >= 4)
            {
                return Settings.SettingsManager.PoliceTaskSettings.InvestigationRespondingOfficers_Wanted4;
            }
            else if (wantedLevel >= 3)
            {
                return Settings.SettingsManager.PoliceTaskSettings.InvestigationRespondingOfficers_Wanted3;
            }
            else if (wantedLevel >= 2)
            {
                return Settings.SettingsManager.PoliceTaskSettings.InvestigationRespondingOfficers_Wanted2;
            }
            else
            {
                return Settings.SettingsManager.PoliceTaskSettings.InvestigationRespondingOfficers_Wanted1;
            }
        }
    }

}


//private void UpdateZombies()
//{
//    int localRan = 0;
//    foreach (PedExt ped in World.Pedestrians.ZombieList.OrderBy(x => x.GameTimeLastUpdated))
//    {
//        try
//        {
//            bool yield = false;
//            if (ped.NeedsFullUpdate)
//            {
//                yield = true;
//                TotalRan++;
//                localRan++;
//            }
//            ped.Update(Perceptable, PoliceRespondable, Vector3.Zero, World);
//            if (yield && localRan == 5)
//            {
//                GameFiber.Yield();
//                localRan = 0;
//            }
//            TotalChecked++;
//        }
//        catch (Exception e)
//        {
//            EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
//            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Updating Zombie Data");
//        }
//    }
//}