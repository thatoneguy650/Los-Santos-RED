
using ExtensionsMethods;
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
    public bool CanHearPlayer { get; set; } = false;
    public Vector3 PositionLastSeenPlayer { get; private set; }
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
    public bool CanBeTasked { get; set; } = true;
    public bool WillCallPolice { get; set; } = true;
    public List<Crime> CrimesWitnessed { get; set; } = new List<Crime>();
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
    public PedExt(Ped _Pedestrian, int _Health)
    {
        Pedestrian = _Pedestrian;
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
    private void UpdateDistance()
    {
        if (NeedsDistanceCheck)
        {
            DistanceToPlayer = Pedestrian.DistanceTo2D(Game.LocalPlayer.Character.Position);
            DistanceToLastSeen = Pedestrian.DistanceTo2D(Police.PlaceLastSeenPlayer);

            if (DistanceToPlayer <= 0.1f)
                DistanceToPlayer = 999f;


            if (DistanceToPlayer <= ClosestDistanceToPlayer)
                ClosestDistanceToPlayer = DistanceToPlayer;

            if (DistanceToPlayer <= 45f)
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
                if (PlayerState.IsWanted)
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
        if(IsCop)
        {
            Police.WasPlayerLastSeenInVehicle = PlayerState.IsInVehicle;
            Police.PlayerLastSeenHeading = Game.LocalPlayer.Character.Heading;
            Police.PlayerLastSeenForwardVector = Game.LocalPlayer.Character.ForwardVector;
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

