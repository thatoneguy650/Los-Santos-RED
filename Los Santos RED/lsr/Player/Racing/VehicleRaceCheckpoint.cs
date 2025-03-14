using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


public class VehicleRaceCheckpoint
{
    public VehicleRaceCheckpoint()
    {
    }

    public VehicleRaceCheckpoint(int order, Vector3 position)
    {
        Order = order;
        Position = position;
    }

    public int Order { get; set; }
    public Vector3 Position { get; set; }
    [XmlIgnore]
    public bool IsFinish { get; set; } = false;
}

