using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ITargetable
    {
        bool IsInVehicle { get; }
        VehicleExt CurrentVehicle { get; }
        bool IsAttemptingToSurrender { get; }
        bool IsBusted { get; }
        int WantedLevel { get; }
        Ped Character { get; }
        bool IsStill { get; }
        bool IsMovingFast { get; }
        bool IsWanted { get; }
        PoliceResponse PoliceResponse { get; }
        bool IsInSearchMode { get; }
        float ActiveDistance { get; }
    }
}
