using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

[Serializable()]
public class GangTerritory 
{
    public string ZoneInternalGameName { get; set; } = "";
    public string GangID { get; set; } = "";
    public int Priority { get; set; } = 99;
    public int AmbientSpawnChance { get; set; } = 0;
    public int WantedSpawnChance { get; set; } = 0;
    [XmlIgnore]
    public float DrugDealerPercentage { get; set; } = -1f;
    // Arsenal
    [XmlIgnore]
    public float PercentageWithLongGuns { get; set; }  = -1f;
    [XmlIgnore]
    public float PercentageWithSidearms { get; set; }  = -1f;
    [XmlIgnore]
    public float PercentageWithMelee { get; set; }  = -1f;
    // Violence
    [XmlIgnore]
    public float FightPercentage { get; set; } = -1f;
    [XmlIgnore]
    public float FightPolicePercentage { get; set; } = -1f;
    [XmlIgnore]
    public float AlwaysFightPolicePercentage { get; set; } = -1f;
    public GangTerritory()
    {

    }
    public GangTerritory(string gangID, string zoneInternalName, int priority, int ambientSpawnChance, int wantedSpawnChance)
    {
        GangID = gangID;
        ZoneInternalGameName = zoneInternalName;
        Priority = priority;
        AmbientSpawnChance = ambientSpawnChance;
        WantedSpawnChance = wantedSpawnChance;
    }
    public GangTerritory(string gangID, string zoneInternalName, int priority, int ambientSpawnChance, int wantedSpawnChance, float drugDealerPercentage, float percentageWithLongGuns, 
        float percentageWithSidearms, float percentageWithMelee, float fightPercentage, float fightPolicePercentage, float alwaysFightPolicePercentage)
    {
        GangID = gangID;
        ZoneInternalGameName = zoneInternalName;
        Priority = priority;
        AmbientSpawnChance = ambientSpawnChance;
        WantedSpawnChance = wantedSpawnChance;
        DrugDealerPercentage = drugDealerPercentage;
        PercentageWithLongGuns = percentageWithLongGuns;
        PercentageWithSidearms = percentageWithSidearms;
        PercentageWithMelee = percentageWithMelee;
        FightPercentage = fightPercentage;
        FightPolicePercentage = fightPolicePercentage;
        AlwaysFightPolicePercentage = alwaysFightPolicePercentage;
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
    public void Setup(Gang gang)
    {
        if (DrugDealerPercentage < 0) DrugDealerPercentage = gang.DrugDealerPercentage;
        if (PercentageWithLongGuns < 0) PercentageWithLongGuns = gang.PercentageWithLongGuns;
        if (PercentageWithSidearms < 0) PercentageWithSidearms = gang.PercentageWithSidearms;
        if (PercentageWithMelee < 0) PercentageWithMelee = gang.PercentageWithMelee;
        if (FightPercentage < 0) FightPercentage = gang.FightPercentage;
        if (FightPolicePercentage < 0) FightPolicePercentage = gang.FightPolicePercentage;
        if (AlwaysFightPolicePercentage < 0) AlwaysFightPolicePercentage = gang.AlwaysFightPolicePercentage;
    }
}
