using Rage;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Interface
{
    public interface IDances
    {
        List<DanceData> DanceLookups { get; }

        DanceData GetRandomDance();
    }
}
