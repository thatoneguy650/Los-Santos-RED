using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI.Elements;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using NAudio.Wave;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

public class ActivityManager
{
    private uint GameTimeLastYelled;
    private int TimeBetweenYelling = 2500;
    private uint GameTimeLastClosedDoor;

    private IActivityManageable Player;
    private ISettingsProvideable Settings;
    private IActionable Actionable;
    private IIntoxicatable Intoxicatable;
    private IInteractionable Interactionable;
    private IPlateChangeable PlateChangeable;
    private ILocationInteractable LocationInteractable;
    private IPlacesOfInterest PlacesOfInterest;

    private ITimeControllable Time;
    private IRadioStations RadioStations;
    private ICrimes Crimes;
    private IModItems ModItems;
    private IDances Dances;
    private IEntityProvideable World;
    private IIntoxicants Intoxicants;
    private ISpeeches Speeches;
    private ISeats Seats;
    private IWeapons Weapons;
    private IZones Zones;
    private IShopMenus ShopMenus;
    private IGangs Gangs;
    private IGangTerritories GangTerritories;
    private ICellphones Cellphones;
    private IVehicleSeatAndDoorLookup VehicleSeatDoorData;

    private DynamicActivity LowerBodyActivity;
    private DynamicActivity UpperBodyActivity;
    private ICameraControllable CameraControllable;

    private bool canSeePoliceBlips = false;



    private MenuPool MenuPool;
    private UIMenu continueActivityMenu;
    private uint GameTimeLastSetBlips;
    private bool IsDoingVehileAnim;
    private uint GameTimeLastStartedHotwiring;

    public bool IsUsingToolAsWeapon { get; set; }



    public bool HasScrewdriverInHand { get; set; }
    public ScrewdriverItem CurrentScrewdriver { get; set; }

    public bool HasDrillInHand { get; set; }
    public DrillItem CurrentDrill { get; set; }

    //DO I NEED ALL 3?
    public bool CanPerformActivitesBase => Player.IsAliveAndFree && !Player.CuffManager.IsHandcuffed && !Player.IsIncapacitated && !Player.IsGettingIntoAVehicle && !Player.RecentlyGotOutOfVehicle && !Player.IsBreakingIntoCar && (!Player.IsInVehicle || Player.CurrentVehicle == null || Player.CurrentVehicle.CanPerformActivitiesInside);// && (Interaction == null || Interaction.CanPerformActivities);
    public bool CanPerformActivitiesExtended => CanPerformActivitesBase && (!Player.IsMovingFast || (Player.IsInVehicle && Player.CurrentVehicle != null && Player.CurrentVehicle.CanPerformActivitiesInside == true)) && !Player.IsMovingDynamically;
    public bool CanPerformActivitiesMiddle => CanPerformActivitesBase && (!Player.IsMovingFast || (Player.IsInVehicle && Player.CurrentVehicle != null && Player.CurrentVehicle.CanPerformActivitiesInside == true));
    public bool CanPerformActivitiesOnFoot => CanPerformActivitesBase && !Player.IsInVehicle;







    //ALL THESE BOOLS GOTTA GO

    public bool CanConverse =>
        Player.IsAliveAndFree &&
        !Player.IsIncapacitated &&
        !Player.IsVisiblyArmed &&
        !Player.IsMovingDynamically &&
        ((Player.IsInVehicle && Player.VehicleSpeedMPH <= 5f) || !Player.IsMovingFast) &&
        (LowerBodyActivity == null || !IsPerformingActivity);
    public bool CanConverseWithLookedAtPed => 
        Player.CurrentLookedAtPed != null && 
        Player.CurrentTargetedPed == null && 
        Player.CurrentLookedAtPed.CanConverse && 
        !Player.RelationshipManager.GangRelationships.IsHostile(Player.CurrentLookedAtGangMember?.Gang) && 
        (!Player.CurrentLookedAtPed.IsCop || (Player.IsNotWanted && !Player.Investigation.IsActive)) && 
        CanConverse;
   
    





    public bool CanGrabLookedAtPed => 
        Player.CurrentLookedAtPed != null && 
        Player.CurrentTargetedPed == null && 
        CanGrabPed && 
        !Player.CurrentLookedAtPed.IsInVehicle && 
        !Player.CurrentLookedAtPed.IsUnconscious && 
        !Player.CurrentLookedAtPed.IsDead && 
        Player.CurrentLookedAtPed.DistanceToPlayer <= 5.0f && 
        Player.CurrentLookedAtPed.Pedestrian.Exists() && 
        !Player.CurrentLookedAtPed.Pedestrian.IsRagdoll && 
        Player.CurrentLookedAtPed.Pedestrian.IsThisPedInFrontOf(Player.Character) && 
        !Player.Character.IsThisPedInFrontOf(Player.CurrentLookedAtPed.Pedestrian);



    public bool CanGrabPed =>
        CanPerformActivitiesOnFoot &&
        !IsPerformingActivity && 
        (Player.CanBustPeds || Player.WeaponEquipment.CurrentWeapon?.CanPistolSuicide == true);

    public bool CanHoldUpTargettedPed => 
        Player.CurrentTargetedPed != null && 
        Player.CurrentTargetedPed.CanBeMugged &&
        CanPerformActivitesBase &&
        !IsPerformingActivity &&
        Player.IsVisiblyArmed && 
        !Player.CurrentTargetedPed.PedViolations.IsVisiblyArmed && 
        Player.CurrentTargetedPed.DistanceToPlayer <= Settings.SettingsManager.ActivitySettings.HoldUpDistance;

    public bool CanInspectPed => 
        CanPerformActivitiesExtended &&
        CanPerformActivitiesOnFoot && 
        !IsPerformingActivity;

    public bool CanInspectLookedAtPed => 
        Player.CurrentLookedAtPed != null && 
        Player.CurrentTargetedPed == null && 
        CanInspectPed && 
        !Player.CurrentLookedAtPed.IsInVehicle && 
        (Player.CurrentLookedAtPed.IsUnconscious || Player.CurrentLookedAtPed.IsDead);

    public bool CanDrag =>
        CanPerformActivitiesExtended &&
        CanPerformActivitiesOnFoot &&
        !IsPerformingActivity;
    public bool CanDragLookedAtPed => 
        Player.CurrentLookedAtPed != null && 
        Player.CurrentTargetedPed == null && 
        CanDrag && 
        Player.CurrentLookedAtPed.CanBeDragged && 
        !Player.CurrentLookedAtPed.IsInVehicle && 
        (Player.CurrentLookedAtPed.IsUnconscious || Player.CurrentLookedAtPed.IsDead);
     
    public bool CanRecruitLookedAtGangMember => 
        Player.CurrentLookedAtGangMember != null &&
        Player.CurrentTargetedPed == null && 
        //Player.CurrentLookedAtGangMember.WasModSpawned &&
        Player.RelationshipManager.GangRelationships.CurrentGang != null && 
        Player.CurrentLookedAtGangMember.Gang != null && 
        Player.RelationshipManager.GangRelationships.CurrentGang.ID == Player.CurrentLookedAtGangMember.Gang.ID &&
        !Player.GroupManager.IsMember(Player.CurrentLookedAtGangMember);


    public string ContinueCurrentActivityPrompt => UpperBodyActivity != null ? UpperBodyActivity.ContinuePrompt : LowerBodyActivity != null ? LowerBodyActivity.ContinuePrompt : "";
    public string CancelCurrentActivityPrompt => UpperBodyActivity != null ? UpperBodyActivity.CancelPrompt : LowerBodyActivity != null ? LowerBodyActivity.CancelPrompt : "";
    public string PauseCurrentActivityPrompt => UpperBodyActivity != null ? UpperBodyActivity.PausePrompt : LowerBodyActivity != null ? LowerBodyActivity.PausePrompt : "";
  
    
    
    
    
    public bool CanCancelCurrentActivity => UpperBodyActivity?.CanCancel == true || LowerBodyActivity?.CanCancel == true;
    public bool CanPauseCurrentActivity => UpperBodyActivity?.CanPause == true || LowerBodyActivity?.CanPause == true;
    public bool IsCurrentActivityPaused => UpperBodyActivity?.IsPaused() == true || LowerBodyActivity?.IsPaused() == true;
   // public bool HasCurrentActivity => UpperBodyActivity != null || LowerBodyActivity != null;


    public bool IsResting => IsSitting || IsLayingDown;
    public bool IsPerformingActivity { get; set; }


    public bool IsSitting { get; set; }
    public bool IsLayingDown { get; set; } = false;
    public bool IsCommitingSuicide { get; set; }
    public bool IsLootingBody { get; set; }

    public bool IsTreatingPed { get; set; }

