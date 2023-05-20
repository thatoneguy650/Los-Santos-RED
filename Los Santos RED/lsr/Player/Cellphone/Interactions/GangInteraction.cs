using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public class GangInteraction : IContactMenuInteraction
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
    private GangReputation ActiveGangReputation;
    private IGangs Gangs;
    private IPlacesOfInterest PlacesOfInterest;
    private UIMenuItem PayoffDebt;
    private GangContact GangContact;
    private IEntityProvideable World;
    private UIMenuItem LeaveGangMenu;
    private ISettingsProvideable Settings;
    private UIMenuItem GangJoinMenu;

    public GangInteraction(IContactInteractable player, IGangs gangs, IPlacesOfInterest placesOfInterest, GangContact gangContact, IEntityProvideable world, ISettingsProvideable settings)
    {
        Player = player;
        Gangs = gangs;
        PlacesOfInterest = placesOfInterest;
        MenuPool = new MenuPool();
        GangContact = gangContact;
        World = world;
        Settings = settings;
    }
    public void Start(PhoneContact phoneContact)
    {
        if(phoneContact == null)
        {
            EntryPoint.WriteToConsole("GangInteraction phoneContact IS NULL");
            return;
        }
        Start(Gangs.GetGangByContact(phoneContact.Name));
    }
    public void Start(Gang gang)
    {
        ActiveGang = gang;
        SetupInteraction();
        StartLoop();
    }
    private void SetupInteraction()
    {
        if (ActiveGang == null)
        {
            return;
        }
        ActiveGangReputation = Player.RelationshipManager.GangRelationships.GetReputation(ActiveGang);
        if(ActiveGangReputation == null)
        {
            return;
        }
        GangMenu = new UIMenu("", "Select an Option");
        GangMenu.RemoveBanner();
        MenuPool.Add(GangMenu);
        if(ActiveGangReputation.IsEnemy)
        {
            EnemyReply();
            return;
        }
        if (ActiveGangReputation.PlayerDebt > 0)
        {
            AddHasDebtItems();
        }
        else if (ActiveGangReputation.ReputationLevel < 0)
        {
            AddHostileRepItems();
        }
        else if (ActiveGangReputation.ReputationLevel >= ActiveGangReputation.FriendlyRepLevel)
        {
            AddFriendlyRepItems();        
        }
        else
        {
            AddNeutralRepItems();
        }
        GangMenu.Visible = true;
    }

    private void AddNeutralRepItems()
    {
        PayoffGangFriendly = new UIMenuItem("Payoff", "Payoff the gang to get a friendly relationship") { RightLabel = "~r~" + ActiveGangReputation.CostToPayoff.ToString("C0") + "~s~" };
        PayoffGangFriendly.Activated += (sender, selectedItem) =>
        {
            Player.PlayerTasks.GangTasks.StartPayoffGang(ActiveGang);
            sender.Visible = false;
        };
        GangMenu.AddItem(PayoffGangFriendly);
    }
    private void AddFriendlyRepItems()
    {
        if (Player.PlayerTasks.HasTask(ActiveGang.ContactName))
        {
            GangTaskCancel = new UIMenuItem("Cancel Task", "Tell the gang you can't complete the task.") { RightLabel = "~o~$?~s~" };
            GangTaskCancel.Activated += (sender, selectedItem) =>
            {
                Player.PlayerTasks.CancelTask(ActiveGang?.ContactName);
                sender.Visible = false;
            };
            GangMenu.AddItem(GangTaskCancel);
        }
        else
        {
            GangHit = new UIMenuItem("Hit", "Do a hit for the gang on a rival.") { RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.HitPaymentMin:C0}-{ActiveGang.HitPaymentMax:C0}~s~" };
            GangHit.Activated += (sender, selectedItem) =>
            {
                Player.PlayerTasks.GangTasks.StartGangHit(ActiveGang, 1);
                sender.Visible = false;
            };
            GangMoneyPickup = new UIMenuItem("Money Pickup", "Pickup some cash from a dead drop for the gang and bring it back.") { RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.PickupPaymentMin:C0}-{ActiveGang.PickupPaymentMax:C0}~s~" };
            GangMoneyPickup.Activated += (sender, selectedItem) =>
            {
                Player.PlayerTasks.GangTasks.StartGangPickup(ActiveGang);
                sender.Visible = false;
            };
            GangTheft = new UIMenuItem("Theft", "Steal an item for the gang. ~r~WIP~s~") { RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.TheftPaymentMin:C0}-{ActiveGang.TheftPaymentMax:C0}~s~" };
            GangTheft.Activated += (sender, selectedItem) =>
            {
                Player.PlayerTasks.GangTasks.StartGangTheft(ActiveGang);
                sender.Visible = false;
            };
            GangDelivery = new UIMenuItem("Delivery", "Source some items for the gang.") { RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.DeliveryPaymentMin:C0}-{ActiveGang.DeliveryPaymentMax:C0}~s~" };
            GangDelivery.Activated += (sender, selectedItem) =>
            {
                Player.PlayerTasks.GangTasks.StartGangDelivery(ActiveGang);
                sender.Visible = false;
            };
            GangWheelman = new UIMenuItem("Wheelman", "Be a wheelman for the gang ~r~WIP~s~") { RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.WheelmanPaymentMin:C0}-{ActiveGang.WheelmanPaymentMax:C0}~s~" };
            GangWheelman.Activated += (sender, selectedItem) =>
            {
                Player.PlayerTasks.GangTasks.StartGangWheelman(ActiveGang);
                sender.Visible = false;
            };
            GangPizza = new UIMenuItem("Pizza Man", "Pizza Time.") { RightLabel = $"~HUD_COLOUR_GREENDARK~{100:C0}-{250:C0}~s~" };
            GangPizza.Activated += (sender, selectedItem) =>
            {
                Player.PlayerTasks.GangTasks.StartGangPizza(ActiveGang);
                sender.Visible = false;
            };
            GangMenu.AddItem(GangHit);
            GangMenu.AddItem(GangMoneyPickup);
            GangMenu.AddItem(GangTheft);
            GangMenu.AddItem(GangDelivery);
            GangMenu.AddItem(GangWheelman);
            if (ActiveGang.GangClassification == GangClassification.Mafia)// == "Gambetti" || ActiveGang.ShortName == "Pavano" || ActiveGang.ShortName == "Lupisella" || ActiveGang.ShortName == "Messina" || ActiveGang.ShortName == "Ancelotti")
            {
                GangMenu.AddItem(GangPizza);
            }
            if(ActiveGangReputation.CanAskToJoin)
            {
                GangJoinMenu = new UIMenuItem("Join gang", "Prove your worth and become a member of the gang");
                GangJoinMenu.Activated += (sender, selectedItem) =>
                {
                    Player.PlayerTasks.GangTasks.StartGangProveWorth(ActiveGang, 2);
                    sender.Visible = false;
                };
                GangMenu.AddItem(GangJoinMenu);
            }
        }
        RequestGangDen = new UIMenuItem("Request Invite", "Request the location of the gang den");
        RequestGangDen.Activated += (sender, selectedItem) =>
        {
            RequestDenAddress();
            sender.Visible = false;
        };
        GangMenu.AddItem(RequestGangDen);
        if (ActiveGangReputation.IsMember)
        {
            LeaveGangMenu = new UIMenuItem("Leave Gang", "Inform the gang that you no longer want to be a member");
            LeaveGangMenu.Activated += (sender, selectedItem) =>
            {
                if (LeaveGang())
                {
                    sender.Visible = false;
                }
            };
            GangMenu.AddItem(LeaveGangMenu);
        }
    }
    private void AddHostileRepItems()
    {
        PayoffGangNeutral = new UIMenuItem("Payoff", "Payoff the gang to return to a neutral relationship") { RightLabel = "~r~" + ActiveGangReputation.CostToPayoff.ToString("C0") + "~s~" };
        PayoffGangNeutral.Activated += (sender, selectedItem) =>
        {
            Player.PlayerTasks.GangTasks.StartPayoffGang(ActiveGang);
            sender.Visible = false;
        };
        ApoligizeToGang = new UIMenuItem("Apologize", "Apologize to the gang for your actions");
        ApoligizeToGang.Activated += (sender, selectedItem) =>
        {
            ApologizeToGang();
            sender.Visible = false;
        };
        GangMenu.AddItem(PayoffGangNeutral);
        GangMenu.AddItem(ApoligizeToGang);
    }

    private void AddHasDebtItems()
    {
        PayoffDebt = new UIMenuItem("Payoff Debt", "Payoff your current debt to the gang") { RightLabel = "~r~" + ActiveGangReputation.PlayerDebt.ToString("C0") + "~s~" };
        PayoffDebt.Activated += (sender, selectedItem) =>
        {
            Player.PlayerTasks.GangTasks.StartPayoffGang(ActiveGang);
            sender.Visible = false;
        };
        GangMenu.AddItem(PayoffDebt);
    }
    private void StartLoop()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                while (MenuPool.IsAnyMenuOpen())
                {
                    GameFiber.Yield();
                }
                Player.CellPhone.Close(250);
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "CellPhone");
    }
    public void Update()
    {
        MenuPool.ProcessMenus();
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
    private void EnemyReply()
    {
        List<string> Replies = new List<string>() {
                    "Fuck off prick.",
                    "Go fuck yourself prick.",
                    "(click)",
                    };
        Player.CellPhone.AddPhoneResponse(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies.PickRandom());
    }
    private void RequestDenAddress()
    {
        GangDen myDen = PlacesOfInterest.GetMainDen(ActiveGang.ID, World.IsMPMapLoaded);
        if (myDen != null)
        {
            Player.GPSManager.AddGPSRoute(myDen.Name, myDen.EntrancePosition);
            List<string> Replies = new List<string>() {
                    $"Our {ActiveGang.DenName} is located on {myDen.FullStreetAddress} come see us.",
                    $"Come check out our {ActiveGang.DenName} on {myDen.FullStreetAddress}.",
                    $"You can find our {ActiveGang.DenName} on {myDen.FullStreetAddress}.",
                    $"{myDen.FullStreetAddress}.",
                    $"It's on {myDen.FullStreetAddress} come see us.",
                    $"The {ActiveGang.DenName}? It's on {myDen.FullStreetAddress}.",

                    };
            Player.CellPhone.AddPhoneResponse(ActiveGang.ContactName, ActiveGang.ContactIcon, Replies.PickRandom());
        }
    }
    private bool LeaveGang()
    {
        SimpleWarning popUpWarning = new SimpleWarning("Leave Gang", $"Are you sure you want to leave the gang {Player.RelationshipManager.GangRelationships.CurrentGang?.ShortName}", "", Player.ButtonPrompts, Settings);
        popUpWarning.Show();
        if (!popUpWarning.IsAccepted)
        {
            return false;
        }
        Player.RelationshipManager.GangRelationships.ResetGang(true);
        Player.RelationshipManager.GangRelationships.SetReputation(ActiveGang,ActiveGang.MinimumRep,true);

        return true;
    }
}

