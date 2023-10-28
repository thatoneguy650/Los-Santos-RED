using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VehicleExporterContact : PhoneContact, IPhoneContact
{
    public VehicleExporterContact()
    {
    }

    public VehicleExporterContact(string name) : base(name)
    {
    }

    public override void OnAnswered(IContactInteractable player, CellPhone cellPhone, IGangs gangs, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IJurisdictions jurisdictions,
        ICrimes crimes, IEntityProvideable world, IModItems modItems, IWeapons weapons, INameProvideable names, IShopMenus shopMenus, IAgencies agencies)
    {
        MenuInteraction = new VehicleExporterInteraction(player, gangs, placesOfInterest, settings, modItems, this);
        MenuInteraction.Start(this);
    }
}