    public bool IsDraggingBody { get; set; }
    public bool IsHoldingHostage { get; set; }
    public bool IsDancing { get; set; }
    public bool IsConversing { get; set; }
    public bool IsHoldingUp { get; set; }
    public bool IsInteractingWithLocation { get; set; } = false;
    public bool IsInteracting => IsConversing || IsHoldingUp;
    private bool CanYell => !IsYellingTimeOut;
    private bool IsYellingTimeOut => Game.GameTime - GameTimeLastYelled < TimeBetweenYelling;
    public GestureData LastGesture { get; set; }
    public DanceData LastDance { get; set; }
    public Interaction Interaction { get; private set; }
    public DynamicActivity Activity => UpperBodyActivity != null ? UpperBodyActivity : LowerBodyActivity;


    public List<DynamicActivity> PausedActivites { get; set; } = new List<DynamicActivity>();
    public bool IsWavingHands { get; set; }
    public bool IsBuryingBody { get; set; }

    public bool IsHailingTaxi { get; set; }

    public bool HasScannerOut { get; set; }
    public bool CanHearScanner => !Settings.SettingsManager.ScannerSettings.DisableScannerWithoutRadioItem || Player.Inventory.Has(typeof(RadioItem));
    public bool CanSeePoliceBlips => !Settings.SettingsManager.PoliceSpawnSettings.ShowSpawnedBlips && Settings.SettingsManager.ScannerSettings.ShowPoliceVehicleBlipsWithScanner && Player.Inventory.Has(typeof(RadioItem)) && (Player.IsInVehicle || HasScannerOut);

    public bool IsEnteringAsPassenger { get; set; }
    public bool IsUrinatingDefecting { get; set; }
    public bool IsUrinatingDefectingOnToilet { get; set; }
    public bool IsUsingIllegalItem { get; internal set; }

    public ActivityManager(IActivityManageable player, ISettingsProvideable settings, IActionable actionable, IIntoxicatable intoxicatable, IInteractionable interactionable, ICameraControllable cameraControllable, ILocationInteractable locationInteractable,
        ITimeControllable time, IRadioStations radioStations, ICrimes crimes, IModItems modItems, 
        IDances dances, IEntityProvideable world, IIntoxicants intoxicants, IPlateChangeable plateChangeable, ISpeeches speeches, ISeats seats, IWeapons weapons, IPlacesOfInterest placesOfInterest, IZones zones, IShopMenus shopMenus, IGangs gangs, IGangTerritories gangTerritories,
        IVehicleSeatAndDoorLookup vehicleSeatDoorData, ICellphones cellphones)
    {
        Player = player;
        Settings = settings;
        Actionable = actionable;
        Intoxicatable = intoxicatable;
        Interactionable = interactionable;
        CameraControllable = cameraControllable;
        LocationInteractable = locationInteractable;
        Time = time;
        RadioStations = radioStations;
        Crimes = crimes;
        ModItems = modItems;
        Dances = dances;
        World = world;
        Intoxicants = intoxicants;
        PlateChangeable = plateChangeable;
        Speeches = speeches;
        Seats = seats;
        Weapons = weapons;
        PlacesOfInterest = placesOfInterest;
        Zones = zones;
        ShopMenus = shopMenus;
        Gangs = gangs;
        GangTerritories = gangTerritories;
        VehicleSeatDoorData = vehicleSeatDoorData;
        Cellphones = cellphones;
    }
    public void Setup()
    {
        LastGesture = new GestureData("Thumbs Up Quick", "anim@mp_player_intselfiethumbs_up", "enter");
        LastDance = Dances.GetRandomDance();

        AnimationDictionary.RequestAnimationDictionay("facials@gen_female@base");
        AnimationDictionary.RequestAnimationDictionay("facials@gen_male@base");
        AnimationDictionary.RequestAnimationDictionay("facials@p_m_zero@base");
        AnimationDictionary.RequestAnimationDictionay("facials@p_m_one@base");
        AnimationDictionary.RequestAnimationDictionay("facials@p_m_two@base");
    }
    public void Dispose()
    {
        Interaction?.Dispose();
        Interaction = null;
        ForceCancelAllActivities();
    }
    public void Update()
    {
        if (CanSeePoliceBlips)
        {
            World.Vehicles.UpdatePoliceSonarBlips(true);
        }
        if (canSeePoliceBlips != CanSeePoliceBlips)
        {
            if(!CanSeePoliceBlips)
            {
                World.Vehicles.UpdatePoliceSonarBlips(false);
            }
            EntryPoint.WriteToConsole($"CanSeePoliceBlips changed to {CanSeePoliceBlips}");
            canSeePoliceBlips = CanSeePoliceBlips;
        }

    }
    public void Reset()
    {
        IsPerformingActivity = false;
        ForceCancelAllActivities();
    }
    public void AddPausedActivity(DynamicActivity da)
    {
        UpperBodyActivity = null;
        LowerBodyActivity = null;
        if (!PausedActivites.Contains(da))
        {
            PausedActivites.Add(da);
        }
    }
    public void ForceCancelAllActivities()
    {
        ForceCancelAllActive();
        ForceCancelAllPaused();
    }
    private void ForceCancelUpperBody()
    {
        //EntryPoint.WriteToConsole("ForceCancelUpperBody");
        if (UpperBodyActivity != null)
        {
            Player.ButtonPrompts.RemoveActivityPrompts();
            UpperBodyActivity.Cancel();
            UpperBodyActivity = null;
        }
    }
    private void ForceCancelLowerBody()
    {
        //EntryPoint.WriteToConsoleTestLong("ForceCancelLowerBody");
        if (LowerBodyActivity != null)
        {
            Player.ButtonPrompts.RemoveActivityPrompts();
            LowerBodyActivity.Cancel();
            LowerBodyActivity = null;
        }
    }
    private void ForceCancelAllActive()
    {
        ForceCancelUpperBody();
        ForceCancelLowerBody();
    }
    public void ForceCancelAllPaused()
    {
        //EntryPoint.WriteToConsoleTestLong("ForceCancelAllPaused");
        foreach (DynamicActivity da in PausedActivites)
        {
            da.Cancel();
        }
        PausedActivites.Clear();
    }
    public void ContinueCurrentActivity()
    {
        if (PausedActivites.Count > 1)
        {
            MenuPool = new MenuPool();
            continueActivityMenu = new UIMenu("Activity", "Select an activity");
            continueActivityMenu.RemoveBanner();
            MenuPool.Add(continueActivityMenu);
            foreach(DynamicActivity da in PausedActivites)
            {
                UIMenuItem uii = new UIMenuItem(da.ModItem?.Name);
                uii.Activated += (menu, item) =>
                {
                    UpperBodyActivity = da;
                    da.Continue();
                    menu.Visible = false;
                };
                continueActivityMenu.AddItem(uii);
            }
            continueActivityMenu.Visible = true;
            GameFiber activityMenuWatcher = GameFiber.StartNew(delegate
            {
                try
                {
                    while (continueActivityMenu.Visible)
                    {
                        MenuPool.ProcessMenus();
                        GameFiber.Yield();
                    }
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }

            }, "ActivityMenuWatcher");
        }
        else
        {
            DynamicActivity toContinue = PausedActivites.FirstOrDefault();
            if (toContinue != null && toContinue.CanPause)
            {
                UpperBodyActivity = toContinue;
                toContinue.Continue();
            }
        }
    }
    public void PauseCurrentActivity()
    {
        if (UpperBodyActivity != null && UpperBodyActivity.CanPause)
        {
            UpperBodyActivity.Pause();
        }
        else if (LowerBodyActivity != null && LowerBodyActivity.CanPause)
        {
            LowerBodyActivity.Pause();
        }
    }
    public void CancelCurrentActivity()
    {
        foreach(ButtonPrompt bp in Player.ButtonPrompts.Prompts)
        {
            if(bp.IsPressedNow || bp.IsHeldNow || bp.IsFakePressed)
            {
                //EntryPoint.WriteToConsoleTestLong($"BP PRESSED: {bp.Text} IsPressedNow{bp.IsPressedNow} IsHeldNow{bp.IsHeldNow} IsAlternativePressed{bp.IsFakePressed}");
            }
            else
            {
                //EntryPoint.WriteToConsoleTestLong($"BP NOT PRESSED: {bp.Text}");
            }
        }
        //EntryPoint.WriteToConsoleTestLong("CancelCurrentActivity");
        if (UpperBodyActivity != null && UpperBodyActivity.CanCancel)
        {
            Player.ButtonPrompts.RemoveActivityPrompts();
            UpperBodyActivity.Cancel();
            UpperBodyActivity = null;
        }
        else if (LowerBodyActivity != null && LowerBodyActivity.CanCancel)
        {
            Player.ButtonPrompts.RemoveActivityPrompts();
            LowerBodyActivity.Cancel();
            LowerBodyActivity = null;
        }
    }
    public void StopDynamicActivity()
    {
        if (IsPerformingActivity)
        {
            //EntryPoint.WriteToConsoleTestLong("StopDynamicActivity");
            Player.ButtonPrompts.RemoveActivityPrompts();
            UpperBodyActivity?.Cancel();
            UpperBodyActivity = null;
            IsPerformingActivity = false;
        }
    }
    public void StartUpperBodyActivity(DynamicActivity toStart)
    {
        if (!PausedActivites.Any(x => x.GetType() == toStart.GetType()))
        {
            ForceCancelUpperBody();
            IsPerformingActivity = true;
            UpperBodyActivity = toStart;
            UpperBodyActivity.Start();
        }
        else
        {
            Game.DisplayHelp("Stop existing activity to start");
        }
    }
    public void StartLowerBodyActivity(DynamicActivity toStart)
    {
        if (!PausedActivites.Any(x => x.GetType() == toStart.GetType()))
        {
            ForceCancelUpperBody();
            IsPerformingActivity = true;
            LowerBodyActivity = toStart;
            LowerBodyActivity.Start();
        }
        else
        {
            Game.DisplayHelp("Stop existing activity to start");
        }
    }

