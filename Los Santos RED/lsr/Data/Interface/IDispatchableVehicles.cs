using System.Collections.Generic;

namespace LosSantosRED.lsr.Interface
{
    public interface IDispatchableVehicles
    {
        List<DispatchableVehicle> GetVehicleData(string dispatchableVehicleGroupID);
    }
}