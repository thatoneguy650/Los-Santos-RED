using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Collections.Generic;
using System.Linq;

public class DeathMenu : ModUIMenu
{
    private UIMenu Menu;
    private IPedSwap PedSwap;
    private IPlacesOfInterest PlacesOfInterest;
    private IRespawning PlayerRespawning;//this is confusing
    private List<DistanceSelect> Distances;
    private ISettingsProvideable Settings;
    private IRespawnable Player;
    private IGameSaves GameSaves;
    private MenuPool MenuPool;
    public DeathMenu(MenuPool menuPool, IPedSwap pedSwap, IRespawning respawning, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, IRespawnable player, IGameSaves gameSaves)
    {
        MenuPool = menuPool;
        PedSwap = pedSwap;
        PlayerRespawning = respawning;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        Player = player;
        GameSaves = gameSaves;
    }
    public void Setup()
    {

        string description = "Choose Respawn";
        Distances = new List<DistanceSelect>();

        Menu = new UIMenu("Wasted", description);
        Menu.SetBannerType(System.Drawing.Color.FromArgb(181, 48, 48));
        MenuPool.Add(Menu);

    }
    public override void Hide()
    {
        Menu.Visible = false;
    }
    public override void Show()
    {
        if(!Menu.Visible)
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
        CreateUndieItem();
        CreateHospitalRespawnItem();
        CreateTakeoverItem();
    }
    private void CreateUndieItem()
    {
        UIMenuItem Undie = new UIMenuItem("Un-Die", "Respawn at this exact spot as yourself.");
        Undie.Activated += (sender, selectedItem) =>
        {
            PlayerRespawning.Respawning.RespawnAtCurrentLocation(true, false, false, false);
            Menu.Visible = false;
        };
        if (Settings.SettingsManager.RespawnSettings.AllowUndie && Player.Respawning.CanUndie && !Settings.SettingsManager.RespawnSettings.PermanentDeathMode)
        {
            Menu.AddItem(Undie);
        }
    }
    private void CreateHospitalRespawnItem()
    {
        string hosptalRespawnText = "Give Up";
        string hospitalrespawnDescription = "Respawn at the nearest hospital. Lose a hospital fee, ~r~cash on hand~s~, guns, and drugs.";
        PlayerRespawning.Respawning.CalculateHospitalStay();
        hospitalrespawnDescription += $"~n~~n~Hospital Fee: ~r~${PlayerRespawning.Respawning.HospitalFee}~s~";
        if (PlayerRespawning.Respawning.HospitalBillPastDue > 0)
        {
            hospitalrespawnDescription += $" (~r~${PlayerRespawning.Respawning.HospitalBillPastDue} past due~s~)";
        }
        hospitalrespawnDescription += $"~n~Hosptial Stay Length: ~y~{PlayerRespawning.Respawning.HospitalDuration}~s~ days";
        UIMenuListScrollerItem<Hospital> HospitalRespawn = new UIMenuListScrollerItem<Hospital>(hosptalRespawnText, hospitalrespawnDescription, PlacesOfInterest.PossibleLocations.Hospitals.Where(x => x.IsEnabled && x.IsSameState(Player.CurrentLocation?.CurrentZone?.GameState)).OrderBy(x => x.EntrancePosition.DistanceTo2D(Player.Character)));
        HospitalRespawn.Activated += (sender, selectedItem) =>
        {
            PlayerRespawning.Respawning.RespawnAtHospital(HospitalRespawn.SelectedItem);
            Menu.Visible = false;
        };
        if (!Settings.SettingsManager.RespawnSettings.PermanentDeathMode)
        {
            Menu.AddItem(HospitalRespawn);
        }
    }
    private void CreateTakeoverItem()
    {
        Distances.Clear();
        if (1==0 && Player.PedLastKilledPlayer != null && Player.PedLastKilledPlayer.Pedestrian.Exists())
        {
            Distances = new List<DistanceSelect>() { new DistanceSelect("Killer", -2f), };
        }

        List<DistanceSelect> RegularDistances = new List<DistanceSelect> { new DistanceSelect("Closest", -1f), new DistanceSelect("100 M", 100f), new DistanceSelect("500 M", 500f), new DistanceSelect("Any", 1000f) };
        Distances.AddRange(RegularDistances);


        
        string takeoverRespawnText = "Takeover Pedestrian";
        string takeoverrespawnDescription = "Takeover a random pedestrian around the player.";
        UIMenuListScrollerItem<DistanceSelect> TakeoverRandomPed = new UIMenuListScrollerItem<DistanceSelect>(takeoverRespawnText, takeoverrespawnDescription, Distances);
        TakeoverRandomPed.Activated += (sender, selectedItem) =>
        {
            if (Settings.SettingsManager.RespawnSettings.PermanentDeathMode)//shouldnt be here!
            {
                GameSaves.DeleteSave();
            }
            if (TakeoverRandomPed.SelectedItem.Distance == -2f)
            {
                PedSwap.BecomeKnownPed(Player.PedLastKilledPlayer, false, false);
            }
            else if (TakeoverRandomPed.SelectedItem.Distance == -1f)
            {
                PedSwap.BecomeExistingPed(500f, true, false, true, true);
            }
            else
            {
                PedSwap.BecomeExistingPed(TakeoverRandomPed.SelectedItem.Distance, false, false, true, true);
            }

            Menu.Visible = false;
        };
        Menu.AddItem(TakeoverRandomPed);


        //if (Player.PedLastKilledPlayer != null && Player.PedLastKilledPlayer.Pedestrian.Exists())
        //{
        //    //string takeoverKillerRespawnText = "Takeover Killer";
        //    //string takeoverKillerrespawnDescription = $"Takeover your killer {Player.PedLastKilledPlayer.Name}";
        //    //UIMenuItem TakeoverKillerPed = new UIMenuItem(takeoverKillerRespawnText, takeoverKillerrespawnDescription);
        //    //TakeoverKillerPed.Activated += (sender, selectedItem) =>
        //    //{
        //    //    PedSwap.BecomeKnownPed(Player.PedLastKilledPlayer, false, false);
        //    //    if (Settings.SettingsManager.RespawnSettings.PermanentDeathMode)//shouldnt be here!
        //    //    {
        //    //        GameSaves.DeleteSave();
        //    //    }
        //    //    Menu.Visible = false;
        //    //};
        //    //Menu.AddItem(TakeoverKillerPed);
        //}

    }
}