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
    private string PlayerAnimation;
    private string TargetAnimation;
    private string PlayerDictionary;
    private string TargetDictionary;
    private bool IsCancelled;


    private SeatAssigner SeatAssigner;
    private int SeatTryingToEnter;
    private VehicleExt VehicleTryingToEnter;
    private Vehicle VehicleTaskedToEnter;
    private int SeatTaskedToEnter;

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

    }
    public void Dispose()
    {

    }
    public void Start()
    {
        Cop arrestingCop = World.Pedestrians.PoliceList.Where(x => x.DistanceToPlayer <= 20f && x.HeightToPlayer <= 5f && !x.IsInVehicle && !x.IsUnconscious && !x.IsInWrithe && !x.IsDead && !x.Pedestrian.IsRagdoll).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
        if (arrestingCop != null && arrestingCop.Pedestrian.Exists())
        {

            GameFiber.StartNew(delegate
            {
                Ped = arrestingCop;

                Ped.CurrentTask = null;
                Ped.CanBeAmbientTasked = false;
                Ped.CanBeTasked = false;

                
                Game.TimeScale = 1.0f;

                MoveCopToPosition();

               // Attach();

                PlayPairedAnimation();

                Ped.CanBeTasked = true;
                Ped.CanBeAmbientTasked = true;

                GetClosesetPoliceVehicle();
                


                while(!Player.IsInVehicle && Player.IsBusted && Player.IsAlive)
                {
                    GetInCar(true);
                    GameFiber.Yield();
                }
                if(Player.IsInVehicle)
                {
                    Player.Reset(true, false, true, true, true, false, false, false, false, false, false, false, false, false);
                    //Game.DisplaySubtitle("Arrested");
                }


               // GameFiber.Sleep(15000);


                //Player.Respawning.SurrenderToPolice(Location);

            }, "Booking");

        }
        else
        {
            EntryPoint.WriteToConsole("Booking Activity, No Cop Found");
        }
    }
    private void MoveCopToPosition()
    {
        if(Ped.Pedestrian.Exists())
        {
            NativeFunction.Natives.CLEAR_PED_TASKS(Ped.Pedestrian);
            Vector3 BehindPlayerPosition = Player.Character.GetOffsetPositionFront(-0.9f);
            float desiredHeading = Player.Character.Heading;
            NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Ped.Pedestrian, BehindPlayerPosition.X, BehindPlayerPosition.Y, BehindPlayerPosition.Z, 1.0f, -1, desiredHeading, 0.1f);
            uint GameTimeStarted = Game.GameTime;
            while(Ped.Pedestrian.Exists())
            {
                if(Ped.Pedestrian.DistanceTo2D(BehindPlayerPosition) <= 0.05f && Math.Abs(Extensions.GetHeadingDifference(Ped.Pedestrian.Heading, desiredHeading)) <= 0.5f)
                {
                    break;
                }
                if(Ped.Pedestrian.IsDead || Ped.Pedestrian.DistanceTo2D(Player.Character) >= 30f)
                {
                    break;
                }
                if(Game.GameTime - GameTimeStarted>= 15000)//timeout for now
                {
                    break;
                }
                GameFiber.Yield();
            }
            GameFiber.Wait(500);
        }
    }
    private void Attach()
    {
        NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(Player.Character,Ped.Pedestrian,11816, -0.1f, 0.45f, 0.0f, 0.0f, 0.0f, 20.0f, false, false, false, false, 20, false);
    }
    private void PlayPairedAnimation()
    {
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



       // NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, "mp_arresting", "idle", 1.0f, -1.0f, -1, 1, 0, false, false, false);
       // Game.DisplaySubtitle("FINISHED");



        // GameFiber.Sleep(4000);



    }


    private void GetInCar(bool IsFirstRun)
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

            unsafe
            {
                int lol = 0;
                NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, VehicleTryingToEnter.Vehicle, -1, SeatTryingToEnter, 1f, 9);
                NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(4000, 8000));
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


}

