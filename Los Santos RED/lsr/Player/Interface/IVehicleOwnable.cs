using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IVehicleOwnable
    {
        VehicleExt CurrentVehicle { get; }
        Ped Character { get; }
        string PlayerName { get; }
        bool IsCop { get; }
        VehicleManager VehicleManager { get; }
    }
}
