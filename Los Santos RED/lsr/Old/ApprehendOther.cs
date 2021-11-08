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
    private float MoveRate = 1.0f;
    private float CurrentDistanceToTarget = 999f;
    private enum Task
    {
        Main,
        Nothing,
    }
    private Task CurrentTaskDynamic
    {
        get
        {
            if (OtherTargets != null && OtherTargets.Any())
            {
                return Task.Main;
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
            MoveRate = (float)(RandomItems.MyRand.NextDouble() * (1.175 - 1.1) + 1.1);
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
            //Ped.Pedestrian.Tasks.Clear();
            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
            Ped.Pedestrian.BlockPermanentEvents = false;
            Ped.Pedestrian.KeepTasks = false;
            //Ped.Pedestrian.Tasks.Clear();
            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
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
        if (myCurrentTask == Task.Main)
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
            RunInterval = 500;
            if (IsArresting && OtherTarget != null && OtherTarget.WantedLevel > 2)
            {
                IsArresting = false;
            }
            ClearTasks(false);
            OtherTargetTask();
        }
        if(OtherTarget?.Handle != ClosestPed?.Handle)
        {
            SubTaskName = "";
        }
        OtherTarget = ClosestPed;
        if (OtherTarget != null && !OtherTarget.Pedestrian.Exists())
        {
            OtherTarget = null;
        }
        else
        {
            if (Ped != null && Ped.Pedestrian.Exists() && OtherTarget != null && OtherTarget.Pedestrian.Exists())
            {
                CurrentDistanceToTarget = OtherTarget.Pedestrian.DistanceTo2D(Ped.Pedestrian);
            }
            else
            {
                CurrentDistanceToTarget = 999f;
            }
        }

        if(IsArresting && OtherTarget != null && OtherTarget.WantedLevel >= 3)
        {
            IsArresting = false;
        }
        else if (!IsArresting && OtherTarget != null && OtherTarget.WantedLevel < 3)
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

        if (OtherTarget != null && OtherTarget.Pedestrian.Exists() && OtherTarget.Pedestrian.IsStunned)
        {
            //NativeFunction.Natives.CLEAR_PED_TASKS(OtherTarget.Pedestrian);
            SetArrestedAnimation(OtherTarget.Pedestrian, false, false);
            OtherTarget.SetWantedLevel(0);
            //OtherTarget.Pedestrian.Health = 0;
            //OtherTarget.Pedestrian.Kill();//for now simulate arrested?
            //EntryPoint.WriteToConsole($"Should kill {OtherTarget.Pedestrian.Handle}", 3);
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
                NativeFunction.Natives.SET_PED_MOVE_RATE_OVERRIDE<uint>(Ped.Pedestrian, MoveRate);
                IsReadyForWeaponUpdates = true;
                if (IsArresting)
                {
                    if (OtherTarget.IsInVehicle)
                    {
                        if (SubTaskName != "ArrestingInVehicle")
                        {
                            unsafe
                            {
                                int lol = 0;
                                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                NativeFunction.CallByName<bool>("TASK_ARREST_PED", 0, OtherTarget.Pedestrian);
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

                        if(CurrentDistanceToTarget <= 7f && OtherTarget.Pedestrian.IsStunned)
                        {
                            if (SubTaskName != "ArrestingStunned")
                            {
                                unsafe
                                {
                                    int lol = 0;
                                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                    NativeFunction.CallByName<bool>("TASK_ARREST_PED", 0, OtherTarget.Pedestrian);
                                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Ped.Pedestrian, lol);
                                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                                }
                                SubTaskName = "ArrestingStunned";
                            }
                        }
                        else if (CurrentDistanceToTarget >= 10f)
                        {
                            if (SubTaskName != "ArrestingFar")
                            {
                                unsafe
                                {
                                    int lol = 0;
                                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                    NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, OtherTarget.Pedestrian, -1, 5f, 2.0f, 1073741824, 1); //Original and works ok//7f
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
                            if (SubTaskName != "ArrestingClose")
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
                        }
                    }
                    EntryPoint.WriteToConsole($"COP EVENT {Ped.Pedestrian.Handle}: ApprehendClosest Set Arrest Task Sequence: {OtherTarget.Pedestrian.Handle}", 3);
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
                        NativeFunction.Natives.TASK_COMBAT_PED(Ped.Pedestrian, OtherTarget.Pedestrian, 0, 16);

                        SubTaskName = "Fighting";
                    }
                }
            }
        }
    }
    private void SetArrestedAnimation(Ped PedToArrest, bool MarkAsNoLongerNeeded, bool StayStanding)
    {
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
                    //EntryPoint.WriteToConsole("SetArrestedAnimation! Tasked to leave the vehicle");
                    NativeFunction.CallByName<uint>("TASK_LEAVE_VEHICLE", PedToArrest, oldVehicle, 256);
                    GameFiber.Wait(2500);
                }
            }
            if (StayStanding)
            {
                if (!NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "ped", "handsup_enter", 3))
                {
                    NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
                    //EntryPoint.WriteToConsole("SetArrestedAnimation! Standing Animation");
                }
            }
            else
            {
                if (!NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_2_hands_up", 3) && !NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_a", 3))
                {
                    //EntryPoint.WriteToConsole("SetArrestedAnimation! Kneel Animation");
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_2_hands_up", 8.0f, -8.0f, -1, 2, 0, false, false, false);
                    GameFiber.Wait(6000);

                    if (!PedToArrest.Exists() || (PedToArrest == Game.LocalPlayer.Character && !Player.IsBusted))
                    {
                        return;
                    }

                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_a", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                }
            }
            PedToArrest.KeepTasks = true;

            if (MarkAsNoLongerNeeded)
            {
                PedToArrest.IsPersistent = false;
            }
        }, "SetArrestedAnimation");
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

