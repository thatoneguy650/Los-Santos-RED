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
        ButtonPrompts ButtonPrompts { get; }
        CellPhone CellPhone { get; }
        LocationData CurrentLocation { get; set; }
        Equipment Equipment { get; }
        HumanState HumanState { get; set; }
        Investigation Investigation { get; }
        PoliceResponse PoliceResponse { get; }
        Sprinting Sprinting { get; }
        Violations Violations { get; }
        bool CanConverseWithLookedAtPed { get; }
        int CellX { get; }
        int CellY { get; }
        Ped Character { get; }
        bool CharacterModelIsPrimaryCharacter { get; }
        int CriminalHistoryMaxWantedLevel { get; }
        VehicleExt CurrentSeenVehicle { get; }
        VehicleExt CurrentVehicle { get; }
        string DebugLine1 { get; }
        string DebugLine2 { get; }
        string DebugLine3 { get; }
        string DebugLine4 { get; set; }
        string DebugLine5 { get; }
        string DebugLine6 { get; }
        string DebugLine7 { get; }
        string DebugLine8 { get; }
        string DebugLine9 { get; }
        bool HasCriminalHistory { get; }
        bool HasDeadlyCriminalHistory { get; }
        float IntoxicatedIntensityPercent { get; }
        bool IsAliveAndFree { get; }
        bool IsBusted { get; }
        bool IsConversing { get; }
        bool IsDead { get; }
        bool IsInSearchMode { get; }
        bool IsInVehicle { get; }
        bool IsNotWanted { get; }
        bool IsPerformingActivity { get; }
        bool IsSleeping { get; }
        bool IsSpeeding { get; }
        bool IsTransacting { get; }
        bool IsWanted { get; }
        int LastChangeMoneyAmount { get; }
        int Money { get; }
        bool RecentlyChangedMoney { get; }
        float VehicleSpeedKMH { get; }
        float VehicleSpeedMPH { get; }
    }
}
