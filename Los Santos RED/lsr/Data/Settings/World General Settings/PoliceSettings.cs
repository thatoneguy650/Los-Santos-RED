using System.ComponentModel;

public class PoliceSettings : ISettingsDefaultable
{
    [Description("Allows mod spawning of police in the world. (Not recommended to disable)")]
    public bool ManageDispatching { get; set; }
    [Description("Allows tasking of ambient police pedestrians in the world. (Not recommended to disable)")]
    public bool ManageTasking { get; set; }
    [Description("Attach a blip to any spawned police pedestrian")]
    public bool ShowSpawnedBlips { get; set; }
    [Description("Show the vanilla police blip")]
    public bool ShowVanillaBlips { get; set; }
    [Description("Enable or disable the non-vanilla wanted system. (Not recommended to disable)")]
    public bool UseFakeWantedLevelSystem { get; set; }
    [Description("If enabled, only LSR will be able to set the wanted level.")]
    public bool TakeExclusiveControlOverWantedLevel { get; set; }
    [Description("If enabled, one star wanted levels not set by the mod will be ignored.")]
    public bool TakeExclusiveControlOverWantedLevelOneStarAndBelow { get; set; }
    [Description("If enabled, any observed crime that results in deadly chase will automatically increase the wanted level to 3.")]
    public bool DeadlyChaseRequiresThreeStars { get; set; }
    [Description("Maximum wanted level allowed. Maximum = 6.")]
    public int MaxWantedLevel { get; set; }






    [Description("Enable or Disable accuracy override")]
    public bool OverrideAccuracy { get; set; }
    [Description("Enable or disable health override")]
    public bool OverrideHealth { get; set; }
    [Description("Enable or disable armor override")]
    public bool OverrideArmor { get; set; }
    [Description("Enable or disable auto load setting. (Not recommended to disable)")]
    public bool ManageLoadout { get; set; }





    [Description("Distance (in meters) that police can see.")]
    public float SightDistance { get; set; }
    [Description("Distance (in meters) that police can hear gunshots.")]
    public float GunshotHearingDistance { get; set; }
    [Description("Additional distance (in meters) that police can see when in a helicopter.")]
    public float SightDistance_Helicopter { get; set; }
    [Description("Additional distance (in meters) that police can see when in a helicopter and you are wanted.")]
    public float SightDistance_Helicopter_AdditionalAtWanted { get; set; }
    [Description("Enable or disable cops knowing your position when you fire a weapon within their hearing distance. Realistic and useful for more cover based shootouts.")]
    public bool KnowsShootingSourceLocation { get; set; }
    [Description("Distance (in meters) that the police will instantly recognize you if you are seen.")]
    public float AutoRecognizeDistance { get; set; }
    [Description("Distance (in meters) that police will instance recongize you even if technically unseen. Useful to stop cops from being obvivious when you are directly behind them. Set to 0 to disable.")]
    public float AlwaysRecognizeDistance { get; set; }
    [Description("Time (in ms) that you will still be considered seen after police have lost sight.")]
    public int RecentlySeenTime { get; set; }
    [Description("Distance (in meters) police need to be within to bust the player.")]
    public float BustDistance { get; set; }
    [Description("Allow cops to use the drive by sight driving flag when chasing.")]
    public bool AllowDriveBySight { get; set; }



    [Description("Fine amount when caught at 1 star with no other serious crimes.")]
    public int GeneralFineAmount { get; set; }
    [Description("Additional fine amount when you are caught at one star and are driving without a valid license.")]
    public int DrivingWithoutLicenseFineAmount { get; set; }
    [Description("Additional fine amount added when you fail at talking your way out of a ticket.")]
    public int TalkFailFineAmount { get; set; }


    [Description("Enable of disable removing all police peds not spawned by LSR.")]
    public bool RemoveNonSpawnedPolice { get; set; }
    [Description("Enable of disable removing all police peds not spawned by LSR that are non persistent.")]
    public bool RemoveAmbientPolice { get; set; }
    [Description("Percentage of time to add optional passenegers to a vehicle. 0 is never 100 is always.")]
    public float AddOptionalPassengerPercentage { get; set; }
    [Description("Percentage of time to spawn foot patrol officers (when possible). 0 is never 100 is always. WIP")]
    public float PedestrianSpawnPercentage { get; set; }





