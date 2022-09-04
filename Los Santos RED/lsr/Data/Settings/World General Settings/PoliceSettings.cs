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

    [Description("Enable or Disable accuracy override")]
    public bool OverrideAccuracy { get; set; }
    [Description("Enable or disable health override")]
    public bool OverrideHealth { get; set; }
    [Description("Enable or disable armor override")]
    public bool OverrideArmor { get; set; }

    [Description("Enable or disable auto load setting. (Not recommended to disable)")]
    public bool ManageLoadout { get; set; }
    [Description("Enable or disable ambient speech from police during chases")]
    public bool AllowAmbientSpeech { get; set; }
    [Description("Enable or disable chase assists to allow the police to better keep up with the player")]
    public bool AllowChaseAssists { get; set; }
    [Description("Enable or disable clearing or offscreen non-mission vehicles that are blocking police vehicles")]
    public bool AllowFrontVehicleClearAssist { get; set; }
    [Description("Enable or disable collision proffing for police vehicles")]
    public bool AllowReducedCollisionPenaltyAssist { get; set; }
    [Description("Enable or disable increased power for police vehicles")]
    public bool AllowPowerAssist { get; set; }


    [Description("Enable or disable the non-vanilla wanted system. (Not recommended to disable)")]
    public bool UseFakeWantedLevelSystem { get; set; }
    [Description("If enabled, only LSR will be able to set the wanted level.")]
    public bool TakeExclusiveControlOverWantedLevel { get; set; }
    [Description("If enabled, one star wanted levels not set by the mod will be ignored.")]
    public bool TakeExclusiveControlOverWantedLevelOneStarAndBelow { get; set; }










    public bool RoadblockEnabled { get; set; }
    public bool RoadblockSpikeStripsEnabled { get; set; }
    public int RoadblockMinWantedLevel { get; set; }
    public int RoadblockMaxWantedLevel { get; set; }
    public int TimeBetweenRoadblock_Unseen { get; set; }
    public int TimeBetweenRoadblock_Seen_Min { get; set; }
    public int TimeBetweenRoadblock_Seen_AdditionalTimeScaler { get; set; }





    public bool WantedLevelIncreasesByKillingPolice { get; set; }
    public int KillLimit_Wanted4 { get; set; }
    public int KillLimit_Wanted5 { get; set; }
    public int KillLimit_Wanted6 { get; set; }
    public bool WantedLevelIncreasesOverTime { get; set; }
   // public uint WantedLevelIncreaseTime { get; set; }


    public uint WantedLevelIncreaseTime_FromWanted1 { get; set; }
    public uint WantedLevelIncreaseTime_FromWanted2 { get; set; }
    public uint WantedLevelIncreaseTime_FromWanted3 { get; set; }
    public uint WantedLevelIncreaseTime_FromWanted4 { get; set; }
    public uint WantedLevelIncreaseTime_FromWanted5 { get; set; }


    public bool DeadlyChaseRequiresThreeStars { get; set; }
    public int MaxWantedLevel { get; set; }



    public float SightDistance { get; set; }
    public float GunshotHearingDistance { get; set; }
    public float SightDistance_Helicopter { get; set; }
    public float SightDistance_Helicopter_AdditionalAtWanted { get; set; }
    public bool KnowsShootingSourceLocation { get; set; }
    public float AutoRecognizeDistance { get; set; }
    public float AlwaysRecognizeDistance { get; set; }
    public int RecentlySeenTime { get; set; }
    public float BustDistance { get; set; }



    public float MaxDistanceToSpawn_WantedSeen { get; set; }
    public float MaxDistanceToSpawn_WantedUnseen { get; set; }
    public float MaxDistanceToSpawn_NotWanted { get; set; }
    public float MinDistanceToSpawn_WantedUnseen { get; set; }
    public float MinDistanceToSpawn_WantedSeen { get; set; }
    public float MinDistanceToSpawn_NotWanted { get; set; }


    public int TimeBetweenCopSpawn_Unseen { get; set; }
    public int TimeBetweenCopSpawn_Seen_Min { get; set; }
    public int TimeBetweenCopSpawn_Seen_AdditionalTimeScaler { get; set; }
    public int TimeBetweenCopDespawn_Unseen { get; set; }
    public int TimeBetweenCopDespawn_Seen_Min { get; set; }
    public int TimeBetweenCopDespawn_Seen_AdditionalTimeScaler { get; set; }


    public int PedSpawnLimit_Default { get; set; }
    public int PedSpawnLimit_Investigation { get; set; }
    public int PedSpawnLimit_Wanted1 { get; set; }
    public int PedSpawnLimit_Wanted2 { get; set; }
    public int PedSpawnLimit_Wanted3 { get; set; }
    public int PedSpawnLimit_Wanted4 { get; set; }
    public int PedSpawnLimit_Wanted5 { get; set; }
    public int PedSpawnLimit_Wanted6 { get; set; }
    public int VehicleSpawnLimit_Default { get; set; }
    public int VehicleSpawnLimit_Investigation { get; set; }
    public int VehicleSpawnLimit_Wanted1 { get; set; }
    public int VehicleSpawnLimit_Wanted2 { get; set; }
    public int VehicleSpawnLimit_Wanted3 { get; set; }
    public int VehicleSpawnLimit_Wanted4 { get; set; }
    public int VehicleSpawnLimit_Wanted5 { get; set; }
    public int VehicleSpawnLimit_Wanted6 { get; set; }




    public float AddOptionalPassengerPercentage { get; set; }
    public float PedestrianSpawnPercentage { get; set; }
    public int GeneralFineAmount { get; set; }
    public int DrivingWithoutLicenseFineAmount { get; set; }


    public int InvestigationRespondingOfficers_Wanted1 { get; set; }
    public int InvestigationRespondingOfficers_Wanted2 { get; set; }
    public int InvestigationRespondingOfficers_Wanted3 { get; set; }
    public int InvestigationRespondingOfficers_Wanted4 { get; set; }
    public int InvestigationRespondingOfficers_Wanted5 { get; set; }
    public int InvestigationRespondingOfficers_Wanted6 { get; set; }


    public int PercentageSpawnOnFootNearStation { get; set; }
    public int LikelyHoodOfAnySpawn_Default { get; set; }
    public int LikelyHoodOfCountySpawn_Default { get; set; }

    public int LikelyHoodOfAnySpawn_Wanted1 { get; set; }
    public int LikelyHoodOfCountySpawn_Wanted1 { get; set; }

    public int LikelyHoodOfAnySpawn_Wanted2 { get; set; }
    public int LikelyHoodOfCountySpawn_Wanted2 { get; set; }

    public int LikelyHoodOfAnySpawn_Wanted3 { get; set; }
    public int LikelyHoodOfCountySpawn_Wanted3 { get; set; }

    public int LikelyHoodOfAnySpawn_Wanted4 { get; set; }
    public int LikelyHoodOfCountySpawn_Wanted4 { get; set; }

    public int LikelyHoodOfAnySpawn_Wanted5 { get; set; }
    public int LikelyHoodOfCountySpawn_Wanted5 { get; set; }

    public int LikelyHoodOfAnySpawn_Wanted6{ get; set; }
    public int LikelyHoodOfCountySpawn_Wanted6 { get; set; }
    public bool RemoveVanillaSpawnedPeds { get; set; }




    public int TimeBetweenCopSpeak_Armed_Min { get; set; }
    public int TimeBetweenCopSpeak_Armed_Randomizer_Min { get; set; }
    public int TimeBetweenCopSpeak_Armed_Randomizer_Max { get; set; }
    public int TimeBetweenCopSpeak_General_Min { get; set; }
    public int TimeBetweenCopSpeak_General_Randomizer_Min { get; set; }
    public int TimeBetweenCopSpeak_General_Randomizer_Max { get; set; }
    public int TimeBetweenCopSpeak_Deadly_Min { get; set; }
    public int TimeBetweenCopSpeak_Deadly_Randomizer_Min { get; set; }
    public int TimeBetweenCopSpeak_Deadly_Randomizer_Max { get; set; }
    public int TimeBetweenCopSpeak_WeaponsFree_Min { get; set; }
    public int TimeBetweenCopSpeak_WeaponsFree_Randomizer_Min { get; set; }
    public int TimeBetweenCopSpeak_WeaponsFree_Randomizer_Max { get; set; }


    [Description("Percentage of cops that have an idea of your location during search mode.")]
    public float SixthSensePercentage { get; set; }
    [Description("Percentage of cops in a helicopter that have an idea of your location during search mode.")]
    public float SixthSenseHelicopterPercentage { get; set; }

    [Description("Percentage of cops that have an idea of your location during search mode and are already close to you when it starts.")]
    public float SixthSensePercentageClose { get; set; }

    [Description("Percentage of search mode that cops will be able to use their sixth sense. A value of 0.7 means they would be able to use their sixth sense powers for the first 30% of search mode (1.0 is none, 0.0 is the entire search mode).")]
    public float SixthSenseSearchModeLimitPercentage { get; set; }
    public bool AllowDriveBySight { get; set; }
    public bool AllowSiegeMode { get; set; }
    public float SiegeGotoDistance { get; set; }
    public float SiegeAimDistance { get; set; }
    public float SiegePercentage { get; set; }

    public PoliceSettings()
    {
        SetDefault();


    }
    public void SetDefault()
    {
        GeneralFineAmount = 500;
        DrivingWithoutLicenseFineAmount = 1000;
        RoadblockEnabled = true;
        RoadblockSpikeStripsEnabled = true;
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

        RemoveVanillaSpawnedPeds = false;

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
        ShowVanillaBlips = false;
#else
#endif

        AllowSiegeMode = true;
        SiegeGotoDistance = 8f;
        SiegeAimDistance = 15f;
        SiegePercentage = 80f;


    }
}