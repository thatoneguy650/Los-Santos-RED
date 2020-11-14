using Rage;
using Rage.Native;
using System.Collections.Generic;
using System.Linq;

public static class NewTasking
{
    private static uint GameTimeLastBusted;
    private static bool SurrenderBust;
    private static List<TaskableCop> TaskableCops;
    private static List<TaskableCivilian> TaskableCivilians;
    public static bool IsRunning { get; set; }
    private static bool IsBustTimeOut
    {
        get
        {
            if (WantedLevelScript.HasBeenWantedFor <= 3000)
                return true;
            else if (Surrender.IsCommitingSuicide)
                return true;
            else if (Game.GameTime - GameTimeLastBusted >= 10000)
                return false;
            else
                return true;
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
            }

            foreach (TaskableCivilian Civilian in TaskableCivilians.Where(x => x.CivilianToTask.Pedestrian.Exists()))
            {
                Civilian.RunCurrentActivity();
            }

            if (SurrenderBust && !IsBustTimeOut)
                SurrenderBustEvent();
        }
    }
    public static void PrintActivities()
    {
        Debugging.WriteToLog("Tasking", "================================================");
        foreach (TaskableCop Cop in TaskableCops.Where(x => x.CopToTask.Pedestrian.Exists()))
        {
            Debugging.WriteToLog("Tasking", string.Format("{0}, {1}",Cop.CopToTask.Pedestrian.Handle, Cop.DebugTaskState));
        }
        Debugging.WriteToLog("Tasking", "================================================");
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
            GameTimeLastBusted = Game.GameTime;
            Debugging.WriteToLog("Tasking", "SurrenderBust Executed");
        }
    }
    private static void SetSurrenderBust(bool ValueToSet, string DebugReason)
    {
        SurrenderBust = ValueToSet;
        Debugging.WriteToLog("Tasking", string.Format("Set Surrender Bust Reason: {0}", DebugReason));
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
            None();
        }
        private void None()
        {
            CopToTask.Pedestrian.BlockPermanentEvents = false;
        }
        private void Idle()
        {
            if (CurrentTaskLoop != "Idle")
            {
                if (CopToTask.Pedestrian.Exists())
                {
                    ClearTasks();
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
                    Debugging.WriteToLog("Tasking", string.Format("     Started Idle: {0} Old CurrentTaskLoop: {1}", CopToTask.Pedestrian.Handle, CurrentTaskLoop));
                }
            }
            else
            {
                if (CopToTask.Pedestrian.Exists() && CopToTask.IsDriver && CopToTask.Pedestrian.CurrentVehicle.HasSiren)
                {
                    CopToTask.Pedestrian.CurrentVehicle.IsSirenOn = false;
                    CopToTask.Pedestrian.CurrentVehicle.IsSirenSilent = false;
                } 
            }
            CurrentTaskLoop = "Idle";
            GameTimeLastTasked = Game.GameTime;
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
                        InvestigateStart();
                    }
                    else
                    {
                        InvestigateNormal();
                    }
                    GameTimeLastTasked = Game.GameTime;
                }
            }
            CurrentTaskLoop = "Investigation";
        }
        private void InvestigateStart()
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
        private void InvestigateNormal()
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
                        if (CopToTask.DistanceToPlayer <= 25f || CopToTask.CanSeePlayer)
                        {
                            CurrentTaskLoop = "None";
                        }
                        else
                        {
                            //CurrentTaskLoop = "VehicleChase_InVehicle";
                            VehicleChase();
                        }
                    }
                    else if (CurrentDynamic == AIDynamic.Cop_InVehicle_Player_OnFoot)
                    {
                        CurrentTaskLoop = "None";
                    }
                    else if (CurrentDynamic == AIDynamic.Cop_OnFoot_Player_InVehicle)
                    {
                        CurrentTaskLoop = "None";
                    }
                    else if (CurrentDynamic == AIDynamic.Cop_OnFoot_Player_OnFoot)
                    {
                        if (CopToTask.DistanceToPlayer <= OnFootTaskDistance || CopToTask.RecentlySeenPlayer)
                        {
                            //CurrentTaskLoop = "FootChase_OnFoot";
                            FootChase();
                        }
                    }
                }
                else
                {
                    if (CopToTask.IsInVehicle)
                    {
                        //CurrentTaskLoop = "GoToLastSeen_InVehicle";
                        GoToLastSeen();
                    }
                    else if (CopToTask.DistanceToLastSeen <= OnFootTaskDistance)
                    {
                        //CurrentTaskLoop = "GoToLastSeen_OnFoot";
                        GoToLastSeen();
                    }
                }
            }
        }
        private void VehicleChase()
        {
            if (!CopToTask.Pedestrian.Exists() || !CopToTask.IsDriver)
                return;

            if (CurrentTaskLoop != "VehicleChase")
            {
                VehicleChaseStart();
            }
            else
            {
                VehicleChaseNormal();
            }
        }
        private void VehicleChaseStart()
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
        private void VehicleChaseNormal()
        {
            Vector3 WantedCenter = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
            if(CurrentTaskedPosition.DistanceTo2D(WantedCenter) >= 10f)
            {
                if (!PlayerState.IsInVehicle)
                {
                    NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", CopToTask.Pedestrian, CopToTask.Pedestrian.CurrentVehicle, WantedCenter.X, WantedCenter.Y, WantedCenter.Z, 20f, 4 | 16 | 32 | 262144, 20f);
                    Debugging.WriteToLog("Tasking", string.Format("     Updated VehicleChase: {0}", CopToTask.Pedestrian.Handle));
                }
                CurrentTaskedPosition = WantedCenter;
            }
        }
        private void FootChase()
        {
            if (!CopToTask.Pedestrian.Exists())
                return;

            if(CurrentTaskLoop != "FootChase")
            {
                FootChase_Start();
            }
            else
            {
                FootChase_Normal();
            }

            if ((PlayerState.HandsAreUp || Game.LocalPlayer.Character.IsStunned || Game.LocalPlayer.Character.IsRagdoll || PlayerState.IsStationary) && !PlayerState.IsBusted && CopToTask.DistanceToPlayer >= 0.1f && CopToTask.DistanceToPlayer <= 5f && !IsBustTimeOut)// && !Police.PlayerWasJustJacking)
                SetSurrenderBust(true, string.Format("TaskPoliceOnFoot 1: {0}", CopToTask.Pedestrian.Handle));
        }
        private void FootChase_Start()
        {
            double cool = General.MyRand.NextDouble() * (1.175 - 1.1) + 1.1;//(1.17 - 1.075) + 1.075;//(1.175 - 1.1) + 1.1;
            float MoveRate = (float)cool;
            NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_CLIMBOVERS", CopToTask.Pedestrian, true);
            NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_LADDERS", CopToTask.Pedestrian, true);
            NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_DROP_FROM_HEIGHT", CopToTask.Pedestrian, true);
            CopToTask.Pedestrian.BlockPermanentEvents = true;
            CopToTask.Pedestrian.KeepTasks = true;
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
            CopToTask.Pedestrian.KeepTasks = true;
            if (CurrentSubTaskLoop != "Shoot" && !PlayerState.IsBusted && CopToTask.DistanceToPlayer <= 7f)
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
            else if (CurrentSubTaskLoop != "Aim" && PlayerState.IsBusted && CopToTask.DistanceToPlayer <= 7f)
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
                GoToLastSeenStart();
            }
            else
            {
                GoToLastSeenNormal();
            }  
        }
        private void GoToLastSeenStart()
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
        private void GoToLastSeenNormal()
        {
            Vector3 WantedCenter = NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
            if (!NearWantedCenterThisWanted)
            {
                if (CurrentTaskedPosition != WantedCenter)
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
                CopToTask.SetTazer();
            }
        }
        private void ClearTasks()
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
                if (PlayerState.IsWanted)
                    NativeFunction.CallByName<bool>("SET_PED_ALERTNESS", CopToTask.Pedestrian, 3);

                Debugging.WriteToLog("Tasking", string.Format("     ClearedTasks: {0}", CopToTask.Pedestrian.Handle));
            }

        }
    }
    private class TaskableCivilian
    {
        private enum Activities
        {
            Idle,
            Flee,
            Fight,
        }
        private string CurrentTaskLoop;
        public PedExt CivilianToTask { get; set; }
        public uint GameTimeLastTasked { get; set; }
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
                    if (CivilianToTask.CanRecognizePlayer && Crimes.CiviliansCanReport)
                    {
                        return Activities.Flee;
                    }
                    else if (CivilianToTask.CanHearPlayerShooting && Crimes.CiviliansCanAudioReport)
                    {
                        return Activities.Flee;
                    }     
                }
                return Activities.Idle;
            }
        }
        public void RunCurrentActivity()
        {
            if(CurrentActivity == Activities.Flee)
            {
                Flee();
                return;
            }
            else if  (CurrentActivity == Activities.Idle)
            {

            }
            CurrentTaskLoop = "None";
        }
        private void Flee()
        {
            if (CurrentTaskLoop != "Flee")
            {
                if (CivilianToTask.Pedestrian.Exists())
                {
                    CivilianToTask.Pedestrian.Tasks.Flee(Game.LocalPlayer.Character, 100f, -1);
                    Debugging.WriteToLog("Tasking", string.Format("     Started Flee: {0} Old CurrentTaskLoop: {1}", CivilianToTask.Pedestrian.Handle, CurrentTaskLoop));
                }
            }
            else
            {

            }
            CurrentTaskLoop = "Flee";
            GameTimeLastTasked = Game.GameTime;
        }
    }  
}

