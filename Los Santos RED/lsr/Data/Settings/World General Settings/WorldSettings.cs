using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

public class WorldSettings : ISettingsDefaultable
{
    [Description("Updates vehicle plates for the given state, plate style, and number format given in PlateTypes.xml.")]
    public bool UpdateVehiclePlates { get; set; }
    [Description("Percentage of vehicles that will get a plate type to match your current state (if not San Andreas).")]
    public float OutOfStateRandomVehiclePlatesPercent { get; set; }
    [Description("Percentage of vehicles that will get a random plate type (not dependant on state).")]
    public float RandomVehiclePlatesPercent { get; set; }
    [Description("Allow settings random vanity plates.")]
    public bool AllowRandomVanityPlates { get; set; }
    [Description("Percentage of vehicles that will get a random vanity plate.")]
    public float RandomVehicleVanityPlatesPercent { get; set; }
    [Description("Remove ambient vehicles that are empty from the game world. Not recommended to be disabled.")]
    public bool CleanupVehicles { get; set; }
    [Description("Delete the ambient shopkeeper peds as they spawn to not interfere with mod spawned merchant peds.")]
    public bool ReplaceVanillaShopKeepers { get; set; }
    [Description("If enabled all locations will be visible on the in game directoy (Messages or Player Info Menu). Disabled will show only legal locations.")]
    public bool ShowAllLocationsOnDirectory { get; set; }

    [Description("Sets the default spawn multiplier when LSR is active. Vanilla/Default is 1.0")]
    public float DefaultSpawnMultiplier { get; set; }
    [Description("If enabled, the civilian ped population will be lessened at 4+ stars.")]
    public bool LowerPedSpawnsAtHigherWantedLevels { get; set; }

    //[Description("Civilian ped density multiplier at 2 stars.")]
    //public float LowerPedSpawnsAtHigherWantedLevels_Wanted2Multiplier { get; set; }

    //[Description("Civilian ped density multiplier at 3 stars.")]
    //public float LowerPedSpawnsAtHigherWantedLevels_Wanted3Multiplier { get; set; }

    [Description("Civilian ped density multiplier at 4 stars.")]
    public float LowerPedSpawnsAtHigherWantedLevels_Wanted4Multiplier { get; set; }
    [Description("Civilian ped density multiplier at 5 stars.")]
    public float LowerPedSpawnsAtHigherWantedLevels_Wanted5Multiplier { get; set; }
    [Description("Civilian ped density multiplier at 6 stars.")]
    public float LowerPedSpawnsAtHigherWantedLevels_Wanted6Multiplier { get; set; }
    [Description("Civilian ped density multiplier at 7 stars.")]
    public float LowerPedSpawnsAtHigherWantedLevels_Wanted7Multiplier { get; set; }
    [Description("Civilian ped density multiplier at 8 stars.")]
    public float LowerPedSpawnsAtHigherWantedLevels_Wanted8Multiplier { get; set; }
    [Description("Civilian ped density multiplier at 9 stars.")]
    public float LowerPedSpawnsAtHigherWantedLevels_Wanted9Multiplier { get; set; }
    [Description("Civilian ped density multiplier at 10 stars.")]
    public float LowerPedSpawnsAtHigherWantedLevels_Wanted10Multiplier { get; set; }
    [Description("If enabled, ALL static blips will be added to the map.")]
    public bool ShowAllBlipsOnMap { get; set; }
    [Description("If enabled, there will be a 3D entrance marker around location entrances. Performance Intensive")]
    public bool ShowMarkersOnLocationEntrances { get; set; }
    [Description("If enabled, hotels will use specific rooms (if available).")]
    public bool HotelsUsesRooms { get; set; }
    [Description("If enabled, the blip showing where police are requsting backup will appear.")]
    public bool AllowPoliceBackupBlip { get; set; }

