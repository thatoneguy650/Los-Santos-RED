using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.IO;
using System.Xml.Serialization;

[XmlInclude(typeof(GangContact))]
[XmlInclude(typeof(GunDealerContact))]
[XmlInclude(typeof(EmergencyServicesContact))]
[XmlInclude(typeof(CorruptCopContact))]
public class PhoneContact
{
    public string Name { get; set; } = "Unknown"; 
    public int Index { get; set; } = 0;
    public bool Active { get; set; } = true;
    public int DialTimeout { get; set; } = 3000;  
    public bool RandomizeDialTimeout { get; set; } = true;
    public string IconName { get; set; } = "CHAR_BLANK_ENTRY";
    public bool Bold { get; set; } = false;

    [XmlIgnore]
    public IContactMenuInteraction MenuInteraction { get; set; }
    public PhoneContact(string name)
    {
        Name = name;
    }
    public PhoneContact(string name, string iconName)
    {
        Name = name;
        IconName = iconName;
    }
    public PhoneContact()
    {

    }
    public virtual void OnAnswered(IContactInteractable player, CellPhone cellPhone, IGangs gangs, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IJurisdictions jurisdictions, ICrimes crimes, IEntityProvideable world)
    {
        cellPhone.Close(0);
    }

 }
