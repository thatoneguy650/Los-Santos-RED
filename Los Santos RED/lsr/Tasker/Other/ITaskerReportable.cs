using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface ITaskerReportable
{
    bool IsSeatAssigned(IComplexTaskable ped,VehicleExt copCar, int seat);
    void RemoveSeatAssignment(IComplexTaskable ped);
    bool AddSeatAssignment(IComplexTaskable ped, VehicleExt vehicle, int seat);
}


