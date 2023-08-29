using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CorruptCopContact : PhoneContact
{
    public CorruptCopContact()
    {

    }

    public CorruptCopContact(string name) : base(name)
    {

    }

    public override void OnAnswered(IContactInteractable player, CellPhone cellPhone, IGangs gangs, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IJurisdictions jurisdictions, ICrimes crimes, IEntityProvideable world, IModItems modItems, IWeapons weapons, INameProvideable names, IShopMenus shopMenus)
    {
        MenuInteraction = new CorruptCopInteraction(player, gangs, placesOfInterest, settings);
        MenuInteraction.Start(this);       
    }

}

