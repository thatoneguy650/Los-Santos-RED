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
        Agency GetRandomAgency(string internalGameName, int wantedLevel);
        List<Agency> GetAgencies(string internalGameName, int wantedLevel);
        Agency GetMainAgency(string internalGameName);
    }
}
