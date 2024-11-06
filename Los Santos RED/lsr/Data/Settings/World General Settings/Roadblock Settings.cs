using System.ComponentModel;
using System.Runtime.Serialization;

public class RoadblockSettings : ISettingsDefaultable
{
    [Description("Enable or diable the dynamic roadblock system.")]
    public bool RoadblockEnabled { get; set; }
    [Description("Distance in meters in front of the player that the reoadblock will spawn.")]
    public float RoadblockSpawnDistance { get; set; }
    [Description("Enable or disable the cars across the road for the dynamic roadblocks.")]
    public bool RoadblockCarBlocksEnabled { get; set; }
    [Description("Enable or disable the spike strips for the dynamic roadblocks.")]
    public bool RoadblockSpikeStripsEnabled { get; set; }
    [Description("Enable or disable other barriers (cones, barriers, etc. for the dynamic roadblocks.")]
    public bool RoadblockOtherBarriersEnabled { get; set; }
    [Description("Minimum wanted level before dynamic roadblocks can spawn.")]
    public int RoadblockMinWantedLevel { get; set; }
    [Description("Maximum level that dynamic roadblocks can spawn at.")]
    public int RoadblockMaxWantedLevel { get; set; }
    [Description("Distance from center the peds will spawn behind.")]
    public float Roadblock_PedDistance { get; set; }
    [Description("Distance from center the barriers.")]
    public float Roadblock_BarrierDistance { get; set; }
    [Description("Distance from center the cones will spawn at.")]
    public float Roadblock_ConeDistance { get; set; }
    [Description("Time (in ms) between roadblocks when you are not actively seen by police.")]
    public int TimeBetweenRoadblock_Unseen { get; set; }
    [Description("Minimum time (in ms) between roadblocks.")]
    public int TimeBetweenRoadblock_Seen_Min { get; set; }
    [Description("Decreased time (in ms) scalar between roadblocks as wanted level increases. Formula: ((6 - WantedLevel) * TimeBetweenRoadblock_Seen_AdditionalTimeScaler) + TimeBetweenRoadblock_Seen_Min")]
    public int TimeBetweenRoadblock_Seen_AdditionalTimeScaler { get; set; }
    [Description("Enable or disable removing vehicles around the roadblocks.")]
    public bool RemoveGeneratedVehiclesAroundRoadblock { get; set; }
    [Description("Radius distance (in meters) to remove spawned vehicles around the vehicle and strip centers.")]
    public float RemoveGeneratedVehiclesAroundRoadblockDistance { get; set; }
    [Description("Enable or disable disabling vehicle generation around the roadblocks.")]
    public bool DisableVehicleGenerationAroundRoadblock { get; set; }
    [Description("Radius distance (in meters) to disabling vehicle generation around the vehicle and strip centers.")]
    public float DisableVehicleGenerationAroundRoadblockDistance { get; set; }

    [Description("Likelyhood a roadblock will spawn each interval for wanted level 1.")]
    public int RoadblockSpawnPercentage_Wanted1 { get ; set; }
    [Description("Likelyhood a roadblock will spawn each interval for wanted level 2.")]
    public int RoadblockSpawnPercentage_Wanted2 { get; set; }
    [Description("Likelyhood a roadblock will spawn each interval for wanted level 3.")]
    public int RoadblockSpawnPercentage_Wanted3 { get; set; }
    [Description("Likelyhood a roadblock will spawn each interval for wanted level 4.")]
    public int RoadblockSpawnPercentage_Wanted4 { get; set; }
    [Description("Likelyhood a roadblock will spawn each interval for wanted level 5.")]
    public int RoadblockSpawnPercentage_Wanted5 { get; set; }
    [Description("Likelyhood a roadblock will spawn each interval for wanted level 6.")]
    public int RoadblockSpawnPercentage_Wanted6 { get; set; }



    [Description("Likelyhood a roadblock will have a spike strip for wanted level 1.")]
    public int RoadblockSpikeStripSpawnPercentage_Wanted1 { get; set; }
    [Description("Likelyhood a roadblock will have a spike strip for wanted level 2.")]
    public int RoadblockSpikeStripSpawnPercentage_Wanted2 { get; set; }
    [Description("Likelyhood a roadblock will have a spike strip for wanted level 3.")]
    public int RoadblockSpikeStripSpawnPercentage_Wanted3 { get; set; }
    [Description("Likelyhood a roadblock will have a spike strip for wanted level 4.")]
    public int RoadblockSpikeStripSpawnPercentage_Wanted4 { get; set; }
    [Description("Likelyhood a roadblock will have a spike strip for wanted level 5.")]
    public int RoadblockSpikeStripSpawnPercentage_Wanted5 { get; set; }
    [Description("Likelyhood a roadblock will have a spike strip for wanted level 6.")]
    public int RoadblockSpikeStripSpawnPercentage_Wanted6 { get; set; }



