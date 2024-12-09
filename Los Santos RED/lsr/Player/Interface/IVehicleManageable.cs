using LSR.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IVehicleManageable
    {
        bool IsInVehicle { get; }
        VehicleExt CurrentVehicle { get; }
        VehicleOwnership VehicleOwnership { get; }
    }
}
