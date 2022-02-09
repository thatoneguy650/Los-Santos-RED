using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GunDealerRelationship
{
    private IGunDealerRelateable Player;
    private IPlacesOfInterest PlacesOfInterest;
    public int TotalMoneySpent { get; private set; } = 0;
    public GunDealerRelationship(IGunDealerRelateable player, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        PlacesOfInterest = placesOfInterest;
    }
    public void AddMoneySpent(int Amount)
    {
        TotalMoneySpent += Amount;
        foreach (GunStore gs in PlacesOfInterest.PossibleLocations.GunStores)
        {
            if (gs.IsIllegalShop && !gs.IsEnabled && TotalMoneySpent >= gs.MoneyToUnlock)
            {
                gs.IsEnabled = true;

                List<string> Replies = new List<string>()
                {
                    $"Thanks for the business, come check out our other store on {gs.StreetAddress}",
                    $"Come check out our other location on {gs.StreetAddress}",
                    $"We have some other items available at our store on {gs.StreetAddress}",
                    $"You seem legit, come check out our store located on {gs.StreetAddress}",
                    $"Come do some business at the shop on {gs.StreetAddress}",
                    $"Got some things you might be interested in, its on {gs.StreetAddress}",
                    $"{gs.StreetAddress}, keep it on the down-low",
                    $"{gs.StreetAddress}",
                    $"Need some extra hardware? {gs.StreetAddress}",
                    $"Got some other things at the shop on {gs.StreetAddress}",
                };

                Player.CellPhone.AddScheduledText("Underground Guns", "CHAR_BLANK_ENTRY", Replies.PickRandom());
                EntryPoint.WriteToConsole($"{gs.Name} is now enabled");
            }
        }
        if(TotalMoneySpent >= 2000)
        {
            Player.CellPhone.AddGunDealerContact(true);
        }
        EntryPoint.WriteToConsole($"You spent {Amount} for a total of {TotalMoneySpent}");

    }
    public void SetMoneySpent(int Amount, bool sendNotification)
    {
        TotalMoneySpent = Amount;
        foreach (GunStore gs in PlacesOfInterest.PossibleLocations.GunStores)
        {
            if(gs.IsIllegalShop)
            {
                if(TotalMoneySpent >= gs.MoneyToUnlock)
                {
                    if (!gs.IsEnabled)
                    {
                        gs.IsEnabled = true;
                        if (sendNotification)
                        {
                            List<string> Replies = new List<string>()
                            {
                                $"Thanks for the business, come check out our other store on {gs.StreetAddress}",
                                $"Come check out our other location on {gs.StreetAddress}",
                                $"We have some other items available at our store on {gs.StreetAddress}",
                                $"You seem legit, come check out our store located on {gs.StreetAddress}",
                                $"Come do some business at the shop on {gs.StreetAddress}",
                                $"Got some things you might be interested in, its on {gs.StreetAddress}",
                                $"{gs.StreetAddress}, keep it on the down-low",
                                $"{gs.StreetAddress}",
                                $"Need some extra hardware? {gs.StreetAddress}",
                                $"Got some other things at the shop on {gs.StreetAddress}",
                            };

                            Player.CellPhone.AddScheduledText("Underground Guns", "CHAR_BLANK_ENTRY", Replies.PickRandom());
                        }
                        EntryPoint.WriteToConsole($"{gs.Name} is now enabled");
                    }
                }
                else
                {
                    gs.IsEnabled = false;
                }
            }
        }
        if (TotalMoneySpent >= 2000)
        {
            Player.CellPhone.AddGunDealerContact(sendNotification);
        }
    }
}