    [Description("Likelyhood a roadblock will have cars blocking the road for wanted level 1.")]
    public int RoadblockCarBlockSpawnPercentage_Wanted1 { get; set; }
    [Description("Likelyhood a roadblock will have cars blocking the road for wanted level 2.")]
    public int RoadblockCarBlockSpawnPercentage_Wanted2 { get; set; }
    [Description("Likelyhood a roadblock will have cars blocking the road for wanted level 3.")]
    public int RoadblockCarBlockSpawnPercentage_Wanted3 { get; set; }
    [Description("Likelyhood a roadblock will have cars blocking the road for wanted level 4.")]
    public int RoadblockCarBlockSpawnPercentage_Wanted4 { get; set; }
    [Description("Likelyhood a roadblock will have cars blocking the road for wanted level 5.")]
    public int RoadblockCarBlockSpawnPercentage_Wanted5 { get; set; }
    [Description("Likelyhood a roadblock will have cars blocking the road for wanted level 6.")]
    public int RoadblockCarBlockSpawnPercentage_Wanted6 { get; set; }


    [Description("Likelyhood a roadblock will have other barrier props spawn for wanted level 1.")]
    public int RoadblockOtherBarrierSpawnPercentage_Wanted1 { get; set; }
    [Description("Likelyhood a roadblock will have other barrier props spawn for wanted level 2.")]
    public int RoadblockOtherBarrierSpawnPercentage_Wanted2 { get; set; }
    [Description("Likelyhood a roadblock will have other barrier props spawn for wanted level 3.")]
    public int RoadblockOtherBarrierSpawnPercentage_Wanted3 { get; set; }
    [Description("Likelyhood a roadblock will have other barrier props spawn for wanted level 4.")]
    public int RoadblockOtherBarrierSpawnPercentage_Wanted4 { get; set; }
    [Description("Likelyhood a roadblock will have other barrier props spawn for wanted level 5.")]
    public int RoadblockOtherBarrierSpawnPercentage_Wanted5 { get; set; }
    [Description("Likelyhood a roadblock will have other barrier props spawn for wanted level 6.")]
    public int RoadblockOtherBarrierSpawnPercentage_Wanted6 { get; set; }
    public bool AllowRoadblockOnNonCurrentStreet { get; set; }

    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        SetDefault();
    }

    public RoadblockSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {

        RoadblockEnabled = true;
        RoadblockCarBlocksEnabled = true;
        RoadblockSpikeStripsEnabled = true;
        RoadblockOtherBarriersEnabled = true;

        RoadblockSpawnDistance = 225f;
        RoadblockMinWantedLevel = 2;
        RoadblockMaxWantedLevel = 10;

        Roadblock_PedDistance = 5f;// 15f;
        Roadblock_BarrierDistance = 10f;// 17f;
        Roadblock_ConeDistance = 12f;// 19f;

        TimeBetweenRoadblock_Unseen = 999999;
        TimeBetweenRoadblock_Seen_Min = 90000;
        TimeBetweenRoadblock_Seen_AdditionalTimeScaler = 25000;

        RemoveGeneratedVehiclesAroundRoadblock = true;
        RemoveGeneratedVehiclesAroundRoadblockDistance = 15f;

        DisableVehicleGenerationAroundRoadblock = true;
        DisableVehicleGenerationAroundRoadblockDistance = 15f;


        RoadblockSpawnPercentage_Wanted1 = 0;
        RoadblockSpawnPercentage_Wanted2 = 5;
        RoadblockSpawnPercentage_Wanted3 = 65;
        RoadblockSpawnPercentage_Wanted4 = 95;
        RoadblockSpawnPercentage_Wanted5 = 100;
        RoadblockSpawnPercentage_Wanted6 = 0;



        RoadblockSpikeStripSpawnPercentage_Wanted1 = 0;
        RoadblockSpikeStripSpawnPercentage_Wanted2 = 0;
        RoadblockSpikeStripSpawnPercentage_Wanted3 = 85;
        RoadblockSpikeStripSpawnPercentage_Wanted4 = 95;
        RoadblockSpikeStripSpawnPercentage_Wanted5 = 95;
        RoadblockSpikeStripSpawnPercentage_Wanted6 = 0;



        RoadblockCarBlockSpawnPercentage_Wanted1 = 0;
        RoadblockCarBlockSpawnPercentage_Wanted2 = 100;
        RoadblockCarBlockSpawnPercentage_Wanted3 = 65;
        RoadblockCarBlockSpawnPercentage_Wanted4 = 100;
        RoadblockCarBlockSpawnPercentage_Wanted5 = 100;
        RoadblockCarBlockSpawnPercentage_Wanted6 = 100;

        RoadblockOtherBarrierSpawnPercentage_Wanted1 = 0;
        RoadblockOtherBarrierSpawnPercentage_Wanted2 = 0;
        RoadblockOtherBarrierSpawnPercentage_Wanted3 = 95;
        RoadblockOtherBarrierSpawnPercentage_Wanted4 = 75;
        RoadblockOtherBarrierSpawnPercentage_Wanted5 = 25;
        RoadblockOtherBarrierSpawnPercentage_Wanted6 = 0;

        AllowRoadblockOnNonCurrentStreet = false;
    }
}