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
using System.Xml.Linq;
using System.Xml.Schema;

public class TaxiInteractionMenu : VehicleInteractionMenu
{
    private TaxiVehicleExt TaxiVehicleExt;
    private TaxiRide TaxiRide;
    private IPlacesOfInterest PlacesOfInterest;
    private ITimeReportable Time;
    public TaxiInteractionMenu(VehicleExt vehicleExt, TaxiVehicleExt taxiVehicleExt) : base(vehicleExt)
    {
        TaxiVehicleExt = taxiVehicleExt;
    }
    public override void ShowInteractionMenu(IInteractionable player, IWeapons weapons, IModItems modItems, VehicleDoorSeatData vehicleDoorSeatData, IVehicleSeatAndDoorLookup vehicleSeatDoorData, IEntityProvideable world, ISettingsProvideable settings, 
        bool showDefault, IPlacesOfInterest placesOfInterest, ITimeReportable time)
    {
        if (showDefault)
        {
            base.ShowInteractionMenu(player,weapons,modItems, vehicleDoorSeatData, vehicleSeatDoorData, world, settings, true, placesOfInterest, time);
            return;
        }
        VehicleDoorSeatData = vehicleDoorSeatData;
        Player = player;
        PlacesOfInterest = placesOfInterest;
        Time = time;
        TaxiRide = Player.TaxiManager.GetOrCreateRide(TaxiVehicleExt);
        if(TaxiRide == null)
        {
            Player.ButtonPrompts.RemovePrompts("VehicleInteract");
            return;
        }
        EntryPoint.WriteToConsole($"SHOW TAXI INTERACTION USING THE TAXI RIDE FROM:{TaxiRide.RespondingDriver?.Handle} VEH:{TaxiRide.RespondingVehicle?.Handle}");
        CreateInteractionMenuTaxi();
        AddItems();
        VehicleInteractMenu.Visible = true;
        IsShowingMenu = true;
        Player.ButtonPrompts.RemovePrompts("VehicleInteract");
        ProcessMenuTaxi();
    }
    private void AddItems()
    {
        AddDestinationMenu();
        AddDrivingStyleMenu();
        AddOtherOptions();
    }
    private void AddDestinationMenu()
    {
        UIMenu DestinationSubMenu = MenuPool.AddSubMenu(VehicleInteractMenu, "Destinations");
        VehicleInteractMenu.MenuItems[VehicleInteractMenu.MenuItems.Count() - 1].Description = "Set the current chosen destination.";
        DestinationSubMenu.SetBannerType(EntryPoint.LSRedColor);
        DestinationSubMenu.OnMenuOpen += (sender1) =>
        {
            DestinationSubMenu.Clear();
            Vector3 MarkerPosOrig = Player.GPSManager.GetGPSRoutePosition();
            if (MarkerPosOrig != Vector3.Zero)
            {
                SpawnLocation spawnLocation = new SpawnLocation(MarkerPosOrig);
                spawnLocation.GetClosestStreet(false);
                int EstimatedPrice = TaxiRide.GetPrice(Player.Character.Position.DistanceTo2D(spawnLocation.StreetPosition));
                DestinationSubMenu.SetBannerType(EntryPoint.LSRedColor);
                UIMenuItem GoToMarker = new UIMenuItem("Marker", "Drive to the current marker") { RightLabel = $"~r~${EstimatedPrice}" };
                GoToMarker.Activated += (sender, e) =>
                {
                    SetDestinationMarker();
                    DestinationSubMenu.Visible = false;
                };
                DestinationSubMenu.AddItem(GoToMarker);
            }


            List<UIMenu> SubMenus = new List<UIMenu>();

            foreach (GameLocation gameLocation in PlacesOfInterest.PossibleLocations.InteractableLocations().OrderBy(x => x.TypeName))
            {
                if (!gameLocation.IsEnabled || !gameLocation.ShowsOnTaxi || !gameLocation.IsSameState(Player.CurrentLocation?.CurrentZone?.GameState))
                {
                    continue;
                }
                UIMenu categoryToAdd = SubMenus.FirstOrDefault(x => x.SubtitleText == gameLocation.TypeName);
                if (categoryToAdd == null)
                {
                    categoryToAdd = MenuPool.AddSubMenu(DestinationSubMenu, gameLocation.TypeName);
                    categoryToAdd.SetBannerType(EntryPoint.LSRedColor);
                    SubMenus.Add(categoryToAdd);
                }
            }





            foreach (GameLocation gameLocation in PlacesOfInterest.PossibleLocations.InteractableLocations().OrderBy(x => Player.Character.Position.DistanceTo2D(x.EntrancePosition)))
            {
                if (!gameLocation.IsEnabled || !gameLocation.ShowsOnTaxi || !gameLocation.IsSameState(Player.CurrentLocation?.CurrentZone?.GameState))
                {
                    continue;
                }
                UIMenu categoryToAdd = SubMenus.FirstOrDefault(x => x.SubtitleText == gameLocation.TypeName);
                if (categoryToAdd == null)
                {
                    continue;
                }
                float Distance = Player.Character.Position.DistanceTo2D(gameLocation.EntrancePosition);
                float distanceMile = Distance * 0.000621371f;
                int EstimatedLocationPrice = TaxiRide.GetPrice(Distance);
                UIMenuItem goToLocation = new UIMenuItem(gameLocation.Name, gameLocation.TaxiInfo(Time.CurrentHour, Distance)) { RightLabel = ($"~s~{Math.Round(distanceMile,2)} mi - ~r~${EstimatedLocationPrice}") };
                goToLocation.Activated += (sender, e) =>
                {
                    SetLocationMarker(gameLocation);
                    Player.GPSManager.AddGPSRoute(gameLocation.Name, gameLocation.EntrancePosition);
                    DestinationSubMenu.Visible = false;
                };
                categoryToAdd.AddItem(goToLocation);
            }
        };    
    }

