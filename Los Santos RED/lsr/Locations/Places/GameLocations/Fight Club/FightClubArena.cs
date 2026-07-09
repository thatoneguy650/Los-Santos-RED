using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class FightClubArena
{
    public Vector3 ArenaCenter { get; set; }
    public List<SpawnPlace> FighterSpawnLocations { get; set; }
    public List<SpawnPlace> SpectatorLocations { get; set; }
    public FightClubArena()
    {

    }

    public FightClubArena(Vector3 arenaCenter, List<SpawnPlace> fighterSpawnLocations, List<SpawnPlace> spectatorLocations)
    {
        ArenaCenter = arenaCenter;
        FighterSpawnLocations = fighterSpawnLocations;
        SpectatorLocations = spectatorLocations;
    }
}