    //Dynamic Activites w/ IsPerforming
    public void RemovePlate()
    {
        if(IsPerformingActivity)
        {
            Game.DisplayHelp("Cancel existing activity to start");
            return;
        }
        PlateTheft plateTheft = new PlateTheft(Actionable, Settings, World, CurrentScrewdriver);
        if(plateTheft.CanPerform(Actionable))
        {
            ModItem li = Player.Inventory.Get(typeof(ScrewdriverItem))?.ModItem;
            if (li == null)
            {
                Game.DisplayHelp($"Need a ~r~Screwdriver~s~ to remove plates.");
                return;
            }
            ForceCancelAllActive();
            IsPerformingActivity = true;
            LowerBodyActivity = plateTheft;
            LowerBodyActivity.Start();
        }
    }
    public void CommitSuicide()
    {
        if (IsPerformingActivity)
        {
            Game.DisplayHelp("Cancel existing activity to start");
            return;
        }
        SuicideActivity suicideActivity = new SuicideActivity(Actionable, Settings);
        if (suicideActivity.CanPerform(Actionable))
        {
            ForceCancelAllActive();
            IsPerformingActivity = true;
            LowerBodyActivity = suicideActivity;
            LowerBodyActivity.Start();
        }
    }
    public void Gesture(GestureData gestureData)
    {
        if (IsPerformingActivity)
        {
            Game.DisplayHelp("Cancel existing activity to start");
            return;
        }
        GestureActivity gestureActivity = new GestureActivity(Actionable, gestureData);
        if (gestureActivity.CanPerform(Actionable))
        {
            ForceCancelUpperBody();
            IsPerformingActivity = true;
            LastGesture = gestureData;
            UpperBodyActivity = gestureActivity;
            UpperBodyActivity.Start();
        }
    }
    public void Gesture()
    {
        Gesture(LastGesture);
    }
    public void WaveHands()
    {
        if (IsPerformingActivity)
        {
            Game.DisplayHelp("Cancel existing activity to start");
            return;
        }
        WaveHandsActivity waveHandsActivity = new WaveHandsActivity(Actionable, World, Settings);
        if (waveHandsActivity.CanPerform(Actionable))
        {
            ForceCancelUpperBody();
            IsPerformingActivity = true;
            UpperBodyActivity = waveHandsActivity;
            UpperBodyActivity.Start();
        }
    }
    public void HailTaxi()
    {
        if (IsPerformingActivity)
        {
            Game.DisplayHelp("Cancel existing activity to start");
            return;
        }
        HailCabActivity hailCabactivity = new HailCabActivity(Actionable, World, Settings);
        if (hailCabactivity.CanPerform(Actionable))
        {
            ForceCancelUpperBody();
            IsPerformingActivity = true;
            UpperBodyActivity = hailCabactivity;
            UpperBodyActivity.Start();
        }
    }

    public void DismissTaxi()
    {
        if (IsPerformingActivity)
        {
            Game.DisplayHelp("Cancel existing activity to start");
            return;
        }
        DismissCabActivity releaseCabActivity = new DismissCabActivity(Actionable, World, Settings);
        if (releaseCabActivity.CanPerform(Actionable))
        {
            ForceCancelUpperBody();
            IsPerformingActivity = true;
            UpperBodyActivity = releaseCabActivity;
            UpperBodyActivity.Start();
        }
    }

    public void Dance(DanceData danceData)
    {
        if (IsPerformingActivity)
        {
            Game.DisplayHelp("Cancel existing activity to start");
            return;
        }
        DanceActivity danceActivity = new DanceActivity(Actionable, danceData, RadioStations, Settings, Dances);
        if (danceActivity.CanPerform(Actionable))
        {
            ForceCancelAllActive();
            LastDance = danceData;
            LowerBodyActivity = danceActivity;
            LowerBodyActivity.Start();
        }
    }
    public void Dance()
    {
        StopDynamicActivity();
        LastDance = Dances.DanceLookups.PickRandom();
        Dance(LastDance);
    }  
    public void UseInventoryItem(ModItem modItem, bool performActivity)
    {
        if(!performActivity)
        {
            Time.FastForward(Time.CurrentDateTime.AddMinutes(3));

            //GameFiber ScenarioWatcher = GameFiber.StartNew(delegate
            //{
            //    try
            //    {
                    modItem.ConsumeItemSlow(Actionable, Settings.SettingsManager.NeedsSettings.ApplyNeeds, Settings);
            //    }
            //    catch (Exception ex)
            //    {
            //        EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
            //        EntryPoint.ModController.CrashUnload();
            //    }
            //}, "Consumewatcher");
        }
        else
        {
            if (IsPerformingActivity)
            {
                Game.DisplayHelp("Cancel existing activity to start");
                return;
            }
            modItem.UseItem(Actionable, Settings, World, CameraControllable, Intoxicants, Time);
        }
    }
    public void DropInventoryItem(ModItem modItem, int amount)
    {
        if (IsPerformingActivity)
        {
            Game.DisplayHelp("Cancel existing activity to start");
            return;
        }
        modItem.DropItem(Actionable, Settings, amount); 
    }
    public void StartScenario()
    {
        if (IsPerformingActivity)
        {
            Game.DisplayHelp("Cancel existing activity to start");
            return;
        }
        ScenarioActivity scenarioActivity = new ScenarioActivity(Intoxicatable);
        if (scenarioActivity.CanPerform(Actionable))
        {
            ForceCancelUpperBody();
            IsPerformingActivity = true;
            UpperBodyActivity = scenarioActivity;
            UpperBodyActivity.Start();
        }
    }
    //Dynamic Activities W/o Performing
    public void GrabPed()
    {
        if (IsPerformingActivity)
        {
            Game.DisplayHelp("Cancel existing activity to start");
            return;
        }
        DynamicActivity toPerform;
        if(Player.CanBustPeds)
        {
            toPerform = new PedGrab(Interactionable, Player.CurrentLookedAtPed, Settings, Crimes, ModItems, World);
        }
        else
        {
            toPerform = new HumanShield(Interactionable, Player.CurrentLookedAtPed, Settings, Crimes, ModItems, World);
        }
        if (toPerform.CanPerform(Actionable))
        {
            ForceCancelAllActive();
            IsPerformingActivity = true;
            LowerBodyActivity = toPerform;
            LowerBodyActivity.Start();
        }
    }
    public void InspectPed()
    {
        if (IsPerformingActivity)
        {
            Game.DisplayHelp("Cancel existing activity to start");
            return;
        }
        PedInspect pedInspect = new PedInspect(Interactionable, Player.CurrentLookedAtPed, Settings, Crimes, ModItems, Cellphones);
        if (pedInspect.CanPerform(Actionable))
        {
            ForceCancelAllActive();
            IsPerformingActivity = true;
            LowerBodyActivity = pedInspect;
            LowerBodyActivity.Start();
        }
    }
    //public void TreatPed()
    //{
    //    if (IsPerformingActivity)
    //    {
    //        Game.DisplayHelp("Cancel existing activity to start");
    //        return;
    //    }
    //    TreatmentActivity_Old treatmentActivity = new TreatmentActivity_Old(Interactionable, Player.CurrentLookedAtPed, Settings, Crimes, ModItems);
    //    if (treatmentActivity.CanPerform(Actionable))
    //    {
    //        ForceCancelAllActive();
    //        IsPerformingActivity = true;
    //        LowerBodyActivity = treatmentActivity;
    //        LowerBodyActivity.Start();
    //    }
    //}
    public void DragPed()
    {
        if (IsPerformingActivity)
        {
            Game.DisplayHelp("Cancel existing activity to start");
            return;
        }
        if (!Settings.SettingsManager.DebugSettings.UseNewDrag)
        {
            Drag drag = new Drag(Interactionable, Player.CurrentLookedAtPed, Settings, Crimes, ModItems, World, VehicleSeatDoorData);
            if (drag.CanPerform(Actionable))
            {
                ForceCancelAllActive();
                IsPerformingActivity = true;
                LowerBodyActivity = drag;
                LowerBodyActivity.Start();
            }
        }
        else
        {
            NewDrag drag = new NewDrag(Interactionable, Player.CurrentLookedAtPed, Settings, Crimes, ModItems, World, VehicleSeatDoorData);
            if (drag.CanPerform(Actionable))
            {
                ForceCancelAllActive();
                IsPerformingActivity = true;
                LowerBodyActivity = drag;
                LowerBodyActivity.Start();
            }
        }
    }
    public void StartSleeping()
    {
        if (IsPerformingActivity)
        {
            Game.DisplayHelp("Cancel existing activity to start");
            return;
        }
        SleepingActivity sleeping = new SleepingActivity(Actionable, Settings, Time);
        if (sleeping.CanPerform(Actionable))
        {
            ForceCancelAllActive();
            IsPerformingActivity = true;
            LowerBodyActivity = sleeping;
            LowerBodyActivity.Start();
        }
    }


