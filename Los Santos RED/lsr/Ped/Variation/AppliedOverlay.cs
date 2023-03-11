using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AppliedOverlay
{
    public AppliedOverlay()
    {
    }

    public AppliedOverlay(string collectionName, string overlayName, string zoneName)
    {
        CollectionName = collectionName;
        OverlayName = overlayName;
        ZoneName = zoneName;
    }

    public string CollectionName { get; set; }
    public string OverlayName { get; set; }
    public string ZoneName { get; set; }
}
