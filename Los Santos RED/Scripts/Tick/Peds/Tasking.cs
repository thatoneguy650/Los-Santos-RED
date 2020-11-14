using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

public static class Tasking
{   
    private static uint LastBust;
    private static bool SurrenderBust;
    private static List<CopTaskQueueItem> CopTaskQueue;
    private static List<CivilianTaskQueueItem> CivilianTaskQueue;
    private static List<TaskableCop> TaskableCops;
    private static List<TaskableCivilian> TaskableCivilians;
    public static int CiviliansReportingCrimes { get; set; }
    public static string CurrentPoliceTickRunning { get; set; }
    public static bool IsRunning { get; set; }
    public static bool PoliceChasingRecklessly
    {
        get
        {
            if (WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.DeadlyChase && (WantedLevelScript.CurrentCrimes.InstancesOfCrime(Crimes.KillingPolice) >= 1 || WantedLevelScript.CurrentCrimes.InstancesOfCrime(Crimes.KillingCivilians) >= 2 || PlayerState.WantedLevel >= 4))
                return true;
            else
                return false;
        }
    }
    private static bool IsBustTimeOut
    {
        get
        {
            if (WantedLevelScript.HasBeenWantedFor <= 3000)
                return true;
            else if (Surrender.IsCommitingSuicide)
                return true;
            else if (Game.GameTime - LastBust >= 10000)
                return false;
            else
                return true;
        }
    }
    public static bool HasCopsInvestigating
    {
        get
        {
            if (TaskableCops.Any(x => x.TaskGTACop.Pedestrian.Exists() && x.RunningTask == VehicleInvestigation))
                return true;
            else
                return false;
        }
    }
    public static int CopsFootChasingFootSuspect
    {
        get
        {
            return TaskableCops.Count(x => x.TaskGTACop.Pedestrian.Exists() && (x.RunningTask == FootChaseOnFoot || x.RunningTask == FootArrestOnFoot));
        }
    }
    public static int CopsVehicleChasingFootSuspect
    {
        get
        {
            return TaskableCops.Count(x => x.TaskGTACop.Pedestrian.Exists() && x.RunningTask == FootChaseWithVehicle);
        }
    }
    private enum ChaseStatus
    {
        Idle = 0,
        Investigation = 1,
        Active = 2,
    }
    private static bool CanVehicleChase(Cop Cop)
    {
        if (!Cop.Pedestrian.IsInAnyVehicle(false))
            return false;
        else if (PlayerState.IsInVehicle)
            return false;
        else if (!Cop.RecentlySeenPlayer)
            return false;  
        else if (WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.Normal || WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.DeadlyChase || WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.ArrestedWait || PlayerState.IsBusted || PlayerState.IsDead)
            return false;
        else if (PlayerLocation.PlayerIsOffroad)
            return true;
        else if (CopsFootChasingFootSuspect > 1)
            return true;
        else
            return true;
        
    }
    public static void Initialize()
    {
        IsRunning = true;
        CopTaskQueue = new List<CopTaskQueueItem>();
        CivilianTaskQueue = new List<CivilianTaskQueueItem>();
        TaskableCops = new List<TaskableCop>();
        TaskableCivilians = new List<TaskableCivilian>();

        LastBust = 0;
        SurrenderBust = false;
        CurrentPoliceTickRunning = "";   
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static string GetCopTask(uint PedHandle)
    {
        TaskableCop MyCop = TaskableCops.Where(x => x.TaskGTACop.Pedestrian.Handle == PedHandle).FirstOrDefault();
        if(MyCop == null)
        {
            return "";
        }
        Action<TaskableCop> RT = MyCop.RunningTask;
        if(RT == null)
        {
            return "";
        }
        else
        {
            if (MyCop.SubTaskName != "")
            {
                return RT.Method.Name.ToString() + " - " + MyCop.SubTaskName;
            }
            else
            {
                return RT.Method.Name.ToString();
            }
        }
    }
    public static void TaskPeds()
    {
        if (IsRunning)
        {
            UpdatedPedsToTask();

            if (PlayerState.IsWanted)
            {
                PoliceTickWanted();
            }
            else
            {
                PoliceTickNotWanted();
            }
            CivilianTick();
        }
    }
    public static void ProcessQueue()
    {
        if (IsRunning)
        {
            
            if (CopTaskQueue.Any())
            {
                //Debugging.WriteToLog("ProcessQueue", string.Format("Cop: Cop Queue {0}, CivilianQueue {1}", CopTaskQueue.Count(), CivilianTaskQueue.Count()));
                CopTaskQueueItem CopTaskToRun = CopTaskQueue[0];
                CopTaskToRun.TaskedCopToAssign.IsTasked = true;

                if (CopTaskToRun.TaskToRun == Untask && CopTaskQueue.Any(x => x.TaskedCopToAssign == CopTaskToRun.TaskedCopToAssign && x.TaskToRun != Untask && x.GameTimeAssigned >= CopTaskToRun.GameTimeAssigned))
                {
                    CopTaskToRun.TaskedCopToAssign.TaskIsQueued = false;
                    CopTaskQueue.RemoveAt(0);
                }
                else
                {
                    CopTaskToRun.TaskToRun(CopTaskToRun.TaskedCopToAssign);
                    CopTaskToRun.TaskedCopToAssign.RunningTask = CopTaskToRun.TaskToRun;
                    CopTaskToRun.TaskedCopToAssign.TaskIsQueued = false;
                    CopTaskQueue.RemoveAt(0);
                    Debugging.WriteToLog("Tasking", string.Format("Assigned Task -{0}- to {1}", CopTaskToRun.TaskToRun.Method.Name,CopTaskToRun.TaskedCopToAssign.TaskGTACop.Pedestrian.Handle));
                }
            }
            else if (CivilianTaskQueue.Any())
            {
                //Debugging.WriteToLog("ProcessQueue", string.Format("Civ: Cop Queue {0}, CivilianQueue {1}", CopTaskQueue.Count(), CivilianTaskQueue.Count()));
                CivilianTaskQueueItem CivilianTaskToRun = CivilianTaskQueue[0];
                if (!CivilianTaskToRun.TaskedCivilianToAssign.TaskGTAPed.CanBeTasked)
                {
                    Debugging.WriteToLog("Tasking", string.Format("Cannot Task Civ, {0}", CivilianTaskToRun.TaskedCivilianToAssign.TaskGTAPed.Pedestrian.Handle));
                    CivilianTaskToRun.TaskedCivilianToAssign.IsTasked = false;
                    CivilianTaskToRun.TaskedCivilianToAssign.TaskIsQueued = false;
                    CivilianTaskQueue.RemoveAt(0);
                }
                else
                {
                    CivilianTaskToRun.TaskedCivilianToAssign.IsTasked = true;

                    if (CivilianTaskToRun.TaskToRun == UntaskCivilian && CivilianTaskQueue.Any(x => x.TaskedCivilianToAssign == CivilianTaskToRun.TaskedCivilianToAssign && x.TaskToRun != UntaskCivilian && x.GameTimeAssigned >= CivilianTaskToRun.GameTimeAssigned))
                    {
                        CivilianTaskToRun.TaskedCivilianToAssign.TaskIsQueued = false;
                        CivilianTaskQueue.RemoveAt(0);
                    }
                    else
                    {
                        //Debugging.WriteToLog("CivilianTaskQueue", "-------------Start--------------");
                        CivilianTaskToRun.TaskToRun(CivilianTaskToRun.TaskedCivilianToAssign);
                        CivilianTaskToRun.TaskedCivilianToAssign.RunningTask = CivilianTaskToRun.TaskToRun;
                        CivilianTaskToRun.TaskedCivilianToAssign.TaskIsQueued = false;
                        CivilianTaskQueue.RemoveAt(0);
                    }
                }
            }
        }
    }
    private static void UpdatedPedsToTask()
    {
        PedList.CopPeds.RemoveAll(x => !x.Pedestrian.Exists());
        TaskableCops.RemoveAll(x => !x.TaskGTACop.Pedestrian.Exists());
        TaskableCivilians.RemoveAll(x => !x.TaskGTAPed.Pedestrian.Exists());
        foreach (Cop Cop in PedList.CopPeds.Where(x => x.Pedestrian.Exists()))
        {
            if (!TaskableCops.Any(x => x.TaskGTACop.Pedestrian.Handle == Cop.Pedestrian.Handle))
            {
                TaskableCops.Add(new TaskableCop(Cop));
            }
        }
        foreach (PedExt Ped in PedList.Civilians.Where(x => x.Pedestrian.Exists()))
        {
            if (!TaskableCivilians.Any(x => x.TaskGTAPed.Pedestrian.Handle == Ped.Pedestrian.Handle))
            {
                TaskableCivilians.Add(new TaskableCivilian(Ped));
            }
        }
    }
    private static void PoliceTickNotWanted()
    {
        foreach (TaskableCop Cop in TaskableCops.Where(x => x.TaskGTACop.Pedestrian.Exists()))
        {
            SetChaseStatus(Cop);
            if (Cop.RunningTask != Idle && !Cop.TaskIsQueued && Cop.CurrentChaseStatus == ChaseStatus.Idle)
            {
                AddItemToCopQueue(new CopTaskQueueItem(Cop, Idle,"Idle"));
            }
            else if (Cop.RunningTask != VehicleInvestigation && !Cop.TaskIsQueued && Cop.CurrentChaseStatus == ChaseStatus.Investigation)
            {
                AddItemToCopQueue(new CopTaskQueueItem(Cop, VehicleInvestigation,"VehicleInvestigation"));
            }
            if(Cop.IsTasked && Cop.RunningTask == Idle)
            {
                TurnOffSiren(Cop.TaskGTACop);
            }         
        }
    }
    private static void PoliceTickWanted()
    {
        foreach (TaskableCop Cop in TaskableCops.Where(x => x.TaskGTACop.Pedestrian.Exists()))
        {
            RemoveIdleTask(Cop);
            SetChaseStatus(Cop);
            if (Cop.TaskGTACop.IsInVehicle)
            {
                //SetDrivingFlags(Cop.TaskGTACop);
                TaskPoliceDriver(Cop);
            }
            else
            {
                TaskPoliceOnFoot(Cop);
            }
        }

        if (SurrenderBust && !IsBustTimeOut)
            SurrenderBustEvent();

        SearchModeStopping.StopSearchMode = true;
    }
    private static void CivilianTick()
    {
        List<Crime> CrimesToCallIn = Crimes.CurrentlyViolatingCanBeReportedByCivilians;
        if (CrimesToCallIn.Any())
        {
            foreach (TaskableCivilian Snitch in TaskableCivilians.Where(x => x.TaskGTAPed.Pedestrian.Exists()))
            {
                if (Snitch.TaskGTAPed.CanRecognizePlayer)
                {
                    foreach (Crime Bad in CrimesToCallIn)
                    {
                        Snitch.TaskGTAPed.AddCrime(Bad, Snitch.TaskGTAPed.Pedestrian.Position);
                    }
                }
                else if (Snitch.TaskGTAPed.CanHearPlayerShooting)
                {
                    foreach (Crime Bad in CrimesToCallIn.Where(x => x.CanReportBySound))
                    {
                        Snitch.TaskGTAPed.AddCrime(Bad, Snitch.TaskGTAPed.Pedestrian.Position);
                    }
                }
                if (!Snitch.IsTasked && !Snitch.TaskIsQueued && Snitch.TaskGTAPed.CanBeTasked && Snitch.TaskGTAPed.CrimesWitnessed.Any() && Snitch.TaskGTAPed.DistanceToPlayer >= 2f)
                {
                    AddItemCivilianToQueue(new CivilianTaskQueueItem(Snitch, ReactToCrime, "ReactToCrime"));
                }
                else if (Snitch.IsTasked && Snitch.TaskGTAPed.CanBeTasked && !Snitch.TaskGTAPed.CanSeePlayer && !Snitch.TaskIsQueued && Snitch.TaskGTAPed.DistanceToPlayer >= 100f)
                {
                    AddItemCivilianToQueue(new CivilianTaskQueueItem(Snitch, UntaskCivilian, "UntaskCivilian"));
                }
            }
        }
    }
    private static void AddItemToCopQueue(CopTaskQueueItem MyTask)
    {
        if (!CopTaskQueue.Any(x => x.TaskedCopToAssign.TaskGTACop == MyTask.TaskedCopToAssign.TaskGTACop && x.TaskToRun == MyTask.TaskToRun))
        {
            MyTask.GameTimeAssigned = Game.GameTime;
            CopTaskQueue.Add(MyTask);
            MyTask.TaskedCopToAssign.TaskIsQueued = true;
            MyTask.TaskedCopToAssign.GameTimeLastTasked = Game.GameTime;

            Debugging.WriteToLog("Tasking", string.Format("Add Item To Queue: Cop: {0} {1}", MyTask.TaskedCopToAssign.TaskGTACop.Pedestrian.Handle, MyTask.TaskToRun.Method.Name));
        }
    }
    private static void AddItemCivilianToQueue(CivilianTaskQueueItem MyTask)
    {
        if (!CivilianTaskQueue.Any(x => x.TaskedCivilianToAssign.TaskGTAPed == MyTask.TaskedCivilianToAssign.TaskGTAPed && x.TaskToRun == MyTask.TaskToRun))
        {
            MyTask.GameTimeAssigned = Game.GameTime;
            CivilianTaskQueue.Add(MyTask);
            MyTask.TaskedCivilianToAssign.TaskIsQueued = true;
            MyTask.TaskedCivilianToAssign.GameTimeLastTasked = Game.GameTime;

            //Debugging.WriteToLog("AddItemCivilianToQueue", string.Format("Civilian: {0} {1}", MyTask.TaskedCivilianToAssign.TaskGTAPed.Pedestrian.Handle, MyTask.TaskToRun));

        }
    }
    private static void TaskPoliceDriver(TaskableCop Cop)
    {
        if(Cop.TaskGTACop.IsInVehicle && Cop.CurrentChaseStatus == ChaseStatus.Active)
        {
            if (WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.DeadlyChase && Game.LocalPlayer.WantedLevel >= 4)
                SetCopDeadly(Cop.TaskGTACop);
            else
                SetUnarmed(Cop.TaskGTACop);

            if (!Cop.TaskIsQueued && Cop.TaskGTACop.Pedestrian.IsDriver())
            {
                if (Cop.TaskGTACop.IsInHelicopter)
                {
                    if (Cop.RunningTask != VehicleChaseWithHelicopter)
                    {
                        AddItemToCopQueue(new CopTaskQueueItem(Cop, VehicleChaseWithHelicopter, "VehicleChaseWithHelicopter"));
                    }
                }
                else
                {
                    if ((!Cop.TaskGTACop.RecentlySeenPlayer || Cop.TaskGTACop.DistanceToPlayer >= 150f) && Cop.RunningTask != VehicleChaseWithVehicle)
                    {
                        AddItemToCopQueue(new CopTaskQueueItem(Cop, VehicleChaseWithVehicle, "VehicleChaseWithVehicle"));
                    }
                    else if (Cop.TaskGTACop.RecentlySeenPlayer && CanVehicleChase(Cop.TaskGTACop) && Cop.RunningTask != FootChaseWithVehicle && TaskableCops.Count(x => x.RunningTask == FootChaseWithVehicle) <= 2)
                    {
                        AddItemToCopQueue(new CopTaskQueueItem(Cop, FootChaseWithVehicle, "FootChaseWithVehicle"));
                    }
                    //else
                    //{
                    //    AddItemToCopQueue(new CopTaskQueueItem(Cop, Untask, "Untask"));
                    //}
                    //if(PlayerState.IsInVehicle)
                    //{

                    //    else
                    //    {
                    //        AddItemToCopQueue(new CopTaskQueueItem(Cop, Untask, "Untask"));
                    //    }
                    //}
                    //else
                    //{
                    //    if (Cop.RunningTask != FootChaseWithVehicle && CanVehicleChase(Cop.TaskGTACop))//temp off && PedList.CopPeds.Any(x => x.TaskType == Chase))
                    //    {
                    //        AddItemToCopQueue(new CopTaskQueueItem(Cop, FootChaseWithVehicle, "FootChaseWithVehicle"));
                    //    }
                    //    else
                    //    {
                    //        AddItemToCopQueue(new CopTaskQueueItem(Cop, Untask, "Untask"));
                    //    }
                    //}           
                }
            }
            //if(!Cop.TaskIsQueued && Cop.TaskGTACop.IsInHelicopter && !Cop.TaskGTACop.Pedestrian.IsDriver())
            //{
            //    if(Cop.TaskGTACop.RecentlySeenPlayer() && Cop.RunningTask != AttackWithVehicleWeapon)
            //    {
            //        AddItemToCopQueue(new CopTaskQueueItem(Cop, AttackWithVehicleWeapon, "AttackWithVehicleWeapon"));   
            //    }
            //}

            if(WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.DeadlyChase)
            {
                if (Cop.TaskGTACop.IssuedHeavyWeapon == null)
                {
                    Cop.TaskGTACop.IssueHeavyWeapon();
                }
            }
        }

    }
    private static void TaskPoliceOnFoot(TaskableCop Cop)
    {
        if(WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.DeadlyChase)
            SetCopDeadly(Cop.TaskGTACop);
        else if(Cop.RunningTask != FootChaseOnFoot && Cop.RunningTask != FootArrestOnFoot)
            SetCopTazer(Cop.TaskGTACop);

        if(WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.DeadlyChase)
        {
            if (Cop.IsTasked && !Cop.TaskIsQueued && !PlayerState.HandsAreUp && !PlayerState.BeingArrested)
            {
                AddItemToCopQueue(new CopTaskQueueItem(Cop, Untask, "Untask"));
            }
        }  
        else
        {
            if ((!PlayerState.IsInVehicle || Game.LocalPlayer.Character.Speed <= 2f) && Cop.CurrentChaseStatus == ChaseStatus.Active && !Cop.TaskIsQueued)
            {
                if ((Cop.TaskGTACop.RecentlySeenPlayer || Cop.TaskGTACop.DistanceToPlayer <= 20f))//50f;
                {
                    if (Cop.RunningTask != FootChaseOnFoot)
                    {
                        AddItemToCopQueue(new CopTaskQueueItem(Cop, FootChaseOnFoot, "FootChaseOnFoot"));
                    }
                }
                else
                {
                    if (Cop.RunningTask != FootArrestOnFoot && Cop.RunningTask != FootChaseOnFoot)
                    {
                        AddItemToCopQueue(new CopTaskQueueItem(Cop, FootArrestOnFoot, "FootArrestOnFoot"));
                    }
                }
                
            }
            else if (Cop.CurrentChaseStatus == ChaseStatus.Idle && Cop.IsTasked && !Cop.TaskIsQueued)
            {
                AddItemToCopQueue(new CopTaskQueueItem(Cop, Untask, "Untask")); 
            }
        }

        if ((PlayerState.HandsAreUp || Game.LocalPlayer.Character.IsStunned || Game.LocalPlayer.Character.IsRagdoll || PlayerState.IsStationary) && !PlayerState.IsBusted && Cop.TaskGTACop.DistanceToPlayer >= 0.1f && Cop.TaskGTACop.DistanceToPlayer <= 5f && !IsBustTimeOut)// && !Police.PlayerWasJustJacking)
            SetSurrenderBust(true, string.Format("TaskPoliceOnFoot 1: {0}",Cop.TaskGTACop.Pedestrian.Handle));

    }
    private static void SetDrivingFlags(PedExt Cop)
    {
        //NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Cop.Pedestrian, 100f);
        //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Cop.Pedestrian, 8f);
        //if (!Cop.IsInHelicopter)
        //{
        //    if (PoliceChasingRecklessly)
        //    {
        //        //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.Pedestrian, 4, true);
        //        //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.Pedestrian, 8, true);
        //        //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.Pedestrian, 16, true);
        //        //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.Pedestrian, 512, true);
        //        //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.Pedestrian, 262144, true);
        //    }
        //    else if(!PoliceChasingRecklessly && Cop.DistanceToPlayer <= 9f)
        //    {
        //        NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.Pedestrian, 32, true);//only originally this one for reckless pursuit
        //    }

        //    if (PlayerLocation.PlayerIsOffroad && Cop.DistanceToPlayer <= 200f)
        //    {
        //        NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.Pedestrian, 4194304, true);
        //    }
        //    else
        //    {
        //        NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.Pedestrian, 4194304, false);
        //    }
        //}
    }

    private static void FootChaseOnFoot(TaskableCop Cop)
    {
        if (!Cop.TaskGTACop.Pedestrian.Exists())
            return;
        if (Cop.TaskGTACop.Pedestrian.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f) && Cop.TaskFiber != null && Cop.TaskFiber.Name == "Chase" && !Cop.TaskGTACop.RecentlySeenPlayer)
        {
            return;
        }
        if (!Cop.TaskGTACop.Pedestrian.IsInRangeOf(Game.LocalPlayer.Character.Position, 100f) && Cop.TaskFiber != null)
        {
            Cop.TaskGTACop.Pedestrian.Tasks.Clear();
            Cop.TaskGTACop.Pedestrian.BlockPermanentEvents = false;
            Cop.TaskFiber.Abort();
            Cop.TaskFiber = null;
            return;
        }

        Cop.TaskFiber =
        GameFiber.StartNew(delegate
        {
            if (!Cop.TaskGTACop.Pedestrian.Exists())
                return;

            double cool = General.MyRand.NextDouble() * (1.175 - 1.1) + 1.1;//(1.17 - 1.075) + 1.075;//(1.175 - 1.1) + 1.1;
            float MoveRate = (float)cool;
            Cop.IsTasked = true;
            Cop.RunningTask = FootChaseOnFoot;
            Cop.SubTaskName = "Goto";
            NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_CLIMBOVERS", Cop.TaskGTACop.Pedestrian, true);
            NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_LADDERS", Cop.TaskGTACop.Pedestrian, true);
            NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_DROP_FROM_HEIGHT", Cop.TaskGTACop.Pedestrian, true);
            Cop.TaskGTACop.Pedestrian.BlockPermanentEvents = true;

            //Main Loop
            while (Cop.TaskGTACop.Pedestrian.Exists() && !Cop.TaskGTACop.Pedestrian.IsDead)
            {
                Cop.TaskGTACop.Pedestrian.BlockPermanentEvents = true;
                Cop.TaskGTACop.Pedestrian.KeepTasks = true;

                if (PlayerState.WantedLevel >= 2)
                    NativeFunction.CallByName<uint>("SET_PED_MOVE_RATE_OVERRIDE", Cop.TaskGTACop.Pedestrian, MoveRate);

                ArmCopAppropriately(Cop.TaskGTACop);
                if (Cop.TaskGTACop.DistanceToPlayer > 100f || !Cop.TaskGTACop.RecentlySeenPlayer)
                    break;

                if (PlayerState.IsInVehicle && Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle != null && PlayerState.IsStationary)
                {
                    if (Cop.TaskGTACop.DistanceToPlayer <= 25f && Cop.SubTaskName != "CarJack")
                    {
                        Cop.TaskGTACop.Pedestrian.CanRagdoll = false;
                        NativeFunction.CallByName<bool>("TASK_OPEN_VEHICLE_DOOR", Cop.TaskGTACop.Pedestrian, Game.LocalPlayer.Character.CurrentVehicle, -1, -1, 10f);
                        Cop.SubTaskName = "CarJack";
                    }
                }
                else
                {
                    if (PlayerState.WantedLevel <= 1)
                    {
                        if (Cop.SubTaskName != "Approach" && WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.UnarmedChase && Cop.TaskGTACop.DistanceToPlayer >= 7f)
                        {
                            NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Game.LocalPlayer.Character, -1, 4f, 0.25f, 1073741824, 1); //Original and works ok
                            Cop.SubTaskName = "Approach";
                        }
                    }
                    else
                    {
                        if (Cop.SubTaskName != "Arrest" && (WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.ArrestedWait))// || (WantedLevel.CurrentPoliceState == WantedLevel.PoliceState.CautiousChase && Cop.DistanceToPlayer <= 15f)))
                        {
                            unsafe
                            {
                                int lol = 0;
                                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Game.LocalPlayer.Character, -1, 20f, 500f, 1073741824, 1); //Original and works ok
                                NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", 0, Game.LocalPlayer.Character, 4f, 20f);
                                NativeFunction.CallByName<bool>("TASK_AIM_GUN_AT_ENTITY", 0, Game.LocalPlayer.Character, 10000, false);
                                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.TaskGTACop.Pedestrian, lol);
                                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                            }
                            Cop.SubTaskName = "Arrest";
                        }
                        else if (Cop.SubTaskName != "GotoShooting" && (WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.UnarmedChase || WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.CautiousChase) && Cop.TaskGTACop.DistanceToPlayer <= 7f)
                        {
                            Cop.TaskGTACop.Pedestrian.CanRagdoll = true;
                            NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY", Cop.TaskGTACop.Pedestrian, Game.LocalPlayer.Character, Game.LocalPlayer.Character, 200f, true, 4.0f, 200f, false, false, (uint)FiringPattern.DelayFireByOneSecond);
                            Cop.SubTaskName = "GotoShooting";
                        }
                        else if (Cop.SubTaskName != "Goto" && (WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.UnarmedChase || WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.CautiousChase) && Cop.TaskGTACop.DistanceToPlayer >= 15f) //was 15f
                        {
                            Cop.TaskGTACop.Pedestrian.CanRagdoll = true;
                            NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", Cop.TaskGTACop.Pedestrian, Game.LocalPlayer.Character, -1, 5.0f, 500f, 1073741824, 1); //Original and works ok
                            Cop.SubTaskName = "Goto";
                        }
                    }
                }

                if (PlayerState.IsInVehicle && Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle != null && (Cop.TaskGTACop.DistanceToPlayer >= 45f || Game.LocalPlayer.Character.CurrentVehicle.Speed >= 1f))
                {
                    GameFiber.Sleep(General.MyRand.Next(500, 2000));
                    break;
                }
                if (WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.Normal || WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.DeadlyChase || PlayerState.IsDead)
                {
                    GameFiber.Sleep(General.MyRand.Next(500, 2000));
                    break;
                }
                GameFiber.Sleep(250);//500//250
            }
            if (Cop.TaskGTACop.Pedestrian.Exists() && !Cop.TaskGTACop.Pedestrian.IsDead)
            {
                Cop.TaskGTACop.Pedestrian.BlockPermanentEvents = false;
                Cop.TaskGTACop.Pedestrian.Tasks.Clear();
                if (Cop.TaskGTACop.Pedestrian.LastVehicle.Exists() && !Cop.TaskGTACop.Pedestrian.LastVehicle.IsPoliceVehicle)
                    Cop.TaskGTACop.Pedestrian.ClearLastVehicle();
            }
            Cop.TaskFiber = null;
            Cop.IsTasked = false;
            Cop.RunningTask = null;
            Cop.SubTaskName = "";
            if (Cop.TaskGTACop.Pedestrian.Exists() && !Cop.TaskGTACop.Pedestrian.IsDead)
                Cop.TaskGTACop.Pedestrian.CanRagdoll = true;

        }, "Chase");
        Debugging.GameFibers.Add(Cop.TaskFiber);
    }
    private static void FootChaseWithVehicle(TaskableCop Cop)
    {
        Cop.TaskFiber =
        GameFiber.StartNew(delegate
        {
            // LocalWriteToLog("Task Vehicle Chasing", string.Format("Started Vehicle Chase: {0}", Cop.CopPed.Handle));
            uint TaskTime = Game.GameTime;
            if (!Cop.TaskGTACop.Pedestrian.Exists())
                return;

            Cop.TaskGTACop.Pedestrian.BlockPermanentEvents = true;
            Cop.IsTasked = true;
            Cop.RunningTask = FootChaseWithVehicle;

            NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Cop.TaskGTACop.Pedestrian, 100f);
            Cop.TaskGTACop.Pedestrian.KeepTasks = true;

            while (Cop.TaskGTACop.Pedestrian.Exists() && !Cop.TaskGTACop.Pedestrian.IsDead && !PlayerState.IsDead && !PlayerState.IsBusted)
            {

                if (Game.GameTime - TaskTime >= 250)
                {
                    Vector3 PlayerPos = Game.LocalPlayer.Character.Position;
                    Vector3 DrivingCoords = World.GetNextPositionOnStreet(PlayerPos);
                    if (!CanVehicleChase(Cop.TaskGTACop) || DrivingCoords == Vector3.Zero)
                    {
                        break;
                    }
                    NativeFunction.CallByName<bool>("SET_DRIVE_TASK_DRIVING_STYLE", Cop.TaskGTACop.Pedestrian, 6);
                    NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.TaskGTACop.Pedestrian, 2, true);
                    NativeFunction.CallByName<bool>("TASK_VEHICLE_GOTO_NAVMESH", Cop.TaskGTACop.Pedestrian, Cop.TaskGTACop.Pedestrian.CurrentVehicle, DrivingCoords.X, DrivingCoords.Y, DrivingCoords.Z, 25f, 110, 10f);
                    Cop.TaskGTACop.Pedestrian.KeepTasks = true;
                    TaskTime = Game.GameTime;
                }
                GameFiber.Yield();
            }
            if (Cop.TaskGTACop.Pedestrian.Exists() && !Cop.TaskGTACop.Pedestrian.IsDead)
            {
                NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.TaskGTACop.Pedestrian, 3, true);
                Cop.TaskGTACop.Pedestrian.BlockPermanentEvents = false;
                Cop.TaskGTACop.Pedestrian.Tasks.Clear();
            }
            Cop.TaskFiber = null;
            Cop.IsTasked = false;
            Cop.RunningTask = null;
        }, "VehicleChase");
        Debugging.GameFibers.Add(Cop.TaskFiber);
    }
    private static void VehicleChaseWithVehicle(TaskableCop Cop)
    {
        if (!Cop.TaskGTACop.Pedestrian.Exists() || !Cop.TaskGTACop.Pedestrian.IsDriver())
            return;

        Cop.TaskFiber =
        GameFiber.StartNew(delegate
        {
            if (!Cop.TaskGTACop.Pedestrian.Exists() || !Cop.TaskGTACop.Pedestrian.IsDriver())
                return;

            Cop.IsTasked = true;
            Cop.RunningTask = VehicleChaseWithVehicle;
            Cop.TaskGTACop.Pedestrian.BlockPermanentEvents = false;

            Vector3 WantedCenter = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
            Vector3 TaskedLocation;
            string SubTask;
            if (Police.InSearchMode)
            {
                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.TaskGTACop.Pedestrian, Cop.TaskGTACop.Pedestrian.CurrentVehicle, WantedCenter.X, WantedCenter.Y, WantedCenter.Z, 20f, 4 | 16 | 32 | 262144, 20f);
                SubTask = "DriveTo";
                TaskedLocation = WantedCenter;
            }
            else
            {
                if (PlayerState.IsInVehicle)
                {
                    NativeFunction.CallByName<bool>("TASK_VEHICLE_CHASE", Cop.TaskGTACop.Pedestrian, Game.LocalPlayer.Character); //NativeFunction.CallByName<bool>("TASK_VEHICLE_FOLLOW", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, Game.LocalPlayer.Character, 22f, 4 | 16 | 32 | 262144, 8f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_CHASE", Cop.Pedestrian, Game.LocalPlayer.Character);
                }
                else
                {
                    NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.TaskGTACop.Pedestrian, Cop.TaskGTACop.Pedestrian.CurrentVehicle, WantedCenter.X, WantedCenter.Y, WantedCenter.Z, 20f, 4 | 16 | 32 | 262144, 20f);
                }
                SubTask = "Chase";
                TaskedLocation = WantedCenter;
            }

            Debugging.WriteToLog("Tasking", string.Format("Started DriveTo/Chase: {0}", Cop.TaskGTACop.Pedestrian.Handle));

            while (Cop.TaskGTACop.Pedestrian.Exists() && Cop.TaskGTACop.Pedestrian.IsDriver())
            {
                WantedCenter = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
                if (Police.InSearchMode)
                {
                    if (Cop.TaskGTACop.Pedestrian.DistanceTo2D(WantedCenter) <= 25f && !Cop.TaskGTACop.AtWantedCenterDuringSearchMode && SubTask != "Cruise")
                    {
                        Cop.TaskGTACop.AtWantedCenterDuringSearchMode = true;
                        Cop.TaskGTACop.Pedestrian.Tasks.CruiseWithVehicle(30f, VehicleDrivingFlags.Emergency);
                        SubTask = "Cruise";
                        TaskedLocation = Vector3.Zero;
                        Debugging.WriteToLog("Tasking", string.Format("DriveTo/Chase Cruise: {0}", Cop.TaskGTACop.Pedestrian.Handle));
                    }
                    else
                    {
                        if ((!Cop.TaskGTACop.AtWantedCenterDuringSearchMode && SubTask != "DriveTo") && (TaskedLocation != WantedCenter))
                        {
                            NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.TaskGTACop.Pedestrian, Cop.TaskGTACop.Pedestrian.CurrentVehicle, WantedCenter.X, WantedCenter.Y, WantedCenter.Z, 20f, 4 | 16 | 32 | 262144, 20f);
                            SubTask = "DriveTo";
                            TaskedLocation = WantedCenter;
                            Debugging.WriteToLog("Tasking", string.Format("DriveTo/Chase Location Updated: {0}", Cop.TaskGTACop.Pedestrian.Handle));
                        }
                    }
                }
                else if (PlayerState.IsWanted)
                {
                    if (PlayerState.IsInVehicle && SubTask != "Chase")
                    {
                        NativeFunction.CallByName<bool>("TASK_VEHICLE_CHASE", Cop.TaskGTACop.Pedestrian, Game.LocalPlayer.Character);//NativeFunction.CallByName<bool>("TASK_VEHICLE_CHASE", Cop.TaskGTACop.Pedestrian, Game.LocalPlayer.Character); //NativeFunction.CallByName<bool>("TASK_VEHICLE_FOLLOW", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, Game.LocalPlayer.Character, 22f, 4 | 16 | 32 | 262144, 8f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_CHASE", Cop.Pedestrian, Game.LocalPlayer.Character);
                        SubTask = "Chase";
                        Debugging.WriteToLog("Tasking", string.Format("DriveTo/Chase Location Updated (OnFootPlayer): {0}", Cop.TaskGTACop.Pedestrian.Handle));
                    }
                    else if (!PlayerState.IsInVehicle && TaskedLocation.DistanceTo2D(WantedCenter) >= 10f && SubTask == "Chase" && Cop.TaskGTACop.DistanceToPlayer >= 20f)
                    {
                        NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.TaskGTACop.Pedestrian, Cop.TaskGTACop.Pedestrian.CurrentVehicle, WantedCenter.X, WantedCenter.Y, WantedCenter.Z, 20f, 4 | 16 | 32 | 262144, 20f);
                        TaskedLocation = WantedCenter;
                        SubTask = "Chase";
                        Debugging.WriteToLog("Tasking", string.Format("DriveTo/Chase Location Updated (OnFootPlayer): {0}", Cop.TaskGTACop.Pedestrian.Handle));
                    }
                }

                if (Cop.TaskGTACop.Pedestrian.CurrentVehicle.HasSiren && !Cop.TaskGTACop.Pedestrian.CurrentVehicle.IsSirenOn)
                {
                    Cop.TaskGTACop.Pedestrian.CurrentVehicle.IsSirenOn = true;
                    Cop.TaskGTACop.Pedestrian.CurrentVehicle.IsSirenSilent = false;

                }


                if(!PlayerState.IsInVehicle || PlayerState.IsStationary)
                {
                    break;
                }
                GameFiber.Sleep(1500);//1000
            }


            if (!Cop.TaskGTACop.Pedestrian.Exists())
                return;


            AddItemToCopQueue(new CopTaskQueueItem(Cop, Untask,"Untask"));
            Debugging.WriteToLog("Tasking", string.Format("Finished DriveTo/Chase: {0}", Cop.TaskGTACop.Pedestrian.Handle));

        }, "TaskDriveToAndChase");
        Debugging.GameFibers.Add(Cop.TaskFiber);
    }
    private static void VehicleChaseWithHelicopter(TaskableCop Cop)
    {
        if (!Cop.TaskGTACop.Pedestrian.Exists() || !Cop.TaskGTACop.Pedestrian.IsDriver())
            return;

        Cop.TaskFiber =
        GameFiber.StartNew(delegate
        {
            Cop.IsTasked = true;
            Cop.RunningTask = VehicleChaseWithHelicopter;
            Cop.TaskGTACop.Pedestrian.BlockPermanentEvents = false;

            Vector3 WantedCenter = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
            string SubTask;
            if (Police.InSearchMode)
            {
                Cop ClosestCop = PedList.CopPeds.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsDriver()).OrderBy(x => x.DistanceToLastSeen).FirstOrDefault();
                if (ClosestCop == null)
                    return;
                NativeFunction.CallByName<bool>("TASK_HELI_CHASE", Cop.TaskGTACop.Pedestrian, ClosestCop.Pedestrian, -50f, 50f, 60f);
                SubTask = "DriveTo";
            }
            else
            {
                NativeFunction.CallByName<bool>("TASK_HELI_CHASE", Cop.TaskGTACop.Pedestrian, Game.LocalPlayer.Character, -50f, 50f, 60f);
                SubTask = "Chase";
            }

            Debugging.WriteToLog("Tasking", string.Format("Started HeliChase: {0}", Cop.TaskGTACop.Pedestrian.Handle));

            while (Cop.TaskGTACop.Pedestrian.Exists() && Cop.TaskGTACop.Pedestrian.IsDriver())
            {
                WantedCenter = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
                if (Police.InSearchMode)
                {
                    if (SubTask != "DriveTo")
                    {
                        Cop ClosestCop = PedList.CopPeds.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsDriver()).OrderBy(x => x.DistanceToLastSeen).FirstOrDefault();
                        if (ClosestCop == null)
                            break;
                        NativeFunction.CallByName<bool>("TASK_HELI_CHASE", Cop.TaskGTACop.Pedestrian, ClosestCop.Pedestrian, -50f, 50f, 60f);
                        SubTask = "DriveTo";
                        Debugging.WriteToLog("Tasking", string.Format("Heli Lost you following closest cop: {0}", Cop.TaskGTACop.Pedestrian.Handle));
                    }
                }
                else
                {
                    if (SubTask != "Chase")
                    {
                        NativeFunction.CallByName<bool>("TASK_HELI_CHASE", Cop.TaskGTACop.Pedestrian, Game.LocalPlayer.Character, -50f, 50f, 60f);
                        SubTask = "Chase";
                        Debugging.WriteToLog("Tasking", string.Format("Heli Found you: {0}", Cop.TaskGTACop.Pedestrian.Handle));
                    }
                }
                GameFiber.Sleep(1000);
            }

            if (!Cop.TaskGTACop.Pedestrian.Exists())
                return;

            AddItemToCopQueue(new CopTaskQueueItem(Cop, Untask, "Untask"));
            Debugging.WriteToLog("Tasking", string.Format("Finished HeliChase: {0}", Cop.TaskGTACop.Pedestrian.Handle));

        }, "HeliCHase");
        Debugging.GameFibers.Add(Cop.TaskFiber);

    }
    private static void AttackWithVehicleWeapon(TaskableCop Cop)
    {
        if (!Cop.TaskGTACop.Pedestrian.Exists() || Cop.TaskGTACop.Pedestrian.IsDriver())
            return;

        Cop.TaskFiber =
        GameFiber.StartNew(delegate
        {
            Cop.IsTasked = true;
            Cop.RunningTask = AttackWithVehicleWeapon;
            Cop.TaskGTACop.Pedestrian.BlockPermanentEvents = false;

            Debugging.WriteToLog("Tasking", string.Format("Started HeliAttack: {0}", Cop.TaskGTACop.Pedestrian.Handle));

            while (Cop.TaskGTACop.Pedestrian.Exists() && !Cop.TaskGTACop.Pedestrian.IsDriver())
            {
                if (Cop.TaskGTACop.CanSeePlayer)
                {

                    //none of this works, they still wont shoot the miniguns
                    //NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.TaskGTACop.Pedestrian,2,true);

                    //NativeFunction.CallByName<bool>("SET_PED_COMBAT_RANGE", Cop.TaskGTACop.Pedestrian, 2);
                    
                    //bool ConControl = NativeFunction.CallByName<bool>("CONTROL_MOUNTED_WEAPON", Cop.TaskGTACop.Pedestrian);
                    //// NativeFunction.CallByName<bool>("CONTROL_MOUNTED_WEAPON", Cop.TaskGTACop.Pedestrian);
                    //Debugging.WriteToLog("Tasking", string.Format("HeliAttack: {0} CanControl: {1}", Cop.TaskGTACop.Pedestrian.Handle, ConControl));


                    //Debugging.WriteToLog("Tasking", string.Format("DOES_VEHICLE_HAVE_WEAPONS: {0}", NativeFunction.CallByName<bool>("DOES_VEHICLE_HAVE_WEAPONS", Cop.TaskGTACop.Pedestrian.CurrentVehicle)));


                    
                    //NativeFunction.CallByHash<bool>(0x44CD1F493DB2A0A6, Cop.TaskGTACop.Pedestrian.CurrentVehicle, 0,20);
                    //Debugging.WriteToLog("Tasking", string.Format("0x44CD1F493DB2A0A6: {0}", 0));
                    //NativeFunction.CallByHash<bool>(0x44CD1F493DB2A0A6, Cop.TaskGTACop.Pedestrian.CurrentVehicle, 1, 20);
                    //Debugging.WriteToLog("Tasking", string.Format("0x44CD1F493DB2A0A6: {0}", 1));

                    //if (ConControl)
                    //{
                    //    //NativeFunction.CallByName<bool>("SET_MOUNTED_WEAPON_TARGET", Cop.TaskGTACop.Pedestrian, Game.LocalPlayer.Character, 0, 25f, 25f, 25f, 0, 0);
                    //}
                    //NativeFunction.CallByName<bool>("SET_MOUNTED_WEAPON_TARGET", Cop.TaskGTACop.Pedestrian, Game.LocalPlayer.Character, 0, 5f, 5f, 5f, 0, 0);


                }
                GameFiber.Sleep(1000);
            }

            if (!Cop.TaskGTACop.Pedestrian.Exists())
                return;

            AddItemToCopQueue(new CopTaskQueueItem(Cop, Untask, "Untask"));
            Debugging.WriteToLog("Tasking", string.Format("Finished HeliAttack: {0}", Cop.TaskGTACop.Pedestrian.Handle));

        }, "HeliAttack");
        Debugging.GameFibers.Add(Cop.TaskFiber);

    }
    private static void FootArrestOnFoot(TaskableCop Cop)
    {
        if (!Cop.TaskGTACop.Pedestrian.Exists())
            return;
        Cop.IsTasked = true;
        Cop.RunningTask = FootArrestOnFoot;
        Cop.TaskGTACop.Pedestrian.BlockPermanentEvents = true;
        //Cop.SimpleTaskName = "SimpleArrest";
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Game.LocalPlayer.Character, -1, 20f, 500f, 1073741824, 1); //Original and works ok
            NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", 0, Game.LocalPlayer.Character, 4f, 20f);
            NativeFunction.CallByName<bool>("TASK_AIM_GUN_AT_ENTITY", 0, Game.LocalPlayer.Character, -1, false);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.TaskGTACop.Pedestrian, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
        Cop.TaskGTACop.Pedestrian.KeepTasks = true;
        Debugging.WriteToLog("Tasking", string.Format("Started SimpleArrest: {0}", Cop.TaskGTACop.Pedestrian.Handle));
    }
    private static void VehicleInvestigation(TaskableCop Cop)
    {
        if (!Cop.TaskGTACop.Pedestrian.Exists())
            return;

        if (PlayerState.IsWanted)
            return;

        Cop.TaskFiber =
        GameFiber.StartNew(delegate
        {
            Investigation.InInvestigationMode = true;
            Cop.IsTasked = true;
            Cop.RunningTask = VehicleInvestigation;
            Cop.TaskGTACop.Pedestrian.BlockPermanentEvents = false;

            if (Investigation.InvestigationPosition == Vector3.Zero)
            {
                Investigation.InvestigationPosition = Game.LocalPlayer.Character.Position;
                if (PlayerState.IsWanted)
                    Investigation.InvestigationPosition = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
            }


            float DrivingSpeed = 20f;
            bool NeedSirenOn = true;
            if (WantedLevelScript.CurrentResponse == WantedLevelScript.ResponsePriority.Medium)
            {
                DrivingSpeed = 25f;
            }
            else if(WantedLevelScript.CurrentResponse == WantedLevelScript.ResponsePriority.Low)
            {
                DrivingSpeed = 20f;
                NeedSirenOn = false;
            }


            Vector3 OriginalTaskedPosition = Investigation.InvestigationPosition;
            if (Cop.TaskGTACop.Pedestrian.IsInAnyVehicle(false))
                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.TaskGTACop.Pedestrian, Cop.TaskGTACop.Pedestrian.CurrentVehicle, Investigation.InvestigationPosition.X, Investigation.InvestigationPosition.Y, Investigation.InvestigationPosition.Z, DrivingSpeed, 4 | 16 | 32 | 262144, 10f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, PositionOfInterest.X, PositionOfInterest.Y, PositionOfInterest.Z, 70f, 4 | 16 | 32 | 262144, 35f);
            else
                NativeFunction.CallByName<bool>("TASK_GO_STRAIGHT_TO_COORD", Cop.TaskGTACop.Pedestrian, Investigation.InvestigationPosition.X, Investigation.InvestigationPosition.Y, Investigation.InvestigationPosition.Z, 500f, -1, 0f, 2f);


            Debugging.WriteToLog("Tasking", string.Format("Started Investigate: {0}, CurrentResponse {1}, DrivingSpeed {2}, NeedSirenOn {3}", Cop.TaskGTACop.Pedestrian.Handle, WantedLevelScript.CurrentResponse, DrivingSpeed, NeedSirenOn));

            uint GameTimestartedInvestigation = Game.GameTime;
            while (Cop.TaskGTACop.Pedestrian.Exists() && Cop.TaskGTACop.Pedestrian.DistanceTo2D(Investigation.InvestigationPosition) >= 15f && Investigation.InInvestigationMode && Game.GameTime - GameTimestartedInvestigation <= 180000)//less than 3 minutes
            {
                if (Investigation.InvestigationPosition != Vector3.Zero && Investigation.InvestigationPosition != OriginalTaskedPosition) //retask them if it changes
                {
                    if (WantedLevelScript.CurrentResponse == WantedLevelScript.ResponsePriority.Medium)
                    {
                        DrivingSpeed = 25f;
                    }
                    else if (WantedLevelScript.CurrentResponse == WantedLevelScript.ResponsePriority.Low)
                    {
                        DrivingSpeed = 20f;
                        NeedSirenOn = false;
                    }
                    OriginalTaskedPosition = Investigation.InvestigationPosition;
                    if (Cop.TaskGTACop.Pedestrian.IsInAnyVehicle(false))
                        NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.TaskGTACop.Pedestrian, Cop.TaskGTACop.Pedestrian.CurrentVehicle, Investigation.InvestigationPosition.X, Investigation.InvestigationPosition.Y, Investigation.InvestigationPosition.Z, DrivingSpeed, 4 | 16 | 32 | 262144, 10f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, PositionOfInterest.X, PositionOfInterest.Y, PositionOfInterest.Z, 70f, 4 | 16 | 32 | 262144, 35f);
                    else
                        NativeFunction.CallByName<bool>("TASK_GO_STRAIGHT_TO_COORD", Cop.TaskGTACop.Pedestrian, Investigation.InvestigationPosition.X, Investigation.InvestigationPosition.Y, Investigation.InvestigationPosition.Z, 500f, -1, 0f, 2f);

                    //Debugging.WriteToLog("TaskInvestigateCrime", string.Format("Reset Investigate: {0}, CurrentResponse {1}, DrivingSpeed {2}, NeedSirenOn {3}", Cop.TaskGTACop.Pedestrian.Handle, WantedLevelScript.CurrentResponse, DrivingSpeed, NeedSirenOn));

                }

                if (Cop.TaskGTACop.Pedestrian.IsDriver() && Cop.TaskGTACop.Pedestrian.CurrentVehicle.HasSiren && NeedSirenOn)
                {
                    if (!Cop.TaskGTACop.Pedestrian.CurrentVehicle.IsSirenOn)
                    {
                        Cop.TaskGTACop.Pedestrian.CurrentVehicle.IsSirenOn = true;
                        Cop.TaskGTACop.Pedestrian.CurrentVehicle.IsSirenSilent = false;
                    }
                }
                GameFiber.Sleep(100);
            }
            if(Cop.TaskGTACop.Pedestrian.Exists() && Cop.TaskGTACop.Pedestrian.CurrentVehicle.Exists())
            {
                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", Cop.TaskGTACop.Pedestrian, Cop.TaskGTACop.Pedestrian.CurrentVehicle, DrivingSpeed, 4 | 16 | 32 | 262144, 10f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, PositionOfInterest.X, PositionOfInterest.Y, PositionOfInterest.Z, 70f, 4 | 16 | 32 | 262144, 35f);
            }

            uint GameTimeStartedInvestigating = Game.GameTime;
            while (Game.GameTime - GameTimeStartedInvestigating <= 25000)
            {
                if(!Investigation.InInvestigationMode || PlayerState.IsWanted)
                {
                    break;
                }
                GameFiber.Sleep(100);
            }


            if (!Cop.TaskGTACop.Pedestrian.Exists())
                return;

            if (PlayerState.IsNotWanted)
            {
                if (Cop.TaskGTACop.Pedestrian.Exists() && Cop.TaskGTACop.Pedestrian.IsDriver() && Cop.TaskGTACop.Pedestrian.CurrentVehicle.HasSiren)
                {
                    Cop.TaskGTACop.Pedestrian.CurrentVehicle.IsSirenOn = false;
                    Cop.TaskGTACop.Pedestrian.CurrentVehicle.IsSirenSilent = false;
                }
                Investigation.InInvestigationMode = false;
            }

            AddItemToCopQueue(new CopTaskQueueItem(Cop, Untask, "Untask"));
            Debugging.WriteToLog("Tasking", string.Format("Finished TaskInvestigateCrime: {0}", Cop.TaskGTACop.Pedestrian.Handle));

        }, "InvestigateCrime");
        Debugging.GameFibers.Add(Cop.TaskFiber);
    }
    private static void Idle(TaskableCop Cop)
    {
        if (Cop.TaskGTACop.Pedestrian.Exists())
        {
            if (!Cop.TaskGTACop.Pedestrian.IsInAnyVehicle(false))
            {
                Vehicle LastVehicle = Cop.TaskGTACop.Pedestrian.LastVehicle;
                if (LastVehicle.Exists() && LastVehicle.IsDriveable && Cop.TaskGTACop.WasRandomSpawnDriver)
                {
                    unsafe
                    {
                        int lol = 0;
                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, LastVehicle, -1, -1, 2f, 9);
                        NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, LastVehicle, 18f, 183);
                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.TaskGTACop.Pedestrian, lol);
                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    }
                    // LocalWriteToLog("RetaskAllRandomSpawns", "Told him to get in and drive");
                }
                else
                {
                    Cop.TaskGTACop.Pedestrian.Tasks.Wander();
                    //   LocalWriteToLog("RetaskAllRandomSpawns", "Told him to wander");
                }
            }
            else
            {
                if (Cop.TaskGTACop.Pedestrian.IsDriver())
                {
                    Cop.TaskGTACop.Pedestrian.Tasks.CruiseWithVehicle(Cop.TaskGTACop.Pedestrian.CurrentVehicle, 15f, VehicleDrivingFlags.Normal);
                    Cop.TaskGTACop.Pedestrian.CurrentVehicle.IsSirenOn = false;
                }
                //NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", Cop.CopPed, Cop.CopPed.CurrentVehicle, 18f, 183);
                //Cop.CopPed.Tasks.Wander();
                // LocalWriteToLog("RetaskAllRandomSpawns", "Told him to drive");
            }
        }

    }

    private static void ReactToCrime(TaskableCivilian Snitch)
    {

        if (!Snitch.TaskGTAPed.Pedestrian.Exists() || Snitch.TaskGTAPed.Pedestrian.IsDead || !Snitch.TaskGTAPed.CanBeTasked)
            return;

        Snitch.IsTasked = true;
        //Debugging.WriteToLog("ReactToCrime", string.Format("Handle: {0}, Crimes: {1}", Snitch.TaskGTAPed.Pedestrian.Handle, string.Join(",", Snitch.TaskGTAPed.CrimesWitnessed.Where(x => x.CanBeReportedByCivilians).Select(x => x.Name))));
        bool ShouldCallIn = Snitch.TaskGTAPed.CrimesWitnessed.Any(x => x.CanBeReportedByCivilians);
        if (ShouldCallIn && Snitch.TaskGTAPed.WillCallPolice && CiviliansReportingCrimes <= 5)
        {
            CivilianReportCrime(Snitch.TaskGTAPed);
        }
        else
        {
            PickReactTask(Snitch.TaskGTAPed);
        }
    }
    private static void Untask(TaskableCop Cop)
    {
        if (Cop.TaskGTACop.Pedestrian.Exists())
        {
            if (Cop.TaskFiber != null)
            {
                Cop.TaskFiber.Abort();
                Cop.TaskFiber = null;
            }
            int seatIndex = 0;
            Vehicle CurrentVehicle = null;
            bool WasInVehicle = false;
            if (Cop.TaskGTACop.WasModSpawned && Cop.TaskGTACop.Pedestrian.IsInAnyVehicle(false))
            {
                WasInVehicle = true;
                CurrentVehicle = Cop.TaskGTACop.Pedestrian.CurrentVehicle;
                seatIndex = Cop.TaskGTACop.Pedestrian.SeatIndex;
            }
            Cop.TaskGTACop.Pedestrian.Tasks.Clear();

            Cop.TaskGTACop.Pedestrian.BlockPermanentEvents = false;

            if (!Cop.TaskGTACop.WasModSpawned)
                Cop.TaskGTACop.Pedestrian.IsPersistent = false;

            if (Cop.TaskGTACop.WasModSpawned && WasInVehicle && !Cop.TaskGTACop.Pedestrian.IsInAnyVehicle(false) && CurrentVehicle != null)
            {
                Cop.TaskGTACop.Pedestrian.WarpIntoVehicle(CurrentVehicle, seatIndex);

            }
            if (Cop.TaskGTACop.Pedestrian.IsDriver() && Cop.TaskGTACop.Pedestrian.CurrentVehicle.HasSiren)
            {
                Cop.TaskGTACop.Pedestrian.CurrentVehicle.IsSirenOn = false;
                Cop.TaskGTACop.Pedestrian.CurrentVehicle.IsSirenSilent = false;
            }
            if(PlayerState.IsWanted)
                NativeFunction.CallByName<bool>("SET_PED_ALERTNESS", Cop.TaskGTACop.Pedestrian, 3);
        }

        Cop.RunningTask = null;
        Cop.IsTasked = false;
    }
    private static void UntaskCivilian(TaskableCivilian Civilian)
    {
        if (Civilian.TaskGTAPed.Pedestrian.Exists())
        {
            if (Civilian.TaskFiber != null)
            {
                Civilian.TaskFiber.Abort();
                Civilian.TaskFiber = null;
            }
            int seatIndex = 0;
            Vehicle CurrentVehicle = null;
            bool WasInVehicle = false;
            if (Civilian.TaskGTAPed.Pedestrian.IsInAnyVehicle(false))
            {
                WasInVehicle = true;
                CurrentVehicle = Civilian.TaskGTAPed.Pedestrian.CurrentVehicle;
                seatIndex = Civilian.TaskGTAPed.Pedestrian.SeatIndex;
            }
            Civilian.TaskGTAPed.Pedestrian.Tasks.Clear();

            Civilian.TaskGTAPed.Pedestrian.BlockPermanentEvents = false;

            Civilian.TaskGTAPed.Pedestrian.IsPersistent = false;

            if (WasInVehicle && !Civilian.TaskGTAPed.Pedestrian.IsInAnyVehicle(false) && CurrentVehicle != null)
            {
                Civilian.TaskGTAPed.Pedestrian.WarpIntoVehicle(CurrentVehicle, seatIndex);

            }

            if (Civilian.TaskGTAPed.Pedestrian.IsDriver() && Civilian.TaskGTAPed.Pedestrian.CurrentVehicle.HasSiren)
            {
                Civilian.TaskGTAPed.Pedestrian.CurrentVehicle.IsSirenOn = false;
                Civilian.TaskGTAPed.Pedestrian.CurrentVehicle.IsSirenSilent = false;
            }
        }

        Civilian.RunningTask = null;
        Civilian.IsTasked = false;
    }
    private static void RemoveIdleTask(TaskableCop Cop)
    {
        if (Cop.IsTasked && (Cop.RunningTask == Idle || Cop.RunningTask == VehicleInvestigation) && Cop.TaskGTACop.DistanceToPlayer <= 350f)
        {
            if (!Cop.TaskIsQueued)
            {
                AddItemToCopQueue(new CopTaskQueueItem(Cop, Untask, "Untask"));
            }
        }
    }
    private static void CivilianReportCrime(PedExt CivilianToReport)
    {
        if (CivilianToReport == null)
            return;
        if (!CivilianToReport.Pedestrian.Exists() || CivilianToReport.Pedestrian.IsDead || !CivilianToReport.CanBeTasked)
            return;

        GameFiber CrimeReportedFiber = GameFiber.StartNew(delegate
        {

            uint GameTimeStarted = Game.GameTime;

            if (!CivilianToReport.Pedestrian.Exists() || CivilianToReport.Pedestrian.IsDead)
                return;


           // Debugging.WriteToLog("CivilianReportCrime", string.Format("Handle: {0}", CivilianToReport.Pedestrian.Handle));

            CivilianToReport.Pedestrian.IsPersistent = true;
            CiviliansReportingCrimes++;


            PickReactTask(CivilianToReport);

            int TimeToWait = General.MyRand.Next(3000, 5000);

            //Pre Call, running away from crime
            bool AbortReport = false;
            while (Game.GameTime - GameTimeStarted <= TimeToWait)
            {
                if (!CivilianToReport.CanBeTasked)
                {
                    AbortReport = true;
                    break;
                }
                if (PedSwap.RecentlyTakenOver)
                {
                    AbortReport = true;
                    break;
                }
                if (!CivilianToReport.Pedestrian.Exists() || CivilianToReport.Pedestrian.IsDead)
                {
                    AbortReport = true;
                    break;
                }
                GameFiber.Sleep(200);
            }
            if (AbortReport)
            {
                if (CivilianToReport.Pedestrian.Exists())
                    CivilianToReport.Pedestrian.IsPersistent = false;

                CiviliansReportingCrimes--;
                return;
            }

            if (!CivilianToReport.Pedestrian.Exists() || CivilianToReport.Pedestrian.IsDead || PlayerState.IsDead || PlayerState.IsBusted)
            {
                if (CivilianToReport.Pedestrian.Exists())
                    CivilianToReport.Pedestrian.IsPersistent = false;

                CiviliansReportingCrimes--;
                return;
            }

            VehicleExt VehToReport = PlayerState.CurrentVehicle;
           // Debugging.WriteToLog("Check Snitches", string.Format("Civilian Reporting: {0},Crimes: {1}", CivilianToReport.Pedestrian.Handle, string.Join(",", CivilianToReport.CrimesWitnessed.Select(x => x.Name))));

            //Call It In
            NativeFunction.CallByName<bool>("TASK_USE_MOBILE_PHONE_TIMED", CivilianToReport.Pedestrian, 10000);
            CivilianToReport.Pedestrian.PlayAmbientSpeech("JACKED_GENERIC");
            if (PedSwap.RecentlyTakenOver)
            {
                CivilianToReport.Pedestrian.IsPersistent = false;
                CiviliansReportingCrimes--;
                return;
            }

            Crime WorstCrime = CivilianToReport.CrimesWitnessed.OrderBy(x => x.Priority).FirstOrDefault();
            if (WorstCrime == null)
            {
                CivilianToReport.Pedestrian.IsPersistent = false;
                CiviliansReportingCrimes--;
                return;
            }
            GameFiber.Sleep(General.MyRand.Next(3000, 7000));


            if(VehToReport == null)
                VehToReport = PlayerState.CurrentVehicle;

            Debugging.WriteToLog("Tasking","Crime Pre Recorded " + WorstCrime.Name);
            if (CivilianToReport.Pedestrian.Exists() && CivilianToReport.Pedestrian.IsAlive && !CivilianToReport.Pedestrian.IsRagdoll)
            {
                if (PlayerState.IsNotWanted)
                {
                    WantedLevelScript.CurrentCrimes.AddCrime(WorstCrime,false,CivilianToReport.PositionLastSeenCrime,VehToReport,null);
                    if (VehToReport != null)
                        VehToReport.WasReportedStolen = true;//even if it doesnt make it to us

                    Investigation.InInvestigationMode = true;

                    if (CivilianToReport.EverSeenPlayer && CivilianToReport.ClosestDistanceToPlayer <= 20f)
                        Investigation.HavePlayerDescription = true;

                    if (CivilianToReport.EverSeenPlayer)
                        Investigation.InvestigationPosition = CivilianToReport.PositionLastSeenPlayer;
                    else if (CivilianToReport.PositionLastSeenCrime != Vector3.Zero)
                        Investigation.InvestigationPosition = CivilianToReport.PositionLastSeenCrime;
                    else
                        Investigation.InvestigationPosition = CivilianToReport.Pedestrian.Position;
                }
                else
                {
                    if (PlayerState.AreStarsGreyedOut)
                    {
                        Vector3 UpdatedPosition;
                        if (CivilianToReport.EverSeenPlayer)
                            UpdatedPosition = CivilianToReport.PositionLastSeenPlayer;
                        else if (CivilianToReport.PositionLastSeenCrime != Vector3.Zero && CivilianToReport.PositionLastSeenCrime != Vector3.Zero)
                            UpdatedPosition = CivilianToReport.PositionLastSeenCrime;
                        else
                            UpdatedPosition = CivilianToReport.Pedestrian.Position;

                        Police.PlaceLastSeenPlayer = UpdatedPosition;
                    }
                }
            }
            if (CivilianToReport.Pedestrian.Exists())
                CivilianToReport.Pedestrian.IsPersistent = false;
            CiviliansReportingCrimes--;

        }, "CrimeCalledInByCivilians");
        Debugging.GameFibers.Add(CrimeReportedFiber);
    }
    private static void PickReactTask(PedExt Snitch)
    {
        Debugging.WriteToLog("Tasking", string.Format("PickReactTask Handle: {0}", Snitch.Pedestrian.Handle));
        if (!Snitch.CrimesWitnessed.Any(x => x.WillScareCivilians))
        {
            if (!Snitch.Pedestrian.IsInAnyVehicle(false))
            {
                NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", Snitch.Pedestrian, Game.LocalPlayer.Character, 15000, 2048, 3);
            }
        }
        else
        {
            if (Snitch.Pedestrian.IsInAnyVehicle(false))
            {
                if (General.MyRand.Next(1, 11) <= 7 && Snitch.Pedestrian.IsDriver())
                {
                   // Debugging.WriteToLog("PickReactTask", string.Format("Flee 1 Handle: {0}", Snitch.Pedestrian.Handle));
                    Snitch.Pedestrian.Tasks.Flee(Game.LocalPlayer.Character, 100f, -1);
                }
                else if (Snitch.Pedestrian.IsInAnyVehicle(false) && Snitch.Pedestrian.CurrentVehicle.Speed == 0f)
                {
                   // Debugging.WriteToLog("PickReactTask", string.Format("Flee 2 Handle: {0}", Snitch.Pedestrian.Handle));
                    Snitch.Pedestrian.Tasks.Flee(Game.LocalPlayer.Character, 100f, -1);
                }
            }
            else
            {
                if (Snitch.CrimesWitnessed.Any(x => x.WillAngerCivilians) && !Snitch.CrimesWitnessed.Any(x => !x.WillAngerCivilians))
                {
                    int Random = General.MyRand.Next(1, 11);
                    if (Snitch.WillFight) //atack player
                    {
                        NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Snitch.Pedestrian, 5, true);//BF_CanFightArmedPedsWhenNotArmed = 5,
                                                                                                                 // NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Snitch.Pedestrian, 46, true);//BF_AlwaysFight = 46,
                        if (General.MyRand.Next(1, 2) <= 1)
                        {
                            GTAWeapon GunToGive = Weapons.GetRandomRegularWeaponByCategory(new List<GTAWeapon.WeaponCategory>() { GTAWeapon.WeaponCategory.Pistol,GTAWeapon.WeaponCategory.Shotgun,GTAWeapon.WeaponCategory.Melee }.PickRandom());
                            Snitch.Pedestrian.Inventory.GiveNewWeapon(GunToGive.Name, GunToGive.AmmoAmount, true);
                        }
                        Snitch.Pedestrian.Tasks.FightAgainst(Game.LocalPlayer.Character);
                        Snitch.Pedestrian.KeepTasks = true;
                    }
                    else if (Random <= 5)
                    {
                        Snitch.Pedestrian.Tasks.ReactAndFlee(Game.LocalPlayer.Character);
                    }
                    else if (Random <= 9)
                    {
                        NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", Snitch.Pedestrian, Game.LocalPlayer.Character, -1, 2048, 3);
                    }
                    else
                    {
                        Snitch.Pedestrian.Tasks.Cower(-1);
                    }
                }
                else //regular react
                {
                    if (General.MyRand.Next(1, 11) <= 9)
                    {
                        Snitch.Pedestrian.Tasks.ReactAndFlee(Game.LocalPlayer.Character);
                    }
                    else
                    {
                        Snitch.Pedestrian.Tasks.Cower(-1);
                    }
                }
            }
        }
    }
    private static void ArmCopAppropriately(Cop Cop)
    {
        if (WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.UnarmedChase)
        {
            SetCopTazer(Cop);
        }
        else if (WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.CautiousChase)
        {
            SetCopTazer(Cop);
        }
        //else if (WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.ArrestedWait && WantedLevelScript.LastPoliceState == WantedLevelScript.PoliceState.UnarmedChase)
        //{
        //    SetCopTazer(Cop);
        //}
        else if (WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.DeadlyChase)
        {
            SetCopDeadly(Cop);
        }
    }
    private static void TurnOffSiren(Cop Cop)
    {
        if (Cop != null && Cop.Pedestrian.Exists() && Cop.Pedestrian.IsDriver() && Cop.Pedestrian.CurrentVehicle.Exists() && Cop.Pedestrian.CurrentVehicle.HasSiren && Cop.Pedestrian.CurrentVehicle.IsSirenOn)
        {
            Cop.Pedestrian.CurrentVehicle.IsSirenOn = false;
            Cop.Pedestrian.CurrentVehicle.IsSirenSilent = false;
        }
    }
    private static void SurrenderBustEvent()
    {
        if (Game.LocalPlayer.WantedLevel == 0)
        {
            SetSurrenderBust(false, "Reset SurrenderBustEvent Wanted = 0");
            SurrenderBust = false;
        }
        else
        {
            PlayerState.StartArrestManual();
            WantedLevelScript.CurrentPoliceState = WantedLevelScript.PoliceState.ArrestedWait;
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)2725352035, true);
            PlayerState.HandsAreUp = false;
            SetSurrenderBust(false, "Reset SurrenderBustEvent");
            LastBust = Game.GameTime;
            Debugging.WriteToLog("Tasking", "SurrenderBust Executed");
        }
    }
    private static void SetUnarmed(Cop Cop)
    {
        if (!Cop.Pedestrian.Exists() || (Cop.IsSetUnarmed && !Cop.NeedsWeaponCheck))
            return;
        if (General.MySettings.Police.OverridePoliceAccuracy)
            Cop.Pedestrian.Accuracy = General.MySettings.Police.PoliceGeneralAccuracy;

        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.Pedestrian, 0);
        if (!(Cop.Pedestrian.Inventory.EquippedWeapon == null))
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Cop.Pedestrian, (uint)2725352035, true); //Unequip weapon so you don't get shot
            NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.Pedestrian, false);
        }
        NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.Pedestrian, 2, false);//cant do drivebys
        Cop.IsSetTazer = false;
        Cop.IsSetUnarmed = true;
        Cop.IsSetDeadly = false;
        Cop.GameTimeLastWeaponCheck = Game.GameTime;
    }
    private static void SetCopDeadly(Cop Cop)
    {
        if (!Cop.Pedestrian.Exists() || (Cop.IsSetDeadly && !Cop.NeedsWeaponCheck))
            return;
        if (General.MySettings.Police.OverridePoliceAccuracy)
            Cop.Pedestrian.Accuracy = General.MySettings.Police.PoliceGeneralAccuracy;
        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.Pedestrian, 30);
        if (!Cop.Pedestrian.Inventory.Weapons.Contains(Cop.IssuedPistol.Name))
            Cop.Pedestrian.Inventory.GiveNewWeapon(Cop.IssuedPistol.Name, -1, true);

        if ((Cop.Pedestrian.Inventory.EquippedWeapon == null || Cop.Pedestrian.Inventory.EquippedWeapon.Hash == WeaponHash.StunGun) && Game.LocalPlayer.WantedLevel >= 0)
            Cop.Pedestrian.Inventory.GiveNewWeapon(Cop.IssuedPistol.Name, -1, true);

        if (Cop.IssuedHeavyWeapon != null)
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Cop.Pedestrian, NativeFunction.CallByName<bool>("GET_BEST_PED_WEAPON", Cop.Pedestrian, 0),true);
        }

        if (General.MySettings.Police.AllowPoliceWeaponVariations)
            General.ApplyWeaponVariation(Cop.Pedestrian, (uint)Cop.IssuedPistol.Hash, Cop.PistolVariation);
        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.Pedestrian, true);
        NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.Pedestrian, 2, true);//can do drivebys

        Cop.IsSetTazer = false;
        Cop.IsSetUnarmed = false;
        Cop.IsSetDeadly = true;
        Cop.GameTimeLastWeaponCheck = Game.GameTime;
    }
    private static void SetCopTazer(Cop Cop)
    {
        if (!Cop.Pedestrian.Exists() || (Cop.IsSetTazer && !Cop.NeedsWeaponCheck))
            return;

        if (General.MySettings.Police.OverridePoliceAccuracy)
            Cop.Pedestrian.Accuracy = General.MySettings.Police.PoliceTazerAccuracy;
        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Cop.Pedestrian, 100);
        if (!Cop.Pedestrian.Inventory.Weapons.Contains(WeaponHash.StunGun))
        {
            Cop.Pedestrian.Inventory.GiveNewWeapon(WeaponHash.StunGun, 100, true);
        }
        else if (Cop.Pedestrian.Inventory.EquippedWeapon != WeaponHash.StunGun)
        {
            Cop.Pedestrian.Inventory.EquippedWeapon = WeaponHash.StunGun;
        }
        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Cop.Pedestrian, false);
        NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Cop.Pedestrian, 2, false);//cant do drivebys
        Cop.IsSetTazer = true;
        Cop.IsSetUnarmed = false;
        Cop.IsSetDeadly = false;
        Cop.GameTimeLastWeaponCheck = Game.GameTime;
    }
    private static void SetChaseStatus(TaskableCop Cop)
    {
        if (PlayerState.IsWanted)
        {
            if (Cop.TaskGTACop.DistanceToPlayer <= Police.ActiveDistance)
            {
                Cop.CurrentChaseStatus = ChaseStatus.Active;
            }
            else
            {
                Cop.CurrentChaseStatus = ChaseStatus.Idle;
            }
        }
        else if (Investigation.InInvestigationMode)
        {
            float DistToInvest = Cop.TaskGTACop.Pedestrian.DistanceTo2D(Investigation.InvestigationPosition);
            if (DistToInvest <= Investigation.InvestigationDistance)
            {
                Cop.CurrentChaseStatus = ChaseStatus.Investigation;
            }
            else
            {
                Cop.CurrentChaseStatus = ChaseStatus.Idle;
            }
        }
        else
        {
            Cop.CurrentChaseStatus = ChaseStatus.Idle;
        }

    }
    private static void SetSurrenderBust(bool ValueToSet, string DebugReason)
    {
        SurrenderBust = ValueToSet;
        Debugging.WriteToLog("Tasking", string.Format("Set Surrender Bust Reason: {0}", DebugReason));
    }
    public static void OutputTasks()
    {
        Debugging.WriteToLog("Tasking", "--------------------------------");
        foreach (CopTaskQueueItem QueueItem in CopTaskQueue)
        {
            bool IsIdle = false;
            if (QueueItem.TaskToRun == Idle)
                IsIdle = true;
            Debugging.WriteToLog("Tasking", string.Format("     Cop: {0} {1}  IsIdle {2}", QueueItem.TaskedCopToAssign.TaskGTACop.Pedestrian.Handle, QueueItem.TaskName, IsIdle));
        }
        foreach (CivilianTaskQueueItem QueueItem in CivilianTaskQueue)
        {
            Debugging.WriteToLog("Tasking", string.Format("     Ped: {0} {1}", QueueItem.TaskedCivilianToAssign.TaskGTAPed.Pedestrian.Handle, QueueItem.TaskName));
        }

        Debugging.WriteToLog("Tasking", "--------------------------------");
        foreach (TaskableCop Cop in TaskableCops)
        {
            string IsRunningName = "";
            if (Cop.RunningTask != null)
                IsRunningName = Cop.RunningTask.Method.Name;
            Debugging.WriteToLog("Tasking", string.Format("     Cop: {0} {1} {2} {3}", Cop.TaskGTACop.Pedestrian.Handle, Cop.IsTasked, IsRunningName,Cop.CurrentChaseStatus));
        }
        //foreach (TaskableCivilian Civilian in TaskableCivilians.OrderBy(x => x.TaskGTAPed.DistanceToPlayer))
        //{
        //    string IsRunningName = "";
        //    if (Civilian.RunningTask != null)
        //        IsRunningName = Civilian.RunningTask.Method.Name;
        //    Debugging.WriteToLog("      ", string.Format("Ped: {0} CanSee {1}, CanRecognize {2}, CanTask {3},CountCrimes {4}, TimeBehind  {5}, IsTasked {6}, Distance {7}, TaskName {8}", Civilian.TaskGTAPed.Pedestrian.Handle, Civilian.TaskGTAPed.CanSeePlayer, Civilian.TaskGTAPed.CanRecognizePlayer, Civilian.TaskGTAPed.CanBeTasked, Civilian.TaskGTAPed.CrimesWitnessed.Count, Civilian.TaskGTAPed.TimeBehindPlayer, Civilian.IsTasked, Civilian.TaskGTAPed.DistanceToPlayer, IsRunningName));
        //}
        Debugging.WriteToLog("Tasking", "--------------------------------");
    }
    private class CopTaskQueueItem
    {
        public TaskableCop TaskedCopToAssign { get; set; }
        public uint GameTimeAssigned { get; set; }
        public Action<TaskableCop> TaskToRun { get; set; }
        public string TaskName { get; set; }
        public CopTaskQueueItem(TaskableCop _TaskedPedToAssign, Action<TaskableCop> _TaskToRun, string _TaskName)
        {
            TaskedCopToAssign = _TaskedPedToAssign;
            TaskToRun = _TaskToRun;
            TaskName = _TaskName;
        }
    }
    private class CivilianTaskQueueItem
    {
        public TaskableCivilian TaskedCivilianToAssign { get; set; }
        public uint GameTimeAssigned { get; set; }
        public Action<TaskableCivilian> TaskToRun { get; set; }
        public string TaskName { get; set; }
        public CivilianTaskQueueItem(TaskableCivilian _TaskedPedToAssign, Action<TaskableCivilian> _TaskToRun, string _TaskName)
        {
            TaskedCivilianToAssign = _TaskedPedToAssign;
            TaskToRun = _TaskToRun;
            TaskName = _TaskName;
        }
    }
    private class TaskableCop
    {
        public ChaseStatus CurrentChaseStatus { get; set; } = ChaseStatus.Idle;
        public Cop TaskGTACop { get; set; }
        public bool IsTasked { get; set; } = false;
        public bool TaskIsQueued { get; set; } = false;
        public Action<TaskableCop> RunningTask { get; set; }
        public string SubTaskName { get; set; } = "";
        public GameFiber TaskFiber { get; set; }
        public uint GameTimeLastTasked { get; set; }
        public TaskableCop(Cop _GTAPedToTask)
        {
            TaskGTACop = _GTAPedToTask;
        }
    }
    private class TaskableCivilian
    {
        public PedExt TaskGTAPed { get; set; }
        public bool IsTasked { get; set; } = false;
        public bool TaskIsQueued { get; set; } = false;
        public Action<TaskableCivilian> RunningTask { get; set; }
        public GameFiber TaskFiber { get; set; }
        public uint GameTimeLastTasked { get; set; }
        public TaskableCivilian(PedExt _GTAPedToTask)
        {
            TaskGTAPed = _GTAPedToTask;
        }
    }
}

