using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Idle : ComplexTask
{
    private bool IsReturningToStation = false;
    private bool NeedsUpdates;
    private Task CurrentTask = Task.Nothing;
    private uint GameTimeClearedIdle;
    private int SeatTryingToEnter;
    private VehicleExt VehicleTryingToEnter;
    private IEntityProvideable World;
    private ITaskerReportable Tasker;
    private Vehicle VehicleTaskedToEnter;
    private int SeatTaskedToEnter;
    private IPlacesOfInterest PlacesOfInterest;

    private enum Task
    {
        GetInCar,
        Wander,
        Nothing,
        OtherTarget,
    }
    private Task CurrentTaskDynamic
    {
        get
        {
            if(!Ped.Pedestrian.IsInAnyVehicle(false))
            {
                if(Ped.DistanceToPlayer <= 75f && VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists() && VehicleTryingToEnter.Vehicle.IsDriveable && VehicleTryingToEnter.Vehicle.FreeSeatsCount > 0 && VehicleTryingToEnter.Vehicle.Speed < 1.0f) //if (Ped.DistanceToPlayer <= 75f && Ped.Pedestrian.LastVehicle.Exists() && Ped.Pedestrian.LastVehicle.IsDriveable && Ped.Pedestrian.LastVehicle.FreeSeatsCount > 0)
                {
                    return Task.GetInCar;
                }
                else if (CurrentTask == Task.GetInCar)
                {
                    return Task.GetInCar;
                }
                else
                {
                    return Task.Wander;
                }
            }
            else
            {
                return Task.Wander;
            }
        }
    }
    public Idle(IComplexTaskable cop, ITargetable player, IEntityProvideable world, ITaskerReportable tasker, IPlacesOfInterest placesOfInterest) : base(player, cop, 1500)//1500
    {
        Name = "Idle";
        SubTaskName = "";
        World = world;
        Tasker = tasker;
        PlacesOfInterest = placesOfInterest;
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            EntryPoint.WriteToConsole($"TASKER: Idle Start: {Ped.Pedestrian.Handle}", 5);
            ClearTasks(true);
            GetClosesetPoliceVehicle();
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
                //EntryPoint.WriteToConsole($"      Idle SubTask Changed: {Ped.Pedestrian.Handle} to {CurrentTask} {CurrentDynamic}");
                ExecuteCurrentSubTask(true);
            }
            else if (NeedsUpdates)
            {
                ExecuteCurrentSubTask(false);
            }
            Ped.Pedestrian.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Player, Relationship.Neutral);
            SetSiren();
        }
    }
    public override void Stop()
    {

    }
    private void ExecuteCurrentSubTask(bool IsFirstRun)
    {
        if (CurrentTask == Task.Wander)
        {
            RunInterval = 1500;
            SubTaskName = "Wander";
            Wander(IsFirstRun);
        }
        else if (CurrentTask == Task.GetInCar)
        {
            RunInterval = 500;
            SubTaskName = "GetInCar";
            GetInCar(IsFirstRun);
        }
        else if (CurrentTask == Task.Nothing)
        {
            RunInterval = 1500;
            SubTaskName = "Nothing";
            Nothing(IsFirstRun);
        }
        GameTimeLastRan = Game.GameTime;
    }
    private void Wander(bool IsFirstRun)
    {
        if (Ped.Pedestrian.Exists())
        {
            if (IsFirstRun)
            {
                EntryPoint.WriteToConsole($"COP EVENT: Wander Idle Start: {Ped.Pedestrian.Handle}", 3);
                NeedsUpdates = true;
                ClearTasks(true);
                if(Ped.Pedestrian.CurrentVehicle.Exists())
                {
                    foreach(Ped ped in Ped.Pedestrian.CurrentVehicle.Passengers)
                    {
                        PedExt pedExt = World.GetPedExt(ped.Handle);
                        if(pedExt != null && pedExt.IsArrested)
                        {
                            IsReturningToStation = true;
                            break;
                        }
                        if(ped.Handle == Player.Character.Handle)
                        {
                            IsReturningToStation = true;
                            break;
                        }
                    }
                }
                WanderTask();
            }
            else if (Ped.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask)//might be a crash cause?, is there a regular native for this?
            {
                WanderTask();
                EntryPoint.WriteToConsole($"COP EVENT: Wander Idle Reset: {Ped.Pedestrian.Handle}", 3);
            }
        }
    }
    private void WanderTask()
    {
        if (Ped.Pedestrian.Exists())
        {
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            if (Ped.Pedestrian.IsInAnyVehicle(false))
            {
                if (Ped.IsDriver && Ped.Pedestrian.CurrentVehicle.Exists())
                {
                    if(IsReturningToStation)
                    {
                        GameLocation closestPoliceStation = PlacesOfInterest.GetClosestLocation(Ped.Pedestrian.Position, LocationType.Police);
                        if(closestPoliceStation != null)
                        {
                            Vector3 taskedPosition = NativeHelper.GetStreetPosition(closestPoliceStation.LocationPosition);
                            NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, taskedPosition.X, taskedPosition.Y, taskedPosition.Z, 12f, (int)VehicleDrivingFlags.Normal, 20f);
                        }
                        else
                        {
                            unsafe
                            {
                                int lol = 0;
                                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(4000, 8000));
                                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, Ped.Pedestrian.CurrentVehicle, 10f, (int)VehicleDrivingFlags.Normal, 10f);
                                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                            }
                        }
                    }
                    else
                    {
                        unsafe
                        {
                            int lol = 0;
                            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                            NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(4000, 8000));
                            NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, Ped.Pedestrian.CurrentVehicle, 10f, (int)VehicleDrivingFlags.Normal, 10f);
                            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                        }
                    }

                }
            }
            else
            {
                //Ped.Pedestrian.Tasks.Wander();
                NativeFunction.Natives.TASK_WANDER_STANDARD(Ped.Pedestrian, 0, 0);
            }
        }
    }
    private void GetInCar(bool IsFirstRun)
    {
        if (Ped.Pedestrian.Exists())
        {
            if (IsFirstRun)
            {
                EntryPoint.WriteToConsole($"Idle {Ped.Pedestrian.Handle}: Get in Car Start", 3);
                NeedsUpdates = true;
            }
            if (VehicleTaskedToEnter == null || !VehicleTaskedToEnter.Exists())
            {
                GetClosesetPoliceVehicle();
                EntryPoint.WriteToConsole($"Idle {Ped.Pedestrian.Handle}: Get in Car, Got New Car, was Blank", 3);
                GetInCarTask();
            }
            else if (VehicleTryingToEnter != null && VehicleTaskedToEnter.Exists() && !VehicleTaskedToEnter.IsSeatFree(SeatTaskedToEnter) && VehicleTaskedToEnter.GetPedOnSeat(SeatTaskedToEnter).Exists() && VehicleTaskedToEnter.GetPedOnSeat(SeatTaskedToEnter).Handle != Ped.Pedestrian.Handle)// && (VehicleTryingToEnter.Vehicle.Handle != VehicleTaskedToEnter.Handle || SeatTaskedToEnter != SeatTryingToEnter) && Ped.Pedestrian.Exists() && !Ped.Pedestrian.IsInAnyVehicle(true))
            {
                GetClosesetPoliceVehicle();
                EntryPoint.WriteToConsole($"Idle {Ped.Pedestrian.Handle}: Get in Car Got New Car, was occupied?", 3);
                GetInCarTask();
            }
            else if (VehicleTryingToEnter != null && VehicleTaskedToEnter.Exists() && VehicleTaskedToEnter.Speed > 1.0f)// && (VehicleTryingToEnter.Vehicle.Handle != VehicleTaskedToEnter.Handle || SeatTaskedToEnter != SeatTryingToEnter) && Ped.Pedestrian.Exists() && !Ped.Pedestrian.IsInAnyVehicle(true))
            {
                GetClosesetPoliceVehicle();
                EntryPoint.WriteToConsole($"Idle {Ped.Pedestrian.Handle}: Get in Car Got New Car, was driving away?", 3);
                GetInCarTask();
            }
        }
    }
    private void GetInCarTask()
    {
        if (Ped.Pedestrian.Exists() && VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists())
        {
            EntryPoint.WriteToConsole($"Idle {Ped.Pedestrian.Handle}: Get in Car TASK START", 3);
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
    private void GetClosesetPoliceVehicle()
    {
        VehicleExt ClosestAvailablePoliceVehicle = null;
        int OpenSeatInClosestAvailablePoliceVehicle = 9;
        float ClosestAvailablePoliceVehicleDistance = 999f;
        if (Ped.Pedestrian.LastVehicle.Exists() && Ped.Pedestrian.LastVehicle.IsPoliceVehicle)
        {
            VehicleExt myCopCar = World.GetVehicleExt(Ped.Pedestrian.LastVehicle);
            if (myCopCar.Vehicle.IsSeatFree(Ped.LastSeatIndex) && !Tasker.IsSeatAssigned(Ped, myCopCar, Ped.LastSeatIndex))
            {
                OpenSeatInClosestAvailablePoliceVehicle = Ped.LastSeatIndex;
                ClosestAvailablePoliceVehicle = myCopCar;
            }
        }
        VehicleTryingToEnter = ClosestAvailablePoliceVehicle;
        SeatTryingToEnter = OpenSeatInClosestAvailablePoliceVehicle;
        if (ClosestAvailablePoliceVehicle != null && ClosestAvailablePoliceVehicle.Vehicle.Exists())
        {
            Tasker.RemoveSeatAssignment(Ped);
            Tasker.AddSeatAssignment(Ped, ClosestAvailablePoliceVehicle, OpenSeatInClosestAvailablePoliceVehicle);
            EntryPoint.WriteToConsole($"Idle {Ped.Pedestrian.Handle}: Seat Assigned Vehicle {VehicleTryingToEnter.Vehicle.Handle} Seat {SeatTryingToEnter}", 3);
        }
        else
        {
            foreach (VehicleExt copCar in World.PoliceVehicleList)
            {
                if (copCar.Vehicle.Exists() && copCar.Vehicle.Speed < 0.5f)//stopped 4 door car with at least one seat free in back
                {
                    float DistanceTo = copCar.Vehicle.DistanceTo2D(Ped.Pedestrian);
                    if (DistanceTo <= 50f)
                    {
                        if (copCar.Vehicle.IsSeatFree(-1) && !Tasker.IsSeatAssigned(Ped, copCar, -1))
                        {
                            if (DistanceTo < ClosestAvailablePoliceVehicleDistance)
                            {
                                OpenSeatInClosestAvailablePoliceVehicle = -1;
                                ClosestAvailablePoliceVehicle = copCar;
                                ClosestAvailablePoliceVehicleDistance = DistanceTo;
                            }

                        }
                        else if (copCar.Vehicle.IsSeatFree(0) && !Tasker.IsSeatAssigned(Ped, copCar, 0))
                        {
                            if (DistanceTo < ClosestAvailablePoliceVehicleDistance)
                            {
                                OpenSeatInClosestAvailablePoliceVehicle = 0;
                                ClosestAvailablePoliceVehicle = copCar;
                                ClosestAvailablePoliceVehicleDistance = DistanceTo;
                            }
                        }
                    }
                }
            }
            VehicleTryingToEnter = ClosestAvailablePoliceVehicle;
            SeatTryingToEnter = OpenSeatInClosestAvailablePoliceVehicle;
            if (ClosestAvailablePoliceVehicle != null && ClosestAvailablePoliceVehicle.Vehicle.Exists())
            {
                Tasker.RemoveSeatAssignment(Ped);
                Tasker.AddSeatAssignment(Ped, ClosestAvailablePoliceVehicle, OpenSeatInClosestAvailablePoliceVehicle);
                EntryPoint.WriteToConsole($"Idle {Ped.Pedestrian.Handle}: Seat Assigned Vehicle {VehicleTryingToEnter.Vehicle.Handle} Seat {SeatTryingToEnter}", 3);
            }
            else
            {
                EntryPoint.WriteToConsole($"Idle {Ped.Pedestrian.Handle}: Seat NOT Assigned", 3);
            }
        }
        //Tasker.PrintAllSeatAssignments();
    }
    private void GetInCar_Old(bool IsFirstRun)
    {
        if (Ped.Pedestrian.Exists())
        {
            if (IsFirstRun)
            {
                EntryPoint.WriteToConsole($"COP EVENT: Get in Car Idle Start: {Ped.Pedestrian.Handle}", 3);
                NeedsUpdates = true;
                GetInCarTask();
            }
            else if (Ped.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask)
            {
                GetInCarTask();
            }
            if(VehicleTryingToEnter == null)//|| !VehicleTryingToEnter.Vehicle.Exists() || !VehicleTryingToEnter.Vehicle.IsSeatFree(SeatTryingToEnter))
            {
                GetClosesetPoliceVehicle();
                GetInCarTask();
            }
            else if (!VehicleTryingToEnter.Vehicle.Exists())
            {
                GetClosesetPoliceVehicle();
                GetInCarTask();
            }
            else if (!VehicleTryingToEnter.Vehicle.IsSeatFree(SeatTryingToEnter) || VehicleTryingToEnter.Vehicle.Speed >= 2.0f)
            {
                GetClosesetPoliceVehicle();
                GetInCarTask();
            }
            //instead need to keep checking if the seat is free!!!!!!
        }
    }
    private void GetInCarTask_Old()
    {
        if (Ped.Pedestrian.Exists() && VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists()) //if (Ped.Pedestrian.Exists() && Ped.Pedestrian.LastVehicle.Exists())
        {
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, VehicleTryingToEnter.Vehicle, -1, SeatTryingToEnter, 1f, 9);
                //NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(4000, 8000));
                NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(8000, 16000));
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }

        }
    }
    private void GetClosesetPoliceVehicle_Old()
    {
        VehicleTryingToEnter = World.PoliceVehicleList.Where(x => x.Vehicle.Exists() && ((x.Vehicle.Model.NumberOfSeats > 1 && x.Vehicle.GetFreeSeatIndex(-1, 0) != null) || (x.Vehicle.Model.NumberOfSeats == 1 && x.Vehicle.GetFreeSeatIndex(-1, -1) != null)) && x.Vehicle.Speed == 0f).OrderByDescending(x => x.Vehicle.HasPassengers).ThenBy(x => x.Vehicle.DistanceTo2D(Ped.Pedestrian)).FirstOrDefault();
        if (VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists())
        {
            if (VehicleTryingToEnter.Vehicle.Model.NumberOfSeats > 1)
            {
                int? PossileSeat = VehicleTryingToEnter.Vehicle.GetFreeSeatIndex(-1, 0);
                if (PossileSeat != null)
                {
                    SeatTryingToEnter = PossileSeat ?? default(int);
                }
            }
            else
            {
                int? PossileSeat = VehicleTryingToEnter.Vehicle.GetFreeSeatIndex(-1, -1);
                if (PossileSeat != null)
                {
                    SeatTryingToEnter = PossileSeat ?? default(int);
                }
            }

        }
        if (VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists())
        {
            EntryPoint.WriteToConsole($"COP EVENT: {Ped.Pedestrian.Handle} Get in Car UPDATE {VehicleTryingToEnter.Vehicle.Handle} Seat {SeatTryingToEnter}  ", 3);
        }
        else
        {
            EntryPoint.WriteToConsole($"COP EVENT: {Ped.Pedestrian.Handle} Get in Car UPDATE NONE Seat {SeatTryingToEnter}  ", 3);
        }
    }
    private void ClearTasks(bool resetAlertness)//temp public
    {
        if (Ped.Pedestrian.Exists())
        {
            int seatIndex = 0;
            Vehicle CurrentVehicle = null;
            bool WasInVehicle = false;
            if (Ped.Pedestrian.IsInAnyVehicle(false))
            {
                WasInVehicle = true;
                CurrentVehicle = Ped.Pedestrian.CurrentVehicle;
                seatIndex = Ped.Pedestrian.SeatIndex;
            }
            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
            Ped.Pedestrian.BlockPermanentEvents = false;
            Ped.Pedestrian.KeepTasks = false;
            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
            if (resetAlertness)
            {
                NativeFunction.Natives.SET_PED_ALERTNESS(Ped.Pedestrian, 0);
            }
            if (WasInVehicle && !Ped.Pedestrian.IsInAnyVehicle(false) && CurrentVehicle != null)
            {
                Ped.Pedestrian.WarpIntoVehicle(CurrentVehicle, seatIndex);
            }
            //EntryPoint.WriteToConsole(string.Format("     ClearedTasks: {0}", Ped.Pedestrian.Handle));
        }
    }
    private void Nothing(bool IsFirstRun)
    {
        if (Ped.Pedestrian.Exists())
        {
            EntryPoint.WriteToConsole($"COP EVENT: Nothing Idle Start: {Ped.Pedestrian.Handle}", 3);
            if (IsFirstRun)
            {
                ClearTasks(false);
                GameTimeClearedIdle = Game.GameTime;
            }
            else if (Game.GameTime - GameTimeClearedIdle >= 10000)
            {
                NativeFunction.Natives.SET_PED_ALERTNESS(Ped.Pedestrian, 0);
            }
        }
    }
    private void SetSiren()
    {
        if (Ped.Pedestrian.Exists() && Ped.Pedestrian.CurrentVehicle.Exists() && Ped.Pedestrian.CurrentVehicle.HasSiren && Ped.Pedestrian.CurrentVehicle.IsSirenOn)
        {
            Ped.Pedestrian.CurrentVehicle.IsSirenOn = false;
            Ped.Pedestrian.CurrentVehicle.IsSirenSilent = false;
        }
    }

}

