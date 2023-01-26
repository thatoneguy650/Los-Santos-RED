using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Collections.Generic;
using System.Linq;

public class BustedMenu : Menu
{
    private MenuPool MenuPool;
    private UIMenuItem Bribe;
    private List<DistanceSelect> Distances;
    private UIMenu Menu;
    private UIMenuItem PayFine;
    private IPedSwap PedSwap;
    private IPlacesOfInterest PlacesOfInterest;
    private IPoliceRespondable Player;
    private UIMenuItem ResistArrest;
    private IRespawning Respawning;
    private ISettingsProvideable Settings;
    private UIMenuListScrollerItem<ILocationRespawnable> Surrender;
    private UIMenuListScrollerItem<DistanceSelect> TakeoverRandomPed;
    private ITimeReportable Time;
    private UIMenuItem TalkItOut;
    private UIMenuItem GetBooked;
    private UIMenuItem AskForCrimes;
    private UIMenuItem ConsentToSearch;

    public BustedMenu(MenuPool menuPool, IPedSwap pedSwap, IRespawning respawning, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IPoliceRespondable policeRespondable, ITimeReportable time)
    {
        PedSwap = pedSwap;
        Respawning = respawning;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        Player = policeRespondable;
        Time = time;
        MenuPool = menuPool;
    }
    public void Setup()
    {
        Menu = new UIMenu("Busted", "Choose Respawn");
        Menu.SetBannerType(EntryPoint.LSRedColor);
        MenuPool.Add(Menu);
        Distances = new List<DistanceSelect> { new DistanceSelect("Closest", -1f), new DistanceSelect("20 M", 20f), new DistanceSelect("40 M", 40f), new DistanceSelect("100 M", 100f), new DistanceSelect("500 M", 500f), new DistanceSelect("Any", 1000f) };     
    }
    public override void Hide()
    {
        Menu.Visible = false;
    }
    public override void Show()
    {
        if (!Menu.Visible)
        {
            Create();
            Player.ButtonPrompts.RemovePrompts("MenuShowDead");
            Player.ButtonPrompts.RemovePrompts("MenuShowBusted");
            Menu.Visible = true;
        }
    }
    public override void Toggle()
    {
        if (!Menu.Visible)
        {
            Show();
        }
        else
        {
            Menu.Visible = false;
        }
    }
    private void Create()
    {
        Menu.Clear();
        AddGeneralItems();
        if (Player.WantedLevel <= 1)
        {
            CreateLowLevelItems();
        }
        else
        {
            CreateHighLevelItems();
        }
        AddRespawningOptions();    
    }
    private void AddGeneralItems()
    {
        ResistArrest = new UIMenuItem("Resist Arrest", "Better hope you're strapped.");
        ResistArrest.RightBadge = UIMenuItem.BadgeStyle.Alert;
        ResistArrest.Activated += (sender, selectedItem) =>
        {
            Respawning.Respawning.ResistArrest();
            Menu.Visible = false;
        };
        Menu.AddItem(ResistArrest);
    }
    private void CreateLowLevelItems()
    {

        AskForCrimes = new UIMenuItem("Ask About Crimes", "Ask the officer what crimes you are suspected of committing.");
        AskForCrimes.RightBadge = UIMenuItem.BadgeStyle.Alert;
        AskForCrimes.Activated += (sender, selectedItem) =>
        {
            Menu.Visible = false;
            Respawning.Respawning.AskAboutCrimes();
            Show();
        };
        Menu.AddItem(AskForCrimes);

        if (Player.PoliceResponse.CrimesObserved.All(x => x.AssociatedCrime.RequiresSearch))
        {
            ConsentToSearch = new UIMenuItem("Consent To Search", "Consent to a search of your person. You have nothing to hide right?");
            ConsentToSearch.RightBadge = UIMenuItem.BadgeStyle.Alert;
            ConsentToSearch.Activated += (sender, selectedItem) =>
            {
                Menu.Visible = false;
                if(!Respawning.Respawning.ConsentToSearch())
                {
                    Show();
                }
            };
            Menu.AddItem(ConsentToSearch);
        }
        TalkItOut = new UIMenuItem("Talk It Out", $"Attempt to talk your way out of the ticket.");
        TalkItOut.RightBadge = UIMenuItem.BadgeStyle.Makeup;
        TalkItOut.Activated += (sender, selectedItem) =>
        {
            Menu.Visible = false;
            Respawning.Respawning.TalkOutOfTicket();
        };
        if (Respawning.Respawning.TimesTalked <= 0)
        {
            Menu.AddItem(TalkItOut);
        }
        PayFine = new UIMenuItem("Pay Citation", $"Pay the citation to be on your way.") { RightLabel = $"{Player.FineAmount():C0}" };
        PayFine.Activated += (sender, selectedItem) =>
        {
            Respawning.Respawning.PayFine();
            Menu.Visible = false;
        };
        Menu.AddItem(PayFine);


    }
    private void CreateHighLevelItems()
    {
        Respawning.Respawning.CalulateBribe();
        Bribe = new UIMenuItem("Bribe Police", "Bribe the police to let you go. Don't be cheap.");

        if (Settings.SettingsManager.RespawnSettings.ShowRequiredBribeAmount ||  (Settings.SettingsManager.RespawnSettings.ShowRequiredBribeAmountControllerOnly && Respawning.IsUsingController))
        {
            Bribe.RightBadge = UIMenuItem.BadgeStyle.None;
            Bribe.RightLabel = $"~r~${Respawning.Respawning.RequiredBribeAmount}~s~" ;
            Bribe.Activated += (sender, selectedItem) =>
            {
                Respawning.Respawning.BribePolice(Respawning.Respawning.RequiredBribeAmount);
                Menu.Visible = false;
            };
        }
        else
        {
            Bribe.RightBadge = UIMenuItem.BadgeStyle.Trevor;
            Bribe.RightLabel = "";
            Bribe.Activated += (sender, selectedItem) =>
            {
                if (int.TryParse(NativeHelper.GetKeyboardInput(""), out int BribeAmount))
                {
                    Respawning.Respawning.BribePolice(BribeAmount);
                }
                Menu.Visible = false;
            };
        }
        Menu.AddItem(Bribe);


        if (Settings.SettingsManager.RespawnSettings.ForceBooking && 1==0)
        {
            if (Player.IsBeingBooked)
            {
                Surrender = new UIMenuListScrollerItem<ILocationRespawnable>("Skip Booking", "Skip booking.", PlacesOfInterest.BustedRespawnLocations().Where(x => x.IsEnabled && x.StateLocation == Player.CurrentLocation?.CurrentZone?.State).OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)).Take(1));
                Surrender.Activated += (sender, selectedItem) =>
                {
                    Respawning.Respawning.SurrenderToPolice(Surrender.SelectedItem);
                    Menu.Visible = false;
                };
                Menu.AddItem(Surrender);
            }
            else
            {
                GetBooked = new UIMenuItem("Get Booked", "Get Booked. Lose bail money and your guns.");
                GetBooked.Activated += (sender, selectedItem) =>
                {
                    Respawning.Respawning.GetBooked(PlacesOfInterest.BustedRespawnLocations().Where(x => x.IsEnabled && x.StateLocation == Player.CurrentLocation?.CurrentZone?.State).OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)).FirstOrDefault());
                    Menu.Visible = false;
                };
                Menu.AddItem(GetBooked);
            }
        }
        else
        {
            Surrender = new UIMenuListScrollerItem<ILocationRespawnable>("Surrender", "Surrender and get out on bail. Lose bail money and your guns.", PlacesOfInterest.BustedRespawnLocations().Where(x => x.IsEnabled && x.StateLocation == Player.CurrentLocation?.CurrentZone?.State).OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)));
            Surrender.Activated += (sender, selectedItem) =>
            {
                Respawning.Respawning.SurrenderToPolice(Surrender.SelectedItem);
                Menu.Visible = false;
            };
            Menu.AddItem(Surrender);
        }

    }
    private void AddRespawningOptions()
    {
        TakeoverRandomPed = new UIMenuListScrollerItem<DistanceSelect>("Takeover Random Pedestrian", "Takes over a random pedestrian around the player.", Distances);
        TakeoverRandomPed.Activated += (sender, selectedItem) =>
        {
            if (TakeoverRandomPed.SelectedItem.Distance == -1f)
            {
                PedSwap.BecomeExistingPed(500f, true, false, true, false);
            }
            else
            {
                PedSwap.BecomeExistingPed(TakeoverRandomPed.SelectedItem.Distance, false, false, true, false);
            }
            Menu.Visible = false;
        };
        Menu.AddItem(TakeoverRandomPed);
    }





}