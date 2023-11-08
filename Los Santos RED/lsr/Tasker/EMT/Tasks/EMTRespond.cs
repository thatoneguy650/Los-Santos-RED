using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class EMTRespond : ComplexTask
{
    private uint GameTimeStartedTreatingVictim;
    private bool NeedsUpdates;
    private Vector3 CurrentTaskedPosition;
    private Task CurrentTask = Task.Nothing;
    private bool HasReachedReportedPosition;
    private uint GameTimeLastSpoke;
    private int SeatTryingToEnter;
    private VehicleExt VehicleTryingToEnter;
    private Vehicle VehicleTaskedToEnter;
    private int SeatTaskedToEnter;
    private EMT EMT;
    private IEntityProvideable World;
    private IPlacesOfInterest PlacesOfInterest;

    private enum Task
    {
        Wander,
        GoTo,
        Nothing,
        ExitVehicle,
        TreatVictim,
        GetInCar,
    }
    private Task CurrentTaskDynamic
    {
        get
        {
            if(!HasReachedReportedPosition && !Ped.IsInVehicle && !Ped.Pedestrian.IsInAnyVehicle(false) && Ped.Pedestrian.DistanceTo2D(CurrentTaskedPosition) >= 75f)
            {
                if (Ped.DistanceToPlayer <= 75f && VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists() && VehicleTryingToEnter.Vehicle.IsDriveable && VehicleTryingToEnter.Vehicle.FreeSeatsCount > 0 && VehicleTryingToEnter.Vehicle.Speed < 1.0f) //if (Ped.DistanceToPlayer <= 75f && Ped.Pedestrian.LastVehicle.Exists() && Ped.Pedestrian.LastVehicle.IsDriveable && Ped.Pedestrian.LastVehicle.FreeSeatsCount > 0)
                {
                    return Task.GetInCar;
                }
                else if (CurrentTask == Task.GetInCar)
                {
                    return Task.GetInCar;
                }
                else
                {
                    return Task.GoTo;
                }
            }
            else if(HasReachedReportedPosition)
            {
                return Task.Wander;
            }
            return Task.GoTo;
        }
    }
    public EMTRespond(IComplexTaskable cop, ITargetable player, IEntityProvideable world, IPlacesOfInterest placesOfInterest, EMT emt) : base(player, cop, 1000)
    {
        Name = "EMTRespond";
        SubTaskName = "";
        World = world;
        EMT = emt;
        PlacesOfInterest = placesOfInterest;
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            //NativeFunction.Natives.SET_DRIVE_TASK_CRUISE_SPEED(Ped.Pedestrian, 10f);//tr cruise speed test
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
            SetSiren();
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
        else if (CurrentTask == Task.GetInCar)
        {
            RunInterval = 500;
            SubTaskName = "GetInCar";
            GetInCar();
        }
        else if (CurrentTask == Task.ExitVehicle)
        {
            RunInterval = 200;
            SubTaskName = "ExitVehicle";
            ExitVehicle();
        }
        GameTimeLastRan = Game.GameTime;
    }
    private void ExitVehicle()
    {
        NeedsUpdates = false;
        Ped.Pedestrian.BlockPermanentEvents = true;
        Ped.Pedestrian.KeepTasks = true;
        if (Ped.Pedestrian.Exists() && Ped.Pedestrian.CurrentVehicle.Exists() && OtherTarget != null && OtherTarget.Pedestrian.Exists())
        {
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", 0, Ped.Pedestrian.CurrentVehicle, 27, 1000);
                NativeFunction.CallByName<bool>("TASK_LEAVE_VEHICLE", 0, Ped.Pedestrian.CurrentVehicle, 64);
                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, OtherTarget.Pedestrian, -1, 3f, 4.4f, 1073741824, 1); //Original and works ok
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
    }
    private void Wander()
    {
        if (Ped.Pedestrian.Exists())
        {
            NeedsUpdates = false;
            if (Ped.Pedestrian.IsInAnyVehicle(false) && Ped.Pedestrian.CurrentVehicle.Exists())
            {
                Ped.Pedestrian.BlockPermanentEvents = true;
                Ped.Pedestrian.KeepTasks = true;
                //4 | 16 | 32 | 262144
                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 12f, (int)eCustomDrivingStyles.Code3, 10f);
            }
            else
            {
                Ped.Pedestrian.BlockPermanentEvents = true;
                Ped.Pedestrian.KeepTasks = true;
                Vector3 Pos = Ped.Pedestrian.Position;
                NativeFunction.Natives.TASK_WANDER_IN_AREA(Ped.Pedestrian, Pos.X, Pos.Y, Pos.Z, 45f, 0f, 0f);
            }
        }
    }
    private void GoTo()
    {
        if (Ped.Pedestrian.Exists())
        {
            NeedsUpdates = true;
            if (Player.Investigation.IsActive && Player.Investigation.RequiresEMS && CurrentTaskedPosition.DistanceTo2D(Player.Investigation.Position) >= 5f)
            {
                HasReachedReportedPosition = false;
                CurrentTaskedPosition = Player.Investigation.Position;
                UpdateGoTo();
               // EntryPoint.WriteToConsole(string.Format("TASKER: EMTRespond Position Updated 1: {0}", Ped.Pedestrian.Handle), 5);
            }
            float DistanceTo = Ped.Pedestrian.DistanceTo2D(CurrentTaskedPosition);
            if (DistanceTo <= 25f)
            {
                HasReachedReportedPosition = true;
               // EntryPoint.WriteToConsole(string.Format("TASKER: EMTRespond Position Reached: {0}", Ped.Pedestrian.Handle), 5);
            }
            else if (DistanceTo < 50f)
            {
                UpdateGoTo();
               // EntryPoint.WriteToConsole(string.Format("TASKER: EMTRespond Position Near: {0}", Ped.Pedestrian.Handle), 5);
            }
        }
    }
    private void UpdateGoTo()
    {
        if (Ped.Pedestrian.Exists())
        {
            if (Ped.Pedestrian.IsInAnyVehicle(false))
            {
                if (Ped.IsDriver && Ped.Pedestrian.CurrentVehicle.Exists())// && Ped.Pedestrian.SeatIndex == -1)
                {
                    Ped.Pedestrian.BlockPermanentEvents = true;
                    Ped.Pedestrian.KeepTasks = true;
                    if (Ped.Pedestrian.DistanceTo2D(CurrentTaskedPosition) >= 50f)
                    {
                        NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 20f, (int)eCustomDrivingStyles.Code3, 20f);
                    }
                    else
                    {
                        NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 12f, (int)eCustomDrivingStyles.Code3, 20f); //NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 15f, (int)VehicleDrivingFlags.Normal, 20f);
                    }
                    //EntryPoint.WriteToConsole(string.Format("TASKER: EMTRespond UpdateGoTo Driver: {0}", Ped.Pedestrian.Handle), 5);
                }
            }
            else
            {
                Ped.Pedestrian.BlockPermanentEvents = true;
                Ped.Pedestrian.KeepTasks = true;
                NativeFunction.Natives.TASK_FOLLOW_NAV_MESH_TO_COORD(Ped.Pedestrian, CurrentTaskedPosition.X, CurrentTaskedPosition.Y, CurrentTaskedPosition.Z, 2.0f, -1, 5f, true, 0f);
            }
        }
    }
    private void SetSiren()
    {
        if (Ped.Pedestrian.Exists() && Ped.Pedestrian.CurrentVehicle.Exists() && Ped.IsDriver && Ped.Pedestrian.CurrentVehicle.HasSiren && !Ped.Pedestrian.CurrentVehicle.IsSirenOn)
        {
            Ped.Pedestrian.CurrentVehicle.IsSirenOn = true;
            Ped.Pedestrian.CurrentVehicle.IsSirenSilent = false;
        }
    }


    private void GetInCar()
    {
        if (Ped.Pedestrian.Exists())
        {
            if (!NeedsUpdates)
            {
                //EntryPoint.WriteToConsole($"Idle {Ped.Pedestrian.Handle}: Get in Car Start", 3);
                NeedsUpdates = true;
            }
            if (VehicleTaskedToEnter == null || !VehicleTaskedToEnter.Exists())
            {
                GetClosesetAmbulanceVehicle();
                //EntryPoint.WriteToConsole($"Idle {Ped.Pedestrian.Handle}: Get in Car, Got New Car, was Blank", 3);
                GetInCarTask();
            }
            else if (VehicleTryingToEnter != null && VehicleTaskedToEnter.Exists() && !VehicleTaskedToEnter.IsSeatFree(SeatTaskedToEnter) && VehicleTaskedToEnter.GetPedOnSeat(SeatTaskedToEnter).Exists() && VehicleTaskedToEnter.GetPedOnSeat(SeatTaskedToEnter).Handle != Ped.Pedestrian.Handle)// && (VehicleTryingToEnter.Vehicle.Handle != VehicleTaskedToEnter.Handle || SeatTaskedToEnter != SeatTryingToEnter) && Ped.Pedestrian.Exists() && !Ped.Pedestrian.IsInAnyVehicle(true))
            {
                GetClosesetAmbulanceVehicle();
                //EntryPoint.WriteToConsole($"Idle {Ped.Pedestrian.Handle}: Get in Car Got New Car, was occupied?", 3);
                GetInCarTask();
            }
            else if (VehicleTryingToEnter != null && VehicleTaskedToEnter.Exists() && VehicleTaskedToEnter.Speed > 1.0f)// && (VehicleTryingToEnter.Vehicle.Handle != VehicleTaskedToEnter.Handle || SeatTaskedToEnter != SeatTryingToEnter) && Ped.Pedestrian.Exists() && !Ped.Pedestrian.IsInAnyVehicle(true))
            {
                GetClosesetAmbulanceVehicle();
                //EntryPoint.WriteToConsole($"Idle {Ped.Pedestrian.Handle}: Get in Car Got New Car, was driving away?", 3);
                GetInCarTask();
            }
        }
    }
    private void GetInCarTask()
    {
        if (Ped.Pedestrian.Exists() && VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists())
        {
            //EntryPoint.WriteToConsole($"Idle {Ped.Pedestrian.Handle}: Get in Car TASK START", 3);
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            VehicleTaskedToEnter = VehicleTryingToEnter.Vehicle;
            SeatTaskedToEnter = SeatTryingToEnter;
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, VehicleTryingToEnter.Vehicle, -1, SeatTryingToEnter, 1f, 9);
                NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(8000, 16000));
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
    }
    private void GetClosesetAmbulanceVehicle()
    {
        if (Ped.AssignedVehicle != null)
        {
            VehicleExt ClosestAvailableEMSVehicle = null;
            int OpenSeatInClosestAvailableEMSVehicle = 9;
            float ClosestAvailableEMSVehicleDistance = 999f;
            if (Ped.AssignedVehicle != null && Ped.AssignedVehicle.Vehicle.Exists() && Ped.AssignedVehicle.Vehicle.IsSeatFree(Ped.AssignedSeat) && !World.Pedestrians.IsSeatAssigned(Ped, Ped.AssignedVehicle, Ped.AssignedSeat) && NativeFunction.Natives.x639431E895B9AA57<bool>(Ped.Pedestrian, Ped.AssignedVehicle.Vehicle, Ped.AssignedSeat, false, true))
            {
                OpenSeatInClosestAvailableEMSVehicle = Ped.AssignedSeat;
                ClosestAvailableEMSVehicle = Ped.AssignedVehicle;
            }
            else if (Ped.Pedestrian.LastVehicle.Exists())// && Ped.Pedestrian.LastVehicle.IsPoliceVehicle)
            {
                VehicleExt myCopCar = World.Vehicles.GetVehicleExt(Ped.Pedestrian.LastVehicle);
                if (myCopCar != null && myCopCar.Vehicle.Exists() && myCopCar.Vehicle.IsSeatFree(Ped.LastSeatIndex) && !World.Pedestrians.IsSeatAssigned(Ped, myCopCar, Ped.LastSeatIndex) && NativeFunction.Natives.x639431E895B9AA57<bool>(Ped.Pedestrian, myCopCar.Vehicle, Ped.LastSeatIndex, false, true))
                {
                    OpenSeatInClosestAvailableEMSVehicle = Ped.LastSeatIndex;
                    ClosestAvailableEMSVehicle = myCopCar;
                }
            }
            VehicleTryingToEnter = ClosestAvailableEMSVehicle;
            SeatTryingToEnter = OpenSeatInClosestAvailableEMSVehicle;
            if (ClosestAvailableEMSVehicle != null && ClosestAvailableEMSVehicle.Vehicle.Exists())
            {
                World.Pedestrians.RemoveSeatAssignment(Ped);
                World.Pedestrians.AddSeatAssignment(Ped, ClosestAvailableEMSVehicle, OpenSeatInClosestAvailableEMSVehicle);
                //EntryPoint.WriteToConsole($"Idle {Ped.Pedestrian.Handle}: Seat Assigned Vehicle {VehicleTryingToEnter.Vehicle.Handle} Seat {SeatTryingToEnter}", 3);
            }
            else
            {
                foreach (VehicleExt ambulance in World.Vehicles.EMSVehicles)
                {
                    if (ambulance.Vehicle.Exists() && ambulance.Vehicle.Speed < 0.5f)//stopped 4 door car with at least one seat free in back
                    {
                        float DistanceTo = ambulance.Vehicle.DistanceTo2D(Ped.Pedestrian);
                        if (DistanceTo <= 50f)
                        {
                            if (ambulance.Vehicle.IsSeatFree(-1) && !World.Pedestrians.IsSeatAssigned(Ped, ambulance, -1) && NativeFunction.Natives.x639431E895B9AA57<bool>(Ped.Pedestrian, ambulance.Vehicle, -1, false, true))
                            {
                                if (DistanceTo < ClosestAvailableEMSVehicleDistance)
                                {
                                    OpenSeatInClosestAvailableEMSVehicle = -1;
                                    ClosestAvailableEMSVehicle = ambulance;
                                    ClosestAvailableEMSVehicleDistance = DistanceTo;
                                }

                            }
                            else if (ambulance.Vehicle.IsSeatFree(0) && !World.Pedestrians.IsSeatAssigned(Ped, ambulance, 0) && NativeFunction.Natives.x639431E895B9AA57<bool>(Ped.Pedestrian, ambulance.Vehicle, 0, false, true))
                            {
                                if (DistanceTo < ClosestAvailableEMSVehicleDistance)
                                {
                                    OpenSeatInClosestAvailableEMSVehicle = 0;
                                    ClosestAvailableEMSVehicle = ambulance;
                                    ClosestAvailableEMSVehicleDistance = DistanceTo;
                                }
                            }
                        }
                    }
                }
                VehicleTryingToEnter = ClosestAvailableEMSVehicle;
                SeatTryingToEnter = OpenSeatInClosestAvailableEMSVehicle;
                if (ClosestAvailableEMSVehicle != null && ClosestAvailableEMSVehicle.Vehicle.Exists())
                {
                    World.Pedestrians.RemoveSeatAssignment(Ped);
                    World.Pedestrians.AddSeatAssignment(Ped, ClosestAvailableEMSVehicle, OpenSeatInClosestAvailableEMSVehicle);
                    //EntryPoint.WriteToConsole($"Idle {Ped.Pedestrian.Handle}: Seat Assigned Vehicle {VehicleTryingToEnter.Vehicle.Handle} Seat {SeatTryingToEnter}", 3);
                }
                else
                {
                    //EntryPoint.WriteToConsole($"Idle {Ped.Pedestrian.Handle}: Seat NOT Assigned", 3);
                }
            }

        }
        //Tasker.PrintAllSeatAssignments();
    }


    public override void Stop()
    {

    }
}

