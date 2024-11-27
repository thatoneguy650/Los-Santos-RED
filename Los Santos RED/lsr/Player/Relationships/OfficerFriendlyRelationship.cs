using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class OfficerFriendlyRelationship : ContactRelationship
{
    public OfficerFriendlyRelationship()
    {

    }
    public OfficerFriendlyRelationship(string contactName, PhoneContact phoneContact) : base(contactName, phoneContact)
    {

    }
    public override void SetupContact(IContacts contacts)
    {
        if (contacts == null)
        {
            return;
        }
        PhoneContact = contacts.PossibleContacts.CorruptCopContact;
    }
}