    public void Urinate()
    {
        if (IsPerformingActivity)
        {
            Game.DisplayHelp("Cancel existing activity to start");
            return;
        }
        UrinatingActivity urinating = new UrinatingActivity(Actionable, Settings);
        if (urinating.CanPerform(Actionable))
        {
            ForceCancelAllActive();
            IsPerformingActivity = true;
            LowerBodyActivity = urinating;
            LowerBodyActivity.Start();
        }
    }

    public void Defecate()
    {
        if (IsPerformingActivity)
        {
            Game.DisplayHelp("Cancel existing activity to start");
            return;
        }
        DefecatingActivity defecating = new DefecatingActivity(Actionable, Settings);
        if (defecating.CanPerform(Actionable))
        {
            ForceCancelAllActive();
            IsPerformingActivity = true;
            LowerBodyActivity = defecating;
            LowerBodyActivity.Start();
        }
    }



    public void StartSittingDown(bool findSittingProp, bool enterForward)
    {
        if (IsPerformingActivity)
        {
            Game.DisplayHelp("Cancel existing activity to start");
            return;
        }
        SittingActivity sitting = new SittingActivity(Actionable, Settings, findSittingProp, enterForward, Seats, CameraControllable);

        if (sitting.CanPerform(Actionable))
        {
            ForceCancelAllActive();
            IsPerformingActivity = true;
            LowerBodyActivity = sitting;
            LowerBodyActivity.Start();
        }
    }

    public void StartSittingOnToilet(bool findSittingProp, bool enterForward)
    {
        if (IsPerformingActivity)
        {
            Game.DisplayHelp("Cancel existing activity to start");
            return;
        }
        SittingActivity sitting = new SittingActivity(Actionable, Settings, findSittingProp, enterForward, Seats, CameraControllable);
        sitting.IsSittingOnToilet = true;
        if (sitting.CanPerform(Actionable))
        {
            ForceCancelAllActive();
            IsPerformingActivity = true;
            LowerBodyActivity = sitting;
            LowerBodyActivity.Start();
        }
    }


