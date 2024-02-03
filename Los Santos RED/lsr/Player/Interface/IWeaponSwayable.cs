using LosSantosRED.lsr.Locations;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IWeaponSwayable
    {
        WeaponEquipment WeaponEquipment { get; }
        bool IsInVehicle { get; }
        Ped Character { get; }
        bool IsRagdoll { get; }
        bool IsStunned { get; }
        bool IsInFirstPerson { get; }
        string DebugString { get; set; }
        bool IsUsingController { get; }
        bool IsOnMuscleRelaxants { get; }
    }
}
