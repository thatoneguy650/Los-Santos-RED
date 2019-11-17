
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GTALocation
{
    public enum LocationType : int
    {
        Police = 0,
        Hospital = 1,
        ConvenienceStore = 2,
    }
    public Vector3 Location;
    public float Heading;
    public LocationType Type;
    public String Name;
    public Blip LocationBlip;
    public GTALocation()
    {

    }
    public GTALocation(Vector3 _Location,float _Heading, LocationType _Type,String _Name)
    {
        Location = _Location;
        Heading = _Heading;
        Type = _Type;
        Name = _Name;

        LocationBlip = new Blip(Location);
        LocationBlip.Name = _Name;

        if (Type == LocationType.Hospital)
        {
            LocationBlip.Sprite = BlipSprite.Hospital;
            LocationBlip.Color = Color.White;
        }
        else if (Type == LocationType.Police)
        {
            LocationBlip.Sprite = BlipSprite.PoliceStation;
            LocationBlip.Color = Color.White;
        }
        else if (Type == LocationType.ConvenienceStore)
        {
            LocationBlip.Sprite = BlipSprite.CriminalHoldups;
            LocationBlip.Color = Color.White;
        }

        NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)LocationBlip.Handle, true);
        InstantAction.CreatedBlips.Add(LocationBlip);
    }
}

