using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

public class SecuritySettings : ISettingsDefaultable
{












    public int TotalSpawnedMembersLimit { get; set; }








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

    [Description("Enable of disable removing all security peds not spawned by LSR.")]   
    public bool RemoveNonSpawnedSecurity { get; set; }
    [Description("Enable of disable removing all security peds not spawned by LSR that are non persistent.")]
    public bool RemoveAmbientSecurity { get; set; }

    [Description("Attach a blip to any ambient security peds.")]
    public bool AttachBlipsToAmbientPeds { get; set; }


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


    public bool AllowReactionsToBodies { get; set; }
    public bool AllowShootingInvestigations { get; set; }
    public bool AllowAlerts { get; set; }

    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        SetDefault();
    }
    public SecuritySettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        ManageDispatching = true;
        ManageTasking = true;
        ShowSpawnedBlips = false;


        DetainDistance = 4.0f;
        AllowDetainment = true;
        AttachBlipsToAmbientPeds = false;

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
        RemoveNonSpawnedSecurity = false;// true;
        RemoveAmbientSecurity = false;// true;
        AllowReactionsToBodies = true;
        AllowShootingInvestigations = true;
        AllowAlerts = true;
        TotalSpawnedMembersLimit = 3;
        ForceDefaultWeaponAnimations = true;
    }
}