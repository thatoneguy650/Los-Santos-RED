using LosSantosRED.lsr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PedestrianInformation
{
    public string ModelName { get; set; }
    public int AmbientSpawnChance { get; set; } = 0;
    public int WantedSpawnChance { get; set; } = 0;
    public int MinWantedLevelSpawn { get; set; } = 0;
    public int MaxWantedLevelSpawn { get; set; } = 5;
    public PedVariation RequiredVariation { get; set; }
    public bool CanCurrentlySpawn
    {
        get
        {
            if (Mod.Player.IsWanted)
            {
                if (Mod.Player.WantedLevel >= MinWantedLevelSpawn && Mod.Player.WantedLevel <= MaxWantedLevelSpawn)
                    return WantedSpawnChance > 0;
                else
                    return false;
            }
            else
                return AmbientSpawnChance > 0;
        }
    }
    public int CurrentSpawnChance
    {
        get
        {
            if (!CanCurrentlySpawn)
                return 0;
            if (Mod.Player.IsWanted)
            {
                if (Mod.Player.WantedLevel >= MinWantedLevelSpawn && Mod.Player.WantedLevel <= MaxWantedLevelSpawn)
                    return WantedSpawnChance;
                else
                    return 0;
            }
            else
                return AmbientSpawnChance;
        }
    }
    public bool CanSpawnWanted
    {
        get
        {
            if (WantedSpawnChance > 0)
                return true;
            else
                return false;
        }
    }
    public bool CanSpawnAmbient
    {
        get
        {
            if (AmbientSpawnChance > 0)
                return true;
            else
                return false;
        }
    }
    public PedestrianInformation()
    {

    }
    public PedestrianInformation(string _ModelName, int ambientSpawnChance, int wantedSpawnChance)
    {
        ModelName = _ModelName;
        AmbientSpawnChance = ambientSpawnChance;
        WantedSpawnChance = wantedSpawnChance;
    }
}