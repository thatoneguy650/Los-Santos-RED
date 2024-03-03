using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

public class FireSettings : ISettingsDefaultable
{
    [Description("Allows mod spawning of firefighting services in the world.")]
    public bool ManageDispatching { get; set; }
    [Description("Allows tasking of firefighter pedestrians in the world.")]
    public bool ManageTasking { get; set; }
    [Description("Attach a blip to any spawned firefighter pedestrian.")]
    public bool ShowSpawnedBlips { get; set; }



    [Description("Enable or disable ambient spawns around hospitals.")]
    public bool AllowStationSpawning { get; set; }
    [Description("If enabled, hospital ambient spawns ignore the ped and vehicle spawn limits.")]
    public bool StationSpawningIgnoresLimits { get; set; }
    public bool AllowAlerts { get; set; }





    [Description("Minimum time in milliseconds between a spawn.")]
    public int TimeBetweenSpawn { get; set; }
    public int TimeBetweenSpawn_Investigation { get; set; }
    public int TimeBetweenSpawn_DowntownAdditional { get; set; }
    public int TimeBetweenSpawn_WildernessAdditional { get; set; }
    public int TimeBetweenSpawn_RuralAdditional { get; set; }
    public int TimeBetweenSpawn_SuburbAdditional { get; set; }
    public int TimeBetweenSpawn_IndustrialAdditional { get; set; }



    [Description("Minimum distance in meters to spawn from the player.")]
    public float MinDistanceToSpawn { get; set; }
    [Description("Maximum distance in meters to spawn from the player.")]
    public float MaxDistanceToSpawn { get; set; }
    [Description("Minimum distance in meters to spawn from the player during an investigation.")]
    public float MinDistanceToSpawn_Investigation { get; set; }
    [Description("Maximum distance in meters to spawn from the player during an investigation.")]
    public float MaxDistanceToSpawn_Investigation { get; set; }

    [Description("Total limit of spawned ems peds. Does not include vanilla members.")]
    public int TotalSpawnedMembersLimit { get; set; }



    [Description("Total limit of ambient spawned ems peds. Does not include vanilla members or ems members spawned by location.")]
    public int TotalSpawnedAmbientMembersLimit { get; set; }
    public int TotalSpawnedAmbientMembersLimit_Downtown { get; set; }
    public int TotalSpawnedAmbientMembersLimit_Wilderness { get; set; }
    public int TotalSpawnedAmbientMembersLimit_Rural { get; set; }
    public int TotalSpawnedAmbientMembersLimit_Suburb { get; set; }
    public int TotalSpawnedAmbientMembersLimit_Industrial { get; set; }

    public int TotalSpawnedAmbientMembersLimit_Investigation { get; set; }

    [Description("Percentage of the time to allow an ambient spawn. Minimum 0, maximum 100.")]
    public int AmbientSpawnPercentage { get; set; }
    public int AmbientSpawnPercentage_Wilderness { get; set; }
    public int AmbientSpawnPercentage_Rural { get; set; }
    public int AmbientSpawnPercentage_Suburb { get; set; }
    public int AmbientSpawnPercentage_Industrial { get; set; }
    public int AmbientSpawnPercentage_Downtown { get; set; }

    public int AmbientSpawnPercentage_Investigation { get; set; }


    [Description("If enabled, ambient spawns will happen regardless of the player wanted level. If disabled, gangs will not have ambient spawns when you are wanted.")]
    public bool AllowAmbientSpawningWhenPlayerWanted { get; set; }

    [Description("Max wanted level that the mod will ambiently spawn gang peds.")]
    public int AmbientSpawningWhenPlayerWantedMaxWanted { get; set; }


    public bool AllowStationSpawningWhenPlayerWanted { get; set; }
    [Description("Max wanted level that dens will spawn gang peds.")]
    public int StationSpawningWhenPlayerWantedMaxWanted { get; set; }
    public float FireAwareDistance { get; set; }
    public float LikelyHoodOfCountySpawn { get; set; }


    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        SetDefault();
    }



    public FireSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        ManageDispatching = true;
        ManageTasking = true;
        ShowSpawnedBlips = false;


        AllowStationSpawning = true;
        StationSpawningIgnoresLimits = true;


        AllowAlerts = true;



        TimeBetweenSpawn = 60000;//10000;
        TimeBetweenSpawn_Investigation = 10000;
        TimeBetweenSpawn_DowntownAdditional = 20000;
        TimeBetweenSpawn_WildernessAdditional = 90000;
        TimeBetweenSpawn_RuralAdditional = 70000;
        TimeBetweenSpawn_SuburbAdditional = 30000;
        TimeBetweenSpawn_IndustrialAdditional = 30000;


        MinDistanceToSpawn = 350f;// 50f;
        MaxDistanceToSpawn = 750f;// 1000f;// 150f;

        MinDistanceToSpawn_Investigation = 150f;// 50f;
        MaxDistanceToSpawn_Investigation = 250f;// 1000f;// 150f;

        TotalSpawnedMembersLimit = 4;// 6;//5
        TotalSpawnedAmbientMembersLimit = 2;// 8;

        TotalSpawnedAmbientMembersLimit_Downtown = 2;
        TotalSpawnedAmbientMembersLimit_Wilderness = 0;
        TotalSpawnedAmbientMembersLimit_Rural = 1;
        TotalSpawnedAmbientMembersLimit_Suburb = 2;
        TotalSpawnedAmbientMembersLimit_Industrial = 2;
        TotalSpawnedAmbientMembersLimit_Investigation = 2;

        AmbientSpawnPercentage = 2;// 30;
        AmbientSpawnPercentage_Wilderness = 0;
        AmbientSpawnPercentage_Rural = 0;
        AmbientSpawnPercentage_Suburb = 1;// 10;// 15;
        AmbientSpawnPercentage_Industrial = 1;// 15;// 25;
        AmbientSpawnPercentage_Downtown = 2;// 25;// 40;
        AmbientSpawnPercentage_Investigation = 25;// 25;// 70;

        AllowStationSpawningWhenPlayerWanted = true;
        StationSpawningWhenPlayerWantedMaxWanted = 2;
        AllowAmbientSpawningWhenPlayerWanted = true;
        AmbientSpawningWhenPlayerWantedMaxWanted = 2;
        FireAwareDistance = 35f;
        LikelyHoodOfCountySpawn = 30;
    }
}