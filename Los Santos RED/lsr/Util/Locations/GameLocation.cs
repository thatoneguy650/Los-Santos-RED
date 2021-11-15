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
    public MerchantType MerchantType = MerchantType.None;
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
    public override string ToString()
    {
        return Name.ToString();
    }
}
