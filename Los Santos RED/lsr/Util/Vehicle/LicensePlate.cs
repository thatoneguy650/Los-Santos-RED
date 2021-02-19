using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSR.Vehicles
{
    public class LicensePlate
    {
        public string PlateNumber { get; set; }
        public bool IsWanted { get; set; }
        public int PlateType { get; set; }
        public LicensePlate(string plateNumber, int plateType, bool isWanted)
        {
            PlateNumber = plateNumber;
            PlateType = plateType;
            IsWanted = isWanted;
        }
        public override string ToString()
        {
            if (IsWanted)
                return PlateNumber + "!";
            else
                return PlateNumber;

        }
    }
}