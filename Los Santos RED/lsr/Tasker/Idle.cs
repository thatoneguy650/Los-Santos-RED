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
    private bool NeedsUpdates;
    private Task CurrentTask = Task.Nothing;
    private uint GameTimeClearedIdle;
    private int SeatTryingToEnter;
    private VehicleExt VehicleTryingToEnter;
    private IEntityProvideable World;

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
                if(Ped.DistanceToPlayer <= 75f && VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists() && VehicleTryingToEnter.Vehicle.IsDriveable && VehicleTryingToEnter.Vehicle.FreeSeatsCount > 0) //if (Ped.DistanceToPlayer <= 75f && Ped.Pedestrian.LastVehicle.Exists() && Ped.Pedestrian.LastVehicle.IsDriveable && Ped.Pedestrian.LastVehicle.FreeSeatsCount > 0)
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
    public Idle(IComplexTaskable cop, ITargetable player, IEntityProvideable world) : base(player, cop, 1500)//1500
    {
        Name = "Idle";
        SubTaskName = "";
        World = world;
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            EntryPoint.WriteToConsole($"TASKER: Idle Start: {Ped.Pedestrian.Handle}", 5);
            ClearTasks(true);
            if(Ped.Pedestrian.LastVehicle.Exists() && Ped.Pedestrian.LastVehicle.IsPoliceVehicle)
            {
                VehicleTryingToEnter = World.GetVehicleExt(Ped.Pedestrian.LastVehicle.Handle);
                if (VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists())
                {
                    int? PossileSeat;
                    if (VehicleTryingToEnter.Vehicle.Model.NumberOfSeats == 1)
                    {
                        PossileSeat = VehicleTryingToEnter.Vehicle.GetFreeSeatIndex(-1, -1);
                        if (PossileSeat != null)
                        {
                            SeatTryingToEnter = PossileSeat ?? default(int);
                        }
                    }
                    else
                    {
                        PossileSeat = VehicleTryingToEnter.Vehicle.GetFreeSeatIndex(-1, 0);
                        if (PossileSeat != null)
                        {
                            SeatTryingToEnter = PossileSeat ?? default(int);
                        }
                    }
                }
                else
                {
                    GetClosesetPoliceVehicle();
                }
            }
            else
            {
                GetClosesetPoliceVehicle();
            }

            if(VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists())
            {
                EntryPoint.WriteToConsole($"COP EVENT: {Ped.Pedestrian.Handle} Get in Car START {VehicleTryingToEnter.Vehicle.Handle} Seat {SeatTryingToEnter}  ", 3);
            }
            else
            {
                EntryPoint.WriteToConsole($"COP EVENT: {Ped.Pedestrian.Handle} Get in Car START NONE Seat {SeatTryingToEnter}  ", 3);
            }
            Update();
        }
    }
    private void GetClosesetPoliceVehicle()
    {
        VehicleTryingToEnter = World.PoliceVehicleList.Where(x => x.Vehicle.Exists() && ((x.Vehicle.Model.NumberOfSeats > 1 && x.Vehicle.GetFreeSeatIndex(-1, 0) != null) || (x.Vehicle.Model.NumberOfSeats == 1 && x.Vehicle.GetFreeSeatIndex(-1, -1) != null)) && x.Vehicle.Speed == 0f).OrderBy(x => x.Vehicle.DistanceTo2D(Ped.Pedestrian)).FirstOrDefault();
        if (VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists())
        {
            int? PossileSeat = VehicleTryingToEnter.Vehicle.GetFreeSeatIndex(-1, 0);
            if (PossileSeat != null)
            {
                SeatTryingToEnter = PossileSeat ?? default(int);
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
                WanderTask();
            }
            else if (Ped.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask)//might be a crash cause?, is there a regular native for this?
            {
                WanderTask();
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
            else if (!VehicleTryingToEnter.Vehicle.IsSeatFree(SeatTryingToEnter))
            {
                GetClosesetPoliceVehicle();
                GetInCarTask();
            }
            //instead need to keep checking if the seat is free!!!!!!
        }
    }
    private void GetInCarTask()
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
                NativeFunction.CallByName<bool>("TASK_PAUSE", 0, 12000);// RandomItems.MyRand.Next(4000, 8000));
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }

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
    public override void Stop()
    {

    }
}

