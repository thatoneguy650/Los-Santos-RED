using LosSantosRED.lsr.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IZoneJurisdictions
    {
        Agency GetRandomAgency(string internalGameName, int wantedLevel, ResponseType responseType);
        List<Agency> GetAgencies(string internalGameName, int wantedLevel, ResponseType responseType);
        Agency GetMainAgency(string internalGameName, ResponseType responseType);
    }
}
