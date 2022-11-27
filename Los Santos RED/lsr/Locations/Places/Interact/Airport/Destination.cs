using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class Destination
{
    public string AirportID { get; set; }
    public int Cost { get; set; }
    public Destination()
    {

    }
    public Destination(string airportID, int cost)
    {
        AirportID = airportID;
        Cost = cost;
    }
}

