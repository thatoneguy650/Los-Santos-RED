using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class YanktonAiport : Airport
{
    public YanktonAiport(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
    }

    public YanktonAiport()
    {

    }

    public override void OnDepart()
    {
        base.OnDepart();
        NativeFunction.Natives.SET_MINIMAP_IN_PROLOGUE(false);
        NativeFunction.Natives.SET_ALLOW_STREAM_PROLOGUE_NODES(false);
        NativeFunction.Natives.SET_ROADS_IN_ANGLED_AREA(5526.24f, -5137.23f, 61.78925f, 3679.327f, -4973.879f, 125.0828f, 192.0f, 0, 0, 1);
        NativeFunction.Natives.SET_ROADS_IN_ANGLED_AREA(3691.211f, -4941.24f, 94.59368f, 3511.115f, -4689.191f, 126.7621f, 16.0f, 0, 0, 1);
        NativeFunction.Natives.SET_ROADS_IN_ANGLED_AREA(3510.004f, -4865.81f, 94.69557f, 3204.424f, -4833.8147f, 126.8152f, 16.0f, 0, 0, 1);
        NativeFunction.Natives.SET_ROADS_IN_ANGLED_AREA(3186.534f, -4832.798f, 109.8148f, 3204.187f, -4833.993f, 114.815f, 16.0f, 0, 0, 1);
    }
    public override void OnArrive()
    {
        base.OnArrive();
        NativeFunction.Natives.SET_MINIMAP_IN_PROLOGUE(true);
        NativeFunction.Natives.SET_ALLOW_STREAM_PROLOGUE_NODES(true);
        NativeFunction.Natives.SET_ROADS_IN_ANGLED_AREA(5526.24f, -5137.23f, 61.78925f, 3679.327f, -4973.879f, 125.0828f, 192.0f, 0, 1, 1);
        NativeFunction.Natives.SET_ROADS_IN_ANGLED_AREA(3691.211f, -4941.24f, 94.59368f, 3511.115f, -4689.191f, 126.7621f, 16.0f, 0, 1, 1);
        NativeFunction.Natives.SET_ROADS_IN_ANGLED_AREA(3510.004f, -4865.81f, 94.69557f, 3204.424f, -4833.8147f, 126.8152f, 16.0f, 0, 1, 1);
        NativeFunction.Natives.SET_ROADS_IN_ANGLED_AREA(3186.534f, -4832.798f, 109.8148f, 3204.187f, -4833.993f, 114.815f, 16.0f, 0, 1, 1);
    }
}

