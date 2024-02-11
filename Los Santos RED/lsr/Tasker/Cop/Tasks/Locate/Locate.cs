using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Locate : ComplexTask
{
    private bool NeedsUpdates;
    private Vector3 CurrentTaskedPosition;
    private Task CurrentTask = Task.Nothing;
    private bool HasReachedReportedPosition;
    private bool isSetCode3Close;
    private bool hasSixthSense = false;
    private ISettingsProvideable Settings;
    private Vector3 PlaceToGoTo => hasSixthSense ? Player.PlacePoliceShouldSearchForPlayer : Player.PlacePoliceLastSeenPlayer;

    private enum Task
    {
        Wander,
        GoTo,
        Nothing,
    }
    private Task CurrentTaskDynamic
    {
        get
        {
            if (HasReachedReportedPosition)
            {
                return Task.Wander;
            }
            else
            {
                return Task.GoTo;
            }
        }
    }
    public Locate(IComplexTaskable cop, ITargetable player, ISettingsProvideable settings) : base(player, cop, 1000)
    {
        Name = "Locate";
        SubTaskName = "";
        Settings = settings;
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            //NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 10f);//tr cruise speed test
            hasSixthSense = RandomItems.RandomPercent(Ped.IsInHelicopter ? Settings.SettingsManager.PoliceTaskSettings.SixthSenseHelicopterPercentage : Settings.SettingsManager.PoliceTaskSettings.SixthSensePercentage);
            if (!hasSixthSense && Ped.DistanceToPlayer <= 40f && RandomItems.RandomPercent(Settings.SettingsManager.PoliceTaskSettings.SixthSensePercentageClose))
            {
                hasSixthSense = true;
            }

            EntryPoint.WriteToConsole($"LOCATE TASK: Cop {Ped.Handle} hasSixthSense {hasSixthSense}");
            Update();
        }
    }
    public override void Update()
    {
        if (Ped.Pedestrian.Exists() && ShouldUpdate)
        {
            if (CurrentTask != CurrentTaskDynamic)
            {
                CurrentTask = CurrentTaskDynamic;
                ExecuteCurrentSubTask();
            }
            else if (NeedsUpdates)
            {
                ExecuteCurrentSubTask();
            }
            SetVehicle();
        }
    }
    public override void ReTask()
    {

    }
    private void ExecuteCurrentSubTask()
    {
        if (CurrentTask == Task.Wander)
        {
            SubTaskName = "Wander";
            Wander();
        }
        else if (CurrentTask == Task.GoTo)
        {
            SubTaskName = "GoTo";
            GoTo();
        }
        GameTimeLastRan = Game.GameTime;
    }
    private void Wander()
    {
        NeedsUpdates = false;
        if (Ped == null || !Ped.Pedestrian.Exists())
        {
            return;
        }
        if (Ped.Pedestrian.IsInAnyVehicle(false) && Ped.Pedestrian.CurrentVehicle.Exists())
        {
            WanderInVehicle();
        }
        else
        {
            WanderOnFoot();
        }
    }
    private void WanderInVehicle()
    {
        if (!Ped.Pedestrian.Exists() || !Ped.IsDriver)
        {
            return;
        }
        Ped.Pedestrian.BlockPermanentEvents = Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringVehicleLocate;
        Ped.Pedestrian.KeepTasks = true;
        NativeFunction.Natives.TASK_VEHICLE_DRIVE_WANDER(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 30f, (int)eCustomDrivingStyles.Code3, 10f);
    }
    private void WanderOnFoot()
    {
        if (!Ped.Pedestrian.Exists())
        {
            return;
        }
        Ped.Pedestrian.BlockPermanentEvents = Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringLocate;
        Ped.Pedestrian.KeepTasks = true;
        NativeFunction.Natives.TASK_WANDER_IN_AREA(Ped.Pedestrian, PlaceToGoTo, 100f, 3.0f, 6.0f);
        //NativeFunction.Natives.TASK_WANDER_STANDARD(Ped.Pedestrian, 0, 0);
    }
    private void GoTo()
    {
        if (Ped == null || !Ped.Pedestrian.Exists())
        {
            return;
        }
        NeedsUpdates = true;
        if (CurrentTaskedPosition.DistanceTo2D(PlaceToGoTo) >= 5f && !HasReachedReportedPosition)
        {
            HasReachedReportedPosition = false;
            CurrentTaskedPosition = PlaceToGoTo;
            if (Ped.Pedestrian.IsInAnyVehicle(false))
            {
                GoToInVehicle();
            }
            else
            {
                GoToOnFoot();
            }
        }
        CheckGoToDistances();
        SetGoToDrivingStyle();
    }

    private void CheckGoToDistances()
    {
        float DistanceToCoordinates = Ped.Pedestrian.DistanceTo2D(CurrentTaskedPosition);
        if (Ped.Pedestrian.IsInAirVehicle)
        {
            //if (DistanceToCoordinates <= 150f)
            //{
            //    NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 10f);//tr cruise speed test
            //}
            //else
            //{
            //    NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 50f);//tr cruise speed test

            //}
        }
        if (DistanceToCoordinates <= 25f)
        {
            if (hasSixthSense && Player.SearchMode.IsInStartOfSearchMode)
            {

            }
            else
            {
                HasReachedReportedPosition = true;
            }
            EntryPoint.WriteToConsole($"LOCATE TASK: Cop {Ped.Handle} HAS REACHED POSITION");
        }
    }
    private void SetGoToDrivingStyle()
    {
        if (Ped.IsDriver && !Ped.IsInHelicopter && !Ped.IsInBoat && Ped.DistanceToPlayer <= Settings.SettingsManager.PoliceTaskSettings.DriveBySightDuringLocateDistance && Settings.SettingsManager.PoliceTaskSettings.AllowDriveBySightDuringLocate)// && Player.CurrentLocation.IsOffroad && Player.CurrentLocation.HasBeenOffRoad)
        {
            if (!isSetCode3Close)
            {
                NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.Code3Close);
                isSetCode3Close = true;
            }
        }
        else
        {
            if (isSetCode3Close)
            {
                NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.Code3);
                isSetCode3Close = false;
            }
        }
    }

    private void GoToInVehicle()
    {
        if (Ped == null || !Ped.Pedestrian.Exists() || CurrentTaskedPosition == null || CurrentTaskedPosition == Vector3.Zero || !Ped.IsDriver)
        {
            return;
        }
        Ped.Pedestrian.BlockPermanentEvents = Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringVehicleLocate;
        Ped.Pedestrian.KeepTasks = true;
        if (Ped.IsInHelicopter)
        {
            NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, 0, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 4, 50f, 150f, -1f, -1, 30, -1.0f, 0);//NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, 0, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 4, 50f, 10f, 0f, -1, -1, -1, 0);
        }
        if (Ped.IsInPlane)
        {
            NativeFunction.Natives.TASK_PLANE_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, 0, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 4, 70f, 40, -1.0f, 40, 20, true);
        }
        else if (Ped.IsInBoat)
        {
            NativeFunction.Natives.TASK_BOAT_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, 0, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 4, 50f, (int)eCustomDrivingStyles.Code3, -1.0f, 7);
        }
        else
        {
            NativeFunction.Natives.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 70f, (int)eCustomDrivingStyles.Code3, 10f); //30f speed
        }
    }
    private void GoToOnFoot()
    {
        if (Ped == null || !Ped.Pedestrian.Exists() || CurrentTaskedPosition == null || CurrentTaskedPosition == Vector3.Zero)
        {
            return;
        }
        Ped.Pedestrian.BlockPermanentEvents = Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringLocate;
        Ped.Pedestrian.KeepTasks = true;
        //EntryPoint.WriteToConsole($"GoToOnFoot1 {Ped.Pedestrian.Handle} {CurrentTaskedPosition}");
        //EntryPoint.WriteToConsole($"GoToOnFoot2 {CurrentTaskedPosition}");
        NativeFunction.Natives.TASK_FOLLOW_NAV_MESH_TO_COORD(Ped.Pedestrian, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 15f, -1, 0.25f, 0, 40000.0f);
        //NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Ped.Pedestrian, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 15f, -1, 0f, 0f);

        /*//INFO: IMPORTANT NOTE : Sometimes a path may not be able to be found. This could happen because there simply isn't any way to get there, or maybe a bunch of dynamic objects have blocked the way, 
        //  or maybe the destination is too far away. In this case the ped will simply stand still.
        //  To identify when this has happened, you can use GET_NAVMESH_ROUTE_RESULT. This will help you find situations where peds cannot get to their target. 
        //PARAM NOTES:Time is an INT value specifying milliseconds. If the ped has not achieved the target within the allotted time then the ped will be teleported to the target. 
        // if Time is chosen to be -1 the ped will never warp. 
        //PURPOSE: Tells a ped to follow the navmesh to the given coord. More info..  
        NATIVE PROC TASK_FOLLOW_NAV_MESH_TO_COORD(PED_INDEX PedIndex
        ,VECTOR VecCoors
        , FLOAT MoveBlendRatio
        , INT Time = DEFAULT_TIME_BEFORE_WARP
        , FLOAT Radius = DEFAULT_NAVMESH_RADIUS
        , ENAV_SCRIPT_FLAGS NavFlags = ENAV_DEFAULT
        , FLOAT FinalHeading = DEFAULT_NAVMESH_FINAL_HEADING 
        */
    }
    private void SetVehicle()
    {
        if (Settings.SettingsManager.WorldSettings.AllowSettingSirenState && Ped.IsDriver && Ped.Pedestrian.Exists() && Ped.Pedestrian.CurrentVehicle.Exists() && Ped.Pedestrian.CurrentVehicle.HasSiren && !Ped.Pedestrian.CurrentVehicle.IsSirenOn)
        {
            Ped.Pedestrian.CurrentVehicle.IsSirenOn = true;
            Ped.Pedestrian.CurrentVehicle.IsSirenSilent = false;
        }
        if (Ped.IsInVehicle)
        {
            NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, Settings.SettingsManager.PoliceTaskSettings.DriverAbility);
            NativeFunction.Natives.SET_DRIVER_AGGRESSIVENESS(Ped.Pedestrian, Settings.SettingsManager.PoliceTaskSettings.DriverAggressiveness);
            if (Settings.SettingsManager.PoliceTaskSettings.DriverRacing > 0f)
            {
                NativeFunction.Natives.SET_DRIVER_RACING_MODIFIER(Ped.Pedestrian, Settings.SettingsManager.PoliceTaskSettings.DriverRacing);
            }
        }
        if (Ped.IsDriver && (Ped.IsInHelicopter || Ped.IsInPlane))
        {
            Ped.ControlLandingGear();
        }
    }
    public override void Stop()
    {

    }

    //private void Wander()
    //{
    //    NeedsUpdates = false;
    //    if (Ped.Pedestrian.Exists())
    //    {
    //        if (Ped.Pedestrian.IsInAnyVehicle(false) && Ped.Pedestrian.CurrentVehicle.Exists())
    //        {
    //            if (Ped.IsDriver)
    //            {
    //                if (Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringVehicleLocate)
    //                {
    //                    Ped.Pedestrian.BlockPermanentEvents = true;
    //                }
    //                else
    //                {
    //                    Ped.Pedestrian.BlockPermanentEvents = false;
    //                }
    //                Ped.Pedestrian.KeepTasks = true;
    //                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 30f, (int)eCustomDrivingStyles.Code3, 10f);
    //            }
    //        }
    //        else
    //        {
    //            if (Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringLocate)
    //            {
    //                Ped.Pedestrian.BlockPermanentEvents = true;
    //            }
    //            else
    //            {
    //                Ped.Pedestrian.BlockPermanentEvents = false;
    //            }
    //            Ped.Pedestrian.KeepTasks = true;
    //            NativeFunction.Natives.TASK_WANDER_STANDARD(Ped.Pedestrian, 0, 0);
    //        }
    //    }
    //}
    //private void GoTo()
    //{
    //    if (Ped.Pedestrian.Exists())
    //    {
    //        NeedsUpdates = true;
    //        if (CurrentTaskedPosition.DistanceTo2D(PlaceToGoTo) >= 5f && !HasReachedReportedPosition)
    //        {
    //            HasReachedReportedPosition = false;
    //            CurrentTaskedPosition = PlaceToGoTo;
    //            if (Ped.Pedestrian.IsInAnyVehicle(false))
    //            {
    //                if (Ped.IsDriver)
    //                {
    //                    if (Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringVehicleLocate)
    //                    {
    //                        Ped.Pedestrian.BlockPermanentEvents = true;
    //                    }
    //                    else
    //                    {
    //                        Ped.Pedestrian.BlockPermanentEvents = false;
    //                    }
    //                    Ped.Pedestrian.KeepTasks = true;
    //                    if (Ped.IsInHelicopter)
    //                    {
    //                        NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, 0, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 4, 50f, 150f, -1f, -1, 30, -1.0f, 0);//NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, 0, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 4, 50f, 10f, 0f, -1, -1, -1, 0);
    //                    }
    //                    else if (Ped.IsInBoat)
    //                    {
    //                        NativeFunction.Natives.TASK_BOAT_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, 0, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 4, 50f, (int)eCustomDrivingStyles.Code3, -1.0f, 7);
    //                    }
    //                    else
    //                    {
    //                        NativeFunction.Natives.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 70f, (int)eCustomDrivingStyles.Code3, 10f); //30f speed
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                if (Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringLocate)
    //                {
    //                    Ped.Pedestrian.BlockPermanentEvents = true;
    //                }
    //                else
    //                {
    //                    Ped.Pedestrian.BlockPermanentEvents = false;
    //                }
    //                Ped.Pedestrian.KeepTasks = true;
    //                NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Ped.Pedestrian, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 15f, -1, 0f, 0f);
    //            }
    //        }
    //        float DistanceToCoordinates = Ped.Pedestrian.DistanceTo2D(CurrentTaskedPosition);
    //        if (Ped.Pedestrian.IsInAirVehicle)
    //        {
    //            if (DistanceToCoordinates <= 150f)
    //            {
    //                NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 10f);
    //            }
    //            else
    //            {
    //                NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 50f);

    //            }
    //        }
    //        if (DistanceToCoordinates <= 25f)
    //        {
    //            if (hasSixthSense && Player.SearchMode.IsInStartOfSearchMode)
    //            {

    //            }
    //            else
    //            {
    //                HasReachedReportedPosition = true;
    //            }

    //            EntryPoint.WriteToConsole($"LOCATE TASK: Cop {Ped.Handle} HAS REACHED POSITION");
    //        }
    //        if (Ped.IsDriver && !Ped.IsInHelicopter && !Ped.IsInBoat && Ped.DistanceToPlayer <= Settings.SettingsManager.PoliceTaskSettings.DriveBySightDuringLocateDistance && Settings.SettingsManager.PoliceTaskSettings.AllowDriveBySightDuringLocate)// && Player.CurrentLocation.IsOffroad && Player.CurrentLocation.HasBeenOffRoad)
    //        {
    //            if (!isSetCode3Close)
    //            {
    //                NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.Code3Close);
    //                isSetCode3Close = true;
    //            }
    //        }
    //        else
    //        {
    //            if (isSetCode3Close)
    //            {
    //                NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.Code3);
    //                isSetCode3Close = false;
    //            }
    //        }
    //    }
    //}
}



