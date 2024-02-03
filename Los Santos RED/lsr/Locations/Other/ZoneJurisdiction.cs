using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ZoneJurisdiction 
{
    public string ZoneInternalGameName { get; set; } = "";
    public string AgencyID { get; set; } = "";
    public int Priority { get; set; } = 99;
    public int AmbientSpawnChance { get; set; } = 0;
    public int WantedSpawnChance { get; set; } = 0;
    public bool CanSpawnPedestrianOfficers { get; set; } = false;
    public bool CanSpawnBicycleOfficers { get; set; } = false;
    public bool CanSpawnDirtBikeOfficers { get; set; } = false;
    public ZoneJurisdiction()
    {

    }
    public ZoneJurisdiction(string agencyID, string zoneInternalName, int priority, int ambientSpawnChance, int wantedSpawnChance)
    {
        AgencyID = agencyID;
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
