using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public enum eCustomDrivingStyles : int
{
	//ORIGINAL
	/*	
	 *	RegularDriving = (int)VehicleDrivingFlags.FollowTraffic | (int)VehicleDrivingFlags.YieldToCrossingPedestrians | 8 | (int)VehicleDrivingFlags.RespectIntersections | 256,
	RegularDrivingClose = (int)VehicleDrivingFlags.FollowTraffic | (int)VehicleDrivingFlags.YieldToCrossingPedestrians | 8 | (int)VehicleDrivingFlags.RespectIntersections | 256 | (int)VehicleDrivingFlags.DriveBySight,
	Code2 = (int)VehicleDrivingFlags.DriveAroundVehicles | (int)VehicleDrivingFlags.DriveAroundPeds | 8 | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing | 524288,  //20220531 allowwrongway and median crossing is new
	Code3 = (int)VehicleDrivingFlags.DriveAroundVehicles | (int)VehicleDrivingFlags.DriveAroundPeds | 8 | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing | 524288,
	Code3Close = (int)VehicleDrivingFlags.DriveAroundVehicles | (int)VehicleDrivingFlags.DriveAroundPeds | 8 | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing | (int)VehicleDrivingFlags.DriveBySight | 524288,
	Panic = (int)VehicleDrivingFlags.DriveAroundVehicles | (int)VehicleDrivingFlags.DriveAroundPeds | 8 | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing | 524288,
	
	Alerted = (int)786469,
	*/


	RegularDriving = 1 | 2 | 8 | 128 | 256 | 16384 | 524288,




	Taxi_StandardDrivingMode = VanillaDrivingStyles.DF_StopForCars | VanillaDrivingStyles.DF_StopForPeds | VanillaDrivingStyles.DF_SteerAroundObjects | VanillaDrivingStyles.DF_SteerAroundStationaryCars | VanillaDrivingStyles.DF_StopAtLights |
	VanillaDrivingStyles.DF_UseShortCutLinks | VanillaDrivingStyles.DF_ChangeLanesAroundObstructions | VanillaDrivingStyles.DF_AdjustCruiseSpeedBasedOnRoadSpeed,

	TaxiRushed_AvoidCarsObeyLights = VanillaDrivingStyles.DF_SwerveAroundAllCars | VanillaDrivingStyles.DF_StopAtLights | VanillaDrivingStyles.DF_SteerAroundObjects | VanillaDrivingStyles.DF_UseShortCutLinks | VanillaDrivingStyles.DF_ChangeLanesAroundObstructions | VanillaDrivingStyles.DF_StopForCars,


	TaxiCrazy_AvoidCars = VanillaDrivingStyles.DF_SwerveAroundAllCars | VanillaDrivingStyles.DF_SteerAroundObjects | VanillaDrivingStyles.DF_UseShortCutLinks | VanillaDrivingStyles.DF_ChangeLanesAroundObstructions | VanillaDrivingStyles.DF_StopForCars,


	VanillaStandardDrivingMode = VanillaDrivingStyles.DF_StopForCars | VanillaDrivingStyles.DF_StopForPeds | VanillaDrivingStyles.DF_SteerAroundObjects | VanillaDrivingStyles.DF_SteerAroundStationaryCars | VanillaDrivingStyles.DF_StopAtLights |
		VanillaDrivingStyles.DF_UseShortCutLinks | VanillaDrivingStyles.DF_ChangeLanesAroundObstructions,

	VanillaAvoidCarsObeyLights = VanillaDrivingStyles.DF_SwerveAroundAllCars | VanillaDrivingStyles.DF_StopAtLights | VanillaDrivingStyles.DF_SteerAroundObjects | VanillaDrivingStyles.DF_UseShortCutLinks | VanillaDrivingStyles.DF_ChangeLanesAroundObstructions | VanillaDrivingStyles.DF_StopForCars,



	//RegularDrivingClose = (int)VehicleDrivingFlags.FollowTraffic | (int)VehicleDrivingFlags.YieldToCrossingPedestrians | 8 | (int)VehicleDrivingFlags.RespectIntersections | 256 | (int)VehicleDrivingFlags.DriveBySight,
	Code2 = (int)VehicleDrivingFlags.DriveAroundVehicles | (int)VehicleDrivingFlags.DriveAroundPeds | 8 | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing | 524288 | (int)VanillaDrivingStyles.DF_StopForCars,  //20220531 allowwrongway and median crossing is new
	
	
	
	Code3 = (int)VehicleDrivingFlags.DriveAroundVehicles | (int)VehicleDrivingFlags.DriveAroundPeds | 8 | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing | 524288 | (int)VanillaDrivingStyles.DF_StopForCars,
    
	
	
	Code3Close = (int)VehicleDrivingFlags.DriveAroundVehicles | (int)VehicleDrivingFlags.DriveAroundPeds | 8 | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing | (int)VehicleDrivingFlags.DriveBySight | 524288 | (int)VanillaDrivingStyles.DF_StopForCars,



	 


    //Panic = (int)VehicleDrivingFlags.DriveAroundVehicles | (int)VehicleDrivingFlags.DriveAroundPeds | 8 | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing | 524288,

    Vanilla_Alerted = (int)786469,



    DrunkDriving = VanillaDrivingStyles.DF_ChangeLanesAroundObstructions | VanillaDrivingStyles.DF_UseWanderFallbackInsteadOfStraightLine | VanillaDrivingStyles.DF_DriveIntoOncomingTraffic | VanillaDrivingStyles.DF_SwerveAroundAllCars,
    RecklessDriving = VanillaDrivingStyles.DF_SwerveAroundAllCars | VanillaDrivingStyles.DF_SteerAroundObjects | VanillaDrivingStyles.DF_UseShortCutLinks | VanillaDrivingStyles.DF_ChangeLanesAroundObstructions,
    SpeedingDriving = VanillaDrivingStyles.DF_SwerveAroundAllCars | VanillaDrivingStyles.DF_StopAtLights | VanillaDrivingStyles.DF_SteerAroundObjects | VanillaDrivingStyles.DF_UseShortCutLinks | VanillaDrivingStyles.DF_ChangeLanesAroundObstructions,

}


