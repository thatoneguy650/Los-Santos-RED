using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


class DrivePlayerInVehicleTaskState : TaskState
{
    private PedExt PedGeneral;
    private IEntityProvideable World;
    private SeatAssigner SeatAssigner;

    private VehicleExt TaskedVehicle;
    private int TaskedSeat;
    private ISettingsProvideable Settings;
    private ITargetable Player;
    private bool BlockPermanentEvents = false;
    private Vector3 PlaceToDriveTo;
    private bool isSetCode3Close;
    private ILocationReachable LocationReachable;
    private uint GametimeLastRetasked;
    private float drivingSpeed;
    private Vector3 AssignedDrivePlace;
    private bool IsSetAggressive;
    private GeneralFollow GeneralFollow;
    private bool ReachedLocation;
    private bool IsParking;
    private Vector3 ParkingSpace;
    private float ParkingHeading;

    public DrivePlayerInVehicleTaskState(PedExt pedGeneral, ITargetable player, IEntityProvideable world, SeatAssigner seatAssigner, ISettingsProvideable settings, 
        bool blockPermanentEvents, ILocationReachable locationReachable, GeneralFollow generalFollow)
    {
        PedGeneral = pedGeneral;
        Player = player;
        World = world;
        SeatAssigner = seatAssigner;
        Settings = settings;
        BlockPermanentEvents = blockPermanentEvents;
        LocationReachable = locationReachable;
        GeneralFollow = generalFollow;
    }
    public bool IsValid => PedGeneral != null && PedGeneral.IsInVehicle && !LocationReachable.HasReachedLocatePosition && Player.CurrentVehicle != null && PedGeneral.Pedestrian.Exists() && PedGeneral.Pedestrian.CurrentVehicle.Exists() && Player.CurrentVehicle.Handle == PedGeneral.Pedestrian.CurrentVehicle.Handle;
    public string DebugName => $"DrivePlayerInVehicleTaskState";
    public void Dispose()
    {
        Stop();
    }
    public void Start()
    {
        PedGeneral.ClearTasks(true);
        TaskEntry();
    }
    public void Stop()
    {
        PedGeneral.ClearTasks(true);
    }
    public void Update()
    {
        if (!PedGeneral.Pedestrian.Exists())
        {
            return;
        }


        


        Vector3 NewDrivePlace = Player.GPSManager.GetGPSRoutePosition();


        float DistanceToCoordinates = PedGeneral.Pedestrian.DistanceTo(AssignedDrivePlace);
        if (NewDrivePlace == Vector3.Zero && DistanceToCoordinates <= 50f)
        {
            ReachedLocation = true;
            EntryPoint.WriteToConsole($"NewDrivePlace Update REACHED THE LOCATION");
        }
        else if(!IsParking)
        {
            ReachedLocation = false;
            EntryPoint.WriteToConsole($"NewDrivePlace Update DID NOT REACHED THE LOCATION");
        }

        //
        //if (DistanceToCoordinates <= 50f)
        //{
        //    ReachedLocation = true;
        //}
        //else
        //{
        //    ReachedLocation = false;
        //}


        //if (NewDrivePlace == Vector3.Zero || AssignedDrivePlace == Vector3.Zero)
        //{
        //    //IsBlipGone = true;
        //    EntryPoint.WriteToConsole($"NewDrivePlace BLIP IS GONE, DONT DO ANYTHING");
        //}
        //else if(NewDrivePlace == AssignedDrivePlace)
        //{
        //   // IsBlipGone = false;


        //    if(DistanceToCoordinates <= 50f)
        //    {
        //        ReachedLocation = true;
        //        EntryPoint.WriteToConsole($"NewDrivePlace YOU ARE STILL DRIVING TO THE SAME SPOT AND YOU REACHED THE LOCATION");
        //    }

        //    EntryPoint.WriteToConsole($"NewDrivePlace YOU ARE STILL DRIVING TO THE SAME SPOT");
        //}
        //else if(NewDrivePlace != AssignedDrivePlace)
        //{
        //    ReachedLocation = false;
        //    EntryPoint.WriteToConsole($"NewDrivePlace YOU GOT A NEW POSITION TO DRIVE TO RESETTING REACHED LOCATION");
        //}

        if(NewDrivePlace != AssignedDrivePlace && NewDrivePlace != Vector3.Zero)
        {
            IsParking = false;
            EntryPoint.WriteToConsole($"NewDrivePlace STOPPED PARKING SINCE YOU SET A NEW THINGO");
        }


        if (NewDrivePlace != AssignedDrivePlace && !IsParking)
        {
            EntryPoint.WriteToConsole($"GoToBlipInVehicleTaskState SET NEW BLIP OR NOT NewDrivePlace{NewDrivePlace}");
            //if(NewDrivePlace != Vector3.Zero)
            //{
            //    ReachedLocation = false;
            //}

            //if(!ReachedLocation || NewDrivePlace != Vector3.Zero)
            //{



                TaskEntry();
            //}

            
        }





        //CheckTasks();
        //CheckGoToDistances();


        //if (ReachedLocation)
        //{
            
        //}



        //SetGoToDrivingStyle();
        if (PedGeneral.IsDriver && (PedGeneral.IsInHelicopter || PedGeneral.IsInPlane))
        {
            PedGeneral.ControlLandingGear();
        }



        float newDrivingSpeed = GeneralFollow.SetCombat ? 100f : Player.CurrentLocation != null && Player.CurrentLocation.CurrentStreet != null ? Player.CurrentLocation.CurrentStreet.SpeedLimitMS : 10f;


        if (GeneralFollow.SetCombat && !IsSetAggressive)
        {
            IsSetAggressive = true;
            SetAggressiveDriving();
            TaskEntry();
        }
        if (!GeneralFollow.SetCombat && IsSetAggressive)
        {
            IsSetAggressive = false;
            SetPassiveDriving();
            TaskEntry();
        }

        if (!GeneralFollow.SetCombat && newDrivingSpeed != drivingSpeed)
        {
            IsSetAggressive = false;
            SetPassiveDriving();
            TaskEntry();
        }



    }

   
    private void SetPassiveDriving()
    {
        //NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(PedGeneral.Pedestrian, 10f);
        //NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(PedGeneral.Pedestrian, (int)eCustomDrivingStyles.RegularDriving);
        EntryPoint.WriteToConsole("DrivePlayerInVehicleTaskState PASSSIVE DRIVNG RAN");
    }

