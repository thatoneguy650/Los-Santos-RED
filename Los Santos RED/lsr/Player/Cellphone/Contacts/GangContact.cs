using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GangContact : PhoneContact
{
    public GangContact()
    {

    }

    public GangContact(string name, string iconName) : base(name, iconName)
    {

    }
    public override void OnAnswered(IContactInteractable player, CellPhone cellPhone, IGangs gangs, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IJurisdictions jurisdictions, ICrimes crimes, IEntityProvideable world)
    {
        Gang myGang = gangs.GetAllGangs().FirstOrDefault(x => x.ContactName == Name);
        if (myGang != null)
        {
            MenuInteraction = new GangInteraction(player, gangs, placesOfInterest, this);
            MenuInteraction.Start(this);
        }
        else
        {
            EntryPoint.WriteToConsole("NO GANG FOUND :(");
        }
    }

}