    //Interactions
    public void StartLocationInteraction()
    {

        if (!IsInteracting && !IsInteractingWithLocation)
        {
            if (Interaction != null)
            {
                Interaction.Dispose();
            }
            ForceCancelUpperBody();//was only if performing
            try
            {
                Player.ClosestInteractableLocation?.OnInteract();// LocationInteractable, ModItems, World, Settings, Weapons, Time, PlacesOfInterest);
            }
            catch(Exception e) 
            {
                EntryPoint.WriteToConsole("Location Interaction: " + e.StackTrace + e.Message, 0);
            }
        }
    }
    public void StartConversation()
    {
        if (!IsInteracting && CanConverseWithLookedAtPed)
        {
            if (Interaction != null)
            {
                Interaction.Dispose();
            }
            Interaction = new Conversation(Interactionable, Player.CurrentLookedAtPed, Settings, Crimes, ModItems, Zones, ShopMenus, PlacesOfInterest, Gangs, GangTerritories, Speeches, World, LocationInteractable);
            Interaction.Start();  
        }
    }
    public void StartHoldUp()
    {
        if (!IsInteracting && CanHoldUpTargettedPed)
        {
            if (Interaction != null)
            {
                Interaction.Dispose();
            }
            Interaction = new HoldUp(Interactionable, Player.CurrentTargetedPed, Settings, ModItems, Cellphones);
            Interaction.Start();
        }
    }
    public void OnTargetHandleChanged()
    {
        if (Settings.SettingsManager.KeySettings.HoldUpPedGameControl >= 0 && Settings.SettingsManager.ActivitySettings.AllowPedHoldUps && !IsInteracting && Player.IsOnFoot && CanHoldUpTargettedPed && Player.CurrentTargetedPed != null && Player.CurrentTargetedPed.CanBeMugged && (!Player.IsCop || Player.CurrentTargetedPed.IsNotWanted))//isinvehicle added here
        {
            Player.ButtonPrompts.AttemptAddPrompt("HoldUp", $"HoldUp {Player.CurrentTargetedPed.FormattedName}", $"HoldUp {Player.CurrentTargetedPed.Handle}", (GameControl)Settings.SettingsManager.KeySettings.HoldUpPedGameControl, 999);
            //StartHoldUp();
        }
        else
        {
            Player.ButtonPrompts.RemovePrompts("HoldUp");
        }
    }
    public void StartTransaction()
    {
        if (!IsInteracting && CanConverseWithLookedAtPed)
        {
            if (Interaction != null)
            {
                Interaction.Dispose();
            }
            Merchant merchant = World.Pedestrians.Merchants.FirstOrDefault(x => x.Handle == Player.CurrentLookedAtPed.Handle);
            try
            {
                GameLocation associatedStore = null;
                if (merchant != null)
                {
                    associatedStore = merchant.AssociatedStore;
                }
                Interaction = new PersonTransaction(LocationInteractable, Player.CurrentLookedAtPed, Player.CurrentLookedAtPed.ShopMenu, ModItems, World, Settings, Weapons, Time) { AssociatedStore = associatedStore };
                Interaction.Start();
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Interaction: " + e.StackTrace + e.Message, 0);
            }
        }
    }
    public void StartTransaction(PedExt pedExt)
    {
        if (!IsInteracting)
        {
            if (Interaction != null)
            {
                Interaction.Dispose();
            }
            Merchant merchant = World.Pedestrians.Merchants.FirstOrDefault(x => x.Handle == pedExt.Handle);
            try
            {
                GameLocation associatedStore = null;
                if (merchant != null)
                {
                    associatedStore = merchant.AssociatedStore;
                }
                Interaction = new PersonTransaction(LocationInteractable, pedExt, pedExt.ShopMenu, ModItems, World, Settings, Weapons, Time) { AssociatedStore = associatedStore, DoGreet = false };
                Interaction.Start();
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Interaction: " + e.StackTrace + e.Message, 0);
            }
        }
    }
    //Other
    public void EnterVehicleAsPassenger(bool withBlocking, bool onlyBack, bool stopDriver)
    {
        if(IsEnteringAsPassenger)
        {
            return;
        }

        EntryPoint.WriteToConsole("ENTER AS PASSENGER RAN");

        VehicleExt toEnter = GetInterestedVehicle();
        if (toEnter == null || !toEnter.Vehicle.Exists())
        {
            return;
        }
        int? seatIndex = null;
        if (onlyBack)
        {
            int TotalSeats = NativeFunction.Natives.GET_VEHICLE_MODEL_NUMBER_OF_SEATS<int>(toEnter.Vehicle.Model.Hash);

            if (TotalSeats < 4)
            {
                seatIndex = toEnter.Vehicle.GetFreePassengerSeatIndex();

            }
            else
            {
                seatIndex = toEnter.Vehicle.GetFreeSeatIndex(1, 2);
            }
        }
        else
        {
            seatIndex = toEnter.Vehicle.GetFreePassengerSeatIndex();
        }
        if (seatIndex == null)
        {
            return;
        }
        if (withBlocking)
        {
            foreach (Ped passenger in toEnter.Vehicle.Occupants)
            {
                if (passenger.Exists())
                {
                    //passenger.CanBePulledOutOfVehicles = false;//when does this get turned off  ?
                    passenger.StaysInVehiclesWhenJacked = true;
                    passenger.BlockPermanentEvents = true;
                }
            }
        }
        IsEnteringAsPassenger = true;
        Player.LastFriendlyVehicle = toEnter.Vehicle;


        //if(toEnter.ModelName().ToLower() == "oppressor")
        //{
        //    DoComplicatedVehiclePassengerEntry();
        //}

        NativeFunction.Natives.TASK_ENTER_VEHICLE(Player.Character, toEnter.Vehicle, -1, seatIndex, 1f, (int)eEnter_Exit_Vehicle_Flags.ECF_RESUME_IF_INTERRUPTED | (int)eEnter_Exit_Vehicle_Flags.ECF_DONT_JACK_ANYONE);
        WatchVehicleEntry(toEnter, stopDriver);
    }
    public void EnterVehicleInSpecificSeat(bool withBlocking, int seatIndex, bool addFriendly, bool stopDriver)
    {
        VehicleExt toEnter = GetInterestedVehicle();
        if (toEnter == null || !toEnter.Vehicle.Exists() || !toEnter.Vehicle.IsSeatFree(seatIndex))
        {
            return;
        }
        if (withBlocking)
        {
            foreach (Ped passenger in toEnter.Vehicle.Occupants)
            {
                if (passenger.Exists())
                {
                    //passenger.CanBePulledOutOfVehicles = false;//when does this get turned off  ?
                    passenger.StaysInVehiclesWhenJacked = true;
                    passenger.BlockPermanentEvents = true;
                }
            }
        }
        if (addFriendly)
        {
            Player.LastFriendlyVehicle = toEnter.Vehicle;
        }
        NativeFunction.Natives.TASK_ENTER_VEHICLE(Player.Character, toEnter.Vehicle, -1, seatIndex, 1f, (int)eEnter_Exit_Vehicle_Flags.ECF_RESUME_IF_INTERRUPTED | (int)eEnter_Exit_Vehicle_Flags.ECF_DONT_JACK_ANYONE);
        WatchVehicleEntry(toEnter, stopDriver);
    }
    public void ToggleDoor(int doorIndex, bool withAnimation, VehicleExt mainVehicle)
    {
        VehicleExt toToggleDoor = null;
        if (mainVehicle != null)
        {
            toToggleDoor = mainVehicle;
        }
        else
        {
            toToggleDoor = GetInterestedVehicle();
        }
        if (toToggleDoor == null || !toToggleDoor.Vehicle.Exists())
        {
            return;
        }
        if (withAnimation)
        {
            if (IsPerformingActivity)
            {
                Game.DisplayHelp("Cancel existing activity to start");
                return;
            }
            DoorToggle doorToggle = new DoorToggle(Actionable, Settings, World, toToggleDoor, doorIndex, false,false);
            if (doorToggle.CanPerform(Actionable))
            {
                ForceCancelAllActive();
                IsPerformingActivity = true;
                LowerBodyActivity = doorToggle;
                LowerBodyActivity.Start();
            }
        }
        else
        {
            if (toToggleDoor.Vehicle.Doors[doorIndex].IsOpen)
            {
                //EntryPoint.WriteToConsoleTestLong($"CLOSE DOOR {doorIndex}");
                toToggleDoor.Vehicle.Doors[doorIndex].Close(false);
            }
            else
            {
                //EntryPoint.WriteToConsoleTestLong($"OPEN DOOR {doorIndex}");
                toToggleDoor.Vehicle.Doors[doorIndex].Open(false, false);
            }
        }
    }
    public bool SetDoor(int doorIndex, bool setOpen, bool includeWarning, VehicleExt mainVehicle) 
    {
        VehicleExt toToggleDoor = null;
        if (mainVehicle != null)
        {
            toToggleDoor = mainVehicle;
        }
        else
        {
            toToggleDoor = GetInterestedVehicle();
        }
        if (toToggleDoor == null || !toToggleDoor.Vehicle.Exists())
        {
            return false;
        }
        if (IsPerformingActivity)
        {
            if (includeWarning)
            {
                Game.DisplayHelp("Cancel existing activity to start");
            }
            return false;
        }
        DoorToggle doorToggle = new DoorToggle(Actionable, Settings, World, toToggleDoor, doorIndex, true,setOpen);
        if (doorToggle.CanPerform(Actionable))
        {
            ForceCancelAllActive();
            IsPerformingActivity = true;
            LowerBodyActivity = doorToggle;
            LowerBodyActivity.Start();
        }
        return true;
    }
    public bool SetDoor_Old(int doorIndex, bool withAnimation, bool setOpen, bool includeWarning)
    {
        VehicleExt toToggleDoor = GetInterestedVehicle();
        if (toToggleDoor == null || !toToggleDoor.Vehicle.Exists())
        {
            return false;
        }
        bool isAlreadyOpen = toToggleDoor.Vehicle.Doors[doorIndex].IsOpen;

        if (setOpen)
        {
            if (!isAlreadyOpen)
            {
                //EntryPoint.WriteToConsoleTestLong($"OPEN DOOR {doorIndex}");
                if (withAnimation)
                {

                    if (IsPerformingActivity)
                    {
                        if (includeWarning)
                        {
                            Game.DisplayHelp("Cancel existing activity to start");
                        }
                        return false;
                    }
                    DoorToggle doorToggle = new DoorToggle(Actionable, Settings, World, toToggleDoor, doorIndex, false, false);
                    if (doorToggle.CanPerform(Actionable))
                    {
                        ForceCancelAllActive();
                        IsPerformingActivity = true;
                        LowerBodyActivity = doorToggle;
                        LowerBodyActivity.Start();
                    }
                }
                else
                {
                    toToggleDoor.Vehicle.Doors[doorIndex].Open(false, false);
                }
            }
        }
        else
        {
            if (isAlreadyOpen)
            {
                //EntryPoint.WriteToConsoleTestLong($"CLOSE DOOR {doorIndex}");
                if (withAnimation)
                {


                    if (IsPerformingActivity)
                    {
                        Game.DisplayHelp("Cancel existing activity to start");
                        return false;
                    }
                    DoorToggle doorToggle = new DoorToggle(Actionable, Settings, World, toToggleDoor, doorIndex, false, false);
                    if (doorToggle.CanPerform(Actionable))
                    {
                        ForceCancelAllActive();
                        IsPerformingActivity = true;
                        LowerBodyActivity = doorToggle;
                        LowerBodyActivity.Start();
                    }


                }
                else
                {
                    toToggleDoor.Vehicle.Doors[doorIndex].Close(false);
                }
            }
        }
        return true;
    }
    private void WatchVehicleEntry(VehicleExt toEnter, bool stopDriver)
    {
        if(toEnter == null || !toEnter.Vehicle.Exists())
        {
            IsEnteringAsPassenger = false;
            return;
        }
        GameFiber DoorWatcher = GameFiber.StartNew(delegate
        {
            try
            {
                uint GameTimeStarted = Game.GameTime;
                //EntryPoint.WriteToConsoleTestLong("WatchVehicleEntry START");

                PedExt driverPed = null;// World.Pedestrians.GetPedExt(toEnter.Vehicle.Driver.Handle);

                if (stopDriver && toEnter != null && toEnter.Vehicle.Exists() && toEnter.Vehicle.Driver.Exists())
                {
                    //toEnter.Vehicle.Driver.BlockPermanentEvents = true;
                    driverPed = World.Pedestrians.GetPedExt(toEnter.Vehicle.Driver.Handle); 
                    driverPed?.SetPersistent();
                    NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", toEnter.Vehicle.Driver, toEnter.Vehicle, 6, 9999);
                }
                bool isCancelled = false;
                uint GameTimeStartedGettingIntoVehicle = 0;
                //string modelName = toEnter.Vehicle.Model.Name.ToLower();
                EntryPoint.WriteToConsole($"WATCH VEHICLE ENTRY RAN! FOR {toEnter.VehicleModelName}");
                if (toEnter.HasSpecialPassengerEntry)//  modelName == "0xd227bdbb" || modelName == "caddy3")
                {
                    NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                    while (EntryPoint.ModController.IsRunning && Game.GameTime - GameTimeStarted <= 20000)
                    {
                        if (Player.IsMoveControlPressed || toEnter == null || !toEnter.Vehicle.Exists() || toEnter.Vehicle.Speed >= 0.5f)
                        {
                            isCancelled = true;
                            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                            break;
                        }
                        NativeFunction.Natives.SET_CONTROL_VALUE_NEXT_FRAME<bool>(0, (int)GameControl.Enter, 1.0f);
                        EntryPoint.WriteToConsole("SPECIAL PASSENGER ENTRY WATCHED SETTING VALUE");
                        GameFiber.Yield();
                    }
                }
                else
                {
                    while (EntryPoint.ModController.IsRunning && Game.GameTime - GameTimeStarted <= 20000)
                    {
                        int taskStatus = NativeFunction.Natives.GET_SCRIPT_TASK_STATUS<int>(Player.Character, Game.GetHashKey("SCRIPT_TASK_ENTER_VEHICLE"));
                        //Game.DisplaySubtitle($"taskStatus {taskStatus}");
                        if (taskStatus != 1)
                        {
                            //isCancelled = true;
                            break;
                        }
                        if (Player.IsMoveControlPressed || toEnter == null || !toEnter.Vehicle.Exists() || toEnter.Vehicle.Speed >= 0.5f)
                        {
                            isCancelled = true;
                            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                            break;
                        }
                        GameFiber.Yield();
                    }
                }
                if (isCancelled && stopDriver && toEnter != null && toEnter.Vehicle.Exists() && toEnter.Vehicle.Driver.Exists())
                {
                    //toEnter.Vehicle.Driver.BlockPermanentEvents = false;
                    driverPed?.SetNonPersistent();
                    NativeFunction.Natives.CLEAR_PED_TASKS(toEnter.Vehicle.Driver);
                }
                IsEnteringAsPassenger = false;
                //EntryPoint.WriteToConsoleTestLong("WatchVehicleEntry END");
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
            }
        }, "DoorWatcher");
    }
    public VehicleExt GetInterestedVehicle()
    {
        VehicleExt toToggleDoor = Player.CurrentLookedAtVehicle;
        if (toToggleDoor == null || !toToggleDoor.Vehicle.Exists())
        {
            toToggleDoor = World.Vehicles.GetClosestVehicleExt(Player.Character.Position, false, 10f);
        }
        return toToggleDoor;
    }


