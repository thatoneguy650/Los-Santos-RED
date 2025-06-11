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
        //foreach (Residence residence in Residences)
        //{
        //    if (!residence.IsOwned && residence.IsRented && residence.DateRentalPaymentDue != null && DateTime.Compare(Time.CurrentDateTime, residence.DateRentalPaymentDue) >= 0)
        //    {
        //        residence.ReRent(Player, Time);
        //    }
        //    else if(residence.IsOwned && residence.IsRentedOut && residence.DateRentalPaymentDue != null && DateTime.Compare(Time.CurrentDateTime, residence.DateRentalPaymentDue) >= 0)
        //    {
        //        residence.Payout(Player, Time);
        //        businessesPayingOut++;
        //    }
        //}
        //foreach(GameLocation location in PayoutProperties)
        //{
        //    if(location.DatePayoutDue !=null && location.DatePayoutPaid != null && DateTime.Compare(Time.CurrentDateTime, location.DatePayoutDue) >=0)
        //    {
        //        location.Payout(Player, Time);
        //        businessesPayingOut++;
        //    }
        //}
        //foreach(Business business in Businesses)
        //{
        //    if (business.DatePayoutDue != null && business.DatePayoutPaid != null && DateTime.Compare(Time.CurrentDateTime, business.DatePayoutDue) >= 0)
        //    {
        //        business.Payout(Player, Time);
        //        businessesPayingOut++;
        //    }
        //}
        //if(businessesPayingOut > 0)
        //{
        //    Game.DisplayNotification($"{businessesPayingOut} of your investment(s) have paid out.");
        //}
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
        //foreach (Residence residence in Residences)
        //{
        //    residence.Reset();
        //}
        //Residences.Clear();
    }
    public void AddOwnedLocation(GameLocation toAdd)
    {
        if (!PropertyList.Any(x => x.Name == toAdd.Name && x.EntrancePosition == toAdd.EntrancePosition))
        {
            PropertyList.Add(toAdd);
        }
    }
    public void RemoveOwnedLocation(GameLocation toRemove)
    {
        if (!PropertyList.Any(x => x.Name == toRemove.Name && x.EntrancePosition == toRemove.EntrancePosition))
        {
            PropertyList.Remove(toRemove);
        }
    }
}

