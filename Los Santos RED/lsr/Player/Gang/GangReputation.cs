using Rage;
using System;

[Serializable]
public class GangReputation
{
    private int reputationLevel = 200;
    
    private int RepMaximum = 2000;
    private int RepMinimum = -2000;
    private GangRespect PreviousGangRelationship;
    public readonly int DefaultRepAmount = 200;
    public int ReputationLevel
    {
        get => reputationLevel;
        set
        {
            if (reputationLevel != value)
            {
                if(value > RepMaximum)
                {
                    reputationLevel = RepMaximum;
                }
                else if (value < RepMinimum)
                {
                    reputationLevel = RepMinimum;
                }
                else
                {
                    reputationLevel = value;
                }
                OnReputationChanged();
            }
        }
    }
    
    public GangReputation()
    {

    }
    public GangReputation(Gang gang)
    {
        Gang = gang;
    }
    public Gang Gang { get; set; }
    public GangRespect GangRelationship
    {
        get
        {
            if(ReputationLevel <= 0)
            {
                return GangRespect.Hostile;
            }
            else if(ReputationLevel > 0 && ReputationLevel < 500)
            {
                return GangRespect.Neutral;
            }
            else
            {
                return GangRespect.Friendly;
            }
        }
    }

    public void AddembientRep()
    {
        if (ReputationLevel < DefaultRepAmount && Game.GameTime - GameTimeLastAddedAmbientRep >= Gang.GameTimeToRecoverAmbientRep)
        {
            ReputationLevel += 1;
            GameTimeLastAddedAmbientRep = Game.GameTime;
        }
    }
    public uint GameTimeLastAddedAmbientRep { get; internal set; }
    private void OnReputationChanged()
    {
        if(PreviousGangRelationship != GangRelationship)
        {
            RelationshipGroup rg = new RelationshipGroup(Gang.ID);
            if (GangRelationship == GangRespect.Hostile)
            {
                rg.SetRelationshipWith(RelationshipGroup.Player, Relationship.Hate);
                RelationshipGroup.Player.SetRelationshipWith(rg, Relationship.Hate);
            }
            else if (GangRelationship == GangRespect.Friendly)
            {
                rg.SetRelationshipWith(RelationshipGroup.Player, Relationship.Respect);
                RelationshipGroup.Player.SetRelationshipWith(rg, Relationship.Respect);
            }
            else if (GangRelationship == GangRespect.Neutral)
            {
                rg.SetRelationshipWith(RelationshipGroup.Player, Relationship.Neutral);
                RelationshipGroup.Player.SetRelationshipWith(rg, Relationship.Neutral);
            }
            EntryPoint.WriteToConsole($"GangReputation {Gang.FullName} changed from {PreviousGangRelationship} to {GangRelationship}", 5);
            PreviousGangRelationship = GangRelationship;
        }
    }
    public override string ToString()
    {
        string ending = "";
        if(GangRelationship == GangRespect.Friendly)
        {
            ending = ": ~g~Friendly~s~";
        }
        else if (GangRelationship == GangRespect.Neutral)
        {
            ending = ": ~s~Neutral~s~";
        }
        else if (GangRelationship == GangRespect.Hostile)
        {
            ending = ": ~r~Hostile~s~";
        }
        return Gang.ColorInitials.ToString() + ending + $" ({ReputationLevel})";
    }
}