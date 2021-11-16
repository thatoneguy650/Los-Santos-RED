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
    public Vector3 LocationPosition { get; set; }
    public float Heading { get; set; }
    public LocationType Type { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Vector3> GasPumps { get; set; } = new List<Vector3>();
    public List<ConsumableSubstance> SellableItems { get; set; } = new List<ConsumableSubstance>();
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
    public GameLocation(Vector3 _LocationPosition, float _Heading, LocationType _Type, String _Name, string _Description)
    {
        LocationPosition = _LocationPosition;
        Heading = _Heading;
        Type = _Type;
        Name = _Name;
        Description = _Description;
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
