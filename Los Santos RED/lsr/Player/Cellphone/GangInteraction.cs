using ExtensionsMethods;
using iFruitAddon2;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GangInteraction
{
    private IContactInteractable Player;
    private UIMenu GangMenu;
    private MenuPool MenuPool;
    private UIMenuItem PayoffGang;
    private UIMenuItem PayoffGangNeutral;
    private UIMenuItem ApoligizeToGang;
    private UIMenuItem RequestGangDen;
    private UIMenuItem PayoffGangFriendly;
    private UIMenu GangWorkMenu;
    private UIMenuItem GangHit;
    private UIMenuItem DeadDropPickup;
    private UIMenuItem GangTheft;
    private UIMenuItem GangTaskCancel;
    private Gang ActiveGang;
    private IGangs Gangs;
    private IPlacesOfInterest PlacesOfInterest;
    public GangInteraction(IContactInteractable player, IGangs gangs, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        Gangs = gangs;
        PlacesOfInterest = placesOfInterest;
        MenuPool = new MenuPool();
    }
    public void Start(Gang gang)
    {
        ActiveGang = gang;
        int repLevel = Player.GangRelationships.GetRepuationLevel(ActiveGang);

        GangMenu = new UIMenu("", "Select an Option");
        GangMenu.RemoveBanner();
        MenuPool.Add(GangMenu);
        GangMenu.OnItemSelect += OnGangItemSelect;

        if (repLevel < 0)
        {
            PayoffGangNeutral = new UIMenuItem("Payoff", "Payoff the gang to return to a neutral relationship") { RightLabel = "~r~" + Player.GangRelationships.CostToPayoffGang(ActiveGang).ToString("C0") + "~s~" };
            ApoligizeToGang = new UIMenuItem("Apologize", "Apologize to the gang for your actions");
            GangMenu.AddItem(PayoffGangNeutral);
            GangMenu.AddItem(ApoligizeToGang);
        }
        else if (repLevel >= 500)
        {
            if (Player.PlayerTasks.HasTask(ActiveGang.ContactName))
            {
                GangTaskCancel = new UIMenuItem("Cancel Task", "Tell the gang you can't complete the task.") { RightLabel = "~o~$?~s~" };
                GangMenu.AddItem(GangTaskCancel);
            }
            else
            {
                GangHit = new UIMenuItem("Hit", "Do a hit for the gang on a rival") { RightLabel = "~g~$10,000+~s~" };
                DeadDropPickup = new UIMenuItem("Pickup", "Pickup an item for the gang and bring it back") { RightLabel = "~g~$200-$1,000~s~" };
                GangTheft = new UIMenuItem("Theft", "Steal an item for the gang") { RightLabel = "~g~$1,000+~s~" };
                GangMenu.AddItem(GangHit);
                GangMenu.AddItem(DeadDropPickup);
                GangMenu.AddItem(GangTheft);
            }
            RequestGangDen = new UIMenuItem("Request Invite", "Request the location of the gang den");
            GangMenu.AddItem(RequestGangDen);
        }
        else
        {
            PayoffGangFriendly = new UIMenuItem("Payoff", "Payoff the gang to get a friendly relationship") { RightLabel = "~r~" + Player.GangRelationships.CostToPayoffGang(ActiveGang).ToString("C0") + "~s~" };
            GangMenu.AddItem(PayoffGangFriendly);
        }
        GangMenu.Visible = true;
        GameFiber.StartNew(delegate
        {
            while (MenuPool.IsAnyMenuOpen())
            {
                GameFiber.Yield();
            }
            Player.CellPhone.Close(250);
        }, "CellPhone");
    }
    public void Update()
    {
        MenuPool.ProcessMenus();
    }
    private void OnGangItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        //if (Player.PlayerTasks.HasTask(ActiveGang.ContactName))
        //{
        //    if (selectedItem == PayoffGangNeutral || selectedItem == GangHit || selectedItem == PayoffGangFriendly || selectedItem == DeadDropPickup || selectedItem == GangTheft)//cant do more than one of these.....
        //    {
        //        AlreadyWorkingForGang();
        //        sender.Visible = false;
        //        return;
        //    }
        //}
        if (selectedItem == PayoffGangFriendly)
        {
            Player.PlayerTasks.PayoffGangToFriendly(ActiveGang);
            sender.Visible = false;
        }
        else if (selectedItem == PayoffGangNeutral)
        {
            Player.PlayerTasks.PayoffGangToNeutral(ActiveGang);
            sender.Visible = false;
        }
        else if (selectedItem == ApoligizeToGang)
        {
            ApologizeToGang();
            sender.Visible = false;
        }
        else if (selectedItem == RequestGangDen)
        {
            RequestDenAddress();
            sender.Visible = false;
        }
        else if (selectedItem == GangTaskCancel)
        {
            Player.PlayerTasks.GangCancel(ActiveGang);
            sender.Visible = false;
        }
        else if (selectedItem == DeadDropPickup)
        {
            Player.PlayerTasks.GangPickupWork(ActiveGang);
            sender.Visible = false;
        }
        else if (selectedItem == GangTheft)
        {
            Player.PlayerTasks.GangTheftWork(ActiveGang);
            sender.Visible = false;
        }
        else if (selectedItem == GangHit)
        {
            Player.PlayerTasks.GangHitWork(ActiveGang);
            sender.Visible = false;
        }

    }
    private void ApologizeToGang()
    {
        List<string> Replies = new List<string>() {
                    "You think I give a shit?",
                    "Fuck off prick.",
                    "Go fuck yourself prick.",
                    "You are really starting to piss me off",
                    "(click)",
                    "I'm not even going to respond to this shit.",
                    "You are a stupid motherfucker aren't you?",
                    "Good luck dickhead.",
                    };
        Player.CellPhone.AddPhoneResponse(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies.PickRandom());
        //CustomiFruit.Close();
    }
    private void RequestDenAddress()
    {
        GangDen myDen = PlacesOfInterest.PossibleLocations.GangDens.FirstOrDefault(x => x.AssociatedGang?.ID == ActiveGang.ID);
        if (myDen != null)
        {
            Player.AddGPSRoute(myDen.Name, myDen.EntrancePosition);
            List<string> Replies = new List<string>() {
                    $"Our {ActiveGang.DenName} is located on {myDen.StreetAddress} come see us.",
                    $"Come check out our {ActiveGang.DenName} on {myDen.StreetAddress}.",
                    $"You can find our {ActiveGang.DenName} on {myDen.StreetAddress}.",
                    $"{myDen.StreetAddress}.",
                    $"It's on {myDen.StreetAddress} come see us.",
                    $"The {ActiveGang.DenName}? It's on {myDen.StreetAddress}.",

                    };
            Player.CellPhone.AddPhoneResponse(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies.PickRandom());
        }
    }
    private void AlreadyWorkingForGang()
    {
        List<string> Replies = new List<string>() {
                    $"Aren't you already taking care of that thing for us?",
                    $"Didn't we already give you something to do?",
                    $"Finish your task before you call us again prick.",
                    $"Get going on that thing, stop calling me",
                    $"I alredy told you what to do, stop calling me.",
                    $"You already have an item, stop with the calls.",

                    };
        Player.CellPhone.AddPhoneResponse(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies.PickRandom());
    }
}

