//using LosSantosRED.lsr.Helper;
//using LosSantosRED.lsr.Interface;
//using Rage;
//using Rage.Native;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Runtime;
//using System.Xml.Serialization;

//[Serializable()]
//public class BasicLocation
//{

//    private float currentblipAlpha = 0.25f;
//    private Color currentBlipColor = Color.White;
//    private IEntityProvideable World;
//    private Blip createdBlip;
//    private Interior interior;
//    private float distanceToPlayer = 999f;
//    private int CellsAway = 99;
//    private uint GameTimeLastCheckedDistance;
//    private uint GameTimeLastCheckedNearby;
//    private uint DistanceUpdateIntervalTime
//    {
//        get
//        {
//            if (DistanceToPlayer >= 999f)
//            {
//                return 10000;
//            }
//            else if (DistanceToPlayer >= 500)
//            {
//                return 5000;
//            }
//            else if (DistanceToPlayer >= 20)
//            {
//                return 2000;
//            }
//            else
//            {
//                return 200;
//            }
//        }
//    }
//    private uint NearbyUpdateIntervalTime
//    {
//        get
//        {
//            if (CellsAway >= 20)
//            {
//                return 2000;// 8000;
//            }
//            else if (CellsAway >= 10)
//            {
//                return 1000;// 4000;
//            }
//            else if (CellsAway >= 6)
//            {
//                return 500;// 2000;
//            }
//            else
//            {
//                return 500;// 1000;
//            }
//        }
//    }
//    public BasicLocation()
//    {

//    }
//    public BasicLocation(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description)
//    {
//        EntrancePosition = _EntrancePosition;
//        EntranceHeading = _EntranceHeading;
//        Name = _Name;
//        Description = _Description;
//        FullName = Name;
//    }
//    public string Name { get; set; }
//    public string FullName { get; set; }
//    public string Description { get; set; }
//    public bool IsEnabled { get; set; } = true;
//    public bool IsTemporarilyClosed { get; set; } = false;
//    public string BannerImagePath { get; set; } = "";
//    public bool RemoveBanner { get; set; } = false;
//    public virtual bool IsBlipEnabled { get; set; } = true;
//    public virtual int MapIcon { get; set; } = (int)BlipSprite.PointOfInterest;
//    public Color MapIconColor => Color.FromName(MapIconColorString);
//    public virtual string MapIconColorString { get; set; } = "White";
//    public virtual float MapIconScale { get; set; } = 1.0f;
//    public virtual float MapIconRadius { get; set; } = 1.0f;
//    public virtual float MapOpenIconAlpha { get; set; } = 1.0f;
//    public virtual float MapClosedIconAlpha { get; set; } = 0.25f;
//    public Vector3 EntrancePosition { get; set; } = Vector3.Zero;
//    public float EntranceHeading { get; set; }
//    public int OpenTime { get; set; } = 6;
//    public int CloseTime { get; set; } = 20;
//    public int InteriorID { get; set; } = -1;
//    public bool IsWalkup { get; set; } = false;
//    public bool IsOnSPMap { get; set; } = true;
//    public bool IsOnMPMap { get; set; } = true;
//    public virtual bool ShowsOnDirectory { get; set; } = true;
//    public virtual string TypeName { get; set; } = "Location";
//    public virtual int SortOrder { get; set; } = 999;
//    public string StateID { get; set; }
//    public string ScannerFilePath { get; set; } = "";
//    public virtual int ActivateCells { get; set; } = 5;
//    public virtual RestrictedAreas RestrictedAreas { get; set; }


//    [XmlIgnore]
//    public bool IsActivated { get; set; } = false;
//    [XmlIgnore]
//    public int CellX { get; set; }
//    [XmlIgnore]
//    public int CellY { get; set; }
//    [XmlIgnore]
//    public string FullStreetAddress { get; set; }
//    [XmlIgnore]
//    public string StreetAddress { get; set; }
//    [XmlIgnore]
//    public string ZoneName { get; set; }
//    [XmlIgnore]
//    public GameState GameState { get; set; }
//    [XmlIgnore]
//    public bool IsNearby { get; private set; } = false;
//    [XmlIgnore]
//    public uint GameTimeLastMentioned { get; set; }
//    [XmlIgnore]
//    public Texture BannerImage { get; set; }
//    [XmlIgnore]
//    public bool IsPlayerInterestedInLocation { get; set; } = false;


