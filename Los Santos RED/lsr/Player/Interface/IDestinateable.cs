using LosSantosRED.lsr.Locations;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IDestinateable
    {
        Vector3 Position { get; }
        bool IsInVehicle { get; }
        VehicleExt CurrentVehicle { get; }
        Ped Character { get; }
        VehicleExt InterestedVehicle { get; }
    }
}
