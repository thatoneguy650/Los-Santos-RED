using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CountyJurisdiction
{
    public string AgencyInitials { get; set; } = "";
    public Zone.County County { get; set; }
    public int Priority { get; set; } = 99;
    public int AmbientSpawnChance { get; set; } = 0;
    public int WantedSpawnChance { get; set; } = 0;
    public bool CanSpawnPedestrianOfficers { get; set; } = false;
    public Agency GameAgency
    {
        get
        {
            return Agencies.GetAgency(AgencyInitials);
        }
    }
    public bool CanCurrentlySpawn
    {
        get
        {
            if (PlayerState.IsWanted)
                return WantedSpawnChance > 0;
            else
                return AmbientSpawnChance > 0;
        }
    }
    public int CurrentSpawnChance
    {
        get
        {
            if (PlayerState.IsWanted)
                return WantedSpawnChance;
            else
                return AmbientSpawnChance;
        }
    }
    public CountyJurisdiction()
    {

    }
    public CountyJurisdiction(string agencyInitials, Zone.County county, int priority, int ambientSpawnChance, int wantedSpawnChance)
    {
        AgencyInitials = agencyInitials;
        County = county;
        Priority = priority;
        AmbientSpawnChance = ambientSpawnChance;
        WantedSpawnChance = wantedSpawnChance;
    }

}