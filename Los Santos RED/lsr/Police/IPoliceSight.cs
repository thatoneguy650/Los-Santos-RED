using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IPoliceSight
    {
        bool AnySeenPlayerCurrentWanted { get; }///scanner
        Vector3 PlaceLastSeenPlayer { get; }//tasking scanner
        bool AnyCanSeePlayer { get; }//scanner
        bool AnyRecentlySeenPlayer { get; }//dispatcher scanner, search mode, tasking
        bool RecentlyNoticedVehicleChange { get; }//scanner
    }
}
