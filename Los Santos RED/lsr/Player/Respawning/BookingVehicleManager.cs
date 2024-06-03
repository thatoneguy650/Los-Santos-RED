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


public class BookingVehicleManager
{
    private IRespawnable Player;
    private IEntityProvideable World;
    private IPoliceRespondable PoliceRespondable;
    private ILocationRespawnable Location;
    private ISettingsProvideable Settings;
    private BookingActivity BookingActivity;
    private Cop Cop;
    private Vector3 AttachOffset;
    private bool IsCancelled;
    private SeatAssigner SeatAssigner;
    private int SeatTryingToEnter;
    private int DoorTryingToEnter;
    private Vector3 SeatTryingToEnterEntryPosition;
    private VehicleExt VehicleTryingToEnter;
    private Vehicle VehicleTaskedToEnter;
    private int SeatTaskedToEnter;
    private bool hasEnteredVehicle;
    private bool isWalking;
    private string PlayerCuffedDictionary;
    private string PlayerCuffedAnimation;
    private bool canEnterCar;
    private bool isAttached;
    private uint GameTimeLastTaskedOpenDoor;

    public bool IsActive { get; private set; }
    public BookingVehicleManager(IRespawnable player, IEntityProvideable world, IPoliceRespondable policeRespondable, ILocationRespawnable location, ISeatAssignable seatAssignable, ISettingsProvideable settings, BookingActivity bookingActivity, Cop cop)
    {
        Player = player;
        World = world;
        PoliceRespondable = policeRespondable;
        Location = location;
        Settings = settings;
        BookingActivity = bookingActivity;
        Cop = cop;
        SeatAssigner = new SeatAssigner(seatAssignable, World, World.Vehicles.SimplePoliceVehicles);
    }
    public void Setup()
    {
        AttachOffset = new Vector3(-0.5f, 0.5f, 0.04f); //new Vector3(-0.31f, 0.12f, 0.04f);
        if(Cop != null)
        {
            Cop.CanBeAmbientTasked = true;
            Cop.CanBeTasked = true;
            Cop.CurrentTask = null;
        }
    }
    public void Start()
    {
        canEnterCar = false;
        bool hasOpenedDoor = false;
        uint GameStartEntered = 0;
        uint GameTimeOpenedDoor = 0;
        while (!Player.IsInVehicle && BookingActivity.CanContinueBooking)// && !Game.IsKeyDown(System.Windows.Forms.Keys.I))
        {
            PlayerEnterCarLoop();
            if(VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists() && Cop.Pedestrian.Exists())
            {
                float doorAngle = NativeFunction.Natives.GET_VEHICLE_DOOR_ANGLE_RATIO<float>(VehicleTryingToEnter.Vehicle, DoorTryingToEnter);//VehicleTryingToEnter.Vehicle.GetDoors()[DoorTryingToEnter].IsOpen;
                bool isOpen = doorAngle > 0;
                float distanceTo = VehicleTryingToEnter.Vehicle.DistanceTo(Game.LocalPlayer.Character);
                if(!isOpen && Game.GameTime - GameTimeLastTaskedOpenDoor >= 2000 && distanceTo <= 10f)
                {
                    EntryPoint.WriteToConsole("BOOKING ACTIVITY OPEN DOOR RAN");
                    NativeFunction.Natives.TASK_OPEN_VEHICLE_DOOR(Cop.Pedestrian, VehicleTaskedToEnter, -1, SeatTaskedToEnter, 2.0f);
                    hasOpenedDoor = true;
                    GameTimeLastTaskedOpenDoor = Game.GameTime;
                }
                if (isOpen && distanceTo <= 10f)
                {
                    if (GameTimeOpenedDoor == 0)
                    {
                        GameTimeOpenedDoor = Game.GameTime;
                        //ReleasePeds();
                        //EntryPoint.WriteToConsole("BOOKING ACTIVITY OPEN DOOR RELEASE PEDS RAN");
                    }
                }
                else
                {
                    GameTimeOpenedDoor = 0;
                }
                if(isOpen && distanceTo <= 10f && GameTimeOpenedDoor != 0 && Game.GameTime - GameTimeOpenedDoor >= 1500 && Game.GameTime - GameStartEntered >= 3000)
                {
                    EntryPoint.WriteToConsole("BOOKING ACTIVITY ENTER VEHICLE RAN 1!");
                    EnterVehicle();
                    //ReleasePeds();
                    //canEnterCar = true;
                    //break;
                    GameStartEntered = Game.GameTime;
                }
                if(Player.IsInVehicle)
                {
                    ReleasePeds();
                    break;
                }
                //Game.DisplaySubtitle($"Seat {SeatTryingToEnter} Door {DoorTryingToEnter} doorAngle {doorAngle} Open {isOpen} Dist {distanceTo}");
            }
            GameFiber.Yield();
        }
        EntryPoint.WriteToConsole("BOOKING ACTIVITY ENDED MAIN LOOP!");
        if (BookingActivity.CanContinueBooking && Player.IsInVehicle)
        {
            Player.IsArrested = true;
            Player.SetNotBusted();
            Player.SetWantedLevel(0, "Handcuffed", true);
            EntryPoint.WriteToConsole("BOOKING ACTIVITY CVEHICLE FINAL RAN");
        }
        //ReleasePeds();
        //EnterVehicle();
    }
    private void EnterVehicle()
    {
        if (VehicleTryingToEnter.Vehicle.Exists())
        {
            EntryPoint.WriteToConsole("BOOKING ACTIVITY ENTER VEHICLE RAN 2!");
            NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", Game.LocalPlayer.Character, VehicleTryingToEnter.Vehicle, -1, SeatTryingToEnter, 1f, 9);
            GameFiber.Sleep(3000);
        }
    }
    private void ReleasePeds()
    {
        if (Cop.Pedestrian.Exists())
        {
            Cop.CanBeTasked = true;
            Cop.CanBeAmbientTasked = true;
        }
    }
    private void PlayerEnterCarLoop()
    {
        if (VehicleTaskedToEnter == null || !VehicleTaskedToEnter.Exists())
        {
            ReTaskCarLoop();
        }
        else if (!VehicleTaskedToEnter.IsSeatFree(SeatTaskedToEnter))// && VehicleTaskedToEnter.GetPedOnSeat(SeatTaskedToEnter).Exists() && VehicleTaskedToEnter.GetPedOnSeat(SeatTaskedToEnter).Handle != Player.Character.Handle)
        {
            ReTaskCarLoop();
        }
        else if (VehicleTaskedToEnter.Speed > 1.0f)
        {
            ReTaskCarLoop();
        }
    }
    private void ReTaskCarLoop()
    {
        GetClosesetPoliceVehicle();
        GetInCarTask();
    }
    private void GetInCarTask()
    {
        if (Player.Character.Exists() && VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists() && Cop.Pedestrian.Exists())
        {
            VehicleTaskedToEnter = VehicleTryingToEnter.Vehicle;
            SeatTaskedToEnter = SeatTryingToEnter;
            float vehicleHEading = VehicleTaskedToEnter.Heading;
            Player.LastFriendlyVehicle = VehicleTryingToEnter.Vehicle;
            Vector3 TargetPosition = NativeFunction.Natives.GET_ENTRY_POINT_POSITION<Vector3>(VehicleTryingToEnter.Vehicle, DoorTryingToEnter);
            float TargetHeading = vehicleHEading;// Game.LocalPlayer.Character.Heading;


            bool isEven = DoorTryingToEnter % 2 == 0;

            TargetPosition = NativeHelper.GetOffsetPosition(TargetPosition, isEven ? vehicleHEading - 90f : vehicleHEading + 90f, 1.0f);

            //NativeHelper.GetOffsetPosition(TargetPosition)

            NativeFunction.Natives.TASK_FOLLOW_NAV_MESH_TO_COORD(Game.LocalPlayer.Character, TargetPosition.X, TargetPosition.Y, TargetPosition.Z, 1.0f, -1, 0.1f, 0, TargetHeading);
            //NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", Game.LocalPlayer.Character, VehicleTryingToEnter.Vehicle, -1, SeatTryingToEnter, 1f, 9);
            //PedVehicleInteract PedVehicleInteract = new PedVehicleInteract(Player, Cop, -0.9f, VehicleTaskedToEnter, Settings);
            //PedVehicleInteract.Start();
           // VehicleDoorSeatData vdsd = new VehicleDoorSeatData("unknow", "unknow", 5, SeatTaskedToEnter);
            //TargetPosition = vdsd.GetDoorOffset(VehicleTaskedToEnter, Settings);
            //TargetHeading = vdsd.GetDoorHeading(VehicleTaskedToEnter, Settings);
            //NativeFunction.Natives.TASK_OPEN_VEHICLE_DOOR(Player.Character, VehicleTaskedToEnter, -1, SeatTaskedToEnter, 1.0f);
        }
    }
    private void GetClosesetPoliceVehicle()
    {
        SeatAssigner.AssignPrisonerSeat();
        VehicleTryingToEnter = SeatAssigner.VehicleAssigned;
        SeatTryingToEnter = SeatAssigner.SeatAssigned;
        DoorTryingToEnter = SeatAssigner.GetDoorFromSeat(SeatTryingToEnter);
        EntryPoint.WriteToConsole($" SeatTryingToEnter:{SeatTryingToEnter} DoorTryingToEnter:{DoorTryingToEnter}");
    }
}