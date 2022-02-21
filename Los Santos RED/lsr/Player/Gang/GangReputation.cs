using LosSantosRED.lsr.Interface;
using Rage;
using System;

[Serializable]
public class GangReputation
{
    private int reputationLevel = 200;
    private GangRespect PreviousGangRelationship = GangRespect.Neutral;
    public readonly int DefaultRepAmount = 200;
    public readonly int RepMaximum = 2000;
    public readonly int RepMinimum = -2000;
    private IGangRelateable Player;
    public int ReputationLevel
    {
        get => reputationLevel;
        //set
        //{
        //    if (reputationLevel != value)
        //    {
        //        if(value > RepMaximum)
        //        {
        //            reputationLevel = RepMaximum;
        //        }
        //        else if (value < RepMinimum)
        //        {
        //            reputationLevel = RepMinimum;
        //        }
        //        else
        //        {
        //            reputationLevel = value;
        //        }
        //        OnReputationChanged();
        //    }
        //}
    } 
    public GangReputation()
    {

    }
    public GangReputation(Gang gang, IGangRelateable player)
    {
        Gang = gang;
        Player = player;
    }
    public Gang Gang { get; set; }
    public GangRespect GangRelationship
    {
        get
        {
            if(ReputationLevel < 0)
            {
                return GangRespect.Hostile;
            }
            else if(ReputationLevel >= 0 && ReputationLevel < 500)
            {
                return GangRespect.Neutral;
            }
            else
            {
                return GangRespect.Friendly;
            }
        }
    }
   // public bool HasActiveTask { get; set; }
    public int MembersHurt { get; set; }
    public int MembersKilled { get; set; }
    public int MembersCarJacked { get; set; }

    public int MembersHurtInTerritory { get; set; }
    public int MembersKilledInTerritory { get; set; }
    public int MembersCarJackedInTerritory { get; set; }

    public int PlayerDebt { get; set; } = 0;

    public void SetRepuation(int value, bool sendText)
    {
        if(reputationLevel != value)
        {
            if (value > RepMaximum)
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
            OnReputationChanged(sendText);
        }
    }
    public void Reset(bool sendText)
    {
        SetRepuation(DefaultRepAmount, sendText);
        MembersHurt = 0;
        MembersKilled = 0;
        MembersCarJacked = 0;
        MembersHurtInTerritory = 0;
        MembersKilledInTerritory = 0;
        MembersCarJackedInTerritory = 0;
        PlayerDebt = 0;
    }
    public void AddembientRep()
    {
        if (ReputationLevel < DefaultRepAmount && Game.GameTime - GameTimeLastAddedAmbientRep >= Gang.GameTimeToRecoverAmbientRep)
        {
            SetRepuation(ReputationLevel + 1, true);
            GameTimeLastAddedAmbientRep = Game.GameTime;
        }
    }
    public uint GameTimeLastAddedAmbientRep { get; internal set; }
    private void OnReputationChanged(bool sendText)
    {
        if(PreviousGangRelationship != GangRelationship)
        {
            RelationshipGroup rg = new RelationshipGroup(Gang.ID);
            if (GangRelationship == GangRespect.Hostile)
            {
                rg.SetRelationshipWith(RelationshipGroup.Player, Relationship.Hate);
                RelationshipGroup.Player.SetRelationshipWith(rg, Relationship.Hate);
                Player.SetDenStatus(Gang, false);
                if (sendText)
                {
                    Player.CellPhone.AddGangText(Gang, false);
                }
                else
                {
                    Player.CellPhone.AddContact(Gang, false);
                }
            }
            else if (GangRelationship == GangRespect.Friendly)
            {
                rg.SetRelationshipWith(RelationshipGroup.Player, Relationship.Like);
                RelationshipGroup.Player.SetRelationshipWith(rg, Relationship.Like);
                //rg.SetRelationshipWith(RelationshipGroup.Player, Relationship.Respect);
                //RelationshipGroup.Player.SetRelationshipWith(rg, Relationship.Respect);
                Player.SetDenStatus(Gang, true);
                if (sendText)
                {
                    Player.CellPhone.AddGangText(Gang, true);
                }
                else
                {
                    Player.CellPhone.AddContact(Gang, false);
                }
            }
            else if (GangRelationship == GangRespect.Neutral)
            {
                rg.SetRelationshipWith(RelationshipGroup.Player, Relationship.Neutral);
                RelationshipGroup.Player.SetRelationshipWith(rg, Relationship.Neutral);
                Player.SetDenStatus(Gang, false);
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
    public string ToStringBare()
    {
        string ending = "";
        if (GangRelationship == GangRespect.Friendly)
        {
            ending = "~g~Friendly~s~";
        }
        else if (GangRelationship == GangRespect.Neutral)
        {
            ending = "~s~Neutral~s~";
        }
        else if (GangRelationship == GangRespect.Hostile)
        {
            ending = "~r~Hostile~s~";
        }
        return ending + $" ({ReputationLevel})";
    }
    public string ToStringBareRaw()
    {
        string ending = "";
        if (GangRelationship == GangRespect.Friendly)
        {
            ending = "~g~Friendly~s~";
        }
        else if (GangRelationship == GangRespect.Neutral)
        {
            ending = "~s~Neutral~s~";
        }
        else if (GangRelationship == GangRespect.Hostile)
        {
            ending = "~r~Hostile~s~";
        }
        return ending;
    }
    public string ToBlip()
    {
        string ending = "";
        if (GangRelationship == GangRespect.Friendly)
        {
            ending = "~g~(Friendly)~s~";
        }
        else if (GangRelationship == GangRespect.Neutral)
        {
            ending = "";
        }
        else if (GangRelationship == GangRespect.Hostile)
        {
            ending = "~r~(Hostile)~s~";
        }
        return ending;
    }
}