//public enum eCustomDrivingStyles : int
//{
//	//ORIGINAL
//	/*	
//	 *	RegularDriving = (int)VehicleDrivingFlags.FollowTraffic | (int)VehicleDrivingFlags.YieldToCrossingPedestrians | 8 | (int)VehicleDrivingFlags.RespectIntersections | 256,
//	RegularDrivingClose = (int)VehicleDrivingFlags.FollowTraffic | (int)VehicleDrivingFlags.YieldToCrossingPedestrians | 8 | (int)VehicleDrivingFlags.RespectIntersections | 256 | (int)VehicleDrivingFlags.DriveBySight,
//	Code2 = (int)VehicleDrivingFlags.DriveAroundVehicles | (int)VehicleDrivingFlags.DriveAroundPeds | 8 | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing | 524288,  //20220531 allowwrongway and median crossing is new
//	Code3 = (int)VehicleDrivingFlags.DriveAroundVehicles | (int)VehicleDrivingFlags.DriveAroundPeds | 8 | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing | 524288,
//	Code3Close = (int)VehicleDrivingFlags.DriveAroundVehicles | (int)VehicleDrivingFlags.DriveAroundPeds | 8 | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing | (int)VehicleDrivingFlags.DriveBySight | 524288,
//	Panic = (int)VehicleDrivingFlags.DriveAroundVehicles | (int)VehicleDrivingFlags.DriveAroundPeds | 8 | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing | 524288,

//	Alerted = (int)786469,
//	*/


//	RegularDriving = 1 | 2 | 8 | 128 | 256 | 16384 | 524288,




//    Taxi_StandardDrivingMode = VanillaDrivingStyles.DF_StopForCars | VanillaDrivingStyles.DF_StopForPeds | VanillaDrivingStyles.DF_SteerAroundObjects | VanillaDrivingStyles.DF_SteerAroundStationaryCars | VanillaDrivingStyles.DF_StopAtLights |
//    VanillaDrivingStyles.DF_UseShortCutLinks | VanillaDrivingStyles.DF_ChangeLanesAroundObstructions | VanillaDrivingStyles.DF_AdjustCruiseSpeedBasedOnRoadSpeed,

//    TaxiRushed_AvoidCarsObeyLights = VanillaDrivingStyles.DF_SwerveAroundAllCars | VanillaDrivingStyles.DF_StopAtLights | VanillaDrivingStyles.DF_SteerAroundObjects | VanillaDrivingStyles.DF_UseShortCutLinks | VanillaDrivingStyles.DF_ChangeLanesAroundObstructions,


//    TaxiCrazy_AvoidCars = VanillaDrivingStyles.DF_SwerveAroundAllCars | VanillaDrivingStyles.DF_SteerAroundObjects | VanillaDrivingStyles.DF_UseShortCutLinks | VanillaDrivingStyles.DF_ChangeLanesAroundObstructions,