    public void PlaySpecificFacialAnimations(string Animation)
    {
        if (Player.IsMale)
        {
            if (Player.ModelName.ToLower() == "player_zero")
            {
                NativeFunction.Natives.PLAY_FACIAL_ANIM(Player.Character, Animation, "facials@p_m_zero@base");
            }
            else if (Player.ModelName.ToLower() == "player_one")
            {
                NativeFunction.Natives.PLAY_FACIAL_ANIM(Player.Character, Animation, "facials@p_m_one@base");
            }
            else if (Player.ModelName.ToLower() == "player_two")
            {
                NativeFunction.Natives.PLAY_FACIAL_ANIM(Player.Character, Animation, "facials@p_m_two@base");
            }
            else
            {
                NativeFunction.Natives.PLAY_FACIAL_ANIM(Player.Character, Animation, "facials@gen_male@base");
            }
        }
        else
        {
            NativeFunction.Natives.PLAY_FACIAL_ANIM(Player.Character, Animation, "facials@gen_female@base");
        }
    }
    public void YellInPain()
    {
        if (CanYell)
        {
            if (RandomItems.RandomPercent(80))
            {
                List<int> PossibleYells = new List<int>() { 8 };
                int YellType = PossibleYells.PickRandom();
                NativeFunction.Natives.PLAY_PAIN(Player.Character, YellType, 0, 0);

                List<string> PossibleAnimations = new List<string>() { "pain_6","pain_5","pain_4","pain_3","pain_2","pain_1",
      "electrocuted_1",
      "burning_1" };
                string Animation = PossibleAnimations.PickRandom();
                if (Player.IsMale)
                {
                    if (Player.ModelName.ToLower() == "player_zero")
                    {
                        NativeFunction.Natives.PLAY_FACIAL_ANIM(Player.Character, Animation, "facials@p_m_zero@base");
                    }
                    else if (Player.ModelName.ToLower() == "player_one")
                    {
                        NativeFunction.Natives.PLAY_FACIAL_ANIM(Player.Character, Animation, "facials@p_m_one@base");
                    }
                    else if (Player.ModelName.ToLower() == "player_two")
                    {
                        NativeFunction.Natives.PLAY_FACIAL_ANIM(Player.Character, Animation, "facials@p_m_two@base");
                    }
                    else
                    {
                        NativeFunction.Natives.PLAY_FACIAL_ANIM(Player.Character, Animation, "facials@gen_male@base");
                    }
                }
                else
                {
                    NativeFunction.Natives.PLAY_FACIAL_ANIM(Player.Character, Animation, "facials@gen_female@base");
                }
                //EntryPoint.WriteToConsoleTestLong($"PLAYER YELL IN PAIN {Player.Character.Handle} YellType {YellType} Animation {Animation}");
            }
            else
            {
                Player.PlaySpeech("GENERIC_FRIGHTENED_HIGH", false);
                //EntryPoint.WriteToConsoleTestLong($"PLAYER CRY SPEECH FOR PAIN {Player.Character.Handle}");
            }

            GameTimeLastYelled = Game.GameTime;
        }
    }
    //Vehicle Stuff
    public void ShuffleToNextSeat()
    {
        if (Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() && Player.IsInVehicle && Player.Character.IsInAnyVehicle(false) && Player.Character.SeatIndex != -1 && NativeFunction.Natives.CAN_SHUFFLE_SEAT<bool>(Player.CurrentVehicle.Vehicle, true))
        {
            NativeFunction.Natives.TASK_SHUFFLE_TO_NEXT_VEHICLE_SEAT(Player.Character, Player.CurrentVehicle.Vehicle, 0);
        }
    }
    //public void StartHotwiring()
    //{
    //    if (Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() && Player.CurrentVehicle.IsHotWireLocked)
    //    {
    //        Player.CurrentVehicle.IsHotWireLocked = false;
    //        Player.CurrentVehicle.Vehicle.MustBeHotwired = true;
    //    }
    //}
    public void ForceErraticDriver()
    {
        if (Player.IsInVehicle && !Player.IsDriver && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
        {
            Ped Driver = Player.CurrentVehicle.Vehicle.Driver;
            if (Driver.Exists() && Driver.Handle != Player.Character.Handle)
            {
                PedExt DriverExt = World.Pedestrians.GetPedExt(Driver.Handle);
                Driver.BlockPermanentEvents = true;
                Driver.KeepTasks = true;
                if (DriverExt != null)
                {
                    DriverExt.CanBeAmbientTasked = false;
                    DriverExt.WillCallPolice = false;
                    DriverExt.WillCallPoliceIntense = false;
                    DriverExt.WillFight = false;
                    DriverExt.WillFightPolice = false;
                    DriverExt.WillAlwaysFightPolice = false;
                    DriverExt.CanBeTasked = false;
                }
                NativeFunction.Natives.SET_DRIVER_ABILITY(Driver, 100f);

                unsafe
                {
                    int lol = 0;
                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                    NativeFunction.CallByName<bool>("TASK_VEHICLE_MISSION_COORS_TARGET", 0, Player.CurrentVehicle.Vehicle, 358.9726f, -1582.881f, 29.29195f, 8, 50f, (int)eCustomDrivingStyles.Code3, 0f, 2f, true);//8f
                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Driver, lol);
                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                }

                //unsafe
                //{
                //    int lol = 0;
                //    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                //    //NativeFunction.CallByName<bool>("TASK_ENTER_VEHICLE", 0, CurrentVehicle.Vehicle, -1, -1, 15.0f, 9);
                //    NativeFunction.CallByName<bool>("TASK_SMART_FLEE_COORD", 0, Position.X,Position.Y,Position.Z,5000f,-1, false, false);

                //    //NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, CurrentVehicle.Vehicle, 25f, (int)eCustomDrivingStyles.FastEmergency, 25f);
                //    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                //    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                //    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Driver, lol);
                //    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                //}
            }
        }
    }

