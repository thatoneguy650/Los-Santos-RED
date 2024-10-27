using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Serializable]
public class AppliedOverlay
{
    private uint CollectionHash;
    private uint OverlayHash;
    public AppliedOverlay()
    {

    }
    public AppliedOverlay(string collectionName, string overlayName, string zoneName)
    {
        CollectionName = collectionName;
        OverlayName = overlayName;
        ZoneName = zoneName;
    }
    public AppliedOverlay(string collectionName, string overlayName, string zoneName, uint collectionHash, uint overlayHash)
    {
        CollectionName = collectionName;
        OverlayName = overlayName;
        ZoneName = zoneName;
        CollectionHash = collectionHash;
        OverlayHash = overlayHash;
    }
    public string CollectionName { get; set; }
    public string OverlayName { get; set; }
    public string ZoneName { get; set; }
    public void Apply(Ped ped)
    {
        if(!ped.Exists())
        {
            return;
        }
        NativeFunction.Natives.ADD_PED_DECORATION_FROM_HASHES(ped, CollectionHash, OverlayHash);
    }
    public void Setup()
    {
        CollectionHash = Game.GetHashKey(CollectionName);
        OverlayHash = Game.GetHashKey(OverlayName);
    }

}
