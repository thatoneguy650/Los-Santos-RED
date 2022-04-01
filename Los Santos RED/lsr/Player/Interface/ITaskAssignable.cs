using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ITaskAssignable
    {
        GangRelationships GangRelationships { get; }
        CellPhone CellPhone { get; }
        VehicleExt CurrentVehicle { get; }
        Ped Character { get; }
        Vehicle LastFriendlyVehicle { get; set; }
        GunDealerRelationship GunDealerRelationship { get; }
        OfficerFriendlyRelationship OfficerFriendlyRelationship { get; }
        bool IsNotWanted { get; }
        ButtonPrompts ButtonPrompts { get; }
        Investigation Investigation { get; }
        bool IsAliveAndFree { get; }
        int WantedLevel { get; }
        PoliceResponse PoliceResponse { get; }
        WeaponInformation CurrentWeapon { get; }
        bool RecentlyShot { get; }
        bool IsWanted { get; }
        bool AnyPoliceRecentlySeenPlayer { get; }
        bool AnyPoliceCanSeePlayer { get; }

        void GiveMoney(int paymentAmountOnCompletion);
        void AddCrime(Crime crimeObserved, bool isObservedByPolice, Vector3 Location, VehicleExt VehicleObserved, WeaponInformation WeaponObserved, bool HaveDescription, bool AnnounceCrime, bool isForPlayer);
    }
}