    public void ToggleDriverDoor()
    {
        if (Game.GameTime - GameTimeLastClosedDoor < 1500)
        {
            return;
        }
        if (Player.CurrentVehicle == null || !Player.CurrentVehicle.Vehicle.Exists())
        {
            return;
        }
        if(Player.CurrentVehicle.IsMotorcycle || Player.CurrentVehicle.IsBicycle || Player.CurrentVehicle.IsBoat)
        {
            return;
        }
        if (!Player.IsDriver)
        {
            Game.DisplayHelp("Cannot toggle driver door from current seat");
            return;
        }
        bool isValid = NativeFunction.Natives.x645F4B6E8499F632<bool>(Player.CurrentVehicle.Vehicle, 0);
        if (!isValid)
        {
            return;
        }
        string animName = "d_close_in";
        float doorAngle = Player.CurrentVehicle.Doors.GetDoorAngle(0);

        if(doorAngle <= -1.0f)//Invalid door or something
        {
            return;
        }

        int TimeToWait = 250;
        if(doorAngle <= 0.0f)
        {
            animName = "d_open_out";
            TimeToWait = 500;
        }
        else if (doorAngle >= 0.7)
        {
            animName = "d_close_in";
            TimeToWait = 500;
        }
        else
        {
            animName = "d_close_in_near";
        }
        EntryPoint.WriteToConsole($"doorAngle {doorAngle} animName{animName} TimeToWait{TimeToWait}");
        if (IsPerformingActivity || !Settings.SettingsManager.VehicleSettings.PlayControlAnimations)
        {
            Player.CurrentVehicle?.Doors.Toggle(0, Player);
        }
        else
        {
            DoSimpleVehicleAnimation(new Action(() => Player.CurrentVehicle?.Doors.Toggle(0, Player)), "veh@std@ds@enter_exit", animName, TimeToWait);
        }
    }
    public void CloseDriverDoor()
    {
        if (Game.GameTime - GameTimeLastClosedDoor < 1500)
        {
            return;
        }
        if(Player.CurrentVehicle == null || !Player.CurrentVehicle.Vehicle.Exists())
        {
            return;
        }
        if (Player.CurrentVehicle.IsMotorcycle || Player.CurrentVehicle.IsBicycle || Player.CurrentVehicle.IsBoat)
        {
            return;
        }
        if (!Player.IsDriver)
        {
            Game.DisplayHelp("Cannot close driver door from current seat");
            return;
        }
        bool isValid = NativeFunction.Natives.x645F4B6E8499F632<bool>(Player.CurrentVehicle.Vehicle, 0);
        if (!isValid)
        {
            return;
        }
        string animName = "d_close_in";
        float doorAngle = Player.CurrentVehicle.Doors.GetDoorAngle(0);

        if (doorAngle <= -1.0f)//Invalid door or something
        {
            return;
        }
        int TimeToWait = 250;
        if (doorAngle >= 0.7)
        {
            animName = "d_close_in";
            TimeToWait = 500;
        }
        else
        {
            animName = "d_close_in_near";
        }
        if (IsPerformingActivity || !Settings.SettingsManager.VehicleSettings.PlayControlAnimations)
        {
            Player.CurrentVehicle?.Doors.SetState(0, true, Player);
        }
        else
        {
            DoSimpleVehicleAnimation(new Action(() => Player.CurrentVehicle?.Doors.SetState(0, true, Player)), "veh@std@ds@enter_exit", animName, TimeToWait);
        }
    }
    public void ToggleLeftIndicator()
    {
        if (Player.CurrentVehicle == null)
        {
            return;
        }
        if (!Player.IsDriver)
        {
            Game.DisplayHelp("Cannot toggle indicators from current seat");
            return;
        }
        if (IsPerformingActivity || !Settings.SettingsManager.VehicleSettings.PlayControlAnimations)
        {
            Player.CurrentVehicle?.Indicators.ToggleLeft();
        }
        else
        {
            DoSimpleVehicleAnimation(new Action(() => Player.CurrentVehicle?.Indicators.ToggleLeft()), "veh@std@ds@base", "change_station", 750);
        }
    }
    public void ToggleHazards()
    {
        if (Player.CurrentVehicle == null)
        {
            return;
        }
        if (!Player.IsDriver)
        {
            Game.DisplayHelp("Cannot toggle hazards from current seat");
            return;
        }
        if (IsPerformingActivity || !Settings.SettingsManager.VehicleSettings.PlayControlAnimations)
        {
            Player.CurrentVehicle?.Indicators.ToggleHazards();
        }
        else
        {
            DoSimpleVehicleAnimation(new Action(() => Player.CurrentVehicle?.Indicators.ToggleHazards()), "veh@std@ds@base", "change_station", 750);
        }
    }
    public void ToggleRightIndicator()
    {
        if (Player.CurrentVehicle == null)
        {
            return;
        }
        if (!Player.IsDriver)
        {
            Game.DisplayHelp("Cannot toggle indicators from current seat");
            return;
        }
        if (IsPerformingActivity || !Settings.SettingsManager.VehicleSettings.PlayControlAnimations)
        {
            Player.CurrentVehicle?.Indicators.ToggleRight();
        }
        else
        {
            DoSimpleVehicleAnimation(new Action(() => Player.CurrentVehicle?.Indicators.ToggleRight()), "veh@std@ds@base", "change_station", 750);
        }
    }
    public void ToggleVehicleEngine()
    {
        if (Player.CurrentVehicle == null)
        {
            return;
        }
        if (Player.CurrentVehicle.IsHotWireLocked)
        {
            return;
        }
        if (!Player.IsDriver)
        {
            Game.DisplayHelp("Cannot change engine status from current seat");
            return;
        }
        if (IsPerformingActivity || !Settings.SettingsManager.VehicleSettings.PlayControlAnimations)
        {
            Player.CurrentVehicle?.Engine.Toggle();
        }
        else
        {
            DoSimpleVehicleAnimation(new Action(() => Player.CurrentVehicle?.Engine.Toggle()), "veh@std@ds@base", "start_engine", 750);
        }
    }
    public void SetVehicleEngine(bool desiredStatus)
    {
        if (Player.CurrentVehicle == null)
        {
            return;
        }
        if(Player.CurrentVehicle.IsHotWireLocked)
        {
            return;
        }
        if (!Player.IsDriver)
        {
            Game.DisplayHelp("Cannot change engine status from current seat");
            return;
        }
        if (IsPerformingActivity || !Settings.SettingsManager.VehicleSettings.PlayControlAnimations)
        {
            Player.CurrentVehicle?.Engine.SetState(desiredStatus);
        }
        else
        {
            DoSimpleVehicleAnimation(new Action(() => Player.CurrentVehicle?.Engine.SetState(desiredStatus)), "veh@std@ds@base", "start_engine", 750);
        }
    }
    public void ToggleDriverWindow()
    {
        if (Player.CurrentVehicle == null)
        {
            return;
        }
        if (Player.CurrentVehicle.IsMotorcycle || Player.CurrentVehicle.IsBicycle || Player.CurrentVehicle.IsBoat)
        {
            return;
        }
        if (!Player.CurrentVehicle.Engine.IsRunning)
        {
            Game.DisplayHelp("Engine must be running to control windows");
            return;
        }
        if (!Player.IsDriver)
        {
            Game.DisplayHelp("Cannot control driver window from current seat");
            return;
        }
        if (IsPerformingActivity || !Settings.SettingsManager.VehicleSettings.PlayControlAnimations)
        {
            Player.CurrentVehicle?.Windows.ToggleWindow(0);
        }
        else
        {
            DoSimpleVehicleAnimation(new Action(() => Player.CurrentVehicle?.Windows.ToggleWindow(0)), "veh@std@ds@enter_exit", "d_close_in_near", 750);
        }
    }
    public void ToggleWindowState(int windowID)
    {
       
        if (Player.CurrentVehicle == null)
        {
            return;
        }
        if (Player.CurrentVehicle.IsMotorcycle || Player.CurrentVehicle.IsBicycle || Player.CurrentVehicle.IsBoat)
        {
            return;
        }
        if (!Player.CurrentVehicle.Engine.IsRunning)
        {
            Game.DisplayHelp("Engine must be running to control windows");
            return;
        }
        if (!Player.IsDriver)
        {
            if (!CanControlWindowFromSeat(windowID, Player.CurrentSeat))
            {
                Game.DisplayHelp("Cannot control window from current seat");
                return;
            }
        }
        if (IsPerformingActivity || !Settings.SettingsManager.VehicleSettings.PlayControlAnimations)
        {
            Player.CurrentVehicle?.Windows.ToggleWindow(windowID);
        }
        else
        {
            DoSimpleVehicleAnimation(new Action(() => Player.CurrentVehicle?.Windows.ToggleWindow(windowID)), "veh@std@ds@enter_exit", "d_close_in_near", 750);
        }


    }

    public void SetWindowState(int windowID, bool desiredStatus)
    {
        if (Player.CurrentVehicle == null)// || (!Player.IsDriver || )
        {
            return;
        }
        if(!Player.CurrentVehicle.Engine.IsRunning)
        {
            Game.DisplayHelp("Engine must be running to control windows");
            return;
        }
        if (Player.CurrentVehicle.IsMotorcycle || Player.CurrentVehicle.IsBicycle || Player.CurrentVehicle.IsBoat)
        {
            return;
        }
        if (!Player.IsDriver)
        {
            if(!CanControlWindowFromSeat(windowID, Player.CurrentSeat))
            {
                Game.DisplayHelp("Cannot control window from current seat");
                return;
            }
        }
        if (IsPerformingActivity || !Settings.SettingsManager.VehicleSettings.PlayControlAnimations)
        {
            Player.CurrentVehicle?.Windows.SetState(windowID, desiredStatus);
        }
        else
        {
            DoSimpleVehicleAnimation(new Action(() => Player.CurrentVehicle?.Windows.SetState(windowID, desiredStatus)), "veh@std@ds@enter_exit", "d_close_in_near", 750);
        }
    }

