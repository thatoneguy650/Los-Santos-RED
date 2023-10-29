using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GangContact : PhoneContact, IPhoneContact
{
    public GangContact()
    {

    }

    public GangContact(string name, string iconName) : base(name, iconName)
    {

    }
    public override void OnAnswered(IContactInteractable player, CellPhone cellPhone, IGangs gangs, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IJurisdictions jurisdictions, ICrimes crimes, IEntityProvideable world, IModItems modItems, IWeapons weapons, INameProvideable names, IShopMenus shopMenus, IAgencies agencies)
    {
        Gang myGang = gangs.GetAllGangs().FirstOrDefault(x => x.ContactName == Name);
        if (myGang == null)
        {
            return;
        }
        MenuInteraction = new GangInteraction(player, gangs, placesOfInterest, this, world, settings, agencies, modItems);
        MenuInteraction.Start(this);
    }

}

