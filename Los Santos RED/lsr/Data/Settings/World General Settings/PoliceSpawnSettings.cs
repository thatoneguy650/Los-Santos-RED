using System.ComponentModel;
using System.Runtime.Serialization;

public class PoliceSpawnSettings : ISettingsDefaultable
{
    [Description("Allows mod spawning of police in the world. (Not recommended to disable)")]
    public bool ManageDispatching { get; set; }
    [Description("Attach a blip to any spawned police pedestrian")]
    public bool ShowSpawnedBlips { get; set; }
    [Description("Enable of disable removing all police peds not spawned by LSR.")]
    public bool RemoveNonSpawnedPolice { get; set; }
    [Description("Enable of disable removing all police peds not spawned by LSR that are non persistent.")]
    public bool RemoveAmbientPolice { get; set; }
    [Description("Percentage of time to add optional passengers to a vehicle. 0 is never 100 is always.")]
    public float AddOptionalPassengerPercentage { get; set; }
    [Description("Enable or disable ambient spawns around police stations.")]
    public bool AllowLocationSpawning { get; set; }
    [Description("Ped limit on how many location police can spawn.")]
    public int LocationSpawnedPedLimit { get; set; }
    [Description("Vehicle limit on how many location police can spawn.")]
    public int LocationSpawnedVehicleLimit { get; set; }

    [Description("Maximum distance (in meters) that police can spawn when you are wanted and seen by police.")]
    public float MaxDistanceToSpawn_WantedSeen { get; set; }
    [Description("Maximum distance (in meters) that police can spawn when you are wanted and not seen by police.")]
    public float MaxDistanceToSpawn_WantedUnseen { get; set; }
    [Description("Maximum distance (in meters) that police can spawn when you not wanted.")]
    public float MaxDistanceToSpawn_NotWanted { get; set; }
    [Description("Minimum distance (in meters) that police can spawn when you are wanted and not seen by police.")]
    public float MinDistanceToSpawn_WantedUnseen { get; set; }
    [Description("Minimum distance (in meters) that police can spawn when you are wanted and seen by police.")]
    public float MinDistanceToSpawn_WantedSeen { get; set; }
    [Description("Decrease min distance (in meters) that police can spawn for each wanted level when you are wanted and not seen by the police. A value of 40 with a wanted level of 3 would be (MinDistanceToSpawn_WantedUnseen - (3 * 40))")]
    public float MinDistanceToSpawn_WantedUnseenScalar { get; set; }
    [Description("Decrease min distance (in meters) that police can spawn for each wanted level when you are wanted and seen by police. A value of 40 with a wanted level of 3 would be (MinDistanceToSpawn_WantedUnseen - (3 * 40))")]
    public float MinDistanceToSpawn_WantedSeenScalar { get; set; }
    [Description("Minimum distance (in meters) that police can spawn when you are not wanted.")]
    public float MinDistanceToSpawn_NotWanted { get; set; }







    [Description("Time (in ms) between cop spawns when you are not seen.")]
    public int TimeBetweenCopSpawn_Unseen { get; set; }
    [Description("Minimum time (in ms) between cop spawns when you are seen by police.")]
    public int TimeBetweenCopSpawn_Seen_Min { get; set; }
    [Description("Decreased time (in ms) between cop spawns when you are seen as you increase your wanted level. Formula: ((6 - WantedLevel) * TimeBetweenCopSpawn_Seen_AdditionalTimeScaler) + TimeBetweenCopSpawn_Seen_Min;")]
    public int TimeBetweenCopSpawn_Seen_AdditionalTimeScaler { get; set; }
    [Description("Time (in ms) between cops despawning when you are unseen.")]
    public int TimeBetweenCopDespawn_Unseen { get; set; }
    [Description("Minimum time (in ms) between cop despawns when you are seen by police.")]
    public int TimeBetweenCopDespawn_Seen_Min { get; set; }
    [Description("Decreased time (in ms) between cop despawns when you are seen as you increase your wanted level. Formula: ((6 - WantedLevel) * TimeBetweenCopDespawn_Seen_AdditionalTimeScaler) + TimeBetweenCopDespawn_Seen_Min;")]
    public int TimeBetweenCopDespawn_Seen_AdditionalTimeScaler { get; set; }


    [Description("Minimum time in milliseconds between a spawn.")]
    public int AmbientTimeBetweenSpawn { get; set; }
    public int AmbientTimeBetweenSpawn_DowntownAdditional { get; set; }
    public int AmbientTimeBetweenSpawn_WildernessAdditional { get; set; }
    public int AmbientTimeBetweenSpawn_RuralAdditional { get; set; }
    public int AmbientTimeBetweenSpawn_SuburbAdditional { get; set; }
    public int AmbientTimeBetweenSpawn_IndustrialAdditional { get; set; }



