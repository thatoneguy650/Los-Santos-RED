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


#if DEBUG
        RandomVehiclePlatesPercent = 15f;
        RandomVehicleVanityPlatesPercent = 10f;
#endif

    }

}