//using LosSantosRED.lsr.Interface;
//using Rage;
//using Rage.Native;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//public class Locate : ComplexTask
//{
//    private bool NeedsUpdates;
//    private Vector3 CurrentTaskedPosition;
//    private Task CurrentTask = Task.Nothing;
//    private bool HasReachedReportedPosition;
//    private bool isSetCode3Close;
//    private bool hasSixthSense = false;
//    private ISettingsProvideable Settings;


//    private Vector3 prevPlaceToGoTo;
//    private Vector3 prevPlaceToGoToOnFoot;

//    private Vector3 PlaceToGoTo => hasSixthSense ? Player.StreetPlacePoliceShouldSearchForPlayer : Player.StreetPlacePoliceLastSeenPlayer;
//    private Vector3 PlaceToGoToOnFoot => hasSixthSense ? Player.PlacePoliceShouldSearchForPlayer : Player.PlacePoliceLastSeenPlayer;
//    private enum Task
//    {
//        Wander,
//        GoTo,
//        Nothing,
//        InvestigateOnFoot,
//    }
//    private Task CurrentTaskDynamic
//    {
//        get
//        {
//            if(!HasReachedReportedPosition)
//            {
//                return Task.GoTo;
//            }
//            if (!Ped.IsInHelicopter && Player.IsOnFoot && Player.PoliceLastSeenOnFoot && Player.IsNearbyPlacePoliceShouldSearchForPlayer)
//            {
//                return Task.InvestigateOnFoot;
//            }
//            else
//            {
//                return Task.Wander;
//            }
//        }
//    }
//    public Locate(IComplexTaskable cop, ITargetable player, ISettingsProvideable settings) : base(player, cop, 1000)
//    {
//        Name = "Locate";
//        SubTaskName = "";
//        Settings = settings;
//    }
//    public override void Start()
//    {
//        if(!Ped.Pedestrian.Exists())
//        {
//            return;
//        }
//        if(!Ped.IsInHelicopter)
//        {
//            NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 10f);
//        }       
//        hasSixthSense = RandomItems.RandomPercent(Ped.IsInHelicopter ? Settings.SettingsManager.PoliceTaskSettings.SixthSenseHelicopterPercentage : Settings.SettingsManager.PoliceTaskSettings.SixthSensePercentage);
//        if (!hasSixthSense && Ped.DistanceToPlayer <= 40f && RandomItems.RandomPercent(Settings.SettingsManager.PoliceTaskSettings.SixthSensePercentageClose))
//        {
//            hasSixthSense = true;
//        }
//        //EntryPoint.WriteToConsoleTestLong($"LOCATE TASK: Cop {Ped.Handle} hasSixthSense {hasSixthSense}");
//        Update();    
//    }
//    public override void Update()
//    {
//        if(!Ped.Pedestrian.Exists() || !ShouldUpdate)
//        {
//            return;
//        }
//        if (CurrentTask != CurrentTaskDynamic)
//        {
//            CurrentTask = CurrentTaskDynamic;
//            ExecuteCurrentSubTask();
//        }
//        else if (NeedsUpdates)
//        {
//            ExecuteCurrentSubTask();
//        }
//        SetVehicle();      
//    }
//    public override void ReTask()
//    {

