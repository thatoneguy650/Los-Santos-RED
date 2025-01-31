using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IPlateTypes
    {
        PlateType GetRandomPlateType(bool isMotorcycle);
        PlateType GetPlateType(int v);
        PlateType GetPlateType(string State);
        string GetRandomVanityPlateText();
        PlateType GetPlateByDescription(string plateTypeName);
        PlateType GetRandomInStatePlate(string stateID, bool isMotorcycle);

        PlateTypeManager PlateTypeManager { get; }
    }
}
