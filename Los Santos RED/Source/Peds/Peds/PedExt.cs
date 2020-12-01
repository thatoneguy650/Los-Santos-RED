
using ExtensionsMethods;
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
    private bool playerIsInFront;

    private uint GameTimeBehindPlayer;
    private uint GameTimeLastSeenPlayer;
    private uint GameTimeContinuoslySeenPlayerSince;
    public int Health { get; set; }
    public Ped Pedestrian { get; set; }
    public bool CanSeePlayer { get; set; } = false;
    public bool CanRecognizePlayer { get; set; } = false;
    public bool WithinWeaponsAudioRange { get; set; } = false;
    public Vector3 PositionLastSeenPlayer { get; private set; }
    public bool HurtByPlayer { get; set; } = false;
    public bool KilledByPlayer { get; set; } = false;
    public uint GameTimeLastDistanceCheck { get; set; }
    public bool IsCop { get; set; } = false;
    public uint GameTimeLastLOSCheck { get; set; }
    public uint GameTimeSpawned { get; set; }
    public bool IsDriver { get; set; } = false;
    public bool IsInVehicle { get; set; } = false;
    public bool IsInHelicopter { get; set; } = false;
    public bool IsOnBike { get; set; } = false;
    public float DistanceToPlayer { get; set; } = 999f;
    public float DistanceToLastSeen { get; set; } = 99f;
    public bool WasMarkedNonPersistent { get; set; } = false;
    public bool HasBeenMugged { get; set; } = false;
    public bool WillFight { get; set; } = false;
    public int LastSeatIndex { get; set; }

    //Temp Crapola
    public bool IsWaitingAtTrafficLight { get; set; } = false;
    public bool IsFirstWaitingAtTrafficLight { get; set; } = false;
    public Vector3 PlaceCheckingInfront { get; set; } = Vector3.Zero;



    public float ClosestDistanceToPlayer { get; set; } = 2000f;
    public Vector3 PositionLastSeenCrime { get; set; } = Vector3.Zero;
    public bool CanBeTasked { get; set; } = true;
    public bool WillCallPolice { get; set; } = true;
    public List<Crime> CrimesWitnessed { get; set; } = new List<Crime>();
    public VehicleExt VehicleLastSeenPlayerIn { get; set; }
    public WeaponInformation WeaponLastSeenPlayerWith { get; set; }
    public bool NeedsUpdate
    {
        get
        {
            if ((NeedsDistanceCheck || NeedsLOSCheck) && Pedestrian.IsAlive)
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
    public Vector3 PositionToReportToPolice
    {
        get
        {
            if (EverSeenPlayer)
                return PositionLastSeenPlayer;
            else if (PositionLastSeenCrime != Vector3.Zero)
                return PositionLastSeenCrime;
            else
                return Pedestrian.Position;
        }
    }
    public bool SeenPlayerSince(int _Duration)
    {
        if (CanSeePlayer)
            return true;
        else if (Game.GameTime - GameTimeLastSeenPlayer <= _Duration)
            return true;
        else
            return false;
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
    public void UpdateContinuouslySeen()
    {
        if (GameTimeContinuoslySeenPlayerSince == 0)
        {
            GameTimeContinuoslySeenPlayerSince = Game.GameTime;
        }
    }
    public void Update()
    {
        if (NeedsUpdate && Pedestrian.IsAlive)
        {
            IsInVehicle = Pedestrian.IsInAnyVehicle(false);
            if (IsInVehicle)
            {
                IsDriver = Pedestrian.IsDriver();
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
            if (!Game.LocalPlayer.Character.IsInFront(Pedestrian))
            {
                if (GameTimeBehindPlayer == 0)
                    GameTimeBehindPlayer = Game.GameTime;
            }
            else
                GameTimeBehindPlayer = 0;
            UpdateDistance();
            UpdateLineOfSight();
        }
    }
    private void SetDrivingFlags()
    {
        NativeFunction.CallByName<bool>("SET_DRIVER_ABILITY", Pedestrian, 100f);
        if (IsCop && PlayerStateManager.IsWanted)
        {
            NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE", Pedestrian, 8f);
            if (!IsInHelicopter)
            {
                if (WantedLevelManager.PoliceChasingRecklessly)
                {
                    //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.Pedestrian, 4, true);
                    //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.Pedestrian, 8, true);
                    //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.Pedestrian, 16, true);
                    //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.Pedestrian, 512, true);
                    //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Cop.Pedestrian, 262144, true);
                }
                else if (!WantedLevelManager.PoliceChasingRecklessly && DistanceToPlayer <= 15f)
                {
                    NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Pedestrian, 32, true);//only originally this one for reckless pursuit
                }

                if (PlayerLocationManager.PlayerIsOffroad && DistanceToPlayer <= 200f)
                {
                    NativeFunction.CallByName<bool>("SET_DRIVE_TASK_DRIVING_STYLE", Pedestrian, 4194304);
                    //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Pedestrian, 4194304, true);
                }
                else
                {
                    NativeFunction.CallByName<bool>("SET_DRIVE_TASK_DRIVING_STYLE", Pedestrian, 1074528293);
                    //NativeFunction.CallByName<bool>("SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG", Pedestrian, 4194304, false);
                }
            }
        }
        else
        {
            NativeFunction.CallByName<bool>("SET_DRIVE_TASK_DRIVING_STYLE", Pedestrian, 183);
            
        }
    }
    private void UpdateDistance()
    {
        if (NeedsDistanceCheck)
        {
            DistanceToPlayer = Pedestrian.DistanceTo2D(Game.LocalPlayer.Character.Position);
            DistanceToLastSeen = Pedestrian.DistanceTo2D(PoliceManager.PlaceLastSeenPlayer);


            if (DistanceToPlayer <= 0.1f)
                DistanceToPlayer = 999f;


            if (DistanceToPlayer <= ClosestDistanceToPlayer)
                ClosestDistanceToPlayer = DistanceToPlayer;

            if (DistanceToPlayer <= 45f)
                WithinWeaponsAudioRange = true;
            else
                WithinWeaponsAudioRange = false;

            GameTimeLastDistanceCheck = Game.GameTime;
        }
    }
    private void UpdateLineOfSight()
    {
        if (NeedsLOSCheck)
        {
            bool PlayerInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
            Entity ToCheck = PlayerInVehicle ? (Entity)Game.LocalPlayer.Character.CurrentVehicle : (Entity)Game.LocalPlayer.Character;
            if (IsCop && !Pedestrian.IsInHelicopter)
            {  
                if (DistanceToPlayer <= 90f && Pedestrian.PlayerIsInFront() && !Pedestrian.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Pedestrian, ToCheck))//55f
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
                if (PlayerStateManager.IsWanted)
                    DistanceToSee = 350f;
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
                if (DistanceToPlayer <= 90f && Pedestrian.PlayerIsInFront() && !Pedestrian.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Pedestrian, ToCheck))//55f
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
                    CanRecognizePlayer = true;
                else
                    CanRecognizePlayer = false;
            }
            else
            {
                if (TimeContinuoslySeenPlayer >= 1250)
                    CanRecognizePlayer = true;
                else if (DistanceToPlayer <= 2f && DistanceToPlayer > 0f)
                    CanRecognizePlayer = true;
                else
                    CanRecognizePlayer = false;
            }

            GameTimeLastLOSCheck = Game.GameTime;
        }
    }
    public void AddCrime(Crime CrimeToAdd, Vector3 PositionToReport)
    {
        if (!CrimesWitnessed.Any(x => x.Name == CrimeToAdd.Name))
        {
            CrimesWitnessed.Add(CrimeToAdd);
            PositionLastSeenCrime = PositionToReport;
        }
    }
    private void SetPlayerSeen()
    {
        UpdateContinuouslySeen();
        CanSeePlayer = true;
        GameTimeLastSeenPlayer = Game.GameTime;
        PositionLastSeenPlayer = Game.LocalPlayer.Character.Position;
        VehicleLastSeenPlayerIn = PlayerStateManager.CurrentVehicle;
        WeaponLastSeenPlayerWith = PlayerStateManager.CurrentWeapon;
        if(IsCop)
        {
            PoliceManager.WasPlayerLastSeenInVehicle = PlayerStateManager.IsInVehicle;
            PoliceManager.PlayerLastSeenHeading = Game.LocalPlayer.Character.Heading;
            PoliceManager.PlayerLastSeenForwardVector = Game.LocalPlayer.Character.ForwardVector;
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
                Entity killer = NativeFunction.Natives.GetPedSourceOfDeath<Entity>(Pedestrian);
                if (killer.Handle == Game.LocalPlayer.Character.Handle || (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Handle == killer.Handle))
                    KilledByPlayer = true;
            }
        }
        catch
        {
            if (HurtByPlayer)
                KilledByPlayer = true;
        }     
    }
}

