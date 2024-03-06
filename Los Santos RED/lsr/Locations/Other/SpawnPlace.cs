using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class SpawnPlace
{
    public SpawnPlace()
    {
    }

    public SpawnPlace(Vector3 position, float heading)
    {
        Position = position;
        Heading = heading;
    }

    public Vector3 Position { get; set; }
    public float Heading { get; set; }

    public void AddDistanceOffset(Vector3 offsetToAdd)
    {
        Position += offsetToAdd;
    }

}