//    }
//    private void ExecuteCurrentSubTask()
//    {
//        if (CurrentTask == Task.Wander)
//        {
//            SubTaskName = "Wander";
//            Wander();
//        }
//        else if (CurrentTask == Task.GoTo)
//        {
//            SubTaskName = "GoTo";
//            GoTo();
//        }
//        else if (CurrentTask == Task.InvestigateOnFoot)
//        {
//            SubTaskName = "InvestigateOnFoot";
//            InvestigateOnFoot();
//        }
//        GameTimeLastRan = Game.GameTime;
//    }


//    private void InvestigateOnFoot()
//    {
//        if (Ped == null || !Ped.Pedestrian.Exists())
//        {
//            return;
//        }
//        NeedsUpdates = true;
//        if(prevPlaceToGoTo != PlaceToGoTo || prevPlaceToGoToOnFoot != PlaceToGoToOnFoot)
//        {
//            EntryPoint.WriteToConsole("LOCATE InvestigateOnFoot POSITION CHANGED");
//            prevPlaceToGoTo = PlaceToGoTo;
//            prevPlaceToGoToOnFoot = PlaceToGoToOnFoot;
//            InvestigatePosition();
//        }
//    }


//    private void InvestigatePosition()
//    {
//        if (!Ped.Pedestrian.Exists())
//        {
//            return;
//        }
//        Ped.Pedestrian.BlockPermanentEvents = Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringVehicleLocate;
//        Ped.Pedestrian.KeepTasks = true;

