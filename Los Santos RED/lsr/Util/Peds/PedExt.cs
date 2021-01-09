﻿
using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PedExt : IComplexTaskable
{
    private IPoliceRespondable PlayerToCheck;
    private HealthState CurrentHealthState;
    private uint GameTimeBehindPlayer;
    private uint GameTimeContinuoslySeenPlayerSince;
    private uint GameTimeLastDistanceCheck;
    private uint GameTimeLastLOSCheck;
    private uint GameTimeLastReportedCrime;
    private uint GameTimeLastSeenCrime;
    private uint GameTimeLastSeenPlayer;
    private Entity Killer;
    private Entity LastHurtBy;
    public ComplexTask CurrentTask { get; set; }
    public string DebugString => $"Handle: {Pedestrian.Handle} Distance {DistanceToPlayer} Task: {CurrentTask?.Name}, {CurrentTask?.SubTaskName}";
    public PedExt(Ped _Pedestrian)
    {
        Pedestrian = _Pedestrian;
        Health = Pedestrian.Health;
        CurrentHealthState = new HealthState(this);
    }
    public PedExt(Ped _Pedestrian, bool _WillFight, bool _WillCallPolice) : this(_Pedestrian)
    {
        WillFight = _WillFight;
        WillCallPolice = _WillCallPolice;
    }
    public bool CanBeTasked { get; set; } = true;
    public bool CanRecognizePlayer
    {
        get
        {
            //if (Mod.Player.Instance.IsInVehicle)
            //{
            //    if (CanSeePlayer || DistanceToPlayer <= 7f)
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //else
            //{
                if (TimeContinuoslySeenPlayer >= 500)//1250
                {
                    return true;
                }
                else if (CanSeePlayer && DistanceToPlayer <= 8f && DistanceToPlayer > 0.1f)//(DistanceToPlayer <= 2f && DistanceToPlayer > 0f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            //}
        }
    }
    public bool CanRemove
    {
        get
        {
            if (!Pedestrian.Exists())
            {
                return true;
            }
            else if (Pedestrian.IsDead && CurrentHealthState.HasLoggedDeath)
            {
                return true;
            }
            return false;
        }
    }
    public bool CanSeePlayer { get; private set; } = false;
    public float ClosestDistanceToPlayer { get; private set; } = 2000f;
    public List<Crime> CrimesWitnessed { get; private set; } = new List<Crime>();
    public float DistanceToPlayer { get; private set; } = 999f;
    public float DistanceToLastSeen { get; private set; } = 999f;
    public bool EverSeenPlayer
    {
        get
        {
            if (CanSeePlayer)
                return true;
            else if (GameTimeLastSeenPlayer > 0)
                return true;
            else
                return false;
        }
    }
    public bool HasBeenMugged { get; set; } = false;
    public bool HasReactedToCrimes { get; set; } = false;
    public bool HasSeenPlayerCommitCrime { get; private set; } = false;
    public int Health { get; set; }
    public bool IsCop { get; set; } = false;
    public bool IsDriver { get; private set; } = false;
    public bool IsInHelicopter { get; private set; } = false;
    public bool IsInVehicle { get; private set; } = false;
    public bool IsOnBike { get; private set; } = false;
    public int LastSeatIndex { get; private set; }
    public bool NeedsDistanceCheck
    {
        get
        {
            if (GameTimeLastDistanceCheck == 0)
            {
                return true;
            }
            else if (Game.GameTime > GameTimeLastDistanceCheck + DistanceUpdate + RandomItems.MyRand.Next(100))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool NeedsLOSCheck
    {
        get
        {
            if (GameTimeLastLOSCheck == 0)
            {
                return true;
            }
            else if (Game.GameTime > GameTimeLastLOSCheck + LosUpdate + RandomItems.MyRand.Next(100))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool NeedsUpdate
    {
        get
        {
            if ((NeedsDistanceCheck || NeedsLOSCheck) && Pedestrian.IsAlive)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
    public bool IsStill { get; private set; }
    public Ped Pedestrian { get; private set; }

    public Vector3 PositionLastSeenCrime { get; private set; } = Vector3.Zero;
    public Vector3 PositionLastSeenPlayer { get; private set; }
    public Vector3 PositionToReportToPolice
    {
        get
        {
            if (EverSeenPlayer)
            {
                return PositionLastSeenPlayer;
            }
            else if (PositionLastSeenCrime != Vector3.Zero)
            {
                return PositionLastSeenCrime;
            }
            else
            {
                return Pedestrian.Position;
            }
        }
    }
    public bool RecentlySeenPlayer
    {
        get
        {
            if (CanSeePlayer)
                return true;
            else if (Game.GameTime - GameTimeLastSeenPlayer <= 10000)//Seen in last 10 seconds?
                return true;
            else
                return false;
        }
    }
    public bool RecentlyUpdated
    {
        get
        {
            if (GameTimeLastDistanceCheck == 0)
                return false;
            else if (Game.GameTime - GameTimeLastDistanceCheck >= 2000)
                return false;
            else
                return true;
        }

    }
    public bool ShouldReportCrime
    {
        get
        {
            if (GameTimeLastSeenCrime == 0)
            {
                return false;
            }
            else if (Game.GameTime - GameTimeLastSeenCrime < 10000)
            {
                return false;
            }
            else if (!CrimesWitnessed.Any())
            {
                return false;
            }
            //else if (Mod.Player.Instance.RecentlyBribedPolice)
            //{
            //    return false;
            //}
            else
            {
                return true;
            }
        }
    }
    public uint TimeBehindPlayer
    {
        get
        {
            if (GameTimeBehindPlayer == 0)
                return 0;
            else
                return (Game.GameTime - GameTimeBehindPlayer);
        }
    }
    public uint TimeContinuoslySeenPlayer
    {
        get
        {
            if (GameTimeContinuoslySeenPlayerSince == 0)
            {
                return 0;
            }
            else
            {
                return (Game.GameTime - GameTimeContinuoslySeenPlayerSince);
            }
        }
    }
    public VehicleExt VehicleLastSeenPlayerIn { get; set; }
    public WeaponInformation WeaponLastSeenPlayerWith { get; set; }
    public bool WillCallPolice { get; private set; } = true;
    public bool WillFight { get; private set; } = false;
    public bool WithinWeaponsAudioRange { get; private set; } = false;
    private int DistanceUpdate
    {
        get
        {
            if(IsCop)
            {
                return 150;//150//keep this low for busting and tasking etc?
            }
            else
            {
                return 750;//500
            }
        }
    }
    private int LosUpdate
    {
        get
        {
            if (IsCop)
            {
                return 750;//500
            }
            else
            {
                return 750;//750
            }
        }
    }
    public bool HurtBy(Ped ToCheck)
    {
        if (LastHurtBy == ToCheck)
        {
            return true;
        }
        if (Pedestrian.Handle != ToCheck.Handle)
        {
            if (NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY", Pedestrian, ToCheck, true))
            {
                LastHurtBy = ToCheck;
                return true;

            }
            else if (ToCheck.IsInAnyVehicle(false) && NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY", Pedestrian, ToCheck.CurrentVehicle, true))
            {
                LastHurtBy = ToCheck;
                return true;
            }
        }
        return false;
    }
    public bool KilledBy(Ped ToCheck)
    {
        try
        {
            if (Pedestrian.Exists() && Pedestrian.IsDead && Pedestrian.Handle != ToCheck.Handle)
            {
                Killer = NativeFunction.Natives.GetPedSourceOfDeath<Entity>(Pedestrian);
                if (Killer.Handle == ToCheck.Handle || (ToCheck.IsInAnyVehicle(false) && ToCheck.Handle == Killer.Handle))
                {
                    return true;
                }
            }
            return false;
        }
        catch (Exception ex)
        {
            //Game.Console.Print($"KilledBy Error! Ped To Check: {Pedestrian.Handle}, assumeing you killed them if you hurt them");
            return HurtBy(ToCheck);
        }

    }
    public bool SeenPlayerFor(int _Duration)
    {
        if (CanSeePlayer)
        {
            return true;
        }
        else if (Game.GameTime - GameTimeLastSeenPlayer <= _Duration)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Update(IPoliceRespondable playerToCheck,Vector3 placeLastSeen)
    {
        PlayerToCheck = playerToCheck;
        if (Pedestrian.IsAlive)
        {
            if (NeedsUpdate)
            {
                UpdateVehicleState();
                UpdateDistance(placeLastSeen);
                UpdateLineOfSight();
                UpdateCrimes();
            }
        }
        else
        {
            CanSeePlayer = false;
            GameTimeContinuoslySeenPlayerSince = 0;
        }
        CurrentHealthState.Update(playerToCheck);
    }
    public void UpdateTask()
    {
        if (CurrentTask != null)
        {
            CurrentTask.Update();
        }
    }
    public void WitnessedCrime(Crime CrimeToAdd, Vector3 PositionToReport)
    {
        if (!CrimesWitnessed.Any(x => x.Name == CrimeToAdd.Name))
        {
            CrimesWitnessed.Add(CrimeToAdd);
            PositionLastSeenCrime = PositionToReport;
            GameTimeLastSeenCrime = Game.GameTime;
            HasSeenPlayerCommitCrime = true;
            //Game.Console.Print(string.Format("AddCrime Handle {0} GameTimeLastReactedToCrime {1}, CrimeToAdd.Name {2}", Pedestrian.Handle, GameTimeLastSeenCrime, CrimeToAdd.Name));
        }
    }
    private float GetDotVectorResult(Entity source, Entity target)
    {
        if (source.Exists() && target.Exists())
        {
            Vector3 dir = (target.Position - source.Position).ToNormalized();
            return Vector3.Dot(dir, source.ForwardVector);
        }
        else return -1.0f;
    }
    private bool IsBehind(Entity Target)
    {
        if (GetDotVectorResult(Target, Pedestrian) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    private bool IsInFrontOf(Ped ToCheck)
    {
        float Result = GetDotVectorResult(Pedestrian, ToCheck);
        if (Result > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void ReportCrime()
    {
        if (Pedestrian.Exists() && Pedestrian.IsAlive && !Pedestrian.IsRagdoll)
        {
            PlayerToCheck.AddCrime(CrimesWitnessed.OrderBy(x => x.Priority).FirstOrDefault(), false, PositionLastSeenCrime, VehicleLastSeenPlayerIn, WeaponLastSeenPlayerWith, EverSeenPlayer && ClosestDistanceToPlayer <= 20f);
            CrimesWitnessed.Clear();
            GameTimeLastReportedCrime = Game.GameTime;
            Pedestrian.IsPersistent = false;
            //Game.Console.Print(string.Format("Handle {0} WillCall {1} ShouldReportCrime {2}", Pedestrian.Handle, WillCallPolice, ShouldReportCrime));
        }
    }
    private void SetDrivingFlags()
    {  
        if (IsCop)
        {
            NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Pedestrian, 100f);
            NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Pedestrian, 8f);
            
            if (!IsInHelicopter && PlayerToCheck.IsWanted)// && PlayerToCheck.IsWanted)
            {
              //  NativeFunction.CallByName<bool>("SET_DRIVER_AGGRESSIVENESS", Pedestrian, 1.0f);
                //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Pedestrian, 32, true);
                //NativeFunction.CallByName<bool>("SET_DRIVE_TASK_DRIVING_STYLE", Pedestrian, 4);
                //NativeFunction.CallByName<bool>("SET_DRIVE_TASK_DRIVING_STYLE", Pedestrian, 8);
                //NativeFunction.CallByName<bool>("SET_DRIVE_TASK_DRIVING_STYLE", Pedestrian, 16);
                //NativeFunction.CallByName<bool>("SET_DRIVE_TASK_DRIVING_STYLE", Pedestrian, 32);
                //NativeFunction.CallByName<bool>("SET_DRIVE_TASK_DRIVING_STYLE", Pedestrian, 512);


                ////new 
                //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Pedestrian, 262144, true);



                //if (PlayerToCheck.CurrentPoliceResponse.PoliceChasingRecklessly)
                //{
                //   //NativeFunction.CallByName<bool>("SET_DRIVER_AGGRESSIVENESS", Pedestrian, 1.0f);
                //    //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Pedestrian, 4, true);
                //    //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Pedestrian, 8, true);
                //    //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Pedestrian, 16, true);
                //    //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Pedestrian, 512, true);
                //    //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Pedestrian, 262144, true);
                //}
                //else// if (!Mod.Player.Instance.CurrentPoliceResponse.PoliceChasingRecklessly && DistanceToPlayer <= 15f)
                //{
                //   //NativeFunction.CallByName<bool>("SET_DRIVER_AGGRESSIVENESS", Pedestrian, 0.75f);
                //    NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Pedestrian, 32, true);//only originally this one for reckless pursuit
                //}

                //if (PlayerToCheck.IsOffroad && DistanceToPlayer <= 200f)
                //{
                //    NativeFunction.CallByName<bool>("SET_DRIVE_TASK_DRIVING_STYLE", Pedestrian, 4194304);
                //    NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Pedestrian, 4194304, true);
                //}
                //else
                //{
                //    NativeFunction.CallByName<bool>("SET_DRIVE_TASK_DRIVING_STYLE", Pedestrian, 1074528293);
                //    NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Pedestrian, 4194304, false);
                //}
            }
        }
        else
        {
            NativeFunction.CallByName<bool>("SET_DRIVE_TASK_DRIVING_STYLE", Pedestrian, 183);
        }
    }
    private void SetPlayerSeen()
    {
        CanSeePlayer = true;
        GameTimeLastSeenPlayer = Game.GameTime;
        PositionLastSeenPlayer = Game.LocalPlayer.Character.Position;
        VehicleLastSeenPlayerIn = PlayerToCheck.CurrentSeenVehicle;
        WeaponLastSeenPlayerWith = PlayerToCheck.CurrentSeenWeapon;
        if (GameTimeContinuoslySeenPlayerSince == 0)
        {
            GameTimeContinuoslySeenPlayerSince = Game.GameTime;
        }
    }
    private void SetPlayerUnseen()
    {
        GameTimeContinuoslySeenPlayerSince = 0;
        CanSeePlayer = false;
    }
    private void UpdateCrimes()
    {
        if (WillCallPolice && ShouldReportCrime)
        {
            ReportCrime();
        }
        if (!HasSeenPlayerCommitCrime && Pedestrian.IsPersistent)
        {
            Pedestrian.IsPersistent = false;
        }
    }
    private void UpdateDistance(Vector3 placeLastSeen)
    {
        if (!NeedsDistanceCheck)
        {
            return;
        }
        DistanceToPlayer = Pedestrian.DistanceTo2D(Game.LocalPlayer.Character.Position);
        if(IsCop)
        {
            DistanceToLastSeen = Pedestrian.DistanceTo2D(placeLastSeen);
        }
        if (DistanceToPlayer <= 0.1f)
        {
            DistanceToPlayer = 999f;
        }
        if (DistanceToPlayer <= ClosestDistanceToPlayer)
        {
            ClosestDistanceToPlayer = DistanceToPlayer;
        }
        if (DistanceToPlayer <= 100f)//45f
        {
            WithinWeaponsAudioRange = true;
        }
        else
        {
            WithinWeaponsAudioRange = false;
        }
        if (!IsBehind(Game.LocalPlayer.Character))
        {
            if (GameTimeBehindPlayer == 0)
            {
                GameTimeBehindPlayer = Game.GameTime;
            }
        }
        else
        {
            GameTimeBehindPlayer = 0;
        }
        if(Pedestrian.IsStill)
        {
            IsStill = true;
        }
        else
        {
            IsStill = false;
        }
        GameTimeLastDistanceCheck = Game.GameTime;
    }
    private void UpdateLineOfSight()
    {
        if (NeedsLOSCheck)
        {
            bool PlayerInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            Entity ToCheck = PlayerInVehicle ? (Entity)Game.LocalPlayer.Character.CurrentVehicle : (Entity)Game.LocalPlayer.Character;
            if (IsCop && !Pedestrian.IsInHelicopter)
            {
                if (DistanceToPlayer <= 90f && IsInFrontOf(Game.LocalPlayer.Character) && !Pedestrian.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Pedestrian, ToCheck))//55f
                {
                    SetPlayerSeen();
                }
                else
                {
                    SetPlayerUnseen();
                }
            }
            else if (Pedestrian.IsInHelicopter)
            {
                float DistanceToSee = 150f;
                if (PlayerToCheck.IsWanted)
                {
                    DistanceToSee = 350f;
                }
                if (DistanceToPlayer <= DistanceToSee && !Pedestrian.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY", Pedestrian, ToCheck, 17))
                {
                    SetPlayerSeen();
                }
                else
                {
                    SetPlayerUnseen();
                }
            }
            else
            {
                if (DistanceToPlayer <= 90f && IsInFrontOf(Game.LocalPlayer.Character) && !Pedestrian.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Pedestrian, ToCheck))//55f
                {
                    SetPlayerSeen();
                }
                else
                {
                    SetPlayerUnseen();
                }
            }
            GameTimeLastLOSCheck = Game.GameTime;
        }
    }
    private void UpdateVehicleState()
    {
        IsInVehicle = Pedestrian.IsInAnyVehicle(false);
        if (IsInVehicle)
        {
            IsDriver = Pedestrian.SeatIndex == -1;
            LastSeatIndex = Pedestrian.SeatIndex;
            IsInHelicopter = Pedestrian.IsInHelicopter;
            if (!IsInHelicopter)
            {
                IsOnBike = Pedestrian.IsOnBike;
                SetDrivingFlags();
            }
        }
        else
        {
            IsInHelicopter = false;
            IsOnBike = false;
            IsDriver = false;
        }
    }
}
