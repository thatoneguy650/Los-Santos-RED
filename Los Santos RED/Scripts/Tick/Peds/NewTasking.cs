using Rage;
using Rage.Native;
using System.Collections.Generic;
using System.Linq;

public static class NewTasking
{
    private static List<TaskableCop> TaskableCops;
    private static List<TaskableCivilian> TaskableCivilians;
    public static bool IsRunning { get; set; }
    public static bool HasCopsInvestigating
    {
        get
        {
            if (TaskableCops.Any(x => x.CopToTask.Pedestrian.Exists() && x.IsInvestigating))
                return true;
            else
                return false;
        }
    }
    public static void Initialize()
    {
        IsRunning = true;
        TaskableCops = new List<TaskableCop>();
        TaskableCivilians = new List<TaskableCivilian>();
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void UpdateTaskableCops()
    {
        if (IsRunning)
        {
            PedList.CopPeds.RemoveAll(x => !x.Pedestrian.Exists());

            TaskableCops.RemoveAll(x => !x.CopToTask.Pedestrian.Exists());
            foreach (Cop Cop in PedList.CopPeds.Where(x => x.Pedestrian.Exists()))
            {
                if (!TaskableCops.Any(x => x.CopToTask.Pedestrian.Handle == Cop.Pedestrian.Handle))
                {
                    TaskableCops.Add(new TaskableCop(Cop));
                }
            }

            TaskableCivilians.RemoveAll(x => !x.CivilianToTask.Pedestrian.Exists());
            foreach (PedExt Civilian in PedList.Civilians.Where(x => x.Pedestrian.Exists()))
            {
                if (!TaskableCivilians.Any(x => x.CivilianToTask.Pedestrian.Handle == Civilian.Pedestrian.Handle))
                {
                    TaskableCivilians.Add(new TaskableCivilian(Civilian));
                }
            }
        }
    }
    public static void RunActivities()
    {
        if (IsRunning)
        {
            foreach (TaskableCop Cop in TaskableCops.Where(x => x.CopToTask.Pedestrian.Exists()))
            {
                Cop.RunCurrentActivity();
                if(Cop.ShouldBustPlayer)
                {
                    PlayerState.StartManualArrest();
                }
            }

            foreach (TaskableCivilian Civilian in TaskableCivilians.Where(x => x.CivilianToTask.Pedestrian.Exists()))
            {
                Civilian.RunCurrentActivity();
            }

        }
    }
    public static void PrintActivities()//Debugging Proc
    {
        Debugging.WriteToLog("Tasking", "================================================");
        foreach (TaskableCop Cop in TaskableCops.Where(x => x.CopToTask.Pedestrian.Exists()))
        {
            Debugging.WriteToLog("Tasking", string.Format("Distance: {0}, Handle: {1}, {2}",Cop.CopToTask.DistanceToPlayer,Cop.CopToTask.Pedestrian.Handle, Cop.DebugTaskState));
        }
        Debugging.WriteToLog("Tasking", "================================================");
    }
    public static void UnTask()//Debugging Proc
    {
        Debugging.WriteToLog("Tasking", "================================================");
        foreach (TaskableCop Cop in TaskableCops.Where(x => x.CopToTask.Pedestrian.Exists()))
        {
            Cop.ClearTasks();
        }
        Debugging.WriteToLog("Tasking", "================================================");
    }
    private class TaskableCop
    {
        private float OnFootTaskDistance = 70f;
        private Vector3 CurrentTaskedPosition = Vector3.Zero;
        private bool AtInvesstigationPositionThisInvestigation = false;
        private bool NearWantedCenterThisWanted = false;
        private string CurrentTaskLoop;
        private string CurrentSubTaskLoop;
        private bool NeedSirenOn
        {
            get
            {
                if (WantedLevelScript.CurrentResponse == WantedLevelScript.ResponsePriority.Low || WantedLevelScript.CurrentResponse == WantedLevelScript.ResponsePriority.None)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
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
        private float DrivingSpeed
        {
            get
            {
                if(WantedLevelScript.CurrentResponse == WantedLevelScript.ResponsePriority.High)
                {
                    return 25f; //55 mph
                }
                else if (WantedLevelScript.CurrentResponse == WantedLevelScript.ResponsePriority.Medium)
                {
                    return 25f; //55 mph
                }
                else
                {
                    return 20f; //40 mph
                }
            }
        }
        private bool AttemptingToSurrender
        {
            get
            {
                if (PlayerState.HandsAreUp && !WantedLevelScript.IsWeaponsFree)
                    return true;
                else
                    return false;
            }
        }
        public bool ShouldBustPlayer
        {
            get
            {
                if (PlayerState.IsBusted)
                {
                    return false;
                }
                else if (!PlayerState.IsBustable)
                {
                    return false;
                }
                else if (CopToTask.DistanceToPlayer < 0.1f) //weird cases where they are my same position
                {
                    return false;
                }
                else if (PlayerState.HandsAreUp && CopToTask.DistanceToPlayer <= 5f)
                {
                    return true;
                }
                if(PlayerState.IsInVehicle)
                {
                    if(PlayerState.IsStationary && CopToTask.DistanceToPlayer <= 1f)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if ((Game.LocalPlayer.Character.IsStunned || Game.LocalPlayer.Character.IsRagdoll) && CopToTask.DistanceToPlayer <= 3f)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        private enum Activities
        {
            Investigate,
            Idle,
            Chase,
        }
        private enum AIDynamic
        {
            Cop_InVehicle_Player_InVehicle,
            Cop_InVehicle_Player_OnFoot,
            Cop_OnFoot_Player_InVehicle,
            Cop_OnFoot_Player_OnFoot,
        }
        public Cop CopToTask { get; set; }
        public uint GameTimeLastTasked { get; set; }
        public string DebugTaskState
        {
            get
            {
                return string.Format("Dynamic: {0}      Activity: {1}       Loop: {2}       SubLoop: {3}        DistanceToPlayer: {4}       TimeTasked: {5}", CurrentDynamic.ToString(),CurrentActivity.ToString(),CurrentTaskLoop,CurrentSubTaskLoop,CopToTask.DistanceToPlayer,GameTimeLastTasked);
            }
        }
        private Activities CurrentActivity
        {
            get
            {
                if (PlayerState.IsNotWanted)
                {
                    if (Investigation.InInvestigationMode)
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
                    if (CopToTask.WithinChaseDistance)
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
                if (PlayerState.IsInVehicle)
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
        public TaskableCop(Cop _GTAPedToTask)
        {
            CopToTask = _GTAPedToTask;
        }
        public void RunCurrentActivity()
        {
            ArmCopAppropriately();
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
               // CopToTask.Pedestrian.BlockPermanentEvents = true;
                CopToTask.Pedestrian.KeepTasks = true;
                if (!CopToTask.Pedestrian.IsInAnyVehicle(false))
                {
                    Vehicle LastVehicle = CopToTask.Pedestrian.LastVehicle;
                    if (LastVehicle.Exists() && LastVehicle.IsDriveable && CopToTask.WasRandomSpawnDriver)
                    {
                        unsafe
                        {
                            int lol = 0;
                            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                            NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, LastVehicle, -1, -1, 2f, 9);
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
                Debugging.WriteToLog("Tasking", string.Format("     Started Idle: {0} Old CurrentTaskLoop: {1}", CopToTask.Pedestrian.Handle, CurrentTaskLoop));
            }
        }
        private void Idle_Normal()
        {
            if (CopToTask.Pedestrian.Exists() && CopToTask.IsDriver && CopToTask.Pedestrian.CurrentVehicle.Exists() && CopToTask.Pedestrian.CurrentVehicle.HasSiren)
            {
                CopToTask.Pedestrian.CurrentVehicle.IsSirenOn = false;
                CopToTask.Pedestrian.CurrentVehicle.IsSirenSilent = false;
            }

            //if (Game.GameTime - GameTimeLastTasked >= 2000 && TimesExecuted <= 5)//weird bug that makes them attack me after not wanted and being idle, reset them ever second for now
            //{
            //    if (!CopToTask.Pedestrian.IsInAnyVehicle(false))
            //    {
            //        Vehicle LastVehicle = CopToTask.Pedestrian.LastVehicle;
            //        if (LastVehicle.Exists() && LastVehicle.IsDriveable && CopToTask.WasRandomSpawnDriver)
            //        {
            //            unsafe
            //            {
            //                int lol = 0;
            //                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            //                NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, LastVehicle, -1, -1, 2f, 9);
            //                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, LastVehicle, 18f, 183);
            //                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
            //                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            //                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", CopToTask.Pedestrian, lol);
            //                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            //            }
            //        }
            //        else
            //        {
            //            CopToTask.Pedestrian.Tasks.Wander();
            //        }
            //    }
            //    TimesExecuted++;
            //}
        }
        private void Investigate()
        {
            float DisanceToInvestigation = CopToTask.Pedestrian.DistanceTo2D(Investigation.InvestigationPosition);
            if (DisanceToInvestigation <= Investigation.InvestigationDistance)
            {
                if (CopToTask.IsInVehicle || DisanceToInvestigation <= OnFootTaskDistance)
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
            }
            CurrentTaskLoop = "Investigation";
        }
        private void Investigate_Start()
        {
            if (Investigation.InvestigationPosition == Vector3.Zero)
            {
                Investigation.InvestigationPosition = Game.LocalPlayer.Character.Position;
                if (PlayerState.IsWanted)
                    Investigation.InvestigationPosition = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
            }
            CurrentTaskedPosition = Investigation.InvestigationPosition;
            CopToTask.Pedestrian.BlockPermanentEvents = false;
            if (CopToTask.Pedestrian.IsInAnyVehicle(false))
                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", CopToTask.Pedestrian, CopToTask.Pedestrian.CurrentVehicle, Investigation.InvestigationPosition.X, Investigation.InvestigationPosition.Y, Investigation.InvestigationPosition.Z, DrivingSpeed, 4 | 16 | 32 | 262144, 10f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, PositionOfInterest.X, PositionOfInterest.Y, PositionOfInterest.Z, 70f, 4 | 16 | 32 | 262144, 35f);
            else
                NativeFunction.CallByName<bool>("TASK_GO_STRAIGHT_TO_COORD", CopToTask.Pedestrian, Investigation.InvestigationPosition.X, Investigation.InvestigationPosition.Y, Investigation.InvestigationPosition.Z, 500f, -1, 0f, 2f);
            CurrentTaskLoop = "Investigation";
            AtInvesstigationPositionThisInvestigation = false;
            Debugging.WriteToLog("Tasking", string.Format("     Started Investigate: {0}, CurrentResponse {1}, DrivingSpeed {2}, NeedSirenOn {3}", CopToTask.Pedestrian.Handle, WantedLevelScript.CurrentResponse, DrivingSpeed, NeedSirenOn));
        }
        private void Investigate_Normal()
        {
            if (!AtInvesstigationPositionThisInvestigation)
            {
                if (Investigation.InvestigationPosition != Vector3.Zero && Investigation.InvestigationPosition != CurrentTaskedPosition) //retask them if it changes
                {
                    CurrentTaskedPosition = Investigation.InvestigationPosition;
                    if (CopToTask.Pedestrian.IsInAnyVehicle(false))
                        NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", CopToTask.Pedestrian, CopToTask.Pedestrian.CurrentVehicle, Investigation.InvestigationPosition.X, Investigation.InvestigationPosition.Y, Investigation.InvestigationPosition.Z, DrivingSpeed, 4 | 16 | 32 | 262144, 10f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, PositionOfInterest.X, PositionOfInterest.Y, PositionOfInterest.Z, 70f, 4 | 16 | 32 | 262144, 35f);
                    else
                        NativeFunction.CallByName<bool>("TASK_GO_STRAIGHT_TO_COORD", CopToTask.Pedestrian, Investigation.InvestigationPosition.X, Investigation.InvestigationPosition.Y, Investigation.InvestigationPosition.Z, 500f, -1, 0f, 2f);

                    Debugging.WriteToLog("Tasking", string.Format("     Reset Investigate: {0}, CurrentResponse {1}, DrivingSpeed {2}, NeedSirenOn {3}", CopToTask.Pedestrian.Handle, WantedLevelScript.CurrentResponse, DrivingSpeed, NeedSirenOn));

                }
                if (CopToTask.IsDriver && CopToTask.Pedestrian.CurrentVehicle.HasSiren && NeedSirenOn)
                {
                    if (!CopToTask.Pedestrian.CurrentVehicle.IsSirenOn)
                    {
                        CopToTask.Pedestrian.CurrentVehicle.IsSirenOn = true;
                        CopToTask.Pedestrian.CurrentVehicle.IsSirenSilent = false;
                    }
                }
                if (CopToTask.Pedestrian.DistanceTo2D(Investigation.InvestigationPosition) <= 15f)
                {
                    AtInvesstigationPositionThisInvestigation = true;
                    if (CopToTask.Pedestrian.Exists() && CopToTask.Pedestrian.CurrentVehicle.Exists())
                    {
                        NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", CopToTask.Pedestrian, CopToTask.Pedestrian.CurrentVehicle, DrivingSpeed, 4 | 16 | 32 | 262144, 10f);
                        Debugging.WriteToLog("Tasking", string.Format("     Started Investigation Wander: {0}", CopToTask.Pedestrian.Handle));
                    }
                }
            }
        }
        private void Chase()
        {
            if (CopToTask.WithinChaseDistance)
            {
                if (Police.AnyRecentlySeenPlayer && !PlayerState.AreStarsGreyedOut)
                {
                    if (CurrentDynamic == AIDynamic.Cop_InVehicle_Player_InVehicle)
                    {
                        if (!CopToTask.IsInHelicopter)
                        {
                            if (CopToTask.DistanceToPlayer <= 25f || CopToTask.CanSeePlayer)
                            {
                                CurrentTaskLoop = "None";
                            }
                            else
                            {
                                VehicleChase();
                            }
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
                        if (CopToTask.DistanceToPlayer >= 30f)
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
                //    Debugging.WriteToLog("Tasking", string.Format("Surrender Bust Chase: {0}", CopToTask.Pedestrian.Handle));
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
            CopToTask.Pedestrian.BlockPermanentEvents = false;
            Vector3 WantedCenter = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
            if (PlayerState.IsInVehicle)
            {
                NativeFunction.CallByName<bool>("TASK_VEHICLE_CHASE", CopToTask.Pedestrian, Game.LocalPlayer.Character); //NativeFunction.CallByName<bool>("TASK_VEHICLE_FOLLOW", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, Game.LocalPlayer.Character, 22f, 4 | 16 | 32 | 262144, 8f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_CHASE", Cop.Pedestrian, Game.LocalPlayer.Character);
            }
            else
            {
                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", CopToTask.Pedestrian, CopToTask.Pedestrian.CurrentVehicle, WantedCenter.X, WantedCenter.Y, WantedCenter.Z, 20f, 4 | 16 | 32 | 262144, 20f);
            }
            CurrentTaskedPosition = WantedCenter;
            CurrentTaskLoop = "VehicleChase";
            Debugging.WriteToLog("Tasking", string.Format("     Started VehicleChase: {0}", CopToTask.Pedestrian.Handle));
        }
        private void VehicleChase_Normal()
        {
            Vector3 WantedCenter = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
            if(CurrentTaskedPosition.DistanceTo2D(WantedCenter) >= 10f)
            {
                if (!PlayerState.IsInVehicle)
                {
                    if (CopToTask.Pedestrian.CurrentVehicle.Exists())
                    {
                        NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", CopToTask.Pedestrian, CopToTask.Pedestrian.CurrentVehicle, WantedCenter.X, WantedCenter.Y, WantedCenter.Z, 20f, 4 | 16 | 32 | 262144, 20f);
                        Debugging.WriteToLog("Tasking", string.Format("     Updated VehicleChase: {0}", CopToTask.Pedestrian.Handle));
                    }
                }
                CurrentTaskedPosition = WantedCenter;
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
                Debugging.WriteToLog("Tasking", string.Format("     Started HeliChase: {0}", CopToTask.Pedestrian.Handle));
            }
        }
        private void CarJack()
        {
            if (CurrentTaskLoop != "CarJack")
            {
                if (CopToTask.Pedestrian.Exists())
                {
                    if(PlayerState.IsInVehicle && Game.LocalPlayer.Character.CurrentVehicle.Exists())
                    {
                        NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", CopToTask.Pedestrian, Game.LocalPlayer.Character.CurrentVehicle, -1, -1, 2f, 9);
                    }
                    Debugging.WriteToLog("Tasking", string.Format("     Started CarJack: {0} Old CurrentTaskLoop: {1}", CopToTask.Pedestrian.Handle, CurrentTaskLoop));
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
            double cool = General.MyRand.NextDouble() * (1.175 - 1.1) + 1.1;//(1.17 - 1.075) + 1.075;//(1.175 - 1.1) + 1.1;
            float MoveRate = (float)cool;
            NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_CLIMBOVERS", CopToTask.Pedestrian, true);
            NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_LADDERS", CopToTask.Pedestrian, true);
            NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_DROP_FROM_HEIGHT", CopToTask.Pedestrian, true);
            CopToTask.Pedestrian.BlockPermanentEvents = true;
           // CopToTask.Pedestrian.KeepTasks = true;
            if (PlayerState.WantedLevel >= 2)
                NativeFunction.CallByName<uint>("SET_PED_MOVE_RATE_OVERRIDE", CopToTask.Pedestrian, MoveRate);
            CurrentTaskLoop = "FootChase";
            CurrentSubTaskLoop = "";
            Debugging.WriteToLog("Tasking", string.Format("     Started FootChase: {0}", CopToTask.Pedestrian.Handle));
            FootChase_Normal();
        }
        private void FootChase_Normal()
        {
            CopToTask.Pedestrian.BlockPermanentEvents = true;
            //CopToTask.Pedestrian.KeepTasks = true;
            if (CurrentSubTaskLoop != "Shoot" && (!PlayerState.IsBusted && !AttemptingToSurrender) && CopToTask.DistanceToPlayer <= 7f)
            {
                Debugging.WriteToLog("Tasking", string.Format("     FootChase Shoot: {0}", CopToTask.Pedestrian.Handle));
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
            else if (CurrentSubTaskLoop != "Aim" && (PlayerState.IsBusted || AttemptingToSurrender) && CopToTask.DistanceToPlayer <= 7f)
            {
                Debugging.WriteToLog("Tasking", string.Format("     FootChase Aim: {0}", CopToTask.Pedestrian.Handle));
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
                Debugging.WriteToLog("Tasking", string.Format("     FootChase GoTo: {0}", CopToTask.Pedestrian.Handle));
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
            Vector3 WantedCenter = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
            if (CopToTask.IsInVehicle)
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
            Debugging.WriteToLog("Tasking", string.Format("     Started GoToLastSeen: {0}", CopToTask.Pedestrian.Handle));
        }
        private void GoToLastSeen_Normal()
        {
            Vector3 WantedCenter = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
            if (!NearWantedCenterThisWanted)
            {
                if (CurrentTaskedPosition.DistanceTo2D( WantedCenter) >= 5f)
                {
                    if (CopToTask.IsInVehicle)
                    {
                        NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", CopToTask.Pedestrian, CopToTask.Pedestrian.CurrentVehicle, WantedCenter.X, WantedCenter.Y, WantedCenter.Z, 20f, 4 | 16 | 32 | 262144, 20f);
                    }
                    else
                    {
                        CopToTask.Pedestrian.Tasks.GoStraightToPosition(WantedCenter, 15f, 0f, 2f, 0);
                    }
                    CurrentTaskedPosition = WantedCenter;
                    CurrentTaskLoop = "GoToLastSeen";
                    Debugging.WriteToLog("Tasking", string.Format("     Updated GoToLastSeen: {0}", CopToTask.Pedestrian.Handle));
                }
                if (CopToTask.DistanceToLastSeen <= 25f)
                {
                    NearWantedCenterThisWanted = true;
                    if(CopToTask.IsDriver && CopToTask.IsInVehicle)
                    {
                        CopToTask.Pedestrian.Tasks.CruiseWithVehicle(30f, VehicleDrivingFlags.Emergency);
                    }
                    else if(!CopToTask.IsInVehicle)
                    {
                        CopToTask.Pedestrian.Tasks.Wander();
                    }
                    Debugging.WriteToLog("Tasking", string.Format("     Post GoToLastSeen: {0}", CopToTask.Pedestrian.Handle));
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
            Cop ClosestCop = PedList.CopPeds.Where(x => x.Pedestrian.Exists() && x.IsDriver).OrderBy(x => x.DistanceToLastSeen).FirstOrDefault();
            if (ClosestCop == null)
                return;
            NativeFunction.CallByName<bool>("TASK_HELI_CHASE", CopToTask.Pedestrian, ClosestCop.Pedestrian, -50f, 50f, 60f);
            Debugging.WriteToLog("Tasking", string.Format("     Heli Lost you following closest cop: {0}", CopToTask.Pedestrian.Handle));
            NearWantedCenterThisWanted = false;
            CurrentTaskLoop = "HeliGoToLastSeen";
        }
        private void HeliGoToLastSeen_Normal()
        {

        }
        private void ArmCopAppropriately()
        {
            if (WantedLevelScript.IsDeadlyChase)
            {
                if (CopToTask.IsInVehicle && PlayerState.WantedLevel < 4)
                {
                    CopToTask.SetUnarmed();
                }
                else
                {
                    CopToTask.SetDeadly();
                }
            }
            else
            {
                if (PlayerState.IsNotWanted)
                {
                    CopToTask.SetUnarmed();
                }
                else
                {
                    CopToTask.SetLessLethal();
                }
            }
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
                if (CopToTask.IsDriver && CopToTask.Pedestrian.CurrentVehicle.HasSiren)
                {
                    CopToTask.Pedestrian.CurrentVehicle.IsSirenOn = false;
                    CopToTask.Pedestrian.CurrentVehicle.IsSirenSilent = false;
                }
                //if (PlayerState.IsWanted)
                //    NativeFunction.CallByName<bool>("SET_PED_ALERTNESS", CopToTask.Pedestrian, 3);

                CurrentTaskLoop = "None";
                CurrentSubTaskLoop = "";

                Debugging.WriteToLog("Tasking", string.Format("     ClearedTasks: {0}", CopToTask.Pedestrian.Handle));
            }

        }
    }
    private class TaskableCivilian
    {
        private uint GameTimeLastReportedCrime { get; set; }
        private uint GameTimeLastReactedToCrime { get; set; }
        private bool HasSeenPlayerCommitCrime { get; set; } = false;
        private enum Activities
        {
            Idle,
            ReactToCrime,
        }
        private string CurrentTaskLoop;
        private string CurrentSubTaskLoop;
        public PedExt CivilianToTask { get; set; }
        public uint GameTimeLastTasked { get; set; }
        public bool ShouldReportCrime
        {
            get
            {
                if(GameTimeLastReactedToCrime == 0)
                {
                    return false;
                }
                else if (Game.GameTime - GameTimeLastReactedToCrime < 15000)
                {
                    return false;
                }
                else if (!CivilianToTask.CrimesWitnessed.Any())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public string DebugTaskState
        {
            get
            {
                return string.Format("Loop: {0} DistanceToPlayer: {1} TimeTasked: {2}", CurrentTaskLoop, CivilianToTask.DistanceToPlayer, GameTimeLastTasked);
            }
        }
        public TaskableCivilian(PedExt _GTAPedToTask)
        {
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
                    if(HasSeenPlayerCommitCrime)
                    {
                        return Activities.ReactToCrime;
                    }
                    else if(CivilianToTask.CanRecognizePlayer && Crimes.PlayerViolatingInFrontOfCivilians)
                    {
                        return Activities.ReactToCrime;
                    }
                    else if (CivilianToTask.WithinWeaponsAudioRange && Crimes.PlayerViolatingAudioCivilians)
                    {
                        return Activities.ReactToCrime;
                    }
                }
                return Activities.Idle;
            }
        }
        public void RunCurrentActivity()
        {
            if  (CurrentActivity == Activities.ReactToCrime)
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
                    if (CivilianToTask.WillFight)
                    {
                        GiveWeapon();
                        NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", CivilianToTask.Pedestrian, 5, true);//BF_CanFightArmedPedsWhenNotArmed = 5,
                        CivilianToTask.Pedestrian.Tasks.FightAgainst(Game.LocalPlayer.Character);
                        CivilianToTask.Pedestrian.KeepTasks = true;
                    }
                    else if (CivilianToTask.WillCallPolice)
                    {
                        AddCurrentCrimes();
                        unsafe
                        {
                            int lol = 0;
                            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                            NativeFunction.CallByName<bool>("TASK_SMART_FLEE_PED", 0, Game.LocalPlayer.Character, 100f, 10000);
                            NativeFunction.CallByName<bool>("TASK_USE_MOBILE_PHONE_TIMED", 0, 10000);
                            NativeFunction.CallByName<bool>("TASK_SMART_FLEE_PED", 0, Game.LocalPlayer.Character, 100f, -1);
                            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", CivilianToTask.Pedestrian, lol);
                            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                        }
                        CivilianToTask.Pedestrian.IsPersistent = true;
                    }
                    else
                    {
                        CivilianToTask.Pedestrian.Tasks.Flee(Game.LocalPlayer.Character, 100f, -1);
                    }
                    HasSeenPlayerCommitCrime = true;
                    GameTimeLastReactedToCrime = Game.GameTime;
                    Debugging.WriteToLog("Tasking", string.Format("     Started ReactToCrime: {0} Old CurrentTaskLoop: {1} Fight: {2} CallPolice: {3}", CivilianToTask.Pedestrian.Handle, CurrentTaskLoop, CivilianToTask.WillFight,CivilianToTask.WillCallPolice));
                }
            }
            else
            {
                if(CivilianToTask.WillFight)
                {

                }
                else if (CivilianToTask.WillCallPolice && ShouldReportCrime)
                {
                    ReportCrime();
                }
            }
            CurrentTaskLoop = "ReactToCrime";
            GameTimeLastTasked = Game.GameTime;
        }
        private void ReportCrime()
        {
            if (CivilianToTask.Pedestrian.Exists() && CivilianToTask.Pedestrian.IsAlive && !CivilianToTask.Pedestrian.IsRagdoll)
            {
                if (PlayerState.IsNotWanted)
                {
                    WantedLevelScript.CurrentCrimes.AddCrime(CivilianToTask.CrimesWitnessed.OrderBy(x => x.Priority).FirstOrDefault(), false, CivilianToTask.PositionLastSeenCrime, null, null);
                    
                    //WANT TO READD THIS IN TEH FUTURE, NEED TO PASS CAR
                    //if (VehToReport != null)
                    //    VehToReport.WasReportedStolen = true;//even if it doesnt make it to us

                    Investigation.InInvestigationMode = true;

                    if (CivilianToTask.EverSeenPlayer && CivilianToTask.ClosestDistanceToPlayer <= 20f)
                        Investigation.HavePlayerDescription = true;

                    if (CivilianToTask.EverSeenPlayer)
                        Investigation.InvestigationPosition = CivilianToTask.PositionLastSeenPlayer;
                    else if (CivilianToTask.PositionLastSeenCrime != Vector3.Zero)
                        Investigation.InvestigationPosition = CivilianToTask.PositionLastSeenCrime;
                    else
                        Investigation.InvestigationPosition = CivilianToTask.Pedestrian.Position;
                }
                else
                {
                    if (PlayerState.AreStarsGreyedOut)
                    {
                        Vector3 UpdatedPosition;
                        if (CivilianToTask.EverSeenPlayer)
                            UpdatedPosition = CivilianToTask.PositionLastSeenPlayer;
                        else if (CivilianToTask.PositionLastSeenCrime != Vector3.Zero && CivilianToTask.PositionLastSeenCrime != Vector3.Zero)
                            UpdatedPosition = CivilianToTask.PositionLastSeenCrime;
                        else
                            UpdatedPosition = CivilianToTask.Pedestrian.Position;

                        Police.PlaceLastSeenPlayer = UpdatedPosition;
                    }
                }
                CivilianToTask.CrimesWitnessed.Clear();
                CivilianToTask.Pedestrian.IsPersistent = false;
                GameTimeLastReportedCrime = Game.GameTime;
            }
        }
        private void AddCurrentCrimes()
        {
            foreach(Crime CurrentlyViolating in Crimes.CurrentlyViolatingCanBeReportedByCivilians)
            {
                CivilianToTask.AddCrime(CurrentlyViolating, Game.LocalPlayer.Character.Position);
            }
        }
        private void GiveWeapon()
        {
            GTAWeapon myGun = Weapons.GetRandomRegularWeapon();
            if (myGun != null)
                Game.LocalPlayer.Character.Inventory.GiveNewWeapon(myGun.Name, myGun.AmmoAmount, true);
        }
    }  
}

