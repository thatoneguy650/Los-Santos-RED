using LosSantosRED.lsr.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IRestrictedAreaManagable
    {
        Violations Violations { get; }
        LocationData CurrentLocation { get; }

        void OnSeenInRestrictedAreaOnCamera(bool isSevere);
    }
}
