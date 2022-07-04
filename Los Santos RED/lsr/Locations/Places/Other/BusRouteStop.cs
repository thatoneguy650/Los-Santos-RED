using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BusRouteStop
{
    public BusRouteStop(int stopOrder, string name, BusStop busStop)
    {
        StopOrder = stopOrder;
        Name = name;
        BusStop = busStop;
    }
    public int StopOrder { get; set; }
    public string Name { get; set; }
    public BusStop BusStop { get; set; }
    public bool HasCompletedStop { get; set; } = false;
    public void DisplayArrivedNotification()
    {
        Game.DisplayNotification("CHAR_LS_TOURIST_BOARD", "CHAR_LS_TOURIST_BOARD", Name, "~g~Arrived", $"Arriving at {BusStop.FullStreetAddress}.~n~If this is your stop, please make your way off the bus.~s~");
    }
    public void DisplayNextStopNotification()
    {
        Game.DisplayNotification("CHAR_LS_TOURIST_BOARD", "CHAR_LS_TOURIST_BOARD", Name, "~g~Next Stop", $"En Route to {BusStop.FullStreetAddress}.~n~~o~Distance: {Math.Round(BusStop.EntrancePosition.DistanceTo2D(Game.LocalPlayer.Character) * 0.000621371f, 2)} miles~s~");
    }
    public void DisplayStopNotification()
    {
        Game.DisplayNotification("CHAR_LS_TOURIST_BOARD", "CHAR_LS_TOURIST_BOARD", Name, "~y~Stop Info", $"Name: {Name}~n~Stop Number: {StopOrder+1}~n~Address: {BusStop.FullStreetAddress}~n~~o~Distance: {Math.Round(BusStop.EntrancePosition.DistanceTo2D(Game.LocalPlayer.Character) * 0.000621371f, 2)} miles~s~");
    }
    public override string ToString()
    {
        return $"Stop: {StopOrder + 1}";
    }

}

