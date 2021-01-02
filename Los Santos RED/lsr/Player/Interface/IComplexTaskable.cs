using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IComplexTaskable
    {
        bool IsInVehicle { get; }
        bool IsInHelicopter { get; }
        float DistanceToPlayer { get; }
        bool IsDriver { get; }
        bool IsStill { get; }
        Ped Pedestrian { get; }
        int LastSeatIndex { get; }
    }
}
