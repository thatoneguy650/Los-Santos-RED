using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GTAVehicle
{
    private static Random rnd;

    public Vehicle VehicleEnt = null;
    public uint GameTimeStolen;
    public bool WillBeReportedStolen = false;
    public bool WasReportedStolen = false;
    public bool CopWillRecognize = false;
    public bool WasAlarmed = false;
    public bool WasJacked = false;
    public uint GameTimeToReportStolen;
    public uint ReportInterval = 30000;
    public Ped PreviousOwner = null;
    public bool PreviousOwnerDied = false;
    public bool ShouldReportStolen
    {
        get
        {
            if (WasReportedStolen)
                return false;
            else if (WillBeReportedStolen && Game.GameTime - GameTimeStolen >= ReportInterval && Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Handle == VehicleEnt.Handle)
                return true;
            else
                return false;
        }
    }
    static GTAVehicle()
    {
        rnd = new Random();
    }
    public void WatchForDeath(Ped Pedestrian)
    {
        GameFiber.StartNew(delegate
        {
            while(Pedestrian.Exists())
            {
                if(Pedestrian.IsDead)
                {
                    WillBeReportedStolen = false;
                    PreviousOwnerDied = true;
                    InstantAction.WriteToLog("StolenVehicles", string.Format("PreviousOwnerDied {0},WillBeReportedStolen {1}", PreviousOwnerDied, WillBeReportedStolen));
                    break;
                }
                GameFiber.Yield();
            }
        });
    }
    public GTAVehicle(Vehicle _Vehicle,uint _GameTimeStolen,bool _WasJacked, bool _WasAlarmed, Ped _PrevIousOwner)
    {
        VehicleEnt = _Vehicle;
        GameTimeStolen = _GameTimeStolen;
        WasJacked = _WasJacked;
        WasAlarmed = _WasAlarmed;
        PreviousOwner = _PrevIousOwner;


        if(!WasJacked && !WasAlarmed && PreviousOwner == null)
        {
            WillBeReportedStolen = false;
        }
        else
        {
            WillBeReportedStolen = true;
        }

        if (PreviousOwner != null)
        {
            if (PreviousOwner.IsDead)
            {
                WillBeReportedStolen = false;
            }
            WatchForDeath(PreviousOwner);
        }


        if (WasJacked)
            ReportInterval = 15000;
        else if (WasAlarmed)
            ReportInterval = 30000;
        else
            ReportInterval = 600000;

        

        InstantAction.WriteToLog("GTAVehicleS", string.Format("Stolen Vehicle Created: Handle {0},GameTimeStolen {1},WasJacked {2},WasAlarmed {3},ReportInterval {4},WillBeReportedStolen {5},WillWatchLastOwner {6}", VehicleEnt.Handle,GameTimeStolen,WasJacked,WasAlarmed,ReportInterval,WillBeReportedStolen, PreviousOwner != null));
    }

}

