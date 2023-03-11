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

    public bool CanContinueSearch => EntryPoint.ModController.IsRunning && (Player.IsBusted || Player.IsArrested) && !Player.IsIncapacitated && Player.IsAlive && Cop.Pedestrian.Exists() && !Cop.Pedestrian.IsDead && !Cop.IsInWrithe && !Cop.IsUnconscious;
    public bool IsActive { get; private set; }

    private bool FoundIllegalDrugs;
    private bool FoundIllegalWeapons;
    private bool DidItemsSearch;
    private bool DidWeaponSearch;

    public bool FoundIllegalItems { get; private set; }
    public bool CompletedSearch { get; private set; } = false;

    public SearchActivity(IRespawnable player, IEntityProvideable world, IPoliceRespondable policeRespondable, ISeatAssignable seatAssignable, ISettingsProvideable settings, ITimeReportable time, IModItems modItems)
    {
        Player = player;
        World = world;
        PoliceRespondable = policeRespondable;
        Settings = settings;
        SeatAssignable = seatAssignable;
        Time = time;
        ModItems = modItems;
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
                        PlayEntryAnimation();
                        ReleaseCop();
                        EndSearch();
                        EntryPoint.WriteToConsole("Search Activity, Finished Searching Player");
                    }
                    else
                    {
                        ReleaseCop();
                        EndSearch();
                        EntryPoint.WriteToConsole("Search Activity, Failure Moving Cop To Search Position");
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
            EndSearch();
            EntryPoint.WriteToConsole("Booking Activity, No Cop Found");
        }
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

        //NativeFunction.Natives.TASK_GOTO_ENTITY_OFFSET(Cop.Pedestrian, Player.Character,-1,1.0f,-180f,1.0f,0);//does the same as below, but worse
        //TASK_GOTO_ENTITY_OFFSET( PED_INDEX PedIndex, ENTITY_INDEX EntityIndex,
        //INT Time = DEFAULT_TIME_BEFORE_WARP,
        //FLOAT SeekRadius = DEFAULT_SEEK_RADIUS,
        //FLOAT SeekAngle = 0.0,
        //FLOAT MoveBlendRatio = PEDMOVEBLENDRATIO_RUN,
        //ESEEK_ENTITY_OFFSET_FLAGS OffsetFlags = ESEEK_DEFAULT ) = "0x6624b56c8f9a7bbf"

        NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Cop.Pedestrian, CopTargetPosition.X, CopTargetPosition.Y, CopTargetPosition.Z, 1.0f, -1, CopTargetHeading, 0.1f);
    }
    private void CopMoveLoop()
    {
        isCopInPosition = false;
        while (CanContinueSearch)
        {
            if (CopTargetPosition.DistanceTo2D(Player.Character.GetOffsetPositionFront(-0.9f)) >= 0.1f)
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
    private void PlayEntryAnimation()
    {
        CameraControl cameraControl = new CameraControl(Player);
        if (Settings.SettingsManager.RespawnSettings.UseCustomCameraWhenBooking)
        {
            cameraControl.Setup();
            cameraControl.HighlightEntity(Player.Character);
        }
       // isPlayerCuffed = false;
        Cop.WeaponInventory.ShouldAutoSetWeaponState = false;
        Cop.WeaponInventory.SetUnarmed();
        NativeFunction.Natives.TASK_PLAY_ANIM(Cop.Pedestrian, CopDoSearchDictionary, CopDoSearchAnimation, 1.0f, -1.0f, -1, 2, 0, false, false, false);


        Player.Surrendering.SetArrestedAnimation(true);
        
        //NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, PlayerGetSearchDictionary, PlayerGetSearchedAnimation, 1.0f, -1.0f, -1, 0, 0, false, false, false);
        bool endLoop = false;
        //DoWeaponAndItemSearch();
        while (Cop.Pedestrian.Exists() && !endLoop)
        {
           
            float animTime = NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Cop.Pedestrian, CopDoSearchDictionary, CopDoSearchAnimation);
            if (animTime >= 0.4f)
            {
                if (!DidWeaponSearch)
                {
                    EntryPoint.WriteToConsole("Cop Search Do Weapons Search");
                    DoWeaponSearch();
                }
                if(FoundIllegalWeapons && !AnnouncedIllegalWeapons)
                {
                    EntryPoint.WriteToConsole("Cop Search Announce Found Weapons");
                    CopAnnounceFoundWeapon();
                }
            }

            if (animTime >= 0.7f)
            {
                if (!DidItemsSearch)
                {
                    EntryPoint.WriteToConsole("Cop Search Do Items Search");
                    DoItemSearch();
                    
                }
                if (FoundIllegalDrugs && !AnnouncedIllegalDrugs)
                {
                    EntryPoint.WriteToConsole("Cop Search Announce Found Drugs");
                    CopAnnounceFoundDrugs();
                }
            }

            if(animTime >= 1.0f)
            {
                CompletedSearch = true;
                if (!FoundIllegalItems)
                {
                    EntryPoint.WriteToConsole("Cop Search Announce Found Nothing");
                    CopAnnounceFoundNothing();
                }
                endLoop = true;
            }

            bool isAnimRunning = AnimationWatcher.IsAnimationRunning(animTime);
            if (!isAnimRunning)
            {
                EntryPoint.WriteToConsole("Cop Animation on Search Not Running");
                endLoop = true;
            }

            GameFiber.Yield();
        }
        EntryPoint.WriteToConsole("Cop Search ANIM LOOP ENDED");
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
            if (CanContinueSearch)
            {
                //NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                //Player.Character.KeepTasks = true;
                //NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, PlayerCuffedDictionary, PlayerCuffedAnimation, 1.0f, -1.0f, -1, 49, 0, 0, 1, 0);
               // isPlayerCuffed = true;
            }
        }
        if (Settings.SettingsManager.RespawnSettings.UseCustomCameraWhenBooking)
        {
            cameraControl.ReturnToGameplayCam();
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
    private void DoWeaponSearch()
    {
        bool hasCCW = Player.Licenses.HasValidCCWLicense(Time);
        DidWeaponSearch = true;
        List<WeaponInformation> IllegalWeapons = Player.WeaponEquipment.GetIllegalWeapons(hasCCW);
        WeaponInformation worstWeapon = IllegalWeapons.OrderByDescending(x => x.WeaponLevel).FirstOrDefault();
        if(worstWeapon == null)
        {
            return;
        }
        foreach (WeaponInformation weapon in IllegalWeapons)
        {
            int TimesToCheck = 1;
            WeaponItem wi = ModItems.PossibleItems.WeaponItems.FirstOrDefault(x => x.ModelName == weapon.ModelName);
            if(wi == null)
            {
                EntryPoint.WriteToConsole($"SEARCH WEAPON {weapon.ModelName} DID NOT FIND WEAPON, CONTINUING");
                continue;
            }
            if(weapon.Category == WeaponCategory.Throwable)
            {
                TimesToCheck = NativeFunction.Natives.GET_AMMO_IN_PED_WEAPON<int>(Player.Character, weapon.Hash);
                TimesToCheck.Clamp(1, 10);
            }
            EntryPoint.WriteToConsole($"SEARCH WEAPON {weapon.ModelName} FOUND MODITEM %:{wi.PoliceFindDuringPlayerSearchPercentage} TimesToCheck {TimesToCheck}");
            if (RandomItems.RandomPercent(wi.PoliceFindDuringPlayerSearchPercentage * TimesToCheck))
            {
                Player.Violations.WeaponViolations.AddFoundWeapon(worstWeapon, hasCCW);
                FoundIllegalWeapons = true;
                FoundIllegalItems = true;
                Player.WeaponEquipment.RemoveIllegalWeapons(hasCCW);
                EntryPoint.WriteToConsole($"SEARCH WEAPON {weapon.ModelName} PERCENTAGE MET, WEAPONS FOUND");
                break;
            }
        }
    }
    private void DoItemSearch()
    {
        DidItemsSearch = true;
        List<ModItem> IllegalItems = Player.Inventory.GetIllicitItems();
        foreach (ModItem modItem in IllegalItems)
        {
            int ItemsOwned = 1;
            InventoryItem ii = Player.Inventory.Get(modItem);
            if(ii != null)
            {
                ItemsOwned = ii.Amount;
            }
            EntryPoint.WriteToConsole($"SEARCH WEAPON {modItem.Name} %:{modItem.PoliceFindDuringPlayerSearchPercentage} ItemsOwned {ItemsOwned} Total%{modItem.PoliceFindDuringPlayerSearchPercentage * ItemsOwned}");
            if (RandomItems.RandomPercent(modItem.PoliceFindDuringPlayerSearchPercentage * ItemsOwned))
            {
                Player.Violations.OtherViolations.AddFoundIllegalItem();
                FoundIllegalDrugs = true;
                FoundIllegalItems = true;
                Player.Inventory.RemoveIllicitInventoryItems();
                EntryPoint.WriteToConsole($"SEARCH ITEMS {modItem.Name} PERCENTAGE MET, ITEMS FOUND");
                break;
            }
        }
    }
}