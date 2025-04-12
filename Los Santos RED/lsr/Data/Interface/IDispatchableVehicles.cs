using System.Collections.Generic;

namespace LosSantosRED.lsr.Interface
{
    public interface IDispatchableVehicles
    {
        List<DispatchableVehicleGroup> AllVehicles { get; }

        List<DispatchableVehicle> GetVehicleData(string dispatchableVehicleGroupID);
    }
}