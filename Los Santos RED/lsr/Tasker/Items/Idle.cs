using LosSantosRED.lsr.Interface;
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
    private enum Task
    {
        GetInCar,
        Wander,
        Nothing,
    }
    private Task CurrentTaskDynamic
    {
        get
        {
            if (Ped.DistanceToPlayer <= 75f && Ped.Pedestrian.Tasks.CurrentTaskStatus != Rage.TaskStatus.InProgress && !Ped.Pedestrian.IsInAnyVehicle(false) && Ped.Pedestrian.LastVehicle.Exists() && Ped.Pedestrian.LastVehicle.IsDriveable && Ped.Pedestrian.LastVehicle.FreeSeatsCount > 0)
            {
                return Task.GetInCar;
            }
            else
            {
                return Task.Wander;
            }
        }
    }
    public Idle(IComplexTaskable cop, ITargetable player) : base(player, cop, 1000)
    {
        Name = "Idle";
        SubTaskName = "";
    }
    public override void Start()
    {
        Game.Console.Print($"TASKER: Idle Start: {Ped.Pedestrian.Handle}");
        ClearTasks();
        Update();
    }
    private void ClearTasks()//temp public
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
            Ped.Pedestrian.BlockPermanentEvents = false;
            Ped.Pedestrian.KeepTasks = false;
            Ped.Pedestrian.Tasks.Wander();
            Ped.Pedestrian.Tasks.Clear();

           // Ped.Pedestrian.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Player, Relationship.Neutral);
            if (WasInVehicle && !Ped.Pedestrian.IsInAnyVehicle(false) && CurrentVehicle != null)
            {
                Ped.Pedestrian.WarpIntoVehicle(CurrentVehicle, seatIndex);
            }
            //Game.Console.Print(string.Format("     ClearedTasks: {0}", Ped.Pedestrian.Handle));
        }
    }
    public override void Update()
    {
        if (Ped.Pedestrian.Exists() && ShouldUpdate)
        {
            if (CurrentTask != CurrentTaskDynamic)
            {
                CurrentTask = CurrentTaskDynamic;
                //Game.Console.Print($"      Idle SubTask Changed: {Ped.Pedestrian.Handle} to {CurrentTask} {CurrentDynamic}");
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
            SubTaskName = "Wander";
            Wander(IsFirstRun);
        }
        else if (CurrentTask == Task.GetInCar)
        {
            SubTaskName = "GetInCar";
            GetInCar(IsFirstRun);
        }
        GameTimeLastRan = Game.GameTime;
    }
    private void Wander(bool IsFirstRun)
    {
        if(IsFirstRun)
        {
            NeedsUpdates = false;
            if (Ped.Pedestrian.Exists())
            {
                if (Ped.Pedestrian.IsInAnyVehicle(false))
                {
                    if (Ped.IsDriver && Ped.Pedestrian.CurrentVehicle.Exists())
                    {
                        NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 15f, (int)VehicleDrivingFlags.Normal, 10f);
                    }
                }
                else
                {
                    Ped.Pedestrian.Tasks.Wander();
                }
                //Game.Console.Print(string.Format("Idle Began Wander: {0}", Ped.Pedestrian.Handle));
            }
        }
        //else
        //{
        //    if(Ped.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.Interrupted)
        //    {
        //        if (Ped.Pedestrian.Exists())
        //        {
        //            if (Ped.Pedestrian.IsInAnyVehicle(false))
        //            {
        //                if (Ped.IsDriver && Ped.Pedestrian.CurrentVehicle.Exists())
        //                {
        //                    NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, 15f, (int)VehicleDrivingFlags.Normal, 10f);
        //                }
        //            }
        //            else
        //            {
        //                Ped.Pedestrian.Tasks.Wander();
        //            }
        //            Game.Console.Print(string.Format("Idle Interrupted Wander: {0}", Ped.Pedestrian.Handle));
        //        }
        //    }
        //}

    }
    private void GetInCar(bool IsFirstRun)
    {
        if (IsFirstRun)
        {
            NeedsUpdates = false;
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, Ped.Pedestrian.LastVehicle, -1, Ped.LastSeatIndex, 1f, 9);
                NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(4000, 8000));
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
            //Game.Console.Print(string.Format("Idle Began GetInCar: {0}", Ped.Pedestrian.Handle));
        }
        //else
        //{
        //    if (Ped.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.Interrupted)
        //    {
        //        unsafe
        //        {
        //            int lol = 0;
        //            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
        //            NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, Ped.Pedestrian.LastVehicle, -1, Ped.LastSeatIndex, 1f, 9);
        //            NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(4000, 8000));
        //            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
        //            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
        //            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
        //            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        //        }
        //        Game.Console.Print(string.Format("Idle Interrupted GetInCar: {0}", Ped.Pedestrian.Handle));
        //    }
        //}
    }
    private void SetSiren()
    {
        if (Ped.Pedestrian.CurrentVehicle.Exists() && Ped.Pedestrian.CurrentVehicle.HasSiren && Ped.Pedestrian.CurrentVehicle.IsSirenOn)
        {
            Ped.Pedestrian.CurrentVehicle.IsSirenOn = false;
            Ped.Pedestrian.CurrentVehicle.IsSirenSilent = false;
        }
    }
    public override void Stop()
    {

    }
}

