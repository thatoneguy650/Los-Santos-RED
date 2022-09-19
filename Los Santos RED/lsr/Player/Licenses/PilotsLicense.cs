using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PilotsLicense : License
{
    public PilotsLicense()
    {
    }
    public bool IsRotaryEndorsed { get; set; } = false;
    public bool IsFixedWingEndorsed { get; set; } = false;
    public bool IsLighterThanAirEndorsed { get; set; } = false;
    public bool CanFlyType(VehicleClass vehicleClass )
    {
        if(vehicleClass == VehicleClass.Helicopter)
        {
            return IsRotaryEndorsed;
        }
        else if (vehicleClass == VehicleClass.Plane)
        {
            return IsFixedWingEndorsed;
        }
        return false;
    }

}
