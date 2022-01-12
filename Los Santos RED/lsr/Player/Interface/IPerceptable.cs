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
        Ped Character { get; }
        VehicleExt CurrentSeenVehicle { get; }
        WeaponInformation CurrentSeenWeapon { get; }
        bool IsWanted { get; }
        List<Crime> CivilianReportableCrimesViolating { get; }
        Vector3 Position { get; }
        int CellX { get; }
        int CellY { get; }
        bool AnyGangMemberCanSeePlayer { get; set; }
        bool AnyGangMemberCanHearPlayer { get; set; }
        bool AnyGangMemberRecentlySeenPlayer { get; set; }
    }
}
