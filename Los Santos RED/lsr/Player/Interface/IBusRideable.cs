using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IBusRideable
    {
        ButtonPrompts ButtonPrompts { get; }
        GPSManager GPSManager { get; }
        bool IsInVehicle { get; }
        bool IsRidingBus { get; set; }
        bool IsGettingIntoAVehicle { get; }
        Ped Character { get; }
        Vehicle LastFriendlyVehicle { get; set; }
        VehicleExt CurrentVehicle { get; }
        float VehicleSpeedMPH { get; }
        bool IsNotWanted { get; }
        bool IsAliveAndFree { get; }
    }
}
