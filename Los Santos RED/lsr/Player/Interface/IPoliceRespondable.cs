using LosSantosRED.lsr.Locations;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Interface
{
    public interface IPoliceRespondable
    {
        bool AnyPoliceCanHearPlayer { get; set; }
        bool AnyPoliceCanRecognizePlayer { get; set; }
        bool AnyPoliceCanSeePlayer { get; set; }
        bool AnyPoliceRecentlySeenPlayer { get; set; }
        bool AnyPoliceSeenPlayerCurrentWanted { get; set; }
        LocationData CurrentLocation { get; set; }
        PoliceResponse PoliceResponse { get;  }//should not be
        VehicleExt CurrentSeenVehicle { get; }
        WeaponInformation CurrentSeenWeapon { get; }
        VehicleExt CurrentVehicle { get; }
        WeaponInformation CurrentWeapon { get; }
        WeaponCategory CurrentWeaponCategory { get; }
        Investigation Investigation { get; }
        bool IsAliveAndFree { get; }
        bool IsBustable { get; }
        bool IsBusted { get; }
        bool IsVisiblyArmed { get; }
        bool IsDead { get; }
        bool IsInSearchMode { get; set; }
        bool IsInVehicle { get; }
        bool IsNotWanted { get; }
        bool IsWanted { get; }
        int MaxWantedLastLife { get; }
        Vector3 PlacePoliceLastSeenPlayer { get; set; }
        bool RecentlyAppliedWantedStats { get; }
        bool RecentlyBusted { get; }
        bool RecentlyDied { get; }
        bool RecentlyStartedPlaying { get; }
        List<VehicleExt> ReportedStolenVehicles { get; }
        int WantedLevel { get; }
        uint TimeToRecognize { get; }
        bool PoliceRecentlyNoticedVehicleChange { get; set; }
        uint TimeInSearchMode { get; }
        bool StarsRecentlyGreyedOut { get; }
        bool StarsRecentlyActive { get; }
        Violations Violations { get; }//not good comrade
        Vector3 Position { get; }
        void CheckInjured(PedExt myPed);
        void CheckMurdered(PedExt myPed);
        void Arrest();
        void StoreCriminalHistory();
        void AddCrime(Crime crime, bool ByPolice, Vector3 positionLastSeenCrime, VehicleExt vehicleLastSeenPlayerIn, WeaponInformation weaponLastSeenPlayerWith, bool HaveDescription);
    }
}