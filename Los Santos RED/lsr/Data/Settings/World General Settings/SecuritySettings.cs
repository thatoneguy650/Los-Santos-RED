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
    public SecuritySettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        ManageDispatching = false;
        ManageTasking = true;
        ShowSpawnedBlips = false;

#if DEBUG
        ManageDispatching = true;
        ShowSpawnedBlips = true;
#endif
    }
}