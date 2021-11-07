﻿using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Helper;


public class AIApprehend : ComplexTask
{
    private bool NeedsUpdates;
    private SubTask CurrentSubTask;
    private Task CurrentTask = Task.Nothing;
    private Vehicle CopsVehicle;
    private bool IsGoingFast;
    private bool IsStuck;
    private Vector3 LastPosition;
    private bool IsFirstRun;
    private uint GameTimeGotStuck;
    private uint GameTimeVehicleStoppedMoving;
    private uint GameTimeChaseStarted;
    private float ChaseDistance = 5f;
    private int VehicleMissionFlag;
    private bool IsChasingRecklessly;
    private uint CurrentlyChasingHandle;


    private float DistanceToTarget = 999f;
    private bool IsArresting = false;
    private float MoveRate = 2.0f;
    private uint GameTimeTargetLastMovedFast;
    private uint GameTimeLastBustedTarget;
    private uint GameTimeLastReleasedTarget;

    private bool IsTargetMovingFast => GameTimeTargetLastMovedFast != 0 && Game.GameTime - GameTimeTargetLastMovedFast <= 2000;
    private enum Task
    {
        VehicleChase,
        VehicleChasePed,
        ExitVehicle,
        EnterVehicle,
        CarJack,
        FootChase,
        Nothing,
    }
    private enum eVehicleMissionType
    {
        Cruise = 1,
        Ram = 2,
        Block = 3,
        GoTo = 4,
        Stop = 5,
        Attack = 6,
        Follow = 7,
        Flee = 8,
        Circle = 9,
        Escort = 12,
        FollowRecording = 15,
        PoliceBehaviour = 16,
        Land = 19,
        Land2 = 20,
        Crash = 21,
        PullOver = 22,
        HeliProtect = 23
    };
    private enum SubTask
    {
        Shoot,
        Aim,
        Goto,
        None,
    }
    private bool ShouldChaseRecklessly => OtherTarget.IsDeadlyChase;
    private bool ShouldChaseVehicleInVehicle => Ped.IsDriver && Ped.Pedestrian.CurrentVehicle.Exists() && !ShouldExitPoliceVehicle && OtherTarget.IsInVehicle;
    private bool ShouldChasePedInVehicle => DistanceToTarget >= 35f;//25f
    private bool ShouldGetBackInCar => CopsVehicle.Exists() && Ped.Pedestrian.DistanceTo2D(CopsVehicle) <= 30f && CopsVehicle.IsDriveable && CopsVehicle.FreeSeatsCount > 0;
    private bool ShouldCarJackPlayer => Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() && !IsTargetMovingFast;// && !Cop.Pedestrian.IsGettingIntoVehicle;
    private bool ShouldExitPoliceVehicle => DistanceToTarget < 35f && Ped.Pedestrian.CurrentVehicle.Exists() && VehicleIsStopped && !IsTargetMovingFast && !ChaseRecentlyStarted && !Ped.IsInHelicopter && !Ped.IsInBoat;//25f
    private bool ChaseRecentlyStarted => GameTimeChaseStarted != 0 && Game.GameTime - GameTimeChaseStarted <= 3000;
    private bool VehicleIsStopped => GameTimeVehicleStoppedMoving != 0 && Game.GameTime - GameTimeVehicleStoppedMoving >= 500;//20000
    private bool ShouldShoot => !OtherTarget.IsBusted && OtherTarget.WantedLevel > 1;
    private bool ShouldAim => OtherTarget.WantedLevel > 1;
    public AIDynamic CurrentAIDynamic
    {
        get
        {
            if (OtherTarget.IsInVehicle)
            {
                if (Ped.IsInVehicle)
                {
                    return AIDynamic.Cop_InVehicle_Player_InVehicle;
                }
                else
                {
                    return AIDynamic.Cop_OnFoot_Player_InVehicle;
                }
            }
            else
            {
                if (Ped.IsInVehicle)
                {
                    return AIDynamic.Cop_InVehicle_Player_OnFoot;
                }
                else
                {
                    return AIDynamic.Cop_OnFoot_Player_OnFoot;
                }
            }
        }
    }
    private Task GetCurrentTaskDynamic()
    {
        if (CurrentAIDynamic == AIDynamic.Cop_InVehicle_Player_InVehicle)
        {
            if (ShouldExitPoliceVehicle)
            {
                return Task.ExitVehicle;
            }
            else if (ShouldChaseVehicleInVehicle)
            {
                return Task.VehicleChase;
            }
            else
            {
                return Task.Nothing;
            }
        }
        else if (CurrentAIDynamic == AIDynamic.Cop_InVehicle_Player_OnFoot)
        {
            if (Ped.IsDriver)
            {
                if (ShouldChasePedInVehicle)
                {
                    return Task.VehicleChasePed;
                }
                else if (ShouldExitPoliceVehicle)
                {
                    return Task.ExitVehicle;
                }
                else
                {
                    return Task.Nothing;
                }
            }
            else
            {
                if (ShouldExitPoliceVehicle)
                {
                    return Task.ExitVehicle;
                }
                else
                {
                    return Task.Nothing;
                }
            }
        }
        else if (CurrentAIDynamic == AIDynamic.Cop_OnFoot_Player_InVehicle)
        {
            if (ShouldCarJackPlayer)
            {
                return Task.CarJack;
            }
            else if (ShouldGetBackInCar)
            {
                return Task.EnterVehicle;
            }
            else
            {
                return Task.Nothing;
            }
        }
        else if (CurrentAIDynamic == AIDynamic.Cop_OnFoot_Player_OnFoot)
        {
            return Task.FootChase;
        }
        else
        {
            return Task.Nothing;
        }
    }
    public AIApprehend(IComplexTaskable cop, ITargetable player) : base(player, cop, 500)//was 500
    {
        Name = "AIApprehend";
        SubTaskName = "";
    }
    public AIApprehend(IComplexTaskable cop, ITargetable player, float chaseDistance, int vehicleMissionFlag) : base(player, cop, 500)//was 500
    {
        Name = "AIApprehend";
        SubTaskName = "";
        ChaseDistance = chaseDistance;
        VehicleMissionFlag = vehicleMissionFlag;
    }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            EntryPoint.WriteToConsole($"TASKER: Chase Start: {Ped.Pedestrian.Handle} ChaseDistance: {ChaseDistance} VehicleMissionFlag: {VehicleMissionFlag}", 5);
            GameTimeChaseStarted = Game.GameTime;
            NativeFunction.Natives.SET_PED_PATH_CAN_USE_CLIMBOVERS(Ped.Pedestrian, true);
            NativeFunction.Natives.SET_PED_PATH_CAN_USE_LADDERS(Ped.Pedestrian, true);
            NativeFunction.Natives.SET_PED_PATH_CAN_DROP_FROM_HEIGHT(Ped.Pedestrian, true);
            Update();
        }
    }
    public override void Update()
    {
        if (Ped.Pedestrian.Exists() && ShouldUpdate)
        {
            //PedExt ClosestPed = OtherTargets.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive).OrderBy(x => x.IsBusted).ThenByDescending(x => x.WantedLevel).ThenBy(x => x.Pedestrian.DistanceTo2D(Ped.Pedestrian)).FirstOrDefault();////.OrderBy(x => x.Pedestrian.DistanceTo2D(Ped.Pedestrian)).OrderByDescending(x => x.WantedLevel).FirstOrDefault();
            bool TargetChanged = false;
            if(CurrentlyChasingHandle != 0)
            {
                if(OtherTarget != null && OtherTarget.Pedestrian.Exists() && OtherTarget.Handle != CurrentlyChasingHandle)
                {
                    CurrentlyChasingHandle = OtherTarget.Handle;
                    TargetChanged = true;
                }
            }






            if (OtherTarget != null && OtherTarget.Pedestrian.Exists())
            {









                DistanceToTarget = OtherTarget.Pedestrian.DistanceTo2D(Ped.Pedestrian);

                if (OtherTarget.IsInVehicle && OtherTarget.Pedestrian.CurrentVehicle.Exists())
                {
                    if (OtherTarget.Pedestrian.CurrentVehicle.Speed >= 2.0f)
                    {
                        GameTimeTargetLastMovedFast = Game.GameTime;
                    }
                    else
                    {
                        GameTimeTargetLastMovedFast = 0;
                    }
                }
                else
                {
                    if (OtherTarget.Pedestrian.Speed >= 7.0f)
                    {
                        GameTimeTargetLastMovedFast = Game.GameTime;
                    }
                    else
                    {
                        GameTimeTargetLastMovedFast = 0;
                    }

                }













                if (Ped.Pedestrian.IsInAnyPoliceVehicle && !CopsVehicle.Exists())
                {
                    CopsVehicle = Ped.Pedestrian.CurrentVehicle;
                }
                Task UpdatedTask = GetCurrentTaskDynamic();
                if (CurrentTask != UpdatedTask || TargetChanged)
                {
                    IsFirstRun = true;
                    CurrentTask = UpdatedTask;
                    //EntryPoint.WriteToConsole($"TASKER: Chase SubTask Changed: {Ped.Pedestrian.Handle} to {CurrentTask} {CurrentDynamic}");
                    ExecuteCurrentSubTask();
                }
                else if (NeedsUpdates)
                {
                    ExecuteCurrentSubTask();
                }
                if (Ped.IsInVehicle)//CurrentTask == Task.VehicleChase || CurrentTask == Task.VehicleChasePed || Cu)
                {
                    NativeFunction.Natives.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG(Ped.Pedestrian, (int)eChaseBehaviorFlag.NoContact, true);
                    SetSiren();
                    if (Ped.Pedestrian.CurrentVehicle.Exists())
                    {
                        if (Ped.Pedestrian.CurrentVehicle.Speed == 0f)
                        {
                            if (GameTimeVehicleStoppedMoving == 0)
                            {
                                GameTimeVehicleStoppedMoving = Game.GameTime;
                            }

                        }
                        else
                        {
                            GameTimeVehicleStoppedMoving = 0;
                        }
                    }
                }



                //if (OtherTarget.Pedestrian.IsStunned)
                //{
                //    //NativeFunction.Natives.CLEAR_PED_TASKS(OtherTarget.Pedestrian);
                //    SetArrestedAnimation(OtherTarget.Pedestrian, false, false);
                //    //OtherTarget.SetWantedLevel(0);
                //    //OtherTarget.Pedestrian.Health = 0;
                //    //OtherTarget.Pedestrian.Kill();//for now simulate arrested?
                //    EntryPoint.WriteToConsole($"Should kill {OtherTarget.Pedestrian.Handle}", 3);
                //}



                GameTimeLastRan = Game.GameTime;
            }




        }
        //EntryPoint.WriteToConsole($"TASKER: Chase UpdateEnd: {Ped.Pedestrian.Handle} InVeh: {Ped.IsInVehicle} InVeh2: {Ped.Pedestrian.IsInAnyVehicle(false)}");
    }
    private void ExecuteCurrentSubTask()
    {
        if (CurrentTask == Task.CarJack)
        {
            RunInterval = 200;
            SubTaskName = "CarJack";
            GoToPlayersCar();
        }
        else if (CurrentTask == Task.EnterVehicle)
        {
            RunInterval = 200;
            SubTaskName = "EnterVehicle";
            EnterVehicle();
        }
        else if (CurrentTask == Task.ExitVehicle)
        {
            RunInterval = 200;
            SubTaskName = "ExitVehicle";
            ExitVehicle();
        }
        else if (CurrentTask == Task.FootChase)
        {
            RunInterval = 200;
            SubTaskName = "FootChase";
            FootChase();
        }
        else if (CurrentTask == Task.VehicleChase)
        {
            RunInterval = 500;
            SubTaskName = "VehicleChase";
            VehicleChase();
        }
        else if (CurrentTask == Task.VehicleChasePed)
        {
            RunInterval = 500;
            SubTaskName = "VehicleChasePed";
            VehicleChasePed();
        }
        else if (CurrentTask == Task.Nothing)
        {
            RunInterval = 500;
            SubTaskName = "Nothing";
            VehicleChasePed();
        }
        GameTimeLastRan = Game.GameTime;
    }
    private void GoToPlayersCar()
    {
        NeedsUpdates = false;
        if (OtherTarget.IsInVehicle && OtherTarget.Pedestrian.CurrentVehicle.Exists())
        {
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, OtherTarget.Pedestrian.CurrentVehicle, -1, 7f, 500f, 1073741824, 1); //Original and works ok
                                                                                                                                        // NativeFunction.CallByName<bool>("TASK_OPEN_VEHICLE_DOOR", 0, Player.CurrentVehicle.Vehicle, -1, -1, 7f); //doesnt really work
                                                                                                                                        //NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, Player.CurrentVehicle.Vehicle, -1, Target.Pedestrian.SeatIndex, 5.0f, 9);//caused them to get confused about getting back in thier car
                NativeFunction.CallByName<bool>("TASK_ARREST_PED", 0, OtherTarget.Pedestrian);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
    }
    private void EnterVehicle()
    {
        NeedsUpdates = false;
        if (CopsVehicle.Exists())
        {
            NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", Ped.Pedestrian, CopsVehicle, -1, Ped.LastSeatIndex, 2.0f, 9);
        }
        //EntryPoint.WriteToConsole(string.Format("Started Enter Old Car: {0}", Ped.Pedestrian.Handle));
    }
    private void ExitVehicle()
    {
        NeedsUpdates = false;
        if (Ped.Pedestrian.CurrentVehicle.Exists())
        {
                unsafe
                {
                    int lol = 0;
                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                    NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", 0, Ped.Pedestrian.CurrentVehicle, 27, 1000);
                    NativeFunction.CallByName<bool>("TASK_LEAVE_VEHICLE", 0, Ped.Pedestrian.CurrentVehicle, 256);
                    NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, OtherTarget.Pedestrian, -1, 7f, 500f, 1073741824, 1); //Original and works ok
                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                }
            //EntryPoint.WriteToConsole(string.Format("Started Exit Car: {0}", Ped.Pedestrian.Handle));
        }
    }
    private void FootChase()
    {
        if (IsFirstRun)
        {
            IsFirstRun = false;
            NeedsUpdates = true;
            EntryPoint.WriteToConsole($"COP EVENT: OtherTarget Idle Start: {Ped.Pedestrian.Handle}", 3);
            NeedsUpdates = true;
            if (IsArresting && OtherTarget != null && OtherTarget.IsDeadlyChase)
            {
                IsArresting = false;
            }
            OtherTargetTask();
        }
        if (IsArresting && OtherTarget != null && OtherTarget.IsDeadlyChase)
        {
            IsArresting = false;
        }
        else if (!IsArresting && OtherTarget != null && !OtherTarget.IsDeadlyChase)
        {
            IsArresting = true;
        }
        if (Ped.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.None || Ped.Pedestrian.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask)
        {
            SubTaskName = "";
        }
        if (OtherTarget != null && OtherTarget.Pedestrian.Exists())
        {
            OtherTargetTask();
        }
        if (OtherTarget != null && OtherTarget.Pedestrian.Exists() && OtherTarget.Pedestrian.IsStunned && !OtherTarget.IsBusted)
        {
            //SetArrestedAnimation(OtherTarget.Pedestrian, false, false);
            OtherTarget.IsBusted = true;
            OtherTarget.IsArrested = true;
            //GameTimeLastBustedTarget = Game.GameTime;
            EntryPoint.WriteToConsole($"Should bust {OtherTarget.Pedestrian.Handle}", 3);
        }
        //if(OtherTarget != null && OtherTarget.Pedestrian.Exists() && OtherTarget.IsBusted && GameTimeLastBustedTarget > 0 && Game.GameTime - GameTimeLastBustedTarget >= 10000)
        //{
        //    UnSetArrestedAnimation(OtherTarget.Pedestrian);
        //    GameTimeLastReleasedTarget = Game.GameTime;
        //}
        //if(OtherTarget != null && OtherTarget.Pedestrian.Exists() && OtherTarget.IsBusted && GameTimeLastReleasedTarget > 0 && Game.GameTime - GameTimeLastReleasedTarget >= 5000)
        //{
        //    Vector3 Pos = OtherTarget.Pedestrian.Position;
        //    NativeFunction.Natives.TASK_WANDER_IN_AREA(OtherTarget.Pedestrian, Pos.X, Pos.Y, Pos.Z, 45f, 0f, 0f);
        //    GameTimeLastReleasedTarget = 0;
        //}
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
            if (base.OtherTarget != null && base.OtherTarget.Pedestrian.Exists())
            {
                NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE<uint>(Ped.Pedestrian, MoveRate);
                IsReadyForWeaponUpdates = true;
                if (IsArresting)
                {
                    if (base.OtherTarget.IsInVehicle)
                    {
                        if (SubTaskName != "ArrestingInVehicle")
                        {
                            unsafe
                            {
                                int lol = 0;
                                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                NativeFunction.CallByName<bool>("TASK_ARREST_PED", 0, base.OtherTarget.Pedestrian);
                                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                            }
                            SubTaskName = "ArrestingInVehicle";
                        }
                    }
                    else
                    {
                        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_CanChaseTargetOnFoot, true);
                        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_Aggressive, true);
                        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_CanUseCover, false);

                        //if (DistanceToTarget <= 7f && OtherTarget.Pedestrian.IsStunned)
                        //{
                        //    if (SubTaskName != "ArrestingStunned")
                        //    {
                        //        unsafe
                        //        {
                        //            int lol = 0;
                        //            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        //            NativeFunction.CallByName<bool>("TASK_ARREST_PED", 0, base.OtherTarget.Pedestrian);
                        //            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                        //            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        //            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                        //            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                        //        }
                        //        SubTaskName = "ArrestingStunned";
                        //    }
                        //}
                        //else 
                        
                        if (DistanceToTarget >= 10f)
                        {
                            if (SubTaskName != "ArrestingFar")
                            {
                                unsafe
                                {
                                    int lol = 0;
                                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                    NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, base.OtherTarget.Pedestrian, -1, 5f, 2.0f, 1073741824, 1); //Original and works ok//7f
                                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                                }
                                SubTaskName = "ArrestingFar";
                            }
                        }
                        else
                        {
                            if(OtherTarget.IsBusted)
                            {
                                if (SubTaskName != "ArrestingCloseBusted")
                                {
                                    unsafe
                                    {
                                        int lol = 0;
                                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                        NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", 0, OtherTarget.Pedestrian, 4f, 20f);
                                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                                    }
                                    SubTaskName = "ArrestingCloseBusted";
                                }
                            }
                            else
                            {
                                if (SubTaskName != "ArrestingClose")
                                {
                                    unsafe
                                    {
                                        int lol = 0;
                                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                        NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY", 0, base.OtherTarget.Pedestrian, base.OtherTarget.Pedestrian, 200f, true, 4.0f, 200f, false, false, (uint)FiringPattern.DelayFireByOneSecond);
                                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                                    }
                                    SubTaskName = "ArrestingClose";
                                }
                            }

                        }
                    }
                    //EntryPoint.WriteToConsole($"COP EVENT {Ped.Pedestrian.Handle}: ApprehendClosest Set Arrest Task Sequence: {base.OtherTarget.Pedestrian.Handle}", 3);
                    //NativeFunction.Natives.TASK_ARREST_PED(Ped.Pedestrian, OtherTarget.Pedestrian);
                    // Ped.Pedestrian.Tasks.FightAgainst(OtherTarget.Pedestrian, -1);
                }
                else
                {
                    if (SubTaskName != "Fighting")
                    {
                        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Ped.Pedestrian, true);
                        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_CanChaseTargetOnFoot, false);
                        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_Aggressive, true);
                        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Ped.Pedestrian, (int)eCombatAttributes.BF_CanUseCover, true);
                        //Ped.Pedestrian.Tasks.FightAgainst(OtherTarget.Pedestrian, -1);
                        NativeFunction.Natives.TASK_COMBAT_PED(Ped.Pedestrian, base.OtherTarget.Pedestrian, 0, 16);

                        SubTaskName = "Fighting";
                    }
                }
            }
        }
    }
    private void VehicleChase()
    {
        NeedsUpdates = true;
        if (IsFirstRun)
        {
            NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, 100f);
            Ped.Pedestrian.BlockPermanentEvents = true;
            Ped.Pedestrian.KeepTasks = true;
            int DesiredStyle = 0;
            if (Ped.IsInHelicopter)
            {
                NativeFunction.Natives.TASK_HELI_CHASE(Ped.Pedestrian, OtherTarget.Pedestrian, -50f, 50f, 60f);
            }
            else if (Ped.IsInBoat)
            {
                NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, OtherTarget.Pedestrian);
            }
            else
            {
                if (ShouldChaseRecklessly)
                {
                    IsChasingRecklessly = true;
                    unsafe
                    {
                        int lol = 0;
                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION", 0, Ped.Pedestrian.CurrentVehicle, Player.CurrentVehicle.Vehicle, (int)eVehicleMissionType.Ram, 50f, DesiredStyle, ChaseDistance, 0f, true);//8f
                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    }
                }
                else
                {
                    IsChasingRecklessly = false;
                    NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, OtherTarget.Pedestrian);
                }
            }
            EntryPoint.WriteToConsole($"VehicleChase Vehicle Target: {Ped.Pedestrian.Handle} IsChasingRecklessly: {IsChasingRecklessly}", 5);
            IsFirstRun = false;
        }
        else
        {
            NativeFunction.Natives.SET_DRIVER_ABILITY(Ped.Pedestrian, 100f);
            if (Ped.IsInHelicopter)
            {
                NativeFunction.Natives.TASK_HELI_CHASE(Ped.Pedestrian, OtherTarget.Pedestrian, -50f, 50f, 60f);
            }
            else if (Ped.IsInBoat)
            {
                NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, OtherTarget.Pedestrian);
            }
            else
            {
                if (IsChasingRecklessly != ShouldChaseRecklessly)
                {
                    if (ShouldChaseRecklessly)
                    {
                        IsChasingRecklessly = true;
                        unsafe
                        {
                            int lol = 0;
                            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                            NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION", 0, Ped.Pedestrian.CurrentVehicle, OtherTarget.Pedestrian.CurrentVehicle, (int)eVehicleMissionType.Ram, 50f, 0, 0f, 0f, true);//8f
                            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                        }
                    }
                    else
                    {
                        IsChasingRecklessly = false;
                        NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, OtherTarget.Pedestrian);
                    }
                }
            }
            Vector3 CurrentPosition = Ped.Pedestrian.Position;
            IsStuck = LastPosition.DistanceTo2D(CurrentPosition) <= 1.0f;
            if (IsStuck)
            {
                if (GameTimeGotStuck == 0)
                {
                    GameTimeGotStuck = Game.GameTime;
                }
            }
            else
            {
                GameTimeGotStuck = 0;
            }
            if (IsStuck && Game.GameTime - GameTimeGotStuck >= 3000)
            {
                EntryPoint.WriteToConsole($"VehicleChase Vehicle Target I AM STUCK!!: {Ped.Pedestrian.Handle}", 5);
            }
            LastPosition = CurrentPosition;
        }
    }
    private void VehicleChasePed()
    {
        if (Ped.Pedestrian.CurrentVehicle.Exists())
        {
            NeedsUpdates = false;
        }
        else
        {
            NeedsUpdates = true;
            return;

        }
        Ped.Pedestrian.BlockPermanentEvents = true;
        Ped.Pedestrian.KeepTasks = true;
        float Speed = 30f;
        if (DistanceToTarget <= 10f)
        {
            Speed = 10f;
        }
        if (Ped.IsInHelicopter)
        {
            NativeFunction.Natives.TASK_HELI_CHASE(Ped.Pedestrian, OtherTarget.Pedestrian, -50f, 50f, 60f);
        }
        else if (Ped.IsInBoat)
        {
            NativeFunction.Natives.TASK_VEHICLE_CHASE(Ped.Pedestrian, OtherTarget.Pedestrian);
        }
        else
        {
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION_PED_TARGET", 0, Ped.Pedestrian.CurrentVehicle, OtherTarget.Pedestrian, 7, Speed, 541327934, 8f, 0f, true);//541327934//4 | 8 | 16 | 32 | 512 | 262144
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
        //NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION_PED_TARGET", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, Target.Pedestrian, 7, 30f, 4 | 8 | 16 | 32 | 512 | 262144, 0f, 0f, true);
        EntryPoint.WriteToConsole($"VehicleChase Ped Target: {Ped.Pedestrian.Handle}", 5);
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
