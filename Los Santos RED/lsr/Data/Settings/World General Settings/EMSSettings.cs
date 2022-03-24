using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EMSSettings : ISettingsDefaultable
{
    [Description("Allows mod spawning of EMS services in the world. (Currently Disabled)")]
    public bool ManageDispatching { get; set; }
    [Description("Allows tasking of ambient EMS pedestrians in the world. (Currently Disabled)")]
    public bool ManageTasking { get; set; }
    [Description("Attach a blip to any spawned EMS pedestrian")]
    public bool ShowSpawnedBlips { get; set; }
    [Description("Percent of pedestrians that are revived from unconsciousness. Max of 100")]
    public float RevivePercentage { get; set; }

    public EMSSettings()
    {
        SetDefault();
        #if DEBUG
            ShowSpawnedBlips =  true;
            ManageDispatching = true;
            ManageTasking = true;
        #else
            ShowSpawnedBlips = false;
        #endif
    }
    public void SetDefault()
    {
        ManageDispatching = false;
        ManageTasking = false;
        ShowSpawnedBlips = false;
        RevivePercentage = 40f;
    }
}