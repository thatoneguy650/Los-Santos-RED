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
    private SeatAssigner SeatAssigner;
    private int SeatTryingToEnter;
    private VehicleExt VehicleTryingToEnter;
    private Vehicle VehicleTaskedToEnter;
    private int SeatTaskedToEnter;
    private bool NeedsUpdates;
    private Task CurrentTask = Task.Wait;



    private bool ShouldStayStanding => Player.HasBustPowers && Ped.DistanceToPlayer <= 50f;
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
            if(!PlayedArrestAnimation || ShouldStayStanding)
            {
                return Task.SetArrested;
            }
            else if (!PlayedUnArrestAnimation && !Ped.IsInVehicle)
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
    public GetArrested(IComplexTaskable ped, ITargetable player, IEntityProvideable world) : base(player, ped, 500)
    {
        Name = "GetArrested";
        SubTaskName = "";
        World = world;
        SeatAssigner = new SeatAssigner(Ped,World, World.Vehicles.SimplePoliceVehicles);
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
           //EntryPoint.WriteToConsole($"GetArrested {Ped.Pedestrian.Handle}: START", 3);
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            Ped.Pedestrian.IsPersistent = true;
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
            if (Ped.Pedestrian.IsPersistent && Ped.DistanceToPlayer >= 50f)
            {
                Ped.Pedestrian.IsPersistent = false;
                unsafe
                {
                    uint lol = Ped.Pedestrian.Handle;
                    NativeFunction.CallByName<bool>("SET_ENTITY_AS_NO_LONGER_NEEDED", &lol);
                }
            }
        }
    }
    public override void Stop()
    {

    }
    public override void ReTask()
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
            if(Ped.WantedLevel >= 3)
            {
                SetArrestedAnimationKneeling(Ped.Pedestrian);
            }
            else
            {
                SetArrestedAnimation(Ped.Pedestrian);
            }
        }
    }
    private void UnSetArrested(bool IsFirstRun)
    {
        if(IsFirstRun)
        {
            NeedsUpdates = true;
        }
        GetClosesetPoliceVehicle();
        if(VehicleTryingToEnter != null && !PlayedUnArrestAnimation)
        {
            NeedsUpdates = false;
            if(Ped.WantedLevel >= 3)
            {
                UnSetArrestedAnimationKneeling(Ped.Pedestrian);
            }
            else
            {
                UnSetArrestedAnimation(Ped.Pedestrian);
            }        
        }
    }
    private void Wait(bool IsFirstRun)
    {
        if(IsFirstRun)
        {
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            if (Ped.Pedestrian.Exists() && Ped.IsInVehicle && !Ped.IsDriver)
            {
                Ped.Pedestrian.CanBePulledOutOfVehicles = false;
            }
        }
        if (Ped.IsInVehicle && !Ped.IsDriver && Ped.IsBusted && !Ped.IsArrested)
        {
            Ped.IsArrested = true;
        }
    }
    private void GetInCar(bool IsFirstRun)
    {
        if (Ped.Pedestrian.Exists())
        {
            if (IsFirstRun)
            {

                //EntryPoint.WriteToConsole($"GetArrested {Ped.Pedestrian.Handle}: Get in Car Start", 3);
                NeedsUpdates = true;
            }
            if(VehicleTaskedToEnter == null || !VehicleTaskedToEnter.Exists())
            {
                GetClosesetPoliceVehicle();
                //EntryPoint.WriteToConsole($"GetArrested {Ped.Pedestrian.Handle}: Get in Car, Got New Car, was Blank", 3);
                GetInCarTask();
            }
            else if (VehicleTryingToEnter != null && VehicleTaskedToEnter.Exists() && !VehicleTaskedToEnter.IsSeatFree(SeatTaskedToEnter) && VehicleTaskedToEnter.GetPedOnSeat(SeatTaskedToEnter).Exists() && VehicleTaskedToEnter.GetPedOnSeat(SeatTaskedToEnter).Handle != Ped.Pedestrian.Handle)// && (VehicleTryingToEnter.Vehicle.Handle != VehicleTaskedToEnter.Handle || SeatTaskedToEnter != SeatTryingToEnter) && Ped.Pedestrian.Exists() && !Ped.Pedestrian.IsInAnyVehicle(true))
            {
                GetClosesetPoliceVehicle();
                //EntryPoint.WriteToConsole($"GetArrested {Ped.Pedestrian.Handle}: Get in Car Got New Car, was occupied?", 3);
                GetInCarTask();
            }
            else if (VehicleTryingToEnter != null && VehicleTaskedToEnter.Exists() && VehicleTaskedToEnter.Speed > 1.0f)// && (VehicleTryingToEnter.Vehicle.Handle != VehicleTaskedToEnter.Handle || SeatTaskedToEnter != SeatTryingToEnter) && Ped.Pedestrian.Exists() && !Ped.Pedestrian.IsInAnyVehicle(true))
            {
                GetClosesetPoliceVehicle();
               // EntryPoint.WriteToConsole($"GetArrested {Ped.Pedestrian.Handle}: Get in Car Got New Car, was driving away?", 3);
                GetInCarTask();
            }
            else if (Ped.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.None || Ped.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask)//might be a error?
            {
                Ped.Pedestrian.BlockPermanentEvents = true;
                Ped.Pedestrian.KeepTasks = true;
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
        else if (Ped.Pedestrian.Exists())
        {
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_STAND_STILL", 0, -1);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
    }
    private void GetClosesetPoliceVehicle()
    {

        SeatAssigner.AssignPrisonerSeat();
        VehicleTryingToEnter = SeatAssigner.VehicleAssigned;
        SeatTryingToEnter = SeatAssigner.SeatAssigned;
    }
    private void SetArrestedAnimation(Ped PedToArrest)
    {
        SubTaskName = "SetArrested";
        GameFiber SetArrestedAnimation = GameFiber.StartNew(delegate
        {
            try
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
                        if (PedToArrest.Exists() && PedToArrest.IsInAnyVehicle(false))//try it again i guess
                        {
                            oldVehicle = PedToArrest.CurrentVehicle;
                            if (PedToArrest.Exists() && oldVehicle.Exists())
                            {
                                NativeFunction.CallByName<uint>("TASK_LEAVE_VEHICLE", PedToArrest, oldVehicle, 256);
                                GameFiber.Wait(2500);
                            }
                        }
                    }
                }
                //
                if (!NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_2_hands_up", 3) && !NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_a", 3) && !NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "ped", "handsup_enter", 3) && !NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "ped", "handsup", 3) && !NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "ped", "handsup_base", 3))
                {
                    // EntryPoint.WriteToConsole($"TASKER: {Ped.Pedestrian.Handle}                 GetArrested Start Hands Up", 3);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);//2
                    GameFiber.Wait(1000);
                    if (PedToArrest.Exists())
                    {
                        //EntryPoint.WriteToConsole($"TASKER: {Ped.Pedestrian.Handle}                 GetArrested Start Drop Gun", 3);
                        NativeFunction.Natives.SET_PED_DROPS_WEAPON(PedToArrest);
                        GameFiber.Wait(2000);
                        if (!PedToArrest.Exists() || (PedToArrest.Handle == Game.LocalPlayer.Character.Handle && !Player.IsBusted))
                        {
                            return;
                        }
                        //EntryPoint.WriteToConsole($"TASKER: {Ped.Pedestrian.Handle}                 GetArrested Start Hands Up Idle", 3);
                        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "ped", "handsup_base", 2.0f, -2.0f, -1, 1, 0, false, false, false);//1
                    }
                }
                if (PedToArrest.Exists())
                {
                    PedToArrest.BlockPermanentEvents = true;
                    PedToArrest.KeepTasks = true;
                }
                GameTimeFinishedArrestedAnimation = Game.GameTime;
                PlayedArrestAnimation = true;
                //EntryPoint.WriteToConsole($"TASKER: GetArrested Played Arrest Animation: {Ped.Pedestrian.Handle}", 3);
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "SetArrestedAnimation");
    }
    private void SetArrestedAnimationKneeling(Ped PedToArrest)
    {
        SubTaskName = "SetArrested";
        GameFiber SetArrestedAnimation = GameFiber.StartNew(delegate
        {
            try
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
                        if (PedToArrest.Exists() && PedToArrest.IsInAnyVehicle(false))
                        {
                            oldVehicle = PedToArrest.CurrentVehicle;
                            if (PedToArrest.Exists() && oldVehicle.Exists())
                            {
                                NativeFunction.CallByName<uint>("TASK_LEAVE_VEHICLE", PedToArrest, oldVehicle, 256);
                                GameFiber.Wait(2500);
                            }
                        }
                    }
                }
                if (!NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_2_hands_up", 3) && !NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_a", 3))
                {
                    // EntryPoint.WriteToConsole($"TASKER: {Ped.Pedestrian.Handle}                 GetArrested Start Hands Up", 3);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_2_hands_up", 8.0f, -8.0f, -1, 2, 0, false, false, false);
                    GameFiber.Wait(1000);
                    if (PedToArrest.Exists())
                    {
                        //EntryPoint.WriteToConsole($"TASKER: {Ped.Pedestrian.Handle}                 GetArrested Start Drop Gun", 3);
                        NativeFunction.Natives.SET_PED_DROPS_WEAPON(PedToArrest);
                        GameFiber.Wait(5000);
                        if (!PedToArrest.Exists() || (PedToArrest.Handle == Game.LocalPlayer.Character.Handle && !Player.IsBusted))
                        {
                            return;
                        }
                        //EntryPoint.WriteToConsole($"TASKER: {Ped.Pedestrian.Handle}                 GetArrested Start Hands Up Idle", 3);
                        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_a", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                    }
                }
                if (PedToArrest.Exists())
                {
                    PedToArrest.BlockPermanentEvents = true;
                    PedToArrest.KeepTasks = true;
                }
                GameTimeFinishedArrestedAnimation = Game.GameTime;
                PlayedArrestAnimation = true;
               //EntryPoint.WriteToConsole($"TASKER: GetArrested Played Arrest Animation: {Ped.Pedestrian.Handle}", 3);
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "SetArrestedAnimation");
    }
    public void UnSetArrestedAnimation(Ped PedToArrest)
    {
        SubTaskName = "UnSetArrested";
        GameFiber UnSetArrestedAnimationGF = GameFiber.StartNew(delegate
        {
            try
            {
                //AnimationDictionary.RequestAnimationDictionay("random@arrests");
                //AnimationDictionary.RequestAnimationDictionay("busted");
                AnimationDictionary.RequestAnimationDictionay("ped");
                AnimationDictionary.RequestAnimationDictionay("mp_arresting");
                if (PedToArrest.Exists())
                {
                    //if (NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "ped", "handsup_base", 3))
                    //{
                    //    PlayedUnArrestAnimation = true;
                    //}
                    //else
                    //{
                    //EntryPoint.WriteToConsole($"TASKER: {Ped.Pedestrian.Handle}                 GetArrested Start UnSet", 3);
                    // NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "hands_up_2_idle", 4.0f, -4.0f, -1, 4096, 0, 0, 1, 0);
                    GameFiber.Wait(1500);
                    if (PedToArrest.Exists())
                    {
                        NativeFunction.Natives.CLEAR_PED_TASKS(PedToArrest);
                        //EntryPoint.WriteToConsole($"TASKER: {Ped.Pedestrian.Handle}                 GetArrested Start Hands Behind Back", 3);
                        //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "mp_arresting", "idle", 1.0f, -1.0f, -1, 49, 0, 0, 1, 0);
                        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "ped", "handsup_base", 2.0f, -2.0f, -1, 49, 0, false, false, false);
                        GameFiber.Wait(2000);
                        PlayedUnArrestAnimation = true;
                    }
                    //   }
                }
                if (PedToArrest.Exists())
                {
                    PedToArrest.BlockPermanentEvents = true;
                }
                //EntryPoint.WriteToConsole($"TASKER: GetArrested Played UNArrest Animation: {Ped.Pedestrian.Handle}", 3);
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "UnSetArrestedAnimation");
    }
    public void UnSetArrestedAnimationKneeling(Ped PedToArrest)
    {
        SubTaskName = "UnSetArrested";
        GameFiber UnSetArrestedAnimationGF = GameFiber.StartNew(delegate
        {
            try
            {
                AnimationDictionary.RequestAnimationDictionay("random@arrests");
                AnimationDictionary.RequestAnimationDictionay("busted");
                AnimationDictionary.RequestAnimationDictionay("ped");
                AnimationDictionary.RequestAnimationDictionay("mp_arresting");
                if (PedToArrest.Exists())
                {
                    //EntryPoint.WriteToConsole($"TASKER: {Ped.Pedestrian.Handle}                 GetArrested Start UnSet", 3);
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "hands_up_2_idle", 4.0f, -4.0f, -1, 4096, 0, 0, 1, 0);
                    GameFiber.Wait(1500);
                    if (PedToArrest.Exists())
                    {
                        NativeFunction.Natives.CLEAR_PED_TASKS(PedToArrest);
                        //EntryPoint.WriteToConsole($"TASKER: {Ped.Pedestrian.Handle}                 GetArrested Start Hands Behind Back", 3);
                        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "ped", "handsup_base", 2.0f, -2.0f, -1, 49, 0, false, false, false);
                        //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "mp_arresting", "idle", 1.0f, -1.0f, -1, 49, 0, 0, 1, 0);
                        GameFiber.Wait(2000);
                        PlayedUnArrestAnimation = true;
                    }
                }
                if (PedToArrest.Exists())
                {
                    PedToArrest.BlockPermanentEvents = true;
                }
                //EntryPoint.WriteToConsole($"TASKER: GetArrested Played UNArrest Animation: {Ped.Pedestrian.Handle}", 3);
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
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

