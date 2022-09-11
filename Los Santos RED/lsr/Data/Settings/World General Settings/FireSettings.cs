using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FireSettings : ISettingsDefaultable
{
    [Description("Allows mod spawning of firefighting services in the world. (Currently Disabled)")]
    public bool ManageDispatching { get; set; }
    [Description("Allows tasking of firefighter pedestrians in the world. (Currently Disabled)")]
    public bool ManageTasking { get; set; }
    [Description("Attach a blip to any spawned firefighter pedestrian. (Currently Disabled)")]
    public bool ShowSpawnedBlips { get; set; }
    public FireSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        ManageDispatching = false;
        ManageTasking = false;
        ShowSpawnedBlips = false;
    }
}