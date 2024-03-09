using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class StoredSpawn : SpawnPlace
{
    [XmlIgnore]
    public int CellX { get; private set; }
    [XmlIgnore]
    public int CellY { get; private set; }
    public bool IsPedestrianOnlySpawn { get; set; }
    public float MinSpawnDistance { get; set; } = 75f;
    public float MaxSpawnDistance { get; set; } = 200f;
    public StoredSpawn()
    {
    }

    public StoredSpawn(Vector3 position, float heading) : base(position, heading)
    {
    }

    public void Setup()
    {
        CellX = (int)(Position.X / EntryPoint.CellSize);
        CellY = (int)(Position.Y / EntryPoint.CellSize);
    }
}

