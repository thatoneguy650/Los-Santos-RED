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
using System.Xml.Schema;

public class TaxiInteractionMenu : VehicleInteractionMenu
{
    private TaxiVehicleExt TaxiVehicleExt;
    private TaxiRide TaxiRide;
    public TaxiInteractionMenu(VehicleExt vehicleExt, TaxiVehicleExt taxiVehicleExt) : base(vehicleExt)
    {
        TaxiVehicleExt = taxiVehicleExt;
    }
    public override void ShowInteractionMenu(IInteractionable player, IWeapons weapons, IModItems modItems, VehicleDoorSeatData vehicleDoorSeatData, IVehicleSeatAndDoorLookup vehicleSeatDoorData, IEntityProvideable world, ISettingsProvideable settings, bool showDefault)
    {
        if (showDefault)
        {
            base.ShowInteractionMenu(player,weapons,modItems, vehicleDoorSeatData, vehicleSeatDoorData, world, settings, true);
            return;
        }

        VehicleDoorSeatData = vehicleDoorSeatData;
        Player = player;

        TaxiRide = Player.TaxiManager.GetOrCreateRide(TaxiVehicleExt);
        if(TaxiRide == null)
        {
            Player.ButtonPrompts.RemovePrompts("VehicleInteract");
            return;
        }
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


        Vector3 MarkerPosOrig = Player.GPSManager.GetGPSRoutePosition();
        SpawnLocation spawnLocation2 = new SpawnLocation(MarkerPosOrig);
        spawnLocation2.GetClosestStreet(false);
        int guessPrice = GetPrice(Player.Character.Position.DistanceTo2D(spawnLocation2.StreetPosition));

        UIMenu DestinationSubMenu = MenuPool.AddSubMenu(VehicleInteractMenu, "Destinations");
        VehicleInteractMenu.MenuItems[VehicleInteractMenu.MenuItems.Count() - 1].Description = "Set the current chosen destination.";
        DestinationSubMenu.SetBannerType(EntryPoint.LSRedColor);
        UIMenuItem GoToMarker = new UIMenuItem("Marker", "Drive to the current marker") { RightLabel = $"~r~${guessPrice}" };
        GoToMarker.Activated += (sender, e) =>
        {
            if(TaxiRide == null)
            {
                EntryPoint.WriteToConsole("Add Destination, no taxi ride found");
                return;
            }
            Vector3 MarkerPos = Player.GPSManager.GetGPSRoutePosition();
            SpawnLocation spawnLocation = new SpawnLocation(MarkerPos);
            spawnLocation.GetClosestStreet(false);
            if(!spawnLocation.HasStreetPosition)
            {
                EntryPoint.WriteToConsole("Add Destination, no street pos");
                return;
            }
            if(MarkerPos == Vector3.Zero)
            {
                EntryPoint.WriteToConsole("Add Destination, marker pos not found");
                return;
            }


            int guessPrice2 = GetPrice(Player.Character.Position.DistanceTo2D(spawnLocation.StreetPosition));

            Game.DisplaySubtitle($"Total New Price = {guessPrice2}");
            TaxiRide.UpdateDestination(MarkerPos);
        };
        DestinationSubMenu.AddItem(GoToMarker);
    }
    private int GetPrice(float distance)
    {
        int totalFare = 20;
        float smallerDistance = distance / 100f;
        int AdditionalFare = (int)Math.Floor(smallerDistance) * 3;
        return totalFare + AdditionalFare;
    }
    private void AddDrivingStyleMenu()
    {


        List<PedDrivingStyle> PossibleStyles = new List<PedDrivingStyle>() { 
            new PedDrivingStyle("Normal", eCustomDrivingStyles.RegularDriving, 10f), 
            new PedDrivingStyle("Fast", eCustomDrivingStyles.RegularDriving, 40f), 
            new PedDrivingStyle("Crazy", eCustomDrivingStyles.Code3, 70f), };
        UIMenu DrivingStyle = MenuPool.AddSubMenu(VehicleInteractMenu, "Driving Style");
        VehicleInteractMenu.MenuItems[VehicleInteractMenu.MenuItems.Count() - 1].Description = "Set the driving style.";
        DrivingStyle.SetBannerType(EntryPoint.LSRedColor);
        UIMenuListScrollerItem<PedDrivingStyle> drivingStyleScroller = new UIMenuListScrollerItem<PedDrivingStyle>("Styles","Choose a style", PossibleStyles);
        drivingStyleScroller.Activated += (sender,selectedItem) =>
        {
            if(TaxiRide == null || TaxiRide.RespondingDriver == null)
            {
                return;
            }
            TaxiRide.RespondingDriver.TaxiDrivingStyle = drivingStyleScroller.SelectedItem;
        };
        DrivingStyle.AddItem(drivingStyleScroller);
    }
    private void AddOtherOptions()
    {
        UIMenuItem CancelRide = new UIMenuItem("Cancel Ride", "Cancel the current ride.");
        CancelRide.Activated += (sender, e) =>
        {
            Player.TaxiManager.CancelRide(TaxiRide);
            VehicleInteractMenu.Visible = false;
        };
        VehicleInteractMenu.AddItem(CancelRide);
    }
    private void CreateInteractionMenuTaxi()
    {
        MenuPool = new MenuPool();
        VehicleInteractMenu = new UIMenu("Taxi", "Destination: None");
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

