using Rage;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Interface
{
    public interface IGangs
    {
        Gang GetGang(string GangInitials);
        List<Gang> GetGangs(Ped cop);
        List<Gang> GetGangs(Vehicle CopCar);
        List<Gang> GetSpawnableGangs(int wantedLevel);
    }
}
