using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
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
    public bool RemoveNonSpawnedGangMembers { get; set; }
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
    [Description("Minimum distance in meters to spawn a pedestrian gang member from the player.")]
    public float MinDistanceToSpawnOnFoot { get; set; }
    [Description("Maximum distance in meters to spawn a pedestrian gang member from the player.")]
    public float MaxDistanceToSpawnOnFoot { get; set; }
    [Description("Minimum distance in meters to spawn a gang member in a vehicle from the player.")]
    public float MinDistanceToSpawnInVehicle { get; set; }
    [Description("Maximum distance in meters to spawn a gang member in a vehicle from the player.")]
    public float MaxDistanceToSpawnInVehicle { get; set; }
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
    public float AmbientSpawnPedestrianAttemptPercentage { get; set; }
    [Description("Percentage of the time to spawn a gang outside of the den when near.")]
    public int PercentageSpawnNearDen { get; set; }
    [Description("Attempt to remove all non-mod spawned on-foot gang members from the world. (Not Currently Recommended)")]
    public bool RemoveNonSpawnedGangMembersOnFoot { get; set; }
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
   // public float EscortSpeed { get; set; }


    public float EscortSpeedFast { get; set; }
    public float EscortSpeedNormal { get; set; }

    public float DistanceToReportRepChanges { get; set; }
    public uint GameTimeToReportRepChanges { get; set; }
    public uint GameTimeToReportRepChangesInTerritory { get; set; }
    public float MurderDistance { get; set; }
    public uint GameTimeRecentlyKilled { get; set; }
    public float MinDistanceToReportTimeoutRepChanges { get; set; }
    public bool AllowHitSquads { get; set; }

    public bool AllowHitSquadsOnlyEnemy { get; set; }

    public uint MinTimeBetweenHitSquads { get; set; }
    public uint MaxTimeBetweenHitSquads { get; set; }


   // [Description("Maximum gang boats that can be spawned.")]
    //public int BoatSpawnLimit_Default { get; set; }

    [Description("Maximum gang helicopters that can be spawned.")]
    public int HeliSpawnLimit_Default { get; set; }
    public bool SendHitSquadText { get; set; }


    public bool ShowGangTerritoryBlip { get; set; }
    public float GangTerritoryBlipSize { get; set; }
    public float GangTerritoryBlipAlpha { get; set; }
    public bool SetPersistent { get; set; }


    [Description("Enable or disable ambient speech from gang members")]
    public bool AllowAmbientSpeech { get; set; }
    [Description("Minimum time (in ms) between gang members speaking when idle.")]
    public int TimeBetweenGangSpeak_General_Min { get; set; }
    [Description("Minimum additional time (in ms) between gang members speaking when idle.")]
    public int TimeBetweenGangSpeak_General_Randomizer_Min { get; set; }
    [Description("Maximum additional time (in ms) between gang members speaking when idle.")]
    public int TimeBetweenGangSpeak_General_Randomizer_Max { get; set; }




    [Description("Minimum time (in ms) between gang members speaking when you are armed.")]
    public int TimeBetweenGangSpeak_Fleeing_Min { get; set; }
    [Description("Minimum additional time (in ms) between gang members speaking when you are armed.")]
    public int TimeBetweenGangSpeak_Fleeing_Randomizer_Min { get; set; }
    [Description("Maximum additional time (in ms) between gang members speaking when you are armed.")]
    public int TimeBetweenGangSpeak_Fleeing_Randomizer_Max { get; set; }
    [Description("Minimum time (in ms) between gang members speaking when in a deadly chase.")]
    public int TimeBetweenGangSpeak_LowCombat_Min { get; set; }
    [Description("Minimum additional time (in ms) between gang members speaking when in a deadly chase.")]
    public int TimeBetweenGangSpeak_LowCombat_Randomizer_Min { get; set; }
    [Description("Maximum additional time (in ms) between gang members speaking when in a deadly chase.")]
    public int TimeBetweenGangSpeak_LowCombat_Randomizer_Max { get; set; }
    [Description("Minimum time (in ms) between gang members speaking when in a weapons free mode.")]
    public int TimeBetweenGangSpeak_HighCombat_Min { get; set; }
    [Description("Minimum additional time (in ms) between gang members speaking when in weapons free mode.")]
    public int TimeBetweenGangSpeak_HighCombat_Randomizer_Min { get; set; }
    [Description("Maximum additional time (in ms) between gang members speaking when in weapons free mode.")]
    public int TimeBetweenGangSpeak_HighCombat_Randomizer_Max { get; set; }

    public int RepDeductedKilled { get; set; }
    [Description("Reputation deducted when killing an enemy Gang Member.")]
    public int RepDeductedKilledTerritory { get; set; }
    [Description("Additional reputation deducted if an enemy Gang Member is killed in their territory.")]
    public int RepDeductedInjured { get; set; }
    [Description("Reputation deducted when injuring an enemy Gang Member.")]
    public int RepDeductedInjuredTerritory { get; set; }
    [Description("Additional reputation deducted if an enemy Gang Member is injured in their territory.")]
    public int RepDeductedCarjacked { get; set; }
    [Description("Reputation deducted when carjacking an enemy Gang Member.")]
    public int RepDeductedCarjackedTerritory { get; set; }
    [Description("Additional reputation deducted if an enemy Gang Member is carjacked in their territory.")]

    public int IdleSpeakPercentage { get; set; }
    public int SevereCombatSpeakPercentage { get; set; }
    public int LowCombatSpeakPercentage { get; set; }
    public int FleeingSpeakPercentage { get; set; }
    public bool AllowBackupAssaultSpawns { get; set; }
    public bool AllowNonEnemyTargets { get; set; }

    public GangSettings()
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
        ManageTasking = true;
        CheckCrimes = true;
        ShowSpawnedBlip = false;
        ShowAmbientBlips = false;
        RemoveNonSpawnedGangMembers = false;
        RemoveVanillaSpawnedPedsOutsideTerritory = false;
        RemoveNonSpawnedGangMembersOnFoot = false;
        PercentSpawnOutsideTerritory = 10;
        ManageDispatching = true;

        TimeBetweenSpawn = 10000;//10000;

        TimeBetweenSpawn_DowntownAdditional = 10000;
        TimeBetweenSpawn_WildernessAdditional = 80000;
        TimeBetweenSpawn_RuralAdditional = 50000;
        TimeBetweenSpawn_SuburbAdditional = 20000;
        TimeBetweenSpawn_IndustrialAdditional = 20000;




        MinDistanceToSpawnOnFoot = 125f;//75f// 50f;
        MaxDistanceToSpawnOnFoot = 225f;//200f// 150f;


        MinDistanceToSpawnInVehicle = 300f;// 400f;//300f// 50f;
        MaxDistanceToSpawnInVehicle = 500f;// 600f;//500f// 150f;



        TotalSpawnedMembersLimit = 10;// 8;// 10;////12;// 16;//5
        TotalSpawnedAmbientMembersLimit = 8;// 4;// 5;// 8;

        TotalSpawnedAmbientMembersLimit_Downtown = 6;// 4;// 5;
        TotalSpawnedAmbientMembersLimit_Wilderness = 2;
        TotalSpawnedAmbientMembersLimit_Rural = 3;
        TotalSpawnedAmbientMembersLimit_Suburb = 4;
        TotalSpawnedAmbientMembersLimit_Industrial = 4;// 3;// 4;

        AmbientSpawnPercentage = 60;
        AmbientSpawnPercentage_Wilderness = 35;
        AmbientSpawnPercentage_Rural = 45;
        AmbientSpawnPercentage_Suburb = 55;
        AmbientSpawnPercentage_Industrial = 55;
        AmbientSpawnPercentage_Downtown = 75;




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
        DenSpawningIgnoresLimits = false;
        AllowDenSpawningWhenPlayerWanted = true;


        EscortOffsetValue = 3.0f;
        EscortSpeedFast = 100f;
        EscortSpeedNormal = 30f;
        DenSpawningWhenPlayerWantedMaxWanted = 4;
        AmbientSpawningWhenPlayerWantedMaxWanted = 4;
        AmbientSpawnPedestrianAttemptPercentage = 20f;

        DistanceToReportRepChanges = 75f;
        GameTimeToReportRepChanges = 45000;
        GameTimeToReportRepChangesInTerritory = 20000;
        MurderDistance = 15f;
        GameTimeRecentlyKilled = 20000;
        MinDistanceToReportTimeoutRepChanges = 25f;
        AllowHitSquads = true;

        MinTimeBetweenHitSquads = 900000;
        MaxTimeBetweenHitSquads = 1500000;
        AllowHitSquadsOnlyEnemy = false;

       // BoatSpawnLimit_Default = 1;
        HeliSpawnLimit_Default = 1;
        SendHitSquadText = true;

        ShowGangTerritoryBlip = true;//HAS DESERIALIZED VALUES
        GangTerritoryBlipSize = 125f;//HAS DESERIALIZED VALUES
        GangTerritoryBlipAlpha = 0.15f;//HAS DESERIALIZED VALUES

        SetPersistent = false;


        AllowAmbientSpeech = true;


        TimeBetweenGangSpeak_General_Min = 30000;
        TimeBetweenGangSpeak_General_Randomizer_Min = 30000;
        TimeBetweenGangSpeak_General_Randomizer_Max = 60000;


        TimeBetweenGangSpeak_Fleeing_Min = 20000;
        TimeBetweenGangSpeak_Fleeing_Randomizer_Min = 20000;
        TimeBetweenGangSpeak_Fleeing_Randomizer_Max = 35000;


        TimeBetweenGangSpeak_LowCombat_Min = 15000;
        TimeBetweenGangSpeak_LowCombat_Randomizer_Min = 20000;
        TimeBetweenGangSpeak_LowCombat_Randomizer_Max = 35000;

        TimeBetweenGangSpeak_HighCombat_Min = 12000;
        TimeBetweenGangSpeak_HighCombat_Randomizer_Min = 20000;
        TimeBetweenGangSpeak_HighCombat_Randomizer_Max = 35000;

        RepDeductedKilled = 1000;
        RepDeductedKilledTerritory = 4000;
        RepDeductedInjured = 500;
        RepDeductedInjuredTerritory = 2500;
        RepDeductedCarjacked = 2500;
        RepDeductedCarjackedTerritory = 2500;

        IdleSpeakPercentage = 10;
        SevereCombatSpeakPercentage = 35;
        LowCombatSpeakPercentage = 15;
        FleeingSpeakPercentage = 15;

        AllowBackupAssaultSpawns = true;

        AllowNonEnemyTargets = true;

       // RemoveNonSpawnedGangMembersOnFoot_Extra = false;
    }

}
