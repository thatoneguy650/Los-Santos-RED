using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Util.Locations;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

[Serializable()]
public class GameLocation
{   
    private Blip createdBlip;
    private Merchant merchant;
    private Interior interior;
    private Rage.Object propObject;
    private float distanceToPlayer = 999f;
    private int CellsAway = 99;
    private bool isNearby = false;
    private uint GameTimeLastCheckedDistance;
    private uint GameTimeLastCheckedNearby;
    private uint DistanceUpdateIntervalTime
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
    private uint NearbyUpdateIntervalTime
    {
        get
        {
            if (CellsAway >= 20)
            {
                return 8000;
            }
            else if (CellsAway >= 10)
            {
                return 4000;
            }
            else if (CellsAway >= 6)
            {
                return 2000;
            }
            else
            {
                return 1000;
            }
        }
    }
    public BlipSprite BlipSprite
    {
        get
        {
            if (Type == LocationType.Hospital)
            {
                return BlipSprite.Hospital;
            }
            if (Type == LocationType.FireStation)
            {
                return (BlipSprite)436;
            }
            else if (Type == LocationType.Police)
            {
                return BlipSprite.PoliceStation;
            }
            else if (Type == LocationType.Stadium)
            {
                return (BlipSprite)459;
            }
            else if (Type == LocationType.ConvenienceStore)
            {
                return BlipSprite.CriminalHoldups;
            }
            else if (Type == LocationType.GasStation)
            {
                return BlipSprite.JerryCan;
            }
            else if (Type == LocationType.Grave)
            {
                return BlipSprite.Dead;
            }
            else if (Type == LocationType.Morgue)
            {
                return BlipSprite.Dead;
            }
            else if (Type == LocationType.FoodStand)
            {
                return (BlipSprite)480;//radar_vip
            }
            else if (Type == LocationType.Dispensary)
            {
                return BlipSprite.Stash;
            }
            else if (Type == LocationType.Restaurant)
            {
                return (BlipSprite)621;// 475;//.Bar;(304)(621)
            }
            else if (Type == LocationType.DriveThru)
            {
                return (BlipSprite)523;//.Bar;
            }
            else if (Type == LocationType.LiquorStore)
            {
                return BlipSprite.Bar;
            }
            else if (Type == LocationType.Bank)
            {
                return BlipSprite.Devin;
            }
            else if (Type == LocationType.Bar)
            {
                return BlipSprite.Bar;
            }
            else if (Type == LocationType.Pharmacy)
            {
                return BlipSprite.CriminalDrugs;
            }
            else if (Type == LocationType.Hotel)
            {
                return (BlipSprite)475;
            }
            else if (Type == LocationType.Apartment)
            {
                return BlipSprite.GarageForSale;
            }
            else if (Type == LocationType.GangDen)
            {
                return BlipSprite.Shrink;
            }
            else if (Type == LocationType.House)
            {
                return BlipSprite.GarageForSale;
            }
            else if (Type == LocationType.BeautyShop)
            {
                return BlipSprite.Barber;
            }
            else if (Type == LocationType.ScrapYard)
            {
                return BlipSprite.CriminalCarsteal;
            }
            else if (Type == LocationType.GunShop)
            {
                return BlipSprite.AmmuNation;
            }
            else if (Type == LocationType.HardwareStore)
            {
                return (BlipSprite)566; //BlipSprite.Repair;
            }
            else if (Type == LocationType.Headshop)
            {
                return (BlipSprite)96;// BlipSprite.Stash;
            }
            else if (Type == LocationType.CarDealer)
            {
                return BlipSprite.GangVehicle;
            }
            else if (Type == LocationType.PawnShop)
            {
                return BlipSprite.PointOfInterest;
            }
            else if (Type == LocationType.BusStop)
            {
                return BlipSprite.VinewoodTours;
            }
            else if (Type == LocationType.Brothel)
            {
                return BlipSprite.DropOffHooker;
            }
            else if (Type == LocationType.Yoga)
            {
                return BlipSprite.Yoga;
            }
            else if (Type == LocationType.MassageParlor)
            {
                return (BlipSprite)466;
            }
            else if (Type == LocationType.DrugDealer)
            {
                return BlipSprite.UGCMission;
            }
            else if (Type == LocationType.VendingMachine)
            {
                return BlipSprite.PointOfInterest;
            }
            else
            {
                return BlipSprite.PointOfInterest;
            }
        }
    }