    [Description("Percentage of the time to allow an ambient spawn. Minimum 0, maximum 100.")]
    public int AmbientSpawnPercentage { get; set; }
    public int AmbientSpawnPercentage_Wanted { get; set; }
    public int AmbientSpawnPercentage_Investigation { get; set; }
    public int AmbientSpawnPercentage_Wilderness { get; set; }
    public int AmbientSpawnPercentage_Rural { get; set; }
    public int AmbientSpawnPercentage_Suburb { get; set; }
    public int AmbientSpawnPercentage_Industrial { get; set; }
    public int AmbientSpawnPercentage_Downtown { get; set; }



    [Description("Maximum police peds that can be spawned when you are not wanted and no investigation is active.")]
    public int PedSpawnLimit_Default { get; set; }
    [Description("Maximum police peds that can be spawned when you are not wanted and no investigation is active for the wilderness zones")]
    public int PedSpawnLimit_Default_Wilderness { get; set; }
    [Description("Maximum police peds that can be spawned when you are not wanted and no investigation is active for the rural zones")]
    public int PedSpawnLimit_Default_Rural { get; set; }
    [Description("Maximum police peds that can be spawned when you are not wanted and no investigation is active for the suburb zones")]
    public int PedSpawnLimit_Default_Suburb { get; set; }
    [Description("Maximum police peds that can be spawned when you are not wanted and no investigation is active for the industrial zones")]
    public int PedSpawnLimit_Default_Industrial { get; set; }
    [Description("Maximum police peds that can be spawned when you are not wanted and no investigation is active for the downtown zones")]
    public int PedSpawnLimit_Default_Downtown { get; set; }
    [Description("Maximum police peds that can be spawned when you are not wanted and an investigation is active.")]
    public int PedSpawnLimit_Investigation { get; set; }
    [Description("Maximum police peds that can be spawned when you are at wanted level 1.")]
    public int PedSpawnLimit_Wanted1 { get; set; }
    [Description("Maximum police peds that can be spawned when you are at wanted level 2.")]
    public int PedSpawnLimit_Wanted2 { get; set; }
    [Description("Maximum police peds that can be spawned when you are at wanted level 3.")]
    public int PedSpawnLimit_Wanted3 { get; set; }
    [Description("Maximum police peds that can be spawned when you are at wanted level 4.")]
    public int PedSpawnLimit_Wanted4 { get; set; }
    [Description("Maximum police peds that can be spawned when you are at wanted level 5.")]
    public int PedSpawnLimit_Wanted5 { get; set; }
    [Description("Maximum police peds that can be spawned when you are at wanted level 6.")]
    public int PedSpawnLimit_Wanted6 { get; set; }
    [Description("Maximum police peds that can be spawned when you are at wanted level 7.")]
    public int PedSpawnLimit_Wanted7 { get; set; }
    [Description("Maximum police peds that can be spawned when you are at wanted level 8.")]
    public int PedSpawnLimit_Wanted8 { get; set; }
    [Description("Maximum police peds that can be spawned when you are at wanted level 9.")]
    public int PedSpawnLimit_Wanted9 { get; set; }
    [Description("Maximum police peds that can be spawned when you are at wanted level 10.")]
    public int PedSpawnLimit_Wanted10 { get; set; }






