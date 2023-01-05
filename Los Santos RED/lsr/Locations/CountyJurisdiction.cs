using LosSantosRED.lsr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CountyJurisdiction
{
    public string CountyID { get; set; }
    public string AgencyInitials { get; set; } = "";
    public int Priority { get; set; } = 99;
    public int AmbientSpawnChance { get; set; } = 0;
    public int WantedSpawnChance { get; set; } = 0;
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
    public CountyJurisdiction()
    {

    }
    public CountyJurisdiction(string agencyInitials, string countyID, int priority, int ambientSpawnChance, int wantedSpawnChance)
    {
        AgencyInitials = agencyInitials;
        CountyID = countyID;
        Priority = priority;
        AmbientSpawnChance = ambientSpawnChance;
        WantedSpawnChance = wantedSpawnChance;
    }

}