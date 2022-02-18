public class DispatchablePerson
{
    public string ModelName { get; set; }
    public int AmbientSpawnChance { get; set; } = 0;
    public int WantedSpawnChance { get; set; } = 0;
    public int MinWantedLevelSpawn { get; set; } = 0;
    public int MaxWantedLevelSpawn { get; set; } = 6;
    public int HealthMin { get; set; } = 85;
    public int HealthMax { get; set; } = 125;
    public int ArmorMin { get; set; } = 0;
    public int ArmorMax { get; set; } = 50;
    public PedVariation RequiredVariation { get; set; }
    public bool RandomizeHead { get; set; }
    public bool CanCurrentlySpawn(int WantedLevel)
    {
        if (WantedLevel > 0)
        {
            if (WantedLevel >= MinWantedLevelSpawn && WantedLevel <= MaxWantedLevelSpawn)
            {
                return WantedSpawnChance > 0;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return AmbientSpawnChance > 0;
        }
    }
    public int CurrentSpawnChance(int WantedLevel)
    {
        if (!CanCurrentlySpawn(WantedLevel))
        {
            return 0;
        }
        if (WantedLevel > 0)
        {
            if (WantedLevel >= MinWantedLevelSpawn && WantedLevel <= MaxWantedLevelSpawn)
            {
                return WantedSpawnChance;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return AmbientSpawnChance;
        }
    }
    public DispatchablePerson()
    {

    }
    public DispatchablePerson(string _ModelName, int ambientSpawnChance, int wantedSpawnChance)
    {
        ModelName = _ModelName;
        AmbientSpawnChance = ambientSpawnChance;
        WantedSpawnChance = wantedSpawnChance;
    }
}