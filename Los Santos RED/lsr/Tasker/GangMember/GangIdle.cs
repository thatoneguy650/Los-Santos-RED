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


public class GangIdle : ComplexTask
{
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
    private Vector3 taskedPosition;

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
            //if (!Ped.Pedestrian.IsInAnyVehicle(false))
            //{
            //    if (Ped.DistanceToPlayer <= 75f && VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists() && VehicleTryingToEnter.Vehicle.IsDriveable && VehicleTryingToEnter.Vehicle.FreeSeatsCount > 0 && VehicleTryingToEnter.Vehicle.Speed < 1.0f) //if (Ped.DistanceToPlayer <= 75f && Ped.Pedestrian.LastVehicle.Exists() && Ped.Pedestrian.LastVehicle.IsDriveable && Ped.Pedestrian.LastVehicle.FreeSeatsCount > 0)
            //    {
            //        return Task.GetInCar;
            //    }
            //    else if (CurrentTask == Task.GetInCar)
            //    {
            //        return Task.GetInCar;
            //    }
            //    else
            //    {
            //        return Task.Wander;
            //    }
            //}
            //else
            //{
                return Task.Wander;
           // }
        }
    }
    public GangIdle(IComplexTaskable cop, ITargetable player, IEntityProvideable world, ITaskerReportable tasker, IPlacesOfInterest placesOfInterest) : base(player, cop, 1500)//1500
    {
        Name = "GangIdle";
        SubTaskName = "";
        World = world;
        Tasker = tasker;
        PlacesOfInterest = placesOfInterest;
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            //EntryPoint.WriteToConsole($"TASKER: Idle Start: {Ped.Pedestrian.Handle}", 5);
            ClearTasks(true);
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
                //EntryPoint.WriteToConsole($"COP EVENT: Wander Idle Start: {Ped.Pedestrian.Handle}", 3);
                NeedsUpdates = true;
                ClearTasks(true);



                WanderTask(IsFirstRun);
            }
            else if (Ped.DistanceToPlayer <= 150f && Ped.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask)//might be a crash cause?, is there a regular native for this?
            {
                WanderTask(IsFirstRun);
                EntryPoint.WriteToConsole($"COP EVENT: Wander Idle Reset: {Ped.Pedestrian.Handle}", 3);
            }
        }
    }
    private void WanderTask(bool IsFirstRun)
    {
        if (Ped.Pedestrian.Exists())
        {
            //Ped.Pedestrian.BlockPermanentEvents = true;
            //Ped.Pedestrian.KeepTasks = true;
            if (Ped.Pedestrian.IsInAnyVehicle(false))
            {
                if (Ped.IsDriver && Ped.Pedestrian.CurrentVehicle.Exists())
                {
                    if (Ped.IsInHelicopter)
                    {
                        NativeFunction.CallByName<bool>("TASK_HELI_MISSION", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 0, 0, 0f, 0f, 300f, 9, 50f, 150f, -1f, -1, 30, -1.0f, 0);
                    }
                    else
                    {
                        unsafe
                        {
                            int lol = 0;
                            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                            //NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(4000, 8000));
                            NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, Ped.Pedestrian.CurrentVehicle, 10f, (int)eCustomDrivingStyles.RegularDriving, 10f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, Ped.Pedestrian.CurrentVehicle, 10f, (int)(VehicleDrivingFlags.FollowTraffic | VehicleDrivingFlags.YieldToCrossingPedestrians | VehicleDrivingFlags.RespectIntersections | (VehicleDrivingFlags)8), 10f);
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
                Vector3 pedPos = Ped.Pedestrian.Position;
                if (1==0 && IsFirstRun && NativeFunction.Natives.DOES_SCENARIO_EXIST_IN_AREA<bool>(pedPos.X, pedPos.Y, pedPos.Z, 15f, true))
                {
                    NativeFunction.Natives.TASK_USE_NEAREST_SCENARIO_TO_COORD(Ped.Pedestrian, pedPos.X, pedPos.Y, pedPos.Z, 15f, 10000);
                    EntryPoint.WriteToConsole($"PED {Ped.Pedestrian.Handle} Started Scenarion", 5);
                }
                else
                {
                    NativeFunction.Natives.TASK_WANDER_STANDARD(Ped.Pedestrian, 0, 0);
                    EntryPoint.WriteToConsole($"PED {Ped.Pedestrian.Handle} Started Regular wander on foot", 5);
                }
            }
        }
    }
    private void GetInCar(bool IsFirstRun)
    {
        if (Ped.Pedestrian.Exists())
        {
            if (IsFirstRun)
            {
                NeedsUpdates = true;
            }
            if (VehicleTaskedToEnter == null || !VehicleTaskedToEnter.Exists())
            {
                GetInCarTask();
            }
            else if (VehicleTryingToEnter != null && VehicleTaskedToEnter.Exists() && !VehicleTaskedToEnter.IsSeatFree(SeatTaskedToEnter) && VehicleTaskedToEnter.GetPedOnSeat(SeatTaskedToEnter).Exists() && VehicleTaskedToEnter.GetPedOnSeat(SeatTaskedToEnter).Handle != Ped.Pedestrian.Handle)// && (VehicleTryingToEnter.Vehicle.Handle != VehicleTaskedToEnter.Handle || SeatTaskedToEnter != SeatTryingToEnter) && Ped.Pedestrian.Exists() && !Ped.Pedestrian.IsInAnyVehicle(true))
            {
                GetInCarTask();
            }
            else if (VehicleTryingToEnter != null && VehicleTaskedToEnter.Exists() && VehicleTaskedToEnter.Speed > 1.0f)// && (VehicleTryingToEnter.Vehicle.Handle != VehicleTaskedToEnter.Handle || SeatTaskedToEnter != SeatTryingToEnter) && Ped.Pedestrian.Exists() && !Ped.Pedestrian.IsInAnyVehicle(true))
            {
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
                //NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(8000, 16000));
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
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
            //EntryPoint.WriteToConsole($"COP EVENT: Nothing Idle Start: {Ped.Pedestrian.Handle}", 3);
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