//    VanillaStandardDrivingMode = VanillaDrivingStyles.DF_StopForCars | VanillaDrivingStyles.DF_StopForPeds | VanillaDrivingStyles.DF_SteerAroundObjects | VanillaDrivingStyles.DF_SteerAroundStationaryCars | VanillaDrivingStyles.DF_StopAtLights | 
//		VanillaDrivingStyles.DF_UseShortCutLinks | VanillaDrivingStyles.DF_ChangeLanesAroundObstructions,

//	VanillaAvoidCarsObeyLights = VanillaDrivingStyles.DF_SwerveAroundAllCars | VanillaDrivingStyles.DF_StopAtLights | VanillaDrivingStyles.DF_SteerAroundObjects | VanillaDrivingStyles.DF_UseShortCutLinks | VanillaDrivingStyles.DF_ChangeLanesAroundObstructions | VanillaDrivingStyles.DF_StopForCars,



//    //RegularDrivingClose = (int)VehicleDrivingFlags.FollowTraffic | (int)VehicleDrivingFlags.YieldToCrossingPedestrians | 8 | (int)VehicleDrivingFlags.RespectIntersections | 256 | (int)VehicleDrivingFlags.DriveBySight,
//	Code2 = (int)VehicleDrivingFlags.DriveAroundVehicles | (int)VehicleDrivingFlags.DriveAroundPeds | 8 | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing | 524288,  //20220531 allowwrongway and median crossing is new
//	Code3 = (int)VehicleDrivingFlags.DriveAroundVehicles | (int)VehicleDrivingFlags.DriveAroundPeds | 8 | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing | 524288,
//	Code3Close = (int)VehicleDrivingFlags.DriveAroundVehicles | (int)VehicleDrivingFlags.DriveAroundPeds | 8 | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing | (int)VehicleDrivingFlags.DriveBySight | 524288,
//	//Panic = (int)VehicleDrivingFlags.DriveAroundVehicles | (int)VehicleDrivingFlags.DriveAroundPeds | 8 | (int)VehicleDrivingFlags.DriveAroundObjects | (int)VehicleDrivingFlags.AllowWrongWay | (int)VehicleDrivingFlags.AllowMedianCrossing | 524288,

//	Vanilla_Alerted = (int)786469,



//	DrunkDriving = VanillaDrivingStyles.DF_ChangeLanesAroundObstructions | VanillaDrivingStyles.DF_UseWanderFallbackInsteadOfStraightLine | VanillaDrivingStyles.DF_DriveIntoOncomingTraffic | VanillaDrivingStyles.DF_SwerveAroundAllCars,
//    RecklessDriving = VanillaDrivingStyles.DF_SwerveAroundAllCars | VanillaDrivingStyles.DF_SteerAroundObjects | VanillaDrivingStyles.DF_UseShortCutLinks | VanillaDrivingStyles.DF_ChangeLanesAroundObstructions,
//    SpeedingDriving = VanillaDrivingStyles.DF_SwerveAroundAllCars | VanillaDrivingStyles.DF_StopAtLights | VanillaDrivingStyles.DF_SteerAroundObjects | VanillaDrivingStyles.DF_UseShortCutLinks | VanillaDrivingStyles.DF_ChangeLanesAroundObstructions,

//}

public enum VanillaDrivingStyles : int
{
    DF_StopForCars = 1,
    DF_StopForPeds = 2,
    DF_SwerveAroundAllCars = 4,
    DF_SteerAroundStationaryCars = 8,
    DF_SteerAroundPeds = 16,
    DF_SteerAroundObjects = 32,
    DF_DontSteerAroundPlayerPed = 64,
    DF_StopAtLights = 128,
    DF_GoOffRoadWhenAvoiding = 256,
    DF_DriveIntoOncomingTraffic = 512,
    DF_DriveInReverse = 1024,

    //if pathfinding fails, cruise randomly instead of going on a straight line
    DF_UseWanderFallbackInsteadOfStraightLine = 2048,

    DF_AvoidRestrictedAreas = 4096,

    // These only work on MISSION_CRUISE
    DF_PreventBackgroundPathfinding = 8192,
    DF_AdjustCruiseSpeedBasedOnRoadSpeed = 16384,

    DF_UseShortCutLinks = 262144,
    DF_ChangeLanesAroundObstructions = 524288,
    DF_UseSwitchedOffNodes = 2097152,   //cruise tasks ignore this anyway--only used for goto's
    DF_PreferNavmeshRoute = 4194304,    //if you're going to be primarily driving off road

    // Only works for planes using MISSION_GOTO, will cause them to drive along the ground instead of fly
    DF_PlaneTaxiMode = 8388608,

