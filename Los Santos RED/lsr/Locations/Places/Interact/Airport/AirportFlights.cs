using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class AirportFlights
{
    public string ToAirportID { get; set; }
    public string Airline { get; set; }
    public string Description { get; set; }
    public int Cost { get; set; }
    public int FlightTime { get; set; }
    public AirportFlights()
    {

    }

    public AirportFlights(string airportID, string airline, string description, int cost, int flightTime)
    {
        ToAirportID = airportID;
        Airline = airline;
        Description = description;
        Cost = cost;
        FlightTime = flightTime;
    }
}

