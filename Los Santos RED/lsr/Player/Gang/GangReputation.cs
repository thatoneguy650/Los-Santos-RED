using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using System;

[Serializable]
public class GangReputation
{

    private GangRespect PreviousGangRelationship = GangRespect.Neutral;




    private int reputationLevel = 200;
    public int DefaultRepAmount = 200;
    public int RepMaximum = 2000;
    public int RepMinimum = -2000;

    public int NeutralRepLevel = 0;
    public int FriendlyRepLevel = 500;

    private IGangRelateable Player;
    public int ReputationLevel
    {
        get => reputationLevel;
    }
    public int CostToPayoff
    {
        get
        {
            if (ReputationLevel < NeutralRepLevel)
            {
                return ((NeutralRepLevel - ReputationLevel) * Gang.CostToPayoffGangScalar).Round(100);
            }
            else if (ReputationLevel >= FriendlyRepLevel)
            {
                return 0;
            }
            else
            {
                return ((FriendlyRepLevel - ReputationLevel) * Gang.CostToPayoffGangScalar).Round(100);
            }
        }
    }
    public int RepToNextLevel
    {
        get
        {
            if (ReputationLevel < NeutralRepLevel)
            {
                return NeutralRepLevel - ReputationLevel;
            }
            else if (ReputationLevel >= FriendlyRepLevel)
            {
                return 0;
            }
            else
            {
                return FriendlyRepLevel - ReputationLevel;
            }
        }
    }
    //public int CostToPayoff
    //{
    //    get
    //    {
    //        if (ReputationLevel < 0)
    //        {
    //            return ((0 - ReputationLevel) * Gang.CostToPayoffGangScalar).Round(100);
    //        }
    //        else if (ReputationLevel >= 500)
    //        {
    //            return 0;
    //        }
    //        else
    //        {
    //            return ((500 - ReputationLevel) * Gang.CostToPayoffGangScalar).Round(100);
    //        }
    //    }
    //}
    //public int RepToNextLevel
    //{
    //    get
    //    {
    //        if (ReputationLevel < 0)
    //        {
    //            return 0 - ReputationLevel;
    //        }
    //        else if (ReputationLevel >= 500)
    //        {
    //            return 0;
    //        }
    //        else
    //        {
    //            return 500 - ReputationLevel;
    //        }
    //    }
    //}
    public bool IsMember { get; set; }

    public GangReputation()
    {

    }
    public GangReputation(Gang gang, IGangRelateable player)
    {
        Gang = gang;
        Player = player;

        RepMinimum = Gang.MinimumRep;
        RepMaximum = Gang.MaximumRep;
        DefaultRepAmount = Gang.StartingRep;
        reputationLevel = Gang.StartingRep;
        NeutralRepLevel = Gang.NeutralRepLevel;
        FriendlyRepLevel = Gang.FriendlyRepLevel;
    }
    public Gang Gang { get; set; }
    public GangRespect GangRelationship
    {
        get
        {
            if(IsMember)
            {
                return GangRespect.Member;
            }
            else if(ReputationLevel < NeutralRepLevel)
            {
                return GangRespect.Hostile;
            }
            else if(ReputationLevel >= NeutralRepLevel && ReputationLevel < FriendlyRepLevel)
            {
                return GangRespect.Neutral;
            }
            else
            {
                return GangRespect.Friendly;
            }
        }
    }
    public bool RecentlyAttacked => GameTimeLastAttacked > 0 && Game.GameTime - GameTimeLastAttacked <= 30000;
    public int MembersHurt { get; set; }
    public int MembersKilled { get; set; }
    public int MembersCarJacked { get; set; }

    public int MembersHurtInTerritory { get; set; }
    public int MembersKilledInTerritory { get; set; }
    public int MembersCarJackedInTerritory { get; set; }

    public int PlayerDebt { get; set; } = 0;
    public void ResetRelationshipGroups()
    {
        RelationshipGroup rg = new RelationshipGroup(Gang.ID);
        if (GangRelationship == GangRespect.Hostile)
        {
            rg.SetRelationshipWith(RelationshipGroup.Player, Relationship.Dislike);//changed from hate, lets tone them down
            RelationshipGroup.Player.SetRelationshipWith(rg, Relationship.Dislike);
        }
        else if (GangRelationship == GangRespect.Friendly)
        {
            rg.SetRelationshipWith(RelationshipGroup.Player, Relationship.Like);//changed from like to companion?
            RelationshipGroup.Player.SetRelationshipWith(rg, Relationship.Like);      
        }
        else if (GangRelationship == GangRespect.Neutral)
        {
            rg.SetRelationshipWith(RelationshipGroup.Player, Relationship.Neutral);
            RelationshipGroup.Player.SetRelationshipWith(rg, Relationship.Neutral);
        }
        else if (GangRelationship == GangRespect.Member)
        {
            rg.SetRelationshipWith(RelationshipGroup.Player, Relationship.Companion);
            RelationshipGroup.Player.SetRelationshipWith(rg, Relationship.Companion);
        }
    }
    public void SetRelationshipGroupNeutral()
    {
        RelationshipGroup rg = new RelationshipGroup(Gang.ID);
        rg.SetRelationshipWith(RelationshipGroup.Player, Relationship.Neutral);
        RelationshipGroup.Player.SetRelationshipWith(rg, Relationship.Neutral);
    }
    public void SetReputation(int value, bool sendText)
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
        IsMember = false;
        SetReputation(DefaultRepAmount, sendText);
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
            SetReputation(ReputationLevel + 1, true);
            GameTimeLastAddedAmbientRep = Game.GameTime;
        }
    }
    public uint GameTimeLastAddedAmbientRep { get; set; }
    public uint GameTimeLastAttacked { get; set; }
    private void OnReputationChanged(bool sendText)
    {
        if(PreviousGangRelationship != GangRelationship)
        {
            RelationshipGroup rg = new RelationshipGroup(Gang.ID);
            if (GangRelationship == GangRespect.Hostile)
            {
                if (Player.IsNotWanted)//handled on the became/lost wanted events
                {
                    rg.SetRelationshipWith(RelationshipGroup.Player, Relationship.Hate);
                    RelationshipGroup.Player.SetRelationshipWith(rg, Relationship.Hate);
                }
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
                rg.SetRelationshipWith(RelationshipGroup.Player, Relationship.Neutral);
                RelationshipGroup.Player.SetRelationshipWith(rg, Relationship.Neutral);
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
            else if (GangRelationship == GangRespect.Member || IsMember)
            {
                rg.SetRelationshipWith(RelationshipGroup.Player, Relationship.Companion);
                RelationshipGroup.Player.SetRelationshipWith(rg, Relationship.Companion);
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
        string repLevel = $" ({ReputationLevel})";
        if (GangRelationship == GangRespect.Member)
        {
            ending = ": ~g~Member~s~";
            repLevel = "";
        }
        else if (GangRelationship == GangRespect.Friendly)
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
        return Gang.ColorInitials.ToString() + ending + repLevel;
    }
    public string ToStringBare()
    {
        string ending = "";
        string repLevel = $" ({ReputationLevel})";
        if (GangRelationship == GangRespect.Member)
        {
            ending = "~g~Member~s~";
            repLevel = "";
        }
        else if (GangRelationship == GangRespect.Friendly)
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
        return ending + repLevel;
    }
    public string ToBlip()
    {
        string ending = "";
        if (GangRelationship == GangRespect.Member)
        {
            ending = "~g~(Member)~s~";
        }
        else if (GangRelationship == GangRespect.Friendly)
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