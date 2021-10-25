using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WorldSettings
{
    public bool AddPOIBlipsToMap { get; set; } = true;
    public bool UpdateVehiclePlates { get; set; } = true;
    public bool CleanupVehicles { get; set; } = true;
    public WorldSettings()
    {

    }

}