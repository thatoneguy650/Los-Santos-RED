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
        bool IsIntoxicated { get; set; }
        float IntoxicatedIntensity { get; set; }
        Vector3 Position { get; }
       // bool IsMoveControlPressed { get; }
        Scenario ClosestScenario { get; }
        bool IsInVehicle { get; }
        bool IsDriver { get; }
        VehicleExt CurrentVehicle { get; }
        bool IsSitting { get; set; }
        Rage.Object AttachedProp { get; set; }
        float IntoxicatedIntensityPercent { get; set; }
        Intoxication Intoxication { get; }
        string DebugLine4 { get; set; }
        Sprinting Sprinting { get; }
        bool IsOnMuscleRelaxants { get; set; }

        // void SetUnarmed();
        void PauseDynamicActivity();
        void ChangeHealth(int healthGained);
    }
}
