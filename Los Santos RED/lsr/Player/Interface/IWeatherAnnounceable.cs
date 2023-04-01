using LosSantosRED.lsr.Locations;
using LSR.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IWeatherAnnounceable
    {
        bool IsDead { get; }
        bool IsBusted { get; }
        bool IsInVehicle { get; }
        bool IsWanted { get; }
        bool IsNotWanted { get; }
        bool IsAliveAndFree { get; }
        Investigation Investigation { get; }
    }
}
