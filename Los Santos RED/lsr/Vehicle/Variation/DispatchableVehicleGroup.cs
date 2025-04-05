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
    public DispatchableVehicleGroup(string dispatchableVehicleGroupID, string name, string description, List<DispatchableVehicle> dispatchableVehicles, DispatchbleVehicleGroupType dispatchbleVehicleGroupType)
    {
        Name = name;
        Description = description;
        DispatchableVehicleGroupID = dispatchableVehicleGroupID;
        DispatchableVehicles = dispatchableVehicles;
        DispatchbleVehicleGroupType = dispatchbleVehicleGroupType;
    }
    public string DispatchableVehicleGroupID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<DispatchableVehicle> DispatchableVehicles { get; set; }
    public DispatchbleVehicleGroupType DispatchbleVehicleGroupType { get; set; } = DispatchbleVehicleGroupType.Other;

    public override string ToString()
    {
        return DispatchableVehicleGroupID.ToString();
    }
}

