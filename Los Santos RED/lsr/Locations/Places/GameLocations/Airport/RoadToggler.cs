using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

public class RoadToggler
{
    private bool IsOn = false;
    public Vector3 Vector1 { get; set; }
    public Vector3 Vector2 { get; set; }
    public float AreaWidth { get; set; }
    public bool HighlightArea { get; set; } = false;
    public bool Network { get; set; } = true;
    public RoadToggler()
    {
        
    }

    public RoadToggler(Vector3 vector1, Vector3 vector2, float areaWidth)
    {
        Vector1 = vector1;
        Vector2 = vector2;
        AreaWidth = areaWidth;
    }

    public void SetRoad(bool enable)
    {
        NativeFunction.Natives.SET_ROADS_IN_ANGLED_AREA(Vector1.X, Vector1.Y, Vector1.Z, Vector2.X, Vector2.Y, Vector2.Z, AreaWidth, HighlightArea, enable, Network);
    }
}

