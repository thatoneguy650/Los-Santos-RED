using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PerformanceSettings : ISettingsDefaultable
{
    public bool PrintUpdateTimes { get; set; }
    public bool PrintCivilianOnlyUpdateTimes { get; set; }
    public bool PrintCivilianUpdateTimes { get; set; }
    public bool YieldAfterEveryPedExtUpdate { get; set; }
    public bool CivilianUpdatePerformanceMode1 { get; set; }//Civilians & Merchants
    public bool CivilianUpdatePerformanceMode2 { get; set; }
    public bool IsCivilianYield1Active { get; set; }
    public bool IsCivilianYield2Active { get; set; }
    public bool IsCivilianYield3Active { get; set; }
    public bool IsCivilianYield4Active { get; set; }
    public bool CopUpdatePerformanceMode1 { get; set; }
    public bool CopUpdatePerformanceMode2 { get; set; }
    public bool IsCopYield1Active { get; set; }
    public bool IsCopYield2Active { get; set; }
    public bool IsCopYield3Active { get; set; }




    public bool IsCopYield10Active { get; set; }
    public bool IsCopYield11Active { get; set; }
    public bool IsCopYield12Active { get; set; }
    public bool IsCopYield13Active { get; set; }

    public bool GangMemberUpdatePerformanceMode1 { get; set; }
    public bool GangMemberUpdatePerformanceMode2 { get; set; }
    public bool IsGangMemberYield1Active { get; set; }
    public bool IsGangMemberYield2Active { get; set; }
    public bool IsGangMemberYield3Active { get; set; }
    public bool IsGangMemberYield4Active { get; set; }
    public bool EMSUpdatePerformanceMode1 { get; set; }
    public bool EMSUpdatePerformanceMode2 { get; set; }
    public bool IsEMSYield2Active { get; set; }
    public bool IsEMSYield1Active { get; set; }
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
  //  public bool CopGetPedToAttackYield1 { get; set; }
    public bool CopDisableFootChaseFiber { get; set; }
   // public bool FootChaseFiberSetSleep { get; set; }
   // public int FootChaseFiberSleepTime { get; set; }

    public PerformanceSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {

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

        CivilianUpdatePerformanceMode1 = true;
        CivilianUpdatePerformanceMode2 = false;
        IsCivilianYield1Active = false;
        IsCivilianYield2Active = false;
        IsCivilianYield3Active = false;
        IsCivilianYield4Active = false;


        GangMemberUpdatePerformanceMode1 = true;
        GangMemberUpdatePerformanceMode2 = false;
        IsGangMemberYield1Active = false;
        IsGangMemberYield2Active = false;
        IsGangMemberYield3Active = false;
        IsGangMemberYield4Active = false;


        CopUpdatePerformanceMode1 = true;
        CopUpdatePerformanceMode2 = false;
        IsCopYield1Active = false;
        IsCopYield2Active = false;
        IsCopYield3Active = false;

        IsCopYield10Active = false;
        IsCopYield11Active = false;
        IsCopYield12Active = false;
        IsCopYield13Active = false;


        EMSUpdatePerformanceMode1 = true;
        EMSUpdatePerformanceMode2 = false;
        IsEMSYield2Active = false;
        IsEMSYield1Active = false;


        CopGetPedToAttackDisable = false;
       // CopGetPedToAttackYield1 = false;
        CopDisableFootChaseFiber = false;

     //   FootChaseFiberSetSleep = false;
      //  FootChaseFiberSleepTime = 500;

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