using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IAssaultSpawnable
{
    int MaxAssaultSpawns { get; }
    List<SpawnPlace> AssaultSpawnLocations { get; }
    List<ConditionalLocation> PossiblePedSpawns { get; }
    bool RestrictAssaultSpawningUsingPedSpawns { get; }
    float AssaultSpawnHeavyWeaponsPercent { get; }
}

