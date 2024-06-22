using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ILocationReachable
    {
        bool HasReachedLocatePosition { get; }

        void OnFinalSearchLocationReached();
        void OnLocationReached();
    }
}
