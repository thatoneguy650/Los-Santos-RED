
using Rage;
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
    public bool NeedsDistanceCheck
    {
        get
        {
            if (GameTimeLastDistanceCheck == 0)
                return true;
            else if (Game.GameTime > GameTimeLastDistanceCheck + 25)
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
    public void UpdateDistance()
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
    public void UpdateSight()
    {
        if (NeedsSightCheck)
        {
            DistanceToPlayer = Pedestrian.DistanceTo2D(Game.LocalPlayer.Character.Position);
            DistanceToLastSeen = Pedestrian.DistanceTo2D(Police.PlacePlayerLastSeen);


            if (DistanceToPlayer <= ClosestDistanceToPlayer)
                ClosestDistanceToPlayer = DistanceToPlayer;

            if (DistanceToPlayer <= 35f)
                CanHearPlayer = true;
            else
                CanHearPlayer = false;

            GameTimeLastSightCheck = Game.GameTime;
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
}

