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
    private List<GangReputation> GangReputations =  new List<GangReputation>();
    //private int DefaultRepAmount = 200;
    //private int RepMaximum = 2000;
    //private int RepMinimum = -2000;
    private IGangRelateable Player;
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
            GangReputations.Add(new GangReputation(gang));
        }
    }
    public void Update()
    {
        string CurrentGangTerritoryID = Player.CurrentLocation.CurrentZone.AssignedGangInitials;//might need the key here instead of just iniitilas
        foreach (GangReputation rg in GangReputations)
        {
            if(Player.IsWanted)
            {
                if (rg.Gang.ColorInitials == CurrentGangTerritoryID)
                {
                    ChangeReputation(rg.Gang, -5);
                }
            }
            else
            {
                rg.AddembientRep();
            }
        }
    }
    public void ChangeReputation(Gang gang, int amount)
    {
        if (gang == null)
        {
            return;
        }
        GangReputation gr = GangReputations.Where(x => x.Gang.ID == gang.ID).FirstOrDefault();
        if(gr == null)
        {
            gr = new GangReputation(gang);
            GangReputations.Add(gr);
        }
        gr.ReputationLevel += amount;
        if (amount > 1)
        {
            EntryPoint.WriteToConsole($"GangRelationships ChangeReputation {gang.FullName} amount {amount} current {gr.ReputationLevel}", 5);
        }
    }
    public void SetReputation(Gang gang, int value)
    {
        if(gang == null)
        {
            return;
        }
        GangReputation gr = GangReputations.Where(x => x.Gang.ID == gang.ID).FirstOrDefault();
        if (gr == null)
        {
            gr = new GangReputation(gang);
            GangReputations.Add(gr);
        }
        gr.ReputationLevel = value;
        EntryPoint.WriteToConsole($"GangRelationships SetReputation {gang.FullName} value {value} current {gr.ReputationLevel}", 5);
    }
    public void ResetReputations()
    {
        foreach (GangReputation rg in GangReputations)
        {
            rg.ReputationLevel = rg.DefaultRepAmount;
        }
    }
    public void RandomReputations()
    {
        foreach (GangReputation rg in GangReputations)
        {
            rg.ReputationLevel = RandomItems.GetRandomNumberInt(-200, 600);
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
            gr = new GangReputation(gang);
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
            GangReputation newRep = new GangReputation(gang);
            GangReputations.Add(newRep);
            return newRep.GangRelationship == GangRespect.Hostile;
        }
    }

}

