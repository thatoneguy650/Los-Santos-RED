using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ApprehendOther : ComplexTask
{
    private bool NeedsUpdates;
    private Task myCurrentTask = Task.Nothing;
    private uint GameTimeClearedIdle;
    private bool IsArresting = true;

    private enum Task
    {
        ApprehendOther,
        Nothing,
    }
    private Task CurrentTaskDynamic
    {
        get
        {
            if (OtherTargets != null && OtherTargets.Any())
            {
                return Task.ApprehendOther;
            }
            else
            {
                return Task.Nothing;
            }
        }
    }
    public ApprehendOther(IComplexTaskable cop, ITargetable player) : base(player, cop, 1500)
    {
        Name = "ApprehendOther";
        SubTaskName = "";
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            EntryPoint.WriteToConsole($"TASKER: ApprehendOther Start: {Ped.Pedestrian.Handle}", 5);
            ClearTasks(false);
            Update();
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
            Ped.Pedestrian.Tasks.Clear();
            Ped.Pedestrian.BlockPermanentEvents = false;
            Ped.Pedestrian.KeepTasks = false;
            Ped.Pedestrian.Tasks.Clear();
            if (resetAlertness)
            {
                NativeFunction.Natives.SET_PED_ALERTNESS(Ped.Pedestrian, 0);
            }
            Ped.Pedestrian.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Player, Relationship.Neutral);
            if (WasInVehicle && !Ped.Pedestrian.IsInAnyVehicle(false) && CurrentVehicle != null)
            {
                Ped.Pedestrian.WarpIntoVehicle(CurrentVehicle, seatIndex);
            }
            //EntryPoint.WriteToConsole(string.Format("     ClearedTasks: {0}", Ped.Pedestrian.Handle));
            IsReadyForWeaponUpdates = false;
        }
    }
    public override void Update()
    {
        if (Ped.Pedestrian.Exists() && ShouldUpdate)
        {
            if (myCurrentTask != CurrentTaskDynamic)
            {
                myCurrentTask = CurrentTaskDynamic;
                //EntryPoint.WriteToConsole($"      Idle SubTask Changed: {Ped.Pedestrian.Handle} to {CurrentTask} {CurrentDynamic}");
                ExecuteCurrentSubTask(true);
            }
            else if (NeedsUpdates)
            {
                ExecuteCurrentSubTask(false);
            }
            SetSiren();
        }
    }
    private void ExecuteCurrentSubTask(bool IsFirstRun)
    {
        if (myCurrentTask == Task.ApprehendOther)
        {
            //SubTaskName = "ApprehendOther";
            ApprehendClosest(IsFirstRun);
        }
        GameTimeLastRan = Game.GameTime;
    }
 
    private void Nothing(bool IsFirstRun)
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

    private void ApprehendClosest(bool IsFirstRun)
    {
        PedExt ClosestPed = OtherTargets.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive).OrderBy(x => x.Pedestrian.DistanceTo2D(Ped.Pedestrian)).OrderByDescending(x => x.WantedLevel).FirstOrDefault();//.OrderBy(x => x.Pedestrian.DistanceTo2D(Ped.Pedestrian)).FirstOrDefault();
        if (IsFirstRun)
        {
            OtherTarget = ClosestPed;
            EntryPoint.WriteToConsole($"COP EVENT: OtherTarget Idle Start: {Ped.Pedestrian.Handle}", 3);
            if (ClosestPed != null && ClosestPed.Pedestrian.Exists())
            {
                EntryPoint.WriteToConsole($"COP EVENT {Ped.Pedestrian.Handle}:                      ApprehendClosest Start Target Handle: {ClosestPed.Pedestrian.Handle}", 3);
            }
            NeedsUpdates = true;
           // RunInterval = 500;// 2000;
            //NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Ped.Pedestrian, true);
            if (IsArresting && OtherTarget != null && OtherTarget.WantedLevel > 2)
            {
                IsArresting = false;
            }
            ClearTasks(false);
            OtherTargetTask();
        }
        if (OtherTarget != null && !OtherTarget.Pedestrian.Exists())
        {
            OtherTarget = null;
        }
        if (ClosestPed == null)
        {
            OtherTarget = null;
        }
        if (ClosestPed != null && OtherTarget == null)
        {
            OtherTarget = ClosestPed;
            EntryPoint.WriteToConsole($"COP EVENT {Ped.Pedestrian.Handle}: ApprehendOther Target Changed to: {OtherTarget.Pedestrian.Handle}", 3);
            ClearTasks(false);
            OtherTargetTask();
        }
        else if (ClosestPed != null && OtherTarget != null && ClosestPed.Pedestrian.Exists() && OtherTarget.Pedestrian.Exists() && ClosestPed.Pedestrian.Handle != OtherTarget.Pedestrian.Handle)
        {
            OtherTarget = ClosestPed;
            EntryPoint.WriteToConsole($"COP EVENT {Ped.Pedestrian.Handle}: ApprehendOther Target Changed to: {OtherTarget.Pedestrian.Handle}", 3);
            ClearTasks(false);
            OtherTargetTask();
        }
        else if(IsArresting && OtherTarget != null && OtherTarget.WantedLevel >= 3)
        {
            IsArresting = false;
            EntryPoint.WriteToConsole($"COP EVENT {Ped.Pedestrian.Handle}: ApprehendClosest Target Mode Changed (Kill) for: {OtherTarget.Pedestrian.Handle}", 3);
            ClearTasks(false);
            OtherTargetTask();
        }
        else if (!IsArresting && OtherTarget != null && OtherTarget.WantedLevel < 3)
        {
            IsArresting = true;
            EntryPoint.WriteToConsole($"COP EVENT {Ped.Pedestrian.Handle}: ApprehendClosest Target Mode Changed (Kill) for: {OtherTarget.Pedestrian.Handle}", 3);
            ClearTasks(false);
            OtherTargetTask();
        }
        //else if(Ped.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.None || Ped.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask)
        //{
        //    if (OtherTarget != null && OtherTarget.Pedestrian.Exists())
        //    {
        //        EntryPoint.WriteToConsole($"COP EVENT {Ped.Pedestrian.Handle}: ApprehendClosest Ped Lost Task for: {OtherTarget.Pedestrian.Handle}", 3);
        //        OtherTargetTask();
        //    }
        //}


        if(OtherTarget != null && OtherTarget.Pedestrian.Exists() && OtherTarget.Pedestrian.IsStunned)
        {
            OtherTarget.Pedestrian.Health = 0;
            OtherTarget.Pedestrian.Kill();//for now simulate arrested?
            EntryPoint.WriteToConsole($"Should kill {OtherTarget.Pedestrian.Handle}", 3);
        }
    }
    private void OtherTargetTask()
    {
        Ped.Pedestrian.BlockPermanentEvents = true;
        Ped.Pedestrian.KeepTasks = true;

        NativeFunction.Natives.SET_PED_SHOOT_RATE(Ped.Pedestrian, 100);//30
        NativeFunction.Natives.SET_PED_ALERTNESS(Ped.Pedestrian, 3);//very altert
        NativeFunction.Natives.SET_PED_COMBAT_ABILITY(Ped.Pedestrian, 2);//professional
        NativeFunction.Natives.SET_PED_COMBAT_RANGE(Ped.Pedestrian, 2);//far
        NativeFunction.Natives.SET_PED_COMBAT_MOVEMENT(Ped.Pedestrian, 2);//offensinve

        if (Ped.Pedestrian.Exists())
        {
            if (OtherTarget != null && OtherTarget.Pedestrian.Exists())
            {
                //Ped.Pedestrian.Tasks.FightAgainst(OtherTarget.Pedestrian, -1);
                IsReadyForWeaponUpdates = true;
                ////EntryPoint.WriteToConsole($"COP EVENT {Ped.Pedestrian.Handle}: ApprehendClosest Applied Task To Chase: {OtherTarget.Pedestrian.Handle}", 3);
                ///
                

                float CurrentDistance = OtherTarget.Pedestrian.DistanceTo2D(Ped.Pedestrian);
                //if (Ped.IsInVehicle)
                //{
                //    if (CurrentDistance >= 15f)
                //    {
                //        if (Ped.IsDriver)
                //        {
                //            NativeFunction.Natives.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE(Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, OtherTarget.Pedestrian.Position.X, OtherTarget.Pedestrian.Position.Y, OtherTarget.Pedestrian.Position.Z, 30f, (int)VehicleDrivingFlags.Emergency, 10f);
                //            EntryPoint.WriteToConsole($"COP EVENT {Ped.Pedestrian.Handle}: ApprehendClosest TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE: {OtherTarget.Pedestrian.Handle}", 3);

                //            SubTaskName = "DrivingTo";

                //        }
                //    }
                //    else
                //    {
                //        unsafe
                //        {
                //            int lol = 0;
                //            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                //            NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", 0, Ped.Pedestrian.CurrentVehicle, 27, 1000);
                //            NativeFunction.CallByName<bool>("TASK_LEAVE_VEHICLE", 0, Ped.Pedestrian.CurrentVehicle, 256);
                //            NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Player.Character, -1, 7f, 500f, 1073741824, 1); //Original and works ok
                //            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                //            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                //            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                //            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                //        }
                //        EntryPoint.WriteToConsole($"COP EVENT {Ped.Pedestrian.Handle}: ApprehendClosest TASK_LEAVE_VEHICLE: {OtherTarget.Pedestrian.Handle}", 3);

                //        SubTaskName = "GettingOutOfCar";
                //    }
                //}
                //else
                if(CurrentDistance <= 15f)
                {
                    if (IsArresting)
                    {
                        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_CanChaseTargetOnFoot, true);
                        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_Aggressive, true);
                        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_CanUseCover, false);
                        if (CurrentDistance >= 12f)
                        {
                            unsafe
                            {
                                int lol = 0;
                                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, OtherTarget.Pedestrian, -1, 7f, 500f, 1073741824, 1); //Original and works ok
                                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY", 0, OtherTarget.Pedestrian, OtherTarget.Pedestrian, 200f, true, 4.0f, 200f, false, false, (uint)FiringPattern.DelayFireByOneSecond);
                                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                            }
                            SubTaskName = "ArrestingFar";
                        }
                        else
                        {
                            unsafe
                            {
                                int lol = 0;
                                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY", 0, OtherTarget.Pedestrian, OtherTarget.Pedestrian, 200f, true, 4.0f, 200f, false, false, (uint)FiringPattern.DelayFireByOneSecond);
                                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                            }
                            SubTaskName = "ArrestingClose";
                        }
                        EntryPoint.WriteToConsole($"COP EVENT {Ped.Pedestrian.Handle}: ApprehendClosest Set Arrest Task Sequence: {OtherTarget.Pedestrian.Handle}", 3);
                        //NativeFunction.Natives.TASK_ARREST_PED(Ped.Pedestrian, OtherTarget.Pedestrian);
                        // Ped.Pedestrian.Tasks.FightAgainst(OtherTarget.Pedestrian, -1);
                    }
                    else
                    {
                        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_CanChaseTargetOnFoot, false);
                        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_Aggressive, true);
                        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_CanUseCover, true);
                        Ped.Pedestrian.Tasks.FightAgainst(OtherTarget.Pedestrian, -1);
                        SubTaskName = "Fighting";
                    }
                }
                EntryPoint.WriteToConsole($"COP EVENT {Ped.Pedestrian.Handle}: ApprehendClosest Ran For: {OtherTarget.Pedestrian.Handle}", 3);
            }
        }
    }


    private void SetSiren()
    {
        if (Ped.Pedestrian.CurrentVehicle.Exists() && Ped.Pedestrian.CurrentVehicle.HasSiren && !Ped.Pedestrian.CurrentVehicle.IsSirenOn)
        {
            Ped.Pedestrian.CurrentVehicle.IsSirenOn = true;
            Ped.Pedestrian.CurrentVehicle.IsSirenSilent = false;
        }
    }
    public override void Stop()
    {

    }
}

