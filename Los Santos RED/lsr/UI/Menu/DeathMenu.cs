using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Collections.Generic;
using System.Linq;

public class DeathMenu : Menu
{
    private Hospital CurrentSelectedHospitalLocation;
    private UIMenu Menu;
    private UIMenuListItem HospitalRespawn;
    private UIMenuListItem TakeoverRandomPed;
    private UIMenuItem Undie;
    private IPedSwap PedSwap;
    private IPlacesOfInterest PlacesOfInterest;
    private IRespawning Respawning;
    private List<Hospital> Hospitals;
    private List<DistanceSelect> Distances;
    private ISettingsProvideable Settings;
    private IRespawnable Player;
    private IGameSaves GameSaves;
    public DeathMenu(MenuPool menuPool, IPedSwap pedSwap, IRespawning respawning, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IRespawnable player, IGameSaves gameSaves)
    {
        PedSwap = pedSwap;
        Respawning = respawning;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        Player = player;
        GameSaves = gameSaves;
        Menu = new UIMenu("Wasted", "Choose Respawn");
        Menu.SetBannerType(System.Drawing.Color.FromArgb(181, 48, 48));
        menuPool.Add(Menu);
        Menu.OnItemSelect += OnItemSelect;
        Menu.OnListChange += OnListChange;
        CreateDeathMenu();
    }
    public float SelectedTakeoverRadius { get; set; } = -1f;
    public override void Hide()
    {
        Menu.Visible = false;
    }
    public override void Show()
    {
        if(!Menu.Visible)
        {
            UpdateClosestHospitalIndex();
            if(Settings.SettingsManager.RespawnSettings.PermanentDeathMode)
            {
                Undie.Enabled = false;
                HospitalRespawn.Enabled = false;
            }
            else
            {
                HospitalRespawn.Enabled = true;
            }
            if(Settings.SettingsManager.RespawnSettings.AllowUndie && Player.CanUndie && !Settings.SettingsManager.RespawnSettings.PermanentDeathMode)
            {
                Undie.Enabled = true;
            }
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
        Hospitals = PlacesOfInterest.PossibleLocations.Hospitals;//PlacesOfInterest.GetLocations(LocationType.Hospital);
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
            Respawning.RespawnAtCurrentLocation(true, false, false, false);
        }
        if (selectedItem == HospitalRespawn)
        {
            Respawning.RespawnAtHospital(CurrentSelectedHospitalLocation);
        }
        else if (selectedItem == TakeoverRandomPed)
        {
            if (SelectedTakeoverRadius == -1f)
            {
                PedSwap.BecomeExistingPed(500f, true, false, true, true);
            }
            else
            {
                PedSwap.BecomeExistingPed(SelectedTakeoverRadius, false, false, true, true);
            }
            if (Settings.SettingsManager.RespawnSettings.PermanentDeathMode)//shouldnt be here!
            {
                GameSaves.DeleteSave(Player.PlayerName, Player.ModelName);
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
        HospitalRespawn.Index = Hospitals.IndexOf(PlacesOfInterest.PossibleLocations.Hospitals.OrderBy(x => Game.LocalPlayer.Character.Position.DistanceTo2D(x.EntrancePosition)).FirstOrDefault());
    }
}