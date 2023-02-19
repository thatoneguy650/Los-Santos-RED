using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SecuritySettings : ISettingsDefaultable
{
    [Description("Allows mod spawning of security services in the world.")]
    public bool ManageDispatching { get; set; }
    [Description("Allows tasking of ambient security pedestrians in the world.")]
    public bool ManageTasking { get; set; }
    [Description("Attach a blip to any spawned security pedestrian")]
    public bool ShowSpawnedBlips { get; set; }
    [Description("Distance (in meters) security need to be within to detain the player.")]
    public float DetainDistance { get; set; }
    [Description("Allows detaining of the player by security.")]
    public bool AllowDetainment { get; set; }

    public SecuritySettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        ManageDispatching = true;
        ManageTasking = true;
        ShowSpawnedBlips = false;

#if DEBUG
        ShowSpawnedBlips = true;
#endif
        DetainDistance = 4.0f;
        AllowDetainment = true;
    }
}