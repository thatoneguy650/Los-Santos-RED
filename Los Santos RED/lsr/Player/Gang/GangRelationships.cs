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
    public List<GangReputation> GangReputations { get; private set; } = new List<GangReputation>();
    public GangRelationships(IGangs gangs, IGangRelateable player)
    {
        Gangs = gangs;
        Player = player;
    }
    public void Dispose()
    {
        ResetReputations();
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
        string CurrentGangTerritoryID = Player.CurrentLocation.CurrentZone.AssignedGangInitials;//might need the key here instead of just iniitilas
        int WantedRep = 5 * Player.WantedLevel;
        foreach (GangReputation rg in GangReputations)
        {
            if(Player.IsWanted)
            {
                if (rg.Gang.ColorInitials == CurrentGangTerritoryID  && rg.ReputationLevel >= WantedRep)
                {
                    ChangeReputation(rg.Gang, -1 * WantedRep, false);
                }
            }
            else
            {
                rg.AddembientRep();
            }
        }
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
        gr.SetRepuation(gr.ReputationLevel + amount, sendNotification);
        if (amount > 1)
        {
            EntryPoint.WriteToConsole($"GangRelationships ChangeReputation {gang.FullName} amount {amount} current {gr.ReputationLevel}", 5);
        }
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
        gr.SetRepuation(value, sendNotification);
        EntryPoint.WriteToConsole($"GangRelationships SetReputation {gang.FullName} value {value} current {gr.ReputationLevel}", 5);
    }
    public void ResetReputations()
    {
        foreach (GangReputation rg in GangReputations)
        {
            rg.SetRepuation(rg.DefaultRepAmount,false);
        }
    }
    public void SetRandomReputations()
    {
        foreach (GangReputation rg in GangReputations)
        {
            rg.SetRepuation(RandomItems.GetRandomNumberInt(-200, 600),false);
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
            rg.SetRepuation(rg.RepMaximum,false);
        }
    }
    public void SetHostileReputations()
    {
        foreach (GangReputation rg in GangReputations)
        {
            rg.SetRepuation(rg.RepMinimum,false);
        }
    }
}

