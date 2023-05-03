using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
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
    private bool IsDetained => Player.WantedLevel == 0;

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
        if(IsDetained)
        {
            CreateDetainItems();
        }
        else if (Player.WantedLevel <= 1)
        {
            CreateLowLevelItems();
        }
        else
        {
            CreateHighLevelItems();
        }
        AddRespawningOptions();    
    }

    private void CreateDetainItems()
    {
        AddListOffenses();
        bool hasOption = false;
        CrimeEvent highestPriorityCrimeEvent = Player.PoliceResponse.CrimesReported.OrderBy(x => x.AssociatedCrime.Priority).FirstOrDefault();
        if (highestPriorityCrimeEvent != null && highestPriorityCrimeEvent.AssociatedCrime.CanReleaseOnCleanSearch) //Player.PoliceResponse.CrimesObserved.All(x => x.AssociatedCrime.CanReleaseOnCleanSearch))
        {
            hasOption = true;
            AddConsentToSearch();
        }
        if (Player.PoliceResponse.CrimesObserved.All(x => x.AssociatedCrime.CanReleaseOnTalkItOut) && Respawning.Respawning.TimesTalked <= 0)
        {
            AddTalkItOut();
            hasOption = true;
        }
        if (highestPriorityCrimeEvent != null && highestPriorityCrimeEvent.AssociatedCrime.CanReleaseOnCite)
        {
            AddPayCitation();
            hasOption = true;
        }
        if (!hasOption)
        {
            AddSurrenderToPolice();
        }
    }
    private void AddGeneralItems()
    {
        AddResist();
    }
    private void CreateLowLevelItems()
    {
        AddListOffenses();
        bool hasOption = false;
        CrimeEvent highestPriorityCrimeEvent = Player.PoliceResponse.CrimesObserved.OrderBy(x => x.AssociatedCrime.Priority).FirstOrDefault();
        if (highestPriorityCrimeEvent != null && highestPriorityCrimeEvent.AssociatedCrime.CanReleaseOnCleanSearch) //Player.PoliceResponse.CrimesObserved.All(x => x.AssociatedCrime.CanReleaseOnCleanSearch))
        {
            AddConsentToSearch();
            hasOption = true;
        }
        if (Player.PoliceResponse.CrimesObserved.All(x => x.AssociatedCrime.CanReleaseOnTalkItOut) && Respawning.Respawning.TimesTalked <= 0)
        {
            AddTalkItOut();
            hasOption = true;
        }
        if (highestPriorityCrimeEvent != null && highestPriorityCrimeEvent.AssociatedCrime.CanReleaseOnCite)
        {
            AddPayCitation();
            hasOption = true;
        }   
        if(!hasOption)
        {
            AddSurrenderToPolice();
        }
    }
    private void CreateHighLevelItems()
    {
        AddBribeOptions();
        AddSurrenderToPolice();
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



    private void AddResist()
    {
        string resistArrestText = "Resist Arrest";
        string resistArrestDescription = "Immediately attempt to escape from police custody. Got ten angry cops around? Better hope you're ~r~strapped~s~.";
        if (IsDetained)
        {
            resistArrestText = "Resist Detainment";
            resistArrestDescription = "Immediately attempt to escape from custody. They wouldn't shoot you in the back would they?";
        }
        ResistArrest = new UIMenuItem(resistArrestText, resistArrestDescription);
        ResistArrest.RightBadge = UIMenuItem.BadgeStyle.Alert;
        ResistArrest.Activated += (sender, selectedItem) =>
        {
            Respawning.Respawning.ResistArrest();
            Menu.Visible = false;
        };
        Menu.AddItem(ResistArrest);
    }
    private void AddListOffenses()
    {
        AskForCrimes = new UIMenuItem("List Offenses", "Ask the officer to list the crimes your are ~y~suspected~s~ of committing. Innocent until proven guilty!");
        AskForCrimes.Activated += (sender, selectedItem) =>
        {
            Menu.Visible = false;
            Respawning.Respawning.AskAboutCrimes();
            Show();
        };
        Menu.AddItem(AskForCrimes);
    }
    private void AddConsentToSearch()
    {
        ConsentToSearch = new UIMenuItem("Consent To Search", "Consent to a ~y~search~s~ of your person. You have nothing to hide right?");
        ConsentToSearch.RightBadge = UIMenuItem.BadgeStyle.Ammo;
        ConsentToSearch.Activated += (sender, selectedItem) =>
        {
            Menu.Visible = false;
            Respawning.Respawning.ConsentToSearchNew(this);
        };
        Menu.AddItem(ConsentToSearch);
    }
    private void AddPayCitation()
    {
        string payCitationText = "Pay Citation";
        string payCitationDescription = $"Pay the citation to be on your way. The LSPD slush fund isn't going to grow itself is it? Help feed the ~o~beast~s~!";
        if (IsDetained)
        {
            payCitationText = "Pay Citation";
            payCitationDescription = "Pay the citation to be on your way.";
        }
        PayFine = new UIMenuItem(payCitationText, payCitationDescription) { RightLabel = $"{Player.FineAmount():C0}" };
        PayFine.RightBadge = UIMenuItem.BadgeStyle.Crown;
        PayFine.Activated += (sender, selectedItem) =>
        {
            Respawning.Respawning.PayFine();
            Menu.Visible = false;
        };
        Menu.AddItem(PayFine);
    }
    private void AddTalkItOut()
    {
        TalkItOut = new UIMenuItem("Talk It Out", $"Attempt to talk your way out of the ticket. Better be charming, others bring ~g~cash~s~.");
        TalkItOut.RightBadge = UIMenuItem.BadgeStyle.Makeup;
        TalkItOut.Activated += (sender, selectedItem) =>
        {
            Menu.Visible = false;
            Respawning.Respawning.TalkOutOfTicket(this);
        };
        Menu.AddItem(TalkItOut);
    }


    private void AddBribeOptions()
    {
        string bribeText = "Bribe Police";
        string bribeDescription = "Bribe the police to let you go. Can't buy a boat on a cop's salary can you? Don't be cheap or you will regret it.";
        if (IsDetained)
        {
            bribeText = "Bribe";
            bribeDescription = "Pay a Bribe to get out of trouble. Don't be cheap or you will regret it.";
        }
        Respawning.Respawning.CalulateBribe();
        Bribe = new UIMenuItem(bribeText, bribeDescription);

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
    private void AddSurrenderToPolice()//fallback, always works
    {
        string surrenderText = "Surrender";
        string surrenderDescription = "Surrender and get out on bail. Due to jail overcrowding, bail is granted regardless of the offense. Makes you feel safe in ~p~San Andreas~s~ does't it? Lose bail money, guns, and drugs at least.";
        if (IsDetained)
        {
            surrenderText = "Surrender";
            surrenderDescription = "Get transferred to police custody. Lose bail money, guns, and drugs at least.";
        }

        if (Settings.SettingsManager.RespawnSettings.ForceBooking && 1 == 0)
        {
            if (Player.IsBeingBooked)
            {
                Surrender = new UIMenuListScrollerItem<ILocationRespawnable>("Skip Booking", "Skip booking.", PlacesOfInterest.BustedRespawnLocations().Where(x => x.IsEnabled && x.IsSameState(Player.CurrentLocation?.CurrentZone?.GameState)).OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)).Take(1));
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
                    Respawning.Respawning.GetBooked(PlacesOfInterest.BustedRespawnLocations().Where(x => x.IsEnabled && x.IsSameState(Player.CurrentLocation?.CurrentZone?.GameState)).OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)).FirstOrDefault());
                    Menu.Visible = false;
                };
                Menu.AddItem(GetBooked);
            }
        }
        else
        {
            Surrender = new UIMenuListScrollerItem<ILocationRespawnable>(surrenderText, surrenderDescription, PlacesOfInterest.BustedRespawnLocations().Where(x => x.IsEnabled && x.IsSameState(Player.CurrentLocation?.CurrentZone?.GameState)).OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)));
            Surrender.Activated += (sender, selectedItem) =>
            {
                Respawning.Respawning.SurrenderToPolice(Surrender.SelectedItem);
                Menu.Visible = false;
            };
            Menu.AddItem(Surrender);
        }
    }


}