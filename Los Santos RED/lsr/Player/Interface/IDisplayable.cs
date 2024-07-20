using LosSantosRED.lsr.Locations;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IDisplayable
    {
        ActivityManager ActivityManager { get; }
        BankAccounts BankAccounts { get; }
        CriminalHistory CriminalHistory { get; }
        ButtonPrompts ButtonPrompts { get; }
        CellPhone CellPhone { get; }
        LocationData CurrentLocation { get; set; }
        WeaponEquipment WeaponEquipment { get; }
        HumanState HumanState { get; set; }
        Investigation Investigation { get; }
        PoliceResponse PoliceResponse { get; }
        Sprinting Sprinting { get; }
        Violations Violations { get; }
        Intoxication Intoxication { get; }
        //bool CanConverseWithLookedAtPed { get; }
        int CellX { get; }
        int CellY { get; }
        Ped Character { get; }
        bool CharacterModelIsPrimaryCharacter { get; }
        VehicleExt CurrentSeenVehicle { get; }
        VehicleExt CurrentVehicle { get; }
        bool IsAliveAndFree { get; }
        bool IsBusted { get; }
      //  bool IsConversing { get; }
        bool IsDead { get; }
        bool IsInSearchMode { get; }
        bool IsInVehicle { get; }
        bool IsNotWanted { get; }
     //   bool IsPerformingActivity { get; }
        bool IsSleeping { get; }
        bool IsTransacting { get; }
        bool IsWanted { get; }
        float VehicleSpeedKMH { get; }
        float VehicleSpeedMPH { get; }
        SearchMode SearchMode { get; }
        bool IsStill { get; }
    //   bool IsSitting { get; }
        bool AnyPoliceKnowInteriorLocation { get; }
        HealthManager HealthManager { get; }
        string DebugString { get; }
        bool IsCustomizingPed { get; }
        bool IsUsingController { get; }
        bool IsShowingActionWheel { get; set; }
        bool IsInAutomobile { get; }
        bool IsOnMotorcycle { get; }
        RelationshipManager RelationshipManager { get; }
        int WantedLevel { get; }
        bool IsAlive { get; }
        bool IsNotShowingFrontEndMenus { get; set; }
        InteriorManager InteriorManager { get; }
        IntimidationManager IntimidationManager { get; }
        GroupManager GroupManager { get; }
    }
}
