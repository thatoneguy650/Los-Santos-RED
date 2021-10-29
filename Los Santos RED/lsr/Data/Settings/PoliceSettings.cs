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
    public int MinTimeBetweenRoadblocks { get; set; } = 120000;
    public int RoadblockWantedLevelAdditionalTimeScaler { get; set; } = 30000;
    public bool ManageDispatching { get; set; } = true;
    public bool ManageTasking { get; set; } = true;
    public bool AllowExclusiveControlOverWantedLevel { get; set; } = true;
    public bool ShowSpawnedBlips { get; set; } = false;
    public bool ShowVanillaBlips { get; set; } = false;
    public bool OverrideAccuracy { get; set; } = true;
    public int GeneralAccuracy { get; set; } = 8;//10;
    public float AutoRecognizeDistance { get; set; } = 15f;//20f
    public float AlwaysRecognizeDistance { get; set; } = 7f;
    public int RecentlySeenTime { get; set; } = 10000;//17000
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
    public int SpawnLimit_Default { get; set; } = 5;
    public int SpawnLimit_Investigation { get; set; } = 6;
    public int SpawnLimit_Wanted1 { get; set; } = 7;
    public int SpawnLimit_Wanted2 { get; set; } = 10;
    public int SpawnLimit_Wanted3 { get; set; } = 18;
    public int SpawnLimit_Wanted4 { get; set; } = 25;
    public int SpawnLimit_Wanted5 { get; set; } = 35;
    public int KillLimit_Wanted4 { get; set; } = 4;
    public int KillLimit_Wanted5 { get; set; } = 10;
    public uint WantedLevelIncreaseTime { get; set; } = 240000;
    public float SightDistance { get; set; } = 70f;//90f
    public float GunshotHearingDistance { get; set; } = 80f;//100f
    public float SightDistance_Helicopter { get; set; } = 150f;
    public float SightDistance_Helicopter_AdditionalAtWanted { get; set; } = 100f;
    public PoliceSettings()
    {
        #if DEBUG
                    ShowSpawnedBlips =  true;
        #else
                ShowSpawnedBlips = false;
        #endif
    }
}