    [Description("Enable or disable chase assists to allow the police to better keep up with the player")]
    public bool AllowChaseAssists { get; set; }
    [Description("Enable or disable clearing or offscreen non-mission vehicles that are blocking police vehicles")]
    public bool AllowFrontVehicleClearAssist { get; set; }
    [Description("Enable or disable collision proffing for police vehicles")]
    public bool AllowReducedCollisionPenaltyAssist { get; set; }
    [Description("Enable or disable increased power for police vehicles")]
    public bool AllowPowerAssist { get; set; }



    [Description("Enable or diable the dynamic roadblock system.")]
    public bool RoadblockEnabled { get; set; }
    [Description("Enable or disable the spike strips for the dynamic roadblocks.")]
    public bool RoadblockSpikeStripsEnabled { get; set; }
    [Description("Minimum wanted level before dynamic roadblocks can spawn.")]
    public int RoadblockMinWantedLevel { get; set; }
    [Description("Maximum level that dynamic roadblocks can spawn at.")]
    public int RoadblockMaxWantedLevel { get; set; }


    [Description("Vehicles to add in front of the initial vehicle.")]
    public int Roadblock_VehiclesToAddFront { get; set; }
    [Description("Vehicles to add behind the initial vehicle.")]
    public int Roadblock_VehiclesToAddRear { get; set; }
    [Description("Barriers to add in front of the initial barrier.")]
    public int Roadblock_BarriersToAddFront { get; set; }
    [Description("Barriers to add behind the initial barrier.")]
    public int Roadblock_BarriersToAddRear { get; set; }




    [Description("Time (in ms) between roadblocks when you are not actively seen by police.")]
    public int TimeBetweenRoadblock_Unseen { get; set; }
    [Description("Minimum time (in ms) between roadblocks.")]
    public int TimeBetweenRoadblock_Seen_Min { get; set; }
    [Description("Decreased time (in ms) scalar between roadblocks as wanted level increases. Formula: ((6 - WantedLevel) * TimeBetweenRoadblock_Seen_AdditionalTimeScaler) + TimeBetweenRoadblock_Seen_Min")]
    public int TimeBetweenRoadblock_Seen_AdditionalTimeScaler { get; set; }




    [Description("Enable or disable wanted level increasing by killing police.")]
    public bool WantedLevelIncreasesByKillingPolice { get; set; }
    [Description("Minimum police killed before being given a four star wanted level.")]
    public int KillLimit_Wanted4 { get; set; }
    [Description("Minimum police killed before being given a five star wanted level.")]
    public int KillLimit_Wanted5 { get; set; }
    [Description("Minimum police killed before being given a six star wanted level.")]
    public int KillLimit_Wanted6 { get; set; }
    [Description("Enable or diable wanted level increasing over time.")]
    public bool WantedLevelIncreasesOverTime { get; set; }
    [Description("Tim (in ms) at wanted level 1 required to increase wanted level to 2.")]
    public uint WantedLevelIncreaseTime_FromWanted1 { get; set; }
    [Description("Tim (in ms) at wanted level 2 required to increase wanted level to 3.")]
    public uint WantedLevelIncreaseTime_FromWanted2 { get; set; }
    [Description("Tim (in ms) at wanted level 3 required to increase wanted level to 4.")]
    public uint WantedLevelIncreaseTime_FromWanted3 { get; set; }
    [Description("Tim (in ms) at wanted level 4 required to increase wanted level to 5.")]
    public uint WantedLevelIncreaseTime_FromWanted4 { get; set; }
    [Description("Tim (in ms) at wanted level 5 required to increase wanted level to 6 (maximum).")]
    public uint WantedLevelIncreaseTime_FromWanted5 { get; set; }





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


    [Description("Maximum police peds that can be spawned when you are not wanted and no investigation is active.")]
    public int PedSpawnLimit_Default { get; set; }
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


    [Description("Maximum police vehicles that can be spawned when you are not wanted and no investigation is active.")]
    public int VehicleSpawnLimit_Default { get; set; }
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






