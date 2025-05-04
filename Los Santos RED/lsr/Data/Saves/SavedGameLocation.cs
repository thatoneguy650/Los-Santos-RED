using Rage;

public class SavedGameLocation
{
    public SavedGameLocation()
    {
    }

    public SavedGameLocation(string name, bool isOwnedByPlayer)
    {
        Name = name;
        IsOwnedByPlayer = isOwnedByPlayer;
    }
    public virtual string Name { get; set; }
    public virtual bool IsOwnedByPlayer { get; set; } = false;
    public virtual int CurrentSalesPrice { get; set; }
    public virtual Vector3 EntrancePosition { get; set; }
}
