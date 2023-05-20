using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSR.Vehicles
{
    [Serializable]
    public class LicensePlate
    {
        public LicensePlate()
        {

        }
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
        public string GenerateDescription()
        {
            return $"Plate that can be applied to most vehicles.~n~Number: {PlateNumber}~n~Type: {PlateType}~n~Wanted: {(IsWanted ? "Yes" : "No")}";
        }
    }
}