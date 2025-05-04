using Rage;
using System;

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
}

