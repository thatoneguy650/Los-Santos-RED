using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

[XmlInclude(typeof(OfficerFriendlyRelationship))]
[XmlInclude(typeof(GunDealerRelationship))]
public class ContactRelationship
{
    protected PhoneContact PhoneContact;
    protected IContactRelateable Player;
    protected IPlacesOfInterest PlacesOfInterest;
    public readonly int DefaultRepAmount = 200;
    public readonly int RepMaximum = 2000;
    public readonly int RepMinimum = -2000;
    protected int PrevRelationshipLevel;
    public int TotalMoneySpent { get; set; } = 0;
    public int PlayerDebt { get; set; } = 0;
    public int ReputationLevel { get; set; } = 200;//=> reputationLevel;
    public string ContactName { get; set; }


    public virtual int CostToPayoff => ReputationLevel < 0 ? Math.Abs(ReputationLevel) * 2 : 0;
    public virtual string Stuff => "NONE";
    public bool IsHostile => ReputationLevel < 0;


    public bool HasPhoneContact => PhoneContact != null;

    public ContactRelationship()
    {

    }
    public ContactRelationship(string contactName, PhoneContact phoneContact)
    {
        ContactName = contactName;
        PhoneContact = phoneContact;
    }
    public void Setup(IContactRelateable player, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        PlacesOfInterest = placesOfInterest;
    }
    public void Dispose()
    {
        //Reset(false);
    }
    public void Reset(bool sendText)
    {
        EntryPoint.WriteToConsole($"ContactRelationship RESET RAN {TotalMoneySpent}");
        SetReputation(DefaultRepAmount, sendText);
        SetMoneySpent(0, false);
        PlayerDebt = 0;
    }
    public virtual void Deactivate()
    {

    }
    public void SetReputation(int value, bool sendText)
    {
        EntryPoint.WriteToConsole($"ContactRelationship SetReputation value:{value} ContactName:{ContactName} PhoneContactName = {PhoneContact?.Name}");
        if (ReputationLevel != value)
        {
            if (value > RepMaximum)
            {
                ReputationLevel = RepMaximum;
            }
            else if (value < RepMinimum)
            {
                ReputationLevel = RepMinimum;
            }
            else
            {
                ReputationLevel = value;
            }
            OnReputationChanged(sendText);
        }
    }
    public void ChangeReputation(int Amount, bool sendText)
    {
        SetReputation(ReputationLevel + Amount, sendText);
    }
    private void OnReputationChanged(bool sendText)
    {
        
        int CurrentRelationshipLevel = 0;
        if(ReputationLevel < 0)
        {
            CurrentRelationshipLevel = -1;
        }
        else if (ReputationLevel == 0)
        {
            CurrentRelationshipLevel = 0;
        }
        else if (ReputationLevel > 0)
        {
            CurrentRelationshipLevel = 1;
        }
        bool isPositive = CurrentRelationshipLevel > 0;
        if (PrevRelationshipLevel != CurrentRelationshipLevel)
        {
            if(PhoneContact == null)
            {
                EntryPoint.WriteToConsole($"OnReputationChanged for: PhoneContact IS NULL");
            }
            else 
            { 
                if (sendText)
                {
                    SendInfoText(PhoneContact, isPositive);
                }
                else
                {
                    Player.CellPhone.AddContact(PhoneContact, false);
                }
            }
            EntryPoint.WriteToConsole($"OnReputationChanged for: {PhoneContact?.Name} CurrentRelationshipLevel:{CurrentRelationshipLevel} PrevRelationshipLevel:{PrevRelationshipLevel} ReputationLevel{ReputationLevel}");
            PrevRelationshipLevel = CurrentRelationshipLevel;
        }

    }

    public void SendInfoText(PhoneContact phoneContact, bool isPositive)
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
        string MessageToSend;
        MessageToSend = Replies.PickRandom();
        Player.CellPhone.AddScheduledText(phoneContact, MessageToSend, 1, false);
        EntryPoint.WriteToConsole("Contact Relationship SendInfoText");   
    }



    public void AddDebt(int Amount)
    {
        PlayerDebt += Amount;
    }
    public void SetDebt(int Amount)
    {
        PlayerDebt = Amount;
    }
    public virtual void AddMoneySpent(int Amount)
    {
        TotalMoneySpent += Amount;
    }
    public virtual void SetMoneySpent(int Amount, bool sendNotification)
    {
        TotalMoneySpent = Amount;
    }

    public virtual void Activate()
    {

    }

    public virtual void SetupContact(IContacts contacts)
    {
        if(contacts == null)
        {
            return;
        }
        PhoneContact = contacts.PossibleContacts.AllContacts().Where(x => x.Name.ToLower() == ContactName.ToLower()).FirstOrDefault();
    }
}

