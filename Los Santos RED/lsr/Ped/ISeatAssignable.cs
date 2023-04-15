using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ISeatAssignable
    {
        bool IsInVehicle { get; }
        int AssignedSeat { get; }
        List<uint> BlackListedVehicles { get; }
        Ped Pedestrian { get; }
        int LastSeatIndex { get; }
        uint Handle { get; }
        VehicleExt AssignedVehicle { get; }
        bool IsAnimal { get; }
    }
}