//        Vector3 RandomPlaceOnFoot = PlaceToGoToOnFoot.Around2D(15f);

//        if (Ped.Pedestrian.IsInAnyVehicle(false) && Ped.Pedestrian.CurrentVehicle.Exists())
//        { 
//            EntryPoint.WriteToConsole("LOCATE SET TO LEAVE VEHICLE AND WANDER");
//            unsafe
//            {
//                int lol = 0;
//                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
//                NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", 0, Ped.Pedestrian.CurrentVehicle, 27, 1000);
//                NativeFunction.CallByName<bool>("TASK_LEAVE_VEHICLE", 0, Ped.Pedestrian.CurrentVehicle, 256);
//                NativeFunction.CallByName<bool>("TASK_FOLLOW_NAV_MESH_TO_COORD", 0, RandomPlaceOnFoot.X, RandomPlaceOnFoot.Y, RandomPlaceOnFoot.Z, 100.0f, -1, 0f, 0, 0f);//15f, -1, 0.25f, 0, 40000.0f);
//                NativeFunction.CallByName<bool>("TASK_WANDER_IN_AREA", 0, PlaceToGoToOnFoot.X, PlaceToGoToOnFoot.Y, PlaceToGoToOnFoot.Z, 150f, 0.0f,0.0f);
//                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
//                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
//                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
//                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
//            }
//        }
//        else
//        {
//            EntryPoint.WriteToConsole("LOCATE SET TO JUST WANDER");
//            unsafe
//            {
//                int lol = 0;
//                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
//                NativeFunction.CallByName<bool>("TASK_FOLLOW_NAV_MESH_TO_COORD", 0, RandomPlaceOnFoot.X, RandomPlaceOnFoot.Y, RandomPlaceOnFoot.Z,100.0f, -1, 0f, 0, 0f);//15f, -1, 0.25f, 0, 40000.0f);
//                NativeFunction.CallByName<bool>("TASK_WANDER_IN_AREA", 0, PlaceToGoToOnFoot.X, PlaceToGoToOnFoot.Y, PlaceToGoToOnFoot.Z, 150f, 0.0f, 0.0f);//DONT REALLY WNADER MOST TIMES....
//                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
//                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
//                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
//                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
//            }
//        }
//    }