    [Description("Maximum police vehicles that can be spawned when you are not wanted and no investigation is active.")]
    public int VehicleSpawnLimit_Default { get; set; }
    [Description("Maximum police vehicles that can be spawned when you are not wanted and no investigation is active for the wilderness zones")]
    public int VehicleSpawnLimit_Default_Wilderness { get; set; }
    [Description("Maximum police vehicles that can be spawned when you are not wanted and no investigation is active for the rural zones")]
    public int VehicleSpawnLimit_Default_Rural { get; set; }
    [Description("Maximum police vehicles that can be spawned when you are not wanted and no investigation is active for the suburb zones")]
    public int VehicleSpawnLimit_Default_Suburb { get; set; }
    [Description("Maximum police vehicles that can be spawned when you are not wanted and no investigation is active for the industrial zones")]
    public int VehicleSpawnLimit_Default_Industrial { get; set; }
    [Description("Maximum police vehicles that can be spawned when you are not wanted and no investigation is active for the downtown zones")]
    public int VehicleSpawnLimit_Default_Downtown { get; set; }
    [Description("Maximum police vehicles that can be spawned when you are not wanted and an investigation is active.")]
    public int VehicleSpawnLimit_Investigation { get; set; }
    [Description("Maximum police vehicles that can be spawned when you are at wanted level 1.")]
    public int VehicleSpawnLimit_Wanted1 { get; set; }
    [Description("Maximum police vehicles that can be spawned when you are at wanted level 2.")]
    public int VehicleSpawnLimit_Wanted2 { get; set; }
    [Description("Maximum police vehicles that can be spawned when you are at wanted level 3.")]
    public int VehicleSpawnLimit_Wanted3 { get; set; }
    [Description("Maximum police vehicles that can be spawned when you are at wanted level 4.")]
    public int VehicleSpawnLimit_Wanted4 { get; set; }
    [Description("Maximum police vehicles that can be spawned when you are at wanted level 5.")]
    public int VehicleSpawnLimit_Wanted5 { get; set; }
    [Description("Maximum police vehicles that can be spawned when you are at wanted level 6.")]
    public int VehicleSpawnLimit_Wanted6 { get; set; }
    [Description("Maximum police vehicles that can be spawned when you are at wanted level 7.")]
    public int VehicleSpawnLimit_Wanted7 { get; set; }
    [Description("Maximum police vehicles that can be spawned when you are at wanted level 8.")]
    public int VehicleSpawnLimit_Wanted8 { get; set; }
    [Description("Maximum police vehicles that can be spawned when you are at wanted level 9.")]
    public int VehicleSpawnLimit_Wanted9 { get; set; }
    [Description("Maximum police vehicles that can be spawned when you are at wanted level 10.")]
    public int VehicleSpawnLimit_Wanted10 { get; set; }


    [Description("Maximum police boats that can be spawned when you are not wanted and no investigation is active.")]
    public int BoatSpawnLimit_Default { get; set; }
    [Description("Maximum police boats that can be spawned when you are not wanted and an investigation is active.")]
    public int BoatSpawnLimit_Investigation { get; set; }
    [Description("Maximum police boats that can be spawned when you are at wanted level 1.")]
    public int BoatSpawnLimit_Wanted1 { get; set; }
    [Description("Maximum police boats that can be spawned when you are at wanted level 2.")]
    public int BoatSpawnLimit_Wanted2 { get; set; }
    [Description("Maximum police boats that can be spawned when you are at wanted level 3.")]
    public int BoatSpawnLimit_Wanted3 { get; set; }
    [Description("Maximum police boats that can be spawned when you are at wanted level 4.")]
    public int BoatSpawnLimit_Wanted4 { get; set; }
    [Description("Maximum police boats that can be spawned when you are at wanted level 5.")]
    public int BoatSpawnLimit_Wanted5 { get; set; }
    [Description("Maximum police boats that can be spawned when you are at wanted level 6.")]
    public int BoatSpawnLimit_Wanted6 { get; set; }
    [Description("Maximum police boats that can be spawned when you are at wanted level 7.")]
    public int BoatSpawnLimit_Wanted7 { get; set; }
    [Description("Maximum police boats that can be spawned when you are at wanted level 8.")]
    public int BoatSpawnLimit_Wanted8 { get; set; }
    [Description("Maximum police boats that can be spawned when you are at wanted level 9.")]
    public int BoatSpawnLimit_Wanted9 { get; set; }
    [Description("Maximum police boats that can be spawned when you are at wanted level 10.")]
    public int BoatSpawnLimit_Wanted10 { get; set; }


    [Description("Maximum police helicopters that can be spawned when you are not wanted and no investigation is active.")]
    public int HeliSpawnLimit_Default { get; set; }
    [Description("Maximum police helicopters that can be spawned when you are not wanted and an investigation is active.")]
    public int HeliSpawnLimit_Investigation { get; set; }
    [Description("Maximum police helicopters that can be spawned when you are at wanted level 1.")]
    public int HeliSpawnLimit_Wanted1 { get; set; }
    [Description("Maximum police helicopters that can be spawned when you are at wanted level 2.")]
    public int HeliSpawnLimit_Wanted2 { get; set; }
    [Description("Maximum police helicopters that can be spawned when you are at wanted level 3.")]
    public int HeliSpawnLimit_Wanted3 { get; set; }
    [Description("Maximum police helicopters that can be spawned when you are at wanted level 4.")]
    public int HeliSpawnLimit_Wanted4 { get; set; }
    [Description("Maximum police helicopters that can be spawned when you are at wanted level 5.")]
    public int HeliSpawnLimit_Wanted5 { get; set; }
    [Description("Maximum police helicopters that can be spawned when you are at wanted level 6.")]
    public int HeliSpawnLimit_Wanted6 { get; set; }
    [Description("Maximum police helicopters that can be spawned when you are at wanted level 7.")]
    public int HeliSpawnLimit_Wanted7 { get; set; }
    [Description("Maximum police helicopters that can be spawned when you are at wanted level 8.")]
    public int HeliSpawnLimit_Wanted8 { get; set; }
    [Description("Maximum police helicopters that can be spawned when you are at wanted level 9.")]
    public int HeliSpawnLimit_Wanted9 { get; set; }
    [Description("Maximum police helicopters that can be spawned when you are at wanted level 10.")]
    public int HeliSpawnLimit_Wanted10 { get; set; }




