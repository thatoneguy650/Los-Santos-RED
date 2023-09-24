using Rage;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Interface
{
    public interface IAgencies
    {
        Agency GetAgency(string agencyInitials);
        List<Agency> GetAgencies(Ped cop);
        List<Agency> GetAgencies(Vehicle CopCar);
        List<Agency> GetSpawnableAgencies(int WantedLevel, ResponseType responseType);
        List<Agency> GetSpawnableHighwayAgencies(int wantedLevel, ResponseType responseType);
        Agency GetRandomAgency(ResponseType lawEnforcement);
        List<Agency> GetAgencies();
        List<Agency> GetAgenciesByResponse(ResponseType responseType);
    }
}
