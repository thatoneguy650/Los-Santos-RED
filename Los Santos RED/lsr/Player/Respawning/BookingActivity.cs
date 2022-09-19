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


public class BookingActivity
{
    private IRespawnable Player;
    private IEntityProvideable World;
    private IPoliceRespondable PoliceRespondable;
    private ILocationRespawnable Location;

    private Cop Ped;
    private string PlayerGetCuffedAnimation;
    private string CopApplyCuffsAnimation;
    private string PlayerGetCuffedDictionary;
    private string CopApplyCuffsDictionary;
    private string PlayerCuffedDictionary;
    private string PlayerCuffedAnimation;
    private bool IsCancelled;


    private SeatAssigner SeatAssigner;
    private int SeatTryingToEnter;
    private VehicleExt VehicleTryingToEnter;
    private Vehicle VehicleTaskedToEnter;
    private int SeatTaskedToEnter;
    private bool isCopInPosition;
    private bool isPlayerCuffed;
    private Vector3 CopTargetPosition;
    private float CopTargetHeading;
    private bool hasEnteredVehicle;

    private bool CanContinueBooking => EntryPoint.ModController.IsRunning && Player.IsBusted && !Player.IsIncapacitated && Player.IsAlive && Ped.Pedestrian.Exists() && !Ped.Pedestrian.IsDead && !Ped.IsInWrithe && !Ped.IsUnconscious;
    public BookingActivity(IRespawnable player, IEntityProvideable world, IPoliceRespondable policeRespondable, ILocationRespawnable location, ISeatAssignable seatAssignable)
    {
        Player = player;
        World = world;
        PoliceRespondable = policeRespondable;
        Location = location;
        SeatAssigner = new SeatAssigner(seatAssignable, World, World.Vehicles.PoliceVehicleList);
    }
    public void Setup()
    {
        PlayerGetCuffedAnimation = "crook_p2_back_left";
        CopApplyCuffsAnimation = "cop_p2_back_left";
        PlayerGetCuffedDictionary = "mp_arrest_paired";
        CopApplyCuffsDictionary = "mp_arrest_paired";


        PlayerCuffedDictionary = "mp_arresting";
        PlayerCuffedAnimation = "idle";


        AnimationDictionary.RequestAnimationDictionay(PlayerGetCuffedDictionary);
        AnimationDictionary.RequestAnimationDictionay(CopApplyCuffsDictionary);
        AnimationDictionary.RequestAnimationDictionay(PlayerCuffedDictionary);
    }
    public void Dispose()
    {
        ReleaseCop();
    }
    public void Start()
    {
        GetCop();
        if (Ped != null && Ped.Pedestrian.Exists())
        {
            Player.IsBeingBooked = true;
            GameFiber.StartNew(delegate
            {
                SetupCop();
                SetupWorld();
                MoveCopBehindPlayer();
                if (isCopInPosition)
                {
                    PlayCuffAnimation();
                    ReleaseCop();
                    if (isPlayerCuffed)
                    {
                        TaskPlayerIntoVehicle();
                        FinishBooking();
                    }
                    else
                    {
                        Player.IsBeingBooked = false;
                        Player.IsArrested = false;
                        EntryPoint.WriteToConsole("Booking Activity, Failure Cuffing Player");
                    }
                }
                else
                {
                    ReleaseCop();
                    Player.IsBeingBooked = false;
                    Player.IsArrested = false;
                    EntryPoint.WriteToConsole("Booking Activity, Failure Moving Cop To Cuff Position");
                }
            }, "Booking");
        }
        else
        {
            Player.IsBeingBooked = false;
            Player.IsArrested = false;
            EntryPoint.WriteToConsole("Booking Activity, No Cop Found");
        }
    }
    private void GetCop()
    {
        Ped = World.Pedestrians.PoliceList.Where(x => x.DistanceToPlayer <= 20f && x.HeightToPlayer <= 5f && !x.IsInVehicle && !x.IsUnconscious && !x.IsInWrithe && !x.IsDead && !x.Pedestrian.IsRagdoll).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
    }
    private void SetupCop()
    {
        if (Ped != null)
        {
            Ped.CurrentTask = null;
            Ped.CanBeAmbientTasked = false;
            Ped.CanBeTasked = false;
        }
    }
    private void SetupWorld()
    {
        Game.TimeScale = 1.0f;
    }
    private void ReleaseCop()
    {
        if (Ped != null)
        {
            Ped.CanBeTasked = true;
            Ped.CanBeAmbientTasked = true;
        }
    }
    private void MoveCopBehindPlayer()
    {
        if(Ped.Pedestrian.Exists())
        {
            GetCopDesiredPosition();
            TaskCopToDesiredPosition();
            CopMoveLoop();
        }
    }
    private void GetCopDesiredPosition()
    {
        CopTargetPosition = Player.Character.GetOffsetPositionFront(-0.9f);
        CopTargetHeading = Player.Character.Heading;
    }
    private void TaskCopToDesiredPosition()
    {
        NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
        NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Ped.Pedestrian, CopTargetPosition.X, CopTargetPosition.Y, CopTargetPosition.Z, 1.0f, -1, CopTargetHeading, 0.1f);
    }
    private void CopMoveLoop()
    {
        isCopInPosition = false;
        while (CanContinueBooking)
        {
            if(CopTargetPosition.DistanceTo2D(Player.Character.GetOffsetPositionFront(-0.9f)) >= 0.1f)
            {
                GetCopDesiredPosition();
                TaskCopToDesiredPosition();
            }


            if (Ped.Pedestrian.DistanceTo2D(CopTargetPosition) <= 0.05f && Math.Abs(Extensions.GetHeadingDifference(Ped.Pedestrian.Heading, CopTargetHeading)) <= 0.5f)
            {
                isCopInPosition = true;
                break;
            }
            GameFiber.Yield();
        }
        if (isCopInPosition)
        {
            GameFiber.Wait(500);
        }
    }
    private void PlayCuffAnimation()
    {
        isPlayerCuffed = false;
        Ped.WeaponInventory.ShouldAutoSetWeaponState = false;
        Ped.WeaponInventory.SetUnarmed();
        NativeFunction.Natives.TASK_PLAY_ANIM(Ped.Pedestrian, CopApplyCuffsDictionary, CopApplyCuffsAnimation, 1.0f, -1.0f, -1, 2, 0, false, false, false);
        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, PlayerGetCuffedDictionary, PlayerGetCuffedAnimation, 1.0f, -1.0f, -1, 0, 0, false, false, false);
        bool endLoop = false;
        while(Ped.Pedestrian.Exists() && !endLoop)
        {
            if(NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Ped.Pedestrian, CopApplyCuffsDictionary, CopApplyCuffsAnimation) >= 0.5f)
            {
                endLoop = true;
            }
            GameFiber.Yield();
        }

        //GameFiber.Wait(950);
        //GameFiber.Wait(3000);
        if (CanContinueBooking)
        {
            Ped.WeaponInventory.ShouldAutoSetWeaponState = true;
            Ped.WeaponInventory.RemoveHeavyWeapon();
            Ped.WeaponInventory.UpdateLoadout(PoliceRespondable);

            endLoop = false;
            while (Ped.Pedestrian.Exists() && !endLoop)
            {
                if (NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Ped.Pedestrian, CopApplyCuffsDictionary, CopApplyCuffsAnimation) >= 1.0f)
                {
                    endLoop = true;
                }
                GameFiber.Yield();
            }


            //GameFiber.Wait(1000);
            if (CanContinueBooking)
            {
                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                Player.Character.KeepTasks = true;
                NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, PlayerCuffedDictionary, PlayerCuffedAnimation, 1.0f, -1.0f, -1, 49, 0, 0, 1, 0);
                isPlayerCuffed = true;
            }
        }
    }
    private void TaskPlayerIntoVehicle()
    {
        GetClosesetPoliceVehicle();
        while (!Player.IsInVehicle && CanContinueBooking)
        {
            PlayerEnterCarLoop();
            GameFiber.Yield();
        }
    }
    private void FinishBooking()
    {
        if (Player.IsInVehicle)
        {
            hasEnteredVehicle = true;
            Player.IsArrested = true;
        }


        //Player.ButtonPrompts.AddPrompt("HotelStay", "Cancel Stay", "CancelHotelStay", Settings.SettingsManager.KeySettings.InteractCancel, 99);
        //while (Player.IsArrested)
        //{
        //    if (Player.ButtonPrompts.IsPressed("SkipRide"))
        //    {

        //        break;
        //    }
        //    GameFiber.Yield();
        //}
        //Player.ButtonPrompts.RemovePrompts("HotelStay");



        if (Player.IsInVehicle)
        {
            Player.Reset(true, false, true, true, true, false, false, false, false, false, false, false, false, false, false);

            Game.LocalPlayer.HasControl = false;
            GameFiber.Sleep(20000);
            Player.Respawning.SurrenderToPolice(Location);

        }
    }
    private bool IsPlayingAnimation(Ped ped, string dictionary, string animation)
    {
        return NativeFunction.Natives.IS_ENTITY_PLAYING_ANIM<bool>(ped, dictionary, animation, 3) || NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(ped, dictionary, animation) > 0f;
    }
    private void PlayerEnterCarLoop()
    {
        if (Player.Character.Exists())
        {
            if (VehicleTaskedToEnter == null || !VehicleTaskedToEnter.Exists())
            {
                GetClosesetPoliceVehicle();
                EntryPoint.WriteToConsole($"GetArrested {Player.Character.Handle}: Get in Car, Got New Car, was Blank", 3);
                GetInCarTask();
            }
            else if (VehicleTryingToEnter != null && VehicleTaskedToEnter.Exists() && !VehicleTaskedToEnter.IsSeatFree(SeatTaskedToEnter) && VehicleTaskedToEnter.GetPedOnSeat(SeatTaskedToEnter).Exists() && VehicleTaskedToEnter.GetPedOnSeat(SeatTaskedToEnter).Handle != Player.Character.Handle)// && (VehicleTryingToEnter.Vehicle.Handle != VehicleTaskedToEnter.Handle || SeatTaskedToEnter != SeatTryingToEnter) && Ped.Pedestrian.Exists() && !Ped.Pedestrian.IsInAnyVehicle(true))
            {
                GetClosesetPoliceVehicle();
                EntryPoint.WriteToConsole($"GetArrested {Player.Character.Handle}: Get in Car Got New Car, was occupied?", 3);
                GetInCarTask();
            }
            else if (VehicleTryingToEnter != null && VehicleTaskedToEnter.Exists() && VehicleTaskedToEnter.Speed > 1.0f)// && (VehicleTryingToEnter.Vehicle.Handle != VehicleTaskedToEnter.Handle || SeatTaskedToEnter != SeatTryingToEnter) && Ped.Pedestrian.Exists() && !Ped.Pedestrian.IsInAnyVehicle(true))
            {
                GetClosesetPoliceVehicle();
                EntryPoint.WriteToConsole($"GetArrested {Player.Character.Handle}: Get in Car Got New Car, was driving away?", 3);
                GetInCarTask();
            }
            else if (Player.Character.Tasks.CurrentTaskStatus == Rage.TaskStatus.None || Player.Character.Tasks.CurrentTaskStatus == Rage.TaskStatus.NoTask)//might be a error?
            {
                Player.Character.BlockPermanentEvents = true;
                Player.Character.KeepTasks = true;
            }
            if (Player.Character.IsGettingIntoVehicle)
            {
                NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            }
        }
    }
    private void GetInCarTask()
    {
        if (Player.Character.Exists() && VehicleTryingToEnter != null && VehicleTryingToEnter.Vehicle.Exists())
        {
            Player.Character.BlockPermanentEvents = true;
            Player.Character.KeepTasks = true;
            VehicleTaskedToEnter = VehicleTryingToEnter.Vehicle;
            SeatTaskedToEnter = SeatTryingToEnter;

            Player.LastFriendlyVehicle = VehicleTryingToEnter.Vehicle;


            ////0xC0572928C0ABFDA _GET_ENTRY_POSITION_OF_DOOR

            //int doorTryingToEnter = SeatTryingToEnter == 1 ? 1 : SeatTryingToEnter == 2 ? 3 : 1;
            //Vector3 EntryPos = NativeFunction.Natives.xC0572928C0ABFDA<Vector3>(VehicleTryingToEnter.Vehicle, doorTryingToEnter);


            //NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Ped.Pedestrian, EntryPos.X, EntryPos.Y, EntryPos.Z, 1.0f, -1, 0f, 0.1f);


            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, VehicleTryingToEnter.Vehicle, -1, SeatTryingToEnter, 1f, 9);
                //NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(4000, 8000));
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
        else if (Player.Character.Exists())
        {
            Player.Character.BlockPermanentEvents = true;
            Player.Character.KeepTasks = true;
            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_STAND_STILL", 0, -1);
                NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
                NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
            }
        }
    }
    private void GetClosesetPoliceVehicle()
    {
        SeatAssigner.AssignPrisonerSeat();
        VehicleTryingToEnter = SeatAssigner.VehicleTryingToEnter;
        SeatTryingToEnter = SeatAssigner.SeatTryingToEnter;
    }
    private void Attach()
    {
        NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(Player.Character, Ped.Pedestrian, 11816, -0.1f, 0.45f, 0.0f, 0.0f, 0.0f, 20.0f, false, false, false, false, 20, false);
    }
}

