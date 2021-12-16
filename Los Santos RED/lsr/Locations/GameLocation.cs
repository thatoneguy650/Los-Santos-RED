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
    private float distanceToPlayer = 999f;
    private Blip createdBlip;
    public Vector3 VendorPosition { get; set; } = Vector3.Zero;
    public float VendorHeading { get; set; } = 0f;
    public Vector3 EntrancePosition { get; set; } = Vector3.Zero;
    public float EntranceHeading { get; set; }
    public LocationType Type { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int CellX { get; set; }
    public int CellY { get; set; }
    public List<MenuItem> Menu { get; set; } = new List<MenuItem>();
    public bool HasVendor => VendorPosition != Vector3.Zero;
    public int InteriorID { get; set; } = -1;
    public bool HasInterior => InteriorID != -1;
    public bool ShouldAlwaysHaveBlip => false;//Type == LocationType.Police || Type == LocationType.Hospital;
    public bool IsStore => Type != LocationType.Police && Type != LocationType.Hospital && Type != LocationType.Grave && Type != LocationType.FireStation;
    public Blip CreatedBlip => createdBlip;
    public bool CanTransact => Menu.Any();
    public bool Is247 => CloseTime >= 24;
    public int OpenTime { get; set; } = 6;
    public int CloseTime { get; set; } = 20;
    public bool HasCustomCamera => CameraPosition != Vector3.Zero;
    public bool HasCustomItemPostion => ItemPreviewPosition != Vector3.Zero;
    public bool HasTeleportEnter => TeleportEnterPosition != Vector3.Zero;
    public Vector3 CameraPosition { get; set; } = Vector3.Zero;
    public Vector3 CameraDirection { get; set; } = Vector3.Zero;
    public Rotator CameraRotation { get; set; }
    public Vector3 ItemPreviewPosition { get; set; } = Vector3.Zero;
    public Vector3 ItemDeliveryPosition { get; set; } = Vector3.Zero;
    public Vector3 TeleportEnterPosition { get; set; } = Vector3.Zero;
    public float TeleportEnterHeading { get; set; } = 0f;
    public string BannerImage { get; set; } = "";
    public float ItemPreviewHeading { get; set; } = 0f;
    public float ItemDeliveryHeading { get; set; } = 0f;
    public List<string> VendorModels { get; set; } = new List<string>() { "s_m_m_strvend_01", "s_m_m_linecook" };
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
        CellX = (int)(VendorPosition.X / EntryPoint.CellSize);
        CellY = (int)(VendorPosition.Y / EntryPoint.CellSize);
    }
    public GameLocation(Vector3 _EntrancePosition, float _EntranceHeading, LocationType _Type, string _Name, string _Description)
    {
        EntrancePosition = _EntrancePosition;
        EntranceHeading = _EntranceHeading;
        Type = _Type;
        Name = _Name;
        Description = _Description;
        CellX = (int)(EntrancePosition.X / EntryPoint.CellSize);
        CellY = (int)(EntrancePosition.Y / EntryPoint.CellSize);
    }
    public override string ToString()
    {
        return Name.ToString();
    }
    public bool IsOpen(int currentHour)
    {
        return (CloseTime == 24 && OpenTime == 0) || (currentHour >= OpenTime && currentHour <= CloseTime);
    }
    private uint GameTimeLastCheckedDistance;
    private uint UpdateIntervalTime
    {
        get
        {
            if (DistanceToPlayer >= 999f)
            {
                return 10000;
            }
            else if (DistanceToPlayer >= 500)
            {
                return 5000;
            }
            else if (DistanceToPlayer >= 200)
            {
                return 2000;
            }
            else
            {
                return 1000;
            }
        }
    }
    public float DistanceToPlayer => distanceToPlayer;
    public bool IsWalkup { get; set; } = false;
    public void Update()
    {
        if (GameTimeLastCheckedDistance == 0 || Game.GameTime - GameTimeLastCheckedDistance >= UpdateIntervalTime)
        {
            distanceToPlayer = EntrancePosition.DistanceTo2D(Game.LocalPlayer.Character);
            GameTimeLastCheckedDistance = Game.GameTime;
        }
    }

}

