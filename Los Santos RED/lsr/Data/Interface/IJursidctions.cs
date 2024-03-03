using LosSantosRED.lsr.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IJurisdictions
    {
        Agency GetRandomAgency(string internalGameName, int wantedLevel, ResponseType responseType);
        Agency GetRandomCountyAgency(string countyID, int wantedLevel, ResponseType responseType);
        List<Agency> GetAgencies(string internalGameName, int wantedLevel, ResponseType responseType);
        Agency GetMainAgency(string internalGameName, ResponseType responseType);
        Agency GetNthAgency(string internalGameName, ResponseType lawEnforcement, int v);
        string TestString();
        bool CanSpawnAmbientPedestrians(string internalGameName, Agency agency);
        ZoneJurisdiction GetJurisdiction(string internalGameName, Agency agency);
        Agency GetRespondingAgency(string internalGameName, string countyID, ResponseType fire);
        // bool CanSpawnPedestrianAtZone(string v, string iD);
    }
}
