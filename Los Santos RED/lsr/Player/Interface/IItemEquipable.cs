using LosSantosRED.lsr.Locations;
using LosSantosRED.lsr.Player;
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
    public interface IItemEquipable
    {
        bool IsInVehicle { get; }
        bool IsVisiblyArmed { get; set; }
        bool IsDangerouslyArmed { get; }
        bool WasDangerouslyArmedWhenBusted { get; }
        bool VeryRecentlyShot { get; }
        ActivityManager ActivityManager { get; }
        string DebugString { get; set; }

        void SetShot();
    }
}
