using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ITaskAssignable
    {
        GangRelationships GangRelationships { get; }
        CellPhone CellPhone { get; }
        VehicleExt CurrentVehicle { get; }

        void GiveMoney(int paymentAmountOnCompletion);
    }
}
