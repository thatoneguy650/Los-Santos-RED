using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CayoPericoAirport : Airport
{
    public CayoPericoAirport(string airportID, Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(airportID, _EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public CayoPericoAirport()
    {

    }

    public override void OnDepart()
    {
        NativeFunction.Natives.SET_ISLAND_ENABLED("HeistIsland", false);
        NativeFunction.Natives.SET_USE_ISLAND_MAP(false);
        NativeFunction.Natives.SET_ALLOW_STREAM_HEIST_ISLAND_NODES(false);
        base.OnDepart();
    }
    public override void OnArrive()
    {
        NativeFunction.Natives.SET_ISLAND_ENABLED("HeistIsland", true);
        NativeFunction.Natives.SET_USE_ISLAND_MAP(true);
        NativeFunction.Natives.SET_ALLOW_STREAM_HEIST_ISLAND_NODES(true);
        base.OnArrive();
    }
}

