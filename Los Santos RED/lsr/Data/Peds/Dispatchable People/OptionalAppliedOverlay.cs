using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
[Serializable]
public class OptionalAppliedOverlay
{
    private uint CollectionHash;
    private uint OverlayHash;
    public OptionalAppliedOverlay()
    {

    }
    public OptionalAppliedOverlay(string collectionName, string overlayName, string zoneName)
    {
        CollectionName = collectionName;
        OverlayName = overlayName;
        ZoneName = zoneName;
    }
    public OptionalAppliedOverlay(string collectionName, string overlayName, string zoneName, string subZoneName)
    {
        CollectionName = collectionName;
        OverlayName = overlayName;
        ZoneName = zoneName;
        SubZoneName = subZoneName;
    }
    public string CollectionName { get; set; }
    public string OverlayName { get; set; }
    public string ZoneName { get; set; }
    public string SubZoneName { get; set; }
    // public float Percentage { get; set; } = 20f;
    public bool Apply(Ped ped, PedVariation variationToSet)
    {
        if (!ped.Exists())
        {
            return false;
        }
        if (variationToSet == null)
        {
            return false;
        }
        //if(!RandomItems.RandomPercent(Percentage))
        //{
        //    return false;
        //}
        NativeFunction.Natives.ADD_PED_DECORATION_FROM_HASHES(ped, CollectionHash, OverlayHash);
        variationToSet.AppliedOverlays.Add(new AppliedOverlay(CollectionName, OverlayName, ZoneName, CollectionHash, OverlayHash));
        return true;
    }
    public void Setup()
    {
        CollectionHash = Game.GetHashKey(CollectionName);
        OverlayHash = Game.GetHashKey(OverlayName);
    }
}

