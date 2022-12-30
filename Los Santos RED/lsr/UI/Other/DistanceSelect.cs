using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DistanceSelect
{
    public DistanceSelect(string display, float distance)
    {
        Display = display;
        Distance = distance;
    }

    public string Display { get; set; }
    public float Distance { get; set; }
    public override string ToString()
    {
        return Display.ToString();
    }
}

