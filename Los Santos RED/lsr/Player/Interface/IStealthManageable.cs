using LSR.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    internal interface IStealthManageable
    {
        bool IsInVehicle { get; }
        VehicleExt CurrentVehicle { get; }
        bool IsDuckingInVehicle { get; }
        WeatherReporting Weather { get; }
        ActivityManager ActivityManager { get; }
        Violations Violations { get; }
        string DebugString { get; set; }
        bool IsWanted { get; }
    }
}
