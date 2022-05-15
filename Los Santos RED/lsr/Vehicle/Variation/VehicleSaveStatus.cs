using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class VehicleSaveStatus
{
    public VehicleSaveStatus()
    {

    }
    public VehicleSaveStatus(string modelName, Vector3 position, float heading)
    {
        ModelName = modelName;
        LastPosition = position;
        LastHeading = heading;
    }
    public VehicleSaveStatus(uint modelHash, Vector3 position, float heading)
    {
        ModelHash = modelHash;
        LastPosition = position;
        LastHeading = heading;
    }
    public string ModelName { get; set; } = "";
    public uint ModelHash { get; set; }
    public bool HasModeName => ModelName != "";
    public Vector3 LastPosition { get; set; } = Vector3.Zero;
    public float LastHeading { get; set; }
    public VehicleVariation VehicleVariation { get; set; }




}

