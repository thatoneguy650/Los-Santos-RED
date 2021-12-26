using LSR.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class VehicleVariation
{
    public VehicleVariation()
    {

    }
    public VehicleVariation(string modelName, int primaryColor, int secondaryColor, LicensePlate licensePlate)
    {
        ModelName = modelName;
        PrimaryColor = primaryColor;
        SecondaryColor = secondaryColor;
        LicensePlate = licensePlate;
    }
    public VehicleVariation(uint modelHash, int primaryColor, int secondaryColor, LicensePlate licensePlate)
    {
        ModelHash = modelHash;
        PrimaryColor = primaryColor;
        SecondaryColor = secondaryColor;
        LicensePlate = licensePlate;
    }
    public string ModelName { get; set; } = "";
    public uint ModelHash { get; set; }
    public int PrimaryColor {get;set;}
    public int SecondaryColor { get; set; }
    public bool HasModeName => ModelName != "";
    public LicensePlate LicensePlate { get; set; }
}

