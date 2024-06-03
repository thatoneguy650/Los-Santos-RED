using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class ReturnToStationVehicleTaskState : TaskState
{
    private PedExt PedGeneral;
    private IEntityProvideable World;
    private IPlacesOfInterest PlacesOfInterest;
    private bool HasArrivedAtStation;
    private Vector3 taskedPosition;
    private ISettingsProvideable Settings;
    private bool BlockPermanentEvents = false;
    private bool IsNearStation;
    private PoliceStation closestPoliceStation;

    public ReturnToStationVehicleTaskState(PedExt pedGeneral, IEntityProvideable world, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, bool blockPermanentEvents)
    {
        PedGeneral = pedGeneral;
        World = world;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        BlockPermanentEvents = blockPermanentEvents;
    }

    public bool IsValid => PedGeneral != null && PedGeneral.Pedestrian.Exists() && PedGeneral.IsInVehicle && HasArrestedPassengers() && !HasArrivedAtStation;
    public string DebugName => $"WanderInVehicleTaskState HasArrivedAtStation {HasArrivedAtStation}";
    public void Dispose()
    {
        PedGeneral.ClearTasks(true);
        Stop();
    }
    public void Start()
    {
        PedGeneral.ClearTasks(true);
        TaskReturnToStation();
    }
    public void Stop()
    {

    }
    public void Update()
    {
        if(!IsNearStation && PedGeneral.Pedestrian.DistanceTo2D(taskedPosition) < 50f)
        {
            IsNearStation = true;
            TaskReturnToStation();//retask to get correct parking spot
        }



        if (!HasArrivedAtStation && PedGeneral.Pedestrian.DistanceTo2D(taskedPosition) < 10f && PedGeneral.Pedestrian.CurrentVehicle.Exists() && PedGeneral.Pedestrian.CurrentVehicle.Speed <= 1.0f && !PedGeneral.Pedestrian.CurrentVehicle.IsEngineOn)//arrived, wait then drive away
        {
            HasArrivedAtStation = true;
            foreach (Ped ped in PedGeneral.Pedestrian.CurrentVehicle.Passengers)
            {
                if(ped.Exists())
                {
                    ped.Delete();
                }
            }
            //EntryPoint.WriteToConsole($"EVENT: ReturnToStationVehicleTaskState HasArrivedAtStation {PedGeneral.Pedestrian.Handle}", 3);
        }
    }
    private void TaskReturnToStation()
    {
        if(!PedGeneral.Pedestrian.Exists())
        {
            return;
        }
        if (BlockPermanentEvents)
        {
            PedGeneral.Pedestrian.BlockPermanentEvents = true;
            PedGeneral.Pedestrian.KeepTasks = true;
        }
        if ((PedGeneral.IsDriver || PedGeneral.Pedestrian.SeatIndex == -1) && PedGeneral.Pedestrian.CurrentVehicle.Exists())
        {
            if (closestPoliceStation == null)
            {
                closestPoliceStation = PlacesOfInterest.PossibleLocations.PoliceStations.OrderBy(x => PedGeneral.Pedestrian.DistanceTo2D(x.EntrancePosition)).FirstOrDefault();
            }
            if (closestPoliceStation != null)
            {
                ConditionalLocation parkingSpot = null;//closestPoliceStation.PossibleVehicleSpawns.PickRandom();

                if(closestPoliceStation.PossibleVehicleSpawns != null)
                {
                    if (closestPoliceStation.EntrancePosition.DistanceTo2D(PedGeneral.Pedestrian.Position) >= 150f)
                    {
                        parkingSpot = closestPoliceStation.PossibleVehicleSpawns.PickRandom();
                    }
                    else
                    {
                        foreach (ConditionalLocation cl in closestPoliceStation.PossibleVehicleSpawns)
                        {
                            if (!World.Vehicles.AllVehicleList.Any(x => x.Vehicle.Exists() && x.Vehicle.DistanceTo2D(cl.Location) <= 4.0f))
                            {
                                parkingSpot = cl;
                                break;
                            }
                        }
                    }
                }
                if(parkingSpot != null)
                {
                    taskedPosition = parkingSpot.Location;
                    unsafe
                    {
                        int lol = 0;
                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        //NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(4000, 8000));
                        NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", 0, PedGeneral.Pedestrian.CurrentVehicle, taskedPosition.X, taskedPosition.Y, taskedPosition.Z, 12f, (int)eCustomDrivingStyles.RegularDriving, 20f);
                        NativeFunction.CallByName<bool>("TASK_VEHICLE_PARK", 0, PedGeneral.Pedestrian.CurrentVehicle, taskedPosition.X, taskedPosition.Y, taskedPosition.Z,parkingSpot.Heading,1,20f,false);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, Ped.Pedestrian.CurrentVehicle, 10f, (int)(VehicleDrivingFlags.FollowTraffic | VehicleDrivingFlags.YieldToCrossingPedestrians | VehicleDrivingFlags.RespectIntersections | (VehicleDrivingFlags)8), 10f);
                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", PedGeneral.Pedestrian, lol);
                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    }
                    //EntryPoint.WriteToConsoleTestLong("Return to Station With Parking Spot");
                }
                else
                {
                    taskedPosition = NativeHelper.GetStreetPosition(closestPoliceStation.EntrancePosition, false);
                    NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", PedGeneral.Pedestrian, PedGeneral.Pedestrian.CurrentVehicle, taskedPosition.X, taskedPosition.Y, taskedPosition.Z, 12f, (int)eCustomDrivingStyles.RegularDriving, 20f);//NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", Ped.Pedestrian, Ped.Pedestrian.CurrentVehicle, taskedPosition.X, taskedPosition.Y, taskedPosition.Z, 12f, (int)(VehicleDrivingFlags.FollowTraffic | VehicleDrivingFlags.YieldToCrossingPedestrians | VehicleDrivingFlags.RespectIntersections | (VehicleDrivingFlags)8), 20f);
                    //EntryPoint.WriteToConsoleTestLong("Return to Station Without Parking Spot");
                }
                    
            }
            
        }
    }
    public bool HasArrestedPassengers()
    {
        if (PedGeneral.IsDriver && PedGeneral.Pedestrian.CurrentVehicle.Exists())
        {
            foreach (Ped ped in PedGeneral.Pedestrian.CurrentVehicle.Passengers)
            {
                PedExt pedExt = World.Pedestrians.GetPedExt(ped.Handle);
                if (pedExt != null && pedExt.IsArrested)
                {
                    return true;
                }
                if (ped.Handle == Game.LocalPlayer.Character.Handle)
                {
                    return true;
                }
            }
        }
        return false;
    }
}

