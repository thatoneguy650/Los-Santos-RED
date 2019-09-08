
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GTALocationInfo
{
    public string ID { get; set; }
    public Vector3 Location;
    public GTAGetawayCar GetawayCar;
    public int WantedLevel = 0;
    public int Level = 1;
    public bool PlayerNearby = false;

}