    [Description("If enabled, airports will require your owned vehicles to be nearby to use them for takeoff.")]
    public bool AirportsRequireOwnedPlanesLocal { get; set; }
    [Description("Distance (in meters) considered nearby in the case of owned planes being used for takeoff.")]
    public float AirportsOwnedPlanesLocalDistance { get; set; }
    [Description("If enabled, airports will require you to have a valid pilots license to take off.")]
    public bool AirportsRequireLicenseForPrivateFlights { get; set; }
    public bool AllowSettingDistantSirens { get; set; }
    public uint DeadBodyAlertTimeout { get; set; }
    public uint UnconsciousBodyAlertTimeout { get; set; }
    public uint GunshotAlertTimeout { get; set; }
    public uint HelpCryAlertTimeout { get; set; }
    public float OfficerMIACallInExpireDistance { get; set; }
    public float OfficerMIACallInDistance { get; set; }
    public uint OfficerMIACallInTimeMin { get; set; }
    public uint OfficerMIACallInTimeMax { get; set; }
    public bool AllowOfficerMIACallIn { get; set; }
    public float OfficerMIAStartPercentage_Alterted { get; set; }
    public float OfficerMIAStartPercentage_Regular { get; set; }
    [Description("If enabled, LSR set the siren state for any vehicle an AI Cop is in.")]
    public bool AllowSettingSirenState { get; set; }
    [Description("If enabled, LSR will set the vanilla taxi model as suppressed and only LSR will spawn them.")]
    public bool SetVanillaTaxiSuppressed { get; set; }
    public bool CreateObjectLocationsFromScanning { get; set; }
    public bool ShowMarkersInInteriors { get; set; }
    public int InteriorMarkerType { get; set; }
    public float InteriorMarkerZOffset { get; set; }
    public float InteriorMarkerScale { get; set; }
    [Description("If enabled, LSR will cancel a vehicle spawn if there is a mission entity within a certain radius")]
    public bool CheckAreaBeforeVehicleSpawn { get; set; }

    [Description("More aggressively remove abandoned or empty vehicle from the game world.")]
    public bool ExtendedVehicleCleanup { get;  set; }
    public bool SuppressFEJVehiclesFromGenerators { get; set; }
    public bool SetMissionFlagOn { get; set; }
    //public int MaxPedsBeforeDispatchPause { get; set; }
    //public int MaxVehiclesBeforeDispatchPause { get; set; }

    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        SetDefault();
    }
    public WorldSettings()
    {
        SetDefault();

    }
    public void SetDefault()
    {
        UpdateVehiclePlates = true;
        CleanupVehicles = true;
        ReplaceVanillaShopKeepers = true;
        RandomVehiclePlatesPercent = 7f;
        AllowRandomVanityPlates = true;
        RandomVehicleVanityPlatesPercent = 5f;
        ShowAllLocationsOnDirectory = false;

        LowerPedSpawnsAtHigherWantedLevels = true;

        DefaultSpawnMultiplier = 1.0f;

        //LowerPedSpawnsAtHigherWantedLevels_Wanted2Multiplier = 0.9f;
        //LowerPedSpawnsAtHigherWantedLevels_Wanted3Multiplier = 0.75f;
        LowerPedSpawnsAtHigherWantedLevels_Wanted4Multiplier = 0.5f;
        LowerPedSpawnsAtHigherWantedLevels_Wanted5Multiplier = 0.3f;
        LowerPedSpawnsAtHigherWantedLevels_Wanted6Multiplier = 0.1f;

        LowerPedSpawnsAtHigherWantedLevels_Wanted7Multiplier = 0.1f;
        LowerPedSpawnsAtHigherWantedLevels_Wanted8Multiplier = 0.1f;
        LowerPedSpawnsAtHigherWantedLevels_Wanted9Multiplier = 0.1f;
        LowerPedSpawnsAtHigherWantedLevels_Wanted10Multiplier = 0.1f;

        // ShowAllBlipsOnMap = false;
        ShowMarkersOnLocationEntrances = false;
        HotelsUsesRooms = false;
        AllowPoliceBackupBlip = true;

        ShowAllBlipsOnMap = true;

        AirportsRequireOwnedPlanesLocal = true;
        AirportsOwnedPlanesLocalDistance = 1000f;
        AirportsRequireLicenseForPrivateFlights = true;
        AllowSettingDistantSirens = true;
        OutOfStateRandomVehiclePlatesPercent = 90f;
        DeadBodyAlertTimeout = 25000;
        UnconsciousBodyAlertTimeout = 25000;
        GunshotAlertTimeout = 35000;
        HelpCryAlertTimeout = 20000;

        OfficerMIACallInExpireDistance = 250f;

        OfficerMIACallInDistance = 150f;
        OfficerMIACallInTimeMin = 90000;
        OfficerMIACallInTimeMax = 150000;


        AllowOfficerMIACallIn = true;
        AllowSettingSirenState = true;
        OfficerMIAStartPercentage_Alterted = 80f;
        OfficerMIAStartPercentage_Regular = 40f;


        SetVanillaTaxiSuppressed = true;
        // SetReducedPropsOnMap = true;
        CreateObjectLocationsFromScanning = false;
        ShowMarkersInInteriors = true;
        InteriorMarkerType = 0;
        InteriorMarkerZOffset = 0.2f;
        InteriorMarkerScale = 0.25f;

        CheckAreaBeforeVehicleSpawn = true;
        ExtendedVehicleCleanup = true;

        SuppressFEJVehiclesFromGenerators = true;
        SetMissionFlagOn = true;

        //MaxPedsBeforeDispatchPause = 120;
        //MaxVehiclesBeforeDispatchPause = 180;

    }

}