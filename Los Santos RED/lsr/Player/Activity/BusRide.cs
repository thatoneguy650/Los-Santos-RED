using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

public class BusRide
{
    private IBusRideable Player;
    private Vehicle Bus;
    private IEntityProvideable World;
    private IPlacesOfInterest PlacesOfInterest;
    private BusRoute ChosenRoute;
    private ISettingsProvideable Settings;

    private MenuPool MenuPool;
    private UIMenu BusRouteMenu;
    private UIMenuItem ShowRouteMenuItem;

    private bool HasRoute => ChosenRoute != null && ChosenRoute.HasRoute;
    public BusRide(IBusRideable player, Vehicle bus, IEntityProvideable world, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings)
    {
        Player = player;
        Bus = bus;
        World = world;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
    }
    public void Start()
    {
        try
        {
            Player.IsRidingBus = true;
            GetRoute();
            CreateMenu();
            if (HasRoute)
            {
                GameFiber BusRideFiber = GameFiber.StartNew(delegate
                {
                    try
                    {
                        GameFiber.Yield();
                        HandleRide();
                        Player.IsRidingBus = false;
                    }
                    catch (Exception ex)
                    {
                        EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                        EntryPoint.ModController.CrashUnload();
                    }
                }, "BusRide");
            }
            else
            {
                Player.IsRidingBus = false;
            }
        }
        catch (Exception e)
        {
            Player.IsRidingBus = false;
            EntryPoint.WriteToConsole("BusRide" + e.Message + e.StackTrace, 0);
        }
    }
    private void CreateMenu()
    {
        MenuPool = new MenuPool();
        BusRouteMenu = new UIMenu("Bus Route", "Select an Option");
        BusRouteMenu.RemoveBanner();
        MenuPool.Add(BusRouteMenu);


        UIMenuItem ShowDestinationStopMenuItem = new UIMenuItem("Show Next Stop", "Show info abou the next stop.");
        ShowDestinationStopMenuItem.Activated += (menu, item) =>
        {
            ChosenRoute.DisplayDestinationInfo();
        };

        ShowRouteMenuItem = new UIMenuItem("Show Route Info","Show info about the bus route.");
        ShowRouteMenuItem.Activated += (menu, item) =>
        {
            ChosenRoute.DisplayCurrentRouteInfo();
        };

        UIMenuItem SkipRideMenu = new UIMenuItem("Skip Ride","Skip the ride and immediately arrive at the next stop.");
        SkipRideMenu.Activated += (menu, item) =>
        {
            Game.DisplaySubtitle("Skip Ride Selected");
        };

        UIMenuListScrollerItem<BusRouteStop> ShowStopInfo = new UIMenuListScrollerItem<BusRouteStop>("Show Stop Info","Show info about a specific stop.",ChosenRoute.RouteStops);
        ShowStopInfo.Activated += (menu, item) =>
        {
            ShowStopInfo.SelectedItem.DisplayStopNotification();
        };

        BusRouteMenu.AddItem(ShowDestinationStopMenuItem);
        BusRouteMenu.AddItem(ShowRouteMenuItem);
        BusRouteMenu.AddItem(ShowStopInfo);
        BusRouteMenu.AddItem(SkipRideMenu);
    }
    private void HandleRide()
    {
        if (Bus.Exists())
        {
            //EntryPoint.WriteToConsole("PLAYER EVENT: BusRide Start");
            FixPassengers();
            Player.LastFriendlyVehicle = Bus;
            while (!Player.IsInVehicle)
            {
                GameFiber.Yield();
            }
            GameFiber.Sleep(5000);
            if (Player.IsInVehicle)
            {
                Player.IsRidingBus = true;
                TaskDriver();
                MonitorRide();
                Player.IsRidingBus = false;
                //EntryPoint.WriteToConsole("PLAYER EVENT: BusRide End");
            }
            else
            {
                //EntryPoint.WriteToConsole("PLAYER EVENT: BusRide End (Timeout entry)", 3);
            }
        }
    }
    private void MonitorRide()
    {
        Player.ButtonPrompts.AddPrompt("BusRideActivity", "Route Info", "BusRouteInfo", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 500);
        if (HasRoute)
        {
            ChosenRoute.OnRouteStart();
            Player.GPSManager.AddGPSRoute("Bus Destination", ChosenRoute.DestinationStop.BusStop.EntrancePosition);
        }
        while (Player.IsAliveAndFree && Player.IsInVehicle && Player.IsNotWanted && HasRoute)
        {
            if(ChosenRoute.DestinationStop?.BusStop != null && ChosenRoute.DestinationStop.BusStop.DistanceToPlayer <= 15f && Player.VehicleSpeedMPH <= 2f)
            {
                ChosenRoute.OnArrivedAtStop();
                TaskDriver();
                Player.GPSManager.AddGPSRoute("Bus Destination", ChosenRoute.DestinationStop.BusStop.EntrancePosition);
            }
            if (Player.ButtonPrompts.IsPressed("BusRouteInfo"))
            {
                BusRouteMenu.Visible = true;
            }
            MenuPool.ProcessMenus();
            GameFiber.Yield();
        }
        Player.ButtonPrompts.RemovePrompts("BusRideActivity");

        Player.GPSManager.RemoveGPSRoute();

    }
    private void TaskDriver()
    {
        if (Bus.Driver.Exists() && HasRoute)
        {
            PedExt BusDriver = World.Pedestrians.GetPedExt(Bus.Driver.Handle);
            NativeFunction.Natives.CLEAR_PED_TASKS(Bus.Driver);
            Bus.Driver.BlockPermanentEvents = true;
            Bus.Driver.KeepTasks = true;
            BusDriver.CanBeAmbientTasked = false;
            if (ChosenRoute.DestinationStop?.BusStop != null)
            {
                Vector3 taskedPosition = ChosenRoute.DestinationStop.BusStop.EntrancePosition;
                unsafe
                {
                    int lol = 0;
                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                    NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(5000, 8000));
                    NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", 0, Bus, taskedPosition.X, taskedPosition.Y, taskedPosition.Z, 12f, (int)eCustomDrivingStyles.RegularDriving, 5f);
                    NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(10000, 15000));
                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Bus.Driver, lol);
                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                }
               // EntryPoint.WriteToConsole("PLAYER EVENT: BusRide Tasked Driver");
            }
        }
    }
    private void FixPassengers()
    {
        if (Bus.Exists())
        {
            foreach (Ped passenger in Bus.Occupants)
            {
                if (passenger.Exists())
                {
                    passenger.BlockPermanentEvents = true;
                    passenger.StaysInVehiclesWhenJacked = true;
                }
            }
        }
    }
    private void GetRoute()
    {
        ChosenRoute = new BusRoute("Test Bus Route", PlacesOfInterest);
        ChosenRoute.AddStops();
    }
}