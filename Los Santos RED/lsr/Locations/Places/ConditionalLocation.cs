using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ConditionalLocation
{
    public ConditionalLocation()
    { 
    }
    public ConditionalLocation(Vector3 location, float heading, float percentage)
    {
        Location = location;
        Heading = heading;
        Percentage = percentage;
    }

    public Vector3 Location { get; set; }
    public float Heading { get; set; }
    public float Percentage { get; set; }
}