    private void SetLocationMarker(GameLocation gameLocation)
    {
        if (TaxiRide == null)
        {
            EntryPoint.WriteToConsole("Add Destination, no taxi ride found");
            return;
        }
        Vector3 MarkerPos = gameLocation.EntrancePosition;
        SpawnLocation spawnLocation = new SpawnLocation(MarkerPos);
        spawnLocation.GetClosestStreet(false);
        if (!spawnLocation.HasStreetPosition)
        {
            EntryPoint.WriteToConsole("Add Destination, no street pos");
            return;
        }
        if (MarkerPos == Vector3.Zero)
        {
            EntryPoint.WriteToConsole("Add Destination, marker pos not found");
            return;
        }
        VehicleInteractMenu.SubtitleText = $"Destination: {gameLocation.Name}";
        TaxiRide.UpdateDestination(MarkerPos, Player.Character.Position, gameLocation.Name);
    }

    private void SetDestinationMarker()
    {
        if (TaxiRide == null)
        {
            EntryPoint.WriteToConsole("Add Destination, no taxi ride found");
            return;
        }
        Vector3 MarkerPos = Player.GPSManager.GetGPSRoutePosition();
        SpawnLocation spawnLocation = new SpawnLocation(MarkerPos);
        spawnLocation.GetClosestStreet(false);
        if (!spawnLocation.HasStreetPosition)
        {
            EntryPoint.WriteToConsole("Add Destination, no street pos");
            return;
        }
        if (MarkerPos == Vector3.Zero)
        {
            EntryPoint.WriteToConsole("Add Destination, marker pos not found");
            return;
        }
        VehicleInteractMenu.SubtitleText = "Destination: Marker";
        TaxiRide.UpdateDestination(MarkerPos, Player.Character.Position, "Marker");
    }
    private void AddDrivingStyleMenu()
    {
        int fastFee = 40;
        if(TaxiRide?.HasSetFast == true)
        {
            fastFee = 0;
        }
        List<PedDrivingStyle> PossibleStyles = new List<PedDrivingStyle>() { 
            new PedDrivingStyle("Normal", eCustomDrivingStyles.RegularDriving, 10f), 
            new PedDrivingStyle("Fast", eCustomDrivingStyles.RegularDriving, 40f) { Fee = fastFee }, 
        };
        UIMenuListScrollerItem<PedDrivingStyle> drivingStyleScroller = new UIMenuListScrollerItem<PedDrivingStyle>("Styles","Choose a style", PossibleStyles);
        drivingStyleScroller.Activated += (sender,selectedItem) =>
        {
            if(TaxiRide == null || TaxiRide.RespondingDriver == null || TaxiRide.RespondingDriver.TaxiRide == null)
            {
                return;
            }
            if(Player.BankAccounts.GetMoney(true) < drivingStyleScroller.SelectedItem.Fee)
            {
                TaxiRide.DisplayNotification("~r~Insufficient Funds", "We are sorry, we are unable to complete this transaction.");
                return;
            }    
            if(drivingStyleScroller.SelectedItem.Fee > 0)
            {
                TaxiRide.HasSetFast = true;
            }
            TaxiRide.RespondingDriver.TaxiRide.TaxiDrivingStyle = drivingStyleScroller.SelectedItem;
            TaxiRide.DisplayNotification("~g~Driving Style", $"Updated to {drivingStyleScroller.SelectedItem.Name}~n~Price: ~r~${drivingStyleScroller.SelectedItem.Fee}");
            Player.BankAccounts.GiveMoney(-1 * drivingStyleScroller.SelectedItem.Fee, true);
            VehicleInteractMenu.Visible = false;
        };
        VehicleInteractMenu.AddItem(drivingStyleScroller);
    }
    private void AddOtherOptions()
    {
        UIMenuItem CancelRide = new UIMenuItem("Cancel Ride", "Cancel the current ride.");
        CancelRide.Activated += (sender, e) =>
        {
            Player.TaxiManager.CancelRide(TaxiRide, true);
            VehicleInteractMenu.Visible = false;
        };
        VehicleInteractMenu.AddItem(CancelRide);
    }
    private void CreateInteractionMenuTaxi()
    {
        MenuPool = new MenuPool();
        VehicleInteractMenu = new UIMenu("Taxi", $"Destination: {(TaxiRide == null ? "None" : TaxiRide.DesitnationName)}");
        VehicleInteractMenu.SetBannerType(EntryPoint.LSRedColor);
        MenuPool.Add(VehicleInteractMenu);
        if(TaxiVehicleExt != null && TaxiVehicleExt.TaxiFirm != null)
        {
            VehicleInteractMenu.TitleText = TaxiVehicleExt.TaxiFirm.ShortName;
        }
    }
    private void ProcessMenuTaxi()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                while (EntryPoint.ModController.IsRunning && Player.IsAliveAndFree && MenuPool.IsAnyMenuOpen() && VehicleExt.Vehicle.Exists() && VehicleExt.Vehicle.DistanceTo2D(Game.LocalPlayer.Character) <= 7f)
                {
                    MenuPool.ProcessMenus();
                    GameFiber.Yield();
                }
                IsShowingMenu = false;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "VehicleInteraction");
    }
}