    //Gang Stuff
    public string GangID { get; set; } = "";
    public bool IsEnabled { get; set; } = true;

    //Store
    public string BannerImage { get; set; } = "";
    public bool CanTransact => Menu.Any();
    public bool IsStore => Type != LocationType.Police && Type != LocationType.Hospital && Type != LocationType.Grave && Type != LocationType.FireStation;
    public LocationType Type { get; set; }
    public List<MenuItem> Menu { get; set; } = new List<MenuItem>();

    //Store with Merchant
    public Merchant Merchant => merchant;
    public bool HasVendor => VendorPosition != Vector3.Zero;
    public Vector3 VendorPosition { get; set; } = Vector3.Zero;
    public float VendorHeading { get; set; } = 0f;
    public List<string> VendorModels { get; set; } = new List<string>() { "s_m_m_strvend_01", "s_m_m_linecook" };

    //Car Dealers only?
    public bool HasCustomItemPostion => ItemPreviewPosition != Vector3.Zero;
    public Vector3 ItemPreviewPosition { get; set; } = Vector3.Zero;
    public float ItemPreviewHeading { get; set; } = 0f;
    public Vector3 ItemDeliveryPosition { get; set; } = Vector3.Zero;
    public float ItemDeliveryHeading { get; set; } = 0f;


    //Vending machines only?
    public Rage.Object PropObject => propObject;

