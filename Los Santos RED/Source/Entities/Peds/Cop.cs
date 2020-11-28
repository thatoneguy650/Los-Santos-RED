
using ExtensionsMethods;
using Rage;
using Rage.Native;
using System.Linq;


public class Cop : PedExt
{
    public bool WasModSpawned { get; set; }
    public bool WasSpawnedAsDriver { get; set; }
    public bool ShouldAutoSetWeaponState { get; set; } = true;
    public Agency AssignedAgency { get; set; } = new Agency();
    public float DistanceToInvestigationPosition
    {
        get
        {
            return Pedestrian.DistanceTo2D(Investigation.InvestigationPosition);
        }
    }
    public uint HasBeenSpawnedFor
    {
        get
        {
            return Game.GameTime - GameTimeSpawned;
        }
    }
    public int CountNearbyCops
    {
        get
        {
            return PedList.Cops.Count(x => Pedestrian.Exists() && x.Pedestrian.Exists() && Pedestrian.Handle != x.Pedestrian.Handle && x.Pedestrian.DistanceTo2D(Pedestrian) >= 3f && x.Pedestrian.DistanceTo2D(Pedestrian) <= 50f);
        }
    }
    public bool ShouldBustPlayer
    {
        get
        {
            if (PlayerState.IsBusted)
            {
                return false;
            }
            else if (!PlayerState.IsBustable)
            {
                return false;
            }
            else if (IsInVehicle)
            {
                return false;
            }
            else if (DistanceToPlayer < 0.1f) //weird cases where they are my same position
            {
                return false;
            }
            else if (PlayerState.HandsAreUp && DistanceToPlayer <= 5f)
            {
                return true;
            }
            if (PlayerState.IsInVehicle)
            {
                if (PlayerState.IsStationary && DistanceToPlayer <= 1f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if ((Game.LocalPlayer.Character.IsStunned || Game.LocalPlayer.Character.IsRagdoll) && DistanceToPlayer <= 3f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
    public Cop(Ped pedestrian, int health, Agency agency) : base(pedestrian)
    {
        IsCop = true;
        Health = health;
        AssignedAgency = agency;

        Pedestrian.VisionRange = 55f;
        Pedestrian.HearingRange = 25;
        if (General.MySettings.Police.OverridePoliceAccuracy)
            Pedestrian.Accuracy = General.MySettings.Police.PoliceGeneralAccuracy;
    }
}

