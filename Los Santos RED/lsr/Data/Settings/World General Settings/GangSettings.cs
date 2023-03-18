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
    [Description("Attach a blip to any ambient gang member pedestrian.")]
    public bool ShowAmbientBlips { get; set; }
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

    public int TimeBetweenSpawn_DowntownAdditional { get; set; }
    public int TimeBetweenSpawn_WildernessAdditional { get; set; }
    public int TimeBetweenSpawn_RuralAdditional { get; set; }
    public int TimeBetweenSpawn_SuburbAdditional { get; set; }
    public int TimeBetweenSpawn_IndustrialAdditional { get; set; }





    [Description("Minimum distance in meters to spawn from the player.")]
    public float MinDistanceToSpawn { get; set; }
    [Description("Maximum distance in meters to spawn from the player.")]
    public float MaxDistanceToSpawn { get; set; }
    [Description("Total limit of spawned gang members between all gangs. Does not include vanilla members.")]
    public int TotalSpawnedMembersLimit { get; set; }


    [Description("Total limit of ambient spawned gang members between all gangs. Does not include vanilla members or gang members spawned by location.")]
    public int TotalSpawnedAmbientMembersLimit { get; set; }
    public int TotalSpawnedAmbientMembersLimit_Downtown { get; set; }
    public int TotalSpawnedAmbientMembersLimit_Wilderness { get; set; }
    public int TotalSpawnedAmbientMembersLimit_Rural { get; set; }
    public int TotalSpawnedAmbientMembersLimit_Suburb { get; set; }
    public int TotalSpawnedAmbientMembersLimit_Industrial { get; set; }



    [Description("Percentage of the time to allow an ambient spawn. Minimum 0, maximum 100.")]
    public int AmbientSpawnPercentage { get; set; }
    public int AmbientSpawnPercentage_Wilderness { get; set; }
    public int AmbientSpawnPercentage_Rural { get; set; }
    public int AmbientSpawnPercentage_Suburb { get; set; }
    public int AmbientSpawnPercentage_Industrial { get; set; }
    public int AmbientSpawnPercentage_Downtown { get; set; }





    [Description("Percentage of the time to spawn a gang outside of the den when near.")]
    public int PercentageSpawnNearDen { get; set; }
    [Description("Attempt to remove all non-mod spawned on-foot gang members from the world. (Not Currently Recommended)")]
    public bool RemoveVanillaSpawnedPedsOnFoot { get; set; }
    [Description("Enable or disable the ability for gangs to fight each other over crimes observed in the world.")]
    public bool AllowFightingOtherCriminals { get; set; }
    [Description("If enabled, peds will be forced to flee and all tasking will be blocked. If disabled, they will be tasked to flee, but let vanilla AI decide what to do.")]
    public bool BlockEventsDuringFlee { get; set; }
    [Description("If enabled, vanilla spaned gangsters in cars will always run from fights, police, and the player.")]
    public bool ForceAmbientCarDocile { get; set; }
    [Description("If enabled, ambient spawns will happen regardless of the player wanted level. If disabled, gangs will not have ambient spawns when you are wanted.")]
    public bool AllowAmbientSpawningWhenPlayerWanted { get; set; }

    [Description("Max wanted level that the mod will ambiently spawn gang peds.")]
    public int AmbientSpawningWhenPlayerWantedMaxWanted { get; set; }



    [Description("Enable or disable ambient spawns around dens.")]
    public bool AllowDenSpawning { get; set; }
    [Description("If enabled, den ambient spawns ignore the ped and vehicle spawn limits.")]
    public bool DenSpawningIgnoresLimits { get; set; }
    [Description("If enabled, den ambient spawns will happen regardless of the player wanted level. If disabled, dens will not have ambient spawns when you are wanted.")]
    public bool AllowDenSpawningWhenPlayerWanted { get; set; }
    [Description("Max wanted level that dens will spawn gang peds.")]
    public int DenSpawningWhenPlayerWantedMaxWanted { get; set; }
    public float EscortOffsetValue { get; set; }
    public float EscortSpeed { get; set; }


    public GangSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        ManageTasking = true;
        CheckCrimes = true;
        ShowSpawnedBlip = false;
        ShowAmbientBlips = false;
        RemoveVanillaSpawnedPeds = false;
        RemoveVanillaSpawnedPedsOutsideTerritory = false;
        RemoveVanillaSpawnedPedsOnFoot = false;
        PercentSpawnOutsideTerritory = 10;
        ManageDispatching = true;

        TimeBetweenSpawn = 30000;//10000;

        TimeBetweenSpawn_DowntownAdditional = 10000;
        TimeBetweenSpawn_WildernessAdditional = 90000;
        TimeBetweenSpawn_RuralAdditional = 60000;
        TimeBetweenSpawn_SuburbAdditional = 20000;
        TimeBetweenSpawn_IndustrialAdditional = 20000;




        MinDistanceToSpawn = 75f;// 50f;
        MaxDistanceToSpawn = 200f;// 150f;

        TotalSpawnedMembersLimit = 20;//5
        TotalSpawnedAmbientMembersLimit = 5;// 8;

        TotalSpawnedAmbientMembersLimit_Downtown = 5;
        TotalSpawnedAmbientMembersLimit_Wilderness = 2;
        TotalSpawnedAmbientMembersLimit_Rural = 3;
        TotalSpawnedAmbientMembersLimit_Suburb = 4;
        TotalSpawnedAmbientMembersLimit_Industrial = 4;

        AmbientSpawnPercentage = 40;
        AmbientSpawnPercentage_Wilderness = 5;
        AmbientSpawnPercentage_Rural = 20;
        AmbientSpawnPercentage_Suburb = 30;
        AmbientSpawnPercentage_Industrial = 40;
        AmbientSpawnPercentage_Downtown = 50;




    OverrideArmor = true;
        OverrideHealth = true;
        OverrideAccuracy = true;
        PercentageSpawnNearDen = 10;
        // MakeVanillaSpawnedGangMembersPersistent = false;
        AllowFightingOtherCriminals = false;
        BlockEventsDuringFlee = false;
        ForceAmbientCarDocile = true;


        AllowAmbientSpawningWhenPlayerWanted = true;

        AllowDenSpawning = true;
        DenSpawningIgnoresLimits = true;
        AllowDenSpawningWhenPlayerWanted = true;


#if DEBUG
        ShowSpawnedBlip = true;
#else

#endif
        EscortOffsetValue = 3.0f;
        EscortSpeed = 100f;
        DenSpawningWhenPlayerWantedMaxWanted = 4;
        AmbientSpawningWhenPlayerWantedMaxWanted = 4;
    }

}