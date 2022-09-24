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
        SeatAssigner = new SeatAssigner(seatAssignable, World, World.Vehicles.PoliceVehicleList);
    }
    public void Setup()
    {
        AttachOffset = new Vector3(-0.5f, 0.5f, 0.04f); //new Vector3(-0.31f, 0.12f, 0.04f);
        AnimationDictionary.RequestAnimationDictionay("move_action@generic@core");
        PlayerCuffedDictionary = "mp_arresting";
        PlayerCuffedAnimation = "idle";
        AnimationDictionary.RequestAnimationDictionay(PlayerCuffedDictionary);
        AnimationDictionary.RequestAnimationDictionay("doors@");
    }
    public void Start()
    {
        GetClosesetPoliceVehicle();
        Cop.Pedestrian.CollisionIgnoredEntity = Game.LocalPlayer.Character;
        Game.LocalPlayer.Character.CollisionIgnoredEntity = Cop.Pedestrian;

        NativeFunction.Natives.SET_PED_CONFIG_FLAG(Cop.Pedestrian, 225, false);//CPED_CONFIG_FLAG_DisablePotentialToBeWalkedIntoResponse 
        NativeFunction.Natives.SET_PED_CONFIG_FLAG(Cop.Pedestrian, 226, false);//CPED_CONFIG_FLAG_DisablePedAvoidance  

        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, PlayerCuffedDictionary, PlayerCuffedAnimation, 1.0f, -1.0f, -1, 1 | 16, 0, 0, 1, 0);

        //NativeFunction.Natives.TASK_FOLLOW_TO_OFFSET_OF_ENTITY(Player.Character, Cop.Pedestrian, -0.5f, -0.5f, 0f, 1.0f, -1, 10.0f, true);
        NativeFunction.Natives.SET_EVERYONE_IGNORE_PLAYER(Game.LocalPlayer, true);

       Cop.Pedestrian.Tasks.PlayAnimation("doors@", "door_sweep_r_hand_medium", 9f, AnimationFlags.StayInEndFrame | AnimationFlags.SecondaryTask | AnimationFlags.UpperBodyOnly).WaitForCompletion(1000);

        AttachPeds();
       // NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(Game.LocalPlayer.Character, Cop.Pedestrian, (int)PedBoneId.RightHand, 0.2f, 0.4f, 0f, 0f, 0f, 0f, true, true, false, false, 2, true);
        //NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(Game.LocalPlayer.Character, Cop.Pedestrian,(int)PedBoneId.RightHand, 0.2f, 0.4f, 0f, 0f, 0f, 0f, true, true, false, false, 2, true);

        Game.LocalPlayer.Character.IsCollisionEnabled = false;

        ReTaskCarLoop();
        bool canEnterCar = false;
        while (!Player.IsInVehicle && BookingActivity.CanContinueBooking)
        {
            HandlePlayerDistance();
            PlayerEnterCarLoop();


            if(VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists() && VehicleTryingToEnter.Vehicle.GetDoors()[DoorTryingToEnter].IsOpen && VehicleTryingToEnter.Vehicle.DistanceTo(Game.LocalPlayer.Character) <= 10f)
            {
                canEnterCar = true;
                break;
            }
            GameFiber.Yield();
        }
        if (Cop.Pedestrian.Exists())
        {
            Game.LocalPlayer.Character.Detach();
            Cop.Pedestrian.Detach();
            NativeFunction.Natives.SET_PED_RESET_FLAG(Cop.Pedestrian, 225, true);
            NativeFunction.Natives.SET_PED_RESET_FLAG(Cop.Pedestrian, 226, true);
            NativeFunction.Natives.CLEAR_PED_TASKS(Cop.Pedestrian);
            Cop.CanBeTasked = true;
            Cop.CanBeAmbientTasked = true;
            Cop.Pedestrian.CollisionIgnoredEntity = null;
        }
        else
        {
            Game.LocalPlayer.Character.Detach();
        }
        Game.LocalPlayer.Character.IsCollisionEnabled = true;
        Game.LocalPlayer.Character.CollisionIgnoredEntity = null;

        NativeFunction.Natives.SET_EVERYONE_IGNORE_PLAYER(Game.LocalPlayer, false);
        if (canEnterCar && VehicleTryingToEnter.Vehicle.Exists())
        {
            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", Game.LocalPlayer.Character, VehicleTryingToEnter.Vehicle, -1, SeatTryingToEnter, 1f, 9);
            GameFiber.Sleep(5000);
        }
        else
        {
            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        }

    }
    private void AttachPeds()
    {
        NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(Game.LocalPlayer.Character, Cop.Pedestrian, (int)PedBoneId.RightHand, Settings.SettingsManager.RespawnSettings.OffsetX, Settings.SettingsManager.RespawnSettings.OffsetY, 0f, 0f, 0f, 0f, true, true, false, false, 2, true);
        //NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(Game.LocalPlayer.Character, Cop.Pedestrian, (int)PedBoneId.RightHand, 0.2f, 0.4f, 0f, 0f, 0f, 0f, true, true, false, false, 2, true);
    }
    private void HandlePlayerDistance()
    {
        if (Cop.Pedestrian.Exists())
        {
            //AttachPeds();
            //Player.Character.Position = Cop.Pedestrian.GetOffsetPosition(AttachOffset);
            //Game.LocalPlayer.Character.Heading = Cop.Pedestrian.Heading;

            if (Cop.Pedestrian.IsWalking)
            {
                if (!isWalking)
                {
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "move_action@generic@core", "walk", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                    NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, PlayerCuffedDictionary, PlayerCuffedAnimation, 1.0f, -1.0f, -1, 1 | 16 | 32, 0, 0, 1, 0);
                    isWalking = true;
                }
            }
            else
            {
                if (isWalking)
                {
                    NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                    NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, PlayerCuffedDictionary, PlayerCuffedAnimation, 1.0f, -1.0f, -1, 1 | 16 | 32, 0, 0, 1, 0);
                    isWalking = false;
                }
            }
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
        EntryPoint.WriteToConsole("Retask Loop Ran");
        GetClosesetPoliceVehicle();
        GetInCarTask();
    }
    private void GetInCarTask()
    {
        if (Player.Character.Exists() && VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists() && Cop.Pedestrian.Exists())
        {
            //Player.Character.BlockPermanentEvents = true;
            //Player.Character.KeepTasks = true;


            Cop.Pedestrian.BlockPermanentEvents = true;
            Cop.Pedestrian.KeepTasks = true;

            VehicleTaskedToEnter = VehicleTryingToEnter.Vehicle;
            SeatTaskedToEnter = SeatTryingToEnter;
            Player.LastFriendlyVehicle = VehicleTryingToEnter.Vehicle;

            ////0xC0572928C0ABFDA _GET_ENTRY_POSITION_OF_DOOR
            //int doorTryingToEnter = SeatTryingToEnter == 1 ? 1 : SeatTryingToEnter == 2 ? 3 : 1;
            //Vector3 EntryPos = NativeFunction.Natives.xC0572928C0ABFDA<Vector3>(VehicleTryingToEnter.Vehicle, doorTryingToEnter);
            //NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Ped.Pedestrian, EntryPos.X, EntryPos.Y, EntryPos.Z, 1.0f, -1, 0f, 0.1f);
            NativeFunction.Natives.TASK_OPEN_VEHICLE_DOOR(Cop.Pedestrian, VehicleTaskedToEnter, -1, SeatTaskedToEnter, 1.0f);

            //unsafe
            //{
            //    int lol = 0;
            //    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            //    NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, VehicleTryingToEnter.Vehicle, -1, SeatTryingToEnter, 1f, 9);
            //    //NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(4000, 8000));
            //    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
            //    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            //    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
            //    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            //}
        }
        else if (Player.Character.Exists())
        {
            //Player.Character.BlockPermanentEvents = true;
            //Player.Character.KeepTasks = true;
            //unsafe
            //{
            //    int lol = 0;
            //    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            //    NativeFunction.CallByName<bool>("TASK_STAND_STILL", 0, -1);
            //    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
            //    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            //    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
            //    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            //}
        }
    }
    private void GetClosesetPoliceVehicle()
    {
        SeatAssigner.AssignPrisonerSeat();
        VehicleTryingToEnter = SeatAssigner.VehicleTryingToEnter;
        SeatTryingToEnter = SeatAssigner.SeatTryingToEnter;
        DoorTryingToEnter = SeatAssigner.GetDoorFromSeat(SeatTryingToEnter);
        //SeatTryingToEnterEntryPosition = SeatAssigner.GetEntryPosition(VehicleTryingToEnter, SeatTryingToEnter);
    }
}