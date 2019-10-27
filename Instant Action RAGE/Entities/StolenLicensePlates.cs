using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class StolenLicensePlate
{
    public string PlateNumber { get; set; }
    public bool CopsHaveSeen { get; set; } = false;
    public uint OriginalVehicleHandle { get; set; }
    public StolenLicensePlate()
    {

    }
    public StolenLicensePlate(string _PlateNumber,uint _OriginalVehicleHandle)
    {
        PlateNumber = _PlateNumber;
        OriginalVehicleHandle = _OriginalVehicleHandle;
    }
    public override string ToString()
    {
        return PlateNumber;
    }
}

