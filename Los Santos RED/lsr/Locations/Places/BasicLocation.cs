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
    private IEntityProvideable World;
    private Blip createdBlip;
    private Interior interior;
    private float distanceToPlayer = 999f;
    private int CellsAway = 99;
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
                return 2000;// 8000;
            }
            else if (CellsAway >= 10)
            {
                return 1000;// 4000;
            }
            else if (CellsAway >= 6)
            {
                return 500;// 2000;
            }
            else
            {
                return 500;// 1000;
            }
        }
    }

    public void StoreData(IZones zones, IStreets streets)
    {
        Zone placeZone = zones.GetZone(EntrancePosition);
        string betweener = "";
        string zoneString = "";
        if (placeZone != null)
        {
            if (placeZone.IsSpecificLocation)
            {
                betweener = $"near";
            }
            else
            {
                betweener = $"in";
            }
            zoneString = $"~p~{placeZone.DisplayName}~s~";
        }
        string streetName = streets.GetStreetNames(EntrancePosition, false);
        string StreetNumber = "";
        if (streetName == "")
        {
            betweener = "";
        }
        else
        {
            StreetNumber = NativeHelper.CellToStreetNumber(CellX, CellY);
        }
        string LocationName = $"{StreetNumber} {streetName} {betweener} {zoneString}".Trim();
        string ShortLocationName = $"{StreetNumber} {streetName}".Trim();
        FullStreetAddress = LocationName;
        StreetAddress = ShortLocationName;
        ZoneName = zoneString;
        CellX = (int)(EntrancePosition.X / EntryPoint.CellSize);
        CellY = (int)(EntrancePosition.Y / EntryPoint.CellSize);
    }

    public bool HasBannerImage => BannerImagePath != "";
    [XmlIgnore]
    public Texture BannerImage { get; set; }
    public string BannerImagePath { get; set; } = "";
    public bool RemoveBanner { get; set; } = false;
    public bool IsEnabled { get; set; } = true;
    public string Name { get; set; }
    public string FullName { get; set; }







    public bool IsTemporarilyClosed { get; set; } = false;

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
    [XmlIgnore]
    public bool IsPlayerInterestedInLocation { get; set; } = false;
    public virtual string TypeName { get; set; } = "Location";
    public virtual bool ShowsOnDirectory { get; set; } = true;
    public virtual int MapIcon { get; set; } = (int)BlipSprite.PointOfInterest;
    public virtual Color MapIconColor { get; set; } = Color.White;
    public virtual float MapIconScale { get; set; } = 1.0f;
    public virtual float MapIconRadius { get; set; } = 1.0f;
    public virtual float MapIconAlpha { get; set; } = 1.0f;

    public virtual int SortOrder { get; set; } = 999;


    [XmlIgnore]
    public bool IsActivated { get; set; } = false;

    [XmlIgnore]
    public int CellX { get; set; }
    [XmlIgnore]
    public int CellY { get; set; }
    [XmlIgnore]
    public string FullStreetAddress { get; set; }
    [XmlIgnore]
    public string StreetAddress { get; set; }
    [XmlIgnore]
    public string ZoneName { get; set; }
    [XmlIgnore]
    public bool IsNearby { get; private set; } = false;



    public string ScannerFilePath { get; set; } = "";

    [XmlIgnore]
    public uint GameTimeLastMentioned { get; set; }
    //public string FullName => Name + " - " + StreetAddress;


    public BasicLocation()
    {

    }
    public BasicLocation(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description)
    {
        EntrancePosition = _EntrancePosition;
        EntranceHeading = _EntranceHeading;
        Name = _Name;
        Description = _Description;
        FullName = Name;
        //CellX = (int)(EntrancePosition.X / EntryPoint.CellSize);
        //CellY = (int)(EntrancePosition.Y / EntryPoint.CellSize);
    }

    public BasicLocation(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string _FullName)
    {
        EntrancePosition = _EntrancePosition;
        EntranceHeading = _EntranceHeading;
        Name = _Name;
        Description = _Description;
        FullName = _FullName;
        //CellX = (int)(EntrancePosition.X / EntryPoint.CellSize);
        //CellY = (int)(EntrancePosition.Y / EntryPoint.CellSize);
    }

    public float DistanceToPlayer => distanceToPlayer;
    public bool IsWalkup { get; set; } = false;
    public override string ToString()
    {
        return Name.ToString();
    }
    public bool IsOpen(int currentHour)
    {
        if(IsTemporarilyClosed)
        {
            return false;
        }
        return (CloseTime == 24 && OpenTime == 0) || (currentHour >= OpenTime && currentHour <= CloseTime);
    }
    public virtual void Activate(IInteriors interiors, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, ITimeReportable time, IEntityProvideable world)
    {
        IsActivated = true;
        World = world;
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
            createdBlip = AddIconToMap(time);
            GameFiber.Yield();
        }
        SetNearby();
        Update(time);
        if (!World.Places.ActiveLocations.Contains(this))
        {
            World.Places.ActiveLocations.Add(this);
        }
        world.AddBlip(Blip);
    }
    public void Update(ITimeReportable time)
    {
        if (IsNearby)
        {
            if (GameTimeLastCheckedDistance == 0 || Game.GameTime - GameTimeLastCheckedDistance >= DistanceUpdateIntervalTime)
            {
                distanceToPlayer = EntrancePosition.DistanceTo(Game.LocalPlayer.Character);

                if (Blip.Exists())
                {
                    if (IsOpen(time.CurrentHour))
                    {
                        Blip.Alpha = MapIconAlpha;
                    }
                    else
                    {
                        Blip.Alpha = 0.25f;
                    }
                }
                GameTimeLastCheckedDistance = Game.GameTime;
            }
        }
        else
        {
            distanceToPlayer = 999f;
            GameTimeLastCheckedDistance = Game.GameTime;
        }
    }
    public virtual void Deactivate()
    {
        IsActivated = false;
        if (createdBlip.Exists())
        {
            createdBlip.Delete();
        }
        if (interior != null)
        {
            interior.Unload();
        }
        if (World.Places.ActiveLocations.Contains(this))
        {
            World.Places.ActiveLocations.Remove(this);
        }
    }
    private Blip AddIconToMap(ITimeReportable time)
    {
        bool iscurrentlyOpen = IsOpen(time.CurrentHour);

        if (MapIconRadius != 1.0f)
        {
            Blip MyLocationBlip = new Blip(EntrancePosition, MapIconRadius)
            {
                Name = Name
            };
            MyLocationBlip.Color = Color.Blue;

            if (iscurrentlyOpen)
            {
                MyLocationBlip.Alpha = MapIconAlpha;
            }
            else
            {
                MyLocationBlip.Alpha = 0.25f;
            }
            
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
            if(IsPlayerInterestedInLocation)
            {
                MyLocationBlip.Color = Color.Blue;
            }
            else
            {
                MyLocationBlip.Color = MapIconColor;
            }
            MyLocationBlip.Scale = MapIconScale;
            if (iscurrentlyOpen)
            {
                MyLocationBlip.Alpha = MapIconAlpha;
            }
            else
            {
                MyLocationBlip.Alpha = 0.25f;
            }
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
    public virtual List<Tuple<string, string>> DirectoryInfo(int currentHour, float distanceTo)
    {
        List<Tuple<string, string>> toreturn = new List<Tuple<string, string>>();
        if(Description != "")
        {
            toreturn.Add(Tuple.Create(Description, ""));
        }
        toreturn.Add(Tuple.Create("Currently:", IsTemporarilyClosed ? "~r~Temporarily Closed~s~" : IsOpen(currentHour) ? "~s~Open~s~" : "~m~Closed~s~"));
        toreturn.Add(Tuple.Create("Hours:", Is247 ? "~g~24/7~s~" : $"{OpenTime}{(OpenTime <= 11 ? " am" : " pm")}-{CloseTime - 12}{(CloseTime <= 11 ? " am" : " pm")}"));
        toreturn.Add(Tuple.Create("Address:", StreetAddress));
        toreturn.Add(Tuple.Create("Location:", "~p~" + ZoneName + "~s~"));
        toreturn.Add(Tuple.Create("Distance:", Math.Round(distanceTo * 0.000621371, 2).ToString() + " Miles away"));
        return toreturn;

    }

}

