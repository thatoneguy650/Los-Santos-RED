using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TaxiServiceContact : PhoneContact, IPhoneContact
{
    public TaxiServiceContact()
    {
    }

    public TaxiServiceContact(string name) : base(name)
    {
    }

    public override void OnAnswered(IContactInteractable player, CellPhone cellPhone, IGangs gangs, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IJurisdictions jurisdictions, ICrimes crimes, IEntityProvideable world, IModItems modItems, IWeapons weapons, INameProvideable names, IShopMenus shopMenus)
    {
        MenuInteraction = new TaxiServiceInteraction(player, gangs, placesOfInterest, settings, modItems, this,crimes,weapons,names,shopMenus,world);
        MenuInteraction.Start(this);
    }

}

