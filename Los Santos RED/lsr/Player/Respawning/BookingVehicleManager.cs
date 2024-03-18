using ExtensionsMethods;
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
        }
    }
    public void Start()
    {
        canEnterCar = false;
        bool hasOpenedDoor = false;
        uint GameTimeOpenedDoor = 0;
        while (!Player.IsInVehicle && BookingActivity.CanContinueBooking)// && !Game.IsKeyDown(System.Windows.Forms.Keys.I))
        {
            PlayerEnterCarLoop();
            if(VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists())
            {
                float doorAngle = NativeFunction.Natives.GET_VEHICLE_DOOR_ANGLE_RATIO<float>(VehicleTryingToEnter.Vehicle, DoorTryingToEnter);//VehicleTryingToEnter.Vehicle.GetDoors()[DoorTryingToEnter].IsOpen;
                bool isOpen = doorAngle > 0;
                float distanceTo = VehicleTryingToEnter.Vehicle.DistanceTo(Game.LocalPlayer.Character);
                if(!isOpen && !hasOpenedDoor && distanceTo <= 5f)
                {
                    NativeFunction.Natives.TASK_OPEN_VEHICLE_DOOR(Cop.Pedestrian, VehicleTaskedToEnter, -1, SeatTaskedToEnter, 1.0f);
                    hasOpenedDoor = true;
                }
                if (isOpen && distanceTo <= 5f)
                {
                    if(GameTimeOpenedDoor == 0)
                    {
                        GameTimeOpenedDoor = Game.GameTime;
                    }
                }
                else
                {
                    GameTimeOpenedDoor = 0;
                }

                if(isOpen && distanceTo <= 5f && GameTimeOpenedDoor != 0 && Game.GameTime - GameTimeOpenedDoor >= 2500)
                {
                    canEnterCar = true;
                    break;
                }
                Game.DisplaySubtitle($"Seat {SeatTryingToEnter} Door {DoorTryingToEnter} doorAngle {doorAngle} Open {isOpen} Dist {distanceTo}");
            }
            GameFiber.Yield();
        }
        if(BookingActivity.CanContinueBooking)
        {
            Player.SetNotBusted();
            Player.SetWantedLevel(0, "Handcuffed", true);
            Player.IsArrested = true;
        }
        ReleasePeds();
        EnterVehicle();
    }
    private void EnterVehicle()
    {
        if (canEnterCar && VehicleTryingToEnter.Vehicle.Exists())
        {
            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", Game.LocalPlayer.Character, VehicleTryingToEnter.Vehicle, -1, SeatTryingToEnter, 1f, 9);
            GameFiber.Sleep(3000);
        }
        //else
        //{
        //    NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        //}
        if (Player.IsInVehicle)
        {
            Player.SetNotBusted();
            Player.SetWantedLevel(0, "Handcuffed", true);
            Player.IsArrested = true;
        }
    }

    private void ReleasePeds()
    {
        if (Cop.Pedestrian.Exists())
        {
            //NativeFunction.Natives.CLEAR_PED_TASKS(Cop.Pedestrian);
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
            //Cop.Pedestrian.BlockPermanentEvents = true;
            //Cop.Pedestrian.KeepTasks = true;
            VehicleTaskedToEnter = VehicleTryingToEnter.Vehicle;
            SeatTaskedToEnter = SeatTryingToEnter;
            Player.LastFriendlyVehicle = VehicleTryingToEnter.Vehicle;



            Vector3 TargetPosition = NativeFunction.Natives.GET_ENTRY_POINT_POSITION<Vector3>(VehicleTryingToEnter.Vehicle, DoorTryingToEnter);
            float TargetHeading = Game.LocalPlayer.Character.Heading;


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
    }
}