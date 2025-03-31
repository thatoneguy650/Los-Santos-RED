using System.Collections.Generic;

public class SavedBusiness : SavedPayoutProperty
{
    public SavedBusiness()
    {
    }

    public SavedBusiness(string name, bool isOwnedByPlayer) : base(name, isOwnedByPlayer)
    {
    }
    public bool IsPayoutInModItems { get; set; } = false;
    public bool IsPayoutDepositedToBank { get; set; }
    public string ModItemToPayout { get; set; }
    public List<StoredWeapon> WeaponInventory { get; set; } = new List<StoredWeapon>();
    public List<InventorySave> InventoryItems { get; set; } = new List<InventorySave>();
    public int StoredCash { get; set; }

}

