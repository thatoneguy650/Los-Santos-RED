using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SavedPayoutProperty
{
    public SavedPayoutProperty()
    {
    }

    public SavedPayoutProperty(string name, bool isOwnedByPlayer)
    {
        Name = name;
        IsOwnedByPlayer = isOwnedByPlayer;
    }
    public string Name { get; set; }
    public bool IsOwnedByPlayer { get; set; } = false;
    public DateTime PayoutDate { get; set; }
    public DateTime DateOfLastPayout { get; set; }
    public Vector3 EntrancePosition { get; set; }
    public int CurrentSalesPrice { get; set; }

}

