using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IHumanStateable
    {
        HealthManager HealthManager { get; }
        ActivityManager ActivityManager { get; }
        bool IsDead { get; }
        bool IsBusted { get; }
        bool IsInVehicle { get; }
        //bool IsPerformingActivity { get; set; }
       // bool CanPerformActivities { get; }
       // DanceData LastDance { get; set; }
        bool IsAlive { get; }
        bool IsMovingDynamically { get; }
        bool IsMovingFast { get; }
        float FootSpeed { get; }
        bool IsResting { get; }
        bool IsSleeping { get; }
      //  bool IsSitting { get; }
       // bool IsLayingDown { get; }
        Sprinting Sprinting { get; }
    }
}
