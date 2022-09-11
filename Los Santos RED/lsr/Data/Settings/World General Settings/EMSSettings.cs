using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EMSSettings : ISettingsDefaultable
{
    [Description("Allows mod spawning of EMS services in the world.")]
    public bool ManageDispatching { get; set; }
    [Description("Allows tasking of ambient EMS pedestrians in the world.")]
    public bool ManageTasking { get; set; }
    [Description("Attach a blip to any spawned EMS pedestrian")]
    public bool ShowSpawnedBlips { get; set; }
    [Description("Percent of pedestrians that are revived from unconsciousness. Max of 100")]
    public float RevivePercentage { get; set; }



    [Description("Enable or disable ambient spawns around hospitals.")]
    public bool AllowStationSpawning { get; set; }
    [Description("If enabled, hospital ambient spawns ignore the ped and vehicle spawn limits.")]
    public bool StationSpawningIgnoresLimits { get; set; }


    public EMSSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        ManageDispatching = true;
        ManageTasking = true;
        ShowSpawnedBlips = false;
        RevivePercentage = 40f;
        AllowStationSpawning = true;
        StationSpawningIgnoresLimits = true;

#if DEBUG
        ShowSpawnedBlips = true;
#endif

    }
}