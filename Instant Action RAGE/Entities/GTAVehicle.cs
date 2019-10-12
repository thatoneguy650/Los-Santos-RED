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
    
    public uint GameTimeEntered;
    public bool WillBeReportedStolen = false;
    public bool WasReportedStolen = false;
    public bool CopWillRecognize = false;
    public bool WasAlarmed = false;
    public bool WasJacked = false;
    public uint GameTimeToReportStolen;
    public uint ReportInterval = 30000;
    public Ped PreviousOwner = null;
    public bool PreviousOwnerDied = false;
    public bool IsPlayersVehicle = false;
    public bool IsStolen = false;
    public bool ShouldReportStolen
    {
        get
        {
            if (WasReportedStolen)
                return false;
            else if (WillBeReportedStolen && Game.GameTime >= GameTimeToReportStolen && Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Handle == VehicleEnt.Handle)
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
    public GTAVehicle(Vehicle _Vehicle,uint _GameTimeEntered,bool _WasJacked, bool _WasAlarmed, Ped _PrevIousOwner, bool _IsPlayersVehicle, bool _IsStolen)
    {
        VehicleEnt = _Vehicle;
        GameTimeEntered = _GameTimeEntered;
        WasJacked = _WasJacked;
        WasAlarmed = _WasAlarmed;
        PreviousOwner = _PrevIousOwner;
        IsStolen = _IsStolen;
        IsPlayersVehicle = _IsPlayersVehicle;

        if (IsPlayersVehicle)
            IsStolen = false;

        if (IsStolen)
            WillBeReportedStolen = true;

        if (IsStolen && WillBeReportedStolen && PreviousOwner != null && PreviousOwner.Handle != Game.LocalPlayer.Character.Handle)
            WatchForDeath(PreviousOwner);

        if (WasJacked)
            GameTimeToReportStolen = GameTimeEntered + 15000;
        else if(WasAlarmed)
            GameTimeToReportStolen = GameTimeEntered + 30000;
        else
            GameTimeToReportStolen = GameTimeEntered + 600000;


        InstantAction.WriteToLog("GTAVehicleS", string.Format("Vehicle Created: Handle {0},GameTimeEntered,{1},GameTimeToReportStolen {2},WasJacked {3},WasAlarmed {4},IsStolen {5},WillBeReportedStolen {6},WillWatchLastOwner {7}", VehicleEnt.Handle, GameTimeEntered, GameTimeToReportStolen, WasJacked,WasAlarmed, IsStolen, WillBeReportedStolen, PreviousOwner != null));
    }

}

