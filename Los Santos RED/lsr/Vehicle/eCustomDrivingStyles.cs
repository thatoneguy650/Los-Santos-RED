using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum eCustomDrivingStyles
{
    RegularDriving = (int)VehicleDrivingFlags.FollowTraffic | (int)VehicleDrivingFlags.YieldToCrossingPedestrians | (int)VehicleDrivingFlags.RespectIntersections | 8,
    FakeEmergency = (int)eCustomDrivingStyles.FastEmergency,
    FastEmergency = (int)VehicleDrivingFlags.DriveAroundVehicles | 8 | (int)VehicleDrivingFlags.DriveAroundPeds | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing,
    FastEmergencyClose = (int)VehicleDrivingFlags.DriveAroundVehicles | 8 | (int)VehicleDrivingFlags.DriveAroundPeds | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing | (int)VehicleDrivingFlags.DriveBySight,
    CrazyEmergency = 8 | (int)VehicleDrivingFlags.DriveAroundPeds | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing,
    CrazyEmergencyClose = 8 | (int)VehicleDrivingFlags.DriveAroundPeds | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing | (int)VehicleDrivingFlags.DriveBySight,
    //8 = avoid empty vehicles
}
