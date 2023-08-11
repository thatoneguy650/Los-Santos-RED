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
    private int reputationLevel = 200;
    public readonly int DefaultRepAmount = 200;
    public readonly int RepMaximum = 2000;
    public readonly int RepMinimum = -2000;
    public int TotalMoneySpentAtShops { get; private set; } = 0;
    public int PlayerDebt { get; private set; } = 0;
    public int ReputationLevel => reputationLevel;
    public GunDealerRelationship(IGunDealerRelateable player, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        PlacesOfInterest = placesOfInterest;
    }
    public void Setup()
    {

    }
    public void Dispose()
    {
        Reset(false);
    }
    public void Reset(bool sendText)
    {
        SetReputation(DefaultRepAmount, sendText);
        PlayerDebt = 0;
    }
    public void SetReputation(int value, bool sendText)
    {
        if (reputationLevel != value)
        {
            if (value > RepMaximum)
            {
                reputationLevel = RepMaximum;
            }
            else if (value < RepMinimum)
            {
                reputationLevel = RepMinimum;
            }
            else
            {
                reputationLevel = value;
            }
            OnReputationChanged(sendText);
        }
    }
    public void ChangeReputation(int Amount, bool sendText)
    {
        SetReputation(ReputationLevel + Amount, sendText);
    }
    private void OnReputationChanged(bool sendText)
    {

    }
    public void AddDebt(int Amount)
    {
        PlayerDebt += Amount;
    }
    public void SetDebt(int Amount)
    {
        PlayerDebt = Amount;
    }
    public void AddMoneySpent(int Amount)
    {
        TotalMoneySpentAtShops += Amount;

        int TextToSend = 0;
        bool sendGroupText = false;

        List<string> GroupReplies = new List<string>()
                {
                    $"Thanks for the business, call us up for directions to the other stores",
                    $"We have multiple stores available, give us a call to get directions to the other stores.",
                    $"Got some other stores as well, hit us up for directions.",
                };

        TextToSend = PlacesOfInterest.PossibleLocations.GunStores.Where(gs => gs.ContactName == StaticStrings.UndergroundGunsContactName && !gs.IsEnabled && TotalMoneySpentAtShops >= gs.MoneyToUnlock).Count();
        if(TextToSend > 1)
        {
            sendGroupText = true;
            Player.CellPhone.AddScheduledText(new GunDealerContact(StaticStrings.UndergroundGunsContactName), GroupReplies.PickRandom());
        }
        foreach (GunStore gs in PlacesOfInterest.PossibleLocations.GunStores)
        {
            if (gs.ContactName == StaticStrings.UndergroundGunsContactName && !gs.IsEnabled && TotalMoneySpentAtShops >= gs.MoneyToUnlock)
            {
                gs.IsEnabled = true;
                if(sendGroupText)
                {
                    continue;
                }
                List<string> Replies = new List<string>()
                {
                    $"Thanks for the business, come check out our other store on {gs.FullStreetAddress}",
                    $"Come check out our other location on {gs.FullStreetAddress}",
                    $"We have some other items available at our store on {gs.FullStreetAddress}",
                    $"You seem legit, come check out our store located on {gs.FullStreetAddress}",
                    $"Come do some business at the shop on {gs.FullStreetAddress}",
                    $"Got some things you might be interested in, its on {gs.FullStreetAddress}",
                    $"{gs.FullStreetAddress}, keep it on the down-low",
                    $"{gs.FullStreetAddress}",
                    $"Need some extra hardware? {gs.FullStreetAddress}",
                    $"Got some other things at the shop on {gs.FullStreetAddress}",
                };
                Player.CellPhone.AddScheduledText(new GunDealerContact(StaticStrings.UndergroundGunsContactName), Replies.PickRandom());
                //EntryPoint.WriteToConsoleTestLong($"{gs.Name} is now enabled");
            }
        }
        if(TotalMoneySpentAtShops >= 2000)
        {
            Player.CellPhone.AddContact(new GunDealerContact(StaticStrings.UndergroundGunsContactName), true);
        }
        //EntryPoint.WriteToConsoleTestLong($"You spent {Amount} for a total of {TotalMoneySpentAtShops}");
    }
    public void SetMoneySpent(int Amount, bool sendNotification)
    {
        TotalMoneySpentAtShops = Amount;
        foreach (GunStore gs in PlacesOfInterest.PossibleLocations.GunStores)
        {
            if(gs.ContactName == StaticStrings.UndergroundGunsContactName)
            {
                if(TotalMoneySpentAtShops >= gs.MoneyToUnlock)
                {
                    if (!gs.IsEnabled)
                    {
                        gs.IsEnabled = true;
                        if (sendNotification)
                        {
                            List<string> Replies = new List<string>()
                            {
                                $"Thanks for the business, come check out our other store on {gs.FullStreetAddress}",
                                $"Come check out our other location on {gs.FullStreetAddress}",
                                $"We have some other items available at our store on {gs.FullStreetAddress}",
                                $"You seem legit, come check out our store located on {gs.FullStreetAddress}",
                                $"Come do some business at the shop on {gs.FullStreetAddress}",
                                $"Got some things you might be interested in, its on {gs.FullStreetAddress}",
                                $"{gs.FullStreetAddress}, keep it on the down-low",
                                $"{gs.FullStreetAddress}",
                                $"Need some extra hardware? {gs.FullStreetAddress}",
                                $"Got some other things at the shop on {gs.FullStreetAddress}",
                            };

                            Player.CellPhone.AddScheduledText(new GunDealerContact(StaticStrings.UndergroundGunsContactName), Replies.PickRandom());
                        }
                        //EntryPoint.WriteToConsoleTestLong($"{gs.Name} is now enabled");
                    }
                }
                else
                {
                    gs.IsEnabled = false;
                }
            }
        }
        if (TotalMoneySpentAtShops >= 2000)
        {
            Player.CellPhone.AddContact(new GunDealerContact(StaticStrings.UndergroundGunsContactName), sendNotification);
            //Player.CellPhone.AddGunDealerContact(sendNotification);
        }
    }
}

