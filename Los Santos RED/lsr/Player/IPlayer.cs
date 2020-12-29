using LSR.Vehicles;
using Rage;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Interface
{
    public interface IPlayer
    {
        bool AnyHumansNear { get; }
        bool AnyPoliceCanHearPlayer { get; set; }
        bool AnyPoliceCanRecognizePlayer { get; set; }
        bool AnyPoliceCanSeePlayer { get; set; }
        bool AnyPoliceRecentlySeenPlayer { get; set; }
        bool AnyPoliceSeenPlayerCurrentWanted { get; set; }
        ArrestWarrant ArrestWarrant { get; }
        bool CanDropWeapon { get; }
        bool CanSurrender { get; }
        Ped Character { get; }
        List<Crime> CivilianReportableCrimesViolating { get; }
        Street CurrentCrossStreet { get; }
        PoliceResponse CurrentPoliceResponse { get; }
        Vector3 CurrentPosition { get; }
        VehicleExt CurrentSeenVehicle { get; }
        WeaponInformation CurrentSeenWeapon { get; }
        Street CurrentStreet { get; }
        VehicleExt CurrentVehicle { get; }
        WeaponInformation CurrentWeapon { get; }
        WeaponCategory CurrentWeaponCategory { get; }
        Zone CurrentZone { get; }
        bool DiedInVehicle { get; }
        bool HandsAreUp { get; set; }
        Investigations Investigations { get; }
        bool IsAliveAndFree { get; }
        bool IsAttemptingToSurrender { get; }
        bool IsBreakingIntoCar { get; }
        bool IsBustable { get; }
        bool IsBusted { get; }
        bool IsChangingLicensePlates { get; set; }
        bool IsCommitingSuicide { get; }

        bool IsConsideredArmed { get; }
        bool IsDead { get; }
        bool IsDrunk { get; }
        bool IsHoldingEnter { get; set; }
        bool IsInAirVehicle { get; }
        bool IsInAutomobile { get; }
        bool IsInSearchMode { get; set; }
        bool IsInVehicle { get; }
        bool IsLockPicking { get; set; }
        bool IsMoveControlPressed { get; set; }
        bool IsMugging { get; }
        bool IsNotWanted { get; }
        bool IsOffroad { get; }
        bool IsOnMotorcycle { get; }
        bool IsSpeeding { get; }
        bool IsStationary { get; }
        bool IsViolatingAnyAudioBasedCivilianReportableCrime { get; }
        bool IsViolatingAnyCivilianReportableCrime { get; }
        bool IsViolatingAnyTrafficLaws { get; }
        bool IsWanted { get; }
        bool KilledAnyCops { get; }
        string LawsViolatingDisplay { get; }
        int MaxWantedLastLife { get; }
        int Money { get; }
        bool NearCivilianMurderVictim { get; }
        Vector3 PlacePoliceLastSeenPlayer { get; set; }
        bool RecentlyBusted { get; }
        bool RecentlyDied { get; }
        bool RecentlyHurtCivilian { get; }
        bool RecentlyHurtCop { get; }
        bool RecentlyKilledCivilian { get; }
        bool RecentlyKilledCop { get; }
        bool RecentlyStartedPlaying { get; }
        List<VehicleExt> ReportedStolenVehicles { get; }
        List<LicensePlate> SpareLicensePlates { get; }
        int TimesDied { get; set; }
        List<VehicleExt> TrackedVehicles { get; }
        int WantedLevel { get; }
        void CommitSuicide();
        void DisplayPlayerNotification();
        void DropWeapon();
        void GiveMoney(int v);
        void Injured(PedExt myPed);
        void Killed(PedExt myPed);
        void LowerHands();
        void RaiseHands();
        bool RecentlyShot(int v);
        void Reset(bool resetWanted, bool resetTimesDied, bool clearWeapons);
        void SetCarJacking(bool v);
        void SetPlayerToLastWeapon();
        void SetShot();
        void SetSmashedWindow();
        void SetUnarmed();
        void StartManualArrest();
        void UnSetArrestedAnimation(Ped character);
    }
}