    private bool CanControlWindowFromSeat(int windowID, int seatID)
    {
        if (seatID == -1 || seatID == 0)//driver or passenger can control ALL
        {
            return true;
        }
        else if (seatID == 1)//rear driver
        {
            return windowID == 2;
        }
        else if (seatID == 2)//rear passenger
        {
            return windowID == 3;
        }
        return false;
    }


    public void ToggleDoorLocks()
    {
        if (Player.CurrentVehicle == null)
        {
            return;
        }
        if (!Player.IsDriver)
        {
            Game.DisplayHelp("Cannot change door locks from current seat");
            return;
        }
        if (IsPerformingActivity || !Settings.SettingsManager.VehicleSettings.PlayControlAnimations)
        {
            Player.CurrentVehicle?.Doors.ToggleDoorLocks();
        }
        else
        {
            DoSimpleVehicleAnimation(new Action(() => Player.CurrentVehicle?.Doors.ToggleDoorLocks()), "veh@std@ds@enter_exit", "d_close_in_near", 750);
        }
    }
    private void DoSimpleVehicleAnimation(Action action, string dictionary, string anim, int timeToWait)
    {
        if(Player.CurrentVehicle == null || !Player.CurrentVehicle.Vehicle.Exists())
        {
            return;
        }
        if(Player.CurrentVehicle.IsHotWireLocked)
        {
            return;
        }
        if(Player.CurrentVehicle.Vehicle.MustBeHotwired)
        {
            return;
        }
        if(!Player.CurrentVehicle.UsePlayerAnimations)
        {
            action();
            return;
        }
        if(IsDoingVehileAnim)
        {
            return;
        }
        AnimationDictionary.RequestAnimationDictionay(dictionary);
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, dictionary, anim, 4.0f, -4.0f, -1, (int)(eAnimationFlags.AF_UPPERBODY | eAnimationFlags.AF_SECONDARY), 0, false, false, false);//-1
        GameFiber animWatcher = GameFiber.StartNew(delegate
        {
            try
            {
                IsDoingVehileAnim = true;
                bool IsFinished = false;
                uint GameTimeStarted = Game.GameTime;
                bool performedAction = false;
                while (!IsFinished)
                {
                    if(!performedAction && Game.GameTime - GameTimeStarted >= timeToWait)
                    {
                        performedAction = true;
                        action();
                    }
                    if(Game.GameTime - GameTimeStarted >= timeToWait + 1000)
                    {
                        IsFinished = true;
                    }
                    if(!Player.Character.IsInAnyVehicle(false))
                    {
                        IsFinished = true;
                    }
                    if(Game.IsControlJustPressed(0,GameControl.VehicleExit))
                    {
                        IsFinished = true;
                    }
                    GameFiber.Yield();
                }
                NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
                IsDoingVehileAnim = false;
                // GameFiber.Sleep(timeToWait);

                //GameFiber.Sleep(1000);

            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "VehicleAnimationWatcher");
    }

    public void OnPlayerBusted()
    {
        ForceCancelAllActivities();
    }
    public void OnPlayerDied()
    {
        ForceCancelAllActivities();
    }
    public void PerformItemAnimation(ModItem modItem, bool isTaking)
    {
        //throw new NotImplementedException();
        if (IsPerformingActivity)
        {
            return;
        }
        if(modItem == null)
        {
            return;
        }
        modItem.PerformItemAnimation(Player, isTaking);
        //AnimationDictionary.RequestAnimationDictionay("mp_common");
        //string animation = isTaking ? "givetake1_b" : "givetake1_a";
       // NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "mp_common", animation, 1.0f, -1.0f, 5000, (int)(AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask), 0, false, false, false);
    }
    public void PerformCashAnimation(bool isTaking)
    {
        //prop_cash_pile_02.ydr

        if (IsPerformingActivity)
        {
            return;
        }

        ModItem cashItem = ModItems.Get("Cash Bundle");
        if(cashItem == null)
        {
            return;
        }
        cashItem.PerformItemAnimation(Player, isTaking);
    }
    public Rage.Object AttachScrewdriverToPed(ModItem screwdriverItem, bool allowGeneric)
    {
        Rage.Object Screwdriver = null;
        try
        {
            ModItem ScrewDriverItem = screwdriverItem;
            if(ScrewDriverItem == null)
            {
                ScrewDriverItem = Player.Inventory.Get(typeof(ScrewdriverItem))?.ModItem;
            }
            if (ScrewDriverItem == null && allowGeneric)
            {
                ScrewDriverItem = ModItems.PossibleItems.ScrewdriverItems.FirstOrDefault();
            }
            Screwdriver = new Rage.Object(ScrewDriverItem.ModelItem.ModelName, Player.Character.GetOffsetPositionUp(50f));
            if (Screwdriver.Exists())
            {
                PropAttachment pa = ScrewDriverItem.ModelItem.Attachments.FirstOrDefault(x => x.Name == "RightHand");
                if (pa != null)
                {
                    Screwdriver.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, pa.BoneName), pa.Attachment, pa.Rotation);
                }
                else
                {
                    Screwdriver.Delete();
                }
            }
            return Screwdriver;
        }
        catch (Exception ex)
        {
            return Screwdriver;
        }
    }
    public void EnterVehicleGeneric()
    {
        NativeFunction.Natives.SET_CONTROL_VALUE_NEXT_FRAME<bool>(0, (int)GameControl.Enter, 1.0f);
    }
    public void LeaveVehicle(bool isRegular)
    {
        if(!Player.Character.CurrentVehicle.Exists())
        {
            return;
        }
        WatchVehicleLeave(Player.Character.CurrentVehicle, true);
    }
    private void WatchVehicleLeave(Vehicle vehicle, bool stopDriver)
    {
        if (!vehicle.Exists())
        {
            return;
        }
        GameFiber DoorWatcher = GameFiber.StartNew(delegate
        {
            try
            {
                EntryPoint.WriteToConsole("WATCH VEHICLE LEAVE STARTED");
                int flags = (int)eEnter_Exit_Vehicle_Flags.ECF_RESUME_IF_INTERRUPTED | (int)eEnter_Exit_Vehicle_Flags.ECF_WAIT_FOR_ENTRY_POINT_TO_BE_CLEAR;
                uint GameTimeStarted = Game.GameTime;
                if (stopDriver && vehicle.Exists() && vehicle.Driver.Exists())
                {
                    //vehicle.Driver.BlockPermanentEvents = true;
                    EntryPoint.WriteToConsole("WATCH VEHICLE RUNNING TEMP ACTION");
                    NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", vehicle.Driver, vehicle, 6, 99999);
                }
                while (EntryPoint.ModController.IsRunning && Game.GameTime - GameTimeStarted <= 20000)
                {
                    if(vehicle.Exists() && vehicle.Speed <= 0.5f)
                    {
                        break;
                    }
                    GameFiber.Yield();
                }
                if (stopDriver && vehicle.Exists() && vehicle.Driver.Exists())
                {
                    //vehicle.Driver.BlockPermanentEvents = false;
                    NativeFunction.Natives.CLEAR_PED_TASKS(vehicle.Driver);
                }
                if (!Player.Character.CurrentVehicle.Exists())
                {
                    EntryPoint.WriteToConsole("WATCH VEHICLE LEAVE NO VEHICLE");
                    return;
                }
                NativeFunction.Natives.TASK_LEAVE_VEHICLE(Player.Character, Player.Character.CurrentVehicle, flags);
                EntryPoint.WriteToConsole("WATCH VEHICLE LEAVE LEAVING");
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
            }
        }, "DoorWatcher");
    }
    public void DebugPlayVehicleAnim(string dictionaryName, string animName)
    {
        DoSimpleVehicleAnimation(new Action(() => Player.CurrentVehicle?.Windows.ToggleWindow(0)), dictionaryName, animName, 750);
    }

    public void HotwireVehicle()
    {
        if(Player.CurrentVehicle == null || !Player.CurrentVehicle.IsHotWireLocked)
        {
            return;
        }
        bool currentlyHasScrewdriver = HasScrewdriverInHand || Player.Inventory.Has(typeof(ScrewdriverItem)); //Inventory.HasTool(ToolTypes.Screwdriver);
        if (Settings.SettingsManager.VehicleSettings.RequireScrewdriverForHotwire &&  !currentlyHasScrewdriver)
        {
            Game.DisplayHelp("Screwdriver required to hotwire");
            return;
        }
        Player.CurrentVehicle.IsHotWireLocked = false;
        GameTimeLastStartedHotwiring = Game.GameTime;
        if(!Player.CurrentVehicle.Vehicle.Exists())
        {
            return;
        }
        Player.CurrentVehicle.Vehicle.MustBeHotwired = true;
        Player.CurrentVehicle.Engine.SetState(true);
        //GameFiber.Sleep(2500);
        //Player.CurrentVehicle?.Engine.Synchronize();
    }
}


