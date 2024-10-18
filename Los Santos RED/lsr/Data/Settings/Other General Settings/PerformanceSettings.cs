using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

public class PerformanceSettings : ISettingsDefaultable
{
    [Description("If enabled, lsr will update more items each frame. This should increase respinsiveness but can decrease performance.")]
    public bool EnableIncreasedUpdateMode { get; set; }
    public bool PrintUpdateTimes { get; set; }
    public bool PrintCivilianOnlyUpdateTimes { get; set; }
    public bool PrintCivilianUpdateTimes { get; set; }
    public bool YieldAfterEveryPedExtUpdate { get; set; }
    public int CivilianUpdateBatch { get; set; }
    public int GangUpdateBatch { get; set; }
    public int EMTsUpdateBatch { get; set; }
    public int SecurityGuardsUpdateBatch { get; set; }
    public int MerchantsUpdateBatch { get; set; }
    public uint TaskAssignmentCheckFrequency { get; set; }
    public int CopUpdateIntervalClose { get; set; }
    public int CopUpdateIntervalMedium { get; set; }
    public int CopUpdateIntervalFar { get; set; }
    public int CopUpdateIntervalVeryFar { get; set; }
    public int OtherUpdateIntervalWanted { get; set; }
    public int OtherUpdateIntervalClose { get; set; }
    public int OtherUpdateIntervalMedium { get; set; }
    public int OtherUpdateIntervalFar { get; set; }
    public int OtherUpdateIntervalVeryFar { get; set; }
    public bool CopGetPedToAttackDisable { get; set; }
    public bool CopDisableFootChaseFiber { get; set; }
    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        SetDefault();
    }
    public PerformanceSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        EnableIncreasedUpdateMode = false;

        PrintUpdateTimes = false;
        PrintCivilianUpdateTimes = false;
        PrintCivilianOnlyUpdateTimes = false;

        CivilianUpdateBatch = 1;// 10;
        GangUpdateBatch = 1;//10;
        MerchantsUpdateBatch = 1;//10;
        EMTsUpdateBatch = 1;//10;
        SecurityGuardsUpdateBatch = 1;
        TaskAssignmentCheckFrequency = 500;


        CopUpdateIntervalClose = 250;
        CopUpdateIntervalMedium = 250;
        CopUpdateIntervalFar = 750;
        CopUpdateIntervalVeryFar = 2000;

        OtherUpdateIntervalWanted = 500;
        OtherUpdateIntervalClose = 500;//250;
        OtherUpdateIntervalMedium = 500;//750;
        OtherUpdateIntervalFar = 1000;
        OtherUpdateIntervalVeryFar = 2000;

        YieldAfterEveryPedExtUpdate = false;

        CopGetPedToAttackDisable = false;
        CopDisableFootChaseFiber = false;

    }
}