//    private void Wander()
//    {
//        NeedsUpdates = false;
//        if (Ped == null ||  !Ped.Pedestrian.Exists())
//        {
//            return;
//        }
//        if (Ped.Pedestrian.IsInAnyVehicle(false) && Ped.Pedestrian.CurrentVehicle.Exists())
//        {
//            WanderInVehicle();
//        }
//        else
//        {
//            WanderOnFoot();
//        }     
//    }
//    private void WanderInVehicle()
//    {
//        if(!Ped.Pedestrian.Exists() || !Ped.IsDriver)
//        {
//            return;
//        }
//        Ped.Pedestrian.BlockPermanentEvents = Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringVehicleLocate;    
//        Ped.Pedestrian.KeepTasks = true;
//        NativeFunction.Natives.TASK_VEHICLE_DRIVE_WANDER(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 30f, (int)eCustomDrivingStyles.Code3, 10f);   
//    }
//    private void WanderOnFoot()
//    {
//        if (!Ped.Pedestrian.Exists())
//        {
//            return;
//        }
//        Ped.Pedestrian.BlockPermanentEvents = Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringLocate;
//        Ped.Pedestrian.KeepTasks = true;
//        NativeFunction.Natives.TASK_WANDER_IN_AREA(Ped.Pedestrian, PlaceToGoTo, 100f, 3.0f, 6.0f);
//        //NativeFunction.Natives.TASK_WANDER_STANDARD(Ped.Pedestrian, 0, 0);
//    }
//    private void GoTo()
//    {
//        if (Ped == null || !Ped.Pedestrian.Exists())
//        {
//            return;
//        }
//        NeedsUpdates = true;
//        if (CurrentTaskedPosition.DistanceTo2D(PlaceToGoTo) >= 5f && !HasReachedReportedPosition)
//        {
//            HasReachedReportedPosition = false;
//            CurrentTaskedPosition = PlaceToGoTo;
//            if (Ped.Pedestrian.IsInAnyVehicle(false))
//            {
//                GoToInVehicle();
//            }
//            else
//            {
//                GoToOnFoot();
//            }
//        }
//        CheckGoToDistances();
//        SetGoToDrivingStyle();
//    }

