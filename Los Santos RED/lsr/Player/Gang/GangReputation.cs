using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class GangReputation
{
    private GangRespect PreviousGangRelationship = GangRespect.Neutral;
    private int reputationLevel = 200;
    public int DefaultRepAmount = 200;
    public int RepMaximum = 2000;
    public int RepMinimum = -2000;
    private uint GameTimeLastAddedAmbientRep;
    private uint GameTimeLastAttacked;
    public int NeutralRepLevel = 0;
    public int HostileRepLevel = -200;
    public int FriendlyRepLevel = 500;
    private IGangRelateable Player;

    public bool CanAskToJoin => !IsMember && ReputationLevel >= Gang.MemberOfferRepLevel;
    public GangLoan GangLoan { get; private set; }
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
    public bool IsMember { get; set; }
    public bool IsEnemy { get; set; }
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
        HostileRepLevel = Gang.HostileRepLevel;
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
            else if (IsEnemy)
            {
                return GangRespect.Hostile;
            }
            else if(ReputationLevel <= HostileRepLevel)
            {
                return GangRespect.Hostile;
            }
            else if(ReputationLevel > HostileRepLevel && ReputationLevel < FriendlyRepLevel)
            {
                return GangRespect.Neutral;
            }
            else
            {
                return GangRespect.Friendly;
            }
        }
    }
    public bool CanDispatchHitSquad => IsEnemy || ReputationLevel <= Gang.HitSquadRep;
    public bool RecentlyAttacked => GameTimeLastAttacked > 0 && Game.GameTime - GameTimeLastAttacked <= 90000;
    public int MembersHurt { get; set; }
    public int MembersKilled { get; set; }
    public int MembersCarJacked { get; set; }
    public int MembersHurtInTerritory { get; set; }
    public int MembersKilledInTerritory { get; set; }
    public int MembersCarJackedInTerritory { get; set; }
    public int TasksCompleted { get; set; }
    public int PlayerDebt { get; set; } = 0;
    public void ResetRelationshipGroups()
    {
        RelationshipGroup rg = new RelationshipGroup(Gang.ID);
        if (GangRelationship == GangRespect.Hostile)
        {
            rg.SetRelationshipWith(RelationshipGroup.Player, Relationship.Neutral);//changed from hate, lets tone them down
            RelationshipGroup.Player.SetRelationshipWith(rg, Relationship.Neutral);
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
        IsEnemy = false;
        SetReputation(DefaultRepAmount, sendText);
        SetRelationshipGroupNeutral();
        Player.CellPhone.ClearPendingGangTexts(Gang);
        MembersHurt = 0;
        MembersKilled = 0;
        MembersCarJacked = 0;
        MembersHurtInTerritory = 0;
        MembersKilledInTerritory = 0;
        MembersCarJackedInTerritory = 0;
        ClearDebt();
        GangLoan?.Reset();
        GangLoan = null;
        Player.SetDenStatus(Gang, false);
        GameTimeLastAttacked = 0;
    }
    public void AddembientRep()
    {
        if (ReputationLevel < DefaultRepAmount && Game.GameTime - GameTimeLastAddedAmbientRep >= Gang.GameTimeToRecoverAmbientRep)
        {
            SetReputation(ReputationLevel + 1, true);
            GameTimeLastAddedAmbientRep = Game.GameTime;
        }
    }
    public void SetAttacked()
    {
        GameTimeLastAttacked = Game.GameTime;
    }
    private void OnReputationChanged(bool sendText)
    {
        if(PreviousGangRelationship != GangRelationship)
        {
            RelationshipGroup rg = new RelationshipGroup(Gang.ID);
            if (GangRelationship == GangRespect.Hostile || IsEnemy)
            {
                rg.SetRelationshipWith(RelationshipGroup.Player, Relationship.Neutral);
                RelationshipGroup.Player.SetRelationshipWith(rg, Relationship.Neutral); 

                Player.SetDenStatus(Gang, false);
                if (!IsEnemy)
                {
                    if (sendText)
                    {
                        SendInfoText(Gang.Contact, Gang, false);
                    }
                    else
                    {
                        Player.CellPhone.AddContact(Gang.Contact, false);
                    }
                }
            }
            else if (GangRelationship == GangRespect.Friendly)
            {
                rg.SetRelationshipWith(RelationshipGroup.Player, Relationship.Companion);
                RelationshipGroup.Player.SetRelationshipWith(rg, Relationship.Companion);
                //rg.SetRelationshipWith(RelationshipGroup.Player, Relationship.Like);
                //RelationshipGroup.Player.SetRelationshipWith(rg, Relationship.Like);
                Player.SetDenStatus(Gang, true);
                if (sendText)
                {
                    SendInfoText(Gang.Contact, Gang, true);
                }
                else
                {
                    Player.CellPhone.AddContact(Gang.Contact, false);
                }
            }
            else if (GangRelationship == GangRespect.Member || IsMember)
            {
                rg.SetRelationshipWith(RelationshipGroup.Player, Relationship.Companion);
                RelationshipGroup.Player.SetRelationshipWith(rg, Relationship.Companion);
                Player.SetDenStatus(Gang, true);
                if (sendText)
                {
                    SendInfoText(Gang.Contact, Gang, true);
                }
                else
                {
                    Player.CellPhone.AddContact(Gang.Contact, false);
                }
            }
            else if (GangRelationship == GangRespect.Neutral)
            {
                rg.SetRelationshipWith(RelationshipGroup.Player, Relationship.Neutral);
                RelationshipGroup.Player.SetRelationshipWith(rg, Relationship.Neutral);
                Player.SetDenStatus(Gang, false);
            }
            //EntryPoint.WriteToConsole($"GangReputation {Gang.FullName} changed from {PreviousGangRelationship} to {GangRelationship}");
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
        else if (IsEnemy)
        {
            ending = "~r~Enemy~s~";
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
    public string ToStringSimple()
    {
        string ending = "";
        if (GangRelationship == GangRespect.Member)
        {
            ending = "~g~Member~s~";
        }
        else if (IsEnemy)
        {
            ending = "~r~Enemy~s~";
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
        return ending;
    }
    public string ToZoneString()
    {
        string ending = "";
        if (GangRelationship == GangRespect.Member)
        {
            ending = " ~s~(~g~Member~s~)";
        }
        else if (IsEnemy)
        {
            ending = " ~s~(~r~Enemy~s~)";
        }
        else if (GangRelationship == GangRespect.Friendly)
        {
            ending = " ~s~(~g~Friendly~s~)";
        }
        else if (GangRelationship == GangRespect.Hostile)
        {
            ending = " ~s~(~r~Hostile~s~)";
        }
        return ending;
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
    public void SendInfoText(PhoneContact phoneContact, Gang gang, bool isPositive)
    {
        if (gang != null)
        {
            List<string> Replies = new List<string>();
            if (isPositive)
            {
                Replies.AddRange(new List<string>() {
                    $"Heard some good things about you, come see us sometime.",
                    $"Call us soon to discuss business.",
                    $"Might have some business opportunites for you soon, give us a call.",
                    $"You've been making some impressive moves, call us to discuss.",
                    $"Give us a call soon.",
                    $"We may have some opportunites for you.",
                    $"My guys tell me you are legit, hit us up sometime.",
                    $"Looking for people I can trust, if so give us a call.",
                    $"Word has gotten around about you, mostly positive, give us a call soon.",
                    $"Always looking for help with some 'items'. Call us if you think you can handle it.",
                });
            }
            else
            {
                Replies.AddRange(new List<string>() {
                    $"Watch your back",
                    $"Dead man walking",
                    $"ur fucking dead",
                    $"You just fucked with the wrong people asshole",
                    $"We're gonna fuck you up buddy",
                    $"My boys are gonna skin you alive prick.",
                    $"You will die slowly.",
                    $"I'll take pleasure in guttin you boy.",
                    $"Better leave LS while you can...",
                    $"We'll be waiting for you asshole.",
                    $"You're gonna wish you were dead motherfucker.",
                    $"Got some 'associates' out looking for you prick. Where you at?",


                    $"We'll be seeing you soon",
                    $"{Player.PlayerName}? Better watch out.",
                    $"You'll never hear us coming",
                    $"You are a dead man",
                    $"You're gonna find out what happens when you fuck with us asshole.",
                    $"When my boys find you...",
                });
            }
            //dont have zones of territories
            //List<ZoneJurisdiction> myGangTerritories = GangTerritories.GetGangTerritory(gang.ID);
            //ZoneJurisdiction mainTerritory = myGangTerritories.OrderBy(x => x.Priority).FirstOrDefault();

            //if (mainTerritory != null)
            //{
            //    Zone mainGangZone = Zones.GetZone(mainTerritory.ZoneInternalGameName);
            //    if (mainGangZone != null)
            //    {
            //        if (isPositive)
            //        {
            //            Replies.AddRange(new List<string>() {
            //                $"Heard some good things about you, come see us sometime in ~p~{mainGangZone.DisplayName}~s~ to discuss some business",
            //                $"Call us soon to discuss business in ~p~{mainGangZone.DisplayName}~s~.",
            //                $"Might have some business opportunites for you soon in ~p~{mainGangZone.DisplayName}~s~, give us a call.",
            //                $"You've been making some impressive moves, call us to discuss.",
            //            });
            //        }
            //        else
            //        {
            //            Replies.AddRange(new List<string>() {
            //                $"Watch your back next time you are in ~p~{mainGangZone.DisplayName}~s~ motherfucker",
            //                $"You are dead next time we see you in ~p~{mainGangZone.DisplayName}~s~",
            //                $"Better stay out of ~p~{mainGangZone.DisplayName}~s~ cocksucker",
            //            });
            //        }
            //    }
            //}
            string MessageToSend;
            MessageToSend = Replies.PickRandom();
            Player.CellPhone.AddScheduledText(phoneContact, MessageToSend, 1, false);
        }
    }
    public void AddDebt(int value)
    {
        PlayerDebt += value;
    }
    public void ClearDebt()
    {
        PlayerDebt = 0;
    }
    public void TakeLoan(int value,ITimeReportable time, LoanParameter loanParameter, bool sendMessage)
    {
        GangLoan = new GangLoan(Player, Gang, time, loanParameter, value);
        GangLoan.Start(sendMessage);
    }

    public void RestartLoan(GangLoanSave gls, ITimeReportable time)
    {
        if(Gang == null)
        {
            return;
        }   
        LoanParameter lp = Gang.LoanParameters.GetParameters(GangRelationship);
        if(lp == null)
        {
            return;
        }
        GangLoan = new GangLoan(Player, Gang, time, lp, gls.DueAmount);
        GangLoan.RestartFromSaved(gls.DueAmount,gls.VigAmount, gls.MissedPeriods, gls.DueDate, lp);
    }

    public void ClearLoan()
    {
        GangLoan = null;
    }
}