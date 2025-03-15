using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


public class VehicleRacesMenu
{
    private MenuPool MenuPool;
    private UIMenu RaceMenu;
    private PedExt PedExt;
    private IVehicleRaces VehicleRaces;
    private UIMenu TrackMenu;
    private UIMenu trackSubMenu;
    private GameLocation SelectedPointDestination;
    private IPlacesOfInterest PlacesOfInterest;
    private IEntityProvideable World;
    private IInteractionable Player;
    private UIMenu bettingSubMenu;
    private UIMenuItem startRaceMenuItem;
    private UIMenuNumericScrollerItem<int> MoneyBetScoller;
    private UIMenuCheckboxItem RaceForPinksCheckbox;
    private AdvancedConversation AdvancedConversation;

    public VehicleRacesMenu(MenuPool menuPool, UIMenu raceSubMenu, PedExt conversingPed, IVehicleRaces vehicleRaces, IPlacesOfInterest placesOfInterest, IEntityProvideable world, IInteractionable player, bool isPointRace, AdvancedConversation advancedConversation)
    {
        MenuPool = menuPool;
        RaceMenu = raceSubMenu;
        PedExt = conversingPed;
        VehicleRaces = vehicleRaces;
        IsPointRace = isPointRace;
        PlacesOfInterest = placesOfInterest;
        World = world;
        Player = player;
        AdvancedConversation = advancedConversation;
    }

    public bool IsPointRace { get; private set; }

    public void Setup()
    {
        EntryPoint.WriteToConsole("VehicleRacesMenu START RAN");
        AddTrackSubMenu();
        AddBettingSubMenu();
        startRaceMenuItem = new UIMenuItem("Start Race", "Select to start the current race");
        startRaceMenuItem.Activated += (menu, item) =>
        {      
            if(!AttemptStartRace(menu))
            {
                Game.DisplayHelp("Error starting race");
            }            
        };
        RaceMenu.AddItem(startRaceMenuItem);

        RaceMenu.OnMenuOpen += (sender) =>
        {
            if(PedExt != null && PedExt.Pedestrian.Exists())
            {
                PedExt.Pedestrian.BlockPermanentEvents = true;
                PedExt.Pedestrian.Tasks.Pause(-1);
            }
        };
        RaceMenu.OnMenuClose += (sender) =>
        {
            if (PedExt != null && PedExt.Pedestrian.Exists())
            {
                PedExt.Pedestrian.BlockPermanentEvents = false;
                PedExt.Pedestrian.Tasks.Clear();
            }
        };
    }



    private bool AttemptStartRace(UIMenu uIMenu)
    {
        if(SelectedPointDestination == null)
        {
            Game.DisplaySubtitle("No Race Finish Set");
            return false;
        }
        Vector3 FinishPosition = NativeHelper.GetStreetPosition(SelectedPointDestination.EntrancePosition, false);
        if(FinishPosition == Vector3.Zero)
        {
            FinishPosition = SelectedPointDestination.EntrancePosition;
        }
        VehicleRace newRace = new VehicleRace("PointToPointRace",new List<VehicleRaceCheckpoint>() { new VehicleRaceCheckpoint(0,FinishPosition) },null);

        //Player.ActivityManager.EndInteraction();
        uIMenu.Visible = false;

        AdvancedConversation?.DisposeConversation();
        GameFiber.Yield();
        Player.RacingManager.StartPointToPointRace(newRace, PedExt);
        return true;
    }

    private void AddTrackSubMenu()
    {

        trackSubMenu = MenuPool.AddSubMenu(RaceMenu,(IsPointRace ? "Finish Point" : "Track"));
        trackSubMenu.RemoveBanner();
        if (IsPointRace)
        {
            AddPointDestinations();
        }
        else
        {
            AddTracks();
        }
    }
    private void AddPointDestinations()
    {
        List<GameLocation> AllLocations = PlacesOfInterest.AllLocations().Where(x => x.IsEnabled && x.IsCorrectMap(World.IsMPMapLoaded) && x.IsSameState(Player.CurrentLocation)).ToList();
        foreach (string typeName in AllLocations.OrderBy(x => x.TypeName).Select(x => x.TypeName).Distinct())
        {
            UIMenuListScrollerItem<GameLocation> myLocationType = new UIMenuListScrollerItem<GameLocation>($"{typeName}", "Select a finish line destination", AllLocations.Where(x => x.TypeName == typeName).OrderBy(x=> x.EntrancePosition.DistanceTo2D(Player.Character)));
            myLocationType.Activated += (menu, item) =>
            {
                SelectedPointDestination = myLocationType.SelectedItem;
                Game.DisplaySubtitle($"Selected: {SelectedPointDestination?.Name}");
                UpdateStartRaceDescription();
            };
            trackSubMenu.AddItem(myLocationType);
        }
        EntryPoint.WriteToConsole("VehicleRacesMenu AddPointDestinations");
    }

    private void UpdateStartRaceDescription()
    {
        string finalDescription = $"Finish Line: ~p~{SelectedPointDestination?.Name}~s~";
        if(RaceForPinksCheckbox.Checked)
        {
            finalDescription += $"~n~~r~Racing for Pinks~s~";
        }
        else if (MoneyBetScoller.Value > 0)
        {
            finalDescription += $"~n~Bet Amount: ~r~${MoneyBetScoller.Value}~s~";
        }
        startRaceMenuItem.Description = finalDescription;
    }
    private void AddTracks()
    {
        foreach (VehicleRace vehicleRace in VehicleRaces.VehicleRaceTypeManager.VehiclesRaces)
        {
            vehicleRace.AddTrackToMenu(MenuPool, RaceMenu);
        }
    }
    private void AddBettingSubMenu()
    {
        bettingSubMenu = MenuPool.AddSubMenu(RaceMenu, "Betting");
        bettingSubMenu.RemoveBanner();
        MoneyBetScoller = new UIMenuNumericScrollerItem<int>("Cash Bet", "Enter the cash bet amount. Only winners are paid.", 0, 5000, 100) { Formatter = v => "$" + v + "" };
        RaceForPinksCheckbox = new UIMenuCheckboxItem("Pink Slip Race", false);
        MoneyBetScoller.Value = 0;
        MoneyBetScoller.Activated += (sender, e) =>
        {

        };
        MoneyBetScoller.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            if (Player.CurrentVehicle == null || !Player.CurrentVehicle.IsOwnedByPlayer)
            {
                RaceForPinksCheckbox.Enabled = false;
                return;
            }
            RaceForPinksCheckbox.Enabled = MoneyBetScoller.Value == 0;
            UpdateStartRaceDescription();
        };
        RaceForPinksCheckbox.CheckboxEvent += (sender, Checked) =>
        {
            MoneyBetScoller.Enabled = !Checked;
            UpdateStartRaceDescription();
        };
        if(Player.CurrentVehicle == null || !Player.CurrentVehicle.IsOwnedByPlayer)
        {
            RaceForPinksCheckbox.Enabled = false;

        }
        bettingSubMenu.AddItem(MoneyBetScoller);
        bettingSubMenu.AddItem(RaceForPinksCheckbox);
    }
}

