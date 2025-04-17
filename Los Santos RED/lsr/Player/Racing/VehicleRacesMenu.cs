using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
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
    private IDispatchablePeople DispatchablePeople; 
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
    private List<string> SupportedTracks = new List<string>();
    private List<string> AllowedOpponentGroups = new List<string>();    
    //private UIMenu playerVehicleSubMenu;
    //private UIMenuItem playerVehicleSubMenuItem;
    private VehicleExt selectedPlayerVehicle;
    private int totalSelectedOpponents;
    private UIMenu opponentsVehicleGroupSubMenu;
    private UIMenuItem opponentsVehicleGroupSubMenuItem;
    private int MaxBet;
    private UIMenu raceSettingsSubMenu;
    private UIMenuItem raceSettingsSubMenuItem;
    private UIMenuCheckboxItem clearTrafficMenuItem;
    private UIMenuNumericScrollerItem<int> LapsMenuItem;

    public VehicleRacesMenu(MenuPool menuPool, UIMenu raceSubMenu, PedExt conversingPed, IVehicleRaces vehicleRaces, IPlacesOfInterest placesOfInterest, IEntityProvideable world,
        IInteractionable player, bool isPointRace, AdvancedConversation advancedConversation, IDispatchableVehicles dispatchableVehicles, List<string> supportedTracks,
        List<string> allowedOpponentGroups, int maxBex, IDispatchablePeople dispatchablePeople)
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
        SupportedTracks = supportedTracks;
        AllowedOpponentGroups = allowedOpponentGroups;
        MaxBet = maxBex;
        DispatchablePeople = dispatchablePeople;
    }

    public bool IsPointRace { get; private set; }
    public void Setup()
    {
        EntryPoint.WriteToConsole("VehicleRacesMenu START RAN");

        selectedPlayerVehicle = Player.CurrentVehicle;

        AddTrackSubMenu();
        AddPlayerVehicleMenu();
        AddBettingSubMenu();
        AddOpponentsSubMenu();

        AddSettingsSubMenu();
        startRaceMenuItem = new UIMenuItem("Start Race", "Select to start the current race");
        startRaceMenuItem.Activated += (menu, item) =>
        {      
            if(!AttemptStartRace(menu))
            {
                Game.DisplayHelp("Error starting race");
            }            
        };
        RaceMenu.AddItem(startRaceMenuItem);

        UpdateStartRaceDescription();
    }

    private void AddSettingsSubMenu()
    {
        raceSettingsSubMenu = MenuPool.AddSubMenu(RaceMenu, "Settings");
        raceSettingsSubMenuItem = RaceMenu.MenuItems[RaceMenu.MenuItems.Count() - 1];
        clearTrafficMenuItem = new UIMenuCheckboxItem("Disable Traffic", false, "If enabled, ambient traffic will not spawn when racing.");
        raceSettingsSubMenu.AddItem(clearTrafficMenuItem);
        LapsMenuItem = new UIMenuNumericScrollerItem<int>("Laps", "Set number of laps to complete", 1, 10, 1);
        LapsMenuItem.Value = 1;

        if(!IsPointRace)
        {
            raceSettingsSubMenu.AddItem(LapsMenuItem);
        }
        
    }

    private void AddPlayerVehicleMenu()
    {
        if(IsPointRace)
        {
            return;
        }
        List<VehicleExt> possibleVehicles = new List<VehicleExt>(); 
        if(Player.CurrentVehicle != null)
        {
            possibleVehicles.Add(Player.CurrentVehicle);
        }
        possibleVehicles.AddRange(Player.VehicleOwnership.OwnedVehicles);
        possibleVehicles.AddRange(World.Vehicles.AllVehicleList.Where(x => x.HasBeenEnteredByPlayer && x.Vehicle.Exists() && !x.Vehicle.HasOccupants));
        selectedPlayerVehicle = possibleVehicles.FirstOrDefault();

        if (selectedPlayerVehicle != null) 
        {
            EntryPoint.WriteToConsole($"Selected vehicle set initially: {selectedPlayerVehicle.GetCarName()}");
        }
        UIMenuListScrollerItem<VehicleExt> possibleVehiclesScroller = new UIMenuListScrollerItem<VehicleExt>("Player Vehicle: ", "Select the vehicle you want to use during the race.", possibleVehicles);
        possibleVehiclesScroller.Activated += (menu, item) =>
        {
            if(!possibleVehiclesScroller.Items.Any())
            {
                return;
            }
            if(possibleVehiclesScroller.SelectedItem == null)
            {
                return;
            }
            selectedPlayerVehicle = possibleVehiclesScroller.SelectedItem;
            Game.DisplaySubtitle($"Selected: {possibleVehiclesScroller.SelectedItem.GetCarName()}");
            UpdateStartRaceDescription();
        };
        RaceMenu.AddItem(possibleVehiclesScroller);
        
    }
    private void AddTrackSubMenu()
    {
        trackSubMenu = MenuPool.AddSubMenu(RaceMenu,(IsPointRace ? "Finish Point" : "Track"));
        trackSubMenuItem = RaceMenu.MenuItems[RaceMenu.MenuItems.Count() - 1];  
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
            if (SupportedTracks == null || !SupportedTracks.Any() || SupportedTracks.Contains(vehicleRaceTrack.ID)) 
            {
                UIMenuItem trackName = new UIMenuItem(vehicleRaceTrack.Name, vehicleRaceTrack.Description);
                trackName.Activated += (sender, selectedItem) =>
                {
                    SelectedTrack = vehicleRaceTrack;
                    Game.DisplaySubtitle($"Selected Track: {vehicleRaceTrack.Name}");
                    trackSubMenuItem.RightLabel = vehicleRaceTrack.Name;
                    UpdateStartRaceDescription();
                };
                trackSubMenu.AddItem(trackName);
            }
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
        UIMenuNumericScrollerItem<int> totalOpponentsMenuItem = new UIMenuNumericScrollerItem<int>("Racers:","Set the number of opponents",1,10,1);
        totalOpponentsMenuItem.Value = 3;
        totalSelectedOpponents = 3;
        totalOpponentsMenuItem.Activated += (menu, item) =>
        {
            totalSelectedOpponents = totalOpponentsMenuItem.Value;
            UpdateStartRaceDescription();
        };
        totalOpponentsMenuItem.IndexChanged += (sender, oldindex, newindex) =>
        {
            totalSelectedOpponents = totalOpponentsMenuItem.Value;
            UpdateStartRaceDescription();
        };
        opponentsSubMenu.AddItem(totalOpponentsMenuItem);
        opponentsVehicleGroupSubMenu = MenuPool.AddSubMenu(opponentsSubMenu, "Vehicles:");
        opponentsVehicleGroupSubMenuItem = opponentsSubMenu.MenuItems[opponentsSubMenu.MenuItems.Count() - 1];


        bool selected = false;

        foreach (DispatchableVehicleGroup dvg in DispatchableVehicles.AllVehicles.Where(x => x.DispatchbleVehicleGroupType == DispatchbleVehicleGroupType.Racing))
        {
            if (AllowedOpponentGroups == null || !AllowedOpponentGroups.Any() || AllowedOpponentGroups.Contains(dvg.DispatchableVehicleGroupID))
            {
                if (!selected)
                {
                    selectedOpponentVehicles = dvg;
                    selected = true;
                }
                string fulldescription = dvg.Description;
                fulldescription += "~n~~n~";
                foreach (DispatchableVehicle dv in dvg.DispatchableVehicles)
                {
                    string ModelName = NativeHelper.VehicleModelName(Game.GetHashKey(dv.ModelName.ToLower()));
                    fulldescription += ModelName + " ";
                }
                UIMenuItem groupMenuItem = new UIMenuItem(dvg.Name, fulldescription);
                groupMenuItem.Activated += (menu, item) =>
                {
                    selectedOpponentVehicles = dvg;
                    Game.DisplaySubtitle($"Selected Vehicles: {dvg.Name}");
                    UpdateStartRaceDescription();
                };
                opponentsVehicleGroupSubMenu.AddItem(groupMenuItem);
            }
        }
    }
    private void AddPointDestinations()
    {
        trackSubMenu.RemoveBanner();
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
        if (!IsPointRace)
        {
            finalDescription = $"Track: {SelectedTrack?.Name}";
        }
        if(selectedPlayerVehicle == null || !selectedPlayerVehicle.IsOwnedByPlayer)
        {
            if (RaceForPinksCheckbox != null)
            {
                RaceForPinksCheckbox.Enabled = false;
                RaceForPinksCheckbox.Checked = false;
            }
        }
        if (RaceForPinksCheckbox?.Checked == true)
        {
            finalDescription += $"~n~~r~Racing for Pinks~s~";
        }
        else if (MoneyBetScoller?.Value > 0)
        {
            finalDescription += $"~n~Bet Amount: ~r~${MoneyBetScoller.Value}~s~";
        }
        startRaceMenuItem.Description = finalDescription;
        trackSubMenuItem.RightLabel = SelectedPointDestination?.Name;
        if (!IsPointRace)
        {
            trackSubMenuItem.RightLabel = SelectedTrack?.Name;
        }
        if (RaceForPinksCheckbox.Checked)
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
        if (opponentsSubMenuItem != null)
        {
            opponentsSubMenuItem.RightLabel = $"({totalSelectedOpponents}) " + selectedOpponentVehicles?.Name;
            opponentsVehicleGroupSubMenuItem.RightLabel = selectedOpponentVehicles?.Name;
        }
        if(IsPointRace)
        {
            if (selectedPlayerVehicle == null || SelectedPointDestination == null)
            {
                startRaceMenuItem.Enabled = false;
            }
            else
            {
                startRaceMenuItem.Enabled = true;
            }
        }
        else
        {
            if(selectedOpponentVehicles == null || selectedPlayerVehicle == null || SelectedTrack == null)
            {
                startRaceMenuItem.Enabled = false;
            }
            else
            {
                startRaceMenuItem.Enabled = true;
            }
        }

        EntryPoint.WriteToConsole($"selectedOpponentVehicles:{selectedOpponentVehicles != null} selectedPlayerVehicle:{selectedPlayerVehicle != null} SelectedTrack:{SelectedTrack != null}");
    }
    private void AddBettingSubMenu()
    {
        bettingSubMenu = MenuPool.AddSubMenu(RaceMenu, "Betting");
        bettingSubMenuItem = RaceMenu.MenuItems[RaceMenu.MenuItems.Count() - 1];

        if (IsPointRace)
        {
            bettingSubMenu.RemoveBanner();
        }
        int scrollerUpperLimit = Player.BankAccounts.GetMoney(false);
        if (MaxBet != -1)
        {
            if(scrollerUpperLimit > MaxBet)
            {
                scrollerUpperLimit = MaxBet;
            }
        }
        string description = $"Enter the cash bet amount. Only winners are paid.";
        if(MaxBet > 0)
        {
            description += $"~n~Max Bet: ${MaxBet}";
        }
        MoneyBetScoller = new UIMenuNumericScrollerItem<int>("Cash Bet", description, 0, scrollerUpperLimit, 100) { Formatter = v => "$" + v + "" };
        RaceForPinksCheckbox = new UIMenuCheckboxItem("Pink Slip Race", false);
        MoneyBetScoller.Value = 0;
        MoneyBetScoller.Activated += (sender, e) =>
        {

        };
        MoneyBetScoller.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            if (selectedPlayerVehicle == null || !selectedPlayerVehicle.IsOwnedByPlayer)
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
        if(RaceForPinksCheckbox != null && (selectedPlayerVehicle == null || !selectedPlayerVehicle.IsOwnedByPlayer))
        {
            RaceForPinksCheckbox.Enabled = false;
        }
        bettingSubMenu.AddItem(MoneyBetScoller);
        bettingSubMenu.AddItem(RaceForPinksCheckbox);      
    }
    private bool AttemptStartRace(UIMenu uIMenu)
    {
        if (IsPointRace)
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
            //Game.DisplaySubtitle("No Race Track Set");
            return false;
        }
        if(selectedPlayerVehicle == null)
        {
            return false;
        }
        VehicleRace newRace = new VehicleRace(SelectedTrack.Name, SelectedTrack, selectedPlayerVehicle, World,LapsMenuItem.Value,clearTrafficMenuItem.Checked);
        uIMenu.Visible = false;
        GameFiber.StartNew(delegate
        {
            try
            {
                Game.FadeScreenOut(1000, true);
                MenuPool.CloseAllMenus();
                while (Player.ActivityManager.IsInteractingWithLocation)
                {
                    GameFiber.Yield();
                }

                 

                if(!Player.RacingManager.StartRegularRace(newRace, MoneyBetScoller.Value, RaceForPinksCheckbox.Checked, selectedOpponentVehicles, 
                    DispatchablePeople.AllPeople.Where(x => x.DispatchablePersonGroupID == "VehicleRacePeds").FirstOrDefault(), 
                    totalSelectedOpponents))
                {
                    Game.DisplayHelp("Error Starting Race");
                }
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
        if(Player.CurrentVehicle == null)
        {
            Game.DisplaySubtitle("No Player Vehicle Found");
            return false;
        }
        Vector3 FinishPosition = NativeHelper.GetStreetPosition(SelectedPointDestination.EntrancePosition, false);
        if (FinishPosition == Vector3.Zero)
        {
            FinishPosition = SelectedPointDestination.EntrancePosition;
        }
        VehicleRace newRace = new VehicleRace("PointToPointRace", new VehicleRaceTrack("ptp1", "PointToPointRace", "", new List<VehicleRaceCheckpoint>() { new VehicleRaceCheckpoint(0, FinishPosition) }, null), Player.CurrentVehicle, World,1,clearTrafficMenuItem.Checked);
        uIMenu.Visible = false;
        AdvancedConversation?.DisposeConversation();
        GameFiber.Yield();
        if(!Player.RacingManager.StartPointToPointRace(newRace, PedExt, MoneyBetScoller.Value, RaceForPinksCheckbox.Checked))
        {
            Game.DisplayHelp("Error Starting Race");
            return false;
        }
        return true;
    }


}

