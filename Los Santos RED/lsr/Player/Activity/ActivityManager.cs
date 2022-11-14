using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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

    private DynamicActivity LowerBodyActivity;
    private DynamicActivity UpperBodyActivity;
    private ICameraControllable CameraControllable;


    public bool IsUsingToolAsWeapon { get; set; }

    public bool CanUseItemsBase => !Player.IsIncapacitated && Player.IsAliveAndFree && !Player.IsGettingIntoAVehicle && !IsHoldingHostage && !Player.RecentlyGotOutOfVehicle && !IsLootingBody;
    public bool CanPerformActivities => (!Player.IsMovingFast || Player.IsInVehicle) && !Player.IsIncapacitated && Player.IsAliveAndFree && !Player.IsGettingIntoAVehicle && !Player.IsMovingDynamically && !IsHoldingHostage && !Player.RecentlyGotOutOfVehicle;
    public bool CanPerformMobileActivities => !Player.IsInVehicle && !Player.IsIncapacitated && Player.IsAliveAndFree && !Player.IsGettingIntoAVehicle && !IsHoldingHostage && !Player.RecentlyGotOutOfVehicle && !IsLootingBody;








    public bool CanConverse => !Player.IsIncapacitated && !Player.IsVisiblyArmed && Player.IsAliveAndFree && !Player.IsMovingDynamically && ((Player.IsInVehicle && Player.VehicleSpeedMPH <= 5f) || !Player.IsMovingFast) && !IsLootingBody && !IsDraggingBody && !IsHoldingHostage && !IsDancing;
    public bool CanConverseWithLookedAtPed => Player.CurrentLookedAtPed != null && Player.CurrentTargetedPed == null && Player.CurrentLookedAtPed.CanConverse && !Player.RelationshipManager.GangRelationships.IsHostile(Player.CurrentLookedAtGangMember?.Gang) && (!Player.CurrentLookedAtPed.IsCop || (Player.IsNotWanted && !Player.Investigation.IsActive)) && CanConverse;
    public bool CanTakeHostageWithLookedAtPed => Player.CurrentLookedAtPed != null && Player.CurrentTargetedPed == null && CanTakeHostage && !Player.CurrentLookedAtPed.IsInVehicle && !Player.CurrentLookedAtPed.IsUnconscious && !Player.CurrentLookedAtPed.IsDead && Player.CurrentLookedAtPed.DistanceToPlayer <= 3.0f && Player.CurrentLookedAtPed.Pedestrian.Exists() && Player.CurrentLookedAtPed.Pedestrian.IsThisPedInFrontOf(Player.Character) && !Player.Character.IsThisPedInFrontOf(Player.CurrentLookedAtPed.Pedestrian);
    public bool CanTakeHostage => !Player.IsCop && !Player.IsInVehicle && !Player.IsIncapacitated && !IsLootingBody && !IsDancing && !IsHoldingHostage && Player.WeaponEquipment.CurrentWeapon != null && Player.WeaponEquipment.CurrentWeapon.CanPistolSuicide;
    public bool CanHoldUpTargettedPed => Player.CurrentTargetedPed != null && !Player.IsCop && Player.CurrentTargetedPed.CanBeMugged && Player.IsAliveAndFree && !Player.IsIncapacitated && !Player.IsGettingIntoAVehicle && !Player.IsBreakingIntoCar && Player.IsVisiblyArmed && Player.CurrentTargetedPed.DistanceToPlayer <= 15f;
    public bool CanLoot => !Player.IsCop && !Player.IsInVehicle && !Player.IsIncapacitated && !Player.IsMovingDynamically && !IsLootingBody && !IsHoldingHostage && !IsDraggingBody && !IsConversing && !IsDancing;
    public bool CanLootLookedAtPed => Player.CurrentLookedAtPed != null && Player.CurrentTargetedPed == null && CanLoot && !Player.CurrentLookedAtPed.HasBeenLooted && !Player.CurrentLookedAtPed.IsInVehicle && (Player.CurrentLookedAtPed.IsUnconscious || Player.CurrentLookedAtPed.IsDead);
    public bool CanDrag => !Player.IsInVehicle && !Player.IsIncapacitated && !Player.IsMovingDynamically && !IsLootingBody && !IsDraggingBody && !IsHoldingHostage && !IsDancing;
    public bool CanDragLookedAtPed => Player.CurrentLookedAtPed != null && Player.CurrentTargetedPed == null && CanDrag && !Player.CurrentLookedAtPed.IsInVehicle && (Player.CurrentLookedAtPed.IsUnconscious || Player.CurrentLookedAtPed.IsDead);
    public bool CanRecruitLookedAtGangMember => Player.CurrentLookedAtGangMember != null && Player.CurrentTargetedPed == null && Player.RelationshipManager.GangRelationships.CurrentGang != null && Player.CurrentLookedAtGangMember.Gang != null && Player.RelationshipManager.GangRelationships.CurrentGang.ID == Player.CurrentLookedAtGangMember.Gang.ID && !Player.GroupManager.IsMember(Player.CurrentLookedAtGangMember);
    public string ContinueCurrentActivityPrompt => UpperBodyActivity != null ? UpperBodyActivity.ContinuePrompt : LowerBodyActivity != null ? LowerBodyActivity.ContinuePrompt : "";
    public string CancelCurrentActivityPrompt => UpperBodyActivity != null ? UpperBodyActivity.CancelPrompt : LowerBodyActivity != null ? LowerBodyActivity.CancelPrompt : "";
    public string PauseCurrentActivityPrompt => UpperBodyActivity != null ? UpperBodyActivity.PausePrompt : LowerBodyActivity != null ? LowerBodyActivity.PausePrompt : "";
    public bool CanCancelCurrentActivity => UpperBodyActivity?.CanCancel == true || LowerBodyActivity?.CanCancel == true;
    public bool CanPauseCurrentActivity => UpperBodyActivity?.CanPause == true || LowerBodyActivity?.CanPause == true;
    public bool IsCurrentActivityPaused => UpperBodyActivity?.IsPaused() == true || LowerBodyActivity?.IsPaused() == true;
    public bool HasCurrentActivity => UpperBodyActivity != null || LowerBodyActivity != null;
    public bool IsResting => IsSitting || IsLayingDown;
    public bool IsPerformingActivity { get; set; }
    public bool IsSitting { get; set; }
    public bool IsLayingDown { get; set; } = false;
    public bool IsCommitingSuicide { get; set; }
    public bool IsLootingBody { get; set; }
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
    public ActivityManager(IActivityManageable player, ISettingsProvideable settings, IActionable actionable, IIntoxicatable intoxicatable, IInteractionable interactionable, ICameraControllable cameraControllable, ILocationInteractable locationInteractable,
        ITimeControllable time, IRadioStations radioStations, ICrimes crimes, IModItems modItems, 
        IDances dances, IEntityProvideable world, IIntoxicants intoxicants, IPlateChangeable plateChangeable, ISpeeches speeches, ISeats seats, IWeapons weapons)
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
    }
    public void Setup()
    {
        LastGesture = new GestureData("Thumbs Up Quick", "anim@mp_player_intselfiethumbs_up", "enter");
        LastDance = Dances.GetRandomDance();
    }
    public void Dispose()
    {
        Interaction?.Dispose();
    }
    public void Update()
    {

    }
    public void Reset()
    {
        IsPerformingActivity = false;
    }
    public void ContinueCurrentActivity()
    {
        if (UpperBodyActivity != null && UpperBodyActivity.CanPause && UpperBodyActivity.IsPaused())
        {
            UpperBodyActivity.Continue();
        }
        else if (LowerBodyActivity != null && LowerBodyActivity.CanPause && LowerBodyActivity.IsPaused())
        {
            LowerBodyActivity.Continue();
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
            Player.ButtonPrompts.RemoveActivityPrompts();
            UpperBodyActivity?.Cancel();
            UpperBodyActivity = null;
            IsPerformingActivity = false;
        }
    }
    private void ForceCancelUpperBody()
    {
        if (UpperBodyActivity != null)
        {
            Player.ButtonPrompts.RemoveActivityPrompts();
            UpperBodyActivity.Cancel();
            UpperBodyActivity = null;
        }
    }
    private void ForceCancelAll()
    {
        if (UpperBodyActivity != null)
        {
            Player.ButtonPrompts.RemoveActivityPrompts();
            UpperBodyActivity.Cancel();
            UpperBodyActivity = null;
        }
        if (LowerBodyActivity != null)
        {
            Player.ButtonPrompts.RemoveActivityPrompts();
            LowerBodyActivity.Cancel();
            LowerBodyActivity = null;
        }
    }
    public void StartUpperBodyActivity(DynamicActivity toStart)
    {
        ForceCancelUpperBody();
        IsPerformingActivity = true;
        UpperBodyActivity = toStart;
        UpperBodyActivity.Start();
    }
    public void StartLowerBodyActivity(DynamicActivity toStart)
    {
        ForceCancelUpperBody();
        IsPerformingActivity = true;
        LowerBodyActivity = toStart;
        LowerBodyActivity.Start();
    }
    //Dynamic Activites w/ IsPerforming
    public void RemovePlate()
    {
        if (!IsPerformingActivity && CanPerformActivities && Player.IsOnFoot && !IsResting)
        {
            ForceCancelAll();
            IsPerformingActivity = true;
            UpperBodyActivity = new PlateTheft(Actionable, Settings, World);
            UpperBodyActivity.Start();
        }
    }
    public void CommitSuicide()
    {
        if (!IsPerformingActivity && CanPerformActivities && Player.IsOnFoot && !IsResting)
        {
            ForceCancelUpperBody();
            IsPerformingActivity = true;
            UpperBodyActivity = new SuicideActivity(Actionable, Settings);
            UpperBodyActivity.Start();
        }
    }
    public void Gesture(GestureData gestureData)
    {
        EntryPoint.WriteToConsole($"Gesture Start 2 NO DATA?: {gestureData == null}");
        if (!IsPerformingActivity && CanPerformActivities && Player.IsOnFoot && !IsResting)
        {
            ForceCancelUpperBody();
            IsPerformingActivity = true;
            LastGesture = gestureData;
            UpperBodyActivity = new GestureActivity(Actionable, gestureData);
            UpperBodyActivity.Start();
        }
    }
    public void Gesture()
    {
        Gesture(LastGesture);
    }
    public void Dance(DanceData danceData)
    {
        EntryPoint.WriteToConsole($"Dance Start 2 NO DATA?: {danceData == null}");
        if (!IsPerformingActivity && CanPerformActivities && Player.IsOnFoot && !IsResting)
        {
            ForceCancelAll();
            IsPerformingActivity = true;
            LastDance = danceData;
            UpperBodyActivity = new DanceActivity(Actionable, danceData, RadioStations, Settings, Dances);
            UpperBodyActivity.Start();
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
        if (((!IsPerformingActivity && CanPerformActivities) || !performActivity))// modItem.Type != eConsumableType.None)
        {
            if (performActivity)
            {
                modItem.UseItem(Actionable, Settings, World, CameraControllable, Intoxicants);
            }
            else
            {
                Time.FastForward(Time.CurrentDateTime.AddMinutes(3));
                modItem.ConsumeItem(Actionable, Settings.SettingsManager.NeedsSettings.ApplyNeeds);
            }
        }
    }
    public void StartScenario()
    {
        if (!IsPerformingActivity && CanPerformActivities && Player.IsOnFoot && !IsResting)
        {
            if (UpperBodyActivity != null)
            {
                UpperBodyActivity.Cancel();
            }
            IsPerformingActivity = true;
            UpperBodyActivity = new ScenarioActivity(Intoxicatable);
            UpperBodyActivity.Start();
        }
    }

    //Dynamic Activities W/o Performing
    public void GrabPed()
    {
        if (!IsPerformingActivity && CanPerformActivities && CanTakeHostageWithLookedAtPed && Player.IsOnFoot && !IsResting)
        {
            ForceCancelAll();
            LowerBodyActivity = new HumanShield(Interactionable, Player.CurrentLookedAtPed, Settings, Crimes, ModItems);
            LowerBodyActivity.Start();
        }
    }
    public void LootPed()
    {
        if (!IsPerformingActivity && CanPerformActivities && CanLootLookedAtPed && Player.IsOnFoot && !IsResting)
        {
            ForceCancelAll();
            LowerBodyActivity = new Loot(Interactionable, Player.CurrentLookedAtPed, Settings, Crimes, ModItems);
            LowerBodyActivity.Start();
        }
    }
    public void DragPed()
    {
        if (!IsPerformingActivity && CanPerformActivities && CanDragLookedAtPed && CanDrag && Player.IsOnFoot && !IsResting)
        {
            ForceCancelAll();
            LowerBodyActivity = new Drag(Interactionable, Player.CurrentLookedAtPed, Settings, Crimes, ModItems, World);
            LowerBodyActivity.Start();
        }
    }
    public void StartSleeping()
    {
        if (!IsPerformingActivity && CanPerformActivities && !IsResting)
        {
            if (Player.HumanState.Sleep.IsNearMax)
            {
                Game.DisplayHelp("You are not tired enough to sleep");
            }
            else
            {
                ForceCancelAll();
                LowerBodyActivity = new SleepingActivity(Actionable, Settings);
                LowerBodyActivity.Start();
            }
        }
    }
    public void StartLocationInteraction()
    {
        if (!IsInteracting && !IsInteractingWithLocation)
        {
            if (Interaction != null)
            {
                Interaction.Dispose();
            }
            //if (IsPerformingActivity)
            //{
            //    UpperBodyActivity?.Cancel();
            //}
            ForceCancelUpperBody();//was only if performing
            Player.ClosestInteractableLocation.OnInteract(LocationInteractable, ModItems, World, Settings, Weapons, Time);
        }
    }
    public void StartSittingDown(bool findSittingProp, bool enterForward)
    {
        if (!IsPerformingActivity && CanPerformActivities && Player.IsOnFoot && !IsResting )
        {
            ForceCancelAll();
            LowerBodyActivity = new SittingActivity(Actionable, Settings, findSittingProp, enterForward, Seats, CameraControllable);
            LowerBodyActivity.Start();
        }
    }
    //Interactions
    public void StartConversation()
    {
        if (!IsInteracting && CanConverseWithLookedAtPed)
        {
            if (Interaction != null)
            {
                Interaction.Dispose();
            }
            if (Settings.SettingsManager.ActivitySettings.UseSimpleConversation)
            {
                Interaction = new Conversation_Simple(Interactionable, Player.CurrentLookedAtPed, Settings, Crimes);
                Interaction.Start();
            }
            else
            {
                Interaction = new Conversation(Interactionable, Player.CurrentLookedAtPed, Settings, Crimes, Speeches);
                Interaction.Start();
            }

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
            Interaction = new HoldUp(Interactionable, Player.CurrentTargetedPed, Settings, ModItems);
            Interaction.Start();
        }
    }
    public void OnTargetHandleChanged()
    {
        if (!IsInteracting && Player.IsOnFoot && CanHoldUpTargettedPed && Player.CurrentTargetedPed != null && Player.CurrentTargetedPed.CanBeMugged)//isinvehicle added here
        {
            StartHoldUp();
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
            IsConversing = true;
            Merchant merchant = World.Pedestrians.Merchants.FirstOrDefault(x => x.Handle == Player.CurrentLookedAtPed.Handle);
            if (merchant != null)
            {
                EntryPoint.WriteToConsole("Transaction: 1 Start Ran", 5);
                Interaction = new PersonTransaction(LocationInteractable, merchant, merchant.ShopMenu, ModItems, World, Settings, Weapons, Time) { AssociatedStore = merchant.AssociatedStore };// Settings, ModItems, TimeControllable, World, Weapons); 
                Interaction.Start();
            }
            else
            {
                EntryPoint.WriteToConsole("Transaction: 2 Start Ran", 5);
                Interaction = new PersonTransaction(LocationInteractable, Player.CurrentLookedAtPed, Player.CurrentLookedAtPed.ShopMenu, ModItems, World, Settings, Weapons, Time);// Settings, ModItems, TimeControllable, World, Weapons);
                Interaction.Start();
            }
        }
    }
    //Other
    public void EnterVehicleAsPassenger(bool withBlocking)
    {
        VehicleExt toEnter = World.Vehicles.GetClosestVehicleExt(Player.Character.Position, false, 10f);
        if (toEnter != null && toEnter.Vehicle.Exists())
        {
            int? seatIndex = toEnter.Vehicle.GetFreePassengerSeatIndex();
            if (seatIndex != null)
            {
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
                Player.LastFriendlyVehicle = toEnter.Vehicle;
                NativeFunction.Natives.TASK_ENTER_VEHICLE(Player.Character, toEnter.Vehicle, 5000, seatIndex, 1f, 9);
            }
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
                EntryPoint.WriteToConsole($"PLAYER YELL IN PAIN {Player.Character.Handle} YellType {YellType} Animation {Animation}");
            }
            else
            {
                Player.PlaySpeech("GENERIC_FRIGHTENED_HIGH", false);
                EntryPoint.WriteToConsole($"PLAYER CRY SPEECH FOR PAIN {Player.Character.Handle}");
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
    public void StartHotwiring()
    {
        if (Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists() && Player.CurrentVehicle.IsHotWireLocked)
        {
            Player.CurrentVehicle.IsHotWireLocked = false;
            Player.CurrentVehicle.Vehicle.MustBeHotwired = true;
        }
    }
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
    public void CloseDriverDoor()
    {
        if (Game.GameTime - GameTimeLastClosedDoor >= 1500)
        {
            if (!IsPerformingActivity && Player.IsDriver && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())// Game.LocalPlayer.Character.CurrentVehicle.Exists() && )
            {
                bool isValid = NativeFunction.Natives.x645F4B6E8499F632<bool>(Player.CurrentVehicle.Vehicle, 0);
                if (isValid)
                {
                    float DoorAngle = NativeFunction.Natives.GET_VEHICLE_DOOR_ANGLE_RATIO<float>(Player.CurrentVehicle.Vehicle, 0);

                    if (DoorAngle > 0.0f)
                    {
                        string toPlay = "";
                        int TimeToWait = 250;
                        if (DoorAngle >= 0.7)
                        {
                            toPlay = "d_close_in";
                            TimeToWait = 500;
                        }
                        else
                        {
                            toPlay = "d_close_in_near";
                        }
                        EntryPoint.WriteToConsole($"Player Event: Closing Door Manually Angle {DoorAngle} Dict veh@std@ds@enter_exit Animation {toPlay}", 5);
                        AnimationDictionary.RequestAnimationDictionay("veh@std@ds@enter_exit");
                        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "veh@std@ds@enter_exit", toPlay, 4.0f, -4.0f, -1, 50, 0, false, false, false);//-1
                        GameFiber DoorWatcher = GameFiber.StartNew(delegate
                        {
                            GameFiber.Sleep(TimeToWait);
                            if (Game.LocalPlayer.Character.CurrentVehicle.Exists())
                            {
                                NativeFunction.Natives.SET_VEHICLE_DOOR_SHUT(Game.LocalPlayer.Character.CurrentVehicle, 0, false);
                                GameFiber.Sleep(250);
                                NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
                            }
                            else
                            {
                                NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
                            }
                        }, "DoorWatcher");
                    }
                    GameTimeLastClosedDoor = Game.GameTime;
                }
            }
        }
    }
    public void ToggleLeftIndicator()
    {
        if (Player.CurrentVehicle != null)
        {
            Player.CurrentVehicle.Indicators.ToggleLeft();
        }
    }
    public void ToggleHazards()
    {
        if (Player.CurrentVehicle != null)
        {
            Player.CurrentVehicle.Indicators.ToggleHazards();
        }
    }
    public void ToggleRightIndicator()
    {
        if (Player.CurrentVehicle != null)
        {
            Player.CurrentVehicle.Indicators.ToggleRight();
        }
    }
    public void ToggleVehicleEngine()
    {
        if (Player.CurrentVehicle != null)
        {
            Player.CurrentVehicle.Engine.Toggle();
        }
    }
    public void ToggleDriverWindow()
    {
        if (Player.CurrentVehicle != null)
        {
            Player.CurrentVehicle.SetDriverWindow(!Player.CurrentVehicle.ManuallyRolledDriverWindowDown);
        }
    }
}


