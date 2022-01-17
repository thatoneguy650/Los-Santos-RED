using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ActivitySettings : ISettingsDefaultable
{
    [Description("Will teleport to the sitting entry point instead of walking. Useful when there are objects in the way like a large table you dont want to hit.")]
    public bool TeleportWhenSitting { get; set; }
    [Description("Attempt to set any blocking object as no collision with the player when sitting down. Used to stop from tipping over tables.")]
    public bool SetNoTableCollisionWhenSitting { get; set; }

    public ActivitySettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        TeleportWhenSitting = false;
        SetNoTableCollisionWhenSitting = true;
    }
}