using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IPerceptable
    {
        Equipment Equipment { get; }
        Violations Violations { get; }
        Ped Character { get; }
        VehicleExt CurrentSeenVehicle { get; }
        bool IsWanted { get; }
        Vector3 Position { get; }
        int CellX { get; }
        int CellY { get; }
        bool AnyGangMemberCanSeePlayer { get; set; }
        bool AnyGangMemberCanHearPlayer { get; set; }
        bool AnyGangMemberRecentlySeenPlayer { get; set; }
        bool IsInVehicle { get; }
        void AddDistressedPed(Vector3 positionLastSeenDistressedPed);
    }
}
