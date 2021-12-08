using LosSantosRED.lsr.Locations;
using LSR.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IDriveable
    {
        void OnVehicleHealthDecreased(int amount, bool isCollision);
        void OnVehicleEngineHealthDecreased(float amount, bool isCollision);
        void OnVehicleStartedFire();
    }
}
