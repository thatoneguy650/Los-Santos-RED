using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GunDealerRelationship : ContactRelationship
{
    public GunDealerRelationship()
    {

    }
    public GunDealerRelationship(string contactName) : base(contactName)
    {

    }
    public override void AddMoneySpent(int Amount)
    {
        TotalMoneySpent += Amount;
        int TextToSend = 0;
        bool sendGroupText = false;
        List<string> GroupReplies = new List<string>()
                {
                    $"Thanks for the business, call us up for directions to the other stores",
                    $"We have multiple stores available, give us a call to get directions to the other stores.",
                    $"Got some other stores as well, hit us up for directions.",
                };

        TextToSend = PlacesOfInterest.PossibleLocations.GunStores.Where(gs => gs.ContactName == ContactName && !gs.IsEnabled && TotalMoneySpent >= gs.MoneyToUnlock).Count();
        if(TextToSend > 1)
        {
            sendGroupText = true;
            Player.CellPhone.AddScheduledText(new GunDealerContact(ContactName), GroupReplies.PickRandom());
        }
        foreach (GunStore gs in PlacesOfInterest.PossibleLocations.GunStores)
        {
            if (gs.ContactName == ContactName && !gs.IsEnabled && TotalMoneySpent >= gs.MoneyToUnlock)
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
                Player.CellPhone.AddScheduledText(new GunDealerContact(ContactName), Replies.PickRandom());
            }
        }
        if(TotalMoneySpent >= 2000)
        {
            Player.CellPhone.AddContact(new GunDealerContact(ContactName), true);
        }
    }
    public override void SetMoneySpent(int Amount, bool sendNotification)
    {
        TotalMoneySpent = Amount;
        foreach (GunStore gs in PlacesOfInterest.PossibleLocations.GunStores)
        {
            if(gs.ContactName == ContactName)
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
                            Player.CellPhone.AddScheduledText(new GunDealerContact(ContactName), Replies.PickRandom());
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
        if (TotalMoneySpent >= 2000)
        {
            Player.CellPhone.AddContact(new GunDealerContact(ContactName), sendNotification);
        }
    }
}

