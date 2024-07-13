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
    private UIMenuListScrollerItem<PossibleBribe> Bribe;
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
    private string BribeDescription
    {
        get
        {
            string bribedesc = "Bribe the police to let you go. Can't buy a boat on a cop's salary can you? Don't be cheap or you will regret it.";
            if (IsDetained)
            {
                bribedesc = "Pay a Bribe to get out of trouble. Don't be cheap or you will regret it.";
            }
            return bribedesc;
        }
    }
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
        if (!Player.IsArrested)
        {
            AddGeneralItems();
        }
        if(Player.IsArrested || Player.IsBeingBooked)
        {
            CreateArrestedItems();
        }
        else if(IsDetained)
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
    private void CreateArrestedItems()
    {
        AddSurrenderToPolice();
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
            Respawning.Respawning.ConsentToSearch(this);
        };
        Menu.AddItem(ConsentToSearch);
    }
    private void AddPayCitation()
    {
        string payCitationText = "Pay Citation";

        string agencyName = "LSPD";
        Agency agency = Player.CurrentLocation.CurrentZone?.AssignedLEAgency;
        if (agency != null)
        {
            agencyName = agency.ShortName;
        }

        string payCitationDescription = $"Pay the citation to be on your way. The {agencyName} slush fund isn't going to grow itself is it? Help feed the ~o~beast~s~!";
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
        if (IsDetained)
        {
            bribeText = "Bribe";
        }
        Respawning.Respawning.CalculateBribe();
        List<PossibleBribe> possibleBribes = new List<PossibleBribe>() 
        { 
            new PossibleBribe(Respawning.Respawning.RequiredBribeAmount,100f),
        };
        if(Respawning.Respawning.RequiredBribeAmount >= 500)
        {
            for (int i = 9; i > 2; i--)
            {
                int bribeAmount = (int)Math.Floor(Respawning.Respawning.RequiredBribeAmount * (0.1f * (float)i));
                float bribePercentage = (float)i * 10f;
                if(bribeAmount <= 500)
                {
                    break;
                }
                possibleBribes.Add(new PossibleBribe(bribeAmount, bribePercentage));
            }
        }
        Bribe = new UIMenuListScrollerItem<PossibleBribe>(bribeText, BribeDescription, possibleBribes.OrderByDescending(x=> x.Percentage));
        Bribe.Activated += (sender, selectedItem) =>
        {
            if (Respawning.Respawning.BribePolice(this, Bribe.SelectedItem))
            {
                Menu.Visible = false;
            }
            else
            {

            }
        };
        Menu.AddItem(Bribe);
    }
    private void AddSurrenderToPolice()//fallback, always works
    {
        string surrenderText = "Surrender";
        string currentState = Player.CurrentLocation.CurrentZone?.GameState?.StateName;
        if(string.IsNullOrEmpty(currentState))
        {
            currentState = "San Andreas";
        }
        string surrenderDescription = $"Surrender and get out on bail. Due to jail overcrowding, bail is granted regardless of the offense. Makes you feel safe in ~p~{currentState}~s~ does't it? Lose bail money, guns, and drugs.";
        if (IsDetained)
        {
            surrenderText = "Surrender";
            surrenderDescription = "Get transferred to police custody. Lose bail money, guns, and drugs.";
        }
        Respawning.Respawning.CalculateBailDurationAndFees();
        surrenderDescription += $"~n~~n~Bail Fee: ~r~${Respawning.Respawning.BailFee}~s~";
        if(Respawning.Respawning.BailFeePastDue > 0)
        {
            surrenderDescription += $" (~r~${Respawning.Respawning.BailFeePastDue} past due~s~)";
        }
        surrenderDescription += $"~n~Bail Length: ~y~{Respawning.Respawning.BailDuration}~s~ days";
        if (Settings.SettingsManager.RespawnSettings.AllowBookingSurrender)
        {
            GetBooked = new UIMenuItem("Full Surrender", $"Go through the full process of getting booked ~r~WIP~s~. {surrenderDescription}");
            GetBooked.Activated += (sender, selectedItem) =>
            {
                Respawning.Respawning.GetBooked(PlacesOfInterest.BustedRespawnLocations().Where(x => x.IsEnabled && x.IsSameState(Player.CurrentLocation?.CurrentZone?.GameState)).OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)).FirstOrDefault());
                Menu.Visible = false;
            };
            Menu.AddItem(GetBooked);
        }
        Surrender = new UIMenuListScrollerItem<ILocationRespawnable>(surrenderText, surrenderDescription, PlacesOfInterest.BustedRespawnLocations().Where(x => x.IsEnabled && x.IsSameState(Player.CurrentLocation?.CurrentZone?.GameState)).OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)));
        Surrender.Activated += (sender, selectedItem) =>
        {
            Respawning.Respawning.SurrenderToPolice(Surrender.SelectedItem);
            Menu.Visible = false;
        };
        Menu.AddItem(Surrender);   
    }
}