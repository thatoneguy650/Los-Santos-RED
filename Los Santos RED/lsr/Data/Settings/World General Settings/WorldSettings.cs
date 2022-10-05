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



    public bool ShowAllBlipsOnMap { get; set; }
    public bool ShowMarkersOnLocationEntrances { get; set; }
    public bool HotelsUsesRooms { get; set; }
    public bool AllowPoliceBackupBlip { get; set; }

    public WorldSettings()
    {
        SetDefault();

    }
    public void SetDefault()
    {
        UpdateVehiclePlates = true;
        CleanupVehicles = true;
        ReplaceVanillaShopKeepers = true;
        RandomVehiclePlatesPercent = 10f;
        AllowRandomVanityPlates = true;
        RandomVehicleVanityPlatesPercent = 2f;
        ShowAllLocationsOnDirectory = false;

        LowerPedSpawnsAtHigherWantedLevels = true;
        LowerPedSpawnsAtHigherWantedLevels_Wanted4Multiplier = 0.5f;
        LowerPedSpawnsAtHigherWantedLevels_Wanted5Multiplier = 0.3f;
        LowerPedSpawnsAtHigherWantedLevels_Wanted6Multiplier = 0.1f;
#if DEBUG
        RandomVehiclePlatesPercent = 15f;
        RandomVehicleVanityPlatesPercent = 10f;
#endif
        ShowAllBlipsOnMap = false;
        ShowMarkersOnLocationEntrances = false;
        HotelsUsesRooms = false;
        AllowPoliceBackupBlip = true;

    }

}