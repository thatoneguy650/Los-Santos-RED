using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Collections.Generic;
using System.Linq;

public class BustedMenu : ModUIMenu
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
        ResistArrest = new UIMenuItem("Resist Arrest", "Immediately attempt to escape from police custody. Got ten angry cops around? Better hope you're ~r~strapped~s~.");
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
        AskForCrimes = new UIMenuItem("List Offenses", "Ask the officer to list the crimes your are ~y~suspected~s~ of committing. Innocent until proven guilty!");
        AskForCrimes.Activated += (sender, selectedItem) =>
        {
            Menu.Visible = false;
            Respawning.Respawning.AskAboutCrimes();
            Show();
        };
        Menu.AddItem(AskForCrimes);

        bool hasOption = false;

        CrimeEvent highestPriorityCrimeEvent = Player.PoliceResponse.CrimesObserved.OrderBy(x => x.AssociatedCrime.Priority).FirstOrDefault();
        if (highestPriorityCrimeEvent != null && highestPriorityCrimeEvent.AssociatedCrime.CanReleaseOnCleanSearch) //Player.PoliceResponse.CrimesObserved.All(x => x.AssociatedCrime.CanReleaseOnCleanSearch))
        {
            ConsentToSearch = new UIMenuItem("Consent To Search", "Consent to a ~y~search~s~ of your person. You have nothing to hide right?");
            ConsentToSearch.RightBadge = UIMenuItem.BadgeStyle.Ammo;
            ConsentToSearch.Activated += (sender, selectedItem) =>
            {
                Menu.Visible = false;
                Respawning.Respawning.ConsentToSearchNew(this);
            };
            Menu.AddItem(ConsentToSearch);
            hasOption = true;
        }

        if (Player.PoliceResponse.CrimesObserved.All(x => x.AssociatedCrime.CanReleaseOnTalkItOut) && Respawning.Respawning.TimesTalked <= 0)
        {
            TalkItOut = new UIMenuItem("Talk It Out", $"Attempt to talk your way out of the ticket. Better be charming, others bring ~g~cash~s~.");
            TalkItOut.RightBadge = UIMenuItem.BadgeStyle.Makeup;
            TalkItOut.Activated += (sender, selectedItem) =>
            {
                Menu.Visible = false;
                Respawning.Respawning.TalkOutOfTicket(this);
            };
            Menu.AddItem(TalkItOut);
            hasOption = true;
        }

        if (highestPriorityCrimeEvent != null && highestPriorityCrimeEvent.AssociatedCrime.CanReleaseOnCite)
        {
            PayFine = new UIMenuItem("Pay Citation", $"Pay the citation to be on your way. The LSPD slush fund isn't going to grow itself is it? Help feed the ~o~beast~s~!") { RightLabel = $"{Player.FineAmount():C0}" };
            PayFine.RightBadge = UIMenuItem.BadgeStyle.Crown;
            PayFine.Activated += (sender, selectedItem) =>
            {
                Respawning.Respawning.PayFine();
                Menu.Visible = false;
            };
            Menu.AddItem(PayFine);
            hasOption = true;
        }   
        if(!hasOption)
        {
            AddSurrenderOptions();
        }
    }
    private void CreateHighLevelItems()
    {
        AddBribeItems();
        AddSurrenderOptions();
    }
    private void AddRespawningOptions()
    {
        TakeoverRandomPed = new UIMenuListScrollerItem<DistanceSelect>("Takeover Random Pedestrian", "Takes over a random pedestrian around the player. Feel the need to ruin another person's life?", Distances);
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
    private void AddBribeItems()
    {
        Respawning.Respawning.CalulateBribe();
        Bribe = new UIMenuItem("Bribe Police", "Bribe the police to let you go. Can't buy a boat on a cop's salary can you? Don't be cheap or you will regret it.");

        if (Settings.SettingsManager.RespawnSettings.ShowRequiredBribeAmount || (Settings.SettingsManager.RespawnSettings.ShowRequiredBribeAmountControllerOnly && Respawning.IsUsingController))
        {
            Bribe.RightBadge = UIMenuItem.BadgeStyle.None;
            Bribe.RightLabel = $"~r~${Respawning.Respawning.RequiredBribeAmount}~s~";
            Bribe.Activated += (sender, selectedItem) =>
            {
                Respawning.Respawning.BribePolice(Respawning.Respawning.RequiredBribeAmount, this);
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
                    Respawning.Respawning.BribePolice(BribeAmount, this);
                }
                Menu.Visible = false;
            };
        }
        Menu.AddItem(Bribe);
    }
    private void AddSurrenderOptions()//fallback, always works
    {
        if (Settings.SettingsManager.RespawnSettings.ForceBooking && 1 == 0)
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
            Surrender = new UIMenuListScrollerItem<ILocationRespawnable>("Surrender", "Surrender and get out on bail. Due to jail overcrowding, bail is granted regardless of the offense. Makes you feel safe in ~p~San Andreas~s~ does't it? Lose bail money, guns, and drugs at least.", PlacesOfInterest.BustedRespawnLocations().Where(x => x.IsEnabled && x.StateLocation == Player.CurrentLocation?.CurrentZone?.State).OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)));
            Surrender.Activated += (sender, selectedItem) =>
            {
                Respawning.Respawning.SurrenderToPolice(Surrender.SelectedItem);
                Menu.Visible = false;
            };
            Menu.AddItem(Surrender);
        }
    }
}