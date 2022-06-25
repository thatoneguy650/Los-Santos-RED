using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PoliceSettings : ISettingsDefaultable
{
    public bool ManageDispatching { get; set; }
    public bool ManageTasking { get; set; }

    public bool ShowSpawnedBlips { get; set; }
    public bool ShowVanillaBlips { get; set; }


    public bool OverrideAccuracy { get; set; }
    public bool OverrideHealth { get; set; }
    public bool OverrideArmor { get; set; }


    public bool ManageLoadout { get; set; }
    public bool AllowAmbientSpeech { get; set; }
    public bool AllowChaseAssists { get; set; }
    public bool AllowFrontVehicleClearAssist { get; set; }
    public bool AllowReducedCollisionPenaltyAssist { get; set; }
    public bool AllowPowerAssist { get; set; }



    public bool UseFakeWantedLevelSystem { get; set; }
    public bool TakeExclusiveControlOverWantedLevel { get; set; }
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
    public int LikelyHoodOfAnySpawn_NotWanted { get; set; }
    public int LikelyHoodOfCountySpawn_NotWanted { get; set; }

    public int LikelyHoodOfAnySpawn_Wanted { get; set; }
    public int LikelyHoodOfCountySpawn_Wanted { get; set; }

    public PoliceSettings()
    {
        SetDefault();
        #if DEBUG
                ShowSpawnedBlips = true;
                ShowVanillaBlips = false;
       // ManageDispatching = false;
       // AllowRadioInAnimations = true;
#else
                       // ShowSpawnedBlips = false;
#endif

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



        //GeneralCombatAbility = 1;



        //GeneralAccuracy = 40;
        //TaserAccuracy = 30;
        //VehicleAccuracy = 10;
        //GeneralShootRate = 500;//even
        //TaserShootRate = 100;
        //VehicleShootRate = 20;




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
        //AllowRadioInAnimations = false;
        OverrideHealth = true;
        //MinHealth = 85;
        //MaxHealth = 125;
        //OverrideArmor = true;
        //MinArmor = 0;
        //MaxArmor = 50;
        PedSpawnLimit_Default = 7;
        PedSpawnLimit_Investigation = 8;
        PedSpawnLimit_Wanted1 = 9;//7;
        PedSpawnLimit_Wanted2 = 10;//9;
        PedSpawnLimit_Wanted3 = 16;//13;
        PedSpawnLimit_Wanted4 = 20;
        PedSpawnLimit_Wanted5 = 24;
        PedSpawnLimit_Wanted6 = 26;
        VehicleSpawnLimit_Default = 6;
        VehicleSpawnLimit_Investigation = 7;
        VehicleSpawnLimit_Wanted1 = 8;
        VehicleSpawnLimit_Wanted2 = 9;
        VehicleSpawnLimit_Wanted3 = 13;
        VehicleSpawnLimit_Wanted4 = 15;
        VehicleSpawnLimit_Wanted5 = 16;
        VehicleSpawnLimit_Wanted6 = 18;
        WantedLevelIncreasesByKillingPolice = true;
        KillLimit_Wanted4 = 5;
        KillLimit_Wanted5 = 10;
        KillLimit_Wanted6 = 20;
        WantedLevelIncreasesOverTime = true;
        //WantedLevelIncreaseTime = 180000;//240000

        //WantedLevelIncreaseTime_FromWanted1 = 60000;//1 min
        //WantedLevelIncreaseTime_FromWanted2 = 120000;//2 mins
        //WantedLevelIncreaseTime_FromWanted3 = 210000;//3.5 mins
        //WantedLevelIncreaseTime_FromWanted4 = 240000;//4 minutes
        //WantedLevelIncreaseTime_FromWanted5 = 240000;//4 minutes

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


        LikelyHoodOfAnySpawn_NotWanted = 5;
        LikelyHoodOfCountySpawn_NotWanted = 5;


        LikelyHoodOfAnySpawn_Wanted = 20;
        LikelyHoodOfCountySpawn_Wanted = 20;
    }
}