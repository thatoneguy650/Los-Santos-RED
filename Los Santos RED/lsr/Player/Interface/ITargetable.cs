using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ITargetable
    {
        bool IsInVehicle { get; }
        VehicleExt CurrentVehicle { get; }
        bool IsAttemptingToSurrender { get; }
        bool IsBusted { get; }
        int WantedLevel { get; }
        Ped Character { get; }
        bool IsStill { get; }
        bool IsMovingFast { get; }
        bool IsWanted { get; }
        PoliceResponse PoliceResponse { get; }
        bool IsInSearchMode { get; }
        float ActiveDistance { get; }
        Investigation Investigation { get; }
        Vector3 PlacePoliceLastSeenPlayer { get; }
        bool IsNotWanted { get; }
        ComplexTask CurrentTask { get; }
        Cop AliasedCop { get; }
        bool IsCop { get; }
        bool IsMoving { get; }
        bool AnyGangMemberRecentlySeenPlayer { get; }

        void AddCrime(Crime CrimeInstance, bool ByPolice, Vector3 Location, VehicleExt VehicleObserved, WeaponInformation WeaponObserved, bool HaveDescription, bool AnnounceCrime, bool IsForPlayer);
        GangRelationships GangRelationships { get; }
        bool RecentlyShot { get; }
        float ClosestPoliceDistanceToPlayer { get; }
        //bool IsHostile(Gang gang);
    }
}
