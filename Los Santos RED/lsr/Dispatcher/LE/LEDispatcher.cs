using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

public class LEDispatcher
{
    private readonly ISettingsProvideable Settings;
    private readonly IStreets Streets;
    private readonly IEntityProvideable World;
    private readonly IJurisdictions Jurisdictions;
    private readonly IZones Zones;
    private readonly IAgencies Agencies;
    private readonly IDispatchable Player;
    private readonly IWeapons Weapons;
    private readonly INameProvideable Names;
    private readonly IPlacesOfInterest PlacesOfInterest;
    private readonly IModItems ModItems;

    private readonly float MinimumDeleteDistance = 150f;//200f
    private readonly uint MinimumExistingTime = 20000;

    private bool HasDispatchedThisTick;
    private bool ShouldRunAmbientDispatch;
    private uint GameTimeAttemptedDispatch;
    private uint GameTimeAttemptedDispatchRoadblock;
    private uint GameTimeAttemptedRecall;
    private uint GameTimeLastSpawnedRoadblock;
    private Roadblock Roadblock;
    private Agency LastAgencySpawned;
    private PoliceStation PoliceStation;
    private SpawnLocation SpawnLocation;
    private Agency Agency;
    private DispatchableVehicle VehicleType;
    private DispatchablePerson PersonType;
    private bool IsOffDutySpawn;

    private MarshalDispatcher MarshalDispatcher;
    private Vector3 RoadblockInitialPosition;
    private Vector3 RoadblockAwayPosition;
    private Street RoadblockInitialPositionStreet;
    private Vector3 RoadblockFinalPosition;
    private float RoadblockFinalHeading;
    private uint GameTimeLastAttemptedAssaultSpawn;
    private bool IsTunnelSpawn;
    private StoredSpawn SelectedTunnelSpawn;
    private uint GameTimeLastCheckedHeliFill;
    private uint GameTimeLastSpawnedOrRecalledHeli;

    private uint GameTimeLastSpawnedOrRecalledCanineUnit;


