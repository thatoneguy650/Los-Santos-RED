
using ExtensionsMethods;
using LosSantosRED.lsr;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PedExt
{
    private uint GameTimeBehindPlayer;
    private uint GameTimeLastSeenPlayer;
    private uint GameTimeContinuoslySeenPlayerSince;
    private uint GameTimeLastDistanceCheck;
    private uint GameTimeLastLOSCheck;
    private uint GameTimeLastSeenCrime;
    private uint GameTimeLastReportedCrime;

    public int Health { get; set; }
    public Ped Pedestrian { get; set; }
    public uint GameTimeSpawned { get; set; }
    public bool IsCop { get; set; } = false;
    public bool IsDriver { get; set; } = false;
    public bool IsInVehicle { get; set; } = false;
    public bool IsInHelicopter { get; set; } = false;
    public bool IsOnBike { get; set; } = false;
    public bool CanBeTasked { get; set; } = true;
    public bool WillCallPolice { get; set; } = true;
    public bool WasMarkedNonPersistent { get; set; } = false;
    public bool HasBeenMugged { get; set; } = false;
    public bool WillFight { get; set; } = false;
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
    public bool NeedsDistanceCheck
    {
        get
        {
            int DistanceUpdate = 150;//25
            if (!IsCop)
                DistanceUpdate = 500;//150
            if (GameTimeLastDistanceCheck == 0)
                return true;
            else if (Game.GameTime > GameTimeLastDistanceCheck + DistanceUpdate)
                return true;
            else
                return false;
        }
    }
    public bool NeedsLOSCheck
    {
        get
        {
            int DistanceUpdate = 500;
            if (!IsCop)
                DistanceUpdate = 750;
            if (GameTimeLastLOSCheck == 0)
                return true;
            else if (Game.GameTime > GameTimeLastLOSCheck + DistanceUpdate)
                return true;
            else
                return false;
        }
    }
    public int LastSeatIndex { get; set; }
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
            else if (Mod.Player.Respawning.RecentlyBribedPolice)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    public List<Crime> CrimesWitnessed { get; set; } = new List<Crime>();
    public Vector3 PositionLastSeenCrime { get; set; } = Vector3.Zero;

    public bool HasSeenPlayerCommitCrime { get; set; } = false;
    public float ClosestDistanceToPlayer { get; set; } = 2000f;
    public bool CanSeePlayer { get; set; } = false;
    public bool CanRecognizePlayer { get; set; } = false;
    public bool WithinWeaponsAudioRange { get; set; } = false;
    public bool HurtByPlayer { get; set; } = false;
    public bool KilledByPlayer { get; set; } = false;
    public float DistanceToPlayer { get; set; } = 999f;
    public float DistanceToLastSeen { get; set; } = 99f;
    public uint TimeContinuoslySeenPlayer
    {
        get
        {
            if (GameTimeContinuoslySeenPlayerSince == 0)
                return 0;
            else
                return (Game.GameTime - GameTimeContinuoslySeenPlayerSince);
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
    public VehicleExt VehicleLastSeenPlayerIn { get; set; }
    public WeaponInformation WeaponLastSeenPlayerWith { get; set; }
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


    public PedExt(Ped _Pedestrian)
    {
        Pedestrian = _Pedestrian;
        Health = Pedestrian.Health;
    }
    public PedExt(Ped _Pedestrian, bool _WillFight, bool _WillCallPolice)
    {
        Pedestrian = _Pedestrian;
        Health = Pedestrian.Health;
        WillFight = _WillFight;
        WillCallPolice = _WillCallPolice;
    }

    public void Update()
    {
        if (NeedsUpdate && Pedestrian.IsAlive)
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
                //NativeFunction.CallByName<bool>("SET_DRIVE_TASK_DRIVING_STYLE", Pedestrian, 1|2|128|256);//use blinkers?
            }
            else
            {
                IsInHelicopter = false;
                IsOnBike = false;
                IsDriver = false;
            }
            if (!IsInFront(Game.LocalPlayer.Character, Pedestrian))
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
            if (NeedsDistanceCheck)
            {
                UpdateDistance();
            }
            if (NeedsLOSCheck)
            {
                UpdateLineOfSight();
            }


            if (WillCallPolice && ShouldReportCrime)
            {
                ReportCrime();
                Mod.Debug.WriteToLog("WillCallPolice && ShouldReportCrime", string.Format(" {0} to WillCall {1} ShouldReportCrime {2}", Pedestrian.Handle, WillCallPolice, ShouldReportCrime));
            }

            if(!HasSeenPlayerCommitCrime)
            {
                Pedestrian.IsPersistent = false;
            }
        }
    }
    public void AddCrime(Crime CrimeToAdd, Vector3 PositionToReport)
    {
        if (!CrimesWitnessed.Any(x => x.Name == CrimeToAdd.Name))
        {
            CrimesWitnessed.Add(CrimeToAdd);
            PositionLastSeenCrime = PositionToReport;
            GameTimeLastSeenCrime = Game.GameTime;
            Mod.Debug.WriteToLog("AddCrime", string.Format(" Handle {0} GameTimeLastReactedToCrime {1}, CrimeToAdd.Name {2}", Pedestrian.Handle, GameTimeLastSeenCrime, CrimeToAdd.Name));
        }
    }
    private void SetDrivingFlags()
    {
        NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Pedestrian, 100f);
        if (IsCop && Mod.Player.IsWanted)
        {
            
            NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Pedestrian, 8f);
            if (!IsInHelicopter)
            {
                if (Mod.Player.CurrentPoliceResponse.PoliceChasingRecklessly)
                {
                    NativeFunction.CallByName<bool>("SET_DRIVER_AGGRESSIVENESS", Pedestrian, 1f);
                    NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Pedestrian, 4, true);
                    NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Pedestrian, 8, true);
                    NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Pedestrian, 16, true);
                    NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Pedestrian, 512, true);
                    NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Pedestrian, 262144, true);
                }
                else// if (!Mod.Player.CurrentPoliceResponse.PoliceChasingRecklessly && DistanceToPlayer <= 15f)
                {
                    NativeFunction.CallByName<bool>("SET_DRIVER_AGGRESSIVENESS", Pedestrian, 0.5f);
                    NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Pedestrian, 32, true);//only originally this one for reckless pursuit
                }

                if (Mod.Player.CurrentLocation.IsOffroad && DistanceToPlayer <= 200f)
                {
                    NativeFunction.CallByName<bool>("SET_DRIVE_TASK_DRIVING_STYLE", Pedestrian, 4194304);
                    NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Pedestrian, 4194304, true);
                }
                else
                {
                    NativeFunction.CallByName<bool>("SET_DRIVE_TASK_DRIVING_STYLE", Pedestrian, 1074528293);
                    NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Pedestrian, 4194304, false);
                }
            }
        }
        else
        {
            NativeFunction.CallByName<bool>("SET_DRIVE_TASK_DRIVING_STYLE", Pedestrian, 183);

        }
    }
    public bool SeenPlayerSince(int _Duration)
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
    public bool PlayerIsInFront()
    {
        float Result = GetDotVectorResult(Pedestrian, Game.LocalPlayer.Character);
        if (Result > 0)
            return true;
        else
            return false;

    }
    public void UpdateContinuouslySeen()
    {
        if (GameTimeContinuoslySeenPlayerSince == 0)
        {
            GameTimeContinuoslySeenPlayerSince = Game.GameTime;
        }
    }
    public void CheckPlayerHurtPed()
    {
        if (NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY", Pedestrian, Game.LocalPlayer.Character, true))
        {
            HurtByPlayer = true;

        }
        else if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY", Pedestrian, Game.LocalPlayer.Character.CurrentVehicle, true))
        {
            HurtByPlayer = true;
        }
    }
    public void CheckPlayerKilledPed()
    {
        try
        {
            if (Pedestrian.IsDead)
            {
                //Entity killer = NativeFunction.CallByName<Entity>("GET_PED_SOURCE_OF_DEATH", Pedestrian);
                //Entity killer = NativeFunction.Natives.GetPedSourceOfDeath<Entity>(Pedestrian);//was working before update from 2060.
                //Entity killer = NativeFunction.Natives.x93C8B64DEB84728C<Entity>(Pedestrian);//was working before update from 2060, with Hash instead of name
                //uint Handle = NativeFunction.CallByName<uint>("GET_PED_SOURCE_OF_DEATH", Pedestrian);
                //Mod.Debug.WriteToLog("CheckPlayerKilledPed", string.Format("Killed Handle: {0}, Player Handle: {1}, killer: {2}", Handle,Game.LocalPlayer.Character.Handle, killer.Handle));
                //if (Handle == Game.LocalPlayer.Character.Handle || (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Handle == Handle))
                //{
                //    KilledByPlayer = true;
                //}

                //temp as the above native functions are not working on this version of RPH, assume if you hurt them, you killed them
                if(!HurtByPlayer)
                {
                    CheckPlayerHurtPed();
                }
                if (HurtByPlayer)
                {
                    KilledByPlayer = true;
                }

            }
        }
        catch
        {
            if (HurtByPlayer)
            {
                KilledByPlayer = true;
            }
        }
    }
    private void ReportCrime()
    {
        if (Pedestrian.Exists() && Pedestrian.IsAlive && !Pedestrian.IsRagdoll)
        {
            Mod.Player.CurrentPoliceResponse.CurrentCrimes.AddCrime(CrimesWitnessed.OrderBy(x => x.Priority).FirstOrDefault(), false, PositionLastSeenCrime, VehicleLastSeenPlayerIn, WeaponLastSeenPlayerWith, EverSeenPlayer && ClosestDistanceToPlayer <= 20f);
            CrimesWitnessed.Clear();
            GameTimeLastReportedCrime = Game.GameTime;
            Pedestrian.IsPersistent = false;
        }
    }
    private void UpdateDistance()
    {
        DistanceToPlayer = Pedestrian.DistanceTo2D(Game.LocalPlayer.Character.Position);
        DistanceToLastSeen = Pedestrian.DistanceTo2D(Mod.World.Police.PlaceLastSeenPlayer);
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
        GameTimeLastDistanceCheck = Game.GameTime;  
    }
    private void UpdateLineOfSight()
    {
        bool PlayerInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
        Entity ToCheck = PlayerInVehicle ? (Entity)Game.LocalPlayer.Character.CurrentVehicle : (Entity)Game.LocalPlayer.Character;
        if (IsCop && !Pedestrian.IsInHelicopter)
        {
            if (DistanceToPlayer <= 90f && PlayerIsInFront() && !Pedestrian.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Pedestrian, ToCheck))//55f
            {
                SetPlayerSeen();
            }
            else
            {
                GameTimeContinuoslySeenPlayerSince = 0;
                CanSeePlayer = false;
            }
        }
        else if (Pedestrian.IsInHelicopter)
        {
            float DistanceToSee = 150f;
            if (Mod.Player.IsWanted)
            {
                DistanceToSee = 350f;
            }
            if (DistanceToPlayer <= DistanceToSee && !Pedestrian.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY", Pedestrian, ToCheck, 17))
            {
                SetPlayerSeen();
            }
            else
            {
                GameTimeContinuoslySeenPlayerSince = 0;
                CanSeePlayer = false;
            }
        }
        else
        {
            if (DistanceToPlayer <= 90f && PlayerIsInFront() && !Pedestrian.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Pedestrian, ToCheck))//55f
            {
                SetPlayerSeen();
            }
            else
            {
                GameTimeContinuoslySeenPlayerSince = 0;
                CanSeePlayer = false;
            }
        }

        if (PlayerInVehicle)
        {
            if (CanSeePlayer || DistanceToPlayer <= 7f)
            {
                CanRecognizePlayer = true;
            }
            else
            {
                CanRecognizePlayer = false;
            }
        }
        else
        {
            if (TimeContinuoslySeenPlayer >= 1250)
            {
                CanRecognizePlayer = true;
            }
            else if (DistanceToPlayer <= 2f && DistanceToPlayer > 0f)
            {
                CanRecognizePlayer = true;
            }
            else
            {
                CanRecognizePlayer = false;
            }
        }
        GameTimeLastLOSCheck = Game.GameTime;
    }
    private void SetPlayerSeen()
    {
        UpdateContinuouslySeen();
        CanSeePlayer = true;
        GameTimeLastSeenPlayer = Game.GameTime;
        PositionLastSeenPlayer = Game.LocalPlayer.Character.Position;
        VehicleLastSeenPlayerIn = Mod.Player.CurrentSeenVehicle;
        WeaponLastSeenPlayerWith = Mod.Player.CurrentSeenWeapon;
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
    private bool IsInFront(Entity Target, Entity Source)
    {
        float Result = GetDotVectorResult(Target, Source);
        if (Result > 0)
            return true;
        else
            return false;

    }

}

