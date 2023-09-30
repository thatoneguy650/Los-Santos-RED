using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

public class ServiceSettings : ISettingsDefaultable
{
    [Description("Allows mod spawning of service peds in the world.")]
    public bool ManageDispatching { get; set; }
    [Description("Attach a blip to any spawned service pedestrian.")]
    public bool ShowSpawnedBlip { get; set; }
    [Description("Attach a blip to any ambient service pedestrian.")]
    public bool ShowAmbientBlips { get; set; }
    [Description("Minimum time in milliseconds between a spawn.")]
    public int TimeBetweenSpawn { get; set; }
    public int TimeBetweenSpawn_DowntownAdditional { get; set; }
    public int TimeBetweenSpawn_WildernessAdditional { get; set; }
    public int TimeBetweenSpawn_RuralAdditional { get; set; }
    public int TimeBetweenSpawn_SuburbAdditional { get; set; }
    public int TimeBetweenSpawn_IndustrialAdditional { get; set; }
    [Description("Minimum distance in meters to spawn a service ped from the player.")]
    public float MinDistanceToSpawnOnFoot { get; set; }
    [Description("Maximum distance in meters to spawn a service ped from the player.")]
    public float MaxDistanceToSpawnOnFoot { get; set; }
    [Description("Minimum distance in meters to spawn a service ped in a vehicle from the player.")]
    public float MinDistanceToSpawnInVehicle { get; set; }
    [Description("Maximum distance in meters to spawn a service ped in a vehicle from the player.")]
    public float MaxDistanceToSpawnInVehicle { get; set; }
    [Description("Total limit of spawned service peds. Does not include vanilla members.")]
    public int TotalSpawnedMembersLimit { get; set; }
    [Description("Total limit of ambient spawned service peds. Does not include vanilla service peds or service peds spawned by location.")]
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
    public float AmbientSpawnPedestrianAttemptPercentage { get; set; }
 

    public ServiceSettings()
    {
        SetDefault();
    }

    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        SetDefault();
    }

    public void SetDefault()
    {
        ShowSpawnedBlip = false;
        ShowAmbientBlips = false;
        ManageDispatching = true;

        TimeBetweenSpawn = 20000;//10000;

        TimeBetweenSpawn_DowntownAdditional = 20000;
        TimeBetweenSpawn_WildernessAdditional = 90000;
        TimeBetweenSpawn_RuralAdditional = 60000;
        TimeBetweenSpawn_SuburbAdditional = 30000;
        TimeBetweenSpawn_IndustrialAdditional = 30000;

        MinDistanceToSpawnOnFoot = 125f;//75f// 50f;
        MaxDistanceToSpawnOnFoot = 225f;//200f// 150f;

        MinDistanceToSpawnInVehicle = 250f;//300f// 50f;
        MaxDistanceToSpawnInVehicle = 500f;//500f// 150f;

        TotalSpawnedMembersLimit = 2;//5
        TotalSpawnedAmbientMembersLimit = 1;// 8;

        TotalSpawnedAmbientMembersLimit_Downtown = 2;
        TotalSpawnedAmbientMembersLimit_Wilderness = 1;
        TotalSpawnedAmbientMembersLimit_Rural = 1;
        TotalSpawnedAmbientMembersLimit_Suburb = 1;
        TotalSpawnedAmbientMembersLimit_Industrial = 2;

        AmbientSpawnPercentage = 40;
        AmbientSpawnPercentage_Wilderness = 20;
        AmbientSpawnPercentage_Rural = 25;
        AmbientSpawnPercentage_Suburb = 35;
        AmbientSpawnPercentage_Industrial = 35;
        AmbientSpawnPercentage_Downtown = 45;

#if DEBUG
        ShowSpawnedBlip = true;
#else

#endif
    }

}