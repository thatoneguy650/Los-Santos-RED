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






    [Description("Enable or Disable accuracy override")]
    public bool OverrideAccuracy { get; set; }
    [Description("Enable or disable health override")]
    public bool OverrideHealth { get; set; }
    [Description("Enable or disable armor override")]
    public bool OverrideArmor { get; set; }
    [Description("Enable or disable auto load setting. (Not recommended to disable)")]
    public bool ManageLoadout { get; set; }




    public bool ForceDefaultWeaponAnimations { get; set; }
    public bool EnableCombatAttributeCanInvestigate { get; set; }
    public bool EnableCombatAttributeDisableEntryReactions { get; set; }
    public bool EnableCombatAttributeCanFlank { get; set; }
    public bool EnableCombatAttributeCanChaseOnFoot { get; set; }
    public bool OverrrideTargetLossResponse { get; set; }
    public int OverrrideTargetLossResponseValue { get; set; }
    public bool EnableConfigFlagAlwaysSeeAproachingVehicles { get; set; }
    public bool EnableConfigFlagDiveFromApproachingVehicles { get; set; }
    public bool AllowMinorReactions { get; set; }

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


        OverrideAccuracy = true;
        ManageLoadout = true;
        OverrideHealth = true;
        OverrideArmor = true;


        EnableCombatAttributeCanInvestigate = true;
        EnableCombatAttributeDisableEntryReactions = true;
        EnableCombatAttributeCanFlank = true;
        EnableCombatAttributeCanChaseOnFoot = true;

        OverrrideTargetLossResponse = true;
        OverrrideTargetLossResponseValue = 2;

        EnableConfigFlagAlwaysSeeAproachingVehicles = true;
        EnableConfigFlagDiveFromApproachingVehicles = true;
        AllowMinorReactions = true;
    }
}