    [Description("Maximum police K9 units that can be spawned when you are not wanted and no investigation is active.")]
    public int K9SpawnLimit_Default { get; set; }
    [Description("Maximum police K9 units that can be spawned when you are not wanted and an investigation is active.")]
    public int K9SpawnLimit_Investigation { get; set; }
    [Description("Maximum police K9 units that can be spawned when you are at wanted level 1.")]
    public int K9SpawnLimit_Wanted1 { get; set; }
    [Description("Maximum police K9 units that can be spawned when you are at wanted level 2.")]
    public int K9SpawnLimit_Wanted2 { get; set; }
    [Description("Maximum police K9 units that can be spawned when you are at wanted level 3.")]
    public int K9SpawnLimit_Wanted3 { get; set; }
    [Description("Maximum police K9 units that can be spawned when you are at wanted level 4.")]
    public int K9SpawnLimit_Wanted4 { get; set; }
    [Description("Maximum police K9 units that can be spawned when you are at wanted level 5.")]
    public int K9SpawnLimit_Wanted5 { get; set; }
    [Description("Maximum police K9 units that can be spawned when you are at wanted level 6.")]
    public int K9SpawnLimit_Wanted6 { get; set; }
    [Description("Maximum police K9 units that can be spawned when you are at wanted level 7.")]
    public int K9SpawnLimit_Wanted7 { get; set; }
    [Description("Maximum police K9 units that can be spawned when you are at wanted level 8.")]
    public int K9SpawnLimit_Wanted8 { get; set; }
    [Description("Maximum police K9 units that can be spawned when you are at wanted level 9.")]
    public int K9SpawnLimit_Wanted9 { get; set; }
    [Description("Maximum police K9 units that can be spawned when you are at wanted level 10.")]
    public int K9SpawnLimit_Wanted10 { get; set; }

    //[Description("Percentage of time to allow adding a canine unit to a spawn when possible. 0 is never 100 is always.")]
    //public int AddK9Percentage { get; set; }


    [Description("Percentage of time to add optional passengers to a vehicle. 0 is never 100 is always.")]
    public float AddAdditionalK9PassengerPercentage { get; set; }



