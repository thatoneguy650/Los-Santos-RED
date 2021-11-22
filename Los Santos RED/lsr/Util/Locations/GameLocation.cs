using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Util.Locations;
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
    private Blip createdBlip;
    public Vector3 VendorPosition { get; set; } = Vector3.Zero;
    public float VendorHeading { get; set; } = 0f;
    public Vector3 EntrancePosition { get; set; } = Vector3.Zero;
    public float EntranceHeading { get; set; }
    public LocationType Type { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<ConsumableSubstance> SellableItems { get; set; } = new List<ConsumableSubstance>();
    public bool HasVendor => VendorPosition != Vector3.Zero;
    public bool ShouldAlwaysHaveBlip => Type == LocationType.Police || Type == LocationType.Hospital;
    public Blip CreatedBlip => createdBlip;
    public bool CanPurchase => SellableItems.Any();
    public bool Is247 => CloseTime >= 24;
    public int OpenTime { get; set; } = 6;
    public int CloseTime { get; set; } = 20;
    public void SetCreatedBlip(Blip toset)
    {
        createdBlip = toset;
    }
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
    public bool IsOpen(int currentHour)
    {
        return CloseTime == 24 || (currentHour >= OpenTime && currentHour <= CloseTime);
    }

}

