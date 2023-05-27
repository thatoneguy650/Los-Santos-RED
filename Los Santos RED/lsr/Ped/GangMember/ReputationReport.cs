using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ReputationReport
{
    private GangMember GangMember;
    private uint GameTimeLastChangedRep;
    private uint GameTimeFirstChangedRep;
    public ReputationReport(GangMember gangMember)
    {
        GangMember = gangMember;
    }
    public int ReputationChangeAmount { get; private set; }
    public uint GangMemberHandle => GangMember.Handle;  

    public void AddReputation(int amount)
    {
        ReputationChangeAmount =+ amount;
        if(GameTimeFirstChangedRep == 0)
        {
            GameTimeFirstChangedRep = Game.GameTime;
        }
        GameTimeLastChangedRep = Game.GameTime;
    }
    public void ApplyReputation(IPerceptable Player)
    {
        if(ReputationChangeAmount == 0)
        {
            return;
        }
        EntryPoint.WriteToConsole($"APPLIED REPUTATION FOR {GangMember.Handle} ReputationChangeAmount {ReputationChangeAmount}");
        Player.RelationshipManager.GangRelationships.ChangeReputation(GangMember.Gang, ReputationChangeAmount, true);
        Player.RelationshipManager.GangRelationships.AddAttacked(GangMember.Gang);
        ReputationChangeAmount = 0;
    }
    public void Update(IPerceptable Player, IEntityProvideable world, ISettingsProvideable settings)
    {
        if(ReputationChangeAmount == 0 && !GangMember.WitnessedReports.Any(x=> x.ReputationChangeAmount != 0))
        {
            return;
        }
        EntryPoint.WriteToConsole($"ReputationReport.Update {GangMember.Handle}");
        if (!GangMember.Pedestrian.Exists())
        {
            if(!GangMember.IsDead && GangMember.EverSeenPlayer)
            {
                EntryPoint.WriteToConsole($"GANGMEMBER DOES NOT EXIST, BUT ALIVE, APPLYING REP");
                ApplyReputation(Player);
                GangMember.WitnessedReports.ForEach(x=> x.ApplyReputation(Player));
                GangMember.WitnessedReports.Clear();
            }
            return;
        }
        if(!GangMember.IsDead && GangMember.EverSeenPlayer)
        {
            if(GangMember.DistanceToPlayer >= settings.SettingsManager.GangSettings.DistanceToReportRepChanges)
            {
                EntryPoint.WriteToConsole($"GANGMEMBER EXISTS AND IS FAR FROM PLAYER, BUT ALIVE, APPLYING REP");
                ApplyReputation(Player);
                GangMember.WitnessedReports.ForEach(x => x.ApplyReputation(Player));
                GangMember.WitnessedReports.Clear();
            }
            if (Game.GameTime - GameTimeFirstChangedRep >= settings.SettingsManager.GangSettings.GameTimeToReportRepChanges && GangMember.DistanceToPlayer >= settings.SettingsManager.GangSettings.MinDistanceToReportTimeoutRepChanges && !GangMember.Pedestrian.IsRagdoll && !GangMember.Pedestrian.IsStunned)
            {
                EntryPoint.WriteToConsole($"GANGMEMBER EXISTS AND HAS HAD REPORT FOR A WHILE, ALIVE, APPLYING REP");
                ApplyReputation(Player);
                GangMember.WitnessedReports.ForEach(x => x.ApplyReputation(Player));
                GangMember.WitnessedReports.Clear();
            }
            if(EntryPoint.FocusZone.AssignedGang?.ID == GangMember.Gang?.ID && Game.GameTime - GameTimeFirstChangedRep >= settings.SettingsManager.GangSettings.GameTimeToReportRepChangesInTerritory && GangMember.DistanceToPlayer >= settings.SettingsManager.GangSettings.MinDistanceToReportTimeoutRepChanges && !GangMember.Pedestrian.IsRagdoll && !GangMember.Pedestrian.IsStunned)
            {
                EntryPoint.WriteToConsole($"GANGMEMBER EXISTS AND HAS HAD REPORT FOR A BIT IN TERRITORY, ALIVE, APPLYING REP");
                ApplyReputation(Player);
                GangMember.WitnessedReports.ForEach(x => x.ApplyReputation(Player));
                GangMember.WitnessedReports.Clear();
            }
        }
        if(GangMember.IsDead)
        {
            bool isRecentlyKilled = Game.GameTime - GangMember.GameTimeKilled <= settings.SettingsManager.GangSettings.GameTimeRecentlyKilled;// 15000;
            bool isPlayerNearby = GangMember.Pedestrian.DistanceTo2D(Player.Character) <= settings.SettingsManager.GangSettings.MurderDistance;//  15f;

            EntryPoint.WriteToConsole($"GANGMEMBER {GangMember.Handle} IS DEAD isRecentlyKilled{isRecentlyKilled} isPlayerNearby{isPlayerNearby}");

            if (isRecentlyKilled || isPlayerNearby)
            {
                foreach(GangMember gm in world.Pedestrians.GangMemberList.Where(x => !x.IsDead && x.Gang.ID == GangMember.Gang.ID && x.RecentlySeenPlayer).ToList())
                {
                    if (!gm.WitnessedReports.Any(x => x.GangMemberHandle == gm.Handle))
                    {
                        EntryPoint.WriteToConsole($"DEAD GANG MEMBER {GangMember.Handle} ADDING WITNESSED REPORT TO {gm.Handle}");
                        gm.WitnessedReports.Add(this);
                    }
                    else
                    {
                        EntryPoint.WriteToConsole($"DEAD GANG MEMBER {GangMember.Handle} ALREADY HAD WITNESSED REPORT TO {gm.Handle}");
                    }
                }
            }
        }
    }
}

