using LSR.Vehicles;
using Rage;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Interface
{
    public interface ITaskableTarget_Old
    {
        bool AnyHumansNear { get; }
        bool AnyPoliceCanHearPlayer { get; set; }
        bool AnyPoliceCanRecognizePlayer { get; set; }
        bool AnyPoliceCanSeePlayer { get; set; }
        bool AnyPoliceRecentlySeenPlayer { get; set; }
        bool AnyPoliceSeenPlayerCurrentWanted { get; set; }
        bool CanDropWeapon { get; }
        bool CanSurrender { get; }
        Ped Character { get; }
        List<Crime> CivilianReportableCrimesViolating { get; }
        Street CurrentCrossStreet { get; }
        PoliceResponse PoliceResponse { get; }
        Vector3 CurrentPosition { get; }
        VehicleExt CurrentSeenVehicle { get; }
        WeaponInformation CurrentSeenWeapon { get; }
        Street CurrentStreet { get; }
        VehicleExt CurrentVehicle { get; }
        WeaponInformation CurrentWeapon { get; }
        WeaponCategory CurrentWeaponCategory { get; }
        Zone CurrentZone { get; }
        string DebugString_Drunk { get; }
        bool DiedInVehicle { get; }
        bool HandsAreUp { get; set; }
        float IntoxicatedIntensity { get; set; }
        Investigation Investigation { get; }
        bool IsAliveAndFree { get; }
        bool IsAttemptingToSurrender { get; }
        bool IsBreakingIntoCar { get; }
        bool IsBustable { get; }
        bool IsBusted { get; }
        bool IsChangingLicensePlates { get; set; }
        bool IsCommitingSuicide { get; }
        bool IsConsideredArmed { get; }
        bool IsDead { get; }
        bool IsHoldingEnter { get; set; }
        bool IsInAirVehicle { get; }
        bool IsInAutomobile { get; }
        bool IsInSearchMode { get; set; }
        bool IsIntoxicated { get; set; }
        bool IsInVehicle { get; }
        bool IsLockPicking { get; set; }
        bool IsMale { get; }
        bool IsMoveControlPressed { get; set; }
        bool IsMoving { get; }
        bool IsMovingFast { get; }
        bool IsMugging { get; }
        bool IsNotWanted { get; }
        bool IsOffroad { get; }
        bool IsOnMotorcycle { get; }
        bool IsSpeeding { get; }
        bool IsStill { get; }
        bool IsViolatingAnyAudioBasedCivilianReportableCrime { get; }
        bool IsViolatingAnyCivilianReportableCrime { get; }
        bool IsViolatingAnyTrafficLaws { get; }
        bool IsWanted { get; }
        bool KilledAnyCops { get; }
        string DebugString_LawsViolating { get; }
        int MaxWantedLastLife { get; }
        string ModelName { get; }
        int Money { get; }
        bool NearCivilianMurderVictim { get; }
        Vector3 PlacePoliceLastSeenPlayer { get; set; }
        bool RecentlyAppliedWantedStats { get; }
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
        void Arrest();
        void CommitSuicide();
        void DisplayPlayerNotification();
        void DrinkBeer();
        void DropWeapon();
        void GiveMoney(int v);
        void Injured(PedExt myPed);
        void Murdered(PedExt myPed);
        void LowerHands();
        void RaiseHands();
        void Reset(bool resetWanted, bool resetTimesDied, bool clearWeapons);
        void SetPlayerToLastWeapon();
        void SetUnarmed();
        void StartSmoking();
        void StartSmokingPot();
        void UnSetArrestedAnimation(Ped character);
    }
}