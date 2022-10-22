using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum eCustomDrivingStyles
{
    RegularDriving = (int)VehicleDrivingFlags.FollowTraffic | (int)VehicleDrivingFlags.YieldToCrossingPedestrians  | 8 | (int)VehicleDrivingFlags.RespectIntersections | 256,

    RegularDrivingClose = (int)VehicleDrivingFlags.FollowTraffic | (int)VehicleDrivingFlags.YieldToCrossingPedestrians | 8 | (int)VehicleDrivingFlags.RespectIntersections | 256 | (int)VehicleDrivingFlags.DriveBySight,

    Code2 = (int)VehicleDrivingFlags.DriveAroundVehicles | (int)VehicleDrivingFlags.DriveAroundPeds | 8 | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing,  //20220531 allowwrongway and median crossing is new

    Code3 = (int)VehicleDrivingFlags.DriveAroundVehicles | (int)VehicleDrivingFlags.DriveAroundPeds | 8  | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing,

    Code3Close = (int)VehicleDrivingFlags.DriveAroundVehicles | (int)VehicleDrivingFlags.DriveAroundPeds | 8 | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing | (int)VehicleDrivingFlags.DriveBySight,


    Panic = (int)VehicleDrivingFlags.DriveAroundVehicles | (int)VehicleDrivingFlags.DriveAroundPeds | 8 | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay,
    //8 = avoid empty vehicles
    //256 use blinkers
    //AllowMedianCrossing is mostly called take shortest path

}