/*
 *     private void PlayCuffAnimation()
    {

        isPlayerCuffed = false;

        PlayerAnimation = "crook_p2_back_left"; 
        TargetAnimation = "cop_p2_back_left";
        PlayerDictionary = "mp_arrest_paired";
        TargetDictionary = "mp_arrest_paired";
        AnimationDictionary.RequestAnimationDictionay(PlayerDictionary);
        AnimationDictionary.RequestAnimationDictionay(TargetDictionary);


        Ped.WeaponInventory.ShouldAutoSetWeaponState = false;
        Ped.WeaponInventory.SetUnarmed();



        //NativeFunction.Natives.TASK_PLAY_ANIM(Ped.Pedestrian, TargetDictionary, TargetAnimation, 8.0f, -8.0f, 5500, 33, 0, false, false, false);
        //NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, PlayerDictionary, PlayerAnimation, 8.0f, -8.0f, 5500, 33, 0, false, false, false);

        NativeFunction.Natives.TASK_PLAY_ANIM(Ped.Pedestrian, TargetDictionary, TargetAnimation, 4.0f, -4.0f, -1, 2, 0, false, false, false);
        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, PlayerDictionary, PlayerAnimation, 4.0f, -4.0f, -1, 0, 0, false, false, false);


        GameFiber.Wait(950);
        //Player.Character.Detach();



        //GameFiber.Sleep(4000);


        GameFiber.Wait(3000);

        //Ped.CanBeAmbientTasked = true;
        //Ped.CanBeTasked = true;



        //Ped.WeaponInventory.SetLessLethal();


        //Ped.WeaponInventory.Reset();
        //Ped.WeaponInventory.UpdateLoadout(PoliceRespondable);
        //Game.DisplaySubtitle("SET LOADOUT");

        Ped.WeaponInventory.ShouldAutoSetWeaponState = true;

        Ped.WeaponInventory.RemoveHeavyWeapon();


        Ped.WeaponInventory.UpdateLoadout(PoliceRespondable);



        //Ped.WeaponInventory.Reset();
        //Ped.WeaponInventory.SetLessLethal();

        //Ped.WeaponInventory.UpdateLoadout(PoliceRespondable);


        //NativeFunction.Natives.SET_CURRENT_PED_WEAPON(Ped.Pedestrian, (uint)Game.GetHashKey("weapon_stungun"), true);


        GameFiber.Wait(1000);

        AnimationDictionary.RequestAnimationDictionay("mp_arresting");




        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        Player.Character.KeepTasks = true;

        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "mp_arresting", "idle", 1.0f, -1.0f, -1, 49, 0, 0, 1, 0);



        isPlayerCuffed = true;




       // NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, "mp_arresting", "idle", 1.0f, -1.0f, -1, 1, 0, false, false, false);
       // Game.DisplaySubtitle("FINISHED");



        // GameFiber.Sleep(4000);



    }*/