//    private void CheckGoToDistances()
//    {
//        float DistanceToCoordinates = Ped.Pedestrian.DistanceTo2D(CurrentTaskedPosition);
//        if (Ped.Pedestrian.IsInAirVehicle)
//        {
//            if (DistanceToCoordinates <= 150f)
//            {
//                NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 10f);
//            }
//            else
//            {
//                NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 50f);

//            }
//        }
//        else
//        {
//            if(DistanceToCoordinates >= 50)
//            {
//                NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 70f);
//            }
//            else if (DistanceToCoordinates >= 35f)
//            {
//                NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 10f);
//            }
//            else
//            {
//                NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 5f);
//            }
//        }
//        if (DistanceToCoordinates <= 25f)
//        {
//            if (hasSixthSense && Player.SearchMode.IsInStartOfSearchMode)
//            {

//            }
//            else
//            {
//                HasReachedReportedPosition = true;
//            }
//            //EntryPoint.WriteToConsoleTestLong($"LOCATE TASK: Cop {Ped.Handle} HAS REACHED POSITION");
//        }     
//    }
//    private void SetGoToDrivingStyle()
//    {
//        if (Ped.IsDriver && !Ped.IsInHelicopter && !Ped.IsInBoat && Ped.DistanceToPlayer <= Settings.SettingsManager.PoliceTaskSettings.DriveBySightDuringLocateDistance && Settings.SettingsManager.PoliceTaskSettings.AllowDriveBySightDuringLocate)// && Player.CurrentLocation.IsOffroad && Player.CurrentLocation.HasBeenOffRoad)
//        {
//            if (!isSetCode3Close)
//            {
//                NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.Code3Close);
//                isSetCode3Close = true;
//            }
//        }
//        else
//        {
//            if (isSetCode3Close)
//            {
//                NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(Ped.Pedestrian, (int)eCustomDrivingStyles.Code3);
//                isSetCode3Close = false;
//            }
//        }
//    }