//    public float DistanceToPlayer => distanceToPlayer;
//    public Blip Blip => createdBlip;
//    public bool ShouldAlwaysHaveBlip => false;
//    public bool Is247 => CloseTime >= 24;
//    public bool HasInterior => InteriorID != -1;
//    public bool HasBannerImage => BannerImagePath != "";
//    public Interior Interior => interior;
//    public bool IsSameState(GameState state)
//    {
//        return state == null || GameState == null || state.StateID == GameState.StateID || state.IsSisterState(GameState);
//    }
//    public void StoreData(IZones zones, IStreets streets, ILocationTypes locationTypes)
//    {
//        Zone placeZone = zones.GetZone(EntrancePosition);
//        string betweener = "";
//        string zoneString = "";
//        //string stateString = "San Andreas";
//        if (placeZone != null)
//        {
//            if (placeZone.IsSpecificLocation)
//            {
//                betweener = $"near";
//            }
//            else
//            {
//                betweener = $"in";
//            }
//            zoneString = $"~p~{placeZone.DisplayName}~s~";
//            //stateString = placeZone.GameState?.StateName;
//        }
//        string streetName = streets.GetStreetNames(EntrancePosition, false);
//        string StreetNumber = "";
//        if (streetName == "")
//        {
//            betweener = "";
//        }
//        else
//        {
//            StreetNumber = NativeHelper.CellToStreetNumber(CellX, CellY);
//        }
//        string LocationName = $"{StreetNumber} {streetName} {betweener} {zoneString}".Trim();
//        string ShortLocationName = $"{StreetNumber} {streetName}".Trim();
//        FullStreetAddress = LocationName;
//        StreetAddress = ShortLocationName;
//        ZoneName = zoneString;

//        if (string.IsNullOrEmpty(StateID))
//        {
//            StateID = StaticStrings.SanAndreasStateID;
//        }

//        GameState = locationTypes.GetState(StateID);

//        CellX = (int)(EntrancePosition.X / EntryPoint.CellSize);
//        CellY = (int)(EntrancePosition.Y / EntryPoint.CellSize);
//    }
//    public bool IsOpen(int currentHour)
//    {
//        if(IsTemporarilyClosed)
//        {
//            return false;
//        }
//        return (CloseTime == 24 && OpenTime == 0) || (currentHour >= OpenTime && currentHour <= CloseTime);
//    }
//    public override string ToString()
//    {
//        return Name;
//    }
//    public virtual void Activate(IInteriors interiors, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, ITimeReportable time, IEntityProvideable world)
//    {
//        IsActivated = true;
//        World = world;
//        if (HasInterior)
//        {
//            interior = interiors.GetInteriorByLocalID(InteriorID);
//            if (interior != null)
//            {
//                interior.Load();
//            }
//        }
//        if (!ShouldAlwaysHaveBlip && IsBlipEnabled)
//        {
//            if (!Blip.Exists())
//            {
//                createdBlip = CreateBlip(time, true);
//                GameFiber.Yield();
//            }
//        }
//        SetNearby();
//        Update(time);
//        if (!World.Places.ActiveLocations.Contains(this))
//        {
//            World.Places.ActiveLocations.Add(this);
//        }
//        world.AddBlip(Blip);
//    }
//    public void ActivateBlip(ITimeReportable time, IEntityProvideable world)
//    {
//        if (!createdBlip.Exists())
//        {
//            createdBlip = CreateBlip(time, true);
//            world.AddBlip(Blip);
//        }
//    }
//    public void DeactivateBlip()
//    {
//        if (createdBlip.Exists())
//        {
//            //EntryPoint.WriteToConsole("DEACTIVATING BLIP 2222222");
//            createdBlip.Delete();
//        }
//    }
//    public void Update(ITimeReportable time)
//    {
//        if (IsNearby)
//        {
//            if (GameTimeLastCheckedDistance == 0 || Game.GameTime - GameTimeLastCheckedDistance >= DistanceUpdateIntervalTime)
//            {
//                distanceToPlayer = EntrancePosition.DistanceTo(Game.LocalPlayer.Character);
//                UpdateBlip(time);
//                GameTimeLastCheckedDistance = Game.GameTime;
//            }
//            //RestrictedAreas?.Update(Player);
//        }
//        else
//        {
//            distanceToPlayer = 999f;
//            GameTimeLastCheckedDistance = Game.GameTime;
//        }
        
