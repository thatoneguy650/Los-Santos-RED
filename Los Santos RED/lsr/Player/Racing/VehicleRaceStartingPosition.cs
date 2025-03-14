using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class VehicleRaceStartingPosition
{
    public VehicleRaceStartingPosition()
    {
    }

    public VehicleRaceStartingPosition(int order, Vector3 position , float heading)
    {
        Order = order;
        Position = position;
        Heading = heading;
    }

    public int Order { get; set; }
    public Vector3 Position { get; set; }
    public float Heading { get; set; }
}