//    private void GoToInVehicle()
//    {
//        if(Ped == null || !Ped.Pedestrian.Exists() || CurrentTaskedPosition == null || CurrentTaskedPosition == Vector3.Zero || !Ped.IsDriver)
//        {
//            return;
//        }
//        Ped.Pedestrian.BlockPermanentEvents = Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringVehicleLocate;
//        Ped.Pedestrian.KeepTasks = true;
//        if (Ped.IsInHelicopter)
//        {
//            NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, 0, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 4, 50f, 150f, -1f, -1, 30, -1.0f, 0);//NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, 0, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 4, 50f, 10f, 0f, -1, -1, -1, 0);
//        }
//        else if (Ped.IsInBoat)
//        {
//            NativeFunction.Natives.TASK_BOAT_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, 0, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 4, 50f, (int)eCustomDrivingStyles.Code3, -1.0f, 7);
//        }
//        else
//        {
//            NativeFunction.Natives.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 70f, (int)eCustomDrivingStyles.Code3, 10f); //30f speed
//        }    
//    }
//    private void GoToOnFoot()
//    {
//        if (Ped == null || !Ped.Pedestrian.Exists() || CurrentTaskedPosition == null || CurrentTaskedPosition == Vector3.Zero)
//        {
//            return;
//        }
//        Ped.Pedestrian.BlockPermanentEvents = Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringLocate;
//        Ped.Pedestrian.KeepTasks = true;
//        //EntryPoint.WriteToConsole($"GoToOnFoot1 {Ped.Pedestrian.Handle} {CurrentTaskedPosition}");
//        //EntryPoint.WriteToConsole($"GoToOnFoot2 {CurrentTaskedPosition}");
//        NativeFunction.Natives.TASK_FOLLOW_NAV_MESH_TO_COORD(Ped.Pedestrian, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 15f, -1, 0.25f, 0, 40000.0f);
//        //NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Ped.Pedestrian, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 15f, -1, 0f, 0f);

