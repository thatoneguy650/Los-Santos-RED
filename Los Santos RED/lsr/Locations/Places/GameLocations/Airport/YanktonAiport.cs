using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class YanktonAiport : Airport
{
    private TaxiDropOff TaxiDropOff;
    public YanktonAiport(string airportID, Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(airportID, _EntrancePosition, _EntranceHeading, _Name, _Description)
    {
    }

    public YanktonAiport()
    {

    }

    public override void OnDepart()
    {
        NativeFunction.Natives.SET_MINIMAP_IN_PROLOGUE(false);
        NativeFunction.Natives.SET_ALLOW_STREAM_PROLOGUE_NODES(false);
        base.OnDepart();
    }
    public override void OnArrive(bool setPos)
    {
        NativeFunction.Natives.SET_MINIMAP_IN_PROLOGUE(true);
        NativeFunction.Natives.SET_ALLOW_STREAM_PROLOGUE_NODES(true);
        base.OnArrive(setPos);

        if (setPos)
        {
            TaxiDropOff = new TaxiDropOff(ArrivalPosition, Settings, Crimes, Weapons, Names, World, ModItems, null);
            TaxiDropOff.Setup();
            TaxiDropOff.Start();
        }
    }
}

