using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


public class SavedResidence : SavedGameLocation
{
    public SavedResidence()
    {
    }

    public SavedResidence(string name, bool isOwnedByPlayer, bool isRentedByPlayer)
    {
        Name = name;
        IsOwnedByPlayer = isOwnedByPlayer;
        IsRentedByPlayer = isRentedByPlayer;
    }

    public bool IsRentedByPlayer { get; set; } = false;
    public bool IsOwnedByPlayer { get; set; }
    public DateTime RentalPaymentDate { get; set; }
    public DateTime DateOfLastRentalPayment { get; set; }
    public List<StoredWeapon> WeaponInventory { get; set; } = new List<StoredWeapon>();
    public List<InventorySave> InventoryItems { get; set; } = new List<InventorySave>();

    public int StoredCash { get; set; }
    [XmlArray("TrophyPlacements")]
    [XmlArrayItem("TrophyPlacement")]
    public List<TrophyPlacement> PlacedTrophies { get; set; } = new List<TrophyPlacement>();

    public override void LoadSavedData(IInventoryable player, IPlacesOfInterest placesOfInterest, IModItems modItems, ISettingsProvideable settings, IEntityProvideable world)
    {
        if (IsOwnedByPlayer || IsRentedByPlayer)
        {
            Residence savedPlace = placesOfInterest.PossibleLocations.Residences.Where(x => x.Name == Name && x.IsCorrectMap(world.IsMPMapLoaded)).FirstOrDefault();
            if (savedPlace != null)
            {
                player.Properties.AddOwnedLocation(savedPlace);
                savedPlace.IsOwned = IsOwnedByPlayer;
                savedPlace.IsRented = IsRentedByPlayer;
                savedPlace.DateRentalPaymentDue = RentalPaymentDate;
                savedPlace.DateRentalPaymentPaid = DateOfLastRentalPayment;
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
                if (savedPlace.ResidenceInterior != null)
                {
                    ResidenceInterior interior = savedPlace.ResidenceInterior;

                    // Clear previous trophies/props
                    interior.RemoveSpawnedProps();
                    interior.PlacedTrophies.Clear();
                    interior.SavedPlacedTrophies.Clear();

                    // Set interior references
                    interior.SetResidence(savedPlace);

                    // Load trophies from save
                    interior.SavedPlacedTrophies = PlacedTrophies.ToList();
                    foreach (TrophyPlacement tp in interior.SavedPlacedTrophies)
                        interior.PlacedTrophies[tp.SlotID] = tp.TrophyID;

                    // Load interior and spawn trophies asynchronously
                    interior.Load(true);

                }
                savedPlace.RefreshUI();
            }
        }
    }


}

