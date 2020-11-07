using Rage;
using Rage.Native;
using System.Collections.Generic;
using System.Linq;

public static class NewTasking
{
    private static List<TaskableCop> TaskableCops;
    public static bool IsRunning { get; set; }
    public static void Initialize()
    {
        IsRunning = true;
        TaskableCops = new List<TaskableCop>();
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void TaskPeds()
    {
        if (IsRunning)
        {
            AddTaskablePeds();
        }
    }
    private static void AddTaskablePeds()
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
    }
    private static void TaskLoop()
    {
        foreach (TaskableCop Cop in TaskableCops.Where(x => x.CopToTask.Pedestrian.Exists()))
        {
            //if(Cop.NeedsTasking)
            //{

            //}
        }
    }
    private class TaskableCop
    {
        private float OnFootTaskDistance = 70f;
        private bool FirstLoopExecution = true;
        private Vector3 CurrentTaskedPosition = Vector3.Zero;
        private bool 
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
        private void PickTaskLoop()
        {
            string TaskLoop = "None";
            if (CurrentActivity == Activities.Idle)
            {
                TaskLoop = "Idle";
            }
            else if (CurrentActivity == Activities.Investigate)
            {
                float DisanceToInvestigation = CopToTask.Pedestrian.DistanceTo2D(Investigation.InvestigationPosition);
                if (DisanceToInvestigation <= Investigation.InvestigationDistance)
                {
                    if (CopToTask.IsInVehicle)
                    {
                        TaskLoop = "Investigate_InVehicle";
                    }
                    else if (DisanceToInvestigation <= OnFootTaskDistance)
                    {
                        TaskLoop = "Investigate_OnFoot";
                    }
                }
            }
            else if (CurrentActivity == Activities.Chase)
            {
                if (CopToTask.WithinChaseDistance)
                {
                    if (Police.AnyRecentlySeenPlayer)
                    {
                        if (CurrentDynamic == AIDynamic.Cop_InVehicle_Player_InVehicle)
                        {
                            if (CopToTask.DistanceToPlayer <= 25f || CopToTask.CanSeePlayer)
                            {
                                TaskLoop = "None";
                            }
                            else
                            {
                                TaskLoop = "VehicleChase_InVehicle";
                            }
                        }
                        else if (CurrentDynamic == AIDynamic.Cop_InVehicle_Player_OnFoot)
                        {
                            TaskLoop = "None";
                        }
                        else if (CurrentDynamic == AIDynamic.Cop_OnFoot_Player_InVehicle)
                        {
                            TaskLoop = "None";
                        }
                        else if (CurrentDynamic == AIDynamic.Cop_OnFoot_Player_OnFoot)
                        {
                            if (CopToTask.DistanceToPlayer <= OnFootTaskDistance || CopToTask.CanSeePlayer)
                            {
                                TaskLoop = "FootChase_OnFoot";
                            }
                        }
                    }
                    else
                    {
                        if (CopToTask.IsInVehicle)
                        {
                            TaskLoop = "GoToLastSeen_InVehicle";
                        }
                        else if (CopToTask.DistanceToLastSeen <= OnFootTaskDistance)
                        {
                            TaskLoop = "GoToLastSeen_OnFoot";
                        }
                    }
                }
            }
        }
        private void Idle()
        {
            if (FirstLoopExecution)
            {
                if (CopToTask.Pedestrian.Exists())
                {
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
        }
        private void Investigate()
        {
            if(FirstLoopExecution)
            {
                InvestigateStart();
            }
            else
            {
                InvestigateNormal();
            }
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
            if (CopToTask.Pedestrian.IsInAnyVehicle(false))
                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", CopToTask.Pedestrian, CopToTask.Pedestrian.CurrentVehicle, Investigation.InvestigationPosition.X, Investigation.InvestigationPosition.Y, Investigation.InvestigationPosition.Z, DrivingSpeed, 4 | 16 | 32 | 262144, 10f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, PositionOfInterest.X, PositionOfInterest.Y, PositionOfInterest.Z, 70f, 4 | 16 | 32 | 262144, 35f);
            else
                NativeFunction.CallByName<bool>("TASK_GO_STRAIGHT_TO_COORD", CopToTask.Pedestrian, Investigation.InvestigationPosition.X, Investigation.InvestigationPosition.Y, Investigation.InvestigationPosition.Z, 500f, -1, 0f, 2f);

            uint GameTimestartedInvestigation = Game.GameTime;
            Debugging.WriteToLog("Tasking", string.Format("Started Investigate: {0}, CurrentResponse {1}, DrivingSpeed {2}, NeedSirenOn {3}", CopToTask.Pedestrian.Handle, WantedLevelScript.CurrentResponse, DrivingSpeed, NeedSirenOn));
        }
        private void InvestigateNormal()
        {
            if (Investigation.InvestigationPosition != Vector3.Zero && Investigation.InvestigationPosition != CurrentTaskedPosition) //retask them if it changes
            {
                CurrentTaskedPosition = Investigation.InvestigationPosition;
                if (CopToTask.Pedestrian.IsInAnyVehicle(false))
                    NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", CopToTask.Pedestrian, CopToTask.Pedestrian.CurrentVehicle, Investigation.InvestigationPosition.X, Investigation.InvestigationPosition.Y, Investigation.InvestigationPosition.Z, DrivingSpeed, 4 | 16 | 32 | 262144, 10f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, PositionOfInterest.X, PositionOfInterest.Y, PositionOfInterest.Z, 70f, 4 | 16 | 32 | 262144, 35f);
                else
                    NativeFunction.CallByName<bool>("TASK_GO_STRAIGHT_TO_COORD", CopToTask.Pedestrian, Investigation.InvestigationPosition.X, Investigation.InvestigationPosition.Y, Investigation.InvestigationPosition.Z, 500f, -1, 0f, 2f);

                Debugging.WriteToLog("Tasking", string.Format("Reset Investigate: {0}, CurrentResponse {1}, DrivingSpeed {2}, NeedSirenOn {3}", CopToTask.Pedestrian.Handle, WantedLevelScript.CurrentResponse, DrivingSpeed, NeedSirenOn));

            }
            if (CopToTask.IsDriver && CopToTask.Pedestrian.CurrentVehicle.HasSiren && NeedSirenOn)
            {
                if (!CopToTask.Pedestrian.CurrentVehicle.IsSirenOn)
                {
                    CopToTask.Pedestrian.CurrentVehicle.IsSirenOn = true;
                    CopToTask.Pedestrian.CurrentVehicle.IsSirenSilent = false;
                }
            }
        }
    }
}

