using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Util.Tasking
{
    //public class Activity
    //{
    //    public Cop Cop { get; set; }
    //    public string CurrentTaskLoop { get; set; }
    //    public string CurrentSubTaskLoop { get; set; }
    //    public uint GameTimeLastRanActivity { get; set; }
    //    public uint GameTimeLastTasked { get; set; }
    //    public Vector3 CurrentTaskedPosition { get; set; } = Vector3.Zero;
    //    public string DebugTaskState
    //    {
    //        get
    //        {
    //            return string.Format("Loop: {0} SubLoop: {1} TimeTasked: {2}", CurrentTaskLoop, CurrentSubTaskLoop, GameTimeLastTasked);
    //        }
    //    }
    //    public Activity(Cop cop)
    //    {
    //        Cop = cop;
    //    }
    //    public void ClearTasks()//temp public
    //    {
    //        if (Cop.Pedestrian.Exists())
    //        {
    //            int seatIndex = 0;
    //            Vehicle CurrentVehicle = null;
    //            bool WasInVehicle = false;
    //            if (Cop.WasModSpawned && Cop.Pedestrian.IsInAnyVehicle(false))
    //            {
    //                WasInVehicle = true;
    //                CurrentVehicle = Cop.Pedestrian.CurrentVehicle;
    //                seatIndex = Cop.Pedestrian.SeatIndex;
    //            }
    //            Cop.Pedestrian.Tasks.Clear();

    //            Cop.Pedestrian.BlockPermanentEvents = false;
    //            Cop.Pedestrian.KeepTasks = false;
    //            Cop.Pedestrian.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Player, Relationship.Neutral);

    //            if (!Cop.WasModSpawned)
    //                Cop.Pedestrian.IsPersistent = false;

    //            if (Cop.WasModSpawned && WasInVehicle && !Cop.Pedestrian.IsInAnyVehicle(false) && CurrentVehicle != null)
    //            {
    //                Cop.Pedestrian.WarpIntoVehicle(CurrentVehicle, seatIndex);

    //            }
    //            if (Cop.IsDriver && Cop.Pedestrian.CurrentVehicle != null && Cop.Pedestrian.CurrentVehicle.HasSiren)
    //            {
    //                Cop.Pedestrian.CurrentVehicle.IsSirenOn = false;
    //                Cop.Pedestrian.CurrentVehicle.IsSirenSilent = false;
    //            }
    //            if (Mod.Player.Instance.IsWanted)
    //            {
    //                NativeFunction.CallByName<bool>("SET_PED_ALERTNESS", Cop.Pedestrian, 3);
    //                if (Mod.Player.Instance.CurrentPoliceResponse.IsDeadlyChase)
    //                {
    //                    Cop.Pedestrian.Tasks.FightAgainst(Game.LocalPlayer.Character, -1);//just help them incase they get confused
    //                }
    //            }
    //            CurrentTaskLoop = "None";
    //            CurrentSubTaskLoop = "";

    //            Debug.Instance.WriteToLog("Tasking", string.Format("     ClearedTasks: {0}", Cop.Pedestrian.Handle));
    //        }

    //    }
    //}
    //public class Investigate : Activity
    //{
        
    //    private bool AtInvesstigationPositionThisInvestigation = false;
    //    private bool PositionChanged
    //    {
    //        get
    //        {
    //            return Mod.Player.Instance.Investigations.Position != Vector3.Zero && Mod.Player.Instance.Investigations.Position != CurrentTaskedPosition;
    //        }
    //    }
    //    public Investigate(Cop PedToAssign) : base(PedToAssign)
    //    {
    //        PedToAssign.Pedestrian.BlockPermanentEvents = false;
    //    }
    //    public void Update()
    //    {
    //        if (!AtInvesstigationPositionThisInvestigation)
    //        {
    //            if (PositionChanged)
    //            {
    //                UpdatePositionToSearch();
    //            }
    //            CheckArrivedAtPosition();
    //        }
    //        SetSiren();
    //    }
    //    private void UpdatePositionToSearch()
    //    {
    //        CurrentTaskedPosition = Mod.Player.Instance.Investigations.Position;
    //        if (Cop.Pedestrian.IsInAnyVehicle(false))
    //        {
    //            if (Cop.IsDriver)
    //            {
    //                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, Mod.Player.Instance.Investigations.Position.X, Mod.Player.Instance.Investigations.Position.Y, Mod.Player.Instance.Investigations.Position.Z, Mod.Player.Instance.CurrentPoliceResponse.ResponseDrivingSpeed, 4 | 16 | 32 | 262144, 10f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, PositionOfInterest.X, PositionOfInterest.Y, PositionOfInterest.Z, 70f, 4 | 16 | 32 | 262144, 35f);
    //            }
    //        }
    //        else
    //        {
    //            NativeFunction.CallByName<bool>("TASK_GO_STRAIGHT_TO_COORD", Cop.Pedestrian, Mod.Player.Instance.Investigations.Position.X, Mod.Player.Instance.Investigations.Position.Y, Mod.Player.Instance.Investigations.Position.Z, 500f, -1, 0f, 2f);
    //        }
    //        Debug.Instance.WriteToLog("Tasking", string.Format("     Reset Investigate: {0}, CurrentResponse {1}, DrivingSpeed {2}, NeedSirenOn {3}", Cop.Pedestrian.Handle, Mod.Player.Instance.CurrentPoliceResponse.CurrentResponse, Mod.Player.Instance.CurrentPoliceResponse.ResponseDrivingSpeed, Mod.Player.Instance.CurrentPoliceResponse.ShouldSirenBeOn));
    //    }
    //    private void SetSiren()
    //    {
    //        if (Cop.IsDriver && Cop.Pedestrian.CurrentVehicle != null && Cop.Pedestrian.CurrentVehicle.HasSiren && Mod.Player.Instance.CurrentPoliceResponse.ShouldSirenBeOn)
    //        {
    //            if (!Cop.Pedestrian.CurrentVehicle.IsSirenOn)
    //            {
    //                Cop.Pedestrian.CurrentVehicle.IsSirenOn = true;
    //                Cop.Pedestrian.CurrentVehicle.IsSirenSilent = false;
    //            }
    //        }
    //    }
    //    private void CheckArrivedAtPosition()
    //    {
    //        if (Cop.DistanceToInvestigationPosition <= 15f)
    //        {
    //            AtInvesstigationPositionThisInvestigation = true;
    //            if (Cop.Pedestrian.Exists())
    //            {
    //                if (Cop.Pedestrian.IsInAnyVehicle(false) && Cop.Pedestrian.CurrentVehicle.Exists())
    //                {
    //                    NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, Mod.Player.Instance.CurrentPoliceResponse.ResponseDrivingSpeed, 4 | 16 | 32 | 262144, 10f);
    //                }
    //                else
    //                {
    //                    Cop.Pedestrian.Tasks.Wander();
    //                }
    //                Debug.Instance.WriteToLog("Tasking", string.Format("     Started Investigation Wander: {0}", Cop.Pedestrian.Handle));
    //            }
    //        }
    //    }
    //}
    //public class Chase : Activity
    //{
    //    private bool NearWantedCenterThisWanted;
    //    private enum AIDynamic
    //    {
    //        Cop_InVehicle_Player_InVehicle,
    //        Cop_InVehicle_Player_OnFoot,
    //        Cop_OnFoot_Player_InVehicle,
    //        Cop_OnFoot_Player_OnFoot,
    //    }
    //    private AIDynamic CurrentDynamic
    //    {
    //        get
    //        {
    //            if (Mod.Player.Instance.IsInVehicle)
    //            {
    //                if (Cop.IsInVehicle)
    //                {
    //                    return AIDynamic.Cop_InVehicle_Player_InVehicle;
    //                }
    //                else
    //                {
    //                    return AIDynamic.Cop_OnFoot_Player_InVehicle;
    //                }
    //            }
    //            else
    //            {
    //                if (Cop.IsInVehicle)
    //                {
    //                    return AIDynamic.Cop_InVehicle_Player_OnFoot;
    //                }
    //                else
    //                {
    //                    return AIDynamic.Cop_OnFoot_Player_OnFoot;
    //                }
    //            }
    //        }
    //    }
    //    private bool WithinChaseDistance
    //    {
    //        get
    //        {
    //            if (Cop.DistanceToPlayer <= Mod.World.Instance.ActiveDistance)
    //                return true;
    //            else
    //                return false;
    //        }
    //    }
    //    public Chase(Cop PedToAssign) : base(PedToAssign)
    //    {
    //        PedToAssign.Pedestrian.BlockPermanentEvents = false;
    //    }
    //    public void Update()
    //    {
    //        if (WithinChaseDistance)
    //        {
    //            if (Mod.World.Instance.AnyPoliceRecentlySeenPlayer && !Mod.Player.Instance.AreStarsGreyedOut)
    //            {
    //                if (CurrentDynamic == AIDynamic.Cop_InVehicle_Player_InVehicle)
    //                {
    //                    if (!Cop.IsInHelicopter)
    //                    {
    //                        if (Cop.DistanceToPlayer <= 25f || Cop.CanSeePlayer)
    //                        {
    //                            CurrentTaskLoop = "None";
    //                        }
    //                        else
    //                        {
    //                            VehicleChase();
    //                        }
    //                    }
    //                    else
    //                    {
    //                        HeliChase();
    //                    }
    //                }
    //                else if (CurrentDynamic == AIDynamic.Cop_InVehicle_Player_OnFoot)
    //                {
    //                    if (Cop.DistanceToPlayer >= 30f)
    //                    {
    //                        VehicleChase();
    //                    }
    //                }
    //                else if (CurrentDynamic == AIDynamic.Cop_OnFoot_Player_InVehicle)
    //                {
    //                    if (Cop.DistanceToPlayer <= 10f)
    //                    {
    //                        CarJack();
    //                    }
    //                }
    //                else if (CurrentDynamic == AIDynamic.Cop_OnFoot_Player_OnFoot)
    //                {
    //                    if (Cop.DistanceToPlayer <= 25f || Cop.RecentlySeenPlayer)
    //                    {
    //                        if (Mod.Player.Instance.CurrentPoliceResponse.IsDeadlyChase && !Mod.Player.Instance.IsAttemptingToSurrender)
    //                        {
    //                            Kill();
    //                        }
    //                        else
    //                        {
    //                            FootChase();
    //                        }
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                if (Cop.IsInVehicle)
    //                {
    //                    if (!Cop.IsInHelicopter)
    //                    {
    //                        GoToLastSeen();
    //                    }
    //                    else
    //                    {
    //                        HeliGoToLastSeen();
    //                    }
    //                }
    //                else if (Cop.DistanceToLastSeen <= 25f)
    //                {
    //                    GoToLastSeen();
    //                }
    //            }
    //        }
    //    }
    //    private void VehicleChase()
    //    {
    //        if (!Cop.Pedestrian.Exists() || !Cop.IsDriver)
    //            return;

    //        if (CurrentTaskLoop != "VehicleChase")
    //        {
    //            VehicleChase_Start();
    //        }
    //        else
    //        {
    //            VehicleChase_Normal();
    //        }
    //    }
    //    private void VehicleChase_Start()
    //    {
    //        Cop.Pedestrian.BlockPermanentEvents = false;
    //        Vector3 WantedCenter = Mod.World.Instance.PlacePoliceLastSeenPlayer;//NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
    //        if (Mod.Player.Instance.IsInVehicle)
    //        {
    //            NativeFunction.CallByName<bool>("TASK_VEHICLE_CHASE", Cop.Pedestrian, Game.LocalPlayer.Character); //NativeFunction.CallByName<bool>("TASK_VEHICLE_FOLLOW", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, Game.LocalPlayer.Character, 22f, 4 | 16 | 32 | 262144, 8f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_CHASE", Cop.Pedestrian, Game.LocalPlayer.Character);
    //        }
    //        else
    //        {
    //            NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, WantedCenter.X, WantedCenter.Y, WantedCenter.Z, 20f, 4 | 16 | 32 | 262144, 20f);
    //        }
    //        CurrentTaskedPosition = WantedCenter;
    //        CurrentTaskLoop = "VehicleChase";
    //        Debug.Instance.WriteToLog("Tasking", string.Format("     Started VehicleChase: {0}", Cop.Pedestrian.Handle));
    //    }
    //    private void VehicleChase_Normal()
    //    {
    //        Vector3 WantedCenter = Mod.World.Instance.PlacePoliceLastSeenPlayer; //NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
    //        if (CurrentTaskedPosition.DistanceTo2D(WantedCenter) >= 10f)
    //        {
    //            if (!Mod.Player.Instance.IsInVehicle)
    //            {
    //                if (Cop.Pedestrian.CurrentVehicle.Exists())
    //                {
    //                    NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, WantedCenter.X, WantedCenter.Y, WantedCenter.Z, 20f, 4 | 16 | 32 | 262144, 20f);
    //                    Debug.Instance.WriteToLog("Tasking", string.Format("     Updated VehicleChase: {0}", Cop.Pedestrian.Handle));
    //                }
    //            }
    //            CurrentTaskedPosition = WantedCenter;
    //        }
    //        if (Cop.Pedestrian.CurrentVehicle.Exists() && Cop.Pedestrian.CurrentVehicle.HasSiren && !Cop.Pedestrian.CurrentVehicle.IsSirenOn)
    //        {
    //            Cop.Pedestrian.CurrentVehicle.IsSirenOn = true;
    //            Cop.Pedestrian.CurrentVehicle.IsSirenSilent = false;
    //        }
    //    }
    //    private void HeliChase()
    //    {
    //        if (!Cop.Pedestrian.Exists() || !Cop.IsDriver || !Cop.IsInHelicopter)
    //            return;

    //        if (CurrentTaskLoop != "HeliChase")
    //        {
    //            NativeFunction.CallByName<bool>("TASK_HELI_CHASE", Cop.Pedestrian, Game.LocalPlayer.Character, -50f, 50f, 60f);
    //            CurrentTaskLoop = "HeliChase";
    //            GameTimeLastTasked = Game.GameTime;
    //            Debug.Instance.WriteToLog("Tasking", string.Format("     Started HeliChase: {0}", Cop.Pedestrian.Handle));
    //        }
    //    }
    //    private void CarJack()
    //    {
    //        if (CurrentTaskLoop != "CarJack")
    //        {
    //            if (Cop.Pedestrian.Exists())
    //            {
    //                if (Mod.Player.Instance.IsInVehicle && Game.LocalPlayer.Character.CurrentVehicle.Exists())
    //                {
    //                    NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", Cop.Pedestrian, Game.LocalPlayer.Character.CurrentVehicle, -1, -1, 2f, 9);
    //                }
    //                Debug.Instance.WriteToLog("Tasking", string.Format("     Started CarJack: {0} Old CurrentTaskLoop: {1}", Cop.Pedestrian.Handle, CurrentTaskLoop));
    //            }
    //        }
    //        CurrentTaskLoop = "CarJack";
    //        GameTimeLastTasked = Game.GameTime;
    //    }
    //    private void FootChase()
    //    {
    //        if (!Cop.Pedestrian.Exists())
    //            return;

    //        if (CurrentTaskLoop != "FootChase")
    //        {
    //            FootChase_Start();
    //        }
    //        else
    //        {
    //            FootChase_Normal();
    //        }
    //    }
    //    private void FootChase_Start()
    //    {
    //        double cool = RandomItems.MyRand.NextDouble() * (1.175 - 1.1) + 1.1;//(1.17 - 1.075) + 1.075;//(1.175 - 1.1) + 1.1;
    //        float MoveRate = (float)cool;
    //        NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_CLIMBOVERS", Cop.Pedestrian, true);
    //        NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_USE_LADDERS", Cop.Pedestrian, true);
    //        NativeFunction.CallByName<bool>("SET_PED_PATH_CAN_DROP_FROM_HEIGHT", Cop.Pedestrian, true);
    //        Cop.Pedestrian.BlockPermanentEvents = true;
    //        Cop.Pedestrian.KeepTasks = true;
    //        if (Mod.Player.Instance.WantedLevel >= 2)
    //            NativeFunction.CallByName<uint>("SET_PED_MOVE_RATE_OVERRIDE", Cop.Pedestrian, MoveRate);
    //        CurrentTaskLoop = "FootChase";
    //        CurrentSubTaskLoop = "";
    //        Debug.Instance.WriteToLog("Tasking", string.Format("     Started FootChase: {0}", Cop.Pedestrian.Handle));
    //        FootChase_Normal();
    //    }
    //    private void FootChase_Normal()
    //    {
    //        Cop.Pedestrian.BlockPermanentEvents = true;
    //        Cop.Pedestrian.KeepTasks = true;



    //        //GET_SCRIPT_TASK_STATUS
    //        //uses eScriptTaskHash
    //        //maybe is just the status?
    //        //Can be used to get the actual task assigned instead of using the subtask strings?


    //        if (CurrentSubTaskLoop != "Shoot" && (!Mod.Player.Instance.IsBusted && !Mod.Player.Instance.IsAttemptingToSurrender) && Cop.DistanceToPlayer <= 7f)
    //        {
    //            Debug.Instance.WriteToLog("Tasking", string.Format("     FootChase Shoot: {0}", Cop.Pedestrian.Handle));
    //            CurrentSubTaskLoop = "Shoot";
    //            unsafe
    //            {
    //                int lol = 0;
    //                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
    //                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY_WHILE_AIMING_AT_ENTITY", 0, Game.LocalPlayer.Character, Game.LocalPlayer.Character, 200f, true, 4.0f, 200f, false, false, (uint)FiringPattern.DelayFireByOneSecond);
    //                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
    //                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
    //                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.Pedestrian, lol);
    //                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
    //            }
    //        }
    //        else if (CurrentSubTaskLoop != "Aim" && (Mod.Player.Instance.IsBusted || Mod.Player.Instance.IsAttemptingToSurrender) && Cop.DistanceToPlayer <= 7f)
    //        {
    //            Debug.Instance.WriteToLog("Tasking", string.Format("     FootChase Aim: {0}", Cop.Pedestrian.Handle));
    //            CurrentSubTaskLoop = "Aim";
    //            unsafe
    //            {
    //                int lol = 0;
    //                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
    //                NativeFunction.CallByName<bool>("TASK_GOTO_ENTITY_AIMING", 0, Game.LocalPlayer.Character, 4f, 20f);
    //                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
    //                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
    //                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.Pedestrian, lol);
    //                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
    //            }
    //        }
    //        else if (CurrentSubTaskLoop != "GoTo" && Cop.DistanceToPlayer >= 15f)
    //        {
    //            Debug.Instance.WriteToLog("Tasking", string.Format("     FootChase GoTo: {0}", Cop.Pedestrian.Handle));
    //            CurrentSubTaskLoop = "GoTo";
    //            unsafe
    //            {
    //                int lol = 0;
    //                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
    //                NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Game.LocalPlayer.Character, -1, 7f, 500f, 1073741824, 1); //Original and works ok
    //                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
    //                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
    //                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Cop.Pedestrian, lol);
    //                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
    //            }
    //        }
    //    }
    //    private void GoToLastSeen()
    //    {
    //        if (!Cop.Pedestrian.Exists() || !Cop.IsDriver)
    //            return;

    //        if (CurrentTaskLoop != "GoToLastSeen")
    //        {
    //            GoToLastSeen_Start();
    //        }
    //        else
    //        {
    //            GoToLastSeen_Normal();
    //        }
    //    }
    //    private void GoToLastSeen_Start()
    //    {
    //        Cop.Pedestrian.BlockPermanentEvents = false;
    //        Vector3 WantedCenter = Mod.World.Instance.PlacePoliceLastSeenPlayer; //NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
    //        if (Cop.IsInVehicle && Cop.Pedestrian.CurrentVehicle != null)
    //        {
    //            NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, WantedCenter.X, WantedCenter.Y, WantedCenter.Z, 20f, 4 | 16 | 32 | 262144, 20f);
    //        }
    //        else
    //        {
    //            Cop.Pedestrian.Tasks.GoStraightToPosition(WantedCenter, 15f, 0f, 2f, 0);
    //        }
    //        CurrentTaskedPosition = WantedCenter;
    //        NearWantedCenterThisWanted = false;
    //        CurrentTaskLoop = "GoToLastSeen";
    //        Debug.Instance.WriteToLog("Tasking", string.Format("     Started GoToLastSeen: {0}", Cop.Pedestrian.Handle));
    //    }
    //    private void GoToLastSeen_Normal()
    //    {
    //        Vector3 WantedCenter = Mod.World.Instance.PlacePoliceLastSeenPlayer; //NativeFunction.CallByName<Vector3>("GET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer);
    //        if (!NearWantedCenterThisWanted)
    //        {
    //            if (CurrentTaskedPosition.DistanceTo2D(WantedCenter) >= 5f)
    //            {
    //                if (Cop.IsInVehicle && Cop.Pedestrian.IsInAnyVehicle(false))
    //                {
    //                    NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, WantedCenter.X, WantedCenter.Y, WantedCenter.Z, 20f, 4 | 16 | 32 | 262144, 20f);
    //                }
    //                else
    //                {
    //                    Cop.Pedestrian.Tasks.GoStraightToPosition(WantedCenter, 15f, 0f, 2f, 0);
    //                }
    //                CurrentTaskedPosition = WantedCenter;
    //                CurrentTaskLoop = "GoToLastSeen";
    //                Debug.Instance.WriteToLog("Tasking", string.Format("     Updated GoToLastSeen: {0}", Cop.Pedestrian.Handle));
    //            }
    //            if (Cop.DistanceToLastSeen <= 25f)
    //            {
    //                NearWantedCenterThisWanted = true;
    //                if (Cop.IsDriver && Cop.IsInVehicle)
    //                {
    //                    Cop.Pedestrian.Tasks.CruiseWithVehicle(30f, VehicleDrivingFlags.Emergency);
    //                }
    //                else if (!Cop.IsInVehicle)
    //                {
    //                    Cop.Pedestrian.Tasks.Wander();
    //                }
    //                Debug.Instance.WriteToLog("Tasking", string.Format("     Post GoToLastSeen: {0}", Cop.Pedestrian.Handle));
    //            }
    //        }
    //    }
    //    private void HeliGoToLastSeen()
    //    {
    //        if (!Cop.Pedestrian.Exists() || !Cop.IsDriver || !Cop.IsInHelicopter)
    //            return;

    //        if (CurrentTaskLoop != "HeliGoToLastSeen")
    //        {
    //            HeliGoToLastSeen_Start();
    //        }
    //        else
    //        {
    //            HeliGoToLastSeen_Normal();
    //        }
    //    }
    //    private void HeliGoToLastSeen_Start()
    //    {
    //        Cop ClosestCop = Mod.World.Instance.PoliceList.Where(x => x.IsDriver).OrderBy(x => x.DistanceToLastSeen).FirstOrDefault();
    //        if (ClosestCop == null)
    //            return;
    //        NativeFunction.CallByName<bool>("TASK_HELI_CHASE", Cop.Pedestrian, ClosestCop.Pedestrian, -50f, 50f, 60f);
    //        Debug.Instance.WriteToLog("Tasking", string.Format("     Heli Lost you following closest cop: {0}", Cop.Pedestrian.Handle));
    //        NearWantedCenterThisWanted = false;
    //        CurrentTaskLoop = "HeliGoToLastSeen";
    //    }
    //    private void HeliGoToLastSeen_Normal()
    //    {

    //    }
    //    private void Kill()
    //    {
    //        if (CurrentTaskLoop != "Kill")
    //        {
    //            Kill_Start();
    //        }
    //        else
    //        {

    //        }
    //    }
    //    private void Kill_Start()
    //    {
    //        ClearTasks();
    //        CurrentTaskLoop = "Kill";
    //        GameTimeLastTasked = Game.GameTime;
    //        Debug.Instance.WriteToLog("Tasking", string.Format("     Started Kill: {0} Old CurrentTaskLoop: {1}", Cop.Pedestrian.Handle, CurrentTaskLoop));
    //    }
    //}
    //public class Kill : Activity
    //{
    //    public Kill(Cop PedToAssign) : base(PedToAssign)
    //    {
    //        PedToAssign.Pedestrian.BlockPermanentEvents = false;
    //    }
    //    public void Update()
    //    {
    //        if (CurrentTaskLoop != "Kill")
    //        {
    //            Kill_Start();
    //        }
    //        else
    //        {

    //        }
    //    }
    //    private void Kill_Start()
    //    {
    //        ClearTasks();
    //        CurrentTaskLoop = "Kill";
    //        GameTimeLastTasked = Game.GameTime;
    //        Debug.Instance.WriteToLog("Tasking", string.Format("     Started Kill: {0} Old CurrentTaskLoop: {1}", Cop.Pedestrian.Handle, CurrentTaskLoop));
    //    }
    //}
}
