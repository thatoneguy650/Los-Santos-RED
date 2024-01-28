using LosSantosRED.lsr.Locations;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ITaxiRideable
    {
        TaxiManager TaxiManager { get; }
        Vector3 Position { get; }
        Dispatcher Dispatcher { get; }
        VehicleExt CurrentVehicle { get; }
        ActivityManager ActivityManager { get; }
        BankAccounts BankAccounts { get; }
        int WantedLevel { get; }
        GPSManager GPSManager { get; }
        bool IsInVehicle { get; }
        bool IsWanted { get; }
        bool IsDead { get; }
        CellPhone CellPhone { get; }
        bool AnyPoliceRecentlySeenPlayer { get; }
        PoliceResponse PoliceResponse { get; }
        SearchMode SearchMode { get; }
        bool IsInSearchMode { get; }
    }
}
