
using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GTAPed
{
    public int Health { get; set; }
    public Ped Pedestrian { get; set; }
    public bool CanSeePlayer { get; set; } = false;
    public bool CanRecognizePlayer { get; set; } = false;
    public bool CanHearPlayer { get; set; } = false;
    public uint GameTimeLastSeenPlayer { get; set; }
    public uint GameTimeContinuoslySeenPlayerSince { get; set; }
    public Vector3 PositionLastSeenPlayer { get; set; }
    public bool HurtByPlayer { get; set; } = false;
    public bool KilledByPlayer { get; set; } = false;
    public uint GameTimeLastDistanceCheck { get; set; }
    public bool IsCop { get; set; } = false;
    public uint GameTimeLastLOSCheck { get; set; }
    public uint GameTimeSpawned { get; set; }
    public bool IsInVehicle { get; set; } = false;
    public bool IsInHelicopter { get; set; } = false;
    public bool IsOnBike { get; set; } = false;
    public float DistanceToPlayer { get; set; }
    public float DistanceToLastSeen { get; set; }
    public bool WasMarkedNonPersistent { get; set; } = false;
    public bool HasBeenMugged { get; set; } = false;
    public bool WillFight { get; set; } = false;
    public float ClosestDistanceToPlayer { get; set; } = 2000f;
    public Vector3 PositionLastSeenCrime { get; set; } = Vector3.Zero;
    public bool CanFlee { get; set; } = true;
    public bool WillCallPolice { get; set; } = true;
    public List<Crime> CrimesWitnessed { get; set; } = new List<Crime>();
    public bool IsTasked { get; set; } = false;
    public bool TaskIsQueued { get; set; } = false;
    public Tasking.AssignableTasks TaskType { get; set; } = Tasking.AssignableTasks.NoTask;
    public GameFiber TaskFiber { get; set; }
    public bool NeedsUpdate
    {
        get
        {
            if (NeedsDistanceCheck || NeedsLOSCheck)
                return true;
            else
                return false;
        }
            
    }
    public bool NeedsDistanceCheck
    {
        get
        {
            int DistanceUpdate = 25;
            if (!IsCop)
                DistanceUpdate = 150;
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
    public uint HasSeenPlayerFor
    {
        get
        {
            if (GameTimeContinuoslySeenPlayerSince == 0)
                return 0;
            else
                return (Game.GameTime - GameTimeContinuoslySeenPlayerSince);
        }
    }
    public bool RecentlySeenPlayer()
    {
        if (CanSeePlayer)
            return true;
        else if (Game.GameTime - GameTimeLastSeenPlayer <= 10000)//Seen in last 10 seconds?
            return true;
        else
            return false;
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
    public bool SeenPlayerSince(int _Duration)
    {
        if (CanSeePlayer)
            return true;
        else if (Game.GameTime - GameTimeLastSeenPlayer <= _Duration)
            return true;
        else
            return false;
    }
    public GTAPed(Ped _Pedestrian, bool _canSeePlayer, int _Health)
    {
        Pedestrian = _Pedestrian;
        CanSeePlayer = _canSeePlayer;
        Health = _Health;
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
        if (NeedsUpdate)
        {
            int NewHealth = Pedestrian.Health;
            if (NewHealth != Health)
            {
                if (!HurtByPlayer && CheckPlayerHurtPed)
                {
                    HurtByPlayer = true;
                }
                Health = NewHealth;
            }
            if (Pedestrian.IsDead)
            {
                if (CheckPlayerKilledPed)
                    KilledByPlayer = true;
                return;
            }
            IsInVehicle = Pedestrian.IsInAnyVehicle(false);
            if (IsInVehicle)
            {
                IsInHelicopter = Pedestrian.IsInHelicopter;
                if (!IsInHelicopter)
                    IsOnBike = Pedestrian.IsOnBike;
            }
            else
            {
                IsInHelicopter = false;
                IsOnBike = false;
            }
            UpdateDistance();
            UpdateLineOfSight();
        }
    }
    private void UpdateDistance()
    {
        if (NeedsDistanceCheck)
        {
            DistanceToPlayer = Pedestrian.DistanceTo2D(Game.LocalPlayer.Character.Position);
            DistanceToLastSeen = Pedestrian.DistanceTo2D(Police.PlacePlayerLastSeen);


            if (DistanceToPlayer <= ClosestDistanceToPlayer)
                ClosestDistanceToPlayer = DistanceToPlayer;

            if (DistanceToPlayer <= 35f)
                CanHearPlayer = true;
            else
                CanHearPlayer = false;

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
                if (DistanceToPlayer <= 55f && Pedestrian.PlayerIsInFront() && !Pedestrian.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Pedestrian, ToCheck))
                {
                    SetPlayerSeen();
                    Police.PlayerSeen();
                }
                else
                {
                    GameTimeContinuoslySeenPlayerSince = 0;
                    CanSeePlayer = false;
                }
            }
            else if (Pedestrian.IsInHelicopter)
            {
                if(DistanceToPlayer <= 350f && !Pedestrian.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY", Pedestrian, ToCheck, 17))
                {
                    SetPlayerSeen();
                    Police.PlayerSeen();
                }
                else
                {
                    GameTimeContinuoslySeenPlayerSince = 0;
                    CanSeePlayer = false;
                }
            }
            else
            {
                if (DistanceToPlayer <= 40f && Pedestrian.PlayerIsInFront() && !Pedestrian.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Pedestrian, ToCheck))
                {
                    SetPlayerSeen();
                    Police.PlayerSeen();
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
                if (HasSeenPlayerFor >= 1250)
                    CanRecognizePlayer = true;
                else if (DistanceToPlayer <= 2f && DistanceToPlayer > 0f)
                    CanRecognizePlayer = true;
                else
                    CanRecognizePlayer = false;
            }

            GameTimeLastLOSCheck = Game.GameTime;
        }
    }
    public void AddCrime(Crime CrimeToAdd,Vector3 PositionToReport)
    {
        if(!CrimesWitnessed.Any(x => x.Name == CrimeToAdd.Name))
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
    }
    public bool CheckPlayerHurtPed
    {
        get
        {
            if (NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY", Pedestrian, Game.LocalPlayer.Character, true))
            {
                return true;

            }
            else if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY", Pedestrian, Game.LocalPlayer.Character.CurrentVehicle, true))
            {
                return true;
            }
            return false;
        }
    }
    public bool CheckPlayerKilledPed
    {
        get
        {
            try
            {
                if (Pedestrian.IsDead)
                {
                    Entity killer = NativeFunction.Natives.GetPedSourceOfDeath<Entity>(Pedestrian);
                    if (killer.Handle == Game.LocalPlayer.Character.Handle || (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Handle == killer.Handle))
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch
            {
                if (HurtByPlayer)
                    return true;
                else
                    return false;
            }
        }
    }
}

