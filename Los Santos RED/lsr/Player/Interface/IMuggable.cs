using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IMuggable
    {
        bool IsVisiblyArmed { get; }
        bool IsInVehicle { get; }
        WeaponInformation CurrentWeapon { get; }
    }
}
