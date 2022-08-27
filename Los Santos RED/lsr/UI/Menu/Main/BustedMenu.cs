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
            Player.ButtonPrompts.AttemptAddPrompt("MenuShowBusted", "Toggle Busted Menu", "MenuShowBusted", Settings.SettingsManager.KeySettings.MenuKey, 999);
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
        Bribe = new UIMenuItem("Bribe Police", "Bribe the police to let you go. Don't be cheap.");
        Bribe.RightBadge = UIMenuItem.BadgeStyle.Trevor;
        Bribe.Activated += (sender, selectedItem) =>
        {
            if (int.TryParse(NativeHelper.GetKeyboardInput(""), out int BribeAmount))
            {
                Respawning.Respawning.BribePolice(BribeAmount);
            }
            Menu.Visible = false;
        };
        Menu.AddItem(Bribe);
        
    }
    private void AddRespawningOptions()
    {



        Surrender = new UIMenuListScrollerItem<ILocationRespawnable>("Surrender", "Surrender and get out on bail. Lose bail money and your guns.", PlacesOfInterest.BustedRespawnLocations().Where(x => x.IsEnabled).OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)));
        Surrender.Activated += (sender, selectedItem) =>
        {
            Respawning.Respawning.SurrenderToPolice(Surrender.SelectedItem);
            Menu.Visible = false;
        };
        Menu.AddItem(Surrender);
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