using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SavedBusiness
{
    public SavedBusiness()
    {
    }

    public SavedBusiness(string name, bool isOwnedByPlayer)
    {
        Name = name;
        IsOwnedByPlayer = isOwnedByPlayer;
    }

    public string Name { get; set; }
    public Vector3 Position { get; set; }
    public bool IsOwnedByPlayer { get; set; } = false;
    public DateTime PayoutDate { get; set; }
    public DateTime DateOfLastPayout { get; set; }
    public bool IsPayoutInModItems { get; set; } = false;
    public string ModItemToPayout { get; set; }
    public Vector3 EntrancePosition { get; set; }
    public List<StoredWeapon> WeaponInventory { get; set; } = new List<StoredWeapon>();
    public List<InventorySave> InventoryItems { get; set; } = new List<InventorySave>();

    public int StoredCash { get; set; }

}

