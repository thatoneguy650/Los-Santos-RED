using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class FireStation : BasicLocation
{
    public FireStation(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public FireStation() : base()
    {

    }
    public override string TypeName { get; set; } = "Fire Station";
    public override int MapIcon { get; set; } = 436;
    public override Color MapIconColor { get; set; } = Color.White;
    public override float MapIconScale { get; set; } = 1.0f;


    public List<ConditionalLocation> PossiblePedSpawns { get; set; }
    public List<ConditionalLocation> PossibleVehicleSpawns { get; set; }


    public string AssignedAgencyID { get; set; }

    [XmlIgnore]
    public Agency AssignedAgency { get; set; }

    [XmlIgnore]
    public bool IsDispatchFilled { get; set; } = false;

}

