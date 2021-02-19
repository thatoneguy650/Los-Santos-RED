using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TrafficSettings
{
    public bool Enabled = true;
    public bool ExemptCode3 = true;
    public bool Speeding = true;
    public float SpeedingOverLimitThreshold = 25f;
    public bool HitPed = true;
    public bool HitVehicle = true;
    public bool DrivingAgainstTraffic = true;
    public bool DrivingOnPavement = true;
    public bool NotRoadworthy = true;
    public bool RunningRedLight = true;

    public TrafficSettings()
    {

    }
}