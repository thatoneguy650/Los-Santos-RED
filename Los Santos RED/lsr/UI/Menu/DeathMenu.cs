using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Collections.Generic;

public class DeathMenu : Menu
{
    private GameLocation CurrentSelectedHospitalLocation;
    private UIMenu Menu;
    private UIMenuListItem HospitalRespawn;
    private UIMenuListItem TakeoverRandomPed;
    private UIMenuItem Undie;
    private IPedswappable PedSwap;
    private IPlacesOfInterest PlacesOfInterest;
    private IRespawning Respawning;
    private List<GameLocation> Hospitals;
    private List<DistanceSelect> Distances;
    public DeathMenu(MenuPool menuPool, IPedswappable pedSwap, IRespawning respawning, IPlacesOfInterest placesOfInterest)
    {
        PedSwap = pedSwap;
        Respawning = respawning;
        PlacesOfInterest = placesOfInterest;
        Menu = new UIMenu("Wasted", "Choose Respawn");
        menuPool.Add(Menu);
        Menu.OnItemSelect += OnItemSelect;
        Menu.OnListChange += OnListChange;
        CreateDeathMenu();
    }
    public float SelectedTakeoverRadius { get; set; }
    public override void Hide()
    {
        Menu.Visible = false;
    }
    public override void Show()
    {
        if(!Menu.Visible)
        {
            UpdateClosestHospitalIndex();
            Menu.Visible = true;
        }
    }
    public override void Toggle()
    {
        if (!Menu.Visible)
        {
            UpdateClosestHospitalIndex();
            Menu.Visible = true;
        }
        else
        {
            Menu.Visible = false;
        }
    }
    private void CreateDeathMenu()
    {
        Hospitals = PlacesOfInterest.GetLocations(LocationType.Hospital);
        Distances = new List<DistanceSelect> { new DistanceSelect("Closest", -1f), new DistanceSelect("20 M", 20f), new DistanceSelect("40 M", 40f), new DistanceSelect("100 M", 100f), new DistanceSelect("500 M", 500f), new DistanceSelect("Any", 1000f) };
        Undie = new UIMenuItem("Un-Die", "Respawn at this exact spot as yourself.");
        HospitalRespawn = new UIMenuListItem("Give Up", "Respawn at the nearest hospital. Lose a hospital fee and your guns.", Hospitals);
        TakeoverRandomPed = new UIMenuListItem("Takeover Pedestrian", "Takes over a random pedestrian around the player.", Distances);
        UpdateClosestHospitalIndex();
        Menu.AddItem(Undie);
        Menu.AddItem(HospitalRespawn);
        Menu.AddItem(TakeoverRandomPed);
    }
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == Undie)
        {
            Respawning.RespawnAtCurrentLocation(true, false, false);
        }
        if (selectedItem == HospitalRespawn)
        {
            if (RandomItems.RandomPercent(90))//turned off for testing
            {
                Respawning.RespawnAtHospital(CurrentSelectedHospitalLocation);
            }
            else
            {
                Respawning.RespawnAtGrave();
            }
        }
        else if (selectedItem == TakeoverRandomPed)
        {
            if (SelectedTakeoverRadius == -1f)
            {
                PedSwap.TakeoverPed(500f, true, true, true);
            }
            else
            {
                PedSwap.TakeoverPed(SelectedTakeoverRadius, false, true, true);
            }
        }
        Menu.Visible = false;
    }
    private void OnListChange(UIMenu sender, UIMenuListItem list, int index)
    {
        if (sender == Menu)
        {
            if (list == HospitalRespawn)
            {
                CurrentSelectedHospitalLocation = Hospitals[index];
                //EntryPoint.WriteToConsole($"Current Death Respawn Location {CurrentSelectedHospitalLocation.Name}");
            }
            else if (list == TakeoverRandomPed)
            {
                SelectedTakeoverRadius = Distances[index].Distance;
                //EntryPoint.WriteToConsole($"Current Death Takeover Distance {SelectedTakeoverRadius}");
            }
        }
    }
    private void UpdateClosestHospitalIndex()
    {
        HospitalRespawn.Index = Hospitals.IndexOf(PlacesOfInterest.GetClosestLocation(Game.LocalPlayer.Character.Position, LocationType.Hospital));
    }
}