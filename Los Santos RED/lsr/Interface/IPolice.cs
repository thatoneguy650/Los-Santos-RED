using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IPolice
    {
        bool AnySeenPlayerCurrentWanted { get; }
        Vector3 PlaceLastSeenPlayer { get; }
        bool AnyCanSeePlayer { get; }
        bool AnyRecentlySeenPlayer { get; }
        bool RecentlyNoticedVehicleChange { get; }
        bool AnyCanRecognizePlayer { get; }
        float ActiveDistance { get; }
    }
}
