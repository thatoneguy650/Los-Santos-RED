using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SearchActivity
{
    private IRespawnable Player;
    private IEntityProvideable World;
    private IPoliceRespondable PoliceRespondable;
    private ISettingsProvideable Settings;
    private ISeatAssignable SeatAssignable;
    private ITimeReportable Time;
    private IModItems ModItems;
    private AnimationWatcher AnimationWatcher;
    private Cop Cop;
    private VehicleExt CarToSearch;
    private string PlayerGetSearchedAnimation;
    private string CopDoSearchAnimation;
    private string PlayerGetSearchDictionary;
    private string CopDoSearchDictionary;
    private string PlayerCuffedDictionary;
    private string PlayerCuffedAnimation;
    private bool IsCancelled;
    private bool isCopInPosition;
    private bool isPlayerCuffed;
    private Vector3 CopTargetPosition;
    private float CopTargetHeading;
    private bool AnnouncedIllegalWeapons;
    private bool AnnouncedIllegalDrugs;
    private PlayerPoliceSearch PlayerPoliceSearch;
    private CameraControl CameraControl;
    private string animDict;
    private string anim;
    private float AnimationToggleTime;

    public bool CanContinueSearch => EntryPoint.ModController.IsRunning && (Player.IsBusted || Player.IsArrested) && !Player.IsIncapacitated && Player.IsAlive && Cop.Pedestrian.Exists() && !Cop.Pedestrian.IsDead && !Cop.IsInWrithe && !Cop.IsUnconscious;
    public bool IsActive { get; private set; }
    public bool FoundIllegalItems => PlayerPoliceSearch.FoundIllegalItems ||PlayerPoliceSearch.FoundVehicleIllegalItems;
    public bool CompletedSearch { get; private set; } = false;
    public bool HasVehicle => CarToSearch != null && CarToSearch.Vehicle.Exists();
    public SearchActivity(IRespawnable player, IEntityProvideable world, IPoliceRespondable policeRespondable, ISeatAssignable seatAssignable, ISettingsProvideable settings, ITimeReportable time, IModItems modItems, VehicleExt carToSearch)
    {
        Player = player;
        World = world;
        PoliceRespondable = policeRespondable;
        Settings = settings;
        SeatAssignable = seatAssignable;
        Time = time;
        ModItems = modItems;
        CarToSearch = carToSearch;
        PlayerPoliceSearch = new PlayerPoliceSearch(Player,Time,ModItems, CarToSearch);
    }
    public void Setup()
    {
        PlayerGetSearchDictionary = "ped";
        PlayerGetSearchedAnimation = "handsup_enter";


        CopDoSearchDictionary = "oddjobs@assassinate@construction@";
        CopDoSearchAnimation = "cs_getinlift";
        
        PlayerCuffedDictionary = "ped"; //"mp_arresting";
        PlayerCuffedAnimation = "handsup_enter"; //"idle";

        AnimationWatcher = new AnimationWatcher();
        AnimationDictionary.RequestAnimationDictionay(PlayerGetSearchDictionary);
        AnimationDictionary.RequestAnimationDictionay(CopDoSearchDictionary);
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
            GameFiber.StartNew(delegate
            {
                try
                {
                    SetupCop();
                    SetupWorld();
                    SetupPlayer();
                    MoveCopBehindPlayer();
                    if (isCopInPosition)
                    {
                        PlayPlayerSearchAnimation();
                        if (HasVehicle && Settings.SettingsManager.RespawnSettings.IncludeCarInSearch)
                        {

                            MoveCopToCar();
                            if (isCopInPosition)
                            {
                                PlayCarSearchAnimation();
                            }
                        }
                    }
                    ReleaseCop();
                    EndSearch();
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
            EndSearch();
            //EntryPoint.WriteToConsoleTestLong("Booking Activity, No Cop Found");
        }
    }
    private void ToggleTrunkLoop(bool setOpen)
    {
        if (!Cop.Pedestrian.Exists() || CarToSearch == null || !CarToSearch.Vehicle.Exists())
        {
            return;
        }

        bool isOpen = CarToSearch != null && CarToSearch.Vehicle.Exists() && CarToSearch.Vehicle.Doors[5].IsOpen;
        animDict = "anim_heist@hs4f@ig14_open_car_trunk@male@";
        anim = setOpen ? "open_trunk_rushed" : "close_trunk";
        AnimationToggleTime = setOpen ? Settings.SettingsManager.DoorToggleSettings.OpenHoodAnimationTime : Settings.SettingsManager.DoorToggleSettings.CloseHoodAnimationTime;
        if ((setOpen && isOpen) || (!setOpen && !isOpen))
        {
            EntryPoint.WriteToConsole($"ToggleTrunkLoop setOpen{setOpen} isOpen{isOpen}");
            return;
        }
        AnimationDictionary.RequestAnimationDictionay(animDict);
        AnimationWatcher aw = new AnimationWatcher();
        uint GameTimeStartedAnimation = Game.GameTime;
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Cop.Pedestrian, animDict, anim, 2.0f, -2.0f, -1, (int)(AnimationFlags.StayInEndFrame), 0, false, false, false);
        bool toggledDoor = false;
        EntryPoint.WriteToConsole($"DOOR TOGGLE: STARTED ANIM");
        while (CanContinueSearch)
        {
            float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Cop.Pedestrian, animDict, anim);
            if (AnimationTime >= 1.0f)
            {
                EntryPoint.WriteToConsole($"DOOR TOGGLE: FINISHED ANIM ANIM TIME TIME OUT");
                break;
            }
            if (!aw.IsAnimationRunning(AnimationTime))
            {
                EntryPoint.WriteToConsole($"DOOR TOGGLE: FINISHED ANIM ANIM NOT RUNNING");
                break;
            }
            if (!toggledDoor && AnimationTime >= AnimationToggleTime)
            {
                DoorMovement();
                toggledDoor = true;
            }
//#if DEBUG
//            Game.DisplaySubtitle($"AnimationTime: {AnimationTime}");
//#endif
            GameFiber.Yield();
        }
    }


    private void DoorMovement()
    {
        if (!Cop.Pedestrian.Exists() || CarToSearch == null || !CarToSearch.Vehicle.Exists())
        {
            return;
        }
        EntryPoint.WriteToConsole($"DoorMovement");
        if (CarToSearch.Vehicle.Doors[5].IsOpen)
        {
            CarToSearch.Vehicle.Doors[5].Close(false);
        }
        else
        {
            CarToSearch.Vehicle.Doors[5].Open(false, false);
            GameFiber.Wait(750);
            if (CarToSearch.Vehicle.Exists())
            {
                CarToSearch.Vehicle.Doors[5].Open(true, false);
            }
        }
    }

    private void PlayCarSearchAnimation()
    {
        if (!Cop.Pedestrian.Exists() || CarToSearch == null || !CarToSearch.Vehicle.Exists())
        {
            return;
        }

        if (Settings.SettingsManager.RespawnSettings.UseCustomCameraWhenBooking)
        {
            CameraControl.Setup();
            CameraControl.HighlightEntity(CarToSearch.Vehicle, 3f,2f,1f);
        }


        ToggleTrunkLoop(true);
        //cameraControl = new CameraControl(Player);




        Cop.WeaponInventory.ShouldAutoSetWeaponState = false;
        Cop.WeaponInventory.SetUnarmed();




        NativeFunction.Natives.TASK_PLAY_ANIM(Cop.Pedestrian, CopDoSearchDictionary, CopDoSearchAnimation, 1.0f, -1.0f, -1, 2, 0, false, false, false);
        Player.Surrendering.SetArrestedAnimation(true);
        bool endLoop = false;
        AnnouncedIllegalWeapons = false;
        AnnouncedIllegalDrugs = false;
        while (Cop.Pedestrian.Exists() && !endLoop)
        {

            float animTime = NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Cop.Pedestrian, CopDoSearchDictionary, CopDoSearchAnimation);
            if (animTime >= 0.4f)
            {
                if (!PlayerPoliceSearch.DidVehicleWeaponSearch)
                {
                    //EntryPoint.WriteToConsoleTestLong("Cop Search Do Weapons Search");
                    PlayerPoliceSearch.DoVehicleWeaponSearch();
                }
                if (PlayerPoliceSearch.FoundVehicleIllegalWeapons && !AnnouncedIllegalWeapons)
                {
                    //EntryPoint.WriteToConsoleTestLong("Cop Search Announce Found Weapons");
                    CopAnnounceFoundWeapon();
                }
            }
            if (animTime >= 0.7f)
            {
                if (!PlayerPoliceSearch.DidVehicleItemsSearch)
                {
                    //EntryPoint.WriteToConsoleTestLong("Cop Search Do Items Search");
                    PlayerPoliceSearch.DoVehicleItemSearch();
                }
                if (PlayerPoliceSearch.FoundVehicleIllegalDrugs && !AnnouncedIllegalDrugs)
                {
                    //EntryPoint.WriteToConsoleTestLong("Cop Search Announce Found Drugs");
                    CopAnnounceFoundDrugs();
                }
            }
            if (animTime >= 1.0f)
            {
                CompletedSearch = true;
                if (!FoundIllegalItems)
                {
                    //EntryPoint.WriteToConsoleTestLong("Cop Search Announce Found Nothing");
                    CopAnnounceFoundNothing();
                }
                endLoop = true;
            }
            bool isAnimRunning = AnimationWatcher.IsAnimationRunning(animTime);
            if (!isAnimRunning)
            {
                //EntryPoint.WriteToConsoleTestLong("Cop Animation on Search Not Running");
                endLoop = true;
            }

            GameFiber.Yield();
        }
        //EntryPoint.WriteToConsoleTestLong("Cop Search ANIM LOOP ENDED");
        if (CanContinueSearch)
        {
            Cop.WeaponInventory.ShouldAutoSetWeaponState = true;
            Cop.WeaponInventory.RemoveHeavyWeapon();
            Cop.WeaponInventory.UpdateLoadout(PoliceRespondable, false, Settings.SettingsManager.PoliceSettings.OverrideAccuracy);
            endLoop = false;
            while (Cop.Pedestrian.Exists() && !endLoop)
            {
                if (NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Cop.Pedestrian, CopDoSearchDictionary, CopDoSearchAnimation) >= 1.0f)
                {
                    endLoop = true;
                }
                GameFiber.Yield();
            }
        }

        if(CanContinueSearch)
        {
            ToggleTrunkLoop(false);
        }

        if (Settings.SettingsManager.RespawnSettings.UseCustomCameraWhenBooking)
        {
            CameraControl.ReturnToGameplayCam();
        }
    }

    private void MoveCopToCar()
    {
        if (!Cop.Pedestrian.Exists() || CarToSearch == null || !CarToSearch.Vehicle.Exists())
        {
            return;
        }
        
        GetCopDesiredCarPosition();
        TaskCopToDesiredPosition();
        CopMoveLoop(true);
    }
    private void GetCopDesiredCarPosition()
    {
        if (CarToSearch == null || !CarToSearch.Vehicle.Exists())
        {
            return;
        }
        VehicleDoorSeatData vdsd = new VehicleDoorSeatData("unknow", "unknow", 5, -1);
        CopTargetPosition = vdsd.GetDoorOffset(CarToSearch.Vehicle, Settings);
        CopTargetHeading = vdsd.GetDoorHeading(CarToSearch.Vehicle, Settings);
    }
    private void EndSearch()
    {
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
            Cop.CanBeAmbientTasked = false;
            Cop.CanBeTasked = false;
        }
    }
    private void SetupWorld()
    {
        Game.TimeScale = 1.0f;
    }
    private void SetupPlayer()
    {
        if (Player.Character.IsInAnyVehicle(false))
        {
            Vehicle oldVehicle = Player.Character.CurrentVehicle;
            if (Player.Character.Exists() && oldVehicle.Exists())
            {
                NativeFunction.Natives.TASK_LEAVE_VEHICLE(Player.Character, oldVehicle, (int)eEnter_Exit_Vehicle_Flags.ECF_DONT_CLOSE_DOOR);
                while (Player.Character.IsInAnyVehicle(false) && CanContinueSearch)
                {
                    GameFiber.Yield();
                }
                Player.WeaponEquipment.SetUnarmed();
            }
        }
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
        if (Cop.Pedestrian.Exists())
        {
            GetCopDesiredPosition();
            TaskCopToDesiredPosition();
            CopMoveLoop(false);
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
        NativeFunction.Natives.TASK_FOLLOW_NAV_MESH_TO_COORD(Cop.Pedestrian, CopTargetPosition.X, CopTargetPosition.Y, CopTargetPosition.Z, 1.0f, -1, 0.1f, 0, CopTargetHeading);
    }
    private void CopMoveLoop(bool isVehicle)
    {
        isCopInPosition = false;
        while (CanContinueSearch)
        {
            if (!isVehicle && CopTargetPosition.DistanceTo2D(Player.Character.GetOffsetPositionFront(-0.9f)) >= 0.1f)
            {
                if (isVehicle)
                {
                    GetCopDesiredCarPosition();
                    TaskCopToDesiredPosition();
                }
                else
                {
                    GetCopDesiredPosition();
                    TaskCopToDesiredPosition();
                }
            }
            float distanceToPos = Cop.Pedestrian.DistanceTo2D(CopTargetPosition);
            float headingDiff = Math.Abs(Extensions.GetHeadingDifference(Cop.Pedestrian.Heading, CopTargetHeading));
            if (distanceToPos <= 0.3f && headingDiff <= 0.5f)
            {
                isCopInPosition = true;
                break;
            }
//#if DEBUG
//            Game.DisplaySubtitle($"distanceToPos:{distanceToPos} headingDiff{headingDiff} isCopInPosition{isCopInPosition} {isVehicle}");
//            Rage.Debug.DrawArrowDebug(CopTargetPosition + new Vector3(0f, 0f, 2f), Vector3.Zero, Rotator.Zero, 1f, System.Drawing.Color.White);
//#endif
            GameFiber.Yield();
        }
        if (isCopInPosition)
        {
            GameFiber.Wait(500);
        }
    }
    private void PlayPlayerSearchAnimation()
    {
        CameraControl = new CameraControl(Player);
        if (Settings.SettingsManager.RespawnSettings.UseCustomCameraWhenBooking)
        {
            CameraControl.Setup();
            CameraControl.HighlightEntity(Player.Character);
        }
        Cop.WeaponInventory.ShouldAutoSetWeaponState = false;
        Cop.WeaponInventory.SetUnarmed();
        NativeFunction.Natives.TASK_PLAY_ANIM(Cop.Pedestrian, CopDoSearchDictionary, CopDoSearchAnimation, 1.0f, -1.0f, -1, 2, 0, false, false, false);
        Player.Surrendering.SetArrestedAnimation(true);
        bool endLoop = false;
        while (Cop.Pedestrian.Exists() && !endLoop)
        {
           
            float animTime = NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Cop.Pedestrian, CopDoSearchDictionary, CopDoSearchAnimation);
            if (animTime >= 0.4f)
            {
                if (!PlayerPoliceSearch.DidWeaponSearch)
                {
                    //EntryPoint.WriteToConsoleTestLong("Cop Search Do Weapons Search");
                    PlayerPoliceSearch.DoWeaponSearch();
                }
                if(PlayerPoliceSearch.FoundIllegalWeapons && !AnnouncedIllegalWeapons)
                {
                    //EntryPoint.WriteToConsoleTestLong("Cop Search Announce Found Weapons");
                    CopAnnounceFoundWeapon();
                }
            }
            if (animTime >= 0.7f)
            {
                if (!PlayerPoliceSearch.DidItemsSearch)
                {
                    //EntryPoint.WriteToConsoleTestLong("Cop Search Do Items Search");
                    PlayerPoliceSearch.DoItemSearch();
                }
                if (PlayerPoliceSearch.FoundIllegalDrugs && !AnnouncedIllegalDrugs)
                {
                    //EntryPoint.WriteToConsoleTestLong("Cop Search Announce Found Drugs");
                    CopAnnounceFoundDrugs();
                }
            }
            if(animTime >= 1.0f)
            {
                CompletedSearch = true;
                if (!FoundIllegalItems && !HasVehicle)
                {
                    //EntryPoint.WriteToConsoleTestLong("Cop Search Announce Found Nothing");
                    CopAnnounceFoundNothing();
                }
                endLoop = true;
            }
            bool isAnimRunning = AnimationWatcher.IsAnimationRunning(animTime);
            if (!isAnimRunning)
            {
                //EntryPoint.WriteToConsoleTestLong("Cop Animation on Search Not Running");
                endLoop = true;
            }

            GameFiber.Yield();
        }
        //EntryPoint.WriteToConsoleTestLong("Cop Search ANIM LOOP ENDED");
        if (CanContinueSearch)
        {
            Cop.WeaponInventory.ShouldAutoSetWeaponState = true;
            Cop.WeaponInventory.RemoveHeavyWeapon();
            Cop.WeaponInventory.UpdateLoadout(PoliceRespondable, false, Settings.SettingsManager.PoliceSettings.OverrideAccuracy);
            endLoop = false;
            while (Cop.Pedestrian.Exists() && !endLoop)
            {
                if (NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Cop.Pedestrian, CopDoSearchDictionary, CopDoSearchAnimation) >= 1.0f)
                {
                    endLoop = true;
                }
                GameFiber.Yield();
            }
        }
        if (!HasVehicle && Settings.SettingsManager.RespawnSettings.UseCustomCameraWhenBooking)
        {
            CameraControl.ReturnToGameplayCam();
        }
    }
    private void CopAnnounceFoundWeapon()
    {
        AnnouncedIllegalWeapons = true;
        List<string> foundWeaponResponse = new List<string>()
                {
                    $"Got enough weapons on you?",
                    $"Seems you are starting a little weapon collection.",
                    $"Might need to add this to my drop gun collection.",
                };

        Game.DisplayHelp("Illegal Weapons Found");
        Game.DisplaySubtitle("~g~Cop: ~s~" + foundWeaponResponse.PickRandom());
        //GameFiber.Sleep(4000);
    }
    private void CopAnnounceFoundDrugs()
    {
        AnnouncedIllegalDrugs = true;
        List<string> foundItemResponse = new List<string>()
                {
                    $"I don't think these are legal.",
                    $"Seems you've got some illegal items here.",
                    $"Guess you didn't want us to find that.",
                };
        Game.DisplayHelp("Illegal Items Found");
        Game.DisplaySubtitle("~g~Cop: ~s~" + foundItemResponse.PickRandom());
    }
    private void CopAnnounceFoundNothing()
    {
        List<string> foundNothingResponse = new List<string>()
                {
                    $"I guess you are clean. Don't hang around.",
                    $"You're clean. Get lost.",
                    $"Nothing? Really? Beat your feet.",
                    $"Keep your nose clean. Get outta here.",
                };
        Game.DisplaySubtitle("~g~Cop: ~s~" + foundNothingResponse.PickRandom());
    }
}