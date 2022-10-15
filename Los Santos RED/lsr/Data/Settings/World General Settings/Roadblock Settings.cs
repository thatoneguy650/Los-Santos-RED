using System.ComponentModel;

public class RoadblockSettings : ISettingsDefaultable
{
    [Description("Enable or diable the dynamic roadblock system.")]
    public bool RoadblockEnabled { get; set; }
    [Description("Enable or disable the spike strips for the dynamic roadblocks.")]
    public bool RoadblockSpikeStripsEnabled { get; set; }
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



    public bool RemoveGeneratedVehiclesAroundRoadblock { get; set; }

    public float RemoveGeneratedVehiclesAroundRoadblockDistance { get; set; }


    public bool DisableVehicleGenerationAroundRoadblock { get; set; }
    public float DisableVehicleGenerationAroundRoadblockDistance { get; set; }

    public RoadblockSettings()
    {
        SetDefault();


    }
    public void SetDefault()
    {

        RoadblockEnabled = true;
        RoadblockSpikeStripsEnabled = true;



        RoadblockMinWantedLevel = 3;
        RoadblockMaxWantedLevel = 5;


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
    }
}