    [Description("Maximum amount of police that can respawn to an investigation when the most serious crime reported is 1 star.")]
    public int InvestigationRespondingOfficers_Wanted1 { get; set; }
    [Description("Maximum amount of police that can respawn to an investigation when the most serious crime reported is 2 star.")]
    public int InvestigationRespondingOfficers_Wanted2 { get; set; }
    [Description("Maximum amount of police that can respawn to an investigation when the most serious crime reported is 3 star.")]
    public int InvestigationRespondingOfficers_Wanted3 { get; set; }
    [Description("Maximum amount of police that can respawn to an investigation when the most serious crime reported is 4 star.")]
    public int InvestigationRespondingOfficers_Wanted4 { get; set; }
    [Description("Maximum amount of police that can respawn to an investigation when the most serious crime reported is 5 star.")]
    public int InvestigationRespondingOfficers_Wanted5 { get; set; }
    [Description("Maximum amount of police that can respawn to an investigation when the most serious crime reported is 6 star.")]
    public int InvestigationRespondingOfficers_Wanted6 { get; set; }


    [Description("Percentage of time to spawn foot patrol police when near a police station. 0 is never, 100 is always. Inactive Setting.")]
    public int PercentageSpawnOnFootNearStation { get; set; }
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
    public int LikelyHoodOfAnySpawn_Wanted6{ get; set; }
    [Description("Percentage of time to allow spawning the county assigned jurisdiction when at 6 stars. Allows the possibility of county agencies to spawn when in a location with zone based jurisdiction. 0 is never 100 is always.")]
    public int LikelyHoodOfCountySpawn_Wanted6 { get; set; }








    [Description("Enable or disable ambient speech from police during chases")]
    public bool AllowAmbientSpeech { get; set; }
    [Description("Minimum time (in ms) between cops speaking when not armed or in a deadly chase.")]
    public int TimeBetweenCopSpeak_General_Min { get; set; }
    [Description("Minimum additional time (in ms) between cops speaking when not armed or in a deadly chase.")]
    public int TimeBetweenCopSpeak_General_Randomizer_Min { get; set; }
    [Description("Maximum additional time (in ms) between cops speaking when not armed or in a deadly chase.")]
    public int TimeBetweenCopSpeak_General_Randomizer_Max { get; set; }
    [Description("Minimum time (in ms) between cops speaking when you are armed.")]
    public int TimeBetweenCopSpeak_Armed_Min { get; set; }
    [Description("Minimum additional time (in ms) between cops speaking when you are armed.")]
    public int TimeBetweenCopSpeak_Armed_Randomizer_Min { get; set; }
    [Description("Maximum additional time (in ms) between cops speaking when you are armed.")]
    public int TimeBetweenCopSpeak_Armed_Randomizer_Max { get; set; }
    [Description("Minimum time (in ms) between cops speaking when in a deadly chase.")]
    public int TimeBetweenCopSpeak_Deadly_Min { get; set; }
    [Description("Minimum additional time (in ms) between cops speaking when in a deadly chase.")]
    public int TimeBetweenCopSpeak_Deadly_Randomizer_Min { get; set; }
    [Description("Maximum additional time (in ms) between cops speaking when in a deadly chase.")]
    public int TimeBetweenCopSpeak_Deadly_Randomizer_Max { get; set; }
    [Description("Minimum time (in ms) between cops speaking when in a weapons free mode.")]
    public int TimeBetweenCopSpeak_WeaponsFree_Min { get; set; }
    [Description("Minimum additional time (in ms) between cops speaking when in weapons free mode.")]
    public int TimeBetweenCopSpeak_WeaponsFree_Randomizer_Min { get; set; }
    [Description("Maximum additional time (in ms) between cops speaking when in weapons free mode.")]
    public int TimeBetweenCopSpeak_WeaponsFree_Randomizer_Max { get; set; }


    [Description("Percentage of cops that have an idea of your location during search mode.")]
    public float SixthSensePercentage { get; set; }
    [Description("Percentage of cops in a helicopter that have an idea of your location during search mode.")]
    public float SixthSenseHelicopterPercentage { get; set; }
    [Description("Percentage of cops that have an idea of your location during search mode and are already close to you when it starts.")]
    public float SixthSensePercentageClose { get; set; }
    [Description("Percentage of search mode that cops will be able to use their sixth sense. A value of 0.7 means they would be able to use their sixth sense powers for the first 30% of search mode (1.0 is none, 0.0 is the entire search mode).")]
    public float SixthSenseSearchModeLimitPercentage { get; set; }


    [Description("Enables or disables siege mode when you are in a building the police saw you enter. Allows police to keep an active search mode when they cannot directly see you.")]
    public bool AllowSiegeMode { get; set; }
    [Description("When in siege mode, how close the entry team will attempt to get to the player.")]
    public float SiegeGotoDistance { get; set; }
    [Description("When in siege mode, how close the entry team will attempt to get before aiming at the player.")]
    public float SiegeAimDistance { get; set; }
    [Description("Percentage of cops that will be set to siege mode. IF in siege mode, the cop will be forced to attempt entry. If not in siege mode cops may or may not attempt entry depending on vanilla AI.")]
    public float SiegePercentage { get; set; }





