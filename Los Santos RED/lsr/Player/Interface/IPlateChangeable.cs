using LSR.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IPlateChangeable
    {
        List<VehicleExt> TrackedVehicles { get; }
        bool IsChangingLicensePlates { get; set; }
        List<LicensePlate> SpareLicensePlates { get; }

        void SetUnarmed();
    }
}
