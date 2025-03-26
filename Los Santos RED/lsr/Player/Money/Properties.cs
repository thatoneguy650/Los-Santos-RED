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
        }
        foreach(Business business in Businesses)
        {
            if(business.DatePayoutDue !=null && business.DatePayoutPaid != null && DateTime.Compare(Time.CurrentDateTime, business.DatePayoutDue) >=0)
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
    public void AddBusiness(Business toAdd)
    {
        if (!Businesses.Any(x => x.Name == toAdd.Name && x.EntrancePosition == toAdd.EntrancePosition))
        {
            Businesses.Add(toAdd);
        }
    }
    public void RemoveBusiness(Business toRemove)
    {
        if (!Businesses.Any(x => x.Name == toRemove.Name && x.EntrancePosition == toRemove.EntrancePosition))
        {
            toRemove.Reset();
            Businesses.Remove(toRemove);
        }
    }


}