    private uint DelayBetweenHeliSpawnAfterSpawnOrRecall
    {
        get
        {
            if (World.TotalWantedLevel == 1)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnDelay_Wanted1;
            }
            else if (World.TotalWantedLevel == 2)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnDelay_Wanted2;
            }
            else if (World.TotalWantedLevel == 3)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnDelay_Wanted3;
            }
            else if (World.TotalWantedLevel == 4)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnDelay_Wanted4;
            }
            else if (World.TotalWantedLevel == 5)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnDelay_Wanted5;
            }
            else if (World.TotalWantedLevel == 6)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnDelay_Wanted6;
            }
            else if (World.TotalWantedLevel == 7)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnDelay_Wanted7;
            }
            else if (World.TotalWantedLevel == 8)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnDelay_Wanted8;
            }
            else if (World.TotalWantedLevel == 9)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnDelay_Wanted9;
            }
            else if (World.TotalWantedLevel == 10)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnDelay_Wanted10;
            }

            else
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnDelay_Default;
            }
        }
    }
    private uint DelayBetweenCanineSpawnAfterSpawnOrRecall
    {
        get
        {
            if (World.TotalWantedLevel == 1)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnDelay_Wanted1;
            }
            else if (World.TotalWantedLevel == 2)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnDelay_Wanted2;
            }
            else if (World.TotalWantedLevel == 3)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnDelay_Wanted3;
            }
            else if (World.TotalWantedLevel == 4)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnDelay_Wanted4;
            }
            else if (World.TotalWantedLevel == 5)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnDelay_Wanted5;
            }
            else if (World.TotalWantedLevel == 6)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnDelay_Wanted6;
            }
            else if (World.TotalWantedLevel == 7)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnDelay_Wanted7;
            }
            else if (World.TotalWantedLevel == 8)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnDelay_Wanted8;
            }
            else if (World.TotalWantedLevel == 9)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnDelay_Wanted9;
            }
            else if (World.TotalWantedLevel == 10)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnDelay_Wanted10;
            }

            else
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnDelay_Default;
            }
        }
    }
    private bool HasNeedToSpawnHeli => World.Vehicles.PoliceHelicoptersCount < SpawnedHeliLimit && (GameTimeLastSpawnedOrRecalledHeli == 0 || Game.GameTime - GameTimeLastSpawnedOrRecalledHeli >= DelayBetweenHeliSpawnAfterSpawnOrRecall);
    private bool HasNeedToSpawnBoat => (Player.CurrentVehicle?.IsBoat == true || Player.IsSwimming) && World.Vehicles.PoliceBoatsCount < SpawnedBoatLimit;
    private bool TotalIsWanted => World.TotalWantedLevel > 0;
    public LEDispatcher(IEntityProvideable world, IDispatchable player, IAgencies agencies, ISettingsProvideable settings, IStreets streets, IZones zones, IJurisdictions jurisdictions, IWeapons weapons, INameProvideable names, IPlacesOfInterest placesOfInterest, IModItems modItems)
    {
        Player = player;
        World = world;
        Agencies = agencies;
        Settings = settings;
        Streets = streets;
        Zones = zones;
        Jurisdictions = jurisdictions;
        Weapons = weapons;
        Names = names;
        PlacesOfInterest = placesOfInterest;
        ModItems = modItems;
        MarshalDispatcher = new MarshalDispatcher(Player, this, Settings, World, Weapons, Names, PlacesOfInterest, ModItems, Agencies);
    }
    private float LikelyHoodOfAnySpawn
    {
        get
        {
            if (World.TotalWantedLevel == 1)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.LikelyHoodOfAnySpawn_Wanted1;
            }
            else if (World.TotalWantedLevel == 2)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.LikelyHoodOfAnySpawn_Wanted2;
            }
            else if (World.TotalWantedLevel == 3)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.LikelyHoodOfAnySpawn_Wanted3;
            }
            else if (World.TotalWantedLevel == 4)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.LikelyHoodOfAnySpawn_Wanted4;
            }
            else if (World.TotalWantedLevel == 5)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.LikelyHoodOfAnySpawn_Wanted5;
            }
            else if (World.TotalWantedLevel == 6)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.LikelyHoodOfAnySpawn_Wanted6;
            }
            else if (World.TotalWantedLevel == 7)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.LikelyHoodOfAnySpawn_Wanted7;
            }
            else if (World.TotalWantedLevel == 8)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.LikelyHoodOfAnySpawn_Wanted8;
            }
            else if (World.TotalWantedLevel == 9)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.LikelyHoodOfAnySpawn_Wanted9;
            }
            else if (World.TotalWantedLevel == 10)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.LikelyHoodOfAnySpawn_Wanted10;
            }

            else
            {
                return Settings.SettingsManager.PoliceSpawnSettings.LikelyHoodOfAnySpawn_Default;
            }
        }
    }
    private float LikelyHoodOfCountySpawn
    {
        get
        {
            if(World.TotalWantedLevel == 1)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.LikelyHoodOfCountySpawn_Wanted1;
            }
            else if (World.TotalWantedLevel == 2)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.LikelyHoodOfCountySpawn_Wanted2;
            }
            else if (World.TotalWantedLevel == 3)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.LikelyHoodOfCountySpawn_Wanted3;
            }
            else if (World.TotalWantedLevel == 4)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.LikelyHoodOfCountySpawn_Wanted4;
            }
            else if (World.TotalWantedLevel == 5)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.LikelyHoodOfCountySpawn_Wanted5;
            }
            else if (World.TotalWantedLevel == 6)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.LikelyHoodOfCountySpawn_Wanted6;
            }
            else if (World.TotalWantedLevel == 7)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.LikelyHoodOfCountySpawn_Wanted7;
            }
            else if (World.TotalWantedLevel == 8)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.LikelyHoodOfCountySpawn_Wanted8;
            }
            else if (World.TotalWantedLevel == 9)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.LikelyHoodOfCountySpawn_Wanted9;
            }
            else if (World.TotalWantedLevel == 10)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.LikelyHoodOfCountySpawn_Wanted10;
            }
            else
            {
                return Settings.SettingsManager.PoliceSpawnSettings.LikelyHoodOfCountySpawn_Default;
            }
        }
    }
    private bool HasNeedToAmbientCanineDispatch => World.Pedestrians.TotalSpawnedAmbientPoliceCanines < SpawnedK9Limit && (GameTimeLastSpawnedOrRecalledCanineUnit == 0 || Game.GameTime - GameTimeLastSpawnedOrRecalledCanineUnit >= DelayBetweenCanineSpawnAfterSpawnOrRecall);
    public float ClosestPoliceSpawnToOtherPoliceAllowed => TotalIsWanted ? 200f : 500f;
    public float ClosestPoliceSpawnToSuspectAllowed => TotalIsWanted ? 200f : 250f;//150f : 250f;
    private List<Cop> DeletableCops => World.Pedestrians.AllPoliceList.Where(x => (x.RecentlyUpdated && x.DistanceToPlayer >= MinimumDeleteDistance && x.HasBeenSpawnedFor >= MinimumExistingTime && x.Handle != Player.Handle) || x.CanRemove).ToList();//NEED TO ADD WAS MOD SPAWNED HERE, LET THE REST OF THE FUCKERS MANAGE THEIR OWN STUFF?
    private float DistanceToDelete => TotalIsWanted ? Settings.SettingsManager.PoliceSpawnSettings.DistanceToRecallInVehicle_Wanted : Settings.SettingsManager.PoliceSpawnSettings.DistanceToRecallInVehicle_NotWanted;
    private float DistanceToDeleteOnFoot => TotalIsWanted ? Settings.SettingsManager.PoliceSpawnSettings.DistanceToRecallOnFoot_Wanted : Settings.SettingsManager.PoliceSpawnSettings.DistanceToRecallOnFoot_NotWanted;
    private bool HasNeedToAmbientDispatch => World.Pedestrians.TotalSpawnedAmbientPolice < SpawnedCopLimit && World.Vehicles.SpawnedAmbientPoliceVehiclesCount < SpawnedCopVehicleLimit;
    private bool HasNeedToDispatchRoadblock => Settings.SettingsManager.RoadblockSettings.RoadblockEnabled && Player.WantedLevel >= Settings.SettingsManager.RoadblockSettings.RoadblockMinWantedLevel && Player.WantedLevel <= Settings.SettingsManager.RoadblockSettings.RoadblockMaxWantedLevel && Roadblock == null;//roadblocks are only for player
    private bool IsTimeToAmbientDispatch => Game.GameTime - GameTimeAttemptedDispatch >= TimeBetweenSpawn;
    private bool IsTimeToDispatchRoadblock => Game.GameTime - GameTimeLastSpawnedRoadblock >= TimeBetweenRoadblocks && Player.PoliceResponse.HasBeenAtCurrentWantedLevelFor >= 30000;
    private bool IsTimeToRecall => Game.GameTime - GameTimeAttemptedRecall >= TimeBetweenRecall;
    private bool IsTimeToAmbientCanineDispatch => Game.GameTime - GameTimeAttemptedDispatch >= TimeBetweenSpawn;
    private int TimeBetweenSpawn
    {
        get
        {

            if (World.TotalWantedLevel == 0 && !Player.Investigation.IsActive)
            {
                int TotalTimeBetweenSpawns = Settings.SettingsManager.PoliceSpawnSettings.AmbientTimeBetweenSpawn;
                if (EntryPoint.FocusZone?.Type == eLocationType.Wilderness)
                {
                    TotalTimeBetweenSpawns += Settings.SettingsManager.PoliceSpawnSettings.AmbientTimeBetweenSpawn_WildernessAdditional;
                }
                else if (EntryPoint.FocusZone?.Type == eLocationType.Rural)
                {
                    TotalTimeBetweenSpawns += Settings.SettingsManager.PoliceSpawnSettings.AmbientTimeBetweenSpawn_RuralAdditional;
                }
                else if (EntryPoint.FocusZone?.Type == eLocationType.Suburb)
                {
                    TotalTimeBetweenSpawns += Settings.SettingsManager.PoliceSpawnSettings.AmbientTimeBetweenSpawn_SuburbAdditional;
                }
                else if (EntryPoint.FocusZone?.Type == eLocationType.Industrial)
                {
                    TotalTimeBetweenSpawns += Settings.SettingsManager.PoliceSpawnSettings.AmbientTimeBetweenSpawn_IndustrialAdditional;
                }
                else if (EntryPoint.FocusZone?.Type == eLocationType.Downtown)
                {
                    TotalTimeBetweenSpawns += Settings.SettingsManager.PoliceSpawnSettings.AmbientTimeBetweenSpawn_DowntownAdditional;
                }
                return TotalTimeBetweenSpawns;
            }
            else if (Player.Investigation.IsActive)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.AmbientTimeBetweenSpawn;
            }


            int UnseenTime = Settings.SettingsManager.PoliceSpawnSettings.TimeBetweenCopSpawn_Unseen;//5000
            int SeenScalarTime = Settings.SettingsManager.PoliceSpawnSettings.TimeBetweenCopSpawn_Seen_AdditionalTimeScaler;//1000
            int SeenMinTime = Settings.SettingsManager.PoliceSpawnSettings.TimeBetweenCopSpawn_Seen_Min;//3000


            int ExtraSpawnTimeByLocationType = Settings.SettingsManager.PoliceSpawnSettings.PoliceDispatchDelay_Default;
            if (EntryPoint.FocusZone?.Type == eLocationType.Wilderness)
            {
                ExtraSpawnTimeByLocationType = Settings.SettingsManager.PoliceSpawnSettings.PoliceDispatchDelay_Wilderness;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Rural)
            {
                ExtraSpawnTimeByLocationType = Settings.SettingsManager.PoliceSpawnSettings.PoliceDispatchDelay_Rural;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Suburb)
            {
                ExtraSpawnTimeByLocationType = Settings.SettingsManager.PoliceSpawnSettings.PoliceDispatchDelay_Suburb;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Industrial)
            {
                ExtraSpawnTimeByLocationType = Settings.SettingsManager.PoliceSpawnSettings.PoliceDispatchDelay_Industrial;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Downtown)
            {
                ExtraSpawnTimeByLocationType = Settings.SettingsManager.PoliceSpawnSettings.PoliceDispatchDelay_Downtown;
            }



            if (World.TotalWantedLevel > Player.WantedLevel)
            {
                return UnseenTime + ExtraSpawnTimeByLocationType;
            }
            else if (!Player.AnyPoliceRecentlySeenPlayer && !Settings.SettingsManager.PoliceSpawnSettings.UseScalingLogicForSpawnWhenUnseen)
            {
                return UnseenTime + ExtraSpawnTimeByLocationType;
            }
            else
            {
                if (World.TotalWantedLevel <= 6)
                {
                    int totalTime = ((6 - World.TotalWantedLevel) * SeenScalarTime) + SeenMinTime + ExtraSpawnTimeByLocationType;
                    EntryPoint.WriteToConsole($"Time Between Dispatching when seen {totalTime} WantedLevel{World.TotalWantedLevel} ExtraSpawnTimeByLocationType{ExtraSpawnTimeByLocationType}");
                    return totalTime;
                }
                return SeenMinTime;  
            }
        }
    }
    private int TimeBetweenRecall
    {
        get
        {
            int UnseenTime = Settings.SettingsManager.PoliceSpawnSettings.TimeBetweenCopDespawn_Unseen;
            int SeenScalarTime = Settings.SettingsManager.PoliceSpawnSettings.TimeBetweenCopDespawn_Seen_AdditionalTimeScaler;
            int SeenMinTime = Settings.SettingsManager.PoliceSpawnSettings.TimeBetweenCopDespawn_Seen_Min;
            if (World.TotalWantedLevel > Player.WantedLevel)
            {
                return UnseenTime;
            }
            else if (!Player.AnyPoliceRecentlySeenPlayer)
            {
                return UnseenTime;
            }
            else
            {
                if(World.TotalWantedLevel <= 6)
                {
                    return ((6 - World.TotalWantedLevel) * SeenScalarTime) + SeenMinTime;
                }
                return SeenMinTime;
            }
        }
    }
    private int TimeBetweenRoadblocks
    {
        get
        {
            int UnseenTime = Settings.SettingsManager.RoadblockSettings.TimeBetweenRoadblock_Unseen;
            int SeenScalarTime = Settings.SettingsManager.RoadblockSettings.TimeBetweenRoadblock_Seen_AdditionalTimeScaler;
            int SeenMinTime = Settings.SettingsManager.RoadblockSettings.TimeBetweenRoadblock_Seen_Min;

            if (!Player.AnyPoliceRecentlySeenPlayer)
            {
                return UnseenTime;
            }
            else
            {
                return ((Settings.SettingsManager.PoliceSettings.MaxWantedLevel - World.TotalWantedLevel) * SeenScalarTime) + SeenMinTime;
            }
        }
    }
    private float MaxDistanceToSpawn
    {
        get
        {
            float MaxWantedUnseen = Settings.SettingsManager.PoliceSpawnSettings.MaxDistanceToSpawn_WantedUnseen;
            float MaxWantedSeen = Settings.SettingsManager.PoliceSpawnSettings.MaxDistanceToSpawn_WantedSeen;
            float MaxNotWanted = Settings.SettingsManager.PoliceSpawnSettings.MaxDistanceToSpawn_NotWanted;
            if(World.TotalWantedLevel > Player.WantedLevel)
            {
                return MaxWantedUnseen;
            }
            else if (TotalIsWanted)
            {
                if (!Player.AnyPoliceRecentlySeenPlayer)
                {
                    return MaxWantedUnseen;
                }
                else
                {
                    return MaxWantedSeen;
                }
            }
            else if (Player.Investigation.IsActive)
            {
                return Player.Investigation.Distance;
            }
            else
            {
                return MaxNotWanted;
            }
        }
    }
    private float MinDistanceToSpawn
    {
        get
        {
            float MinWantedUnseen = Settings.SettingsManager.PoliceSpawnSettings.MinDistanceToSpawn_WantedUnseen;
            float MinWantedSeen = Settings.SettingsManager.PoliceSpawnSettings.MinDistanceToSpawn_WantedSeen;
            float MinNotWanted = Settings.SettingsManager.PoliceSpawnSettings.MinDistanceToSpawn_NotWanted;
            float MinScalerUnseen = Settings.SettingsManager.PoliceSpawnSettings.MinDistanceToSpawn_WantedUnseenScalar; //40f;
            float MinScalerSeen = Settings.SettingsManager.PoliceSpawnSettings.MinDistanceToSpawn_WantedSeenScalar; //40f;
            if (World.TotalWantedLevel > Player.WantedLevel)
            {
                return MinWantedUnseen;
            }
            else if (TotalIsWanted)
            {
                if (!Player.AnyPoliceRecentlySeenPlayer)
                {
                    float calcDistance = MinWantedUnseen - (World.TotalWantedLevel * MinScalerUnseen);
                    return calcDistance < 150f ? 150f : calcDistance;
                }
                else
                {
                    float calcDistance = MinWantedSeen - (World.TotalWantedLevel * MinScalerSeen);
                    return calcDistance < 150f ? 150f : calcDistance;
                }
            }
            else if (Player.Investigation.IsActive)
            {
                return Player.Investigation.Distance / 2;
            }
            else
            {
                return MinNotWanted;
            }
        }
    }
    private int SpawnedCopLimit
    {
        get
        {
            if (World.TotalWantedLevel == 10)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.PedSpawnLimit_Wanted10;
            }
            else if (World.TotalWantedLevel == 9)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.PedSpawnLimit_Wanted9;
            }
            else if (World.TotalWantedLevel == 8)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.PedSpawnLimit_Wanted8;
            }
            else if (World.TotalWantedLevel == 7)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.PedSpawnLimit_Wanted7;
            }
            else if (World.TotalWantedLevel == 6)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.PedSpawnLimit_Wanted6;
            }
            else if (World.TotalWantedLevel == 5)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.PedSpawnLimit_Wanted5;
            }
            else if (World.TotalWantedLevel == 4)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.PedSpawnLimit_Wanted4;
            }
            else if (World.TotalWantedLevel == 3)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.PedSpawnLimit_Wanted3;
            }
            else if (World.TotalWantedLevel == 2)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.PedSpawnLimit_Wanted2;
            }
            else if (World.TotalWantedLevel == 1)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.PedSpawnLimit_Wanted1;
            }
            else if (Player.Investigation.IsActive)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.PedSpawnLimit_Investigation;
            }
            else if (World.TotalWantedLevel == 0)
            {
                if (EntryPoint.FocusZone?.Type == eLocationType.Wilderness)
                {
                    return Settings.SettingsManager.PoliceSpawnSettings.PedSpawnLimit_Default_Wilderness;
                }
                else if (EntryPoint.FocusZone?.Type == eLocationType.Rural)
                {
                    return Settings.SettingsManager.PoliceSpawnSettings.PedSpawnLimit_Default_Rural;
                }
                else if (EntryPoint.FocusZone?.Type == eLocationType.Suburb)
                {
                    return Settings.SettingsManager.PoliceSpawnSettings.PedSpawnLimit_Default_Suburb;
                }
                else if (EntryPoint.FocusZone?.Type == eLocationType.Industrial)
                {
                    return Settings.SettingsManager.PoliceSpawnSettings.PedSpawnLimit_Default_Industrial;
                }
                else if (EntryPoint.FocusZone?.Type == eLocationType.Downtown)
                {
                    return Settings.SettingsManager.PoliceSpawnSettings.PedSpawnLimit_Default_Downtown;
                }
                else
                {
                    return Settings.SettingsManager.PoliceSpawnSettings.PedSpawnLimit_Default;
                }
            }
            else
            {
                return Settings.SettingsManager.PoliceSpawnSettings.PedSpawnLimit_Default;
            }
        }
    }
    private int SpawnedK9Limit
    {
        get
        {
            if (World.TotalWantedLevel == 10)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.K9SpawnLimit_Wanted10;// return 2;
            }
            if (World.TotalWantedLevel == 9)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.K9SpawnLimit_Wanted9;// return 2;
            }
            if (World.TotalWantedLevel == 8)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.K9SpawnLimit_Wanted8;// return 2;
            }
            if (World.TotalWantedLevel == 7)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.K9SpawnLimit_Wanted7;// return 2;
            }
            if (World.TotalWantedLevel == 6)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.K9SpawnLimit_Wanted6;// return 2;
            }
            else if (World.TotalWantedLevel == 5)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.K9SpawnLimit_Wanted5;//return 2;
            }
            else if (World.TotalWantedLevel == 4)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.K9SpawnLimit_Wanted4;//return 1;
            }
            else if (World.TotalWantedLevel == 3)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.K9SpawnLimit_Wanted3;//return 1;
            }
            else if (World.TotalWantedLevel == 2)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.K9SpawnLimit_Wanted2;//return 0;
            }
            else if (World.TotalWantedLevel == 1)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.K9SpawnLimit_Wanted1;//return 0;
            }
            else if (Player.Investigation.IsActive)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.K9SpawnLimit_Investigation;//return 0;
            }
            else if (World.TotalWantedLevel == 0)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.K9SpawnLimit_Default;//return 0;
            }
            else
            {
                return Settings.SettingsManager.PoliceSpawnSettings.K9SpawnLimit_Default;//return 0;
            }
        }
    }
    private int SpawnedHeliLimit
    {
        get
        {
            if (World.TotalWantedLevel == 10)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnLimit_Wanted10;// return 2;
            }
            if (World.TotalWantedLevel == 9)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnLimit_Wanted9;// return 2;
            }
            if (World.TotalWantedLevel == 8)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnLimit_Wanted8;// return 2;
            }
            if (World.TotalWantedLevel == 7)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnLimit_Wanted7;// return 2;
            }
            if (World.TotalWantedLevel == 6)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnLimit_Wanted6;// return 2;
            }
            else if (World.TotalWantedLevel == 5)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnLimit_Wanted5;//return 2;
            }
            else if (World.TotalWantedLevel == 4)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnLimit_Wanted4;//return 1;
            }
            else if (World.TotalWantedLevel == 3)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnLimit_Wanted3;//return 1;
            }
            else if (World.TotalWantedLevel == 2)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnLimit_Wanted2;//return 0;
            }
            else if (World.TotalWantedLevel == 1)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnLimit_Wanted1;//return 0;
            }
            else if (Player.Investigation.IsActive)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnLimit_Investigation;//return 0;
            }
            else if (World.TotalWantedLevel == 0)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnLimit_Default;//return 0;
            }
            else
            {
                return Settings.SettingsManager.PoliceSpawnSettings.HeliSpawnLimit_Default;//return 0;
            }
        }
    }
    private int SpawnedBoatLimit
    {
        get
        {

            if (World.TotalWantedLevel == 10)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.BoatSpawnLimit_Wanted10; //1;
            }
            else if (World.TotalWantedLevel == 9)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.BoatSpawnLimit_Wanted9; //1;
            }
            else if (World.TotalWantedLevel == 8)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.BoatSpawnLimit_Wanted8; //1;
            }
            else if (World.TotalWantedLevel == 7)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.BoatSpawnLimit_Wanted7; //1;
            }
            else if (World.TotalWantedLevel == 6)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.BoatSpawnLimit_Wanted6; //1;
            }
            else if (World.TotalWantedLevel == 5)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.BoatSpawnLimit_Wanted5; //return 1;
            }
            else if (World.TotalWantedLevel == 4)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.BoatSpawnLimit_Wanted4; //return 1;
            }
            else if (World.TotalWantedLevel == 3)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.BoatSpawnLimit_Wanted3; //return 1;
            }
            else if (World.TotalWantedLevel == 2)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.BoatSpawnLimit_Wanted2; //return 0;
            }
            else if (World.TotalWantedLevel == 1)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.BoatSpawnLimit_Wanted1; //return 0;
            }
            else if (Player.Investigation.IsActive)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.BoatSpawnLimit_Investigation; //return 0;
            }
            else if (World.TotalWantedLevel == 0)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.BoatSpawnLimit_Default; //return 0;
            }
            else
            {
                return Settings.SettingsManager.PoliceSpawnSettings.BoatSpawnLimit_Default; //return 0;
            }
        }
    }
    private int SpawnedCopVehicleLimit
    {
        get
        {
            if (World.TotalWantedLevel == 10)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.VehicleSpawnLimit_Wanted10;
            }
            else if (World.TotalWantedLevel == 9)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.VehicleSpawnLimit_Wanted9;
            }
            else if (World.TotalWantedLevel == 8)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.VehicleSpawnLimit_Wanted8;
            }
            else if (World.TotalWantedLevel == 7)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.VehicleSpawnLimit_Wanted7;
            }
            else if (World.TotalWantedLevel == 6)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.VehicleSpawnLimit_Wanted6;
            }
            else if (World.TotalWantedLevel == 5)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.VehicleSpawnLimit_Wanted5;
            }
            else if (World.TotalWantedLevel == 4)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.VehicleSpawnLimit_Wanted4;
            }
            else if (World.TotalWantedLevel == 3)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.VehicleSpawnLimit_Wanted3;
            }
            else if (World.TotalWantedLevel == 2)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.VehicleSpawnLimit_Wanted2;
            }
            else if (World.TotalWantedLevel == 1)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.VehicleSpawnLimit_Wanted1;
            }
            else if (Player.Investigation.IsActive)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.VehicleSpawnLimit_Investigation;
            }
            else if (World.TotalWantedLevel == 0)
            {
                if (EntryPoint.FocusZone?.Type == eLocationType.Wilderness)
                {
                    return Settings.SettingsManager.PoliceSpawnSettings.VehicleSpawnLimit_Default_Wilderness;
                }
                else if (EntryPoint.FocusZone?.Type == eLocationType.Rural)
                {
                    return Settings.SettingsManager.PoliceSpawnSettings.VehicleSpawnLimit_Default_Rural;
                }
                else if (EntryPoint.FocusZone?.Type == eLocationType.Suburb)
                {
                    return Settings.SettingsManager.PoliceSpawnSettings.VehicleSpawnLimit_Default_Suburb;
                }
                else if (EntryPoint.FocusZone?.Type == eLocationType.Industrial)
                {
                    return Settings.SettingsManager.PoliceSpawnSettings.VehicleSpawnLimit_Default_Industrial;
                }
                else if (EntryPoint.FocusZone?.Type == eLocationType.Downtown)
                {
                    return Settings.SettingsManager.PoliceSpawnSettings.VehicleSpawnLimit_Default_Downtown;
                }
                else
                {
                    return Settings.SettingsManager.PoliceSpawnSettings.VehicleSpawnLimit_Default;
                }
            }
            else
            {
                return Settings.SettingsManager.PoliceSpawnSettings.VehicleSpawnLimit_Default;
            }
        }
    }
    private int PercentageOfAmbientSpawn // => Settings.SettingsManager.GangSettings.TimeBetweenSpawn;//15000;
    {
        get
        {
            if(World.TotalWantedLevel >= 3)
            {
                return 100;
            }
            else if(World.TotalWantedLevel > 0)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.AmbientSpawnPercentage_Wanted;
            }
            else if (Player.Investigation.IsActive)
            {
                return Settings.SettingsManager.PoliceSpawnSettings.AmbientSpawnPercentage_Investigation;
            }
            int ambientSpawnPercent = Settings.SettingsManager.PoliceSpawnSettings.AmbientSpawnPercentage;
            if (EntryPoint.FocusZone?.Type == eLocationType.Wilderness)
            {
                ambientSpawnPercent = Settings.SettingsManager.PoliceSpawnSettings.AmbientSpawnPercentage_Wilderness;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Rural)
            {
                ambientSpawnPercent = Settings.SettingsManager.PoliceSpawnSettings.AmbientSpawnPercentage_Rural;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Suburb)
            {
                ambientSpawnPercent = Settings.SettingsManager.PoliceSpawnSettings.AmbientSpawnPercentage_Suburb;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Industrial)
            {
                ambientSpawnPercent = Settings.SettingsManager.PoliceSpawnSettings.AmbientSpawnPercentage_Industrial;
            }
            else if (EntryPoint.FocusZone?.Type == eLocationType.Downtown)
            {
                ambientSpawnPercent = Settings.SettingsManager.PoliceSpawnSettings.AmbientSpawnPercentage_Downtown;
            }
            return ambientSpawnPercent;
        }
    }
    private float CurrentRoadblockSpawnChance()
    {
        if (Player.WantedLevel == 0)
        {
            return 0;
        }
        else if(Player.WantedLevel == 1)
        {
            return Settings.SettingsManager.RoadblockSettings.RoadblockSpawnPercentage_Wanted1;
        }
        else if (Player.WantedLevel == 2)
        {
            return Settings.SettingsManager.RoadblockSettings.RoadblockSpawnPercentage_Wanted2;
        }
        else if (Player.WantedLevel == 3)
        {
            return Settings.SettingsManager.RoadblockSettings.RoadblockSpawnPercentage_Wanted3;
        }
        else if (Player.WantedLevel == 4)
        {
            return Settings.SettingsManager.RoadblockSettings.RoadblockSpawnPercentage_Wanted4;
        }
        else if (Player.WantedLevel == 5)
        {
            return Settings.SettingsManager.RoadblockSettings.RoadblockSpawnPercentage_Wanted5;
        }
        else if (Player.WantedLevel >= 6)
        {
            return Settings.SettingsManager.RoadblockSettings.RoadblockSpawnPercentage_Wanted6;
        }
        return 0;
    }
    public bool Dispatch()
    {
        //EntryPoint.WriteToConsole("LE DISPATCHER HAS DISPATCHED RAN1");
        HasDispatchedThisTick = false;
        if (!Settings.SettingsManager.PoliceSpawnSettings.ManageDispatching)
        {
            return HasDispatchedThisTick;
        }
        HandleAmbientSpawns();
        HandleRoadblockSpawns();
        if(MarshalDispatcher.Dispatch())
        {
            HasDispatchedThisTick = true;
        }
        HandleAssaultSpawns();
        HandleHelicopterRefill();
        return HasDispatchedThisTick;
    }
    private void HandleHelicopterRefill()
    {
        //EntryPoint.WriteToConsole("CHECK HELI REFIL RAN 0");
        if (!Settings.SettingsManager.PoliceSpawnSettings.AllowHelicopterPassengerRespawn)
        {
            return;
        }
        bool shouldAttempt = GameTimeLastCheckedHeliFill == 0 || Game.GameTime - GameTimeLastCheckedHeliFill >= 9000;
        if (!shouldAttempt)
        {
            return;
        }
        //EntryPoint.WriteToConsole("CHECK HELI REFIL RAN 1");
        GameTimeLastCheckedHeliFill = Game.GameTime;
        VehicleExt helicopter = World.Vehicles.PoliceVehicles.Where(x => x.TimesPassengersRefilled <= Settings.SettingsManager.PoliceSpawnSettings.HelicopterPassengerRespawnLimit && x.HasHadPedsRappelOrParachute && Game.GameTime - x.GameTimeLastHadPedsRappelOrParachute >= 25000 && x.Vehicle.Exists() && !x.Vehicle.IsOnScreen).FirstOrDefault();
        if(helicopter == null || !helicopter.Vehicle.Exists())
        {
            return;
        }
       // EntryPoint.WriteToConsole("CHECK HELI REFIL RAN 2");
        if (helicopter.RappelledSeats == null || !helicopter.RappelledSeats.Any())
        {
            return;
        }
        //EntryPoint.WriteToConsole("CHECK HELI REFIL RAN 3");
        foreach (RappelledSeat rs in helicopter.RappelledSeats)
        {
            if(helicopter == null || !helicopter.Vehicle.Exists())
            {
                return;
            }
            if (helicopter.Vehicle.IsSeatFree(rs.SeatRappelledFrom))
            {
                //EntryPoint.WriteToConsole("CHECK HELI REFIL RAN SPAWN !");
                CallRefillSpawnTask(helicopter, rs.SeatRappelledFrom);
            }
            GameFiber.Yield();
        }
        if (helicopter == null || !helicopter.Vehicle.Exists())
        {
            return;
        }
        helicopter.RappelledSeats.Clear();
        helicopter.TimesPassengersRefilled++;
        EntryPoint.WriteToConsole($"HandleHelicopterRefill Filled Heli TimesPassengersRefilled:{helicopter.TimesPassengersRefilled}");
    }
    private void CallRefillSpawnTask(VehicleExt vehicleExt,int seatIndex)
    {
        if (vehicleExt == null)
        {
            return;
        }
        Agency = vehicleExt.AssociatedAgency;
        if (Agency == null)
        {
            return;
        }
        LESpawnTask spawnTask = new LESpawnTask(Agency, SpawnLocation, vehicleExt.DispatchableVehicle, null, Settings.SettingsManager.PoliceSpawnSettings.ShowSpawnedBlips, Settings, Weapons, Names, false, World, ModItems, false);
        spawnTask.SpawnWithAllWeapons = true;
        EntryPoint.WriteToConsole($"DEBUG LE DISPATCH RESPAWNING RAPPELLED PED VehicleType:{vehicleExt?.DispatchableVehicle} PersonType:{null} RequiredPedGroup:{vehicleExt?.DispatchableVehicle?.RequiredPedGroup} GroupName:{null}");
        spawnTask.SpawnAsPassenger(vehicleExt, seatIndex);
        GameFiber.Yield();
        spawnTask.CreatedPeople.ForEach(x => { World.Pedestrians.AddEntity(x); x.IsLocationSpawned = false; });
        //spawnTask.CreatedVehicles.ForEach(x => x.AddVehicleToList(World));
        HasDispatchedThisTick = true;
        Player.OnLawEnforcementSpawn(Agency, VehicleType, PersonType);
    }
    private void HandleAssaultSpawns()
    {
        bool shouldAttempt = GameTimeLastAttemptedAssaultSpawn == 0 || Game.GameTime - GameTimeLastAttemptedAssaultSpawn >= 9000;
        if (!shouldAttempt)
        {
            return;
        }
        if (Player.IsNotWanted || Player.IsDead || Player.WantedLevel <= 2)// || Player.IsInVehicle)
        {
            //EntryPoint.WriteToConsole("Assault Spawn failed NOT NEEDED");
            return;
        }
        if (World.Pedestrians.TotalSpawnedAmbientPolice >= SpawnedCopLimit)
        {
            EntryPoint.WriteToConsole("Assault Spawn failed TOO MANY COPS");
            return;
        }
        GameFiber.Yield();
        GameTimeLastAttemptedAssaultSpawn = Game.GameTime;
        ILEDispatchableLocation ClosestStation = PlacesOfInterest.LEDispatchLocations().Where(x => x.DistanceToPlayer <= 150f && x.IsEnabled && x.IsActivated && x.AssignedAgency != null).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
        if(ClosestStation == null || ClosestStation.AssignedAgency == null)
        {
            //EntryPoint.WriteToConsole("Assault Spawn failed no station or agency");
            return;
        }
        if(ClosestStation.TotalAssaultSpawns >= ClosestStation.MaxAssaultSpawns)
        {
            //EntryPoint.WriteToConsole("Assault Spawn failed too many spawns already");
            return;
        }
        if (!ClosestStation.AssignedAgency.CanSpawn(World.TotalWantedLevel))
        {
            //EntryPoint.WriteToConsole("Assault Spawn failed cantspawn");
            return;
        }
        if (!GetAssaultSpawnTypes(ClosestStation.AssignedAgency))
        {
            //EntryPoint.WriteToConsole("Assault Spawn failed type");
            return;
        }
        if (!GetAssaultSpawnLocation(ClosestStation))
        {
            //EntryPoint.WriteToConsole("Assault Spawn failed location");
            return;
        }
        EntryPoint.WriteToConsole($"Assault Spawn EXECUTED TotalAssaultSpawns SO FAR:{ClosestStation.TotalAssaultSpawns}");
        GameFiber.Yield();
        bool spawnsWithHeavyWeapons = RandomItems.RandomPercent(ClosestStation.AssaultSpawnHeavyWeaponsPercent);
        if(CallSpawnTask(true, true, true, false, TaskRequirements.None, false, false, spawnsWithHeavyWeapons))
        {
            ClosestStation.TotalAssaultSpawns++;
        }
    }
    private bool GetAssaultSpawnLocation(IAssaultSpawnable ClosestStation)
    {
        SpawnLocation = new SpawnLocation();
        if(ClosestStation == null || PersonType == null || string.IsNullOrEmpty(PersonType.ModelName))
        {
            return false;
        }
        uint modelHash = Game.GetHashKey(PersonType.ModelName);
        uint GameTimeStarted = Game.GameTime;
        if (!NativeFunction.Natives.HAS_MODEL_LOADED<bool>(modelHash))
        {
            NativeFunction.Natives.REQUEST_MODEL(modelHash);
            while (!NativeFunction.Natives.HAS_MODEL_LOADED<bool>(modelHash) && Game.GameTime - GameTimeStarted <= 1000)
            {
                GameFiber.Yield();
            }
        }

        List<SpawnPlace> PossibleSpawnPlaces = new List<SpawnPlace>();
        if(ClosestStation.AssaultSpawnLocations != null && ClosestStation.AssaultSpawnLocations.Any())
        {
            PossibleSpawnPlaces.AddRange(ClosestStation.AssaultSpawnLocations.Where(x => x.Position.DistanceTo2D(Game.LocalPlayer.Character) >= 20f).ToList());
            GameFiber.Yield();
        }
        if (!ClosestStation.RestrictAssaultSpawningUsingPedSpawns && ClosestStation.PossiblePedSpawns != null && ClosestStation.PossiblePedSpawns.Any())
        {
            foreach(ConditionalLocation cl in ClosestStation.PossiblePedSpawns.Where(x => x.Location.DistanceTo2D(Game.LocalPlayer.Character) >= 20f).ToList())
            {
                PossibleSpawnPlaces.Add(new SpawnPlace(cl.Location,cl.Heading));
            }
            GameFiber.Yield();
        }
        if (PossibleSpawnPlaces.Any())
        {
            foreach(SpawnPlace cl in PossibleSpawnPlaces.OrderBy(x=> Guid.NewGuid()))
            {
                if(NativeFunction.Natives.WOULD_ENTITY_BE_OCCLUDED<bool>(modelHash, cl.Position.X,cl.Position.Y,cl.Position.Z,true))
                {
                    SpawnLocation.InitialPosition = cl.Position;
                    SpawnLocation.Heading = cl.Heading;
                    EntryPoint.WriteToConsole("POSITION IS OCCLUDED, SPAWN THE PED");
                    return true;
                }
                GameFiber.Yield();
            }
        }
        return false;
    }
    private bool GetAssaultSpawnTypes(Agency agency)
    {
        Agency = null;
        VehicleType = null;
        PersonType = null;
        IsOffDutySpawn = false;
        Agency = agency;
        if(Agency == null)
        {
            return false;
        }
        PersonType = Agency.GetRandomPed(World.TotalWantedLevel, "");
        return PersonType != null;
    }
    public void Dispose()
    {
        RemoveRoadblock();
        MarshalDispatcher.Dispose();
    }
    public void Recall()
    {
        if (Settings.SettingsManager.PoliceSpawnSettings.ManageDispatching && IsTimeToRecall)
        {
            GameFiber.Yield();
            foreach (Cop DeleteableCop in DeletableCops)
            {
                if (ShouldCopBeRecalled(DeleteableCop))
                {
                    GameFiber.Yield();
                    Delete(DeleteableCop);
                }
                GameFiber.Yield();
            }
            GameFiber.Yield();
            if (Roadblock != null)//550f)
            {
                float distanceTo = Player.Position.DistanceTo2D(Roadblock.CenterPosition);
                if((Player.IsInVehicle && distanceTo >= 350f) || (!Player.IsInVehicle && distanceTo >= 225f))//remove it a lot closer if you are on foot?//was 550f when in vehicle
                {
                    Roadblock.Dispose();
                    Roadblock = null;
                    //EntryPoint.WriteToConsole($"DISPATCHER: Deleted Roadblock", 3);
                }
            }   
            GameTimeAttemptedRecall = Game.GameTime;
        }
        CleanupAbandonedOrEmpty();
        RepairDamagedVehicles();
    }
    private void RepairDamagedVehicles()
    {
        if (!Settings.SettingsManager.PoliceTaskSettings.AllowFixOffScreenVehiclesAssist)
        {
            return;
        }
        FixDamagedPoliceVehicles();
    }
    public void CleanupAbandonedOrEmpty()
    {
        if (!Settings.SettingsManager.WorldSettings.CleanupVehicles)
        {
            return;
        }
        RemoveAbandonedPoliceVehicles(); 
    }
    private void RemoveAbandonedPoliceVehicles()
    {
        try
        {
            int TotalPoliceCars = World.Vehicles.SpawnedPoliceVehiclesCount;
            int PossibleSpawnedPoliceCars = SpawnedCopVehicleLimit;

            bool isNearLimit = PossibleSpawnedPoliceCars - TotalPoliceCars <= 3 && TotalPoliceCars >= 15;
            int updated = 0;
            foreach (VehicleExt PoliceCar in World.Vehicles.PoliceVehicles.Where(x => !x.IsOwnedByPlayer && x.Vehicle.Exists() && !x.WasSpawnedEmpty && x.HasExistedFor >= 15000 && !x.IsManualCleanup).ToList())
            {
                if (PoliceCar.Vehicle.Exists())
                {
                    if (!PoliceCar.Vehicle.Occupants.Any(x => x.Exists() && x.IsAlive))
                    {
                        PoliceCar.SetBecameEmpty();
                        //float distanceTo = PoliceCar.Vehicle.DistanceTo2D(Game.LocalPlayer.Character);
                        float distanceTo = PoliceCar.DistanceChecker.DistanceToPlayer;
                       // PoliceCar.DistanceChecker.UpdateMovement(distanceTo);
                        if (isNearLimit /*TotalPoliceCars >= 15*/ && PoliceCar.HasBeenEmptyFor >= 60000 && PoliceCar.Vehicle.Exists() && !PoliceCar.Vehicle.IsOnScreen && PoliceCar.Vehicle.IsPersistent)
                        {
                            PoliceCar.Vehicle.IsPersistent = false;
                            EntryPoint.WriteToConsole($"RemoveAbandonedPoliceVehicles 1 NONPERS isNearLimit{isNearLimit} TotalPoliceCars{TotalPoliceCars} PossibleSpawnedPoliceCars{PossibleSpawnedPoliceCars}");
                            GameFiber.Yield();
                        }
                        else if (isNearLimit /*TotalPoliceCars >= 10 */ && distanceTo >= 50f && PoliceCar.HasBeenEmptyFor >= 35000 && PoliceCar.Vehicle.IsPersistent)
                        {
                            PoliceCar.Vehicle.IsPersistent = false;
                            EntryPoint.WriteToConsole($"RemoveAbandonedPoliceVehicles 2 NONPERS isNearLimit{isNearLimit} TotalPoliceCars{TotalPoliceCars} PossibleSpawnedPoliceCars{PossibleSpawnedPoliceCars}");
                            GameFiber.Yield();
                        }
                        else if (PoliceCar.DistanceChecker.IsMovingAway && TotalPoliceCars > 12 && distanceTo >= 200f && PoliceCar.HasBeenEmptyFor >= 20000)//250f)
                        {
                            if (PoliceCar.Vehicle.IsPersistent)
                            {
                                EntryPoint.PersistentVehiclesDeleted++;
                            }
                            EntryPoint.WriteToConsole($"RemoveAbandonedPoliceVehicles 3 DELETE isNearLimit{isNearLimit} TotalPoliceCars{TotalPoliceCars} PossibleSpawnedPoliceCars{PossibleSpawnedPoliceCars}");
                            if (PoliceCar.IsHeli)
                            {
                                OnHelicopterSpawnedOrRecalled();
                            }
                            PoliceCar.FullyDelete();
                            GameFiber.Yield();
                        }
                        else if (PoliceCar.DistanceChecker.IsMovingAway && distanceTo >= 125f && PoliceCar.HasBeenEmptyFor >= 20000 && PoliceCar.Vehicle.IsPersistent)//200f
                        {
                            PoliceCar.Vehicle.IsPersistent = false;
                            EntryPoint.WriteToConsole($"RemoveAbandonedPoliceVehicles 4 NONPERS isNearLimit{isNearLimit} TotalPoliceCars{TotalPoliceCars} PossibleSpawnedPoliceCars{PossibleSpawnedPoliceCars}");
                            GameFiber.Yield();
                        }
                        GameFiber.Yield();
                    }
                    else
                    {
                        PoliceCar.ResetBecameEmpty();
                    }
                }
                if (updated > 10)
                {
                    GameFiber.Yield();
                    updated = 0;
                }
            }
            GameFiber.Yield();
            foreach (VehicleExt PoliceCar in World.Vehicles.PoliceVehicles.Where(x => !x.IsOwnedByPlayer && x.Vehicle.Exists() && x.WasSpawnedEmpty && x.HasExistedFor >= 15000 && !x.IsManualCleanup).ToList())
            {
                if (PoliceCar.Vehicle.Exists())
                {
                    if (!PoliceCar.Vehicle.Occupants.Any(x => x.Exists() && x.IsAlive))
                    {
                        PoliceCar.SetBecameEmpty();
                        float distanceTo = PoliceCar.DistanceChecker.DistanceToPlayer;
                        //float distanceTo = PoliceCar.Vehicle.DistanceTo2D(Game.LocalPlayer.Character);
                        //PoliceCar.DistanceChecker.UpdateMovement(distanceTo);
                        if (PoliceCar.DistanceChecker.IsMovingAway && distanceTo >= 225f && PoliceCar.HasBeenEmptyFor >= 15000)//300f//10000))//225f
                        {
                            PoliceCar.Vehicle.IsPersistent = false;
                            EntryPoint.WriteToConsole($"RemoveAbandonedPoliceVehicles 1 SPAWNED EMPTY NONPERS isNearLimit{isNearLimit} TotalPoliceCars{TotalPoliceCars} PossibleSpawnedPoliceCars{PossibleSpawnedPoliceCars}");
                            GameFiber.Yield();
                        }
                        else if (PoliceCar.DistanceChecker.IsMovingAway && distanceTo >= 300f && PoliceCar.HasBeenEmptyFor >= 10000)//375f//10000))
                        {
                            PoliceCar.Vehicle.IsPersistent = false;
                            EntryPoint.WriteToConsole($"RemoveAbandonedPoliceVehicles 2 SPAWNED EMPTY NONPERS isNearLimit{isNearLimit} TotalPoliceCars{TotalPoliceCars} PossibleSpawnedPoliceCars{PossibleSpawnedPoliceCars}");
                            GameFiber.Yield();
                        }
                    }
                    else
                    {
                        PoliceCar.ResetBecameEmpty();
                    }
                }
            }
            GameFiber.Yield();//TR 29
        }
        catch (InvalidOperationException ex)
        {
            EntryPoint.WriteToConsole($"Remove Abandoned Vehicles, Collection Modified Error: {ex.Message} {ex.StackTrace}", 0);
        }
    }
    private void FixDamagedPoliceVehicles()
    {
        int updated = 0;
        foreach (VehicleExt PoliceCar in World.Vehicles.PoliceVehicles.Where(x => !x.IsOwnedByPlayer && x.Vehicle.Exists() && x.WasModSpawned && x.HasExistedFor >= 15000).ToList())
        {
            if (PoliceCar.Vehicle.Exists())
            {
                if (PoliceCar.Vehicle.HasOccupants && (PoliceCar.Vehicle.Health < PoliceCar.Vehicle.MaxHealth - 500 || PoliceCar.Vehicle.EngineHealth < 200f) && PoliceCar.Vehicle.DistanceTo2D(Game.LocalPlayer.Character) >= 25f && !PoliceCar.Vehicle.IsOnScreen)
                {
                    PoliceCar.Vehicle.Repair();
                    GameFiber.Yield();
                }
                // GameFiber.Yield();//TR 29
            }
            if (updated > 10)
            {
                GameFiber.Yield();
                updated = 0;
            }
        }
    }
    private void HandleAmbientSpawns()
    {
        if (!IsTimeToAmbientDispatch || !HasNeedToAmbientDispatch)
        {
            return;
        }
        //EntryPoint.WriteToConsole("LE DISPATCHER HAS DISPATCHED RAN2");
        HasDispatchedThisTick = true;//up here for now, might be better down low
        if (ShouldRunAmbientDispatch)
        {
            //EntryPoint.WriteToConsoleTestLong($"AMBIENT COP RunAmbientDispatch 1 TimeBetweenSpawn{TimeBetweenSpawn}");
            RunAmbientDispatch();
        }
        else
        {
            ShouldRunAmbientDispatch = RandomItems.RandomPercent(PercentageOfAmbientSpawn);
            if (ShouldRunAmbientDispatch)
            {
                //EntryPoint.WriteToConsoleTestLong($"AMBIENT COP RunAmbientDispatch 2 TimeBetweenSpawn{TimeBetweenSpawn}");
                RunAmbientDispatch();
            }
            else
            {
                //EntryPoint.WriteToConsoleTestLong($"AMBIENT COP Aborting Spawn for this dispatch TimeBetweenSpawn{TimeBetweenSpawn} PercentageOfAmbientSpawn{PercentageOfAmbientSpawn}");
                GameTimeAttemptedDispatch = Game.GameTime;
            }
        }
    }
    private void RunAmbientDispatch()
    {
        //EntryPoint.WriteToConsole($"AMBIENT COP SPAWN RunAmbientDispatch ShouldRunAmbientDispatch{ShouldRunAmbientDispatch}: %{PercentageOfAmbientSpawn} TimeBetween:{TimeBetweenSpawn} SpawnedCopLimit:{SpawnedCopLimit}");
        bool getspawnLocation = GetSpawnLocation();
        GameFiber.Yield();
        bool getSpawnTypes = GetSpawnTypes();
        //EntryPoint.WriteToConsole($"Attempt {Agency?.ShortName}  {VehicleType?.ModelName} {VehicleType?.DebugName} HasNeedToSpawnBoat {HasNeedToSpawnBoat} {getspawnLocation} {getSpawnTypes}");
        //EntryPoint.WriteToConsole($"getspawnLocation:{getspawnLocation} getSpawnTypes:{getSpawnTypes}");
        if (getspawnLocation && getSpawnTypes)
        {
            GameFiber.Yield();
            GameTimeAttemptedDispatch = Game.GameTime;
            //EntryPoint.WriteToConsoleTestLong($"AMBIENT COP CALLED SPAWN TASK");

            bool allowAny = false;
            if(IsTunnelSpawn)
            {
                allowAny = true;
            }

            if (CallSpawnTask(allowAny, true, false, false, TaskRequirements.None, false, IsOffDutySpawn, false))
            {
                //EntryPoint.WriteToConsoleTestLong($"AMBIENT COP SPAWN TASK RAN");
                ShouldRunAmbientDispatch = false;
                //GameTimeAttemptedDispatch = Game.GameTime;
            }
        }
    }
    private void HandleRoadblockSpawns()
    {
        if (IsTimeToDispatchRoadblock && HasNeedToDispatchRoadblock)
        {
            GameFiber.Yield();
            bool enableCarBlocks = true;
            if(!Settings.SettingsManager.RoadblockSettings.RoadblockCarBlocksEnabled)
            {
                enableCarBlocks = false;
            }
            bool enableSpikeStrips = true;
            if(!Settings.SettingsManager.RoadblockSettings.RoadblockSpikeStripsEnabled)
            {
                enableSpikeStrips = false;
            }
            bool enableOtherBarriers = true;
            if (!Settings.SettingsManager.RoadblockSettings.RoadblockOtherBarriersEnabled)
            {
                enableOtherBarriers = false;
            }

            if(Player.WantedLevel <= 2)
            {
                enableCarBlocks = false;
            }

            if (RandomItems.RandomPercent(CurrentRoadblockSpawnChance()))
            {
                SpawnRoadblock(false, enableCarBlocks, enableSpikeStrips, enableOtherBarriers, Settings.SettingsManager.RoadblockSettings.RoadblockSpawnDistance);// 225f);//300f
            }
            else
            {
                GameTimeLastSpawnedRoadblock = Game.GameTime;
                EntryPoint.WriteToConsole("Roadblock not spawning (chance)");
            }
        }
    }
    private bool CallSpawnTask(bool allowAny, bool allowBuddy, bool isLocationSpawn, bool clearArea, TaskRequirements spawnRequirement, bool forcek9, bool isOffDuty, bool spawnsWithAllWeapons)
    {
        try
        {
            GameFiber.Yield();
            bool addOptionalPassengers = RandomItems.RandomPercent(Settings.SettingsManager.PoliceSpawnSettings.AddOptionalPassengerPercentage);
            bool addCanine = HasNeedToAmbientCanineDispatch;// && RandomItems.RandomPercent(Settings.SettingsManager.PoliceSpawnSettings.AddK9Percentage);

            EntryPoint.WriteToConsole($"addCanine {addCanine} HasNeedToAmbientCanineDispatch {HasNeedToAmbientCanineDispatch}");

            if (forcek9)
            {
                addCanine = true;
            }

            if (VehicleType != null && VehicleType.CaninePossibleSeats != null && VehicleType.CaninePossibleSeats.Any())//already picked a k9 car, spawn it anways
            {
                addCanine = true;

                EntryPoint.WriteToConsole($"already picked k9 car, continue spawning");

            }

            LESpawnTask spawnTask = new LESpawnTask(Agency, SpawnLocation, VehicleType, PersonType, Settings.SettingsManager.PoliceSpawnSettings.ShowSpawnedBlips, Settings, Weapons, Names, addOptionalPassengers, World, ModItems, addCanine);
            spawnTask.AllowAnySpawn = allowAny;
            spawnTask.AllowBuddySpawn = allowBuddy && !isOffDuty;
            spawnTask.ClearVehicleArea = clearArea;
            spawnTask.SpawnRequirement = spawnRequirement;
            spawnTask.IsOffDutySpawn = isOffDuty;
            spawnTask.SpawnWithAllWeapons = spawnsWithAllWeapons;
            if(IsTunnelSpawn)
            { 
                spawnTask.PlacePedOnGround = false;
            }
            // spawnTask.PlacePedOnGround = VehicleType == null;
            EntryPoint.WriteToConsole($"DEBUG LE DISPATCH CallSpawnTask: VehicleType:{VehicleType?.ModelName} PersonType:{PersonType?.ModelName} RequiredPedGroup:{VehicleType?.RequiredPedGroup} GroupName:{PersonType?.GroupName}");
            spawnTask.AttemptSpawn();
            GameFiber.Yield();
            spawnTask.CreatedPeople.ForEach(x => { World.Pedestrians.AddEntity(x); x.IsLocationSpawned = isLocationSpawn; });
            spawnTask.CreatedVehicles.ForEach(x => x.AddVehicleToList(World));
            HasDispatchedThisTick = true;
            Player.OnLawEnforcementSpawn(Agency, VehicleType, PersonType);
            return spawnTask.CreatedPeople.Any(x => x.Pedestrian.Exists());
        }
        catch (Exception ex)
        {        
            EntryPoint.WriteToConsole($"LE Dispatcher Spawn Error: {ex.Message} : {ex.StackTrace}", 0);
            return false;
        }
    }
    private bool GetSpawnLocation()
    {
        int timesTried = 0;
        bool isValidSpawn = false;
        PoliceStation = null;
        SpawnLocation = new SpawnLocation();
        bool tryBoat = HasNeedToSpawnBoat;
        IsTunnelSpawn = false;
        SelectedTunnelSpawn = null;


        if(CheckSpecialSpawnCases())
        {
            return true;
        }
        do//regular spawn place checking
        {
            SpawnLocation.InitialPosition = GetSpawnPosition();
            if(tryBoat)
            {
                SpawnLocation.GetWaterHeight();
            }
            if(tryBoat && SpawnLocation.IsWater)
            {
                isValidSpawn = IsValidSpawn(SpawnLocation.InitialPosition);
            }
            else
            {
                SpawnLocation.GetClosestStreet(Player.IsWanted);
                SpawnLocation.GetClosestSidewalk();
                GameFiber.Yield();
                isValidSpawn = AreSpawnsValidSpawn(SpawnLocation);
            }
            timesTried++;
            GameFiber.Yield();
        }
        while (!SpawnLocation.HasSpawns && !isValidSpawn && timesTried < 3);//2//10
        return isValidSpawn && SpawnLocation.HasSpawns;
    }
    private bool CheckSpecialSpawnCases()
    {
        if (Player.WantedLevel >= Settings.SettingsManager.PoliceSpawnSettings.TunnelSpawnWantedLevelMinimum && Player.CurrentLocation.TreatAsInTunnel && (Player.CurrentLocation.IsOffroad || Player.CurrentLocation.IsInside))
        {
            int totalCalc = 0;
            foreach (StoredSpawn spawnPlace in PlacesOfInterest.PossibleLocations.StoredSpawns)
            {
                if(totalCalc > 10)
                {
                    GameFiber.Yield();
                    totalCalc = 0;
                }
                int CellsAway = NativeHelper.MaxCellsAway(EntryPoint.FocusCellX, EntryPoint.FocusCellY, spawnPlace.CellX, spawnPlace.CellY);
                if (CellsAway > 7)
                {
                    continue;
                }
                float distanceTo = spawnPlace.Position.DistanceTo(Player.Position);
                if (distanceTo <= spawnPlace.MaxSpawnDistance && distanceTo >= spawnPlace.MinSpawnDistance)
                {
                    SpawnLocation.InitialPosition = spawnPlace.Position;
                    SpawnLocation.Heading = spawnPlace.Heading;
                    SpawnLocation.StreetPosition = spawnPlace.Position;
                    SpawnLocation.SidewalkPosition = spawnPlace.Position;
                    EntryPoint.WriteToConsole("LE DISPATCHER TUNNEL DETECTION SPAWNING NEARBY");

                    IsTunnelSpawn = true;
                    SelectedTunnelSpawn = spawnPlace;

                    return true;
                }
                totalCalc++;
            }
        }
        return false;
    }
    private bool GetSpawnTypes()
    {
        Agency = null;
        VehicleType = null;
        PersonType = null;
        IsOffDutySpawn = false;
        if (SpawnLocation.IsWater && !HasNeedToSpawnBoat && !HasNeedToSpawnHeli)
        {
            EntryPoint.WriteToConsole("Spawn Location is water and no need to spawn heli or boat, exit");
            return false;
        }
        Zone zoneAtPosition = Zones.GetZone(SpawnLocation.FinalPosition);
        Street streetAtPosition = Streets.GetStreet(SpawnLocation.FinalPosition);   
        Agency = GetRandomAgency(SpawnLocation, zoneAtPosition, streetAtPosition);
        ZoneJurisdiction zoneJurisdiction = Jurisdictions.GetJurisdiction(zoneAtPosition?.InternalGameName, Agency);
        GameFiber.Yield();
        bool isAmbientPedSpawn = false;
        if (Agency == null)
        {
            return false;
        }
        IsOffDutySpawn = RandomItems.RandomPercent(Agency.OffDutyDispatchPercent) && World.TotalWantedLevel == 0 && Agency.OffDutyVehicles != null && Agency.OffDutyVehicles.Any() && Agency.OffDutyPersonnel != null && Agency.OffDutyPersonnel.Any();
        if (SpawnLocation.IsWater)
        {
            VehicleType = Agency.GetRandomWaterVehicle(World.TotalWantedLevel, "", Settings);
            IsOffDutySpawn = false;
        }
        else 
        {
            if(IsTunnelSpawn && SelectedTunnelSpawn != null && SelectedTunnelSpawn.IsPedestrianOnlySpawn)
            {
                isAmbientPedSpawn = true;
            }
            else if(World.TotalWantedLevel == 0 && SpawnLocation.HasSidewalk && RandomItems.RandomPercent(Settings.SettingsManager.PoliceSpawnSettings.FootPatrolSpawnPercentage) && zoneJurisdiction != null && zoneJurisdiction.CanSpawnPedestrianOfficers)
            {
                isAmbientPedSpawn = true;
                //EntryPoint.WriteToConsole("LE Dispatcher IS FOOT SPAWN");
            }          
            else
            {
                if (IsOffDutySpawn)
                {
                    VehicleType = Agency.GetRandomOffDutyVehicle(World.TotalWantedLevel, HasNeedToSpawnHeli, false, true, "", Settings);
                }
                else
                {
                    eSpawnAdjustment addedSpawnAdjustments = eSpawnAdjustment.None;
                    bool addedAdjustment = false;
                    if(zoneAtPosition != null && (zoneAtPosition.Type == eLocationType.Wilderness || (zoneAtPosition.Type == eLocationType.Rural && Player.CurrentLocation.IsOffroad)))
                    {
                        addedSpawnAdjustments |= eSpawnAdjustment.OffRoad;
                        addedAdjustment = true;
                    }
                    if (streetAtPosition != null && streetAtPosition.IsHighway)
                    {
                        addedSpawnAdjustments |= eSpawnAdjustment.Highway;
                        addedAdjustment = true;
                    }
                    if(zoneJurisdiction != null && zoneJurisdiction.CanSpawnBicycleOfficers && (Player.IsNotWanted || !Player.IsInVehicle))
                    {
                        addedSpawnAdjustments |= eSpawnAdjustment.Bicycle;
                        addedAdjustment = true;
                        EntryPoint.WriteToConsole("LE DISPATCHER ADDED BIKE ADJUSTMENT");
                    }
                    if (zoneJurisdiction != null && zoneJurisdiction.CanSpawnDirtBikeOfficers)
                    {
                        addedSpawnAdjustments |= eSpawnAdjustment.DirtBike | eSpawnAdjustment.ATV;
                        addedAdjustment = true;
                        EntryPoint.WriteToConsole("LE DISPATCHER ADDED DIRT BIKE ATV ADJUSTMENT");
                    }
                    if(Player.IsInAirVehicle)
                    {
                        addedSpawnAdjustments |= eSpawnAdjustment.InAirVehicle;
                        addedAdjustment = true;
                        EntryPoint.WriteToConsole("LE DISPATCHER ADDED AIR VEHICLE ADJUSTMENT");
                    }

                    if (HasNeedToAmbientCanineDispatch)
                    {
                        addedSpawnAdjustments |= eSpawnAdjustment.K9;
                        addedAdjustment = true;
                        EntryPoint.WriteToConsole("LE DISPATCHER ADDED K9 VEHICLE ADJUSTMENT");
                    }

                    if (addedAdjustment)
                    {
                        VehicleType = Agency.GetRandomAdjustedVehicle(World.TotalWantedLevel, HasNeedToSpawnHeli, false, true, "", Settings, addedSpawnAdjustments);
                    }
                    else
                    {
                        VehicleType = Agency.GetRandomVehicle(World.TotalWantedLevel, HasNeedToSpawnHeli, false, true, "", Settings);
                    }
                }
            }
        }
        GameFiber.Yield();
        if (VehicleType == null && !isAmbientPedSpawn)
        {
            return false;
        }
        string RequiredGroup = "";
        if (VehicleType != null)
        {
            RequiredGroup = VehicleType.RequiredPedGroup;
        }
        GameFiber.Yield();
        if (IsOffDutySpawn)
        {
            PersonType = Agency.GetRandomOffDutyPed(World.TotalWantedLevel, RequiredGroup);
        }
        else
        {
            PersonType = Agency.GetRandomPed(World.TotalWantedLevel, RequiredGroup);
        }
        return PersonType != null;
    }
    private void Delete(PedExt Cop)
    {
        if (Cop != null && Cop.Pedestrian.Exists())
        {
            if(Cop.Pedestrian.Handle == Game.LocalPlayer.Character.Handle)
            {
                return;
            }

            if(Cop.IsAnimal)
            {
                GameTimeLastSpawnedOrRecalledCanineUnit = Game.GameTime;
            }

            //EntryPoint.WriteToConsole($"Attempting to Delete {Cop.Pedestrian.Handle}");
            if (Cop.Pedestrian.IsInAnyVehicle(false))
            {
                if (Cop.Pedestrian.CurrentVehicle.HasPassengers)
                {
                    foreach (Ped Passenger in Cop.Pedestrian.CurrentVehicle.Passengers)
                    {
                        if (Passenger.Handle != Game.LocalPlayer.Character.Handle)
                        {
                            RemoveBlip(Passenger);
                            Passenger.Delete();

                            //Passenger.IsPersistent = false;

                            EntryPoint.PersistentPedsDeleted++;
                            GameFiber.Yield();
                        }
                    }
                }
                if (Cop.Pedestrian.Exists() && Cop.Pedestrian.CurrentVehicle.Exists() && Cop.Pedestrian.CurrentVehicle != null)
                {
                    Blip carBlip = Cop.Pedestrian.CurrentVehicle.GetAttachedBlip();
                    if (carBlip.Exists())
                    {
                        carBlip.Delete();
                    }
                    VehicleExt vehicleExt =  World.Vehicles.GetVehicleExt(Cop.Pedestrian.CurrentVehicle);
                    if (vehicleExt != null)
                    {
                        if(vehicleExt.IsHeli)
                        {
                            OnHelicopterSpawnedOrRecalled();
                        }
                        vehicleExt.FullyDelete();
                    }
                    else
                    {
                        Cop.Pedestrian.CurrentVehicle.Delete();
                    }
                    EntryPoint.PersistentVehiclesDeleted++;
                    GameFiber.Yield();
                }
            }
            RemoveBlip(Cop.Pedestrian);
            if (Cop.Pedestrian.Exists())
            {
                //EntryPoint.WriteToConsole(string.Format("Delete Cop Handle: {0}, {1}, {2}", Cop.Pedestrian.Handle, Cop.DistanceToPlayer, Cop.AssignedAgency.Initials));
                Cop.Pedestrian.Delete();
                EntryPoint.PersistentPedsDeleted++;
                GameFiber.Yield();
            }
        }
    }
    private void RemoveBlip(Ped MyPed)
    {
        if (!MyPed.Exists())
        {
            return;
        }
        Blip MyBlip = MyPed.GetAttachedBlip();
        if (MyBlip.Exists())
        {
            MyBlip.Delete();
        }
    }
    private List<Agency> GetAgencies(Vector3 Position, int WantedLevel, Zone positionZone, Street positionStreet)
    {
        List<Agency> ToReturn = new List<Agency>();
        if (positionStreet != null && positionStreet.IsHighway) //Highway Patrol Jurisdiction
        {
            ToReturn.AddRange(Agencies.GetSpawnableHighwayAgencies(WantedLevel, ResponseType.LawEnforcement));
        }
        if (positionZone == null)
        {
            ToReturn.AddRange(Agencies.GetSpawnableAgencies(WantedLevel, ResponseType.LawEnforcement));
            return ToReturn;
        }
        Agency ZoneAgency = Jurisdictions.GetRandomAgency(positionZone.InternalGameName, WantedLevel, ResponseType.LawEnforcement);
        if (ZoneAgency != null)
        {
            ToReturn.Add(ZoneAgency); //Zone Jurisdiciton Random
        }
        if (!ToReturn.Any() || RandomItems.RandomPercent(LikelyHoodOfCountySpawn))
        {
            Agency CountyAgency = Jurisdictions.GetRandomCountyAgency(positionZone.CountyID, WantedLevel, ResponseType.LawEnforcement);
            if (CountyAgency != null)//randomly spawn the county agency
            {
                ToReturn.Add(CountyAgency); //Zone Jurisdiciton Random
            }
            //EntryPoint.WriteToConsole("ATTEMPING TO ADD COUNTY AGENCY!");
        }
        if (!ToReturn.Any() || RandomItems.RandomPercent(LikelyHoodOfAnySpawn))//fall back to anybody
        {
            ToReturn.AddRange(Agencies.GetSpawnableAgencies(WantedLevel, ResponseType.LawEnforcement));
            //EntryPoint.WriteToConsole("ATTEMPING TO ADD ANY AGENCY!");
        }
        return ToReturn;
    }
    private Vector3 GetSpawnPosition()
    {
        Vector3 Position;
        if(World.TotalWantedLevel > 0 && (World.TotalWantedLevel > Player.WantedLevel || Player.IsNotWanted))//someone else is the priority
        {
            Position = World.PoliceBackupPoint;
        }
        else//player is priority
        {
            if(Player.IsWanted && Player.IsInVehicle && Player.IsMoving)//if you are wanted and in a car, put it out front to better get it
            {
                Position = Player.Character.GetOffsetPositionFront(250f);// 200f);// 250f);//350f
            }
            else if (Player.Investigation.IsActive)//investigations mode takes over too?
            {
                Position = Player.Investigation.Position;
            }
            else
            {
                Position = Player.Position;
            }
        }
        Position = Position.Around2D(MinDistanceToSpawn, MaxDistanceToSpawn);
        return Position;
    }
    private Agency GetRandomAgency(SpawnLocation spawnLocation, Zone spawnZone, Street spawnStreet)
    {
        Agency agency = null;
        List<Agency> PossibleAgencies = GetAgencies(spawnLocation.FinalPosition, World.TotalWantedLevel, spawnZone, spawnStreet);

        EntryPoint.WriteToConsole($"LEDIS GetRandomAgency 1 {string.Join(",",PossibleAgencies)}");

        if(PossibleAgencies == null)
        {
            return agency;
        }
        agency = PossibleAgencies.Where(x=>x.Personnel.Any(y =>y.CanCurrentlySpawn(World.TotalWantedLevel))).PickRandom();
        EntryPoint.WriteToConsole($"LEDIS GetRandomAgency 2 SELECTED {agency?.FullName}");
        if (agency == null)
        {
            agency = GetAgencies(spawnLocation.InitialPosition, World.TotalWantedLevel, spawnZone, spawnStreet).Where(x => x.Personnel.Any(y => y.CanCurrentlySpawn(World.TotalWantedLevel))).PickRandom();
            EntryPoint.WriteToConsole($"LEDIS GetRandomAgency 3 SELECTED {agency?.FullName}");
        }
        if (agency == null)
        {
            //EntryPoint.WriteToConsoleTestLong("Dispatcher could not find Agency To Spawn");
        }
        return agency;
    }
    private bool AreSpawnsValidSpawn(SpawnLocation spawnLocation)
    {
        if (spawnLocation.FinalPosition.DistanceTo2D(Player.Position) < ClosestPoliceSpawnToSuspectAllowed || World.Pedestrians.AnyCopsNearPosition(spawnLocation.FinalPosition, ClosestPoliceSpawnToOtherPoliceAllowed))
        {
            return false;
        }
        //else if (spawnLocation.InitialPosition.DistanceTo2D(Player.Position) < ClosestPoliceSpawnToSuspectAllowed || World.Pedestrians.AnyCopsNearPosition(spawnLocation.InitialPosition, ClosestPoliceSpawnToOtherPoliceAllowed))
        //{
        //    return false;
        //}
        return true;
    }
    private bool IsValidSpawn(Vector3 location)
    {
        if (location.DistanceTo2D(Player.Position) < ClosestPoliceSpawnToSuspectAllowed || World.Pedestrians.AnyCopsNearPosition(location, ClosestPoliceSpawnToOtherPoliceAllowed))
        {
            return false;
        }
        return true;
    }
    private bool ShouldCopBeRecalled(Cop cop)
    {
        int totalCopsNearCop = World.Pedestrians.TotalCopsNearCop(cop, 3);
        bool anyCopsNearCop = totalCopsNearCop > 0;
        if (!cop.AssignedAgency.CanSpawn(World.TotalWantedLevel))
        {
            EntryPoint.WriteToConsole($"{cop.Handle} Distance {cop.DistanceToPlayer} DELETE COP, CANNOT SPAWN AGENCY");
            return true;
        }
        else if (!cop.WasModSpawned && cop.DistanceToPlayer >= 175f)
        {
            EntryPoint.WriteToConsole($"{cop.Handle} Distance {cop.DistanceToPlayer} DELETE COP, NON MOD SPAWNED COPS");
            return true;
        }
        else if (cop.IsInVehicle && cop.DistanceToPlayer > DistanceToDelete && cop.DistanceChecker.IsMovingAway) //Beyond Caring
        {
            EntryPoint.WriteToConsole($"{cop.Handle} Distance {cop.DistanceToPlayer} DELETE COP, IN VEHICLE DELETE");
            return true;
        }
        else if (!cop.IsInVehicle && !cop.IsRoadblockSpawned && (!cop.WasModSpawned || cop.HasBeenSpawnedFor >= 25000) && (cop.DistanceToPlayer > DistanceToDeleteOnFoot) && cop.DistanceChecker.IsMovingAway)// || cop.DistanceToPlayer > 300f))//else if (!cop.IsInVehicle && !cop.IsRoadblockSpawned && (!cop.WasModSpawned || cop.HasBeenSpawnedFor >= 25000) && ((cop.DistanceToPlayer > DistanceToDeleteOnFoot && cop.ClosestDistanceToPlayer <= 50f) || cop.DistanceToPlayer > 300f)) //Beyond Caring
        {
            EntryPoint.WriteToConsole($"{cop.Handle} Distance {cop.DistanceToPlayer} DELETE COP, NOT IN VEHICLE DELETE NOT ROADBLOCK");
            return true;
        }
        //Wanted Cleanups below
        else if (!cop.IsInVehicle && cop.IsRoadblockSpawned && cop.HasBeenSpawnedFor >= 25000 && cop.DistanceToPlayer > 350f) //Beyond Caring
        {
            EntryPoint.WriteToConsole($"{cop.Handle} Distance {cop.DistanceToPlayer} DELETE COP, NOT IN VEHICLE DELETE YES ROADBLOCK");
            return true;
        }
        else if (Player.IsWanted && cop.IsInVehicle && cop.DistanceToPlayer >= 300f && !cop.IsRespondingToWanted && !cop.IsInHelicopter)
        {
            EntryPoint.WriteToConsole($"{cop.Handle} Distance {cop.DistanceToPlayer} DELETE COP, Ambient NOT RESPONDING");
            return true;
        }
        else if (Player.IsWanted && cop.DistanceToPlayer >= 300f && cop.ClosestDistanceToPlayer <= 15f && !cop.IsInHelicopter) //Got Close and Then got away, when wanted
        {
            EntryPoint.WriteToConsole($"{cop.Handle} Distance {cop.DistanceToPlayer} DELETE COP, CLOSE THEN FAR");
            return true;
        }
        else if (Player.IsWanted && !cop.IsInHelicopter && cop.DistanceToPlayer >= 250f && cop.ClosestDistanceToPlayer <= 35f && anyCopsNearCop && !cop.Pedestrian.IsOnScreen)//else if (!cop.IsInHelicopter && cop.DistanceToPlayer >= 150f && cop.ClosestDistanceToPlayer <= 35f && anyCopsNearCop && !cop.Pedestrian.IsOnScreen)
        {
            EntryPoint.WriteToConsole($"{cop.Handle} Distance {cop.DistanceToPlayer} DELETE COP, LAST ONE");
            return true;
        }
        else if (Player.IsWanted && !cop.IsInHelicopter && cop.DistanceToPlayer >= 350f && totalCopsNearCop >= 4 && !cop.Pedestrian.IsOnScreen)//else if (!cop.IsInHelicopter && cop.DistanceToPlayer >= 300f && totalCopsNearCop >= 4 && !cop.Pedestrian.IsOnScreen)
        {
            EntryPoint.WriteToConsole($"{cop.Handle} Distance {cop.DistanceToPlayer} DELETE COP, LAST total COPS");
            return true;
        }
       // if(cop.DistanceToPlayer >= 150f && cop.IsUnconscious)
        return false;
    }
    public void SpawnRoadblock(bool force, bool enableCarBlocks, bool enableSpikeStrips, bool enableOtherBarriers, float distance)//temp public
    {
        GetRoadblockLocation(force, distance);
        GameFiber.Yield();
        if(!GetRoadblockNode(force))
        {
            return;
        }
        Zone zoneAtPosition = Zones.GetZone(RoadblockFinalPosition);
        Street streetAtPosition = Streets.GetStreet(RoadblockFinalPosition);
        SpawnLocation roadblockSpawnLocations = new SpawnLocation(RoadblockFinalPosition);
        Agency ToSpawn = GetRandomAgency(roadblockSpawnLocations, zoneAtPosition, streetAtPosition);
        GameFiber.Yield();
        if(ToSpawn == null)
        { 
            return;
        }
        DispatchableVehicle VehicleToUse = ToSpawn.GetRandomVehicle(World.TotalWantedLevel, false, false, false, "", Settings);
        GameFiber.Yield();
        if(VehicleToUse == null)
        {
            return;
        }
        string RequiredGroup = "";
        if (VehicleToUse != null)
        {
            RequiredGroup = VehicleToUse.RequiredPedGroup;
        }
        DispatchablePerson OfficerType = ToSpawn.GetRandomPed(World.TotalWantedLevel, RequiredGroup);
        GameFiber.Yield();
        if(OfficerType == null)
        {
            return;
        }
        if (Roadblock != null)
        {
            Roadblock.Dispose();
            GameFiber.Yield();
        }
        Roadblock = new Roadblock(Player, World, ToSpawn, VehicleToUse, OfficerType, RoadblockFinalPosition, RoadblockFinalHeading, Settings, Weapons, Names, force, ModItems, enableCarBlocks,enableSpikeStrips,enableOtherBarriers);
        Roadblock.SpawnRoadblock();
        GameFiber.Yield();
        GameTimeLastSpawnedRoadblock = Game.GameTime;    
    }
    private void GetRoadblockLocation(bool force, float forceDistance)
    {
        float distance = 300f;
        if(force)
        {
            distance = forceDistance;
        }    
        RoadblockInitialPosition = Player.Character.GetOffsetPositionFront(distance);//400f 400 is mostly far enough to not see it
        RoadblockAwayPosition = Player.Character.GetOffsetPositionFront(distance + 100f);
        RoadblockInitialPositionStreet = Streets.GetStreet(RoadblockInitialPosition);
        RoadblockFinalPosition = Vector3.Zero;
        RoadblockFinalHeading = 0f;
    }
    private bool GetRoadblockNode(bool force)
    {

        if (RoadblockInitialPositionStreet?.Name == Player.CurrentLocation.CurrentStreet?.Name || force || Settings.SettingsManager.RoadblockSettings.AllowRoadblockOnNonCurrentStreet)
        {
            bool hasNode = false;

            Vector3 desiredHeadingPos = RoadblockAwayPosition;// Game.LocalPlayer.Character.Position;
                hasNode = NativeFunction.Natives.GET_NTH_CLOSEST_VEHICLE_NODE_FAVOUR_DIRECTION<bool>(RoadblockInitialPosition.X, RoadblockInitialPosition.Y, RoadblockInitialPosition.Z, desiredHeadingPos.X, desiredHeadingPos.Y, desiredHeadingPos.Z
                    , 0, out RoadblockFinalPosition, out RoadblockFinalHeading, 0, 0x40400000, 0);
            //}
            //else
            //{
            //    hasNode = NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(RoadblockInitialPosition.X, RoadblockInitialPosition.Y, RoadblockInitialPosition.Z, out RoadblockFinalPosition, out RoadblockFinalHeading, 0, 3, 0);
            //}



            if (hasNode)//NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(RoadblockInitialPosition.X, RoadblockInitialPosition.Y, RoadblockInitialPosition.Z, out RoadblockFinalPosition, out RoadblockFinalHeading, 1, 3.0f, 0))
            {


                //if (NativeFunction.Natives.GET_NTH_CLOSEST_VEHICLE_NODE_FAVOUR_DIRECTION<bool>(RoadblockInitialPosition.X, RoadblockInitialPosition.Y, RoadblockInitialPosition.Z, RoadblockAwayPosition.X, RoadblockAwayPosition.Y, RoadblockAwayPosition.Z
                //    , 0, out RoadblockFinalPosition, out RoadblockFinalHeading, Settings.SettingsManager.PoliceSettings.RoadblockNodeType, 0x40400000, 0))
                //    { 
                float headingDiff = Math.Abs(Extensions.GetHeadingDifference(Game.LocalPlayer.Character.Heading, RoadblockFinalHeading));
                //EntryPoint.WriteToConsoleTestLong($"Roadblock RoadblockFinalPosition {RoadblockFinalPosition} RoadblockFinalHeading {RoadblockFinalHeading} PlayerHeading: {Game.LocalPlayer.Character.Heading} {headingDiff}");
                if (headingDiff > 50f)
                {
                    return false;
                }

                //int StreetHash = 0;
                //int CrossingHash = 0;
                //unsafe
                //{
                //    NativeFunction.Natives.GET_STREET_NAME_AT_COORD(RoadblockFinalPosition.X, RoadblockFinalPosition.Y, RoadblockFinalPosition.Z, out StreetHash, out CrossingHash);
                //}
                //if(CrossingHash != 0)
                //{
                //    EntryPoint.WriteToConsole("Roadblock location is near another road, failing");
                //    return false;
                //}
                    return true;
            }
        }
        return false;
     }
    public void RemoveRoadblock()//temp public
    {
        if (Roadblock != null)
        {
            Roadblock.Dispose();
            Roadblock = null;

        }
    }
    public void DebugSpawnCop(string agencyID, bool onFoot, bool isEmpty, DispatchableVehicle vehicleType, DispatchablePerson personType, bool forcek9)
    {
        VehicleType = null;
        PersonType = null;
        Agency = null;
        IsOffDutySpawn = false;
        //EntryPoint.WriteToConsoleTestLong($"DEBUG SPAWN COP agencyID: {agencyID} onFoot: {onFoot}");
        SpawnLocation = new SpawnLocation();
        SpawnLocation.InitialPosition = Game.LocalPlayer.Character.GetOffsetPositionFront(10f);
        if (Game.LocalPlayer.Character.DistanceTo2D(new Vector3(682.6665f, 668.7299f, 128.4526f)) <= 30f)
        {
            SpawnLocation.InitialPosition = new Vector3(682.6665f, 668.7299f, 128.4526f);
            SpawnLocation.Heading = 189.3264f;
        }

        if (Game.LocalPlayer.Character.DistanceTo2D(new Vector3(229.028f, -988.8007f, -99.52672f)) <= 30f)
        {
            SpawnLocation.InitialPosition = new Vector3(229.028f, -988.8007f, -99.52672f);
            SpawnLocation.Heading = 358.3758f;
        }


        SpawnLocation.Heading = Game.LocalPlayer.Character.Heading;
        SpawnLocation.StreetPosition = SpawnLocation.InitialPosition;
        if (agencyID == "")
        {
            Agency = Agencies.GetRandomAgency(ResponseType.LawEnforcement);
        }
        else
        {
            Agency = Agencies.GetAgency(agencyID);
        }
        if (Agency == null)
        {
            //EntryPoint.WriteToConsoleTestLong($"DEBUG SPAWN COP NO AGENCY FOUND");
            return;
        }

        if (World.TotalWantedLevel == 0 && Agency.OffDutyDispatchPercent > 0 && RandomItems.RandomPercent(Agency.OffDutyDispatchPercent))
        {
            IsOffDutySpawn = true;
            //EntryPoint.WriteToConsole($"DEBUG LE DISPATCH IS OFF DUTY SPAWN");
        }



        if (!onFoot)
        {
            if (IsOffDutySpawn)
            {
                VehicleType = Agency.GetRandomOffDutyVehicle(World.TotalWantedLevel, true, true, true, "", Settings);
            }
            else
            {
                VehicleType = Agency.GetRandomVehicle(World.TotalWantedLevel, true, true, true, "", Settings);
            }
        }

        //EntryPoint.WriteToConsole($"DEBUG LE DISPATCH vehicleType: {VehicleType?.ModelName}");

        if (forcek9)
        {
            VehicleType = Agency.GetRandomCanineVehicle(World.TotalWantedLevel, "", Settings);
        }


        if (VehicleType != null || onFoot)
        {
            string RequiredGroup = "";
            if (VehicleType != null)
            {
                RequiredGroup = VehicleType.RequiredPedGroup;
            }

            if (IsOffDutySpawn)
            {
                PersonType = Agency.GetRandomOffDutyPed(World.TotalWantedLevel, RequiredGroup);
            }
            else
            {
                PersonType = Agency.GetRandomPed(World.TotalWantedLevel, RequiredGroup);
            }
          //  EntryPoint.WriteToConsole($"DEBUG LE DISPATCH RequiredGroup: {RequiredGroup} HasOffDuty:{Agency.OffDutyPersonnel?.Any()}");
        }


       // EntryPoint.WriteToConsole($"DEBUG LE DISPATCH vehicleType: {VehicleType?.ModelName}");
       // EntryPoint.WriteToConsole($"DEBUG LE DISPATCH PERSONTYTPE: {PersonType?.ModelName}");


        if (isEmpty)
        {
            PersonType = null;
        }
        if(!isEmpty && PersonType == null)
        {
            PersonType = Agency.GetRandomPed(World.TotalWantedLevel, "");
        }


       // EntryPoint.WriteToConsole($"DEBUG LE DISPATCH vehicleType: {VehicleType?.ModelName}");
       // EntryPoint.WriteToConsole($"DEBUG LE DISPATCH PERSONTYTPE: {PersonType?.ModelName}");


       // EntryPoint.WriteToConsole($"personType.ModelName: {personType.ModelName}");

        if (vehicleType != null)
        {
            VehicleType = vehicleType;
        }
        if (personType != null && !string.IsNullOrEmpty(personType.ModelName) && personType.ModelName != "") 
        {
            //EntryPoint.WriteToConsole($"I WENT IN THIS STUPID FUCKING IF STATEMENT CUZ IM A FUCKING ASSHGO<E");
            PersonType = personType;       
        }

       //EntryPoint.WriteToConsole($"DEBUG LE DISPATCH vehicleType: {VehicleType?.ModelName}");
       // EntryPoint.WriteToConsole($"DEBUG LE DISPATCH PERSONTYTPE: {PersonType?.ModelName}");

        CallSpawnTask(true, true, true, true, TaskRequirements.None, forcek9, IsOffDutySpawn, true);
    }
    public void OnHelicopterSpawnedOrRecalled()
    {
        GameTimeLastSpawnedOrRecalledHeli = Game.GameTime;
        EntryPoint.WriteToConsole($"OnHelicopterSpawnedOrRecalled {GameTimeLastSpawnedOrRecalledHeli}");
    }
    public void OnK9SpawnedOrRecalled()
    {
        GameTimeLastSpawnedOrRecalledCanineUnit = Game.GameTime;
        EntryPoint.WriteToConsole($"OnK9SpawnedOrRecalled {GameTimeLastSpawnedOrRecalledCanineUnit}");
    }
}