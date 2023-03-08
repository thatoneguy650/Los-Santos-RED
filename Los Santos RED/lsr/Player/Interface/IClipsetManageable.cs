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
    public interface IClipsetManageable
    {

        Ped Character { get; }
        WeaponEquipment WeaponEquipment { get; }
    }
}
