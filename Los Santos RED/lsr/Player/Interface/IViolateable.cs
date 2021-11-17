using LosSantosRED.lsr.Locations;
using LSR.Vehicles;
using Rage;

namespace LosSantosRED.lsr.Interface
{
    public interface IViolateable
    {
        bool AnyHumansNear { get; }/// <summary>
        /// test456
        /// </summary>
        bool AnyPoliceCanHearPlayer { get; }
        bool AnyPoliceCanSeePlayer { get; }
        bool AnyPoliceRecentlySeenPlayer { get; }
        Ped Character { get; }
        PoliceResponse PoliceResponse { get; }
        Vector3 Position { get; }

        LocationData CurrentLocation { get; set; }
        VehicleExt CurrentSeenVehicle { get; }
        WeaponInformation CurrentSeenWeapon { get; }
        //Street CurrentStreet { get; }
        VehicleExt CurrentVehicle { get; }
        WeaponInformation CurrentWeapon { get; }
        WeaponCategory CurrentWeaponCategory { get; }
      //  Zone CurrentZone { get; }
        bool HandsAreUp { get; }
        float IntoxicatedIntensity { get; }
        Investigation Investigation { get; }
        bool IsAliveAndFree { get; }
        bool IsBreakingIntoCar { get; }
        bool IsChangingLicensePlates { get; }
        bool IsCommitingSuicide { get; }
        bool IsVisiblyArmed { get; }
        bool IsInAirVehicle { get; }
        bool IsInAutomobile { get; }
        bool IsIntoxicated { get; }
        bool IsInVehicle { get; }
        bool IsHoldingUp { get; }
        bool IsOnMotorcycle { get; }
        bool IsWanted { get; }
        bool RecentlyStartedPlaying { get; }
        bool RecentlyShot { get; }
        float VehicleSpeedMPH { get; }
        bool RecentlyBribedPolice { get; }
        bool RecentlyPaidFine { get; }
        bool ShouldCheckViolations { get; }
        bool TreatAsCop { get; }
        bool RecentlyFedUpCop { get; }
        int GroupID { get; }

        void AddCrime(Crime violating, bool v1, Vector3 currentPosition, VehicleExt currentSeenVehicle, WeaponInformation currentSeenWeapon, bool v2, bool announceCrime, bool IsForPlayer);
    }
}