using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Properties
{
    private IPropertyOwnable Player;
    private IPlacesOfInterest PlacesOfInterest;
    private ITimeReportable Time;
    private IEntityProvideable World;
    public Properties(IPropertyOwnable player, IPlacesOfInterest placesOfInterest, ITimeReportable time, IEntityProvideable world)
    {
        Player = player;
        PlacesOfInterest = placesOfInterest;
        Time = time;
        World = world;
    }
    public List<GameLocation> PropertyList { get; private set; } = new List<GameLocation>();
    //public List<Residence> Residences { get; private set; } = new List<Residence>();
    //public List<Business> Businesses { get; private set; } = new List<Business>();
    //public List<GameLocation> PayoutProperties { get; private set; } = new List<GameLocation>();
    //public List<GameLocation> CraftingLocations { get; private set; } = new List<GameLocation>();
    public void Setup()
    {

    }
    public void Update()
    {
        //int businessesPayingOut = 0;
        foreach (GameLocation property in PropertyList)
        {
            property.HandleOwnedLocation(Player, Time);
        }
    }
    public void Dispose()
    {
        Reset();
    }
    public void Reset()
    {
        foreach(GameLocation property in PropertyList)
        {
            property.Reset();
        }
        PropertyList.Clear();
    }
    public void AddOwnedLocation(GameLocation toAdd)
    {
        if (!PropertyList.Any(x => x.Name == toAdd.Name && x.EntrancePosition == toAdd.EntrancePosition && x.IsCorrectMap(World.IsMPMapLoaded)))
        {
            PropertyList.Add(toAdd);
        }
    }
    public void RemoveOwnedLocation(GameLocation toRemove)
    {
        if (!PropertyList.Any(x => x.Name == toRemove.Name && x.EntrancePosition == toRemove.EntrancePosition && x.IsCorrectMap(World.IsMPMapLoaded)))
        {
            PropertyList.Remove(toRemove);
        }
    }
}

