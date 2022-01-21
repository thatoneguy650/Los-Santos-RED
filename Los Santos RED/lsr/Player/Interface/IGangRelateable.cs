using LosSantosRED.lsr.Locations;
using LSR.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IGangRelateable
    {
        LocationData CurrentLocation { get; }
        bool IsWanted { get; }
        int WantedLevel { get; }
        List<GangReputation> GangReputations { get; }
        int Money { get; }
        string PlayerName { get; }
        List<Crime> WantedCrimes { get; }
        VehicleExt OwnedVehicle { get; }

        void SetDenStatus(Gang gang, bool v);
    }
}
