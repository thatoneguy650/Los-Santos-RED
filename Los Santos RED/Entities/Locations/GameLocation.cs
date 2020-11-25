using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable()]
public class GameLocation
{
    public Vector3 LocationPosition;
    public float Heading;
    public LocationType Type;
    public string Name;
    public List<Vector3> GasPumps = new List<Vector3>();

    public GameLocation()
    {

    }
    public GameLocation(Vector3 _LocationPosition, float _Heading, LocationType _Type, String _Name)
    {
        LocationPosition = _LocationPosition;
        Heading = _Heading;
        Type = _Type;
        Name = _Name;
    }
    public GameLocation(Vector3 _LocationPosition, float _Heading, LocationType _Type, String _Name, List<Vector3> _GasPumps)
    {
        LocationPosition = _LocationPosition;
        Heading = _Heading;
        Type = _Type;
        Name = _Name;
        GasPumps = _GasPumps;
    }
    public void CreateLocationBlip()
    {
        Blip MyLocationBlip = new Blip(LocationPosition)
        {
            Name = Name
        };

        if (Type == LocationType.Hospital)
        {
            MyLocationBlip.Sprite = BlipSprite.Hospital;
            MyLocationBlip.Color = Color.White;
        }
        else if (Type == LocationType.Police)
        {
            MyLocationBlip.Sprite = BlipSprite.PoliceStation;
            MyLocationBlip.Color = Color.White;
        }
        else if (Type == LocationType.ConvenienceStore)
        {
            MyLocationBlip.Sprite = BlipSprite.CriminalHoldups;
            MyLocationBlip.Color = Color.White;
        }
        else if (Type == LocationType.GasStation)
        {
            MyLocationBlip.Sprite = BlipSprite.JerryCan;
            MyLocationBlip.Color = Color.White;
        }

        NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)MyLocationBlip.Handle, true);
        General.CreatedBlips.Add(MyLocationBlip);
    }
    public override string ToString()
    {
        return Name.ToString();
    }
}
