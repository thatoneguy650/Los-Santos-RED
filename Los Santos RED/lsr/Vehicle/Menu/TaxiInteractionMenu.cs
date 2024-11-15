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
    private UIMenuItem teleportMenuItem;
    private IEntityProvideable World;

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
        World = world;
        TaxiRide = Player.TaxiManager.GetOrCreateRide(TaxiVehicleExt);
        if(TaxiRide == null)
        {
            EntryPoint.WriteToConsole($"COULD NOT FIND TAXI RIDE FOR THIS VEHICLE");
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
        AddQuickTravelItem();
        AddOtherOptions();
    }

    private void AddQuickTravelItem()
    {
        if (TaxiRide == null || !TaxiRide.HasDestination || TaxiRide.HasArrivedAtDestination)
        {
            return;
        }
        teleportMenuItem = new UIMenuItem("Quick Travel", "Quick travel to the destination");// { RightLabel = $"~r~${TaxiRide.RequestedFirm.TeleportFee}" };
        teleportMenuItem.Activated += (sender, selectedItem) =>
        {

            if (TaxiRide == null || TaxiRide.RespondingDriver == null || TaxiRide.RespondingDriver.TaxiRide == null)
            {
                return;
            }
            sender.Visible = false;
            float DistanceToMiles = Player.Character.Position.DistanceTo2D(TaxiRide.DestinationLocation.StreetPosition) * 0.000621371f;
            int hoursToTake = (int)Math.Ceiling(DistanceToMiles / 5.0f);
            EntryPoint.WriteToConsole($"Teleporting to pos with taxi Distance (mi) {DistanceToMiles} hoursToTake {hoursToTake}");


            TaxiRide.DestinationLocation.GetRoadBoundaryPosition();
            if (TaxiRide.DestinationLocation.HasRoadBoundaryPosition)
            {
                TaxiRide.DestinationLocation.StreetPosition = TaxiRide.DestinationLocation.RoadBoundaryPosition;
            }




            Player.GPSManager.TeleportToCoords(TaxiRide.DestinationLocation.StreetPosition,TaxiRide.CurrentDriveToHeading, true , false, hoursToTake);
        };
        VehicleInteractMenu.AddItem(teleportMenuItem);
    }

    private void AddDestinationMenu()
    {
        UIMenu DestinationSubMenu = MenuPool.AddSubMenu(VehicleInteractMenu, "Destinations");
        VehicleInteractMenu.MenuItems[VehicleInteractMenu.MenuItems.Count() - 1].Description = "Set the current chosen destination.";
        SetMenuBanner(DestinationSubMenu);
        DestinationSubMenu.OnMenuOpen += (sender1) =>
        {
            DestinationSubMenu.Clear();
            Vector3 MarkerPosOrig = Player.GPSManager.GetGPSRoutePosition();
            if (MarkerPosOrig != Vector3.Zero)
            {
                SpawnLocation spawnLocation = new SpawnLocation(MarkerPosOrig);
                spawnLocation.GetClosestStreet(false);
                float DistanceToMiles = Player.Character.Position.DistanceTo2D(spawnLocation.StreetPosition) * 0.000621371f;
                int EstimatedPrice = TaxiRide.GetPrice(DistanceToMiles);
                string Description = "Drive to the current marker";
                if (TaxiRide.RequestedFirm != null)
                {
                    Description += $"~n~~n~Base Fare: ${TaxiRide.RequestedFirm.BaseFare} ";
                    Description += $"~n~Price Per Mile: ${TaxiRide.RequestedFirm.PricePerMile} ";
                    Description += $"~n~Total Fare: ${TaxiRide.RequestedFirm.CalculateFare(DistanceToMiles)} ";
                }
                UIMenuItem GoToMarker = new UIMenuItem("Marker", Description) { RightLabel = ($"~s~{Math.Round(DistanceToMiles, 2)} mi - ~r~${EstimatedPrice}") };
                GoToMarker.Activated += (sender, e) =>
                {
                    SetDestinationMarker();
                    sender.Visible = false;
                };
                DestinationSubMenu.AddItem(GoToMarker);
            }
            List<UIMenu> SubMenus = new List<UIMenu>();
            foreach (GameLocation gameLocation in PlacesOfInterest.PossibleLocations.InteractableLocations().OrderBy(x => x.TypeName))
            {
                if (!gameLocation.IsEnabled || !gameLocation.ShowsOnTaxi || !gameLocation.IsSameState(Player.CurrentLocation?.CurrentZone?.GameState) || !gameLocation.IsCorrectMap(World.IsMPMapLoaded))
                {
                    continue;
                }
                UIMenu categoryToAdd = SubMenus.FirstOrDefault(x => x.SubtitleText == gameLocation.TypeName);
                if (categoryToAdd == null)
                {
                    categoryToAdd = MenuPool.AddSubMenu(DestinationSubMenu, gameLocation.TypeName);
                    SetMenuBanner(categoryToAdd);
                    SubMenus.Add(categoryToAdd);
                }
            }
            foreach (GameLocation gameLocation in PlacesOfInterest.PossibleLocations.InteractableLocations().OrderBy(x => Player.Character.Position.DistanceTo2D(x.EntrancePosition)))
            {
                if (!gameLocation.IsEnabled || !gameLocation.ShowsOnTaxi || !gameLocation.IsSameState(Player.CurrentLocation?.CurrentZone?.GameState) || !gameLocation.IsCorrectMap(World.IsMPMapLoaded))
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
                int EstimatedLocationPrice = TaxiRide.GetPrice(distanceMile);
                UIMenuItem goToLocation = new UIMenuItem(gameLocation.Name, gameLocation.TaxiInfo(Time.CurrentHour, Distance, TaxiRide.RequestedFirm)) { RightLabel = ($"~s~{Math.Round(distanceMile,2)} mi - ~r~${EstimatedLocationPrice}") };
                goToLocation.Activated += (sender, e) =>
                {
                    SetLocationMarker(gameLocation);
                    sender.Visible = false;
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
        spawnLocation.GetClosestSideOfRoad();


        spawnLocation.GetRoadBoundaryPosition();
        if (spawnLocation.HasRoadBoundaryPosition)
        {
            spawnLocation.StreetPosition = spawnLocation.RoadBoundaryPosition;
        }

        EntryPoint.WriteToConsole($"TAXI SetLocationMarker {gameLocation.Name} HasStreetPosition:{spawnLocation.HasStreetPosition} HasSideOfRoadPosition:{spawnLocation.HasSideOfRoadPosition}");
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

        spawnLocation.StreetPosition = Player.GPSManager.ForceGroundZ(spawnLocation.StreetPosition);
        VehicleInteractMenu.SubtitleText = $"Destination: {gameLocation.Name}";


        TaxiRide.UpdateDestination(spawnLocation.FinalPosition, Player.Character.Position, gameLocation.Name);
        Player.GPSManager.AddGPSRoute(gameLocation.Name, spawnLocation.FinalPosition);
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
        spawnLocation.GetClosestSideOfRoad();

        spawnLocation.GetRoadBoundaryPosition();
        if (spawnLocation.HasRoadBoundaryPosition)
        {
            spawnLocation.StreetPosition = spawnLocation.RoadBoundaryPosition;
        }

        EntryPoint.WriteToConsole($"TAXI SetDestinationMarker MARKER HasStreetPosition:{spawnLocation.HasStreetPosition} HasSideOfRoadPosition:{spawnLocation.HasSideOfRoadPosition}");
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

        spawnLocation.StreetPosition = Player.GPSManager.ForceGroundZ(spawnLocation.StreetPosition);

        VehicleInteractMenu.SubtitleText = "Destination: Marker";
        TaxiRide.UpdateDestination(spawnLocation.FinalPosition, Player.Character.Position, "Marker");
        Player.GPSManager.AddGPSRoute("Marker", spawnLocation.FinalPosition);
    }
    private void AddDrivingStyleMenu()
    {
        if (TaxiRide == null || TaxiRide.HasArrivedAtDestination)
        {
            return;
        }
        UIMenuListScrollerItem<PedDrivingStyle> drivingStyleScroller = new UIMenuListScrollerItem<PedDrivingStyle>("Driving Style","Inform the driver how you want them to drive", TaxiRide.PossibleTaxiDrivingStyles);
        drivingStyleScroller.Activated += (sender,selectedItem) =>
        {
            if(TaxiRide == null || TaxiRide.RespondingDriver == null || TaxiRide.RespondingDriver.TaxiRide == null)
            {
                return;
            }
            if(!drivingStyleScroller.SelectedItem.HasBeenPurchased && Player.BankAccounts.GetMoney(true) < drivingStyleScroller.SelectedItem.Fee)
            {
                TaxiRide.DisplayNotification("~r~Insufficient Funds", "We are sorry, we are unable to complete this transaction.");
                return;
            }    

            if(!drivingStyleScroller.SelectedItem.HasBeenPurchased && drivingStyleScroller.SelectedItem.Fee > 0)
            {
                TaxiRide.DisplayNotification($"~g~Driving Style", $"Updated driving style to {drivingStyleScroller.SelectedItem.Name}~n~Fee: ~r~${drivingStyleScroller.SelectedItem.Fee}");
                Player.BankAccounts.GiveMoney(-1 * drivingStyleScroller.SelectedItem.Fee, true);
            }
            else
            {
                TaxiRide.DisplayNotification($"~g~Driving Style", $"Updated driving style to {drivingStyleScroller.SelectedItem.Name}");
            }
            drivingStyleScroller.SelectedItem.HasBeenPurchased = true;
            TaxiRide.RespondingDriver.TaxiRide.TaxiDrivingStyle = drivingStyleScroller.SelectedItem;
            TaxiRide.RespondingDriver.PlaySpeech("TAXID_SPEED_UP", false);
            sender.Visible = false;
        };
        VehicleInteractMenu.AddItem(drivingStyleScroller);
    }
    private void AddOtherOptions()
    {



        if(TaxiRide == null)
        {
            return;
        }

        if(TaxiRide.IsWaitingOnPlayer)
        {
            UIMenuItem continueRide = new UIMenuItem("Continue", "Tell the driver to continue the ride.");
            continueRide.Activated += (sender, e) =>
            {
                TaxiRide.ContinueRide();
                VehicleInteractMenu.Visible = false;
            };
            if (TaxiRide != null)
            {
                VehicleInteractMenu.AddItem(continueRide);
            }
        }
        else
        {
            UIMenuItem StopVehicle = new UIMenuItem("Pullover", "Tell the driver to pullover here.");
            StopVehicle.Activated += (sender, e) =>
            {
                TaxiRide.Pullover();
                VehicleInteractMenu.Visible = false;
            };
            if (TaxiRide != null)
            {
                VehicleInteractMenu.AddItem(StopVehicle);
            }
        }
        UIMenuItem CancelRide = new UIMenuItem("Cancel Ride", "Cancel the current ride.");
        CancelRide.Activated += (sender, e) =>
        {
            Player.TaxiManager.CancelRide(TaxiRide, true);
            VehicleInteractMenu.Visible = false;
        };
        if (TaxiRide != null && !TaxiRide.HasArrivedAtDestination)
        {
            VehicleInteractMenu.AddItem(CancelRide);
        }

    }
    private void SetMenuBanner(UIMenu toSet)
    {
        if(TaxiRide != null && TaxiRide.RequestedFirm != null && TaxiRide.RequestedFirm.HasBannerImage)
        {
            if(TaxiRide.RequestedFirm.BannerImage == null)
            {
                TaxiRide.RequestedFirm.BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{TaxiRide.RequestedFirm.BannerImagePath}");
            }
            toSet.SetBannerType(TaxiRide.RequestedFirm.BannerImage);
            Game.RawFrameRender += (s, e) => MenuPool.DrawBanners(e.Graphics);
        }
        else
        {
            toSet.SetBannerType(EntryPoint.LSRedColor);
        }
    }
    private void CreateInteractionMenuTaxi()
    {
        MenuPool = new MenuPool();

        string TitleMenuString = "Taxi";
        if(TaxiRide != null && TaxiRide.RequestedFirm != null && TaxiRide.RequestedFirm.IsRideShare)
        {
            TitleMenuString = "Ride Share";
        }

        VehicleInteractMenu = new UIMenu(TitleMenuString, $"Destination: {(TaxiRide == null ? "None" : TaxiRide.DestinationName)}");
        SetMenuBanner(VehicleInteractMenu);
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

                Game.RawFrameRender -= (s, e) => MenuPool.DrawBanners(e.Graphics);

            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "VehicleInteraction");
    }
}

