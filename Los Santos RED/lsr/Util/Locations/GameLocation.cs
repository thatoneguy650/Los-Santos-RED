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
    public Vector3 VendorPosition { get; set; } = Vector3.Zero;
    public float VendorHeading { get; set; } = 0f;
    public Vector3 EntrancePosition { get; set; }
    public float EntranceHeading { get; set; }
    public LocationType Type { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    //public List<Vector3> GasPumps { get; set; } = new List<Vector3>();
    public List<ConsumableSubstance> SellableItems { get; set; } = new List<ConsumableSubstance>();
    public GameLocation()
    {

    }
    public GameLocation(Vector3 _EntrancePosition, float _EntranceHeading, Vector3 _VendorPosition, float _VendorHeading, LocationType _Type, string _Name, string _Description)
    {
        EntrancePosition = _EntrancePosition;
        EntranceHeading = _EntranceHeading;
        VendorPosition = _VendorPosition;
        VendorHeading = _VendorHeading;
        Type = _Type;
        Name = _Name;
        Description = _Description;
    }
    public GameLocation(Vector3 _EntrancePosition, float _EntranceHeading, LocationType _Type, string _Name, string _Description)
    {
        EntrancePosition = _EntrancePosition;
        EntranceHeading = _EntranceHeading;
        Type = _Type;
        Name = _Name;
        Description = _Description;
    }
    public override string ToString()
    {
        return Name.ToString();
    }
}
