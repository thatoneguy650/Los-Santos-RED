using LSR.Vehicles;
using Rage;
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
    public VehicleVariation(string modelName, int primaryColor, int secondaryColor, LicensePlate licensePlate, Vector3 position, float heading)
    {
        ModelName = modelName;
        PrimaryColor = primaryColor;
        SecondaryColor = secondaryColor;
        LicensePlate = licensePlate;
        LastPosition = position;
        LastHeading = heading;
    }
    public VehicleVariation(uint modelHash, int primaryColor, int secondaryColor, LicensePlate licensePlate, Vector3 position, float heading)
    {
        ModelHash = modelHash;
        PrimaryColor = primaryColor;
        SecondaryColor = secondaryColor;
        LicensePlate = licensePlate;
        LastPosition = position;
        LastHeading = heading;
    }
    public string ModelName { get; set; } = "";
    public uint ModelHash { get; set; }
    public int PrimaryColor {get;set;}
    public int SecondaryColor { get; set; }
    public bool HasModeName => ModelName != "";
    public LicensePlate LicensePlate { get; set; }
    public Vector3 LastPosition { get; set; } = Vector3.Zero;
    public float LastHeading { get; set; }
}

