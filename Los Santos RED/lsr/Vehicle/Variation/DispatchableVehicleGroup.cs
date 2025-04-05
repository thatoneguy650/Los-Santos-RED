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
    public DispatchableVehicleGroup(string dispatchableVehicleGroupID, List<DispatchableVehicle> dispatchableVehicles, DispatchbleVehicleGroupType dispatchbleVehicleGroupType)
    {
        DispatchableVehicleGroupID = dispatchableVehicleGroupID;
        DispatchableVehicles = dispatchableVehicles;
        DispatchbleVehicleGroupType = dispatchbleVehicleGroupType;
    }
    public string DispatchableVehicleGroupID { get; set; }
    public List<DispatchableVehicle> DispatchableVehicles { get; set; }
    public DispatchbleVehicleGroupType DispatchbleVehicleGroupType { get; set; } = DispatchbleVehicleGroupType.Other;

    public override string ToString()
    {
        return DispatchableVehicleGroupID.ToString();
    }
}

