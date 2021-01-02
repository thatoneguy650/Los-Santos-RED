using LSR.Vehicles;
using Rage;
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
        PoliceResponse CurrentPoliceResponse { get; }
        VehicleExt CurrentSeenVehicle { get; }
        WeaponInformation CurrentSeenWeapon { get; }
        Street CurrentStreet { get; }
        VehicleExt CurrentVehicle { get; }
        WeaponInformation CurrentWeapon { get; }
        WeaponCategory CurrentWeaponCategory { get; }
        Zone CurrentZone { get; }
        Investigations Investigations { get; }
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
        bool RecentlyKilledCop { get; }
        bool RecentlyStartedPlaying { get; }
        List<VehicleExt> ReportedStolenVehicles { get; }
        int WantedLevel { get; }
        uint TimeToRecognize { get; }
        bool PoliceRecentlyNoticedVehicleChange { get; set; }
        uint TimeInSearchMode { get; }
        bool StarsRecentlyGreyedOut { get; }
        bool StarsRecentlyActive { get; }
        void Injured(PedExt myPed);
        void Killed(PedExt myPed);
        void Arrest();
    }
}