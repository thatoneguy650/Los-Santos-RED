using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class AngledRestrictedArea
{
    public AngledRestrictedArea()
    {
    }

    public AngledRestrictedArea(Vector3 vecCoors1, Vector3 vecCoors2, float areaWidth)
    {
        VecCoors1 = vecCoors1;
        VecCoors2 = vecCoors2;
        AreaWidth = areaWidth;
    }
    public Vector3 VecCoors1 { get; set; }
    public Vector3 VecCoors2 { get; set; }
    public float AreaWidth { get; set; }
    public bool HighlightArea { get; set; } = false;
    public bool Check3D { get; set; } = true;
    public bool CheckInside(Vector3 pointToCheck)
    {
        return NativeFunction.Natives.IS_POINT_IN_ANGLED_AREA<bool>(pointToCheck.X, pointToCheck.Y, pointToCheck.Z, VecCoors1.X, VecCoors1.Y, VecCoors1.Z, VecCoors2.X, VecCoors2.Y, VecCoors2.Z, AreaWidth, HighlightArea, Check3D);
    }
}

