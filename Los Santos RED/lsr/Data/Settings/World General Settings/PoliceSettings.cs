using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PoliceSettings : ISettingsDefaultable
{
    public int GeneralFineAmount { get; set; }
    public bool RoadblockEnabled { get; set; }
    public bool RoadblockSpikeStripsEnabled { get; set; }
    public int RoadblockMinWantedLevel { get; set; }
    public int RoadblockMaxWantedLevel { get; set; }
    public int TimeBetweenRoadblock_Unseen { get; set; }
    public int TimeBetweenRoadblock_Seen_Min { get; set; }
    public int TimeBetweenRoadblock_Seen_AdditionalTimeScaler { get; set; }
    public bool ManageDispatching { get; set; }
    public bool ManageTasking { get; set; }
    public bool TakeExclusiveControlOverWantedLevel { get; set; }
    public bool TakeExclusiveControlOverWantedLevelOneStarAndBelow { get; set; }
    public bool ShowSpawnedBlips { get; set; }
    public bool ShowVanillaBlips { get; set; }
    public bool OverrideAccuracy { get; set; }
    public int GeneralCombatAbility { get; set; }
    public int GeneralAccuracy { get; set; }
    public int TaserAccuracy { get; set; }
    public int VehicleAccuracy { get; set; }
    public int GeneralShootRate { get; set; }
    public int TaserShootRate { get; set; } 
    public int VehicleShootRate { get; set; }
    public float AutoRecognizeDistance { get; set; }
    public float AlwaysRecognizeDistance { get; set; }
    public int RecentlySeenTime { get; set; }
    public bool AllowAmbientSpeech { get; set; }
    public bool AllowChaseAssists { get; set; }
    public bool ManageLoadout { get; set; }
    public float BustDistance { get; set; }
    public bool AllowRadioInAnimations { get; set; }
    public bool OverrideHealth { get; set; }
    public int MinHealth { get; set; }
    public int MaxHealth { get; set; }
    public bool OverrideArmor { get; set; }
    public int MinArmor { get; set; }
    public int MaxArmor { get; set; }
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
    public bool WantedLevelIncreasesByKillingPolice { get; set; }
    public int KillLimit_Wanted4 { get; set; }
    public int KillLimit_Wanted5 { get; set; }
    public int KillLimit_Wanted6 { get; set; }
    public bool WantedLevelIncreasesOverTime { get; set; }
    public uint WantedLevelIncreaseTime { get; set; }
    public float SightDistance { get; set; }
    public float GunshotHearingDistance { get; set; }
    public float SightDistance_Helicopter { get; set; }
    public float SightDistance_Helicopter_AdditionalAtWanted { get; set; }
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
    public bool DeadlyChaseRequiresThreeStars { get; set; }
    public int MaxWantedLevel { get; set; }
    public float AddOptionalPassengerPercentage { get; set; }
    public bool KnowsShootingSourceLocation { get; set; }
    public PoliceSettings()
    {
        SetDefault();
        #if DEBUG
                ShowSpawnedBlips = true;
                ShowVanillaBlips = false;
        #else
                       // ShowSpawnedBlips = false;
        #endif
            
    }
    public void SetDefault()
    {
        GeneralFineAmount = 500;
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
        GeneralCombatAbility = 1;
        GeneralAccuracy = 40;
        TaserAccuracy = 30;
        VehicleAccuracy = 10;
        GeneralShootRate = 500;//even
        TaserShootRate = 100;
        VehicleShootRate = 20;
        AutoRecognizeDistance = 15f;
        AlwaysRecognizeDistance = 7f;
        RecentlySeenTime = 15000;
        AllowAmbientSpeech = true;
        AllowChaseAssists = true;
        ManageLoadout = true;
        BustDistance = 4f;//5f;
        AllowRadioInAnimations = false;
        OverrideHealth = true;
        MinHealth = 85;
        MaxHealth = 125;
        OverrideArmor = true;
        MinArmor = 0;
        MaxArmor = 50;
        PedSpawnLimit_Default = 5;
        PedSpawnLimit_Investigation = 6;
        PedSpawnLimit_Wanted1 = 7;//7;
        PedSpawnLimit_Wanted2 = 9;//9;
        PedSpawnLimit_Wanted3 = 14;//13;
        PedSpawnLimit_Wanted4 = 18;
        PedSpawnLimit_Wanted5 = 22;
        PedSpawnLimit_Wanted6 = 24;
        VehicleSpawnLimit_Default = 5;
        VehicleSpawnLimit_Investigation = 6;
        VehicleSpawnLimit_Wanted1 = 6;
        VehicleSpawnLimit_Wanted2 = 8;
        VehicleSpawnLimit_Wanted3 = 12;
        VehicleSpawnLimit_Wanted4 = 14;
        VehicleSpawnLimit_Wanted5 = 16;
        VehicleSpawnLimit_Wanted6 = 18;
        WantedLevelIncreasesByKillingPolice = true;
        KillLimit_Wanted4 = 4;
        KillLimit_Wanted5 = 12;
        KillLimit_Wanted6 = 20;
        WantedLevelIncreasesOverTime = true;
        WantedLevelIncreaseTime = 240000;
        SightDistance = 90f;//70f;
        GunshotHearingDistance = 125f;
        SightDistance_Helicopter = 175f;
        SightDistance_Helicopter_AdditionalAtWanted = 100f;
        MaxDistanceToSpawn_WantedSeen = 550f;
        MaxDistanceToSpawn_WantedUnseen = 350f;
        MaxDistanceToSpawn_NotWanted = 900f;
        MinDistanceToSpawn_WantedUnseen = 250f;
        MinDistanceToSpawn_WantedSeen = 400f;
        MinDistanceToSpawn_NotWanted = 350f;
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
    }
}