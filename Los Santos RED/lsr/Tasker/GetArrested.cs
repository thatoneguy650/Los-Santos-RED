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
    private uint GameTimeFinishedUnArrestedAnimation;
    private bool PlayedArrestAnimation = false;
    private bool PlayedUnArrestAnimation = false;
    private bool StartedWalking = false;


    private int SeatTryingToEnter;
    private VehicleExt VehicleTryingToEnter;
    private IEntityProvideable World;

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
            else if (!Ped.Pedestrian.IsInAnyVehicle(false))
            {
                if (Ped.DistanceToPlayer <= 75f && VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists() && VehicleTryingToEnter.Vehicle.IsDriveable && VehicleTryingToEnter.Vehicle.FreeSeatsCount > 0) //if (Ped.DistanceToPlayer <= 75f && Ped.Pedestrian.LastVehicle.Exists() && Ped.Pedestrian.LastVehicle.IsDriveable && Ped.Pedestrian.LastVehicle.FreeSeatsCount > 0)
                {
                    return Task.GetInCar;
                }
                else if (CurrentTask == Task.GetInCar)
                {
                    return Task.GetInCar;
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
    public GetArrested(IComplexTaskable ped, ITargetable player, IEntityProvideable world) : base(player, ped, 500)
    {
        Name = "GetArrested";
        SubTaskName = "";
        World = world;
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            EntryPoint.WriteToConsole($"TASKER: GetArrested Start: {Ped.Pedestrian.Handle}", 3);
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            uint currentWeapon;
            NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(Ped.Pedestrian, out currentWeapon, true);
            if (currentWeapon != 2725352035)
            {
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Ped.Pedestrian, 2725352035, true);
                NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Ped.Pedestrian, false);
            }
            GetClosesetPoliceVehicle();
            Update();
        }
        GameTimeLastRan = Game.GameTime;
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
        if (Ped.IsInVehicle && !Ped.IsDriver && Ped.WantedLevel > 0)
        {
            Ped.SetWantedLevel(0);
        }
    }
    private void GetInCar(bool IsFirstRun)
    {
        if (Ped.Pedestrian.Exists())
        {
            if (IsFirstRun)
            {
                EntryPoint.WriteToConsole($"CRIMINAL EVENT: Get in Car Idle Start: {Ped.Pedestrian.Handle}", 3);
                NeedsUpdates = true;
                GetInCarTask();
            }
            else if (Ped.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask)
            {
                GetClosesetPoliceVehicle();
                GetInCarTask();
            }
            if (VehicleTryingToEnter == null)//|| !VehicleTryingToEnter.Vehicle.Exists() || !VehicleTryingToEnter.Vehicle.IsSeatFree(SeatTryingToEnter))
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

            if(Ped.Pedestrian.IsGettingIntoVehicle)
            {
                NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Ped.Pedestrian);
            }
        }
    }
    private void GetInCarTask()
    {
        if (Ped.Pedestrian.Exists() && VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists()) //if (Ped.Pedestrian.Exists() && Ped.Pedestrian.LastVehicle.Exists())
        {
            EntryPoint.WriteToConsole($"TASKER: {Ped.Pedestrian.Handle}                 GetArrested Start GetInCar", 3);
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
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
    public override void Stop()
    {

    }
    private void ReportCrime()
    {
        if (Ped.Pedestrian.Exists() && Ped.Pedestrian.IsAlive && !Ped.Pedestrian.IsRagdoll)
        {
            Crime ToReport = Ped.PlayerCrimesWitnessed.OrderBy(x => x.Priority).FirstOrDefault();
            foreach (Crime toReport in Ped.PlayerCrimesWitnessed)
            {
                Player.AddCrime(ToReport, false, Ped.PositionLastSeenCrime, Ped.VehicleLastSeenPlayerIn, Ped.WeaponLastSeenPlayerWith, Ped.EverSeenPlayer && Ped.ClosestDistanceToPlayer <= 10f, true, true);
            }
            Ped.PlayerCrimesWitnessed.Clear();
        }
    }
    private void GetClosesetPoliceVehicle()
    {
        VehicleTryingToEnter = World.PoliceVehicleList.Where(x => x.Vehicle.Exists() && x.Vehicle.Model.NumberOfSeats >= 4 && x.Vehicle.GetFreeSeatIndex(1,2) != null && x.Vehicle.Speed == 0f).OrderBy(x => x.Vehicle.DistanceTo2D(Ped.Pedestrian)).FirstOrDefault();
        if (VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists())
        {
            int? PossileSeat = VehicleTryingToEnter.Vehicle.GetFreeSeatIndex(1,2);
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

    //private void SetArrestedAnimation(Ped PedToArrest)
    //{
    //    SubTaskName = "SetArrested";
    //    GameFiber SetArrestedAnimation = GameFiber.StartNew(delegate
    //    {
    //        AnimationDictionary.RequestAnimationDictionay("mp_arresting");
    //        if (!PedToArrest.Exists())
    //        {
    //            return;
    //        }
    //        while (PedToArrest.Exists() && (PedToArrest.IsRagdoll || PedToArrest.IsStunned))
    //        {
    //            GameFiber.Yield();
    //        }
    //        if (!PedToArrest.Exists())
    //        {
    //            return;
    //        }
    //        if (PedToArrest.IsInAnyVehicle(false))
    //        {
    //            Vehicle oldVehicle = PedToArrest.CurrentVehicle;
    //            if (PedToArrest.Exists() && oldVehicle.Exists())
    //            {
    //                NativeFunction.CallByName<uint>("TASK_LEAVE_VEHICLE", PedToArrest, oldVehicle, 256);
    //                GameFiber.Wait(2500);
    //            }
    //        }
    //        NativeFunction.Natives.SET_PED_DROPS_WEAPON(PedToArrest);
    //        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "mp_arresting", "idle", 1.0f, -1.0f, -1, 49, 0, 0, 1, 0);
    //        GameTimeFinishedArrestedAnimation = Game.GameTime;
    //        PlayedArrestAnimation = true;
    //        EntryPoint.WriteToConsole($"TASKER: GetArrested Played Arrest Animation: {Ped.Pedestrian.Handle}", 3);

    //    }, "SetArrestedAnimation");
    //}
    //public void UnSetArrestedAnimation(Ped PedToArrest)
    //{
    //    //SubTaskName = "UnSetArrested";
    //    //GameFiber UnSetArrestedAnimationGF = GameFiber.StartNew(delegate
    //    //{
    //    //    AnimationDictionary.RequestAnimationDictionay("mp_arresting");
    //    //    if (PedToArrest.Exists())
    //    //    {
    //    //        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "mp_arresting", "idle", 4.0f, -4.0f, -1, 49, 0, 0, 1, 0);
    //    //        GameFiber.Wait(2000);
    //    //        PlayedUnArrestAnimation = true;
    //    //    }
    //    //    EntryPoint.WriteToConsole($"TASKER: GetArrested Played UNArrest Animation: {Ped.Pedestrian.Handle}", 3);
    //    //}, "UnSetArrestedAnimation");
    //    PlayedUnArrestAnimation = true;
    //}

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
            PedToArrest.KeepTasks = true;
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
}

