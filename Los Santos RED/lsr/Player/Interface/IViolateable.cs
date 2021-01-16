using LSR.Vehicles;
using Rage;

namespace LosSantosRED.lsr.Interface
{
    public interface IViolateable
    {
        bool AnyHumansNear { get; }
        bool AnyPoliceCanHearPlayer { get; }
        bool AnyPoliceCanSeePlayer { get; }
        bool AnyPoliceRecentlySeenPlayer { get; }
        bool AnyPoliceSeenPlayerCurrentWanted { get; }
        Ped Character { get; }
        PoliceResponse PoliceResponse { get; }
        Vector3 CurrentPosition { get; }
        VehicleExt CurrentSeenVehicle { get; }
        WeaponInformation CurrentSeenWeapon { get; }
        Street CurrentStreet { get; }
        VehicleExt CurrentVehicle { get; }
        WeaponInformation CurrentWeapon { get; }
        WeaponCategory CurrentWeaponCategory { get; }
        Zone CurrentZone { get; }
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
        bool RecentlyShot(int v);
        void AddCrime(Crime violating, bool v1, Vector3 currentPosition, VehicleExt currentSeenVehicle, WeaponInformation currentSeenWeapon, bool v2);
    }
}