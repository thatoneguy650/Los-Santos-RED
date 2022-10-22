using LosSantosRED.lsr.Locations;
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

        bool IsCop { get; }
        bool IsMoving { get; }
        bool AnyGangMemberRecentlySeenPlayer { get; }

        void AddCrime(Crime CrimeInstance, bool ByPolice, Vector3 Location, VehicleExt VehicleObserved, WeaponInformation WeaponObserved, bool HaveDescription, bool AnnounceCrime, bool IsForPlayer);
        RelationshipManager RelationshipManager { get; }
        bool RecentlyShot { get; }
        float ClosestPoliceDistanceToPlayer { get; }
       // bool IsHoldingHostage { get; }
      //  bool IsCommitingSuicide { get; }
        Cop ClosestCopToPlayer { get; }

        void AddMedicalEvent(Vector3 positionLastSeenDistressedPed);

        LocationData CurrentLocation { get; }
        bool IsVisiblyArmed { get; }
        WeaponEquipment WeaponEquipment { get; }
        bool IsDangerouslyArmed { get; }
        Vector3 PlacePoliceShouldSearchForPlayer { get; }
        SearchMode SearchMode { get; }
        bool AnyPoliceRecentlySeenPlayer { get; }
        bool AnyPoliceKnowInteriorLocation { get; }

        ActivityManager ActivityManager { get; }
        //bool IsHostile(Gang gang);
    }
}
