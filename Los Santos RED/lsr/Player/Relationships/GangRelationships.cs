using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GangRelationships
{
    private IGangs Gangs;
    private IGangRelateable Player;
    private ISettingsProvideable Settings;
    private IPlacesOfInterest PlacesOfInterest;
    private ITimeReportable Time;
    public List<GangReputation> GangReputations { get; private set; } = new List<GangReputation>();
    public Gang CurrentGang { get; private set; }
    public GangKickUp CurrentGangKickUp { get; private set; }
    public List<Gang> EnemyGangs => GangReputations.Where(x => x.IsEnemy).Select(x => x.Gang).ToList();
    public List<Gang> HitSquadGangs => GangReputations.Where(x=> x.CanDispatchHitSquad).Select(x => x.Gang).ToList();
    public GangRelationships(IGangs gangs, IGangRelateable player, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest, ITimeReportable time)
    {
        Gangs = gangs;
        Player = player;
        Settings = settings;
        PlacesOfInterest = placesOfInterest;
        Time = time;
    }
    public void Dispose()
    {
        Reset();
        CurrentGangKickUp?.Dispose();
        GangReputations.ForEach(x => x.GangLoan?.Dispose());
    }
    public void Setup()
    {
        foreach (Gang gang in Gangs.GetAllGangs())
        {
            GangReputations.Add(new GangReputation(gang, Player));
        }
    }
    public void Update()
    {
        //string CurrentGangTerritoryID = Player.CurrentLocation.CurrentZone.AssignedGang;//might need the key here instead of just iniitilas
        foreach (GangReputation rg in GangReputations)
        {
            if (!rg.IsMember)
            {
                int WantedRep = rg.Gang.RemoveRepOnWantedInTerritoryScalar * Player.WantedLevel;
                if (Player.IsWanted)
                {
                    if(rg.Gang.RemoveRepOnWantedInTerritory && rg.Gang.ID == Player.CurrentLocation.CurrentZone?.AssignedGang?.ID)
                    {
                        if(rg.IsEnemy || rg.GangRelationship == GangRespect.Hostile)
                        {
                            ChangeReputation(rg.Gang, -1 * WantedRep, false);
                        }
                        else if(rg.ReputationLevel >= WantedRep && (rg.ReputationLevel - WantedRep) >= rg.HostileRepLevel)
                        {
                            ChangeReputation(rg.Gang, -1 * WantedRep, false);
                        }
                    }


                    //if (rg.Gang.RemoveRepOnWantedInTerritory && rg.Gang.ID == Player.CurrentLocation.CurrentZone?.AssignedGang?.ID && rg.ReputationLevel >= WantedRep && (rg.ReputationLevel - WantedRep) >= rg.HostileRepLevel)
                    //{
                    //    ChangeReputation(rg.Gang, -1 * WantedRep, false);
                    //}
                }
                else
                {
                    if (rg.Gang.AddAmbientRep && !rg.IsEnemy)
                    {
                        rg.AddembientRep();
                    }
                }
            }
            rg.GangLoan?.Update();
        }
        CurrentGangKickUp?.Update();
        
    }
    public void ChangeReputation(Gang gang, int amount, bool sendNotification)
    {
        if (gang == null)
        {
            return;
        }
        GangReputation gr = GangReputations.Where(x => x.Gang.ID == gang.ID).FirstOrDefault();
        if(gr == null)
        {
            gr = new GangReputation(gang, Player);
            GangReputations.Add(gr);
        }
        int preValue = gr.ReputationLevel;
        gr.SetReputation(gr.ReputationLevel + amount, sendNotification);
        //if (amount > 1)
        //{
        //    EntryPoint.WriteToConsole($"GangRelationships ChangeReputation {gang.FullName} preValue {preValue} amount {amount} current {gr.ReputationLevel}", 5);
        //}
    }
    public void AddAttacked(Gang gang)
    {
        if (gang == null)
        {
            return;
        }
        GangReputation gr = GangReputations.Where(x => x.Gang.ID == gang.ID).FirstOrDefault();
        if (gr == null)
        {
            gr = new GangReputation(gang, Player);
            GangReputations.Add(gr);
        }
        gr.SetAttacked(); 
        //EntryPoint.WriteToConsole($"GangRelationships AddAttacked {gang.FullName} {gr.RecentlyAttacked} RecentlyAttacked {gr.RecentlyAttacked} current {gr.ReputationLevel}", 5);
    }
    public void SetReputation(Gang gang, int value, bool sendNotification)
    {
        if(gang == null)
        {
            return;
        }
        GangReputation gr = GangReputations.Where(x => x.Gang.ID == gang.ID).FirstOrDefault();
        if (gr == null)
        {
            gr = new GangReputation(gang, Player);
            GangReputations.Add(gr);
        }
        int preValue = gr.ReputationLevel;
        gr.SetReputation(value, sendNotification);
        //EntryPoint.WriteToConsole($"GangRelationships SetReputation {gang.FullName} preValue {preValue} toset {value} current {gr.ReputationLevel}", 5);
    }
    public void AddDebt(Gang gang, int amount)
    {
        if (gang == null)
        {
            return;
        }
        GangReputation gr = GangReputations.Where(x => x.Gang.ID == gang.ID).FirstOrDefault();
        if (gr == null)
        {
            gr = new GangReputation(gang, Player);
            GangReputations.Add(gr);
        }
        if (!gr.IsMember)
        {
            gr.AddDebt(Math.Abs(amount));
            //gr.PlayerDebt += Math.Abs(amount);
        }
    }
    public void ClearDebt(Gang gang)
    {
        SetDebt(gang, 0);
    }
    public void SetDebt(Gang gang, int amount)
    {
        if (gang == null)
        {
            return;
        }
        GangReputation gr = GangReputations.Where(x => x.Gang.ID == gang.ID).FirstOrDefault();
        if (gr == null)
        {
            gr = new GangReputation(gang, Player);
            GangReputations.Add(gr);
        }
        gr.PlayerDebt = Math.Abs(amount);
    }
    public void SetRepStats(Gang gang, int hurt, int hurtInTerritory, int killed, int killedInTerritory, int carjacked, int carjackedInTerritory, int playerDebt, bool isMember, bool isEnemy, int tasksCompleted)
    {
        if (gang == null)
        {
            return;
        }
        GangReputation gr = GangReputations.Where(x => x.Gang.ID == gang.ID).FirstOrDefault();
        if (gr == null)
        {
            gr = new GangReputation(gang, Player);
            GangReputations.Add(gr);
        }
        gr.MembersHurt = hurt;
        gr.MembersHurtInTerritory = hurtInTerritory;
        gr.MembersKilled = killed;
        gr.MembersKilledInTerritory = killedInTerritory;
        gr.MembersCarJacked = carjacked;
        gr.MembersCarJackedInTerritory = carjackedInTerritory;
        gr.PlayerDebt = playerDebt;
        gr.IsMember = isMember;
        gr.IsEnemy = isEnemy;
        gr.TasksCompleted = tasksCompleted;
    }
    public void SetKickStatus(Gang gang, DateTime kickDueDate, int kickMissedPeriods, int kickMissedAmount)
    {
        CurrentGangKickUp = new GangKickUp(Player, gang, Time);
        CurrentGangKickUp.Restart(kickDueDate, kickMissedPeriods, kickMissedAmount);
    }
    public void Reset()
    {
        foreach (GangReputation rg in GangReputations)
        {
            rg.Reset(false);
        }
        foreach (GangDen gd in PlacesOfInterest.PossibleLocations.GangDens)
        {
            gd.ResetItems();
        }
        foreach (DeadDrop gd in PlacesOfInterest.PossibleLocations.DeadDrops)
        {
            gd.Reset();
        }
        CurrentGang = null;
        CurrentGangKickUp = null;
    }
    public void SetAllRandomReputations()
    {
        foreach (GangReputation rg in GangReputations)
        {
            rg.SetReputation(RandomItems.GetRandomNumberInt(rg.RepMinimum, rg.RepMaximum),false);
        }
    }
    public string PrintRelationships()
    {
        string toReturn = "";
        foreach (GangReputation rg in GangReputations.OrderByDescending(x=> x.GangRelationship == GangRespect.Hostile).ThenByDescending(x=> x.GangRelationship == GangRespect.Friendly).ThenByDescending(x=> Math.Abs(x.ReputationLevel)).ThenBy(x=>x.Gang.ShortName))
        {
            toReturn += rg.ToString() + "~n~";
            if(toReturn.Length >= 800)
            {
                toReturn += "...";
            }
        }
        return toReturn;
    }
    public GangReputation GetReputation(Gang gang)
    {
        if (gang == null)
        {
            return null;
        }
        GangReputation gr = GangReputations.Where(x => x.Gang.ID == gang.ID).FirstOrDefault();
        if (gr == null)
        {
            gr = new GangReputation(gang, Player);
            GangReputations.Add(gr);
        }
        return gr;
    }
    public bool IsHostile(Gang gang)
    {
        if (gang == null)
        {
            return false;
        }
        GangReputation gr = GangReputations.Where(x => x.Gang.ID == gang.ID).FirstOrDefault();
        if (gr != null)
        {
            return gr.GangRelationship == GangRespect.Hostile;
        }
        else
        {
            GangReputation newRep = new GangReputation(gang, Player);
            GangReputations.Add(newRep);
            return newRep.GangRelationship == GangRespect.Hostile;
        }
    }
    public bool IsFriendly(Gang gang)
    {
        if (gang == null)
        {
            return false;
        }
        GangReputation gr = GangReputations.Where(x => x.Gang.ID == gang.ID).FirstOrDefault();
        if (gr != null)
        {
            return gr.GangRelationship == GangRespect.Friendly;
        }
        else
        {
            GangReputation newRep = new GangReputation(gang, Player);
            GangReputations.Add(newRep);
            return newRep.GangRelationship == GangRespect.Friendly;
        }
    }
    public int GetRepuationLevel(Gang gang)
    {
        if (gang == null)
        {
            return 0;
        }
        GangReputation gr = GangReputations.Where(x => x.Gang.ID == gang.ID).FirstOrDefault();
        if (gr != null)
        {
            return gr.ReputationLevel;
        }
        else
        {
            GangReputation newRep = new GangReputation(gang, Player);
            GangReputations.Add(newRep);
            return newRep.ReputationLevel;
        }
    }
    public void SetFriendlyReputations()
    {
        foreach (GangReputation rg in GangReputations)
        {
            rg.SetReputation(rg.FriendlyRepLevel,false);
        }
    }
    public void SetHostileReputations()
    {
        foreach (GangReputation rg in GangReputations)
        {
            rg.SetReputation(rg.HostileRepLevel,false);
        }
    }
    public void SetSingleRandomReputation()
    {
        GangReputation gr = GangReputations.PickRandom();
        if(gr != null)
        {
            GangReputations.PickRandom()?.SetReputation(RandomItems.GetRandomNumberInt(gr.RepMinimum, gr.RepMaximum), false);
        }
        
    }
    public void OnLostWanted()
    {
        foreach (GangReputation gangRep in GangReputations)
        {
            gangRep.ResetRelationshipGroups();
        }
    }
    public void OnBecameWanted()
    {
        foreach (GangReputation gangRep in GangReputations)
        {
            if(gangRep.GangRelationship == GangRespect.Hostile)
            {
                gangRep.SetRelationshipGroupNeutral();
            }
        }      
    }
    public void SetGang(Gang gang, bool showNotification)
    {
        ResetGang(showNotification);
        if (gang != null)
        {
            CurrentGang = gang;
            CurrentGangKickUp = new GangKickUp(Player, CurrentGang, Time);
            CurrentGangKickUp.Start(true);
            foreach (GangReputation rg in GangReputations)
            {
                if (rg.Gang?.ID == gang.ID)
                {
                    rg.Reset(false);
                    rg.IsMember = true;
                    rg.SetReputation(rg.RepMaximum, false);
                }
                else
                {
                    rg.IsMember = false;
                }

                if(CurrentGang.EnemyGangs?.Any(x=> x == rg.Gang?.ID) == true)
                {
                    rg.Reset(false);
                    rg.IsEnemy = true;
                    rg.SetReputation(rg.RepMinimum, false);
                }
                else
                {
                    rg.IsEnemy = false;
                }
            }
            if (showNotification)
            {
                Game.DisplayHelp($"Joined {CurrentGang.FullName}");
            }
        }
    }
    public void ResetGang(bool showNotification)
    {
        if (CurrentGang != null)
        {
            CurrentGangKickUp.Dispose();
            CurrentGangKickUp = null;
            foreach (GangReputation rg in GangReputations)
            {
                if (rg.Gang?.ID == CurrentGang.ID)
                {
                    rg.Reset(false);
                }
                else
                {
                    rg.IsEnemy = false;
                    rg.IsMember = false;
                }
            }
            if (showNotification)
            {
                Game.DisplayHelp($"Left {CurrentGang.FullName}");
            }
            CurrentGang = null;
        }
    }
    public void SetCompletedTask(Gang gang)
    {
        if (gang == null)
        {
            return;
        }
        GangReputation gr = GangReputations.Where(x => x.Gang.ID == gang.ID).FirstOrDefault();
        if (gr == null)
        {
            gr = new GangReputation(gang, Player);
            GangReputations.Add(gr);
        }
        gr.TasksCompleted++;

        
    }
    public void SetFailedTask(Gang gang)
    {
        if (gang == null)
        {
            return;
        }
        GangReputation gr = GangReputations.Where(x => x.Gang.ID == gang.ID).FirstOrDefault();
        if (gr == null)
        {
            gr = new GangReputation(gang, Player);
            GangReputations.Add(gr);
        }
        gr.TasksCompleted--;
    }
    public void OnMoneyWon(Gang associatedGang, int totalMoneyWon)
    {
        if(associatedGang == null)
        {
            return;
        }
        if(totalMoneyWon >= 0)
        {
            return;
        }
        ChangeReputation(associatedGang, Math.Abs(totalMoneyWon), true);
        EntryPoint.WriteToConsole($"OnMoneyWon {associatedGang.ShortName} YOU WON {totalMoneyWon} ADDING REP:{Math.Abs(totalMoneyWon)}");
    }
}

