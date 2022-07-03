using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

public class BusRide
{
    private int BusGroupID;
    private IBusRideable Player;
    private Vehicle Bus;
    private IEntityProvideable World;
    private IPlacesOfInterest PlacesOfInterest;
    private BusStop Stop1;
    private BusStop Stop2;
    private BusStop CurrentStop;
    private bool HasReachedStop1;
    private bool HasReachedStop2;
    public BusRide(IBusRideable player, Vehicle bus, IEntityProvideable world, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        Bus = bus;
        World = world;
        PlacesOfInterest = placesOfInterest;
    }
    public void Start()
    {
        try
        {
            Player.IsRidingBus = true;
            Setup();
            if (Stop1 != null && Stop2 != null)
            {
                GameFiber BusRideFiber = GameFiber.StartNew(delegate
                {
                    GameFiber.Yield();
                    if (Bus.Exists())
                    {
                        EntryPoint.WriteToConsole("PLAYER EVENT: BusRide Start", 3);
                        foreach (Ped passenger in Bus.Occupants)
                        {
                            if (passenger.Exists())
                            {
                                passenger.BlockPermanentEvents = true;
                                passenger.StaysInVehiclesWhenJacked = true;
                            }
                        }
                        Player.LastFriendlyVehicle = Bus;
                        while (Player.IsGettingIntoAVehicle)
                        {
                            GameFiber.Yield();
                        }
                        GameFiber.Sleep(5000);
                        if (Player.IsInVehicle)
                        {
                            DisplayNextStop(Stop1);
                            Player.IsRidingBus = true;
                            if (Bus.Driver.Exists())
                            {
                                PedExt BusDriver = World.Pedestrians.GetPedExt(Bus.Driver.Handle);
                                Bus.Driver.BlockPermanentEvents = true;
                                Bus.Driver.KeepTasks = true;
                                BusDriver.CanBeAmbientTasked = false;
                                Vector3 taskedPosition = Stop1.EntrancePosition;
                                Vector3 taskedPosition2 = Stop2.EntrancePosition;
                                unsafe
                                {
                                    int lol = 0;
                                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                    NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", 0, Bus, taskedPosition.X, taskedPosition.Y, taskedPosition.Z, 12f, (int)eCustomDrivingStyles.RegularDriving, 5f);
                                    NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(4000, 8000));
                                    NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", 0, Bus, taskedPosition2.X, taskedPosition2.Y, taskedPosition2.Z, 12f, (int)eCustomDrivingStyles.RegularDriving, 5f);
                                    NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(4000, 8000));
                                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Bus.Driver, lol);
                                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                                }
                                EntryPoint.WriteToConsole("PLAYER EVENT: BusRide Tasked Driver", 3);
                            }
                            bool isFinished = false;
                            while (Player.IsInVehicle && !isFinished)
                            {
                                if (CurrentStop.Name == Stop1.Name && Stop1.DistanceToPlayer <= 15f && Player.VehicleSpeedMPH <= 2f && !HasReachedStop1)
                                {
                                    DisplayArrivedAtStop(Stop1);
                                    DisplayNextStop(Stop2);
                                    CurrentStop = Stop2;
                                    HasReachedStop1 = true;
                                    HasReachedStop2 = false;
                                }
                                else if (CurrentStop.Name == Stop2.Name && Stop2.DistanceToPlayer <= 15f && Player.VehicleSpeedMPH <= 2f && !HasReachedStop2)
                                {
                                    DisplayArrivedAtStop(Stop2);
                                    DisplayNextStop(Stop1);
                                    CurrentStop = Stop1;
                                    HasReachedStop2 = true;
                                    HasReachedStop2 = false;
                                }
                                GameFiber.Yield();
                            }
                            Player.IsRidingBus = false;
                            EntryPoint.WriteToConsole("PLAYER EVENT: BusRide End", 3);
                        }
                        else
                        {
                            EntryPoint.WriteToConsole("PLAYER EVENT: BusRide End (Timeout entry)", 3);
                        }
                    }
                    Player.IsRidingBus = false;
                }, "BusRide");
            }

        }
        catch (Exception e)
        {
            Player.IsRidingBus = false;
            EntryPoint.WriteToConsole("BusRide" + e.Message + e.StackTrace, 0);
        }
    }
    private void Setup()
    {
        Stop1 = PlacesOfInterest.PossibleLocations.BusStops.PickRandom();
        if (Stop1 != null)
        {
            Stop2 = PlacesOfInterest.PossibleLocations.BusStops.Where(x => x.Name != Stop1.Name).PickRandom();
        }
        if(Stop1 != null)
        {
            CurrentStop = Stop1;
        }
    }
    private void DisplayArrivedAtStop(BusStop busStop)
    {
        Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~g~Arriving", busStop.Name, "If this is your stop, please make your way off the bus.");
    }
    private void DisplayNextStop(BusStop busStop)
    {
        Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~g~Next Stop", busStop.Name, $"Continuing Bus Route Service. {busStop.FullStreetAddress}");
    }


}