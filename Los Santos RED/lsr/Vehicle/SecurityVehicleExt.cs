using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSR.Vehicles
{
    public class SecurityVehicleExt : VehicleExt
    {
        public override bool CanRandomlyHaveIllegalItems { get; set; } = false;
        public SecurityVehicleExt(Vehicle vehicle, ISettingsProvideable settings) : base(vehicle, settings)
        {
        }
    }
}
