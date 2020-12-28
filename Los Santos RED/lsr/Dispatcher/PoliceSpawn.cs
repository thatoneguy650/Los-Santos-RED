using Rage;


public class PoliceSpawn
{
    private SpawnLocation SpawnLocation;

    public PoliceSpawn(SpawnLocation spawnLocation, Agency agency, int currentWantedLevel, bool canSpawnHelicopter, bool canSpawnBoat, bool addBlip)
    {
        SpawnLocation = spawnLocation;
        WantedLevel = currentWantedLevel;
        CanSpawnHelicopter = canSpawnHelicopter;
        CanSpawnBoat = canSpawnBoat;
        Agency = agency;
        AddBlip = addBlip;
    }
    public bool AddBlip { get; set; }
    public Agency Agency { get; set; }
    public bool CanSpawnBoat { get; set; }
    public bool CanSpawnHelicopter { get; set; }
    public int WantedLevel { get; set; }
    public Vector3 InitialPosition
    {
        get
        {
            return SpawnLocation.InitialPosition;
        }
    }
    public Vector3 StreetPosition
    {
        get
        {
            return SpawnLocation.StreetPosition;
        }
    }
    public float Heading
    {
        get
        {
            return SpawnLocation.Heading;
        }
    }

}

