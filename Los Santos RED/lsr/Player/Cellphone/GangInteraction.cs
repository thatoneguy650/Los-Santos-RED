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
    private UIMenuItem GangMoneyPickup;
    private UIMenuItem GangTheft;
    private UIMenuItem GangDelivery;
    private UIMenuItem GangWheelman;
    private UIMenuItem GangPizza;
    private UIMenuItem GangTaskCancel;
    private Gang ActiveGang;
    private IGangs Gangs;
    private IPlacesOfInterest PlacesOfInterest;
    private UIMenuItem PayoffDebt;

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
        GangReputation gr = Player.GangRelationships.GetReputation(ActiveGang);
        int repLevel = Player.GangRelationships.GetRepuationLevel(ActiveGang);

        GangMenu = new UIMenu("", "Select an Option");
        GangMenu.RemoveBanner();
        MenuPool.Add(GangMenu);
        GangMenu.OnItemSelect += OnGangItemSelect;


        if(gr != null && gr.PlayerDebt > 0)
        {
            PayoffDebt = new UIMenuItem("Payoff Debt", "Payoff your current debt to the gang") { RightLabel = "~r~" + gr.PlayerDebt.ToString("C0") + "~s~" };
            GangMenu.AddItem(PayoffDebt);
        }
        else if (repLevel < 0)
        {
            PayoffGangNeutral = new UIMenuItem("Payoff", "Payoff the gang to return to a neutral relationship") { RightLabel = "~r~" + gr.CostToPayoff.ToString("C0") + "~s~" };
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
                GangHit = new UIMenuItem("Hit", "Do a hit for the gang on a rival") { RightLabel = $"~HUD_COLOUR_GREENDARK~{gang.HitPaymentMin:C0}-{gang.HitPaymentMax:C0}~s~" };
                GangMoneyPickup = new UIMenuItem("Money Pickup", "Pickup some cash from a dead drop for the gang and bring it back") { RightLabel = $"~HUD_COLOUR_GREENDARK~{gang.PickupPaymentMin:C0}-{gang.PickupPaymentMax:C0}~s~" };
                GangTheft = new UIMenuItem("Theft", "Steal an item for the gang") { RightLabel = $"~HUD_COLOUR_GREENDARK~{gang.TheftPaymentMin:C0}-{gang.TheftPaymentMax:C0}~s~" };
                GangDelivery = new UIMenuItem("Delivery", "Source some items for the gang") { RightLabel = $"~HUD_COLOUR_GREENDARK~{gang.DeliveryPaymentMin:C0}-{gang.DeliveryPaymentMax:C0}~s~" };
                GangWheelman = new UIMenuItem("Wheelman", "Be a wheelman for the gang") { RightLabel = $"~HUD_COLOUR_GREENDARK~{gang.WheelmanPaymentMin:C0}-{gang.WheelmanPaymentMax:C0}~s~" };
                GangPizza = new UIMenuItem("Pizza Man", "Pizza Time") { RightLabel = $"~HUD_COLOUR_GREENDARK~{100:C0}-{250:C0}~s~" };

                GangMenu.AddItem(GangHit);
                GangMenu.AddItem(GangMoneyPickup);
                GangMenu.AddItem(GangTheft);
                GangMenu.AddItem(GangDelivery);
                GangMenu.AddItem(GangWheelman);


                if(gang.ShortName == "Gambetti" || gang.ShortName == "Pavano" || gang.ShortName == "Lupisella" || gang.ShortName == "Messina" || gang.ShortName == "Ancelotti")
                {
                    GangMenu.AddItem(GangPizza);
                }

            }
            RequestGangDen = new UIMenuItem("Request Invite", "Request the location of the gang den");
            GangMenu.AddItem(RequestGangDen);
        }
        else
        {
            PayoffGangFriendly = new UIMenuItem("Payoff", "Payoff the gang to get a friendly relationship") { RightLabel = "~r~" + gr.CostToPayoff.ToString("C0") + "~s~" };
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
        if (selectedItem == ApoligizeToGang)
        {
            ApologizeToGang();
            sender.Visible = false;
        }
        else if (selectedItem == RequestGangDen)
        {
            RequestDenAddress();
            sender.Visible = false;
        }
        else if (selectedItem == PayoffDebt || selectedItem == PayoffGangFriendly || selectedItem == PayoffGangNeutral)
        {
            Player.PlayerTasks.GangTasks.StartPayoffGang(ActiveGang);
            sender.Visible = false;
        }
        else if (selectedItem == GangTaskCancel)
        {
            Player.PlayerTasks.CancelTask(ActiveGang?.ContactName);
            sender.Visible = false;
        }
        else if (selectedItem == GangMoneyPickup)
        {
            Player.PlayerTasks.GangTasks.StartGangPickup(ActiveGang);
            sender.Visible = false;
        }
        else if (selectedItem == GangTheft)
        {
            Player.PlayerTasks.GangTasks.StartGangTheft(ActiveGang);
            sender.Visible = false;
        }
        else if (selectedItem == GangHit)
        {
            Player.PlayerTasks.GangTasks.StartGangHit(ActiveGang);
            sender.Visible = false;
        }
        else if (selectedItem == GangDelivery)
        {
            Player.PlayerTasks.GangTasks.StartGangDelivery(ActiveGang);
            sender.Visible = false;
        }
        else if (selectedItem == GangWheelman)
        {
            Player.PlayerTasks.GangTasks.StartGangWheelman(ActiveGang);
            sender.Visible = false;
        }
        else if (selectedItem == GangPizza)
        {
            Player.PlayerTasks.GangTasks.StartGangPizza(ActiveGang);
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
}

