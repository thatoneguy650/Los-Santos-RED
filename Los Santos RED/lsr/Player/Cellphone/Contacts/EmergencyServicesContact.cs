using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class EmergencyServicesContact : PhoneContact, IPhoneContact
{
    public EmergencyServicesContact()
    {

    }

    public EmergencyServicesContact(string name, string iconName) : base(name, iconName)
    {

    }

    public override void OnAnswered(IContactInteractable player, CellPhone cellPhone, IGangs gangs, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IJurisdictions jurisdictions,
        ICrimes crimes, IEntityProvideable world, IModItems modItems, IWeapons weapons, INameProvideable names, IShopMenus shopMenus, IAgencies agencies)
    {
        MenuInteraction = new EmergencyServicesInteraction(player, gangs, placesOfInterest, settings, jurisdictions, crimes, world);
        MenuInteraction.Start(this);
    }

}

