using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IRespawning//needs better name and to be moved
    {
        bool RecentlyRespawned { get; }
        Respawning Respawning { get; }
        bool IsUsingController { get; }
    }
}
