using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class StolenVehicle
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
    static StolenVehicle()
    {
        rnd = new Random();
    }
    public StolenVehicle(Vehicle _Vehicle,uint _GameTimeStolen,bool _WasJacked, bool _WasAlarmed)
    {
        VehicleEnt = _Vehicle;
        GameTimeStolen = _GameTimeStolen;
        WasJacked = _WasJacked;
        WasAlarmed = _WasAlarmed;
        WillBeReportedStolen = true;

        if (WasJacked)
            ReportInterval = 15000;
        else if (WasAlarmed)
            ReportInterval = 30000;
        else
            ReportInterval = 600000;

        InstantAction.WriteToLog("StolenVehicles", string.Format("Stolen Vehicle Created: Handle {0},GameTimeStolen {1},WasJacked {2},WasAlarmed {3},ReportInterval {4}", VehicleEnt.Handle,GameTimeStolen,WasJacked,WasAlarmed,ReportInterval));
    }

}

