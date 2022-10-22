using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IWeaponDroppable
    {
        ActivityManager ActivityManager { get; }
        bool IsInVehicle { get; }
        bool IsVisiblyArmed { get; }
        bool IsBusted { get; }
        //bool CanPerformActivities { get; }
        bool IsAliveAndFree { get; }
        bool IsIncapacitated { get; }
    }
}
