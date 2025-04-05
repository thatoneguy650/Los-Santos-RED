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
using System.Xml.Linq;


public class VehicleRacesMenu
{
    private MenuPool MenuPool;
    private UIMenu RaceMenu;
    private PedExt PedExt;
    private IVehicleRaces VehicleRaces;
    private UIMenu TrackMenu;
    private UIMenu trackSubMenu;
    private UIMenuItem trackSubMenuItem;
    private GameLocation SelectedPointDestination;
    private IPlacesOfInterest PlacesOfInterest;
    private IEntityProvideable World;
    private IInteractionable Player;
    private UIMenu bettingSubMenu;
    private UIMenuItem bettingSubMenuItem;
    private UIMenuItem startRaceMenuItem;
    private UIMenuNumericScrollerItem<int> MoneyBetScoller;
    private UIMenuCheckboxItem RaceForPinksCheckbox;
    private AdvancedConversation AdvancedConversation;
    private UIMenuItem raceToMarkerPositionMenuItem;
    private UIMenu opponentsSubMenu;
    private UIMenuItem opponentsSubMenuItem;
    private IDispatchableVehicles DispatchableVehicles;
    private DispatchableVehicleGroup selectedOpponentVehicles;
    private VehicleRaceTrack SelectedTrack;

    public VehicleRacesMenu(MenuPool menuPool, UIMenu raceSubMenu, PedExt conversingPed, IVehicleRaces vehicleRaces, IPlacesOfInterest placesOfInterest, IEntityProvideable world, IInteractionable player, bool isPointRace, AdvancedConversation advancedConversation, IDispatchableVehicles dispatchableVehicles)
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
        DispatchableVehicles = dispatchableVehicles;
    }

    public bool IsPointRace { get; private set; }
    public void Setup()
    {
        EntryPoint.WriteToConsole("VehicleRacesMenu START RAN");
        AddTrackSubMenu();
        AddBettingSubMenu();
        AddOpponentsSubMenu();


        startRaceMenuItem = new UIMenuItem("Start Race", "Select to start the current race");
        startRaceMenuItem.Activated += (menu, item) =>
        {      
            if(!AttemptStartRace(menu))
            {
                Game.DisplayHelp("Error starting race");
            }            
        };
        RaceMenu.AddItem(startRaceMenuItem);
    }
    private bool AttemptStartRace(UIMenu uIMenu)
    {
        if(IsPointRace)
        {
            return AttemptStartPointRace(uIMenu);
        }
        else
        {
            return AttemptStartRegularRace(uIMenu);
        }
    }
    private bool AttemptStartRegularRace(UIMenu uIMenu)
    {
        if (SelectedTrack == null)
        {
            Game.DisplaySubtitle("No Race Track Set");
            return false;
        }
        VehicleRace newRace = new VehicleRace(SelectedTrack.Name, SelectedTrack);
        uIMenu.Visible = false;
        GameFiber.StartNew(delegate
        {
            try
            {
                MenuPool.CloseAllMenus();
                while (Player.ActivityManager.IsInteractingWithLocation)
                {
                    GameFiber.Yield();
                }
                //GameFiber.Sleep(1000);
                Player.RacingManager.StartRegularRace(newRace, MoneyBetScoller.Value, RaceForPinksCheckbox.Checked, selectedOpponentVehicles);
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "RaceMeetupInteract");



        
        return true;
    }
    private bool AttemptStartPointRace(UIMenu uIMenu)
    {
        if (SelectedPointDestination == null)
        {
            Game.DisplaySubtitle("No Race Finish Set");
            return false;
        }
        Vector3 FinishPosition = NativeHelper.GetStreetPosition(SelectedPointDestination.EntrancePosition, false);
        if (FinishPosition == Vector3.Zero)
        {
            FinishPosition = SelectedPointDestination.EntrancePosition;
        }
        VehicleRace newRace = new VehicleRace("PointToPointRace", new VehicleRaceTrack("PointToPointRace", new List<VehicleRaceCheckpoint>() { new VehicleRaceCheckpoint(0, FinishPosition) }, null));
        //Player.ActivityManager.EndInteraction();
        uIMenu.Visible = false;
        AdvancedConversation?.DisposeConversation();
        GameFiber.Yield();
        Player.RacingManager.StartPointToPointRace(newRace, PedExt, MoneyBetScoller.Value, RaceForPinksCheckbox.Checked);
        return true;
    }

    private void AddTrackSubMenu()
    {

        trackSubMenu = MenuPool.AddSubMenu(RaceMenu,(IsPointRace ? "Finish Point" : "Track"));
        trackSubMenuItem = RaceMenu.MenuItems[RaceMenu.MenuItems.Count() - 1];
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
    private void AddTracks()
    {
        foreach (VehicleRaceTrack vehicleRaceTrack in VehicleRaces.VehicleRaceTypeManager.VehicleRaceTracks)
        {
            UIMenuItem trackName = new UIMenuItem(vehicleRaceTrack.Name);
            trackName.Activated += (sender, selectedItem) =>
            {
                SelectedTrack = vehicleRaceTrack;
                Game.DisplaySubtitle($"Selected Track: {vehicleRaceTrack.Name}");
            };
            trackSubMenu.AddItem(trackName);
        }
    }
    private void AddOpponentsSubMenu()
    {
        if(IsPointRace)
        {
            return;
        }
        opponentsSubMenu = MenuPool.AddSubMenu(RaceMenu, "Opponents");
        opponentsSubMenuItem = RaceMenu.MenuItems[RaceMenu.MenuItems.Count() - 1];
        opponentsSubMenu.RemoveBanner();

        UIMenuListScrollerItem<DispatchableVehicleGroup> opponentVehicleScroller = new UIMenuListScrollerItem<DispatchableVehicleGroup>("Vehicles", "Select opponent vehicle group list", DispatchableVehicles.AllVehicles.Where(x => x.DispatchbleVehicleGroupType == DispatchbleVehicleGroupType.Racing));
        opponentVehicleScroller.Activated += (menu, item) =>
        {
            selectedOpponentVehicles = opponentVehicleScroller.SelectedItem;
            Game.DisplaySubtitle($"Selected: {opponentVehicleScroller.SelectedItem.DispatchableVehicleGroupID}");
        };
        opponentsSubMenu.AddItem(opponentVehicleScroller);

    }
    private void AddPointDestinations()
    {
        Vector3 MarkerPosOrig = Player.GPSManager.GetGPSRoutePosition();
        raceToMarkerPositionMenuItem = new UIMenuItem("Marker Position", "Set the finish line at the current marker position") { RightLabel = Math.Round(Player.Character.DistanceTo2D(MarkerPosOrig) * 0.000621371, 2).ToString() + " Miles away" };
        raceToMarkerPositionMenuItem.Activated += (menu, item) =>
        {
            SelectedPointDestination = new GameLocation(MarkerPosOrig, 0, "Marker Position", "");
            Game.DisplaySubtitle($"Selected: {SelectedPointDestination?.Name}");
            UpdateStartRaceDescription();
        };
        trackSubMenu.AddItem(raceToMarkerPositionMenuItem);
        if (MarkerPosOrig == Vector3.Zero)
        {
            raceToMarkerPositionMenuItem.Enabled = false;
        }




        List<GameLocation> AllLocations = PlacesOfInterest.AllLocations().Where(x => x.IsEnabled && x.IsCorrectMap(World.IsMPMapLoaded) && x.IsSameState(Player.CurrentLocation)).ToList();


        AllLocations.ForEach(x => x.RefreshDistances());

        foreach (string typeName in AllLocations.OrderBy(x => x.TypeName).Select(x => x.TypeName).Distinct())
        {
            UIMenu typeNameMenu = MenuPool.AddSubMenu(trackSubMenu,typeName);
            List<GameLocation> FilteredItems = AllLocations.Where(x => x.TypeName == typeName).OrderBy(x => x.DistanceToPlayer).ToList();
            foreach (GameLocation location in FilteredItems)
            {
                location.RefreshDistances();
                UIMenuItem uIMenuItem = new UIMenuItem(location.Name, location.Description) { RightLabel = Math.Round(location.DistanceToPlayer * 0.000621371, 2).ToString() + " Miles away" };
                uIMenuItem.Activated += (menu, item) =>
                {
                    SelectedPointDestination = location;
                    Game.DisplaySubtitle($"Selected: {SelectedPointDestination?.Name}");
                    UpdateStartRaceDescription();
                };
                typeNameMenu.AddItem(uIMenuItem);
            }
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



        trackSubMenuItem.RightLabel = SelectedPointDestination?.Name;
        if(RaceForPinksCheckbox.Checked)
        {
            bettingSubMenuItem.RightLabel = "Pink Slip Race";
        }
        else if (MoneyBetScoller.Value > 0)
        {
            bettingSubMenuItem.RightLabel = $"~r~${MoneyBetScoller.Value}~s~";
        }
        else
        {
            bettingSubMenuItem.RightLabel = "";
        }
        
    }

    private void AddBettingSubMenu()
    {
        bettingSubMenu = MenuPool.AddSubMenu(RaceMenu, "Betting");
        bettingSubMenuItem = RaceMenu.MenuItems[RaceMenu.MenuItems.Count() - 1];
        bettingSubMenu.RemoveBanner();
        MoneyBetScoller = new UIMenuNumericScrollerItem<int>("Cash Bet", "Enter the cash bet amount. Only winners are paid.", 0, Player.BankAccounts.GetMoney(false), 100) { Formatter = v => "$" + v + "" };
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

