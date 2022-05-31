using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum eCustomDrivingStyles
{
    RegularDriving = (int)VehicleDrivingFlags.FollowTraffic | (int)VehicleDrivingFlags.YieldToCrossingPedestrians | (int)VehicleDrivingFlags.RespectIntersections | 8,

    SlowEmergency = (int)VehicleDrivingFlags.DriveAroundVehicles | (int)VehicleDrivingFlags.DriveAroundPeds | 8 | (int)VehicleDrivingFlags.DriveAroundObjects          | (int)VehicleDrivingFlags.AllowWrongWay,  //20220531 allowwrongway and median crossing is new


    FakeEmergency = (int)eCustomDrivingStyles.FastEmergency,
    FastEmergency = (int)VehicleDrivingFlags.DriveAroundVehicles | 8 | (int)VehicleDrivingFlags.DriveAroundPeds | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing,
    FastEmergencyClose = (int)VehicleDrivingFlags.DriveAroundVehicles | 8 | (int)VehicleDrivingFlags.DriveAroundPeds | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing,
    CrazyEmergency = 8 | (int)VehicleDrivingFlags.DriveAroundPeds | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing                 | (int)VehicleDrivingFlags.DriveAroundVehicles, //20220531 drivearoundvehicles is now



    CrazyEmergencyClose = 8  | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing,
    //8 = avoid empty vehicles


    //i removed the drive by sight ones in here
}
