using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NeedsSettings : ISettingsDefaultable
{
    [Description("Enable or disable the entire needs system")]
    public bool ApplyNeeds { get; set; }
    [Description("Enable or diable the thirst component of the needs system")]
    public bool ApplyThirst { get; set; }
    [Description("Change the intensity of the drain and recovery for thirst. Default 1.0")]
    public float ThirstChangeScalar { get; set; }
    [Description("Enable or diable the hunger component of the needs system")]
    public bool ApplyHunger { get; set; }
    [Description("Change the intensity of the drain and recovery for hunger. Default 1.0")]
    public float HungerChangeScalar { get; set; }
    [Description("Enable or diable the sleep component of the needs system")]
    public bool ApplySleep { get; set; }
    [Description("Change the intensity of the drain and recovery for sleep. Default 1.0")]
    public float SleepChangeScalar { get; set; }

    public NeedsSettings()
    {
        SetDefault();
#if DEBUG

#endif
    }
    public void SetDefault()
    {
        ApplyNeeds = true;
        ApplyThirst = true;
        ThirstChangeScalar = 1.0f;
        ApplyHunger = true;
        HungerChangeScalar = 1.0f;
        ApplySleep = true;
        SleepChangeScalar = 1.0f;

    }

}