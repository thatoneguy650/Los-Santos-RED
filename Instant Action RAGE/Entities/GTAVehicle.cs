using Rage;
using Rage.Native;
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
    public bool QuedeReportedStolen = false;
    public bool ShouldReportStolen
    {
        get
        {
            if (WasReportedStolen || QuedeReportedStolen)
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
            uint GameTimeStolen = Game.GameTime;
            while(Pedestrian.Exists())
            {
                Pedestrian.IsPersistent = true;
                WillBeReportedStolen = false;

                if (Pedestrian.IsDead)
                {
                    WillBeReportedStolen = false;
                    PreviousOwnerDied = true;
                    InstantAction.WriteToLog("StolenVehicles", string.Format("PreviousOwnerDied {0},WillBeReportedStolen {1}", PreviousOwnerDied, WillBeReportedStolen));
                    break;
                }
                else if(Game.GameTime - GameTimeStolen > 20000 && !Pedestrian.IsRagdoll)
                {
                    NativeFunction.CallByName<bool>("TASK_USE_MOBILE_PHONE_TIMED", Pedestrian, 10000);
                    Pedestrian.PlayAmbientSpeech("JACKED_GENERIC");
                    GameFiber.Sleep(5000);
                    if (Pedestrian.Exists() && !Pedestrian.IsDead && !Pedestrian.IsRagdoll)
                    {
                        WillBeReportedStolen = true;
                        Pedestrian.IsPersistent = false;
                    }
                    InstantAction.WriteToLog("StolenVehicles", string.Format("WillBeReportedStolen {0}", WillBeReportedStolen));
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
        {
            InstantAction.WriteToLog("StolenVehicles","Previous Owner is alive, will watch for death");
            WatchForDeath(PreviousOwner);
        }

        if (WasJacked)
            GameTimeToReportStolen = GameTimeEntered + 15000;
        else if(WasAlarmed)
            GameTimeToReportStolen = GameTimeEntered + 30000;
        else
            GameTimeToReportStolen = GameTimeEntered + 600000;


        InstantAction.WriteToLog("GTAVehicle", string.Format("Vehicle Created: Handle {0},GTEntered,{1},GTReportStolen {2},WasJacked {3},WasAlarmed {4},IsStolen {5},WillBeRptdStoln {6},WatchLastOwner {7}", VehicleEnt.Handle, GameTimeEntered, GameTimeToReportStolen, WasJacked,WasAlarmed, IsStolen, WillBeReportedStolen, PreviousOwner != null));
    }

}