    [XmlIgnore]
    public string FullAddressText { get; set; }
    [XmlIgnore]
    public string StreetAddress { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public int CellX { get; set; }
    public int CellY { get; set; }
    public Vector3 EntrancePosition { get; set; } = Vector3.Zero;
    public float EntranceHeading { get; set; }
    public bool HasTeleportEnter => TeleportEnterPosition != Vector3.Zero;
    public Vector3 TeleportEnterPosition { get; set; } = Vector3.Zero;
    public float TeleportEnterHeading { get; set; } = 0f;
    public Blip Blip => createdBlip;
    public bool ShouldAlwaysHaveBlip => false;
    public bool IsBlipEnabled { get; set; } = true;
    public bool Is247 => CloseTime >= 24;
    public int OpenTime { get; set; } = 6;
    public int CloseTime { get; set; } = 20;
    public bool HasInterior => InteriorID != -1;
    public int InteriorID { get; set; } = -1;
    public Interior Interior => interior;
    public bool HasCustomCamera => CameraPosition != Vector3.Zero;
    public Vector3 CameraPosition { get; set; } = Vector3.Zero;
    public Vector3 CameraDirection { get; set; } = Vector3.Zero;
    public Rotator CameraRotation { get; set; }
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
    public GameLocation(Vector3 _EntrancePosition, float _EntranceHeading, LocationType _Type, string _Name, string _Description, Rage.Object prop)
    {
        EntrancePosition = _EntrancePosition;
        EntranceHeading = _EntranceHeading;
        Type = _Type;
        Name = _Name;
        Description = _Description;
        propObject = prop;
        CellX = (int)(EntrancePosition.X / EntryPoint.CellSize);
        CellY = (int)(EntrancePosition.Y / EntryPoint.CellSize);
    }
    public float DistanceToPlayer => distanceToPlayer;
    public bool IsWalkup { get; set; } = false;
    public bool IsPurchaseable { get; set; } = false;
    public bool IsPurchased { get; set; } = false;
    public override string ToString()
    {
        return Name.ToString();
    }
    public bool IsOpen(int currentHour)
    {
        return (CloseTime == 24 && OpenTime == 0) || (currentHour >= OpenTime && currentHour <= CloseTime);
    }
    public void Setup(IInteriors interiors, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons)
    {
        if (HasInterior)
        {
            interior = interiors.GetInterior(InteriorID);
            if (interior != null)
            {
                interior.Load();
            }
        }
        if (HasVendor)
        {
            SpawnVendor(settings, crimes, weapons);
            GameFiber.Yield();
        }
        if (!ShouldAlwaysHaveBlip && IsBlipEnabled)
        {
            createdBlip = new MapBlip(EntrancePosition, Name, BlipSprite).AddToMap();
            GameFiber.Yield();
        }
        SetNearby();
        Update();
    }
    public void Update()
    {
        if (isNearby)
        {
            if (GameTimeLastCheckedDistance == 0 || Game.GameTime - GameTimeLastCheckedDistance >= DistanceUpdateIntervalTime)
            {
                distanceToPlayer = EntrancePosition.DistanceTo2D(Game.LocalPlayer.Character);
                GameTimeLastCheckedDistance = Game.GameTime;
            }
        }
        else
        {
            distanceToPlayer = 999f;
            GameTimeLastCheckedDistance = Game.GameTime;
        }
    }
    public void Dispose()
    {
        if(createdBlip.Exists())
        {
            createdBlip.Delete();
        }
        if(Merchant != null && Merchant.Pedestrian.Exists())
        {
            Merchant.Pedestrian.Delete();
        }
        if (interior != null)
        {
            interior.Unload();
        }
    }
    public bool IsNearby(int cellX, int cellY, int Distance)
    {
        if (GameTimeLastCheckedNearby == 0 || Game.GameTime - GameTimeLastCheckedNearby >= NearbyUpdateIntervalTime)
        {
            CellsAway = NativeHelper.MaxCellsAway(cellX, cellY, CellX, CellY);
            if(CellsAway <= Distance)
            {
                isNearby = true;
            }
            else
            {
                isNearby = false;
            }
            GameTimeLastCheckedNearby = Game.GameTime;
        }
        return isNearby;
    }
    public void SetNearby()
    {
        isNearby = true;
    }
    private void SpawnVendor(ISettingsProvideable settings, ICrimes crimes, IWeapons weapons)
    {
        Ped ped;
        string ModelName = VendorModels.PickRandom();
        if (RandomItems.RandomPercent(30))
        {
            Model modelToCreate = new Model(Game.GetHashKey(ModelName));
            modelToCreate.LoadAndWait();
            ped = NativeFunction.Natives.CREATE_PED<Ped>(26, Game.GetHashKey(ModelName), VendorPosition.X, VendorPosition.Y, VendorPosition.Z + 1f, VendorHeading, false, false);
        }
        else
        {
            Model modelToCreate = new Model(Game.GetHashKey(ModelName));
            modelToCreate.LoadAndWait();
            ped = NativeFunction.Natives.CREATE_PED<Ped>(26, Game.GetHashKey(ModelName), VendorPosition.X, VendorPosition.Y, VendorPosition.Z + 1f, VendorHeading, false, false);
        }
        GameFiber.Yield();
        if (ped.Exists())
        {
            ped.IsPersistent = true;//THIS IS ON FOR NOW!
            ped.RandomizeVariation();
            ped.Tasks.StandStill(-1);
            ped.KeepTasks = true;
            EntryPoint.SpawnedEntities.Add(ped);
            GameFiber.Yield();
            merchant = new Merchant(ped, settings, false, false, false, "Vendor", new PedGroup("Vendor", Name, "Vendor", false), crimes, weapons);
            merchant.Store = this;
            //AddEntity(Person);
        }
    }


}

