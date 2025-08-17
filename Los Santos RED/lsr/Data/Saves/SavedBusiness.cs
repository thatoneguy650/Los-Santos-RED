using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;

public class SavedBusiness : SavedGameLocation
{
    public SavedBusiness()
    {
    }

    public SavedBusiness(string name, bool isOwnedByPlayer)
    {
        Name = name;
    }
    public DateTime PayoutDate { get; set; }
    public DateTime DateOfLastPayout { get; set; }
    public string ModItemToPayout { get; set; }
    public List<StoredWeapon> WeaponInventory { get; set; } = new List<StoredWeapon>();
    public List<InventorySave> InventoryItems { get; set; } = new List<InventorySave>();
    public int StoredCash { get; set; }
    public override void LoadSavedData(IInventoryable player, IPlacesOfInterest placesOfInterest, IModItems modItems, ISettingsProvideable settings, IEntityProvideable world)
    {

        Business savedPlace = placesOfInterest.PossibleLocations.Businesses.Where(x => x.Name == Name && x.EntrancePosition == EntrancePosition && x.IsCorrectMap(world.IsMPMapLoaded)).FirstOrDefault();
        if (savedPlace != null)
        {
            player.Properties.AddOwnedLocation(savedPlace);
            savedPlace.IsOwned = true;
            savedPlace.DatePayoutDue = PayoutDate;
            savedPlace.DatePayoutPaid = DateOfLastPayout;
            //savedPlace.IsPayoutInModItems = biz.IsPayoutInModItems;
            savedPlace.ModItemToPayout = ModItemToPayout;
            //savedPlace.IsPayoutDepositedToBank = biz.IsPayoutDepositedToBank;
            savedPlace.CurrentSalesPrice = CurrentSalesPrice;
            if (savedPlace.WeaponStorage == null)
            {
                savedPlace.WeaponStorage = new WeaponStorage(settings);
            }
            if (savedPlace.SimpleInventory == null)
            {
                savedPlace.SimpleInventory = new SimpleInventory(settings);
            }
            foreach (StoredWeapon storedWeap in WeaponInventory)
            {
                savedPlace.WeaponStorage.StoredWeapons.Add(storedWeap.Copy());
            }
            foreach (InventorySave stest in InventoryItems)
            {
                savedPlace.SimpleInventory.Add(modItems.Get(stest.ModItemName), stest.RemainingPercent);
            }
            savedPlace.CashStorage.StoredCash = StoredCash;
            savedPlace.RefreshUI();
        }
    }
}

