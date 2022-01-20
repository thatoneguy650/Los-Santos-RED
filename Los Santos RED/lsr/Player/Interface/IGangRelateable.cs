using LosSantosRED.lsr.Locations;
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

        void SetDenStatus(Gang gang, bool v);
    }
}
