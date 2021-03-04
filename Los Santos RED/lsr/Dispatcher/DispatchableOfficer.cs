public class DispatchableOfficer
{
    public string ModelName { get; set; }
    public int AmbientSpawnChance { get; set; } = 0;
    public int WantedSpawnChance { get; set; } = 0;
    public int MinWantedLevelSpawn { get; set; } = 0;
    public int MaxWantedLevelSpawn { get; set; } = 5;
    public PedVariation RequiredVariation { get; set; }
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
    public DispatchableOfficer()
    {
    }
    public DispatchableOfficer(string _ModelName, int ambientSpawnChance, int wantedSpawnChance)
    {
        ModelName = _ModelName;
        AmbientSpawnChance = ambientSpawnChance;
        WantedSpawnChance = wantedSpawnChance;
    }
}