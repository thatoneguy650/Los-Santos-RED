using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RelationshipManager
{
    private IGangs Gangs;
    private ISettingsProvideable Settings;
    private IPlacesOfInterest PlacesOfInterest;
    private ITimeControllable TimeControllable;
    private IContactRelateable ContactRelateable;
    public RelationshipManager(IGangs gangs, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest, ITimeControllable timeControllable, IGangRelateable gangRelateable, IContactRelateable contactRelateable)
    {
        Gangs = gangs;
        Settings = settings;
        PlacesOfInterest = placesOfInterest;
        TimeControllable = timeControllable;
        ContactRelateable = contactRelateable;
        GangRelationships = new GangRelationships(Gangs, gangRelateable, Settings, PlacesOfInterest, TimeControllable);
        ContactRelationships = new List<ContactRelationship>();
    }
    public GangRelationships GangRelationships { get; private set; }
    public List<ContactRelationship> ContactRelationships { get; private set; } = new List<ContactRelationship>();
    public void Dispose()
    {
        GangRelationships.Dispose();
        foreach (ContactRelationship relationship in ContactRelationships)
        {
            relationship.Dispose();
        }
    }
    public void Reset(bool sendText)
    {
        GangRelationships.Reset();
        foreach (ContactRelationship relationship in ContactRelationships)
        {
            //relationship.Reset(sendText);
            relationship.Deactivate();
        }
        ContactRelationships.Clear();
    }
    public void Setup()
    {
        GangRelationships.Setup();
        foreach(ContactRelationship relationship in ContactRelationships)
        {
            relationship.Setup(ContactRelateable, PlacesOfInterest);
        }
    }
    public void Add(ContactRelationship contactRelationship)
    {
        if(contactRelationship == null || ContactRelationships.Any(x => x.ContactName.ToLower() == contactRelationship.ContactName.ToLower()))
        {
            return;
        }
        contactRelationship.Setup(ContactRelateable, PlacesOfInterest);
        ContactRelationships.Add(contactRelationship);
    }
    public void OnInteracted(string contactName, int moneySpent, int repGained)
    {
        ContactRelationship contactRelationship = GetOrCreate(contactName);
        if(contactRelationship == null)
        {
            return;
        }
        contactRelationship.AddMoneySpent(moneySpent);
        contactRelationship.SetReputation(repGained, false);
    }
    private ContactRelationship GetOrCreate(string contactName)
    {
        ContactRelationship contactRelationship = ContactRelationships.FirstOrDefault(x => x.ContactName.ToLower() == contactName.ToLower());
        if (contactRelationship == null)
        {
            contactRelationship = new ContactRelationship(contactName);
            contactRelationship.Setup(ContactRelateable, PlacesOfInterest);
            ContactRelationships.Add(contactRelationship);
        }
        return contactRelationship;
    }
    public void SetCompleteTask(string contactName, int repAmountOnCompletion, bool joinGangOnComplete)
    {
        Gang myGang = Gangs.GetGangByContact(contactName);
        if (myGang != null)
        {
            if (repAmountOnCompletion != 0)
            {
                GangRelationships.ChangeReputation(myGang, repAmountOnCompletion, false);
            }
            GangRelationships.SetDebt(myGang, 0);
            GangRelationships.SetCompletedTask(myGang);
            if (joinGangOnComplete)
            {
                GangRelationships.SetGang(myGang, true);
            }
        }
       ContactRelationship contactRelationship = GetOrCreate(contactName);
        if (contactRelationship == null)
        {
            return;
        }
        if (repAmountOnCompletion != 0)
        {
            contactRelationship.ChangeReputation(repAmountOnCompletion, false);
        }
        contactRelationship.SetDebt(0);  
    }
    public void SetFailedTask(string contactName, int repAmountOnFail, int debtAmountOnFail)
    {
        Gang myGang = Gangs.GetGangByContact(contactName);
        if (myGang != null)
        {
            if (repAmountOnFail != 0)
            {
                GangRelationships.ChangeReputation(myGang, repAmountOnFail, false);
            }
            if (debtAmountOnFail != 0)
            {
                GangRelationships.AddDebt(myGang, debtAmountOnFail);
            }
            GangRelationships.SetFailedTask(myGang);
        }
        ContactRelationship contactRelationship = GetOrCreate(contactName);
        if (contactRelationship == null)
        {
            return;
        }
        if (repAmountOnFail != 0)
        {
            contactRelationship.ChangeReputation(repAmountOnFail, false);
        }
        if (debtAmountOnFail != 0)
        {
            contactRelationship.AddDebt(debtAmountOnFail);
        }     
    }

    public void ResetRelationship(string contactName, bool sendText)
    {
        ContactRelationship contactRelationship = GetOrCreate(contactName);
        if(contactRelationship == null)
        {
            return;
        }
        contactRelationship.Reset(sendText);
    }
    public void SetMaxReputation(string contactName, bool sendText)
    {
        ContactRelationship contactRelationship = GetOrCreate(contactName);
        if (contactRelationship == null)
        {
            return;
        }
        contactRelationship.SetReputation(contactRelationship.RepMaximum,sendText);
    }
    public void SetMoneySpent(string contactName, int moneySpent, bool sendText)
    {
        ContactRelationship contactRelationship = GetOrCreate(contactName);
        if (contactRelationship == null)
        {
            return;
        }
        contactRelationship.SetMoneySpent(moneySpent, sendText);
    }
}