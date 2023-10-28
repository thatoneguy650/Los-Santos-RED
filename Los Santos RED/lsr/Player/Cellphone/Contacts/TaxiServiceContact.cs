using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class TaxiServiceContact : PhoneContact, IPhoneContact
{
    [XmlIgnore]
    public TaxiFirm TaxiFirm { get; set; }
    public TaxiServiceContact()
    {
    }

    public TaxiServiceContact(string name, string iconName) : base(name, iconName)
    {
    }

    public override void OnAnswered(IContactInteractable player, CellPhone cellPhone, IGangs gangs, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IJurisdictions jurisdictions,
        ICrimes crimes, IEntityProvideable world, IModItems modItems, IWeapons weapons, INameProvideable names, IShopMenus shopMenus, IAgencies agencies)
    {
        MenuInteraction = new TaxiServiceInteraction(player, gangs, placesOfInterest, settings, modItems, this,crimes,weapons,names,shopMenus,world, TaxiFirm);
        MenuInteraction.Start(this);
    }

}

