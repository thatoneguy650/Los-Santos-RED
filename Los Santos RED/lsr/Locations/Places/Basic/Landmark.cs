using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Landmark : BasicLocation
{
    public Landmark(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        OpenTime = 0;
        CloseTime = 24;
    }
    public Landmark() : base()
    {

    }
    public override string TypeName { get; set; } = "Landmark";
    public override int MapIcon { get; set; } = 162;
    public override Color MapIconColor { get; set; } = Color.White;
    public override float MapIconScale { get; set; } = 0.5f;

}