    DF_ForceStraightLine = 16777216,
    DF_UseStringPullingAtJunctions = 33554432,

    DF_AvoidHighways = 536870912,
    DF_ForceJoinInRoadDirection = 1073741824,
}


/*
 * ENUM DRIVINGMODE

	DF_StopForCars					= 1,
	DF_StopForPeds					= 2,
	DF_SwerveAroundAllCars			= 4,
	DF_SteerAroundStationaryCars	= 8,
	DF_SteerAroundPeds				= 16,
	DF_SteerAroundObjects			= 32,
	DF_DontSteerAroundPlayerPed		= 64,
	DF_StopAtLights					= 128,
	DF_GoOffRoadWhenAvoiding		= 256,
	DF_DriveIntoOncomingTraffic		= 512,
	DF_DriveInReverse				= 1024,
	
	//if pathfinding fails, cruise randomly instead of going on a straight line
	DF_UseWanderFallbackInsteadOfStraightLine = 2048,

	DF_AvoidRestrictedAreas			= 4096,

	// These only work on MISSION_CRUISE
	DF_PreventBackgroundPathfinding		= 8192,
	DF_AdjustCruiseSpeedBasedOnRoadSpeed = 16384,
	
	DF_UseShortCutLinks				=  262144,
	DF_ChangeLanesAroundObstructions = 524288,
	DF_UseSwitchedOffNodes			=  2097152,	//cruise tasks ignore this anyway--only used for goto's
	DF_PreferNavmeshRoute			=  4194304,	//if you're going to be primarily driving off road
	
	// Only works for planes using MISSION_GOTO, will cause them to drive along the ground instead of fly
	DF_PlaneTaxiMode				=  8388608,

	DF_ForceStraightLine			= 16777216,
	DF_UseStringPullingAtJunctions	= 33554432,
	
	DF_AvoidHighways				= 536870912,
	DF_ForceJoinInRoadDirection		= 1073741824,

	//standard driving mode. stops for cars, peds, and lights, goes around stationary obstructions
	DRIVINGMODE_STOPFORCARS = 786603,//DF_StopForCars|DF_StopForPeds|DF_SteerAroundObjects|DF_SteerAroundStationaryCars|DF_StopAtLights|DF_UseShortCutLinks|DF_ChangeLanesAroundObstructions,		// Obey lights too
	
	//like the above, but doesn't steer around anything in its way--will only wait instead.
	DRIVINGMODE_STOPFORCARS_STRICT = 262275,//DF_StopForCars|DF_StopForPeds|DF_StopAtLights|DF_UseShortCutLinks,		// Doesn't deviate an inch.
	
	//default "alerted" driving mode. drives around everything, doesn't obey lights
	DRIVINGMODE_AVOIDCARS = 786469, //DF_SwerveAroundAllCars|DF_SteerAroundObjects|DF_UseShortCutLinks|DF_ChangeLanesAroundObstructions|DF_StopForCars,
	
	//very erratic driving. difference between this and AvoidCars is that it doesn't use the brakes at ALL to help with steering
	DRIVINGMODE_AVOIDCARS_RECKLESS = 786468, //DF_SwerveAroundAllCars|DF_SteerAroundObjects|DF_UseShortCutLinks|DF_ChangeLanesAroundObstructions,
	
	//smashes through everything
	DRIVINGMODE_PLOUGHTHROUGH = 262144, //DF_UseShortCutLinks
	
	//drives normally except for the fact that it ignores lights
	DRIVINGMODE_STOPFORCARS_IGNORELIGHTS =786475,//DF_StopForCars|DF_SteerAroundStationaryCars|DF_StopForPeds|DF_SteerAroundObjects|DF_UseShortCutLinks|DF_ChangeLanesAroundObstructions
	
	//try to swerve around everything, but stop for lights if necessary
	DRIVINGMODE_AVOIDCARS_OBEYLIGHTS = 786597,//DF_SwerveAroundAllCars|DF_StopAtLights|DF_SteerAroundObjects|DF_UseShortCutLinks|DF_ChangeLanesAroundObstructions|DF_StopForCars
	
	//swerve around cars, be careful around peds, and stop for lights
	DRIVINGMODE_AVOIDCARS_STOPFORPEDS_OBEYLIGHTS = 786599//DF_SwerveAroundAllCars|DF_StopAtLights|DF_StopForPeds|DF_SteerAroundObjects|DF_UseShortCutLinks|DF_ChangeLanesAroundObstructions|DF_StopForCars,
ENDENUM*/