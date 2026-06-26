using LosSantosRED.lsr.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IGangTerritoryManageable
    {
        Gang CurrentGang { get; }
        bool RecentlyRespawned { get; }
        CellPhone CellPhone { get; }
        LocationData CurrentLocation { get; }
        bool IsAliveAndFree { get; }
    }
}
