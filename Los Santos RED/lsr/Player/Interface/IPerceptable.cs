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
        WeaponEquipment WeaponEquipment { get; }
        RelationshipManager RelationshipManager { get; }
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
        Stance Stance { get; }
        IntimidationManager IntimidationManager { get; }

        void AddMedicalEvent(Vector3 positionLastSeenDistressedPed);
    }
}