    [Description("Percentage of time to allow spawning a random agency (that can spawn in the given location) instead of the main assigned jurisdiction when not wanted. Allows agencies without territory to spawn randomly. 0 is never 100 is always.")]
    public int LikelyHoodOfAnySpawn_Default { get; set; }
    [Description("Percentage of time to allow spawning the county assigned jurisdiction when not wanted. Allows the possibility of county agencies to spawn when in a location with zone based jurisdiction. 0 is never 100 is always.")]
    public int LikelyHoodOfCountySpawn_Default { get; set; }
    [Description("Percentage of time to allow spawning a random agency (that can spawn in the given location) instead of the main assigned jurisdiction when at 1 star. Allows agencies without territory to spawn randomly. 0 is never 100 is always.")]
    public int LikelyHoodOfAnySpawn_Wanted1 { get; set; }
    [Description("Percentage of time to allow spawning the county assigned jurisdiction when at 1 star. Allows the possibility of county agencies to spawn when in a location with zone based jurisdiction. 0 is never 100 is always.")]
    public int LikelyHoodOfCountySpawn_Wanted1 { get; set; }
    [Description("Percentage of time to allow spawning a random agency (that can spawn in the given location) instead of the main assigned jurisdiction when at 2 stars. Allows agencies without territory to spawn randomly. 0 is never 100 is always.")]
    public int LikelyHoodOfAnySpawn_Wanted2 { get; set; }
    [Description("Percentage of time to allow spawning the county assigned jurisdiction when at 2 stars. Allows the possibility of county agencies to spawn when in a location with zone based jurisdiction. 0 is never 100 is always.")]
    public int LikelyHoodOfCountySpawn_Wanted2 { get; set; }
    [Description("Percentage of time to allow spawning a random agency (that can spawn in the given location) instead of the main assigned jurisdiction when at 3 stars. Allows agencies without territory to spawn randomly. 0 is never 100 is always.")]
    public int LikelyHoodOfAnySpawn_Wanted3 { get; set; }
    [Description("Percentage of time to allow spawning the county assigned jurisdiction when at 3 stars. Allows the possibility of county agencies to spawn when in a location with zone based jurisdiction. 0 is never 100 is always.")]
    public int LikelyHoodOfCountySpawn_Wanted3 { get; set; }
    [Description("Percentage of time to allow spawning a random agency (that can spawn in the given location) instead of the main assigned jurisdiction when at 4 stars. Allows agencies without territory to spawn randomly. 0 is never 100 is always.")]
    public int LikelyHoodOfAnySpawn_Wanted4 { get; set; }
    [Description("Percentage of time to allow spawning the county assigned jurisdiction when at 4 stars. Allows the possibility of county agencies to spawn when in a location with zone based jurisdiction. 0 is never 100 is always.")]
    public int LikelyHoodOfCountySpawn_Wanted4 { get; set; }
    [Description("Percentage of time to allow spawning a random agency (that can spawn in the given location) instead of the main assigned jurisdiction when at 5 star. Allows agencies without territory to spawn randomly. 0 is never 100 is always.")]
    public int LikelyHoodOfAnySpawn_Wanted5 { get; set; }
    [Description("Percentage of time to allow spawning the county assigned jurisdiction when at 5 stars. Allows the possibility of county agencies to spawn when in a location with zone based jurisdiction. 0 is never 100 is always.")]
    public int LikelyHoodOfCountySpawn_Wanted5 { get; set; }
    [Description("Percentage of time to allow spawning a random agency (that can spawn in the given location) instead of the main assigned jurisdiction when at 6 star. Allows agencies without territory to spawn randomly. 0 is never 100 is always.")]
    public int LikelyHoodOfAnySpawn_Wanted6 { get; set; }
    [Description("Percentage of time to allow spawning the county assigned jurisdiction when at 6 stars. Allows the possibility of county agencies to spawn when in a location with zone based jurisdiction. 0 is never 100 is always.")]
    public int LikelyHoodOfCountySpawn_Wanted6 { get; set; }
    [Description("Percentage of time to allow spawning a random agency (that can spawn in the given location) instead of the main assigned jurisdiction when at 7 star. Allows agencies without territory to spawn randomly. 0 is never 100 is always.")]
    public int LikelyHoodOfAnySpawn_Wanted7 { get; set; }
    [Description("Percentage of time to allow spawning the county assigned jurisdiction when at 7 stars. Allows the possibility of county agencies to spawn when in a location with zone based jurisdiction. 0 is never 100 is always.")]
    public int LikelyHoodOfCountySpawn_Wanted7 { get; set; }
    [Description("Percentage of time to allow spawning a random agency (that can spawn in the given location) instead of the main assigned jurisdiction when at 8 star. Allows agencies without territory to spawn randomly. 0 is never 100 is always.")]
    public int LikelyHoodOfAnySpawn_Wanted8 { get; set; }
    [Description("Percentage of time to allow spawning the county assigned jurisdiction when at 8 stars. Allows the possibility of county agencies to spawn when in a location with zone based jurisdiction. 0 is never 100 is always.")]
    public int LikelyHoodOfCountySpawn_Wanted8 { get; set; }
    [Description("Percentage of time to allow spawning a random agency (that can spawn in the given location) instead of the main assigned jurisdiction when at 9 star. Allows agencies without territory to spawn randomly. 0 is never 100 is always.")]
    public int LikelyHoodOfAnySpawn_Wanted9 { get; set; }
    [Description("Percentage of time to allow spawning the county assigned jurisdiction when at 9 stars. Allows the possibility of county agencies to spawn when in a location with zone based jurisdiction. 0 is never 100 is always.")]
    public int LikelyHoodOfCountySpawn_Wanted9 { get; set; }
    [Description("Percentage of time to allow spawning a random agency (that can spawn in the given location) instead of the main assigned jurisdiction when at 10 star. Allows agencies without territory to spawn randomly. 0 is never 100 is always.")]
    public int LikelyHoodOfAnySpawn_Wanted10 { get; set; }
    [Description("Percentage of time to allow spawning the county assigned jurisdiction when at 10 stars. Allows the possibility of county agencies to spawn when in a location with zone based jurisdiction. 0 is never 100 is always.")]
    public int LikelyHoodOfCountySpawn_Wanted10 { get; set; }


    [Description("Distance required from the player (in meters) to be forcibly despawned when in a vehicle and there is no wanted level active.")]
    public float DistanceToRecallInVehicle_NotWanted { get; set; }
    [Description("Distance required from the player (in meters) to be forcibly despawned when in a vehicle and there is any wanted level active.")]
    public float DistanceToRecallInVehicle_Wanted { get; set; }

