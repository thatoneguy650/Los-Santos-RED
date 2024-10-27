using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Serializable]
public class DispatchablePersonGroup
{
    public DispatchablePersonGroup()
    {
    }

    public DispatchablePersonGroup(string dispatchableVehicleGroupID, List<DispatchablePerson> dispatchablePeople)
    {
        DispatchablePersonGroupID = dispatchableVehicleGroupID;
        DispatchablePeople = dispatchablePeople;
    }

    public string DispatchablePersonGroupID { get; set; }
    public List<DispatchablePerson> DispatchablePeople { get; set; }
}

