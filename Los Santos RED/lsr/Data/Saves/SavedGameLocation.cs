using LosSantosRED.lsr.Interface;
using Rage;
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
    public bool IsOnSPMap { get; set; } = true;
    public bool IsOnMPMap { get; set; } = true;
    public virtual void LoadSavedData(IInventoryable player, IPlacesOfInterest placesOfInterest, IModItems modItems, ISettingsProvideable settings)
    {
        GameLocation savedPlace = placesOfInterest.AllLocations().Where(x => x.Name == Name && x.EntrancePosition == EntrancePosition && x.IsOnMPMap == IsOnMPMap && x.IsOnSPMap == IsOnSPMap).FirstOrDefault();
        if (savedPlace != null)
        {
            player.Properties.AddOwnedLocation(savedPlace);
            savedPlace.CurrentSalesPrice = CurrentSalesPrice;
        }
    }
}
