using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BusRoute
{
    private IPlacesOfInterest PlacesOfInterest;
    private int StopsMin = 3;
    private int StopsMax = 4;
    private int DestinationStopID = 0;
    public BusRoute(string name, IPlacesOfInterest placesOfInterest)
    {
        Name = name;
        PlacesOfInterest = placesOfInterest;
    }
    public BusRoute(string name, List<BusRouteStop> busRouteStops, IPlacesOfInterest placesOfInterest)
    {
        Name = name;
        RouteStops = busRouteStops;
        PlacesOfInterest = placesOfInterest;
    }
    public string Name { get; set; } = "Bus Route";
    public bool HasFinishedRoute { get; private set;}
    public List<BusRouteStop> RouteStops { get; set; }
    public bool HasRoute => RouteStops != null && RouteStops.Any();
    public BusRouteStop DestinationStop => RouteStops?.FirstOrDefault(x => x.StopOrder == DestinationStopID);
    public BusRouteStop NextStop => RouteStops?.FirstOrDefault(x => x.StopOrder == DestinationStopID+1);
    public void OnRouteStart()
    {
        DestinationStop.DisplayNextStopNotification();
    }
    public void OnArrivedAtStop()
    {
        DestinationStop.DisplayArrivedNotification();
        if (DestinationStopID > RouteStops.Max(x=> x.StopOrder))
        {
            DestinationStopID = 0;
            HasFinishedRoute = true;
        }
        else
        {
            DestinationStopID++;
            DestinationStop.DisplayNextStopNotification();
        }
    }
    public void AddStops()
    {
        RouteStops = new List<BusRouteStop>();
        int StopsToAdd = RandomItems.GetRandomNumberInt(StopsMin, StopsMax);
        int stopsAdded = 0;
        foreach(BusStop busStop in PlacesOfInterest.PossibleLocations.BusStops.OrderBy(x => Guid.NewGuid()).Take(StopsToAdd))//Have these come in by a config?, pick one closest to it? right now just random 
        {
            RouteStops.Add(new BusRouteStop(stopsAdded, $"{busStop.Name}", busStop));
            stopsAdded++;
        }
    }
    public void DisplayCurrentRouteInfo()
    {
        if (HasRoute && DestinationStop != null)
        {
            Game.DisplayNotification("CHAR_LS_TOURIST_BOARD", "CHAR_LS_TOURIST_BOARD", Name , "~y~Route Info", $"Stops: {RouteStops.Count()}~n~Destination #: {DestinationStopID + 1} {DestinationStop.Name}");
        }
    }
    public void DisplayDestinationInfo()
    {
        DestinationStop.DisplayNextStopNotification();
    }
}

