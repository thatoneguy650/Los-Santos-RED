using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

//Needs Full Rewrite/Refactor
public class Tasking_Old
{
    private ITaskableWorld_Old World;
    private ITaskableTarget_Old CurrentPlayer;
    private ITargetable Player;
    private List<TaskableCop> TaskableCops = new List<TaskableCop>();
    private List<TaskableCivilian> TaskableCivilians = new List<TaskableCivilian>();

    public Tasking_Old(ITaskableWorld_Old world, ITaskableTarget_Old currentPlayer, ITargetable player)
    {
        World = world;
        CurrentPlayer = currentPlayer;
        Player = player;
    }

    public bool HasCopsInvestigating
    {
        get
        {
            if (TaskableCops.Any(x => x.CopToTask.Pedestrian.Exists() && x.IsInvestigating))
                return true;
            else
                return false;
        }
    }
    public void AddTaskablePeds()
    {
        World.PoliceList.RemoveAll(x => !x.Pedestrian.Exists());

        TaskableCops.RemoveAll(x => !x.CopToTask.Pedestrian.Exists());
        foreach (Cop Cop in World.PoliceList.Where(x => x.Pedestrian.Exists()))
        {
            if (!TaskableCops.Any(x => x.CopToTask.Pedestrian.Handle == Cop.Pedestrian.Handle))
            {
                TaskableCops.Add(new TaskableCop(World, CurrentPlayer, Cop));
            }
        }

        TaskableCivilians.RemoveAll(x => !x.CivilianToTask.Pedestrian.Exists());
        foreach (PedExt Civilian in World.CivilianList.Where(x => x.Pedestrian.Exists()))
        {
            if (!TaskableCivilians.Any(x => x.CivilianToTask.Pedestrian.Handle == Civilian.Pedestrian.Handle))
            {
                TaskableCivilians.Add(new TaskableCivilian(CurrentPlayer, Civilian));
            }
        }
    }
    public void TaskCops()
    {
        try
        {
            int TaskedCops = 0;
            foreach (TaskableCop Cop in TaskableCops.Where(x => x.CopToTask.Pedestrian.Exists()).OrderBy(x => x.CopToTask.DistanceToPlayer))//foreach (TaskableCop Cop in TaskableCops.Where(x => x.CopToTask.Pedestrian.Exists()).OrderBy(x => x.GameTimeLastRanActivity))
            {
                if (TaskedCops < 5)//2
                {
                    Cop.RunCurrentActivity();
                    TaskedCops++;
                }
                else
                {
                    break;
                }
            }
        }
        catch (Exception e)
        {
            //Game.Console.Print("RunActivities Error" + e.Message + " : " + e.StackTrace);
        }
    }
    public void TaskCivilians()
    {
        try
        {
            //TaskableCivilian Civilian = TaskableCivilians.Where(x => x.CivilianToTask.Pedestrian.Exists()).OrderBy(x => x.CivilianToTask.DistanceToPlayer).FirstOrDefault();
            //if(Civilian != null)
            //{
            //    Civilian.RunCurrentActivity();
            //}

            int TaskedCivilians = 0;
            foreach (TaskableCivilian Civilian in TaskableCivilians.Where(x => x.CivilianToTask.Pedestrian.Exists()).OrderBy(x => x.CivilianToTask.DistanceToPlayer))//foreach (TaskableCivilian Civilian in TaskableCivilians.Where(x => x.CivilianToTask.Pedestrian.Exists()).OrderBy(x => x.GameTimeLastRanActivity))
            {
                if (TaskedCivilians < 5)//5//4
                {
                    Civilian.RunCurrentActivity();
                    TaskedCivilians++;
                }
                else
                {
                    break;
                }
            }
        }
        catch (Exception e)
        {
            //Game.Console.Print("RunActivities Error" + e.Message + " : " + e.StackTrace);
        }
    }
    public void PrintActivities()//Debugging Proc
    {
        //Game.Console.Print("Tasking================================================");
        foreach (TaskableCop Cop in TaskableCops.Where(x => x.CopToTask.Pedestrian.Exists()))
        {
            //Game.Console.Print(string.Format("Distance: {0}, Handle: {1}, {2}", Cop.CopToTask.DistanceToPlayer, Cop.CopToTask.Pedestrian.Handle, Cop.DebugTaskState));
        }
        //Game.Console.Print("Tasking================================================");
    }
    public void UnTask()//Debugging Proc
    {
        //Game.Console.Print("Tasking================================================");
        foreach (TaskableCop Cop in TaskableCops.Where(x => x.CopToTask.Pedestrian.Exists()))
        {
            Cop.ClearTasks();
        }
        //Game.Console.Print("Tasking================================================");
    }
    private class TaskableCop
    {
        private ITaskableWorld_Old World;
        private ITaskableTarget_Old CurrentPlayer;
        private readonly float OnFootTaskDistance = 25f;//70f
        private Vector3 CurrentTaskedPosition = Vector3.Zero;
        private bool AtInvesstigationPositionThisInvestigation = false;
        private bool NearWantedCenterThisWanted = false;
        private string CurrentTaskLoop;
        private string CurrentSubTaskLoop;
        private bool ChasingVehicle;

