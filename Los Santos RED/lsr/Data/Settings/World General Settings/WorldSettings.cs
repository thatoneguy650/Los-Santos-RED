using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    [Description("If enabled, the civilian ped population will be lessened at 4+ stars.")]
    public bool LowerPedSpawnsAtHigherWantedLevels { get; set; }
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
    }

}