    [Description("Enable or disable ambient spawns around police stations.")]
    public bool AllowStationSpawning { get; set; }
    [Description("Maximum wanted level to spawn ambient peds and vehicles around the station.")]
    public int StationSpawning_MaxWanted { get; set; }
    [Description("If enabled, police stations ambient spawns ignore the ped and vehicle spawn limits. Use with StationSpawning_MaxWanted of 1 or 2 to allow ambient spawning, but not overload ped limits when in a serious chase.")]
    public bool StationSpawningIgnoresLimits { get; set; }
    [Description("Enable or disable dropping your current weapon when busted and armed.")]
    public bool DropWeaponWhenBusted { get; set; }



    public bool AllowRespondingWithoutCallIn { get; set; }

    public PoliceSettings()
    {
        SetDefault();


    }
    public void SetDefault()
    {
        GeneralFineAmount = 500;
        TalkFailFineAmount = 500;
        DrivingWithoutLicenseFineAmount = 1000;
        RoadblockEnabled = true;
        RoadblockSpikeStripsEnabled = true;

        Roadblock_VehiclesToAddFront = 2;
        Roadblock_VehiclesToAddRear = 2;
        Roadblock_BarriersToAddFront = 3;
        Roadblock_BarriersToAddRear = 3;


        RoadblockMinWantedLevel = 3;
        RoadblockMaxWantedLevel = 5;
        TimeBetweenRoadblock_Unseen = 999999;
        TimeBetweenRoadblock_Seen_Min = 120000;
        TimeBetweenRoadblock_Seen_AdditionalTimeScaler = 30000;
        ManageDispatching = true;
        ManageTasking = true;
        TakeExclusiveControlOverWantedLevel = true;
        TakeExclusiveControlOverWantedLevelOneStarAndBelow = false;
        ShowSpawnedBlips = false;
        ShowVanillaBlips = false;
        OverrideAccuracy = true;
        AutoRecognizeDistance = 15f;
        AlwaysRecognizeDistance = 7f;
        RecentlySeenTime = 15000;
        AllowAmbientSpeech = true;
        AllowChaseAssists = true;
        AllowFrontVehicleClearAssist = true;
        AllowReducedCollisionPenaltyAssist = true;
        AllowPowerAssist = true;




        ManageLoadout = true;
        BustDistance = 4f;//5f;
        OverrideHealth = true;
        PedSpawnLimit_Default = 7;
        PedSpawnLimit_Investigation = 8;
        PedSpawnLimit_Wanted1 = 9;
        PedSpawnLimit_Wanted2 = 10;
        PedSpawnLimit_Wanted3 = 16;
        PedSpawnLimit_Wanted4 = 20;
        PedSpawnLimit_Wanted5 = 24;
        PedSpawnLimit_Wanted6 = 26;
        VehicleSpawnLimit_Default = 9;// 6;
        VehicleSpawnLimit_Investigation = 10;// 7;
        VehicleSpawnLimit_Wanted1 = 11;// 8;
        VehicleSpawnLimit_Wanted2 = 12;// 9;
        VehicleSpawnLimit_Wanted3 = 13;
        VehicleSpawnLimit_Wanted4 = 15;
        VehicleSpawnLimit_Wanted5 = 16;
        VehicleSpawnLimit_Wanted6 = 18;
        WantedLevelIncreasesByKillingPolice = true;
        KillLimit_Wanted4 = 5;
        KillLimit_Wanted5 = 10;
        KillLimit_Wanted6 = 20;

        WantedLevelIncreasesOverTime = true;
        WantedLevelIncreaseTime_FromWanted1 = 90000;//1.5 min
        WantedLevelIncreaseTime_FromWanted2 = 180000;//3 mins
        WantedLevelIncreaseTime_FromWanted3 = 270000;//4.5 mins
        WantedLevelIncreaseTime_FromWanted4 = 360000;//6 minutes
        WantedLevelIncreaseTime_FromWanted5 = 600000;//10 minutes

        SightDistance = 90f;//70f;
        GunshotHearingDistance = 125f;
        SightDistance_Helicopter = 175f;
        SightDistance_Helicopter_AdditionalAtWanted = 100f;
        MaxDistanceToSpawn_WantedSeen = 550f;
        MaxDistanceToSpawn_WantedUnseen = 350f;
        MaxDistanceToSpawn_NotWanted = 900f;
        MinDistanceToSpawn_WantedUnseen = 250f;
        MinDistanceToSpawn_WantedSeen = 400f;
        MinDistanceToSpawn_NotWanted = 150f;//350f;
        TimeBetweenCopSpawn_Unseen = 3000;
        TimeBetweenCopSpawn_Seen_Min = 2000;
        TimeBetweenCopSpawn_Seen_AdditionalTimeScaler = 2000;
        TimeBetweenCopDespawn_Unseen = 2000;
        TimeBetweenCopDespawn_Seen_Min = 1000;
        TimeBetweenCopDespawn_Seen_AdditionalTimeScaler = 1000;
        DeadlyChaseRequiresThreeStars = true;
        MaxWantedLevel = 6;
        AddOptionalPassengerPercentage = 75f;
        KnowsShootingSourceLocation = true;
        UseFakeWantedLevelSystem = true;
        PedestrianSpawnPercentage = 50f;

        InvestigationRespondingOfficers_Wanted1 = 2;
        InvestigationRespondingOfficers_Wanted2 = 4;
        InvestigationRespondingOfficers_Wanted3 = 6;
        InvestigationRespondingOfficers_Wanted4 = 8;
        InvestigationRespondingOfficers_Wanted5 = 10;
        InvestigationRespondingOfficers_Wanted6 = 14;

        PercentageSpawnOnFootNearStation = 50;

        LikelyHoodOfAnySpawn_Default = 5;
        LikelyHoodOfCountySpawn_Default = 5;

        LikelyHoodOfAnySpawn_Wanted1 = 5;// 5;
        LikelyHoodOfCountySpawn_Wanted1 = 5;

        LikelyHoodOfAnySpawn_Wanted2 = 5;
        LikelyHoodOfCountySpawn_Wanted2 = 5;

        LikelyHoodOfAnySpawn_Wanted3 = 5;// 10;
        LikelyHoodOfCountySpawn_Wanted3 = 10;

        LikelyHoodOfAnySpawn_Wanted4 = 5;//20;
        LikelyHoodOfCountySpawn_Wanted4 = 20;

        LikelyHoodOfAnySpawn_Wanted5 = 5;//20;
        LikelyHoodOfCountySpawn_Wanted5 = 20;

        LikelyHoodOfAnySpawn_Wanted6 = 5;//20;
        LikelyHoodOfCountySpawn_Wanted6 = 20;

        RemoveNonSpawnedPolice = false;
        RemoveAmbientPolice = false;

        TimeBetweenCopSpeak_Armed_Min = 10000;
        TimeBetweenCopSpeak_Armed_Randomizer_Min = 0;
        TimeBetweenCopSpeak_Armed_Randomizer_Max = 5000;


        TimeBetweenCopSpeak_General_Min = 20000;
        TimeBetweenCopSpeak_General_Randomizer_Min = 0;
        TimeBetweenCopSpeak_General_Randomizer_Max = 13000;


        TimeBetweenCopSpeak_Deadly_Min = 15000;
        TimeBetweenCopSpeak_Deadly_Randomizer_Min = 0;
        TimeBetweenCopSpeak_Deadly_Randomizer_Max = 8000;

        TimeBetweenCopSpeak_WeaponsFree_Min = 10000;
        TimeBetweenCopSpeak_WeaponsFree_Randomizer_Min = 0;
        TimeBetweenCopSpeak_WeaponsFree_Randomizer_Max = 5000;

        SixthSensePercentage = 70f;
        SixthSenseHelicopterPercentage = 90f;
        SixthSenseSearchModeLimitPercentage = 0.7f;

        AllowDriveBySight = false;
        AllowPowerAssist = false;


#if DEBUG
        ShowSpawnedBlips = true;
#else
#endif

        AllowSiegeMode = true;
        SiegeGotoDistance = 8f;
        SiegeAimDistance = 15f;
        SiegePercentage = 80f;

        AllowStationSpawning = true;
        StationSpawning_MaxWanted = 2;

        StationSpawningIgnoresLimits = true;

        DropWeaponWhenBusted = true;

        AllowRespondingWithoutCallIn = true;


    }
}