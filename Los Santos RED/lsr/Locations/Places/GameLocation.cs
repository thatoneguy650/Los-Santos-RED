using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Locations;
using LSR.Vehicles;
using Microsoft.VisualBasic;
using Mod;
using Rage;
using Rage.Native;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime;
using System.Runtime.Serialization;
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
    protected IPlateTypes PlateTypes;
    protected IOrganizations Associations;
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
    protected uint GameTimeLastInteracted;
    protected DateTime NextRestockTime;
    protected DateTime NextPriceRefreshTime;
    protected DateTime LastInteractTime;

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
                return 4000;// 5000;
            }
            else if (DistanceToPlayer >= 250)
            {
                return 2000;// 5000;
            }
            else if (DistanceToPlayer >= 35)//20)
            {
                return 1000;// 2000;
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
    public string Description { get; set; } = "";
    public bool IsEnabled { get; set; } = true;
    public bool IsTemporarilyClosed { get; set; } = false;
    public string BannerImagePath { get; set; } = "";
    public bool RemoveBanner { get; set; } = false;
    public virtual bool IsBlipEnabled { get; set; } = true;
    public virtual int MapIcon { get; set; } = (int)BlipSprite.PointOfInterest;
    public virtual Color MapIconColor => Color.FromName(MapIconColorString);
    public virtual string MapIconColorString { get; set; } = "White";
    public virtual float MapIconScale { get; set; } = 0.5f;//1.0f;
    public virtual float MapIconRadius { get; set; } = 1.0f;
    public virtual float MapOpenIconAlpha { get; set; } = 1.0f;
    public virtual float MapClosedIconAlpha { get; set; } = 0.25f;
    public Vector3 EntrancePosition { get; set; } = Vector3.Zero;//ConditionalLocation
    public float EntranceHeading { get; set; }
    public int OpenTime { get; set; } = 6;
    public int CloseTime { get; set; } = 20;
    public int InteriorID { get; set; } = -1;
    public bool IsWalkup { get; set; } = false;
    public bool IsOnSPMap { get; set; } = true;
    public bool IsOnMPMap { get; set; } = true;
    public virtual bool ShowsOnDirectory { get; set; } = true;
    public virtual bool ShowsOnTaxi { get; set; } = true;
    public virtual string TypeName { get; set; } = "Location";
    public virtual int SortOrder { get; set; } = 999;
    public string StateID { get; set; }
    public string ScannerFilePath { get; set; } = "";
    public virtual int ActivateCells { get; set; } = 4;//4;//5;
    public virtual float ActivateDistance { get; set; } = 225f;//150f;//225;
    public virtual RestrictedAreas RestrictedAreas { get; set; }
    public virtual string AssociationID => AssignedAssociationID;
    public string AssignedAssociationID { get; set; }
    public bool DisableRegularInteract { get; set; } = false;
    public string MenuID { get; set; }

    public Vector3 CameraPosition { get; set; } = Vector3.Zero;
    public Vector3 CameraDirection { get; set; } = Vector3.Zero;
    public Rotator CameraRotation { get; set; }
    public virtual string BlipName => TypeName;
    public virtual bool CanInteractWhenWanted { get; set; } = false;
    public virtual bool ShowsMarker { get; set; } = true;
    public List<ConditionalGroup> PossibleGroupSpawns { get; set; }
    public List<ConditionalLocation> PossiblePedSpawns { get; set; }
    public List<ConditionalLocation> PossibleVehicleSpawns { get; set; }



    //  public List<ConditionalLocation> PossibleMerchantSpawns { get; set; }
    //public Vector3 VendorPosition { get; set; } = Vector3.Zero;
    //public float VendorHeading { get; set; } = 0f;
    //public List<string> VendorModels { get; set; }






    public List<SpawnPlace> VendorLocations { get; set; } = new List<SpawnPlace>();
    public virtual float ExtaVendorSpawnPercentage { get; set; } = 70f;
    public virtual string VendorHeadDataGroupID { get; set; }
    public virtual string VendorPersonnelID { get; set; } = "VendorPeds";
    public virtual string VendorMeleeWeaponsID { get; set; } = "VendorMeleeWeapons";
    public virtual string VendorSidearmWeaponsID { get; set; } = "VendorSidearms";
    public virtual string VendorLongGunWeaponsID { get; set; } = "VendorLongGuns";


    public virtual float VendorMeleePercent { get; set; } = 20f;
    public virtual float VendorSidearmPercent { get; set; } = 5f;
    public virtual float VendorLongGunPercent { get; set; } = 2f;


    public virtual int VendorMoneyMin { get; set; } = -1;
    public virtual int VendorMoneyMax { get; set; } = -1;

    public virtual float VendorFightPercentage { get; set; } = -1f;
    public virtual float VendorCallPolicePercentage { get; set; } = -1f;
    public virtual float VendorCallPoliceForSeriousCrimesPercentage { get; set; } = -1f;
    public virtual float VendorFightPolicePercentage { get; set; } = -1f;
    public virtual float VendorCowerPercentage { get; set; } = -1f;
    public virtual float VendorSurrenderPercentage { get; set; } = -1f;



    /*Money = RandomItems.GetRandomNumberInt(Settings.SettingsManager.CivilianSettings.MerchantMoneyMin, Settings.SettingsManager.CivilianSettings.MerchantMoneyMax);


            WillFight = RandomItems.RandomPercent(CivilianFightPercentage());
            WillCallPolice = RandomItems.RandomPercent(CivilianCallPercentage());
            WillCallPoliceIntense = RandomItems.RandomPercent(CivilianSeriousCallPercentage());
            WillFightPolice = RandomItems.RandomPercent(CivilianFightPolicePercentage());
            WillCower = RandomItems.RandomPercent(CivilianCowerPercentage());
            CanSurrender = RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.PossibleSurrenderPercentage);*/

    [XmlIgnore]
    public List<RandomHeadData> VendorPossibleHeads { get; set; } = new List<RandomHeadData>();
    [XmlIgnore]
    public List<DispatchablePerson> VendorPersonnel { get; set; } = new List<DispatchablePerson>();
    [XmlIgnore]
    public List<IssuableWeapon> VendorSideArms { get; set; } = new List<IssuableWeapon>();
    [XmlIgnore]
    public List<IssuableWeapon> VendorLongGuns { get; set; } = new List<IssuableWeapon>();
    [XmlIgnore]
    public List<IssuableWeapon> VendorMeleeWeapons { get; set; } = new List<IssuableWeapon>();



    [XmlIgnore]
    public virtual List<PedExt> SpawnedVendors
    {
        get
        {
            List<PedExt> toreturn = new List<PedExt>() { };
            toreturn.AddRange(Vendors);
            return toreturn;
        }
    }

    [XmlIgnore]
    public List<Merchant> Vendors { get; set; } = new List<Merchant>();







    public Vector3 VehiclePreviewCameraPosition { get; set; } = Vector3.Zero;
    public Vector3 VehiclePreviewCameraDirection { get; set; } = Vector3.Zero;
    public Rotator VehiclePreviewCameraRotation { get; set; }
    public SpawnPlace VehiclePreviewLocation { get; set; }
    public List<SpawnPlace> VehicleDeliveryLocations { get; set; } = new List<SpawnPlace>();
    public virtual int RegisterCashMin { get; set; } = 250;
    public virtual int RegisterCashMax { get; set; } = 1550;
    public bool NoEntryCam { get; set; } = false;

    public virtual int MinPriceRefreshHours { get; set; }
    public virtual int MaxPriceRefreshHours { get; set; }
    public virtual int MinRestockHours { get; set; }
    public virtual int MaxRestockHours { get; set; }

    //public int MaxAssaultSpawns { get; set; } = 15;

    public float CloseRange { get; set; } = 10f;



    public virtual int RacketeeringAmountMin { get; set; } = 500;
    public virtual int RacketeeringAmountMax { get; set; } = 1000;


    [XmlIgnore]
    public List<PedExt> LocationSpawnedPedExts { get; set; } = new List<PedExt>();
    [XmlIgnore]
    public List<VehicleExt> LocationSpawnedVehicleExts { get; set; } = new List<VehicleExt>();

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
    //[XmlIgnore]
    //public Merchant Vendor { get; set; }
    [XmlIgnore]
    public bool HasVendor => VendorLocations != null && VendorLocations.Any();// VendorPosition != Vector3.Zero;
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
    public bool IsServiceFilled { get; set; } = false;
    [XmlIgnore]
    public float EntranceGroundZ { get; set; } = 0.0f;
    [XmlIgnore]
    public int ProtectionMoneyDue { get; set; } = 0;
    [XmlIgnore]
    public int TotalAssaultSpawns { get; set; } = 0;
    [XmlIgnore]
    public float InteriorMaxUpdateDistance => Interior == null ? 50f : Interior.MaxUpdateDistance;






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
    public LocationCamera LocationCamera => StoreCamera;
    public bool IgnoreEntranceInteract { get; set; } = false;
    public virtual bool ShowInteractPrompt => !IgnoreEntranceInteract && CanInteract;
    public string MapTeleportString => IsOnSPMap && !IsOnMPMap ? "(SP)" : IsOnMPMap && !IsOnSPMap ? "(MP)" : "";


    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        //RacketeeringAmountMin = 500;
        //RacketeeringAmountMax = 1000;
    }

    public virtual void Activate(IInteriors interiors, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, ITimeReportable time, IEntityProvideable world)
    {
        //EntryPoint.WriteToConsole($"Activate Location {Name} {DistanceToPlayer}");
        World = world;
        bool isOpen = IsOpen(time.CurrentHour);
        if (HasVendor)
        {
            EntryPoint.WriteToConsole($"{Name} NOINT:{Interior == null} ISTELE:{Interior?.IsTeleportEntry} ISTELE:{Interior?.Name}");
            if (Interior == null || !Interior.IsTeleportEntry)
            {
                CanInteract = false;
            }
            //AttemptVendorSpawn(isOpen, interiors,settings,crimes,weapons,time,world);
        }
        if(DisableRegularInteract)
        {
            CanInteract = false;
        }
        //World.Pedestrians.AddEntity(Vendor);
        IsActivated = true;
        World = world;
        if (Interior != null && !Interior.IsTeleportEntry)
        {
            LoadInterior(isOpen);
        }
        if (!ShouldAlwaysHaveBlip && IsBlipEnabled)
        {
            ActivateBlip(time, world);
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
    public virtual void AttemptVendorSpawn(bool isOpen, IInteriors interiors, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, ITimeReportable time, IEntityProvideable world, bool isInterior)
    {
        //EntryPoint.WriteToConsole($"ATTEMPT VENDOR SPAWN AT {Name}");
        int VendorsSpawned = 0;
        List<SpawnPlace> spawns = new List<SpawnPlace>();
        if(isInterior)
        {
            if (Interior != null && Interior.VendorLocations != null && Interior.VendorLocations.Any())
            {
                spawns = Interior.VendorLocations.ToList();
            }
        }
        else
        {
            spawns = VendorLocations;
        }
        foreach (SpawnPlace spawnPlace in spawns)
        {
            if (IsOpen(time.CurrentHour) && ShouldSpawnVendor() && settings.SettingsManager.CivilianSettings.ManageDispatching && (isInterior || world.Pedestrians.TotalSpawnedServiceWorkers < settings.SettingsManager.CivilianSettings.TotalSpawnedServiceMembersLimit) && (VendorsSpawned == 0 || RandomItems.RandomPercent(ExtaVendorSpawnPercentage)))
            {
                //EntryPoint.WriteToConsole($"ATTEMPT VENDOR SPAWN AT {Name} START");
                if (SpawnVendor(spawnPlace))
                {
                    EntryPoint.WriteToConsole($"VENDOR SPAWNED AT {Name}");
                    VendorsSpawned++;
                }
            }
        }
    }
    protected virtual bool ShouldSpawnVendor() => true;
    protected virtual void LoadInterior(bool isOpen)
    {
        if (HasInterior && interior != null)
        {
            interior.Load(isOpen);
        }
    }
    public virtual void Deactivate(bool deleteBlip)
    {
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
        if (interior != null && !interior.IsTeleportEntry)
        {
            interior.Unload();
        }
        if (World != null && World.Places.ActiveLocations.Contains(this))
        {
            World.Places.ActiveLocations.Remove(this);
        }
        RestrictedAreas?.Deactivate();
        TotalAssaultSpawns = 0;
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
    public virtual string TaxiInfo(int currentHour, float distanceTo, TaxiFirm taxiFirm)
    {
        string toReturn = Description;
        toReturn += "~n~Currently: " + (IsTemporarilyClosed ? "~r~Temporarily Closed~s~" : IsOpen(currentHour) ? "~s~Open~s~" : "~m~Closed~s~");
        toReturn += "~n~Hours: " + (Is247 ? "~g~24/7~s~" : $"{OpenTime}{(OpenTime <= 11 ? " am" : " pm")}-{CloseTime - 12}{(CloseTime <= 11 ? " am" : " pm")}");
        toReturn += "~n~Address: " + StreetAddress;
        toReturn += "~n~Location: " + "~p~" + ZoneName + "~s~";
        toReturn += "~n~Distance: " + Math.Round(distanceTo * 0.000621371f, 2).ToString() + " Miles away";

        if(taxiFirm== null)
        {
            return toReturn;
        }
        toReturn += $"~n~~n~Base Fare: ${taxiFirm.BaseFare} ";
        toReturn += $"~n~Price Per Mile: ${taxiFirm.PricePerMile} ";
        toReturn += $"~n~Total Fare: ${taxiFirm.CalculateFare(distanceTo * 0.000621371f)} ";
        return toReturn;
    }
    public virtual void StoreData(IShopMenus shopMenus, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, INameProvideable names, ICrimes crimes, IPedGroups PedGroups,
        IEntityProvideable world, IStreets streets, ILocationTypes locationTypes, ISettingsProvideable settings, IPlateTypes plateTypes, IOrganizations associations, IContacts contacts, IInteriors interiors, 
        ILocationInteractable player, IModItems modItems, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest, IIssuableWeapons issuableWeapons, IHeads heads, IDispatchablePeople dispatchablePeople)
    {
        ShopMenus = shopMenus;
        World = world;
        Crimes = crimes;
        Names = names;
        Settings = settings;
        PlateTypes = plateTypes;
        Associations = associations;
        Player = player;
        ModItems = modItems;
        Weapons = weapons;
        Time = time;


        VendorMeleeWeapons = issuableWeapons.GetWeaponData(VendorMeleeWeaponsID);
        VendorLongGuns = issuableWeapons.GetWeaponData(VendorLongGunWeaponsID);
        VendorSideArms = issuableWeapons.GetWeaponData(VendorSidearmWeaponsID);
        VendorPersonnel = dispatchablePeople.GetPersonData(VendorPersonnelID);
        VendorPossibleHeads = heads.GetHeadData(VendorHeadDataGroupID);



        StoreBasicData(zones, streets, locationTypes);
        if (AssignedAssociationID != null)
        {
            AssignedAgency = agencies.GetAgency(AssignedAssociationID);
        }
        Menu = shopMenus.GetSpecificMenu(MenuID);
        if (HasInterior)
        {
            interior = interiors?.GetInteriorByLocalID(InteriorID);
        }  
    }
    public virtual void OnInteract()//ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        if (IsLocationClosed())
        {
            return;
        }
        if (!CanInteract)
        {
            return;
        }
        if (Interior != null && Interior.IsTeleportEntry)
        {
            DoEntranceCamera(true);
            Interior.Teleport(Player, this, StoreCamera);
        }
        else
        {
            StandardInteract(null, false);
        }
    }
    public virtual void StandardInteract(LocationCamera locationCamera, bool isInside)
    {
        Player.ActivityManager.IsInteractingWithLocation = true;
        CanInteract = false;
        Player.IsTransacting = true;
        GameFiber.StartNew(delegate
        {
            try
            {
                SetupLocationCamera(locationCamera, isInside, true);
                CreateInteractionMenu();
                HandleVariableItems();
                Transaction = new Transaction(MenuPool, InteractionMenu, Menu, this);
                Transaction.VehicleDeliveryLocations = VehicleDeliveryLocations;
                Transaction.VehiclePreviewPosition = VehiclePreviewLocation;
                Transaction.CreateTransactionMenu(Player, ModItems, World, Settings, Weapons, Time);
                InteractionMenu.Visible = true;
                Transaction.ProcessTransactionMenu();
                Transaction.DisposeTransactionMenu();
                DisposeInteractionMenu();
                DisposeCamera(isInside);
                DisposeInterior();
                Player.IsTransacting = false;
                Player.ActivityManager.IsInteractingWithLocation = false;
                CanInteract = true;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "StandardInteract");
    }
    public virtual void HandleVariableItems()
    {
        HandlePriceRefreshes();
        HandleSupplyRefreshes();
    }
    protected virtual void HandlePriceRefreshes()
    {
        if (MinPriceRefreshHours <= 0 && MaxPriceRefreshHours <= 0)
        {
            return;
        }
        if (DateTime.Compare(Time.CurrentDateTime, NextPriceRefreshTime) == 1)
        {
            foreach (MenuItem menuItem in Menu.Items)
            {
                menuItem.UpdatePrices();
            }
            NextPriceRefreshTime = Time.CurrentDateTime.AddHours(RandomItems.GetRandomNumberInt(MinPriceRefreshHours, MaxPriceRefreshHours));
            EntryPoint.WriteToConsole($"{Name} AND THE CURRENT TIME IS LATER THAN THE PRICE REFRESHTIME Current:{Time.CurrentDateTime} RefreshTime:{NextPriceRefreshTime} bPRICES HAVE BEEN REFRESHED");
        }
        else
        {
            EntryPoint.WriteToConsole($"{Name} AND WE ARE WAITING FOR THE PRICE REFRESH Current:{Time.CurrentDateTime} RefreshTime:{NextPriceRefreshTime}");
        }
    }
    protected virtual void HandleSupplyRefreshes()
    {
        if (MinRestockHours <= 0 && MaxRestockHours <= 0)
        {
            return;
        }
        if (DateTime.Compare(Time.CurrentDateTime, NextRestockTime) == 1)
        {
            foreach (MenuItem menuItem in Menu.Items)
            {
                menuItem.UpdateStock();
            }
            NextRestockTime = Time.CurrentDateTime.AddHours(RandomItems.GetRandomNumberInt(MinRestockHours, MaxRestockHours));
            EntryPoint.WriteToConsole($"{Name} AND THE CURRENT TIME IS LATER THAN THE NextRestockTime Current:{Time.CurrentDateTime} RefreshTime:{NextRestockTime} STOCK HAS BEEN UPDATED");
        }
        else
        {
            EntryPoint.WriteToConsole($"{Name} AND WE ARE WAITING FOR THE STOCK REFRESH Current:{Time.CurrentDateTime} RefreshTime:{NextRestockTime}");
        }
    }
    protected void DoEntranceCamera(bool sayGreeting)
    {
        StoreCamera = new LocationCamera(this, Player, Settings, NoEntryCam);
        StoreCamera.SayGreeting = sayGreeting;
        StoreCamera.DoEntranceOnly();
    }
    public void DoExitCamera(bool sayGreeting)
    {
        StoreCamera = new LocationCamera(this, Player, Settings, NoEntryCam);
        StoreCamera.SayGreeting = sayGreeting;
        StoreCamera.Dispose();
    }
    public void StandardInteractWithNewCamera(Vector3 desiredPosition, Vector3 desiredDirection, Rotator desiredRotation)
    {
        if(StoreCamera == null)
        {
            StoreCamera = new LocationCamera(this, Player, Settings, NoEntryCam);
        }
        StoreCamera.MoveToPosition(desiredPosition, desiredDirection, desiredRotation, true, true, false);
        StandardInteract(StoreCamera, true);
    }
    public virtual void OnPlayerBecameClose()
    {

    }
    protected virtual void DisposeInterior()
    {
        if (Interior != null)
        {
            Interior.IsMenuInteracting = false;
        }
    }
    protected virtual void DisposeCamera(bool isInside)
    {
        if (isInside)
        {
            StoreCamera?.ReturnToGameplay(true);
            StoreCamera?.StopImmediately(true);
        }
        else
        {
            StoreCamera?.Dispose();
        }
    }
    public virtual void OnItemSold(ModItem modItem, MenuItem menuItem, int totalItems)
    {
        //if (menuItem == null)
        //{
        //    return;
        //}
        //menuItem.NumberOfItemsPurchasedByPlayer += totalItems;
        //ItemDesires.OnItemsBoughtFromPlayer(modItem, totalItems);
    }
    public virtual bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Interact with {Name}";
        return true;
    }
    public virtual void OnItemPurchased(ModItem modItem, MenuItem menuItem, int totalItems)
    {
        //if(menuItem == null)
        //{
        //    return;
        //}
       // menuItem.NumberOfItemsSoldToPlayer += totalItems;
        //ItemDesires.OnItemsSoldToPlayer(modItem, totalItems);
    }
    public virtual void AddDistanceOffset(Vector3 offsetToAdd)
    {
        EntrancePosition += offsetToAdd;
        foreach(SpawnPlace sl in VendorLocations)
        {
            if (sl.Position != Vector3.Zero)
            {
                sl.Position += offsetToAdd;
            }
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
    public virtual void RemoveMessage()
    {
        Game.RemoveNotification(NotificationHandle);
    }
    public virtual void DisplayMessage(string header, string message)
    {
        Game.RemoveNotification(NotificationHandle);
        NotificationHandle = Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, header, message);
    }
    public virtual void Reset()
    {
        TotalAssaultSpawns = 0;
    }
    protected void SetupLocationCamera(LocationCamera locationCamera, bool isInside, bool sayGreeting)
    {
        if (locationCamera == null)
        {
            StoreCamera = new LocationCamera(this, Player, Settings, NoEntryCam || isInside);
            StoreCamera.SayGreeting = sayGreeting;
            if (isInside && Interior != null)
            {
                StoreCamera.IsInterior = true;
                StoreCamera.Interior = Interior;
            }
            if (!isInside)
            {
                StoreCamera.Setup();
            }
            EntryPoint.WriteToConsole("SetupLocationCamera CAM RAN");
        }
        else
        {
            StoreCamera = locationCamera;
            StoreCamera.SayGreeting = sayGreeting;
            EntryPoint.WriteToConsole("SetupLocationCamera CAM GOT PASSED IN");
        }
    }
    public bool IsSameState(LocationData locationData)
    {
        if(locationData == null)
        {
            return false;
        }
        if(locationData.CurrentZone == null)
        {
            return false;
        }
        return IsSameState(locationData.CurrentZone.GameState);
    }
    public bool IsSameState(GameState state)
    {
        return state == null || GameState == null || state.StateID == GameState.StateID || state.IsSisterState(GameState);
    }
    private void StoreBasicData(IZones zones, IStreets streets, ILocationTypes locationTypes)
    {
        CellX = (int)(EntrancePosition.X / EntryPoint.CellSize);
        CellY = (int)(EntrancePosition.Y / EntryPoint.CellSize);
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
        //else
        //{
            StreetNumber = NativeHelper.CellToStreetNumber(CellX, CellY);
        //}
        string LocationName = $"{StreetNumber} {streetName} {betweener} {zoneString}".Trim();
        string ShortLocationName = $"{StreetNumber} {streetName}".Trim();

        if(streetName == "")
        {
            LocationName = $"{StreetNumber} {zoneString}".Trim();
            ShortLocationName = $"{StreetNumber}".Trim();
        }


        FullStreetAddress = LocationName;
        StreetAddress = ShortLocationName;
        ZoneName = zoneString;
        if (string.IsNullOrEmpty(StateID))
        {
            StateID = StaticStrings.SanAndreasStateID;
        }
        GameState = locationTypes.GetState(StateID);
    }
    public bool IsOpen(int currentHour)
    {
        if (IsTemporarilyClosed)
        {
            return false;
        }
        if(CloseTime == 24 && OpenTime == 0)
        {
            return true;
        }//18 & 4, NEW BAD
        //8 & 20, OLD GOOD
        if(OpenTime < CloseTime)
        {
            return (currentHour >= OpenTime && currentHour <= CloseTime);
        }
        else
        {
            return (currentHour >= OpenTime || currentHour <= CloseTime);
        }

        //return (currentHour >= OpenTime && currentHour <= CloseTime);
    }
    public override string ToString()
    {
        return Name;
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
            if(DistanceToPlayer <= 100f)
            {
               /// UpdatePrompts();
                if (IsActivated && HasInterior)
                {
                    Interior?.Update();
                }
                if(DistanceToPlayer <= CloseRange)
                {
                    OnPlayerBecameClose();
                }
            }
            //RestrictedAreas?.Update(Player);
        }
        else
        {
            distanceToPlayer = 999f;
            GameTimeLastCheckedDistance = Game.GameTime;
        }
    }
    public virtual void ActivateBlip(ITimeReportable time, IEntityProvideable world)
    {
        if (!createdBlip.Exists())
        {
            //EntryPoint.WriteToConsole($"CREATE BLIP RAN FROM ActivateBlip {Name}");
            createdBlip = CreateBlip(time, true);
            world.AddBlip(Blip);
        }
    }
    public virtual void DeactivateBlip()
    {
        if (createdBlip.Exists())
        {
            //EntryPoint.WriteToConsole("DEACTIVATING BLIP 2222222");
            createdBlip.Delete();
        }
    }
    public virtual void UpdateBlip(ITimeReportable time)
    {
        if (!Blip.Exists())
        {
            return;
        }
        Color newColor;
        float newAlpha = GetCurrentIconAlpha(time);
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
            EntryPoint.WriteToConsole($"CHANGING BLIP Color {Name} {currentBlipColor}");
        }
    }
    protected virtual float GetCurrentIconAlpha(ITimeReportable time)
    {
        float newAlpha;
        if (IsOpen(time.CurrentHour))
        {
            newAlpha = MapOpenIconAlpha;
        }
        else
        {
            newAlpha = MapClosedIconAlpha;
        }
        return newAlpha;
    }
    private Blip CreateBlip(ITimeReportable time, bool isShortRange)
    {
        Blip locationBlip;
        if (MapIconRadius != 1.0f)
        {
            locationBlip = new Blip(EntrancePosition, MapIconRadius) { Color = MapIconColor };
        }
        else
        {
            locationBlip = new Blip(EntrancePosition);
            if ((BlipSprite)MapIcon != BlipSprite.Destination)
            {
                locationBlip.Sprite = (BlipSprite)MapIcon;
            }
            locationBlip.Scale = MapIconScale;

        }
        //EntryPoint.WriteToConsole($"Locations BLIP CREATED {Name}");
        currentblipAlpha = GetCurrentIconAlpha(time);// IsOpen(time.CurrentHour) ? MapOpenIconAlpha : MapClosedIconAlpha;
        currentBlipColor = IsPlayerInterestedInLocation ? Color.Blue : MapIconColor;
        locationBlip.Color = currentBlipColor;
        locationBlip.Alpha = currentblipAlpha;
        if (isShortRange)
        {
            NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)locationBlip.Handle, true);
        }
        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");



        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(BlipName);
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
        Player.OnInteractionMenuCreated(this, MenuPool, InteractionMenu);
        EntryPoint.WriteToConsole($"{Name} Interaction Created");
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
        NativeHelper.PlayErrorSound();
        //NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "ERROR", "HUD_LIQUOR_STORE_SOUNDSET", 0);
    }
    public void PlaySuccessSound()
    {
        NativeHelper.PlaySuccessSound();
        //NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET", 0);
        //NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "WEAPON_PURCHASE", "HUD_AMMO_SHOP_SOUNDSET", 0);
    }
    protected bool SpawnVendor(SpawnPlace spawnPlace)
    {
        //Ped ped;
        DispatchablePerson VendorPersonType = null;
        if (VendorPersonnel != null && VendorPersonnel.Any())
        {
            VendorPersonType = VendorPersonnel.PickRandom();
        }
        if(VendorPersonType == null)
        {
            VendorPersonType = new DispatchablePerson(FallBackVendorModels.PickRandom(), 100, 100);
        }
        
        HandleVariableItems();
        EntryPoint.WriteToConsole($"ATTEMPTING VENDOR AT {Name} {VendorPersonType.ModelName}");
        Vendors = new List<Merchant>();
        SpawnLocation sl = new SpawnLocation(spawnPlace.Position) { Heading = spawnPlace.Heading };
        MerchantSpawnTask merchantSpawnTask = new MerchantSpawnTask(sl, null,VendorPersonType,false,false,true,Settings,Crimes,Weapons,Names,World,ModItems,ShopMenus,this);

        merchantSpawnTask.PossibleHeads = VendorPossibleHeads;
        merchantSpawnTask.AllowAnySpawn = true;
        merchantSpawnTask.SpawnWithAllWeapons = true;
        merchantSpawnTask.AllowBuddySpawn = false;
        merchantSpawnTask.AttemptSpawn();
        EntryPoint.WriteToConsole($"ADDING VENDOR TO SPAWNED VENDORS LIST {Name} {VendorPersonType.ModelName}");

        foreach(Merchant merchant in merchantSpawnTask.SpawnedVendors)
        {
            EntryPoint.WriteToConsole($"I HAVE CREATED {merchant.Handle}");
        }

        Vendors.AddRange(merchantSpawnTask.SpawnedVendors);

        return merchantSpawnTask.CreatedPeople.Any();
    }

    protected virtual bool IsLocationClosed()
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

            int newOpenTime = OpenTime > 12 ? OpenTime - 12 : OpenTime;
            int newCloseTime = CloseTime > 12 ? CloseTime - 12 : CloseTime;
            string openRange = $"{newOpenTime}{(OpenTime <= 11 ? " am" : " pm")} to {newCloseTime}{(CloseTime <= 11 ? " am" : " pm")}";
            NotificationHandle = Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~r~Closed", $"We're sorry, this location is now closed.~n~Hours: {openRange}");
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

    public virtual bool HasAssociation(Gang gang)
    {
        return false;
    }

    public virtual void HighlightVehicle()
    {
        if (StoreCamera == null || VehiclePreviewLocation == null)
        {
            EntryPoint.WriteToConsole("Highlight vehicle ERROR");
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

    public virtual void AttemptVendorDespawn()
    {
        foreach(PedExt pedExt in SpawnedVendors.ToList())
        {
            if(pedExt.Pedestrian.Exists())
            {
                pedExt.DeleteBlip();
                pedExt.Pedestrian.IsPersistent = false;
                EntryPoint.WriteToConsole($"AttemptVendorDespawn MADE NON PERSIST");
            }
        }
        SpawnedVendors.Clear();
    }

    public IssuableWeapon GetRandomWeapon(bool isSidearm, IWeapons weapons)
    {
        List<IssuableWeapon> PossibleWeapons;
        if (isSidearm)
        {
            PossibleWeapons = VendorSideArms;
        }
        else
        {
            PossibleWeapons = VendorLongGuns;
        }
        if (PossibleWeapons != null && PossibleWeapons.Any())
        {
            EntryPoint.WriteToConsole($"GetRandomWeapon FROM STORE POSSIBLE WEAPONS{string.Join(",", PossibleWeapons)}");

            int Total = PossibleWeapons.Sum(x => x.SpawnChance);
            int RandomPick = RandomItems.MyRand.Next(0, Total);
            foreach (IssuableWeapon weapon in PossibleWeapons)
            {
                int SpawnChance = weapon.SpawnChance;
                if (RandomPick < SpawnChance)
                {
                    WeaponInformation WeaponLookup = weapons.GetWeapon(weapon.ModelName);
                    if (WeaponLookup != null)
                    {

                        weapon.SetIssued(Game.GetHashKey(weapon.ModelName), WeaponLookup.PossibleComponents, WeaponLookup.IsTaser);
                        EntryPoint.WriteToConsole($"GetRandomWeapon FROM STORE PCIKED 1 {weapon.ModelName}");
                        return weapon;
                    }
                }
                RandomPick -= SpawnChance;
            }
            if (PossibleWeapons.Any())
            {
                IssuableWeapon issuableWeapon = PossibleWeapons.PickRandom();
                EntryPoint.WriteToConsole($"GetRandomWeapon FROM STORE PCIKED 2 {issuableWeapon.ModelName}");
                return issuableWeapon;
            }
        }
        else
        {
            EntryPoint.WriteToConsole($"GetRandomWeapon FROM STORE POSSIBLE WEAPONS IS NULL");
        }
        return null;
    }
    public IssuableWeapon GetRandomMeleeWeapon(IWeapons weapons)
    {
        List<IssuableWeapon> PossibleWeapons = VendorMeleeWeapons;
        if (PossibleWeapons != null && PossibleWeapons.Any())
        {
            int Total = PossibleWeapons.Sum(x => x.SpawnChance);
            int RandomPick = RandomItems.MyRand.Next(0, Total);
            foreach (IssuableWeapon weapon in PossibleWeapons)
            {
                int SpawnChance = weapon.SpawnChance;
                if (RandomPick < SpawnChance)
                {
                    WeaponInformation WeaponLookup = weapons.GetWeapon(weapon.ModelName);
                    if (WeaponLookup != null)
                    {
                        weapon.SetIssued(Game.GetHashKey(weapon.ModelName), WeaponLookup.PossibleComponents, WeaponLookup.IsTaser);
                        return weapon;
                    }
                }
                RandomPick -= SpawnChance;
            }
            if (PossibleWeapons.Any())
            {
                return PossibleWeapons.PickRandom();
            }
        }
        return null;
    }

    public virtual int GetRacketeeringPaymentAmount()
    {
        return RandomItems.GetRandomNumberInt(500, 1000).Round(50);
    }

    public virtual void OnVendorKilledByPlayer(Merchant merchant, IViolateable player, IZones zones, IGangTerritories gangTerritories)
    {

    }
    public virtual void OnVendorInjuredByPlayer(Merchant merchant, IViolateable player, IZones zones, IGangTerritories gangTerritories)
    {
 
    }

    public virtual void AddLocation(PossibleLocations possibleLocations)
    {

    }


    //public virtual void UpdatePrompts()
    //{

    //}
}

