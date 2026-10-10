using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VehicleBaseHandlingData
{
    public VehicleBaseHandlingData(uint modelHash, float deformationDamageMultiplier)
    {
        ModelHash = modelHash;
        VanillaDeformationDamageMultiplier = deformationDamageMultiplier;
    }

    public uint ModelHash { get; set; }
    public float VanillaDeformationDamageMultiplier { get; set; }
    public HandlingData HandlingData { get; set; }
}

