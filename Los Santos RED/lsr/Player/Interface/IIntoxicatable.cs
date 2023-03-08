using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IIntoxicatable : IActivityPerformable
    {
        HealthManager HealthManager { get; }
        Scenario ClosestScenario { get; }
      //  bool IsSitting { get; set; }
        List<Rage.Object> AttachedProp { get; set; }
        Intoxication Intoxication { get; }
        Sprinting Sprinting { get; }
        bool IsOnMuscleRelaxants { get; set; }
        HumanState HumanState { get; }
        string Gender { get; }
        ClipsetManager ClipsetManager { get; }
        // void PauseCurrentActivity();
    }
}
