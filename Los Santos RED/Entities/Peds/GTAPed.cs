﻿
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GTAPed
{
    public GTAPed(Ped _Pedestrian, bool _canSeePlayer, int _Health)
    {
        Pedestrian = _Pedestrian;
        CanSeePlayer = _canSeePlayer;
        Health = _Health;
    }
    public int Health { get; set; }
    public Ped Pedestrian { get; set; }
    public bool CanSeePlayer { get; set; }
    public bool CanRecognizePlayer { get; set; }
    public bool CanHearPlayer { get; set; }
    public uint GameTimeLastSeenPlayer { get; set; }
    public uint GameTimeContinuoslySeenPlayerSince { get; set; }
    public Vector3 PositionLastSeenPlayer { get; set; }
    public bool HurtByPlayer { get; set; } = false;
    public bool KilledByPlayer { get; set; } = false;
    public uint GameTimeLastDistanceCheck { get; set; }
    public uint GameTimeLastLOSCheck { get; set; }
    public bool isInVehicle { get; set; } = false;
    public bool isInHelicopter { get; set; } = false;
    public bool isOnBike { get; set; } = false;
    public float DistanceToPlayer { get; set; }
    public float DistanceToLastSeen { get; set; }
    public bool WasMarkedNonPersistent { get; set; } = false;
    public bool HasBeenMugged { get; set; } = false;
    public bool CanFlee { get; set; } = true;
    public bool WillCallPolice { get; set; } = true;
    public List<Crime> CrimesWitnessed { get; set; } = new List<Crime>();
    public bool isTasked { get; set; } = false;
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
    public bool SeenPlayerSince(int _Duration)
    {
        if (CanSeePlayer)
            return true;
        else if (Game.GameTime - GameTimeLastSeenPlayer <= _Duration)
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
    public void UpdateDistance()
    {
        if (NeedsDistanceCheck)
        {
            DistanceToPlayer = Pedestrian.DistanceTo(Game.LocalPlayer.Character.Position);
            DistanceToLastSeen = Pedestrian.DistanceTo(Police.PlacePlayerLastSeen);

            if (DistanceToPlayer <= 45f)
                CanHearPlayer = true;
            else
                CanHearPlayer = false;

            GameTimeLastDistanceCheck = Game.GameTime;
        }
    }
    public void AddCrime(Crime CrimeToAdd)
    {
        if(!CrimesWitnessed.Any(x => x.DebugName == CrimeToAdd.DebugName))
        {
            CrimesWitnessed.Add(CrimeToAdd);
        }
    }
}

