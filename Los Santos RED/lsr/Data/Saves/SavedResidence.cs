using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SavedResidence
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

    public string Name { get; set; }
    public bool IsOwnedByPlayer { get; set; } = false;
    public bool IsRentedByPlayer { get; set; } = false;
    public DateTime RentalPaymentDate { get; set; }
    public DateTime DateOfLastRentalPayment { get; set; }

    public List<StoredWeapon> WeaponInventory { get; set; } = new List<StoredWeapon>();
    public List<InventorySave> InventoryItems { get; set; } = new List<InventorySave>();

    public int StoredCash { get; set; }


}

