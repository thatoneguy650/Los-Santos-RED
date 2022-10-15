using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class DispatchableVehicleGroup
{
    public DispatchableVehicleGroup()
    {
    }

    public DispatchableVehicleGroup(string dispatchableVehicleGroupID, List<DispatchableVehicle> dispatchableVehicles)
    {
        DispatchableVehicleGroupID = dispatchableVehicleGroupID;
        DispatchableVehicles = dispatchableVehicles;
    }

    public string DispatchableVehicleGroupID { get; set; }
    public List<DispatchableVehicle> DispatchableVehicles { get; set; }
}

