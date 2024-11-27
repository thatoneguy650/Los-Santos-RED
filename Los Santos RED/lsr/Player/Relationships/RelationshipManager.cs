using ExtensionsMethods;
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

        ContactRelationships.Clear();
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
    public void OnInteracted(PhoneContact phoneContact, int moneySpent, int repGained)
    {
        if (phoneContact == null)
        {
            EntryPoint.WriteToConsole("phoneContact IS NULL");
            return;
        }
        Gang myGang = Gangs.GetGangByContact(phoneContact.Name);
        if (myGang != null)
        {
            GangRelationships.SetDebt(myGang, 0);
            GangRelationships.OnMoneyWon(myGang, moneySpent);
            return;
        }
        ContactRelationship contactRelationship = GetOrCreate(phoneContact);// ContactRelationships.FirstOrDefault(x => x.ContactName.ToLower() == contactName.ToLower());
        if (contactRelationship == null)
        {
            contactRelationship = phoneContact.CreateRelationship();
            contactRelationship.Setup(ContactRelateable, PlacesOfInterest);
            ContactRelationships.Add(contactRelationship);
        }
        if (contactRelationship == null)
        {
            EntryPoint.WriteToConsole("contactRelationship IS NULL");
        }
        else
        {
            contactRelationship.AddMoneySpent(moneySpent);
            contactRelationship.ChangeReputation(repGained, false);
        }
    }
    public ContactRelationship GetOrCreate(PhoneContact phoneContact)// string contactName)
    {
        if(phoneContact == null)
        {
            return null;
        }
        EntryPoint.WriteToConsole($"ContactRelationship GetOrCreate FOR: {phoneContact.Name}");
        ContactRelationship contactRelationship = ContactRelationships.FirstOrDefault(x => x.ContactName.ToLower() == phoneContact.Name.ToLower());
        if (contactRelationship == null)
        {
            EntryPoint.WriteToConsole($"No contact relationship for {phoneContact.Name} CREATING");
            contactRelationship = phoneContact.CreateRelationship();
            contactRelationship.Setup(ContactRelateable, PlacesOfInterest);
            ContactRelationships.Add(contactRelationship);
        }

        EntryPoint.WriteToConsole($"contactRelationship.HasPhoneContact: {contactRelationship.HasPhoneContact}");
        return contactRelationship;
    }
    public void SetCompleteTask(PhoneContact phoneContact, int repAmountOnCompletion, bool joinGangOnComplete)
    {
        if (phoneContact == null)
        {
            return;
        }
        EntryPoint.WriteToConsole($"SetCompleteTask phoneContact exists:{phoneContact !=null} Name: {phoneContact?.Name}");
        Gang myGang = Gangs.GetGangByContact(phoneContact.Name);
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
            return;
        }
       ContactRelationship contactRelationship = GetOrCreate(phoneContact);
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
    public void SetFailedTask(PhoneContact phoneContact, int repAmountOnFail, int debtAmountOnFail)
    {
        if (phoneContact == null)
        {
            return;
        }
        Gang myGang = Gangs.GetGangByContact(phoneContact.Name);
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
            return;
        }
        ContactRelationship contactRelationship = GetOrCreate(phoneContact);
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
    public void ResetRelationship(PhoneContact phoneContact, bool sendText)
    {
        Gang myGang = Gangs.GetGangByContact(phoneContact.Name);
        if (myGang != null)
        {
            GangRelationships.SetReputation(myGang, 0, sendText);
            return;
        }
        ContactRelationship contactRelationship = GetOrCreate(phoneContact);
        if(contactRelationship == null)
        {
            return;
        }
        contactRelationship.Reset(sendText);
    }
    public void SetMaxReputation(PhoneContact phoneContact, bool sendText)
    {
        Gang myGang = Gangs.GetGangByContact(phoneContact.Name);
        if (myGang != null)
        {
            GangRelationships.SetReputation(myGang, myGang.MaximumRep, sendText);
            return;
        }
        ContactRelationship contactRelationship = GetOrCreate(phoneContact);
        if (contactRelationship == null)
        {
            return;
        }
        contactRelationship.SetReputation(contactRelationship.RepMaximum,sendText);
    }
    public void SetMoneySpent(PhoneContact phoneContact, int moneySpent, bool sendText)
    {
        Gang myGang = Gangs.GetGangByContact(phoneContact.Name);
        if (myGang != null)
        {
            GangRelationships.OnMoneyWon(myGang, moneySpent);
            return;
        }
        ContactRelationship contactRelationship = GetOrCreate(phoneContact);
        if (contactRelationship == null)
        {
            return;
        }
        contactRelationship.SetMoneySpent(moneySpent, sendText);
    }
    public void OnVendorKilledByPlayer(PhoneContact phoneContact, Merchant merchant, IViolateable player, IZones zones, IGangTerritories gangTerritories)
    {
        Gang myGang = Gangs.GetGangByContact(phoneContact.Name);
        if (myGang != null)
        {
            GangRelationships.ChangeReputation(myGang, -2000, true);
            EntryPoint.WriteToConsole("OnVendorKilledByPlayer SET GANG REP -2000");
            return;
        }
        ContactRelationship contactRelationship = GetOrCreate(phoneContact);
        if (contactRelationship == null)
        {
            return;
        }
        contactRelationship.SetReputation(-2000, true);
        EntryPoint.WriteToConsole("OnVendorKilledByPlayer SET REP -2000");
    }
    public void OnVendorInjuredByPlayer(PhoneContact phoneContact, Merchant merchant, IViolateable player, IZones zones, IGangTerritories gangTerritories)
    {
        Gang myGang = Gangs.GetGangByContact(phoneContact.Name);
        if (myGang != null)
        {
            GangRelationships.ChangeReputation(myGang, -500, true);
            EntryPoint.WriteToConsole("OnVendorInjuredByPlayer SET GANG REP -500");
            return;
        }
        ContactRelationship contactRelationship = GetOrCreate(phoneContact);
        if (contactRelationship == null)
        {
            return;
        }
        contactRelationship.SetReputation(-500, true);
        EntryPoint.WriteToConsole("OnVendorInjuredByPlayer SET REP -500");
    }
}