using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PoliceSettings
{
    public int GeneralFineAmount { get; set; } = 500;
    public int RoadblockMinWantedLevel { get; set; } = 3;
    public int RoadblockMaxWantedLevel { get; set; } = 5;
    public int TimeBetweenRoadblock_Unseen { get; set; } = 999999;
    public int TimeBetweenRoadblock_Seen_Min { get; set; } = 120000;
    public int TimeBetweenRoadblock_Seen_AdditionalTimeScaler { get; set; } = 30000;
    public bool ManageDispatching { get; set; } = true;
    public bool ManageTasking { get; set; } = true;
    public bool TakeExclusiveControlOverWantedLevel { get; set; } = true;
    public bool TakeExclusiveControlOverWantedLevelOneStarAndBelow { get; set; } = false;
    public bool ShowSpawnedBlips { get; set; } = false;
    public bool ShowVanillaBlips { get; set; } = false;
    public bool OverrideAccuracy { get; set; } = true;
    public int GeneralAccuracy { get; set; } = 55;//15
    public float AutoRecognizeDistance { get; set; } = 15f;
    public float AlwaysRecognizeDistance { get; set; } = 7f;
    public int RecentlySeenTime { get; set; } = 15000;
    public bool AllowAmbientSpeech { get; set; } = true;
    public bool AllowChaseAssists { get; set; } = true;
    public bool ManageLoadout { get; set; } = true;
    public float BustDistance { get; set; } = 4f;//5f;
    public bool AllowRadioInAnimations { get; set; } = false;
    public bool OverrideHealth { get; set; } = true;
    public int MinHealth { get; set; } = 85;
    public int MaxHealth { get; set; } = 125;
    public bool OverrideArmor { get; set; } = true;
    public int MinArmor { get; set; } = 0;
    public int MaxArmor { get; set; } = 50;
    public int PedSpawnLimit_Default { get; set; } = 5;
    public int PedSpawnLimit_Investigation { get; set; } = 6;
    public int PedSpawnLimit_Wanted1 { get; set; } = 8;
    public int PedSpawnLimit_Wanted2 { get; set; } = 10;
    public int PedSpawnLimit_Wanted3 { get; set; } = 18;
    public int PedSpawnLimit_Wanted4 { get; set; } = 25;
    public int PedSpawnLimit_Wanted5 { get; set; } = 35;
    public int PedSpawnLimit_Wanted6 { get; set; } = 40;
    public int VehicleSpawnLimit_Default { get; set; } = 5;
    public int VehicleSpawnLimit_Investigation { get; set; } = 6;
    public int VehicleSpawnLimit_Wanted1 { get; set; } = 8;
    public int VehicleSpawnLimit_Wanted2 { get; set; } = 10;
    public int VehicleSpawnLimit_Wanted3 { get; set; } = 15;
    public int VehicleSpawnLimit_Wanted4 { get; set; } = 18;
    public int VehicleSpawnLimit_Wanted5 { get; set; } = 20;
    public int VehicleSpawnLimit_Wanted6 { get; set; } = 22;
    public bool WantedLevelIncreasesByKillingPolice { get; set; } = true;
    public int KillLimit_Wanted4 { get; set; } = 4;
    public int KillLimit_Wanted5 { get; set; } = 8;
    public int KillLimit_Wanted6 { get; set; } = 12;
    public bool WantedLevelIncreasesOverTime { get; set; } = true;
    public uint WantedLevelIncreaseTime { get; set; } = 240000;
    public float SightDistance { get; set; } = 90f;//70f;
    public float GunshotHearingDistance { get; set; } = 125f;
    public float SightDistance_Helicopter { get; set; } = 175f;
    public float SightDistance_Helicopter_AdditionalAtWanted { get; set; } = 100f;
    public float MaxDistanceToSpawn_WantedSeen { get; set; } = 550f;
    public float MaxDistanceToSpawn_WantedUnseen { get; set; } = 350f;
    public float MaxDistanceToSpawn_NotWanted { get; set; } = 900f;
    public float MinDistanceToSpawn_WantedUnseen { get; set; } = 250f;
    public float MinDistanceToSpawn_WantedSeen { get; set; } = 400f;
    public float MinDistanceToSpawn_NotWanted { get; set; } = 350f;
    public int TimeBetweenCopSpawn_Unseen { get; set; } = 3000;
    public int TimeBetweenCopSpawn_Seen_Min { get; set; } = 2000;
    public int TimeBetweenCopSpawn_Seen_AdditionalTimeScaler { get; set; } = 2000;
    public bool DeadlyChaseRequiresThreeStars { get; set; } = true;
    public int MaxWantedLevel { get; set; } = 6;


    public PoliceSettings()
    {
#if DEBUG
        ShowSpawnedBlips = true;
        ShowVanillaBlips = false;
#else
               // ShowSpawnedBlips = false;
#endif
    }
}