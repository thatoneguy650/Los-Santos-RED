using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GetArrested : ComplexTask
{
    private uint GameTimeFinishedArrestedAnimation;
    private bool PlayedArrestAnimation = false;
    private bool PlayedUnArrestAnimation = false;
    private uint GameTimeStartedBeingArrested;
    private IEntityProvideable World;
    private ITaskerReportable Tasker;
    private int SeatTryingToEnter;
    private VehicleExt VehicleTryingToEnter;
    private Vehicle VehicleTaskedToEnter;
    private int SeatTaskedToEnter;
    private bool NeedsUpdates;
    private Task CurrentTask = Task.Wait;
    private enum Task
    {
        GetInCar,
        SetArrested,
        UnSetArrested,
        Wait,
    }
    private Task CurrentTaskDynamic
    {
        get
        {
            if(!PlayedArrestAnimation)
            {
                return Task.SetArrested;
            }
            else if (!PlayedUnArrestAnimation)
            {
                return Task.UnSetArrested;
            }
            else if (!Ped.IsArrested)
            {
                if(Ped.IsInVehicle)
                {
                    return Task.Wait;
                }
                else
                {
                    return Task.GetInCar;
                }

            }
            else
            {
                return Task.Wait;
            }
        }
    }
    public GetArrested(IComplexTaskable ped, ITargetable player, IEntityProvideable world, ITaskerReportable tasker) : base(player, ped, 500)
    {
        Name = "GetArrested";
        SubTaskName = "";
        World = world;
        Tasker = tasker;
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            EntryPoint.WriteToConsole($"GetArrested {Ped.Pedestrian.Handle}: START", 3);
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            GameTimeStartedBeingArrested = Game.GameTime;
            SetUnarmed();
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
        if (CurrentTask == Task.SetArrested)
        {
            SubTaskName = "SetArrested";
            SetArrested(IsFirstRun);
        }
        else if (CurrentTask == Task.UnSetArrested)
        {
            SubTaskName = "UnSetArrested";
            UnSetArrested(IsFirstRun);
        }
        else if (CurrentTask == Task.GetInCar)
        {
            SubTaskName = "GetInCar";
            GetInCar(IsFirstRun);
        }
        else if (CurrentTask == Task.Wait)
        {
            SubTaskName = "Wait";
            Wait(IsFirstRun);
        }
        GameTimeLastRan = Game.GameTime;
    }
    private void SetArrested(bool IsFirstRun)
    {
        if (IsFirstRun)
        {
            SetArrestedAnimation(Ped.Pedestrian);
        }
    }
    private void UnSetArrested(bool IsFirstRun)
    {
        if (IsFirstRun)
        {
            UnSetArrestedAnimation(Ped.Pedestrian);
        }
    }
    private void Wait(bool IsFirstRun)
    {
        if(IsFirstRun)
        {
            if(Ped.Pedestrian.Exists() && Ped.IsInVehicle && !Ped.IsDriver)
            {
                Ped.Pedestrian.CanBePulledOutOfVehicles = false;
            }
        }
        if (Ped.IsInVehicle && !Ped.IsDriver && Ped.IsBusted && !Ped.IsArrested)
        {
            Ped.IsArrested = true;
            EntryPoint.WriteToConsole($"GetArrested {Ped.Pedestrian.Handle}: GotArrested in Car", 3);
        }
        //if(Ped.IsArrested && !Ped.IsInVehicle && Ped.Pedestrian.Exists() && Ped.Pedestrian.IsAlive)
        //{
        //    Ped.Pedestrian.Health = 0;
        //    Ped.Pedestrian.Kill();
        //    EntryPoint.WriteToConsole($"GetArrested {Ped.Pedestrian.Handle}: GotArrested in Car", 3);
        //}
    }
    private void GetInCar(bool IsFirstRun)
    {
        if (Ped.Pedestrian.Exists())
        {
            if (IsFirstRun)
            {
                EntryPoint.WriteToConsole($"GetArrested {Ped.Pedestrian.Handle}: Get in Car Start", 3);
                NeedsUpdates = true;
            }
            if(VehicleTaskedToEnter == null || !VehicleTaskedToEnter.Exists())
            {
                GetClosesetPoliceVehicle();
                EntryPoint.WriteToConsole($"GetArrested {Ped.Pedestrian.Handle}: Get in Car, Got New Car, was Blank", 3);
                GetInCarTask();
            }
            else if (VehicleTryingToEnter != null && VehicleTaskedToEnter.Exists() && !VehicleTaskedToEnter.IsSeatFree(SeatTaskedToEnter) && VehicleTaskedToEnter.GetPedOnSeat(SeatTaskedToEnter).Exists() && VehicleTaskedToEnter.GetPedOnSeat(SeatTaskedToEnter).Handle != Ped.Pedestrian.Handle)// && (VehicleTryingToEnter.Vehicle.Handle != VehicleTaskedToEnter.Handle || SeatTaskedToEnter != SeatTryingToEnter) && Ped.Pedestrian.Exists() && !Ped.Pedestrian.IsInAnyVehicle(true))
            {
                GetClosesetPoliceVehicle();
                EntryPoint.WriteToConsole($"GetArrested {Ped.Pedestrian.Handle}: Get in Car Got New Car, was occupied?", 3);
                GetInCarTask();
            }
            else if (VehicleTryingToEnter != null && VehicleTaskedToEnter.Exists() && VehicleTaskedToEnter.Speed > 1.0f)// && (VehicleTryingToEnter.Vehicle.Handle != VehicleTaskedToEnter.Handle || SeatTaskedToEnter != SeatTryingToEnter) && Ped.Pedestrian.Exists() && !Ped.Pedestrian.IsInAnyVehicle(true))
            {
                GetClosesetPoliceVehicle();
                EntryPoint.WriteToConsole($"GetArrested {Ped.Pedestrian.Handle}: Get in Car Got New Car, was driving away?", 3);
                GetInCarTask();
            }
            if (Ped.Pedestrian.IsGettingIntoVehicle)
            {
                NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Ped.Pedestrian);
            }
        }
    }
    private void GetInCarTask()
    {
        if (Ped.Pedestrian.Exists() && VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists())
        {
            EntryPoint.WriteToConsole($"GetArrested {Ped.Pedestrian.Handle}: Get in Car TASK START", 3);
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            VehicleTaskedToEnter = VehicleTryingToEnter.Vehicle;
            SeatTaskedToEnter = SeatTryingToEnter;
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, VehicleTryingToEnter.Vehicle, -1, SeatTryingToEnter, 1f, 9);
                NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(4000, 8000));
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
        foreach (VehicleExt copCar in World.PoliceVehicleList)
        {
            if(copCar.Vehicle.Exists() && copCar.Vehicle.Model.NumberOfSeats >= 4 && copCar.Vehicle.Speed == 0f)//stopped 4 door car with at least one seat free in back
            {
                float DistanceTo = copCar.Vehicle.DistanceTo2D(Ped.Pedestrian);
                if (DistanceTo <= 50f)
                {
                    if (copCar.Vehicle.IsSeatFree(1) && !Tasker.IsSeatAssigned(Ped, copCar, 1))
                    {
                        if (DistanceTo < ClosestAvailablePoliceVehicleDistance)
                        {
                            OpenSeatInClosestAvailablePoliceVehicle = 1;
                            ClosestAvailablePoliceVehicle = copCar;
                            ClosestAvailablePoliceVehicleDistance = DistanceTo;
                        }

                    }
                    else if (copCar.Vehicle.IsSeatFree(2) && !Tasker.IsSeatAssigned(Ped, copCar, 2))
                    {
                        if (DistanceTo < ClosestAvailablePoliceVehicleDistance)
                        {
                            OpenSeatInClosestAvailablePoliceVehicle = 2;
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
            EntryPoint.WriteToConsole($"GetArrested {Ped.Pedestrian.Handle}: Seat Assigned Vehicle {VehicleTryingToEnter.Vehicle.Handle} Seat {SeatTryingToEnter}", 3);
        }
        else
        {
            EntryPoint.WriteToConsole($"GetArrested {Ped.Pedestrian.Handle}: Seat NOT Assigned", 3);
        }


        //Tasker.PrintAllSeatAssignments();


        ////kinda works, but not really and super heavy performance.. needa better way
        //List<VehicleExt> PossibleVehicles = World.PoliceVehicleList.Where(x => x.Vehicle.Exists() && x.Vehicle.Model.NumberOfSeats >= 4 && x.Vehicle.GetFreeSeatIndex(1, 2) != null && x.Vehicle.Speed == 0f).ToList();
        //float closest = 999f;
        //VehicleExt closestCleanVehicle = null;
        //bool OtherTryingToEnter = false;
        //foreach(VehicleExt veh in PossibleVehicles)
        //{
        //    float DistanceTo = veh.Vehicle.DistanceTo2D(Ped.Pedestrian);
        //    int? PossileSeat = veh.Vehicle.GetFreeSeatIndex(1, 2);
        //    if (PossileSeat != null)
        //    {
        //        SeatTryingToEnter = PossileSeat ?? default(int);
        //        foreach (PedExt ped in World.CivilianList.Where(x => x.Pedestrian.Exists() && x.IsBusted && !x.IsArrested && x.Handle != Ped.Handle))
        //        {
        //            if (ped.Pedestrian.VehicleTryingToEnter.Exists() && ped.Pedestrian.VehicleTryingToEnter.Handle == veh.Vehicle.Handle && ped.Pedestrian.SeatIndexTryingToEnter == SeatTryingToEnter)
        //            {
        //                OtherTryingToEnter = true;
        //                break;
        //            }

        //        }
        //        if (DistanceTo <= closest && !OtherTryingToEnter)
        //        {
        //            closest = DistanceTo;
        //            closestCleanVehicle = veh;
        //        }
        //    }
        //}
        //VehicleTryingToEnter = closestCleanVehicle;
        //VehicleTryingToEnter = World.PoliceVehicleList.Where(x => x.Vehicle.Exists() && x.Vehicle.Model.NumberOfSeats >= 4 && x.Vehicle.GetFreeSeatIndex(1,2) != null && x.Vehicle.Speed == 0f).OrderBy(x => x.Vehicle.DistanceTo2D(Ped.Pedestrian)).FirstOrDefault();
        //get all of the arrested tasked civies, check what car and seat they are assigned to and disallow it!



        //if (VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists())
        //{
        //    int? PossileSeat = VehicleTryingToEnter.Vehicle.GetFreeSeatIndex(1,2);
        //    if (PossileSeat != null)
        //    {
        //        SeatTryingToEnter = PossileSeat ?? default(int);
        //    }
        //}
        //if (VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists())
        //{
        //    EntryPoint.WriteToConsole($"GetArrested {Ped.Pedestrian.Handle}: Get Closest Vehicle {VehicleTryingToEnter.Vehicle.Handle} Seat {SeatTryingToEnter}", 3);
        //}
        //else
        //{
        //    EntryPoint.WriteToConsole($"GetArrested {Ped.Pedestrian.Handle}: Get Closest Vehicle (None Found) ", 3);
        //}
    }
    private void SetArrestedAnimation(Ped PedToArrest)
    {
        SubTaskName = "SetArrested";
        GameFiber SetArrestedAnimation = GameFiber.StartNew(delegate
        {
            AnimationDictionary.RequestAnimationDictionay("veh@busted_std");
            AnimationDictionary.RequestAnimationDictionay("busted");
            AnimationDictionary.RequestAnimationDictionay("ped");
            if (!PedToArrest.Exists())
            {
                return;
            }
            while (PedToArrest.Exists() && (PedToArrest.IsRagdoll || PedToArrest.IsStunned))
            {
                GameFiber.Yield();
            }
            if (!PedToArrest.Exists())
            {
                return;
            }
            if (PedToArrest.IsInAnyVehicle(false))
            {
                Vehicle oldVehicle = PedToArrest.CurrentVehicle;
                if (PedToArrest.Exists() && oldVehicle.Exists())
                {
                    NativeFunction.CallByName<uint>("TASK_LEAVE_VEHICLE", PedToArrest, oldVehicle, 256);
                    GameFiber.Wait(2500);
                }
            }
            if (!NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_2_hands_up", 3) && !NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_a", 3))
            {
                EntryPoint.WriteToConsole($"TASKER: {Ped.Pedestrian.Handle}                 GetArrested Start Hands Up", 3);
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_2_hands_up", 8.0f, -8.0f, -1, 2, 0, false, false, false);
                GameFiber.Wait(1000);
                if (PedToArrest.Exists())
                {
                    EntryPoint.WriteToConsole($"TASKER: {Ped.Pedestrian.Handle}                 GetArrested Start Drop Gun", 3);
                    NativeFunction.Natives.SET_PED_DROPS_WEAPON(PedToArrest);
                    GameFiber.Wait(5000);
                    if (!PedToArrest.Exists() || (PedToArrest == Game.LocalPlayer.Character && !Player.IsBusted))
                    {
                        return;
                    }
                    EntryPoint.WriteToConsole($"TASKER: {Ped.Pedestrian.Handle}                 GetArrested Start Hands Up Idle", 3);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_a", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                }
            }
            if(PedToArrest.Exists())
            {
                PedToArrest.KeepTasks = true;
            }
            GameTimeFinishedArrestedAnimation = Game.GameTime;
            PlayedArrestAnimation = true;
            EntryPoint.WriteToConsole($"TASKER: GetArrested Played Arrest Animation: {Ped.Pedestrian.Handle}", 3);

        }, "SetArrestedAnimation");
    }
    public void UnSetArrestedAnimation(Ped PedToArrest)
    {
        SubTaskName = "UnSetArrested";
        GameFiber UnSetArrestedAnimationGF = GameFiber.StartNew(delegate
        {
            AnimationDictionary.RequestAnimationDictionay("random@arrests");
            AnimationDictionary.RequestAnimationDictionay("busted");
            AnimationDictionary.RequestAnimationDictionay("ped");
            AnimationDictionary.RequestAnimationDictionay("mp_arresting");
            if (PedToArrest.Exists())
            {
                EntryPoint.WriteToConsole($"TASKER: {Ped.Pedestrian.Handle}                 GetArrested Start UnSet", 3);
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "hands_up_2_idle", 4.0f, -4.0f, -1, 4096, 0, 0, 1, 0);
                GameFiber.Wait(1500);
                if (PedToArrest.Exists())
                {
                    NativeFunction.Natives.CLEAR_PED_TASKS(PedToArrest);
                    EntryPoint.WriteToConsole($"TASKER: {Ped.Pedestrian.Handle}                 GetArrested Start Hands Behind Back", 3);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "mp_arresting", "idle", 1.0f, -1.0f, -1, 49, 0, 0, 1, 0);
                    GameFiber.Wait(2000);
                    PlayedUnArrestAnimation = true;
                }
            }
            EntryPoint.WriteToConsole($"TASKER: GetArrested Played UNArrest Animation: {Ped.Pedestrian.Handle}", 3);
        }, "UnSetArrestedAnimation");
    }
    private void SetUnarmed()
    {
        uint currentWeapon;
        NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(Ped.Pedestrian, out currentWeapon, true);
        if (currentWeapon != 2725352035)
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Ped.Pedestrian, 2725352035, true);
            NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Ped.Pedestrian, false);
        }
    }
}

