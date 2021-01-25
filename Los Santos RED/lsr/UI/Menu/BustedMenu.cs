using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class BustedMenu : Menu
{
    private UIMenu Menu;
    private GameLocation CurrentSelectedSurrenderLocation;
    private UIMenuItem Bribe;
    private UIMenuItem ResistArrest;
    private UIMenuListItem Surrender;
    private UIMenuListItem TakeoverRandomPed;
    private UIMenuItem menuBustedTalk;
    private IPedswappable PedSwap;
    private IPlacesOfInterest PlacesOfInterest;
    private IRespawning Respawning;
    private float SelectedTakeoverRadius;
    private List<GameLocation> PoliceStations;
    private List<DistanceSelect> Distances;
    public BustedMenu(MenuPool menuPool, IPedswappable pedSwap, IRespawning respawning, IPlacesOfInterest placesOfInterest)
    {
        PedSwap = pedSwap;
        Respawning = respawning;
        PlacesOfInterest = placesOfInterest;
        Menu = new UIMenu("Busted", "Choose Respawn");
        menuPool.Add(Menu);
        CreateBustedMenu();
    }
    public override void Hide()
    {
        Menu.Visible = false;
    }
    public override void Show()
    {
        if(!Menu.Visible)
        {
            UpdateClosestPoliceStationIndex();
            Menu.Visible = true;
        }
    }
    public override void Toggle()
    {
        if (!Menu.Visible)
        {
            UpdateClosestPoliceStationIndex();
            Menu.Visible = true;
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
        Surrender = new UIMenuListItem("Surrender", "Surrender and get out on bail. Lose bail money and your guns.", PoliceStations);
        TakeoverRandomPed = new UIMenuListItem("Takeover Random Pedestrian", "Takes over a random pedestrian around the player.", Distances);   
        Menu.AddItem(ResistArrest);
        Menu.AddItem(Bribe);
        Menu.AddItem(Surrender);
        Menu.AddItem(TakeoverRandomPed);
        Menu.OnItemSelect += OnItemSelect;
        Menu.OnListChange += OnListChange;
    }
    private string GetKeyboardInput(string DefaultText)
    {
        NativeFunction.Natives.DISPLAY_ONSCREEN_KEYBOARD<bool>(true, "FMMC_KEY_TIP8", "", DefaultText, "", "", "", 255 + 1);
        while (NativeFunction.Natives.UPDATE_ONSCREEN_KEYBOARD<int>() == 0)
        {
            GameFiber.Sleep(500);
        }
        string Value;
        IntPtr ptr = NativeFunction.Natives.GET_ONSCREEN_KEYBOARD_RESULT<IntPtr>();
        Value = Marshal.PtrToStringAnsi(ptr);
        return Value;
    }
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == ResistArrest)
        {
            Respawning.ResistArrest();
        }
        else if (selectedItem == Bribe)
        {
            if (int.TryParse(GetKeyboardInput(""), out int BribeAmount))
            {
                Respawning.BribePolice(BribeAmount);
            }
        }
        if (selectedItem == Surrender)
        {
            Respawning.SurrenderToPolice(CurrentSelectedSurrenderLocation);
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
        if (list == Surrender)
        {
            CurrentSelectedSurrenderLocation = PoliceStations[index];
            Game.Console.Print($"Current Busted Surrender Location {CurrentSelectedSurrenderLocation.Name}");
        }   
        else if (list == TakeoverRandomPed)
        {
            SelectedTakeoverRadius = Distances[index].Distance;
            Game.Console.Print($"Current Busted Takeover Distance {SelectedTakeoverRadius}");
        }
    }
    private void UpdateClosestPoliceStationIndex()
    {
        Surrender.Index = PoliceStations.IndexOf(PlacesOfInterest.GetClosestLocation(Game.LocalPlayer.Character.Position, LocationType.Police));
    }
}