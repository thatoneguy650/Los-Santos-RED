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
        private string registrationId;

        public string PlateNumber { get; set; }
        public bool IsWanted { get; set; }
        public int PlateType { get; set; }
        public uint OriginalModelHash { get; set; }

        // Stable registration identity. The lazy fallback supports older serialized
        // LicensePlate objects that predate this field.
        public string RegistrationId
        {
            get
            {
                if (string.IsNullOrEmpty(registrationId))
                {
                    registrationId = Guid.NewGuid().ToString("N");
                }
                return registrationId;
            }
            set
            {
                registrationId = value;
            }
        }
        public LicensePlate(string plateNumber, int plateType, bool isWanted, uint originalModelHash)
        {
            PlateNumber = plateNumber;
            PlateType = plateType;
            IsWanted = isWanted;
            OriginalModelHash = originalModelHash;
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