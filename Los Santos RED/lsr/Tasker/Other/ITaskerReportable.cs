using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface ITaskerReportable
{
    bool IsSeatAssignedToAnyone(VehicleExt copCar, int seat);
    bool IsSeatAssigned(ISeatAssignable ped,VehicleExt copCar, int seat);
    void RemoveSeatAssignment(ISeatAssignable ped);
    bool AddSeatAssignment(ISeatAssignable ped, VehicleExt vehicle, int seat);
}


