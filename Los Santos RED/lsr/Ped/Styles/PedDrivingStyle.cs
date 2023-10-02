using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PedDrivingStyle
{
    public PedDrivingStyle()
    {
    }

    public PedDrivingStyle(string name, eCustomDrivingStyles drivingStyle, float speed)
    {
        Name = name;
        DrivingStyle = drivingStyle;
        Speed = speed;
    }

    public string Name { get; set; }
    public eCustomDrivingStyles DrivingStyle { get; set; }
    public float Speed { get; set; }
    public override string ToString()
    {
        return Name;
    }
}

