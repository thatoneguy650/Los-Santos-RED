//using LosSantosRED.lsr.Interface;
//using LSR.Vehicles;
//using Rage;
//using Rage.Native;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//public class GetArrestedSimple : ComplexTask
//{
//    private uint GameTimeFinishedArrestedAnimation;
//    private bool PlayedArrestAnimation = false;
//    private bool PlayedUnArrestAnimation = false;
//    private uint GameTimeStartedBeingArrested;
//    private int SeatTryingToEnter;
//    private VehicleExt VehicleTryingToEnter;
//    private Vehicle VehicleTaskedToEnter;
//    private int SeatTaskedToEnter;
//    private bool NeedsUpdates;
//    private Task CurrentTask = Task.Wait;
//    private enum Task
//    {
//        GetInCar,
//        SetArrested,
//        UnSetArrested,
//        Wait,
//    }
//    private Task CurrentTaskDynamic
//    {
//        get
//        {
//            if (!PlayedArrestAnimation)
//            {
//                return Task.SetArrested;
//            }
//            else
//            {
//                return Task.Wait;
//            }
//        }
//    }
//    public GetArrestedSimple(IComplexTaskable ped, ITargetable player) : base(player, ped, 500)
//    {
//        Name = "GetArrestedSimple";
//        SubTaskName = "";
//    }
//    public override void Start()
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            //EntryPoint.WriteToConsole($"GetArrested {Ped.Pedestrian.Handle}: START", 3);
//            Ped.Pedestrian.BlockPermanentEvents = true;
//            Ped.Pedestrian.KeepTasks = true;
//            GameTimeStartedBeingArrested = Game.GameTime;
//            SetUnarmed();
//            Update();
//        }
//    }
//    public override void Update()
//    {
//        if (Ped.Pedestrian.Exists() && ShouldUpdate)
//        {
//            if (CurrentTask != CurrentTaskDynamic)
//            {
//                CurrentTask = CurrentTaskDynamic;
//                ExecuteCurrentSubTask(true);
//            }
//            else if (NeedsUpdates)
//            {
//                ExecuteCurrentSubTask(false);
//            }
//        }
//    }
//    public override void Stop()
//    {

//    }
//    public override void ReTask()
//    {

//    }
//    private void ExecuteCurrentSubTask(bool IsFirstRun)
//    {
//        if (CurrentTask == Task.SetArrested)
//        {
//            SubTaskName = "SetArrested";
//            SetArrested(IsFirstRun);
//        }
//        else if (CurrentTask == Task.Wait)
//        {
//            SubTaskName = "Wait";
//            Wait(IsFirstRun);
//        }
//        GameTimeLastRan = Game.GameTime;
//    }
//    private void SetArrested(bool IsFirstRun)
//    {
//        if (IsFirstRun)
//        {
//            SetArrestedAnimation(Ped.Pedestrian);
//        }
//    }
//    private void Wait(bool IsFirstRun)
//    {
//        if (IsFirstRun)
//        {
//            if (Ped.Pedestrian.Exists() && Ped.IsInVehicle && !Ped.IsDriver)
//            {
//                Ped.Pedestrian.CanBePulledOutOfVehicles = false;
//            }
//        }
//        if (Ped.IsInVehicle && !Ped.IsDriver && Ped.IsBusted && !Ped.IsArrested)
//        {
//            Ped.IsArrested = true;
//            //EntryPoint.WriteToConsole($"GetArrested {Ped.Pedestrian.Handle}: GotArrested in Car", 3);
//        }
//    }
//    private void SetArrestedAnimation(Ped PedToArrest)
//    {
//        SubTaskName = "SetArrested";
//        GameFiber SetArrestedAnimation = GameFiber.StartNew(delegate
//        {
//            try
//            {
//                AnimationDictionary.RequestAnimationDictionay("veh@busted_std");
//                AnimationDictionary.RequestAnimationDictionay("busted");
//                AnimationDictionary.RequestAnimationDictionay("ped");
//                if (!PedToArrest.Exists())
//                {
//                    return;
//                }
//                while (PedToArrest.Exists() && (PedToArrest.IsRagdoll || PedToArrest.IsStunned))
//                {
//                    GameFiber.Yield();
//                }
//                if (!PedToArrest.Exists())
//                {
//                    return;
//                }
//                if (PedToArrest.IsInAnyVehicle(false))
//                {
//                    Vehicle oldVehicle = PedToArrest.CurrentVehicle;
//                    if (PedToArrest.Exists() && oldVehicle.Exists())
//                    {
//                        NativeFunction.CallByName<uint>("TASK_LEAVE_VEHICLE", PedToArrest, oldVehicle, 256);
//                        GameFiber.Wait(2500);
//                    }
//                }
//                if (!NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_2_hands_up", 3) && !NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_a", 3))
//                {
//                    //EntryPoint.WriteToConsole($"TASKER: {Ped.Pedestrian.Handle}                 GetArrested Start Hands Up", 3);
//                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_2_hands_up", 8.0f, -8.0f, -1, 2, 0, false, false, false);
//                    GameFiber.Wait(1000);
//                    if (PedToArrest.Exists())
//                    {
//                        //EntryPoint.WriteToConsole($"TASKER: {Ped.Pedestrian.Handle}                 GetArrested Start Drop Gun", 3);
//                        NativeFunction.Natives.SET_PED_DROPS_WEAPON(PedToArrest);
//                        GameFiber.Wait(5000);
//                        if (!PedToArrest.Exists() || (PedToArrest == Game.LocalPlayer.Character && !Player.IsBusted))
//                        {
//                            return;
//                        }
//                        //EntryPoint.WriteToConsole($"TASKER: {Ped.Pedestrian.Handle}                 GetArrested Start Hands Up Idle", 3);
//                        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_a", 8.0f, -8.0f, -1, 1, 0, false, false, false);
//                    }
//                }
//                if (PedToArrest.Exists())
//                {
//                    PedToArrest.KeepTasks = true;
//                    unsafe
//                    {
//                        uint lol = PedToArrest.Handle;
//                        NativeFunction.CallByName<bool>("SET_ENTITY_AS_NO_LONGER_NEEDED", &lol);
//                    }
//                }
//                GameTimeFinishedArrestedAnimation = Game.GameTime;
//                PlayedArrestAnimation = true;






//                //EntryPoint.WriteToConsole($"TASKER: GetArrested Played Arrest Animation: {Ped.Pedestrian.Handle}", 3);
//            }
//            catch (Exception ex)
//            {
//                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
//                EntryPoint.ModController.CrashUnload();
//            }
//        }, "SetArrestedAnimation");
//    }
//    private void SetUnarmed()
//    {
//        uint currentWeapon;
//        NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(Ped.Pedestrian, out currentWeapon, true);
//        if (currentWeapon != 2725352035)
//        {
//            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Ped.Pedestrian, 2725352035, true);
//            NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Ped.Pedestrian, false);
//        }
//    }
//}

