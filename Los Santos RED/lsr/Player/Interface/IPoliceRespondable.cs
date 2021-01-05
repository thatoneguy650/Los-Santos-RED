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
        Street CurrentCrossStreet { get; }
        PoliceResponse PoliceResponse { get; set; }//should not be
        VehicleExt CurrentSeenVehicle { get; }
        WeaponInformation CurrentSeenWeapon { get; }
        Street CurrentStreet { get; }
        VehicleExt CurrentVehicle { get; }
        WeaponInformation CurrentWeapon { get; }
        WeaponCategory CurrentWeaponCategory { get; }
        Zone CurrentZone { get; }
        Investigation Investigation { get; }
        bool IsAliveAndFree { get; }
        bool IsBustable { get; }
        bool IsBusted { get; }
        bool IsConsideredArmed { get; }
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
        event EventHandler CitizenReportedCrime;

        event EventHandler BecameWanted;
        event EventHandler LostWanted;
        event EventHandler WantedLevelIncreased;
        void Injured(PedExt myPed);
        void Murdered(PedExt myPed);
        void Arrest();
        void StoreCriminalHistory();
        void AddCrime(Crime crime, bool v1, Vector3 positionLastSeenCrime, VehicleExt vehicleLastSeenPlayerIn, WeaponInformation weaponLastSeenPlayerWith, bool v2);
    }
}