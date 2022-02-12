using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Collections.Generic;

public class BustedMenu : Menu
{
    private UIMenuItem Bribe;
    private GameLocation CurrentSelectedSurrenderLocation;
    private List<DistanceSelect> Distances;
    private UIMenu Menu;
    private UIMenuItem PayFine;
    private IPedSwap PedSwap;
    private IPlacesOfInterest PlacesOfInterest;
    private IPoliceRespondable Player;
    private List<GameLocation> PoliceStations;
    private UIMenuItem ResistArrest;
    private IRespawning Respawning;
    private float SelectedTakeoverRadius;
    private ISettingsProvideable Settings;
    private UIMenuListItem Surrender;
    private UIMenuListItem TakeoverRandomPed;
    public BustedMenu(MenuPool menuPool, IPedSwap pedSwap, IRespawning respawning, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IPoliceRespondable policeRespondable)
    {
        PedSwap = pedSwap;
        Respawning = respawning;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        Player = policeRespondable;
        Menu = new UIMenu("Busted", "Choose Respawn");
        Menu.SetBannerType(System.Drawing.Color.FromArgb(181, 48, 48));
        menuPool.Add(Menu);
        Menu.OnItemSelect += OnItemSelect;
        Menu.OnListChange += OnListChange;
        CreateBustedMenu();
    }
    public override void Hide()
    {
        Menu.Visible = false;
    }
    public override void Show()
    {
        if (!Menu.Visible)
        {
            UpdateClosestPoliceStationIndex();

            if (Player.WantedLevel == 1)
            {
                PayFine.Enabled = true;
                Bribe.Enabled = false;
            }
            else
            {
                PayFine.Enabled = false;
                Bribe.Enabled = true;
            }

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
    private void CreateBustedMenu()
    {
        PoliceStations = PlacesOfInterest.GetLocations(LocationType.Police);
        Distances = new List<DistanceSelect> { new DistanceSelect("Closest", -1f), new DistanceSelect("20 M", 20f), new DistanceSelect("40 M", 40f), new DistanceSelect("100 M", 100f), new DistanceSelect("500 M", 500f), new DistanceSelect("Any", 1000f) };
        ResistArrest = new UIMenuItem("Resist Arrest", "Better hope you're strapped.");
        Bribe = new UIMenuItem("Bribe Police", "Bribe the police to let you go. Don't be cheap.");
        PayFine = new UIMenuItem("Pay Citation", $"Pay a citation of ${Settings.SettingsManager.PoliceSettings.GeneralFineAmount}.");
        Surrender = new UIMenuListItem("Surrender", "Surrender and get out on bail. Lose bail money and your guns.", PoliceStations);
        TakeoverRandomPed = new UIMenuListItem("Takeover Random Pedestrian", "Takes over a random pedestrian around the player.", Distances);
        Menu.AddItem(ResistArrest);
        Menu.AddItem(Bribe);
        Menu.AddItem(PayFine);
        Menu.AddItem(Surrender);
        Menu.AddItem(TakeoverRandomPed);
    }
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == ResistArrest)
        {
            Respawning.ResistArrest();
        }
        else if (selectedItem == Bribe)
        {
            if (int.TryParse(NativeHelper.GetKeyboardInput(""), out int BribeAmount))
            {
                if (Respawning.BribePolice(BribeAmount))
                {

                }
            }
        }
        else if (selectedItem == PayFine)
        {
            Respawning.PayFine();
        }
        else if (selectedItem == Surrender)
        {
            Respawning.SurrenderToPolice(CurrentSelectedSurrenderLocation);
        }
        else if (selectedItem == TakeoverRandomPed)
        {
            if (SelectedTakeoverRadius == -1f)
            {
                PedSwap.BecomeExistingPed(500f, true, false, true, false);
            }
            else
            {
                PedSwap.BecomeExistingPed(SelectedTakeoverRadius, false, false, true, false);
            }
        }
        Menu.Visible = false;
    }
    private void OnListChange(UIMenu sender, UIMenuListItem list, int index)
    {
        if (list == Surrender)
        {
            CurrentSelectedSurrenderLocation = PoliceStations[index];
        }
        else if (list == TakeoverRandomPed)
        {
            SelectedTakeoverRadius = Distances[index].Distance;
        }
    }
    private void UpdateClosestPoliceStationIndex()
    {
        Surrender.Index = PoliceStations.IndexOf(PlacesOfInterest.GetClosestLocation(Game.LocalPlayer.Character.Position, LocationType.Police));
    }
}