    private void SetAggressiveDriving()
    {
        //NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(PedGeneral.Pedestrian, 100f);
        //NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(PedGeneral.Pedestrian, (int)eCustomDrivingStyles.TaxiCrazy_AvoidCars);
        EntryPoint.WriteToConsole("DrivePlayerInVehicleTaskState AGGRESSIVE DRIVNG RAN");
    }

    private void CheckTasks()
    {
        Rage.TaskStatus taskStatus = PedGeneral.Pedestrian.Tasks.CurrentTaskStatus;
        if (PedGeneral.IsDriver && (taskStatus == Rage.TaskStatus.NoTask || taskStatus == Rage.TaskStatus.Preparing) && Game.GameTime - GametimeLastRetasked >= 2000)
        {
            //PedGeneral.ClearTasks(true);
            TaskEntry();
            GametimeLastRetasked = Game.GameTime;
            EntryPoint.WriteToConsole($"LOCATE TASK: Cop {PedGeneral?.Handle} RETASKED");
        }
    }

    private void TaskEntry()
    {
        if (!PedGeneral.Pedestrian.Exists())
        {
            return;
        }
        if (BlockPermanentEvents)
        {
            PedGeneral.Pedestrian.BlockPermanentEvents = true;
            PedGeneral.Pedestrian.KeepTasks = true;
        }
        PlaceToDriveTo = Player.GPSManager.GetGPSRoutePosition();

        //if(PlaceToDriveTo != Vector3.Zero)
        //{
        //    ReachedLocation = false;
        //}
        //ReachedLocation = false;

        int DrivingStyle = GeneralFollow.SetCombat ? (int)eCustomDrivingStyles.RecklessDriving : (int)eCustomDrivingStyles.RegularDriving;
        drivingSpeed = GeneralFollow.SetCombat ? 100f : Player.CurrentLocation != null && Player.CurrentLocation.CurrentStreet != null ? Player.CurrentLocation.CurrentStreet.SpeedLimitMS : 10f;

        
        bool pedExists = PedGeneral != null && PedGeneral.Pedestrian.Exists();
        bool pedVehicleExists = PedGeneral != null && PedGeneral.Pedestrian.Exists() && PedGeneral.Pedestrian.CurrentVehicle.Exists();
        bool locationEror = PlaceToDriveTo == null || PlaceToDriveTo == Vector3.Zero;
        // Vector3 JitterPlace = PlaceToDriveTo.Around2D(5f);

        EntryPoint.WriteToConsole($"GoToBlipInVehicleTaskState TaskEntry PlaceToDriveTo{PlaceToDriveTo} locationEror{locationEror}");
        if (locationEror && PedGeneral != null && PedGeneral.Pedestrian.Exists() && PedGeneral.Pedestrian.CurrentVehicle.Exists())
        {
            
            if(Player.GroupManager.PlayerDriverWander)
            {

           
                unsafe
                {
                    int lol = 0;
                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                    //NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(1000, 2000));
                    NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, PedGeneral.Pedestrian.CurrentVehicle, drivingSpeed, DrivingStyle);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, Ped.Pedestrian.CurrentVehicle, 10f, (int)(VehicleDrivingFlags.FollowTraffic | VehicleDrivingFlags.YieldToCrossingPedestrians | VehicleDrivingFlags.RespectIntersections | (VehicleDrivingFlags)8), 10f);
                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", PedGeneral.Pedestrian, lol);
                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                }
                IsParking = false;
            }
            else if (ReachedLocation)
            {
                if (!IsParking)
                {
                    ParkVehicle();
                }
            }
            else
            {
                
                //PedGeneral.ClearTasks(true);
                NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, 6, 9999);
                IsParking = false;
            }
            GametimeLastRetasked = Game.GameTime;
            return;
        }

        EntryPoint.WriteToConsole("THE TASKS ARE BEING ASSIGNED TO DRIVE THE PLAYER");
        IsParking = false;
        AssignedDrivePlace = PlaceToDriveTo;

        if (PedGeneral == null || !PedGeneral.Pedestrian.Exists() || !PedGeneral.Pedestrian.CurrentVehicle.Exists() || PlaceToDriveTo == null || PlaceToDriveTo == Vector3.Zero || !PedGeneral.IsDriver)
        {
            //EntryPoint.WriteToConsole($"LOCATE TASK: Cop {PedGeneral?.Handle} TASK ENTRY ABORTED Driver:{PedGeneral?.IsDriver} pedExists{pedExists} pedVehicleExists {pedVehicleExists} locationEror {locationEror}");
            return;
        }
        if (PedGeneral.IsInHelicopter)
        {
            //EntryPoint.WriteToConsole($"LOCATE TASK: Cop {PedGeneral?.Handle} HELI TASK ASSIGNED");
            NativeFunction.Natives.TASK_HELI_MISSION(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, 0, 0, PlaceToDriveTo.X, PlaceToDriveTo.Y, PlaceToDriveTo.Z, 6, 50f, 10f, -1f, 60, 60, -1.0f, 0);//6 = attack
            //NativeFunction.Natives.TASK_HELI_MISSION(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, 0, 0, PlaceToDriveTo.X, PlaceToDriveTo.Y, PlaceToDriveTo.Z, 4, 50f, 150f, -1f, -1, 30, -1.0f, 0);//NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, 0, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 4, 50f, 10f, 0f, -1, -1, -1, 0);
        }
        else if (PedGeneral.IsInPlane)
        {
            //EntryPoint.WriteToConsole($"LOCATE TASK: Cop {PedGeneral?.Handle} HELI TASK ASSIGNED");
            NativeFunction.Natives.TASK_PLANE_MISSION(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, 0, 0, PlaceToDriveTo.X, PlaceToDriveTo.Y, PlaceToDriveTo.Z, 4, 70f, 40, -1.0f, 40, 20, true);//THIS KINDA WORKS//NativeFunction.Natives.TASK_PLANE_MISSION(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, 0, 0, PlaceToDriveTo.X, PlaceToDriveTo.Y, PlaceToDriveTo.Z, 6, 70f, 40, -1.0f, 40, 20, true);
            //NativeFunction.Natives.TASK_HELI_MISSION(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, 0, 0, PlaceToDriveTo.X, PlaceToDriveTo.Y, PlaceToDriveTo.Z, 4, 50f, 150f, -1f, -1, 30, -1.0f, 0);//NativeFunction.Natives.TASK_HELI_MISSION(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, 0, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 4, 50f, 10f, 0f, -1, -1, -1, 0);
        }
        else if (PedGeneral.IsInBoat)
        {
            //EntryPoint.WriteToConsole($"LOCATE TASK: Cop {PedGeneral?.Handle} BOAT TASK ASSIGNED");
            NativeFunction.Natives.TASK_BOAT_MISSION(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, 0, 0, PlaceToDriveTo.X, PlaceToDriveTo.Y, PlaceToDriveTo.Z, 4, drivingSpeed, DrivingStyle, -1.0f, 7);
        }
        else
        {
            //EntryPoint.WriteToConsole($"LOCATE TASK: Cop {PedGeneral?.Handle} DRIVE TASK ASSIGNED");

            //NativeFunction.Natives.TASK_VEHICLE_DRIVE_TO_COORD(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, JitterPlace.X, JitterPlace.Y, JitterPlace.Z, 70f, 0, "", (int)eCustomDrivingStyles.Code3, 15.0f, -1);


            NativeFunction.Natives.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE(PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, PlaceToDriveTo.X, PlaceToDriveTo.Y, PlaceToDriveTo.Z, drivingSpeed, DrivingStyle, 10f); //30f speed
            EntryPoint.WriteToConsole($"DrivePlayerInVehicleTaskState GIVE DRIVE TASK TO COORDS {PlaceToDriveTo}");
        
        }
        GametimeLastRetasked = Game.GameTime;
    }

    private void CheckGoToDistances()
    {
        float DistanceToCoordinates = PedGeneral.Pedestrian.DistanceTo(AssignedDrivePlace); //PedGeneral.Pedestrian.DistanceTo2D(PlaceToDriveTo);
        //if (PedGeneral.Pedestrian.IsInAirVehicle)
        //{
        //    //if (DistanceToCoordinates <= 150f)
        //    //{
        //    //    NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(PedGeneral.Pedestrian, 10f);//tr cruise speed test
        //    //}
        //    //else
        //    //{
        //    //    NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(PedGeneral.Pedestrian, 50f);
        //    //}
        //}
        //else
        //{
        //    if (DistanceToCoordinates >= 100)
        //    {
        //        NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(PedGeneral.Pedestrian, 40f);//tr cruise speed test
        //    }
        //    else if (DistanceToCoordinates >= 45f)
        //    {
        //        NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(PedGeneral.Pedestrian, 15f);
        //    }
        //    else
        //    {
        //        NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(PedGeneral.Pedestrian, 10f);
        //    }
        //}
        if (DistanceToCoordinates <= 50f && AssignedDrivePlace != Vector3.Zero)
        {
            //if (!ReachedLocation)
            //{

            //    //LastReachedLocation = AssignedDrivePlace;
            //    ParkVehicle();
            //}
            ReachedLocation = true;
            LocationReachable.OnLocationReached();





            //LocationReachable.HasReachedLocatePosition = true;
            EntryPoint.WriteToConsole($"LOCATE TASK: DRIVER {PedGeneral?.Handle} HAS REACHED POSITION");
        }
  
        EntryPoint.WriteToConsole($"            DRIVE PLAYER: DRIVER {PedGeneral?.Handle} DistanceToCoordinates{DistanceToCoordinates}");
    }


    private void ParkVehicle()
    {
        IsParking = true;
        EntryPoint.WriteToConsole("PLAYER DRIVER ON LOCATION RECHED TRANSITION TO PARKING");

        ParkingSpace = Vector3.Zero;
        ParkingHeading = 0f;
        SpawnLocation DestinationLocation = new SpawnLocation(AssignedDrivePlace);
        DestinationLocation.Heading = PedGeneral.Pedestrian.Heading-180f;
        DestinationLocation.GetClosestStreet(false);



            DestinationLocation.GetClosestSideOfRoadForward();
       // }
        //DestinationLocation.GetRoadBoundaryPosition();
        if (DestinationLocation.HasStreetPosition)
        {
            //DestinationLocation.StreetPosition = DestinationLocation.RoadBoundaryPosition;
            ParkingSpace = DestinationLocation.StreetPosition;
            ParkingHeading = DestinationLocation.Heading;
        }

        if (ParkingSpace == Vector3.Zero)
        {
            NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, 6, 9999);
            EntryPoint.WriteToConsole("NO PARKING SPACE FOUND");
        }
        else
        {
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                //NativeFunction.CallByName<bool>("TASK_VEHICLE_TEMP_ACTION", 0, PedGeneral.Pedestrian.CurrentVehicle, 6, 9999);
                NativeFunction.CallByName<bool>("TASK_VEHICLE_PARK", 0, PedGeneral.Pedestrian.CurrentVehicle, ParkingSpace.X, ParkingSpace.Y, ParkingSpace.Z, ParkingHeading, 0, 20f, false);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", PedGeneral.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
            //AssignedDrivePlace = ParkingSpace;
            EntryPoint.WriteToConsole("SET PARK RAN");
        }

        GameFiber.StartNew(MarkParkingSpace);

    }
    private void MarkParkingSpace()
    {
#if DEBUG
        while(IsParking && EntryPoint.ModController.IsRunning)
        {

         
            Rage.Debug.DrawArrowDebug(ParkingSpace + new Vector3(0f, 0f, 1f), Vector3.Zero, Rotator.Zero, 1f, System.Drawing.Color.Red);
            GameFiber.Yield();
        }
#endif 
    }




}

