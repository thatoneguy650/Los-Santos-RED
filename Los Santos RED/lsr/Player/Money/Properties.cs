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
    public Properties(IPropertyOwnable player, IPlacesOfInterest placesOfInterest, ITimeReportable time)
    {
        Player = player;
        PlacesOfInterest = placesOfInterest;
        Time = time;
    }
    public List<Residence> Residences { get; private set; } = new List<Residence>();
    public List<Business> Businesses { get; private set; } = new List<Business>();
    public List<GameLocation> PayoutProperties { get; private set; } = new List<GameLocation>();

    public void Setup()
    {

    }
    public void Update()
    {
        foreach (Residence residence in Residences)
        {
            if (!residence.IsOwned && residence.IsRented && residence.DateRentalPaymentDue != null && DateTime.Compare(Time.CurrentDateTime, residence.DateRentalPaymentDue) >= 0)
            {
                residence.ReRent(Player, Time);
            }
            else if(residence.IsOwned && residence.IsRentedOut && residence.DateRentalPaymentDue != null && DateTime.Compare(Time.CurrentDateTime, residence.DateRentalPaymentDue) >= 0)
            {
                residence.Payout(Player, Time);
            }
        }
        foreach(GameLocation location in PayoutProperties)
        {
            if(location.DatePayoutDue !=null && location.DatePayoutPaid != null && DateTime.Compare(Time.CurrentDateTime, location.DatePayoutDue) >=0)
            {
                location.Payout(Player, Time);
            }
        }
        foreach(Business business in Businesses)
        {
            if (business.DatePayoutDue != null && business.DatePayoutPaid != null && DateTime.Compare(Time.CurrentDateTime, business.DatePayoutDue) >= 0)
            {
                business.Payout(Player, Time);
            }
        }
    }
    public void Dispose()
    {
        Reset();
    }
    public void Reset()
    {
        foreach (Residence residence in Residences)
        {
            residence.Reset();
        }
        Residences.Clear();
    }
    public void AddResidence(Residence toAdd)
    {
        if(!Residences.Any(x=> x.Name == toAdd.Name))
        {
            Residences.Add(toAdd);
        }
    }
    public void RemoveResidence(Residence toAdd)
    {
        if (!Residences.Any(x => x.Name == toAdd.Name))
        {
            toAdd.Reset();
            Residences.Remove(toAdd);
        }
    }
    public void AddPayoutProperty(GameLocation toAdd)
    {
        if (!PayoutProperties.Any(x => x.Name == toAdd.Name && x.EntrancePosition == toAdd.EntrancePosition))
        {
            PayoutProperties.Add(toAdd);
        }
    }
    public void RemovePayoutProperty(GameLocation toRemove)
    {
        if (!PayoutProperties.Any(x => x.Name == toRemove.Name && x.EntrancePosition == toRemove.EntrancePosition))
        {
            toRemove.Reset();
            PayoutProperties.Remove(toRemove);
        }
    }

}

