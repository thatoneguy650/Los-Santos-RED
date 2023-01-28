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
    private ISettingsProvideable Settings;
    private ISeatAssignable SeatAssignable;

    private Cop Cop;
    private string PlayerGetCuffedAnimation;
    private string CopApplyCuffsAnimation;
    private string PlayerGetCuffedDictionary;
    private string CopApplyCuffsDictionary;
    private string PlayerCuffedDictionary;
    private string PlayerCuffedAnimation;
    private Vector3 AttachOffset;
    private bool IsCancelled;


    //private SeatAssigner SeatAssigner;
    //private int SeatTryingToEnter;
    //private Vector3 SeatTryingToEnterEntryPosition;
    //private VehicleExt VehicleTryingToEnter;
    //private Vehicle VehicleTaskedToEnter;
    //private int SeatTaskedToEnter;
    private bool isCopInPosition;
    private bool isPlayerCuffed;
    private Vector3 CopTargetPosition;
    private float CopTargetHeading;
    //private bool hasEnteredVehicle;

    public bool CanContinueBooking => EntryPoint.ModController.IsRunning && (Player.IsBusted || Player.IsArrested) && !Player.IsIncapacitated && Player.IsAlive && Cop.Pedestrian.Exists() && !Cop.Pedestrian.IsDead && !Cop.IsInWrithe && !Cop.IsUnconscious;
    public bool IsActive { get; private set; }
    public BookingActivity(IRespawnable player, IEntityProvideable world, IPoliceRespondable policeRespondable, ILocationRespawnable location, ISeatAssignable seatAssignable, ISettingsProvideable settings)
    {
        Player = player;
        World = world;
        PoliceRespondable = policeRespondable;
        Location = location;
        Settings = settings;
        SeatAssignable = seatAssignable;

    }
    public void Setup()
    {
        PlayerGetCuffedAnimation = "crook_p2_back_left";
        CopApplyCuffsAnimation = "cop_p2_back_left";
        PlayerGetCuffedDictionary = "mp_arrest_paired";
        CopApplyCuffsDictionary = "mp_arrest_paired";


        PlayerCuffedDictionary = "mp_arresting";
        PlayerCuffedAnimation = "idle";

        AttachOffset = new Vector3(-0.31f, 0.12f, 0.04f);
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
        if (Cop != null && Cop.Pedestrian.Exists())
        {
            IsActive = true;
            Player.IsBeingBooked = true;
            GameFiber.StartNew(delegate
            {
                try
                {
                    SetupCop();
                    SetupWorld();
                    MoveCopBehindPlayer();
                    if (isCopInPosition)
                    {
                        PlayCuffAnimation();
                        if (isPlayerCuffed)
                        {
                            NativeFunction.Natives.SET_ENABLE_HANDCUFFS(Player.Character, true);

                            Player.SetNotBusted();

                            Player.SetWantedLevel(0, "Handcuffed", true);
                            Player.IsArrested = true;


                            try
                            {
                                BookingVehicleManager bookingVehicleManager = new BookingVehicleManager(Player, World, PoliceRespondable, Location, SeatAssignable, Settings, this, Cop);
                                bookingVehicleManager.Setup();
                                bookingVehicleManager.Start();
                            }
                            catch (Exception e)
                            {
                                EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Booking");
                            }
                            //TaskPlayerIntoVehicle();
                            FinishBooking();
                        }
                        else
                        {
                            ReleaseCop();
                            EndBooking();
                            EntryPoint.WriteToConsole("Booking Activity, Failure Cuffing Player");
                        }
                    }
                    else
                    {
                        ReleaseCop();
                        EndBooking();
                        EntryPoint.WriteToConsole("Booking Activity, Failure Moving Cop To Cuff Position");
                    }
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "Booking");
        }
        else
        {
            ReleaseCop();
            EndBooking();
            EntryPoint.WriteToConsole("Booking Activity, No Cop Found");
        }
    }
    private void EndBooking()
    {
        Player.IsBeingBooked = false;
        IsActive = false;
    }
    private void GetCop()
    {
        Cop = World.Pedestrians.PoliceList.Where(x => x.DistanceToPlayer <= 20f && x.HeightToPlayer <= 5f && !x.IsInVehicle && !x.IsUnconscious && !x.IsInWrithe && !x.IsDead && !x.Pedestrian.IsRagdoll).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
    }
    private void SetupCop()
    {
        if (Cop != null)
        {
            //Ped.CurrentTask = null;
            Cop.CanBeAmbientTasked = false;
            Cop.CanBeTasked = false;
        }
    }
    private void SetupWorld()
    {
        Game.TimeScale = 1.0f;
    }
    private void ReleaseCop()
    {
        if (Cop != null)
        {
            Cop.CanBeTasked = true;
            Cop.CanBeAmbientTasked = true;
        }
    }
    private void MoveCopBehindPlayer()
    {
        if(Cop.Pedestrian.Exists())
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
        NativeFunction.Natives.CLEAR_PED_TASKS(Cop.Pedestrian);
        NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Cop.Pedestrian, CopTargetPosition.X, CopTargetPosition.Y, CopTargetPosition.Z, 1.0f, -1, CopTargetHeading, 0.1f);
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


            if (Cop.Pedestrian.DistanceTo2D(CopTargetPosition) <= 0.05f && Math.Abs(Extensions.GetHeadingDifference(Cop.Pedestrian.Heading, CopTargetHeading)) <= 0.5f)
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
        CameraControl cameraControl = new CameraControl(Player);

        if (Settings.SettingsManager.RespawnSettings.UseCustomCameraWhenBooking)
        {

            cameraControl.Setup();
            cameraControl.HighlightEntity(Player.Character);
        }


        isPlayerCuffed = false;
        Cop.WeaponInventory.ShouldAutoSetWeaponState = false;
        Cop.WeaponInventory.SetUnarmed();
        NativeFunction.Natives.TASK_PLAY_ANIM(Cop.Pedestrian, CopApplyCuffsDictionary, CopApplyCuffsAnimation, 1.0f, -1.0f, -1, 2, 0, false, false, false);
        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, PlayerGetCuffedDictionary, PlayerGetCuffedAnimation, 1.0f, -1.0f, -1, 0, 0, false, false, false);
        bool endLoop = false;
        while(Cop.Pedestrian.Exists() && !endLoop)
        {
            if(NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Cop.Pedestrian, CopApplyCuffsDictionary, CopApplyCuffsAnimation) >= 0.5f)
            {
                endLoop = true;
            }
            GameFiber.Yield();
        }

        //GameFiber.Wait(950);
        //GameFiber.Wait(3000);
        if (CanContinueBooking)
        {
            Cop.WeaponInventory.ShouldAutoSetWeaponState = true;
            Cop.WeaponInventory.RemoveHeavyWeapon();
            Cop.WeaponInventory.UpdateLoadout(PoliceRespondable, false);

            endLoop = false;
            while (Cop.Pedestrian.Exists() && !endLoop)
            {
                if (NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Cop.Pedestrian, CopApplyCuffsDictionary, CopApplyCuffsAnimation) >= 1.0f)
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

        if (Settings.SettingsManager.RespawnSettings.UseCustomCameraWhenBooking)
        {
            cameraControl.ReturnToGameplayCam();
        }

    }
    private void FinishBooking()
    {
        if (Player.IsInVehicle)
        {
            Player.IsBeingBooked = false;
            //hasEnteredVehicle = true;
            IsActive = false;//temp here
        }
        else//for now
        {
            Player.IsBeingBooked = false;
            //hasEnteredVehicle = true;
            IsActive = false;//temp here
        }
    }
}