//            /*//INFO: IMPORTANT NOTE : Sometimes a path may not be able to be found. This could happen because there simply isn't any way to get there, or maybe a bunch of dynamic objects have blocked the way, 
//            //  or maybe the destination is too far away. In this case the ped will simply stand still.
//            //  To identify when this has happened, you can use GET_NAVMESH_ROUTE_RESULT. This will help you find situations where peds cannot get to their target. 
//            //PARAM NOTES:Time is an INT value specifying milliseconds. If the ped has not achieved the target within the allotted time then the ped will be teleported to the target. 
//            // if Time is chosen to be -1 the ped will never warp. 
//            //PURPOSE: Tells a ped to follow the navmesh to the given coord. More info..  
//            NATIVE PROC TASK_FOLLOW_NAV_MESH_TO_COORD(PED_INDEX PedIndex
//            ,VECTOR VecCoors
//            , FLOAT MoveBlendRatio
//            , INT Time = DEFAULT_TIME_BEFORE_WARP
//            , FLOAT Radius = DEFAULT_NAVMESH_RADIUS
//            , ENAV_SCRIPT_FLAGS NavFlags = ENAV_DEFAULT
//            , FLOAT FinalHeading = DEFAULT_NAVMESH_FINAL_HEADING 
//            */
//    }
//    private void SetVehicle()
//    {
//        if(!Ped.IsInVehicle || !Ped.Pedestrian.Exists())
//        {
//            return;
//        }
//        if (Settings.SettingsManager.PoliceTaskSettings.AllowSettingSirenState && Ped.Pedestrian.CurrentVehicle.Exists() && Ped.Pedestrian.CurrentVehicle.HasSiren && !Ped.Pedestrian.CurrentVehicle.IsSirenOn)
//        {
//            Ped.Pedestrian.CurrentVehicle.IsSirenOn = true;
//            Ped.Pedestrian.CurrentVehicle.IsSirenSilent = false;
//        }
//        NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, Settings.SettingsManager.PoliceTaskSettings.DriverAbility);
//        NativeFunction.Natives.SET_DRIVER_AGGRESSIVENESS(Ped.Pedestrian, Settings.SettingsManager.PoliceTaskSettings.DriverAggressiveness);
//        if (Settings.SettingsManager.PoliceTaskSettings.DriverRacing > 0f)
//        {
//            NativeFunction.Natives.SET_DRIVER_RACING_MODIFIER(Ped.Pedestrian, Settings.SettingsManager.PoliceTaskSettings.DriverRacing);
//        }      
//    }
//    public override void Stop()
//    {

//    }
//}

