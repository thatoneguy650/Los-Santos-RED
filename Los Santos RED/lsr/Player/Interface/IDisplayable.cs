using LSR.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IDisplayable
    {
        Street CurrentCrossStreet { get; }
        VehicleExt CurrentSeenVehicle { get; }
        WeaponInformation CurrentSeenWeapon { get; }
        Street CurrentStreet { get; }
        VehicleExt CurrentVehicle { get; }
        Zone CurrentZone { get; }
        string DebugString_ModelInfo { get; }
        string DebugString_ObservedCrimes { get; }
        string DebugString_ReportedCrimes { get; }
        string DebugString_SearchMode { get; }
        string DebugString_State { get; }
        string DebugString_Drunk { get; }
        bool IsBusted { get; }
        bool IsDead { get; }
        bool IsSpeeding { get; }
        bool IsViolatingAnyTrafficLaws { get; }
        string LawsViolatingDisplay { get; }
    }
}
