using System.Collections.Generic;

public class TattooOverlay
{
    public TattooOverlay(string collectionName, uint collectionHash, string overlayName, uint overlayHash, string gender, string type, string zoneName, string garment, int price, int awardLevel, string customZoneNameDetailed, string updateGroup, string facing)
    {
        CollectionName = collectionName;
        CollectionHash = collectionHash;
        OverlayName = overlayName;
        OverlayHash = overlayHash;
        Gender = gender;
        Type = type;
        ZoneName = zoneName;
        Garment = garment;
        Price = price;
        AwardLevel = awardLevel;
        CustomZoneNameDetailed = customZoneNameDetailed;
        UpdateGroup = updateGroup;
        Facing = facing;
    }

    public string CollectionName { get; set; }
    public uint CollectionHash { get; set; }
    public string OverlayName { get; set; }
    public uint OverlayHash { get; set; }
    public string Gender { get; set; }
    public string Type { get; set; }
    public string ZoneName { get; set; }
    public string Garment { get; set; }
    public int Price { get; set; }
    public int AwardLevel { get; set; }
    public string CustomZoneNameDetailed { get; set; }
    public string UpdateGroup { get; set; }
    public string Facing { get; set; }
    public override string ToString()
    {
        return $"{OverlayName} {(IsApplied ? " - Applied" :"")}";
    }
    public bool IsApplied { get; set; } = false;
}

public class TattooRoot
{

    public List<TattooOverlay> Overlays { get; set; }
}