using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GunDealerContact : PhoneContact, IPhoneContact
{
    public GunDealerContact()
    {
    }

    public GunDealerContact(string name) : base(name)
    {
    }

    public override void OnAnswered(IContactInteractable player, CellPhone cellPhone, IGangs gangs, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IJurisdictions jurisdictions, 
        ICrimes crimes, IEntityProvideable world, IModItems modItems, IWeapons weapons, INameProvideable names, IShopMenus shopMenus, IAgencies agencies)
    {
        MenuInteraction = new GunDealerInteraction(player, gangs, placesOfInterest, settings, this);
        MenuInteraction.Start(this);
    }

    public override ContactRelationship CreateRelationship()
    {
        return new GunDealerRelationship(Name, this);
    }
}

