using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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



    public string AssociationID { get; set; }
    public string RequiredGroup { get; set; }

    public SpawnRequirement SpawnRequirement { get; set; } = SpawnRequirement.None;

    //[XmlIgnore]
    //public ILocationDispatchable LocationDispatchable { get; set; }


}

