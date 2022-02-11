using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionsMethods;
using LSR.Vehicles;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using Mod;
using LosSantosRED.lsr.Interface;

public class BusRide
{
    private int BusGroupID;
    private IBusRideable Player;
    private Vehicle Bus;
    private IEntityProvideable World;
    public BusRide(IBusRideable player, Vehicle bus, IEntityProvideable world)
    {
        Player = player;
        Bus = bus;
        World = world;
    }
    public void Start()
    {
        try
        {
            GameFiber BusRideFiber = GameFiber.StartNew(delegate
            {
                GameFiber.Yield();
                if (Bus.Exists())
                {
                    EntryPoint.WriteToConsole("PLAYER EVENT: BusRide Start", 3);
                    BusGroupID = NativeFunction.Natives.CREATE_GROUP<int>(0);
                    NativeFunction.Natives.SET_PED_AS_GROUP_LEADER(Player.Character, BusGroupID);
                    NativeFunction.Natives.SET_PED_AS_GROUP_MEMBER(Player.Character, BusGroupID);
                    foreach (Ped passenger in Bus.Passengers)
                    {
                        if (passenger.Exists())
                        {
                            NativeFunction.Natives.SET_PED_AS_GROUP_MEMBER(passenger, BusGroupID);
                            passenger.StaysInVehiclesWhenJacked = true;
                        }
                    }
                    if (Bus.Driver.Exists())
                    {
                        NativeFunction.Natives.SET_PED_AS_GROUP_MEMBER(Bus.Driver, BusGroupID);
                        Bus.Driver.StaysInVehiclesWhenJacked = true;
                    }
                    while (Player.IsGettingIntoAVehicle)
                    {
                        GameFiber.Yield();
                    }
                    GameFiber.Sleep(5000);
                    if (Player.IsInVehicle)
                    {
                        Player.IsRidingBus = true;
                        if(Bus.Driver.Exists())
                        {
                            PedExt BusDriver = World.Pedestrians.GetPedExt(Bus.Driver.Handle);
                            Bus.Driver.BlockPermanentEvents = true;
                            Bus.Driver.KeepTasks = true;
                            BusDriver.CanBeAmbientTasked = false;
                            Vector3 taskedPosition = new Vector3(307.3152f, -766.6166f, 29.24787f);
                            Vector3 taskedPosition2 = new Vector3(355.6272f, -1064.027f, 28.86697f);
                            unsafe
                            {
                                int lol = 0;
                                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                                NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(4000, 8000));
                                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", 0, Bus, taskedPosition.X, taskedPosition.Y, taskedPosition.Z, 12f, (int)VehicleDrivingFlags.FollowTraffic, 20f);
                                NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(4000, 8000));
                                NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE", 0, Bus, taskedPosition2.X, taskedPosition2.Y, taskedPosition2.Z, 12f, (int)VehicleDrivingFlags.FollowTraffic, 20f);
                                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Bus.Driver, lol);
                                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                            }
                            EntryPoint.WriteToConsole("PLAYER EVENT: BusRide Tasked Driver", 3);
                        }
                    
                            while (Player.IsInVehicle)
                            {
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
            }, "BusRide");
        }
        catch (Exception e)
        {
            Player.IsRidingBus = false;
            EntryPoint.WriteToConsole("BusRide" + e.Message + e.StackTrace, 0);
        }
    }

    //new GameLocation(new Vector3(307.3152f, -766.6166f, 29.24787f), 155.4713f, LocationType.BusStop, "PillBoxHospitalStop", ""),
  //  new GameLocation(new Vector3(355.6272f, -1064.027f, 28.86697f), 270.2965f, LocationType.BusStop, "LaMesaPoliceStop1", ""),

}

