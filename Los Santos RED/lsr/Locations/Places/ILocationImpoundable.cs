using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ILocationImpoundable
{
    VehicleImpoundLot VehicleImpoundLot { get; }
    bool HasImpoundLot { get; }
    Vector3 EntrancePosition { get; }
    string Name { get; }
}
