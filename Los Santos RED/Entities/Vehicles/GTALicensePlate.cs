using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GTALicensePlate
{
    public string PlateNumber { get; set; }
    public bool IsWanted { get; set; } = false;
    public uint OriginalVehicleHandle { get; set; }
    public int PlateType { get; set; }
    public GTALicensePlate()
    {

    }
    public GTALicensePlate(string _PlateNumber,uint _OriginalVehicleHandle,int _PlateType,bool _IsWanted)
    {
        PlateNumber = _PlateNumber;
        OriginalVehicleHandle = _OriginalVehicleHandle;
        PlateType = _PlateType;
        IsWanted = _IsWanted;
    }
    public override string ToString()
    {
        if(IsWanted)
            return PlateNumber + "!";
        else
            return PlateNumber;

    }
}

