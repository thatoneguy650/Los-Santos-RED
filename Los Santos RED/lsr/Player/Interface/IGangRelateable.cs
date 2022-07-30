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
    public interface IGangRelateable
    {
        VehicleOwnership VehicleOwnership { get; }
        PoliceResponse PoliceResponse { get; }
        CellPhone CellPhone { get; }
        GangRelationships GangRelationships { get; }
        PlayerTasks PlayerTasks { get; }
        Licenses Licenses { get; }
        BankAccounts BankAccounts { get; }
        LocationData CurrentLocation { get; }
        bool IsWanted { get; }
        int WantedLevel { get; }
        string PlayerName { get; }
        List<Crime> WantedCrimes { get; }
        Ped Character { get; }
        bool IsNotWanted { get; }
        Destinations Destinations { get; }
        void SetDenStatus(Gang gang, bool v);
        void StopDynamicActivity();
    }
}
