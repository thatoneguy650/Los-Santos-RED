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

    private bool isCopInPosition;
    private bool isPlayerCuffed;
    private Vector3 CopTargetPosition;
    private float CopTargetHeading;
    private PedPlayerInteract PedPlayerInteract;

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

        if(Cop == null || !Cop.Pedestrian.Exists())
        {
            ReleaseCop();
            EndBooking();
            return;
        }
        IsActive = true;
        Player.IsBeingBooked = true;
        GameFiber.StartNew(delegate
        {
            try
            {
                SetupCop();
                SetupWorld();
                PedPlayerInteract = new PedPlayerInteract(Player, Cop, -0.9f);
                PedPlayerInteract.CanUseEitherSide = false;
                PedPlayerInteract.Start();
                if(CanContinueBooking)
                {
                    if (!PedPlayerInteract.IsInPosition)
                    {
                        PedPlayerInteract.SetPlayerInFront();
                    }
                    PlayCuffAnimation();
                    if(!isPlayerCuffed)
                    {
                        ReleaseCop();
                        EndBooking();
                        return;
                    }
                    Player.CuffManager.SetPlayerHandcuffed();
                    ReleaseCop();
                    BookingVehicleManager bookingVehicleManager = new BookingVehicleManager(Player, World, PoliceRespondable, Location, SeatAssignable, Settings, this, Cop);
                    bookingVehicleManager.Setup();
                    bookingVehicleManager.Start();
                    FinishBooking();
                }
                ReleaseCop();
                EndBooking();
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "Booking");
    }
   
    private void EndBooking()
    {
        Player.IsBeingBooked = false;
        IsActive = false;
        EntryPoint.WriteToConsole("BOOKING ENDED!");
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
            Cop.CurrentTask = null;
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
        if (CanContinueBooking)
        {
            Cop.WeaponInventory.ShouldAutoSetWeaponState = true;
            Cop.WeaponInventory.RemoveHeavyWeapon();
            Cop.WeaponInventory.UpdateLoadout(PoliceRespondable, World, false, Settings.SettingsManager.PoliceSettings.OverrideAccuracy);
            endLoop = false;
            while (Cop.Pedestrian.Exists() && !endLoop)
            {
                if (NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Cop.Pedestrian, CopApplyCuffsDictionary, CopApplyCuffsAnimation) >= 1.0f)
                {
                    endLoop = true;
                }
                GameFiber.Yield();
            }
            //GameFiber.Wait(250);
            if (CanContinueBooking)
            {
                //NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                Player.Character.KeepTasks = true;
                NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, PlayerCuffedDictionary, PlayerCuffedAnimation, 8.0f, -8.0f, -1, 1 | 8 | 16 | 32 | 8388608, 0, 0, 1, 0);
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
            IsActive = false;//temp here
        }
        else//for now
        {
            Player.IsBeingBooked = false;
            IsActive = false;//temp here
        }
    }
}