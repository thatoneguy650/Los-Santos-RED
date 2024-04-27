using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class PropSpawn
{
    public string ModelName { get; set; }   
    public SpawnPlace SpawnPlace { get; set; }
    public bool PlaceOnGround { get; set; } = false;
    public PropSpawn()
    {
    }

    public PropSpawn(string modelName, SpawnPlace spawnPlace)
    {
        ModelName = modelName;
        SpawnPlace = spawnPlace;
    }
}

