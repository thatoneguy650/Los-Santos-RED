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
    public interface ITaskAssignable
    {
        BankAccounts BankAccounts { get; }
        ButtonPrompts ButtonPrompts { get; }
        CellPhone CellPhone { get; }
        WeaponEquipment WeaponEquipment { get; }
        RelationshipManager RelationshipManager { get; }
        Investigation Investigation { get; }
        PoliceResponse PoliceResponse { get; }
        bool AnyPoliceCanSeePlayer { get; }
        bool AnyPoliceRecentlySeenPlayer { get; }
        Ped Character { get; }
        VehicleExt CurrentVehicle { get; }
        bool IsAliveAndFree { get; }
        bool IsNotWanted { get; }
        bool IsWanted { get; }
        Vehicle LastFriendlyVehicle { get; set; }
        bool RecentlyShot { get; }
        int WantedLevel { get; }
        GroupManager GroupManager { get; }
        Violations Violations { get; }
        string ModelName { get; }
        Dispatcher Dispatcher { get; }
        LocationData CurrentLocation { get; }

        void AddCrime(Crime crimeObserved, bool isObservedByPolice, Vector3 Location, VehicleExt VehicleObserved, WeaponInformation WeaponObserved, bool HaveDescription, bool AnnounceCrime, bool isForPlayer);
    }
}
