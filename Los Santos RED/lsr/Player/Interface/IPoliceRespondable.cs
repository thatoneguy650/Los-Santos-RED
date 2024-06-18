using LosSantosRED.lsr.Locations;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Interface
{
    public interface IPoliceRespondable
    {
        Respawning Respawning { get; }
        LocationData CurrentLocation { get; set; }
        WeaponEquipment WeaponEquipment { get; }
        RelationshipManager RelationshipManager { get; }
        Investigation Investigation { get; }
        Licenses Licenses { get; }
        PoliceResponse PoliceResponse { get; }
        Violations Violations { get; }
        ActivityManager ActivityManager { get; }
        bool AnyPoliceCanHearPlayer { get; set; }
        bool AnyPoliceCanRecognizePlayer { get; set; }
        bool AnyPoliceCanSeePlayer { get; set; }
        bool AnyPoliceInHeliCanSeePlayer { get; set; }
        bool AnyPoliceRecentlySeenPlayer { get; set; }
        Ped Character { get; }
        Cop ClosestCopToPlayer { get; set; }
        float ClosestPoliceDistanceToPlayer { get; set; }
        VehicleExt CurrentSeenVehicle { get; }
        VehicleExt CurrentVehicle { get; }
        bool IsAliveAndFree { get; }
        bool IsAttemptingToSurrender { get; }
        bool IsBustable { get; }
        bool IsBusted { get; }
        bool IsCop { get; }
        bool IsDead { get; }
        bool IsIncapacitated { get; }
        bool IsInSearchMode { get; set; }
        bool IsInVehicle { get; }
        bool IsNotWanted { get; }
        bool IsVisiblyArmed { get; }
        bool IsWanted { get; }
        //int MaxWantedLastLife { get; }
        Vector3 PlacePoliceLastSeenPlayer { get; set; }


        Vector3 PlacePoliceShouldSearchForPlayer { get; set; }


        Vector3 Position { get; }
        bool RecentlyBusted { get; }
        bool RecentlyShot { get; }
        bool RecentlyStartedPlaying { get; }
        List<VehicleExt> ReportedStolenVehicles { get; }
        float SearchModePercentage { get; }
        uint TimeInSearchMode { get; }
        uint TimeToRecognize { get; }
        int WantedLevel { get; }
        bool IsDangerouslyArmed { get; }
        bool IsStill { get; }
        SearchMode SearchMode { get; }
        bool WasDangerouslyArmedWhenBusted { get; }
        ButtonPrompts ButtonPrompts { get; }
        bool AnyPoliceKnowInteriorLocation { get; set; }
        bool IsBeingBooked { get; }
        Scanner Scanner { get; }
        bool IsAlive { get; }
        bool IsDetainable { get; }
        bool IsOnFoot { get; }
        bool PoliceLastSeenOnFoot { get; set; }
        bool IsNearbyPlacePoliceShouldSearchForPlayer { get; set; }
        Vector3 StreetPlacePoliceShouldSearchForPlayer { get; set; }
        Vector3 StreetPlacePoliceLastSeenPlayer { get; set; }
        Vector3 PlacePolicePhysicallyLastSeenPlayer { get; set; }
        bool AutoDispatch { get; set; }
        bool AnyPoliceSawPlayerViolating { get; set; }
        bool CanBustPeds { get; }
        OfficerMIAWatcher OfficerMIAWatcher { get; }
        bool IsSecurityGuard { get; }
        bool IsSetAutoCallBackup { get; }
        bool IsRidingOnTrain { get; }
        bool IsArrested { get; }
        Cop ClosestCopDriverToPlayer { get; set; }
        Inventory Inventory { get; }

        void AddCrime(Crime crime, bool ByPolice, Vector3 positionLastSeenCrime, VehicleExt vehicleLastSeenPlayerIn, WeaponInformation weaponLastSeenPlayerWith, bool HaveDescription, bool announceCrime, bool IsForPlayer);
        void AddMedicalEvent(Vector3 positionLastSeenDistressedPed);
        void AddOfficerMIACall(Vector3 positionLastReported);
        void Arrest();
        int FineAmount();
        void GetKillingPed();
        void OnAppliedWantedStats(int wantedLevel);
        void OnInvestigationExpire();
        void OnLethalForceAuthorized();
        void OnPoliceNoticeVehicleChange();
        void OnRequestedBackUp();
        void OnSuspectEluded();
        void OnWantedActiveMode();
        void OnWantedSearchMode();
        void OnWeaponsFree();
        void SetWantedLevel(int resultingWantedLevel, string name, bool v);
        //void YellInPain();
    }
}