//    }
//    protected virtual void ExtraUpdate()
//    {

//    }
//    public void UpdateBlip(ITimeReportable time)
//    {
//        if(!Blip.Exists())
//        {
//            return;
//        }

//        float newAlpha;
//        Color newColor;

//        if (IsOpen(time.CurrentHour))
//        {
//            newAlpha = MapOpenIconAlpha;
//        }
//        else
//        {
//            newAlpha = MapClosedIconAlpha;
//        }
//        if (IsPlayerInterestedInLocation)
//        {
//            newColor = Color.Blue;
//        }
//        else
//        {
//            newColor = MapIconColor;
//        }

//        if(newAlpha != currentblipAlpha)
//        {
//            Blip.Alpha = newAlpha;
//            currentblipAlpha = newAlpha;
//           // EntryPoint.WriteToConsole($"CHANGING BLIP ALPHA {Name} {currentblipAlpha}");
//        }
//        if (newColor != currentBlipColor)
//        {
//            Blip.Color = newColor;
//            currentBlipColor = newColor;
//            //EntryPoint.WriteToConsole($"CHANGING BLIP Color {Name} {currentBlipColor}");
//        }

//    }
//    public virtual void Deactivate(bool deleteBlip)
//    {
//        IsActivated = false;

//        if (deleteBlip)
//        {
//            if (Blip.Exists())
//            {
//                Blip.Delete();
//            }
//            if (createdBlip.Exists())
//            {
//                createdBlip.Delete();
//            }
//        }
//        if (interior != null)
//        {
//            interior.Unload();
//        }
//        if (World != null && World.Places.ActiveLocations.Contains(this))
//        {
//            World.Places.ActiveLocations.Remove(this);
//        }
//    }
//    private Blip CreateBlip(ITimeReportable time, bool isShortRange)
//    {
//        Blip locationBlip;
//        if (MapIconRadius != 1.0f)
//        {
//            locationBlip = new Blip(EntrancePosition, MapIconRadius){ Name = Name, Color = MapIconColor };
//        }
//        else
//        {
//            locationBlip = new Blip(EntrancePosition){ Name = Name};
//            if ((BlipSprite)MapIcon != BlipSprite.Destination)
//            {
//                locationBlip.Sprite = (BlipSprite)MapIcon;
//            }
//            locationBlip.Scale = MapIconScale;
//        }

//        currentblipAlpha = IsOpen(time.CurrentHour) ? MapOpenIconAlpha : MapClosedIconAlpha;
//        currentBlipColor = IsPlayerInterestedInLocation ? Color.Blue : MapIconColor;

//        locationBlip.Color = currentBlipColor;
//        locationBlip.Alpha = currentblipAlpha;
//        if (isShortRange)
//        {
//            NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)locationBlip.Handle, true);
//        }
//        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");
//        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(Name);
//        NativeFunction.Natives.END_TEXT_COMMAND_SET_BLIP_NAME(locationBlip);
//        return locationBlip;

