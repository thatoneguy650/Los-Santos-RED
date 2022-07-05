using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IHumanStateable
    {
        bool IsDead { get; }
        bool IsBusted { get; }
        bool IsInVehicle { get; }
        bool IsPerformingActivity { get; set; }
        bool CanPerformActivities { get; }
        DanceData LastDance { get; set; }
        bool IsAlive { get; }
        bool IsMovingDynamically { get; }
        bool IsMovingFast { get; }
        float FootSpeed { get; }
    }
}
