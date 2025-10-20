using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Linq;
using System.Xml.Serialization;

[XmlInclude(typeof(SavedResidence))]
[XmlInclude(typeof(SavedBusiness))]
public class SavedGameLocation
{
    public SavedGameLocation()
    {
    }

    public SavedGameLocation(string name, bool isOwnedByPlayer)
    {
        Name = name;
    }
    public virtual string Name { get; set; }
    public virtual int CurrentSalesPrice { get; set; }
    public virtual Vector3 EntrancePosition { get; set; }
    public DateTime PayoutDate { get; set; }
    public DateTime DateOfLastPayout { get; set; }
    public virtual void LoadSavedData(IInventoryable player, IPlacesOfInterest placesOfInterest, IModItems modItems, ISettingsProvideable settings, IEntityProvideable world)
    {
        GameLocation savedPlace = placesOfInterest.AllLocations().Where(x => x.Name == Name && x.EntrancePosition == EntrancePosition && x.IsCorrectMap(world.IsMPMapLoaded)).FirstOrDefault();
        if (savedPlace != null)
        {
            player.Properties.AddOwnedLocation(savedPlace);
            savedPlace.IsOwned = true;
            savedPlace.DatePayoutDue = PayoutDate;
            savedPlace.DatePayoutPaid = DateOfLastPayout;
            savedPlace.CurrentSalesPrice = CurrentSalesPrice;
        }
    }
}