//    }
//    public bool CheckIsNearby(int cellX, int cellY, int Distance)
//    {
//        if (GameTimeLastCheckedNearby == 0 || Game.GameTime - GameTimeLastCheckedNearby >= NearbyUpdateIntervalTime)
//        {
//            CellsAway = NativeHelper.MaxCellsAway(cellX, cellY, CellX, CellY);
//            if (CellsAway <= Distance)
//            {
//                IsNearby = true;
//            }
//            else
//            {
//                IsNearby = false;
//            }
//            GameTimeLastCheckedNearby = Game.GameTime;
//        }
//        return IsNearby;
//    }
//    public void SetNearby()
//    {
//        IsNearby = true;
//    }
//    public virtual List<Tuple<string, string>> DirectoryInfo(int currentHour, float distanceTo)
//    {
//        List<Tuple<string, string>> toreturn = new List<Tuple<string, string>>();
//        if(Description != "")
//        {
//            toreturn.Add(Tuple.Create(Description, ""));
//        }
//        toreturn.Add(Tuple.Create("Currently:", IsTemporarilyClosed ? "~r~Temporarily Closed~s~" : IsOpen(currentHour) ? "~s~Open~s~" : "~m~Closed~s~"));
//        toreturn.Add(Tuple.Create("Hours:", Is247 ? "~g~24/7~s~" : $"{OpenTime}{(OpenTime <= 11 ? " am" : " pm")}-{CloseTime - 12}{(CloseTime <= 11 ? " am" : " pm")}"));
//        toreturn.Add(Tuple.Create("Address:", StreetAddress));
//        toreturn.Add(Tuple.Create("Location:", "~p~" + ZoneName + "~s~"));
//        toreturn.Add(Tuple.Create("Distance:", Math.Round(distanceTo * 0.000621371, 2).ToString() + " Miles away"));
//        return toreturn;

//    }
//    public bool IsCorrectMap(bool isMPMapLoaded)
//    {
//        if(isMPMapLoaded)
//        {
//            return IsOnMPMap;
//        }
//        return IsOnSPMap;
//    }
//    public void CheckActivation(IEntityProvideable world, IInteriors interiors, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, ITimeReportable time)
//    {
//        World = world;
//        if (CheckIsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, ActivateCells) && IsEnabled && IsCorrectMap(World.IsMPMapLoaded))// ((World.IsMPMapLoaded && IsOnMPMap) || (!World.IsMPMapLoaded && IsOnSPMap)))
//        {
//            if (!IsActivated)
//            {
//                Activate(interiors, settings, crimes, weapons, time, World);
//                //EntryPoint.WriteToConsole($"Activated {Name}");
//                GameFiber.Yield();
//            }
//        }
//        else
//        {
//            if (IsActivated)
//            {
//                Deactivate(!settings.SettingsManager.WorldSettings.ShowAllBlipsOnMap);
//                GameFiber.Yield();
//            }
//        }
//        if (settings.SettingsManager.WorldSettings.ShowAllBlipsOnMap)
//        {
//            if (!IsActivated && IsEnabled && IsBlipEnabled && !Blip.Exists() && IsSameState(EntryPoint.FocusZone?.GameState) && IsCorrectMap(World.IsMPMapLoaded))//(EntryPoint.FocusZone == null || EntryPoint.FocusZone.State == StateLocation))
//            {
//                ActivateBlip(time, World);
//               // EntryPoint.WriteToConsole($"Activated BLIP {Name} State:{GameState?.StateID} MyState:{EntryPoint.FocusZone?.GameState.StateID}");
//            }
//            else if ((!IsEnabled && Blip.Exists()) || (IsEnabled && IsBlipEnabled && Blip.Exists() && !IsSameState(EntryPoint.FocusZone?.GameState)))
//            {
//                DeactivateBlip();
//               // EntryPoint.WriteToConsole($"DeactivateBlip BLIP {Name} State:{GameState?.StateID} MyState:{EntryPoint.FocusZone?.GameState.StateID}");
//            }
//            else
//            {
//                if (IsEnabled && IsBlipEnabled)
//                {
//                    UpdateBlip(time);
//                }
//            }
//        }
//        else
//        {
//            if (!IsActivated && Blip.Exists())
//            {
//                DeactivateBlip();
//            }
//        }
//    }
//}

