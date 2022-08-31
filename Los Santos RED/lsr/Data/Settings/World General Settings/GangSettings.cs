using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GangSettings : ISettingsDefaultable
{
    [Description("Allows mod spawning of gang members in the world.")]
    public bool ManageDispatching { get; set; }
    [Description("Allows tasking of ambient gang member pedestrians in the world.")]
    public bool ManageTasking { get; set; }
    [Description("Attach a blip to any spawned gang member pedestrian.")]
    public bool ShowSpawnedBlip { get; set; }
    [Description("Allows settings custom armor values on gang members.")]
    public bool OverrideArmor { get; set; }
    [Description("Allows settings custom health values on gang members.")]
    public bool OverrideHealth { get; set; }
    [Description("Allows settings custom accuracy values on gang members.")]
    public bool OverrideAccuracy { get; set; }
    [Description("Check and enforce crimes committed by ambient gang members. Required for police to react to gang member crimes.")]
    public bool CheckCrimes { get; set; }
    [Description("Attempt to remove all non-mod spawned gang members from the world. (Not Currently Recommended)")]
    public bool RemoveVanillaSpawnedPeds { get; set; }
    [Description("Attempt to remove all non-mod spawned gang members outside of their defined territories from the world. (Not Currently Recommended)")]
    public bool RemoveVanillaSpawnedPedsOutsideTerritory { get; set; }
    [Description("Percentage of the time to spawn a gang outside of their regular territory.")]
    public int PercentSpawnOutsideTerritory { get; set; }
    [Description("Minimum time in milliseconds between a spawn.")]
    public int TimeBetweenSpawn { get; set; }
    [Description("Minimum distance in meters to spawn from the player.")]
    public float MinDistanceToSpawn { get; set; }
    [Description("Maximum distance in meters to spawn from the player.")]
    public float MaxDistanceToSpawn { get; set; }
    [Description("Total limit of spawned gang members between all gangs. Does not include vanilla members.")]
    public int TotalSpawnedMembersLimit { get; set; }
    [Description("Percentage of the time to spawn a gang outside of the den when near.")]
    public int PercentageSpawnNearDen { get; set; }
    [Description("Attempt to remove all non-mod spawned on-foot gang members from the world. (Not Currently Recommended)")]
    public bool RemoveVanillaSpawnedPedsOnFoot { get; set; }
    public bool AllowFightingOtherCriminals { get; set; }

    //public bool MakeVanillaSpawnedGangMembersPersistent { get; set; }

    public GangSettings()
    {
        SetDefault();
#if DEBUG
        ShowSpawnedBlip = true;
        RemoveVanillaSpawnedPedsOutsideTerritory = false;
        //ManageDispatching = false;
        RemoveVanillaSpawnedPedsOnFoot = false;
      //  MakeVanillaSpawnedGangMembersPersistent = true;
#else
               // ShowSpawnedBlips = false;
#endif
    }
    public void SetDefault()
    {
        ManageTasking = true;
        CheckCrimes = true;
        ShowSpawnedBlip = false;
        RemoveVanillaSpawnedPeds = false;
        RemoveVanillaSpawnedPedsOutsideTerritory = false;
        RemoveVanillaSpawnedPedsOnFoot = false;
        PercentSpawnOutsideTerritory = 10;
        ManageDispatching = true;

        TimeBetweenSpawn = 10000;
        MinDistanceToSpawn = 50f;
        MaxDistanceToSpawn = 150f;
        TotalSpawnedMembersLimit = 8;//5
        OverrideArmor = true;
        OverrideHealth = true;
        OverrideAccuracy = true;
        PercentageSpawnNearDen = 10;
        // MakeVanillaSpawnedGangMembersPersistent = false;
        AllowFightingOtherCriminals = false;


    }

}