        public bool IsInvestigating
        {
            get
            {
                if (CurrentTaskLoop == "Investigation")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public uint GameTimeLastRanActivity { get; set; }
        private bool WithinChaseDistance
        {
            get
            {
                if (CopToTask.DistanceToPlayer <= 400f + (CurrentPlayer.WantedLevel * 200f))
                    return true;
                else
                    return false;
            }
        }
        private bool WithinInvestigationDistance
        {
            get
            {
                if (CopToTask.DistanceToInvestigationPosition(CurrentPlayer.Investigations.Position) <= CurrentPlayer.Investigations.Distance)
                    return true;
                else
                    return false;
            }
        }
        private enum Activities
        {
            Investigate,
            Idle,
            Chase,
        }

        public Cop CopToTask { get; set; }
        public uint GameTimeLastTasked { get; set; }
        public string DebugTaskState
        {
            get
            {
                return string.Format("Dynamic: {0}      Activity: {1}       Loop: {2}       SubLoop: {3}        DistanceToPlayer: {4}       TimeTasked: {5}", CurrentDynamic.ToString(), CurrentActivity.ToString(), CurrentTaskLoop, CurrentSubTaskLoop, CopToTask.DistanceToPlayer, GameTimeLastTasked);
            }
        }
        private Activities CurrentActivity
        {
            get
            {
                if (CurrentPlayer.IsNotWanted)
                {
                    if (CurrentPlayer.Investigations.IsActive && WithinInvestigationDistance)
                    {
                        return Activities.Investigate;
                    }
                    else
                    {
                        return Activities.Idle;
                    }
                }
                else
                {
                    if (WithinChaseDistance)
                    {
                        return Activities.Chase;
                    }
                    else
                    {
                        return Activities.Idle;
                    }
                }
            }
        }
        private AIDynamic CurrentDynamic
        {
            get
            {
                if (CurrentPlayer.IsInVehicle)
                {
                    if (CopToTask.IsInVehicle)
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
                    if (CopToTask.IsInVehicle)
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
        public TaskableCop(ITaskableWorld_Old world, ITaskableTarget_Old currentPlayer, Cop _GTAPedToTask)
        {
            World = world;
            CurrentPlayer = currentPlayer;
            CopToTask = _GTAPedToTask;
        }
        public void RunCurrentActivity()
        {
            if (CopToTask.CanBeTasked)
            {
                GameTimeLastRanActivity = Game.GameTime;
                if (CurrentActivity == Activities.Idle)
                {
                    Idle();
                    return;
                }
                else if (CurrentActivity == Activities.Investigate)
                {
                    Investigate();
                    return;
                }
                else if (CurrentActivity == Activities.Chase)
                {
                    Chase();
                    return;
                }
                CurrentTaskLoop = "None";
                CopToTask.Pedestrian.BlockPermanentEvents = false;
                CopToTask.Pedestrian.KeepTasks = false;
            }
        }
        private void Idle()
        {
            if (CurrentTaskLoop != "Idle")
            {
                Idle_Start();
            }
            else
            {
                Idle_Normal();
            }
        }
        private void Idle_Start()
        {
            if (CopToTask.Pedestrian.Exists())
            {
                ClearTasks();
                CopToTask.Pedestrian.BlockPermanentEvents = true;
                CopToTask.Pedestrian.KeepTasks = true;
                if (!CopToTask.Pedestrian.IsInAnyVehicle(false))
                {
                    Vehicle LastVehicle = CopToTask.Pedestrian.LastVehicle;
                    if (LastVehicle.Exists() && LastVehicle.IsDriveable && LastVehicle.FreeSeatsCount > 0)
                    {
                        //LastVehicle.LockStatus = (VehicleLockStatus)1;
                        unsafe
                        {
                            int lol = 0;
                            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                            NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, LastVehicle, -1, CopToTask.LastSeatIndex, 1f, 9);
                            NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(4000, 8000));
                            NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, LastVehicle, 18f, 183);
                            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", CopToTask.Pedestrian, lol);
                            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                        }
                    }
                    else
                    {
                        CopToTask.Pedestrian.Tasks.Wander();
                    }
                }
                else
                {
                    if (CopToTask.IsDriver)
                    {
                        CopToTask.Pedestrian.Tasks.CruiseWithVehicle(CopToTask.Pedestrian.CurrentVehicle, 15f, VehicleDrivingFlags.Normal);
                        CopToTask.Pedestrian.CurrentVehicle.IsSirenOn = false;
                    }
                }
                CurrentTaskLoop = "Idle";
                GameTimeLastTasked = Game.GameTime;
                //Game.Console.Print(string.Format("Tasking     Started Idle: {0} Old CurrentTaskLoop: {1}", CopToTask.Pedestrian.Handle, CurrentTaskLoop));
            }
        }
        private void Idle_Normal()
        {
            if (CopToTask.Pedestrian.Exists() && CopToTask.IsDriver && CopToTask.Pedestrian.CurrentVehicle.Exists() && CopToTask.Pedestrian.CurrentVehicle.HasSiren)
            {
                CopToTask.Pedestrian.CurrentVehicle.IsSirenOn = false;
                CopToTask.Pedestrian.CurrentVehicle.IsSirenSilent = false;
            }
            if (CopToTask.DistanceToPlayer <= 75f && CopToTask.Pedestrian.Tasks.CurrentTaskStatus == TaskStatus.NoTask && !CopToTask.Pedestrian.IsInAnyVehicle(false))//150f causes spam
            {
                Vehicle LastVehicle = CopToTask.Pedestrian.LastVehicle;
                if (LastVehicle.Exists() && LastVehicle.IsDriveable && LastVehicle.FreeSeatsCount > 0)
                {
                    //LastVehicle.LockStatus = (VehicleLockStatus)1;
                    unsafe
                    {
                        int lol = 0;
                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, LastVehicle, -1, CopToTask.LastSeatIndex, 0.5f, 9);
                        NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(4000, 8000));
                        NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, LastVehicle, 18f, 183);
                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", CopToTask.Pedestrian, lol);
                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    }
                }
                else
                {
                    CopToTask.Pedestrian.Tasks.Wander();
                }
                //Game.Console.Print(string.Format("Tasking     ReSet Idle: {0} CurrentTaskLoop: {1}", CopToTask.Pedestrian.Handle, CurrentTaskLoop));
            }
        }
        private void Investigate()
        {
            if (CopToTask.IsInVehicle || CopToTask.DistanceToLastSeen <= OnFootTaskDistance)//was investigation place
            {
                if (CurrentTaskLoop != "Investigation")
                {
                    Investigate_Start();
                }
                else
                {
                    Investigate_Normal();
                }
                GameTimeLastTasked = Game.GameTime;
            }
            CurrentTaskLoop = "Investigation";
        }
        private void Investigate_Start()
        {
            if (CurrentPlayer.Investigations.Position == Vector3.Zero)
            {
                CurrentPlayer.Investigations.Position = Game.LocalPlayer.Character.Position;
                if (CurrentPlayer.IsWanted)
                {
                    CurrentPlayer.Investigations.Position = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
                }
            }
            CurrentTaskedPosition = CurrentPlayer.Investigations.Position;
            CopToTask.Pedestrian.BlockPermanentEvents = false;
            if (CopToTask.Pedestrian.IsInAnyVehicle(false))
            {
                if (CopToTask.IsDriver)
                {
                    NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", CopToTask.Pedestrian, CopToTask.Pedestrian.CurrentVehicle, CurrentPlayer.Investigations.Position.X, CurrentPlayer.Investigations.Position.Y, CurrentPlayer.Investigations.Position.Z, CurrentPlayer.CurrentPoliceResponse.ResponseDrivingSpeed, 4 | 16 | 32 | 262144, 10f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, PositionOfInterest.X, PositionOfInterest.Y, PositionOfInterest.Z, 70f, 4 | 16 | 32 | 262144, 35f);
                }
            }
            else
            {
                NativeFunction.CallByName<bool>("TASK_GO_STRAIGHT_TO_COORD", CopToTask.Pedestrian, CurrentPlayer.Investigations.Position.X, CurrentPlayer.Investigations.Position.Y, CurrentPlayer.Investigations.Position.Z, 500f, -1, 0f, 2f);
            }
            CurrentTaskLoop = "Investigation";
            AtInvesstigationPositionThisInvestigation = false;
            //Game.Console.Print(string.Format("     Started Investigate: {0}, CurrentResponse {1}, DrivingSpeed {2}, NeedSirenOn {3}", CopToTask.Pedestrian.Handle, CurrentPlayer.CurrentPoliceResponse.CurrentResponse, CurrentPlayer.CurrentPoliceResponse.ResponseDrivingSpeed, CurrentPlayer.CurrentPoliceResponse.ShouldSirenBeOn));
        }
        private void Investigate_Normal()
        {
            if (!AtInvesstigationPositionThisInvestigation)
            {
                if (CurrentPlayer.Investigations.Position != Vector3.Zero && CurrentPlayer.Investigations.Position != CurrentTaskedPosition) //retask them if it changes
                {
                    CurrentTaskedPosition = CurrentPlayer.Investigations.Position;
                    if (CopToTask.Pedestrian.IsInAnyVehicle(false))
                    {
                        if (CopToTask.IsDriver)
                        {
                            NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", CopToTask.Pedestrian, CopToTask.Pedestrian.CurrentVehicle, CurrentPlayer.Investigations.Position.X, CurrentPlayer.Investigations.Position.Y, CurrentPlayer.Investigations.Position.Z, CurrentPlayer.CurrentPoliceResponse.ResponseDrivingSpeed, 4 | 16 | 32 | 262144, 10f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, PositionOfInterest.X, PositionOfInterest.Y, PositionOfInterest.Z, 70f, 4 | 16 | 32 | 262144, 35f);
                        }
                    }
                    else
                    {
                        NativeFunction.CallByName<bool>("TASK_GO_STRAIGHT_TO_COORD", CopToTask.Pedestrian, CurrentPlayer.Investigations.Position.X, CurrentPlayer.Investigations.Position.Y, CurrentPlayer.Investigations.Position.Z, 500f, -1, 0f, 2f);
                    }
                    //Game.Console.Print(string.Format("     Reset Investigate: {0}, CurrentResponse {1}, DrivingSpeed {2}, NeedSirenOn {3}", CopToTask.Pedestrian.Handle, CurrentPlayer.CurrentPoliceResponse.CurrentResponse, CurrentPlayer.CurrentPoliceResponse.ResponseDrivingSpeed, CurrentPlayer.CurrentPoliceResponse.ShouldSirenBeOn));

                }
                if (CopToTask.IsDriver && CopToTask.Pedestrian.CurrentVehicle != null && CopToTask.Pedestrian.CurrentVehicle.HasSiren && CurrentPlayer.CurrentPoliceResponse.ShouldSirenBeOn)
                {
                    if (!CopToTask.Pedestrian.CurrentVehicle.IsSirenOn)
                    {
                        CopToTask.Pedestrian.CurrentVehicle.IsSirenOn = true;
                        CopToTask.Pedestrian.CurrentVehicle.IsSirenSilent = false;
                    }
                }
                if (CopToTask.DistanceToInvestigationPosition(CurrentPlayer.Investigations.Position) <= 15f)
                {
                    AtInvesstigationPositionThisInvestigation = true;
                    if (CopToTask.Pedestrian.Exists())
                    {
                        if (CopToTask.Pedestrian.IsInAnyVehicle(false) && CopToTask.Pedestrian.CurrentVehicle.Exists())
                        {
                            NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", CopToTask.Pedestrian, CopToTask.Pedestrian.CurrentVehicle, CurrentPlayer.CurrentPoliceResponse.ResponseDrivingSpeed, 4 | 16 | 32 | 262144, 10f);
                        }
                        else
                        {
                            CopToTask.Pedestrian.Tasks.Wander();
                        }
                        //Game.Console.Print(string.Format("     Started Investigation Wander: {0}", CopToTask.Pedestrian.Handle));
                    }
                }
            }
        }
        private void Kill()
        {
            if (CurrentTaskLoop != "Kill")
            {
                Kill_Start();
            }
            else
            {

            }
        }
        private void Kill_Start()
        {
            ClearTasks();
            CopToTask.Pedestrian.Tasks.FightAgainstClosestHatedTarget(70f);
            CurrentTaskLoop = "Kill";
            GameTimeLastTasked = Game.GameTime;
            //Game.Console.Print(string.Format("     Started Kill: {0} Old CurrentTaskLoop: {1}", CopToTask.Pedestrian.Handle, CurrentTaskLoop));
        }
        private void Chase()
        {
            if (WithinChaseDistance)
            {
                if (CurrentPlayer.AnyPoliceRecentlySeenPlayer && !CurrentPlayer.IsInSearchMode)
                {
                    if (CurrentDynamic == AIDynamic.Cop_InVehicle_Player_InVehicle)
                    {
                        if (!CopToTask.IsInHelicopter)
                        {
                            //if (CopToTask.DistanceToPlayer <= 25f || CopToTask.CanSeePlayer)
                            //{
                            //    CurrentTaskLoop = "None";
                            //}
                            //else
                            //{
                                VehicleChase();
                            //}
                        }
                        else
                        {
                            HeliChase();
                        }
                    }
                    else if (CurrentDynamic == AIDynamic.Cop_InVehicle_Player_OnFoot)
                    {
                        //CurrentTaskLoop = "None";
                        //CarChase();?????
                        if (CopToTask.DistanceToPlayer >= 15F)
                        {
                            VehicleChase();
                        }
                    }
                    else if (CurrentDynamic == AIDynamic.Cop_OnFoot_Player_InVehicle)
                    {
                        if (CopToTask.DistanceToPlayer <= 10f)
                        {
                            CarJack();
                        }
                    }
                    else if (CurrentDynamic == AIDynamic.Cop_OnFoot_Player_OnFoot)
                    {
                        if (CopToTask.DistanceToPlayer <= OnFootTaskDistance || CopToTask.RecentlySeenPlayer)
                        {
                            if (CurrentPlayer.CurrentPoliceResponse.IsDeadlyChase && !CurrentPlayer.IsAttemptingToSurrender)
                                Kill();
                            else
                                FootChase();
                        }
                    }
                }
                else
                {
                    if (CopToTask.IsInVehicle)
                    {
                        if (!CopToTask.IsInHelicopter)
                        {
                            GoToLastSeen();
                        }
                        else
                        {
                            HeliGoToLastSeen();
                        }
                    }
                    else if (CopToTask.DistanceToLastSeen <= OnFootTaskDistance)
                    {
                        GoToLastSeen();
                    }
                }
                //if (ShouldBustPlayer)
                //{
                //    SurrenderBust = true;
                //    Mod.Debugging.WriteToLog("Tasking", string.Format("Surrender Bust Chase: {0}", CopToTask.Pedestrian.Handle));
                //}
            }

        }
        private void VehicleChase()
        {
            if (!CopToTask.Pedestrian.Exists() || !CopToTask.IsDriver)
                return;

            if (CurrentTaskLoop != "VehicleChase")
            {
                VehicleChase_Start();
            }
            else
            {
                VehicleChase_Normal();
            }
        }
        private void VehicleChase_Start()
        {
            CopToTask.Pedestrian.BlockPermanentEvents = true;//was false
            CopToTask.Pedestrian.KeepTasks = true;//wasnt here
            Vector3 WantedCenter = CurrentPlayer.PlacePoliceLastSeenPlayer;//NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
            if (CurrentPlayer.IsInVehicle)
            {
                ChasingVehicle = true;
                //NativeFunction.CallByName<bool>("TASK_VEHICLE_CHASE", CopToTask.Pedestrian, Game.LocalPlayer.Character); //NativeFunction.CallByName<bool>("TASK_VEHICLE_FOLLOW", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, Game.LocalPlayer.Character, 22f, 4 | 16 | 32 | 262144, 8f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_CHASE", Cop.Pedestrian, Game.LocalPlayer.Character);
                NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION", CopToTask.Pedestrian, CopToTask.Pedestrian.CurrentVehicle, Game.LocalPlayer.Character.CurrentVehicle, 7, 100f, 0f, 0f, 0f, true);
            }
            else
            {
                ChasingVehicle = false;
                //NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", CopToTask.Pedestrian, CopToTask.Pedestrian.CurrentVehicle, WantedCenter.X, WantedCenter.Y, WantedCenter.Z, 20f, 4 | 16 | 32 | 262144, 20f);
                NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION", CopToTask.Pedestrian, CopToTask.Pedestrian.CurrentVehicle, Game.LocalPlayer.Character, 7, 100f, 0f, 0f, 0f, true);
            }
            CurrentTaskedPosition = WantedCenter;
            CurrentTaskLoop = "VehicleChase";
            //Game.Console.Print(string.Format("     Started VehicleChase: {0}", CopToTask.Pedestrian.Handle));
        }
        private void VehicleChase_Normal()
        {
            //Vector3 WantedCenter = Police.PlaceLastSeenPlayer; //NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
            //if (CurrentTaskedPosition.DistanceTo2D(WantedCenter) >= 10f)
            //{
            //    if (!CurrentPlayer.IsInVehicle)
            //    {
            //        if (CopToTask.Pedestrian.CurrentVehicle.Exists())
            //        {
            //            NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", CopToTask.Pedestrian, CopToTask.Pedestrian.CurrentVehicle, WantedCenter.X, WantedCenter.Y, WantedCenter.Z, 20f, 4 | 16 | 32 | 262144, 20f);
            //            //Game.Console.Print(string.Format("     Updated VehicleChase: {0}", CopToTask.Pedestrian.Handle));
            //        }
            //    }
            //    CurrentTaskedPosition = WantedCenter;
            //}
            if(CurrentPlayer.IsInVehicle)
            {
                if(!ChasingVehicle)
                {
                    ChasingVehicle = true;
                    NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION", CopToTask.Pedestrian, CopToTask.Pedestrian.CurrentVehicle, Game.LocalPlayer.Character.CurrentVehicle, 7, 100f, 0f, 0f, 0f, true);
                    //Game.Console.Print(string.Format("     Updated VehicleChase To VehicleTarget: {0}", CopToTask.Pedestrian.Handle));
                }
            }
            else
            {
                if(ChasingVehicle)
                {
                    ChasingVehicle = false;
                    NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION", CopToTask.Pedestrian, CopToTask.Pedestrian.CurrentVehicle, Game.LocalPlayer.Character, 7, 100f, 0f, 0f, 0f, true);
                    //Game.Console.Print(string.Format("     Updated VehicleChase To Ped Target: {0}", CopToTask.Pedestrian.Handle));
                }
            }
            if (CopToTask.Pedestrian.CurrentVehicle.Exists() && CopToTask.Pedestrian.CurrentVehicle.HasSiren && !CopToTask.Pedestrian.CurrentVehicle.IsSirenOn)
            {
                CopToTask.Pedestrian.CurrentVehicle.IsSirenOn = true;
                CopToTask.Pedestrian.CurrentVehicle.IsSirenSilent = false;
            }
        }
        private void HeliChase()
        {
            if (!CopToTask.Pedestrian.Exists() || !CopToTask.IsDriver || !CopToTask.IsInHelicopter)
                return;

            if (CurrentTaskLoop != "HeliChase")
            {
                NativeFunction.CallByName<bool>("TASK_HELI_CHASE", CopToTask.Pedestrian, Game.LocalPlayer.Character, -50f, 50f, 60f);
                CurrentTaskLoop = "HeliChase";
                GameTimeLastTasked = Game.GameTime;
                //Game.Console.Print(string.Format("     Started HeliChase: {0}", CopToTask.Pedestrian.Handle));
            }
        }
        private void CarJack()
        {
            if (CurrentTaskLoop != "CarJack")
            {
                if (CopToTask.Pedestrian.Exists())
                {
                    if (CurrentPlayer.IsInVehicle && Game.LocalPlayer.Character.CurrentVehicle.Exists())
                    {
                        NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", CopToTask.Pedestrian, Game.LocalPlayer.Character.CurrentVehicle, -1, -1, 2f, 9);
                    }
                    //Game.Console.Print(string.Format("     Started CarJack: {0} Old CurrentTaskLoop: {1}", CopToTask.Pedestrian.Handle, CurrentTaskLoop));
                }
            }
            CurrentTaskLoop = "CarJack";
            GameTimeLastTasked = Game.GameTime;
        }
        private void FootChase()
        {
            if (!CopToTask.Pedestrian.Exists())
                return;

            if (CurrentTaskLoop != "FootChase")
            {
                FootChase_Start();
            }
            else
            {
                FootChase_Normal();
            }
        }
        private void FootChase_Start()
        {
            double cool = RandomItems.MyRand.NextDouble() * (1.175 - 1.1) + 1.1;//(1.17 - 1.075) + 1.075;//(1.175 - 1.1) + 1.1;
            float MoveRate = (float)cool;
            NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_CLIMBOVERS", CopToTask.Pedestrian, true);
            NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_LADDERS", CopToTask.Pedestrian, true);
            NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_DROP_FROM_HEIGHT", CopToTask.Pedestrian, true);
            CopToTask.Pedestrian.BlockPermanentEvents = true;
            CopToTask.Pedestrian.KeepTasks = true;
            if (CurrentPlayer.WantedLevel >= 2)
                NativeFunction.CallByName<uint>("SET_PED_MOVE_RATE_OVERRIDE", CopToTask.Pedestrian, MoveRate);
            CurrentTaskLoop = "FootChase";
            CurrentSubTaskLoop = "";
            //Game.Console.Print(string.Format("     Started FootChase: {0}", CopToTask.Pedestrian.Handle));
            FootChase_Normal();
        }
        private void FootChase_Normal()
        {
            CopToTask.Pedestrian.BlockPermanentEvents = true;
            CopToTask.Pedestrian.KeepTasks = true;



            //GET_SCRIPT_TASK_STATUS
            //uses eScriptTaskHash
            //maybe is just the status?
            //Can be used to get the actual task assigned instead of using the subtask strings?


            if (CurrentSubTaskLoop != "Shoot" && (!CurrentPlayer.IsBusted && !CurrentPlayer.IsAttemptingToSurrender) && CopToTask.DistanceToPlayer <= 7f)
            {
                //Game.Console.Print(string.Format("     FootChase Shoot: {0}", CopToTask.Pedestrian.Handle));
                CurrentSubTaskLoop = "Shoot";
                unsafe
                {
                    int lol = 0;
                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                    NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY", 0, Game.LocalPlayer.Character, Game.LocalPlayer.Character, 200f, true, 4.0f, 200f, false, false, (uint)FiringPattern.DelayFireByOneSecond);
                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", CopToTask.Pedestrian, lol);
                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                }
            }
            else if (CurrentSubTaskLoop != "Aim" && (CurrentPlayer.IsBusted || CurrentPlayer.IsAttemptingToSurrender) && CopToTask.DistanceToPlayer <= 7f)
            {
                //Game.Console.Print(string.Format("     FootChase Aim: {0}", CopToTask.Pedestrian.Handle));
                CurrentSubTaskLoop = "Aim";
                unsafe
                {
                    int lol = 0;
                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                    NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", 0, Game.LocalPlayer.Character, 4f, 20f);
                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", CopToTask.Pedestrian, lol);
                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                }
            }
            else if (CurrentSubTaskLoop != "GoTo" && CopToTask.DistanceToPlayer >= 15f)
            {
                //Game.Console.Print(string.Format("     FootChase GoTo: {0}", CopToTask.Pedestrian.Handle));
                CurrentSubTaskLoop = "GoTo";
                unsafe
                {
                    int lol = 0;
                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                    NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Game.LocalPlayer.Character, -1, 7f, 500f, 1073741824, 1); //Original and works ok
                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", CopToTask.Pedestrian, lol);
                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                }
            }
        }
        private void GoToLastSeen()
        {
            if (!CopToTask.Pedestrian.Exists() || !CopToTask.IsDriver)
                return;

            if (CurrentTaskLoop != "GoToLastSeen")
            {
                GoToLastSeen_Start();
            }
            else
            {
                GoToLastSeen_Normal();
            }
        }
        private void GoToLastSeen_Start()
        {
            CopToTask.Pedestrian.BlockPermanentEvents = false;
            Vector3 WantedCenter = CurrentPlayer.PlacePoliceLastSeenPlayer; //NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
            if (CopToTask.IsInVehicle && CopToTask.Pedestrian.CurrentVehicle != null)
            {
                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", CopToTask.Pedestrian, CopToTask.Pedestrian.CurrentVehicle, WantedCenter.X, WantedCenter.Y, WantedCenter.Z, 20f, 4 | 16 | 32 | 262144, 20f);
            }
            else
            {
                CopToTask.Pedestrian.Tasks.GoStraightToPosition(WantedCenter, 15f, 0f, 2f, 0);
            }
            CurrentTaskedPosition = WantedCenter;
            NearWantedCenterThisWanted = false;
            CurrentTaskLoop = "GoToLastSeen";
            //Game.Console.Print(string.Format("     Started GoToLastSeen: {0}", CopToTask.Pedestrian.Handle));
        }
        private void GoToLastSeen_Normal()
        {
            Vector3 WantedCenter = CurrentPlayer.PlacePoliceLastSeenPlayer; //NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
            if (!NearWantedCenterThisWanted)
            {
                if (CurrentTaskedPosition.DistanceTo2D(WantedCenter) >= 5f)
                {
                    if (CopToTask.IsInVehicle && CopToTask.Pedestrian.IsInAnyVehicle(false))
                    {
                        NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", CopToTask.Pedestrian, CopToTask.Pedestrian.CurrentVehicle, WantedCenter.X, WantedCenter.Y, WantedCenter.Z, 20f, 4 | 16 | 32 | 262144, 20f);
                    }
                    else
                    {
                        CopToTask.Pedestrian.Tasks.GoStraightToPosition(WantedCenter, 15f, 0f, 2f, 0);
                    }
                    CurrentTaskedPosition = WantedCenter;
                    CurrentTaskLoop = "GoToLastSeen";
                    //Game.Console.Print(string.Format("     Updated GoToLastSeen: {0}", CopToTask.Pedestrian.Handle));
                }
                if (CopToTask.DistanceToLastSeen <= 25f)
                {
                    NearWantedCenterThisWanted = true;
                    if (CopToTask.IsDriver && CopToTask.IsInVehicle)
                    {
                        CopToTask.Pedestrian.Tasks.CruiseWithVehicle(30f, VehicleDrivingFlags.Emergency);
                    }
                    else if (!CopToTask.IsInVehicle)
                    {
                        CopToTask.Pedestrian.Tasks.Wander();
                    }
                    //Game.Console.Print(string.Format("     Post GoToLastSeen: {0}", CopToTask.Pedestrian.Handle));
                }
            }
        }
        private void HeliGoToLastSeen()
        {
            if (!CopToTask.Pedestrian.Exists() || !CopToTask.IsDriver || !CopToTask.IsInHelicopter)
                return;

            if (CurrentTaskLoop != "HeliGoToLastSeen")
            {
                HeliGoToLastSeen_Start();
            }
            else
            {
                HeliGoToLastSeen_Normal();
            }
        }
        private void HeliGoToLastSeen_Start()
        {
            Cop ClosestCop = World.PoliceList.Where(x => x.Pedestrian.Exists() && x.IsDriver).OrderBy(x => x.DistanceToLastSeen).FirstOrDefault();
            if (ClosestCop == null)
                return;
            NativeFunction.CallByName<bool>("TASK_HELI_CHASE", CopToTask.Pedestrian, ClosestCop.Pedestrian, -50f, 50f, 60f);
            //Game.Console.Print(string.Format("     Heli Lost you following closest cop: {0}", CopToTask.Pedestrian.Handle));
            NearWantedCenterThisWanted = false;
            CurrentTaskLoop = "HeliGoToLastSeen";
        }
        private void HeliGoToLastSeen_Normal()
        {

        }
        public void ClearTasks()//temp public
        {
            if (CopToTask.Pedestrian.Exists())
            {
                int seatIndex = 0;
                Vehicle CurrentVehicle = null;
                bool WasInVehicle = false;
                if (CopToTask.WasModSpawned && CopToTask.Pedestrian.IsInAnyVehicle(false))
                {
                    WasInVehicle = true;
                    CurrentVehicle = CopToTask.Pedestrian.CurrentVehicle;
                    seatIndex = CopToTask.Pedestrian.SeatIndex;
                }
                CopToTask.Pedestrian.Tasks.Clear();

                CopToTask.Pedestrian.BlockPermanentEvents = false;
                CopToTask.Pedestrian.KeepTasks = false;
                CopToTask.Pedestrian.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Player, Relationship.Neutral);

                if (!CopToTask.WasModSpawned)
                    CopToTask.Pedestrian.IsPersistent = false;

                if (CopToTask.WasModSpawned && WasInVehicle && !CopToTask.Pedestrian.IsInAnyVehicle(false) && CurrentVehicle != null)
                {
                    CopToTask.Pedestrian.WarpIntoVehicle(CurrentVehicle, seatIndex);

                }
                if (CopToTask.IsDriver && CopToTask.Pedestrian.CurrentVehicle != null && CopToTask.Pedestrian.CurrentVehicle.HasSiren)
                {
                    CopToTask.Pedestrian.CurrentVehicle.IsSirenOn = false;
                    CopToTask.Pedestrian.CurrentVehicle.IsSirenSilent = false;
                }
                if (CurrentPlayer.IsWanted)
                {
                    NativeFunction.CallByName<bool>("SET_PED_ALERTNESS", CopToTask.Pedestrian, 3);
                    if (CurrentPlayer.CurrentPoliceResponse.IsDeadlyChase)
                    {
                        CopToTask.Pedestrian.Tasks.FightAgainst(Game.LocalPlayer.Character, -1);
                    }
                }

                CurrentTaskLoop = "None";
                CurrentSubTaskLoop = "";

                //Game.Console.Print(string.Format("     ClearedTasks: {0}", CopToTask.Pedestrian.Handle));
            }

        }

        //    private class InvestigateActivity//trying to move the spaghetti above into classes as proper
        //    {
        //        private readonly float OnFootTaskDistance = 25f;//70f
        //        private Vector3 CurrentTaskedPosition = Vector3.Zero;
        //        private bool AtInvesstigationPositionThisInvestigation = false;
        //        private string CurrentTaskLoop;
        //        public bool IsInvestigating
        //        {
        //            get
        //            {
        //                if (CurrentTaskLoop == "Investigation")
        //                {
        //                    return true;
        //                }
        //                else
        //                {
        //                    return false;
        //                }
        //            }
        //        }
        //        private Cop CopToTask { get; set; }
        //        public uint GameTimeLastTasked { get; set; }
        //        private void Investigate()
        //        {
        //            float DisanceToInvestigation = CopToTask.Pedestrian.DistanceTo2D(CurrentPlayer.Investigations.InvestigationPosition);
        //            if (DisanceToInvestigation <= CurrentPlayer.Investigations.InvestigationDistance)
        //            {
        //                if (CopToTask.IsInVehicle || DisanceToInvestigation <= OnFootTaskDistance)
        //                {
        //                    if (!HasStarted)
        //                    {
        //                        Investigate_Start();
        //                    }
        //                    else
        //                    {
        //                        Investigate_Normal();
        //                    }
        //                    GameTimeLastTasked = Game.GameTime;
        //                }
        //            }
        //            CurrentTaskLoop = "Investigation";
        //        }
        //        private void Investigate_Start()
        //        {
        //            if (CurrentPlayer.Investigations.InvestigationPosition == Vector3.Zero)
        //            {
        //                CurrentPlayer.Investigations.InvestigationPosition = Game.LocalPlayer.Character.Position;
        //                if (CurrentPlayer.IsWanted)
        //                    CurrentPlayer.Investigations.InvestigationPosition = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
        //            }
        //            CurrentTaskedPosition = CurrentPlayer.Investigations.InvestigationPosition;
        //            CopToTask.Pedestrian.BlockPermanentEvents = false;
        //            if (CopToTask.Pedestrian.IsInAnyVehicle(false))
        //                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", CopToTask.Pedestrian, CopToTask.Pedestrian.CurrentVehicle, CurrentPlayer.Investigations.InvestigationPosition.X, CurrentPlayer.Investigations.InvestigationPosition.Y, CurrentPlayer.Investigations.InvestigationPosition.Z, CurrentPlayer.CurrentPoliceResponse.ResponseDrivingSpeed, 4 | 16 | 32 | 262144, 10f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, PositionOfInterest.X, PositionOfInterest.Y, PositionOfInterest.Z, 70f, 4 | 16 | 32 | 262144, 35f);
        //            else
        //                NativeFunction.CallByName<bool>("TASK_GO_STRAIGHT_TO_COORD", CopToTask.Pedestrian, CurrentPlayer.Investigations.InvestigationPosition.X, CurrentPlayer.Investigations.InvestigationPosition.Y, CurrentPlayer.Investigations.InvestigationPosition.Z, 500f, -1, 0f, 2f);
        //            CurrentTaskLoop = "Investigation";
        //            AtInvesstigationPositionThisInvestigation = false;
        //            //Game.Console.Print(string.Format("     Started Investigate: {0}, CurrentResponse {1}, DrivingSpeed {2}, NeedSirenOn {3}", CopToTask.Pedestrian.Handle, CurrentPlayer.CurrentPoliceResponse.CurrentResponse, CurrentPlayer.CurrentPoliceResponse.ResponseDrivingSpeed, CurrentPlayer.CurrentPoliceResponse.ShouldSirenBeOn));
        //        }
        //        private void Investigate_Normal()
        //        {
        //            if (!AtInvesstigationPositionThisInvestigation)
        //            {
        //                if (CurrentPlayer.Investigations.InvestigationPosition != Vector3.Zero && CurrentPlayer.Investigations.InvestigationPosition != CurrentTaskedPosition) //retask them if it changes
        //                {
        //                    CurrentTaskedPosition = CurrentPlayer.Investigations.InvestigationPosition;
        //                    if (CopToTask.Pedestrian.IsInAnyVehicle(false))
        //                        NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", CopToTask.Pedestrian, CopToTask.Pedestrian.CurrentVehicle, CurrentPlayer.Investigations.InvestigationPosition.X, CurrentPlayer.Investigations.InvestigationPosition.Y, CurrentPlayer.Investigations.InvestigationPosition.Z, CurrentPlayer.CurrentPoliceResponse.ResponseDrivingSpeed, 4 | 16 | 32 | 262144, 10f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, PositionOfInterest.X, PositionOfInterest.Y, PositionOfInterest.Z, 70f, 4 | 16 | 32 | 262144, 35f);
        //                    else
        //                        NativeFunction.CallByName<bool>("TASK_GO_STRAIGHT_TO_COORD", CopToTask.Pedestrian, CurrentPlayer.Investigations.InvestigationPosition.X, CurrentPlayer.Investigations.InvestigationPosition.Y, CurrentPlayer.Investigations.InvestigationPosition.Z, 500f, -1, 0f, 2f);

        //                    //Game.Console.Print(string.Format("     Reset Investigate: {0}, CurrentResponse {1}, DrivingSpeed {2}, NeedSirenOn {3}", CopToTask.Pedestrian.Handle, CurrentPlayer.CurrentPoliceResponse.CurrentResponse, CurrentPlayer.CurrentPoliceResponse.ResponseDrivingSpeed, CurrentPlayer.CurrentPoliceResponse.ShouldSirenBeOn));

        //                }
        //                if (CopToTask.IsDriver && CopToTask.Pedestrian.CurrentVehicle != null && CopToTask.Pedestrian.CurrentVehicle.HasSiren && CurrentPlayer.CurrentPoliceResponse.ShouldSirenBeOn)
        //                {
        //                    if (!CopToTask.Pedestrian.CurrentVehicle.IsSirenOn)
        //                    {
        //                        CopToTask.Pedestrian.CurrentVehicle.IsSirenOn = true;
        //                        CopToTask.Pedestrian.CurrentVehicle.IsSirenSilent = false;
        //                    }
        //                }
        //                if (CopToTask.Pedestrian.DistanceTo2D(CurrentPlayer.Investigations.InvestigationPosition) <= 15f)
        //                {
        //                    AtInvesstigationPositionThisInvestigation = true;
        //                    if (CopToTask.Pedestrian.Exists() && CopToTask.Pedestrian.CurrentVehicle.Exists())
        //                    {
        //                        NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", CopToTask.Pedestrian, CopToTask.Pedestrian.CurrentVehicle, CurrentPlayer.CurrentPoliceResponse.ResponseDrivingSpeed, 4 | 16 | 32 | 262144, 10f);
        //                        //Game.Console.Print(string.Format("     Started Investigation Wander: {0}", CopToTask.Pedestrian.Handle));
        //                    }
        //                }
        //            }
        //        }
        //    }
    }
    private class TaskableCivilian
    {
        private ITaskableTarget_Old CurrentPlayer;
        private enum Activities
        {
            Idle,
            ReactToCrime,
        }
        private string CurrentTaskLoop;
        private string CurrentSubTaskLoop;
        public PedExt CivilianToTask { get; set; }
        public uint GameTimeLastRanActivity { get; set; }
        public uint GameTimeLastTasked { get; set; }
        public string DebugTaskState
        {
            get
            {
                return string.Format("Loop: {0} DistanceToPlayer: {1} TimeTasked: {2}", CurrentTaskLoop, CivilianToTask.DistanceToPlayer, GameTimeLastTasked);
            }
        }
        public TaskableCivilian(ITaskableTarget_Old currentPlayer, PedExt _GTAPedToTask)
        {
            CurrentPlayer = currentPlayer;
            CivilianToTask = _GTAPedToTask;
        }
        private Activities CurrentActivity
        {
            get
            {
                if (!CivilianToTask.CanBeTasked)
                {
                    return Activities.Idle;
                }
                else
                {
                    if (CivilianToTask.HasSeenPlayerCommitCrime)
                    {
                        return Activities.ReactToCrime;
                    }
                    else if (CivilianToTask.CanRecognizePlayer && CurrentPlayer.IsViolatingAnyCivilianReportableCrime)
                    {
                        return Activities.ReactToCrime;
                    }
                    else if (CivilianToTask.WithinWeaponsAudioRange && CurrentPlayer.IsViolatingAnyAudioBasedCivilianReportableCrime)
                    {
                        return Activities.ReactToCrime;
                    }
                }
                return Activities.Idle;
            }
        }
        public void RunCurrentActivity()
        {
            GameTimeLastRanActivity = Game.GameTime;
            if (CurrentActivity == Activities.ReactToCrime)
            {
                ReactToCrime();
                return;
            }
            else if (CurrentActivity == Activities.Idle)
            {

            }
            CurrentTaskLoop = "None";
            CivilianToTask.Pedestrian.BlockPermanentEvents = false;
            CivilianToTask.Pedestrian.KeepTasks = false;
        }
        private void ReactToCrime()
        {
            if (CurrentTaskLoop != "ReactToCrime")
            {
                if (CivilianToTask.Pedestrian.Exists())
                {
                    CivilianToTask.HasReactedToCrimes = true;

                    if (CivilianToTask.CrimesWitnessed.Any(x => x.AngersCivilians))
                    {
                        if (CivilianToTask.WillFight && CurrentPlayer.IsNotWanted)
                        {
                            CurrentSubTaskLoop = "Fight";
                            GiveWeapon();
                            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", CivilianToTask.Pedestrian, 5, true);//BF_CanFightArmedPedsWhenNotArmed = 5,
                            CivilianToTask.Pedestrian.Tasks.FightAgainst(Game.LocalPlayer.Character);
                            CivilianToTask.Pedestrian.KeepTasks = true;
                        }
                    }
                    if(CivilianToTask.CrimesWitnessed.Any(x => x.ScaresCivilians))
                    {
                        if (CivilianToTask.WillCallPolice)
                        {
                            AddCurrentCrimes();
                            unsafe
                            {
                                int lol = 0;
                                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                NativeFunction.CallByName<bool>("TASK_SMART_FLEE_PED", 0, Game.LocalPlayer.Character, 50f, 7000);//100f
                                NativeFunction.CallByName<bool>("TASK_USE_MOBILE_PHONE_TIMED", 0, 5000);
                                NativeFunction.CallByName<bool>("TASK_SMART_FLEE_PED", 0, Game.LocalPlayer.Character, 100f, -1);
                                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", CivilianToTask.Pedestrian, lol);
                                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                            }
                            CurrentSubTaskLoop = "CallIn";
                        }
                        else
                        {
                            CivilianToTask.Pedestrian.Tasks.Flee(Game.LocalPlayer.Character, 50f, -1);
                        }
                    }
                    else
                    {
                        if(CivilianToTask.WillCallPolice)
                        {
                            AddCurrentCrimes();
                            NativeFunction.CallByName<bool>("TASK_USE_MOBILE_PHONE_TIMED", CivilianToTask.Pedestrian, 5000);
                        }
                    }
                    CurrentSubTaskLoop = "Flee";
                    //Game.Console.Print(string.Format("     Started ReactToCrime: {0} Old CurrentTaskLoop: {1} Fight: {2} CallPolice: {3}", CivilianToTask.Pedestrian.Handle, CurrentTaskLoop, CivilianToTask.WillFight, CivilianToTask.WillCallPolice));
                }
            }
            else
            {
                if (CivilianToTask.WillFight && CurrentPlayer.IsWanted && CurrentSubTaskLoop == "Fight")
                {
                    CivilianToTask.Pedestrian.Tasks.Clear();
                    CivilianToTask.Pedestrian.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Player, Relationship.Neutral);
                    CivilianToTask.Pedestrian.Tasks.Flee(Game.LocalPlayer.Character, 100f, -1);
                    CurrentSubTaskLoop = "Flee";
                    //Game.Console.Print(string.Format("     Started Flee After Fight: {0} Old CurrentTaskLoop: {1} Fight: {2} CallPolice: {3}", CivilianToTask.Pedestrian.Handle, CurrentTaskLoop, CivilianToTask.WillFight, CivilianToTask.WillCallPolice));
                }
            }
            CurrentTaskLoop = "ReactToCrime";
            GameTimeLastTasked = Game.GameTime;
        }
        private void AddCurrentCrimes()
        {
            foreach (Crime CurrentlyViolating in CurrentPlayer.CivilianReportableCrimesViolating)
            {
                CivilianToTask.WitnessedCrime(CurrentlyViolating, Game.LocalPlayer.Character.Position);
            }
        }
        private void GiveWeapon()
        {
            CivilianToTask.Pedestrian.Inventory.GiveNewWeapon("weapon_pistol", 60, true);
        }
    }
    private enum eScriptTaskHash : uint
    {
        SCRIPT_TASK_ANY = 0x55966344,
        SCRIPT_TASK_INVALID = 0x811E343C,
        SCRIPT_TASK_PAUSE = 0x03C990EC,
        SCRIPT_TASK_STAND_STILL = 0xC572E06A,
        SCRIPT_TASK_0xA6296C9D = 0xA6296C9D,
        SCRIPT_TASK_JUMP = 0x24415046,
        SCRIPT_TASK_COWER = 0x1C43F4CF,
        SCRIPT_TASK_HANDS_UP = 0xA573B67C,
        SCRIPT_TASK_DUCK = 0x1D415F6C,
        SCRIPT_TASK_0xD9162485 = 0xD9162485,
        SCRIPT_TASK_0x255F21CC = 0x255F21CC,
        SCRIPT_TASK_ENTER_VEHICLE = 0x950B6492,
        SCRIPT_TASK_LEAVE_VEHICLE = 0x1AE73569,
        SCRIPT_TASK_VEHICLE_DRIVE_TO_COORD = 0x93A5526E,
        SCRIPT_TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE = 0x21D33957,
        SCRIPT_TASK_VEHICLE_DRIVE_WANDER = 0xF09B15B3,
        SCRIPT_TASK_GO_STRAIGHT_TO_COORD = 0x7D8F4411,
        SCRIPT_TASK_GO_STRAIGHT_TO_COORD_RELATIVE_TO_ENTITY = 0x78EC0FF6,
        SCRIPT_TASK_0x96066708 = 0x96066708,
        SCRIPT_TASK_ACHIEVE_HEADING = 0x7276D3DF,
        SCRIPT_TASK_FOLLOW_POINT_ROUTE = 0xB232526F,
        SCRIPT_TASK_GO_TO_ENTITY = 0x4924437D,
        SCRIPT_TASK_0xD7F626D1 = 0xD7F626D1,
        SCRIPT_TASK_0xEEDD9B66 = 0xEEDD9B66,
        SCRIPT_TASK_0x114F64E3 = 0x114F64E3,
        SCRIPT_TASK_0xF10822AA = 0xF10822AA,
        SCRIPT_TASK_SMART_FLEE_PED = 0x6BA30179,
        SCRIPT_TASK_WANDER_STANDARD = 0xBBA3B7CA,
        SCRIPT_TASK_FOLLOW_NAV_MESH_TO_COORD = 0x2A89B8A7,
        SCRIPT_TASK_GO_TO_COORD_ANY_MEANS = 0x93399E79,
        SCRIPT_TASK_PERFORM_SEQUENCE = 0x0E763797,
        SCRIPT_TASK_LEAVE_ANY_VEHICLE = 0xCE98FBB3,
        SCRIPT_TASK_AIM_GUN_SCRIPTED = 0x0C69931F,
        SCRIPT_TASK_AIM_GUN_AT_ENTITY = 0x6134071B,
        SCRIPT_TASK_GO_TO_COORD_WHILE_SHOOTING = 0x9387DEAB,
        SCRIPT_TASK_TURN_PED_TO_FACE_ENTITY = 0xCBCE4595,
        SCRIPT_TASK_0xE51B372C = 0xE51B372C,
        SCRIPT_TASK_AIM_GUN_AT_COORD = 0x49BEF36E,
        SCRIPT_TASK_SHOOT_AT_COORD = 0xD90EF188,
        SCRIPT_TASK_0x0B45DACC = 0x0B45DACC,
        SCRIPT_TASK_0x7BA620DD = 0x7BA620DD,
        SCRIPT_TASK_SHUFFLE_TO_NEXT_VEHICLE_SEAT = 0x153011FC,
        SCRIPT_TASK_EVERYONE_LEAVE_VEHICLE = 0xA569F146,
        SCRIPT_TASK_0xC09E33A2 = 0xC09E33A2,
        SCRIPT_TASK_GOTO_ENTITY_OFFSET = 0x87E3E0A8,
        SCRIPT_TASK_0xBF57AF1C = 0xBF57AF1C,
        SCRIPT_TASK_0x190DC01B = 0x190DC01B,
        SCRIPT_TASK_TURN_PED_TO_FACE_COORD = 0x574BB8F5,
        SCRIPT_TASK_DRIVE_POINT_ROUTE = 0xBAE13130,
        SCRIPT_TASK_0x7DEC090B = 0x7DEC090B,
        SCRIPT_TASK_VEHICLE_TEMP_ACTION = 0x81B4D53A,
        SCRIPT_TASK_0x30A0DC39 = 0x30A0DC39,
        SCRIPT_TASK_VEHICLE_MISSION = 0xB41F1A34,
        SCRIPT_TASK_0xE4A207BD = 0xE4A207BD,
        SCRIPT_TASK_0xB2A2BF11 = 0xB2A2BF11,
        SCRIPT_TASK_0x2C1A612F = 0x2C1A612F,
        SCRIPT_TASK_DRIVE_BY = 0x7D711E7D,
        SCRIPT_TASK_USE_MOBILE_PHONE = 0x37D339A1,
        SCRIPT_TASK_WARP_PED_INTO_VEHICLE = 0xBC555B9D,
        SCRIPT_TASK_0x63694D9D = 0x63694D9D,
        SCRIPT_TASK_SHOOT_AT_ENTITY = 0x0A01F8B8,
        SCRIPT_TASK_0x15F49B5F = 0x15F49B5F,
        SCRIPT_TASK_0x3A82EBC5 = 0x3A82EBC5,
        SCRIPT_TASK_0xF793E251 = 0xF793E251,
        SCRIPT_TASK_0x9A2943F2 = 0x9A2943F2,
        SCRIPT_TASK_CLIMB = 0xB802FDCA,
        SCRIPT_TASK_PERFORM_SEQUENCE_FROM_PROGRESS = 0x5485FD94,
        SCRIPT_TASK_GOTO_ENTITY_AIMING = 0x967EA21C,
        SCRIPT_TASK_0x1A230A59 = 0x1A230A59,
        SCRIPT_TASK_0x6EA2E79A = 0x6EA2E79A,
        SCRIPT_TASK_SET_PED_DECISION_MAKER = 0x4E5B453C,
        SCRIPT_TASK_0x00A101C8 = 0x00A101C8,
        SCRIPT_TASK_0xC8BCA367 = 0xC8BCA367,
        SCRIPT_TASK_0xA9D6E737 = 0xA9D6E737,
        SCRIPT_TASK_PED_SLIDE_TO_COORD = 0x3E5094A7,
        SCRIPT_TASK_0xABFCB97C = 0xABFCB97C,
        SCRIPT_TASK_DRIVE_POINT_ROUTE_ADVANCED = 0xEA6A323F,
        SCRIPT_TASK_PED_SLIDE_TO_COORD_AND_PLAY_ANIM = 0x8A0970F4,
        SCRIPT_TASK_0x22024D52 = 0x22024D52,
        SCRIPT_TASK_0xAD4CD615 = 0xAD4CD615,
        SCRIPT_TASK_0xD73264BC = 0xD73264BC,
        SCRIPT_TASK_0xA0A7761F = 0xA0A7761F,
        SCRIPT_TASK_0xA92F7B36 = 0xA92F7B36,
        SCRIPT_TASK_0xD0D5F297 = 0xD0D5F297,
        SCRIPT_TASK_0xADC7E889 = 0xADC7E889,
        SCRIPT_TASK_0x1F53A7DA = 0x1F53A7DA,
        SCRIPT_TASK_PLAY_ANIM = 0x87B9A382,
        SCRIPT_TASK_0x8ECCBFB3 = 0x8ECCBFB3,
        SCRIPT_TASK_0xAF35BD9C = 0xAF35BD9C,
        SCRIPT_TASK_0xB99876B9 = 0xB99876B9,
        SCRIPT_TASK_ARREST_PED = 0x52FF82C0,
        SCRIPT_TASK_COMBAT = 0x2E85A751,
        SCRIPT_TASK_COMBAT_TIMED = 0xF2E41A8A,
        SCRIPT_TASK_SEEK_COVER_FROM_POS = 0xA77A06C5,
        SCRIPT_TASK_SEEK_COVER_FROM_PED = 0x71E30BDC,
        SCRIPT_TASK_SEEK_COVER_TO_COVER_POINT = 0x99AFA8A3,
        SCRIPT_TASK_0x9B95A683 = 0x9B95A683,
        SCRIPT_TASK_TOGGLE_DUCK = 0x0F3B8554,
        SCRIPT_TASK_0x97AE64AB = 0x97AE64AB,
        SCRIPT_TASK_0xDF5F4BA7 = 0xDF5F4BA7,
        SCRIPT_TASK_PICKUP_AND_CARRY_OBJECT = 0x89025025,
        SCRIPT_TASK_0x5A2825BB = 0x5A2825BB,
        SCRIPT_TASK_SEEK_COVER_TO_COORDS = 0x6C01775C,
        SCRIPT_TASK_0xBA284891 = 0xBA284891,
        SCRIPT_TASK_GUARD_ANGLED_DEFENSIVE_AREA = 0x84AEE7A0,
        SCRIPT_TASK_STAND_GUARD = 0xD88F2CDE,
        SCRIPT_TASK_CLIMB_LADDER = 0x66403353,
        SCRIPT_TASK_0xFD790A1B = 0xFD790A1B,
        SCRIPT_TASK_GUARD_SPHERE_DEFENSIVE_AREA = 0x21E8D4E4,
        SCRIPT_TASK_START_SCENARIO_IN_PLACE = 0x3B3A458F,
        SCRIPT_TASK_START_SCENARIO_AT_POSITION = 0xBE86C566,
        SCRIPT_TASK_0x86016E38 = 0x86016E38,
        SCRIPT_TASK_PUT_PED_DIRECTLY_INTO_COVER = 0x8B2F140E,
        SCRIPT_TASK_0x9DD414F5 = 0x9DD414F5,
        SCRIPT_TASK_PUT_PED_DIRECTLY_INTO_MELEE = 0xFBBF6F4D,
        SCRIPT_TASK_GUARD_CURRENT_POSITION = 0x8CE49D34,
        SCRIPT_TASK_0x623A5EFE = 0x623A5EFE,
        SCRIPT_TASK_0x9BD19AE7 = 0x9BD19AE7,
        SCRIPT_TASK_0x9F5DBCE5 = 0x9F5DBCE5,
        SCRIPT_TASK_PERFORM_SEQUENCE_LOCALLY = 0xE7FBAB4F,
        SCRIPT_TASK_COMBAT_HATED_TARGETS_IN_AREA = 0x42CC4F21,
        SCRIPT_TASK_COMBAT_HATED_TARGETS_AROUND_PED = 0xAA05B492,
        SCRIPT_TASK_0x81FB0B11 = 0x81FB0B11,
        SCRIPT_TASK_0x71F49E88 = 0x71F49E88,
        SCRIPT_TASK_0xE3380A30 = 0xE3380A30,
        SCRIPT_TASK_SWAP_WEAPON = 0x2AB81462,
        SCRIPT_TASK_RELOAD_WEAPON = 0xC322ED6F,
        SCRIPT_TASK_0xAB4B293A = 0xAB4B293A,
        SCRIPT_TASK_COMBAT_HATED_TARGETS_AROUND_PED_TIMED = 0x2719C0D1,
        SCRIPT_TASK_GET_OFF_BOAT = 0x9A27A999,
        SCRIPT_TASK_0x9C4FBCAC = 0x9C4FBCAC,
        SCRIPT_TASK_PATROL = 0xB550726C,
        SCRIPT_TASK_STAY_IN_COVER = 0xE1C16E99,
        SCRIPT_TASK_HANG_GLIDER = 0x00E1228C,
        SCRIPT_TASK_FOLLOW_TO_OFFSET_OF_ENTITY = 0x3EF867F4,
        SCRIPT_TASK_0x70AEF4E9 = 0x70AEF4E9,
        SCRIPT_TASK_GO_TO_COORD_WHILE_AIMING_AT_COORD = 0x19CE5AFC,
        SCRIPT_TASK_GO_TO_COORD_WHILE_AIMING_AT_ENTITY = 0x972C6757,
        SCRIPT_TASK_0x0A81CE80 = 0x0A81CE80,
        SCRIPT_TASK_0xE677F9FB = 0xE677F9FB,
        SCRIPT_TASK_0x89E45204 = 0x89E45204,
        SCRIPT_TASK_GO_TO_ENTITY_WHILE_AIMING_AT_COORD = 0xBAEB517C,
        SCRIPT_TASK_0xA2B07D24 = 0xA2B07D24,
        SCRIPT_TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY = 0xB80BFB24,
        SCRIPT_TASK_0x6C095462 = 0x6C095462,
        SCRIPT_TASK_0x7BF24249 = 0x7BF24249,
        SCRIPT_TASK_0xC93D7834 = 0xC93D7834,
        SCRIPT_TASK_USE_WALKIE_TALKIE = 0x29BABC64,
        SCRIPT_TASK_CHAT_TO_PED = 0x0FC239CD,
        SCRIPT_TASK_0xFCC0F996 = 0xFCC0F996,
        SCRIPT_TASK_FIRE_FLARE = 0xDEB1C08F,
        SCRIPT_TASK_BIND_POSE = 0x4929CE40,
        SCRIPT_TASK_NM_ELECTROCUTE = 0x8944A9A0,
        SCRIPT_TASK_NM_HIGH_FALL = 0x015D63E3,
        SCRIPT_TASK_NM_DANGLE = 0x0B49EAEC,
        SCRIPT_TASK_0xF0F9FFC0 = 0xF0F9FFC0,
        SCRIPT_TASK_NM_STUMBLE = 0xBACF9837,
        SCRIPT_TASK_SKY_DIVE = 0x4B65F15C,
        SCRIPT_TASK_PARACHUTE = 0x76CA4A8E,
        SCRIPT_TASK_PARACHUTE_TO_TARGET = 0x4921B47A,
        SCRIPT_TASK_0x9B4FC7D8 = 0x9B4FC7D8,
        SCRIPT_TASK_0x536E59F9 = 0x536E59F9,
        SCRIPT_TASK_0x4847A94F = 0x4847A94F,
        SCRIPT_TASK_SET_BLOCKING_OF_NON_TEMPORARY_EVENTS = 0x6F9C865C,
        SCRIPT_TASK_MOVE_NETWORK = 0x0494661C,
        SCRIPT_TASK_SYNCHRONIZED_SCENE = 0x6A67A5CC,
        SCRIPT_TASK_VEHICLE_SHOOT_AT_COORD = 0xAF18B824,
        SCRIPT_TASK_0x20123810 = 0x20123810,
        SCRIPT_TASK_VEHICLE_PARK = 0xEFC8537E,
        SCRIPT_TASK_MOUNT_ANIMAL = 0x6F5F73AE,
        SCRIPT_TASK_DISMOUNT_ANIMAL = 0x1DE2A7BD,
        SCRIPT_TASK_THROW_PROJECTILE = 0xAD37BF03,
        SCRIPT_TASK_VEHICLE_AIM_AT_COORD = 0x00C59C52,
        SCRIPT_TASK_0x6F30F4C1 = 0x6F30F4C1,
        SCRIPT_TASK_0x3BDBC83C = 0x3BDBC83C,
        SCRIPT_TASK_0xCC312EC4 = 0xCC312EC4,
        SCRIPT_TASK_RAPPEL_FROM_HELI = 0xEF8D6B40,
        SCRIPT_TASK_0x491A782D = 0x491A782D,
        SCRIPT_TASK_0x87A3DFEA = 0x87A3DFEA,
        SCRIPT_TASK_VEHICLE_FOLLOW_WAYPOINT_RECORDING = 0xF1F17AE7,
        SCRIPT_TASK_0x8E29DEF2 = 0x8E29DEF2,
        SCRIPT_TASK_GO_TO_COORD_AND_AIM_AT_HATED_ENTITIES_NEAR_COORD = 0x290A02BC,
        SCRIPT_TASK_WANDER_IN_AREA = 0x370BCF53,
        SCRIPT_TASK_VEHICLE_GOTO_NAVMESH = 0xFBB43C4A,
        SCRIPT_TASK_FORCE_MOTION_STATE = 0x9E78AC1F,
        SCRIPT_TASK_IN_CUSTODY = 0x6D4411C9,
        SCRIPT_TASK_LOOK_AT_ENTITY = 0x08F5AF9D,
        SCRIPT_TASK_LOOK_AT_COORD = 0xCB842EEC,
        SCRIPT_TASK_VEHICLE_CHASE = 0x2288A57C,
        SCRIPT_TASK_STEALTH_KILL = 0x5014CC1A,
        SCRIPT_TASK_HELI_CHASE = 0x27369192,
        SCRIPT_TASK_PLANE_CHASE = 0x02DBA9BF,
        SCRIPT_TASK_PLANE_LAND = 0x043E4A56,
        SCRIPT_TASK_0x7F9814E9 = 0x7F9814E9,
        SCRIPT_TASK_0xEC685098 = 0xEC685098,
        SCRIPT_TASK_SHOCKING_EVENT_REACT = 0x498BABE3,
        SCRIPT_TASK_WRITHE = 0x8EC23E41,
        SCRIPT_TASK_EXIT_COVER = 0x4E961D82,
        SCRIPT_TASK_PLANT_BOMB = 0x8127FD1A,
        SCRIPT_TASK_INVESTIGATE_COORDS = 0x9C250C19,
        SCRIPT_TASK_WANDER_SPECIFIC = 0xD46F7254,
        SCRIPT_TASK_0x48EED267 = 0x48EED267,
        SCRIPT_TASK_0xFD0B5826 = 0xFD0B5826,
        SCRIPT_TASK_0x29269FF1 = 0x29269FF1,
        SCRIPT_TASK_REACT_AND_FLEE_PED = 0x7DEDF098,
        SCRIPT_TASK_GO_TO_COORD_ANY_MEANS_EXTRA_PARAMS = 0x45B5A146,
        SCRIPT_TASK_0xA5806868 = 0xA5806868,
        SCRIPT_TASK_JETPACK = 0x828EBA07,
        SCRIPT_TASK_GO_TO_COORD_ANY_MEANS_EXTRA_PARAMS_WITH_CRUISE_SPEED = 0x4DE5C290,
        SCRIPT_TASK_AGITATED_ACTION = 0x548CB4B4
    };
}

