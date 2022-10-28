using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DebugSettings : ISettingsDefaultable
{
    public bool ShowPoliceTaskArrows { get; set; }
    public bool ShowCivilianTaskArrows { get; set; }
    public bool ShowCivilianPerceptionArrows { get; set; }
    public bool ShowTrafficArrows { get; set; }
    public bool PrintUpdateTimes { get; set; }


    public int CivilianUpdateBatch { get; set; }
    public int GangUpdateBatch { get; set; }
    public int EMTsUpdateBatch { get; set; }
    public int MerchantsUpdateBatch { get; set; }

    public bool YieldAfterEveryPedExtUpdate { get; set; }

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


    public DebugSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        ShowPoliceTaskArrows = false;
        ShowCivilianTaskArrows = false;
        ShowCivilianPerceptionArrows = false;
        ShowTrafficArrows = false;
        PrintUpdateTimes = false;

        CivilianUpdateBatch = 1;// 10;
        GangUpdateBatch = 1;//10;
        MerchantsUpdateBatch = 1;//10;
        EMTsUpdateBatch = 1;//10;
        TaskAssignmentCheckFrequency = 500;


        CopUpdateIntervalClose = 250;
        CopUpdateIntervalMedium = 250;
        CopUpdateIntervalFar = 750;
        CopUpdateIntervalVeryFar = 2000;


        OtherUpdateIntervalWanted = 250;
        OtherUpdateIntervalClose = 250;
        OtherUpdateIntervalMedium = 750;
        OtherUpdateIntervalFar = 2000;
        OtherUpdateIntervalVeryFar = 300;

        YieldAfterEveryPedExtUpdate = true;



        /*
         *         CivilianUpdateBatch = 10;
        PoliceUpdateBatch = 10;
        GangUpdateBatch = 10;
        MerchantsUpdateBatch = 10;
        EMTsUpdateBatch = 10;
        TaskAssignmentCheckFrequency = 500;


        CopUpdateIntervalClose = 250;
        CopUpdateIntervalMedium = 500;
        CopUpdateIntervalFar = 2000;
        CopUpdateIntervalVeryFar = 3000;


        OtherUpdateIntervalWanted = 500;
        OtherUpdateIntervalClose = 500;
        OtherUpdateIntervalMedium = 750;
        OtherUpdateIntervalFar = 2000;
        OtherUpdateIntervalVeryFar = 300;*/

    }

}