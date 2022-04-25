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
    [Description("Remove ambient vehicles that are empty from the game world. Not recommended to be disabled.")]
    public bool CleanupVehicles { get; set; }
    [Description("Delete the ambient shopkeeper peds as they spawn to not interfere with mod spawned merchant peds.")]
    public bool ReplaceVanillaShopKeepers { get; set; }
    public WorldSettings()
    {
        SetDefault();
#if DEBUG
        RandomVehiclePlatesPercent = 20f;
#endif
    }
    public void SetDefault()
    {
        UpdateVehiclePlates = true;
        CleanupVehicles = true;
        ReplaceVanillaShopKeepers = true;
        RandomVehiclePlatesPercent = 15f;
    }

}