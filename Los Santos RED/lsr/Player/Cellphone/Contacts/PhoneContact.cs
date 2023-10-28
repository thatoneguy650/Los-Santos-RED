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
[XmlInclude(typeof(KillerContact))]
[XmlInclude(typeof(VehicleExporterContact))]
[XmlInclude(typeof(TaxiServiceContact))]
public class PhoneContact
{

    protected ContactRelationship contactRelationship;
    public string Name { get; set; } = "Unknown"; 
    public int Index { get; set; } = 0;
    public bool Active { get; set; } = true;
    public int DialTimeout { get; set; } = 3000;  
    public bool RandomizeDialTimeout { get; set; } = true;
    public string IconName { get; set; } = "CHAR_BLANK_ENTRY";
    public bool IsDefault { get; set; } = false;
    public string FullNumber { get; set; } = "";
    public string Number { get; set; } = "";
    [XmlIgnore]
    public IContactMenuInteraction MenuInteraction { get; set; }
    public int CurrentDialTimeout => RandomizeDialTimeout ? RandomizedDialTimeout : DialTimeout;
    [XmlIgnore]
    public int RandomizedDialTimeout { get; set; } = 3000;
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
    public virtual void OnAnswered(IContactInteractable player, CellPhone cellPhone, IGangs gangs, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IJurisdictions jurisdictions, ICrimes crimes, IEntityProvideable world, IModItems modItems, IWeapons weapons, INameProvideable names, IShopMenus shopMenus, IAgencies agencies)
    {
        GameFiber.Sleep(1000);


       // player.CellPhone.BurnerPhone.ReturnHome(0);


        player.CellPhone.Close(250);


    }
    public virtual ContactRelationship CreateRelationship()
    {
        return new ContactRelationship(Name);
    }
}
