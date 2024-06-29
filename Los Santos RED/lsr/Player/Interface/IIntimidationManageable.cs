using LosSantosRED.lsr.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IIntimidationManageable
    {
        LocationData CurrentLocation { get; }
        bool RecentlyShot { get; }
        bool IsVisiblyArmed { get; }
        WeaponEquipment WeaponEquipment { get; }
        Violations Violations { get; }
        int WantedLevel { get; }
        PlayerVoice PlayerVoice { get; }
        bool IsInVehicle { get; }
        bool SemiRecentlyShot { get; }
    }
}
