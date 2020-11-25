using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LicensePlate
{
    public string PlateNumber { get; private set; }
    public bool IsWanted { get; set; }
    public uint OriginalVehicleHandle { get; set; }
    public int PlateType { get; set; }
    public LicensePlate(string plateNumber,uint originalVehicleHandle,int plateType,bool isWanted)
    {
        PlateNumber = plateNumber;
        OriginalVehicleHandle = originalVehicleHandle;
        PlateType = plateType;
        IsWanted = isWanted;
    }
    public override string ToString()
    {
        if(IsWanted)
            return PlateNumber + "!";
        else
            return PlateNumber;

    }
}

