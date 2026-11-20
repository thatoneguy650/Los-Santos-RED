using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
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

        private static readonly HashSet<string> WantedPlateNumbers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private static readonly object WantedPlateNumbersLock = new object();
        private bool isWanted;

        public string PlateNumber { get; set; }
        public bool IsWanted
        {
            get
            {
                if (isWanted) return true;
                string normalizedPlate = NormalizePlateNumber(PlateNumber);
                if (string.IsNullOrEmpty(normalizedPlate)) return false;
                lock (WantedPlateNumbersLock)
                {
                    return WantedPlateNumbers.Contains(normalizedPlate);
                }
            }
            set
            {
                isWanted = value;
                if (value) RegisterWantedPlateNumber(PlateNumber);
            }
        }
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
        public bool HasRegistrationId => !string.IsNullOrEmpty(registrationId);
        private static string NormalizePlateNumber(string plateNumber)
        {
            return string.IsNullOrWhiteSpace(plateNumber) ? string.Empty : plateNumber.Trim().ToUpperInvariant();
        }

        public static void RegisterWantedPlateNumber(string plateNumber)
        {
            string normalizedPlate = NormalizePlateNumber(plateNumber);
            if (string.IsNullOrEmpty(normalizedPlate)) return;
            lock (WantedPlateNumbersLock)
            {
                WantedPlateNumbers.Add(normalizedPlate);
            }
        }

        public static void ClearWantedPlateNumber(string plateNumber)
        {
            string normalizedPlate = NormalizePlateNumber(plateNumber);
            if (string.IsNullOrEmpty(normalizedPlate)) return;
            lock (WantedPlateNumbersLock)
            {
                WantedPlateNumbers.Remove(normalizedPlate);
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (isWanted) RegisterWantedPlateNumber(PlateNumber);
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