using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ZoneJurisdiction 
{
    public string AgencyInitials { get; set; } = "";
    public string ZoneInternalGameName { get; set; } = "";
    public int Priority { get; set; } = 99;
    public int AmbientSpawnChance { get; set; } = 0;
    public int WantedSpawnChance { get; set; } = 0;
    public bool CanSpawnPedestrianOfficers { get; set; } = false;
    //public Zone GameZone
    //{
    //    get
    //    {
    //        return DataMart.Zones.GetZone(ZoneInternalGameName);
    //    }
    //}
    //public Agency GameAgency
    //{
    //    get
    //    {
    //        return DataMart.Agencies.GetAgency(AgencyInitials);
    //    }
    //}
    public ZoneJurisdiction()
    {

    }
    public ZoneJurisdiction(string agencyInitials, string zoneInternalName, int priority, int ambientSpawnChance, int wantedSpawnChance)
    {
        AgencyInitials = agencyInitials;
        ZoneInternalGameName = zoneInternalName;
        Priority = priority;
        AmbientSpawnChance = ambientSpawnChance;
        WantedSpawnChance = wantedSpawnChance;
    }
    public bool CanCurrentlySpawn(int WantedLevel)
    {
        if (WantedLevel > 0)
        {
            return WantedSpawnChance > 0;
        }
        else
        {
            return AmbientSpawnChance > 0;
        }
    }
    public int CurrentSpawnChance(int WantedLevel)
    {
        if (WantedLevel > 0)
        {
            return WantedSpawnChance;
        }
        else
        {
            return AmbientSpawnChance;
        }
    }
}
