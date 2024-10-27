using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class AppliedOverlayZonePercentage
{
    public AppliedOverlayZonePercentage()
    {
    }

    public AppliedOverlayZonePercentage(string zoneName, float percentage, int limit)
    {
        ZoneName = zoneName;
        Percentage = percentage;
        Limit = limit;
    }

    public string ZoneName { get; set; }
    public float Percentage { get; set; }
    public int Limit { get; set; } = 1;
}

