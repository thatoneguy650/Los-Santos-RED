using Rage;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Interface
{
    public interface IAgencies
    {
        Agency GetAgency(string agencyInitials);
        Agency GetRandomMilitaryAgency();
        List<Agency> GetAgencies(Ped cop);
        List<Agency> GetAgencies(Vehicle CopCar);
        List<Agency> GetSpawnableAgencies(int WantedLevel);
        List<Agency> GetSpawnableHighwayAgencies(int wantedLevel);
    }
}