    [Description("Distance required from the player (in meters) to be forcibly despawned when in on foot and there is no wanted level active.")]
    public float DistanceToRecallOnFoot_NotWanted { get; set; }
    [Description("Distance required from the player (in meters) to be forcibly despawned when in on foot and there is any wanted level active.")]
    public float DistanceToRecallOnFoot_Wanted { get; set; }
    public float FootPatrolSpawnPercentage { get; set; }

    [Description("Minimum wanted level required to allow police to spawn on the tunnel spawn points.")]
    public int TunnelSpawnWantedLevelMinimum { get; set; }
    public bool AllowHelicopterPassengerRespawn { get; set; }
    public int HelicopterPassengerRespawnLimit { get; set; }



    public int PoliceDispatchDelay_Wilderness { get; set; }
    public int PoliceDispatchDelay_Rural { get; set; }
    public int PoliceDispatchDelay_Suburb { get; set; }
    public int PoliceDispatchDelay_Industrial { get; set; }
    public int PoliceDispatchDelay_Default { get; set; }
    public int PoliceDispatchDelay_Downtown { get; set; }


    public bool UseScalingLogicForSpawnWhenUnseen { get; set; }

    public uint HeliSpawnDelay_Default { get; set; }
    public uint HeliSpawnDelay_Wanted1 { get; set; }
    public uint HeliSpawnDelay_Wanted2 { get; set; }
    public uint HeliSpawnDelay_Wanted3 { get; set; }
    public uint HeliSpawnDelay_Wanted4 { get; set; }
    public uint HeliSpawnDelay_Wanted5 { get; set; }
    public uint HeliSpawnDelay_Wanted6 { get; set; }
    public uint HeliSpawnDelay_Wanted7 { get; set; }
    public uint HeliSpawnDelay_Wanted8 { get; set; }
    public uint HeliSpawnDelay_Wanted9 { get; set; }
    public uint HeliSpawnDelay_Wanted10 { get; set; }



