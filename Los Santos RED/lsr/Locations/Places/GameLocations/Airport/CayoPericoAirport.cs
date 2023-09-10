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
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Heist_Island_Peds", false);
        NativeFunction.Natives.SET_AMBIENT_ZONE_STATE_PERSISTENT("AZL_DLC_Hei4_Island_Zones", false, false);
        NativeFunction.Natives.SET_AMBIENT_ZONE_STATE_PERSISTENT("AZL_DLC_Hei4_Island_Disabled_Zones", true, false);
        NativeFunction.Natives.SET_ALLOW_STREAM_HEIST_ISLAND_NODES(false);
        base.OnDepart();
    }
    public override void OnArrive(bool setPos)
    {
        NativeFunction.Natives.SET_ISLAND_ENABLED("HeistIsland", true);
        NativeFunction.Natives.SET_USE_ISLAND_MAP(true);
        NativeFunction.Natives.SET_ALLOW_STREAM_HEIST_ISLAND_NODES(true);
        NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Heist_Island_Peds", true);
        NativeFunction.Natives.SET_AMBIENT_ZONE_STATE_PERSISTENT("AZL_DLC_Hei4_Island_Zones", true, true);
        NativeFunction.Natives.SET_AMBIENT_ZONE_STATE_PERSISTENT("AZL_DLC_Hei4_Island_Disabled_Zones", false, true);
        NativeFunction.Natives.SET_ALLOW_STREAM_HEIST_ISLAND_NODES(true);
        base.OnArrive(setPos);
    }
}

