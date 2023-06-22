using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

public class GameLocation : ILocationDispatchable
{
    protected LocationCamera StoreCamera;
    protected ILocationInteractable Player;
    protected IModItems ModItems;
    protected IEntityProvideable World;
    protected ISettingsProvideable Settings;
    protected IWeapons Weapons;
    protected ITimeControllable Time;
    protected ICrimes Crimes;
    protected INameProvideable Names;
    protected IShopMenus ShopMenus;
    protected Transaction Transaction;
    protected uint NotificationHandle;
    protected readonly List<string> FallBackVendorModels = new List<string>() { "s_m_m_strvend_01", "s_m_m_linecook" };

    protected float currentblipAlpha = 0.25f;
    protected Color currentBlipColor = Color.White;
    protected Blip createdBlip;
    protected Interior interior;
    protected float distanceToPlayer = 999f;
    protected int CellsAway = 99;
    protected uint GameTimeLastCheckedDistance;
    protected uint GameTimeLastCheckedNearby;
    protected uint DistanceUpdateIntervalTime
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
            else if (DistanceToPlayer >= 20)
            {
                return 2000;
            }
            else
            {
                return 200;
            }
        }
    }
    protected uint NearbyUpdateIntervalTime
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
    public GameLocation() : base()
    {

    }
    public GameLocation(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description)
    {
        EntrancePosition = _EntrancePosition;
        EntranceHeading = _EntranceHeading;
        Name = _Name;
        Description = _Description;
        FullName = Name;
    }
    public string Name { get; set; }
    public string FullName { get; set; }
    public string Description { get; set; }
    public bool IsEnabled { get; set; } = true;
    public bool IsTemporarilyClosed { get; set; } = false;
    public string BannerImagePath { get; set; } = "";
    public bool RemoveBanner { get; set; } = false;
    public virtual bool IsBlipEnabled { get; set; } = true;
    public virtual int MapIcon { get; set; } = (int)BlipSprite.PointOfInterest;
    public Color MapIconColor => Color.FromName(MapIconColorString);
    public virtual string MapIconColorString { get; set; } = "White";
    public virtual float MapIconScale { get; set; } = 1.0f;
    public virtual float MapIconRadius { get; set; } = 1.0f;
    public virtual float MapOpenIconAlpha { get; set; } = 1.0f;
    public virtual float MapClosedIconAlpha { get; set; } = 0.25f;
    public Vector3 EntrancePosition { get; set; } = Vector3.Zero;
    public float EntranceHeading { get; set; }
    public int OpenTime { get; set; } = 6;
    public int CloseTime { get; set; } = 20;
    public int InteriorID { get; set; } = -1;
    public bool IsWalkup { get; set; } = false;
    public bool IsOnSPMap { get; set; } = true;
    public bool IsOnMPMap { get; set; } = true;
    public virtual bool ShowsOnDirectory { get; set; } = true;
    public virtual string TypeName { get; set; } = "Location";
    public virtual int SortOrder { get; set; } = 999;
    public string StateID { get; set; }
    public string ScannerFilePath { get; set; } = "";
    public virtual int ActivateCells { get; set; } = 5;
    public virtual RestrictedAreas RestrictedAreas { get; set; }
    public virtual string AssociationID => AssignedAssociationID;
    public string AssignedAssociationID { get; set; }
    public string MenuID { get; set; }
    public Vector3 VendorPosition { get; set; } = Vector3.Zero;
    public float VendorHeading { get; set; } = 0f;
    public List<string> VendorModels { get; set; }
    public Vector3 CameraPosition { get; set; } = Vector3.Zero;
    public Vector3 CameraDirection { get; set; } = Vector3.Zero;
    public Rotator CameraRotation { get; set; }
    public bool CanInteractWhenWanted { get; set; } = false;
    public virtual bool ShowsMarker { get; set; } = true;
    public virtual float ActivateDistance { get; set; } = 225;
    public List<ConditionalGroup> PossibleGroupSpawns { get; set; }
    public List<ConditionalLocation> PossiblePedSpawns { get; set; }
    public List<ConditionalLocation> PossibleVehicleSpawns { get; set; }



    public Vector3 VehiclePreviewCameraPosition { get; set; } = Vector3.Zero;
    public Vector3 VehiclePreviewCameraDirection { get; set; } = Vector3.Zero;
    public Rotator VehiclePreviewCameraRotation { get; set; }
    public SpawnPlace VehiclePreviewLocation { get; set; }

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
    public GameState GameState { get; set; }
    [XmlIgnore]
    public bool IsNearby { get; private set; } = false;
    [XmlIgnore]
    public uint GameTimeLastMentioned { get; set; }
    [XmlIgnore]
    public Texture BannerImage { get; set; }
    [XmlIgnore]
    public bool IsPlayerInterestedInLocation { get; set; } = false;
    [XmlIgnore]
    public virtual string ButtonPromptText { get; set; }
    [XmlIgnore]
    public Merchant Vendor { get; set; }
    [XmlIgnore]
    public bool HasVendor => VendorPosition != Vector3.Zero;
    [XmlIgnore]
    public ShopMenu Menu { get; set; }
    [XmlIgnore]
    public bool CanInteract { get; set; } = true;
    [XmlIgnore]
    public UIMenu InteractionMenu { get; private set; }
    [XmlIgnore]
    public MenuPool MenuPool { get; private set; }
    [XmlIgnore]
    public bool VendorAbandoned { get; set; } = false;
    [XmlIgnore]
    public Agency AssignedAgency { get; set; }
    [XmlIgnore]
    public bool IsDispatchFilled { get; set; } = false;
    [XmlIgnore]
    public float EntranceGroundZ { get; set; } = 0.0f;

    public bool IsAnyMenuVisible => MenuPool.IsAnyMenuOpen();
    public bool HasCustomCamera => CameraPosition != Vector3.Zero;
    public bool HasCustomVehicleCamera => VehiclePreviewCameraPosition != Vector3.Zero;
    public float DistanceToPlayer => distanceToPlayer;
    public Blip Blip => createdBlip;
    public bool ShouldAlwaysHaveBlip => false;
    public bool Is247 => CloseTime >= 24;
    public bool HasInterior => InteriorID != -1;
    public bool HasBannerImage => BannerImagePath != "";
    public Interior Interior => interior;

    public virtual void Activate(IInteriors interiors, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, ITimeReportable time, IEntityProvideable world)
    {
        World = world;
        if (HasVendor)
        {
            CanInteract = false;
            if (IsOpen(time.CurrentHour))
            {
                SpawnVendor(settings, crimes, weapons, true);// InteractsWithVendor);
            }
        }
        World.Pedestrians.AddEntity(Vendor);
        IsActivated = true;
        World = world;
        if (HasInterior)
        {
            interior = interiors.GetInteriorByLocalID(InteriorID);
            if (interior != null)
            {
                interior.Load();
            }
        }
        if (!ShouldAlwaysHaveBlip && IsBlipEnabled)
        {
            if (!Blip.Exists())
            {
                createdBlip = CreateBlip(time, true);
                GameFiber.Yield();
            }
        }
        SetNearby();
        Update(time);
        if (!World.Places.ActiveLocations.Contains(this))
        {
            World.Places.ActiveLocations.Add(this);
        }
        world.AddBlip(Blip);
        RestrictedAreas?.Activate(world);
    }
    public virtual void Deactivate(bool deleteBlip)
    {
        if (Vendor != null && Vendor.Pedestrian.Exists())
        {
            Vendor.Pedestrian.Delete();
        }
        IsActivated = false;
        if (deleteBlip)
        {
            if (Blip.Exists())
            {
                Blip.Delete();
            }
            if (createdBlip.Exists())
            {
                createdBlip.Delete();
            }
        }
        if (interior != null)
        {
            interior.Unload();
        }
        if (World != null && World.Places.ActiveLocations.Contains(this))
        {
            World.Places.ActiveLocations.Remove(this);
        }
        RestrictedAreas?.Deactivate();
    }
    public virtual List<Tuple<string, string>> DirectoryInfo(int currentHour, float distanceTo)
    {
        List<Tuple<string, string>> toreturn = new List<Tuple<string, string>>();
        if (Description != "")
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
    public virtual void StoreData(IShopMenus shopMenus, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, INameProvideable names, ICrimes crimes, IPedGroups PedGroups,
        IEntityProvideable world, IStreets streets, ILocationTypes locationTypes, ISettingsProvideable settings)
    {
        ShopMenus = shopMenus;
        World = world;
        Crimes = crimes;
        Names = names;
        Settings = settings;
        StoreBasicData(zones, streets, locationTypes);
        if (AssignedAssociationID != null)
        {
            AssignedAgency = agencies.GetAgency(AssignedAssociationID);
        }
        Menu = shopMenus.GetSpecificMenu(MenuID);
        if (PossiblePedSpawns != null)
        {
            foreach (ConditionalLocation cl in PossiblePedSpawns)
            {
                cl.Setup(agencies, gangs, zones, jurisdictions, gangTerritories, Settings, World, AssociationID, Weapons, Names, Crimes, PedGroups, shopMenus, Time, ModItems);
            }
        }
        if (PossibleVehicleSpawns != null)
        {
            foreach (ConditionalLocation cl in PossibleVehicleSpawns)
            {
                cl.Setup(agencies, gangs, zones, jurisdictions, gangTerritories, Settings, World, AssociationID, Weapons, Names, Crimes, PedGroups, shopMenus, Time, ModItems);
            }
        }     
    }
    public virtual void OnInteract(ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;

        if (IsLocationClosed())
        {
            return;
        }



        if (CanInteract)
        {
            Player.ActivityManager.IsInteractingWithLocation = true;
            CanInteract = false;
            Player.IsTransacting = true;

            GameFiber.StartNew(delegate
            {
                try
                {
                    StoreCamera = new LocationCamera(this, Player, Settings);
                    StoreCamera.Setup();

                    CreateInteractionMenu();
                    Transaction = new Transaction(MenuPool, InteractionMenu, Menu, this);
                    Transaction.CreateTransactionMenu(Player, modItems, world, settings, weapons, time);

                    InteractionMenu.Visible = true;
                    InteractionMenu.OnItemSelect += (selnder, selectedItem, index) =>
                    {
                        if (selectedItem.Text == "Buy" || selectedItem.Text == "Select")
                        {
                            Transaction?.SellMenu?.Dispose();
                            Transaction?.PurchaseMenu?.Show();
                        }
                        else if (selectedItem.Text == "Sell")
                        {
                            Transaction?.PurchaseMenu?.Dispose();
                            Transaction?.SellMenu?.Show();
                        }
                    };
                    Transaction.ProcessTransactionMenu();

                    Transaction.DisposeTransactionMenu();
                    DisposeInteractionMenu();

                    StoreCamera.Dispose();
                    Player.IsTransacting = false;
                    Player.ActivityManager.IsInteractingWithLocation = false;
                    CanInteract = true;
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "BarInteract");
        }
    }
    public virtual void OnItemSold(ModItem modItem, MenuItem menuItem, int totalItems)
    {

    }
    public virtual bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Interact with {Name}";
        return true;
    }
    public virtual void OnItemPurchased(ModItem modItem, MenuItem menuItem, int totalItems)
    {

    }
    public virtual void AddDistanceOffset(Vector3 offsetToAdd)
    {
        EntrancePosition += offsetToAdd;
        if (VendorPosition != Vector3.Zero)
        {
            VendorPosition += offsetToAdd;
        }
        if (CameraPosition != Vector3.Zero)
        {
            CameraPosition += offsetToAdd;
        }
        if (CameraDirection != Vector3.Zero)
        {
            CameraDirection += offsetToAdd;
        }
        List<ConditionalLocation> AllLocation = new List<ConditionalLocation>();
        if (PossiblePedSpawns != null)
        {
            AllLocation.AddRange(PossiblePedSpawns);
        }
        if (PossibleVehicleSpawns != null)
        {
            AllLocation.AddRange(PossibleVehicleSpawns);
        }
        foreach (ConditionalLocation cl in AllLocation)
        {
            cl.AddDistanceOffset(offsetToAdd);
        }
        RestrictedAreas?.AddDistanceOffset(offsetToAdd);
    }
    public virtual void DisplayMessage(string header, string message)
    {
        Game.RemoveNotification(NotificationHandle);
        NotificationHandle = Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, header, message);
    }
    public bool IsSameState(GameState state)
    {
        return state == null || GameState == null || state.StateID == GameState.StateID || state.IsSisterState(GameState);
    }
    private void StoreBasicData(IZones zones, IStreets streets, ILocationTypes locationTypes)
    {
        Zone placeZone = zones.GetZone(EntrancePosition);
        string betweener = "";
        string zoneString = "";
        //string stateString = "San Andreas";
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
            //stateString = placeZone.GameState?.StateName;
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

        if (string.IsNullOrEmpty(StateID))
        {
            StateID = StaticStrings.SanAndreasStateID;
        }

        GameState = locationTypes.GetState(StateID);

        CellX = (int)(EntrancePosition.X / EntryPoint.CellSize);
        CellY = (int)(EntrancePosition.Y / EntryPoint.CellSize);
    }
    public bool IsOpen(int currentHour)
    {
        if (IsTemporarilyClosed)
        {
            return false;
        }
        return (CloseTime == 24 && OpenTime == 0) || (currentHour >= OpenTime && currentHour <= CloseTime);
    }
    public override string ToString()
    {
        return Name;
    }
    public void ActivateBlip(ITimeReportable time, IEntityProvideable world)
    {
        if (!createdBlip.Exists())
        {
            createdBlip = CreateBlip(time, true);
            world.AddBlip(Blip);
        }
    }
    public void DeactivateBlip()
    {
        if (createdBlip.Exists())
        {
            //EntryPoint.WriteToConsole("DEACTIVATING BLIP 2222222");
            createdBlip.Delete();
        }
    }
    public void Update(ITimeReportable time)
    {
        if (IsNearby)
        {
            if (GameTimeLastCheckedDistance == 0 || Game.GameTime - GameTimeLastCheckedDistance >= DistanceUpdateIntervalTime)
            {
                distanceToPlayer = EntrancePosition.DistanceTo(Game.LocalPlayer.Character);
                UpdateBlip(time);
                GameTimeLastCheckedDistance = Game.GameTime;
            }
            //RestrictedAreas?.Update(Player);
        }
        else
        {
            distanceToPlayer = 999f;
            GameTimeLastCheckedDistance = Game.GameTime;
        }

    }
    public void UpdateBlip(ITimeReportable time)
    {
        if (!Blip.Exists())
        {
            return;
        }

        float newAlpha;
        Color newColor;

        if (IsOpen(time.CurrentHour))
        {
            newAlpha = MapOpenIconAlpha;
        }
        else
        {
            newAlpha = MapClosedIconAlpha;
        }
        if (IsPlayerInterestedInLocation)
        {
            newColor = Color.Blue;
        }
        else
        {
            newColor = MapIconColor;
        }

        if (newAlpha != currentblipAlpha)
        {
            Blip.Alpha = newAlpha;
            currentblipAlpha = newAlpha;
            // EntryPoint.WriteToConsole($"CHANGING BLIP ALPHA {Name} {currentblipAlpha}");
        }
        if (newColor != currentBlipColor)
        {
            Blip.Color = newColor;
            currentBlipColor = newColor;
            //EntryPoint.WriteToConsole($"CHANGING BLIP Color {Name} {currentBlipColor}");
        }

    }
    private Blip CreateBlip(ITimeReportable time, bool isShortRange)
    {
        Blip locationBlip;
        if (MapIconRadius != 1.0f)
        {
            locationBlip = new Blip(EntrancePosition, MapIconRadius) { Name = Name, Color = MapIconColor };
        }
        else
        {
            locationBlip = new Blip(EntrancePosition) { Name = Name };
            if ((BlipSprite)MapIcon != BlipSprite.Destination)
            {
                locationBlip.Sprite = (BlipSprite)MapIcon;
            }
            locationBlip.Scale = MapIconScale;
        }

        currentblipAlpha = IsOpen(time.CurrentHour) ? MapOpenIconAlpha : MapClosedIconAlpha;
        currentBlipColor = IsPlayerInterestedInLocation ? Color.Blue : MapIconColor;

        locationBlip.Color = currentBlipColor;
        locationBlip.Alpha = currentblipAlpha;
        if (isShortRange)
        {
            NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)locationBlip.Handle, true);
        }
        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");
        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(Name);
        NativeFunction.Natives.END_TEXT_COMMAND_SET_BLIP_NAME(locationBlip);
        return locationBlip;

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
    public bool IsCorrectMap(bool isMPMapLoaded)
    {
        if (isMPMapLoaded)
        {
            return IsOnMPMap;
        }
        return IsOnSPMap;
    }
    public void CheckActivation(IEntityProvideable world, IInteriors interiors, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, ITimeReportable time)
    {
        World = world;
        if (CheckIsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, ActivateCells) && IsEnabled && IsCorrectMap(World.IsMPMapLoaded))// ((World.IsMPMapLoaded && IsOnMPMap) || (!World.IsMPMapLoaded && IsOnSPMap)))
        {
            if (!IsActivated)
            {
                Activate(interiors, settings, crimes, weapons, time, World);
                //EntryPoint.WriteToConsole($"Activated {Name}");
                GameFiber.Yield();
            }
        }
        else
        {
            if (IsActivated)
            {
                Deactivate(!settings.SettingsManager.WorldSettings.ShowAllBlipsOnMap);
                GameFiber.Yield();
            }
        }
        if (settings.SettingsManager.WorldSettings.ShowAllBlipsOnMap)
        {
            if (!IsActivated && IsEnabled && IsBlipEnabled && !Blip.Exists() && IsSameState(EntryPoint.FocusZone?.GameState) && IsCorrectMap(World.IsMPMapLoaded))//(EntryPoint.FocusZone == null || EntryPoint.FocusZone.State == StateLocation))
            {
                ActivateBlip(time, World);
                // EntryPoint.WriteToConsole($"Activated BLIP {Name} State:{GameState?.StateID} MyState:{EntryPoint.FocusZone?.GameState.StateID}");
            }
            else if ((!IsEnabled && Blip.Exists()) || (IsEnabled && IsBlipEnabled && Blip.Exists() && !IsSameState(EntryPoint.FocusZone?.GameState)))
            {
                DeactivateBlip();
                // EntryPoint.WriteToConsole($"DeactivateBlip BLIP {Name} State:{GameState?.StateID} MyState:{EntryPoint.FocusZone?.GameState.StateID}");
            }
            else
            {
                if (IsEnabled && IsBlipEnabled)
                {
                    UpdateBlip(time);
                }
            }
        }
        else
        {
            if (!IsActivated && Blip.Exists())
            {
                DeactivateBlip();
            }
        }
    }
    public void CreateInteractionMenu()
    {
        MenuPool = new MenuPool();
        InteractionMenu = new UIMenu(Name, Description);
        if (HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
            InteractionMenu.SetBannerType(BannerImage);
            Game.RawFrameRender += (s, e) => MenuPool.DrawBanners(e.Graphics);
            RemoveBanner = false;
            //EntryPoint.WriteToConsoleTestLong($"BANNER REGULAR {BannerImagePath} {HasBannerImage}");
        }
        else if (Menu != null && !string.IsNullOrEmpty(Menu.BannerOverride))
        {
            BannerImagePath = $"Plugins\\LosSantosRED\\images\\{Menu.BannerOverride}";// = true;
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{Menu.BannerOverride}");
            InteractionMenu.SetBannerType(BannerImage);
            RemoveBanner = false;
            Game.RawFrameRender += (s, e) => MenuPool.DrawBanners(e.Graphics);
            //EntryPoint.WriteToConsoleTestLong($"BANNER OVERRIDE {BannerImagePath} {HasBannerImage}");
        }
        //InteractionMenu.OnItemSelect += OnItemSelect;
        MenuPool.Add(InteractionMenu);
        CanInteract = false;
    }
    public void DisposeInteractionMenu()
    {
        Game.RawFrameRender -= (s, e) => MenuPool.DrawBanners(e.Graphics);
        if (InteractionMenu != null)
        {
            InteractionMenu.Visible = false;
        }
        CanInteract = true;
    }
    public void SetupPlayer(ILocationInteractable player)
    {
        Player = player;
    }
    public void ProcessInteractionMenu()
    {
        while (IsAnyMenuVisible)
        {
            MenuPool.ProcessMenus();
            GameFiber.Yield();
        }
    }
    public void PlayErrorSound()
    {
        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "ERROR", "HUD_LIQUOR_STORE_SOUNDSET", 0);
    }
    public void PlaySuccessSound()
    {
        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET", 0);
        //NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "WEAPON_PURCHASE", "HUD_AMMO_SHOP_SOUNDSET", 0);
    }
    protected void SpawnVendor(ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, bool addMenu)
    {
        Ped ped;
        string ModelName;
        if (VendorModels != null && VendorModels.Any())
        {
            ModelName = VendorModels.PickRandom();
        }
        else
        {
            ModelName = FallBackVendorModels.PickRandom();
        }


        NativeFunction.Natives.CLEAR_AREA(VendorPosition.X, VendorPosition.Y, VendorPosition.Z, 2f, true, false, false, false);

        Model modelToCreate = new Model(Game.GetHashKey(ModelName));
        modelToCreate.LoadAndWait();
        ped = NativeFunction.Natives.CREATE_PED<Ped>(26, Game.GetHashKey(ModelName), VendorPosition.X, VendorPosition.Y, VendorPosition.Z, VendorHeading, false, false);//ped = NativeFunction.Natives.CREATE_PED<Ped>(26, Game.GetHashKey(ModelName), VendorPosition.X, VendorPosition.Y, VendorPosition.Z + 1f, VendorHeading, false, false);
        GameFiber.Yield();
        if (ped.Exists())
        {
            ped.IsPersistent = true;//THIS IS ON FOR NOW!
            ped.RandomizeVariation();

            //

            NativeFunction.CallByName<bool>("TASK_START_SCENARIO_IN_PLACE", ped, "WORLD_HUMAN_STAND_IMPATIENT", 0, true);
            //ped.Tasks.StandStill(-1);
            ped.KeepTasks = true;
            EntryPoint.SpawnedEntities.Add(ped);
            GameFiber.Yield();
            if (ped.Exists())
            {
                Vendor = new Merchant(ped, settings, "Vendor", crimes, weapons, World);
                if (addMenu)
                {
                    //Merchant.ShopMenu = Menu;
                    Vendor.SetupTransactionItems(Menu);
                }
                Vendor.AssociatedStore = this;

                Vendor.SpawnPosition = VendorPosition;
                //EntryPoint.WriteToConsole($"MERCHANT SPAWNED? Menu: {Menu == null} HANDLE {ped.Handle}");


                //if (1 == 1)//PlacePedOnGround)
                //{
                //    float resultArg = ped.Position.Z;
                //    NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD(ped.Position.X, ped.Position.Y, ped.Position.Z, out resultArg, false);
                //    ped.Position = new Vector3(ped.Position.X, ped.Position.Y, resultArg);
                //}


            }
        }
    }
    protected bool IsLocationClosed()
    {
        if (IsTemporarilyClosed)
        {
            Game.RemoveNotification(NotificationHandle);
            NotificationHandle = Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~r~Closed", $"We're sorry, this location is ~r~Temporarily Closed~s~.");
            return true;
        }
        if (!IsOpen(Time.CurrentHour))
        {
            Game.RemoveNotification(NotificationHandle);
            NotificationHandle = Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~r~Closed", $"We're sorry, this location is now closed.~n~Hours: {OpenTime} to {CloseTime}");
            return true;
        }
        if (Player.IsWanted && !CanInteractWhenWanted)
        {
            Game.DisplayHelp($"{Name} is unavailable when wanted");
            PlayErrorSound();
            return true;
        }
        if (Player.IsWanted && CanInteractWhenWanted && (Player.ClosestPoliceDistanceToPlayer < 20f || Player.AnyPoliceRecentlySeenPlayer))
        {
            Game.DisplayHelp($"{Name} is unavailable when police are nearby");
            PlayErrorSound();
            return true;
        }
        return false;
    }

    public virtual void HighlightVehicle()
    {
        if (StoreCamera == null || VehiclePreviewLocation == null)
        {
            return;
        }
        StoreCamera.HighlightVehicle();
        //StoreCamera.HighlightPosition(VehiclePreviewLocation.Position, VehiclePreviewLocation.Heading);
    }

    public virtual void ReHighlightStore()
    {
        if (StoreCamera == null || VehiclePreviewLocation == null)
        {
            return;
        }
        StoreCamera.HighlightHome();
    }
}

