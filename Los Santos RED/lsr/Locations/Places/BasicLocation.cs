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
public class BasicLocation
{
    private Blip createdBlip;
    private Interior interior;
    private float distanceToPlayer = 999f;
    private int CellsAway = 99;
    /// private bool isNearby = false;
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

    public bool HasBannerImage => BannerImagePath != "";

    [XmlIgnore]
    public Texture BannerImage { get; set; }
    public string BannerImagePath { get; set; } = "";
    public bool RemoveBanner { get; set; } = false;
    //[XmlIgnore]
    public bool IsEnabled { get; set; } = true;
    public string Name { get; set; }
    public string Description { get; set; }

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


    public virtual int MapIcon { get; set; } = (int)BlipSprite.PointOfInterest;
    public virtual Color MapIconColor { get; set; } = Color.White;
    public virtual float MapIconScale { get; set; } = 1.0f;
    public virtual float MapIconRadius { get; set; } = 1.0f;
    public virtual float MapIconAlpha { get; set; } = 1.0f;
    //public virtual string ButtonPromptText { get; set; }


  //  [XmlIgnore]
    public int CellX { get; set; }
    //[XmlIgnore]
    public int CellY { get; set; }
    [XmlIgnore]
    public string StreetAddress { get; set; }
    [XmlIgnore]
    public bool IsNearby { get; private set; } = false;
    public BasicLocation()
    {

    }
    public BasicLocation(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description)
    {
        EntrancePosition = _EntrancePosition;
        EntranceHeading = _EntranceHeading;
        Name = _Name;
        Description = _Description;
        CellX = (int)(EntrancePosition.X / EntryPoint.CellSize);
        CellY = (int)(EntrancePosition.Y / EntryPoint.CellSize);
    }
    public float DistanceToPlayer => distanceToPlayer;
    public bool IsWalkup { get; set; } = false;
    public override string ToString()
    {
        return Name.ToString();
    }
    public bool IsOpen(int currentHour)
    {
        return (CloseTime == 24 && OpenTime == 0) || (currentHour >= OpenTime && currentHour <= CloseTime);
    }
    public virtual void Setup(IInteriors interiors, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons)
    {
        if (HasInterior)
        {
            interior = interiors.GetInterior(InteriorID);
            if (interior != null)
            {
                interior.Load();
            }
        }
        if (!ShouldAlwaysHaveBlip && IsBlipEnabled)
        {
            createdBlip = AddIconToMap();
            GameFiber.Yield();
        }
        SetNearby();
        Update();
    }
    public void Update()
    {
        if (IsNearby)
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
    public virtual void Dispose()
    {
        if (createdBlip.Exists())
        {
            createdBlip.Delete();
        }
        if (interior != null)
        {
            interior.Unload();
        }
    }
    private Blip AddIconToMap()
    {
        if (MapIconRadius != 1.0f)
        {
            Blip MyLocationBlip = new Blip(EntrancePosition, MapIconRadius)
            {
                Name = Name
            };
            MyLocationBlip.Color = Color.Blue;
            MyLocationBlip.Alpha = MapIconAlpha;
            NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)MyLocationBlip.Handle, true);
            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");
            NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(Name);
            NativeFunction.Natives.END_TEXT_COMMAND_SET_BLIP_NAME(MyLocationBlip);

            //EntryPoint.WriteToConsole($"CREATE LOCATION {Name} as a stupid fuck blip");


            return MyLocationBlip;
        }
        else
        {
            Blip MyLocationBlip = new Blip(EntrancePosition)
            {
                Name = Name
            };
            if ((BlipSprite)MapIcon != BlipSprite.Destination)
            {
                MyLocationBlip.Sprite = (BlipSprite)MapIcon;
            }

            MyLocationBlip.Color = MapIconColor;
            MyLocationBlip.Scale = MapIconScale;
            MyLocationBlip.Alpha = MapIconAlpha;
            NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)MyLocationBlip.Handle, true);
            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");
            NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(Name);
            NativeFunction.Natives.END_TEXT_COMMAND_SET_BLIP_NAME(MyLocationBlip);
            return MyLocationBlip;
        }


    }
    public bool CheckIsNearby(int cellX, int cellY, int Distance)
    {
        if (GameTimeLastCheckedNearby == 0 || Game.GameTime - GameTimeLastCheckedNearby >= NearbyUpdateIntervalTime)
        {
            CellsAway = NativeHelper.MaxCellsAway(cellX, cellY, CellX, CellY);
            if (CellsAway <= Distance)
            {
                IsNearby = true;
            }
            else
            {
                IsNearby = false;
            }
            GameTimeLastCheckedNearby = Game.GameTime;
        }
        return IsNearby;
    }
    public void SetNearby()
    {
        IsNearby = true;
    }

}

