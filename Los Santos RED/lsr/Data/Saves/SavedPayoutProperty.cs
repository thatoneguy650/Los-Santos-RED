using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Linq;

public class SavedPayoutProperty : SavedGameLocation
{
    public SavedPayoutProperty()
    {
    }

    public SavedPayoutProperty(string name, bool isOwnedByPlayer)
    {
        Name = name;
        IsOwnedByPlayer = isOwnedByPlayer;
    }
    public DateTime PayoutDate { get; set; }
    public DateTime DateOfLastPayout { get; set; }
    public override void LoadSavedData(IInventoryable player, IPlacesOfInterest placesOfInterest, IModItems modItems, ISettingsProvideable settings)
    {
        if (IsOwnedByPlayer)
        {
            GameLocation savedPlace = placesOfInterest.AllLocations().Where(x => x.Name == Name && x.EntrancePosition == EntrancePosition).FirstOrDefault();
            if (savedPlace != null)
            {
                player.Properties.AddOwnedLocation(savedPlace);
                savedPlace.IsOwned = IsOwnedByPlayer;
                savedPlace.DatePayoutDue = PayoutDate;
                savedPlace.DatePayoutPaid = DateOfLastPayout;
                savedPlace.CurrentSalesPrice = CurrentSalesPrice;
            }
        }
    }
}

