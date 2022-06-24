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
    public bool PrintUpdateTimes { get; set; }


    public int CivilianUpdateBatch { get; set; }
    public int PoliceUpdateBatch { get; set; }
    public int GangUpdateBatch { get; set; }
    public int EMTsUpdateBatch { get; set; }
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

    public DebugSettings()
    {

        SetDefault();
    }
    public void SetDefault()
    {
        ShowPoliceTaskArrows = false;
        ShowCivilianTaskArrows = false;
        ShowCivilianPerceptionArrows = false;
        PrintUpdateTimes = false;


        CivilianUpdateBatch = 8;
        PoliceUpdateBatch = 5;
        GangUpdateBatch = 5;
        MerchantsUpdateBatch = 5;
        EMTsUpdateBatch = 5;
        TaskAssignmentCheckFrequency = 500;


        CopUpdateIntervalClose = 250;
        CopUpdateIntervalMedium = 500;
        CopUpdateIntervalFar = 2000;
        CopUpdateIntervalVeryFar = 3000;


        OtherUpdateIntervalWanted = 500;
        OtherUpdateIntervalClose = 500;
        OtherUpdateIntervalMedium = 750;
        OtherUpdateIntervalFar = 2000;
        OtherUpdateIntervalVeryFar = 300;


        //Old defaults
        //CivilianUpdateBatch = 8;
        //PoliceUpdateBatch = 4;
        //GangUpdateBatch = 5;
        //MerchantsUpdateBatch = 5;
        //EMTsUpdateBatch = 5;
        //TaskAssignmentCheckFrequency = 500;


        //CopUpdateIntervalClose = 500;
        //CopUpdateIntervalMedium = 750;
        //CopUpdateIntervalFar = 2000;
        //CopUpdateIntervalVeryFar = 3000;


        //OtherUpdateIntervalWanted = 500;
        //OtherUpdateIntervalClose = 750;
        //OtherUpdateIntervalMedium = 1000;
        //OtherUpdateIntervalFar = 3000;
        //OtherUpdateIntervalVeryFar = 4000;
    }

}