    public uint K9SpawnDelay_Default { get; set; }
    public uint K9SpawnDelay_Wanted1 { get; set; }
    public uint K9SpawnDelay_Wanted2 { get; set; }
    public uint K9SpawnDelay_Wanted3 { get; set; }
    public uint K9SpawnDelay_Wanted4 { get; set; }
    public uint K9SpawnDelay_Wanted5 { get; set; }
    public uint K9SpawnDelay_Wanted6 { get; set; }
    public uint K9SpawnDelay_Wanted7 { get; set; }
    public uint K9SpawnDelay_Wanted8 { get; set; }
    public uint K9SpawnDelay_Wanted9 { get; set; }
    public uint K9SpawnDelay_Wanted10 { get; set; }


    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        SetDefault();
    }


    public PoliceSpawnSettings()
    {
        SetDefault();


    }
    public void SetDefault()
    {

        ManageDispatching = true;


        PedSpawnLimit_Default = 8;// 5;
        PedSpawnLimit_Default_Wilderness = 2;
        PedSpawnLimit_Default_Rural = 3;
        PedSpawnLimit_Default_Suburb = 4;
        PedSpawnLimit_Default_Industrial = 5;
        PedSpawnLimit_Default_Downtown = 9;// 6;

        LocationSpawnedPedLimit = 8;// 6;
        LocationSpawnedVehicleLimit = 8;// 6;


        PedSpawnLimit_Investigation = 7;
        PedSpawnLimit_Wanted1 = 8;// 7;// 8;
        PedSpawnLimit_Wanted2 = 9;// 8;// 9;// 9;
        PedSpawnLimit_Wanted3 = 12;// 10;// 12;// 14;
        PedSpawnLimit_Wanted4 = 14;// 13;// 16;// 18;
        PedSpawnLimit_Wanted5 = 15;// 20;// 22;
        PedSpawnLimit_Wanted6 = 17;// 22;// 24;

        PedSpawnLimit_Wanted7 = 17;// 24;// 26;
        PedSpawnLimit_Wanted8 = 17;//24;//26;
        PedSpawnLimit_Wanted9 = 17;//24;//26;
        PedSpawnLimit_Wanted10 = 17;//24;//26;



        VehicleSpawnLimit_Default = 7;// 5;
        VehicleSpawnLimit_Default_Wilderness = 2;
        VehicleSpawnLimit_Default_Rural = 3;
        VehicleSpawnLimit_Default_Suburb = 3;
        VehicleSpawnLimit_Default_Industrial = 4;
        VehicleSpawnLimit_Default_Downtown = 7;// 5;

        VehicleSpawnLimit_Investigation = 6;
        VehicleSpawnLimit_Wanted1 = 8;// 7;// 8;
        VehicleSpawnLimit_Wanted2 = 9;// 7;// 8;// 9;
        VehicleSpawnLimit_Wanted3 = 10;// 9;// 10;// 13;
        VehicleSpawnLimit_Wanted4 = 14;// 12;// 14;// 15;
        VehicleSpawnLimit_Wanted5 = 14;// 13;// 15;// 16;
        VehicleSpawnLimit_Wanted6 = 15;// 16;// 18;

        VehicleSpawnLimit_Wanted7 = 15;// 18;
        VehicleSpawnLimit_Wanted8 = 15;//18;
        VehicleSpawnLimit_Wanted9 = 15;//18;
        VehicleSpawnLimit_Wanted10 = 15;//18;

        BoatSpawnLimit_Default = 1;
        BoatSpawnLimit_Investigation = 1;
        BoatSpawnLimit_Wanted1 = 1;
        BoatSpawnLimit_Wanted2 = 2;
        BoatSpawnLimit_Wanted3 = 3;
        BoatSpawnLimit_Wanted4 = 3;
        BoatSpawnLimit_Wanted5 = 4;
        BoatSpawnLimit_Wanted6 = 4;

        BoatSpawnLimit_Wanted7 = 6;
        BoatSpawnLimit_Wanted8 = 6;
        BoatSpawnLimit_Wanted9 = 6;
        BoatSpawnLimit_Wanted10 = 6;

        HeliSpawnLimit_Default = 0;
        HeliSpawnLimit_Investigation = 0;
        HeliSpawnLimit_Wanted1 = 0;
        HeliSpawnLimit_Wanted2 = 0;
        HeliSpawnLimit_Wanted3 = 1;
        HeliSpawnLimit_Wanted4 = 1;
        HeliSpawnLimit_Wanted5 = 2;
        HeliSpawnLimit_Wanted6 = 2;// 3;// 2;

        HeliSpawnLimit_Wanted7 = 3;//2;
        HeliSpawnLimit_Wanted8 = 3;//2;
        HeliSpawnLimit_Wanted9 = 3;//2;
        HeliSpawnLimit_Wanted10 = 3;//2;




        K9SpawnLimit_Default = 0;
        K9SpawnLimit_Investigation = 0;
        K9SpawnLimit_Wanted1 = 0;
        K9SpawnLimit_Wanted2 = 1;
        K9SpawnLimit_Wanted3 = 2;
        K9SpawnLimit_Wanted4 = 3;
        K9SpawnLimit_Wanted5 = 0;
        K9SpawnLimit_Wanted6 = 0;

        K9SpawnLimit_Wanted7 = 0;
        K9SpawnLimit_Wanted8 = 0;
        K9SpawnLimit_Wanted9 = 0;
        K9SpawnLimit_Wanted10 = 0;

        // AddK9Percentage = 30;



        MaxDistanceToSpawn_WantedSeen = 450f;// 550f;// 650f;//550f
        MaxDistanceToSpawn_WantedUnseen = 350f;// 450f;//350f
        MaxDistanceToSpawn_NotWanted = 750f;// 900f;
        MinDistanceToSpawn_WantedUnseen = 250f;// 350f;//250f
        MinDistanceToSpawn_WantedSeen = 300f;// 400f;// 500f;//400f
        MinDistanceToSpawn_NotWanted = 150f;// 200f;//350f;//150f

        MinDistanceToSpawn_WantedUnseenScalar = 40f;
        MinDistanceToSpawn_WantedSeenScalar = 40f;

        

        TimeBetweenCopSpawn_Unseen = 5000;//3000
        TimeBetweenCopSpawn_Seen_Min = 3000;//2000
        TimeBetweenCopSpawn_Seen_AdditionalTimeScaler = 1000;// 3000;//2000
        TimeBetweenCopDespawn_Unseen = 2000;
        TimeBetweenCopDespawn_Seen_Min = 1000;
        TimeBetweenCopDespawn_Seen_AdditionalTimeScaler = 1000;


        AmbientTimeBetweenSpawn = 8000;// 10000;//10000;
        AmbientTimeBetweenSpawn_DowntownAdditional = 5000;
        AmbientTimeBetweenSpawn_WildernessAdditional = 20000;
        AmbientTimeBetweenSpawn_RuralAdditional = 12000;
        AmbientTimeBetweenSpawn_SuburbAdditional = 10000;
        AmbientTimeBetweenSpawn_IndustrialAdditional = 10000;


        AmbientSpawnPercentage = 70;
        AmbientSpawnPercentage_Wanted = 95;

        AmbientSpawnPercentage_Investigation = 75;
        AmbientSpawnPercentage_Wilderness = 55;// 25;
        AmbientSpawnPercentage_Rural = 65;// 45;
        AmbientSpawnPercentage_Suburb = 75;// 55;
        AmbientSpawnPercentage_Industrial = 75;// 55;
        AmbientSpawnPercentage_Downtown = 90;//70;



        AddOptionalPassengerPercentage = 80f;
       // PedestrianSpawnPercentage = 50f;
        //PercentageSpawnOnFootNearStation = 50;

        LikelyHoodOfAnySpawn_Default = 5;
        LikelyHoodOfCountySpawn_Default = 5;
        LikelyHoodOfAnySpawn_Wanted1 = 5;// 5;
        LikelyHoodOfCountySpawn_Wanted1 = 10;// 5;
        LikelyHoodOfAnySpawn_Wanted2 = 5;
        LikelyHoodOfCountySpawn_Wanted2 = 15;// 5;
        LikelyHoodOfAnySpawn_Wanted3 = 5;// 10;
        LikelyHoodOfCountySpawn_Wanted3 = 25;// 15;
        LikelyHoodOfAnySpawn_Wanted4 = 40;// 5;//20;
        LikelyHoodOfCountySpawn_Wanted4 = 25;// 20;
        LikelyHoodOfAnySpawn_Wanted5 = 40;// 5;//20;
        LikelyHoodOfCountySpawn_Wanted5 = 20;
        LikelyHoodOfAnySpawn_Wanted6 = 40;// 5;//20;
        LikelyHoodOfCountySpawn_Wanted6 = 20;

        LikelyHoodOfAnySpawn_Wanted7 = 5;
        LikelyHoodOfCountySpawn_Wanted7 = 20;
        LikelyHoodOfAnySpawn_Wanted8 = 5;
        LikelyHoodOfCountySpawn_Wanted8 = 20;
        LikelyHoodOfAnySpawn_Wanted9 = 5;
        LikelyHoodOfCountySpawn_Wanted9 = 20;
        LikelyHoodOfAnySpawn_Wanted10 = 5;
        LikelyHoodOfCountySpawn_Wanted10 = 20;

        RemoveNonSpawnedPolice = false;
        RemoveAmbientPolice = true;
        AllowLocationSpawning = true;
        //StationSpawning_MaxWanted = 3;
        // StationSpawningIgnoresLimits = true;



        DistanceToRecallInVehicle_NotWanted = 1000f;
        DistanceToRecallInVehicle_Wanted = 850f;
        DistanceToRecallOnFoot_NotWanted = 300f;
        DistanceToRecallOnFoot_Wanted = 175f;


        FootPatrolSpawnPercentage = 55f;
        TunnelSpawnWantedLevelMinimum = 0;

        AllowHelicopterPassengerRespawn = true;
        HelicopterPassengerRespawnLimit = 2;



        PoliceDispatchDelay_Wilderness = 7000;
        PoliceDispatchDelay_Rural = 5000;
        PoliceDispatchDelay_Suburb = 3000;
        PoliceDispatchDelay_Industrial = 3000;
        PoliceDispatchDelay_Default = 1000;
        PoliceDispatchDelay_Downtown = 0;
        UseScalingLogicForSpawnWhenUnseen = true;

        HeliSpawnDelay_Default = 10000;
        HeliSpawnDelay_Wanted1 = 125000;
        HeliSpawnDelay_Wanted2 = 95000;
        HeliSpawnDelay_Wanted3 = 65000;
        HeliSpawnDelay_Wanted4 = 45000;
        HeliSpawnDelay_Wanted5 = 45000;
        HeliSpawnDelay_Wanted6 = 15000;
        HeliSpawnDelay_Wanted7 = 15000;
        HeliSpawnDelay_Wanted8 = 15000;
        HeliSpawnDelay_Wanted9 = 15000;
        HeliSpawnDelay_Wanted10 = 15000;

        K9SpawnDelay_Default = 90000;
        K9SpawnDelay_Wanted1 = 125000;
        K9SpawnDelay_Wanted2 = 100000;
        K9SpawnDelay_Wanted3 = 85000;
        K9SpawnDelay_Wanted4 = 65000;
        K9SpawnDelay_Wanted5 = 65000;
        K9SpawnDelay_Wanted6 = 65000;
        K9SpawnDelay_Wanted7 = 65000;
        K9SpawnDelay_Wanted8 = 65000;
        K9SpawnDelay_Wanted9 = 65000;
        K9SpawnDelay_Wanted10 = 65000;

        AddAdditionalK9PassengerPercentage = 10;

    }
}